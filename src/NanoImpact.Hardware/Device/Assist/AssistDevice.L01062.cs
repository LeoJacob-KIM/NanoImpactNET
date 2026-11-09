// 원본 ModularAssistDevice.cpp:1062-2079 을 NanoImpact.CodeGen translate로 옮긴 뒤 손으로 고친 코드.
#pragma warning disable IDE1006, IDE0059, CS0162, CS0168, CS0219
using System;
using NanoImpact.Core.Native;
using static NanoImpact.Core.Def;
using static NanoImpact.Core.Native.CRuntime;

namespace NanoImpact.Hardware.Device
{
    public sealed partial class AssistDevice
    {
        // ---- ModularAssistDevice.cpp:1062 ----
        // SEM USB DEVICE CONTROL : BEAM ALIGN 설정
        public void SetSEMControlBeamAlignValue(int nBAX = 0, int nBAY = 0, int nExecute = 0)
        {
            /*
            // USB DEVICE : BEAM ALIGN XY VALUE 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData;
            // 초기화
            memset(&RequestDeviceData, 0x00, sizeof(REQUEST_USB_DEVICE_DATA));
            // SET USB DEVICE DATA : SEM CONTROL -> BEAM ALIGN XY
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_CONTROL_BEAM_ALIGN_VALUE;
            RequestDeviceData.nRequestType    = 0;        // int:0, double:1
            RequestDeviceData.nRequestOption  = 3;        // Data Count
            RequestDeviceData.nRequestData[0] = nBAX;     // BEAM ALIGN X
            RequestDeviceData.nRequestData[1] = nBAY;     // BEAM ALIGN Y
            RequestDeviceData.nRequestData[2] = nExecute; // USB EXECUTE (0:설정, 1:설정및실행)
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_CONTROL_BEAM_ALIGN_VALUE, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != NULL)
            {
                ((CNanoImpact2020cfeDlg*)m_pOwnerWindow)->OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, (LPARAM)&RequestDeviceData);
            }
            */
            
            // USB DEVICE : BEAM ALIGN XY VALUE 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceSemControlBeamAlignValue(nBAX, nBAY, nExecute);
        }

        // ---- ModularAssistDevice.cpp:1092 ----
        // SEM USB DEVICE CONTROL : IMAGE SHIFT 설정
        public void SetSEMControlImageShiftValue(int nISX = 0, int nISY = 0, int nExecute = 0)
        {
            /*
            // USB DEVICE : IMAGE SHIFT XY VALUE 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData;
            // 초기화
            memset(&RequestDeviceData, 0x00, sizeof(REQUEST_USB_DEVICE_DATA));
            // SET USB DEVICE DATA : SEM CONTROL -> IMAGE SHIFT XY
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_CONTROL_IMAGE_SHIFT_VALUE;
            RequestDeviceData.nRequestType    = 0;        // int:0, double:1
            RequestDeviceData.nRequestOption  = 3;        // Data Count
            RequestDeviceData.nRequestData[0] = nISX;     // IMAGE SHIFT X
            RequestDeviceData.nRequestData[1] = nISY;     // IMAGE SHIFT Y
            RequestDeviceData.nRequestData[2] = nExecute; // USB EXECUTE (0:설정, 1:설정및실행)
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_CONTROL_IMAGE_SHIFT_VALUE, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != NULL)
            {
                ((CNanoImpact2020cfeDlg*)m_pOwnerWindow)->OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, (LPARAM)&RequestDeviceData);
            }
            */
            
            // USB DEVICE : IMAGE SHIFT XY VALUE 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceSemControlImageShiftValue(nISX, nISY, nExecute);
        }

        // ---- ModularAssistDevice.cpp:1122 ----
        // SEM USB DEVICE CONTROL : STIG 설정
        public void SetSEMControlStigValue(int nStigX = 0, int nStigY = 0, int nExecute = 0)
        {
            /*
            // USB DEVICE : STIG XY VALUE 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData;
            // 초기화
            memset(&RequestDeviceData, 0x00, sizeof(REQUEST_USB_DEVICE_DATA));
            // SET USB DEVICE DATA : SEM CONTROL -> STIG XY
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_CONTROL_STIG_VALUE;
            RequestDeviceData.nRequestType    = 0;        // int:0, double:1
            RequestDeviceData.nRequestOption  = 3;        // Data Count
            RequestDeviceData.nRequestData[0] = nStigX;   // STIG X
            RequestDeviceData.nRequestData[1] = nStigY;   // STIG Y
            RequestDeviceData.nRequestData[2] = nExecute; // USB EXECUTE (0:설정, 1:설정및실행)
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_CONTROL_STIG_VALUE, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != NULL)
            {
                ((CNanoImpact2020cfeDlg*)m_pOwnerWindow)->OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, (LPARAM)&RequestDeviceData);
            }
            */
            
            // USB DEVICE : STIG XY VALUE 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceSemControlStigValue(nStigX, nStigY, nExecute);
        }

