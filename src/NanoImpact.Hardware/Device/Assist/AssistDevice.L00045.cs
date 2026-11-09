// 원본 ModularAssistDevice.cpp:45-1058 을 NanoImpact.CodeGen translate로 옮긴 뒤 손으로 고친 코드.
#pragma warning disable IDE1006, IDE0059, CS0162, CS0168, CS0219
using System;
using NanoImpact.Core.Native;
using static NanoImpact.Core.Def;
using static NanoImpact.Core.Native.CRuntime;

namespace NanoImpact.Hardware.Device
{
    public sealed partial class AssistDevice
    {
        // ---- ModularAssistDevice.cpp:45 ----
        public void SetOwner(int hOwner)
        {
            // SET OWNER WINDOW HANDLE
            m_hOwnerWnd = hOwner;
        }

        // ---- ModularAssistDevice.cpp:52 ----
        public void SetOwnerWindow(IAssistOwner pOwner)
        {
            // SET OWNER WINDOW HANDLE
            m_pOwnerWindow = pOwner;
        }

        // ---- ModularAssistDevice.cpp:59 ----
        public void SetAssistPublicTool(object pAssistPublicTool)
        {
            // Assist Public Tool 설정
            m_pAssistPublicTool = pAssistPublicTool;
        }

        // ---- ModularAssistDevice.cpp:66 ----
        public void SetVacuumBeamAlignFocusPublicData(VACUUM_BEAM_ALIGN_FOCUS_DATA pVacuumBeamAlignFocusPublicData)
        {
            // VACUUM, BEAM, ALIGN, FOCUS Public Data 설정
            m_pVacuumBeamAlignFocusPublicData = pVacuumBeamAlignFocusPublicData;
        }

        // ---- ModularAssistDevice.cpp:78 ----
        //
        // SEM USB DEVICE CONTROL
        //

