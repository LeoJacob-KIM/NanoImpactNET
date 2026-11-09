// 원본 ModularBeamControlDlg.cpp:6771-8845, 8848-8934 을 NanoImpact.CodeGen translate로 옮긴 뒤 손으로 고친 코드.
#pragma warning disable IDE1006, IDE0059, CS0162, CS0168, CS0219
using System;
using NanoImpact.Core.Native;
using static NanoImpact.Core.Def;
using static NanoImpact.Core.Native.CRuntime;

namespace NanoImpact.Hardware.Spellman
{
    public sealed partial class CfeHVPowerControl
    {
        // ---- ModularBeamControlDlg.cpp:6771 ----
        public void SetCfeHVPowerMonitoringTimerExecute(int nTimerMode)
        {
            // DIALOG WINDOW INITILIZE START FLAG
            if (m_bWindowInitStartFlag == FALSE) return;

            // SET MONITORING TIMER
            if (m_pUsbRS232Comm == null) return;

            // CFE HV POWER MONITORING MODE
            // 0 -> 일반모드 : 작업준비상태,  1 -> 스캔모드 : 작업준비상태
            if (m_nCfeHVPowerMonitoringMode == nTimerMode)
            {
                return;
            }
            m_nCfeHVPowerMonitoringMode = nTimerMode;

            KillTimer(ID_TIMER_CFE_HV_POWER_ALL_MONITORING);
            KillTimer(ID_TIMER_CFE_HV_POWER_HV_MONITORING);

            m_nCfeHVPowerAllMonitoringIndex = 0;
            m_nCfeHVPowerHVMonitoringIndex  = 0;
            m_nCfeHVPowerOneTimeMonitoring  = 0;

            // NORMAL MONITORING MODE
            if (nTimerMode == 0)
            {
                // Normal Mode : 100[ms] -> All Data
                SetTimer(ID_TIMER_CFE_HV_POWER_ALL_MONITORING, 100, NULL);
            }
            // BEAM, HEATER, SUPPRESSOR, EXTRACTOR HV PROCESS MODE
            else
            {
                // HV Mode : 200[ms] -> BEAM, HEATER, SUPPRESSOR, EXTRACTOR
                SetTimer(ID_TIMER_CFE_HV_POWER_HV_MONITORING, 200, NULL);

            }

        }

