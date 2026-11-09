// 원본 ModularAssistDevice.cpp:3113-3912 을 NanoImpact.CodeGen translate로 옮긴 뒤 손으로 고친 코드.
#pragma warning disable IDE1006, IDE0059, CS0162, CS0168, CS0219
using System;
using NanoImpact.Core.Native;
using static NanoImpact.Core.Def;
using static NanoImpact.Core.Native.CRuntime;

namespace NanoImpact.Hardware.Device
{
    public sealed partial class AssistDevice
    {
        // ---- ModularAssistDevice.cpp:3113 ----
        public void SetUsbDeviceHWSetupXYAmpOffsetCalibrationOptionValue(REQUEST_USB_DEVICE_DATA pRequestSetUsbDeviceData)
        {
            IntegratedCommandStruct pIntegratedData = m_dev.AfxGetIntegratedPointer();
            if (pIntegratedData == null) return;
            if (pRequestSetUsbDeviceData == null) return;

            ///
            // SCAN MODE에 따른 X-Y AMP/OFFSET 및 SCAN FREQ 설정값
            ///
            // SCAN MODE에 따른 X-Y AMP/OFFSET 배율보정 설정값
            if (pRequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_XY_AMP_OFFSET_APPLY)
            {
                // X-Y AMP/OFFSET 설정을 위한 SCAN MODE
                int nScanMode = pRequestSetUsbDeviceData.nRequestData[0];
                pIntegratedData.nXScanAmpVal[nScanMode] = pRequestSetUsbDeviceData.nRequestData[1]; // 새로운 X AMP    설정값
                pIntegratedData.nXScanOffset[nScanMode] = pRequestSetUsbDeviceData.nRequestData[2]; // 새로운 X OFFSET 설정값
                pIntegratedData.nYScanAmpVal[nScanMode] = pRequestSetUsbDeviceData.nRequestData[3]; // 새로운 Y AMP    설정값
                pIntegratedData.nYScanOffset[nScanMode] = pRequestSetUsbDeviceData.nRequestData[4]; // 새로운 Y OFFSET 설정값
                // USB DEVICE 실행
                SetUsbDeviceXScanAmp(pRequestSetUsbDeviceData.nRequestData[1]);    // X AMP
                SetUsbDeviceXScanOffset(pRequestSetUsbDeviceData.nRequestData[2]); // X OFFSET
                SetUsbDeviceYScanAmp(pRequestSetUsbDeviceData.nRequestData[3]);    // Y AMP
                SetUsbDeviceYScanOffset(pRequestSetUsbDeviceData.nRequestData[4]); // Y OFFSET
            }
            // SCAN MODE에 따른 X-Y SCAN FREQ 설정값
            else if (pRequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_XY_SCAN_FREQ_APPLY)
            {
                // X-Y AMP/OFFSET 설정을 위한 SCAN MODE
                int nScanMode = pRequestSetUsbDeviceData.nRequestData[0];
                pIntegratedData.nSkipPixNum[nScanMode]    = pRequestSetUsbDeviceData.nRequestData[1]; // 새로운 SCAN  SKIP  설정값
                pIntegratedData.nOverSampleNum[nScanMode] = pRequestSetUsbDeviceData.nRequestData[2]; // 새로운 SCAN  OVER  SAMPLE 설정값
                pIntegratedData.nFrameSec[nScanMode]      = pRequestSetUsbDeviceData.nRequestData[3]; // 새로운 FRAME FREQ  설정값
                pIntegratedData.nFrameSecVal[nScanMode]   = pRequestSetUsbDeviceData.nRequestData[3]; // 새로운 FRAME FREQ  설정값
                pIntegratedData.nXScanUpNum[nScanMode]    = pRequestSetUsbDeviceData.nRequestData[4]; // 새로운 X SCAN UP   설정값
                pIntegratedData.nXScanDownNum[nScanMode]  = pRequestSetUsbDeviceData.nRequestData[5]; // 새로운 X SCAN DOWN 설정값
                pIntegratedData.nYScanUpNum[nScanMode]    = pRequestSetUsbDeviceData.nRequestData[6]; // 새로운 Y SCAN UP   설정값
                pIntegratedData.nYScanDownNum[nScanMode]  = pRequestSetUsbDeviceData.nRequestData[7]; // 새로운 Y SCAN DOWN 설정값
                // USB DEVICE 실행
                SetUsbDeviceScanSkip(pRequestSetUsbDeviceData.nRequestData[1]);  // SCAN SKIP
                SetUsbDeviceScanOSam(pRequestSetUsbDeviceData.nRequestData[2]);  // SCAN OVER SAMPLE
                SetUsbDeviceFrameFreq(pRequestSetUsbDeviceData.nRequestData[3]); // FRAME FREQ
                // X-Y SCAN UP, DOWN
                SetUsbDeviceXYScanUpDown(pRequestSetUsbDeviceData.nRequestData[4], pRequestSetUsbDeviceData.nRequestData[5], pRequestSetUsbDeviceData.nRequestData[6], pRequestSetUsbDeviceData.nRequestData[7]);
            }

        }