        // SEM USB DEVICE CONTROL : SEM STATE 설정
        public void SetSEMControlSemVacuumState(int nState)
        {
            // 2020.04.28
            // USB DEVICE : SEM VACUUM STATE 적용
            int nClientSemVacState = 0;
            if      (nState == CLIENT_POWER_OFF_STATE   ) nClientSemVacState = POWER_OFF_STATE;
            else if (nState == CLIENT_AIR_ON_STATE      ) nClientSemVacState = AIR_ON_STATE;
            else if (nState == CLIENT_VAC_EVAC_STATE    ) nClientSemVacState = VAC_EVAC_STATE;
            else if (nState == CLIENT_VAC_OK_STATE      ) nClientSemVacState = VAC_OK_STATE;
            else if (nState == CLIENT_HV_ON_STATE       ) nClientSemVacState = HV_ON_STATE;
            else if (nState == CLIENT_CNC5_GO_SAM_LOAD  ) nClientSemVacState = CNC5_GO_SAM_LOAD;
            else if (nState == CLIENT_CNC5_GO_SAM_UNLOAD) nClientSemVacState = CNC5_GO_SAM_UNLOAD;
            else if (nState == CLIENT_HV_VAC_OK_STATE   ) nClientSemVacState = VAC_OK_STATE;  // CLIENT_HV_VAC_OK_STATE;
            else if (nState == CLIENT_HV_AIR_ON_STATE   ) nClientSemVacState = AIR_ON_STATE;  // CLIENT_HV_AIR_ON_STATE;
            else if (nState == CLIENT_HV_EVAC_STATE     ) nClientSemVacState = VAC_EVAC_STATE;// CLIENT_HV_EVAC_STATE;
            else return;
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : SEM CONTROL -> SEM VACUUM STATE
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_CONTROL_VACUUM_STATE;
            RequestDeviceData.nRequestType    = 0; // int:0, double:1
            RequestDeviceData.nRequestOption  = 1; // Data Count
            RequestDeviceData.nRequestData[0] = nClientSemVacState;
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_CONTROL_VACUUM_STATE, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != null)
            {
                m_pOwnerWindow.OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, RequestDeviceData);
            }
        }

        // ---- ModularAssistDevice.cpp:112 ----
        // SEM USB DEVICE CONTROL : GUN VALVE OPEN/CLOSE
        public void SetSEMControlGunValveOpenClose(int nOpenClose)
        {
            // GUN VALVE OPEN/CLOSE
            m_pVacuumBeamAlignFocusPublicData.m_nGunValveState = nOpenClose;
            /*
            // USB DEVICE : GUN VALVE OPEN/CLOSE 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData;
            // 초기화
            memset(&RequestDeviceData, 0x00, sizeof(REQUEST_USB_DEVICE_DATA));
            // SET USB DEVICE DATA : SEM CONTROL -> GUN VALVE OPEN/CLOSE
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_CONTROL_GUN_VALVE;
            RequestDeviceData.nRequestType    = 0; // int:0, double:1
            RequestDeviceData.nRequestOption  = 1; // Data Count
            RequestDeviceData.nRequestData[0] = m_pVacuumBeamAlignFocusPublicData->m_nGunValveState;
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_CONTROL_GUN_VALVE, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != NULL)
            {
                ((CNanoImpact2020cfeDlg*)m_pOwnerWindow)->OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, (LPARAM)&RequestDeviceData);
            }
            */
            // 2020.04.28
            // SET USB DEVICE : GUN VALUE OPEN/CLOSE 실행
            // USB DEVICE SET 명령어설정
            if (m_pVacuumBeamAlignFocusPublicData.m_nGunValveState == GUNVALVE_STATE_OPEN)
            {
                m_dev.AfxScanBridgeGunValveOpen();  // Gun Valve -> Open
            }
            else
            {
                m_dev.AfxScanBridgeGunValveClose(); // Gun Valve -> Close
            }
        }

        // ---- ModularAssistDevice.cpp:148 ----
        // SEM USB DEVICE CONTROL : AIR ON/OFF -> CHAMBER OPEN/CLOSE
        public void SetSEMControlChamberOpenClose(int nOpenClose)
        {
            // Chamber Open/Close State
            m_pVacuumBeamAlignFocusPublicData.m_nChamberState = nOpenClose;
            /*
            // USB DEVICE : GUN VALVE OPEN/CLOSE 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData;
            // 초기화
            memset(&RequestDeviceData, 0x00, sizeof(REQUEST_USB_DEVICE_DATA));
            // SET USB DEVICE DATA : SEM CONTROL -> CHAMBER AIR ON/OFF
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_CONTROL_CHAMBER_STATE;
            RequestDeviceData.nRequestType    = 0; // int:0, double:1
            RequestDeviceData.nRequestOption  = 1; // Data Count
            RequestDeviceData.nRequestData[0] = m_pVacuumBeamAlignFocusPublicData->m_nChamberState;
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_CONTROL_CHAMBER_STATE, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != NULL)
            {
                ((CNanoImpact2020cfeDlg*)m_pOwnerWindow)->OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, (LPARAM)&RequestDeviceData);
            }
            */
            // 2020.04.28
            // SEM CHAMBER STATE 설정
            // USB DEVICE SET 명령어설정
            if (m_pVacuumBeamAlignFocusPublicData.m_nChamberState == CHMABER_STATE_OPEN)
            {
                m_dev.AfxScanBridgeSetSEMAirOnExecute();  // AIR ON  : CHAMEBER OPEN
            }
            else
            {
                m_dev.AfxScanBridgeSetSEMAirOffExecute(); // AIR OFF : CHAMBER CLOSE
            }
        }

        // ---- ModularAssistDevice.cpp:184 ----
        // SEM USB DEVICE CONTROL : FILAMENT IMAGE SCAN START/STOP
        public void SetSEMControlFilamentImageScanStartStop(int nStartStop)
        {
            /*
            // USB DEVICE : FILAMENT IMAGE SCAN START/STOP 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData;
            // 초기화
            memset(&RequestDeviceData, 0x00, sizeof(REQUEST_USB_DEVICE_DATA));
            // SET USB DEVICE DATA : SEM CONTROL -> FILAMENT IMAGE SCAN START/STOP
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_CONTROL_FILAMENT_IMAGE_SCAN;
            RequestDeviceData.nRequestType    = 0; // int:0, double:1
            RequestDeviceData.nRequestOption  = 1; // Data Count
            RequestDeviceData.nRequestData[0] = nStartStop;
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_CONTROL_FILAMENT_IMAGE_SCAN, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != NULL)
            {
                ((CNanoImpact2020cfeDlg*)m_pOwnerWindow)->OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, (LPARAM)&RequestDeviceData);
            }
            */
            // 2020.04.28
            // SET USB DEVICE : FILAMENT IMAGE SCAN START/STOP 실행
            // USB DEVICE SET 명령어설정
            if (nStartStop == FILAMENT_IMAGE_SCAN_START)
            {
                m_dev.AfxScanBridgeFilamentImageScanStart();  // F.I Start
            }
            else
            {
                m_dev.AfxScanBridgeFilamentImageScanStop();   // F.I Stop
            }
        }

        // ---- ModularAssistDevice.cpp:219 ----
        // SEM USB DEVICE CONTROL : SPELLMAN HV DISABLED -> PMT/SCINTILATOR/COLLECTOR
        public void SetSEMControlSpellmanHVDisabled(int nGunFilBias, int nPMT, int nSCIN, int nCOL)
        {
            /*
            // USB DEVICE : SPELLMAN HV ( GUN/FIL/BIAS, PMT, SCINTILATOR, COLLECTOR ) -> DISABLED 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData;
            // 초기화
            memset(&RequestDeviceData, 0x00, sizeof(REQUEST_USB_DEVICE_DATA));
            // SET USB DEVICE DATA : SEM CONTROL -> SPELLMAN HV GUN/FIL/BIAS,PMT,SCIN,COLLECTOR DISABLED
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_CONTROL_SPELLMAN_HV;
            RequestDeviceData.nRequestType    = 0; // int:0, double:1
            RequestDeviceData.nRequestOption  = 4; // Data Count
            RequestDeviceData.nRequestData[0] = nGunFilBias; // SPELLMAN_GUN_FIL_BIAS_DISABLED;
            RequestDeviceData.nRequestData[1] = nPMT;        // SPELLMAN_PMT_DISABLED;
            RequestDeviceData.nRequestData[2] = nSCIN;       // SPELLMAN_SCINTILATOR_DISABLED;
            RequestDeviceData.nRequestData[3] = nCOL;        // SPELLMAN_COLLECTOR_DISABLED;
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_CONTROL_SPELLMAN_HV, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != NULL)
            {
                ((CNanoImpact2020cfeDlg*)m_pOwnerWindow)->OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, (LPARAM)&RequestDeviceData);
            }
            */
            // 2020.04.28
            // USB DEVICE : SPELLMAN HV ( GUN/FIL/BIAS, PMT, SCINTILATOR, COLLECTOR ) -> DISABLED 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceSemControlSpellmanHVOnOff(nGunFilBias, nPMT, nSCIN, nCOL);
        }

        // ---- ModularAssistDevice.cpp:250 ----
        // SEM USB DEVICE CONTROL : SPELLMAN HV ENABLED -> PMT/SCINTILATOR/COLLECTOR
        public void SetSEMControlSpellmanHVEnabled(int nGunFilBias, int nPMT, int nSCIN, int nCOL)
        {
            /*
            // USB DEVICE : SPELLMAN HV ( GUN/FIL/BIAS, PMT, SCINTILATOR, COLLECTOR ) -> ENABLED 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData;
            // 초기화
            memset(&RequestDeviceData, 0x00, sizeof(REQUEST_USB_DEVICE_DATA));
            // SET USB DEVICE DATA : SEM CONTROL -> SPELLMAN HV GUN/FIL/BIAS,PMT,SCIN,COLLECTOR ENABLED
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_CONTROL_SPELLMAN_HV;
            RequestDeviceData.nRequestType    = 0; // int:0, double:1
            RequestDeviceData.nRequestOption  = 4; // Data Count
            RequestDeviceData.nRequestData[0] = nGunFilBias; // SPELLMAN_GUN_FIL_BIAS_ENABLED;
            RequestDeviceData.nRequestData[1] = nPMT;        // SPELLMAN_PMT_ENABLED;
            RequestDeviceData.nRequestData[2] = nSCIN;       // SPELLMAN_SCINTILATOR_ENABLED;
            RequestDeviceData.nRequestData[3] = nCOL;        // SPELLMAN_COLLECTOR_ENABLED;
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_CONTROL_SPELLMAN_HV, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != NULL)
            {
                ((CNanoImpact2020cfeDlg*)m_pOwnerWindow)->OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, (LPARAM)&RequestDeviceData);
            }
            */
            // 2020.04.28
            // USB DEVICE : SPELLMAN HV ( GUN/FIL/BIAS, PMT, SCINTILATOR, COLLECTOR ) -> ENABLED 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceSemControlSpellmanHVOnOff(nGunFilBias, nPMT, nSCIN, nCOL);
        }

        // ---- ModularAssistDevice.cpp:281 ----
        // SEM USB DEVICE CONTROL : SE IMAGE CAPTURE ON/OFF
        public void SetSEMImageCaptureOnOffValueUsbDeviceData(int nOnOff = 0, int nScanMode = 0)
        {
            
            // USB DEVICE : IMAGE CAPTURE ON/OFF 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceImageCaptureOnOffValue(nOnOff, nScanMode);
        }

        // ---- ModularAssistDevice.cpp:291 ----
        // SEM USB DEVICE CONTROL : SE SCAN GENERATION
        public void SetSEMScanGenerationValueUsbDeviceData(int nScanMode = 0)
        {
            
            // USB DEVICE : SCAN GENERATION 설정
            // USB DEVICE SET 명령어설정
            SetUsbDeviceSemSEScanGernerarion(nScanMode);
        }

        // ---- ModularAssistDevice.cpp:301 ----
        // SEM USB DEVICE CONTROL : SE IMAGE CAPTURE SLOW1,2 ON/OFF
        public void SetSEMSlow12ImageCaptureOnUsbDeviceData(int nScanMode = 0)
        {
            // USB SCAN MODE 설정
            m_dev.AfxIntegratedSetScanMode(nScanMode);
        }

        // ---- ModularAssistDevice.cpp:309 ----
        // SEM USB DEVICE CONTROL : NORMAL SCREEN MODE
        public void SetSEMNormalScreenModeValueUsbDeviceData(int nScreenMode = 0)
        {
            /*
            // USB DEVICE : NORMAL SCREEN MODE 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData;
            // 초기화
            memset(&RequestDeviceData, 0x00, sizeof(REQUEST_USB_DEVICE_DATA));
            // SET USB DEVICE DATA : SEM CONTROL -> NORMAL SCREEN MODE
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_NORMAL_SCREEN_MODE_VALUE;
            RequestDeviceData.nRequestType    = 0; // int:0, double:1
            RequestDeviceData.nRequestOption  = 1; // Data Count
            RequestDeviceData.nRequestData[0] = nScreenMode;
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_NORMAL_SCREEN_MODE_VALUE, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != NULL)
            {
                ((CNanoImpact2020cfeDlg*)m_pOwnerWindow)->OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, (LPARAM)&RequestDeviceData);
            }
            */

            // USB DEVICE : NORMAL SCREEN MODE 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceNormalScreenModeValue(nScreenMode);
        }

        // ---- ModularAssistDevice.cpp:337 ----
        // SEM USB DEVICE CONTROL : SEM START
        public void SetSEMControlStartExecute()
        {
            
            // USB DEVICE : SEM START 설정
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : SEM CONTROL -> SEM START
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_CONTROL_START_EXECUTE;
            RequestDeviceData.nRequestData[0] = 0; // DEVICE 서버 SEM START 상태설정
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_CONTROL_FLOW_EXECUTE, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != null)
            {
                m_pOwnerWindow.OnSetUsbDeviceSEMControlDataToFlowControl(NULL, RequestDeviceData);
            }
        }

        // ---- ModularAssistDevice.cpp:358 ----
        // SEM USB DEVICE CONTROL : SEM STANDBY
        public void SetSEMControlStandbyExecute()
        {
            
            // USB DEVICE : SEM STANDBY 설정
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : SEM CONTROL -> SEM START
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_CONTROL_STANDBY_EXECUTE;
            RequestDeviceData.nRequestData[0] = 0; // DEVICE 서버 SEM STANDBY 상태설정
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_CONTROL_FLOW_EXECUTE, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != null)
            {
                m_pOwnerWindow.OnSetUsbDeviceSEMControlDataToFlowControl(NULL, RequestDeviceData);
            }
        }

        // ---- ModularAssistDevice.cpp:379 ----
        // SEM USB DEVICE CONTROL : SEM GUN HV ON/OFF
        public void SetSEMControlGunHVOnOffFlag(int nOnOff = 0)
        {
            // USB DEVICE : SEM STANDBY 설정
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : SEM CONTROL -> SEM START
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_CONTROL_GUN_HV_ON_OFF_FLAG;
            RequestDeviceData.nRequestData[0] = nOnOff; // DEVICE 서버 SEM GUN HV ON/OFF 상태설정
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_CONTROL_FLOW_EXECUTE, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != null)
            {
                m_pOwnerWindow.OnSetUsbDeviceSEMControlDataToFlowControl(NULL, RequestDeviceData);
            }

            
            // USB DEVICE 설정작업 안함
        }

        // ---- ModularAssistDevice.cpp:402 ----
        // SEM USB DEVICE CONTROL : GUN HV INDEX 설정
        public void SetSEMGunHVIndexUsbDeviceData(int nGunHVIndex = 0, int nGunHVExecute = 0)
        {
            // USB DEVICE : GUN HV INDEX 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : SEM CONTROL -> GUN HV INDEX VALUE
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_GUN_HV_INDEX;
            RequestDeviceData.nRequestType    = 0; // int:0, double:1
            RequestDeviceData.nRequestOption  = 1; // Data Count
            RequestDeviceData.nRequestData[0] = nGunHVIndex;
            if (nGunHVExecute == 1)
            {
                RequestDeviceData.nRequestData[1] = 1; // GUN HV 실행
            }
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_GUN_HV_INDEX, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != null)
            {
                m_pOwnerWindow.OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, RequestDeviceData);
            }

            
            // USB DEVICE : GUN HV INDEX 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceGunHVIndex(nGunHVIndex, nGunHVExecute);
        }

        // ---- ModularAssistDevice.cpp:433 ----
        // SEM USB DEVICE CONTROL : GUN HV VALUE 설정
        public void SetSEMGunHVVoltageValueUsbDeviceData(double dGunHVMin, double dGunHVMax, double dGunHVStart, double dGunHVStop, double dGunHVCur, string strData, int nAVExecute)
        {
            /*
            // USB DEVICE : GUN HV VALUE 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData;
            // 초기화
            memset(&RequestDeviceData, 0x00, sizeof(REQUEST_USB_DEVICE_DATA));
            // SET USB DEVICE DATA : SEM CONTROL -> GUN HV VALUE
            RequestDeviceData.nRequestID     = REQIDX_SET_USB_DEVICE_SEM_GUN_HV_VOLT_VALUE;
            RequestDeviceData.nRequestType   = 0; // int:0, double:1
            RequestDeviceData.nRequestOption = 5; // Data Count
            if (nAVExecute == 1)
            {
                RequestDeviceData.dRequestData[0] = dGunHVMin;   // Min     Voltage
                RequestDeviceData.dRequestData[1] = dGunHVMax;   // Max     Voltage
                RequestDeviceData.dRequestData[2] = dGunHVStart; // Start   Voltage
                RequestDeviceData.dRequestData[3] = dGunHVStop;  // Stop    Voltage
                RequestDeviceData.dRequestData[4] = dGunHVCur;   // Current Voltage
                RequestDeviceData.nRequestData[0] = 1;           // USB DEVICE "AV" 실행
                strcpy_s(RequestDeviceData.szData, strData); // 원본 (CStringA) = ANSI(CP949) 변환 // Text Data
            }
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_GUN_HV_VOLT_VALUE, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != NULL)
            {
                ((CNanoImpact2020cfeDlg*)m_pOwnerWindow)->OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, (LPARAM)&RequestDeviceData);
            }
            */
            
            // USB DEVICE : GUN HV VALUE 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceGunHVVoltValue(dGunHVMin, dGunHVMax, dGunHVStart, dGunHVStop, dGunHVCur, strData, nAVExecute);
        }

        // ---- ModularAssistDevice.cpp:470 ----
        // SEM USB DEVICE CONTROL : GUN HV EC VALUE 설정
        public void SetSEMControlGunHVECValue(int nECValue = 0)
        {
            /*
            // USB DEVICE : GUN HV EC VALUE 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData;
            // 초기화
            memset(&RequestDeviceData, 0x00, sizeof(REQUEST_USB_DEVICE_DATA));
            // SET USB DEVICE DATA : SEM CONTROL -> GUN HV EC VALUE
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_GUN_HV_EC_VALUE;
            RequestDeviceData.nRequestType    = 0; // int:0, double:1
            RequestDeviceData.nRequestOption  = 1; // Data Count
            RequestDeviceData.nRequestData[0] = nECValue; // GUN HV EC VALUE
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_GUN_HV_EC_VALUE, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != NULL)
            {
                ((CNanoImpact2020cfeDlg*)m_pOwnerWindow)->OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, (LPARAM)&RequestDeviceData);
            }
            */
            // 2020.04.28
            // SET USB DEVICE : GUN HV EC VALUE 실행
            // USB DEVICE SET 명령어설정
            m_dev.AfxIntegratedSetGunHVECValue((ushort)nECValue);
        }

        // ---- ModularAssistDevice.cpp:498 ----
        // SEM USB DEVICE CONTROL : EMISSION AUTO TRANSFER REQUEST VALUE 설정
        public void SetSEMEmissionRequestValueUsbDeviceData(int nAutoTransfer = 0)
        {
            /*
            // USB DEVICE : EMISSION AUTO TRANSFER REQUEST VALUE 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData;
            // 초기화
            memset(&RequestDeviceData, 0x00, sizeof(REQUEST_USB_DEVICE_DATA));
            // SET USB DEVICE DATA : SEM CONTROL -> EMISSION AUTO TRANSFER REQUEST VALUE
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_EMISSION_AUTO_TRANSFER_VALUE;
            RequestDeviceData.nRequestType    = 0; // int:0, double:1
            RequestDeviceData.nRequestOption  = 1; // Data Count
            RequestDeviceData.nRequestData[0] = nAutoTransfer;
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_EMISSION_AUTO_TRANSFER_VALUE, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != NULL)
            {
                ((CNanoImpact2020cfeDlg*)m_pOwnerWindow)->OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, (LPARAM)&RequestDeviceData);
            }
            */
            
            // USB DEVICE : EMISSION AUTO TRANSFER REQUEST VALUE 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceEmissionAutoTransferValue(nAutoTransfer);
        }

        // ---- ModularAssistDevice.cpp:526 ----
        // SEM USB DEVICE CONTROL : FILAMENT VALUE 설정
        public void SetSEMFilamentValueUsbDeviceData(int nFilamentTargetPosition = 0, int nFilamentRunPosition = 0, int nFVExecute = 0)
        {
            /*
            // USB DEVICE : FILAMENT VALUE 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData;
            // 초기화
            memset(&RequestDeviceData, 0x00, sizeof(REQUEST_USB_DEVICE_DATA));
            // SET USB DEVICE DATA : SEM CONTROL -> FILAMENT VALUE
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_FILAMENT_VALUE;
            RequestDeviceData.nRequestType    = 0; // int:0, double:1
            RequestDeviceData.nRequestOption  = 1; // Data Count
            RequestDeviceData.nRequestData[0] = nFilamentTargetPosition;  // 목표값
            if (nFVExecute == 1)
            {
                RequestDeviceData.nRequestData[0] = nFilamentRunPosition; // 실행값
                RequestDeviceData.nRequestData[1] = 1;                    // USB DEVICE "FV" 실행
            }
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_FILAMENT_VALUE, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != NULL)
            {
                ((CNanoImpact2020cfeDlg*)m_pOwnerWindow)->OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, (LPARAM)&RequestDeviceData);
            }
            */
            
            // USB DEVICE : FILAMENT VALUE 적용
            // USB DEVICE SET 명령어설정
            if (nFVExecute == 1)
            {
                // 실행
                SetUsbDeviceFilamentValue(nFilamentRunPosition, nFVExecute);
            }
            else
            {
                // 설정
                SetUsbDeviceFilamentValue(nFilamentTargetPosition, 0);
            }
        }

        // ---- ModularAssistDevice.cpp:568 ----
        // SEM USB DEVICE CONTROL : FILAMENT VOLTAGE VALUE 설정
        public void SetSEMFilamentVoltageValueUsbDeviceData(int nPos, double dVMin, double dVMax, double dVStart, double dVStop, double dVCur, string strData, int nFVExecute)
        {
            /*
            // USB DEVICE : FILAMENT VOLTAGE VALUE 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData;
            // 초기화
            memset(&RequestDeviceData, 0x00, sizeof(REQUEST_USB_DEVICE_DATA));
            // SET USB DEVICE DATA : SEM CONTROL -> FILAMENT VOLTAGE VALUE
            RequestDeviceData.nRequestID     = REQIDX_SET_USB_DEVICE_SEM_FILAMENT_VOLT_VALUE;
            RequestDeviceData.nRequestType   = 0; // int:0, double:1
            RequestDeviceData.nRequestOption = 5; // Data Count
            if (nFVExecute == 1)
            {
                RequestDeviceData.dRequestData[0] = dVMin;   // Min     Voltage
                RequestDeviceData.dRequestData[1] = dVMax;   // Max     Voltage
                RequestDeviceData.dRequestData[2] = dVStart; // Start   Voltage
                RequestDeviceData.dRequestData[3] = dVStop;  // Stop    Voltage
                RequestDeviceData.dRequestData[4] = dVCur;   // Current Voltage
                RequestDeviceData.nRequestData[0] = nPos;    // Filament Position
                RequestDeviceData.nRequestData[1] = 1;       // USB DEVICE "FV" 실행
                strcpy_s(RequestDeviceData.szData, strData); // 원본 (CStringA) = ANSI(CP949) 변환 // Text Data
            }
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_FILAMENT_VOLT_VALUE, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != NULL)
            {
                ((CNanoImpact2020cfeDlg*)m_pOwnerWindow)->OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, (LPARAM)&RequestDeviceData);
            }
            */
            
            // USB DEVICE : FILAMENT VOLTAGE VALUE 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceFilamentVoltValue(nPos, dVMin, dVMax, dVStart, dVStop, dVCur, strData, nFVExecute);
        }

        // ---- ModularAssistDevice.cpp:606 ----
        // SEM USB DEVICE CONTROL : BIAS VALUE 설정
        public void SetSEMBiasValueUsbDeviceData(int nBiasTargetPosition = 0, int nBiasRunPosition = 0, int nBVExecute = 0)
        {
            /*
            // USB DEVICE : BIAS VALUE 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData;
            // 초기화
            memset(&RequestDeviceData, 0x00, sizeof(REQUEST_USB_DEVICE_DATA));
            // SET USB DEVICE DATA : SEM CONTROL -> BIAS VALUE
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_BIAS_VALUE;
            RequestDeviceData.nRequestType    = 0; // int:0, double:1
            RequestDeviceData.nRequestOption  = 1; // Data Count
            RequestDeviceData.nRequestData[0] = nBiasTargetPosition;  // 목표값
            if (nBVExecute == 1)
            {
                RequestDeviceData.nRequestData[0] = nBiasRunPosition; // 실행값
                RequestDeviceData.nRequestData[1] = 1;                // USB DEVICE "BV" 실행
            }
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_BIAS_VALUE, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != NULL)
            {
                ((CNanoImpact2020cfeDlg*)m_pOwnerWindow)->OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, (LPARAM)&RequestDeviceData);
            }
            */
            
            // USB DEVICE : BIAS VALUE 적용
            // USB DEVICE SET 명령어설정
            if (nBVExecute == 1)
            {
                // 실행
                SetUsbDeviceBiasValue(nBiasRunPosition, nBVExecute);
            }
            else
            {
                // 적용
                SetUsbDeviceBiasValue(nBiasTargetPosition, 0);
            }
        }

        // ---- ModularAssistDevice.cpp:648 ----
        // SEM USB DEVICE CONTROL : BIAS VOLTAGE VALUE 설정
        public void SetSEMBiasVoltageValueUsbDeviceData(int nPos, double dVMin, double dVMax, double dVStart, double dVStop, double dVCur, string strData, int nBVExecute)
        {
            /*
            // USB DEVICE : BIAS VOLTAGE VALUE 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData;
            // 초기화
            memset(&RequestDeviceData, 0x00, sizeof(REQUEST_USB_DEVICE_DATA));
            // SET USB DEVICE DATA : SEM CONTROL -> BIAS VOLTAGE VALUE
            RequestDeviceData.nRequestID = REQIDX_SET_USB_DEVICE_SEM_BIAS_VOLT_VALUE;
            RequestDeviceData.nRequestType = 0; // int:0, double:1
            RequestDeviceData.nRequestOption = 5; // Data Count
            if (nBVExecute == 1)
            {
                RequestDeviceData.dRequestData[0] = dVMin;   // Min     Voltage
                RequestDeviceData.dRequestData[1] = dVMax;   // Max     Voltage
                RequestDeviceData.dRequestData[2] = dVStart; // Start   Voltage
                RequestDeviceData.dRequestData[3] = dVStop;  // Stop    Voltage
                RequestDeviceData.dRequestData[4] = dVCur;   // Current Voltage
                RequestDeviceData.nRequestData[0] = nPos;    // Filament Position
                RequestDeviceData.nRequestData[1] = 1;       // USB DEVICE "BV" 실행
                strcpy_s(RequestDeviceData.szData, strData); // 원본 (CStringA) = ANSI(CP949) 변환 // Text Data
            }
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_BIAS_VOLT_VALUE, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != NULL)
            {
                ((CNanoImpact2020cfeDlg*)m_pOwnerWindow)->OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, (LPARAM)&RequestDeviceData);
            }
            */
            
            // USB DEVICE : BIAS VOLTAGE VALUE 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceBiasVoltValue(nPos, dVMin, dVMax, dVStart, dVStop, dVCur, strData, nBVExecute);
        }

        // ---- ModularAssistDevice.cpp:686 ----
        // SEM USB DEVICE CONTROL : SPOT SIZE 설정
        public void SetSEMSpotSizeUsbDeviceData(int nSemSpotSize = 0)
        {
            // SPOT SIZE 계산
            int nSpotSize = nSemSpotSize;// nSemSpotSizeCombo + 1;

            // USB DEVICE : SPOT SIZE 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : SEM CONTROL -> SPOT SIZE VALUE
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_SPOT_SIZE;
            RequestDeviceData.nRequestType    = 0; // int:0, double:1
            RequestDeviceData.nRequestOption  = 1; // Data Count
            RequestDeviceData.nRequestData[0] = nSpotSize;
            // 메시지전송
            if (m_pOwnerWindow != null)
            {
                m_pOwnerWindow.OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, RequestDeviceData);
            }

            
            // USB DEVICE : SPOT SIZE 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceSpotSize(nSpotSize);
        }

        // ---- ModularAssistDevice.cpp:715 ----
        // SEM USB DEVICE CONTROL : SPOT SIZE에 따른 CL1, CL2 설정
        public void SetSEMControlCL1CL2Value(int nSpotSize = 0)
        {
            
            // USB DEVICE : SPOT SIZE에 따른 CL1, CL2 VALUE 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceSemControlCL1CL2Value(nSpotSize);
        }

        // ---- ModularAssistDevice.cpp:725 ----
        // SEM USB DEVICE CONTROL : CL1, CL2 사용자 설정
        public void SetSEMControlCL1CL2ExecuteValue(int nCL1 = 0, int nCL2 = 0)
        {
            
            // USB DEVICE : CL1, CL2 사용자 VALUE 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceSemControlCL1CL2ExecuteValue(nCL1, nCL2);
        }

        // ---- ModularAssistDevice.cpp:735 ----
        // SEM USB DEVICE CONTROL : MAGNIFICATION INDEX 설정
        public void SetSEMMagnificationIndexUsbDeviceData(int nSemMagnificationCombo = 0, int nMagExecute = 1)
        {
            // MAGNIFICATION 인덱스 계산
            int nMagIndex = (MAX_MAG_STEP - 1) - nSemMagnificationCombo;

            // USB DEVICE : MAGNIFICATION INDEX 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : SEM CONTROL -> MAGNIFICATION INDEX VALUE
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_MAGNIFICATION_INDEX;
            RequestDeviceData.nRequestType    = 0; // int:0, double:1
            RequestDeviceData.nRequestOption  = 1; // Data Count
            RequestDeviceData.nRequestData[0] = nMagIndex;
            RequestDeviceData.nRequestData[1] = nMagExecute;
            // 메시지전송
            if (m_pOwnerWindow != null)
            {
                m_pOwnerWindow.OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, RequestDeviceData);
            }

            
            // USB DEVICE : MAGNIFICATION INDEX 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceMagnificationIndex(nMagIndex, nMagExecute);
        }

        // ---- ModularAssistDevice.cpp:765 ----
        // SEM USB DEVICE CONTROL : GUN ALIGN X 설정
        public void SetSEMGunAlignXValueUsbDeviceData(int nPos, string strData, int nExecute)
        {
            // USB DEVICE : GUN ALIGN X VALUE 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : SEM CONTROL -> GUN ALIGN X VALUE
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_GUN_ALIGN_X_VALUE;
            RequestDeviceData.nRequestType    = 0;        // int:0, double:1
            RequestDeviceData.nRequestOption  = 2;        // Data Count
            RequestDeviceData.nRequestData[0] = nPos;     // Gun Align X값 설정
            RequestDeviceData.nRequestData[1] = nExecute; // Gun Align X값 실행 (0:설정, 1:설정+실행)
            // Position String
            strcpy_s(RequestDeviceData.szData, strData); // 원본 (CStringA) = ANSI(CP949) 변환
            // 메시지전송
            if (m_pOwnerWindow != null)
            {
                m_pOwnerWindow.OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, RequestDeviceData);
            }

            // 2020.05.06
            // USB DEVICE : GUN ALIGN X VALUE 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceGunAlignXValue(nPos, nExecute);
        }

        // ---- ModularAssistDevice.cpp:794 ----
        // SEM USB DEVICE CONTROL : GUN ALIGN Y 설정
        public void SetSEMGunAlignYValueUsbDeviceData(int nPos, string strData, int nExecute)
        {
            // USB DEVICE : GUN ALIGN Y VALUE 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : SEM CONTROL -> GUN ALIGN Y VALUE
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_GUN_ALIGN_Y_VALUE;
            RequestDeviceData.nRequestType    = 0;        // int:0, double:1
            RequestDeviceData.nRequestOption  = 1;        // Data Count
            RequestDeviceData.nRequestData[0] = nPos;     // Gun Align Y값 설정
            RequestDeviceData.nRequestData[1] = nExecute; // Gun Align Y값 실행 (0:설정, 1:설정+실행)
            // Position String
            strcpy_s(RequestDeviceData.szData, strData); // 원본 (CStringA) = ANSI(CP949) 변환
            // 메시지전송
            if (m_pOwnerWindow != null)
            {
                m_pOwnerWindow.OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, RequestDeviceData);
            }

            // 2020.05.06
            // USB DEVICE : GUN ALIGN Y VALUE 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceGunAlignYValue(nPos, nExecute);
        }

        // ---- ModularAssistDevice.cpp:823 ----
        // SEM USB DEVICE CONTROL : BEAM ALIGN X 설정
        public void SetSEMBeamAlignXValueUsbDeviceData(int nPos, string strData, int nExecute)
        {
            // USB DEVICE : BEAM ALIGN X VALUE 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : SEM CONTROL -> BEAM ALIGN X VALUE
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_BEAM_ALIGN_X_VALUE;
            RequestDeviceData.nRequestType    = 0;        // int:0, double:1
            RequestDeviceData.nRequestOption  = 1;        // Data Count
            RequestDeviceData.nRequestData[0] = nPos;     // BEAM Align X값 설정
            RequestDeviceData.nRequestData[1] = nExecute; // BEAM Align X값 실행 (0:설정, 1:설정+실행)
            // Position String
            strcpy_s(RequestDeviceData.szData, strData); // 원본 (CStringA) = ANSI(CP949) 변환
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_BEAM_ALIGN_X_VALUE, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != null)
            {
                m_pOwnerWindow.OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, RequestDeviceData);
            }

            // 2020.05.06
            // USB DEVICE : BEAM ALIGN X VALUE 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceBeamAlignXValue(nPos, nExecute);
        }

        // ---- ModularAssistDevice.cpp:853 ----
        // SEM USB DEVICE CONTROL : BEAM ALIGN Y 설정
        public void SetSEMBeamAlignYValueUsbDeviceData(int nPos, string strData, int nExecute)
        {
            // USB DEVICE : BEAM ALIGN Y VALUE 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : SEM CONTROL -> BEAM ALIGN Y VALUE
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_BEAM_ALIGN_Y_VALUE;
            RequestDeviceData.nRequestType    = 0;        // int:0, double:1
            RequestDeviceData.nRequestOption  = 1;        // Data Count
            RequestDeviceData.nRequestData[0] = nPos;     // BEAM Align Y값 설정
            RequestDeviceData.nRequestData[1] = nExecute; // BEAM Align Y값 실행 (0:설정, 1:설정+실행)
            // Position String
            strcpy_s(RequestDeviceData.szData, strData); // 원본 (CStringA) = ANSI(CP949) 변환
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_BEAM_ALIGN_Y_VALUE, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != null)
            {
                m_pOwnerWindow.OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, RequestDeviceData);
            }

            // 2020.05.06
            // USB DEVICE : BEAM ALIGN Y VALUE 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceBeamAlignYValue(nPos, nExecute);
        }

        // ---- ModularAssistDevice.cpp:883 ----
        // SEM USB DEVICE CONTROL : IMAGE SHIFT ALIGN X 설정
        public void SetSEMImageShiftXValueUsbDeviceData(int nPos, string strData, int nExecute)
        {
            // USB DEVICE : IMAGE SHIFT X VALUE 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : SEM CONTROL -> IMAGE SHIFT X VALUE
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_IMAGE_SHIFT_X_VALUE;
            RequestDeviceData.nRequestType    = 0;        // int:0, double:1
            RequestDeviceData.nRequestOption  = 1;        // Data Count
            RequestDeviceData.nRequestData[0] = nPos;     // IMAGE SHIFT X값 설정
            RequestDeviceData.nRequestData[1] = nExecute; // IMAGE SHIFT X값 실행 (0:설정, 1:설정+실행)
            // Position String
            strcpy_s(RequestDeviceData.szData, strData); // 원본 (CStringA) = ANSI(CP949) 변환
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_IMAGE_SHIFT_X_VALUE, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != null)
            {
                m_pOwnerWindow.OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, RequestDeviceData);
            }

            // 2020.05.06
            // USB DEVICE : IMAGE SHIFT X VALUE 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceImageShiftXValue(nPos, nExecute);
        }

        // ---- ModularAssistDevice.cpp:913 ----
        // SEM USB DEVICE CONTROL : IMAGE SHIFT ALIGN Y 설정
        public void SetSEMImageShiftYValueUsbDeviceData(int nPos, string strData, int nExecute)
        {
            // USB DEVICE : IMAGE SHIFT Y VALUE 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : SEM CONTROL -> IMAGE SHIFT Y VALUE
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_IMAGE_SHIFT_Y_VALUE;
            RequestDeviceData.nRequestType    = 0;        // int:0, double:1
            RequestDeviceData.nRequestOption  = 1;        // Data Count
            RequestDeviceData.nRequestData[0] = nPos;     // IMAGE SHIFT Y값 설정
            RequestDeviceData.nRequestData[1] = nExecute; // IMAGE SHIFT Y값 실행 (0:설정, 1:설정+실행)
            // Position String
            strcpy_s(RequestDeviceData.szData, strData); // 원본 (CStringA) = ANSI(CP949) 변환
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_IMAGE_SHIFT_Y_VALUE, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != null)
            {
                m_pOwnerWindow.OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, RequestDeviceData);
            }

            // 2020.05.06
            // USB DEVICE : IMAGE SHIFT Y VALUE 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceImageShiftYValue(nPos, nExecute);
        }

        // ---- ModularAssistDevice.cpp:943 ----
        // SEM USB DEVICE CONTROL : STIG X 설정
        public void SetSEMStigXValueUsbDeviceData(int nPos, string strData, int nExecute)
        {
            // USB DEVICE : STIG X VALUE 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : SEM CONTROL -> STIG X VALUE
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_STIG_X_VALUE;
            RequestDeviceData.nRequestType    = 0;        // int:0, double:1
            RequestDeviceData.nRequestOption  = 1;        // Data Count
            RequestDeviceData.nRequestData[0] = nPos;     // STIG X값 설정
            RequestDeviceData.nRequestData[1] = nExecute; // STIG X값 실행 (0:설정, 1:설정+실행)
            // Position String
            strcpy_s(RequestDeviceData.szData, strData); // 원본 (CStringA) = ANSI(CP949) 변환
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_STIG_X_VALUE, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != null)
            {
                m_pOwnerWindow.OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, RequestDeviceData);
            }

            // 2020.05.06
            // USB DEVICE : STIG X VALUE 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceStigXValue(nPos, nExecute);
        }

        // ---- ModularAssistDevice.cpp:973 ----
        // SEM USB DEVICE CONTROL : STIG Y 설정
        public void SetSEMStigYValueUsbDeviceData(int nPos, string strData, int nExecute)
        {
            // USB DEVICE : STIG Y VALUE 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : SEM CONTROL -> STIG Y VALUE
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_STIG_Y_VALUE;
            RequestDeviceData.nRequestType    = 0;        // int:0, double:1
            RequestDeviceData.nRequestOption  = 1;        // Data Count
            RequestDeviceData.nRequestData[0] = nPos;     // STIG Y값 설정
            RequestDeviceData.nRequestData[1] = nExecute; // STIG Y값 실행 (0:설정, 1:설정+실행)
            // Position String
            strcpy_s(RequestDeviceData.szData, strData); // 원본 (CStringA) = ANSI(CP949) 변환
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_STIG_Y_VALUE, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != null)
            {
                m_pOwnerWindow.OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, RequestDeviceData);
            }

            // 2020.05.06
            // USB DEVICE : STIG Y VALUE 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceStigYValue(nPos, nExecute);
        }

        // ---- ModularAssistDevice.cpp:1003 ----
        // SEM USB DEVICE CONTROL : SCAN ROTATION 설정
        public void SetSEMScanRotationValueUsbDeviceData(int nValue, string strData, int nExecute)
        {
            // USB DEVICE : SCAN ROTATION VALUE 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // 초기화
            RequestDeviceData = new REQUEST_USB_DEVICE_DATA();
            // SET USB DEVICE DATA : SEM CONTROL -> SCAN ROTATION VALUE
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_SCAN_ROTATION_VALUE;
            RequestDeviceData.nRequestType    = 0; // int:0, double:1
            RequestDeviceData.nRequestOption  = 1; // Data Count
            RequestDeviceData.nRequestData[0] = nValue;   // m_nSemScanRotationScrollPos;
            RequestDeviceData.nRequestData[1] = nExecute; // USB DEVICE EXECUTE
            // Position String
            strcpy_s(RequestDeviceData.szData, strData); // 원본 (CStringA) = ANSI(CP949) 변환
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_SCAN_ROTATION_VALUE, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != null)
            {
                m_pOwnerWindow.OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, RequestDeviceData);
            }
            
            // USB DEVICE : SCAN ROTATION VALUE 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceScanRotationValue(nValue, nExecute);
        }

        // ---- ModularAssistDevice.cpp:1032 ----
        // SEM USB DEVICE CONTROL : GUN ALIGN 설정
        public void SetSEMControlGunAlignValue(int nGAX = 0, int nGAY = 0, int nExecute = 0)
        {
            /*
            // USB DEVICE : GUN ALIGN XY VALUE 적용
            // USB DEVICE SET 명령어설정
            REQUEST_USB_DEVICE_DATA RequestDeviceData;
            // 초기화
            memset(&RequestDeviceData, 0x00, sizeof(REQUEST_USB_DEVICE_DATA));
            // SET USB DEVICE DATA : SEM CONTROL -> GUN ALIGN XY
            RequestDeviceData.nRequestID      = REQIDX_SET_USB_DEVICE_SEM_CONTROL_GUN_ALIGN_VALUE;
            RequestDeviceData.nRequestType    = 0;        // int:0, double:1
            RequestDeviceData.nRequestOption  = 3;        // Data Count
            RequestDeviceData.nRequestData[0] = nGAX;     // GUN ALIGN X
            RequestDeviceData.nRequestData[1] = nGAY;     // GUN ALIGN Y
            RequestDeviceData.nRequestData[2] = nExecute; // USB EXECUTE (0:설정, 1:설정및실행)
            // 메시지전송
            //::SendMessage(m_hOwnerWnd, UM_SET_USB_DEVICE_SEM_CONTROL_GUN_ALIGN_VALUE, NULL, (LPARAM)&RequestDeviceData);
            if (m_pOwnerWindow != NULL)
            {
                ((CNanoImpact2020cfeDlg*)m_pOwnerWindow)->OnSetUsbDeviceSEMControlDataToDeviceServer(NULL, (LPARAM)&RequestDeviceData);
            }
            */
            
            // USB DEVICE : GUN ALIGN XY VALUE 적용
            // USB DEVICE SET 명령어설정
            SetUsbDeviceSemControlGunAlignValue(nGAX, nGAY, nExecute);
        }

    }
}