        // ---- ModularBeamControlDlg.cpp:6811 ----
        public void ExecuteCfeHVPowerAllDataMonitoring()
        {
            // CFE HV POWER ALL DATA MONITORING

            // LIST INSERT TEST
            //((CNanoImpact2020cfeDlg*)m_pOwnerWindow)->InsertTestListControl(m_nCfeHVPowerAllMonitoringIndex);

            //
            // MONITORING
            //

            //
            // 00, 
            //
            if (m_nCfeHVPowerAllMonitoringIndex == 0)
            {
                // MONITORING 제외
            }
            //
            // 01, Get Connection
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 1)
            {
                // MONITORING 시작시 1번만 한다
                if (m_nCfeHVPowerOneTimeMonitoring == 0)
                {
                    SendingCfeHVPower01Command();
                }
            }
            //
            // 02, Get Short Status
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 2)
            {
                // MONITORING 시작 후 3 배수
                if (m_nCfeHVPowerOneTimeMonitoring % 3 == 0)
                {
                    SendingCfeHVPower02Command();
                }
            }
            //
            // 03, Switch Supply
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 3)
            {
                // MONITORING 제외
            }
            //
            // 04, Switch Supply
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 4)
            {
                // MONITORING 제외
            }
            //
            // 05, Get Firmware Version
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 5)
            {
                // MONITORING 시작시 1번만 한다
                if (m_nCfeHVPowerOneTimeMonitoring == 0)
                {
                    // 0 = Main Controller (Gateway)
                    SendingCfeHVPower05Command(0);
                }
                else if (m_nCfeHVPowerOneTimeMonitoring == 1)
                {
                    // 1 = Floating Deck (extractor, suppressor, heater)
                    SendingCfeHVPower05Command(1);
                }
                else
                {
                    //
                }
            }
            //
            // 08, Get Beam Voltage
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 6)
            {
                // MONITORING 시작 후 짝수
                if (m_nCfeHVPowerOneTimeMonitoring % 2 == 0)
                {
                    // 0 = Target Value
                    SendingCfeHVPower08Command(0);
                }
                // MONITORING 시작 후 홀수
                else
                {
                    // 1 = Monitored Value
                    SendingCfeHVPower08Command(1);
                }
            }
            //
            // 09, Set Beam Voltage
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 7)
            {
                // MONITORING 제외
            }
            //
            // 0A, Get Beam Voltage Ramping Rates
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 8)
            {
                // MONITORING 시작 후 짝수
                if (m_nCfeHVPowerOneTimeMonitoring % 2 == 0)
                {
                    SendingCfeHVPower0ACommand();
                }
                // MONITORING 시작 후 홀수
                else
                {
                }
            }
            //
            // 0B, Set Beam Voltage Ramping Rates
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 9)
            {
                // MONITORING 제외
            }
            //
            // 0E, Get Beam Current
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 10)
            {
                // MONITORING 시작 후 짝수
                if (m_nCfeHVPowerOneTimeMonitoring % 2 == 0)
                {
                }
                // MONITORING 시작 후 홀수
                else
                {
                    SendingCfeHVPower0ECommand();
                }
            }

            //
            // 14, Get Extractor Voltage
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 11)
            {
                // MONITORING 시작 후 짝수
                if (m_nCfeHVPowerOneTimeMonitoring % 2 == 0)
                {
                    // 0 = Target Value
                    SendingCfeHVPower14Command(0);
                }
                // MONITORING 시작 후 홀수
                else
                {
                    // 1 = Monitored Value
                    SendingCfeHVPower14Command(1);
                }
            }
            //
            // 15, Set Extractor Voltage
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 12)
            {
                // MONITORING 제외
            }
            //
            // 16, Get Extractor Voltage Ramping Rates
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 13)
            {
                // MONITORING 시작 후 짝수
                if (m_nCfeHVPowerOneTimeMonitoring % 2 == 0)
                {
                    SendingCfeHVPower16Command();
                }
                // MONITORING 시작 후 홀수
                else
                {
                }
            }
            //
            // 17, Set Extractor Voltage Ramping Rates
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 14)
            {
            // MONITORING 제외
            }
            //
            // 1A, Get Extractor Current
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 15)
            {
                // MONITORING 시작 후 짝수
                if (m_nCfeHVPowerOneTimeMonitoring % 2 == 0)
                {
                }
                // MONITORING 시작 후 홀수
                else
                {
                    SendingCfeHVPower1ACommand();
                }
            }
            //
            // 1C, Get Extractor Trip Current
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 16)
            {
                // MONITORING 시작 후 3 배수
                if (m_nCfeHVPowerOneTimeMonitoring % 3 == 0)
                {
                    SendingCfeHVPower1CCommand();
                }
            }
            //
            // 1D, Set Extractor Trip Current
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 17)
            {
                // MONITORING 제외
            }

            //
            // 1E, Get Suppressor Voltage
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 18)
            {
                // MONITORING 시작 후 짝수
                if (m_nCfeHVPowerOneTimeMonitoring % 2 == 0)
                {
                    // 0 = Target Value
                    SendingCfeHVPower1ECommand(0);
                }
                // MONITORING 시작 후 홀수
                else
                {
                    // 1 = Monitored Value
                    SendingCfeHVPower1ECommand(1);
                }
            }
            //
            // 1F, Set Suppressor Voltage
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 19)
            {
                // MONITORING 제외
            }
            //
            // 20, Get Suppressor Voltage Ramping Rates
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 20)
            {
                // MONITORING 시작 후 3 배수
                if (m_nCfeHVPowerOneTimeMonitoring % 3 == 0)
                {
                    SendingCfeHVPower20Command();
                }
            }
            //
            // 21, Set Suppressor Voltage Ramping Rates
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 21)
            {
                // MONITORING 제외
            }
            //
            // 24, Get Suppressor Current
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 22)
            {
                // MONITORING 시작 후 짝수
                if (m_nCfeHVPowerOneTimeMonitoring % 2 == 0)
                {
                }
                // MONITORING 시작 후 홀수
                else
                {
                    SendingCfeHVPower24Command();
                }
            }
            //
            // 26, Get Heater Voltage
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 23)
            {
                // MONITORING 시작 후 짝수
                if (m_nCfeHVPowerOneTimeMonitoring % 2 == 0)
                {
                }
                // MONITORING 시작 후 홀수
                else
                {
                    SendingCfeHVPower26Command();
                }
            }
            //
            // 28, Get Heater Current
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 24)
            {
                // MONITORING 시작 후 짝수
                if (m_nCfeHVPowerOneTimeMonitoring % 2 == 0)
                {
                    // 0 = Target Current
                    SendingCfeHVPower28Command(0);
                }
                // MONITORING 시작 후 홀수
                else
                {
                    // 1 = Monitored Current
                    SendingCfeHVPower28Command(1);
                }
            }
            //
            // 29, Set Heater Current
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 25)
            {
                // MONITORING 제외
            }
            //
            // 2A, Get Heater Current Ramping Rates
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 26)
            {
                // MONITORING 시작 후 짝수
                if (m_nCfeHVPowerOneTimeMonitoring % 3 == 0)
                {
                    SendingCfeHVPower2ACommand();
                }
            }
            //
            // 2B, Set Heater Current Ramping Rates
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 27)
            {
                // MONITORING 제외
            }
            //
            // 50, Get Overall Status
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 28)
            {
                // MONITORING 시작 후 10배수
                if (m_nCfeHVPowerOneTimeMonitoring % 10 == 0)
                {
                    SendingCfeHVPower50Command();
                }
            }

            // 순번 증가
            m_nCfeHVPowerAllMonitoringIndex++;

            if (m_nCfeHVPowerAllMonitoringIndex >= 28) // MAX_CFE_HV_POWER_COMMAND)
            {
                // CFE HV POWER DATA MONITORING - 1 TIME (한번만)
                m_nCfeHVPowerOneTimeMonitoring++;
                if (m_nCfeHVPowerOneTimeMonitoring > 90000)
                {
                    m_nCfeHVPowerOneTimeMonitoring = 90;
                }

                // INDEX RESET 
                m_nCfeHVPowerAllMonitoringIndex = 0;
            }

        }

        // ---- ModularBeamControlDlg.cpp:7177 ----
        public void ExecuteCfeHVPowerHVProcessDataMonitoring()
        {
            // 스캔모드 작업시 DATA MONITORING

            //
            // MONITORING
            //

            //
            // 02, Get Short Status
            //
            if (m_nCfeHVPowerAllMonitoringIndex == 0)
            {
                SendingCfeHVPower02Command();
            }

            //
            // 08, Get Beam Voltage
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 1)
            {
                // 0 = Target Value
                SendingCfeHVPower08Command(0);
            }
            //
            // 08, Get Beam Voltage
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 2)
            {
                // 1 = Monitored Value
                SendingCfeHVPower08Command(1);
            }
            //
            // 0A, Get Beam Voltage Ramping Rates
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 3)
            {
                SendingCfeHVPower0ACommand();
            }
            //
            // 0E, Get Beam Current
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 4)
            {
                SendingCfeHVPower0ECommand();
            }
            //
            // 14, Get Extractor Voltage
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 5)
            {
                // 0 = Target Value
                SendingCfeHVPower14Command(0);
            }
            //
            // 14, Get Extractor Voltage
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 6)
            {
                // 1 = Monitored Value
                SendingCfeHVPower14Command(1);
            }
            //
            // 16, Get Extractor Voltage Ramping Rates
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 7)
            {
                SendingCfeHVPower16Command();
            }
            //
            // 1A, Get Extractor Current
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 8)
            {
                SendingCfeHVPower1ACommand();
            }
            //
            // 1C, Get Extractor Trip Current
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 9)
            {
                SendingCfeHVPower1CCommand();
            }
            //
            // 1E, Get Suppressor Voltage
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 10)
            {
                // 0 = Target Value
                SendingCfeHVPower1ECommand(0);
            }
            //
            // 1E, Get Suppressor Voltage
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 11)
            {
                // 1 = Monitored Value
                SendingCfeHVPower1ECommand(1);
            }
            //
            // 20, Get Suppressor Voltage Ramping Rates
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 12)
            {
                SendingCfeHVPower20Command();
            }
            //
            // 24, Get Suppressor Current
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 13)
            {
                SendingCfeHVPower24Command();
            }
            //
            // 26, Get Heater Voltage
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 14)
            {
                SendingCfeHVPower26Command();
            }
            //
            // 28, Get Heater Current
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 15)
            {
                // 0 = Target Current
                SendingCfeHVPower28Command(0);
            }
            //
            // 28, Get Heater Current
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 16)
            {
                // 1 = Monitored Current
                SendingCfeHVPower28Command(1);
            }
            //
            // 2A, Get Heater Current Ramping Rates
            //
            else if (m_nCfeHVPowerAllMonitoringIndex == 17)
            {
                SendingCfeHVPower2ACommand();
            }

            // 순번 증가
            m_nCfeHVPowerAllMonitoringIndex++;

            if (m_nCfeHVPowerAllMonitoringIndex >= 17)
            {
                // CFE HV POWER DATA MONITORING - 1 TIME (한번만)
                m_nCfeHVPowerOneTimeMonitoring++;
                if (m_nCfeHVPowerOneTimeMonitoring > 90000)
                {
                    m_nCfeHVPowerOneTimeMonitoring = 90;
                }

                // INDEX RESET 
                m_nCfeHVPowerAllMonitoringIndex = 0;
            }

        }

        // ---- ModularBeamControlDlg.cpp:7340 ----
        public int SendingCfeHVPower00Command()
        {
            if (m_pUsbRS232Comm == null) return 0;

            // CFE HV POWER ALL DATA MONITORING
            CharBuffer szCommand = new CharBuffer(64);
            CharBuffer szHexCSUM = new CharBuffer(10);
            ushort wCheckSum = 0;

            // 00, Reset Supply Requset
            // <:><00><SP><CSUM><LF>
            memset(szCommand, 0x00, 64);
            memset(szHexCSUM, 0x00, 10);

            // Command
            szCommand[0] = (byte)':';  // START
            szCommand[1] = (byte)'0';  // CMD
            szCommand[2] = (byte)'0';  // CMD
            szCommand[3] = (byte)' ';  // SP
            szCommand[4] = NULL; // CSUM
            szCommand[5] = NULL; // CSUM
            szCommand[6] = 0x0A; // LF
            szCommand[7] = NULL; // NULL

            // CHECK SUM 구하기
            wCheckSum = MakeCfeHVPowerCommandCheckSum(szCommand, (ushort)(3));
            sprintf(szHexCSUM, "%02X", wCheckSum);
            szCommand[4] = szHexCSUM[0]; // CSUM
            szCommand[5] = szHexCSUM[1]; // CSUM

            // 전송
            m_pUsbRS232Comm.Send(szCommand, 7);

            return 1;
        }

        // ---- ModularBeamControlDlg.cpp:7377 ----
        public int SendingCfeHVPower01Command()
        {
            if (m_pUsbRS232Comm == null) return 0;

            // CFE HV POWER ALL DATA MONITORING
            CharBuffer szCommand = new CharBuffer(64);
            CharBuffer szHexCSUM = new CharBuffer(10);
            ushort wCheckSum = 0;

            // 01, Get Connection
            // <:><01><SP><CSUM><LF>
            memset(szCommand, 0x00, 64);
            memset(szHexCSUM, 0x00, 10);

            // Command
            szCommand[0] = (byte)':';  // START
            szCommand[1] = (byte)'0';  // CMD
            szCommand[2] = (byte)'1';  // CMD
            szCommand[3] = (byte)' ';  // SP
            szCommand[4] = NULL; // CSUM
            szCommand[5] = NULL; // CSUM
            szCommand[6] = 0x0A; // LF
            szCommand[7] = NULL; // NULL

            // CHECK SUM 구하기
            wCheckSum = MakeCfeHVPowerCommandCheckSum(szCommand, (ushort)(3));
            sprintf(szHexCSUM, "%02X", wCheckSum);
            szCommand[4] = szHexCSUM[0]; // CSUM
            szCommand[5] = szHexCSUM[1]; // CSUM

            // 전송
            m_pUsbRS232Comm.Send(szCommand, 7);

            return 1;
        }

        // ---- ModularBeamControlDlg.cpp:7414 ----
        public int SendingCfeHVPower02Command()
        {
            if (m_pUsbRS232Comm == null) return 0;

            // CFE HV POWER ALL DATA MONITORING
            CharBuffer szCommand = new CharBuffer(64);
            CharBuffer szHexCSUM = new CharBuffer(10);
            ushort wCheckSum = 0;

            // 01, Get Short Status
            // <:><02><SP><CSUM><LF>
            memset(szCommand, 0x00, 64);
            memset(szHexCSUM, 0x00, 10);

            // Command
            szCommand[0] = (byte)':';  // START
            szCommand[1] = (byte)'0';  // CMD
            szCommand[2] = (byte)'2';  // CMD
            szCommand[3] = (byte)' ';  // SP
            szCommand[4] = NULL; // CSUM
            szCommand[5] = NULL; // CSUM
            szCommand[6] = 0x0A; // LF
            szCommand[7] = NULL; // NULL

            // CHECK SUM 구하기
            wCheckSum = MakeCfeHVPowerCommandCheckSum(szCommand, (ushort)(3));
            sprintf(szHexCSUM, "%02X", wCheckSum);
            szCommand[4] = szHexCSUM[0]; // CSUM
            szCommand[5] = szHexCSUM[1]; // CSUM

            // 전송
            m_pUsbRS232Comm.Send(szCommand, 7);

            return 1;
        }

        // ---- ModularBeamControlDlg.cpp:7451 ----
        public int SendingCfeHVPower03Command(int nArg)
        {
            if (m_pUsbRS232Comm == null) return 0;

            // CFE HV POWER ALL DATA MONITORING
            CharBuffer szCommand = new CharBuffer(64);
            CharBuffer szHexCSUM = new CharBuffer(10);
            ushort wCheckSum = 0;

            if (nArg < 0 || nArg > 9) return 0;

            /*
            ARG Command
            00 Beam Energy Supply < ON>
            01 Beam Energy Supply < OFF>
            02 Extractor Supply < ON>
            03 Extractor Supply < OFF>
            04 Suppressor Supply < ON>
            05 Suppressor Supply < OFF>
            06 Heater Supply < ON>
            07 Heater Supply <OFF>
            08 Wobbler <ON>
            09 Wobbler <OFF>
            */

            // 03, Switch Supply
            // <:><03><ARG><SP><CSUM><LF>
            memset(szCommand, 0x00, 64);
            memset(szHexCSUM, 0x00, 10);

            // Command
            szCommand[0] = (byte)':';  // START
            szCommand[1] = (byte)'0';  // CMD
            szCommand[2] = (byte)'3';  // CMD
            // ARG
            if (nArg == 0)
            {
                szCommand[3] = (byte)'0';  // Beam Energy Supply < ON>
                szCommand[4] = (byte)'0';  // Beam Energy Supply < ON>
            }
            else if (nArg == 1)
            {
                szCommand[3] = (byte)'0';  // Beam Energy Supply <OFF>
                szCommand[4] = (byte)'1';  // Beam Energy Supply <OFF>
            }
            else if (nArg == 2)
            {
                szCommand[3] = (byte)'0';  // Extractor Supply <ON>
                szCommand[4] = (byte)'2';  // Extractor Supply <ON>
            }
            else if (nArg == 3)
            {
                szCommand[3] = (byte)'0';  // Extractor Supply <OFF>
                szCommand[4] = (byte)'3';  // Extractor Supply <OFF>
            }
            else if (nArg == 4)
            {
                szCommand[3] = (byte)'0';  // Suppressor Supply <ON>
                szCommand[4] = (byte)'4';  // Suppressor Supply <ON>
            }
            else if (nArg == 5)
            {
                szCommand[3] = (byte)'0';  // Suppressor Supply <OFF>
                szCommand[4] = (byte)'5';  // Suppressor Supply <OFF>
            }
            else if (nArg == 6)
            {
                szCommand[3] = (byte)'0';  // Heater Supply <ON>
                szCommand[4] = (byte)'6';  // Heater Supply <ON>
            }
            else if (nArg == 7)
            {
                szCommand[3] = (byte)'0';  // Heater Supply <OFF>
                szCommand[4] = (byte)'7';  // Heater Supply <OFF>
            }
            else if (nArg == 8)
            {
                szCommand[3] = (byte)'0';  // Wobbler <ON>
                szCommand[4] = (byte)'8';  // Wobbler <ON>
            }
            else if (nArg == 9)
            {
                szCommand[3] = (byte)'0';  // Wobbler <OFF>
                szCommand[4] = (byte)'9';  // Wobbler <OFF>
            }

            szCommand[5] = (byte)' ';  // SP
            szCommand[6] = NULL; // CSUM
            szCommand[7] = NULL; // CSUM
            szCommand[8] = 0x0A; // LF
            szCommand[9] = NULL; // NULL

            // CHECK SUM 구하기
            wCheckSum = MakeCfeHVPowerCommandCheckSum(szCommand, (ushort)(5));
            sprintf(szHexCSUM, "%02X", wCheckSum);
            szCommand[6] = szHexCSUM[0]; // CSUM
            szCommand[7] = szHexCSUM[1]; // CSUM

            // 전송
            m_pUsbRS232Comm.Send(szCommand, 9);

            return 1;
        }

        // ---- ModularBeamControlDlg.cpp:7556 ----
        public int SendingCfeHVPower04Command(int nArg)
        {
            if (m_pUsbRS232Comm == null) return 0;

            // CFE HV POWER ALL DATA MONITORING
            CharBuffer szCommand = new CharBuffer(64);
            CharBuffer szHexCSUM = new CharBuffer(10);
            ushort wCheckSum = 0;

            if (nArg < 1 || nArg > 4) return 0;

            /*
            ARG Command
            01 Beam Energy Supply
            02 Extractor Supply
            03 Suppressor Supply
            04 Heater Supply
            */

            // 04, Set Stop Ramping
            // <:><04><ARG><SP><CSUM><LF>
            memset(szCommand, 0x00, 64);
            memset(szHexCSUM, 0x00, 10);

            // Command
            szCommand[0] = (byte)':';  // START
            szCommand[1] = (byte)'0';  // CMD
            szCommand[2] = (byte)'4';  // CMD
            // ARG
            if (nArg == 1)
            {
                szCommand[3] = (byte)'0';  // Beam Energy Supply
                szCommand[4] = (byte)'1';  // Beam Energy Supply
            }
            else if (nArg == 2)
            {
                szCommand[3] = (byte)'0';  // Extractor Supply
                szCommand[4] = (byte)'2';  // Extractor Supply
            }
            else if (nArg == 3)
            {
                szCommand[3] = (byte)'0';  // Suppressor Supply
                szCommand[4] = (byte)'3';  // Suppressor Supply
            }
            else if (nArg == 4)
            {
                szCommand[3] = (byte)'0';  // Heater Supply
                szCommand[4] = (byte)'4';  // Heater Supply
            }

            szCommand[5] = (byte)' ';  // SP
            szCommand[6] = NULL; // CSUM
            szCommand[7] = NULL; // CSUM
            szCommand[8] = 0x0A; // LF
            szCommand[9] = NULL; // NULL

            // CHECK SUM 구하기
            wCheckSum = MakeCfeHVPowerCommandCheckSum(szCommand, (ushort)(5));
            sprintf(szHexCSUM, "%02X", wCheckSum);
            szCommand[6] = szHexCSUM[0]; // CSUM
            szCommand[7] = szHexCSUM[1]; // CSUM

            // 전송
            m_pUsbRS232Comm.Send(szCommand, 9);

            return 1;
        }

        // ---- ModularBeamControlDlg.cpp:7625 ----
        public int SendingCfeHVPower05Command(int nArg)
        {
            if (m_pUsbRS232Comm == null) return 0;

            // CFE HV POWER ALL DATA MONITORING
            CharBuffer szCommand = new CharBuffer(64);
            CharBuffer szHexCSUM = new CharBuffer(10);
            ushort wCheckSum = 0;

            if (nArg < 0 || nArg > 1) return 0;

            /*
            ARG Command
            0 = Main Controller (Gateway)
            1 = Floating Deck (extractor, suppressor, heater)
            */

            // 05, Get Firmware Version
            // <:><05><ARG><SP><CSUM><LF>
            memset(szCommand, 0x00, 64);
            memset(szHexCSUM, 0x00, 10);

            // Command
            szCommand[0] = (byte)':';  // START
            szCommand[1] = (byte)'0';  // CMD
            szCommand[2] = (byte)'5';  // CMD
            // ARG
            if (nArg == 0)
            {
                szCommand[3] = (byte)'0';  // Main Controller (Gateway)
            }
            else if (nArg == 1)
            {
                szCommand[3] = (byte)'1';  // Floating Deck (extractor, suppressor, heater)
            }

            szCommand[4] = (byte)' ';  // SP
            szCommand[5] = NULL; // CSUM
            szCommand[6] = NULL; // CSUM
            szCommand[7] = 0x0A; // LF
            szCommand[8] = NULL; // NULL

            // CHECK SUM 구하기
            wCheckSum = MakeCfeHVPowerCommandCheckSum(szCommand, (ushort)(4));
            sprintf(szHexCSUM, "%02X", wCheckSum);
            szCommand[5] = szHexCSUM[0]; // CSUM
            szCommand[6] = szHexCSUM[1]; // CSUM

            // 전송
            m_pUsbRS232Comm.Send(szCommand, 8);

            return 1;
        }

        // ---- ModularBeamControlDlg.cpp:7680 ----
        public int SendingCfeHVPower08Command(int nArg)
        {
            if (m_pUsbRS232Comm == null) return 0;

            // CFE HV POWER ALL DATA MONITORING
            CharBuffer szCommand = new CharBuffer(64);
            CharBuffer szHexCSUM = new CharBuffer(10);
            ushort wCheckSum = 0;

            if (nArg < 0 || nArg > 1) return 0;

            /*
            ARG Command
            0 = Target Value
            1 = Monitored Value
            */

            // 08, Get Beam Voltage
            // <:><08><ARG><SP><CSUM><LF>
            memset(szCommand, 0x00, 64);
            memset(szHexCSUM, 0x00, 10);

            // Command
            szCommand[0] = (byte)':';  // START
            szCommand[1] = (byte)'0';  // CMD
            szCommand[2] = (byte)'8';  // CMD
            // ARG
            if (nArg == 0)
            {
                szCommand[3] = (byte)'0';  // Target Value
            }
            else if (nArg == 1)
            {
                szCommand[3] = (byte)'1';  // Monitored Value
            }

            szCommand[4] = (byte)' ';  // SP
            szCommand[5] = NULL; // CSUM
            szCommand[6] = NULL; // CSUM
            szCommand[7] = 0x0A; // LF
            szCommand[8] = NULL; // NULL

            // CHECK SUM 구하기
            wCheckSum = MakeCfeHVPowerCommandCheckSum(szCommand, (ushort)(4));
            sprintf(szHexCSUM, "%02X", wCheckSum);
            szCommand[5] = szHexCSUM[0]; // CSUM
            szCommand[6] = szHexCSUM[1]; // CSUM

            // 전송
            m_pUsbRS232Comm.Send(szCommand, 8);

            return 1;
        }

        // ---- ModularBeamControlDlg.cpp:7733 ----
        public int SendingCfeHVPower09Command(string strText)
        {
            if (m_pUsbRS232Comm == null) return 0;

            // CFE HV POWER ALL DATA MONITORING
            CharBuffer szCommand = new CharBuffer(64);
            CharBuffer szHexCSUM = new CharBuffer(10);
            CharBuffer szValue = new CharBuffer(10);
            ushort wCheckSum = 0;

            // Set Value Max 7 Digit : Target value in the format xxxxx.x
            // Valid range : 0V to 30000V
            double dValue = _wtof(strText);
            if (dValue < 0.0    ) dValue = 0.0;
            if (dValue > 30000.0) dValue = 30000.0;
            memset(szValue, 0x00, 10);
            sprintf(szValue, "%07.1f", dValue);

            int nLen = strlen(szValue);
            if (nLen < 3 || nLen > 7) return 0;

            // 09, Set Beam Voltage
            // <:><09><ARG><SP><CSUM><LF>
            memset(szCommand, 0x00, 64);
            memset(szHexCSUM, 0x00, 10);

            // Command
            szCommand[0] = (byte)':';  // START
            szCommand[1] = (byte)'0';  // CMD
            szCommand[2] = (byte)'9';  // CMD
            // Value
            strncpy(szCommand + 3, szValue, nLen);
            nLen = strlen(szCommand);
            szCommand[nLen] = (byte)' ';  // SP
            // CSUM
            szCommand[nLen + 1] = NULL; // CSUM
            szCommand[nLen + 2] = NULL; // CSUM
            szCommand[nLen + 3] = 0x0A; // LF
            szCommand[nLen + 4] = NULL; // NULL

            // CHECK SUM 구하기
            wCheckSum = MakeCfeHVPowerCommandCheckSum(szCommand, (ushort)(nLen + 1));
            sprintf(szHexCSUM, "%02X", wCheckSum);
            szCommand[nLen + 1] = szHexCSUM[0]; // CSUM
            szCommand[nLen + 2] = szHexCSUM[1]; // CSUM

            // 전송
            nLen = strlen(szCommand);
            m_pUsbRS232Comm.Send(szCommand, nLen);

            return 1;
        }

        // ---- ModularBeamControlDlg.cpp:7787 ----
        public int SendingCfeHVPower0ACommand()
        {
            if (m_pUsbRS232Comm == null) return 0;

            // CFE HV POWER ALL DATA MONITORING
            CharBuffer szCommand = new CharBuffer(64);
            CharBuffer szHexCSUM = new CharBuffer(10);
            ushort wCheckSum = 0;

            // 0A, Get Beam Voltage Ramping Rates
            // <:><0A><SP><CSUM><LF>
            memset(szCommand, 0x00, 64);
            memset(szHexCSUM, 0x00, 10);

            // Command
            szCommand[0] = (byte)':';  // START
            szCommand[1] = (byte)'0';  // CMD
            szCommand[2] = (byte)'A';  // CMD
            szCommand[3] = (byte)' ';  // SP
            szCommand[4] = NULL; // CSUM
            szCommand[5] = NULL; // CSUM
            szCommand[6] = 0x0A; // LF
            szCommand[7] = NULL; // NULL

            // CHECK SUM 구하기
            wCheckSum = MakeCfeHVPowerCommandCheckSum(szCommand, (ushort)(3));
            sprintf(szHexCSUM, "%02X", wCheckSum);
            szCommand[4] = szHexCSUM[0]; // CSUM
            szCommand[5] = szHexCSUM[1]; // CSUM

            // 전송
            m_pUsbRS232Comm.Send(szCommand, 7);

            return 1;
        }

        // ---- ModularBeamControlDlg.cpp:7824 ----
        public int SendingCfeHVPower0BCommand(string strText1, string strText2)
        {
            if (m_pUsbRS232Comm == null) return 0;

            // CFE HV POWER ALL DATA MONITORING
            CharBuffer szCommand = new CharBuffer(64);
            CharBuffer szHexCSUM = new CharBuffer(10);
            CharBuffer szValue1 = new CharBuffer(10);
            CharBuffer szValue2 = new CharBuffer(10);
            ushort wCheckSum = 0;

            // Set Value Max 5 Digit : Ramp rate value in the format xxxxx
            // Valid Range : 0V / s to 30000V / s
            int nValue1 = StrToInt(strText1);
            if (nValue1 < 0    ) nValue1 = 0;
            if (nValue1 > 30000) nValue1 = 30000;
            memset(szValue1, 0x00, 10);
            sprintf(szValue1, "%05d", nValue1);
            int nLen1 = strlen(szValue1);
            if (nLen1 < 1 || nLen1 > 5) return 0;

            // Set Value Max 5 Digit : Ramp rate value in the format xxxxx
            // Valid Range : 0V / s to 30000V / s
            int nValue2 = StrToInt(strText2);
            if (nValue2 < 0    ) nValue2 = 0;
            if (nValue2 > 30000) nValue2 = 30000;
            memset(szValue2, 0x00, 10);
            sprintf(szValue2, "%05d", nValue2);
            int nLen2 = strlen(szValue2);
            if (nLen2 < 1 || nLen2 > 5) return 0;

            // 0B, Set Beam Voltage Ramping Rates
            // <:><0B><ARG1><SP><ARG2><SP><CSUM><LF>
            memset(szCommand, 0x00, 64);
            memset(szHexCSUM, 0x00, 10);
            int nLen = 0;

            // Command
            szCommand[0] = (byte)':';  // START
            szCommand[1] = (byte)'0';  // CMD
            szCommand[2] = (byte)'B';  // CMD
            nLen = strlen(szCommand);
            // Value1
            strncpy(szCommand + nLen, szValue1, nLen1);
            nLen = strlen(szCommand);
            szCommand[nLen] = (byte)' ';  // SP
            nLen = strlen(szCommand);
            // Value2
            strncpy(szCommand + nLen, szValue2, nLen2);
            nLen = strlen(szCommand);
            szCommand[nLen] = (byte)' ';  // SP
            // CSUM
            szCommand[nLen + 1] = NULL; // CSUM
            szCommand[nLen + 2] = NULL; // CSUM
            szCommand[nLen + 3] = 0x0A; // LF
            szCommand[nLen + 4] = NULL; // NULL

            // CHECK SUM 구하기
            wCheckSum = MakeCfeHVPowerCommandCheckSum(szCommand, (ushort)(nLen + 1));
            sprintf(szHexCSUM, "%02X", wCheckSum);
            szCommand[nLen + 1] = szHexCSUM[0]; // CSUM
            szCommand[nLen + 2] = szHexCSUM[1]; // CSUM

            // 전송
            nLen = strlen(szCommand);
            m_pUsbRS232Comm.Send(szCommand, nLen);

            return 1;
        }

        // ---- ModularBeamControlDlg.cpp:7895 ----
        public int SendingCfeHVPower0ECommand()
        {
            if (m_pUsbRS232Comm == null) return 0;

            // CFE HV POWER ALL DATA MONITORING
            CharBuffer szCommand = new CharBuffer(64);
            CharBuffer szHexCSUM = new CharBuffer(10);
            ushort wCheckSum = 0;

            // 0E, Get Beam Current
            // <:><0E><SP><CSUM><LF>
            memset(szCommand, 0x00, 64);
            memset(szHexCSUM, 0x00, 10);

            // Command
            szCommand[0] = (byte)':';  // START
            szCommand[1] = (byte)'0';  // CMD
            szCommand[2] = (byte)'E';  // CMD
            szCommand[3] = (byte)' ';  // SP
            szCommand[4] = NULL; // CSUM
            szCommand[5] = NULL; // CSUM
            szCommand[6] = 0x0A; // LF
            szCommand[7] = NULL; // NULL

            // CHECK SUM 구하기
            wCheckSum = MakeCfeHVPowerCommandCheckSum(szCommand, (ushort)(3));
            sprintf(szHexCSUM, "%02X", wCheckSum);
            szCommand[4] = szHexCSUM[0]; // CSUM
            szCommand[5] = szHexCSUM[1]; // CSUM

            // 전송
            m_pUsbRS232Comm.Send(szCommand, 7);

            return 1;
        }

        // ---- ModularBeamControlDlg.cpp:7932 ----
        public int SendingCfeHVPower14Command(int nArg)
        {
            if (m_pUsbRS232Comm == null) return 0;

            // CFE HV POWER ALL DATA MONITORING
            CharBuffer szCommand = new CharBuffer(64);
            CharBuffer szHexCSUM = new CharBuffer(10);
            ushort wCheckSum = 0;

            if (nArg < 0 || nArg > 1) return 0;

            /*
            ARG Command
            0 = Target Value
            1 = Monitored Value
            */

            // 14, Get Extractor Voltage
            // <:><14><ARG><SP><CSUM><LF>
            memset(szCommand, 0x00, 64);
            memset(szHexCSUM, 0x00, 10);

            // Command
            szCommand[0] = (byte)':';  // START
            szCommand[1] = (byte)'1';  // CMD
            szCommand[2] = (byte)'4';  // CMD
            // ARG
            if (nArg == 0)
            {
                szCommand[3] = (byte)'0';  // Target Value
            }
            else if (nArg == 1)
            {
                szCommand[3] = (byte)'1';  // Monitored Value
            }

            szCommand[4] = (byte)' ';  // SP
            szCommand[5] = NULL; // CSUM
            szCommand[6] = NULL; // CSUM
            szCommand[7] = 0x0A; // LF
            szCommand[8] = NULL; // NULL

            // CHECK SUM 구하기
            wCheckSum = MakeCfeHVPowerCommandCheckSum(szCommand, (ushort)(4));
            sprintf(szHexCSUM, "%02X", wCheckSum);
            szCommand[5] = szHexCSUM[0]; // CSUM
            szCommand[6] = szHexCSUM[1]; // CSUM

            // 전송
            m_pUsbRS232Comm.Send(szCommand, 8);

            return 1;
        }

        // ---- ModularBeamControlDlg.cpp:7987 ----
        public int SendingCfeHVPower15Command(string strText)
        {
            if (m_pUsbRS232Comm == null) return 0;

            // CFE HV POWER ALL DATA MONITORING
            CharBuffer szCommand = new CharBuffer(64);
            CharBuffer szHexCSUM = new CharBuffer(10);
            CharBuffer szValue = new CharBuffer(10);
            ushort wCheckSum = 0;

            // Set Value Max 7 Digit : Target value in the format xxxxx.x
            // Valid range : 0V to 10000V
            double dValue = _wtof(strText);
            if (dValue < 0.0    ) dValue = 0.0;
            if (dValue > 10000.0) dValue = 10000.0;
            memset(szValue, 0x00, 10);
            sprintf(szValue, "%07.1f", dValue);

            int nLen = strlen(szValue);
            if (nLen < 3 || nLen > 7) return 0;

            // 15, Set Extractor Voltage
            // <:><15><ARG><SP><CSUM><LF>
            memset(szCommand, 0x00, 64);
            memset(szHexCSUM, 0x00, 10);

            // Command
            szCommand[0] = (byte)':';  // START
            szCommand[1] = (byte)'1';  // CMD
            szCommand[2] = (byte)'5';  // CMD
            // Value
            strncpy(szCommand + 3, szValue, nLen);
            nLen = strlen(szCommand);
            szCommand[nLen] = (byte)' ';  // SP
            // CSUM
            szCommand[nLen + 1] = NULL; // CSUM
            szCommand[nLen + 2] = NULL; // CSUM
            szCommand[nLen + 3] = 0x0A; // LF
            szCommand[nLen + 4] = NULL; // NULL

            // CHECK SUM 구하기
            wCheckSum = MakeCfeHVPowerCommandCheckSum(szCommand, (ushort)(nLen + 1));
            sprintf(szHexCSUM, "%02X", wCheckSum);
            szCommand[nLen + 1] = szHexCSUM[0]; // CSUM
            szCommand[nLen + 2] = szHexCSUM[1]; // CSUM

            // 전송
            nLen = strlen(szCommand);
            m_pUsbRS232Comm.Send(szCommand, nLen);

            return 1;
        }

        // ---- ModularBeamControlDlg.cpp:8041 ----
        public int SendingCfeHVPower16Command()
        {
            if (m_pUsbRS232Comm == null) return 0;

            // CFE HV POWER ALL DATA MONITORING
            CharBuffer szCommand = new CharBuffer(64);
            CharBuffer szHexCSUM = new CharBuffer(10);
            ushort wCheckSum = 0;

            // 16, Get Extractor Voltage Ramping Rates
            // <:><16><SP><CSUM><LF>
            memset(szCommand, 0x00, 64);
            memset(szHexCSUM, 0x00, 10);

            // Command
            szCommand[0] = (byte)':';  // START
            szCommand[1] = (byte)'1';  // CMD
            szCommand[2] = (byte)'6';  // CMD
            szCommand[3] = (byte)' ';  // SP
            szCommand[4] = NULL; // CSUM
            szCommand[5] = NULL; // CSUM
            szCommand[6] = 0x0A; // LF
            szCommand[7] = NULL; // NULL

            // CHECK SUM 구하기
            wCheckSum = MakeCfeHVPowerCommandCheckSum(szCommand, (ushort)(3));
            sprintf(szHexCSUM, "%02X", wCheckSum);
            szCommand[4] = szHexCSUM[0]; // CSUM
            szCommand[5] = szHexCSUM[1]; // CSUM

            // 전송
            m_pUsbRS232Comm.Send(szCommand, 7);

            return 1;
        }

        // ---- ModularBeamControlDlg.cpp:8078 ----
        public int SendingCfeHVPower17Command(string strText1, string strText2)
        {
            if (m_pUsbRS232Comm == null) return 0;

            // CFE HV POWER ALL DATA MONITORING
            CharBuffer szCommand = new CharBuffer(64);
            CharBuffer szHexCSUM = new CharBuffer(10);
            CharBuffer szValue1 = new CharBuffer(10);
            CharBuffer szValue2 = new CharBuffer(10);
            ushort wCheckSum = 0;

            // Set Value Max 5 Digit : Ramp rate value in the format xxxxx
            // Valid Range: 0V/s to 10000V/s
            int nValue1 = StrToInt(strText1);
            if (nValue1 < 0    ) nValue1 = 0;
            if (nValue1 > 10000) nValue1 = 10000;
            memset(szValue1, 0x00, 10);
            sprintf(szValue1, "%05d", nValue1);
            int nLen1 = strlen(szValue1);
            if (nLen1 < 1 || nLen1 > 5) return 0;

            // Set Value Max 5 Digit : Ramp rate value in the format xxxxx
            // Valid Range: 0V/s to 10000V/s
            int nValue2 = StrToInt(strText2);
            if (nValue2 < 0    ) nValue2 = 0;
            if (nValue2 > 10000) nValue2 = 10000;
            memset(szValue2, 0x00, 10);
            sprintf(szValue2, "%05d", nValue2);
            int nLen2 = strlen(szValue2);
            if (nLen2 < 1 || nLen2 > 5) return 0;

            // 17, Set Extractor Voltage Ramping Rates
            // <:><17><ARG1><SP><ARG2><SP><CSUM><LF>
            memset(szCommand, 0x00, 64);
            memset(szHexCSUM, 0x00, 10);
            int nLen = 0;

            // Command
            szCommand[0] = (byte)':';  // START
            szCommand[1] = (byte)'1';  // CMD
            szCommand[2] = (byte)'7';  // CMD
            nLen = strlen(szCommand);
            // Value1
            strncpy(szCommand + nLen, szValue1, nLen1);
            nLen = strlen(szCommand);
            szCommand[nLen] = (byte)' ';  // SP
            nLen = strlen(szCommand);
            // Value2
            strncpy(szCommand + nLen, szValue2, nLen2);
            nLen = strlen(szCommand);
            szCommand[nLen] = (byte)' ';  // SP
            // CSUM
            szCommand[nLen + 1] = NULL; // CSUM
            szCommand[nLen + 2] = NULL; // CSUM
            szCommand[nLen + 3] = 0x0A; // LF
            szCommand[nLen + 4] = NULL; // NULL

            // CHECK SUM 구하기
            wCheckSum = MakeCfeHVPowerCommandCheckSum(szCommand, (ushort)(nLen + 1));
            sprintf(szHexCSUM, "%02X", wCheckSum);
            szCommand[nLen + 1] = szHexCSUM[0]; // CSUM
            szCommand[nLen + 2] = szHexCSUM[1]; // CSUM

            // 전송
            nLen = strlen(szCommand);
            m_pUsbRS232Comm.Send(szCommand, nLen);

            return 1;
        }

        // ---- ModularBeamControlDlg.cpp:8149 ----
        public int SendingCfeHVPower1ACommand()
        {
            if (m_pUsbRS232Comm == null) return 0;

            // CFE HV POWER ALL DATA MONITORING
            CharBuffer szCommand = new CharBuffer(64);
            CharBuffer szHexCSUM = new CharBuffer(10);
            ushort wCheckSum = 0;

            // 1A, Get Extractor Current
            // <:><1A><SP><CSUM><LF>
            memset(szCommand, 0x00, 64);
            memset(szHexCSUM, 0x00, 10);

            // Command
            szCommand[0] = (byte)':';  // START
            szCommand[1] = (byte)'1';  // CMD
            szCommand[2] = (byte)'A';  // CMD
            szCommand[3] = (byte)' ';  // SP
            szCommand[4] = NULL; // CSUM
            szCommand[5] = NULL; // CSUM
            szCommand[6] = 0x0A; // LF
            szCommand[7] = NULL; // NULL

            // CHECK SUM 구하기
            wCheckSum = MakeCfeHVPowerCommandCheckSum(szCommand, (ushort)(3));
            sprintf(szHexCSUM, "%02X", wCheckSum);
            szCommand[4] = szHexCSUM[0]; // CSUM
            szCommand[5] = szHexCSUM[1]; // CSUM

            // 전송
            m_pUsbRS232Comm.Send(szCommand, 7);

            return 1;
        }

        // ---- ModularBeamControlDlg.cpp:8186 ----
        public int SendingCfeHVPower1CCommand()
        {
            if (m_pUsbRS232Comm == null) return 0;

            // CFE HV POWER ALL DATA MONITORING
            CharBuffer szCommand = new CharBuffer(64);
            CharBuffer szHexCSUM = new CharBuffer(10);
            ushort wCheckSum = 0;

            // 1CA, Get Extractor Trip Current
            // <:><1C><SP><CSUM><LF>
            memset(szCommand, 0x00, 64);
            memset(szHexCSUM, 0x00, 10);

            // Command
            szCommand[0] = (byte)':';  // START
            szCommand[1] = (byte)'1';  // CMD
            szCommand[2] = (byte)'C';  // CMD
            szCommand[3] = (byte)' ';  // SP
            szCommand[4] = NULL; // CSUM
            szCommand[5] = NULL; // CSUM
            szCommand[6] = 0x0A; // LF
            szCommand[7] = NULL; // NULL

            // CHECK SUM 구하기
            wCheckSum = MakeCfeHVPowerCommandCheckSum(szCommand, (ushort)(3));
            sprintf(szHexCSUM, "%02X", wCheckSum);
            szCommand[4] = szHexCSUM[0]; // CSUM
            szCommand[5] = szHexCSUM[1]; // CSUM

            // 전송
            m_pUsbRS232Comm.Send(szCommand, 7);

            return 1;
        }

        // ---- ModularBeamControlDlg.cpp:8223 ----
        public int SendingCfeHVPower1DCommand(string strText)
        {
            if (m_pUsbRS232Comm == null) return 0;

            // CFE HV POWER ALL DATA MONITORING
            CharBuffer szCommand = new CharBuffer(64);
            CharBuffer szHexCSUM = new CharBuffer(10);
            CharBuffer szValue = new CharBuffer(10);
            ushort wCheckSum = 0;

            // Set Value Max 3 Digit : Value in the format xxx
            // Valid range: 50μA to 770μA
            int nValue = StrToInt(strText);
            if (nValue < 50 ) nValue = 50;
            if (nValue > 700) nValue = 700;
            memset(szValue, 0x00, 10);
            sprintf(szValue, "%03d", nValue);

            int nLen = strlen(szValue);
            if (nLen < 1 || nLen > 3) return 0;

            // 1D, Set Extractor Trip Current
            // <:><1D><ARG><SP><CSUM><LF>
            memset(szCommand, 0x00, 64);
            memset(szHexCSUM, 0x00, 10);

            // Command
            szCommand[0] = (byte)':';  // START
            szCommand[1] = (byte)'1';  // CMD
            szCommand[2] = (byte)'D';  // CMD
            // Value
            strncpy(szCommand + 3, szValue, nLen);
            nLen = strlen(szCommand);
            szCommand[nLen] = (byte)' ';  // SP
            // CSUM
            szCommand[nLen + 1] = NULL; // CSUM
            szCommand[nLen + 2] = NULL; // CSUM
            szCommand[nLen + 3] = 0x0A; // LF
            szCommand[nLen + 4] = NULL; // NULL

            // CHECK SUM 구하기
            wCheckSum = MakeCfeHVPowerCommandCheckSum(szCommand, (ushort)(nLen + 1));
            sprintf(szHexCSUM, "%02X", wCheckSum);
            szCommand[nLen + 1] = szHexCSUM[0]; // CSUM
            szCommand[nLen + 2] = szHexCSUM[1]; // CSUM

            // 전송
            nLen = strlen(szCommand);
            m_pUsbRS232Comm.Send(szCommand, nLen);

            return 1;
        }

        // ---- ModularBeamControlDlg.cpp:8277 ----
        public int SendingCfeHVPower1ECommand(int nArg)
        {
            if (m_pUsbRS232Comm == null) return 0;

            // CFE HV POWER ALL DATA MONITORING
            CharBuffer szCommand = new CharBuffer(64);
            CharBuffer szHexCSUM = new CharBuffer(10);
            ushort wCheckSum = 0;

            if (nArg < 0 || nArg > 1) return 0;

            /*
            ARG Command
            0 = Target Value
            1 = Monitored Value
            */

            // 1E, Get Suppressor Voltage
            // <:><1E><ARG><SP><CSUM><LF>
            memset(szCommand, 0x00, 64);
            memset(szHexCSUM, 0x00, 10);

            // Command
            szCommand[0] = (byte)':';  // START
            szCommand[1] = (byte)'1';  // CMD
            szCommand[2] = (byte)'E';  // CMD
            // ARG
            if (nArg == 0)
            {
                szCommand[3] = (byte)'0';  // Target Value
            }
            else if (nArg == 1)
            {
                szCommand[3] = (byte)'1';  // Monitored Value
            }

            szCommand[4] = (byte)' ';  // SP
            szCommand[5] = NULL; // CSUM
            szCommand[6] = NULL; // CSUM
            szCommand[7] = 0x0A; // LF
            szCommand[8] = NULL; // NULL

            // CHECK SUM 구하기
            wCheckSum = MakeCfeHVPowerCommandCheckSum(szCommand, (ushort)(4));
            sprintf(szHexCSUM, "%02X", wCheckSum);
            szCommand[5] = szHexCSUM[0]; // CSUM
            szCommand[6] = szHexCSUM[1]; // CSUM

            // 전송
            m_pUsbRS232Comm.Send(szCommand, 8);

            return 1;
        }

        // ---- ModularBeamControlDlg.cpp:8332 ----
        public int SendingCfeHVPower1FCommand(string strText)
        {
            if (m_pUsbRS232Comm == null) return 0;

            // CFE HV POWER ALL DATA MONITORING
            CharBuffer szCommand = new CharBuffer(64);
            CharBuffer szHexCSUM = new CharBuffer(10);
            CharBuffer szValue = new CharBuffer(10);
            ushort wCheckSum = 0;

            // Set Value Max 7 Digit : Target value in the format xxxxx.x
            // Valid range: 0V to 1000V
            double dValue = _wtof(strText);
            if (dValue < 0.0   ) dValue = 0.0;
            if (dValue > 1000.0) dValue = 1000.0;
            memset(szValue, 0x00, 10);
            sprintf(szValue, "%07.1f", dValue);

            int nLen = strlen(szValue);
            if (nLen < 3 || nLen > 7) return 0;

            // 1F, Set Suppressor Voltage
            // <:><1F><ARG><SP><CSUM><LF>
            memset(szCommand, 0x00, 64);
            memset(szHexCSUM, 0x00, 10);

            // Command
            szCommand[0] = (byte)':';  // START
            szCommand[1] = (byte)'1';  // CMD
            szCommand[2] = (byte)'F';  // CMD
            // Value
            strncpy(szCommand + 3, szValue, nLen);
            nLen = strlen(szCommand);
            szCommand[nLen] = (byte)' ';  // SP
            // CSUM
            szCommand[nLen + 1] = NULL; // CSUM
            szCommand[nLen + 2] = NULL; // CSUM
            szCommand[nLen + 3] = 0x0A; // LF
            szCommand[nLen + 4] = NULL; // NULL

            // CHECK SUM 구하기
            wCheckSum = MakeCfeHVPowerCommandCheckSum(szCommand, (ushort)(nLen + 1));
            sprintf(szHexCSUM, "%02X", wCheckSum);
            szCommand[nLen + 1] = szHexCSUM[0]; // CSUM
            szCommand[nLen + 2] = szHexCSUM[1]; // CSUM

            // 전송
            nLen = strlen(szCommand);
            m_pUsbRS232Comm.Send(szCommand, nLen);

            return 1;
        }

        // ---- ModularBeamControlDlg.cpp:8386 ----
        public int SendingCfeHVPower20Command()
        {
            if (m_pUsbRS232Comm == null) return 0;

            // CFE HV POWER ALL DATA MONITORING
            CharBuffer szCommand = new CharBuffer(64);
            CharBuffer szHexCSUM = new CharBuffer(10);
            ushort wCheckSum = 0;

            // 20, Get Suppressor Voltage Ramping Rates
            // <:><20><SP><CSUM><LF>
            memset(szCommand, 0x00, 64);
            memset(szHexCSUM, 0x00, 10);

            // Command
            szCommand[0] = (byte)':';  // START
            szCommand[1] = (byte)'2';  // CMD
            szCommand[2] = (byte)'0';  // CMD
            szCommand[3] = (byte)' ';  // SP
            szCommand[4] = NULL; // CSUM
            szCommand[5] = NULL; // CSUM
            szCommand[6] = 0x0A; // LF
            szCommand[7] = NULL; // NULL

            // CHECK SUM 구하기
            wCheckSum = MakeCfeHVPowerCommandCheckSum(szCommand, (ushort)(3));
            sprintf(szHexCSUM, "%02X", wCheckSum);
            szCommand[4] = szHexCSUM[0]; // CSUM
            szCommand[5] = szHexCSUM[1]; // CSUM

            // 전송
            m_pUsbRS232Comm.Send(szCommand, 7);

            return 1;
        }

        // ---- ModularBeamControlDlg.cpp:8423 ----
        public int SendingCfeHVPower21Command(string strText1, string strText2)
        {
            if (m_pUsbRS232Comm == null) return 0;

            // CFE HV POWER ALL DATA MONITORING
            CharBuffer szCommand = new CharBuffer(64);
            CharBuffer szHexCSUM = new CharBuffer(10);
            CharBuffer szValue1 = new CharBuffer(10);
            CharBuffer szValue2 = new CharBuffer(10);
            ushort wCheckSum = 0;

            // Set Value Max 5 Digit : Ramp rate value in the format xxxx
            // Valid Range: 0V/s to 1000V/s
            int nValue1 = StrToInt(strText1);
            if (nValue1 < 0   ) nValue1 = 0;
            if (nValue1 > 1000) nValue1 = 1000;
            memset(szValue1, 0x00, 10);
            sprintf(szValue1, "%05d", nValue1);
            int nLen1 = strlen(szValue1);
            if (nLen1 < 1 || nLen1 > 5) return 0;

            // Set Value Max 5 Digit : Ramp rate value in the format xxxx
            // Valid Range: 0V/s to 1000V/s
            int nValue2 = StrToInt(strText2);
            if (nValue2 < 0   ) nValue2 = 0;
            if (nValue2 > 1000) nValue2 = 1000;
            memset(szValue2, 0x00, 10);
            sprintf(szValue2, "%05d", nValue2);
            int nLen2 = strlen(szValue2);
            if (nLen2 < 1 || nLen2 > 5) return 0;

            // 21, Set Suppressor Voltage Ramping Rates
            // <:><21><ARG1><SP><ARG2><SP><CSUM><LF>
            memset(szCommand, 0x00, 64);
            memset(szHexCSUM, 0x00, 10);
            int nLen = 0;

            // Command
            szCommand[0] = (byte)':';  // START
            szCommand[1] = (byte)'2';  // CMD
            szCommand[2] = (byte)'1';  // CMD
            nLen = strlen(szCommand);
            // Value1
            strncpy(szCommand + nLen, szValue1, nLen1);
            nLen = strlen(szCommand);
            szCommand[nLen] = (byte)' ';  // SP
            nLen = strlen(szCommand);
            // Value2
            strncpy(szCommand + nLen, szValue2, nLen2);
            nLen = strlen(szCommand);
            szCommand[nLen] = (byte)' ';  // SP
            // CSUM
            szCommand[nLen + 1] = NULL; // CSUM
            szCommand[nLen + 2] = NULL; // CSUM
            szCommand[nLen + 3] = 0x0A; // LF
            szCommand[nLen + 4] = NULL; // NULL

            // CHECK SUM 구하기
            wCheckSum = MakeCfeHVPowerCommandCheckSum(szCommand, (ushort)(nLen + 1));
            sprintf(szHexCSUM, "%02X", wCheckSum);
            szCommand[nLen + 1] = szHexCSUM[0]; // CSUM
            szCommand[nLen + 2] = szHexCSUM[1]; // CSUM

            // 전송
            nLen = strlen(szCommand);
            m_pUsbRS232Comm.Send(szCommand, nLen);

            return 1;
        }

        // ---- ModularBeamControlDlg.cpp:8494 ----
        public int SendingCfeHVPower24Command()
        {
            if (m_pUsbRS232Comm == null) return 0;

            // CFE HV POWER ALL DATA MONITORING
            CharBuffer szCommand = new CharBuffer(64);
            CharBuffer szHexCSUM = new CharBuffer(10);
            ushort wCheckSum = 0;

            // 24, Get Suppressor Current
            // <:><24><SP><CSUM><LF>
            memset(szCommand, 0x00, 64);
            memset(szHexCSUM, 0x00, 10);

            // Command
            szCommand[0] = (byte)':';  // START
            szCommand[1] = (byte)'2';  // CMD
            szCommand[2] = (byte)'4';  // CMD
            szCommand[3] = (byte)' ';  // SP
            szCommand[4] = NULL; // CSUM
            szCommand[5] = NULL; // CSUM
            szCommand[6] = 0x0A; // LF
            szCommand[7] = NULL; // NULL

            // CHECK SUM 구하기
            wCheckSum = MakeCfeHVPowerCommandCheckSum(szCommand, (ushort)(3));
            sprintf(szHexCSUM, "%02X", wCheckSum);
            szCommand[4] = szHexCSUM[0]; // CSUM
            szCommand[5] = szHexCSUM[1]; // CSUM

            // 전송
            m_pUsbRS232Comm.Send(szCommand, 7);

            return 1;
        }

        // ---- ModularBeamControlDlg.cpp:8531 ----
        public int SendingCfeHVPower26Command()
        {
            if (m_pUsbRS232Comm == null) return 0;

            // CFE HV POWER ALL DATA MONITORING
            CharBuffer szCommand = new CharBuffer(64);
            CharBuffer szHexCSUM = new CharBuffer(10);
            ushort wCheckSum = 0;

            // 26, Get Heater Voltage
            // <:><26><SP><CSUM><LF>
            memset(szCommand, 0x00, 64);
            memset(szHexCSUM, 0x00, 10);

            // Command
            szCommand[0] = (byte)':';  // START
            szCommand[1] = (byte)'2';  // CMD
            szCommand[2] = (byte)'6';  // CMD
            szCommand[3] = (byte)' ';  // SP
            szCommand[4] = NULL; // CSUM
            szCommand[5] = NULL; // CSUM
            szCommand[6] = 0x0A; // LF
            szCommand[7] = NULL; // NULL

            // CHECK SUM 구하기
            wCheckSum = MakeCfeHVPowerCommandCheckSum(szCommand, (ushort)(3));
            sprintf(szHexCSUM, "%02X", wCheckSum);
            szCommand[4] = szHexCSUM[0]; // CSUM
            szCommand[5] = szHexCSUM[1]; // CSUM

            // 전송
            m_pUsbRS232Comm.Send(szCommand, 7);

            return 1;
        }

        // ---- ModularBeamControlDlg.cpp:8568 ----
        public int SendingCfeHVPower28Command(int nArg)
        {
            if (m_pUsbRS232Comm == null) return 0;

            // CFE HV POWER ALL DATA MONITORING
            CharBuffer szCommand = new CharBuffer(64);
            CharBuffer szHexCSUM = new CharBuffer(10);
            ushort wCheckSum = 0;

            if (nArg < 0 || nArg > 1) return 0;

            /*
            ARG Command
            0 = Target Value
            1 = Monitored Value
            */

            // 28, Get Heater Current
            // <:><28><ARG><SP><CSUM><LF>
            memset(szCommand, 0x00, 64);
            memset(szHexCSUM, 0x00, 10);

            // Command
            szCommand[0] = (byte)':';  // START
            szCommand[1] = (byte)'2';  // CMD
            szCommand[2] = (byte)'8';  // CMD
            // ARG
            if (nArg == 0)
            {
                szCommand[3] = (byte)'0';  // Target Value
            }
            else if (nArg == 1)
            {
                szCommand[3] = (byte)'1';  // Monitored Value
            }

            szCommand[4] = (byte)' ';  // SP
            szCommand[5] = NULL; // CSUM
            szCommand[6] = NULL; // CSUM
            szCommand[7] = 0x0A; // LF
            szCommand[8] = NULL; // NULL

            // CHECK SUM 구하기
            wCheckSum = MakeCfeHVPowerCommandCheckSum(szCommand, (ushort)(4));
            sprintf(szHexCSUM, "%02X", wCheckSum);
            szCommand[5] = szHexCSUM[0]; // CSUM
            szCommand[6] = szHexCSUM[1]; // CSUM

            // 전송
            m_pUsbRS232Comm.Send(szCommand, 8);

            return 1;
        }

        // ---- ModularBeamControlDlg.cpp:8623 ----
        public int SendingCfeHVPower29Command(string strText)
        {
            if (m_pUsbRS232Comm == null) return 0;

            // CFE HV POWER ALL DATA MONITORING
            CharBuffer szCommand = new CharBuffer(64);
            CharBuffer szHexCSUM = new CharBuffer(10);
            CharBuffer szValue = new CharBuffer(10);
            ushort wCheckSum = 0;

            // Set Value Max 6 Digit : Value in the format xxxx.x
            // Valid range: 0mA to 3000mA
            double dValue = _wtof(strText);
            if (dValue < 0.0   ) dValue = 0.0;
            if (dValue > 3000.0) dValue = 3000.0;
            memset(szValue, 0x00, 10);
            sprintf(szValue, "%06.1f", dValue);

            int nLen = strlen(szValue);
            if (nLen < 3 || nLen > 6) return 0;

            // 29, Set Heater Current
            // <:><29><ARG><SP><CSUM><LF>
            memset(szCommand, 0x00, 64);
            memset(szHexCSUM, 0x00, 10);

            // Command
            szCommand[0] = (byte)':';  // START
            szCommand[1] = (byte)'2';  // CMD
            szCommand[2] = (byte)'9';  // CMD
            // Value
            strncpy(szCommand + 3, szValue, nLen);
            nLen = strlen(szCommand);
            szCommand[nLen] = (byte)' ';  // SP
            // CSUM
            szCommand[nLen + 1] = NULL; // CSUM
            szCommand[nLen + 2] = NULL; // CSUM
            szCommand[nLen + 3] = 0x0A; // LF
            szCommand[nLen + 4] = NULL; // NULL

            // CHECK SUM 구하기
            wCheckSum = MakeCfeHVPowerCommandCheckSum(szCommand, (ushort)(nLen + 1));
            sprintf(szHexCSUM, "%02X", wCheckSum);
            szCommand[nLen + 1] = szHexCSUM[0]; // CSUM
            szCommand[nLen + 2] = szHexCSUM[1]; // CSUM

            // 전송
            nLen = strlen(szCommand);
            m_pUsbRS232Comm.Send(szCommand, nLen);

            return 1;
        }

        // ---- ModularBeamControlDlg.cpp:8677 ----
        public int SendingCfeHVPower2ACommand()
        {
            if (m_pUsbRS232Comm == null) return 0;

            // CFE HV POWER ALL DATA MONITORING
            CharBuffer szCommand = new CharBuffer(64);
            CharBuffer szHexCSUM = new CharBuffer(10);
            ushort wCheckSum = 0;

            // 2A, Get Heater Current Ramping Rates
            // <:><2A><SP><CSUM><LF>
            memset(szCommand, 0x00, 64);
            memset(szHexCSUM, 0x00, 10);

            // Command
            szCommand[0] = (byte)':';  // START
            szCommand[1] = (byte)'2';  // CMD
            szCommand[2] = (byte)'A';  // CMD
            szCommand[3] = (byte)' ';  // SP
            szCommand[4] = NULL; // CSUM
            szCommand[5] = NULL; // CSUM
            szCommand[6] = 0x0A; // LF
            szCommand[7] = NULL; // NULL

            // CHECK SUM 구하기
            wCheckSum = MakeCfeHVPowerCommandCheckSum(szCommand, (ushort)(3));
            sprintf(szHexCSUM, "%02X", wCheckSum);
            szCommand[4] = szHexCSUM[0]; // CSUM
            szCommand[5] = szHexCSUM[1]; // CSUM

            // 전송
            m_pUsbRS232Comm.Send(szCommand, 7);

            return 1;
        }

        // ---- ModularBeamControlDlg.cpp:8714 ----
        public int SendingCfeHVPower2BCommand(string strText1, string strText2)
        {
            if (m_pUsbRS232Comm == null) return 0;

            // CFE HV POWER ALL DATA MONITORING
            CharBuffer szCommand = new CharBuffer(64);
            CharBuffer szHexCSUM = new CharBuffer(10);
            CharBuffer szValue1 = new CharBuffer(10);
            CharBuffer szValue2 = new CharBuffer(10);
            ushort wCheckSum = 0;

            // Set Value Max 4 Digit : Ramp rate value in the format xxxx
            // Valid Range: 0mA/s to 3000mA/s
            int nValue1 = StrToInt(strText1);
            if (nValue1 < 0   ) nValue1 = 0;
            if (nValue1 > 3000) nValue1 = 3000;
            memset(szValue1, 0x00, 10);
            sprintf(szValue1, "%04d", nValue1);
            int nLen1 = strlen(szValue1);
            if (nLen1 < 1 || nLen1 > 4) return 0;

            // Set Value Max 4 Digit : Ramp rate value in the format xxxx
            // Valid Range: 0mA/s to 3000mA/s
            int nValue2 = StrToInt(strText2);
            if (nValue2 < 0   ) nValue2 = 0;
            if (nValue2 > 3000) nValue2 = 3000;
            memset(szValue2, 0x00, 10);
            sprintf(szValue2, "%04d", nValue2);
            int nLen2 = strlen(szValue2);
            if (nLen2 < 1 || nLen2 > 4) return 0;

            // 2B, Set Heater Current Ramping Rates
            // <:><2B><ARG1><SP><ARG2><SP><CSUM><LF>
            memset(szCommand, 0x00, 64);
            memset(szHexCSUM, 0x00, 10);
            int nLen = 0;

            // Command
            szCommand[0] = (byte)':';  // START
            szCommand[1] = (byte)'2';  // CMD
            szCommand[2] = (byte)'B';  // CMD
            nLen = strlen(szCommand);
            // Value1
            strncpy(szCommand + nLen, szValue1, nLen1);
            nLen = strlen(szCommand);
            szCommand[nLen] = (byte)' ';  // SP
            nLen = strlen(szCommand);
            // Value2
            strncpy(szCommand + nLen, szValue2, nLen2);
            nLen = strlen(szCommand);
            szCommand[nLen] = (byte)' ';  // SP
            // CSUM
            szCommand[nLen + 1] = NULL; // CSUM
            szCommand[nLen + 2] = NULL; // CSUM
            szCommand[nLen + 3] = 0x0A; // LF
            szCommand[nLen + 4] = NULL; // NULL

            // CHECK SUM 구하기
            wCheckSum = MakeCfeHVPowerCommandCheckSum(szCommand, (ushort)(nLen + 1));
            sprintf(szHexCSUM, "%02X", wCheckSum);
            szCommand[nLen + 1] = szHexCSUM[0]; // CSUM
            szCommand[nLen + 2] = szHexCSUM[1]; // CSUM

            // 전송
            nLen = strlen(szCommand);
            m_pUsbRS232Comm.Send(szCommand, nLen);

            return 1;
        }

        // ---- ModularBeamControlDlg.cpp:8785 ----
        public int SendingCfeHVPower50Command()
        {
            if (m_pUsbRS232Comm == null) return 0;

            // CFE HV POWER ALL DATA MONITORING
            CharBuffer szCommand = new CharBuffer(64);
            CharBuffer szHexCSUM = new CharBuffer(10);
            ushort wCheckSum = 0;

            // 50, Get Overall Status
            // <:><50><SP><CSUM><LF>
            memset(szCommand, 0x00, 64);
            memset(szHexCSUM, 0x00, 10);

            // Command
            szCommand[0] = (byte)':';  // START
            szCommand[1] = (byte)'5';  // CMD
            szCommand[2] = (byte)'0';  // CMD
            szCommand[3] = (byte)' ';  // SP
            szCommand[4] = NULL; // CSUM
            szCommand[5] = NULL; // CSUM
            szCommand[6] = 0x0A; // LF
            szCommand[7] = NULL; // NULL

            // CHECK SUM 구하기
            wCheckSum = MakeCfeHVPowerCommandCheckSum(szCommand, (ushort)(3));
            sprintf(szHexCSUM, "%02X", wCheckSum);
            szCommand[4] = szHexCSUM[0]; // CSUM
            szCommand[5] = szHexCSUM[1]; // CSUM

            // 전송
            m_pUsbRS232Comm.Send(szCommand, 7);

            return 1;
        }

        // ---- ModularBeamControlDlg.cpp:8822 ----
        public ushort MakeCfeHVPowerCommandCheckSum(ICharArray pszMessage, ushort index_of_last_character)
        {
            // CFE HV POWER COMMAND CHECKSUM
            ushort loop_total = 0;
            ushort checksum   = 0;
            ushort loop_count = 0;

            // start loop at 1, to discount the <:>
            for (loop_count = 1; loop_count <= index_of_last_character; loop_count++)
            {
                loop_total += pszMessage[loop_count];
            }
            checksum  = (ushort)(HV_CHECKSUM_NEGATION - loop_total);
            checksum &= HV_UPPER_CHECKSUM_BOUND;
            checksum |= HV_LOWER_CHECKSUM_BOUND;

            if (checksum < HV_LOWER_CHECKSUM_BOUND || checksum > HV_UPPER_CHECKSUM_BOUND)
            {
                return HV_CHECKSUM_FAIL_VALUE_OUTSIDE_ACCEPTABLE_RANGE;
            }

            return checksum;

        }

        // ---- ModularBeamControlDlg.cpp:8848 ----
        public void SetBeamTargetVoltageValue(string strText)
        {
            // TARGET VALUE 는 1번만 설정한다
            if (m_bSetBeamTargetValueFlag == FALSE)
            {
                // 09, Set Beam Voltage Target
                m_CfeControlBeamEnergyVoltageEdit.SetWindowTextW(strText);
                // SPIN
                double dSpin = _wtof(strText);
                int nSpin = (int)dSpin;
                m_CfeControlBeamEnergyVoltageSpin.SetPos(nSpin);

                m_bSetBeamTargetValueFlag = TRUE;
            }

        }

        // ---- ModularBeamControlDlg.cpp:8866 ----
        public void SetHeaterTargetCurrentValue(string strText)
        {
            // TARGET VALUE 는 1번만 설정한다
            if (m_bSetHeaterTargetValueFlag == FALSE)
            {
                // 29, Set Heater Current Target
                m_CfeControlHeaterCurrentEdit.SetWindowTextW(strText);
                // SPIN
                double dSpin = _wtof(strText);
                int nSpin = (int)dSpin;
                m_CfeControlHeaterCurrentSpin.SetPos(nSpin);

                m_bSetHeaterTargetValueFlag = TRUE;
            }
        }

        // ---- ModularBeamControlDlg.cpp:8883 ----
        public void SetSuppressorTargetVoltageValue(string strText)
        {
            // TARGET VALUE 는 1번만 설정한다
            if (m_bSetSuppressorTargetValueFlag == FALSE)
            {
                // 1F, Set Suppressor Voltage Target
                m_CfeControlSuppressorVoltageEdit.SetWindowTextW(strText);
                // SPIN
                double dSpin = _wtof(strText);
                int nSpin = (int)dSpin;
                m_CfeControlSuppressorVoltageSpin.SetPos(nSpin);

                m_bSetSuppressorTargetValueFlag = TRUE;
            }
        }

        // ---- ModularBeamControlDlg.cpp:8900 ----
        public void SetExtractorTargetVoltageValue(string strText)
        {
            // TARGET VALUE 는 1번만 설정한다
            if (m_bSetExtractorTargetValueFlag == FALSE)
            {
                // 15, Set Extractor Voltage Target
                m_CfeControlExtractorVoltageEdit.SetWindowTextW(strText);
                // SPIN
                double dSpin = _wtof(strText);
                int nSpin = (int)dSpin;
                m_CfeControlExtractorVoltageSpin.SetPos(nSpin);

                m_bSetExtractorTargetValueFlag = TRUE;
            }

        }

        // ---- ModularBeamControlDlg.cpp:8918 ----
        public void SetExtractorTripCurrentValue(string strText)
        {
            // TARGET VALUE 는 1번만 설정한다
            if (m_bSetExtractorTripTargetValueFlag == FALSE)
            {
                // 1C, Set Extractor Trip Current
                m_CfeControlExtractorTripCurrentEdit.SetWindowTextW(strText);
                // SPIN
                double dSpin = _wtof(strText);
                int nSpin = (int)dSpin;
                m_CfeControlExtractorTripCurrentSpin.SetPos(nSpin);

                m_bSetExtractorTripTargetValueFlag = TRUE;
            }
        }

    }
}
