// 원본 ModularAssistDevice.cpp (CModularAssistDevice)의 멤버·생성자. 메서드 154개는 같은 폴더의 AssistDevice.L*.cs (번역 후 손질, 다시 생성하지 않음).
#pragma warning disable IDE1006, CS0414, CS0649 // 원본 이름·원본에서 안 쓰는 멤버 그대로
using System;
using NanoImpact.Core.Native;

namespace NanoImpact.Hardware.Device
{
    /// <summary>
    /// 원본 메인 화면(CNanoImpact2020cfeDlg) 중 ModularAssistDevice가 m_pOwnerWindow로 부르는 것.
    /// 메인 화면의 장치 처리기(6b)가 구현한다.
    /// </summary>
    public interface IAssistOwner
    {
        /// <summary>원본 OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, &amp;REQUEST_USB_DEVICE_DATA) — 메인 화면 공용 값·Detail 표시 갱신</summary>
        int OnSetUsbDeviceSEMControlDataToDeviceServer(int wParam, object lParam);

        /// <summary>원본 OnSetUsbDeviceSEMControlDataToFlowControl — SEM 시작·대기·종료 흐름</summary>
        int OnSetUsbDeviceSEMControlDataToFlowControl(int wParam, object lParam);

        /// <summary>원본 OnSemControlIntegratedData(크기, 공용 데이터) — 장치 연결 직후 공용 데이터 표시</summary>
        int OnSemControlIntegratedData(int wParam, object lParam);

        /// <summary>원본 OnSemStageUsbControlData(1, null) — 시작할 때 Stage 설정값 읽기 타이머</summary>
        int OnSemStageUsbControlData(int wParam, object lParam);

        /// <summary>원본 SetWDSetupEnabledOLValue(nCoarse)</summary>
        void SetWDSetupEnabledOLValue(int nCoarse);
    }

    /// <summary>
    /// 원본 CModularAssistDevice: 패널이 장비를 다룰 때 부르는 명령 묶음. 장치 계층(SemDevice의 Afx* 함수)을 부르거나,
    /// REQUEST_USB_DEVICE_DATA를 만들어 메인 화면 처리기(IAssistOwner)로 넘긴다. 원본 전역 Afx* 호출은 m_dev.Afx*로 옮겼다.
    /// </summary>
    public sealed partial class AssistDevice
    {
        private const int TRUE = 1;
        private const int FALSE = 0;
        private const int NULL = 0;
        private const uint MB_OK = 0x00000000;

        private readonly SemDevice m_dev;
        private readonly IDeviceHost _host;

        public AssistDevice(SemDevice device, IDeviceHost host)
        {
            m_dev = device ?? throw new ArgumentNullException(nameof(device));
            _host = host ?? throw new ArgumentNullException(nameof(host));

            // SET OWNER WINDOW HANDLE
            m_hOwnerWnd = 0;
            m_pOwnerWindow = null;
            // Assist Public Tool 설정
            m_pAssistPublicTool = null;
            // VACUUM, BEAM, ALIGN, FOCUS Public Data 설정
            m_pVacuumBeamAlignFocusPublicData = null;
            // FT232H USB DEVICE CONTROL
            m_bSbrConnected = FALSE;    // FT232H USB [SCAN BRIDGE PORT] : CONNECT FLAG
            m_bSemConnected = FALSE;    // FT232H USB [SCAN IMAGE  PORT] : CONNECT FLAG
            m_bBseConnected = FALSE;    // FT232H USB [BSE  IMAGE  PORT] : CONNECT FLAG
            m_bUsbConnected = FALSE;    // FT232H USB DEVICE : CONNECT FLAG
            m_bSysInitilized = FALSE;   // FT232H USB DEVICE : SYSTEM INITILIZE FLAG
            // SE MODE 설정
            m_nSeBseScanMode = 0;
        }

        // ---- 원본 멤버 (ModularAssistDevice.h) ----

        public object m_pAssistPublicTool;                                    // ASSIST PUBLIC TOOL CLASS (이 클래스에서는 쓰지 않음)
        public VACUUM_BEAM_ALIGN_FOCUS_DATA m_pVacuumBeamAlignFocusPublicData; // VACUUM, BEAM, ALIGN, FOCUS PUBLIC DATA
        public int m_hOwnerWnd;                                                // OWNER WINDOW HANDLE
        public IAssistOwner m_pOwnerWindow;                                    // OWNER WINDOW CLASS
        public int m_bSbrConnected;
        public int m_bSemConnected;
        public int m_bBseConnected;
        public int m_bUsbConnected;
        public int m_bSysInitilized;
        public int m_nSeBseScanMode;                                           // SE / BSE SCAN MODE SELECT

        /// <summary>원본 AfxMessageBox(text) — 앱 제목 캡션, MB_OK</summary>
        private int AfxMessageBox(string text) => _host.MessageBox(0, text, "", MB_OK);
    }
}
