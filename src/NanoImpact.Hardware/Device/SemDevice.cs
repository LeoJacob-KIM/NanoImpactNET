// 원본 USB_FT232H_Device.cpp의 전역 변수(1~445행)와 Win32 연결부. 함수 본문은 Device\Port\SemDevice.L*.cs.
#pragma warning disable IDE1006, CS0414, CS0169, CS0649 // 원본 이름·원본에서 안 쓰는 전역 그대로
using System;
using NanoImpact.Core.Native;
using NanoImpact.Hardware.Ftdi;
using static NanoImpact.Core.Def;

namespace NanoImpact.Hardware.Device
{
    /// <summary>
    /// 원본 USB_FT232H_Device.cpp(장비 통신·제어 전역 함수 507개)를 그대로 옮긴 클래스.
    /// 전역 변수 → 필드, Afx* 전역 함수 → 같은 이름의 메서드. 원본과의 대조는 tools\device_harness.
    /// 원본 형식 대응: BOOL → int(TRUE/FALSE), HWND → int(창 번호, 0 = 없음), char[] → CharBuffer, 구조체 포인터 → 객체(null).
    /// </summary>
    public sealed partial class SemDevice
    {
        // ---- C/Win32 상수 (원본이 windows.h에서 받던 것) ----
        private const int TRUE = 1;
        private const int FALSE = 0;
        private const int NULL = 0;
        private const uint MB_OK = 0x00000000;
        private const uint MB_OKCANCEL = 0x00000001;
        private const uint MB_YESNO = 0x00000004;
        private const uint MB_ICONERROR = 0x00000010;
        private const uint MB_ICONQUESTION = 0x00000020;
        private const uint MB_ICONWARNING = 0x00000030;
        private const uint MB_ICONINFORMATION = 0x00000040;
        private const int IDOK = 1;
        private const int IDCANCEL = 2;
        private const int IDYES = 6;
        private const int IDNO = 7;

        private readonly IDeviceHost _host;
        private readonly IFtdi _ftdi;

