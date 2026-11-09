// 원본 USB_FT232H_Device.cpp:5318-7697 을 NanoImpact.CodeGen translate로 옮긴 뒤 손으로 고친 코드.
#pragma warning disable IDE1006, IDE0059, CS0162, CS0168, CS0219
using System;
using NanoImpact.Core.Native;
using static NanoImpact.Core.Def;
using static NanoImpact.Core.Native.CRuntime;

namespace NanoImpact.Hardware.Device
{
    public sealed partial class SemDevice
    {
        // ---- USB_FT232H_Device.cpp:5318 ----
        //
        //
        //
        //
        //
        //
        //
        //

        //
        // Scan Bridge : 수신된 명령어 이상 유무 확인 (Check Sum)
        //
        public int AfxSbrCheckSumCommand()
        {
            if (m_pScanBridgeThreadParams == null) return RET_ERROR_SBR_THREAD_PARAMS;               // Scan Bridge 쓰레드 파라메터

            int nRet = 0;
            int nSum = 0;
            int nCheckSum = 0;
            CharBuffer szStrBuf = new CharBuffer(MAX_FT232H_ASSEMBLE_BUFFER  + 1);
            int pBuf; // 원본 char* pBuf (C#: 위치)

            ZeroMem(szStrBuf);

            if (m_nFT232H_SStrCount > 0)
            {
                nRet = 0;
                nSum = 0;
                nCheckSum = 0;

                //----
                // Assemble Command
                //----
                nCheckSum = atoi(m_szFT232H_SStrBuffer);

                // Command
                strcpy_s(szStrBuf, m_szFT232H_CmdBuffer);
                if (m_nFT232H_XStrCount != 0)
                {
                    // X Str Buffer
                    strcat(szStrBuf, m_szFT232H_XStrBuffer);
                }
                if (m_nFT232H_YStrCount != 0)
                {
                    // Y Str Buffer
                    strcat(szStrBuf, ",");
                    strcat(szStrBuf, m_szFT232H_YStrBuffer);
                }
                if (m_nFT232H_DStrCount != 0)
                {
                    // D Str Buffer
                    strcat(szStrBuf, ",");
                    strcat(szStrBuf, m_szFT232H_DStrBuffer);
                }
                // Sbr 데이터 조합 완성
                strcat(szStrBuf, ";");
                //----
                // Check Sum
                //----
                pBuf = 0;
                while (szStrBuf[pBuf] != (byte)';')
                {
                    nSum += (sbyte)szStrBuf[pBuf++]; // 원본 char는 signed
                }
                nSum = nSum % 100;
                if (nCheckSum == nSum) nRet = 1;
                //----
            }
            else
            {
                // S Str Buffer 자료가 없다면...
                nRet = 1;
            }

            return nRet;
        }

