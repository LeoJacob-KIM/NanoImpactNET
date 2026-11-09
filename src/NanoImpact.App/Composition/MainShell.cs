// 원본 메인 화면 CNanoImpact2020cfeDlg의 뼈대 (7단계): 장치 계층·영상 조립·Spellman 감시를 만들고 잇는 곳(조립 루트),
// 창 1번 메시지 분배, 메인 화면 타이머(OnTimer), 시작 순서(OnInitDialog + 타이머 98 단계 0~9), 종료 순서(OnClose/OnDestroy).
// 화면 쪽 인터페이스 구현은 MainShell.*View.cs, 패널은 8단계 전까지 임시 패널(Placeholders).
#pragma warning disable IDE1006 // 원본 이름 그대로
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using NanoImpact.App.Placeholders;
using NanoImpact.App.Shell;
using NanoImpact.App.Sim;
using NanoImpact.Core;
using NanoImpact.Core.Native;
using NanoImpact.Hardware.Device;
using NanoImpact.Hardware.Ftdi;
using NanoImpact.Hardware.Imaging;
using NanoImpact.Hardware.Spellman;
using static NanoImpact.Core.Def;

namespace NanoImpact.App.Composition
{
    internal sealed partial class MainShell : IPanelSink
    {
        private const int TRUE = 1;
        private const int FALSE = 0;

        // 원본 창 번호 (DeviceHost 등록): 메인 화면, Scan 보정 창(HW setup), Stage 보정 창
        public const int HWND_MAIN = 1;
        public const int HWND_HW_SETUP = 3;
        public const int HWND_STAGE_SETUP = 4;

        // NanoImpact2020cfeDlg.cpp:408-436
        private const int TIMER_ID_SE_SCAN_IMAGE_DISPLAY = 10;
        private const int TIMER_ID_SE_SCAN_IMAGE_DISPLAY_NEXT = 11;
        private const int TIMER_ID_SE_PHOTO12_SCAN_IMAGE_DISPLAY = 20;
        private const int TIMER_ID_SE_PHOTO345_SCAN_IMAGE_DISPLAY = 21;
        private const int TIMER_ID_SE_PHOTO6_SCAN_IMAGE_DISPLAY = 22;
        private const int TIMER_ID_BSE_SCAN_IMAGE_DISPLAY = 30;
        private const int TIMER_ID_MAG_XY_AXIS_ACCEL_CHANGE = 40;
        private const int TIMER_ID_AUTO_START_PROCESS = 50;
        private const int TIMER_ID_VACUUM_STATE_UPDATE = 90;
        private const int TIMER_ID_DIALOG_START_PROCESS = 98;
        private const int TIMER_ID_DIALOG_PROGRAM_CLOSE = 99;
        private const int TIMER_ID_JOYSTICK_STATE_PROCESS = 100;
        private const int SE_SCAN_IMAGE_UPDATE_ELAPSE_TIME = 50;
        private const int VACUUM_STATE_PROCESS_ELAPSE_TIME = 500;

        private readonly MainWindow _w;
        private readonly bool _sim;
        private readonly string _iniFolder;

        private readonly DeviceHost _host;
        private readonly SemDevice _dev;
        private readonly AssistDevice m_pAssistDeviceTool;
        private readonly MainDeviceService _main;
        private readonly SemScanImageProcessor _proc;
        private readonly CfeHVPowerMonitor _cfe;
        private readonly SimFtdi _simFtdi;
        private readonly SimScanSource _simScan;
        private readonly SEM_SE_SCAN_CONTROL_PUBLIC_DATA m_SemSEScanControlPublicData = new SEM_SE_SCAN_CONTROL_PUBLIC_DATA();

        private readonly TimerTable _mainTimers;   // 메인 화면 SetTimer
        private readonly TimerTable _beamTimers;   // Beam 창 Spellman 감시 타이머 (원본 CModularBeamControlDlg)

