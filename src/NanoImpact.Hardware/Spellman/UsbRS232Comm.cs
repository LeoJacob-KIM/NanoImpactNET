// 원본 USB_RS232_Comm.cpp (CUsbRS232Comm, CommThread)를 손으로 옮긴 코드. Win32 직렬 함수는 ISerialCommApi, 창 메시지·스레드는 IDeviceHost.
#pragma warning disable IDE1006 // 원본 이름 그대로
using System;
using NanoImpact.Core.Native;
using NanoImpact.Hardware.Device;
using static NanoImpact.Core.Def;
using static NanoImpact.Hardware.Spellman.WinComm;

namespace NanoImpact.Hardware.Spellman
{
    /// <summary>
    /// 원본 CUsbRS232Comm: COM 포트 하나를 열고, 받는 스레드(CommThread)가 받은 바이트를 m_szInBuffer에 쌓은 뒤
    /// 감시 모드에 맞는 메시지(Spellman C-FE HV = UM_CFE_HV_POWER_CONTROL_RECEIVE_DATA)를 창에 SendMessage로 보낸다.
    /// 창은 Receive로 쌓인 바이트를 가져간다.
    /// </summary>
    public sealed class UsbRS232Comm
    {
        private const int TRUE = 1;
        private const int FALSE = 0;

        // USB_RS232_Comm.h
        public const int MAX_IN_BUF_SIZE = 4096;
        public const int MAX_OUT_BUF_SIZE = 4096;
        public const int RS232_ASCII_XON = 0x11;
        public const int RS232_ASCII_XOFF = 0x13;
        public const int RS232_MONITORING_MODE_FE_POWER = 1000;
        public const int RS232_MONITORING_MODE_ION_PUMP1 = 2000;
        public const int RS232_MONITORING_MODE_ION_PUMP2 = 3000;
        public const int RS232_MONITORING_MODE_VAC_GAUGE = 4000;
        public const int MAX_CFE_HV_POWER_COMMAND = 35;

        /// <summary>원본 전역 m_szFEPowerCommand[35][3] — Spellman 응답 명령 코드</summary>
        public static readonly string[] m_szFEPowerCommand =
        {
            "00", "01", "02", "03", "04", "05", "08", "09", "0A", "0B", "0E",
            "10", "11", "12", "13", "14", "15", "16", "17", "1A", "1C", "1D", "1E", "1F",
            "20", "21", "24", "26", "28", "29", "2A", "2B",
            "50", "52", "53",
        };

        private readonly ISerialCommApi _api;
        private readonly IDeviceHost _host;
        private readonly object _lock = new object(); // 원본 m_pEvent(CEvent) + CSingleLock 자리

        public object m_hComDev;
        public int m_hWnd;
        public int m_bOpenned;
        public string m_strComPort;
        public string m_strBaudRate;
        public string m_strParity;
        public string m_strDataBit;
        public string m_strStopBit;
        public int m_bFlowChk;
        public object m_overlapReceive_hEvent;
        public object m_overlapWrite_hEvent;
        public int m_nMonitoringMode;
        public readonly byte[] m_szInBuffer = new byte[MAX_RS232_BUFFER * 2];
        public int m_nLength;

        /// <summary>원본 CUsbRS232Comm(strPort, strBaudrate, strParity, strDatabit, strStopbit)</summary>
        public UsbRS232Comm(ISerialCommApi api, IDeviceHost host, string strPort, string strBaudrate, string strParity, string strDatabit, string strStopbit)
        {
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _host = host ?? throw new ArgumentNullException(nameof(host));
            m_strComPort = strPort;
            m_strBaudRate = strBaudrate;
            m_strParity = strParity;
            m_strDataBit = strDatabit;
            m_strStopBit = strStopbit;
            m_bFlowChk = 1;
            m_bOpenned = FALSE;
            m_nLength = 0;

            // BUFFER CLEAR
            Array.Clear(m_szInBuffer, 0, m_szInBuffer.Length);
        }

