// 원본 NanoImpact2020cfeDlg.cpp:29831-31999 을 NanoImpact.CodeGen translate로 옮긴 뒤 손으로 고친 코드.
#pragma warning disable IDE1006, IDE0059, CS0162, CS0168, CS0219
using System;
using NanoImpact.Core.Native;
using static NanoImpact.Core.Def;
using static NanoImpact.Core.Native.CRuntime;

namespace NanoImpact.Hardware.Spellman
{
    public sealed partial class CfeHVPowerMonitor
    {
        // ---- NanoImpact2020cfeDlg.cpp:29831 ----
        public void CfeHVPowerRS232ReceiveDataProcess(CharBuffer pszData, int nData)
        {
            if (pszData == null) return;
            if (nData < 1) return;

            string strText = "";

            // RECEIVE DATA
            strText = "";
            strText = Format("%s", ToStr(pszData));
            m_TopToolFEPowerStatusText.SetWindowTextW(strText);

            // LIST CONTROL : ITEM 추가
            // C-FE HV POWER : RECEIVE DATA VIEW 확인
            if (m_wndCfeHVPowerMonitorDlg.m_CfeMonitorReceiveDataViewCheck.GetCheck() == 1)
            {
                // ITEM 추가
                // INDEX
                strText = Format("%d", m_nReceiveDataListItemIndex);
                m_CfeHVPowerReceiveDataListCtrl.InsertItem(0, strText);

                // RECEIVE DATA
                strText = "";
                strText = Format("%s", ToStr(pszData));
                m_CfeHVPowerReceiveDataListCtrl.SetItemText(0, 1, strText);
            }

            // RS232 PORT RECEIVE DATA LIST
            m_nReceiveDataListItemIndex++;

            // COMMAND 분석
            CharBuffer szCommand = new CharBuffer(256);
            int  n = 0;
            int  k = 0;
            int  m = 0;

            // CLEAR
            memset(szCommand, 0x00, 256);

            for (n = 0; n < nData; n++)
            {
                // 콜론 문자 확인 : COMMAND & 관련 DATA 시작
                if (pszData[n] == (byte)':')
                {
                    // CLEAR
                    k = 0;
                    memset(szCommand, 0x00, 256);
                    m++;
                }
                // LF 문자 확인 : COMMAND & 관련 DATA 마지막 문자
                else if (pszData[n] == 0x0A)
                {
                    if (m > 0)
                    {
                        //COMMAND DATA 복사 (원본은 ':' 뒤 256바이트가 넘으면 스택 버퍼 밖에 썼다 → 넘는 바이트는 버린다)
                        if (k < szCommand.Capacity) szCommand[k] = pszData[n];
                        k++;
                        // 마지막 DATA 인지 확인
                        if (n == nData - 1)
                        {
                            // 남은 자료가 없기 때문에 종료
                            break;
                        }
                        else
                        {
                            // COMMAND 분석 PROCESS 실행을 한번도 하지 않았다면 COMMAND 분석 실행
                            CfeHVPowerCommandDataAnalysis(szCommand, k);
                            // CLEAR
                            k = 0;
                            m = 0;
                            memset(szCommand, 0x00, 256);
                        }
                    }
                }
                // COMMAND 및 관련 DATA 복사
                else
                {
                    // 콜론 문자 확인 되었다면 : COMMAND DATA 복사
                    if (m > 0)
                    {
                        if (k < szCommand.Capacity) szCommand[k] = pszData[n];
                        k++;
                    }
                }
            }

            if (m > 0)
            {
                // COMMAND 분석 PROCESS 실행을 한번도 하지 않았다면 COMMAND 분석 실행
                CfeHVPowerCommandDataAnalysis(szCommand, k);
            }
        }

