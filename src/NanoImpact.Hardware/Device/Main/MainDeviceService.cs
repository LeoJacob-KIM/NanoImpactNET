// 원본 메인 화면(CNanoImpact2020cfeDlg)의 장치 처리기가 쓰는 멤버·패널·화면 함수 대체. 처리기 본문은 Device\Main\Port (번역 후 손질, 다시 생성하지 않음).
//   장치 → 화면: OnSemControl*Data / OnSemHWSetup* / OnSemControlStage* / OnGetUsbDevice* (UM_USB_SBR_*, UM_REQUEST_GET_USB_DATA*)
//   화면 → 장치: OnSetUsbDevice* (패널·ModularAssistDevice가 부름)
#pragma warning disable IDE1006, CS0414, CS0649 // 원본 이름·원본에서 안 쓰는 멤버 그대로
using System;
using NanoImpact.Core;
using NanoImpact.Core.Native;

namespace NanoImpact.Hardware.Device
{
    /// <summary>
    /// 장치 처리기가 부르는 원본 메인 화면 함수 중 이 범위 밖(화면 그리기·스캔 시작·자동 기능·타이머)인 것. 7·9단계가 구현한다.
    /// </summary>
    public interface IMainDeviceView
    {
        void Update1stImageInfoBitmap(int nX, int nY, int nWidth, int nHeight);
        void Update2ndImageInfoBitmap(int nX, int nY, int nWidth, int nHeight);
        void DisplayVacuumStateInfo(int nVacuumState);
        void SetMainDlgMagComboIndex(int nIndex);
        void SetMainDlgGunHVComboIndex(int nIndex);
        int SemScanModeSelectAndScanStart(int nScanMode, int nScanWidth, int nScanHeight, int nBitmapXOffset, int nBitmapYOffset, int nBitmapAverage, int bScanGeneration);
        int SemSlow12ScanModeSelectAndScanStart(int nScanMode, int nScanWidth, int nScanHeight, int nBitmapXOffset, int nBitmapYOffset, int nBitmapAverage);
        void SemSEScanImageDataUpdateControl();
        void StageXYAxisMagnificationMovingSpeedChange(int nMagIndex);
        void SetExecuteAutoProcessingStop();
        void SetExecuteAutoFocusProcessing();
        void SetExecuteAutoContrastProcessing();
        void SetExecuteAutoContrastBrightnessProcessing();
        void SetExecuteAutoBrightnessProcessing();
        int SetAutoBCCompletedFlag(int bOnOff);
        int GetAutoBCRunFlag();
        /// <summary>원본 SetWDSetupEnabledOLValue(nCoarse) — ModularAssistDevice가 메인 화면을 거쳐 Scan 보정 창 WD 설정에 넣는 OL 값</summary>
        void SetWDSetupEnabledOLValue(int nCoarse);
        /// <summary>원본 메인 화면 SetTimer(TIMER_ID_*, ms) — 실제 구현은 시간이 되면 메인 화면 OnTimer 처리</summary>
        void SetTimer(int nIDEvent, int nElapse);
        void KillTimer(int nIDEvent);
    }

    /// <summary>메인 화면 장치 처리기가 부르는 패널 (원본 m_wndXxx 멤버). 실제 패널(8단계) 또는 원본 대조의 기록 클래스.</summary>
    public sealed class MainPanels
    {
        public IModularAlignControlDlg AlignControl;
        public IModularBeamControlDlg BeamControl;
        public IModularDetailControlDlg DetailControl;
        public IModularEnvironmentToolDlg EnvironmentTool;
        public IModularFocusControlDlg FocusControl;
        public IModularScanCalibrationToolDlg ScanCalibrationTool;
        public IModularScanControlDlg ScanControl;
        public IModularStageCalibrationToolDlg StageCalibrationTool;
        public IModularStageControlDlg StageControl;
        public IModularVacuumControlDlg VacuumControl;
    }

    /// <summary>
    /// 원본 메인 화면의 장치 처리기 (6b). 원본 멤버를 같은 이름으로 들고, 장치 계층이 보내는 창 메시지(<see cref="WindowProc"/>)와
    /// ModularAssistDevice의 호출(<see cref="IAssistOwner"/>)을 받는다.
    /// </summary>
    public sealed partial class MainDeviceService : IAssistOwner
    {
        private const int TRUE = 1;
        private const int FALSE = 0;
        private const int NULL = 0;

