// 원본 NanoImpact2020cfeDlg.cpp:9564-10183 을 NanoImpact.CodeGen translate로 옮긴 뒤 손으로 고친 코드.
#pragma warning disable IDE1006, IDE0059, CS0162, CS0168, CS0219
using System;
using NanoImpact.Core.Native;
using static NanoImpact.Core.Def;
using static NanoImpact.Core.Native.CRuntime;

namespace NanoImpact.Hardware.Imaging
{
    public sealed partial class SemScanImageProcessor
    {
        // ---- NanoImpact2020cfeDlg.cpp:9564 ----
        // CLIENT --> DEVICE : SET USB DEVICE DATA ( SEM SCAN MODE SELECT & SCAN START )
        public int SemScanModeSelectAndScanStart(int nScanMode, int nScanWidth, int nScanHeight, int nBitmapXOffset, int nBitmapYOffset, int nBitmapAverage, int bScanGeneration)
        {
            // 2020.11.25
            int bAcSyncFlag = AfxGetIntegratedPointer().bScanOptionFlag[IDX_HWSETUP_OPTION_BOOL_AC_SYNC_ON_OFF];

            //
            // SEM SE IMAGE SCAN : IMAGE CAPTURE STOP
            //
            if (bScanGeneration == TRUE)
            {
                // CAPTURE STOP
                m_pAssistDeviceTool.SetSEMImageCaptureOnOffValueUsbDeviceData(0);

                // Capture Start Flag : False
                m_SemSEScanControlPublicData.m_bSEImageCaptureStart = FALSE;

                // SEM SE SCAN IMAGE DATA UPDATE ( DISPLAY ) TIMER 
                KillTimer(TIMER_ID_SE_SCAN_IMAGE_DISPLAY);
                KillTimer(TIMER_ID_SE_SCAN_IMAGE_DISPLAY_NEXT);
                // SEM SE PHOTO SCAN IMAGE DATA UPDATE ( DISPLAY ) TIMER 
                KillTimer(TIMER_ID_SE_PHOTO12_SCAN_IMAGE_DISPLAY);
                KillTimer(TIMER_ID_SE_PHOTO345_SCAN_IMAGE_DISPLAY);
                KillTimer(TIMER_ID_SE_PHOTO6_SCAN_IMAGE_DISPLAY);
            }

            // SCAN MODE : RED, FAST, SLOW1, SLOW2 --> 1st Bitmap Image Draw
            if (nScanMode == RED_SCAN_MODE ||
                nScanMode == FAST_SCAN_MODE ||
                nScanMode == SLOW1_SCAN_MODE ||
                nScanMode == SLOW2_SCAN_MODE)
            {
                if (nScanMode == RED_SCAN_MODE)
                {
                    Draw1stREDScanBox(nBitmapXOffset - 1, nBitmapYOffset - 1, ((nBitmapXOffset - 1) + nScanWidth) + 1, ((nBitmapYOffset - 1) + nScanHeight) + 1, RED_SCAN_BOX_COLOR);
                }
            }
            // SCAN MODE : PHOTO1~6 --> 2nd Bitmap Image Draw
            else if (nScanMode == PHOTO1_SCAN_MODE ||
                nScanMode == PHOTO2_SCAN_MODE ||
                nScanMode == PHOTO3_SCAN_MODE ||
                nScanMode == PHOTO4_SCAN_MODE ||
                nScanMode == PHOTO5_SCAN_MODE ||
                nScanMode == PHOTO6_SCAN_MODE)
            {
                ;
            }
            else
            {
                return 0;
            }

            m_SemSEScanControlPublicData.m_nSEAutoBrightContrastCount = 0;         // SEM SE Frame IMAGE DATA 의 Auto Bright, Contrast Code 갯수
            m_SemSEScanControlPublicData.m_nSEAutoBrightValue         = 0;         // SEM SE Frame IMAGE DATA 의 Auto Bright   값
            m_SemSEScanControlPublicData.m_nSEAutoContrastValue       = 0;         // SEM SE Frame IMAGE DATA 의 Auto Contrast 값

            // Aver Count Check?
            if (nBitmapAverage < 1)
                nBitmapAverage = 1;

            if (nBitmapAverage > MAX_SE_SCAN_IMAGE_AVERAGE)
                nBitmapAverage = MAX_SE_SCAN_IMAGE_AVERAGE;

            //
            // SEM SCAN START 설정
            //

            // SEM SE SCAN IMAGE DATA 초기화
            // SEM SE SCAN IMAGE BUFFER CLEAR
            memset(m_pSemSEScanImageReadBuffer  , 0x00, m_nSemSEScanImageDataMax);    // 이미지 처리용 저장버퍼
            memset(m_pSemSEScanImageThreadBuffer, 0x00, m_nSemSEScanImageDataMax);    // 쓰레드 처리용 임시버퍼
            memset(m_pSemSEScanImageTempBuffer  , 0x00, m_nSemSEScanImageDataMax);    // 이미지 저장 위치 재조정 버퍼

            // RECEIVE COUNT & INDEX 초기화
            m_nSemSEScanImageReadStart = 0;      // SEM SE SCAN IMAGE READ BUFFER : 임시 버퍼 처리 시작 위치
            m_nSemSEScanImageSaveStart = 0;      // SEM SE SCAN IMAGE READ BUFFER : 임시 버퍼 처리 저장 위치
            m_nSemSEScanImageSaveCount = 0;      // SEM SE SCAN IMAGE READ BUFFER : 임시 버퍼 처리 저장 위치
            m_nReceiveDataCount        = 0;
            m_nReceiveTotalImageCount  = 0;

            // BITMAP AVERAGE 사용변수 CLEAR
            // BITMAP BUFFER CLEAR 
            memset(m_pSemSEScanImageBitmapBufferA  , 0x00, STD_SE_SCAN_IMAGE_WIDTH*STD_SE_SCAN_IMAGE_HEIGHT*MAX_SE_SCAN_IMAGE_AVERAGE); // FRAME A ( RED, FAST, SLOW1/2 )
            m_pSemSEScanImageBitmapBufferB.Clear(); // FRAME B ( PHOTO MODE )
            memset(m_pSemSEScanImageBitmapBufferBF1, 0x00, MAX_SE_SCAN_IMAGE_WIDTH*MAX_SE_SCAN_IMAGE_HEIGHT);                           // FRAME B ( PHOTO MODE : FILTER #1)
            memset(m_pSemSEScanImageBitmapBufferBF2, 0x00, MAX_SE_SCAN_IMAGE_WIDTH*MAX_SE_SCAN_IMAGE_HEIGHT);                           // FRAME B ( PHOTO MODE : FILTER #2)

            m_pSEScanImageBufferA = null;       // SE Scan Image 저장 Buffer Pointer : FRAME A
            m_pSEScanImageBitmapA = null;       // SE Scan Image Display Bitmap Buffer Pointer : FRAME A
            m_pSEScanImageBufferB = null;       // SE Scan Image 저장 Buffer Pointer : FRAME B (PHOTO MODE)
            m_pSEScanImageBitmapB = null;       // SE Scan Image Display Bitmap Buffer Pointer : FRAME B (PHOTO MODE)

            m_SemSEScanControlPublicData.m_bSEImageCaptureStart     = FALSE; // SE SCAN IMAGE CAPTURE START Flag
            m_SemSEScanControlPublicData.m_nABCCount                = 0;     // Auto Contrast/Brightness
            m_SemSEScanControlPublicData.m_nAvrContrast             = 0;     // Auto Contrast   변수
            m_SemSEScanControlPublicData.m_nAvrBright               = 0;     // Auto Brightness 변수
            m_SemSEScanControlPublicData.m_wAvrAutoValue[0]         = 0;     // Auto Contrast/Brightness 변수
            m_SemSEScanControlPublicData.m_wAvrAutoValue[1]         = 0;     // Auto Contrast/Brightness 변수
            m_SemSEScanControlPublicData.m_nSEFrameCount            = 0;     // SE SCAN IMAGE FRAME Count
            m_SemSEScanControlPublicData.m_nSEPixelCount            = 0;     // SE SCAN IMAGE PIXEL Count
            m_SemSEScanControlPublicData.m_nSEFrameBrightLevel      = 0;     // SE SCAN IMAGE FRAME BRIGHTNESS Level
            m_SemSEScanControlPublicData.m_nSEPixelLevelSum         = 0;     // SE SCAN IMAGE PIXEL LEVEL Sum
            m_SemSEScanControlPublicData.m_nSEPixelLevelCount       = 0;     // SE SCAN IMAGE PIXEL LEVEL COUNT
            m_SemSEScanControlPublicData.m_nSEDualScreenModeFlag    = 0;     // SE SCAN IMAGE DUAL SCREEN Mode
            m_SemSEScanControlPublicData.m_nSEDrawXPositionA        = 0;     // SE SCAN IMAGE X Draw Position : FRAME A
            m_SemSEScanControlPublicData.m_nSEDrawYPositionA        = 0;     // SE SCAN IMAGE Y Draw Position : FRAME A
            m_SemSEScanControlPublicData.m_nSEDrawXPositionB        = 0;     // SE SCAN IMAGE X Draw Position : FRAME B (Photo Mode)
            m_SemSEScanControlPublicData.m_nSEDrawYPositionB        = 0;     // SE SCAN IMAGE Y Draw Position : FRAME B (Photo Mode)
            m_SemSEScanControlPublicData.m_nSEBitmapXOffsetA        = 0;     // SE SCAN IMAGE BITMAP X Offset
            m_SemSEScanControlPublicData.m_nSEBitmapYOffsetA        = 0;     // SE SCAN IMAGE BITMAP Y Offset
            m_SemSEScanControlPublicData.m_nSEBitmapXOffsetB        = 0;     // SE SCAN IMAGE BITMAP X Offset (Photo Mode)
            m_SemSEScanControlPublicData.m_nSEBitmapYOffsetB        = 0;     // SE SCAN IMAGE BITMAP Y Offset (Photo Mode)
            m_SemSEScanControlPublicData.m_nSEAverageNumber         = 0;     // SE SCAN IMAGE Average Number
            m_SemSEScanControlPublicData.m_nSEAverageCount          = 0;     // SE SCAN IMAGE Average Count
            m_SemSEScanControlPublicData.m_nSEAverageIndex          = 0;     // SE SCAN IMAGE Average Count Index
            m_SemSEScanControlPublicData.m_nSEFrameAverageA         = 0;     // SE SCAN IMAGE Frame A Average Count
            m_SemSEScanControlPublicData.m_nSEFrameAverageB         = 0;     // SE SCAN IMAGE Frame B Average Count (Dual Screen Mode)
            m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth      = 0;     // SE SCAN IMAGE X PIXEL WIDTH
            m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight     = 0;     // SE SCAN IMAGE Y PIXEL HEIGHT
            m_SemSEScanControlPublicData.m_nSemSEScanModeValue      = 0;     // SE SCAN MODE VALUE ( RED, FAST, SLOW1, SLOW2, PHOTO1~6 )

            // SE SCAN IMAGE BUFFER : INIT
            m_nSemSEScanImageReadStart = 0;          // SEM SE SCAN IMAGE READ BUFFER : 임시 버퍼 처리 시작 위치
            m_nSemSEScanImageSaveStart = 0;          // SEM SE SCAN IMAGE READ BUFFER : 임시 버퍼 처리 저장 위치
            m_nSemSEScanImageSaveCount = 0;          // SEM SE SCAN IMAGE READ BUFFER : 임시 버퍼 처리 저장 크기
            m_nSemSEScanImageReadSize  = SCAN_IMAGE_PACKET_SIZE;                             // 임시 버퍼 처리 크기
            m_bSemSEFrameFirstCatch    = FALSE;      // SEM SE SCAN IMAGE READ BUFFER : FRAME 처음 읽기 처리 완료

            memset(m_pSemSEScanImageReadBuffer  , 0x00, m_nSemSEScanImageDataMax);  // 이미지 처리용 저장버퍼
            memset(m_pSemSEScanImageThreadBuffer, 0x00, m_nSemSEScanImageDataMax);  // 쓰레드 처리용 임시버퍼
            memset(m_pSemSEScanImageTempBuffer  , 0x00, m_nSemSEScanImageDataMax);  // 이미지 저장 위치 재조정 임시버퍼

            // Frame Rate Test
            m_dwFirstTick         = GetTickCount();
            m_dwSecondTick        = m_dwFirstTick;
            m_dwFrameDataCount    = 0;
            m_dwDisplayFrameCount = m_dwFrameDataCount;

            // SCAN MODE : RED, FAST, SLOW1, SLOW2 --> 1st Bitmap Image Draw
            if (nScanMode == RED_SCAN_MODE ||
                nScanMode == FAST_SCAN_MODE ||
                nScanMode == SLOW1_SCAN_MODE ||
                nScanMode == SLOW2_SCAN_MODE)
            {
                // Capture Start Flag
                m_SemSEScanControlPublicData.m_nSEFrameCount        = 0;                 // SE SCAN IMAGE FRAME COUNT
                m_SemSEScanControlPublicData.m_nSEAverageNumber     = nBitmapAverage;    // BITMAP AGERAGE 설정 ( RED:6, FAST:6, SLOW1:1, SLOW2:1 )
                m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth  = nScanWidth;        // X-WIDTH
                m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight = nScanHeight;       // Y-HEIGHT
                m_SemSEScanControlPublicData.m_bSEImageCaptureStart = TRUE;              // SE SCAN IMAGE CAPTURE 시작됨
                m_SemSEScanControlPublicData.m_nSEDrawXPositionA    = 0;                 // SE SCAN IMAGE X Draw Position : FRAME A
                m_SemSEScanControlPublicData.m_nSEDrawYPositionA    = 0;                 // SE SCAN IMAGE Y Draw Position : FRAME A
                m_SemSEScanControlPublicData.m_nSEBitmapXOffsetA    = nBitmapXOffset;    // SE SCAN IMAGE BITMAP X Offset
                m_SemSEScanControlPublicData.m_nSEBitmapYOffsetA    = nBitmapYOffset;    // SE SCAN IMAGE BITMAP Y Offset
                m_SemSEScanControlPublicData.m_nSemSEScanModeValue  = nScanMode;         // RED, FAST, SLOW1, SLOW2, PHOTO1~6
                m_SemSEScanControlPublicData.m_nSaveScanMode        = nScanMode;         // RED, FAST, SLOW1, SLOW2, PHOTO1~6
                m_SemSEScanControlPublicData.m_bPhotoScanCompleted  = TRUE;              // PHOTO SCAN IMAGE 1-FRAME COMPLETED

                // SEM SCAN IMAGE BITMAP DATA
                m_bm1stBitmap = m_1stImageBitmap.Info.Clone();
                // 1st BITMAP ARRAY RELEASE
                if (m_p1stBitmapBits != null)
                {
                    m_p1stBitmapBits = null;
                }
                // 1st BITMAP ARRAY CREATE
                m_p1stBitmapBits = new RGBQUAD[(m_bm1stBitmap.bmWidthBytes * m_bm1stBitmap.bmHeight) / 4];
                // 1st BITMAP ARRAY CLEAR
                memset(m_p1stBitmapBits, 0x00, m_bm1stBitmap.bmWidthBytes * m_bm1stBitmap.bmHeight);
                // GET BITMAP DATA
                m_1stImageBitmap.GetBitmapBits(m_bm1stBitmap.bmWidthBytes * m_bm1stBitmap.bmHeight, m_p1stBitmapBits);
                // 1st RGB COLOR POINT
                m_p1stImageRgb = m_p1stBitmapBits;

                //
                // Background Color
                //
                if (m_SemSEScanControlPublicData.m_nSemSEScanModeValue == RED_SCAN_MODE)
                {
                    // CLEAR 하지 않는다
                    if (m_SemSEScanControlPublicData.m_nSemSEScanModeBackup != RED_SCAN_MODE)
                    {
                        // BITMAP IMAGE UPDATE
                        SemSEScanImageDataUpdateControl();
                    }
                }
                else if (m_SemSEScanControlPublicData.m_nSemSEScanModeValue == FAST_SCAN_MODE)
                {
                    if (m_SemSEScanControlPublicData.m_nSemSEScanModeBackup != FAST_SCAN_MODE)
                    {
                        // SLOW1/SLOW2 이면 CLEAR
                        //Fill1stImageBitmap(0, 0, STD_SE_SCAN_IMAGE_WIDTH, STD_SE_SCAN_IMAGE_HEIGHT, DISPLAY_1ST_IMAGE_BACKGROUND_COLOR, TRUE);
                        Fill1stImageBitmap(0, 0, m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth, m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight, DISPLAY_1ST_IMAGE_BACKGROUND_COLOR, TRUE);
                        // SE SCAN IMAGE PIXEL BITMAP DATA UPDATE ( REDRAW )
                        m_1stImageBitmap.SetBitmapBits(m_bm1stBitmap.bmWidthBytes * m_bm1stBitmap.bmHeight, m_p1stBitmapBits);
                        // BITMAP IMAGE UPDATE
                        SemSEScanImageDataUpdateControl();
                    }
                }
                else if (m_SemSEScanControlPublicData.m_nSemSEScanModeValue == SLOW1_SCAN_MODE)
                {
                    // SLOW1 <--> SLOW2 SCAN MODE 전환시에는 BITMAP DATA를 CLEAR 하지 않는다
                    if (m_SemSEScanControlPublicData.m_nSemSEScanModeBackup != SLOW2_SCAN_MODE || bAcSyncFlag == TRUE)
                    {
                        // SLOW2 가 아니면 CLEAR
                        //Fill1stImageBitmap(0, 0, STD_SE_SCAN_IMAGE_WIDTH, STD_SE_SCAN_IMAGE_HEIGHT, DISPLAY_1ST_IMAGE_BACKGROUND_COLOR, TRUE);
                        Fill1stImageBitmap(0, 0, m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth, m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight, DISPLAY_1ST_IMAGE_BACKGROUND_COLOR, TRUE);
                        // SE SCAN IMAGE PIXEL BITMAP DATA UPDATE ( REDRAW )
                        m_1stImageBitmap.SetBitmapBits(m_bm1stBitmap.bmWidthBytes * m_bm1stBitmap.bmHeight, m_p1stBitmapBits);
                        // BITMAP IMAGE UPDATE
                        SemSEScanImageDataUpdateControl();
                    }
                }
                else if (m_SemSEScanControlPublicData.m_nSemSEScanModeValue == SLOW2_SCAN_MODE)
                {
                    // SLOW2 <--> SLOW1 SCAN MODE 전환시에는 BITMAP DATA를 CLEAR 하지 않는다
                    if (m_SemSEScanControlPublicData.m_nSemSEScanModeBackup != SLOW1_SCAN_MODE || bAcSyncFlag == TRUE)
                    {
                        // SLOW1 이 아니면 CLEAR
                        //Fill1stImageBitmap(0, 0, STD_SE_SCAN_IMAGE_WIDTH, STD_SE_SCAN_IMAGE_HEIGHT, DISPLAY_1ST_IMAGE_BACKGROUND_COLOR, TRUE);
                        Fill1stImageBitmap(0, 0, m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth, m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight, DISPLAY_1ST_IMAGE_BACKGROUND_COLOR, TRUE);
                        // SE SCAN IMAGE PIXEL BITMAP DATA UPDATE ( REDRAW )
                        m_1stImageBitmap.SetBitmapBits(m_bm1stBitmap.bmWidthBytes * m_bm1stBitmap.bmHeight, m_p1stBitmapBits);
                        // BITMAP IMAGE UPDATE
                        SemSEScanImageDataUpdateControl();
                    }
                }

                // SEM SE SCAN IMAGE DATA UPDATE ( DISPLAY ) TIMER  : 처음업데이트 타임 (잠시 기다린 후, 빠르게 업데이트 한다)
                SetTimer(TIMER_ID_SE_SCAN_IMAGE_DISPLAY, SE_SCAN_IMAGE_UPDATE_ELAPSE_TIME*20, NULL);
            }
            // SCAN MODE : PHOTO 1~2 --> 2nd Bitmap Image Draw
            else if (nScanMode == PHOTO1_SCAN_MODE ||
                     nScanMode == PHOTO2_SCAN_MODE)
            {
                // Capture Start Flag
                m_SemSEScanControlPublicData.m_nSEFrameCount        = 0;                 // SE SCAN IMAGE FRAME COUNT
                m_SemSEScanControlPublicData.m_nSEAverageNumber     = nBitmapAverage;    // BITMAP AGERAGE 설정 ( RED:6, FAST:6, SLOW1:1, SLOW2:1 )
                m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth  = nScanWidth;        // X-WIDTH
                m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight = nScanHeight;       // Y-HEIGHT
                m_SemSEScanControlPublicData.m_bSEImageCaptureStart = TRUE;              // SE SCAN IMAGE CAPTURE 시작됨
                m_SemSEScanControlPublicData.m_nSEDrawXPositionB    = 0;                 // SE SCAN IMAGE X Draw Position : FRAME B
                m_SemSEScanControlPublicData.m_nSEDrawYPositionB    = 0;                 // SE SCAN IMAGE Y Draw Position : FRAME B
                m_SemSEScanControlPublicData.m_nSEBitmapXOffsetB    = nBitmapXOffset;    // SE SCAN IMAGE BITMAP X Offset
                m_SemSEScanControlPublicData.m_nSEBitmapYOffsetB    = nBitmapYOffset;    // SE SCAN IMAGE BITMAP Y Offset
                m_SemSEScanControlPublicData.m_nSemSEScanModeValue  = nScanMode;         // RED, FAST, SLOW1, SLOW2, PHOTO1~6
                m_SemSEScanControlPublicData.m_bPhotoScanCompleted  = FALSE;             // PHOTO SCAN IMAGE 1-FRAME COMPLETED

                //
                // SEM PHOTO SCAN IMAGE BITMAP DATA
                //
                m_bm2ndBitmap = m_2ndImage640x480Bitmap.Info.Clone();
                // BITMAP DATA 삭제
                if (m_p2ndBitmapBits != null)
                {
                    m_p2ndBitmapBits = null;
                }
                if (m_p2ndOrgBitmapBits != null)
                {
                    m_p2ndOrgBitmapBits = null;
                }
                // 2nd PHOTO LAYER
                m_nLayerPhotoMode = 1; // PHOTO1/2 SCAN MODE ( 640x480 )
                // 2nd BITMAP ARRAY CREATE
                m_p2ndBitmapBits    = new RGBQUAD[(m_bm2ndBitmap.bmWidthBytes * m_bm2ndBitmap.bmHeight) / 4]; // 실사용 PHOTO IMAGE
                m_p2ndOrgBitmapBits = new RGBQUAD[(m_bm2ndBitmap.bmWidthBytes * m_bm2ndBitmap.bmHeight) / 4]; // 원본 저장용 PHOTO IMAGE
                // 2nd BITMAP ARRAY CLEAR
                memset(m_p2ndBitmapBits   , 0x00, m_bm2ndBitmap.bmWidthBytes * m_bm2ndBitmap.bmHeight);
                memset(m_p2ndOrgBitmapBits, 0x00, m_bm2ndBitmap.bmWidthBytes * m_bm2ndBitmap.bmHeight);
                // GET PHOTO BITMAP DATA
                m_2ndImage640x480Bitmap.GetBitmapBits(m_bm2ndBitmap.bmWidthBytes * m_bm2ndBitmap.bmHeight, m_p2ndBitmapBits);
                // 2nd RGB COLOR POINT
                m_p2ndImageRgb = m_p2ndBitmapBits;
                // Background Color
                Fill2ndImage640x480Bitmap(0, 0, m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth, m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight, DISPLAY_2ND_IMAGE_BACKGROUND_COLOR, TRUE);

                //
                // PHOTO LAYER IMAGE BITMAP DATA
                //
                m_bm2ndLayerBitmap = m_2ndLayer640x480Bitmap.Info.Clone();
                if (m_p2ndLayerBitmapBits != null)
                {
                    m_p2ndLayerBitmapBits = null;
                }
                // 2nd LAYER BITMAP ARRAY CREATE
                m_p2ndLayerBitmapBits = new RGBQUAD[(m_bm2ndLayerBitmap.bmWidthBytes * m_bm2ndLayerBitmap.bmHeight) / 4];
                // 2nd LAYER BITMAP ARRAY CLEAR
                memset(m_p2ndLayerBitmapBits, 0x00, m_bm2ndLayerBitmap.bmWidthBytes * m_bm2ndLayerBitmap.bmHeight);
                // GET PHOTO BITMAP DATA
                m_2ndLayer640x480Bitmap.GetBitmapBits(m_bm2ndLayerBitmap.bmWidthBytes * m_bm2ndLayerBitmap.bmHeight, m_p2ndLayerBitmapBits);
                // 2nd LAYER RGB COLOR POINT
                m_p2ndLayerRgb = m_p2ndLayerBitmapBits;
                // Layer Background Color
                Fill2ndLayer640x480Bitmap(0, 0, m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth, m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight, RGB(0, 0, 0), TRUE);

                // SEM SE PHOTO SCAN IMAGE DATA UPDATE ( DISPLAY ) TIMER 
                SetTimer(TIMER_ID_SE_PHOTO12_SCAN_IMAGE_DISPLAY, SE_PHOTO12_SCAN_IMAGE_UPDATE_ELAPSE_TIME, NULL);
            }
            // SCAN MODE : PHOTO 3~5 --> 2nd Bitmap Image Draw
            else if (nScanMode == PHOTO3_SCAN_MODE ||
                     nScanMode == PHOTO4_SCAN_MODE ||
                     nScanMode == PHOTO5_SCAN_MODE)
            {
                // Capture Start Flag
                m_SemSEScanControlPublicData.m_nSEFrameCount        = 0;                 // SE SCAN IMAGE FRAME COUNT
                m_SemSEScanControlPublicData.m_nSEAverageNumber     = nBitmapAverage;    // BITMAP AGERAGE 설정 ( RED:6, FAST:6, SLOW1:1, SLOW2:1 )
                m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth  = nScanWidth;        // X-WIDTH
                m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight = nScanHeight;       // Y-HEIGHT
                m_SemSEScanControlPublicData.m_bSEImageCaptureStart = TRUE;              // SE SCAN IMAGE CAPTURE 시작됨
                m_SemSEScanControlPublicData.m_nSEDrawXPositionB    = 0;                 // SE SCAN IMAGE X Draw Position : FRAME B
                m_SemSEScanControlPublicData.m_nSEDrawYPositionB    = 0;                 // SE SCAN IMAGE Y Draw Position : FRAME B
                m_SemSEScanControlPublicData.m_nSEBitmapXOffsetB    = nBitmapXOffset;    // SE SCAN IMAGE BITMAP X Offset
                m_SemSEScanControlPublicData.m_nSEBitmapYOffsetB    = nBitmapYOffset;    // SE SCAN IMAGE BITMAP Y Offset
                m_SemSEScanControlPublicData.m_nSemSEScanModeValue  = nScanMode;         // RED, FAST, SLOW1, SLOW2, PHOTO1~6
                m_SemSEScanControlPublicData.m_bPhotoScanCompleted  = FALSE;             // PHOTO SCAN IMAGE 1-FRAME COMPLETED

                //
                // SEM PHOTO SCAN IMAGE BITMAP DATA
                //
                m_bm2ndBitmap = m_2ndImage1280x960Bitmap.Info.Clone();
                // BITMAP DATA 삭제
                if (m_p2ndBitmapBits != null)
                {
                    m_p2ndBitmapBits = null;
                }
                if (m_p2ndOrgBitmapBits != null)
                {
                    m_p2ndOrgBitmapBits = null;
                }
                // 2nd PHOTO LAYER
                m_nLayerPhotoMode = 2; // PHOTO3/4/5 SCAN MODE ( 1280x960 )
                // 2nd BITMAP ARRAY CREATE
                m_p2ndBitmapBits    = new RGBQUAD[(m_bm2ndBitmap.bmWidthBytes * m_bm2ndBitmap.bmHeight) / 4]; // 실사용
                m_p2ndOrgBitmapBits = new RGBQUAD[(m_bm2ndBitmap.bmWidthBytes * m_bm2ndBitmap.bmHeight) / 4]; // 원본 저장용
                // 2nd BITMAP ARRAY CLEAR
                memset(m_p2ndBitmapBits   , 0x00, m_bm2ndBitmap.bmWidthBytes * m_bm2ndBitmap.bmHeight);
                memset(m_p2ndOrgBitmapBits, 0x00, m_bm2ndBitmap.bmWidthBytes * m_bm2ndBitmap.bmHeight);
                // GET PHOTO BITMAP DATA
                m_2ndImage1280x960Bitmap.GetBitmapBits(m_bm2ndBitmap.bmWidthBytes * m_bm2ndBitmap.bmHeight, m_p2ndBitmapBits);
                // 2nd RGB COLOR POINT
                m_p2ndImageRgb = m_p2ndBitmapBits;
                // Background Color
                Fill2ndImage1280x960Bitmap(0, 0, m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth, m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight, DISPLAY_2ND_IMAGE_BACKGROUND_COLOR, TRUE);

                // FULL SIZE IMAGE SCAN FLAG
                m_n2ndFullSizeImageScan = 1;

                //
                // PHOTO LAYER IMAGE BITMAP DATA
                //
                m_bm2ndLayerBitmap = m_2ndLayer1280x960Bitmap.Info.Clone();
                if (m_p2ndLayerBitmapBits != null)
                {
                    m_p2ndLayerBitmapBits = null;
                }
                // 2nd LAYER BITMAP ARRAY CREATE
                m_p2ndLayerBitmapBits = new RGBQUAD[(m_bm2ndLayerBitmap.bmWidthBytes * m_bm2ndLayerBitmap.bmHeight) / 4];
                // 2nd LAYER BITMAP ARRAY CLEAR
                memset(m_p2ndLayerBitmapBits, 0x00, m_bm2ndLayerBitmap.bmWidthBytes * m_bm2ndLayerBitmap.bmHeight);
                // GET PHOTO BITMAP DATA
                m_2ndLayer1280x960Bitmap.GetBitmapBits(m_bm2ndLayerBitmap.bmWidthBytes * m_bm2ndLayerBitmap.bmHeight, m_p2ndLayerBitmapBits);
                // 2nd LAYER RGB COLOR POINT
                m_p2ndLayerRgb = m_p2ndLayerBitmapBits;
                // Layer Background Color
                Fill2ndLayer1280x960Bitmap(0, 0, m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth, m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight, RGB(0, 0, 0), TRUE);

                // SEM SE PHOTO SCAN IMAGE DATA UPDATE ( DISPLAY ) TIMER 
                SetTimer(TIMER_ID_SE_PHOTO345_SCAN_IMAGE_DISPLAY, SE_PHOTO345_SCAN_IMAGE_UPDATE_ELAPSE_TIME, NULL);

            }
            // SCAN MODE : PHOTO 6 --> 2nd Bitmap Image Draw
            else if (nScanMode == PHOTO6_SCAN_MODE)
            {
                // Capture Start Flag
                m_SemSEScanControlPublicData.m_nSEFrameCount        = 0;                 // SE SCAN IMAGE FRAME COUNT
                m_SemSEScanControlPublicData.m_nSEAverageNumber     = nBitmapAverage;    // BITMAP AGERAGE 설정 ( RED:6, FAST:6, SLOW1:1, SLOW2:1 )
                m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth  = nScanWidth;        // X-WIDTH
                m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight = nScanHeight;       // Y-HEIGHT
                m_SemSEScanControlPublicData.m_bSEImageCaptureStart = TRUE;              // SE SCAN IMAGE CAPTURE 시작됨
                m_SemSEScanControlPublicData.m_nSEDrawXPositionB    = 0;                 // SE SCAN IMAGE X Draw Position : FRAME B
                m_SemSEScanControlPublicData.m_nSEDrawYPositionB    = 0;                 // SE SCAN IMAGE Y Draw Position : FRAME B
                m_SemSEScanControlPublicData.m_nSEBitmapXOffsetB    = nBitmapXOffset;    // SE SCAN IMAGE BITMAP X Offset
                m_SemSEScanControlPublicData.m_nSEBitmapYOffsetB    = nBitmapYOffset;    // SE SCAN IMAGE BITMAP Y Offset
                m_SemSEScanControlPublicData.m_nSemSEScanModeValue  = nScanMode;         // RED, FAST, SLOW1, SLOW2, PHOTO1~6
                m_SemSEScanControlPublicData.m_bPhotoScanCompleted  = FALSE;             // PHOTO SCAN IMAGE 1-FRAME COMPLETED

                //
                // SEM PHOTO SCAN IMAGE BITMAP DATA
                //
                m_bm2ndBitmap = m_2ndImage2560x1920Bitmap.Info.Clone();
                // BITMAP DATA 삭제
                if (m_p2ndBitmapBits != null)
                {
                    m_p2ndBitmapBits = null;
                }
                if (m_p2ndOrgBitmapBits != null)
                {
                    m_p2ndOrgBitmapBits = null;
                }
                // 2nd PHOTO LAYER
                m_nLayerPhotoMode = 3; // PHOTO6 SCAN MODE ( 2560x1920 )
                // 2nd BITMAP ARRAY CREATE
                m_p2ndBitmapBits    = new RGBQUAD[(m_bm2ndBitmap.bmWidthBytes * m_bm2ndBitmap.bmHeight) / 4]; // 실사용
                m_p2ndOrgBitmapBits = new RGBQUAD[(m_bm2ndBitmap.bmWidthBytes * m_bm2ndBitmap.bmHeight) / 4]; // 원본 저장용
                // 2nd BITMAP ARRAY CLEAR
                memset(m_p2ndBitmapBits   , 0x00, m_bm2ndBitmap.bmWidthBytes * m_bm2ndBitmap.bmHeight);
                memset(m_p2ndOrgBitmapBits, 0x00, m_bm2ndBitmap.bmWidthBytes * m_bm2ndBitmap.bmHeight);
                // GET PHOTO BITMAP DATA
                m_2ndImage2560x1920Bitmap.GetBitmapBits(m_bm2ndBitmap.bmWidthBytes * m_bm2ndBitmap.bmHeight, m_p2ndBitmapBits);
                // 2nd RGB COLOR POINT
                m_p2ndImageRgb = m_p2ndBitmapBits;
                // Background Color
                Fill2ndImage2560x1920Bitmap(0, 0, m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth, m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight, DISPLAY_2ND_IMAGE_BACKGROUND_COLOR, TRUE);

                //
                // PHOTO LAYER IMAGE BITMAP DATA
                //
                m_bm2ndLayerBitmap = m_2ndLayer2560x1920Bitmap.Info.Clone();
                if (m_p2ndLayerBitmapBits != null)
                {
                    m_p2ndLayerBitmapBits = null;
                }
                // 2nd LAYER BITMAP ARRAY CREATE
                m_p2ndLayerBitmapBits = new RGBQUAD[(m_bm2ndLayerBitmap.bmWidthBytes * m_bm2ndLayerBitmap.bmHeight) / 4];
                // 2nd LAYER BITMAP ARRAY CLEAR
                memset(m_p2ndLayerBitmapBits, 0x00, m_bm2ndLayerBitmap.bmWidthBytes * m_bm2ndLayerBitmap.bmHeight);
                // GET PHOTO BITMAP DATA
                m_2ndLayer2560x1920Bitmap.GetBitmapBits(m_bm2ndLayerBitmap.bmWidthBytes * m_bm2ndLayerBitmap.bmHeight, m_p2ndLayerBitmapBits);
                // 2nd LAYER RGB COLOR POINT
                m_p2ndLayerRgb = m_p2ndLayerBitmapBits;
                // 2020.06.15
                Fill2ndLayer2560x1920Bitmap(0, 0, DISP_2ND_IMAGE_WINDOW_WIDTH, DISP_2ND_IMAGE_WINDOW_HEIGHT, RGB(0, 0, 0), TRUE);
                // FULL SIZE IMAGE SCAN FLAG
                m_n2ndFullSizeImageScan = 1;

                // NORMAL SIZE IMAGE DISPLAY
                if (m_nNormalFullImageSizeFlag == 0)
                {
                    //
                    // PHOTO LAYER IMAGE BITMAP DATA
                    //
                    m_bm2ndLayerBitmap = m_2ndLayer2560x1920Bitmap.Info.Clone();
                    if (m_p2ndLayerBitmapBits != null)
                    {
                        m_p2ndLayerBitmapBits = null;
                    }
                    // 2nd LAYER BITMAP ARRAY CREATE
                    m_p2ndLayerBitmapBits = new RGBQUAD[(m_bm2ndLayerBitmap.bmWidthBytes * m_bm2ndLayerBitmap.bmHeight) / 4];
                    // 2nd LAYER BITMAP ARRAY CLEAR
                    memset(m_p2ndLayerBitmapBits, 0x00, m_bm2ndLayerBitmap.bmWidthBytes * m_bm2ndLayerBitmap.bmHeight);
                    // GET PHOTO BITMAP DATA
                    m_2ndLayer2560x1920Bitmap.GetBitmapBits(m_bm2ndLayerBitmap.bmWidthBytes * m_bm2ndLayerBitmap.bmHeight, m_p2ndLayerBitmapBits);
                    // 2nd LAYER RGB COLOR POINT
                    m_p2ndLayerRgb = m_p2ndLayerBitmapBits;
                    // 2020.06.15
                    Fill2ndLayer2560x1920Bitmap(0, 0, DISP_2ND_IMAGE_WINDOW_WIDTH, DISP_2ND_IMAGE_WINDOW_HEIGHT, RGB(0, 0, 0), TRUE);
                }
                // FULL SIZE IMAGE DISPLAY
                else
                {
                    //
                    // PHOTO LAYER IMAGE BITMAP DATA
                    //
                    m_bm2ndLayerBitmap = m_2ndFullSizeLayer2560x1920Bitmap.Info.Clone();
                    if (m_p2ndLayerBitmapBits != null)
                    {
                        m_p2ndLayerBitmapBits = null;
                    }
                    // 2nd LAYER BITMAP ARRAY CREATE
                    m_p2ndLayerBitmapBits = new RGBQUAD[(m_bm2ndLayerBitmap.bmWidthBytes * m_bm2ndLayerBitmap.bmHeight) / 4];
                    // 2nd LAYER BITMAP ARRAY CLEAR
                    memset(m_p2ndLayerBitmapBits, 0x00, m_bm2ndLayerBitmap.bmWidthBytes * m_bm2ndLayerBitmap.bmHeight);
                    // GET PHOTO BITMAP DATA
                    m_2ndFullSizeLayer2560x1920Bitmap.GetBitmapBits(m_bm2ndLayerBitmap.bmWidthBytes * m_bm2ndLayerBitmap.bmHeight, m_p2ndLayerBitmapBits);
                    // 2nd LAYER RGB COLOR POINT
                    m_p2ndLayerRgb = m_p2ndLayerBitmapBits;
                    // 2020.06.15
                    Fill2ndLayer2560x1920Bitmap(0, 0, DISP_2ND_IMAGE_WINDOW_WIDTH, DISP_2ND_IMAGE_WINDOW_HEIGHT, RGB(0, 0, 0), TRUE);
                }
                // SEM SE PHOTO SCAN IMAGE DATA UPDATE ( DISPLAY ) TIMER 
                SetTimer(TIMER_ID_SE_PHOTO6_SCAN_IMAGE_DISPLAY, SE_PHOTO6_SCAN_IMAGE_UPDATE_ELAPSE_TIME, NULL);
            }

            //
            // SEM SE IMAGE SCAN : IMAGE CAPTURE START
            //
            if (bScanGeneration == TRUE)
            {
                // SEM SE IMAGE SCAN START : SCAN GENERATION SETTING
                m_pAssistDeviceTool.SetSEMScanGenerationValueUsbDeviceData(nScanMode);
                //Sleep(100);

                // SEM SE IMAGE SCAN START : IMAGE CAPTURE START
                m_pAssistDeviceTool.SetSEMImageCaptureOnOffValueUsbDeviceData(1, nScanMode);
            }

            // 2021.03.05
            // 배율설정 재실행
            m_wndScanControlDlg.SetMagnificationUpDown(0);

            // 2021.03.05
            // SCAN CALIBRATION DIALOG : SE SCAN MODE DATA UPDATE
            m_wndScanCalibrationToolDlg.SemSEScanModeDataUpdateControl(nScanMode);

            //
            // CL LMH MODE
            //
            
            int nCLLMHModeUse = 0;
            int nCLLMHModeSel = -1;
            int nCLLMHMode123 = -1;
            if (AfxGetIntegratedPointer() != null)
            {
                nCLLMHModeUse = AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MODE_USE_FLAG];
                nCLLMHModeSel = AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MODE_EXEC_SELECT];

