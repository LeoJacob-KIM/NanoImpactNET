// 원본 USB_FT232H_Device.cpp:14996-17159 을 NanoImpact.CodeGen translate로 옮긴 뒤 손으로 고친 코드.
#pragma warning disable IDE1006, IDE0059, CS0162, CS0168, CS0219
using System;
using NanoImpact.Core.Native;
using static NanoImpact.Core.Def;
using static NanoImpact.Core.Native.CRuntime;

namespace NanoImpact.Hardware.Device
{
    public sealed partial class SemDevice
    {
        // ---- USB_FT232H_Device.cpp:14996 ----
        //
        // Integrated Control : Scan Generation (스캔모드에 따라 이미지 스캔을 위한 신호 생성)
        //
        public int AfxIntegratedSetScanGeneration(int nSM)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            ushort[] wComBuf = NewArray<ushort>(TMP_STRING_LENGTH+1);
            ZeroMem(wComBuf);

            //------------------
            // DDS에 의한 ScanClk 발생
            //------------------
            int nScanClock = 0;
            int nPhaseInc  = 0;
            double dFrame  = 0.0;

            dFrame = (double)m_pIntegrated.nFrameSec[nSM] * 0.001;
            dFrame = (double)(m_pIntegrated.nXScanUpNum[nSM] + m_pIntegrated.nXScanDownNum[nSM]) * (double)(m_pIntegrated.nYScanUpNum[nSM] + m_pIntegrated.nYScanDownNum[nSM]) * dFrame;
            nScanClock = (int)dFrame;

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
                nPhase_inc64 = (long)(nScanClock * (Exp2E48 / 100000000));
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
                nPhaseInc = (int)(nScanClock * Exp2E32 / PLL_SCAN_CLOCK); //(int)((double)(nScanClock * Exp2E32 / PLL_SCAN_CLOCK));
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

            // 잠시대기
            //Sleep(100);

            ZeroMem(wComBuf);
            //--
            wComBuf[0] = (ushort)(cOS_NUM_LOAD);      wComBuf[1] = (ushort)(m_pIntegrated.nOverSampleNum[nSM]);
            wComBuf[2] = (ushort)(cSKIP_NUM_LOAD);    wComBuf[3] = (ushort)(m_pIntegrated.nSkipPixNum[nSM]);
            // Smg 명령 전송
            AfxFT232HPortSendMultiCommand(IDX_FT232H_SEM_IMAGE_DEVICE, wComBuf, 4);
            //--

            // 잠시대기
            //Sleep(100);

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
            if (m_pIntegrated.nXScanUpNum[nSM] > 0 && m_pIntegrated.nXScanDownNum[nSM] > 0)
            {
                nIncVal    = 4096 / m_pIntegrated.nXScanUpNum[nSM];
                nIncHeight = nIncVal * m_pIntegrated.nXScanUpNum[nSM];
                nLowVal    = (4096 - nIncHeight) / 2;
                nHighVal   = 4096 - nLowVal;
                nDecVal    = nIncHeight / m_pIntegrated.nXScanDownNum[nSM];
            }

            wComBuf[0] = (ushort)(cWR_XSCAN_LOW);       wComBuf[1] = (ushort)(nLowVal);
            wComBuf[2] = (ushort)(cWR_XSCAN_HIGH);      wComBuf[3] = (ushort)(nHighVal);
            wComBuf[4] = (ushort)(cWR_XSCAN_INC_VAL);   wComBuf[5] = (ushort)(nIncVal);
            wComBuf[6] = (ushort)(cWR_XSCAN_DEC_VAL);   wComBuf[7] = (ushort)(nDecVal);
            wComBuf[8] = (ushort)(cWR_XSCAN_INC_PIX);   wComBuf[9] = (ushort)(m_pIntegrated.nXScanUpNum[nSM]);
            wComBuf[10] = (ushort)(cWR_XSCAN_DEC_PIX);   wComBuf[11] = (ushort)(m_pIntegrated.nXScanDownNum[nSM]);
            //--

            nIncVal    = 0;
            nIncHeight = 0;
            nLowVal    = 0;
            nHighVal   = 0;
            nDecVal    = 0;
            //--
            // YScan 기본파형 발생
            //--
            if (m_pIntegrated.nYScanUpNum[nSM] > 0 && m_pIntegrated.nYScanDownNum[nSM] > 0)
            {
                nIncVal    = 4096 / m_pIntegrated.nYScanUpNum[nSM];
                nIncHeight = nIncVal * m_pIntegrated.nYScanUpNum[nSM];
                nLowVal    = (4096 - nIncHeight) / 2;
                nHighVal   = 4096 - nLowVal;
                nDecVal    = nIncHeight / m_pIntegrated.nYScanDownNum[nSM];
            }

            wComBuf[12] = (ushort)(cWR_YSCAN_LOW);       wComBuf[13] = (ushort)(nLowVal);
            wComBuf[14] = (ushort)(cWR_YSCAN_HIGH);      wComBuf[15] = (ushort)(nHighVal);
            wComBuf[16] = (ushort)(cWR_YSCAN_INC_VAL);   wComBuf[17] = (ushort)(nIncVal);
            wComBuf[18] = (ushort)(cWR_YSCAN_DEC_VAL);   wComBuf[19] = (ushort)(nDecVal);
            wComBuf[20] = (ushort)(cWR_YSCAN_INC_PIX);   wComBuf[21] = (ushort)(m_pIntegrated.nYScanUpNum[nSM]);    // 2048
            wComBuf[22] = (ushort)(cWR_YSCAN_DEC_PIX);   wComBuf[23] = (ushort)(m_pIntegrated.nYScanDownNum[nSM]);  //  128
            //--

            //--
            // SBR 명령 전송
            AfxFT232HPortSendMultiCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, wComBuf, 24);
            //--

            // 잠시대기
            //Sleep(50);

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

            // 잠시대기
            //Sleep(50);

            //--
            // BSE Board가 연결된 경우
            //--
            // 2021.07.23 : 별도의 BSE 이미지 보드가 없기 때문에 아래 기능은 사용하지 않는다.
            /*
            if (m_pIntegrated->bBseActiveFlag == TRUE)
            {
                WORD wBseBuf[CMD_STRING_LENGTH+1];

                wBseBuf[0] = bOS_NUM_LOAD;    wBseBuf[1] = m_pIntegrated->nOverSampleNum[nSM];   // not use
                wBseBuf[2] = bSKIP_NUM_LOAD;  wBseBuf[3] = m_pIntegrated->nSkipPixNum[nSM];
                AfxFT232HPortSendMultiCommand(IDX_FT232H_BSE_IMAGE_DEVICE, wBseBuf, 4);
            }
            */
            //--


            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:15175 ----
        //
        // Integrated Control : Scan Generation 초기화 (스캔신호 OFF)
        //
        public int AfxIntegratedResetScanGeneration()
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
            //--
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

        // ---- USB_FT232H_Device.cpp:15267 ----
        //
        // IniPar.MicronBarMag   IniPar.dMicronBarSize    IniPar.MicronBarPixelNum
        //
        //           x100            100 um                      100 Pixel
        //           x500             20 um
        //           x1.0k            10 um
        //           x2.0k             5 um
        //           x10k              1 um
        //           x20k            0.5 um  (500 nm)
        //           x50k            0.2 um  (200 nm)
        //           x100k           0.1 um  (100 nm)
        //
        //
        // Integrated Control : Scan Image Data Capture 작업 중지
        //
        public int AfxIntegratedSetMagnification(int nMag, int nWD10)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 배율확인
            if (nMag == 0) return RET_ERROR_STATUS;

