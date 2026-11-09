// 원본 USB_FT232H_Device.cpp의 송신 패킷·수신 파서를 거의 그대로 옮긴 기준 구현.
// FT_Write 대신 바이트를 hex로 출력한다. C# NanoImpact.Verify와 같은 vectors.txt를 넣고 출력이 같아야 한다.
// 빌드: tools\golden\build.bat (MSVC x86)
#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
typedef unsigned short WORD;
typedef unsigned char BYTE;

static void PrintHex(const char* tag, const void* p, int n)
{
    const BYTE* b = (const BYTE*)p;
    printf("%s", tag);
    for (int i = 0; i < n; i++) printf(" %02X", b[i]);
    printf("\n");
}

// ---- AfxFT232HPortSendCommand (원본 그대로) ----
static void SendCommand(WORD wCommand, int nPara)
{
    WORD wTemp = 0;
    WORD wComBuf[4] = { 0,0,0,0 };
    int nNum = 4;
    BYTE* pData = (BYTE*)wComBuf;
    wTemp = nPara >> 16;
    wComBuf[0] = (wTemp << 8) | wCommand;
    wComBuf[1] = (WORD)nPara;
    PrintHex("CMD", pData, nNum);
}

// ---- AfxFT232HPortSendAvrCommand(port, cmd, val) (원본 그대로) ----
static void SendAvrVal(char head, const char* pszCommand, int nVal)
{
    char szData[255 + 1];
    char szVal[255 + 1];
    char* pszData = NULL;
    int nCheckSum = 0;
    int nNum = 0;
    memset(szData, 0x00, sizeof(szData));
    memset(szVal, 0x00, sizeof(szVal));
    szData[0] = head;
    szData[1] = (char)0x30;
    szData[2] = (char)0x30;
    szData[3] = (char)0x30;
    pszData = szData + 4;
    strcat(pszData, pszCommand);
    _itoa(nVal, szVal, 10);
    nNum = strlen(szVal);
    strcat(szData, szVal);
    while (*pszData != NULL) { nCheckSum += *pszData; pszData++; }
    *pszData = '?';
    memset(szVal, 0x00, sizeof(szVal));
    nCheckSum = nCheckSum % 100;
    _itoa(nCheckSum, szVal, 10);
    strcat(szData, szVal);
    strcat(szData, ";");
    nNum = strlen(szData);
    szData[nNum] = (char)0xFF;
    nNum++;
    szData[nNum] = NULL;
    PrintHex("AVR", szData, nNum);
}

// ---- AfxFT232HPortSendAvrCommand(port, cmd) (원본 그대로) ----
static void SendAvrStr(char head, const char* pszCommand)
{
    char szData[255 + 1];
    char szVal[255 + 1];
    char* pszData = NULL;
    int nCheckSum = 0;
    int nNum = 0;
    memset(szData, 0x00, sizeof(szData));
    memset(szVal, 0x00, sizeof(szVal));
    szData[0] = head;
    szData[1] = (char)0x30;
    szData[2] = (char)0x30;
    szData[3] = (char)0x30;
    pszData = szData + 4;
    strcpy(pszData, pszCommand);
    while (*pszData != ';') { nCheckSum += *pszData; pszData++; }
    *pszData = '?';
    memset(szVal, 0x00, sizeof(szVal));
    nCheckSum = nCheckSum % 100;
    _itoa(nCheckSum, szVal, 10);
    strcat(szData, szVal);
    strcat(szData, ";");
    nNum = strlen(szData);
    szData[nNum] = (char)0xFF;
    nNum++;
    szData[nNum] = NULL;
    PrintHex("AVR", szData, nNum);
}

// ---- AfxScanBridgeDataRead / AfxSbrCheckSumCommand ----
#define MAX_FT232H_COMMAND_BUFFER 64
#define MAX_FT232H_XYDSStr_BUFFER 64
#define MAX_FT232H_ASSEMBLE_BUFFER 65536
#define MAX_SBR_COMMAND_NUMBER 44
const char m_szSbrCommandList[MAX_SBR_COMMAND_NUMBER][3] =
{
    "PD", "AR", "EV", "VL", "VS", "HV", "FV", "BV", "AV", "EM",
    "AE", "AT", "RF", "RG", "SN", "RN", "SA", "RA", "SP", "RP",
    "SM", "RM", "MH", "MC", "MP", "MA", "MM", "TS", "MG", "AP",
    "PR", "LT", "RB", "ZT", "SB", "TC", "LS", "TB", "BT", "EC",
    "VR", "TM", "UL", "BF"
};
static char m_szFT232H_CmdBuffer[MAX_FT232H_COMMAND_BUFFER];
static char m_szFT232H_XStrBuffer[MAX_FT232H_XYDSStr_BUFFER];
static char m_szFT232H_YStrBuffer[MAX_FT232H_XYDSStr_BUFFER];
static char m_szFT232H_DStrBuffer[MAX_FT232H_XYDSStr_BUFFER];
static char m_szFT232H_SStrBuffer[MAX_FT232H_XYDSStr_BUFFER];
static int m_nFT232H_CmdCount, m_nFT232H_XStrCount, m_nFT232H_YStrCount, m_nFT232H_DStrCount, m_nFT232H_SStrCount;
static int m_nFT232H_CommaCount;
static int m_nFT232H_XYDState = 'x';
static char szStrBuf[MAX_FT232H_ASSEMBLE_BUFFER + 1];