        // ---- ModularAssistDevice.cpp:3161 ----
        public void SetUsbDeviceHWSetupWDSetupDataCalibrationOptionValue(REQUEST_USB_DEVICE_DATA pRequestSetUsbDeviceData)
        {
            IntegratedCommandStruct pIntegratedData = m_dev.AfxGetIntegratedPointer();
            if (pIntegratedData == null) return;
            if (pRequestSetUsbDeviceData == null) return;

            ///
            // WD에 따른 배율보정 설정값
            ///
            // WD에 따른 배율보정을 위한 설정값 자동저장 선택값
            if (pRequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_WD_SETUP_ONOFF)
            {
                // 새로운 WD 자동저장 선택값 설정
                pIntegratedData.bWDSetupFlag  = pRequestSetUsbDeviceData.nRequestData[0];
                // 새로운 WD(Z-AXIS) 선택값 설정
                pIntegratedData.nWDStepNumber = pRequestSetUsbDeviceData.nRequestData[1];
                // 새로운 WD-MAX 설정값
                pIntegratedData.nWDStepMax    = pRequestSetUsbDeviceData.nRequestData[6];
                // 새로운 OL 설정값
                pIntegratedData.nWDOLCoarse[pIntegratedData.nWDStepNumber]  = pRequestSetUsbDeviceData.nRequestData[2];
                // 새로운 S-ROT 설정값
                pIntegratedData.nWDSRotation[pIntegratedData.nWDStepNumber] = pRequestSetUsbDeviceData.nRequestData[3];
                // 새로운 ZOOM 설정값
                pIntegratedData.nWDZooming[pIntegratedData.nWDStepNumber]   = pRequestSetUsbDeviceData.nRequestData[4];
                // 새로운 MIN-MAG 설정값
                pIntegratedData.nWDMinMag[pIntegratedData.nWDStepNumber]    = pRequestSetUsbDeviceData.nRequestData[5];
            }
            // WD에 따른 배율보정을 위한 WD(Z-AXIS) 선택값
            else if (pRequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_WD_DATA_ZAXIS)
            {
                // 새로운 WD(Z-AXIS) 선택값 설정
                pIntegratedData.nWDStepNumber = pRequestSetUsbDeviceData.nRequestData[0];
            }
            // WD에 따른 배율보정을 위한 OL 설정값
            else if (pRequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_WD_DATA_OL)
            {
                // 새로운 OL 설정값
                pIntegratedData.nWDOLCoarse[pIntegratedData.nWDStepNumber] = pRequestSetUsbDeviceData.nRequestData[0];
            }
            // WD에 따른 배율보정을 위한 S-ROT 설정값
            else if (pRequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_WD_DATA_SROT)
            {
                // 새로운 S-ROT 설정값
                pIntegratedData.nWDSRotation[pIntegratedData.nWDStepNumber] = pRequestSetUsbDeviceData.nRequestData[0];
            }
            // WD에 따른 배율보정을 위한 ZOOM 설정값
            else if (pRequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_WD_DATA_ZOOM)
            {
                // 새로운 ZOOM 설정값
                pIntegratedData.nWDZooming[pIntegratedData.nWDStepNumber] = pRequestSetUsbDeviceData.nRequestData[0];
            }
            // WD에 따른 배율보정을 위한 MIN-MAG 설정값
            else if (pRequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_WD_DATA_MINMAG)
            {
                // 새로운 MIN-MAG 설정값
                pIntegratedData.nWDMinMag[pIntegratedData.nWDStepNumber] = pRequestSetUsbDeviceData.nRequestData[0];
            }
            // WD에 따른 배율보정을 위한 WD-MAX 설정값
            else if (pRequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_WD_DATA_MAX)
            {
                // 새로운 WD-MAX 설정값
                pIntegratedData.nWDStepMax = pRequestSetUsbDeviceData.nRequestData[0];
            }
            // WD에 따른 배율보정을 위한 설정값 저장
            else if (pRequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_WD_DATA_APPLY)
            {
                // 새로운 WD 자동저장 선택값 설정
                pIntegratedData.bWDSetupFlag  = pRequestSetUsbDeviceData.nRequestData[0];
                // 새로운 WD(Z-AXIS) 선택값 설정
                pIntegratedData.nWDStepNumber = pRequestSetUsbDeviceData.nRequestData[1];
                // 새로운 OL 설정값
                pIntegratedData.nWDOLCoarse[pIntegratedData.nWDStepNumber]  = pRequestSetUsbDeviceData.nRequestData[2];
                // 새로운 S-ROT 설정값
                pIntegratedData.nWDSRotation[pIntegratedData.nWDStepNumber] = pRequestSetUsbDeviceData.nRequestData[3];
                // 새로운 ZOOM 설정값
                pIntegratedData.nWDZooming[pIntegratedData.nWDStepNumber]   = pRequestSetUsbDeviceData.nRequestData[4];
                // 새로운 MIN-MAG 설정값
                pIntegratedData.nWDMinMag[pIntegratedData.nWDStepNumber]    = pRequestSetUsbDeviceData.nRequestData[5];
                // 새로운 WD-MAX 설정값
                pIntegratedData.nWDStepMax = pRequestSetUsbDeviceData.nRequestData[6];
            }

        }

        // ---- ModularAssistDevice.cpp:3246 ----
        public void SetUsbDeviceHWSetupCLSpotSizeOptionValue(REQUEST_USB_DEVICE_DATA pRequestSetUsbDeviceData)
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

        }

