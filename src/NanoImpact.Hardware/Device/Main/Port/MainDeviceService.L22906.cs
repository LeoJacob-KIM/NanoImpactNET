// 원본 NanoImpact2020cfeDlg.cpp:22906-23139 을 NanoImpact.CodeGen translate로 옮긴 뒤 손으로 고친 코드.
#pragma warning disable IDE1006, IDE0059, CS0162, CS0168, CS0219
using System;
using NanoImpact.Core.Native;
using static NanoImpact.Core.Def;
using static NanoImpact.Core.Native.CRuntime;

namespace NanoImpact.Hardware.Device
{
    public sealed partial class MainDeviceService
    {
        // ---- NanoImpact2020cfeDlg.cpp:22906 ----
        //
        //
        // CLIENT SEM CONTROL DATA UPDATE ( DISPLAY 전용 )
        //
        //
        public int OnSetSemControlDataUpdate(int wParam, object lParam)
        {
            if (lParam == null) return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : HW SETUP OPTION DATA
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            //
            // SEM CONTROL DATA UPDATE
            //

            //  SEM VACUUM 상태값 UPDATE
            if (RequestSetUsbDeviceData.nRequestID == REQIDX_DEVICE_SET_SEM_VACUUM_STATE_DATA)
            {
                // VACUUM STATE DATA UPDATE
                m_wndVacuumControlDlg.SetVacuumStateInfo(RequestSetUsbDeviceData.nRequestData[0]);
                // VACUUM 상태값 표시
                DisplayVacuumStateInfo(RequestSetUsbDeviceData.nRequestData[0]);
            }
            //  SEM SPELLMAN HV 상태값 UPDATE
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_DEVICE_SET_SEM_SPELLMAN_HV_STATE_DATA)
            {
                // SPELLMAN HV STATE DATA UPDATE ( GUN/FIL/BIAS, PMT, SCINTILATOR, COLLECTOR )
                m_wndVacuumControlDlg.SetSpellmanHVStatus(RequestSetUsbDeviceData.nRequestData[0], RequestSetUsbDeviceData.nRequestData[1], RequestSetUsbDeviceData.nRequestData[2], RequestSetUsbDeviceData.nRequestData[3], RequestSetUsbDeviceData.nRequestData[4], RequestSetUsbDeviceData.nRequestData[5]);
            }


            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:22943 ----
        //
        // CLIENT SEM SE SCAN CONTROL DATA UPDATE ( CONTROL 전용 )
        //
        public int OnSetSemSEScanControlDataUpdate(int wParam, object lParam)
        {
            if (lParam == null)
                return 0;

            REQUEST_USB_DEVICE_DATA RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            RequestSetUsbDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : HW SETUP OPTION DATA
            RequestSetUsbDeviceData.CopyFrom((REQUEST_USB_DEVICE_DATA)lParam);

            //
            // SEM SE SCAN CONTROL DATA UPDATE
            //

            //  SEM SE SCAN MODE DATA UPDATE
            if (RequestSetUsbDeviceData.nRequestID == REQIDX_DEVICE_SET_SEM_SE_SCAN_CONTROL_DATA)
            {
                // SE SCAN MODE 설징 및 실행

                // 스캔모드 확인
                if (RequestSetUsbDeviceData.nRequestOption == STOP_SCAN_MODE)
                {
                    //
                    // SEM SE IMAGE SCAN : IMAGE CAPTURE STOP
                    //

                    // CAPTURE STOP
                    m_pAssistDeviceTool.SetSEMImageCaptureOnOffValueUsbDeviceData(0); 

                    //
                    // IMAGE SCAN DATA RECEIVE : STANDBY
                    //

                    // SCAN MODE : STOP STATE
                    m_SemSEScanControlPublicData.m_nSemSEScanModeValue  = STOP_SCAN_MODE;
                    m_SemSEScanControlPublicData.m_nSemSEScanModeBackup = STOP_SCAN_MODE;

                    // Capture Start Flag : False
                    m_SemSEScanControlPublicData.m_bSEImageCaptureStart = FALSE;

                    // PHOTO SCAN IMAGE 1-FRAME COMPLETED
                    m_SemSEScanControlPublicData.m_bPhotoScanCompleted = TRUE;

                    // SEM SE SCAN IMAGE DATA UPDATE ( DISPLAY ) TIMER 
                    KillTimer(TIMER_ID_SE_SCAN_IMAGE_DISPLAY);
                    KillTimer(TIMER_ID_SE_SCAN_IMAGE_DISPLAY_NEXT);
                    // SEM SE PHOTO SCAN IMAGE DATA UPDATE ( DISPLAY ) TIMER 
                    KillTimer(TIMER_ID_SE_PHOTO12_SCAN_IMAGE_DISPLAY);
                    KillTimer(TIMER_ID_SE_PHOTO345_SCAN_IMAGE_DISPLAY);
                    KillTimer(TIMER_ID_SE_PHOTO6_SCAN_IMAGE_DISPLAY);
                }
                // AC SYNC PHOTO SCAN START
                else if (RequestSetUsbDeviceData.nRequestOption == AC_SYNC_PHOTO_SCAN_MODE)
                {
                    // PHOTO1 SCAN MODE START
                    if (RequestSetUsbDeviceData.nRequestData[0] == 4)
                    {
                        // PHOTO1 MODE 선택
                        m_wndScanControlDlg.OnBnClickedScanControlPhoto1ScanBtn();
                        // PHOTO CAPTURE
                        m_wndScanControlDlg.OnBnClickedScanControlPhotoCaptureBtn();
                    }
                    // PHOTO2 SCAN MODE START
                    else if (RequestSetUsbDeviceData.nRequestData[0] == 5)
                    {
                        // PHOTO2 MODE 선택
                        m_wndScanControlDlg.OnBnClickedScanControlPhoto2ScanBtn();
                        // PHOTO CAPTURE
                        m_wndScanControlDlg.OnBnClickedScanControlPhotoCaptureBtn();
                    }
                    // PHOTO3 SCAN MODE START : OVERLAP
                    else if (RequestSetUsbDeviceData.nRequestData[0] == 6)
                    {
                        // PHOTO3 MODE 선택
                        m_wndScanControlDlg.OnBnClickedScanControlPhoto3ScanBtn();
                        // PHOTO CAPTURE
                        m_wndScanControlDlg.OnBnClickedScanControlPhotoCaptureBtn();
                    }
                    // PHOTO4 SCAN MODE START : 1280
                    else if (RequestSetUsbDeviceData.nRequestData[0] == 7)
                    {
                        // PHOTO4 MODE 선택
                        m_wndScanControlDlg.OnBnClickedScanControlPhoto4ScanBtn();
                        // PHOTO CAPTURE
                        m_wndScanControlDlg.OnBnClickedScanControlPhotoCaptureBtn();
                    }
                    // PHOTO6 SCAN MODE START : 2560
                    else if (RequestSetUsbDeviceData.nRequestData[0] == 9)
                    {
                        // PHOTO6 MODE 선택
                        m_wndScanControlDlg.OnBnClickedScanControlPhoto6ScanBtn();
                        // PHOTO CAPTURE
                        m_wndScanControlDlg.OnBnClickedScanControlPhotoCaptureBtn();
                    }
                }
                else
                {
                    // 스캔설정 명령 (RED, FAST, SLOW1, SLOW2, PHOTO1, PHOTO2, PHOTO3, PHOTO4, PHOTO5, PHOTO6)
                    // 0:Scan Control, 1:스캔 FREQ 만 변경한다
                    if (RequestSetUsbDeviceData.nRequestType == 0)
                    {
                        // 스캔모드 설정 : 스캔모드, 가로폭, 세로폭, X시작위치, Y시작위치, AVERAGE, SCAN GENERATION
                        SemScanModeSelectAndScanStart(RequestSetUsbDeviceData.nRequestOption,
                            RequestSetUsbDeviceData.nRequestData[0],
                            RequestSetUsbDeviceData.nRequestData[1],
                            RequestSetUsbDeviceData.nRequestData[2],
                            RequestSetUsbDeviceData.nRequestData[3],
                            RequestSetUsbDeviceData.nRequestData[4],
                            RequestSetUsbDeviceData.nRequestData[5]);
                    }
                    else
                    {
                        // SLOW1 <--> SLOW2 SCAN MODE : 스캔 FREQ 만 변경한 후, SCAN 이미지를 계속해서 받아 들인다
                        SemSlow12ScanModeSelectAndScanStart(RequestSetUsbDeviceData.nRequestOption,
                            RequestSetUsbDeviceData.nRequestData[0],
                            RequestSetUsbDeviceData.nRequestData[1],
                            RequestSetUsbDeviceData.nRequestData[2],
                            RequestSetUsbDeviceData.nRequestData[3],
                            RequestSetUsbDeviceData.nRequestData[4]);
                    }
                }

                // 음영반전
                if (m_SemSEScanControlPublicData.m_bPhotoInverse1SelectFlag == TRUE)
                {
                    m_dev.AfxSetSEMNegativeImageCheckFlag(m_SemSEScanControlPublicData.m_bPhotoInverse1SelectFlag, TRUE);
                }
                // 좌우반전
                if (m_SemSEScanControlPublicData.m_bPhotoInverse2SelectFlag == TRUE)
                {
                    m_dev.AfxSetSEMLRMirrorImageCheckFlag(m_SemSEScanControlPublicData.m_bPhotoInverse2SelectFlag, TRUE);
                }
                // 상하반전
                if (m_SemSEScanControlPublicData.m_bPhotoInverse3SelectFlag == TRUE)
                {
                    m_dev.AfxSetSEMTBMirrorImageCheckFlag(m_SemSEScanControlPublicData.m_bPhotoInverse3SelectFlag, TRUE);
                }

            }
            //  SEM SE SCAN IMAGE DATA UPDATE
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_DEVICE_SET_SEM_SE_SCAN_IMAGE_DATA)
            {
            }
            //  SEM SE SCAN IMAGE INFO DATA UPDATE
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_DEVICE_SET_SEM_SE_SCAN_IMAGE_INFO_DATA)
            {
                // 포토 스캔모드 확인
                if (RequestSetUsbDeviceData.nRequestOption == PHOTO1_SCAN_MODE || RequestSetUsbDeviceData.nRequestOption == PHOTO2_SCAN_MODE || RequestSetUsbDeviceData.nRequestOption == PHOTO3_SCAN_MODE ||
                    RequestSetUsbDeviceData.nRequestOption == PHOTO4_SCAN_MODE || RequestSetUsbDeviceData.nRequestOption == PHOTO5_SCAN_MODE || RequestSetUsbDeviceData.nRequestOption == PHOTO6_SCAN_MODE)
                {
                    // SCAN INFO DATA UPDATE : X시작위치, Y시작위치, 가로폭, 세로폭
                    Update2ndImageInfoBitmap(RequestSetUsbDeviceData.nRequestData[0], RequestSetUsbDeviceData.nRequestData[1], RequestSetUsbDeviceData.nRequestData[2], RequestSetUsbDeviceData.nRequestData[3]);
                }
            }
            //  SEM SE SCAN AUTO PROCESSING DATA UPDATE
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_DEVICE_SET_SEM_SE_AUTO_PROCESS_DATA)
            {
                // AUTO PROCESSING COMMAND 확인
                if (RequestSetUsbDeviceData.nRequestOption == SEM_SE_AUTO_CONTRAST_PROCESS)
                {
                    // AUTO CONTRAST PROCESSING
                    SetExecuteAutoContrastProcessing();
                }
                else if (RequestSetUsbDeviceData.nRequestOption == SEM_SE_AUTO_BRIGHTNESS_PROCESS)
                {
                    // AUTO BRIGHTNESS PROCESSING
                    SetExecuteAutoBrightnessProcessing();
                }
                else if (RequestSetUsbDeviceData.nRequestOption == SEM_SE_AUTO_CONT_BRIGHT_PROCESS)
                {
                    // AUTO CONTRAST & BRIGHTNESS PROCESSING
                    SetExecuteAutoContrastBrightnessProcessing();
                }
                else if (RequestSetUsbDeviceData.nRequestOption == SEM_SE_AUTO_FOCUS_PROCESS)
                {
                    // AUTO FOCUS PROCESSING
                    SetExecuteAutoFocusProcessing();
                }
                else if (RequestSetUsbDeviceData.nRequestOption == SEM_SE_AUTO_PROCESS_STOP)
                {
                    // AUTO PROCESSING STOP
                    SetExecuteAutoProcessingStop();
                }
            }
            //  MAIN DLG GUN HV COMBO INDEX DATA UPDATE
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_DEVICE_SET_SEM_MAIN_DLG_GUNHV_COMBO_DATA)
            {
                SetMainDlgGunHVComboIndex(RequestSetUsbDeviceData.nRequestData[0]);
            }
            //  MAIN DLG MAGNIFICATION COMBO INDEX DATA UPDATE
            else if (RequestSetUsbDeviceData.nRequestID == REQIDX_DEVICE_SET_SEM_MAIN_DLG_MAG_COMBO_DATA)
            {
                SetMainDlgMagComboIndex(RequestSetUsbDeviceData.nRequestData[0]);
            }

            return 1;
        }

    }
}