        // ---- ModularAssistDevice.cpp:1152 ----
        // SEM USB DEVICE CONTROL : MAGNIFICATION 설정
        public void SetSEMControlMagnificationIndex(int nMagIndex = 0, double dOLWD = 0.0)
        {
            /*
            // USB DEVICE : MAGNIFICATION INDEX 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData;
            // 초기화
            memset(&RequestDeviceData, 0x00, sizeof(REQUEST_USB_DEVICE_DATA));
            // SET USB DEVICE DATA : SEM CONTROL -> MAGNIFICATION INDEX
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_CONTROL_MAGNIFICATION_INDEX;
            RequestDeviceData.nRequestType    = 0;         // int:0, double:1
            RequestDeviceData.nRequestOption  = 1;         // Data Count
            RequestDeviceData.nRequestData[0] = nMagIndex; // MAG INDEX
            RequestDeviceData.dRequestData[0] = dOLWD;     // OL_WD
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_CONTROL_MAGNIFICATION_INDEX, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != NULL)
            {
                ((CNanoImpact2020cfeDlg*)m_pOwnerWindow)->OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, (LPARAM)&RequestDeviceData);
            }
            */
            // USB DEVICE : MAGNIFICATION INDEX 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceSemControlMagnificationIndex(nMagIndex);
        }

        // ---- ModularAssistDevice.cpp:1180 ----
        // SEM USB DEVICE CONTROL : SCAN ROTATION 설정
        public void SetSEMScanRotationValueUsbDeviceData(int nValue = 0, int nExecute = 0)
        {
            /*
            // USB DEVICE : SCAN ROTATION VALUE 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData;
            // 초기화
            memset(&RequestDeviceData, 0x00, sizeof(REQUEST_USB_DEVICE_DATA));
            // SET USB DEVICE DATA : SEM CONTROL -> SCAN ROTATION VALUE
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_SCAN_ROTATION_VALUE;
            RequestDeviceData.nRequestType    = 0; // int:0, double:1
            RequestDeviceData.nRequestOption  = 1; // Data Count
            RequestDeviceData.nRequestData[0] = nValue;   // m_nSemScanRotationScrollPos;
            RequestDeviceData.nRequestData[1] = nExecute; // USB DEVICE EXECUTE
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_SCAN_ROTATION_VALUE, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != NULL)
            {
                ((CNanoImpact2020cfeDlg*)m_pOwnerWindow)->OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, (LPARAM)&RequestDeviceData);
            }
            */
            
            // USB DEVICE : SCAN ROTATION VALUE 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceScanRotationValue(nValue, nExecute);
        }

