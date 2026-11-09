// 원본 NanoImpact2020cfeDlg.cpp:22540-22612 을 NanoImpact.CodeGen translate로 옮긴 뒤 손으로 고친 코드.
#pragma warning disable IDE1006, IDE0059, CS0162, CS0168, CS0219
using System;
using NanoImpact.Core.Native;
using static NanoImpact.Core.Def;
using static NanoImpact.Core.Native.CRuntime;

namespace NanoImpact.Hardware.Spellman
{
    public sealed partial class CfeHVPowerMonitor
    {
        // ---- NanoImpact2020cfeDlg.cpp:22540 ----
        //
        // CFE HV POWER CONTROL & MONITORING COM PORT CLOSE
        //
        public int OnCfeHVPowerUsbRS232ComPortClose(int wParam, object lParam)
        {
            if (wParam == 1)
            {
                m_pUsbRS232Comm = null;
                m_bUsbRS232ComPortOpen = FALSE;
                m_TopToolFEPowerStatusText.SetWindowTextW("FE HV Port Closed.!!!");
            }
            else
            {
                m_pUsbRS232Comm = null;
                m_bUsbRS232ComPortOpen = FALSE;
                string strErr;
                strErr = Format("FE HV Port Closed[%d].!!!", lParam);
                m_TopToolFEPowerStatusText.SetWindowTextW(strErr);
            }

            return 1;
        }

        // ---- NanoImpact2020cfeDlg.cpp:22563 ----
        //
        // CFE HV POWER CONTROL & MONITORING COM PORT RECEIVE DATA PROCESS
        //
        public int OnCfeHVPowerUsbRS232ComPortReceiveData(int wParam, object lParam)
        {
            if (lParam == null) return 0;
            if (m_pUsbRS232Comm == null) return 0;

            // DATA COUNT
            int nReceiveData = (int)wParam;
            // CLASS
            UsbRS232Comm pUsbRS232Comm = lParam as UsbRS232Comm;
            if (pUsbRS232Comm == null) return 0;

            // COM PORT DATA RECEIVE PROCESS
            CharBuffer szData = new CharBuffer(MAX_RS232_BUFFER);

            // BUFFER CLEAR
            ZeroMem(szData);

            // DATA RECEIVE
            int nCount = pUsbRS232Comm.Receive(szData, nReceiveData);
            // 원본은 Receive가 -1(4096바이트 넘게 쌓임)이거나 4096일 때 szData 밖에 NUL을 썼다
            if (nCount >= 0 && nCount < MAX_RS232_BUFFER) szData[nCount] = NULL;

            // C-FE HV POWER CONTROL & MONITORING DATA
            if (nCount < 1) return 0;

            // CFE HV POWER RECEIVE DATA BUFFER
            int nBuffer = strlen(m_szCfeHVPowerDataBuffer);
            // 원본은 LF 없이 4096바이트 넘게 이어지면 버퍼 뒤 멤버를 덮어썼다 → 모아 둔 조각을 버리고 새로 받은 것부터
            if (nBuffer + nCount > MAX_RS232_BUFFER)
            {
                ZeroMem(m_szCfeHVPowerDataBuffer);
                nBuffer = 0;
            }
            // 기존 Receive Data Buffer에 자료가 남아 있는지 검사한다
            if (nBuffer > 0)
            {
                // 기존 DATA 와 새로 받은 DATA를 합친다
                memcpy(m_szCfeHVPowerDataBuffer + nBuffer, szData, nCount);
                nCount = nCount + nBuffer;
            }
            else
            {
                // 새로운 DATA로 설정한다
                memcpy(m_szCfeHVPowerDataBuffer, szData, nCount);
            }

            // 마지막 문자 LF 검사
            if (m_szCfeHVPowerDataBuffer[nCount - 1] == 0x0A)
            {
                // CFE HV POWER RS232 RECEIVE DATA PROCESS
                CfeHVPowerRS232ReceiveDataProcess(m_szCfeHVPowerDataBuffer, nCount);

                // CLEAR
                ZeroMem(m_szCfeHVPowerDataBuffer);
            }

            return 1;
        }

    }
}
