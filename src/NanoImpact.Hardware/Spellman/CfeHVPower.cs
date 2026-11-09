// Spellman C-FE HV 전원 (RS232) 송신·수신 처리의 원본 멤버와 화면 객체 대체. 메서드 본문은 Spellman\Port.
//   송신: 원본 CModularBeamControlDlg (ModularBeamControlDlg.cpp:6771-8934) → CfeHVPowerControl
//   수신: 원본 CNanoImpact2020cfeDlg (NanoImpact2020cfeDlg.cpp:22540-22612, 29789-31999, 32100-32112) → CfeHVPowerMonitor
//   모니터 창 상태: 원본 CModularCfeHVPowerMonitorDlg (ModularCfeHVPowerMonitorDlg.cpp:1209-1733) → CfeHVPowerMonitor.CfeHVPowerMonitorDlg
#pragma warning disable IDE1006 // 원본 이름 그대로
using System;
using NanoImpact.Core.Native;
using NanoImpact.Hardware.Device;
using static NanoImpact.Core.Def;
using static NanoImpact.Core.Native.CRuntime;

namespace NanoImpact.Hardware.Spellman
{
    /// <summary>
    /// Spellman 처리가 화면으로 내보내는 것 (원본은 대화상자 컨트롤을 바로 건드렸다). 대상 이름은 원본 함수·컨트롤 이름.
    /// 실제 앱은 WPF 쪽 구현, 원본 대조 시험은 기록하는 구현을 넣는다.
    /// </summary>
    public interface ICfeHVPowerView
    {
        /// <summary>글자 칸: 모니터 창 Set*Message/Set*Text, 빔 창 SetReadRampRate*Text, 상단 바 m_TopTool*Text, 빔 창 목표값 m_CfeControl*Edit</summary>
        void SetText(string target, string text);

        /// <summary>램프 그림: SetBeamStatusBitmapImage(IDX_CFE_HV_BEAM_STATUS_*), SetEnableSupplyBitmapImage(IDX_CFE_HV_ENABLE_SUPPLY_*)</summary>
        void SetImage(string target, int index, int onOff);

        /// <summary>빔 창 목표값 스핀 m_CfeControl*Spin.SetPos</summary>
        void SetSpinPos(string control, int pos);

        /// <summary>메인 화면 받은 줄 목록 m_CfeHVPowerReceiveDataListCtrl.InsertItem(item, text)</summary>
        void ListInsertItem(int item, string text);

        /// <summary>m_CfeHVPowerReceiveDataListCtrl.SetItemText(item, subItem, text)</summary>
        void ListSetItemText(int item, int subItem, string text);

        /// <summary>모니터 창 "받은 줄 보기" m_CfeMonitorReceiveDataViewCheck.GetCheck()</summary>
        int GetReceiveDataViewCheck();

        /// <summary>빔 창 감시 타이머 (ID_TIMER_CFE_HV_POWER_*). 실제 구현은 시간이 되면 CfeHVPowerControl.OnTimer(id)를 부른다.</summary>
        void SetTimer(int id, int elapse);

        void KillTimer(int id);

        /// <summary>원본 m_pAssistDeviceTool->ExecuteFocusDegaussing(ms) — HV가 목표값에 도달했을 때</summary>
        void ExecuteFocusDegaussing(int milliseconds);
    }

    /// <summary>원본 화면 컨트롤 대체 (SetWindowTextW/SetPos를 원본 이름으로 부를 수 있게)</summary>
    public sealed class CfeHVPowerText
    {
        private readonly ICfeHVPowerView _v;
        private readonly string _name;
        internal CfeHVPowerText(ICfeHVPowerView v, string name) { _v = v; _name = name; }
        public void SetWindowTextW(string s) => _v.SetText(_name, s);
    }

    public sealed class CfeHVPowerSpin
    {
        private readonly ICfeHVPowerView _v;
        private readonly string _name;
        internal CfeHVPowerSpin(ICfeHVPowerView v, string name) { _v = v; _name = name; }
        public void SetPos(int pos) => _v.SetSpinPos(_name, pos);
    }

    /// <summary>
    /// 원본 CModularBeamControlDlg의 Spellman 송신 부분: 감시 타이머(일반 100 ms 전체 항목 / HV 동작 중 200 ms 주요 항목)마다
    /// 명령 하나씩 <c>:CC[인자] CS\n</c> 형식으로 보낸다 (CS = 0x200 - 합, &amp;0x7F, |0x40을 16진 2글자).
    /// </summary>
    public sealed partial class CfeHVPowerControl
    {
        private const int TRUE = 1;
        private const int FALSE = 0;
        private const int NULL = 0;

