// 원본 USB_FT232H_Device.cpp:9877-11888 을 NanoImpact.CodeGen translate로 옮긴 뒤 손으로 고친 코드.
#pragma warning disable IDE1006, IDE0059, CS0162, CS0168, CS0219
using System;
using NanoImpact.Core.Native;
using static NanoImpact.Core.Def;
using static NanoImpact.Core.Native.CRuntime;

namespace NanoImpact.Hardware.Device
{
    public sealed partial class SemDevice
    {
        // ---- USB_FT232H_Device.cpp:9877 ----
        //
        // Scan Bridge : "LS" -> Read Limit SW Stop Status 값 읽기 및 적용
        //
        public int AfxScanBridgeLSCommand()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // CNC 상태 및 제어용 윈도우 핸들 확인
            if (m_hStageSetupControlWnd == NULL) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 메시지 처리 전달용 USB Data
            USB_DEVICE_DATA_INFO UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            int bMessage = FALSE;

            // CNC Command 설정 : 'LS' Command
            UsbDeviceDataInfo.nCommand = SBR_CMD_LS;

            int nEmergency_val = 0;
            int nLimit_type = 0;

            // Limit SW Stop ?
            nLimit_type = (int)(m_pIntegrated.szXStr[0] - (byte)'0');

            // Limit SW Stop ?
            if (nLimit_type == 0)
            {
                // 모터 Stop 상태 값 읽기 및 확인
                nEmergency_val = atoi(m_pIntegrated.szYStr);

                // X축 Limit & Home
                if ((nEmergency_val & 0x01) != 0)
                {
                    // X축 Limit & Home = On 상태
                    m_pIntegrated.nAxisLimitSWStatus[IDX_X_AXIS] = 1;
                    bMessage = TRUE;
                    // Axis 설정
                    UsbDeviceDataInfo.nState = IDX_X_AXIS;
                    // X축 Limit & Home = On 상태
                    UsbDeviceDataInfo.nData[UsbDeviceDataInfo.nState] = m_pIntegrated.nAxisLimitSWStatus[UsbDeviceDataInfo.nState];
                }
                else
                {
                    // X축 Limit & Home = Off 상태
                    m_pIntegrated.nAxisLimitSWStatus[IDX_X_AXIS] = 0;
                    bMessage = TRUE;
                    // Axis 설정
                    UsbDeviceDataInfo.nState = IDX_X_AXIS;
                    // X축 Limit & Home = Off 상태
                    UsbDeviceDataInfo.nData[UsbDeviceDataInfo.nState] = m_pIntegrated.nAxisLimitSWStatus[UsbDeviceDataInfo.nState];
                }

                // Y축 Limit & Home
                if ((nEmergency_val & 0x02) != 0)
                {
                    // Y축 Limit & Home = On 상태
                    m_pIntegrated.nAxisLimitSWStatus[IDX_Y_AXIS] = 1;
                    bMessage = TRUE;
                    // Axis 설정
                    UsbDeviceDataInfo.nState = IDX_Y_AXIS;
                    // Y축 Limit & Home = On 상태
                    UsbDeviceDataInfo.nData[UsbDeviceDataInfo.nState] = m_pIntegrated.nAxisLimitSWStatus[UsbDeviceDataInfo.nState];
                }
                else
                {
                    // Y축 Limit & Home = Off 상태
                    m_pIntegrated.nAxisLimitSWStatus[IDX_Y_AXIS] = 0;
                    bMessage = TRUE;
                    // Axis 설정
                    UsbDeviceDataInfo.nState = IDX_Y_AXIS;
                    // Y축 Limit & Home = Off 상태
                    UsbDeviceDataInfo.nData[UsbDeviceDataInfo.nState] = m_pIntegrated.nAxisLimitSWStatus[UsbDeviceDataInfo.nState];
                }

                // Z축 Limit & Home
                if ((nEmergency_val & 0x04) != 0)
                {
                    // Z축 Limit & Home = On 상태
                    m_pIntegrated.nAxisLimitSWStatus[IDX_Z_AXIS] = 1;
                    bMessage = TRUE;
                    // Axis 설정
                    UsbDeviceDataInfo.nState = IDX_Z_AXIS;
                    // Z축 Limit & Home = On 상태
                    UsbDeviceDataInfo.nData[UsbDeviceDataInfo.nState] = m_pIntegrated.nAxisLimitSWStatus[UsbDeviceDataInfo.nState];
                }
                else
                {
                    // Z축 Limit & Home = Off 상태
                    m_pIntegrated.nAxisLimitSWStatus[IDX_Z_AXIS] = 0;
                    bMessage = TRUE;
                    // Axis 설정
                    UsbDeviceDataInfo.nState = IDX_Z_AXIS;
                    // Z축 Limit & Home = Off 상태
                    UsbDeviceDataInfo.nData[UsbDeviceDataInfo.nState] = m_pIntegrated.nAxisLimitSWStatus[UsbDeviceDataInfo.nState];
                }

                // R축 Limit & Home
                if ((nEmergency_val & 0x08) != 0)
                {
                    // R축 Limit & Home = On 상태
                    m_pIntegrated.nAxisLimitSWStatus[IDX_R_AXIS] = 1;
                    bMessage = TRUE;
                    // Axis 설정
                    UsbDeviceDataInfo.nState = IDX_R_AXIS;
                    // R축 Limit & Home = On 상태
                    UsbDeviceDataInfo.nData[UsbDeviceDataInfo.nState] = m_pIntegrated.nAxisLimitSWStatus[UsbDeviceDataInfo.nState];
                }
                else
                {
                    // R축 Limit & Home = Off 상태
                    m_pIntegrated.nAxisLimitSWStatus[IDX_R_AXIS] = 0;
                    bMessage = TRUE;
                    // Axis 설정
                    UsbDeviceDataInfo.nState = IDX_R_AXIS;
                    // R축 Limit & Home = Off 상태
                    UsbDeviceDataInfo.nData[UsbDeviceDataInfo.nState] = m_pIntegrated.nAxisLimitSWStatus[UsbDeviceDataInfo.nState];
                }

                // T축 Limit & Home
                if ((nEmergency_val & 0x10) != 0)
                {
                    // T축 Limit & Home = On 상태
                    m_pIntegrated.nAxisLimitSWStatus[IDX_T_AXIS] = 1;
                    bMessage = TRUE;
                    // Axis 설정
                    UsbDeviceDataInfo.nState = IDX_T_AXIS;
                    // T축 Limit & Home = On 상태
                    UsbDeviceDataInfo.nData[UsbDeviceDataInfo.nState] = m_pIntegrated.nAxisLimitSWStatus[UsbDeviceDataInfo.nState];
                }
                else
                {
                    // T축 Limit & Home = Off 상태
                    m_pIntegrated.nAxisLimitSWStatus[IDX_T_AXIS] = 0;
                    bMessage = TRUE;
                    // Axis 설정
                    UsbDeviceDataInfo.nState = IDX_T_AXIS;
                    // T축 Limit & Home = Off 상태
                    UsbDeviceDataInfo.nData[UsbDeviceDataInfo.nState] = m_pIntegrated.nAxisLimitSWStatus[UsbDeviceDataInfo.nState];
                }


                // B축 Home S/W
                if ((nEmergency_val & 0x20) != 0)
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
                else if ((nEmergency_val & 0x40) != 0)
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
                    // BSE축 Home = Off 상태
                    m_pIntegrated.nAxisLimitSWStatus[IDX_B_AXIS] = 0;
                    bMessage = TRUE;
                    // Axis 설정
                    UsbDeviceDataInfo.nState = IDX_B_AXIS;
                    // BSE축 Home = Off 상태
                    UsbDeviceDataInfo.nData[UsbDeviceDataInfo.nState] = m_pIntegrated.nAxisLimitSWStatus[UsbDeviceDataInfo.nState];
                }


                // Limit & Home Switch 값 정보 전송
                if (bMessage == TRUE)
                {
                    if (m_hOwnerWnd != NULL)
                    {
                        // DEVICE MAIN SERVER
                        // Limit & Home Switch 값 정보 메시지 보내기
                        // 2020.05.04
                        SendMessage(m_hOwnerWnd, UM_USB_SBR_STAGE_LIMIT_STATUS, 0, UsbDeviceDataInfo);
                    }
                }

                // Axis Limit State 표시하기 
                SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, 0, UsbDeviceDataInfo);
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:10071 ----
        //
        // Scan Bridge : "EC" -> Echo Command ( STAGE COMMAND ECHO ) 값 읽기 및 적용
        //
        public int AfxScanBridgeECCommand()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // CNC 상태 및 제어용 윈도우 핸들 확인
            if (m_hStageSetupControlWnd == NULL) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 메시지 처리 전달용 USB Data
            USB_DEVICE_DATA_INFO UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            int bMessage = FALSE;

