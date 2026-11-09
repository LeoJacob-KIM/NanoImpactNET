// 원본 USB_FT232H_Device.cpp:17198-19203 을 NanoImpact.CodeGen translate로 옮긴 뒤 손으로 고친 코드.
#pragma warning disable IDE1006, IDE0059, CS0162, CS0168, CS0219
using System;
using NanoImpact.Core.Native;
using static NanoImpact.Core.Def;
using static NanoImpact.Core.Native.CRuntime;

namespace NanoImpact.Hardware.Device
{
    public sealed partial class SemDevice
    {
        // ---- USB_FT232H_Device.cpp:17198 ----
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //


        //
        //
        //
        // BSE-Image Device Command Control...
        //
        //
        //

        //
        // BSE Control : BSE Image Device Initilized (BSE 시스템 초기화)
        //
        public int AfxBseImageInitilized()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 2021.07.23
            // 초기설정 : 초기값은 BSE 채널은 사용하지 않는다.
            m_pIntegrated.bBseActiveFlag = FALSE;

            // 2021.07.23 : 별도의 BSE 이미지 보드가 없기 때문에 아래 기능은 사용하지 않는다.
            /*
            // BSE Board가 연결 되지 않은 경우
            if (m_pIntegrated->bBseActiveFlag != TRUE) return RET_ERROR_FT232H_BSE_CONNECT;

            // BSE Offset AD5724 초기화

            // Power Control Register : Bse Command 전송 -> DAC0, DAC1, DAC2, DAC3 => Normal Mode
            AfxFT232HPortSendCommand(IDX_FT232H_BSE_IMAGE_DEVICE, bWR_AD5724, 0x10000F);
            // Output Range Select Register : Bse Command 전송 -> DAC A, +-10V
            AfxFT232HPortSendCommand(IDX_FT232H_BSE_IMAGE_DEVICE, bWR_AD5724, 0x080004);
            AfxFT232HPortSendCommand(IDX_FT232H_BSE_IMAGE_DEVICE, bWR_AD5724, 0x090004);
            AfxFT232HPortSendCommand(IDX_FT232H_BSE_IMAGE_DEVICE, bWR_AD5724, 0x0A0004);
            AfxFT232HPortSendCommand(IDX_FT232H_BSE_IMAGE_DEVICE, bWR_AD5724, 0x0B0004);
            */

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:17263 ----
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //


        //
        //
        //
        // SEM System Control...
        //
        //
        //


        //
        // System Control : SEM System Initilized (시스템 초기화, USB 장치 연결시 1번만 실행한다)
        //
        public int AfxSEMSystemInitilize()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            //============================
            // HV POWER ON/OFF FLAG : OFF 설정
            //============================
            m_pIntegrated.bGunHVPowerOnFlag      = FALSE;                  // GUN HV Power On Flag
            m_pIntegrated.bFilamentPowerOnFlag   = FALSE;                  // Filament HV Power On Flag
            m_pIntegrated.bBiasPowerOnFlag       = FALSE;                  // Bias HV Power On Flag
            m_pIntegrated.bPMTPowerOnFlag        = FALSE;                  // PMT HV Power On Flag
            m_pIntegrated.bDETPowerOnFlag        = FALSE;                  // Detector HV Power On Flag
            m_pIntegrated.bSM2500PowerOnFlag     = FALSE;                  // Hitachi SM-2500 HV Power On Flag
            m_pIntegrated.bContrastOnFlag        = FALSE;                  // HV 설정시 Contrast 처음 실행 Flag
            m_pIntegrated.bScanRotationOnFlag    = FALSE;                  // HV 설정시 Scan Rotation 처음 실행 Flag
            m_pIntegrated.bScreenModeOnFlag      = FALSE;                  // HV 설정시 Screen Mode 처음 실행 Flag
            m_pIntegrated.bHVUpEndProcessFlag    = FALSE;                  // HV ALL 실행시 Up   Ending 처음 실행 Flag
            m_pIntegrated.bHVDownEndProcessFlag  = FALSE;                  // HV ALL 실행시 Down Ending 처음 실행 Flag
            m_pIntegrated.nHVStandbyProcess      = 0;                      // HV Standby Mode Up/Down Process Status : 1->Standby, 0->Normal
            m_pIntegrated.nGunHVUpDownProcess    = 0;                      // Gun HV   Up/Down Process Status : 10->Start, 20->Process, 99->End
            m_pIntegrated.nFilamentUpDownProcess = 0;                      // Filament Up/Down Process Status : 10->Start, 20->Process, 99->End
            m_pIntegrated.nBiasUpDownProcess     = 0;                      // Bias     Up/Down Process Status : 10->Start, 20->Process, 99->End
            m_pIntegrated.bHVStartProcessFlag    = FALSE;                  // HV Start Process Execute Flag
            m_pIntegrated.bHVAllStartProcessFlag = FALSE;                  // HV All Start Process Execute Flag
            m_pIntegrated.bHVAllStopProcessFlag  = FALSE;                  // HV All Stop  Process Execute Flag
            //============================
            m_pIntegrated.nMagRelayOldNo         = 9;                      // Mag Relay No : 0 ~ 4, 배율 초기값을 실행 시키기 위해 Old Relay NO를 9로 설정
            m_pIntegrated.wMagRelayNumberOld = (ushort)(9);                      // Mag Relay No : 0 ~ 4, 배율 초기값을 실행 시키기 위해 Old Relay NO를 9로 설정
            //============================
            m_pIntegrated.nZoomXMaxValue[0]      = m_nZoomXMaxValue[0];
            m_pIntegrated.nZoomXMaxValue[1]      = m_nZoomXMaxValue[1];
            m_pIntegrated.nZoomXMaxValue[2]      = m_nZoomXMaxValue[2];
            m_pIntegrated.nZoomXMaxValue[3]      = m_nZoomXMaxValue[3];
            m_pIntegrated.nZoomXMaxValue[4]      = m_nZoomXMaxValue[4];
            m_pIntegrated.nZoomYMaxValue[0]      = m_nZoomYMaxValue[0];
            m_pIntegrated.nZoomYMaxValue[1]      = m_nZoomYMaxValue[1];
            m_pIntegrated.nZoomYMaxValue[2]      = m_nZoomYMaxValue[2];
            m_pIntegrated.nZoomYMaxValue[3]      = m_nZoomYMaxValue[3];
            m_pIntegrated.nZoomYMaxValue[4]      = m_nZoomYMaxValue[4];
            //============================
            m_pIntegrated.bWDHVCompensationFlag  = TRUE;                   // GUN HV에 따른 배율(Zoom) 보정은 필수 선택 사항이다
            m_pIntegrated.bWDScaleAdjustFlag     = TRUE;                   // WD    에 따른 배율(Zoom) 보정은 필수 선택 사항이다
            //============================
            m_pIntegrated.nOLCompensation        = OL_IMAGE_ZOOM_CHANGE;   // WD에 따른 배율(Zoom) 보정 방식
            //============================

            //============================
            //============================
            //============================

            // CL 모델 선택 : 작업 필요함
            double dAmp = m_pIntegrated.dSemModelAmp; // kohm
            double dRef = m_pIntegrated.dSemModelRef; // ohm
            //
            // CL1, CL2값 보관
            //
            for (int n = 0; n < MAX_SM300_SM200_CL12_ITEM; n++)
            {
                m_nDA_CL1[n] = (int)m_pIntegrated.wCL1List[n];
                m_nDA_CL2[n] = (int)m_pIntegrated.wCL2List[n];
            }
            //

            //
            // GUN VALVE CLOSE
            //
            AfxScanBridgeGunValveClose();
            //AfxSystemWaitTime(100);
            Sleep(100);
            //

            //
            // SEM SPELLMAN HV DISABLED
            //
            // SpellMan GUN HV OFF
            AfxScanBridgeSpellmanGunHVOff();
            //AfxSystemWaitTime(100);
            Sleep(100);
            // SpellMan PMT OFF
            AfxIntegratedSpellmanPMTHVOff();
            //AfxSystemWaitTime(100);
            Sleep(100);
            // SpellMan Scintillator OFF
            AfxIntegratedSpellmanScintilatorHVOff();
            //AfxSystemWaitTime(100);
            Sleep(100);
            // SpellMan Collector OFF
            AfxScanBridgeCollectorOff();
            //AfxSystemWaitTime(100);
            Sleep(100);
            //

            //
            // SE SCAN IMAGE CAPTURE OFF
            //
            AfxIntegratedScanImageDataCaptureStop();
            //

            //
            // SEM MODEL에 따른 CL값 초기화 (변수 초기화)
            //
            // 2019.11.01
            //AfxSemModelCLInitialize(m_pIntegrated->nSemModel, dAmp, dRef);
            //

            // Photo Mode Mag 초기값 설정
            memset(m_pIntegrated.nPhotoImageMag, 0x00, sizeof(int) * MAX_PHOTO_MAG_COUNT);
            m_pIntegrated.nPhotoImageNum = 10;
            // Full Photo Mode 초기값 설정
            memset(m_pIntegrated.nFullPhotoImageMag, 0x00, sizeof(int) * MAX_PHOTO_MAG_COUNT);
            m_pIntegrated.nFullPhotoImageNum = 10; 

            //
            // BSE 시스템 초기화
            //
            AfxBseImageInitilized();
            //

            //
            // Track Ball 초기화
            //
            m_pIntegrated.nXYMovingState    = 0;
            m_pIntegrated.nTrackButtonState = TB_MOTOR_BUTTON;
            //

