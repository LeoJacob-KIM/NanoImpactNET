// 원본 USB_FT232H_Device.cpp:19204-21312 을 NanoImpact.CodeGen translate로 옮긴 뒤 손으로 고친 코드.
#pragma warning disable IDE1006, IDE0059, CS0162, CS0168, CS0219
using System;
using NanoImpact.Core.Native;
using static NanoImpact.Core.Def;
using static NanoImpact.Core.Native.CRuntime;

namespace NanoImpact.Hardware.Device
{
    public sealed partial class SemDevice
    {
        // ---- USB_FT232H_Device.cpp:19204 ----
        public int AfxSetSEMCorrectionWDMinMagValue(int nValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;
            if (m_pIntegrated.nWDPosition < SEM_WD_CALI_POS_MIN) m_pIntegrated.nWDPosition = SEM_WD_CALI_POS_MIN;
            if (m_pIntegrated.nWDPosition > SEM_WD_CALI_POS_MAX) m_pIntegrated.nWDPosition = SEM_WD_CALI_POS_MAX;

            // WD에 따른 보정(Correction) -> WD Min Mag 값
            m_pIntegrated.nWDMinMag[m_pIntegrated.nWDPosition] = nValue;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:19219 ----
        //
        // Integrated Control : WD에 따른 보정(Correction) -> WD Min-Mag 값
        //
        public int AfxGetSEMCorrectionWDMinMag(int nWD, int bOrig = FALSE)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // WD에 따른 보정(Correction) -> WD Min-Mag 값
            int n = 0;

            while (m_pIntegrated.nWDMinMag[nWD] != m_nMagStepValue[n] && n < 20)
            {
                n++;
            }

            if (bOrig == TRUE)
            {
                return n;
            }

            if (n >= 20)
            {
                return 10;
            }
            else
            {
                return n - 1;
            }
        }

        // ---- USB_FT232H_Device.cpp:19249 ----
        //
        // Integrated Control : Spot Size 값
        //
        public int AfxGetSEMSpotSizeValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Spot Size 값
            return m_pIntegrated.nSpotSize;
        }