        // ---- 원본 메인 화면 멤버 (이 단계에서 쓰는 것) ----
        private int m_nDialogStartStep;
        private int m_bFirstStageInitFlag;
        private int m_bProgramCloseTimerFlag;
        private int m_nProgramCloseFlagCode;
        private int m_nVacuumStateBlinkFlag;
        private int m_nVacuumStateBlinkCount;
        private string m_strVacuumState = "SEM STATE";
        private bool _closing;
        private bool _closed;

        public readonly ObservableCollection<string> PanelCalls = new ObservableCollection<string>();
        public readonly ObservableCollection<string> Log = new ObservableCollection<string>();

        public MainShell(MainWindow window, bool sim, string iniFolder)
        {
            _w = window ?? throw new ArgumentNullException(nameof(window));
            _sim = sim;
            _iniFolder = iniFolder;
            Dispatcher ui = window.Dispatcher;

            _host = new DeviceHost(SynchronizationContext.Current ?? new DispatcherSynchronizationContext(ui), ShowMessageBox);
            _host.ThreadFailed += (name, ex) => ui.BeginInvoke(new Action(() => AddLog("THREAD FAILED " + name + ": " + ex.GetBaseException().Message)));
            _host.UnhandledMessage += (hWnd, msg) => AddLog("unhandled message hWnd " + hWnd + " msg " + msg);

            _mainTimers = new TimerTable(ui, OnTimer);
            _beamTimers = new TimerTable(ui, id => _cfe.m_wndBeamControlDlg.OnTimer(id));

            if (sim)
            {
                _simFtdi = new SimFtdi();
                _simScan = new SimScanSource(_simFtdi);
            }
            _dev = new SemDevice(_host, sim ? (IFtdi)_simFtdi : new Ftd2xxApi());
            m_pAssistDeviceTool = new AssistDevice(_dev, _host);
            _main = new MainDeviceService(_dev, m_pAssistDeviceTool, new MainView(this), PanelStubs.Create(this), m_SemSEScanControlPublicData, HWND_MAIN);
            _proc = new SemScanImageProcessor(new ScanView(this), () => _dev.AfxGetIntegratedPointer(),
                new ScanDeviceAdapter(m_pAssistDeviceTool, _dev, _main, _simScan), m_SemSEScanControlPublicData);
            _cfe = new CfeHVPowerMonitor(new CfeView(this), _host, new SerialCommApi(), HWND_MAIN);

            _host.RegisterWindow(HWND_MAIN, MainWindowProc);
            _host.RegisterWindow(HWND_HW_SETUP, (msg, w, l) => { PanelLine("hwnd3 (Scan calibration window) msg " + msg + " " + w); return 0; });
            _host.RegisterWindow(HWND_STAGE_SETUP, (msg, w, l) => { PanelLine("hwnd4 (Stage calibration window) msg " + msg + " " + w); return 0; });
        }

        public SemScanImageProcessor Processor => _proc;
        public MainDeviceService Main => _main;
        public bool IsSimulator => _sim;

        /// <summary>
        /// 원본 OnInitDialog의 장치 쪽 (NanoImpact2020cfeDlg.cpp:1355-1875 중 화면 그리기 외):
        /// ftd2xx 로드, 공용 데이터, CLIENTPUBLICDATA.INI, Spellman COM, 진공 표시 "WAITING", 시작 타이머 98.
        /// </summary>
        public void Start()
        {
            AddLog("start: " + (_sim ? "SIMULATOR" : "hardware") + ", INI folder " + _iniFolder);
            _dev.AfxUSBFT232HLoadDLL();
            _dev.AfxCreateIntegratedCommand();

            // CreateClientPublicData + GetClientPublicDataFromFile + SetClientPublicData
            string clientIni = Path.Combine(_iniFolder, "CLIENTPUBLICDATA.INI");
            if (File.Exists(clientIni))
            {
                _main.m_ClientPublicData.LoadFile(clientIni);
                AddLog("CLIENTPUBLICDATA.INI loaded");
            }
            else AddLog("CLIENTPUBLICDATA.INI not found: " + clientIni);

            // Spellman RS232 (115200 8N1) — 시뮬레이터는 열지 않는다
            if (!_sim)
            {
                _cfe.CfeHVPowerUsbRS232ComPortOpen(_main.m_ClientPublicData.nOptionData[IDX_CLIENT_OPTION_INT_SEM_CFE_COM_PORT]);
                _w.CfePortText.Text = _cfe.m_bUsbRS232ComPortOpen == TRUE ? _cfe.m_pUsbRS232Comm?.ToString() ?? "open" : "closed (open failed)";
            }
            else _w.CfePortText.Text = "not opened (simulator)";

            DisplayVacuumStateInfo(-1000);

            // DIALOG START TIMER (원본은 OnInitDialog 끝에서 단계 0으로 건다)
            m_nDialogStartStep = 0;
            _mainTimers.Set(TIMER_ID_DIALOG_START_PROCESS, 100);
        }