        // ---- ModularAssistDevice.cpp:3262 ----
        public void SetUsbDeviceHWSetupScanAverageOptionValue(REQUEST_USB_DEVICE_DATA pRequestSetUsbDeviceData)
        {
            IntegratedCommandStruct pIntegratedData = m_dev.AfxGetIntegratedPointer();
            if (pIntegratedData == null) return;
            if (pRequestSetUsbDeviceData == null) return;

            if (pRequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_SCAN_AVERAGE)
            {
                // RED SCAN AVERAGE
                pIntegratedData.nWAverageNum[RED_SCAN_MODE]   = pRequestSetUsbDeviceData.nRequestData[0];
                // FAST SCAN AVERAGE
                pIntegratedData.nWAverageNum[FAST_SCAN_MODE]  = pRequestSetUsbDeviceData.nRequestData[1];
                // SLOW1 SCAN AVERAGE
                pIntegratedData.nWAverageNum[SLOW1_SCAN_MODE] = pRequestSetUsbDeviceData.nRequestData[2];
                // SLOW2 SCAN AVERAGE
                pIntegratedData.nWAverageNum[SLOW2_SCAN_MODE] = pRequestSetUsbDeviceData.nRequestData[3];
            }
            else if (pRequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_SCAN_RED_AVERAGE)
            {
                // RED SCAN AVERAGE
                pIntegratedData.nWAverageNum[RED_SCAN_MODE] = pRequestSetUsbDeviceData.nRequestData[0];
            }
            else if (pRequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_SCAN_FAST_AVERAGE)
            {
                // FAST SCAN AVERAGE
                pIntegratedData.nWAverageNum[FAST_SCAN_MODE] = pRequestSetUsbDeviceData.nRequestData[0];
            }

        }

