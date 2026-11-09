// 원본 USB_FT232H_Device.cpp:21318-21627 을 NanoImpact.CodeGen translate로 옮긴 뒤 손으로 고친 코드.
#pragma warning disable IDE1006, IDE0059, CS0162, CS0168, CS0219
using System;
using NanoImpact.Core.Native;
using static NanoImpact.Core.Def;
using static NanoImpact.Core.Native.CRuntime;

namespace NanoImpact.Hardware.Device
{
    public sealed partial class SemDevice
    {
        // ---- USB_FT232H_Device.cpp:21318 ----
        //
        // Integrated Control : CL SPOT-SIZE MODE --> CL LMH(LOW,MIDDLE,HIGH) MODE 로 전환시 GAX/Y, BAX/Y 백업
        //
        public int AfxIntegratedCLLMHModeDataBackup(int nCLSpotGAX, int nCLSpotGAY, int nCLSpotBAX, int nCLSpotBAY)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            
            // CL SPOT-SIZE MODE 시 설정된 GAX/Y, BAX/Y DATA 백업
            // GAX 값 백업
            m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_SPOT_GAX_VALUE] = nCLSpotGAX;
            // GAY 값 백업
            m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_SPOT_GAY_VALUE] = nCLSpotGAY;
            // BAX 값 백업
            m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_SPOT_BAX_VALUE] = nCLSpotBAX;
            // BAX 값 백업
            m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_SPOT_BAY_VALUE] = nCLSpotBAY;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:21340 ----
        // 2021.06.15
        //
        // FOCUS DEGAUSSING 처리 쓰레드 생성
        //
        public int AfxCreateFocusDegaussingThread(int nWDPos, int nCoarse, int nFine, int nProcessTime)
        {
            // FOCUS DEGAUSSING 쓰레드 사용 중이면...
            if (m_pFocusDegaussingThread != null)
            {
                // FOCUS DEGAUSSING 쓰레드 파라메터 사용 중인지 확인
                if (m_pFocusDegaussingThreadParams != null)
                {
                    // FOCUS DEGAUSSING 쓰레드 종료 확인
                    if (m_pFocusDegaussingThreadParams.bThreadDone == TRUE)
                    {
                        // 2021.05.31
                        // 쓰레드 종료 됨, 삭제
                        // FOCUS DEGAUSSING 쓰레드에서 사용한 파라메터 삭제

                        m_pFocusDegaussingThreadParams = null;
                    }
                    else
                    {
                        // 쓰레드 동작 중, 리턴
                        return RET_SUCCESS_OK;
                    }
                }
                //return RET_SUCCESS_OK;
            }

            // FOCUS DEGAUSSING 쓰레드 파라메터 사용 중인지 확인
            if (m_pFocusDegaussingThreadParams != null)
            {
                // FOCUS DEGAUSSING 쓰레드에서 사용한 파라메터 삭제

                m_pFocusDegaussingThreadParams = null;
            }

            // FOCUS DEGAUSSING 쓰레드에서 사용할 파라메터를 생성한다
            m_pFocusDegaussingThreadParams = new FOCUSDEGAUSSINGTHREADPARAMS();
            if (m_pFocusDegaussingThreadParams == null) return RET_ERROR_SBR_THREAD_PARAMS;
            m_pFocusDegaussingThreadParams = new FOCUSDEGAUSSINGTHREADPARAMS();
            // FOCUS DEGAUSSING 쓰레드에서 사용할 파라메터 기본값 설정
            m_pFocusDegaussingThreadParams.bThreadDone  = FALSE;        // 쓰레드 종료 설정
            m_pFocusDegaussingThreadParams.bThreadStop  = FALSE;        // 쓰레드 강제 종료
            m_pFocusDegaussingThreadParams.nFT232HIndex = IDX_FT232H_SEM_IMAGE_DEVICE; // Scan Image 장치 인덱스
            m_pFocusDegaussingThreadParams.hFT232Handle = IntPtr.Zero;         // Scan Bridge 장치 핸들
            m_pFocusDegaussingThreadParams.hEventHandle = null;         // Scan Bridge 이벤트 핸들
            //
            m_pFocusDegaussingThreadParams.nWDPosition  = nWDPos;       // FOCUS DEGAUSSING : Z AXIS WD POSITION
            m_pFocusDegaussingThreadParams.nFocusCoarse = nCoarse;      // FOCUS DEGAUSSING : FOCUS COARSE VALUE
            m_pFocusDegaussingThreadParams.nFocusFine   = nFine;        // FOCUS DEGAUSSING : FOCUS FINE   VALUE
            m_pFocusDegaussingThreadParams.nProcessTime = nProcessTime; // FOCUS DEGAUSSING : PROCESS TIME
            //

            // 핸들 설정
            IntPtr ft232hHandle = AfxFT232HGetHandle(m_pFocusDegaussingThreadParams.nFT232HIndex);
            if (ft232hHandle == IntPtr.Zero) return RET_ERROR_FT232H_SBR_HANDLE;
            m_pFocusDegaussingThreadParams.hFT232Handle = ft232hHandle; // FOCUS DEGAUSSING 장치 핸들

            // FOCUS DEGAUSSING 쓰레드 생성 및 시작...
            m_pFocusDegaussingThread = AfxBeginThread(AfxThreadFocusDegaussingProcessing, m_pFocusDegaussingThreadParams);
            if (m_pFocusDegaussingThread == null) return RET_ERROR_SBR_THREAD_HANDLE;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:21407 ----
        //
        // FOCUS DEGAUSSING 처리(작업) 쓰레드
        //
        public uint AfxThreadFocusDegaussingProcessing(object pParams)
        {
            FOCUSDEGAUSSINGTHREADPARAMS pThreadParams = (FOCUSDEGAUSSINGTHREADPARAMS)pParams;

            if (pThreadParams == null) return 0;

            int  nFocusJumpStep = 10;
            int  nProcessStep   = 0;
            int  nFocusCoarse   = 0;
            int  nFocusFine     = 0;
            int  nIntervalTime  = 0;
            int  nProcessTime   = 0;
            int  nWDPosition    = 0;
            int bbThreadDone   = FALSE;

            // 쓰레드 작업 상태 설정
            pThreadParams.bThreadDone = FALSE;
            pThreadParams.bThreadStop = FALSE;
            bbThreadDone = pThreadParams.bThreadDone;
            // 작업시간 설정
            nProcessTime = pThreadParams.nProcessTime;
            nWDPosition  = pThreadParams.nWDPosition;
            nFocusCoarse = pThreadParams.nFocusCoarse;
            nFocusFine   = pThreadParams.nFocusFine;
            // 처리횟수 = OCUS COARSE / FOCUS 점프스텝
            nProcessStep = nFocusCoarse / nFocusJumpStep;
            // INTERVAL TIME = 총처리시간 / 총처리횟수
            double dTime = (double)nProcessTime;
            double dStep = (double)nProcessStep;
            double dIntv = (dTime / dStep) + 0.5;
            nIntervalTime = (int)dIntv;
            if (nIntervalTime < 1) nIntervalTime = 1;

            //
            //  FocusFine 값의 변화 범위가 0 - 128 이므로, 최하의
            //  0 값 근처에서, OP-Amp의 오프셋 때문에 변화가 없을 수 있으므로
            //  중간값에서 변화되도록 변화시킴
            //
            // Fine 값은 2048 +/- 128 이다
            //
            // Fine 값 설정
            int nFine = pThreadParams.nFocusFine;
            // FOCUS FINE 값이 변경 되었는지 확인한다.
            if (nFine < SEM_FOCUS_FINE_MIN) nFine = SEM_FOCUS_FINE_MIN;
            if (nFine > SEM_FOCUS_FINE_MAX) nFine = SEM_FOCUS_FINE_MAX;
            // Smg Command 전송
            AfxFT232HPortSendCommand(pThreadParams.nFT232HIndex, cWR_AD5361, AD5361_OLF + (2048 - 128 + nFine << 4));

            ///
            // FOCUS DEGAUSSING 쓰레드 프로세싱
            ///
            int bLoop   = TRUE;
            int  nCoarse = SEM_FOCUS_COARSE_MIN;
            while (bLoop == TRUE)
            {
                //
                // FOCUS DEGAUSSING 순차적 처리
                //
                // FOCUS COARSE 값 확인한다.
                if (nCoarse >= nFocusCoarse)
                {
                    // 마지막 작업설정 및 LOOP 종료
                    nCoarse = nFocusCoarse;
                    bLoop = FALSE;
                }

                // Focus Coarse 값 USB 설정
                // Smg Command 전송
                AfxFT232HPortSendCommand(pThreadParams.nFT232HIndex, cWR_AD5361, AD5361_OLC + (nCoarse + 4096 << 3));

                // FOCUS COARSE 값 증가
                nCoarse += nFocusJumpStep;

                if (bLoop == TRUE)
                {
                    // 강제종료 CHECK?
                    if (pThreadParams.bThreadStop == TRUE)
                    {
                        // 쓰레드 종료
                        bLoop = FALSE;
                    }
                    else
                    {
                        // 인터벌 타임[ms] 대기
                        Sleep(nIntervalTime);
                    }
                }
            }

            ///
            // 쓰레드 종료 확인
            if (pThreadParams != null)
            {
                pThreadParams.bThreadDone = TRUE;
            }
            ///

            // WD 값이 변동 있으면 WD에 따른 FOCUS COARSE 실행한다
            AfxIntegratedSetWDAutoAdjustFocusCoarse(nWDPosition); // 새로운 WD 적용
            AfxScanBridgeSetScanRotation(nWDPosition);            // WD에 따른 배율조정을 위한 Scan Rotation 실행

            // FOCUS COARSE, FINE 재설정
            m_pIntegrated.nFocusOldCoarse = SEM_FOCUS_COARSE_STR;
            m_pIntegrated.nFocusOldFine   = 1;
            AfxIntegratedSetOL(nFocusCoarse, nFocusFine);

            // 2021.07.22
            // FOCUS CONTROL DIALOG에서 FOCUS COARSE를 실행한다
            USB_DEVICE_DATA_INFO UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();

            // 전송자료
            UsbDeviceDataInfo.nState = 1; // FOCUS COARSE 값
            UsbDeviceDataInfo.nData[0] = nFocusCoarse;
            UsbDeviceDataInfo.nData[1] = nFocusFine;

            if (m_hOwnerWnd != NULL)
            {
                // 2021.07.22
                // FOCUS CONTROL DIALOG에서 FOCUS COARSE를 실행한다
                SendMessage(m_hOwnerWnd, UM_USB_SBR_FOCUS_COARSE_DATA_EXECUTE, NULL, UsbDeviceDataInfo);
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:21537 ----
        //
        // Z-AXIS(WD)에 따른 FOCUS DEGAUSSING 실행 : 0~500 , 0~50.0mm
        //
        public int AfxIntegratedFocusDegaussing(int nWDPos = 40, int nRunTime = 500)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // 2021.06.15
            // WD 값에 따른 FOCUS DEGAUSSING 실행
            int nOLWD = nWDPos;
            if (nOLWD < 40)  nOLWD = 40;  // 4 [mm]
            if (nOLWD > 500) nOLWD = 500; // 50[mm]

            // 실행(소요)시간 : 500[ms]
            int nProcessTime = nRunTime;
            if (nProcessTime < 500) nProcessTime = 500;

            // FOCUS COARSE 에 해당 하는 DEGAUSSING 실행
            int nFocusCoarse = m_pIntegrated.nWDOLCoarseLinear[nOLWD];
            // DEGAUSSING : COARSE / FINE / PROCESS TIME[ms]
            AfxCreateFocusDegaussingThread(nOLWD, nFocusCoarse, SEM_FOCUS_FINE_STR, nProcessTime);

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:21622 ----
        // 2023.03.14 : MAG RELAY IMAGE SHIFT XY CORRECT VALUE
        public void SetClientPublicData(ClientPublicData pClientPublicData)
        {
            if (pClientPublicData == null) return;

            m_pClientPublicData = pClientPublicData;
        }

    }
}