        /// <summary>원본 DialogStart (단계 0): 장치 서버 경로·소유 창 설정 후 USB 연결</summary>
        private void DialogStart()
        {
            _mainTimers.Kill(TIMER_ID_DIALOG_START_PROCESS);
            _dev.AfxSetDeviceServerPath(_iniFolder);
            _dev.AfxUSBFT232HSetOwnerWindow(HWND_MAIN);
            _dev.AfxUSBFT232HSetHWSetupControlWindow(HWND_HW_SETUP);
            _dev.AfxUSBFT232HSetStageSetupControlWindow(HWND_STAGE_SETUP);
            m_pAssistDeviceTool.UsbDeviceConnect();
            AddLog("UsbDeviceConnect done");
            m_nDialogStartStep = 1;
            _mainTimers.Set(TIMER_ID_DIALOG_START_PROCESS, 100);
        }

        /// <summary>원본 DialogStartStep1: CNC 송신 스레드 → 2초 뒤 단계 2</summary>
        private void DialogStartStep1()
        {
            _mainTimers.Kill(TIMER_ID_DIALOG_START_PROCESS);
            if (m_bFirstStageInitFlag == FALSE)
            {
                m_bFirstStageInitFlag = TRUE;
                _dev.AfxCreateCNCSendThread();
            }
            m_nDialogStartStep = 2;
            _mainTimers.Set(TIMER_ID_DIALOG_START_PROCESS, 2000);
        }