        // ---- NanoImpact2020cfeDlg.cpp:29925 ----
        public int CfeHVPowerCommandDataAnalysis(CharBuffer pszCommand, int nData)
        {
            if (pszCommand == null) return -1;
            if (nData < 2) return -1;

            // COMMAND 분석
            CharBuffer szCommand = new CharBuffer(256);
            CharBuffer szARG1 = new CharBuffer(256);
            CharBuffer szARG2 = new CharBuffer(256);
            CharBuffer szARG3 = new CharBuffer(256);
            CharBuffer szARG4 = new CharBuffer(256);
            CharBuffer szARG5 = new CharBuffer(256);
            int  n = 0;
            int  k = 0;
            int  m = 0;
            int  j = 0;
            int  nCommand = -1;

            // CLEAR
            memset(szCommand, 0x00, 256);
            memset(szARG1   , 0x00, 256);
            memset(szARG2   , 0x00, 256);
            memset(szARG3   , 0x00, 256);
            memset(szARG4   , 0x00, 256);
            memset(szARG5   , 0x00, 256);

            // COMMAND 문자 2BYTE 복사
            szCommand[0] = pszCommand[0];
            szCommand[1] = pszCommand[1];
            k = 2;

            for (n = 0; n < MAX_CFE_HV_POWER_COMMAND; n++)
            {
                // COMMAND 확인
                if (memcmp(szCommand, m_szFEPowerCommand[n], 2) == 0)
                {
                    nCommand = n;
                    break;
                }
            }

            // COMMAND 오류
            if (nCommand < 0)
            {
                return -1;
            }

            // COMMAND : "00" -> Reset Response
            if (nCommand == 0)
            {
                // <:><00><ARG><SP><CSUM><LF>, <ARG> : 8개문자 ( '0' ~ '9', 'A' ~ 'F' )
                // ARG 문자 8 BYTE 복사
                // ARG1
                memcpy(szARG1, pszCommand+k, 8);
                k += 8;

                // CMD 02 PROCESS
                CfeHVPowerCommand02Process(szARG1, 8);

            }
            // COMMAND : "01" -> Return Connection Status
            else if (nCommand == 1)
            {
                // <:><01><ARG1><SP><ARG2><SP><CSUM><LF>
                // ARG1 문자 2 BYTE, ARG2 1 BYTE 복사
                // ARG1
                memcpy(szARG1, pszCommand + k, 2);
                k += 2;
                // <SP>
                k++;
                // ARG2
                memcpy(szARG2, pszCommand + k, 1);
                k++;

                // CMD 01 PROCESS
                CfeHVPowerCommand01Process(szARG1, 2, szARG2, 1);

            }
            // COMMAND : "02" -> Return Short Status
            else if (nCommand == 2)
            {
                // <:><02><ARG><SP><CSUM><LF>
                // ARG 문자 8 BYTE 복사
                // ARG1
                memcpy(szARG1, pszCommand + k, 8);
                k += 8;

                // CMD 02 PROCESS
                CfeHVPowerCommand02Process(szARG1, 8);

            }
            // COMMAND : "05" -> Return Firmware Version
            else if (nCommand == 5)
            {
                // <:><05><ARG1><SP><ARG2><SP><CSUM><LF>
                // ARG1 문자 1 BYTE, ARG2 14 BYTE 복사
                // ARG1
                memcpy(szARG1, pszCommand + k, 1);
                k += 1;
                // <SP>
                k++;
                // ARG2
                memcpy(szARG2, pszCommand + k, 14);
                k += 14;

                // CMD 05 PROCESS
                CfeHVPowerCommand05Process(szARG1, 1, szARG2, 14);

            }
            // COMMAND : "08" -> Return Beam Voltage Monitor
            else if (nCommand == 6)
            {
                // <:><08><ARG1><SP><ARG2><SP><CSUM><LF>
                // ARG1 문자 2 BYTE, ARG2 max 7 BYTE 복사
                // ARG1
                memcpy(szARG1, pszCommand + k, 2);
                k += 2;
                // <SP>
                k++;
                // ARG2
                if (pszCommand[k + 3] == 0x20) m = 3;
                else if (pszCommand[k + 4] == 0x20) m = 4;
                else if (pszCommand[k + 5] == 0x20) m = 5;
                else if (pszCommand[k + 6] == 0x20) m = 6;
                else if (pszCommand[k + 7] == 0x20) m = 7;
                else m = 0;
                memcpy(szARG2, pszCommand + k, m);
                k += m;

                // CMD 08 PROCESS
                CfeHVPowerCommand08Process(szARG1, 2, szARG2, m);

            }
            // COMMAND : "09" -> Return Beam Voltage Target
            else if (nCommand == 7)
            {
                // <:><09><ARG1><SP><ARG2><SP><CSUM><LF>
                // ARG1 문자 2 BYTE, ARG2 max 7 BYTE 복사
                // ARG1
                memcpy(szARG1, pszCommand + k, 2);
                k += 2;
                // <SP>
                k++;
                // ARG2
                if (pszCommand[k + 3] == 0x20) m = 3;
                else if (pszCommand[k + 4] == 0x20) m = 4;
                else if (pszCommand[k + 5] == 0x20) m = 5;
                else if (pszCommand[k + 6] == 0x20) m = 6;
                else if (pszCommand[k + 7] == 0x20) m = 7;
                else m = 0;
                memcpy(szARG2, pszCommand + k, m);
                k += m;

                // CMD 09 PROCESS
                CfeHVPowerCommand09Process(szARG1, 2, szARG2, m);

            }
            // COMMAND : "0A" -> Return Beam Ramping Rate
            else if (nCommand == 8)
            {
                // <:><0A><ARG1><SP><ARG2><SP><CSUM><LF>
                // ARG1 max 5 BYTE, ARG2 max 5 BYTE 복사
                // ARG1
                if (pszCommand[k + 1] == 0x20) m = 1;
                else if (pszCommand[k + 2] == 0x20) m = 2;
                else if (pszCommand[k + 3] == 0x20) m = 3;
                else if (pszCommand[k + 4] == 0x20) m = 4;
                else if (pszCommand[k + 5] == 0x20) m = 5;
                else m = 0;
                memcpy(szARG1, pszCommand + k, m);
                k += m;
                j = m;
                // <SP>
                k++;
                // ARG2
                if (pszCommand[k + 1] == 0x20) m = 1;
                else if (pszCommand[k + 2] == 0x20) m = 2;
                else if (pszCommand[k + 3] == 0x20) m = 3;
                else if (pszCommand[k + 4] == 0x20) m = 4;
                else if (pszCommand[k + 5] == 0x20) m = 5;
                else m = 0;
                memcpy(szARG2, pszCommand + k, m);
                k += m;

                // CMD 0A PROCESS
                CfeHVPowerCommand0AProcess(szARG1, j, szARG2, m);

            }
            // COMMAND : "0E" -> Return Beam Current Monitor
            else if (nCommand == 10)
            {
                // <:><0E><ARG><SP><CSUM><LF>
                // ARG1 7 BYTE 복사
                memcpy(szARG1, pszCommand + k, 7);
                k += 7;

                // CMD 0E PROCESS
                CfeHVPowerCommand0EProcess(szARG1, 7);

            }
            // COMMAND : "10" -> Return Wobble Amplitude
            else if (nCommand == 11)
            {
                // <:><10><ARG1><SP><ARG2><SP><CSUM><LF>
                // ARG1 문자 3 BYTE, ARG2 6 BYTE 복사
                // ARG1
                memcpy(szARG1, pszCommand + k, 3);
                k += 3;
                // <SP>
                k++;
                // ARG2
                memcpy(szARG2, pszCommand + k, 6);
                k += 6;

                // CMD 10 PROCESS
                CfeHVPowerCommand10Process(szARG1, 3, szARG2, 6);

            }
            // COMMAND : "12" -> Return Wobble Period
            else if (nCommand == 13)
            {
                // <:><12><ARG><SP><CSUM><LF>
                // ARG1 4 BYTE 복사
                memcpy(szARG1, pszCommand + k, 4);
                k += 4;

                // CMD 12 PROCESS
                CfeHVPowerCommand12Process(szARG1, 4);

            }
            // COMMAND : "14" -> Return Extractor Voltage Monitor
            else if (nCommand == 15)
            {
                // <:><14><ARG1><SP><ARG2><SP><CSUM><LF>
                // ARG1 문자 2 BYTE, ARG2 max 7 BYTE 복사
                // ARG1
                memcpy(szARG1, pszCommand + k, 2);
                k += 2;
                // <SP>
                k++;
                // ARG2
                if (pszCommand[k + 3] == 0x20) m = 3;
                else if (pszCommand[k + 4] == 0x20) m = 4;
                else if (pszCommand[k + 5] == 0x20) m = 5;
                else if (pszCommand[k + 6] == 0x20) m = 6;
                else if (pszCommand[k + 7] == 0x20) m = 7;
                else m = 0;
                memcpy(szARG2, pszCommand + k, m);
                k += m;

                // CMD 14 PROCESS
                CfeHVPowerCommand14Process(szARG1, 2, szARG2, m);

            }
            // COMMAND : "15" -> Return Extractor Voltage Target
            else if (nCommand == 16)
            {
                // <:><15><ARG1><SP><ARG2><SP><CSUM><LF>
                // ARG1 문자 2 BYTE, ARG2 max 7 BYTE 복사
                // ARG1
                memcpy(szARG1, pszCommand + k, 2);
                k += 2;
                // <SP>
                k++;
                // ARG2
                if (pszCommand[k + 3] == 0x20) m = 3;
                else if (pszCommand[k + 4] == 0x20) m = 4;
                else if (pszCommand[k + 5] == 0x20) m = 5;
                else if (pszCommand[k + 6] == 0x20) m = 6;
                else if (pszCommand[k + 7] == 0x20) m = 7;
                else m = 0;
                memcpy(szARG2, pszCommand + k, m);
                k += m;

                // CMD 15 PROCESS
                CfeHVPowerCommand15Process(szARG1, 2, szARG2, m);

            }
            // COMMAND : "16" -> Return Extractor Voltage Ramping Rates
            else if (nCommand == 17)
            {
                // <:><16><ARG1><SP><ARG2><SP><CSUM><LF>
                // ARG1 max 5 BYTE, ARG2 max 5 BYTE 복사
                // ARG1
                if (pszCommand[k + 1] == 0x20) m = 1;
                else if (pszCommand[k + 2] == 0x20) m = 2;
                else if (pszCommand[k + 3] == 0x20) m = 3;
                else if (pszCommand[k + 4] == 0x20) m = 4;
                else if (pszCommand[k + 5] == 0x20) m = 5;
                else m = 0;
                memcpy(szARG1, pszCommand + k, m);
                k += m;
                j = m;
                // <SP>
                k++;
                // ARG2
                if (pszCommand[k + 1] == 0x20) m = 1;
                else if (pszCommand[k + 2] == 0x20) m = 2;
                else if (pszCommand[k + 3] == 0x20) m = 3;
                else if (pszCommand[k + 4] == 0x20) m = 4;
                else if (pszCommand[k + 5] == 0x20) m = 5;
                else m = 0;
                memcpy(szARG2, pszCommand + k, m);
                k += m;

                // CMD 16 PROCESS
                CfeHVPowerCommand16Process(szARG1, j, szARG2, m);

            }
            // COMMAND : "1A" -> Return Extractor Current Monitor
            else if (nCommand == 19)
            {
                // <:><1A><ARG><SP><CSUM><LF>
                // ARG1 max 7 BYTE 복사
                // ARG1
                if (pszCommand[k + 3] == 0x20) m = 3;
                else if (pszCommand[k + 4] == 0x20) m = 4;
                else if (pszCommand[k + 5] == 0x20) m = 5;
                else if (pszCommand[k + 6] == 0x20) m = 6;
                else if (pszCommand[k + 7] == 0x20) m = 7;
                else m = 0;
                memcpy(szARG1, pszCommand + k, m);
                k += m;

                // CMD 1A PROCESS
                CfeHVPowerCommand1AProcess(szARG1, m);

            }
            // COMMAND : "1C" -> Return Extractor Trip Current
            else if (nCommand == 20)
            {
                // <:><1C><ARG><SP><CSUM><LF>
                // ARG1 max 3 BYTE 복사
                // ARG1
                if (pszCommand[k + 1] == 0x20) m = 1;
                else if (pszCommand[k + 2] == 0x20) m = 2;
                else if (pszCommand[k + 3] == 0x20) m = 3;
                else m = 0;
                memcpy(szARG1, pszCommand + k, m);
                k += m;

                // CMD 1C PROCESS
                CfeHVPowerCommand1CProcess(szARG1, m);

            }
            // COMMAND : "1E" -> Return Suppressor Voltage Monitor
            else if (nCommand == 22)
            {
                // <:><1E><ARG1><SP><ARG2><SP><CSUM><LF>
                // ARG1 문자 2 BYTE, ARG2 max 7 BYTE 복사
                // ARG1
                memcpy(szARG1, pszCommand + k, 2);
                k += 2;
                // <SP>
                k++;
                // ARG2
                if (pszCommand[k + 3] == 0x20) m = 3;
                else if (pszCommand[k + 4] == 0x20) m = 4;
                else if (pszCommand[k + 5] == 0x20) m = 5;
                else if (pszCommand[k + 6] == 0x20) m = 6;
                else if (pszCommand[k + 7] == 0x20) m = 7;
                else m = 0;
                memcpy(szARG2, pszCommand + k, m);
                k += m;

                // CMD 1E PROCESS
                CfeHVPowerCommand1EProcess(szARG1, 2, szARG2, m);

            }
            // COMMAND : "1F" -> Return Suppressor Voltage Target
            else if (nCommand == 23)
            {
                // <:><1F><ARG1><SP><ARG2><SP><CSUM><LF>
                // ARG1 문자 2 BYTE, ARG2 max 7 BYTE 복사
                // ARG1
                memcpy(szARG1, pszCommand + k, 2);
                k += 2;
                // <SP>
                k++;
                // ARG2
                if (pszCommand[k + 3] == 0x20) m = 3;
                else if (pszCommand[k + 4] == 0x20) m = 4;
                else if (pszCommand[k + 5] == 0x20) m = 5;
                else if (pszCommand[k + 6] == 0x20) m = 6;
                else if (pszCommand[k + 7] == 0x20) m = 7;
                else m = 0;
                memcpy(szARG2, pszCommand + k, m);
                k += m;

                // CMD 1F PROCESS
                CfeHVPowerCommand1FProcess(szARG1, 2, szARG2, m);

            }
            // COMMAND : "20" -> Return Suppressor Voltage Ramping Rates
            else if (nCommand == 24)
            {
                // <:><20><ARG1><SP><ARG2><SP><CSUM><LF>
                // ARG1 max 5 BYTE, ARG2 max 5 BYTE 복사
                // ARG1
                if (pszCommand[k + 1] == 0x20) m = 1;
                else if (pszCommand[k + 2] == 0x20) m = 2;
                else if (pszCommand[k + 3] == 0x20) m = 3;
                else if (pszCommand[k + 4] == 0x20) m = 4;
                else if (pszCommand[k + 5] == 0x20) m = 5;
                else m = 0;
                memcpy(szARG1, pszCommand + k, m);
                k += m;
                j = m;
                // <SP>
                k++;
                // ARG2
                if (pszCommand[k + 1] == 0x20) m = 1;
                else if (pszCommand[k + 2] == 0x20) m = 2;
                else if (pszCommand[k + 3] == 0x20) m = 3;
                else if (pszCommand[k + 4] == 0x20) m = 4;
                else if (pszCommand[k + 5] == 0x20) m = 5;
                else m = 0;
                memcpy(szARG2, pszCommand + k, m);
                k += m;

                // CMD 20 PROCESS
                CfeHVPowerCommand20Process(szARG1, j, szARG2, m);

            }
            // COMMAND : "24" -> Return Suppressor Current Monitor
            else if (nCommand == 26)
            {
                // <:><24><ARG><SP><CSUM><LF>
                // ARG1 max 7 BYTE 복사
                // ARG1
                if (pszCommand[k + 3] == 0x20) m = 3;
                else if (pszCommand[k + 4] == 0x20) m = 4;
                else if (pszCommand[k + 5] == 0x20) m = 5;
                else if (pszCommand[k + 6] == 0x20) m = 6;
                else if (pszCommand[k + 7] == 0x20) m = 7;
                else m = 0;
                memcpy(szARG1, pszCommand + k, m);
                k += m;

                // CMD 24 PROCESS
                CfeHVPowerCommand24Process(szARG1, m);

            }
            // COMMAND : "26" -> Return Heater Voltage Monitor
            else if (nCommand == 27)
            {
                // <:><26><ARG><SP><CSUM><LF>
                // ARG1 max 5 BYTE 복사
                // ARG1
                if (pszCommand[k + 1] == 0x20) m = 1;
                else if (pszCommand[k + 2] == 0x20) m = 2;
                else if (pszCommand[k + 3] == 0x20) m = 3;
                else if (pszCommand[k + 4] == 0x20) m = 4;
                else if (pszCommand[k + 5] == 0x20) m = 5;
                else m = 0;
                memcpy(szARG1, pszCommand + k, m);
                k += m;

                // CMD 26 PROCESS
                // "00000" --> "0.000" 으로 변환 시킨다
                szARG1[1] = (byte)'.';
                CfeHVPowerCommand26Process(szARG1, m);

            }
            // COMMAND : "28" -> Return Heater Current Monitor
            else if (nCommand == 28)
            {
                // <:><28><ARG1><SP><ARG2><SP><CSUM><LF>
                // ARG1 문자 2 BYTE, ARG2 max 6 BYTE 복사
                // ARG1
                memcpy(szARG1, pszCommand + k, 2);
                k += 2;
                // <SP>
                k++;
                // ARG2
                if (pszCommand[k + 3] == 0x20) m = 3;
                else if (pszCommand[k + 4] == 0x20) m = 4;
                else if (pszCommand[k + 5] == 0x20) m = 5;
                else if (pszCommand[k + 6] == 0x20) m = 6;
                else m = 0;
                memcpy(szARG2, pszCommand + k, m);
                k += m;

                // CMD 28 PROCESS
                CfeHVPowerCommand28Process(szARG1, 2, szARG2, m);

            }
            // COMMAND : "29" -> Return Heater Current Target
            else if (nCommand == 29)
            {
                // <:><29><ARG1><SP><ARG2><SP><CSUM><LF>
                // ARG1 문자 2 BYTE, ARG2 max 6 BYTE 복사
                // ARG1
                memcpy(szARG1, pszCommand + k, 2);
                k += 2;
                // <SP>
                k++;
                // ARG2
                if (pszCommand[k + 3] == 0x20) m = 3;
                else if (pszCommand[k + 4] == 0x20) m = 4;
                else if (pszCommand[k + 5] == 0x20) m = 5;
                else if (pszCommand[k + 6] == 0x20) m = 6;
                else m = 0;
                memcpy(szARG2, pszCommand + k, m);
                k += m;

                // CMD 29 PROCESS
                CfeHVPowerCommand29Process(szARG1, 2, szARG2, m);

            }
            // COMMAND : "2A" -> Return Heater Current Ramping Rates
            else if (nCommand == 30)
            {
                // <:><2A><ARG1><SP><ARG2><SP><CSUM><LF>
                // ARG1 max 4 BYTE, ARG2 max 4 BYTE 복사
                // ARG1
                if (pszCommand[k + 1] == 0x20) m = 1;
                else if (pszCommand[k + 2] == 0x20) m = 2;
                else if (pszCommand[k + 3] == 0x20) m = 3;
                else if (pszCommand[k + 4] == 0x20) m = 4;
                else m = 0;
                memcpy(szARG1, pszCommand + k, m);
                k += m;
                j = m;
                // <SP>
                k++;
                // ARG2
                if (pszCommand[k + 1] == 0x20) m = 1;
                else if (pszCommand[k + 2] == 0x20) m = 2;
                else if (pszCommand[k + 3] == 0x20) m = 3;
                else if (pszCommand[k + 4] == 0x20) m = 4;
                else m = 0;
                memcpy(szARG2, pszCommand + k, m);
                k += m;

                // CMD 2A PROCESS
                CfeHVPowerCommand2AProcess(szARG1, j, szARG2, m);

            }
            // COMMAND : "50" -> Return Overall Status
            else if (nCommand == 32)
            {
                int n1 = 0;
                int n2 = 0;
                int n3 = 0;
                int n4 = 0;
                int n5 = 0;
                // <:><50><ARG1><SP><ARG2><SP><ARG3><SP><ARG4><SP><ARG5><SP><CSUM><LF>
                // ARG1 max 7 BYTE, ARG2 max 7 BYTE, ARG3 max 7 BYTE, ARG4 max 6 BYTE, ARG5 8 BYTE 복사
                // ARG1
                if (pszCommand[k + 3] == 0x20) n1 = 3;
                else if (pszCommand[k + 4] == 0x20) n1 = 4;
                else if (pszCommand[k + 5] == 0x20) n1 = 5;
                else if (pszCommand[k + 6] == 0x20) n1 = 6;
                else if (pszCommand[k + 7] == 0x20) n1 = 7;
                else m = 0;
                memcpy(szARG1, pszCommand + k, n1);
                k += n1;
                // <SP>
                k++;
                // ARG2
                if (pszCommand[k + 3] == 0x20) n2 = 3;
                else if (pszCommand[k + 4] == 0x20) n2 = 4;
                else if (pszCommand[k + 5] == 0x20) n2 = 5;
                else if (pszCommand[k + 6] == 0x20) n2 = 6;
                else if (pszCommand[k + 7] == 0x20) n2 = 7;
                else n2 = 0;
                memcpy(szARG2, pszCommand + k, n2);
                k += n2;
                // <SP>
                k++;
                // ARG3
                if (pszCommand[k + 3] == 0x20) n3 = 3;
                else if (pszCommand[k + 4] == 0x20) n3 = 4;
                else if (pszCommand[k + 5] == 0x20) n3 = 5;
                else if (pszCommand[k + 6] == 0x20) n3 = 6;
                else if (pszCommand[k + 7] == 0x20) n3 = 7;
                else n3 = 0;
                memcpy(szARG3, pszCommand + k, n3);
                k += n3;
                // <SP>
                k++;
                // ARG4
                if (pszCommand[k + 3] == 0x20) n4 = 3;
                else if (pszCommand[k + 4] == 0x20) n4 = 4;
                else if (pszCommand[k + 5] == 0x20) n4 = 5;
                else if (pszCommand[k + 6] == 0x20) n4 = 6;
                else n4 = 0;
                memcpy(szARG4, pszCommand + k, n4);
                k += n4;
                // <SP>
                k++;
                // ARG5
                n5 = 8;
                memcpy(szARG5, pszCommand + k, n5);
                k += n5;

                // CMD 50 PROCESS
                CfeHVPowerCommand50Process(szARG1, n1, szARG2, n2, szARG3, n3, szARG4, n4, szARG5, n5);
            }
            // COMMAND : "52" -> Return Beam Energy ARC settings
            else if (nCommand == 33)
            {
                // <:><52><ARG1><SP><ARG2><SP><CSUM><LF>
                // ARG1 max 2 BYTE, ARG2 max 2 BYTE 복사
                // ARG1
                if (pszCommand[k + 1] == 0x20) m = 1;
                else if (pszCommand[k + 2] == 0x20) m = 2;
                else m = 0;
                memcpy(szARG1, pszCommand + k, m);
                k += m;
                j = m;
                // <SP>
                k++;
                // ARG2
                if (pszCommand[k + 1] == 0x20) m = 1;
                else if (pszCommand[k + 2] == 0x20) m = 2;
                else m = 0;
                memcpy(szARG2, pszCommand + k, m);
                k += m;

                // CMD 52 PROCESS
                CfeHVPowerCommand52Process(szARG1, j, szARG2, m);
            }

            return nCommand;
        }

