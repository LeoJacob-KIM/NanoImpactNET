using System;

namespace NanoImpact.Hardware.Spellman
{
    /// <summary>원본이 채워 넣는 Win32 DCB 필드 (나머지는 쓰지 않는다)</summary>
    public sealed class DCB
    {
        public uint DCBlength;
        public uint BaudRate;
        public int fBinary;
        public int fParity;
        public int fOutxCtsFlow;
        public int fOutxDsrFlow;
        public int fDtrControl;
        public int fOutX;
        public int fInX;
        public int fRtsControl;
        public ushort XonLim;
        public ushort XoffLim;
        public byte ByteSize;
        public byte Parity;
        public byte StopBits;
    }

    public sealed class COMMTIMEOUTS
    {
        public uint ReadIntervalTimeout;
        public uint ReadTotalTimeoutMultiplier;
        public uint ReadTotalTimeoutConstant;
        public uint WriteTotalTimeoutMultiplier;
        public uint WriteTotalTimeoutConstant;
    }

    public sealed class COMSTAT
    {
        public uint cbInQue;
        public uint cbOutQue;
    }

    /// <summary>
    /// 원본 CUsbRS232Comm이 부르는 Win32 직렬 포트 함수. 핸들은 구현이 정하는 객체(null = INVALID_HANDLE_VALUE).
    /// ReadFile/WriteFile은 원본의 겹친 I/O 처리(ERROR_IO_PENDING이면 1000 ms 기다린 뒤 GetOverlappedResult)까지 합친 결과를 돌려준다.
    /// 실제 앱은 <see cref="SerialCommApi"/>, 원본 대조 시험은 C++ 하네스의 가짜 함수와 같은 구현을 넣는다.
    /// </summary>
    public interface ISerialCommApi
    {
        object CreateFile(string portName);
        void SetupComm(object h, int inQueue, int outQueue);
        void PurgeComm(object h, uint flags);
        void SetCommTimeouts(object h, COMMTIMEOUTS t);
        void GetCommState(object h, DCB dcb);
        void SetCommState(object h, DCB dcb);
        void SetCommMask(object h, uint mask);
        bool WaitCommEvent(object h, ref uint evtMask);
        uint GetLastError();
        void ClearCommError(object h, ref uint errors, COMSTAT stat);
        /// <summary>읽은 바이트 수 (기다려도 안 오면 0 — 원본 dwLength = 0)</summary>
        uint ReadFile(object h, byte[] buffer, int offset, int count);
        /// <summary>원본 Send의 bRet (1000 ms 안에 못 보내면 false)</summary>
        bool WriteFile(object h, byte[] buffer, int count);
        void EscapeCommFunction(object h, uint func);
        void CloseHandle(object h);
        object CreateEvent();
    }

    /// <summary>원본이 쓰는 Win32 상수 (winbase.h)</summary>
    public static class WinComm
    {
        public const uint PURGE_TXABORT = 0x0001;
        public const uint PURGE_RXABORT = 0x0002;
        public const uint PURGE_TXCLEAR = 0x0004;
        public const uint PURGE_RXCLEAR = 0x0008;
        public const uint EV_RXCHAR = 0x0001;
        public const uint SETDTR = 5;
        public const uint CLRDTR = 6;
        public const uint MAXDWORD = 0xFFFFFFFF;
        public const byte NOPARITY = 0;
        public const byte ODDPARITY = 1;
        public const byte EVENPARITY = 2;
        public const byte ONESTOPBIT = 0;
        public const byte ONE5STOPBITS = 1;
        public const byte TWOSTOPBITS = 2;
        public const int RTS_CONTROL_ENABLE = 1;
        public const int RTS_CONTROL_HANDSHAKE = 2;
        public const int DTR_CONTROL_ENABLE = 1;
        public const uint CBR_300 = 300, CBR_600 = 600, CBR_1200 = 1200, CBR_2400 = 2400, CBR_4800 = 4800, CBR_9600 = 9600, CBR_14400 = 14400,
            CBR_19200 = 19200, CBR_38400 = 38400, CBR_56000 = 56000, CBR_57600 = 57600, CBR_115200 = 115200, CBR_128000 = 128000, CBR_256000 = 256000;
    }
}
