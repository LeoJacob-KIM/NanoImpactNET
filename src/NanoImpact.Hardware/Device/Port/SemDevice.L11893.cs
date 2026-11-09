// 원본 USB_FT232H_Device.cpp:11893-14965 을 NanoImpact.CodeGen translate로 옮긴 뒤 손으로 고친 코드.
#pragma warning disable IDE1006, IDE0059, CS0162, CS0168, CS0219
using System;
using NanoImpact.Core.Native;
using static NanoImpact.Core.Def;
using static NanoImpact.Core.Native.CRuntime;

namespace NanoImpact.Hardware.Device
{
    public sealed partial class SemDevice
    {
        // ---- USB_FT232H_Device.cpp:11893 ----
        //
        // Scan Bridge : Program Exit 실행 -> 장치오류시 Sem PowerOff 모드로 종료
        //
        public int AfxScanBridgeProgramExitDeviceError()
        {
            //if (m_pSbrCommand == NULL) return RET_ERROR_SBR_COMMAND_HANDLE;
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            //
            // Scan Bridge 장치가 연결되어 있다면
            // SEM Power Off
            //
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "PS;");  // Sbr Command
            Sleep(100);
            //
            // Stage Board : Save axis position and power off code
            //
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "PS1;"); // Sbr Command
            Sleep(100);
            //
            // SEM Power Off (Shutdown)
            //
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "PD;");  // Sbr Command
            Sleep(100);
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "SB0;"); // Sbr Command
            Sleep(100);
            //

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:11939 ----
        //---
        // MM : Motor Move            Ex) [MM0,1234,56788;]
        //                                 XStr,YStr,DStr
        //---
        //      XStr[] : Axis  [ '0', '1', '2', '3', '4' ]
        //                        X    Y    Z    R    T    
        //      YStr[] : Accel [ 16 bit ]
        //      DStr[] : Time  [ 32 bit ]
        //---
        // X, Y, Z, R, T Axis Motion
        //---
        //#define  NO_MOVING_ECHO  0
        //#define  ENDING_ECHO     1
        //#define  MOVING_ECHO     2
        //---
        //
        // Scan Bridge : Motor Axis Motion
        //
        public double AfxScanBridgeAxisMotion(int nAxis, double dDistance, int nEchoFlag)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            CharBuffer szBuf = new CharBuffer(20+1);
            CharBuffer szCom = new CharBuffer(25+1);

            ZeroMem(szBuf);
            ZeroMem(szCom);

            switch (nAxis)
            {
                case X_AXIS:
                {
                    strcpy_s(szCom, "MM0,");
                    break;
                }
                case Y_AXIS:
                {
                    strcpy_s(szCom, "MM1,");
                    break;
                }
                case Z_AXIS:
                {
                    strcpy_s(szCom, "MM2,");
                    break;
                }
                case R_AXIS:
                {
                    strcpy_s(szCom, "MM3,");
                    break;
                }
                case T_AXIS:
                {
                    strcpy_s(szCom, "MM4,");
                    break;
                }
            }

            sprintf( szBuf, "%.5f", dDistance);
            strcat(szCom, szBuf);

            if (nEchoFlag == 1)
            {
                strcat(szCom, ",1");
            }
            else
            if (nEchoFlag == 2)
            {
                strcat(szCom, ",2");
            }

            strcat(szCom, ";");

            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCom); // Sbr Command

            return atof(szBuf);
        }

        // ---- USB_FT232H_Device.cpp:12002 ----
        //
        // Scan Bridge : X-Axis [mm] 이동
        //
        public int AfxScanBridgeXmmMotion(double dMM, int nEchoFlag)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            CharBuffer szBuf = new CharBuffer(20+1);
            CharBuffer szCom = new CharBuffer(25+1);

            ZeroMem(szBuf);
            ZeroMem(szCom);

            strcpy_s(szCom, "MM0,");
            sprintf(szBuf, "%.5f", dMM); // 이동거리[mm]
            strcat(szCom, szBuf);

            if (nEchoFlag == 1)
            {
                strcat(szCom, ",1");
            }
            else
            if (nEchoFlag == 2)
            {
                strcat(szCom, ",2");
            }

            strcat(szCom, ";");

            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCom); // Sbr Command

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12036 ----
        //
        // Scan Bridge : Y-Axis [mm] 이동
        //
        public int AfxScanBridgeYmmMotion(double dMM, int nEchoFlag)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            CharBuffer szBuf = new CharBuffer(20 + 1);
            CharBuffer szCom = new CharBuffer(25 + 1);

            ZeroMem(szBuf);
            ZeroMem(szCom);

            strcpy_s(szCom, "MM1,");
            sprintf(szBuf, "%.5f", dMM); // 이동거리[mm]
            strcat(szCom, szBuf);

            if (nEchoFlag == 1)
            {
                strcat(szCom, ",1");
            }
            else
            if (nEchoFlag == 2)
            {
                strcat(szCom, ",2");
            }

            strcat(szCom, ";");

            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCom); // Sbr Command

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12070 ----
        //
        // Scan Bridge : Z-Axis [mm] 이동
        //
        public int AfxScanBridgeZmmMotion(double dMM, int nEchoFlag)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            CharBuffer szBuf = new CharBuffer(20 + 1);
            CharBuffer szCom = new CharBuffer(25 + 1);

            ZeroMem(szBuf);
            ZeroMem(szCom);

            strcpy_s(szCom, "MM2,");
            sprintf(szBuf, "%.5f", dMM); // 이동거리[mm]
            strcat(szCom, szBuf);

            if (nEchoFlag == 1)
            {
                strcat(szCom, ",1");
            }
            else
                if (nEchoFlag == 2)
                {
                    strcat(szCom, ",2");
                }

            strcat(szCom, ";");

            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCom); // Sbr Command

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12104 ----
        //
        // Scan Bridge : R-Axis [deg:상대각도] 이동
        //
        public int AfxScanBridgeRdegMotion(double dDeg, int nEchoFlag)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            CharBuffer szBuf = new CharBuffer(20 + 1);
            CharBuffer szCom = new CharBuffer(25 + 1);

            ZeroMem(szBuf);
            ZeroMem(szCom);

            strcpy_s(szCom, "MM3,");
            sprintf(szBuf, "%3.1f", dDeg); // 상대적 이동 각도
            strcat(szCom, szBuf);

            if (nEchoFlag == 1)
            {
                strcat(szCom, ",1");
            }
            else
            if (nEchoFlag == 2)
            {
                strcat(szCom, ",2");
            }

            strcat(szCom, ";");

            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCom); // Sbr Command

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12138 ----
        //
        // Scan Bridge : R-Axis [abs_deg:절대각도] 이동
        //
        public int AfxScanBridgeRabsMotion(double dAbsDeg, int nEchoFlag)
        {
            //if (m_pSbrCommand == NULL) return RET_ERROR_SBR_COMMAND_HANDLE;
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            CharBuffer szBuf = new CharBuffer(20 + 1);
            CharBuffer szCom = new CharBuffer(25 + 1);

            ZeroMem(szBuf);
            ZeroMem(szCom);

            strcpy_s(szCom, "MM3,");
            sprintf(szBuf, "%3.1f", dAbsDeg); // 절대적 이동 각도 (0~359)
            strcat(szCom, szBuf);

            if (nEchoFlag == 1)
            {
                strcat(szCom, ",1");
            }
            else
            if (nEchoFlag == 2)
            {
                strcat(szCom, ",2");
            }

            strcat(szCom, ";");

            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCom); // Sbr Command

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12173 ----
        //
        // Scan Bridge : T-Axis [deg:상대각도] 이동
        //
        public int AfxScanBridgeTdegMotion(double dDeg, int nEchoFlag)
        {
            //if (m_pSbrCommand == NULL) return RET_ERROR_SBR_COMMAND_HANDLE;
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            CharBuffer szBuf = new CharBuffer(20 + 1);
            CharBuffer szCom = new CharBuffer(25 + 1);

            double dDegx10 = dDeg; // 상대적 이동 각도 (x10)
            m_pIntegrated.dTAxisDegree += dDegx10;
            m_pIntegrated.nTAxisDegree = (int)m_pIntegrated.dTAxisDegree;

            if (m_pIntegrated.nTAxisDegree < 0)
            {
                m_pIntegrated.nTAxisDegree += 3600;
                m_pIntegrated.dTAxisDegree = (double)m_pIntegrated.nTAxisDegree;
            }
            else
            if (m_pIntegrated.nTAxisDegree >= 3600)
            {
                m_pIntegrated.nTAxisDegree -= 3600;
                m_pIntegrated.dTAxisDegree = (double)m_pIntegrated.nTAxisDegree;
            }

            ZeroMem(szBuf);
            ZeroMem(szCom);

            strcpy_s(szCom, "MM4,");
            sprintf(szBuf, "%3.1f", dDegx10); // 상대적 이동 각도 (x10)
            strcat(szCom, szBuf);

            if (nEchoFlag == 1)
            {
                strcat(szCom, ",1");
            }
            else
            if (nEchoFlag == 2)
            {
                strcat(szCom, ",2");
            }

            strcat(szCom, ";");

            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCom); // Sbr Command

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12224 ----
        //
        // Scan Bridge : T-Axis [mm] 이동
        //
        public int AfxScanBridgeTmmMotion(double dMM, int nEchoFlag)
        {
            //if (m_pSbrCommand == NULL) return RET_ERROR_SBR_COMMAND_HANDLE;
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            CharBuffer szBuf = new CharBuffer(20 + 1);
            CharBuffer szCom = new CharBuffer(25 + 1);

            ZeroMem(szBuf);
            ZeroMem(szCom);

            strcpy_s(szCom, "MM4,");
            sprintf(szBuf, "%3.1f", dMM); // 틸트 거리[mm] 이동
            strcat(szCom, szBuf);

            if (nEchoFlag == 1)
            {
                strcat(szCom, ",1");
            }
            else
            if (nEchoFlag == 2)
            {
                strcat(szCom, ",2");
            }

            strcat(szCom, ";");

            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCom); // Sbr Command

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12259 ----
        //
        // Scan Bridge : Axis Limit 이동 명령
        //
        public int AfxScanBridgeSetAxisLimitMotion(int nAxis)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Axis Check?
            if (nAxis < IDX_X_AXIS || nAxis > IDX_T_AXIS) return RET_ERROR_STATUS;

            CharBuffer szCom = new CharBuffer(25 + 1);
            ZeroMem(szCom);

            // Axis Set Limit Command
            sprintf(szCom, "MH%d,1;", nAxis);

            // Sbr Command 전송
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCom); 

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12281 ----
        //
        // Scan Bridge : Limit Switch 상태 정보 전송 기능 활성화 설정
        //
        public int AfxScanBridgeSetAxisLimitStateSending(int nSending)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Sending Check?

            CharBuffer szCom = new CharBuffer(25 + 1);
            ZeroMem(szCom);

            if (nSending == 1)
            {
                // Limit Switch State Sending On
                sprintf(szCom, "LS1;");
            }
            else
            {
                // Limit Switch State Sending Off
                sprintf(szCom, "LS0;");
            }

            // Sbr Command 전송
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCom);

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12310 ----
        //
        // Scan Bridge : Go to XTZRT-Sample Loading Position --> Scan Position (스캔 위치로 이동)
        //
        public int AfxScanBridgeGotoXYZRTSampleLoadPosition()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            CharBuffer szCom = new CharBuffer(255 + 1);

            // VacBoard -> PC -> To CncBoard
            ZeroMem(szCom);
            strcpy_s(szCom, "SB20;");
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCom); // Sbr Command

            // SAMPLE SCAN POSITION MOVE
            ZeroMem(szCom);
            strcpy_s(szCom, "VL9,1;");
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCom); // Sbr Command

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12332 ----
        //
        // Scan Bridge : Go to XTZRT-Sample UnLoading Position --> Exchange Position (교환 위치로 이동)
        //
        public int AfxScanBridgeGotoXYZRTSampleUnloadPosition()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            CharBuffer szCom = new CharBuffer(255 + 1);
            ZeroMem(szCom);

            // VacBoard -> PC -> To CncBoard
            ZeroMem(szCom);
            strcpy_s(szCom, "SB21;");
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCom); // Sbr Command

            // SAMPLE EXCHANGE POSITION MOVE
            ZeroMem(szCom);
            strcpy_s(szCom, "VL9,0;");
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCom); // Sbr Command

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12355 ----
        //
        // Scan Bridge : Stage Axis Emergency Stop Execute ( STAGE AXIS 긴급정지 실행)
        //
        public int AfxScanBridgeStageAxisEmergencyStopExecute()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // 2021.03.25
            // AXIS ALL STOP 으로 대체
            AfxScanBridgeASCommand();

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12369 ----
        //
        // Scan Bridge : Stage Axis Reset Control Execute ( STAGE RESET CONTROL 실행)
        //
        public int AfxScanBridgeStageAxisResetControlExecute()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // 2021.04.02
            // STAGE RESET CONTROL

            // X-AXIS HOME 실행
            AfxScanBridgeMHCommand(IDX_X_AXIS);

            // Y-AXIS HOME 실행
            AfxScanBridgeMHCommand(IDX_Y_AXIS);

            // Z-AXIS HOME 실행
            AfxScanBridgeMHCommand(IDX_Z_AXIS);

            // STAGE AXIS 정보 읽기
            //AfxThreadCNCSendProcessing(NULL);

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12395 ----
        //
        // Scan Bridge : SEM StandBy(대기모드) 설정
        //
        public int AfxScanBridgeSetSemStandbyMode()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 2021.07.23 : 사용하지 않는 기능임.
            /*
            // SEM 고압사용(HV_ON_STATE) 중 일 경우에만, 실행한다...
            if (m_pIntegrated->nSemVacState != HV_ON_STATE) return RET_SUCCESS_OK;

            //---------------
            // Freeze Mode(일시정지 모드)
            //---------------
            // 스캔 중인 이미지를 정지 시킨다 (SE Image Scan, Photo Image Scan, BSE Image Scan 등)
            m_pIntegrated->bScanDataReceive = FALSE; // SEM SE Image Scan 허용 Flag
            m_pIntegrated->bPhotoGoOnFlag   = FALSE; // SEM SE Image Photo Scan 허용 Flag
            
            // 기존 C++Builder 프로그램에서는 Image Update를 타이머로 처리하였음...
            //PhotoViewTimer->Enabled = false;       // SE Image Photo Scan 처리 타이머 : Interval 10[ms]
            //ImageViewTimer->Enabled = false;         // SE Image Scan 처리 타이머: Interval 50[ms]
            // 프레임 ImgBuf_A에 입력된 영상을 Frame Average 값을 구한 후, m_pBitmap_A 에 저장하는 과정을
            // Thread에서 연속적인 작업과정을 진행하고,
            // ImageViewTimer에서는 Thread와 무관하게 Screen에 영상을 50ms 마다 반복적으로 표시한다.
            //---------------

            // Gun HV 및 Bias HV 전압 등을 다운시킨다...
            m_pIntegrated->nGunHVCount   = 0;
            m_pIntegrated->nProcessValue = m_pIntegrated->nFilamentValue;
            m_pIntegrated->nGunHVProcess = FIL_HV_DOWN_PROC;
            // BSE 활성화 처리 설정
            m_pIntegrated->bBseActiveFlag = FALSE;
            */

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12434 ----
        //

        //
        // Scan Bridge : Set SEM Air ON/OFF : ON->챔버 진공해제상태, OFF->진공중 또는 진공완료상태
        //
        public int AfxScanBridgeSetSEMAirOnOff()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            CharBuffer szCom = new CharBuffer(255 + 1);

            if (m_pIntegrated.nSemVacState == AIR_ON_STATE)
            {
                // Air-Off ( Evac ) -> (상태 : 진공 진행중...)
                // SB : Set Exchange Sample Position Pulse 으로 변경

                ZeroMem(szCom);
                strcpy_s(szCom, "AR0,0;"); // ScanBridgeBoard -> VacBoard
                AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCom);

                ZeroMem(szCom);
                strcpy_s(szCom, "SB1,0;"); // ScanBridgeBoard -> CncVacBoard (EM40)
                AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCom);
            }
            else
            {
                // VAC_EVAC_STATE : 진공 진행중
                // VAC_OK_STATE   : 진공 상태
                // HV_ON_STATE    : 진공 상태가 HV 사용 가능 상태에 도달함
                // 챔버 진공(EVAC,VAC,HV)상태에서 -> 진공해제 상태로 전환...

                if (m_pIntegrated.nSemVacState == HV_ON_STATE)
                {
                    // 진공상태 및 고압사용 상태이면...
                    // HV On 상태이면, AIR ON 상태로 전환할 수 없다
                    return RET_SUCCESS_OK;
                }

                // Air-On -> (상태 : SEM 대기상태...)
                // SB : Set Exchange Sample Position Pulse 으로 변경

                ZeroMem(szCom);
                strcpy_s(szCom, "AR0,1;"); // ScanBridgeBoard -> VacBoard
                AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCom);

                ZeroMem(szCom);
                strcpy_s(szCom, "SB1,1;"); // ScanBridgeBoard -> CncVacBoard (EM40)
                AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCom);
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12485 ----
        //
        // Scan Bridge : Set SEM AIR ON ( CHAMBER OPEN )
        //
        public int AfxScanBridgeSetSEMAirOnExecute()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            CharBuffer szCom = new CharBuffer(255 + 1);

            if (m_pIntegrated.nSemVacState == HV_ON_STATE)
            {
                // HV On 상태이면, AIR ON 상태로 전환할 수 없다
                return RET_SUCCESS_OK;
            }

            // AIR-ON 상태 실행
            ZeroMem(szCom);
            strcpy_s(szCom, "AR0,1;"); // ScanBridgeBoard -> VacBoard
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCom);

            // 대기
            //AfxSystemWaitTime(100);

            ZeroMem(szCom);
            strcpy_s(szCom, "SB1,1;"); // ScanBridgeBoard -> CncVacBoard (EM40)
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCom);

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12515 ----
        //
        // Scan Bridge : Set SEM AIR OFF ( CHAMBER CLOSE )
        //
        public int AfxScanBridgeSetSEMAirOffExecute()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            CharBuffer szCom = new CharBuffer(255 + 1);

            if (m_pIntegrated.nSemVacState == HV_ON_STATE)
            {
                // HV On 상태이면, AIR OFF 상태로 전환할 수 없다
                return RET_SUCCESS_OK;
            }

            // AIR-OFF (EVAC:진공 진행중) 상태 실행
            ZeroMem(szCom);
            strcpy_s(szCom, "AR0,0;"); // ScanBridgeBoard -> VacBoard
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCom);

            // 대기
            //AfxSystemWaitTime(100);

            ZeroMem(szCom);
            strcpy_s(szCom, "SB1,0;"); // ScanBridgeBoard -> CncVacBoard (EM40)
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, szCom);

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12589 ----
        //
        //
        //
        // SEM SE-Image Device Command Control...
        //
        //
        //








        //
        //
        //
        // BSE Image Device Command Control...
        //
        //
        //






        //
        //
        //
        // Scan Bridge, Sem SE-Image, BSE Image Device Command Integrated(통합) Control...
        //
        //
        //

        //
        // Integrated Control : Scintillator HV ON : 10kV
        //
        public int AfxIntegratedScintilatorHVOn()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // a) Scan Image Board에서
            m_pIntegrated.btImageAvrBPort |= NEW_DET_HV_ON;
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SEM_IMAGE_DEVICE, "MA5,", m_pIntegrated.btImageAvrBPort); // SE-Image Command 전송

            // b) SpellMan Scintillator ON
            //AfxIntegratedSpellmanScintilatorHVOn();

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12602 ----
        public int AfxIntegratedSpellmanScintilatorHVOn()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // b) SpellMan Scintillator ON
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RG2,0;"); // Sbr Data 전송

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12615 ----
        //
        // Integrated Control : Scintillator HV OFF : 10kV
        //
        public int AfxIntegratedScintilatorHVOff()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // a) Scan Image Board에서
            m_pIntegrated.btImageAvrBPort &= NEW_DET_HV_OFF;
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SEM_IMAGE_DEVICE, "MA5,", m_pIntegrated.btImageAvrBPort); // SE-Image Command 전송

            // b) SpellMan Scintillator OFF
            //AfxIntegratedSpellmanScintilatorHVOff();

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12628 ----
        public int AfxIntegratedSpellmanScintilatorHVOff()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // b) SpellMan Scintillator OFF
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RG2,1;"); // Sbr Data 전송

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12641 ----
        //
        // Integrated Control : PMT HV ON
        //
        public int AfxIntegratedPMTHVOn()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // a) Scan Image Board에서
            m_pIntegrated.btImageAvrBPort |= NEW_PMT_HV_ON;
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SEM_IMAGE_DEVICE, "MA5,", m_pIntegrated.btImageAvrBPort); // SE-Image Command 전송

            // b) SpellMan PMT ON
            //AfxIntegratedSpellmanPMTHVOn();

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12654 ----
        public int AfxIntegratedSpellmanPMTHVOn()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // b) SpellMan PMT ON
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RG1,0;"); // Sbr Data 전송

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12667 ----
        //
        // Integrated Control : PMT HV OFF
        //
        public int AfxIntegratedPMTHVOff()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // a) Scan Image Board에서
            m_pIntegrated.btImageAvrBPort &= NEW_PMT_HV_OFF;
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SEM_IMAGE_DEVICE, "MA5,", m_pIntegrated.btImageAvrBPort); // SE-Image Command 전송

            // b) SpellMan PMT OFF
            //AfxIntegratedSpellmanPMTHVOff();

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12680 ----
        public int AfxIntegratedSpellmanPMTHVOff()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // b) SpellMan PMT OFF
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RG1,1;"); // Sbr Data 전송

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12693 ----
        //
        // Integrated Control : PMT HV Relay ON
        //
        public int AfxIntegratedSetPMTHVRelayOn()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // a) Gnd 통합 Board에서
            m_pIntegrated.wRelayState |= PMT_HV_ON;
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_RelayCntl, m_pIntegrated.wRelayState); // SE-Image Command 전송

            // b) Gnd 분리 Board에서
            m_pIntegrated.btImgRelayState |= NEW_PMT_HV_ON;
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SEM_IMAGE_DEVICE, "MA5,", m_pIntegrated.btImgRelayState); // SE-Image Command 전송

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12711 ----
        //
        // Integrated Control : PMT HV Relay OFF
        //
        public int AfxIntegratedSetPMTHVRelayOff()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // a) Gnd 통합 Board에서
            m_pIntegrated.wRelayState &= PMT_HV_OFF;
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_RelayCntl, m_pIntegrated.wRelayState); // SE-Image Command 전송

            // b) Gnd 분리 Board에서
            m_pIntegrated.btImgRelayState &= NEW_PMT_HV_OFF;
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SEM_IMAGE_DEVICE, "MA5,", m_pIntegrated.btImgRelayState); // SE-Image Command 전송

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12729 ----
        //
        // Integrated Control : DET(Detection) HV ON
        //
        public int AfxIntegratedDETHVOn()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Detection HV ON

            // a) Scan Image Board에서
            m_pIntegrated.btImageAvrBPort |= NEW_DET_HV_ON;
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SEM_IMAGE_DEVICE, "MA5,", m_pIntegrated.btImageAvrBPort); // SE-Image Command 전송

            // b) SpellMan DET ON - Scintilator
            //AfxIntegratedSpellmanScintilatorHVOn();

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12748 ----
        //
        // Integrated Control : DET(Detection) HV OFF
        //
        public int AfxIntegratedDETHVOff()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Detection HV OFF

            // a) Scan Image Board에서
            m_pIntegrated.btImageAvrBPort &= NEW_DET_HV_OFF;
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SEM_IMAGE_DEVICE, "MA5,", m_pIntegrated.btImageAvrBPort); // SE-Image Command 전송

            // b) SpellMan DET OFF
            //AfxIntegratedSpellmanScintilatorHVOff();

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12767 ----
        //
        // Integrated Control : DET HV Relay ON
        //
        public int AfxIntegratedSetDETHVRelayOn()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // a) Gnd 통합 Board에서
            m_pIntegrated.wRelayState |= DET_HV_ON;
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_RelayCntl, m_pIntegrated.wRelayState); // SE-Image Command 전송

            // b) Gnd 분리 Board에서
            m_pIntegrated.btImgRelayState |= NEW_DET_HV_ON;
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SEM_IMAGE_DEVICE, "MA5,", m_pIntegrated.btImgRelayState); // SE-Image Command 전송

            // c) Image Scan Base All Board에서
            m_pIntegrated.btImgRelayState |= NEW_IR_LED_ON;
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SEM_IMAGE_DEVICE, "MA5,", m_pIntegrated.btImgRelayState); // SE-Image Command 전송

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12789 ----
        //
        // Integrated Control : DET HV Relay OFF
        //
        public int AfxIntegratedSetDETHVRelayOff()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // a) Gnd 통합 Board에서
            m_pIntegrated.wRelayState &= DET_HV_OFF;
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_RelayCntl, m_pIntegrated.wRelayState); // SE-Image Command 전송

            // b) Gnd 분리 Board에서
            m_pIntegrated.btImgRelayState &= NEW_DET_HV_OFF;
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SEM_IMAGE_DEVICE, "MA5,", m_pIntegrated.btImgRelayState); // SE-Image Command 전송

            // c) Image Scan Base All Board에서
            m_pIntegrated.btImgRelayState &= NEW_IR_LED_OFF;
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SEM_IMAGE_DEVICE, "MA5,", m_pIntegrated.btImgRelayState); // SE-Image Command 전송

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12811 ----
        //
        // Integrated Control : IMAGE CH2 ON ( BSE IMAGE )
        //
        public int AfxIntegratedBSEImageOn()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // IMAGE CH2 ON : BSE IMAGE

            // 2021.07.23
            m_pIntegrated.bBseActiveFlag = TRUE;

            // a) Scan Image Board에서
            m_pIntegrated.btImageAvrBPort |= NEW_IMG_CH2_ON;
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SEM_IMAGE_DEVICE, "MA5,", m_pIntegrated.btImageAvrBPort); // SE-Image Command 전송

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12830 ----
        //
        // Integrated Control : IMAGE CH2 OFF ( SE IMAGE )
        //
        public int AfxIntegratedBSEImageOff()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // IMAGE CH2 OFF : SE IMAGE

            // 2021.07.23
            m_pIntegrated.bBseActiveFlag = FALSE;

            // a) Scan Image Board에서
            m_pIntegrated.btImageAvrBPort &= NEW_IMG_CH2_OFF;
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SEM_IMAGE_DEVICE, "MA5,", m_pIntegrated.btImageAvrBPort); // SE-Image Command 전송

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12849 ----
        //
        // Integrated Control : Emission Current 자동 수신 인터벌 타임 설정
        //
        public int AfxIntegratedSetEmissionReceive()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // 1초 간격으로 Emission Current 자동 수신 설정
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "AE10;"); // Sbr Data 전송

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12862 ----
        //
        // Integrated Control : Emission Current 자동 수신 인터벌 타임 설정
        //
        public int AfxIntegratedSetEmissionReceiveStop()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Emission Current 자동 수신 설정 종료
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "AE0;"); // Sbr Data 전송

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12875 ----
        //
        // Integrated Control : Contrast Value 설정
        //
        public int AfxIntegratedSetContrast()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // 1) Image Board 에서 제어할 경우
            m_pIntegrated.nAD5361ContrastValue = AD5361_PMT + ((m_pIntegrated.nContrastValue + 4096) << 3);
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, m_pIntegrated.nAD5361ContrastValue); // SE-Image Command 전송

            // 2) Spellman 고압에서 제어할 경우
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RF", m_pIntegrated.nContrastValue); // Sbr Data 전송

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12888 ----
        public int AfxIntegratedSetContrastZero()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // CONTRAST ZERO ( PMT HV ZERO )
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_PMT + (4096 << 3)); // SE-Image Command 전송

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12897 ----
        public int AfxIntegratedSetContrast(int nContrast)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Contrast 설정
            m_pIntegrated.nContrastValue = nContrast;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12906 ----
        public int AfxIntegratedGetContrast()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Contrast 설정
            return m_pIntegrated.nContrastValue;
        }

        // ---- USB_FT232H_Device.cpp:12917 ----
        //
        // Integrated Control : Brightness Value 설정
        //
        public int AfxIntegratedSetBrightness()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Brightness 설정
            m_pIntegrated.nAD5361BrightnessValue = AD5361_BRT + (m_pIntegrated.nBrightValue << 4);
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, m_pIntegrated.nAD5361BrightnessValue); // SE-Image Command 전송

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12927 ----
        public int AfxIntegratedSetBrightness(int nBright)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Brightness 설정
            m_pIntegrated.nBrightValue = nBright;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12936 ----
        public int AfxIntegratedGetBrightness()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Brightness 설정
            return m_pIntegrated.nBrightValue;
        }

        // ---- USB_FT232H_Device.cpp:12947 ----
        //
        // Integrated Control : W.D 값에 따른 OL전압, Scan Rotation, 배율보정을 적용할 것인지 여부 Flag
        //
        public int AfxIntegratedSetWDSetupFlag(int bWDSetup)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Set W.D 값에 따른 OL전압, Scan Rotation, 배율보정
            m_pIntegrated.bWDSetupFlag = bWDSetup;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12956 ----
        public int AfxIntegratedGetWDSetupFlag()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Get W.D 값에 따른 OL전압, Scan Rotation, 배율보정
            return m_pIntegrated.bWDSetupFlag;
        }

        // ---- USB_FT232H_Device.cpp:12967 ----
        //
        // Integrated Control : W.D 교정값 DISPLAY 여부 Flag
        //
        public int AfxIntegratedSetWDDataDisplayFlag(int bWDSetup)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Set W.D 교정값 DISPLAY 여부 Flag
            m_pIntegrated.bWDDataDisplayFlag = bWDSetup;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12976 ----
        public int AfxIntegratedGetWDDataDisplayFlag()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Get W.D 교정값 DISPLAY 여부 Flag
            return m_pIntegrated.bWDDataDisplayFlag;
        }

        // ---- USB_FT232H_Device.cpp:12987 ----
        //
        // Integrated Control : Stage 수평값 적용 Check
        //
        public int AfxIntegratedSetStageHorAlign(int bStageHorAlign)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 하드웨어 설정 기능은 사용하지 않으며, Check 여부에 따라 다른 Sub 기능에서 참조 하여 기능을 사용함.
            // Set Stage Hor Align Check
            m_pIntegrated.bStageHorAlignCheck = bStageHorAlign;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:12997 ----
        public int AfxIntegratedGetStageHorAlign()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Get Stage Hor Align Check
            return m_pIntegrated.bStageHorAlignCheck;
        }

        // ---- USB_FT232H_Device.cpp:13008 ----
        //
        // Integrated Control : WD S-Rotation 보정값 적용 Check
        //
        public int AfxIntegratedSetWDSRotationAdjust(int bWDSRotationAdjust)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 하드웨어 설정 기능은 사용하지 않으며, Check 여부에 따라 다른 Sub 기능에서 참조 하여 기능을 사용함.
            // Set WD S-Rotation Adjust Check
            m_pIntegrated.bWDSRotationAdjustFlag = bWDSRotationAdjust;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:13018 ----
        public int AfxIntegratedGetWDSRotationAdjust()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Get WD S-Rotation Adjust Check
            return m_pIntegrated.bWDSRotationAdjustFlag;
        }

        // ---- USB_FT232H_Device.cpp:13029 ----
        //
        // Integrated Control : WD Scale Zooming 보정값 적용 Check
        //
        public int AfxIntegratedSetWDScaleZoomAdjust(int bWDScaleZoomAdjust)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 하드웨어 설정 기능은 사용하지 않으며, Check 여부에 따라 다른 Sub 기능에서 참조 하여 기능을 사용함.
            // Set WD Scale Zooming Adjust Check
            m_pIntegrated.bWDScaleAdjustFlag = bWDScaleZoomAdjust;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:13039 ----
        public int AfxIntegratedGetWDScaleZoomAdjust()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Get WD Scale Zooming Adjust Check
            return m_pIntegrated.bWDScaleAdjustFlag;
        }

        // ---- USB_FT232H_Device.cpp:13050 ----
        //
        // Integrated Control : Hitachi SM-2500 HV 10K 설정
        //
        public int AfxIntegratedSetSM2500Hv10k(int nIndex)
        {
            //if (m_pSbrCommand == NULL) return RET_ERROR_SBR_COMMAND_HANDLE;
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            // Hitachi SM-2500 HV 10K 설정
            switch (nIndex)
            {
                case  0: // All Off
                {
                    //Sbr_StrAvrSend("PO0;"); 
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "PO0;"); // Sbr Data 전송
                    break;
                }
                case  1: // Lower - POST1 - ON
                {
                    //Sbr_StrAvrSend("PO1;"); 
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "PO1;"); // Sbr Data 전송
                    break;
                }
                case  2: // Upper - POST2 - ON
                {
                    //Sbr_StrAvrSend("PO2;"); 
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "PO2;"); // Sbr Data 전송
                    break;
                }
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:13084 ----
        //
        // Integrated Control : Scan Rotation Value 설정 -> W.D 값에 따른 OL전압, Scan Rotation, 배율보정을 적용할 것인지 여부
        //
        public int AfxIntegratedSetScanRotationValue(int nValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // W.D 값에 따른 보정값(사용자입력값)을 적용할 것인지 여부
            // 보정값 종류 : OLC(ObjectLensCoarse), Scan Rotation, 배율보정
            if (m_pIntegrated.bWDSetupFlag == TRUE)
            {
                // 이전 프로그램 작업한 내용
                // ScanRotation() 자동적으로 수행됨
                //HwSetupForm->WD_SRotCSpinEdit->Value = SRotationValue;

                // 2018.10.11
                // 니중에 작업을 추가한다
            }
            else
            {
                // Scan Rotation 설정 및 실행하기 
                //ScanRotation(SRotationValue);
                // Rotation 범위 : 0 ~ 3599 -> 0.0도~359.9도
                AfxScanBridgeSetScanRotation(nValue);
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:13113 ----
        //
        // Integrated Control : Program Start SeCommand Value Set
        //
        public int AfxIntegratedSetSeCommandStartValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 스캔모드 설정
            m_pIntegrated.nScanMode = FAST_SCAN_MODE;

            // 프로그램 처음 시작시 SeCommand 값을 설정한다
            // 기존 프로그램에서는 필터값을 적용하여 0x0010(16) 이었다
            m_pIntegrated.nSeCommandValue &= SEL_FILT_MASK;
            m_pIntegrated.nSeCommandValue |= m_wFilter[m_pIntegrated.nFilterNum[m_pIntegrated.nScanMode]];
            // Se Command 전송
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_OUTPORT, m_pIntegrated.nSeCommandValue);

            // 프로그램 처음 시작시 CommandState 값을 설정한다
            // 기존 프로그램에서는 필터값을 적용하여 0 이었다
            m_pIntegrated.nSbrCommandState = 0;
            // Sbr Command 전송
            AfxFT232HPortSendCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, cWR_OUTPORT, m_pIntegrated.nSbrCommandState);

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:13139 ----
        //
        // Integrated Control : Screen Mode 설정 및 스캔 이미지 데이터 수집(쓰레드생성)
        //
        public int AfxIntegratedSetScreenMode(int nValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            //
            // SE + BSE 모드일 때,
            // Dual Screen Mode 또는 Full Screen Mode로 변경하려고 하는 경우,
            // 이를 금지시킨다.
            //

            //
            // 이미지 데이터 스캔(캡처) 작업 중지...
            //
            AfxIntegratedScanImageDataCaptureStop();
            //

            // 새로 설정된 스캔모드와 작업중인 스캔모드가 다를 경우
            if (m_pIntegrated.nScreenMode != m_pIntegrated.nWorkingScreenMode)
            {
                // 스캔모드 재설정 
                m_pIntegrated.nScreenMode = m_pIntegrated.nWorkingScreenMode;
                // Freeze 된 상태로 만들어야, 같은 Scan Mode 상태라도 Scan을 진행
                m_pIntegrated.bFreezeModeFlag = TRUE;

                switch (m_pIntegrated.nScreenMode)
                {
                    // Normal Mode
                    case NORMAL_SCREEN:
                    {
                        // 웬캠 사용(실행) 여부
                        if (m_pIntegrated.bWebCamRunFlag == TRUE)
                        {
                            // 이전 프로그램 : 웹캠 Stop/Start
                        }
                        else
                        {
                            // 이전 프로그램
                            // 포토 스캔이미지 표시창 활성화
                        }

                        // 2020.05.11
                        // 배율 설정하기
                        AfxIntegratedSetMagnification(m_nMagStepValue[m_pIntegrated.nMagStepIndex], m_pIntegrated.nOLWD);

                        //------------------------------------
                        // 화면에 Box가 그려진 Dual Screen Mode에서
                        // Normal Mode로 되돌아 올 경우
                        // 화면에 그려진 Layer가 있으면 모두 지운다.
                        //------------------------------------
                        // 이전 프로그램
                        // Dual Screen Mode에서 Normal Mode로 왔으므로
                        //------------------------------------

                        // Set FPGA
                        m_pIntegrated.nSeCommandValue &= DUAL_SCREEN_LOW;
                        AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_OUTPORT, m_pIntegrated.nSeCommandValue); // SE-Image Command 전송
                        m_pIntegrated.nSbrCommandState &= DUAL_SCREEN_LOW;
                        AfxFT232HPortSendCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, cWR_OUTPORT, m_pIntegrated.nSbrCommandState); // Sbr Data 전송

                        // Fast Scan Run...
                        //AfxIntegratedSetFastScanRun();

                        break;
                    }

                    //-----------------------------------------
                    // Dual Screen Mode
                    //-----------------------------------------
                    case DUAL_SCREEN:
                    {
                        break;
                    }

                    //-----------------------------------------
                    // Full Screen Mode
                    //-----------------------------------------
                    case FULL_SCREEN:
                    {
                        break;
                    }
                }

                // 이전 프로그램 : 관련자료 표시하기
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:13230 ----
        //
        // Integrated Control : Scan Image Data Capture 작업 시작
        //
        public int AfxIntegratedSetScanMode(int nScanMode)
        {
            // 스캔모드
            m_pIntegrated.nScanMode = nScanMode;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:13237 ----
        public int AfxIntegratedScanImageDataCaptureStart(int nScanMode)
        {
            // 스캔모드
            m_pIntegrated.nScanMode = nScanMode;

            // Image Capture Start
            AfxIntegratedScanImageDataCaptureStart();

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:13247 ----
        public int AfxIntegratedScanImageDataCaptureStart()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;
            if (m_pSEImageDataThreadParams == null)return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // SE Image Data 스캔 허용 Flag 설정 ( STOP )
            m_pIntegrated.bScanDataReceive = FALSE;
            // SE Image Data Read Buffer Clear
            ZeroMem(m_pSEImageDataThreadParams.szFTReadBuf);

            // 스캔모드
            int nSM = m_pIntegrated.nScanMode;

            // Avr에서 Afc 종료가 입력되지 않는 경우 때문에 
            m_pIntegrated.nAutoAvrActionState = AUTO_HALT_ST;
            // X,Y Image Pixel 갯수
            m_pIntegrated.nXBmpPixelVal   = m_pIntegrated.nXBmpPixel[nSM];
            m_pIntegrated.nYBmpPixelVal   = m_pIntegrated.nYBmpPixel[nSM];
            // X,Y Bitmap Offset
            m_pIntegrated.nXBmpOffsetVal  = m_pIntegrated.nXBmpOffset[nSM];
            m_pIntegrated.nYBmpOffsetVal  = m_pIntegrated.nYBmpOffset[nSM];
            // Bitmap 이미지 Average - Data 갯수
            m_pIntegrated.nWAverageNumVal = m_pIntegrated.nWAverageNum[nSM];

            // Weighted Average : Scan 이미지 Average 사용시 프레임당 비율설정
            // 사용예: 1번프레임의Pixel값을 1/3만사용, 2번프레임의Pixel값을 2/3만사용 하여 합산
            // WAFrameNum = 2 : 1,2     (1+2)     (1/3, 2/3)
            //              3 : 1,2,3   (1+2+3)   (1/6, 2/6, 3/6)
            //              4 : 1,2,3,4 (1+2+3+4) (1/10, 2/10,3/10,4/10)
            m_pIntegrated.nWAFrameNumA        = 0;
            m_pIntegrated.nWAFrameNumB        = 0;
            m_pIntegrated.bLineNumberFlag     = FALSE;
            m_pIntegrated.bFirstButtonPressed = TRUE;

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            // Set   : Scan Mode, Filtering
            // Start : Scan
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //
            // Image Capture Start : Se Command 설정
            //
            /*
            m_pIntegrated->nSeCommandValue &= SEL_SCAN_MASK;                                                                     // m_SeCommand : 0     (0x00000000)   ==>   0     (0x00000000) 
            m_pIntegrated->nSeCommandValue |= m_pIntegrated->nFPGABmpSize[nSM] << 6;                                             // m_SeCommand : 0     (0x00000000)   ==>   64    (0x00000040)

            m_pIntegrated->nSeCommandValue &= SEL_FILT_MASK;                                                                     // m_SeCommand : 64    (0x00000040)   ==>   64    (0x00000040)
            m_pIntegrated->nSeCommandValue |= m_wFilter[m_pIntegrated->nFilterNum[nSM]];                                         // m_SeCommand : 64    (0x00000040)   ==>   72    (0x00000048)

            m_pIntegrated->nSeCommandValue &= DUAL_SCREEN_LOW;                                                                   // m_SeCommand : 72    (0x00000048)   ==>   72    (0x00000048)
            m_pIntegrated->nSeCommandValue |= IMG_CAP_START_HIGH;                                                                // m_SeCommand : 72    (0x00000048)   ==>   73    (0x00000049)

            // Se Command 전송
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_OUTPORT, m_pIntegrated->nSeCommandValue);                  // m_SeCommand : 73    (0x00000049)
            */
            // 2019.10.10
            // 스캔모드에 따른 SeCommand를 설정하여 적용한다
            if (nSM == RED_SCAN_MODE)
            {
                m_pIntegrated.nSeCommandValue  = 0x00000011;                                                                    // SeCommand    값 설정 -> RED    : 17     ( 0x00000011 )
                m_pIntegrated.nSbrCommandState = 0x00021001;                                                                    // CommandState 값 설정 -> RED    : 135169 ( 0x00021001 )
            }
            else if (nSM == FAST_SCAN_MODE)
            {
                m_pIntegrated.nSeCommandValue  = 0x00000051;                                                                    // SeCommand    값 설정 -> FAST   : 81     ( 0x00000051 )
                m_pIntegrated.nSbrCommandState = 0x00011001;                                                                    // CommandState 값 설정 -> FAST   : 69633  ( 0x00011001 )
            }
            else if (nSM == SLOW1_SCAN_MODE)
            {
                m_pIntegrated.nSeCommandValue  = 0x00000051;                                                                    // SeCommand    값 설정 -> SLOW1  : 81     ( 0x00000051 )
                m_pIntegrated.nSbrCommandState = 0x00060001;                                                                    // CommandState 값 설정 -> SLOW1  : 393217 ( 0x00060001 )
            }
            else if (nSM == SLOW2_SCAN_MODE)
            {
                m_pIntegrated.nSeCommandValue  = 0x00000051;                                                                    // SeCommand    값 설정 -> SLOW2  : 81     ( 0x00000051 )
                m_pIntegrated.nSbrCommandState = 0x00040001;                                                                    // CommandState 값 설정 -> SLOW2  : 262145 ( 0x00040001 )
            }
            else if (nSM == PHOTO1_SCAN_MODE)
            {
                m_pIntegrated.nSeCommandValue  = 0x00000051;                                                                    // SeCommand    값 설정 -> PHOTO1 : 81     ( 0x00000051 )
                m_pIntegrated.nSbrCommandState = 0x00030001;                                                                    // CommandState 값 설정 -> PHOTO1 : 196609 ( 0x00030001 )
            }
            else if (nSM == PHOTO2_SCAN_MODE)
            {
                m_pIntegrated.nSeCommandValue  = 0x00000051;                                                                    // SeCommand    값 설정 -> PHOTO2 : 81     ( 0x00000051 )
                m_pIntegrated.nSbrCommandState = 0x00020001;                                                                    // CommandState 값 설정 -> PHOTO2 : 131073 ( 0x00020001 )
            }
            else if (nSM == PHOTO3_SCAN_MODE)
            {
                m_pIntegrated.nSeCommandValue  = 0x00000091;                                                                    // SeCommand    값 설정 -> PHOTO3 : 145    ( 0x00000091 )
                m_pIntegrated.nSbrCommandState = 0x00030001;                                                                    // CommandState 값 설정 -> PHOTO3 : 196609 ( 0x00030001 )
            }
            else if (nSM == PHOTO4_SCAN_MODE)
            {
                m_pIntegrated.nSeCommandValue  = 0x00000091;                                                                    // SeCommand    값 설정 -> PHOTO4 : 145    ( 0x00000091 )
                m_pIntegrated.nSbrCommandState = 0x00020001;                                                                    // CommandState 값 설정 -> PHOTO4 : 131073 ( 0x00020001 )
            }
            else if (nSM == PHOTO5_SCAN_MODE)
            {
                m_pIntegrated.nSeCommandValue  = 0x00000091;                                                                    // SeCommand    값 설정 -> PHOTO5 : 145    ( 0x00000091 )
                m_pIntegrated.nSbrCommandState = 0x00010001;                                                                    // CommandState 값 설정 -> PHOTO5 : 65537  ( 0x00010001 )
            }
            else if (nSM == PHOTO6_SCAN_MODE)
            {
                m_pIntegrated.nSeCommandValue  = 0x000000D1;                                                                    // SeCommand    값 설정 -> PHOTO6 : 209    ( 0x000000D1 )
                m_pIntegrated.nSbrCommandState = 0x00020001;                                                                    // CommandState 값 설정 -> PHOTO6 : 131073 ( 0x00020001 )
            }
            else
            {
                m_pIntegrated.nSeCommandValue  = 0x00000051;                                                                    // SeCommand    값 설정 -> FAST   : 81     ( 0x00000051 )
                m_pIntegrated.nSbrCommandState = 0x00011001;                                                                    // CommandState 값 설정 -> FAST   : 69633  ( 0x00011001 )
            }
            // Se Command 전송
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_OUTPORT, m_pIntegrated.nSeCommandValue);                  // m_SeCommand : 실행
            //
            //
            //

            //
            // Image Capture Start : Sbr Command 설정
            //
            // 2019.10.10
            // 스캔모드에 따른 SeCommand를 설정하여 적용한다
            if (nSM == RED_SCAN_MODE)
            {
                m_pIntegrated.nSeCommandValue  = 0x00000011;                                                                    // SeCommand    값 설정 -> RED    : 17     ( 0x00000011 )
                m_pIntegrated.nSbrCommandState = 0x00021001;                                                                    // CommandState 값 설정 -> RED    : 135169 ( 0x00021001 )
            }
            else if (nSM == FAST_SCAN_MODE)
            {
                m_pIntegrated.nSeCommandValue  = 0x00000051;                                                                    // SeCommand    값 설정 -> FAST   : 81     ( 0x00000051 )
                m_pIntegrated.nSbrCommandState = 0x00011001;                                                                    // CommandState 값 설정 -> FAST   : 69633  ( 0x00011001 )
            }
            else if (nSM == SLOW1_SCAN_MODE)
            {
                m_pIntegrated.nSeCommandValue  = 0x00000051;                                                                    // SeCommand    값 설정 -> SLOW1  : 81     ( 0x00000051 )
                m_pIntegrated.nSbrCommandState = 0x00060001;                                                                    // CommandState 값 설정 -> SLOW1  : 393217 ( 0x00060001 )
            }
            else if (nSM == SLOW2_SCAN_MODE)
            {
                m_pIntegrated.nSeCommandValue  = 0x00000051;                                                                    // SeCommand    값 설정 -> SLOW2  : 81     ( 0x00000051 )
                m_pIntegrated.nSbrCommandState = 0x00040001;                                                                    // CommandState 값 설정 -> SLOW2  : 262145 ( 0x00040001 )
            }
            else if (nSM == PHOTO1_SCAN_MODE)
            {
                m_pIntegrated.nSeCommandValue  = 0x00000051;                                                                    // SeCommand    값 설정 -> PHOTO1 : 81     ( 0x00000051 )
                m_pIntegrated.nSbrCommandState = 0x00030001;                                                                    // CommandState 값 설정 -> PHOTO1 : 196609 ( 0x00030001 )
            }
            else if (nSM == PHOTO2_SCAN_MODE)
            {
                m_pIntegrated.nSeCommandValue  = 0x00000051;                                                                    // SeCommand    값 설정 -> PHOTO2 : 81     ( 0x00000051 )
                m_pIntegrated.nSbrCommandState = 0x00020001;                                                                    // CommandState 값 설정 -> PHOTO2 : 131073 ( 0x00020001 )
            }
            else if (nSM == PHOTO3_SCAN_MODE)
            {
                m_pIntegrated.nSeCommandValue  = 0x00000091;                                                                    // SeCommand    값 설정 -> PHOTO3 : 145    ( 0x00000091 )
                m_pIntegrated.nSbrCommandState = 0x00030001;                                                                    // CommandState 값 설정 -> PHOTO3 : 196609 ( 0x00030001 )
            }
            else if (nSM == PHOTO4_SCAN_MODE)
            {
                m_pIntegrated.nSeCommandValue  = 0x00000091;                                                                    // SeCommand    값 설정 -> PHOTO4 : 145    ( 0x00000091 )
                m_pIntegrated.nSbrCommandState = 0x00020001;                                                                    // CommandState 값 설정 -> PHOTO4 : 131073 ( 0x00020001 )
            }
            else if (nSM == PHOTO5_SCAN_MODE)
            {
                m_pIntegrated.nSeCommandValue  = 0x00000091;                                                                    // SeCommand    값 설정 -> PHOTO5 : 145    ( 0x00000091 )
                m_pIntegrated.nSbrCommandState = 0x00010001;                                                                    // CommandState 값 설정 -> PHOTO5 : 65537  ( 0x00010001 )
            }
            else if (nSM == PHOTO6_SCAN_MODE)
            {
                m_pIntegrated.nSeCommandValue  = 0x000000D1;                                                                    // SeCommand    값 설정 -> PHOTO6 : 209    ( 0x000000D1 )
                m_pIntegrated.nSbrCommandState = 0x00020001;                                                                    // CommandState 값 설정 -> PHOTO6 : 131073 ( 0x00020001 )
            }
            else
            {
                m_pIntegrated.nSeCommandValue  = 0x00000051;                                                                    // SeCommand    값 설정 -> FAST   : 81     ( 0x00000051 )
                m_pIntegrated.nSbrCommandState = 0x00011001;                                                                    // CommandState 값 설정 -> FAST   : 69633  ( 0x00011001 )
            }
            // Sbr Command 전송
            AfxFT232HPortSendCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, cWR_OUTPORT, m_pIntegrated.nSbrCommandState);               // m_CommandState : 실행

            //
            //
            //

            // SE Image Data Read Buffer Clear
            ZeroMem(m_pSEImageDataThreadParams.szFTReadBuf);
            // SE Image Data 스캔 허용 Flag 설정 ( START )
            m_pIntegrated.bScanDataReceive = TRUE;

            //
            //
            //

            //------------
            // BSE Board가 연결된 경우
            //------------
            // 2021.07.23 : 별도의 BSE 이미지 보드가 없기 때문에 아래 기능은 사용하지 않는다.
            /*
            if (m_pIntegrated->bBseActiveFlag == TRUE)
            {
                // BSE BITMAP DISPLAY 포인터
                // 1) Bse OUTPORT 설정
                m_pIntegrated->nBseCommandValue |= IMG_CAP_START_HIGH;
                m_pIntegrated->nBseCommandValue &= SEL_SCAN_MASK;
                m_pIntegrated->nBseCommandValue |= m_pIntegrated->nFPGABmpSize[nSM] << 6;

                // 2) Filtering 값 설정 ( Avr에서 제어함 )
                m_pIntegrated->nBseCommandValue &= SEL_FILT_MASK;
                m_pIntegrated->nBseCommandValue |= m_wFilter[m_pIntegrated->nBseFilterIndex];

                // Bse Command 전송
                AfxFT232HPortSendCommand(IDX_FT232H_BSE_IMAGE_DEVICE, cWR_OUTPORT, m_pIntegrated->nBseCommandValue);
            }
            */
            //------------

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:13673 ----
        //
        // Integrated Control : Scan Image Data Capture 작업 중지
        //
        public int AfxIntegratedScanImageDataCaptureStop()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // SE Image Data 스캔 허용 Flag 설정 ( STOP )
            m_pIntegrated.bScanDataReceive = FALSE;

            //--------------------------------
            // SE Image Sampling Capture Stop
            //--------------------------------
            m_pIntegrated.nSeCommandValue &= IMG_CAP_START_LOW;
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_OUTPORT, m_pIntegrated.nSeCommandValue); // SE-Image Command 전송

            m_pIntegrated.nSbrCommandState &= IMG_CAP_START_LOW;
            AfxFT232HPortSendCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, cWR_OUTPORT, m_pIntegrated.nSbrCommandState); // Sbr Command 전송

            //--------------------------------
            // BSE Image Sampling Capture Stop
            //--------------------------------
            // BSE Thread가 종료될 때 까지
            if (m_pIntegrated.nBseDetectorValue == SE_BSE_DETECTOR)
            {
                m_pIntegrated.nBseCommandValue &= IMG_CAP_START_LOW;
                AfxFT232HPortSendCommand(IDX_FT232H_BSE_IMAGE_DEVICE, bWR_OUTPORT, m_pIntegrated.nBseCommandValue); // BSE-Image Command 전송
            }
            //--------------------------------

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:13706 ----
        //
        // Integrated Control : Initialize Parameters (파라메터 초기화)
        //
        public int AfxIntegratedInitializeParameters(int nSemType)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            //-----------------------------------------------
            // 4 x 4.8MHz = 19,200,000 => 38,400,000 => 57,600,000
            //-----------------------------------------------
            // 4 x  640 x  250 x 30  =>  320x240
            // 2 x 1280 x  500 x 15  =>  640x480
            // 1 x 2560 x 1000 x 7.5 => 1280x960
            //-----------------------------------------------
            //-----------------------------------------------
            // 4 x 4.5MHz = 18,000,000 => 36,000,000 => 54,000,000
            //-----------------------------------------------
            // 4 x  600 x  250 x 30  =>  320x240
            // 2 x 1200 x  500 x 15  =>  640x480
            // 1 x 2400 x 1000 x 7.5 => 1280x960
            //-----------------------------------------------
            //-----------------------------------------------
            // 4 x 4.125MHz = 16,500,000 => 33,000,000 => 49,500,000
            //-----------------------------------------------
            // 4 x  550 x  250 x 30  =>  320x240
            // 2 x 1000 x  500 x 15  =>  640x480
            // 1 x 2000 x 1000 x 7.5 => 1280x960
            //-----------------------------------------------
            //-----------------------------------------------
            // 500 x 250 x 30 = 3.75MHz
            // 4 x 3.75MHz = 15MHz => 30MHz
            //-----------------------------------------------
            //  8 x  500(x) x  250(y) x 30  = 30MHz
            //  4 x 1000(x) x  500(y) x 15  = 30MHz
            //  2 x 2000(x) x 1000(y) x 7.5 = 30MHz
            //-----------------------------------------------
            //-----------------------------------------------
            // 500 x 250 x 30frame = 3750000 = 3.75MHz
            //-----------------------------------------------

            //
            // 초기화 화일이 없을 때 초기화 데이터
            //
            //------------
            // SyncFreqNum :  0       1      2     3     4     5     6      7        8     9       10
            //               /16     /8     /4    /2    x1    x2     x4     x8     x16    x32     x64
            // Sync Freq   : 3.75Hz, 7.5Hz, 15Hz, 30Hz, 60Hz, 120Hz, 240Hz, 480Hz, 960Hz, 1920Hz, 3840Hz
            //------------
            // Frame Sync  :   0      0      0     0     x      x      x      x      x      x      x
            // Line  Sync  :   x      0      0     0     0      0      0      0      0      0      0
            //------------

            //-----------------------------------------------
            // FT2_BridgeScan VHDL CommandPort의 m_CommandState 값
            //-----------------------------------------------
            //ReScanStart  => image_cap_start,  -- D0 : img_scan_reset 발생
            //MultiFrame   => auto_focus_start, -- D1 : Auto Focus 시작, 반복실행
            //NormalScan   => auto_focus_mode,  -- D2 :
            //SelFilter    => open,             -- D5, D4, D3
            //SelScan      => open,             -- D9, D8, D7, D6
            //FrameSync    => frame_sync_flag,  -- D10
            //DualScreenOn => dual_screen_on,   -- D11
            //ScanOnFlag   => pll_60hz_flag,    -- D12 : scan_active,
            //XScanInverse => xscan_inverse,    -- D13
            //YScanInverse => yscan_inverse,    -- D14
            //SyncScanOn   => sync_scan_on,     -- D15
            //SelSyncFreq  => sync_freq_mode    -- D19, D18, D17, D16
            //-----------------------------------------------
            //-----------------------------------------------
            // Frame Sync,  30Hz => 0011 0000 0100 0000 0000
            //                      ^  ^       ^
            //                      D..D       D
            //                      1..1       1
            //                      9..6       0
            //-----------------------------------------------
            // Line  Sync, 480Hz => 0111 0000 0000 0000 0000
            //                      ^  ^       ^
            //                      D..D       D
            //                      1..1       1
            //                      9..6       0
            //-----------------------------------------------

            m_pIntegrated.nPLLClk = PLL_SCAN_CLOCK;

            //--
            // Half Red Scan Mode : 320x240 ( 500x250 = 125,000 Hz)
            // Frame Sync Freq    : 30Hz, 15Hz, 7.5Hz
            //--
            m_pIntegrated.nScanClk[0] = 3750000; // (125kHz x 30)   // HalfRed 1 - < 30  Frame/sec >
            m_pIntegrated.nSyncVal[0] = 0x31000; // ( 30Hz, Frame Sync )
            m_pIntegrated.nScanClk[1] = 1875000; // (125kHz x 15)   // HalfRed 2 - < 15  Frame/sec >
            m_pIntegrated.nSyncVal[1] = 0x21000; // ( 15Hz, Frame Sync )
            m_pIntegrated.nScanClk[2] = 937500;  // (125kHz x 7.5)  // HalfRed 3 - < 7.5 Frame/sec >
            m_pIntegrated.nSyncVal[2] = 0x11000; // ( 7.5Hz, Frame Sync )
            //--
            // Full Red Scan Mode : 640x480 ( 1000x500 = 500,000 Hz)
            // Frame Sync Freq    : 15Hz, 7.5Hz, 3.75Hz
            //--
            m_pIntegrated.nScanClk[3] = 7500000; // (500kHz x 15)   // FullRed 1 - < 15   Frame/sec >
            m_pIntegrated.nSyncVal[3] = 0x21000; // ( 15Hz, Frame Sync )
            m_pIntegrated.nScanClk[4] = 3750000; // (500kHz x 7.5)  // FullRed 2 - < 7.5  Frame/sec >
            m_pIntegrated.nSyncVal[4] = 0x11000; // ( 7.5Hz, Frame Sync )
            m_pIntegrated.nScanClk[5] = 1875000; // (500kHz x 3.75) // FullRed 3 - < 3.75 Frame/sec >
            m_pIntegrated.nSyncVal[5] = 0x01000; // ( 3.75Hz, Frame Sync )
            //--
            // Half Fast Scan Mode : 640x480 ( 1000x500 = 500,000 Hz)
            // Frame Sync Freq     : 15Hz, 7.5Hz, 3.75Hz
            //--
            m_pIntegrated.nScanClk[6] = 7500000; // (500kHz x 15)   // HalfFast 1 - < 15   Frame/sec >
            m_pIntegrated.nSyncVal[6] = 0x21000; // ( 15Hz, Frame Sync )
            m_pIntegrated.nScanClk[7] = 3750000; // (500kHz x 7.5)  // HalfFast 2 - < 7.5  Frame/sec >
            m_pIntegrated.nSyncVal[7] = 0x11000; // ( 7.5Hz, Frame Sync )
            m_pIntegrated.nScanClk[8] = 1875000; // (500kHz x 3.75) // HalfFast 3 - < 3.75 Frame/sec >
            m_pIntegrated.nSyncVal[8] = 0x01000; // ( 3.75Hz, Frame Sync )
            //---------------------------------------
            // Half Slow Scan Mode : 640x480 ( 1000 x 500 )
            // Line Sync Freq      : 240Hz, 120Hz, 60Hz
            //---------------------------------------
            // 1 frame = (1/240) x  500(line) = 2.08 sec  <= 240Hz LineSyncFreq
            // 1 frame = (1/120) x  500(line) = 4.16 sec  <= 120Hz LineSyncFreq
            // 1 frame = (1/60)  x  500(line) = 8.33 sec  <=  60Hz LineSyncFreq
            m_pIntegrated.nScanClk[9]  = 240000;  // (240Hz x 1000) // HalfSlow 1 - < 2.08 sec/frame = 0.48 Frame/sec >
            m_pIntegrated.nSyncVal[9]  = 0x60000; // (240Hz, Line Sync)
            m_pIntegrated.nScanClk[10] = 120000;  // (120Hz x 1000) // HalfSlow 2 - < 4.16 sec/frame = 0.24 Frame/sec >
            m_pIntegrated.nSyncVal[10] = 0x50000; // (120Hz, Line Sync)
            m_pIntegrated.nScanClk[11] = 60000;   // ( 60Hz x 1000) // HalfSlow 3 - < 8.33 sec/frame = 0.12 Frame/sec >
            m_pIntegrated.nSyncVal[11] = 0x40000; // ( 60Hz, Line Sync)
            //--
            // Full Fast Scan Mode : 1280 x 960 ( 2000 x 1000 )
            // Line Sync Freq      : 3840Hz, 1920Hz, 960Hz
            //--
            // 1 frame = (1/3840) x 1000(line) =  3.84 fr/sec
            // 1 frame = (1/1920) x 1000(line) =  1.92 fr/sec
            // 1 frame = (1/960)  x 1000(line) =   0.96 fr/sec = 1.04 sec/fr
            m_pIntegrated.nScanClk[12] = 7500000; // (3840Hz x 1954pix) // Full Fast1   - <  3.8 Frame/sec >
            m_pIntegrated.nSyncVal[12] = 0xA0000; // (3840Hz, Line Sync)
            m_pIntegrated.nScanClk[13] = 3750000; // (1920Hz x 1954pix) // Full Fast2   - <  1.9 Frame/sec >
            m_pIntegrated.nSyncVal[13] = 0x90000; // (1920Hz, Line Sync)
            m_pIntegrated.nScanClk[14] = 1875000; // ( 960Hz x 1954pix) // Full Fast3   - <  0.9 Frame/sec >
            m_pIntegrated.nSyncVal[14] = 0x80000; // ( 960Hz, Line Sync)
            //--
            // Full Slow Scan Mode : 1280 x 960 ( 2000 x 1000 )
            // Line Sync Freq      : 480Hz, 240Hz, 120Hz
            //--
            // 1 frame = (1/480) x  1000(line) =  2.08 sec/fr
            // 1 frame = (1/240) x  1000(line) =  4.16 sec/fr
            // 1 frame = (1/120) x  1000(line) =  8.33 sec/fr
            // 1 frame = (1/60)  x  1000(line) =  16.6 sec/fr
            m_pIntegrated.nScanClk[15] = 1000000;  // (480Hz x 2084pix) // Full Slow1 - <  2.08 sec/frame >
            m_pIntegrated.nSyncVal[15] = 0x70000;  // (480Hz, Line Sync)
            m_pIntegrated.nScanClk[16] = 500000;   // (240Hz x 2084pix) // Full Slow2 - <  4.16 sec/frame >
            m_pIntegrated.nSyncVal[16] = 0x60000;  // (240Hz, Line Sync)
            m_pIntegrated.nScanClk[17] = 240000;   // (120Hz x 2000pix) // Full Slow3 - <  8.33 sec/frame >
            m_pIntegrated.nSyncVal[17] = 0x50000;  // (120Hz, Line Sync)
            //--
            //--
            // Photo Scan Mode
            //--
            //--
            //  640 x 480
            // 1200 x 500 x (1/15 sec) = 40000 = 40kHz
            // 1 line = 60Hz x 1920 = 57.6kHz
            //--------------------------
            // Photo : 640 x 480
            // Line Sync Freq    : 30Hz, 15Hz
            //--------------------------
            // 1 frame = (1/30) x 480(line) = 16 sec
            // 1 frame = (1/15) x 480(line) = 32 sec
            m_pIntegrated.nScanClk[18] = 30000;   // ( 30Hz x 1000pix ) // Photo1 < 16 sec/frame >
            m_pIntegrated.nSyncVal[18] = 0x30000; // ( 30Hz, Line Sync)
            m_pIntegrated.nScanClk[19] = 15000;   // ( 15Hz x 1000pix ) // Photo2 < 32 sec/frame >
            m_pIntegrated.nSyncVal[19] = 0x20000; // ( 15Hz, Line Sync)
            //--------------------------------
            // Photo : 1280 x 960
            // Line Sync Freq    : 30Hz, 15Hz, 7.5Hz
            //--------------------------------
            // 1 frame = (1/30)  x 960(line) =  32 sec
            // 1 frame = (1/15)  x 960(line) =  64 sec
            // 1 frame = (1/7.5) x 960(line) = 128 sec
            m_pIntegrated.nScanClk[20] = 60000;    // ( 30Hz x 2000pix ) // Photo3 <  32 sec/frame >
            m_pIntegrated.nSyncVal[20] = 0x30000;  // ( 30Hz, Line Sync)
            m_pIntegrated.nScanClk[21] = 30000;    // ( 15Hz x 2000pix ) // Photo4 <  64 sec/frame >
            m_pIntegrated.nSyncVal[21] = 0x20000;  // ( 15Hz, Line Sync)
            m_pIntegrated.nScanClk[22] = 15000;    // ( 7.5Hz x 2000pix ) // Photo5 < 128 sec/frame >
            m_pIntegrated.nSyncVal[22] = 0x10000;  // ( 7.5Hz, Line Sync)
            //-------------------------------
            // Photo : 2560 x 1920  => 5120 x 3840
            //-------------------------------
            // 1 frame = (1/30) x  1920(line) =  64 sec // not used
            // 1 frame = (1/15) x  1920(line) = 128 sec
            m_pIntegrated.nScanClk[23] = 60000;    // ( 15Hz x 4000pix ) // Photo6 < 128 sec/frame >
            m_pIntegrated.nSyncVal[23] = 0x20000;  // ( 15Hz, Line Sync)
            //------------
            // WFM Mode
            //------------
            m_pIntegrated.nScanClk[24] = 120000;      // WFM
            //------------
            // Auto Focus Mode
            //------------
            // 36,000,000/480,000 = 75
            // OverSampling   = 150;
            m_pIntegrated.nScanClk[25] = 450000;      // Auto Focus - Slow1 Speed
            //--
            // Half Red Scan Mode : 320x240
            //  30 x 250 x 640
            //--
            // 0 : Half Red  1  -  < 30 Frame/sec >
            m_pIntegrated.nFPGABmpSize[0]   = 0;        // 320x240
            m_pIntegrated.nScanCodeA[0]     = 0xA0FF;
            m_pIntegrated.nScanCodeB[0]     = 0xB0FF;
            m_pIntegrated.nXScanUpNum[0]    = 1024;     // 550
            m_pIntegrated.nXScanDownNum[0]  = 256;      //  50
            m_pIntegrated.nYScanUpNum[0]    = 244;      // 244
            m_pIntegrated.nYScanDownNum[0]  = 4;        //   6
            m_pIntegrated.nXBmpPixel[0]     = 320;
            m_pIntegrated.nYBmpPixel[0]     = 240;
            m_pIntegrated.nXBmpOffset[0]    = 160;
            m_pIntegrated.nYBmpOffset[0]    = 120;
            m_pIntegrated.nWAverageNum[0]   = 1;
            m_pIntegrated.nFilterNum[0]     = 1;
            m_pIntegrated.nXScanAmpVal[0]   = 1057;     // 417;
            m_pIntegrated.nXScanOffset[0]   = 160;      // 30;
            m_pIntegrated.nYScanAmpVal[0]   = 539;
            m_pIntegrated.nYScanOffset[0]   = 0;
            m_pIntegrated.nSkipPixNum[0]    = 115;      // 0 - 499
            m_pIntegrated.nFrameSec[0]      = 30000;    // 30 f/s
            m_pIntegrated.nOverSampleNum[0] = 10;
            //--
            // 1 : Half Red  2  -  <  15 Frame/sec >     <<<<< RED SCAN MODE >>>>>
            //--
            m_pIntegrated.nFPGABmpSize[1]   = 0;        // 320x240
            m_pIntegrated.nScanCodeA[1]     = 0xA0FF;   // Default
            m_pIntegrated.nScanCodeB[1]     = 0xB0FF;   // Default
            m_pIntegrated.nXScanUpNum[1]    = 1024;     // X Scan Up   Value
            m_pIntegrated.nXScanDownNum[1]  = 128;      // X Scan Down Value
            m_pIntegrated.nYScanUpNum[1]    = 243;      // Y Scan Up   Value
            m_pIntegrated.nYScanDownNum[1]  = 28;       // Y Scan Down Value
            m_pIntegrated.nXBmpPixel[1]     = 320;      // X PIXEL WIDTH
            m_pIntegrated.nYBmpPixel[1]     = 240;      // Y PIXEL HEIGHT 
            m_pIntegrated.nXBmpOffset[1]    = 160;      // X PIXEL OFFSET
            m_pIntegrated.nYBmpOffset[1]    = 120;      // Y PIXEL OFFSET
            m_pIntegrated.nWAverageNum[1]   = 6;        // Frame Average : 6
            m_pIntegrated.nFilterNum[1]     = 1;        // Filter ( 0->0x0008, 1->0x0010, 2->0x0020, 3->0x8000 )
            m_pIntegrated.nXScanAmpVal[1]   = 1586;     // 1586,  3.87[v] : X축 배율보정 (크기)
            m_pIntegrated.nXScanOffset[1]   = 1565;     // 1565, -1.18[v] : X축 위치보정 (센터)
            m_pIntegrated.nYScanAmpVal[1]   = 747;      //  747,  1.82[v] : Y축 배율보정 (크기)
            m_pIntegrated.nYScanOffset[1]   = 2030;     // 2030, -0.04[v] : Y축 위치보정 (센터)
            m_pIntegrated.nSkipPixNum[1]    = 258;      // 0 - 499 -> SCAN SKIP PIXEL COUNT ( 일그러진 SCAN 이미지 SKIP )
            m_pIntegrated.nFrameSec[1]      = 15000;    // 15[f/s]
            m_pIntegrated.nOverSampleNum[1] = 17;       // Over Sampling Value
            //--
            // 2 : Half Red  3  -  <  7.5 Frame/sec >
            //--
            m_pIntegrated.nFPGABmpSize[2]   = 0;        // 320x240
            m_pIntegrated.nScanCodeA[2]     = 0xA0FF;   // 0xFF01;
            m_pIntegrated.nScanCodeB[2]     = 0xB0FF;   // 0xFFB1;
            m_pIntegrated.nXScanUpNum[2]    = 1024;     // 600
            m_pIntegrated.nXScanDownNum[2]  = 64;       //
            m_pIntegrated.nYScanUpNum[2]    = 244;      // 250
            m_pIntegrated.nYScanDownNum[2]  = 4;
            m_pIntegrated.nXBmpPixel[2]     = 320;
            m_pIntegrated.nYBmpPixel[2]     = 240;
            m_pIntegrated.nXBmpOffset[2]    = 160;
            m_pIntegrated.nYBmpOffset[2]    = 120;
            m_pIntegrated.nWAverageNum[2]   = 1;
            m_pIntegrated.nFilterNum[2]     = 1;
            m_pIntegrated.nXScanAmpVal[2]   = 741;
            m_pIntegrated.nXScanOffset[2]   = 178;      // 34;
            m_pIntegrated.nYScanAmpVal[2]   = 541;
            m_pIntegrated.nYScanOffset[2]   = 0;
            m_pIntegrated.nSkipPixNum[2]    = 13;       // 0 - 499
            m_pIntegrated.nFrameSec[2]      = 7500;     // 7.5frame
            m_pIntegrated.nOverSampleNum[2] = 53;       // XScan Freq. => 1.8kHz
            //--
            // Full Red Scan Mode : 640x480
            //--
            // 3 : Full Red  1  -  < 15 Frame/sec >
            //--
            m_pIntegrated.nFPGABmpSize[3]   = 1;        // 640x480
            m_pIntegrated.nScanCodeA[3]     = 0xA1FF;   //
            m_pIntegrated.nScanCodeB[3]     = 0xB1FF;   //
            m_pIntegrated.nXScanUpNum[3]    = 2048;     // 1200
            m_pIntegrated.nXScanDownNum[3]  = 256;      //
            m_pIntegrated.nYScanUpNum[3]    = 484;      //  500
            m_pIntegrated.nYScanDownNum[3]  = 4;        //
            m_pIntegrated.nXBmpPixel[3]     = 640;
            m_pIntegrated.nYBmpPixel[3]     = 480;
            m_pIntegrated.nXBmpOffset[3]    = 320;
            m_pIntegrated.nYBmpOffset[3]    = 240;
            m_pIntegrated.nWAverageNum[3]   = 6;
            m_pIntegrated.nFilterNum[3]     = 1;
            m_pIntegrated.nXScanAmpVal[3]   = 1077;
            m_pIntegrated.nXScanOffset[3]   = 30;
            m_pIntegrated.nYScanAmpVal[3]   = 547;
            m_pIntegrated.nYScanOffset[3]   = 0;
            m_pIntegrated.nSkipPixNum[3]    = 269;      // 0 - 499
            m_pIntegrated.nFrameSec[3]      = 15000;    // 15f/s
            m_pIntegrated.nOverSampleNum[3] = 5;        // XScan Freq. => 3.6kHz
            //--
            // 4 : Full Red  2  -   <  7.5 Frame/sec >
            //--
            m_pIntegrated.nFPGABmpSize[4]   = 1;        // 640x480
            m_pIntegrated.nScanCodeA[4]     = 0xA1FF;   //
            m_pIntegrated.nScanCodeB[4]     = 0xB1FF;   //
            m_pIntegrated.nXScanUpNum[4]    = 2048;     // 1200
            m_pIntegrated.nXScanDownNum[4]  = 128;      //
            m_pIntegrated.nYScanUpNum[4]    = 484;      // 500
            m_pIntegrated.nYScanDownNum[4]  = 4;        //
            m_pIntegrated.nXBmpPixel[4]     = 640;
            m_pIntegrated.nYBmpPixel[4]     = 480;
            m_pIntegrated.nXBmpOffset[4]    = 320;
            m_pIntegrated.nYBmpOffset[4]    = 240;
            m_pIntegrated.nWAverageNum[4]   = 1;
            m_pIntegrated.nFilterNum[4]     = 1;
            m_pIntegrated.nXScanAmpVal[4]   = 864;
            m_pIntegrated.nXScanOffset[4]   = 136;
            m_pIntegrated.nYScanAmpVal[4]   = 544;
            m_pIntegrated.nYScanOffset[4]   = 0;
            m_pIntegrated.nSkipPixNum[4]    = 86;       // 0 - 499
            m_pIntegrated.nFrameSec[4]      = 7500;     // 7.5f/s
            m_pIntegrated.nOverSampleNum[4] = 12;
            //--
            // 5 : Full Red  3  -   <  3.8 Frame/sec >
            // 5 : Full Red  3  -   <  3.25 Frame/sec >
            //--
            m_pIntegrated.nFPGABmpSize[5]   = 1;        // 640x480
            m_pIntegrated.nScanCodeA[5]     = 0xA1FF;   //
            m_pIntegrated.nScanCodeB[5]     = 0xB1FF;   //
            m_pIntegrated.nXScanUpNum[5]    = 2048;     // 1200
            m_pIntegrated.nXScanDownNum[5]  = 64;       //
            m_pIntegrated.nYScanUpNum[5]    = 484;      //  500
            m_pIntegrated.nYScanDownNum[5]  = 4;        //
            m_pIntegrated.nXBmpPixel[5]     = 640;
            m_pIntegrated.nYBmpPixel[5]     = 480;
            m_pIntegrated.nXBmpOffset[5]    = 320;
            m_pIntegrated.nYBmpOffset[5]    = 240;
            m_pIntegrated.nWAverageNum[5]   = 1;
            m_pIntegrated.nFilterNum[5]     = 1;
            m_pIntegrated.nXScanAmpVal[5]   = 774;
            m_pIntegrated.nXScanOffset[5]   = 115;
            m_pIntegrated.nYScanAmpVal[5]   = 544;
            m_pIntegrated.nYScanOffset[5]   = 0;
            m_pIntegrated.nSkipPixNum[5]    = 38;       // 0 - 499
            m_pIntegrated.nFrameSec[5]      = 3750;     // 3.75f/s
            m_pIntegrated.nOverSampleNum[5] = 26;       // XScan Freq. => 1.8kHz
            //--
            // Half Fast Scan Mode
            //--
            // 6 : Half Fast High   -  < 15 Frame/sec >
            //--
            m_pIntegrated.nFPGABmpSize[6]   = 1;        // 640x480
            m_pIntegrated.nScanCodeA[6]     = 0xA1FF;
            m_pIntegrated.nScanCodeB[6]     = 0xB1FF;
            m_pIntegrated.nXScanUpNum[6]    = 2048;     // 1200
            m_pIntegrated.nXScanDownNum[6]  = 256;      //
            m_pIntegrated.nYScanUpNum[6]    = 484;      //  500
            m_pIntegrated.nYScanDownNum[6]  = 4;        //
            m_pIntegrated.nXBmpPixel[6]     = 640;
            m_pIntegrated.nYBmpPixel[6]     = 480;
            m_pIntegrated.nXBmpOffset[6]    = 0;
            m_pIntegrated.nYBmpOffset[6]    = 0;
            m_pIntegrated.nWAverageNum[6]   = 1;
            m_pIntegrated.nFilterNum[6]     = 1;
            m_pIntegrated.nXScanAmpVal[6]   = 2047;     //1823;
            m_pIntegrated.nXScanOffset[6]   = 0;
            m_pIntegrated.nYScanAmpVal[6]   = 1045;
            m_pIntegrated.nYScanOffset[6]   = 0;
            m_pIntegrated.nSkipPixNum[6]    = 273;      // 0 - 719                             1
            m_pIntegrated.nFrameSec[6]      = 15000;    // 15f/s
            m_pIntegrated.nOverSampleNum[6] = 5;        // XScan Freq. => 3.6kHz
            //--
            // 7 : Half Fast Middle - <  7.5 Frame/sec > <<<<< FAST SCAN MODE >>>>>
            //--
            m_pIntegrated.nFPGABmpSize[7]   = 1;        // 640x480
            m_pIntegrated.nScanCodeA[7]     = 0xA1FF;   // Default
            m_pIntegrated.nScanCodeB[7]     = 0xB1FF;   // Default
            m_pIntegrated.nXScanUpNum[7]    = 2048;     // X Scan Up   Value
            m_pIntegrated.nXScanDownNum[7]  = 128;      // X Scan Down Value
            m_pIntegrated.nYScanUpNum[7]    = 481;      // Y Scan Up   Value
            m_pIntegrated.nYScanDownNum[7]  = 22;       // Y Scan Down Value
            m_pIntegrated.nXBmpPixel[7]     = 640;      // X PIXEL WIDTH
            m_pIntegrated.nYBmpPixel[7]     = 480;      // Y PIXEL HEIGHT 
            m_pIntegrated.nXBmpOffset[7]    = 0;        // X PIXEL OFFSET
            m_pIntegrated.nYBmpOffset[7]    = 0;        // Y PIXEL OFFSET
            m_pIntegrated.nWAverageNum[7]   = 6;        // Frame Average : 6
            m_pIntegrated.nFilterNum[7]     = 1;        // Filter ( 0->0x0008, 1->0x0010, 2->0x0020, 3->0x8000 )
            m_pIntegrated.nXScanAmpVal[7]   = 3381;     // 3381,  8.26[v] : X축 배율보정 (크기)
            m_pIntegrated.nXScanOffset[7]   = 1413;     // 1413, -1.55[v] : X축 위치보정 (센터)
            m_pIntegrated.nYScanAmpVal[7]   = 1447;     // 1447,  3.53[v] : Y축 배율보정 (크기) 
            m_pIntegrated.nYScanOffset[7]   = 2070;     // 2070,  0.06[v] : Y축 위치보정 (센터)
            m_pIntegrated.nSkipPixNum[7]    = 500;      // 0 - 999 -> SCAN SKIP PIXEL COUNT ( 일그러진 SCAN 이미지 SKIP )
            m_pIntegrated.nFrameSec[7]      = 7474;     // 7.5[f/s]
            m_pIntegrated.nOverSampleNum[7] = 8;        // Over Sampling Value
            //--
            // 8 : Half Fast Slow  -   <  3.8 Frame/sec >
            // 8 : Half Fast Slow  -   <  3.25 Frame/sec >
            //--
            m_pIntegrated.nFPGABmpSize[8]   = 1;        // 640x480
            m_pIntegrated.nScanCodeA[8]     = 0xA1FF;   //
            m_pIntegrated.nScanCodeB[8]     = 0xB1FF;   //
            m_pIntegrated.nXScanUpNum[8]    = 2048;     // 1200
            m_pIntegrated.nXScanDownNum[8]  = 64;       //
            m_pIntegrated.nYScanUpNum[8]    = 484;      //  500
            m_pIntegrated.nYScanDownNum[8]  = 4;        //
            m_pIntegrated.nXBmpPixel[8]     = 640;
            m_pIntegrated.nYBmpPixel[8]     = 480;
            m_pIntegrated.nXBmpOffset[8]    = 0;
            m_pIntegrated.nYBmpOffset[8]    = 0;
            m_pIntegrated.nWAverageNum[8]   = 1;
            m_pIntegrated.nFilterNum[8]     = 1;
            m_pIntegrated.nXScanAmpVal[8]   = 1483;
            m_pIntegrated.nXScanOffset[8]   = 190;
            m_pIntegrated.nYScanAmpVal[8]   = 1062;
            m_pIntegrated.nYScanOffset[8]   = 10;
            m_pIntegrated.nSkipPixNum[8]    = 37;       // 0 - 499
            m_pIntegrated.nFrameSec[8]      = 3750;     // 3.75f/s
            m_pIntegrated.nOverSampleNum[8] = 26;       // XScan Freq. => 1.8kHz
            //--
            // Half Slow Scan Mode
            //--
            // 9 : Half Slow 1   -  < 0.6 Frame/sec >    <<<<< SLOW1 SCAN MODE >>>>>
            //--
            m_pIntegrated.nFPGABmpSize[9]   = 1;        // 640x480
            m_pIntegrated.nScanCodeA[9]     = 0xA1FF;   // Default
            m_pIntegrated.nScanCodeB[9]     = 0xB1FF;   // Default 
            m_pIntegrated.nXScanUpNum[9]    = 2048;     // X Scan Up   Value
            m_pIntegrated.nXScanDownNum[9]  = 32;       // X Scan Down Value
            m_pIntegrated.nYScanUpNum[9]    = 484;      // Y Scan Up   Value
            m_pIntegrated.nYScanDownNum[9]  = 41;       // Y Scan Down Value
            m_pIntegrated.nXBmpPixel[9]     = 640;      // X PIXEL WIDTH
            m_pIntegrated.nYBmpPixel[9]     = 480;      // Y PIXEL HEIGHT 
            m_pIntegrated.nXBmpOffset[9]    = 0;        // X PIXEL OFFSET
            m_pIntegrated.nYBmpOffset[9]    = 0;        // Y PIXEL OFFSET
            m_pIntegrated.nWAverageNum[9]   = 1;        // Frame Average : 1
            m_pIntegrated.nFilterNum[9]     = 1;        // Filter ( 0->0x0008, 1->0x0010, 2->0x0020, 3->0x8000 )
            m_pIntegrated.nXScanAmpVal[9]   = 2766;     // 2766,  6.75[v] : X축 배율보정 (크기)
            m_pIntegrated.nXScanOffset[9]   = 2203;     // 2203, -1.18[v] : X축 위치보정 (센터)
            m_pIntegrated.nYScanAmpVal[9]   = 1446;     // 1446,  3.53[v] : Y축 배율보정 (크기) 
            m_pIntegrated.nYScanOffset[9]   = 2050;     // 2050,  0.01[v] : Y축 위치보정 (센터)
            m_pIntegrated.nSkipPixNum[9]    = 190;      // 0 - 999 -> SCAN SKIP PIXEL COUNT ( 일그러진 SCAN 이미지 SKIP )
            m_pIntegrated.nFrameSec[9]      = 234;      // 0.23[f/s]
            m_pIntegrated.nOverSampleNum[9] = 362;      // Over Sampling Value
            //--
            // 10 : Half Slow 2  -  <  0.3 Frame/sec >
            //--
            m_pIntegrated.nFPGABmpSize[10]   = 1;       // 640x480
            m_pIntegrated.nScanCodeA[10]     = 0xA1FF;
            m_pIntegrated.nScanCodeB[10]     = 0xB1FF;
            m_pIntegrated.nXScanUpNum[10]    = 2048;    // 1200
            m_pIntegrated.nXScanDownNum[10]  = 16;      //
            m_pIntegrated.nYScanUpNum[10]    = 484;     //  500
            m_pIntegrated.nYScanDownNum[10]  = 4;
            m_pIntegrated.nXBmpPixel[10]     = 640;
            m_pIntegrated.nYBmpPixel[10]     = 480;
            m_pIntegrated.nXBmpOffset[10]    = 0;
            m_pIntegrated.nYBmpOffset[10]    = 0;
            m_pIntegrated.nWAverageNum[10]   = 1;
            m_pIntegrated.nFilterNum[10]     = 1;
            m_pIntegrated.nXScanAmpVal[10]   = 1385;    //1752;
            m_pIntegrated.nXScanOffset[10]   = 155;
            m_pIntegrated.nYScanAmpVal[10]   = 1072;
            m_pIntegrated.nYScanOffset[10]   = 6;
            m_pIntegrated.nSkipPixNum[10]    = 10;      // 0 - 999
            m_pIntegrated.nFrameSec[10]      = 340;     // 0.34 f/s
            m_pIntegrated.nOverSampleNum[10] = 414;     // XScan Freq. => 1.8kHz
            //--
            // 11 : Half Slow 3  -  <  0.15 Frame/sec >  <<<<< SLOW2 SCAN MODE >>>>>
            //--
            m_pIntegrated.nFPGABmpSize[11]   = 1;       // 640x480
            m_pIntegrated.nScanCodeA[11]     = 0xA1FF;  // Default
            m_pIntegrated.nScanCodeB[11]     = 0xB1FF;  // Default 
            m_pIntegrated.nXScanUpNum[11]    = 2048;    // X Scan Up   Value
            m_pIntegrated.nXScanDownNum[11]  = 8;       // X Scan Down Value
            m_pIntegrated.nYScanUpNum[11]    = 484;     // Y Scan Up   Value
            m_pIntegrated.nYScanDownNum[11]  = 4;       // Y Scan Down Value
            m_pIntegrated.nXBmpPixel[11]     = 640;     // X PIXEL WIDTH
            m_pIntegrated.nYBmpPixel[11]     = 480;     // Y PIXEL HEIGHT 
            m_pIntegrated.nXBmpOffset[11]    = 0;       // X PIXEL OFFSET
            m_pIntegrated.nYBmpOffset[11]    = 0;       // Y PIXEL OFFSET
            m_pIntegrated.nWAverageNum[11]   = 1;       // Frame Average : 1
            m_pIntegrated.nFilterNum[11]     = 1;       // Filter ( 0->0x0008, 1->0x0010, 2->0x0020, 3->0x8000 )
            m_pIntegrated.nXScanAmpVal[11]   = 2305;    // 2305,  5.63[v] : X축 배율보정 (크기)
            m_pIntegrated.nXScanOffset[11]   = 1744;    // 1744, -0.74[v] : X축 위치보정 (센터)
            m_pIntegrated.nYScanAmpVal[11]   = 1450;    // 1450,  3.54[v] : Y축 배율보정 (크기) 
            m_pIntegrated.nYScanOffset[11]   = 2040;    // 2040, -0.02[v] : Y축 위치보정 (센터)
            m_pIntegrated.nSkipPixNum[11]    = 190;     // 0 - 999 -> SCAN SKIP PIXEL COUNT ( 일그러진 SCAN 이미지 SKIP )
            m_pIntegrated.nFrameSec[11]      = 123;     // 0.12[f/s]
            m_pIntegrated.nOverSampleNum[11] = 900;     // Over Sampling Value
            //--
            // Full Fast Scan Mode
            //--
            // 12 : Full Fast1 < 1280x960 : 3.8 frame/sec >
            //--
            m_pIntegrated.nFPGABmpSize[12]   = 2;        // 1280x960
            m_pIntegrated.nScanCodeA[12]     = 0xA2FF;
            m_pIntegrated.nScanCodeB[12]     = 0xB2FF;
            m_pIntegrated.nXScanUpNum[12]    = 2048;     // 2400
            m_pIntegrated.nXScanDownNum[12]  = 128;
            m_pIntegrated.nYScanUpNum[12]    = 964;      // 1000
            m_pIntegrated.nYScanDownNum[12]  = 4;
            m_pIntegrated.nXBmpPixel[12]     = 1280;
            m_pIntegrated.nYBmpPixel[12]     = 960;
            m_pIntegrated.nXBmpOffset[12]    = 0;
            m_pIntegrated.nYBmpOffset[12]    = 0;
            m_pIntegrated.nWAverageNum[12]   = 1;
            m_pIntegrated.nFilterNum[12]     = 1;
            m_pIntegrated.nXScanAmpVal[12]   = 1694;
            m_pIntegrated.nXScanOffset[12]   = 100;
            m_pIntegrated.nYScanAmpVal[12]   = 1084;
            m_pIntegrated.nYScanOffset[12]   = 7;
            m_pIntegrated.nSkipPixNum[12]    = 204;      // 0 - 1999
            m_pIntegrated.nFrameSec[12]      = 3800;     // 3.8f/s
            m_pIntegrated.nOverSampleNum[12] = 6;        // XScan Freq. => 1.8kHz
            //--
            // 13: Full Fast2 < 1280x960 : 1.9 frame/sec >
            //--
            m_pIntegrated.nFPGABmpSize[13]   = 2;        // 1280x960
            m_pIntegrated.nScanCodeA[13]     = 0xA2FF;
            m_pIntegrated.nScanCodeB[13]     = 0xB2FF;
            m_pIntegrated.nXScanUpNum[13]    = 2048;     // 2400
            m_pIntegrated.nXScanDownNum[13]  = 64;
            m_pIntegrated.nYScanUpNum[13]    = 964;      // 1000
            m_pIntegrated.nYScanDownNum[13]  = 4;
            m_pIntegrated.nXBmpPixel[13]     = 1280;
            m_pIntegrated.nYBmpPixel[13]     = 960;
            m_pIntegrated.nXBmpOffset[13]    = 0;
            m_pIntegrated.nYBmpOffset[13]    = 0;
            m_pIntegrated.nWAverageNum[13]   = 1;
            m_pIntegrated.nFilterNum[13]     = 1;
            m_pIntegrated.nXScanAmpVal[13]   = 1652;
            m_pIntegrated.nXScanOffset[13]   = 270;
            m_pIntegrated.nYScanAmpVal[13]   = 1075;
            m_pIntegrated.nYScanOffset[13]   = 10;
            m_pIntegrated.nSkipPixNum[13]    = 78;       // 0 - 1999
            m_pIntegrated.nFrameSec[13]      = 1900;     // 1.9f/s
            m_pIntegrated.nOverSampleNum[13] = 12;       // XScan Freq. =>  900Hz
            //--
            // 14: Full Fast3 < 1280x960 : 0.9 frame/sec >
            //--
            m_pIntegrated.nFPGABmpSize[14]   = 2;        // 1280x960
            m_pIntegrated.nScanCodeA[14]     = 0xA2FF;
            m_pIntegrated.nScanCodeB[14]     = 0xB2FF;
            m_pIntegrated.nXScanUpNum[14]    = 1800;     // 2000
            m_pIntegrated.nXScanDownNum[14]  = 200;
            m_pIntegrated.nYScanUpNum[14]    = 976;      // 1000
            m_pIntegrated.nYScanDownNum[14]  = 24;
            m_pIntegrated.nXBmpPixel[14]     = 1280;
            m_pIntegrated.nYBmpPixel[14]     = 960;
            m_pIntegrated.nXBmpOffset[14]    = 0;
            m_pIntegrated.nYBmpOffset[14]    = 0;
            m_pIntegrated.nWAverageNum[14]   = 1;
            m_pIntegrated.nFilterNum[14]     = 1;
            m_pIntegrated.nXScanAmpVal[14]   = 1476;
            m_pIntegrated.nXScanOffset[14]   = 120;
            m_pIntegrated.nYScanAmpVal[14]   = 1085;
            m_pIntegrated.nYScanOffset[14]   = 10;
            m_pIntegrated.nSkipPixNum[14]    = 39;       // 0 - 1999
            m_pIntegrated.nFrameSec[14]      = 900;      // 0.9f/s
            m_pIntegrated.nOverSampleNum[14] = 49;       // XScan Freq. =>  900Hz
            //--
            // Full Slow Scan Mode
            //--
            // 15 : Full Slow1 < 1280x960 : 2.5 sec/frame > 0.4 f/s
            //--
            m_pIntegrated.nFPGABmpSize[15]   = 2;        // 1280x960
            m_pIntegrated.nScanCodeA[15]     = 0xA2FF;
            m_pIntegrated.nScanCodeB[15]     = 0xB2FF;
            m_pIntegrated.nXScanUpNum[15]    = 2048;     // 2000
            m_pIntegrated.nXScanDownNum[15]  = 64;
            m_pIntegrated.nYScanUpNum[15]    = 964;      // 1000
            m_pIntegrated.nYScanDownNum[15]  = 4;
            m_pIntegrated.nXBmpPixel[15]     = 1280;
            m_pIntegrated.nYBmpPixel[15]     = 960;
            m_pIntegrated.nXBmpOffset[15]    = 0;
            m_pIntegrated.nYBmpOffset[15]    = 0;
            m_pIntegrated.nWAverageNum[15]   = 1;
            m_pIntegrated.nFilterNum[15]     = 1;
            m_pIntegrated.nXScanAmpVal[15]   = 1476;
            m_pIntegrated.nXScanOffset[15]   = 120;
            m_pIntegrated.nYScanAmpVal[15]   = 1085;
            m_pIntegrated.nYScanOffset[15]   = 0;
            m_pIntegrated.nSkipPixNum[15]    = 39;       // 0 - 1999
            m_pIntegrated.nFrameSec[15]      = 400;      // 0.4 f/s
            m_pIntegrated.nOverSampleNum[15] = 49;       // XScan Freq. => 1.44MHz / 2000 = 720Hz /2 = 360Hz
            //--
            // 16 : Full Slow2 < 1280x960 : 5 sec/frame > 0.2 f/s
            //--
            m_pIntegrated.nFPGABmpSize[16]   = 2;        // 1280x960
            m_pIntegrated.nScanCodeA[16]     = 0xA2FF;
            m_pIntegrated.nScanCodeB[16]     = 0xB2FF;
            m_pIntegrated.nXScanUpNum[16]    = 2048;     // 2000
            m_pIntegrated.nXScanDownNum[16]  = 32;
            m_pIntegrated.nYScanUpNum[16]    = 964;      // 1000
            m_pIntegrated.nYScanDownNum[16]  = 4;
            m_pIntegrated.nXBmpPixel[16]     = 1280;
            m_pIntegrated.nYBmpPixel[16]     = 960;
            m_pIntegrated.nXBmpOffset[16]    = 0;
            m_pIntegrated.nYBmpOffset[16]    = 0;
            m_pIntegrated.nWAverageNum[16]   = 1;
            m_pIntegrated.nFilterNum[16]     = 1;
            m_pIntegrated.nXScanAmpVal[16]   = 1476;
            m_pIntegrated.nXScanOffset[16]   = 120;
            m_pIntegrated.nYScanAmpVal[16]   = 1085;
            m_pIntegrated.nYScanOffset[16]   = 10;
            m_pIntegrated.nSkipPixNum[16]    = 39;       // 0 - 1999
            m_pIntegrated.nFrameSec[16]      = 200;      // 0.2 f/s
            m_pIntegrated.nOverSampleNum[16] = 97;       // XScan Freq. => 1.44MHz / 2000 = 720Hz /2 = 360Hz
            //--
            // 17 : Full Slow3 < 1280x960 : 10 sec/frame >  0.1 f/s
            //--
            m_pIntegrated.nFPGABmpSize[17]   = 2;        // 1280x960
            m_pIntegrated.nScanCodeA[17]     = 0xA2FF;
            m_pIntegrated.nScanCodeB[17]     = 0xB2FF;
            m_pIntegrated.nXScanUpNum[17]    = 2048;     // 2000
            m_pIntegrated.nXScanDownNum[17]  = 16;
            m_pIntegrated.nYScanUpNum[17]    = 964;      // 1000
            m_pIntegrated.nYScanDownNum[17]  = 4;
            m_pIntegrated.nXBmpPixel[17]     = 640 * 2;
            m_pIntegrated.nYBmpPixel[17]     = 480 * 2;
            m_pIntegrated.nXBmpOffset[17]    = 0;
            m_pIntegrated.nYBmpOffset[17]    = 0;
            m_pIntegrated.nWAverageNum[17]   = 1;
            m_pIntegrated.nFilterNum[17]     = 1;
            m_pIntegrated.nXScanAmpVal[17]   = 1476;
            m_pIntegrated.nXScanOffset[17]   = 120;
            m_pIntegrated.nYScanAmpVal[17]   = 1085;
            m_pIntegrated.nYScanOffset[17]   = 10;
            m_pIntegrated.nSkipPixNum[17]    = 39;       // 0 - 1999
            m_pIntegrated.nFrameSec[17]      = 100;      // 0.1f/s
            m_pIntegrated.nOverSampleNum[17] = 200;      // XScan Freq. => 4.8MHz / 2000 = 240Hz /2 = 120Hz
            //--
            //--
            //--
            // Photo Scan Mode
            //--
            // 18 : Photo 1 < 640x480 : 16 sec/frame > 0.0625f/s <<<<< PHOTO1 SCAN MODE >>>>>
            //--
            m_pIntegrated.nFPGABmpSize[18]   = 1;        // 640x480
            m_pIntegrated.nScanCodeA[18]     = 0xA1FF;   // Default
            m_pIntegrated.nScanCodeB[18]     = 0xB1FF;   // Default 
            m_pIntegrated.nXScanUpNum[18]    = 2048;     // X Scan Up   Value
            m_pIntegrated.nXScanDownNum[18]  = 8;        // X Scan Down Value
            m_pIntegrated.nYScanUpNum[18]    = 484;      // Y Scan Up   Value
            m_pIntegrated.nYScanDownNum[18]  = 4;        // Y Scan Down Value
            m_pIntegrated.nXBmpPixel[18]     = 640;      // X PIXEL WIDTH
            m_pIntegrated.nYBmpPixel[18]     = 480;      // Y PIXEL HEIGHT 
            m_pIntegrated.nXBmpOffset[18]    = 0;        // X PIXEL OFFSET
            m_pIntegrated.nYBmpOffset[18]    = 0;        // Y PIXEL OFFSET
            m_pIntegrated.nWAverageNum[18]   = 1;        // Frame Average : 1
            m_pIntegrated.nFilterNum[18]     = 1;        // Filter ( 0->0x0008, 1->0x0010, 2->0x0020, 3->0x8000 )
            m_pIntegrated.nXScanAmpVal[18]   = 2448;     // 2448,  5.98[v] : X축 배율보정 (크기)
            m_pIntegrated.nXScanOffset[18]   = 2453;     // 2453,  0.99[v] : X축 위치보정 (센터)
            m_pIntegrated.nYScanAmpVal[18]   = 1455;     // 1455,  3.55[v] : Y축 배율보정 (크기) 
            m_pIntegrated.nYScanOffset[18]   = 2052;     // 2052,  0.01[v] : Y축 위치보정 (센터)
            m_pIntegrated.nSkipPixNum[18]    = 84;       // 0 - 999 -> SCAN SKIP PIXEL COUNT ( 일그러진 SCAN 이미지 SKIP )
            m_pIntegrated.nFrameSec[18]      = 62;       // 0.062[f/s]
            m_pIntegrated.nOverSampleNum[18] = 1675;     // Over Sampling Value
            //--
            // 19 : Photo 2 < 640x480 : 33.3 sec/frame > 0.031f/s <<<<< PHOTO2 SCAN MODE >>>>>
            //--
            m_pIntegrated.nFPGABmpSize[19]   = 1;        // 640x480
            m_pIntegrated.nScanCodeA[19]     = 0xA1FF;   // Default
            m_pIntegrated.nScanCodeB[19]     = 0xB1FF;   // Default 
            m_pIntegrated.nXScanUpNum[19]    = 2048;     // X Scan Up   Value
            m_pIntegrated.nXScanDownNum[19]  = 8;        // X Scan Down Value
            m_pIntegrated.nYScanUpNum[19]    = 484;      // Y Scan Up   Value
            m_pIntegrated.nYScanDownNum[19]  = 4;        // Y Scan Down Value
            m_pIntegrated.nXBmpPixel[19]     = 640;      // X PIXEL WIDTH
            m_pIntegrated.nYBmpPixel[19]     = 480;      // Y PIXEL HEIGHT 
            m_pIntegrated.nXBmpOffset[19]    = 0;        // X PIXEL OFFSET
            m_pIntegrated.nYBmpOffset[19]    = 0;        // Y PIXEL OFFSET
            m_pIntegrated.nWAverageNum[19]   = 1;        // Frame Average : 1
            m_pIntegrated.nFilterNum[19]     = 1;        // Filter ( 0->0x0008, 1->0x0010, 2->0x0020, 3->0x8000 )
            m_pIntegrated.nXScanAmpVal[19]   = 2168;     // 2168,  5.29[v] : X축 배율보정 (크기)
            m_pIntegrated.nXScanOffset[19]   = 2355;     // 2355,  0.75[v] : X축 위치보정 (센터)
            m_pIntegrated.nYScanAmpVal[19]   = 1452;     // 1452,  3.55[v] : Y축 배율보정 (크기)  
            m_pIntegrated.nYScanOffset[19]   = 2062;     // 2062,  0.04[v] : Y축 위치보정 (센터)
            m_pIntegrated.nSkipPixNum[19]    = 38;       // 0 - 999 -> SCAN SKIP PIXEL COUNT ( 일그러진 SCAN 이미지 SKIP )
            m_pIntegrated.nFrameSec[19]      = 41;       // 0.041[f/s]
            m_pIntegrated.nOverSampleNum[19] = 2838;     // Over Sampling Value
            //--
            // 20 : Photo 3 < 1280x960 : 33.3 sec/frame = 0.031 f/s > <<<<< PHOTO3 SCAN MODE >>>>>
            //--
            m_pIntegrated.nFPGABmpSize[20]   = 2;        // 1280x960
            m_pIntegrated.nScanCodeA[20]     = 0xA2FF;   // Default
            m_pIntegrated.nScanCodeB[20]     = 0xB2FF;   // Default 
            m_pIntegrated.nXScanUpNum[20]    = 2048;     // X Scan Up   Value
            m_pIntegrated.nXScanDownNum[20]  = 4;        // X Scan Down Value
            m_pIntegrated.nYScanUpNum[20]    = 964;      // Y Scan Up   Value
            m_pIntegrated.nYScanDownNum[20]  = 4;        // Y Scan Down Value
            m_pIntegrated.nXBmpPixel[20]     = 1280;     // X PIXEL WIDTH
            m_pIntegrated.nYBmpPixel[20]     = 960;      // Y PIXEL HEIGHT 
            m_pIntegrated.nXBmpOffset[20]    = 0;        // X PIXEL OFFSET
            m_pIntegrated.nYBmpOffset[20]    = 0;        // Y PIXEL OFFSET
            m_pIntegrated.nWAverageNum[20]   = 1;        // Frame Average : 1
            m_pIntegrated.nFilterNum[20]     = 1;        // Filter ( 0->0x0008, 1->0x0010, 2->0x0020, 3->0x8000 )
            m_pIntegrated.nXScanAmpVal[20]   = 1733;     // 1733,  4.23[v] : X축 배율보정 (크기)
            m_pIntegrated.nXScanOffset[20]   = 1993;     // 1993, -0.13[v] : X축 위치보정 (센터)
            m_pIntegrated.nYScanAmpVal[20]   = 1445;     // 1445,  3.53[v] : Y축 배율보정 (크기)  
            m_pIntegrated.nYScanOffset[20]   = 2045;     // 2045, -0.01[v] : Y축 위치보정 (센터)
            m_pIntegrated.nSkipPixNum[20]    = 69;       // 0 - 1999 -> SCAN SKIP PIXEL COUNT ( 일그러진 SCAN 이미지 SKIP )
            m_pIntegrated.nFrameSec[20]      = 31;       // 0.031[f/s]
            m_pIntegrated.nOverSampleNum[20] = 1186;     // Over Sampling Value
            //--
            // 21 : Photo 4 < 1280x960 : 66.6 sec/frame = 0.015 f/s > <<<<< PHOTO4 SCAN MODE >>>>>
            //--
            m_pIntegrated.nFPGABmpSize[21]   = 2;        // 1280x960
            m_pIntegrated.nScanCodeA[21]     = 0xA2FF;   // Default
            m_pIntegrated.nScanCodeB[21]     = 0xB2FF;   // Default 
            m_pIntegrated.nXScanUpNum[21]    = 3000;     // X Scan Up   Value
            m_pIntegrated.nXScanDownNum[21]  = 4;        // X Scan Down Value
            m_pIntegrated.nYScanUpNum[21]    = 964;      // Y Scan Up   Value
            m_pIntegrated.nYScanDownNum[21]  = 4;        // Y Scan Down Value
            m_pIntegrated.nXBmpPixel[21]     = 1280;     // X PIXEL WIDTH
            m_pIntegrated.nYBmpPixel[21]     = 960;      // Y PIXEL HEIGHT 
            m_pIntegrated.nXBmpOffset[21]    = 0;        // X PIXEL OFFSET
            m_pIntegrated.nYBmpOffset[21]    = 0;        // Y PIXEL OFFSET
            m_pIntegrated.nWAverageNum[21]   = 1;        // Frame Average : 1
            m_pIntegrated.nFilterNum[21]     = 1;        // Filter ( 0->0x0008, 1->0x0010, 2->0x0020, 3->0x8000 )
            m_pIntegrated.nXScanAmpVal[21]   = 3058;     // 3058,  7.47[v] : X축 배율보정 (크기) 
            m_pIntegrated.nXScanOffset[21]   = 2330;     // 2330,  0.69[v] : X축 위치보정 (센터)
            m_pIntegrated.nYScanAmpVal[21]   = 1438;     // 1438,  3.51[v] : Y축 배율보정 (크기)  
            m_pIntegrated.nYScanOffset[21]   = 2050;     // 2050,  0.01[v] : Y축 위치보정 (센터)
            m_pIntegrated.nSkipPixNum[21]    = 122;      // 0 - 1999 -> SCAN SKIP PIXEL COUNT ( 일그러진 SCAN 이미지 SKIP )
            m_pIntegrated.nFrameSec[21]      = 25;       // 0.025[f/s]
            m_pIntegrated.nOverSampleNum[21] = 1137;     // Over Sampling Value
            //--
            // 22 : Photo 5 < 1280x960 : 133.3 sec/frame = 0.0075 f/s > <<<<< PHOTO5 SCAN MODE >>>>>
            //--
            m_pIntegrated.nFPGABmpSize[22]   = 2;        // 1280x960
            m_pIntegrated.nScanCodeA[22]     = 0xA2FF;   // Default
            m_pIntegrated.nScanCodeB[22]     = 0xB2FF;   // Default 
            m_pIntegrated.nXScanUpNum[22]    = 2946;     // X Scan Up   Value
            m_pIntegrated.nXScanDownNum[22]  = 4;        // X Scan Down Value
            m_pIntegrated.nYScanUpNum[22]    = 964;      // Y Scan Up   Value
            m_pIntegrated.nYScanDownNum[22]  = 4;        // Y Scan Down Value
            m_pIntegrated.nXBmpPixel[22]     = 1280;     // X PIXEL WIDTH
            m_pIntegrated.nYBmpPixel[22]     = 960;      // Y PIXEL HEIGHT 
            m_pIntegrated.nXBmpOffset[22]    = 0;        // X PIXEL OFFSET
            m_pIntegrated.nYBmpOffset[22]    = 0;        // Y PIXEL OFFSET
            m_pIntegrated.nWAverageNum[22]   = 1;        // Frame Average : 1
            m_pIntegrated.nFilterNum[22]     = 1;        // Filter ( 0->0x0008, 1->0x0010, 2->0x0020, 3->0x8000 )
            m_pIntegrated.nXScanAmpVal[22]   = 3900;     // 3900,  9.52[v] : X축 배율보정 (크기) 
            m_pIntegrated.nXScanOffset[22]   = 3048;     // 3048,  2.45[v] : X축 위치보정 (센터)
            m_pIntegrated.nYScanAmpVal[22]   = 1453;     // 1453,  3.55[v] : Y축 배율보정 (크기)  
            m_pIntegrated.nYScanOffset[22]   = 2020;     // 2020, -0.07[v] : Y축 위치보정 (센터)
            m_pIntegrated.nSkipPixNum[22]    = 58;       // 0 - 1999 -> SCAN SKIP PIXEL COUNT ( 일그러진 SCAN 이미지 SKIP )
            m_pIntegrated.nFrameSec[22]      = 6;        // 0.006[f/s]
            m_pIntegrated.nOverSampleNum[22] = 3800;     // Over Sampling Value
            //--
            // 23 : Photo 6 < 2560x1920 : 133.3 sec/frame = 0.0075 f/s > <<<<< PHOTO6 SCAN MODE >>>>>
            //--
            m_pIntegrated.nFPGABmpSize[23]   = 3;        // 2560x1920
            m_pIntegrated.nScanCodeA[23]     = 0xA3FF;   // Default
            m_pIntegrated.nScanCodeB[23]     = 0xB3FF;   // Default 
            m_pIntegrated.nXScanUpNum[23]    = 3000;     // X Scan Up   Value
            m_pIntegrated.nXScanDownNum[23]  = 4;        // X Scan Down Value
            m_pIntegrated.nYScanUpNum[23]    = 1924;     // Y Scan Up   Value
            m_pIntegrated.nYScanDownNum[23]  = 4;        // Y Scan Down Value
            m_pIntegrated.nXBmpPixel[23]     = 2560;     // X PIXEL WIDTH
            m_pIntegrated.nYBmpPixel[23]     = 1920;     // Y PIXEL HEIGHT 
            m_pIntegrated.nXBmpOffset[23]    = 0;        // X PIXEL OFFSET
            m_pIntegrated.nYBmpOffset[23]    = 0;        // Y PIXEL OFFSET
            m_pIntegrated.nWAverageNum[23]   = 1;        // Frame Average : 1
            m_pIntegrated.nFilterNum[23]     = 1;        // Filter ( 0->0x0008, 1->0x0010, 2->0x0020, 3->0x8000 )
            m_pIntegrated.nXScanAmpVal[23]   = 2730;     // 2730,  6.67[v] : X축 배율보정 (크기) 
            m_pIntegrated.nXScanOffset[23]   = 2260;     // 2260,  0.52[v] : X축 위치보정 (센터)
            m_pIntegrated.nYScanAmpVal[23]   = 1445;     // 1445,  3.53[v] : Y축 배율보정 (크기)  
            m_pIntegrated.nYScanOffset[23]   = 2045;     // 2020, -0.01[v] : Y축 위치보정 (센터)
            m_pIntegrated.nSkipPixNum[23]    = 139;      // 0 - 3999 -> SCAN SKIP PIXEL COUNT ( 일그러진 SCAN 이미지 SKIP )
            m_pIntegrated.nFrameSec[23]      = 6;        // 0.006[f/s]
            m_pIntegrated.nOverSampleNum[23] = 1338;     // Over Sampling Value
            //--
            //--
            //--
            // WFM Mode
            //--
            // 24 : WFM
            //--
            m_pIntegrated.nFPGABmpSize[24]   = 5;
            m_pIntegrated.nScanCodeA[24]     = 0xA5FF;
            m_pIntegrated.nScanCodeB[24]     = 0xB5FF;
            m_pIntegrated.nXScanUpNum[24]    = 899;
            m_pIntegrated.nXScanDownNum[24]  = 100;
            m_pIntegrated.nYScanUpNum[24]    = 480;
            m_pIntegrated.nYScanDownNum[24]  = 20;
            m_pIntegrated.nXBmpPixel[24]     = 640;
            m_pIntegrated.nYBmpPixel[24]     = 1;
            m_pIntegrated.nXBmpOffset[24]    = 0;
            m_pIntegrated.nYBmpOffset[24]    = 0;
            m_pIntegrated.nWAverageNum[24]   = 1;
            m_pIntegrated.nFilterNum[24]     = 6;
            m_pIntegrated.nXScanAmpVal[24]   = 590;
            m_pIntegrated.nXScanOffset[24]   = 23;
            m_pIntegrated.nYScanAmpVal[24]   = 441;
            m_pIntegrated.nYScanOffset[24]   = 30;
            m_pIntegrated.nSkipPixNum[24]    = 10;       // 0 - 719
            m_pIntegrated.nFrameSec[24]      = 1;        // 0 - 499
            m_pIntegrated.nOverSampleNum[24] = 862;
            //--
            //--
            //--
            //--
            // Auto Focus Mode
            //--
            // 25 : AutoFocus
            //--
            m_pIntegrated.nFPGABmpSize[25]   = 6;
            m_pIntegrated.nScanCodeA[25]     = 0xA6FF;
            m_pIntegrated.nXScanAmpVal[25]   = 0;       // not used
            m_pIntegrated.nXScanOffset[25]   = 0;       // not used
            m_pIntegrated.nYScanAmpVal[25]   = 0;       // not used
            m_pIntegrated.nYScanOffset[25]   = 0;       // not used
            m_pIntegrated.nXScanUpNum[25]    = 0;       // not used
            m_pIntegrated.nXScanDownNum[25]  = 0;       // not used
            m_pIntegrated.nYScanUpNum[25]    = 0;       // not used
            m_pIntegrated.nYScanDownNum[25]  = 0;       // not used
            m_pIntegrated.nSkipPixNum[25]    = 0;       // not used
            m_pIntegrated.nFrameSec[25]      = 0;       // not used
            m_pIntegrated.nWAverageNum[25]   = 1;
            m_pIntegrated.nFilterNum[25]     = 6;
            m_pIntegrated.nOverSampleNum[25] = 150;
            //--
            //--
            //--

            //--
            // Half Red Scan Mode : 320x240
            //--
            // 0 : Half Red  1  - over: 28     < 14.4 Frame/sec >
            // 1 : Half Red  2  - over: 56     <  7.2 Frame/sec >
            //--
            // Full Red Scan Mode : 640x480
            //--
            // 2 : Full Red  1  - over: 56     < 7.2 Frame/sec >
            // 3 : Full Red  2  - over: 56     <  3.6 Frame/sec >
            //--
            // Half Fast Scan Mode
            //--
            // 4 : Half Fast High 1
            // 5 : Half Fast Low 2
            //--
            // Half Slow Scan Mode
            //--
            // 6 : Half Slow 1  - over: 256    < 2.08 sec/frame = 0.48 Frame/sec >
            // 7 : Half Slow 2  - over: 512   < 4.16 sec/frame = 0.24 Frame/sec >
            // 8 : Half Slow 3  - over: 1024   < 8.32 sec/frame = 0.12 Frame/sec  >
            //--
            // Photo Scan Mode
            //--
            // 9:   Photo 1 < 640x480 : 16.7 sec/frame = 0.06 Frame/sec >
            // 10 : Photo 2 < 640x480 : 33.3 sec/frame = 0.03 Frame/sec >
            // 20 : Photo 3 < 1280x960 : 33.3 sec/frame = 0.03 Frame/sec >
            // 11 : Photo 4 < 1280x960 : 66.6 sec/frame = 0.015 Frame/sec >
            // 12 : Photo 5 < 2560x1920 : 133.3 sec/frame = 0.0075 Frame/sec >
            //--
            // Full Screen Mode
            //--
            //--
            // Full Fast Scan Mode
            //--
            // 13 : Full Fast1 < 1280x960 : 1.8 frame/sec >
            // 14 : Full Fast2 < 1280x960 : 0.9 frame/sec >
            //--
            // Full Slow Scan Mode
            //--
            // 15 : Full Slow1 < 1280x960 : 2.22 sec/frame >
            // 16 : Full Slow2 < 1280x960 : 8.88 sec/frame >
            // 17 : Full Slow3 < 1280x960 : 17.6 sec/frame >
            //--
            // WFM Mode
            //--
            // 18 : WFM
            //--


            //
            //   DM = 4개 항목
            //
            //                               80              320                560
            //                          DM_XMoveLeft    DM_XMoveCenter      DM_XMoveRight
            //                        -------------------------------------------------
            //                        | 0                    320                   640|
            //     DM_YMoveTop      60|                                               |
            //                        |                                               |
            //     DM_YMoveCenter  240|240                                            |
            //                        |                                               |
            //     DM_YMoveBottom  420|                                               |
            //                        |480                                            |
            //                        -------------------------------------------------
            //
            //               2120             200
            //   a  = ( DM_XMoveRight - DM_XMoveLeft ) / (560 - 80)
            //
            //  da  = a * cx + ( DM_XMoveLeft * 7 - DM_XMoveRight ) / 6
            //
            //


            //--
            m_pIntegrated.nScreenMode      = NORMAL_SCREEN;
            //--
            // (1) Half Scan
            m_pIntegrated.nHalfRedScan     = RED_SCAN_MODE;        // 0, 1, 2
            m_pIntegrated.nHalfFastScan    = FAST_SCAN_MODE;       // 6, 7, 8
            m_pIntegrated.nHalfSlow1Scan   = SLOW1_SCAN_MODE;      // 9, 10, 11
            m_pIntegrated.nHalfSlow2Scan   = SLOW2_SCAN_MODE;      // 9, 10, 11
            // (2) Full Scan
            m_pIntegrated.nFullRedScan     = 3;                    // 3, 4, 5
            m_pIntegrated.nFullFastScan    = 12;                   // 12, 13, 14
            m_pIntegrated.nFullSlow1Scan   = 15;                   // 15, 16, 17
            m_pIntegrated.nFullSlow2Scan   = 17;                   // 15, 16, 17
            // (3) Photo Scan
            m_pIntegrated.nPhotoScan       = PHOTO1_SCAN_MODE;     // 18, 19, 20, 21, 22, 23
            //--
            m_pIntegrated.nXScanMoveLeft   = -654;
            m_pIntegrated.nXScanMoveRight  = 700;
            m_pIntegrated.nYScanMoveTop    = -538;
            m_pIntegrated.nYScanMoveBottom = 404;
            //--
            m_pIntegrated.nDMXMoveLeft     = 1125;
            m_pIntegrated.nDMXMoveRight    = 3040;
            m_pIntegrated.nDMYMoveTop      = 1245;
            m_pIntegrated.nDMYMoveBottom   = 2864;
            //--
            m_pIntegrated.nStageHorAlign   = -190;                 // SM-300 DP
            //-----
            //  30인치 LCD 모니터 기준
            //-----
            m_pIntegrated.nImageScreenSize = 160;                  // ( 160mm -> 640Pixel )
            m_pIntegrated.nMicronBarMag    = 100;                  // x100   x100   x100    |  x200    Magnification
            m_pIntegrated.dMicronBarSize   = 300.0;                // 300um  200um  100um   |  100um   Mircon Bar Size

            //--
            // 위 세가지 정보에 의해 결정됨
            //--
            //                                  0.64 => ( 640 Pixel,  200um-> 0.2mm )
            //
            m_pIntegrated.nMicronBarPixelNum = (int)(0.64 * m_pIntegrated.nMicronBarMag * m_pIntegrated.dMicronBarSize / m_pIntegrated.nImageScreenSize);
            //--

            m_pIntegrated.nMotorSpeed    = 1; // Mid
            m_pIntegrated.nWDTopLimit    = 4;
            m_pIntegrated.nWDBottomLimit = 51;

            m_pIntegrated.dCurXPos       = 0.0;
            m_pIntegrated.dCurYPos       = 0.0;
            m_pIntegrated.dCurZPos       = 15.0;
            m_pIntegrated.nCurRDeg       = 0; // 0 - 3600
            m_pIntegrated.nCurTDeg       = 0; // 0 -   70

            m_pIntegrated.nPhotoNum          = 1234;
            m_pIntegrated.bViewParameterFlag = TRUE;

            ZeroMem(m_pIntegrated.szUserText);
            strcpy_s(m_pIntegrated.szUserText, "ModuleSci ");

            //----------------------------------------
            m_pIntegrated.bUserTextViewFlag     = TRUE;
            m_pIntegrated.bGunHViewFlag         = TRUE;
            m_pIntegrated.bMagViewFlag          = TRUE;
            m_pIntegrated.bSpotSizeViewFlag     = TRUE;
            m_pIntegrated.bWorkDistViewFlag     = TRUE;
            m_pIntegrated.bMicroBarViewFlag     = TRUE;
            m_pIntegrated.bPhotoNumViewFlag     = FALSE;
            m_pIntegrated.bDateTimeViewFlag     = FALSE;
            //----------------------------------------
            m_pIntegrated.bAutoZoomMicroBarFlag = FALSE;
            m_pIntegrated.bMedianFilterFlag     = FALSE;
            m_pIntegrated.bMovingAverageFlag    = FALSE;
            m_pIntegrated.bRedBottomClearFlag   = FALSE;
            m_pIntegrated.bPhotoBottomClearFlag = FALSE;
            m_pIntegrated.bEucentricRotFlag     = TRUE;
            m_pIntegrated.bZFocusLinkFlag       = TRUE;
            //----------------------------------------
            m_pIntegrated.bAFBaseOnWDFlag       = FALSE;
            m_pIntegrated.nOLCompensation       = OL_IMAGE_ZOOM_CHANGE;
            m_pIntegrated.nSemInfoType          = 0;

            //----------------------------------------
            // Mag. Relay 선택시 저항오차에 의한 배율교정
            //----------------------------------------
            m_pIntegrated.nZoomXMaxValue[0]  = 3799;
            m_pIntegrated.nZoomYMaxValue[0]  = 3399;
            m_pIntegrated.nZoomXMaxValue[1]  = 3817;
            m_pIntegrated.nZoomYMaxValue[1]  = 3327;
            m_pIntegrated.nZoomXMaxValue[2]  = 3615;
            m_pIntegrated.nZoomYMaxValue[2]  = 3195;
            m_pIntegrated.nZoomXMaxValue[3]  = 3110;
            m_pIntegrated.nZoomYMaxValue[3]  = 2923;
            m_pIntegrated.nZoomXMaxValue[4]  = 4095;
            m_pIntegrated.nZoomYMaxValue[4]  = 4095;
            //----------------------------------------
            m_pIntegrated.nBrightLowLimit    = 513;
            m_pIntegrated.nBrightHighLimit   = 2017;
            m_pIntegrated.nContrastLowLimit  = 1298;
            m_pIntegrated.nContrastHighLimit = 2505;
            m_pIntegrated.nStigXLowLimit     = 1000;
            m_pIntegrated.nStigXHighLimit    = 3000;
            m_pIntegrated.nStigYLowLimit     = 1000;
            m_pIntegrated.nStigYHighLimit    = 3000;
            //----------------------------------------

            //----------------------------------------
            //   직교도 보상 ----------------------------
            //----------------------------------------
            m_pIntegrated.nARAngle[0] = 0;    m_pIntegrated.nBRAngle[0] = 0;    m_pIntegrated.nCRAngle[0] = 90;    m_pIntegrated.nDRAngle[0] = 0;
            m_pIntegrated.nARAngle[1] = 0;    m_pIntegrated.nBRAngle[1] = -30;  m_pIntegrated.nCRAngle[1] = 120;   m_pIntegrated.nDRAngle[1] = 0;
            m_pIntegrated.nARAngle[2] = 0;    m_pIntegrated.nBRAngle[2] = 20;   m_pIntegrated.nCRAngle[2] = 170;   m_pIntegrated.nDRAngle[2] = -40;
            m_pIntegrated.nARAngle[3] = 10;   m_pIntegrated.nBRAngle[3] = 0;    m_pIntegrated.nCRAngle[3] = 150;   m_pIntegrated.nDRAngle[3] = 0;
            m_pIntegrated.nARAngle[4] = 0;    m_pIntegrated.nBRAngle[4] = -10;  m_pIntegrated.nCRAngle[4] = 130;   m_pIntegrated.nDRAngle[4] = 70;
            m_pIntegrated.nARAngle[5] = -50;  m_pIntegrated.nBRAngle[5] = 50;   m_pIntegrated.nCRAngle[5] = 70;    m_pIntegrated.nDRAngle[5] = 80;
            m_pIntegrated.nARAngle[6] = 10;   m_pIntegrated.nBRAngle[6] = 0;    m_pIntegrated.nCRAngle[6] = 0;     m_pIntegrated.nDRAngle[6] = 100;
            m_pIntegrated.nARAngle[7] = 50;   m_pIntegrated.nBRAngle[7] = 40;   m_pIntegrated.nCRAngle[7] = 150;   m_pIntegrated.nDRAngle[7] = 100;
            m_pIntegrated.nARAngle[8] = 80;   m_pIntegrated.nBRAngle[8] = 70;   m_pIntegrated.nCRAngle[8] = 70;    m_pIntegrated.nDRAngle[8] = 180;
            m_pIntegrated.nARAngle[9] = 60;   m_pIntegrated.nBRAngle[9] = -40;  m_pIntegrated.nCRAngle[9] = 120;   m_pIntegrated.nDRAngle[9] = 30;
            m_pIntegrated.nARAngle[10] = 0;   m_pIntegrated.nBRAngle[10] = 0;   m_pIntegrated.nCRAngle[10] = 90;   m_pIntegrated.nDRAngle[10] = 110;
            m_pIntegrated.nARAngle[11] = 40;  m_pIntegrated.nBRAngle[11] = -30; m_pIntegrated.nCRAngle[11] = 30;   m_pIntegrated.nDRAngle[11] = 40;
            m_pIntegrated.nARAngle[12] = 40;  m_pIntegrated.nBRAngle[12] = 20;  m_pIntegrated.nCRAngle[12] = -10;  m_pIntegrated.nDRAngle[12] = 0;
            m_pIntegrated.nARAngle[13] = 20;  m_pIntegrated.nBRAngle[13] = 60;  m_pIntegrated.nCRAngle[13] = -100; m_pIntegrated.nDRAngle[13] = 50;
            m_pIntegrated.nARAngle[14] = -40; m_pIntegrated.nBRAngle[14] = 50;  m_pIntegrated.nCRAngle[14] = -120; m_pIntegrated.nDRAngle[14] = 60;
            m_pIntegrated.nARAngle[15] = 50;  m_pIntegrated.nBRAngle[15] = 90;  m_pIntegrated.nCRAngle[15] = -60;  m_pIntegrated.nDRAngle[15] = -10;
            m_pIntegrated.nARAngle[16] = 20;  m_pIntegrated.nBRAngle[16] = 70;  m_pIntegrated.nCRAngle[16] = -120; m_pIntegrated.nDRAngle[16] = -30;
            m_pIntegrated.nARAngle[17] = 40;  m_pIntegrated.nBRAngle[17] = -10; m_pIntegrated.nCRAngle[17] = -70;  m_pIntegrated.nDRAngle[17] = -50;
            m_pIntegrated.nARAngle[18] = -20; m_pIntegrated.nBRAngle[18] = 50;  m_pIntegrated.nCRAngle[18] = 0;    m_pIntegrated.nDRAngle[18] = -30;
            m_pIntegrated.nARAngle[19] = -70; m_pIntegrated.nBRAngle[19] = -60; m_pIntegrated.nCRAngle[19] = -40;  m_pIntegrated.nDRAngle[19] = -60;
            m_pIntegrated.nARAngle[20] = -40; m_pIntegrated.nBRAngle[20] = -20; m_pIntegrated.nCRAngle[20] = -80;  m_pIntegrated.nDRAngle[20] = -80;
            m_pIntegrated.nARAngle[21] = -30; m_pIntegrated.nBRAngle[21] = 10;  m_pIntegrated.nCRAngle[21] = -70;  m_pIntegrated.nDRAngle[21] = -50;
            m_pIntegrated.nARAngle[22] = 0;   m_pIntegrated.nBRAngle[22] = 40;  m_pIntegrated.nCRAngle[22] = -20;  m_pIntegrated.nDRAngle[22] = -40;
            m_pIntegrated.nARAngle[23] = 0;   m_pIntegrated.nBRAngle[23] = -10; m_pIntegrated.nCRAngle[23] = 0;    m_pIntegrated.nDRAngle[23] = -60;
            //----------------------------------------
            m_pIntegrated.nRightAngle = 64;
            //----------------------------------------
            //   직교도 보상 -끝- -----------------------
            //----------------------------------------

            //----------------------------------------
            // 화일 초기화 변수와 Global 변수를 따로 사용
            //----------------------------------------
            m_pIntegrated.bNEGImageFlag      = FALSE;  // 음영반전 Flag
            m_pIntegrated.bLRMirrorFlag      = FALSE;  // 좌우반전 Flag
            m_pIntegrated.bTBMirrorFlag      = FALSE;  // 상하반전 Flag
            m_pIntegrated.bRED1FileScanFlag  = FALSE;  // RED1 File Scan Flag
            m_pIntegrated.bRED2FileScanFlag  = FALSE;  // RED2 File Scan Flag
            m_pIntegrated.bFULL1FileScanFlag = FALSE;  // FULL1 File Scan Flag
            m_pIntegrated.bFULL2FileScanFlag = FALSE;  // FULL2 File Scan Flag

            //----------------------------------------
            // 스캔모드 :
            //----------------------------------------
            // (1) Half Scan 모드 종류
            //     HalfRedScan   -> 0, 1, 2     >> 기본값 0
            //     HalfFastScan  -> 6, 7, 8     >> 기본값 6
            //     HalfSlow1Scan -> 9, 10, 11   >> 기본값 9
            //     HalfSlow2Scan -> 9, 10, 11   >> 기본값 11
            // (2) Full Scan 모드 종류
            //     FullRedScan   -> 3, 4, 5     >> 기본값 3
            //     FullFastScan  -> 12, 13, 14  >> 기본값 12
            //     FullSlow1Scan -> 15, 16, 17  >> 기본값 15
            //     FullSlow2Scan -> 15, 16, 17  >> 기본값 17
            //----------------------------------------
            // 초기스캔모드 : Half Fast Scan 설정 640x480, 1초당 7.5 프레임
            m_pIntegrated.nScanMode          = FAST_SCAN_MODE;
            //----------------------------------------
            // System 처음 시작시, 100배로
            m_pIntegrated.nMagStepIndex      = 10;  // mag x100
            //----------------------------------------
            m_pIntegrated.nFilamentValue     = 2008;
            m_pIntegrated.nBiasValue         = 2000;
            m_pIntegrated.nGunHVIndex        = 2;
            m_pIntegrated.nSpotSize          = 16;
            m_pIntegrated.nCLSpotSize        = 16;
            //----------------------------------------
            m_pIntegrated.nGAXValue          = 2048;
            m_pIntegrated.nGAYValue          = 2048;
            m_pIntegrated.nStigXValue        = 2048;
            m_pIntegrated.nStigYValue        = 2048;
            m_pIntegrated.nISXValue          = 2048;
            m_pIntegrated.nISYValue          = 2048;
            //----------------------------------------
            m_pIntegrated.nBrightValue       = 1660;
            m_pIntegrated.nBrightLowLimit    = 513;
            m_pIntegrated.nBrightHighLimit   = 2017;
            //----------------------------------------
            m_pIntegrated.nContrastValue     = 1822;
            m_pIntegrated.nContrastLowLimit  = 0;
            m_pIntegrated.nContrastHighLimit = 4095;
            //----------------------------------------
            m_pIntegrated.nBrightReference   = 1817;
            m_pIntegrated.nContrastReference = 1753;
            m_pIntegrated.nStigXReference    = 2388;
            m_pIntegrated.nStigYReference    = 2124;
            //----------------------------------------
            m_pIntegrated.nFocusCoarse       = 4095;
            m_pIntegrated.nFocusFine         = 0;
            //----------------------------------------
            m_pIntegrated.wAutoBrightLevel = (ushort)(127);     // (0 - 255) set by user
            m_pIntegrated.wAutoContrastLevel = (ushort)(127);     // (0 - 255) set by user
            //----------------------------------------

            //----------------------------------------
            //----------------------------------------
            //----------------------------------------

            m_pIntegrated.nXSR45Value = 0;
            m_pIntegrated.nYSR45Value = 0;
            m_pIntegrated.nXSR90Value = 0;
            m_pIntegrated.nYSR90Value = 0;

            //----------------------------------------
            //----------------------------------------
            //----------------------------------------

            //
            // WD에 따른 WD_OL_Coarse, WD_SRotation, WD_Zooming 값을 설정하는 방법
            //
            // 1) 처음에는  IniPar.WD_OL_Coarse[] 값을 Board에서 측정한 전압값을 발생시키는
            //    Cal_WD_OL_Value_From_SM300_OL_Voltage() 함수에서 만들려고 하였으나.
            //
            // 2) WD=5mm (IniPar.WD_TopLimit)에서 AutoFocus를 수행하고 얻은 OL값을 IniPar.WD_OL_Coarse[]에
            //    저장하고, 1mm씩 이동한 후 반복 시행을 WD=40mm(IniPar.WD_BottomLimit)까지 진행한다.
            //
            // 3) 이 때 얻은 IniPar.WD_OL_Coarse[] 값이 정확한지 WD 위치에서 확인하면서,
            //    IniPar.WD_SRotation[], IniPar.WD_Zooming[] 값을 저장한다.
            //
            // 4) 1mm 간격으로 측정값을 얻으므로, 분해능을 0.1mm 까지 얻기위해
            //    1mm 간격의 Data를 10등분으로 선형화 작업을 프로그램 시작시 진행한다.
            //
            AfxIntegratedSetWDCompensation();

            //-----------------------------------------------
            // CL1, CL2 전류를 출력시키기 위한 D/A 변환기 값...
            //-----------------------------------------------
            // m_dRefR, m_dAmpRk 값은 SEM 모델에 따라 다르다.
            // 기본은 SM-300B CL Type 모델, 값은 Ref:2.0, Amp:2.4
            //-----------------------------------------------
            double dFT = 0.0;

            for (int n=0; n<MAX_SM300_SM200_CL12_ITEM; n++)
            {
                switch (nSemType)
                {
                    case 0:
                    case 1:
                    {
                        //-------------------------------------------
                        // SM-300A 전류에 따른 12bit DA
                        //-------------------------------------------
                        // 3.9k / 3.3ohm = 1.18A (Max) 10V -> 4096 -> 3.9V
                        // 2.4k / 2.0ohm = 1.2A  (Max) 10V -> 4096 -> 2.4V
                        //-------------------------------------------
                        dFT = I_SM300_CL1[n] * m_pIntegrated.dRefR * 4096.0 / m_pIntegrated.dAmpRk;
                        m_nDA_CL1[n] = (int)dFT;
                        //-------------------------------------------
                        dFT = I_SM300_CL2[n] * m_pIntegrated.dRefR * 4096.0 / m_pIntegrated.dAmpRk;
                        m_nDA_CL2[n] = (int)dFT;
                        //-------------------------------------------
                        //m_dV_CL1[n] = I_SM300_CL1[n] * m_pIntegrated->dRefR;
                        //m_dV_CL2[n] = I_SM300_CL2[n] * m_pIntegrated->dRefR;
                        break;
                        //-------------------------------------------
                    }
                    case 2:
                    {
                        //-------------------------------------------
                        // SM-200A 전류에 따른 12bit DA 인가 Code로 변경
                        //-------------------------------------------
                        // 3k / 2ohm = 1.5A (Max)   10V -> 4096
                        //-------------------------------------------
                        // 4096 => 10V => 3V
                        dFT = I_SM200_CL1[n] * m_pIntegrated.dRefR * 4096.0 / m_pIntegrated.dAmpRk;
                        m_nDA_CL1[n] = (int)dFT;
                        //-------------------------------------------
                        dFT = I_SM200_CL2[n] * m_pIntegrated.dRefR * 4096.0 / m_pIntegrated.dAmpRk;
                        m_nDA_CL2[n] = (int)dFT;
                        //-------------------------------------------
                        //m_dV_CL1[n] = I_SM200_CL1[n] * m_pIntegrated->dRefR;
                        //m_dV_CL2[n] = I_SM200_CL2[n] * m_pIntegrated->dRefR;
                        break;
                        //-------------------------------------------
                    }
                    case 3:
                    case 4:
                    case 5:
                    {
                        //-------------------------------------------
                        // SM-300의 저항값으로 SM-200에서 Test할 경우
                        //-------------------------------------------
                        // 2.4k / 2.0 ohm = 1.2A  (Max) 10V -> 4096
                        //-------------------------------------------
                        // 4096 => 10V => 3V
                        dFT = I_SM200_CL1[n] * m_pIntegrated.dRefR * 4096.0 / m_pIntegrated.dAmpRk;
                        m_nDA_CL1[n] = (int)dFT;
                        if (m_nDA_CL1[n] > 4095)
                        {
                            m_nDA_CL1[n] = 4095;
                        }
                        //-------------------------------------------
                        dFT = I_SM200_CL2[n] * m_pIntegrated.dRefR * 4096.0 / m_pIntegrated.dAmpRk;
                        m_nDA_CL2[n] = (int)dFT;
                        if (m_nDA_CL2[n] > 4095)
                        {
                            m_nDA_CL2[n] = 4095;
                        }
                        //-------------------------------------------
                        //m_dV_CL1[n] = I_SM200_CL1[n] * m_pIntegrated->dRefR;
                        //m_dV_CL2[n] = I_SM200_CL2[n] * m_pIntegrated->dRefR;
                        //-------------------------------------------
                        break;
                    }
                    case 6:
                    {
                        //-------------------------------------------
                        // SM-200의 저항값으로 SM-300에서 Test할 경우
                        //-------------------------------------------
                        // 3k / 2.0 ohm = 1.5A     (Max) 10V -> 4096
                        //-------------------------------------------
                        // 4096 => 10V => 3V
                        dFT = I_SM300_CL1[n] * m_pIntegrated.dRefR * 4096.0 / m_pIntegrated.dAmpRk;
                        m_nDA_CL1[n] = (int)dFT;
                        if (m_nDA_CL1[n] > 4095)
                        {
                            m_nDA_CL1[n] = 4095;
                        }
                        //-------------------------------------------
                        dFT = I_SM300_CL2[n] * m_pIntegrated.dRefR * 4096.0 / m_pIntegrated.dAmpRk;
                        m_nDA_CL2[n] = (int)dFT;
                        if (m_nDA_CL2[n] > 4095)
                        {
                            m_nDA_CL2[n] = 4095;
                        }
                        //-------------------------------------------
                        //m_dV_CL1[n] = I_SM200_CL1[n] * m_pIntegrated->dRefR;
                        //m_dV_CL2[n] = I_SM200_CL2[n] * m_pIntegrated->dRefR;
                        //-------------------------------------------
                        break;
                    }
                }
            }
            //-
            // CL1, CL2값 보관
            for (int n = 0; n<MAX_SM300_SM200_CL12_ITEM; n++)
            {
                m_pIntegrated.wCL1List[n] = (ushort)m_nDA_CL1[n];
                m_pIntegrated.wCL2List[n] = (ushort)m_nDA_CL2[n];
            }
            //-
            m_pIntegrated.nWobblerWidth = 4;
            //-
            m_pIntegrated.nTiltZeroAnglePulsePos = 4000;
            //-
            // Step Motor (X,Y,Z,R,T) Micro Step Index -> (0=Full, 1=1/2, 2=1/4, 3=1/8)
            m_pIntegrated.nMicroStep[0] = 3;
            m_pIntegrated.nMicroStep[1] = 3;
            m_pIntegrated.nMicroStep[2] = 2;
            m_pIntegrated.nMicroStep[3] = 2;
            m_pIntegrated.nMicroStep[4] = 2;
            //-
            m_pIntegrated.nRightAngleStepIndex = 0; // 15 deg or 18 deg
            //-

            return 0;
        }

    }
}
