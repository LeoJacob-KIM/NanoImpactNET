// Spellman C-FE HV harness: runs the ORIGINAL RS232 code unmodified - USB_RS232_Comm.cpp (compiled as is), the beam dialog
// send side (ModularBeamControlDlg.cpp:6771-8845, 8848-8932), the main dialog receive side (NanoImpact2020cfeDlg.cpp:
// 22540-22612, 29789-29799, 29831-31999, 32100-32112, port open 1473-1495, port close 2798-2805) and the monitor dialog
// ramping state (ModularCfeHVPowerMonitorDlg.cpp:1664-1733) - against stub dialogs and fake serial port functions.
// NanoImpact.Verify hv <script> must print exactly the same for the C# port (src\NanoImpact.Hardware\Spellman).
// Keep ASCII only.
//
// Script lines:
//   # comment
//   OPEN <com>                 main dialog opens COM<com> (OnInitDialog); the beam dialog then copies the port pointer
//   FAILOPEN                   the next CreateFile fails
//   CLOSE                      main dialog closes the port (OnClose)
//   INIT                       beam dialog init finished (m_bWindowInitStartFlag = TRUE)
//   START                      SetCfeHVPowerMonitoringStart()
//   MODE <n>                   SetCfeHVPowerMonitoringTimerExecute(n)
//   TIMER <id> <count>         beam dialog OnTimer(id), count times
//   SEND <code> [args...]      SendingCfeHVPower<code>Command(args)  (int or text arguments)
//   FW <0|1>                   GetCfeHVPowerGroundFirmwareVersion / GetCfeHVPowerHVFirmwareVersion
//   CHECK <n>                  "receive data view" check box value
//   RX <text>                  queue one chunk for the port; escapes \n \r \t \\ \xHH, everything after "RX " is data
//   RUN                        run the receive thread body (CommThread) until the queue is empty and the port closes
#include "pch.h"
#include "DefineStructCode.h"
#include "USB_RS232_Comm.h"
#include <vector>
#include <deque>
#include <string>

static FILE* g_out = stdout;

static std::string Narrow(LPCTSTR w)
{
    if (!w) return std::string();
    int n = WideCharToMultiByte(CP_UTF8, 0, w, -1, NULL, 0, NULL, NULL);
    std::string s(n > 0 ? n - 1 : 0, '\0');
    if (n > 1) WideCharToMultiByte(CP_UTF8, 0, w, -1, &s[0], n, NULL, NULL);
    return s;
}

// text shown on screen: control characters as \xHH so each record stays on one line
static std::string Text(LPCTSTR w)
{
    std::string s = Narrow(w), r;
    char b[8];
    for (unsigned char c : s) { if (c < 0x20 || c == 0x7F) { sprintf(b, "\\x%02X", c); r += b; } else r += (char)c; }
    return r;
}

// ---- fake serial port (same behaviour as NanoImpact.Verify HvHarness.FakeSerial) ----
static const HANDLE COMM_HANDLE = (HANDLE)0x100;
static const HANDLE EVENT_HANDLE = (HANDLE)0x200;
static bool g_failOpen = false;
static DWORD g_mask = 0;
static std::deque<std::vector<unsigned char>> g_chunks;
static std::deque<unsigned char> g_inQue;

static const char* HandleName(HANDLE h) { return h == COMM_HANDLE ? "comm" : h == EVENT_HANDLE ? "event" : "?"; }