        // ---- USB_FT232H_Device.cpp:5386 ----
        //
        // Scan Bridge : 수신된 명령어 조합하기
        //
        public int AfxSbrAssembleCommand(ICharArray pStrBuf)
        {
            if (m_pScanBridgeThreadParams == null) return RET_ERROR_SBR_THREAD_PARAMS;               // Scan Bridge 쓰레드 파라메터
            if (pStrBuf == null) return 0;

            //
            // Assemble Command
            //
            // Command
            strcpy(pStrBuf, m_szFT232H_CmdBuffer);
            if (m_nFT232H_XStrCount != 0)
            {
                // X Str Buffer
                strcat(pStrBuf, m_szFT232H_XStrBuffer);
            }
            if (m_nFT232H_YStrCount != 0)
            {
                // Y Str Buffer
                strcat(pStrBuf, ",");
                strcat(pStrBuf, m_szFT232H_YStrBuffer);
            }
            if (m_nFT232H_DStrCount != 0)
            {
                // D Str Buffer
                strcat(pStrBuf, ",");
                strcat(pStrBuf, m_szFT232H_DStrBuffer);
            }
            if (m_nFT232H_SStrCount != 0)
            {
                // S Str Buffer
                strcat(pStrBuf, "?");
                strcat(pStrBuf, m_szFT232H_SStrBuffer);
            }
            // Sbr 데이터 조합 완성
            strcat(pStrBuf, ";");
            //

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:5447 ----
        //
        //
        // Scan Bridge : SEM Power Down 상태정보 수신
        //
        public int AfxScanBridgePDCommand()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            USB_DEVICE_DATA_INFO UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();

            // 전송자료
            UsbDeviceDataInfo.nState   = 1; // VACUUM 상태값
            UsbDeviceDataInfo.nData[0] = m_pIntegrated.nSemVacState;

            if (m_hOwnerWnd != NULL)
            {
                // DEVICE MAIN SERVER
                // 현재 SEM상태(진공,POWER) 정보 메시지 보내기
                // 2020.05.04
                SendMessage(m_hOwnerWnd, UM_USB_SBR_VACUUM_DATA, NULL, UsbDeviceDataInfo);
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:5473 ----
        //
        // Scan Bridge : SEM 챔버 Air On/Off 상태정보 수신
        //
        public int AfxScanBridgeARCommand()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            USB_DEVICE_DATA_INFO UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();

            m_pIntegrated.nSemAirState = (int)(m_pIntegrated.szXStr[0] - (byte)'0');

            if (m_pIntegrated.nSemAirState > 0)  // Air On Mode 설정
            {
                // SEM 상태 값이 HV(고압) 사용중인 상태이면 -> SEM 대기 상태로 전환한다
                if (m_pIntegrated.nSemVacState == HV_ON_STATE)
                {
                    // 2018.09.28
                    // HV On 상태이면, 대기상태로 전환한다...
                    //AfxScanBridgeSemStandBy(); // 2020.05.11

                    // SEM 상태값을 -> Air On 상태로 설정한다
                    m_pIntegrated.nSemVacState = AIR_ON_STATE;
                }
            }
            else // Air Off Mode 설정
            {
                // SEM 상태값을 -> 진공 진행중 상태로 설정한다
                m_pIntegrated.nSemVacState = VAC_EVAC_STATE;
            }

            // 전송자료
            UsbDeviceDataInfo.nState   = 1; // VACUUM 상태값
            UsbDeviceDataInfo.nData[0] = m_pIntegrated.nSemVacState;

            if (m_hOwnerWnd != NULL)
            {
                // DEVICE MAIN SERVER
                // 현재 SEM상태(진공,POWER) 정보 메시지 보내기
                // 2020.05.04
                SendMessage(m_hOwnerWnd, UM_USB_SBR_VACUUM_DATA, NULL, UsbDeviceDataInfo);
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:5520 ----
        //
        // Scan Bridge : SEM Evaccum(진공율) 상태정보 수신 -> 진공센서 값
        //
        public int AfxScanBridgeEVCommand()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // VACUUM GAUGE 값은 클라이언트 연결 후 받을 준비가 되었다는 설정값이 있을때까지 보내지 않는다
            int bSendFlag = FALSE;
            int bMessage  = FALSE;

            // 진공상태정보
            int nXStr = m_pIntegrated.szXStr[0] - (byte)'0';

            USB_DEVICE_DATA_INFO UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();

            CharBuffer szCalc = new CharBuffer(MAX_FT232H_XYDSStr_BUFFER + 1);
            int    nEvacValue = 0;
            double dEvacValue = 0.0;
            double dCalcValue = 0.0;

            memset(szCalc, 0x00, MAX_FT232H_XYDSStr_BUFFER);

            // 전송자료
            UsbDeviceDataInfo.nState = 2; // VACUUM GAUGE 값

            if (nXStr == 0)
            {
                szCalc[0] = m_pIntegrated.szYStr[0];
                szCalc[1] = m_pIntegrated.szYStr[1];
                szCalc[2] = m_pIntegrated.szYStr[2];
                szCalc[3] = m_pIntegrated.szYStr[3];
                szCalc[4] = NULL;

                // Evac 계산 (현재 진공값 계산)
                nEvacValue = atoi(szCalc);
                dEvacValue = (double)nEvacValue;// atof(szCalc);
                dCalcValue = ((dEvacValue / 102.3) - 6.143) / 1.286;
                dEvacValue = pow(10.0, dCalcValue);

                // 진공상태 데이터 MESSAGE 보내기
                bSendFlag = TRUE;
                bMessage  = TRUE;
                UsbDeviceDataInfo.nState   = 2; // (1:Vacuum State, 2 : 진공게이지값, 3 : Emission값)
                UsbDeviceDataInfo.nData[1] = nEvacValue;
                UsbDeviceDataInfo.dData[1] = dEvacValue;
                // 챔버 진공값
                m_pIntegrated.nSemVacValue  = nEvacValue; // 0~1023
                m_pIntegrated.dSemVacValue1 = dEvacValue; // 계산값
            }
            else
            {
                //
            }


            // 진공상태 데이터 MESSAGE 보내기
            if (bSendFlag == TRUE)
            {
                if (bMessage == TRUE)
                {
                    if (m_hOwnerWnd != NULL)
                    {
                        // DEVICE MAIN SERVER
                        // 진공상태 정보 메시지 보내기 (진공값 포함)
                        if (m_dwVaccumInfoSendCount > 0)
                        {
                            m_dwVaccumInfoSecondTime = GetTickCount();
                            // VACUUM 값 전송을 1초당 2번 이하로 전송한다
                            if (abs((int)(m_dwVaccumInfoSecondTime - m_dwVaccumInfoFirstTime)) > 500) // 시간차 500[ms]
                            {
                                // 2020.05.04
                                SendMessage(m_hOwnerWnd, UM_USB_SBR_VACUUM_DATA, NULL, UsbDeviceDataInfo);
                                // VACUUM 값 전송을 1초당 5번 이하로 전송한다
                                m_dwVaccumInfoFirstTime = m_dwVaccumInfoSecondTime;
                            }
                        }
                        else
                        {
                            // 2020.05.04
                            SendMessage(m_hOwnerWnd, UM_USB_SBR_VACUUM_DATA, NULL, UsbDeviceDataInfo);
                            // VACUUM 값 전송을 1초당 5번 이하로 전송한다
                            m_dwVaccumInfoFirstTime  = GetTickCount();
                            m_dwVaccumInfoSecondTime = m_dwVaccumInfoFirstTime;
                            m_dwVaccumInfoSendCount++;
                        }
                    }
                }
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:5625 ----
        //
        //#define  POWER_OFF_STATE    0   // SEM Power Off 상태
        //#define  AIR_ON_STATE       1   // Air On  상태
        //#define  VAC_EVAC_STATE     2   // Evacuation Working 상태
        //#define  VAC_OK_STATE       3   // 진공 OK 상태
        //#define  HV_ON_STATE        4   // 진공 상태가 HV 사용 가능 상태에 도달함
        //#define  HV_VAC_OK_STATE    7   // GUN 진공OK 및 챔버 진공OK 상태
        //#define  HV_AIR_ON_STATE    8   // GUN 진공OK 및 챔버 AIR ON 상태
        //#define  HV_EVAC_STATE      9   // GUN 진공OK 및 챔버 진공진행 상태
        //
        //
        // Scan Bridge : SEM Vaccum(진공) 상태정보 수신 (위의 상태정보 설명 참조)
        //
        public int AfxScanBridgeVSCommand()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // 진공상태정보
            int nXStr = m_pIntegrated.szXStr[0] - (byte)'0';

            int bMessage = FALSE;
            USB_DEVICE_DATA_INFO UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();

            switch (nXStr)
            {
                case POWER_OFF_STATE:  // 0 : SEM Power Off 상태
                {
                    // SEM 상태를 Power Off 상태로 설정
                    m_pIntegrated.nSemVacState = POWER_OFF_STATE;
                        
                    // 진공상태 데이터 MESSAGE 보내기
                    bMessage = TRUE;
                    UsbDeviceDataInfo.nState   = 1; // (1:Vacuum State, 2 : 진공게이지값, 3 : Emission값)
                    UsbDeviceDataInfo.nData[0] = m_pIntegrated.nSemVacState;

                    break;
                }
                case AIR_ON_STATE:     // 1 : Air On  상태
                {
                    // SEM 상태를 Air On 상태로 설정
                    m_pIntegrated.nSemVacState = AIR_ON_STATE;

                    // 진공상태 데이터 MESSAGE 보내기
                    bMessage = TRUE;
                    UsbDeviceDataInfo.nState   = 1; // (1:Vacuum State, 2 : 진공게이지값, 3 : Emission값)
                    UsbDeviceDataInfo.nData[0] = m_pIntegrated.nSemVacState;

                    break;
                }
                case VAC_EVAC_STATE: // 2 : Evacuation Working 상태
                {
                    // SEM 상태를 Evacuation Working(진공진행중) 상태로 설정
                    m_pIntegrated.nSemVacState = VAC_EVAC_STATE;

                    // 진공상태 데이터 MESSAGE 보내기
                    bMessage = TRUE;
                    UsbDeviceDataInfo.nState   = 1; // (1:Vacuum State, 2 : 진공게이지값, 3 : Emission값)
                    UsbDeviceDataInfo.nData[0] = m_pIntegrated.nSemVacState;

                    break;
                }
                case VAC_OK_STATE:     // 3 : 진공 OK(완료) 상태
                {
                    // SEM 상태를 진공 OK(완료) 상태로 설정
                    m_pIntegrated.nSemVacState = VAC_OK_STATE;

                    // 진공상태 데이터 MESSAGE 보내기
                    bMessage = TRUE;
                    UsbDeviceDataInfo.nState   = 1; // (1:Vacuum State, 2 : 진공게이지값, 3 : Emission값)
                    UsbDeviceDataInfo.nData[0] = m_pIntegrated.nSemVacState;

                    break;
                }
                // 프로그램 버그로 인하여 Power Off Sequence가 진행되지 않고, 갑자기 중단된 경우는 HV ON 상태에 있다.
                // 이런 경우 진공OK(완료)상태로(VAC_OK_STATE) 전환한다. -->> 기존 프로그램에서 "VS" 상태정보 요청은
                // 프로그램 시작시 한번 요청하였다. SEM 상태가 HV_ON_STATE 상태가 되려면, SemStart 버튼을 눌러 스캔작업을 실행해야한다.
                case HV_ON_STATE:     // 4 : 진공 상태가 HV 사용 가능 상태에 도달함
                {
                    //--
                    // 2018.10.01
                    // 아래의 작업은 기존 프로그램에서 프로그램 시작시
                    // VS명령을 1번 요청하는 방식이므로 해당되는 내용이다
                    //--
                    // SEM 상태를 진공 OK(완료) 상태로 설정
                    m_pIntegrated.nSemVacState = VAC_OK_STATE;
                    //--

                    // 진공상태 데이터 MESSAGE 보내기
                    bMessage = TRUE;
                    UsbDeviceDataInfo.nState   = 1; // (1:Vacuum State, 2 : 진공게이지값, 3 : Emission값)
                    UsbDeviceDataInfo.nData[0] = m_pIntegrated.nSemVacState;

                    break;
                }
                case HV_VAC_OK_STATE:     // 7 : GUN진공OK 및 챔버진공OK
                {
                    // SEM 상태를 GUN진공OK 및 챔버진공OK 상태로 설정
                    m_pIntegrated.nSemVacState = VAC_OK_STATE;// HV_VAC_OK_STATE;

                    // 진공상태 데이터 MESSAGE 보내기
                    bMessage = TRUE;
                    UsbDeviceDataInfo.nState   = 1; // (1:Vacuum State, 2 : 진공게이지값, 3 : Emission값)
                    UsbDeviceDataInfo.nData[0] = m_pIntegrated.nSemVacState;

                    break;
                }
                case HV_AIR_ON_STATE:     // 8 : GUN진공OK 및 챔버AIR-ON 상태
                {
                    // SEM 상태를 GUN진공OK 및 챔버AIR-ON 상태 상태로 설정
                    m_pIntegrated.nSemVacState = AIR_ON_STATE;// HV_AIR_ON_STATE;

                    // 진공상태 데이터 MESSAGE 보내기
                    bMessage = TRUE;
                    UsbDeviceDataInfo.nState   = 1; // (1:Vacuum State, 2 : 진공게이지값, 3 : Emission값)
                    UsbDeviceDataInfo.nData[0] = m_pIntegrated.nSemVacState;

                    break;
                }
                case HV_EVAC_STATE: // 9 : GUN 진공OK 및 챔버 진공진행 상태
                {
                    // SEM 상태를 GUN진공OK 및 챔버 진공진행 상태 상태로 설정
                    m_pIntegrated.nSemVacState = VAC_EVAC_STATE;// HV_EVAC_STATE;

                    // 진공상태 데이터 MESSAGE 보내기
                    bMessage = TRUE;
                    UsbDeviceDataInfo.nState   = 1; // (1:Vacuum State, 2 : 진공게이지값, 3 : Emission값)
                    UsbDeviceDataInfo.nData[0] = m_pIntegrated.nSemVacState;

                    break;
                }
                case CNC5_GO_SAM_LOAD:    // 5 : X,Y,Z,T축 샘플로드(샘플장착됨) 위치로 이동하기
                {
                    // 아래 변수의 시작값이다
                    //int SampleLoadState = 2;   // 0:Unload,  1:Loaded,      2:No response
                    //int LodMovingState = 0;

                    // Go to XTZRT-Sample Loading Position
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "SB20;"); // VacBoard -> PC -> To CncBoard

                    // 이동 위치값 Clear
                    m_pIntegrated.nLodCount = 0;
                    m_pIntegrated.nLodPixHistory[m_pIntegrated.nLodCount++] = 0;
                    // 이동상태 설정
                    m_pIntegrated.nLodDirChangedFlag  = 0;
                    m_pIntegrated.nLodMovingState     = 1;
                    m_pIntegrated.nSampleLoadStateOld = m_pIntegrated.nSampleLoadState;

                    // 진공상태 데이터 MESSAGE 보내기
                    //bMessage = TRUE;
                    UsbDeviceDataInfo.nState   = 1; // (1:Vacuum State, 2 : 진공게이지값, 3 : Emission값)
                    UsbDeviceDataInfo.nData[0] = m_pIntegrated.nSemVacState;

                    break;
                }
                case CNC5_GO_SAM_UNLOAD:  // 6 : X,Y,Z,T축 샘플로드 위치에서 -> 샘플제거위치로 이동하기
                {
                    // Go to XTZRT-Sample UnLoading Position
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "SB21;"); // VacBoard -> PC -> To CncBoard

                    // 이동 위치값 Clear
                    m_pIntegrated.nLodCount = 0;
                    m_pIntegrated.nLodPixHistory[m_pIntegrated.nLodCount++] = 0;
                    // 이동상태 설정
                    m_pIntegrated.nLodDirChangedFlag  = 0;
                    m_pIntegrated.nLodMovingState     = 1;
                    m_pIntegrated.nSampleLoadStateOld = m_pIntegrated.nSampleLoadState;

                    // 진공상태 데이터 MESSAGE 보내기
                    //bMessage = TRUE;
                    UsbDeviceDataInfo.nState   = 1; // (1:Vacuum State, 2 : 진공게이지값, 3 : Emission값)
                    UsbDeviceDataInfo.nData[0] = m_pIntegrated.nSemVacState;

                    break;
                }
            }

            // 진공상태 데이터 MESSAGE 보내기
            if (bMessage == TRUE)
            {
                if (m_hOwnerWnd != NULL)
                {
                    // DEVICE MAIN SERVER
                    // 진공상태 정보 메시지 보내기
                    // 2020.05.04
                    SendMessage(m_hOwnerWnd, UM_USB_SBR_VACUUM_DATA, NULL, UsbDeviceDataInfo);
                }
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:5890 ----
        //----
        //  0) SN : Set mm per pulse (mm/step)   <- PC  ==>  VALVE MOTOR mm/step 설정
        //----
        // VL0,0,3200;  // Extra Gate Valve Motor
        // VL0,1,3200;  // Steel Tape Motor
        // VL0,2,3200;  // Damper Motor
        // VL0,3,3200;  // Main Gate Valve Motor
        //----
        //  1) RN : Read mm per pulse (mm/step)  -> PC  ==>  VALVE MOTOR mm/step 읽기
        //----
        // VL1,0;  // Extra Gate Valve Motor
        // VL1,1;  // Steel Tape Motor
        // VL1,2;  // Damper Motor
        // VL1,3;  // Main Gate Valve Motor
        //----
        //  2) SA : Set Accelation table         <- PC  ==>  VALVE MOTOR accel 설정
        //----
        // VL2,0,8000;  // Extra Gate Valve Motor
        // VL2,1,8000;  // Steel Tape Motor
        // VL2,2,8000;  // Damper Motor
        // VL2,3,8000;  // Main Gate Valve Motor
        //----
        //  3)  RA : Read Accelation table      -> PC  ==>  VALVE MOTOR accel 읽기
        //----
        // VL3,0;  // Extra Gate Valve Motor
        // VL3,1;  // Steel Tape Motor
        // VL3,2;  // Damper Motor
        // VL3,3;  // Main Gate Valve Motor
        //----
        //  4) SP :  Set Fixed Pulse            <- PC  ==>  VALVE MOTOR 고정위치step값 설정
        //----
        // VL4,0,1000;  // Extra Gate Valve Motor
        // VL4,1,500;   // Steel Tape Motor
        // VL4,2,4000;  // Damper Motor
        // VL4,3,2000;  // Main Gate Valve Motor
        //----
        //  5) Send FixedPositionPulse & Read MovingPulse -> PC
        //----
        // Send FixedPositionPulse  ==>  VALVE MOTOR 고정위치step값 읽기
        //----
        // VL5,0;  // Extra Gate Valve FixedPositionPulse 
        // VL5,1;  // Steel Tape FixedPositionPulse
        // VL5,2;  // Damper FixedPositionPulse 
        // VL5,3;  // Main Gate Valve FixedPositionPulse
        //----
        // Send MovingPulse ( Current Motor Position )  ==>  VALVE MOTOR 현재위치step값 읽기
        //----
        // VL5,5;  // Extra Gate Valve MovingPulse 
        // VL5,6;  // Steel Tape MovingPulse
        // VL5,7;  // Damper MovingPulse 
        // VL5,8;  // Main Gate Valve MovingPulse
        //----
        //  6)  Motion mm 이동 명령어          <- PC  ==>  VALVE MOTOR 이동위치??mm값 설정
        //----
        // VL6,0,21;   // Extra Gate Valve Motor
        // VL6,1,2.12; // Steel Tape Motor
        // VL6,2,12;   // Damper Motor
        // VL6,3,34;   // Main Gate Valve Motor
        //----
        //  7)  Motion Home & Fixed Position 이동 명령어 ( From PC )  ==>  VALVE MOTOR Home & Position 상태값 설정
        //----
        // VL7,0,0;  // Extra Gate Valve Close Home
        // VL7,0,1;  // Extra Gate Valve Fixed Open Position  
        // VL7,0,2;  // EGV, Home <-> Open Position  ( Home Switch Checking에 의한 Toggling Move )
        // VL7,0,3;  // EGV, Home <-> Open Position  ( 현재 위치값에 의한 Toggling Move )
        // VL7,1,0;  // Steel Tape, Go Home
        // VL7,1,1;  // Steel Tape, Go Sample Position  
        // VL7,1,2;  // SAL, Home <-> Go Sample Loading Position ( Home Switch Checking에 의한 Toggling Move ) 
        // VL7,1,3;  // SAL, Home <-> Go Sample Loading Position ( 현재 위치값에 의한 Toggling Move )  
        // VL7,2,0;  // Damper Go Home
        // VL7,2,1;  // Damper Fixed Damping Position
        // VL7,2,2;  // DAM, Home <-> Go Damping Position ( Home Switch Checking에 의한 Toggling Move ) 
        // VL7,2,3;  // DAM, Home <-> Go Damping Position ( 현재 위치값에 의한 Toggling Move ) 
        // VL7,3,0;  // Main Gate Valve Home
        // VL7,3,1;  // Main Gate Valve Fixed Open Position 
        // VL7,3,2;  // MGV, Home <-> Open Position  
        // VL7,3,3;  // MGV, Home <-> Open Position  
        //----
        //  8)  Valve State ( Home or Fixed Position ) 상태 읽기 명령어 ( From PC )  ==>  VALVE MOTOR Home & Position 상태값 읽기
        //----
        // "VL8,????,????;"
        //----
        //
        // Scan Bridge : SEM Valve 상태정보 수신
        //
        public int AfxScanBridgeVLCommand()
        {
            //if (m_pSbrCommand == NULL) return RET_ERROR_SBR_COMMAND_HANDLE;         // Scan Bridge  Command & Data Handle
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // CNC 상태 및 제어용 윈도우 핸들 확인
            if (m_hStageSetupControlWnd == NULL) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 메시지 처리 전달용 USB Data
            int bMessage = FALSE;
            USB_DEVICE_DATA_INFO UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();

            // Value 상태정보값
            int nValve = atoi(m_pIntegrated.szXStr); // VALVE 상태 명령(숫자)
            int nAxis  = atoi(m_pIntegrated.szYStr); // 선택 VALVE AXIS(숫자)

            double dFixed_mm = 0.0;
            double dMore_mm  = 0.0;

            // CNC Command 설정 : 'VL' Command
            UsbDeviceDataInfo.nCommand = SBR_CMD_VL;
            // Valve 설정
            UsbDeviceDataInfo.nState = nValve;

            // Valve 상태 명령어
            switch (nValve)
            {
                case 8: // Valve State : Valve, Steel Tape, Sampler ( Home or Fixed Position ) 상태 읽기 명령어
                {
                    if (m_pIntegrated.nExtraLimState != nAxis)
                    {
                        m_pIntegrated.nExtraLimState = nAxis;

                        //
                        // Extra Gate Valve(EGV) Top Close Home Position
                        //
                        if ((nAxis & 0x0001) != 0)
                        {
                            // X,Y,Z,T 축이 정지해 있거나 Home & Limit 상태를 Check 한다면...
                            if (m_pIntegrated.bMotorStageHomeCheckFlag == TRUE || m_pIntegrated.nLodMovingState == 0)
                            {
                                // EGV 고정위치값 Clear
                                m_pIntegrated.nEgvPixPosOld = 0;
                            }
                            m_pIntegrated.nExtraCloseState = 1;
                            bMessage = TRUE;
                        }
                        //
                        // Extra Gate Valve(EGV) Open Position
                        //
                        else
                        {
                            // X,Y,Z,T 축이 정지해 있거나 Home & Limit 상태를 Check 한다면...
                            if (m_pIntegrated.bMotorStageHomeCheckFlag == TRUE || m_pIntegrated.nLodMovingState == 0)
                            {
                                // EGV 고정위치값 재설정
                                m_pIntegrated.nEgvPixPosOld = EGV_WIDTH_PIX / 2;
                            }
                            m_pIntegrated.nExtraCloseState = 0;
                            bMessage = TRUE;
                        }
                        //
                        // Extra Gate Valve(EGV) Bot Open Home Position
                        //
                        if ((nAxis & 0x0020) != 0)
                        {
                            // X,Y,Z,T 축이 정지해 있거나 Home & Limit 상태를 Check 한다면...
                            if (m_pIntegrated.bMotorStageHomeCheckFlag == TRUE || m_pIntegrated.nLodMovingState == 0)
                            {
                                // EGV 고정위치값 재설정
                                m_pIntegrated.nEgvPixPosOld = EGV_WIDTH_PIX;
                            }
                            m_pIntegrated.nExtraCloseState = 0;
                            bMessage = TRUE;
                        }
                        else
                        {
                            // X,Y,Z,T 축이 정지해 있거나 Home & Limit 상태를 Check 한다면...
                            if (m_pIntegrated.bMotorStageHomeCheckFlag == TRUE || m_pIntegrated.nLodMovingState == 0)
                            {
                                // EGV 고정위치값 재설정
                                m_pIntegrated.nEgvPixPosOld = EGV_WIDTH_PIX / 2;
                            }
                            m_pIntegrated.nExtraCloseState = 0;
                            bMessage = TRUE;
                        }

                        //
                        // Lod (Close) Home Position
                        //
                        if ((nAxis & 0x0002) != 0)
                        {
                            // X,Y,Z,T 축 Home & Limit 상태를 Check 한다면...
                            if (m_pIntegrated.bMotorStageHomeCheckFlag == TRUE)
                            {
                                // clear In Lod Position
                                // Home Lod

                                // Sample 고정위치값 재설정
                                m_pIntegrated.nSamLodPixPosOld = SL_WIDTH_PIX;
                            }
                            // Sample Load Home 위치상태값 설정
                            m_pIntegrated.nLodHomeState = 1;
                            bMessage = TRUE;
                        }
                        else             // Lod Moving State
                        {
                            // X,Y,Z,T 축 Home & Limit 상태를 Check 한다면...
                            if (m_pIntegrated.bMotorStageHomeCheckFlag == TRUE)
                            {
                                // clear Home Lod
                                // view In Lod

                                // Sample 고정위치값 재설정
                                m_pIntegrated.nSamLodPixPosOld = 0;
                            }
                            m_pIntegrated.nLodHomeState = 0;
                            bMessage = TRUE;
                        }

                        //
                        // Clamp (Off) Home Position
                        //
                        if ((nAxis & 0x0004) != 0)
                        {
                            // X,Y,Z,T 축이 정지해 있거나 Home & Limit 상태를 Check 한다면...
                            if (m_pIntegrated.bMotorStageHomeCheckFlag == TRUE || m_pIntegrated.nLodMovingState == 0)
                            {
                            }
                            // Damper On/Off -> Off 상태설정
                            m_pIntegrated.nDamperOnState = 0;
                            bMessage = TRUE;
                        }
                        //
                        // Clamp (on)  Fixed Position
                        //
                        else            
                        {
                            // X,Y,Z,T 축이 정지해 있거나 Home & Limit 상태를 Check 한다면...
                            if (m_pIntegrated.bMotorStageHomeCheckFlag == TRUE || m_pIntegrated.nLodMovingState == 0)
                            {
                            }
                            // Damper On/Off -> On 상태설정
                            m_pIntegrated.nDamperOnState = 1;
                            bMessage = TRUE;
                        }

                        //
                        // MGV Home Position       [ MainChamber || Turbo Vacuum ]
                        //
                        if ((nAxis & 0x0008) != 0) 
                        {
                            // X,Y,Z,T 축 Home & Limit 상태를 Check 한다면...
                            if (m_pIntegrated.bMotorStageHomeCheckFlag == TRUE)
                            {
                                bMessage = TRUE;
                            }
                        }
                        //
                        // MGV (on) Fixed Position [ MainChamber <-> Turbo Vacuum ]
                        //
                        else             
                        {
                            // X,Y,Z,T 축 Home & Limit 상태를 Check 한다면...
                            if (m_pIntegrated.bMotorStageHomeCheckFlag == TRUE)
                            {
                                bMessage = TRUE;
                            }
                        }

                        //
                        //  Extra Door          (not used ) BSE Home Position
                        //
                        if ((nAxis & 0x0040) != 0) 
                        {
                            //if (LodMovingState == 0)
                            // X,Y,Z,T 축이 정지해 있다면...
                            if (m_pIntegrated.nLodMovingState == 0)
                            {
                                //-----------------------------------------
                                // Sample + Lod + ExtraWall ==> Out Side => Clear
                                //-----------------------------------------

                                //-----------------------------------------
                                // Sample + Lod + ExtraWall ==> Home Pos
                                //-----------------------------------------
                                if (m_pIntegrated.nSampleLoadState == 0)
                                {
                                }
                                //-----------------------------------------

                                bMessage = TRUE;
                            }
                        }
                        else
                        {
                            //-----------------------------------------------
                            //  Limit Home Bmp Update의 CheckBox에 무관한게 동작함
                            //-----------------------------------------------
                            // X,Y,Z,T 축이 정지해 있다면...
                            if (m_pIntegrated.nLodMovingState == 0)
                            {
                                //-----------------------------------------
                                // Sample + Lod + ExtraWall ==> Home Pos => Clear
                                //-----------------------------------------

                                //---------------------------------
                                // Sample + Lod + ExtraWall ==> Out Side
                                //---------------------------------
                                if (m_pIntegrated.nSampleLoadState == 0)
                                {
                                }
                                //---------------------------------

                                bMessage = TRUE;
                            }
                        }
                        //

                        //
                        //PaintBox_SST->Canvas->Draw(0, 0, m_pBitmap_SST);
                        //

                        //
                        // Sample Loaded State
                        //
                        if ((nAxis & 0x0080) != 0) 
                        {
                            // X,Y,Z,T 축 Home & Limit 상태를 Check 한다면...
                            if (m_pIntegrated.bMotorStageHomeCheckFlag == TRUE)
                            {
                                // In Sample
                                // Home Sample
                            }
                            m_pIntegrated.nSampleLoadState = 1;
                            bMessage = TRUE;
                        }
                        else             // Sample Unload State
                        {
                            // X,Y,Z,T 축 Home & Limit 상태를 Check 한다면...
                            if (m_pIntegrated.bMotorStageHomeCheckFlag == TRUE)
                            {
                                // In Sample
                                // Home Sample
                            }
                            m_pIntegrated.nSampleLoadState = 0;
                            bMessage = TRUE;
                        }
                        //

                        if ((nAxis & 0x0010) != 0) // Extra Evac SW
                        {
                            // X,Y,Z,T 축 Home & Limit 상태를 Check 한다면...
                            if (m_pIntegrated.bMotorStageHomeCheckFlag == TRUE)
                            {
                                bMessage = TRUE;
                            }
                        }
                        else
                        {
                            // X,Y,Z,T 축 Home & Limit 상태를 Check 한다면...
                            if (m_pIntegrated.bMotorStageHomeCheckFlag == TRUE)
                            {
                                bMessage = TRUE;
                            }
                        }
                        //
                    }
                    break;
                }


                //
                //  Read 1mm per pulse (mm/step) <= String
                //
                case 1: 
                case 2:
                {
                    // 모터축(Axis) 1Pulse 당 [mm] 이동거리
                    m_pIntegrated.dAxisMMPerPulse[nAxis + 5] = atof(m_pIntegrated.szDStr);
                    bMessage = TRUE;
                    break;
                }

                //
                // Read Accelation table <= String
                //
                case 3: 
                case 4:
                {
                    // 모터축(Axis) Acc(가속) 값
                    bMessage = TRUE;
                    break;
                }

                //
                // Read FixedPosition pulse <= pulse
                //
                case 5: 
                case 6:
                case 7:
                case 9:
                {
                    // 모터축(Axis) 고정위치값 [mm] 계산 및 설정
                    m_pIntegrated.nFixedPositionPulse[nAxis] = atoi(m_pIntegrated.szDStr);
                    dFixed_mm = (double)(m_pIntegrated.nFixedPositionPulse[nAxis] * m_pIntegrated.dAxisMMPerPulse[nAxis + 5]);
                    bMessage = TRUE;
                    break;
                }

                //
                // Read Position pulse <= pulse
                //
                case 10:
                case 11:
                {
                    // 모터축(Axis) 현재위치값 [mm] 계산 및 설정
                    m_pIntegrated.nMorePositionPulse[nAxis] = atoi(m_pIntegrated.szDStr);
                    dMore_mm = (double)(m_pIntegrated.nMorePositionPulse[nAxis] * m_pIntegrated.dAxisMMPerPulse[nAxis + 5]);
                    bMessage = TRUE;
                    break;
                }

                //
                // Main & Extra 챔버 Air 상태 표시,
                // Low Vac Sol Valve / TP NW16 Angle Valve /
                // Extra NW16 Angle Valve ==> On/Off 상태표시
                //
                case 12:
                case 13:
                {
                    if ((nAxis & 0x0001) != 0) // Main Extra Chamber Air Valve On/Off
                    {
                        // Main Chamber Air Valve On
                        m_pIntegrated.nMainChamberAirFlag = 1;

                        // 숫자 Data : Valve Index
                        UsbDeviceDataInfo.nData[0] = 1;
                        // 숫자 Data : On/Off 값
                        UsbDeviceDataInfo.nData[1] = m_pIntegrated.nMainChamberAirFlag;

                        // Main Chamber Air Valve On 상태정보 표시하기 
                        SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, nAxis, UsbDeviceDataInfo);
                    }
                    else
                    {
                        //MotorStageSetupForm->McAirSpeedButton->Glyph->LoadFromFile(m_strAppDir + "\\Tool_Image\\red_ledon.bmp");

                        // Main Chamber Air Valve Off
                        m_pIntegrated.nMainChamberAirFlag = 0;

                        // 숫자 Data : Valve Index
                        UsbDeviceDataInfo.nData[0] = 1;
                        // 숫자 Data : On/Off 값
                        UsbDeviceDataInfo.nData[1] = m_pIntegrated.nMainChamberAirFlag;

                        // Main Chamber Air Valve On 상태정보 표시하기 
                        SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, nAxis, UsbDeviceDataInfo);
                    }

                    if ((nAxis & 0x0002) != 0) // ExtraChamber Air Valve On/Off
                    {
                        //MotorStageSetupForm->ExAirSpeedButton->Glyph->LoadFromFile(m_strAppDir + "\\Tool_Image\\green_ledon.bmp");

                        // Extra Chamber Air Valve On
                        m_pIntegrated.nExtraChamberAirFlag = 1;

                        // 숫자 Data : Valve Index
                        UsbDeviceDataInfo.nData[0] = 2;
                        // 숫자 Data : On/Off 값
                        UsbDeviceDataInfo.nData[1] = m_pIntegrated.nExtraChamberAirFlag;

                        // Main Chamber Air Valve On 상태정보 표시하기 
                        SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, nAxis, UsbDeviceDataInfo);
                    }
                    else
                    {
                        //MotorStageSetupForm->ExAirSpeedButton->Glyph->LoadFromFile(m_strAppDir + "\\Tool_Image\\red_ledon.bmp");

                        // Extra Chamber Air Valve Off
                        m_pIntegrated.nExtraChamberAirFlag = 0;

                        // 숫자 Data : Valve Index
                        UsbDeviceDataInfo.nData[0] = 2;
                        // 숫자 Data : On/Off 값
                        UsbDeviceDataInfo.nData[1] = m_pIntegrated.nExtraChamberAirFlag;

                        // Main Chamber Air Valve On 상태정보 표시하기 
                        SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, nAxis, UsbDeviceDataInfo);
                    }

                    if ((nAxis & 0x0004) != 0) // Low Vacuum Sol Valve On/Off
                    {
                        //MotorStageSetupForm->LVacSolVSpeedButton->Glyph->LoadFromFile(m_strAppDir + "\\Tool_Image\\green_ledon.bmp");

                        // Low Vacuum Sol Valve Air On
                        m_pIntegrated.nLowVacSolAirFlag = 1;

                        // 숫자 Data : Valve Index
                        UsbDeviceDataInfo.nData[0] = 3;
                        // 숫자 Data : On/Off 값
                        UsbDeviceDataInfo.nData[1] = m_pIntegrated.nLowVacSolAirFlag;

                        // Main Chamber Air Valve On 상태정보 표시하기 
                        SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, nAxis, UsbDeviceDataInfo);
                    }
                    else
                    {
                        //MotorStageSetupForm->LVacSolVSpeedButton->Glyph->LoadFromFile(m_strAppDir + "\\Tool_Image\\red_ledon.bmp");

                        // Low Vacuum Sol Valve Air Off
                        m_pIntegrated.nLowVacSolAirFlag = 0;

                        // 숫자 Data : Valve Index
                        UsbDeviceDataInfo.nData[0] = 3;
                        // 숫자 Data : On/Off 값
                        UsbDeviceDataInfo.nData[1] = m_pIntegrated.nLowVacSolAirFlag;

                        // Main Chamber Air Valve On 상태정보 표시하기 
                        SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, nAxis, UsbDeviceDataInfo);
                    }