        /// <summary>원본 ~CUsbRS232Comm (delete 자리에서 부른다)</summary>
        public void Destroy()
        {
            if (m_bOpenned == TRUE)
            {
                Close();
            }
        }

        public void ResetSerial()
        {
            // PORT OPEN 확인
            if (m_bOpenned != TRUE) return;

            uint dwErr = 0;
            _api.ClearCommError(m_hComDev, ref dwErr, null);
            _api.SetupComm(m_hComDev, MAX_IN_BUF_SIZE, MAX_OUT_BUF_SIZE);
            _api.PurgeComm(m_hComDev, PURGE_TXABORT | PURGE_RXABORT | PURGE_TXCLEAR | PURGE_RXCLEAR);

            // Set up for overlapped I/O
            var CommTimeOuts = new COMMTIMEOUTS
            {
                ReadIntervalTimeout = MAXDWORD,
                ReadTotalTimeoutMultiplier = 0,
                ReadTotalTimeoutConstant = 0,
                WriteTotalTimeoutMultiplier = 0,
                WriteTotalTimeoutConstant = 1000,
            };
            _api.SetCommTimeouts(m_hComDev, CommTimeOuts);

            var dcb = new DCB();
            dcb.DCBlength = 28; // sizeof(DCB)
            _api.GetCommState(m_hComDev, dcb);

            dcb.fBinary = TRUE;
            dcb.fParity = TRUE;

            // Baudrate
            if (m_strBaudRate == "300") dcb.BaudRate = CBR_300;
            else if (m_strBaudRate == "600") dcb.BaudRate = CBR_600;
            else if (m_strBaudRate == "1200") dcb.BaudRate = CBR_1200;
            else if (m_strBaudRate == "2400") dcb.BaudRate = CBR_2400;
            else if (m_strBaudRate == "4800") dcb.BaudRate = CBR_4800;
            else if (m_strBaudRate == "9600") dcb.BaudRate = CBR_9600;
            else if (m_strBaudRate == "14400") dcb.BaudRate = CBR_14400;
            else if (m_strBaudRate == "19200") dcb.BaudRate = CBR_19200;
            else if (m_strBaudRate == "28800") dcb.BaudRate = CBR_38400;
            else if (m_strBaudRate == "33600") dcb.BaudRate = CBR_38400;
            else if (m_strBaudRate == "38400") dcb.BaudRate = CBR_38400;
            else if (m_strBaudRate == "56000") dcb.BaudRate = CBR_56000;
            else if (m_strBaudRate == "57600") dcb.BaudRate = CBR_57600;
            else if (m_strBaudRate == "115200") dcb.BaudRate = CBR_115200;
            else if (m_strBaudRate == "128000") dcb.BaudRate = CBR_128000;
            else if (m_strBaudRate == "256000") dcb.BaudRate = CBR_256000;
            else if (m_strBaudRate == "PCI_9600") dcb.BaudRate = 1075;
            else if (m_strBaudRate == "PCI_19200") dcb.BaudRate = 2212;
            else if (m_strBaudRate == "PCI_38400") dcb.BaudRate = 4300;
            else if (m_strBaudRate == "PCI_57600") dcb.BaudRate = 6450;
            else if (m_strBaudRate == "PCI_500K") dcb.BaudRate = 56000;
            else dcb.BaudRate = CBR_115200;

            // Parity
            if (m_strParity == "None") dcb.Parity = NOPARITY;
            else if (m_strParity == "Even") dcb.Parity = EVENPARITY;
            else if (m_strParity == "Odd") dcb.Parity = ODDPARITY;

            // Data Bits
            if (m_strDataBit == "7Bit") dcb.ByteSize = 7;
            else if (m_strDataBit == "8Bit") dcb.ByteSize = 8;
            else dcb.ByteSize = 8;

            // Stop Bits
            if (m_strStopBit == "1Bit") dcb.StopBits = ONESTOPBIT;
            else if (m_strStopBit == "1.5Bit") dcb.StopBits = ONE5STOPBITS;
            else if (m_strStopBit == "2Bit") dcb.StopBits = TWOSTOPBITS;
            else dcb.StopBits = ONESTOPBIT;

            // FLOW CONTROL
            dcb.fRtsControl = RTS_CONTROL_ENABLE;
            dcb.fDtrControl = DTR_CONTROL_ENABLE;
            dcb.fOutxDsrFlow = FALSE;

            // Flow Control 사용
            if (m_bFlowChk == TRUE)
            {
                dcb.fOutX = FALSE;
                dcb.fInX = FALSE;
                dcb.XonLim = 2048;
                dcb.XoffLim = 1024;
            }
            // Flow Control 사용안함
            else
            {
                dcb.fOutxCtsFlow = TRUE;
                dcb.fRtsControl = RTS_CONTROL_HANDSHAKE;
            }

            // 통신 PORT 설정
            _api.SetCommState(m_hComDev, dcb);
            // 통신 EVENT 설정
            _api.SetCommMask(m_hComDev, EV_RXCHAR);
        }