HANDLE WINAPI HvCreateFile(LPCTSTR name, DWORD, DWORD, LPSECURITY_ATTRIBUTES, DWORD, DWORD, HANDLE)
{
    fprintf(g_out, "C CreateFile %s\n", Narrow(name).c_str());
    if (g_failOpen) { g_failOpen = false; return INVALID_HANDLE_VALUE; }
    g_mask = 0;
    g_inQue.clear();
    return COMM_HANDLE;
}
BOOL WINAPI HvSetupComm(HANDLE, DWORD inQ, DWORD outQ) { fprintf(g_out, "C SetupComm %u %u\n", inQ, outQ); return TRUE; }
BOOL WINAPI HvPurgeComm(HANDLE, DWORD flags) { fprintf(g_out, "C PurgeComm %u\n", flags); return TRUE; }
BOOL WINAPI HvSetCommTimeouts(HANDLE, LPCOMMTIMEOUTS t)
{
    fprintf(g_out, "C SetCommTimeouts %u %u %u %u %u\n", t->ReadIntervalTimeout, t->ReadTotalTimeoutMultiplier, t->ReadTotalTimeoutConstant,
        t->WriteTotalTimeoutMultiplier, t->WriteTotalTimeoutConstant);
    return TRUE;
}
BOOL WINAPI HvGetCommState(HANDLE, LPDCB) { return TRUE; }
BOOL WINAPI HvSetCommState(HANDLE, LPDCB d)
{
    fprintf(g_out, "C SetCommState BaudRate=%u fBinary=%u fParity=%u Parity=%u ByteSize=%u StopBits=%u fRtsControl=%u fDtrControl=%u "
        "fOutxDsrFlow=%u fOutX=%u fInX=%u XonLim=%u XoffLim=%u fOutxCtsFlow=%u\n",
        d->BaudRate, d->fBinary, d->fParity, d->Parity, d->ByteSize, d->StopBits, d->fRtsControl, d->fDtrControl,
        d->fOutxDsrFlow, d->fOutX, d->fInX, d->XonLim, d->XoffLim, d->fOutxCtsFlow);
    return TRUE;
}
BOOL WINAPI HvSetCommMask(HANDLE, DWORD mask) { fprintf(g_out, "C SetCommMask %u\n", mask); g_mask = mask; return TRUE; }
BOOL WINAPI HvWaitCommEvent(HANDLE, LPDWORD mask, LPOVERLAPPED)
{
    if (g_mask != 0 && g_inQue.empty() && !g_chunks.empty())
    {
        g_inQue.insert(g_inQue.end(), g_chunks.front().begin(), g_chunks.front().end());
        g_chunks.pop_front();
    }
    if (g_mask == 0 || g_inQue.empty()) { *mask = 0; SetLastError(ERROR_OPERATION_ABORTED); return FALSE; }
    *mask = EV_RXCHAR;
    return TRUE;
}
BOOL WINAPI HvClearCommError(HANDLE, LPDWORD errors, LPCOMSTAT stat)
{
    if (errors) *errors = 0;
    if (stat) { memset(stat, 0, sizeof(COMSTAT)); stat->cbInQue = (DWORD)g_inQue.size(); }
    return TRUE;
}
BOOL WINAPI HvReadFile(HANDLE, LPVOID buf, DWORD n, LPDWORD read, LPOVERLAPPED)
{
    DWORD k = 0;
    for (; k < n && !g_inQue.empty(); k++) { ((unsigned char*)buf)[k] = g_inQue.front(); g_inQue.pop_front(); }
    *read = k;
    return TRUE;
}
BOOL WINAPI HvWriteFile(HANDLE, LPCVOID buf, DWORD n, LPDWORD written, LPOVERLAPPED)
{
    fprintf(g_out, "W");
    for (DWORD i = 0; i < n; i++) fprintf(g_out, " %02X", ((const unsigned char*)buf)[i]);
    fprintf(g_out, "\n");
    *written = n;
    return TRUE;
}
BOOL WINAPI HvGetOverlappedResult(HANDLE, LPOVERLAPPED, LPDWORD n, BOOL) { *n = 0; return TRUE; }
BOOL WINAPI HvEscapeCommFunction(HANDLE, DWORD func) { fprintf(g_out, "C EscapeCommFunction %u\n", func); return TRUE; }
BOOL WINAPI HvCloseHandle(HANDLE h) { fprintf(g_out, "C CloseHandle %s\n", HandleName(h)); return TRUE; }
HANDLE WINAPI HvCreateEvent(LPSECURITY_ATTRIBUTES, BOOL, BOOL, LPCTSTR) { return EVENT_HANDLE; }

