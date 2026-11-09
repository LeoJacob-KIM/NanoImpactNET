// 원본 NanoImpact2020cfeDlg.cpp:19407-20614 을 NanoImpact.CodeGen translate로 옮긴 뒤 손으로 고친 코드.
#pragma warning disable IDE1006, IDE0059, CS0162, CS0168, CS0219
using System;
using NanoImpact.Core.Native;
using static NanoImpact.Core.Def;
using static NanoImpact.Core.Native.CRuntime;

namespace NanoImpact.Hardware.Device
{
    public sealed partial class MainDeviceService
    {
        // ---- NanoImpact2020cfeDlg.cpp:19407 ----
        // CLIENT --> DEVICE : SET USB DEVICE DATA ( STAGE width AXIS )
        public int OnSetUsbDeviceStageWidthAxis(int wParam, object lParam)
        {
            // 2020.04.28
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : STAGE width AXIS
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // 설정값
            CharBuffer szText = new CharBuffer(255 + 1);
            memset(szText, 0x00, 255);
            strcpy(szText, RequestSetUsbDeviceData.szData);

            // CNC Command : "RM" Max[mm][deg]값 정보 쓰기 -> (0:읽기,1:쓰기 / Axis0...4 / 저장데이터)
            int nWriteFlag = 1;

            // USB DEVICE : STAGE 정보 요청
            if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_WIDTH_X_AXIS)
            {
                // Stage AXIS width [mm],[deg] : X-Axis 정보 쓰기
                m_dev.AfxScanBridgeRMCommand(nWriteFlag, IDX_X_AXIS, ToStr(szText));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_WIDTH_Y_AXIS)
            {
                // Stage AXIS width [mm],[deg] : Y-Axis 정보 쓰기
                m_dev.AfxScanBridgeRMCommand(nWriteFlag, IDX_Y_AXIS, ToStr(szText));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_WIDTH_Z_AXIS)
            {
                // Stage AXIS width [mm],[deg] : Z-Axis 정보 쓰기
                m_dev.AfxScanBridgeRMCommand(nWriteFlag, IDX_Z_AXIS, ToStr(szText));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_LIMIT_Z_AXIS)
            {
                // Stage AXIS width [mm],[deg] : Z-Axis Top Limit 정보 쓰기
                m_dev.AfxScanBridgeZTCommand(nWriteFlag, ToStr(szText));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_WIDTH_T_AXIS)
            {
                // Stage AXIS width [mm],[deg] : T-Axis 정보 쓰기
                m_dev.AfxScanBridgeRMCommand(nWriteFlag, IDX_T_AXIS, ToStr(szText));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_ZERO_T_AXIS)
            {
                // Stage AXIS width [mm],[deg] : T-Axis ZERO 정보 쓰기
                m_dev.AfxScanBridgeRMCommand(nWriteFlag, IDX_T_AXIS + 1, ToStr(szText));
            }
               
            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:19463 ----
        // CLIENT --> DEVICE : SET USB DEVICE DATA ( STAGE accelation AXIS )
        public int OnSetUsbDeviceStageAccelAxis(int wParam, object lParam)
        {
            // 2020.04.28
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : STAGE accelation AXIS
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // 설정값
            CharBuffer szText = new CharBuffer(255 + 1);
            memset(szText, 0x00, 255);
            strcpy(szText, RequestSetUsbDeviceData.szData);

            // CNC Command : "RA" accel값 정보 쓰기 -> (0:읽기,1:쓰기 / Axis0...4 / 저장데이터)
            int nWriteFlag = 1;

            // USB DEVICE : STAGE 정보 요청
            if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_ACCEL_X_AXIS)
            {
                // Stage accelation : X-Axis 정보 쓰기
                m_dev.AfxScanBridgeRACommand(nWriteFlag, IDX_X_AXIS, ToStr(szText));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_ACCEL_Y_AXIS)
            {
                // Stage accelation : Y-Axis 정보 쓰기
                m_dev.AfxScanBridgeRACommand(nWriteFlag, IDX_Y_AXIS, ToStr(szText));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_ACCEL_Z_AXIS)
            {
                // Stage accelation : Z-Axis 정보 쓰기
                m_dev.AfxScanBridgeRACommand(nWriteFlag, IDX_Z_AXIS, ToStr(szText));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_ACCEL_R_AXIS)
            {
                // Stage accelation : R-Axis 정보 쓰기
                m_dev.AfxScanBridgeRACommand(nWriteFlag, IDX_R_AXIS, ToStr(szText));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_ACCEL_T_AXIS)
            {
                // Stage accelation : T-Axis 정보 쓰기
                m_dev.AfxScanBridgeRACommand(nWriteFlag, IDX_T_AXIS, ToStr(szText));
            }
            // X-Y AXIS ACCEL SETTING : 기본 ACCEL --> 배율에 따른 ACCEL로 재설정
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_MAG_XY_AXIS_ACCEL)
            {
                if (RequestSetUsbDeviceData.nRequestData[0] < 0 && RequestSetUsbDeviceData.nRequestData[1] < 0)
                {
                    // 배율에 따른 X-Y AXIS 이동속도 자동조절
                    StageXYAxisMagnificationMovingSpeedChange(m_IntegratedData.nMagStepIndex);
                    return 1;
                }

                m_IntegratedData.nAxisAccCalVal[IDX_X_AXIS] = RequestSetUsbDeviceData.nRequestData[0];
                m_IntegratedData.nAxisAccCalVal[IDX_Y_AXIS] = RequestSetUsbDeviceData.nRequestData[1];

                // X-Axis 정보 쓰기
                ZeroMem(szText);
                sprintf(szText, "%d", RequestSetUsbDeviceData.nRequestData[0]);
                m_dev.AfxScanBridgeRACommand(nWriteFlag, IDX_X_AXIS, ToStr(szText));

                // Y-Axis 정보 쓰기
                ZeroMem(szText);
                sprintf(szText, "%d", RequestSetUsbDeviceData.nRequestData[1]);
                m_dev.AfxScanBridgeRACommand(nWriteFlag, IDX_Y_AXIS, ToStr(szText));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_ACCEL_B_AXIS)
            {

                // Stage accelation : BSE-Axis 정보 쓰기
                m_dev.AfxScanBridgeRACommand(nWriteFlag, IDX_B_AXIS, ToStr(szText));
            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:19543 ----
        // CLIENT --> DEVICE : SET USB DEVICE DATA ( STAGE center AXIS )
        public int OnSetUsbDeviceStageCenterAxis(int wParam, object lParam)
        {
            // 2020.04.28
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : STAGE center AXIS
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            double dXCenter = RequestSetUsbDeviceData.dRequestData[0];
            double dYCenter = RequestSetUsbDeviceData.dRequestData[1];
            double dBCenter = RequestSetUsbDeviceData.dRequestData[0];

            int    nValue1  = RequestSetUsbDeviceData.nRequestData[0];

            // 설정값
            CharBuffer szText1 = new CharBuffer(255 + 1);
            CharBuffer szText2 = new CharBuffer(255 + 1);

            memset(szText1, 0x00, 255);
            memset(szText2, 0x00, 255);

            int nWriteFlag = 1;

            // USB DEVICE : STAGE 정보 요청
            if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_CENTER_X_AXIS)
            {
                // CNC Command : "MC" -> Write Center Pos (X축의 센터 위치 값 읽기 및 적용한다)
                strcpy_s(szText1, "0");
                sprintf_s(szText2, "%.2f", dXCenter);
                // Stage Center Pos : X-Axis 정보 쓰기
                m_dev.AfxScanBridgeMCCommand(nWriteFlag, ToStr(szText1), ToStr(szText2));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_CENTER_Y_AXIS)
            {
                // CNC Command : "MC" -> Write Center Pos (Y축의 센터 위치 값 읽기 및 적용한다)
                strcpy_s(szText1, "1");
                sprintf_s(szText2, "%.2f", dYCenter);
                // Stage Center Pos : Y-Axis 정보 쓰기
                m_dev.AfxScanBridgeMCCommand(nWriteFlag, ToStr(szText1), ToStr(szText2));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_HW_CENTER_AXIS)
            {
                // CNC Command : "MC" -> Write Center Pos (X,Y축의 센터 위치 값 읽기 및 적용한다)
                // 2021.03.31
                // Stage Set H/W Cur Pos Center : szText1, szText2 모두 NULL DATA 로 설정한다
                m_dev.AfxScanBridgeMCCommand(nWriteFlag, ToStr(szText1), ToStr(szText2));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_ROTARY_AREA_PULSE)
            {
                // CNC Command : "BF" -> Write Pulse ( ROTARY 영역 PULSE 적용 )
                sprintf_s(szText1, "%d", nValue1);
                // 2021.03.31
                // Stage Set Rotary Area Pulse
                m_dev.AfxScanBridgeBFCommand(nWriteFlag, ToStr(szText1));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_CENTER_B_AXIS)
            {
                // Stage BSE Set Scan Position : BSE-Axis 정보 쓰기

                if (m_dev.AfxGetIntegratedPointer() != null)
                {
                    double dBsePos = dBCenter;
                    m_dev.AfxGetIntegratedPointer().dStageOptionData[IDX_STAGE_OPTION_DOUBLE_BSE_POSITION] = dBsePos;
                    // BSE SET SCAN POSITION VALUE
                    m_wndStageCalibrationToolDlg.IntegratedSetStageOptionData(IDX_STAGE_OPTION_DOUBLE_TYPE, IDX_STAGE_OPTION_DOUBLE_BSE_POSITION, 0, 0, 0, 0, dBsePos);
                    m_wndStageControlDlg.SetStageOptionDataDOUBLEType(dBsePos);
                }

            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:19621 ----
        // CLIENT --> DEVICE : SET USB DEVICE DATA ( STAGE home move AXIS )
        public int OnSetUsbDeviceStageHomeAxis(int wParam, object lParam)
        {
            // 2020.04.28
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : STAGE home move AXIS
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // USB DEVICE : STAGE 정보 요청
            if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_HOME_X_AXIS)
            {
                // Stage Home Move : X-Axis 정보 쓰기
                m_dev.AfxScanBridgeMHCommand(IDX_X_AXIS);
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_HOME_Y_AXIS)
            {
                // Stage Home Move : Y-Axis 정보 쓰기
                m_dev.AfxScanBridgeMHCommand(IDX_Y_AXIS);
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_HOME_Z_AXIS)
            {
                // Stage Home Move : Z-Axis 정보 쓰기
                m_dev.AfxScanBridgeMHCommand(IDX_Z_AXIS);
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_HOME_R_AXIS)
            {
                // Stage Home Move : R-Axis 정보 쓰기
                m_dev.AfxScanBridgeMHCommand(IDX_R_AXIS);
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_HOME_T_AXIS)
            {
                // Stage Home Move : T-Axis 정보 쓰기
                m_dev.AfxScanBridgeMHCommand(IDX_T_AXIS);
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_HOME_B_AXIS)
            {
                // Stage Home Move : BSE-Axis 정보 쓰기
                m_dev.AfxScanBridgeMHCommand(IDX_B_AXIS);
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_LIMIT_B_AXIS)
            {
                // Stage Limit(set point) Move : BSE-Axis 정보 쓰기
                
            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:19674 ----
        // CLIENT --> DEVICE : SET USB DEVICE DATA ( STAGE +/- move AXIS )
        public int OnSetUsbDeviceStageMoveAxis(int wParam, object lParam)
        {
            // 2020.04.28
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : STAGE +/- move AXIS
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            CharBuffer szData = new CharBuffer(255 + 1);
            ZeroMem(szData);
            sprintf(szData, "%.6f", fabs(RequestSetUsbDeviceData.dRequestData[0]));

            // USB DEVICE : STAGE 정보 요청
            if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_MOVE_X_AXIS)
            {
                // STAGE +/- move AXIS : X-Axis 정보 쓰기
                m_dev.AfxScanBridgeMMCommand(IDX_X_AXIS, RequestSetUsbDeviceData.nRequestOption, ToStr(szData));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_MOVE_Y_AXIS)
            {
                // STAGE +/- move AXIS : Y-Axis 정보 쓰기
                m_dev.AfxScanBridgeMMCommand(IDX_Y_AXIS, RequestSetUsbDeviceData.nRequestOption, ToStr(szData));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_MOVE_Z_AXIS)
            {
                // STAGE +/- move AXIS : Z-Axis 정보 쓰기
                m_dev.AfxScanBridgeMMCommand(IDX_Z_AXIS, RequestSetUsbDeviceData.nRequestOption, ToStr(szData));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_MOVE_R_AXIS)
            {
                // STAGE +/- move AXIS : R-Axis 정보 쓰기
                m_dev.AfxScanBridgeMMCommand(IDX_R_AXIS, RequestSetUsbDeviceData.nRequestOption, ToStr(szData));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_MOVE_T_AXIS)
            {
                // STAGE +/- move AXIS : T-Axis 정보 쓰기
                m_dev.AfxScanBridgeMMCommand(IDX_T_AXIS, RequestSetUsbDeviceData.nRequestOption, ToStr(szData));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_AXIS_ALL_STOP)
            {
                // STAGE AXIS ALL STOP
                m_dev.AfxScanBridgeASCommand();
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_SAMPLE_SCAN_POS_MOVE)
            {
                // SAMPLE : GO TO THE SCAN POSITION ( X-Y-Z AXIS )
                m_dev.AfxScanBridgeGotoXYZRTSampleLoadPosition();
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_SAMPLE_EXCHANGE_POS_MOVE)
            {
                // SAMPLE : GO TO THE EXCHANGE POSITION ( X-Y-Z AXIS )
                m_dev.AfxScanBridgeGotoXYZRTSampleUnloadPosition();
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_EMERGENCY_STOP)
            {
                // STAGE AXIS EMERGENCY STOP 실행
                m_dev.AfxScanBridgeStageAxisEmergencyStopExecute();
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_RESET_CONTROL)
            {
                // STAGE RESET CONTROL 실행
                m_dev.AfxScanBridgeStageAxisResetControlExecute();
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_STAGE_MOVE_B_AXIS)
            {
                // STAGE +/- move AXIS : BSE-Axis 정보 쓰기
                m_dev.AfxScanBridgeMMCommand(IDX_B_AXIS, RequestSetUsbDeviceData.nRequestOption, ToStr(szData));
            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:19751 ----
        // CLIENT --> DEVICE : SET USB DEVICE DATA ( STAGE OPTION DATA )
        public int OnSetUsbDeviceStageOptionData(int wParam, object lParam)
        {
            // 2020.04.28
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : STAGE OPTION DATA
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // STAGE OPTION DATA 설정
            if (RequestSetUsbDeviceData.nRequestType == IDX_STAGE_OPTION_BOOL_TYPE)
            {
                // BOOL 형 Data Type
                if (RequestSetUsbDeviceData.nRequestData[0] == IDX_STAGE_OPTION_BOOL_XY_REVERSE)
                {
                    m_IntegratedData.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_XY_REVERSE] = (int)RequestSetUsbDeviceData.nRequestData[1];         // X<-->Y 반전
                }
                else if (RequestSetUsbDeviceData.nRequestData[0] == IDX_STAGE_OPTION_BOOL_X_LR_REVERSE)
                {
                    m_IntegratedData.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_X_LR_REVERSE] = (int)RequestSetUsbDeviceData.nRequestData[1];       // X LR 반전
                }
                else if (RequestSetUsbDeviceData.nRequestData[0] == IDX_STAGE_OPTION_BOOL_Y_TB_REVERSE)
                {
                    m_IntegratedData.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_Y_TB_REVERSE] = (int)RequestSetUsbDeviceData.nRequestData[1];       // Y TB 반전
                }
                else if (RequestSetUsbDeviceData.nRequestData[0] == IDX_STAGE_OPTION_BOOL_Z_UD_REVERSE)
                {
                    m_IntegratedData.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_Z_UD_REVERSE] = (int)RequestSetUsbDeviceData.nRequestData[1];       // Z UD 반전
                }
                else if (RequestSetUsbDeviceData.nRequestData[0] == IDX_STAGE_OPTION_BOOL_Z_MOVE_ECHO)
                {
                    m_IntegratedData.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_Z_MOVE_ECHO] = (int)RequestSetUsbDeviceData.nRequestData[1];       // Z MOVING ECHO ON/OFF
                }
                else if (RequestSetUsbDeviceData.nRequestData[0] == IDX_STAGE_OPTION_BOOL_ALL_DATA_VIEW)
                {
                    m_IntegratedData.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_ALL_DATA_VIEW] = (int)RequestSetUsbDeviceData.nRequestData[1];     // ALL DATA VIEW : 5 Axis 사용
                }

