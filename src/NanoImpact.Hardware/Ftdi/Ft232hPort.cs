using System;
using System.Threading;

namespace NanoImpact.Hardware.Ftdi
{
    /// <summary>FT232H로 연결된 보드 3개. 값은 원본 IDX_FT232H_* 인덱스와 같다.</summary>
    public enum Ft232hDevice
    {
        /// <summary>SE 영상 스캔·캡처 보드 (시리얼 "ImgCap001")</summary>
        SemImage = 0,
        /// <summary>BSE 영상 스캔·캡처 보드 (시리얼 "BseICap001")</summary>
        BseImage = 1,
        /// <summary>장비 제어·스캔 보드 Scan Bridge (시리얼 "ScanBri001")</summary>
        ScanBridge = 2,
    }

    /// <summary>
    /// FT232H 보드 하나. 원본 AfxFT232HPortOpen/Close/SendCommand/SendMultiCommand/SendAvrCommand.
    /// 원본은 전역 m_nFT232HIndex를 바꿔 가며 썼다(스레드 경합 가능) — 여기서는 포트마다 잠금을 둔다.
    /// </summary>
    public sealed class Ft232hPort : IDisposable
    {
        /// <summary>송·수신 버퍼 갱신 시간 [ms] (FT232H_LATENCY_TIME)</summary>
        public const byte LatencyMs = 2;
        /// <summary>USB 송·수신 버퍼 크기 65536 바이트</summary>
        public const uint UsbTransferSize = 0x10000;

        private readonly object _io = new object();
        private IntPtr _handle;
        private bool _sendErrorReported;

        public Ft232hPort(Ft232hDevice device)
        {
            Device = device;
        }

        public Ft232hDevice Device { get; }

        public string SerialNumber => SerialOf(Device);

        public bool IsOpen => _handle != IntPtr.Zero;

        /// <summary>마지막 오류 메시지 (원본 m_szErrorMessage[index]와 같은 문구)</summary>
        public string LastError { get; private set; } = "";

        /// <summary>FT_SetEventNotification 등에 쓰는 원시 핸들</summary>
        internal IntPtr Handle => _handle;

        public static string SerialOf(Ft232hDevice device)
        {
            switch (device)
            {
                case Ft232hDevice.SemImage: return "ImgCap001";
                case Ft232hDevice.BseImage: return "BseICap001";
                case Ft232hDevice.ScanBridge: return "ScanBri001";
                default: throw new ArgumentOutOfRangeException(nameof(device));
            }
        }

        private string BoardName
        {
            get
            {
                switch (Device)
                {
                    case Ft232hDevice.SemImage: return "SE-Image";
                    case Ft232hDevice.BseImage: return "BSE-Image";
                    default: return "SCAN Bridge";
                }
            }
        }

        /// <summary>
        /// 장치 열기: 목록 확인 → 시리얼로 OpenEx → BitMode reset(0x00) → 10 ms → sync FIFO(0x40)
        /// → Latency 2 ms → USB 버퍼 64 KB → RTS/CTS → RX/TX purge. 원본 순서 그대로.
        /// </summary>
        public bool Open()
        {
            lock (_io)
            {
                if (IsOpen) return true;
                _sendErrorReported = false;

                if (Ftd2xx.FT_CreateDeviceInfoList(out uint count) != Ftd2xx.FT_OK)
                    return Fail("Error: Can't Create FT232H " + ListName + " Device List!");
                if (count < 1)
                    return Fail("Error: FT232H " + ListName + " Device Info List Count!");

                if (Ftd2xx.FT_OpenEx(SerialNumber, Ftd2xx.FT_OPEN_BY_SERIAL_NUMBER, out IntPtr h) != Ftd2xx.FT_OK)
                {
                    _handle = IntPtr.Zero;
                    return Fail("Error: Can't Connect FT232H " + ConnectName + " Board!");
                }
                _handle = h;

                // 원본은 설정 단계에서 실패하면 핸들을 연 채로 오류만 돌려줬다. 여기서는 닫는다.
                if (Ftd2xx.FT_SetBitMode(h, 0xFF, 0x00) != Ftd2xx.FT_OK) return FailClose("Reset Bit Mode!");
                Thread.Sleep(10);
                if (Ftd2xx.FT_SetBitMode(h, 0xFF, 0x40) != Ftd2xx.FT_OK) return FailClose("Sync FIFO Mode!");
                if (Ftd2xx.FT_SetLatencyTimer(h, LatencyMs) != Ftd2xx.FT_OK) return FailClose("Latency Time!");
                if (Ftd2xx.FT_SetUSBParameters(h, UsbTransferSize, UsbTransferSize) != Ftd2xx.FT_OK) return FailClose("USB Parameters!");
                if (Ftd2xx.FT_SetFlowControl(h, Ftd2xx.FT_FLOW_RTS_CTS, 0, 0) != Ftd2xx.FT_OK) return FailClose("Flow Control!");
                if (Ftd2xx.FT_Purge(h, Ftd2xx.FT_PURGE_RX | Ftd2xx.FT_PURGE_TX) != Ftd2xx.FT_OK) return FailClose("Purge RX and TX!");

                LastError = "";
                return true;
            }
        }

