// 원본 ModularAssistDevice.cpp:2082-3110 을 NanoImpact.CodeGen translate로 옮긴 뒤 손으로 고친 코드.
#pragma warning disable IDE1006, IDE0059, CS0162, CS0168, CS0219
using System;
using NanoImpact.Core.Native;
using static NanoImpact.Core.Def;
using static NanoImpact.Core.Native.CRuntime;

namespace NanoImpact.Hardware.Device
{
    public sealed partial class AssistDevice
    {
        // ---- ModularAssistDevice.cpp:2082 ----
        public void SetUsbDeviceSemControlGunHVOnOff(int nGunHV)
        {
            // SET USB DEVICE : GUN HV ON/OFF 실행
            // USB DEVICE 적용
            // GUN HV
            if (nGunHV == SPELLMAN_GUN_FIL_BIAS_ENABLED)
            {
                m_dev.AfxScanBridgeSpellmanGunHVOn();          // GUN/FIL/BIAS HV ENABLED
            }
            else
            {
                m_dev.AfxScanBridgeSpellmanGunHVOff();         // GUN/FIL/BIAS HV DISABLED
            }
        }

        // ---- ModularAssistDevice.cpp:2098 ----
        public void SetUsbDeviceSemControlPMTOnOff(int nPMT)
        {
            // SET USB DEVICE : PMT HV ON/OFF 실행
            // USB DEVICE 적용
            // PMT
            if (nPMT == SPELLMAN_PMT_ENABLED)
            {
                m_dev.AfxIntegratedSpellmanPMTHVOn();          // PMT  HV ENABLED
            }
            else
            {
                m_dev.AfxIntegratedSpellmanPMTHVOff();         // PMT  HV DISABLED
            }
        }

        // ---- ModularAssistDevice.cpp:2114 ----
        public void SetUsbDeviceSemControlSCINOnOff(int nSCIN)
        {
            // SET USB DEVICE : SCIN HV ON/OFF 실행
            // USB DEVICE 적용
            // SCINTILATOR
            if (nSCIN == SPELLMAN_SCINTILATOR_ENABLED)
            {
                m_dev.AfxIntegratedSpellmanScintilatorHVOn();  // SCIN HV ENABLED
            }
            else
            {
                m_dev.AfxIntegratedSpellmanScintilatorHVOff(); // SCIN HV DISABLED
            }
        }

        // ---- ModularAssistDevice.cpp:2130 ----
        public void SetUsbDeviceSemControlCOLLOnOff(int nCOLL)
        {
            // SET USB DEVICE : COLLECTOR HV ON/OFF 실행
            // USB DEVICE 적용
            // COLLECTOR
            if (nCOLL == SPELLMAN_COLLECTOR_ENABLED)
            {
                m_dev.AfxScanBridgeCollectorOn();              // COLLECTOR HV ENABLED
            }
            else
            {
                m_dev.AfxScanBridgeCollectorOff();             // COLLECTOR HV DISABLED
            }
        }

        // ---- ModularAssistDevice.cpp:2146 ----
        public void SetUsbDeviceSemControlCL1CL2Value(int nSpot)
        {
            // SET USB DEVICE : SEM VACUUM STATE 설정
            // USB DEVICE 적용
            // Spot Size
            int nSpotSize = nSpot;// +1;
            // Spot Size에 따른 CL1, CL2
            int nCL1 = m_dev.AfxIntegratedGetCL1(nSpotSize);
            int nCL2 = m_dev.AfxIntegratedGetCL2(nSpotSize);
            // CL1, CL2 설정
            m_dev.AfxIntegratedSetCL1(nCL1);
            m_dev.AfxIntegratedSetCL2(nCL2);
            // CL1, CL2 적용
            m_dev.AfxIntegratedSetCL1();
            m_dev.AfxIntegratedSetCL2();
        }

        // ---- ModularAssistDevice.cpp:2164 ----
        public void SetUsbDeviceSemControlGunAlignValue(int nGAX, int nGAY, int nExecute)
        {
            // SET USB DEVICE : GUN ALIGN XY 설정
            // USB DEVICE 적용
            // GUN ALIGN XY 적용
            m_dev.AfxIntegratedSetGunAlignX(nGAX);
            m_dev.AfxIntegratedSetGunAlignY(nGAY);
            if (nExecute == 1)
            {
                // GUN ALIGN XY 실행
                m_dev.AfxIntegratedSetGunAlignX();
                m_dev.AfxIntegratedSetGunAlignY();
            }
        }

        // ---- ModularAssistDevice.cpp:2180 ----
        public void SetUsbDeviceSemControlBeamAlignValue(int nBAX, int nBAY, int nExecute)
        {
            // SET USB DEVICE : BEAM ALIGN XY 설정
            // USB DEVICE 적용
            // BEAM ALIGN XY 적용
            m_dev.AfxIntegratedSetBeamAlignX(nBAX);
            m_dev.AfxIntegratedSetBeamAlignY(nBAY);
            if (nExecute == 1)
            {
                // BEAM ALIGN XY 실행
                m_dev.AfxIntegratedSetBeamAlignX();
                m_dev.AfxIntegratedSetBeamAlignY();
            }
        }

        // ---- ModularAssistDevice.cpp:2196 ----
        public void SetUsbDeviceSemControlImageShiftValue(int nISX, int nISY, int nExecute)
        {
            // SET USB DEVICE : IMAGE SHIFT XY 설정
            // USB DEVICE 적용
            // IMAGE SHIFT 초기화
            m_dev.AfxIntegratedSetImageShiftInitilize();
            // IMAGE SHIFT XY 적용
            m_dev.AfxIntegratedSetImageShiftX(nISX);
            m_dev.AfxIntegratedSetImageShiftY(nISY);
            if (nExecute == 1)
            {
                // IMAGE SHIFT XY 실행
                m_dev.AfxIntegratedSetImageShiftX();
                m_dev.AfxIntegratedSetImageShiftY();
            }
        }

