// 원본 NanoImpact2020cfeDlg.cpp:16901-18175 을 NanoImpact.CodeGen translate로 옮긴 뒤 손으로 고친 코드.
#pragma warning disable IDE1006, IDE0059, CS0162, CS0168, CS0219
using System;
using NanoImpact.Core.Native;
using static NanoImpact.Core.Def;
using static NanoImpact.Core.Native.CRuntime;

namespace NanoImpact.Hardware.Device
{
    public sealed partial class MainDeviceService
    {
        // ---- NanoImpact2020cfeDlg.cpp:16901 ----
        // USB CONTROL DATA : SEM MODEL DATA
        public int OnSemControlSemModelData(int wParam, object lParam)
        {
            if (lParam == null) return 0;

            // SEM MODEL DATA 정보
            SEM_CONTROL_MODEL_DATA SemModel = new SEM_CONTROL_MODEL_DATA();

            // LONG DATA 복사
            SemModel.CopyFrom((SEM_CONTROL_MODEL_DATA)lParam);

            // SEM MODEL 변경 확인
            if (m_IntegratedData.nSemModel != SemModel.nSemModel)
            {
                // SEM MODEL 값 적용
                m_IntegratedData.nSemModel = SemModel.nSemModel;
                // SEM MODEL DATA 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nSemModel);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }

            // SEM MODEL AMP 변경 확인
            if (m_IntegratedData.dSemModelAmp != SemModel.dSemModelAmp)
            {
                // SEM MODEL AMP 적용
                m_IntegratedData.dSemModelAmp = SemModel.dSemModelAmp;
                // SEM MODEL AMP 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.dSemModelAmp);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }

            // SEM MODEL REF 변경 확인
            if (m_IntegratedData.dSemModelRef != SemModel.dSemModelRef)
            {
                // SEM MODEL REF 적용
                m_IntegratedData.dSemModelRef = SemModel.dSemModelRef;
                // SEM MODEL REF 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.dSemModelRef);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:16952 ----
        // USB CONTROL DATA : SCAN MODE DATA
        public int OnSemControlScanModeData(int wParam, object lParam)
        {
            if (lParam == null) return 0;

            // SCAN MODE DATA 정보
            SEM_CONTROL_SCANMODE_DATA ScanMode = new SEM_CONTROL_SCANMODE_DATA();
            // LONG DATA 복사
            ScanMode.CopyFrom((SEM_CONTROL_SCANMODE_DATA)lParam);

            // SCAN MODE 변경 확인
            if (m_IntegratedData.nScanMode != ScanMode.nScanMode)
            {
                // SCAN MODE 적용
                m_IntegratedData.nScanMode = ScanMode.nScanMode;
                // SCAN MODE 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nScanMode);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            // SCAN MODE : Freeze Mode 변경 확인
            if (m_IntegratedData.bFreezeModeFlag != ScanMode.bFreezeModeFlag)
            {
                // SCAN MODE : Freeze Mode 적용
                m_IntegratedData.bFreezeModeFlag = ScanMode.bFreezeModeFlag;
                // SCAN MODE : Freeze Mode 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.bFreezeModeFlag);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }

            // SCAN MODE : Half Red   Scan Mode 설정값 변경 확인
            if (m_IntegratedData.nHalfRedScan != ScanMode.nHalfRedScan)
            {
                // SCAN MODE : Half Red   Scan Mode 설정값 적용
                m_IntegratedData.nHalfRedScan = ScanMode.nHalfRedScan;
                // SCAN MODE : Half Red   Scan Mode 설정값 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nHalfRedScan);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            // SCAN MODE : Half Fast   Scan Mode 설정값 변경 확인
            if (m_IntegratedData.nHalfFastScan != ScanMode.nHalfFastScan)
            {
                // SCAN MODE : Half Fast   Scan Mode 설정값 적용
                m_IntegratedData.nHalfFastScan = ScanMode.nHalfFastScan;
                // SCAN MODE : Half Fast   Scan Mode 설정값 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nHalfFastScan);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            // SCAN MODE : Half Slow1   Scan Mode 설정값 변경 확인
            if (m_IntegratedData.nHalfSlow1Scan != ScanMode.nHalfSlow1Scan)
            {
                // SCAN MODE : Slow1   Scan Mode 설정값 적용
                m_IntegratedData.nHalfSlow1Scan = ScanMode.nHalfSlow1Scan;
                // SCAN MODE : Half Slow1   Scan Mode 설정값 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nHalfSlow1Scan);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            // SCAN MODE : Half Slow2   Scan Mode 설정값 변경 확인
            if (m_IntegratedData.nHalfSlow2Scan != ScanMode.nHalfSlow2Scan)
            {
                // SCAN MODE : Half Slow2   Scan Mode 설정값 적용
                m_IntegratedData.nHalfSlow2Scan = ScanMode.nHalfSlow2Scan;
                // SCAN MODE : Half Slow2   Scan Mode 설정값 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nHalfSlow2Scan);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            // SCAN MODE : Half Photo   Scan Mode 설정값 변경 확인
            if (m_IntegratedData.nPhotoScan != ScanMode.nPhotoScan)
            {
                // SCAN MODE : Half Photo   Scan Mode 설정값 적용
                m_IntegratedData.nPhotoScan = ScanMode.nPhotoScan;
                // SCAN MODE : Half Photo   Scan Mode 설정값 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nPhotoScan);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:17038 ----
        // USB CONTROL DATA : VACUUM STATE DATA
        public int OnSemControlVacuumData(int wParam, object lParam)
        {
            if (lParam == null) return 0;

            string strText = "";

            // 2020.05.04
            // USB DEVICE DATA
            USB_DEVICE_DATA_INFO UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            // USB DATA 복사
            UsbDeviceDataInfo.CopyFrom((USB_DEVICE_DATA_INFO)lParam);

            // VACUUM STATE 정보
            SEM_CONTROL_VACUUM_DATA VacuumState = new SEM_CONTROL_VACUUM_DATA();

            // DATA 복사 : (1:Vacuum State, 2 : 진공게이지값, 3 : Emission값)
            VacuumState.nProcess        = UsbDeviceDataInfo.nState;
            VacuumState.nSemVacState    = UsbDeviceDataInfo.nData[0]; // VACUUM STATE
            VacuumState.nSemVacValue    = UsbDeviceDataInfo.nData[1]; // VACUUM GAUGE
            VacuumState.dSemVacValue1   = UsbDeviceDataInfo.dData[1]; // VACUUM GAUGE
            VacuumState.nEmCurrentLevel = UsbDeviceDataInfo.nData[2]; // EM CURRENT

            int nClientSemVacState = -1;
            // 2019.10.31
            if (VacuumState.nProcess == 1 || VacuumState.nProcess == 2)
            {
                // 진공상태값
                m_IntegratedData.nSemVacState = VacuumState.nSemVacState;
                // 진공상태 값 적용 (클라이언트에 적용 한다)
                if      (VacuumState.nSemVacState == POWER_OFF_STATE   ) nClientSemVacState = CLIENT_POWER_OFF_STATE;
                else if (VacuumState.nSemVacState == AIR_ON_STATE      ) nClientSemVacState = CLIENT_AIR_ON_STATE;
                else if (VacuumState.nSemVacState == VAC_EVAC_STATE    ) nClientSemVacState = CLIENT_VAC_EVAC_STATE;
                else if (VacuumState.nSemVacState == VAC_OK_STATE      ) nClientSemVacState = CLIENT_VAC_OK_STATE;
                else if (VacuumState.nSemVacState == HV_ON_STATE       ) nClientSemVacState = CLIENT_HV_ON_STATE;
                else if (VacuumState.nSemVacState == CNC5_GO_SAM_LOAD  ) nClientSemVacState = CLIENT_CNC5_GO_SAM_LOAD;
                else if (VacuumState.nSemVacState == CNC5_GO_SAM_UNLOAD) nClientSemVacState = CLIENT_CNC5_GO_SAM_UNLOAD;
                else if (VacuumState.nSemVacState == HV_VAC_OK_STATE   ) nClientSemVacState = CLIENT_VAC_OK_STATE;  // CLIENT_HV_VAC_OK_STATE;
                else if (VacuumState.nSemVacState == HV_AIR_ON_STATE   ) nClientSemVacState = CLIENT_AIR_ON_STATE;  // CLIENT_HV_AIR_ON_STATE;
                else if (VacuumState.nSemVacState == HV_EVAC_STATE     ) nClientSemVacState = CLIENT_VAC_EVAC_STATE;// CLIENT_HV_EVAC_STATE;
            }


            // 진공상태 변경 확인
            if (VacuumState.nProcess == 1)
            {
                // 진공상태 변경 및 UI 처리
                // 진공상태 표시 ( SEM_POWER_OFF, SEM_EVAC_PROCCED, SEM_VACUUM_STATE, SEM_HV_ON ... )
                m_wndVacuumControlDlg.SetIntegratedVacuumStateInfo(nClientSemVacState);
                DisplayVacuumStateInfo(m_VacuumBeamAlignFocusData.m_nIntegratedVacuumState);

            }

            // 챔버 진공값 변경 확인
            else if (VacuumState.nProcess == 2)
            {
                // 진공상태 변경 및 UI 처리
                // 진공상태 표시 ( SEM_POWER_OFF, SEM_EVAC_PROCCED, SEM_VACUUM_STATE, SEM_HV_ON ... )
                //m_wndVacuumControlDlg.SetIntegratedVacuumStateInfo(nClientSemVacState);
                //DisplayVacuumStateInfo(m_VacuumBeamAlignFocusData.m_nIntegratedVacuumState);

                // 챔버 진공 값 적용
                m_IntegratedData.nSemVacValue  = VacuumState.nSemVacValue;
                m_IntegratedData.dSemVacValue1 = VacuumState.dSemVacValue1;
                // 진공값 변경 및 UI 처리
                // 진공게이지 값 표시
                m_wndVacuumControlDlg.SetChamberVacuumValue(m_IntegratedData.nSemVacValue);

            }

            // Emission Current 값 변경 확인
            else if (VacuumState.nProcess == 3)
            {
                // Emission Current 값 적용
                m_IntegratedData.nEmCurrentLevel = VacuumState.nEmCurrentLevel;
                // Emission Current 값 변경 및 UI 처리
                strText = "";
                strText = Format("%d", m_IntegratedData.nEmCurrentLevel);
                m_wndBeamControlDlg.m_SemEmissionCurrentCaption.SetTextDataString(strText);

            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:17125 ----
        // USB CONTROL DATA : GUN HV CONTROL DATA
        public int OnSemControlGunHVData(int wParam, object lParam)
        {
            if (lParam == null) return 0;

            // 2020.05.04
            USB_DEVICE_DATA_INFO UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            // DATA 복사
            UsbDeviceDataInfo.CopyFrom((USB_DEVICE_DATA_INFO)lParam);

            // GUN HV DATA 정보
            SEM_CONTROL_GUNHV_DATA GunHVData = new SEM_CONTROL_GUNHV_DATA();

            // DATA 복사
            GunHVData.nGunHVValue = UsbDeviceDataInfo.nData[1];

            // GUN HV 값 변경 확인
            if (m_IntegratedData.nGunHVValue != GunHVData.nGunHVValue)
            {
                // Gun HV 처리를 위한 HV 설정값(목표설정값) 적용
                m_IntegratedData.nGunHVValue = GunHVData.nGunHVValue;
            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:17153 ----
        // USB CONTROL DATA : FILAMENT CONTROL DATA
        public int OnSemControlFilamentData(int wParam, object lParam)
        {
            if (lParam == null) return 0;

            // FILAMENT DATA 정보
            SEM_CONTROL_FILAMENT_DATA FilamentData = new SEM_CONTROL_FILAMENT_DATA();
            // LONG DATA 복사
            FilamentData.CopyFrom((SEM_CONTROL_FILAMENT_DATA)lParam);

            // FILAMENT 값 변경 확인
            if (m_IntegratedData.nFilamentValue != FilamentData.nFilamentValue)
            {
                // 필라멘트(Filament) HV 목표 설정값 적용
                m_IntegratedData.nFilamentValue = FilamentData.nFilamentValue;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nFilamentValue);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:17179 ----
        // USB CONTROL DATA : BIAS CONTROL DATA
        public int OnSemControlBiasData(int wParam, object lParam)
        {
            if (lParam == null) return 0;

            // BIAS DATA 정보
            SEM_CONTROL_BIAS_DATA BiasData = new SEM_CONTROL_BIAS_DATA();
            // LONG DATA 복사
            BiasData.CopyFrom((SEM_CONTROL_BIAS_DATA)lParam);

            // BIAS 값 변경 확인
            if (m_IntegratedData.nBiasValue != BiasData.nBiasValue)
            {
                // 바이어스(Bias) HV 목표 설정값 적용
                m_IntegratedData.nBiasValue = BiasData.nBiasValue;
                // 변경 및 UI 처리
            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:17201 ----
        // USB CONTROL DATA : IMAGE SHIFT CONTROL DATA
        public int OnSemControlImageShiftData(int wParam, object lParam)
        {
            if (lParam == null) return 0;

            // IMAGE SHIFT DATA 정보
            SEM_CONTROL_IMAGESHIFT_DATA ImageShiftData = new SEM_CONTROL_IMAGESHIFT_DATA();
            // LONG DATA 복사
            ImageShiftData.CopyFrom((SEM_CONTROL_IMAGESHIFT_DATA)lParam);

            // IMAGE SHIFT X 값 변경 확인
            if (m_IntegratedData.nISXValue != ImageShiftData.nISXValue)
            {
                // IMAGE SHIFT 값 적용
                m_IntegratedData.nISXValue = ImageShiftData.nISXValue;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nISXValue);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }

            // IMAGE SHIFT Y 값 변경 확인
            if (m_IntegratedData.nISYValue != ImageShiftData.nISYValue)
            {
                // IMAGE SHIFT 값 적용
                m_IntegratedData.nISYValue = ImageShiftData.nISYValue;
                // 변경 및 UI 처리
            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:17235 ----
        // USB CONTROL DATA : GUN ALIGN CONTROL DATA
        public int OnSemControlGunAlignData(int wParam, object lParam)
        {
            if (lParam == null) return 0;

            // GUN ALIGN DATA 정보
            SEM_CONTROL_GUNALIGN_DATA GunAlignData = new SEM_CONTROL_GUNALIGN_DATA();
            // LONG DATA 복사
            GunAlignData.CopyFrom((SEM_CONTROL_GUNALIGN_DATA)lParam);

            // GUN ALIGN X 값 변경 확인
            if (m_IntegratedData.nGAXValue != GunAlignData.nGAXValue)
            {
                // GUN ALIGN 값 적용
                m_IntegratedData.nGAXValue = GunAlignData.nGAXValue;
                // 변경 및 UI 처리
            }

            // GUN ALIGN Y 값 변경 확인
            if (m_IntegratedData.nGAYValue != GunAlignData.nGAYValue)
            {
                // GUN ALIGN 값 적용
                m_IntegratedData.nGAYValue = GunAlignData.nGAYValue;
                // 변경 및 UI 처리
            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:17265 ----
        // USB CONTROL DATA : STIG CONTROL DATA
        public int OnSemControlStigData(int wParam, object lParam)
        {
            if (lParam == null) return 0;

            // STIG DATA 정보
            SEM_CONTROL_STIG_DATA StigData = new SEM_CONTROL_STIG_DATA();
            // LONG DATA 복사
            StigData.CopyFrom((SEM_CONTROL_STIG_DATA)lParam);

            // STIG X 값 변경 확인
            if (m_IntegratedData.nStigXValue != StigData.nStigXValue)
            {
                // STIG 값 적용
                m_IntegratedData.nStigXValue = StigData.nStigXValue;
                // 변경 및 UI 처리
            }

            // STIG Y 값 변경 확인
            if (m_IntegratedData.nStigYValue != StigData.nStigYValue)
            {
                // STIG 값 적용
                m_IntegratedData.nStigYValue = StigData.nStigYValue;
                // 변경 및 UI 처리
            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:17295 ----
        // USB CONTROL DATA : CONTRAST & BRIGHTNESS CONTROL DATA
        public int OnSemControlContrastBrightData(int wParam, object lParam)
        {
            if (lParam == null) return 0;

            // CONTRAST & BRIGHTNESS DATA 정보
            SEM_CONTROL_CONTBRIGHT_DATA ContrastBright = new SEM_CONTROL_CONTBRIGHT_DATA();
            // LONG DATA 복사
            ContrastBright.CopyFrom((SEM_CONTROL_CONTBRIGHT_DATA)lParam);

            // CONTRAST 값 변경 확인
            if (m_IntegratedData.nContrastValue != ContrastBright.nContrastValue)
            {
                // CONTRAST 값 적용
                m_IntegratedData.nContrastValue = ContrastBright.nContrastValue;
                // 변경 및 UI 처리
            }

            // BRIGHTNESS 값 변경 확인
            if (m_IntegratedData.nBrightValue != ContrastBright.nBrightValue)
            {
                // BRIGHTNESS 값 적용
                m_IntegratedData.nBrightValue = ContrastBright.nBrightValue;
                // 변경 및 UI 처리
            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:17325 ----
        // USB CONTROL DATA : FOCUS & SPOT SIZE CONTROL DATA
        public int OnSemControlFocusData(int wParam, object lParam)
        {
            if (lParam == null) return 0;

            // FOCUS & SPOT SIZE DATA 정보
            SEM_CONTROL_FOCUS_DATA FocusSpot = new SEM_CONTROL_FOCUS_DATA();
            // LONG DATA 복사
            FocusSpot.CopyFrom((SEM_CONTROL_FOCUS_DATA)lParam);

            // FOCUS COARSE 값 변경 확인
            if (m_IntegratedData.nFocusCoarse != FocusSpot.nFocusCoarse)
            {
                // FOCUS 값 적용
                m_IntegratedData.nFocusCoarse = FocusSpot.nFocusCoarse;
                // 변경 및 UI 처리
            }

            // FOCUS FINE 값 변경 확인
            if (m_IntegratedData.nFocusFine != FocusSpot.nFocusFine)
            {
                // FOCUS 값 적용
                m_IntegratedData.nFocusFine = FocusSpot.nFocusFine;
                // 변경 및 UI 처리
            }

            // SPOT SIZE 값 변경 확인
            if (m_IntegratedData.nSpotSize != FocusSpot.nSpotSize)
            {
                // SPOT SIZE 값 적용
                m_IntegratedData.nSpotSize = FocusSpot.nSpotSize;
                // 변경 및 UI 처리
            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:17363 ----
        // USB CONTROL DATA : MAGNIFICATION CONTROL DATA
        public int OnSemControlMagnificationData(int wParam, object lParam)
        {
            if (lParam == null) return 0;

            // MAGNIFICATION DATA 정보
            SEM_CONTROL_MAGNIFICATION_DATA Magnification = new SEM_CONTROL_MAGNIFICATION_DATA();
            // LONG DATA 복사
            Magnification.CopyFrom((SEM_CONTROL_MAGNIFICATION_DATA)lParam);

            // MAGNIFICATION 값 변경 확인
            if (m_IntegratedData.nMagStepIndex != Magnification.nMagStepIndex)
            {
                // MAGNIFICATION 배율 인덱스 값 적용
                m_IntegratedData.nMagStepIndex = Magnification.nMagStepIndex;
                // 변경 및 UI 처리
            }

            // MAGNIFICATION 값 변경 확인
            if (m_IntegratedData.wMagRelayNumber != Magnification.wMagRelayNumber)
            {
                // MAGNIFICATION 배율에 따른 릴레이 값 적용
                m_IntegratedData.wMagRelayNumber = (ushort)(Magnification.wMagRelayNumber);
                // 변경 및 UI 처리
            }

            // MAGNIFICATION 값 변경 확인
            if (m_IntegratedData.wZoomXValue != Magnification.wZoomXValue)
            {
                // MAGNIFICATION X축 배율 설정 값 적용
                m_IntegratedData.wZoomXValue = (ushort)(Magnification.wZoomXValue);
                // 변경 및 UI 처리
                m_wndScanCalibrationToolDlg.SetZoomXIntegratedPosition(m_IntegratedData.wZoomXValue);

            }

            // MAGNIFICATION 값 변경 확인
            if (m_IntegratedData.wZoomYValue != Magnification.wZoomYValue)
            {
                // MAGNIFICATION Y축 배율 설정 값 적용
                m_IntegratedData.wZoomYValue = (ushort)(Magnification.wZoomYValue);
                // 변경 및 UI 처리
                m_wndScanCalibrationToolDlg.SetZoomYIntegratedPosition(m_IntegratedData.wZoomYValue);

            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:17413 ----
        // USB CONTROL DATA : ZOOM-RELAY & ZOOM X-Y CONTROL DATA
        public int OnSemControlZoomXYData(int wParam, object lParam)
        {
            if (lParam == null) return 0;
            
            // ZOOM X-Y DATA 정보
            SEM_CONTROL_ZOOM_XY_DATA ZoomXYData = new SEM_CONTROL_ZOOM_XY_DATA();
            // LONG DATA 복사
            ZoomXYData.CopyFrom((SEM_CONTROL_ZOOM_XY_DATA)lParam);

            // ZOOM X-Y DATA 값 변경 확인
            if (m_IntegratedData.wMagRelayNumber != ZoomXYData.wMagRelayNumber)
            {
                // ZOOM X-Y DATA 배율에 따른 릴레이 값 적용
                m_IntegratedData.wMagRelayNumber = (ushort)(ZoomXYData.wMagRelayNumber);
                // 변경 및 UI 처리

            }

            // ZOOM X-Y DATA 값 변경 확인
            if (m_IntegratedData.wZoomXValue != ZoomXYData.wZoomXValue)
            {
                // ZOOM X축 배율 설정 값 적용
                m_IntegratedData.wZoomXValue = (ushort)(ZoomXYData.wZoomXValue); // Zoom X 값
                m_IntegratedData.nZoomXValue = ZoomXYData.nZoomXValue; // Zoom X 계산값 (USB 적용값)
                // 변경 및 UI 처리
                m_wndScanCalibrationToolDlg.SetZoomXIntegratedPosition(m_IntegratedData.wZoomXValue);

            }

            // ZOOM X-Y DATA 값 변경 확인
            if (m_IntegratedData.wZoomYValue != ZoomXYData.wZoomYValue)
            {
                // ZOOM Y축 배율 설정 값 적용
                m_IntegratedData.wZoomYValue = (ushort)(ZoomXYData.wZoomYValue); // Zoom Y 값
                m_IntegratedData.nZoomYValue = ZoomXYData.nZoomYValue; // Zoom Y 계산값 (USB 적용값)
                // 변경 및 UI 처리
                m_wndScanCalibrationToolDlg.SetZoomYIntegratedPosition(m_IntegratedData.wZoomYValue);

            }
            
            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:17458 ----
        // USB CONTROL DATA : ZOOM X-Y CONTROL DATA
        public int OnSemControlZoomXYValueData(int wParam, object lParam)
        {
            if (lParam == null) return 0;
            
            // 2020.05.04
            // ZOOM X-Y DATA 정보
            USB_DEVICE_DATA_INFO UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            // LONG DATA 복사
            UsbDeviceDataInfo.CopyFrom((USB_DEVICE_DATA_INFO)lParam);

            SEM_CONTROL_ZOOM_XY_DATA ZoomXYData = new SEM_CONTROL_ZOOM_XY_DATA();

            // DATA 복사
            ZoomXYData.nZoomXValue     = UsbDeviceDataInfo.nData[0]; // ZOOM X
            ZoomXYData.nZoomYValue     = UsbDeviceDataInfo.nData[1]; // ZOOM Y
            ZoomXYData.wMagRelayNumber = (ushort)(UsbDeviceDataInfo.nData[2]); // ZOOM RELAY NUMBER
            ZoomXYData.wZoomXValue     = (ushort)ZoomXYData.nZoomXValue;
            ZoomXYData.wZoomYValue     = (ushort)ZoomXYData.nZoomYValue;

            // ZOOM X-Y DATA 값 변경 확인
            if (m_IntegratedData.wMagRelayNumber != ZoomXYData.wMagRelayNumber)
            {
                // ZOOM X-Y DATA 배율에 따른 릴레이 값 적용
                m_IntegratedData.wMagRelayNumber = (ushort)(ZoomXYData.wMagRelayNumber);
                // 변경 및 UI 처리

            }

            // ZOOM X-Y DATA 값 변경 확인
            if (m_IntegratedData.wZoomXValue != ZoomXYData.wZoomXValue)
            {
                // ZOOM X축 배율 설정 값 적용
                m_IntegratedData.wZoomXValue = (ushort)(ZoomXYData.wZoomXValue); // Zoom X 값
                m_IntegratedData.nZoomXValue = ZoomXYData.nZoomXValue; // Zoom X 계산값 (USB 적용값)
                // 변경 및 UI 처리
                m_wndScanCalibrationToolDlg.SetZoomXIntegratedPosition(m_IntegratedData.wZoomXValue);

            }

            // ZOOM X-Y DATA 값 변경 확인
            if (m_IntegratedData.wZoomYValue != ZoomXYData.wZoomYValue)
            {
                // ZOOM Y축 배율 설정 값 적용
                m_IntegratedData.wZoomYValue = (ushort)(ZoomXYData.wZoomYValue); // Zoom Y 값
                m_IntegratedData.nZoomYValue = ZoomXYData.nZoomYValue; // Zoom Y 계산값 (USB 적용값)
                // 변경 및 UI 처리
                m_wndScanCalibrationToolDlg.SetZoomYIntegratedPosition(m_IntegratedData.wZoomYValue);

            }
            
            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:17514 ----
        // USB CONTROL DATA : WD CURRENT POSITION CONTROL DATA
        public int OnSemControlWDCurPositionData(int wParam, object lParam)
        {
            if (lParam == null) return 0;

            // WD CURRENT POSITION DATA 정보
            SEM_CONTROL_WDCURPOS_DATA WDCurPosition = new SEM_CONTROL_WDCURPOS_DATA();
            // LONG DATA 복사
            WDCurPosition.CopyFrom((SEM_CONTROL_WDCURPOS_DATA)lParam);

            // WD CURRENT POSITION 값 변경 확인
            if (m_IntegratedData.nWDPosition != WDCurPosition.nWDPosition)
            {
                // WD CURRENT POSITION 값 적용
                m_IntegratedData.nWDPosition = WDCurPosition.nWDPosition;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nWDPosition);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }

            // WD CURRENT POSITION - Z축 값 변경 확인
            if (m_IntegratedData.dCurZPos != WDCurPosition.dCurZPos)
            {
                // WD CURRENT POSITION - Z축 값 적용
                m_IntegratedData.dCurZPos = WDCurPosition.dCurZPos;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.dCurZPos);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:17552 ----
        // USB CONTROL DATA : HW SETUP의 SCAN MODE에 따른 X/Y AMP, OFFSET DATA
        public int OnSemHWSetupXYScanCalibrationData(int wParam, object lParam)
        {
            if (lParam == null) return 0;

            // HW SETUP의 SCAN MODE에 따른 X/Y AMP, OFFSET DATA 정보
            SEM_CONTROL_XYSCANCALIBRATION_DATA XYScanCalibration = new SEM_CONTROL_XYSCANCALIBRATION_DATA();
            // LONG DATA 복사
            XYScanCalibration.CopyFrom((SEM_CONTROL_XYSCANCALIBRATION_DATA)lParam);

            // HW SETUP의 SCAN MODE에 따른 X/Y AMP, OFFSET DATA 변경 확인
            if (m_IntegratedData.nXImageAmp != XYScanCalibration.nXImageAmp)
            {
                // X-AMP 값 적용
                m_IntegratedData.nXImageAmp = XYScanCalibration.nXImageAmp;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nXImageAmp);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }
            // HW SETUP의 SCAN MODE에 따른 X/Y AMP, OFFSET DATA 변경 확인
            if (m_IntegratedData.nXImageOffset != XYScanCalibration.nXImageOffset)
            {
                // X-OFFSET 값 적용
                m_IntegratedData.nXImageOffset = XYScanCalibration.nXImageOffset;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nXImageOffset);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }

            // HW SETUP의 SCAN MODE에 따른 X/Y AMP, OFFSET DATA 변경 확인
            if (m_IntegratedData.nYImageAmp != XYScanCalibration.nYImageAmp)
            {
                // Y-AMP 값 적용
                m_IntegratedData.nYImageAmp = XYScanCalibration.nYImageAmp;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nYImageAmp);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }
            // HW SETUP의 SCAN MODE에 따른 X/Y AMP, OFFSET DATA 변경 확인
            if (m_IntegratedData.nYImageOffset != XYScanCalibration.nYImageOffset)
            {
                // Y-OFFSET 값 적용
                m_IntegratedData.nYImageOffset = XYScanCalibration.nYImageOffset;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nYImageOffset);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }

            // HW SETUP의 SCAN MODE에 따른 X/Y AMP, OFFSET DATA 변경 확인
            if (memcmp(m_IntegratedData.nXScanAmpVal, XYScanCalibration.nXScanAmpVal, sizeof(int)*MAX_SCAN_MODE) != NULL)
            {
                // X-AMP 설정값 적용
                memcpy(m_IntegratedData.nXScanAmpVal, XYScanCalibration.nXScanAmpVal, sizeof(int)*MAX_SCAN_MODE);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nXScanAmpVal[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }
            // HW SETUP의 SCAN MODE에 따른 X/Y AMP, OFFSET DATA 변경 확인
            if (memcmp(m_IntegratedData.nXScanOffset, XYScanCalibration.nXScanOffset, sizeof(int)*MAX_SCAN_MODE) != NULL)
            {
                // X-OFFSET 설정값 적용
                memcpy(m_IntegratedData.nXScanOffset, XYScanCalibration.nXScanOffset, sizeof(int)*MAX_SCAN_MODE);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nXScanOffset[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }

            // HW SETUP의 SCAN MODE에 따른 X/Y AMP, OFFSET DATA 변경 확인
            if (memcmp(m_IntegratedData.nYScanAmpVal, XYScanCalibration.nYScanAmpVal, sizeof(int)*MAX_SCAN_MODE) != NULL)
            {
                // Y-AMP 설정값 적용
                memcpy(m_IntegratedData.nYScanAmpVal, XYScanCalibration.nYScanAmpVal, sizeof(int)*MAX_SCAN_MODE);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nYScanAmpVal[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }
            // HW SETUP의 SCAN MODE에 따른 X/Y AMP, OFFSET DATA 변경 확인
            if (memcmp(m_IntegratedData.nYScanOffset, XYScanCalibration.nYScanOffset, sizeof(int)*MAX_SCAN_MODE) != NULL)
            {
                // Y-OFFSET 설정값 적용
                memcpy(m_IntegratedData.nYScanOffset, XYScanCalibration.nYScanOffset, sizeof(int)*MAX_SCAN_MODE);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nYScanOffset[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:17658 ----
        // USB CONTROL DATA : HW SETUP의 SCAN MODE에 따른 SCAN IMAGE SKIP, OVER DATA
        public int OnSemHWSetupXYScanSkipOverData(int wParam, object lParam)
        {
            if (lParam == null) return 0;

            // HW SETUP의 SCAN MODE에 따른 SCAN IMAGE SKIP, OVER DATA 정보
            SEM_CONTROL_XYSCANSKIPOVER_DATA XYScanSkipOver = new SEM_CONTROL_XYSCANSKIPOVER_DATA();
            // LONG DATA 복사
            XYScanSkipOver.CopyFrom((SEM_CONTROL_XYSCANSKIPOVER_DATA)lParam);

            // HW SETUP의 SCAN MODE에 따른 SCAN IMAGE SKIP, OVER 값 변경 확인
            if (memcmp(m_IntegratedData.nSkipPixNum, XYScanSkipOver.nSkipPixNum, sizeof(int)*MAX_SCAN_MODE) != NULL)
            {
                // XY SCAN SKIP 설정값 적용
                memcpy(m_IntegratedData.nSkipPixNum, XYScanSkipOver.nSkipPixNum, sizeof(int)*MAX_SCAN_MODE);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nSkipPixNum[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }

            // HW SETUP의 SCAN MODE에 따른 SCAN IMAGE SKIP, OVER 값 변경 확인
            if (memcmp(m_IntegratedData.nOverSampleNum, XYScanSkipOver.nOverSampleNum, sizeof(int)*MAX_SCAN_MODE) != NULL)
            {
                // XY SCAN OVER 설정값 적용
                memcpy(m_IntegratedData.nOverSampleNum, XYScanSkipOver.nOverSampleNum, sizeof(int)*MAX_SCAN_MODE);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nOverSampleNum[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:17696 ----
        // USB CONTROL DATA : HW SETUP의 SCAN MODE에 따른 UP, DOWN SAMPLE CLOCK DATA
        public int OnSemHWSetupXYScanUpDownClockData(int wParam, object lParam)
        {
            if (lParam == null) return 0;

            // HW SETUP의 SCAN MODE에 따른 UP, DOWN SAMPLE CLOCK DATA 정보
            SEM_CONTROL_XYSCANUPDOWNCLOCK_DATA XYScanUpDown = new SEM_CONTROL_XYSCANUPDOWNCLOCK_DATA();
            // LONG DATA 복사
            XYScanUpDown.CopyFrom((SEM_CONTROL_XYSCANUPDOWNCLOCK_DATA)lParam);

            //  HW SETUP의 SCAN MODE에 따른 UP, DOWN SAMPLE CLOCK DATA 변경 확인
            if (m_IntegratedData.nXScanUp != XYScanUpDown.nXScanUp)
            {
                // X SCAN UP 값 적용
                m_IntegratedData.nXScanUp = XYScanUpDown.nXScanUp;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nXScanUp);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }
            // HW SETUP의 SCAN MODE에 따른 UP, DOWN SAMPLE CLOCK 값 변경 확인
            if (memcmp(m_IntegratedData.nXScanUpNum, XYScanUpDown.nXScanUpNum, sizeof(int)*MAX_SCAN_MODE) != NULL)
            {
                // X SCAN UP 설정값 적용
                memcpy(m_IntegratedData.nXScanUpNum, XYScanUpDown.nXScanUpNum, sizeof(int)*MAX_SCAN_MODE);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nXScanUpNum[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }

            //  HW SETUP의 SCAN MODE에 따른 UP, DOWN SAMPLE CLOCK DATA 변경 확인
            if (m_IntegratedData.nXBlankWidth != XYScanUpDown.nXBlankWidth)
            {
                // X SCAN DOWN 값 적용
                m_IntegratedData.nXBlankWidth = XYScanUpDown.nXBlankWidth;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nXBlankWidth);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }
            // HW SETUP의 SCAN MODE에 따른 UP, DOWN SAMPLE CLOCK 값 변경 확인
            if (memcmp(m_IntegratedData.nXScanDownNum, XYScanUpDown.nXScanDownNum, sizeof(int)*MAX_SCAN_MODE) != NULL)
            {
                // X SCAN DOWN 설정값 적용
                memcpy(m_IntegratedData.nXScanDownNum, XYScanUpDown.nXScanDownNum, sizeof(int)*MAX_SCAN_MODE);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nXScanDownNum[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }

            //  HW SETUP의 SCAN MODE에 따른 UP, DOWN SAMPLE CLOCK DATA 변경 확인
            if (m_IntegratedData.nYScanUp != XYScanUpDown.nYScanUp)
            {
                // Y SCAN UP 값 적용
                m_IntegratedData.nYScanUp = XYScanUpDown.nYScanUp;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nYScanUp);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }
            // HW SETUP의 SCAN MODE에 따른 UP, DOWN SAMPLE CLOCK 값 변경 확인
            if (memcmp(m_IntegratedData.nYScanUpNum, XYScanUpDown.nYScanUpNum, sizeof(int)*MAX_SCAN_MODE) != NULL)
            {
                // Y SCAN UP 설정값 적용
                memcpy(m_IntegratedData.nYScanUpNum, XYScanUpDown.nYScanUpNum, sizeof(int)*MAX_SCAN_MODE);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nXScanUpNum[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }

            //  HW SETUP의 SCAN MODE에 따른 UP, DOWN SAMPLE CLOCK DATA 변경 확인
            if (m_IntegratedData.nYBlankWidth != XYScanUpDown.nYBlankWidth)
            {
                // Y SCAN DOWN 값 적용
                m_IntegratedData.nYBlankWidth = XYScanUpDown.nYBlankWidth;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nYBlankWidth);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }
            // HW SETUP의 SCAN MODE에 따른 UP, DOWN SAMPLE CLOCK 값 변경 확인
            if (memcmp(m_IntegratedData.nYScanDownNum, XYScanUpDown.nYScanDownNum, sizeof(int)*MAX_SCAN_MODE) != NULL)
            {
                // Y SCAN DOWN 설정값 적용
                memcpy(m_IntegratedData.nYScanDownNum, XYScanUpDown.nYScanDownNum, sizeof(int)*MAX_SCAN_MODE);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nYScanDownNum[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:17802 ----
        // USB CONTROL DATA : HW SETUP의 SCAN MODE에 따른 FRAME SAMPLE CLOCK DATA
        public int OnSemHWSetupXYScanFrameClockData(int wParam, object lParam)
        {
            if (lParam == null) return 0;

            // HW SETUP의 SCAN MODE에 따른 FRAME SAMPLE CLOCK DATA 정보
            SEM_CONTROL_XYSCANFRAMECLOCK_DATA XYScanFrameClock = new SEM_CONTROL_XYSCANFRAMECLOCK_DATA();
            // LONG DATA 복사
            XYScanFrameClock.CopyFrom((SEM_CONTROL_XYSCANFRAMECLOCK_DATA)lParam);

            //  HW SETUP의 SCAN MODE에 따른 FRAME SAMPLE CLOCK DATA 변경 확인
            if (m_IntegratedData.nPLLClk != XYScanFrameClock.nPLLClk)
            {
                // 고정값->PLL Scan Clock (48Mhz, 48000000) 값 적용
                m_IntegratedData.nPLLClk = XYScanFrameClock.nPLLClk;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nPLLClk);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }

            // HW SETUP의 SCAN MODE에 따른 FRAME SAMPLE CLOCK 값 변경 확인
            if (memcmp(m_IntegratedData.nSyncVal, XYScanFrameClock.nSyncVal, sizeof(int)*MAX_SCAN_MODE) != NULL)
            {
                // 고정값->Scan Clock Sync Value 적용
                memcpy(m_IntegratedData.nSyncVal, XYScanFrameClock.nSyncVal, sizeof(int)*MAX_SCAN_MODE);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nSyncVal[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }

            // HW SETUP의 SCAN MODE에 따른 FRAME SAMPLE CLOCK 값 변경 확인
            if (memcmp(m_IntegratedData.nScanClk, XYScanFrameClock.nScanClk, sizeof(int)*MAX_SCAN_MODE) != NULL)
            {
                // 고정값->Scan Clock Value 적용
                memcpy(m_IntegratedData.nScanClk, XYScanFrameClock.nScanClk, sizeof(int)*MAX_SCAN_MODE);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nScanClk[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }

            // HW SETUP의 SCAN MODE에 따른 FRAME SAMPLE CLOCK 값 변경 확인
            if (memcmp(m_IntegratedData.nFrameSec, XYScanFrameClock.nFrameSec, sizeof(int)*MAX_SCAN_MODE) != NULL)
            {
                // 설정값->Scan Mode 에 따른 1초 당 최대 프레임 수 적용
                memcpy(m_IntegratedData.nFrameSec, XYScanFrameClock.nFrameSec, sizeof(int)*MAX_SCAN_MODE);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nFrameSec[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }

            // HW SETUP의 SCAN MODE에 따른 FRAME SAMPLE CLOCK 값 변경 확인
            if (memcmp(m_IntegratedData.nFrameSecVal, XYScanFrameClock.nFrameSecVal, sizeof(int)*MAX_SCAN_MODE) != NULL)
            {
                // 설정값->Scan Mode 에 따른 Frame/sec 보관 적용
                memcpy(m_IntegratedData.nFrameSecVal, XYScanFrameClock.nFrameSecVal, sizeof(int)*MAX_SCAN_MODE);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nFrameSecVal[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);

            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:17876 ----
        // USB CONTROL DATA : HW SETUP의 SCAN MODE에 따른 FRAME BITMAP SIZE DATA
        public int OnSemHWSetupXYScanFrameBitmapData(int wParam, object lParam)
        {
            if (lParam == null) return 0;

            // HW SETUP의 SCAN MODE에 따른 FRAME BITMAP SIZE DATA 정보
            SEM_CONTROL_XYSCANFRAMEBMP_DATA XYScanFrameBitmap = new SEM_CONTROL_XYSCANFRAMEBMP_DATA();
            // LONG DATA 복사
            XYScanFrameBitmap.CopyFrom((SEM_CONTROL_XYSCANFRAMEBMP_DATA)lParam);

            //  HW SETUP의 SCAN MODE에 따른 FRAME BITMAP SIZE DATA 변경 확인
            if (m_IntegratedData.nXBmpPixelVal != XYScanFrameBitmap.nXBmpPixelVal)
            {
                // 사용값->Scan Mode 에 따른 X축 Pixel 갯수 값 적용
                m_IntegratedData.nXBmpPixelVal = XYScanFrameBitmap.nXBmpPixelVal;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nXBmpPixelVal);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            //  HW SETUP의 SCAN MODE에 따른 FRAME BITMAP SIZE DATA 변경 확인
            if (m_IntegratedData.nYBmpPixelVal != XYScanFrameBitmap.nYBmpPixelVal)
            {
                // 사용값->Scan Mode 에 따른 Y축 Pixel 갯수 값 적용
                m_IntegratedData.nYBmpPixelVal = XYScanFrameBitmap.nYBmpPixelVal;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nYBmpPixelVal);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            //  HW SETUP의 SCAN MODE에 따른 FRAME BITMAP SIZE DATA 변경 확인
            if (m_IntegratedData.nXBmpOffsetVal != XYScanFrameBitmap.nXBmpOffsetVal)
            {
                // 사용값->Scan Mode 에 따른 X축 Offset Pixel 갯수 값 적용
                m_IntegratedData.nXBmpOffsetVal = XYScanFrameBitmap.nXBmpOffsetVal;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nXBmpOffsetVal);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            //  HW SETUP의 SCAN MODE에 따른 FRAME BITMAP SIZE DATA 변경 확인
            if (m_IntegratedData.nYBmpOffsetVal != XYScanFrameBitmap.nYBmpOffsetVal)
            {
                // 사용값->Scan Mode 에 따른 Y축 Offset Pixel 갯수 값 적용
                m_IntegratedData.nYBmpOffsetVal = XYScanFrameBitmap.nYBmpOffsetVal;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nYBmpOffsetVal);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            //  HW SETUP의 SCAN MODE에 따른 FRAME BITMAP SIZE DATA 변경 확인
            if (m_IntegratedData.nWAverageNumVal != XYScanFrameBitmap.nWAverageNumVal)
            {
                // 사용값->Scan Mode 에 따른 Average Pixel 갯수 값 적용
                m_IntegratedData.nWAverageNumVal = XYScanFrameBitmap.nWAverageNumVal;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nWAverageNumVal);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }

            // HW SETUP의 SCAN MODE에 따른 FRAME BITMAP SIZE 값 변경 확인
            if (memcmp(m_IntegratedData.nFPGABmpSize, XYScanFrameBitmap.nFPGABmpSize, sizeof(int)*MAX_SCAN_MODE) != NULL)
            {
                // FPGA BMP Size 적용
                memcpy(m_IntegratedData.nFPGABmpSize, XYScanFrameBitmap.nFPGABmpSize, sizeof(int)*MAX_SCAN_MODE);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nFPGABmpSize[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            // HW SETUP의 SCAN MODE에 따른 FRAME BITMAP SIZE 값 변경 확인
            if (memcmp(m_IntegratedData.nScanCodeA, XYScanFrameBitmap.nScanCodeA, sizeof(int)*MAX_SCAN_MODE) != NULL)
            {
                // 설정값->Scan Code Type A 적용
                memcpy(m_IntegratedData.nScanCodeA, XYScanFrameBitmap.nScanCodeA, sizeof(int)*MAX_SCAN_MODE);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nScanCodeA[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            // HW SETUP의 SCAN MODE에 따른 FRAME BITMAP SIZE 값 변경 확인
            if (memcmp(m_IntegratedData.nScanCodeB, XYScanFrameBitmap.nScanCodeB, sizeof(int)*MAX_SCAN_MODE) != NULL)
            {
                // 설정값->Scan Code Type B 적용
                memcpy(m_IntegratedData.nScanCodeB, XYScanFrameBitmap.nScanCodeB, sizeof(int)*MAX_SCAN_MODE);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nScanCodeB[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            // HW SETUP의 SCAN MODE에 따른 FRAME BITMAP SIZE 값 변경 확인
            if (memcmp(m_IntegratedData.nXBmpPixel, XYScanFrameBitmap.nXBmpPixel, sizeof(int)*MAX_SCAN_MODE) != NULL)
            {
                // 설정값->Scan Mode 에 따른 X축 Pixel 갯수 적용
                memcpy(m_IntegratedData.nXBmpPixel, XYScanFrameBitmap.nXBmpPixel, sizeof(int)*MAX_SCAN_MODE);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nXBmpPixel[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            // HW SETUP의 SCAN MODE에 따른 FRAME BITMAP SIZE 값 변경 확인
            if (memcmp(m_IntegratedData.nYBmpPixel, XYScanFrameBitmap.nYBmpPixel, sizeof(int)*MAX_SCAN_MODE) != NULL)
            {
                // 설정값->Scan Mode 에 따른 Y축 Pixel 갯수 적용
                memcpy(m_IntegratedData.nYBmpPixel, XYScanFrameBitmap.nYBmpPixel, sizeof(int)*MAX_SCAN_MODE);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nYBmpPixel[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            // HW SETUP의 SCAN MODE에 따른 FRAME BITMAP SIZE 값 변경 확인
            if (memcmp(m_IntegratedData.nXBmpOffset, XYScanFrameBitmap.nXBmpOffset, sizeof(int)*MAX_SCAN_MODE) != NULL)
            {
                // 설정값->Scan Mode 에 따른 X축 Offset Pixel 갯수 적용
                memcpy(m_IntegratedData.nXBmpOffset, XYScanFrameBitmap.nXBmpOffset, sizeof(int)*MAX_SCAN_MODE);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nXBmpOffset[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            // HW SETUP의 SCAN MODE에 따른 FRAME BITMAP SIZE 값 변경 확인
            if (memcmp(m_IntegratedData.nYBmpOffset, XYScanFrameBitmap.nYBmpOffset, sizeof(int)*MAX_SCAN_MODE) != NULL)
            {
                // 설정값->Scan Mode 에 따른 Y축 Offset Pixel 갯수 적용
                memcpy(m_IntegratedData.nYBmpOffset, XYScanFrameBitmap.nYBmpOffset, sizeof(int)*MAX_SCAN_MODE);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nYBmpOffset[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            // HW SETUP의 SCAN MODE에 따른 FRAME BITMAP SIZE 값 변경 확인
            if (memcmp(m_IntegratedData.nWAverageNum, XYScanFrameBitmap.nWAverageNum, sizeof(int)*MAX_SCAN_MODE) != NULL)
            {
                // 설정값->Scan Mode 에 따른 Average Pixel 갯수 적용
                memcpy(m_IntegratedData.nWAverageNum, XYScanFrameBitmap.nWAverageNum, sizeof(int)*MAX_SCAN_MODE);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nWAverageNum[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            // HW SETUP의 SCAN MODE에 따른 FRAME BITMAP SIZE 값 변경 확인
            if (memcmp(m_IntegratedData.nFilterNum, XYScanFrameBitmap.nFilterNum, sizeof(int)*MAX_SCAN_MODE) != NULL)
            {
                // 설정값->Scan Mode 에 따른 Filter Pixel 갯수 적용
                memcpy(m_IntegratedData.nFilterNum, XYScanFrameBitmap.nFilterNum, sizeof(int)*MAX_SCAN_MODE);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nFilterNum[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:18032 ----
        // USB CONTROL DATA : HW SETUP의 WD에 따른 배율보정(MAG CALIBRATION) DATA
        public int OnSemHWSetupWDMagCalibrationData(int wParam, object lParam)
        {
            if (lParam == null) return 0;

            // HW SETUP의 WD에 따른 배율보정(MAG CALIBRATION) DATA 정보
            SEM_CONTROL_WDMAGCALIBRATION_DATA WDMagCalibration = new SEM_CONTROL_WDMAGCALIBRATION_DATA();
            // LONG DATA 복사
            WDMagCalibration.CopyFrom((SEM_CONTROL_WDMAGCALIBRATION_DATA)lParam);

            //  HW SETUP의 WD에 따른 배율보정(MAG CALIBRATION) DATA 변경 확인
            if (m_IntegratedData.bWDSetupFlag != WDMagCalibration.bWDSetupFlag)
            {
                // 사용값->WD 값에 따른 OL전압, Scan Rotation, 배율보정을 적용할 것인지 여부 적용
                m_IntegratedData.bWDSetupFlag = WDMagCalibration.bWDSetupFlag;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.bWDSetupFlag);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            //  HW SETUP의 WD에 따른 배율보정(MAG CALIBRATION) DATA 변경 확인
            if (m_IntegratedData.nWDStepNumber != WDMagCalibration.nWDStepNumber)
            {
                // 사용값->WD Step Number 적용
                m_IntegratedData.nWDStepNumber = WDMagCalibration.nWDStepNumber;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nWDStepNumber);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            //  HW SETUP의 WD에 따른 배율보정(MAG CALIBRATION) DATA 변경 확인
            if (m_IntegratedData.nWDStepMax != WDMagCalibration.nWDStepMax)
            {
                // 사용값->Max WD Step Number 적용
                m_IntegratedData.nWDStepMax = WDMagCalibration.nWDStepMax;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nWDStepMax);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            //  HW SETUP의 WD에 따른 배율보정(MAG CALIBRATION) DATA 변경 확인
            if (m_IntegratedData.nWDMaxValue != WDMagCalibration.nWDMaxValue)
            {
                // 사용값->WD의 최대값 적용
                m_IntegratedData.nWDMaxValue = WDMagCalibration.nWDMaxValue;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nWDMaxValue);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            //  HW SETUP의 WD에 따른 배율보정(MAG CALIBRATION) DATA 변경 확인
            if (m_IntegratedData.nOLWD != WDMagCalibration.nOLWD)
            {
                // 사용값->WD_OL_coarse 리스트 값 중에 Focus Coarse에 의한 값 적용
                m_IntegratedData.nOLWD = WDMagCalibration.nOLWD;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nOLWD);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            //  HW SETUP의 WD에 따른 배율보정(MAG CALIBRATION) DATA 변경 확인
            if (m_IntegratedData.nOLWDOld != WDMagCalibration.nOLWDOld)
            {
                // 사용값->WD_OL_coarse 리스트 값 중에 Focus Coarse에 의한 값 보관 적용
                m_IntegratedData.nOLWDOld = WDMagCalibration.nOLWDOld;
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nOLWDOld);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }

            // HW SETUP의 WD에 따른 배율보정(MAG CALIBRATION) 값 변경 확인
            if (memcmp(m_IntegratedData.nWDOLCoarse, WDMagCalibration.nWDOLCoarse, sizeof(int)*MAX_WD_COMPENSATION_COUNT) != NULL)
            {
                // 설정값->WD에 따른 배율보정 OL Coarse 보정값 적용
                memcpy(m_IntegratedData.nWDOLCoarse, WDMagCalibration.nWDOLCoarse, sizeof(int)*MAX_WD_COMPENSATION_COUNT);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nWDOLCoarse[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            // HW SETUP의 WD에 따른 배율보정(MAG CALIBRATION) 값 변경 확인
            if (memcmp(m_IntegratedData.nWDSRotation, WDMagCalibration.nWDSRotation, sizeof(int)*MAX_WD_COMPENSATION_COUNT) != NULL)
            {
                // 설정값->WD에 따른 배율보정 Scan Rotation 보정값 적용
                memcpy(m_IntegratedData.nWDSRotation, WDMagCalibration.nWDSRotation, sizeof(int)*MAX_WD_COMPENSATION_COUNT);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nWDSRotation[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            // HW SETUP의 WD에 따른 배율보정(MAG CALIBRATION) 값 변경 확인
            if (memcmp(m_IntegratedData.nWDZooming, WDMagCalibration.nWDZooming, sizeof(int)*MAX_WD_COMPENSATION_COUNT) != NULL)
            {
                // 설정값->WD에 따른 배율보정 Zooming 보정값 적용
                memcpy(m_IntegratedData.nWDZooming, WDMagCalibration.nWDZooming, sizeof(int)*MAX_WD_COMPENSATION_COUNT);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nWDZooming[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            // HW SETUP의 WD에 따른 배율보정(MAG CALIBRATION) 값 변경 확인
            if (memcmp(m_IntegratedData.nWDMinMag, WDMagCalibration.nWDMinMag, sizeof(int)*MAX_WD_COMPENSATION_COUNT) != NULL)
            {
                // 설정값->WD에 따른 배율보정 Min Mag 보정값 적용
                memcpy(m_IntegratedData.nWDMinMag, WDMagCalibration.nWDMinMag, sizeof(int)*MAX_WD_COMPENSATION_COUNT);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nWDMinMag[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            // HW SETUP의 WD에 따른 배율보정(MAG CALIBRATION) 값 변경 확인
            if (memcmp(m_IntegratedData.nWDOLCoarseLinear, WDMagCalibration.nWDOLCoarseLinear, sizeof(int)*MAX_WD_COMPENSATION_COUNT) != NULL)
            {
                // 계산값->WD에 따른 배율보정 OL Coarse 보정값 10등분 선형화 설정값 적용
                memcpy(m_IntegratedData.nWDOLCoarseLinear, WDMagCalibration.nWDOLCoarseLinear, sizeof(int)*MAX_WD_COMPENSATION_COUNT);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nWDOLCoarseLinear[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            // HW SETUP의 WD에 따른 배율보정(MAG CALIBRATION) 값 변경 확인
            if (memcmp(m_IntegratedData.nWDSRotationLinear, WDMagCalibration.nWDSRotationLinear, sizeof(int)*MAX_WD_COMPENSATION_COUNT) != NULL)
            {
                // 계산값->WD에 따른 배율보정 Scan Rotation 보정값 10등분 선형화 설정값 적용
                memcpy(m_IntegratedData.nWDSRotationLinear, WDMagCalibration.nWDSRotationLinear, sizeof(int)*MAX_WD_COMPENSATION_COUNT);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nWDSRotationLinear[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }
            // HW SETUP의 WD에 따른 배율보정(MAG CALIBRATION) 값 변경 확인
            if (memcmp(m_IntegratedData.nWDZoomingLinear, WDMagCalibration.nWDZoomingLinear, sizeof(int)*MAX_WD_COMPENSATION_COUNT) != NULL)
            {
                // 계산값->WD에 따른 배율보정 Zooming 보정값 10등분 선형화 설정값 적용
                memcpy(m_IntegratedData.nWDZoomingLinear, WDMagCalibration.nWDZoomingLinear, sizeof(int)*MAX_WD_COMPENSATION_COUNT);
                // 변경 및 UI 처리
                //CString strText = _T("");
                //strText.Format(_T("%d"), m_IntegratedData.nWDZoomingLinear[0]);
                //m_wndStageControlDlg.m_ToolTitleText.SetWindowTextW(strText);
            }

            return 1;
        }

    }
}