        // NanoImpact2020cfeDlg.cpp:408-426
        public const int TIMER_ID_SE_SCAN_IMAGE_DISPLAY = 10;
        public const int TIMER_ID_SE_SCAN_IMAGE_DISPLAY_NEXT = 11;
        public const int TIMER_ID_SE_PHOTO12_SCAN_IMAGE_DISPLAY = 20;
        public const int TIMER_ID_SE_PHOTO345_SCAN_IMAGE_DISPLAY = 21;
        public const int TIMER_ID_SE_PHOTO6_SCAN_IMAGE_DISPLAY = 22;
        public const int TIMER_ID_BSE_SCAN_IMAGE_DISPLAY = 30;
        public const int TIMER_ID_MAG_XY_AXIS_ACCEL_CHANGE = 40;
        public const int TIMER_ID_AUTO_START_PROCESS = 50;
        public const int TIMER_ID_DIALOG_START_PROCESS = 98;
        public const int TIMER_ID_DIALOG_PROGRAM_CLOSE = 99;

        private readonly IMainDeviceView _view;

        /// <param name="device">장치 계층</param>
        /// <param name="assist">ModularAssistDevice (이 객체를 owner로 연결한다)</param>
        /// <param name="view">범위 밖 메인 화면 함수</param>
        /// <param name="panels">패널</param>
        /// <param name="scanControlData">SE 스캔 공용 상태 (영상 조립 SemScanImageProcessor와 같은 객체를 넘긴다)</param>
        /// <param name="hWnd">메인 화면 창 번호 (원본 GetSafeHwnd())</param>
        public MainDeviceService(SemDevice device, AssistDevice assist, IMainDeviceView view, MainPanels panels,
            SEM_SE_SCAN_CONTROL_PUBLIC_DATA scanControlData, int hWnd)
        {
            m_dev = device ?? throw new ArgumentNullException(nameof(device));
            m_pAssistDeviceTool = assist ?? throw new ArgumentNullException(nameof(assist));
            _view = view ?? throw new ArgumentNullException(nameof(view));
            if (panels == null) throw new ArgumentNullException(nameof(panels));
            m_wndAlignControlDlg = panels.AlignControl;
            m_wndBeamControlDlg = panels.BeamControl;
            m_wndDetailControlDlg = panels.DetailControl;
            m_wndEnvironmentToolDlg = panels.EnvironmentTool;
            m_wndFocusControlDlg = panels.FocusControl;
            m_wndScanCalibrationToolDlg = panels.ScanCalibrationTool;
            m_wndScanControlDlg = panels.ScanControl;
            m_wndStageCalibrationToolDlg = panels.StageCalibrationTool;
            m_wndStageControlDlg = panels.StageControl;
            m_wndVacuumControlDlg = panels.VacuumControl;
            m_SemSEScanControlPublicData = scanControlData ?? new SEM_SE_SCAN_CONTROL_PUBLIC_DATA();
            m_hWnd = hWnd;
            assist.SetOwnerWindow(this);
            assist.SetVacuumBeamAlignFocusPublicData(m_VacuumBeamAlignFocusData);
        }

        // ---- 원본 멤버 (NanoImpact2020cfeDlg.h) ----

        private readonly SemDevice m_dev;
        public readonly int m_hWnd;
        public readonly AssistDevice m_pAssistDeviceTool;
        public readonly IntegratedCommandStruct m_IntegratedData = new IntegratedCommandStruct();  // 장치 공용 데이터의 메인 화면 사본
        public readonly ClientPublicData m_ClientPublicData = new ClientPublicData();              // CLIENTPUBLICDATA.INI
        public readonly SEM_SE_SCAN_CONTROL_PUBLIC_DATA m_SemSEScanControlPublicData;
        public readonly VACUUM_BEAM_ALIGN_FOCUS_DATA m_VacuumBeamAlignFocusData = new VACUUM_BEAM_ALIGN_FOCUS_DATA();
        public string m_strDistDecimalPlaces = "%6.2f";          // 원본 생성자 값 (NanoImpact2020cfeDlg.cpp:870)
        public int m_bCLOnOffFlag = TRUE;                        // 원본 생성자 값 (NanoImpact2020cfeDlg.cpp:960)
        public int m_bAutoStartConfigChengeFlag;
        public object m_pdlgMessageNotice;               // 원본 CMessageNoticeBoxDlg* (자동 시작 안내 창)
        public object m_pSEScanImageDataThreadParams;    // 원본 LPCLIENTSEIMAGEDATATHREADPARAMS
        public object m_pSEScanImageDataProcessThread;   // 원본 CWinThread*
        public int m_nSpotSizeValue;
        public int m_bPhotoCalibrationFlag;
        public int m_nWDx10Position;
        public int m_nDialogStartStep;