        // ---- ModularAssistDevice.cpp:2214 ----
        public void SetUsbDeviceSemControlStigValue(int nStigX, int nStigY, int nExecute)
        {
            // SET USB DEVICE : STIG XY 설정
            // USB DEVICE 적용
            // STIG XY 적용
            m_dev.AfxIntegratedSetStigX(nStigX);
            m_dev.AfxIntegratedSetStigY(nStigY);
            if (nExecute == 1)
            {
                // STIG XY 실행
                m_dev.AfxIntegratedSetStigX();
                m_dev.AfxIntegratedSetStigY();
            }
        }

        // ---- ModularAssistDevice.cpp:2230 ----
        public void SetUsbDeviceSemControlMagnificationIndex(int nMagIndex)
        {
            // SET USB DEVICE : STIG XY 설정
            // USB DEVICE 적용
            // OLWD 구하기
            int nFocusCoarse = m_dev.AfxGetSEMFocusCoarse();                   // Focus Coarse
            int nOLWD        = m_dev.AfxIntegratedFindWDValue(nFocusCoarse);   // OL WD
            // Magnification 설정
            m_dev.AfxSetSEMMagnificationIndex(nMagIndex);
            int nMagIdx = m_dev.AfxGetSEMMagnificationIndex();                 // 배율 Index값
            int nMag    = m_dev.m_nMagStepValue[nMagIdx];                      // 배율 값
            m_dev.AfxIntegratedSetMagnification(nMag, nOLWD);
        }

        // ---- ModularAssistDevice.cpp:2245 ----
        public void SetUsbDeviceSemControlOLValueChange(int nCoarse, int nFine)
        {
            // SET USB DEVICE : FOCUS COARSE, FINE 설정
            // USB DEVICE 적용
            // OL의 FOCUS COARSE/FINE 실행 및 배율보정, Rotation보정 실행
            m_dev.AfxIntegratedSetOL(nCoarse, nFine);
        }

        // ---- ModularAssistDevice.cpp:2254 ----
        public void SetUsbDeviceSemControlFocusCoarseValueChange(int nCoarse)
        {
            // SET USB DEVICE : FOCUS COARSE 설정 (디가우징을 위한 실행)
            // USB DEVICE 적용
            m_dev.AfxIntegratedSetFocusCoarse(nCoarse);
        }

        // ---- ModularAssistDevice.cpp:2262 ----
        public void SetSemSystemShutDownExecute(int nShutDown)
        {
            // SEM POWER SHUTDOWN 실행
            // USB DEVICE 적용
            if (nShutDown == 0)
            {
                // SEM SYSTEM POWER SHUTDOWN 실행
                m_dev.AfxScanBridgePowerOff(DEF_PROGRAM_EXIT_POWEROFF);
            }
            else if (nShutDown == 1)
            {
                // SEM SYSTEM POWER SHUTDOWN 실행 & SERVER PROGRAM EXIT
                m_dev.AfxScanBridgePowerOff(DEF_PROGRAM_EXIT_POWEROFF);
                // 잠시대기
                //AfxSystemWaitTime(200);
            }
        }

        // ---- ModularAssistDevice.cpp:2281 ----
        public void SetUsbDeviceImageCaptureOnOffValue(int nOnOff, int nScanMode)
        {
            // SET USB DEVICE : NORMAL SCREEN MODE 설정
            // USB DEVICE 적용
            if (nOnOff == 1)
            {
                // AC SYNC ON/OFF
                int bAcSyncFlag = m_dev.AfxGetIntegratedPointer().bScanOptionFlag[IDX_HWSETUP_OPTION_BOOL_AC_SYNC_ON_OFF];

                // AC SYNC : OFF
                if (bAcSyncFlag == FALSE)
                {
                    // USB DEVICE 실행
                    // IMAGE CAPTURE ON
                    m_dev.AfxIntegratedScanImageDataCaptureStart(nScanMode);
                }
                // AC SYNC : ON
                else
                {
                    // AC SYNC : SCAN MODE
                    int nAcSyncScanMode = 0;
                    if      (nScanMode == RED_SCAN_MODE   ) nAcSyncScanMode = 0;
                    else if (nScanMode == FAST_SCAN_MODE  ) nAcSyncScanMode = 1;
                    else if (nScanMode == SLOW1_SCAN_MODE ) nAcSyncScanMode = 2;
                    else if (nScanMode == SLOW2_SCAN_MODE ) nAcSyncScanMode = 3;
                    else if (nScanMode == PHOTO1_SCAN_MODE) nAcSyncScanMode = 4;
                    else if (nScanMode == PHOTO2_SCAN_MODE) nAcSyncScanMode = 5;
                    else if (nScanMode == PHOTO3_SCAN_MODE) nAcSyncScanMode = 6;
                    else if (nScanMode == PHOTO4_SCAN_MODE) nAcSyncScanMode = 7;
                    else if (nScanMode == PHOTO5_SCAN_MODE) nAcSyncScanMode = 8;
                    else if (nScanMode == PHOTO6_SCAN_MODE) nAcSyncScanMode = 9;
                    else return;

                    // INDEX 기본값
                    string strText = "";
                    int nScanModeIndex = IDX_HWSETUP_OPTION_INT_AC_SYNC_RED_FRAME_LINE_SYNC;

                    // SCAN MODE START INDEX 설정
                    nScanModeIndex = nScanModeIndex + (nAcSyncScanMode * 10);

                    // FRAME or LINE SYNC 설정값
                    int nFrameSync = m_dev.AfxGetIntegratedPointer().nScanOptionData[nScanModeIndex];

                    // SCAN SKIP VALUE
                    int nScanSkip = m_dev.AfxGetIntegratedPointer().nScanOptionData[nScanModeIndex + 1];

                    // SCAN OVER SAMPLE
                    int nScanOSam = m_dev.AfxGetIntegratedPointer().nScanOptionData[nScanModeIndex + 2];

                    // X SCAN UP
                    int nXScanUp = m_dev.AfxGetIntegratedPointer().nScanOptionData[nScanModeIndex + 3];

                    // X SCAN DOWN
                    int nXScanDown = m_dev.AfxGetIntegratedPointer().nScanOptionData[nScanModeIndex + 4];

                    // Y SCAN UP
                    int nYScanUp = m_dev.AfxGetIntegratedPointer().nScanOptionData[nScanModeIndex + 5];

                    // Y SCAN DOWN
                    int nYScanDown = m_dev.AfxGetIntegratedPointer().nScanOptionData[nScanModeIndex + 6];

                    // X BITMAP PIXEL
                    int nXBmpPixel = m_dev.AfxGetIntegratedPointer().nScanOptionData[nScanModeIndex + 7];

                    // FRAME FREQ [Hz]
                    double dFrameFreq = m_dev.AfxGetIntegratedPointer().dScanOptionData[nScanModeIndex];

                    // LINE  FREQ [Hz]
                    double dLineFreq = m_dev.AfxGetIntegratedPointer().dScanOptionData[nScanModeIndex + 1];

                    // SYNC FREQ [Hz] = FRAME FREQ 계산값
                    double dSyncFreq = m_dev.AfxGetIntegratedPointer().dScanOptionData[nScanModeIndex + 2];

                    // SCAN CLOCK [Hz] 계산
                    int nScanClock = (int)m_dev.AfxGetIntegratedPointer().dScanOptionData[nScanModeIndex + 3];

                    // SCAN GENERATION
                    //AfxIntegratedSetAcSyncScanGeneration(nAcSyncScanMode, dFrameFreq, nScanSkip, nScanOSam, nXScanUp, nXScanDown, nYScanUp, nYScanDown, nScanClock);

                    // SCAN START
                    m_dev.AfxIntegratedAcSyncScanImageCaptureStart(nAcSyncScanMode, bAcSyncFlag, nFrameSync, dFrameFreq, dLineFreq);
                }
            }
            else
            {
                // USB DEVICE 실행
                // IMAGE CAPTURE OFF
                m_dev.AfxIntegratedScanImageDataCaptureStop();
            }
        }