                // STAGE UI DIALOG : Set Stage Option
                m_wndStageControlDlg.SetStageOptionDataBOOLType(m_IntegratedData.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_XY_REVERSE]  , m_IntegratedData.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_X_LR_REVERSE],
                                                                m_IntegratedData.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_Y_TB_REVERSE], m_IntegratedData.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_Z_UD_REVERSE],
                                                                m_IntegratedData.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_Z_MOVE_ECHO] , m_IntegratedData.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_ALL_DATA_VIEW]);
                // SET STAGE OPTION DATA
                m_dev.AfxIntegratedSetStageOptionData(RequestSetUsbDeviceData.nRequestType, RequestSetUsbDeviceData.nRequestData[0], (int)RequestSetUsbDeviceData.nRequestData[1], (byte)RequestSetUsbDeviceData.nRequestData[2], (int)RequestSetUsbDeviceData.nRequestData[3], (uint)RequestSetUsbDeviceData.nRequestData[4], RequestSetUsbDeviceData.dRequestData[0]);

            }
            else if (RequestSetUsbDeviceData.nRequestType == IDX_STAGE_OPTION_BYTE_TYPE)
            {
                // BYTE 형 Data Type
            }
            else if (RequestSetUsbDeviceData.nRequestType == IDX_STAGE_OPTION_INT_TYPE)
            {
                // INT 형 Data Type
            }
            else if (RequestSetUsbDeviceData.nRequestType == IDX_STAGE_OPTION_DWORD_TYPE)
            {
                // DWORD 형 Data Type
            }
            else if (RequestSetUsbDeviceData.nRequestType == IDX_STAGE_OPTION_DOUBLE_TYPE)
            {
                // DOUBLE 형 Data Type
            }


            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:19822 ----
        // CLIENT --> DEVICE : SET USB DEVICE DATA ( HW SETUP : XY-AMP,OFFSET )
        public int OnSetUsbDeviceHWSetupXYAmpOffset(int wParam, object lParam)
        {
            // 2020.05.01
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : HW SETUP XY-AMP,OFFSET
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // SCAN MODE
            m_IntegratedData.nScanMode = m_SemSEScanControlPublicData.m_nSemSEScanModeValue;
            // X SCAN AMP
            if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_X_AMP)
            {
                // 새로운 X SCAN AMP 값 설정
                m_IntegratedData.nXScanAmpVal[m_IntegratedData.nScanMode] = RequestSetUsbDeviceData.nRequestData[0];
            }
            // X SCAN OFFSET
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_X_OFF)
            {
                // 새로운 X SCAN OFFSET 값 설정
                m_IntegratedData.nXScanOffset[m_IntegratedData.nScanMode] = RequestSetUsbDeviceData.nRequestData[0];
            }
            // Y SCAN AMP
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_Y_AMP)
            {
                // 새로운 Y SCAN AMP 값 설정
                m_IntegratedData.nYScanAmpVal[m_IntegratedData.nScanMode] = RequestSetUsbDeviceData.nRequestData[0];
            }
            // Y SCAN OFFSET
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_Y_OFF)
            {
                // 새로운 Y SCAN OFFSET 값 설정
                m_IntegratedData.nYScanOffset[m_IntegratedData.nScanMode] = RequestSetUsbDeviceData.nRequestData[0];
            }

            // SCAN MODE 추가
            RequestSetUsbDeviceData.nRequestData[1] = RequestSetUsbDeviceData.nRequestData[0];
            RequestSetUsbDeviceData.nRequestData[0] = m_IntegratedData.nScanMode;

            // X-Y SCAN AMP & OFFSET DATA VALUE 설정
            m_pAssistDeviceTool.SetUsbDeviceHWSetupXYAmpOffsetOptionValue(RequestSetUsbDeviceData);

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:19872 ----
        // CLIENT --> DEVICE : SET USB DEVICE DATA ( HW SETUP : ORTHOGONAL )
        public int OnSetUsbDeviceHWSetupOrthogonal(int wParam, object lParam)
        {
            // 2020.05.01
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : HW SETUP XY-AMP,OFFSET
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // ORTHOGONAL VALUE
            m_IntegratedData.nOrthogonal = RequestSetUsbDeviceData.nRequestData[0];

            // ORTHOGONAL DATA VALUE 설정
            m_pAssistDeviceTool.SetUsbDeviceHWSetupOrthogonalOptionValue(RequestSetUsbDeviceData);

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:19894 ----
        // CLIENT --> DEVICE : SET USB DEVICE DATA ( HW SETUP : SPOTSIZE CL1,CL2 )
        public int OnSetUsbDeviceHWSetupSpotSizeCL(int wParam, object lParam)
        {
            // 2020.05.01
            if (lParam == null) return 0;

            //
            // CL LMH MODE
            //
            
            int nCLLMHModeUse = 0;
            if (m_dev.AfxGetIntegratedPointer() != null)
            {
                nCLLMHModeUse = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MODE_USE_FLAG];
            }
            //

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : HW SETUP XY-AMP,OFFSET
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // SPOT-SIZE 
            if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_CL_DATA_SPOTSIZE)
            {
                // 2021.03.30
                // CL 1,2 OFF
                if (m_bCLOnOffFlag == FALSE)
                {
                    // CL 1,2 OFF 일 경우 SPOT-SIZE에 따른 CL1, CL2 값 적용안함
                    return 0;
                }
                // 새로운 SPOT-SIZE 선택값 설정
                m_IntegratedData.nCLSpotSize = RequestSetUsbDeviceData.nRequestData[0];
                // VACUUM CONTROL DLG : SPOT SIZE 값 설정
                m_wndBeamControlDlg.SetSpotSizeIntegratedItem(m_IntegratedData.nCLSpotSize);
                // SCAN CALIBRATION : SPOT-SIZE에 따른 CL1, CL2
                m_wndScanCalibrationToolDlg.SetSpotSizeCLCalibrationData(m_IntegratedData.nCLSpotSize, m_IntegratedData.wCL1List[m_IntegratedData.nCLSpotSize - 1], m_IntegratedData.wCL2List[m_IntegratedData.nCLSpotSize - 1], 0);
                // 2021.04.05
                // SPOT SIZE 값
                m_nSpotSizeValue = m_IntegratedData.nCLSpotSize;// m_IntegratedData.nSpotSize;
                // SPOT SIZE 값 표시
                // Display 1st Information Text
                Update1stImageInfoBitmap(m_SemSEScanControlPublicData.m_n1stImageInfoDrawX, m_SemSEScanControlPublicData.m_n1stImageInfoDrawY, m_SemSEScanControlPublicData.m_n1stImageInfoWidth, m_SemSEScanControlPublicData.m_n1stImageInfoHeight);
            }
            // CL1
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_SPOTSIZE_CL1)
            {
                // 2021.03.30
                // CL 1,2 OFF
                if (m_bCLOnOffFlag == FALSE)
                {
                    // CL LMH MODE 에 상관없이 실행
                    if (nCLLMHModeUse == 0 || nCLLMHModeUse == 1)
                    {
                        // CL 1,2 OFF 일 경우 SPOT-SIZE에 따른 CL1, CL2 값 적용안함
                        int nCL1 = RequestSetUsbDeviceData.nRequestData[0];    // 새로운 CL1 설정값
                        m_dev.AfxIntegratedSetCL1(nCL1);                             // CL1 값 설정
                        m_dev.AfxIntegratedSetCL1();                                 // CL1 USB DEVICE 적용
                    }
                    return 0;
                }
                // 새로운 CL1 설정값
                m_IntegratedData.wCL1List[m_IntegratedData.nCLSpotSize - 1] = (ushort)(RequestSetUsbDeviceData.nRequestData[0]);
                // SCAN CALIBRATION : SPOT-SIZE에 따른 CL1
                m_wndScanCalibrationToolDlg.SetSpotSizeCL1CalibrationData(m_IntegratedData.wCL1List[m_IntegratedData.nCLSpotSize - 1], 1);
            }
            // CL2
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_SPOTSIZE_CL2)
            {
                // 2021.03.30
                // CL 1,2 OFF
                if (m_bCLOnOffFlag == FALSE)
                {
                    // CL LMH MODE 에 상관없이 실행
                    if (nCLLMHModeUse == 0 || nCLLMHModeUse == 1)
                    {
                        // CL 1,2 OFF 일 경우 SPOT-SIZE에 따른 CL1, CL2 값 적용안함
                        int nCL2 = RequestSetUsbDeviceData.nRequestData[0];    // 새로운 CL2 설정값
                        m_dev.AfxIntegratedSetCL2(nCL2);                             // CL2 값 설정
                        m_dev.AfxIntegratedSetCL2();                                 // CL2 USB DEVICE 적용
                    }
                    return 0;
                }
                // 새로운 CL2 설정값
                m_IntegratedData.wCL2List[m_IntegratedData.nCLSpotSize - 1] = (ushort)(RequestSetUsbDeviceData.nRequestData[0]);
                // SCAN CALIBRATION : SPOT-SIZE에 따른 CL2
                m_wndScanCalibrationToolDlg.SetSpotSizeCL2CalibrationData(m_IntegratedData.wCL2List[m_IntegratedData.nCLSpotSize - 1], 1);
            }
            // SPOT-SIZE에 따른 CL1,CL2 설정값 적용
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_CL_DATA_APPLY)
            {
                // 2021.03.30
                // CL 1,2 OFF
                if (m_bCLOnOffFlag == FALSE)
                {
                    // CL 1,2 OFF 일 경우 SPOT-SIZE에 따른 CL1, CL2 값 적용안함
                    return 0;
                }
                // 새로운 SPOT-SIZE 선택값 설정
                m_IntegratedData.nCLSpotSize = RequestSetUsbDeviceData.nRequestData[0];
                // 새로운 CL1 설정값 적용
                m_IntegratedData.wCL1List[m_IntegratedData.nCLSpotSize - 1] = (ushort)(RequestSetUsbDeviceData.nRequestData[1]);
                // 새로운 CL2 설정값 적용
                m_IntegratedData.wCL2List[m_IntegratedData.nCLSpotSize - 1] = (ushort)(RequestSetUsbDeviceData.nRequestData[2]);
                // SCAN CALIBRATION : SPOT-SIZE에 따른 CL1, CL2
                m_wndScanCalibrationToolDlg.SetSpotSizeCLCalibrationData(m_IntegratedData.nCLSpotSize, m_IntegratedData.wCL1List[m_IntegratedData.nCLSpotSize - 1], m_IntegratedData.wCL2List[m_IntegratedData.nCLSpotSize - 1], 1);
            }

            // SPOT-SIZE에 따른 CL1, CL2 DATA VALUE 설정
            m_pAssistDeviceTool.SetUsbDeviceHWSetupSpotSizeCLOptionValue(RequestSetUsbDeviceData);

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:20011 ----
        // CLIENT --> DEVICE : SET USB DEVICE DATA ( HW SETUP : SCAN CLOCK CALIBRATION )
        public int OnSetUsbDeviceHWSetupClockCalibration(int wParam, object lParam)
        {
            // 2020.05.01
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : HW SETUP XY-AMP,OFFSET
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // SCAN MODE
            m_IntegratedData.nScanMode = m_SemSEScanControlPublicData.m_nSemSEScanModeValue;

            // SCAN SKIP
            if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_SCAN_SKIP)
            {
                // 새로운 SCAN SKIP 값 설정
                m_IntegratedData.nSkipPixNum[m_IntegratedData.nScanMode] = RequestSetUsbDeviceData.nRequestData[0];
            }
            // SCAN OSAM
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_SCAN_OSAM)
            {
                // 새로운 SCAN OSAM 값 설정
                m_IntegratedData.nOverSampleNum[m_IntegratedData.nScanMode] = RequestSetUsbDeviceData.nRequestData[0];
            }
            // FRAME FREQ
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_FRAME_FREQ)
            {
                // 새로운 FRAME FREQ 값 설정
                m_IntegratedData.nFrameSec[m_IntegratedData.nScanMode] = RequestSetUsbDeviceData.nRequestData[0];
            }
            // X SCAN UP
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_X_SCAN_UP)
            {
                // 새로운 X SCAN UP 값 설정
                m_IntegratedData.nXScanUpNum[m_IntegratedData.nScanMode] = RequestSetUsbDeviceData.nRequestData[0];
            }
            // X SCAN DOWN
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_X_SCAN_DOWN)
            {
                // 새로운 X SCAN DOWN 값 설정
                m_IntegratedData.nXScanDownNum[m_IntegratedData.nScanMode] = RequestSetUsbDeviceData.nRequestData[0];
            }
            // Y SCAN UP
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_Y_SCAN_UP)
            {
                // 새로운 Y SCAN UP 값 설정
                m_IntegratedData.nYScanUpNum[m_IntegratedData.nScanMode] = RequestSetUsbDeviceData.nRequestData[0];
            }
            // Y SCAN DOWN
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_X_SCAN_DOWN)
            {
                // 새로운 Y SCAN DOWN 값 설정
                m_IntegratedData.nYScanDownNum[m_IntegratedData.nScanMode] = RequestSetUsbDeviceData.nRequestData[0];
            }
            // SYNC ON/OFF
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_SCAN_SYNC)
            {
                // SYNC : ON->PHOTO CALIBRATION MODE, OFF->PHOTO SCAN MODE
                if (RequestSetUsbDeviceData.nRequestData[0] == 1) m_bPhotoCalibrationFlag = TRUE; // PHOTO SCAN CALIBRATION MODE FLAG
                else m_bPhotoCalibrationFlag = FALSE; // PHOTO SCAN MODE FLAG
                return 1;
            }

            // SCAN MODE 추가
            RequestSetUsbDeviceData.nRequestData[1] = RequestSetUsbDeviceData.nRequestData[0];
            RequestSetUsbDeviceData.nRequestData[0] = m_IntegratedData.nScanMode;

            // SCAN CLOCK & FREQ DATA VALUE 설정
            m_pAssistDeviceTool.SetUsbDeviceHWSetupScanClockOptionValue(RequestSetUsbDeviceData);

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:20088 ----
        // CLIENT --> DEVICE : SET USB DEVICE DATA ( HW SETUP : X-Y AMP/OFFSET CALIBRATION )
        public int OnSetUsbDeviceHWSetupXYAmpOffsetCalibration(int wParam, object lParam)
        {
            // 2020.05.01
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : HW SETUP XY-AMP,OFFSET
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            ///
            // SCAN MODE에 따른 X-Y AMP/OFFSET 및 SCAN FREQ 설정값
            ///
            // SCAN MODE에 따른 X-Y AMP/OFFSET 배율보정 설정값
            if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_XY_AMP_OFFSET_APPLY)
            {
                // X-Y AMP/OFFSET 설정을 위한 SCAN MODE
                int nScanMode = RequestSetUsbDeviceData.nRequestData[0];
                m_IntegratedData.nXScanAmpVal[nScanMode] = RequestSetUsbDeviceData.nRequestData[1]; // 새로운 X AMP    설정값
                m_IntegratedData.nXScanOffset[nScanMode] = RequestSetUsbDeviceData.nRequestData[2]; // 새로운 X OFFSET 설정값
                m_IntegratedData.nYScanAmpVal[nScanMode] = RequestSetUsbDeviceData.nRequestData[3]; // 새로운 Y AMP    설정값
                m_IntegratedData.nYScanOffset[nScanMode] = RequestSetUsbDeviceData.nRequestData[4]; // 새로운 Y OFFSET 설정값
            }
            // SCAN MODE에 따른 X-Y SCAN FREQ 설정값
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_XY_SCAN_FREQ_APPLY)
            {
                // X-Y AMP/OFFSET 설정을 위한 SCAN MODE
                int nScanMode = RequestSetUsbDeviceData.nRequestData[0];
                m_IntegratedData.nSkipPixNum[nScanMode]    = RequestSetUsbDeviceData.nRequestData[1]; // 새로운 SCAN  SKIP  설정값
                m_IntegratedData.nOverSampleNum[nScanMode] = RequestSetUsbDeviceData.nRequestData[2]; // 새로운 SCAN  OVER  SAMPLE 설정값
                m_IntegratedData.nFrameSec[nScanMode]      = RequestSetUsbDeviceData.nRequestData[3]; // 새로운 FRAME FREQ  설정값
                m_IntegratedData.nFrameSecVal[nScanMode]   = RequestSetUsbDeviceData.nRequestData[3]; // 새로운 FRAME FREQ  설정값
                m_IntegratedData.nXScanUpNum[nScanMode]    = RequestSetUsbDeviceData.nRequestData[4]; // 새로운 X SCAN UP   설정값
                m_IntegratedData.nXScanDownNum[nScanMode]  = RequestSetUsbDeviceData.nRequestData[5]; // 새로운 X SCAN DOWN 설정값
                m_IntegratedData.nYScanUpNum[nScanMode]    = RequestSetUsbDeviceData.nRequestData[6]; // 새로운 Y SCAN UP   설정값
                m_IntegratedData.nYScanDownNum[nScanMode]  = RequestSetUsbDeviceData.nRequestData[7]; // 새로운 Y SCAN DOWN 설정값
            }

            // SCAN MODE에 따른 X-Y AMP/OFFSET 및 SCAN FREQ 설정값
            m_pAssistDeviceTool.SetUsbDeviceHWSetupXYAmpOffsetCalibrationOptionValue(RequestSetUsbDeviceData);

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:20135 ----
        // CLIENT --> DEVICE : SET USB DEVICE DATA ( HW SETUP : WD SETUP DATA CALIBRATION )
        public int OnSetUsbDeviceHWSetupWDSetupDataCalibration(int wParam, object lParam)
        {
            // 2020.05.01
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : HW SETUP XY-AMP,OFFSET
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            ///
            // WD에 따른 배율보정 설정값
            ///
            // WD에 따른 배율보정을 위한 설정값 자동저장 선택값
            if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_WD_SETUP_ONOFF)
            {
                // 새로운 WD 자동저장 선택값 설정
                m_IntegratedData.bWDSetupFlag = RequestSetUsbDeviceData.nRequestData[0];
                if (m_IntegratedData.bWDSetupFlag == TRUE)
                {
                    // 디가우징을 위한 OL값 RESET
                    //m_pAssistDeviceTool->SetSEMControlFocusOLValue(SEM_FOCUS_COARSE_STR, SEM_FOCUS_FINE_STR);
                    // 2021.06.15
                    // Z-AXIS(WD)에 해당하는 FOCUS COARSE DEGAUSSING 실행
                    int nZAxis = RequestSetUsbDeviceData.nRequestData[1];
                    // WD 값이 변동이 있으면 WD에 따른 FOCUS DEGAUSSING 및 FOCUS COARSE 실행한다0725
                    
                    m_dev.AfxIntegratedFocusDegaussing(nZAxis*10);
                    m_dev.AfxThreadWaitDialog(this.GetSafeHwnd());
                }
                // 새로운 WD(Z-AXIS) 선택값 설정
                m_IntegratedData.nWDStepNumber = RequestSetUsbDeviceData.nRequestData[1];
                // 새로운 WD-MAX 설정값
                m_IntegratedData.nWDStepMax    = RequestSetUsbDeviceData.nRequestData[6];
                // 새로운 OL 설정값
                m_IntegratedData.nWDOLCoarse[m_IntegratedData.nWDStepNumber]  = RequestSetUsbDeviceData.nRequestData[2];
                // 새로운 S-ROT 설정값
                m_IntegratedData.nWDSRotation[m_IntegratedData.nWDStepNumber] = RequestSetUsbDeviceData.nRequestData[3];
                // 새로운 ZOOM 설정값
                m_IntegratedData.nWDZooming[m_IntegratedData.nWDStepNumber]   = RequestSetUsbDeviceData.nRequestData[4];
                // 새로운 MIN-MAG 설정값
                m_IntegratedData.nWDMinMag[m_IntegratedData.nWDStepNumber]    = RequestSetUsbDeviceData.nRequestData[5];

                // 2021.06.03
                // m_pIntegrated 포인터에 새로운 WD SETUP 데이터 저장
                if (m_dev.AfxGetIntegratedPointer() != null)
                {
                    // WD SETUP FLAG
                    m_dev.AfxGetIntegratedPointer().bWDSetupFlag  = m_IntegratedData.bWDSetupFlag;
                    // WD SETUP : Z-AXIS POSITION
                    m_dev.AfxGetIntegratedPointer().nWDStepNumber = m_IntegratedData.nWDStepNumber;
                    // WD SETUP : MAX VALUE
                    m_dev.AfxGetIntegratedPointer().nWDStepMax    = m_IntegratedData.nWDStepMax;
                    // WD SETUP : FOCUS(OL) COARSE VALUE
                    m_dev.AfxGetIntegratedPointer().nWDOLCoarse[m_IntegratedData.nWDStepNumber]  = m_IntegratedData.nWDOLCoarse[m_IntegratedData.nWDStepNumber];
                    // WD SETUP : SCAN ROTATION VALUE
                    m_dev.AfxGetIntegratedPointer().nWDSRotation[m_IntegratedData.nWDStepNumber] = m_IntegratedData.nWDSRotation[m_IntegratedData.nWDStepNumber];
                    // WD SETUP : ZOOM VALUE
                    m_dev.AfxGetIntegratedPointer().nWDZooming[m_IntegratedData.nWDStepNumber]   = m_IntegratedData.nWDZooming[m_IntegratedData.nWDStepNumber];
                    // WD SETUP : MIN-MAG VALUE
                    m_dev.AfxGetIntegratedPointer().nWDMinMag[m_IntegratedData.nWDStepNumber]    = m_IntegratedData.nWDMinMag[m_IntegratedData.nWDStepNumber];
                }

                // 새로운 WD 자동저장 CHECK?
                if (m_IntegratedData.bWDSetupFlag == TRUE)
                {
                    // OL값 재설정
                    m_wndFocusControlDlg.SetFocusCoarseFineValue(m_IntegratedData.nWDOLCoarse[m_IntegratedData.nWDStepNumber], SEM_FOCUS_FINE_STR);      // 위치값 재설정
                    m_pAssistDeviceTool.SetSEMControlFocusOLValue(m_IntegratedData.nWDOLCoarse[m_IntegratedData.nWDStepNumber], SEM_FOCUS_FINE_STR);    // USB DEVICE 적용
                }
            }
            // WD에 따른 배율보정을 위한 WD(Z-AXIS) 선택값
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_WD_DATA_ZAXIS)
            {
                // 새로운 WD(Z-AXIS) 선택값 설정
                m_IntegratedData.nWDStepNumber = RequestSetUsbDeviceData.nRequestData[0];

                // 2021.06.03
                // m_pIntegrated 포인터에 새로운 WD SETUP 데이터 저장
                if (m_dev.AfxGetIntegratedPointer() != null)
                {
                    // WD SETUP : Z-AXIS POSITION
                    m_dev.AfxGetIntegratedPointer().nWDStepNumber = m_IntegratedData.nWDStepNumber;

                    // 2021.07.27
                    // WD SETUP에서 Z-AXIS 값을 변경 했기 때문에 Z-AXIS에 해당하는 원본 데이터를 가져온다
                    // WD SETUP : FOCUS(OL) COARSE VALUE
                    m_IntegratedData.nWDOLCoarse[m_IntegratedData.nWDStepNumber]  = m_dev.AfxGetIntegratedPointer().nWDOLCoarse[m_IntegratedData.nWDStepNumber];
                    // WD SETUP : SCAN ROTATION VALUE
                    m_IntegratedData.nWDSRotation[m_IntegratedData.nWDStepNumber] = m_dev.AfxGetIntegratedPointer().nWDSRotation[m_IntegratedData.nWDStepNumber];
                    // WD SETUP : ZOOM VALUE
                    m_IntegratedData.nWDZooming[m_IntegratedData.nWDStepNumber]   = m_dev.AfxGetIntegratedPointer().nWDZooming[m_IntegratedData.nWDStepNumber];
                    // WD SETUP : MIN-MAG VALUE
                    m_IntegratedData.nWDMinMag[m_IntegratedData.nWDStepNumber]    = m_dev.AfxGetIntegratedPointer().nWDMinMag[m_IntegratedData.nWDStepNumber];
                }

                int nZAxis = m_IntegratedData.nWDStepNumber;

                // WD : Z-AXIS, OL, SROT, ZOOM, MINMAG, MAX
                // SET WD CALIBRATION DATA : WD(Z-AXIS), WD-OL, S-ROT, ZOOM, MIN-MAG, WD MAX
                m_wndScanCalibrationToolDlg.SetWDPositionCalibrationData(nZAxis, m_IntegratedData.nWDOLCoarse[nZAxis], m_IntegratedData.nWDSRotation[nZAxis], m_IntegratedData.nWDZooming[nZAxis], m_IntegratedData.nWDMinMag[nZAxis], m_IntegratedData.nWDMaxValue);

                m_dev.AfxThreadWaitDialog(this.GetSafeHwnd());

            }
            // WD에 따른 배율보정을 위한 OL 설정값
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_WD_DATA_OL)
            {
                int nZAxis = m_IntegratedData.nWDStepNumber;
                // 새로운 OL 설정값
                m_IntegratedData.nWDOLCoarse[nZAxis] = RequestSetUsbDeviceData.nRequestData[0];

                // 2021.06.03
                // m_pIntegrated 포인터에 새로운 WD SETUP 데이터 저장
                if (m_dev.AfxGetIntegratedPointer() != null)
                {
                    // WD SETUP : Z-AXIS POSITION
                    m_dev.AfxGetIntegratedPointer().nWDStepNumber = nZAxis;
                    // WD SETUP : FOCUS(OL) COARSE VALUE
                    m_dev.AfxGetIntegratedPointer().nWDOLCoarse[nZAxis] = m_IntegratedData.nWDOLCoarse[nZAxis];
                }

                if (m_IntegratedData.bWDSetupFlag == TRUE)
                {
                    // SET WD CALIBRATION DATA : WD-OL
                    m_wndScanCalibrationToolDlg.SetWDPositionOLCalibrationData(m_IntegratedData.nWDOLCoarse[nZAxis], 1);
                    // OL값 재설정
                    m_wndFocusControlDlg.SetFocusCoarseFineValue(m_IntegratedData.nWDOLCoarse[nZAxis], SEM_FOCUS_FINE_STR);   // 위치값 재설정
                }
                else
                {
                    // SET WD CALIBRATION DATA : WD-OL
                    m_wndScanCalibrationToolDlg.SetWDPositionOLCalibrationData(m_IntegratedData.nWDOLCoarse[nZAxis], 0);
                }

                //AfxThreadWaitDialog(this->GetSafeHwnd());
            }
            // WD에 따른 배율보정을 위한 S-ROT 설정값
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_WD_DATA_SROT)
            {
                int nZAxis = m_IntegratedData.nWDStepNumber;
                // 새로운 S-ROT 설정값
                m_IntegratedData.nWDSRotation[nZAxis] = RequestSetUsbDeviceData.nRequestData[0];

                // 2021.06.03
                // m_pIntegrated 포인터에 새로운 WD SETUP 데이터 저장
                if (m_dev.AfxGetIntegratedPointer() != null)
                {
                    // WD SETUP : Z-AXIS POSITION
                    m_dev.AfxGetIntegratedPointer().nWDStepNumber = nZAxis;
                    // WD SETUP : SCAN ROTATION VALUE
                    m_dev.AfxGetIntegratedPointer().nWDSRotation[nZAxis] = m_IntegratedData.nWDSRotation[nZAxis];
                }

                if (m_IntegratedData.bWDSetupFlag == TRUE)
                {
                    // SET WD CALIBRATION DATA : S-ROT
                    m_wndScanCalibrationToolDlg.SetWDPositionSRotCalibrationData(m_IntegratedData.nWDSRotation[nZAxis], 1);
                    // S-ROT값 재설정
                    // 2020.05.04 추가 작업 필요함
                    //m_wndAlignControlDlg.SetScanRotationValue(m_IntegratedData.nWDSRotation[nZAxis]);   // 위치값 재설정
                }
                else
                {
                    // SET WD CALIBRATION DATA : S-ROT
                    m_wndScanCalibrationToolDlg.SetWDPositionSRotCalibrationData(m_IntegratedData.nWDSRotation[nZAxis], 0);
                }

                //AfxThreadWaitDialog(this->GetSafeHwnd());
            }
            // WD에 따른 배율보정을 위한 ZOOM 설정값
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_WD_DATA_ZOOM)
            {
                int nZAxis = m_IntegratedData.nWDStepNumber;
                // 새로운 ZOOM 설정값
                m_IntegratedData.nWDZooming[nZAxis] = RequestSetUsbDeviceData.nRequestData[0];

                // 2021.06.03
                // m_pIntegrated 포인터에 새로운 WD SETUP 데이터 저장
                if (m_dev.AfxGetIntegratedPointer() != null)
                {
                    // WD SETUP : Z-AXIS POSITION
                    m_dev.AfxGetIntegratedPointer().nWDStepNumber = nZAxis;
                    // WD SETUP : ZOOM VALUE
                    m_dev.AfxGetIntegratedPointer().nWDZooming[nZAxis] = m_IntegratedData.nWDZooming[nZAxis];
                }

                if (m_IntegratedData.bWDSetupFlag == TRUE)
                {
                    // SET WD CALIBRATION DATA : ZOOM
                    m_wndScanCalibrationToolDlg.SetWDPositionZoomingCalibrationData(m_IntegratedData.nWDZooming[nZAxis], 1);
                }
                else
                {
                    // SET WD CALIBRATION DATA : ZOOM
                    m_wndScanCalibrationToolDlg.SetWDPositionZoomingCalibrationData(m_IntegratedData.nWDZooming[nZAxis], 0);
                }

                //AfxThreadWaitDialog(this->GetSafeHwnd());
            }
            // WD에 따른 배율보정을 위한 MIN-MAG 설정값
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_WD_DATA_MINMAG)
            {
                int nZAxis = m_IntegratedData.nWDStepNumber;
                // 새로운 MIN-MAG 설정값
                m_IntegratedData.nWDMinMag[nZAxis] = RequestSetUsbDeviceData.nRequestData[0];

                // 2021.06.03
                // m_pIntegrated 포인터에 새로운 WD SETUP 데이터 저장
                if (m_dev.AfxGetIntegratedPointer() != null)
                {
                    // WD SETUP : Z-AXIS POSITION
                    m_dev.AfxGetIntegratedPointer().nWDStepNumber = nZAxis;
                    // WD SETUP : MIN-MAG VALUE
                    m_dev.AfxGetIntegratedPointer().nWDMinMag[nZAxis] = m_IntegratedData.nWDMinMag[nZAxis];
                }

                if (m_IntegratedData.bWDSetupFlag == TRUE)
                {
                    // SET WD CALIBRATION DATA : MIN-MAG
                    m_wndScanCalibrationToolDlg.SetWDPositionMinMagCalibrationData(m_IntegratedData.nWDMinMag[nZAxis], 1);
                }
                else
                {
                    // SET WD CALIBRATION DATA : MIN-MAG
                    m_wndScanCalibrationToolDlg.SetWDPositionMinMagCalibrationData(m_IntegratedData.nWDMinMag[nZAxis], 0);
                }
            }
            // WD에 따른 배율보정을 위한 WD-MAX 설정값
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_WD_DATA_MAX)
            {
                // 새로운 WD-MAX 설정값
                m_IntegratedData.nWDStepMax = RequestSetUsbDeviceData.nRequestData[0];

                // 2021.06.03
                // m_pIntegrated 포인터에 새로운 WD SETUP 데이터 저장
                if (m_dev.AfxGetIntegratedPointer() != null)
                {
                    // WD SETUP : MAX VALUE
                    m_dev.AfxGetIntegratedPointer().nWDStepMax = m_IntegratedData.nWDStepMax;
                }

                if (m_IntegratedData.bWDSetupFlag == TRUE)
                {
                    // SET WD CALIBRATION DATA : WD MAX
                    m_wndScanCalibrationToolDlg.SetWDPositionWDMaxCalibrationData(m_IntegratedData.nWDStepMax, 1);
                }
                else
                {
                    // SET WD CALIBRATION DATA : WD MAX
                    m_wndScanCalibrationToolDlg.SetWDPositionWDMaxCalibrationData(m_IntegratedData.nWDStepMax, 0);
                }
            }
            // WD에 따른 배율보정을 위한 설정값 저장
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_WD_DATA_APPLY)
            {
                // 새로운 WD 자동저장 선택값 설정
                m_IntegratedData.bWDSetupFlag         = RequestSetUsbDeviceData.nRequestData[0];
                // 새로운 WD(Z-AXIS) 선택값 설정
                m_IntegratedData.nWDStepNumber        = RequestSetUsbDeviceData.nRequestData[1];
                int nZAxis = m_IntegratedData.nWDStepNumber;
                // 새로운 OL 설정값
                m_IntegratedData.nWDOLCoarse[nZAxis]  = RequestSetUsbDeviceData.nRequestData[2];
                // 새로운 S-ROT 설정값
                m_IntegratedData.nWDSRotation[nZAxis] = RequestSetUsbDeviceData.nRequestData[3];
                // 새로운 ZOOM 설정값
                m_IntegratedData.nWDZooming[nZAxis]   = RequestSetUsbDeviceData.nRequestData[4];
                // 새로운 MIN-MAG 설정값
                m_IntegratedData.nWDMinMag[nZAxis]    = RequestSetUsbDeviceData.nRequestData[5];
                // 새로운 WD-MAX 설정값
                m_IntegratedData.nWDStepMax           = RequestSetUsbDeviceData.nRequestData[6];

                // 2021.08.02
                // WD(Z-AXIS) 40[mm] 이상은 APPLY 버튼을 실행 했을경우, 현재 WD값 하위(예:40일경우 41~59) 모든값 현재 WD값의
                // 아이템(OL,S-ROT,ZOOM,MIN-MAG) 값으로 동일하게 설정한다
                int nWDDefaultAll = RequestSetUsbDeviceData.nRequestData[7];

                // 2021.08.02
                // WD : SETUP ON 상태, 선택 Z-AXIS(WD) 값 -> 4[mm] 인 경우, 0[mm]~3[mm]의 SETUP 값은 모두 기본값으로 설정한다.
                if (m_IntegratedData.bWDSetupFlag == TRUE && m_IntegratedData.nWDStepNumber == 4)
                {
                    // 0~3[mm] OL      설정값
                    m_IntegratedData.nWDOLCoarse[3]  = m_IntegratedData.nWDOLCoarse[m_IntegratedData.nWDStepNumber] + 100; // 3[mm] : 4[mm]+100
                    m_IntegratedData.nWDOLCoarse[2]  = m_IntegratedData.nWDOLCoarse[m_IntegratedData.nWDStepNumber] + 200; // 2[mm] : 4[mm]+200
                    m_IntegratedData.nWDOLCoarse[1]  = m_IntegratedData.nWDOLCoarse[m_IntegratedData.nWDStepNumber] + 300; // 1[mm] : 4[mm]+300
                    m_IntegratedData.nWDOLCoarse[0]  = m_IntegratedData.nWDOLCoarse[m_IntegratedData.nWDStepNumber] + 400; // 0[mm] : 4[mm]+400
                    // 0~3[mm] S-ROT   설정값
                    m_IntegratedData.nWDSRotation[3] = 0;    // 3[mm] : S-ROT -> 0[deg]
                    m_IntegratedData.nWDSRotation[2] = 0;    // 2[mm] : S-ROT -> 0[deg]
                    m_IntegratedData.nWDSRotation[1] = 0;    // 1[mm] : S-ROT -> 0[deg]
                    m_IntegratedData.nWDSRotation[0] = 0;    // 0[mm] : S-ROT -> 0[deg]
                    // 0~3[mm] ZOOM    설정값
                    m_IntegratedData.nWDZooming[3]   = 1000; // 3[mm] : ZOOM -> 1000
                    m_IntegratedData.nWDZooming[2]   = 1000; // 2[mm] : ZOOM -> 1000
                    m_IntegratedData.nWDZooming[1]   = 1000; // 1[mm] : ZOOM -> 1000
                    m_IntegratedData.nWDZooming[0]   = 1000; // 0[mm] : ZOOM -> 1000
                    // 0~3[mm] MIN-MAG 설정값
                    m_IntegratedData.nWDMinMag[3]    = 20;   // 3[mm] : MIN-MAG -> 20
                    m_IntegratedData.nWDMinMag[2]    = 20;   // 3[mm] : MIN-MAG -> 20
                    m_IntegratedData.nWDMinMag[1]    = 20;   // 3[mm] : MIN-MAG -> 20
                    m_IntegratedData.nWDMinMag[0]    = 20;   // 3[mm] : MIN-MAG -> 20
                }
                // 2021.08.02
                // WD : SETUP ON 상태, 선택 Z-AXIS(WD) 값 -> 45[mm] 인 경우, 46[mm]~59[mm]의 SETUP 값은 모두 기본값으로 설정한다.
                else if (m_IntegratedData.bWDSetupFlag == TRUE && nWDDefaultAll == 1)
                {
                    int k = m_IntegratedData.nWDStepNumber+1;
                    int n = 0;
                    // 46~59[mm] OL      설정값
                    for (n = k; n <= 59; n++)
                    {
                        // 46[mm]~59[mm] : 45[mm]값 적용
                        m_IntegratedData.nWDOLCoarse[n] = m_IntegratedData.nWDOLCoarse[m_IntegratedData.nWDStepNumber];
                    }
                    // 46~59[mm] S-ROT   설정값
                    for (n = k; n <= 59; n++)
                    {
                        // 46[mm]~59[mm] : 45[mm]값 적용
                        m_IntegratedData.nWDSRotation[n] = m_IntegratedData.nWDSRotation[m_IntegratedData.nWDStepNumber];
                    }
                    // 46~59[mm] ZOOM    설정값
                    for (n = k; n <= 59; n++)
                    {
                        // 46[mm]~59[mm] : 45[mm]값 적용
                        m_IntegratedData.nWDZooming[n] = m_IntegratedData.nWDZooming[m_IntegratedData.nWDStepNumber];
                    }
                    // 46~59[mm] MIN-MAG 설정값
                    for (n = k; n <= 59; n++)
                    {
                        // 46[mm]~59[mm] : 45[mm]값 적용
                        m_IntegratedData.nWDMinMag[n] = m_IntegratedData.nWDMinMag[m_IntegratedData.nWDStepNumber];
                    }
                }

                // 2021.06.03
                // m_pIntegrated 포인터에 새로운 WD SETUP 데이터 저장
                if (m_dev.AfxGetIntegratedPointer() != null)
                {
                    // WD SETUP FLAG
                    m_dev.AfxGetIntegratedPointer().bWDSetupFlag  = m_IntegratedData.bWDSetupFlag;
                    // WD SETUP : Z-AXIS POSITION
                    m_dev.AfxGetIntegratedPointer().nWDStepNumber = m_IntegratedData.nWDStepNumber;
                    // WD SETUP : MAX VALUE
                    m_dev.AfxGetIntegratedPointer().nWDStepMax    = m_IntegratedData.nWDStepMax;
                    // WD SETUP : FOCUS(OL) COARSE VALUE
                    m_dev.AfxGetIntegratedPointer().nWDOLCoarse[m_IntegratedData.nWDStepNumber]  = m_IntegratedData.nWDOLCoarse[m_IntegratedData.nWDStepNumber];
                    // WD SETUP : SCAN ROTATION VALUE
                    m_dev.AfxGetIntegratedPointer().nWDSRotation[m_IntegratedData.nWDStepNumber] = m_IntegratedData.nWDSRotation[m_IntegratedData.nWDStepNumber];
                    // WD SETUP : ZOOM VALUE
                    m_dev.AfxGetIntegratedPointer().nWDZooming[m_IntegratedData.nWDStepNumber]   = m_IntegratedData.nWDZooming[m_IntegratedData.nWDStepNumber];
                    // WD SETUP : MIN-MAG VALUE
                    m_dev.AfxGetIntegratedPointer().nWDMinMag[m_IntegratedData.nWDStepNumber]    = m_IntegratedData.nWDMinMag[m_IntegratedData.nWDStepNumber];

                    // 2021.08.02
                    // WD : SETUP ON 상태, 선택 Z-AXIS(WD) 값 -> 4[mm] 인 경우, 0[mm]~3[mm]의 SETUP 값은 모두 기본값으로 설정한다.
                    if (m_dev.AfxGetIntegratedPointer().bWDSetupFlag == TRUE && m_dev.AfxGetIntegratedPointer().nWDStepNumber == 4)
                    {
                        // 0~3[mm] OL      설정값
                        m_dev.AfxGetIntegratedPointer().nWDOLCoarse[3]  = m_IntegratedData.nWDOLCoarse[3];     // 3[mm] : 4[mm] OL + 100
                        m_dev.AfxGetIntegratedPointer().nWDOLCoarse[2]  = m_IntegratedData.nWDOLCoarse[2];     // 2[mm] : 4[mm] OL + 200
                        m_dev.AfxGetIntegratedPointer().nWDOLCoarse[1]  = m_IntegratedData.nWDOLCoarse[1];     // 1[mm] : 4[mm] OL + 300
                        m_dev.AfxGetIntegratedPointer().nWDOLCoarse[0]  = m_IntegratedData.nWDOLCoarse[0];     // 0[mm] : 4[mm] OL + 400
                        // 0~3[mm] S-ROT   설정값
                        m_dev.AfxGetIntegratedPointer().nWDSRotation[3] = m_IntegratedData.nWDSRotation[3];    // 3[mm] : S-ROT -> 0[deg]
                        m_dev.AfxGetIntegratedPointer().nWDSRotation[2] = m_IntegratedData.nWDSRotation[2];    // 2[mm] : S-ROT -> 0[deg]
                        m_dev.AfxGetIntegratedPointer().nWDSRotation[1] = m_IntegratedData.nWDSRotation[1];    // 1[mm] : S-ROT -> 0[deg]
                        m_dev.AfxGetIntegratedPointer().nWDSRotation[0] = m_IntegratedData.nWDSRotation[0];    // 0[mm] : S-ROT -> 0[deg]
                        // 0~3[mm] ZOOM    설정값
                        m_dev.AfxGetIntegratedPointer().nWDZooming[3]   = m_IntegratedData.nWDZooming[3];      // 3[mm] : ZOOM -> 1000
                        m_dev.AfxGetIntegratedPointer().nWDZooming[2]   = m_IntegratedData.nWDZooming[2];      // 2[mm] : ZOOM -> 1000
                        m_dev.AfxGetIntegratedPointer().nWDZooming[1]   = m_IntegratedData.nWDZooming[1];      // 1[mm] : ZOOM -> 1000
                        m_dev.AfxGetIntegratedPointer().nWDZooming[0]   = m_IntegratedData.nWDZooming[0];      // 0[mm] : ZOOM -> 1000
                        // 0~3[mm] MIN-MAG 설정값
                        m_dev.AfxGetIntegratedPointer().nWDMinMag[3]    = m_IntegratedData.nWDMinMag[3];       // 3[mm] : MIN-MAG -> 20
                        m_dev.AfxGetIntegratedPointer().nWDMinMag[2]    = m_IntegratedData.nWDMinMag[2];       // 3[mm] : MIN-MAG -> 20
                        m_dev.AfxGetIntegratedPointer().nWDMinMag[1]    = m_IntegratedData.nWDMinMag[1];       // 3[mm] : MIN-MAG -> 20
                        m_dev.AfxGetIntegratedPointer().nWDMinMag[0]    = m_IntegratedData.nWDMinMag[0];       // 3[mm] : MIN-MAG -> 20
                    }
                    // 2021.08.02
                    // WD : SETUP ON 상태, 선택 Z-AXIS(WD) 값 -> 45[mm] 인 경우, 46[mm]~59[mm]의 SETUP 값은 모두 기본값으로 설정한다.
                    else if (m_IntegratedData.bWDSetupFlag == TRUE && nWDDefaultAll != 0)
                    {
                        int k = m_IntegratedData.nWDStepNumber+1;
                        int n = 0;
                        // 46~59[mm] OL      설정값
                        for (n = k; n <= 59; n++)
                        {
                            // 46[mm]~59[mm] : 45[mm]값 적용
                            m_dev.AfxGetIntegratedPointer().nWDOLCoarse[n] = m_IntegratedData.nWDOLCoarse[n];
                        }
                        // 46~59[mm] S-ROT   설정값
                        for (n = k; n <= 59; n++)
                        {
                            // 46[mm]~59[mm] : 45[mm]값 적용
                            m_dev.AfxGetIntegratedPointer().nWDSRotation[n] = m_IntegratedData.nWDSRotation[n];
                        }
                        // 46~59[mm] ZOOM    설정값
                        for (n = k; n <= 59; n++)
                        {
                            // 46[mm]~59[mm] : 45[mm]값 적용
                            m_dev.AfxGetIntegratedPointer().nWDZooming[n] = m_IntegratedData.nWDZooming[n];
                        }
                        // 46~59[mm] MIN-MAG 설정값
                        for (n = k; n <= 59; n++)
                        {
                            // 46[mm]~59[mm] : 45[mm]값 적용
                            m_dev.AfxGetIntegratedPointer().nWDMinMag[n] = m_IntegratedData.nWDMinMag[n];
                        }
                    }
                }

                // WD : Z-AXIS, OL, SROT, ZOOM, MINMAG, MAX
                int nExecute = 0;
                if (m_IntegratedData.bWDSetupFlag == TRUE) nExecute = 1;

                // SET WD CALIBRATION DATA : WD(Z-AXIS), WD-OL, S-ROT, ZOOM, MIN-MAG, WD MAX
                // 10등분 선형화 작업 및 FOCUS COARSE 실행
                m_wndScanCalibrationToolDlg.SetWDPositionCalibrationData(nZAxis, m_IntegratedData.nWDOLCoarse[nZAxis], m_IntegratedData.nWDSRotation[nZAxis], m_IntegratedData.nWDZooming[nZAxis], m_IntegratedData.nWDMinMag[nZAxis], m_IntegratedData.nWDMaxValue);

                // 2021.08.02 : 10등분 선형화 작업 후, 실행한다
                // 2021.07.27 : 변경된 FOCUS COARSE 값을 적용하여 실행한다
                // Z-AXIS(WD)에 해당하는 FOCUS COARSE DEGAUSSING 실행
                // WD 값이 변동이 있으면 WD에 따른 FOCUS DEGAUSSING 및 FOCUS COARSE 실행한다
                m_dev.AfxIntegratedFocusDegaussing(nZAxis * 10);

                //AfxThreadWaitDialog(this->GetSafeHwnd());
            }


            ///
            // WD에 따른 배율보정 설정값
            ///

            m_pAssistDeviceTool.SetUsbDeviceHWSetupWDSetupDataCalibrationOptionValue(RequestSetUsbDeviceData);

            ///
            ///
            ///

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:20580 ----
        // CLIENT --> DEVICE : SET USB DEVICE DATA ( HW SETUP : CL SETUP DATA CALIBRATION )
        public int OnSetUsbDeviceHWSetupCLSetupDataCalibration(int wParam, object lParam)
        {
            // 2020.05.01
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : HW SETUP XY-AMP,OFFSET
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // SPOT-SIZE 
            if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_CL_DATA_SPOTSIZE)
            {
                // 새로운 SPOT-SIZE 선택값 설정
                m_IntegratedData.nCLSpotSize = RequestSetUsbDeviceData.nRequestData[0];
                // BEAM CONTROL DLG : SPOT SIZE 설정
                m_wndBeamControlDlg.SetSpotSizeIntegratedItem(m_IntegratedData.nCLSpotSize); //m_IntegratedData.nCLSpotSize - 1);

                // SCAN CALIBRATION : SPOT-SIZE에 따른 CL1, CL2
                m_wndScanCalibrationToolDlg.SetSpotSizeCLCalibrationData(m_IntegratedData.nCLSpotSize, m_IntegratedData.wCL1List[m_IntegratedData.nCLSpotSize - 1], m_IntegratedData.wCL2List[m_IntegratedData.nCLSpotSize - 1], 0);
            }

            ///
            // SPOT-SIZE 선택값
            ///

            m_pAssistDeviceTool.SetUsbDeviceHWSetupCLSpotSizeOptionValue(RequestSetUsbDeviceData);

            ///
            ///
            ///

            return 1;
        }

    }
}
