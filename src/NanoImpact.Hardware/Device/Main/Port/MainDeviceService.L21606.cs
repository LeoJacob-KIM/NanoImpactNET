// 원본 NanoImpact2020cfeDlg.cpp:21606-22308 을 NanoImpact.CodeGen translate로 옮긴 뒤 손으로 고친 코드.
#pragma warning disable IDE1006, IDE0059, CS0162, CS0168, CS0219
using System;
using NanoImpact.Core.Native;
using static NanoImpact.Core.Def;
using static NanoImpact.Core.Native.CRuntime;

namespace NanoImpact.Hardware.Device
{
    public sealed partial class MainDeviceService
    {
        // ---- NanoImpact2020cfeDlg.cpp:21606 ----
        // DEVICE --> CLIENT : RECEIVE USB DEVICE DATA
        public int OnGetUsbDeviceDataProcesss(int wParam, object lParam)
        {
            // After displaying the dialog screen & Process function
            //if (m_pSocketProcess == NULL) return 0;
            //if (m_nDeviceSocketIndex < 0) return 0;
            //if (lParam == null) return 0;

            // 2020.04.28
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 전송자료복사
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // Stage Axis step/mm, step/deg Read Data Set & Display
            if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_MMSTEP_X_AXIS)
            {
                // STAGE X-AXIS step/mm
                m_IntegratedData.dAxisMMPerPulse[IDX_X_AXIS]   = RequestSetUsbDeviceData.dRequestData[0];
                // STAGE AXIS unit[mm,deg]/step : 축(Axis)별 Unit 당 Pulse 갯수
                m_IntegratedData.dAxisPulsePerUnit[IDX_X_AXIS] = RequestSetUsbDeviceData.dRequestData[1];
                // SET STAGE CALIBRATION DATA : step/mm, step/deg
                m_wndStageCalibrationToolDlg.IntegratedSetAxisMMStepReadValue(IDX_X_AXIS, m_IntegratedData.dAxisMMPerPulse[IDX_X_AXIS], m_IntegratedData.dAxisPulsePerUnit[IDX_X_AXIS]);
            }
            // Stage Axis step/mm, step/deg Read Data Set & Display
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_MMSTEP_Y_AXIS)
            {
                // STAGE Y-AXIS step/mm
                m_IntegratedData.dAxisMMPerPulse[IDX_Y_AXIS]   = RequestSetUsbDeviceData.dRequestData[0];
                // STAGE AXIS unit[mm,deg]/step : 축(Axis)별 Unit 당 Pulse 갯수
                m_IntegratedData.dAxisPulsePerUnit[IDX_Y_AXIS] = RequestSetUsbDeviceData.dRequestData[1];
                // SET STAGE CALIBRATION DATA : step/mm, step/deg
                m_wndStageCalibrationToolDlg.IntegratedSetAxisMMStepReadValue(IDX_Y_AXIS, m_IntegratedData.dAxisMMPerPulse[IDX_Y_AXIS], m_IntegratedData.dAxisPulsePerUnit[IDX_Y_AXIS]);
            }
            // Stage Axis step/mm, step/deg Read Data Set & Display
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_MMSTEP_Z_AXIS)
            {
                // STAGE Z-AXIS step/mm
                m_IntegratedData.dAxisMMPerPulse[IDX_Z_AXIS]   = RequestSetUsbDeviceData.dRequestData[0];
                // STAGE AXIS unit[mm,deg]/step : 축(Axis)별 Unit 당 Pulse 갯수
                m_IntegratedData.dAxisPulsePerUnit[IDX_Z_AXIS] = RequestSetUsbDeviceData.dRequestData[1];
                // SET STAGE CALIBRATION DATA : step/mm, step/deg
                m_wndStageCalibrationToolDlg.IntegratedSetAxisMMStepReadValue(IDX_Z_AXIS, m_IntegratedData.dAxisMMPerPulse[IDX_Z_AXIS], m_IntegratedData.dAxisPulsePerUnit[IDX_Z_AXIS]);
            }
            // Stage Axis step/mm, step/deg Read Data Set & Display
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_DEGSTEP_R_AXIS)
            {
                // STAGE R-AXIS step/deg
                m_IntegratedData.dAxisMMPerPulse[IDX_R_AXIS]   = RequestSetUsbDeviceData.dRequestData[0];
                // STAGE AXIS unit[mm,deg]/step : 축(Axis)별 Unit 당 Pulse 갯수
                m_IntegratedData.dAxisPulsePerUnit[IDX_R_AXIS] = RequestSetUsbDeviceData.dRequestData[1];
                // SET STAGE CALIBRATION DATA : step/mm, step/deg
                m_wndStageCalibrationToolDlg.IntegratedSetAxisMMStepReadValue(IDX_R_AXIS, m_IntegratedData.dAxisMMPerPulse[IDX_R_AXIS], m_IntegratedData.dAxisPulsePerUnit[IDX_R_AXIS]);
            }
            // Stage Axis step/mm, step/deg Read Data Set & Display
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_DEGSTEP_T_AXIS)
            {
                // STAGE T-AXIS step/deg
                m_IntegratedData.dAxisMMPerPulse[IDX_T_AXIS]   = RequestSetUsbDeviceData.dRequestData[0];
                // STAGE AXIS unit[mm,deg]/step : 축(Axis)별 Unit 당 Pulse 갯수
                m_IntegratedData.dAxisPulsePerUnit[IDX_T_AXIS] = RequestSetUsbDeviceData.dRequestData[1];
                // SET STAGE CALIBRATION DATA : step/mm, step/deg
                m_wndStageCalibrationToolDlg.IntegratedSetAxisMMStepReadValue(IDX_T_AXIS, m_IntegratedData.dAxisMMPerPulse[IDX_T_AXIS], m_IntegratedData.dAxisPulsePerUnit[IDX_T_AXIS]);
            }
            // Stage Axis step/mm, step/deg Read Data Set & Display
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_MMSTEP_B_AXIS)
            {

                // STAGE BSE-AXIS step/mm
                m_IntegratedData.dAxisMMPerPulse[IDX_B_AXIS]   = RequestSetUsbDeviceData.dRequestData[0];
                // STAGE AXIS unit[mm,deg]/step : 축(Axis)별 Unit 당 Pulse 갯수
                m_IntegratedData.dAxisPulsePerUnit[IDX_B_AXIS] = RequestSetUsbDeviceData.dRequestData[1];
                // SET STAGE CALIBRATION DATA : step/mm, step/deg
                m_wndStageCalibrationToolDlg.IntegratedSetAxisMMStepReadValue(IDX_B_AXIS, m_IntegratedData.dAxisMMPerPulse[IDX_B_AXIS], m_IntegratedData.dAxisPulsePerUnit[IDX_B_AXIS]);
            }

            // Stage Axis position Read Data Set & Display
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_POS_X_AXIS)
            {
                // STAGE X-AXIS position [step]
                m_IntegratedData.nAxisPosition[IDX_X_AXIS] = RequestSetUsbDeviceData.nRequestData[0];
                // STAGE AXIS position [mm],[deg]
                m_IntegratedData.dCurAxisPos[IDX_X_AXIS]   = RequestSetUsbDeviceData.dRequestData[0];
                // STAGE X-AXIS position
                m_IntegratedData.dCurXPos = RequestSetUsbDeviceData.dRequestData[0];
                // SET STAGE POSITION DATA : position [mm],[deg]
                m_wndStageControlDlg.SetStageAxisCurPositionMMDeg(IDX_X_AXIS, m_IntegratedData.dCurAxisPos[IDX_X_AXIS]);
                // SET STAGE CALIBRATION DATA : position [mm],[deg]
                m_wndStageCalibrationToolDlg.IntegratedSetAxisPosStepValue(IDX_X_AXIS, m_IntegratedData.nAxisPosition[IDX_X_AXIS], m_IntegratedData.dCurAxisPos[IDX_X_AXIS]);
            }
            // Stage Axis position Read Data Set & Display
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_POS_Y_AXIS)
            {
                // STAGE Y-AXIS position [step]
                m_IntegratedData.nAxisPosition[IDX_Y_AXIS] = RequestSetUsbDeviceData.nRequestData[0];
                // STAGE AXIS position [mm],[deg]
                m_IntegratedData.dCurAxisPos[IDX_Y_AXIS]   = RequestSetUsbDeviceData.dRequestData[0];
                // STAGE Y-AXIS position
                m_IntegratedData.dCurYPos = RequestSetUsbDeviceData.dRequestData[0];
                // SET STAGE POSITION DATA : position [mm],[deg]
                m_wndStageControlDlg.SetStageAxisCurPositionMMDeg(IDX_Y_AXIS, m_IntegratedData.dCurAxisPos[IDX_Y_AXIS]);
                // SET STAGE CALIBRATION DATA : position [mm],[deg]
                m_wndStageCalibrationToolDlg.IntegratedSetAxisPosStepValue(IDX_Y_AXIS, m_IntegratedData.nAxisPosition[IDX_Y_AXIS], m_IntegratedData.dCurAxisPos[IDX_Y_AXIS]);
            }
            // Stage Axis position Read Data Set & Display
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_POS_Z_AXIS)
            {
                // STAGE Z-AXIS position [step]
                m_IntegratedData.nAxisPosition[IDX_Z_AXIS] = RequestSetUsbDeviceData.nRequestData[0];
                // STAGE AXIS position [mm],[deg]
                m_IntegratedData.dCurAxisPos[IDX_Z_AXIS]   = RequestSetUsbDeviceData.dRequestData[0];
                // STAGE Z-AXIS position
                m_IntegratedData.dCurZPos = RequestSetUsbDeviceData.dRequestData[0];
                // SET STAGE POSITION DATA : position [mm],[deg]
                m_wndStageControlDlg.SetStageAxisCurPositionMMDeg(IDX_Z_AXIS, m_IntegratedData.dCurAxisPos[IDX_Z_AXIS]);
                // SET STAGE CALIBRATION DATA : position [mm],[deg]
                m_wndStageCalibrationToolDlg.IntegratedSetAxisPosStepValue(IDX_Z_AXIS, m_IntegratedData.nAxisPosition[IDX_Z_AXIS], m_IntegratedData.dCurAxisPos[IDX_Z_AXIS]);
                // 2021.06.16
                if (m_dev.AfxGetIntegratedPointer() != null)
                {
                    // 2021.06.16
                    // Z-AXIS MOVING or STOP 상태 확인
                    // Z축 이동에 따른 FOCUS 변경은 STOP 상태에서만 실행한다
                    if (m_dev.AfxGetIntegratedPointer().nBmpMovingFlag[IDX_Z_AXIS] == 0)
                    {
                        // 2020.06.01
                        // FOCUS COARSE 재설정
                        m_wndFocusControlDlg.SetFocusCoarseFineValue(m_dev.AfxGetSEMFocusCoarse(), SEM_FOCUS_FINE_STR);
                        m_wndFocusControlDlg.SetCurrentFocusValueToUsbDevice(); // 실행
                    }
                }
            }
            // Stage Axis position Read Data Set & Display
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_POS_R_AXIS)
            {
                // STAGE R-AXIS position [step]
                m_IntegratedData.nAxisPosition[IDX_R_AXIS] = RequestSetUsbDeviceData.nRequestData[0];
                // STAGE AXIS position [mm],[deg]
                m_IntegratedData.dCurAxisPos[IDX_R_AXIS]   = RequestSetUsbDeviceData.dRequestData[0];
                // STAGE R-AXIS position
                m_IntegratedData.dCurRPos = RequestSetUsbDeviceData.dRequestData[0];
                // SET STAGE POSITION DATA : position [mm],[deg]
                m_wndStageControlDlg.SetStageAxisCurPositionMMDeg(IDX_R_AXIS, m_IntegratedData.dCurAxisPos[IDX_R_AXIS]);
                // SET STAGE CALIBRATION DATA : position [mm],[deg]
                m_wndStageCalibrationToolDlg.IntegratedSetAxisPosStepValue(IDX_R_AXIS, m_IntegratedData.nAxisPosition[IDX_R_AXIS], m_IntegratedData.dCurAxisPos[IDX_R_AXIS]);
            }
            // Stage Axis position Read Data Set & Display
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_POS_T_AXIS)
            {
                // STAGE T-AXIS position [step]
                m_IntegratedData.nAxisPosition[IDX_T_AXIS] = RequestSetUsbDeviceData.nRequestData[0];
                // STAGE AXIS position [mm],[deg]
                m_IntegratedData.dCurAxisPos[IDX_T_AXIS]   = RequestSetUsbDeviceData.dRequestData[0];
                // STAGE T-AXIS position
                m_IntegratedData.dCurTPos = RequestSetUsbDeviceData.dRequestData[0];
                // SET STAGE POSITION DATA : position [mm],[deg]
                m_wndStageControlDlg.SetStageAxisCurPositionMMDeg(IDX_T_AXIS, m_IntegratedData.dCurAxisPos[IDX_T_AXIS]);
                // SET STAGE CALIBRATION DATA : position [mm],[deg]
                m_wndStageCalibrationToolDlg.IntegratedSetAxisPosStepValue(IDX_T_AXIS, m_IntegratedData.nAxisPosition[IDX_T_AXIS], m_IntegratedData.dCurAxisPos[IDX_T_AXIS]);
            }
            // Stage Axis position Read Data Set & Display
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_POS_B_AXIS)
            {
                // STAGE BSE-AXIS position [step]
                m_IntegratedData.nAxisPosition[IDX_B_AXIS] = RequestSetUsbDeviceData.nRequestData[0];
                // STAGE AXIS position [mm],[deg]
                m_IntegratedData.dCurAxisPos[IDX_B_AXIS]   = RequestSetUsbDeviceData.dRequestData[0];
                // STAGE BSE-AXIS position ( 3축 일 경우 T-AXIS를 활용한다 )
                m_IntegratedData.dCurTPos = RequestSetUsbDeviceData.dRequestData[0];
                // SET STAGE POSITION DATA : position [mm],[deg]
                m_wndStageControlDlg.SetStageAxisCurPositionMMDeg(IDX_B_AXIS, m_IntegratedData.dCurAxisPos[IDX_B_AXIS]);
                // SET STAGE CALIBRATION DATA : position [mm],[deg]
                m_wndStageCalibrationToolDlg.IntegratedSetAxisPosStepValue(IDX_B_AXIS, m_IntegratedData.nAxisPosition[IDX_B_AXIS], m_IntegratedData.dCurAxisPos[IDX_B_AXIS]);
            }

            // Stage Axis width Read Data Set & Display
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_WIDTH_X_AXIS)
            {
                int nAxis = IDX_X_AXIS;
                // STAGE X-AXIS width
                m_IntegratedData.nAxisMaxWidthPulse[nAxis] = RequestSetUsbDeviceData.nRequestData[0];
                // STAGE AXIS width [mm],[deg]
                m_IntegratedData.dAxisMaxWidthMM[nAxis]    = RequestSetUsbDeviceData.dRequestData[0];
                // SET STAGE CALIBRATION DATA : width [mm],[deg]
                m_wndStageCalibrationToolDlg.IntegratedSetAxisWidthReadValue(nAxis, m_IntegratedData.dAxisMaxWidthMM[nAxis]);
            }
            // Stage Axis width Read Data Set & Display
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_WIDTH_Y_AXIS)
            {
                int nAxis = IDX_Y_AXIS;
                // STAGE Y-AXIS width
                m_IntegratedData.nAxisMaxWidthPulse[nAxis] = RequestSetUsbDeviceData.nRequestData[0];
                // STAGE AXIS width [mm],[deg]
                m_IntegratedData.dAxisMaxWidthMM[nAxis]    = RequestSetUsbDeviceData.dRequestData[0];
                // SET STAGE CALIBRATION DATA : width [mm],[deg]
                m_wndStageCalibrationToolDlg.IntegratedSetAxisWidthReadValue(nAxis, m_IntegratedData.dAxisMaxWidthMM[nAxis]);
            }
            // Stage Axis width Read Data Set & Display
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_WIDTH_Z_AXIS)
            {
                int nAxis = IDX_Z_AXIS;
                // STAGE Z-AXIS width
                m_IntegratedData.nAxisMaxWidthPulse[nAxis] = RequestSetUsbDeviceData.nRequestData[0];
                // STAGE AXIS width [mm],[deg]
                m_IntegratedData.dAxisMaxWidthMM[nAxis]    = RequestSetUsbDeviceData.dRequestData[0];
                // SET STAGE CALIBRATION DATA : width [mm],[deg]
                m_wndStageCalibrationToolDlg.IntegratedSetAxisWidthReadValue(nAxis, m_IntegratedData.dAxisMaxWidthMM[nAxis]);
            }
            // Stage Axis width Read Data Set & Display
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_LIMIT_Z_AXIS)
            {
                int nAxis = IDX_Z_AXIS + 1;
                // STAGE Z-AXIS limit
                m_IntegratedData.nZTopLimitSwPulse = RequestSetUsbDeviceData.nRequestData[0];
                // STAGE AXIS width [mm],[deg]
                m_IntegratedData.dZTopLimitPosMM   = RequestSetUsbDeviceData.dRequestData[0];
                // SET STAGE CALIBRATION DATA : width [mm],[deg]
                m_wndStageCalibrationToolDlg.IntegratedSetAxisWidthReadValue(nAxis, m_IntegratedData.dZTopLimitPosMM);
            }
            // Stage Axis width Read Data Set & Display
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_WIDTH_T_AXIS)
            {
                int nAxis = IDX_T_AXIS;
                // STAGE T-AXIS width
                m_IntegratedData.nAxisMaxWidthPulse[nAxis] = RequestSetUsbDeviceData.nRequestData[0];
                // STAGE AXIS width [mm],[deg]
                m_IntegratedData.dAxisMaxWidthMM[nAxis]    = RequestSetUsbDeviceData.dRequestData[0];
                // SET STAGE CALIBRATION DATA : width [mm],[deg]
                m_wndStageCalibrationToolDlg.IntegratedSetAxisWidthReadValue(nAxis, m_IntegratedData.dAxisMaxWidthMM[nAxis]);
            }
            // Stage Axis width Read Data Set & Display
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_ZERO_T_AXIS)
            {
                int nAxis = IDX_T_AXIS + 1;
                // STAGE T-AXIS zero pos
                m_IntegratedData.nTiltZeroAnglePulsePos = RequestSetUsbDeviceData.nRequestData[0];
                // STAGE AXIS width [mm],[deg]
                m_IntegratedData.dTiltZeroAnglePosMM    = RequestSetUsbDeviceData.dRequestData[0];
                // SET STAGE CALIBRATION DATA : width [mm],[deg]
                m_wndStageCalibrationToolDlg.IntegratedSetAxisWidthReadValue(nAxis, m_IntegratedData.dTiltZeroAnglePosMM);
            }

            // Stage Axis accelation Read Data Set & Display
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_ACCEL_X_AXIS)
            {
                // STAGE X-AXIS accelation
                int nAxis = IDX_X_AXIS;
                m_IntegratedData.nAxisAccCalVal[nAxis] = RequestSetUsbDeviceData.nRequestData[0];
                // SET STAGE CALIBRATION DATA : accelation
                m_wndStageCalibrationToolDlg.IntegratedSetAxisAccelReadValue(nAxis, m_IntegratedData.nAxisAccCalVal[nAxis]);
            }
            // Stage Axis accelation Read Data Set & Display
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_ACCEL_Y_AXIS)
            {
                // STAGE Y-AXIS accelation
                int nAxis = IDX_Y_AXIS;
                m_IntegratedData.nAxisAccCalVal[nAxis] = RequestSetUsbDeviceData.nRequestData[0];
                // SET STAGE CALIBRATION DATA : accelation
                m_wndStageCalibrationToolDlg.IntegratedSetAxisAccelReadValue(nAxis, m_IntegratedData.nAxisAccCalVal[nAxis]);
            }
            // Stage Axis accelation Read Data Set & Display
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_ACCEL_Z_AXIS)
            {
                // STAGE Z-AXIS accelation
                int nAxis = IDX_Z_AXIS;
                m_IntegratedData.nAxisAccCalVal[nAxis] = RequestSetUsbDeviceData.nRequestData[0];
                // SET STAGE CALIBRATION DATA : accelation
                m_wndStageCalibrationToolDlg.IntegratedSetAxisAccelReadValue(nAxis, m_IntegratedData.nAxisAccCalVal[nAxis]);
            }
            // Stage Axis accelation Read Data Set & Display
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_ACCEL_R_AXIS)
            {
                // STAGE R-AXIS accelation
                int nAxis = IDX_R_AXIS;
                m_IntegratedData.nAxisAccCalVal[nAxis] = RequestSetUsbDeviceData.nRequestData[0];
                // SET STAGE CALIBRATION DATA : accelation
                m_wndStageCalibrationToolDlg.IntegratedSetAxisAccelReadValue(nAxis, m_IntegratedData.nAxisAccCalVal[nAxis]);
            }
            // Stage Axis accelation Read Data Set & Display
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_ACCEL_T_AXIS)
            {
                // STAGE T-AXIS accelation
                int nAxis = IDX_T_AXIS;
                m_IntegratedData.nAxisAccCalVal[nAxis] = RequestSetUsbDeviceData.nRequestData[0];
                // SET STAGE CALIBRATION DATA : accelation
                m_wndStageCalibrationToolDlg.IntegratedSetAxisAccelReadValue(nAxis, m_IntegratedData.nAxisAccCalVal[nAxis]);
            }
            // Stage Axis accelation Read Data Set & Display
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_ACCEL_B_AXIS)
            {
                // STAGE BSE-AXIS accelation
                int nAxis = IDX_B_AXIS;
                m_IntegratedData.nAxisAccCalVal[nAxis] = RequestSetUsbDeviceData.nRequestData[0];
                // SET STAGE CALIBRATION DATA : accelation
                m_wndStageCalibrationToolDlg.IntegratedSetAxisAccelReadValue(nAxis, m_IntegratedData.nAxisAccCalVal[nAxis]);
            }
            // Stage Axis limit state Read Data Set & Display
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_LIMIT_AXIS)
            {
                // STAGE AXIS limit state
                m_IntegratedData.nAxisLimitSWStatus[IDX_X_AXIS] = RequestSetUsbDeviceData.nRequestData[0]; // X-AXIS limit state
                m_IntegratedData.nAxisLimitSWStatus[IDX_Y_AXIS] = RequestSetUsbDeviceData.nRequestData[1]; // Y-AXIS limit state
                m_IntegratedData.nAxisLimitSWStatus[IDX_Z_AXIS] = RequestSetUsbDeviceData.nRequestData[2]; // Z-AXIS limit state
                m_IntegratedData.nAxisLimitSWStatus[IDX_R_AXIS] = RequestSetUsbDeviceData.nRequestData[3]; // R-AXIS limit state
                m_IntegratedData.nAxisLimitSWStatus[IDX_T_AXIS] = RequestSetUsbDeviceData.nRequestData[4]; // T-AXIS limit state
                m_IntegratedData.nAxisLimitSWStatus[IDX_B_AXIS] = RequestSetUsbDeviceData.nRequestData[5]; // BSE-AXIS limit state
                // SET STAGE AXIS DATA : limit state
                m_wndStageCalibrationToolDlg.IntegratedSetAxisLimitStateValue(IDX_X_AXIS, m_IntegratedData.nAxisLimitSWStatus[IDX_X_AXIS]);
                m_wndStageCalibrationToolDlg.IntegratedSetAxisLimitStateValue(IDX_Y_AXIS, m_IntegratedData.nAxisLimitSWStatus[IDX_Y_AXIS]);
                m_wndStageCalibrationToolDlg.IntegratedSetAxisLimitStateValue(IDX_Z_AXIS, m_IntegratedData.nAxisLimitSWStatus[IDX_Z_AXIS]);
                m_wndStageCalibrationToolDlg.IntegratedSetAxisLimitStateValue(IDX_R_AXIS, m_IntegratedData.nAxisLimitSWStatus[IDX_R_AXIS]);
                m_wndStageCalibrationToolDlg.IntegratedSetAxisLimitStateValue(IDX_T_AXIS, m_IntegratedData.nAxisLimitSWStatus[IDX_T_AXIS]);
                m_wndStageCalibrationToolDlg.IntegratedSetAxisLimitStateValue(IDX_B_AXIS, m_IntegratedData.nAxisLimitSWStatus[IDX_B_AXIS]);
            }

            // Stage Axis center Read Data Set & Display
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_CENTER_X_AXIS)
            {
                // STAGE XY-AXIS center
                int nAxis = IDX_X_AXIS;
                // STAGE XY-AXIS center : step
                m_IntegratedData.nBeamCenterXPos = RequestSetUsbDeviceData.nRequestData[0];
                m_IntegratedData.nBeamCenterYPos = RequestSetUsbDeviceData.nRequestData[1];
                // STAGE XY-AXIS center : [mm]
                m_IntegratedData.dBeamCenterXPos = RequestSetUsbDeviceData.dRequestData[0];
                m_IntegratedData.dBeamCenterYPos = RequestSetUsbDeviceData.dRequestData[1];
                // SET STAGE CALIBRATION DATA : center
                m_wndStageCalibrationToolDlg.IntegratedSetAxisXYCenterReadValue(m_IntegratedData.dBeamCenterXPos, m_IntegratedData.dBeamCenterYPos);
            }
            // Stage Axis center Read Data Set & Display
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_STAGE_CENTER_Y_AXIS)
            {
                // STAGE Y-AXIS center
                int nAxis = IDX_Y_AXIS;
                // STAGE XY-AXIS center : step
                m_IntegratedData.nBeamCenterXPos = RequestSetUsbDeviceData.nRequestData[0];
                m_IntegratedData.nBeamCenterYPos = RequestSetUsbDeviceData.nRequestData[1];
                // STAGE XY-AXIS center : [mm]
                m_IntegratedData.dBeamCenterXPos = RequestSetUsbDeviceData.dRequestData[0];
                m_IntegratedData.dBeamCenterYPos = RequestSetUsbDeviceData.dRequestData[1];
                // SET STAGE CALIBRATION DATA : center
                m_wndStageCalibrationToolDlg.IntegratedSetAxisXYCenterReadValue(m_IntegratedData.dBeamCenterXPos, m_IntegratedData.dBeamCenterYPos);
            }

            // WD에 따른 CALIBRATION DATA (OL, SROT, ZOOM, MINMAG, MAX) Set & Display
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_HWSETUP_WD_DATA_ALL)
            {
                // WD : Z-AXIS, OL, SROT, ZOOM, MINMAG, MAX
                int nZAxis = RequestSetUsbDeviceData.nRequestOption;
                m_IntegratedData.nWDPosition = nZAxis;                                           // Z-AXIS
                m_IntegratedData.nWDOLCoarse[nZAxis]  = RequestSetUsbDeviceData.nRequestData[0]; // OL
                m_IntegratedData.nWDSRotation[nZAxis] = RequestSetUsbDeviceData.nRequestData[1]; // SROT
                m_IntegratedData.nWDZooming[nZAxis]   = RequestSetUsbDeviceData.nRequestData[2]; // ZOOM
                m_IntegratedData.nWDMinMag[nZAxis]    = RequestSetUsbDeviceData.nRequestData[3]; // MIN-MAG
                m_IntegratedData.nWDMaxValue          = RequestSetUsbDeviceData.nRequestData[4]; // WD-MAX
                // SET WD CALIBRATION DATA
                m_wndScanCalibrationToolDlg.IntegratedSetWDCalibrationData(m_IntegratedData.nWDPosition, m_IntegratedData.nWDOLCoarse[nZAxis], m_IntegratedData.nWDSRotation[nZAxis], m_IntegratedData.nWDZooming[nZAxis], m_IntegratedData.nWDMinMag[nZAxis], m_IntegratedData.nWDMaxValue);
            }

            // SPOT-SIZE에 따른 CL1/CL2 CALIBRATION DATA Set & Display
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_HWSETUP_CL_DATA_ALL)
            {
                // CL1/CL2 CALIBRATION : SPOT-SIZE, CL1, CL2, AMP_R, REF_R
                int nSpotSize = RequestSetUsbDeviceData.nRequestOption;;
                m_IntegratedData.nCLSpotSize = nSpotSize;                                       // SPOT-SIZE
                m_IntegratedData.wCL1 = (ushort)(RequestSetUsbDeviceData.nRequestData[0]);                // CL1
                m_IntegratedData.wCL2 = (ushort)(RequestSetUsbDeviceData.nRequestData[1]);                // CL2
                m_IntegratedData.wCL1List[nSpotSize - 1] = (ushort)(m_IntegratedData.wCL1); // CL1 List
                m_IntegratedData.wCL2List[nSpotSize - 1] = (ushort)(m_IntegratedData.wCL2); // CL2 List
                // SET CL CALIBRATION DATA
                m_wndScanCalibrationToolDlg.IntegratedSetCLCalibrationData(m_IntegratedData.nCLSpotSize, m_IntegratedData.wCL1List[nSpotSize - 1], m_IntegratedData.wCL2List[nSpotSize - 1]);
            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:21986 ----
        // DEVICE --> CLIENT : RECEIVE USB DEVICE DATA (PART1)
        public int OnGetUsbDeviceDataPart1Processs(int wParam, object lParam)
        {
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            //
            // AUTO PROCESS 실행 결과값 처리 ( FOCUS, CONTRAST, BRIGHTNESS, CONTRAST+BRIGHTNESS )
            //
            if (RequestSetUsbDeviceData.nRequestID == REQIDX_GET_USB_DEVICE_AUTO_PROCESS_RESULT)
            {
                // AUTO START CHECK ?
                if (m_SemSEScanControlPublicData.m_bAutoStartFlag == FALSE)
                {
                    // Notice Message Dialog
                    if (m_pdlgMessageNotice != null)
                    {

                        m_pdlgMessageNotice = null;
                    }
                }

                // AUTO PROCESS RESULT 처리
                m_SemSEScanControlPublicData.m_nResultAutoProcess = RequestSetUsbDeviceData.nRequestData[0];  // AUTO PROCESS 실행 결과 종류 ( FOCUS, CONTRAST, BRIGHTNESS, CONTRAST+BRIGHTNESS )
                m_SemSEScanControlPublicData.m_nResultAutoState   = RequestSetUsbDeviceData.nRequestData[1];  // AUTO PROCESS 실행 결과 상태값 ( RUN, STOP, DONE ... )
                m_SemSEScanControlPublicData.m_nResultOldScanMode = RequestSetUsbDeviceData.nRequestData[2];  // AUTO PROCESS 실행 이전 스캔모드
                m_SemSEScanControlPublicData.m_nResultFocusCoarse = RequestSetUsbDeviceData.nRequestData[3];  // AUTO PROCESS 실행 Focus Coarse
                m_SemSEScanControlPublicData.m_nResultFocusFine   = RequestSetUsbDeviceData.nRequestData[4];  // AUTO PROCESS 실행 Focus Fine
                m_SemSEScanControlPublicData.m_nResultContrast    = RequestSetUsbDeviceData.nRequestData[5];  // AUTO PROCESS 실행 Contrast Value
                m_SemSEScanControlPublicData.m_nResultBrightness  = RequestSetUsbDeviceData.nRequestData[6];  // AUTO PROCESS 실행 Brightness Value
                m_SemSEScanControlPublicData.m_nResultMagIndex    = RequestSetUsbDeviceData.nRequestData[7];  // AUTO PROCESS 실행 Magnification Index
                m_SemSEScanControlPublicData.m_nResultGunAlignX   = RequestSetUsbDeviceData.nRequestData[8];  // AUTO PROCESS 실행 Gun Align X
                m_SemSEScanControlPublicData.m_nResultGunAlignY   = RequestSetUsbDeviceData.nRequestData[9];  // AUTO PROCESS 실행 Gun Align Y

                switch (m_SemSEScanControlPublicData.m_nResultAutoProcess)
                {
                    //
                    // 결과처리 : AUTO FOCUS PROCESS RESULT
                    //
                    case AUTO_PROCESS_FOCUS_MODE:
                    {
                        // AUTO FOCUS PROCESS : 정상작업종료
                        // AUTO START CHECK ?
                        if (m_SemSEScanControlPublicData.m_bAutoStartFlag == TRUE)
                        {
                            m_SemSEScanControlPublicData.m_nAutoStartProcess = 2; // AUTO FOCUS 실행 종료
                        }
                        // 스캔모드
                        m_SemSEScanControlPublicData.m_nSemSEScanModeValue = 0;// m_nResultOldScanMode;
                        m_IntegratedData.nScanMode       = m_SemSEScanControlPublicData.m_nResultOldScanMode;
                        // FOCUS COARSE/FINE 값 설정
                        m_IntegratedData.nFocusCoarse    = m_SemSEScanControlPublicData.m_nResultFocusCoarse;
                        m_IntegratedData.nFocusFine      = m_SemSEScanControlPublicData.m_nResultFocusFine;
                        m_wndFocusControlDlg.SetFocusCoarseFineValue(m_IntegratedData.nFocusCoarse, m_IntegratedData.nFocusFine);
                        // CONTRAST / BRIGHTNESS
                        m_IntegratedData.nContrastValue  = m_SemSEScanControlPublicData.m_nResultContrast;
                        m_IntegratedData.nBrightValue    = m_SemSEScanControlPublicData.m_nResultBrightness;
                        m_wndFocusControlDlg.SetContrastIntegratedPosition(m_IntegratedData.nContrastValue);
                        m_wndFocusControlDlg.SetBrightnessIntegratedPosition(m_IntegratedData.nBrightValue);
                        // MAGNIFICATION
                        m_IntegratedData.nMagStepIndex   = m_SemSEScanControlPublicData.m_nResultMagIndex;
                        m_wndScanControlDlg.SetMagnificationIndexIntegratedItem(m_IntegratedData.nMagStepIndex);

                        // 스캔모드 실행
                        if (m_SemSEScanControlPublicData.m_nResultOldScanMode == RED_SCAN_MODE)
                        {
                            // RED SCAN MODE
                            m_wndScanControlDlg.OnBnClickedScanControlRedScanBtn();
                        }
                        else if (m_SemSEScanControlPublicData.m_nResultOldScanMode == FAST_SCAN_MODE)
                        {
                            // FAST SCAN MODE
                            m_wndScanControlDlg.OnBnClickedScanControlFastScanBtn();
                        }
                        else if (m_SemSEScanControlPublicData.m_nResultOldScanMode == SLOW1_SCAN_MODE)
                        {
                            // SLOW1 SCAN MODE
                            m_wndScanControlDlg.OnBnClickedScanControlSlow1ScanBtn();
                        }
                        else if (m_SemSEScanControlPublicData.m_nResultOldScanMode == SLOW2_SCAN_MODE)
                        {
                            // SLOW2 SCAN MODE
                            m_wndScanControlDlg.OnBnClickedScanControlSlow2ScanBtn();
                        }
                        else
                        {
                            // FAST SCAN MODE
                            m_wndScanControlDlg.OnBnClickedScanControlFastScanBtn();
                        }
                        break;
                    }

                //
                // 결과처리 : AUTO CONTRAST PROCESS RESULT
                //
                case AUTO_PROCESS_CONTRAST_MODE:
                {
                    // AUTO CONTRAST PROCESS : 정상작업종료
                    // AUTO START CHECK ?
                    if (m_SemSEScanControlPublicData.m_bAutoStartFlag == TRUE)
                    {
                        m_SemSEScanControlPublicData.m_nAutoStartProcess = 2; // AUTO CONTRAST 실행 종료
                    }
                    // 스캔모드
                    m_SemSEScanControlPublicData.m_nSemSEScanModeValue = 0;// m_nResultOldScanMode;
                    m_IntegratedData.nScanMode       = m_SemSEScanControlPublicData.m_nResultOldScanMode;
                    // FOCUS COARSE/FINE 값 설정
                    m_IntegratedData.nFocusCoarse    = m_SemSEScanControlPublicData.m_nResultFocusCoarse;
                    m_IntegratedData.nFocusFine      = m_SemSEScanControlPublicData.m_nResultFocusFine;
                    m_wndFocusControlDlg.SetFocusCoarseFineValue(m_IntegratedData.nFocusCoarse, m_IntegratedData.nFocusFine);
                    // CONTRAST / BRIGHTNESS
                    m_IntegratedData.nContrastValue  = m_SemSEScanControlPublicData.m_nResultContrast;
                    m_IntegratedData.nBrightValue    = m_SemSEScanControlPublicData.m_nResultBrightness;
                    m_wndFocusControlDlg.SetContrastIntegratedPosition(m_IntegratedData.nContrastValue);
                    m_wndFocusControlDlg.SetBrightnessIntegratedPosition(m_IntegratedData.nBrightValue);
                    // MAGNIFICATION
                    m_IntegratedData.nMagStepIndex   = m_SemSEScanControlPublicData.m_nResultMagIndex;
                    m_wndScanControlDlg.SetMagnificationIndexIntegratedItem(m_IntegratedData.nMagStepIndex);

                    // 스캔모드 실행
                    if (m_SemSEScanControlPublicData.m_nResultOldScanMode == RED_SCAN_MODE)
                    {
                        // RED SCAN MODE
                        m_wndScanControlDlg.OnBnClickedScanControlRedScanBtn();
                    }
                    else if (m_SemSEScanControlPublicData.m_nResultOldScanMode == FAST_SCAN_MODE)
                    {
                        // FAST SCAN MODE
                        m_wndScanControlDlg.OnBnClickedScanControlFastScanBtn();
                    }
                    else if (m_SemSEScanControlPublicData.m_nResultOldScanMode == SLOW1_SCAN_MODE)
                    {
                        // SLOW1 SCAN MODE
                        m_wndScanControlDlg.OnBnClickedScanControlSlow1ScanBtn();
                    }
                    else if (m_SemSEScanControlPublicData.m_nResultOldScanMode == SLOW2_SCAN_MODE)
                    {
                        // SLOW2 SCAN MODE
                        m_wndScanControlDlg.OnBnClickedScanControlSlow2ScanBtn();
                    }
                    else
                    {
                        // FAST SCAN MODE
                        m_wndScanControlDlg.OnBnClickedScanControlFastScanBtn();
                    }
                    break;
                }

                //
                // 결과처리 : AUTO BRIGHTNESS PROCESS RESULT
                //
                case AUTO_PROCESS_BRIGHTNESS_MODE:
                {
                    // AUTO BRIGHT PROCESS : 정상작업종료
                    // AUTO START CHECK ?
                    if (m_SemSEScanControlPublicData.m_bAutoStartFlag == TRUE)
                    {
                        m_SemSEScanControlPublicData.m_nAutoStartProcess = 2; // AUTO BRIGHTNESS 실행 종료
                    }
                    // 스캔모드
                    m_SemSEScanControlPublicData.m_nSemSEScanModeValue = 0;// m_nResultOldScanMode;
                    m_IntegratedData.nScanMode       = m_SemSEScanControlPublicData.m_nResultOldScanMode;
                    // FOCUS COARSE/FINE 값 설정
                    m_IntegratedData.nFocusCoarse    = m_SemSEScanControlPublicData.m_nResultFocusCoarse;
                    m_IntegratedData.nFocusFine      = m_SemSEScanControlPublicData.m_nResultFocusFine;
                    m_wndFocusControlDlg.SetFocusCoarseFineValue(m_IntegratedData.nFocusCoarse, m_IntegratedData.nFocusFine);
                    // CONTRAST / BRIGHTNESS
                    m_IntegratedData.nContrastValue  = m_SemSEScanControlPublicData.m_nResultContrast;
                    m_IntegratedData.nBrightValue    = m_SemSEScanControlPublicData.m_nResultBrightness;
                    m_wndFocusControlDlg.SetContrastIntegratedPosition(m_IntegratedData.nContrastValue);
                    m_wndFocusControlDlg.SetBrightnessIntegratedPosition(m_IntegratedData.nBrightValue);
                    // MAGNIFICATION
                    m_IntegratedData.nMagStepIndex   = m_SemSEScanControlPublicData.m_nResultMagIndex;
                    m_wndScanControlDlg.SetMagnificationIndexIntegratedItem(m_IntegratedData.nMagStepIndex);

                    // 스캔모드 실행
                    if (m_SemSEScanControlPublicData.m_nResultOldScanMode == RED_SCAN_MODE)
                    {
                        // RED SCAN MODE
                        m_wndScanControlDlg.OnBnClickedScanControlRedScanBtn();
                    }
                    else if (m_SemSEScanControlPublicData.m_nResultOldScanMode == FAST_SCAN_MODE)
                    {
                        // FAST SCAN MODE
                        m_wndScanControlDlg.OnBnClickedScanControlFastScanBtn();
                    }
                    else if (m_SemSEScanControlPublicData.m_nResultOldScanMode == SLOW1_SCAN_MODE)
                    {
                        // SLOW1 SCAN MODE
                        m_wndScanControlDlg.OnBnClickedScanControlSlow1ScanBtn();
                    }
                    else if (m_SemSEScanControlPublicData.m_nResultOldScanMode == SLOW2_SCAN_MODE)
                    {
                        // SLOW2 SCAN MODE
                        m_wndScanControlDlg.OnBnClickedScanControlSlow2ScanBtn();
                    }
                    else
                    {
                        // FAST SCAN MODE
                        m_wndScanControlDlg.OnBnClickedScanControlFastScanBtn();
                    }
                    break;
                }

                //
                // 결과처리 : AUTO CONTRAST+BRIGHTNESS PROCESS RESULT
                //
                case AUTO_PROCESS_CONTRAST_BRIGHT_MODE:
                {
                    // AUTO CONTRAST+BRIGHTNESS PROCESS : 정상작업종료
                    // AUTO START CHECK ?
                    if (m_SemSEScanControlPublicData.m_bAutoStartFlag == TRUE)
                    {
                        m_SemSEScanControlPublicData.m_nAutoStartProcess = 2; // AUTO CONTRAST & BRIGHT 실행 종료
                    }
                    // 스캔모드
                    m_SemSEScanControlPublicData.m_nSemSEScanModeValue = 0;// m_nResultOldScanMode;
                    m_IntegratedData.nScanMode       = m_SemSEScanControlPublicData.m_nResultOldScanMode;
                    // FOCUS COARSE/FINE 값 설정
                    m_IntegratedData.nFocusCoarse    = m_SemSEScanControlPublicData.m_nResultFocusCoarse;
                    m_IntegratedData.nFocusFine      = m_SemSEScanControlPublicData.m_nResultFocusFine;
                    m_wndFocusControlDlg.SetFocusCoarseFineValue(m_IntegratedData.nFocusCoarse, m_IntegratedData.nFocusFine);
                    // CONTRAST / BRIGHTNESS
                    m_IntegratedData.nContrastValue  = m_SemSEScanControlPublicData.m_nResultContrast;
                    m_IntegratedData.nBrightValue    = m_SemSEScanControlPublicData.m_nResultBrightness;
                    m_wndFocusControlDlg.SetContrastIntegratedPosition(m_IntegratedData.nContrastValue);
                    m_wndFocusControlDlg.SetBrightnessIntegratedPosition(m_IntegratedData.nBrightValue);
                    // MAGNIFICATION
                    m_IntegratedData.nMagStepIndex   = m_SemSEScanControlPublicData.m_nResultMagIndex;
                    m_wndScanControlDlg.SetMagnificationIndexIntegratedItem(m_IntegratedData.nMagStepIndex);

                    // 스캔모드 실행
                    if (m_SemSEScanControlPublicData.m_nResultOldScanMode == RED_SCAN_MODE)
                    {
                        // RED SCAN MODE
                        m_wndScanControlDlg.OnBnClickedScanControlRedScanBtn();
                    }
                    else if (m_SemSEScanControlPublicData.m_nResultOldScanMode == FAST_SCAN_MODE)
                    {
                        // FAST SCAN MODE
                        m_wndScanControlDlg.OnBnClickedScanControlFastScanBtn();
                    }
                    else if (m_SemSEScanControlPublicData.m_nResultOldScanMode == SLOW1_SCAN_MODE)
                    {
                        // SLOW1 SCAN MODE
                        m_wndScanControlDlg.OnBnClickedScanControlSlow1ScanBtn();
                    }
                    else if (m_SemSEScanControlPublicData.m_nResultOldScanMode == SLOW2_SCAN_MODE)
                    {
                        // SLOW2 SCAN MODE
                        m_wndScanControlDlg.OnBnClickedScanControlSlow2ScanBtn();
                    }
                    else
                    {
                        // FAST SCAN MODE
                        m_wndScanControlDlg.OnBnClickedScanControlFastScanBtn();
                    }

                    // edit by SFA(wjYoo)
                    if (GetAutoBCRunFlag() == TRUE)
                    {
                        SetAutoBCCompletedFlag(TRUE);
                    }
                    // edit by SFA(wjYoo)
                    break;
                }

                //
                // 결과처리 : AUTO GUN ALIGNMENT PROCESS RESULT
                //
                case AUTO_PROCESS_GUN_ALIGN_MODE:
                {
                    // AUTO GUN ALIGNMENT PROCESS : 정상작업종료
                    // AUTO START CHECK ?
                    if (m_SemSEScanControlPublicData.m_bAutoStartFlag == TRUE)
                    {
                        m_SemSEScanControlPublicData.m_nAutoStartProcess = 2; // AUTO GUN ALIGNMENT 실행 종료
                    }
                    // 스캔모드
                    m_SemSEScanControlPublicData.m_nSemSEScanModeValue = 0;// m_nResultOldScanMode;
                    m_IntegratedData.nScanMode       = m_SemSEScanControlPublicData.m_nResultOldScanMode;

                    // GUN ALIGNMENT X, Y 설정
                    m_IntegratedData.nGAXValue       = m_SemSEScanControlPublicData.m_nResultGunAlignX;
                    m_IntegratedData.nGAYValue       = m_SemSEScanControlPublicData.m_nResultGunAlignY;
                    m_wndAlignControlDlg.SetGunAlignXIntegratedPosition(m_IntegratedData.nGAXValue);
                    m_wndAlignControlDlg.SetGunAlignYIntegratedPosition(m_IntegratedData.nGAYValue);

                    // 스캔모드 실행
                    if (m_SemSEScanControlPublicData.m_nResultOldScanMode == RED_SCAN_MODE)
                    {
                        // RED SCAN MODE
                        m_wndScanControlDlg.OnBnClickedScanControlRedScanBtn();
                    }
                    else if (m_SemSEScanControlPublicData.m_nResultOldScanMode == FAST_SCAN_MODE)
                    {
                        // FAST SCAN MODE
                        m_wndScanControlDlg.OnBnClickedScanControlFastScanBtn();
                    }
                    else if (m_SemSEScanControlPublicData.m_nResultOldScanMode == SLOW1_SCAN_MODE)
                    {
                        // SLOW1 SCAN MODE
                        m_wndScanControlDlg.OnBnClickedScanControlSlow1ScanBtn();
                    }
                    else if (m_SemSEScanControlPublicData.m_nResultOldScanMode == SLOW2_SCAN_MODE)
                    {
                        // SLOW2 SCAN MODE
                        m_wndScanControlDlg.OnBnClickedScanControlSlow2ScanBtn();
                    }
                    else
                    {
                        // FAST SCAN MODE
                        m_wndScanControlDlg.OnBnClickedScanControlFastScanBtn();
                    }
                    break;
                }
                }
            }

            return 1;
        }

    }
}