// ---- redirections declared in the device shim ----
class CNanoImpact2020cfeDlg;
static CNanoImpact2020cfeDlg* g_dlg;
static LRESULT Dispatch(UINT msg, WPARAM w, LPARAM l);
int WINAPI HarnessMessageBox(HWND, LPCTSTR text, LPCTSTR caption, UINT type) { fprintf(g_out, "BOX %s | %s | %u\n", Narrow(caption).c_str(), Narrow(text).c_str(), type); return IDOK; }
LRESULT WINAPI HarnessSendMessage(HWND h, UINT msg, WPARAM w, LPARAM l)
{
    if (msg == UM_CFE_HV_POWER_CONTROL_RECEIVE_DATA) fprintf(g_out, "MSG %u %u %u P\n", (unsigned)(UINT_PTR)h, msg, (unsigned)w);
    else fprintf(g_out, "MSG %u %u %u %d\n", (unsigned)(UINT_PTR)h, msg, (unsigned)w, (int)l);
    return Dispatch(msg, w, l);
}
BOOL WINAPI HarnessPostMessage(HWND, UINT msg, WPARAM w, LPARAM) { fprintf(g_out, "POST %u %u\n", msg, (unsigned)w); return TRUE; }
void WINAPI HarnessSleep(DWORD ms) { fprintf(g_out, "SLEEP %u\n", ms); }
DWORD WINAPI HarnessGetTickCount(void) { return 0; }
// the receive thread is not started: RUN calls its body
static AFX_THREADPROC g_threadProc;
static LPVOID g_threadParam;
CWinThread* AfxBeginThread(AFX_THREADPROC proc, LPVOID param, int, UINT, DWORD, LPSECURITY_ATTRIBUTES)
{
    fprintf(g_out, "THREAD\n");
    g_threadProc = proc;
    g_threadParam = param;
    return new CWinThread();
}

// ---- stand-ins for the dialog controls ----
static void Ui(const char* target, LPCTSTR text) { fprintf(g_out, "UI %s %s\n", target, Text(text).c_str()); }

class CTextX
{
    const char* m_name;
public:
    explicit CTextX(const char* name) : m_name(name) {}
    void SetWindowTextW(LPCTSTR s) { Ui(m_name, s); }
};

class CSpinX
{
    const char* m_name;
public:
    explicit CSpinX(const char* name) : m_name(name) {}
    int SetPos(int n) { fprintf(g_out, "SPIN %s %d\n", m_name, n); return 0; }
};

class CListX
{
public:
    int InsertItem(int n, LPCTSTR s) { fprintf(g_out, "LIST INSERT %d %s\n", n, Text(s).c_str()); return n; }
    BOOL SetItemText(int n, int sub, LPCTSTR s) { fprintf(g_out, "LIST SET %d %d %s\n", n, sub, Text(s).c_str()); return TRUE; }
};

static int g_check = 0;
class CCheckX { public: int GetCheck() { return g_check; } };

class CModularAssistDevice { public: void ExecuteFocusDegaussing(int ms) { fprintf(g_out, "DEGAUSS %d\n", ms); } };

#define TEXTFN(n) void n(CString s) { Ui(#n, s); }

class CModularCfeHVPowerMonitorDlg
{
public:
    CCheckX m_CfeMonitorReceiveDataViewCheck;
    int m_nBeamRampingState = 0;
    int m_nBeamOnTargetFlag = 0;
    CModularAssistDevice m_AssistDevice;
    CModularAssistDevice* m_pAssistDeviceTool = &m_AssistDevice;

    void SetBeamStatusBitmapImage(int i, int o) { fprintf(g_out, "IMG SetBeamStatusBitmapImage %d %d\n", i, o); }
    TEXTFN(SetBeamEnergyStatusMessage) TEXTFN(SetHeaterSupplyStatusMessage) TEXTFN(SetSuppressorSupplyStatusMessage)
    TEXTFN(SetExtractorSupplyStatusMessage) TEXTFN(SetConnectionStatusMessage) TEXTFN(SetOutputStatusMessage)
    TEXTFN(SetGroundFirmwareVersText) TEXTFN(SetHVFirmwareVersText) TEXTFN(SetBeamVoltageMonitorText) TEXTFN(SetBeamVoltageTargetText)
    TEXTFN(SetBeamCurrentMonitorText) TEXTFN(SetExtractorVoltageMonitorText) TEXTFN(SetExtractorVoltageTargetText)
    TEXTFN(SetExtractorCurrentMonitorText) TEXTFN(SetExtractorTripCurrentMonitorText) TEXTFN(SetSuppressorVoltageMonitorText)
    TEXTFN(SetSuppressorVoltageTargetText) TEXTFN(SetSuppressorCurrentMonitorText) TEXTFN(SetHeaterCurrentMonitorText)
    TEXTFN(SetHeaterCurrentTargetText) TEXTFN(SetHeaterVoltageMonitorText)

    void ResetBeamVoltageRampingOnTarget(void);
    void SetBeamVoltageRampingUpDown(int nRamping);
    void SetBeamVoltageOnTarget(int nOnTarget);
    void BeamVoltageTargetExecuteDegaussing(int nDegaussing);
};
#include "monitor_ramping.inc"