        // ---- NanoImpact2020cfeDlg.cpp:30554 ----
        public int CfeHVPowerCommand01Process(CharBuffer pszARG1, int nData1, CharBuffer pszARG2, int nData2)
        {
            if (pszARG1 == null) return -1;
            if (pszARG2 == null) return -1;
            if (nData1 < 1) return -1;
            if (nData2 < 1) return -1;

            // CMD (01)

            // ARG1 분석 2 BYTE : Working Mode
            if (memcmp(pszARG1, "00", 2) == 0)
            {
                // Reset, waiting for connection
                m_wndCfeHVPowerMonitorDlg.SetConnectionStatusMessage("Reset, waiting for connection !");
            }
            else if (memcmp(pszARG1, "04", 2) == 0)
            {
                // Normal Operation Mode
                m_wndCfeHVPowerMonitorDlg.SetConnectionStatusMessage("Normal Operation Mode !");
            }
            else if (memcmp(pszARG1, "99", 2) == 0)
            {
                // Initialized
                m_wndCfeHVPowerMonitorDlg.SetConnectionStatusMessage("Initialized !");
            }

            // ARG2 분석 1 BYTE : Error State Register
            // 원본 (BYTE)x & 0x0N == 1 은 C 우선순위로 x & (0x0N == 1) — bit 0만 보고 나머지는 늘 OK (원본 그대로)
            if ((pszARG2[0] & (0x01 == 1 ? 1 : 0)) != 0)
            {
                // bit 0 : Beam Energy Supply ( Bit =0 -> Ok, Bit =1 -> Error )
                // Error Message
                m_wndCfeHVPowerMonitorDlg.SetBeamEnergyStatusMessage("Beam Energy Supply Error !");
            }
            else
            {
                // bit 0 : Beam Energy Supply ( Bit =0 -> Ok, Bit =1 -> Error )
                // OK Message
                m_wndCfeHVPowerMonitorDlg.SetBeamEnergyStatusMessage("Beam Energy Supply OK !");
            }
            // 원본 (BYTE)x & 0x0N == 1 은 C 우선순위로 x & (0x0N == 1) — bit 0만 보고 나머지는 늘 OK (원본 그대로)
            if ((pszARG2[0] & (0x02 == 1 ? 1 : 0)) != 0)
            {
                // bit 1 : Extractor Supply   ( Bit =0 -> Ok, Bit =1 -> Error )
                // Error Message
                m_wndCfeHVPowerMonitorDlg.SetExtractorSupplyStatusMessage("Extractor Supply Error !");
            }
            else
            {
                // bit 1 : Extractor Supply   ( Bit =0 -> Ok, Bit =1 -> Error )
                // OK Message
                m_wndCfeHVPowerMonitorDlg.SetExtractorSupplyStatusMessage("Extractor Supply OK !");
            }
            // 원본 (BYTE)x & 0x0N == 1 은 C 우선순위로 x & (0x0N == 1) — bit 0만 보고 나머지는 늘 OK (원본 그대로)
            if ((pszARG2[0] & (0x04 == 1 ? 1 : 0)) != 0)
            {
                // bit 2 : Suppressor Supply  ( Bit =0 -> Ok, Bit =1 -> Error )
                // Error Message
                m_wndCfeHVPowerMonitorDlg.SetSuppressorSupplyStatusMessage("Suppressor Supply Error !");
            }
            else
            {
                // bit 2 : Suppressor Supply  ( Bit =0 -> Ok, Bit =1 -> Error )
                // OK Message
                m_wndCfeHVPowerMonitorDlg.SetSuppressorSupplyStatusMessage("Suppressor Supply OK !");
            }
            // 원본 (BYTE)x & 0x0N == 1 은 C 우선순위로 x & (0x0N == 1) — bit 0만 보고 나머지는 늘 OK (원본 그대로)
            if ((pszARG2[0] & (0x08 == 1 ? 1 : 0)) != 0)
            {
                // bit 3 : Heater Supply      ( Bit =0 -> Ok, Bit =1 -> Error )
                // Error Message
                m_wndCfeHVPowerMonitorDlg.SetHeaterSupplyStatusMessage("Heater Error !");
            }
            else
            {
                // bit 3 : Heater Supply      ( Bit =0 -> Ok, Bit =1 -> Error )
                // OK Message
                m_wndCfeHVPowerMonitorDlg.SetHeaterSupplyStatusMessage("Heater Supply OK !");
            }

            return 0;
        }