        // ---- ModularAssistDevice.cpp:2373 ----
        public void SetUsbDeviceNormalScreenModeValue(int nMode)
        {
            // SET USB DEVICE : NORMAL SCREEN MODE 설정
            // USB DEVICE 적용
            if (nMode == NORMAL_SCREEN)
            {
                // USB DEVICE 실행
                m_dev.AfxIntegratedSetFPGANormalScreenMode();
            }
        }

        // ---- ModularAssistDevice.cpp:2385 ----
        public void SetUsbDeviceContrastValue(int nValue, int nExecute)
        {
            // SET USB DEVICE : CONTRAST VALUE 설정
            // USB DEVICE 적용
            m_dev.AfxIntegratedSetContrast(nValue);
            if (nExecute == 1)
            {
                // USB DEVICE 실행
                m_dev.AfxIntegratedSetContrast();
            }
        }

        // ---- ModularAssistDevice.cpp:2398 ----
        public void SetUsbDeviceBrightnessValue(int nValue, int nExecute)
        {
            // SET USB DEVICE : BRIGHTNESS VALUE 설정
            // USB DEVICE 적용
            m_dev.AfxIntegratedSetBrightness(nValue);
            if (nExecute == 1)
            {
                // USB DEVICE 실행
                m_dev.AfxIntegratedSetBrightness();
            }
        }

        // ---- ModularAssistDevice.cpp:2411 ----
        public void SetUsbDeviceScanRotationValue(int nValue, int nExecute)
        {
            // SET USB DEVICE : SCAN ROTATION VALUE 설정
            // ROTATION VALUE 범위 : 0 ~ 3599 (0도 ~ 359.9도)
            // USB DEVICE 적용
            m_dev.AfxScanBridgeSetScanRotationValue(nValue);
            if (nExecute == 1)
            {
                // USB DEVICE 실행
                m_dev.AfxScanBridgeSetScanRotation(nValue);
            }
        }

        // ---- ModularAssistDevice.cpp:2425 ----
        public void SetUsbDeviceEmissionAutoTransferValue(int nValue)
        {
            // SET USB DEVICE : EMISSION AUTO TRANSFER VALUE 설정
            // USB DEVICE 적용
            if (nValue == 1)
            {
                // USB DEVICE 실행
                // 1초 간격으로 Emission Current 자동 수신 설정
                m_dev.AfxIntegratedSetEmissionReceive();
            }
            else
            {
                // 자동전송 종료
                m_dev.AfxIntegratedSetEmissionReceiveStop();
            }
        }