            //
            // GUN HV Ratio 계산값
            //
            double dMaxGunHV = m_nGunHVTable[0];                           // MAX GUN HV VOLTAGE : m_nGunHVTable[0];
            double dGunHV    = m_nGunHVTable[m_pIntegrated.nGunHVIndex];  // Gun HV Voltage
            double dRatio    = sqrt(dGunHV / dMaxGunHV);                   // Gun HV Voltage / MAX GUN HV Voltage -> 비율
            // Emission current[µA] Setting
            ushort   wECVal    = 0;

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
                if (m_nGunHVTable[m_pIntegrated.nGunHVIndex] == 20000)
                {
                    m_pIntegrated.dMagHVRatio = sqrt(dGunHV / dMaxGunHV) / DEFAULT_MAG_HV_RATIO;
                }
                else
                {
                    // 사용자 설정 배율 HV RATIO 보정값 설정
                    m_pIntegrated.dMagHVRatio = m_dOptionMagHVRatio[m_pIntegrated.nGunHVIndex]; //sqrt(dGunHV / dMaxGunHV) / m_dOptionMagHVRatio[m_pIntegrated->nGunHVIndex];// OPTION_MAG_HV_RATIO;
                }
            }
            //

            // 변수 초기화
            m_pIntegrated.nXYPosCount  = 0;
            m_pIntegrated.nCurRDeg     = 0;
            m_pIntegrated.nSemVacState = POWER_OFF_STATE;

            //
            // Lens Coil에 흐르는 전류 최소화 ( USB DEVICE 실행 )
            //
            AfxIntegratedLensCoilCurrentMinimum();

            //
            // SCAN 신호 초기화 ( 신호 OFF )
            //
            AfxIntegratedResetScanGeneration();

            // Scan Mode 설정
            m_pIntegrated.nOldSlowScanMode = m_pIntegrated.nHalfSlow1Scan;

            //
            // Filament ( 변수초기화 )
            //
            m_pIntegrated.nFilamentOldValue = m_pIntegrated.nFilamentValue;
            AfxScanBridgeSetFilamentValue(m_pIntegrated.nFilamentValue);

            //
            // Bias ( 변수초기화)
            //
            m_pIntegrated.nBiasOldValue = m_pIntegrated.nBiasValue;
            AfxScanBridgeSetBiasValue(m_pIntegrated.nBiasValue);

            //
            // Contrast
            //-------------------------------------
            m_pIntegrated.nContrastOldValue = m_pIntegrated.nContrastValue;

            //
            // Bright
            //
            m_pIntegrated.nBrightOldValue = m_pIntegrated.nBrightValue;

            //
            // Stig X,Y
            //
            m_pIntegrated.nStigXOldValue = m_pIntegrated.nStigXValue;
            m_pIntegrated.nStigYOldValue = m_pIntegrated.nStigYValue;

            //
            // ImgShift X,Y : 클라이언트에서 초기화 설정 한다 ( 2019.08.12 )
            //
            m_pIntegrated.nISXOldValue = m_pIntegrated.nISXValue;
            m_pIntegrated.nISYOldValue = m_pIntegrated.nISYValue;
            //AfxIntegratedSetImageShiftX();
            //AfxIntegratedSetImageShiftY();

            //
            //
            //

            // WFM Line
            m_pIntegrated.nWFMLineNum = 240;
            // Main Scrollbar Update Enable Falg
            m_pIntegrated.bMainScrollBarUpdateEnable = TRUE;

            //
            //
            //

            // divide zero 방지
            if (m_pIntegrated.dAxisPulsePerUnit[0] == 0)  m_pIntegrated.dAxisPulsePerUnit[0] = 20000.0;
            if (m_pIntegrated.dAxisPulsePerUnit[1] == 0)  m_pIntegrated.dAxisPulsePerUnit[1] = 20000.0;
            if (m_pIntegrated.dAxisPulsePerUnit[2] == 0)  m_pIntegrated.dAxisPulsePerUnit[2] = 4000.0;
            if (m_pIntegrated.dAxisPulsePerUnit[3] == 0)  m_pIntegrated.dAxisPulsePerUnit[3] = 80000.0;
            if (m_pIntegrated.dAxisPulsePerUnit[4] == 0)  m_pIntegrated.dAxisPulsePerUnit[4] = 4000.0;

            if (m_pIntegrated.dAxisMaxWidthMM[0] == 0)  m_pIntegrated.dAxisMaxWidthMM[0] = 20.0;
            if (m_pIntegrated.dAxisMaxWidthMM[1] == 0)  m_pIntegrated.dAxisMaxWidthMM[1] = 20.0;
            if (m_pIntegrated.dAxisMaxWidthMM[2] == 0)  m_pIntegrated.dAxisMaxWidthMM[2] = 20.0;
            if (m_pIntegrated.dAxisMaxWidthMM[3] == 0)  m_pIntegrated.dAxisMaxWidthMM[3] = 20.0;
            if (m_pIntegrated.dAxisMaxWidthMM[4] == 0)  m_pIntegrated.dAxisMaxWidthMM[4] = 20.0;

            //
            // 실제 X, Y Stage 폭에 비례한 m_pBitmap_XYR 크기 결정
            //
            // AxisMaxWidthMM[] 값은 초기의 ini File에 저장된 값으로 표시함.
            // 즉, Motor Board의 EEProm에 저장된 값과 같아야 됨.
            //
            // 시료의 Bmp 위치
            m_pIntegrated.nXStagePos = 0;// 이전프로그램 -> m_pBitmap_XYR->Width / 2;
            m_pIntegrated.nYStagePos = 0;// 이전프로그램 -> m_pBitmap_XYR->Height / 2;

            // 시료의 Diameter
            m_pIntegrated.nSDpix = 0; //이전 프로그램 ->  m_pBitmap_XYR->Height * SDmm / AxisMaxWidthMM[1];

            // 시료의 Radius
            m_pIntegrated.nSRpix = m_pIntegrated.nSDpix / 2;

            //
            // 시료 위치
            //

            // 시료 Rotation Point 그리기
            m_pIntegrated.nRotAngLeft   = m_pIntegrated.nXStagePos - 6;
            m_pIntegrated.nRotAngRight  = m_pIntegrated.nXStagePos + 6;
            m_pIntegrated.nRotAngTop    = m_pIntegrated.nYStagePos - m_pIntegrated.nSRpix + 12 + 2;
            m_pIntegrated.nRotAngBottom = m_pIntegrated.nYStagePos - m_pIntegrated.nSRpix + 2;

            m_pIntegrated.nXBeamCenterPosPix = 0; // 이전프로그램 -> m_pBitmap_XYR->Width / 2;
            m_pIntegrated.nYBeamCenterPosPix = 0; // 이전프로그램 -> m_pBitmap_XYR->Height / 2;

            //
            //
            //

            //--
            // Scan Mode 설정
            //--
            // Freeze 설정
            m_pIntegrated.bFreezeModeFlag = FALSE; // FreezeButtonFlag = FALSE;


            //--
            // SEM ON 이 눌려질 때, SEM의 모든 초기과정을 설정하고,
            // 전에 종료되었던 ScreenMode 및 Scan 모드로 복귀하고 영상을 관찰한다.
            //--
            //--
            // 전에 종료되었던 ScreenMode 및 Scan 모드에 무관하게
            // 무조건 Normal Mode로 시작
            //--
            m_pIntegrated.nScreenMode = NORMAL_SCREEN;

            //------------------------------------------------
            // SCAN ROTATION 각도 초기 설정
            //------------------------------------------------
            // SRotationValue 값은 각도 단위 값이며, 0~3599(3600) 범위이다(분해능을 높이기 위해 본래 값에 x10을 사용하였다)
            m_pIntegrated.nSRotationValue = 0;
            AfxScanBridgeSetScanRotation(m_pIntegrated.nSRotationValue);

            //------------------------------------------------
            // 직교도...( 변수초기화 )
            //------------------------------------------------
            if (m_pIntegrated.nOrthogonal < 0   ) m_pIntegrated.nOrthogonal = 0;
            if (m_pIntegrated.nOrthogonal > 4095) m_pIntegrated.nOrthogonal = 4095;
            AfxScanBridgeSetOrthogonal(m_pIntegrated.nOrthogonal); // Orthogonal 설정 (m_pIntegrated->nARAngle[0]);
            AfxScanBridgeSetOrthogonal();                           // Orthogonal 실행 

            //
            //
            //
            //
            //
            //
            //
            //
            //

            //
            // 이전 프로그램 아래의 처리를 타이머를 이용하여 실행하였으나
            // 아래의 프로그램은 쓰레드로 처리하기 때문에 마지막 문장에서
            // 처리할 기능들을 실행한다. 타이머의 경우 Function의 루틴이
            // 종료 된 후, 이벤트가 발생하므로 쓰레드는 타이머와 다르게 처리한다.
            //

            //
            // Scan Bridge Device 상태 정보 읽기 쓰레드 생성 및 실행
            //
            // Scan Bridge 상태정보 읽기 쓰레드 생성하기
            AfxCreateScanBridgeThread();
            //
            //
            //

            //
            // SEM SE Scan Image Data 쓰레드 생성 및 실행
            //
            // SE Image Processing Thread 생성 및 시작 (SEM Image Scan 쓰레드 생성)
            AfxCreateSEImageDataThread();
            //
            //
            //

            //-------------------------------------
            // X, Y, Z, R, T Stage ( Read )
            //-------------------------------------
            // CNC 명령어 전송: 각 축에 설정된 설정값 및 상태정보를 읽어온다. 쓰레드를 생성한다
            m_pIntegrated.nCncSendTimerCount = 0;

