// 원본 USB_FT232H_Device.cpp:3174-5303 을 NanoImpact.CodeGen translate로 옮긴 뒤 손으로 고친 코드.
#pragma warning disable IDE1006, IDE0059, CS0162, CS0168, CS0219
using System;
using NanoImpact.Core.Native;
using static NanoImpact.Core.Def;
using static NanoImpact.Core.Native.CRuntime;

namespace NanoImpact.Hardware.Device
{
    public sealed partial class SemDevice
    {
        // ---- USB_FT232H_Device.cpp:3174 ----
        //
        // FT232H : USB Device 사용여부 설정
        //
        public int AfxFT232HDeviceUse(int nUse)
        {
            m_nFT232HDeviceUse = nUse;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:3259 ----
        //
        // Se Command : Command Create & Initilize (SEM Image Command 변수 초기화)
        //
        public int AfxCreateSeImageCommand()
        {
            /*
            if (m_pSeCommand != NULL)
            {
                delete m_pSeCommand;
                m_pSeCommand = NULL;
            }

            // 공용 명령어에 필요한 변수 생성
            m_pSeCommand = new SEM_IMAGE_COMMAND_STRUCT;
            // 초기화
            memset(m_pSeCommand, 0x00, sizeof(SEM_IMAGE_COMMAND_STRUCT));

            // 초기값 설정
            m_pIntegrated->btImageAvrBPort = 0;
            */

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:3283 ----
        //
        // Integrated : Command Create & Initilize (공용변수 초기화)
        //
        public int AfxCreateIntegratedCommand()
        {
            if (m_pIntegrated != null)
            {

                m_pIntegrated = null;
            }

            // 공용 명령어에 필요한 변수 생성
            m_pIntegrated = new IntegratedCommandStruct();
            // 초기화
            m_pIntegrated.Clear();
            // VERSION 설정 : ??20190722 (앞의 두자리는 프로그램 종류에 따른 설정, 10이면 범용 버전) 
            m_pIntegrated.dwVersion         = 1020190722;     // 10->범용, 20->NanoFlash 계열, 30->BioFlash 계열 ...
            // 장치연결 Flag
            m_pIntegrated.bSemConnectedFlag = m_bSemImageConnected;  // SE Image 장치
            m_pIntegrated.bBseConnectedFlag = m_bBseImageConnected;  // BSE Image 장치
            m_pIntegrated.bSbrConnectedFlag = m_bScanBridgeConnected;// Scan Bridge 장치
            // SEM MODEL
            m_pIntegrated.nSemModel      = 1;                 // SEM Model : SM-300B CL 기본값
            // GUN HV 초기값 설정
            m_pIntegrated.nGunHVIndex    = 0;                 // GUN HV 목표값 인덱스
            m_pIntegrated.nGunHVValue    = 0;                 // GUN HV 목표값 (전압)
            m_pIntegrated.nGunHVCurIndex = 0;                 // GUN HV 현재값 인덱스
            m_pIntegrated.nGunHVCurValue = 0;                 // GUN HV 현재값 (전압)
            m_pIntegrated.nGunHVExeIndex = 0;                 // GUN HV 처리중 인덱스
            m_pIntegrated.nGunHVExeValue = 0;                 // GUN HV 처리중 (전압)
            // 필라멘트
            m_pIntegrated.nFilamentValue    = 0;              // Filament 목표값
            m_pIntegrated.nFilamentCurValue = 0;              // Filament 현재값
            m_pIntegrated.nFilamentExeValue = 0;              // Filament 실행중인값
            m_pIntegrated.nFilamentOldValue = 0;              // Filament 목표 및 현재값 보관
            // BIAS
            m_pIntegrated.nBiasValue    = 0;                  // Bias 목표값
            m_pIntegrated.nBiasCurValue = 0;                  // Bias 현재값
            m_pIntegrated.nBiasExeValue = 0;                  // Bias 실행중인값
            m_pIntegrated.nBiasOldValue = 0;                  // Bias 목표 및 현재값 보관
            //
            m_pIntegrated.dAmpRk=2.4;                         // SEM 모델에 따라 기본값을 설정한다 (SM-300A CL:3.9, SM-300B CL:2.4, SM-200A CL:3.0, BK-200:2.4, S-2500히타치:4.4, 200CL USER:3.0, 300CL USER:2.4)
            m_pIntegrated.dRefR=2.0;                          // SEM 모델에 따라 기본값을 설정한다 (SM-300A CL:3.3, SM-300B CL:2.0, SM-200A CL:2.0, BK-200:2.0, S-2500히타치:2.0, 200CL USER:2.0, 300CL USER:2.0)
            // HV Power On Flag
            m_pIntegrated.bGunHVPowerOnFlag      = FALSE;     // GUN HV Power On Flag
            m_pIntegrated.bFilamentPowerOnFlag   = FALSE;     // Filament HV Power On Flag
            m_pIntegrated.bBiasPowerOnFlag       = FALSE;     // Bias HV Power On Flag
            m_pIntegrated.bPMTPowerOnFlag        = FALSE;     // PMT HV Power On Flag
            m_pIntegrated.bDETPowerOnFlag        = FALSE;     // Detector HV Power On Flag
            m_pIntegrated.bSM2500PowerOnFlag     = FALSE;     // Hitachi SM-2500 HV Power On Flag
            m_pIntegrated.bContrastOnFlag        = FALSE;     // HV 설정시 Contrast 처음 실행 Flag
            m_pIntegrated.bScanRotationOnFlag    = FALSE;     // HV 설정시 Scan Rotation 처음 실행 Flag
            m_pIntegrated.bScreenModeOnFlag      = FALSE;     // HV 설정시 Screen Mode 처음 실행 Flag
            m_pIntegrated.bHVUpEndProcessFlag    = FALSE;     // HV ALL 실행시 Up   Ending 처음 실행 Flag
            m_pIntegrated.bHVDownEndProcessFlag  = FALSE;     // HV ALL 실행시 Down Ending 처음 실행 Flag
            m_pIntegrated.nHVStandbyProcess      = 0;         // HV Standby Mode Up/Down Process Status : 1->Standby, 0->Normal
            m_pIntegrated.nGunHVUpDownProcess    = 0;         // Gun HV   Up/Down Process Status : 10->Start, 20->Process, 99->End
            m_pIntegrated.nFilamentUpDownProcess = 0;         // Filament Up/Down Process Status : 10->Start, 20->Process, 99->End
            m_pIntegrated.nBiasUpDownProcess     = 0;         // Bias     Up/Down Process Status : 10->Start, 20->Process, 99->End
            m_pIntegrated.bHVStartProcessFlag    = FALSE;     // HV Start Process Execute Flag
            m_pIntegrated.bHVAllStartProcessFlag = FALSE;     // HV All Start Process Execute Flag
            m_pIntegrated.bHVAllStopProcessFlag  = FALSE;     // HV All Stop  Process Execute Flag
            // GUN HV Voltage 초기화
            m_pIntegrated.dGunHVCurVoltage    = 0.0;          // GUN HV 현재 설정된 Voltage
            m_pIntegrated.dGunHVUpVoltage     = 0.0;          // GUN HV 현재 설정된 Up Voltage
            m_pIntegrated.dGunHVDownVoltage   = 0.0;          // GUN HV 현재 설정된 Down Voltage
            m_pIntegrated.dGunHVStartVoltage  = 0.0;          // GUN HV 현재 설정된 Start Voltage
            m_pIntegrated.dGunHVStopVoltage   = 0.0;          // GUN HV 현재 설정된 Stop  Voltage
            m_pIntegrated.dGunHVStepVoltage   = 0.0;          // GUN HV 현재 설정된 Step  Voltage
            m_pIntegrated.dGunHVMaxVoltage    = 0.0;          // GUN HV 현재 설정된 Max   Voltage
            m_pIntegrated.dGunHVMinVoltage    = 0.0;          // GUN HV 현재 설정된 Min   Voltage
            m_pIntegrated.dGunHVBackupVoltage = 0.0;          // GUN HV 현재 설정된 Voltage Backup
            // FILAMENT Voltage 초기화
            m_pIntegrated.dFilamentCurVoltage    = 0.0;       // FILAMENT 현재 설정된 Voltage
            m_pIntegrated.dFilamentUpVoltage     = 0.0;       // FILAMENT 현재 설정된 Up Voltage
            m_pIntegrated.dFilamentDownVoltage   = 0.0;       // FILAMENT 현재 설정된 Down Voltage
            m_pIntegrated.dFilamentStartVoltage  = 0.0;       // FILAMENT 현재 설정된 Start Voltage
            m_pIntegrated.dFilamentStopVoltage   = 0.0;       // FILAMENT 현재 설정된 Stop  Voltage
            m_pIntegrated.dFilamentStepVoltage   = 0.0;       // FILAMENT 현재 설정된 Step  Voltage
            m_pIntegrated.dFilamentMaxVoltage    = 0.0;       // FILAMENT 현재 설정된 Max   Voltage
            m_pIntegrated.dFilamentMinVoltage    = 0.0;       // FILAMENT 현재 설정된 Min   Voltage
            m_pIntegrated.dFilamentBackupVoltage = 0.0;       // FILAMENT 현재 설정된 Voltage Backup
            // BIAS Voltage 초기화
            m_pIntegrated.dBiasCurVoltage    = 0.0;           // BIAS 현재 설정된 Voltage
            m_pIntegrated.dBiasUpVoltage     = 0.0;           // BIAS 현재 설정된 Up Voltage
            m_pIntegrated.dBiasDownVoltage   = 0.0;           // BIAS 현재 설정된 Down Voltage
            m_pIntegrated.dBiasStartVoltage  = 0.0;           // BIAS 현재 설정된 Start Voltage
            m_pIntegrated.dBiasStopVoltage   = 0.0;           // BIAS 현재 설정된 Stop  Voltage
            m_pIntegrated.dBiasStepVoltage   = 0.0;           // BIAS 현재 설정된 Step  Voltage
            m_pIntegrated.dBiasMaxVoltage    = 0.0;           // BIAS 현재 설정된 Max   Voltage
            m_pIntegrated.dBiasMinVoltage    = 0.0;           // BIAS 현재 설정된 Min   Voltage
            m_pIntegrated.dBiasBackupVoltage = 0.0;           // BIAS 현재 설정된 Voltage Backup

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:3380 ----
        //
        // Integrated : Release Command (공용변수 제거)
        //
        public int AfxReleaseIntegratedCommand()
        {
            // 공용 명령어 변수 제거
            if (m_pIntegrated != null)
            {

                m_pIntegrated = null;
            }

            // SCAN ROTATION CALIBRATION DATA 삭제
            if (m_pScanRotCaliData != null)
            {

                m_pScanRotCaliData = null;
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:3403 ----
        //
        // Integrated : Get Pointer (공용변수 포인터)
        //
        public IntegratedCommandStruct AfxGetIntegratedPointer()
        {
            // 공용 명령어 변수
            if (m_pIntegrated != null)
            {
                return m_pIntegrated;
            }

            return null;
        }

        // ---- USB_FT232H_Device.cpp:3418 ----
        //
        // Integrated : Get Pointer (SCAN ROTATION CALIBRATION DATA 포인터)
        //
        public ScanRotationCalibrationData AfxGetScanRotationCalibrationDataPointer()
        {
            // 공용 명령어 변수
            if (m_pScanRotCaliData != null)
            {
                return m_pScanRotCaliData;
            }

            return null;
        }

        // ---- USB_FT232H_Device.cpp:3433 ----
        //
        // Integrated : INTEGRATED 파일 불러오기
        //
        public int AfxGetIntegratedDataFromFile()
        {
            // 공용 명령어 변수
            if (m_pIntegrated == null) return -1;

            // 공용 변수 생성
            IntegratedCommandStruct pIntegrated = new IntegratedCommandStruct();

            // 초기화
            pIntegrated.Clear();
            // 초기화 데이터 복사
            pIntegrated.CopyFrom(m_pIntegrated);

            uint nDataSize = IntegratedCommandStruct.NativeSize;
            string strFileName = "";
            strFileName = m_strDeviceServerPath + "\\" + DEVICE_INTEGRATED_FILE_NAME;

            CFile IntegratedFile = new CFile();

            // Integrated Data File Load
            if (!IntegratedFile.Open(strFileName, CFile.modeRead))
            {
                // Open or Read Error
                if (!IntegratedFile.Open(strFileName, CFile.modeCreate))
                {
                    // File Create Error
                    return -1;
                }
                IntegratedFile.Close();

                // Initilize Integrated Data To File
                if (!IntegratedFile.Open(strFileName, CFile.modeWrite))
                {
                    return -1;
                }

                //
                // DATA 초기화
                //
                // CL 모델에 따른 변수 초기화
                AfxSemModelCLInitialize(m_pIntegrated.nSemModel, m_pIntegrated.dSemModelAmp, m_pIntegrated.dSemModelRef);
                // 초기화
                pIntegrated.Clear();
                // 초기화 데이터 복사
                pIntegrated.CopyFrom(m_pIntegrated);
                //

                // 데이터 저장
                IntegratedFile.Write(pIntegrated, nDataSize);
                IntegratedFile.Close();

                // 파일 오픈 : read mode
                if (!IntegratedFile.Open(strFileName, CFile.modeRead))
                {
                    return -1;
                }
            }

            // 데이터 크기
            uint nFileSize = (uint)IntegratedFile.GetLength();
            // 데이터 크기 비교
            if (nDataSize > nFileSize)
            {
                //IntegratedFile.Close();
                //return -1;
            }

            // 데이터 읽기
            IntegratedFile.Read(pIntegrated, nFileSize);// nDataSize);

            // 파일 닫기
            IntegratedFile.Close();

            // 데이터 복사
            m_pIntegrated.CopyFrom(pIntegrated);

            //
            // 데이터 오류 확인?
            //
            AfxCheckIntegratedDataError();
            //

            // 버퍼 삭제

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:3524 ----
        //
        // Integrated : INTEGRATED 데이터 오류 확인
        //
        public int AfxCheckIntegratedDataError()
        {
            // 공용 명령어 변수
            if (m_pIntegrated == null) return -1;

            //
            // 데이터 오류 확인?
            //
            if (m_pIntegrated.dScanOptionData[IDX_HWSETUP_OPTION_DOUBLE_MONITOR_SIZE] < 1.0)
            {
                // 모니터크기 기본 24인치로 재설정
                m_pIntegrated.dScanOptionData[IDX_HWSETUP_OPTION_DOUBLE_MONITOR_SIZE] = 24.0;
            }
            if (m_pIntegrated.dScanOptionData[IDX_HWSETUP_OPTION_DOUBLE_MONITOR_RATIO] < 1.0)
            {
                // 모니터크기 실제 적용율 98.5%로 재설정
                m_pIntegrated.dScanOptionData[IDX_HWSETUP_OPTION_DOUBLE_MONITOR_RATIO] = 98.5;
            }
            if ((int)strlen(m_pIntegrated.szScanOption32Text1) < 1)
            {
                // SEM DEVICE COMPANY 재설정
                strcpy_s(m_pIntegrated.szScanOption32Text1, SEM_DEVICE_COMPANY);
            }
            if ((int)strlen(m_pIntegrated.szScanOption32Text2) < 1)
            {
                // SEM DEVICE MODEL 재설정
                strcpy_s(m_pIntegrated.szScanOption32Text2, SEM_DEVICE_MODEL);
            }
            //
            if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_SCANMODE] < 1)
            {
                // AUTO FOCUS : SCAN MODE 재설정 --> FAST_SCAN_MODE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_SCANMODE] = FAST_SCAN_MODE;
            }
            if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_UP_NUMBER] < 1)
            {
                // AUTO FOCUS : UP SCAN 갯수 재설정 --> 20
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_UP_NUMBER] = 20;
            }
            if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_DOWN_NUMBER] < 1)
            {
                // AUTO FOCUS : DOWN SCAN 갯수 재설정 --> 10
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_DOWN_NUMBER] = 10;
            }
            if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_STEP_VALUE] < 1)
            {
                // AUTO FOCUS : SCAN STEP VALUE 재설정 --> 5
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_STEP_VALUE] = 5;
            }
            if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_YPOS_VALUE] < 1)
            {
                // AUTO FOCUS : FOCUS CHANGE Y-POS 재설정 --> 450
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_FOCUS_YPOS_VALUE] = 450;
            }
            //
            if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_CONTRAST_VALUE] < 1)
            {
                // AUTO CONTRAST : CONTRAST VALUE 재설정 --> 40 (CONTRAST 목표값)
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_CONTRAST_VALUE] = 40;
            }
            if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_CONTRAST_100JUMP] < 1)
            {
                // AUTO CONTRAST : CONTRAST 100 JUMP 재설정 --> 50 (CONTRAST값 검색 100 JUMP STEP)
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_CONTRAST_100JUMP] = 50;
            }
            if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_CONTRAST_10JUMP] < 1)
            {
                // AUTO CONTRAST : CONTRAST 10 JUMP STEP 재설정 --> 20 (CONTRAST 10 JUMP STEP 증가값)
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_CONTRAST_10JUMP] = 20;
            }
            if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_BRIGHT_VALUE] < 1)
            {
                // AUTO CONTRAST : BRIGHTNESS VALUE 재설정 --> 50 (BRIGHTNESS 목표값)
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_BRIGHT_VALUE] = 50;
            }
            if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_BRIGHT_100JUMP] < 1)
            {
                // AUTO CONTRAST : BRIGHTNESS 100 JUMP 재설정 --> 50 (BRIGHTNESS값 검색 100 JUMP STEP)
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_BRIGHT_100JUMP] = 50;
            }
            if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_BRIGHT_10JUMP] < 1)
            {
                // AUTO CONTRAST : BRIGHTNESS 10 JUMP STEP 재설정 --> 20 (BRIGHTNESS 10 JUMP STEP 증가값)
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_AUTO_BRIGHT_10JUMP] = 20;
            }
            //
            //
            //

            // 샘플높이
            m_dSampleHeight = m_pIntegrated.dScanOptionData[IDX_HWSETUP_OPTION_DOUBLE_SAMPLE_HEIGHT];
            // 샘플넓이
            m_dSampleWidth  = m_pIntegrated.dScanOptionData[IDX_HWSETUP_OPTION_DOUBLE_SAMPLE_WIDTH];
            // 샘플명칭
            if (strlen(m_pIntegrated.szScanOption128Text1) < 1)
            {
                // 샘플명칭 초기화
                strcpy(m_pIntegrated.szScanOption128Text1, "UnnameSample");
            }

            //
            //
            //

            //
            // 초기값 설정하기
            //
            // FOCUS COARSE/FINE OLD값
            m_pIntegrated.nFocusOldCoarse = SEM_FOCUS_COARSE_MAX;
            m_pIntegrated.nFocusOldFine   = SEM_FOCUS_FINE_MAX;

            // IMAGE INVERSE FLAG
            //m_pIntegrated->bNEGImageFlag = FALSE; // 음영반전
            //m_pIntegrated->bLRMirrorFlag = FALSE; // 좌우반전
            //m_pIntegrated->bTBMirrorFlag = FALSE; // 상하반전

            //
            //
            //

            
            // CL LMH MODE 전체 설정값 초기화 실행 FLAG
            if (m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MODE_INIT_FLAG] < 1)
            {
                // CL1/2 기준값
                int nCL1Base = m_pIntegrated.wCL1List[7]; // CL1 SPOT-SIZE : 8
                int nCL2Base = m_pIntegrated.wCL2List[7]; // CL2 SPOT-SIZE : 8
                // CL LMH MODE 전체 설정값을 초기화 하지 않았다면 초기화 해야 한다
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MODE_INIT_FLAG] = 1;
                // CL LMH MODE 전체 초기화
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MODE_USE_FLAG]      = 0;                    // CL LMH MODE 사용여부 FLAG : 0->기존 SPOTSIZE 사용, 1->CL LMH MODE 사용
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MODE_EXEC_SELECT]   = 0;                    // CL LMH MODE SCAN        에서 사용할 모드 : 0->CL LOW, 1->CL MIDDLE, 2->CL HIGH MODE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MODE_CALI_SELECT]   = 0;                    // CL LMH MODE CALIBRATION 에서 사용할 모드 : 0->CL LOW, 1->CL MIDDLE, 2->CL HIGH MODE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW_EXEC_SELECT]    = 0;                    // CL LMH MODE SCAN        에서 사용할 LOW    모드 : 0->LOW1   , 1->LOW2   , 2->LOW3    MODE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE_EXEC_SELECT] = 0;                    // CL LMH MODE SCAN        에서 사용할 MIDDLE 모드 : 0->MIDDLE1, 1->MIDDLE2, 2->MIDDLE3 MODE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH_EXEC_SELECT]   = 0;                    // CL LMH MODE SCAN        에서 사용할 HIGH   모드 : 0->HIGH1  , 1->HIGH2  , 2->HIGH3   MODE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW_CALI_SELECT]    = 0;                    // CL LMH MODE CALIBRATION 에서 사용할 LOW    모드 : 0->LOW1   , 1->LOW2   , 2->LOW3    MODE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE_CALI_SELECT] = 0;                    // CL LMH MODE CALIBRATION 에서 사용할 MIDDLE 모드 : 0->MIDDLE1, 1->MIDDLE2, 2->MIDDLE3 MODE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH_CALI_SELECT]   = 0;                    // CL LMH MODE CALIBRATION 에서 사용할 HIGH   모드 : 0->HIGH1  , 1->HIGH2  , 2->HIGH3   MODE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW1_CL1_VALUE]     = nCL1Base;             // CL LMH MODE LOW1    CL1 VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW1_CL2_VALUE]     = nCL2Base;             // CL LMH MODE LOW1    CL2 VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW2_CL1_VALUE]     = nCL1Base;             // CL LMH MODE LOW2    CL1 VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW2_CL2_VALUE]     = nCL2Base;             // CL LMH MODE LOW2    CL2 VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW3_CL1_VALUE]     = nCL1Base;             // CL LMH MODE LOW3    CL1 VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW3_CL2_VALUE]     = nCL2Base;             // CL LMH MODE LOW3    CL2 VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE1_CL1_VALUE]  = nCL1Base;             // CL LMH MODE MIDDLE1 CL1 VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE1_CL2_VALUE]  = nCL2Base;             // CL LMH MODE MIDDLE1 CL2 VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE2_CL1_VALUE]  = nCL1Base;             // CL LMH MODE MIDDLE2 CL1 VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE2_CL2_VALUE]  = nCL2Base;             // CL LMH MODE MIDDLE2 CL2 VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE3_CL1_VALUE]  = nCL1Base;             // CL LMH MODE MIDDLE3 CL1 VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE3_CL2_VALUE]  = nCL2Base;             // CL LMH MODE MIDDLE3 CL2 VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH1_CL1_VALUE]    = nCL1Base;             // CL LMH MODE HIGH1   CL1 VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH1_CL2_VALUE]    = nCL2Base;             // CL LMH MODE HIGH1   CL2 VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH2_CL1_VALUE]    = nCL1Base;             // CL LMH MODE HIGH2   CL1 VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH2_CL2_VALUE]    = nCL2Base;             // CL LMH MODE HIGH2   CL2 VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH3_CL1_VALUE]    = nCL1Base;             // CL LMH MODE HIGH3   CL1 VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH3_CL2_VALUE]    = nCL2Base;             // CL LMH MODE HIGH3   CL2 VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW1_GAX_VALUE]     = SEM_GUN_ALIGN_X_STR;  // CL LMH MODE LOW1    GAX VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW1_GAY_VALUE]     = SEM_GUN_ALIGN_Y_STR;  // CL LMH MODE LOW1    GAY VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW2_GAX_VALUE]     = SEM_GUN_ALIGN_X_STR;  // CL LMH MODE LOW2    GAX VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW2_GAY_VALUE]     = SEM_GUN_ALIGN_Y_STR;  // CL LMH MODE LOW2    GAY VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW3_GAX_VALUE]     = SEM_GUN_ALIGN_X_STR;  // CL LMH MODE LOW3    GAX VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW3_GAY_VALUE]     = SEM_GUN_ALIGN_Y_STR;  // CL LMH MODE LOW3    GAY VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE1_GAX_VALUE]  = SEM_GUN_ALIGN_X_STR;  // CL LMH MODE MIDDLE1 GAX VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE1_GAY_VALUE]  = SEM_GUN_ALIGN_Y_STR;  // CL LMH MODE MIDDLE1 GAY VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE2_GAX_VALUE]  = SEM_GUN_ALIGN_X_STR;  // CL LMH MODE MIDDLE2 GAX VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE2_GAY_VALUE]  = SEM_GUN_ALIGN_Y_STR;  // CL LMH MODE MIDDLE2 GAY VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE3_GAX_VALUE]  = SEM_GUN_ALIGN_X_STR;  // CL LMH MODE MIDDLE3 GAX VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE3_GAY_VALUE]  = SEM_GUN_ALIGN_Y_STR;  // CL LMH MODE MIDDLE3 GAY VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH1_GAX_VALUE]    = SEM_GUN_ALIGN_X_STR;  // CL LMH MODE HIGH1   GAX VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH1_GAY_VALUE]    = SEM_GUN_ALIGN_Y_STR;  // CL LMH MODE HIGH1   GAY VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH2_GAX_VALUE]    = SEM_GUN_ALIGN_X_STR;  // CL LMH MODE HIGH2   GAX VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH2_GAY_VALUE]    = SEM_GUN_ALIGN_Y_STR;  // CL LMH MODE HIGH2   GAY VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH3_GAX_VALUE]    = SEM_GUN_ALIGN_X_STR;  // CL LMH MODE HIGH3   GAX VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH3_GAY_VALUE]    = SEM_GUN_ALIGN_Y_STR;  // CL LMH MODE HIGH3   GAY VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW1_BAX_VALUE]     = SEM_BEAM_ALIGN_X_STR; // CL LMH MODE LOW1    BAX VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW1_BAY_VALUE]     = SEM_BEAM_ALIGN_Y_STR; // CL LMH MODE LOW1    BAY VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW2_BAX_VALUE]     = SEM_BEAM_ALIGN_X_STR; // CL LMH MODE LOW2    BAX VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW2_BAY_VALUE]     = SEM_BEAM_ALIGN_Y_STR; // CL LMH MODE LOW2    BAY VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW3_BAX_VALUE]     = SEM_BEAM_ALIGN_X_STR; // CL LMH MODE LOW3    BAX VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW3_BAY_VALUE]     = SEM_BEAM_ALIGN_Y_STR; // CL LMH MODE LOW3    BAY VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE1_BAX_VALUE]  = SEM_BEAM_ALIGN_X_STR; // CL LMH MODE MIDDLE1 BAX VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE1_BAY_VALUE]  = SEM_BEAM_ALIGN_Y_STR; // CL LMH MODE MIDDLE1 BAY VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE2_BAX_VALUE]  = SEM_BEAM_ALIGN_X_STR; // CL LMH MODE MIDDLE2 BAX VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE2_BAY_VALUE]  = SEM_BEAM_ALIGN_Y_STR; // CL LMH MODE MIDDLE2 BAY VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE3_BAX_VALUE]  = SEM_BEAM_ALIGN_X_STR; // CL LMH MODE MIDDLE3 BAX VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE3_BAY_VALUE]  = SEM_BEAM_ALIGN_Y_STR; // CL LMH MODE MIDDLE3 BAY VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH1_BAX_VALUE]    = SEM_BEAM_ALIGN_X_STR; // CL LMH MODE HIGH1   BAX VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH1_BAY_VALUE]    = SEM_BEAM_ALIGN_Y_STR; // CL LMH MODE HIGH1   BAY VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH2_BAX_VALUE]    = SEM_BEAM_ALIGN_X_STR; // CL LMH MODE HIGH2   BAX VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH2_BAY_VALUE]    = SEM_BEAM_ALIGN_Y_STR; // CL LMH MODE HIGH2   BAY VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH3_BAX_VALUE]    = SEM_BEAM_ALIGN_X_STR; // CL LMH MODE HIGH3   BAX VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH3_BAY_VALUE]    = SEM_BEAM_ALIGN_Y_STR; // CL LMH MODE HIGH3   BAY VALUE
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_SPOT_GAX_VALUE]     = SEM_GUN_ALIGN_X_STR;  // CL SPOT-SIZE --> CL LMH MODE 전환시 GAX VALUE BACKUP
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_SPOT_GAY_VALUE]     = SEM_GUN_ALIGN_Y_STR;  // CL SPOT-SIZE --> CL LMH MODE 전환시 GAY VALUE BACKUP
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_SPOT_BAX_VALUE]     = SEM_BEAM_ALIGN_X_STR; // CL SPOT-SIZE --> CL LMH MODE 전환시 BAX VALUE BACKUP
                m_pIntegrated.nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_SPOT_BAY_VALUE]     = SEM_BEAM_ALIGN_Y_STR; // CL SPOT-SIZE --> CL LMH MODE 전환시 BAY VALUE BACKUP
            }
            // CL LMH MODE : SCAN(EXEC) SELECT MODE --> CALI SELECT MODE 에 적용
            AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MODE_CALI_SELECT]   = AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MODE_EXEC_SELECT];
            AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW_CALI_SELECT]    = AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_LOW_EXEC_SELECT];
            AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE_CALI_SELECT] = AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_MIDDLE_EXEC_SELECT];
            AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH_CALI_SELECT]   = AfxGetIntegratedPointer().nScanOptionData[IDX_HWSETUP_OPTION_INT_CL_LMH_HIGH_EXEC_SELECT];

            //
            //
            //


            //
            //
            //


            // STAGE OPTION ERROR CHECK?
            if (m_pIntegrated.dStageOptionData[IDX_STAGE_OPTION_DOUBLE_BSE_POSITION] < 0.0 || m_pIntegrated.dStageOptionData[IDX_STAGE_OPTION_DOUBLE_BSE_POSITION] > 100.0)
            {
                // BSE SET SCAN POSITION
                m_pIntegrated.dStageOptionData[IDX_STAGE_OPTION_DOUBLE_BSE_POSITION] = 0.0;
            }

            //
            //
            //


            return 0;
        }

        // ---- USB_FT232H_Device.cpp:3756 ----
        //
        // Integrated : INTEGRATED 파일 저장하기
        //
        public int AfxSetIntegratedDataToFile()
        {
            // 공용 명령어 변수
            if (m_pIntegrated == null) return -1;

            // 공용 변수 생성
            IntegratedCommandStruct pIntegrated = new IntegratedCommandStruct();

            // 초기화
            pIntegrated.Clear();
            // 초기화 데이터 복사
            pIntegrated.CopyFrom(m_pIntegrated);

            int nDataSize = IntegratedCommandStruct.NativeSize;
            string strFileName = "";
            strFileName = m_strDeviceServerPath + "\\" + DEVICE_INTEGRATED_FILE_NAME;

            CFile IntegratedFile = new CFile();

            // INI 파일이 있는지 확인한다.
            // Integrated Data File Load
            if (!IntegratedFile.Open(strFileName, CFile.modeRead))
            {
                // Open Error
                return -1;
            }
            // 닫기
            IntegratedFile.Close();

            // INI 파일이 존재 하면 저장한다
            // 파일 오픈 : write mdoe
            if (!IntegratedFile.Open(strFileName, CFile.modeWrite))
            {
                return -1;
            }

            // 데이터 저장
            IntegratedFile.Write(pIntegrated, nDataSize);

            // 파일 닫기
            IntegratedFile.Close();

            // 버퍼 삭제

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:3807 ----
        //
        // Integrated Utility : Public Function & Parameters Initilize (공용함수 및 변수 초기화)
        //
        public int AfxInitilizePublicParameters()
        {
            int n = 0;

            // 파라메터 인덱스 초기값
            m_nPublicToolsIndex = -1;

            // Public Tools Process 쓰레드 & 파라메터
            for (n = 0; n < MAX_PUBLIC_THREAD; n++)
            {
                m_pPublicToolsThread[n] = null; // 쓰레드
                m_pPublicToolsParams[n] = null; // 파라메터
            }

            // 쓰레드가 종료 될 때 까지 기다림
            // 아래의 함수는 예를 들어 Gun HV Up 기능을 쓰레드로 실행하고 
            // Gun HV Up 기능이 종료될 때 까지 기다릴수 있다(쓰레드 종료) <-방법1
            // WaitForSingleObject(m_pThread->m_hThread, INFINITE);
            // 방법2->파라메터에 쓰레드 실행상태 관련 변수를 이용하여 쓰레드가 자동삭제 되었는지 확인

            //
            m_pIntegrated.bSemConnectedFlag = m_bSemImageConnected;   // USB FT232 SE Image Scan Board Connected Flag
            m_pIntegrated.bBseConnectedFlag = m_bBseImageConnected;   // USB FT232 BSE Image Scan Board Connected Flag
            m_pIntegrated.bSbrConnectedFlag = m_bScanBridgeConnected; // USB FT232 Scan Bridge Board Connected Flag
            //

            //
            // 시스템 변수 초기화
            //
            // SEM Model 초기화 및 사용 변수 초기화
            AfxIntegratedInitializeParameters(m_pIntegrated.nSemModel);// SEM_SM_300B_CL_MODEL);
            // Integrated Control : WD에 따른 OL_Coarse, ScanRotation, Zooming, MinMag 보정값 설정
            AfxIntegratedSetWDCompensation();
            // Integrated Control : WD에 따른 OL_Coarse, ScanRotation, Zooming, MinMag 보정값 10등분 선형화 작업
            AfxIntegratedSetWDOLSrotZoomingLinearize();                                
            //

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:3868 ----
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
        // Integrated : SCAN ROTATION CALIBRATION DATA 파일 불러오기
        //
        public int AfxGetScanRotationCalibrationDataFromFile()
        {
            // SCAN ROTATION CALIBRATION DATA 삭제
            if (m_pScanRotCaliData != null)
            {

                m_pScanRotCaliData = null;
            }

            // SCAN ROTATION CALIBRATION DATA 생성
            m_pScanRotCaliData = new ScanRotationCalibrationData();

            // 초기화
            m_pScanRotCaliData.Clear();

            uint nDataSize = ScanRotationCalibrationData.NativeSize;
            string strFileName = "";
            strFileName = m_strDeviceServerPath + "\\" + SEM_SROT_CALIBRATION_FILE_NAME;

            CFile fileSRotCaliData = new CFile();

            // SCAN ROTATION CALIBRATION DATA FILE LOAD
            if (!fileSRotCaliData.Open(strFileName, CFile.modeRead))
            {
                // Open or Read Error
                if (!fileSRotCaliData.Open(strFileName, CFile.modeCreate))
                {
                    // File Create Error
                    return -1;
                }
                fileSRotCaliData.Close();

                // Initilize Integrated Data To File
                if (!fileSRotCaliData.Open(strFileName, CFile.modeWrite))
                {
                    return -1;
                }

                //
                // DATA 초기화
                //
                // 초기화
                AfxScanRotationCalibrationDataInitialize(m_pScanRotCaliData);
                //

                // 데이터 저장
                fileSRotCaliData.Write(m_pScanRotCaliData, nDataSize);
                fileSRotCaliData.Close();

                // 파일 오픈 : read mode
                if (!fileSRotCaliData.Open(strFileName, CFile.modeRead))
                {
                    return -1;
                }
            }

            // 데이터 크기
            uint nFileSize = (uint)fileSRotCaliData.GetLength();
            // 데이터 크기 비교
            if (nFileSize != ScanRotationCalibrationData.NativeSize)
            {
                // 파일 닫기
                fileSRotCaliData.Close();
                return -1;
            }

            // 데이터 읽기
            fileSRotCaliData.Read(m_pScanRotCaliData, nFileSize);// nDataSize);

            // 파일 닫기
            fileSRotCaliData.Close();


            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:3947 ----
        //
        // Integrated : SCAN ROTATION CALIBRATION DATA 파일 저장하기
        //
        public int AfxSetScanRotationCalibrationDataToFile()
        {
            // SCAN ROTATION CALIBRATION DATA 변수
            if (m_pScanRotCaliData == null) return -1;

            // SCAN ROTATION CALIBRATION DATA 생성
            ScanRotationCalibrationData pSRotCaliData = new ScanRotationCalibrationData();

            // 초기화
            pSRotCaliData.Clear();
            // 초기화 데이터 복사
            pSRotCaliData.CopyFrom(m_pScanRotCaliData);

            int nDataSize = ScanRotationCalibrationData.NativeSize;
            string strFileName = "";
            strFileName = m_strDeviceServerPath + "\\" + SEM_SROT_CALIBRATION_FILE_NAME;

            CFile fileSRotCaliData = new CFile();

            // INI 파일이 있는지 확인한다.
            // fileSRotCaliData Data File Load
            if (!fileSRotCaliData.Open(strFileName, CFile.modeRead))
            {
                // Open Error
                return -1;
            }
            // 닫기
            fileSRotCaliData.Close();

            // INI 파일이 존재 하면 저장한다
            // 파일 오픈 : write mdoe
            if (!fileSRotCaliData.Open(strFileName, CFile.modeWrite))
            {
                return -1;
            }

            // 데이터 저장
            fileSRotCaliData.Write(pSRotCaliData, nDataSize);

            // 파일 닫기
            fileSRotCaliData.Close();

            // 버퍼 삭제

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:3998 ----
        //
        // Integrated : SCAN ROTATION CALIBRATION DATA 초기화
        //
        public int AfxScanRotationCalibrationDataInitialize(ScanRotationCalibrationData pSRotCaliData)
        {
            // Scan Rotation Calibration Data Initilize
            // Scan Rotation에 따른 Orthogonal, Zoom X-Y 보정값
            pSRotCaliData.Clear();

            ///
            // 초기값 설정
            ///
            // Flag
            for (int k = 0; k < MAX_SROT_CALIBRATION_ANGLE; k++)
            {
                pSRotCaliData.bSRotCaliFlag[k] = FALSE;
            }
            ///
            // Orthogonal 보정값
            pSRotCaliData.nSRotOrthogonalInput[0]  = m_pIntegrated.nARAngle[0]; // 입력값 (0도)
            pSRotCaliData.nSRotOrthogonalInput[15] = m_pIntegrated.nARAngle[0]; // 입력값 (15도)
            pSRotCaliData.nSRotOrthogonalInput[30] = m_pIntegrated.nARAngle[0]; // 입력값 (30도)
            pSRotCaliData.nSRotOrthogonalInput[45] = m_pIntegrated.nARAngle[0]; // 입력값 (45도)
            pSRotCaliData.nSRotOrthogonalInput[60] = m_pIntegrated.nARAngle[0]; // 입력값 (60도)
            pSRotCaliData.nSRotOrthogonalInput[75] = m_pIntegrated.nARAngle[0]; // 입력값 (75도)
            pSRotCaliData.nSRotOrthogonalInput[90] = m_pIntegrated.nARAngle[0]; // 입력값 (90도)
            ///

            // Orthogonal 보정값 : 계산값 (0.1도씩)
            int nDivRot = 0;
            for (int k = 0; k < MAX_SROT_CALIBRATION_ANGLE; k++)
            {
                for (int i = 0; i < 10; i++) // 10등분
                {
                    nDivRot = k * 10;
                    pSRotCaliData.nSRotOrthogonalData[nDivRot + i] = m_pIntegrated.nARAngle[0];
                }
            }

            ///
            // IniPar.ZoomXMaxValue[0]~[4] = 4095 값으로 설정 되어 있음
            // IniPar.ZoomYMaxValue[0]~[4] = 4095 값으로 설정 되어 있음
            // Magnification Relay는 4단계로 구성됨
            // Relay 0 : x20    ~ x250       --> ratio = 배율/30
            // Relay 1 : x300   ~ x2,500     --> ratio = 배율/300
            // Relay 2 : x3000  ~ x30,000    --> ratio = 배율/3000
            // Relay 3 : x30000 ~ x300,000   --> ratio = 배율/30000
            ///
            // Magnification Table
            // Relay 0     :     20,     30,     35,     40,     45,     50,     60,     70,     80,     90,    100,    110,    120,    130,    150,    180,    200,    220,    250,
            // Relay 1     :            300,    350,    400,    450,    500,    600,    700,    800,    900,   1000,   1100,   1200,   1300,   1500,   1800,   2000,   2200,   2500,
            // Relay 2     :           3000,   3500,   4000,   4500,   5000,   6000,   7000,   8000,   9000,  10000,  11000,  12000,  13000,  15000,  18000,  20000,  22000,  25000,
            // Relay 3     :          30000,  35000,  40000,  45000,  50000,  60000,  70000,  80000,  90000, 100000, 110000, 120000, 130000, 150000, 180000, 200000, 220000, 250000, 300000
            // ratio(20개) : 0.6667,    1.0, 1.1667, 1.3334,    1.5, 1.6667,    2.0, 2.3334, 2.6667,    3.0, 3.3334, 3.6667,    4.0, 4.3334,    5.0,    6.0, 6.6667, 7.3334, 8.3334,   10.0
            ///
            ushort wZoom = 0;
            double dZoomXYMax = m_nZoomXMaxValue[0];
            if (m_nZoomXMaxValue[0] < m_nZoomYMaxValue[0]) dZoomXYMax = m_nZoomYMaxValue[0];
            ///
            // Mag Relay Zoom X, Y 보정값 : Input 값
            for (int i = 0; i < MAX_SROT_CALIBRATION_ZOOM; i++)
            {
                switch (i)
                {
                case  0: { wZoom = (ushort)(dZoomXYMax / 1.0     ); break; } // x20
                case  1: { wZoom = (ushort)(dZoomXYMax / 1.0     ); break; } // x30,x300,x3000,x30000
                case  2: { wZoom = (ushort)(dZoomXYMax / 1.166667); break; } // x35,x350,x3500,x35000
                case  3: { wZoom = (ushort)(dZoomXYMax / 1.333334); break; } // x40,x400,x4000,x40000
                case  4: { wZoom = (ushort)(dZoomXYMax / 1.5     ); break; } // x45,x450,x4500,x45000
                case  5: { wZoom = (ushort)(dZoomXYMax / 1.666667); break; } // x50,x500,x5000,x50000
                case  6: { wZoom = (ushort)(dZoomXYMax / 2.0     ); break; } // x60,x600,x6000,x60000
                case  7: { wZoom = (ushort)(dZoomXYMax / 2.333334); break; } // x70,x700,x7000,x70000
                case  8: { wZoom = (ushort)(dZoomXYMax / 2.666667); break; } // x80,x800,x8000,x80000
                case  9: { wZoom = (ushort)(dZoomXYMax / 3.0     ); break; } // x90,x900,x9000,x90000
                case 10: { wZoom = (ushort)(dZoomXYMax / 3.333334); break; } // x100,x1000,x10000,x100000
                case 11: { wZoom = (ushort)(dZoomXYMax / 3.666667); break; } // x110,x1100,x11000,x110000
                case 12: { wZoom = (ushort)(dZoomXYMax / 4.0     ); break; } // x120,x1200,x12000,x120000
                case 13: { wZoom = (ushort)(dZoomXYMax / 4.333334); break; } // x130,x1300,x13000,x130000
                case 14: { wZoom = (ushort)(dZoomXYMax / 5.0     ); break; } // x150,x1500,x15000,x150000
                case 15: { wZoom = (ushort)(dZoomXYMax / 6.0     ); break; } // x180,x1800,x18000,x180000
                case 16: { wZoom = (ushort)(dZoomXYMax / 6.666667); break; } // x200,x2000,x20000,x200000
                case 17: { wZoom = (ushort)(dZoomXYMax / 7.333334); break; } // x220,x2200,x22000,x220000
                case 18: { wZoom = (ushort)(dZoomXYMax / 8.333334); break; } // x250,x2500,x25000,x250000
                case 19: { wZoom = (ushort)(dZoomXYMax / 10.0    ); break; } // x300000
                default: { wZoom = (ushort)(dZoomXYMax / 1.0     ); break; } //
                }

                pSRotCaliData.nSRotRelayZoomXInput[i, 0]   = pSRotCaliData.nSRotRelayZoomYInput[i, 0]   = wZoom; // 입력값 : 0도
                pSRotCaliData.nSRotRelayZoomXInput[i, 15]  = pSRotCaliData.nSRotRelayZoomYInput[i, 15]  = wZoom; // 입력값 : 15도
                pSRotCaliData.nSRotRelayZoomXInput[i, 30]  = pSRotCaliData.nSRotRelayZoomYInput[i, 30]  = wZoom; // 입력값 : 30도
                pSRotCaliData.nSRotRelayZoomXInput[i, 45]  = pSRotCaliData.nSRotRelayZoomYInput[i, 45]  = wZoom; // 입력값 : 45도
                pSRotCaliData.nSRotRelayZoomXInput[i, 60]  = pSRotCaliData.nSRotRelayZoomYInput[i, 60]  = wZoom; // 입력값 : 60도
                pSRotCaliData.nSRotRelayZoomXInput[i, 75]  = pSRotCaliData.nSRotRelayZoomYInput[i, 75]  = wZoom; // 입력값 : 75도
                pSRotCaliData.nSRotRelayZoomXInput[i, 90]  = pSRotCaliData.nSRotRelayZoomYInput[i, 90]  = wZoom; // 입력값 : 90도
                pSRotCaliData.nSRotRelayZoomXInput[i, 105] = pSRotCaliData.nSRotRelayZoomYInput[i, 105] = wZoom; // 입력값 : 105도
                pSRotCaliData.nSRotRelayZoomXInput[i, 120] = pSRotCaliData.nSRotRelayZoomYInput[i, 120] = wZoom; // 입력값 : 120도
                pSRotCaliData.nSRotRelayZoomXInput[i, 135] = pSRotCaliData.nSRotRelayZoomYInput[i, 135] = wZoom; // 입력값 : 135도
                pSRotCaliData.nSRotRelayZoomXInput[i, 150] = pSRotCaliData.nSRotRelayZoomYInput[i, 150] = wZoom; // 입력값 : 150도
                pSRotCaliData.nSRotRelayZoomXInput[i, 165] = pSRotCaliData.nSRotRelayZoomYInput[i, 165] = wZoom; // 입력값 : 165도
                pSRotCaliData.nSRotRelayZoomXInput[i, 180] = pSRotCaliData.nSRotRelayZoomYInput[i, 180] = wZoom; // 입력값 : 180도
                pSRotCaliData.nSRotRelayZoomXInput[i, 195] = pSRotCaliData.nSRotRelayZoomYInput[i, 195] = wZoom; // 입력값 : 195도
                pSRotCaliData.nSRotRelayZoomXInput[i, 210] = pSRotCaliData.nSRotRelayZoomYInput[i, 210] = wZoom; // 입력값 : 210도
                pSRotCaliData.nSRotRelayZoomXInput[i, 225] = pSRotCaliData.nSRotRelayZoomYInput[i, 225] = wZoom; // 입력값 : 225도
                pSRotCaliData.nSRotRelayZoomXInput[i, 240] = pSRotCaliData.nSRotRelayZoomYInput[i, 240] = wZoom; // 입력값 : 240도
                pSRotCaliData.nSRotRelayZoomXInput[i, 255] = pSRotCaliData.nSRotRelayZoomYInput[i, 255] = wZoom; // 입력값 : 255도
                pSRotCaliData.nSRotRelayZoomXInput[i, 270] = pSRotCaliData.nSRotRelayZoomYInput[i, 270] = wZoom; // 입력값 : 270도
                pSRotCaliData.nSRotRelayZoomXInput[i, 285] = pSRotCaliData.nSRotRelayZoomYInput[i, 285] = wZoom; // 입력값 : 285도
                pSRotCaliData.nSRotRelayZoomXInput[i, 300] = pSRotCaliData.nSRotRelayZoomYInput[i, 300] = wZoom; // 입력값 : 300도
                pSRotCaliData.nSRotRelayZoomXInput[i, 315] = pSRotCaliData.nSRotRelayZoomYInput[i, 315] = wZoom; // 입력값 : 315도
                pSRotCaliData.nSRotRelayZoomXInput[i, 330] = pSRotCaliData.nSRotRelayZoomYInput[i, 330] = wZoom; // 입력값 : 330도
                pSRotCaliData.nSRotRelayZoomXInput[i, 345] = pSRotCaliData.nSRotRelayZoomYInput[i, 345] = wZoom; // 입력값 : 345도

                // Mag Relay Zoom X, Y 보정값 : Calc 값
                for (int k = 0; k < MAX_SROT_CALIBRATION_ANGLE; k++)
                {
                    pSRotCaliData.nSRotRelayZoomXData[i, k] = wZoom;
                    pSRotCaliData.nSRotRelayZoomYData[i, k] = wZoom;
                }
            }
            ///
            ///
            ///
            /*
            // S-ROT CALIBRATION DATA 복사
            for (int k = 0; k < MAX_SROT_CALIBRATION_ANGLE; k++)
            {
                // Flag
                m_SRotCal_Flag[k] = pSRotCaliData->bSRotCaliFlag[k];

                // Orthogonal 보정값 : 입력값 (1도씩)
                m_SRotCal_OrthogonalInput[k] = pSRotCaliData->nSRotOrthogonalInput[k];

                // Orthogonal 보정값
                for (int i = 0; i < 10; i++) // 10 등분
                {
                    nDivRot = k * 10;
                    // Orthogonal 보정값 : 계산값 (0.1도씩)
                    m_SRotCal_OrthogonalData[nDivRot + i] = pSRotCaliData->nSRotOrthogonalData[nDivRot + i];
                }

                // Relay Zoom X-Y 보정값
                for (int i = 0; i < MAX_SROT_CALIBRATION_ZOOM; i++)
                {
                    // Mag Relay Zoom X, Y 보정값 : Input 값
                    m_SRotCal_InputZoomX[i][k] = pSRotCaliData->nSRotRelayZoomXInput[i][k];
                    m_SRotCal_InputZoomY[i][k] = pSRotCaliData->nSRotRelayZoomYInput[i][k];

                    // Mag Relay Zoom X, Y 보정값 : Calc 값
                    m_SRotCal_CalcZoomX[i][k]  = pSRotCaliData->nSRotRelayZoomXData[i][k];
                    m_SRotCal_CalcZoomY[i][k]  = pSRotCaliData->nSRotRelayZoomYData[i][k];
                }
            }
            */
            ///
            ///
            ///

            return 0;
        }

        // ---- USB_FT232H_Device.cpp:4177 ----
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
        // Integrated Utility : SEM MODEL 선택
        //
        public int AfxSetSemModel(int nModel)
        {
            // SEM Model 선택
            m_pIntegrated.nSemModel = nModel;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:4188 ----
        //
        // Integrated Utility : SEM MODEL 선택 및 AMP, REF 설정
        //
        public int AfxSetSemModel(int nModel, double dAmp, double dRef)
        {
            // SEM Model 선택
            m_pIntegrated.nSemModel = nModel;
            // SEM Model AMP
            m_pIntegrated.dSemModelAmp = dAmp;
            // SEM Model REF
            m_pIntegrated.dSemModelRef = dRef;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:4203 ----
        //
        // Integrated Utility : Public Function & Parameters Release (공용함수 및 변수 삭제)
        //
        public int AfxReleasePublicParameters()
        {
            if (m_pPublicToolsParams != null)
            {
                int n = 0;
                // 파라메터 인덱스 초기값
                m_nPublicToolsIndex = -1;

                // Public Tools Process 쓰레드 & 파라메터
                for (n = 0; n < MAX_PUBLIC_THREAD; n++)
                {
                    // Public Tools Process 쓰레드 파라메터 중복 확인
                    if (m_pPublicToolsParams[n] != null)
                    {
                        // Public Tools Process 쓰레드에서 사용한 파라메터 삭제

                        m_pPublicToolsParams[n] = null;
                    }
                }
                // 파라메터 Array 삭제
                //delete[] & m_pPublicToolsParams;
            }

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:4232 ----
        //
        // Integrated Control : SEM VACUUM STATE 설정
        //
        public int AfxSetSEMVacuumState(int nState)
        {
            if (m_pIntegrated == null) return RET_ERROR_INTEGRATED_COMMAND_HANDLE;

            // SEM 상태 설정
            m_pIntegrated.nSemVacState = nState;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:4278 ----
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
        // Scan Bridge Device Command Control...
        //
        //
        //


        //
        // Scan Bridge : 명령어 및 데이터 획득 쓰레드 생성
        //
        public int AfxCreateScanBridgeThread()
        {
            // Scan Bridge 가 사용 중이면...
            if (m_pScanBridgeThread != null) return RET_SUCCESS_OK;
            // Scan Bridge 쓰레드 파라메터 중복 확인
            if (m_pScanBridgeThreadParams != null)
            {
                // Scan Bridge 쓰레드에서 사용한 파라메터 삭제

                m_pScanBridgeThreadParams = null;
            }

            // Scan Bridge 쓰레드에서 사용할 파라메터를 생성한다
            m_pScanBridgeThreadParams = new SCANBRIDGETHREADPARAMS();
            if (m_pScanBridgeThreadParams == null) return RET_ERROR_SBR_THREAD_PARAMS;
            m_pScanBridgeThreadParams = new SCANBRIDGETHREADPARAMS();
            // Scan Bridge 쓰레드에서 사용할 파라메터 기본값 설정
            m_pScanBridgeThreadParams.bThreadDone = FALSE;         // 쓰레드 종료 설정
            m_pScanBridgeThreadParams.bThreadStop = FALSE;         // 쓰레드 강제 종료 설정
            //m_pScanBridgeThreadParams->pSbrCommand = m_pSbrCommand; // Scan Bridge Command Data
            m_pScanBridgeThreadParams.nFT232HIndex = IDX_FT232H_SCAN_BRIDGE_DEVICE; // Scan Bridge 장치 인덱스
            m_pScanBridgeThreadParams.hFT232Handle = IntPtr.Zero;         // Scan Bridge 장치 핸들
            m_pScanBridgeThreadParams.hEventHandle = null;         // Scan Bridge 이벤트 핸들
            //
            /*
            memset(m_pScanBridgeThreadParams->szSbrRxBuffer, 0x00, sizeof(m_pScanBridgeThreadParams->szSbrRxBuffer)); // Scan Bridge Read Buffer
            memset(m_pScanBridgeThreadParams->szCmdBuffer  , 0x00, sizeof(m_pScanBridgeThreadParams->szCmdBuffer  )); // Scan Bridge Command Buffer
            memset(m_pScanBridgeThreadParams->szXStrBuffer , 0x00, sizeof(m_pScanBridgeThreadParams->szXStrBuffer )); // Scan Bridge XStr Buffer
            memset(m_pScanBridgeThreadParams->szYStrBuffer , 0x00, sizeof(m_pScanBridgeThreadParams->szYStrBuffer )); // Scan Bridge YStr Buffer
            memset(m_pScanBridgeThreadParams->szDStrBuffer , 0x00, sizeof(m_pScanBridgeThreadParams->szDStrBuffer )); // Scan Bridge DStr Buffer
            memset(m_pScanBridgeThreadParams->szSStrBuffer , 0x00, sizeof(m_pScanBridgeThreadParams->szSStrBuffer )); // Scan Bridge SStr Buffer
            m_pScanBridgeThreadParams->nCmdCount    = 0;            // Scan Bridge Command Count
            m_pScanBridgeThreadParams->nXStrCount   = 0;            // Scan Bridge XStr Count
            m_pScanBridgeThreadParams->nYStrCount   = 0;            // Scan Bridge YStr Count
            m_pScanBridgeThreadParams->nDStrCount   = 0;            // Scan Bridge DStr Count
            m_pScanBridgeThreadParams->nSStrCount   = 0;            // Scan Bridge SStr Count
            m_pScanBridgeThreadParams->nCommaCount  = 0;            // Scan Bridge Comma Count
            m_pScanBridgeThreadParams->nXYDState    = 'x';          // Scan Bridge XYD State Flag
            m_pScanBridgeThreadParams->nCommandViewFlag   = 0;      // Scan Bridge Command View Falg
            memset(m_pScanBridgeThreadParams->szEchoCmdStr, 0x00, sizeof(m_pScanBridgeThreadParams->szEchoCmdStr)); // Scan Bridge Echo Command Str
            m_pScanBridgeThreadParams->nEchoCommandEnable = 0;      // Scan Bridge Echo Command Enable
            m_pScanBridgeThreadParams->nEchoCommandFlag   = 0;      // Scan Bridge Echo Command Flag
            */
            //
            // 2020.05.13
            ZeroMem(m_szFT232H_SbrRxBuffer); // Scan Bridge Read Buffer
            ZeroMem(m_szFT232H_CmdBuffer); // Scan Bridge Command Buffer
            memset(m_szFT232H_XStrBuffer , 0x00, MAX_FT232H_XYDSStr_BUFFER); // Scan Bridge XStr Buffer
            memset(m_szFT232H_YStrBuffer , 0x00, MAX_FT232H_XYDSStr_BUFFER); // Scan Bridge YStr Buffer
            memset(m_szFT232H_DStrBuffer , 0x00, MAX_FT232H_XYDSStr_BUFFER); // Scan Bridge DStr Buffer
            memset(m_szFT232H_SStrBuffer , 0x00, MAX_FT232H_XYDSStr_BUFFER); // Scan Bridge SStr Buffer
            ZeroMem(m_szFT232H_EchoCmdStr); // Scan Bridge Echo Command Str  
            m_nFT232H_CmdCount   = 0;                                        // Scan Bridge Command Count
            m_nFT232H_XStrCount  = 0;                                        // Scan Bridge XStr Count
            m_nFT232H_YStrCount  = 0;                                        // Scan Bridge YStr Count
            m_nFT232H_DStrCount  = 0;                                        // Scan Bridge DStr Count
            m_nFT232H_SStrCount  = 0;                                        // Scan Bridge SStr Count
            m_nFT232H_CommaCount = 0;                                        // Scan Bridge Comma Count
            m_nFT232H_XYDState   = (byte)'x';                                      // Scan Bridge XYD State Flag
            m_nFT232H_CommandViewFlag   = 0;                                 // Scan Bridge Command View Falg
            m_nFT232H_EchoCommandEnable = 0;                                 // Scan Bridge Echo Command Enable
            m_nFT232H_EchoCommandFlag   = 0;                                 // Scan Bridge Echo Command Flag
            //

            // 핸들 설정
            IntPtr ft232hHandle = AfxFT232HGetHandle(m_pScanBridgeThreadParams.nFT232HIndex);
            if (ft232hHandle == IntPtr.Zero) return RET_ERROR_FT232H_SBR_HANDLE;
            m_pScanBridgeThreadParams.hFT232Handle = ft232hHandle; // Scan Bridge 장치 핸들
            
            // Scan Bridge 이벤트 마스크
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
            m_pScanBridgeThreadParams.hEventHandle = CreateEvent(NULL, FALSE, FALSE, "");              // Scan Bridge 쓰레드에서 사용할 이벤트 생성
            if (m_pScanBridgeThreadParams.hEventHandle == null) return RET_ERROR_SBR_EVENT_HANDLE;         // 이벤트 생성 오류 확인
            dwEventMask = FT_EVENT_RXCHAR;                                                                  // Scan Bridge 장치에서 사용할 이벤트 항목 설정
            ftStatus = AfxFT232HSetEventNotification(m_pScanBridgeThreadParams.nFT232HIndex, dwEventMask, m_pScanBridgeThreadParams.hEventHandle); // Scan Bridge 쓰레드에서 사용할 이벤트 설정
            if (ftStatus != FT_OK) return RET_ERROR_SBR_EVENT_MASK;
            //

            // Scan Bridge 쓰레드 생성 및 시작...
            m_pScanBridgeThread = AfxBeginThread(AfxThreadScanBridgeProcessing, m_pScanBridgeThreadParams); // , THREAD_PRIORITY_NORMAL);
            if (m_pScanBridgeThread == null) return RET_ERROR_SBR_THREAD_HANDLE;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:4379 ----
        //
        // Scan Bridge : 명령어 및 데이터 획득 처리(작업) 쓰레드
        //
        public uint AfxThreadScanBridgeProcessing(object pParams)
        {
            SCANBRIDGETHREADPARAMS pThreadParams = (SCANBRIDGETHREADPARAMS)pParams;

            // 쓰레드 작업 상태 설정
            pThreadParams.bThreadDone = FALSE;

            int  bLoop = TRUE;
            uint dwRet = 0;

            //
            // Scan Bridge 쓰레드 프로세싱
            //
            while (bLoop == TRUE)
            {
                
                //
                // Scan Bridge 장치 이벤트 발생 확인...타임아웃 1000[ms]
                //
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
                            // Scan Bridge : 수신된 명령어 데이터 처리(실행) 하기
                            AfxScanBridgeDataRead();
                            break;
                        }
                        case WAIT_TIMEOUT:   // 이벤트 대기시간 경과 : Loop 순환
                        {
                            // 쓰레드 순환
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

                // Scan Bridge : 수신된 명령어 데이터 처리(실행) 하기
                //AfxScanBridgeDataRead();

                // 쓰레드 강제 종료 확인
                if (pThreadParams.bThreadStop == TRUE)
                {
                    bLoop = FALSE;
                }
            }

            //
            // 쓰레드 종료 확인
            pThreadParams.bThreadDone = TRUE;
            //

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:4452 ----
        //
        // Scan Bridge : 수신된 명령어 데이터 처리(실행) 하기
        //
        public int AfxScanBridgeDataRead()
        {
            if (m_pScanBridgeThreadParams == null) return RET_ERROR_SBR_THREAD_PARAMS;               // Scan Bridge 쓰레드 파라메터
            if (m_pScanBridgeThreadParams.hFT232Handle == IntPtr.Zero) return RET_ERROR_FT232H_SBR_HANDLE; // Scan Bridge 장치 핸들

            int   n  = 0;
            byte  bt = 0;

            uint dwBytesRead = 0;
            uint dwRXBytes   = 0;
            uint dwTXBytes   = 0;
            uint dwEvent     = 0;

            uint ftStatus = 0;

            //
            // Scan Bridge USB Data Read and Command Execute...
            //
            AfxFT232HGetStatus(m_pScanBridgeThreadParams.nFT232HIndex, ref dwRXBytes, ref dwTXBytes, ref dwEvent);

            // 받은 데이터 확인
            if (dwRXBytes > 0)
            {
                // 2020.05.13
                // RECEIVE BUFFER CLEAR
                ZeroMem(m_szFT232H_SbrRxBuffer); // Scan Bridge Read Buffer

                // 2020.05.13
                // 자료갯수 확인
                if (dwRXBytes > MAX_FT232H_READ_BUFFER)
                {
                    // 설정한 버퍼 크기 보다 큰 경우
                    dwRXBytes = MAX_FT232H_READ_BUFFER;
                }

                // 2020.05.13
                // USB Data Read
                ftStatus = AfxFT232HRead(m_pScanBridgeThreadParams.nFT232HIndex, m_szFT232H_SbrRxBuffer, dwRXBytes, ref dwBytesRead);
                if (ftStatus != FT_OK) return RET_ERROR_FT232H_SBR_READ;

                // 2020.05.13
                // 자료갯수 확인
                if (dwBytesRead > MAX_FT232H_READ_BUFFER)
                {
                    // 설정한 버퍼 크기 보다 큰 경우
                    dwBytesRead = MAX_FT232H_READ_BUFFER;
                }

                // USB 장치로 부터 읽은 자료 수 만큼 처리한다
                for (n = 0; n<(int)dwBytesRead; n++)
                {
                    //--------------------------------------------
                    // TX, TY, TW, TS 다음에 오는 Data는 Ascii 가아니므로
                    //( ComStr[0] == 'T' && CC == 2 && XC == 0 )
                    //  XStr[0] = bt;
                    //  XC = 1;
                    //--------------------------------------------
                    // 2020.05.13
                    //if (n > MAX_FT232H_READ_BUFFER - 1) break;

                    // 2020.05.13
                    bt = (byte)m_szFT232H_SbrRxBuffer[n];

                    // 2020.05.13
                    // 명령어 자료 : 'A' 부터 'Z' 문자를 조합하여 사용한다
                    if ((byte)'A' <= bt && bt <= (byte)'Z')
                    {
                        // Scan Bridge Command(명령어)는 2개의 문자로 구성 되어 있다
                        if (m_nFT232H_CmdCount < 2)
                        {
                            // 명령어를 보관한다
                            m_szFT232H_CmdBuffer[m_nFT232H_CmdCount] = bt;
                            m_nFT232H_CmdCount++;
                        }
                        else
                        {
                            // TS 명령어 확인
                            if (0 == strcmp("TS", m_szFT232H_CmdBuffer) && m_szFT232H_XStrBuffer[0] == (byte)'0')
                            {
                                m_szFT232H_YStrBuffer[m_nFT232H_YStrCount] = bt;
                                m_nFT232H_YStrCount++;
                                if (m_nFT232H_YStrCount > MAX_FT232H_XYDSStr_BUFFER-1)
                                {
                                    // Y Buffer 초과 : 데이터 오류
                                    m_nFT232H_YStrCount = 0;
                                    memset(m_szFT232H_YStrBuffer, 0x00, MAX_FT232H_XYDSStr_BUFFER);
                                }
                            }
                        }
                    }
                    else
                    // 명령어 다음 처리할 버퍼에 따라 자료를 보관한다(X, Y, D, S Str Buffer)
                    if ((byte)'0' <= bt && bt <= (byte)'9' || bt == (byte)'.' || bt == (byte)'-')
                    {
                        switch (m_nFT232H_XYDState)
                        {
                            case (byte)'x':
                            {
                                m_szFT232H_XStrBuffer[m_nFT232H_XStrCount] = bt; // X Str 자료 보관
                                m_nFT232H_XStrCount++;
                                if (m_nFT232H_XStrCount > MAX_FT232H_XYDSStr_BUFFER-1)
                                {
                                    // X Buffer 초과 : 데이터 오류
                                    m_nFT232H_XStrCount = 0;
                                    memset(m_szFT232H_XStrBuffer, 0x00, MAX_FT232H_XYDSStr_BUFFER);
                                }
                                break;
                            }
                            case (byte)'y':
                            {
                                m_szFT232H_YStrBuffer[m_nFT232H_YStrCount] = bt; // Y Str 자료 보관
                                m_nFT232H_YStrCount++;
                                if (m_nFT232H_YStrCount >= MAX_FT232H_XYDSStr_BUFFER-1)
                                {
                                    // Y Buffer 초과 : 데이터 오류
                                    m_nFT232H_YStrCount = 0;
                                    memset(m_szFT232H_YStrBuffer, 0x00, MAX_FT232H_XYDSStr_BUFFER);
                                }
                                break;
                            }
                            case (byte)'d':
                            {
                                m_szFT232H_DStrBuffer[m_nFT232H_DStrCount] = bt; // D Str 자료 보관
                                m_nFT232H_DStrCount++;
                                if (m_nFT232H_DStrCount > MAX_FT232H_XYDSStr_BUFFER-1)
                                {
                                    // D Buffer 초과 : 데이터 오류
                                    m_nFT232H_DStrCount = 0;
                                    memset(m_szFT232H_DStrBuffer, 0x00, MAX_FT232H_XYDSStr_BUFFER);
                                }
                                break;
                            }
                            case (byte)'s':
                            {
                                m_szFT232H_SStrBuffer[m_nFT232H_SStrCount] = bt; // S Str 자료 보관
                                m_nFT232H_SStrCount++;
                                if (m_nFT232H_SStrCount >= MAX_FT232H_XYDSStr_BUFFER)
                                {
                                    // S Buffer 초과 : 데이터 오류
                                    m_nFT232H_SStrCount = 0;
                                    memset(m_szFT232H_SStrBuffer, 0x00, MAX_FT232H_XYDSStr_BUFFER);
                                }
                                break;
                            }
                        }
                    }
                    else
                    // 명령어와 X,Y,D,S Str 간의 ',' 데이터를 확인하여 카운트 한다
                    if (bt == (byte)',')
                    {
                        // 명령어및데이터 예: "AAxxxxx,yyyyy,dddddd;"
                        // 명령어와 xxxx 데이터 사이에는 ',' 가 없다
                        // 기본(처음)은 X Str Buffer 를 사용할 순서 설정
                        m_nFT232H_CommaCount++;
                        switch (m_nFT232H_CommaCount)
                        {
                            case 1:
                            {
                                m_nFT232H_XYDState = (byte)'y'; // Y Str Buffer 를 사용할 순서 설정
                                break;
                            }
                            case 2:
                            {
                                m_nFT232H_XYDState = (byte)'d'; // D Str Buffer 를 사용할 순서 설정
                                break;
                            }
                        }
                    }
                    else
                    // 수신된 문자 중 '?' 확인하고 처리한다
                    if (bt == (byte)'?')
                    {
                        m_nFT232H_XYDState = (byte)'s';
                    }
                    else
                    // 수신된 문자 중 ';' 확인하고 처리한다
                    // 수신된 명령어 및 데이터 조합이 완료 되었음
                    if (bt == (byte)';')
                    {
                        m_szFT232H_CmdBuffer[m_nFT232H_CmdCount]   = (byte)'\0'; // Command Buffer 완료
                        m_szFT232H_XStrBuffer[m_nFT232H_XStrCount] = (byte)'\0'; // X Str   Buffer 완료
                        m_szFT232H_YStrBuffer[m_nFT232H_YStrCount] = (byte)'\0'; // Y Str   Buffer 완료
                        m_szFT232H_DStrBuffer[m_nFT232H_DStrCount] = (byte)'\0'; // D Str   Buffer 완료
                        m_szFT232H_SStrBuffer[m_nFT232H_SStrCount] = (byte)'\0'; // S Str   Buffer 완료

                        // 명령어를 표시할 것인지 확인한다...
                        if (m_nFT232H_EchoCommandEnable > 0)
                        {
                            m_nFT232H_EchoCommandEnable = 0; // 한번만 사용하도록,
                            m_nFT232H_EchoCommandFlag   = 1;
                            // Echo Command Buffer 초기화 및 복사
                            ZeroMem(m_szFT232H_EchoCmdStr);
                            strcpy_s(m_szFT232H_EchoCmdStr, m_szFT232H_CmdBuffer);
                        }
                        else
                        {
                            // 수신된 Sbr 명령어 자료 이상 유무 확인(CheckSum)
                            if (AfxSbrCheckSumCommand() > 0)
                            {
                                int nCmd  = -1;
                                int nIdx  = 0;
                                // 원본 조건은 'n < MAX_SBR_COMMAND_NUMBER'(n = 읽은 바이트 위치): 묶음의 44번째 바이트 뒤에 끝나는 메시지를 무시하던 버그. 의도대로 nIdx로 고침.
                                for (nIdx = 0; nIdx < MAX_SBR_COMMAND_NUMBER; nIdx++)
                                {
                                    // Sbr 명령어 문자 확인 및 명령어 인덱스 구하기
                                    if (strncmp(m_szSbrCommandList[nIdx], m_szFT232H_CmdBuffer, 2) == 0)
                                    {
                                        nCmd = nIdx;
                                        break;
                                    }
                                }

                                if (nCmd > -1)
                                {
                                    // Data Buffer Clear
                                    ZeroMem(m_pIntegrated.szComStr);
                                    ZeroMem(m_pIntegrated.szXStr);
                                    ZeroMem(m_pIntegrated.szYStr);
                                    ZeroMem(m_pIntegrated.szDStr);
                                    ZeroMem(m_pIntegrated.szSStr);

                                    // Command
                                    strcpy_s(m_pIntegrated.szComStr, m_szFT232H_CmdBuffer);
                                    // X Str Data
                                    strcpy_s(m_pIntegrated.szXStr  , m_szFT232H_XStrBuffer);
                                    // Y Str Data
                                    strcpy_s(m_pIntegrated.szYStr  , m_szFT232H_YStrBuffer);
                                    // D Str Data
                                    strcpy_s(m_pIntegrated.szDStr  , m_szFT232H_DStrBuffer);
                                    // S Str Data
                                    strcpy_s(m_pIntegrated.szSStr  , m_szFT232H_SStrBuffer);
                                }

                                //-----------------------------------------
                                // Scan Bridge (Sbr) 수신 명령어 실행
                                //-----------------------------------------
                                switch (nCmd)
                                {
                                    //*
                                    //-------------------------------------
                                    // All Control Board
                                    //-------------------------------------
                                    case SBR_CMD_PD: // Power Down 상태정보 수신
                                    {
                                        AfxScanBridgePDCommand();
                                        break;
                                    }
                                    //-------------------------------------
                                    // Vac Control Board
                                    //-------------------------------------
                                    case SBR_CMD_AR: // Air On/Off 상태정보 수신
                                    {
                                        AfxScanBridgeARCommand();
                                        break;
                                    }
                                    // 2020.01.17 오류가 확인되어 디버깅함
                                    case SBR_CMD_EV: // Evacation 상태정보 수신
                                    {
                                        AfxScanBridgeEVCommand();
                                        break;
                                    }
                                    case SBR_CMD_VS: // Vacuum State 상태정보 수신
                                    {
                                        AfxScanBridgeVSCommand();
                                        break;
                                    }
                                    case SBR_CMD_VL: // Valve On-Off 상태정보 수신
                                    {
                                        AfxScanBridgeVLCommand();
                                        break;
                                    }
                                    //-------------------------------------
                                    // HV Control Board
                                    //-------------------------------------
                                    case SBR_CMD_EM: // Emission Current
                                    {
                                        AfxScanBridgeEMCommand();
                                        break;
                                    }
                                    //-------------------------------------
                                    // CNC Board
                                    //-------------------------------------
                                    case SBR_CMD_RN: // Read  mm per step
                                    {
                                        AfxScanBridgeRNCommand();
                                        break;
                                    }
                                    case SBR_CMD_RA: // Read Accelation Table
                                    {
                                        AfxScanBridgeRACommand();
                                        break;
                                    }
                                    case SBR_CMD_RP: // Read Position
                                    {
                                        AfxScanBridgeRPCommand();
                                        break;
                                    }
                                    case SBR_CMD_RM: // Read Maximum Width
                                    {
                                        AfxScanBridgeRMCommand();
                                        break;
                                    }
                                    case SBR_CMD_MC: // Read BeamCenter
                                    {
                                        AfxScanBridgeMCCommand();
                                        break;
                                    }
                                    case SBR_CMD_TS: // TrackBall State
                                    {
                                        AfxScanBridgeTSCommand();
                                        break;
                                    }
                                    case SBR_CMD_LT: // Limit SW Test
                                    {
                                        AfxScanBridgeLTCommand();
                                        break;
                                    }
                                    case SBR_CMD_RB: // Read Backlash 값
                                    {
                                        AfxScanBridgeRBCommand();
                                        break;
                                    }
                                    case SBR_CMD_ZT: // Read ZTopLimitSwPulse 값
                                    {
                                        AfxScanBridgeZTCommand();
                                        break;
                                    }
                                    case SBR_CMD_SB: // Read BacklashMovingFlag 값
                                    {
                                        AfxScanBridgeSBCommand();
                                        break;
                                    }
                                    case SBR_CMD_TC: // Read TrackBall Count
                                    {
                                        AfxScanBridgeTCCommand();
                                        break;
                                    }
                                    case SBR_CMD_LS: //  Limit SW Test
                                    {
                                        AfxScanBridgeLSCommand();
                                        break;
                                    }
                                    //-------------------------------------
                                    // TrackBall Board
                                    //-------------------------------------
                                    case SBR_CMD_TB: //  TrackBall Command
                                    {
                                        AfxScanBridgeTBCommand();
                                        break;
                                    }
                                    //-------------------------------------
                                    // BlueTooth Board
                                    //-------------------------------------
                                    case SBR_CMD_BT: //  BlueTooth Command
                                    {
                                        AfxScanBridgeBTCommand();
                                        break;
                                    }
                                    //-----------------------------------------
                                    //-----------------------------------------
                                    //-----------------------------------------
                                    case SBR_CMD_EC: //  Echo Command : Stage Command Echo
                                    {
                                        AfxScanBridgeECCommand();
                                        break;
                                    }
                                    case SBR_CMD_VR: //  H/W Version String
                                    {
                                        AfxScanBridgeVRCommand();
                                        break;
                                    }
                                    case SBR_CMD_UL: //  Sample Load / Unload State
                                    {
                                        AfxScanBridgeULCommand();
                                        break;
                                    }
                                    case SBR_CMD_BF: //  Rotary Area Pulse
                                    {
                                        // 2021.03.31
                                        AfxScanBridgeBFCommand();
                                        break;
                                    }
                                    //-----------------------------------------
                                    //-----------------------------------------
                                    //-----------------------------------------
                                    //*/
                                } // Sbr Command 처리(실행)
                            }
                        }

                        //----------------------------
                        //  다음 Command를 위한 변수 초기화
                        //----------------------------
                        m_nFT232H_CmdCount   = 0;            // Scan Bridge Command Count
                        m_nFT232H_XStrCount  = 0;            // Scan Bridge XStr Count
                        m_nFT232H_YStrCount  = 0;            // Scan Bridge YStr Count
                        m_nFT232H_DStrCount  = 0;            // Scan Bridge DStr Count
                        m_nFT232H_SStrCount  = 0;            // Scan Bridge SStr Count
                        m_nFT232H_CommaCount = 0;            // Scan Bridge Comma Count
                        m_nFT232H_XYDState   = (byte)'x';          // Scan Bridge XYD State Flag
                        ZeroMem(m_szFT232H_CmdBuffer); // X Str 자료 보관
                        memset(m_szFT232H_XStrBuffer, 0x00, MAX_FT232H_XYDSStr_BUFFER); // X Str 자료 보관
                        memset(m_szFT232H_YStrBuffer, 0x00, MAX_FT232H_XYDSStr_BUFFER); // Y Str 자료 보관
                        memset(m_szFT232H_DStrBuffer, 0x00, MAX_FT232H_XYDSStr_BUFFER); // D Str 자료 보관
                        memset(m_szFT232H_SStrBuffer, 0x00, MAX_FT232H_XYDSStr_BUFFER); // S Str 자료 보관
                    }
                } // 받은자료 수 만큼 처리 for 문장
            } // 받은자료 상태확인 if 문장

            return RET_SUCCESS_OK;

        }

        // ---- USB_FT232H_Device.cpp:4876 ----
        //
        //
        //
        //
        //
        //
        //


        //
        // Scan Bridge : CNC 명령어 전송 처리 쓰레드 생성
        //
        public int AfxCreateCNCSendThread()
        {
            // CNC Send 쓰레드 사용 중이면...
            if (m_pCNCSendThread != null)
            {
                // CNC Send 쓰레드 파라메터 사용 중인지 확인
                if (m_pCNCSendThreadParams != null)
                {
                    // CNC Send 쓰레드 종료 확인
                    if (m_pCNCSendThreadParams.bThreadDone == TRUE)
                    {
                        // 2021.05.31
                        // 쓰레드 종료 됨, 삭제
                        // Scan Bridge 쓰레드에서 사용한 파라메터 삭제

                        m_pCNCSendThreadParams = null;
                    }
                    else
                    {
                        // 쓰레드 동작 중, 리턴
                        return RET_SUCCESS_OK;
                    }
                }
                //return RET_SUCCESS_OK;
            }

            // CNC Send 쓰레드 파라메터 사용 중인지 확인
            if (m_pCNCSendThreadParams != null)
            {
                // Scan Bridge 쓰레드에서 사용한 파라메터 삭제

                m_pCNCSendThreadParams = null;
            }

            // Scan Bridge 쓰레드에서 사용할 파라메터를 생성한다
            m_pCNCSendThreadParams = new SCANBRIDGETHREADPARAMS();
            if (m_pCNCSendThreadParams == null) return RET_ERROR_SBR_THREAD_PARAMS;
            m_pCNCSendThreadParams = new SCANBRIDGETHREADPARAMS();
            // Scan Bridge 쓰레드에서 사용할 파라메터 기본값 설정
            m_pCNCSendThreadParams.bThreadDone  = FALSE;        // 쓰레드 종료 설정
            m_pCNCSendThreadParams.bThreadStop  = FALSE;        // 쓰레드 강제 종료
            m_pCNCSendThreadParams.nFT232HIndex = IDX_FT232H_SCAN_BRIDGE_DEVICE; // Scan Bridge 장치 인덱스
            m_pCNCSendThreadParams.hFT232Handle = IntPtr.Zero;         // Scan Bridge 장치 핸들
            m_pCNCSendThreadParams.hEventHandle = null;         // Scan Bridge 이벤트 핸들
            //
            /*
            memset(m_pCNCSendThreadParams->szSbrRxBuffer, 0x00, sizeof(m_pCNCSendThreadParams->szSbrRxBuffer)); // Scan Bridge Read Buffer
            memset(m_pCNCSendThreadParams->szCmdBuffer  , 0x00, sizeof(m_pCNCSendThreadParams->szCmdBuffer  )); // Scan Bridge Command Buffer
            memset(m_pCNCSendThreadParams->szXStrBuffer , 0x00, sizeof(m_pCNCSendThreadParams->szXStrBuffer )); // Scan Bridge XStr Buffer
            memset(m_pCNCSendThreadParams->szYStrBuffer , 0x00, sizeof(m_pCNCSendThreadParams->szYStrBuffer )); // Scan Bridge YStr Buffer
            memset(m_pCNCSendThreadParams->szDStrBuffer , 0x00, sizeof(m_pCNCSendThreadParams->szDStrBuffer )); // Scan Bridge DStr Buffer
            memset(m_pCNCSendThreadParams->szSStrBuffer , 0x00, sizeof(m_pCNCSendThreadParams->szSStrBuffer )); // Scan Bridge SStr Buffer
            m_pCNCSendThreadParams->nCmdCount   = 0;            // Scan Bridge Command Count
            m_pCNCSendThreadParams->nXStrCount  = 0;            // Scan Bridge XStr Count
            m_pCNCSendThreadParams->nYStrCount  = 0;            // Scan Bridge YStr Count
            m_pCNCSendThreadParams->nDStrCount  = 0;            // Scan Bridge DStr Count
            m_pCNCSendThreadParams->nSStrCount  = 0;            // Scan Bridge SStr Count
            m_pCNCSendThreadParams->nCommaCount = 0;            // Scan Bridge Comma Count
            m_pCNCSendThreadParams->nXYDState   = 'x';          // Scan Bridge XYD State Flag
            m_pCNCSendThreadParams->nCommandViewFlag   = 0;     // Scan Bridge Command View Falg
            memset(m_pCNCSendThreadParams->szEchoCmdStr, 0x00, sizeof(m_pCNCSendThreadParams->szEchoCmdStr)); // Scan Bridge Echo Command Str
            m_pCNCSendThreadParams->nEchoCommandEnable = 0;     // Scan Bridge Echo Command Enable
            m_pCNCSendThreadParams->nEchoCommandFlag   = 0;     // Scan Bridge Echo Command Flag
            */
            //
            /*
            // 2020.05.13
            memset(m_szFT232H_SbrRxBuffer, 0x00, MAX_FT232H_READ_BUFFER   ); // Scan Bridge Read Buffer
            memset(m_szFT232H_CmdBuffer  , 0x00, MAX_FT232H_COMMAND_BUFFER); // Scan Bridge Command Buffer
            memset(m_szFT232H_XStrBuffer , 0x00, MAX_FT232H_XYDSStr_BUFFER); // Scan Bridge XStr Buffer
            memset(m_szFT232H_YStrBuffer , 0x00, MAX_FT232H_XYDSStr_BUFFER); // Scan Bridge YStr Buffer
            memset(m_szFT232H_DStrBuffer , 0x00, MAX_FT232H_XYDSStr_BUFFER); // Scan Bridge DStr Buffer
            memset(m_szFT232H_SStrBuffer , 0x00, MAX_FT232H_XYDSStr_BUFFER); // Scan Bridge SStr Buffer
            memset(m_szFT232H_EchoCmdStr , 0x00, MAX_FT232H_COMMAND_BUFFER); // Scan Bridge Echo Command Str  
            //
            m_nFT232H_CmdCount   = 0;                                        // Scan Bridge Command Count
            m_nFT232H_XStrCount  = 0;                                        // Scan Bridge XStr Count
            m_nFT232H_YStrCount  = 0;                                        // Scan Bridge YStr Count
            m_nFT232H_DStrCount  = 0;                                        // Scan Bridge DStr Count
            m_nFT232H_SStrCount  = 0;                                        // Scan Bridge SStr Count
            m_nFT232H_CommaCount = 0;                                        // Scan Bridge Comma Count
            m_nFT232H_XYDState   = 'x';                                      // Scan Bridge XYD State Flag
            m_nFT232H_CommandViewFlag   = 0;                                 // Scan Bridge Command View Falg
            m_nFT232H_EchoCommandEnable = 0;                                 // Scan Bridge Echo Command Enable
            m_nFT232H_EchoCommandFlag   = 0;                                 // Scan Bridge Echo Command Flag
            */
            //

            // 핸들 설정
            IntPtr ft232hHandle = AfxFT232HGetHandle(m_pCNCSendThreadParams.nFT232HIndex);
            if (ft232hHandle == IntPtr.Zero) return RET_ERROR_FT232H_SBR_HANDLE;
            m_pCNCSendThreadParams.hFT232Handle = ft232hHandle; // CNC 장치 핸들

            // Scan Bridge 쓰레드 생성 및 시작...
            m_pCNCSendThread = AfxBeginThread(AfxThreadCNCSendProcessing, m_pCNCSendThreadParams); // , THREAD_PRIORITY_NORMAL);
            if (m_pCNCSendThread == null) return RET_ERROR_SBR_THREAD_HANDLE;

            return RET_SUCCESS_OK;
        }

        // ---- USB_FT232H_Device.cpp:4980 ----
        //
        // Scan Bridge : CNC 명령어 처리(작업) 쓰레드
        //
        public uint AfxThreadCNCSendProcessing(object pParams)
        {
            SCANBRIDGETHREADPARAMS pThreadParams = (SCANBRIDGETHREADPARAMS)pParams;

            int  nCncCount    = 0;
            int bbThreadDone = FALSE;
            if (pThreadParams != null)
            {
                nCncCount = m_nFT232H_CmdCount;
                // 쓰레드 작업 상태 설정
                pThreadParams.bThreadDone = FALSE;
                pThreadParams.bThreadStop = FALSE;
                bbThreadDone = pThreadParams.bThreadDone;
            }


            int  bLoop  = TRUE;
            uint dwRet  = 0;
            int   nSeqNo = 0;

            ///
            // Scan Bridge 쓰레드 프로세싱
            ///
            while (bLoop == TRUE)
            {
                //
                // CNC 명령어 순차적 전송 처리
                //
                switch (nCncCount)// pThreadParams->nCmdCount)
                {
                    case 0:  // "LS1"
                    {
                        // LIMIT SW STATE 읽기
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "LS1;");
                        break;
                    }
                    case 1:  // "RN0"
                    {
                        //Sbr_StrAvrSend("RN0;"); break;  // X축 1Step 당 ??mm 값 읽기(Read mm per step(mm/step))
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RN0;");
                        break;
                    }
                    case 2:  // "RN1"
                    {
                        //Sbr_StrAvrSend("RN1;"); break;  // Y축 1Step 당 ??mm 값 읽기(Read mm per step(mm/step))
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RN1;");
                        break;
                    }
                    case 3:  // "RN2"
                    {
                        //Sbr_StrAvrSend("RN2;"); break;  // Z축 1Step 당 ??mm 값 읽기(Read mm per step(mm/step))
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RN2;");
                        break;
                    }
                    case 4:  // "RN3"
                    {
                        //Sbr_StrAvrSend("RN3;"); break;  // R축 1Step 당 ??mm 값 읽기(Read mm per step(mm/step))
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RN3;");
                        break;
                    }
                    case 5:  // "RN4"
                    {
                        //Sbr_StrAvrSend("RN4;"); break;  // T축 1Step 당 ??mm 값 읽기(Read mm per step(mm/step))
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RN4;");
                        break;
                    }
                    case 6:  // "RM0"
                    {
                        //Sbr_StrAvrSend("RM0;"); break;  // X축 이동가능거리 최대값 (Read Maximum Width)
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RM0;");
                        break;
                    }
                    case 7:  // "RM1"
                    {
                        //Sbr_StrAvrSend("RM1;"); break;  // Y축 이동가능거리 최대값 (Read Maximum Width)
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RM1;");
                        break;
                    }
                    case 8:  // "RM2"
                    {
                        //Sbr_StrAvrSend("RM2;"); break;  // Z축 이동가능거리 최대값 (Read Maximum Width)
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RM2;");
                        break;
                    }
                    case 9:  // "RM3"
                    {
                        //Sbr_StrAvrSend("RM3;"); break;  // R축 이동가능거리 최대값 (Read Maximum Width) -> 실제 사용하지 않는 것으로 추정됨
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RM3;");
                        break;
                    }
                    case 10:  // "RM4"
                    {
                        //Sbr_StrAvrSend("RM4;"); break;  // T축 최대값 읽기 (Read Tilt Max)
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RM4;");
                        break;
                    }
                    case 11:  // "RM5"
                    {
                        //Sbr_StrAvrSend("RM5;"); break;  // T축 0각도의 위치값 읽기 (Read  TiltZeroAnglePulsePos)
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RM5;");
                        break;
                    }
                    case 12:  // "RA0"
                    {
                        // X Axis Accelation 데이터 가져오기
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RA0;");
                        break;
                    }
                    case 13:  // "RA1"
                    {
                        // Y Axis Accelation 데이터 가져오기
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RA1;");
                        break;
                    }
                    case 14:  // "RA2"
                    {
                        // Z Axis Accelation 데이터 가져오기
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RA2;");
                        break;
                    }
                    case 15:  // "RA3"
                    {
                        // R Axis Accelation 데이터 가져오기
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RA3;");
                        break;
                    }
                    case 16:  // "RA4"
                    {
                        // T Axis Accelation 데이터 가져오기
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RA4;");
                        break;
                    }
                    case 17:  // "ZT1"
                    {
                        //Sbr_StrAvrSend("ZT1;"); break;  // Z축 TOP Limit 값 읽기
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "ZT1;");
                        break;
                    }
                    case 18:  // "MC2"
                    {
                        //Sbr_StrAvrSend("MC2;"); break;  // 빔 센터를 위한 X,Y축 위치값 읽기 (Read BeamCenter)
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "MC2;");
                        break;
                    }
                    case 19:  // "RP0"
                    {
                        //Sbr_StrAvrSend("RP0;"); break;  // X축 위치값 읽기 (Read Position)
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RP0;");
                        break;
                    }
                    case 20:  // "RP1"
                    {
                        //Sbr_StrAvrSend("RP1;"); break;  // Y축 위치값 읽기 (Read Position)
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RP1;");
                        break;
                    }
                    case 21:  // "RP2"
                    {
                        //Sbr_StrAvrSend("RP2;"); break;  // Z축 위치값 읽기 (Read Position)
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RP2;");
                        break;
                    }
                    case 22:  // "RP3"
                    {
                        //Sbr_StrAvrSend("RP3;"); break;  // R축 위치값 읽기 (Read Position)
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RP3;");
                        break;
                    }
                    case 23:  // "RP4"
                    {
                        //Sbr_StrAvrSend("RP4;"); break;  // T축 위치값 읽기 (Read Position)
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "RP4;");
                        break;
                    }
                    case 24:  // "VL8"
                    {
                        //Sbr_StrAvrSend("TS0;");         // TrackBall Moving State
                        //Sbr_StrAvrSend("VL8;");         // Read 현재상태 ( Home or Fixed Position )
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "TS0;");
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "VL8;");
                        break;
                    }
                    case 25:  // Eg : Extra Gate
                    {
                        //Sbr_StrAvrSend("VL1,0;"); break;  // Extra gate valve 모터의 1Step 당 이동거리[mm] (Read mm per step(mm/step)) 데이터 가져오기
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "VL1,0;");
                        break;
                    }
                    case 26:  // St
                    {
                        //Sbr_StrAvrSend("VL1,1;"); break;  // Steel tape 모터의 1Step 당 이동거리[mm] (Read mm per step(mm/step)) 데이터 가져오기
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "VL1,1;");
                        break;
                    }
                    case 27:  // Dm
                    {
                        //Sbr_StrAvrSend("VL1,2;"); break;  // Damper 모터의 1Step 당 이동거리[mm] (Read mm per step(mm/step)) 데이터 가져오기
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "VL1,2;");
                        break;
                    }
                    case 28:  // Mg
                    {
                        //Sbr_StrAvrSend("VL1,3;"); break;  // Main gate value 모터의 1Step 당 이동거리[mm] (Read mm per step(mm/step)) 데이터 가져오기
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "VL1,3;");
                        break;
                    }
                    case 29:  // Mg
                    {
                        //Sbr_StrAvrSend("VL1,4;"); break;  // AVR 에서는 사용하지 않는 것으로 판단됨. (Read mm per step(mm/step))
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "VL1,4;");
                        break;
                    }
                    case 30:  // Eg
                    {
                        //Sbr_StrAvrSend("VL5,0;"); break;  // Eatra gate 모터의 지정된 위치 이동을 위한 Pulse 갯수 (Read FixedPosition) 데이터 가져오기
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "VL5,0;");
                        break;
                    }
                    case 31:  // St
                    {
                        //Sbr_StrAvrSend("VL5,1;"); break;  // Steel tape 모터의 지정된 위치 이동을 위한 Pulse 갯수 (Read FixedPosition) 데이터 가져오기
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "VL5,1;");
                        break;
                    }
                    case 32:  // Dm
                    {
                        //Sbr_StrAvrSend("VL5,2;"); break;  // Damper 모터의 지정된 위치 이동을 위한 Pulse 갯수 (Read FixedPosition) 데이터 가져오기
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "VL5,2;");
                        break;
                    }
                    case 33:  // Mg
                    {
                        //Sbr_StrAvrSend("VL5,3;"); break;  // Main gate 모터의 지정된 위치 이동을 위한 Pulse 갯수 (Read FixedPosition) 데이터 가져오기
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "VL5,3;");
                        break;
                    }
                    case 34:  // Mg
                    {
                        //Sbr_StrAvrSend("VL5,4;");         // Extra gate valve 모터의 이동을 위한 Pulse 값 설정, Pulse 값을 설정하지 않으면 현재위치값 가져오기 (Send MovingPulse ( Current Motor Position ))
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "VL5,4;");
                        break;
                    }
                    case 35:  // "BF1"
                    {
                        // 2021.03.31
                        //Sbr_StrAvrSend("BF1;");           // Rotary Area Pulse 값 가져오기
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "BF1;");
                        // CNC COMMAND SEND 종료
                        bLoop = FALSE;
                        break;
                    }
                    case 36:  // "VR0"
                    {
                        // H/W #0 Version
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "VR0;");
                        break;
                    }
                    case 37:  // "VR1"
                    {
                        // H/W #1 Version
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "VR1;");
                        break;
                    }
                    case 38:  // "VR2"
                    {
                        // H/W #2 Version
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "VR2;");
                        break;
                    }
                    case 39:  // "VR3"
                    {
                        // H/W #3 Version
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "VR3;");
                        break;
                    }
                    case 40:  // "VR4"
                    {
                        // H/W #4 Version
                        AfxFT232HPortSendAvrCommand(IDX_FT232H_SCAN_BRIDGE_DEVICE, "VR4;");
                        bLoop = FALSE;
                        break;
                    }
                    case 41:  // R축 => Go Home
                    {
                        bLoop = FALSE;
                        break;
                    }
                }

                if (bLoop == TRUE)
                {
                    nCncCount++;
                    if (pThreadParams != null)
                    {
                        // 강제종료 CHECK?
                        if (pThreadParams.bThreadStop == TRUE)
                        {
                            // 쓰레드 종료
                            bLoop = FALSE;
                        }
                        else
                        {
                            // 100[ms] 대기
                            Sleep(PUB_CNC_SEND_MIN_INTERVAL);
                        }
                    }
                    else
                    {
                        // 200[ms] 대기
                        Sleep(200);
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

            return RET_SUCCESS_OK;
        }

    }
}