        public readonly IModularAlignControlDlg m_wndAlignControlDlg;
        public readonly IModularBeamControlDlg m_wndBeamControlDlg;
        public readonly IModularDetailControlDlg m_wndDetailControlDlg;
        public readonly IModularEnvironmentToolDlg m_wndEnvironmentToolDlg;
        public readonly IModularFocusControlDlg m_wndFocusControlDlg;
        public readonly IModularScanCalibrationToolDlg m_wndScanCalibrationToolDlg;
        public readonly IModularScanControlDlg m_wndScanControlDlg;
        public readonly IModularStageCalibrationToolDlg m_wndStageCalibrationToolDlg;
        public readonly IModularStageControlDlg m_wndStageControlDlg;
        public readonly IModularVacuumControlDlg m_wndVacuumControlDlg;

        // ---- 원본 메인 화면의 다른 함수 (이 범위 밖) ----

        private int GetSafeHwnd() => m_hWnd;
        private void Update1stImageInfoBitmap(int x, int y, int w, int h) => _view.Update1stImageInfoBitmap(x, y, w, h);
        private void Update2ndImageInfoBitmap(int x, int y, int w, int h) => _view.Update2ndImageInfoBitmap(x, y, w, h);
        private void DisplayVacuumStateInfo(int n) => _view.DisplayVacuumStateInfo(n);
        private void SetMainDlgMagComboIndex(int n = 0) => _view.SetMainDlgMagComboIndex(n);
        private void SetMainDlgGunHVComboIndex(int n = 0) => _view.SetMainDlgGunHVComboIndex(n);
        private int SemScanModeSelectAndScanStart(int a, int b, int c, int d, int e, int f, int g) => _view.SemScanModeSelectAndScanStart(a, b, c, d, e, f, g);
        private int SemSlow12ScanModeSelectAndScanStart(int a, int b, int c, int d, int e, int f) => _view.SemSlow12ScanModeSelectAndScanStart(a, b, c, d, e, f);
        private void SemSEScanImageDataUpdateControl() => _view.SemSEScanImageDataUpdateControl();
        private void StageXYAxisMagnificationMovingSpeedChange(int n) => _view.StageXYAxisMagnificationMovingSpeedChange(n);
        private void SetExecuteAutoProcessingStop() => _view.SetExecuteAutoProcessingStop();
        private void SetExecuteAutoFocusProcessing() => _view.SetExecuteAutoFocusProcessing();
        private void SetExecuteAutoContrastProcessing() => _view.SetExecuteAutoContrastProcessing();
        private void SetExecuteAutoContrastBrightnessProcessing() => _view.SetExecuteAutoContrastBrightnessProcessing();
        private void SetExecuteAutoBrightnessProcessing() => _view.SetExecuteAutoBrightnessProcessing();
        private int SetAutoBCCompletedFlag(int b) => _view.SetAutoBCCompletedFlag(b);
        private int GetAutoBCRunFlag() => _view.GetAutoBCRunFlag();
        private void SetTimer(int id, int ms, int _) => _view.SetTimer(id, ms);
        private void KillTimer(int id) => _view.KillTimer(id);

        // ---- IAssistOwner (ModularAssistDevice → 메인 화면) ----

        int IAssistOwner.OnSetUsbDeviceSEMControlDataToDeviceServer(int wParam, object lParam) => OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
        int IAssistOwner.OnSetUsbDeviceSEMControlDataToFlowControl(int wParam, object lParam) => OnSetUsbDeviceSEMControlDataToFlowControl(wParam, lParam);
        int IAssistOwner.OnSemControlIntegratedData(int wParam, object lParam) => OnSemControlIntegratedData(wParam, lParam);
        int IAssistOwner.OnSemStageUsbControlData(int wParam, object lParam) => OnSemStageUsbControlData(wParam, lParam);
        void IAssistOwner.SetWDSetupEnabledOLValue(int nCoarse) => SetWDSetupEnabledOLValue(nCoarse);

        private void SetWDSetupEnabledOLValue(int nCoarse) => _view.SetWDSetupEnabledOLValue(nCoarse);
    }
}