        public void Close()
        {
            if (m_bOpenned == FALSE) return;

            // CLEAR & 초기화 — 마스크를 0으로 바꾸면 받는 스레드의 WaitCommEvent가 끝나고 스레드가 핸들을 닫는다
            m_bOpenned = FALSE;
            _api.SetCommMask(m_hComDev, 0);
            _api.EscapeCommFunction(m_hComDev, CLRDTR);
            _api.PurgeComm(m_hComDev, PURGE_TXABORT | PURGE_RXABORT | PURGE_TXCLEAR | PURGE_RXCLEAR);

            _host.Sleep(500);
        }

        /// <summary>원본 전역 UINT CommThread(LPVOID lpData) — 받는 스레드 본문. 포트가 닫히거나 오류가 날 때까지 돈다.</summary>
        public static uint CommThread(object lpData)
        {
            uint dwErrorFlags = 0;
            var ComStat = new COMSTAT();
            uint dwEvtMask;
            var szBuffer = new byte[MAX_RS232_BUFFER];
            uint dwLength;
            int nSize = 0;
            int nInSize = 0;
            bool bWaitFlag;
            int bErrFlag = FALSE;

            var pCommClass = lpData as UsbRS232Comm;
            if (pCommClass == null) return 0;
            ISerialCommApi api = pCommClass._api;
            IDeviceHost host = pCommClass._host;

            int bLoop = pCommClass.m_bOpenned;
            while (bLoop == TRUE)
            {
                dwEvtMask = 0;
                dwLength = 0;
                nInSize = 0;

                // BUFFER CLEAR
                Array.Clear(szBuffer, 0, szBuffer.Length);

                // COM PORT 이벤트 발생 기다리기
                bWaitFlag = api.WaitCommEvent(pCommClass.m_hComDev, ref dwEvtMask);
                // CHECK COM PORT STATE
                if (!bWaitFlag && dwEvtMask == 0)
                {
                    // COM PORT CLOSE 또는 ERROR
                    bLoop = FALSE;
                    bErrFlag = TRUE;
                    dwErrorFlags = api.GetLastError();
                    break;
                }

                // COM PORT ERROR 상태 정보 CLEAR
                api.ClearCommError(pCommClass.m_hComDev, ref dwErrorFlags, ComStat);

                // RECEIVE DATA 확인?
                if ((dwEvtMask & EV_RXCHAR) != 0 && ComStat.cbInQue != 0)
                {
                    // RECEIVE DATA 갯수 설정
                    if (ComStat.cbInQue > MAX_RS232_BUFFER) nSize = MAX_RS232_BUFFER;
                    else nSize = (int)ComStat.cbInQue;

                    // RECEIVE DATA 처리
                    do
                    {
                        // COM PORT ERROR 상태 정보 CLEAR
                        api.ClearCommError(pCommClass.m_hComDev, ref dwErrorFlags, ComStat);

                        // RECEIVE DATA 읽기 (원본: nSize를 그대로 넘겨 두 번째 읽기부터 szBuffer 밖까지 허용함 → 남은 칸만큼만)
                        dwLength = api.ReadFile(pCommClass.m_hComDev, szBuffer, nInSize, Math.Min(nSize, szBuffer.Length - nInSize));
                        nInSize += (int)dwLength;

                    } while ((dwLength != 0) && (nInSize < nSize));

                    // COM PORT ERROR 상태 정보 CLEAR
                    api.ClearCommError(pCommClass.m_hComDev, ref dwErrorFlags, ComStat);

                    lock (pCommClass._lock)
                    {
                        // 원본은 넘칠 때 nInSize = (m_nLength + nInSize) - MAX*2 로 계산해(빈 칸이 아니라 넘친 양) 버퍼 밖에 쓸 수 있었다 → 빈 칸만큼만
                        if (pCommClass.m_nLength + nInSize > MAX_RS232_BUFFER * 2)
                        {
                            nInSize = MAX_RS232_BUFFER * 2 - pCommClass.m_nLength;
                        }

                        Buffer.BlockCopy(szBuffer, 0, pCommClass.m_szInBuffer, pCommClass.m_nLength, nInSize);
                        pCommClass.m_nLength += nInSize;
                    }

                    // 데이터 수신 알림 및 처리
                    switch (pCommClass.m_nMonitoringMode)
                    {
                        case RS232_MONITORING_MODE_FE_POWER:
                            host.SendMessage(pCommClass.m_hWnd, UM_CFE_HV_POWER_CONTROL_RECEIVE_DATA, pCommClass.m_nLength, pCommClass);
                            break;
                        case RS232_MONITORING_MODE_ION_PUMP1:
                            host.SendMessage(pCommClass.m_hWnd, UM_RS232_PROCESS_ION_PUMP1_RECEIVE_DATA, pCommClass.m_nLength, pCommClass);
                            break;
                        case RS232_MONITORING_MODE_ION_PUMP2:
                            host.SendMessage(pCommClass.m_hWnd, UM_RS232_PROCESS_ION_PUMP2_RECEIVE_DATA, pCommClass.m_nLength, pCommClass);
                            break;
                        case RS232_MONITORING_MODE_VAC_GAUGE:
                            host.SendMessage(pCommClass.m_hWnd, UM_RS232_PROCESS_VAC_GAUGE_RECEIVE_DATA, pCommClass.m_nLength, pCommClass);
                            break;
                    }
                }
            }

            api.PurgeComm(pCommClass.m_hComDev, PURGE_TXABORT | PURGE_RXABORT | PURGE_TXCLEAR | PURGE_RXCLEAR);

            // HANDLE CLOSE
            pCommClass.HandleClose();

            // OWNER WINDOW HANDLE
            int hOwner = pCommClass.m_hWnd;
            int nMonitoringMode = pCommClass.m_nMonitoringMode;
            object tempClass = 0;

            // DELETE COM CLASS
            pCommClass.Destroy();
            pCommClass = null;

            // COM PORT CLOSE: 정상 종료는 wParam 1, 프로그램 종료·오류는 wParam 0 + 오류 코드
            int wParam;
            if (bErrFlag == FALSE)
            {
                wParam = 1;
            }
            else
            {
                wParam = 0;
                tempClass = (int)dwErrorFlags;
            }
            if (hOwner != 0)
            {
                switch (nMonitoringMode)
                {
                    case RS232_MONITORING_MODE_FE_POWER:
                        host.SendMessage(hOwner, UM_CFE_HV_POWER_CONTROL_PORT_CLOSE, wParam, tempClass);
                        break;
                    case RS232_MONITORING_MODE_ION_PUMP1:
                        host.SendMessage(hOwner, UM_RS232_PROCESS_ION_PUMP1_PORT_CLOSE, wParam, tempClass);
                        break;
                    case RS232_MONITORING_MODE_ION_PUMP2:
                        host.SendMessage(hOwner, UM_RS232_PROCESS_ION_PUMP2_PORT_CLOSE, wParam, tempClass);
                        break;
                    case RS232_MONITORING_MODE_VAC_GAUGE:
                        host.SendMessage(hOwner, UM_RS232_PROCESS_VAC_GAUGE_PORT_CLOSE, wParam, tempClass);
                        break;
                }
            }

            return 0;
        }