#define ID_TIMER_CFE_HV_POWER_ALL_MONITORING 2002
#define ID_TIMER_CFE_HV_POWER_HV_MONITORING  2003

class CModularBeamControlDlg
{
public:
    CModularBeamControlDlg()
        : m_CfeControlBeamEnergyVoltageEdit("m_CfeControlBeamEnergyVoltageEdit"), m_CfeControlHeaterCurrentEdit("m_CfeControlHeaterCurrentEdit"),
          m_CfeControlSuppressorVoltageEdit("m_CfeControlSuppressorVoltageEdit"), m_CfeControlExtractorVoltageEdit("m_CfeControlExtractorVoltageEdit"),
          m_CfeControlExtractorTripCurrentEdit("m_CfeControlExtractorTripCurrentEdit"),
          m_CfeControlBeamEnergyVoltageSpin("m_CfeControlBeamEnergyVoltageSpin"), m_CfeControlHeaterCurrentSpin("m_CfeControlHeaterCurrentSpin"),
          m_CfeControlSuppressorVoltageSpin("m_CfeControlSuppressorVoltageSpin"), m_CfeControlExtractorVoltageSpin("m_CfeControlExtractorVoltageSpin"),
          m_CfeControlExtractorTripCurrentSpin("m_CfeControlExtractorTripCurrentSpin")
    {
    }

    // ---- members (ModularBeamControlDlg.h, constructor values) ----
    CUsbRS232Comm* m_pUsbRS232Comm = NULL;
    CWnd* m_pOwnerWindow = NULL;
    BOOL m_bWindowInitStartFlag = FALSE;
    int m_nCfeHVPowerMonitoringMode = -1;
    int m_nCfeHVPowerAllMonitoringIndex = 0;
    int m_nCfeHVPowerHVMonitoringIndex = 0;
    int m_nCfeHVPowerOneTimeMonitoring = 0;
    BOOL m_bSetBeamTargetValueFlag = FALSE, m_bSetHeaterTargetValueFlag = FALSE, m_bSetSuppressorTargetValueFlag = FALSE;
    BOOL m_bSetExtractorTargetValueFlag = FALSE, m_bSetExtractorTripTargetValueFlag = FALSE;
    CTextX m_CfeControlBeamEnergyVoltageEdit, m_CfeControlHeaterCurrentEdit, m_CfeControlSuppressorVoltageEdit;
    CTextX m_CfeControlExtractorVoltageEdit, m_CfeControlExtractorTripCurrentEdit;
    CSpinX m_CfeControlBeamEnergyVoltageSpin, m_CfeControlHeaterCurrentSpin, m_CfeControlSuppressorVoltageSpin;
    CSpinX m_CfeControlExtractorVoltageSpin, m_CfeControlExtractorTripCurrentSpin;

    // ---- dialog functions outside the pasted ranges ----
    UINT_PTR SetTimer(UINT_PTR id, UINT ms, void*) { fprintf(g_out, "TIMER SET %u %u\n", (unsigned)id, ms); return id; }
    BOOL KillTimer(UINT_PTR id) { fprintf(g_out, "TIMER KILL %u\n", (unsigned)id); return TRUE; }
    void SetEnableSupplyBitmapImage(int i, int o) { fprintf(g_out, "IMG SetEnableSupplyBitmapImage %d %d\n", i, o); }
    TEXTFN(SetReadRampRateUpBeamEnergyText) TEXTFN(SetReadRampRateUpHeaterCurrentText) TEXTFN(SetReadRampRateUpSuppressorVoltText)
    TEXTFN(SetReadRampRateUpExtractorVoltageText) TEXTFN(SetReadRampRateDownBeamEnergyText) TEXTFN(SetReadRampRateDownHeaterCurrentText)
    TEXTFN(SetReadRampRateDownSuppressorVoltText) TEXTFN(SetReadRampRateDownExtractorVoltageText)
    // ModularBeamControlDlg.cpp:477
    void SetCfeHVPowerMonitoringStart(void) { SetCfeHVPowerMonitoringTimerExecute(0); }
    // ModularBeamControlDlg.cpp:856-867 (the two Spellman branches of OnTimer)
    void OnTimer(UINT_PTR nIDEvent)
    {
        if (nIDEvent == ID_TIMER_CFE_HV_POWER_ALL_MONITORING) ExecuteCfeHVPowerAllDataMonitoring();
        else if (nIDEvent == ID_TIMER_CFE_HV_POWER_HV_MONITORING) ExecuteCfeHVPowerHVProcessDataMonitoring();
    }

