// 원본 NanoImpact2020cfeDlg.cpp:20618-21566 을 NanoImpact.CodeGen translate로 옮긴 뒤 손으로 고친 코드.
#pragma warning disable IDE1006, IDE0059, CS0162, CS0168, CS0219
using System;
using NanoImpact.Core.Native;
using static NanoImpact.Core.Def;
using static NanoImpact.Core.Native.CRuntime;

namespace NanoImpact.Hardware.Device
{
    public sealed partial class MainDeviceService
    {
        // ---- NanoImpact2020cfeDlg.cpp:20618 ----
        // CLIENT --> DEVICE : SET USB DEVICE DATA ( HW SETUP : SCAN AVERAGE )
        public int OnSetUsbDeviceHWSetupScanAverage(int wParam, object lParam)
        {
            // 2020.05.01
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : HW SETUP XY-AMP,OFFSET
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_SCAN_AVERAGE)
            {
                // RED SCAN AVERAGE
                m_IntegratedData.nWAverageNum[RED_SCAN_MODE]   = RequestSetUsbDeviceData.nRequestData[0];
                // FAST SCAN AVERAGE
                m_IntegratedData.nWAverageNum[FAST_SCAN_MODE]  = RequestSetUsbDeviceData.nRequestData[1];
                // SLOW1 SCAN AVERAGE
                m_IntegratedData.nWAverageNum[SLOW1_SCAN_MODE] = RequestSetUsbDeviceData.nRequestData[2];
                // SLOW2 SCAN AVERAGE
                m_IntegratedData.nWAverageNum[SLOW2_SCAN_MODE] = RequestSetUsbDeviceData.nRequestData[3];

                // SCAN MODE CHECK?
                if (m_SemSEScanControlPublicData.m_nSemSEScanModeValue == RED_SCAN_MODE)
                {
                    m_SemSEScanControlPublicData.m_nSEAverageNumber = m_IntegratedData.nWAverageNum[RED_SCAN_MODE];
                    // SCAN CLIBRATION 재설정
                    m_wndScanCalibrationToolDlg.SetSEScanAverageValue(RED_SCAN_MODE, m_IntegratedData.nWAverageNum[RED_SCAN_MODE]);
                }
                else if (m_SemSEScanControlPublicData.m_nSemSEScanModeValue == FAST_SCAN_MODE)
                {
                    m_SemSEScanControlPublicData.m_nSEAverageNumber = m_IntegratedData.nWAverageNum[FAST_SCAN_MODE];
                    // SCAN CLIBRATION 재설정
                    m_wndScanCalibrationToolDlg.SetSEScanAverageValue(FAST_SCAN_MODE, m_IntegratedData.nWAverageNum[FAST_SCAN_MODE]);
                }
                else if (m_SemSEScanControlPublicData.m_nSemSEScanModeValue == SLOW1_SCAN_MODE)
                {
                    m_SemSEScanControlPublicData.m_nSEAverageNumber = m_IntegratedData.nWAverageNum[SLOW1_SCAN_MODE];
                    // SCAN CLIBRATION 재설정
                    m_wndScanCalibrationToolDlg.SetSEScanAverageValue(SLOW1_SCAN_MODE, m_IntegratedData.nWAverageNum[SLOW1_SCAN_MODE]);
                }
                else if (m_SemSEScanControlPublicData.m_nSemSEScanModeValue == SLOW2_SCAN_MODE)
                {
                    m_SemSEScanControlPublicData.m_nSEAverageNumber = m_IntegratedData.nWAverageNum[SLOW2_SCAN_MODE];
                    // SCAN CLIBRATION 재설정
                    m_wndScanCalibrationToolDlg.SetSEScanAverageValue(SLOW2_SCAN_MODE, m_IntegratedData.nWAverageNum[SLOW2_SCAN_MODE]);
                }

                // SCAN AVERAGE
                m_pAssistDeviceTool.SetUsbDeviceHWSetupScanAverageOptionValue(RequestSetUsbDeviceData);

            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_SCAN_RED_AVERAGE)
            {
                // RED SCAN AVERAGE
                m_IntegratedData.nWAverageNum[RED_SCAN_MODE] = RequestSetUsbDeviceData.nRequestData[0];

                // SCAN MODE CHECK?
                if (m_SemSEScanControlPublicData.m_nSemSEScanModeValue == RED_SCAN_MODE)
                {
                    m_SemSEScanControlPublicData.m_nSEAverageNumber = m_IntegratedData.nWAverageNum[RED_SCAN_MODE];
                }
                // SCAN CLIBRATION 재설정
                m_wndScanCalibrationToolDlg.SetSEScanAverageValue(RED_SCAN_MODE, m_IntegratedData.nWAverageNum[RED_SCAN_MODE]);

                // SCAN AVERAGE
                m_pAssistDeviceTool.SetUsbDeviceHWSetupScanAverageOptionValue(RequestSetUsbDeviceData);
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_HWSETUP_SCAN_FAST_AVERAGE)
            {
                // FAST SCAN AVERAGE
                m_IntegratedData.nWAverageNum[FAST_SCAN_MODE] = RequestSetUsbDeviceData.nRequestData[0];

                // SCAN MODE CHECK?
                if (m_SemSEScanControlPublicData.m_nSemSEScanModeValue == FAST_SCAN_MODE)
                {
                    m_SemSEScanControlPublicData.m_nSEAverageNumber = m_IntegratedData.nWAverageNum[FAST_SCAN_MODE];
                }
                // SCAN CLIBRATION 재설정
                m_wndScanCalibrationToolDlg.SetSEScanAverageValue(FAST_SCAN_MODE, m_IntegratedData.nWAverageNum[FAST_SCAN_MODE]);

                // SCAN AVERAGE
                m_pAssistDeviceTool.SetUsbDeviceHWSetupScanAverageOptionValue(RequestSetUsbDeviceData);
            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:20708 ----
        // CLIENT --> DEVICE : SET USB DEVICE DATA ( HW SETUP : OPTION DATA )
        public int OnSetUsbDeviceHWSetupOptionData(int wParam, object lParam)
        {
            // 2020.05.01
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : HW SETUP XY-AMP,OFFSET
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // PHOTO NONE STOP --> 2020.05.26 PHOTO IMAGE FILTERING
            if (RequestSetUsbDeviceData.nRequestData[0] >= 0) m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_PHOTO_IMAGE_FILTER]     = RequestSetUsbDeviceData.nRequestData[0];
            // IMAGE SHIFT X REVERSE
            if (RequestSetUsbDeviceData.nRequestData[1] >= 0) m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_IMAGE_SHIFT_X_REVERSE]  = RequestSetUsbDeviceData.nRequestData[1];  // IMAGE EFFECT  OPTION 으로 사용
            // IMAGE SHIFT Y REVERSE
            if (RequestSetUsbDeviceData.nRequestData[2] >= 0) m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_IMAGE_SHIFT_Y_REVERSE]  = RequestSetUsbDeviceData.nRequestData[2];  // x35k 배율보정 OPTION 으로 사용
            // ORTHOGONAL CORRECTION
            if (RequestSetUsbDeviceData.nRequestData[3] >= 0) m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_ORTHOGONAL_CORRRECTION] = RequestSetUsbDeviceData.nRequestData[3];
            // STAGE HORZ ALIGN
            if (RequestSetUsbDeviceData.nRequestData[4] >= 0) m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_STAGE_HORZ_ALIGN]       = RequestSetUsbDeviceData.nRequestData[4];
            // Z AXIS FOCUS LINK
            if (RequestSetUsbDeviceData.nRequestData[5] >= 0) m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_Z_AXIS_FOCUS_LINK]      = RequestSetUsbDeviceData.nRequestData[5];
            // WD SCAN ROTATION CORRECTION
            if (RequestSetUsbDeviceData.nRequestData[6] >= 0) m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_WD_SCAN_ROTATION]       = RequestSetUsbDeviceData.nRequestData[6];
            // WD SCALE ZOOM CORRECTION
            if (RequestSetUsbDeviceData.nRequestData[7] >= 0) m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_WD_SCALE_ZOOM]          = RequestSetUsbDeviceData.nRequestData[7];
            // HV CORRECTION
            if (RequestSetUsbDeviceData.nRequestData[8] >= 0) m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_HV_CORRECTION]          = RequestSetUsbDeviceData.nRequestData[8];
            // 2021.08.23 : HV PROCESSING 완료 후, FOCUS DEGAUSSING
            if (RequestSetUsbDeviceData.nRequestData[9] >= 0) m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_HV_DEGAUSSING]          = RequestSetUsbDeviceData.nRequestData[9];
            // 2021.08.23 : 사용중단
            // 2021.07.29 : SCAN CLOCK 48 BITS
            //if (RequestSetUsbDeviceData.nRequestData[9] >= 0) m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_SCAN_CLOCK_48]        = RequestSetUsbDeviceData.nRequestData[9];
            m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_SCAN_CLOCK_48] = 0;

            // SCAN HW SETUP : SCAN OPTION DATA
            m_pAssistDeviceTool.SetUsbDeviceHWSetupScanCheckOptionValue(RequestSetUsbDeviceData);

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:20752 ----
        // CLIENT --> DEVICE : SET USB DEVICE DATA ( HW SETUP : MONITOR INFORMATION )
        public int OnSetUsbDeviceHWSetupMonitorInformation(int wParam, object lParam)
        {
            // 2020.05.01
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : HW SETUP XY-AMP,OFFSET
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // MONITOR SIZE (inch)
            m_IntegratedData.dScanOptionData[IDX_HWSETUP_OPTION_DOUBLE_MONITOR_SIZE]  = RequestSetUsbDeviceData.dRequestData[0];
            // MONITOR SIZE RATIO(%)
            m_IntegratedData.dScanOptionData[IDX_HWSETUP_OPTION_DOUBLE_MONITOR_RATIO] = RequestSetUsbDeviceData.dRequestData[1];

            // SCAN HW SETUP : MONITOR INFORMATION DATA
            m_pAssistDeviceTool.SetUsbDeviceHWSetupMonitorInformationOptionValue(RequestSetUsbDeviceData);

            // Redraw 1st Information Data
            Update1stImageInfoBitmap(m_SemSEScanControlPublicData.m_n1stImageInfoDrawX, m_SemSEScanControlPublicData.m_n1stImageInfoDrawY, m_SemSEScanControlPublicData.m_n1stImageInfoWidth, m_SemSEScanControlPublicData.m_n1stImageInfoHeight);

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:20779 ----
        // CLIENT --> DEVICE : SET USB DEVICE DATA ( HW SETUP : AUTO FOCUS CONTROL VALUE )
        public int OnSetUsbDeviceHWSetupAutoFocusControlValue(int wParam, object lParam)
        {
            // 2020.05.01
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : HW SETUP XY-AMP,OFFSET
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // AUTO FOCUS CONTROL VALUE
            // SCAN MODE
            m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_SCANMODE]    = RequestSetUsbDeviceData.nRequestData[0];
            // UP SCAN NUMBER
            m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_UP_NUMBER]   = RequestSetUsbDeviceData.nRequestData[1];
            // DOWN SCAN NUMBER
            m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_DOWN_NUMBER] = RequestSetUsbDeviceData.nRequestData[2];
            // SCAN STEP VALUE
            m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_STEP_VALUE]  = RequestSetUsbDeviceData.nRequestData[3];
            // FOCUS CHANGE Y-POS VALUE
            m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_YPOS_VALUE]  = RequestSetUsbDeviceData.nRequestData[4];

            // AUTO FOCUS OPTIN VALUE 설정
            m_pAssistDeviceTool.SetUsbDeviceAutoFocusOptionValue(RequestSetUsbDeviceData);

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:20810 ----
        // CLIENT --> DEVICE : SET USB DEVICE DATA ( HW SETUP : AUTO CONTRAST & BRIGHTNESS CONTROL VALUE )
        public int OnSetUsbDeviceHWSetupAutoContrastBrightValue(int wParam, object lParam)
        {
            // 2020.05.01
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : HW SETUP XY-AMP,OFFSET
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // AUTO CONTRAST & BRIGHTNESS CONTROL VALUE
            // CONTRAST VALUE
            m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_CONTRAST_VALUE]   = RequestSetUsbDeviceData.nRequestData[0];
            // CONTRAST 100 JUMP STEP
            m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_CONTRAST_100JUMP] = RequestSetUsbDeviceData.nRequestData[1];
            // CONTRAST 10 JUMP STEP
            m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_CONTRAST_10JUMP]  = RequestSetUsbDeviceData.nRequestData[2];
            // BRIGHTNESS VALUE
            m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_BRIGHT_VALUE]     = RequestSetUsbDeviceData.nRequestData[3];
            // BRIGHTNESS 100 JUMP STEP
            m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_BRIGHT_100JUMP]   = RequestSetUsbDeviceData.nRequestData[4];
            // BRIGHTNESS 10 JUMP STEP
            m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_BRIGHT_10JUMP]    = RequestSetUsbDeviceData.nRequestData[5];

            // AUTO CONTRAST & BRIGHTNESS OPTIN VALUE 설정
            m_pAssistDeviceTool.SetUsbDeviceAutoContrastBrightnessOptionValue(RequestSetUsbDeviceData);

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:20843 ----
        // CLIENT --> DEVICE : SET USB DEVICE DATA ( HW SETUP : AUTO START CONFIG VALUE )
        public int OnSetUsbDeviceHWSetupAutoStartConfigValue(int wParam, object lParam)
        {
            // 2020.05.01
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : HW SETUP XY-AMP,OFFSET
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // AUTO START CONFIG VALUE
            // FILAMENT VALUE
            m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_FILAMENT_VALUE]   = RequestSetUsbDeviceData.nRequestData[0];
            // BIAS VALUE
            m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_BIAS_VALUE]       = RequestSetUsbDeviceData.nRequestData[1];
            // CONTRAST VALUE
            m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_CONTRAST_VALUE]   = RequestSetUsbDeviceData.nRequestData[2];
            // BRIGHT VALUE
            m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_BRIGHT_VALUE]     = RequestSetUsbDeviceData.nRequestData[3];
            // FILAMENT/BIAS 변화폭 적용율(%)
            m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_FIL_BIAS_RATIO]   = RequestSetUsbDeviceData.nRequestData[4];
            // FILAMENT/BIAS STEP INTERVAL TIME[ms]
            m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_STEP_TIME_VALUE]  = RequestSetUsbDeviceData.nRequestData[5];
            // EMISSION CURRENT VALUE [ua]
            m_IntegratedData.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_START_EM_CURRENT_VALUE] = RequestSetUsbDeviceData.nRequestData[6];

            // AUTO START OPTIN VALUE 설정
            m_pAssistDeviceTool.SetUsbDeviceAutoStartOptionValue(RequestSetUsbDeviceData);

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:20878 ----
        // CLIENT --> DEVICE : SET USB DEVICE DATA ( HW SETUP : SEM DEVICE MODEL INFORMATION )
        public int OnSetUsbDeviceHWSetupSemDeviceModel(int wParam, object lParam)
        {
            // 2020.05.01
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : HW SETUP XY-AMP,OFFSET
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // SEM DEVICE MODEL INFORMATION
            ZeroMem(m_IntegratedData.szScanOption32Text1);
            int nLength = strlen(RequestSetUsbDeviceData.szData);
            memcpy(m_IntegratedData.szScanOption32Text1, RequestSetUsbDeviceData.szData, nLength);

            // SEM DEVICE MODEL INFORMATION 설정
            m_pAssistDeviceTool.SetUsbDeviceSemDeviceModelOptionValue(RequestSetUsbDeviceData);

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:20902 ----
        // CLIENT --> DEVICE : SET USB DEVICE DATA ( HW SETUP : SEM DEVICE COMPANY INFORMATION )
        public int OnSetUsbDeviceHWSetupSemDeviceCompany(int wParam, object lParam)
        {
            // 2020.05.01
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : HW SETUP XY-AMP,OFFSET
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // SEM DEVICE COMPANY INFORMATION
            ZeroMem(m_IntegratedData.szScanOption32Text2);
            int nLength = strlen(RequestSetUsbDeviceData.szData);
            memcpy(m_IntegratedData.szScanOption32Text2, RequestSetUsbDeviceData.szData, nLength);

            // SEM DEVICE COMPANY INFORMATION 설정
            m_pAssistDeviceTool.SetUsbDeviceSemDeviceCompanyOptionValue(RequestSetUsbDeviceData);

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:20926 ----
        // CLIENT PUBLIC DATA 정보 ( OPTION : USER NAME INFORMATION )
        public int OnSetUsbDeviceOptionUserName(int wParam, object lParam)
        {
            // After displaying the dialog screen & Process function
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : OPTION : USER NAME
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // CLIENT PUBLIC DATA 정보 : USER NAME INFORMATION
            ZeroMem(m_ClientPublicData.szUserName);
            int nLength = strlen(RequestSetUsbDeviceData.szData);
            memcpy(m_ClientPublicData.szUserName, RequestSetUsbDeviceData.szData, nLength);

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:20947 ----
        // CLIENT PUBLIC DATA 정보 ( OPTION : USER PASSWORD INFORMATION )
        public int OnSetUsbDeviceOptionUserPassword(int wParam, object lParam)
        {
            // After displaying the dialog screen & Process function
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : OPTION : USER PASSWORD
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // CLIENT PUBLIC DATA 정보 : USER PASSWORD INFORMATION
            ZeroMem(m_ClientPublicData.szPassword);
            int nLength = strlen(RequestSetUsbDeviceData.szData);
            memcpy(m_ClientPublicData.szPassword, RequestSetUsbDeviceData.szData, nLength);

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:20968 ----
        // CLIENT PUBLIC DATA 정보 ( OPTION : USER COMPANY INFORMATION )
        public int OnSetUsbDeviceOptionUserCompany(int wParam, object lParam)
        {
            // After displaying the dialog screen & Process function
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : OPTION : USER COMPANY
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // CLIENT PUBLIC DATA 정보 : USER COMPANY INFORMATION
            ZeroMem(m_ClientPublicData.szCompanyName);
            int nLength = strlen(RequestSetUsbDeviceData.szData);
            memcpy(m_ClientPublicData.szCompanyName, RequestSetUsbDeviceData.szData, nLength);

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:20989 ----
        // CLIENT PUBLIC DATA 정보 ( OPTION : SERVER IPADDRESS INFORMATION )
        public int OnSetUsbDeviceOptionServerIPAddress(int wParam, object lParam)
        {
            // After displaying the dialog screen & Process function
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : OPTION : SERVER IPADDRESS
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // CLIENT PUBLIC DATA 정보 : SERVER IPADDRESS INFORMATION
            ZeroMem(m_ClientPublicData.szServerIPAddress);
            int nLength = strlen(RequestSetUsbDeviceData.szData);
            memcpy(m_ClientPublicData.szServerIPAddress, RequestSetUsbDeviceData.szData, nLength);

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:21010 ----
        // CLIENT PUBLIC DATA 정보 ( OPTION : SERVER PORT INFORMATION )
        public int OnSetUsbDeviceOptionServerPort(int wParam, object lParam)
        {
            // After displaying the dialog screen & Process function
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : OPTION : SERVER PORT
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // CLIENT PUBLIC DATA 정보 : SERVER PORT INFORMATION
            ZeroMem(m_ClientPublicData.szServerPort);
            int nLength = strlen(RequestSetUsbDeviceData.szData);
            memcpy(m_ClientPublicData.szServerPort, RequestSetUsbDeviceData.szData, nLength);

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:21031 ----
        // CLIENT PUBLIC DATA 정보 ( OPTION : SERVER CONNECT CODE INFORMATION )
        public int OnSetUsbDeviceOptionServerConnectCode(int wParam, object lParam)
        {
            // After displaying the dialog screen & Process function
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : OPTION : SERVER CONNECT CODE
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // CLIENT PUBLIC DATA 정보 : SERVER CONNECT CODE INFORMATION
            ZeroMem(m_ClientPublicData.szServerCode);
            int nLength = strlen(RequestSetUsbDeviceData.szData);
            memcpy(m_ClientPublicData.szServerCode, RequestSetUsbDeviceData.szData, nLength);

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:21052 ----
        // CLIENT PUBLIC DATA 정보 ( OPTION : SEM SYSTEM COMPACT-FE MODE )
        public int OnSetUsbDeviceOptionSemSystemCompactMode(int wParam, object lParam)
        {
            // After displaying the dialog screen & Process function
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : OPTION -> SEM SYSTEM MODE
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // CLIENT PUBLIC DATA 정보 : OPTION -> SEM SYSTEM COMPACT-FE, COMPACT-W MODE
            // 0->COMPACT-W MODE, 1->COMPACT-FE MODE
            m_ClientPublicData.nOptionData[IDX_CLIENT_OPTION_INT_SEM_SYSTEM_MODE] = RequestSetUsbDeviceData.nRequestData[0];

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:21072 ----
        // CLIENT PUBLIC DATA 정보 ( OPTION : PENNING VACUUM GAUGE MODE )
        public int OnSetUsbDeviceOptionPenningVacuumGaugeMode(int wParam, object lParam)
        {
            // After displaying the dialog screen & Process function
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : OPTION -> PENNING VACUUM GAUGE
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // CLIENT PUBLIC DATA 정보 : OPTION -> PENNING VACUUM GAUGE
            m_ClientPublicData.bOptionFlag[IDX_CLIENT_OPTION_BOOL_PENNING_GAUGE_SEL] = RequestSetUsbDeviceData.nRequestData[0];

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:21091 ----
        // CLIENT PUBLIC DATA 정보 ( OPTION : VACUUM WATTING MODE )
        public int OnSetUsbDeviceOptionVacuumWattingMode(int wParam, object lParam)
        {
            // After displaying the dialog screen & Process function
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : OPTION -> VACUUM WATTING MODE
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // CLIENT PUBLIC DATA 정보 : OPTION -> VACUUM WATTING MODE
            m_ClientPublicData.bOptionFlag[IDX_CLIENT_OPTION_BOOL_VACUUM_WAITTING_SEL] = RequestSetUsbDeviceData.nRequestData[0];

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:21110 ----
        // CLIENT PUBLIC DATA 정보 ( OPTION : SEM CFE COM PORT )
        public int OnSetUsbDeviceOptionSemSystemCFEComPort(int wParam, object lParam)
        {
            // After displaying the dialog screen & Process function
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : OPTION -> SEM CFE COM PORT
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // CLIENT PUBLIC DATA 정보 : OPTION -> SEM SYSTEM COMPACT-FE COM PORT
            m_ClientPublicData.nOptionData[IDX_CLIENT_OPTION_INT_SEM_CFE_COM_PORT] = RequestSetUsbDeviceData.nRequestData[0];

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:21129 ----
        // CLIENT PUBLIC DATA 정보 ( OPTION : SEM SYSTEM DATA )
        public int OnSetUsbDeviceOptionSemSystemSystemData(int wParam, object lParam)
        {
            // After displaying the dialog screen & Process function
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : OPTION -> SEM SYSTEM MODE
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // CLIENT PUBLIC DATA 정보 : OPTION -> SEM SYSTEM DATA
            // 길이표현 소숫점 자리수
            m_ClientPublicData.nOptionData[IDX_CLIENT_OPTION_INT_SEM_DECIMAL_PLACES] = RequestSetUsbDeviceData.nRequestData[0];

            m_strDistDecimalPlaces = "%6.2f";

            if (m_ClientPublicData.nOptionData[IDX_CLIENT_OPTION_INT_SEM_DECIMAL_PLACES] == 0)
            {
                m_strDistDecimalPlaces = "%6.f";
            }
            else if (m_ClientPublicData.nOptionData[IDX_CLIENT_OPTION_INT_SEM_DECIMAL_PLACES] == 1)
            {
                m_strDistDecimalPlaces = "%6.1f";
            }
            else if (m_ClientPublicData.nOptionData[IDX_CLIENT_OPTION_INT_SEM_DECIMAL_PLACES] == 2)
            {
                m_strDistDecimalPlaces = "%6.2f";
            }
            else if (m_ClientPublicData.nOptionData[IDX_CLIENT_OPTION_INT_SEM_DECIMAL_PLACES] == 3)
            {
                m_strDistDecimalPlaces = "%6.3f";
            }

            // MAG HV RATIO
            //m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_HV_RATIO] = RequestSetUsbDeviceData.dRequestData[0];
            // MAG HV RATIO 적용
            //AfxIntegratedSetMagHVRatio(m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_HV_RATIO]);

            // MAG x35k ZOOM RATIO
            m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_x35k_ZOOM_RATIO] = RequestSetUsbDeviceData.dRequestData[0];
            // MAG x35k ZOOM RATIO
            m_dev.AfxIntegratedSetMagx35kZoomRatio(m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_x35k_ZOOM_RATIO]);

            // PHOTO FILTER#1 MAGNIFICATION 값
            m_ClientPublicData.nOptionData[IDX_CLIENT_OPTION_INT_PHOTO_FILTER1_MAG_VALUE] = RequestSetUsbDeviceData.nRequestData[1];

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:21181 ----
        // CLIENT PUBLIC DATA 정보 ( OPTION : SEM MAG HV RATIO )
        public int OnSetUsbDeviceOptionSemMagHVRatioData(int wParam, object lParam)
        {
            // After displaying the dialog screen & Process function
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : OPTION -> MAG HV RATIO
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // MAG HV RATIO 30[kV] ~ 0.5[kV]
            if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_OPTION_MAG_HV_RATIO_1_DATA)
            {
                m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_HV300_RATIO] = RequestSetUsbDeviceData.dRequestData[0];    // MAG HV RATIO  30[kV]
                m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_HV250_RATIO] = RequestSetUsbDeviceData.dRequestData[1];    // MAG HV RATIO  25[kV]
                m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_HV200_RATIO] = RequestSetUsbDeviceData.dRequestData[2];    // MAG HV RATIO  20[kV]
                m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_HV150_RATIO] = RequestSetUsbDeviceData.dRequestData[3];    // MAG HV RATIO  15[kV]
                m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_HV100_RATIO] = RequestSetUsbDeviceData.dRequestData[4];    // MAG HV RATIO  10[kV]
                m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_HV050_RATIO] = RequestSetUsbDeviceData.dRequestData[5];    // MAG HV RATIO   5[kV]
                m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_HV030_RATIO] = RequestSetUsbDeviceData.dRequestData[6];    // MAG HV RATIO   3[kV]
                m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_HV029_RATIO] = RequestSetUsbDeviceData.dRequestData[7];    // MAG HV RATIO 2.9[kV]
                m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_HV028_RATIO] = RequestSetUsbDeviceData.dRequestData[8];    // MAG HV RATIO 2.8[kV]
                m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_HV027_RATIO] = RequestSetUsbDeviceData.dRequestData[9];    // MAG HV RATIO 2.7[kV]
                // MAG HV RATIO 적용
                m_dev.AfxIntegratedSetMagHVRatio(m_ClientPublicData.dOptionData.ToArray());
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_OPTION_MAG_HV_RATIO_2_DATA)
            {
                m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_HV026_RATIO] = RequestSetUsbDeviceData.dRequestData[0];    // MAG HV RATIO 2.6[kV]
                m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_HV025_RATIO] = RequestSetUsbDeviceData.dRequestData[1];    // MAG HV RATIO 2.5[kV]
                m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_HV024_RATIO] = RequestSetUsbDeviceData.dRequestData[2];    // MAG HV RATIO 2.4[kV]
                m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_HV023_RATIO] = RequestSetUsbDeviceData.dRequestData[3];    // MAG HV RATIO 2.3[kV]
                m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_HV022_RATIO] = RequestSetUsbDeviceData.dRequestData[4];    // MAG HV RATIO 2.2[kV]
                m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_HV021_RATIO] = RequestSetUsbDeviceData.dRequestData[5];    // MAG HV RATIO 2.1[kV]
                m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_HV020_RATIO] = RequestSetUsbDeviceData.dRequestData[6];    // MAG HV RATIO 2.0[kV]
                m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_HV019_RATIO] = RequestSetUsbDeviceData.dRequestData[7];    // MAG HV RATIO 1.9[kV]
                m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_HV018_RATIO] = RequestSetUsbDeviceData.dRequestData[8];    // MAG HV RATIO 1.8[kV]
                m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_HV017_RATIO] = RequestSetUsbDeviceData.dRequestData[9];    // MAG HV RATIO 1.7[kV]
                // MAG HV RATIO 적용
                m_dev.AfxIntegratedSetMagHVRatio(m_ClientPublicData.dOptionData.ToArray());
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_OPTION_MAG_HV_RATIO_3_DATA)
            {
                m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_HV016_RATIO] = RequestSetUsbDeviceData.dRequestData[0];    // MAG HV RATIO 1.6[kV]
                m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_HV015_RATIO] = RequestSetUsbDeviceData.dRequestData[1];    // MAG HV RATIO 1.5[kV]
                m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_HV014_RATIO] = RequestSetUsbDeviceData.dRequestData[2];    // MAG HV RATIO 1.4[kV]
                m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_HV013_RATIO] = RequestSetUsbDeviceData.dRequestData[3];    // MAG HV RATIO 1.3[kV]
                m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_HV012_RATIO] = RequestSetUsbDeviceData.dRequestData[4];    // MAG HV RATIO 1.2[kV]
                m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_HV011_RATIO] = RequestSetUsbDeviceData.dRequestData[5];    // MAG HV RATIO 1.1[kV]
                m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_HV010_RATIO] = RequestSetUsbDeviceData.dRequestData[6];    // MAG HV RATIO 1.0[kV]
                m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_HV009_RATIO] = RequestSetUsbDeviceData.dRequestData[7];    // MAG HV RATIO 0.9[kV]
                m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_HV008_RATIO] = RequestSetUsbDeviceData.dRequestData[8];    // MAG HV RATIO 0.8[kV]
                m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_HV007_RATIO] = RequestSetUsbDeviceData.dRequestData[9];    // MAG HV RATIO 0.7[kV]
                // MAG HV RATIO 적용
                m_dev.AfxIntegratedSetMagHVRatio(m_ClientPublicData.dOptionData.ToArray());
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_OPTION_MAG_HV_RATIO_4_DATA)
            {
                m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_HV006_RATIO] = RequestSetUsbDeviceData.dRequestData[0];    // MAG HV RATIO 0.6[kV]
                m_ClientPublicData.dOptionData[IDX_CLIENT_OPTION_DOUBLE_MAG_HV005_RATIO] = RequestSetUsbDeviceData.dRequestData[1];    // MAG HV RATIO 0.5[kV]
                // MAG HV RATIO 적용
                m_dev.AfxIntegratedSetMagHVRatio(m_ClientPublicData.dOptionData.ToArray());
            }
            // 2023.03.14 : MAG RELAY IMAGE SHIFT CORRECT DATA
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_OPTION_MAG_IMAGE_SHIFT_DATA)
            {
                m_ClientPublicData.nOptionData[IDX_CLIENT_OPTION_INT_MAG_IMAGE_SHIFT_USED]   = RequestSetUsbDeviceData.nRequestData[0]; // MAG IMAGE SHIFT : MAG IMAGE SHIFT USED
                m_ClientPublicData.nOptionData[IDX_CLIENT_OPTION_INT_MAG_IMAGE_SHIFT_X20]    = RequestSetUsbDeviceData.nRequestData[1]; // MAG IMAGE SHIFT : MAG IMAGE SHIFT X, MAG x20 ~ x250
                m_ClientPublicData.nOptionData[IDX_CLIENT_OPTION_INT_MAG_IMAGE_SHIFT_Y20]    = RequestSetUsbDeviceData.nRequestData[2]; // MAG IMAGE SHIFT : MAG IMAGE SHIFT Y, MAG x20 ~ x250
                m_ClientPublicData.nOptionData[IDX_CLIENT_OPTION_INT_MAG_IMAGE_SHIFT_X300]   = RequestSetUsbDeviceData.nRequestData[3]; // MAG IMAGE SHIFT : MAG IMAGE SHIFT X, MAG x300 ~ x2500
                m_ClientPublicData.nOptionData[IDX_CLIENT_OPTION_INT_MAG_IMAGE_SHIFT_Y300]   = RequestSetUsbDeviceData.nRequestData[4]; // MAG IMAGE SHIFT : MAG IMAGE SHIFT Y, MAG x300 ~ x2500
                m_ClientPublicData.nOptionData[IDX_CLIENT_OPTION_INT_MAG_IMAGE_SHIFT_X3000]  = RequestSetUsbDeviceData.nRequestData[5]; // MAG IMAGE SHIFT : MAG IMAGE SHIFT X, MAG x3000 ~ x30000
                m_ClientPublicData.nOptionData[IDX_CLIENT_OPTION_INT_MAG_IMAGE_SHIFT_Y3000]  = RequestSetUsbDeviceData.nRequestData[6]; // MAG IMAGE SHIFT : MAG IMAGE SHIFT Y, MAG x3000 ~ x30000
                m_ClientPublicData.nOptionData[IDX_CLIENT_OPTION_INT_MAG_IMAGE_SHIFT_X35000] = RequestSetUsbDeviceData.nRequestData[7]; // MAG IMAGE SHIFT : MAG IMAGE SHIFT X, MAG x35000 ~ x300000
                m_ClientPublicData.nOptionData[IDX_CLIENT_OPTION_INT_MAG_IMAGE_SHIFT_Y35000] = RequestSetUsbDeviceData.nRequestData[8]; // MAG IMAGE SHIFT : MAG IMAGE SHIFT Y, MAG x35000 ~ x300000
            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:21264 ----
        // CLIENT --> DEVICE : SET USB DEVICE DATA ( SEM CONTROL DATA -> GUNHV, FILAMENT, BIAS, GUNALIGN, IMAGESHIFT, STIG, CONTRAST, BRIGHT, FOCUS ... )
        public int OnSetUsbDeviceSEMControlDataToDeviceServer(int wParam, object lParam)
        {
            if (lParam == null)
                return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : SEM CONTROL DATA -> GUNHV, FILAMENT, BIAS, GUNALIGN, IMAGESHIFT, STIG, CONTRAST, BRIGHT, FOCUS ...
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            //
            // VAC/STAGE/CALIBRATION STRING DATA
            //
            if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_SEM_CONTROL_VACUUM_STATE)
            {
                // SEM VACUUM STATE 설정
                m_IntegratedData.nSemVacState = RequestSetUsbDeviceData.nRequestData[0];
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_SEM_GUN_HV_INDEX)
            {
                // GUN HV INDEX
                m_IntegratedData.nGunHVIndex = RequestSetUsbDeviceData.nRequestData[0];
                // Update Infomation Text
                Update1stImageInfoBitmap(m_SemSEScanControlPublicData.m_n1stImageInfoDrawX, m_SemSEScanControlPublicData.m_n1stImageInfoDrawY, m_SemSEScanControlPublicData.m_n1stImageInfoWidth, m_SemSEScanControlPublicData.m_n1stImageInfoHeight);
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_SEM_MAGNIFICATION_INDEX)
            {
                // MAGNIFICATION INDEX
                m_IntegratedData.nMagStepIndex = RequestSetUsbDeviceData.nRequestData[0];
                // Update Infomation Text
                Update1stImageInfoBitmap(m_SemSEScanControlPublicData.m_n1stImageInfoDrawX, m_SemSEScanControlPublicData.m_n1stImageInfoDrawY, m_SemSEScanControlPublicData.m_n1stImageInfoWidth, m_SemSEScanControlPublicData.m_n1stImageInfoHeight);
                // Update Infomation Text
                Update2ndImageInfoBitmap(m_SemSEScanControlPublicData.m_n2ndImageInfoDrawX, m_SemSEScanControlPublicData.m_n2ndImageInfoDrawY, m_SemSEScanControlPublicData.m_n2ndImageInfoWidth, m_SemSEScanControlPublicData.m_n2ndImageInfoHeight);

                // 2020.01.21
                // 배율에 따른 X-Y AXIS 이동속도 자동조절
                KillTimer(TIMER_ID_MAG_XY_AXIS_ACCEL_CHANGE);
                SetTimer(TIMER_ID_MAG_XY_AXIS_ACCEL_CHANGE, 500, NULL);
            }

            //
            // VAC/STAGE/CALIBRATION STRING DATA ==> DETAIL CONTROL TO DATA
            //
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_SEM_GUN_ALIGN_X_VALUE)
            {
                // GUN ALIGN X
                m_IntegratedData.nGAXValue = RequestSetUsbDeviceData.nRequestData[0];
                // GUN ALIGN CONTROL-->DETAIL CONTROL : GUN ALIGN X POSITION 값
                m_wndDetailControlDlg.GunAlignPositionToDetailControl(IDX_X_AXIS, ToStr(RequestSetUsbDeviceData.szData));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_SEM_GUN_ALIGN_Y_VALUE)
            {
                // GUN ALIGN Y
                m_IntegratedData.nGAYValue = RequestSetUsbDeviceData.nRequestData[0];
                // GUN ALIGN CONTROL-->DETAIL CONTROL : GUN ALIGN Y POSITION 값
                m_wndDetailControlDlg.GunAlignPositionToDetailControl(IDX_Y_AXIS, ToStr(RequestSetUsbDeviceData.szData));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_SEM_BEAM_ALIGN_X_VALUE)
            {
                // BEAM ALIGN X
                m_IntegratedData.nBAXValue = RequestSetUsbDeviceData.nRequestData[0];
                // BEAM ALIGN CONTROL-->DETAIL CONTROL : BEAM ALIGN X POSITION 값
                m_wndDetailControlDlg.BeamAlignPositionToDetailControl(IDX_X_AXIS, ToStr(RequestSetUsbDeviceData.szData));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_SEM_BEAM_ALIGN_Y_VALUE)
            {
                // BEAM ALIGN Y
                m_IntegratedData.nBAYValue = RequestSetUsbDeviceData.nRequestData[0];
                // BEAM ALIGN CONTROL-->DETAIL CONTROL : BEAM ALIGN Y POSITION 값
                m_wndDetailControlDlg.BeamAlignPositionToDetailControl(IDX_Y_AXIS, ToStr(RequestSetUsbDeviceData.szData));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_SEM_IMAGE_SHIFT_X_VALUE)
            {
                // IMAGE SHIFT X
                m_IntegratedData.nISXValue = RequestSetUsbDeviceData.nRequestData[0];
                // IMAGE SHIFT CONTROL-->DETAIL CONTROL : IMAGE SHIFT X POSITION 값
                m_wndDetailControlDlg.ImageShiftPositionToDetailControl(IDX_X_AXIS, ToStr(RequestSetUsbDeviceData.szData));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_SEM_IMAGE_SHIFT_Y_VALUE)
            {
                // IMAGE SHIFT Y
                m_IntegratedData.nISYValue = RequestSetUsbDeviceData.nRequestData[0];
                // IMAGE SHIFT CONTROL-->DETAIL CONTROL : IMAGE SHIFT Y POSITION 값
                m_wndDetailControlDlg.ImageShiftPositionToDetailControl(IDX_Y_AXIS, ToStr(RequestSetUsbDeviceData.szData));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_SEM_STIG_X_VALUE)
            {
                // STIG X
                m_IntegratedData.nStigXValue = RequestSetUsbDeviceData.nRequestData[0];
                // STIG CONTROL-->DETAIL CONTROL : STIG X POSITION 값
                m_wndDetailControlDlg.StigPositionToDetailControl(IDX_X_AXIS, ToStr(RequestSetUsbDeviceData.szData));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_SEM_STIG_Y_VALUE)
            {
                // STIG Y
                m_IntegratedData.nStigYValue = RequestSetUsbDeviceData.nRequestData[0];
                // STIG CONTROL-->DETAIL CONTROL : STIG Y POSITION 값
                m_wndDetailControlDlg.StigPositionToDetailControl(IDX_Y_AXIS, ToStr(RequestSetUsbDeviceData.szData));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_SEM_CONTRAST_VALUE)
            {
                // CONTRAST
                m_IntegratedData.nContrastValue = RequestSetUsbDeviceData.nRequestData[0];
                // CONTRAST CONTROL-->DETAIL CONTROL : CONTRAST POSITION 값
                m_wndDetailControlDlg.ContrastPositionToDetailControl(ToStr(RequestSetUsbDeviceData.szData));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_SEM_BRIGHT_VALUE)
            {
                // BRIGHTNESS
                m_IntegratedData.nBrightValue = RequestSetUsbDeviceData.nRequestData[0];
                // BRIGHTNESS CONTROL-->DETAIL CONTROL : BRIGHTNESS POSITION 값
                m_wndDetailControlDlg.BrightnessPositionToDetailControl(ToStr(RequestSetUsbDeviceData.szData));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_SEM_FOCUS_COARSE_FINE_VALUE)
            {
                // FOCUS : COARSE/FINE
                m_IntegratedData.nFocusCoarse = RequestSetUsbDeviceData.nRequestData[0];
                m_IntegratedData.nFocusFine   = RequestSetUsbDeviceData.nRequestData[1];
                // BRIGHTNESS CONTROL-->DETAIL CONTROL : BRIGHTNESS POSITION 값
                m_wndDetailControlDlg.FocusCoarseFinePositionToDetailControl(ToStr(RequestSetUsbDeviceData.szData), m_IntegratedData.nFocusCoarse, m_IntegratedData.nFocusFine);
                // WDx10 위치값
                m_IntegratedData.nWDPosition = RequestSetUsbDeviceData.nRequestData[2];
                m_nWDx10Position = m_IntegratedData.nWDPosition;

                // WDx10 위치값 표시
                // Display 1st Information Text
                Update1stImageInfoBitmap(m_SemSEScanControlPublicData.m_n1stImageInfoDrawX, m_SemSEScanControlPublicData.m_n1stImageInfoDrawY, m_SemSEScanControlPublicData.m_n1stImageInfoWidth, m_SemSEScanControlPublicData.m_n1stImageInfoHeight);
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_SEM_SCAN_ROTATION_VALUE)
            {
                // SCAN ROTATION
                m_IntegratedData.nScanRotValue = RequestSetUsbDeviceData.nRequestData[0];
                // SCAN ROTATION CONTROL-->DETAIL CONTROL : SCAN ROTATION POSITION 값
                m_wndDetailControlDlg.ScanRotationPositionToDetailControl(ToStr(RequestSetUsbDeviceData.szData));
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_SEM_SPOT_SIZE)
            {
                // SPOT SIZE
                m_IntegratedData.nSpotSize   = RequestSetUsbDeviceData.nRequestData[0];
                m_IntegratedData.nCLSpotSize = RequestSetUsbDeviceData.nRequestData[0];
                // SCAN CALIBRATION : SPOT SIZE 값 설정
                m_wndScanCalibrationToolDlg.SetCLSpotSizeControlIntegratedValue(m_IntegratedData.nSpotSize);
                // SCAN CALIBRATION : SPOT SIZE에 따른 CL1, CL2 값 설정
                m_wndScanCalibrationToolDlg.SetSpotSizeUsbDeviceData();
                // SPOT SIZE 값
                m_nSpotSizeValue = m_IntegratedData.nSpotSize;
                // SPOT SIZE 값 표시
                // Display 1st Information Text
                Update1stImageInfoBitmap(m_SemSEScanControlPublicData.m_n1stImageInfoDrawX, m_SemSEScanControlPublicData.m_n1stImageInfoDrawY, m_SemSEScanControlPublicData.m_n1stImageInfoWidth, m_SemSEScanControlPublicData.m_n1stImageInfoHeight);
            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:21420 ----
        // CLIENT --> DEVICE : SET USB DEVICE DATA ( STAGE CONTROL DATA )
        public int OnSetUsbDeviceStageControlDataToDeviceServer(int wParam, object lParam)
        {
            
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : SEM CONTROL DATA -> STAGE MOVE CONTROL
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // SEM STAGE CONTROL
            if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_SEM_STAGE_MOVE_EXECUTE_VALUE)
            {
                // AXIS MOVE CONTROL
                m_pAssistDeviceTool.SetStageAxisMove(RequestSetUsbDeviceData.nRequestData[0], RequestSetUsbDeviceData.nRequestData[1], RequestSetUsbDeviceData.nRequestData[2], RequestSetUsbDeviceData.nRequestData[3], RequestSetUsbDeviceData.dRequestData[0]);
            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_SEM_STAGE_CENTER_MOVE_EXECUTE)
            {
                // XY CENTER MOVE CONTROL
                m_pAssistDeviceTool.SetUsbDeviceStageXYCenterMove(RequestSetUsbDeviceData.nRequestData[0]);
            }


            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:21448 ----
        // CLIENT --> DEVICE : SET USB DEVICE DATA ( SEM CONTROL DATA -> START, STANDBY ... )
        public int OnSetUsbDeviceSEMControlDataToFlowControl(int wParam, object lParam)
        {
            // After displaying the dialog screen & Process function
            //if (m_pSocketProcess == NULL) return 0;
            //if (m_nDeviceSocketIndex < 0) return 0;
            
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : SEM CONTROL DATA -> SEM START, STANDBY
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            // SEM FLOW CONTROL 설정
            if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_SEM_CONTROL_START_EXECUTE)
            {
                //
                // SEM START 설정
                //

                
                // CLIENT->DEVICE서버 : SEM START 실행 상태설정 명령전송
                //m_pSocketProcess->SendCommand(m_nDeviceSocketIndex, CMDIDX_REQUEST_SET_DATA, (LPSTR)&RequestSetUsbDeviceData, sizeof(REQUEST_USB_DEVICE_DATA), sizeof(REQUEST_USB_DEVICE_DATA));

                // SCAN MODE       : FAST --> Index 7
                // SCAN FRAME      : 7.5[f/s]
                // PIXEL WIDTH     : 640
                // PIXEL HEIGHT    : 480
                // BITMAP X OFFSET : 0
                // BITMAP Y OFFSET : 0
                // BITMAP AVERAGE  : 6
                SemScanModeSelectAndScanStart(FAST_SCAN_MODE, 640, 480, 0, 0, 6, FALSE);

            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_SEM_CONTROL_STANDBY_EXECUTE)
            {
                //
                // SEM STANDBY 설정
                //

                // CLIENT->DEVICE서버 : SEM STANDBY 실행 상태설정 명령전송
                
                //m_pSocketProcess->SendCommand(m_nDeviceSocketIndex, CMDIDX_REQUEST_SET_DATA, (LPSTR)&RequestSetUsbDeviceData, sizeof(REQUEST_USB_DEVICE_DATA), sizeof(REQUEST_USB_DEVICE_DATA));

                // Capture Start Flag : False
                m_SemSEScanControlPublicData.m_bSEImageCaptureStart = FALSE;

                // SEM SE SCAN IMAGE DATA UPDATE ( DISPLAY ) TIMER 
                KillTimer(TIMER_ID_SE_SCAN_IMAGE_DISPLAY);
                KillTimer(TIMER_ID_SE_SCAN_IMAGE_DISPLAY_NEXT);
                // SEM SE PHOTO SCAN IMAGE DATA UPDATE ( DISPLAY ) TIMER 
                KillTimer(TIMER_ID_SE_PHOTO12_SCAN_IMAGE_DISPLAY);
                KillTimer(TIMER_ID_SE_PHOTO345_SCAN_IMAGE_DISPLAY);
                KillTimer(TIMER_ID_SE_PHOTO6_SCAN_IMAGE_DISPLAY);

                // 쓰레드 제거
                m_pSEScanImageDataProcessThread = null;
                // 파라메터 제거
                if (m_pSEScanImageDataThreadParams != null)
                {

                    m_pSEScanImageDataThreadParams = null;
                }

            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_SEM_CONTROL_GUN_HV_ON_OFF_FLAG)
            {
                //
                // SEM GUN HV ON/OFF 설정
                //

                
                // CLIENT->DEVICE서버 : SEM GUN HV ON/OFF 상태설정 명령전송
                //m_pSocketProcess->SendCommand(m_nDeviceSocketIndex, CMDIDX_REQUEST_SET_DATA, (LPSTR)&RequestSetUsbDeviceData, sizeof(REQUEST_USB_DEVICE_DATA), sizeof(REQUEST_USB_DEVICE_DATA));

                // 별도 설정 안함

            }
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_SET_USB_DEVICE_SEM_CONTROL_SHUTDOWN_EXECUTE)
            {
                //
                // SEM POWER SHUTDOWN 실행
                //

                
                // CLIENT->DEVICE서버 : SEM POWER SHUTDOWN 실행 명령전송
                //m_pSocketProcess->SendCommand(m_nDeviceSocketIndex, CMDIDX_REQUEST_SET_DATA, (LPSTR)&RequestSetUsbDeviceData, sizeof(REQUEST_USB_DEVICE_DATA), sizeof(REQUEST_USB_DEVICE_DATA));

                // USB DEVICE 실행
                m_pAssistDeviceTool.SetSemSystemShutDownExecute(RequestSetUsbDeviceData.nRequestData[0]);
            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:21546 ----
        // CLIENT FLOW CONTROL : SCAN IMAGE UPDATE ( SE SCAN, BSE SCAN, PHOTO, OPTICS, CHAMBER SCOPE ... )
        public int OnSemSEScainImageDataUpdateControl(int wParam, object lParam)
        {
            // After displaying the dialog screen & Process function

            // SE SCAN IMAGE UPDATE
            SemSEScanImageDataUpdateControl();

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:21558 ----
        // CLIENT FLOW CONTROL : SCAN MODE UPDATE ( RED, FAST, SLOW1/2, PHOTO1~6 )
        public int OnSemSEScainModeDataUpdateControl(int wParam, object lParam)
        {
            // After displaying the dialog screen & Process function

            // SCAN CALIBRATION DIALOG : SE SCAN MODE DATA UPDATE
            m_wndScanCalibrationToolDlg.SemSEScanModeDataUpdateControl((int)wParam);

            return 1;
        }

    }
}