        // ---- NanoImpact2020cfeDlg.cpp:30634 ----
        public int CfeHVPowerCommand02Process(CharBuffer pszARG1, int nData)
        {
            if (pszARG1 == null) return -1;
            if (nData < 1) return -1;

            // COMMAND 02 DATA 분석 : 8 BYTE
            byte btARG1 = CfeHVPowerCommandArgCharToHex(pszARG1[0]);
            byte btARG2 = CfeHVPowerCommandArgCharToHex(pszARG1[1]);
            byte btARG3 = CfeHVPowerCommandArgCharToHex(pszARG1[2]);
            byte btARG4 = CfeHVPowerCommandArgCharToHex(pszARG1[3]);
            byte btARG5 = CfeHVPowerCommandArgCharToHex(pszARG1[4]);
            byte btARG6 = CfeHVPowerCommandArgCharToHex(pszARG1[5]);
            byte btARG7 = CfeHVPowerCommandArgCharToHex(pszARG1[6]);
            byte btARG8 = CfeHVPowerCommandArgCharToHex(pszARG1[7]);

            //BYTE btDATA1 = (btARG1 << 4) + btARG2;
            //BYTE btDATA2 = (btARG3 << 4) + btARG4;
            //BYTE btDATA3 = (btARG5 << 4) + btARG6;
            //BYTE btDATA4 = (btARG7 << 4) + btARG8;
            byte btDATA1 = (byte)((btARG7 << 4) | btARG8);
            byte btDATA2 = (byte)((btARG5 << 4) | btARG6);
            byte btDATA3 = (byte)((btARG3 << 4) | btARG4);
            byte btDATA4 = (byte)((btARG1 << 4) | btARG2);

            //
            // 1번째 BYTE
            //
            // bit 0 : Beam Energy Supply ( Bit = 0 -> OFF, Bit =1 -> ON )
            // bit 1 : Extractor Supply   ( Bit = 0 -> OFF, Bit =1 -> ON )
            // bit 2 : Suppressor Supply  ( Bit = 0 -> OFF, Bit =1 -> ON )
            // bit 3 : Heater Supply      ( Bit = 0 -> OFF, Bit =1 -> ON )
            // bit 4 : Wobbler            ( Bit = 0 -> OFF, Bit =1 -> ON )

            // bit 0 Check : Beam Energy Supply
            if ((btDATA1 & 0x01) == 0x01)
            {
                // 상태설정 : Beam Energy Supply ( Bit = 0 -> OFF, Bit =1 -> ON )
                // ON
                m_wndBeamControlDlg.SetEnableSupplyBitmapImage(IDX_CFE_HV_ENABLE_SUPPLY_BEAM_ENERGY, 1);
            }
            else
            {
                // 상태설정 : Beam Energy Supply ( Bit = 0 -> OFF, Bit =1 -> ON )
                // OFF
                m_wndBeamControlDlg.SetEnableSupplyBitmapImage(IDX_CFE_HV_ENABLE_SUPPLY_BEAM_ENERGY, 0);
            }

            // bit 1 Check : Extractor Supply
            if ((btDATA1 & 0x02) == 0x02)
            {
                // 상태설정 : Extractor Supply ( Bit = 0 -> OFF, Bit =1 -> ON )
                // ON
                m_wndBeamControlDlg.SetEnableSupplyBitmapImage(IDX_CFE_HV_ENABLE_SUPPLY_EXTRACTOR, 1);
            }
            else
            {
                // 상태설정 : Extractor Supply ( Bit = 0 -> OFF, Bit =1 -> ON )
                // OFF
                m_wndBeamControlDlg.SetEnableSupplyBitmapImage(IDX_CFE_HV_ENABLE_SUPPLY_EXTRACTOR, 0);
            }

            // bit 2 Check : Suppressor Supply
            if ((btDATA1 & 0x04) == 0x04)
            {
                // 상태설정 : Suppressor Supply ( Bit = 0 -> OFF, Bit =1 -> ON )
                // ON
                m_wndBeamControlDlg.SetEnableSupplyBitmapImage(IDX_CFE_HV_ENABLE_SUPPLY_SUPPRESSOR, 1);
            }
            else
            {
                // 상태설정 : Suppressor Supply ( Bit = 0 -> OFF, Bit =1 -> ON )
                // OFF
                m_wndBeamControlDlg.SetEnableSupplyBitmapImage(IDX_CFE_HV_ENABLE_SUPPLY_SUPPRESSOR, 0);
            }

            // bit 3 Check : Heater Supply
            if ((btDATA1 & 0x08) == 0x08)
            {
                // 상태설정 : Heater Supply ( Bit = 0 -> OFF, Bit =1 -> ON )
                // ON
                m_wndBeamControlDlg.SetEnableSupplyBitmapImage(IDX_CFE_HV_ENABLE_SUPPLY_HEATER, 1);
            }
            else
            {
                // 상태설정 : Heater Supply ( Bit = 0 -> OFF, Bit =1 -> ON )
                // OFF
                m_wndBeamControlDlg.SetEnableSupplyBitmapImage(IDX_CFE_HV_ENABLE_SUPPLY_HEATER, 0);
            }

            // bit 4 Check : Wobbler
            if ((btDATA1 & 0x10) == 0x10)
            {
                // 상태설정 : Wobbler ( Bit = 0 -> OFF, Bit =1 -> ON )
                // ON
                m_wndBeamControlDlg.SetEnableSupplyBitmapImage(IDX_CFE_HV_ENABLE_SUPPLY_WOBBLER, 1);
            }
            else
            {
                // 상태설정 : Wobbler ( Bit = 0 -> OFF, Bit =1 -> ON )
                // OFF
                m_wndBeamControlDlg.SetEnableSupplyBitmapImage(IDX_CFE_HV_ENABLE_SUPPLY_WOBBLER, 0);
            }

            //
            // 2번째 BYTE
            //
            // bit 0 : Reset Pending               ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
            // bit 1 :                             ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
            // bit 2 : Interlock open Latched      ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
            // bit 3 :                             ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
            // bit 4 : Input voltage out of range  ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
            // bit 5 : Beam Over Voltage Trip      ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
            // bit 6 : Beam Over Current Trip      ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
            // bit 7 : Extractor Over Current Trip ( Bit = 0 -> OK, Bit =1 -> Condition occurred )

            // bit 0 Check
            if ((btDATA2 & 0x01) == 0x01)
            {
                // 상태설정 : Reset Pending, Startup Connection not ready Set on reset(power - up or command). Cleared with command “Get Connection”(CMD = 01). ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
                // C-FE HV POWER BEAM STATUS : Power On Reset
                // Condition occurred
                m_wndCfeHVPowerMonitorDlg.SetBeamStatusBitmapImage(IDX_CFE_HV_BEAM_STATUS_POWER_ON_RESET, 0); 
            }
            else
            {
                // C-FE HV POWER BEAM STATUS : Power On Reset
                // OK
                m_wndCfeHVPowerMonitorDlg.SetBeamStatusBitmapImage(IDX_CFE_HV_BEAM_STATUS_POWER_ON_RESET, 1); 
            }

            // bit 1 Check

            // bit 2 Check
            if ((btDATA2 & 0x04) == 0x04)
            {
                // 상태설정 : Interlock open Latched.Cleared on Reset command. ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
                // C-FE HV POWER BEAM STATUS : Vacuum Interlock
                // Condition occurred
                m_wndCfeHVPowerMonitorDlg.SetBeamStatusBitmapImage(IDX_CFE_HV_BEAM_STATUS_VACUUM_INTERLOCK, 0);
            }
            else
            {
                // C-FE HV POWER BEAM STATUS : Vacuum Interlock
                // OK
                m_wndCfeHVPowerMonitorDlg.SetBeamStatusBitmapImage(IDX_CFE_HV_BEAM_STATUS_VACUUM_INTERLOCK, 1);
            }

            // bit 3 Check

            // bit 4 Check
            if ((btDATA2 & 0x10) == 0x10)
            {
                // 상태설정 : Input voltage out of range. Cleared when input voltage in range. ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
                // C-FE HV POWER BEAM STATUS : Input Voltage Out Of Range
                // Condition occurred
                m_wndCfeHVPowerMonitorDlg.SetBeamStatusBitmapImage(IDX_CFE_HV_BEAM_STATUS_INPUT_VOLTAGE_OUT_OF_RANGE, 0);
            }
            else
            {
                // C-FE HV POWER BEAM STATUS : Input Voltage Out Of Range
                // OK
                m_wndCfeHVPowerMonitorDlg.SetBeamStatusBitmapImage(IDX_CFE_HV_BEAM_STATUS_INPUT_VOLTAGE_OUT_OF_RANGE, 1);
            }

            // bit 5 Check
            if ((btDATA2 & 0x20) == 0x20)
            {
                // 상태설정 : Beam Over Voltage Trip. Cleared when input voltage in range. ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
                // C-FE HV POWER BEAM STATUS : Beam Over Voltage Trip
                // Condition occurred
                m_wndCfeHVPowerMonitorDlg.SetBeamStatusBitmapImage(IDX_CFE_HV_BEAM_STATUS_BEAM_OVER_VOLTAGE_TRIP, 0);
            }
            else
            {
                // C-FE HV POWER BEAM STATUS : Beam Over Voltage Trip
                // OK
                m_wndCfeHVPowerMonitorDlg.SetBeamStatusBitmapImage(IDX_CFE_HV_BEAM_STATUS_BEAM_OVER_VOLTAGE_TRIP, 1);
            }

            // bit 6 Check
            if ((btDATA2 & 0x40) == 0x40)
            {
                // 상태설정 : Beam Over Current Trip. Cleared when switching this output <ON>. ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
                // C-FE HV POWER BEAM STATUS : Beam Over Current Trip
                // Condition occurred
                m_wndCfeHVPowerMonitorDlg.SetBeamStatusBitmapImage(IDX_CFE_HV_BEAM_STATUS_BEAM_OVER_CURRENT_TRIP, 0);
            }
            else
            {
                // C-FE HV POWER BEAM STATUS : Beam Over Current Trip
                // OK
                m_wndCfeHVPowerMonitorDlg.SetBeamStatusBitmapImage(IDX_CFE_HV_BEAM_STATUS_BEAM_OVER_CURRENT_TRIP, 1);
            }

            // bit 7 Check
            if ((btDATA2 & 0x80) == 0x80)
            {
                // 상태설정 : Extractor Over Current Trip. Cleared when switching this output <ON>. ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
                // C-FE HV POWER BEAM STATUS : Extractor Over Current Trip
                // Condition occurred
                m_wndCfeHVPowerMonitorDlg.SetBeamStatusBitmapImage(IDX_CFE_HV_BEAM_STATUS_EXTRACTOR_OVER_CURRENT_TRIP, 0);
            }
            else
            {
                // C-FE HV POWER BEAM STATUS : Extractor Over Current Trip
                // OK
                m_wndCfeHVPowerMonitorDlg.SetBeamStatusBitmapImage(IDX_CFE_HV_BEAM_STATUS_EXTRACTOR_OVER_CURRENT_TRIP, 1);
            }

            //
            // 3번째 BYTE
            //
            // bit 0 : Heater Open Circuit         ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
            // bit 1 : Heater Current Trip         ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
            // bit 2 : Internal Over Temperature   ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
            // bit 3 : Suppressor Over Current Trip( Bit = 0 -> OK, Bit =1 -> Condition occurred )
            // bit 4 : ARC                         ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
            // bit 5 : Suppressor regulation Trip  ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
            // bit 6 : Heater regulation Trip      ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
            // bit 7 : Extractor regulation Trip   ( Bit = 0 -> OK, Bit =1 -> Condition occurred )

            // bit 0 Check
            if ((btDATA3 & 0x01) == 0x01)
            {
                // 상태설정 : Heater Open Circuit, The Heater is <ON> and the Heater Voltage exceeds 5.2V(Open Circuit) Cleared when Heater switched <ON>. ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
                // C-FE HV POWER BEAM STATUS : Heater Open Circuit
                // Condition occurred
                m_wndCfeHVPowerMonitorDlg.SetBeamStatusBitmapImage(IDX_CFE_HV_BEAM_STATUS_HEATER_OPEN_CIRCUIT, 0);
            }
            else
            {
                // C-FE HV POWER BEAM STATUS : Heater Open Circuit
                // OK
                m_wndCfeHVPowerMonitorDlg.SetBeamStatusBitmapImage(IDX_CFE_HV_BEAM_STATUS_HEATER_OPEN_CIRCUIT, 1);
            }

            // bit 1 Check
            if ((btDATA3 & 0x02) == 0x02)
            {
                // 상태설정 : Heater Current Trip > 3.15A for 10sec. Cleared when Heater switched <ON>. ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
                // C-FE HV POWER BEAM STATUS : Heater Current Trip
                // Condition occurred
                m_wndCfeHVPowerMonitorDlg.SetBeamStatusBitmapImage(IDX_CFE_HV_BEAM_STATUS_HEATER_CURRENT_TRIP, 0);
            }
            else
            {
                // C-FE HV POWER BEAM STATUS : Heater Current Trip
                // OK
                m_wndCfeHVPowerMonitorDlg.SetBeamStatusBitmapImage(IDX_CFE_HV_BEAM_STATUS_HEATER_CURRENT_TRIP, 1);
            }

            // bit 2 Check
            if ((btDATA3 & 0x04) == 0x04)
            {
                // 상태설정 : Internal Over Temperature. ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
                // C-FE HV POWER BEAM STATUS : Over Temperature
                // Condition occurred
                m_wndCfeHVPowerMonitorDlg.SetBeamStatusBitmapImage(IDX_CFE_HV_BEAM_STATUS_OVER_TEMPERATURE, 0);
            }
            else
            {
                // C-FE HV POWER BEAM STATUS : Over Temperature
                // OK
                m_wndCfeHVPowerMonitorDlg.SetBeamStatusBitmapImage(IDX_CFE_HV_BEAM_STATUS_OVER_TEMPERATURE, 1);
            }

            // bit 3 Check
            if ((btDATA3 & 0x08) == 0x08)
            {
                // 상태설정 : Suppressor Over Current Trip. Cleared when this output is switched <ON>. ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
                // C-FE HV POWER BEAM STATUS : Suppressor Over Current Trip
                // Condition occurred
                m_wndCfeHVPowerMonitorDlg.SetBeamStatusBitmapImage(IDX_CFE_HV_BEAM_STATUS_SUPPRESSOR_OVER_CURRENT_TRIP, 0);
            }
            else
            {
                // C-FE HV POWER BEAM STATUS : Suppressor Over Current Trip
                // OK
                m_wndCfeHVPowerMonitorDlg.SetBeamStatusBitmapImage(IDX_CFE_HV_BEAM_STATUS_SUPPRESSOR_OVER_CURRENT_TRIP, 1);
            }

            // bit 4 Check
            if ((btDATA3 & 0x10) == 0x10)
            {
                // 상태설정 : ARC. ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
                // C-FE HV POWER BEAM STATUS : ARC Trip
                // Condition occurred
                m_wndCfeHVPowerMonitorDlg.SetBeamStatusBitmapImage(IDX_CFE_HV_BEAM_STATUS_ARC_TRIP, 0);
            }
            else
            {
                // C-FE HV POWER BEAM STATUS : ARC Trip
                // OK
                m_wndCfeHVPowerMonitorDlg.SetBeamStatusBitmapImage(IDX_CFE_HV_BEAM_STATUS_ARC_TRIP, 1);
            }

            // bit 5 Check
            if ((btDATA3 & 0x20) == 0x20)
            {
                // 상태설정 : Suppressor regulation Trip. Gun shutdown. ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
                // C-FE HV POWER BEAM STATUS : Suppressor Regulation Trip
                // Condition occurred
                m_wndCfeHVPowerMonitorDlg.SetBeamStatusBitmapImage(IDX_CFE_HV_BEAM_STATUS_SUPPRESSOR_REGULATION_TRIP, 0);
            }
            else
            {
                // C-FE HV POWER BEAM STATUS : Suppressor Regulation Trip
                // OK
                m_wndCfeHVPowerMonitorDlg.SetBeamStatusBitmapImage(IDX_CFE_HV_BEAM_STATUS_SUPPRESSOR_REGULATION_TRIP, 1);
            }

            // bit 6 Check
            if ((btDATA3 & 0x40) == 0x40)
            {
                // 상태설정 : Heater regulation Trip. Gun shutdown. ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
                // C-FE HV POWER BEAM STATUS : Heater Regulation Trip
                // Condition occurred
                m_wndCfeHVPowerMonitorDlg.SetBeamStatusBitmapImage(IDX_CFE_HV_BEAM_STATUS_HEATER_REGULATION_TRIP, 0);
            }
            else
            {
                // C-FE HV POWER BEAM STATUS : Heater Regulation Trip
                // OK
                m_wndCfeHVPowerMonitorDlg.SetBeamStatusBitmapImage(IDX_CFE_HV_BEAM_STATUS_HEATER_REGULATION_TRIP, 1);
            }

            // bit 7 Check
            if ((btDATA3 & 0x80) == 0x80)
            {
                // 상태설정 : Extractor regulation Trip. Gun shutdown. ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
                // C-FE HV POWER BEAM STATUS : Extractor Regulation Trip
                // Condition occurred
                m_wndCfeHVPowerMonitorDlg.SetBeamStatusBitmapImage(IDX_CFE_HV_BEAM_STATUS_EXTRACTOR_REGULATION_TRIP, 0);
            }
            else
            {
                // C-FE HV POWER BEAM STATUS : Extractor Regulation Trip
                // OK
                m_wndCfeHVPowerMonitorDlg.SetBeamStatusBitmapImage(IDX_CFE_HV_BEAM_STATUS_EXTRACTOR_REGULATION_TRIP, 1);
            }

            //
            // 4번째 BYTE
            //
            // bit 0 :                             ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
            // bit 1 :                             ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
            // bit 2 :                             ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
            // bit 3 :                             ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
            // bit 4 :                             ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
            // bit 5 :                             ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
            // bit 6 : Ground CheckSum error       ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
            // bit 7 : Ground Rx Error             ( Bit = 0 -> OK, Bit =1 -> Condition occurred )

            // bit 0 Check

            // bit 1 Check

            // bit 2 Check

            // bit 3 Check

            // bit 4 Check

            // bit 5 Check

            // bit 6 Check
            if ((btDATA4 & 0x40) == 0x40)
            {
                // 상태설정 : Ground CheckSum error. ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
                // C-FE HV POWER BEAM STATUS : Rx CRC Error
                // Condition occurred
                m_wndCfeHVPowerMonitorDlg.SetBeamStatusBitmapImage(IDX_CFE_HV_BEAM_STATUS_RX_CRC_ERROR, 0);
            }
            else
            {
                // C-FE HV POWER BEAM STATUS : Rx CRC Error
                // OK
                m_wndCfeHVPowerMonitorDlg.SetBeamStatusBitmapImage(IDX_CFE_HV_BEAM_STATUS_RX_CRC_ERROR, 1);
            }

            // bit 7 Check
            if ((btDATA4 & 0x80) == 0x80)
            {
                // 상태설정 : Ground Rx Error. (Invalid Start Character). ( Bit = 0 -> OK, Bit =1 -> Condition occurred )
                // C-FE HV POWER BEAM STATUS : Rx Error
                // Condition occurred
                m_wndCfeHVPowerMonitorDlg.SetBeamStatusBitmapImage(IDX_CFE_HV_BEAM_STATUS_RX_ERROR, 0);
            }
            else
            {
                // C-FE HV POWER BEAM STATUS : Rx Error
                // OK
                m_wndCfeHVPowerMonitorDlg.SetBeamStatusBitmapImage(IDX_CFE_HV_BEAM_STATUS_RX_ERROR, 1);
            }

            return 0;
        }