            //
            // Scan Bridge -> Vacuum State ( Read ) 실행
            //
            //AfxScanBridgeGetVaccumState();
            //AfxSystemWaitTime(100);
            //


            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:17619 ----
        public int AfxSEMSystemInitilizeHVOff()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // HV Power On Flag
            m_pIntegrated.bGunHVPowerOnFlag      = FALSE;     // GUN HV Power On Flag
            m_pIntegrated.bFilamentPowerOnFlag   = FALSE;     // Filament HV Power On Flag
            m_pIntegrated.bBiasPowerOnFlag       = FALSE;     // Bias HV Power On Flag
            m_pIntegrated.bPMTPowerOnFlag        = FALSE;     // PMT HV Power On Flag
            m_pIntegrated.bDETPowerOnFlag        = FALSE;     // Detector HV Power On Flag
            m_pIntegrated.bSM2500PowerOnFlag     = FALSE;     // Hitachi SM-2500 HV Power On Flag
            m_pIntegrated.bContrastOnFlag        = FALSE;     // HV 설정시 Contrast 처음 실행 Flag
            m_pIntegrated.bScanRotationOnFlag    = FALSE;     // HV 설정시 Scan Rotation 처음 실행 Flag
            m_pIntegrated.bScreenModeOnFlag      = FALSE;     // HV 설정시 Screen Mode 처음 실행 Flag
            m_pIntegrated.bHVUpEndProcessFlag    = FALSE;     // HV ALL 실행시 Up   Ending 처음 실행 Flag
            m_pIntegrated.bHVDownEndProcessFlag  = FALSE;     // HV ALL 실행시 Down Ending 처음 실행 Flag
            m_pIntegrated.nHVStandbyProcess      = 0;         // HV Standby Mode Up/Down Process Status : 1->Standby, 0->Normal
            m_pIntegrated.nGunHVUpDownProcess    = 0;         // Gun HV   Up/Down Process Status : 10->Start, 20->Process, 99->End
            m_pIntegrated.nFilamentUpDownProcess = 0;         // Filament Up/Down Process Status : 10->Start, 20->Process, 99->End
            m_pIntegrated.nBiasUpDownProcess     = 0;         // Bias     Up/Down Process Status : 10->Start, 20->Process, 99->End
            m_pIntegrated.bHVStartProcessFlag    = FALSE;     // HV Start Process Execute Flag
            m_pIntegrated.bHVAllStartProcessFlag = FALSE;     // HV All Start Process Execute Flag
            m_pIntegrated.bHVAllStopProcessFlag  = FALSE;     // HV All Stop  Process Execute Flag

            //
            // GUN VALVE CLOSE
            //
            AfxScanBridgeGunValveClose();
            //AfxSystemWaitTime(100);
            //

            //
            // SEM SPELLMAN HV DISABLED
            //
            // SpellMan GUN HV OFF
            AfxScanBridgeSpellmanGunHVOff();
            //AfxSystemWaitTime(100);
            // SpellMan PMT OFF
            AfxIntegratedSpellmanPMTHVOff();
            //AfxSystemWaitTime(100);
            // SpellMan Scintillator OFF
            AfxIntegratedSpellmanScintilatorHVOff();
            //AfxSystemWaitTime(100);
            // SpellMan Collector OFF
            AfxScanBridgeCollectorOff();
            //AfxSystemWaitTime(100);
            //

            //
            // Scan Bridge -> Vacuum State ( Read ) 실행
            //
            //AfxScanBridgeGetVaccumState();
            //AfxSystemWaitTime(100);
            //