        // ---- ModularAssistDevice.cpp:2443 ----
        public void SetUsbDeviceSemSEScanGernerarion(int nScanMode)
        {
            // AC SYNC ON/OFF
            int bAcSyncFlag = m_dev.AfxGetIntegratedPointer().bScanOptionFlag[IDX_HWSETUP_OPTION_BOOL_AC_SYNC_ON_OFF];

            // AC SYNC : OFF
            if (bAcSyncFlag == FALSE)
            {
                // SET USB DEVICE : SEM SE SCAN GENERATION 설정
                // USB DEVICE 적용
                // USB DEVICE 실행
                m_dev.AfxIntegratedSetScanGeneration(nScanMode);
            }
            // AC SYNC : ON
            else
            {
                // AC SYNC : SCAN MODE
                int nAcSyncScanMode = 0;
                if      (nScanMode == RED_SCAN_MODE   ) nAcSyncScanMode = 0;
                else if (nScanMode == FAST_SCAN_MODE  ) nAcSyncScanMode = 1;
                else if (nScanMode == SLOW1_SCAN_MODE ) nAcSyncScanMode = 2;
                else if (nScanMode == SLOW2_SCAN_MODE ) nAcSyncScanMode = 3;
                else if (nScanMode == PHOTO1_SCAN_MODE) nAcSyncScanMode = 4;
                else if (nScanMode == PHOTO2_SCAN_MODE) nAcSyncScanMode = 5;
                else if (nScanMode == PHOTO3_SCAN_MODE) nAcSyncScanMode = 6;
                else if (nScanMode == PHOTO4_SCAN_MODE) nAcSyncScanMode = 7;
                else if (nScanMode == PHOTO5_SCAN_MODE) nAcSyncScanMode = 8;
                else if (nScanMode == PHOTO6_SCAN_MODE) nAcSyncScanMode = 9;
                else return;

                // INDEX 기본값
                string strText = "";
                int nScanModeIndex = IDX_HWSETUP_OPTION_INT_AC_SYNC_RED_FRAME_LINE_SYNC;

                // SCAN MODE START INDEX 설정
                nScanModeIndex = nScanModeIndex + (nAcSyncScanMode * 10);

                // FRAME or LINE SYNC 설정값
                int nFrameSync = m_dev.AfxGetIntegratedPointer().nScanOptionData[nScanModeIndex];

                // SCAN SKIP VALUE
                int nScanSkip = m_dev.AfxGetIntegratedPointer().nScanOptionData[nScanModeIndex + 1];

                // SCAN OVER SAMPLE
                int nScanOSam = m_dev.AfxGetIntegratedPointer().nScanOptionData[nScanModeIndex + 2];

                // X SCAN UP
                int nXScanUp = m_dev.AfxGetIntegratedPointer().nScanOptionData[nScanModeIndex + 3];

                // X SCAN DOWN
                int nXScanDown = m_dev.AfxGetIntegratedPointer().nScanOptionData[nScanModeIndex + 4];

                // Y SCAN UP
                int nYScanUp = m_dev.AfxGetIntegratedPointer().nScanOptionData[nScanModeIndex + 5];

                // Y SCAN DOWN
                int nYScanDown = m_dev.AfxGetIntegratedPointer().nScanOptionData[nScanModeIndex + 6];

                // X BITMAP PIXEL
                int nXBmpPixel = m_dev.AfxGetIntegratedPointer().nScanOptionData[nScanModeIndex + 7];

                // FRAME FREQ [Hz]
                double dFrameFreq = m_dev.AfxGetIntegratedPointer().dScanOptionData[nScanModeIndex];

                // LINE  FREQ [Hz]
                double dLineFreq = m_dev.AfxGetIntegratedPointer().dScanOptionData[nScanModeIndex + 1];

                // SYNC FREQ [Hz] = FRAME FREQ 계산값
                double dSyncFreq = m_dev.AfxGetIntegratedPointer().dScanOptionData[nScanModeIndex + 2];

                // SCAN CLOCK [Hz] 계산
                int nScanClock = (int)m_dev.AfxGetIntegratedPointer().dScanOptionData[nScanModeIndex + 3];

                // SCAN GENERATION
                m_dev.AfxIntegratedSetAcSyncScanGeneration(nAcSyncScanMode, nFrameSync, dFrameFreq, nScanSkip, nScanOSam, nXScanUp, nXScanDown, nYScanUp, nYScanDown, nScanClock);

                // SCAN START
                //AfxIntegratedAcSyncScanImageCaptureStart(nAcSyncScanMode, bAcSyncFlag);
            }
        }

        // ---- ModularAssistDevice.cpp:2525 ----
        public void SetUsbDevicePMTRelayOnOffValue(int nRelayOnOff)
        {
            // SET USB DEVICE : PMT RELAY ON/OFF 설정
            // USB DEVICE 적용
            if (nRelayOnOff == 1)
            {
                // USB DEVICE 실행
                // PMT HV ON
                m_dev.AfxIntegratedPMTHVOn();
                // PMT HV RELAY ON
                m_dev.AfxIntegratedSetPMTHVRelayOn();
            }
            else
            {
                // USB DEVICE 실행
                // PMT HV OFF
                m_dev.AfxIntegratedPMTHVOff();
                // PMT HV RELAY OFF
                m_dev.AfxIntegratedSetPMTHVRelayOff();
            }
        }

        // ---- ModularAssistDevice.cpp:2548 ----
        public void SetUsbDeviceDetectorOnOffValue(int nRelayOnOff)
        {
            // SET USB DEVICE : DETECTOR ON/OFF 설정
            // USB DEVICE 적용
            if (nRelayOnOff == 1)
            {
                // USB DEVICE 실행
                // DETECTOR ON
                m_dev.AfxIntegratedDETHVOn();
                // DETECTOR RELAY ON
                m_dev.AfxIntegratedSetDETHVRelayOn();
            }
            else
            {
                // USB DEVICE 실행
                // DETECTOR OFF
                m_dev.AfxIntegratedDETHVOff();
                // DETECTOR RELAY OFF
                m_dev.AfxIntegratedSetDETHVRelayOff();
            }
        }

        // ---- ModularAssistDevice.cpp:2571 ----
        public void SetUsbDeviceWobbleIndex(int nWobIndex)
        {
            // SET USB DEVICE : WOBBLE INDEX 설정
            // USB DEVICE 적용
            m_dev.AfxIntegratedSetWobble(nWobIndex + 1);
        }

        // ---- ModularAssistDevice.cpp:2579 ----
        public void SetUsbDeviceWobblerFocusCoarseValue(int nCoarse, int nExecute)
        {
            // SET USB DEVICE : WOBBLER FOCUS COARSE VALUE 설정
            // USB DEVICE 적용
            if (nExecute == 1)
            {
                // USB DEVICE 실행
                m_dev.AfxIntegratedSetWobblerOLC(nCoarse);
            }
        }