static int AfxSbrCheckSumCommand(void)
{
    int nRet = 0, nSum = 0, nCheckSum = 0;
    char* pBuf;
    memset(szStrBuf, 0x00, MAX_FT232H_ASSEMBLE_BUFFER);
    if (m_nFT232H_SStrCount > 0)
    {
        nCheckSum = atoi(m_szFT232H_SStrBuffer);
        strcpy(szStrBuf, m_szFT232H_CmdBuffer);
        if (m_nFT232H_XStrCount) strcat(szStrBuf, m_szFT232H_XStrBuffer);
        if (m_nFT232H_YStrCount) { strcat(szStrBuf, ","); strcat(szStrBuf, m_szFT232H_YStrBuffer); }
        if (m_nFT232H_DStrCount) { strcat(szStrBuf, ","); strcat(szStrBuf, m_szFT232H_DStrBuffer); }
        strcat(szStrBuf, ";");
        pBuf = szStrBuf;
        while (*pBuf != ';') nSum += *pBuf++;
        nSum = nSum % 100;
        if (nCheckSum == nSum) nRet = 1;
    }
    else nRet = 1;
    return nRet;
}

static void ScanBridgeDataRead(const BYTE* rx, int dwBytesRead)
{
    for (int n = 0; n < dwBytesRead; n++)
    {
        BYTE bt = rx[n];
        if ('A' <= bt && bt <= 'Z')
        {
            if (m_nFT232H_CmdCount < 2) { m_szFT232H_CmdBuffer[m_nFT232H_CmdCount] = bt; m_nFT232H_CmdCount++; }
            else
            {
                if (!strcmp("TS", m_szFT232H_CmdBuffer) && m_szFT232H_XStrBuffer[0] == '0')
                {
                    m_szFT232H_YStrBuffer[m_nFT232H_YStrCount] = bt;
                    m_nFT232H_YStrCount++;
                    if (m_nFT232H_YStrCount > MAX_FT232H_XYDSStr_BUFFER - 1) { m_nFT232H_YStrCount = 0; memset(m_szFT232H_YStrBuffer, 0x00, MAX_FT232H_XYDSStr_BUFFER); }
                }
            }
        }
        else if ('0' <= bt && bt <= '9' || bt == '.' || bt == '-')
        {
            switch (m_nFT232H_XYDState)
            {
            case 'x':
                m_szFT232H_XStrBuffer[m_nFT232H_XStrCount] = bt; m_nFT232H_XStrCount++;
                if (m_nFT232H_XStrCount > MAX_FT232H_XYDSStr_BUFFER - 1) { m_nFT232H_XStrCount = 0; memset(m_szFT232H_XStrBuffer, 0x00, MAX_FT232H_XYDSStr_BUFFER); }
                break;
            case 'y':
                m_szFT232H_YStrBuffer[m_nFT232H_YStrCount] = bt; m_nFT232H_YStrCount++;
                if (m_nFT232H_YStrCount >= MAX_FT232H_XYDSStr_BUFFER - 1) { m_nFT232H_YStrCount = 0; memset(m_szFT232H_YStrBuffer, 0x00, MAX_FT232H_XYDSStr_BUFFER); }
                break;
            case 'd':
                m_szFT232H_DStrBuffer[m_nFT232H_DStrCount] = bt; m_nFT232H_DStrCount++;
                if (m_nFT232H_DStrCount > MAX_FT232H_XYDSStr_BUFFER - 1) { m_nFT232H_DStrCount = 0; memset(m_szFT232H_DStrBuffer, 0x00, MAX_FT232H_XYDSStr_BUFFER); }
                break;
            case 's':
                m_szFT232H_SStrBuffer[m_nFT232H_SStrCount] = bt; m_nFT232H_SStrCount++;
                if (m_nFT232H_SStrCount >= MAX_FT232H_XYDSStr_BUFFER) { m_nFT232H_SStrCount = 0; memset(m_szFT232H_SStrBuffer, 0x00, MAX_FT232H_XYDSStr_BUFFER); }
                break;
            }
        }
        else if (bt == ',')
        {
            m_nFT232H_CommaCount++;
            switch (m_nFT232H_CommaCount)
            {
            case 1: m_nFT232H_XYDState = 'y'; break;
            case 2: m_nFT232H_XYDState = 'd'; break;
            }
        }
        else if (bt == '?') m_nFT232H_XYDState = 's';
        else if (bt == ';')
        {
            m_szFT232H_CmdBuffer[m_nFT232H_CmdCount] = '\0';
            m_szFT232H_XStrBuffer[m_nFT232H_XStrCount] = '\0';
            m_szFT232H_YStrBuffer[m_nFT232H_YStrCount] = '\0';
            m_szFT232H_DStrBuffer[m_nFT232H_DStrCount] = '\0';
            m_szFT232H_SStrBuffer[m_nFT232H_SStrCount] = '\0';
            if (AfxSbrCheckSumCommand() > 0)
            {
                int nCmd = -1;
                // 원본 조건은 `n < MAX_SBR_COMMAND_NUMBER`(버그). 기준 구현은 의도대로 nIdx로 비교한다.
                for (int nIdx = 0; nIdx < MAX_SBR_COMMAND_NUMBER; nIdx++)
                    if (strncmp(m_szSbrCommandList[nIdx], m_szFT232H_CmdBuffer, 2) == 0) { nCmd = nIdx; break; }
                if (nCmd > -1)
                    printf("MSG %d %s|%s|%s|%s|%s\n", nCmd, m_szFT232H_CmdBuffer, m_szFT232H_XStrBuffer, m_szFT232H_YStrBuffer, m_szFT232H_DStrBuffer, m_szFT232H_SStrBuffer);
            }
            else
            {
                printf("BAD\n");
            }
            m_nFT232H_CmdCount = m_nFT232H_XStrCount = m_nFT232H_YStrCount = m_nFT232H_DStrCount = m_nFT232H_SStrCount = 0;
            m_nFT232H_CommaCount = 0;
            m_nFT232H_XYDState = 'x';
            memset(m_szFT232H_CmdBuffer, 0x00, MAX_FT232H_COMMAND_BUFFER);
            memset(m_szFT232H_XStrBuffer, 0x00, MAX_FT232H_XYDSStr_BUFFER);
            memset(m_szFT232H_YStrBuffer, 0x00, MAX_FT232H_XYDSStr_BUFFER);
            memset(m_szFT232H_DStrBuffer, 0x00, MAX_FT232H_XYDSStr_BUFFER);
            memset(m_szFT232H_SStrBuffer, 0x00, MAX_FT232H_XYDSStr_BUFFER);
        }
    }
}