        // ---- USB_FT232H_Device.cpp:19260 ----
        //
        // Integrated Control : Stage Horizental Align 값
        //
        public int AfxGetSEMStageHorzAlignValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Stage Horizental Align 값
            return m_pIntegrated.nStageHorAlign;
        }

        // ---- USB_FT232H_Device.cpp:19267 ----
        public int AfxSetSEMStageHorzAlignValue(int nValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Stage Horizental Align 값
            m_pIntegrated.nStageHorAlign = nValue;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:19280 ----
        //
        // Integrated Control : Set X Mag Step Relay Ratio 값
        //
        public int AfxSetSEMXMagStepRelayRatio(int nPos)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Set X Mag Step Relay Ratio
            // 배율에 따른 Mag-Relay 변경 및 전류구동용 저항값 오차보상을 위한 보정값(Max값)

            double dRatio = 0.0;

            // 현재 선택된 배율값(Magnification)
            int nMag = m_nMagStepValue[m_pIntegrated.nMagStepIndex];

            if (nMag >= SEM_MAG_STEP_X100K)
            {
                dRatio = (double)(nMag / SEM_MAG_STEP_X100K);
                m_pIntegrated.nMagRelayNo = 4;
            }
            else if (nMag >= SEM_MAG_STEP_X10K)
            {
                dRatio = (double)(nMag / SEM_MAG_STEP_X10K);
                m_pIntegrated.nMagRelayNo = 3;
            }
            else if (nMag >= SEM_MAG_STEP_X1K)
            {
                dRatio = (double)(nMag / SEM_MAG_STEP_X1K);
                m_pIntegrated.nMagRelayNo = 2;
            }
            else if (nMag >= SEM_MAG_STEP_X100)
            {
                dRatio = (double)(nMag / SEM_MAG_STEP_X100);
                m_pIntegrated.nMagRelayNo = 1;
            }
            else //if (nMag >= 20)
            {
                dRatio = (double)(nMag / SEM_MAG_STEP_X10);
                m_pIntegrated.nMagRelayNo = 0;
            }

            m_pIntegrated.wMagRelayNumber = (ushort)( (ushort)m_pIntegrated.nMagRelayNo);
            m_pIntegrated.nZoomXMaxValue[m_pIntegrated.wMagRelayNumber] = (int)((double)(nPos * dRatio));

            return m_pIntegrated.nZoomXMaxValue[m_pIntegrated.wMagRelayNumber];
        }

        // ---- USB_FT232H_Device.cpp:19327 ----
        //
        // Integrated Control : Set Y Mag Step Relay Ratio 값
        //
        public int AfxSetSEMYMagStepRelayRatio(int nPos)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Set Y Mag Step Relay Ratio
            // 배율에 따른 Mag-Relay 변경 및 전류구동용 저항값 오차보상을 위한 보정값(Max값)

            double dRatio = 0.0;

            // 현재 선택된 배율값(Magnification)
            int nMag = m_nMagStepValue[m_pIntegrated.nMagStepIndex];

            if (nMag >= SEM_MAG_STEP_X100K)
            {
                dRatio = (double)(nMag / SEM_MAG_STEP_X100K);
                m_pIntegrated.nMagRelayNo = 4;
            }
            else if (nMag >= SEM_MAG_STEP_X10K)
            {
                dRatio = (double)(nMag / SEM_MAG_STEP_X10K);
                m_pIntegrated.nMagRelayNo = 3;
            }
            else if (nMag >= SEM_MAG_STEP_X1K)
            {
                dRatio = (double)(nMag / SEM_MAG_STEP_X1K);
                m_pIntegrated.nMagRelayNo = 2;
            }
            else if (nMag >= SEM_MAG_STEP_X100)
            {
                dRatio = (double)(nMag / SEM_MAG_STEP_X100);
                m_pIntegrated.nMagRelayNo = 1;
            }
            else //if (nMag >= 20)
            {
                dRatio = (double)(nMag / SEM_MAG_STEP_X10);
                m_pIntegrated.nMagRelayNo = 0;
            }

            m_pIntegrated.wMagRelayNumber = (ushort)( (ushort)m_pIntegrated.nMagRelayNo);
            m_pIntegrated.nZoomYMaxValue[m_pIntegrated.wMagRelayNumber] = (int)((double)(nPos * dRatio));

            return m_pIntegrated.nZoomYMaxValue[m_pIntegrated.wMagRelayNumber];
        }

        // ---- USB_FT232H_Device.cpp:19374 ----
        //
        // Integrated Control : Set Magnification Step Index 값
        //
        public int AfxSetSEMMagnificationStepIndex(int nIndex = 0, int bSet = FALSE)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Set Magnification Step Index

            m_pIntegrated.nMagStepIndex = nIndex;

            if (m_pIntegrated.nMagStepIndex >= MAX_MAG_STEP) m_pIntegrated.nMagStepIndex = MAX_MAG_STEP - 1;
            if (m_pIntegrated.nMagStepIndex <= MIN_MAG_STEP) m_pIntegrated.nMagStepIndex = MIN_MAG_STEP;

            if (bSet == FALSE) return RET_SUCCESS_OK;

            // 2020.05.11
            // WD 위치값 찾기
            int nWD10 = AfxIntegratedFindWDValue(m_pIntegrated.nFocusCoarse);
            m_pIntegrated.nOLWD = nWD10;

            AfxIntegratedSetMagnification(m_nMagStepValue[m_pIntegrated.nMagStepIndex], m_pIntegrated.nOLWD);

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:19400 ----
        //
        // Integrated Control : Set(Save) Orthogonal Control Value 값
        //
        public int AfxSetSEMOrthogonalValue(int nValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Save Orthogonal Control Value 값
            m_pIntegrated.nOrthogonal = nValue;
            m_pIntegrated.nARAngle[0] = m_pIntegrated.nOrthogonal;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:19410 ----
        public int AfxGetSEMOrthogonalValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Get Orthogonal Control Value 값
            return m_pIntegrated.nARAngle[0];
        }

        // ---- USB_FT232H_Device.cpp:19421 ----
        //
        // Integrated Control : Get HV Ratio 값
        //
        public double AfxGetSEMHVRatioValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // HV Ratio Calc
            double dHVRatio = sqrt((double)((double)(m_nGunHVTable[m_pIntegrated.nGunHVIndex]) / (double)(MAX_GUN_HV_VOLTAGE)));
            m_pIntegrated.dHVRatio = dHVRatio;

            return m_pIntegrated.dHVRatio;
        }

        // ---- USB_FT232H_Device.cpp:19435 ----
        //
        // Integrated Control : Get CL1,2 Vref Value 값
        //
        public double AfxGetSEMCLVRefValue(int nCLValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            AfxGetSEMHVRatioValue();

            // Calc CL Vref
            double dDavCL = (double)((double)(nCLValue) * m_pIntegrated.dHVRatio * 10.0 / (double)(SEM_CL1_CL2_MAX + 1));
            double dVref = (dDavCL * m_pIntegrated.dAmpRk) / 10.0;

            return dVref;
        }

        // ---- USB_FT232H_Device.cpp:19451 ----
        //
        // Integrated Control : Get CL1,2 Aref Value 값
        //
        public double AfxGetSEMCLARefValue(int nCLValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            AfxGetSEMHVRatioValue();

            // Calc CL Aref
            double dDavCL = (double)((double)(nCLValue)* m_pIntegrated.dHVRatio * 10.0 / (double)(SEM_CL1_CL2_MAX + 1));
            double dVref = (dDavCL * m_pIntegrated.dAmpRk) / 10.0;
            double dAref = dVref / m_pIntegrated.dRefR;

            return dAref;
        }

        // ---- USB_FT232H_Device.cpp:19468 ----
        //
        // Integrated Control : Get CL1,CL2 Amp Rk Value 값
        //
        public double AfxGetSEMAmpRkValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // CL,CL2 Amp Rk
            return m_pIntegrated.dAmpRk;
        }

        // ---- USB_FT232H_Device.cpp:19479 ----
        //
        // Integrated Control : Get CL1,CL2 Ref R Value 값
        //
        public double AfxGetSEMRefRValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // CL,CL2 Ref R
            return m_pIntegrated.dRefR;
        }

        // ---- USB_FT232H_Device.cpp:19490 ----
        //
        // Integrated Control : Get Sync Type Value 값
        //
        public int AfxGetSEMSyncTypeValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Sync Type Value 값
            if ((m_pIntegrated.nSyncVal[m_pIntegrated.nScanMode] & SEM_SYNC_TYPE_FRAME_SYNC) != 0)
            {
                return SEM_FRAME_SYNC_TYPE;
            }

            return SEM_LINE_SYNC_TYPE;
        }

        // ---- USB_FT232H_Device.cpp:19506 ----
        //
        // Integrated Control : Get Sync Freq Value 값
        //
        public double AfxGetSEMSyncFreqValue()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Sync Freq Value 값
            int nIndex = (m_pIntegrated.nSyncVal[m_pIntegrated.nScanMode] >> 16);
            double dSyncFreq = (double)m_nSyncFreqTable[nIndex] / 100.0;

            return dSyncFreq;
        }

        // ---- USB_FT232H_Device.cpp:19520 ----
        //
        // Integrated Control : OL WD Auto Save
        //
        public int AfxSetSEMOLWDAutoSave()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // OL WD Auto Save
            //---------------------------------
            // WD 5mm (IniPar.WD_TopLimit)에 위치시킴
            //---------------------------------
            m_pIntegrated.nWDStepNumber = m_pIntegrated.nWDTopLimit;
            m_pIntegrated.nWDStepMax    = m_pIntegrated.nWDBottomLimit;

            // 2020.01.21
            //m_pIntegrated->nWDPosition   = m_pIntegrated->nWDStepNumber;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:19540 ----
        //
        // Integrated Control : 현재 WD Setup Z-Axis 위치 값
        //
        public int AfxSetSEMWDSetupZAxis(int nWDZAxis)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 현재 WD Setup Z-Axis 위치 값
            m_pIntegrated.nWDStepNumber = nWDZAxis;
            m_pIntegrated.nWDPosition   = nWDZAxis;

            if (m_pIntegrated.nWDPosition   < SEM_WD_CALI_POS_MIN) m_pIntegrated.nWDPosition   = SEM_WD_CALI_POS_MIN;
            if (m_pIntegrated.nWDPosition   > SEM_WD_CALI_POS_MAX) m_pIntegrated.nWDPosition   = SEM_WD_CALI_POS_MAX;
            if (m_pIntegrated.nWDStepNumber < SEM_WD_CALI_POS_MIN) m_pIntegrated.nWDStepNumber = SEM_WD_CALI_POS_MIN;
            if (m_pIntegrated.nWDStepNumber > SEM_WD_CALI_POS_MAX) m_pIntegrated.nWDStepNumber = SEM_WD_CALI_POS_MAX;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:19559 ----
        //
        // Integrated Control : 현재 WD (Z-Axis) 위치 값
        //
        public double AfxGetSEMCurrentZAxisPosition()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 현재 WD (Z-Axis) 위치 값
            return m_pIntegrated.dCurZPos;
        }

        // ---- USB_FT232H_Device.cpp:19570 ----
        //
        // Integrated Control : 현재 WD (Z-Axis) 위치 값
        //
        public int AfxSetSEMCurrentZAxisPosition(double dCurZPos)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 현재 WD (Z-Axis) 위치 값
            m_pIntegrated.dCurZPos = dCurZPos;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:19583 ----
        //
        // Integrated Control : WD에 따른 OL-Coarse 값을 Focus Coarse 값에 적용
        //
        public int AfxSetWDOLCoarseToFocusCoarse()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // WD에 따른 OL-Coarse 값을 Focus Coarse 값에 적용
            m_pIntegrated.nFocusCoarse = m_pIntegrated.nWDOLCoarse[m_pIntegrated.nWDPosition];

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:19596 ----
        //
        // Integrated Control : WD에 따른 S-Rotation 값을 Scan-Rotation 값에 적용
        //
        public int AfxSetWDSRotToSRotation()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // WD에 따른 S-Rotation 값을 Scan-Rotation 값에 적용
            m_pIntegrated.nSRotationValue = m_pIntegrated.nWDSRotation[m_pIntegrated.nWDPosition];

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:19609 ----
        //
        // Integrated Control : Change Enable Flag 값
        //
        public int AfxGetChangeEbableFlag()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Change Enable Flag
            return m_pIntegrated.bChangeEnableFlag;
        }

        // ---- USB_FT232H_Device.cpp:19616 ----
        public int AfxSetChangeEbableFlag(int bEnable)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Change Enable Flag
            m_pIntegrated.bChangeEnableFlag = bEnable;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:19629 ----
        //
        // Integrated Control : 직교도 교정값 보정기능 사용 Flag
        //
        public int AfxGetSEMOrthogonalCheckFlag()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 직교도 교정값 보정기능 사용 Flag
            return m_pIntegrated.bOrthogonalCheckFlag;
        }

        // ---- USB_FT232H_Device.cpp:19636 ----
        public int AfxSetSEMOrthogonalCheckFlag(int bEnable)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 직교도 교정값 보정기능 사용 Flag
            m_pIntegrated.bOrthogonalCheckFlag = bEnable;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:19691 ----
        //
        // Integrated Control : Photo Noe-Stop Scan 기능 사용 Flag
        //
        public int AfxGetSEMPhotoNoneStopFlag()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Photo Noe-Stop Scan 기능 사용 Flag
            return m_pIntegrated.bPhotoNoneStopFlag;
        }

        // ---- USB_FT232H_Device.cpp:19698 ----
        public int AfxSetSEMPhotoNoneStopFlag(int bEnable)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Photo Noe-Stop Scan 기능 사용 Flag
            m_pIntegrated.bPhotoNoneStopFlag = bEnable;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:19711 ----
        //
        // Integrated Control : HV에 따른 Scale 보정 사용 Flag
        //
        public int AfxGetHVCalibrationCheckFlag()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // HV에 따른 Scale 보정 사용 Flag
            return m_pIntegrated.bWDHVCompensationFlag;
        }

        // ---- USB_FT232H_Device.cpp:19718 ----
        public int AfxSetHVCalibrationCheckFlag(int bEnable)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // HV에 따른 Scale 보정 사용 Flag
            m_pIntegrated.bWDHVCompensationFlag = bEnable;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:19731 ----
        //
        // Integrated Control : Mag Step Value
        //
        public int AfxGetSEMMagStepValue(int nIndex)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            if (nIndex < 0) nIndex = 0;
            if (nIndex > MAX_MAG_STEP - 1)nIndex = MAX_MAG_STEP - 1;

            // Mag Step Value
            return m_nMagStepValue[nIndex];
        }

        // ---- USB_FT232H_Device.cpp:19745 ----
        //
        // Integrated Control : 음양반전 기능 사용 Flag
        //
        public int AfxGetSEMNegativeImageCheckFlag()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 음양반전 기능 사용 Flag
            return m_pIntegrated.bNEGImageFlag;
        }

        // ---- USB_FT232H_Device.cpp:19752 ----
        public int AfxSetSEMNegativeImageCheckFlag(int bEnable, int bApply = TRUE)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 음양반전 기능 사용 Flag
            m_pIntegrated.bNEGImageFlag = bEnable;

            if (bApply == FALSE) return RET_SUCCESS_OK;

            if (m_pIntegrated.bNEGImageFlag == TRUE)
            {
                // 음영반전 사용
                m_pIntegrated.nSeCommandValue |= NEGATIVE_IMG_HIGH;
                // Se Command 전송
                AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_OUTPORT, m_pIntegrated.nSeCommandValue);
            }
            else
            {
                // 음영반전 미사용
                m_pIntegrated.nSeCommandValue &= NEGATIVE_IMG_LOW;
                // Se Command 전송
                AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_OUTPORT, m_pIntegrated.nSeCommandValue);
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:19782 ----
        //
        // Integrated Control : 좌우반전 기능 사용 Flag
        //
        public int AfxGetSEMLRMirrorImageCheckFlag()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            //좌우반전 기능 사용 Flag
            return m_pIntegrated.bLRMirrorFlag;
        }

        // ---- USB_FT232H_Device.cpp:19789 ----
        public int AfxSetSEMLRMirrorImageCheckFlag(int bEnable, int bApply = TRUE)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 좌우반전 기능 사용 Flag
            m_pIntegrated.bLRMirrorFlag = bEnable;

            if (bApply == FALSE) return RET_SUCCESS_OK;

            if (m_pIntegrated.bLRMirrorFlag == TRUE)
            {
                // 좌우반전 사용
                m_pIntegrated.nSbrCommandState |= XSCAN_INVERSE_HIGH;
                // Sbr Data 전송
                AfxFT232HPortSendCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, cWR_OUTPORT, m_pIntegrated.nSbrCommandState); 
            }
            else
            {
                // 좌우반전 미사용
                m_pIntegrated.nSbrCommandState &= XSCAN_INVERSE_LOW;
                // Sbr Data 전송
                AfxFT232HPortSendCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, cWR_OUTPORT, m_pIntegrated.nSbrCommandState);
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:19819 ----
        //
        // Integrated Control : 상하반전 기능 사용 Flag
        //
        public int AfxGetSEMTBMirrorImageCheckFlag()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            //상하반전 기능 사용 Flag
            return m_pIntegrated.bTBMirrorFlag;
        }

        // ---- USB_FT232H_Device.cpp:19826 ----
        public int AfxSetSEMTBMirrorImageCheckFlag(int bEnable, int bApply = TRUE)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 상하반전 기능 사용 Flag
            m_pIntegrated.bTBMirrorFlag = bEnable;

            if (bApply == FALSE) return RET_SUCCESS_OK;

            if (m_pIntegrated.bTBMirrorFlag == TRUE)
            {
                // 상하반전 사용
                m_pIntegrated.nSbrCommandState |= YSCAN_INVERSE_HIGH;
                // Sbr Data 전송
                AfxFT232HPortSendCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, cWR_OUTPORT, m_pIntegrated.nSbrCommandState);
            }
            else
            {
                // 상하반전 미사용
                m_pIntegrated.nSbrCommandState &= YSCAN_INVERSE_LOW;
                // Sbr Data 전송
                AfxFT232HPortSendCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, cWR_OUTPORT, m_pIntegrated.nSbrCommandState);
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:19856 ----
        //
        // Integrated Control : BSE 음양반전 기능 사용 Flag
        //
        public int AfxGetSEMBseNegativeImageCheckFlag()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 2021.07.22
            // BSE 음양반전 기능 사용 Flag
            return m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_BSE_NEG_IMAGE];
        }

        // ---- USB_FT232H_Device.cpp:19864 ----
        public int AfxSetSEMBseNegativeImageCheckFlag(int bEnable, int bApply = TRUE)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 2021.07.22
            // BSE 음양반전 기능 사용 Flag
            m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_BSE_NEG_IMAGE] = bEnable;

            if (bApply == FALSE) return RET_SUCCESS_OK;

            if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_BSE_NEG_IMAGE] == TRUE)
            {
                // BSE 음영반전 사용
                m_pIntegrated.nSeCommandValue |= NEGATIVE_IMG_HIGH;
                // Se Command 전송
                AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_OUTPORT, m_pIntegrated.nSeCommandValue);
            }
            else
            {
                // BSE 음영반전 미사용
                m_pIntegrated.nSeCommandValue &= NEGATIVE_IMG_LOW;
                // Se Command 전송
                AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_OUTPORT, m_pIntegrated.nSeCommandValue);
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:19895 ----
        //
        // Integrated Control : BSE 좌우반전 기능 사용 Flag
        //
        public int AfxGetSEMBseLRMirrorImageCheckFlag()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 2021.07.22
            // BSE 좌우반전 기능 사용 Flag
            return m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_BSE_FLIP_HORZ];
        }

        // ---- USB_FT232H_Device.cpp:19903 ----
        public int AfxSetSEMBseLRMirrorImageCheckFlag(int bEnable, int bApply = TRUE)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 2021.07.22
            // BSE 좌우반전 기능 사용 Flag
            m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_BSE_FLIP_HORZ] = bEnable;

            if (bApply == FALSE) return RET_SUCCESS_OK;

            if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_BSE_FLIP_HORZ] == TRUE)
            {
                // BSE 좌우반전 사용
                m_pIntegrated.nSbrCommandState |= XSCAN_INVERSE_HIGH;
                // Sbr Data 전송
                AfxFT232HPortSendCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, cWR_OUTPORT, m_pIntegrated.nSbrCommandState);
            }
            else
            {
                // BSE 좌우반전 미사용
                m_pIntegrated.nSbrCommandState &= XSCAN_INVERSE_LOW;
                // Sbr Data 전송
                AfxFT232HPortSendCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, cWR_OUTPORT, m_pIntegrated.nSbrCommandState);
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:19934 ----
        //
        // Integrated Control : BSE 상하반전 기능 사용 Flag
        //
        public int AfxGetSEMBseTBMirrorImageCheckFlag()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 2021.07.22
            // BSE 상하반전 기능 사용 Flag
            return m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_BSE_FLIP_VERT];
        }

        // ---- USB_FT232H_Device.cpp:19942 ----
        public int AfxSetSEMBseTBMirrorImageCheckFlag(int bEnable, int bApply = TRUE)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 2021.07.22
            // BSE 상하반전 기능 사용 Flag
            m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_BSE_FLIP_VERT] = bEnable;

            if (bApply == FALSE) return RET_SUCCESS_OK;

            if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_BSE_FLIP_VERT] == TRUE)
            {
                // BSE 상하반전 사용
                m_pIntegrated.nSbrCommandState |= YSCAN_INVERSE_HIGH;
                // Sbr Data 전송
                AfxFT232HPortSendCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, cWR_OUTPORT, m_pIntegrated.nSbrCommandState);
            }
            else
            {
                // BSE 상하반전 미사용
                m_pIntegrated.nSbrCommandState &= YSCAN_INVERSE_LOW;
                // Sbr Data 전송
                AfxFT232HPortSendCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, cWR_OUTPORT, m_pIntegrated.nSbrCommandState);
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:19973 ----
        //
        // Integrated Control : CL1 전류에 따른 12bit DA
        //
        public int AfxGetSEMDACL1Value(int nSpotSize)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // CL1 전류에 따른 12bit DA
            return m_nDA_CL1[nSpotSize-1];
        }

        // ---- USB_FT232H_Device.cpp:19980 ----
        public int AfxSetSEMDACL1Value(int nSpotSize, int nValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // CL1 전류에 따른 12bit DA
            m_nDA_CL1[nSpotSize-1] = nValue;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:19993 ----
        //
        // Integrated Control : CL2 전류에 따른 12bit DA
        //
        public int AfxGetSEMDACL2Value(int nSpotSize)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // CL2 전류에 따른 12bit DA
            return m_nDA_CL2[nSpotSize-1];
        }

        // ---- USB_FT232H_Device.cpp:20000 ----
        public int AfxSetSEMDACL2Value(int nSpotSize, int nValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // CL2 전류에 따른 12bit DA
            m_nDA_CL2[nSpotSize-1] = nValue;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:20013 ----
        //
        // Integrated Control : 배율에 따른 OL 보정 방식
        //
        public int AfxGetSEMOLCompensation()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 배율에 따른 OL 보정 방식
            return m_pIntegrated.nOLCompensation;
        }

        // ---- USB_FT232H_Device.cpp:20020 ----
        public int AfxSetSEMOLCompensation(int nOLCompensation)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 배율에 따른 OL 보정 방식 설정
            m_pIntegrated.nOLCompensation = nOLCompensation;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:20033 ----
        //
        // Integrated Control : Skip Pixel 값 설정 및 하드웨어 적용
        //
        public int AfxIntegratedSetSkipPixelNum(int nSkipPixel)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            m_pIntegrated.nSkipPixNum[m_pIntegrated.nScanMode] = nSkipPixel;

            // Skip Pixel 값
            // Smg Command 전송
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cSKIP_NUM_LOAD, m_pIntegrated.nSkipPixNum[m_pIntegrated.nScanMode]);

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:20045 ----
        public int AfxIntegratedGetSkipPixelNum()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Skip Pixel 값
            return m_pIntegrated.nSkipPixNum[m_pIntegrated.nScanMode];
        }

        // ---- USB_FT232H_Device.cpp:20056 ----
        //
        // Integrated Control : Over Sample 값 설정 및 하드웨어 적용
        //
        public int AfxIntegratedSetOverSampleNum(int nOverSample)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            m_pIntegrated.nOverSampleNum[m_pIntegrated.nScanMode] = nOverSample;

            // Over Sample 값
            // Smg Command 전송
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cOS_NUM_LOAD, m_pIntegrated.nOverSampleNum[m_pIntegrated.nScanMode]);

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:20072 ----
        //
        // Integrated Control : Frame Freq 계산 및 하드웨어 적용
        //
        public int AfxIntegratedCalcAndExecuteFrameFreq()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            ushort[] wCom_Buf = NewArray<ushort>(TMP_STRING_LENGTH+1);
            ZeroMem(wCom_Buf);

            double dFrame  = 0.0;
            int nPhase_inc = 0;
            int nScan_clk  = 0;
            long nPhase_inc64 = 0;

            //--
            // Scan Clock 변경
            //--
            dFrame = (double)(m_pIntegrated.nFrameSecVal[m_pIntegrated.nScanMode]) * 0.001;
            dFrame = (double)(m_pIntegrated.nXScanUpNum[m_pIntegrated.nScanMode] + m_pIntegrated.nXScanDownNum[m_pIntegrated.nScanMode]) * (double)(m_pIntegrated.nYScanUpNum[m_pIntegrated.nScanMode] + m_pIntegrated.nYScanDownNum[m_pIntegrated.nScanMode]) * dFrame;
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
                wCom_Buf[0] = (ushort)(cWR_FDDS_LOW);       wCom_Buf[1] = (ushort)( (ushort)(nPhase_inc64 & 0x000000000000FFFF)); nPhase_inc64 = nPhase_inc64 >> 16;
                wCom_Buf[2] = (ushort)(cWR_FDDS_HIGH);      wCom_Buf[3] = (ushort)( (ushort)(nPhase_inc64 & 0x000000000000FFFF)); nPhase_inc64 = nPhase_inc64 >> 16;
                wCom_Buf[4] = (ushort)(cWR_FDDS_HIGH48);    wCom_Buf[5] = (ushort)( (ushort)(nPhase_inc64 & 0x000000000000FFFF));
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
                nPhase_inc   = (int)(nPhase_inc64);
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

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:20135 ----
        //
        // Integrated Control : Frame Freq 값 설정 및 하드웨어 적용
        //
        public int AfxIntegratedSetFrameFreq(int nFrameFreq)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            m_pIntegrated.nFrameSec[m_pIntegrated.nScanMode] = nFrameFreq;
            m_pIntegrated.nFrameSecVal[m_pIntegrated.nScanMode] = m_pIntegrated.nFrameSec[m_pIntegrated.nScanMode];

            // Frame Freq 계산 및 하드웨어 적용
            AfxIntegratedCalcAndExecuteFrameFreq();

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:20151 ----
        //
        // Integrated Control : X Scan Up 값 설정 및 하드웨어 적용
        //
        public int AfxIntegratedSetXScanUp(int nValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            m_pIntegrated.nXScanUp = nValue;
            m_pIntegrated.nXScanUpNum[m_pIntegrated.nScanMode] = m_pIntegrated.nXScanUp;

            // Frame Freq 계산 및 하드웨어 적용 - X Scan Up 변경
            //AfxScanBridgeSetScanClockAndWaveform();

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:20167 ----
        //
        // Integrated Control : Y Scan Up 값 설정 및 하드웨어 적용
        //
        public int AfxIntegratedSetYScanUp(int nValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            m_pIntegrated.nYScanUp = nValue;
            m_pIntegrated.nYScanUpNum[m_pIntegrated.nScanMode] = m_pIntegrated.nYScanUp;

            // Frame Freq 계산 및 하드웨어 적용 - Y Scan Up 변경
            //AfxScanBridgeSetScanClockAndWaveform();

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:20183 ----
        //
        // Integrated Control : X Scan Down 값 설정 및 하드웨어 적용
        //
        public int AfxIntegratedSetXScanDown(int nValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            m_pIntegrated.nXBlankWidth = nValue;
            m_pIntegrated.nXScanDownNum[m_pIntegrated.nScanMode] = m_pIntegrated.nXBlankWidth;

            // Frame Freq 계산 및 하드웨어 적용 - X Scan Down 변경
            //AfxScanBridgeSetScanClockAndWaveform();

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:20199 ----
        //
        // Integrated Control : Y Scan Down 값 설정 및 하드웨어 적용
        //
        public int AfxIntegratedSetYScanDown(int nValue)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            m_pIntegrated.nYBlankWidth = nValue;
            m_pIntegrated.nYScanDownNum[m_pIntegrated.nScanMode] = m_pIntegrated.nYBlankWidth;

            // Frame Freq 계산 및 하드웨어 적용 - Y Scan Down 변경
            //AfxScanBridgeSetScanClockAndWaveform();

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:20215 ----
        //
        // Scan Bridge : Scan Clock(주파수) 변경 및 X,YScan 기본파형 발생
        //
        public int AfxScanBridgeSetScanClockAndWaveform()
        {
            // Scan Clock(주파수) 변경 및 X,YScan 기본파형 발생
            //if (m_pSbrCommand == NULL) return RET_ERROR_SBR_COMMAND_HANDLE;
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;  // Integrated Command & Data Handle

            int nSM = m_pIntegrated.nScanMode; // Scan Mode

            //m_pIntegrated->nFrameSecVal[nSM]  = m_pIntegrated->nFrameSec[nSM];// Frame/sec
            //m_pIntegrated->nXScanUpNum[nSM]   = m_pIntegrated->nXScanUp;      // X Scan Up
            //m_pIntegrated->nXScanDownNum[nSM] = m_pIntegrated->nXBlankWidth;  // X Scan Down
            //m_pIntegrated->nYScanUpNum[nSM]   = m_pIntegrated->nYScanUp;      // Y Scan Up
            //m_pIntegrated->nYScanDownNum[nSM] = m_pIntegrated->nYBlankWidth;  // Y Scan Down

            if (m_pIntegrated.nXScanUpNum[nSM] == 0 || m_pIntegrated.nXScanDownNum[nSM] == 0 ||
                m_pIntegrated.nYScanUpNum[nSM] == 0 || m_pIntegrated.nYScanDownNum[nSM] == 0)
            {
                return RET_SUCCESS_OK;
            }

            //__int8 nSmall;      // Declares 8-bit integer  
            //__int16 nMedium;    // Declares 16-bit integer  
            //int nLarge;     // Declares 32-bit integer  
            //__int64 nHuge;      // Declares 64-bit integer  

            ushort[] wCom_Buf = NewArray<ushort>(TMP_STRING_LENGTH+1);
            ZeroMem(wCom_Buf);

            double dFrame   = 0.0;
            int nPhase_inc  = 0;
            int nScan_clk   = 0;
            int nInc_val    = 0;
            int nInc_height = 0;
            int nLow_val    = 0;
            int nHigh_val   = 0;
            int nDec_val    = 0;

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
                wCom_Buf[0] = (ushort)(cWR_FDDS_LOW);       wCom_Buf[1] = (ushort)( (ushort)(nPhase_inc64 & 0x000000000000FFFF)); nPhase_inc64 = nPhase_inc64 >> 16;
                wCom_Buf[2] = (ushort)(cWR_FDDS_HIGH);      wCom_Buf[3] = (ushort)( (ushort)(nPhase_inc64 & 0x000000000000FFFF)); nPhase_inc64 = nPhase_inc64 >> 16;
                wCom_Buf[4] = (ushort)(cWR_FDDS_HIGH48);    wCom_Buf[5] = (ushort)( (ushort)(nPhase_inc64 & 0x000000000000FFFF));
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
                nPhase_inc   = (int)(nPhase_inc64);
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


            //--
            // XScan 기본파형 발생
            //--
            memset(wCom_Buf, 0x00, sizeof(ushort)*(24 + 1));
            nInc_val    = 4096 / m_pIntegrated.nXScanUpNum[nSM];
            nInc_height = nInc_val * m_pIntegrated.nXScanUpNum[nSM];
            nLow_val    = (4096 - nInc_height) / 2;
            nHigh_val   = 4096 - nLow_val;
            nDec_val    = nInc_height / m_pIntegrated.nXScanDownNum[nSM];

            wCom_Buf[0] = (ushort)(cWR_XSCAN_LOW);        wCom_Buf[1] = (ushort)(nLow_val);
            wCom_Buf[2] = (ushort)(cWR_XSCAN_HIGH);       wCom_Buf[3] = (ushort)(nHigh_val);
            wCom_Buf[4] = (ushort)(cWR_XSCAN_INC_VAL);    wCom_Buf[5] = (ushort)(nInc_val);
            wCom_Buf[6] = (ushort)(cWR_XSCAN_DEC_VAL);    wCom_Buf[7] = (ushort)(nDec_val);
            wCom_Buf[8] = (ushort)(cWR_XSCAN_INC_PIX);    wCom_Buf[9] = (ushort)(m_pIntegrated.nXScanUpNum[nSM]);
            wCom_Buf[10] = (ushort)(cWR_XSCAN_DEC_PIX);   wCom_Buf[11] = (ushort)(m_pIntegrated.nXScanDownNum[nSM]);
            //--
            // YScan 기본파형 발생
            //--
            memset(wCom_Buf, 0x00, sizeof(ushort)*(24 + 1));
            nInc_val = 4096 / m_pIntegrated.nYScanUpNum[nSM];
            nInc_height = nInc_val * m_pIntegrated.nYScanUpNum[nSM];
            nLow_val = (4096 - nInc_height) / 2;
            nHigh_val = 4096 - nLow_val;
            nDec_val = nInc_height / m_pIntegrated.nYScanDownNum[nSM];

            wCom_Buf[12] = (ushort)(cWR_YSCAN_LOW);       wCom_Buf[13] = (ushort)(nLow_val);
            wCom_Buf[14] = (ushort)(cWR_YSCAN_HIGH);      wCom_Buf[15] = (ushort)(nHigh_val);
            wCom_Buf[16] = (ushort)(cWR_YSCAN_INC_VAL);   wCom_Buf[17] = (ushort)(nInc_val);
            wCom_Buf[18] = (ushort)(cWR_YSCAN_DEC_VAL);   wCom_Buf[19] = (ushort)(nDec_val);
            wCom_Buf[20] = (ushort)(cWR_YSCAN_INC_PIX);   wCom_Buf[21] = (ushort)(m_pIntegrated.nYScanUpNum[nSM]);    // 2048
            wCom_Buf[22] = (ushort)(cWR_YSCAN_DEC_PIX);   wCom_Buf[23] = (ushort)(m_pIntegrated.nYScanDownNum[nSM]);  //  128
            //--
            AfxFT232HPortSendMultiCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, wCom_Buf, 24); // Sbr X-Y Waveform Data 전송
            //--

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:20342 ----
        //
        // Integrated Control : Image Capture Change 하드웨어 적용
        //
        public int AfxIntegratedSetImageCaptureChange(int nOSSclk, int nImgSkip)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Image Capture Change
            ushort[] wComBuf = NewArray<ushort>(5 + 1);

            memset(wComBuf, 0x00, sizeof(ushort)*(5 + 1));

            wComBuf[0] = (ushort)(cSCCLK_NUM_LOAD);
            wComBuf[1] = (ushort)(nOSSclk);
            wComBuf[2] = (ushort)(cSKIP_NUM_LOAD);
            wComBuf[3] = (ushort)(nImgSkip);

            // Smg Command 전송
            AfxFT232HPortSendMultiCommand(IDX_FT232H_SEM_IMAGE_DEVICE, wComBuf, 4);

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:20365 ----
        //
        // Integrated Control : Normal Screen Mode 하드웨어 적용
        //
        public int AfxIntegratedSetFPGANormalScreenMode()
        {
            // Set FPGA

            // Scan Image Board : NORMAL SCREEN MODE
            m_pIntegrated.nSeCommandValue &= DUAL_SCREEN_LOW;
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_OUTPORT, m_pIntegrated.nSeCommandValue); // SE-Image Command 전송

            // Scan Bridge Board : NORMAL SCREEN MODE
            m_pIntegrated.nSbrCommandState &= DUAL_SCREEN_LOW;
            AfxFT232HPortSendCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, cWR_OUTPORT, m_pIntegrated.nSbrCommandState); // Sbr Data 전송

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:20383 ----
        //
        // Integrated Control : GUN HV EC VALUE Setting
        //
        public int AfxIntegratedSetGunHVECValue(ushort wECVal)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Smg Command 전송
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_EC + (wECVal + 4096 << 3));

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:20396 ----
        //
        // Integrated Control : SET STAGE OPTION DATA
        //
        public int AfxIntegratedSetStageOptionData(int nType = 0, int nIndex = 0, int bFlag = FALSE, byte btData = 0, int nData = 0, uint dwData = 0, double dData = 0.0)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // STAGE OPTION DATA 설정

            if (nType == IDX_STAGE_OPTION_BOOL_TYPE)
            {
                // BOOL 형 Data Type
                if (nIndex == IDX_STAGE_OPTION_BOOL_XY_REVERSE)
                {
                    m_pIntegrated.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_XY_REVERSE] = bFlag;        // X<-->Y 반전

                    // X축 위치값 읽기 (Read Position)
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RP0;");
                    // WAIT
                    //AfxSystemWaitTime(50);
                    Sleep(50);
                    // Y축 위치값 읽기 (Read Position)
                    AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RP1;");
                }
                else if (nIndex == IDX_STAGE_OPTION_BOOL_X_LR_REVERSE)
                {
                    m_pIntegrated.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_X_LR_REVERSE] = bFlag;      // X LR 반전
                }
                else if (nIndex == IDX_STAGE_OPTION_BOOL_Y_TB_REVERSE)
                {
                    m_pIntegrated.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_Y_TB_REVERSE] = bFlag;      // Y TB 반전
                }
                else if (nIndex == IDX_STAGE_OPTION_BOOL_Z_UD_REVERSE)
                {
                    m_pIntegrated.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_Z_UD_REVERSE] = bFlag;      // Z UD 반전
                }
                else if (nIndex == IDX_STAGE_OPTION_BOOL_Z_MOVE_ECHO)
                {
                    m_pIntegrated.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_Z_MOVE_ECHO]  = bFlag;      // Z MOVING ECHO ON/OFF
                }
                else if (nIndex == IDX_STAGE_OPTION_BOOL_ALL_DATA_VIEW)
                {
                    m_pIntegrated.bStageOptionFlag[IDX_STAGE_OPTION_BOOL_ALL_DATA_VIEW] = bFlag;     // AXIS ALL DATA VIEW : 5 Axis 사용
                }
            }
            else if (nType == IDX_STAGE_OPTION_BYTE_TYPE)
            {
                // BYTE 형 Data Type
            }
            else if (nType == IDX_STAGE_OPTION_INT_TYPE)
            {
                // INT 형 Data Type
            }
            else if (nType == IDX_STAGE_OPTION_DWORD_TYPE)
            {
                // DWORD 형 Data Type
            }
            else if (nType == IDX_STAGE_OPTION_DOUBLE_TYPE)
            {
                // DOUBLE 형 Data Type
            }


            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:20462 ----
        //
        // Integrated Control : AUTO PROCESS MODE
        //
        public int AfxIntegratedSetAutoProcessMode(int nAutoProcessMode)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            //
            // Auto Process Mode
            //
            // SE_SCAN_DATA_NORMAL_EXECUTE
            // SE_SCAN_DATA_AUTO_FOCUS_EXECUTE
            // SE_SCAN_DATA_AUTO_CONTRAST_EXECUTE
            // SE_SCAN_DATA_AUTO_BRIGHTNESS_EXECUTE

            m_nSEScanDataAutoModeExecute = nAutoProcessMode;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:20482 ----
        //
        // Integrated Control : SCAN HW SETUP OPTION DATA 설정
        //
        public int AfxIntegratedSetScanHWSetupOptionFlag(int nPhotoImageFilter, int nISXReverse, int nISYReverse, int nOrthogonal, int nStageHorz, int nZFocusLink, int nWDSRot, int nWDZoom, int nHVCorrect, int nHVDegaussing, int nScanClk48)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // SCAN HW SETUP : OPTION DATA

            // PHOTO NONE STOP --> 2020.05.26 PHOTO IMAGE FILTERING
            if (nPhotoImageFilter >= 0) m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_PHOTO_IMAGE_FILTER] = nPhotoImageFilter;

            // IMAGE SHIFT X REVERSE --> IMAGE EFFECT (음영,좌우,상하) 옵션으로 사용
            if (nISXReverse >= 0) m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_IMAGE_SHIFT_X_REVERSE]    = nISXReverse;

            // IMAGE SHIFT Y REVERSE --> x35k 배율보정 옵션으로 사용
            if (nISYReverse >= 0) m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_IMAGE_SHIFT_Y_REVERSE]    = nISYReverse;

            // ORTHOGONAL CORRECTION
            if (nOrthogonal >= 0) m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_ORTHOGONAL_CORRRECTION]   = nOrthogonal;

            // STAGE HORZ ALIGN
            if (nStageHorz >= 0) m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_STAGE_HORZ_ALIGN]          = nStageHorz;

            // Z AXIS FOCUS LINK
            if (nZFocusLink >= 0) m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_Z_AXIS_FOCUS_LINK]        = nZFocusLink;

            // WD SCAN ROTATION CORRECTION
            if (nWDSRot >= 0) m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_WD_SCAN_ROTATION]             = nWDSRot;

            // WD SCALE ZOOM CORRECTION
            if (nWDZoom >= 0) m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_WD_SCALE_ZOOM]                = nWDZoom;

            // HV CORRECTION
            if (nHVCorrect >= 0) m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_HV_CORRECTION]             = nHVCorrect;

            // 2021.08.23 : HV PROCESSING 완료 후, FOCUS DEGAUSSING
            if (nHVDegaussing >= 0) m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_HV_DEGAUSSING]          = nHVDegaussing;

            // 2021.07.29 : SCAN CLOCK 48BITS
            if (nHVDegaussing >= 0) m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_SCAN_CLOCK_48]          = nScanClk48;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:20527 ----
        //
        // Integrated Control : 배율에 따른 HV 보정값 사용자 설정
        //
        public void AfxIntegratedSetMagHVRatio(double[] pdMagHVRatio)
        {
            // 사용자 설정 배율 HV RATIO 보정값 설정
            Array.Copy(pdMagHVRatio, m_dOptionMagHVRatio, MAX_GUN_HV_TABLE);
            
            // 0값 CHECK?
            for (int n = 0; n < MAX_GUN_HV_TABLE; n++)
            {
                if (m_dOptionMagHVRatio[n] == 0.0)
                {
                    m_dOptionMagHVRatio[n] = DEFAULT_MAG_HV_RATIO;
                }
            }

            // 2020.06.03
            // 사용자 MAG HV RATIO : STAGE 수평 옵션을 임시로 사용한다.
            if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_STAGE_HORZ_ALIGN] == 1)
            {
                // 사용자 설정 배율 HV RATIO 보정값 설정
                // GUN HV 배율 비율 계산
                m_pIntegrated.dMagHVRatio = sqrt((double)m_nGunHVTable[m_pIntegrated.nGunHVIndex] / (double)m_nGunHVTable[0]) / m_dOptionMagHVRatio[m_pIntegrated.nGunHVIndex];

                // 배율 설정하기
                AfxIntegratedSetMagnification(m_nMagStepValue[m_pIntegrated.nMagStepIndex], m_pIntegrated.nOLWD);
            }

        }

        // ---- USB_FT232H_Device.cpp:20558 ----
        //
        // Integrated Control : 배율 x35k 이상 ZOOM 값 보정율 설정
        //
        public void AfxIntegratedSetMagx35kZoomRatio(double dMagx35kZoomRatio)
        {
            // 배율 x35k 이상 ZOOM 값 보정율 설정
            m_dOptionMagx35kZoomRatio = dMagx35kZoomRatio;

        }

        // ---- USB_FT232H_Device.cpp:20569 ----
        //
        // Integrated Control : 쓰레드 강제 종료 설정
        //
        public void AfxIntegratedSetThreadAllStopFlag()
        {
            // SEM Scan Bridge Board
            if (m_bScanBridgeConnected == FALSE) return;
            // SEM Se Image Board
            if (m_bSemImageConnected == FALSE) return;

            // Scan Bridge 쓰레드
            m_pScanBridgeThreadParams.bThreadStop  = TRUE;         // 쓰레드 강제 종료 설정
            // SE Image Data 쓰레드
            m_pSEImageDataThreadParams.bThreadStop = TRUE;         // 쓰레드 강제 종료 설정
        }

        // ---- USB_FT232H_Device.cpp:20586 ----
        //
        // Integrated Control : 샘플 높이 설정
        //
        public void AfxIntegratedSetSampleHeight(double dSampleHeight)
        {
            // 샘플높이
            m_dSampleHeight = dSampleHeight;
            m_pIntegrated.dScanOptionData[IDX_HWSETUP_OPTION_DOUBLE_SAMPLE_HEIGHT] = m_dSampleHeight;
        }

        // ---- USB_FT232H_Device.cpp:20597 ----
        //
        // Integrated Control : 샘플 넓이 설정
        //
        public void AfxIntegratedSetSampleWidth(double dSampleWidth)
        {
            // 샘플넓이
            m_dSampleWidth = dSampleWidth;
            m_pIntegrated.dScanOptionData[IDX_HWSETUP_OPTION_DOUBLE_SAMPLE_WIDTH] = m_dSampleWidth;
        }

        // ---- USB_FT232H_Device.cpp:20608 ----
        //
        // Integrated Control : 샘플 명칭 설정
        //
        public void AfxIntegratedSetSampleName(string pSampleName)
        {
            // 샘플명칭 : IDX_HWSETUP_OPTION_128TEXT_SAMPLE_NAME
            ZeroMem(m_pIntegrated.szScanOption128Text1);
            strcpy(m_pIntegrated.szScanOption128Text1, pSampleName);
        }

        // ---- USB_FT232H_Device.cpp:20619 ----
        //
        // Integrated Control : 샘플 높이 값 리턴
        //
        public double AfxIntegratedGetSampleHeight()
        {
            // 샘플높이
            return m_pIntegrated.dScanOptionData[IDX_HWSETUP_OPTION_DOUBLE_SAMPLE_HEIGHT];
        }

        // ---- USB_FT232H_Device.cpp:20629 ----
        //
        // Integrated Control : 샘플 넓이 값 리턴
        //
        public double AfxIntegratedGetSampleWidth()
        {
            // 샘플넓이
            return m_pIntegrated.dScanOptionData[IDX_HWSETUP_OPTION_DOUBLE_SAMPLE_WIDTH];
        }

        // ---- USB_FT232H_Device.cpp:20658 ----
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
        public int AfxIntegratedAcSyncScanImageCaptureStart(int nAcSyncScanMode, int bAcSyncFlag, int nFrameSync, double dFrameFreq, double dLineFreq)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;
            if (m_pSEImageDataThreadParams == null)return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 스캔모드
            int nSM = 0;
            if      (nAcSyncScanMode == 0) nSM = RED_SCAN_MODE;
            else if (nAcSyncScanMode == 1) nSM = FAST_SCAN_MODE;
            else if (nAcSyncScanMode == 2) nSM = SLOW1_SCAN_MODE;
            else if (nAcSyncScanMode == 3) nSM = SLOW2_SCAN_MODE;
            else if (nAcSyncScanMode == 4) nSM = PHOTO1_SCAN_MODE;
            else if (nAcSyncScanMode == 5) nSM = PHOTO2_SCAN_MODE;
            else if (nAcSyncScanMode == 6) nSM = PHOTO3_SCAN_MODE;
            else if (nAcSyncScanMode == 7) nSM = PHOTO4_SCAN_MODE;
            else if (nAcSyncScanMode == 8) nSM = PHOTO5_SCAN_MODE;
            else if (nAcSyncScanMode == 9) nSM = PHOTO6_SCAN_MODE;
            else return 0;

            // 스캔모드
            m_pIntegrated.nScanMode = nSM;

            // SE Image Data 스캔 허용 Flag 설정 ( STOP )
            m_pIntegrated.bScanDataReceive = FALSE;
            // SE Image Data Read Buffer Clear
            ZeroMem(m_pSEImageDataThreadParams.szFTReadBuf);

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
            m_pIntegrated.nWAFrameNumA = 0;
            m_pIntegrated.nWAFrameNumB = 0;
            m_pIntegrated.bLineNumberFlag     = FALSE;
            m_pIntegrated.bFirstButtonPressed = TRUE;

            //
            // Image Capture Start : Se Command 설정
            //

            // 스캔모드에 따른 SeCommand를 설정하여 적용한다
            int nAcSyncSECommand = 0;

            // Capture Start/Stop Bit : D0 -> 1                          // D7~D4  D3~D0
            nAcSyncSECommand |= IMG_CAP_START_HIGH;                      // 0000   0001

            // Scan Mode 에 따른 Scan Size 설정
            if      (nSM == RED_SCAN_MODE   ) nAcSyncSECommand |= 0x10;  // 0001   0001
            else if (nSM == FAST_SCAN_MODE  ) nAcSyncSECommand |= 0x50;  // 0101   0001
            else if (nSM == SLOW1_SCAN_MODE ) nAcSyncSECommand |= 0x50;  // 0101   0001
            else if (nSM == SLOW2_SCAN_MODE ) nAcSyncSECommand |= 0x50;  // 0101   0001
            else if (nSM == PHOTO1_SCAN_MODE) nAcSyncSECommand |= 0x50;  // 0101   0001
            else if (nSM == PHOTO2_SCAN_MODE) nAcSyncSECommand |= 0x50;  // 0101   0001
            else if (nSM == PHOTO3_SCAN_MODE) nAcSyncSECommand |= 0x90;  // 1001   0001
            else if (nSM == PHOTO4_SCAN_MODE) nAcSyncSECommand |= 0x90;  // 1001   0001
            else if (nSM == PHOTO5_SCAN_MODE) nAcSyncSECommand |= 0x90;  // 1001   0001
            else if (nSM == PHOTO6_SCAN_MODE) nAcSyncSECommand |= 0xD0;  // 1101   0001

            // Se Command 전송
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_OUTPORT, nAcSyncSECommand);
            //
            //
            //

            //
            // Image Capture Start : Sbr Command 설정
            //

            // 스캔모드에 따른 SeCommand를 설정하여 적용한다
            int nAcSyncSBRCommand = 0;

            // Capture Start/Stop Bit : D0 -> 1                             // D19~16  D15~12  D11~8   D7~4   D3~0
            nAcSyncSBRCommand |= IMG_CAP_START_HIGH;                        // 0000    0000    0000    0000   0001

            // FRAME SYNC or LINE SYNC 설정
            /*
            if      (nSM == RED_SCAN_MODE   ) nAcSyncSBRCommand |= 0x1000;  // 0000    0001    0000    0000   0000     <-- FRAME SYNC
            else if (nSM == FAST_SCAN_MODE  ) nAcSyncSBRCommand |= 0x1000;  // 0000    0001    0000    0000   0000     <-- FRAME SYNC
            else if (nSM == SLOW1_SCAN_MODE ) nAcSyncSBRCommand |= 0x0000;  // 0000    0000    0000    0000   0000     <-- LINE  SYNC
            else if (nSM == SLOW2_SCAN_MODE ) nAcSyncSBRCommand |= 0x0000;  // 0000    0000    0000    0000   0000     <-- LINE  SYNC
            else if (nSM == PHOTO1_SCAN_MODE) nAcSyncSBRCommand |= 0x0000;  // 0000    0000    0000    0000   0000     <-- LINE  SYNC
            else if (nSM == PHOTO2_SCAN_MODE) nAcSyncSBRCommand |= 0x0000;  // 0000    0000    0000    0000   0000     <-- LINE  SYNC
            else if (nSM == PHOTO3_SCAN_MODE) nAcSyncSBRCommand |= 0x0000;  // 0000    0000    0000    0000   0000     <-- LINE  SYNC
            else if (nSM == PHOTO4_SCAN_MODE) nAcSyncSBRCommand |= 0x0000;  // 0000    0000    0000    0000   0000     <-- LINE  SYNC
            else if (nSM == PHOTO5_SCAN_MODE) nAcSyncSBRCommand |= 0x0000;  // 0000    0000    0000    0000   0000     <-- LINE  SYNC
            else if (nSM == PHOTO6_SCAN_MODE) nAcSyncSBRCommand |= 0x0000;  // 0000    0000    0000    0000   0000     <-- LINE  SYNC
            */
            // 2020.10.14
            if (nFrameSync == 1)
            {
                // FRAME SYNC                 // D19~16  D15~12  D11~8   D7~4   D3~0
                nAcSyncSBRCommand |= 0x1000;  // 0000    0001    0000    0000   0000     <-- FRAME SYNC
            }
            else
            {
                // LINE  SYNC                 // D19~16  D15~12  D11~8   D7~4   D3~0
                nAcSyncSBRCommand |= 0x0000;  // 0000    0000    0000    0000   0000     <-- LINE  SYNC
            }

            // 스캔모드에 따른 SYNC FREQ 설정 
            // SYNC_3_75HZ  0x000000   // 0 : 0000  0000 0000  0000 0000
            // SYNC_7_5HZ   0x010000   // 1 : 0001  0000 0000  0000 0000
            // SYNC_15HZ    0x020000   // 2 : 0010  0000 0000  0000 0000
            // SYNC_30HZ    0x030000   // 3 : 0011  0000 0000  0000 0000
            // 
            // SYNC_60HZ    0x040000   // 4 : 0100  0000 0000  0000 0000
            // SYNC_120HZ   0x050000   // 5 : 0101  0000 0000  0000 0000
            // SYNC_240HZ   0x060000   // 6 : 0110  0000 0000  0000 0000
            // SYNC_480HZ   0x070000   // 7 : 0111  0000 0000  0000 0000
            // 
            // SYNC_960HZ   0x080000   // 8 : 1000  0000 0000  0000 0000
            // SYNC_1920HZ  0x090000   // 9 : 1001  0000 0000  0000 0000
            // SYNC_3840HZ  0x0A0000   // A : 1010  0000 0000  0000 0000
            /*
            if      (nSM == RED_SCAN_MODE   ) nAcSyncSBRCommand |= 0x20000;  // 0010    0000    0000    0000   0000     <-- 15  [Hz]
            else if (nSM == FAST_SCAN_MODE  ) nAcSyncSBRCommand |= 0x10000;  // 0001    0000    0000    0000   0000     <-- 7.5 [Hz]
            else if (nSM == SLOW1_SCAN_MODE ) nAcSyncSBRCommand |= 0x60000;  // 0110    0000    0000    0000   0000     <-- 240 [Hz]
            else if (nSM == SLOW2_SCAN_MODE ) nAcSyncSBRCommand |= 0x40000;  // 0100    0000    0000    0000   0000     <-- 60  [Hz]
            else if (nSM == PHOTO1_SCAN_MODE) nAcSyncSBRCommand |= 0x30000;  // 0011    0000    0000    0000   0000     <-- 30  [Hz]
            else if (nSM == PHOTO2_SCAN_MODE) nAcSyncSBRCommand |= 0x20000;  // 0010    0000    0000    0000   0000     <-- 15  [Hz]
            else if (nSM == PHOTO3_SCAN_MODE) nAcSyncSBRCommand |= 0x30000;  // 0011    0000    0000    0000   0000     <-- 30  [Hz]
            else if (nSM == PHOTO4_SCAN_MODE) nAcSyncSBRCommand |= 0x20000;  // 0010    0000    0000    0000   0000     <-- 15  [Hz]
            else if (nSM == PHOTO5_SCAN_MODE) nAcSyncSBRCommand |= 0x10000;  // 0001    0000    0000    0000   0000     <-- 7.5 [Hz]
            else if (nSM == PHOTO6_SCAN_MODE) nAcSyncSBRCommand |= 0x20000;  // 0010    0000    0000    0000   0000     <-- 15  [Hz]
            */
            // 2020.10.14
            if (nFrameSync == 1)
            {
                double dFrame = dFrameFreq * 100.0;
                int    nFrame = (int)dFrame;

                // FRAME SYNC                                            // D19~16  D15~12  D11~8   D7~4   D3~0
                if      (nFrame == 375 ) nAcSyncSBRCommand |= 0x000000;  // 0000    0000    0000    0000   0000     <-- 3.75 [Hz]
                else if (nFrame == 750 ) nAcSyncSBRCommand |= 0x010000;  // 0001    0000    0000    0000   0000     <-- 7.5  [Hz]
                else if (nFrame == 1500) nAcSyncSBRCommand |= 0x020000;  // 0010    0000    0000    0000   0000     <-- 15   [Hz]
                else if (nFrame == 3000) nAcSyncSBRCommand |= 0x030000;  // 0011    0000    0000    0000   0000     <-- 30   [Hz]
                else if (nFrame == 6000) nAcSyncSBRCommand |= 0x040000;  // 0100    0000    0000    0000   0000     <-- 60   [Hz]
                else                     nAcSyncSBRCommand |= 0x020000;  // 0010    0000    0000    0000   0000     <-- 15   [Hz]

            }
            else
            {
                double dLine = dLineFreq * 100.0;
                int    nLine = (int)dLine;

                // LINE  SYNC                                             // D19~16  D15~12  D11~8   D7~4   D3~0
                if      (nLine == 750   ) nAcSyncSBRCommand |= 0x010000;  // 0001    0000    0000    0000   0000     <-- 7.5  [Hz]
                else if (nLine == 1500  ) nAcSyncSBRCommand |= 0x020000;  // 0010    0000    0000    0000   0000     <-- 15   [Hz]
                else if (nLine == 3000  ) nAcSyncSBRCommand |= 0x030000;  // 0011    0000    0000    0000   0000     <-- 30   [Hz]
                else if (nLine == 6000  ) nAcSyncSBRCommand |= 0x040000;  // 0100    0000    0000    0000   0000     <-- 60   [Hz]
                else if (nLine == 12000 ) nAcSyncSBRCommand |= 0x050000;  // 0101    0000    0000    0000   0000     <-- 120  [Hz]
                else if (nLine == 24000 ) nAcSyncSBRCommand |= 0x060000;  // 0110    0000    0000    0000   0000     <-- 240  [Hz]
                else if (nLine == 48000 ) nAcSyncSBRCommand |= 0x070000;  // 0111    0000    0000    0000   0000     <-- 480  [Hz]
                else if (nLine == 96000 ) nAcSyncSBRCommand |= 0x080000;  // 1000    0000    0000    0000   0000     <-- 960  [Hz]
                else if (nLine == 192000) nAcSyncSBRCommand |= 0x090000;  // 1001    0000    0000    0000   0000     <-- 1920 [Hz]
                else if (nLine == 384000) nAcSyncSBRCommand |= 0x0A0000;  // 1010    0000    0000    0000   0000     <-- 3840 [Hz]
                else                      nAcSyncSBRCommand |= 0x020000;  // 0010    0000    0000    0000   0000     <-- 15   [Hz]
            }

            //
            //
            //

            // AC SYNC ON/OFF 설정
            if (bAcSyncFlag == TRUE)
            {
                // Ac Sync On
                nAcSyncSBRCommand |= SYNC_SCAN_ON_HIGH;
            }
            else
            {
                // Ac Sync Off
                nAcSyncSBRCommand &= SYNC_SCAN_ON_LOW;
            }

            // Sbr Command 전송
            AfxFT232HPortSendCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, cWR_OUTPORT, nAcSyncSBRCommand);

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


            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:20868 ----
        //
        // Integrated Control : Scan Image Data Capture 작업 중지
        //
        public int AfxIntegratedAcSyncScanImageCaptureStop()
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


            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:20891 ----
        //
        // Integrated Control : Scan Generation (스캔모드에 따라 이미지 스캔을 위한 신호 생성)
        //
        public int AfxIntegratedSetAcSyncScanGeneration(int nAcSyncScanMode, int nFrameSync, double dFrameFreq, int nScanSkip, int nOverSample, int nXScanUp, int nXScanDown, int nYScanUp, int nYScanDown, int nScanClock)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 스캔모드
            int nSM = 0;
            if      (nAcSyncScanMode == 0) nSM = RED_SCAN_MODE;
            else if (nAcSyncScanMode == 1) nSM = FAST_SCAN_MODE;
            else if (nAcSyncScanMode == 2) nSM = SLOW1_SCAN_MODE;
            else if (nAcSyncScanMode == 3) nSM = SLOW2_SCAN_MODE;
            else if (nAcSyncScanMode == 4) nSM = PHOTO1_SCAN_MODE;
            else if (nAcSyncScanMode == 5) nSM = PHOTO2_SCAN_MODE;
            else if (nAcSyncScanMode == 6) nSM = PHOTO3_SCAN_MODE;
            else if (nAcSyncScanMode == 7) nSM = PHOTO4_SCAN_MODE;
            else if (nAcSyncScanMode == 8) nSM = PHOTO5_SCAN_MODE;
            else if (nAcSyncScanMode == 9) nSM = PHOTO6_SCAN_MODE;
            else return 0;

            //
            //
            //


            ushort[] wComBuf = NewArray<ushort>(TMP_STRING_LENGTH+1);
            ZeroMem(wComBuf);

            int nAcSyncScanClock = 0;
            int nPhaseInc = 0;

            // SCAN CLOCK [Hz] 계산
            // FRAME FREQ : 30[Hz], 15[Hz], 7.5[Hz], 3.75[Hz]...
            //double dFrame = (double)(nXScanUp + nXScanDown) * (double)(nYScanUp + nYScanDown) * dFrameFreq;

            // SCAN CLOCK [Hz]
            nAcSyncScanClock = nScanClock;

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
                long nPhase_inc64 = 0;
                nPhase_inc64 = (long)(nAcSyncScanClock * (Exp2E48 / 100000000));
                wComBuf[0] = (ushort)(cWR_FDDS_LOW);          wComBuf[1] = (ushort)( (ushort)(nPhase_inc64 & 0x000000000000FFFF)); nPhase_inc64 = nPhase_inc64 >> 16;
                wComBuf[2] = (ushort)(cWR_FDDS_HIGH);      wComBuf[3] = (ushort)( (ushort)(nPhase_inc64 & 0x000000000000FFFF)); nPhase_inc64 = nPhase_inc64 >> 16;
                wComBuf[4] = (ushort)(cWR_FDDS_HIGH48);    wComBuf[5] = (ushort)( (ushort)(nPhase_inc64 & 0x000000000000FFFF));
                //--
                AfxFT232HPortSendMultiCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, wComBuf, 6); // Sbr Scan Clock Data 전송
                //--
            }
            //
            //
            //
            else
            {
                // SCAN CLOCK 32Bits, 48MHz
                nPhaseInc = (int)(nAcSyncScanClock * Exp2E32 / PLL_SCAN_CLOCK);
                ZeroMem(wComBuf);
                //--
                wComBuf[0] = (ushort)(cWR_FDDS_LOW);      wComBuf[1] = (ushort)(nPhaseInc & 0x0000FFFF);
                wComBuf[2] = (ushort)(cWR_FDDS_HIGH);     wComBuf[3] = (ushort)(nPhaseInc >> 16);
                //--
                // SBR 명령 전송
                AfxFT232HPortSendMultiCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, wComBuf, 4);
                //--
            }
            //
            //
            //

            ZeroMem(wComBuf);
            //--
            wComBuf[0] = (ushort)(cOS_NUM_LOAD);      wComBuf[1] = (ushort)(nOverSample);
            wComBuf[2] = (ushort)(cSKIP_NUM_LOAD);    wComBuf[3] = (ushort)(nScanSkip);
            // Smg 명령 전송
            AfxFT232HPortSendMultiCommand(IDX_FT232H_SEM_IMAGE_DEVICE, wComBuf, 4);
            //--


            //--
            //--
            //--

            int nIncVal    = 0;
            int nIncHeight = 0;
            int nLowVal    = 0;
            int nHighVal   = 0;
            int nDecVal    = 0;

            ZeroMem(wComBuf);

            //--
            // XScan 기본파형 발생
            //--
            if (nXScanUp > 0 && nXScanDown > 0)
            {
                nIncVal    = (4096 / nXScanUp);
                nIncHeight = nIncVal * nXScanUp;
                nLowVal    = (4096 - nIncHeight) / 2;
                nHighVal   = (4096 - nLowVal);
                nDecVal    = nIncHeight / nXScanDown;
            }

            wComBuf[0] = (ushort)(cWR_XSCAN_LOW);       wComBuf[1] = (ushort)(nLowVal);
            wComBuf[2] = (ushort)(cWR_XSCAN_HIGH);      wComBuf[3] = (ushort)(nHighVal);
            wComBuf[4] = (ushort)(cWR_XSCAN_INC_VAL);   wComBuf[5] = (ushort)(nIncVal);
            wComBuf[6] = (ushort)(cWR_XSCAN_DEC_VAL);   wComBuf[7] = (ushort)(nDecVal);
            wComBuf[8] = (ushort)(cWR_XSCAN_INC_PIX);   wComBuf[9] = (ushort)(nXScanUp);
            wComBuf[10] = (ushort)(cWR_XSCAN_DEC_PIX);   wComBuf[11] = (ushort)(nXScanDown);
            //--

            nIncVal    = 0;
            nIncHeight = 0;
            nLowVal    = 0;
            nHighVal   = 0;
            nDecVal    = 0;
            //--
            // YScan 기본파형 발생
            //--
            if (nYScanUp > 0 && nYScanDown > 0)
            {
                nIncVal    = (4096 / nYScanUp);
                nIncHeight = nIncVal * nYScanUp;
                nLowVal    = (4096 - nIncHeight) / 2;
                nHighVal   = (4096 - nLowVal);
                nDecVal    = nIncHeight / nYScanDown;
            }

            wComBuf[12] = (ushort)(cWR_YSCAN_LOW);       wComBuf[13] = (ushort)(nLowVal);
            wComBuf[14] = (ushort)(cWR_YSCAN_HIGH);      wComBuf[15] = (ushort)(nHighVal);
            wComBuf[16] = (ushort)(cWR_YSCAN_INC_VAL);   wComBuf[17] = (ushort)(nIncVal);
            wComBuf[18] = (ushort)(cWR_YSCAN_DEC_VAL);   wComBuf[19] = (ushort)(nDecVal);
            wComBuf[20] = (ushort)(cWR_YSCAN_INC_PIX);   wComBuf[21] = (ushort)(nYScanUp);
            wComBuf[22] = (ushort)(cWR_YSCAN_DEC_PIX);   wComBuf[23] = (ushort)(nYScanDown);  //  128
            //--

            //--
            // SBR 명령 전송
            AfxFT232HPortSendMultiCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, wComBuf, 24);
            //--

            //--
            //--
            //--

            //--
            // X, YScan의 Amplitude 와 Offset 설정 ( "SR6","SR7","SR8","SR9" )
            //--
            AfxScanBridgeSetXImageAmpApply(m_pIntegrated.nXScanAmpVal[nSM]);     // XScanAmp
            AfxScanBridgeSetXImageOffsetApply(m_pIntegrated.nXScanOffset[nSM]);  // XScanOffset
            AfxScanBridgeSetYImageAmpApply(m_pIntegrated.nYScanAmpVal[nSM]);     // YScanAmp
            AfxScanBridgeSetYImageOffsetApply(m_pIntegrated.nYScanOffset[nSM]);  // YScanOffset
            //--

            //--
            //--
            //--

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:21060 ----
        //
        // Integrated Control : Scan Generation 초기화 (스캔신호 OFF)
        //
        public int AfxIntegratedResetAcSyncScanGeneration()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            ushort[] wComBuf = NewArray<ushort>(TMP_STRING_LENGTH + 1);

            ZeroMem(wComBuf);
            //--
            wComBuf[0] = (ushort)(cWR_FDDS_LOW);      wComBuf[1] = (ushort)(0);
            wComBuf[2] = (ushort)(cWR_FDDS_HIGH);     wComBuf[3] = (ushort)(0);
            //--
            // SBR 명령 전송
            AfxFT232HPortSendMultiCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, wComBuf, 4);
            //--

            ZeroMem(wComBuf);
            //--
            wComBuf[0] = (ushort)(cOS_NUM_LOAD);      wComBuf[1] = (ushort)(0);
            wComBuf[2] = (ushort)(cSKIP_NUM_LOAD);    wComBuf[3] = (ushort)(0);
            // Smg 명령 전송
            AfxFT232HPortSendMultiCommand(IDX_FT232H_SEM_IMAGE_DEVICE, wComBuf, 4);
            //--

            //--
            //--
            //--

            ZeroMem(wComBuf);

            //--
            // XScan 기본파형 발생
            //--
            wComBuf[0] = (ushort)(cWR_XSCAN_LOW);       wComBuf[1] = (ushort)(0);
            wComBuf[2] = (ushort)(cWR_XSCAN_HIGH);      wComBuf[3] = (ushort)(0);
            wComBuf[4] = (ushort)(cWR_XSCAN_INC_VAL);   wComBuf[5] = (ushort)(0);
            wComBuf[6] = (ushort)(cWR_XSCAN_DEC_VAL);   wComBuf[7] = (ushort)(0);
            wComBuf[8] = (ushort)(cWR_XSCAN_INC_PIX);   wComBuf[9] = (ushort)(0);
            wComBuf[10] = (ushort)(cWR_XSCAN_DEC_PIX);   wComBuf[11] = (ushort)(0);
            //--

            //--
            // YScan 기본파형 발생
            //--
            wComBuf[12] = (ushort)(cWR_YSCAN_LOW);       wComBuf[13] = (ushort)(0);
            wComBuf[14] = (ushort)(cWR_YSCAN_HIGH);      wComBuf[15] = (ushort)(0);
            wComBuf[16] = (ushort)(cWR_YSCAN_INC_VAL);   wComBuf[17] = (ushort)(0);
            wComBuf[18] = (ushort)(cWR_YSCAN_DEC_VAL);   wComBuf[19] = (ushort)(0);
            wComBuf[20] = (ushort)(cWR_YSCAN_INC_PIX);   wComBuf[21] = (ushort)(0);
            wComBuf[22] = (ushort)(cWR_YSCAN_DEC_PIX);   wComBuf[23] = (ushort)(0);
            //--

            //--
            // SBR 명령 전송
            AfxFT232HPortSendMultiCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, wComBuf, 24);
            //--

            //--
            // X, YScan의 Amplitude 와 Offset 설정 ( "SR6","SR7","SR8","SR9" )
            //--
            AfxScanBridgeSetXImageAmpApply(0);        // "SR6" : XScanAmp
            AfxScanBridgeSetXImageOffsetApply(2048);  // "SR7" : XScanOffset
            AfxScanBridgeSetYImageAmpApply(0);        // "SR8" : YScanAmp
            AfxScanBridgeSetYImageOffsetApply(2048);  // "SR9" : YScanOffset
            //--

            //--
            //--
            //--

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:21136 ----
        //
        // Integrated Control : CL LMH(LOW,MIDDLE,HIGH) MODE SELECT DATA 설정
        //
        public int AfxIntegratedCLLMHModeSelectDataSet(int nCLLMHMode, int nCLLMHLow123Mode, int nCLLMHMiddle123Mode, int nCLLMHHigh123Mode)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            
            // CL LMH(LOW,MIDDLE,HIGH) MODE DATA
            // CL LMH MODE 선택값 설정
            m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MODE_CALI_SELECT]   = nCLLMHMode;
            // CL LMH MODE : LOW    1/2/3 선택값 설정
            m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW_CALI_SELECT]    = nCLLMHLow123Mode;
            // CL LMH MODE : MIDDLE 1/2/3 선택값 설정
            m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE_CALI_SELECT] = nCLLMHMiddle123Mode;
            // CL LMH MODE : HIGH   1/2/3 선택값 설정
            m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH_CALI_SELECT]   = nCLLMHHigh123Mode;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:21156 ----
        //
        // Integrated Control : CL LMH(LOW,MIDDLE,HIGH) MODE VALUE DATA 설정
        //
        public int AfxIntegratedCLLMHModeValueDataSet(int nCLLMHMode, int nCLLMH123Mode, int nCL1, int nCL2, int nGAX, int nGAY, int nBAX, int nBAY)
        {
            if (m_pIntegrated == null)
                return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // CL LMH(LOW,MIDDLE,HIGH) MODE VALUE DATA

            // CL LMH MODE 선택값 : LOW MODE
            if (nCLLMHMode == IDX_CL_LMH_LOW_MODE)
            {
                if (nCLLMH123Mode == IDX_CL_LMH_MODE_1_SELECT)
                {
                    // CL LMH MODE : LOW1 CL1 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW1_CL1_VALUE] = nCL1;
                    // CL LMH MODE : LOW1 CL2 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW1_CL2_VALUE] = nCL2;
                    // CL LMH MODE : LOW1 GAX 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW1_GAX_VALUE] = nGAX;
                    // CL LMH MODE : LOW1 GAY 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW1_GAY_VALUE] = nGAY;
                    // CL LMH MODE : LOW1 BAX 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW1_BAX_VALUE] = nBAX;
                    // CL LMH MODE : LOW1 BAY 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW1_BAY_VALUE] = nBAY;
                }
                else if (nCLLMH123Mode == IDX_CL_LMH_MODE_2_SELECT)
                {
                    // CL LMH MODE : LOW2 CL1 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW2_CL1_VALUE] = nCL1;
                    // CL LMH MODE : LOW2 CL2 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW2_CL2_VALUE] = nCL2;
                    // CL LMH MODE : LOW2 GAX 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW2_GAX_VALUE] = nGAX;
                    // CL LMH MODE : LOW2 GAY 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW2_GAY_VALUE] = nGAY;
                    // CL LMH MODE : LOW2 BAX 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW2_BAX_VALUE] = nBAX;
                    // CL LMH MODE : LOW2 BAY 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW2_BAY_VALUE] = nBAY;
                }
                else if (nCLLMH123Mode == IDX_CL_LMH_MODE_3_SELECT)
                {
                    // CL LMH MODE : LOW3 CL1 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW3_CL1_VALUE] = nCL1;
                    // CL LMH MODE : LOW3 CL2 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW3_CL2_VALUE] = nCL2;
                    // CL LMH MODE : LOW3 GAX 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW3_GAX_VALUE] = nGAX;
                    // CL LMH MODE : LOW3 GAY 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW3_GAY_VALUE] = nGAY;
                    // CL LMH MODE : LOW3 BAX 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW3_BAX_VALUE] = nBAX;
                    // CL LMH MODE : LOW3 BAY 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW3_BAY_VALUE] = nBAY;
                }
            }
            // CL LMH MODE 선택값 : MIDDLE MODE
            else if (nCLLMHMode == IDX_CL_LMH_MIDDLE_MODE)
            {
                if (nCLLMH123Mode == IDX_CL_LMH_MODE_1_SELECT)
                {
                    // CL LMH MODE : MIDDLE1 CL1 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE1_CL1_VALUE] = nCL1;
                    // CL LMH MODE : MIDDLE1 CL2 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE1_CL2_VALUE] = nCL2;
                    // CL LMH MODE : MIDDLE1 GAX 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE1_GAX_VALUE] = nGAX;
                    // CL LMH MODE : MIDDLE1 GAY 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE1_GAY_VALUE] = nGAY;
                    // CL LMH MODE : MIDDLE1 BAX 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE1_BAX_VALUE] = nBAX;
                    // CL LMH MODE : MIDDLE1 BAY 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE1_BAY_VALUE] = nBAY;
                }
                else if (nCLLMH123Mode == IDX_CL_LMH_MODE_2_SELECT)
                {
                    // CL LMH MODE : MIDDLE2 CL1 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE2_CL1_VALUE] = nCL1;
                    // CL LMH MODE : MIDDLE2 CL2 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE2_CL2_VALUE] = nCL2;
                    // CL LMH MODE : MIDDLE2 GAX 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE2_GAX_VALUE] = nGAX;
                    // CL LMH MODE : MIDDLE2 GAY 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE2_GAY_VALUE] = nGAY;
                    // CL LMH MODE : MIDDLE2 BAX 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE2_BAX_VALUE] = nBAX;
                    // CL LMH MODE : MIDDLE2 BAY 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE2_BAY_VALUE] = nBAY;
                }
                else if (nCLLMH123Mode == IDX_CL_LMH_MODE_3_SELECT)
                {
                    // CL LMH MODE : MIDDLE3 CL1 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE3_CL1_VALUE] = nCL1;
                    // CL LMH MODE : MIDDLE3 CL2 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE3_CL2_VALUE] = nCL2;
                    // CL LMH MODE : MIDDLE3 GAX 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE3_GAX_VALUE] = nGAX;
                    // CL LMH MODE : MIDDLE3 GAY 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE3_GAY_VALUE] = nGAY;
                    // CL LMH MODE : MIDDLE3 BAX 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE3_BAX_VALUE] = nBAX;
                    // CL LMH MODE : MIDDLE3 BAY 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE3_BAY_VALUE] = nBAY;
                }
            }
            // CL LMH MODE 선택값 : HIGH MODE
            else if (nCLLMHMode == IDX_CL_LMH_HIGH_MODE)
            {
                if (nCLLMH123Mode == IDX_CL_LMH_MODE_1_SELECT)
                {
                    // CL LMH MODE : HIGH1 CL1 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH1_CL1_VALUE] = nCL1;
                    // CL LMH MODE : HIGH1 CL2 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH1_CL2_VALUE] = nCL2;
                    // CL LMH MODE : HIGH1 GAX 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH1_GAX_VALUE] = nGAX;
                    // CL LMH MODE : HIGH1 GAY 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH1_GAY_VALUE] = nGAY;
                    // CL LMH MODE : HIGH1 BAX 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH1_BAX_VALUE] = nBAX;
                    // CL LMH MODE : HIGH1 BAY 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH1_BAY_VALUE] = nBAY;
                }
                else if (nCLLMH123Mode == IDX_CL_LMH_MODE_2_SELECT)
                {
                    // CL LMH MODE : HIGH2 CL1 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH2_CL1_VALUE] = nCL1;
                    // CL LMH MODE : HIGH2 CL2 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH2_CL2_VALUE] = nCL2;
                    // CL LMH MODE : HIGH2 GAX 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH2_GAX_VALUE] = nGAX;
                    // CL LMH MODE : HIGH2 GAY 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH2_GAY_VALUE] = nGAY;
                    // CL LMH MODE : HIGH2 BAX 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH2_BAX_VALUE] = nBAX;
                    // CL LMH MODE : HIGH2 BAY 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH2_BAY_VALUE] = nBAY;
                }
                else if (nCLLMH123Mode == IDX_CL_LMH_MODE_3_SELECT)
                {
                    // CL LMH MODE : HIGH3 CL1 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH3_CL1_VALUE] = nCL1;
                    // CL LMH MODE : HIGH3 CL2 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH3_CL2_VALUE] = nCL2;
                    // CL LMH MODE : HIGH3 GAX 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH3_GAX_VALUE] = nGAX;
                    // CL LMH MODE : HIGH3 GAY 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH3_GAY_VALUE] = nGAY;
                    // CL LMH MODE : HIGH3 BAX 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH3_BAX_VALUE] = nBAX;
                    // CL LMH MODE : HIGH3 BAY 값 설정
                    m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH3_BAY_VALUE] = nBAY;
                }
            }

            return RET_SUCCESS_OK;
        }

    }
}