        public SemDevice(IDeviceHost host, IFtdi ftdi)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));
            _ftdi = ftdi ?? throw new ArgumentNullException(nameof(ftdi));
            for (int i = 0; i < m_szErrorMessage.Length; i++) m_szErrorMessage[i] = new CharBuffer(255 + 1);
            for (int i = 0; i < m_dOptionMagHVRatio.Length; i++) m_dOptionMagHVRatio[i] = DEFAULT_MAG_HV_RATIO;
        }

        // ---- Win32 호출 → IDeviceHost (원본 ::MessageBox, ::SendMessage, ::PostMessage, ::Sleep, ::GetTickCount, AfxBeginThread) ----

        private int MessageBox(int hWnd, string text, string caption, uint type) => _host.MessageBox(hWnd, text, caption, type);
        private int SendMessage(int hWnd, int msg, int wParam, object lParam) => _host.SendMessage(hWnd, msg, wParam, lParam);
        private bool PostMessage(int hWnd, int msg, int wParam, object lParam) => _host.PostMessage(hWnd, msg, wParam, lParam);
        private void Sleep(int milliseconds) => _host.Sleep(milliseconds);
        private uint GetTickCount() => _host.GetTickCount();
        private object AfxBeginThread(Func<object, uint> proc, object param) => _host.BeginThread(proc, param);

        // ---- USB_FT232H_Device.cpp:21560-21619 ----
        // 원본은 CUPDialog(MFC 진행 대화상자 예제)를 스레드에서 띄웠다. 장치 동작은 없는 화면 전용 코드라
        // 스레드 시작만 남긴다 (대화상자가 필요하면 WPF 쪽에서 만든다).
        public int AfxThreadWaitDialog(int hWnd)
        {
            AfxBeginThread(AfxThreadWaitDialogProcessing, hWnd);
            return 0;
        }

        public uint AfxThreadWaitDialogProcessing(object pParams) => 0;

        // ---- 원본 대조 시험용 (원본 AfxGetIntegratedPointer / AfxGetScanRotationCalibrationDataPointer 자리) ----
        internal IntegratedCommandStruct IntegratedForTest => m_pIntegrated;
        internal ScanRotationCalibrationData ScanRotCaliForTest => m_pScanRotCaliData;
        internal SEIMAGEDATATHREADPARAMS SEImageDataThreadParamsForTest => m_pSEImageDataThreadParams;

        /// <summary>Scan Bridge (Sbr) Command List (DefineStructCode.h:2760, 위치 = SBR_CMD_*)</summary>
        private static readonly string[] m_szSbrCommandList =
        {
            "PD", "AR", "EV", "VL", "VS", "HV", "FV", "BV", "AV", "EM",
            "AE", "AT", "RF", "RG", "SN", "RN", "SA", "RA", "SP", "RP",
            "SM", "RM", "MH", "MC", "MP", "MA", "MM", "TS", "MG", "AP",
            "PR", "LT", "RB", "ZT", "SB", "TC", "LS", "TB", "BT", "EC",
            "VR", "TM", "UL", "BF",
        };

        // ---- 원본 전역 변수 (USB_FT232H_Device.cpp:19-445, 같은 순서) ----

        private int m_hOwnerWnd = NULL;                 // Owner Window Handle
        private int m_hMainControlWnd = NULL;           // Child Window Handle #1 : Main Control
        private int m_hHWSetupControlWnd = NULL;        // Child Window Handle #2 : H/W Setup & Control
        private int m_hStageSetupControlWnd = NULL;     // Child Window Handle #3 : Stage Setup & Control
        private int m_hPopupHWSetupControlWnd = NULL;   // Popup Window Handle #4 : H/W Setup & Control
        private int m_hChild5Wnd = NULL;                // Child Window Handle #5 : Etc Control
        private int m_hChild6Wnd = NULL;
        private int m_hChild7Wnd = NULL;
        private int m_hChild8Wnd = NULL;
        private int m_hChild9Wnd = NULL;
        private int m_hDLLModule = NULL;                // USB FT232 DLL Lib Handle (C#: 1 = 로드됨)
        private readonly IntPtr[] m_hFT232HDevice = new IntPtr[MAX_FT232H_HANDLE]; // USB FT232 Handle Array
        private readonly object[] m_hEventHandle = new object[MAX_FT232H_HANDLE];  // USE FT232 Event Handle
        private int m_nFT232HIndex = 0;                 // USB FT232 Handle Index
        private int m_nFT232HDeviceUse = 0;             // USB FT232 Device Use?
        private int m_bSemImageConnected = FALSE;
        private int m_bBseImageConnected = FALSE;
        private int m_bScanBridgeConnected = FALSE;
        private int m_bSEImageScanPortErrorFlag = FALSE;
        private int m_bBSEImageScanPortErrorFlag = FALSE;
        private int m_bDeviceControlPortErrorFlag = FALSE;
        private uint m_dwFirstTickCount = 0;
        private uint m_dwSecondTickCount = 0;
        private readonly CharBuffer[] m_szErrorMessage = new CharBuffer[MAX_FT232H_HANDLE]; // Error Message 저장

        private string m_strMainServerPath = "";        // MAIN   SERVER 실행파일 경로
        private string m_strDeviceServerPath = "";      // DEVICE SERVER 실행파일 경로

        private IntPtr m_hUSBFT232H_SEImageScan = IntPtr.Zero;
        private IntPtr m_hUSBFT232H_BSEImageScan = IntPtr.Zero;
        private IntPtr m_hUSBFT232H_DeviceControl = IntPtr.Zero;

        // USB FT232H DATA COMMAND 버퍼
        private readonly CharBuffer m_szFT232H_SbrRxBuffer = new CharBuffer(MAX_FT232H_READ_BUFFER + 1);
        private readonly CharBuffer m_szFT232H_CmdBuffer = new CharBuffer(MAX_FT232H_COMMAND_BUFFER + 1);
        private readonly CharBuffer m_szFT232H_XStrBuffer = new CharBuffer(MAX_FT232H_XYDSStr_BUFFER + 1);
        private readonly CharBuffer m_szFT232H_YStrBuffer = new CharBuffer(MAX_FT232H_XYDSStr_BUFFER + 1);
        private readonly CharBuffer m_szFT232H_DStrBuffer = new CharBuffer(MAX_FT232H_XYDSStr_BUFFER + 1);
        private readonly CharBuffer m_szFT232H_SStrBuffer = new CharBuffer(MAX_FT232H_XYDSStr_BUFFER + 1);
        private readonly CharBuffer m_szFT232H_EchoCmdStr = new CharBuffer(MAX_FT232H_COMMAND_BUFFER + 1);
        private int m_nFT232H_CmdCount = 0;
        private int m_nFT232H_XStrCount = 0;
        private int m_nFT232H_YStrCount = 0;
        private int m_nFT232H_DStrCount = 0;
        private int m_nFT232H_SStrCount = 0;
        private int m_nFT232H_CommaCount = 0;
        private int m_nFT232H_XYDState = 0;
        private int m_nFT232H_CommandViewFlag = 0;
        private int m_nFT232H_EchoCommandEnable = 0;
        private int m_nFT232H_EchoCommandFlag = 0;

        // USB FT232H Device Command Control Data
        private IntegratedCommandStruct m_pIntegrated = null;            // Integrated Command & Data
        private ScanRotationCalibrationData m_pScanRotCaliData = null;   // Scan Rotation Calibration Data

        // GUN HV 설정값 리스트
        private readonly int[] m_nGunHVTable = { 30000, 25000, 20000, 15000, 10000, 5000, 3000, 2900, 2800, 2700, 2600, 2500, 2400, 2300, 2200,
            2100, 2000, 1900, 1800, 1700, 1600, 1500, 1400, 1300, 1200, 1100, 1000, 900, 800, 700, 600, 500 };
        // 고압(HV)에 따른 OL 조정(설정)값
        private readonly int[] m_nHvOLRatioTable = { 1400, 1200, 1000, 950, 900, 850, 800, 750, 700, 650, 600, 550, 500, 450, 400, 350, 300, 290, 280, 270,
            260, 250, 240, 230, 220, 210, 200, 190, 180, 170, 160, 150 };
        // 고압(HV)에 따른 SRot 조정(설정)값
        private readonly int[] m_nHvSRotRatioTable = { 1400, 1200, 1000, 950, 900, 850, 800, 750, 700, 650, 600, 550, 500, 450, 400, 350, 300, 290, 280, 270,
            260, 250, 240, 230, 220, 210, 200, 190, 180, 170, 160, 150 };
        // 고압(HV)에 따른 배율 조정(설정)값
        private readonly int[] m_nHvZoomingTable = { 1400, 1200, 1000, 950, 900, 850, 800, 750, 700, 650, 600, 550, 500, 450, 400, 350, 300, 290, 280, 270,
            260, 250, 240, 230, 220, 210, 200, 190, 180, 170, 160, 150 };
        // 이미지 스캔 주파수(Freq) 설정값
        private readonly int[] m_nSyncFreqTable = { 375, 750, 1500, 3000, 6000, 12000, 24000, 48000, 96000, 192000, 384000 };
        // Magification (배율)에 릴레이 설정값 (Mx100k 사용 안함)
        private readonly ushort[] m_MagRelay = { 0x01, 0x02, 0x04, 0x08 };
        // Mag. Relay 선택시 저항오차에 의한 배율교정 (2020.05.28: MAX 3071 --> 2900)
        private readonly int[] m_nZoomXMaxValue = { 2900, 2900, 2900, 2900, 2900 };
        private readonly int[] m_nZoomYMaxValue = { 2900, 2900, 2900, 2900, 2900 };
        // 배율
        private readonly int[] m_nMagStepValue = { 20, 30, 35, 40, 45, 50, 60, 70, 80, 90, 100, 110, 120, 130, 150, 180, 200, 220, 250, 300, 350,
            400, 450, 500, 600, 700, 800, 900, 1000, 1100, 1200, 1300, 1500, 1800, 2000, 2200, 2500, 3000, 3500, 4000, 4500,
            5000, 6000, 7000, 8000, 9000, 10000, 11000, 12000, 13000, 15000, 18000, 20000, 22000, 25000, 30000, 35000, 40000,
            45000, 50000, 60000, 70000, 80000, 90000, 100000, 110000, 120000, 130000, 150000, 180000, 200000, 220000, 250000, 300000 };
        // WOBBLE TABLE
        private readonly double[] m_dSinTable = { 0.0, 0.7, 1.0, 0.7, 0.0, -0.7, -1.0, -0.7 };
        private readonly int[] m_nWobblerTable = { 0, 0, 0, 0, 0, 0, 0, 0 };
        // SM300 SpotSize에 따라 CL1, CL2에 흐르는 전류값 ( at 30kV )
        private readonly double[] I_SM300_CL1 = { 1.0241, 0.9608, 0.8163, 0.7560, 0.6837,
            0.6265, 0.5211, 0.4488, 0.3976, 0.3614, 0.3404, 0.3133, 0.3102, 0.3042, 0.2922, 0.2831 };
        private readonly double[] I_SM300_CL2 = { 1.0602, 1.0602, 1.0602, 1.0602, 1.0632,
            1.0632, 1.0663, 1.0693, 1.0753, 1.0843, 1.0934, 1.1205, 0.9307, 0.8515, 0.8524, 0.8584 };
        // SM200 SpotSize에 따라 CL1, CL2 전류값 ( at 30kV )
        private readonly double[] I_SM200_CL1 = { 1.368, 1.240, 1.124, 1.018, 0.917,
            0.822, 0.737, 0.662, 0.594, 0.530, 0.477, 0.435, 0.397, 0.371, 0.350, 0.339 };
        private readonly double[] I_SM200_CL2 = { 1.137, 1.141, 1.146, 1.145, 1.149,
            1.154, 1.154, 1.158, 1.168, 1.189, 1.225, 1.172, 1.141, 1.130, 1.130, 1.130 };
        private readonly int[] m_nDA_CL1 = new int[MAX_SM300_SM200_CL12_ITEM];
        private readonly int[] m_nDA_CL2 = new int[MAX_SM300_SM200_CL12_ITEM];
        private readonly ushort[] m_wFilter = { 0x0008, 0x0010, 0x0020, 0x8000, 0x8000, 0x8000, 0x8000, 0x8000 };
        private readonly byte[] m_btSEScanImageData = new byte[SCAN_IMAGE_PACKET_SIZE + 1];

        private uint m_dwVaccumInfoFirstTime = 0;
        private uint m_dwVaccumInfoSecondTime = 0;
        private uint m_dwVaccumInfoSendCount = 0;

        // SendMessage 이벤트를 사용하지 않고 Function을 이용하여 직접 USB DATA를 전송할 것인지 선택한다
        private int m_bDirectUSBSending = FALSE;
        // SE SCAN DATA 를 이용한 AUTO MODE EXECUTE
        private int m_nSEScanDataAutoModeExecute = SE_SCAN_DATA_NORMAL_EXECUTE;
        // 배율에 따른 HV 보정값 (생성자에서 DEFAULT_MAG_HV_RATIO로 채움)
        private readonly double[] m_dOptionMagHVRatio = new double[MAX_GUN_HV_TABLE];
        // 배율 x35k 이상 ZOOM 값 보정율
        private double m_dOptionMagx35kZoomRatio = 1.0;
        // 샘플높이 / 샘플넓이
        private double m_dSampleHeight = 0.0;
        private double m_dSampleWidth = 0.0;

        // 2023.03.14 : MAG RELAY IMAGE SHIFT XY CORRECT VALUE
        private ClientPublicData m_pClientPublicData = null;
        public int m_MagRelayImageShiftXValue = 0;
        public int m_MagRelayImageShiftYValue = 0;

        // Public Tools Thread (다용도 순차적 기능처리 쓰레드)
        private readonly object[] m_pPublicToolsThread = new object[MAX_PUBLIC_THREAD];
        private readonly PUBLICTOOLSPARAMS[] m_pPublicToolsParams = new PUBLICTOOLSPARAMS[MAX_PUBLIC_THREAD];
        private int m_nPublicToolsIndex = -1;
        // Scan Bridge Thread
        private object m_pScanBridgeThread = null;
        private SCANBRIDGETHREADPARAMS m_pScanBridgeThreadParams = null;
        // CNC Send Thread (CNC 명령어 순차적 전송 처리 쓰레드)
        private object m_pCNCSendThread = null;
        private SCANBRIDGETHREADPARAMS m_pCNCSendThreadParams = null;
        // SEM SE Scan Image Data Thread
        private object m_pSEImageDataThread = null;
        private SEIMAGEDATATHREADPARAMS m_pSEImageDataThreadParams = null;
        // Focus Degaussing Thread (2021.06.15)
        private object m_pFocusDegaussingThread = null;
        private FOCUSDEGAUSSINGTHREADPARAMS m_pFocusDegaussingThreadParams = null;
    }

    // ---- 원본 스레드 인자 구조체 (DefineStructCode.h:2232-2381) ----

    public sealed class PUBLICTOOLSPARAMS
    {
        public int bThreadDone, bThreadStop;
        public int nSbrIndex, nSeIndex, nBseIndex;
        public IntPtr hSbrHandle, hSeHandle, hBseHandle;
        public int nPublicProcess, nPublicTools, nPublicThreadRun, nPublicThreadDone, nPublicValue, nIntervalTime, nWaitTime;
    }

    public sealed class SCANBRIDGETHREADPARAMS
    {
        public int bThreadDone, bThreadStop;
        public int nFT232HIndex;
        public IntPtr hFT232Handle;
        public System.Threading.EventWaitHandle hEventHandle;
    }

    public sealed class SEIMAGEDATATHREADPARAMS
    {
        public int bThreadDone, bThreadStop;
        public int nFT232HIndex;
        public IntPtr hFT232Handle;
        public System.Threading.EventWaitHandle hEventHandle;
        public int bScanDataClear, bScanDataReceive, bFirstButtonPressed, bErrorFlag;
        public byte[] szFTReadBuf = new byte[SCAN_IMAGE_PACKET_SIZE];
        public int pbfPtr, pbmPtr, pbfPtrB, pbmPtrB; // 원본 BYTE* (버퍼 안 위치, C#: 위치 번호) — 영상 단계에서 다시 정한다
        public ushort[] wAvrAutoValue = new ushort[2];
        public int nABCCount, nFrameCount, nXDrawPosition, nXBmpOffset, nYDrawPosition, nYBmpOffset, nXDrawPositionB, nYDrawPositionB;
        public int nFrameBrightLevel, nPixelLevelSum, nPixelCount, nWAverageNum, nWAFrameNumA, nWAFrameNumB, nXBmpPixel, nYBmpPixel;
        public int nAvrContrast, nAvrBright, nAfcAvrOL, nDMag;
        public ushort[] wXPixNumPerLine = new ushort[1000];
    }

    public sealed class FOCUSDEGAUSSINGTHREADPARAMS
    {
        public int bThreadDone, bThreadStop;
        public int nFT232HIndex;
        public IntPtr hFT232Handle;
        public System.Threading.EventWaitHandle hEventHandle;
        public int nWDPosition, nFocusCoarse, nFocusFine, nProcessTime;
    }
}
