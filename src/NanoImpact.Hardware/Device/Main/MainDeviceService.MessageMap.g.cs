// Generated from the original message map (NanoImpact2020cfeDlg.cpp BEGIN_MESSAGE_MAP) for the ported handlers. Do not edit.
#pragma warning disable IDE1006
using static NanoImpact.Core.Def;

namespace NanoImpact.Hardware.Device
{
    public sealed partial class MainDeviceService
    {
        /// <summary>원본 메시지 맵 중 포팅한 처리기 (DeviceHost.RegisterWindow(hWnd, WindowProc)). 처리기가 없으면 null.</summary>
        public int? Dispatch(int msg, int wParam, object lParam)
        {
            switch (msg)
            {
                case UM_USB_SBR_INTEGRATED_DATA: return OnSemControlIntegratedData(wParam, lParam);
                case UM_USB_SBR_STAGE_INIT_DATA: return OnSemStageUsbControlData(wParam, lParam);
                case UM_USB_SBR_SEMMODEL_DATA: return OnSemControlSemModelData(wParam, lParam);
                case UM_USB_SBR_SCANMODE_DATA: return OnSemControlScanModeData(wParam, lParam);
                case UM_USB_SBR_VACUUM_DATA: return OnSemControlVacuumData(wParam, lParam);
                case UM_USB_SBR_GUN_HV_DATA: return OnSemControlGunHVData(wParam, lParam);
                case UM_USB_SBR_FILAMENT_DATA: return OnSemControlFilamentData(wParam, lParam);
                case UM_USB_SBR_BIAS_DATA: return OnSemControlBiasData(wParam, lParam);
                case UM_USB_SBR_IMAGESHIFT_DATA: return OnSemControlImageShiftData(wParam, lParam);
                case UM_USB_SBR_GUNALIGN_DATA: return OnSemControlGunAlignData(wParam, lParam);
                case UM_USB_SBR_STIG_DATA: return OnSemControlStigData(wParam, lParam);
                case UM_USB_SBR_CONTBRIGHT_DATA: return OnSemControlContrastBrightData(wParam, lParam);
                case UM_USB_SBR_FOCUS_DATA: return OnSemControlFocusData(wParam, lParam);
                case UM_USB_SBR_MAGNIFICATION_DATA: return OnSemControlMagnificationData(wParam, lParam);
                case UM_USB_SBR_ZOOM_XY_EXECUTE_DATA: return OnSemControlZoomXYData(wParam, lParam);
                case UM_USB_SBR_ZOOM_XY_VALUE_DATA: return OnSemControlZoomXYValueData(wParam, lParam);
                case UM_USB_SBR_WDCURPOSITION_DATA: return OnSemControlWDCurPositionData(wParam, lParam);
                case UM_USB_SBR_XYSCANCALIBRATION_DATA: return OnSemHWSetupXYScanCalibrationData(wParam, lParam);
                case UM_USB_SBR_XYSCANSKIPOVER_DATA: return OnSemHWSetupXYScanSkipOverData(wParam, lParam);
                case UM_USB_SBR_XYSCANUPDOWNCLOCK_DATA: return OnSemHWSetupXYScanUpDownClockData(wParam, lParam);
                case UM_USB_SBR_XYSCANFRAMECLOCK_DATA: return OnSemHWSetupXYScanFrameClockData(wParam, lParam);
                case UM_USB_SBR_XYSCANFRAMEBMP_DATA: return OnSemHWSetupXYScanFrameBitmapData(wParam, lParam);
                case UM_USB_SBR_WDMAGCALIBRATION_DATA: return OnSemHWSetupWDMagCalibrationData(wParam, lParam);
                case UM_USB_SBR_SPOTCLCALIBRATION_DATA: return OnSemHWSetupSpotCLCalibrationData(wParam, lParam);
                case UM_USB_SBR_HWIMAGECALOPTION_DATA: return OnSemHWSetupImageCalOptionData(wParam, lParam);
                case UM_USB_SBR_STAGE_AXIS_POSITION: return OnSemControlStagePositionData(wParam, lParam);
                case UM_USB_SBR_STAGE_AXIS_MM_STEP: return OnSemControlStageMMStepData(wParam, lParam);
                case UM_USB_SBR_STAGE_MAX_MM_STEP: return OnSemControlStageMaxMMStepData(wParam, lParam);
                case UM_USB_SBR_STAGE_AXIS_ACCEL: return OnSemControlStageAccelData(wParam, lParam);
                case UM_USB_SBR_STAGE_FIX_POSITION: return OnSemControlStageFixPositionData(wParam, lParam);
                case UM_USB_SBR_STAGE_LIMIT_STATUS: return OnSemControlStageLimitStatusData(wParam, lParam);
                case UM_USB_SBR_STAGE_BMP_POSITION: return OnSemControlStageBmpPositionData(wParam, lParam);
                case UM_USB_SBR_FOCUS_COARSE_DATA_EXECUTE: return OnSemControlFocusCoarseDataExecute(wParam, lParam);
                case UM_HW_SETUP_SROT_CALI_DATA: return OnSemControlSRotOrthoZoomCaliData(wParam, lParam);
                case UM_GET_USB_DEVICE_STAGE_MMSTEP_AXIS: return OnGetUsbDeviceStageMMSTEPAxis(wParam, lParam);
                case UM_SET_USB_DEVICE_STAGE_MMSTEP_AXIS: return OnSetUsbDeviceStageMMSTEPAxis(wParam, lParam);
                case UM_GET_USB_DEVICE_STAGE_POS_AXIS: return OnGetUsbDeviceStagePositionAxis(wParam, lParam);
                case UM_GET_USB_DEVICE_STAGE_WIDTH_AXIS: return OnGetUsbDeviceStageWidthAxis(wParam, lParam);
                case UM_SET_USB_DEVICE_STAGE_WIDTH_AXIS: return OnSetUsbDeviceStageWidthAxis(wParam, lParam);
                case UM_GET_USB_DEVICE_STAGE_ACCEL_AXIS: return OnGetUsbDeviceStageAccelAxis(wParam, lParam);
                case UM_SET_USB_DEVICE_STAGE_ACCEL_AXIS: return OnSetUsbDeviceStageAccelAxis(wParam, lParam);
                case UM_GET_USB_DEVICE_STAGE_CENTER_AXIS: return OnGetUsbDeviceStageCenterAxis(wParam, lParam);
                case UM_SET_USB_DEVICE_STAGE_CENTER_AXIS: return OnSetUsbDeviceStageCenterAxis(wParam, lParam);
                case UM_SET_USB_DEVICE_STAGE_HOME_AXIS: return OnSetUsbDeviceStageHomeAxis(wParam, lParam);
                case UM_SET_USB_DEVICE_STAGE_MOVE_AXIS: return OnSetUsbDeviceStageMoveAxis(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_STAGE_MOVE_VALUE: return OnSetUsbDeviceStageControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_STAGE_XY_CENTER_MOVE_EXECUTE: return OnSetUsbDeviceStageControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_STAGE_OPTION_DATA: return OnSetUsbDeviceStageOptionData(wParam, lParam);
                case UM_SET_USB_DEVICE_HWSETUP_XY_AMPOFF: return OnSetUsbDeviceHWSetupXYAmpOffset(wParam, lParam);
                case UM_SET_USB_DEVICE_HWSETUP_SPOTSIZE_CL: return OnSetUsbDeviceHWSetupSpotSizeCL(wParam, lParam);
                case UM_SET_USB_DEVICE_HWSETUP_CLOCK_CALI: return OnSetUsbDeviceHWSetupClockCalibration(wParam, lParam);
                case UM_SET_USB_DEVICE_HWSETUP_XY_AMP_OFFSET_DATA: return OnSetUsbDeviceHWSetupXYAmpOffsetCalibration(wParam, lParam);
                case UM_SET_USB_DEVICE_HWSETUP_WD_SETUP_DATA: return OnSetUsbDeviceHWSetupWDSetupDataCalibration(wParam, lParam);
                case UM_SET_USB_DEVICE_HWSETUP_CL_SETUP_DATA: return OnSetUsbDeviceHWSetupCLSetupDataCalibration(wParam, lParam);
                case UM_SET_USB_DEVICE_HWSETUP_SCAN_AVERAGE: return OnSetUsbDeviceHWSetupScanAverage(wParam, lParam);
                case UM_SET_USB_DEVICE_HWSETUP_MONITOR_INFO: return OnSetUsbDeviceHWSetupMonitorInformation(wParam, lParam);
                case UM_SET_USB_DEVICE_HWSETUP_AUTO_FOCUS_VALUE: return OnSetUsbDeviceHWSetupAutoFocusControlValue(wParam, lParam);
                case UM_SET_USB_DEVICE_HWSETUP_AUTO_CONTRAST_BRIGHT_VAL: return OnSetUsbDeviceHWSetupAutoContrastBrightValue(wParam, lParam);
                case UM_SET_USB_DEVICE_HWSETUP_AUTO_START_CONFIG_VAL: return OnSetUsbDeviceHWSetupAutoStartConfigValue(wParam, lParam);
                case UM_SET_USB_DEVICE_HWSETUP_ORTHOGONAL_VALUE: return OnSetUsbDeviceHWSetupOrthogonal(wParam, lParam);
                case UM_SET_USB_DEVICE_HWSETUP_OPTION_DATA: return OnSetUsbDeviceHWSetupOptionData(wParam, lParam);
                case UM_SET_USB_DEVICE_HWSETUP_SEM_MODEL: return OnSetUsbDeviceHWSetupSemDeviceModel(wParam, lParam);
                case UM_SET_USB_DEVICE_HWSETUP_SEM_COMPANY: return OnSetUsbDeviceHWSetupSemDeviceCompany(wParam, lParam);
                case UM_SET_USB_DEVICE_OPTION_USER_NAME: return OnSetUsbDeviceOptionUserName(wParam, lParam);
                case UM_SET_USB_DEVICE_OPTION_USER_PASSWORD: return OnSetUsbDeviceOptionUserPassword(wParam, lParam);
                case UM_SET_USB_DEVICE_OPTION_USER_COMPANY: return OnSetUsbDeviceOptionUserCompany(wParam, lParam);
                case UM_SET_USB_DEVICE_OPTION_SERVER_IPADDRESS: return OnSetUsbDeviceOptionServerIPAddress(wParam, lParam);
                case UM_SET_USB_DEVICE_OPTION_SERVER_PORT: return OnSetUsbDeviceOptionServerPort(wParam, lParam);
                case UM_SET_USB_DEVICE_OPTION_SERVER_CONNECT_CODE: return OnSetUsbDeviceOptionServerConnectCode(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_GUN_HV_INDEX: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_SPOT_SIZE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_MAGNIFICATION_INDEX: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_WOBBLE_INDEX: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_FILAMENT_VALUE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_BIAS_VALUE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_GUN_ALIGN_X_VALUE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_GUN_ALIGN_Y_VALUE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_BEAM_ALIGN_X_VALUE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_BEAM_ALIGN_Y_VALUE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_IMAGE_SHIFT_X_VALUE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_IMAGE_SHIFT_Y_VALUE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_STIG_X_VALUE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_STIG_Y_VALUE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_CONTRAST_VALUE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_BRIGHT_VALUE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_FOCUS_COARSE_FINE_VALUE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_SCAN_ROTATION_VALUE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_EMISSION_AUTO_TRANSFER_VALUE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_PMT_RELAY_ON_OFF_VALUE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_DETECTOR_ON_OFF_VALUE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_NORMAL_SCREEN_MODE_VALUE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_IMAGE_CAPTURE_ON_OFF_VALUE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_GUN_HV_VOLT_VALUE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_FILAMENT_VOLT_VALUE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_BIAS_VOLT_VALUE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_GUN_HV_EC_VALUE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_SCAN_GENERATION_VALUE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_CONTROL_VACUUM_STATE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_CONTROL_GUN_VALVE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_CONTROL_CHAMBER_STATE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_CONTROL_SPELLMAN_HV: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_CONTROL_CL1CL2_VALUE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_CONTROL_CL1CL2_EXECUTE_VALUE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_CONTROL_GUN_ALIGN_VALUE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_CONTROL_BEAM_ALIGN_VALUE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_CONTROL_IMAGE_SHIFT_VALUE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_CONTROL_STIG_VALUE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_CONTROL_MAGNIFICATION_INDEX: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_CONTROL_FOCUS_OL_VALUE: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_CONTROL_FILAMENT_IMAGE_SCAN: return OnSetUsbDeviceSEMControlDataToDeviceServer(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_CONTROL_FLOW_EXECUTE: return OnSetUsbDeviceSEMControlDataToFlowControl(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_CONTROL_SHUTDOWN_EXECUTE: return OnSetUsbDeviceSEMControlDataToFlowControl(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_CONTROL_SCAN_IMAGE_UPDATE: return OnSemSEScainImageDataUpdateControl(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_CONTROL_SCAN_MODE_UPDATE: return OnSemSEScainModeDataUpdateControl(wParam, lParam);
                case UM_REQUEST_GET_USB_DATA: return OnGetUsbDeviceDataProcesss(wParam, lParam);
                case UM_REQUEST_GET_USB_DATA_PART1: return OnGetUsbDeviceDataPart1Processs(wParam, lParam);
                case UM_SET_USB_DEVICE_SEM_DATA_UPDATE: return OnSetSemControlDataUpdate(wParam, lParam);
                case UM_SET_USB_DEVICE_SE_SCAN_CONTROL_DATA_UPDATE: return OnSetSemSEScanControlDataUpdate(wParam, lParam);
                default: return null;
            }
        }

        /// <summary>DeviceHost 창 처리기 (처리기 없는 메시지는 0)</summary>
        public int WindowProc(int msg, int wParam, object lParam) => Dispatch(msg, wParam, lParam) ?? 0;
    }
}