        // ModularBeamControlDlg.cpp:50-51
        public const int ID_TIMER_CFE_HV_POWER_ALL_MONITORING = 2002;
        public const int ID_TIMER_CFE_HV_POWER_HV_MONITORING = 2003;

        private readonly ICfeHVPowerView _view;
        private readonly Func<UsbRS232Comm> _comm;

        /// <param name="view">화면 쪽</param>
        /// <param name="comm">메인 화면이 연 포트 (원본은 빔 창 초기화 때 포인터를 한 번 복사해 두어 포트가 닫혀 지워진 뒤에도 그 포인터로 보냈다 → 매번 메인 화면 것을 본다)</param>
        public CfeHVPowerControl(ICfeHVPowerView view, Func<UsbRS232Comm> comm)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _comm = comm ?? throw new ArgumentNullException(nameof(comm));
            m_CfeControlBeamEnergyVoltageEdit = new CfeHVPowerText(view, "m_CfeControlBeamEnergyVoltageEdit");
            m_CfeControlHeaterCurrentEdit = new CfeHVPowerText(view, "m_CfeControlHeaterCurrentEdit");
            m_CfeControlSuppressorVoltageEdit = new CfeHVPowerText(view, "m_CfeControlSuppressorVoltageEdit");
            m_CfeControlExtractorVoltageEdit = new CfeHVPowerText(view, "m_CfeControlExtractorVoltageEdit");
            m_CfeControlExtractorTripCurrentEdit = new CfeHVPowerText(view, "m_CfeControlExtractorTripCurrentEdit");
            m_CfeControlBeamEnergyVoltageSpin = new CfeHVPowerSpin(view, "m_CfeControlBeamEnergyVoltageSpin");
            m_CfeControlHeaterCurrentSpin = new CfeHVPowerSpin(view, "m_CfeControlHeaterCurrentSpin");
            m_CfeControlSuppressorVoltageSpin = new CfeHVPowerSpin(view, "m_CfeControlSuppressorVoltageSpin");
            m_CfeControlExtractorVoltageSpin = new CfeHVPowerSpin(view, "m_CfeControlExtractorVoltageSpin");
            m_CfeControlExtractorTripCurrentSpin = new CfeHVPowerSpin(view, "m_CfeControlExtractorTripCurrentSpin");
        }

        // ---- 원본 멤버 (ModularBeamControlDlg.h, 생성자 값) ----

        public UsbRS232Comm m_pUsbRS232Comm => _comm();
        public int m_bWindowInitStartFlag = FALSE;       // 창 초기화 끝 (원본 SetDialogStart에서 TRUE)
        public int m_nCfeHVPowerMonitoringMode = -1;     // 0 = 일반모드, 1 = HV(스캔)모드
        public int m_nCfeHVPowerAllMonitoringIndex;
        public int m_nCfeHVPowerHVMonitoringIndex;
        public int m_nCfeHVPowerOneTimeMonitoring;
        public int m_bSetBeamTargetValueFlag = FALSE;    // 목표값은 처음 한 번만 편집 칸에 넣는다
        public int m_bSetHeaterTargetValueFlag = FALSE;
        public int m_bSetSuppressorTargetValueFlag = FALSE;
        public int m_bSetExtractorTargetValueFlag = FALSE;
        public int m_bSetExtractorTripTargetValueFlag = FALSE;

        public readonly CfeHVPowerText m_CfeControlBeamEnergyVoltageEdit;
        public readonly CfeHVPowerText m_CfeControlHeaterCurrentEdit;
        public readonly CfeHVPowerText m_CfeControlSuppressorVoltageEdit;
        public readonly CfeHVPowerText m_CfeControlExtractorVoltageEdit;
        public readonly CfeHVPowerText m_CfeControlExtractorTripCurrentEdit;
        public readonly CfeHVPowerSpin m_CfeControlBeamEnergyVoltageSpin;
        public readonly CfeHVPowerSpin m_CfeControlHeaterCurrentSpin;
        public readonly CfeHVPowerSpin m_CfeControlSuppressorVoltageSpin;
        public readonly CfeHVPowerSpin m_CfeControlExtractorVoltageSpin;
        public readonly CfeHVPowerSpin m_CfeControlExtractorTripCurrentSpin;