        /// <summary>원본 OnTimer (NanoImpact2020cfeDlg.cpp:3384) 중 이 단계 범위</summary>
        private void OnTimer(int nIDEvent)
        {
            if (nIDEvent == TIMER_ID_SE_SCAN_IMAGE_DISPLAY)
            {
                _mainTimers.Kill(TIMER_ID_SE_SCAN_IMAGE_DISPLAY);
                // 자동 시작·자동 기능 중이면 화면 갱신 타이머를 다시 걸지 않는다 (9단계)
                if (m_SemSEScanControlPublicData.m_bAutoStartFlag == TRUE || _proc.m_nAutoProcessRunStatus == AUTO_PROCESS_RUN_STATE)
                {
                }
                else
                {
                    SemSEScanImageDataUpdateControl();
                    _mainTimers.Set(TIMER_ID_SE_SCAN_IMAGE_DISPLAY_NEXT, SE_SCAN_IMAGE_UPDATE_ELAPSE_TIME);
                }
            }
            else if (nIDEvent == TIMER_ID_SE_SCAN_IMAGE_DISPLAY_NEXT) SemSEScanImageDataUpdateControl();
            else if (nIDEvent == TIMER_ID_SE_PHOTO12_SCAN_IMAGE_DISPLAY) SemSEPhotoScanImageDataUpdateControl(_proc.m_2ndImage640x480Bitmap);
            else if (nIDEvent == TIMER_ID_SE_PHOTO345_SCAN_IMAGE_DISPLAY) SemSEPhotoScanImageDataUpdateControl(_proc.m_2ndImage1280x960Bitmap);
            else if (nIDEvent == TIMER_ID_SE_PHOTO6_SCAN_IMAGE_DISPLAY) SemSEPhotoScanImageDataUpdateControl(_proc.m_2ndImage2560x1920Bitmap);
            else if (nIDEvent == TIMER_ID_BSE_SCAN_IMAGE_DISPLAY) { }
            else if (nIDEvent == TIMER_ID_AUTO_START_PROCESS) AddLog("TIMER_ID_AUTO_START_PROCESS (auto start state machine: step 9)");
            else if (nIDEvent == TIMER_ID_MAG_XY_AXIS_ACCEL_CHANGE)
            {
                _mainTimers.Kill(TIMER_ID_MAG_XY_AXIS_ACCEL_CHANGE);
                StageXYAxisMagnificationMovingSpeedChange(_main.m_IntegratedData.nMagStepIndex);
            }
            else if (nIDEvent == TIMER_ID_VACUUM_STATE_UPDATE)
            {
                m_nVacuumStateBlinkCount++;
                if (m_nVacuumStateBlinkCount > 1) m_nVacuumStateBlinkCount = 0;
                UpdateVacuumStateInfoData(m_nVacuumStateBlinkCount);
                if (m_nVacuumStateBlinkFlag == 0) _mainTimers.Kill(TIMER_ID_VACUUM_STATE_UPDATE);
            }
            else if (nIDEvent == TIMER_ID_DIALOG_START_PROCESS) DialogStartProcess();
            else if (nIDEvent == TIMER_ID_DIALOG_PROGRAM_CLOSE)
            {
                _mainTimers.Kill(TIMER_ID_DIALOG_PROGRAM_CLOSE);
                _w.Close(); // 원본 SendMessage(WM_CLOSE) → OnClose의 m_bProgramCloseTimerFlag 갈래
            }
            else if (nIDEvent == TIMER_ID_JOYSTICK_STATE_PROCESS) { } // 조이스틱 (10단계)
            else AddLog("timer " + nIDEvent + " (not in step 7)");
        }