            // Command 설정 : 'EC' Command
            bMessage = TRUE;
            UsbDeviceDataInfo.nCommand = SBR_CMD_EC;
            strcpy_s(UsbDeviceDataInfo.szData, m_pIntegrated.szXStr);

            // Echo Command 값 정보 전송
            if (bMessage == TRUE)
            {
                if (m_hOwnerWnd != NULL)
                {
                    // DEVICE MAIN SERVER
                    // Echo Command값 정보 메시지 보내기
                    // 2020.05.04
                    //SendMessage(m_hOwnerWnd, UM_USB_SBR_STAGE_ECHO_COMMAND, (WPARAM)0, (LPARAM)&UsbDeviceDataInfo);
                }
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:10106 ----
        //
        // Scan Bridge : "VR" -> H/W Version String 값 읽기 및 적용
        //
        public int AfxScanBridgeVRCommand()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // 메시지 처리 전달용 USB Data
            USB_DEVICE_DATA_INFO UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            int bMessage = FALSE;

            // Command 설정 : 'VR' Command
            bMessage = TRUE;
            UsbDeviceDataInfo.nCommand = SBR_CMD_VR;
            strcpy_s(UsbDeviceDataInfo.szData, m_pIntegrated.szXStr);

            // H/W Version 값 정보 전송
            if (bMessage == TRUE)
            {
                if (m_hOwnerWnd != NULL)
                {
                    // DEVICE MAIN SERVER
                    // H/W Version 값 정보 메시지 보내기
                    // 2020.05.04
                    //SendMessage(m_hOwnerWnd, UM_USB_SBR_HW_VERSION_DATA, (WPARAM)0, (LPARAM)&UsbDeviceDataInfo);
                }
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:10170 ----
        //
        // Scan Bridge : "UL" -> Sample Load / Unload String 값 읽기 및 적용
        //
        // "UL0,00" --> WAIT STEP
        // "UL0,01" --> IN  CHECK STEP
        // "UL0,02" --> OUT CHECK STEP
        // "UL0,03" --> IN_STEP0
        // "UL0,04" --> IN_STEP1
        // "UL0,05" --> IN_STEP2
        // "UL0,06" --> IN_STEP3
        // "UL0,07" --> TIME_STEP
        // "UL0,08" --> OUT_STEP0
        // "UL0,09" --> OUT_STEP1
        // "UL0,10" --> OUT_STEP2
        // "UL0,11" --> OUT_STEP3
        // "UL0,12" --> OUT_STEP4
        //-----------------------------
        // "UL1,00" --> UL_WAIT
        // "UL1,01" --> UL_LOADING
        // "UL1,02" --> UL_UNLOADING
        // "UL1,03" --> UL_LOADED
        // "UL1,04" --> UL_UNLOADED
        // "UL1,05" --> UL_FAIL_IN_STEP0
        // "UL1,06" --> UL_FAIL_IN_STEP1
        // "UL1,07" --> UL_FAIL_IN_STEP2
        // "UL1,08" --> UL_FAIL_IN_STEP3
        // "UL1,09" --> UL_FAIL_OUT_STEP0
        // "UL1,10" --> UL_FAIL_OUT_STEP1
        // "UL1,11" --> UL_FAIL_OUT_STEP2
        // "UL1,12" --> UL_FAIL_OUT_STEP3
        // "UL1,13" --> UL_FAIL_OUT_STEP4
        //-----------------------------
        // "UL2,00" --> REQUEST from PC : STEP  REQUEST
        // "UL2,01" --> REQUEST from PC : STATE REQUEST
        //
        public int AfxScanBridgeULCommand()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // 메시지 처리 전달용 USB Data
            USB_DEVICE_DATA_INFO UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            int bMessage = FALSE;

            // Command 설정 : 'UL' Command
            bMessage = TRUE;
            UsbDeviceDataInfo.nCommand = SBR_CMD_UL;

            // Command
            strcpy_s(UsbDeviceDataInfo.szData, m_pIntegrated.szComStr);
            // X Str Data
            if ((int)strlen(m_pIntegrated.szXStr) > 0)
            {
                strcat_s(UsbDeviceDataInfo.szData, ",");
                strcat_s(UsbDeviceDataInfo.szData, m_pIntegrated.szXStr);
            }
            // Y Str Data
            if ((int)strlen(m_pIntegrated.szYStr) > 0)
            {
                strcat_s(UsbDeviceDataInfo.szData, ",");
                strcat_s(UsbDeviceDataInfo.szData, m_pIntegrated.szYStr);
            }
            // D Str Data
            if ((int)strlen(m_pIntegrated.szDStr) > 0)
            {
                strcat_s(UsbDeviceDataInfo.szData, ",");
                strcat_s(UsbDeviceDataInfo.szData, m_pIntegrated.szDStr);
            }
            // S Str Data
            if ((int)strlen(m_pIntegrated.szSStr) > 0)
            {
                strcat_s(UsbDeviceDataInfo.szData, ",");
                strcat_s(UsbDeviceDataInfo.szData, m_pIntegrated.szSStr);
            }
            strcat_s(UsbDeviceDataInfo.szData, ";");

            // H/W Version 값 정보 전송
            if (bMessage == TRUE)
            {
                if (m_hOwnerWnd != NULL)
                {
                    // DEVICE MAIN SERVER
                    // Sample Load / Unload 값 정보 메시지 보내기
                    // 2021.03.10
                    SendMessage(m_hOwnerWnd, UM_USB_SBR_STAGE_BMP_POSITION, 0, UsbDeviceDataInfo);
                }
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:10229 ----
        //
        // Scan Bridge : Axis All Stop 적용
        //
        public int AfxScanBridgeASCommand()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // X, Y, Z, R, T 축 이동 중지
            CharBuffer szCNCData = new CharBuffer(255 + 1);
            ZeroMem(szCNCData);

            strcpy_s(szCNCData, "ST");

            // VacBoard -> PC -> To CncBoard
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData, 0x1F);

            // VacBoard -> PC -> To CncBoard
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData, 0x00);

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:10251 ----
        //
        // Scan Bridge : Power Off Command 전송 (명령어전송)
        //
        public int AfxScanBridgePowerOff(int nMode)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // PS : Position Save to Eeprom
            // SB : Set Backlash Pulse / Set Exchange Sample Position Pulse 으로 변경   
            //      "SB0,111;"
            //      "SB1,222;"
            //      "SB2,333;"
            //      "SB3,444;"
            //      "SB6;" 을 PC에서 보내면, 현재의 X,Y,Z 위치를 Exchange Sample Position으로 저장  
            switch (nMode)
            {
                case DEF_PROGRAM_EXIT_CANCEL:
                {
                    // 프로그램 종료 취소
                    break;
                }
                case DEF_PROGRAM_EXIT_AXIS:
                {
                    //
                    // HV DISABLED
                    //
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RG0,1;"); // GUN  HV DISABLED
                    Sleep(100);
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RG1,1;"); // PMT  HV DISABLED
                    Sleep(100);
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RG2,1;"); // SCIN HV DISABLED
                    Sleep(100);
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RG3,1;"); // COLL HV DISABLED
                    Sleep(100);

                    //
                    // Scan Bridge 장치가 연결되어 있다면
                    // StageBoard : axis position 만 저장
                    //
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "PS1;");
                    Sleep(100);
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "SB0;");
                    Sleep(100);
                    break;
                }
                case DEF_PROGRAM_EXIT_POWEROFF:
                {
                    //
                    // HV DISABLED
                    //
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RG0,1;"); // GUN  HV DISABLED
                    Sleep(100);
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RG1,1;"); // PMT  HV DISABLED
                    Sleep(100);
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RG2,1;"); // SCIN HV DISABLED
                    Sleep(100);
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RG3,1;"); // COLL HV DISABLED
                    Sleep(100);

                    //
                    // Scan Bridge 장치가 연결되어 있다면
                    // Save axis position & SEM Power Off
                    //
                    // Stage Board : Save axis position
                    //
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "PS1;"); // Sbr Command
                    Sleep(100);
                    //
                    // SEM Power Off ( Shutdown )
                    //
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "PD;");  // Sbr Command
                    Sleep(100);
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "SB0;"); // Sbr Command
                    Sleep(100);
                    //
                    break;
                }
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:10333 ----
        //
        // Scan Bridge : Left<->Right Mirro 이미지 (좌우 반전)
        //
        public int AfxScanBridgeLRMirror(int nMode)
        {
            //if (m_pSbrCommand == NULL) return RET_ERROR_SBR_COMMAND_HANDLE;
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            m_pIntegrated.nLRMirror = nMode;

            if (m_pIntegrated.nLRMirror == 0)
            {
                m_pIntegrated.nSbrCommand |= XSCAN_INVERSE_LOW;
            }
            else
            {
                m_pIntegrated.nSbrCommand |= XSCAN_INVERSE_HIGH;
            }

            // 단일 명령어 및 데이터 전송
            AfxFT232HPortSendCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, cWR_OUTPORT, m_pIntegrated.nSbrCommand); // Sbr Command

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:10358 ----
        //
        // Scan Bridge : Top<->Bottom Mirro 이미지 (상하 반전)
        //
        public int AfxScanBridgeTBMirror(int nMode)
        {
            //if (m_pSbrCommand == NULL) return RET_ERROR_SBR_COMMAND_HANDLE;
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            m_pIntegrated.nTBMirror = nMode;

            if (m_pIntegrated.nTBMirror == 0)
            {
                m_pIntegrated.nSbrCommand |= YSCAN_INVERSE_LOW;
            }
            else
            {
                m_pIntegrated.nSbrCommand |= YSCAN_INVERSE_HIGH;
            }

            // 단일 명령어 및 데이터 전송
            AfxFT232HPortSendCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, cWR_OUTPORT, m_pIntegrated.nSbrCommand); // Sbr Command

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:10383 ----
        //
        // Scan Bridge : XY Magnification Image Calibration -> X(Hor)-Amp 값 설정
        //
        public int AfxScanBridgeSetXImageAmp()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // X Image Amp
            // 문자 명령어 및 데이터 전송
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "SR6,", m_pIntegrated.nXScanAmpVal[m_pIntegrated.nScanMode]); // X Scan Amp

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:10393 ----
        public int AfxScanBridgeSetXImageAmpApply(int nAmp)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // X Image Amp
            // 문자 명령어 및 데이터 전송
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "SR6,", nAmp); // X Scan Amp

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:10403 ----
        public int AfxScanBridgeSetXImageAmp(int nXImageAmp)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // X Image Amp
            m_pIntegrated.nXImageAmp = nXImageAmp;
            m_pIntegrated.nXScanAmpVal[m_pIntegrated.nScanMode] = m_pIntegrated.nXImageAmp;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:10417 ----
        //
        // Scan Bridge : XY Magnification Image Calibration -> X(Hor)-Offset 값 설정
        //
        public int AfxScanBridgeSetXImageOffset()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // X Image Offset
            // 문자 명령어 및 데이터 전송
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "SR7,", m_pIntegrated.nXScanOffset[m_pIntegrated.nScanMode]); // X Scan Offset

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:10427 ----
        public int AfxScanBridgeSetXImageOffsetApply(int nOffset)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // X Image Offset
            // 문자 명령어 및 데이터 전송
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "SR7,", nOffset); // X Scan Offset

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:10437 ----
        public int AfxScanBridgeSetXImageOffset(int nXImageOffset)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // X Image Offset
            m_pIntegrated.nXImageOffset = nXImageOffset;
            m_pIntegrated.nXScanOffset[m_pIntegrated.nScanMode] = m_pIntegrated.nXImageOffset;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:10451 ----
        //
        // Scan Bridge : XY Magnification Image Calibration -> Y(Ver)-Amp 값 설정
        //
        public int AfxScanBridgeSetYImageAmp()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Y Image Amp
            // 문자 명령어 및 데이터 전송
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "SR8,", m_pIntegrated.nYScanAmpVal[m_pIntegrated.nScanMode]); // Y Scan Amp

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:10461 ----
        public int AfxScanBridgeSetYImageAmpApply(int nAmp)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Y Image Amp
            // 문자 명령어 및 데이터 전송
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "SR8,", nAmp); // Y Scan Amp

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:10471 ----
        public int AfxScanBridgeSetYImageAmp(int nYImageAmp)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Y Image Amp
            m_pIntegrated.nYImageAmp = nYImageAmp;
            m_pIntegrated.nYScanAmpVal[m_pIntegrated.nScanMode] = m_pIntegrated.nYImageAmp;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:10485 ----
        //
        // Scan Bridge : XY Magnification Image Calibration -> Y(Ver)-Offset 값 설정
        //
        public int AfxScanBridgeSetYImageOffset()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Y Image Offset
            // 문자 명령어 및 데이터 전송
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "SR9,", m_pIntegrated.nYScanOffset[m_pIntegrated.nScanMode]); // Y Scan Offset

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:10495 ----
        public int AfxScanBridgeSetYImageOffsetApply(int nOffset)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Y Image Offset
            // 문자 명령어 및 데이터 전송
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "SR9,", nOffset); // Y Scan Offset

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:10505 ----
        public int AfxScanBridgeSetYImageOffset(int nYImageOffset)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Y Image Offset
            m_pIntegrated.nYImageOffset = nYImageOffset;
            m_pIntegrated.nYScanOffset[m_pIntegrated.nScanMode] = m_pIntegrated.nYImageOffset;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:10519 ----
        //
        // Scan Bridge :  Frame/sec 값에 따라 ScanClk의 주파수만 바꾼다.
        //
        public int AfxScanBridgeSetScanClockFramePerSec()
        {
            // Scan Clock(주파수) 변경 및 X,YScan 기본파형 발생
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            int nSM = m_pIntegrated.nScanMode; // Scan Mode

            //m_pIntegrated->nFrameSecVal[nSM]  = m_pIntegrated->nFrameSec[nSM];  // Frame/sec
            //m_pIntegrated->nXScanUpNum[nSM]   = m_pIntegrated->nXScanUp;        // X Scan Up
            //m_pIntegrated->nXScanDownNum[nSM] = m_pIntegrated->nXBlankWidth;    // X Scan Down
            //m_pIntegrated->nYScanUpNum[nSM]   = m_pIntegrated->nYScanUp;        // Y Scan Up
            //m_pIntegrated->nYScanDownNum[nSM] = m_pIntegrated->nYBlankWidth;    // Y Scan Down

            ushort[] wCom_Buf = NewArray<ushort>(TMP_STRING_LENGTH+1);
            ZeroMem(wCom_Buf);

            double dFrame  = 0.0;
            int nPhase_inc = 0;
            int nScan_clk  = 0;

            long nPhase_inc64 = 0;

            //--
            // Scan Clock 변경
            //--
            dFrame = (double)(m_pIntegrated.nFrameSecVal[nSM]) * 0.001;
            dFrame = (double)(m_pIntegrated.nXScanUpNum[nSM] + m_pIntegrated.nXScanDownNum[nSM]) * (double)(m_pIntegrated.nYScanUpNum[nSM] + m_pIntegrated.nYScanDownNum[nSM]) * dFrame;
            nScan_clk = (int)dFrame;

            //
            // 2021.07.29
            // SCAN CLOCK  ( 32Bits, 48MHz ) --> ( 48Bits, 100MHz) 로 변경하여 계산 적용
            //
            if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_SCAN_CLOCK_48] == 1)
            {
                // SCAN CLOCK 48Bits, 100MHz
                //--
                // 2021.07.29
                // 추가옵션사항 : ( ScanClk * 2^32 / 48[MHz] ) --> ( ScanClk * 2^48 / 100[MHz] )
                //--
                nPhase_inc64 = (long)(nScan_clk * (Exp2E48 / 100000000));
                wCom_Buf[0] = (ushort)(cWR_FDDS_LOW);      wCom_Buf[1] = (ushort)( (ushort)(nPhase_inc64 & 0x000000000000FFFF)); nPhase_inc64 = nPhase_inc64 >> 16;
                wCom_Buf[2] = (ushort)(cWR_FDDS_HIGH);     wCom_Buf[3] = (ushort)( (ushort)(nPhase_inc64 & 0x000000000000FFFF)); nPhase_inc64 = nPhase_inc64 >> 16;
                wCom_Buf[4] = (ushort)(cWR_FDDS_HIGH48);   wCom_Buf[5] = (ushort)( (ushort)(nPhase_inc64 & 0x000000000000FFFF));
                //--
                AfxFT232HPortSendMultiCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, wCom_Buf, 6); // Sbr Scan Clock Data 전송
                //--
            }
            //
            //
            //
            else
            {
                // SCAN CLOCK 32Bits, 48MHz
                nPhase_inc64 = (long)(nScan_clk * Exp2E32 / 48000000);
                nPhase_inc = (int)(nPhase_inc64);
                //--
                wCom_Buf[0] = (ushort)(cWR_FDDS_LOW);     wCom_Buf[1] = (ushort)(nPhase_inc & 0x0000FFFF);
                wCom_Buf[2] = (ushort)(cWR_FDDS_HIGH); wCom_Buf[3] = (ushort)(nPhase_inc >> 16);
                //--
                AfxFT232HPortSendMultiCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, wCom_Buf, 4); // Sbr Scan Clock Data 전송
                //--
            }
            //
            //
            //

            // Display
            //ScanClkEdit->Text = IntToStr(scan_clk);
            //LineFreqEdit->Text = IntToStr((YScanUpNum[sm] + YScanDownNum[sm]) *  FrameSec[sm] / 1000);

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:10597 ----
        //
        // Scan Bridge : GUN HV SMPS ON
        //
        public int AfxScanBridgeGunHvSMPSOn()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // HV SMPS ON
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "HV1;");   // Sbr Data 전송
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "SB5,1;"); // ScanBridgeBoard -> CncVacBoard (EM40)

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:10611 ----
        //
        // Scan Bridge : GUN HV SMPS OFF
        //
        public int AfxScanBridgeGunHvSMPSOff()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // HV SMPS OFF
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "HV0;");   // Sbr Data 전송
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "SB5,0;"); // ScanBridgeBoard -> CncVacBoard (EM40)

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:10625 ----
        //
        // Scan Bridge : GUN HV ON (30kV, 3.5kV, 4V)
        //
        public int AfxScanBridgeGunHVOn()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // GUN VALVE : OPEN
            //AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "HV1;");   // Sbr Data 전송

            // SpellMan GUN HV ON
            AfxScanBridgeSpellmanGunHVOn();

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:10637 ----
        public int AfxScanBridgeSpellmanGunHVOn()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // SpellMan GUN HV ON
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RG0,0;"); // Sbr Data 전송

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:10650 ----
        //
        // Scan Bridge : GUN HV OFF (30kV, 3.5kV, 4V)
        //
        public int AfxScanBridgeGunHVOff()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // GUN VALVE : CLOSE
            //AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "HV0;");   // Sbr Data 전송

            // SpellMan GUN HV OFF
            AfxScanBridgeSpellmanGunHVOff();

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:10662 ----
        public int AfxScanBridgeSpellmanGunHVOff()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // SpellMan GUN HV OFF
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RG0,1;"); // Sbr Data 전송

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:10675 ----
        //
        // Scan Bridge : Set GUN HV Index Value (GUN HV 인덱스를 이용한 전압(HV)값 설정하기)
        //
        public int AfxScanBridgeSetGunHVIndex(int nIndex)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // GUN HV 값 설정하기
            // GUN HV : 32개, 30[kV] ~ 0.5[kV]
            // "AV"에 값을 설정하기 위해서는 원하는 GunHV에 나누기 10을 사용하여 설정한다.
            // 예) 30kV -> 3000, 20kV -> 2000, 1000V -> 100, 500 -> 50 으로 설정.
            int    nValue   = m_nGunHVTable[nIndex];
            double dVoltage = (double)(nValue / 10);
            int    nVoltage = (int)(dVoltage);
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "AV", nVoltage);

            // DEVICE SERVER 윈도우
            if (m_hOwnerWnd != NULL)
            {
                USB_DEVICE_DATA_INFO UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
                UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
                UsbDeviceDataInfo.nState = 1;       // Gun HV Index 사용
                UsbDeviceDataInfo.nData[0] = nIndex;// m_pIntegrated->dGunHVCurVoltage;
                UsbDeviceDataInfo.nData[1] = nValue;// m_pIntegrated->dGunHVCurVoltage;
                // GUN HV Up/Down Process 메시지 보내기
                // 2020.05.04
                //SendMessage(m_hOwnerWnd, UM_USB_SBR_GUN_HV_DATA, NULL, (LPARAM)&UsbDeviceDataInfo);
            }

            // Emission current[µA] Setting
            double dMaxGunHV = (double)m_nGunHVTable[0];
            double dGunHV    = (double)nVoltage;
            double dRatio    = sqrt(dGunHV / dMaxGunHV);          // 실행할(목표값) Gun HV Voltage / MAX GUN HV Voltage -> 비율
            ushort   wECVal    = 0;

            // Emission current[µA] 계산
            m_pIntegrated.dHVRatio = dRatio;
            wECVal = (ushort)((double)(4095 * m_pIntegrated.dHVRatio));
            wECVal /= 2;

            // Emission current[µA] 실행
            AfxIntegratedSetGunHVECValue(wECVal);

            // GUN HV 배율 비율 계산
            m_pIntegrated.dMagHVRatio = sqrt(dGunHV / (double)dMaxGunHV) / DEFAULT_MAG_HV_RATIO; // 0.8165;
            // 2020.06.03
            // 사용자 MAG HV RATIO : STAGE 수평 옵션을 임시로 사용한다.
            if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_STAGE_HORZ_ALIGN] == 1)
            {
                // GUN HV : 20KV
                if ((int)dGunHV == 20000)
                {
                    m_pIntegrated.dMagHVRatio = sqrt(dGunHV / dMaxGunHV) / DEFAULT_MAG_HV_RATIO;
                }
                else
                {
                    // 사용자 설정 배율 HV RATIO 보정값 설정
                    m_pIntegrated.dMagHVRatio = m_dOptionMagHVRatio[m_pIntegrated.nGunHVIndex]; //sqrt(dGunHV / dMaxGunHV) / m_dOptionMagHVRatio[m_pIntegrated->nGunHVIndex];// OPTION_MAG_HV_RATIO;
                }
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:10735 ----
        public int AfxScanBridgeSetGunHVIndexExecute(int nIndex)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // GUN HV 값 설정하기
            // GUN HV : 32개, 30[kV] ~ 0.5[kV]
            // "AV"에 값을 설정하기 위해서는 원하는 GunHV에 나누기 10을 사용하여 설정한다.
            // 예) 30kV -> 3000, 20kV -> 2000, 1000V -> 100, 500 -> 50 으로 설정.
            int    nValue   = m_nGunHVTable[nIndex];
            double dVoltage = (double)(nValue / 10);
            int    nVoltage = (int)(dVoltage);
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "AV", nVoltage);

            // Emission current[µA] Setting
            double dMaxGunHV = (double)m_nGunHVTable[0];
            double dGunHV    = (double)nVoltage;
            double dRatio    = sqrt(dGunHV / dMaxGunHV);          // 실행할(목표값) Gun HV Voltage / MAX GUN HV Voltage -> 비율
            ushort   wECVal    = 0;

            // Emission current[µA] 계산
            m_pIntegrated.dHVRatio = dRatio;
            wECVal = (ushort)((double)(4095 * m_pIntegrated.dHVRatio));
            wECVal /= 2;

            // Emission current[µA] 실행
            AfxIntegratedSetGunHVECValue(wECVal);

            // GUN HV 배율 비율 계산
            m_pIntegrated.dMagHVRatio = sqrt(dGunHV / (double)dMaxGunHV) / DEFAULT_MAG_HV_RATIO; // 0.8165;
            // 2020.06.03
            // 사용자 MAG HV RATIO : STAGE 수평 옵션을 임시로 사용한다.
            if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_STAGE_HORZ_ALIGN] == 1)
            {
                // GUN HV : 20KV
                if ((int)dGunHV == 20000)
                {
                    m_pIntegrated.dMagHVRatio = sqrt(dGunHV / dMaxGunHV) / DEFAULT_MAG_HV_RATIO;
                }
                else
                {
                    // 사용자 설정 배율 HV RATIO 보정값 설정
                    m_pIntegrated.dMagHVRatio = m_dOptionMagHVRatio[m_pIntegrated.nGunHVIndex]; //sqrt(dGunHV / dMaxGunHV) / m_dOptionMagHVRatio[m_pIntegrated->nGunHVIndex];// OPTION_MAG_HV_RATIO;
                }
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:10786 ----
        //
        // Scan Bridge : Set GUN HV Value (GUN HV 값 설정하기)
        //
        public int AfxScanBridgeSetGunHVValue(int nValue)
        {
            //if (m_pSbrCommand == NULL) return RET_ERROR_SBR_COMMAND_HANDLE;
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // GUN HV 값 설정하기
            // GUN HV : 32개, 30[kV] ~ 0.5[kV]
            // "AV"에 값을 설정하기 위해서는 원하는 GunHV에 나누기 10을 사용하여 설정한다.
            // 예) 30kV -> 3000, 20kV -> 2000, 1000V -> 100, 500 -> 50 으로 설정.
            double dVoltage = (double)(nValue / 10);
            int    nVoltage = (int)(dVoltage);
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "AV", nVoltage);

            // Emission current[µA] Setting
            double dMaxGunHV = (double)m_nGunHVTable[0];
            double dGunHV    = (double)nVoltage;
            double dRatio    = sqrt(dGunHV / dMaxGunHV);          // 실행할(목표값) Gun HV Voltage / MAX GUN HV Voltage -> 비율
            ushort   wECVal    = 0;

            // Emission current[µA] 계산
            m_pIntegrated.dHVRatio = dRatio;
            wECVal = (ushort)((double)(4095 * m_pIntegrated.dHVRatio));
            wECVal /= 2;

            // Emission current[µA] 실행
            AfxIntegratedSetGunHVECValue(wECVal);

            // GUN HV 배율 비율 계산
            m_pIntegrated.dMagHVRatio = sqrt(dGunHV / (double)dMaxGunHV) / DEFAULT_MAG_HV_RATIO;
            // 2020.06.03
            // 사용자 MAG HV RATIO : STAGE 수평 옵션을 임시로 사용한다.
            if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_STAGE_HORZ_ALIGN] == 1)
            {
                // GUN HV : 20KV
                if ((int)dGunHV == 20000)
                {
                    m_pIntegrated.dMagHVRatio = sqrt(dGunHV / dMaxGunHV) / DEFAULT_MAG_HV_RATIO;
                }
                else
                {
                    // 사용자 설정 배율 HV RATIO 보정값 설정
                    m_pIntegrated.dMagHVRatio = m_dOptionMagHVRatio[m_pIntegrated.nGunHVIndex]; //sqrt(dGunHV / dMaxGunHV) / m_dOptionMagHVRatio[m_pIntegrated->nGunHVIndex];// OPTION_MAG_HV_RATIO;
                }
            }

            // DEVICE SERVER 윈도우
            if (m_hOwnerWnd != NULL)
            {
                USB_DEVICE_DATA_INFO UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
                UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
                UsbDeviceDataInfo.nState   = 2;     // Gun HV Voltage 사용
                UsbDeviceDataInfo.nData[0] = -1;    // m_pIntegrated->dGunHVCurVoltage;
                UsbDeviceDataInfo.nData[1] = nValue;// m_pIntegrated->dGunHVCurVoltage;
                // GUN HV Up/Down Process 메시지 보내기
                // 2020.05.04
                //SendMessage(m_hOwnerWnd, UM_USB_SBR_GUN_HV_DATA, NULL, (LPARAM)&UsbDeviceDataInfo);
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:10846 ----
        public int AfxScanBridgeSetGunHVValue(double dValue, int bCalc = FALSE)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // GUN HV 값 설정하기
            // GUN HV : 32개, 30[kV] ~ 0.5[kV]
            // "AV"에 값을 설정하기 위해서는 원하는 GunHV에 나누기 10을 사용하여 설정한다.
            // 예) 30kV -> 3000, 20kV -> 2000, 1000V -> 100, 500 -> 50 으로 설정.
            double dVoltage = (double)(dValue / 10.0);
            int    nVoltage = (int)(dVoltage);
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "AV", nVoltage);

            //
            // GUN HV Ratio 계산값
            //
            if (bCalc == TRUE)
            {
                if (nVoltage < 1) return RET_SUCCESS_OK;      // 0[v] 이면 리턴
                double dMaxGunHV = m_nGunHVTable[0];          // MAX GUN HV VOLTAGE : m_nGunHVTable[0];
                double dGunHV    = dValue;                    // Gun HV Voltage
                double dRatio    = sqrt(dGunHV / dMaxGunHV);  // Gun HV Voltage / MAX GUN HV Voltage -> 비율
                // GUN HV Index 설정
                int nGunHV = (int)dGunHV;
                int nIndex = -1;
                for (int n = 0; n < MAX_GUN_HV_TABLE; n++)
                {
                    if (nGunHV == m_nGunHVTable[n])
                    {
                        nIndex = n;
                        break;
                    }
                }
                // GUN HV Index 찾음
                if (nIndex > -1)
                {
                    m_pIntegrated.nGunHVIndex    = nIndex;
                    m_pIntegrated.nGunHVValue    = nGunHV;
                    m_pIntegrated.nGunHVCurIndex = nIndex;
                    m_pIntegrated.nGunHVCurValue = nGunHV;
                    m_pIntegrated.nGunHVExeIndex = nIndex;
                    m_pIntegrated.nGunHVExeValue = nIndex;
                }

                // Emission current[µA] Setting
                ushort   wECVal = 0;

                // Emission current[µA] 계산
                m_pIntegrated.dHVRatio = dRatio;
                wECVal = (ushort)((double)(4095 * m_pIntegrated.dHVRatio));
                wECVal /= 2;

                // Emission current[µA] 실행
                AfxIntegratedSetGunHVECValue(wECVal);

                // GUN HV 배율 비율 계산
                m_pIntegrated.dMagHVRatio = sqrt(dGunHV / dMaxGunHV) / DEFAULT_MAG_HV_RATIO;
                // 2020.06.03
                // 사용자 MAG HV RATIO : STAGE 수평 옵션을 임시로 사용한다.
                if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_STAGE_HORZ_ALIGN] == 1)
                {
                    // GUN HV : 20KV
                    if (nGunHV == 20000)
                    {
                        m_pIntegrated.dMagHVRatio = sqrt(dGunHV / dMaxGunHV) / DEFAULT_MAG_HV_RATIO;
                    }
                    else
                    {
                        // 사용자 설정 배율 HV RATIO 보정값 설정
                        m_pIntegrated.dMagHVRatio = m_dOptionMagHVRatio[m_pIntegrated.nGunHVIndex]; //sqrt(dGunHV / dMaxGunHV) / m_dOptionMagHVRatio[m_pIntegrated->nGunHVIndex];// OPTION_MAG_HV_RATIO;
                    }
                }
            }
            //

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:10925 ----
        //
        // Scan Bridge : Set GUN HV Voltage Value (GUN HV 전압(HV)값 실행)
        //
        public int AfxScanBridgeSetGunHVValueVoltage(int nVoltageValue = 0)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // GUN HV 값 설정하기
            // GUN HV : 32개, 30[kV] ~ 0.5[kV]
            // "AV"에 값을 설정하기 위해서는 원하는 GunHV에 나누기 10을 사용하여 설정한다.
            // 예) 30kV -> 3000, 20kV -> 2000, 1000V -> 100, 500 -> 50 으로 설정.
            int    nValue = nVoltageValue;
            double dVoltage = (double)(nValue / 10);
            int    nVoltage = (int)(dVoltage);
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "AV", nVoltage);

            // Emission current[µA] Setting
            double dMaxGunHV = (double)m_nGunHVTable[0];
            double dGunHV    = (double)nVoltage;
            double dRatio    = sqrt(dGunHV / dMaxGunHV);          // 실행할(목표값) Gun HV Voltage / MAX GUN HV Voltage -> 비율
            ushort   wECVal    = 0;

            // Emission current[µA] 계산
            m_pIntegrated.dHVRatio = dRatio;
            wECVal = (ushort)((double)(4095 * m_pIntegrated.dHVRatio));
            wECVal /= 2;

            // Emission current[µA] 실행
            AfxIntegratedSetGunHVECValue(wECVal);

            // GUN HV 배율 비율 계산
            m_pIntegrated.dMagHVRatio = sqrt(dGunHV / (double)dMaxGunHV) / DEFAULT_MAG_HV_RATIO;
            // 2020.06.03
            // 사용자 MAG HV RATIO : STAGE 수평 옵션을 임시로 사용한다.
            if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_STAGE_HORZ_ALIGN] == 1)
            {
                // GUN HV : 20KV
                if ((int)dGunHV == 20000)
                {
                    m_pIntegrated.dMagHVRatio = sqrt(dGunHV / dMaxGunHV) / DEFAULT_MAG_HV_RATIO;
                }
                else
                {
                    // 사용자 설정 배율 HV RATIO 보정값 설정
                    m_pIntegrated.dMagHVRatio = m_dOptionMagHVRatio[m_pIntegrated.nGunHVIndex]; //sqrt(dGunHV / dMaxGunHV) / m_dOptionMagHVRatio[m_pIntegrated->nGunHVIndex];// OPTION_MAG_HV_RATIO;
                }
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:10976 ----
        //
        // Scan Bridge : GUN VALVE OPEN
        //
        public int AfxScanBridgeGunValveOpen()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // GUN VALVE OPEN
            m_pIntegrated.nGunValveState = 1;
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "HV1;");   // Sbr Data 전송

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:10990 ----
        //
        // Scan Bridge : GUN VALVE CLOSE
        //
        public int AfxScanBridgeGunValveClose()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // GUN VALVE CLOSE
            m_pIntegrated.nGunValveState = 0;
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "HV0;");   // Sbr Data 전송

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:11004 ----
        //
        // Scan Bridge : FILAMENT IMAGE SCAN START
        //
        public int AfxScanBridgeFilamentImageScanStart()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // FILAMENT IMAGE SCAN START
            //m_pIntegrated->nFilamentImageScanState = 1;
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "FL0,1;");   // Sbr Data 전송

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:11018 ----
        //
        // Scan Bridge : FILAMENT IMAGE SCAN STOP
        //
        public int AfxScanBridgeFilamentImageScanStop()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // FILAMENT IMAGE SCAN STOP
            //m_pIntegrated->nFilamentImageScanState = 0;
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "FL0,0;");   // Sbr Data 전송

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:11032 ----
        //
        // Scan Bridge : Set BIAS Value (BIAS 값 설정하기)
        //
        public int AfxScanBridgeSetBiasValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // BIAS 값 설정하기
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "BV", m_pIntegrated.nBiasExeValue);

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:11041 ----
        public int AfxScanBridgeSetBiasValueExecute()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // BIAS 값 설정하기
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "BV", m_pIntegrated.nBiasExeValue);

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:11050 ----
        public int AfxScanBridgeSetBiasValue(int nBias)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // BIAS 값 설정하기
            m_pIntegrated.nBiasValue    = nBias;
            m_pIntegrated.nBiasCurValue = nBias;
            m_pIntegrated.nBiasExeValue = nBias;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:11062 ----
        // Scan Bridge : Set BIAS Value (BIAS [V]값 설정하기)
        public int AfxScanBridgeSetBiasVoltageValue(double dVoltage = 0)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // 범위 : 0[v] ~ 3.5[v] -> 0 ~ 4095
            double dVUnit  = (double)(SEM_BIAS_MAX+1) / SEM_BIAS_PROG_MAX;
            double d1Point  = dVUnit / 1.0;
            double dBVCount = dVoltage / d1Point;
            int    nBVCount = (int)dBVCount;

            if (nBVCount < SEM_BIAS_MIN) nBVCount = SEM_BIAS_MIN;
            if (nBVCount > SEM_BIAS_MAX) nBVCount = SEM_BIAS_MAX;

            // BIAS 값 설정하기
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "BV", nBVCount);

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:11080 ----
        public int AfxScanBridgeGetBiasValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // BIAS 값 리턴
            return m_pIntegrated.nBiasValue;
        }

        // ---- USB_FT232H_Device.cpp:11091 ----
        //
        // Scan Bridge : Set Filament Value (필라멘트 값 설정하기)
        //
        public int AfxScanBridgeSetFilamentValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Filament 값 설정하기
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "FV", m_pIntegrated.nFilamentExeValue);

            // DEVICE SERVER 윈도우
            if (m_hOwnerWnd != NULL)
            {
                // FILAMENT DATA 메시지 보내기
                // 2020.05.04
                //SendMessage(m_hOwnerWnd, UM_USB_SBR_FILAMENT_DATA, NULL, (LPARAM)NULL);
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:11108 ----
        public int AfxScanBridgeSetFilamentValueExecute()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Filament 값 적용하기
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "FV", m_pIntegrated.nFilamentExeValue);

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:11117 ----
        public int AfxScanBridgeSetFilamentValue(int nFilament)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Filament 값 설정하기
            m_pIntegrated.nFilamentValue    = nFilament;
            m_pIntegrated.nFilamentCurValue = nFilament;
            m_pIntegrated.nFilamentExeValue = nFilament;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:11129 ----
        // Scan Bridge : Set Filament Voltage Value (필라멘트 [V]값 설정하기)
        public int AfxScanBridgeSetFilamentVoltageValue(double dVoltage = 0.0)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // 범위 : 0[v] ~ 4[v] -> 0 ~ 4095
            double dVUnit   = (double)(SEM_FILAMENT_MAX+1) / SEM_FILAMENT_PROG_MAX;
            double d1Point  = dVUnit / 1.0;
            double dFVCount = dVoltage / d1Point;
            int    nFVCount = (int)dFVCount;

            if (nFVCount < SEM_FILAMENT_MIN) nFVCount = SEM_FILAMENT_MIN;
            if (nFVCount > SEM_FILAMENT_MAX) nFVCount = SEM_FILAMENT_MAX;

            // Filament 값 설정하기
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "FV", nFVCount);

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:11147 ----
        public int AfxScanBridgeGetFilamentValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Filament 값 리턴
            return m_pIntegrated.nFilamentValue;
        }

        // ---- USB_FT232H_Device.cpp:11158 ----
        //
        // Scan Bridge : SpellMan Collector ON
        //
        public int AfxScanBridgeCollectorOn()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // SpellMan Collector On
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RG3,0;"); // Sbr Data 전송

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:11171 ----
        //
        // Scan Bridge : SpellMan Collector OFF
        //
        public int AfxScanBridgeCollectorOff()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // SpellMan Collector On
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RG3,1;"); // Sbr Data 전송

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:11184 ----
        //
        // Scan Bridge : Manification Relay Calibration -> Zoom X Control
        //
        public int AfxScanBridgeSetZoomXControl()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Zoom X Control    
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "ZM0,", m_pIntegrated.nZoomXValue); // Sbr Data 전송
                                                                                  
            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:11193 ----
        public int AfxScanBridgeSetZoomXControl(int nZoomXValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Zoom X Control Value
            m_pIntegrated.nZoomXValue = nZoomXValue;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:11202 ----
        public int AfxScanBridgeGetZoomXControl()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Zoom X Control Value
            return m_pIntegrated.nZoomXValue;
        }

        // ---- USB_FT232H_Device.cpp:11213 ----
        //
        // Scan Bridge : Manification Relay Calibration -> Zoom Y Control
        //
        public int AfxScanBridgeSetZoomYControl()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Zoom Y Control
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "ZM1,", m_pIntegrated.nZoomYValue); // Sbr Data 전송

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:11222 ----
        public int AfxScanBridgeSetZoomYControl(int nZoomYValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Zoom Y Control Value
            m_pIntegrated.nZoomYValue = nZoomYValue;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:11231 ----
        public int AfxScanBridgeGetZoomYControl()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Zoom Y Control Value
            return m_pIntegrated.nZoomYValue;
        }

        // ---- USB_FT232H_Device.cpp:11242 ----
        //
        // Scan Bridge :  Dynamic Focus A, B
        //
        public int AfxScanBridgeSetDynamicFocusAB(int nFocusValue)
        {
            //if (m_pSbrCommand == NULL) return RET_ERROR_SBR_COMMAND_HANDLE;
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Device 설정(수정)이 가능한지 확인...
            //if (m_pIntegrated->bHWChangeEnableFlag == false) return RET_ERROR_STATUS; // 일반적 오류 코드

            // Dynamic Focus A
            m_pIntegrated.nDynamicFocusA = nFocusValue;
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "DF0,", m_pIntegrated.nDynamicFocusA); // Sbr Data 전송

            // Dynamic Focus B
            m_pIntegrated.nDynamicFocusB = nFocusValue;
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "DF1,", m_pIntegrated.nDynamicFocusB); // Sbr Data 전송

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:11264 ----
        //
        // Scan Bridge :  Sync On
        //
        public int AfxScanBridgeSetSyncOn()
        {
            //
            // AC Sync Mode
            //
            // AC sync_scan_on을 off로 설정하므로,
            // AC 전원 Sync 신호가 인가되지 않아도 scan을 진행하게 된다.
            //
            // AC sync_scan_on을 on으로 설정하면
            // AC 전원 Sync 신호가 인가되어야 scan을 진행하게 된다.
            //
            // Frame Sync 또는 Line Sync 기능과 AC Sync Freq 정보를 설정하는
            // 파라미터 값은 m_pIntegrated->nSyncVal[sm] 초기 변수값에 저장되어 있으므로
            // ScanMode가 변경될 때마다, m_CommandState 값에 설정된다.
            //

            //if (m_pSbrCommand == NULL) return RET_ERROR_SBR_COMMAND_HANDLE;
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            if (m_pIntegrated.bAcSyncOn == TRUE)
            {
                // Sync On
                m_pIntegrated.nSbrCommand |= SYNC_SCAN_ON_HIGH;
            }
            else
            {
                // Sync Off
                m_pIntegrated.nSbrCommand &= SYNC_SCAN_ON_LOW;
            }

            // 단일 명령어 및 데이터 전송
            AfxFT232HPortSendCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, cWR_OUTPORT, m_pIntegrated.nSbrCommand); // Sbr Command

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:11299 ----
        public int AfxScanBridgeSetSyncOn(int bAcSyncOn)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // AC Sync On/Off
            m_pIntegrated.bAcSyncOn = bAcSyncOn;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:11308 ----
        public int AfxScanBridgeGetSyncOn()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // AC Sync On/Off Flag
            return m_pIntegrated.bAcSyncOn;
        }

        // ---- USB_FT232H_Device.cpp:11319 ----
        //
        // Scan Bridge :  Orthogonal value set (직교도)
        //
        public int AfxScanBridgeSetOrthogonal()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Orthogonal(직교도) 설정
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "SR5,", m_pIntegrated.nOrthogonal); // Sbr Data 전송

            // Orthogonal(직교도) 설정
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "SR4,", m_pIntegrated.nOrthogonal); // Sbr Data 전송

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:11334 ----
        //
        // Scan Bridge :  Orthogonal value set (직교도)
        //
        public int AfxScanBridgeSetOrthogonal(int nOrthogonal)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Orthogonal(직교도) 설정
            m_pIntegrated.nOrthogonal = nOrthogonal;
            m_pIntegrated.nARAngle[0] = m_pIntegrated.nOrthogonal;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:11347 ----
        //
        // Scan Bridge :  Get Orthogonal Value (직교도)
        //
        public int AfxScanBridgeGetOrthogonal()
        {
            if (m_pIntegrated == null) return 0;  // Integrated Command & Data Handle

            // Orthogonal(직교도)
            return m_pIntegrated.nARAngle[0];// m_pIntegrated->nOrthogonal;
        }

        // ---- USB_FT232H_Device.cpp:11358 ----
        //
        // Scan Bridge : Scan Relay -> Gun Scan (Filament Scan)
        //
        public int AfxScanBridgeSetGunScanRelay(int bGunScanCheck)
        {
            //if (m_pSbrCommand == NULL) return RET_ERROR_SBR_COMMAND_HANDLE;
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            m_pIntegrated.bGunScanCheck = bGunScanCheck;

            if (m_pIntegrated.bGunScanCheck == TRUE)
            {
                // Gun Scan Check : TRUE
                m_pIntegrated.nGunScanRelay = 1;
            }
            else
            {
                // Gun Scan Check : FALSE
                m_pIntegrated.nGunScanRelay = 0;
            }

            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "SR10,", m_pIntegrated.nGunScanRelay); // Sbr Data 전송

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:11380 ----
        public int AfxScanBridgeGetGunScanRelay()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            return m_pIntegrated.bGunScanCheck;
        }

        // ---- USB_FT232H_Device.cpp:11391 ----
        //
        // Scan Bridge : Scan Rotation (degree x 10) : 입력범위 0 ~ 3599, 0도~360도 10등분
        //
        // 2020.05.18
        public int AfxScanBridgeSetScanRotation(int nDegreex10)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            //--
            // 1) Scan Rotation 값 ( SRotationValue )
            //--
            // deg x 10
            //--
            int nNewDegreex10 = nDegreex10;

            //--
            // 2) Stage 수평값 적용(Stage와 Scan 방향을 일치시키기 위한 값)
            //--
            //if (m_pIntegrated->nScanOptionData[IDX_HWSETUP_OPTION_INT_STAGE_HORZ_ALIGN] == 1)
            //{
            //    nNewDegreex10 += m_pIntegrated->nStageHorAlign;
            //}
            //--

            //--
            // 3) Focus OL 값 변경에 따른 SCAN ROTATION(영상회전) 값 보정
            //--
            if(m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_WD_SCAN_ROTATION] == 1)
            {
                // WD에 따른 SCAN ROTATION 보정값 : -3600 ~ 0 ~ +3600
                m_pIntegrated.nWDSRotationValue = m_pIntegrated.nWDSRotationLinear[m_pIntegrated.nOLWD];
                // 2020.05.15
                nNewDegreex10 += m_pIntegrated.nWDSRotationValue;
            }
            //--
            if (nNewDegreex10 >= 3600)
            {
                nNewDegreex10 -= 3600;
            }
            else if (nNewDegreex10 < 0)
            {
                nNewDegreex10 += 3600;
            }
            if (nNewDegreex10 == 3600)  nNewDegreex10 = 0;
            //--


            int nSRotHX = 0;
            int nSRotVX = 0;
            int nSRotHY = 0;
            int nSRotVY = 0;

            double dRAD = 0.0;

            dRAD = PI_CONST * (double)((double)nNewDegreex10 / 1800.0);

            //--
            // 0       90      180      270
            //--
            // 4096(1) 2048      0     2048
            // 2048(0) 4096    2048       0
            // 2048(0) 4096    2048       0
            // 0   (1) 2048    4096    2048
            //--

            nSRotHX = (int)(2048 * (1.0 + cos(dRAD)));
            nSRotVX = (int)(2048 * (1.0 + cos(dRAD)));

            nSRotHY = (int)(2048 * (1.0 + sin(dRAD)));
            nSRotVY = (int)(2048 * (1.0 - sin(dRAD)));

            if (nSRotHX < 0) nSRotHX = 0;
            if (nSRotVX < 0) nSRotVX = 0;
            if (nSRotHY < 0) nSRotHY = 0;
            if (nSRotVY < 0) nSRotVY = 0;

            if (nSRotHX > 4095) nSRotHX = 4095;
            if (nSRotVX > 4095) nSRotVX = 4095;
            if (nSRotHY > 4095) nSRotHY = 4095;
            if (nSRotVY > 4095) nSRotVY = 4095;

            //--
            // Scan Rotation 보정값 설정
            //--
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "SR0,", nSRotHX); // Sbr Data 전송
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "SR1,", nSRotVX); // Sbr Data 전송
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "SR2,", nSRotHY); // Sbr Data 전송
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "SR3,", nSRotVY); // Sbr Data 전송
            //--

            //--
            // x : X(5V) x cos + Y(4V) x sin
            // y : Y(4V) x cos + X(5V) x sin
            //--
            // 
            //if (abs(m_pIntegrated->nSRotationValue - m_pIntegrated->nSRotationValueOld) >= 5)

            double dWDZoomRatio = 1.0; // ZOOM 배율 보정값
            int    nWD10 = m_pIntegrated.nOLWD; // 현재 WD x 10 값 (FOCUS에 의한 WD값)

            // 배율보정 작업중이면
            if (m_pIntegrated.bWDSetupFlag == TRUE)
            {
                // 현재 WD Setup에 선택된 Z축 값 x 10
                nWD10 = m_pIntegrated.nWDStepNumber * 10;
            }

            // 2020.05.15
            //
            // SCAN ROTATION CALIBRATION DATA 적용
            //
            if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_ORTHOGONAL_CORRRECTION] == 1)
            {
                // 2020.07.06
                if (m_pScanRotCaliData == null) return RET_SUCCESS_OK;
                // 2020.05.15
                // SCAN ROTATION 값 변화 확인
                if (m_pIntegrated.nSRotationValue != m_pIntegrated.nSRotationValueOld)
                {
                    ///
                    // Scan Rotation에 따른 Orthogonal, Zoom X-Y 보정값 적용
                    ///
                    // Orthogonal 보정값 적용 : 각도(0~360, 0~3600, 10등분)
                    int nOrtho = m_pScanRotCaliData.nSRotOrthogonalData[nNewDegreex10];
                    AfxScanBridgeSetOrthogonal(nOrtho);
                    // Orthogonal 보정값 실행
                    AfxScanBridgeSetOrthogonal();

                    ///
                    // Zoom X-Y 보정값 적용 ( 0 ~ 360도 )
                    ///
                    int nMagValue = m_nMagStepValue[m_pIntegrated.nMagStepIndex]; // 배율
                    int nZoom     = AfxSRotCaliGetZoomRelayRatio(nMagValue);       // 배율에 따른 Zoom Index
                    int nZoomX    = m_pScanRotCaliData.nSRotRelayZoomXData[nZoom, nNewDegreex10 /10];
                    int nZoomY    = m_pScanRotCaliData.nSRotRelayZoomYData[nZoom, nNewDegreex10 /10];

                    double dZoomX = (double)nZoomX;
                    double dZoomY = (double)nZoomY;

                    // 2020.06.15
                    // 배율 x35k 이상 ZOOM 값 재보정 적용
                    if (nMagValue > 30000)
                    {
                        if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_IMAGE_SHIFT_Y_REVERSE] == 1)
                        {
                            dZoomX = dZoomX * m_dOptionMagx35kZoomRatio;
                            dZoomY = dZoomY * m_dOptionMagx35kZoomRatio;
                            nZoomX = (int)dZoomX;
                            nZoomY = (int)dZoomY;
                        }
                    }

                    //
                    // 2020.05.29
                    // ZOOM 보정
                    //
                    // 1) WD 변화에 따른 Zoom 보정
                    if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_WD_SCALE_ZOOM] == 1)
                    {
                        // WD SCALE ZOOM CORRECTION
                        dWDZoomRatio = (double)(m_pIntegrated.nWDZoomingLinear[nWD10]) / 1000.0;
                    }
                    // 2) HV보정을 선택 했을 경우
                    if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_HV_CORRECTION] == 1)
                    {
                        dWDZoomRatio = dWDZoomRatio * m_pIntegrated.dMagHVRatio;
                    }
                    //
                    // 2020.05.29 Zoom 설정
                    //
                    m_pIntegrated.wZoomXValue = (ushort)(nZoomX);
                    m_pIntegrated.wZoomYValue = (ushort)(nZoomY);
                    AfxIntegratedSetZoomValue(m_pIntegrated.wZoomXValue, m_pIntegrated.wZoomYValue, dWDZoomRatio);
                    ///
                    ///
                    ///

                    m_pIntegrated.nSRotationValueOld = m_pIntegrated.nSRotationValue;

                    // DISPLAY MESSAGE : ORTHOGONAL, ZOOM X-Y 값
                    USB_DEVICE_DATA_INFO UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
                    UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
                    UsbDeviceDataInfo.nState   = PUB_SROT_ORTHO_ZOOM_CALI_DATA;
                    UsbDeviceDataInfo.nData[0] = nOrtho; // ORTHOGONAL
                    UsbDeviceDataInfo.nData[1] = nZoomX; // ZOOM X
                    UsbDeviceDataInfo.nData[2] = nZoomY; // ZOOM Y
                    // ORTHOGONAL, ZOOM X-Y 값 메시지 보내기
                    SendMessage(m_hOwnerWnd, UM_HW_SETUP_SROT_CALI_DATA, NULL, UsbDeviceDataInfo);

                    ///
                    ///
                    ///
                }
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:11588 ----
        //
        // Scan Bridge :  Scan Rotation에 따른 Orthogonal, Zoom X-Y 보정값 적용을 위한 Ratio Zoom
        //
        public int AfxSRotCaliGetZoomRelayRatio(int nMagValue)
        {
            int nZoom = 0;

            // SCAN ROTATION CALIBRATION DATA INDEX 구하기
            // Zoom
            if      (nMagValue == 20    ) nZoom = 0;  else if (nMagValue == 30    ) nZoom = 1;  else if (nMagValue == 35    ) nZoom = 2;  else if (nMagValue == 40    ) nZoom = 3;  else if (nMagValue == 45    ) nZoom = 4;
            else if (nMagValue == 50    ) nZoom = 5;  else if (nMagValue == 60    ) nZoom = 6;  else if (nMagValue == 70    ) nZoom = 7;  else if (nMagValue == 80    ) nZoom = 8;  else if (nMagValue == 90    ) nZoom = 9;
            else if (nMagValue == 100   ) nZoom = 10; else if (nMagValue == 110   ) nZoom = 11; else if (nMagValue == 120   ) nZoom = 12; else if (nMagValue == 130   ) nZoom = 13; else if (nMagValue == 150   ) nZoom = 14;
            else if (nMagValue == 180   ) nZoom = 15; else if (nMagValue == 200   ) nZoom = 16; else if (nMagValue == 220   ) nZoom = 17; else if (nMagValue == 250   ) nZoom = 18; else if (nMagValue == 300   ) nZoom = 1;// 19;
            //
            else if (nMagValue == 350   ) nZoom = 2;  else if (nMagValue == 400   ) nZoom = 3;  else if (nMagValue == 450   ) nZoom = 4;  else if (nMagValue == 500   ) nZoom = 5;  else if (nMagValue == 600   ) nZoom = 6;
            else if (nMagValue == 700   ) nZoom = 7;  else if (nMagValue == 800   ) nZoom = 8;  else if (nMagValue == 900   ) nZoom = 9;  else if (nMagValue == 1000  ) nZoom = 10; else if (nMagValue == 1100  ) nZoom = 11;
            else if (nMagValue == 1200  ) nZoom = 12; else if (nMagValue == 1300  ) nZoom = 13; else if (nMagValue == 1500  ) nZoom = 14; else if (nMagValue == 1800  ) nZoom = 15; else if (nMagValue == 2000  ) nZoom = 16;
            else if (nMagValue == 2200  ) nZoom = 17; else if (nMagValue == 2500  ) nZoom = 18; else if (nMagValue == 3000  ) nZoom = 1;// 19;
            //
            else if (nMagValue == 3500  ) nZoom = 2;  else if (nMagValue == 4000  ) nZoom = 3;  else if (nMagValue == 4500  ) nZoom = 4;  else if (nMagValue == 5000  ) nZoom = 5;  else if (nMagValue == 6000  ) nZoom = 6;
            else if (nMagValue == 7000  ) nZoom = 7;  else if (nMagValue == 8000  ) nZoom = 8;  else if (nMagValue == 9000  ) nZoom = 9;  else if (nMagValue == 10000 ) nZoom = 10; else if (nMagValue == 11000 ) nZoom = 11;
            else if (nMagValue == 12000 ) nZoom = 12; else if (nMagValue == 13000 ) nZoom = 13; else if (nMagValue == 15000 ) nZoom = 14; else if (nMagValue == 18000 ) nZoom = 15; else if (nMagValue == 20000 ) nZoom = 16;
            else if (nMagValue == 22000 ) nZoom = 17; else if (nMagValue == 25000 ) nZoom = 18; else if (nMagValue == 30000 ) nZoom = 19;// 1;// 19;
            //
            else if (nMagValue == 35000 ) nZoom = 2;  else if (nMagValue == 40000 ) nZoom = 3;  else if (nMagValue == 45000 ) nZoom = 4;  else if (nMagValue == 50000 ) nZoom = 5;  else if (nMagValue == 60000 ) nZoom = 6;
            else if (nMagValue == 70000 ) nZoom = 7;  else if (nMagValue == 80000 ) nZoom = 8;  else if (nMagValue == 90000 ) nZoom = 9;  else if (nMagValue == 100000) nZoom = 10; else if (nMagValue == 110000) nZoom = 11;
            else if (nMagValue == 120000) nZoom = 12; else if (nMagValue == 130000) nZoom = 13; else if (nMagValue == 150000) nZoom = 14; else if (nMagValue == 180000) nZoom = 15; else if (nMagValue == 200000) nZoom = 16;
            else if (nMagValue == 220000) nZoom = 17; else if (nMagValue == 250000) nZoom = 18; else if (nMagValue == 300000) nZoom = 19;

            return nZoom;

        }

        // ---- USB_FT232H_Device.cpp:11621 ----
        //
        // Scan Bridge :  Set Scan Rotation Value
        //
        public int AfxScanBridgeSetScanRotationValue(int nScanRotValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Set Scan Rotation Value
            m_pIntegrated.nSRotationValue = nScanRotValue;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:11634 ----
        //
        // Scan Bridge :  Reset Scan Rotation Value
        //
        public int AfxScanBridgeResetScanRotationValue(int nScanRotValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Set Scan Rotation Value
            m_pIntegrated.nSRotationValue = 0;
            m_pIntegrated.nSRotationValueOld = m_pIntegrated.nSRotationValue;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:11648 ----
        //
        // Scan Bridge :  Scan Rotation 45[deg] X-Amp Value
        //
        public int AfxScanBridgeSetSRot45XAmpValue(int nValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Set Scan Rotation 45[deg] X-Amp Value
            m_pIntegrated.nXSR45Value = nValue;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:11661 ----
        //
        // Scan Bridge :  Scan Rotation 90[deg] X-Amp Value
        //
        public int AfxScanBridgeSetSRot90XAmpValue(int nValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Set Scan Rotation 90[deg] X-Amp Value
            m_pIntegrated.nXSR90Value = nValue;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:11674 ----
        //
        // Scan Bridge :  Scan Rotation 45[deg] Y-Amp Value
        //
        public int AfxScanBridgeSetSRot45YAmpValue(int nValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Set Scan Rotation 45[deg] Y-Amp Value
            m_pIntegrated.nYSR45Value = nValue;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:11687 ----
        //
        // Scan Bridge :  Scan Rotation 90[deg] Y-Amp Value
        //
        public int AfxScanBridgeSetSRot90YAmpValue(int nValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Set Scan Rotation 90[deg] Y-Amp Value
            m_pIntegrated.nYSR90Value = nValue;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:11815 ----
        //
        // Scan Bridge : Get Vaccum State Read 명령어 전송 (진공상태 확인)
        //
        public int AfxScanBridgeGetVaccumState()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            //
            // Evac Board의 FT245와 USB 연결
            //
            // 1) 먼저, [ Evac ] Button -> Enable
            //          [ Air  ] Button -> Disable
            // 2) Avr 진공 Controller에게, 진공상태를 확인하는 "VS;" Command를 보낸다.
            //
            // 3) 진공상태 Command 응답이 있으면, Windows 화면에 진공상태 표시
            //
            // 4) 응답이 없으면, [ Evac ] Button Push를 기다림
            //        [ Evac ] Button Push --> "EV;" Command 보냄 ( FT245-Avr )
            //

            // Evac Command
        //    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "ES;");   // Sbr Data 전송

            // Get Vauum State
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "VS;");   // Sbr Data 전송
            // ScanBridgeBoard -> CncVacBoard (EM40) ==> Motor Backlash Read???(알수없는 내용)
            //AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "SB4;"); // Sbr Data 전송

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:11847 ----
        //
        // Scan Bridge : Program Exit 실행 -> Stage Board Axis Position 만 저장 후 프로그램 종료
        //
        public int AfxScanBridgeProgramExitAxisPosition()
        {
            //if (m_pSbrCommand == NULL) return RET_ERROR_SBR_COMMAND_HANDLE;
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // 프로그램 종료시 Stage Board 의 Axis Position 만 저장 한 후, 프로그램을 종료한다.
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "PS0;"); // Sbr Data 전송
            Sleep(100);

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:11862 ----
        //
        // Scan Bridge : Program Exit 실행 -> Sem PowerOff & Axis Position Saved 실행
        //
        public int AfxScanBridgeProgramExitPowerOff()
        {
            //if (m_pSbrCommand == NULL) return RET_ERROR_SBR_COMMAND_HANDLE;
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            //
            // Scan Bridge 장치가 연결되어 있다면
            // Save axis position & SEM Power Off
            //
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "PS;");  // Sbr Command
            Sleep(100);
            //
            // Stage Board : Save axis position and power off code
            //
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "PS1;"); // Sbr Command
            Sleep(100);
            //
            // SEM Power Off (ShutDown)
            //
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "PD;");  // Sbr Command
            Sleep(100);
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "SB0;"); // Sbr Command
            Sleep(100);
            //

            return RET_SUCCESS_OK;
        }

    }
}