            //
            //
            //
            //
            //
            //
            //
            //
            //
            /*
            //
            // 이전 프로그램 아래의 처리를 타이머를 이용하여 실행하였으나
            // 아래의 프로그램은 쓰레드로 처리하기 때문에 마지막 문장에서
            // 처리할 기능들을 실행한다. 타이머의 경우 Function의 루틴이
            // 종료 된 후, 이벤트가 발생하므로 쓰레드는 타이머와 다르게 처리한다.
            //

            //
            // Scan Bridge Device 상태 정보 읽기 쓰레드 생성 및 실행
            //
            // Scan Bridge 상태정보 읽기 쓰레드 생성하기
            AfxCreateScanBridgeThread();
            //
            //
            //

            //
            // SEM SE Scan Image Data 쓰레드 생성 및 실행
            //
            // SE Image Processing Thread 생성 및 시작 (SEM Image Scan 쓰레드 생성)
            AfxCreateSEImageDataThread();
            //
            //
            //

            //-------------------------------------
            // X, Y, Z, R, T Stage ( Read )
            //-------------------------------------
            // CNC 명령어 전송: 각 축에 설정된 설정값 및 상태정보를 읽어온다. 쓰레드를 생성한다
            m_pIntegrated->nCncSendTimerCount = 0;
            */

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:17723 ----
        //
        // System Control : SEM CL Model Initilized (CL MODEL 시스템 초기화)
        //
        public int AfxSemModelCLInitialize(int nSemModel, double dAmp, double dRef)
        {
            // SEM Model 초기화 및 사용 변수 초기화
            AfxIntegratedInitializeParameters(nSemModel);

            m_pIntegrated.dAmpRk = dAmp;
            m_pIntegrated.dRefR  = dRef;
            strcpy_s(m_pIntegrated.szUserText, "ModuleSci ");

            /*
            m_pIntegrated->nZoomXMaxValue[0]    = 4095;
            m_pIntegrated->nZoomYMaxValue[0]    = 4095;
            m_pIntegrated->nZoomXMaxValue[1]    = 4095;
            m_pIntegrated->nZoomYMaxValue[1]    = 4095;
            m_pIntegrated->nZoomXMaxValue[2]    = 4095;
            m_pIntegrated->nZoomYMaxValue[2]    = 4095;
            m_pIntegrated->nZoomXMaxValue[3]    = 4095;
            m_pIntegrated->nZoomYMaxValue[3]    = 4095;
            m_pIntegrated->nZoomXMaxValue[4]    = 4095;
            m_pIntegrated->nZoomYMaxValue[4]    = 4095;

            m_pIntegrated->nWDOLCoarse[0]       = 4095;
            m_pIntegrated->nWDOLCoarse[1]       = 4092;
            m_pIntegrated->nWDOLCoarse[2]       = 4087;
            m_pIntegrated->nWDOLCoarse[3]       = 4085;
            m_pIntegrated->nWDOLCoarse[4]       = 4080;
            */

            if (m_pIntegrated.nWDMaxValue == 0) m_pIntegrated.nWDMaxValue = 50;

            AfxIntegratedSetWDOLSrotZoomingLinearize();

            m_pIntegrated.nXScanMoveLeft       = -654;
            m_pIntegrated.nXScanMoveRight      = 700;
            m_pIntegrated.nYScanMoveTop        = -538;
            m_pIntegrated.nYScanMoveBottom     = 404;

            m_pIntegrated.nDImagXResetPos      = 20;
            m_pIntegrated.nDImagYResetPos      = -40;

            m_pIntegrated.nDMXMoveLeft         = 1125;  // 1392;
            m_pIntegrated.nDMXMoveRight        = 3040;  // 3025;
            m_pIntegrated.nDMYMoveTop          = 1245;
            m_pIntegrated.nDMYMoveBottom       = 2864;

            m_pIntegrated.bFirstClearBitmap    = TRUE;
            m_pIntegrated.bFirstBMouseDown     = FALSE;
            m_pIntegrated.bMagMouseWheelActive = TRUE;
            m_pIntegrated.nMainScrollBarMode   = 0;

            m_pIntegrated.dCurZPos             = 15;

            //
            // 대기 상태의 Menu 상태값 표시
            //
            // 1) 프로그램 시작시, 과거에 설정되었던 값을 windows에 표시하고,
            // 2) 사용자가 설정하고 싶은 값으로 변경하고, Start Radio button을 누르면
            // 3) 현재 설정값으로, SEM 조정을 하고, Image 관찰을 시작한다.
            //

            //
            // Mag에 관련된 설정부
            //
            m_pIntegrated.nWDZoomingRatio    = 1;
            m_pIntegrated.wMagRelayNumber = (ushort)(0);      // nMagRelayNo = 0;
            m_pIntegrated.wMagRelayNumberOld = (ushort)(999);    // nMagRelayOldNo = 999;  // 같으면 초기진입시 Relay가 Off됨
            m_pIntegrated.bMainScrollBarUpdateEnable = FALSE;

            //
            // GHV Setting...
            //
            double dHVMax = (double)m_nGunHVTable[0];
            m_pIntegrated.nGunHVValue = m_nGunHVTable[m_pIntegrated.nGunHVIndex];
            // Emission current[µA] Setting
            ushort   wECVal = 0;
            // Emission current[µA] 계산
            m_pIntegrated.dHVRatio = sqrt((double)m_pIntegrated.nGunHVValue / dHVMax);
            wECVal = (ushort)((double)(4095 * m_pIntegrated.dHVRatio));
            wECVal /= 2;
            // Emission current[µA] 실행
            AfxIntegratedSetGunHVECValue(wECVal);

            m_pIntegrated.dMagHVRatio = sqrt((double)m_pIntegrated.nGunHVValue / dHVMax) / DEFAULT_MAG_HV_RATIO; //30000.0) / 0.8165;
            // 2020.06.03
            // 사용자 MAG HV RATIO : STAGE 수평 옵션을 임시로 사용한다.
            if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_STAGE_HORZ_ALIGN] == 1)
            {
                // GUN HV : 20KV
                if (m_nGunHVTable[m_pIntegrated.nGunHVIndex] == 20000)
                {
                    m_pIntegrated.dMagHVRatio = sqrt((double)m_pIntegrated.nGunHVValue / dHVMax) / DEFAULT_MAG_HV_RATIO;
                }
                else
                {
                    // 사용자 설정 배율 HV RATIO 보정값 설정
                    m_pIntegrated.dMagHVRatio = m_dOptionMagHVRatio[m_pIntegrated.nGunHVIndex]; //sqrt((double)m_pIntegrated->nGunHVValue / dHVMax) / m_dOptionMagHVRatio[m_pIntegrated->nGunHVIndex];// OPTION_MAG_HV_RATIO;
                }
            }

            //
            // SpotSize Setting... (변수 초기화)
            //
            AfxIntegratedSetSpotSize();

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:17833 ----
        //
        // System Control : SEM CL Model Initilized (CL MODEL 시스템 초기화)
        //
        public int AfxSEMSystemThreadClose()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            int bLoop = TRUE;

            //
            // 사용중인 쓰레드 종료
            //
            for (int n = 0; n < MAX_PUBLIC_THREAD; n++)
            {
                object pThread = m_pPublicToolsThread[n];         // Public Tools 쓰레드 핸들
                PUBLICTOOLSPARAMS pParam = m_pPublicToolsParams[n];  // Array Public Tools 쓰레드 사용변수

                if (pThread == null) continue;
                if (pParam == null) continue;

                // 쓰레드 종료
                pParam.bThreadStop = TRUE;
                bLoop = TRUE;

                // 쓰레드가 종료 될 때 까지 기다린다.
                // 잠시대기
                Sleep(100);
            }

            //
            //
            //

            //
            // FOCUS DEGAUSSING 쓰레드 사용 중이면...
            //
            // 2021.06.15
            if (m_pFocusDegaussingThread != null)
            {
                // FOCUS DEGAUSSING 쓰레드 파라메터 중복 확인
                if (m_pFocusDegaussingThreadParams != null)
                {
                    // FOCUS DEGAUSSING 쓰레드 종료 설정
                    m_pFocusDegaussingThreadParams.bThreadStop = TRUE;

                    bLoop = TRUE;

                    // 쓰레드가 종료 될 때 까지 기다린다.
                    // 잠시대기
                    Sleep(100);

                    // FOCUS DEGAUSSING 쓰레드에서 사용한 파라메터 삭제

                    m_pFocusDegaussingThreadParams = null;
                }
            }
            //
            //
            //

            //
            // CNC STAGE 쓰레드 사용 중이면...
            //
            // 2021.05.31
            if (m_pCNCSendThread != null || m_pCNCSendThreadParams != null)
            {
                // SE Image Data 쓰레드 파라메터 중복 확인
                if (m_pCNCSendThreadParams != null)
                {
                    // SE Image Data 쓰레드 종료 설정
                    m_pCNCSendThreadParams.bThreadStop = TRUE;

                    bLoop = TRUE;

                    // 쓰레드가 종료 될 때 까지 기다린다.
                    // 잠시대기
                    Sleep(100);

                    // CNC STAGE 쓰레드에서 사용한 파라메터 삭제

                    m_pCNCSendThreadParams = null;
                }
            }
            //
            //
            //

            //
            // SE Scan Image Data 쓰레드 사용 중이면...
            //
            if (m_pSEImageDataThread != null)
            {
                // SE Image Data 쓰레드 파라메터 중복 확인
                if (m_pSEImageDataThreadParams != null)
                {
                    // SE Image Data 쓰레드 종료 설정
                    m_pSEImageDataThreadParams.bThreadStop = TRUE;
                    m_pIntegrated.bScanThreadStop = TRUE;

                    bLoop = TRUE;

                    // 쓰레드가 종료 될 때 까지 기다린다.
                    // 잠시대기
                    Sleep(100);

                    // SE Image Data 쓰레드에서 사용한 파라메터 삭제

                    m_pSEImageDataThreadParams = null;
                }
            }
            //
            //
            //

            //
            // Scan Bridge 쓰레드 사용 중이면...
            //
            if (m_pScanBridgeThread != null)
            {
                // Scan Bridge 쓰레드 파라메터 중복 확인
                if (m_pScanBridgeThreadParams != null)
                {
                    // Scan Bridge 쓰레드 종료
                    m_pScanBridgeThreadParams.bThreadStop = TRUE;

                    bLoop = TRUE;

                    // 쓰레드가 종료 될 때 까지 기다린다.
                    // 잠시 대기
                    Sleep(100);

                    // Scan Bridge 쓰레드에서 사용한 파라메터 삭제

                    m_pScanBridgeThreadParams = null;
                }
            }
            //
            //
            //


            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:17977 ----
        //
        // Integrated Control : SEM Gun HV Index 설정
        //
        public int AfxSetSEMGunHvIndex(int nIndex = 0, int bReset = FALSE)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 사용자가 선택한 GUN HV Index -> 현재 작업중인 GUN HV Index로 설정
            m_pIntegrated.nGunHVIndex = nIndex;

            //
            // GUN HV Ratio 계산값
            //
            double dMaxGunHV = m_nGunHVTable[0];                           // MAX GUN HV VOLTAGE : m_nGunHVTable[0];
            double dGunHV    = m_nGunHVTable[m_pIntegrated.nGunHVIndex];  // Gun HV Voltage
            double dRatio    = sqrt(dGunHV / dMaxGunHV);                   // Gun HV Voltage / MAX GUN HV Voltage -> 비율

            // 2021.08.23
            /*
            // Emission current[µA] Setting
            WORD wECVal = 0;
            // Emission current[µA] 계산
            m_pIntegrated->dHVRatio = dRatio;
            wECVal = (WORD)((double)(4095 * m_pIntegrated->dHVRatio));
            wECVal /= 2;
            // Emission current[µA] 실행
            AfxIntegratedSetGunHVECValue(wECVal);
            */

            // GUN HV 배율 비율 계산
            m_pIntegrated.dMagHVRatio = sqrt(dGunHV / dMaxGunHV) / DEFAULT_MAG_HV_RATIO;
            // 2020.06.03
            // 사용자 MAG HV RATIO : STAGE 수평 옵션을 임시로 사용한다.
            if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_STAGE_HORZ_ALIGN] == 1)
            {
                // GUN HV : 20KV
                if (m_nGunHVTable[m_pIntegrated.nGunHVIndex] == 20000)
                {
                    m_pIntegrated.dMagHVRatio = sqrt(dGunHV / dMaxGunHV) / DEFAULT_MAG_HV_RATIO;
                }
                else
                {
                    // 사용자 설정 배율 HV RATIO 보정값 설정
                    m_pIntegrated.dMagHVRatio = m_dOptionMagHVRatio[m_pIntegrated.nGunHVIndex]; // sqrt(dGunHV / dMaxGunHV) / m_dOptionMagHVRatio[m_pIntegrated->nGunHVIndex];// OPTION_MAG_HV_RATIO;
                }
            }
            //

            if (bReset == TRUE)
            {
                m_pIntegrated.bGunHVResetFlag    = TRUE; // GUN HV Reset Flag
                m_pIntegrated.nGunHVCurIndex     = -1;   // 사용자가 선택한 GUN HV 인덱스
                m_pIntegrated.nGunHVExeIndex     = -1;   // GUN HV 실행 인덱스
                m_pIntegrated.nGunHVValue        = 0;    // GUN HV 실행 인덱스에 해당하는 GUN HV 값
                m_pIntegrated.nGunHVCurValue     = 0;    // GUN HV 현재 설정값
                m_pIntegrated.nGunHVExeValue     = 0;    // GUN HV 현재 실행값
                m_pIntegrated.dGunHVCurVoltage   = 0.0;  // GUN HV 현재 설정된 Voltage
                m_pIntegrated.dGunHVUpVoltage    = 0.0;  // GUN HV 현재 설정된 Up Voltage
                m_pIntegrated.dGunHVDownVoltage  = 0.0;  // GUN HV 현재 설정된 Down Voltage
                m_pIntegrated.dGunHVStartVoltage = 0.0;  // GUN HV 현재 설정된 Start Voltage
                m_pIntegrated.dGunHVStopVoltage  = 0.0;  // GUN HV 현재 설정된 Stop  Voltage
                m_pIntegrated.dGunHVStepVoltage  = 0.0;  // GUN HV 현재 설정된 Step  Voltage
                m_pIntegrated.dGunHVMaxVoltage   = 0.0;  // GUN HV 현재 설정된 Max   Voltage
                m_pIntegrated.dGunHVMinVoltage   = 0.0;  // GUN HV 현재 설정된 Min   Voltage
                m_pIntegrated.dGunHVBackupVoltage= 0.0;  // GUN HV 현재 설정된 Voltage Backup
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:18047 ----
        //
        // Integrated Control : Get SEM Gun HV Voltage
        //
        public int AfxGetSEMGunHvIndex()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 현재 설정 및 샐행된 GUN HV Voltage Index 값
            return m_pIntegrated.nGunHVIndex;
        }

        // ---- USB_FT232H_Device.cpp:18058 ----
        //
        // Integrated Control : Get SEM Gun HV Voltage
        //
        public double AfxGetSEMGunHvVoltage()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 현재 설정 및 샐행된 GUN HV Voltage 값
            return m_pIntegrated.dGunHVCurVoltage;
        }

        // ---- USB_FT232H_Device.cpp:18069 ----
        //
        // Integrated Control : Get SEM Gun HV Backup Current Voltage
        //
        public double AfxGetSEMGunHvBackupVoltage()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 현재 설정 및 샐행된 GUN HV Backup Current Voltage 값
            return m_pIntegrated.dGunHVBackupVoltage;
        }

        // ---- USB_FT232H_Device.cpp:18080 ----
        //
        // Integrated Control : SEM Filament Voltage 설정 & Reset
        //
        public int AfxSetSEMFilamentValue(int nTarget = 0, int bReset = FALSE)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 사용자가 선택한 Filament Voltage Position
            m_pIntegrated.nFilamentValue = nTarget;
            if (bReset == TRUE)
            {
                m_pIntegrated.bFilamentResetFlag = TRUE;     // Filament Reset Flag
                m_pIntegrated.nFilamentCurValue  = -1;       // Filament 현재 설정값
                m_pIntegrated.nFilamentExeValue  = -1;       // Filament 실행 설정값
                m_pIntegrated.nFilamentValue     = nTarget;  // 사용자가 선택한 Filament 설정값
                m_pIntegrated.dFilamentCurVoltage    = 0.0;  // Filament 현재 설정된 Voltage
                m_pIntegrated.dFilamentUpVoltage     = 0.0;  // Filament 현재 설정된 Up Voltage
                m_pIntegrated.dFilamentDownVoltage   = 0.0;  // Filament 현재 설정된 Down Voltage
                m_pIntegrated.dFilamentStartVoltage  = 0.0;  // Filament 현재 설정된 Start Voltage
                m_pIntegrated.dFilamentStopVoltage   = 0.0;  // Filament 현재 설정된 Stop  Voltage
                m_pIntegrated.dFilamentStepVoltage   = 0.0;  // Filament 현재 설정된 Step  Voltage
                m_pIntegrated.dFilamentMaxVoltage    = 0.0;  // Filament 현재 설정된 Max   Voltage
                m_pIntegrated.dFilamentMinVoltage    = 0.0;  // Filament 현재 설정된 Min   Voltage
                m_pIntegrated.dFilamentBackupVoltage = 0.0;  // Filament 현재 설정된 Voltage Backup
            }

            /*
            // 전송자료
            USB_DEVICE_DATA_INFO UsbDeviceDataInfo;
            memset(&UsbDeviceDataInfo, 0x00, sizeof(USB_DEVICE_DATA_INFO));
            // Filament
            UsbDeviceDataInfo.nState = m_pIntegrated->nFilamentValue;

            if (m_hOwnerWnd != NULL)
            {
                // DEVICE MAIN SERVER
                // 현재 Filament Voltage Position (변경된 Filament Voltage 선택 값) 정보 메시지 보내기
                SendMessage(m_hOwnerWnd, UM_USB_SBR_FILAMENT_DATA, NULL, (LPARAM)&UsbDeviceDataInfo);
            }
            */

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:18124 ----
        //
        // Integrated Control : Get SEM Filament Voltage
        //
        public double AfxGetSEMFilamentVoltage()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 현재 설정 및 샐행된 Filament Voltage 값
            return m_pIntegrated.dFilamentCurVoltage;
        }

        // ---- USB_FT232H_Device.cpp:18135 ----
        //
        // Integrated Control : Get SEM Gun HV Backup Current Voltage
        //
        public double AfxGetSEMFilamentBackupVoltage()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 현재 설정 및 샐행된 Filament Backup Current Voltage 값
            return m_pIntegrated.dFilamentBackupVoltage;
        }

        // ---- USB_FT232H_Device.cpp:18146 ----
        //
        // Integrated Control : Get SEM Filament Start Voltage
        //
        public double AfxGetSEMFilamentStartVoltage()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 현재 설정 및 샐행된 Filament Start Voltage 값
            return m_pIntegrated.dFilamentStartVoltage;
        }

        // ---- USB_FT232H_Device.cpp:18157 ----
        //
        // Integrated Control : Get SEM Filament Stop Voltage
        //
        public double AfxGetSEMFilamentStopVoltage()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 현재 설정 및 샐행된 Filament Stop Voltage 값
            return m_pIntegrated.dFilamentStopVoltage;
        }

        // ---- USB_FT232H_Device.cpp:18168 ----
        //
        // Integrated Control : SEM Bias Voltage 설정 & Reset
        //
        public int AfxSetSEMBiasValue(int nTarget = 0, int bReset = FALSE)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 사용자가 선택한 Bias Voltage Position
            m_pIntegrated.nBiasValue = nTarget;
            if (bReset == TRUE)
            {
                m_pIntegrated.bBiasResetFlag = TRUE;     // Bias Reset Flag
                m_pIntegrated.nBiasCurValue  = -1;       // Bias 현재 설정값
                m_pIntegrated.nBiasExeValue  = -1;       // Bias 실행 설정값
                m_pIntegrated.nBiasValue     = nTarget;  // 사용자가 선택한 Bias 설정값
                m_pIntegrated.dBiasCurVoltage    = 0.0;  // Bias 현재 설정된 Voltage
                m_pIntegrated.dBiasUpVoltage     = 0.0;  // Bias 현재 설정된 Up Voltage
                m_pIntegrated.dBiasDownVoltage   = 0.0;  // Bias 현재 설정된 Down Voltage
                m_pIntegrated.dBiasStartVoltage  = 0.0;  // Bias 현재 설정된 Start Voltage
                m_pIntegrated.dBiasStopVoltage   = 0.0;  // Bias 현재 설정된 Stop  Voltage
                m_pIntegrated.dBiasStepVoltage   = 0.0;  // Bias 현재 설정된 Step  Voltage
                m_pIntegrated.dBiasMaxVoltage    = 0.0;  // Bias 현재 설정된 Max   Voltage
                m_pIntegrated.dBiasMinVoltage    = 0.0;  // Bias 현재 설정된 Min   Voltage
                m_pIntegrated.dBiasBackupVoltage = 0.0;  // Bias 현재 설정된 Voltage Backup
            }

            /*
            // 전송자료
            USB_DEVICE_DATA_INFO UsbDeviceDataInfo;
            memset(&UsbDeviceDataInfo, 0x00, sizeof(USB_DEVICE_DATA_INFO));
            // Bias
            UsbDeviceDataInfo.nState = m_pIntegrated->nBiasValue;

            if (m_hOwnerWnd != NULL)
            {
                // DEVICE MAIN SERVER
                // 현재 Bias Voltage Position (변경된 Bias Voltage 선택 값) 정보 메시지 보내기
                SendMessage(m_hOwnerWnd, UM_USB_SBR_BIAS_DATA, NULL, (LPARAM)&UsbDeviceDataInfo);
            }
            */

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:18212 ----
        //
        // Integrated Control : Get SEM Bias Voltage
        //
        public double AfxGetSEMBiasVoltage()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 현재 설정 및 샐행된 Bias Voltage 값
            return m_pIntegrated.dBiasCurVoltage;
        }

        // ---- USB_FT232H_Device.cpp:18223 ----
        //
        // Integrated Control : Get SEM Bias Backup Current Voltage
        //
        public double AfxGetSEMBiasBackupVoltage()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 현재 설정 및 샐행된 Bias Backup Current Voltage 값
            return m_pIntegrated.dBiasBackupVoltage;
        }

        // ---- USB_FT232H_Device.cpp:18234 ----
        //
        // Integrated Control : Get SEM Bias Start Voltage
        //
        public double AfxGetSEMBiasStartVoltage()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 현재 설정 및 샐행된 Bias Start Voltage 값
            return m_pIntegrated.dBiasStartVoltage;
        }

        // ---- USB_FT232H_Device.cpp:18245 ----
        //
        // Integrated Control : Get SEM Bias Stop Voltage
        //
        public double AfxGetSEMBiasStopVoltage()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 현재 설정 및 샐행된 Bias Stop Voltage 값
            return m_pIntegrated.dBiasStopVoltage;
        }

        // ---- USB_FT232H_Device.cpp:18256 ----
        //
        // Integrated Control : SEM Magnification 설정
        //
        public int AfxSetSEMMagnificationIndex(int nIndex)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 사용자가 선택한 Magnification Index -> 현재 작업중인 Index로 설정
            m_pIntegrated.nMagStepIndex = nIndex;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:18265 ----
        public int AfxGetSEMMagnificationIndex()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 사용자가 선택한 Magnification Index -> 현재 작업중인 Index로 설정
            return m_pIntegrated.nMagStepIndex;
        }

        // ---- USB_FT232H_Device.cpp:18276 ----
        //
        // Integrated Control : SEM OL WD 설정
        //
        public int AfxSetSEMOLWDValue(int nOLWD)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 사용자가 선택한 OL_WD 값
            m_pIntegrated.nOLWD = nOLWD;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:18289 ----
        //
        // Integrated Control : SEM Focus Coarse 설정
        //
        public int AfxSetSEMFocusCoarse(int nCoarse)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 사용자가 선택한 Focus-Coarse 값 -> 현재 작업중인 값으로 설정
            m_pIntegrated.nFocusCoarse = nCoarse;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:18298 ----
        public int AfxGetSEMFocusCoarse()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Focus-Coarse 값
            return m_pIntegrated.nFocusCoarse;
        }

        // ---- USB_FT232H_Device.cpp:18309 ----
        //
        // Integrated Control : SEM Focus Fine 설정
        //
        public int AfxSetSEMFocusFine(int nFine)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 사용자가 선택한 Focus-Fine 값 -> 현재 작업중인 값으로 설정
            m_pIntegrated.nFocusFine = nFine;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:18318 ----
        public int AfxGetSEMFocusFine()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Focus-Fine 값
            return m_pIntegrated.nFocusFine;
        }

        // ---- USB_FT232H_Device.cpp:18329 ----
        //
        // Integrated Control : SEM Emission Current 300[ms] 단위로 자동 받기
        //
        public int AfxGetSEMEmissionCurrent()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Emission Current를 자동닫기 위한 명령어 : PC -> Avr
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "AE3;");

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:18342 ----
        //
        // Integrated Control : SEM Gun Hv Up/Down Time Delay 변수 설정
        //
        public int AfxSetSEMGunHVTimeDelayData(int bTimeDelay, double dUpVoltage, double dDownVoltage, double dMaxVoltage, double dMinVoltage, double dCurVoltage, double dStartVoltage, double dStopVoltage, double dStepVoltage)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // GUN HV Up/Down 시 Time Delay 사용여부 설정
            m_pIntegrated.bGunHVTimeDelay    = bTimeDelay;    // 사용여부 설정
            m_pIntegrated.dGunHVUpVoltage    = dUpVoltage;    // Up   Voltage : [V]/[SEC]
            m_pIntegrated.dGunHVDownVoltage  = dDownVoltage;  // Down Voltage : [V]/[SEC] <- Down시에는 Up 일때보다 빠르게 전압을 내린다 
            m_pIntegrated.dGunHVMaxVoltage   = dMaxVoltage;   // Up/Down Max     Voltage : [V]
            m_pIntegrated.dGunHVMinVoltage   = dMinVoltage;   // Up/Down Min     Voltage : [V]
            m_pIntegrated.dGunHVCurVoltage   = dCurVoltage;   // Up/Down Current Voltage : [V]
            m_pIntegrated.dGunHVStartVoltage = dStartVoltage; // Up/Down Start   Voltage : [V]
            m_pIntegrated.dGunHVStopVoltage  = dStopVoltage;  // Up/Down Stop    Voltage : [V]
            m_pIntegrated.dGunHVStepVoltage  = dStepVoltage;  // Up/Down Step    Voltage : [V]

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:18363 ----
        //
        // Integrated Control : SEM Filament Up/Down Time Delay 변수 설정
        //
        public int AfxSetSEMFilamentTimeDelayData(int bTimeDelay, double dUpVoltage, double dDownVoltage, double dMaxVoltage, double dMinVoltage, double dCurVoltage, double dStartVoltage, double dStopVoltage, double dStepVoltage)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Filament Up/Down 시 Time Delay 사용여부 설정
            m_pIntegrated.bFilamentTimeDelay    = bTimeDelay;    // 사용여부 설정
            m_pIntegrated.dFilamentUpVoltage    = dUpVoltage;    // Up   Voltage : [V]/[SEC]
            m_pIntegrated.dFilamentDownVoltage  = dDownVoltage;  // Down Voltage : [V]/[SEC] <- Down시에는 Up 일때보다 빠르게 전압을 내린다 
            m_pIntegrated.dFilamentMaxVoltage   = dMaxVoltage;   // Up/Down Max     Voltage : [V]
            m_pIntegrated.dFilamentMinVoltage   = dMinVoltage;   // Up/Down Min     Voltage : [V]
            m_pIntegrated.dFilamentCurVoltage   = dCurVoltage;   // Up/Down Current Voltage : [V]
            m_pIntegrated.dFilamentStartVoltage = dStartVoltage; // Up/Down Start   Voltage : [V]
            m_pIntegrated.dFilamentStopVoltage  = dStopVoltage;  // Up/Down Stop    Voltage : [V]
            m_pIntegrated.dFilamentStepVoltage  = dStepVoltage;  // Up/Down Step    Voltage : [V]

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:18384 ----
        //
        // Integrated Control : SEM Bias Up/Down Time Delay 변수 설정
        //
        public int AfxSetSEMBiasTimeDelayData(int bTimeDelay, double dUpVoltage, double dDownVoltage, double dMaxVoltage, double dMinVoltage, double dCurVoltage, double dStartVoltage, double dStopVoltage, double dStepVoltage)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Bias Up/Down 시 Time Delay 사용여부 설정
            m_pIntegrated.bBiasTimeDelay    = bTimeDelay;    // 사용여부 설정
            m_pIntegrated.dBiasUpVoltage    = dUpVoltage;    // Up   Voltage : [V]/[SEC]
            m_pIntegrated.dBiasDownVoltage  = dDownVoltage;  // Down Voltage : [V]/[SEC] <- Down시에는 Up 일때보다 빠르게 전압을 내린다 
            m_pIntegrated.dBiasMaxVoltage   = dMaxVoltage;   // Up/Down Max     Voltage : [V]
            m_pIntegrated.dBiasMinVoltage   = dMinVoltage;   // Up/Down Min     Voltage : [V]
            m_pIntegrated.dBiasCurVoltage   = dCurVoltage;   // Up/Down Current Voltage : [V]
            m_pIntegrated.dBiasStartVoltage = dStartVoltage; // Up/Down Start   Voltage : [V]
            m_pIntegrated.dBiasStopVoltage  = dStopVoltage;  // Up/Down Stop    Voltage : [V]
            m_pIntegrated.dBiasStepVoltage  = dStepVoltage;  // Up/Down Step    Voltage : [V]

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:18405 ----
        //
        // Integrated Control : Get SEM HV ALL 쓰레드 Process Flag
        //
        public int AfxGetSEMHVAllUpDownProcessFlag()
        {
            if (m_pIntegrated == null) return FALSE;

            // HV ALL Process 쓰레드 상태 
            // TRUE : 쓰레드 사용중, FALSE : 쓰레드 종료
            return m_pIntegrated.bHVAllStartProcessFlag;
        }

        // ---- USB_FT232H_Device.cpp:18417 ----
        //
        // Integrated Control : Get Stig-X Low Limit 값 
        //
        public int AfxGetSEMStigXLowLimit()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Stig-X Low Limit
            return m_pIntegrated.nStigXLowLimit;
        }

        // ---- USB_FT232H_Device.cpp:18427 ----
        //
        // Integrated Control : Set Stig-X Low Limit 값 
        //
        public int AfxSetSEMStigXLowLimit(int nLimit)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Stig-X Low Limit
            m_pIntegrated.nStigXLowLimit = nLimit;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:18440 ----
        //
        // Integrated Control : Get Stig-X High Limit 값 
        //
        public int AfxGetSEMStigXHighLimit()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Stig-X High Limit
            return m_pIntegrated.nStigXHighLimit;
        }

        // ---- USB_FT232H_Device.cpp:18450 ----
        //
        // Integrated Control : Set Stig-X High Limit 값 
        //
        public int AfxSetSEMStigXHighLimit(int nLimit)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Stig-X High Limit
            m_pIntegrated.nStigXHighLimit = nLimit;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:18463 ----
        //
        // Integrated Control : Get Stig-Y Low Limit 값 
        //
        public int AfxGetSEMStigYLowLimit()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Stig-Y Low Limit
            return m_pIntegrated.nStigYLowLimit;
        }

        // ---- USB_FT232H_Device.cpp:18473 ----
        //
        // Integrated Control : Set Stig-Y Low Limit 값 
        //
        public int AfxSetSEMStigYLowLimit(int nLimit)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Stig-Y Low Limit
            m_pIntegrated.nStigYLowLimit = nLimit;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:18486 ----
        //
        // Integrated Control : Get Stig-Y High Limit 값 
        //
        public int AfxGetSEMStigYHighLimit()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Stig-Y High Limit
            return m_pIntegrated.nStigXHighLimit;
        }

        // ---- USB_FT232H_Device.cpp:18496 ----
        //
        // Integrated Control : Set Stig-Y High Limit 값 
        //
        public int AfxSetSEMStigYHighLimit(int nLimit)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Stig-Y High Limit
            m_pIntegrated.nStigYHighLimit = nLimit;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:18509 ----
        //
        // Integrated Control : Get Stig-X Reference 값 
        //
        public int AfxGetSEMStigXReference()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Stig X
            //m_pIntegrated->nStigXReference = m_pIntegrated->nStigXValue;

            // Stig-X Reference
            return m_pIntegrated.nStigXReference;
        }

        // ---- USB_FT232H_Device.cpp:18519 ----
        public int AfxGetSEMStigXValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Stig X
            return m_pIntegrated.nStigXValue;
        }

        // ---- USB_FT232H_Device.cpp:18529 ----
        //
        // Integrated Control : Set Stig-X Reference 값 
        //
        public int AfxSetSEMStigXReference(int nRef)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Stig-X Reference
            m_pIntegrated.nStigXReference = nRef;

            // Stig X
            //m_pIntegrated->nStigXValue = m_pIntegrated->nStigXReference;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:18545 ----
        //
        // Integrated Control : Get Stig-Y Reference 값 
        //
        public int AfxGetSEMStigYReference()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Stig Y
            //m_pIntegrated->nStigYReference = m_pIntegrated->nStigYValue;

            // Stig-Y Reference
            return m_pIntegrated.nStigYReference;
        }

        // ---- USB_FT232H_Device.cpp:18555 ----
        public int AfxGetSEMStigYValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Stig Y
            return m_pIntegrated.nStigYValue;
        }

        // ---- USB_FT232H_Device.cpp:18565 ----
        //
        // Integrated Control : Set Stig-Y Reference 값 
        //
        public int AfxSetSEMStigYReference(int nRef)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Stig-Y Reference
            m_pIntegrated.nStigYReference = nRef;

            // Stig Y
            //m_pIntegrated->nStigYValue = m_pIntegrated->nStigYReference;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:18581 ----
        //
        // Integrated Control : Get Zoom X-Max 값 
        //
        public int AfxGetSEMZoomXMax()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Zoom X-Max
            return m_pIntegrated.nZoomXMaxValue[m_pIntegrated.wMagRelayNumber];// nMagRelayNo];
        }

        // ---- USB_FT232H_Device.cpp:18592 ----
        //
        // Integrated Control : Get Zoom Y-Max 값 
        //
        public int AfxGetSEMZoomYMax()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Zoom Y-Max
            return m_pIntegrated.nZoomYMaxValue[m_pIntegrated.wMagRelayNumber];// nMagRelayNo];
        }

        // ---- USB_FT232H_Device.cpp:18603 ----
        //
        // Integrated Control : Get Zoom X-Value 값 
        //
        public int AfxGetSEMZoomXValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Zoom X-Value
            return m_pIntegrated.wZoomXValue;
        }

        // ---- USB_FT232H_Device.cpp:18614 ----
        //
        // Integrated Control : Get Zoom Y-Value 값 
        //
        public int AfxGetSEMZoomYValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Zoom Y-Value
            return m_pIntegrated.wZoomYValue;
        }

        // ---- USB_FT232H_Device.cpp:18625 ----
        //
        // Integrated Control : Scan Mode 에 따른 X Image Amp
        //
        public int AfxGetSEMXScanAmp()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // X Scan Amp
            return m_pIntegrated.nXScanAmpVal[m_pIntegrated.nScanMode];
        }

        // ---- USB_FT232H_Device.cpp:18636 ----
        //
        // Integrated Control : Scan Mode 에 따른 X Image Offset
        //
        public int AfxGetSEMXScanOffset()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // X Scan Offset
            return m_pIntegrated.nXScanOffset[m_pIntegrated.nScanMode];
        }

        // ---- USB_FT232H_Device.cpp:18647 ----
        //
        // Integrated Control : Scan Mode 에 따른 Y Image Amp
        //
        public int AfxGetSEMYScanAmp()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Y Scan Amp
            return m_pIntegrated.nYScanAmpVal[m_pIntegrated.nScanMode];
        }

        // ---- USB_FT232H_Device.cpp:18658 ----
        //
        // Integrated Control : Scan Mode 에 따른 Y Image Offset
        //
        public int AfxGetSEMYScanOffset()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Y Scan Offset
            return m_pIntegrated.nYScanOffset[m_pIntegrated.nScanMode];
        }

        // ---- USB_FT232H_Device.cpp:18669 ----
        //
        // Integrated Control : Spot Size에 따른 CL1 값
        //
        public int AfxGetSEMSpotSizeCL1()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Spot Size에 따른 CL1
            return (int)m_pIntegrated.wCL1List[m_pIntegrated.nSpotSize - 1];
        }

        // ---- USB_FT232H_Device.cpp:18680 ----
        //
        // Integrated Control : Spot Size에 따른 CL2 값
        //
        public int AfxGetSEMSpotSizeCL2()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Spot Size에 따른 CL2
            return (int)m_pIntegrated.wCL2List[m_pIntegrated.nSpotSize - 1];
        }

        // ---- USB_FT232H_Device.cpp:18691 ----
        //
        // Integrated Control : Scan Mode에 따른 Skip Pixel 갯수 값
        //
        public int AfxGetSEMSkipPixelCount()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Scan Mode에 따른 Skip Pixel 갯수 값
            return m_pIntegrated.nSkipPixNum[m_pIntegrated.nScanMode];
        }

        // ---- USB_FT232H_Device.cpp:18702 ----
        //
        // Integrated Control : Scan Mode에 따른 Over Sample 갯수 값
        //
        public int AfxGetSEMOverSampleCount()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Scan Mode에 따른 Over Sample 갯수 값
            return m_pIntegrated.nOverSampleNum[m_pIntegrated.nScanMode];
        }

        // ---- USB_FT232H_Device.cpp:18713 ----
        //
        // Integrated Control : Scan Mode에 따른 Frame / [Sec] 1초당 프레임 갯수 값
        //
        public int AfxGetSEMFrameFreqCount()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Scan Mode에 따른 Frame / [sec] 갯수 값
            return m_pIntegrated.nFrameSec[m_pIntegrated.nScanMode];
        }

        // ---- USB_FT232H_Device.cpp:18724 ----
        //
        // Integrated Control : Scan Mode에 따른 X Scan Up 값
        //
        public int AfxGetSEMXScanUpValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Scan Mode에 따른 X Scan Up 값
            return m_pIntegrated.nXScanUpNum[m_pIntegrated.nScanMode];
        }

        // ---- USB_FT232H_Device.cpp:18735 ----
        //
        // Integrated Control : Scan Mode에 따른 Y Scan Up 값
        //
        public int AfxGetSEMYScanUpValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Scan Mode에 따른 Y Scan Up 값
            return m_pIntegrated.nYScanUpNum[m_pIntegrated.nScanMode];
        }

        // ---- USB_FT232H_Device.cpp:18746 ----
        //
        // Integrated Control : Scan Mode에 따른 X Scan Down 값
        //
        public int AfxGetSEMXScanDownValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Scan Mode에 따른 X Scan Down 값
            return m_pIntegrated.nXScanDownNum[m_pIntegrated.nScanMode];
        }

        // ---- USB_FT232H_Device.cpp:18757 ----
        //
        // Integrated Control : Scan Mode에 따른 Y Scan Down 값
        //
        public int AfxGetSEMYScanDownValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Scan Mode에 따른 Y Scan Down 값
            return m_pIntegrated.nYScanDownNum[m_pIntegrated.nScanMode];
        }

        // ---- USB_FT232H_Device.cpp:18768 ----
        //
        // Integrated Control : Scan Mode에 따른 X Scan Rotation 45 Deg 값
        //
        public int AfxGetSEMXScanRot45DegValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // X Scan Rotation 45 Deg 값
            return m_pIntegrated.nXScanRot45Deg[m_pIntegrated.nScanMode];
        }

        // ---- USB_FT232H_Device.cpp:18775 ----
        public int AfxSetSEMXScanRot45DegValue(int nValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // X Scan Rotation 45 Deg 값
            m_pIntegrated.nXScanRot45Deg[m_pIntegrated.nScanMode] = nValue;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:18788 ----
        //
        // Integrated Control : Scan Mode에 따른 X Scan Rotation 90 Deg 값
        //
        public int AfxGetSEMXScanRot90DegValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // X Scan Rotation 90 Deg 값
            return m_pIntegrated.nXScanRot90Deg[m_pIntegrated.nScanMode];
        }

        // ---- USB_FT232H_Device.cpp:18795 ----
        public int AfxSetSEMXScanRot90DegValue(int nValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // X Scan Rotation 90 Deg 값
            m_pIntegrated.nXScanRot90Deg[m_pIntegrated.nScanMode] = nValue;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:18808 ----
        //
        // Integrated Control : Scan Mode에 따른 Y Scan Rotation 45 Deg 값
        //
        public int AfxGetSEMYScanRot45DegValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Y Scan Rotation 45 Deg 값
            return m_pIntegrated.nYScanRot45Deg[m_pIntegrated.nScanMode];
        }

        // ---- USB_FT232H_Device.cpp:18815 ----
        public int AfxSetSEMYScanRot45DegValue(int nValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Y Scan Rotation 45 Deg 값
            m_pIntegrated.nYScanRot45Deg[m_pIntegrated.nScanMode] = nValue;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:18828 ----
        //
        // Integrated Control : Scan Mode에 따른 Y Scan Rotation 90 Deg 값
        //
        public int AfxGetSEMYScanRot90DegValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Y Scan Rotation 90 Deg 값
            return m_pIntegrated.nYScanRot90Deg[m_pIntegrated.nScanMode];
        }

        // ---- USB_FT232H_Device.cpp:18835 ----
        public int AfxSetSEMYScanRot90DegValue(int nValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Y Scan Rotation 90 Deg 값
            m_pIntegrated.nYScanRot90Deg[m_pIntegrated.nScanMode] = nValue;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:18848 ----
        //
        // Integrated Control : RED Scan Mode의 Filer Factor 값
        //
        public int AfxGetSEMREDScanFilterFactorValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // RED Scan 의 Filter Factor 값

            if (m_pIntegrated.nScanMode == FULL_SCREEN)
            {
                return m_pIntegrated.nFilterNum[m_pIntegrated.nFullRedScan];
            }

            return m_pIntegrated.nFilterNum[m_pIntegrated.nHalfRedScan];
        }

        // ---- USB_FT232H_Device.cpp:18861 ----
        public int AfxSetSEMREDScanFilterFactorValue(int nValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            if (m_pIntegrated.bChangeEnableFlag == FALSE) return RET_SUCCESS_OK;

            // RED Scan 의 Filter Factor 값

            if (m_pIntegrated.nScreenMode == FULL_SCREEN)
            {
                m_pIntegrated.nFilterNum[m_pIntegrated.nFullRedScan] = nValue;
                m_pIntegrated.nSeCommandValue &= SEL_FILT_MASK;
                m_pIntegrated.nSeCommandValue |= m_wFilter[m_pIntegrated.nFilterNum[m_pIntegrated.nFullRedScan]];
            }
            else
            {
                m_pIntegrated.nFilterNum[m_pIntegrated.nHalfRedScan] = nValue;
                m_pIntegrated.nSeCommandValue &= SEL_FILT_MASK;
                m_pIntegrated.nSeCommandValue |= m_wFilter[m_pIntegrated.nFilterNum[m_pIntegrated.nHalfRedScan]];
            }

            // Se Command 전송
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_OUTPORT, m_pIntegrated.nSeCommandValue);

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:18891 ----
        //
        // Integrated Control : FAST Scan Mode의 Filer Factor 값
        //
        public int AfxGetSEMFASTScanFilterFactorValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // FAST Scan 의 Filter Factor 값

            if (m_pIntegrated.nScanMode == FULL_SCREEN)
            {
                return m_pIntegrated.nFilterNum[m_pIntegrated.nFullFastScan];
            }

            return m_pIntegrated.nFilterNum[m_pIntegrated.nHalfFastScan];
        }

        // ---- USB_FT232H_Device.cpp:18904 ----
        public int AfxSetSEMFASTScanFilterFactorValue(int nValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            if (m_pIntegrated.bChangeEnableFlag == FALSE) return RET_SUCCESS_OK;

            // FAST Scan 의 Filter Factor 값

            if (m_pIntegrated.nScreenMode == FULL_SCREEN)
            {
                m_pIntegrated.nFilterNum[m_pIntegrated.nFullFastScan] = nValue;
                m_pIntegrated.nSeCommandValue &= SEL_FILT_MASK;
                m_pIntegrated.nSeCommandValue |= m_wFilter[m_pIntegrated.nFilterNum[m_pIntegrated.nFullFastScan]];
            }
            else
            {
                m_pIntegrated.nFilterNum[m_pIntegrated.nHalfFastScan] = nValue;
                m_pIntegrated.nSeCommandValue &= SEL_FILT_MASK;
                m_pIntegrated.nSeCommandValue |= m_wFilter[m_pIntegrated.nFilterNum[m_pIntegrated.nHalfFastScan]];
            }

            // Se Command 전송
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_OUTPORT, m_pIntegrated.nSeCommandValue);

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:18934 ----
        //
        // Integrated Control : SLOW1 Scan Mode의 Filer Factor 값
        //
        public int AfxGetSEMSLOW1ScanFilterFactorValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // SLOW1 Scan 의 Filter Factor 값

            if (m_pIntegrated.nScanMode == FULL_SCREEN)
            {
                return m_pIntegrated.nFilterNum[m_pIntegrated.nFullSlow1Scan];
            }

            return m_pIntegrated.nFilterNum[m_pIntegrated.nHalfSlow1Scan];
        }

        // ---- USB_FT232H_Device.cpp:18947 ----
        public int AfxSetSEMSLOW1ScanFilterFactorValue(int nValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            if (m_pIntegrated.bChangeEnableFlag == FALSE) return RET_SUCCESS_OK;

            // SLOW1 Scan 의 Filter Factor 값

            if (m_pIntegrated.nScreenMode == FULL_SCREEN)
            {
                m_pIntegrated.nFilterNum[m_pIntegrated.nFullSlow1Scan] = nValue;
                m_pIntegrated.nSeCommandValue &= SEL_FILT_MASK;
                m_pIntegrated.nSeCommandValue |= m_wFilter[m_pIntegrated.nFilterNum[m_pIntegrated.nFullSlow1Scan]];
            }
            else
            {
                m_pIntegrated.nFilterNum[m_pIntegrated.nHalfSlow1Scan] = nValue;
                m_pIntegrated.nSeCommandValue &= SEL_FILT_MASK;
                m_pIntegrated.nSeCommandValue |= m_wFilter[m_pIntegrated.nFilterNum[m_pIntegrated.nHalfSlow1Scan]];
            }

            // Se Command 전송
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_OUTPORT, m_pIntegrated.nSeCommandValue);

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:18977 ----
        //
        // Integrated Control : SLOW2 Scan Mode의 Filer Factor 값
        //
        public int AfxGetSEMSLOW2ScanFilterFactorValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // SLOW2 Scan 의 Filter Factor 값

            if (m_pIntegrated.nScanMode == FULL_SCREEN)
            {
                return m_pIntegrated.nFilterNum[m_pIntegrated.nFullSlow2Scan];
            }

            return m_pIntegrated.nFilterNum[m_pIntegrated.nHalfSlow2Scan];
        }

        // ---- USB_FT232H_Device.cpp:18990 ----
        public int AfxSetSEMSLOW2ScanFilterFactorValue(int nValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            if (m_pIntegrated.bChangeEnableFlag == FALSE) return RET_SUCCESS_OK;

            // SLOW2 Scan 의 Filter Factor 값

            if (m_pIntegrated.nScreenMode == FULL_SCREEN)
            {
                m_pIntegrated.nFilterNum[m_pIntegrated.nFullSlow2Scan] = nValue;
                m_pIntegrated.nSeCommandValue &= SEL_FILT_MASK;
                m_pIntegrated.nSeCommandValue |= m_wFilter[m_pIntegrated.nFilterNum[m_pIntegrated.nFullSlow2Scan]];
            }
            else
            {
                m_pIntegrated.nFilterNum[m_pIntegrated.nHalfSlow2Scan] = nValue;
                m_pIntegrated.nSeCommandValue &= SEL_FILT_MASK;
                m_pIntegrated.nSeCommandValue |= m_wFilter[m_pIntegrated.nFilterNum[m_pIntegrated.nHalfSlow2Scan]];
            }

            // Se Command 전송
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_OUTPORT, m_pIntegrated.nSeCommandValue);

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:19020 ----
        //
        // Integrated Control : PHOTO Scan Mode의 Filer Factor 값
        //
        public int AfxGetSEMPHOTOScanFilterFactorValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // PHOTO Scan 의 Filter Factor 값
            return m_pIntegrated.nFilterNum[m_pIntegrated.nPhotoScan];
        }

        // ---- USB_FT232H_Device.cpp:19027 ----
        public int AfxSetSEMPHOTOScanFilterFactorValue(int nValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            if (m_pIntegrated.bChangeEnableFlag == FALSE) return RET_SUCCESS_OK;

            // PHOTO Scan 의 Filter Factor 값

            m_pIntegrated.nFilterNum[m_pIntegrated.nPhotoScan] = nValue;
            m_pIntegrated.nSeCommandValue &= SEL_FILT_MASK;
            m_pIntegrated.nSeCommandValue |= m_wFilter[m_pIntegrated.nFilterNum[m_pIntegrated.nPhotoScan]];

            // Se Command 전송
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_OUTPORT, m_pIntegrated.nSeCommandValue);

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:19048 ----
        //
        // Integrated Control : WD에 따른 보정(Correction) -> WD Max 값
        //
        public int AfxGetSEMCorrectionWDMaxValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            //int  nWDMaxValue;      // Integrated : WD의 단계(아이템 항목) 최대값
            // WD에 따른 보정(Correction) -> WD Max 값
            return m_pIntegrated.nWDMaxValue;
        }

        // ---- USB_FT232H_Device.cpp:19056 ----
        public int AfxSetSEMCorrectionWDMaxValue(int nValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // WD에 따른 보정(Correction) -> WD Max 값
            m_pIntegrated.nWDMaxValue = nValue;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:19069 ----
        //
        // Integrated Control : WD에 따른 보정(Correction) -> WD Z-Axis 값
        //
        public int AfxGetSEMOLWDx10Value()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // WD의 현재 위치값
            // OL에 따른 WD x10 위치값
            return m_pIntegrated.nOLWD;
        }

        // ---- USB_FT232H_Device.cpp:19077 ----
        public int AfxGetSEMCorrectionWDZAxisValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            //int  nWDPosition;   // Integrated : WD의 현재 위치값
            // WD에 따른 보정(Correction) -> WD Z-Axis 값
            return m_pIntegrated.nWDPosition;
        }

        // ---- USB_FT232H_Device.cpp:19085 ----
        public int AfxSetSEMCorrectionWDZAxisValue(int nWD)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // WD에 따른 보정(Correction) -> WD Z-Axis 값
            m_pIntegrated.nWDPosition = nWD;

            if (m_pIntegrated.nWDPosition < SEM_WD_CALI_POS_MIN) m_pIntegrated.nWDPosition = SEM_WD_CALI_POS_MIN;
            if (m_pIntegrated.nWDPosition > SEM_WD_CALI_POS_MAX) m_pIntegrated.nWDPosition = SEM_WD_CALI_POS_MAX;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:19101 ----
        //
        // Integrated Control : WD에 따른 보정(Correction) -> WD OLC 값
        //
        public int AfxGetSEMCorrectionWDOLCValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;
            if (m_pIntegrated.nWDPosition < SEM_WD_CALI_POS_MIN) m_pIntegrated.nWDPosition = SEM_WD_CALI_POS_MIN;
            if (m_pIntegrated.nWDPosition > SEM_WD_CALI_POS_MAX) m_pIntegrated.nWDPosition = SEM_WD_CALI_POS_MAX;

            // WD에 따른 보정(Correction) -> WD OLC 값
            return m_pIntegrated.nWDOLCoarse[m_pIntegrated.nWDPosition];
        }

        // ---- USB_FT232H_Device.cpp:19110 ----
        public int AfxSetSEMCorrectionWDOLCValue(int nValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;
            if (m_pIntegrated.nWDPosition < SEM_WD_CALI_POS_MIN) m_pIntegrated.nWDPosition = SEM_WD_CALI_POS_MIN;
            if (m_pIntegrated.nWDPosition > SEM_WD_CALI_POS_MAX) m_pIntegrated.nWDPosition = SEM_WD_CALI_POS_MAX;

            // WD에 따른 보정(Correction) -> WD OLC 값
            m_pIntegrated.nWDOLCoarse[m_pIntegrated.nWDPosition] = nValue;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:19125 ----
        //
        // Integrated Control : WD에 따른 보정(Correction) -> WD S-Rot 값
        //
        public int AfxGetSEMCorrectionWDSRotation()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;
            if (m_pIntegrated.nWDPosition < SEM_WD_CALI_POS_MIN) m_pIntegrated.nWDPosition = SEM_WD_CALI_POS_MIN;
            if (m_pIntegrated.nWDPosition > SEM_WD_CALI_POS_MAX) m_pIntegrated.nWDPosition = SEM_WD_CALI_POS_MAX;

            // WD에 따른 보정(Correction) -> WD S-Rot 값
            return m_pIntegrated.nWDSRotation[m_pIntegrated.nWDPosition];
        }

        // ---- USB_FT232H_Device.cpp:19136 ----
        public int AfxSetSEMCorrectionWDSRotation(int nValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;
            if (m_pIntegrated.nWDPosition < SEM_WD_CALI_POS_MIN) m_pIntegrated.nWDPosition = SEM_WD_CALI_POS_MIN;
            if (m_pIntegrated.nWDPosition > SEM_WD_CALI_POS_MAX) m_pIntegrated.nWDPosition = SEM_WD_CALI_POS_MAX;

            // WD에 따른 보정(Correction) -> WD S-Rot 값
            m_pIntegrated.nWDSRotation[m_pIntegrated.nWDPosition] = nValue;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:19151 ----
        //
        // Integrated Control : 현재 Scan Rotation 값
        //
        public int AfxGetSEMSRotationValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 현재 S-Rotation 값
            return m_pIntegrated.nSRotationValue;
        }

        // ---- USB_FT232H_Device.cpp:19158 ----
        public int AfxSetSEMSRotationValue(int nValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 현재 S-Rotation 값
            m_pIntegrated.nSRotationValue = nValue;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:19171 ----
        //
        // Integrated Control : WD에 따른 보정(Correction) -> WD Zoom 값
        //
        public int AfxGetSEMCorrectionWDZoomValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;
            if (m_pIntegrated.nWDPosition < SEM_WD_CALI_POS_MIN) m_pIntegrated.nWDPosition = SEM_WD_CALI_POS_MIN;
            if (m_pIntegrated.nWDPosition > SEM_WD_CALI_POS_MAX) m_pIntegrated.nWDPosition = SEM_WD_CALI_POS_MAX;

            // WD에 따른 보정(Correction) -> WD Zoom 값
            return m_pIntegrated.nWDZooming[m_pIntegrated.nWDPosition];
        }

        // ---- USB_FT232H_Device.cpp:19180 ----
        public int AfxSetSEMCorrectionWDZoomValue(int nValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;
            if (m_pIntegrated.nWDPosition < SEM_WD_CALI_POS_MIN) m_pIntegrated.nWDPosition = SEM_WD_CALI_POS_MIN;
            if (m_pIntegrated.nWDPosition > SEM_WD_CALI_POS_MAX) m_pIntegrated.nWDPosition = SEM_WD_CALI_POS_MAX;

            // WD에 따른 보정(Correction) -> WD Zoom 값
            m_pIntegrated.nWDZooming[m_pIntegrated.nWDPosition] = nValue;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:19195 ----
        //
        // Integrated Control : WD에 따른 보정(Correction) -> Min Mag 값
        //
        public int AfxGetSEMCorrectionWDMinMagValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;
            if (m_pIntegrated.nWDPosition < SEM_WD_CALI_POS_MIN) m_pIntegrated.nWDPosition = SEM_WD_CALI_POS_MIN;
            if (m_pIntegrated.nWDPosition > SEM_WD_CALI_POS_MAX) m_pIntegrated.nWDPosition = SEM_WD_CALI_POS_MAX;

            // WD에 따른 보정(Correction) -> Min Mag 값
            return m_pIntegrated.nWDMinMag[m_pIntegrated.nWDPosition];
        }

    }
}
