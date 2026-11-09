// 원본 NanoImpact2020cfeDlg.cpp:16149-16577 을 NanoImpact.CodeGen translate로 옮긴 뒤 손으로 고친 코드.
#pragma warning disable IDE1006, IDE0059, CS0162, CS0168, CS0219
using System;
using NanoImpact.Core.Native;
using static NanoImpact.Core.Def;
using static NanoImpact.Core.Native.CRuntime;

namespace NanoImpact.Hardware.Device
{
    public sealed partial class MainDeviceService
    {
        // ---- NanoImpact2020cfeDlg.cpp:16149 ----
        //
        // USB CONTROL DATA RECEIVE PROCESS
        //
        // USB CONTROL DATA : SEM INTEGRATED DATA (SEM의 모든정보를 CLIENT 시작시 가져온다)
        public int OnSemControlIntegratedData(int wParam, object lParam)
        {
            if (lParam == null) return 0;

            // SEM INTEGRATED DATA 정보
            // 메모리 초기화
            m_IntegratedData.Clear();
            // SEM INTEGRATED DATA 복사
            m_IntegratedData.CopyFrom((IntegratedCommandStruct)lParam);
            // 크기 Display
            // 2020.01.16 -> sizeof(SEM_INTEGRATED_DATA) : 28440 byte
            string strText = "";
            strText = Format("%d", (int)IntegratedCommandStruct.NativeSize);
            m_wndDetailControlDlg.SetClientInfomation01Text(strText);// m_ClientInfoText1.SetWindowTextW(strText);

            //
            // 진공상태 값 적용 (클라이언트에 적용 한다)
            //
            int nClientSemVacState = -1;
            if      (m_IntegratedData.nSemVacState == POWER_OFF_STATE   ) nClientSemVacState = CLIENT_POWER_OFF_STATE;
            else if (m_IntegratedData.nSemVacState == AIR_ON_STATE      ) nClientSemVacState = CLIENT_AIR_ON_STATE;
            else if (m_IntegratedData.nSemVacState == VAC_EVAC_STATE    ) nClientSemVacState = CLIENT_VAC_EVAC_STATE;
            else if (m_IntegratedData.nSemVacState == VAC_OK_STATE      ) nClientSemVacState = CLIENT_VAC_OK_STATE;
            else if (m_IntegratedData.nSemVacState == HV_ON_STATE       ) nClientSemVacState = CLIENT_HV_ON_STATE;
            else if (m_IntegratedData.nSemVacState == CNC5_GO_SAM_LOAD  ) nClientSemVacState = CLIENT_CNC5_GO_SAM_LOAD;
            else if (m_IntegratedData.nSemVacState == CNC5_GO_SAM_UNLOAD) nClientSemVacState = CLIENT_CNC5_GO_SAM_UNLOAD;
            else if (m_IntegratedData.nSemVacState == HV_VAC_OK_STATE   ) nClientSemVacState = CLIENT_HV_VAC_OK_STATE;
            else if (m_IntegratedData.nSemVacState == HV_AIR_ON_STATE   ) nClientSemVacState = CLIENT_HV_AIR_ON_STATE;
            else if (m_IntegratedData.nSemVacState == HV_EVAC_STATE     ) nClientSemVacState = CLIENT_HV_EVAC_STATE;
            // 진공상태 표시 ( SEM_POWER_OFF, SEM_EVAC_PROCCED, SEM_VACUUM_STATE, SEM_HV_ON ... )
            m_wndVacuumControlDlg.SetIntegratedVacuumStateInfo(nClientSemVacState);
            DisplayVacuumStateInfo(m_VacuumBeamAlignFocusData.m_nIntegratedVacuumState);
            //

            //
            // 챔버 진공 값
            //
            // 진공게이지 값 표시
            m_wndVacuumControlDlg.InitVacuumStateItem(m_VacuumBeamAlignFocusData.m_nIntegratedVacuumState);
            m_wndVacuumControlDlg.SetChamberVacuumValue(m_IntegratedData.nSemVacValue);
            //

            // 기존 설정값의 변화가 AUTO START CONFIG의 설정값 보다 일정비율 이상 차이나면 TRUE
            m_bAutoStartConfigChengeFlag = FALSE;

            // 2020.04.08
            // FILAMENT/BIAS 값이 AUTO START CONFIG 의 값보다 +/- 10% 이상 차이나면
            // AUTO START CONFIG에 있는 FILAMENT/BIAS 설정 값으로 대체한다
            double dAutoStartConfigRatio = (double)(m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_FIL_BIAS_RATIO]) / 100.0;
            double dFilamentValue  = (double)(m_IntegratedData.nFilamentValue);
            double dBiasValue      = (double)(m_IntegratedData.nBiasValue);
            double dFilamentConfig = (double)(m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_FILAMENT_VALUE]); // AUTO START : FILAMENT 최적화 기본값
            double dBiasConfig     = (double)(m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_BIAS_VALUE]);     // AUTO START : BIAS     최적화 기본값
            double dFilConfigMin   = dFilamentConfig - (dFilamentConfig*dAutoStartConfigRatio); // Filament 변화 최소값
            double dFilConfigMax   = dFilamentConfig + (dFilamentConfig*dAutoStartConfigRatio); // Filament 변화 최대값
            double dBiasConfigMin  = dBiasConfig     - (dBiasConfig*dAutoStartConfigRatio    ); // Bias     변화 최소값
            double dBiasConfigMax  = dBiasConfig     + (dBiasConfig*dAutoStartConfigRatio    ); // Bias     변화 최대값
            int    nApplyFilamentValue = m_IntegratedData.nFilamentValue;     // Filament 적용값
            int    nApplyBiasValue     = m_IntegratedData.nBiasValue;         // Bias     적용값
            int    nApplyContrastValue = m_IntegratedData.nContrastValue;     // Contrast 적용값
            int    nApplyBrightValue   = m_IntegratedData.nBrightValue;       // Bright   적용값
            // FILAMENT 변화폭 범위 확인?
            if (dFilamentValue < dFilConfigMin || dFilamentValue > dFilConfigMax)
            {
                // 기존 설정값의 변화가 AUTO START CONFIG의 설정값 보다 일정비율 이상 차이나면 TRUE
                m_bAutoStartConfigChengeFlag = TRUE;
            }
            // BIAS 변화폭 범위 확인?
            if (dBiasValue < dBiasConfigMin || dBiasValue > dBiasConfigMax)
            {
                // 기존 설정값의 변화가 AUTO START CONFIG의 설정값 보다 일정비율 이상 차이나면 TRUE
                m_bAutoStartConfigChengeFlag = TRUE;
            }
            // 변화폭 범위 재조정 -> AUTO START CONFIG의 FILAMENT, BIAS, CONTRAST, BRIGHT 값 적용
            if (m_bAutoStartConfigChengeFlag == TRUE)
            {
                // FILAMENT, BIAS 값  <-- AUTO START CONFIG FILAMENT, BIAS 설정값 적용
                nApplyFilamentValue = m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_FILAMENT_VALUE];
                nApplyBiasValue     = m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_BIAS_VALUE];
                // CONTRAST, BRIGHTNESS 값  <-- AUTO START CONFIG CONTRAST, BRIGHT 설정값 적용
                nApplyContrastValue = m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_CONTRAST_VALUE];
                nApplyBrightValue   = m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_BRIGHT_VALUE];
            }

            //
            // INTEGRATED GUN HV, FILAMENT, BIAS 정보
            //
            //-
            // GUN HV 목표 값 표시
            //-
            m_wndBeamControlDlg.SetGunHVIntegratedItemIndex(m_IntegratedData.nGunHVIndex);
            SetMainDlgGunHVComboIndex(m_IntegratedData.nGunHVIndex);
            //-
            // FILAMENT 목표 값 표시
            //-
            m_wndBeamControlDlg.SetFilamentIntegratedValue(nApplyFilamentValue);
            //-
            // BIAS 목표 값 표시
            //-
            m_wndBeamControlDlg.SetBiasIntegratedValue(nApplyBiasValue);
            //-
            // SPOT SIZE 값 표시
            //-
            m_wndBeamControlDlg.SetSpotSizeIntegratedItem(m_IntegratedData.nSpotSize);
            m_nSpotSizeValue = m_IntegratedData.nSpotSize;
            //-
            // MAGNIFICATION 값 표시
            //-
            m_wndScanControlDlg.SetMagnificationIndexIntegratedItem(m_IntegratedData.nMagStepIndex);
            SetMainDlgMagComboIndex((MAX_MAG_STEP-1)-m_IntegratedData.nMagStepIndex);
            //-
            // WOBBLE 값 표시
            //-
            m_wndFocusControlDlg.SetWobbleIntegratedItem(m_IntegratedData.nWobble - 1);
            //-
            // GUN ALIGN X,Y 값 표시
            //-
            m_wndAlignControlDlg.SetGunAlignXIntegratedPosition(m_IntegratedData.nGAXValue);
            m_wndAlignControlDlg.SetGunAlignYIntegratedPosition(m_IntegratedData.nGAYValue);
            //-
            // BEAM ALIGN X,Y 값 표시
            //-
            m_wndAlignControlDlg.SetBeamAlignXIntegratedPosition(m_IntegratedData.nBAXValue);
            m_wndAlignControlDlg.SetBeamAlignYIntegratedPosition(m_IntegratedData.nBAYValue);
            //-
            // IMAGE SHIFT X,Y 값 표시
            //-
            m_wndAlignControlDlg.SetImageShiftXIntegratedPosition(m_IntegratedData.nISXValue);
            m_wndAlignControlDlg.SetImageShiftYIntegratedPosition(m_IntegratedData.nISYValue);
            //-
            // STIG X,Y 값 표시
            //-
            m_wndFocusControlDlg.SetStigXIntegratedPosition(m_IntegratedData.nStigXValue);
            m_wndFocusControlDlg.SetStigYIntegratedPosition(m_IntegratedData.nStigYValue);
            //-
            // CONTRAST, BRIGHTNESS 값 표시
            //-
            m_wndFocusControlDlg.SetContrastIntegratedPosition(nApplyContrastValue);  // m_IntegratedData.nContrastValue);
            m_wndFocusControlDlg.SetBrightnessIntegratedPosition(nApplyBrightValue);  // m_IntegratedData.nBrightValue);
            //-
            // FOCUS COARSE/FINE 값 설정
            //-
            m_wndFocusControlDlg.SetFocusCoarseFineValue(m_IntegratedData.nFocusCoarse, m_IntegratedData.nFocusFine);
            //-
            // SCAN ROTATION
            //-
            m_wndAlignControlDlg.SetScanRotationIntegratedPosition(m_IntegratedData.nScanRotValue);
            //
            // FOCUS CONTROL : SCAN AVERAGE 설정
            m_wndFocusControlDlg.SetRedAverageIntegratedItem(m_IntegratedData.nWAverageNum[RED_SCAN_MODE]);
            m_wndFocusControlDlg.SetFastAverageIntegratedItem(m_IntegratedData.nWAverageNum[FAST_SCAN_MODE]);
            //

            //
            // INTEGRATED SCAN CALIBRATION 정보
            //
            // SCAN CALIBRATION : USB CONTROL INTEGRATED DATA 설정
            m_wndScanCalibrationToolDlg.SetSemUsbControlIntegratedData(m_IntegratedData);
            // SCAN CALIBRATION : SCAN MODE 설정
            m_wndScanCalibrationToolDlg.SemSEScanModeDataUpdateControl(m_IntegratedData.nScanMode);
            // SCAN CALIBRATION : SCAN AVERAGE 설정
            m_wndScanCalibrationToolDlg.SetSEScanAverageValue(RED_SCAN_MODE  , m_IntegratedData.nWAverageNum[RED_SCAN_MODE]);
            m_wndScanCalibrationToolDlg.SetSEScanAverageValue(FAST_SCAN_MODE , m_IntegratedData.nWAverageNum[FAST_SCAN_MODE]);
            m_wndScanCalibrationToolDlg.SetSEScanAverageValue(SLOW1_SCAN_MODE, m_IntegratedData.nWAverageNum[SLOW1_SCAN_MODE]);
            m_wndScanCalibrationToolDlg.SetSEScanAverageValue(SLOW2_SCAN_MODE, m_IntegratedData.nWAverageNum[SLOW2_SCAN_MODE]);
            //
            // SCAN HW SETUP : OPTION DATA
            m_wndScanCalibrationToolDlg.SetScanHWSetupOptionDataAll(m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_PHOTO_IMAGE_FILTER],    // PHOTO NONE STOP --> 2020.05.26 PHOTO IMAGE FILTERING
                                                                    m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_IMAGE_SHIFT_X_REVERSE], // IMAGE SHIFT X REVERSE
                                                                    m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_IMAGE_SHIFT_Y_REVERSE], // IMAGE SHIFT Y REVERSE
                                                                    m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_ORTHOGONAL_CORRRECTION],// ORTHOGONAL CORRECTION
                                                                    m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_STAGE_HORZ_ALIGN],      // STAGE HORZ ALIGN
                                                                    m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_Z_AXIS_FOCUS_LINK],     // Z AXIS FOCUS LINK
                                                                    m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_WD_SCAN_ROTATION],      // WD SCAN ROTATION CORRECTION
                                                                    m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_WD_SCALE_ZOOM],         // WD SCALE ZOOM CORRECTION
                                                                    m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_HV_CORRECTION],         // HV CORRECTION
                                                                    m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_HV_DEGAUSSING],         // 2021.08.23 : HV PROCESSING 완료 후, FOCUS DEGAUSSING
                                                                    m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_SCAN_CLOCK_48]);        // 2021.07.29 : SCAN CLOCK 48BITS
            //
            // 모니터크기 기본 24인치로 재설정
            if (m_IntegratedData.dScanOptionData[IDX_HWSETUP_OPTION_DOUBLE_MONITOR_SIZE] < 1.0) m_IntegratedData.dScanOptionData[IDX_HWSETUP_OPTION_DOUBLE_MONITOR_SIZE] = 24.0;
            // 계산된 Dot Pich 값의 실제크기 적용율 재설정
            if (m_IntegratedData.dScanOptionData[IDX_HWSETUP_OPTION_DOUBLE_MONITOR_RATIO] < 1.0) m_IntegratedData.dScanOptionData[IDX_HWSETUP_OPTION_DOUBLE_MONITOR_RATIO] = 98.5;
            // 모니터크기 설정
            m_wndScanCalibrationToolDlg.SetMonitorSize(m_IntegratedData.dScanOptionData[IDX_HWSETUP_OPTION_DOUBLE_MONITOR_SIZE]);
            // 계산된 Dot Pich 값 적용율
            m_wndScanCalibrationToolDlg.SetMonitorSizeRatio(m_IntegratedData.dScanOptionData[IDX_HWSETUP_OPTION_DOUBLE_MONITOR_RATIO]);
            //
            // AUTO FOCUS SCAN MODE 설정
            if (m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_SCANMODE]    < 1) m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_SCANMODE]    = FAST_SCAN_MODE;
            if (m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_UP_NUMBER]   < 1) m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_UP_NUMBER]   = 20; // UP SCAN NUMBER
            if (m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_DOWN_NUMBER] < 1) m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_DOWN_NUMBER] = 10; // DOWN SCAN NUMBER
            if (m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_STEP_VALUE]  < 1) m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_STEP_VALUE]  = 5;  // SCAN STEP VALUE
            if (m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_YPOS_VALUE]  < 1) m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_YPOS_VALUE]  = 450;// FOCUS CHANGE Y-POS
            if (m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_CONTRAST_VALUE]    < 1) m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_CONTRAST_VALUE]    = 40; // CONTRAST VALUE
            if (m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_CONTRAST_100JUMP]  < 1) m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_CONTRAST_100JUMP]  = 50; // CONTRAST 100 JUMP STEP
            if (m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_CONTRAST_10JUMP]   < 1) m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_CONTRAST_10JUMP]   = 20; // CONTRAST 10  JUMP STEP
            if (m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_BRIGHT_VALUE]      < 1) m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_BRIGHT_VALUE]      = 50; // BRIGHTNESS VALUE
            if (m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_BRIGHT_100JUMP]    < 1) m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_BRIGHT_100JUMP]    = 50; // BRIGHTNESS 100 JUMP STEP
            if (m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_BRIGHT_10JUMP]     < 1) m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_BRIGHT_10JUMP]     = 20; // BRIGHTNESS 10  JUMP STEP
            // AUTO FOCUS : SCAN MODE 설정
            m_wndScanCalibrationToolDlg.SetAutoFocusScanMode(m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_SCANMODE]);
            // AUTO FOCUS : UP SCAN NUMBER 설정
            m_wndScanCalibrationToolDlg.SetAutoFocusUpNumber(m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_UP_NUMBER]);
            // AUTO FOCUS : DOWN SCAN NUMBER 설정
            m_wndScanCalibrationToolDlg.SetAutoFocusDownNumber(m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_DOWN_NUMBER]);
            // AUTO FOCUS : SCAN STEP VALUE 설정
            m_wndScanCalibrationToolDlg.SetAutoFocusStepValue(m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_STEP_VALUE]);
            // AUTO FOCUS : FOCUS CHANGE Y-POS VALUE 설정
            m_wndScanCalibrationToolDlg.SetAutoFocusYPosValue(m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_YPOS_VALUE]);
            // AUTO CONTRAST : CONTRAST VALUE 설정
            m_wndScanCalibrationToolDlg.SetAutoContrastValue(m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_CONTRAST_VALUE]);
            // AUTO CONTRAST : 100 JUMP STEP VALUE 설정
            m_wndScanCalibrationToolDlg.SetAutoContrastTime(m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_CONTRAST_100JUMP]);
            // AUTO CONTRAST : 10 JUMP STEP VALUE 설정
            m_wndScanCalibrationToolDlg.SetAutoContrastStep(m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_CONTRAST_10JUMP]);
            // AUTO BRIGHTNESS : BRIGHTNESS VALUE 설정
            m_wndScanCalibrationToolDlg.SetAutoBrightnessValue(m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_BRIGHT_VALUE]);
            // AUTO BRIGHTNESS : 100 JUMP VALUE 설정
            m_wndScanCalibrationToolDlg.SetAutoBrightnessTime(m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_BRIGHT_100JUMP]);
            // AUTO BRIGHTNESS : 10 JUMP STEP VALUE 설정
            m_wndScanCalibrationToolDlg.SetAutoBrightnessStep(m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_BRIGHT_10JUMP]);
            //
            // 2020.04.06
            // AUTO START CONFIG VALUE 설정
            if (m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_FILAMENT_VALUE]   < 1) m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_FILAMENT_VALUE]   = 2690; // FILAMENT 최적화 기본값
            if (m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_BIAS_VALUE]       < 1) m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_BIAS_VALUE]       = 1780; // BAIS     최적화 기본값
            if (m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_CONTRAST_VALUE]   < 1) m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_CONTRAST_VALUE]   = 3276; // CONTRAST 설정값 : 3276 (4096의 80%)
            if (m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_BRIGHT_VALUE]     < 1) m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_BRIGHT_VALUE]     = 3276; // BRIGHT   설정값 : 3276 (4096의 80%)
            if (m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_FIL_BIAS_RATIO]   < 1) m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_FIL_BIAS_RATIO]   = AUTO_START_CONFIG_CHANGE_RATIO;// FILAMENT/BIAS 변화폭 적용율(%)
            if (m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_STEP_TIME_VALUE]  < 1) m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_STEP_TIME_VALUE]  = 50;   // STEP TIME[ms] : 50[ms]
            if (m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_EM_CURRENT_VALUE] < 1) m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_EM_CURRENT_VALUE] = 70;   // EM[ua] 목표값 : 70[ua]
            // AUTO START : FILAMENT VALUE 설정
            m_wndScanCalibrationToolDlg.SetAutoStartFilamentValue(m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_FILAMENT_VALUE]);
            // AUTO START : BIAS     VALUE 설정
            m_wndScanCalibrationToolDlg.SetAutoStartBiasValue(m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_BIAS_VALUE]);
            // AUTO START : CONTRAST VALUE 설정
            m_wndScanCalibrationToolDlg.SetAutoStartContrastValue(m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_CONTRAST_VALUE]);
            // AUTO START : BRIGHT   VALUE 설정
            m_wndScanCalibrationToolDlg.SetAutoStartBrightValue(m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_BRIGHT_VALUE]);
            // AUTO START : FILAMENT/BIAS 변화폭 적용율(%)
            m_wndScanCalibrationToolDlg.SetAutoStartFilamentBiasRatio(m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_FIL_BIAS_RATIO]);
            // AUTO START : STEP TIME[ms] VALUE 설정
            m_wndScanCalibrationToolDlg.SetAutoStartStepTimeValue(m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_STEP_TIME_VALUE]);
            // AUTO START : EM[ua]CURRENT VALUE 설정
            m_wndScanCalibrationToolDlg.SetAutoStartEmCurrentValue(m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_EM_CURRENT_VALUE]);
            // 음영반전
            m_wndScanCalibrationToolDlg.SetNegativeEffectOptionSelect(m_IntegratedData.bNEGImageFlag);
            // 좌우반전
            m_wndScanCalibrationToolDlg.SetFlipHorizontalOptionSelect(m_IntegratedData.bLRMirrorFlag);
            // 상하반전
            m_wndScanCalibrationToolDlg.SetFlipVerticalOptionSelect(m_IntegratedData.bTBMirrorFlag);
            // 음영반전 SCAN CONTROL DIALOG 설정
            m_wndScanControlDlg.SetScanControlPhotoInverse1Select(m_IntegratedData.bNEGImageFlag);
            // 2021.07.22 : BSE IMAGE EFFECT
            // BSE 음영반전
            m_wndScanCalibrationToolDlg.SetBseNegativeEffectOptionSelect(m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_BSE_NEG_IMAGE]);
            // BSE 좌우반전
            m_wndScanCalibrationToolDlg.SetBseFlipHorizontalOptionSelect(m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_BSE_FLIP_HORZ]);
            // BSE 상하반전
            m_wndScanCalibrationToolDlg.SetBseFlipVerticalOptionSelect(m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_BSE_FLIP_VERT]);
            //
            //
            //
            // 2020.11.25
            // AC SYNC DATA DISPLAY : FAST SCAN MODE
            m_wndScanCalibrationToolDlg.DisplayAcSyncDataAccordingScanMode(1);
            //
            //
            //
            // SEM 장비 정보
            // ASSIST ENVIRONMENT : SEM DEVICE MODEL
            m_wndEnvironmentToolDlg.SetSemModelData(ToStr(m_IntegratedData.szScanOption32Text1));
            // ASSIST ENVIRONMENT : SEM DEVICE COMPANY
            m_wndEnvironmentToolDlg.SetSemCompanyData(ToStr(m_IntegratedData.szScanOption32Text2));
            //
            // 샘플높이
            m_ClientPublicData.dSampleHeight = m_IntegratedData.dScanOptionData[IDX_HWSETUP_OPTION_DOUBLE_SAMPLE_HEIGHT];
            // 샘플넓이
            m_ClientPublicData.dSampleWidth  = m_IntegratedData.dScanOptionData[IDX_HWSETUP_OPTION_DOUBLE_SAMPLE_WIDTH];
            // 샘플명칭 : IDX_HWSETUP_OPTION_128TEXT_SAMPLE_NAME
            ZeroMem(m_ClientPublicData.szSampleName);
            strcpy(m_ClientPublicData.szSampleName, m_IntegratedData.szScanOption128Text1);
            //
            //
            //
            
            // CL LMH MODE DATA 설정
            m_wndScanCalibrationToolDlg.SetCLLMHModeData();
            /*
            m_wndScanCalibrationToolDlg.SetCLLMHModeData(m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW_CL1_VALUE], m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW_CL2_VALUE],
                m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE_CL1_VALUE], m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE_CL2_VALUE],
                m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH_CL1_VALUE], m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH_CL2_VALUE],
                m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW_GAX_VALUE], m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW_GAY_VALUE],
                m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE_GAX_VALUE], m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE_GAY_VALUE],
                m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH_GAX_VALUE], m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH_GAY_VALUE],
                m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW_BAX_VALUE], m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW_BAY_VALUE],
                m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE_BAX_VALUE], m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE_BAY_VALUE],
                m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH_BAX_VALUE], m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH_BAY_VALUE]);
            */
            // CL LMH MODE : CALIBRATION CL LMH MODE SELECT 설정
            if      (m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MODE_CALI_SELECT] == IDX_CL_LMH_LOW_MODE   ) m_wndScanCalibrationToolDlg.DisplayCLLMHModeData(m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MODE_CALI_SELECT], m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW_EXEC_SELECT]);
            else if (m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MODE_CALI_SELECT] == IDX_CL_LMH_MIDDLE_MODE) m_wndScanCalibrationToolDlg.DisplayCLLMHModeData(m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MODE_CALI_SELECT], m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE_EXEC_SELECT]);
            else if (m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MODE_CALI_SELECT] == IDX_CL_LMH_HIGH_MODE  ) m_wndScanCalibrationToolDlg.DisplayCLLMHModeData(m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MODE_CALI_SELECT], m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH_EXEC_SELECT]);
            // CL LMH MODE : SCAN CL LMH MODE SELECT 설정
            m_wndScanControlDlg.SetScanCLLMHModeSelect(m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MODE_EXEC_SELECT]);
            m_wndScanControlDlg.SetScanCLLMH123ModeSelect(m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW_EXEC_SELECT], m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE_EXEC_SELECT], m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH_EXEC_SELECT]);
            //
            //
            //


            //
            // INTEGRATED 스테이지 CALIBRATION 정보
            //
            // Stage Axis limit sw state Read Data Set & Display
            m_wndStageCalibrationToolDlg.IntegratedSetAxisLimitStateValue(IDX_X_AXIS, m_IntegratedData.nAxisLimitSWStatus[IDX_X_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisLimitStateValue(IDX_Y_AXIS, m_IntegratedData.nAxisLimitSWStatus[IDX_Y_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisLimitStateValue(IDX_Z_AXIS, m_IntegratedData.nAxisLimitSWStatus[IDX_Z_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisLimitStateValue(IDX_R_AXIS, m_IntegratedData.nAxisLimitSWStatus[IDX_R_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisLimitStateValue(IDX_T_AXIS, m_IntegratedData.nAxisLimitSWStatus[IDX_T_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisLimitStateValue(IDX_B_AXIS, m_IntegratedData.nAxisLimitSWStatus[IDX_B_AXIS]);
            // Stage Axis step/mm, step/deg Read Data Set & Display
            m_wndStageCalibrationToolDlg.IntegratedSetAxisMMStepReadValue(IDX_X_AXIS, m_IntegratedData.dAxisMMPerPulse[IDX_X_AXIS], m_IntegratedData.dAxisPulsePerUnit[IDX_X_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisMMStepReadValue(IDX_Y_AXIS, m_IntegratedData.dAxisMMPerPulse[IDX_Y_AXIS], m_IntegratedData.dAxisPulsePerUnit[IDX_Y_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisMMStepReadValue(IDX_Z_AXIS, m_IntegratedData.dAxisMMPerPulse[IDX_Z_AXIS], m_IntegratedData.dAxisPulsePerUnit[IDX_Z_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisMMStepReadValue(IDX_R_AXIS, m_IntegratedData.dAxisMMPerPulse[IDX_R_AXIS], m_IntegratedData.dAxisPulsePerUnit[IDX_R_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisMMStepReadValue(IDX_T_AXIS, m_IntegratedData.dAxisMMPerPulse[IDX_T_AXIS], m_IntegratedData.dAxisPulsePerUnit[IDX_T_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisMMStepReadValue(IDX_B_AXIS, m_IntegratedData.dAxisMMPerPulse[IDX_B_AXIS], m_IntegratedData.dAxisPulsePerUnit[IDX_B_AXIS]);
            // Stage Axis mm/step, deg/step Edit Data Set & Display
            m_wndStageCalibrationToolDlg.IntegratedSetAxisMMStepEditValue(IDX_X_AXIS, m_IntegratedData.dAxisMMPerPulse[IDX_X_AXIS], m_IntegratedData.dAxisPulsePerUnit[IDX_X_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisMMStepEditValue(IDX_Y_AXIS, m_IntegratedData.dAxisMMPerPulse[IDX_Y_AXIS], m_IntegratedData.dAxisPulsePerUnit[IDX_Y_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisMMStepEditValue(IDX_Z_AXIS, m_IntegratedData.dAxisMMPerPulse[IDX_Z_AXIS], m_IntegratedData.dAxisPulsePerUnit[IDX_Z_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisMMStepEditValue(IDX_R_AXIS, m_IntegratedData.dAxisMMPerPulse[IDX_R_AXIS], m_IntegratedData.dAxisPulsePerUnit[IDX_R_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisMMStepEditValue(IDX_T_AXIS, m_IntegratedData.dAxisMMPerPulse[IDX_T_AXIS], m_IntegratedData.dAxisPulsePerUnit[IDX_T_AXIS]);
            // Stage Axis position step Read Data Set & Display
            // Calibration Position
            m_wndStageCalibrationToolDlg.IntegratedSetAxisPosStepValue(IDX_X_AXIS, m_IntegratedData.nAxisPosition[IDX_X_AXIS], m_IntegratedData.dCurAxisPos[IDX_X_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisPosStepValue(IDX_Y_AXIS, m_IntegratedData.nAxisPosition[IDX_Y_AXIS], m_IntegratedData.dCurAxisPos[IDX_Y_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisPosStepValue(IDX_Z_AXIS, m_IntegratedData.nAxisPosition[IDX_Z_AXIS], m_IntegratedData.dCurAxisPos[IDX_Z_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisPosStepValue(IDX_R_AXIS, m_IntegratedData.nAxisPosition[IDX_R_AXIS], m_IntegratedData.dCurAxisPos[IDX_R_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisPosStepValue(IDX_T_AXIS, m_IntegratedData.nAxisPosition[IDX_T_AXIS], m_IntegratedData.dCurAxisPos[IDX_T_AXIS]);
            // Stage Position
            m_wndStageControlDlg.SetStageAxisCurPositionMMDeg(IDX_X_AXIS, m_IntegratedData.dCurAxisPos[IDX_X_AXIS]);
            m_wndStageControlDlg.SetStageAxisCurPositionMMDeg(IDX_Y_AXIS, m_IntegratedData.dCurAxisPos[IDX_Y_AXIS]);
            m_wndStageControlDlg.SetStageAxisCurPositionMMDeg(IDX_Z_AXIS, m_IntegratedData.dCurAxisPos[IDX_Z_AXIS]);
            m_wndStageControlDlg.SetStageAxisCurPositionMMDeg(IDX_R_AXIS, m_IntegratedData.dCurAxisPos[IDX_R_AXIS]);
            m_wndStageControlDlg.SetStageAxisCurPositionMMDeg(IDX_T_AXIS, m_IntegratedData.dCurAxisPos[IDX_T_AXIS]);
            // Stage Axis Width (mm/step) Read Data Set & Display
            m_wndStageCalibrationToolDlg.IntegratedSetAxisWidthReadValue(IDX_X_AXIS, m_IntegratedData.dAxisMaxWidthMM[IDX_X_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisWidthReadValue(IDX_Y_AXIS, m_IntegratedData.dAxisMaxWidthMM[IDX_Y_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisWidthReadValue(IDX_Z_AXIS, m_IntegratedData.dAxisMaxWidthMM[IDX_Z_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisWidthReadValue(IDX_Z_AXIS + 1, m_IntegratedData.dZTopLimitPosMM);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisWidthReadValue(IDX_T_AXIS, m_IntegratedData.dAxisMaxWidthMM[IDX_T_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisWidthReadValue(IDX_T_AXIS + 1, m_IntegratedData.dTiltZeroAnglePosMM);
            // Stage Axis Width (mm/step) Edit Data Set & Display
            m_wndStageCalibrationToolDlg.IntegratedSetAxisWidthEditValue(IDX_X_AXIS, m_IntegratedData.dAxisMaxWidthMM[IDX_X_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisWidthEditValue(IDX_Y_AXIS, m_IntegratedData.dAxisMaxWidthMM[IDX_Y_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisWidthEditValue(IDX_Z_AXIS, m_IntegratedData.dAxisMaxWidthMM[IDX_Z_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisWidthEditValue(IDX_Z_AXIS + 1, m_IntegratedData.dZTopLimitPosMM);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisWidthEditValue(IDX_T_AXIS, m_IntegratedData.dAxisMaxWidthMM[IDX_T_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisWidthEditValue(IDX_T_AXIS + 1, m_IntegratedData.dTiltZeroAnglePosMM);
            // Stage Axis Accelation Read Data Set & Display
            m_wndStageCalibrationToolDlg.IntegratedSetAxisAccelReadValue(IDX_X_AXIS, m_IntegratedData.nAxisAccCalVal[IDX_X_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisAccelReadValue(IDX_Y_AXIS, m_IntegratedData.nAxisAccCalVal[IDX_Y_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisAccelReadValue(IDX_Z_AXIS, m_IntegratedData.nAxisAccCalVal[IDX_Z_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisAccelReadValue(IDX_R_AXIS, m_IntegratedData.nAxisAccCalVal[IDX_R_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisAccelReadValue(IDX_T_AXIS, m_IntegratedData.nAxisAccCalVal[IDX_T_AXIS]);
            // Stage Axis Accelation Edit Data Set & Display
            m_wndStageCalibrationToolDlg.IntegratedSetAxisAccelEditValue(IDX_X_AXIS, m_IntegratedData.nAxisAccCalVal[IDX_X_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisAccelEditValue(IDX_Y_AXIS, m_IntegratedData.nAxisAccCalVal[IDX_Y_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisAccelEditValue(IDX_Z_AXIS, m_IntegratedData.nAxisAccCalVal[IDX_Z_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisAccelEditValue(IDX_R_AXIS, m_IntegratedData.nAxisAccCalVal[IDX_R_AXIS]);
            m_wndStageCalibrationToolDlg.IntegratedSetAxisAccelEditValue(IDX_T_AXIS, m_IntegratedData.nAxisAccCalVal[IDX_T_AXIS]);
            // Stage Axis Accelation Read Data Set & Display
            m_wndStageCalibrationToolDlg.IntegratedSetAxisXYCenterReadValue(m_IntegratedData.dBeamCenterXPos, m_IntegratedData.dBeamCenterYPos);
            // Stage Axis Accelation Edit Data Set & Display
            m_wndStageCalibrationToolDlg.IntegratedSetAxisXYCenterEditValue(m_IntegratedData.dBeamCenterXPos, m_IntegratedData.dBeamCenterYPos);


            // BSE SET SCAN POSITION VALUE
            m_wndStageCalibrationToolDlg.IntegratedSetStageOptionData(IDX_STAGE_OPTION_DOUBLE_TYPE, IDX_STAGE_OPTION_DOUBLE_BSE_POSITION, 0, 0, 0, 0, m_IntegratedData.dStageOptionData[IDX_STAGE_OPTION_DOUBLE_BSE_POSITION]);
            m_wndStageControlDlg.SetStageOptionDataDOUBLEType(m_IntegratedData.dStageOptionData[IDX_STAGE_OPTION_DOUBLE_BSE_POSITION]);

            // Stage Option Data
            m_wndStageCalibrationToolDlg.IntegratedSetStageOptionData(IDX_STAGE_OPTION_BOOL_TYPE, IDX_STAGE_OPTION_BOOL_XY_REVERSE   , m_IntegratedData.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_XY_REVERSE]);     // X<-->Y 반전
            m_wndStageCalibrationToolDlg.IntegratedSetStageOptionData(IDX_STAGE_OPTION_BOOL_TYPE, IDX_STAGE_OPTION_BOOL_X_LR_REVERSE , m_IntegratedData.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_X_LR_REVERSE]);   // X LR 반전
            m_wndStageCalibrationToolDlg.IntegratedSetStageOptionData(IDX_STAGE_OPTION_BOOL_TYPE, IDX_STAGE_OPTION_BOOL_Y_TB_REVERSE , m_IntegratedData.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_Y_TB_REVERSE]);   // Y TB 반전
            m_wndStageCalibrationToolDlg.IntegratedSetStageOptionData(IDX_STAGE_OPTION_BOOL_TYPE, IDX_STAGE_OPTION_BOOL_Z_UD_REVERSE , m_IntegratedData.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_Z_UD_REVERSE]);   // Z UD 반전
            m_wndStageCalibrationToolDlg.IntegratedSetStageOptionData(IDX_STAGE_OPTION_BOOL_TYPE, IDX_STAGE_OPTION_BOOL_Z_MOVE_ECHO  , m_IntegratedData.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_Z_MOVE_ECHO]);    // Z MOVING ECHO
            m_wndStageCalibrationToolDlg.IntegratedSetStageOptionData(IDX_STAGE_OPTION_BOOL_TYPE, IDX_STAGE_OPTION_BOOL_ALL_DATA_VIEW, m_IntegratedData.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_ALL_DATA_VIEW]);  // ALL DATA VIEW : 5 AXIS 사용
            // Stage UI Dialog
            m_wndStageControlDlg.SetStageOptionDataBOOLType(m_IntegratedData.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_XY_REVERSE]  , m_IntegratedData.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_X_LR_REVERSE],
                                                            m_IntegratedData.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_Y_TB_REVERSE], m_IntegratedData.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_Z_UD_REVERSE],
                                                            m_IntegratedData.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_Z_MOVE_ECHO] , m_IntegratedData.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_ALL_DATA_VIEW]);


            //

            // Display 1st Information Text
            Update1stImageInfoBitmap(m_SemSEScanControlPublicData.m_n1stImageInfoDrawX, m_SemSEScanControlPublicData.m_n1stImageInfoDrawY, m_SemSEScanControlPublicData.m_n1stImageInfoWidth, m_SemSEScanControlPublicData.m_n1stImageInfoHeight);

            // SE SCAN IMAGE CAPTURE STOP
            m_pAssistDeviceTool.SetSEMImageCaptureOnOffValueUsbDeviceData(0);

            // 배율에 따른 X-Y 이동 ACCEL 자동조절
            SetTimer(TIMER_ID_MAG_XY_AXIS_ACCEL_CHANGE, 500, NULL);

            //

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:16566 ----
        //
        // USB CONTROL DATA : SEM STAGE CONTROL DATA (SEM STAGE의 모든정보를 가져온다)
        //
        public int OnSemStageUsbControlData(int wParam, object lParam)
        {
            if (wParam == 0) return 0;

            // 프로그램 시작시 초기 실행할 작업

            // STAGE CNC 정보 가져오기
            m_nDialogStartStep = 0;
            SetTimer(TIMER_ID_DIALOG_START_PROCESS, 100, NULL);

            return 1;
        }

    }
}