        // ---- ModularAssistDevice.cpp:2591 ----
        public void SetUsbDeviceWobblerStigXValue(int nStigX, int nExecute)
        {
            // SET USB DEVICE : WOBBLER STIG-X VALUE 설정
            // USB DEVICE 적용
            if (nExecute == 1)
            {
                // USB DEVICE 실행
                m_dev.AfxIntegratedSetWobblerSTX(nStigX);
            }
        }

        // ---- ModularAssistDevice.cpp:2603 ----
        public void SetUsbDeviceWobblerStigYValue(int nStigY, int nExecute)
        {
            // SET USB DEVICE : WOBBLER STIG-Y VALUE 설정
            // USB DEVICE 적용
            if (nExecute == 1)
            {
                // USB DEVICE 실행
                m_dev.AfxIntegratedSetWobblerSTY(nStigY);
            }
        }

        // ---- ModularAssistDevice.cpp:2615 ----
        public void SetUsbDeviceMagnificationIndex(int nValue, int nExecute)
        {
            // SET USB DEVICE : MAGNIFICATION INDEX 설정
            // USB DEVICE 적용
            if (nExecute == 1)
            {
                // Mag 값 설정 및 실행
                m_dev.AfxSetSEMMagnificationStepIndex(nValue, TRUE);
            }
            else
            {
                // Mag 값만 설정
                m_dev.AfxSetSEMMagnificationStepIndex(nValue, FALSE);
            }
        }

        // ---- ModularAssistDevice.cpp:2632 ----
        public void SetUsbDeviceSemControlCL1CL2ExecuteValue(int nCL1, int nCL2)
        {
            // SET USB DEVICE : SEM VACUUM STATE 설정
            // USB DEVICE 적용
            // CL1, CL2 설정
            m_dev.AfxIntegratedSetCL1(nCL1);
            m_dev.AfxIntegratedSetCL2(nCL2);
            // CL1, CL2 적용
            m_dev.AfxIntegratedSetCL1();
            m_dev.AfxIntegratedSetCL2();
        }

        // ---- ModularAssistDevice.cpp:2645 ----
        public void SetUsbDeviceFocusCoarseFineValue(int nCoarse, int nFine, int nExecute)
        {
            // SET USB DEVICE : FOCUS COARSE/FINE VALUE 설정
            // USB DEVICE 적용
            m_dev.AfxSetSEMFocusCoarse(nCoarse);
            m_dev.AfxSetSEMFocusFine(nFine);
            if (nExecute == 1)
            {
                // USB DEVICE 실행
                m_dev.AfxIntegratedSetOL(nCoarse, nFine);

            }
        }

        // ---- ModularAssistDevice.cpp:2660 ----
        public void SetUsbDeviceFilamentValue(int nValue, int nFVExecute)
        {
            // SET USB DEVICE : FILAMENT VALUE 설정
            // USB DEVICE 적용
            m_dev.AfxScanBridgeSetFilamentValue(nValue);
            if (nFVExecute == 1)
            {
                // FILAMENT USB DEVICE "FV" 실행
                m_dev.AfxScanBridgeSetFilamentValueExecute();
            }
        }

        // ---- ModularAssistDevice.cpp:2673 ----
        public void SetUsbDeviceBiasValue(int nValue, int nBVExecute)
        {
            // SET USB DEVICE : BIAS VALUE 설정
            // USB DEVICE 적용
            m_dev.AfxScanBridgeSetBiasValue(nValue);
            if (nBVExecute == 1)
            {
                // BIAS USB DEVICE "BV" 실행
                m_dev.AfxScanBridgeSetBiasValueExecute();
            }
        }

        // ---- ModularAssistDevice.cpp:2686 ----
        public void SetUsbDeviceGunHVIndex(int nGunIndex, int nGunHVExecute)
        {
            // SET USB DEVICE : GUN HV INDEX 설정
            // GUN HV INDEX 값 설정만 하기
            m_dev.AfxSetSEMGunHvIndex(nGunIndex);
            // USB DEVICE 적용
            if (nGunHVExecute == 1)
            {
                // USB DEVICE 실행
                m_dev.AfxScanBridgeSetGunHVIndexExecute(nGunIndex);
            }
        }

        // ---- ModularAssistDevice.cpp:2700 ----
        public void SetUsbDeviceSpotSize(int nSpotSize)
        {
            // SET USB DEVICE : SPOT SIZE 설정 : 인덱스
            // Spot Size 값 설정
            m_dev.AfxIntegratedSetSpotSize(nSpotSize);// -1);
            // Spot Size 실행
            m_dev.AfxIntegratedSetSpotSize();
        }

        // ---- ModularAssistDevice.cpp:2710 ----
        public void SetUsbDeviceGunHVVoltValue(double dGunHVMin, double dGunHVMax, double dGunHVStart, double dGunHVStop, double dGunHVCur, string strData, int nAVExecute)
        {
            // SET USB DEVICE : GUN HV VALUE 설정
            // GUN HV Start Voltage
            // m_dGunHvStartVoltage = dGunHVMin;// dGunHVStart;
            // GUN HV Stop Voltage
            // m_dGunHvStopVoltage  = dGunHVMax;// dGunHVStop;
            // GUN HV Current Voltage
            // m_dGunHvCurVoltage   = dGunHVCur;
            // GUN HV Display
            // SetGunHvProgressInfoText(dGunHVMin, dGunHVMax, dGunHVCur, strData);
            // USB DEVICE 적용
            if (nAVExecute == 1)
            {
                int bRatioCalc = FALSE;
                // 목표값 도달시 배율보정값 계산
                if (dGunHVCur == dGunHVStop)
                {
                    bRatioCalc = TRUE;
                }
                // USB DEVICE "AV" 실행
                m_dev.AfxScanBridgeSetGunHVValue(dGunHVCur, bRatioCalc);
            }
        }

