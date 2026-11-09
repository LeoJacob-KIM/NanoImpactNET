// 원본 USB_FT232H_Device.cpp:1106-3172 (DLL 로드, FT_* 래퍼, 포트 열기·닫기·송신)을 손으로 옮긴 부분.
// 원본은 ftd2xx 함수를 GetProcAddress 포인터로 불렀다 → IFtdi. 송신 바이트는 Ft232hFrames(1단계에서 원본과 대조 완료).
#pragma warning disable IDE1006
using System;
using System.Threading;
using NanoImpact.Core.Native;
using NanoImpact.Hardware.Ftdi;
using static NanoImpact.Core.Def;
using static NanoImpact.Core.Native.CRuntime;

namespace NanoImpact.Hardware.Device
{
    public sealed partial class SemDevice
    {
        private const uint FT_OK = 0;
        private const uint FT_INVALID_HANDLE = 1;
        private const uint FT_OPEN_BY_SERIAL_NUMBER = 1;
        private const ushort FT_FLOW_RTS_CTS = 0x0100;
        private const uint FT_PURGE_RX = 1;
        private const uint FT_PURGE_TX = 2;
        private const uint FT_EVENT_RXCHAR = 1;

        // ---- USB_FT232H_Device.cpp:1106 ----
        // USB FT232H DLL 파일을 로드한다
        public int AfxUSBFT232HLoadDLL()
        {
            if (!_ftdi.Load())
            {
                MessageBox(NULL, "Error: Can't Load FT232H.dll", "FT232H ERROR", MB_ICONERROR | MB_OK);
                return RET_ERROR_FT232H_LIB_LOAD;
            }
            m_hDLLModule = 1;
            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:1270 ----
        // USB FT232H DLL 파일을 메모리에서 제거 한다 (C#: 표시만 지운다)
        public int AfxUSBFT232HFreeDLL()
        {
            if (m_hDLLModule != NULL) m_hDLLModule = NULL;
            return RET_SUCCESS_OK;
        }

        /// <summary>원본 if (!m_pXxx) 검사: DLL이 로드되지 않았으면 메시지 후 FT_INVALID_HANDLE</summary>
        private bool FtNotLoaded(string name, uint mbType)
        {
            if (m_hDLLModule != NULL) return false;
            MessageBox(NULL, "Error: " + name + " is not valid!", "FT232H ERROR", mbType);
            return true;
        }

        // ---- USB_FT232H_Device.cpp:1651-1988 : FT_* 래퍼 ----
        public uint AfxFT232HCreateDeviceInfoList(ref uint lpdwListCount)
        {
            if (FtNotLoaded("FT_CreateDeviceInfoList", MB_ICONERROR | MB_OK)) return FT_INVALID_HANDLE;
            return _ftdi.FT_CreateDeviceInfoList(out lpdwListCount);
        }

        public uint AfxFT232HGetDeviceInfoList(uint lpdwNumDevs)
        {
            if (FtNotLoaded("FT_GetDeviceInfoList", MB_ICONERROR | MB_OK)) return FT_INVALID_HANDLE;
            return _ftdi.FT_GetDeviceInfoList(lpdwNumDevs);
        }

        public IntPtr AfxFT232HGetHandle(int nIndex)
        {
            if (nIndex < 0) return IntPtr.Zero;
            return m_hFT232HDevice[nIndex];
        }

        public uint AfxFT232HOpenEx(string pArg1, uint dwFlags, int nIndex)
        {
            if (FtNotLoaded("FT_OpenEx", MB_OK)) return FT_INVALID_HANDLE;
            if (nIndex < 0) return FT_INVALID_HANDLE;
            return _ftdi.FT_OpenEx(pArg1, dwFlags, out m_hFT232HDevice[nIndex]);
        }

        public uint AfxFT232HClose(int nIndex)
        {
            if (FtNotLoaded("FT_Close", MB_OK)) return FT_INVALID_HANDLE;
            return _ftdi.FT_Close(m_hFT232HDevice[nIndex]);
        }

        public uint AfxFT232HRead(int nIndex, byte[] lpvBuffer, uint dwBuffSize, ref uint lpdwBytesRead)
        {
            if (FtNotLoaded("FT_Read", MB_OK)) return FT_INVALID_HANDLE;
            return _ftdi.FT_Read(m_hFT232HDevice[nIndex], lpvBuffer, dwBuffSize, out lpdwBytesRead);
        }

        public uint AfxFT232HRead(int nIndex, CharBuffer lpvBuffer, uint dwBuffSize, ref uint lpdwBytesRead) =>
            AfxFT232HRead(nIndex, lpvBuffer.Bytes, dwBuffSize, ref lpdwBytesRead);

        public uint AfxFT232HWrite(int nIndex, byte[] lpvBuffer, uint dwBuffSize, ref uint lpdwBytes)
        {
            if (FtNotLoaded("FT_Write", MB_OK)) return FT_INVALID_HANDLE;
            byte[] data = lpvBuffer;
            if (dwBuffSize != lpvBuffer.Length)
            {
                data = new byte[dwBuffSize];
                Array.Copy(lpvBuffer, data, Math.Min(lpvBuffer.Length, (int)dwBuffSize));
            }
            return _ftdi.FT_Write(m_hFT232HDevice[nIndex], data, dwBuffSize, out lpdwBytes);
        }

        public uint AfxFT232HResetDevice(int nIndex)
        {
            if (FtNotLoaded("FT_ResetDevice", MB_OK)) return FT_INVALID_HANDLE;
            return _ftdi.FT_ResetDevice(m_hFT232HDevice[nIndex]);
        }

        public uint AfxFT232HPurge(int nIndex, uint dwMask)
        {
            if (FtNotLoaded("FT_Purge", MB_OK)) return FT_INVALID_HANDLE;
            return _ftdi.FT_Purge(m_hFT232HDevice[nIndex], dwMask);
        }

        public uint AfxFT232HSetTimeouts(int nIndex, uint dwReadTimeout, uint dwWriteTimeout)
        {
            if (FtNotLoaded("FT_SetTimeouts", MB_OK)) return FT_INVALID_HANDLE;
            return _ftdi.FT_SetTimeouts(m_hFT232HDevice[nIndex], dwReadTimeout, dwWriteTimeout);
        }

        public uint AfxFT232HGetStatus(int nIndex, ref uint lpdwRxBytes, ref uint lpdwTxBytes, ref uint lpdwEventDWord)
        {
            if (FtNotLoaded("FT_GetStatus", MB_OK)) return FT_INVALID_HANDLE;
            return _ftdi.FT_GetStatus(m_hFT232HDevice[nIndex], out lpdwRxBytes, out lpdwTxBytes, out lpdwEventDWord);
        }

        public uint AfxFT232HGetQueueStatus(int nIndex, ref uint lpdwAmountInRxQueue)
        {
            if (FtNotLoaded("FT_GetQueueStatus", MB_OK)) return FT_INVALID_HANDLE;
            return _ftdi.FT_GetQueueStatus(m_hFT232HDevice[nIndex], out lpdwAmountInRxQueue);
        }

        public uint AfxFT232HSetBitMode(int nIndex, byte ucMask, byte ucEnable)
        {
            if (FtNotLoaded("FT_SetBitMode", MB_OK)) return FT_INVALID_HANDLE;
            return _ftdi.FT_SetBitMode(m_hFT232HDevice[nIndex], ucMask, ucEnable);
        }

        public uint AfxFT232HSetLatencyTimer(int nIndex, byte ucLatency)
        {
            if (FtNotLoaded("FT_SetLatencyTimer", MB_OK)) return FT_INVALID_HANDLE;
            return _ftdi.FT_SetLatencyTimer(m_hFT232HDevice[nIndex], ucLatency);
        }

        public uint AfxFT232HSetUSBParameters(int nIndex, uint ulInTransferSize, uint ulOutTransferSize)
        {
            if (FtNotLoaded("FT_SetUSBParameters", MB_OK)) return FT_INVALID_HANDLE;
            return _ftdi.FT_SetUSBParameters(m_hFT232HDevice[nIndex], ulInTransferSize, ulOutTransferSize);
        }

        public uint AfxFT232HSetFlowControl(int nIndex, ushort FlowControl, byte XonChar, byte XoffChar)
        {
            if (FtNotLoaded("FT_AfxFT232HSetFlowControl", MB_OK)) return FT_INVALID_HANDLE;
            return _ftdi.FT_SetFlowControl(m_hFT232HDevice[nIndex], FlowControl, XonChar, XoffChar);
        }

        public uint AfxFT232HSetEventNotification(int nIndex, uint dwEventMask, object pvArg)
        {
            if (FtNotLoaded("FT_SetEventNotification", MB_OK)) return FT_INVALID_HANDLE;
            return _ftdi.FT_SetEventNotification(m_hFT232HDevice[nIndex], dwEventMask, (EventWaitHandle)pvArg);
        }

        // ---- 원본 Win32 동기화 함수 (스레드 코드가 쓴다) ----
        private const uint WAIT_OBJECT_0 = 0;
        private const uint WAIT_TIMEOUT = 0x102;
        private const uint WAIT_FAILED = 0xFFFFFFFF;

        private static EventWaitHandle CreateEvent(object securityAttributes, int bManualReset, int bInitialState, string name) =>
            bManualReset != 0 ? (EventWaitHandle)new ManualResetEvent(bInitialState != 0) : new AutoResetEvent(bInitialState != 0);

        private static uint WaitForSingleObject(object handle, int milliseconds)
        {
            if (!(handle is WaitHandle h)) return WAIT_FAILED;
            try { return h.WaitOne(milliseconds) ? WAIT_OBJECT_0 : WAIT_TIMEOUT; }
            catch (ObjectDisposedException) { return WAIT_FAILED; }
        }

        private static int SetEvent(object handle) => handle is EventWaitHandle h && h.Set() ? TRUE : FALSE;
        private static int CloseHandle(object handle) { (handle as IDisposable)?.Dispose(); return TRUE; }

        // ---- 포트 ----

        private void SetErrorMessage(int index, string text)
        {
            memset(m_szErrorMessage[index], 0x00, 256);
            strcpy_s(m_szErrorMessage[index], text);
        }

        private void SetErrorMessageAll(string text)
        {
            SetErrorMessage(IDX_FT232H_SCAN_BRIDGE_DEVICE, text);
            SetErrorMessage(IDX_FT232H_SEM_IMAGE_DEVICE, text);
            SetErrorMessage(IDX_FT232H_BSE_IMAGE_DEVICE, text);
        }

        /// <summary>장치 이름 (원본 오류 문구 안의 이름): Scan Bridge / Sem Image / Bse Image</summary>
        private static string ListName(int nDevicePort) =>
            nDevicePort == IDX_FT232H_SCAN_BRIDGE_DEVICE ? "Scan Bridge " : nDevicePort == IDX_FT232H_SEM_IMAGE_DEVICE ? "Sem Image " : nDevicePort == IDX_FT232H_BSE_IMAGE_DEVICE ? "Bse Image " : "";

        /// <summary>목록 단계 오류: 해당 장치 하나에, 모르는 장치면 세 개 모두에 남긴다 (원본 그대로)</summary>
        private void SetListError(int nDevicePort, string format)
        {
            string text = string.Format(format, ListName(nDevicePort));
            if (nDevicePort == IDX_FT232H_SCAN_BRIDGE_DEVICE || nDevicePort == IDX_FT232H_SEM_IMAGE_DEVICE || nDevicePort == IDX_FT232H_BSE_IMAGE_DEVICE)
                SetErrorMessage(nDevicePort, text);
            else
                SetErrorMessageAll(text);
        }

        // ---- USB_FT232H_Device.cpp:1999 ----
        // FT232H USB Port 연결 장치 열기
        public int AfxFT232HPortOpen(int nDevicePort)
        {
            uint ft232hStatus;
            uint dwft232hDeviceCount = 0;

            // USB FT232H를 이용한 장치 연결정보 생성
            ft232hStatus = AfxFT232HCreateDeviceInfoList(ref dwft232hDeviceCount);
            if (ft232hStatus != FT_OK)
            {
                SetListError(nDevicePort, "Error: Can't Create FT232H {0}Device List!");
                return RET_ERROR_FT232H_DEV_CREATE;
            }
            if (dwft232hDeviceCount < 1)
            {
                SetListError(nDevicePort, "Error: FT232H {0}Device Info List Count!");
                return RET_ERROR_FT232H_DEV_COUNT;
            }
            // (원본: 장치 정보 메모리 할당 실패 → RET_ERROR_FT232H_DEV_INFO. C#에서는 일어나지 않는다)

            // FT232H 장치 연결 정보 가져오기
            ft232hStatus = AfxFT232HGetDeviceInfoList(dwft232hDeviceCount);
            if (ft232hStatus != FT_OK)
            {
                SetListError(nDevicePort, "Error: Can't Get FT232H {0}Device List Info!");
                return RET_ERROR_FT232H_DEV_LIST;
            }

            if (nDevicePort == IDX_FT232H_SEM_IMAGE_DEVICE)
            {
                // FT232H SEM SE 이미지 스캔 및 캡처 장치 연결
                m_bSemImageConnected = FALSE;
                int r = OpenBoard(nDevicePort, DEV_FT232H_SEM_IMAGE_DEVICE, "SE-Image Capture Board", "SE-Image",
                    RET_ERROR_FT232H_SEM_CONNECT, RET_ERROR_FT232H_SEM_RESET, RET_ERROR_FT232H_SEM_SYNC, RET_ERROR_FT232H_SEM_LATENCY,
                    RET_ERROR_FT232H_SEM_PARAMETERS, RET_ERROR_FT232H_SEM_FLOW, RET_ERROR_FT232H_SEM_PURGE);
                if (r != RET_SUCCESS_OK) return r;
                m_bSemImageConnected = TRUE; // SEM Se Image Board OK
            }
            else if (nDevicePort == IDX_FT232H_BSE_IMAGE_DEVICE)
            {
                // FT232H SEM BSE 이미지 스캔 및 캡처 장치 연결
                m_bBseImageConnected = FALSE;
                int r = OpenBoard(nDevicePort, DEV_FT232H_BSE_IMAGE_DEVICE, "BSE-Image Capture Board", "BSE-Image",
                    RET_ERROR_FT232H_BSE_CONNECT, RET_ERROR_FT232H_BSE_RESET, RET_ERROR_FT232H_BSE_SYNC, RET_ERROR_FT232H_BSE_LATENCY,
                    RET_ERROR_FT232H_BSE_PARAMETERS, RET_ERROR_FT232H_BSE_FLOW, RET_ERROR_FT232H_BSE_PURGE);
                if (r != RET_SUCCESS_OK) return r;
                m_bBseImageConnected = TRUE; // BSE Image Board OK
            }
            else if (nDevicePort == IDX_FT232H_SCAN_BRIDGE_DEVICE)
            {
                // FT232H Scan Bridge Device 장치 연결
                m_bScanBridgeConnected = FALSE;
                int r = OpenBoard(nDevicePort, DEV_FT232H_SCAN_BRIDGE_DEVICE, "SCAN Bridge Board", "SCAN Bridge",
                    RET_ERROR_FT232H_SBR_CONNECT, RET_ERROR_FT232H_SBR_RESET, RET_ERROR_FT232H_SBR_SYNC, RET_ERROR_FT232H_SBR_LATENCY,
                    RET_ERROR_FT232H_SBR_PARAMETERS, RET_ERROR_FT232H_SBR_FLOW, RET_ERROR_FT232H_SBR_PURGE);
                if (r != RET_SUCCESS_OK) return r;
                m_bScanBridgeConnected = TRUE; // SEM Scan Bridge Board OK
            }

            return RET_SUCCESS_OK;
        }

        /// <summary>
        /// 원본 AfxFT232HPortOpen의 장치별 블록 (세 장치가 같은 순서·같은 문구): OpenEx → BitMode reset → 10 ms → sync FIFO
        /// → Latency → USB 64KB → RTS/CTS → Purge. 원본처럼 설정 단계에서 실패해도 핸들은 열어 둔다.
        /// </summary>
        private int OpenBoard(int index, string serial, string boardName, string shortName,
            int errConnect, int errReset, int errSync, int errLatency, int errParameters, int errFlow, int errPurge)
        {
            m_nFT232HIndex = index;
            if (AfxFT232HOpenEx(serial, FT_OPEN_BY_SERIAL_NUMBER, m_nFT232HIndex) != FT_OK)
            {
                m_hFT232HDevice[m_nFT232HIndex] = IntPtr.Zero;
                SetErrorMessage(index, "Error: Can't Connect FT232H " + boardName + "!");
                return errConnect;
            }
            if (AfxFT232HSetBitMode(m_nFT232HIndex, 0xFF, 0x00) != FT_OK) { SetErrorMessage(index, "Error: FT232H " + shortName + " Reset Bit Mode!"); return errReset; }
            Sleep(10);
            if (AfxFT232HSetBitMode(m_nFT232HIndex, 0xFF, 0x40) != FT_OK) { SetErrorMessage(index, "Error: FT232H " + shortName + " Sync FIFO Mode!"); return errSync; }
            if (AfxFT232HSetLatencyTimer(m_nFT232HIndex, FT232H_LATENCY_TIME) != FT_OK) { SetErrorMessage(index, "Error: FT232H " + shortName + " Latency Time!"); return errLatency; }
            if (AfxFT232HSetUSBParameters(m_nFT232HIndex, 0x10000, 0x10000) != FT_OK) { SetErrorMessage(index, "Error: FT232H " + shortName + " USB Parameters!"); return errParameters; }
            if (AfxFT232HSetFlowControl(m_nFT232HIndex, FT_FLOW_RTS_CTS, 0, 0) != FT_OK) { SetErrorMessage(index, "Error: FT232H " + shortName + " Flow Control!"); return errFlow; }
            if (AfxFT232HPurge(m_nFT232HIndex, FT_PURGE_RX | FT_PURGE_TX) != FT_OK) { SetErrorMessage(index, "Error: FT232H " + shortName + " Purge RX and TX!"); return errPurge; }
            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:2438 ----
        // FT232H USB Port 연결 장치 닫기
        public int AfxFT232HPortClose(int nDevicePort)
        {
            IntPtr ft232hHandle = AfxFT232HGetHandle(nDevicePort);
            if (ft232hHandle == IntPtr.Zero) return RET_ERROR_FT232H_DEV_HANDLE;

            // FT232H USB Port Close
            AfxFT232HClose(nDevicePort);
            m_hFT232HDevice[nDevicePort] = IntPtr.Zero;

            return RET_SUCCESS_OK;
        }

        private struct PortCodes
        {
            public int Handle, Send, MultiSend;
            public string SendError, MultiSendError;
        }

        private static bool TryPortCodes(int nPort, out PortCodes c)
        {
            switch (nPort)
            {
                case IDX_FT232H_SEM_IMAGE_DEVICE:
                    c = new PortCodes { Handle = RET_ERROR_FT232H_SEM_HANDLE, Send = RET_ERROR_FT232H_SEM_SEND, MultiSend = RET_ERROR_FT232H_SEM_MSEND,
                        SendError = "Error: FT232H Image Port Sending Data!", MultiSendError = "Error: FT232H Image Scan Port Sending Multi Data!" };
                    return true;
                case IDX_FT232H_BSE_IMAGE_DEVICE:
                    c = new PortCodes { Handle = RET_ERROR_FT232H_BSE_HANDLE, Send = RET_ERROR_FT232H_BSE_SEND, MultiSend = RET_ERROR_FT232H_BSE_MSEND,
                        SendError = "Error: FT232H BSE Image Scan Data Sending!", MultiSendError = "Error: FT232H BSE Image Scan Multi Data Sending!" };
                    return true;
                case IDX_FT232H_SCAN_BRIDGE_DEVICE:
                    c = new PortCodes { Handle = RET_ERROR_FT232H_SBR_HANDLE, Send = RET_ERROR_FT232H_SBR_SEND, MultiSend = RET_ERROR_FT232H_SBR_MSEND,
                        SendError = "Error: FT232H SEM Device(Scan Bridge) Control Port Sending Data!",
                        MultiSendError = "Error: FT232H SEM Device(Scan Bridge) Control Port Sending Multi Data!" };
                    return true;
                default:
                    c = default(PortCodes);
                    return false;
            }
        }

        /// <summary>장치별 "에러 메시지를 한번만" 표시 (원본 m_b*PortErrorFlag)</summary>
        private void ReportSendErrorOnce(int nPort, string text)
        {
            if (nPort == IDX_FT232H_SEM_IMAGE_DEVICE) { if (m_bSEImageScanPortErrorFlag != FALSE) return; m_bSEImageScanPortErrorFlag = TRUE; }
            else if (nPort == IDX_FT232H_BSE_IMAGE_DEVICE) { if (m_bBSEImageScanPortErrorFlag != FALSE) return; m_bBSEImageScanPortErrorFlag = TRUE; }
            else { if (m_bDeviceControlPortErrorFlag != FALSE) return; m_bDeviceControlPortErrorFlag = TRUE; }
            SetErrorMessage(nPort, text);
        }

        // ---- USB_FT232H_Device.cpp:2471 ----
        // FT232H Port : 단일 WORD 데이터 전송
        public int AfxFT232HPortSendCommand(int nPort, ushort wCommand, int nPara)
        {
            if (!TryPortCodes(nPort, out PortCodes c))
            {
                SetErrorMessage(IDX_FT232H_SEM_IMAGE_DEVICE, "Error: FT232H SendCommand <Port index value>!");
                SetErrorMessage(IDX_FT232H_BSE_IMAGE_DEVICE, "Error: FT232H SendCommand <Port index value>!");
                SetErrorMessage(IDX_FT232H_SCAN_BRIDGE_DEVICE, "Error: FT232H SendCommand <Port index value>!");
                return RET_ERROR_FT232H_DEV_INDEX;
            }
            m_nFT232HIndex = nPort;
            if (AfxFT232HGetHandle(m_nFT232HIndex) == IntPtr.Zero) return c.Handle;

            byte[] data = Ft232hFrames.Command(wCommand, nPara);
            uint btWrite = 0;
            AfxFT232HWrite(m_nFT232HIndex, data, (uint)data.Length, ref btWrite);
            if (data.Length != (int)btWrite)
            {
                ReportSendErrorOnce(nPort, c.SendError);
                return c.Send;
            }
            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:2623 ----
        // FT232H : 멀티 데이터 전송
        public int AfxFT232HPortSendMultiCommand(int nPort, ushort[] pwScanBuf, int nWords)
        {
            if (!TryPortCodes(nPort, out PortCodes c))
            {
                SetErrorMessage(IDX_FT232H_SEM_IMAGE_DEVICE, "Error: FT232H SendMultiCommand <Port index value>!");
                SetErrorMessage(IDX_FT232H_BSE_IMAGE_DEVICE, "Error: FT232H SendMultiCommand <Port index value>!");
                SetErrorMessage(IDX_FT232H_SCAN_BRIDGE_DEVICE, "Error: FT232H SendMultiCommand <Port index value>!");
                return RET_ERROR_FT232H_DEV_INDEX;
            }
            m_nFT232HIndex = nPort;
            if (AfxFT232HGetHandle(m_nFT232HIndex) == IntPtr.Zero) return c.Handle;

            byte[] data = Ft232hFrames.Words(pwScanBuf, nWords);
            uint btWrite = 0;
            AfxFT232HWrite(m_nFT232HIndex, data, (uint)data.Length, ref btWrite);
            if (data.Length != (int)btWrite)
            {
                ReportSendErrorOnce(nPort, c.MultiSendError);
                return c.MultiSend;
            }
            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:2739 ----
        // FT232H : AVR 데이터 전송. 실제 전송한 바이트 수를 돌려준다.
        public int AfxFT232HPortSendAvrCommand(int nPort, string pszCommand, int nVal)
        {
            if (!TryPortCodes(nPort, out PortCodes c))
            {
                SetErrorMessage(IDX_FT232H_SEM_IMAGE_DEVICE, "Error: FT232H SendAvrCommand <Port index value>!");
                SetErrorMessage(IDX_FT232H_BSE_IMAGE_DEVICE, "Error: FT232H SendAvrCommand <Port index value>!");
                SetErrorMessage(IDX_FT232H_SCAN_BRIDGE_DEVICE, "Error: FT232H SendAvrCommand <Port index value>!");
                return RET_ERROR_FT232H_DEV_INDEX;
            }
            m_nFT232HIndex = nPort;
            if (AfxFT232HGetHandle(m_nFT232HIndex) == IntPtr.Zero) return c.Handle;

            byte head = nPort == IDX_FT232H_BSE_IMAGE_DEVICE ? (byte)bAVR_UART_TX : (byte)sAVR_COM_FIFO;
            byte[] data = Ft232hFrames.Avr(head, pszCommand, nVal);
            uint btWrite = 0;
            AfxFT232HWrite(m_nFT232HIndex, data, (uint)data.Length, ref btWrite);
            return (int)btWrite;
        }

        public int AfxFT232HPortSendAvrCommand(int nPort, ICharArray pszCommand, int nVal) => AfxFT232HPortSendAvrCommand(nPort, ToStr(pszCommand), nVal);

        // ---- USB_FT232H_Device.cpp:2964 ----
        // FT232H : AVR 데이터 전송 (명령어전송, ';'로 끝나는 명령)
        public int AfxFT232HPortSendAvrCommand(int nPort, string pszCommand)
        {
            if (!TryPortCodes(nPort, out PortCodes c))
            {
                SetErrorMessage(IDX_FT232H_SEM_IMAGE_DEVICE, "Error: FT232H SendAvrCommand <Port index value>!");
                SetErrorMessage(IDX_FT232H_BSE_IMAGE_DEVICE, "Error: FT232H SendAvrCommand <Port index value>!");
                SetErrorMessage(IDX_FT232H_SCAN_BRIDGE_DEVICE, "Error: FT232H SendAvrCommand <Port index value>!");
                return RET_ERROR_FT232H_DEV_INDEX;
            }
            m_nFT232HIndex = nPort;
            if (AfxFT232HGetHandle(m_nFT232HIndex) == IntPtr.Zero) return c.Handle;

            byte head = nPort == IDX_FT232H_BSE_IMAGE_DEVICE ? (byte)bAVR_UART_TX : (byte)sAVR_COM_FIFO;
            byte[] data = Ft232hFrames.Avr(head, pszCommand);
            uint btWrite = 0;
            AfxFT232HWrite(m_nFT232HIndex, data, (uint)data.Length, ref btWrite);
            return (int)btWrite;
        }

        public int AfxFT232HPortSendAvrCommand(int nPort, ICharArray pszCommand) => AfxFT232HPortSendAvrCommand(nPort, ToStr(pszCommand));
    }
}