    // ---- the original code ----
    void SetCfeHVPowerMonitoringTimerExecute(int nTimerMode);
    void ExecuteCfeHVPowerAllDataMonitoring(void);
    void ExecuteCfeHVPowerHVProcessDataMonitoring(void);
    int SendingCfeHVPower00Command(void); int SendingCfeHVPower01Command(void); int SendingCfeHVPower02Command(void);
    int SendingCfeHVPower03Command(int nArg); int SendingCfeHVPower04Command(int nArg); int SendingCfeHVPower05Command(int nArg);
    int SendingCfeHVPower08Command(int nArg); int SendingCfeHVPower09Command(CString strText); int SendingCfeHVPower0ACommand(void);
    int SendingCfeHVPower0BCommand(CString strText1, CString strText2); int SendingCfeHVPower0ECommand(void);
    int SendingCfeHVPower14Command(int nArg); int SendingCfeHVPower15Command(CString strText); int SendingCfeHVPower16Command(void);
    int SendingCfeHVPower17Command(CString strText1, CString strText2); int SendingCfeHVPower1ACommand(void); int SendingCfeHVPower1CCommand(void);
    int SendingCfeHVPower1DCommand(CString strText); int SendingCfeHVPower1ECommand(int nArg); int SendingCfeHVPower1FCommand(CString strText);
    int SendingCfeHVPower20Command(void); int SendingCfeHVPower21Command(CString strText1, CString strText2); int SendingCfeHVPower24Command(void);
    int SendingCfeHVPower26Command(void); int SendingCfeHVPower28Command(int nArg); int SendingCfeHVPower29Command(CString strText);
    int SendingCfeHVPower2ACommand(void); int SendingCfeHVPower2BCommand(CString strText1, CString strText2); int SendingCfeHVPower50Command(void);
    WORD MakeCfeHVPowerCommandCheckSum(unsigned char* pszMessage, WORD index_of_last_character);
    void SetBeamTargetVoltageValue(CString strText);
    void SetHeaterTargetCurrentValue(CString strText);
    void SetSuppressorTargetVoltageValue(CString strText);
    void SetExtractorTargetVoltageValue(CString strText);
    void SetExtractorTripCurrentValue(CString strText);
};
#include "beam_methods.inc"
#include "beam_target.inc"

class CNanoImpact2020cfeDlg
{
public:
    CNanoImpact2020cfeDlg()
        : m_TopToolFEPowerStatusText("m_TopToolFEPowerStatusText"), m_TopToolExtractorCurrentText("m_TopToolExtractorCurrentText")
    {
        memset(&m_ClientPublicData, 0x00, sizeof(CLIENT_PUBLIC_DATA));
        memset(m_szCfeHVPowerDataBuffer, 0x00, MAX_RS232_BUFFER);
    }

    // ---- members (NanoImpact2020cfeDlg.h, constructor values) ----
    CUsbRS232Comm* m_pUsbRS232Comm = NULL;
    BOOL m_bUsbRS232ComPortOpen = FALSE;
    CLIENT_PUBLIC_DATA m_ClientPublicData;
    int m_nReceiveDataListItemIndex = 0;
    int m_nCfeHVPowerMainOutputStatus = CFE_OUTPUT_STATUS_MAIN_NO_STATUS;
    int m_nCfeHVPowerErrorOutputStatus = CFE_OUTPUT_STATUS_NO_ERROR;
    char m_szCfeHVPowerDataBuffer[MAX_RS232_BUFFER];
    CString m_strExtratorCurrent;
    CModularBeamControlDlg m_wndBeamControlDlg;
    CModularCfeHVPowerMonitorDlg m_wndCfeHVPowerMonitorDlg;
    CTextX m_TopToolFEPowerStatusText, m_TopToolExtractorCurrentText;
    CListX m_CfeHVPowerReceiveDataListCtrl;
    HWND GetSafeHwnd() { return (HWND)1; }

    // OnInitDialog: open the C-FE HV COM port (lines 1473-1495)
    void PortOpen()
    {
#include "port_open.inc"
    }
    // OnClose: close the port (lines 2798-2805)
    void PortClose()
    {
#include "port_close.inc"
    }