        // ---- NanoImpact2020cfeDlg.cpp:31033 ----
        public int CfeHVPowerCommand05Process(CharBuffer pszARG1, int nData1, CharBuffer pszARG2, int nData2)
        {
            if (pszARG1 == null) return -1;
            if (pszARG2 == null) return -1;
            if (nData1 < 1) return -1;
            if (nData2 < 1) return -1;

            // CMD (05)

            // ARG1 분석 1 BYTE 
            if (pszARG1[0] == (byte)'0')
            {
                // ARG2 분석 14 BYTE : Format: Vx.xx ddmmyy (e.g “V0.01 03/06/15”)
                string strVers = ToStr(pszARG2);
                // Main Controller (Gateway)
                m_wndCfeHVPowerMonitorDlg.SetGroundFirmwareVersText(strVers);
            }
            else if (pszARG1[0] == (byte)'1')
            {
                // ARG2 분석 14 BYTE : Format: Vx.xx ddmmyy (e.g “V0.01 03/06/15”)
                string strVers = ToStr(pszARG2);
                // Floating Deck (extractor, suppressor, heater)
                m_wndCfeHVPowerMonitorDlg.SetHVFirmwareVersText(strVers);
            }

            return 0;
        }

        // ---- NanoImpact2020cfeDlg.cpp:31062 ----
        public int CfeHVPowerCommand08Process(CharBuffer pszARG1, int nData1, CharBuffer pszARG2, int nData2)
        {
            if (pszARG1 == null) return -1;
            if (pszARG2 == null) return -1;
            if (nData1 < 1) return -1;
            if (nData2 < 1) return -1;

            // CMD (08)

            // ARG1 분석 2 BYTE : Format hexadecimal 8 bit Output Status register
            // Output Status
            byte btData1 = (byte)pszARG1[0];
            byte btData2 = (byte)pszARG1[1];

            // OUTPUT STATUS : HI BITS ( Main Status ( bit 4 ~ 7 ) )
            m_nCfeHVPowerMainOutputStatus  = CfeHVPowerCommandArgHexToOutputStatus(btData1, 1);
            // OUTPUT STATUS : LO BITS ( Main Status ( bit 0 ~ 3 ) )
            m_nCfeHVPowerErrorOutputStatus = CfeHVPowerCommandArgHexToOutputStatus(btData2, 0);

            // ARG2 분석 7 BYTE : Monitor value in the format xxxxx.x
            // Return Beam Voltage Monitor
            string strText = ToStr(pszARG2);
            m_wndCfeHVPowerMonitorDlg.SetBeamVoltageMonitorText(strText);

            switch (m_nCfeHVPowerMainOutputStatus)
            {
                case CFE_OUTPUT_STATUS_MAIN_SWITCHED_OFF:
                {
                    m_wndCfeHVPowerMonitorDlg.SetBeamEnergyStatusMessage(">SWITCH OFF !");
                    break;
                }
                case CFE_OUTPUT_STATUS_MAIN_SWITCHED_RAMP_DOWN:
                {
                    m_wndCfeHVPowerMonitorDlg.SetBeamEnergyStatusMessage(">SWITCH ON, RAMPING DOWN !");
                    break;
                }
                case CFE_OUTPUT_STATUS_MAIN_SWITCHED_RAMP_UP:
                {
                    m_wndCfeHVPowerMonitorDlg.SetBeamEnergyStatusMessage(">SWITCH ON, RAMPING UP !");
                    break;
                }
                case CFE_OUTPUT_STATUS_MAIN_SWITCHED_TARGET:
                {
                    m_wndCfeHVPowerMonitorDlg.SetBeamEnergyStatusMessage(">SWITCH ON, ON TARGET !");
                    break;
                }
                case CFE_OUTPUT_STATUS_MAIN_NO_STATUS:
                {
                    m_wndCfeHVPowerMonitorDlg.SetBeamEnergyStatusMessage(">NO STATUS !");
                    break;
                }
            }

            // OUTPUT STATUS MESSAGE
            CfeHVPowerCommandOutputMessage();

            return 0;
        }

        // ---- NanoImpact2020cfeDlg.cpp:31122 ----
        public int CfeHVPowerCommand09Process(CharBuffer pszARG1, int nData1, CharBuffer pszARG2, int nData2)
        {
            if (pszARG1 == null) return -1;
            if (pszARG2 == null) return -1;
            if (nData1 < 1) return -1;
            if (nData2 < 1) return -1;

            // CMD (09)

            // ARG1 분석 2 BYTE : Format hexadecimal 8 bit Output Status register
            // Output Status
            byte btData1 = (byte)pszARG1[0];
            byte btData2 = (byte)pszARG1[1];

            // OUTPUT STATUS : HI BITS ( Main Status ( bit 4 ~ 7 ) )
            m_nCfeHVPowerMainOutputStatus  = CfeHVPowerCommandArgHexToOutputStatus(btData1, 1);
            // OUTPUT STATUS : LO BITS ( Main Status ( bit 0 ~ 3 ) )
            m_nCfeHVPowerErrorOutputStatus = CfeHVPowerCommandArgHexToOutputStatus(btData2, 0);

            // ARG2 분석 7 BYTE : Target value in the format xxxxx.x
            // Return Beam Voltage Target
            string strText = ToStr(pszARG2);
            m_wndCfeHVPowerMonitorDlg.SetBeamVoltageTargetText(strText);

            // Set Beam Target Voltage
            m_wndBeamControlDlg.SetBeamTargetVoltageValue(strText);

            switch (m_nCfeHVPowerMainOutputStatus)
            {
                case CFE_OUTPUT_STATUS_MAIN_SWITCHED_OFF:
                {
                    // 2021.08.23 : BEAM RAMPING UP/DOWN & ON TARGET STATE RESET
                    m_wndCfeHVPowerMonitorDlg.ResetBeamVoltageRampingOnTarget();
                    // STATE DISPLAY
                    m_wndCfeHVPowerMonitorDlg.SetBeamEnergyStatusMessage("=SWITCH OFF !");
                    break;
                }
                case CFE_OUTPUT_STATUS_MAIN_SWITCHED_RAMP_DOWN:
                {
                    // 2021.08.23 : BEAM RAMPING DOWN
                    m_wndCfeHVPowerMonitorDlg.SetBeamVoltageRampingUpDown(2);
                    // STATE DISPLAY
                    m_wndCfeHVPowerMonitorDlg.SetBeamEnergyStatusMessage("=SWITCH ON, RAMPING DOWN !");
                    break;
                }
                case CFE_OUTPUT_STATUS_MAIN_SWITCHED_RAMP_UP:
                {
                    // 2021.08.23 : BEAM RAMPING UP
                    m_wndCfeHVPowerMonitorDlg.SetBeamVoltageRampingUpDown(1);
                    // STATE DISPLAY
                    m_wndCfeHVPowerMonitorDlg.SetBeamEnergyStatusMessage("=SWITCH ON, RAMPING UP !");
                    break;
                }
                case CFE_OUTPUT_STATUS_MAIN_SWITCHED_TARGET:
                {
                    // 2021.08.23 : BEAM ON TARGET STATE & DEGAUSSING 실행
                    m_wndCfeHVPowerMonitorDlg.SetBeamVoltageOnTarget(1);
                    // STATE DISPLAY
                    m_wndCfeHVPowerMonitorDlg.SetBeamEnergyStatusMessage("=SWITCH ON, ON TARGET !");
                    break;
                }
                case CFE_OUTPUT_STATUS_MAIN_NO_STATUS:
                {
                    // 2021.08.23 : BEAM RAMPING UP/DOWN & ON TARGET STATE RESET
                    m_wndCfeHVPowerMonitorDlg.ResetBeamVoltageRampingOnTarget();
                    // STATE DISPLAY
                    m_wndCfeHVPowerMonitorDlg.SetBeamEnergyStatusMessage("=NO STATUS !");
                    break;
                }
            }

            // OUTPUT STATUS MESSAGE
            CfeHVPowerCommandOutputMessage();

            return 0;
        }