                    if ((nAxis & 0x0008) != 0) // TP NW16 Angle Valve On/Off
                    {
                        //MotorStageSetupForm->TpVLSpeedButton->Glyph->LoadFromFile(m_strAppDir + "\\Tool_Image\\red_ledon.bmp");

                        // TP NW16 Angle Valve On -> Closed
                        m_pIntegrated.nTpVL_CloseFlag = 1;

                        // 숫자 Data : Valve Index
                        UsbDeviceDataInfo.nData[0] = 4;
                        // 숫자 Data : On/Off 값
                        UsbDeviceDataInfo.nData[1] = m_pIntegrated.nTpVL_CloseFlag;

                        // Main Chamber Air Valve On 상태정보 표시하기 
                        SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, nAxis, UsbDeviceDataInfo);
                    }
                    else
                    {
                        //MotorStageSetupForm->TpVLSpeedButton->Glyph->LoadFromFile(m_strAppDir + "\\Tool_Image\\green_ledon.bmp");

                        // TP NW16 Angle Valve Off -> Opened
                        m_pIntegrated.nTpVL_CloseFlag = 0;

                        // 숫자 Data : Valve Index
                        UsbDeviceDataInfo.nData[0] = 4;
                        // 숫자 Data : On/Off 값
                        UsbDeviceDataInfo.nData[1] = m_pIntegrated.nTpVL_CloseFlag;

                        // Main Chamber Air Valve On 상태정보 표시하기 
                        SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, nAxis, UsbDeviceDataInfo);
                    }

                    if ((nAxis & 0x0010) != 0) // Extra NW16 Angle Valve On/Off
                    {
                        //MotorStageSetupForm->ExVLSpeedButton->Glyph->LoadFromFile(m_strAppDir + "\\Tool_Image\\red_ledon.bmp");

                        // Extra NW16 Angle Valve On -> Closed
                        m_pIntegrated.nExVL_CloseFlag = 1;

                        // 숫자 Data : Valve Index
                        UsbDeviceDataInfo.nData[0] = 5;
                        // 숫자 Data : On/Off 값
                        UsbDeviceDataInfo.nData[1] = m_pIntegrated.nExVL_CloseFlag;

                        // Main Chamber Air Valve On 상태정보 표시하기 
                        SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, nAxis, UsbDeviceDataInfo);
                    }
                    else
                    {
                        //MotorStageSetupForm->ExVLSpeedButton->Glyph->LoadFromFile(m_strAppDir + "\\Tool_Image\\green_ledon.bmp");

                        // Extra NW16 Angle Valve Off -> Opened
                        m_pIntegrated.nExVL_CloseFlag = 0;

                        // 숫자 Data : Valve Index
                        UsbDeviceDataInfo.nData[0] = 5;
                        // 숫자 Data : On/Off 값
                        UsbDeviceDataInfo.nData[1] = m_pIntegrated.nExVL_CloseFlag;

                        // Main Chamber Air Valve On 상태정보 표시하기 
                        SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, nAxis, UsbDeviceDataInfo);
                    }
                    break;
                }

            }

            // VALVE 상태 데이터 MESSAGE 보내기
            if (bMessage == TRUE)
            {
                if (m_hOwnerWnd != NULL)
                {
                    // 2020.05.04
                    // VALVE 상태 정보 메시지 보내기
                    //SendMessage(m_hOwnerWnd, UM_USB_SBR_VAL_STATE, NULL, (LPARAM)&UsbDeviceDataInfo);
                }
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:6397 ----
        //
        // Scan Bridge : "VL", "AR" -> Valve On/Off State , Valve On/Of 명령 실행
        //
        public int AfxScanBridgeVLCommand(int nWriteFlag, int nValve, string pszData)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Axis Accelation Pulse 설정
            CharBuffer szCNCData = new CharBuffer(255 + 1);
            ZeroMem(szCNCData);

            // 위치값
            int nOnOffValue = 0;

            // 정보값 저장하기 
            if (nWriteFlag == 1)
            {
                // VL, RA ??? 값
                if (pszData != null) nOnOffValue = atoi(pszData);

                // VL, RA Command
                switch (nValve)
                {
                case 13:
                {
                    // X축 Accelation값 설정하기
                    sprintf(szCNCData, "RA%d,%d;", nValve, nOnOffValue);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                }
            }
            else
            {
                // VL, RA ??? 값
                //if (pszData != NULL) nValue = atoi(pszData);

                // RA Command
                switch (nValve)
                {
                case 0:
                {
                    // Main Chamber Air Value On/Off 설정 및 실행하기
                    nOnOffValue = m_pIntegrated.nMainChamberAirFlag;
                    sprintf(szCNCData, "RA%d,%d;", nValve, nOnOffValue);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case 1:
                {
                    // Extra Chamber Air Value On/Off 설정 및 실행하기
                    nOnOffValue = m_pIntegrated.nExtraChamberAirFlag;
                    sprintf(szCNCData, "RA%d,%d;", nValve, nOnOffValue);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case 2:
                {
                    // Low Vacuum Sol Value On/Off 설정 및 실행하기
                    nOnOffValue = m_pIntegrated.nLowVacSolAirFlag;
                    sprintf(szCNCData, "RA%d,%d;", nValve, nOnOffValue);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case 3:
                {
                    // TP NW16 Angle Value On/Off 설정 및 실행하기
                    nOnOffValue = m_pIntegrated.nTpVL_CloseFlag;
                    sprintf(szCNCData, "RA%d,%d;", nValve, nOnOffValue);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case 4:
                {
                    // Extra NW16 Angle Value On/Off 설정 및 실행하기
                    nOnOffValue = m_pIntegrated.nExVL_CloseFlag;
                    sprintf(szCNCData, "RA%d,%d;", nValve, nOnOffValue);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case 13:
                {
                    if (pszData != null) nOnOffValue = atoi(pszData);
                    // Air Valve Check On/Off 설정 및 실행하기
                    sprintf(szCNCData, "VL%d,%d;", nValve, nOnOffValue);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                }
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:6498 ----
        //
        // Scan Bridge : "EM" -> Emission Current 값 Read(수신)
        //
        public int AfxScanBridgeEMCommand()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle
                                                                                    
            // 수신된 Emission Current 값 보관 및 표시
            m_pIntegrated.nEmCurrentLevel = atoi(m_pIntegrated.szXStr);

            int bMessage = FALSE;
            USB_DEVICE_DATA_INFO UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();

            bMessage = TRUE;
            // Emission Level
            UsbDeviceDataInfo.nState   = 3; // Emission 처리 (1:Vacuum State, 2:진공게이지값, 3:Emission값)
            UsbDeviceDataInfo.nData[2] = m_pIntegrated.nEmCurrentLevel;

            // Emission Current 값 MESSAGE 보내기
            if (bMessage == TRUE)
            {
                if (m_hOwnerWnd != NULL)
                {
                    // DEVICE MAIN SERVER
                    // Emission Current 값 정보 메시지 보내기 (진공 데이터에 포함되어 있음)
                    // 2020.05.04
                    SendMessage(m_hOwnerWnd, UM_USB_SBR_VACUUM_DATA, NULL, UsbDeviceDataInfo);
                }
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:6532 ----
        //
        // Scan Bridge : "RN" -> Read  step / mm (1mm당 step갯수) 값 수신 (CNC Board 명령어 자료)
        //
        public int AfxScanBridgeRNCommand()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // 모터 축 정보
            int nAxis = (int)(m_pIntegrated.szXStr[0] - (byte)'0');

            if (nAxis >= IDX_X_AXIS && nAxis <= IDX_B_AXIS)//IDX_T_AXIS)
            {
                // 수신된 1[mm]당 Pulse갯수 보관(적용)
                m_pIntegrated.dAxisPulsePerUnit[nAxis] = atof(m_pIntegrated.szYStr);
            }
            else
            {
                return RET_SUCCESS_OK;
            }

            // 각 축(Axis)별 Unit 당 Pulse 갯수 계산 및 보관(적용)
            if (m_pIntegrated.dAxisPulsePerUnit[nAxis] == 0)
            {
                // 수신된 1Pulse 당 이동거리[mm] 보관(적용) : 소숫점 7자리
                m_pIntegrated.dAxisMMPerPulse[nAxis] = 0.0;
            }
            else
            {
                // 수신된 1Pulse 당 이동거리[mm] 보관(적용) : 소숫점 7자리
                m_pIntegrated.dAxisMMPerPulse[nAxis] = 1.0 / m_pIntegrated.dAxisPulsePerUnit[nAxis];
            }

            // CNC 상태 및 제어용 윈도우 핸들 확인
            if (m_hStageSetupControlWnd == NULL) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 메시지 처리 전달용 USB Data
            int bMessage = FALSE;
            USB_DEVICE_DATA_INFO UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            bMessage = TRUE;

            // CNC Command 설정 : 'RN' Command
            UsbDeviceDataInfo.nCommand = SBR_CMD_RN;
            // Axis 설정
            UsbDeviceDataInfo.nState = nAxis;
            // 수신된 Data String : 1Pulse 당 이동거리[mm]
            strcpy_s(UsbDeviceDataInfo.szData, m_pIntegrated.szYStr);
            // 숫자 Data : 1Pulse 당 이동거리[mm]
            UsbDeviceDataInfo.dData[0] = m_pIntegrated.dAxisMMPerPulse[nAxis];
            // 숫자 Data : 1[mm] 당 Pulse 갯수
            UsbDeviceDataInfo.dData[1] = m_pIntegrated.dAxisPulsePerUnit[nAxis];

            // 2020.04.28
            /*
            // mm/step 계산된 값 자료 전송
            switch (nAxis)
            {
                case IDX_X_AXIS: // X축 1[mm]이동거리 당 Pulse 값 표시하기 
                case IDX_Y_AXIS: // Y축 1[mm]이동거리 당 Pulse 값 표시하기 
                case IDX_Z_AXIS: // Z축 1[mm]이동거리 당 Pulse 값 표시하기 
                case IDX_R_AXIS: // R축 1[deg]이동거리 당 Pulse 값 표시하기 
                case IDX_T_AXIS: // T축 1[deg]이동거리 당 Pulse 값 표시하기 
                {
                    bMessage = TRUE;
                    if (m_hOwnerWnd != NULL)
                    {
                        // DEVICE MAIN SERVER
                        // pulse/1mm 값 정보 메시지 보내기
                        SendMessage(m_hOwnerWnd, UM_USB_SBR_STAGE_AXIS_MM_STEP, (WPARAM)nAxis, (LPARAM)&UsbDeviceDataInfo);
                    }
                    break;
                }
            }
            */

            // pulse/1mm 계산된 값 표시하기
            switch (nAxis)
            {
                case IDX_X_AXIS:
                {
                    // X축 1[mm]이동거리 당 Pulse 값 표시하기 
                    SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, nAxis, UsbDeviceDataInfo);
                    break;
                }
                case IDX_Y_AXIS:
                {
                    // Y축 1[mm]이동거리 당 Pulse 값 표시하기 
                    SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, nAxis, UsbDeviceDataInfo);
                    break;
                }
                case IDX_Z_AXIS:
                {
                    // Z축 1[mm]이동거리 당 Pulse 값 표시하기 
                    SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, nAxis, UsbDeviceDataInfo);
                    break;
                }
                case IDX_R_AXIS:
                {
                    // R축 1[deg]이동거리 당 Pulse 값 표시하기 
                    SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, nAxis, UsbDeviceDataInfo);
                    break;
                }
                case IDX_T_AXIS:
                {
                    // T축 1[deg]이동거리 당 Pulse 값 표시하기 
                    SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, nAxis, UsbDeviceDataInfo);

                    // 3축 STAGE
                    if (m_pIntegrated.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_ALL_DATA_VIEW] == FALSE)
                    {
            
                        // T축을 이용하여 BSE축 정보를 얻는다
                        m_pIntegrated.dAxisMMPerPulse[IDX_B_AXIS]   = m_pIntegrated.dAxisMMPerPulse[IDX_T_AXIS];
                        m_pIntegrated.dAxisPulsePerUnit[IDX_B_AXIS] = m_pIntegrated.dAxisPulsePerUnit[IDX_T_AXIS];
                        UsbDeviceDataInfo.nState = IDX_B_AXIS;
                        SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, nAxis, UsbDeviceDataInfo);
                    }

                    break;
                }
                case IDX_B_AXIS:
                {
                    // BSE 축 1[deg]이동거리 당 Pulse 값 표시하기 
                    SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, nAxis, UsbDeviceDataInfo);
                    break;
                }
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:6663 ----
        //
        // Scan Bridge : "RN" -> Read or Write  mm per step(1Pulse당 이동거리[mm]) 값 수신 (CNC Board 명령어 자료)
        //
        public int AfxScanBridgeRNCommand(int nWriteFlag, int nAxis, string pszData)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // 모터 축 RN 정보
            CharBuffer szCNCData = new CharBuffer(255 + 1);
            CharBuffer szAxis = new CharBuffer(5 + 1);
            ZeroMem(szCNCData);
            ZeroMem(szAxis);

            // 정보값 저장하기 
            if (nWriteFlag == 1)
            {
                // SN Command
                strcpy_s(szCNCData, "SN");

                // 계산된 값 표시하기
                switch (nAxis)
                {
                    case IDX_X_AXIS:
                    {
                        // X축 1[mm]이동거리 당 Pulse 값 저장하기 
                        //strcat(szCNCData, "0,");
                        sprintf(szAxis, "%d,", nAxis);
                        strcat(szCNCData, szAxis);
                        strcat(szCNCData, pszData);
                        strcat(szCNCData, ";");
                        // VacBoard -> PC -> To CncBoard
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                        break;
                    }
                    case IDX_Y_AXIS:
                    {
                        // Y축 1[mm]이동거리 당 Pulse 값 저장하기 
                        //strcat(szCNCData, "1,");
                        sprintf(szAxis, "%d,", nAxis);
                        strcat(szCNCData, szAxis);
                        strcat(szCNCData, pszData);
                        strcat(szCNCData, ";");
                        // VacBoard -> PC -> To CncBoard
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                        break;
                    }
                    case IDX_Z_AXIS:
                    {
                        // Z축 1[mm]이동거리 당 Pulse 값 저장하기 
                        //strcat(szCNCData, "2,");
                        sprintf(szAxis, "%d,", nAxis);
                        strcat(szCNCData, szAxis);
                        strcat(szCNCData, pszData);
                        strcat(szCNCData, ";");
                        // VacBoard -> PC -> To CncBoard
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                        break;
                    }
                    case IDX_R_AXIS:
                    {
                        // R축 1[deg]이동거리 당 Pulse 값 저장하기 
                        //strcat(szCNCData, "3,");
                        sprintf(szAxis, "%d,", nAxis);
                        strcat(szCNCData, szAxis);
                        strcat(szCNCData, pszData);
                        strcat(szCNCData, ";");
                        // VacBoard -> PC -> To CncBoard
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                        break;
                    }
                    case IDX_T_AXIS:
                    {
                        // T축 1[deg]이동거리 당 Pulse 값 저장하기 
                        //strcat(szCNCData, "4,");
                        sprintf(szAxis, "%d,", nAxis);
                        strcat(szCNCData, szAxis);
                        strcat(szCNCData, pszData);
                        strcat(szCNCData, ";");
                        // VacBoard -> PC -> To CncBoard
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                        break;
                    }
                    case IDX_B_AXIS:
                    {
                        // 3축 STAGE
                        if (m_pIntegrated.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_ALL_DATA_VIEW] == FALSE)
                        {
                            // T축을 이용하여 BSE축 정보를 얻는다
                            // BSE축 1[mm]이동거리 당 Pulse 값 저장하기 
                            int nBseAxis = IDX_T_AXIS;
                            sprintf(szAxis, "%d,", nBseAxis);
                            strcat(szCNCData, szAxis);
                            strcat(szCNCData, pszData);
                            strcat(szCNCData, ";");
                            // VacBoard -> PC -> To CncBoard
                            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                        }
                        // 5축 STAGE
                        else
                        {
                            // BSE축 1[mm]이동거리 당 Pulse 값 저장하기 
                            int nBseAxis = IDX_B_AXIS;
                            sprintf(szAxis, "%d,", nBseAxis);
                            strcat(szCNCData, szAxis);
                            strcat(szCNCData, pszData);
                            strcat(szCNCData, ";");
                            // VacBoard -> PC -> To CncBoard
                            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                        }
                        break;
                    }
                }
            }
            else
            {
                // RN Command (예 Z축 : "RN2;" )
                strcpy_s(szCNCData, "RN");

                // 계산된 값 표시하기
                switch (nAxis)
                {
                    case IDX_X_AXIS:
                    {
                        // X축 1[mm]이동거리 당 Pulse 값 표시하기 
                        //strcat(szCNCData, "0,");
                        sprintf(szAxis, "%d", nAxis);
                        strcat(szCNCData, szAxis);
                        strcat(szCNCData, ";");
                        // VacBoard -> PC -> To CncBoard
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                        break;
                    }
                    case IDX_Y_AXIS:
                    {
                        // Y축 1[mm]이동거리 당 Pulse 값 표시하기 
                        //strcat(szCNCData, "1,");
                        sprintf(szAxis, "%d", nAxis);
                        strcat(szCNCData, szAxis);
                        strcat(szCNCData, ";");
                        // VacBoard -> PC -> To CncBoard
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                        break;
                    }
                    case IDX_Z_AXIS:
                    {
                        // Z축 1[mm]이동거리 당 Pulse 값 표시하기 
                        //strcat(szCNCData, "2,");
                        sprintf(szAxis, "%d", nAxis);
                        strcat(szCNCData, szAxis);
                        strcat(szCNCData, ";");
                        // VacBoard -> PC -> To CncBoard
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                        break;
                    }
                    case IDX_R_AXIS:
                    {
                        // R축 1[deg]이동거리 당 Pulse 값 표시하기 
                        //strcat(szCNCData, "3,");
                        sprintf(szAxis, "%d", nAxis);
                        strcat(szCNCData, szAxis);
                        strcat(szCNCData, ";");
                        // VacBoard -> PC -> To CncBoard
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                        break;
                    }
                    case IDX_T_AXIS:
                    {
                        // T축 1[deg]이동거리 당 Pulse 값 표시하기 
                        //strcat(szCNCData, "4,");
                        sprintf(szAxis, "%d", nAxis);
                        strcat(szCNCData, szAxis);
                        strcat(szCNCData, ";");
                        // VacBoard -> PC -> To CncBoard
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                        break;
                    }
                    case IDX_B_AXIS:
                    {
                        // 3축 STAGE
                        if (m_pIntegrated.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_ALL_DATA_VIEW] == FALSE)
                        {
                
                            // T축을 이용하여 BSE축 정보를 얻는다
                            // BSE축 1[mm]이동거리 당 Pulse 값 표시하기 
                            int nBseAxis = IDX_T_AXIS;
                            sprintf(szAxis, "%d", nBseAxis);
                            strcat(szCNCData, szAxis);
                            strcat(szCNCData, ";");
                            // VacBoard -> PC -> To CncBoard
                            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                        }
                        // 5축 STAGE
                        else
                        {
                            // BSE축 1[mm]이동거리 당 Pulse 값 표시하기 
                            int nBseAxis = IDX_B_AXIS;
                            sprintf(szAxis, "%d", nBseAxis);
                            strcat(szCNCData, szAxis);
                            strcat(szCNCData, ";");
                            // VacBoard -> PC -> To CncBoard
                            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                        }
                        break;
                    }
                }
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:6873 ----
        //
        // Scan Bridge : "RA" -> Read Axis Accelation (각 축의 Accelation 값 수신 및 적용) (CNC Board 명령어 자료)
        //
        public int AfxScanBridgeRACommand()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // CNC 상태 및 제어용 윈도우 핸들 확인
            if (m_hStageSetupControlWnd == NULL) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 메시지 처리 전달용 USB Data
            USB_DEVICE_DATA_INFO UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            int bMessage = FALSE;

            // CNC Command 설정 : 'RA' Command
            UsbDeviceDataInfo.nCommand = SBR_CMD_RA;

            // 모터 축(Axis)
            int nAxis = (int)(m_pIntegrated.szXStr[0] - (byte)'0');
            CharBuffer szData = new CharBuffer(255+1);

            // 수신된 값 표시하기
            switch (nAxis)
            {
                case IDX_X_AXIS:
                {
                    // 수신된 모터 축(Axis) Accelation(가속) 값 보관 및 적용
                    m_pIntegrated.nAxisAccCalVal[nAxis] = atoi(m_pIntegrated.szYStr);
                    bMessage = TRUE;

                    ZeroMem(szData);
                    sprintf(szData, "%d", m_pIntegrated.nAxisAccCalVal[nAxis]);

                    // X축 Accelation 값 표시하기 
                    UsbDeviceDataInfo.nState = nAxis;
                    strcpy_s(UsbDeviceDataInfo.szData, szData);
                    break;
                }
                case IDX_Y_AXIS:
                {
                    // 수신된 모터 축(Axis) Accelation(가속) 값 보관 및 적용
                    m_pIntegrated.nAxisAccCalVal[nAxis] = atoi(m_pIntegrated.szYStr);
                    bMessage = TRUE;

                    ZeroMem(szData);
                    sprintf(szData, "%d", m_pIntegrated.nAxisAccCalVal[nAxis]);

                    // Y축 Accelation 값 표시하기 
                    UsbDeviceDataInfo.nState = nAxis;
                    strcpy_s(UsbDeviceDataInfo.szData, szData);
                    break;
                }
                case IDX_Z_AXIS:
                {
                    // 수신된 모터 축(Axis) Accelation(가속) 값 보관 및 적용
                    m_pIntegrated.nAxisAccCalVal[nAxis] = atoi(m_pIntegrated.szYStr);
                    bMessage = TRUE;

                    ZeroMem(szData);
                    sprintf(szData, "%d", m_pIntegrated.nAxisAccCalVal[nAxis]);

                    // Z축 Accelation 값 표시하기 
                    UsbDeviceDataInfo.nState = nAxis;
                    strcpy_s(UsbDeviceDataInfo.szData, szData);
                    break;
                }
                case IDX_R_AXIS:
                {
                    // 수신된 모터 축(Axis) Accelation(가속) 값 보관 및 적용
                    m_pIntegrated.nAxisAccCalVal[nAxis] = atoi(m_pIntegrated.szYStr);
                    bMessage = TRUE;

                    ZeroMem(szData);
                    sprintf(szData, "%d", m_pIntegrated.nAxisAccCalVal[nAxis]);

                    // R축 Accelation 값 표시하기 
                    UsbDeviceDataInfo.nState = nAxis;
                    strcpy_s(UsbDeviceDataInfo.szData, szData);
                    break;
                }
                case IDX_T_AXIS:
                {
                    // 수신된 모터 축(Axis) Accelation(가속) 값 보관 및 적용
                    m_pIntegrated.nAxisAccCalVal[nAxis] = atoi(m_pIntegrated.szYStr);
                    bMessage = TRUE;

                    ZeroMem(szData);
                    sprintf(szData, "%d", m_pIntegrated.nAxisAccCalVal[nAxis]);

                    // T축 Accelation 값 표시하기 
                    UsbDeviceDataInfo.nState = nAxis;
                    strcpy_s(UsbDeviceDataInfo.szData, szData);
                    break;
                }
                case IDX_B_AXIS:
                {
                    // 수신된 모터 축(Axis) Accelation(가속) 값 보관 및 적용
                    m_pIntegrated.nAxisAccCalVal[nAxis] = atoi(m_pIntegrated.szYStr);
                    bMessage = TRUE;

                    ZeroMem(szData);
                    sprintf(szData, "%d", m_pIntegrated.nAxisAccCalVal[nAxis]);

                    // BSE축 Accelation 값 표시하기 
                    UsbDeviceDataInfo.nState = nAxis;
                    strcpy_s(UsbDeviceDataInfo.szData, szData);
                    break;
                }
            }

            /*
            // axis accel 값 자료 전송
            switch (nAxis)
            {
                case IDX_X_AXIS: // X축 accel 값 표시하기 
                case IDX_Y_AXIS: // Y축 accel 값 표시하기 
                case IDX_Z_AXIS: // Z축 accel 값 표시하기 
                case IDX_R_AXIS: // R축 accel 값 표시하기 
                case IDX_T_AXIS: // T축 accel 값 표시하기 
                {
                    if (bMessage == TRUE)
                    {
                        if (m_hOwnerWnd != NULL)
                        {
                            // DEVICE MAIN SERVER
                            // axis accel 값 정보 메시지 보내기
                            SendMessage(m_hOwnerWnd, UM_USB_SBR_STAGE_AXIS_ACCEL, (WPARAM)nAxis, (LPARAM)&UsbDeviceDataInfo);
                        }
                    }
                    break;
                }
            }
            */

            // 수신된 값 표시하기
            switch (nAxis)
            {
            case IDX_X_AXIS:
            {
                // 수신된 모터 축(Axis) Accelation(가속) 값 보관 및 적용
                // X축 Accelation 값 표시하기 
                SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, 0, UsbDeviceDataInfo);
                break;
            }
            case IDX_Y_AXIS:
            {
                // 수신된 모터 축(Axis) Accelation(가속) 값 보관 및 적용
                // Y축 Accelation 값 표시하기 
                SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, 0, UsbDeviceDataInfo);
                break;
            }
            case IDX_Z_AXIS:
            {
                // 수신된 모터 축(Axis) Accelation(가속) 값 보관 및 적용
                // Z축 Accelation 값 표시하기 
                SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, 0, UsbDeviceDataInfo);
                break;
            }
            case IDX_R_AXIS:
            {
                // 수신된 모터 축(Axis) Accelation(가속) 값 보관 및 적용
                // R축 Accelation 값 표시하기 
                SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, 0, UsbDeviceDataInfo);
                break;
            }
            case IDX_T_AXIS:
            {
                // 수신된 모터 축(Axis) Accelation(가속) 값 보관 및 적용
                // T축 Accelation 값 표시하기 
                SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, 0, UsbDeviceDataInfo);

                // 3축 STAGE
                if (m_pIntegrated.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_ALL_DATA_VIEW] == FALSE)
                {

                    // BSE축 Accelation 값 표시하기 
                    m_pIntegrated.nAxisAccCalVal[IDX_B_AXIS] = m_pIntegrated.nAxisAccCalVal[IDX_T_AXIS];
                    UsbDeviceDataInfo.nState = IDX_B_AXIS;
                    SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, 0, UsbDeviceDataInfo);
                }

                break;
            }
            case IDX_B_AXIS:
            {
                // 수신된 모터 축(Axis) Accelation(가속) 값 보관 및 적용
                // BSE축 Accelation 값 표시하기 
                SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, 0, UsbDeviceDataInfo);
                break;
            }
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:7069 ----
        //
        // Scan Bridge : "RA","SA" -> Axis Accelation 읽기 또는 쓰기
        //
        public int AfxScanBridgeRACommand(int nWriteFlag, int nAxis, string pszData)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Axis Accelation Pulse 설정
            CharBuffer szCNCData = new CharBuffer(255 + 1);
            ZeroMem(szCNCData);

            // 위치값
            int nPulse = 0;

            // 정보값 저장하기 
            if (nWriteFlag == 1)
            {
                // Axis Accelation Pulse 값
                if (pszData != null) nPulse = atoi(pszData);

                // SA Command
                switch (nAxis)
                {
                case IDX_X_AXIS:
                {
                    // X축 Accelation값 설정하기
                    sprintf(szCNCData, "SA%d,%d;", nAxis, nPulse);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case IDX_Y_AXIS:
                {
                    // Y축 Accelation값 설정하기
                    sprintf(szCNCData, "SA%d,%d;", nAxis, nPulse);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case IDX_Z_AXIS:
                {
                    // Z축 Accelation값 설정하기
                    sprintf(szCNCData, "SA%d,%d;", nAxis, nPulse);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case IDX_R_AXIS:
                {
                    // R축 Accelation값 설정하기
                    sprintf(szCNCData, "SA%d,%d;", nAxis, nPulse);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case IDX_T_AXIS:
                {
                    // T축 Accelation값 설정하기
                    sprintf(szCNCData, "SA%d,%d;", nAxis, nPulse);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case IDX_B_AXIS:
                {
                    // 3축 STAGE
                    if (m_pIntegrated.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_ALL_DATA_VIEW] == FALSE)
                    {
                        // BSE축 Accelation값 설정하기
                        int nBseAxis = IDX_T_AXIS;
                        sprintf(szCNCData, "SA%d,%d;", nBseAxis, nPulse);
                        // VacBoard -> PC -> To CncBoard
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    }
                    // 5축 STAGE
                    else
                    {
                        // BSE축 Accelation값 설정하기
                        int nBseAxis = IDX_B_AXIS;
                        sprintf(szCNCData, "SA%d,%d;", nBseAxis, nPulse);
                        // VacBoard -> PC -> To CncBoard
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    }
                    break;
                }
                }
            }
            else
            {
                // RA Command
                switch (nAxis)
                {
                case IDX_X_AXIS:
                {
                    // X축 Accelation값 표시하기 
                    sprintf(szCNCData, "RA%d;", nAxis);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case IDX_Y_AXIS:
                {
                    // Y축 Accelation값 표시하기 
                    sprintf(szCNCData, "RA%d;", nAxis);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case IDX_Z_AXIS:
                {
                    // Z축 Accelation값 표시하기 
                    sprintf(szCNCData, "RA%d;", nAxis);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case IDX_R_AXIS:
                {
                    // R축 Accelation값 표시하기 
                    sprintf(szCNCData, "RA%d;", nAxis);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case IDX_T_AXIS:
                {
                    // T축 Accelation값 표시하기 
                    sprintf(szCNCData, "RA%d;", nAxis);
                    // VacBoard -> PC -> To CncBoard
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    break;
                }
                case IDX_B_AXIS:
                {
                    // 3축 STAGE
                    if (m_pIntegrated.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_ALL_DATA_VIEW] == FALSE)
                    {
            
                        // BSE축 Accelation값 표시하기 
                        int nBseAxis = IDX_T_AXIS;
                        sprintf(szCNCData, "RA%d;", nBseAxis);
                        // VacBoard -> PC -> To CncBoard
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    }
                    // 5축 STAGE
                    else
                    {
                        // BSE축 Accelation값 표시하기 
                        int nBseAxis = IDX_B_AXIS;
                        sprintf(szCNCData, "RA%d;", nBseAxis);
                        // VacBoard -> PC -> To CncBoard
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCNCData);
                    }
                    break;
                }
                }
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:7230 ----
        //
        // Scan Bridge : "RP" -> Motor 또는 Valve Current Read Position (모터 또는 밸브 현 위치값 수신 및 적용)
        //
        public int AfxScanBridgeRPCommand()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // CNC 상태 및 제어용 윈도우 핸들 확인
            if (m_hStageSetupControlWnd == NULL) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 메시지 처리 전달용 USB Data
            USB_DEVICE_DATA_INFO UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            int bMessage = FALSE;

            // CNC Command 설정 : 'RP' Command
            UsbDeviceDataInfo.nCommand = SBR_CMD_RP;
            
            int nXpix     = 0;
            int nYpix     = 0;
            int nAngle    = 0;
            int nTilt_deg = 0;
            CharBuffer szCom = new CharBuffer(255+1);
            CharBuffer szData = new CharBuffer(255+1);

            // Motor 축(X,Y,Z,R,T,EG,LOD,CLAMP,MGV,BSE)을 설정한다
            int nAxis = atoi(m_pIntegrated.szXStr);

            // 일반적 오류 코드
            if (nAxis < 0 || nAxis > 10) return RET_ERROR_STATUS;

            // AXIS MOVING FLAG : 0->STOP상태, 1->MOVING상태
            m_pIntegrated.nBmpMovingFlag[nAxis] = m_pIntegrated.szDStr[0] - (byte)'0';

            // X Axis <--> Y Axis 반전
            if (m_pIntegrated.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_XY_REVERSE] == TRUE)
            {
                // X축 위치값을 Y축에 적용한다
                if (nAxis == IDX_X_AXIS) nAxis = IDX_Y_AXIS;
                // Y축 위치값을 X축에 적용한다
                else if (nAxis == IDX_Y_AXIS) nAxis = IDX_X_AXIS;
            }

            // 위치정보 전송
            switch (nAxis)
            {
                case IDX_X_AXIS: // X
                {
                    // Motor 축(X,Y,Z,R,T,EG,LOD,CLAMP,MGV,BSE)의 현 위치값(Pulse)을 설정한다
                    m_pIntegrated.nAxisPosition[nAxis] = atoi(m_pIntegrated.szYStr);
                    // 현재 위치값을 [mm] 단위로 계산 한다
                    m_pIntegrated.dCurAxisPos[nAxis] = (double)(m_pIntegrated.nAxisPosition[nAxis]) * m_pIntegrated.dAxisMMPerPulse[nAxis];
                    m_pIntegrated.dCurXPos = m_pIntegrated.dCurAxisPos[nAxis];
                    bMessage = TRUE;

                    ZeroMem(szData);
                    if (m_pIntegrated.dCurAxisPos[nAxis] == 0)
                    {
                        strcpy_s(szData, "0");
                    }
                    else
                    {
                        sprintf(szData, "%g", m_pIntegrated.dCurAxisPos[nAxis]);
                    }
                    // X축 현재 위치값[mm] 를 표시한다
                    UsbDeviceDataInfo.nState = nAxis;
                    strcpy_s(UsbDeviceDataInfo.szData, szData);
                    break;
                }

                case IDX_Y_AXIS: // Y
                {
                    // Motor 축(X,Y,Z,R,T,EG,LOD,CLAMP,MGV,BSE)의 현 위치값(Pulse)을 설정한다
                    m_pIntegrated.nAxisPosition[nAxis] = atoi(m_pIntegrated.szYStr);
                    // 현재 위치값을 [mm] 단위로 계산 한다
                    m_pIntegrated.dCurAxisPos[nAxis] = (double)(m_pIntegrated.nAxisPosition[nAxis]) * m_pIntegrated.dAxisMMPerPulse[nAxis];
                    m_pIntegrated.dCurYPos = m_pIntegrated.dCurAxisPos[nAxis];
                    bMessage = TRUE;

                    ZeroMem(szData);
                    if (m_pIntegrated.dCurAxisPos[nAxis] == 0)
                    {
                        strcpy_s(szData, "0");
                    }
                    else
                    {
                        sprintf(szData, "%g", m_pIntegrated.dCurAxisPos[nAxis]);
                    }
                    // Y축 현재 위치값[mm] 를 표시한다
                    UsbDeviceDataInfo.nState = nAxis;
                    strcpy_s(UsbDeviceDataInfo.szData, szData);
                    break;
                }

                case IDX_Z_AXIS: // Z
                {
                    // Z축 이전값 보관
                    double dOldZPos = m_pIntegrated.dCurZPos;       // Z축 이전값 보관 
                    double dZPosP   = m_pIntegrated.dCurZPos + 0.1; // 0.1[mm] 더하기
                    double dZPosM   = m_pIntegrated.dCurZPos - 0.1; // 0.1[mm] 빼기
                    double dZPosPM  = 0.0;                           // Z축 차이값 ( 현재 - 이전 )

                    // Motor 축(X,Y,Z,R,T,EG,LOD,CLAMP,MGV,BSE)의 현 위치값(Pulse)을 설정한다
                    m_pIntegrated.nAxisPosition[nAxis] = atoi(m_pIntegrated.szYStr);

                    // Z축 현재 위치값을 [mm] 단위로 계산 한다
                    m_pIntegrated.dCurAxisPos[nAxis] = (double)(m_pIntegrated.nAxisPosition[nAxis]) * m_pIntegrated.dAxisMMPerPulse[nAxis];
                    m_pIntegrated.dCurZPos = m_pIntegrated.dCurAxisPos[nAxis];
                    bMessage = TRUE;

                    // Z축 현재 위치값[mm] 를 표시한다
                    ZeroMem(szData);
                    sprintf(szData, "%g", m_pIntegrated.dCurAxisPos[nAxis]);
                    UsbDeviceDataInfo.nState = nAxis;
                    strcpy_s(UsbDeviceDataInfo.szData, szData);

                    // Z-AXIS MOVING or STOP 상태 확인
                    // Z축 이동에 따른 FOCUS 변경은 STOP 상태에서만 실행한다
                    if (m_pIntegrated.nBmpMovingFlag[nAxis] == 0)
                    {
                        // 2021.06.15 : Z축 위치에 따른 FOCUS DEGAUSSING 및 FOCUS COARSE 실행
                        // OPTION : Z AXIS FOCUS LINK ( Z축 이동에 따른 FOCUS 변경 실행 )
                        if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_Z_AXIS_FOCUS_LINK] == 1)
                        {
                            // WD 계산 : (현재 Z축 위치값 - 샘플높이) x 10
                            double dNewWD = (m_pIntegrated.dCurZPos - m_dSampleHeight) * 10.0;                      // 0~500 , 0 ~ 50.0mm
                            int    nNewWD = (int)dNewWD;
                            if (nNewWD <= 500 && m_pIntegrated.nOLWD != nNewWD)
                            {
                                m_pIntegrated.nOLWD = nNewWD;
                                UsbDeviceDataInfo.nData[0] = m_pIntegrated.nOLWD;                                   // WD에 따른 배율조정을 위한 Z축 위치값 0~500 (0~50mm)
                                UsbDeviceDataInfo.nData[1] = m_pIntegrated.nWDOLCoarseLinear[m_pIntegrated.nOLWD]; // WD에 따른 배율조정을 위한 Focus Coarse
                                UsbDeviceDataInfo.nData[2] = 1;                                                      // WD에 따른 배율조정을 실행한다

                                // 2021.08.03 : 아래의 값 nWDPosition의 범위는 0~50 이므로 nOLWD의 범위 0~500와는 호환되지 않는다
                                // WD POSITION 설정
                                //m_pIntegrated->nWDPosition = m_pIntegrated->nOLWD;

                                // FOCUS COARSE 설정
                                m_pIntegrated.nFocusCoarse = m_pIntegrated.nWDOLCoarseLinear[m_pIntegrated.nOLWD];

                                // WD 값이 변동이 있으면 WD에 따른 FOCUS DEGAUSSING 및 FOCUS COARSE 실행한다
                                AfxIntegratedFocusDegaussing(nNewWD);
                            }
                        }
                        /*
                        // 2021.04.21 : Z축 STOP 상태
                        // OPTION : Z AXIS FOCUS LINK ( Z축 이동에 따른 FOCUS 변경 실행 )
                        if (m_pIntegrated->nScanOptionData[IDX_HWSETUP_OPTION_INT_Z_AXIS_FOCUS_LINK] == 1)
                        {
                            // 2021.02.25
                            // 디가우징을 위한 OL값 RESET : WD에 따른 ZOOM 값이 적용되지 않아 실행 한다.
                            AfxIntegratedSetOL(SEM_FOCUS_COARSE_STR, SEM_FOCUS_FINE_STR);

                            // WD 계산 : (현재 Z축 위치값 - 샘플높이) x 10
                            int nNewWD = (int)(m_pIntegrated->dCurZPos - m_dSampleHeight) * 10;                  // 0~500 , 0 ~ 50.0mm

                            // WD 값이 0.1[mm] 이상 변동이 있으면 WD에 따른 FOCUS COARSE 실행한다
                            AfxIntegratedSetWDAutoAdjustFocusCoarse(nNewWD);                                     // 새로운 WD 적용
                            UsbDeviceDataInfo.nData[0] = m_pIntegrated->nOLWD;                                   // WD에 따른 배율조정을 위한 Z축 위치값 0~500 (0~50mm)
                            UsbDeviceDataInfo.nData[1] = m_pIntegrated->nWDOLCoarseLinear[m_pIntegrated->nOLWD]; // WD에 따른 배율조정을 위한 Focus Coarse
                            UsbDeviceDataInfo.nData[2] = 1;                                                      // WD에 따른 배율조정을 실행한다
                            AfxScanBridgeSetScanRotation(m_pIntegrated->nOLWD);                                  // WD에 따른 배율조정을 위한 Scan Rotation 실행

                            // 2021.02.25
                            // FOCUS COARSE, FINE 재설정 : WD에 따른 ZOOM 값이 적용되지 않아 재실행 한다.
                            m_pIntegrated->nFocusOldCoarse = SEM_FOCUS_COARSE_STR;
                            m_pIntegrated->nFocusOldFine   = 1;
                            AfxIntegratedSetOL(m_pIntegrated->nWDOLCoarseLinear[m_pIntegrated->nOLWD], SEM_FOCUS_FINE_STR);
                        }
                        */
                    }

                    break;
                }

                case IDX_R_AXIS: // R
                {
                    // Motor 축(X,Y,Z,R,T,EG,LOD,CLAMP,MGV,BSE)의 현 위치값(Pulse)을 설정한다
                    m_pIntegrated.nAxisPosition[nAxis] = atoi(m_pIntegrated.szYStr);
                    // 현재 위치값 각도(deg) 계산 : 1step 당 회전각도 x 현재 이동Pulse(step) 갯수
                    m_pIntegrated.dCurAxisPos[nAxis] = (double)(m_pIntegrated.nAxisPosition[nAxis]) * m_pIntegrated.dAxisMMPerPulse[nAxis];
                    // 이동한 상대 각도
                    nAngle = abs((int)(m_pIntegrated.dCurAxisPosOld[nAxis] - m_pIntegrated.dCurAxisPos[nAxis]));
                    bMessage = TRUE;

                    ZeroMem(szData);
                    if (m_pIntegrated.dCurAxisPos[nAxis] == 0 || m_pIntegrated.dCurAxisPos[nAxis] == 360.0)
                    {
                        strcpy_s(szData, "0");
                    }
                    else
                    {
                        sprintf(szData, "%g", m_pIntegrated.dCurAxisPos[nAxis]);
                    }
                    // R축 현재 위치값[mm] 를 표시한다
                    UsbDeviceDataInfo.nState = nAxis;
                    strcpy_s(UsbDeviceDataInfo.szData, szData);
                    break;
                }

                case IDX_T_AXIS: // T
                {
                    // Motor 축(X,Y,Z,R,T,EG,LOD,CLAMP,MGV,BSE)의 현 위치값(Pulse)을 설정한다
                    m_pIntegrated.nAxisPosition[nAxis] = atoi(m_pIntegrated.szYStr);
                    // 현재 기울기 각도(deg) 계산 : 1step 당 회전각도 x (현재 이동Pulse(step) 갯수 - 0각도 Pulse 갯수)
                    m_pIntegrated.dCurAxisPos[nAxis] = (double)(m_pIntegrated.nAxisPosition[nAxis] - m_pIntegrated.nTiltZeroAnglePulsePos) * m_pIntegrated.dAxisMMPerPulse[nAxis];
                    bMessage = TRUE;

                    ZeroMem(szData);
                    if (m_pIntegrated.dCurAxisPos[nAxis] == 0)
                    {
                        strcpy_s(szData, "0");
                    }
                    else
                    {
                        sprintf(szData, "%g", m_pIntegrated.dCurAxisPos[nAxis]);
                    }
                    // T축 현재 위치값[mm] 를 표시한다
                    UsbDeviceDataInfo.nState = nAxis;
                    strcpy_s(UsbDeviceDataInfo.szData, szData);
                    break;
                }

                case IDX_BSE_AXIS: // BSE
                {
                    // Motor 축(X,Y,Z,R,T,EG,LOD,CLAMP,MGV,BSE)의 현 위치값(Pulse)을 설정한다
                    m_pIntegrated.nAxisPosition[nAxis] = atoi(m_pIntegrated.szYStr);
                    // 현재 위치값을 [mm] 단위로 계산 한다
                    m_pIntegrated.dCurAxisPos[nAxis] = (double)(m_pIntegrated.nAxisPosition[nAxis]) * m_pIntegrated.dAxisMMPerPulse[nAxis];
                    bMessage = TRUE;

                    ZeroMem(szData);
                    if (m_pIntegrated.dCurAxisPos[nAxis] == 0)
                    {
                        strcpy_s(szData, "0");
                    }
                    else
                    {
                        sprintf(szData, "%g", m_pIntegrated.dCurAxisPos[nAxis]);
                    }
                    // BSE축 현재 위치값[mm] 를 표시한다
                    UsbDeviceDataInfo.nState = nAxis;
                    strcpy_s(UsbDeviceDataInfo.szData, szData);
                    break;
                }

                //-------
                //-------
                //-------

                //case IDX_EG_AXIS: // EG (Extra gate valve)
                //{
                //    // Motor 축(X,Y,Z,R,T,EG,LOD,CLAMP,MGV,BSE)의 현 위치값(Pulse)을 설정한다
                //    m_pIntegrated->nAxisPosition[nAxis] = atoi(m_pIntegrated->szYStr);
                //    // Sample 고정 위치 확인 : Extra gate valve Sample고정위치 + Extra gate valve Home 위치
                //    if (m_pIntegrated->nFixedPositionPulse[nAxis - MAX_MOTOR_COUNT] + m_pIntegrated->nMorePositionPulse[nAxis - MAX_MOTOR_COUNT])
                //    {
                //        nYpix = (int)(EGV_WIDTH_PIX * (double)m_pIntegrated->nAxisPosition[nAxis] / (double)(m_pIntegrated->nFixedPositionPulse[nAxis - MAX_MOTOR_COUNT] + m_pIntegrated->nMorePositionPulse[nAxis - MAX_MOTOR_COUNT]));
                //    }
                //
                //    //최하로 내려가지 않도록 제한
                //    if (nYpix > EGV_WIDTH_PIX - 10) nYpix = EGV_WIDTH_PIX - 10;
                //    /*
                //    if (nYpix != m_pIntegrated->nValvePositionPulseOld[nAxis - MAX_MOTOR_COUNT])
                //    {
                //        ExtraGV_BmpUpdate(nYpix);
                //    }
                //    */
                //    break;
                //}

                case IDX_LOD_AXIS: // LOD     125
                {
                    // Motor 축(X,Y,Z,R,T,EG,LOD,CLAMP,MGV,BSE)의 현 위치값(Pulse)을 설정한다
                    m_pIntegrated.nAxisPosition[nAxis] = atoi(m_pIntegrated.szYStr);
                    // Sample 고정 위치 확인 : Sample Load 고정위치 + Sample Load Home 위치
                    if ((m_pIntegrated.nFixedPositionPulse[nAxis - MAX_MOTOR_COUNT] + m_pIntegrated.nMorePositionPulse[nAxis - MAX_MOTOR_COUNT]) != 0)
                    {
                        nXpix = (int)(SL_WIDTH_PIX * (double)m_pIntegrated.nAxisPosition[nAxis] / (double)(m_pIntegrated.nFixedPositionPulse[nAxis - MAX_MOTOR_COUNT] + m_pIntegrated.nMorePositionPulse[nAxis - MAX_MOTOR_COUNT]));
                    }

                    if (nXpix > SL_WIDTH_PIX) nXpix = SL_WIDTH_PIX;
                    break;
                }

                //---------
                //---------
                //---------

                case IDX_CLAMP_AXIS: // CLAMP
                {
                    break;
                }

                case IDX_MGV_AXIS: // MGV
                {
                    break;
                }

                case IDX_EXPOS_AXIS:  // Exchange Pos으로 이동 완료했다는 Command를 CNC5 Board가 PC로 보냄
                {
                    // 그려면, PC는 VacExtraChamber Board로 다음 아래의 Command 를 보냄
                    ZeroMem(szCom);
                    strcpy_s(szCom, "VL12,1;");
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCom); // Sbr Command
                    break;
                }

                default:
                {
                    break;
                }
            }

            // Axis Current Position 자료 전송
            switch (nAxis)
            {
            case IDX_X_AXIS: // X축 position 값 표시하기 
            case IDX_Y_AXIS: // Y축 position 값 표시하기 
            case IDX_Z_AXIS: // Z축 position 값 표시하기 
            case IDX_R_AXIS: // R축 position 값 표시하기 
            case IDX_T_AXIS: // T축 position 값 표시하기 
            case IDX_B_AXIS: // BSE position 값 표시하기 
            {
                if (bMessage == TRUE)
                {
                    if (m_hOwnerWnd != NULL)
                    {
                        // DEVICE MAIN SERVER
                        // axis current position 값 정보 메시지 보내기
                        // 2020.04.28
                        REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
                        // 초기화
                        RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
                        // 데이터 설정
                        if      (nAxis == IDX_X_AXIS)RequestSetUsbDeviceData.nRequestID = REQIDX_GET_USB_DEVICE_STAGE_POS_X_AXIS;
                        else if (nAxis == IDX_Y_AXIS)RequestSetUsbDeviceData.nRequestID = REQIDX_GET_USB_DEVICE_STAGE_POS_Y_AXIS;
                        else if (nAxis == IDX_Z_AXIS)RequestSetUsbDeviceData.nRequestID = REQIDX_GET_USB_DEVICE_STAGE_POS_Z_AXIS;
                        else if (nAxis == IDX_R_AXIS)RequestSetUsbDeviceData.nRequestID = REQIDX_GET_USB_DEVICE_STAGE_POS_R_AXIS;
                        else if (nAxis == IDX_T_AXIS)RequestSetUsbDeviceData.nRequestID = REQIDX_GET_USB_DEVICE_STAGE_POS_T_AXIS;
                        else if (nAxis == IDX_B_AXIS)RequestSetUsbDeviceData.nRequestID = REQIDX_GET_USB_DEVICE_STAGE_POS_B_AXIS;
                        RequestSetUsbDeviceData.nRequestData[0] = m_pIntegrated.nAxisPosition[nAxis]; // PULSE값
                        RequestSetUsbDeviceData.dRequestData[0] = m_pIntegrated.dCurAxisPos[nAxis];   // 위치값(mm,deg)
                        // 데이터 전송
                        SendMessage(m_hOwnerWnd, UM_REQUEST_GET_USB_DATA, nAxis, RequestSetUsbDeviceData);

            
                        // BSE AXIS DATA
                        if (nAxis == IDX_T_AXIS)
                        {
                            // 3축 STAGE
                            if (m_pIntegrated.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_ALL_DATA_VIEW] == FALSE)
                            {
                                // T-AXIS 데이터를 이용하여 BSE-AXIS 데이터로 활용한다
                                int nBseAxis = IDX_B_AXIS;
                                m_pIntegrated.nAxisPosition[nBseAxis]  = m_pIntegrated.nAxisPosition[IDX_T_AXIS];
                                m_pIntegrated.dCurAxisPos[nBseAxis]    = m_pIntegrated.dCurAxisPos[IDX_T_AXIS];
                                RequestSetUsbDeviceData.nRequestID      = REQIDX_GET_USB_DEVICE_STAGE_POS_B_AXIS;
                                RequestSetUsbDeviceData.nRequestData[0] = m_pIntegrated.nAxisPosition[nBseAxis]; // PULSE값
                                RequestSetUsbDeviceData.dRequestData[0] = m_pIntegrated.dCurAxisPos[nBseAxis];   // 위치값(mm,deg)
                                // 데이터 전송
                                SendMessage(m_hOwnerWnd, UM_REQUEST_GET_USB_DATA, nBseAxis, RequestSetUsbDeviceData);
                            }
                        }

                    }
                }
                break;
            }
            }

            // 위치정보 표시
            switch (nAxis)
            {
            case IDX_X_AXIS: // X
            {
                // X축 현재 위치값[mm] 를 표시한다
                SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, 0, UsbDeviceDataInfo);
                break;
            }

            case IDX_Y_AXIS: // Y
            {
                // Y축 현재 위치값[mm] 를 표시한다
                SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, 0, UsbDeviceDataInfo);
                break;
            }

            case IDX_Z_AXIS: // Z
            {
                // Z축 현재 위치값[mm] 를 표시한다
                SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, 0, UsbDeviceDataInfo);
                break;
            }

            case IDX_R_AXIS: // R
            {
                // R축 현재 위치값[mm] 를 표시한다
                SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, 0, UsbDeviceDataInfo);
                break;
            }

            case IDX_T_AXIS: // T
            {
                // T축 현재 위치값[mm] 를 표시한다
                SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, 0, UsbDeviceDataInfo);

                // 3축 STAGE
                if (m_pIntegrated.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_ALL_DATA_VIEW] == FALSE)
                {

                    // BSE축 현재 위치값[mm] 를 표시한다
                    // T-AXIS 데이터를 이용하여 BSE-AXIS 데이터로 활용한다
                    int nBseAxis = IDX_B_AXIS;
                    m_pIntegrated.nAxisPosition[nBseAxis] = m_pIntegrated.nAxisPosition[IDX_T_AXIS];
                    m_pIntegrated.dCurAxisPos[nBseAxis]   = m_pIntegrated.dCurAxisPos[IDX_T_AXIS];
                    UsbDeviceDataInfo.nState = IDX_B_AXIS;
                    SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, 0, UsbDeviceDataInfo);
                }

                break;
            }
            case IDX_BSE_AXIS: // BSE
            {
                // BSE축 현재 위치값[mm] 를 표시한다
                SendMessage(m_hStageSetupControlWnd, UM_USB_SBR_CNC_STATE, 0, UsbDeviceDataInfo);
                break;
            }

            //-------
            //-------
            //-------

            //case IDX_EG_AXIS: // EG (Extra gate valve)
            //{
            //    break;
            //}

            case IDX_LOD_AXIS: // LOD     125
            {
                break;
            }

            //---------
            //---------
            //---------

            case IDX_CLAMP_AXIS: // CLAMP
            {
                break;
            }

            case IDX_MGV_AXIS: // MGV
            {
                break;
            }

            case IDX_EXPOS_AXIS:  // Exchange Pos으로 이동 완료했다는 Command를 CNC5 Board가 PC로 보냄
            {
                break;
            }

            default:
            {
                break;
            }
            }

            return RET_SUCCESS_OK;
        }

    }
}