        /// <summary>원본 SetDialogStart 끝 (m_bWindowInitStartFlag = TRUE)</summary>
        public void SetDialogStart() => m_bWindowInitStartFlag = TRUE;

        /// <summary>원본 SetCfeHVPowerMonitoringStart (ModularBeamControlDlg.cpp:477) — 메인 화면 시작 9단계에서 부른다</summary>
        public void SetCfeHVPowerMonitoringStart()
        {
            // CFE HV POWER MONITORING : NORMAL MODE
            SetCfeHVPowerMonitoringTimerExecute(0);
        }

        /// <summary>원본 OnTimer의 Spellman 감시 두 갈래 (ModularBeamControlDlg.cpp:856-867)</summary>
        public void OnTimer(int nIDEvent)
        {
            if (nIDEvent == ID_TIMER_CFE_HV_POWER_ALL_MONITORING)
            {
                // 일반모드 (BEAM, FILAMENT, EXTRACTOR, SUPPRESSOR 준비상태)
                ExecuteCfeHVPowerAllDataMonitoring();
            }
            else if (nIDEvent == ID_TIMER_CFE_HV_POWER_HV_MONITORING)
            {
                // 스캔모드 (스캔작업시 빠른 처리를 위해 중요한 DATA만 모니터링 한다)
                ExecuteCfeHVPowerHVProcessDataMonitoring();
            }
        }

        // ---- 원본 빔 창의 화면 함수 (ModularBeamControlDlg.cpp:6617-6770) ----

        public void SetEnableSupplyBitmapImage(int nImageIndex, int nOnOff) => _view.SetImage("SetEnableSupplyBitmapImage", nImageIndex, nOnOff);
        public void SetReadRampRateUpBeamEnergyText(string s) => _view.SetText("SetReadRampRateUpBeamEnergyText", s);
        public void SetReadRampRateUpHeaterCurrentText(string s) => _view.SetText("SetReadRampRateUpHeaterCurrentText", s);
        public void SetReadRampRateUpSuppressorVoltText(string s) => _view.SetText("SetReadRampRateUpSuppressorVoltText", s);
        public void SetReadRampRateUpExtractorVoltageText(string s) => _view.SetText("SetReadRampRateUpExtractorVoltageText", s);
        public void SetReadRampRateDownBeamEnergyText(string s) => _view.SetText("SetReadRampRateDownBeamEnergyText", s);
        public void SetReadRampRateDownHeaterCurrentText(string s) => _view.SetText("SetReadRampRateDownHeaterCurrentText", s);
        public void SetReadRampRateDownSuppressorVoltText(string s) => _view.SetText("SetReadRampRateDownSuppressorVoltText", s);
        public void SetReadRampRateDownExtractorVoltageText(string s) => _view.SetText("SetReadRampRateDownExtractorVoltageText", s);

        private void SetTimer(int id, int elapse, int _) => _view.SetTimer(id, elapse);
        private void KillTimer(int id) => _view.KillTimer(id);