        // ---- NanoImpact2020cfeDlg.cpp:31200 ----
        public int CfeHVPowerCommand0AProcess(CharBuffer pszARG1, int nData1, CharBuffer pszARG2, int nData2)
        {
            if (pszARG1 == null) return -1;
            if (pszARG2 == null) return -1;
            if (nData1 < 1) return -1;
            if (nData2 < 1) return -1;

            // CMD (0A)

            // ARG1 분석 5 BYTE : Ramp Up Rate. Ramp rate value in the format xxxxx
            // Return Ramp Up Rate
            string strText1 = ToStr(pszARG1);
            m_wndBeamControlDlg.SetReadRampRateUpBeamEnergyText(strText1);

            // ARG2 분석 5 BYTE : Ramp Down Rate. Ramp rate value in the format xxxxx
            // Return Ramp Down Rate
            string strText2 = ToStr(pszARG2);
            m_wndBeamControlDlg.SetReadRampRateDownBeamEnergyText(strText2);

            return 0;
        }

        // ---- NanoImpact2020cfeDlg.cpp:31223 ----
        public int CfeHVPowerCommand0EProcess(CharBuffer pszARG1, int nData1)
        {
            if (pszARG1 == null) return -1;
            if (nData1 < 1) return -1;

            // CMD (0E)

            // ARG1 분석 7 BYTE : Monitor value in the format xxx.xxx
            // Return Beam Current monitor
            string strText = ToStr(pszARG1);
            m_wndCfeHVPowerMonitorDlg.SetBeamCurrentMonitorText(strText);

            return 0;
        }

        // ---- NanoImpact2020cfeDlg.cpp:31239 ----
        public int CfeHVPowerCommand10Process(CharBuffer pszARG1, int nData1, CharBuffer pszARG2, int nData2)
        {
            if (pszARG1 == null) return -1;
            if (pszARG2 == null) return -1;
            if (nData1 < 1) return -1;
            if (nData2 < 1) return -1;

            // CMD (10)

            // WOBBLER 처리 안함

            // ARG1 분석 3 BYTE : Format xxx
            // Wobbler Amplitude 0-100% modulation in steps of 1%
            string strText1 = ToStr(pszARG1);

            // ARG2 분석 5 BYTE : Floating point value in the format xxxx.x
            // Return Wobbler Amplitude
            string strText2 = ToStr(pszARG2);

            return 0;
        }

        // ---- NanoImpact2020cfeDlg.cpp:31262 ----
        public int CfeHVPowerCommand12Process(CharBuffer pszARG1, int nData1)
        {
            if (pszARG1 == null) return -1;
            if (nData1 < 1) return -1;

            // CMD (12)

            // WOBBLER 처리 안함

            // ARG1 분석 4 BYTE : Monitor value in the format xxxx
            // Return Wobbler Period
            string strText = ToStr(pszARG1);

            return 0;
        }

        // ---- NanoImpact2020cfeDlg.cpp:31279 ----
        public int CfeHVPowerCommand14Process(CharBuffer pszARG1, int nData1, CharBuffer pszARG2, int nData2)
        {
            if (pszARG1 == null) return -1;
            if (pszARG2 == null) return -1;
            if (nData1 < 1) return -1;
            if (nData2 < 1) return -1;

            // CMD (14)

            // ARG1 분석 2 BYTE : Format hexadecimal 8 bit Output Status register
            // Output Status
            byte btData1 = (byte)pszARG1[0];
            byte btData2 = (byte)pszARG1[1];

            // OUTPUT STATUS : HI BITS ( Main Status ( bit 4 ~ 7 ) )
            m_nCfeHVPowerMainOutputStatus  = CfeHVPowerCommandArgHexToOutputStatus(btData1, 1);
            // OUTPUT STATUS : LO BITS ( Main Status ( bit 0 ~ 3 ) )
            m_nCfeHVPowerErrorOutputStatus = CfeHVPowerCommandArgHexToOutputStatus(btData2, 0);

            // ARG2 분석 7 BYTE : Monitor value in the format xxxxx.x
            // Return Extractor Voltage Monitor
            string strText = ToStr(pszARG2);
            m_wndCfeHVPowerMonitorDlg.SetExtractorVoltageMonitorText(strText);

            switch (m_nCfeHVPowerMainOutputStatus)
            {
                case CFE_OUTPUT_STATUS_MAIN_SWITCHED_OFF:
                {
                    m_wndCfeHVPowerMonitorDlg.SetExtractorSupplyStatusMessage(">SWITCH OFF !");
                    break;
                }
                case CFE_OUTPUT_STATUS_MAIN_SWITCHED_RAMP_DOWN:
                {
                    m_wndCfeHVPowerMonitorDlg.SetExtractorSupplyStatusMessage(">SWITCH ON, RAMPING DOWN !");
                    break;
                }
                case CFE_OUTPUT_STATUS_MAIN_SWITCHED_RAMP_UP:
                {
                    m_wndCfeHVPowerMonitorDlg.SetExtractorSupplyStatusMessage(">SWITCH ON, RAMPING UP !");
                    break;
                }
                case CFE_OUTPUT_STATUS_MAIN_SWITCHED_TARGET:
                {
                    m_wndCfeHVPowerMonitorDlg.SetExtractorSupplyStatusMessage(">SWITCH ON, ON TARGET !");
                    break;
                }
                case CFE_OUTPUT_STATUS_MAIN_NO_STATUS:
                {
                    m_wndCfeHVPowerMonitorDlg.SetExtractorSupplyStatusMessage(">NO STATUS !");
                    break;
                }
            }

            // OUTPUT STATUS MESSAGE
            CfeHVPowerCommandOutputMessage();

            return 0;
        }

        // ---- NanoImpact2020cfeDlg.cpp:31339 ----
        public int CfeHVPowerCommand15Process(CharBuffer pszARG1, int nData1, CharBuffer pszARG2, int nData2)
        {
            if (pszARG1 == null) return -1;
            if (pszARG2 == null) return -1;
            if (nData1 < 1) return -1;
            if (nData2 < 1) return -1;

            // CMD (15)

            // ARG1 분석 2 BYTE : Format hexadecimal 8 bit Output Status register
            // Output Status
            byte btData1 = (byte)pszARG1[0];
            byte btData2 = (byte)pszARG1[1];

            // OUTPUT STATUS : HI BITS ( Main Status ( bit 4 ~ 7 ) )
            m_nCfeHVPowerMainOutputStatus  = CfeHVPowerCommandArgHexToOutputStatus(btData1, 1);
            // OUTPUT STATUS : LO BITS ( Main Status ( bit 0 ~ 3 ) )
            m_nCfeHVPowerErrorOutputStatus = CfeHVPowerCommandArgHexToOutputStatus(btData2, 0);


            // ARG2 분석 7 BYTE : Target value in the format xxxxx.x
            // Return Extractor Voltage Target
            string strText = ToStr(pszARG2);
            m_wndCfeHVPowerMonitorDlg.SetExtractorVoltageTargetText(strText);

            // Set Extractor Target Voltage
            m_wndBeamControlDlg.SetExtractorTargetVoltageValue(strText);

            switch (m_nCfeHVPowerMainOutputStatus)
            {
                case CFE_OUTPUT_STATUS_MAIN_SWITCHED_OFF:
                {
                    m_wndCfeHVPowerMonitorDlg.SetExtractorSupplyStatusMessage("=SWITCH OFF !");
                    break;
                }
                case CFE_OUTPUT_STATUS_MAIN_SWITCHED_RAMP_DOWN:
                {
                    m_wndCfeHVPowerMonitorDlg.SetExtractorSupplyStatusMessage("=SWITCH ON, RAMPING DOWN !");
                    break;
                }
                case CFE_OUTPUT_STATUS_MAIN_SWITCHED_RAMP_UP:
                {
                    m_wndCfeHVPowerMonitorDlg.SetExtractorSupplyStatusMessage("=SWITCH ON, RAMPING UP !");
                    break;
                }
                case CFE_OUTPUT_STATUS_MAIN_SWITCHED_TARGET:
                {
                    m_wndCfeHVPowerMonitorDlg.SetExtractorSupplyStatusMessage("=SWITCH ON, ON TARGET !");
                    break;
                }
                case CFE_OUTPUT_STATUS_MAIN_NO_STATUS:
                {
                    m_wndCfeHVPowerMonitorDlg.SetExtractorSupplyStatusMessage("=NO STATUS !");
                    break;
                }
            }

            // OUTPUT STATUS MESSAGE
            CfeHVPowerCommandOutputMessage();

            return 0;
        }

        // ---- NanoImpact2020cfeDlg.cpp:31403 ----
        public int CfeHVPowerCommand16Process(CharBuffer pszARG1, int nData1, CharBuffer pszARG2, int nData2)
        {
            if (pszARG1 == null) return -1;
            if (pszARG2 == null) return -1;
            if (nData1 < 1) return -1;
            if (nData2 < 1) return -1;

            // CMD (16)

            // Return Extractor Voltage Ramping Rates

            // ARG1 분석 5 BYTE : Ramp rate value in the format xxxxx
            // Ramp Up Rate
            string strText1 = ToStr(pszARG1);
            m_wndBeamControlDlg.SetReadRampRateUpExtractorVoltageText(strText1);

            // ARG2 분석 5 BYTE : Ramp rate value in the format xxxxx
            // Ramp Down Rate
            string strText2 = ToStr(pszARG2);
            m_wndBeamControlDlg.SetReadRampRateDownExtractorVoltageText(strText2);

            return 0;
        }

        // ---- NanoImpact2020cfeDlg.cpp:31428 ----
        public int CfeHVPowerCommand1AProcess(CharBuffer pszARG1, int nData1)
        {
            if (pszARG1 == null) return -1;
            if (nData1 < 1) return -1;

            // CMD (1A)

            // ARG1 분석 7 BYTE : Monitor value in the format xxxx.xx
            // Return Extractor Current monitor
            string strText = ToStr(pszARG1);
            m_wndCfeHVPowerMonitorDlg.SetExtractorCurrentMonitorText(strText);

            // MAIN WINDOW : EXTRACTOR CURRENT TEXT
            m_TopToolExtractorCurrentText.SetWindowTextW(strText);
            m_strExtratorCurrent = strText;
            return 0;
        }