        // ---- ModularAssistDevice.cpp:3293 ----
        public void SetUsbDeviceHWSetupScanCheckOptionValue(REQUEST_USB_DEVICE_DATA pRequestSetUsbDeviceData)
        {
            IntegratedCommandStruct pIntegratedData = m_dev.AfxGetIntegratedPointer();
            if (pIntegratedData == null) return;
            if (pRequestSetUsbDeviceData == null) return;

            // PHOTO NONE STOP --> 2020.05.26 PHOTO IMAGE FILTERING
            if (pRequestSetUsbDeviceData.nRequestData[0] >= 0) pIntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_PHOTO_IMAGE_FILTER]     = pRequestSetUsbDeviceData.nRequestData[0];
            // IMAGE SHIFT X REVERSE
            if (pRequestSetUsbDeviceData.nRequestData[1] >= 0) pIntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_IMAGE_SHIFT_X_REVERSE]  = pRequestSetUsbDeviceData.nRequestData[1];
            // IMAGE SHIFT Y REVERSE
            if (pRequestSetUsbDeviceData.nRequestData[2] >= 0) pIntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_IMAGE_SHIFT_Y_REVERSE]  = pRequestSetUsbDeviceData.nRequestData[2];
            // ORTHOGONAL CORRECTION
            if (pRequestSetUsbDeviceData.nRequestData[3] >= 0) pIntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_ORTHOGONAL_CORRRECTION] = pRequestSetUsbDeviceData.nRequestData[3];
            // STAGE HORZ ALIGN
            if (pRequestSetUsbDeviceData.nRequestData[4] >= 0) pIntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_STAGE_HORZ_ALIGN]       = pRequestSetUsbDeviceData.nRequestData[4];
            // Z AXIS FOCUS LINK
            if (pRequestSetUsbDeviceData.nRequestData[5] >= 0) pIntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_Z_AXIS_FOCUS_LINK]      = pRequestSetUsbDeviceData.nRequestData[5];
            // WD SCAN ROTATION CORRECTION
            if (pRequestSetUsbDeviceData.nRequestData[6] >= 0) pIntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_WD_SCAN_ROTATION]       = pRequestSetUsbDeviceData.nRequestData[6];
            // WD SCALE ZOOM CORRECTION
            if (pRequestSetUsbDeviceData.nRequestData[7] >= 0) pIntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_WD_SCALE_ZOOM]          = pRequestSetUsbDeviceData.nRequestData[7];
            // HV CORRECTION
            if (pRequestSetUsbDeviceData.nRequestData[8] >= 0) pIntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_HV_CORRECTION]          = pRequestSetUsbDeviceData.nRequestData[8];
            // 2021.08.23 : HV PROCESSING 완료 후, FOCUS DEGAUSSING
            if (pRequestSetUsbDeviceData.nRequestData[9] >= 0) pIntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_HV_DEGAUSSING]          = pRequestSetUsbDeviceData.nRequestData[9];
            // 2021.08.23 : 사용중단
            // 2021.07.29 : SCAN CLOCK 48 BITS
            //if (pRequestSetUsbDeviceData->nRequestData[9] >= 0) pIntegratedData->nScanOptionData[IDX_HWSETUP_OPTION_INT_SCAN_CLOCK_48]        = pRequestSetUsbDeviceData->nRequestData[9];
            pIntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_SCAN_CLOCK_48] = 0;

        }

        // ---- ModularAssistDevice.cpp:3327 ----
        public void SetUsbDeviceHWSetupMonitorInformationOptionValue(REQUEST_USB_DEVICE_DATA pRequestSetUsbDeviceData)
        {
            IntegratedCommandStruct pIntegratedData = m_dev.AfxGetIntegratedPointer();
            if (pIntegratedData == null) return;
            if (pRequestSetUsbDeviceData == null) return;

            // MONITOR SIZE (inch)
            pIntegratedData.dScanOptionData[IDX_HWSETUP_OPTION_DOUBLE_MONITOR_SIZE]  = pRequestSetUsbDeviceData.dRequestData[0];
            // MONITOR SIZE RATIO(%)
            pIntegratedData.dScanOptionData[IDX_HWSETUP_OPTION_DOUBLE_MONITOR_RATIO] = pRequestSetUsbDeviceData.dRequestData[1];

        }

        // ---- ModularAssistDevice.cpp:3341 ----
        public void SetUsbDeviceSemDeviceModelOptionValue(REQUEST_USB_DEVICE_DATA pRequestSetUsbDeviceData)
        {
            IntegratedCommandStruct pIntegratedData = m_dev.AfxGetIntegratedPointer();
            if (pIntegratedData == null) return;
            if (pRequestSetUsbDeviceData == null) return;

            // SEM DEVICE MODEL INFORMATION
            ZeroMem(pIntegratedData.szScanOption32Text1);
            int nLength = strlen(pRequestSetUsbDeviceData.szData);
            memcpy(pIntegratedData.szScanOption32Text1, pRequestSetUsbDeviceData.szData, nLength);

        }

        // ---- ModularAssistDevice.cpp:3355 ----
        public void SetUsbDeviceSemDeviceCompanyOptionValue(REQUEST_USB_DEVICE_DATA pRequestSetUsbDeviceData)
        {
            IntegratedCommandStruct pIntegratedData = m_dev.AfxGetIntegratedPointer();
            if (pIntegratedData == null) return;
            if (pRequestSetUsbDeviceData == null) return;

            // SEM DEVICE COMPANY INFORMATION
            ZeroMem(pIntegratedData.szScanOption32Text2);
            int nLength = strlen(pRequestSetUsbDeviceData.szData);
            memcpy(pIntegratedData.szScanOption32Text2, pRequestSetUsbDeviceData.szData, nLength);

        }

        // ---- ModularAssistDevice.cpp:3369 ----
        public void SetWDZAxisCalibrationData(int nWDPosition, int nOL, int nSRot, int nZoom, int nMinMag, int nWDMax, int nExecute)
        {
            // WD Z-Axis 적용( 변수값만 적용 )
            m_dev.AfxSetSEMCorrectionWDZAxisValue(nWDPosition);

            // WD에 따른 WD_OL 값 설정 및 실행
            SetWDZAxisOLCalibrationData(nOL, nExecute);

            // WD에 따른 S-ROT 값 설정 및 실행
            SetWDZAxisSRotCalibration(nSRot, nExecute);

            // WD에 따른 MIN-MAG 값 설정 및 실행
            SetWDZAxisMinMagCalibration(nMinMag, nExecute);
        }

        // ---- ModularAssistDevice.cpp:3385 ----
        public void SetWDZAxisOLCalibrationData(int nOL, int nExecute)
        {
            // 1) WD에 따른 OL-Coarse 값을 Focus Coarse 값에 적용
            m_dev.AfxSetSEMCorrectionWDOLCValue(nOL);                              // 현재 WD보정 OL-Coarse 값 적용
            if (nExecute < 1) return;

            // 2) WD에 따른 OL_Coarse, ScanRotation, Zooming, MinMag 보정값 10등분 선형화 작업
            m_dev.AfxIntegratedSetWDOLSrotZoomingLinearize();

            // 3) Focus 적용
            int nCoarse = m_dev.AfxGetSEMCorrectionWDOLCValue();                   // WD 보정 OL-Coarse값
            int nFine   = m_dev.AfxGetSEMFocusFine();                              // 현재 Focus-Fine 값
            m_dev.AfxIntegratedSetOL(nCoarse, nFine);                              // OL (Focus) 값 하드웨어 적용
        }

        // ---- ModularAssistDevice.cpp:3401 ----
        public void SetWDZAxisSRotCalibration(int nSRot, int nExecute)
        {
            // 1) WD에 따른 S-Rot 값을 S-Rotation 값에 적용
            m_dev.AfxSetSEMCorrectionWDSRotation(nSRot);                           // 현재 WD보정 S-Rot 값 적용
            if (nExecute < 1) return;

            // 2) WD에 따른 OL_Coarse, ScanRotation, Zooming, MinMag 보정값 10등분 선형화 작업
            m_dev.AfxIntegratedSetWDOLSrotZoomingLinearize();

            // 3) 적용
            int nWDSRot = m_dev.AfxGetSEMCorrectionWDSRotation();                  // WD 보정 S-Rot 값
            // 2020.05.18 : 0도~360도 값을 x10 (0~3599)로 변환한다
            nWDSRot = nWDSRot * 10;
            m_dev.AfxScanBridgeSetScanRotation(nWDSRot);                           // WD SRot 하드웨어 적용 
        }

        // ---- ModularAssistDevice.cpp:3418 ----
        public void SetWDZAxisZoomCalibration(int nZoom, int nExecute)
        {
            // 1) WD에 따른 Zooming 값
            m_dev.AfxSetSEMCorrectionWDZoomValue(nZoom);                           // WD Zoom 하드웨어 적용  
            if (nExecute < 1) return;

            // 2) WD에 따른 OL_Coarse, ScanRotation, Zooming, MinMag 보정값 10등분 선형화 작업
            m_dev.AfxIntegratedSetWDOLSrotZoomingLinearize();

            // 3) WD에 따른 Zooming 값 적용
            int nMag = m_dev.AfxGetSEMMagnificationIndex();                        // 배율 인덱스
            // 2020.01.20
            // 계산방식 변경 -> 배율 재설정
            m_dev.AfxSetSEMMagnificationStepIndex(nMag, TRUE);                     // 배율 적용
        }

        // ---- ModularAssistDevice.cpp:3435 ----
        public void SetWDZAxisMinMagCalibration(int nMinMag, int nExecute)
        {
            // 1) WD에 따른 Min-Mag 값
            m_dev.AfxSetSEMCorrectionWDMinMagValue(nMinMag);                       // WD Min-Mag 하드웨어 적용  
            if (nExecute < 1) return;
        }

        // ---- ModularAssistDevice.cpp:3443 ----
        public void SetWDZAxisWDMaxCalibration(int nWDMax, int nExecute)
        {
            // 1) WD MAX 값
            m_dev.AfxSetSEMCorrectionWDMaxValue(nWDMax);                           // WD Max 하드웨어 적용  
            if (nExecute < 1) return;
        }

        // ---- ModularAssistDevice.cpp:3451 ----
        public void SetSpotSizeCL1CL2CalibrationData(int nSpotSize, int nCL1, int nCL2, int nExecute)
        {
            // SET USB DEVICE : CL1, CL2 설정을 위한 SPOT-SIZE 실행
            m_dev.AfxIntegratedSetCLSpotSize(nSpotSize);                  // SpotSize 설정

            // 새로운 CL1, CL2 설정
            if (nExecute == 1)
            {
                m_dev.AfxIntegratedSetCL1(nCL1);                          // CL1값 설정
                m_dev.AfxIntegratedSetCL2(nCL2);                          // CL2값 설정
            }

            int nSpot  = m_dev.AfxIntegratedGetCLSpotSize();              // CL교정용 Spot Size
            int nSPCL1 = m_dev.AfxIntegratedGetCL1(nSpot);                // CL교정용 CL1 값
            int nSPCL2 = m_dev.AfxIntegratedGetCL2(nSpot);                // CL교정용 CL2 값
            m_dev.AfxIntegratedSetCL1(nSPCL1);                            // CL1값 설정
            m_dev.AfxIntegratedSetCL2(nSPCL2);                            // CL2값 설정
            m_dev.AfxIntegratedSetCL1();                                  // CL1 USB DEVICE 적용
            m_dev.AfxIntegratedSetCL2();                                  // CL2 USB DEVICE 적용

        }

        // ---- ModularAssistDevice.cpp:3474 ----
        public void SetSpotSizeCL1CalibrationData(int nCL1, int nExecute)
        {
            // SET USB DEVICE : CL1, CL2 설정을 위한 SPOT-SIZE 실행
            if (nExecute < 1) return;
            m_dev.AfxIntegratedSetCL1(nCL1);                              // CL1값 설정

            int nSpot  = m_dev.AfxIntegratedGetCLSpotSize();              // CL교정용 Spot Size
            int nSPCL1 = m_dev.AfxIntegratedGetCL1(nSpot);                // CL교정용 CL1 값
            m_dev.AfxIntegratedSetCL1(nSPCL1);                            // CL1값 설정
            m_dev.AfxIntegratedSetCL1();                                  // CL1 USB DEVICE 적용
        }

        // ---- ModularAssistDevice.cpp:3487 ----
        public void SetSpotSizeCL2CalibrationData(int nCL2, int nExecute)
        {
            // SET USB DEVICE : CL2 설정을 위한 SPOT-SIZE 실행
            if (nExecute < 1) return;
            m_dev.AfxIntegratedSetCL2(nCL2);                              // CL2값 설정

            int nSpot  = m_dev.AfxIntegratedGetCLSpotSize();              // CL교정용 Spot Size
            int nSPCL2 = m_dev.AfxIntegratedGetCL2(nSpot);                // CL교정용 CL2 값
            m_dev.AfxIntegratedSetCL2(nCL2);                              // CL2값 설정
            m_dev.AfxIntegratedSetCL2();                                  // CL2 USB DEVICE 적용
        }

        // ---- ModularAssistDevice.cpp:3500 ----
        public void SetUsbDeviceOrthogonalValue(int nOrthogonal)
        {
            // SET USB DEVICE : Orthogonal 실행
            // Orthogonal 설정
            m_dev.AfxScanBridgeSetOrthogonal(nOrthogonal);
            // Orthogonal 실행
            m_dev.AfxScanBridgeSetOrthogonal();
        }

        // ---- ModularAssistDevice.cpp:3510 ----
        public void SetUsbDeviceZoomXValue(int nZoomX)
        {
            // SET USB DEVICE : ZOOM X 실행
            // Zoom X 설정
            m_dev.AfxScanBridgeSetZoomXControl(nZoomX);
            // Zoom X 실행
            m_dev.AfxScanBridgeSetZoomXControl();
        }

        // ---- ModularAssistDevice.cpp:3520 ----
        public void SetUsbDeviceZoomYValue(int nZoomY)
        {
            // SET USB DEVICE : ZOOM Y 실행
            // Zoom Y 설정
            m_dev.AfxScanBridgeSetZoomYControl(nZoomY);
            // Zoom Y 실행
            m_dev.AfxScanBridgeSetZoomYControl();
        }

        // ---- ModularAssistDevice.cpp:3530 ----
        public void SetUsbDeviceXScanAmp(int nXAmp)
        {
            // SET USB DEVICE : X SCAN AMP 실행
            // Image Cal X-Amp 실행
            m_dev.AfxScanBridgeSetXImageAmp(nXAmp);
            m_dev.AfxScanBridgeSetXImageAmp();
        }

        // ---- ModularAssistDevice.cpp:3539 ----
        public void SetUsbDeviceXScanOffset(int nXOffset)
        {
            // SET USB DEVICE : X SCAN OFFSET 실행
            // Image Cal X-Offset 실행
            m_dev.AfxScanBridgeSetXImageOffset(nXOffset);
            m_dev.AfxScanBridgeSetXImageOffset();
        }

        // ---- ModularAssistDevice.cpp:3548 ----
        public void SetUsbDeviceYScanAmp(int nYAmp)
        {
            // SET USB DEVICE : Y SCAN AMP 실행
            // Image Cal X-Amp 실행
            m_dev.AfxScanBridgeSetYImageAmp(nYAmp);
            m_dev.AfxScanBridgeSetYImageAmp();
        }

        // ---- ModularAssistDevice.cpp:3557 ----
        public void SetUsbDeviceYScanOffset(int nYOffset)
        {
            // SET USB DEVICE : Y SCAN OFFSET 실행
            // Image Cal X-Offset 실행
            m_dev.AfxScanBridgeSetYImageOffset(nYOffset);
            m_dev.AfxScanBridgeSetYImageOffset();
        }

        // ---- ModularAssistDevice.cpp:3566 ----
        public void SetUsbDeviceScanSkip(int nSkip)
        {
            // SET USB DEVICE : Spot Size에 따른 CL2 실행
            // Skip Pixel 하드웨어 적용
            m_dev.AfxIntegratedSetSkipPixelNum(nSkip);
        }

        // ---- ModularAssistDevice.cpp:3574 ----
        public void SetUsbDeviceScanOSam(int nOSam)
        {
            // SET USB DEVICE : Scan OSam 실행
            // Over Sample 하드웨어 적용
            m_dev.AfxIntegratedSetOverSampleNum(nOSam);
        }

        // ---- ModularAssistDevice.cpp:3582 ----
        public void SetUsbDeviceFrameFreq(int nFreq)
        {
            // SET USB DEVICE : Frame Freq 실행
            // Frame Freq 하드웨어 적용
            m_dev.AfxIntegratedSetFrameFreq(nFreq);
        }

        // ---- ModularAssistDevice.cpp:3590 ----
        public void SetUsbDeviceXYScanUpDown(int nXUp, int XDown, int nYUp, int nYDown)
        {
            // SET USB DEVICE : X Scan Up 실행
            // Scan Up 변경
            m_dev.AfxIntegratedSetXScanUp(nXUp);     // X Scan Up 값 설정
            m_dev.AfxIntegratedSetYScanUp(nYUp);     // Y Scan Up 값 설정
            // Scan Down 변경
            m_dev.AfxIntegratedSetXScanDown(XDown);  // X Scan Down 값 설정
            m_dev.AfxIntegratedSetYScanDown(nYDown); // Y Scan Down 값 설정
            // 하드웨어 적용
            m_dev.AfxScanBridgeSetScanClockAndWaveform();
        }

        // ---- ModularAssistDevice.cpp:3604 ----
        public void SetExecuteStandbyLensCoilCurrentMinimum()
        {
            // Lens Coil에 흐르는 전류 최소화 ( USB DEVICE 실행 )
            m_dev.AfxIntegratedLensCoilCurrentMinimum();
        }

        // ---- ModularAssistDevice.cpp:3611 ----
        public void SetExecuteStandbyResetScanGeneration()
        {
            // SCAN 신호 초기화 ( 신호 OFF )
            m_dev.AfxIntegratedResetScanGeneration();
        }

        // ---- ModularAssistDevice.cpp:3618 ----
        public void SetExecuteStandbySEMPowerOffMode()
        {
            // STANDBY 모드시, SEM POWER OFF MODE로 설정

            // INTEGRATED DATA 파일 저장
            m_dev.AfxSetIntegratedDataToFile();

            // 2020.05.01
            // 프로그램 종료 : SEM SYSTEM 일반적인 종료
            m_dev.AfxScanBridgePowerOff(DEF_PROGRAM_EXIT_AXIS);
        }

        // ---- ModularAssistDevice.cpp:3631 ----
        public void SetExecutePhotoNegativeInverse(int bInverse)
        {
            // PHOTO 이미지 음영반전 실행
            if (m_nSeBseScanMode == 0)
            {
                // SE SCAN MODE 시 적용 및 실행
                m_dev.AfxSetSEMNegativeImageCheckFlag(bInverse, TRUE);
            }
            else
            {
                // BSE SCAN MODE 시 설정값 만 적용
                m_dev.AfxSetSEMNegativeImageCheckFlag(bInverse, FALSE);
            }
        }

        // ---- ModularAssistDevice.cpp:3647 ----
        public void SetExecutePhotoLRInverse(int bInverse)
        {
            // PHOTO 이미지 좌우반전 실행
            if (m_nSeBseScanMode == 0)
            {
                // SE SCAN MODE 시 적용 및 실행
                m_dev.AfxSetSEMLRMirrorImageCheckFlag(bInverse, TRUE);
            }
            else
            {
                // BSE SCAN MODE 시 설정값 만 적용
                m_dev.AfxSetSEMLRMirrorImageCheckFlag(bInverse, FALSE);
            }
        }

        // ---- ModularAssistDevice.cpp:3663 ----
        public void SetExecutePhotoTBInverse(int bInverse)
        {
            // PHOTO 이미지 상하반전 실행
            if (m_nSeBseScanMode == 0)
            {
                // SE SCAN MODE 시 적용 및 실행
                m_dev.AfxSetSEMTBMirrorImageCheckFlag(bInverse, TRUE);
            }
            else
            {
                // BSE SCAN MODE 시 설정값 만 적용
                m_dev.AfxSetSEMTBMirrorImageCheckFlag(bInverse, FALSE);
            }
        }

        // ---- ModularAssistDevice.cpp:3679 ----
        public void SetExecuteBsePhotoNegativeInverse(int bInverse)
        {
            // BSE PHOTO 이미지 음영반전 실행
            if (m_nSeBseScanMode == 1)
            {
                // BSE SCAN MODE 시 적용 및 실행
                m_dev.AfxSetSEMBseNegativeImageCheckFlag(bInverse, TRUE);
            }
            else
            {
                // SE SCAN MODE 시 설정값 만 적용
                m_dev.AfxSetSEMBseNegativeImageCheckFlag(bInverse, FALSE);
            }

        }

        // ---- ModularAssistDevice.cpp:3696 ----
        public void SetExecuteBsePhotoLRInverse(int bInverse)
        {
            // BSE PHOTO 이미지 좌우반전 실행
            if (m_nSeBseScanMode == 1)
            {
                // BSE SCAN MODE 시 적용 및 실행
                m_dev.AfxSetSEMBseLRMirrorImageCheckFlag(bInverse, TRUE);
            }
            else
            {
                // SE SCAN MODE 시 설정값 만 적용
                m_dev.AfxSetSEMBseLRMirrorImageCheckFlag(bInverse, FALSE);
            }
        }

        // ---- ModularAssistDevice.cpp:3712 ----
        public void SetExecuteBsePhotoTBInverse(int bInverse)
        {
            // BSE PHOTO 이미지 상하반전 실행
            if (m_nSeBseScanMode == 1)
            {
                // BSE SCAN MODE 시 적용 및 실행
                m_dev.AfxSetSEMBseTBMirrorImageCheckFlag(bInverse, TRUE);
            }
            else
            {
                // SE SCAN MODE 시 설정값 만 적용
                m_dev.AfxSetSEMBseTBMirrorImageCheckFlag(bInverse, FALSE);
            }
        }

        // ---- ModularAssistDevice.cpp:3728 ----
        public void SetSeScanModeOn()
        {
            // Integrated Control : IMAGE CH2 OFF ( SE IMAGE )
            m_dev.AfxIntegratedBSEImageOff();
            // SE MODE 설정
            m_nSeBseScanMode = 0;
        }

        // ---- ModularAssistDevice.cpp:3737 ----
        public void SetBseScanModeOn()
        {
            // Integrated Control : IMAGE CH2 ON ( BSE IMAGE )
            m_dev.AfxIntegratedBSEImageOn();
            // BSE MODE 설정
            m_nSeBseScanMode = 1;
        }

        // ---- ModularAssistDevice.cpp:3745 ----
        public void ExecuteCLLMHModeData(int nCLLMHMode, int nCLLMH123Mode)
        {
            if (m_dev.AfxGetIntegratedPointer() == null)
                return;

            int nCL1 = 0;
            int nCL2 = 0;
            int nGAX = 0;
            int nGAY = 0;
            int nBAX = 0;
            int nBAY = 0;

            // CL LMH MODE 사용
            if (nCLLMHMode == IDX_CL_LMH_LOW_MODE)
            {
                // CL LMH MODE : LOW MODE
                if (nCLLMH123Mode == IDX_CL_LMH_MODE_1_SELECT)
                {
                    // CL LMH : LOW 1 MODE
                    nCL1 = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW1_CL1_VALUE];
                    nCL2 = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW1_CL2_VALUE];
                    nGAX = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW1_GAX_VALUE];
                    nGAY = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW1_GAY_VALUE];
                    nBAX = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW1_BAX_VALUE];
                    nBAY = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW1_BAY_VALUE];
                }
                else if (nCLLMH123Mode == IDX_CL_LMH_MODE_2_SELECT)
                {
                    // CL LMH : LOW 2 MODE
                    nCL1 = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW2_CL1_VALUE];
                    nCL2 = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW2_CL2_VALUE];
                    nGAX = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW2_GAX_VALUE];
                    nGAY = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW2_GAY_VALUE];
                    nBAX = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW2_BAX_VALUE];
                    nBAY = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW2_BAY_VALUE];
                }
                else if (nCLLMH123Mode == IDX_CL_LMH_MODE_3_SELECT)
                {
                    // CL LMH : LOW 3 MODE
                    nCL1 = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW3_CL1_VALUE];
                    nCL2 = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW3_CL2_VALUE];
                    nGAX = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW3_GAX_VALUE];
                    nGAY = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW3_GAY_VALUE];
                    nBAX = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW3_BAX_VALUE];
                    nBAY = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW3_BAY_VALUE];
                }
            }
            else if (nCLLMHMode == IDX_CL_LMH_MIDDLE_MODE)
            {
                // CL LMH MODE : MIDDLE MODE
                if (nCLLMH123Mode == IDX_CL_LMH_MODE_1_SELECT)
                {
                    // CL LMH : MIDDLE 1 MODE
                    nCL1 = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE1_CL1_VALUE];
                    nCL2 = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE1_CL2_VALUE];
                    nGAX = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE1_GAX_VALUE];
                    nGAY = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE1_GAY_VALUE];
                    nBAX = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE1_BAX_VALUE];
                    nBAY = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE1_BAY_VALUE];
                }
                else if (nCLLMH123Mode == IDX_CL_LMH_MODE_2_SELECT)
                {
                    // CL LMH : MIDDLE 2 MODE
                    nCL1 = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE2_CL1_VALUE];
                    nCL2 = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE2_CL2_VALUE];
                    nGAX = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE2_GAX_VALUE];
                    nGAY = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE2_GAY_VALUE];
                    nBAX = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE2_BAX_VALUE];
                    nBAY = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE2_BAY_VALUE];
                }
                else if (nCLLMH123Mode == IDX_CL_LMH_MODE_3_SELECT)
                {
                    // CL LMH : MIDDLE 3 MODE
                    nCL1 = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE3_CL1_VALUE];
                    nCL2 = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE3_CL2_VALUE];
                    nGAX = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE3_GAX_VALUE];
                    nGAY = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE3_GAY_VALUE];
                    nBAX = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE3_BAX_VALUE];
                    nBAY = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE3_BAY_VALUE];
                }
            }
            else if (nCLLMHMode == IDX_CL_LMH_HIGH_MODE)
            {
                // CL LMH MODE : HIGH MODE
                if (nCLLMH123Mode == IDX_CL_LMH_MODE_1_SELECT)
                {
                    // CL LMH : HIGH 1 MODE
                    nCL1 = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH1_CL1_VALUE];
                    nCL2 = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH1_CL2_VALUE];
                    nGAX = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH1_GAX_VALUE];
                    nGAY = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH1_GAY_VALUE];
                    nBAX = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH1_BAX_VALUE];
                    nBAY = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH1_BAY_VALUE];
                }
                else if (nCLLMH123Mode == IDX_CL_LMH_MODE_2_SELECT)
                {
                    // CL LMH : HIGH 2 MODE
                    nCL1 = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH2_CL1_VALUE];
                    nCL2 = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH2_CL2_VALUE];
                    nGAX = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH2_GAX_VALUE];
                    nGAY = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH2_GAY_VALUE];
                    nBAX = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH2_BAX_VALUE];
                    nBAY = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH2_BAY_VALUE];
                }
                else if (nCLLMH123Mode == IDX_CL_LMH_MODE_3_SELECT)
                {
                    // CL LMH : HIGH 3 MODE
                    nCL1 = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH3_CL1_VALUE];
                    nCL2 = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH3_CL2_VALUE];
                    nGAX = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH3_GAX_VALUE];
                    nGAY = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH3_GAY_VALUE];
                    nBAX = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH3_BAX_VALUE];
                    nBAY = m_dev.AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH3_BAY_VALUE];
                }
            }

            // CL LMH MODE 사용
            if (nCLLMHMode == IDX_CL_LMH_LOW_MODE ||
                nCLLMHMode == IDX_CL_LMH_MIDDLE_MODE ||
                nCLLMHMode == IDX_CL_LMH_HIGH_MODE)
            {
                // CL LMH MODE에 따른 GUN ALIGN, BEAM ALIGN, CL 값 USB 적용 및 실행

                // CL1/2
                m_dev.AfxIntegratedSetCL1(nCL1);             // CL1 값 설정
                m_dev.AfxIntegratedSetCL2(nCL2);             // CL2 값 설정

                m_dev.AfxIntegratedSetCL1();                 // CL1 USB DEVICE 적용
                m_dev.AfxIntegratedSetCL2();                 // CL2 USB DEVICE 적용

                // GUN ALIGN X/Y
                m_dev.AfxIntegratedSetGunAlignX(nGAX);       // GAX 값 설정
                m_dev.AfxIntegratedSetGunAlignY(nGAY);       // GAY 값 설정

                m_dev.AfxIntegratedSetGunAlignX();           // GAX USB DEVICE 적용
                m_dev.AfxIntegratedSetGunAlignY();           // GAY USB DEVICE 적용

                // BEAM ALIGN X/Y
                m_dev.AfxIntegratedSetBeamAlignX(nBAX);      // BAX 값 설정
                m_dev.AfxIntegratedSetBeamAlignY(nBAY);      // BAY 값 설정

                m_dev.AfxIntegratedSetBeamAlignX();          // BAX USB DEVICE 적용
                m_dev.AfxIntegratedSetBeamAlignY();          // BAY USB DEVICE 적용
            }
        }

        // ---- ModularAssistDevice.cpp:3893 ----
        // SEM USB DEVICE CONTROL : FOCUS DEGAUSSING 실행
        public void ExecuteFocusDegaussing(int nRunTime = 500)
        {
            // 2021.08.23
            // HV PROCESSING 완료 후 또는 Z축 이동시 FOCUS DEGAUSSING 실행
            IntegratedCommandStruct pIntegratedData = m_dev.AfxGetIntegratedPointer();
            if (pIntegratedData == null) return;

            // DEGAUSSING 실행여부 옵션 확인? 
            if (pIntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_HV_DEGAUSSING] == 0) return;

            // WD 계산 : (현재 Z축 위치값 - 샘플높이) x 10
            double dZPos  = m_dev.AfxGetSEMCurrentZAxisPosition();
            double dSamH  = m_dev.AfxIntegratedGetSampleHeight();
            double dNewWD = (dZPos - dSamH) * 10.0;         // 0~500 , 0 ~ 50.0mm
            int    nNewWD = (int)dNewWD;

            // FOCUS DEGAUSSING 실행
            m_dev.AfxIntegratedFocusDegaussing(nNewWD, nRunTime);

        }

    }
}