                if (nCLLMHModeSel == IDX_CL_LMH_LOW_MODE)
                {
                    // LOW MODE
                    nCLLMHMode123 = AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW_EXEC_SELECT];
                }
                else if (nCLLMHModeSel == IDX_CL_LMH_MIDDLE_MODE)
                {
                    // MIDDLE MODE
                    nCLLMHMode123 = AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE_EXEC_SELECT];
                }
                else if (nCLLMHModeSel == IDX_CL_LMH_HIGH_MODE)
                {
                    // HIGH MODE
                    nCLLMHMode123 = AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH_EXEC_SELECT];
                }
            }
            //
            
            // CL LMH MODE 사용안함
            if (nCLLMHModeUse == 0)
            {
                // CL 1/2 ON 상태일 때 만 실행
                if (m_bCLOnOffFlag == TRUE)
                {
                    // CL 1/2 ON 상태
                    // 2021.03.29
                    // CL1,2 재실행
                    int nCL1 = m_dev.AfxIntegratedGetCL1();
                    int nCL2 = m_dev.AfxIntegratedGetCL2();

                    nCL1 = m_wndScanCalibrationToolDlg.GetCurrentCL1Value(); // 현재 CL1값
                    nCL2 = m_wndScanCalibrationToolDlg.GetCurrentCL2Value(); // 현재 CL2값

                    m_dev.AfxIntegratedSetCL1(nCL1);                               // CL1값 적용
                    m_dev.AfxIntegratedSetCL2(nCL2);                               // CL2값 적용

                    m_dev.AfxIntegratedSetCL1();                                   // CL1값 실행
                    m_dev.AfxIntegratedSetCL2();                                   // CL2값 실행
                }
            }
            // CL LMH MODE 사용함
            else
            {
                // CL 1/2 ON 상태일 때 만 실행
                if (m_bCLOnOffFlag == TRUE)
                {
                    // CL 1/2 ON 상태
                    
                    // CL LMH MODE : LOW, MIDDLE, HIGH
                    if (nCLLMHModeSel == IDX_CL_LMH_LOW_MODE ||
                        nCLLMHModeSel == IDX_CL_LMH_MIDDLE_MODE ||
                        nCLLMHModeSel == IDX_CL_LMH_HIGH_MODE)
                    {
                        if (nCLLMHMode123 >= 0)
                        {
                            // CL LMH MODE : CL, GUN ALIGN, BEAM ALIGN USB 적용
                            m_pAssistDeviceTool.ExecuteCLLMHModeData(nCLLMHModeSel, nCLLMHMode123);
                        }
                    }
                }
            }
            
            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:10158 ----
        // CLIENT --> DEVICE : SET USB DEVICE DATA ( SEM SCAN MODE SELECT & SCAN START --> SLOW1/SLOW2 전용 )
        public int SemSlow12ScanModeSelectAndScanStart(int nScanMode, int nScanWidth, int nScanHeight, int nBitmapXOffset, int nBitmapYOffset, int nBitmapAverage)
        {
            //
            // SLOW1 <---> SLOW2 SEM SCAN START 설정
            //
            // SCAN MODE : SLOW1 <---> SLOW2 1st Bitmap Image Draw
            if (nScanMode == SLOW1_SCAN_MODE || nScanMode == SLOW2_SCAN_MODE)
            {
                m_SemSEScanControlPublicData.m_nSemSEScanModeValue = nScanMode;         // RED, FAST, SLOW1, SLOW2, PHOTO1~6
                m_SemSEScanControlPublicData.m_nSaveScanMode       = nScanMode;         // RED, FAST, SLOW1, SLOW2, PHOTO1~6
            }

            // SCAN CALIBRATION DIALOG : SE SCAN MODE DATA UPDATE
            m_wndScanCalibrationToolDlg.SemSEScanModeDataUpdateControl(nScanMode);

            //
            // SEM SE IMAGE SCAN : SLOW1 <---> SLOW2 IMAGE CAPTURE START
            //
            // SEM SE SLOW1/2 IMAGE SCAN START : SCAN GENERATION SETTING
            m_pAssistDeviceTool.SetSEMScanGenerationValueUsbDeviceData(nScanMode);

            // SEM SE SLOW1/2 IMAGE SCAN START : IMAGE CAPTURE START
            m_pAssistDeviceTool.SetSEMSlow12ImageCaptureOnUsbDeviceData(nScanMode);

            return 1;
        }

    }
}
