// 원본 USB_FT232H_Device.cpp:7702-9871 을 NanoImpact.CodeGen translate로 옮긴 뒤 손으로 고친 코드.
#pragma warning disable IDE1006, IDE0059, CS0162, CS0168, CS0219
using System;
using NanoImpact.Core.Native;
using static NanoImpact.Core.Def;
using static NanoImpact.Core.Native.CRuntime;

namespace NanoImpact.Hardware.Device
{
    public sealed partial class SemDevice
    {
        // ---- USB_FT232H_Device.cpp:7702 ----
        //
        // Scan Bridge : "RP","SP" -> Motor 또는 Valve Current Position 읽기 또는 쓰기 (모터 또는 밸브 현 위치값 수신 및 적용)
        //
        public int AfxScanBridgeRPCommand(int nWriteFlag, int nAxis, string pszData)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Axis Position 설정 [mm],[deg]
            CharBuffer szCNCData = new CharBuffer(255 + 1);
            ZeroMem(szCNCData);

            // 위치값
            double dPos_mm = 0.0;
            int nPulse = 0;

            // 정보값 저장하기 
            if (nWriteFlag == 1)
            {
                dPos_mm = atof(pszData);

                // SP Command
                switch (nAxis)
                {
                case IDX_X_AXIS:
                {
                    // X축 위치값[mm] 설정하기
                    nPulse = (int)(dPos_mm * m_pIntegrated.dAxisPulsePerUnit[nAxis]);
                    sprintf(szCNCData, "SP%d,%d;", nAxis, nPulse);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case IDX_Y_AXIS:
                {
                    // Y축 위치값[mm] 설정하기
                    nPulse = (int)(dPos_mm * m_pIntegrated.dAxisPulsePerUnit[nAxis]);
                    sprintf(szCNCData, "SP%d,%d;", nAxis, nPulse);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case IDX_Z_AXIS:
                {
                    // Z축 위치값[mm] 설정하기
                    nPulse = (int)(dPos_mm * m_pIntegrated.dAxisPulsePerUnit[nAxis]);
                    sprintf(szCNCData, "SP%d,%d;", nAxis, nPulse);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case IDX_R_AXIS:
                {
                    // R축 위치값[mm] 설정하기
                    nPulse = (int)(dPos_mm * m_pIntegrated.dAxisPulsePerUnit[nAxis]);
                    sprintf(szCNCData, "SP%d,%d;", nAxis, nPulse);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case IDX_T_AXIS:
                {
                    // T축 위치값[mm] 설정하기
                    nPulse = (int)(dPos_mm * m_pIntegrated.dAxisPulsePerUnit[nAxis]);
                    sprintf(szCNCData, "SP%d,%d;", nAxis, nPulse);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case IDX_B_AXIS:
                {
                    // 3축 STAGE
                    if (m_pIntegrated.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_ALL_DATA_VIEW] == FALSE)
                    {
                        // BSE축 위치값[mm] 설정하기
                        int nBseAxis = IDX_T_AXIS;
                        nPulse = (int)(dPos_mm * m_pIntegrated.dAxisPulsePerUnit[nBseAxis]);
                        sprintf(szCNCData, "SP%d,%d;", nBseAxis, nPulse);
                        // VacBoard -> PC -> To CncBoard
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    }
                    // 5축 STAGE
                    else
                    {
                        // BSE축 위치값[mm] 설정하기
                        int nBseAxis = IDX_B_AXIS;
                        nPulse = (int)(dPos_mm * m_pIntegrated.dAxisPulsePerUnit[nBseAxis]);
                        sprintf(szCNCData, "SP%d,%d;", nBseAxis, nPulse);
                        // VacBoard -> PC -> To CncBoard
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    }
                    break;
                }
                }
            }
            else
            {
                // RP Command
                switch (nAxis)
                {
                case IDX_X_AXIS:
                {
                    // X축 위치값[mm] 표시하기 
                    sprintf(szCNCData, "RP%d;", nAxis);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case IDX_Y_AXIS:
                {
                    // Y축 위치값[mm] 표시하기 
                    sprintf(szCNCData, "RP%d;", nAxis);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case IDX_Z_AXIS:
                {
                    // Z축 위치값[mm] 표시하기 
                    sprintf(szCNCData, "RP%d;", nAxis);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case IDX_R_AXIS:
                {
                    // R축 위치값[mm] 표시하기 
                    sprintf(szCNCData, "RP%d;", nAxis);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case IDX_T_AXIS:
                {
                    // T축 위치값[mm] 표시하기 
                    sprintf(szCNCData, "RP%d;", nAxis);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case IDX_B_AXIS:
                {
                    // 3축 STAGE
                    if (m_pIntegrated.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_ALL_DATA_VIEW] == FALSE)
                    {
                        // BSE축 위치값[mm] 표시하기 
                        int nBseAxis = IDX_T_AXIS;
                        sprintf(szCNCData, "RP%d;", nBseAxis);
                        // VacBoard -> PC -> To CncBoard
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    }
                    // 5축 STAGE
                    else
                    {
                        // BSE축 위치값[mm] 표시하기 
                        int nBseAxis = IDX_B_AXIS;
                        sprintf(szCNCData, "RP%d;", nBseAxis);
                        // VacBoard -> PC -> To CncBoard
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    }
                    break;
                }
                }
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:7869 ----
        //
        // Scan Bridge : "RM" -> Read Maximum Width (모터 축 최대 이동거리[mm] 또는 이동각도[deg])
        //
        public int AfxScanBridgeRMCommand()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // 모터 축(Axis) 계산
            int nAxis = (int)(m_pIntegrated.szXStr[0] - (byte)'0');

            if (nAxis == IDX_T_AXIS+1)
            {
                // Tilt 0도 의 실제 각도 값을 별도로 저장한다
                m_pIntegrated.nTiltZeroAnglePulsePos = atoi(m_pIntegrated.szYStr);
            }
            else
            {
                if (nAxis >= IDX_X_AXIS && nAxis <= IDX_T_AXIS)
                {
                    // 모터 각 축의 최대이동거리를 Pulse 갯수 값으로 받는다
                    m_pIntegrated.nAxisMaxWidthPulse[nAxis] = atoi(m_pIntegrated.szYStr);
                    // 모터 각 축의 최대이동거리를 [mm] 또는 각도[deg]로 환산한다
                    m_pIntegrated.dAxisMaxWidthMM[nAxis] = (double)m_pIntegrated.nAxisMaxWidthPulse[nAxis] * (double)m_pIntegrated.dAxisMMPerPulse[nAxis];
                }
                else
                {
                    return RET_SUCCESS_OK;
                }
            }

            CharBuffer szStr = new CharBuffer(MAX_AXIS_STR_DISPLAY_BUFFER + 1);
            CharBuffer szDisplay = new CharBuffer(MAX_AXIS_STR_DISPLAY_BUFFER + 1);

            ZeroMem(szStr);
            ZeroMem(szDisplay);

            int nDisplay = 0;
            double dDisplay = 0.0;

            // 메시지 처리 전달용 USB Data
            USB_DEVICE_DATA_INFO UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            int bMessage = FALSE;

            // 모터 최대이동거리 또는 각도 표시형식(소수점자리수) 설정
            sprintf(szStr, "%3.1f", m_pIntegrated.dAxisMaxWidthMM[nAxis]);

            // 모터 최대 이동거리 또는 각도 표시
            switch (nAxis)
            {
                case IDX_X_AXIS:
                {
                    // X축 최대이동거리[mm] 표시
                    ZeroMem(szDisplay);
                    sprintf(szDisplay, "%s", szStr);
                    bMessage = TRUE;
                    break;
                }
                case IDX_Y_AXIS:
                {
                    // Y축 최대이동거리[mm] 표시
                    ZeroMem(szDisplay);
                    sprintf(szDisplay, "%s", szStr);
                    bMessage = TRUE;
                    break;
                }
                case IDX_Z_AXIS:
                {
                    // Z축(WD) 최대이동거리[mm] 표시
                    ZeroMem(szDisplay);
                    sprintf(szDisplay, "%s", szStr);
                    bMessage = TRUE;
                    break;
                }
                case IDX_R_AXIS:
                {
                    // R축은 최대이동거리 [각도] 값을 적용하지 않는다 (360도 회전 하기 때문)
                    bMessage = FALSE;
                    break;
                }
                case IDX_T_AXIS: // Read Tilt Max
                {
                    // T축 최대이동거리 [각도] 표시
                    // T축의 실제 최대이동거리 [각도]는 T축의 최대각도일때[Pulse]값  - 0각도일때[Pulse]값 의 이동거리[각도] 환산값이다
                    ZeroMem(szDisplay);
                    sprintf(szDisplay, "%s", szStr);
                    bMessage = TRUE;
                    break;
                }
                case IDX_T_AXIS+1: // Read Tilt TiltZeroAnglePulsePos
                {
                    // Tilt 각도가 0도 일 경우 실제위치값 (예: 틸트0도 -> 실제틸트값-1.5도)
                    // Tilt 0각도 실제각도 계산 및 표시
                    ZeroMem(szDisplay);
                    dDisplay = (double)((double)m_pIntegrated.nTiltZeroAnglePulsePos * (double)m_pIntegrated.dAxisMMPerPulse[IDX_T_AXIS]);
                    m_pIntegrated.dTiltZeroAnglePosMM = dDisplay;
                    sprintf(szDisplay, "%g", dDisplay);
                    bMessage = TRUE;
                    break;
                }
            }

            // CNC 상태 및 제어용 윈도우 핸들 확인
            if (m_hStageSetupControlWnd == NULL) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // CNC Command 설정 : 'RM' Command
            UsbDeviceDataInfo.nCommand = SBR_CMD_RM;
            // Axis 설정
            UsbDeviceDataInfo.nState = nAxis;
            // 수신된 Data String : Axis Max 이동거리[mm] 또는 각도[deg]
            strcpy_s(UsbDeviceDataInfo.szData, szDisplay);

            // 2020.04.28
            //*
            // Axis Max Width 정보 전송
            if (bMessage == TRUE)
            {
                if (m_hOwnerWnd != NULL)
                {
                    // DEVICE MAIN SERVER
                    // axis max mm/step 값 정보 메시지 보내기
                    SendMessage(m_hOwnerWnd, UM_USB_SBR_STAGE_MAX_MM_STEP, nAxis, UsbDeviceDataInfo);
                }
            }
            //*/

            // 계산된 값 표시하기
            switch (nAxis)
            {
            case IDX_X_AXIS:
            {
                // X축 최대 이동거리[mm] 값 표시하기 
                SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, nAxis, UsbDeviceDataInfo);
                break;
            }
            case IDX_Y_AXIS:
            {
                // Y축 최대 이동거리[mm] 값 표시하기 
                SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, nAxis, UsbDeviceDataInfo);
                break;
            }
            case IDX_Z_AXIS:
            {
                // Z축 최대 이동거리[mm] 값 표시하기 
                SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, nAxis, UsbDeviceDataInfo);
                break;
            }
            case IDX_R_AXIS:
            {
                // R축은 최대이동거리 [각도] 값을 적용하지 않는다 (360도 회전 하기 때문)
                break;
            }
            case IDX_T_AXIS:
            {
                // T축 최대 이동각도[deg] 값 표시하기 
                SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, nAxis, UsbDeviceDataInfo);
                break;
            }
            case IDX_T_AXIS+1:
            {
                // Tilt 0각도 실제각도 계산 및 표시
                SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, nAxis, UsbDeviceDataInfo);
                break;
            }
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:8038 ----
        //
        // Scan Bridge : "RM","SM" -> Read or Write  Max Width([mm],[deg]) 값 수신 또는 송신
        //
        public int AfxScanBridgeRMCommand(int nWriteFlag, int nAxis, string pszData)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // 모터 축 RM 정보
            CharBuffer szCNCData = new CharBuffer(255 + 1);
            CharBuffer szCommand = new CharBuffer(20 + 1);
            ZeroMem(szCNCData);
            ZeroMem(szCommand);

            // 정보값 저장하기 
            if (nWriteFlag == 1)
            {
                // SM Command
                strcpy_s(szCommand, "SM");

                // 계산된 값 표시하기
                switch (nAxis)
                {
                case IDX_X_AXIS:
                {
                    // X축 최대 이동거리[mm] 값 저장하기 
                    double dMax = atof(pszData);
                    int nPulse = (int)(dMax * m_pIntegrated.dAxisPulsePerUnit[nAxis]);
                    m_pIntegrated.nAxisMaxWidthPulse[nAxis] = nPulse;
                    sprintf(szCNCData, "%s%d,%d;", szCommand, nAxis, nPulse);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case IDX_Y_AXIS:
                {
                    // Y축 최대 이동거리[mm] 값 저장하기 
                    double dMax = atof(pszData);
                    int nPulse = (int)(dMax * m_pIntegrated.dAxisPulsePerUnit[nAxis]);
                    m_pIntegrated.nAxisMaxWidthPulse[nAxis] = nPulse;
                    sprintf(szCNCData, "%s%d,%d;", szCommand, nAxis, nPulse);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case IDX_Z_AXIS:
                {
                    // Z축 최대 이동거리[mm] 값 저장하기 
                    double dMax = atof(pszData);
                    int nPulse = (int)(dMax * m_pIntegrated.dAxisPulsePerUnit[nAxis]);
                    m_pIntegrated.nAxisMaxWidthPulse[nAxis] = nPulse;
                    sprintf(szCNCData, "%s%d,%d;", szCommand, nAxis, nPulse);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case IDX_T_AXIS:
                {
                    // T축 최대 이동각도[deg] 값 저장하기 
                    double dMax = atof(pszData);
                    int nPulse = (int)(dMax * m_pIntegrated.dAxisPulsePerUnit[nAxis]);
                    m_pIntegrated.nAxisMaxWidthPulse[nAxis] = nPulse;
                    sprintf(szCNCData, "%s%d,%d;", szCommand, nAxis, nPulse);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case IDX_T_AXIS+1:
                {
                    // T축 0도 각도[deg] 값 저장하기 
                    double dZero = atof(pszData);
                    int nPulse = (int)(dZero * m_pIntegrated.dAxisPulsePerUnit[IDX_T_AXIS]);
                    m_pIntegrated.nTiltZeroAnglePulsePos = nPulse;
                    m_pIntegrated.dTiltZeroAnglePosMM    = dZero;
                    sprintf(szCNCData, "%s%d,%d;", szCommand, nAxis, nPulse);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                }
            }
            else
            {
                // RM Command
                strcpy_s(szCommand, "RM");

                // 계산된 값 표시하기
                switch (nAxis)
                {
                case IDX_X_AXIS:
                {
                    // X축 최대 이동거리[mm] 값 표시하기 
                    sprintf(szCNCData, "%s%d;", szCommand, nAxis);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case IDX_Y_AXIS:
                {
                    // Y축 최대 이동거리[mm] 값 표시하기 
                    sprintf(szCNCData, "%s%d;", szCommand, nAxis);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case IDX_Z_AXIS:
                {
                    // Z축 최대 이동거리[mm] 값 표시하기 
                    sprintf(szCNCData, "%s%d;", szCommand, nAxis);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case IDX_T_AXIS:
                {
                    // T축 최대 이동각도[deg] 값 표시하기 
                    sprintf(szCNCData, "%s%d;", szCommand, nAxis);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case IDX_T_AXIS+1:
                {
                    // T축 0도 각도[deg] 값 표시하기 
                    sprintf(szCNCData, "%s%d;", szCommand, nAxis);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                }
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:8172 ----
        //
        // Scan Bridge : "SM" -> Axis Max Width([mm],[deg]) & Home 이동
        //
        public int AfxScanBridgeSMCommand(int nAxis)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // 모터 축 RM 정보
            CharBuffer szCNCData = new CharBuffer(255 + 1);
            CharBuffer szCommand = new CharBuffer(20 + 1);
            ZeroMem(szCNCData);
            ZeroMem(szCommand);

            // SM Command
            strcpy_s(szCommand, "SM");

            // 계산된 값 표시하기
            switch (nAxis)
            {
            case IDX_X_AXIS:
            {
                // X축 최대 이동거리[mm] 값 위치이동 
                sprintf(szCNCData, "%s%d;", szCommand, nAxis);
                // VacBoard -> PC -> To CncBoard
                AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                break;
            }
            case IDX_Y_AXIS:
            {
                // Y축 최대 이동거리[mm] 값 위치이동 
                sprintf(szCNCData, "%s%d;", szCommand, nAxis);
                // VacBoard -> PC -> To CncBoard
                AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                break;
            }
            case IDX_Z_AXIS:
            {
                // Z축 최대 이동거리[mm] 값 위치이동 
                sprintf(szCNCData, "%s%d;", szCommand, nAxis);
                // VacBoard -> PC -> To CncBoard
                AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                break;
            }
            case IDX_T_AXIS:
            {
                // T축 최대 이동각도[deg] 값 위치이동 
                sprintf(szCNCData, "%s%d;", szCommand, nAxis);
                // VacBoard -> PC -> To CncBoard
                AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                break;
            }
            case IDX_T_AXIS + 1:
            {
                // T축 0도 각도[deg] 값 위치이동 
                sprintf(szCNCData, "%s%d;", szCommand, nAxis);
                // VacBoard -> PC -> To CncBoard
                AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                break;
            }
            case IDX_T_AXIS + 2:
            {
                //int nRem = m_pIntegrated->dCurAxisPos[IDX_Z_AXIS];
                int zmm = (int)m_pIntegrated.dCurAxisPos[IDX_Z_AXIS];
                if (zmm == 10.0)
                {
                    // Z Axis Top Limit Switch Pulse 및 Z-Backlash 값 설정
                    ZeroMem(szCNCData);
                    strcpy_s(szCNCData, "ZT2;");
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);

                    // 설정한 Z Axis Top Limit Switch Pulse 값 읽기
                    ZeroMem(szCNCData);
                    strcpy_s(szCNCData, "ZT1;");
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);

                    // Z Axis Backlash 값 읽기
                    ZeroMem(szCNCData);
                    strcpy_s(szCNCData, "RB2;");
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                }
                else
                {
                    // Z 축을 10[mm]로 설정하세요 -> 메시지박스
                }
                break;
            }
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:8266 ----
        //
        // Scan Bridge : "MH" -> Axis Home Move
        //
        public int AfxScanBridgeMHCommand(int nAxis)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Axis Home Move
            CharBuffer szCNCData = new CharBuffer(255 + 1);
            ZeroMem(szCNCData);

            // 계산된 값 표시하기
            switch (nAxis)
            {
            case IDX_X_AXIS:
            {
                // X축 Home 이동 
                sprintf(szCNCData, "MH%d,1;", nAxis);
                // VacBoard -> PC -> To CncBoard
                AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                break;
            }
            case IDX_Y_AXIS:
            {
                // Y축 Home 이동 
                sprintf(szCNCData, "MH%d,1;", nAxis);
                // VacBoard -> PC -> To CncBoard
                AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                break;
            }
            case IDX_Z_AXIS:
            {
                // Z축 Home 이동 
                sprintf(szCNCData, "MH%d,1;", nAxis);
                // VacBoard -> PC -> To CncBoard
                AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                break;
            }
            case IDX_R_AXIS:
            {
                // R축 Home 이동 
                sprintf(szCNCData, "MH%d,1;", nAxis);
                // VacBoard -> PC -> To CncBoard
                AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                break;
            }
            case IDX_T_AXIS:
            {
                // T축 Home 이동 
                sprintf(szCNCData, "MH%d,1;", nAxis);
                // VacBoard -> PC -> To CncBoard
                AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                break;
            }
            case IDX_B_AXIS:
            {
                // 3축 STAGE
                if (m_pIntegrated.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_ALL_DATA_VIEW] == FALSE)
                {
                    // BSE축 Home 이동
                    int nBseAxis = 5; // HOME일 경우 5를 사용한다.
                    sprintf(szCNCData, "MH%d,1;", nBseAxis);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                }
                // 5축 STAGE
                else
                {
                    // BSE축 Home 이동
                    int nBseAxis = IDX_B_AXIS; // HOME일 경우 5를 사용한다.
                    sprintf(szCNCData, "MH%d,1;", nBseAxis);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                }
                break;
            }
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:8347 ----
        //
        // Scan Bridge : "MM" -> Axis Motion Move
        //
        public int AfxScanBridgeMMCommand(int nAxis, int nPM, string pszData, int nEcho)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Axis Motion Move
            CharBuffer szCNCData = new CharBuffer(255 + 1);
            ZeroMem(szCNCData);

            //int nPM = 0;
            //if (pszData != NULL)
            //{
            //    nPM = atoi(pszData);
            //}

            // Axis Move
            switch (nAxis)
            {
            case IDX_X_AXIS:
            {
                // X축 Motion Move 
                if (nPM < 0) sprintf(szCNCData, "MM%d,-%s,%d;", nAxis, pszData, nEcho);
                else sprintf(szCNCData, "MM%d,%s,%d;", nAxis, pszData, nEcho);
                // VacBoard -> PC -> To CncBoard
                AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                break;
            }
            case IDX_Y_AXIS:
            {
                // Y축 Motion Move 
                if (nPM < 0) sprintf(szCNCData, "MM%d,-%s,%d;", nAxis, pszData, nEcho);
                else sprintf(szCNCData, "MM%d,%s,%d;", nAxis, pszData, nEcho);
                // VacBoard -> PC -> To CncBoard
                AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                break;
            }
            case IDX_Z_AXIS:
            {
                // Z MOVING ECHO ON/OFF
                int nZEcho = (m_pIntegrated.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_Z_MOVE_ECHO] == FALSE) ? 1 : 2;
                // Z축 Motion Move 
                if (nPM < 0) sprintf(szCNCData, "MM%d,-%s,%d;", nAxis, pszData, nZEcho);// nEcho);
                else sprintf(szCNCData, "MM%d,%s,%d;", nAxis, pszData, nZEcho);// nEcho);
                // VacBoard -> PC -> To CncBoard
                AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                break;
            }
            case IDX_R_AXIS:
            {
                // R축 Motion Move 
                if (nPM < 0) sprintf(szCNCData, "MM%d,-%s,%d;", nAxis, pszData, nEcho);
                else sprintf(szCNCData, "MM%d,%s,%d;", nAxis, pszData, nEcho);
                // VacBoard -> PC -> To CncBoard
                AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                break;
            }
            case IDX_T_AXIS:
            {
                // T축 Motion Move 
                if (nPM < 0) sprintf(szCNCData, "MM%d,-%s,%d;", nAxis, pszData, nEcho);
                else sprintf(szCNCData, "MM%d,%s,%d;", nAxis, pszData, nEcho);
                // VacBoard -> PC -> To CncBoard
                AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                break;
            }
            case IDX_B_AXIS:
            {
                // 3축 STAGE
                if (m_pIntegrated.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_ALL_DATA_VIEW] == FALSE)
                {
                    // BSE축 Motion Move : T-AXIS를 이용한다
                    int nBseAxis = IDX_T_AXIS;
                    if (nPM < 0) sprintf(szCNCData, "MM%d,-%s,%d;", nBseAxis, pszData, nEcho);
                    else sprintf(szCNCData, "MM%d,%s,%d;", nBseAxis, pszData, nEcho);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                }
                // 5축 STAGE
                else
                {
                    // BSE축 Motion Move
                    int nBseAxis = IDX_B_AXIS;
                    if (nPM < 0) sprintf(szCNCData, "MM%d,-%s,%d;", nBseAxis, pszData, nEcho);
                    else sprintf(szCNCData, "MM%d,%s,%d;", nBseAxis, pszData, nEcho);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                }
                break;
            }
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:8443 ----
        //
        // Scan Bridge : "MM" -> Axis Motion Move
        //
        public int AfxScanBridgeMMCommand(int nAxis, int nPM, string pszData)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Axis Motion Move
            CharBuffer szCNCData = new CharBuffer(255 + 1);
            ZeroMem(szCNCData);

            //int nPM = 0;
            //if (pszData != NULL)
            //{
            //    nPM = atoi(pszData);
            //}

            // Axis Move
            switch (nAxis)
            {
            case IDX_X_AXIS:
            {
                // X축 Motion Move 
                //if (nPM < 0) sprintf(szCNCData, "MM%d,-%s,1;", nAxis, pszData);
                //else sprintf(szCNCData, "MM%d,%s,1;", nAxis, pszData);
                // 2021.07.14 : MOVING ECHO
                if (nPM < 0) sprintf(szCNCData, "MM%d,-%s,2;", nAxis, pszData);
                else sprintf(szCNCData, "MM%d,%s,2;", nAxis, pszData);
                // VacBoard -> PC -> To CncBoard
                AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                break;
            }
            case IDX_Y_AXIS:
            {
                // Y축 Motion Move 
                //if (nPM < 0) sprintf(szCNCData, "MM%d,-%s,1;", nAxis, pszData);
                //else sprintf(szCNCData, "MM%d,%s,1;", nAxis, pszData);
                // Y축 Motion Move 
                // 2021.07.14 : MOVING ECHO
                if (nPM < 0) sprintf(szCNCData, "MM%d,-%s,2;", nAxis, pszData);
                else sprintf(szCNCData, "MM%d,%s,2;", nAxis, pszData);
                // VacBoard -> PC -> To CncBoard
                AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                break;
            }
            case IDX_Z_AXIS:
            {
                // Z MOVING ECHO ON/OFF
                int nZEcho = (m_pIntegrated.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_Z_MOVE_ECHO] == FALSE) ? 1 : 2;
                // Z축 Motion Move 
                if (nPM < 0) sprintf(szCNCData, "MM%d,-%s,%d;", nAxis, pszData, nZEcho);
                else sprintf(szCNCData, "MM%d,%s,%d;", nAxis, pszData, nZEcho);
                // VacBoard -> PC -> To CncBoard
                AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                break;
            }
            case IDX_R_AXIS:
            {
                // R축 Motion Move 
                //if (nPM < 0) sprintf(szCNCData, "MM%d,-%s,1;", nAxis, pszData);
                //else sprintf(szCNCData, "MM%d,%s,1;", nAxis, pszData);
                // R축 Motion Move 
                // 2021.07.14 : MOVING ECHO
                if (nPM < 0) sprintf(szCNCData, "MM%d,-%s,2;", nAxis, pszData);
                else sprintf(szCNCData, "MM%d,%s,2;", nAxis, pszData);
                // VacBoard -> PC -> To CncBoard
                AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                break;
            }
            case IDX_T_AXIS:
            {
                // T축 Motion Move 
                //if (nPM < 0) sprintf(szCNCData, "MM%d,-%s,1;", nAxis, pszData);
                //else sprintf(szCNCData, "MM%d,%s,1;", nAxis, pszData);
                // T축 Motion Move 
                // 2021.07.14 : MOVING ECHO
                if (nPM < 0) sprintf(szCNCData, "MM%d,-%s,2;", nAxis, pszData);
                else sprintf(szCNCData, "MM%d,%s,2;", nAxis, pszData);
                // VacBoard -> PC -> To CncBoard
                AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                break;
            }
            case IDX_B_AXIS:
            {
                // 3축 STAGE
                if (m_pIntegrated.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_ALL_DATA_VIEW] == FALSE)
                {
                    // BSE축 Motion Move : MOVING ECHO
                    int nBseAxis = IDX_T_AXIS;
                    if (nPM < 0) sprintf(szCNCData, "MM%d,-%s,2;", nBseAxis, pszData);
                    else sprintf(szCNCData, "MM%d,%s,2;", nBseAxis, pszData);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                }
                // 5축 STAGE
                else
                {
                    // BSE축 Motion Move : MOVING ECHO
                    int nBseAxis = IDX_B_AXIS;
                    if (nPM < 0) sprintf(szCNCData, "MM%d,-%s,2;", nBseAxis, pszData);
                    else sprintf(szCNCData, "MM%d,%s,2;", nBseAxis, pszData);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                }
                break;
            }
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:8554 ----
        //
        // Scan Bridge : "MC" -> X-Y Beam Center Position Move
        //
        public int AfxScanBridgeXYAxisCenterMoveCommand()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Axis Motion Move
            CharBuffer szCNCData = new CharBuffer(255 + 1);
            ZeroMem(szCNCData);
            
            strcpy_s(szCNCData, "MC0;");

            // X-Y Beam Center Position Move
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:8573 ----
        //
        // Scan Bridge : "MC" -> Read Beam Center Pos (X,Y축의 빔 센터 위치 값 읽기 및 적용한다
        //
        public int AfxScanBridgeMCCommand()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // 2021.03.31
            // MC 명령어 체계 변경 : X,Y,Z,R,T 센터값 읽기
            // "MC2,a,bbbb" : a->X('0'),Y('1'),Z('2'),R('3'),T('4'), b->위치펄스값

            // CNC 상태 및 제어용 윈도우 핸들 확인
            if (m_hStageSetupControlWnd == NULL) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 메시지 처리 전달용 USB Data
            USB_DEVICE_DATA_INFO UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();

            // CNC Command 설정 : 'MC' Command
            UsbDeviceDataInfo.nCommand = SBR_CMD_MC;

            double dCentermm = 0.0;

            // "MC2" COMMAND 인지 확인한다
            int nMC2Cmd = atoi(m_pIntegrated.szXStr);
            if (nMC2Cmd != 2) return RET_SUCCESS_OK;

            // AXIS 값 적용 한다
            int nAxis = atoi(m_pIntegrated.szYStr);
            if (nAxis<IDX_X_AXIS || nAxis>IDX_T_AXIS) return RET_SUCCESS_OK;

            // AXIS 의 Beam Center Position을 Pulse 값으로 보관(적용) 한다
            int nPulse = atoi(m_pIntegrated.szDStr);

            // AXIS BEAM CENTER 값 계산 및 적용
            // Axis 설정
            UsbDeviceDataInfo.nState = nAxis;
            // AXIS Beam Center Position 의 Pulse 값 저장
            if      (nAxis == IDX_X_AXIS) m_pIntegrated.nBeamCenterXPos = nPulse;
            else if (nAxis == IDX_Y_AXIS) m_pIntegrated.nBeamCenterYPos = nPulse;
            // AXIS Beam Center Position 의 거리[mm]를 계산 한다
            dCentermm = (double)nPulse * m_pIntegrated.dAxisMMPerPulse[nAxis];
            sprintf(UsbDeviceDataInfo.szData, "%g", dCentermm);
            // AXIS Beam Center Position 의 거리[mm]를 적용 한다
            if      (nAxis == IDX_X_AXIS) m_pIntegrated.dBeamCenterXPos = dCentermm;
            else if (nAxis == IDX_Y_AXIS) m_pIntegrated.dBeamCenterYPos = dCentermm;
            // X축의 Beam Center Position의 거리[mm] 값을 표시 한다
            SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, UsbDeviceDataInfo.nState, UsbDeviceDataInfo);

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:8625 ----
        //
        // Scan Bridge : "MC" -> X,Y Beam Center Read or Write
        //
        public int AfxScanBridgeMCCommand(int nWriteFlag, string pszXData, string pszYData)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Beam Center 정보
            CharBuffer szCNCData = new CharBuffer(255 + 1);

            int nPulse = 0;
            int nAxis  = 0;
            double dCenter = 0.0;

            // 정보값 저장하기 
            if (nWriteFlag == 1)
            {
                // MC Command
                if (pszXData == null) return RET_SUCCESS_OK;
                if (pszYData == null) return RET_SUCCESS_OK;

                nAxis   = atoi(pszXData); // AXIS
                dCenter = atof(pszYData); // CENTER VALUE

                ZeroMem(szCNCData);

                if (nAxis == IDX_X_AXIS)
                {
                    // X축 Beam Center
                    nPulse = (int)(dCenter / m_pIntegrated.dAxisMMPerPulse[IDX_X_AXIS]);
                    m_pIntegrated.nBeamCenterXPos = nPulse;
                    m_pIntegrated.dBeamCenterXPos = dCenter;
                }
                else if (nAxis == IDX_Y_AXIS)
                {
                    // Y축 Beam Center
                    nPulse = (int)(dCenter / m_pIntegrated.dAxisMMPerPulse[IDX_Y_AXIS]);
                    m_pIntegrated.nBeamCenterYPos = nPulse;
                    m_pIntegrated.dBeamCenterYPos = dCenter;
                }
                else
                {
                    return RET_SUCCESS_OK;
                }

                if (nAxis == 0 && dCenter == 0.0)
                {
                    // H/W 현재위치를 X,Y Beam Center 값 설정
                    strcpy_s(szCNCData, "MC1;");
                    // PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                }
                else
                {
                    // 2021.03.31
                    // X,Y Beam Center 값 설정
                    sprintf(szCNCData, "MC3,%d,%d;", nAxis, nPulse);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                }
            }
            else
            {
                // 2021.03.31
                // X Beam Center 값 읽기 COMMAND 전송
                strcpy_s(szCNCData, "MC2,0;");
                AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                // Y Beam Center 값 읽기 COMMAND 전송
                strcpy_s(szCNCData, "MC2,1;");
                AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:8700 ----
        //
        // Scan Bridge : "BF" -> Rotary Area Pulse 요청
        //
        public int AfxScanBridgeGetRotaryAreaPulseCommand()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // 2021.03.31
            // Rotary Area Pulse 요청
            CharBuffer szCNCData = new CharBuffer(255 + 1);
            ZeroMem(szCNCData);

            // Rotary Area Pulse 요청 COMMAND
            strcpy_s(szCNCData, "BF1;");

            // Rotary Area Pulse 요청 COMMAND 전송
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:8721 ----
        //
        // Scan Bridge : "BF" -> Rotary Area Pulse 요청
        //
        public int AfxScanBridgeBFCommand()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // CNC 상태 및 제어용 윈도우 핸들 확인
            if (m_hStageSetupControlWnd == NULL) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 메시지 처리 전달용 USB Data
            USB_DEVICE_DATA_INFO UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();

            // 2021.03.31
            // CNC Command 설정 : 'BF' Command
            UsbDeviceDataInfo.nCommand = SBR_CMD_BF;

            // Rotary Area Pulse Command Value
            int nCmd   = atoi(m_pIntegrated.szXStr);
            // Rotary Area Pulse 값
            int nPulse = atoi(m_pIntegrated.szYStr);

            // Rotary Area Pulse 설정
            UsbDeviceDataInfo.nState = nCmd;
            sprintf(UsbDeviceDataInfo.szData, "%d", nPulse);
            // Rotary Area Pulse 값을 표시 한다
            SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, UsbDeviceDataInfo.nState, UsbDeviceDataInfo);

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:8753 ----
        //
        // Scan Bridge : "BF" -> Rotary Area Pulse 값 Write
        //
        public int AfxScanBridgeBFCommand(int nWriteFlag, string pszData)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // 2021.03.31
            // Rotary Area Pulse 설정
            CharBuffer szCNCData = new CharBuffer(255 + 1);

            int nPulse = 0;

            // Rotary Area Pulse 값 설정하기 
            if (nWriteFlag == 1)
            {
                // BF Command

                if (pszData != null) nPulse = atoi(pszData);

                ZeroMem(szCNCData);

                // X,Y Beam Center 값 설정
                sprintf(szCNCData, "BF0,%d;", nPulse);
                // 명령어 전송
                AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:8784 ----
        //
        // Scan Bridge : "TS" -> Read TrackBall State (트랙볼의 현재 상태값 읽기 및 적용한다)
        //
        public int AfxScanBridgeTSCommand()
        {
            //if (m_pSbrCommand == NULL) return RET_ERROR_SBR_COMMAND_HANDLE;         // Scan Bridge  Command & Data Handle
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            int nTS = atoi(m_pIntegrated.szXStr);

            switch (nTS)
            {
                case CNC_COMMAND:
                {
                    // 트랙볼 명령어 (스텝모터 명령어) 표시한다
                    //TrackBallPanel->Caption = YStr;
                    break;
                }
                case XY_MOVING_ST:
                {
                    // 트랙볼 상태 : X,Y 이동중 상태 표시한다
                    m_pIntegrated.nTrackBallState = XY_MOVING_ST;
                    if (m_pIntegrated.nTBControlState != NULL)
                    {
                        m_pIntegrated.nTBControlState = 0;
                        //TB_IS_SpeedButton->Glyph->LoadFromFile(m_strAppDir + "\\Tool_Image\\TrackCntl.bmp");
                        //TB_GA_SpeedButton->Glyph->LoadFromFile(m_strAppDir + "\\Tool_Image\\TrackCntl.bmp");
                        //TB_ST_SpeedButton->Glyph->LoadFromFile(m_strAppDir + "\\Tool_Image\\TrackCntl.bmp");
                    }
                    //TrackBallPanel->Caption = " X-Y Moving";
                    break;
                }
                case X_MOVING_ST:
                {
                    // 트랙볼 상태 : X 이동중 상태 표시한다
                    m_pIntegrated.nTrackBallState = X_MOVING_ST;
                    if (m_pIntegrated.nTBControlState != NULL)
                    {
                        m_pIntegrated.nTBControlState = 0;
                        //TB_IS_SpeedButton->Glyph->LoadFromFile(m_strAppDir + "\\Tool_Image\\TrackCntl.bmp");
                        //TB_GA_SpeedButton->Glyph->LoadFromFile(m_strAppDir + "\\Tool_Image\\TrackCntl.bmp");
                        //TB_ST_SpeedButton->Glyph->LoadFromFile(m_strAppDir + "\\Tool_Image\\TrackCntl.bmp");
                    }
                    //TrackBallPanel->Caption = " X Moving";
                    break;
                }
                case Y_MOVING_ST:
                {
                    // 트랙볼 상태 : Y 이동중 상태 표시한다
                    m_pIntegrated.nTrackBallState = Y_MOVING_ST;
                    if (m_pIntegrated.nTBControlState != NULL)
                    {
                        m_pIntegrated.nTBControlState = 0;
                        //TB_IS_SpeedButton->Glyph->LoadFromFile(m_strAppDir + "\\Tool_Image\\TrackCntl.bmp");
                        //TB_GA_SpeedButton->Glyph->LoadFromFile(m_strAppDir + "\\Tool_Image\\TrackCntl.bmp");
                        //TB_ST_SpeedButton->Glyph->LoadFromFile(m_strAppDir + "\\Tool_Image\\TrackCntl.bmp");
                    }
                    //TrackBallPanel->Caption = " Y Moving";
                    break;
                }
                case TRACK_COUNT:
                {
                    // 트랙볼 상태 : Image Shift 상태 표시한다
                    if (m_pIntegrated.nTrackBallState != TRACK_COUNT)
                    {
                        m_pIntegrated.nTrackBallState = TRACK_COUNT;
                        m_pIntegrated.nTBControlState = TB_IS_ACTION_STATE;

                        //TrackBallPanel->Caption = " Image Shift ";
                        //TB_IS_SpeedButton->Glyph->LoadFromFile(m_strAppDir + "\\Tool_Image\\TrackCntlB.bmp");
                    }
                    break;
                }
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:8862 ----
        //
        // Scan Bridge : "TS" -> TrackBall (트랙볼의 관련 명령어 실행한다)
        //
        public int AfxScanBridgeTSCommand(int nWriteFlag, int nCommand, string pszData)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // "TS" -> Trackball 관련 명령어 적용
            CharBuffer szCNCData = new CharBuffer(255 + 1);
            ZeroMem(szCNCData);

            // 정보값 저장하기 
            if (nWriteFlag == 1)
            {
                // TS Command
                switch (nCommand)
                {
                case 3:
                {
                    // 트랙볼
                    //double dZTop_mm = atof(pszData);
                    //int nPulse = (int)(dZTop_mm * m_pIntegrated->dAxisPulsePerUnit[IDX_Z_AXIS]);
                    //sprintf(szCNCData, "TS0,%d;", nPulse);
                    // VacBoard -> PC -> To CncBoard
                    //AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                }
            }
            else
            {
                // TS Command
                switch (nCommand)
                {
                case 3: // Trackball X Direction Change
                {
                    // 트랙볼 X축 방향전환(토글)
                    strcpy_s(szCNCData, "TS3;");
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case 5: // Trackball X Direction Change
                {
                    // 트랙볼 Y축 방향전환(토글)
                    strcpy_s(szCNCData, "TS5;");
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                }
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:8918 ----
        //
        // Scan Bridge : "TC" -> Read TrackBall Command State (트랙볼의 현재 명령어 상태값 읽기 및 적용한다)
        //
        public int AfxScanBridgeTCCommand()
        {
            //if (m_pSbrCommand == NULL) return RET_ERROR_SBR_COMMAND_HANDLE;         // Scan Bridge  Command & Data Handle
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            int nST = atoi(m_pIntegrated.szXStr);
            int nTCount = atoi(m_pIntegrated.szYStr);
            int nValue = 0;

            if (abs(nTCount) > 126)
            {
                nTCount = 0;
                return RET_SUCCESS_OK;
            }

            if (m_pIntegrated.nTBControlState == TB_IS_ACTION_STATE)
            {
                switch (nST)
                {
                    case 0:
                    {
                        //-
                        m_pIntegrated.nTCXBuf[m_pIntegrated.nTCXCount++] = nTCount;
                        if (m_pIntegrated.nTCXCount >= MAX_TC_BUFFER) m_pIntegrated.nTCXCount = 0;
                        //-
                        // Image Shift X 현재 제어값 설정
                        //-
                        nValue = m_pIntegrated.nISXValue + nTCount;
                        if (nValue > m_pIntegrated.nISXMaxValue)
                        {
                            nValue = m_pIntegrated.nISXMaxValue;
                        }
                        if (nValue < m_pIntegrated.nISXMinValue)
                        {
                            nValue = m_pIntegrated.nISXMinValue;
                        }
                        m_pIntegrated.nISXValue = nValue;
                        //-
                        break;
                    }
                    case 1:
                    {
                        //-
                        m_pIntegrated.nTCYBuf[m_pIntegrated.nTCYCount++] = nTCount;
                        if (m_pIntegrated.nTCYCount >= MAX_TC_BUFFER) m_pIntegrated.nTCYCount = 0;
                        //-
                        // Image Shift Y 현재 제어값 설정
                        //-
                        nValue = m_pIntegrated.nISYValue + nTCount;
                        if (nValue > m_pIntegrated.nISYMaxValue)
                        {
                            nValue = m_pIntegrated.nISYMaxValue;
                        }
                        if (nValue < m_pIntegrated.nISYMinValue)
                        {
                            nValue = m_pIntegrated.nISYMinValue;
                        }
                        m_pIntegrated.nISYValue = nValue;
                        //-
                        break;
                    }
                }
            }
            else
            if (m_pIntegrated.nTBControlState == TB_GA_ACTION_STATE)
            {
                switch (nST)
                {
                    case 0:
                    {
                        //-
                        // Gun Align X 현재 제어값 설정
                        //-
                        nValue = m_pIntegrated.nGAXValue + nTCount;
                        if (nValue > m_pIntegrated.nGAXMaxValue)
                        {
                            nValue = m_pIntegrated.nGAXMaxValue;
                        }
                        if (nValue < m_pIntegrated.nGAXMinValue)
                        {
                            nValue = m_pIntegrated.nGAXMinValue;
                        }
                        m_pIntegrated.nGAXValue = nValue;
                        //-
                        break;
                    }
                    case 1:
                    {
                        //-
                        // Gun Align Y 현재 제어값 설정
                        //-
                        nValue = m_pIntegrated.nGAYValue + nTCount;
                        if (nValue > m_pIntegrated.nGAYMaxValue)
                        {
                            nValue = m_pIntegrated.nGAYMaxValue;
                        }
                        if (nValue < m_pIntegrated.nGAYMinValue)
                        {
                            nValue = m_pIntegrated.nGAYMinValue;
                        }
                        m_pIntegrated.nGAYValue = nValue;
                        //-
                        break;
                    }
                }
            }
            else
            if (m_pIntegrated.nTBControlState == TB_ST_ACTION_STATE)
            {
                switch (nST)
                {
                    case 0:
                    {
                        //-
                        // Stig X 현재 제어값 설정
                        //-
                        m_pIntegrated.nStigXValue += nTCount;
                        if (m_pIntegrated.nStigXValue > m_pIntegrated.nStigXMaxValue)
                        {
                            m_pIntegrated.nStigXValue = m_pIntegrated.nStigXMaxValue;
                        }
                        if (m_pIntegrated.nStigXValue < m_pIntegrated.nStigXMinValue)
                        {
                            m_pIntegrated.nStigXValue = m_pIntegrated.nStigXMinValue;
                        }
                        //MainScrollBarUpdate(STIGXY_BAR_MODE);
                        //-
                        break;
                    }
                    case 1:
                    {
                        //-
                        // Stig Y 현재 제어값 설정
                        //-
                        m_pIntegrated.nStigYValue += nTCount;
                        if (m_pIntegrated.nStigYValue > m_pIntegrated.nStigYMaxValue)
                        {
                            m_pIntegrated.nStigYValue = m_pIntegrated.nStigYMaxValue;
                        }
                        if (m_pIntegrated.nStigYValue < m_pIntegrated.nStigYMinValue)
                        {
                            m_pIntegrated.nStigYValue = m_pIntegrated.nStigYMinValue;
                        }
                        //MainScrollBarUpdate(STIGXY_BAR_MODE);
                        //-
                        break;
                    }
                }
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:9074 ----
        //
        // Scan Bridge : "TB" -> Read TrackBall Command
        //
        public int AfxScanBridgeTBCommand()
        {
            //if (m_pSbrCommand == NULL) return RET_ERROR_SBR_COMMAND_HANDLE;         // Scan Bridge  Command & Data Handle
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:9085 ----
        //
        // Scan Bridge : "BT" -> Read BlueTooth Command
        //
        public int AfxScanBridgeBTCommand()
        {
            //if (m_pSbrCommand == NULL) return RET_ERROR_SBR_COMMAND_HANDLE;         // Scan Bridge  Command & Data Handle
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Bluetooth Command Mode
            int nMode = atoi(m_pIntegrated.szXStr);
            // Bluetooth Command State (Select)
            int nST = atoi(m_pIntegrated.szYStr);

            switch (nMode)
            {
                case 1: // Scan Change
                {
                    if (nST == 1)
                    {
                        // RED Scan
                        //RedScanSpeedButtonClick(this);   // "BT,1,1;"

                    }
                    else if (nST == 2)
                    {
                        // FAST Scan
                        //FastScanSpeedButtonClick(this);  // "BT,1,2;"

                    }
                    else if (nST == 3)
                    {
                        // SLOW Scan
                        //SlowScanSpeedButtonClick(this);  // "BT,1,3;"

                    }
                    else if (nST == 4)
                    {
                        // PHOTO Scan
                        //PhotoScanSpeedButtonClick(this); // "BT,1,4;"

                    }
                    else if (nST == 5)
                    {
                        // STOP Scan
                        //FreezeSpeedButtonClick(this);    // "BT,1,5;"
                    }
                    break;
                }
                case 2: // Mag Change
                {
                    if (nST == 1)
                    {
                        // 배율 높이기
                        //MagnitudeCSpinButtonUpClick(this);   // "BT,2,1;"
                    }
                    else if (nST == 2)
                    {
                        // 배율 낮추기
                        //MagnitudeCSpinButtonDownClick(this); // "BT,2,2;"
                    }
                    break;
                }
                case 3: // Focus Change
                {
                    break;
                }
                case 4: // Stage Moving
                {
                    break;
                }
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:9161 ----
        //
        // Scan Bridge : "LT" -> Read Limit SW Status (모터 각 축의 Limit 스위치 상태값을 읽기 및 적용한다
        //
        public int AfxScanBridgeLTCommand()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // CNC 상태 및 제어용 윈도우 핸들 확인
            if (m_hStageSetupControlWnd == NULL) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 메시지 처리 전달용 USB Data
            USB_DEVICE_DATA_INFO UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            int bMessage = FALSE;

            // CNC Command 설정 : 'LT' Command
            UsbDeviceDataInfo.nCommand = SBR_CMD_LT;

            int nLimitSW = 0;
            int nLimitTYPE = 0;

            CharBuffer szStr = new CharBuffer(MAX_AXIS_STR_DISPLAY_BUFFER + 1);
            ZeroMem(szStr);

            nLimitTYPE = (int)(m_pIntegrated.szXStr[0] - (byte)'0');

            // Home Limit Switch Check (Limit스위치를 Home스위치와 겸용으로 사용한다
            if (nLimitTYPE == 1)
            {
                nLimitSW = atoi(m_pIntegrated.szYStr);
                // 1->Limit SW 상태, 2->Home SW 상태
                UsbDeviceDataInfo.nData[10] = 1;

                // X축 Limit & Home
                if ((nLimitSW & 0x01) != 0)
                {
                    // X축 Limit & Home = On 상태
                    m_pIntegrated.nAxisLimitHomeStatus[IDX_X_AXIS] = 1;
                    bMessage = TRUE;
                    // Axis 설정
                    UsbDeviceDataInfo.nState = IDX_X_AXIS;
                    // X축 Limit & Home = On 상태
                    UsbDeviceDataInfo.nData[UsbDeviceDataInfo.nState] = m_pIntegrated.nAxisLimitHomeStatus[UsbDeviceDataInfo.nState];
                }
                else
                {
                    // X축 Limit & Home = Off 상태
                    m_pIntegrated.nAxisLimitHomeStatus[IDX_X_AXIS] = 0;
                    bMessage = TRUE;
                    // Axis 설정
                    UsbDeviceDataInfo.nState = IDX_X_AXIS;
                    // X축 Limit & Home = Off 상태
                    UsbDeviceDataInfo.nData[UsbDeviceDataInfo.nState] = m_pIntegrated.nAxisLimitHomeStatus[UsbDeviceDataInfo.nState];
                }

                // Y축 Limit & Home
                if ((nLimitSW & 0x02) != 0)
                {
                    // Y축 Limit & Home = On 상태
                    m_pIntegrated.nAxisLimitHomeStatus[IDX_Y_AXIS] = 1;
                    bMessage = TRUE;
                    // Axis 설정
                    UsbDeviceDataInfo.nState = IDX_Y_AXIS;
                    // X축 Limit & Home = On 상태
                    UsbDeviceDataInfo.nData[UsbDeviceDataInfo.nState] = m_pIntegrated.nAxisLimitHomeStatus[UsbDeviceDataInfo.nState];
                }
                else
                {
                    // Y축 Limit & Home = Off 상태
                    m_pIntegrated.nAxisLimitHomeStatus[IDX_Y_AXIS] = 0;
                    bMessage = TRUE;
                    // Axis 설정
                    UsbDeviceDataInfo.nState = IDX_Y_AXIS;
                    // Y축 Limit & Home = Off 상태
                    UsbDeviceDataInfo.nData[UsbDeviceDataInfo.nState] = m_pIntegrated.nAxisLimitHomeStatus[UsbDeviceDataInfo.nState];
                }

                // Z축 Limit & Home
                if ((nLimitSW & 0x04) != 0)
                {
                    // Z축 Limit & Home = On 상태
                    m_pIntegrated.nAxisLimitHomeStatus[IDX_Z_AXIS] = 1;
                    bMessage = TRUE;
                    // Axis 설정
                    UsbDeviceDataInfo.nState = IDX_Z_AXIS;
                    // Z축 Limit & Home = On 상태
                    UsbDeviceDataInfo.nData[UsbDeviceDataInfo.nState] = m_pIntegrated.nAxisLimitHomeStatus[UsbDeviceDataInfo.nState];
                }
                else
                {
                    // Z축 Limit & Home = Off 상태
                    m_pIntegrated.nAxisLimitHomeStatus[IDX_Z_AXIS] = 0;
                    bMessage = TRUE;
                    // Axis 설정
                    UsbDeviceDataInfo.nState = IDX_Z_AXIS;
                    // Z축 Limit & Home = Off 상태
                    UsbDeviceDataInfo.nData[UsbDeviceDataInfo.nState] = m_pIntegrated.nAxisLimitHomeStatus[UsbDeviceDataInfo.nState];
                }

                // R축 Limit & Home
                if ((nLimitSW & 0x08) != 0)
                {
                    // R축 Limit & Home = On 상태
                    m_pIntegrated.nAxisLimitHomeStatus[IDX_R_AXIS] = 1;
                    bMessage = TRUE;
                    // Axis 설정
                    UsbDeviceDataInfo.nState = IDX_R_AXIS;
                    // R축 Limit & Home = On 상태
                    UsbDeviceDataInfo.nData[UsbDeviceDataInfo.nState] = m_pIntegrated.nAxisLimitHomeStatus[UsbDeviceDataInfo.nState];
                }
                else
                {
                    // R축 Limit & Home = Off 상태
                    m_pIntegrated.nAxisLimitHomeStatus[IDX_R_AXIS] = 0;
                    bMessage = TRUE;
                    // Axis 설정
                    UsbDeviceDataInfo.nState = IDX_R_AXIS;
                    // R축 Limit & Home = Off 상태
                    UsbDeviceDataInfo.nData[UsbDeviceDataInfo.nState] = m_pIntegrated.nAxisLimitHomeStatus[UsbDeviceDataInfo.nState];
                }

                // T축 Limit & Home
                if ((nLimitSW & 0x10) != 0)
                {
                    // T축 Limit & Home = On 상태
                    m_pIntegrated.nAxisLimitHomeStatus[IDX_T_AXIS] = 1;
                    bMessage = TRUE;
                    // Axis 설정
                    UsbDeviceDataInfo.nState = IDX_T_AXIS;
                    // T축 Limit & Home = On 상태
                    UsbDeviceDataInfo.nData[UsbDeviceDataInfo.nState] = m_pIntegrated.nAxisLimitHomeStatus[UsbDeviceDataInfo.nState];
                }
                else
                {
                    // T축 Limit & Home = Off 상태
                    m_pIntegrated.nAxisLimitHomeStatus[IDX_T_AXIS] = 0;
                    bMessage = TRUE;
                    // Axis 설정
                    UsbDeviceDataInfo.nState = IDX_T_AXIS;
                    // T축 Limit & Home = Off 상태
                    UsbDeviceDataInfo.nData[UsbDeviceDataInfo.nState] = m_pIntegrated.nAxisLimitHomeStatus[UsbDeviceDataInfo.nState];
                }


                // B축 Home S/W
                if ((nLimitSW & 0x20) != 0)
                {
                    // BSE축 Home = On 상태
                    m_pIntegrated.nAxisLimitSWStatus[IDX_B_AXIS] = 1;
                    bMessage = TRUE;
                    // Axis 설정
                    UsbDeviceDataInfo.nState = IDX_B_AXIS;
                    // BSE축 Home = On 상태
                    UsbDeviceDataInfo.nData[UsbDeviceDataInfo.nState] = m_pIntegrated.nAxisLimitSWStatus[UsbDeviceDataInfo.nState];
                }
                // B축 Limit S/W
                else if ((nLimitSW & 0x40) != 0)
                {
                    // BSE축 Limit = On 상태
                    m_pIntegrated.nAxisLimitSWStatus[IDX_B_AXIS] = 10;
                    bMessage = TRUE;
                    // Axis 설정
                    UsbDeviceDataInfo.nState = IDX_B_AXIS;
                    // BSE축 Limit = On 상태
                    UsbDeviceDataInfo.nData[UsbDeviceDataInfo.nState] = m_pIntegrated.nAxisLimitSWStatus[UsbDeviceDataInfo.nState];
                }
                else
                {
                    // BSE축 Home & Limit = Off 상태
                    m_pIntegrated.nAxisLimitSWStatus[IDX_B_AXIS] = 0;
                    bMessage = TRUE;
                    // Axis 설정
                    UsbDeviceDataInfo.nState = IDX_B_AXIS;
                    // BSE축 Home = Off 상태
                    UsbDeviceDataInfo.nData[UsbDeviceDataInfo.nState] = m_pIntegrated.nAxisLimitSWStatus[UsbDeviceDataInfo.nState];
                }


                // Axis Limit & Home Switch 정보 전송
                if (bMessage == TRUE)
                {
                    if (m_hOwnerWnd != NULL)
                    {
                        // DEVICE MAIN SERVER
                        // Axis Limit & Home Switch 값 정보 메시지 보내기
                        // 2020.05.04
                        SendMessage(m_hOwnerWnd, UM_USB_SBR_STAGE_LIMIT_STATUS, 0, UsbDeviceDataInfo);
                    }
                }

                // Axis Limit State 표시하기 
                SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, 0, UsbDeviceDataInfo);
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:9359 ----
        //
        // Scan Bridge : "RB" -> Read Backlash Pulse 값 읽기 및 적용
        //
        public int AfxScanBridgeRBCommand()
        {
            //if (m_pSbrCommand == NULL) return RET_ERROR_SBR_COMMAND_HANDLE;         // Scan Bridge  Command & Data Handle
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // 모터 축(Axis) 설정
            int nAxis = (int)(m_pIntegrated.szXStr[0] - (byte)'0');

            switch (nAxis)
            {
                case IDX_X_AXIS:
                {
                    // X축 Backlash 표시
                    //MotorStageSetupForm->XBacklashEdit->Text = m_pIntegrated->szYStr;
                    //MotorStageSetupForm->XBacklashEdit->Font->Color = clYellow;
                    break;
                }
                case IDX_Y_AXIS:
                {
                    // Y축 Backlash 표시
                    //MotorStageSetupForm->YBacklashEdit->Text = m_pIntegrated->szYStr;
                    //MotorStageSetupForm->YBacklashEdit->Font->Color = clYellow;
                    break;
                }
                case IDX_Z_AXIS:
                {
                    // Z축 Backlash 표시
                    //MotorStageSetupForm->ZBacklashEdit->Text = m_pIntegrated->szYStr;
                    //MotorStageSetupForm->ZBacklashEdit->Font->Color = clYellow;
                    break;
                }
                case IDX_R_AXIS:
                {
                    // R축 Backlash 표시
                    //MotorStageSetupForm->RBacklashEdit->Text = m_pIntegrated->szYStr;
                    //MotorStageSetupForm->RBacklashEdit->Font->Color = clYellow;
                    break;
                }
                case IDX_T_AXIS:
                {
                    // T축 Backlash 표시
                    //MotorStageSetupForm->TBacklashEdit->Text = m_pIntegrated->szYStr;
                    //MotorStageSetupForm->TBacklashEdit->Font->Color = clYellow;
                    break;
                }
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:9413 ----
        //
        // Scan Bridge : "ZT" -> Z Top Limit Sw Pulse 값 읽기 및 적용
        //
        public int AfxScanBridgeZTCommand()
        {
            //if (m_pSbrCommand == NULL) return RET_ERROR_SBR_COMMAND_HANDLE;         // Scan Bridge  Command & Data Handle
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // CNC 상태 및 제어용 윈도우 핸들 확인
            if (m_hStageSetupControlWnd == NULL) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 메시지 처리 전달용 USB Data
            USB_DEVICE_DATA_INFO UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            int bMessage = FALSE;

            // CNC Command 설정 : 'ZT' Command
            UsbDeviceDataInfo.nCommand = SBR_CMD_ZT;

            CharBuffer szStr = new CharBuffer(MAX_AXIS_STR_DISPLAY_BUFFER + 1);
            ZeroMem(szStr);

            // Z Top Limit SW 상태값 읽기
            int nVal = (int)(m_pIntegrated.szXStr[0] - (byte)'0');
            double dZTtop_mm = 0.0;

            // Z축 Top Limit SW -> On 상태일 경우 
            if (nVal == 1)
            {
                // Z축 Top Limit SW -> On 상태의 위치 Pulse 값
                m_pIntegrated.nZTopLimitSwPulse = atoi(m_pIntegrated.szYStr);
                bMessage = TRUE;

                // 축(Axis)의 1[mm] 또는 1[각도] 당 이동Pulse 갯수 확인
                if (m_pIntegrated.dAxisPulsePerUnit[IDX_Z_AXIS] == 0)
                {
                    dZTtop_mm = 0;
                }
                else
                {
                    // 축(Axis)의 1[mm] 또는 1[각도] 당 이동Pulse 갯수 확인하여 -> Z축의 Top 위치를 [mm]로 환산한다
                    dZTtop_mm = (double)m_pIntegrated.nZTopLimitSwPulse / m_pIntegrated.dAxisPulsePerUnit[IDX_Z_AXIS];
                }

                m_pIntegrated.dZTopLimitPosMM = dZTtop_mm;
                ZeroMem(szStr);
                sprintf(szStr, "%3.1f", dZTtop_mm);

                // Z축 Top(또는 Bottom) 위치값[mm] 를 표시한다
                strcpy_s(UsbDeviceDataInfo.szData, szStr);

                // Z-Axis Top(또는 Bottom) 위치값[mm] 정보 전송
                if (bMessage == TRUE)
                {
                    if (m_hOwnerWnd != NULL)
                    {
                        // DEVICE MAIN SERVER
                        // Axis Limit & Home Switch 값 정보 메시지 보내기
                        UsbDeviceDataInfo.nState = IDX_Z_AXIS + 1;
                        // 2020.05.04
                        //SendMessage(m_hOwnerWnd, UM_USB_SBR_STAGE_Z_AXIS_LIMIT, (WPARAM)0, (LPARAM)&UsbDeviceDataInfo);
                    }
                }

                // Z축 Top 위치값[mm] 를 표시한다
                SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, 0, UsbDeviceDataInfo);
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:9485 ----
        //
        // Scan Bridge : "ZT" -> Z Top Limit Sw Pulse 값 읽기 및 적용
        //
        public int AfxScanBridgeZTCommand(int nWriteFlag, string pszData)
        {
            //if (m_pSbrCommand == NULL) return RET_ERROR_SBR_COMMAND_HANDLE;         // Scan Bridge  Command & Data Handle
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Z축 Top Limit 위치값[mm]
            CharBuffer szCNCData = new CharBuffer(255 + 1);
            ZeroMem(szCNCData);

            // 정보값 저장하기 
            if (nWriteFlag == 1)
            {
                // ZT Command
                // Z축 Top Limit 위치값[mm] 저장하기
                double dZTop_mm = atof(pszData);
                int nPulse = (int)(dZTop_mm * m_pIntegrated.dAxisPulsePerUnit[IDX_Z_AXIS]);
                m_pIntegrated.dTiltZeroAnglePosMM = dZTop_mm;
                sprintf(szCNCData, "ZT0,%d;", nPulse);

                // VacBoard -> PC -> To CncBoard
                AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
            }
            else
            {
                // ZT Command
                // Z축 Top Limit 위치값[mm] 표시하기 
                strcpy_s(szCNCData, "ZT1;");

                // VacBoard -> PC -> To CncBoard
                AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:9524 ----
        //
        // Scan Bridge : "SB" -> Sampler Exchange Position Pulse 값 읽기 및 적용 (X, Y, Z, R_Load, R_Unload)
        //
        public int AfxScanBridgeSBCommand()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // CNC 상태 및 제어용 윈도우 핸들 확인
            if (m_hStageSetupControlWnd == NULL) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 메시지 처리 전달용 USB Data
            USB_DEVICE_DATA_INFO UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            int bMessage = FALSE;

            // CNC Command 설정 : 'SB' Command
            UsbDeviceDataInfo.nCommand = SBR_CMD_SB;

            int nNum = 0;
            int nExc_sam_pul = 0;
            double dExc_sam_mm = 0.0;

            // 샘플 교환 모터 축(Axis) 
            nNum = atoi(m_pIntegrated.szXStr);
            // 샘플 교환 위치 값(Pulse)
            nExc_sam_pul = atoi(m_pIntegrated.szYStr);

            // 모터 축(Axis)
            switch (nNum)
            {
                // X
                case IDX_X_AXIS:
                {
                    // X축 샘플교환 위치값[mm] 계산 = (X축 샘플교환위치[Pulse]) x (X축 1Pulse당 이동거리[mm])
                    dExc_sam_mm = (double)nExc_sam_pul * m_pIntegrated.dAxisMMPerPulse[nNum];
                    // X축 샘플교환 위치값[mm] 표시
                    m_pIntegrated.dFixExtraSamPos[nNum] = dExc_sam_mm;
                    UsbDeviceDataInfo.nState      = nNum;
                    UsbDeviceDataInfo.dData[nNum] = dExc_sam_mm;
                    bMessage = TRUE;
                    break;
                }
                // Y
                case IDX_Y_AXIS:
                {
                    UsbDeviceDataInfo.nState = nNum;
                    // Y축 샘플교환 위치값[mm] 계산 = (Y축 샘플교환위치[Pulse]) x (Y축 1Pulse당 이동거리[mm])
                    dExc_sam_mm = (double)nExc_sam_pul * m_pIntegrated.dAxisMMPerPulse[nNum];
                    // Y축 샘플교환 위치값[mm] 표시
                    m_pIntegrated.dFixExtraSamPos[nNum] = dExc_sam_mm;
                    UsbDeviceDataInfo.nState      = nNum;
                    UsbDeviceDataInfo.dData[nNum] = dExc_sam_mm;
                    bMessage = TRUE;
                    break;
                }
                // Z
                case IDX_Z_AXIS:
                {
                    UsbDeviceDataInfo.nState = nNum;
                    // Z축 샘플교환 위치값[mm] 계산 = (Z축 샘플교환위치[Pulse]) x (Z축 1Pulse당 이동거리[mm])
                    dExc_sam_mm = (double)nExc_sam_pul * m_pIntegrated.dAxisMMPerPulse[nNum];
                    // Z축 샘플교환 위치값[mm] 표시
                    m_pIntegrated.dFixExtraSamPos[nNum] = dExc_sam_mm;
                    UsbDeviceDataInfo.nState      = nNum;
                    UsbDeviceDataInfo.dData[nNum] = dExc_sam_mm;
                    bMessage = TRUE;
                    break;
                }
                // R Load
                case IDX_R_AXIS:
                {
                    UsbDeviceDataInfo.nState = nNum;
                    // R축 샘플Load 위치값[mm] 계산 = (R축 샘플Load위치[Pulse]) x (R축 1Pulse당 이동거리[mm])
                    dExc_sam_mm = (double)nExc_sam_pul * m_pIntegrated.dAxisMMPerPulse[nNum];
                    // R축 샘플Load 위치값[mm] 표시
                    m_pIntegrated.dFixExtraSamPos[nNum] = dExc_sam_mm;
                    UsbDeviceDataInfo.nState      = nNum;
                    UsbDeviceDataInfo.dData[nNum] = dExc_sam_mm;
                    bMessage = TRUE;
                    break;
                }
                // R Unload
                case IDX_R_AXIS+1:
                {
                    UsbDeviceDataInfo.nState = nNum;
                    // R축 샘플Unload 위치값[mm] 계산 = (R축 샘플Unload위치[Pulse]) x (R축 1Pulse당 이동거리[mm])
                    dExc_sam_mm = (double)nExc_sam_pul * m_pIntegrated.dAxisMMPerPulse[IDX_R_AXIS];
                    // R축 샘플Unload 위치값[mm] 표시
                    m_pIntegrated.dFixExtraSamPos[nNum] = dExc_sam_mm;
                    UsbDeviceDataInfo.nState      = nNum;
                    UsbDeviceDataInfo.dData[nNum] = dExc_sam_mm;
                    bMessage = TRUE;
                    break;
                }
            }

            // 샘플 교환 위치 값 정보 전송
            if (bMessage == TRUE)
            {
                if (m_hOwnerWnd != NULL)
                {
                    // DEVICE MAIN SERVER
                    // 샘플 교환 위치 값 정보 메시지 보내기
                    // 2020.05.04
                    SendMessage(m_hOwnerWnd, UM_USB_SBR_STAGE_FIX_POSITION, 0, UsbDeviceDataInfo);
                }
            }

            // 모터 축(Axis)
            switch (nNum)
            {
            // X
            case IDX_X_AXIS:
            {
                // X축 샘플교환 위치값[mm] 표시
                UsbDeviceDataInfo.nState = nNum;
                // X축 샘플교환 위치값[mm] 표시
                UsbDeviceDataInfo.dData[UsbDeviceDataInfo.nState] = m_pIntegrated.dFixExtraSamPos[UsbDeviceDataInfo.nState];
                sprintf(UsbDeviceDataInfo.szData, "%g", UsbDeviceDataInfo.dData[UsbDeviceDataInfo.nState]);
                // X축 샘플교환 위치값[mm] 표시
                SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, 0, UsbDeviceDataInfo);
                break;
            }
            // Y
            case IDX_Y_AXIS:
            {
                // Y축 샘플교환 위치값[mm] 표시
                UsbDeviceDataInfo.nState = nNum;
                // Y축 샘플교환 위치값[mm] 표시
                UsbDeviceDataInfo.dData[UsbDeviceDataInfo.nState] = m_pIntegrated.dFixExtraSamPos[UsbDeviceDataInfo.nState];
                sprintf(UsbDeviceDataInfo.szData, "%g", UsbDeviceDataInfo.dData[UsbDeviceDataInfo.nState]);
                // Y축 샘플교환 위치값[mm] 표시
                SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, 0, UsbDeviceDataInfo);
                break;
            }
            // Z
            case IDX_Z_AXIS:
            {
                // Z축 샘플교환 위치값[mm] 표시
                UsbDeviceDataInfo.nState = nNum;
                // Z축 샘플교환 위치값[mm] 표시
                UsbDeviceDataInfo.dData[UsbDeviceDataInfo.nState] = m_pIntegrated.dFixExtraSamPos[UsbDeviceDataInfo.nState];
                sprintf(UsbDeviceDataInfo.szData, "%g", UsbDeviceDataInfo.dData[UsbDeviceDataInfo.nState]);
                // Z축 샘플교환 위치값[mm] 표시
                SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, 0, UsbDeviceDataInfo);
                break;
            }
            // R Load
            case IDX_R_AXIS:
            {
                // R축 샘플Load 위치값[mm] 표시
                UsbDeviceDataInfo.nState = nNum;
                // R축 샘플Load 위치값[mm] 표시
                UsbDeviceDataInfo.dData[UsbDeviceDataInfo.nState] = m_pIntegrated.dFixExtraSamPos[UsbDeviceDataInfo.nState];
                sprintf(UsbDeviceDataInfo.szData, "%g", UsbDeviceDataInfo.dData[UsbDeviceDataInfo.nState]);
                // R축 샘플Load 위치값[mm] 표시
                SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, 0, UsbDeviceDataInfo);
                break;
            }
            // R Unload
            case IDX_R_AXIS + 1:
            {
                // R축 샘플Unload 위치값[mm] 표시
                UsbDeviceDataInfo.nState = nNum;
                // R축 샘플Unload 위치값[mm] 표시
                UsbDeviceDataInfo.dData[UsbDeviceDataInfo.nState] = m_pIntegrated.dFixExtraSamPos[UsbDeviceDataInfo.nState];
                sprintf(UsbDeviceDataInfo.szData, "%g", UsbDeviceDataInfo.dData[UsbDeviceDataInfo.nState]);
                // R축 샘플Unload 위치값[mm] 표시
                SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, 0, UsbDeviceDataInfo);
                break;
            }
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:9700 ----
        //
        // Scan Bridge : "SB" -> Sample Position 관련 명령어 적용
        //
        public int AfxScanBridgeSBCommand(int nWriteFlag, int nCommand, string pszData1 = null, string pszData2 = null, string pszData3 = null)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // "SB" -> Sample Position 관련 명령어 적용
            CharBuffer szCNCData = new CharBuffer(255 + 1);
            ZeroMem(szCNCData);

            // 정보값 저장하기 
            if (nWriteFlag == 1)
            {
                // SB Command
                switch (nCommand)
                {
                case 15:
                case 16:
                case 17:
                {
                    double dXExtra_mm = 0.0;
                    double dYExtra_mm = 0.0;
                    double dZExtra_mm = 0.0;
                    double dRLExtra_mm = 0.0;
                    double dRUExtra_mm = 0.0;
                    int lPulse = 0;

                    // X,Y,Z-Axis Fix Extra Sample Position 값 설정
                    if (pszData1 != null) dXExtra_mm = atof(pszData1);
                    if (pszData2 != null) dYExtra_mm = atof(pszData2);
                    if (pszData3 != null) dZExtra_mm = atof(pszData3);

                    // Position 값 확인
                    if (dXExtra_mm == 0.0 || dYExtra_mm == 0 || dZExtra_mm == 0)
                    {
                        // 현재 위치를 X,Y,Z Fix Extra Sample Position 으로 설정
                        strcpy_s(szCNCData, "SB6;");
                        // VacBoard -> PC -> To CncBoard
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    } 
                    else
                    {
                        // X-Axis Fix Extra Sample Position 설정
                        lPulse = (int)(dXExtra_mm / m_pIntegrated.dAxisMMPerPulse[IDX_X_AXIS]);
                        sprintf(szCNCData, "SB15,%d;", lPulse);
                        // VacBoard -> PC -> To CncBoard
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);

                        // Y-Axis Fix Extra Sample Position 설정
                        lPulse = (int)(dYExtra_mm / m_pIntegrated.dAxisMMPerPulse[IDX_Y_AXIS]);
                        sprintf(szCNCData, "SB16,%d;", lPulse);
                        // VacBoard -> PC -> To CncBoard
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);

                        // Z-Axis Fix Extra Sample Position 설정
                        lPulse = (int)(dZExtra_mm / m_pIntegrated.dAxisMMPerPulse[IDX_Z_AXIS]);
                        sprintf(szCNCData, "SB16,%d;", lPulse);
                        // VacBoard -> PC -> To CncBoard
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    }
                    break;
                }
                case 18:
                {
                    double dRExtra_mm = 0.0;
                    int lPulse = 0;

                    // R-Axis Fix Extra Load Sample Position 값 설정
                    if (pszData1 != null) dRExtra_mm = atof(pszData1);

                    // Position 값 확인
                    if (dRExtra_mm == 0.0)
                    {
                        // 현재 위치를 R-Axis Fix Extra Load Sample Position 으로 설정
                        strcpy_s(szCNCData, "SB9;");
                        // VacBoard -> PC -> To CncBoard
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    }
                    else
                    {
                        // R-Axis Fix Extra Load Sample Position 설정
                        lPulse = (int)((double)(dRExtra_mm+360.0) / m_pIntegrated.dAxisMMPerPulse[IDX_R_AXIS]);
                        sprintf(szCNCData, "SB18,%d;", lPulse);
                        // VacBoard -> PC -> To CncBoard
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    }
                    break;
                }
                case 19:
                {
                    double dRExtra_mm = 0.0;
                    int lPulse = 0;

                    // R-Axis Fix Extra Unload Sample Position 값 설정
                    if (pszData1 != null) dRExtra_mm = atof(pszData1);

                    // Position 값 확인
                    if (dRExtra_mm == 0.0)
                    {
                        // 현재 위치를 R-Axis Fix Extra Unload Sample Position 으로 설정
                        strcpy_s(szCNCData, "SB10;");
                        // VacBoard -> PC -> To CncBoard
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    }
                    else
                    {
                        // R-Axis Fix Extra Unload Sample Position 설정
                        lPulse = (int)((double)(dRExtra_mm+360.0) / m_pIntegrated.dAxisMMPerPulse[IDX_R_AXIS]);
                        sprintf(szCNCData, "SB19,%d;", lPulse);
                        // VacBoard -> PC -> To CncBoard
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    }
                    break;
                }
                }
            }
            else
            {
                // SB Command
                switch (nCommand)
                {
                case 7: // Go Exchange Position
                {
                    // Exchange Sample Position 으로 이동
                    strcpy_s(szCNCData, "SB7;");
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case 13: // Go R-Axis Load Sample Position
                {
                    // R-Axis Load Sample Position 으로 이동
                    strcpy_s(szCNCData, "SB13;");
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case 14: // Go R-Axis Unload Sample Position
                {
                    // R-Axis Unload Sample Position 으로 이동
                    strcpy_s(szCNCData, "SB14;");
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case 8: // X,Y,Z Axis Fix Extra Sample Positon 값 읽기 명령
                {
                    // X,Y,Z Fix Extra Sample Position
                    strcpy_s(szCNCData, "SB8;");
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case 11: // R-Axis Fix Extra Load Sample Positon 값 읽기 명령
                {
                    // R-Axis Fix Extra Load Sample Position
                    strcpy_s(szCNCData, "SB11;");
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case 12: // R-Axis Fix Extra Unload Sample Positon 값 읽기 명령
                {
                    // R-Axis Fix Extra Unload Sample Position
                    strcpy_s(szCNCData, "SB12;");
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                }
            }

            return RET_SUCCESS_OK;
        }

    }
}