// vectors.txt 한 줄:
//   CMD <hex command> <int para>
//   AVRV <hex head> <command> <int value>
//   AVRS <hex head> <command;...>
//   RX <hex bytes...>      (한 번의 FT_Read 묶음; 상태는 다음 RX로 이어짐)
int main(int argc, char** argv)
{
    FILE* f = fopen(argc > 1 ? argv[1] : "vectors.txt", "r");
    if (!f) { printf("no vectors\n"); return 1; }
    static char line[70000];
    static BYTE rx[70000];
    while (fgets(line, sizeof(line), f))
    {
        line[strcspn(line, "\r\n")] = 0;
        if (line[0] == 0 || line[0] == '#') continue;
        char* tok = strtok(line, " ");
        if (!strcmp(tok, "CMD"))
        {
            WORD c = (WORD)strtoul(strtok(NULL, " "), NULL, 16);
            int p = atoi(strtok(NULL, " "));
            SendCommand(c, p);
        }
        else if (!strcmp(tok, "AVRV"))
        {
            char h = (char)strtoul(strtok(NULL, " "), NULL, 16);
            char* c = strtok(NULL, " ");
            int v = atoi(strtok(NULL, " "));
            SendAvrVal(h, c, v);
        }
        else if (!strcmp(tok, "AVRS"))
        {
            char h = (char)strtoul(strtok(NULL, " "), NULL, 16);
            char* c = strtok(NULL, " ");
            SendAvrStr(h, c);
        }
        else if (!strcmp(tok, "RX"))
        {
            int n = 0;
            char* t;
            while ((t = strtok(NULL, " ")) != NULL) rx[n++] = (BYTE)strtoul(t, NULL, 16);
            ScanBridgeDataRead(rx, n);
        }
    }
    fclose(f);
    return 0;
}