        // ---- ModularAssistDevice.cpp:2736 ----
        public void SetUsbDeviceFilamentVoltValue(int nFilamentPos, double dVMin, double dVMax, double dVStart, double dVStop, double dVCur, string strData, int nFVExecute)
        {
            // SET USB DEVICE : FILAMENT VOLTAGE VALUE 설정
            // FILAMENT Start Voltage
            // m_dFilamentStartVoltage = dVMin;// dVStart;
            // FILAMENT Stop Voltage
            // m_dFilamentStopVoltage  = dVMax;// dVStop;
            // FILAMENT Current Voltage
            // m_dFilamentCurVoltage   = dVCur;
            // FILAMENT Display
            // SetFilamentProgressVoltageInfoText(dVMin, dVMax, dVCur, strData);
            // USB DEVICE 적용
            m_dev.AfxScanBridgeSetFilamentValue(nFilamentPos);
            if (nFVExecute == 1)
            {
                // USB DEVICE "FV" 실행
                m_dev.AfxScanBridgeSetFilamentValue(nFilamentPos);
                m_dev.AfxScanBridgeSetFilamentValueExecute();
            }
        }

        // ---- ModularAssistDevice.cpp:2758 ----
        public void SetUsbDeviceBiasVoltValue(int nBiasPos, double dVMin, double dVMax, double dVStart, double dVStop, double dVCur, string strData, int nBVExecute)
        {
            // SET USB DEVICE : BIAS VOLTAGE VALUE 설정
            // BIAS Start Voltage
            // m_dBiasStartVoltage = dVMin;// dVStart;
            // BIAS Stop Voltage
            // m_dBiasStopVoltage  = dVMax;// dVStop;
            // BIAS Current Voltage
            // m_dBiasCurVoltage   = dVCur;
            // BIAS Display
            // SetBiasProgressVoltageInfoText(dVMin, dVMax, dVCur, strData);
            // USB DEVICE 적용
            m_dev.AfxScanBridgeSetBiasValue(nBiasPos);
            if (nBVExecute == 1)
            {
                // USB DEVICE "BV" 실행
                m_dev.AfxScanBridgeSetBiasValueExecute();
            }
        }

        // ---- ModularAssistDevice.cpp:2779 ----
        public void SetUsbDeviceGunAlignXValue(int nValue, int nExecute)
        {
            // SET USB DEVICE : GUN ALIGN X VALUE 설정
            // USB DEVICE 적용
            m_dev.AfxIntegratedSetGunAlignX(nValue);
            if (nExecute == 1)
            {
                // USB DEVICE 실행
                m_dev.AfxIntegratedSetGunAlignX();
            }
        }

        // ---- ModularAssistDevice.cpp:2792 ----
        public void SetUsbDeviceGunAlignYValue(int nValue, int nExecute)
        {
            // SET USB DEVICE : GUN ALIGN Y VALUE 설정
            // USB DEVICE 적용
            m_dev.AfxIntegratedSetGunAlignY(nValue);
            if (nExecute == 1)
            {
                // USB DEVICE 실행
                m_dev.AfxIntegratedSetGunAlignY();
            }
        }

        // ---- ModularAssistDevice.cpp:2805 ----
        public void SetUsbDeviceBeamAlignXValue(int nValue, int nExecute)
        {
            // SET USB DEVICE : BEAM ALIGN X VALUE 설정
            // USB DEVICE 적용
            m_dev.AfxIntegratedSetBeamAlignX(nValue);
            if (nExecute == 1)
            {
                // USB DEVICE 실행
                m_dev.AfxIntegratedSetBeamAlignX();
            }
        }

        // ---- ModularAssistDevice.cpp:2818 ----
        public void SetUsbDeviceBeamAlignYValue(int nValue, int nExecute)
        {
            // SET USB DEVICE : BEAM ALIGN Y VALUE 설정
            // USB DEVICE 적용
            m_dev.AfxIntegratedSetBeamAlignY(nValue);
            if (nExecute == 1)
            {
                // USB DEVICE 실행
                m_dev.AfxIntegratedSetBeamAlignY();
            }
        }

        // ---- ModularAssistDevice.cpp:2831 ----
        public void SetUsbDeviceImageShiftXValue(int nValue, int nExecute)
        {
            // SET USB DEVICE : IMAGE SHIFT X VALUE 설정
            // USB DEVICE 적용
            m_dev.AfxIntegratedSetImageShiftX(nValue);
            if (nExecute == 1)
            {
                // USB DEVICE 실행
                m_dev.AfxIntegratedSetImageShiftX();
            }
        }

        // ---- ModularAssistDevice.cpp:2844 ----
        public void SetUsbDeviceImageShiftYValue(int nValue, int nExecute)
        {
            // SET USB DEVICE : IMAGE SHIFT Y VALUE 설정
            // USB DEVICE 적용
            m_dev.AfxIntegratedSetImageShiftY(nValue);
            if (nExecute == 1)
            {
                // USB DEVICE 실행
                m_dev.AfxIntegratedSetImageShiftY();
            }
        }

        // ---- ModularAssistDevice.cpp:2857 ----
        public void SetUsbDeviceStigXValue(int nValue, int nExecute)
        {
            // SET USB DEVICE : STIG X VALUE 설정
            // USB DEVICE 적용
            m_dev.AfxIntegratedSetStigX(nValue);
            if (nExecute == 1)
            {
                // USB DEVICE 실행
                m_dev.AfxIntegratedSetStigX();
            }
        }

        // ---- ModularAssistDevice.cpp:2870 ----
        public void SetUsbDeviceStigYValue(int nValue, int nExecute)
        {
            // SET USB DEVICE : STIG Y VALUE 설정
            // USB DEVICE 적용
            m_dev.AfxIntegratedSetStigY(nValue);
            if (nExecute == 1)
            {
                // USB DEVICE 실행
                m_dev.AfxIntegratedSetStigY();
            }
        }