        // ---- NanoImpact2020cfeDlg.cpp:31447 ----
        public int CfeHVPowerCommand1CProcess(CharBuffer pszARG1, int nData1)
        {
            if (pszARG1 == null) return -1;
            if (nData1 < 1) return -1;

            // CMD (1C)

            // ARG1 분석 3 BYTE : Monitor value in the format xxx
            // Return Extractor Trip Current
            string strText = ToStr(pszARG1);
            m_wndCfeHVPowerMonitorDlg.SetExtractorTripCurrentMonitorText(strText);

            // Set Extractor Trip Current
            m_wndBeamControlDlg.SetExtractorTripCurrentValue(strText);

            return 0;
        }

        // ---- NanoImpact2020cfeDlg.cpp:31466 ----
        public int CfeHVPowerCommand1EProcess(CharBuffer pszARG1, int nData1, CharBuffer pszARG2, int nData2)
        {
            if (pszARG1 == null) return -1;
            if (pszARG2 == null) return -1;
            if (nData1 < 1) return -1;
            if (nData2 < 1) return -1;

            // CMD (1E)

            // ARG1 분석 2 BYTE : Format hexadecimal 8 bit Output Status register
            // Output Status
            byte btData1 = (byte)pszARG1[0];
            byte btData2 = (byte)pszARG1[1];

            // OUTPUT STATUS : HI BITS ( Main Status ( bit 4 ~ 7 ) )
            m_nCfeHVPowerMainOutputStatus  = CfeHVPowerCommandArgHexToOutputStatus(btData1, 1);
            // OUTPUT STATUS : LO BITS ( Main Status ( bit 0 ~ 3 ) )
            m_nCfeHVPowerErrorOutputStatus = CfeHVPowerCommandArgHexToOutputStatus(btData2, 0);


            // ARG2 분석 7 BYTE : Target value in the format xxxxx.x
            // Return Suppressor Voltage Monitor
            string strText = ToStr(pszARG2);
            m_wndCfeHVPowerMonitorDlg.SetSuppressorVoltageMonitorText(strText);

            switch (m_nCfeHVPowerMainOutputStatus)
            {
                case CFE_OUTPUT_STATUS_MAIN_SWITCHED_OFF:
                {
                    m_wndCfeHVPowerMonitorDlg.SetSuppressorSupplyStatusMessage(">SWITCH OFF !");
                    break;
                }
                case CFE_OUTPUT_STATUS_MAIN_SWITCHED_RAMP_DOWN:
                {
                    m_wndCfeHVPowerMonitorDlg.SetSuppressorSupplyStatusMessage(">SWITCH ON, RAMPING DOWN !");
                    break;
                }
                case CFE_OUTPUT_STATUS_MAIN_SWITCHED_RAMP_UP:
                {
                    m_wndCfeHVPowerMonitorDlg.SetSuppressorSupplyStatusMessage(">SWITCH ON, RAMPING UP !");
                    break;
                }
                case CFE_OUTPUT_STATUS_MAIN_SWITCHED_TARGET:
                {
                    m_wndCfeHVPowerMonitorDlg.SetSuppressorSupplyStatusMessage(">SWITCH ON, ON TARGET !");
                    break;
                }
                case CFE_OUTPUT_STATUS_MAIN_NO_STATUS:
                {
                    m_wndCfeHVPowerMonitorDlg.SetSuppressorSupplyStatusMessage(">NO STATUS !");
                    break;
                }
            }

            // OUTPUT STATUS MESSAGE
            CfeHVPowerCommandOutputMessage();

            return 0;
        }

        // ---- NanoImpact2020cfeDlg.cpp:31527 ----
        public int CfeHVPowerCommand1FProcess(CharBuffer pszARG1, int nData1, CharBuffer pszARG2, int nData2)
        {
            if (pszARG1 == null) return -1;
            if (pszARG2 == null) return -1;
            if (nData1 < 1) return -1;
            if (nData2 < 1) return -1;

            // CMD (1F)

            // ARG1 분석 2 BYTE : Format hexadecimal 8 bit Output Status register
            // Output Status
            byte btData1 = (byte)pszARG1[0];
            byte btData2 = (byte)pszARG1[1];

            // OUTPUT STATUS : HI BITS ( Main Status ( bit 4 ~ 7 ) )
            m_nCfeHVPowerMainOutputStatus  = CfeHVPowerCommandArgHexToOutputStatus(btData1, 1);
            // OUTPUT STATUS : LO BITS ( Main Status ( bit 0 ~ 3 ) )
            m_nCfeHVPowerErrorOutputStatus = CfeHVPowerCommandArgHexToOutputStatus(btData2, 0);


            // ARG2 분석 7 BYTE : Target value in the format xxxxx.x
            // Return Suppressor Voltage Target
            string strText = ToStr(pszARG2);
            m_wndCfeHVPowerMonitorDlg.SetSuppressorVoltageTargetText(strText);

            // Set Suppressor Target Voltage
            m_wndBeamControlDlg.SetSuppressorTargetVoltageValue(strText);

            switch (m_nCfeHVPowerMainOutputStatus)
            {
                case CFE_OUTPUT_STATUS_MAIN_SWITCHED_OFF:
                {
                    m_wndCfeHVPowerMonitorDlg.SetSuppressorSupplyStatusMessage("=SWITCH OFF !");
                    break;
                }
                case CFE_OUTPUT_STATUS_MAIN_SWITCHED_RAMP_DOWN:
                {
                    m_wndCfeHVPowerMonitorDlg.SetSuppressorSupplyStatusMessage("=SWITCH ON, RAMPING DOWN !");
                    break;
                }
                case CFE_OUTPUT_STATUS_MAIN_SWITCHED_RAMP_UP:
                {
                    m_wndCfeHVPowerMonitorDlg.SetSuppressorSupplyStatusMessage("=SWITCH ON, RAMPING UP !");
                    break;
                }
                case CFE_OUTPUT_STATUS_MAIN_SWITCHED_TARGET:
                {
                    m_wndCfeHVPowerMonitorDlg.SetSuppressorSupplyStatusMessage("=SWITCH ON, ON TARGET !");
                    break;
                }
                case CFE_OUTPUT_STATUS_MAIN_NO_STATUS:
                {
                    m_wndCfeHVPowerMonitorDlg.SetSuppressorSupplyStatusMessage("=NO STATUS !");
                    break;
                }
            }

            // OUTPUT STATUS MESSAGE
            CfeHVPowerCommandOutputMessage();

            return 0;
        }

        // ---- NanoImpact2020cfeDlg.cpp:31591 ----
        public int CfeHVPowerCommand20Process(CharBuffer pszARG1, int nData1, CharBuffer pszARG2, int nData2)
        {
            if (pszARG1 == null) return -1;
            if (pszARG2 == null) return -1;
            if (nData1 < 1) return -1;
            if (nData2 < 1) return -1;

            // CMD (20)

            // ARG1 분석 5 BYTE : Ramp rate value in the format xxxxx
            // Return Suppressor Voltage Ramping Rates : Ramp Up Rate
            string strText1 = ToStr(pszARG1);
            m_wndBeamControlDlg.SetReadRampRateUpSuppressorVoltText(strText1);

            // ARG2 분석 5 BYTE : Ramp rate value in the format xxxxx
            // Return Suppressor Voltage Ramping Rates : Ramp Down Rate
            string strText2 = ToStr(pszARG2);
            m_wndBeamControlDlg.SetReadRampRateDownSuppressorVoltText(strText2);

            return 0;
        }

        // ---- NanoImpact2020cfeDlg.cpp:31614 ----
        public int CfeHVPowerCommand24Process(CharBuffer pszARG1, int nData1)
        {
            if (pszARG1 == null) return -1;
            if (nData1 < 1) return -1;

            // CMD (24)

            // ARG1 분석 7 BYTE : Monitor value in the format xxx.xxx
            // Return Suppressor Current Monitor
            string strText = ToStr(pszARG1);
            m_wndCfeHVPowerMonitorDlg.SetSuppressorCurrentMonitorText(strText);

            return 0;
        }

        // ---- NanoImpact2020cfeDlg.cpp:31630 ----
        public int CfeHVPowerCommand26Process(CharBuffer pszARG1, int nData1)
        {
            if (pszARG1 == null) return -1;
            if (nData1 < 1) return -1;

            // CMD (26)

            // ARG1 분석 5 BYTE : Monitor value in the format xxxxx
            // Return Heater Voltage Monitor
            string strText = ToStr(pszARG1);
            m_wndCfeHVPowerMonitorDlg.SetHeaterVoltageMonitorText(strText);

            return 0;
        }

        // ---- NanoImpact2020cfeDlg.cpp:31646 ----
        public int CfeHVPowerCommand28Process(CharBuffer pszARG1, int nData1, CharBuffer pszARG2, int nData2)
        {
            if (pszARG1 == null) return -1;
            if (pszARG2 == null) return -1;
            if (nData1 < 1) return -1;
            if (nData2 < 1) return -1;

            // CMD (28)

            // ARG1 분석 2 BYTE : Format hexadecimal 8 bit Output Status register
            // Output Status
            byte btData1 = (byte)pszARG1[0];
            byte btData2 = (byte)pszARG1[1];

            // OUTPUT STATUS : HI BITS ( Main Status ( bit 4 ~ 7 ) )
            m_nCfeHVPowerMainOutputStatus  = CfeHVPowerCommandArgHexToOutputStatus(btData1, 1);
            // OUTPUT STATUS : LO BITS ( Main Status ( bit 0 ~ 3 ) )
            m_nCfeHVPowerErrorOutputStatus = CfeHVPowerCommandArgHexToOutputStatus(btData2, 0);

            // ARG2 분석 6 BYTE : Target value in the format xxxx.x
            // Return Heater Current Monitor
            string strText = ToStr(pszARG2);
            m_wndCfeHVPowerMonitorDlg.SetHeaterCurrentMonitorText(strText);

            switch (m_nCfeHVPowerMainOutputStatus)
            {
                case CFE_OUTPUT_STATUS_MAIN_SWITCHED_OFF:
                {
                    m_wndCfeHVPowerMonitorDlg.SetHeaterSupplyStatusMessage(">SWITCH OFF !");
                    break;
                }
                case CFE_OUTPUT_STATUS_MAIN_SWITCHED_RAMP_DOWN:
                {
                    m_wndCfeHVPowerMonitorDlg.SetHeaterSupplyStatusMessage(">SWITCH ON, RAMPING DOWN !");
                    break;
                }
                case CFE_OUTPUT_STATUS_MAIN_SWITCHED_RAMP_UP:
                {
                    m_wndCfeHVPowerMonitorDlg.SetHeaterSupplyStatusMessage(">SWITCH ON, RAMPING UP !");
                    break;
                }
                case CFE_OUTPUT_STATUS_MAIN_SWITCHED_TARGET:
                {
                    m_wndCfeHVPowerMonitorDlg.SetHeaterSupplyStatusMessage(">SWITCH ON, ON TARGET !");
                    break;
                }
                case CFE_OUTPUT_STATUS_MAIN_NO_STATUS:
                {
                    m_wndCfeHVPowerMonitorDlg.SetHeaterSupplyStatusMessage(">NO STATUS !");
                    break;
                }
            }

            // OUTPUT STATUS MESSAGE
            CfeHVPowerCommandOutputMessage();

            return 0;
        }