    // ---- the original code ----
    LRESULT OnCfeHVPowerUsbRS232ComPortClose(WPARAM wParam, LPARAM lParam);
    LRESULT OnCfeHVPowerUsbRS232ComPortReceiveData(WPARAM wParam, LPARAM lParam);
    CString GetCfeHVPowerControlAndMonitorComPort(int nComPort);
    void CfeHVPowerRS232ReceiveDataProcess(char* pszData, int nData);
    int CfeHVPowerCommandDataAnalysis(char* pszCommand, int nData);
    int CfeHVPowerCommand01Process(char* pszARG1, int nData1, char* pszARG2, int nData2);
    int CfeHVPowerCommand02Process(char* pszARG1, int nData);
    int CfeHVPowerCommand05Process(char* pszARG1, int nData1, char* pszARG2, int nData2);
    int CfeHVPowerCommand08Process(char* pszARG1, int nData1, char* pszARG2, int nData2);
    int CfeHVPowerCommand09Process(char* pszARG1, int nData1, char* pszARG2, int nData2);
    int CfeHVPowerCommand0AProcess(char* pszARG1, int nData1, char* pszARG2, int nData2);
    int CfeHVPowerCommand0EProcess(char* pszARG1, int nData1);
    int CfeHVPowerCommand10Process(char* pszARG1, int nData1, char* pszARG2, int nData2);
    int CfeHVPowerCommand12Process(char* pszARG1, int nData1);
    int CfeHVPowerCommand14Process(char* pszARG1, int nData1, char* pszARG2, int nData2);
    int CfeHVPowerCommand15Process(char* pszARG1, int nData1, char* pszARG2, int nData2);
    int CfeHVPowerCommand16Process(char* pszARG1, int nData1, char* pszARG2, int nData2);
    int CfeHVPowerCommand1AProcess(char* pszARG1, int nData1);
    int CfeHVPowerCommand1CProcess(char* pszARG1, int nData1);
    int CfeHVPowerCommand1EProcess(char* pszARG1, int nData1, char* pszARG2, int nData2);
    int CfeHVPowerCommand1FProcess(char* pszARG1, int nData1, char* pszARG2, int nData2);
    int CfeHVPowerCommand20Process(char* pszARG1, int nData1, char* pszARG2, int nData2);
    int CfeHVPowerCommand24Process(char* pszARG1, int nData1);
    int CfeHVPowerCommand26Process(char* pszARG1, int nData1);
    int CfeHVPowerCommand28Process(char* pszARG1, int nData1, char* pszARG2, int nData2);
    int CfeHVPowerCommand29Process(char* pszARG1, int nData1, char* pszARG2, int nData2);
    int CfeHVPowerCommand2AProcess(char* pszARG1, int nData1, char* pszARG2, int nData2);
    int CfeHVPowerCommand50Process(char* pszARG1, int nData1, char* pszARG2, int nData2, char* pszARG3, int nData3, char* pszARG4, int nData4, char* pszARG5, int nData5);
    int CfeHVPowerCommand52Process(char* pszARG1, int nData1, char* pszARG2, int nData2);
    int CfeHVPowerCommandOutputMessage(void);
    int CfeHVPowerCommandArgHexToOutputStatus(BYTE btData, int nHiLow);
    BYTE CfeHVPowerCommandArgCharToHex(char szARG);
    void GetCfeHVPowerGroundFirmwareVersion();
    void GetCfeHVPowerHVFirmwareVersion();
};
#include "main_port.inc"
#include "main_comport.inc"
#include "main_receive.inc"
#include "main_firmware.inc"

static LRESULT Dispatch(UINT msg, WPARAM w, LPARAM l)
{
    if (msg == UM_CFE_HV_POWER_CONTROL_RECEIVE_DATA) return g_dlg->OnCfeHVPowerUsbRS232ComPortReceiveData(w, l);
    if (msg == UM_CFE_HV_POWER_CONTROL_PORT_CLOSE) return g_dlg->OnCfeHVPowerUsbRS232ComPortClose(w, l);
    return 0;
}

// ---- state printing (changes only) ----
static std::vector<std::string> g_prev;