        public void Close()
        {
            lock (_io)
            {
                if (_handle == IntPtr.Zero) return;
                Ftd2xx.FT_Close(_handle);
                _handle = IntPtr.Zero;
            }
        }

        public void Dispose() => Close();

        /// <summary>단일 WORD 명령 (AfxFT232HPortSendCommand). 4바이트를 다 못 보내면 false.</summary>
        public bool SendCommand(ushort command, int para)
        {
            return WriteAll(Ft232hFrames.Command(command, para), SendErrorText);
        }

        /// <summary>WORD 배열 전송 (AfxFT232HPortSendMultiCommand)</summary>
        public bool SendWords(ushort[] words, int count)
        {
            return WriteAll(Ft232hFrames.Words(words, count), MultiSendErrorText);
        }

        /// <summary>AVR 명령 + 정수 인자 (AfxFT232HPortSendAvrCommand(port, cmd, val)). 보낸 바이트 수, 닫혀 있으면 -1.</summary>
        public int SendAvr(string command, int value)
        {
            return Write(Ft232hFrames.Avr(AvrHead, command, value));
        }

        /// <summary>AVR 명령 문자열, ';' 포함 (AfxFT232HPortSendAvrCommand(port, cmd)). 보낸 바이트 수, 닫혀 있으면 -1.</summary>
        public int SendAvr(string commandWithSemicolon)
        {
            return Write(Ft232hFrames.Avr(AvrHead, commandWithSemicolon));
        }

        /// <summary>FT_GetStatus의 수신 대기 바이트 수. 닫혀 있거나 실패하면 0.</summary>
        public int GetRxBytes()
        {
            lock (_io)
            {
                if (_handle == IntPtr.Zero) return 0;
                if (Ftd2xx.FT_GetStatus(_handle, out uint rx, out _, out _) != Ftd2xx.FT_OK) return 0;
                return (int)Math.Min(rx, int.MaxValue);
            }
        }

        /// <summary>FT_Read. 읽은 바이트 수, 실패하면 -1.</summary>
        public int Read(byte[] buffer, int count)
        {
            lock (_io)
            {
                if (_handle == IntPtr.Zero) return -1;
                if (Ftd2xx.FT_Read(_handle, buffer, (uint)Math.Min(count, buffer.Length), out uint read) != Ftd2xx.FT_OK) return -1;
                return (int)read;
            }
        }

        /// <summary>수신 바이트가 오면 신호를 받을 이벤트 등록 (FT_EVENT_RXCHAR)</summary>
        public bool SetRxEvent(EventWaitHandle evt)
        {
            lock (_io)
            {
                if (_handle == IntPtr.Zero) return false;
                return Ftd2xx.FT_SetEventNotification(_handle, Ftd2xx.FT_EVENT_RXCHAR, evt.SafeWaitHandle.DangerousGetHandle()) == Ftd2xx.FT_OK;
            }
        }

        private byte AvrHead => Device == Ft232hDevice.BseImage ? Ft232hFrames.AvrUartTx : Ft232hFrames.AvrComFifo;

        private int Write(byte[] data)
        {
            lock (_io)
            {
                if (_handle == IntPtr.Zero) return -1;
                Ftd2xx.FT_Write(_handle, data, (uint)data.Length, out uint written);
                return (int)written;
            }
        }

        private bool WriteAll(byte[] data, string errorText)
        {
            int written = Write(data);
            if (written == data.Length) return true;
            if (written >= 0 && !_sendErrorReported)
            {
                // 원본: 에러 메시지를 한 번만 남긴다
                _sendErrorReported = true;
                LastError = errorText;
            }
            return false;
        }

        private string ListName => Device == Ft232hDevice.SemImage ? "Sem Image" : Device == Ft232hDevice.BseImage ? "Bse Image" : "Scan Bridge";

        private string ConnectName => Device == Ft232hDevice.SemImage ? "SE-Image Capture" : Device == Ft232hDevice.BseImage ? "BSE-Image Capture" : "SCAN Bridge";

        private string SendErrorText =>
            Device == Ft232hDevice.SemImage ? "Error: FT232H Image Port Sending Data!" :
            Device == Ft232hDevice.BseImage ? "Error: FT232H BSE Image Scan Data Sending!" :
            "Error: FT232H SEM Device(Scan Bridge) Control Port Sending Data!";

        private string MultiSendErrorText =>
            Device == Ft232hDevice.SemImage ? "Error: FT232H Image Scan Port Sending Multi Data!" :
            Device == Ft232hDevice.BseImage ? "Error: FT232H BSE Image Scan Multi Data Sending!" :
            "Error: FT232H SEM Device(Scan Bridge) Control Port Sending Multi Data!";

        private bool Fail(string message)
        {
            LastError = message;
            return false;
        }

        private bool FailClose(string step)
        {
            Ftd2xx.FT_Close(_handle);
            _handle = IntPtr.Zero;
            return Fail("Error: FT232H " + BoardName + " " + step);
        }
    }
}