        // ---- ModularAssistDevice.cpp:2883 ----
        public void SetUsbDeviceAutoStartOptionValue(REQUEST_USB_DEVICE_DATA pRequestSetUsbDeviceData)
        {
            IntegratedCommandStruct pIntegratedData = m_dev.AfxGetIntegratedPointer();
            if (pIntegratedData == null) return;
            if (pRequestSetUsbDeviceData == null) return;

            // AUTO START OPTIN VALUE 설정
            // FILAMENT VALUE
            pIntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_FILAMENT_VALUE]   = pRequestSetUsbDeviceData.nRequestData[0];
            // BIAS VALUE
            pIntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_BIAS_VALUE]       = pRequestSetUsbDeviceData.nRequestData[1];
            // CONTRAST VALUE
            pIntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_CONTRAST_VALUE]   = pRequestSetUsbDeviceData.nRequestData[2];
            // BRIGHT VALUE
            pIntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_BRIGHT_VALUE]     = pRequestSetUsbDeviceData.nRequestData[3];
            // FILAMENT/BIAS 변화폭 적용율(%)
            pIntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_FIL_BIAS_RATIO]   = pRequestSetUsbDeviceData.nRequestData[4];
            // FILAMENT/BIAS STEP INTERVAL TIME[ms]
            pIntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_STEP_TIME_VALUE]  = pRequestSetUsbDeviceData.nRequestData[5];
            // EMISSION CURRENT VALUE [ua]
            pIntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_EM_CURRENT_VALUE] = pRequestSetUsbDeviceData.nRequestData[6];

        }

        // ---- ModularAssistDevice.cpp:2908 ----
        public void SetUsbDeviceAutoFocusOptionValue(REQUEST_USB_DEVICE_DATA pRequestSetUsbDeviceData)
        {
            IntegratedCommandStruct pIntegratedData = m_dev.AfxGetIntegratedPointer();
            if (pIntegratedData == null) return;
            if (pRequestSetUsbDeviceData == null) return;

            // AUTO FOCUS OPTIN VALUE 설정
            // SCAN MODE
            pIntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_SCANMODE]    = pRequestSetUsbDeviceData.nRequestData[0];
            // UP SCAN NUMBER
            pIntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_UP_NUMBER]   = pRequestSetUsbDeviceData.nRequestData[1];
            // DOWN SCAN NUMBER
            pIntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_DOWN_NUMBER] = pRequestSetUsbDeviceData.nRequestData[2];
            // SCAN STEP VALUE
            pIntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_STEP_VALUE]  = pRequestSetUsbDeviceData.nRequestData[3];
            // FOCUS CHANGE Y-POS VALUE
            pIntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_YPOS_VALUE]  = pRequestSetUsbDeviceData.nRequestData[4];

        }

        // ---- ModularAssistDevice.cpp:2929 ----
        public void SetUsbDeviceAutoContrastBrightnessOptionValue(REQUEST_USB_DEVICE_DATA pRequestSetUsbDeviceData)
        {
            IntegratedCommandStruct pIntegratedData = m_dev.AfxGetIntegratedPointer();
            if (pIntegratedData == null) return;
            if (pRequestSetUsbDeviceData == null) return;

            // AUTO CONTRAST & BRIGHTNESS CONTROL VALUE
            // CONTRAST VALUE
            pIntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_CONTRAST_VALUE]   = pRequestSetUsbDeviceData.nRequestData[0];
            // CONTRAST 100 JUMP STEP
            pIntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_CONTRAST_100JUMP] = pRequestSetUsbDeviceData.nRequestData[1];
            // CONTRAST 10 JUMP STEP
            pIntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_CONTRAST_10JUMP]  = pRequestSetUsbDeviceData.nRequestData[2];
            // BRIGHTNESS VALUE
            pIntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_BRIGHT_VALUE]     = pRequestSetUsbDeviceData.nRequestData[3];
            // BRIGHTNESS 100 JUMP STEP
            pIntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_BRIGHT_100JUMP]   = pRequestSetUsbDeviceData.nRequestData[4];
            // BRIGHTNESS 10 JUMP STEP
            pIntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_BRIGHT_10JUMP]    = pRequestSetUsbDeviceData.nRequestData[5];

        }

        // ---- ModularAssistDevice.cpp:2952 ----
        public void SetUsbDeviceHWSetupXYAmpOffsetOptionValue(REQUEST_USB_DEVICE_DATA pRequestSetUsbDeviceData)
        {
            IntegratedCommandStruct pIntegratedData = m_dev.AfxGetIntegratedPointer();
            if (pIntegratedData == null) return;
            if (pRequestSetUsbDeviceData == null) return;

            // SCAN MODE
            int nScanMode = pRequestSetUsbDeviceData.nRequestData[0];
            // X SCAN AMP
            if (pRequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_X_AMP)
            {
                int nXAmp = pRequestSetUsbDeviceData.nRequestData[1];
                // 새로운 X SCAN AMP 값 설정
                pIntegratedData.nXScanAmpVal[nScanMode] = nXAmp;
                // Image Cal X-Amp 실행
                //AfxScanBridgeSetXImageAmp(nXAmp);
                //AfxScanBridgeSetXImageAmp();
            }
            // X SCAN OFFSET
            else if (pRequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_X_OFF)
            {
                int nXOff = pRequestSetUsbDeviceData.nRequestData[1];
                // 새로운 X SCAN OFFSET 값 설정
                pIntegratedData.nXScanOffset[nScanMode] = nXOff;
                // Image Cal X-Offset 실행
                //AfxScanBridgeSetXImageOffset(nXOff);
                //AfxScanBridgeSetXImageOffset();
            }
            // Y SCAN AMP
            else if (pRequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_Y_AMP)
            {
                int nYAmp = pRequestSetUsbDeviceData.nRequestData[1];
                // 새로운 Y SCAN AMP 값 설정
                pIntegratedData.nYScanAmpVal[nScanMode] = pRequestSetUsbDeviceData.nRequestData[1];
                // Image Cal Y-Amp 실행
                //AfxScanBridgeSetYImageAmp(nYAmp);
                //AfxScanBridgeSetYImageAmp();
            }
            // Y SCAN OFFSET
            else if (pRequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_Y_OFF)
            {
                int nYOff = pRequestSetUsbDeviceData.nRequestData[1];
                // 새로운 Y SCAN OFFSET 값 설정
                pIntegratedData.nYScanOffset[nScanMode] = nYOff;
                // Image Cal Y-Offset 실행
                //AfxScanBridgeSetYImageOffset(nYOff);
                //AfxScanBridgeSetYImageOffset();
            }

        }

        // ---- ModularAssistDevice.cpp:3004 ----
        public void SetUsbDeviceHWSetupOrthogonalOptionValue(REQUEST_USB_DEVICE_DATA pRequestSetUsbDeviceData)
        {
            IntegratedCommandStruct pIntegratedData = m_dev.AfxGetIntegratedPointer();
            if (pIntegratedData == null) return;
            if (pRequestSetUsbDeviceData == null) return;

            // 새로운 ORTHOGONAL 값 설정
            pIntegratedData.nOrthogonal = pRequestSetUsbDeviceData.nRequestData[0];
            // 2020.05.06
            // USB DEVICE : Orthogonal 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceOrthogonalValue(pIntegratedData.nOrthogonal);

        }

        // ---- ModularAssistDevice.cpp:3020 ----
        public void SetUsbDeviceHWSetupSpotSizeCLOptionValue(REQUEST_USB_DEVICE_DATA pRequestSetUsbDeviceData)
        {
            IntegratedCommandStruct pIntegratedData = m_dev.AfxGetIntegratedPointer();
            if (pIntegratedData == null) return;
            if (pRequestSetUsbDeviceData == null) return;

            // SPOT-SIZE 
            if (pRequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_CL_DATA_SPOTSIZE)
            {
                // 새로운 SPOT-SIZE 선택값 설정
                pIntegratedData.nCLSpotSize = pRequestSetUsbDeviceData.nRequestData[0];
            }
            // CL1
            else if (pRequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_SPOTSIZE_CL1)
            {
                // 새로운 CL1 설정값
                pIntegratedData.wCL1List[pIntegratedData.nCLSpotSize - 1] = (ushort)pRequestSetUsbDeviceData.nRequestData[0];
            }
            // CL2
            else if (pRequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_SPOTSIZE_CL2)
            {
                // 새로운 CL2 설정값
                pIntegratedData.wCL2List[pIntegratedData.nCLSpotSize - 1] = (ushort)pRequestSetUsbDeviceData.nRequestData[0];
            }
            // SPOT-SIZE에 따른 CL1,CL2 설정값 적용
            else if (pRequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_CL_DATA_APPLY)
            {
                // 새로운 SPOT-SIZE 선택값 설정
                pIntegratedData.nCLSpotSize = pRequestSetUsbDeviceData.nRequestData[0];
                // 새로운 CL1 설정값 적용
                pIntegratedData.wCL1List[pIntegratedData.nCLSpotSize - 1] = (ushort)pRequestSetUsbDeviceData.nRequestData[1];
                // 새로운 CL2 설정값 적용
                pIntegratedData.wCL2List[pIntegratedData.nCLSpotSize - 1] = (ushort)pRequestSetUsbDeviceData.nRequestData[2];
            }

        }

        // ---- ModularAssistDevice.cpp:3058 ----
        public void SetUsbDeviceHWSetupScanClockOptionValue(REQUEST_USB_DEVICE_DATA pRequestSetUsbDeviceData)
        {
            IntegratedCommandStruct pIntegratedData = m_dev.AfxGetIntegratedPointer();
            if (pIntegratedData == null) return;
            if (pRequestSetUsbDeviceData == null) return;

            // SCAN MODE
            int nScanMode = pRequestSetUsbDeviceData.nRequestData[0];

            // SCAN SKIP
            if (pRequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_SCAN_SKIP)
            {
                // 새로운 SCAN SKIP 값 설정
                pIntegratedData.nSkipPixNum[nScanMode] = pRequestSetUsbDeviceData.nRequestData[1];
            }
            // SCAN OSAM
            else if (pRequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_SCAN_OSAM)
            {
                // 새로운 SCAN OSAM 값 설정
                pIntegratedData.nOverSampleNum[nScanMode] = pRequestSetUsbDeviceData.nRequestData[1];
            }
            // FRAME FREQ
            else if (pRequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_FRAME_FREQ)
            {
                // 새로운 FRAME FREQ 값 설정
                pIntegratedData.nFrameSec[nScanMode] = pRequestSetUsbDeviceData.nRequestData[1];
            }
            // X SCAN UP
            else if (pRequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_X_SCAN_UP)
            {
                // 새로운 X SCAN UP 값 설정
                pIntegratedData.nXScanUpNum[nScanMode] = pRequestSetUsbDeviceData.nRequestData[1];
            }
            // X SCAN DOWN
            else if (pRequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_X_SCAN_DOWN)
            {
                // 새로운 X SCAN DOWN 값 설정
                pIntegratedData.nXScanDownNum[nScanMode] = pRequestSetUsbDeviceData.nRequestData[1];
            }
            // Y SCAN UP
            else if (pRequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_Y_SCAN_UP)
            {
                // 새로운 Y SCAN UP 값 설정
                pIntegratedData.nYScanUpNum[nScanMode] = pRequestSetUsbDeviceData.nRequestData[1];
            }
            // Y SCAN DOWN
            else if (pRequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_X_SCAN_DOWN)
            {
                // 새로운 Y SCAN DOWN 값 설정
                pIntegratedData.nYScanDownNum[nScanMode] = pRequestSetUsbDeviceData.nRequestData[1];
            }

        }

    }
}