            //
            // 2021.06.10
            // 사용자 MAG HV RATIO : STAGE 수평 옵션을 임시로 사용한다.
            //
            if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_STAGE_HORZ_ALIGN] == 1)
            {
                double dMaxGunHV = m_nGunHVTable[0];                           // MAX GUN HV VOLTAGE : m_nGunHVTable[0];
                double dGunHV = m_nGunHVTable[m_pIntegrated.nGunHVIndex];  // Gun HV Voltage
                // GUN HV : 20KV
                if (m_nGunHVTable[m_pIntegrated.nGunHVIndex] == 20000)
                {
                    m_pIntegrated.dMagHVRatio = sqrt(dGunHV / dMaxGunHV) / DEFAULT_MAG_HV_RATIO;
                }
                else
                {
                    // 사용자 설정 배율 HV RATIO 보정값 설정
                    m_pIntegrated.dMagHVRatio = m_dOptionMagHVRatio[m_pIntegrated.nGunHVIndex];
                }
            }
            //

            double dRatio = 0.0;

            // 배율에 따른 릴레이 동작(설정)과 비율값 계산
            dRatio = AfxIntegratedFindMagRelayRatio(nMag);

            // Zoom Value 계산 및 동작
            m_pIntegrated.wZoomXValue = (ushort)( (ushort)((double)m_nZoomXMaxValue[m_pIntegrated.wMagRelayNumber] / dRatio));
            m_pIntegrated.wZoomYValue = (ushort)( (ushort)((double)m_nZoomYMaxValue[m_pIntegrated.wMagRelayNumber] / dRatio));
            // 2020.05.27
            // SCAN ROTATION CALIBRATION DATA 적용
            if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_ORTHOGONAL_CORRRECTION] == 1)
            {
                // 2020.05.26
                // SCAN ROTATION 보정값 적용시 최대값은 4095이다
                if (m_pIntegrated.wZoomXValue > 4095) m_pIntegrated.wZoomXValue = (ushort)(4095);
                if (m_pIntegrated.wZoomYValue > 4095) m_pIntegrated.wZoomYValue = (ushort)(4095);
            }
            else
            {
                // 2020.05.26
                // ZOOM X-Y MAX값 설정
                if (m_pIntegrated.wZoomXValue > m_nZoomXMaxValue[m_pIntegrated.wMagRelayNumber]) m_pIntegrated.wZoomXValue = (ushort)(m_nZoomXMaxValue[m_pIntegrated.wMagRelayNumber]);
                if (m_pIntegrated.wZoomYValue > m_nZoomYMaxValue[m_pIntegrated.wMagRelayNumber]) m_pIntegrated.wZoomYValue = (ushort)(m_nZoomYMaxValue[m_pIntegrated.wMagRelayNumber]);
            }

            //
            // 2020.05.26
            // SCAN ROTATION : ORTHOGONAL, S-ROT 보정, ZOOM X-Y 보정
            //
            // ORTHOGONAL, ZOOM 보정이 선택 되었다면 --> SCAN ROTATION CALIBRATION DATA 적용
            if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_ORTHOGONAL_CORRRECTION] == 1)
            {
                //
                // SCAN ROTATION 보정을 사용하는 경우
                //

                // SCAN ROTATION 실행을 위해 OLD값 변경
                m_pIntegrated.nSRotationValueOld = m_pIntegrated.nSRotationValue + 1;
                // SCAN ROTATION
                AfxScanBridgeSetScanRotation(m_pIntegrated.nSRotationValue);

            }
            else
            {
                //
                // SCAN ROTATION 보정을 사용하지 않는 경우
                //
                // ZOOM SCALE 보정값
                double dWDZoomRatio = 1.0;

                //
                // 2020.01.17
                // ZOOM 보정
                //
                // 1) WD 변화에 따른 Zoom 보정
                if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_WD_SCALE_ZOOM] == 1)
                {
                    // WD SCALE ZOOM CORRECTION
                    dWDZoomRatio = m_pIntegrated.nWDZoomingLinear[nWD10] / 1000.0;
                }

                // 2) HV보정을 선택 했을 경우
                // MagHV_Ratio 값은 GUN HV 변경시 배율 보정을 위해 재계산 하는 값이다
                // 계산 : MagHV_Ratio = sqrt(GunHV_Table[GunHVIndex] / 30000.0) / 0.8165;
                // HV CORRECTION
                if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_HV_CORRECTION] == 1)
                {
                    dWDZoomRatio = dWDZoomRatio * m_pIntegrated.dMagHVRatio;
                }

                // Zoom 설정
                AfxIntegratedSetZoomValue(m_pIntegrated.wZoomXValue, m_pIntegrated.wZoomYValue, dWDZoomRatio);
            }

            // 배율값 적용
            // Avr에 Mag Scale 값을 알려줌 -> Motor 이동 Scale로 사용
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "MG", nMag); // Sbr Data 전송

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:15384 ----
        //
        // CommandSend( cWR_XZOOM, mag )를 수행한 후, CommandSend( cWR_YZOOM, mag )를
        // 수행하면, FX2 를 통해 FPGA에서 CommandSend 사이의 응답 시간은 약 100 usec 걸린다.
        // Interval 간격을 줄이기 위해 한꺼번에 Bundle로 보낸다.
        //
        //
        // Integrated Control : Zoom 설정하기
        //
        public int AfxIntegratedSetZoomValue(int nXData, int nYData, double dZoomRatio)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            int nXMag = 0;
            int nYMag = 0;

            double dFXData = 0.0;
            double dFYData = 0.0;

            dFXData = (double)(dZoomRatio * nXData);
            dFYData = (double)(dZoomRatio * nYData);

            // 200.05.22
            nXMag = (int)(dFXData); //(int)(dFXData+0.5);
            nYMag = (int)(dFYData); //(int)(dFYData+0.5);

            // Zoom X-Y Value Check : 0 ~ 4095
            if (nXMag < 0) nXMag = 0; if (nXMag > 4095) nXMag = 4095;
            if (nYMag < 0) nYMag = 0; if (nYMag > 4095) nYMag = 4095;

            // Zoom 설정
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "ZM0,", nXMag); // Sbr Data 전송
            AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "ZM1,", nYMag); // Sbr Data 전송

            //
            // Zoom X-Y 설정 및 실행값 CLIENT 전송
            //
            USB_DEVICE_DATA_INFO UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            
            // 2020.05.22
            m_pIntegrated.nZoomXValue = nXData;
            m_pIntegrated.nZoomYValue = nYData;

            // Zoom X-Y Value 처리
            UsbDeviceDataInfo.nState = 0;
            UsbDeviceDataInfo.nData[0] = m_pIntegrated.nZoomXValue;     // Zoom X
            UsbDeviceDataInfo.nData[1] = m_pIntegrated.nZoomYValue;     // Zoom Y
            UsbDeviceDataInfo.nData[2] = m_pIntegrated.wMagRelayNumber; // Zoom Relay Number

            // Zoom X-Y MESSAGE 보내기
            if (m_hOwnerWnd != NULL)
            {
                // Zoom X-Y 정보 메시지 보내기
                // 2020.05.04
                SendMessage(m_hOwnerWnd, UM_USB_SBR_ZOOM_XY_VALUE_DATA, NULL, UsbDeviceDataInfo);
            }
            //

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:15441 ----
        //
        // Integrated Control : 배율에 따른 릴레이 동작 및 비율 계산
        //
        public double AfxIntegratedFindMagRelayRatio(int nMag)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            double dRatio = 0.0;
            ushort   wMask  = 0xFFE0;

            // 2023.03.14
            // RESET : MAG RELAY IMAGE SHIFT XY CORRECT VALUE
            m_MagRelayImageShiftXValue = 0;
            m_MagRelayImageShiftYValue = 0;

            //
            // Mx100k Relay 사용금지
            //
            m_pIntegrated.wMagRelayNumber = (ushort)(0);

            // D3 : Mx10k -> 배율이 3만 보다 클 경우 #3 릴레이 동작
            if (nMag > 30000)
            {
                dRatio = (double)nMag / 30000.0;
                m_pIntegrated.wMagRelayNumber = (ushort)(3);

                // 2023.03.14 : MAG RELAY IMAGE SHIFT XY CORRECT 적용
                if (m_pClientPublicData != null)
                {
                    // MAG RELAY IMAGE SHIFT XY CORRECT 사용여부 확인
                    if (m_pClientPublicData.nOptionData[IDX_CLIENT_OPTION_INT_MAG_IMAGE_SHIFT_USED] == 1)
                    {
                        // MAG RELAY IMAGE SHIFT XY CORRECT 사용
                        m_MagRelayImageShiftXValue = m_pClientPublicData.nOptionData[IDX_CLIENT_OPTION_INT_MAG_IMAGE_SHIFT_X35000];
                        m_MagRelayImageShiftYValue = m_pClientPublicData.nOptionData[IDX_CLIENT_OPTION_INT_MAG_IMAGE_SHIFT_Y35000];
                    }
                }
            }
            // D2 : Mx1k -> 배율이 3천 보다 같거나 클 경우 #2 릴레이 동작
            else if (nMag >= 3000)
            {
                dRatio = (double)nMag / 3000.0;
                m_pIntegrated.wMagRelayNumber = (ushort)(2);

                // 2023.03.14 : MAG RELAY IMAGE SHIFT XY CORRECT 적용
                if (m_pClientPublicData != null)
                {
                    // MAG RELAY IMAGE SHIFT XY CORRECT 사용여부 확인
                    if (m_pClientPublicData.nOptionData[IDX_CLIENT_OPTION_INT_MAG_IMAGE_SHIFT_USED] == 1)
                    {
                        // MAG RELAY IMAGE SHIFT XY CORRECT 사용
                        m_MagRelayImageShiftXValue = m_pClientPublicData.nOptionData[IDX_CLIENT_OPTION_INT_MAG_IMAGE_SHIFT_X3000];
                        m_MagRelayImageShiftYValue = m_pClientPublicData.nOptionData[IDX_CLIENT_OPTION_INT_MAG_IMAGE_SHIFT_Y3000];
                    }
                }
            }
            // D1 : Mx100 -> 배율이 3백 보다 같거나 클 경우 #1 릴레이 동작
            else if (nMag >= 300)
            {
                dRatio = (double)nMag / 300.0;
                m_pIntegrated.wMagRelayNumber = (ushort)(1);

                // 2023.03.14 : MAG RELAY IMAGE SHIFT XY CORRECT 적용
                if (m_pClientPublicData != null)
                {
                    // MAG RELAY IMAGE SHIFT XY CORRECT 사용여부 확인
                    if (m_pClientPublicData.nOptionData[IDX_CLIENT_OPTION_INT_MAG_IMAGE_SHIFT_USED] == 1)
                    {
                        // MAG RELAY IMAGE SHIFT XY CORRECT 사용
                        m_MagRelayImageShiftXValue = m_pClientPublicData.nOptionData[IDX_CLIENT_OPTION_INT_MAG_IMAGE_SHIFT_X300];
                        m_MagRelayImageShiftYValue = m_pClientPublicData.nOptionData[IDX_CLIENT_OPTION_INT_MAG_IMAGE_SHIFT_Y300];
                    }
                }
            }
            // D0 : Mx10 -> 배율이 20 보다 같거나 클 경우 #0 릴레이 동작
            else if (nMag >= 20)
            {
                dRatio = (double)nMag / 30.0;
                m_pIntegrated.wMagRelayNumber = (ushort)(0);

                // 2023.03.14 : MAG RELAY IMAGE SHIFT XY CORRECT 적용
                if (m_pClientPublicData != null)
                {
                    // MAG RELAY IMAGE SHIFT XY CORRECT 사용여부 확인
                    if (m_pClientPublicData.nOptionData[IDX_CLIENT_OPTION_INT_MAG_IMAGE_SHIFT_USED] == 1)
                    {
                        // MAG RELAY IMAGE SHIFT XY CORRECT 사용
                        m_MagRelayImageShiftXValue = m_pClientPublicData.nOptionData[IDX_CLIENT_OPTION_INT_MAG_IMAGE_SHIFT_X20];
                        m_MagRelayImageShiftYValue = m_pClientPublicData.nOptionData[IDX_CLIENT_OPTION_INT_MAG_IMAGE_SHIFT_Y20];
                    }
                }
            }
            
            // 2019.11.12
            if (m_pIntegrated.wMagRelayNumber != m_pIntegrated.wMagRelayNumberOld)
            {
                //
                // Mag Relay (배율 릴레이) 동작
                //
                // 릴레이 동작
                m_pIntegrated.wRelayState &= wMask;
                m_pIntegrated.wRelayState |= m_MagRelay[m_pIntegrated.wMagRelayNumber];
                AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "ZM2,", m_pIntegrated.wRelayState); // Sbr Data 전송

                m_pIntegrated.wMagRelayNumberOld = (ushort)(m_pIntegrated.wMagRelayNumber);
            }

            // 2023.03.14 : MAG RELAY IMAGE SHIFT XY CORRECT 적용
            if (m_pClientPublicData != null)
            {
                // MAG RELAY IMAGE SHIFT XY CORRECT 사용여부 확인
                if (m_pClientPublicData.nOptionData[IDX_CLIENT_OPTION_INT_MAG_IMAGE_SHIFT_USED] == 1)
                {
                    // MAG RELAY IMAGE SHIFT XY CORRECT 사용 --> Image Shift X/Y 재실행
                    AfxIntegratedSetImageShiftX();
                    AfxIntegratedSetImageShiftY();
                }
            }

            // 배율에 따른비율값 리턴
            return dRatio;
        }

        // ---- USB_FT232H_Device.cpp:15565 ----
        //
        // Integrated Control : CL1 값을 설정
        //
        public int AfxIntegratedSetCL1()
        {
            if (m_pIntegrated == null)
                return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // CL1 값
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_CL1 + (4095 - m_pIntegrated.wCL1 << 3));

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:15577 ----
        public int AfxIntegratedSetCL1(int nCL1)
        {
            if (m_pIntegrated == null)
                return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // CL1 값
            m_pIntegrated.wCL1 = (ushort)( (ushort)nCL1);

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:15589 ----
        public int AfxIntegratedSetCL1(int nSpotSize, int nCL1)
        {
            if (m_pIntegrated == null)
                return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // CL1 값
            m_pIntegrated.wCL1 = (ushort)( (ushort)nCL1);
            m_pIntegrated.wCL1List[nSpotSize-1] = m_pIntegrated.wCL1;
            m_nDA_CL1[nSpotSize-1] = m_pIntegrated.wCL1List[nSpotSize-1];

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:15603 ----
        public int AfxIntegratedGetCL1()
        {
            if (m_pIntegrated == null)
                return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // CL1 값
            return m_pIntegrated.wCL1;
        }

        // ---- USB_FT232H_Device.cpp:15613 ----
        public int AfxIntegratedGetCL1(int nSpotSize)
        {
            if (m_pIntegrated == null)
                return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // CL1 값
            return m_pIntegrated.wCL1List[nSpotSize-1];
        }

        // ---- USB_FT232H_Device.cpp:15626 ----
        //
        // Integrated Control : CL2 값을 설정
        //
        public int AfxIntegratedSetCL2()
        {
            if (m_pIntegrated == null)
                return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // CL2 값
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_CL2 + (4095 - m_pIntegrated.wCL2 << 3));

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:15638 ----
        public int AfxIntegratedSetCL2(int nCL2)
        {
            if (m_pIntegrated == null)
                return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // CL2 값
            m_pIntegrated.wCL2 = (ushort)( (ushort)nCL2);

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:15650 ----
        public int AfxIntegratedSetCL2(int nSpotSize, int nCL2)
        {
            if (m_pIntegrated == null)
                return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // CL2 값
            m_pIntegrated.wCL2 = (ushort)( (ushort)nCL2);
            m_pIntegrated.wCL2List[nSpotSize-1] = m_pIntegrated.wCL2;
            m_nDA_CL2[nSpotSize-1] = m_pIntegrated.wCL2List[nSpotSize-1];

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:15664 ----
        public int AfxIntegratedGetCL2()
        {
            if (m_pIntegrated == null)
                return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // CL2 값
            return m_pIntegrated.wCL2;
        }

        // ---- USB_FT232H_Device.cpp:15674 ----
        public int AfxIntegratedGetCL2(int nSpotSize)
        {
            if (m_pIntegrated == null)
                return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // CL2 값
            return m_pIntegrated.wCL2List[nSpotSize-1];
        }

        // ---- USB_FT232H_Device.cpp:15695 ----
        //
        //   OL 전압이 변화될 때 교정해야할 사항
        //
        // 1) 최소 배율값 변경
        // 2) Image 축소되는 현상 --> 배율확대
        // 3) Scan Rotation 보정  --> 직교도 보정
        // 4) 변화된 WD 값 표시
        //
        //
        // Integrated Control : OL 값을 설정
        //
        public int AfxIntegratedSetOL(int nCoarse, int nFine)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Coarse 값 설정
            // FOCUS FINE 값이 변경 되었는지 확인한다.
            if (m_pIntegrated.nFocusOldCoarse != nCoarse)
            {
                m_pIntegrated.nFocusCoarse = nCoarse;
                if (m_pIntegrated.nFocusCoarse < SEM_FOCUS_COARSE_MIN) m_pIntegrated.nFocusCoarse = SEM_FOCUS_COARSE_MIN;
                if (m_pIntegrated.nFocusCoarse > SEM_FOCUS_COARSE_MAX) m_pIntegrated.nFocusCoarse = SEM_FOCUS_COARSE_MAX;

                // Smg Command 전송
                AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_OLC + (m_pIntegrated.nFocusCoarse + 4096 << 3));
                m_pIntegrated.nFocusOldCoarse = m_pIntegrated.nFocusCoarse;
            }

            //
            //  FocusFine 값의 변화 범위가 0 - 128 이므로, 최하의
            //  0 값 근처에서, OP-Amp의 오프셋 때문에 변화가 없을 수 있으므로
            //  중간값에서 변화되도록 변화시킴
            //
            // Fine 값은 2048 +/- 128 이다
            //
            // Fine 값 설정
            if (nFine > 0)
            {
                // FOCUS FINE 값이 변경 되었는지 확인한다.
                if (m_pIntegrated.nFocusOldFine != nFine)
                {
                    m_pIntegrated.nFocusFine = nFine;
                    if (m_pIntegrated.nFocusFine < SEM_FOCUS_FINE_MIN) m_pIntegrated.nFocusFine = SEM_FOCUS_FINE_MIN;
                    if (m_pIntegrated.nFocusFine > SEM_FOCUS_FINE_MAX) m_pIntegrated.nFocusFine = SEM_FOCUS_FINE_MAX;
                    // Smg Command 전송
                    AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_OLF + (2048 - 128 + m_pIntegrated.nFocusFine << 4));
                    m_pIntegrated.nFocusOldFine = m_pIntegrated.nFocusFine;
                }
            }

            //
            // FOCUS COARSE(OL)에 따른 WD값 얻기 (0.1mm단위)
            //
            m_pIntegrated.nOLWD = AfxIntegratedFindWDValue(m_pIntegrated.nFocusCoarse);
            // WD 배율보정 작업중이면
            if (m_pIntegrated.bWDSetupFlag == TRUE)
            {
                // WD SETUP 에 선택된 Z축 위치값 x 10
                m_pIntegrated.nOLWD = m_pIntegrated.nWDStepNumber * 10;
                m_pIntegrated.nOLWDOld = 30; // 3.0[mm]
            }
            // MAX WD-OL 위치값 확인 (MAX : 50.0mm, 500), 버퍼는 최대 60.0mm, 600개 까지 준비됨
            if (m_pIntegrated.nOLWD > 500)
                m_pIntegrated.nOLWD = 500;

            //
            // WD에 따른 보정은 필요성 여부를 확인 한 후, 실행한다
            // OL에 따른 WD 위치값이 변화되었으면 WD에 따른 OL, ZOOM, S-ROT를 적용한다
            //
            // 0.1mm 이상 OL 값이 변화되었을 경우 ( OL값에 따른 WD값 )
            //
            if (m_pIntegrated.nOLWD != m_pIntegrated.nOLWDOld)
            {
                // WD값 보관
                m_pIntegrated.nOLWDOld = m_pIntegrated.nOLWD;

                //
                // OPTION : WD 값 변경에 따른 Scan-Rotation 적용
                //
                if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_WD_SCAN_ROTATION] == 1)
                {
                    // WD에 따른 SCAN ROTATION 보정값 : -3600 ~ 0 ~ +3600
                    m_pIntegrated.nWDSRotationValue = m_pIntegrated.nWDSRotationLinear[m_pIntegrated.nOLWD];
                    // 현재 선택된 SCAN ROTATION 각도 재실행
                    AfxScanBridgeSetScanRotation(m_pIntegrated.nSRotationValue);
                }

                //
                // OPTION : WD 값 변경에 따른 Zoom Scale 적용
                //
                if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_WD_SCALE_ZOOM] == 1)
                {
                    AfxIntegratedSetMagnification(m_nMagStepValue[m_pIntegrated.nMagStepIndex], m_pIntegrated.nOLWD);
                }
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:15787 ----
        //
        // Integrated Control : Focus Coarse 설정 및 실행
        //
        public int AfxIntegratedSetFocusCoarse(int nCoarse)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Coarse 값 설정
            // Smg Command 전송
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_OLC + (nCoarse + 4096 << 3)); // 2020.02.18 계산식 오류 발견 --> ((m_pIntegrated->nFocusCoarse + 4096) << 3)

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:15800 ----
        //
        // Integrated Control : WD 변화에 따른 Focus Coarse 자동설정
        //
        public int AfxIntegratedSetWDAutoAdjustFocusCoarse(int nWDPos)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 2020.05.11
            // WD 값에 따른 FOCUS 재설정
            m_pIntegrated.nOLWD = nWDPos;
            if (m_pIntegrated.nOLWD < 0)   m_pIntegrated.nOLWD = 0;   // 0mm
            if (m_pIntegrated.nOLWD > 500) m_pIntegrated.nOLWD = 500; // 50mm

            // FOCUS COARSE 설정
            int nFocusCoarse = m_pIntegrated.nWDOLCoarseLinear[m_pIntegrated.nOLWD];
            AfxIntegratedSetFocusCoarse(nFocusCoarse);

            // 2021.08.03 : 아래의 값 nWDPosition의 범위는 0~50 이므로 nOLWD의 범위 0~500와는 호환되지 않는다
            // WD POSITION 재설정
            //m_pIntegrated->nWDPosition  = m_pIntegrated->nOLWD;

            // FOCUS COARSE 재설정
            m_pIntegrated.nFocusCoarse = nFocusCoarse;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:15828 ----
        //
        // Integrated Control : WOBBLER 를 위한 OLC 값을 설정
        //
        public int AfxIntegratedSetWobblerOLC(int nWobbblerCoarse)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Smg Command 전송
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_OLC + nWobbblerCoarse);// ((nWobbblerCoarse + 4096) << 3));

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:15842 ----
        //
        // Integrated Control : WOBBLER 를 위한 STIG-X 값을 설정
        //
        public int AfxIntegratedSetWobblerSTX(int nWobbblerStigX)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Smg Command 전송
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_STX + nWobbblerStigX);// ((nWobbblerStigX) << 4));

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:15856 ----
        //
        // Integrated Control : WOBBLER 를 위한 STIG-Y 값을 설정
        //
        public int AfxIntegratedSetWobblerSTY(int nWobbblerStigY)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Smg Command 전송
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_STY + nWobbblerStigY);// ((nWobbblerStigY) << 4));

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:15870 ----
        //
        // Integrated Control : Spot Size Setting
        //
        public int AfxIntegratedSetSpotSize()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Smg Command 전송
            //-------------------------
            // Spot Size ( CL1, CL2 ) Setting
            //-------------------------
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_CL1 + (4095 - m_nDA_CL1[m_pIntegrated.nSpotSize - 1] << 3));
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_CL2 + (4095 - m_nDA_CL2[m_pIntegrated.nSpotSize - 1] << 3));

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:15885 ----
        public int AfxIntegratedSetSpotSize(int nSpotSize)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            m_pIntegrated.nSpotSize = nSpotSize;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:15895 ----
        public int AfxIntegratedGetSpotSize()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            return m_pIntegrated.nSpotSize;
        }

        // ---- USB_FT232H_Device.cpp:15903 ----
        public int AfxIntegratedGetCLSpotSize()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // CL 교정용 SPOT-SIZE
            return m_pIntegrated.nCLSpotSize;
        }

        // ---- USB_FT232H_Device.cpp:15912 ----
        public int AfxIntegratedSetCLSpotSize(int nSpotSize)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // CL 교정용 SPOT-SIZE
            m_pIntegrated.nCLSpotSize = nSpotSize;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:15926 ----
        //
        // Integrated Control : Wobble Setting
        //
        public int AfxIntegratedSetWobble()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Smg Command 전송
            //-------------------------
            // Wobble Setting
            //-------------------------
            int bLoop = TRUE;
            {
                AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_OLC + (m_nWobblerTable[m_pIntegrated.nWobbleCount] + m_pIntegrated.nFocusCoarse + 4096 << 3));
                m_pIntegrated.nWobbleCount++;
                if (m_pIntegrated.nWobbleCount > 7) m_pIntegrated.nWobbleCount = 0;

                // Run Check ?
                bLoop = m_pIntegrated.bWobbleRunFlag;
                if (bLoop == TRUE)
                {
                    //AfxSystemWaitTime(130); // wait 130[ms]
                    Sleep(130);
                }
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:15953 ----
        public int AfxIntegratedSetWobble(int nWob)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            m_pIntegrated.nWobble = nWob;

            // WOBBLE TABLE 설정
            m_pIntegrated.nWobbleCount = 0;
            double dWobbler = (double)m_pIntegrated.nWobble / 10.0;
            int nMagIndex = m_pIntegrated.nMagStepIndex;
            if (nMagIndex < 10) nMagIndex = 10;
            if (nMagIndex > 63) nMagIndex = 63;
            nMagIndex = 64 - nMagIndex;
            m_nWobblerTable[0] = (int)((m_dSinTable[0] * dWobbler) * (double)nMagIndex);
            m_nWobblerTable[1] = (int)((m_dSinTable[1] * dWobbler) * (double)nMagIndex);
            m_nWobblerTable[2] = (int)((m_dSinTable[2] * dWobbler) * (double)nMagIndex);
            m_nWobblerTable[3] = (int)((m_dSinTable[3] * dWobbler) * (double)nMagIndex);
            m_nWobblerTable[4] = (int)((m_dSinTable[4] * dWobbler) * (double)nMagIndex);
            m_nWobblerTable[5] = (int)((m_dSinTable[5] * dWobbler) * (double)nMagIndex);
            m_nWobblerTable[6] = (int)((m_dSinTable[6] * dWobbler) * (double)nMagIndex);
            m_nWobblerTable[7] = (int)((m_dSinTable[7] * dWobbler) * (double)nMagIndex);


            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:15980 ----
        public void AfxIntegratedRunWobble(int bRun = FALSE)
        {
            if (bRun == TRUE)
            {
                m_pIntegrated.nWobbleCount = 0;
                m_pIntegrated.bWobbleRunFlag = bRun;
                AfxIntegratedSetWobble();
            }
            else
            {
                m_pIntegrated.bWobbleRunFlag = bRun;
            }
        }

        // ---- USB_FT232H_Device.cpp:15995 ----
        public int AfxIntegratedGetWobble()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // WOBBLE 값
            return m_pIntegrated.nWobble;
        }

        // ---- USB_FT232H_Device.cpp:16007 ----
        //
        // Integrated Control : Image Shift XY Initilize Setting
        //
        public int AfxIntegratedSetImageShiftInitilize()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Smg Command 전송
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_EC + 0xC0000);

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:16021 ----
        //
        // Integrated Control : Image Shift X Setting
        //
        public int AfxIntegratedSetImageShiftX()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Smg Command 전송
            //AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_ISX + (m_pIntegrated->nISXValue << 4));

            // 2023.03.14 : MAG RELAY IMAGE SHIFT X CORRECT 값 적용
            int nISXValue = m_pIntegrated.nISXValue + m_MagRelayImageShiftXValue;
            // Smg Command 전송
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_ISX + (nISXValue << 4));

            // DEVICE SERVER 윈도우
            if (m_hOwnerWnd != NULL)
            {
                // Image Shift Data 메시지 보내기
                // 2020.05.04
                //SendMessage(m_hOwnerWnd, UM_USB_SBR_IMAGESHIFT_DATA, NULL, NULL);
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:16045 ----
        public int AfxIntegratedSetImageShiftX(int nISX)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Image Shift X
            m_pIntegrated.nISXValue = nISX;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:16056 ----
        public int AfxIntegratedGetImageShiftX()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Image Shift X
            return m_pIntegrated.nISXValue;
        }

        // ---- USB_FT232H_Device.cpp:16068 ----
        //
        // Integrated Control : Image Shift Y Setting
        //
        public int AfxIntegratedSetImageShiftY()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Smg Command 전송
            //AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_ISY + (m_pIntegrated->nISYValue << 4));

            // 2023.03.14 : MAG RELAY IMAGE SHIFT Y CORRECT 값 적용
            int nISYValue = m_pIntegrated.nISYValue + m_MagRelayImageShiftYValue;
            // Smg Command 전송
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_ISY + (nISYValue << 4));

            // DEVICE SERVER 윈도우
            if (m_hOwnerWnd != NULL)
            {
                // Image Shift Data 메시지 보내기
                // 2020.05.04
                //SendMessage(m_hOwnerWnd, UM_USB_SBR_IMAGESHIFT_DATA, NULL, NULL);
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:16092 ----
        public int AfxIntegratedSetImageShiftY(int nISY)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Image Shift Y
            m_pIntegrated.nISYValue = nISY;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:16103 ----
        public int AfxIntegratedGetImageShiftY()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Image Shift Y
            return m_pIntegrated.nISYValue;
        }

        // ---- USB_FT232H_Device.cpp:16115 ----
        //
        // Integrated Control : Gun Align X 값을 설정
        //
        public int AfxIntegratedSetGunAlignX()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Smg Command 전송
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_GAX + (m_pIntegrated.nGAXValue << 4));

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:16126 ----
        public int AfxIntegratedSetGunAlignX(int nGAX)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            m_pIntegrated.nGAXValue = nGAX;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:16136 ----
        public int AfxIntegratedGetGunAlignX()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            return m_pIntegrated.nGAXValue;
        }

        // ---- USB_FT232H_Device.cpp:16147 ----
        //
        // Integrated Control : Gun Align Y 값을 설정
        //
        public int AfxIntegratedSetGunAlignY()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Smg Command 전송
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_GAY + (m_pIntegrated.nGAYValue << 4));

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:16158 ----
        public int AfxIntegratedSetGunAlignY(int nGAY)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            m_pIntegrated.nGAYValue = nGAY;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:16168 ----
        public int AfxIntegratedGetGunAlignY()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            return m_pIntegrated.nGAYValue;
        }

        // ---- USB_FT232H_Device.cpp:16179 ----
        //
        // Integrated Control : Beam Align X 값을 설정
        //
        public int AfxIntegratedSetBeamAlignX()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Smg Command 전송
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_BAX + (m_pIntegrated.nBAXValue << 4));

            // DEVICE SERVER 윈도우
            if (m_hOwnerWnd != NULL)
            {
                // Beam Align Data 메시지 보내기
                // 2020.05.04
                //SendMessage(m_hOwnerWnd, UM_USB_SBR_BEAMALIGN_DATA, NULL, NULL);
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:16198 ----
        public int AfxIntegratedSetBeamAlignX(int nGAX)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            m_pIntegrated.nBAXValue = nGAX;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:16208 ----
        public int AfxIntegratedGetBeamAlignX()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            return m_pIntegrated.nBAXValue;
        }

        // ---- USB_FT232H_Device.cpp:16219 ----
        //
        // Integrated Control : Beam Align Y 값을 설정
        //
        public int AfxIntegratedSetBeamAlignY()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Smg Command 전송
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_BAY + (m_pIntegrated.nBAYValue << 4));

            // DEVICE SERVER 윈도우
            if (m_hOwnerWnd != NULL)
            {
                // Beam Align Data 메시지 보내기
                // 2020.05.04
                //SendMessage(m_hOwnerWnd, UM_USB_SBR_BEAMALIGN_DATA, NULL, NULL);
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:16238 ----
        public int AfxIntegratedSetBeamAlignY(int nGAY)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            m_pIntegrated.nBAYValue = nGAY;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:16248 ----
        public int AfxIntegratedGetBeamAlignY()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            return m_pIntegrated.nBAYValue;
        }

        // ---- USB_FT232H_Device.cpp:16259 ----
        //
        // Integrated Control : Stig X Setting
        //
        public int AfxIntegratedSetStigX()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Smg Command 전송
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_STX + (m_pIntegrated.nStigXValue << 4));

            // DEVICE SERVER 윈도우
            if (m_hOwnerWnd != NULL)
            {
                // Stig Data 메시지 보내기
                // 2020.05.04
                //SendMessage(m_hOwnerWnd, UM_USB_SBR_STIG_DATA, NULL, NULL);
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:16278 ----
        public int AfxIntegratedSetStigX(int nStigX)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Stig X
            m_pIntegrated.nStigXValue = nStigX;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:16289 ----
        public int AfxIntegratedGetStigX()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Stig X
            return m_pIntegrated.nStigXValue;
        }

        // ---- USB_FT232H_Device.cpp:16301 ----
        //
        // Integrated Control : Stig Y Setting
        //
        public int AfxIntegratedSetStigY()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Smg Command 전송
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_STY + (m_pIntegrated.nStigYValue << 4));

            // DEVICE SERVER 윈도우
            if (m_hOwnerWnd != NULL)
            {
                // Stig Data 메시지 보내기
                // 2020.05.04
                //SendMessage(m_hOwnerWnd, UM_USB_SBR_STIG_DATA, NULL, NULL);
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:16320 ----
        public int AfxIntegratedSetStigY(int nStigY)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Stig Y
            m_pIntegrated.nStigYValue = nStigY;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:16331 ----
        public int AfxIntegratedGetStigY()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // Stig Y
            return m_pIntegrated.nStigYValue;
        }

        // ---- USB_FT232H_Device.cpp:16358 ----
        //
        // WD에 따른 WD_OL_Coarse, WD_SRotation, WD_Zooming 값을 설정하는 방법
        //
        // 1) 처음에는  m_pIntegrated->nWDOLCoarse[] 값을 Board에서 측정한 전압값을 발생시키는
        //    Cal_WD_OL_Value_From_SM300_OL_Voltage() 함수에서 만들려고 하였으나.
        //
        // 2) WD=5mm (IniPar.WD_TopLimit)에서 AutoFocus를 수행하고 얻은 OL값을 m_pIntegrated->nWDOLCoarse[]에
        //    저장하고, 1mm씩 이동한 후 반복 시행을 WD=40mm(IniPar.WD_BottomLimit)까지 진행한다.
        //
        // 3) 이 때 얻은 m_pIntegrated->nWDOLCoarse[] 값이 정확한지 WD 위치에서 확인하면서,
        //    m_pIntegrated->nWDSRotation[], m_pIntegrated->nWDZooming[] 값을 저장한다.
        //
        // 4) 1mm 간격으로 측정값을 얻으므로, 분해능을 0.1mm 까지 얻기위해
        //    1mm 간격의 Data를 10등분으로 선형화 작업을 프로그램 시작시 진행한다.
        //
        //
        // Integrated Control : WD에 따른 OL_Coarse, ScanRotation, Zooming, MinMag 보정값 설정
        //
        public int AfxIntegratedSetWDCompensation()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // WD 최대 항목 갯수 : 50 Item (최대 60개)
            m_pIntegrated.nWDMaxValue = 50;

            //-----
            // WD = 5mm                                                                                        // 길이변화의 정확도를  올리기 위해 1000 배율로
            // 100을 기준으로하였을 때 축소되는 배율
            //-----
            m_pIntegrated.nWDOLCoarse[0] = 3800;    m_pIntegrated.nWDSRotation[0] = -36;      m_pIntegrated.nWDZooming[0] = 1000;     m_pIntegrated.nWDMinMag[0] = 70;
            m_pIntegrated.nWDOLCoarse[1] = 3800;    m_pIntegrated.nWDSRotation[1] = -20;      m_pIntegrated.nWDZooming[1] = 1000;     m_pIntegrated.nWDMinMag[1] = 70;
            m_pIntegrated.nWDOLCoarse[2] = 3800;    m_pIntegrated.nWDSRotation[2] = -14;      m_pIntegrated.nWDZooming[2] = 1000;     m_pIntegrated.nWDMinMag[2] = 70;
            m_pIntegrated.nWDOLCoarse[3] = 3800;    m_pIntegrated.nWDSRotation[3] = -10;      m_pIntegrated.nWDZooming[3] = 1000;     m_pIntegrated.nWDMinMag[3] = 70;
            m_pIntegrated.nWDOLCoarse[4] = 3800;    m_pIntegrated.nWDSRotation[4] = -5;       m_pIntegrated.nWDZooming[4] = 1000;     m_pIntegrated.nWDMinMag[4] = 70;
            //-----
            // WD = 5mm    TP14    -7.047V
            m_pIntegrated.nWDOLCoarse[5] = 3505;    m_pIntegrated.nWDSRotation[5] = 0;        m_pIntegrated.nWDZooming[5] = 1000;     m_pIntegrated.nWDMinMag[5] = 70;
            // WD = 6mm            -6.722V
            m_pIntegrated.nWDOLCoarse[6] = 3341;    m_pIntegrated.nWDSRotation[6] = 32;       m_pIntegrated.nWDZooming[6] = 940;      m_pIntegrated.nWDMinMag[6] = 70;
            // WD = 7mm            -6.438V
            m_pIntegrated.nWDOLCoarse[7] = 3200;    m_pIntegrated.nWDSRotation[7] = 57;       m_pIntegrated.nWDZooming[7] = 860;      m_pIntegrated.nWDMinMag[7] = 50;
            // WD = 8mm            -6.183V
            m_pIntegrated.nWDOLCoarse[8] = 3072;    m_pIntegrated.nWDSRotation[8] = 83;       m_pIntegrated.nWDZooming[8] = 810;      m_pIntegrated.nWDMinMag[8] = 50;
            // WD = 9mm            -5.953V
            m_pIntegrated.nWDOLCoarse[9] = 2958;    m_pIntegrated.nWDSRotation[9] = 104;      m_pIntegrated.nWDZooming[9] = 760;      m_pIntegrated.nWDMinMag[9] = 50;
            // WD = 10mm           -5.787V
            m_pIntegrated.nWDOLCoarse[10] = 2874;   m_pIntegrated.nWDSRotation[10] = 124;     m_pIntegrated.nWDZooming[10] = 720;     m_pIntegrated.nWDMinMag[10] = 50;
            // WD = 11mm               67
            m_pIntegrated.nWDOLCoarse[11] = 2886;   m_pIntegrated.nWDSRotation[11] = 143;     m_pIntegrated.nWDZooming[11] = 680;     m_pIntegrated.nWDMinMag[11] = 50;
            // WD = 12mm               80
            m_pIntegrated.nWDOLCoarse[12] = 2806;   m_pIntegrated.nWDSRotation[12] = 162;     m_pIntegrated.nWDZooming[12] = 650;     m_pIntegrated.nWDMinMag[12] = 50;
            // WD = 13mm               85
            m_pIntegrated.nWDOLCoarse[13] = 2721;   m_pIntegrated.nWDSRotation[13] = 176;     m_pIntegrated.nWDZooming[13] = 610;     m_pIntegrated.nWDMinMag[13] = 50;
            // WD = 14mm               68
            m_pIntegrated.nWDOLCoarse[14] = 2658;   m_pIntegrated.nWDSRotation[14] = 187;     m_pIntegrated.nWDZooming[14] = 580;     m_pIntegrated.nWDMinMag[14] = 50;
            // WD = 15mm               48
            m_pIntegrated.nWDOLCoarse[15] = 2610;   m_pIntegrated.nWDSRotation[15] = 200;     m_pIntegrated.nWDZooming[15] = 560;     m_pIntegrated.nWDMinMag[15] = 50;
            // WD = 16mm               79
            m_pIntegrated.nWDOLCoarse[16] = 2531;   m_pIntegrated.nWDSRotation[16] = 214;     m_pIntegrated.nWDZooming[16] = 540;     m_pIntegrated.nWDMinMag[16] = 50;
            // WD = 17mm               47
            m_pIntegrated.nWDOLCoarse[17] = 2484;   m_pIntegrated.nWDSRotation[17] = 225;     m_pIntegrated.nWDZooming[17] = 520;     m_pIntegrated.nWDMinMag[17] = 30;
            // WD = 18mm               36
            m_pIntegrated.nWDOLCoarse[18] = 2448;   m_pIntegrated.nWDSRotation[18] = 235;     m_pIntegrated.nWDZooming[18] = 490;     m_pIntegrated.nWDMinMag[18] = 30;
            // WD = 19mm               56
            m_pIntegrated.nWDOLCoarse[19] = 2392;   m_pIntegrated.nWDSRotation[19] = 248;     m_pIntegrated.nWDZooming[19] = 480;     m_pIntegrated.nWDMinMag[19] = 30;
            // WD = 20mm               45
            m_pIntegrated.nWDOLCoarse[20] = 2347;   m_pIntegrated.nWDSRotation[20] = 255;     m_pIntegrated.nWDZooming[20] = 460;     m_pIntegrated.nWDMinMag[20] = 30;
            // WD = 21mm               40
            m_pIntegrated.nWDOLCoarse[21] = 2307;   m_pIntegrated.nWDSRotation[21] = 265;     m_pIntegrated.nWDZooming[21] = 440;     m_pIntegrated.nWDMinMag[21] = 30;
            // WD = 22mm               41
            m_pIntegrated.nWDOLCoarse[22] = 2266;   m_pIntegrated.nWDSRotation[22] = 269;     m_pIntegrated.nWDZooming[22] = 430;     m_pIntegrated.nWDMinMag[22] = 30;
            // WD = 23mm               35
            m_pIntegrated.nWDOLCoarse[23] = 2231;   m_pIntegrated.nWDSRotation[23] = 281;     m_pIntegrated.nWDZooming[23] = 420;     m_pIntegrated.nWDMinMag[23] = 30;
            // WD = 24mm               45
            m_pIntegrated.nWDOLCoarse[24] = 2186;   m_pIntegrated.nWDSRotation[24] = 288;     m_pIntegrated.nWDZooming[24] = 400;     m_pIntegrated.nWDMinMag[24] = 30;
            // WD = 25mm               25
            m_pIntegrated.nWDOLCoarse[25] = 2161;   m_pIntegrated.nWDSRotation[25] = 294;     m_pIntegrated.nWDZooming[25] = 390;     m_pIntegrated.nWDMinMag[25] = 30;
            // WD = 26mm               33
            m_pIntegrated.nWDOLCoarse[26] = 2128;   m_pIntegrated.nWDSRotation[26] = 300;     m_pIntegrated.nWDZooming[26] = 380;     m_pIntegrated.nWDMinMag[26] = 30;
            // WD = 27mm               21
            m_pIntegrated.nWDOLCoarse[27] = 2107;   m_pIntegrated.nWDSRotation[27] = 308;     m_pIntegrated.nWDZooming[27] = 370;     m_pIntegrated.nWDMinMag[27] = 30;
            // WD = 28mm               31
            m_pIntegrated.nWDOLCoarse[28] = 2076;   m_pIntegrated.nWDSRotation[28] = 313;     m_pIntegrated.nWDZooming[28] = 360;     m_pIntegrated.nWDMinMag[28] = 30;
            // WD = 29mm               20
            m_pIntegrated.nWDOLCoarse[29] = 2056;   m_pIntegrated.nWDSRotation[29] = 318;     m_pIntegrated.nWDZooming[29] = 350;     m_pIntegrated.nWDMinMag[29] = 30;
            // WD = 30mm               36
            m_pIntegrated.nWDOLCoarse[30] = 2020;   m_pIntegrated.nWDSRotation[30] = 323;     m_pIntegrated.nWDZooming[30] = 340;     m_pIntegrated.nWDMinMag[30] = 30;
            // WD = 31mm               39
            m_pIntegrated.nWDOLCoarse[31] = 1981;   m_pIntegrated.nWDSRotation[31] = 327;     m_pIntegrated.nWDZooming[31] = 330;     m_pIntegrated.nWDMinMag[31] = 20;
            // WD = 32mm               16
            m_pIntegrated.nWDOLCoarse[32] = 1965;   m_pIntegrated.nWDSRotation[32] = 332;     m_pIntegrated.nWDZooming[32] = 320;     m_pIntegrated.nWDMinMag[32] = 20;
            // WD = 33mm               20
            m_pIntegrated.nWDOLCoarse[33] = 1945;   m_pIntegrated.nWDSRotation[33] = 337;     m_pIntegrated.nWDZooming[33] = 310;     m_pIntegrated.nWDMinMag[33] = 20;
            // WD = 34mm               29
            m_pIntegrated.nWDOLCoarse[34] = 1916;   m_pIntegrated.nWDSRotation[34] = 345;     m_pIntegrated.nWDZooming[34] = 310;     m_pIntegrated.nWDMinMag[34] = 20;
            // WD = 35mm               17
            m_pIntegrated.nWDOLCoarse[35] = 1899;   m_pIntegrated.nWDSRotation[35] = 346;     m_pIntegrated.nWDZooming[35] = 300;     m_pIntegrated.nWDMinMag[35] = 20;
            // WD = 36mm               16
            m_pIntegrated.nWDOLCoarse[36] = 1883;   m_pIntegrated.nWDSRotation[36] = 351;     m_pIntegrated.nWDZooming[36] = 290;     m_pIntegrated.nWDMinMag[36] = 20;
            // WD = 37mm               23
            m_pIntegrated.nWDOLCoarse[37] = 1860;   m_pIntegrated.nWDSRotation[37] = 353;     m_pIntegrated.nWDZooming[37] = 290;     m_pIntegrated.nWDMinMag[37] = 20;
            // WD = 38mm               20
            m_pIntegrated.nWDOLCoarse[38] = 1840;   m_pIntegrated.nWDSRotation[38] = 356;     m_pIntegrated.nWDZooming[38] = 280;     m_pIntegrated.nWDMinMag[38] = 20;
            // WD = 39mm               19
            m_pIntegrated.nWDOLCoarse[39] = 1821;   m_pIntegrated.nWDSRotation[39] = 360;     m_pIntegrated.nWDZooming[39] = 270;     m_pIntegrated.nWDMinMag[39] = 20;
            // WD = 40mm               36
            m_pIntegrated.nWDOLCoarse[40] = 1790;   m_pIntegrated.nWDSRotation[40] = 362;     m_pIntegrated.nWDZooming[40] = 269;     m_pIntegrated.nWDMinMag[40] = 30;
            // WD = 41mm               39
            m_pIntegrated.nWDOLCoarse[41] = 1780;   m_pIntegrated.nWDSRotation[41] = 363;     m_pIntegrated.nWDZooming[41] = 268;     m_pIntegrated.nWDMinMag[41] = 20;
            // WD = 42mm               16
            m_pIntegrated.nWDOLCoarse[42] = 1760;   m_pIntegrated.nWDSRotation[42] = 364;     m_pIntegrated.nWDZooming[42] = 267;     m_pIntegrated.nWDMinMag[42] = 20;
            // WD = 43mm               20
            m_pIntegrated.nWDOLCoarse[43] = 1750;   m_pIntegrated.nWDSRotation[43] = 365;     m_pIntegrated.nWDZooming[43] = 266;     m_pIntegrated.nWDMinMag[43] = 20;
            // WD = 44mm               29
            m_pIntegrated.nWDOLCoarse[44] = 1740;   m_pIntegrated.nWDSRotation[44] = 366;     m_pIntegrated.nWDZooming[44] = 265;     m_pIntegrated.nWDMinMag[44] = 20;
            // WD = 45mm               17
            m_pIntegrated.nWDOLCoarse[45] = 1730;   m_pIntegrated.nWDSRotation[45] = 367;     m_pIntegrated.nWDZooming[45] = 264;     m_pIntegrated.nWDMinMag[45] = 20;
            // WD = 46mm               16
            m_pIntegrated.nWDOLCoarse[46] = 1720;   m_pIntegrated.nWDSRotation[46] = 368;     m_pIntegrated.nWDZooming[46] = 263;     m_pIntegrated.nWDMinMag[46] = 20;
            // WD = 47mm               23
            m_pIntegrated.nWDOLCoarse[47] = 1710;   m_pIntegrated.nWDSRotation[47] = 369;     m_pIntegrated.nWDZooming[47] = 262;     m_pIntegrated.nWDMinMag[47] = 20;
            // WD = 48mm               20
            m_pIntegrated.nWDOLCoarse[48] = 1700;   m_pIntegrated.nWDSRotation[48] = 370;     m_pIntegrated.nWDZooming[48] = 261;     m_pIntegrated.nWDMinMag[48] = 20;
            // WD = 49mm               19
            m_pIntegrated.nWDOLCoarse[49] = 1690;   m_pIntegrated.nWDSRotation[49] = 371;     m_pIntegrated.nWDZooming[49] = 260;     m_pIntegrated.nWDMinMag[49] = 20;
            // WD = 49mm               19
            m_pIntegrated.nWDOLCoarse[50] = 1690;   m_pIntegrated.nWDSRotation[50] = 371;     m_pIntegrated.nWDZooming[50] = 260;     m_pIntegrated.nWDMinMag[50] = 20;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:16474 ----
        //
        // Integrated Control : WD에 따른 OL_Coarse, ScanRotation, Zooming, MinMag 보정값 10등분 선형화 작업
        //
        public int AfxIntegratedSetWDOLSrotZoomingLinearize()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // WD 최대 항목 갯수 : 50 Item (최대 60개)
            int    nWD = 0;
            int    n   = 0;
            double dOffset   = 0.0;
            double dBase_off = 0.0;

            // 2021.08.02
            int nMaxWD = 60; //m_pIntegrated->nWDMaxValue;

            //--
            // 10등분 선형화 작업 : m_pIntegrated->nWDOLCoarse[] ==> WD_OL_coarse[]
            //--
            for (nWD = 0; nWD < nMaxWD; nWD++)
            {
                // Focus Coarse 값은 점점 감소한다
                dOffset   = (m_pIntegrated.nWDOLCoarse[nWD] - m_pIntegrated.nWDOLCoarse[nWD + 1]) / 10.0;
                dBase_off = (double)m_pIntegrated.nWDOLCoarse[nWD];
                // 10 등분 선형화 작업
                for (n = 0; n<10; n++)
                {
                    // 2021.08.02 : 기본설정값 수정됨, 아래 내용은 사용하지 않음.
                    // 2021.06.03
                    // WD 4.0[mm] 미만 CHECK?
                    //if (nWD < 4)
                    //{
                    //    // WD 4.0[mm] 미만 일 경우 기본값으로 설정 한다
                    //    m_pIntegrated->nWDOLCoarseLinear[(nWD * 10) + n] = 4095;
                    //    continue;
                    //}
                    if (n == 0)
                    {
                        m_pIntegrated.nWDOLCoarseLinear[(nWD * 10) + n] = m_pIntegrated.nWDOLCoarse[nWD];
                    }
                    else
                    {
                        m_pIntegrated.nWDOLCoarseLinear[(nWD * 10) + n] = (int)dBase_off;
                    }
                    dBase_off -= dOffset;
                }
            }

            //--
            // 10등분 선형화 작업 : m_pIntegrated->nWDZooming[] ==> WD_zooming[]
            //--
            for (nWD = 0; nWD < nMaxWD; nWD++)
            {
                // Zoom 값은 점점 감소한다
                dOffset   = (m_pIntegrated.nWDZooming[nWD] - m_pIntegrated.nWDZooming[nWD + 1]) / 10.0;
                dBase_off = (double)m_pIntegrated.nWDZooming[nWD];
                // 10 등분 선형화 작업
                for (n = 0; n<10; n++)
                {
                    // 2021.08.02 : 기본설정값 수정됨, 아래 내용은 사용하지 않음.
                    // 2021.06.03
                    // WD 4.0[mm] 미만 CHECK?
                    //if (nWD < 4)
                    //{
                    //    // WD 4.0[mm] 미만 일 경우 기본값으로 설정 한다
                    //    m_pIntegrated->nWDZoomingLinear[(nWD * 10) + n] = 1000;
                    //    continue;
                    //}
                    if (n == 0)
                    {
                        m_pIntegrated.nWDZoomingLinear[(nWD * 10) + n] = m_pIntegrated.nWDZooming[nWD];
                    }
                    else
                    {
                        m_pIntegrated.nWDZoomingLinear[(nWD * 10) + n] = (int)dBase_off;
                    }
                    dBase_off -= dOffset;
                }
            }

            //--
            // 10등분 선형화 작업 : m_pIntegrated->nWDSRotation[] ==> WD_srotation[]
            //--
            for (nWD = 0; nWD < nMaxWD; nWD++)
            {
                // S-Rot 값은 점점 증가한다
                dOffset   = (m_pIntegrated.nWDSRotation[nWD + 1] - m_pIntegrated.nWDSRotation[nWD]) / 10.0;
                dBase_off = (double)m_pIntegrated.nWDSRotation[nWD];
                // 10 등분 선형화 작업
                for (n = 0; n<10; n++)
                {
                    // 2021.08.02 : 기본설정값 수정됨, 아래 내용은 사용하지 않음.
                    // 2021.06.03
                    // WD 4.0[mm] 미만 CHECK?
                    //if (nWD < 4)
                    //{
                    //    // WD 4.0[mm] 미만 일 경우 기본값으로 설정 한다
                    //    m_pIntegrated->nWDSRotationLinear[(nWD * 10) + n] = 0;
                    //    continue;
                    //}
                    if (n == 0)
                    {
                        m_pIntegrated.nWDSRotationLinear[(nWD * 10) + n] = m_pIntegrated.nWDSRotation[nWD];
                    }
                    else
                    {
                        m_pIntegrated.nWDSRotationLinear[nWD * 10 + n] = (int)dBase_off;
                    }
                    dBase_off += dOffset;
                }
            }
            //--

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:16590 ----
        //
        // Integrated Control : Focus Coarse (OL값) 에 따른 WDx10 값 찾기
        //
        public int AfxIntegratedFindWDValue(int nCoarse)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            int nWDx10 = 0;

            // Focus Coarse를 Linear 한 배열을 검색한다
            int nWDMax10 = m_pIntegrated.nWDMaxValue * 10;

            // 시작은 3.0mm 부터 검색
            for (nWDx10 = 30; nWDx10 < nWDMax10; nWDx10++)
            {
                if (m_pIntegrated.nWDOLCoarseLinear[nWDx10] <= nCoarse)  break;
            }

            return nWDx10;
        }

        // ---- USB_FT232H_Device.cpp:16611 ----
        //
        // Integrated Control : Lens Coil에 흐르는 전류 최소화
        //
        public int AfxIntegratedLensCoilCurrentMinimum()
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            //-------------------------------------
            // Lens Coil에 흐르는 전류 최소화
            //-------------------------------------
            // GAX, GAY
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_GAX + (2048 << 4));
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_GAY + (2048 << 4));
            // BAX, BAY
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_BAX + (2048 << 4));
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_BAY + (2048 << 4));
            // CL1, CL2
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_CL1 + (4095 << 3));
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_CL2 + (4095 << 3));
            // OL
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_OLC + (4095 << 3));
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_OLF + (2048 << 4));
            // StigX, StigY
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_STX + (2048 << 4));
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_STY + (2048 << 4));
            // ImgShiftX, ImgShiftY
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_ISX + (2048 << 4));
            AfxFT232HPortSendCommand(IDX_FT232H_SEM_IMAGE_DEVICE, cWR_AD5361, AD5361_ISY + (2048 << 4));

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:16677 ----
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
        // SEM SE Image Data Device Command Control...
        //
        //
        //

        //
        // SE Image Data : SEM SE Image Data 획득 쓰레드 생성
        //
        public int AfxCreateSEImageDataThread()
        {
            // SE Image Data 쓰레드 사용 중이면...
            if (m_pSEImageDataThread != null) return RET_SUCCESS_OK;
            // SE Image Data 쓰레드 파라메터 중복 확인
            if (m_pSEImageDataThreadParams != null)
            {
                // Scan Bridge 쓰레드에서 사용한 파라메터 삭제

                m_pSEImageDataThreadParams = null;
            }

            // SE Image Scan 쓰레드 강제종료 : 비활성
            m_pIntegrated.bScanThreadStop = FALSE;

            // Scan Bridge 쓰레드에서 사용할 파라메터를 생성한다
            m_pSEImageDataThreadParams = new SEIMAGEDATATHREADPARAMS();
            if (m_pSEImageDataThreadParams == null) return RET_ERROR_SEM_THREAD_PARAMS;
            m_pSEImageDataThreadParams = new SEIMAGEDATATHREADPARAMS();
            // Scan Bridge 쓰레드에서 사용할 파라메터 기본값 설정
            m_pSEImageDataThreadParams.bThreadDone  = FALSE;          // 쓰레드 종료 설정
            m_pSEImageDataThreadParams.nFT232HIndex = IDX_FT232H_SEM_IMAGE_DEVICE; // SE Image Scan 장치 인덱스
            m_pSEImageDataThreadParams.hFT232Handle = IntPtr.Zero;           // SE Image Scan 장치 핸들
            m_pSEImageDataThreadParams.hEventHandle = null;           // SE Image Scan 이벤트 핸들
            //
            ZeroMem(m_pSEImageDataThreadParams.szFTReadBuf); // SE Image Data Read Buffer
            memset(m_pSEImageDataThreadParams.wXPixNumPerLine, 0x00, sizeof(ushort) * 1000                            ); // SE Image Data Debug 용 Line 당 Pixel Count
            memset(m_pSEImageDataThreadParams.wAvrAutoValue  , 0x00, sizeof(ushort) * 2                               ); // SE Image Data Auto Value Buffer
            m_pSEImageDataThreadParams.bFirstButtonPressed = m_pIntegrated.bFirstButtonPressed;                       // SE Image Data Frame Start Flag
            m_pSEImageDataThreadParams.bScanDataClear      = FALSE;   // SE Image Data All Clear Flag
            m_pSEImageDataThreadParams.bScanDataReceive    = FALSE;   // SE Image Data Scan & Read Flag
            m_pSEImageDataThreadParams.bFirstButtonPressed = FALSE;   // SE Image Data Frame Start Flag
            m_pSEImageDataThreadParams.bErrorFlag          = FALSE;   // SE Image Data Error Flag
            m_pSEImageDataThreadParams.pbfPtr = 0;    // SE Image Data Frame Buffer X,Y Position Pointer
            m_pSEImageDataThreadParams.pbmPtr = 0;    // SE Image Data Bitmap Buffer X,Y Position Pointer
            m_pSEImageDataThreadParams.pbfPtrB = 0;    // SE Image Data Frame Buffer X,Y Position Pointer B
            m_pSEImageDataThreadParams.pbmPtrB = 0;    // SE Image Data Bitmap Buffer X,Y Position Pointer B
            m_pSEImageDataThreadParams.nABCCount           = 0;       // SE Image Data ABC Count (Data -> Avr Bright, Contrast Count ???)
            m_pSEImageDataThreadParams.nFrameCount         = 0;       // SE Image Data Frame Count
            m_pSEImageDataThreadParams.nXDrawPosition      = 0;       // SE Image Data Bitmap X Draw Position
            m_pSEImageDataThreadParams.nXBmpOffset         = 0;       // SE Image Data Bitmap X Draw Offset Position
            m_pSEImageDataThreadParams.nYDrawPosition      = 0;       // SE Image Data Bitmap Y Draw Position
            m_pSEImageDataThreadParams.nYBmpOffset         = 0;       // SE Image Data Bitmap Y Draw Offset Position
            m_pSEImageDataThreadParams.nXDrawPositionB     = 0;       // SE Image Data Bitmap X Draw Position B
            m_pSEImageDataThreadParams.nYDrawPositionB     = 0;       // SE Image Data Bitmap Y Draw Position B
            m_pSEImageDataThreadParams.nFrameBrightLevel   = 0;       // SE Image Data Frame Brightness Level
            m_pSEImageDataThreadParams.nPixelLevelSum      = 0;       // SE Image Data Pixel Sum
            m_pSEImageDataThreadParams.nPixelCount         = 0;       // SE Image Data Pixel Count
            m_pSEImageDataThreadParams.nWAverageNum        = 0;       // SE Image Data Average Number
            m_pSEImageDataThreadParams.nWAFrameNumA        = 0;       // SE Image Data A Frame Average Count
            m_pSEImageDataThreadParams.nWAFrameNumB        = 0;       // SE Image Data B Frame Average Count
            m_pSEImageDataThreadParams.nXBmpPixel          = 0;       // SE Image Data Max X Pixel Count
            m_pSEImageDataThreadParams.nYBmpPixel          = 0;       // SE Image Data Max Y Pixel Count
            m_pSEImageDataThreadParams.nAvrContrast        = 0;       // SE Image Data Avr Contrast   
            m_pSEImageDataThreadParams.nAvrBright          = 0;       // SE Image Data Avr Brightness
            m_pSEImageDataThreadParams.nAfcAvrOL           = 0;       // SE Image Data AFC Avr OL
            m_pSEImageDataThreadParams.nDMag               = 0;       // SE Image Data Debug 용 Line 당 Pixel Count
            //

            // 핸들 설정
            IntPtr ft232hHandle = AfxFT232HGetHandle(m_pSEImageDataThreadParams.nFT232HIndex);
            if (ft232hHandle == IntPtr.Zero) return RET_ERROR_FT232H_SEM_HANDLE;
            m_pSEImageDataThreadParams.hFT232Handle = ft232hHandle;   // SE Image Data 장치 핸들

            // SE Image Data 이벤트 마스크
            uint ftStatus = NULL;
            uint     dwEventMask = NULL;

            // CreateEvent
            // CreateEvent( SECURITY_ATTRIBUTES, (대부분 NULL로 사용)
            //                bMaunalReset,        (TRUE이면 수동리셋, FALSE이면 자동리셋 이벤트 생성)
            //              bInitialState,       (TRUE이면 신호 상태, FALSE이면 비신호 상태로 시작)
            //              lpName               (이벤트를 서로 다른 프로세스에 속한 스레드가 사용할 수 있도록, 이름을 줄 수 있다.NULL을 사용하면 이름없는 이벤트 생성)
            // )

            //
            // Scan Bridge 쓰레드에서 사용할 이벤트 항목 : 읽기완료 이벤트로 설정
            //
            m_pSEImageDataThreadParams.hEventHandle = CreateEvent(NULL, FALSE, FALSE, "");              // SE Scan Image Data 쓰레드에서 사용할 이벤트 생성
            if (m_pSEImageDataThreadParams.hEventHandle == null) return RET_ERROR_SEM_EVENT_HANDLE;         // 이벤트 생성 오류 확인
            dwEventMask = FT_EVENT_RXCHAR;                                                                   // SE Image Data 장치에서 사용할 이벤트 항목 설정
            ftStatus = AfxFT232HSetEventNotification(m_pSEImageDataThreadParams.nFT232HIndex, dwEventMask, m_pSEImageDataThreadParams.hEventHandle); // SE Image Scan 쓰레드에서 사용할 이벤트 설정
            if (ftStatus != FT_OK) return RET_ERROR_SBR_EVENT_MASK;
            //

            // SE Image Data 쓰레드 생성 및 시작...
            m_pSEImageDataThread = AfxBeginThread(AfxThreadSEImageDataProcessing, m_pSEImageDataThreadParams); // , THREAD_PRIORITY_NORMAL);
            if (m_pSEImageDataThread == null) return RET_ERROR_SEM_THREAD_HANDLE;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:16773 ----
        //
        // SE Image Data : 수신 데이터 획득 처리(작업) 및 이미지 데이터 처리 쓰레드
        //
        public uint AfxThreadSEImageDataProcessing(object pParams)
        {
            SEIMAGEDATATHREADPARAMS pThreadParams = (SEIMAGEDATATHREADPARAMS)pParams;

            // 쓰레드 작업 상태 설정
            pThreadParams.bThreadDone = FALSE;

            int  bLoop = TRUE;
            uint dwRet = 0;
            int   nScanMode = 0;
            int   nReceiveCount = 0;

            // 쓰레드 종료 (강제)결정
            pThreadParams.bThreadStop = FALSE;

            //
            // SE Image Data 쓰레드 프로세싱
            //
            while (bLoop == TRUE)
            {
                ///
                // SE Image Data 장치 이벤트 발생 확인...타임아웃 1000[ms]
                ///
                dwRet = WaitForSingleObject(pThreadParams.hEventHandle, FT232H_EVENT_TIMEOUT);

                if (m_nFT232HDeviceUse < 1)
                {
                    // 쓰레드 강제 종료
                    pThreadParams.bThreadStop = TRUE;
                }
                else
                {
                    switch (dwRet)
                    {
                        case WAIT_OBJECT_0:  // 해당 이벤트 발생 : 처리작업 실행 
                        {
                            // SE Image Data 스캔 허용 Flag 설정
                            pThreadParams.bScanDataReceive = m_pIntegrated.bScanDataReceive;

                            // 2021.06.09
                            // PHOTO SCAN MODE 에 따른 이미지 업데이트 딜레이
                            // PHOTO3/4/6 스캔모드시 이미지 업데이트를 매번 할 경우, 컨트롤 및 화면 업데이트가 매우 느려짐.
                            // 위의 현상을 제거하기 위해 이벤트 발생시 이미지 업데이트를 일정 횟수 간격으로 딜레이 업데이트 한다.
                            nScanMode = m_pIntegrated.nScanMode;
                            if (nScanMode == PHOTO4_SCAN_MODE)
                            {
                                // PHOTO SCAN MODE (4) : 1280x960 MIDDLE SPEED
                                nReceiveCount++;
                                if (nReceiveCount < 5)
                                {
                                    break;
                                }
                            }
                            else if (nScanMode == PHOTO6_SCAN_MODE)
                            {
                                // PHOTO SCAN MODE (6) : 2560x1920 LOW SPEED
                                nReceiveCount++;
                                if (nReceiveCount < 10)
                                {
                                    break;
                                }
                            }
                            else if (nScanMode == PHOTO3_SCAN_MODE)
                            {
                                // PHOTO SCAN MODE (3) : 1280x960 HIGH SPEED
                                nReceiveCount++;
                                if (nReceiveCount < 3)
                                {
                                    break;
                                }
                            }
                            nReceiveCount = 0;

                            // SE Image Data : 수신된 명령어 데이터 처리(실행) 하기
                            AfxSEScanDataImageProcess(pThreadParams);
                            break;
                        }
                        case WAIT_TIMEOUT:   // 이벤트 대기시간 경과 : Loop 순환
                        {
                            // 쓰레드 LOOP 순환
                            pThreadParams.bThreadDone = FALSE;
                            break;
                        }
                        case WAIT_FAILED:    // 이벤트 함수 오류 : 쓰레드 종료
                        {
                            // 쓰레드 종료 확인 및 쓰레드 종료
                            bLoop = FALSE;
                            break;
                        }
                    }
                }

                ///
                // 시스템 멈춤없이 다른 작업 실행을 위한 메시지 처리
                ///
                
                //while (::PeekMessage(&msg, NULL, 0, 0, PM_REMOVE))
                //{
                //    TranslateMessage(&msg);
                //    DispatchMessage(&msg);
                //}
            
                ///

                ///
                // 쓰레드 종료 (강제)결정 확인하기
                ///
                if (pThreadParams.bThreadStop == TRUE)
                {
                    // 쓰레드 종료 확인 및 쓰레드 종료
                    bLoop = FALSE;
                }
                else
                {
                    // 쓰레드 종료 (강제)결정 확인하기
                    if (m_pIntegrated.bScanThreadStop == TRUE)
                    {
                        // 쓰레드 종료 확인 및 쓰레드 종료
                        bLoop = FALSE;
                    }
                }
                ///
            }

            //
            // 쓰레드 종료 확인
            pThreadParams.bThreadDone = TRUE;
            //

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:16911 ----
        //
        // SE Image Data : 수신 데이터 처리(실행) 및 이미지 데이터 생성, 처리하기
        //
        public int AfxSEScanDataImageProcess(SEIMAGEDATATHREADPARAMS pThreadParams)
        {
            if (pThreadParams == null) return RET_ERROR_SEM_THREAD_PARAMS;               // SE Image Data 쓰레드 파라메터
            if (pThreadParams.hFT232Handle == IntPtr.Zero) return RET_ERROR_FT232H_SEM_HANDLE; // SE Image Data 장치 핸들

            int   n  = 0;
            byte  bt = 0;
            //MSG   msg;

            uint dwBytesRead = 0;
            uint dwRXBytes   = 0;
            uint dwTXBytes   = 0;
            uint dwEvent     = 0;
            uint dwRXCount   = 0;

            uint ftStatus = 0;

            //
            // SE Image Data 스캔을 종료할 경우
            // 수신 버퍼에 있는 잔류 데이터를 읽은 후, 종료한다.
            //
            if (pThreadParams.bScanDataReceive == FALSE || pThreadParams.bThreadStop == TRUE)
            {
                // CAPTURE STOP

                dwBytesRead = 0;
                dwRXBytes   = 0;
                dwTXBytes   = 0;
                dwEvent     = 0;

                int bStopLoop = FALSE;

                while (bStopLoop == FALSE)
                {
                    // SE Image Data Read and Process...
                    AfxFT232HGetStatus(pThreadParams.nFT232HIndex, ref dwRXBytes, ref dwTXBytes, ref dwEvent);

                    // 받은 데이터 확인
                    if (dwRXBytes < 1)
                    {
                        //return RET_SUCCESS_OK;
                        bStopLoop = TRUE;
                        continue; // LOOP 종료
                    }

                    // 수신된 자료가 모두 처리될 때 까지 작업한다
                    while (dwRXBytes > 0)
                    {
                        // 남은 자료갯수 확인
                        if (dwRXBytes < SCAN_IMAGE_PACKET_SIZE)
                        {
                            // 설정한 패킷 크기 보다 작은 경우
                            dwRXCount = dwRXBytes;
                        }
                        else
                        {
                            // 설정한 패킷 크기 보다 같거나 큰 경우 
                            dwRXCount = SCAN_IMAGE_PACKET_SIZE;
                        }

                        // Read Buffer Clear
                        ZeroMem(pThreadParams.szFTReadBuf);

                        // USB Data Read
                        ftStatus = AfxFT232HRead(pThreadParams.nFT232HIndex, pThreadParams.szFTReadBuf, dwRXCount, ref dwBytesRead);
                        if (ftStatus != FT_OK)
                        {
                            // 쓰레드를 (강제)종료한다...
                            //pThreadParams->bThreadStop = TRUE;
                            return RET_ERROR_FT232H_SBR_READ;
                        }

                        // 읽은 자료 갯수 만큼 남은 자료 갯수를 차감한다
                        dwRXBytes -= dwBytesRead;
                    }
                }

                // 쓰레드 강제 종료...
                if (m_pIntegrated.bScanThreadStop == TRUE)
                {
                    // 수신된 자료를 모두 처리 하였다면
                    // 쓰레드를 (강제)종료한다...
                    pThreadParams.bThreadStop = TRUE;
                }

                return RET_SUCCESS_OK;
            }
            //

            //
            //
            //

            dwBytesRead = 0;
            dwRXBytes   = 0;
            dwTXBytes   = 0;
            dwEvent     = 0;
            n = 0;

            // SE Image Data Read and Process...
            AfxFT232HGetStatus(pThreadParams.nFT232HIndex, ref dwRXBytes, ref dwTXBytes, ref dwEvent);

            // 받은 데이터 확인
            if (dwRXBytes < 1)
            {
                return RET_SUCCESS_OK;
            }

            // 수신된 자료가 모두 처리될 때 까지 작업한다
            while (dwRXBytes > 0)
            {
                // 2020.05.01
                // 최대 수신 버퍼 크기 조정

                // 남은 자료갯수 확인
                if (dwRXBytes < SCAN_IMAGE_PACKET_SIZE)
                {
                    // 설정한 패킷 크기 보다 작은 경우
                    dwRXCount = dwRXBytes;
                }
                else
                {
                    // 설정한 패킷 크기 보다 같거나 큰 경우 
                    dwRXCount = SCAN_IMAGE_PACKET_SIZE;
                }

                // Read Buffer Clear
                ZeroMem(pThreadParams.szFTReadBuf);

                // USB Data Read
                ftStatus = AfxFT232HRead(pThreadParams.nFT232HIndex, pThreadParams.szFTReadBuf, dwRXCount, ref dwBytesRead);
                if (ftStatus != FT_OK)
                {
                    //continue;

                    // 2020.05.12
                    // 쓰레드 계속 사용한다...
                    pThreadParams.bThreadStop = FALSE;
                    return RET_SUCCESS_OK;
                }
                if (dwBytesRead < 1)
                {
                    //continue;

                    // 2020.05.12
                    // 쓰레드 계속 사용한다...
                    pThreadParams.bThreadStop = FALSE;
                    return RET_SUCCESS_OK;
                }

                //
                // AUTO CONTROL MODE EXECUTE CHECK?
                //

                // SE SCAN DATA CLIENT 전송 ( NORMAL MODE )
                if (m_nSEScanDataAutoModeExecute == SE_SCAN_DATA_NORMAL_EXECUTE)
                {
                    //
                    // 수신된 SCAN IMAGE DATA 클라이언트 전송 메시지를 보낸다.
                    //
                    if (m_hOwnerWnd != NULL)
                    {
                        // 2020.05.04
                        // SE SCAN IMAGE DATA 전송 메시지 보내기 (카운트 DATA)
                        SendMessage(m_hOwnerWnd, UM_SEM_SE_SCAN_IMAGE_DATA, (int)dwBytesRead, pThreadParams.szFTReadBuf);
                    }
                }
                // SE SCAN DATA 를 이용한 AUTO FOCUS EXECUTE
                else if (m_nSEScanDataAutoModeExecute == SE_SCAN_DATA_AUTO_FOCUS_EXECUTE)
                {
                    //
                    // 수신된 SCAN IMAGE DATA 분석을 위해 Owner 윈도우에 전송 메시지를 보낸다.
                    //
                    if (m_hOwnerWnd != NULL)
                    {
                        // 2020.05.04
                        // SE SCAN IMAGE DATA 전송 메시지 보내기 (카운트 DATA)
                        SendMessage(m_hOwnerWnd, UM_SEM_SE_SCAN_IMAGE_DATA_AUTO_FOCUS, (int)dwBytesRead, pThreadParams.szFTReadBuf);
                    }
                }
                // SE SCAN DATA 를 이용한 AUTO CONTRAST EXECUTE
                else if (m_nSEScanDataAutoModeExecute == SE_SCAN_DATA_AUTO_CONTRAST_EXECUTE)
                {
                    //
                    // 수신된 SCAN IMAGE DATA 분석을 위해 Owner 윈도우에 전송 메시지를 보낸다.
                    //
                    if (m_hOwnerWnd != NULL)
                    {
                        // 2020.05.04
                        // SE SCAN IMAGE DATA 전송 메시지 보내기 (카운트 DATA)
                        SendMessage(m_hOwnerWnd, UM_SEM_SE_SCAN_IMAGE_DATA_AUTO_CONTRAST, (int)dwBytesRead, pThreadParams.szFTReadBuf);
                    }
                }
                // SE SCAN DATA 를 이용한 AUTO BRIGHTNESS EXECUTE
                else if (m_nSEScanDataAutoModeExecute == SE_SCAN_DATA_AUTO_BRIGHTNESS_EXECUTE)
                {
                    //
                    // 수신된 SCAN IMAGE DATA 분석을 위해 Owner 윈도우에 전송 메시지를 보낸다.
                    //
                    if (m_hOwnerWnd != NULL)
                    {
                        // 2020.05.04
                        // SE SCAN IMAGE DATA 전송 메시지 보내기 (카운트 DATA)
                        SendMessage(m_hOwnerWnd, UM_SEM_SE_SCAN_IMAGE_DATA_AUTO_BRIGHTNESS, (int)dwBytesRead, pThreadParams.szFTReadBuf);
                    }
                }
                // SE SCAN DATA 를 이용한 AUTO CONTRAST+BRIGHTNESS EXECUTE
                else if (m_nSEScanDataAutoModeExecute == SE_SCAN_DATA_AUTO_CONTRAST_BRIGHT_EXECUTE)
                {
                    //
                    // 수신된 SCAN IMAGE DATA 분석을 위해 Owner 윈도우에 전송 메시지를 보낸다.
                    //
                    if (m_hOwnerWnd != NULL)
                    {
                        // 2020.05.04
                        // SE SCAN IMAGE DATA 전송 메시지 보내기 (카운트 DATA)
                        SendMessage(m_hOwnerWnd, UM_SEM_SE_SCAN_IMAGE_DATA_AUTO_CONT_BRIG, (int)dwBytesRead, pThreadParams.szFTReadBuf);
                    }
                }
                // SE SCAN DATA 를 이용한 AUTO GUN ALIGNMENT EXECUTE
                else if (m_nSEScanDataAutoModeExecute == SE_SCAN_DATA_AUTO_GUN_ALIGN_EXECUTE)
                {
                    //
                    // 수신된 SCAN IMAGE DATA 분석을 위해 Owner 윈도우에 전송 메시지를 보낸다.
                    //
                    if (m_hOwnerWnd != NULL)
                    {
                        // 2020.05.04
                        // SE SCAN IMAGE DATA 전송 메시지 보내기 (카운트 DATA)
                        SendMessage(m_hOwnerWnd, UM_SEM_SE_SCAN_IMAGE_DATA_AUTO_GUN_ALIGN, (int)dwBytesRead, pThreadParams.szFTReadBuf);
                    }
                }


                // 읽은 자료 갯수 만큼 남은 자료 갯수를 차감한다
                dwRXBytes -= dwBytesRead;

                //
                //
                //

            } // 수신된 자료가 모두 처리될 때 까지 작업한다 while()문 처리

            // 쓰레드 계속 사용한다...
            pThreadParams.bThreadStop = FALSE;

            return RET_SUCCESS_OK;

        }

    }
}