static std::string Escape(const char* p, size_t n)
{
    std::string s;
    for (size_t i = 0; i < n && p[i]; i++)
    {
        unsigned char c = (unsigned char)p[i];
        char b[8];
        if (c == '\\') s += "\\\\";
        else if (c >= 0x20 && c < 0x7F) s += (char)c;
        else { sprintf(b, "\\x%02X", c); s += b; }
    }
    return s;
}

static void PrintState()
{
    CNanoImpact2020cfeDlg* d = g_dlg;
    CModularBeamControlDlg* b = &d->m_wndBeamControlDlg;
    std::vector<std::string> cur;
    char t[64];
#define I(name, v) sprintf(t, "%d", (int)(v)); cur.push_back(std::string(name) + "=" + t);
    I("port", d->m_pUsbRS232Comm != NULL)
    I("m_bUsbRS232ComPortOpen", d->m_bUsbRS232ComPortOpen)
    I("m_nReceiveDataListItemIndex", d->m_nReceiveDataListItemIndex)
    I("m_nCfeHVPowerMainOutputStatus", d->m_nCfeHVPowerMainOutputStatus)
    I("m_nCfeHVPowerErrorOutputStatus", d->m_nCfeHVPowerErrorOutputStatus)
    cur.push_back("m_szCfeHVPowerDataBuffer=" + Escape(d->m_szCfeHVPowerDataBuffer, MAX_RS232_BUFFER));
    cur.push_back("m_strExtratorCurrent=" + Narrow(d->m_strExtratorCurrent));
    I("m_nBeamRampingState", d->m_wndCfeHVPowerMonitorDlg.m_nBeamRampingState)
    I("m_nBeamOnTargetFlag", d->m_wndCfeHVPowerMonitorDlg.m_nBeamOnTargetFlag)
    I("m_bWindowInitStartFlag", b->m_bWindowInitStartFlag)
    I("m_nCfeHVPowerMonitoringMode", b->m_nCfeHVPowerMonitoringMode)
    I("m_nCfeHVPowerAllMonitoringIndex", b->m_nCfeHVPowerAllMonitoringIndex)
    I("m_nCfeHVPowerHVMonitoringIndex", b->m_nCfeHVPowerHVMonitoringIndex)
    I("m_nCfeHVPowerOneTimeMonitoring", b->m_nCfeHVPowerOneTimeMonitoring)
    I("m_bSetBeamTargetValueFlag", b->m_bSetBeamTargetValueFlag)
    I("m_bSetHeaterTargetValueFlag", b->m_bSetHeaterTargetValueFlag)
    I("m_bSetSuppressorTargetValueFlag", b->m_bSetSuppressorTargetValueFlag)
    I("m_bSetExtractorTargetValueFlag", b->m_bSetExtractorTargetValueFlag)
    I("m_bSetExtractorTripTargetValueFlag", b->m_bSetExtractorTripTargetValueFlag)
#undef I
    if (g_prev.size() != cur.size()) g_prev.assign(cur.size(), "");
    for (size_t i = 0; i < cur.size(); i++)
        if (cur[i] != g_prev[i]) fprintf(g_out, "F %s\n", cur[i].c_str());
    g_prev = cur;
}

static std::vector<unsigned char> Unescape(const std::string& s)
{
    std::vector<unsigned char> r;
    for (size_t i = 0; i < s.size(); i++)
    {
        if (s[i] != '\\' || i + 1 >= s.size()) { r.push_back((unsigned char)s[i]); continue; }
        char c = s[++i];
        if (c == 'n') r.push_back(0x0A);
        else if (c == 'r') r.push_back(0x0D);
        else if (c == 't') r.push_back(0x09);
        else if (c == 'x' && i + 2 < s.size() + 1) { r.push_back((unsigned char)strtoul(s.substr(i + 1, 2).c_str(), NULL, 16)); i += 2; }
        else r.push_back((unsigned char)c);
    }
    return r;
}

static CString Wide(const std::string& s) { return CString(s.c_str()); }