        public void HandleClose()
        {
            _api.CloseHandle(m_hComDev);
            _api.CloseHandle(m_overlapReceive_hEvent);
            _api.CloseHandle(m_overlapWrite_hEvent);
        }

        public int Create(int hWnd, int nMonitoringMode)
        {
            // Owner Window Handle
            m_hWnd = hWnd;

            // RS232 MONITORING MODE
            m_nMonitoringMode = nMonitoringMode;

            // COM PORT OPEN
            m_hComDev = _api.CreateFile(m_strComPort);
            if (m_hComDev == null)
            {
                // COM PORT OPEN FAILE
                return FALSE;
            }

            // COM PORT OPEN 성공
            m_bOpenned = TRUE;

            // COM PORT 설정
            ResetSerial();

            // RECEIVE 이벤트 생성
            m_overlapReceive_hEvent = _api.CreateEvent();
            if (m_overlapReceive_hEvent == null)
            {
                _api.CloseHandle(m_hComDev);
                return FALSE;
            }

            // SEND 이벤트 생성
            m_overlapWrite_hEvent = _api.CreateEvent();
            if (m_overlapWrite_hEvent == null)
            {
                _api.CloseHandle(m_hComDev);
                return FALSE;
            }

            // RECEIVE DATA 처리 쓰레드 생성 및 시작
            _host.BeginThread(CommThread, this);
            _api.EscapeCommFunction(m_hComDev, SETDTR);

            return TRUE;
        }