        /// <summary>원본 shlwapi StrToInt: 앞 공백, '-' 하나, 10진 숫자 (그 밖의 글자에서 멈춤)</summary>
        private static int StrToInt(string s)
        {
            s = s ?? "";
            int i = 0;
            while (i < s.Length && (s[i] == ' ' || (s[i] >= '\t' && s[i] <= '\r'))) i++;
            bool neg = i < s.Length && s[i] == '-';
            if (neg) i++;
            int v = 0;
            for (; i < s.Length && s[i] >= '0' && s[i] <= '9'; i++) v = unchecked(v * 10 + (s[i] - '0'));
            return neg ? -v : v;
        }
    }

    /// <summary>
    /// 원본 CNanoImpact2020cfeDlg의 Spellman 수신 부분: COM 포트를 열고, 받는 스레드가 보낸 바이트를 LF까지 모아
    /// <c>:CC 인자... CS\n</c> 응답을 명령별로 해석해 모니터 창·빔 창·상단 바에 표시한다.
    /// </summary>
    public sealed partial class CfeHVPowerMonitor
    {
        private const int TRUE = 1;
        private const int FALSE = 0;
        private const int NULL = 0;
        private const uint MB_OK = 0x00000000;
        private const uint MB_ICONERROR = 0x00000010;
        private const int MAX_CFE_HV_POWER_COMMAND = UsbRS232Comm.MAX_CFE_HV_POWER_COMMAND;
        private static readonly string[] m_szFEPowerCommand = UsbRS232Comm.m_szFEPowerCommand;

        private readonly ICfeHVPowerView _view;
        private readonly IDeviceHost _host;
        private readonly ISerialCommApi _api;

        /// <param name="view">화면 쪽</param>
        /// <param name="host">창 메시지·스레드 (받는 스레드가 UM_CFE_HV_POWER_CONTROL_*를 hWnd로 보낸다)</param>
        /// <param name="api">직렬 포트 함수</param>
        /// <param name="hWnd">메인 화면 창 번호 (원본 GetSafeHwnd())</param>
        public CfeHVPowerMonitor(ICfeHVPowerView view, IDeviceHost host, ISerialCommApi api, int hWnd)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _host = host ?? throw new ArgumentNullException(nameof(host));
            _api = api ?? throw new ArgumentNullException(nameof(api));
            m_hWnd = hWnd;
            m_wndBeamControlDlg = new CfeHVPowerControl(view, () => m_pUsbRS232Comm);
            m_wndCfeHVPowerMonitorDlg = new CfeHVPowerMonitorDlg(view);
            m_TopToolFEPowerStatusText = new CfeHVPowerText(view, "m_TopToolFEPowerStatusText");
            m_TopToolExtractorCurrentText = new CfeHVPowerText(view, "m_TopToolExtractorCurrentText");
            m_CfeHVPowerReceiveDataListCtrl = new ReceiveDataListCtrl(view);
        }

        // ---- 원본 멤버 (NanoImpact2020cfeDlg.h, 생성자 값) ----

        public readonly int m_hWnd;
        public UsbRS232Comm m_pUsbRS232Comm;
        public int m_bUsbRS232ComPortOpen = FALSE;
        public int m_nReceiveDataListItemIndex = 0;
        public int m_nCfeHVPowerMainOutputStatus = CFE_OUTPUT_STATUS_MAIN_NO_STATUS;
        public int m_nCfeHVPowerErrorOutputStatus = CFE_OUTPUT_STATUS_NO_ERROR;
        public readonly CharBuffer m_szCfeHVPowerDataBuffer = new CharBuffer(MAX_RS232_BUFFER);
        public string m_strExtratorCurrent = "";   // 원본 오타 그대로 (추출기 전류 기록용)

        public readonly CfeHVPowerControl m_wndBeamControlDlg;
        public readonly CfeHVPowerMonitorDlg m_wndCfeHVPowerMonitorDlg;
        public readonly CfeHVPowerText m_TopToolFEPowerStatusText;
        public readonly CfeHVPowerText m_TopToolExtractorCurrentText;
        public readonly ReceiveDataListCtrl m_CfeHVPowerReceiveDataListCtrl;

        /// <summary>
        /// 원본 OnInitDialog의 COM 포트 열기 (NanoImpact2020cfeDlg.cpp:1473-1495). nComPort = CLIENTPUBLICDATA.INI의
        /// nOptionData[IDX_CLIENT_OPTION_INT_SEM_CFE_COM_PORT]. 115200 8N1.
        /// </summary>
        public void CfeHVPowerUsbRS232ComPortOpen(int nComPort)
        {
            m_bUsbRS232ComPortOpen = FALSE;

            // CFE HV POWER CONTROL 용 COM PORT 확인
            string strComPort = GetCfeHVPowerControlAndMonitorComPort(nComPort);

            m_pUsbRS232Comm = new UsbRS232Comm(_api, _host, strComPort, "115200", "None", "8Bit", "1Bit");
            if (m_pUsbRS232Comm.Create(m_hWnd, UsbRS232Comm.RS232_MONITORING_MODE_FE_POWER) == FALSE)
            {
                // CFE HV POWER COM PORT OPEN FAILE
                m_bUsbRS232ComPortOpen = FALSE;
                // COM CLASS 삭제
                m_pUsbRS232Comm.Destroy();
                m_pUsbRS232Comm = null;
                // MESSAGE DISPLAY
                _host.MessageBox(0, "RS232 port for FE HV POWER cannot be opened.!!!", "ERROR", MB_ICONERROR | MB_OK);
            }
            else
            {
                // CFE HV POWER COM PORT OPEN SUCCESS
                m_bUsbRS232ComPortOpen = TRUE;
            }
        }

        /// <summary>원본 OnClose의 COM 포트 닫기 (NanoImpact2020cfeDlg.cpp:2798-2805). 클래스는 받는 스레드가 끝나며 지운다.</summary>
        public void CfeHVPowerUsbRS232ComPortClose()
        {
            m_bUsbRS232ComPortOpen = FALSE;
            if (m_pUsbRS232Comm != null)
            {
                m_pUsbRS232Comm.Close();
                // NULL 설정 후, 쓰레드에서 메세지 호출 후 자동삭제됨
                m_pUsbRS232Comm = null;
            }
        }

        /// <summary>원본 GetCfeHVPowerControlAndMonitorComPort (NanoImpact2020cfeDlg.cpp:29789)</summary>
        public string GetCfeHVPowerControlAndMonitorComPort(int nComPort)
        {
            // CFE HV POWER CONTROL 용 COM PORT 확인
            return Format("\\\\.\\COM%d", nComPort);
        }

        /// <summary>원본 GetCfeHVPowerGroundFirmwareVersion (NanoImpact2020cfeDlg.cpp:32100) — 모니터 창 버튼</summary>
        public void GetCfeHVPowerGroundFirmwareVersion()
        {
            // CFE HV POWER : 0 = Main Controller (Gateway)
            m_wndBeamControlDlg.SendingCfeHVPower05Command(0);
        }

        /// <summary>원본 GetCfeHVPowerHVFirmwareVersion (NanoImpact2020cfeDlg.cpp:32107) — 모니터 창 버튼</summary>
        public void GetCfeHVPowerHVFirmwareVersion()
        {
            // CFE HV POWER : 1 = Floating Deck (extractor, suppressor, heater)
            m_wndBeamControlDlg.SendingCfeHVPower05Command(1);
        }

        public sealed class ReceiveDataListCtrl
        {
            private readonly ICfeHVPowerView _v;
            internal ReceiveDataListCtrl(ICfeHVPowerView v) { _v = v; }
            public int InsertItem(int nItem, string text) { _v.ListInsertItem(nItem, text); return nItem; }
            public int SetItemText(int nItem, int nSubItem, string text) { _v.ListSetItemText(nItem, nSubItem, text); return TRUE; }
        }

        /// <summary>
        /// 원본 CModularCfeHVPowerMonitorDlg: 표시 함수는 화면으로 넘기고, 빔 전압 램핑·목표 도달 상태
        /// (ModularCfeHVPowerMonitorDlg.cpp:1664-1733)는 여기서 그대로 계산한다 — 램핑 뒤 처음 목표에 닿으면 포커스 디가우징.
        /// </summary>
        public sealed class CfeHVPowerMonitorDlg
        {
            private readonly ICfeHVPowerView _v;

            internal CfeHVPowerMonitorDlg(ICfeHVPowerView v)
            {
                _v = v;
                m_CfeMonitorReceiveDataViewCheck = new CheckBox(v);
            }

            public int m_nBeamRampingState = 0; // 0->None State, 1->RAMPING UP, 2->RAMPING DOWN
            public int m_nBeamOnTargetFlag = 0; // 0->None State, 1->On Target
            public readonly CheckBox m_CfeMonitorReceiveDataViewCheck;

            public sealed class CheckBox
            {
                private readonly ICfeHVPowerView _v;
                internal CheckBox(ICfeHVPowerView v) { _v = v; }
                public int GetCheck() => _v.GetReceiveDataViewCheck();
            }

            public void SetBeamStatusBitmapImage(int nImageIndex, int nOnOff) => _v.SetImage("SetBeamStatusBitmapImage", nImageIndex, nOnOff);
            public void SetBeamEnergyStatusMessage(string s) => _v.SetText("SetBeamEnergyStatusMessage", s);
            public void SetHeaterSupplyStatusMessage(string s) => _v.SetText("SetHeaterSupplyStatusMessage", s);
            public void SetSuppressorSupplyStatusMessage(string s) => _v.SetText("SetSuppressorSupplyStatusMessage", s);
            public void SetExtractorSupplyStatusMessage(string s) => _v.SetText("SetExtractorSupplyStatusMessage", s);
            public void SetConnectionStatusMessage(string s) => _v.SetText("SetConnectionStatusMessage", s);
            public void SetOutputStatusMessage(string s) => _v.SetText("SetOutputStatusMessage", s);
            public void SetGroundFirmwareVersText(string s) => _v.SetText("SetGroundFirmwareVersText", s);
            public void SetHVFirmwareVersText(string s) => _v.SetText("SetHVFirmwareVersText", s);
            public void SetBeamVoltageMonitorText(string s) => _v.SetText("SetBeamVoltageMonitorText", s);
            public void SetBeamVoltageTargetText(string s) => _v.SetText("SetBeamVoltageTargetText", s);
            public void SetBeamCurrentMonitorText(string s) => _v.SetText("SetBeamCurrentMonitorText", s);
            public void SetExtractorVoltageMonitorText(string s) => _v.SetText("SetExtractorVoltageMonitorText", s);
            public void SetExtractorVoltageTargetText(string s) => _v.SetText("SetExtractorVoltageTargetText", s);
            public void SetExtractorCurrentMonitorText(string s) => _v.SetText("SetExtractorCurrentMonitorText", s);
            public void SetExtractorTripCurrentMonitorText(string s) => _v.SetText("SetExtractorTripCurrentMonitorText", s);
            public void SetSuppressorVoltageMonitorText(string s) => _v.SetText("SetSuppressorVoltageMonitorText", s);
            public void SetSuppressorVoltageTargetText(string s) => _v.SetText("SetSuppressorVoltageTargetText", s);
            public void SetSuppressorCurrentMonitorText(string s) => _v.SetText("SetSuppressorCurrentMonitorText", s);
            public void SetHeaterCurrentMonitorText(string s) => _v.SetText("SetHeaterCurrentMonitorText", s);
            public void SetHeaterCurrentTargetText(string s) => _v.SetText("SetHeaterCurrentTargetText", s);
            public void SetHeaterVoltageMonitorText(string s) => _v.SetText("SetHeaterVoltageMonitorText", s);

            // ---- ModularCfeHVPowerMonitorDlg.cpp:1664 ----
            public void ResetBeamVoltageRampingOnTarget()
            {
                // BEAM HV : RAMPING UP/DOWN , ON TARGET FLAG
                m_nBeamRampingState = 0; // 0->None State, 1->RAMPING UP, 2->RAMPING DOWN
                m_nBeamOnTargetFlag = 0; // 0->None State, 1->On Target
            }

            // ---- ModularCfeHVPowerMonitorDlg.cpp:1674 ----
            public void SetBeamVoltageRampingUpDown(int nRamping)
            {
                if (nRamping == 1)
                {
                    // BEAM : RAMPING UP
                    m_nBeamRampingState = 1;
                    m_nBeamOnTargetFlag = 0;
                }
                else if (nRamping == 2)
                {
                    // BEAM : RAMPING DOWN
                    m_nBeamRampingState = 2;
                    m_nBeamOnTargetFlag = 0;
                }
            }

            // ---- ModularCfeHVPowerMonitorDlg.cpp:1695 ----
            public void SetBeamVoltageOnTarget(int nOnTarget)
            {
                if (nOnTarget == 1)
                {
                    // BEAM : SWITCH ON & ON TARGET
                    // ON TARGET 이전에 RAMP UP or DOWN 작업을 실행 했는지 확인한다
                    if (m_nBeamRampingState == 1 || m_nBeamRampingState == 2)
                    {
                        // RAMPING UP or DOWN 작업 후, 처음으로 ON TARGET 명령을 실행 하는지 확인한다
                        if (m_nBeamOnTargetFlag == 0)
                        {
                            // ON TARGET, RAMPING RESET
                            m_nBeamRampingState = 0;
                            m_nBeamOnTargetFlag = 1;

                            // DEGAUSSING 실행
                            BeamVoltageTargetExecuteDegaussing(1);
                        }
                    }
                }
            }

            // ---- ModularCfeHVPowerMonitorDlg.cpp:1723 ----
            public void BeamVoltageTargetExecuteDegaussing(int nDegaussing)
            {
                // BEAM VOLTAGE TARGET -> FOCUS DEGAUSSING EXECUTE
                if (nDegaussing == 1)
                {
                    // HV PROCESSING 완료 후 또는 Z축 이동시 FOCUS DEGAUSSING 실행
                    _v.ExecuteFocusDegaussing(1000); // 1000[ms]
                }
            }
        }
    }
}