        /// <summary>원본 OnTimer TIMER_ID_DIALOG_START_PROCESS 갈래 (단계 0~9)</summary>
        private void DialogStartProcess()
        {
            _w.StartStepText.Text = "start step " + m_nDialogStartStep;
            if (m_nDialogStartStep == 0)
            {
                DialogStart();
                _main.m_VacuumBeamAlignFocusData.m_nAirToEvacWaitTimerState = 1; // 진공이 완료 될 때까지 WAIT 상태 설정
            }
            else if (m_nDialogStartStep == 1) DialogStartStep1();
            else if (m_nDialogStartStep == 2)
            {
                _mainTimers.Kill(TIMER_ID_DIALOG_START_PROCESS);
                _dev.AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RM0;");
                m_nDialogStartStep = 3;
                _mainTimers.Set(TIMER_ID_DIALOG_START_PROCESS, 100);
            }
            else if (m_nDialogStartStep == 3) { _dev.AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RM1;"); m_nDialogStartStep = 4; }
            else if (m_nDialogStartStep == 4) { _dev.AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RM2;"); m_nDialogStartStep = 5; }
            else if (m_nDialogStartStep == 5) { _dev.AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RP0;"); m_nDialogStartStep = 6; }
            else if (m_nDialogStartStep == 6) { _dev.AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RP1;"); m_nDialogStartStep = 7; }
            else if (m_nDialogStartStep == 7) { _dev.AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RP2;"); m_nDialogStartStep = 8; }
            else if (m_nDialogStartStep == 8)
            {
                m_nDialogStartStep = 9;
                _main.m_VacuumBeamAlignFocusData.m_nAirToEvacWaitTimerState = 0;
                // 진공 중이면 원본은 Air→Evac 대기 안내 창을 띄운다 (SetExecuteAirToEvacWaitMessage, 8단계 Vacuum 패널)
                int vac = _main.m_VacuumBeamAlignFocusData.m_nIntegratedVacuumState;
                if (vac == CLIENT_VAC_EVAC_STATE || vac == CLIENT_VAC_OK_STATE || vac == CLIENT_HV_VAC_OK_STATE || vac == CLIENT_HV_EVAC_STATE)
                    AddLog("vacuum state " + vac + ": Air-to-Evac wait message (step 8)");
            }
            else if (m_nDialogStartStep == 9)
            {
                _mainTimers.Kill(TIMER_ID_DIALOG_START_PROCESS);
                if (_cfe.m_bUsbRS232ComPortOpen == TRUE) _cfe.m_wndBeamControlDlg.SetCfeHVPowerMonitoringStart();
                _mainTimers.Set(TIMER_ID_JOYSTICK_STATE_PROCESS, 100);
                _w.StatusText.Text = _sim ? "Ready (simulator)" : "Ready";
                AddLog("start sequence done");
            }
        }

        /// <summary>
        /// 원본 OnClose (NanoImpact2020cfeDlg.cpp:2635): 고전압 동작 중이면 거부, 확인 후 스레드 정지·RS232 닫기 → 500 ms 뒤 다시 닫기 →
        /// 영상 캡처 끔, 장치 기본값, USBINTEGRATEDDATA.INI 저장, Scan Bridge 끔. true면 창을 닫아도 된다.
        /// </summary>
        public bool OnClose()
        {
            if (_closed) return true;
            if (m_bProgramCloseTimerFlag == TRUE)
            {
                if (m_nProgramCloseFlagCode == 1)
                {
                    m_pAssistDeviceTool.SetSEMImageCaptureOnOffValueUsbDeviceData(0);
                    m_pAssistDeviceTool.InitSEMDeviceDefaultControlValue();
                    _dev.AfxSetIntegratedDataToFile();
                    _dev.AfxScanBridgePowerOff(DEF_PROGRAM_EXIT_AXIS);
                }
                OnDestroy();
                return true;
            }
            if (_closing) return false;

            int nSEMState = _main.m_VacuumBeamAlignFocusData.m_nSemVacuumState;
            string refuse = null;
            if (nSEMState == CLIENT_HV_START_STATE || nSEMState == CLIENT_HV_ON_STATE)
                refuse = "High voltage is used in SEM equipment. Switch the SEM to STANDBY state...!!!";
            else if (nSEMState == CLIENT_HV_RUN_STATE)
                refuse = "GUN HV UP/DOWN is operating.  Program cannot be terminated during GUN HV UP/DOWN operation...!!!";
            else if (nSEMState == CLIENT_FIL_RUN_STATE)
                refuse = "FILAMENT UP/DOWN is operating.  Program cannot be terminated during FILAMENT UP/DOWN operation...!!!";
            else if (nSEMState == CLIENT_BIAS_RUN_STATE)
                refuse = "BIAS UP/DOWN is operating.  Program cannot be terminated during BIAS UP/DOWN operation...!!!";
            if (refuse != null)
            {
                MessageBox.Show(_w, refuse, "NOTICE", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // 원본 CMessageBoxDisplayDlg(99): YES / NO / SHUTDOWN(99). 시스템 끄기 갈래는 8단계 확인 창과 함께.
            if (MessageBox.Show(_w, "Do you want to exit the SEM CLIENT program?", "NOTICE", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return false;

            _closing = true;
            _w.StatusText.Text = "Closing...";
            _simScan?.Stop();
            m_bProgramCloseTimerFlag = TRUE;
            m_nProgramCloseFlagCode = 1;
            _dev.AfxIntegratedSetThreadAllStopFlag();
            _cfe.CfeHVPowerUsbRS232ComPortClose();
            _mainTimers.Set(TIMER_ID_DIALOG_PROGRAM_CLOSE, 500);
            return false;
        }

        /// <summary>원본 OnDestroy: USB 끊기, CLIENTPUBLICDATA.INI 저장(원본 DestoryAll), 공용 데이터 해제, DLL 해제</summary>
        private void OnDestroy()
        {
            _closed = true;
            _mainTimers.KillAll();
            _beamTimers.KillAll();
            Thread.Sleep(100);
            m_pAssistDeviceTool.UsbDeviceDisconnect();
            Thread.Sleep(100);
            if (!_sim)
            {
                try { _main.m_ClientPublicData.SaveFile(Path.Combine(_iniFolder, "CLIENTPUBLICDATA.INI")); }
                catch (Exception ex) { AddLog("CLIENTPUBLICDATA.INI save failed: " + ex.Message); }
            }
            _dev.AfxReleaseIntegratedCommand();
        }

        // ---- 창 1번 메시지 (원본 메시지 맵): 장치 처리기 → 영상 조립 → Spellman ----

        private int MainWindowProc(int msg, int wParam, object lParam)
        {
            if (msg == UM_SEM_SE_SCAN_IMAGE_DATA) return _proc.OnSemSEScanImageData(wParam, lParam);
            if (msg == UM_CFE_HV_POWER_CONTROL_RECEIVE_DATA) return _cfe.OnCfeHVPowerUsbRS232ComPortReceiveData(wParam, lParam);
            if (msg == UM_CFE_HV_POWER_CONTROL_PORT_CLOSE) return _cfe.OnCfeHVPowerUsbRS232ComPortClose(wParam, lParam);
            if (msg == UM_SEM_SE_SCAN_IMAGE_DATA_AUTO_FOCUS || msg == UM_SEM_SE_SCAN_IMAGE_DATA_AUTO_CONTRAST || msg == UM_SEM_SE_SCAN_IMAGE_DATA_AUTO_BRIGHTNESS
                || msg == UM_SEM_SE_SCAN_IMAGE_DATA_AUTO_CONT_BRIG || msg == UM_SEM_SE_SCAN_IMAGE_DATA_AUTO_GUN_ALIGN)
                return 0; // 자동 기능 영상 (9단계)
            int? r = _main.Dispatch(msg, wParam, lParam);
            if (r.HasValue) return r.Value;
            AddLog("hwnd1 msg " + msg + " (no handler in step 7)");
            return 0;
        }

        // ---- 화면 갱신 (원본 Sem*ImageDataUpdateControl → Update1stImageBitmap / Update2ndImage*Bitmap) ----

        private void SemSEScanImageDataUpdateControl()
        {
            _w.First.Show(_proc.m_p1stImageRgb, _proc.m_bm1stBitmap.bmWidth, _proc.m_bm1stBitmap.bmHeight);
            _w.CountFrame();
        }

        private void SemSEPhotoScanImageDataUpdateControl(SemScanImageProcessor.CBitmap bitmap)
        {
            _w.Second.Show(bitmap.Bits, bitmap.Info.bmWidth, bitmap.Info.bmHeight);
        }

        // ---- 진공 상태 표시 (원본 DisplayVacuumStateInfo / UpdateVacuumStateInfoData) ----

        private void DisplayVacuumStateInfo(int nVacuumState)
        {
            _mainTimers.Kill(TIMER_ID_VACUUM_STATE_UPDATE);
            m_nVacuumStateBlinkFlag = 0;
            m_nVacuumStateBlinkCount = 0;
            m_strVacuumState = "SEM STATE";
            bool blink = false;
            if (nVacuumState == -1000) { m_strVacuumState = "WAITING "; blink = true; }
            else if (nVacuumState == CLIENT_POWER_OFF_STATE) { m_strVacuumState = "SEM POWER OFF"; blink = true; }
            else if (nVacuumState == CLIENT_AIR_ON_STATE || nVacuumState == CLIENT_HV_AIR_ON_STATE) { m_strVacuumState = "AIR ON"; blink = true; }
            else if (nVacuumState == CLIENT_VAC_EVAC_STATE || nVacuumState == CLIENT_HV_EVAC_STATE) { m_strVacuumState = "VACUUM PROCEEDING"; blink = true; }
            else if (nVacuumState == CLIENT_VAC_OK_STATE || nVacuumState == CLIENT_HV_VAC_OK_STATE) m_strVacuumState = "SEM STANDBY";
            else if (nVacuumState == CLIENT_HV_START_STATE) m_strVacuumState = "SEM START";
            else if (nVacuumState == CLIENT_HV_ON_STATE) m_strVacuumState = "SEM ON";
            else if (nVacuumState == CLIENT_CNC5_GO_SAM_LOAD) { m_strVacuumState = "SAMPLE LOAD"; blink = true; }
            else if (nVacuumState == CLIENT_CNC5_GO_SAM_UNLOAD) { m_strVacuumState = "SAMPLE UNLOAD"; blink = true; }
            else if (nVacuumState == CLIENT_HV_RUN_STATE || nVacuumState == CLIENT_FIL_RUN_STATE || nVacuumState == CLIENT_BIAS_RUN_STATE) { m_strVacuumState = "PREPARING..."; blink = true; }
            _w.SetVacuumState(m_strVacuumState, nVacuumState, true);
            if (blink)
            {
                m_nVacuumStateBlinkFlag = 1;
                _mainTimers.Set(TIMER_ID_VACUUM_STATE_UPDATE, VACUUM_STATE_PROCESS_ELAPSE_TIME);
            }
        }

        private void UpdateVacuumStateInfoData(int nBlinkCount) => _w.SetVacuumState(m_strVacuumState, -1, nBlinkCount == 0);

        private void StageXYAxisMagnificationMovingSpeedChange(int nMagIndex) => AddLog("StageXYAxisMagnificationMovingSpeedChange " + nMagIndex + " (stage: step 8)");

        // ---- 임시 Scan 버튼 (원본 CModularScanControlDlg::OnBnClickedScanControl*Btn의 요청을 그대로 만든다, 8단계에서 실제 패널로) ----

        public void RequestLiveScan(int nScanMode)
        {
            IntegratedCommandStruct ic = _dev.AfxGetIntegratedPointer();
            if (ic == null) return;
            int w, h, x, y;
            if (nScanMode == RED_SCAN_MODE) { w = SE_RED_SCAN_PIXEL_WIDTH; h = SE_RED_SCAN_PIXEL_HEIGHT; x = SE_RED_SCAN_X_SHIFT; y = SE_RED_SCAN_Y_SHIFT; }
            else if (nScanMode == FAST_SCAN_MODE) { w = SE_FAST_SCAN_PIXEL_WIDTH; h = SE_FAST_SCAN_PIXEL_HEIGHT; x = SE_FAST_SCAN_X_SHIFT; y = SE_FAST_SCAN_Y_SHIFT; }
            else if (nScanMode == SLOW1_SCAN_MODE) { w = SE_SLOW1_SCAN_PIXEL_WIDTH; h = SE_SLOW1_SCAN_PIXEL_HEIGHT; x = SE_SLOW1_SCAN_X_SHIFT; y = SE_SLOW1_SCAN_Y_SHIFT; }
            else { w = SE_SLOW2_SCAN_PIXEL_WIDTH; h = SE_SLOW2_SCAN_PIXEL_HEIGHT; x = SE_SLOW2_SCAN_X_SHIFT; y = SE_SLOW2_SCAN_Y_SHIFT; }
            SendScanRequest(nScanMode, w, h, x, y, ic.nWAverageNum[nScanMode]);
            m_SemSEScanControlPublicData.m_nSemSEScanModeBackup = m_SemSEScanControlPublicData.m_nSemSEScanModeValue;
        }

        public void RequestPhotoScan(int nScanMode)
        {
            IntegratedCommandStruct ic = _dev.AfxGetIntegratedPointer();
            if (ic == null) return;
            if (m_SemSEScanControlPublicData.m_nSemSEScanModeValue == nScanMode) return;
            if (m_SemSEScanControlPublicData.m_bPhotoScanCompleted == FALSE) return;
            int w = nScanMode == PHOTO6_SCAN_MODE ? 2560 : nScanMode == PHOTO4_SCAN_MODE || nScanMode == PHOTO5_SCAN_MODE ? 1280 : 640;
            m_SemSEScanControlPublicData.m_nPhotoScanMode = nScanMode;
            SendScanRequest(nScanMode, w, w * 3 / 4, SE_PHOTO_SCAN_X_SHIFT, SE_PHOTO_SCAN_Y_SHIFT, ic.nWAverageNum[nScanMode]);
        }

        private void SendScanRequest(int mode, int w, int h, int x, int y, int average)
        {
            var req = new REQUEST_USB_DEVICE_DATA { nRequestID = REQIDX_DEVICE_SET_SEM_SE_SCAN_CONTROL_DATA, nRequestType = 0, nRequestOption = mode };
            req.nRequestData[0] = w;
            req.nRequestData[1] = h;
            req.nRequestData[2] = x;
            req.nRequestData[3] = y;
            req.nRequestData[4] = average;
            req.nRequestData[5] = 1; // SCAN GENERATION
            _main.OnSetSemSEScanControlDataUpdate(0, req);
        }

        public void CaptureStop()
        {
            m_pAssistDeviceTool.SetSEMImageCaptureOnOffValueUsbDeviceData(0);
            m_SemSEScanControlPublicData.m_bSEImageCaptureStart = FALSE;
            _mainTimers.Kill(TIMER_ID_SE_SCAN_IMAGE_DISPLAY);
            _mainTimers.Kill(TIMER_ID_SE_SCAN_IMAGE_DISPLAY_NEXT);
            _mainTimers.Kill(TIMER_ID_SE_PHOTO12_SCAN_IMAGE_DISPLAY);
            _mainTimers.Kill(TIMER_ID_SE_PHOTO345_SCAN_IMAGE_DISPLAY);
            _mainTimers.Kill(TIMER_ID_SE_PHOTO6_SCAN_IMAGE_DISPLAY);
            _simScan?.Stop();
            _w.ScanModeText.Text = "scan stopped";
        }

        // ---- 기록 ----

        void IPanelSink.Call(string name, params object[] args) => PanelLine(name + " " + PanelCallFormat.Args(args));

        private void PanelLine(string line)
        {
            PanelCalls.Add(DateTime.Now.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture) + "  " + line);
            if (PanelCalls.Count > 2000) PanelCalls.RemoveAt(0);
        }

        public void AddLog(string line)
        {
            if (!_w.Dispatcher.CheckAccess())
            {
                _w.Dispatcher.BeginInvoke(new Action(() => AddLog(line)));
                return;
            }
            Log.Add(DateTime.Now.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture) + "  " + line);
            if (Log.Count > 2000) Log.RemoveAt(0);
        }

        /// <summary>원본 ::MessageBox (DeviceHost가 UI 스레드에서 부른다) → IDOK/IDYES/IDNO</summary>
        private int ShowMessageBox(string text, string caption, uint type)
        {
            AddLog("MessageBox [" + caption + "] " + text);
            uint buttons = type & 0x0F;
            MessageBoxButton b = buttons == 4 ? MessageBoxButton.YesNo : buttons == 1 ? MessageBoxButton.OKCancel : buttons == 3 ? MessageBoxButton.YesNoCancel : MessageBoxButton.OK;
            MessageBoxImage icon = (type & 0xF0) == 0x10 ? MessageBoxImage.Error : (type & 0xF0) == 0x30 ? MessageBoxImage.Warning : (type & 0xF0) == 0x20 ? MessageBoxImage.Question : MessageBoxImage.Information;
            MessageBoxResult r = _w.IsLoaded ? MessageBox.Show(_w, text, caption, b, icon) : MessageBox.Show(text, caption, b, icon);
            return r == MessageBoxResult.Yes ? 6 : r == MessageBoxResult.No ? 7 : r == MessageBoxResult.Cancel ? 2 : 1;
        }
    }
}