static int Send(CModularBeamControlDlg* b, const std::string& code, const std::vector<std::string>& a)
{
    auto A = [&](size_t i) { return i < a.size() ? a[i] : std::string(); };
    auto N = [&](size_t i) { return atoi(A(i).c_str()); };
#define V(c) if (code == #c) return b->SendingCfeHVPower##c##Command();
#define NI(c) if (code == #c) return b->SendingCfeHVPower##c##Command(N(0));
#define S1(c) if (code == #c) return b->SendingCfeHVPower##c##Command(Wide(A(0)));
#define S2(c) if (code == #c) return b->SendingCfeHVPower##c##Command(Wide(A(0)), Wide(A(1)));
    V(00) V(01) V(02) NI(03) NI(04) NI(05) NI(08) S1(09) V(0A) S2(0B) V(0E) NI(14) S1(15) V(16) S2(17) V(1A) V(1C) S1(1D)
    NI(1E) S1(1F) V(20) S2(21) V(24) V(26) NI(28) S1(29) V(2A) S2(2B) V(50)
#undef V
#undef NI
#undef S1
#undef S2
    fprintf(g_out, "NOCMD %s\n", code.c_str());
    return 0;
}

int main(int argc, char** argv)
{
    if (argc < 2) { fprintf(stderr, "usage: harness <script>\n"); return 2; }
    g_dlg = new CNanoImpact2020cfeDlg();
    PrintState();
    FILE* f = fopen(argv[1], "rb");
    if (!f) { fprintf(stderr, "cannot open %s\n", argv[1]); return 2; }
    static char line[65536];
    while (fgets(line, sizeof(line), f))
    {
        line[strcspn(line, "\r\n")] = 0;
        if ((unsigned char)line[0] == 0xEF && (unsigned char)line[1] == 0xBB && (unsigned char)line[2] == 0xBF) memmove(line, line + 3, strlen(line + 3) + 1);
        if (!line[0] || line[0] == '#') continue;
        std::string raw = line;
        std::vector<std::string> tok;
        for (char* t = strtok(line, " \t"); t; t = strtok(NULL, " \t")) tok.push_back(t);
        if (tok.empty()) continue;
        const std::string cmd = tok[0];
        CModularBeamControlDlg* b = &g_dlg->m_wndBeamControlDlg;
        if (cmd == "RX") { fprintf(g_out, "> RX %s\n", raw.substr(3).c_str()); g_chunks.push_back(Unescape(raw.substr(3))); fflush(g_out); continue; }
        fprintf(g_out, "> %s\n", raw.c_str());
        if (cmd == "OPEN" && tok.size() >= 2)
        {
            g_dlg->m_ClientPublicData.nOptionData[IDX_CLIENT_OPTION_INT_SEM_CFE_COM_PORT] = atoi(tok[1].c_str());
            g_dlg->PortOpen();
            b->m_pUsbRS232Comm = g_dlg->m_pUsbRS232Comm; // beam dialog init copies the pointer (ModularBeamControlDlg.cpp:377)
        }
        else if (cmd == "FAILOPEN") g_failOpen = true;
        else if (cmd == "CLOSE") g_dlg->PortClose();
        else if (cmd == "INIT") b->m_bWindowInitStartFlag = TRUE;
        else if (cmd == "START") b->SetCfeHVPowerMonitoringStart();
        else if (cmd == "MODE" && tok.size() >= 2) b->SetCfeHVPowerMonitoringTimerExecute(atoi(tok[1].c_str()));
        else if (cmd == "TIMER" && tok.size() >= 3)
        {
            int n = atoi(tok[2].c_str());
            for (int i = 0; i < n; i++) b->OnTimer((UINT_PTR)atoi(tok[1].c_str()));
        }
        else if (cmd == "SEND" && tok.size() >= 2)
        {
            int r = Send(b, tok[1], std::vector<std::string>(tok.begin() + 2, tok.end()));
            fprintf(g_out, "RET %d\n", r);
        }
        else if (cmd == "FW" && tok.size() >= 2) { if (atoi(tok[1].c_str()) == 0) g_dlg->GetCfeHVPowerGroundFirmwareVersion(); else g_dlg->GetCfeHVPowerHVFirmwareVersion(); }
        else if (cmd == "CHECK" && tok.size() >= 2) g_check = atoi(tok[1].c_str());
        else if (cmd == "RUN")
        {
            if (g_threadProc) { AFX_THREADPROC p = g_threadProc; g_threadProc = NULL; p(g_threadParam); }
            else fprintf(g_out, "NOTHREAD\n");
            g_chunks.clear();
            b->m_pUsbRS232Comm = g_dlg->m_pUsbRS232Comm; // the port class is gone; do not let the beam dialog keep the stale pointer
        }
        else fprintf(g_out, "UNKNOWN %s\n", cmd.c_str());
        PrintState();
        fflush(g_out);
    }
    fclose(f);
    return 0;
}
