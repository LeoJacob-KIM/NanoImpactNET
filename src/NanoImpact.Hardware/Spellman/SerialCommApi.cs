using System;
using System.IO.Ports;
using System.Threading;
using static NanoImpact.Hardware.Spellman.WinComm;

namespace NanoImpact.Hardware.Spellman
{
    /// <summary>
    /// <see cref="ISerialCommApi"/>의 실제 구현 (System.IO.Ports.SerialPort). 핸들 = 포트 객체.
    /// WaitCommEvent는 받은 바이트가 있으면 EV_RXCHAR로 돌아오고, 마스크가 0이 되거나 포트가 닫히면 false로 돌아온다
    /// (원본 Close가 SetCommMask(0)으로 받는 스레드를 끝내는 방식).
    /// </summary>
    public sealed class SerialCommApi : ISerialCommApi
    {
        private const uint ERROR_OPERATION_ABORTED = 995;
        private const uint ERROR_INVALID_HANDLE = 6;

        private uint _lastError;

        private sealed class Port
        {
            public SerialPort Serial;
            public uint Mask;
            public readonly AutoResetEvent Signal = new AutoResetEvent(false);
        }

        public object CreateFile(string portName)
        {
            // 원본 이름 "\\.\COM10" → "COM10"
            string name = portName.StartsWith(@"\\.\", StringComparison.Ordinal) ? portName.Substring(4) : portName;
            var p = new Port { Serial = new SerialPort(name) { Handshake = Handshake.None, ReadTimeout = 1000, WriteTimeout = 1000 } };
            try
            {
                p.Serial.Open();
            }
            catch (Exception)
            {
                p.Serial.Dispose();
                return null;
            }
            p.Serial.DataReceived += (s, e) => p.Signal.Set();
            p.Serial.ErrorReceived += (s, e) => p.Signal.Set();
            return p;
        }

        public void SetupComm(object h, int inQueue, int outQueue)
        {
            // SerialPort는 연 뒤에 큐 크기를 바꿀 수 없다 (기본 4096 = 원본 값)
        }

        public void PurgeComm(object h, uint flags)
        {
            if (!(h is Port p) || !p.Serial.IsOpen) return;
            try
            {
                if ((flags & PURGE_RXCLEAR) != 0) p.Serial.DiscardInBuffer();
                if ((flags & PURGE_TXCLEAR) != 0) p.Serial.DiscardOutBuffer();
            }
            catch (Exception)
            {
            }
        }

        public void SetCommTimeouts(object h, COMMTIMEOUTS t)
        {
            if (h is Port p && t.WriteTotalTimeoutConstant > 0) p.Serial.WriteTimeout = (int)t.WriteTotalTimeoutConstant;
        }

        public void GetCommState(object h, DCB dcb)
        {
            if (!(h is Port p)) return;
            dcb.BaudRate = (uint)p.Serial.BaudRate;
            dcb.ByteSize = (byte)p.Serial.DataBits;
        }

        public void SetCommState(object h, DCB dcb)
        {
            if (!(h is Port p)) return;
            SerialPort s = p.Serial;
            s.BaudRate = (int)dcb.BaudRate;
            s.DataBits = dcb.ByteSize;
            s.Parity = dcb.Parity == EVENPARITY ? Parity.Even : dcb.Parity == ODDPARITY ? Parity.Odd : Parity.None;
            s.StopBits = dcb.StopBits == TWOSTOPBITS ? StopBits.Two : dcb.StopBits == ONE5STOPBITS ? StopBits.OnePointFive : StopBits.One;
            if (dcb.fRtsControl == RTS_CONTROL_HANDSHAKE && dcb.fOutxCtsFlow != 0)
            {
                s.Handshake = Handshake.RequestToSend;
            }
            else
            {
                s.Handshake = dcb.fOutX != 0 || dcb.fInX != 0 ? Handshake.XOnXOff : Handshake.None;
                s.RtsEnable = dcb.fRtsControl == RTS_CONTROL_ENABLE;
            }
            s.DtrEnable = dcb.fDtrControl == DTR_CONTROL_ENABLE;
        }

        public void SetCommMask(object h, uint mask)
        {
            if (!(h is Port p)) return;
            p.Mask = mask;
            p.Signal.Set();
        }

        public bool WaitCommEvent(object h, ref uint evtMask)
        {
            evtMask = 0;
            if (!(h is Port p))
            {
                _lastError = ERROR_INVALID_HANDLE;
                return false;
            }
            while (true)
            {
                if (p.Mask == 0 || !p.Serial.IsOpen)
                {
                    _lastError = ERROR_OPERATION_ABORTED;
                    return false;
                }
                try
                {
                    if ((p.Mask & EV_RXCHAR) != 0 && p.Serial.BytesToRead > 0)
                    {
                        evtMask = EV_RXCHAR;
                        return true;
                    }
                }
                catch (Exception)
                {
                    _lastError = ERROR_OPERATION_ABORTED;
                    return false;
                }
                p.Signal.WaitOne(250);
            }
        }

        public uint GetLastError() => _lastError;

        public void ClearCommError(object h, ref uint errors, COMSTAT stat)
        {
            errors = 0;
            if (stat == null) return;
            try
            {
                stat.cbInQue = h is Port p && p.Serial.IsOpen ? (uint)p.Serial.BytesToRead : 0;
                stat.cbOutQue = h is Port q && q.Serial.IsOpen ? (uint)q.Serial.BytesToWrite : 0;
            }
            catch (Exception)
            {
                stat.cbInQue = 0;
                stat.cbOutQue = 0;
            }
        }

        public uint ReadFile(object h, byte[] buffer, int offset, int count)
        {
            if (!(h is Port p) || count <= 0) return 0;
            try
            {
                return (uint)p.Serial.Read(buffer, offset, count);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public bool WriteFile(object h, byte[] buffer, int count)
        {
            if (!(h is Port p)) return false;
            try
            {
                p.Serial.Write(buffer, 0, count);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void EscapeCommFunction(object h, uint func)
        {
            if (!(h is Port p) || !p.Serial.IsOpen) return;
            if (func == SETDTR) p.Serial.DtrEnable = true;
            else if (func == CLRDTR) p.Serial.DtrEnable = false;
        }

        public void CloseHandle(object h)
        {
            if (!(h is Port p)) return;
            p.Mask = 0;
            p.Signal.Set();
            p.Serial.Dispose();
        }

        public object CreateEvent() => new object();
    }
}