        // ---- NanoImpact2020cfeDlg.cpp:31706 ----
        public int CfeHVPowerCommand29Process(CharBuffer pszARG1, int nData1, CharBuffer pszARG2, int nData2)
        {
            if (pszARG1 == null) return -1;
            if (pszARG2 == null) return -1;
            if (nData1 < 1) return -1;
            if (nData2 < 1) return -1;

            // CMD (29)

            // ARG1 분석 2 BYTE : Format hexadecimal 8 bit Output Status register
            // Output Status
            byte btData1 = (byte)pszARG1[0];
            byte btData2 = (byte)pszARG1[1];

            // OUTPUT STATUS : HI BITS ( Main Status ( bit 4 ~ 7 ) )
            m_nCfeHVPowerMainOutputStatus  = CfeHVPowerCommandArgHexToOutputStatus(btData1, 1);
            // OUTPUT STATUS : LO BITS ( Main Status ( bit 0 ~ 3 ) )
            m_nCfeHVPowerErrorOutputStatus = CfeHVPowerCommandArgHexToOutputStatus(btData2, 0);

            // ARG2 분석 6 BYTE : Target value in the format xxxx.x
            // Return Heater Current Target
            string strText = ToStr(pszARG2);
            m_wndCfeHVPowerMonitorDlg.SetHeaterCurrentTargetText(strText);

            // Set Heater Target Current
            m_wndBeamControlDlg.SetHeaterTargetCurrentValue(strText);

            switch (m_nCfeHVPowerMainOutputStatus)
            {
                case CFE_OUTPUT_STATUS_MAIN_SWITCHED_OFF:
                {
                    m_wndCfeHVPowerMonitorDlg.SetHeaterSupplyStatusMessage("=SWITCH OFF !");
                    break;
                }
                case CFE_OUTPUT_STATUS_MAIN_SWITCHED_RAMP_DOWN:
                {
                    m_wndCfeHVPowerMonitorDlg.SetHeaterSupplyStatusMessage("=SWITCH ON, RAMPING DOWN !");
                    break;
                }
                case CFE_OUTPUT_STATUS_MAIN_SWITCHED_RAMP_UP:
                {
                    m_wndCfeHVPowerMonitorDlg.SetHeaterSupplyStatusMessage("=SWITCH ON, RAMPING UP !");
                    break;
                }
                case CFE_OUTPUT_STATUS_MAIN_SWITCHED_TARGET:
                {
                    m_wndCfeHVPowerMonitorDlg.SetHeaterSupplyStatusMessage("=SWITCH ON, ON TARGET !");
                    break;
                }
                case CFE_OUTPUT_STATUS_MAIN_NO_STATUS:
                {
                    m_wndCfeHVPowerMonitorDlg.SetHeaterSupplyStatusMessage("=NO STATUS !");
                    break;
                }
            }

            // OUTPUT STATUS MESSAGE
            CfeHVPowerCommandOutputMessage();

            return 0;
        }

        // ---- NanoImpact2020cfeDlg.cpp:31769 ----
        public int CfeHVPowerCommand2AProcess(CharBuffer pszARG1, int nData1, CharBuffer pszARG2, int nData2)
        {
            if (pszARG1 == null) return -1;
            if (pszARG2 == null) return -1;
            if (nData1 < 1) return -1;
            if (nData2 < 1) return -1;

            // CMD (2A)

            // ARG1 분석 4 BYTE : Ramp rate value in the format xxxx
            // Return Heater Current Ramping Rates : Ramp Up Rate
            string strText1 = ToStr(pszARG1);
            m_wndBeamControlDlg.SetReadRampRateUpHeaterCurrentText(strText1);

            // ARG2 분석 4 BYTE : Ramp rate value in the format xxxx
            // Return Heater Current Ramping Rates : Ramp Down Rate
            string strText2 = ToStr(pszARG2);
            m_wndBeamControlDlg.SetReadRampRateDownHeaterCurrentText(strText2);


            return 0;
        }

        // ---- NanoImpact2020cfeDlg.cpp:31793 ----
        public int CfeHVPowerCommand50Process(CharBuffer pszARG1, int nData1, CharBuffer pszARG2, int nData2, CharBuffer pszARG3, int nData3, CharBuffer pszARG4, int nData4, CharBuffer pszARG5, int nData5)
        {
            if (pszARG1 == null) return -1;
            if (pszARG2 == null) return -1;
            if (pszARG3 == null) return -1;
            if (pszARG4 == null) return -1;
            if (pszARG5 == null) return -1;
            if (nData1 < 1) return -1;
            if (nData2 < 1) return -1;
            if (nData3 < 1) return -1;
            if (nData4 < 1) return -1;
            if (nData5 < 1) return -1;

            // CMD (50)

            // ARG1 분석 7 BYTE : Beam Voltage Monitor format xxxxx.x
            // Beam Voltage Monitor
            string strText1 = ToStr(pszARG1);
            m_wndCfeHVPowerMonitorDlg.SetBeamVoltageMonitorText(strText1);

            // ARG2 분석 7 BYTE : Extractor Voltage Monitor format xxxxx.x
            // Extractor Voltage Monitor
            string strText2 = ToStr(pszARG2);
            m_wndCfeHVPowerMonitorDlg.SetExtractorVoltageMonitorText(strText2);

            // ARG3 분석 7 BYTE : Suppressor Voltage Monitor format xxxxx.x
            // Suppressor Voltage Monitor
            string strText3 = ToStr(pszARG3);
            m_wndCfeHVPowerMonitorDlg.SetSuppressorVoltageMonitorText(strText3);

            // ARG4 분석 6 BYTE : Heater Current Monitor format xxxx.x
            // Heater Current Monitor
            string strText4 = ToStr(pszARG4);
            m_wndCfeHVPowerMonitorDlg.SetHeaterCurrentMonitorText(strText4);

            // ARG5 분석 8 BYTE : Short Status. 8 ASCII characters representing the Hexadecimal 32 bit register.
            // Short Status
            CfeHVPowerCommand02Process(pszARG5, nData5);

            return 0;
        }

        // ---- NanoImpact2020cfeDlg.cpp:31836 ----
        public int CfeHVPowerCommand52Process(CharBuffer pszARG1, int nData1, CharBuffer pszARG2, int nData2)
        {
            if (pszARG1 == null) return -1;
            if (pszARG2 == null) return -1;
            if (nData1 < 1) return -1;
            if (nData2 < 1) return -1;

            // CMD (52)

            // 처리 안함

            // ARG1 분석 2 BYTE : Number of arcs format xx
            // Number of arcs
            string strText1 = ToStr(pszARG1);

            // ARG2 분석 2 BYTE : Second period format xx
            // Second period
            string strText2 = ToStr(pszARG2);

            return 0;
        }

        // ---- NanoImpact2020cfeDlg.cpp:31859 ----
        public int CfeHVPowerCommandOutputMessage()
        {
            string strOutputStatus1 = "";
            string strOutputStatus2 = "";
            string strOutputStatus3 = "";

            // MAIN STATUS
            if (m_nCfeHVPowerMainOutputStatus == CFE_OUTPUT_STATUS_MAIN_SWITCHED_OFF)
            {
                strOutputStatus1 = "Switched off(no errors)";
            }
            else if (m_nCfeHVPowerMainOutputStatus == CFE_OUTPUT_STATUS_MAIN_SWITCHED_RAMP_DOWN)
            {
                strOutputStatus1 = "Switched on and ramping down";
            }
            else if (m_nCfeHVPowerMainOutputStatus == CFE_OUTPUT_STATUS_MAIN_SWITCHED_RAMP_UP)
            {
                strOutputStatus1 = "Switched on and ramping up";
            }
            else if (m_nCfeHVPowerMainOutputStatus == CFE_OUTPUT_STATUS_MAIN_SWITCHED_TARGET)
            {
                strOutputStatus1 = "Switched on and <on target>";
            }
            else
            {
                strOutputStatus1 = "No errors";
            }

            // ERROR CODE
            if (m_nCfeHVPowerErrorOutputStatus == CFE_OUTPUT_STATUS_INPUT_VOLTAGE_OUT_OF_RANGE)
            {
                strOutputStatus2 = "Shutdown caused by input volage out of range";
            }
            else if (m_nCfeHVPowerErrorOutputStatus == CFE_OUTPUT_STATUS_SHUTDOWN_BROKEN_INTERLOCK)
            {
                strOutputStatus2 = "Shutdown caused by broken interlock";
            }
            else if (m_nCfeHVPowerErrorOutputStatus == CFE_OUTPUT_STATUS_TRIP_CIRCUIT_ARC_CURR_VOLT)
            {
                strOutputStatus2 = "Shutdown caused by broken interlock";
            }
            else if (m_nCfeHVPowerErrorOutputStatus == CFE_OUTPUT_STATUS_INTERNAL_OVER_TEMPERATURE)
            {
                strOutputStatus2 = "Shutdown caused by broken interlock";
            }
            else if (m_nCfeHVPowerErrorOutputStatus == CFE_OUTPUT_STATUS_REGULATION_ERROR_OUTPUT)
            {
                strOutputStatus2 = "Shutdown caused by broken interlock";
            }
            else if (m_nCfeHVPowerErrorOutputStatus == CFE_OUTPUT_STATUS_SHUTDOWN_ANOTHER_OUTPUT)
            {
                strOutputStatus2 = "Shutdown caused by broken interlock";
            }
            else
            {
                strOutputStatus2 = "No error code";
            }

            // OUTPUT MESSAGE
            strOutputStatus3 = strOutputStatus1 + " / " + strOutputStatus2;

            // OUTPUT STATUS MESSAGE
            m_wndCfeHVPowerMonitorDlg.SetOutputStatusMessage(strOutputStatus3);

            return 0;
        }

        // ---- NanoImpact2020cfeDlg.cpp:31927 ----
        public int CfeHVPowerCommandArgHexToOutputStatus(byte btData, int nHiLow)
        {
            byte btHIByte = (byte)(btData >> 4);
            byte btLOByte = (byte)(btData & 0x0F);

            // HEX 값을 Output Status 값으로 전환

            // HI BYTE -> Output Status
            // Main Status ( bit 4 ~ 7 )

            // SWITCHED OFF
            if (btHIByte == 0x03)
            {
                // LO BYTE -> Output Status
                // Error codes ( bit 0 ~ 3 )

                // No Error
                if (btLOByte == 0x06)
                {
                    return CFE_OUTPUT_STATUS_MAIN_SWITCHED_OFF;
                }
                return CFE_OUTPUT_STATUS_MAIN_NO_STATUS;
            }
            // SWITCHED ON
            else if (btHIByte == 0x04)
            {
                // LO BYTE -> Output Status
                // Error codes ( bit 0 ~ 3 )

                // 0xE : Switched on and <on target>
                if (btLOByte == 0x05)
                {
                    return CFE_OUTPUT_STATUS_MAIN_SWITCHED_TARGET;
                }
                // Switched on and ramping up
                else if (btLOByte == 0x03)
                {
                    return CFE_OUTPUT_STATUS_MAIN_SWITCHED_RAMP_UP;
                }
                // 0xA : Switched on and ramping down
                else if (btLOByte == 0x01)
                {
                    return CFE_OUTPUT_STATUS_MAIN_SWITCHED_RAMP_DOWN;
                }
                return CFE_OUTPUT_STATUS_MAIN_NO_STATUS;
            }
            // No Status
            else
            {
                return CFE_OUTPUT_STATUS_MAIN_NO_STATUS;
            }

            return 0;
        }

        // ---- NanoImpact2020cfeDlg.cpp:31983 ----
        public byte CfeHVPowerCommandArgCharToHex(byte szARG)
        {
            // HEX 문자를 HEX 값으로 변환
            byte btRET = 0x00;

            // '0' ~ '9'
            if (szARG >= (byte)'0' && szARG <= (byte)'9')
            {
                btRET = (byte)(szARG - 0x30); // '?' - '0'
            }
            else if (szARG >= (byte)'A' && szARG <= (byte)'F')
            {
                btRET = (byte)((byte)(szARG - 0x41) + 10); // 0x41; // '?' - 'A'
            }

            return btRET;
        }

    }
}