        // ---- ModularAssistDevice.cpp:1209 ----
        // SEM USB DEVICE CONTROL : CONTRAST Value 설정
        public void SetSEMContrastValueUsbDeviceData(int nContrast, string strData, int nExecute)
        {
            // USB DEVICE : CONTRAST VALUE 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : SEM CONTROL -> CONTRAST VALUE
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_CONTRAST_VALUE;
            RequestDeviceData.nRequestType    = 0;         // int:0, double:1
            RequestDeviceData.nRequestOption  = 1;         // Data Count
            RequestDeviceData.nRequestData[0] = nContrast; // m_nSemContrastScrollPos;
            RequestDeviceData.nRequestData[1] = nExecute;  // USB DEVICE EXECUTE
            // Position String
            strcpy_s(RequestDeviceData.szData, strData); // 원본 (CStringA) = ANSI(CP949) 변환
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_CONTRAST_VALUE, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != null)
            {
                m_pOwnerWindow.OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, RequestDeviceData);
            }

            
            // USB DEVICE : CONTRAST VALUE 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceContrastValue(nContrast, nExecute);
        }

        // ---- ModularAssistDevice.cpp:1239 ----
        // SEM USB DEVICE CONTROL : BRIGHTNESS Value 설정
        public void SetSEMBrightnessValueUsbDeviceData(int nBright, string strData, int nExecute)
        {
            // USB DEVICE : BRIGHTNESS VALUE 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : SEM CONTROL -> BRIGHTNESS VALUE
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_BRIGHT_VALUE;
            RequestDeviceData.nRequestType    = 0;        // int:0, double:1
            RequestDeviceData.nRequestOption  = 1;        // Data Count
            RequestDeviceData.nRequestData[0] = nBright;  // m_nSemBrightnessScrollPos;
            RequestDeviceData.nRequestData[1] = nExecute; // USB DEVICE EXECUTE
            // Position String
            strcpy_s(RequestDeviceData.szData, strData); // 원본 (CStringA) = ANSI(CP949) 변환
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_BRIGHT_VALUE, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != null)
            {
                m_pOwnerWindow.OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, RequestDeviceData);
            }

            
            // USB DEVICE : BRIGHTNESS VALUE 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceBrightnessValue(nBright, nExecute);
        }

        // ---- ModularAssistDevice.cpp:1269 ----
        // SEM USB DEVICE CONTROL : FOCUS OL Value 설정
        public void SetSEMControlFocusOLValue(int nCoarse = 0, int nFine = 0)
        {
            
            // USB DEVICE : FOCUS OL VALUE 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceSemControlOLValueChange(nCoarse, nFine);
        }

        // ---- ModularAssistDevice.cpp:1279 ----
        // SEM USB DEVICE CONTROL : FOCUS COARSE Value 설정
        public void SetSEMControlFocusCoarseValue(int nCoarse = 0)
        {
            
            // USB DEVICE : FOCUS COARSE VALUE 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceSemControlFocusCoarseValueChange(nCoarse);
        }

        // ---- ModularAssistDevice.cpp:1289 ----
        // SEM USB DEVICE CONTROL : FOCUS COARSE Value 설정
        public void SetSEMFocusCoarseFineValueUsbDeviceData(int nCoarse, int nFine, string strData, int nExecute)
        {
            
            // SEM CONTROL을 위한 FOCUS COARSE, FINE VALUE 설정
            // USB DEVICE SET 명령어설정
            SetUsbDeviceFocusCoarseFineValue(nCoarse, nFine, nExecute);

            // WD SETUP ENABLED ?
            if (m_dev.AfxGetIntegratedPointer().bWDSetupFlag == TRUE)
            {
                // WD에 따른 OL값 설정 : INTEGRATED 변수 설정
                m_dev.AfxGetIntegratedPointer().nWDOLCoarse[m_dev.AfxGetIntegratedPointer().nWDPosition] = nCoarse;
                // WD OL 값 설정
                if (m_pOwnerWindow != null)
                {
                    m_pOwnerWindow.SetWDSetupEnabledOLValue(nCoarse);
                }
            }

            // OL(FOCUS COARSE)에 따른 WDx10 값 가져오기
            int nWDx10 = m_dev.AfxGetSEMOLWDx10Value();

            // USB DEVICE : FOCUS COARSE/FINE VALUE 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : SEM CONTROL -> FOCUS COARSE/FINE VALUE
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_FOCUS_COARSE_FINE_VALUE;
            RequestDeviceData.nRequestType    = 0; // int:0, double:1
            RequestDeviceData.nRequestOption  = 2; // Data Count
            RequestDeviceData.nRequestData[0] = nCoarse;  // m_nSemFocusCoarseValue;
            RequestDeviceData.nRequestData[1] = nFine;    // m_nSemFocusFineValue;
            RequestDeviceData.nRequestData[2] = nWDx10;   // WDx10 위치값
            // Position String
            strcpy_s(RequestDeviceData.szData, strData); // 원본 (CStringA) = ANSI(CP949) 변환
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_FOCUS_COARSE_FINE_VALUE, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != null)
            {
                m_pOwnerWindow.OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, RequestDeviceData);
            }
        }

        // ---- ModularAssistDevice.cpp:1335 ----
        // SEM USB DEVICE CONTROL : PMT Relay On/Off
        public void SetSEMPMTRelayOnOffValueUsbDeviceData(int nRelayOnOff = 0)
        {
            /*
            // USB DEVICE : PMT RELAY ON/OFF VALUE 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData;
            // 초기화
            memset(&RequestDeviceData, 0x00, sizeof(REQUEST_USB_DEVICE_DATA));
            // SET USB DEVICE DATA : SEM CONTROL -> PMT RELAY ON/OFF VALUE
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_PMT_RELAY_ON_OFF_VALUE;
            RequestDeviceData.nRequestType    = 0; // int:0, double:1
            RequestDeviceData.nRequestOption  = 1; // Data Count
            RequestDeviceData.nRequestData[0] = nRelayOnOff;
            // 메시지전송
            ::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_PMT_RELAY_ON_OFF_VALUE, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != NULL)
            {
                ((CNanoImpact2020cfeDlg*)m_pOwnerWindow)->OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, (LPARAM)&RequestDeviceData);
            }
            */
            
            // USB DEVICE : PMT RELAY ON/OFF VALUE 적용
            // USB DEVICE SET 명령어설정
            SetUsbDevicePMTRelayOnOffValue(nRelayOnOff);
        }

        // ---- ModularAssistDevice.cpp:1363 ----
        // SEM USB DEVICE CONTROL : DETECTOR On/Off
        public void SetSEMDetectorOnOffValueUsbDeviceData(int nOnOff = 0)
        {
            /*
            // USB DEVICE : DETECTOR ON/OFF VALUE 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData;
            // 초기화
            memset(&RequestDeviceData, 0x00, sizeof(REQUEST_USB_DEVICE_DATA));
            // SET USB DEVICE DATA : SEM CONTROL -> DETECTOR ON/OFF VALUE
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_DETECTOR_ON_OFF_VALUE;
            RequestDeviceData.nRequestType    = 0; // int:0, double:1
            RequestDeviceData.nRequestOption  = 1; // Data Count
            RequestDeviceData.nRequestData[0] = nOnOff;
            // 메시지전송
            ::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_DETECTOR_ON_OFF_VALUE, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != NULL)
            {
                ((CNanoImpact2020cfeDlg*)m_pOwnerWindow)->OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, (LPARAM)&RequestDeviceData);
            }
            */
            
            // USB DEVICE : DETECTOR ON/OFF VALUE 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceDetectorOnOffValue(nOnOff);
        }

        // ---- ModularAssistDevice.cpp:1391 ----
        // SEM USB DEVICE CONTROL : WOBBLE INDEX 설정
        public void SetSEMWobbleIndexUsbDeviceData(int nSemWobbleCombo = 0)
        {
            /*
            // USB DEVICE : WOBBLE INDEX 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData;
            // 초기화
            memset(&RequestDeviceData, 0x00, sizeof(REQUEST_USB_DEVICE_DATA));
            // SET USB DEVICE DATA : SEM CONTROL -> WOBBLE INDEX
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_WOBBLE_INDEX;
            RequestDeviceData.nRequestType    = 0; // int:0, double:1
            RequestDeviceData.nRequestOption  = 1; // Data Count
            RequestDeviceData.nRequestData[0] = nSemWobbleCombo;
            // 메시지전송
            ::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_WOBBLE_INDEX, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != NULL)
            {
                ((CNanoImpact2020cfeDlg*)m_pOwnerWindow)->OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, (LPARAM)&RequestDeviceData);
            }
            */
            
            // USB DEVICE : WOBBLE INDEX 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceWobbleIndex(nSemWobbleCombo);
        }

        // ---- ModularAssistDevice.cpp:1419 ----
        // SEM USB DEVICE CONTROL : WOBBLE FOCUS 설정
        public void SetSEMWobblerFocusCoarseValueUsbDeviceData(int nCoarse = 0, int nExecute = 0)
        {
            /*
            // USB DEVICE : FOCUS COARSE/FINE VALUE 적용
            CString strText = _T("");
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData;
            // 초기화
            memset(&RequestDeviceData, 0x00, sizeof(REQUEST_USB_DEVICE_DATA));
            // SET USB DEVICE DATA : SEM CONTROL -> FOCUS COARSE/FINE VALUE
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_WOBBLER_FOCUS_COARSE_VALUE;
            RequestDeviceData.nRequestType    = 0; // int:0, double:1
            RequestDeviceData.nRequestOption  = 2; // Data Count
            RequestDeviceData.nRequestData[0] = nCoarse;  // FocusCoarseValue;
            RequestDeviceData.nRequestData[1] = nExecute; // USB DEVICE EXECUTE
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_FOCUS_COARSE_FINE_VALUE, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != NULL)
            {
                ((CNanoImpact2020cfeDlg*)m_pOwnerWindow)->OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, (LPARAM)&RequestDeviceData);
            }
            */
            
            // SEM CONTROL을 위한 WOBBLER FOCUS COARSE VALUE 설정
            // USB DEVICE SET 명령어설정
            SetUsbDeviceWobblerFocusCoarseValue(nCoarse, nExecute);
            // WD SETUP ENABLED ?
            if (m_dev.AfxGetIntegratedPointer().bWDSetupFlag == TRUE)
            {
                // WD에 따른 OL값 설정 : INTEGRATED 변수 설정
                m_dev.AfxGetIntegratedPointer().nWDOLCoarse[m_dev.AfxGetIntegratedPointer().nWDPosition] = nCoarse;
            }

        }

        // ---- ModularAssistDevice.cpp:1456 ----
        // SEM USB DEVICE CONTROL : WOBBLE STIG-X 설정
        public void SetSEMWobblerStigXValueUsbDeviceData(int nStigX = 0, int nExecute = 0)
        {
            // 2020.05.06
            // SEM CONTROL을 위한 WOBBLER STIG-X VALUE 설정
            // USB DEVICE SET 명령어설정
            SetUsbDeviceWobblerStigXValue(nStigX, nExecute);
        }

        // ---- ModularAssistDevice.cpp:1466 ----
        // SEM USB DEVICE CONTROL : WOBBLE STIG-Y 설정
        public void SetSEMWobblerStigYValueUsbDeviceData(int nStigY = 0, int nExecute = 0)
        {
            // 2020.05.06
            // SEM CONTROL을 위한 WOBBLER STIG-Y VALUE 설정
            // USB DEVICE SET 명령어설정
            SetUsbDeviceWobblerStigYValue(nStigY, nExecute);
        }

        // ---- ModularAssistDevice.cpp:1475 ----
        public void UsbDeviceConnect()
        {
            int nRet = 0;

            //
            // USB FT232H SBR Port Connect...
            //
            if (m_bSbrConnected == FALSE)
            {
                nRet = m_dev.AfxFT232HPortOpen(IDX_FT232H_SCAN_BRIDGE_DEVICE);
                if (nRet == RET_SUCCESS_OK)
                {
                    m_bSbrConnected = TRUE;
                    // USB FT232H SBR Port Connected.
                    //m_UsbSbrCntEdit.SetWindowTextW(_T("Connected.!!!"));
                    // Sbr On/Off Image
                    //SetBitmapImage(IMAGE_PNG_SBR_ON_STATE);
                    //AfxMessageBox(_T("USB FT232H SBR Device Connected !!!"));
                }
                else
                {
                    m_bSbrConnected = FALSE;
                    // USB FT232H SBR Port Error -> Can't connected.
                    //m_UsbSbrCntEdit.SetWindowTextW(_T("Disconnected.!!!"));
                    // Sbr On/Off Image
                    //SetBitmapImage(IMAGE_PNG_SBR_OFF_STATE);
                    AfxMessageBox("USB FT232H SBR Device Error, Can't connected.!!!");
                }
            }

            //
            // USB FT232H SEM Port Connect...
            //
            if (m_bSemConnected == FALSE)
            {
                nRet = m_dev.AfxFT232HPortOpen(IDX_FT232H_SEM_IMAGE_DEVICE);
                if (nRet == RET_SUCCESS_OK)
                {
                    m_bSemConnected = TRUE;
                    // USB FT232H SEM Port Connected.
                    //m_UsbSemCntEdit.SetWindowTextW(_T("Connected.!!!"));
                    // Se On/Off Image
                    //SetBitmapImage(IMAGE_PNG_SE_ON_STATE);
                    //AfxMessageBox(_T("USB FT232H SEM Device Connected !!!"));
                }
                else
                {
                    m_bSemConnected = FALSE;
                    // USB FT232H SEM Port Error -> Can't connected.
                    //m_UsbSemCntEdit.SetWindowTextW(_T("Disconnected.!!!"));
                    // Se On/Off Image
                    //SetBitmapImage(IMAGE_PNG_SE_OFF_STATE);
                    AfxMessageBox("USB FT232H SEM Device Error, Can't connected.!!!");
                }
            }

            //
            // USB FT232H BSE Port Connect...
            //
            if (m_bBseConnected == FALSE)
            {
                nRet = m_dev.AfxFT232HPortOpen(IDX_FT232H_BSE_IMAGE_DEVICE);
                if (nRet == RET_SUCCESS_OK)
                {
                    m_bBseConnected = TRUE;
                    // USB FT232H SBR Port Connected.
                    //m_UsbBseCntEdit.SetWindowTextW(_T("Connected.!!!"));
                    // Bse On/Off Image
                    //SetBitmapImage(IMAGE_PNG_BSE_ON_STATE);
                    //AfxMessageBox(_T("USB FT232H BSE Device Connected !!!"));
                }
                else
                {
                    m_bBseConnected = FALSE;
                    // USB FT232H BSE Port Error -> Can't connected.
                    //m_UsbBseCntEdit.SetWindowTextW(_T("Disconnected.!!!"));
                    // Bse On/Off Image
                    //SetBitmapImage(IMAGE_PNG_BSE_OFF_STATE);
                    //AfxMessageBox(_T("USB FT232H BSE Device Error, Can't connected.!!!"));
                }
            }

            //
            // System 공유 변수 초기화
            //
            // 1.SEM Model 초기화 및 공용 변수 초기화
            // 2.WD에 따른 OL_Coarse, ScanRotation, Zooming, MinMag 보정 설정값 초기화
            // 3.WD에 따른 OL_Coarse, ScanRotation, Zooming, MinMag 보정값 10등분 선형화 작업
            m_dev.AfxInitilizePublicParameters();

            //
            // 서버 프로그램 시작 후, USB 장치에 연결되면 실행한다
            // 시스템을 초기화 하기 위해서는 Sbr, Se Image 두개의 USB 장치는 연결 되어야 한다
            if (m_bSbrConnected == TRUE && m_bSemConnected == TRUE)
            {
                // SEM Amp, Ref
                string strText = "";
                double dAmp = 0.0;
                double dRef = 0.0;

                //
                // System 공유 변수 초기화
                //
                // 1.SEM Model 초기화 및 공용 변수 초기화
                // 2.WD에 따른 OL_Coarse, ScanRotation, Zooming, MinMag 보정 설정값 초기화
                // 3.WD에 따른 OL_Coarse, ScanRotation, Zooming, MinMag 보정값 10등분 선형화 작업
                //AfxInitilizePublicParameters();
                //

                // SEM Model ComboBox
                //GetDlgItem(IDC_SEM_MODEL_COMBO)->EnableWindow(TRUE);

                // USB Connect Flag
                m_bUsbConnected = TRUE;

                // SEM Model ComboBox
                //GetDlgItem(IDC_SEM_MODEL_COMBO)->EnableWindow(FALSE);

                //
                // SEM USB Device 사용설정
                // FT232H : USB Device 사용여부 설정 : 사용설정
                //
                m_dev.AfxFT232HDeviceUse(1);

                //
                // OWNER 윈도우 설정
                //
                //AfxUSBFT232HSetOwnerWindow(m_hOwnerWnd);

                //
                // Chamber Open/Closed
                //
                //m_bChamberOpen = FALSE;
                //m_ChamberStatusEdit.SetWindowTextW(_T(""));
                //

                //
                // SEM CL Model Initilized (CL MODEL 시스템 초기화)
                //
                //strText = _T("");
                //m_SemModelAmpEdit.GetWindowTextW(strText);
                //dAmp = _wtof(strText);
                //strText = _T("");
                //m_SemModelRefEdit.GetWindowTextW(strText);
                //dRef = _wtof(strText);

                // SEM MODEL 설정 및 CL1,CL2  AMP, REF 값 설정
                m_dev.AfxSetSemModel(0, 2.4, 2.0);

                //
                // 저장된 INTEGRATED DATA 파일 불러오기
                //
                m_dev.AfxGetIntegratedDataFromFile();
                //
                // WD에 따른 OL_Coarse, ScanRotation, Zooming, MinMag 보정값 10등분 선형화 작업
                m_dev.AfxIntegratedSetWDOLSrotZoomingLinearize();
                //

                //
                // 저장된 SCAN ROTATION CALIBRATION DATA 파일 불러오기
                //
                m_dev.AfxGetScanRotationCalibrationDataFromFile();
                //

                //
                // SEM SYSTEM INITILIZE : SEM USB 장치가 연결되면 무조건 장치 초기화 한다
                //
                // 1. HV POWER ON/OFF FLAG : OFF 설정
                // 2. GUN VALVE CLOSE
                // 3. SEM SPELLMAN HV DISABLED (GUN, PMT, SCIN, COLLECTOR)
                // 4. SE SCAN IMAGE CAPTURE OFF
                // 5. SEM MODEL에 따른 CL값 초기화 (변수 초기화)
                // 6. BSE 시스템 초기화
                // 7. Lens Coil에 흐르는 전류 최소화 ( USB DEVICE 실행 )
                // 8. SCAN 신호 초기화 ( 신호 OFF )
                // 9. 범용 변수 초기화 (Filament값,Bias값,Contrast,Brightness,Stig,ImgShift,스캔모드)
                // 10.SCAN ROTATION 각도 초기값 설정 ( 0도 )
                // 11.직교도 ( 변수초기화 )
                // 12.Scan Bridge Device 상태 정보 읽기 쓰레드 생성 및 실행
                // 13.SEM SE Scan Image Data 쓰레드 생성 및 실행
                // 14.VACCUM 상태값 읽기 실행
                //
                m_dev.AfxSEMSystemInitilize();
                m_bSysInitilized = TRUE;

                //
                // SEM Control 초기 상태 설정
                //
                SetSEMDefaultControlValue();
            }
            //
            else
            {
                // System Initilized
                m_bSysInitilized = FALSE;

                // USB Connect Flag
                m_bUsbConnected = FALSE;

                // INTEGRATED DATA 파일 불러오기
                m_dev.AfxGetIntegratedDataFromFile();

                // SEM Control 초기 상태 설정
                SetSEMDefaultControlValue();

                // SEM Model ComboBox
                //GetDlgItem(IDC_SEM_MODEL_COMBO)->EnableWindow(TRUE);
            }
        }

        // ---- ModularAssistDevice.cpp:1686 ----
        public void UsbDeviceDisconnect()
        {
            //
            // SEM USB Device 사용하지 않음 설정
            m_dev.AfxFT232HDeviceUse(0);

            _host.Sleep(300);

            // 시스템 쓰레드 종료
            m_dev.AfxSEMSystemThreadClose();

            // System Initilized
            m_bSysInitilized = FALSE;

            // USB Connect Flag
            m_bUsbConnected = FALSE;

            // SEM Model ComboBox
            //GetDlgItem(IDC_SEM_MODEL_COMBO)->EnableWindow(TRUE);

            // SEM Model ComboBox
            //GetDlgItem(IDC_SEM_MODEL_COMBO)->EnableWindow(FALSE);
            //

            int nRet = 0;

            //
            // USB FT232H SBR Port Disconnect...
            //
            if (m_bSbrConnected == TRUE)
            {
                nRet = m_dev.AfxFT232HPortClose(IDX_FT232H_SCAN_BRIDGE_DEVICE);
                if (nRet == RET_SUCCESS_OK)
                {
                    m_bSbrConnected = FALSE;
                    // USB FT232H SBR Port Connected.
                    //m_UsbSbrCntEdit.SetWindowTextW(_T("Disconnected.!!!"));
                    // Sbr On/Off Image
                    //SetBitmapImage(IMAGE_PNG_SBR_OFF_STATE);
                    //AfxMessageBox(_T("USB FT232H SBR Device Disconnected.!!!"));
                }
                else
                {
                    m_bSbrConnected = FALSE;
                    // USB FT232H SBR Port Error -> Can't connected.
                    //m_UsbSbrCntEdit.SetWindowTextW(_T("USB FT232H Error.!!!"));
                    // Sbr On/Off Image
                    //SetBitmapImage(IMAGE_PNG_SBR_OFF_STATE);
                    //AfxMessageBox(_T("USB FT232H SBR Device Error, Can't disconnected.!!!"));
                }
            }

            //
            // USB FT232H SEM Port Disconnect...
            //
            if (m_bSemConnected == TRUE)
            {
                nRet = m_dev.AfxFT232HPortClose(IDX_FT232H_SEM_IMAGE_DEVICE);
                if (nRet == RET_SUCCESS_OK)
                {
                    m_bSemConnected = FALSE;
                    // USB FT232H SEM Port Connected.
                    //m_UsbSemCntEdit.SetWindowTextW(_T("Disconnected.!!!"));
                    // Se On/Off Image
                    //SetBitmapImage(IMAGE_PNG_SE_OFF_STATE);
                    //AfxMessageBox(_T("USB FT232H SEM Device Disconnected.!!!"));
                }
                else
                {
                    m_bSemConnected = FALSE;
                    // USB FT232H SEM Port Error -> Can't connected.
                    //m_UsbSemCntEdit.SetWindowTextW(_T("USB FT232H Error.!!!"));
                    // Se On/Off Image
                    //SetBitmapImage(IMAGE_PNG_SE_OFF_STATE);
                    //AfxMessageBox(_T("USB FT232H SEM Device Error, Can't disconnected.!!!"));
                }
            }

            //
            // USB FT232H BSE Port Disconnect...
            //
            if (m_bBseConnected == TRUE)
            {
                nRet = m_dev.AfxFT232HPortClose(IDX_FT232H_BSE_IMAGE_DEVICE);
                if (nRet == RET_SUCCESS_OK)
                {
                    m_bBseConnected = FALSE;
                    // USB FT232H BSE Port Connected.
                    //m_UsbBseCntEdit.SetWindowTextW(_T("Disconnected.!!!"));
                    // Bse On/Off Image
                    //SetBitmapImage(IMAGE_PNG_BSE_OFF_STATE);
                    //AfxMessageBox(_T("USB FT232H BSE Device Disconnected.!!!"));
                }
                else
                {
                    m_bBseConnected = FALSE;
                    // USB FT232H BSE Port Error -> Can't connected.
                    //m_UsbBseCntEdit.SetWindowTextW(_T("USB FT232H Error.!!!"));
                    // Bse On/Off Image
                    //SetBitmapImage(IMAGE_PNG_BSE_OFF_STATE);
                    //AfxMessageBox(_T("USB FT232H BSE Device Error, Can't disconnected.!!!"));
                }
            }

            //
            //
            //

            // 시스템 공용 변수 제거
            m_dev.AfxReleasePublicParameters();

            //
            //
            //
        }

        // ---- ModularAssistDevice.cpp:1803 ----
        public void SetSEMDefaultControlValue()
        {
            //
            // SEM DEVICE CONTROL 변수 기본 설정
            //
            string strText = "";
            int nValue = 0;

            // 프로그램 시작시 INTEGRATED FILE LOAD 후, 실행한다

            //
            // STEP #1
            //

            // GUN VALVE CLOSE : 프로그램 시작시 GUN VALVE를 CLOSE 시킨다
            m_dev.AfxScanBridgeGunValveClose();

            // SPELLMAN HV - GUN HV      : 프로그램 시작시 GUN HV      를 DISABLED 시킨다
            m_dev.AfxScanBridgeSpellmanGunHVOff();
            // SPELLMAN HV - PMT         : 프로그램 시작시 PMT         를 DISABLED 시킨다
            m_dev.AfxIntegratedSpellmanPMTHVOff();
            // SPELLMAN HV - SCINTILATOR : 프로그램 시작시 SCINTILATOR 를 DISABLED 시킨다
            m_dev.AfxIntegratedSpellmanScintilatorHVOff();
            // SPELLMAN HV - COLLECTOR   : 프로그램 시작시 COLLECTOR   를 DISABLED 시킨다
            m_dev.AfxScanBridgeCollectorOff();

            // SE SCAN IMAGE CAPTURE OFF
            m_dev.AfxIntegratedScanImageDataCaptureStop();

            // SCAN 신호 초기화 ( 신호 OFF )
            m_dev.AfxIntegratedResetScanGeneration();

            //
            // STEP #2
            //

            // 프로그램 시작시 SeCommand, CommandState 값을 설정한다
            m_dev.AfxIntegratedSetSeCommandStartValue();

            //
            // STEP #3
            //

            // Gun HV Index 설정
            nValue = m_dev.AfxGetSEMGunHvIndex();

            // Filament 설정
            nValue = m_dev.AfxScanBridgeGetFilamentValue();
            m_dev.AfxScanBridgeSetFilamentValue(nValue);

            // Bias 설정
            nValue = m_dev.AfxScanBridgeGetBiasValue();
            m_dev.AfxScanBridgeSetBiasValue(nValue);

            // Magnification 설정
            nValue = m_dev.AfxGetSEMMagnificationIndex();

            // Spot Size 설정
            nValue = m_dev.AfxIntegratedGetSpotSize();

            // Wobble 설정
            nValue = m_dev.AfxIntegratedGetWobble();

            // Gun Align X 설정
            nValue = m_dev.AfxIntegratedGetGunAlignX();
            m_dev.AfxIntegratedSetGunAlignX(nValue);

            // Gun Align Y 설정
            nValue = m_dev.AfxIntegratedGetGunAlignY();
            m_dev.AfxIntegratedSetGunAlignY(nValue);

            // Beam Align X 설정
            nValue = m_dev.AfxIntegratedGetBeamAlignX();
            m_dev.AfxIntegratedSetBeamAlignX(nValue);

            // Beam Align Y 설정
            nValue = m_dev.AfxIntegratedGetBeamAlignY();
            m_dev.AfxIntegratedSetBeamAlignY(nValue);

            // Image Shift X 설정
            nValue = m_dev.AfxIntegratedGetImageShiftX();
            m_dev.AfxIntegratedSetImageShiftX(nValue);

            // Image Shift Y 설정
            nValue = m_dev.AfxIntegratedGetImageShiftY();
            m_dev.AfxIntegratedSetImageShiftY(nValue);

            // Stig X 설정
            nValue = m_dev.AfxIntegratedGetStigX();
            m_dev.AfxIntegratedSetStigX(nValue);

            // Stig Y 설정
            nValue = m_dev.AfxIntegratedGetStigY();
            m_dev.AfxIntegratedSetStigY(nValue);

            // Contrast 설정
            nValue = m_dev.AfxIntegratedGetContrast();
            m_dev.AfxIntegratedSetContrast(nValue);

            // Brightness 설정
            nValue = m_dev.AfxIntegratedGetBrightness();
            m_dev.AfxIntegratedSetBrightness(nValue);

            // Focus Coarse 값 설정
            nValue = m_dev.AfxGetSEMFocusCoarse();

            // Focus Fine 값 설정
            nValue = m_dev.AfxGetSEMFocusFine();
            m_dev.AfxSetSEMFocusFine(nValue);


            //
            // STEP #4
            //

            // USB DEVICE 장치로 부터 INTEGRATED DATA 수신
            IntegratedCommandStruct pIntegratedData = null;
            pIntegratedData = m_dev.AfxGetIntegratedPointer();
            if (pIntegratedData != null)
            {
                // INTEGATED DATA 전송 : USB DEVICE INTEGATED DATA -> Owner Window ( MAIN DLG )
                if (m_pOwnerWindow != null)
                {
                    m_pOwnerWindow.OnSemControlIntegratedData(IntegratedCommandStruct.NativeSize, pIntegratedData);
                }
            }

            // Vaccum State Read 명령어 전송 (진공상태 확인)
            m_dev.AfxScanBridgeGetVaccumState();

            // 프로그램 시작시 STAGE 설정값 불러오기 타이머 실행
            if (m_pOwnerWindow != null)
            {
                m_pOwnerWindow.OnSemStageUsbControlData(1, null);
            }

        }

        // ---- ModularAssistDevice.cpp:1942 ----
        public void InitSEMDeviceDefaultControlValue()
        {
            //
            // 2021.09.03 : 프로그램 종료 또는 기타 상황에서
            // 스캔신호 및 HV 관련사항을 모두 OFF 또는 DISABLED 시킨다.
            //

            // GUN VALVE CLOSE : 프로그램 시작시 GUN VALVE를 CLOSE 시킨다
            m_dev.AfxScanBridgeGunValveClose();

            // SPELLMAN HV - GUN HV      : 프로그램 시작시 GUN HV      를 DISABLED 시킨다
            m_dev.AfxScanBridgeSpellmanGunHVOff();
            // SPELLMAN HV - PMT         : 프로그램 시작시 PMT         를 DISABLED 시킨다
            m_dev.AfxIntegratedSpellmanPMTHVOff();
            // SPELLMAN HV - SCINTILATOR : 프로그램 시작시 SCINTILATOR 를 DISABLED 시킨다
            m_dev.AfxIntegratedSpellmanScintilatorHVOff();
            // SPELLMAN HV - COLLECTOR   : 프로그램 시작시 COLLECTOR   를 DISABLED 시킨다
            m_dev.AfxScanBridgeCollectorOff();

            // SE SCAN IMAGE CAPTURE OFF
            m_dev.AfxIntegratedScanImageDataCaptureStop();

            // SCAN 신호 초기화 ( 신호 OFF )
            m_dev.AfxIntegratedResetScanGeneration();

        }

        // ---- ModularAssistDevice.cpp:1970 ----
        public void SetStageAxisMove(int nAxis, int nPM, int nEcho, int nExecute, double dMove)
        {
            // CNC Command : "MH" Axis +/- Move
            string strData = "";
            CharBuffer szData = new CharBuffer(255 + 1);
            ZeroMem(szData);

            // X-Axis +/- Move
            if (nAxis == IDX_X_AXIS)
            {
                sprintf(szData, "%.6f", fabs(dMove));
                if (nExecute == 1) m_dev.AfxScanBridgeMMCommand(nAxis, nPM, ToStr(szData), nEcho);
                strData = ToStr(szData);
            }
            // Y-Axis +/- Move
            else if (nAxis == IDX_Y_AXIS)
            {
                sprintf(szData, "%.6f", fabs(dMove));
                if (nExecute == 1) m_dev.AfxScanBridgeMMCommand(nAxis, nPM, ToStr(szData), nEcho);
                strData = ToStr(szData);
            }
            // Z-Axis +/- Move
            else if (nAxis == IDX_Z_AXIS)
            {
                sprintf(szData, "%.6f", fabs(dMove));
                if (nExecute == 1) m_dev.AfxScanBridgeMMCommand(nAxis, nPM, ToStr(szData), nEcho);
                strData = ToStr(szData);
            }
            // R-Axis +/- Move
            else if (nAxis == IDX_R_AXIS)
            {
                sprintf(szData, "%.6f", fabs(dMove));
                if (nExecute == 1) m_dev.AfxScanBridgeMMCommand(nAxis, nPM, ToStr(szData), nEcho);
                strData = ToStr(szData);
            }
            // T-Axis +/- Move
            else if (nAxis == IDX_T_AXIS)
            {
                sprintf(szData, "%.6f", fabs(dMove));
                if (nExecute == 1) m_dev.AfxScanBridgeMMCommand(nAxis, nPM, ToStr(szData), nEcho);
                strData = ToStr(szData);
            }
            // BSE-Axis +/- Move
            else if (nAxis == IDX_B_AXIS)
            {
                // BSE AXIS MOVE
                int nBseAxis = IDX_B_AXIS;
                sprintf(szData, "%.6f", fabs(dMove));
                if (nExecute == 1) m_dev.AfxScanBridgeMMCommand(nBseAxis, nPM, ToStr(szData), nEcho);
                strData = ToStr(szData);
            }
        }

        // ---- ModularAssistDevice.cpp:2024 ----
        public void SetUsbDeviceStageXYCenterMove(int nExecute = 0)
        {
            // X-Y CENTER MOVE
            if (nExecute == 1) m_dev.AfxScanBridgeXYAxisCenterMoveCommand();
        }

        // ---- ModularAssistDevice.cpp:2031 ----
        public void SetUsbDeviceSemControlSpellmanHVOnOff(int nGunFilBias, int nPMT, int nSCIN, int nCOL)
        {
            // SET USB DEVICE : SPELLMAN HV (PMT,SCIN,COLLECTOR) ON/OFF 실행
            // USB DEVICE 적용

            // GUN/FILAMENT/BIAS
            if (nGunFilBias == SPELLMAN_GUN_FIL_BIAS_ENABLED)
            {
                m_dev.AfxScanBridgeSpellmanGunHVOn();          // GUN/FIL/BIAS HV ENABLED
            }
            else
            {
                m_dev.AfxScanBridgeSpellmanGunHVOff();         // GUN/FIL/BIAS HV DISABLED
            }
            //AfxSystemWaitTime(10);

            // PMT
            if (nPMT == SPELLMAN_PMT_ENABLED)
            {
                m_dev.AfxIntegratedSpellmanPMTHVOn();          // PMT  HV ENABLED
            }
            else
            {
                m_dev.AfxIntegratedSpellmanPMTHVOff();         // PMT  HV DISABLED
            }
            //AfxSystemWaitTime(10);

            // SCINTILATOR
            if (nSCIN == SPELLMAN_SCINTILATOR_ENABLED)
            {
                m_dev.AfxIntegratedSpellmanScintilatorHVOn();  // SCIN HV ENABLED
            }
            else
            {
                m_dev.AfxIntegratedSpellmanScintilatorHVOff(); // SCIN HV DISABLED
            }
            //AfxSystemWaitTime(10);

            // COLLECTOR
            if (nCOL == SPELLMAN_COLLECTOR_ENABLED)
            {
                m_dev.AfxScanBridgeCollectorOn();              // COLLECTOR HV ENABLED
            }
            else
            {
                m_dev.AfxScanBridgeCollectorOff();             // COLLECTOR HV DISABLED
            }
            //AfxSystemWaitTime(10);
        }

    }
}