        public int Send(ICharArray outbuf, int len)
        {
            var b = new byte[len];
            for (int i = 0; i < len; i++) b[i] = outbuf[i];
            return Send(b, len);
        }

        public int Send(byte[] outbuf, int len)
        {
            int bRet = TRUE;
            uint dwErrorFlags = 0;
            var ComStat = new COMSTAT();

            // COM PORT ERROR 상태 정보 CLEAR
            _api.ClearCommError(m_hComDev, ref dwErrorFlags, ComStat);

            // DATA 전송
            if (!_api.WriteFile(m_hComDev, outbuf, len))
            {
                bRet = FALSE;
            }

            // COM PORT ERROR 상태 정보 CLEAR
            _api.ClearCommError(m_hComDev, ref dwErrorFlags, ComStat);

            return bRet;
        }

        public int Receive(ICharArray lpszInbuf, int nLen)
        {
            // Argument value is not valid
            if (nLen == 0)
            {
                return -1;
            }
            else if (nLen > MAX_RS232_BUFFER)
            {
                return -1;
            }

            lock (_lock)
            {
                // RECEIVE DATA BUFFER
                if (m_nLength == 0)
                {
                    lpszInbuf[0] = 0x00;
                    return 0;
                }
                else if (m_nLength <= nLen)
                {
                    for (int i = 0; i < m_nLength; i++) lpszInbuf[i] = m_szInBuffer[i];
                    Array.Clear(m_szInBuffer, 0, m_szInBuffer.Length);

                    int nTmp = m_nLength;
                    m_nLength = 0;
                    return nTmp;
                }
                else
                {
                    for (int i = 0; i < nLen; i++) lpszInbuf[i] = m_szInBuffer[i];
                    Buffer.BlockCopy(m_szInBuffer, nLen, m_szInBuffer, 0, MAX_RS232_BUFFER * 2 - nLen);
                    m_nLength -= nLen;
                    return nLen;
                }
            }
        }

        public void Clear()
        {
            // BUFFER & EVENT CLEAR
            _api.PurgeComm(m_hComDev, PURGE_TXABORT | PURGE_RXABORT | PURGE_TXCLEAR | PURGE_RXCLEAR);
            lock (_lock)
            {
                Array.Clear(m_szInBuffer, 0, m_szInBuffer.Length);
                m_nLength = 0;
            }
        }
    }
}
