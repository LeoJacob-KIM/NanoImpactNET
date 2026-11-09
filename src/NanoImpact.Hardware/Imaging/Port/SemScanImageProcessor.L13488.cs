// 원본 NanoImpact2020cfeDlg.cpp:13488-16142 을 NanoImpact.CodeGen translate로 옮긴 뒤 손으로 고친 코드.
#pragma warning disable IDE1006, IDE0059, CS0162, CS0168, CS0219
using System;
using NanoImpact.Core.Native;
using static NanoImpact.Core.Def;
using static NanoImpact.Core.Native.CRuntime;

namespace NanoImpact.Hardware.Imaging
{
    public sealed partial class SemScanImageProcessor
    {
        // ---- NanoImpact2020cfeDlg.cpp:13488 ----
        //
        // USB SEM SE SCAN IMAGE DATA RECEIVE PROCESS
        //
        //
        // SEM SE SCAN IMAGE DATA
        public int OnSemSEScanImageData(int wParam, object lParam)
        {
            // LONG DATA ONE(한번에) 커맨드 처리
            if (wParam == NULL) return 0;
            if (lParam == null) return 0;
            if (m_pSemSEScanImageReadBuffer == null) return 0;

            // SEM SE SCAN IMAGE DATA 정보
            int nScanImageDataSize = (int)wParam;

            m_nReceiveTotalImageCount += nScanImageDataSize;
            //
            // SEM SE SCAN IMAGE DATA READ BUFFER : IMAGE DATA 저장
            //
            // SEM SE SCAN IMAGE READ BUFFER : 이미지 쓰레드 처리를 위한 임시 버퍼

            // CHECK
            if (m_nSemSEScanImageSaveCount + nScanImageDataSize > m_nSemSEScanImageDataMax - 1)
            {
                // 저장위치가 버퍼크기보다 크면 : 저장위치 초기화
                m_nSemSEScanImageSaveStart = 0;
                m_nSemSEScanImageSaveCount = nScanImageDataSize;
            }

            //

            // CLEAR
            //memset(m_pSemSEScanImageReadBuffer+m_nSemSEScanImageSaveStart, 0x00, nScanImageDataSize);
            // COPY
            memcpy(m_pSemSEScanImageReadBuffer, m_nSemSEScanImageSaveStart, (byte[])lParam, nScanImageDataSize);
            // TOTAL DATA COUNT
            m_nSemSEScanImageSaveCount += nScanImageDataSize;
            // NEXT SAVE POSITION
            m_nSemSEScanImageSaveStart += nScanImageDataSize;

            //

            uint dwOldDisplayFrameCount = m_dwDisplayFrameCount;

            // FRAME COUNT
            m_dwSecondTick = GetTickCount();
            if (m_dwSecondTick - m_dwFirstTick > 1000)
            {
                // RESET
                m_dwDisplayFrameCount  = m_dwFrameDataCount;
                m_dwDisplayUpdateCount = m_dwFrameUpdateCount;
                m_dwFrameDataCount     = (uint)nScanImageDataSize;
                m_dwFirstTick          = m_dwSecondTick;
                m_dwFrameUpdateCount   = 0;
            }
            else
            {
                // FRAME DATA COUNT
                m_dwFrameDataCount += (uint)nScanImageDataSize;
                m_dwFrameUpdateCount++;
            }

            //

            // SEM SE SCAN IMAGE DATA 처리
            m_nReceiveDataCount++;
            string strText = "";

            //strText.Format(_T("%d"), m_nReceiveDataCount);
            //m_wndDetailControlDlg.SetClientInfomation01Text(strText);
            //strText.Format(_T("%d"), m_nSemSEScanImageSaveCount);
            //m_wndDetailControlDlg.SetClientInfomation03Text(strText);

            if (dwOldDisplayFrameCount != m_dwDisplayFrameCount)
            {
                if      (m_SemSEScanControlPublicData.m_nSemSEScanModeValue == RED_SCAN_MODE   ) strText = "RED";
                else if (m_SemSEScanControlPublicData.m_nSemSEScanModeValue == FAST_SCAN_MODE  ) strText = "FAST";
                else if (m_SemSEScanControlPublicData.m_nSemSEScanModeValue == SLOW1_SCAN_MODE ) strText = "SLOW1";
                else if (m_SemSEScanControlPublicData.m_nSemSEScanModeValue == SLOW2_SCAN_MODE ) strText = "SLOW2";
                else if (m_SemSEScanControlPublicData.m_nSemSEScanModeValue == PHOTO1_SCAN_MODE) strText = "PHOTO1";
                else if (m_SemSEScanControlPublicData.m_nSemSEScanModeValue == PHOTO2_SCAN_MODE) strText = "PHOTO2";
                else if (m_SemSEScanControlPublicData.m_nSemSEScanModeValue == PHOTO3_SCAN_MODE) strText = "OVERLAP";
                else if (m_SemSEScanControlPublicData.m_nSemSEScanModeValue == PHOTO4_SCAN_MODE) strText = "PHOTO4";
                else if (m_SemSEScanControlPublicData.m_nSemSEScanModeValue == PHOTO5_SCAN_MODE) strText = "PHOTO5";
                else if (m_SemSEScanControlPublicData.m_nSemSEScanModeValue == PHOTO6_SCAN_MODE) strText = "PHOTO6";
                else strText = "NONE";
                m_wndDetailControlDlg.SetClientInfomation01Text(strText);
                strText = Format("%d[KB]", m_dwDisplayFrameCount / 1024);
                m_wndDetailControlDlg.SetClientInfomation02Text(strText);
                strText = Format("%d", m_nSemSEScanImageSaveCount);
                m_wndDetailControlDlg.SetClientInfomation03Text(strText);
            }

            //strText.Format(_T("%d[KB]"), m_dwDisplayFrameCount / 1024);
            //m_wndDetailControlDlg.SetClientInfomation02Text(strText);
            //strText.Format(_T("%9d"), m_dwDisplayUpdateCount);
            //m_wndDetailControlDlg.SetClientInfomation04Text(strText);// m_ClientInfoText4.SetWindowTextW(strText);
            //strText.Format(_T("%9d"), m_nReceiveTotalImageCount);
            //m_wndDetailControlDlg.SetClientInfomation03Text(strText);// m_ClientInfoText3.SetWindowTextW(strText);


            //

            // SE Scan Image Data 프로세싱
            // 2019.09.18 이미지 프로세싱은 타이머를 이용하여 정기적으로 실행한다 (인터벌:50[ms]->20[frame])
            //SemSEScanImageDataUpdateProcessing();

            if (m_SemSEScanControlPublicData.m_nSemSEScanModeValue == RED_SCAN_MODE || m_SemSEScanControlPublicData.m_nSemSEScanModeValue == FAST_SCAN_MODE || m_SemSEScanControlPublicData.m_nSemSEScanModeValue == SLOW1_SCAN_MODE || m_SemSEScanControlPublicData.m_nSemSEScanModeValue == SLOW2_SCAN_MODE)
            {
                // SE Scan Image Data 프로세싱
                // 2020.05.01 이미지 프로세싱은 이미지 데이터 이벤트 발생시 실행한다
                SemSEScanImageDataUpdateProcessing();
            }
            else
            {
                // PHOTO1,2 SCAN MODE
                if (m_SemSEScanControlPublicData.m_nPhotoScanMode == PHOTO1_SCAN_MODE || m_SemSEScanControlPublicData.m_nPhotoScanMode == PHOTO2_SCAN_MODE)
                {
                    SemSEPhotoScan12ImageDataUpdateProcessing();
                }
                // PHOTO3,4,5 SCAN MODE
                else if (m_SemSEScanControlPublicData.m_nPhotoScanMode == PHOTO4_SCAN_MODE || m_SemSEScanControlPublicData.m_nPhotoScanMode == PHOTO5_SCAN_MODE)
                {
                    SemSEPhotoScan345ImageDataUpdateProcessing();
                }
                // PHOTO6 SCAN MODE
                else if (m_SemSEScanControlPublicData.m_nPhotoScanMode == PHOTO6_SCAN_MODE)
                {
                    SemSEPhotoScan6ImageDataUpdateProcessing();
                }
                // PHOTO3 (PHOTO OVERLAP MODE) SCAN MODE
                else if (m_SemSEScanControlPublicData.m_nPhotoScanMode == PHOTO3_SCAN_MODE)
                {
                    SemSEPhotoScanOverlapModeImageDataUpdateProcessing();
                }
            }

            //

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:13629 ----
        //
        // SEM SE Scan Image Data : 수신된 데이터 이미지 업데이트 ( DISPLAY ) 처리
        //
        public uint SemSEScanImageDataUpdateProcessing()
        {
            string strText = "";
            int   n = 0;
            int   k = 0;
            int   nReadPointer = 0;
            int   nAverPointer = 0;
            byte  btData       = 0;
            int   nDataSize    = 0;
            uint dwBytesRead  = 0;
            byte  btRED        = 0;
            byte  btGREEN      = 0;
            byte  btBLUE       = 0;
            int   nAverIndex   = 0;
            int   nTotalValue  = 0;
            int   nAverValue   = 0;
            int  bImageUpdate = FALSE;

            //DWORD dwFirstTick = GetTickCount();
            //DWORD dwLastTick  = dwFirstTick;

            //
            // SE Image Data 프로세싱
            //

            // 처리할 DATA 크기 확인
            if (m_nSemSEScanImageSaveCount < 1)
            {
                // 저장버퍼 : 초기화
                m_nSemSEScanImageSaveStart = 0; // 저장버퍼 현재위치
                m_nSemSEScanImageSaveCount = 0; // 저장버퍼 현재크기

                return 0;
            }

            // 처리할 DATA 크기 설정 : 저장된 DATA 모두 처리
            dwBytesRead = (uint)m_nSemSEScanImageSaveCount;

            // 처리할 SEM SE PHOTO SCAN IMAGE DATA 복사
            memcpy(m_pSemSEScanImageThreadBuffer, m_pSemSEScanImageReadBuffer, dwBytesRead);

            // 저장버퍼 CLEAR
            memset(m_pSemSEScanImageReadBuffer, 0x00, dwBytesRead);// m_nSemSEScanImageDataMax);
            // 저장버퍼 : 초기화
            m_nSemSEScanImageSaveStart = 0; // 저장버퍼 현재위치
            m_nSemSEScanImageSaveCount = 0; // 저장버퍼 현재크기


            //
            //
            //

            // SE SCAN IMAGE FRAME 구성
            //                                       1 FRAME, WIDTH 640, HEIGHT 480  일 경우  
            // <------------------------------------------------------------------------------------------------------------------->
            // 0xFF,AB,AC,<-------------------------------------------IMAGE DATA--------------------------------------------------->
            // <0x00,<------------------------------------------------IMAGE DATA--------------------------------------------------->
            // <0x00,<------------------------------------------------IMAGE DATA--------------------------------------------------->
            // LINE COUNT : 479 까지

            // 프레임 구성 코드
            // 0xFF --> FRAME START   CODE : 프레임 시작을 알리는 코드           <----- 프레임 갱신시 발생
            // AB   --> AUTO BRIGHT   CODE : AUTO BRIGHTNESS 처리를 위한 코드    <----- 프레임 갱신시 발생
            // AC   --> AUTO CONTRAST CODE : AUTO CONTRAST   처리를 위한 코드    <----- 프레임 갱신시 발생
            // 0x00 --> LINE START    CODE : LINE의 시작을 알리는 코드           <----- 라인   갱신시 발생  

            // 프레임 시작 코드 0xFF 가 발생될 때, AUTO BRIGHT, CONTRAST 코드를 함께 발생 시키는 경우
            // 0xFF 코드와 AB, AC 코드는 다음과 같이 구성하여 들어온다
            // 1) 프레임 시작 코드만 들어온 경우 : DATA COUNT --> 1             , 코드구성 --> 0xFF
            //                    NEXT DATA 구성 : DATA COUNT --> 2048 ~ 65536  , 코드구성 --> AB,AC,IMAGE DATA ~~~~~~~~~~~~~~~~~~~~
            // 2) 0xFF 및 AB 코드가  들어온 경우 : DATA COUNT --> 2             , 코드구성 --> 0xFF,AB
            //                    NEXT DATA 구성 : DATA COUNT --> 2048 ~ 65536  , 코드구성 --> AC,IMAGE DATA ~~~~~~~~~~~~~~~~~~~~~~~

            // 프레임 코드 및 Auto Bright, Contrast 코드로 구성되어 이미지 데이터가 들어온 경우
            // 0xFF 코드와 AB, AC, 0x00 코드, IMAGE DATA 코드로 구성되어 들어 온다
            // 1) 프레임 시작 코드가 들어온 경우 : DATA COUNT --> 2048 ~ 65536  , 코드구성 --> 0xFF,AB,AC,IMAGE DATA ~~~~~~~~~~~~~~~
            //       (LINE START) NEXT DATA 구성 : DATA COUNT --> 2048 ~ 65536  , 코드구성 --> 0x00,IMAGE DATA ~~~~~~~~~~~~~~~~~~~~~
            // 2) 라인   시작 코드가 들어온 경우 : DATA COUNT --> 2048 ~ 65536  , 코드구성 --> 0x00,IMAGE DATA ~~~~~~~~~~~~~~~~~~~~~

            //
            //
            //

            //m_nSEAutoBrightContrastCount = 0;   // SEM SE Frame IMAGE DATA 의 Auto Bright, Contrast Code 갯수
            //m_nSEAutoBrightValue   = 0;         // SEM SE Frame IMAGE DATA 의 Auto Bright   값
            //m_nSEAutoContrastValue = 0;         // SEM SE Frame IMAGE DATA 의 Auto Contrast 값

            n = 0;                                  // Scan Image 처리 시작 위치 재설정


            //
            // Auto Bright, Auto Contrast 값을 처리한다
            //
            if (m_SemSEScanControlPublicData.m_nSEAutoBrightContrastCount == 1)
            {
                // FRAME START CODE (0xFF) 처리시 Auto Bright Code를 함께 처리하였기 때문에 여기선 Auto Contrast 처리만 한다
                // Auto Bright, Contrast 처리를 위한 남은 Data 갯수 : 1개, Auto Contrast 값을 처리한다

                // Auto Contrast 값
                m_SemSEScanControlPublicData.m_nSEAutoContrastValue = m_pSemSEScanImageThreadBuffer[0];

                // Scan Image 처리 시작 위치 재설정
                n = 1;

                // Auto Bright, Contrast 남은 Data Count Reset
                m_SemSEScanControlPublicData.m_nSEAutoBrightContrastCount = 0;

            }
            else if (m_SemSEScanControlPublicData.m_nSEAutoBrightContrastCount == 2)
            {
                // FRAME START CODE (0xFF) 처리시 Auto Bright, Contrast Code를 처리 하지 않았기 때문에 여기서 Auto Bright, Contrast 를 모두 처리 한다
                // Auto Bright, Contrast 처리를 위한 남은 Data 갯수 : 2개, Auto Bright, Contrast 값을 처리한다

                // Auto Bright 값
                m_SemSEScanControlPublicData.m_nSEAutoBrightValue = m_pSemSEScanImageThreadBuffer[0];
                // Auto Bright, Contrast 남은 Data Count Reset
                m_SemSEScanControlPublicData.m_nSEAutoBrightContrastCount -= 1;
                // Scan Image 처리 시작 위치 재설정
                n = 1;

                // Data 갯수 확인
                if (dwBytesRead > 1)
                {
                    // Auto Contrast 값
                    m_SemSEScanControlPublicData.m_nSEAutoContrastValue = m_pSemSEScanImageThreadBuffer[1];
                    // Auto Bright, Contrast 남은 Data Count Reset
                    m_SemSEScanControlPublicData.m_nSEAutoBrightContrastCount = 0;
                    // Scan Image 처리 시작 위치 재설정
                    n = 2;
                }

            }


            for (; n < (int)dwBytesRead; n++)
            {
                if (m_pSemSEScanImageThreadBuffer[n] == 0xFF)
                {
                    // FRAME START
                    m_SemSEScanControlPublicData.m_nSEDrawXPositionA = 0;       // SE SCAN IMAGE BUFFER : X Draw Position
                    m_SemSEScanControlPublicData.m_nSEDrawYPositionA = 0;       // SE SCAN IMAGE BUFFER : Y Draw Position
                    m_bSemSEFrameFirstCatch = TRUE;                             // SE SCAN IMAGE READ BUFFER : FRAME 처음 읽기 처리 완료

                    // FRAME COUNT
                    m_SemSEScanControlPublicData.m_nSEFrameCount++;

                    // BRIGHTNESS LEVEL 값 계산 (FRAME 단위)
                    if (m_SemSEScanControlPublicData.m_nSEPixelLevelCount > 0)
                    {
                        m_SemSEScanControlPublicData.m_nSEFrameBrightLevel = m_SemSEScanControlPublicData.m_nSEPixelLevelSum / m_SemSEScanControlPublicData.m_nSEPixelLevelCount;
                    }
                    else
                    {
                        m_SemSEScanControlPublicData.m_nSEFrameBrightLevel = 0;
                    }
                    strText = Format("%d", m_SemSEScanControlPublicData.m_nSEFrameBrightLevel);
                    m_wndDetailControlDlg.SetClientInfomation05Text(strText);
                    m_SemSEScanControlPublicData.m_nSEPixelLevelSum    = 0;
                    m_SemSEScanControlPublicData.m_nSEPixelLevelCount  = 0;

                    // BITMAP IMAGE WEIGHT AVERAGE COUNT
                    m_SemSEScanControlPublicData.m_nSEAverageCount++;
                    m_SemSEScanControlPublicData.m_nSEAverageIndex = m_SemSEScanControlPublicData.m_nSEAverageCount - 1;
                    if (m_SemSEScanControlPublicData.m_nSEAverageCount > m_SemSEScanControlPublicData.m_nSEAverageNumber)
                    {
                        // Weight Average Count Check ?
                        m_SemSEScanControlPublicData.m_nSEAverageCount = 1;
                        m_SemSEScanControlPublicData.m_nSEAverageIndex = 0;
                    }

                    // 처리 해야할 Scan Image Data 갯수 확인
                    if (dwBytesRead == 1)
                    {
                        // Scan Image Data 갯수 : 1개
                        // 처리 해야할 Auto Bright, Contrast Data 갯수 설정
                        m_SemSEScanControlPublicData.m_nSEAutoBrightContrastCount = 2; // 2개 (Auto Bright, Contrast )

                    }
                    else if (dwBytesRead == 2)
                    {
                        // Scan Image Data 갯수 : 2개
                        // Auto Bright 값
                        m_SemSEScanControlPublicData.m_nSEAutoBrightValue = m_pSemSEScanImageThreadBuffer[n + 1];
                        // 처리 해야할 Auto Bright, Contrast Data 갯수 설정
                        m_SemSEScanControlPublicData.m_nSEAutoBrightContrastCount = 1; // 1개 (Auto Contrast )
                        // Scan Image 처리 시작 위치 재설정
                        n += 1;

                    }
                    else
                    {
                        // Scan Image Data 갯수 : 2048 ~ 65536개
                        // Auto Bright 값
                        m_SemSEScanControlPublicData.m_nSEAutoBrightValue   = m_pSemSEScanImageThreadBuffer[n + 1];
                        // Auto Contrast 값
                        m_SemSEScanControlPublicData.m_nSEAutoContrastValue = m_pSemSEScanImageThreadBuffer[n + 2];
                        // 처리 해야할 Auto Bright, Contrast Data 갯수 설정
                        m_SemSEScanControlPublicData.m_nSEAutoBrightContrastCount = 0; // 0개
                        // Scan Image 처리 시작 위치 재설정
                        n += 2;

                    }

                    // SCAN IMAGE UPDATE (DISPLAY)
                    bImageUpdate = TRUE;

                }
                //else if (m_pSemSEScanImageThreadBuffer[n] > 0xFB && m_pSemSEScanImageThreadBuffer[n] < 0xFF)
                //{
                //    // 0xFC : Auto Contrast Value
                //    // 0xFD : Auto Contrast, Brightness Value
                //    // 0xFE : DUAL SCREEN MODE : B Frame Start
                //    continue;
                //}
                else if (m_pSemSEScanImageThreadBuffer[n] == 0x00)    // Line Start Code
                {
                    // 2020.05.01
                    if (m_bSemSEFrameFirstCatch == FALSE) continue;

                    // SCAN IMAGE UPDATE (DISPLAY)
                    //if (m_SemSEScanControlPublicData.m_nSemSEScanModeValue != RED_SCAN_MODE) bImageUpdate = TRUE;

                    //
                    // TEST
                    //
                    if (m_SemSEScanControlPublicData.m_nSemSEScanModeValue == RED_SCAN_MODE)
                    {
                        if (m_SemSEScanControlPublicData.m_nSEDrawYPositionA == 239)
                        {
                            m_SemSEScanControlPublicData.m_nSEPixelCount++;
                            strText = Format("%d,%d", m_SemSEScanControlPublicData.m_nSEFrameCount, m_SemSEScanControlPublicData.m_nSEPixelCount);
                            m_wndDetailControlDlg.SetClientInfomation04Text(strText);// m_ClientInfoText4.SetWindowTextW(strText);
                        }
                    }
                    else if (m_SemSEScanControlPublicData.m_nSemSEScanModeValue == FAST_SCAN_MODE)
                    {
                        if (m_SemSEScanControlPublicData.m_nSEDrawYPositionA == 479)
                        {
                            m_SemSEScanControlPublicData.m_nSEPixelCount++;
                            strText = Format("%d,%d", m_SemSEScanControlPublicData.m_nSEFrameCount, m_SemSEScanControlPublicData.m_nSEPixelCount);
                            m_wndDetailControlDlg.SetClientInfomation04Text(strText);// m_ClientInfoText4.SetWindowTextW(strText);
                        }
                    }
                    else
                    {
                        bImageUpdate = TRUE; // IMAGE UPDATE DISPLAY
                        strText = Format("%d,%d", m_SemSEScanControlPublicData.m_nSEDrawXPositionA, m_SemSEScanControlPublicData.m_nSEDrawYPositionA);
                        m_wndDetailControlDlg.SetClientInfomation04Text(strText);// m_ClientInfoText4.SetWindowTextW(strText);
                    }
                    //

                    // LINE START : X POSITION RESET
                    m_SemSEScanControlPublicData.m_nSEDrawXPositionA = 0;
                    // LINE START : Y POSITION 증가 및 확인
                    if (m_SemSEScanControlPublicData.m_nSEDrawYPositionA < m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight - 1)
                    {
                        // FRAME : Y Position 증가
                        m_SemSEScanControlPublicData.m_nSEDrawYPositionA++;
                    }

                }
                else
                {
                    // FRAME 시작 문자를 읽기 전에는 모든 이미지 DATA를 버린다
                    if (m_bSemSEFrameFirstCatch == FALSE) continue;

                    // SCAN IMAGE UPDATE (DISPLAY)
                    //bImageUpdate = TRUE;
                    // 2021.05.10
                    // 이미지 업데이트 속도 개선을 위해 X/Y PIXEL 처리시에는 이미지를 갱신 하지 않는다
                    bImageUpdate = FALSE;
                    if (n == (int)(dwBytesRead-1))
                    {
                        bImageUpdate = TRUE;
                    }

                    // DRAW X POSITION 확인 ( 0 ~ 가로폭 미만)
                    if (m_SemSEScanControlPublicData.m_nSEDrawXPositionA < m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)
                    {
                        // SCAN IMAGE DATA ( 1 BYTE )
                        btData = m_pSemSEScanImageThreadBuffer[n];

                        // BRIGHTNESS LEVEL 값 계산을 위한 처리
                        m_SemSEScanControlPublicData.m_nSEPixelLevelSum += btData; // 이미지 PIXEL 값   합계 계산
                        m_SemSEScanControlPublicData.m_nSEPixelLevelCount++;       // 이미지 PIXEL 갯수 합계 계산


                        // PIXEL DATA BITMAP 위치 설정 계산
                        // BITMAP DATA 배열 순서 : 예) Average:16 일 경우 --> x[0]y[0]a[0],...x[0]y[0]a[15],x[1]y[0]a[0],...x[1]y[0]a[15],x[2]y[0]a[0],...x[2]y[0]a[15],.....x[639]y[479]a[15]
                        nAverPointer = ((m_SemSEScanControlPublicData.m_nSEDrawXPositionA + m_SemSEScanControlPublicData.m_nSEBitmapXOffsetA) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionA + m_SemSEScanControlPublicData.m_nSEBitmapYOffsetA) * STD_SE_SCAN_IMAGE_WIDTH)) * MAX_SE_SCAN_IMAGE_AVERAGE;
                        // 해당 위치의 이미지 값 적용
                        m_pSemSEScanImageBitmapBufferA[nAverPointer + m_SemSEScanControlPublicData.m_nSEAverageIndex] = btData;

                        // SCAN MODE : RED, FAST, SLOW1, SLOW2 -> WEIGHT AVERAGE BITMAP DATA ( BITMAP 이미지 평균값 보정 )
                        if (m_SemSEScanControlPublicData.m_nSEFrameCount > m_SemSEScanControlPublicData.m_nSEAverageNumber)
                        {
                            // 프레임 카운트가 스캔모드에 따른 AVERAGE 카운트를 넘어섰다
                            // BITMAP 이미지 평균값 구하기
                            nTotalValue = 0;
                            nAverValue  = 0;
                            for (nAverIndex = 0; nAverIndex < m_SemSEScanControlPublicData.m_nSEAverageNumber; nAverIndex++)
                            {
                                // BITMAP DATA 합산
                                nTotalValue += m_pSemSEScanImageBitmapBufferA[nAverPointer + nAverIndex];
                            }
                            // BITMAP DATA 평균값
                            nAverValue = nTotalValue / m_SemSEScanControlPublicData.m_nSEAverageNumber;
                            // 결과값 BYTE
                            btData = (byte)nAverValue;
                        }
                        else
                        {
                            // 프레임 카운트가 스캔모드에 따른 AVERAGE 카운트와 같거나 작다
                            // BITMAP 이미지 평균값 구하기
                            nTotalValue = 0;
                            nAverValue  = 0;
                            for (nAverIndex = 0; nAverIndex < m_SemSEScanControlPublicData.m_nSEAverageCount; nAverIndex++)
                            {
                                // BITMAP DATA 합산
                                nTotalValue += m_pSemSEScanImageBitmapBufferA[nAverPointer + nAverIndex];
                            }
                            // BITMAP DATA 평균값
                            nAverValue = nTotalValue / m_SemSEScanControlPublicData.m_nSEAverageCount;
                            // 결과값 BYTE
                            btData = (byte)nAverValue;
                        }


                        // SCAN IMAGE DATA PIXEL POSITION
                        k = (m_SemSEScanControlPublicData.m_nSEDrawXPositionA + m_SemSEScanControlPublicData.m_nSEBitmapXOffsetA) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionA + m_SemSEScanControlPublicData.m_nSEBitmapYOffsetA) * STD_SE_SCAN_IMAGE_WIDTH);// m_bm1stBitmap.bmWidth);

                        // PIXEL DATA SET
                        m_p1stImageRgb[k].rgbRed   = btData;
                        m_p1stImageRgb[k].rgbGreen = btData;
                        m_p1stImageRgb[k].rgbBlue  = btData;

                        // FRAME : X Position 증가
                        m_SemSEScanControlPublicData.m_nSEDrawXPositionA++;

                    }
                } // 식별문자 코드 처리 : 0xFF, 0xFE, 0xFD, 0xFC, 0x00, 이미지데이터, 등등 if()문 처리
            } // 수신한 문자 처리 for()문 처리

            // edit by SFA(wjYoo)
            // Live Image 완료
            if (IsRunImageGrab(GRAB_TYPE_LIVE) == TRUE)
            {
                SetImageGrabEnd(TRUE, m_p1stImageRgb);
            }
            //
            //
            //

            // FRAME 시작 문자를 읽기 전에는 모든 이미지 DATA를 버린다
            if (m_bSemSEFrameFirstCatch == FALSE) return RET_SUCCESS_OK;

            // SE SCAN IMAGE DATA DRAW UPDATE ( DISPLAY ) : 1 FRAME 단위 데이터 처리
            // 202020.01.15 이미지 업데이트(REDRAW)는 타이머를 이용하여 정기적으로 실행한다
            // BITMAP IMAGE UPDATE + LAYER OBJECT DRAW + IMAGE DC DRAW
            //SemSEScanImageDataUpdateControl();

            // 2020.05.04
            // SE SCAN IMAGE DATA DRAW UPDATE ( DISPLAY ) : 1 FRAME 단위 데이터 처리
            // 202020.05.04 이미지 업데이트(REDRAW)는 이미지 데이터 이벤트 발생시 실행한다
            // 또한 타이머를 이용하여 정기적으로 업데이트를 실행한다.(스캔멈춤시 LAYER DRAW를 위한 기능이다)
            // BITMAP IMAGE UPDATE + LAYER OBJECT DRAW + IMAGE DC DRAW
            // AUTO START PROCESS 또는 AUTO FOCUS, AUTO CONTRAST+BRIGHT 실행여부 확인
            if (m_SemSEScanControlPublicData.m_bAutoStartFlag == TRUE || m_nAutoProcessRunStatus == AUTO_PROCESS_RUN_STATE || bImageUpdate == FALSE)
            {
                // 이미지 업데이트 안함
                return RET_SUCCESS_OK;
            }
            if (bImageUpdate == TRUE)
            {
                // SCAN IMAGE UPDATE (DISPLAY)
                SemSEScanImageDataUpdateControl();
            }

            //
            //
            //

            return RET_SUCCESS_OK;
        }

        // ---- NanoImpact2020cfeDlg.cpp:14018 ----
        //
        // SEM SE Photo Scan 1/2 Image Data 640x480 : 수신된 데이터 이미지 업데이트 ( DISPLAY ) 처리
        //
        public uint SemSEPhotoScan12ImageDataUpdateProcessing()
        {
            string strText = "";
            int   n = 0;
            int   k = 0;
            int   nReadPointer = 0;
            int   nAverPointer = 0;
            byte  btData       = 0;
            int   nDataSize    = 0;
            uint dwBytesRead  = 0;
            byte  btRED        = 0;
            byte  btGREEN      = 0;
            byte  btBLUE       = 0;
            int   nAverIndex   = 0;
            int   nTotalValue  = 0;
            int   nAverValue   = 0;
            int  bImageUpdate = FALSE;
            int  bCompleted   = FALSE;

            //
            // SE Photo Scan Image Data 프로세싱
            //

            // 처리할 DATA 크기 확인
            if (m_nSemSEScanImageSaveCount < 1)
            {
                // 저장버퍼 : 초기화
                m_nSemSEScanImageSaveStart = 0; // 저장버퍼 현재위치
                m_nSemSEScanImageSaveCount = 0; // 저장버퍼 현재크기

                return 0;
            }

            // 처리할 DATA 크기 설정 : 저장된 DATA 모두 처리
            dwBytesRead = (uint)m_nSemSEScanImageSaveCount;

            // 처리할 SEM SE PHOTO SCAN IMAGE DATA 복사
            memcpy(m_pSemSEScanImageThreadBuffer, m_pSemSEScanImageReadBuffer, dwBytesRead);

            // 저장버퍼 CLEAR
            memset(m_pSemSEScanImageReadBuffer, 0x00, dwBytesRead);// m_nSemSEScanImageDataMax);
            // 저장버퍼 : 초기화
            m_nSemSEScanImageSaveStart = 0; // 저장버퍼 현재위치
            m_nSemSEScanImageSaveCount = 0; // 저장버퍼 현재크기

            //
            //
            //

            for (n = 0; n < (int)dwBytesRead; n++)
            {
                if (m_pSemSEScanImageThreadBuffer[n] == 0xFF)
                {
                    // FRAME START
                    m_SemSEScanControlPublicData.m_nSEDrawXPositionB = 0;       // SE PHOTO SCAN IMAGE BUFFER : X Draw Position
                    m_SemSEScanControlPublicData.m_nSEDrawYPositionB = 0;       // SE PHOTO SCAN IMAGE BUFFER : Y Draw Position
                    m_bSemSEFrameFirstCatch = TRUE;                             // SE PHOTO SCAN IMAGE READ BUFFER : FRAME 처음 읽기 처리 완료

                    // SCAN IMAGE UPDATE (DISPLAY)
                    bImageUpdate = TRUE;

                    // FRAME COUNT
                    m_SemSEScanControlPublicData.m_nSEFrameCount++;
                    if (m_SemSEScanControlPublicData.m_nSEFrameCount > 1)
                    {
                        // PHOTO SCAN CALIBRATION MODE FLAG CHECK?
                        if (m_bPhotoCalibrationFlag == FALSE)
                        {
                            // PHOTO MODE 스캔은 1 프레임만 스캔한 후, 이전 스캔모드로 돌아간다
                            bCompleted = TRUE;
                            // for문 종료
                            break;
                        }
                        else
                        {
                            // PHOTO CALIBRATION MODE 스캔은 프레임을 연속 스캔한다.
                            bCompleted = FALSE;
                            // FRAME COUNT
                            m_SemSEScanControlPublicData.m_nSEFrameCount = 1;
                        }
                    }

                    // BITMAP IMAGE WEIGHT AVERAGE COUNT
                    m_SemSEScanControlPublicData.m_nSEAverageCount = 1;
                    m_SemSEScanControlPublicData.m_nSEAverageIndex = 0;

                    // FRAME DATA CHECK ( 0x08 보다 작은 DATA가 연속 2개 들어온 경우 )
                    if ((int)dwBytesRead > n + 2 && m_pSemSEScanImageThreadBuffer[n + 1] < 0x08 && m_pSemSEScanImageThreadBuffer[n + 2] < 0x08)
                    {
                        // DATA를 무시한다
                        n += 2;
                    }

                }
                else if (m_pSemSEScanImageThreadBuffer[n] == 0x00)    // Line Start Code
                {
                    // FRAME 시작 문자를 읽기 전에는 모든 이미지 DATA를 버린다
                    if (m_bSemSEFrameFirstCatch == FALSE) continue;

                    // SCAN IMAGE UPDATE (DISPLAY)
                    bImageUpdate = TRUE;

                    // TEST
                    strText = Format("%d,%d", m_SemSEScanControlPublicData.m_nSEDrawXPositionB, m_SemSEScanControlPublicData.m_nSEDrawYPositionB);
                    m_wndDetailControlDlg.SetClientInfomation04Text(strText);// m_ClientInfoText4.SetWindowTextW(strText);
                    
                    //
                    // Noise 축소를 위해 Photo Image 표시전에 Filtering
                    // MedeanFilter or WeightMean 사용
                    //
                    // SCAN CLIBRATION : PHOTO FILTER 선택
                    if (AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_PHOTO_IMAGE_FILTER] == 1)
                    {
                        // 2020.06.05 : PHOTO FILTER #1
                        if(m_SemSEScanControlPublicData.m_bPhotoFilter1SelectFlag==TRUE)
                        {
                            // PHOTO IMAGE FILETR : 미디언 필터
                            if (m_SemSEScanControlPublicData.m_nSEDrawYPositionB > 1 && m_SemSEScanControlPublicData.m_nSEDrawYPositionB < m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight)
                            {
                                int[] nPixelData = NewArray<int>(9);
                                int nFilterPointer = 0;
                                for (int nX = 1; nX < m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth - 1; nX++)
                                {
                                    // FILTER를 위한 PIXEL DATA BITMAP 위치 설정 계산 : 0~8번,                                                                                             해당 위치의 이미지 값 적용
                                    nFilterPointer = ((nX-1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-2)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[0] = m_pSemSEScanImageBitmapBufferB[nFilterPointer];
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-2)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[1] = m_pSemSEScanImageBitmapBufferB[nFilterPointer];
                                    nFilterPointer = ((nX+1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-2)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[2] = m_pSemSEScanImageBitmapBufferB[nFilterPointer];
                                    nFilterPointer = ((nX-1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[3] = m_pSemSEScanImageBitmapBufferB[nFilterPointer];
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[4] = m_pSemSEScanImageBitmapBufferB[nFilterPointer];
                                    nFilterPointer = ((nX+1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[5] = m_pSemSEScanImageBitmapBufferB[nFilterPointer];
                                    nFilterPointer = ((nX-1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-0)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[6] = m_pSemSEScanImageBitmapBufferB[nFilterPointer];
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-0)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[7] = m_pSemSEScanImageBitmapBufferB[nFilterPointer];
                                    nFilterPointer = ((nX+1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-0)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[8] = m_pSemSEScanImageBitmapBufferB[nFilterPointer];

                                    // Insertion sort
                                    m_pAssistDeviceTool.InsertSort(nPixelData, 9);

                                    // FILTER 처리를 위한 버퍼(FILTER#1)에 값 설정
                                    k = ((nX - 0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB - 1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth));
                                    m_pSemSEScanImageBitmapBufferBF1[k] = (byte)nPixelData[4];
                                    
                                    // PHOTO SCAN IMAGE DATA PIXEL POSITION
                                    k = (nX+m_SemSEScanControlPublicData.m_nSEBitmapXOffsetB) + (((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)+m_SemSEScanControlPublicData.m_nSEBitmapYOffsetB)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth);

                                    // PIXEL DATA SET
                                    m_p2ndImageRgb[k].rgbRed   = (byte)nPixelData[4];
                                    m_p2ndImageRgb[k].rgbGreen = (byte)nPixelData[4];
                                    m_p2ndImageRgb[k].rgbBlue  = (byte)nPixelData[4];
                                    
                                }
                            }
                        }

                        //
                        //
                        // 2020.06.05 : PHOTO FILTER #2
                        if(m_SemSEScanControlPublicData.m_bPhotoFilter2SelectFlag==TRUE)
                        {
                            // PHOTO IMAGE FILETR : 샤프닝 필터
                            if (m_SemSEScanControlPublicData.m_nSEDrawYPositionB > 1 && m_SemSEScanControlPublicData.m_nSEDrawYPositionB < m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight)
                            {
                                // 이미지 선명도 필터 ( 샤프닝 기법 ) : 5개 PIXEL 이용
                                // 현재 픽셀의 값에 5를 곱한 후 상하좌우에 있는 픽셀의 값들을 빼준다. 만약 5개 픽셀의 값이 모두 동일하다면 아무 변화가 없을 것이다. 반면 현재 픽셀의 값이 주변 픽셀의 값보다 크다면 그것은 더 부각될 것이고, 작다면 더 작아질 것이다. 결과적으로 엣지가 부각된다.
                                int[] nPixelData = NewArray<int>(5);
                                int nFilterPointer = 0;
                                int nFilterValue   = 0;
                                for (int nX = 1; nX < m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth - 1; nX++)
                                {
                                    // FILTER를 위한 PIXEL DATA BITMAP 위치 설정 계산 : 중심PIXEL, LEFT, UP, RIGHT, DOWN PIXEL 이용                                                        해당 위치의 이미지 값 적용
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[0] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // 중심  PIXEL
                                    nFilterPointer = ((nX-1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[1] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // LEFT  PIXEL
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-2)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[2] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // UP    PIXEL
                                    nFilterPointer = ((nX+1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[3] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // RIGHT PIXEL
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB+0)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[4] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // DOWN  PIXEL

                                    // 이미지 선명도 필터 계산 = 중심PIXEL(x5배) - (나머지 4개 픽셀값) : 중심PIXEL 값이 주변보다 높으면 더높게, 낮으면 더낮게 만든다, 주변과 같으면 같은값이 설정된다
                                    nFilterValue = (nPixelData[0] * 5) - nPixelData[1] - nPixelData[2] - nPixelData[3] - nPixelData[4];
                                    if (nFilterValue < 0  ) nFilterValue = 0;
                                    if (nFilterValue > 255) nFilterValue = 255;

                                    // FILTER 처리를 위한 버퍼(FILTER#2)에 값 설정
                                    k = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth));
                                    m_pSemSEScanImageBitmapBufferBF2[k] = (byte)nFilterValue;

                                    // PHOTO SCAN IMAGE DATA PIXEL POSITION
                                    k = (nX+m_SemSEScanControlPublicData.m_nSEBitmapXOffsetB) + (((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)+m_SemSEScanControlPublicData.m_nSEBitmapYOffsetB)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth);

                                    // PIXEL DATA SET
                                    m_p2ndImageRgb[k].rgbRed   = (byte)nFilterValue;
                                    m_p2ndImageRgb[k].rgbGreen = (byte)nFilterValue;
                                    m_p2ndImageRgb[k].rgbBlue  = (byte)nFilterValue;
                                }
                            }
                        }
                        //
                        //
                        //
                        /*
                        // 2020.06.05 : PHOTO FILTER #3
                        if(m_SemSEScanControlPublicData.m_bPhotoFilter3SelectFlag==TRUE)
                        {
                            // PHOTO IMAGE FILETR : 하이부스트 샤프닝 필터
                            if (m_SemSEScanControlPublicData.m_nSEDrawYPositionB > 1 && m_SemSEScanControlPublicData.m_nSEDrawYPositionB < m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight)
                            {
                                // 이미지 선명도 필터 ( 샤프닝 기법 ) : 9개 PIXEL 이용
                                // 현재 픽셀의 값에 5를 곱한 후 상하좌우에 있는 픽셀의 값들을 빼준다. 만약 5개 픽셀의 값이 모두 동일하다면 아무 변화가 없을 것이다. 반면 현재 픽셀의 값이 주변 픽셀의 값보다 크다면 그것은 더 부각될 것이고, 작다면 더 작아질 것이다. 결과적으로 엣지가 부각된다.
                                int    nPixelData[9];
                                int    nFilterPointer = 0;
                                int    nFilterValue   = 0;
                                double dFilterValue   = 0;
                                double dFilterAlpha   = 9.0; // 중심픽셀 9배
                                for (int nX = 1; nX < m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth - 1; nX++)
                                {
                                    // FILTER를 위한 PIXEL DATA BITMAP 위치 설정 계산 : 중심PIXEL, LEFT, UP, RIGHT, DOWN PIXEL 이용                                                        해당 위치의 이미지 값 적용
                                    // FILTER를 위한 PIXEL DATA BITMAP 위치 설정 계산 : 0~8번,                                                                                             해당 위치의 이미지 값 적용
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[0] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // 중심PIXEL
                                    nFilterPointer = ((nX-1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-2)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[1] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // X-1, Y-1
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-2)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[2] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // X-0, Y-1
                                    nFilterPointer = ((nX+1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-2)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[3] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // X+1, Y-1
                                    nFilterPointer = ((nX-1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[4] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // X-1, Y-0
                                    nFilterPointer = ((nX+1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[5] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // X+1, Y-0
                                    nFilterPointer = ((nX-1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-0)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[6] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // X-1, Y+1
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-0)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[7] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // X-0, Y+1
                                    nFilterPointer = ((nX+1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-0)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[8] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // X+1, Y+1

                                    // 이미지 선명도 필터 계산 = 중심PIXEL(x5배) - (나머지 4개 픽셀값) : 중심PIXEL 값이 주변보다 높으면 더높게, 낮으면 더낮게 만든다, 주변과 같으면 같은값이 설정된다
                                    dFilterValue = (dFilterAlpha*(double)nPixelData[0]) - (double)nPixelData[1] - (double)nPixelData[2] - (double)nPixelData[3] - (double)nPixelData[4] - (double)nPixelData[5] - (double)nPixelData[6] - (double)nPixelData[7] - (double)nPixelData[8];
                                    nFilterValue = (int)(dFilterValue + 0.5);
                                    if (nFilterValue < 0  ) nFilterValue = 0;
                                    if (nFilterValue > 255) nFilterValue = 255;

                                    // FILTER 처리를 위한 버퍼(FILTER#2)에 값 설정
                                    k = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth));
                                    m_pSemSEScanImageBitmapBufferBF2[k] = (BYTE)nFilterValue;

                                    // PHOTO SCAN IMAGE DATA PIXEL POSITION
                                    k = (nX+m_SemSEScanControlPublicData.m_nSEBitmapXOffsetB) + (((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)+m_SemSEScanControlPublicData.m_nSEBitmapYOffsetB)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth);

                                    // PIXEL DATA SET
                                    m_p2ndImageRgb[k].rgbRed   = (BYTE)nFilterValue;
                                    m_p2ndImageRgb[k].rgbGreen = (BYTE)nFilterValue;
                                    m_p2ndImageRgb[k].rgbBlue  = (BYTE)nFilterValue;
                                }
                            }
                        }
                        */
                        /*
                        // 2020.06.10 : PHOTO FILTER #3
                        if(m_SemSEScanControlPublicData.m_bPhotoFilter3SelectFlag==TRUE)
                        {
                            // PHOTO IMAGE FILETR : 언 샤프닝 필터
                            if (m_SemSEScanControlPublicData.m_nSEDrawYPositionB > 1 && m_SemSEScanControlPublicData.m_nSEDrawYPositionB < m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight)
                            {
                                // 이미지 선명도 필터 ( 언 샤프닝 기법 ) : 언샤프PIXEL값 = 원본PIXEL값 - Filter1번PIXEL값 --> 결과PIXEL값 = 원본PIXEL값 + 언샤프PIXEL값
                                int    nPixelData[3];
                                int    nFilterValue = 0;
                                for (int nX = 1; nX < m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth - 1; nX++)
                                {
                                    // PIXEL 위치 계산
                                    k = (nX+((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth));

                                    // 원본 PIXEL값
                                    nPixelData[0] = m_pSemSEScanImageBitmapBufferB[k];

                                    // 언샤프 PIXEL값 계산 : 언샤프    = 원본PIXEL - FILTER1(부드러운이미지)PIXEL
                                    nPixelData[1] = m_pSemSEScanImageBitmapBufferB[k] - m_pSemSEScanImageBitmapBufferBF1[k];

                                    // 언샤프 결과값  계산 : 결과PIXEL = 언샤프PIXEL + 원본PIXEL
                                    nPixelData[2] = m_pSemSEScanImageBitmapBufferB[k] + nPixelData[1];

                                    // 필터 결과값
                                    nFilterValue = nPixelData[2];
                                    if (nFilterValue < 0  ) nFilterValue = 0;
                                    if (nFilterValue > 255) nFilterValue = 255;

                                    // FILTER 처리를 위한 버퍼(FILTER#2)에 값 설정
                                    k = (nX+((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth));
                                    m_pSemSEScanImageBitmapBufferBF2[k] = (BYTE)nFilterValue;

                                    // PHOTO SCAN IMAGE DATA PIXEL POSITION
                                    k = (nX+m_SemSEScanControlPublicData.m_nSEBitmapXOffsetB) + (((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)+m_SemSEScanControlPublicData.m_nSEBitmapYOffsetB)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth);

                                    // PIXEL DATA SET
                                    m_p2ndImageRgb[k].rgbRed   = (BYTE)nFilterValue;
                                    m_p2ndImageRgb[k].rgbGreen = (BYTE)nFilterValue;
                                    m_p2ndImageRgb[k].rgbBlue  = (BYTE)nFilterValue;
                                }
                            }
                        }
                        */
                        // 2020.06.10 : PHOTO FILTER #3
                        if(m_SemSEScanControlPublicData.m_bPhotoFilter3SelectFlag==TRUE)
                        {
                            // PHOTO IMAGE FILETR : 샤프닝 필터
                            if (m_SemSEScanControlPublicData.m_nSEDrawYPositionB > 1 && m_SemSEScanControlPublicData.m_nSEDrawYPositionB < m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight)
                            {
                                // 이미지 선명도 필터 ( 샤프닝 기법 ) : 5개 PIXEL 이용
                                int[] nPixelData = NewArray<int>(5);
                                int nFilterPointer = 0;
                                int nFilterValue   = 0;
                                double dTemp = 0.0;
                                for (int nX = 1; nX < m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth - 1; nX++)
                                {
                                    // FILTER를 위한 PIXEL DATA BITMAP 위치 설정 계산 : 중심PIXEL, LEFT, UP, RIGHT, DOWN PIXEL 이용                                                        해당 위치의 이미지 값 적용
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[0] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // 중심  PIXEL
                                    nFilterPointer = ((nX-1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[1] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // LEFT  PIXEL
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-2)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[2] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // UP    PIXEL
                                    nFilterPointer = ((nX+1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[3] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // RIGHT PIXEL
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB+0)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[4] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // DOWN  PIXEL

                                    // 이미지 선명도 필터 계산 = 중심PIXEL - (나머지4개 픽셀값) : 중심PIXEL 값이 주변보다 높으면 더높게, 낮으면 더낮게 만든다, 주변과 같으면 같은값이 설정된다
                                    dTemp = ((8.0/4.0)*nPixelData[0]) + ((-1.0/4.0)*nPixelData[1]) + ((-1.0/4.0)*nPixelData[2]) + ((-1.0/4.0)*nPixelData[3]) + ((-1.0/4.0)*nPixelData[4]);
                                    nFilterValue = (int)(dTemp + 0.5);
                                    if (nFilterValue < 0  ) nFilterValue = 0;
                                    if (nFilterValue > 255) nFilterValue = 255;

                                    // FILTER 처리를 위한 버퍼(FILTER#2)에 값 설정
                                    k = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth));
                                    m_pSemSEScanImageBitmapBufferBF2[k] = (byte)nFilterValue;

                                    // PHOTO SCAN IMAGE DATA PIXEL POSITION
                                    k = (nX+m_SemSEScanControlPublicData.m_nSEBitmapXOffsetB) + (((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)+m_SemSEScanControlPublicData.m_nSEBitmapYOffsetB)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth);

                                    // PIXEL DATA SET
                                    m_p2ndImageRgb[k].rgbRed   = (byte)nFilterValue;
                                    m_p2ndImageRgb[k].rgbGreen = (byte)nFilterValue;
                                    m_p2ndImageRgb[k].rgbBlue  = (byte)nFilterValue;
                                }
                            }
                        }
                        //
                        //
                        //
                    }
                    else
                    {
                        // 처리안함
                    }
                    //
                    //
                    //
                    
                    // LINE START : X POSITION RESET
                    m_SemSEScanControlPublicData.m_nSEDrawXPositionB = 0;
                    // LINE START : Y POSITION 증가 및 확인
                    if (m_SemSEScanControlPublicData.m_nSEDrawYPositionB < m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight - 1)
                    {
                        // FRAME : Y Position 증가
                        m_SemSEScanControlPublicData.m_nSEDrawYPositionB++;
                    }
                    else
                    {
                        // PHOTO SCAN CALIBRATION MODE FLAG CHECK?
                        if (m_bPhotoCalibrationFlag == FALSE)
                        {
                            // PHOTO1/2 SCAN MODE의 LINE 수 보다 크기 때문에 스캔을 종료한다
                            bCompleted = TRUE;
                            // for문 종료
                            break;
                        }
                        else
                        {
                            // PHOTO CALIBRATION MODE 스캔은 프레임을 연속 스캔한다.
                            bCompleted = FALSE;
                            // FRAME COUNT
                            m_SemSEScanControlPublicData.m_nSEFrameCount = 1;
                        }
                    }

                }
                else
                {
                    // FRAME 시작 문자를 읽기 전에는 모든 이미지 DATA를 버린다
                    if (m_bSemSEFrameFirstCatch == FALSE) continue;

                    // SCAN IMAGE UPDATE (DISPLAY)
                    //bImageUpdate = TRUE;
                    // 2021.05.10
                    // 이미지 업데이트 속도 개선을 위해 X/Y PIXEL 처리시에는 이미지를 갱신 하지 않는다
                    bImageUpdate = FALSE;
                    if (n == (int)(dwBytesRead-1))
                    {
                        bImageUpdate = TRUE;
                    }

                    // DRAW X POSITION 확인 ( 0 ~ 가로폭 미만)
                    if (m_SemSEScanControlPublicData.m_nSEDrawXPositionB < m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)
                    {
                        // PHOTO SCAN IMAGE DATA ( 1 BYTE )
                        btData = m_pSemSEScanImageThreadBuffer[n];

                        // PIXEL DATA BITMAP 위치 설정 계산
                        nAverPointer = ((m_SemSEScanControlPublicData.m_nSEDrawXPositionB + m_SemSEScanControlPublicData.m_nSEBitmapXOffsetB) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB + m_SemSEScanControlPublicData.m_nSEBitmapYOffsetB) * 640)) * 1;
                        // 해당 위치의 이미지 값 적용
                        m_pSemSEScanImageBitmapBufferB[nAverPointer + m_SemSEScanControlPublicData.m_nSEAverageIndex] = btData;
                        // 필터 처리를 위한 버퍼에 값 설정
                        m_pSemSEScanImageBitmapBufferBF1[nAverPointer + m_SemSEScanControlPublicData.m_nSEAverageIndex] = btData;
                        m_pSemSEScanImageBitmapBufferBF2[nAverPointer + m_SemSEScanControlPublicData.m_nSEAverageIndex] = btData;

                        // PHOTO SCAN IMAGE DATA PIXEL POSITION
                        k = (m_SemSEScanControlPublicData.m_nSEDrawXPositionB + m_SemSEScanControlPublicData.m_nSEBitmapXOffsetB) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB + m_SemSEScanControlPublicData.m_nSEBitmapYOffsetB) * 640);

                        // PIXEL DATA SET
                        m_p2ndImageRgb[k].rgbRed   = btData;
                        m_p2ndImageRgb[k].rgbGreen = btData;
                        m_p2ndImageRgb[k].rgbBlue  = btData;

                        // FRAME : X Position 증가
                        m_SemSEScanControlPublicData.m_nSEDrawXPositionB++;

                    }
                } // 식별문자 코드 처리 : 0xFF, 0xFE, 0xFD, 0xFC, 0x00, 이미지데이터, 등등 if()문 처리
            } // 수신한 문자 처리 for()문 처리

            //
            //
            //

            // FRAME 시작 문자를 읽기 전에는 모든 이미지 DATA를 버린다
            if (m_bSemSEFrameFirstCatch == FALSE) return RET_SUCCESS_OK;

            // 2nd PIXEL BITMAP DATA SET
            m_2ndImage640x480Bitmap.SetBitmapBits(m_bm2ndBitmap.bmWidthBytes * m_bm2ndBitmap.bmHeight, m_p2ndBitmapBits);

            // SE PHOTO SCAN IMAGE DATA DRAW UPDATE ( DISPLAY ) : 1 PIXEL 단위 데이터 처리
            // 2019.09.18 이미지 업데이트(REDRAW)는 타이머를 이용하여 정기적으로 실행한다
            // PHOTO SCAN BITMAP IMAGE UPDATE
            if (bImageUpdate == TRUE)
            {
                // SCAN IMAGE UPDATE (DISPLAY)
                SemSEPhotoScan12ImageDataUpdateControl();
            }

            //
            //
            //

            // PHOTO MODE 스캔은 1 프레임만 스캔한 후, 이전 스캔모드로 돌아간다
            if (bCompleted == TRUE)
            {
                // 포토스캔 종료
                m_SemSEScanControlPublicData.m_bPhotoScanCompleted = TRUE;

                // 이전 스캔모드 실행
                if (m_SemSEScanControlPublicData.m_nSaveScanMode == RED_SCAN_MODE)
                {
                    // RED SCAN MODE
                    m_wndScanControlDlg.OnBnClickedScanControlRedScanBtn();
                }
                else if (m_SemSEScanControlPublicData.m_nSaveScanMode == FAST_SCAN_MODE)
                {
                    // FAST SCAN MODE
                    m_wndScanControlDlg.OnBnClickedScanControlFastScanBtn();
                }
                else if (m_SemSEScanControlPublicData.m_nSaveScanMode == SLOW1_SCAN_MODE)
                {
                    // SLOW1 SCAN MODE
                    m_wndScanControlDlg.OnBnClickedScanControlSlow1ScanBtn();
                }
                else if (m_SemSEScanControlPublicData.m_nSaveScanMode == SLOW2_SCAN_MODE)
                {
                    // SLOW2 SCAN MODE
                    m_wndScanControlDlg.OnBnClickedScanControlSlow2ScanBtn();
                }

                // PHOTO IMAGE 원본 백업 ( BITMAP DATA )
                memcpy(m_p2ndOrgBitmapBits, m_p2ndBitmapBits, m_bm2ndBitmap.bmWidthBytes * m_bm2ndBitmap.bmHeight);

                // edit by SFA(wjYoo)
                // Scan Image 완료
                if(IsRunImageGrab(GRAB_TYPE_SCAN)==TRUE)
                {
                    SetImageGrabEnd(TRUE, m_p2ndImageRgb);
                }
                //
                // 히스토그램 그래프 보기
                //
                double[] dHisto = NewArray<double>(256);
                int nCMax = 0;
                memset(dHisto, 0x00, sizeof(double) * 256);
                // 히스토그램 계산
                m_pAssistPublicTool.HistogramNormalize(ref m_2ndImage640x480Bitmap, dHisto, ref nCMax);
                // 히스토그램 그래프 그리기
                m_pAssistPublicTool.DrawHistogramGraph(m_p4thImageDC, ref m_4thImageMemDC, dHisto, nCMax, 256);
                // HISTOGRAM ANALYSIS CONTROL
                m_nHistogramAnalysisContrasrt = 50;
                m_nHistogramAnalysisBright    = 50;
                m_nHistogramAnalysisGamma     = 50;
                //
                //
                //

                // 2020.09.21 : PHOTO FILTER #1 FLAG 복구
                if (m_SemSEScanControlPublicData.m_bPhotoFilter1SelectFlag != m_SemSEScanControlPublicData.m_bOldPhotoFilter1SelectFlag)
                {
                    // 원래 필터 FLAG MODE로 복구
                    m_SemSEScanControlPublicData.m_bPhotoFilter1SelectFlag = m_SemSEScanControlPublicData.m_bOldPhotoFilter1SelectFlag;
                    // PHOTO FILTER SELECT BUTTON REDRAW
                    m_wndScanControlDlg.LoadPhotoFilterSelectButtonBitmapImage(m_SemSEScanControlPublicData.m_bPhotoFilter1SelectFlag, m_SemSEScanControlPublicData.m_bPhotoFilter2SelectFlag, m_SemSEScanControlPublicData.m_bPhotoFilter3SelectFlag, m_SemSEScanControlPublicData.m_bPhotoFilter4SelectFlag);
                    m_wndScanControlDlg.m_bPhotoFilter1SelectFlag    = m_SemSEScanControlPublicData.m_bPhotoFilter1SelectFlag;
                    m_wndScanControlDlg.m_bOldPhotoFilter1SelectFlag = m_SemSEScanControlPublicData.m_bPhotoFilter1SelectFlag;
                }

                // 2021.04.14
                // MONOGRAM TOOLS : SCAN 모드 재설정
                MonogramToolsSetScanModeExecute(); // 원본 theApp.MonogramToolsSetScanModeExecute()
            }

            //
            //
            //

            return RET_SUCCESS_OK;
        }

        // ---- NanoImpact2020cfeDlg.cpp:14537 ----
        //
        // SEM SE Photo Scan 3/4/5 Image Data 1280x960 : 수신된 데이터 이미지 업데이트 ( DISPLAY ) 처리
        //
        public uint SemSEPhotoScan345ImageDataUpdateProcessing()
        {
            string strText    = "";
            int   n            = 0;
            int   k            = 0;
            int   nReadPointer = 0;
            int   nAverPointer = 0;
            byte  btData       = 0;
            int   nDataSize    = 0;
            uint dwBytesRead  = 0;
            byte  btRED        = 0;
            byte  btGREEN      = 0;
            byte  btBLUE       = 0;
            int   nAverIndex   = 0;
            int   nTotalValue  = 0;
            int   nAverValue   = 0;
            int  bImageUpdate = FALSE;
            int  bCompleted   = FALSE;

            //
            // SE Photo Scan 3/4/5 Image Data 프로세싱
            //

            // 처리할 DATA 크기 확인
            if (m_nSemSEScanImageSaveCount < 1)
            {
                // 저장버퍼 : 초기화
                m_nSemSEScanImageSaveStart = 0; // 저장버퍼 현재위치
                m_nSemSEScanImageSaveCount = 0; // 저장버퍼 현재크기

                return 0;
            }

            // 처리할 DATA 크기 설정 : 저장된 DATA 모두 처리
            dwBytesRead = (uint)m_nSemSEScanImageSaveCount;

            // 처리할 SEM SE PHOTO SCAN IMAGE DATA 복사
            memcpy(m_pSemSEScanImageThreadBuffer, m_pSemSEScanImageReadBuffer, dwBytesRead);

            // 저장버퍼 CLEAR
            memset(m_pSemSEScanImageReadBuffer, 0x00, dwBytesRead);// m_nSemSEScanImageDataMax);
            // 저장버퍼 : 초기화
            m_nSemSEScanImageSaveStart = 0; // 저장버퍼 현재위치
            m_nSemSEScanImageSaveCount = 0; // 저장버퍼 현재크기

            //
            //
            //

            for (n = 0; n < (int)dwBytesRead; n++)
            {
                if (m_pSemSEScanImageThreadBuffer[n] == 0xFF)
                {
                    // FRAME START
                    m_SemSEScanControlPublicData.m_nSEDrawXPositionB = 0;       // SE PHOTO SCAN IMAGE BUFFER : X Draw Position
                    m_SemSEScanControlPublicData.m_nSEDrawYPositionB = 0;       // SE PHOTO SCAN IMAGE BUFFER : Y Draw Position
                    m_bSemSEFrameFirstCatch = TRUE;                             // SE PHOTO SCAN IMAGE READ BUFFER : FRAME 처음 읽기 처리 완료

                    // SCAN IMAGE UPDATE (DISPLAY)
                    bImageUpdate = TRUE;

                    // FRAME COUNT
                    m_SemSEScanControlPublicData.m_nSEFrameCount++;
                    if (m_SemSEScanControlPublicData.m_nSEFrameCount > 1)
                    {
                        // PHOTO SCAN CALIBRATION MODE FLAG CHECK?
                        if (m_bPhotoCalibrationFlag == FALSE)
                        {
                            // PHOTO MODE 스캔은 1 프레임만 스캔한 후, 이전 스캔모드로 돌아간다
                            bCompleted = TRUE;
                            // for문 종료
                            break;
                        }
                        else
                        {
                            // PHOTO CALIBRATION MODE 스캔은 프레임을 연속 스캔한다.
                            bCompleted = FALSE;
                            // FRAME COUNT
                            m_SemSEScanControlPublicData.m_nSEFrameCount = 1;
                        }
                    }

                    // BITMAP IMAGE WEIGHT AVERAGE COUNT RESET
                    m_SemSEScanControlPublicData.m_nSEAverageCount = 1;
                    m_SemSEScanControlPublicData.m_nSEAverageIndex = 0;

                    // FRAME DATA CHECK ( 0x08 보다 작은 DATA가 연속 2개 들어온 경우 )
                    if ((int)dwBytesRead > n + 2 && m_pSemSEScanImageThreadBuffer[n + 1] < 0x08 && m_pSemSEScanImageThreadBuffer[n + 2] < 0x08)
                    {
                        // DATA를 무시한다
                        n += 2;
                    }

                }
                else if (m_pSemSEScanImageThreadBuffer[n] == 0x00)    // Line Start Code
                {
                    // FRAME 시작 문자를 읽기 전에는 모든 이미지 DATA를 버린다
                    if (m_bSemSEFrameFirstCatch == FALSE) continue;

                    // SCAN IMAGE UPDATE (DISPLAY)
                    bImageUpdate = TRUE;

                    // TEST
                    strText = Format("%d,%d", m_SemSEScanControlPublicData.m_nSEDrawXPositionB, m_SemSEScanControlPublicData.m_nSEDrawYPositionB);
                    m_wndDetailControlDlg.SetClientInfomation04Text(strText);// m_ClientInfoText4.SetWindowTextW(strText);
                    
                    //
                    // Noise 축소를 위해 Photo Image 표시전에 Filtering
                    // MedeanFilter or WeightMean 사용
                    //
                    // SCAN CLIBRATION : PHOTO FILTER 선택
                    if (AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_PHOTO_IMAGE_FILTER] == 1)
                    {
                        // 2020.06.05 : PHOTO FILTER #1
                        if(m_SemSEScanControlPublicData.m_bPhotoFilter1SelectFlag==TRUE)
                        {
                            // PHOTO IMAGE FILETR : 미디언 필터
                            if (m_SemSEScanControlPublicData.m_nSEDrawYPositionB > 1 && m_SemSEScanControlPublicData.m_nSEDrawYPositionB < m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight)
                            {
                                int[] nPixelData = NewArray<int>(9);
                                int nFilterPointer = 0;
                                for (int nX = 1; nX < m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth - 1; nX++)
                                {
                                    // FILTER를 위한 PIXEL DATA BITMAP 위치 설정 계산 : 0~8번,                                                                                             해당 위치의 이미지 값 적용
                                    nFilterPointer = ((nX-1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-2)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[0] = m_pSemSEScanImageBitmapBufferB[nFilterPointer];
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-2)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[1] = m_pSemSEScanImageBitmapBufferB[nFilterPointer];
                                    nFilterPointer = ((nX+1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-2)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[2] = m_pSemSEScanImageBitmapBufferB[nFilterPointer];
                                    nFilterPointer = ((nX-1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[3] = m_pSemSEScanImageBitmapBufferB[nFilterPointer];
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[4] = m_pSemSEScanImageBitmapBufferB[nFilterPointer];
                                    nFilterPointer = ((nX+1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[5] = m_pSemSEScanImageBitmapBufferB[nFilterPointer];
                                    nFilterPointer = ((nX-1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-0)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[6] = m_pSemSEScanImageBitmapBufferB[nFilterPointer];
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-0)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[7] = m_pSemSEScanImageBitmapBufferB[nFilterPointer];
                                    nFilterPointer = ((nX+1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-0)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[8] = m_pSemSEScanImageBitmapBufferB[nFilterPointer];

                                    // Insertion sort
                                    m_pAssistDeviceTool.InsertSort(nPixelData, 9);

                                    // FILTER 처리를 위한 버퍼(FILTER#1)에 값 설정
                                    k = ((nX - 0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB - 1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth));
                                    m_pSemSEScanImageBitmapBufferBF1[k] = (byte)nPixelData[4];

                                    // PHOTO SCAN IMAGE DATA PIXEL POSITION
                                    k = (nX+m_SemSEScanControlPublicData.m_nSEBitmapXOffsetB) + (((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)+m_SemSEScanControlPublicData.m_nSEBitmapYOffsetB)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth);

                                    // PIXEL DATA SET
                                    m_p2ndImageRgb[k].rgbRed   = (byte)nPixelData[4];
                                    m_p2ndImageRgb[k].rgbGreen = (byte)nPixelData[4];
                                    m_p2ndImageRgb[k].rgbBlue  = (byte)nPixelData[4];

                                }
                            }
                        }
                        //
                        //
                        //
                        // 2020.06.05 : PHOTO FILTER #2
                        if(m_SemSEScanControlPublicData.m_bPhotoFilter2SelectFlag==TRUE)
                        {
                            // PHOTO IMAGE FILETR : 샤프닝 필터
                            if (m_SemSEScanControlPublicData.m_nSEDrawYPositionB > 1 && m_SemSEScanControlPublicData.m_nSEDrawYPositionB < m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight)
                            {
                                // 이미지 선명도 필터 ( 샤프닝 기법 ) : 5개 PIXEL 이용
                                // 현재 픽셀의 값에 5를 곱한 후 상하좌우에 있는 픽셀의 값들을 빼준다. 만약 5개 픽셀의 값이 모두 동일하다면 아무 변화가 없을 것이다. 반면 현재 픽셀의 값이 주변 픽셀의 값보다 크다면 그것은 더 부각될 것이고, 작다면 더 작아질 것이다. 결과적으로 엣지가 부각된다.
                                int[] nPixelData = NewArray<int>(5);
                                int nFilterPointer = 0;
                                int nFilterValue   = 0;
                                for (int nX = 1; nX < m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth - 1; nX++)
                                {
                                    // FILTER를 위한 PIXEL DATA BITMAP 위치 설정 계산 : 중심PIXEL, LEFT, UP, RIGHT, DOWN PIXEL 이용                                                        해당 위치의 이미지 값 적용
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[0] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // 중심  PIXEL
                                    nFilterPointer = ((nX-1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[1] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // LEFT  PIXEL
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-2)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[2] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // UP    PIXEL
                                    nFilterPointer = ((nX+1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[3] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // RIGHT PIXEL
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB+0)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[4] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // DOWN  PIXEL

                                    // 이미지 선명도 필터 계산 = 중심PIXEL(x5배) - (나머지 4개 픽셀값) : 중심PIXEL 값이 주변보다 높으면 더높게, 낮으면 더낮게 만든다, 주변과 같으면 같은값이 설정된다
                                    nFilterValue = (nPixelData[0] * 5) - nPixelData[1] - nPixelData[2] - nPixelData[3] - nPixelData[4];
                                    if (nFilterValue < 0  ) nFilterValue = 0;
                                    if (nFilterValue > 255) nFilterValue = 255;

                                    // FILTER 처리를 위한 버퍼(FILTER#2)에 값 설정
                                    k = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth));
                                    m_pSemSEScanImageBitmapBufferBF2[k] = (byte)nFilterValue;

                                    // PHOTO SCAN IMAGE DATA PIXEL POSITION
                                    k = (nX+m_SemSEScanControlPublicData.m_nSEBitmapXOffsetB) + (((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)+m_SemSEScanControlPublicData.m_nSEBitmapYOffsetB)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth);

                                    // PIXEL DATA SET
                                    m_p2ndImageRgb[k].rgbRed   = (byte)nFilterValue;
                                    m_p2ndImageRgb[k].rgbGreen = (byte)nFilterValue;
                                    m_p2ndImageRgb[k].rgbBlue  = (byte)nFilterValue;
                                }
                            }
                        }
                        //
                        //
                        //
                        /*
                        // 2020.06.05 : PHOTO FILTER #3
                        if(m_SemSEScanControlPublicData.m_bPhotoFilter3SelectFlag==TRUE)
                        {
                            // PHOTO IMAGE FILETR : 하이부스트 샤프닝 필터
                            if (m_SemSEScanControlPublicData.m_nSEDrawYPositionB > 1 && m_SemSEScanControlPublicData.m_nSEDrawYPositionB < m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight)
                            {
                                // 이미지 선명도 필터 ( 샤프닝 기법 ) : 9개 PIXEL 이용
                                // 현재 픽셀의 값에 5를 곱한 후 상하좌우에 있는 픽셀의 값들을 빼준다. 만약 5개 픽셀의 값이 모두 동일하다면 아무 변화가 없을 것이다. 반면 현재 픽셀의 값이 주변 픽셀의 값보다 크다면 그것은 더 부각될 것이고, 작다면 더 작아질 것이다. 결과적으로 엣지가 부각된다.
                                int    nPixelData[9];
                                int    nFilterPointer = 0;
                                int    nFilterValue   = 0;
                                double dFilterValue   = 0;
                                double dFilterAlpha   = 9.0; // 중심픽셀 9배
                                for (int nX = 1; nX < m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth - 1; nX++)
                                {
                                    // FILTER를 위한 PIXEL DATA BITMAP 위치 설정 계산 : 중심PIXEL, LEFT, UP, RIGHT, DOWN PIXEL 이용                                                        해당 위치의 이미지 값 적용
                                    // FILTER를 위한 PIXEL DATA BITMAP 위치 설정 계산 : 0~8번,                                                                                             해당 위치의 이미지 값 적용
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[0] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // 중심PIXEL
                                    nFilterPointer = ((nX-1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-2)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[1] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // X-1, Y-1
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-2)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[2] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // X-0, Y-1
                                    nFilterPointer = ((nX+1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-2)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[3] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // X+1, Y-1
                                    nFilterPointer = ((nX-1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[4] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // X-1, Y-0
                                    nFilterPointer = ((nX+1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[5] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // X+1, Y-0
                                    nFilterPointer = ((nX-1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-0)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[6] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // X-1, Y+1
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-0)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[7] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // X-0, Y+1
                                    nFilterPointer = ((nX+1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-0)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[8] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // X+1, Y+1

                                    // 이미지 선명도 필터 계산 = 중심PIXEL(x5배) - (나머지 4개 픽셀값) : 중심PIXEL 값이 주변보다 높으면 더높게, 낮으면 더낮게 만든다, 주변과 같으면 같은값이 설정된다
                                    dFilterValue = (dFilterAlpha*(double)nPixelData[0]) - (double)nPixelData[1] - (double)nPixelData[2] - (double)nPixelData[3] - (double)nPixelData[4] - (double)nPixelData[5] - (double)nPixelData[6] - (double)nPixelData[7] - (double)nPixelData[8];
                                    nFilterValue = (int)(dFilterValue + 0.5);
                                    if (nFilterValue < 0  ) nFilterValue = 0;
                                    if (nFilterValue > 255) nFilterValue = 255;

                                    // FILTER 처리를 위한 버퍼(FILTER#2)에 값 설정
                                    k = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth));
                                    m_pSemSEScanImageBitmapBufferBF2[k] = (BYTE)nFilterValue;

                                    // PHOTO SCAN IMAGE DATA PIXEL POSITION
                                    k = (nX+m_SemSEScanControlPublicData.m_nSEBitmapXOffsetB) + (((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)+m_SemSEScanControlPublicData.m_nSEBitmapYOffsetB)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth);

                                    // PIXEL DATA SET
                                    m_p2ndImageRgb[k].rgbRed   = (BYTE)nFilterValue;
                                    m_p2ndImageRgb[k].rgbGreen = (BYTE)nFilterValue;
                                    m_p2ndImageRgb[k].rgbBlue  = (BYTE)nFilterValue;
                                }
                            }
                        }
                        */
                        /*
                        // 2020.06.10 : PHOTO FILTER #3
                        if(m_SemSEScanControlPublicData.m_bPhotoFilter3SelectFlag==TRUE)
                        {
                            // PHOTO IMAGE FILETR : 언 샤프닝 필터
                            if (m_SemSEScanControlPublicData.m_nSEDrawYPositionB > 1 && m_SemSEScanControlPublicData.m_nSEDrawYPositionB < m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight)
                            {
                                // 이미지 선명도 필터 ( 언 샤프닝 기법 ) : 언샤프PIXEL값 = 원본PIXEL값 - Filter1번PIXEL값 --> 결과PIXEL값 = 원본PIXEL값 + 언샤프PIXEL값
                                int    nPixelData[3];
                                int    nFilterValue = 0;
                                for (int nX = 1; nX < m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth - 1; nX++)
                                {
                                    // PIXEL 위치 계산
                                    k = (nX+((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth));

                                    // 원본 PIXEL값
                                    nPixelData[0] = m_pSemSEScanImageBitmapBufferB[k];

                                    // 언샤프 PIXEL값 계산 : 언샤프    = 원본PIXEL - FILTER1(부드러운이미지)PIXEL
                                    nPixelData[1] = m_pSemSEScanImageBitmapBufferB[k] - m_pSemSEScanImageBitmapBufferBF1[k];

                                    // 언샤프 결과값  계산 : 결과PIXEL = 언샤프PIXEL + 원본PIXEL
                                    nPixelData[2] = m_pSemSEScanImageBitmapBufferB[k] + nPixelData[1];

                                    // 필터 결과값
                                    nFilterValue = nPixelData[2];
                                    if (nFilterValue < 0  ) nFilterValue = 0;
                                    if (nFilterValue > 255) nFilterValue = 255;

                                    // FILTER 처리를 위한 버퍼(FILTER#2)에 값 설정
                                    k = (nX+((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth));
                                    m_pSemSEScanImageBitmapBufferBF2[k] = (BYTE)nFilterValue;

                                    // PHOTO SCAN IMAGE DATA PIXEL POSITION
                                    k = (nX+m_SemSEScanControlPublicData.m_nSEBitmapXOffsetB) + (((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)+m_SemSEScanControlPublicData.m_nSEBitmapYOffsetB)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth);

                                    // PIXEL DATA SET
                                    m_p2ndImageRgb[k].rgbRed   = (BYTE)nFilterValue;
                                    m_p2ndImageRgb[k].rgbGreen = (BYTE)nFilterValue;
                                    m_p2ndImageRgb[k].rgbBlue  = (BYTE)nFilterValue;
                                }
                            }
                        }
                        */
                        // 2020.06.10 : PHOTO FILTER #3
                        if(m_SemSEScanControlPublicData.m_bPhotoFilter3SelectFlag==TRUE)
                        {
                            // PHOTO IMAGE FILETR : 샤프닝 필터
                            if (m_SemSEScanControlPublicData.m_nSEDrawYPositionB > 1 && m_SemSEScanControlPublicData.m_nSEDrawYPositionB < m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight)
                            {
                                // 이미지 선명도 필터 ( 샤프닝 기법 ) : 5개 PIXEL 이용
                                int[] nPixelData = NewArray<int>(5);
                                int nFilterPointer = 0;
                                int nFilterValue   = 0;
                                double dTemp = 0.0;
                                for (int nX = 1; nX < m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth - 1; nX++)
                                {
                                    // FILTER를 위한 PIXEL DATA BITMAP 위치 설정 계산 : 중심PIXEL, LEFT, UP, RIGHT, DOWN PIXEL 이용                                                        해당 위치의 이미지 값 적용
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[0] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // 중심  PIXEL
                                    nFilterPointer = ((nX-1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[1] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // LEFT  PIXEL
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-2)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[2] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // UP    PIXEL
                                    nFilterPointer = ((nX+1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[3] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // RIGHT PIXEL
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB+0)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[4] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // DOWN  PIXEL

                                    // 이미지 선명도 필터 계산 = 중심PIXEL - (나머지4개 픽셀값) : 중심PIXEL 값이 주변보다 높으면 더높게, 낮으면 더낮게 만든다, 주변과 같으면 같은값이 설정된다
                                    dTemp = ((8.0/4.0)*nPixelData[0]) + ((-1.0/4.0)*nPixelData[1]) + ((-1.0/4.0)*nPixelData[2]) + ((-1.0/4.0)*nPixelData[3]) + ((-1.0/4.0)*nPixelData[4]);
                                    nFilterValue = (int)(dTemp + 0.5);
                                    if (nFilterValue < 0  ) nFilterValue = 0;
                                    if (nFilterValue > 255) nFilterValue = 255;

                                    // FILTER 처리를 위한 버퍼(FILTER#2)에 값 설정
                                    k = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth));
                                    m_pSemSEScanImageBitmapBufferBF2[k] = (byte)nFilterValue;

                                    // PHOTO SCAN IMAGE DATA PIXEL POSITION
                                    k = (nX+m_SemSEScanControlPublicData.m_nSEBitmapXOffsetB) + (((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)+m_SemSEScanControlPublicData.m_nSEBitmapYOffsetB)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth);

                                    // PIXEL DATA SET
                                    m_p2ndImageRgb[k].rgbRed   = (byte)nFilterValue;
                                    m_p2ndImageRgb[k].rgbGreen = (byte)nFilterValue;
                                    m_p2ndImageRgb[k].rgbBlue  = (byte)nFilterValue;
                                }
                            }
                        }
                        //
                        //
                        //
                    }
                    else
                    {
                        // 처리안함
                    }
                    //
                    //
                    //

                    // LINE START : X POSITION RESET
                    m_SemSEScanControlPublicData.m_nSEDrawXPositionB = 0;
                    // LINE START : Y POSITION 증가 및 확인
                    if (m_SemSEScanControlPublicData.m_nSEDrawYPositionB < m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight - 1)
                    {
                        // FRAME : Y Position 증가
                        m_SemSEScanControlPublicData.m_nSEDrawYPositionB++;
                    }
                    else
                    {
                        // PHOTO SCAN CALIBRATION MODE FLAG CHECK?
                        if (m_bPhotoCalibrationFlag == FALSE)
                        {
                            // PHOTO3/4/5 SCAN MODE의 LINE 수 보다 크기 때문에 스캔을 종료한다
                            bCompleted = TRUE;
                            // for문 종료
                            break;
                        }
                        else
                        {
                            // PHOTO CALIBRATION MODE 스캔은 프레임을 연속 스캔한다.
                            bCompleted = FALSE;
                            // FRAME COUNT
                            m_SemSEScanControlPublicData.m_nSEFrameCount = 1;
                        }
                    }

                }
                else
                {
                    // FRAME 시작 문자를 읽기 전에는 모든 이미지 DATA를 버린다
                    if (m_bSemSEFrameFirstCatch == FALSE) continue;

                    // SCAN IMAGE UPDATE (DISPLAY)
                    //bImageUpdate = TRUE;
                    // 2021.05.10
                    // 이미지 업데이트 속도 개선을 위해 X/Y PIXEL 처리시에는 이미지를 갱신 하지 않는다
                    bImageUpdate = FALSE;
                    if (n == (int)(dwBytesRead-1))
                    {
                        bImageUpdate = TRUE;
                    }

                    // DRAW X POSITION 확인 ( 0 ~ 가로폭 미만)
                    if (m_SemSEScanControlPublicData.m_nSEDrawXPositionB < m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)
                    {
                        // PHOTO SCAN IMAGE DATA ( 1 BYTE )
                        btData = m_pSemSEScanImageThreadBuffer[n];

                        // PIXEL DATA BITMAP 위치 설정 계산
                        nAverPointer = (m_SemSEScanControlPublicData.m_nSEDrawXPositionB + m_SemSEScanControlPublicData.m_nSEBitmapXOffsetB) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB + m_SemSEScanControlPublicData.m_nSEBitmapYOffsetB) * 1280);
                        // 해당 위치의 이미지 값 적용
                        m_pSemSEScanImageBitmapBufferB[nAverPointer + m_SemSEScanControlPublicData.m_nSEAverageIndex] = btData;
                        // 필터 처리를 위한 해당 위치의 이미지 값 적용
                        m_pSemSEScanImageBitmapBufferBF1[nAverPointer + m_SemSEScanControlPublicData.m_nSEAverageIndex] = btData;
                        m_pSemSEScanImageBitmapBufferBF2[nAverPointer + m_SemSEScanControlPublicData.m_nSEAverageIndex] = btData;

                        // PHOTO SCAN IMAGE DATA PIXEL POSITION
                        k = (m_SemSEScanControlPublicData.m_nSEDrawXPositionB + m_SemSEScanControlPublicData.m_nSEBitmapXOffsetB) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB + m_SemSEScanControlPublicData.m_nSEBitmapYOffsetB) * 1280);

                        // PIXEL DATA SET
                        m_p2ndImageRgb[k].rgbRed   = btData;
                        m_p2ndImageRgb[k].rgbGreen = btData;
                        m_p2ndImageRgb[k].rgbBlue  = btData;

                        // FRAME : X Position 증가
                        m_SemSEScanControlPublicData.m_nSEDrawXPositionB++;

                    }
                } // 식별문자 코드 처리 : 0xFF, 0xFE, 0xFD, 0xFC, 0x00, 이미지데이터, 등등 if()문 처리
            } // 수신한 문자 처리 for()문 처리

            //
            //
            //

            // FRAME 시작 문자를 읽기 전에는 모든 이미지 DATA를 버린다
            if (m_bSemSEFrameFirstCatch == FALSE) return RET_SUCCESS_OK;

            // 2nd PIXEL BITMAP DATA SET
            m_2ndImage1280x960Bitmap.SetBitmapBits(m_bm2ndBitmap.bmWidthBytes * m_bm2ndBitmap.bmHeight, m_p2ndBitmapBits);

            // SE PHOTO SCAN IMAGE DATA DRAW UPDATE ( DISPLAY ) : 1 PIXEL 단위 데이터 처리
            // 2019.09.18 이미지 업데이트(REDRAW)는 타이머를 이용하여 정기적으로 실행한다
            // PHOTO SCAN BITMAP IMAGE UPDATE
            if (bImageUpdate == TRUE)
            {
                // SCAN IMAGE UPDATE (DISPLAY)
                SemSEPhotoScan345ImageDataUpdateControl();
            }

            //
            //
            //

            // PHOTO MODE 스캔은 1 프레임만 스캔한 후, 이전 스캔모드로 돌아간다
            if (bCompleted == TRUE)
            {
                // 포토스캔 종료
                m_SemSEScanControlPublicData.m_bPhotoScanCompleted = TRUE;

                // 이전 스캔모드 실행
                if (m_SemSEScanControlPublicData.m_nSaveScanMode == RED_SCAN_MODE)
                {
                    // RED SCAN MODE
                    m_wndScanControlDlg.OnBnClickedScanControlRedScanBtn();
                }
                else if (m_SemSEScanControlPublicData.m_nSaveScanMode == FAST_SCAN_MODE)
                {
                    // FAST SCAN MODE
                    m_wndScanControlDlg.OnBnClickedScanControlFastScanBtn();
                }
                else if (m_SemSEScanControlPublicData.m_nSaveScanMode == SLOW1_SCAN_MODE)
                {
                    // SLOW1 SCAN MODE
                    m_wndScanControlDlg.OnBnClickedScanControlSlow1ScanBtn();
                }
                else if (m_SemSEScanControlPublicData.m_nSaveScanMode == SLOW2_SCAN_MODE)
                {
                    // SLOW2 SCAN MODE
                    m_wndScanControlDlg.OnBnClickedScanControlSlow2ScanBtn();
                }

                // PHOTO IMAGE 원본 백업 ( BITMAP DATA )
                memcpy(m_p2ndOrgBitmapBits, m_p2ndBitmapBits, m_bm2ndBitmap.bmWidthBytes * m_bm2ndBitmap.bmHeight);

                //
                // 히스토그램 그래프 보기
                //
                double[] dHisto = NewArray<double>(256);
                int nCMax = 0;
                memset(dHisto, 0x00, sizeof(double) * 256);
                // 히스토그램 계산
                m_pAssistPublicTool.HistogramNormalize(ref m_2ndImage1280x960Bitmap, dHisto, ref nCMax);
                // 히스토그램 그래프 그리기
                m_pAssistPublicTool.DrawHistogramGraph(m_p4thImageDC, ref m_4thImageMemDC, dHisto, nCMax, 256);
                // HISTOGRAM ANALYSIS CONTROL
                m_nHistogramAnalysisContrasrt = 50;
                m_nHistogramAnalysisBright    = 50;
                m_nHistogramAnalysisGamma     = 50;
                //
                //
                //

                // 2020.09.21 : PHOTO FILTER #1 FLAG 복구
                if (m_SemSEScanControlPublicData.m_bPhotoFilter1SelectFlag != m_SemSEScanControlPublicData.m_bOldPhotoFilter1SelectFlag)
                {
                    // 원래 필터 FLAG MODE로 복구
                    m_SemSEScanControlPublicData.m_bPhotoFilter1SelectFlag = m_SemSEScanControlPublicData.m_bOldPhotoFilter1SelectFlag;
                    // PHOTO FILTER SELECT BUTTON REDRAW
                    m_wndScanControlDlg.LoadPhotoFilterSelectButtonBitmapImage(m_SemSEScanControlPublicData.m_bPhotoFilter1SelectFlag, m_SemSEScanControlPublicData.m_bPhotoFilter2SelectFlag, m_SemSEScanControlPublicData.m_bPhotoFilter3SelectFlag, m_SemSEScanControlPublicData.m_bPhotoFilter4SelectFlag);
                    m_wndScanControlDlg.m_bPhotoFilter1SelectFlag    = m_SemSEScanControlPublicData.m_bPhotoFilter1SelectFlag;
                    m_wndScanControlDlg.m_bOldPhotoFilter1SelectFlag = m_SemSEScanControlPublicData.m_bPhotoFilter1SelectFlag;
                }

                // 2021.04.14
                // MONOGRAM TOOLS : SCAN 모드 재설정
                MonogramToolsSetScanModeExecute(); // 원본 theApp.MonogramToolsSetScanModeExecute()

            }

            //
            //
            //


            return RET_SUCCESS_OK;
        }

        // ---- NanoImpact2020cfeDlg.cpp:15052 ----
        //
        // SEM SE Photo Scan 6 Image Data 2560x1920 : 수신된 데이터 이미지 업데이트 ( DISPLAY ) 처리
        //
        public uint SemSEPhotoScan6ImageDataUpdateProcessing()
        {
            string strText = "";
            int   n = 0;
            int   k = 0;
            int   nReadPointer = 0;
            int   nAverPointer = 0;
            byte  btData       = 0;
            int   nDataSize    = 0;
            uint dwBytesRead  = 0;
            byte  btRED        = 0;
            byte  btGREEN      = 0;
            byte  btBLUE       = 0;
            int   nAverIndex   = 0;
            int   nTotalValue  = 0;
            int   nAverValue   = 0;
            int  bImageUpdate = FALSE;
            int  bCompleted   = FALSE;

            //
            // SE Photo Scan 6 Image Data 프로세싱
            //

            // 처리할 DATA 크기 확인
            if (m_nSemSEScanImageSaveCount < 1)
            {
                // 저장버퍼 : 초기화
                m_nSemSEScanImageSaveStart = 0; // 저장버퍼 현재위치
                m_nSemSEScanImageSaveCount = 0; // 저장버퍼 현재크기

                return 0;
            }

            // 처리할 DATA 크기 설정 : 저장된 DATA 모두 처리
            dwBytesRead = (uint)m_nSemSEScanImageSaveCount;

            // 처리할 SEM SE PHOTO SCAN IMAGE DATA 복사
            memcpy(m_pSemSEScanImageThreadBuffer, m_pSemSEScanImageReadBuffer, dwBytesRead);

            // 저장버퍼 CLEAR
            memset(m_pSemSEScanImageReadBuffer, 0x00, dwBytesRead);// m_nSemSEScanImageDataMax);
            // 저장버퍼 : 초기화
            m_nSemSEScanImageSaveStart = 0; // 저장버퍼 현재위치
            m_nSemSEScanImageSaveCount = 0; // 저장버퍼 현재크기


            //
            //
            //

            for (n = 0; n < (int)dwBytesRead; n++)
            {
                if (m_pSemSEScanImageThreadBuffer[n] == 0xFF)
                {
                    // FRAME START
                    m_SemSEScanControlPublicData.m_nSEDrawXPositionB = 0;       // SE PHOTO SCAN IMAGE BUFFER : X Draw Position
                    m_SemSEScanControlPublicData.m_nSEDrawYPositionB = 0;       // SE PHOTO SCAN IMAGE BUFFER : Y Draw Position
                    m_bSemSEFrameFirstCatch = TRUE;                             // SE PHOTO SCAN IMAGE READ BUFFER : FRAME 처음 읽기 처리 완료

                    // SCAN IMAGE UPDATE (DISPLAY)
                    bImageUpdate = TRUE;

                    // FRAME COUNT
                    m_SemSEScanControlPublicData.m_nSEFrameCount++;
                    if (m_SemSEScanControlPublicData.m_nSEFrameCount > 1)
                    {
                        // PHOTO SCAN CALIBRATION MODE FLAG CHECK?
                        if (m_bPhotoCalibrationFlag == FALSE)
                        {
                            // PHOTO MODE 스캔은 1 프레임만 스캔한 후, 이전 스캔모드로 돌아간다
                            bCompleted = TRUE;
                            // for문 종료
                            break;
                        }
                        else
                        {
                            // PHOTO CALIBRATION MODE 스캔은 프레임을 연속 스캔한다.
                            bCompleted = FALSE;
                            // FRAME COUNT
                            m_SemSEScanControlPublicData.m_nSEFrameCount = 1;
                        }
                    }

                    // BITMAP IMAGE WEIGHT AVERAGE COUNT
                    m_SemSEScanControlPublicData.m_nSEAverageCount = 1;
                    m_SemSEScanControlPublicData.m_nSEAverageIndex = 0;

                    // FRAME DATA CHECK ( 0x08 보다 작은 DATA가 연속 2개 들어온 경우 )
                    if ((int)dwBytesRead > n + 2 && m_pSemSEScanImageThreadBuffer[n + 1] < 0x08 && m_pSemSEScanImageThreadBuffer[n + 2] < 0x08)
                    {
                        // DATA를 무시한다
                        n += 2;
                    }

                }
                else if (m_pSemSEScanImageThreadBuffer[n] == 0x00)    // Line Start Code
                {
                    // FRAME 시작 문자를 읽기 전에는 모든 이미지 DATA를 버린다
                    if (m_bSemSEFrameFirstCatch == FALSE) continue;

                    // TEST
                    strText = Format("%d,%d", m_SemSEScanControlPublicData.m_nSEDrawXPositionB, m_SemSEScanControlPublicData.m_nSEDrawYPositionB);
                    m_wndDetailControlDlg.SetClientInfomation04Text(strText);// m_ClientInfoText4.SetWindowTextW(strText);
                    
                    //
                    // Noise 축소를 위해 Photo Image 표시전에 Filtering
                    // MedeanFilter or WeightMean 사용
                    //
                    // SCAN CLIBRATION : PHOTO FILTER 선택
                    if (AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_PHOTO_IMAGE_FILTER] == 1)
                    {
                        // 2020.06.05 : PHOTO FILTER #1
                        if(m_SemSEScanControlPublicData.m_bPhotoFilter1SelectFlag==TRUE)
                        {
                            // PHOTO IMAGE FILETR : 미디언 필터
                            if (m_SemSEScanControlPublicData.m_nSEDrawYPositionB > 1 && m_SemSEScanControlPublicData.m_nSEDrawYPositionB < m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight)
                            {
                                int[] nPixelData = NewArray<int>(9);
                                int nFilterPointer = 0;
                                for (int nX = 1; nX < m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth - 1; nX++)
                                {
                                    // FILTER를 위한 PIXEL DATA BITMAP 위치 설정 계산 : 0~8번,                                                                                             해당 위치의 이미지 값 적용
                                    nFilterPointer = ((nX-1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-2)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[0] = m_pSemSEScanImageBitmapBufferB[nFilterPointer];
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-2)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[1] = m_pSemSEScanImageBitmapBufferB[nFilterPointer];
                                    nFilterPointer = ((nX+1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-2)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[2] = m_pSemSEScanImageBitmapBufferB[nFilterPointer];
                                    nFilterPointer = ((nX-1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[3] = m_pSemSEScanImageBitmapBufferB[nFilterPointer];
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[4] = m_pSemSEScanImageBitmapBufferB[nFilterPointer];
                                    nFilterPointer = ((nX+1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[5] = m_pSemSEScanImageBitmapBufferB[nFilterPointer];
                                    nFilterPointer = ((nX-1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-0)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[6] = m_pSemSEScanImageBitmapBufferB[nFilterPointer];
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-0)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[7] = m_pSemSEScanImageBitmapBufferB[nFilterPointer];
                                    nFilterPointer = ((nX+1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-0)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[8] = m_pSemSEScanImageBitmapBufferB[nFilterPointer];

                                    // Insertion sort
                                    m_pAssistDeviceTool.InsertSort(nPixelData, 9);

                                    // FILTER 처리를 위한 버퍼(FILTER#1)에 값 설정
                                    k = ((nX - 0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB - 1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth));
                                    m_pSemSEScanImageBitmapBufferBF1[k] = (byte)nPixelData[4];

                                    // PHOTO SCAN IMAGE DATA PIXEL POSITION
                                    k = (nX+m_SemSEScanControlPublicData.m_nSEBitmapXOffsetB) + (((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)+m_SemSEScanControlPublicData.m_nSEBitmapYOffsetB)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth);

                                    // PIXEL DATA SET
                                    m_p2ndImageRgb[k].rgbRed   = (byte)nPixelData[4];
                                    m_p2ndImageRgb[k].rgbGreen = (byte)nPixelData[4];
                                    m_p2ndImageRgb[k].rgbBlue  = (byte)nPixelData[4];

                                }
                            }
                        }
                        //
                        //
                        //
                        // 2020.06.05 : PHOTO FILTER #2
                        if(m_SemSEScanControlPublicData.m_bPhotoFilter2SelectFlag==TRUE)
                        {
                            // PHOTO IMAGE FILETR : 샤프닝 필터
                            if (m_SemSEScanControlPublicData.m_nSEDrawYPositionB > 1 && m_SemSEScanControlPublicData.m_nSEDrawYPositionB < m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight)
                            {
                                // 이미지 선명도 필터 ( 샤프닝 기법 ) : 5개 PIXEL 이용
                                // 현재 픽셀의 값에 5를 곱한 후 상하좌우에 있는 픽셀의 값들을 빼준다. 만약 5개 픽셀의 값이 모두 동일하다면 아무 변화가 없을 것이다. 반면 현재 픽셀의 값이 주변 픽셀의 값보다 크다면 그것은 더 부각될 것이고, 작다면 더 작아질 것이다. 결과적으로 엣지가 부각된다.
                                int[] nPixelData = NewArray<int>(5);
                                int nFilterPointer = 0;
                                int nFilterValue   = 0;
                                for (int nX = 1; nX < m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth - 1; nX++)
                                {
                                    // FILTER를 위한 PIXEL DATA BITMAP 위치 설정 계산 : 중심PIXEL, LEFT, UP, RIGHT, DOWN PIXEL 이용                                                        해당 위치의 이미지 값 적용
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[0] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // 중심  PIXEL
                                    nFilterPointer = ((nX-1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[1] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // LEFT  PIXEL
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-2)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[2] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // UP    PIXEL
                                    nFilterPointer = ((nX+1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[3] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // RIGHT PIXEL
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB+0)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[4] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // DOWN  PIXEL

                                    // 이미지 선명도 필터 계산 = 중심PIXEL(x5배) - (나머지 4개 픽셀값) : 중심PIXEL 값이 주변보다 높으면 더높게, 낮으면 더낮게 만든다, 주변과 같으면 같은값이 설정된다
                                    nFilterValue = (nPixelData[0] * 5) - nPixelData[1] - nPixelData[2] - nPixelData[3] - nPixelData[4];
                                    if (nFilterValue < 0  ) nFilterValue = 0;
                                    if (nFilterValue > 255) nFilterValue = 255;

                                    // FILTER 처리를 위한 버퍼(FILTER#2)에 값 설정
                                    k = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth));
                                    m_pSemSEScanImageBitmapBufferBF2[k] = (byte)nFilterValue;

                                    // PHOTO SCAN IMAGE DATA PIXEL POSITION
                                    k = (nX+m_SemSEScanControlPublicData.m_nSEBitmapXOffsetB) + (((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)+m_SemSEScanControlPublicData.m_nSEBitmapYOffsetB)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth);

                                    // PIXEL DATA SET
                                    m_p2ndImageRgb[k].rgbRed   = (byte)nFilterValue;
                                    m_p2ndImageRgb[k].rgbGreen = (byte)nFilterValue;
                                    m_p2ndImageRgb[k].rgbBlue  = (byte)nFilterValue;
                                }
                            }
                        }
                        //
                        //
                        //
                        /*
                        // 2020.06.05 : PHOTO FILTER #3
                        if(m_SemSEScanControlPublicData.m_bPhotoFilter3SelectFlag==TRUE)
                        {
                            // PHOTO IMAGE FILETR : 하이부스트 샤프닝 필터
                            if (m_SemSEScanControlPublicData.m_nSEDrawYPositionB > 1 && m_SemSEScanControlPublicData.m_nSEDrawYPositionB < m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight)
                            {
                                // 이미지 선명도 필터 ( 샤프닝 기법 ) : 9개 PIXEL 이용
                                // 현재 픽셀의 값에 5를 곱한 후 상하좌우에 있는 픽셀의 값들을 빼준다. 만약 5개 픽셀의 값이 모두 동일하다면 아무 변화가 없을 것이다. 반면 현재 픽셀의 값이 주변 픽셀의 값보다 크다면 그것은 더 부각될 것이고, 작다면 더 작아질 것이다. 결과적으로 엣지가 부각된다.
                                int    nPixelData[9];
                                int    nFilterPointer = 0;
                                int    nFilterValue   = 0;
                                double dFilterValue   = 0;
                                double dFilterAlpha   = 9.0; // 중심픽셀 9배
                                for (int nX = 1; nX < m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth - 1; nX++)
                                {
                                    // FILTER를 위한 PIXEL DATA BITMAP 위치 설정 계산 : 중심PIXEL, LEFT, UP, RIGHT, DOWN PIXEL 이용                                                        해당 위치의 이미지 값 적용
                                    // FILTER를 위한 PIXEL DATA BITMAP 위치 설정 계산 : 0~8번,                                                                                             해당 위치의 이미지 값 적용
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[0] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // 중심PIXEL
                                    nFilterPointer = ((nX-1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-2)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[1] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // X-1, Y-1
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-2)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[2] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // X-0, Y-1
                                    nFilterPointer = ((nX+1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-2)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[3] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // X+1, Y-1
                                    nFilterPointer = ((nX-1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[4] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // X-1, Y-0
                                    nFilterPointer = ((nX+1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[5] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // X+1, Y-0
                                    nFilterPointer = ((nX-1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-0)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[6] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // X-1, Y+1
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-0)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[7] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // X-0, Y+1
                                    nFilterPointer = ((nX+1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-0)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[8] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // X+1, Y+1

                                    // 이미지 선명도 필터 계산 = 중심PIXEL(x5배) - (나머지 4개 픽셀값) : 중심PIXEL 값이 주변보다 높으면 더높게, 낮으면 더낮게 만든다, 주변과 같으면 같은값이 설정된다
                                    dFilterValue = (dFilterAlpha*(double)nPixelData[0]) - (double)nPixelData[1] - (double)nPixelData[2] - (double)nPixelData[3] - (double)nPixelData[4] - (double)nPixelData[5] - (double)nPixelData[6] - (double)nPixelData[7] - (double)nPixelData[8];
                                    nFilterValue = (int)(dFilterValue + 0.5);
                                    if (nFilterValue < 0  ) nFilterValue = 0;
                                    if (nFilterValue > 255) nFilterValue = 255;

                                    // FILTER 처리를 위한 버퍼(FILTER#2)에 값 설정
                                    k = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth));
                                    m_pSemSEScanImageBitmapBufferBF2[k] = (BYTE)nFilterValue;

                                    // PHOTO SCAN IMAGE DATA PIXEL POSITION
                                    k = (nX+m_SemSEScanControlPublicData.m_nSEBitmapXOffsetB) + (((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)+m_SemSEScanControlPublicData.m_nSEBitmapYOffsetB)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth);

                                    // PIXEL DATA SET
                                    m_p2ndImageRgb[k].rgbRed   = (BYTE)nFilterValue;
                                    m_p2ndImageRgb[k].rgbGreen = (BYTE)nFilterValue;
                                    m_p2ndImageRgb[k].rgbBlue  = (BYTE)nFilterValue;
                                }
                            }
                        }
                        */
                        /*
                        // 2020.06.10 : PHOTO FILTER #3
                        if(m_SemSEScanControlPublicData.m_bPhotoFilter3SelectFlag==TRUE)
                        {
                            // PHOTO IMAGE FILETR : 언 샤프닝 필터
                            if (m_SemSEScanControlPublicData.m_nSEDrawYPositionB > 1 && m_SemSEScanControlPublicData.m_nSEDrawYPositionB < m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight)
                            {
                                // 이미지 선명도 필터 ( 언 샤프닝 기법 ) : 언샤프PIXEL값 = 원본PIXEL값 - Filter1번PIXEL값 --> 결과PIXEL값 = 원본PIXEL값 + 언샤프PIXEL값
                                int    nPixelData[3];
                                int    nFilterValue = 0;
                                for (int nX = 1; nX < m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth - 1; nX++)
                                {
                                    // PIXEL 위치 계산
                                    k = (nX+((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth));

                                    // 원본 PIXEL값
                                    nPixelData[0] = m_pSemSEScanImageBitmapBufferB[k];

                                    // 언샤프 PIXEL값 계산 : 언샤프    = 원본PIXEL - FILTER1(부드러운이미지)PIXEL
                                    nPixelData[1] = m_pSemSEScanImageBitmapBufferB[k] - m_pSemSEScanImageBitmapBufferBF1[k];

                                    // 언샤프 결과값  계산 : 결과PIXEL = 언샤프PIXEL + 원본PIXEL
                                    nPixelData[2] = m_pSemSEScanImageBitmapBufferB[k] + nPixelData[1];

                                    // 필터 결과값
                                    nFilterValue = nPixelData[2];
                                    if (nFilterValue < 0  ) nFilterValue = 0;
                                    if (nFilterValue > 255) nFilterValue = 255;

                                    // FILTER 처리를 위한 버퍼(FILTER#2)에 값 설정
                                    k = (nX+((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth));
                                    m_pSemSEScanImageBitmapBufferBF2[k] = (BYTE)nFilterValue;

                                    // PHOTO SCAN IMAGE DATA PIXEL POSITION
                                    k = (nX+m_SemSEScanControlPublicData.m_nSEBitmapXOffsetB) + (((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)+m_SemSEScanControlPublicData.m_nSEBitmapYOffsetB)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth);

                                    // PIXEL DATA SET
                                    m_p2ndImageRgb[k].rgbRed   = (BYTE)nFilterValue;
                                    m_p2ndImageRgb[k].rgbGreen = (BYTE)nFilterValue;
                                    m_p2ndImageRgb[k].rgbBlue  = (BYTE)nFilterValue;
                                }
                            }
                        }
                        */
                        // 2020.06.10 : PHOTO FILTER #3
                        if(m_SemSEScanControlPublicData.m_bPhotoFilter3SelectFlag==TRUE)
                        {
                            // PHOTO IMAGE FILETR : 샤프닝 필터
                            if (m_SemSEScanControlPublicData.m_nSEDrawYPositionB > 1 && m_SemSEScanControlPublicData.m_nSEDrawYPositionB < m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight)
                            {
                                // 이미지 선명도 필터 ( 샤프닝 기법 ) : 5개 PIXEL 이용
                                int[] nPixelData = NewArray<int>(5);
                                int nFilterPointer = 0;
                                int nFilterValue   = 0;
                                double dTemp = 0.0;
                                for (int nX = 1; nX < m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth - 1; nX++)
                                {
                                    // FILTER를 위한 PIXEL DATA BITMAP 위치 설정 계산 : 중심PIXEL, LEFT, UP, RIGHT, DOWN PIXEL 이용                                                        해당 위치의 이미지 값 적용
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[0] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // 중심  PIXEL
                                    nFilterPointer = ((nX-1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[1] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // LEFT  PIXEL
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-2)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[2] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // UP    PIXEL
                                    nFilterPointer = ((nX+1) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[3] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // RIGHT PIXEL
                                    nFilterPointer = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB+0)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)); nPixelData[4] = m_pSemSEScanImageBitmapBufferBF1[nFilterPointer]; // DOWN  PIXEL

                                    // 이미지 선명도 필터 계산 = 중심PIXEL - (나머지4개 픽셀값) : 중심PIXEL 값이 주변보다 높으면 더높게, 낮으면 더낮게 만든다, 주변과 같으면 같은값이 설정된다
                                    dTemp = ((8.0/4.0)*nPixelData[0]) + ((-1.0/4.0)*nPixelData[1]) + ((-1.0/4.0)*nPixelData[2]) + ((-1.0/4.0)*nPixelData[3]) + ((-1.0/4.0)*nPixelData[4]);
                                    nFilterValue = (int)(dTemp + 0.5);
                                    if (nFilterValue < 0  ) nFilterValue = 0;
                                    if (nFilterValue > 255) nFilterValue = 255;

                                    // FILTER 처리를 위한 버퍼(FILTER#2)에 값 설정
                                    k = ((nX-0) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth));
                                    m_pSemSEScanImageBitmapBufferBF2[k] = (byte)nFilterValue;

                                    // PHOTO SCAN IMAGE DATA PIXEL POSITION
                                    k = (nX+m_SemSEScanControlPublicData.m_nSEBitmapXOffsetB) + (((m_SemSEScanControlPublicData.m_nSEDrawYPositionB-1)+m_SemSEScanControlPublicData.m_nSEBitmapYOffsetB)*m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth);

                                    // PIXEL DATA SET
                                    m_p2ndImageRgb[k].rgbRed   = (byte)nFilterValue;
                                    m_p2ndImageRgb[k].rgbGreen = (byte)nFilterValue;
                                    m_p2ndImageRgb[k].rgbBlue  = (byte)nFilterValue;
                                }
                            }
                        }
                        //
                        //
                        //
                    }
                    else
                    {
                        // 처리안함
                    }
                    //
                    //
                    //

                    // LINE START : X POSITION RESET
                    m_SemSEScanControlPublicData.m_nSEDrawXPositionB = 0;
                    // LINE START : Y POSITION 증가 및 확인
                    if (m_SemSEScanControlPublicData.m_nSEDrawYPositionB < m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight - 1)
                    {
                        // FRAME : Y Position 증가
                        m_SemSEScanControlPublicData.m_nSEDrawYPositionB++;
                    }
                    else
                    {
                        // PHOTO SCAN CALIBRATION MODE FLAG CHECK?
                        if (m_bPhotoCalibrationFlag == FALSE)
                        {
                            // PHOTO6 SCAN MODE의 LINE 수 보다 크기 때문에 스캔을 종료한다
                            bCompleted = TRUE;
                            // for문 종료
                            break;
                        }
                        else
                        {
                            // PHOTO CALIBRATION MODE 스캔은 프레임을 연속 스캔한다.
                            bCompleted = FALSE;
                            // FRAME COUNT
                            m_SemSEScanControlPublicData.m_nSEFrameCount = 1;
                        }
                    }

                }
                else
                {
                    // FRAME 시작 문자를 읽기 전에는 모든 이미지 DATA를 버린다
                    if (m_bSemSEFrameFirstCatch == FALSE) continue;

                    // SCAN IMAGE UPDATE (DISPLAY)
                    //bImageUpdate = TRUE;
                    // 2021.05.10
                    // 이미지 업데이트 속도 개선을 위해 X/Y PIXEL 처리시에는 이미지를 갱신 하지 않는다
                    bImageUpdate = FALSE;
                    if (n == (int)(dwBytesRead-1))
                    {
                        bImageUpdate = TRUE;
                    }

                    // DRAW X POSITION 확인 ( 0 ~ 가로폭 미만)
                    if (m_SemSEScanControlPublicData.m_nSEDrawXPositionB < m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)
                    {
                        // PHOTO SCAN IMAGE DATA ( 1 BYTE )
                        btData = m_pSemSEScanImageThreadBuffer[n];

                        // PIXEL DATA BITMAP 위치 설정 계산
                        nAverPointer = (m_SemSEScanControlPublicData.m_nSEDrawXPositionB + m_SemSEScanControlPublicData.m_nSEBitmapXOffsetB) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB + m_SemSEScanControlPublicData.m_nSEBitmapYOffsetB) * 2560);
                        // 해당 위치의 이미지 값 적용
                        m_pSemSEScanImageBitmapBufferB[nAverPointer + m_SemSEScanControlPublicData.m_nSEAverageIndex] = btData;
                        // 필터 처리를 위한 해당 위치의 이미지 값 적용
                        m_pSemSEScanImageBitmapBufferBF1[nAverPointer + m_SemSEScanControlPublicData.m_nSEAverageIndex] = btData;
                        m_pSemSEScanImageBitmapBufferBF2[nAverPointer + m_SemSEScanControlPublicData.m_nSEAverageIndex] = btData;

                        // PHOTO SCAN IMAGE DATA PIXEL POSITION
                        k = (m_SemSEScanControlPublicData.m_nSEDrawXPositionB + m_SemSEScanControlPublicData.m_nSEBitmapXOffsetB) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB + m_SemSEScanControlPublicData.m_nSEBitmapYOffsetB) * 2560);

                        // PIXEL DATA SET
                        m_p2ndImageRgb[k].rgbRed   = btData;
                        m_p2ndImageRgb[k].rgbGreen = btData;
                        m_p2ndImageRgb[k].rgbBlue  = btData;

                        // FRAME : X Position 증가
                        m_SemSEScanControlPublicData.m_nSEDrawXPositionB++;

                    }
                } // 식별문자 코드 처리 : 0xFF, 0xFE, 0xFD, 0xFC, 0x00, 이미지데이터, 등등 if()문 처리
            } // 수신한 문자 처리 for()문 처리

            //
            //
            //

            // FRAME 시작 문자를 읽기 전에는 모든 이미지 DATA를 버린다
            if (m_bSemSEFrameFirstCatch == FALSE) return RET_SUCCESS_OK;

            // 2nd PIXEL BITMAP DATA SET
            m_2ndImage2560x1920Bitmap.SetBitmapBits(m_bm2ndBitmap.bmWidthBytes * m_bm2ndBitmap.bmHeight, m_p2ndBitmapBits);

            // SE PHOTO SCAN IMAGE DATA DRAW UPDATE ( DISPLAY ) : 1 PIXEL 단위 데이터 처리
            // 2019.09.18 이미지 업데이트(REDRAW)는 타이머를 이용하여 정기적으로 실행한다
            // PHOTO SCAN BITMAP IMAGE UPDATE
            if (bImageUpdate == TRUE)
            {
                // SCAN IMAGE UPDATE (DISPLAY)
                SemSEPhotoScan6ImageDataUpdateControl();
            }

            //
            //
            //

            // PHOTO MODE 스캔은 1 프레임만 스캔한 후, 이전 스캔모드로 돌아간다
            if (bCompleted == TRUE)
            {
                // 포토스캔 종료
                m_SemSEScanControlPublicData.m_bPhotoScanCompleted = TRUE;

                // 이전 스캔모드 실행
                if (m_SemSEScanControlPublicData.m_nSaveScanMode == RED_SCAN_MODE)
                {
                    // RED SCAN MODE
                    m_wndScanControlDlg.OnBnClickedScanControlRedScanBtn();
                }
                else if (m_SemSEScanControlPublicData.m_nSaveScanMode == FAST_SCAN_MODE)
                {
                    // FAST SCAN MODE
                    m_wndScanControlDlg.OnBnClickedScanControlFastScanBtn();
                }
                else if (m_SemSEScanControlPublicData.m_nSaveScanMode == SLOW1_SCAN_MODE)
                {
                    // SLOW1 SCAN MODE
                    m_wndScanControlDlg.OnBnClickedScanControlSlow1ScanBtn();
                }
                else if (m_SemSEScanControlPublicData.m_nSaveScanMode == SLOW2_SCAN_MODE)
                {
                    // SLOW2 SCAN MODE
                    m_wndScanControlDlg.OnBnClickedScanControlSlow2ScanBtn();
                }

                // PHOTO IMAGE 원본 백업 ( BITMAP DATA )
                memcpy(m_p2ndOrgBitmapBits, m_p2ndBitmapBits, m_bm2ndBitmap.bmWidthBytes * m_bm2ndBitmap.bmHeight);

                //
                // 히스토그램 그래프 보기
                //
                double[] dHisto = NewArray<double>(256);
                int nCMax = 0;
                memset(dHisto, 0x00, sizeof(double) * 256);
                // 히스토그램 계산
                m_pAssistPublicTool.HistogramNormalize(ref m_2ndImage2560x1920Bitmap, dHisto, ref nCMax);
                // 히스토그램 그래프 그리기
                m_pAssistPublicTool.DrawHistogramGraph(m_p4thImageDC, ref m_4thImageMemDC, dHisto, nCMax, 256);
                // HISTOGRAM ANALYSIS CONTROL
                m_nHistogramAnalysisContrasrt = 50;
                m_nHistogramAnalysisBright    = 50;
                m_nHistogramAnalysisGamma     = 50;
                //
                //
                //

                // 2020.09.21 : PHOTO FILTER #1 FLAG 복구
                if (m_SemSEScanControlPublicData.m_bPhotoFilter1SelectFlag != m_SemSEScanControlPublicData.m_bOldPhotoFilter1SelectFlag)
                {
                    // 원래 필터 FLAG MODE로 복구
                    m_SemSEScanControlPublicData.m_bPhotoFilter1SelectFlag = m_SemSEScanControlPublicData.m_bOldPhotoFilter1SelectFlag;
                    // PHOTO FILTER SELECT BUTTON REDRAW
                    m_wndScanControlDlg.LoadPhotoFilterSelectButtonBitmapImage(m_SemSEScanControlPublicData.m_bPhotoFilter1SelectFlag, m_SemSEScanControlPublicData.m_bPhotoFilter2SelectFlag, m_SemSEScanControlPublicData.m_bPhotoFilter3SelectFlag, m_SemSEScanControlPublicData.m_bPhotoFilter4SelectFlag);
                    m_wndScanControlDlg.m_bPhotoFilter1SelectFlag    = m_SemSEScanControlPublicData.m_bPhotoFilter1SelectFlag;
                    m_wndScanControlDlg.m_bOldPhotoFilter1SelectFlag = m_SemSEScanControlPublicData.m_bPhotoFilter1SelectFlag;
                }

                // 2021.04.14
                // MONOGRAM TOOLS : SCAN 모드 재설정
                MonogramToolsSetScanModeExecute(); // 원본 theApp.MonogramToolsSetScanModeExecute()

            }

            //
            //
            //

            return RET_SUCCESS_OK;
        }

        // ---- NanoImpact2020cfeDlg.cpp:15564 ----
        //
        // SEM SE Photo Scan 3 ( Average Mode ) Image Data 1280x960 : 수신된 데이터 이미지 업데이트 ( DISPLAY ) 처리
        //
        public uint SemSEPhotoScanOverlapModeImageDataUpdateProcessing()
        {
            string strText    = "";
            int   n            = 0;
            int   k            = 0;
            int   nReadPointer = 0;
            int   nAverPointer = 0;
            byte  btData       = 0;
            int   nDataSize    = 0;
            uint dwBytesRead  = 0;
            byte  btRED        = 0;
            byte  btGREEN      = 0;
            byte  btBLUE       = 0;
            int   nAverIndex   = 0;
            int   nTotalValue  = 0;
            int   nAverValue   = 0;
            int  bImageUpdate = FALSE;
            int  bCompleted   = FALSE;

            //
            // SE Photo Scan 3/4/5 Image Data 프로세싱
            //

            // 처리할 DATA 크기 확인
            if (m_nSemSEScanImageSaveCount < 1)
            {
                // 저장버퍼 : 초기화
                m_nSemSEScanImageSaveStart = 0; // 저장버퍼 현재위치
                m_nSemSEScanImageSaveCount = 0; // 저장버퍼 현재크기

                return 0;
            }

            // 처리할 DATA 크기 설정 : 저장된 DATA 모두 처리
            dwBytesRead = (uint)m_nSemSEScanImageSaveCount;

            // 처리할 SEM SE PHOTO SCAN IMAGE DATA 복사
            memcpy(m_pSemSEScanImageThreadBuffer, m_pSemSEScanImageReadBuffer, dwBytesRead);

            // 저장버퍼 CLEAR
            memset(m_pSemSEScanImageReadBuffer, 0x00, dwBytesRead);// m_nSemSEScanImageDataMax);
            // 저장버퍼 : 초기화
            m_nSemSEScanImageSaveStart = 0; // 저장버퍼 현재위치
            m_nSemSEScanImageSaveCount = 0; // 저장버퍼 현재크기

            //
            //
            //

            for (n = 0; n < (int)dwBytesRead; n++)
            {
                if (m_pSemSEScanImageThreadBuffer[n] == 0xFF)
                {
                    // FRAME START
                    m_SemSEScanControlPublicData.m_nSEDrawXPositionB = 0;       // SE PHOTO SCAN IMAGE BUFFER : X Draw Position
                    m_SemSEScanControlPublicData.m_nSEDrawYPositionB = 0;       // SE PHOTO SCAN IMAGE BUFFER : Y Draw Position
                    m_bSemSEFrameFirstCatch = TRUE;                             // SE PHOTO SCAN IMAGE READ BUFFER : FRAME 처음 읽기 처리 완료

                    // SCAN IMAGE UPDATE (DISPLAY)
                    bImageUpdate = TRUE;

                    // FRAME COUNT
                    m_SemSEScanControlPublicData.m_nSEFrameCount++;
                    if (m_SemSEScanControlPublicData.m_nSEFrameCount > m_SemSEScanControlPublicData.m_nSEAverageNumber)
                    {
                        // PHOTO AVERAGE MODE 스캔은 AVERAGE 갯수 많큼 프레임 스캔한 후, 이전 스캔모드로 돌아간다
                        bCompleted = TRUE;
                        // for문 종료
                        break;
                    }

                    // BITMAP IMAGE WEIGHT AVERAGE COUNT RESET
                    if (m_SemSEScanControlPublicData.m_nSEAverageCount == 0)
                    {
                        m_SemSEScanControlPublicData.m_nSEAverageCount = 1;
                        m_SemSEScanControlPublicData.m_nSEAverageIndex = 0;
                    }
                    else
                    {
                        m_SemSEScanControlPublicData.m_nSEAverageCount++;
                        m_SemSEScanControlPublicData.m_nSEAverageIndex++;
                    }

                    // PHOTO AVERAGE COUNT TEXT
                    strText = Format("%d", m_SemSEScanControlPublicData.m_nSEAverageCount);
                    m_wndScanControlDlg.SetPhotoAverageCountText(strText);

                    // FRAME DATA CHECK ( 0x08 보다 작은 DATA가 연속 2개 들어온 경우 )
                    if ((int)dwBytesRead > n + 2 && m_pSemSEScanImageThreadBuffer[n + 1] < 0x08 && m_pSemSEScanImageThreadBuffer[n + 2] < 0x08)
                    {
                        // DATA를 무시한다
                        n += 2;
                    }

                }
                else if (m_pSemSEScanImageThreadBuffer[n] == 0x00)    // Line Start Code
                {
                    // FRAME 시작 문자를 읽기 전에는 모든 이미지 DATA를 버린다
                    if (m_bSemSEFrameFirstCatch == FALSE) continue;

                    // SCAN IMAGE UPDATE (DISPLAY)
                    bImageUpdate = TRUE;

                    // TEST
                    strText = Format("%d,%d", m_SemSEScanControlPublicData.m_nSEDrawXPositionB, m_SemSEScanControlPublicData.m_nSEDrawYPositionB);
                    m_wndDetailControlDlg.SetClientInfomation04Text(strText);// m_ClientInfoText4.SetWindowTextW(strText);

                    // LINE START : X POSITION RESET
                    m_SemSEScanControlPublicData.m_nSEDrawXPositionB = 0;
                    // LINE START : Y POSITION 증가 및 확인
                    if (m_SemSEScanControlPublicData.m_nSEDrawYPositionB < m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight - 1)
                    {
                        // FRAME : Y Position 증가
                        m_SemSEScanControlPublicData.m_nSEDrawYPositionB++;
                    }
                    else
                    {
                        // PHOTO3/4/5 SCAN MODE의 LINE 수 보다 크기 때문에 0으로 초기화 한다
                        m_SemSEScanControlPublicData.m_nSEDrawYPositionB = 0;
                    }

                }
                else
                {
                    // FRAME 시작 문자를 읽기 전에는 모든 이미지 DATA를 버린다
                    if (m_bSemSEFrameFirstCatch == FALSE) continue;

                    // SCAN IMAGE UPDATE (DISPLAY)
                    //bImageUpdate = TRUE;
                    // 2021.05.10
                    // 이미지 업데이트 속도 개선을 위해 X/Y PIXEL 처리시에는 이미지를 갱신 하지 않는다
                    bImageUpdate = FALSE;
                    if (n == (int)(dwBytesRead-1))
                    {
                        bImageUpdate = TRUE;
                    }

                    // DRAW X POSITION 확인 ( 0 ~ 가로폭 미만)
                    if (m_SemSEScanControlPublicData.m_nSEDrawXPositionB < m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth)
                    {
                        // PHOTO SCAN IMAGE DATA ( 1 BYTE )
                        btData = m_pSemSEScanImageThreadBuffer[n];

                        // PIXEL DATA BITMAP 위치 설정 계산
                        // BITMAP DATA 배열 순서 : 예) Average:16 일 경우 --> x[0]y[0]a[0],...x[0]y[0]a[15],x[1]y[0]a[0],...x[1]y[0]a[15],x[2]y[0]a[0],...x[2]y[0]a[15],.....x[1279]y[959]a[15]
                        nAverPointer = ((m_SemSEScanControlPublicData.m_nSEDrawXPositionB + m_SemSEScanControlPublicData.m_nSEBitmapXOffsetB) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB + m_SemSEScanControlPublicData.m_nSEBitmapYOffsetB) * 1280)) * MAX_SE_SCAN_IMAGE_AVERAGE;

                        // 해당 위치의 이미지 값 적용
                        m_pSemSEScanImageBitmapBufferB[nAverPointer + m_SemSEScanControlPublicData.m_nSEAverageIndex] = btData;

                        // WEIGHT AVERAGE BITMAP DATA ( BITMAP 이미지 평균값 보정 )
                        if (m_SemSEScanControlPublicData.m_nSEFrameCount > m_SemSEScanControlPublicData.m_nSEAverageNumber)
                        {
                            // 프레임 카운트가 스캔모드에 따른 AVERAGE 카운트를 넘어섰다
                            // BITMAP 이미지 평균값 구하기
                            nTotalValue = 0;
                            nAverValue  = 0;
                            for (nAverIndex = 0; nAverIndex < m_SemSEScanControlPublicData.m_nSEAverageNumber; nAverIndex++)
                            {
                                // BITMAP DATA 합산
                                nTotalValue += m_pSemSEScanImageBitmapBufferB[nAverPointer + nAverIndex];
                            }
                            // BITMAP DATA 평균값
                            nAverValue = nTotalValue / m_SemSEScanControlPublicData.m_nSEAverageNumber;
                            // 결과값 BYTE
                            btData = (byte)nAverValue;
                        }
                        else
                        {
                            // 프레임 카운트가 스캔모드에 따른 AVERAGE 카운트와 같거나 작다
                            // BITMAP 이미지 평균값 구하기
                            nTotalValue = 0;
                            nAverValue  = 0;
                            for (nAverIndex = 0; nAverIndex < m_SemSEScanControlPublicData.m_nSEAverageCount; nAverIndex++)
                            {
                                // BITMAP DATA 합산
                                nTotalValue += m_pSemSEScanImageBitmapBufferB[nAverPointer + nAverIndex];
                            }
                            // BITMAP DATA 평균값
                            nAverValue = nTotalValue / m_SemSEScanControlPublicData.m_nSEAverageCount;
                            // 결과값 BYTE
                            btData = (byte)nAverValue;
                        }

                        // PHOTO SCAN IMAGE DATA PIXEL POSITION
                        k = (m_SemSEScanControlPublicData.m_nSEDrawXPositionB + m_SemSEScanControlPublicData.m_nSEBitmapXOffsetB) + ((m_SemSEScanControlPublicData.m_nSEDrawYPositionB + m_SemSEScanControlPublicData.m_nSEBitmapYOffsetB) * m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth);

                        // PIXEL DATA SET
                        m_p2ndImageRgb[k].rgbRed   = btData;
                        m_p2ndImageRgb[k].rgbGreen = btData;
                        m_p2ndImageRgb[k].rgbBlue  = btData;

                        // FRAME : X Position 증가
                        m_SemSEScanControlPublicData.m_nSEDrawXPositionB++;

                    }
                } // 식별문자 코드 처리 : 0xFF, 0xFE, 0xFD, 0xFC, 0x00, 이미지데이터, 등등 if()문 처리
            } // 수신한 문자 처리 for()문 처리

            //
            //
            //

            // FRAME 시작 문자를 읽기 전에는 모든 이미지 DATA를 버린다
            if (m_bSemSEFrameFirstCatch == FALSE) return RET_SUCCESS_OK;

            // 2nd PIXEL BITMAP DATA SET
            m_2ndImage1280x960Bitmap.SetBitmapBits(m_bm2ndBitmap.bmWidthBytes * m_bm2ndBitmap.bmHeight, m_p2ndBitmapBits);

            // SE PHOTO SCAN IMAGE DATA DRAW UPDATE ( DISPLAY ) : 1 PIXEL 단위 데이터 처리
            // 2019.09.18 이미지 업데이트(REDRAW)는 타이머를 이용하여 정기적으로 실행한다
            // PHOTO SCAN BITMAP IMAGE UPDATE
            if (bImageUpdate == TRUE)
            {
                // SCAN IMAGE UPDATE (DISPLAY)
                SemSEPhotoScan345ImageDataUpdateControl();
            }

            //
            //
            //

            // PHOTO MODE 스캔은 1 프레임만 스캔한 후, 이전 스캔모드로 돌아간다
            if (bCompleted == TRUE)
            {
                // 포토스캔 종료
                m_SemSEScanControlPublicData.m_bPhotoScanCompleted = TRUE;


                //
                // PHOTO OVERLAP : LEVEL COUNT PROCESSING ( LEVEL -> 0 ~ 255 범위이다 )
                // 각 X-Y 픽셀별로 수집된 n개의 해당픽셀(그레이스케일) 데이터를
                // LEVEL별로(BYTE값) 분류 한 후, 가장 많은 카운트의 LEVEL을 최종
                // 그레이 스케일 값으로 설정하는 방식이다.
                //
                // 2020.08.03 : PHOTO FILTER #4 ( OVERLAP COUNT LEVEL )
                // 2020.08.17 : 테스트결과 1개의 프레임을 빠르게 스캔할 경우 PIXEL의 스캔값이 극명하게 갈리는 현상이
                //              발생하여 PIXEL값이 0에 가깝거나, 255에 가까운 현상이 발생하여 아래의 필터링은 사용할 수 없다.
                //              필터링을 사용하면 중간 그레이 스케일이 없는 흰색 이거난 검은색 위주의 PIXEL 들로 채워진다.
                /*
                if (m_SemSEScanControlPublicData.m_bPhotoFilter4SelectFlag == TRUE)
                {
                    BYTE *pLevelCountBuffer = new BYTE[PHOTO_SCAN_345_IMAGE_WIDTH*PHOTO_SCAN_345_IMAGE_HEIGHT * 256];
                    BYTE *pLevelPixelBuffer = new BYTE[PHOTO_SCAN_345_IMAGE_WIDTH*PHOTO_SCAN_345_IMAGE_HEIGHT];
                    memset(pLevelCountBuffer, 0x00, PHOTO_SCAN_345_IMAGE_WIDTH*PHOTO_SCAN_345_IMAGE_HEIGHT * 256);
                    memset(pLevelPixelBuffer, 0x00, PHOTO_SCAN_345_IMAGE_WIDTH*PHOTO_SCAN_345_IMAGE_HEIGHT);
                    int nAverIdx = 0;
                    int nGrayIdx = 0;
                    int nXLevel  = 0;
                    int nYLevel  = 0;
                    int nXOffset = m_SemSEScanControlPublicData.m_nSEBitmapXOffsetB;
                    int nYOffset = m_SemSEScanControlPublicData.m_nSEBitmapYOffsetB;
                    int nMaxW = m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth;
                    int nMaxH = m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight;
                    int nMaxA = m_SemSEScanControlPublicData.m_nSEAverageNumber;
                    int nLPos = 0;
                    int nBPos = 0;
                    for (nYLevel = 0; nYLevel < nMaxH; nYLevel++)
                    {
                        for (nXLevel = 0; nXLevel < nMaxW; nXLevel++)
                        {
                            // PIXEL DATA BITMAP 위치 설정 계산
                            // BITMAP DATA 배열 순서 : 예) Average:16 일 경우 --> x[0]y[0]a[0],...x[0]y[0]a[15],x[1]y[0]a[0],...x[1]y[0]a[15],x[2]y[0]a[0],...x[2]y[0]a[15],.....x[1279]y[959]a[15]
                            nLPos = ((nXLevel + nXOffset) + ((nYLevel + nYOffset) * nMaxW)) * MAX_SE_SCAN_IMAGE_AVERAGE;
                            // AVERAGE 갯수 많큼 LEVEL 값을 분류 한다.
                            for (nAverIdx = 0; nAverIdx < nMaxA; nAverIdx++)
                            {
                                // 해당 위치의 이미지 값 ( X, Y, AVEAGE )
                                btData = m_pSemSEScanImageBitmapBufferB[nLPos + nAverIdx];
                                // LEVEL COUNT를 계산할 X,Y 포인터 위치계산 ( LEVEL 범위 0 ~ 255 )
                                nBPos  = (((nXLevel + nXOffset) + ((nYLevel + nYOffset) * nMaxW)) * 256) + btData;
                                // 해당 LEVEL COUNT 증가
                                pLevelCountBuffer[nBPos] += 1;
                                //btLevelCountBuffer[btData] = pLevelCountBuffer[nBPos];
                            }
                            //memset(&btLevelCountBuffer, 0x00, 256);
                        }
                    }
                    // X,Y 각픽셀의 LEVEL COUNT 최고값 구하기
                    for (nYLevel = 0; nYLevel < nMaxH; nYLevel++)
                    {
                        for (nXLevel = 0; nXLevel < nMaxW; nXLevel++)
                        {
                            // LEVEL COUNT 값을 확인
                            BYTE btLevel = 0;
                            BYTE btCount = 0;
                            BYTE btMaxCt = 0;
                            for (nGrayIdx = 0; nGrayIdx < 256; nGrayIdx++)
                            {
                                // LEVEL COUNT를 계산할 X,Y 포인터 위치계산 ( LEVEL 범위 0 ~ 255 )
                                nBPos   = (((nXLevel + nXOffset) + ((nYLevel + nYOffset) * nMaxW)) * 256) + nGrayIdx;
                                btCount = pLevelCountBuffer[nBPos];
                                // 해당 LEVEL COUNT 값
                                if (btCount > btMaxCt)
                                {
                                    // 해당 X,Y 픽셀의 그레이 스케일 최대값 구하기
                                    btMaxCt = btCount;
                                    // 카운트 최고값이 있는 위치의 그레이스케일 적용
                                    btLevel = nGrayIdx;
                                }
                            }
                            // MAX LEVEL COUNT 포지션의 그레이스케일 LEVEL값 적용
                            // X, Y 픽셀 위치 포인트 계산
                            nLPos = (nXLevel + nXOffset) + ((nYLevel + nYOffset) * nMaxW);
                            // 해당 X,Y 픽셀의 그레이 스케일 적용
                            pLevelPixelBuffer[nLPos] = btLevel;
                        }
                    }
                    // PHOTO OVERLAP 이미지 다시그리기
                    for (nYLevel = 0; nYLevel < nMaxH; nYLevel++)
                    {
                        for (nXLevel = 0; nXLevel < nMaxW; nXLevel++)
                        {
                            // PHOTO SCAN IMAGE DATA PIXEL POSITION
                            k = (nXLevel + nXOffset) + ((nYLevel + nYOffset) * nMaxW);
                            btData = pLevelPixelBuffer[k];
                            // PIXEL DATA SET
                            m_p2ndImageRgb[k].rgbRed   = btData;
                            m_p2ndImageRgb[k].rgbGreen = btData;
                            m_p2ndImageRgb[k].rgbBlue  = btData;
                        }
                    }

                    // 2nd PIXEL BITMAP DATA SET
                    m_2ndImage1280x960Bitmap.SetBitmapBits(m_bm2ndBitmap.bmWidthBytes * m_bm2ndBitmap.bmHeight, m_p2ndBitmapBits);

                    // SE PHOTO SCAN IMAGE DATA DRAW UPDATE ( DISPLAY ) : 1 PIXEL 단위 데이터 처리
                    // 2019.09.18 이미지 업데이트(REDRAW)는 타이머를 이용하여 정기적으로 실행한다
                    // PHOTO SCAN BITMAP IMAGE UPDATE
                    if (bImageUpdate == TRUE)
                    {
                        // SCAN IMAGE UPDATE (DISPLAY)
                        SemSEPhotoScan345ImageDataUpdateControl();
                    }

                    // 버퍼 삭제
                    // delete[] pLevelPixelBuffer; (C#: GC)
                    // delete[] pLevelCountBuffer; (C#: GC)
                }
                */
                //
                //
                //

                // 
                // PHOTO OVERLAP : 이미지 선명도 필터 ( 샤프닝 기법 ) : 9개 PIXEL 이용
                //
                // 2020.08.03 : PHOTO FILTER #4 ( OVERLAP COUNT LEVEL )
                if (m_SemSEScanControlPublicData.m_bPhotoFilter4SelectFlag == TRUE)
                {
                    ushort[] pLevelAverageBuffer  = new ushort[PHOTO_SCAN_345_IMAGE_WIDTH*PHOTO_SCAN_345_IMAGE_HEIGHT];
                    byte[] pAveragePixelBuffer  = new byte[PHOTO_SCAN_345_IMAGE_WIDTH*PHOTO_SCAN_345_IMAGE_HEIGHT];
                    byte[] pAverageFilterBuffer = new byte[PHOTO_SCAN_345_IMAGE_WIDTH*PHOTO_SCAN_345_IMAGE_HEIGHT];
                    byte[] pSharpenFilterBuffer = new byte[PHOTO_SCAN_345_IMAGE_WIDTH*PHOTO_SCAN_345_IMAGE_HEIGHT];
                    memset(pLevelAverageBuffer , 0x00, sizeof(ushort)*(PHOTO_SCAN_345_IMAGE_WIDTH*PHOTO_SCAN_345_IMAGE_HEIGHT));
                    memset(pAveragePixelBuffer , 0x00, PHOTO_SCAN_345_IMAGE_WIDTH*PHOTO_SCAN_345_IMAGE_HEIGHT);
                    memset(pAverageFilterBuffer, 0x00, PHOTO_SCAN_345_IMAGE_WIDTH*PHOTO_SCAN_345_IMAGE_HEIGHT);
                    memset(pSharpenFilterBuffer, 0x00, PHOTO_SCAN_345_IMAGE_WIDTH*PHOTO_SCAN_345_IMAGE_HEIGHT);
                    int nAverIdx = 0;
                    int nXLevel  = 0;
                    int nYLevel  = 0;
                    int nXOffset = m_SemSEScanControlPublicData.m_nSEBitmapXOffsetB;
                    int nYOffset = m_SemSEScanControlPublicData.m_nSEBitmapYOffsetB;
                    int nMaxW = m_SemSEScanControlPublicData.m_nSEBitmapPixelWidth;
                    int nMaxH = m_SemSEScanControlPublicData.m_nSEBitmapPixelHeight;
                    int nMaxA = m_SemSEScanControlPublicData.m_nSEAverageNumber;
                    int nLPos = 0;
                    for (nYLevel = 0; nYLevel < nMaxH; nYLevel++)
                    {
                        for (nXLevel = 0; nXLevel < nMaxW; nXLevel++)
                        {
                            // PIXEL DATA BITMAP 위치 설정 계산
                            // BITMAP DATA 배열 순서 : 예) Average:16 일 경우 --> x[0]y[0]a[0],...x[0]y[0]a[15],x[1]y[0]a[0],...x[1]y[0]a[15],x[2]y[0]a[0],...x[2]y[0]a[15],.....x[1279]y[959]a[15]
                            nLPos = ((nXLevel + nXOffset) + ((nYLevel + nYOffset) * nMaxW)) * MAX_SE_SCAN_IMAGE_AVERAGE;
                            // AVERAGE 갯수 많큼 PIXEL 값을 더한다.
                            for (nAverIdx = 0; nAverIdx < nMaxA; nAverIdx++)
                            {
                                // 해당 위치의 이미지 값 ( X, Y, AVEAGE IDX )
                                btData = m_pSemSEScanImageBitmapBufferB[nLPos + nAverIdx];
                                // PHOTO SCAN IMAGE DATA PIXEL POSITION
                                k = (nXLevel + nXOffset) + ((nYLevel + nYOffset) * nMaxW);
                                // 해당 PIXEL 위치의 값 더하기
                                pLevelAverageBuffer[k] += btData;
                            }
                        }
                    }
                    // X,Y 각픽셀의 PIXEL AVERAGE 값 구하기
                    for (nYLevel = 0; nYLevel < nMaxH; nYLevel++)
                    {
                        for (nXLevel = 0; nXLevel < nMaxW; nXLevel++)
                        {
                            // LEVEL COUNT 값을 확인
                            byte btAver = 0;
                            // PHOTO SCAN IMAGE DATA PIXEL POSITION
                            k = (nXLevel + nXOffset) + ((nYLevel + nYOffset) * nMaxW);
                            ushort wData = pLevelAverageBuffer[k];
                            btAver = (byte)((double)((double)((double)wData / (double)nMaxA) + 0.5));
                            // 해당 X,Y 픽셀의 그레이 스케일 적용
                            pAveragePixelBuffer[k] = btAver;
                        }
                    }

                    int[] nPixelData = NewArray<int>(9);
                    int    nFilterPointer = 0;
                    int    nFilterValue   = 0;
                    double dFilterValue   = 0;
                    double dFilterAlpha   = 9.0; // 중심픽셀 9배

                    //
                    // PHOTO IMAGE FILETR : 미디언 필터 ( 잡음제거 )
                    //
                    for (nYLevel = 1; nYLevel < nMaxH - 1; nYLevel++)
                    {
                        for (nXLevel = 1; nXLevel < nMaxW - 1; nXLevel++)
                        {
                            // 미디언 필터 : 주변 9개의 PIXEL 중 중간값 사용 -> 잡음제거
                            // FILTER를 위한 PIXEL DATA BITMAP 위치 설정 계산 : 0~8번
                            nFilterPointer = ((nXLevel-1) + ((nYLevel-1)*nMaxW)); nPixelData[1] = pAveragePixelBuffer[nFilterPointer]; // X-1, Y-1
                            nFilterPointer = ((nXLevel-0) + ((nYLevel-1)*nMaxW)); nPixelData[2] = pAveragePixelBuffer[nFilterPointer]; // X-0, Y-1
                            nFilterPointer = ((nXLevel+1) + ((nYLevel-1)*nMaxW)); nPixelData[3] = pAveragePixelBuffer[nFilterPointer]; // X+1, Y-1
                            nFilterPointer = ((nXLevel-1) + ((nYLevel-0)*nMaxW)); nPixelData[4] = pAveragePixelBuffer[nFilterPointer]; // X-1, Y-0
                            nFilterPointer = ((nXLevel-0) + ((nYLevel-0)*nMaxW)); nPixelData[0] = pAveragePixelBuffer[nFilterPointer]; // X-0, Y-0 (중심PIXEL)
                            nFilterPointer = ((nXLevel+1) + ((nYLevel-0)*nMaxW)); nPixelData[5] = pAveragePixelBuffer[nFilterPointer]; // X+1, Y-0
                            nFilterPointer = ((nXLevel-1) + ((nYLevel+1)*nMaxW)); nPixelData[6] = pAveragePixelBuffer[nFilterPointer]; // X-1, Y+1
                            nFilterPointer = ((nXLevel-0) + ((nYLevel+1)*nMaxW)); nPixelData[7] = pAveragePixelBuffer[nFilterPointer]; // X-0, Y+1
                            nFilterPointer = ((nXLevel+1) + ((nYLevel+1)*nMaxW)); nPixelData[8] = pAveragePixelBuffer[nFilterPointer]; // X+1, Y+1

                            // Insertion sort : 중간값 찾기 소트
                            m_pAssistDeviceTool.InsertSort(nPixelData, 9);
                            // 중간값
                            nFilterValue = nPixelData[4];

                            // PHOTO SCAN IMAGE DATA PIXEL POSITION
                            k = (nXLevel + nXOffset) + ((nYLevel + nYOffset) * nMaxW);
                            pAverageFilterBuffer[k] = (byte)nFilterValue;
                        }
                    }

                    //
                    // 이미지 선명도 필터 ( 샤프닝 기법 ) : 9개 PIXEL 이용
                    // 현재 픽셀의 값에 9를 곱한 후 상하좌우 주변 8개에 있는 픽셀의 값들을 빼준다. 만약 9개 픽셀의 값이 모두 동일하다면 아무 변화가 없을 것이다. 
                    // 반면 현재 픽셀의 값이 주변 픽셀의 값보다 크다면 그것은 더 부각될 것이고, 작다면 더 작아질 것이다. 결과적으로 엣지가 부각된다.
                    //
                    for (nYLevel = 1; nYLevel < nMaxH - 1; nYLevel++)
                    {
                        for (nXLevel = 1; nXLevel < nMaxW - 1; nXLevel++)
                        {
                            //
                            // FILTER를 위한 PIXEL DATA BITMAP 위치 설정 계산 : 중심PIXEL, LEFT, UP, RIGHT, DOWN & 주변 PIXEL 이용
                            // FILTER를 위한 PIXEL DATA BITMAP 위치 설정 계산 : 0~8번
                            nFilterPointer = ((nXLevel-1) + ((nYLevel-1)*nMaxW)); nPixelData[1] = pAverageFilterBuffer[nFilterPointer]; // X-1, Y-1
                            nFilterPointer = ((nXLevel-0) + ((nYLevel-1)*nMaxW)); nPixelData[2] = pAverageFilterBuffer[nFilterPointer]; // X-0, Y-1
                            nFilterPointer = ((nXLevel+1) + ((nYLevel-1)*nMaxW)); nPixelData[3] = pAverageFilterBuffer[nFilterPointer]; // X+1, Y-1
                            nFilterPointer = ((nXLevel-1) + ((nYLevel-0)*nMaxW)); nPixelData[4] = pAverageFilterBuffer[nFilterPointer]; // X-1, Y-0
                            nFilterPointer = ((nXLevel-0) + ((nYLevel-0)*nMaxW)); nPixelData[0] = pAverageFilterBuffer[nFilterPointer]; // X-0, Y-0 (중심PIXEL)
                            nFilterPointer = ((nXLevel+1) + ((nYLevel-0)*nMaxW)); nPixelData[5] = pAverageFilterBuffer[nFilterPointer]; // X+1, Y-0
                            nFilterPointer = ((nXLevel-1) + ((nYLevel+1)*nMaxW)); nPixelData[6] = pAverageFilterBuffer[nFilterPointer]; // X-1, Y+1
                            nFilterPointer = ((nXLevel-0) + ((nYLevel+1)*nMaxW)); nPixelData[7] = pAverageFilterBuffer[nFilterPointer]; // X-0, Y+1
                            nFilterPointer = ((nXLevel+1) + ((nYLevel+1)*nMaxW)); nPixelData[8] = pAverageFilterBuffer[nFilterPointer]; // X+1, Y+1

                            // 이미지 선명도 필터 계산 = 중심PIXEL(x9배) - (나머지 4개 픽셀값) : 중심PIXEL 값이 주변보다 높으면 더높게, 낮으면 더낮게 만든다, 주변과 같으면 같은값이 설정된다
                            dFilterValue = (dFilterAlpha*(double)nPixelData[0]) - (double)nPixelData[1] - (double)nPixelData[2] - (double)nPixelData[3] - (double)nPixelData[4] - (double)nPixelData[5] - (double)nPixelData[6] - (double)nPixelData[7] - (double)nPixelData[8];
                            nFilterValue = (int)(dFilterValue + 0.5);
                            if (nFilterValue < 0  ) nFilterValue = 0;
                            if (nFilterValue > 255) nFilterValue = 255;
                            //
                            /*
                            // FILTER를 위한 PIXEL DATA BITMAP 위치 설정 계산 : 중심PIXEL, LEFT, UP, RIGHT, DOWN PIXEL 이용
                            // FILTER를 위한 PIXEL DATA BITMAP 위치 설정 계산 : 0~4번
                            dFilterAlpha = 5.0;
                            nFilterPointer = ((nXLevel-0) + ((nYLevel-1)*nMaxW)); nPixelData[1] = pAverageFilterBuffer[nFilterPointer]; // X-0, Y-1
                            nFilterPointer = ((nXLevel-1) + ((nYLevel-0)*nMaxW)); nPixelData[2] = pAverageFilterBuffer[nFilterPointer]; // X-1, Y-0
                            nFilterPointer = ((nXLevel-0) + ((nYLevel-0)*nMaxW)); nPixelData[0] = pAverageFilterBuffer[nFilterPointer]; // X-0, Y-0 (중심PIXEL)
                            nFilterPointer = ((nXLevel+1) + ((nYLevel-0)*nMaxW)); nPixelData[3] = pAverageFilterBuffer[nFilterPointer]; // X+1, Y-0
                            nFilterPointer = ((nXLevel-0) + ((nYLevel+1)*nMaxW)); nPixelData[4] = pAverageFilterBuffer[nFilterPointer]; // X-0, Y+1

                            // 이미지 선명도 필터 계산 = 중심PIXEL(x9배) - (나머지 4개 픽셀값) : 중심PIXEL 값이 주변보다 높으면 더높게, 낮으면 더낮게 만든다, 주변과 같으면 같은값이 설정된다
                            dFilterValue = (dFilterAlpha*(double)nPixelData[0]) - (double)nPixelData[1] - (double)nPixelData[2] - (double)nPixelData[3] - (double)nPixelData[4];
                            nFilterValue = (int)(dFilterValue + 0.5);
                            if (nFilterValue < 0  ) nFilterValue = 0;
                            if (nFilterValue > 255) nFilterValue = 255;
                            */
                            // PHOTO SCAN IMAGE DATA PIXEL POSITION
                            k = (nXLevel + nXOffset) + ((nYLevel + nYOffset) * nMaxW);
                            pSharpenFilterBuffer[k] = (byte)nFilterValue;
                        }
                    }

                    // PHOTO OVERLAP 이미지 다시그리기
                    for (nYLevel = 0; nYLevel < nMaxH; nYLevel++)
                    {
                        for (nXLevel = 0; nXLevel < nMaxW; nXLevel++)
                        {
                            // PHOTO SCAN IMAGE DATA PIXEL POSITION
                            k = (nXLevel + nXOffset) + ((nYLevel + nYOffset) * nMaxW);
                            btData = pSharpenFilterBuffer[k];
                            // PIXEL DATA SET
                            m_p2ndImageRgb[k].rgbRed   = btData;
                            m_p2ndImageRgb[k].rgbGreen = btData;
                            m_p2ndImageRgb[k].rgbBlue  = btData;
                        }
                    }

                    // 2nd PIXEL BITMAP DATA SET
                    m_2ndImage1280x960Bitmap.SetBitmapBits(m_bm2ndBitmap.bmWidthBytes * m_bm2ndBitmap.bmHeight, m_p2ndBitmapBits);

                    // SE PHOTO SCAN IMAGE DATA DRAW UPDATE ( DISPLAY ) : 1 PIXEL 단위 데이터 처리
                    // 2019.09.18 이미지 업데이트(REDRAW)는 타이머를 이용하여 정기적으로 실행한다
                    // PHOTO SCAN BITMAP IMAGE UPDATE
                    if (bImageUpdate == TRUE)
                    {
                        // SCAN IMAGE UPDATE (DISPLAY)
                        SemSEPhotoScan345ImageDataUpdateControl();
                    }

                    // 버퍼 삭제
                    // delete[] pLevelAverageBuffer; (C#: GC)
                    // delete[] pAveragePixelBuffer; (C#: GC)
                    // delete[] pAverageFilterBuffer; (C#: GC)
                    // delete[] pSharpenFilterBuffer; (C#: GC)
                }

                //
                //
                //


                // 이전 스캔모드 실행
                if (m_SemSEScanControlPublicData.m_nSaveScanMode == RED_SCAN_MODE)
                {
                    // RED SCAN MODE
                    m_wndScanControlDlg.OnBnClickedScanControlRedScanBtn();
                }
                else if (m_SemSEScanControlPublicData.m_nSaveScanMode == FAST_SCAN_MODE)
                {
                    // FAST SCAN MODE
                    m_wndScanControlDlg.OnBnClickedScanControlFastScanBtn();
                }
                else if (m_SemSEScanControlPublicData.m_nSaveScanMode == SLOW1_SCAN_MODE)
                {
                    // SLOW1 SCAN MODE
                    m_wndScanControlDlg.OnBnClickedScanControlSlow1ScanBtn();
                }
                else if (m_SemSEScanControlPublicData.m_nSaveScanMode == SLOW2_SCAN_MODE)
                {
                    // SLOW2 SCAN MODE
                    m_wndScanControlDlg.OnBnClickedScanControlSlow2ScanBtn();
                }

                // PHOTO IMAGE 원본 백업 ( BITMAP DATA )
                memcpy(m_p2ndOrgBitmapBits, m_p2ndBitmapBits, m_bm2ndBitmap.bmWidthBytes * m_bm2ndBitmap.bmHeight);

                //
                // 히스토그램 그래프 보기
                //
                double[] dHisto = NewArray<double>(256);
                int nCMax = 0;
                memset(dHisto, 0x00, sizeof(double) * 256);
                // 히스토그램 계산
                m_pAssistPublicTool.HistogramNormalize(ref m_2ndImage1280x960Bitmap, dHisto, ref nCMax);
                // 히스토그램 그래프 그리기
                m_pAssistPublicTool.DrawHistogramGraph(m_p4thImageDC, ref m_4thImageMemDC, dHisto, nCMax, 256);
                // HISTOGRAM ANALYSIS CONTROL
                m_nHistogramAnalysisContrasrt = 50;
                m_nHistogramAnalysisBright    = 50;
                m_nHistogramAnalysisGamma     = 50;
                //
                //
                //

            }

            //
            //
            //


            return RET_SUCCESS_OK;
        }

    }
}
