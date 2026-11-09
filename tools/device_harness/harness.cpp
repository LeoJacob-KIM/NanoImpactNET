// Device harness: drives the ORIGINAL USB_FT232H_Device.cpp (copied unmodified) from a script and prints
// every device write, window message, message box, sleep and change to the integrated data.
// NanoImpact.Verify device <script> must print exactly the same for the C# port.
// Keep ASCII only (compiled with /source-charset:.949 together with the original headers).
//
// Script lines:
//   # comment
//   CALL <key> [args...]   key = "AfxName(type,type)" as listed in dispatch.inc
//   PATH <dir>             AfxSetDeviceServerPath(dir)
//   TICK <n>               value returned by GetTickCount()
//   BOXRET <n>             value returned by MessageBox()
//   RX <port> <hex...>     queue bytes that the fake FT232H port <port> will return on FT_Read
//   SEREAD                 AfxSEScanDataImageProcess(m_pSEImageDataThreadParams) (SE image read thread body)
#include "stdAfx.h"
#include "DefineStructCode.h"
#include "USB_FT232H_Device.h"
#include <vector>
#include <map>
#include <string>
#include <functional>

static FILE* g_out = stdout;
static DWORD g_tick = 0;
static int g_boxRet = IDOK;
static std::vector<unsigned char> g_prevIntegrated;
static std::vector<unsigned char> g_prevSRot;
typedef void (*PushRxFn)(int, const unsigned char*, int);
static PushRxFn g_pushRx = NULL;

static std::string Narrow(LPCTSTR w)
{
    if (!w) return std::string();
    int n = WideCharToMultiByte(CP_UTF8, 0, w, -1, NULL, 0, NULL, NULL);
    std::string s(n > 0 ? n - 1 : 0, '\0');
    if (n > 1) WideCharToMultiByte(CP_UTF8, 0, w, -1, &s[0], n, NULL, NULL);
    return s;
}

static std::wstring Widen(const char* s)
{
    int n = MultiByteToWideChar(CP_ACP, 0, s, -1, NULL, 0);
    std::wstring w(n > 0 ? n - 1 : 0, L'\0');
    if (n > 1) MultiByteToWideChar(CP_ACP, 0, s, -1, &w[0], n);
    return w;
}

static void Sink(const char* s) { fputs(s, g_out); }

// ---- redirections declared in shim\stdAfx.h ----
int WINAPI HarnessMessageBox(HWND, LPCTSTR text, LPCTSTR caption, UINT type)
{
    fprintf(g_out, "BOX %s | %s\n", Narrow(caption).c_str(), Narrow(text).c_str());
    return g_boxRet;
}

static unsigned long long Fnv(const void* p, size_t n)
{
    const unsigned char* b = (const unsigned char*)p;
    unsigned long long h = 1469598103934665603ULL;
    for (size_t i = 0; i < n; i++) { h ^= b[i]; h *= 1099511628211ULL; }
    return h;
}

static bool IsImageDataMessage(UINT msg)
{
    return msg == UM_SEM_SE_SCAN_IMAGE_DATA || msg == UM_SEM_SE_SCAN_IMAGE_DATA_AUTO_FOCUS || msg == UM_SEM_SE_SCAN_IMAGE_DATA_AUTO_CONTRAST ||
           msg == UM_SEM_SE_SCAN_IMAGE_DATA_AUTO_BRIGHTNESS || msg == UM_SEM_SE_SCAN_IMAGE_DATA_AUTO_CONT_BRIG || msg == UM_SEM_SE_SCAN_IMAGE_DATA_AUTO_GUN_ALIGN;
}

static void PrintMessage(const char* tag, HWND h, UINT msg, WPARAM w, LPARAM l)
{
    // lParam is often a pointer to a stack struct: print only whether it is a pointer.
    // SE image data messages carry (size, buffer): print a hash of the buffer too.
    if ((ULONG_PTR)l < 0x10000) fprintf(g_out, "%s %d %u %u %d\n", tag, (int)(INT_PTR)h, msg, (unsigned)w, (int)l);
    else if (IsImageDataMessage(msg)) fprintf(g_out, "%s %d %u %u P %016llX\n", tag, (int)(INT_PTR)h, msg, (unsigned)w, Fnv((const void*)l, (size_t)w));
    else fprintf(g_out, "%s %d %u %u P\n", tag, (int)(INT_PTR)h, msg, (unsigned)w);
}

static LRESULT DispatchMain(UINT msg, WPARAM w, LPARAM l);
// messages to the main window (hWnd 1) run the ported main dialog handlers, like a real window
LRESULT WINAPI HarnessSendMessage(HWND h, UINT msg, WPARAM w, LPARAM l) { PrintMessage("MSG", h, msg, w, l); return (INT_PTR)h == 1 ? DispatchMain(msg, w, l) : 0; }
BOOL WINAPI HarnessPostMessage(HWND h, UINT msg, WPARAM w, LPARAM l) { PrintMessage("POST", h, msg, w, l); return TRUE; }
void WINAPI HarnessSleep(DWORD ms) { fprintf(g_out, "SLEEP %u\n", ms); }
DWORD WINAPI HarnessGetTickCount(void) { return g_tick; }

CWinThread* AfxBeginThread(AFX_THREADPROC, LPVOID, int, UINT, DWORD, LPSECURITY_ATTRIBUTES)
{
    fprintf(g_out, "THREAD\n");
    return new CWinThread();
}

// ---- step 6a/6b: ModularAssistDevice (ModularAssistDevice.cpp:10-3914 as assist_methods.inc) and the main dialog device
// handlers (NanoImpact2020cfeDlg.cpp 16149-16582, 16901-21569, 21606-22313, 22906-23141, 33902-33922 as main_device.inc),
// both pasted unmodified. Panels are recording stand-ins (panels.inc, generated); main dialog functions outside the pasted
// ranges print "V name args"; timers print "TIMER SET/KILL".
static std::string Text(LPCTSTR w)
{
    std::string s = Narrow(w), r;
    char b[8];
    for (unsigned char c : s) { if (c < 0x20 || c == 0x7F) { sprintf(b, "\\x%02X", c); r += b; } else r += (char)c; }
    return r;
}
static std::string g_panelLine;
static void PanelLine(const char* name) { g_panelLine = std::string("P ") + name; }
static void PanelInt(int v) { char b[32]; sprintf(b, " %d", v); g_panelLine += b; }
static void PanelUInt(unsigned v) { char b[32]; sprintf(b, " %u", v); g_panelLine += b; }
static void PanelDbl(double v) { char b[64]; sprintf(b, " %.17g", v); g_panelLine += b; }
static void PanelTxt(LPCTSTR s) { g_panelLine += " " + Text(s); }
static void PanelHash(const void* p, size_t n) { char b[32]; sprintf(b, " %016llX", p ? Fnv(p, n) : 0ULL); g_panelLine += b; }
static void PanelEnd() { fprintf(g_out, "%s\n", g_panelLine.c_str()); }
class CTextRec
{
    const char* m_name;
public:
    CTextRec(const char* name) : m_name(name) {}
    void SetWindowTextW(LPCTSTR s) { fprintf(g_out, "P %s.SetWindowTextW %s\n", m_name, Text(s).c_str()); }
    void SetTextDataString(CString s) { fprintf(g_out, "P %s.SetTextDataString %s\n", m_name, Text(s).c_str()); }
};
#include "panels.inc"

// main dialog timer ids (NanoImpact2020cfeDlg.cpp:408-426)
#define TIMER_ID_SE_SCAN_IMAGE_DISPLAY             10
#define TIMER_ID_SE_SCAN_IMAGE_DISPLAY_NEXT        11
#define TIMER_ID_SE_PHOTO12_SCAN_IMAGE_DISPLAY     20
#define TIMER_ID_SE_PHOTO345_SCAN_IMAGE_DISPLAY    21
#define TIMER_ID_SE_PHOTO6_SCAN_IMAGE_DISPLAY      22
#define TIMER_ID_BSE_SCAN_IMAGE_DISPLAY            30
#define TIMER_ID_MAG_XY_AXIS_ACCEL_CHANGE          40
#define TIMER_ID_AUTO_START_PROCESS                50
#define TIMER_ID_DIALOG_START_PROCESS              98
#define TIMER_ID_DIALOG_PROGRAM_CLOSE              99

#include "ModularAssistDevice.h"
class CMessageNoticeBoxDlg { public: virtual ~CMessageNoticeBoxDlg() {} };
static int g_autoBCRun = 0;

class CNanoImpact2020cfeDlg : public CWnd
{
public:
    CNanoImpact2020cfeDlg()
    {
        memset(&m_IntegratedData, 0x00, sizeof(m_IntegratedData));
        memset(&m_ClientPublicData, 0x00, sizeof(m_ClientPublicData));
        memset(&m_SemSEScanControlPublicData, 0x00, sizeof(m_SemSEScanControlPublicData));
        memset(&m_VacuumBeamAlignFocusData, 0x00, sizeof(m_VacuumBeamAlignFocusData));
    }
    // ---- members (NanoImpact2020cfeDlg.h) ----
    INTEGRATED_COMMAND_STRUCT m_IntegratedData;
    CLIENT_PUBLIC_DATA m_ClientPublicData;
    SEM_SE_SCAN_CONTROL_PUBLIC_DATA m_SemSEScanControlPublicData;
    VACUUM_BEAM_ALIGN_FOCUS_DATA m_VacuumBeamAlignFocusData;
    CString m_strDistDecimalPlaces = _T("%6.2f");  // constructor value (NanoImpact2020cfeDlg.cpp:870)
    BOOL m_bCLOnOffFlag = TRUE, m_bAutoStartConfigChengeFlag = FALSE, m_bPhotoCalibrationFlag = FALSE; // constructor values (:960, :742, :940)
    CMessageNoticeBoxDlg* m_pdlgMessageNotice = NULL;
    LPCLIENTSEIMAGEDATATHREADPARAMS m_pSEScanImageDataThreadParams = NULL;
    CWinThread* m_pSEScanImageDataProcessThread = NULL;
    int m_nSpotSizeValue = 0, m_nWDx10Position = 0, m_nDialogStartStep = 0;
    CModularAssistDevice* m_pAssistDeviceTool = NULL;
    CModularAlignControlDlg m_wndAlignControlDlg;
    CModularBeamControlDlg m_wndBeamControlDlg;
    CModularDetailControlDlg m_wndDetailControlDlg;
    CModularEnvironmentToolDlg m_wndEnvironmentToolDlg;
    CModularFocusControlDlg m_wndFocusControlDlg;
    CModularScanCalibrationToolDlg m_wndScanCalibrationToolDlg;
    CModularScanControlDlg m_wndScanControlDlg;
    CModularStageCalibrationToolDlg m_wndStageCalibrationToolDlg;
    CModularStageControlDlg m_wndStageControlDlg;
    CModularVacuumControlDlg m_wndVacuumControlDlg;

    // ---- main dialog functions outside the pasted ranges ----
    HWND GetSafeHwnd() { return (HWND)1; }
    UINT_PTR SetTimer(UINT_PTR id, UINT ms, void*) { fprintf(g_out, "TIMER SET %u %u\n", (unsigned)id, ms); return id; }
    BOOL KillTimer(UINT_PTR id) { fprintf(g_out, "TIMER KILL %u\n", (unsigned)id); return TRUE; }
    void Update1stImageInfoBitmap(int a, int b, int c, int d) { fprintf(g_out, "V Update1stImageInfoBitmap %d %d %d %d\n", a, b, c, d); }
    void Update2ndImageInfoBitmap(int a, int b, int c, int d) { fprintf(g_out, "V Update2ndImageInfoBitmap %d %d %d %d\n", a, b, c, d); }
    void DisplayVacuumStateInfo(int n) { fprintf(g_out, "V DisplayVacuumStateInfo %d\n", n); }
    void SetMainDlgMagComboIndex(int n = 0) { fprintf(g_out, "V SetMainDlgMagComboIndex %d\n", n); }
    void SetMainDlgGunHVComboIndex(int n = 0) { fprintf(g_out, "V SetMainDlgGunHVComboIndex %d\n", n); }
    int SemScanModeSelectAndScanStart(int a, int b, int c, int d, int e, int f, BOOL g) { fprintf(g_out, "V SemScanModeSelectAndScanStart %d %d %d %d %d %d %d\n", a, b, c, d, e, f, g); return 0; }
    int SemSlow12ScanModeSelectAndScanStart(int a, int b, int c, int d, int e, int f) { fprintf(g_out, "V SemSlow12ScanModeSelectAndScanStart %d %d %d %d %d %d\n", a, b, c, d, e, f); return 0; }
    void SemSEScanImageDataUpdateControl(void) { fprintf(g_out, "V SemSEScanImageDataUpdateControl\n"); }
    void StageXYAxisMagnificationMovingSpeedChange(int n) { fprintf(g_out, "V StageXYAxisMagnificationMovingSpeedChange %d\n", n); }
    void SetExecuteAutoProcessingStop(void) { fprintf(g_out, "V SetExecuteAutoProcessingStop\n"); }
    void SetExecuteAutoFocusProcessing(void) { fprintf(g_out, "V SetExecuteAutoFocusProcessing\n"); }
    void SetExecuteAutoContrastProcessing(void) { fprintf(g_out, "V SetExecuteAutoContrastProcessing\n"); }
    void SetExecuteAutoContrastBrightnessProcessing(void) { fprintf(g_out, "V SetExecuteAutoContrastBrightnessProcessing\n"); }
    void SetExecuteAutoBrightnessProcessing(void) { fprintf(g_out, "V SetExecuteAutoBrightnessProcessing\n"); }
    BOOL SetAutoBCCompletedFlag(BOOL b) { fprintf(g_out, "V SetAutoBCCompletedFlag %d\n", b); return TRUE; }
    BOOL GetAutoBCRunFlag() { fprintf(g_out, "V GetAutoBCRunFlag\n"); return g_autoBCRun; }
    void SetWDSetupEnabledOLValue(int n) { fprintf(g_out, "V SetWDSetupEnabledOLValue %d\n", n); }

    // ---- the original code (main_device.inc) ----
#include "main_protos.inc"
};

int AfxMessageBox(LPCTSTR text, UINT type = MB_OK, UINT = 0) { return HarnessMessageBox(NULL, text, _T(""), type); }

#include "assist_methods.inc"
#include "main_device.inc"

static CModularAssistDevice g_assist;
static CNanoImpact2020cfeDlg g_main;

// the main window's message map for the ported handlers (generated from BEGIN_MESSAGE_MAP)
static LRESULT DispatchMain(UINT msg, WPARAM w, LPARAM l)
{
#include "main_msgmap.inc"
    return 0;
}

// ---- dispatch ----
struct Args
{
    std::vector<std::string> v;
    std::vector<std::vector<char> > bufs;
    std::vector<REQUEST_USB_DEVICE_DATA> reqs;
    CString CStr(size_t k) const { return CString(Widen(k < v.size() ? v[k].c_str() : "").c_str()); }
    // USB_DEVICE_DATA_INFO from "cmd,state;n0,..;w0,..;dw0,..;d0,..;text;remark" (missing parts are 0)
    void Info(size_t k, USB_DEVICE_DATA_INFO* r)
    {
        memset(r, 0x00, sizeof(USB_DEVICE_DATA_INFO));
        std::string s = k < v.size() ? v[k] : std::string();
        std::vector<std::string> part;
        size_t p = 0, q;
        while ((q = s.find(';', p)) != std::string::npos) { part.push_back(s.substr(p, q - p)); p = q + 1; }
        part.push_back(s.substr(p));
        auto items = [](const std::string& x) { std::vector<std::string> o; size_t a = 0, b; if (x.empty()) return o; while ((b = x.find(',', a)) != std::string::npos) { o.push_back(x.substr(a, b - a)); a = b + 1; } o.push_back(x.substr(a)); return o; };
        std::vector<std::string> h = items(part[0]);
        if (h.size() > 0) r->nCommand = (int)strtol(h[0].c_str(), NULL, 0);
        if (h.size() > 1) r->nState = (int)strtol(h[1].c_str(), NULL, 0);
        if (part.size() > 1) { std::vector<std::string> x = items(part[1]); for (size_t i = 0; i < x.size() && i < MAX_USB_DATA_ARRAY; i++) r->nData[i] = (int)strtol(x[i].c_str(), NULL, 0); }
        if (part.size() > 2) { std::vector<std::string> x = items(part[2]); for (size_t i = 0; i < x.size() && i < MAX_USB_DATA_ARRAY; i++) r->wData[i] = (WORD)strtoul(x[i].c_str(), NULL, 0); }
        if (part.size() > 3) { std::vector<std::string> x = items(part[3]); for (size_t i = 0; i < x.size() && i < MAX_USB_DATA_ARRAY; i++) r->dwData[i] = (DWORD)strtoul(x[i].c_str(), NULL, 0); }
        if (part.size() > 4) { std::vector<std::string> x = items(part[4]); for (size_t i = 0; i < x.size() && i < MAX_USB_DATA_ARRAY; i++) r->dData[i] = strtod(x[i].c_str(), NULL); }
        if (part.size() > 5) strncpy_s(r->szData, sizeof(r->szData), part[5].c_str(), _TRUNCATE);
        if (part.size() > 6) strncpy_s(r->szRemark, sizeof(r->szRemark), part[6].c_str(), _TRUNCATE);
    }
    // REQUEST_USB_DEVICE_DATA from "id,type,option;n0,n1,..;d0,d1,..;text" (missing parts are 0)
    REQUEST_USB_DEVICE_DATA* Req(size_t k)
    {
        reqs.reserve(8);
        reqs.push_back(REQUEST_USB_DEVICE_DATA());
        REQUEST_USB_DEVICE_DATA* r = &reqs.back();
        memset(r, 0x00, sizeof(REQUEST_USB_DEVICE_DATA));
        std::string s = k < v.size() ? v[k] : std::string();
        std::vector<std::string> part;
        size_t p = 0, q;
        while ((q = s.find(';', p)) != std::string::npos) { part.push_back(s.substr(p, q - p)); p = q + 1; }
        part.push_back(s.substr(p));
        auto items = [](const std::string& x) { std::vector<std::string> o; size_t a = 0, b; if (x.empty()) return o; while ((b = x.find(',', a)) != std::string::npos) { o.push_back(x.substr(a, b - a)); a = b + 1; } o.push_back(x.substr(a)); return o; };
        std::vector<std::string> h = items(part[0]);
        if (h.size() > 0) r->nRequestID = (int)strtol(h[0].c_str(), NULL, 0);
        if (h.size() > 1) r->nRequestType = (int)strtol(h[1].c_str(), NULL, 0);
        if (h.size() > 2) r->nRequestOption = (int)strtol(h[2].c_str(), NULL, 0);
        if (part.size() > 1) { std::vector<std::string> n = items(part[1]); for (size_t i = 0; i < n.size() && i < REQUEST_HW_DATA_MAX_ITEM; i++) r->nRequestData[i] = (int)strtol(n[i].c_str(), NULL, 0); }
        if (part.size() > 2) { std::vector<std::string> d = items(part[2]); for (size_t i = 0; i < d.size() && i < REQUEST_HW_DATA_MAX_ITEM; i++) r->dRequestData[i] = strtod(d[i].c_str(), NULL); }
        if (part.size() > 3) strncpy_s(r->szData, sizeof(r->szData), part[3].c_str(), _TRUNCATE);
        return r;
    }
    long long Int(size_t k) const { return k < v.size() ? strtoll(v[k].c_str(), NULL, 0) : 0; }
    double Dbl(size_t k) const { return k < v.size() ? strtod(v[k].c_str(), NULL) : 0.0; }
    char* Str(size_t k)
    {
        std::string s = k < v.size() ? v[k] : std::string();
        bufs.push_back(std::vector<char>(s.begin(), s.end()));
        bufs.back().resize(s.size() + 256, '\0'); // room: some originals write into the argument
        return &bufs.back()[0];
    }
};

typedef std::function<std::string(Args&)> Fn;
struct Entry { const char* key; int argc; Fn fn; };

static std::string FormatInt(long long v) { char b[64]; sprintf(b, "%lld", v); return b; }
static std::string FormatDouble(double v) { char b[64]; sprintf(b, "%.17g", v); return b; }
#define RI(x) [](Args& a) -> std::string { return FormatInt((long long)(x)); }
#define RD(x) [](Args& a) -> std::string { return FormatDouble((double)(x)); }
#define RV(x) [](Args& a) -> std::string { x; return std::string(); }
#define DISPATCH(key, n, fn) { key, n, fn },

static Entry g_entries[] = {
#include "dispatch.inc"
#include "assist_dispatch.inc"
};

// ---- state diff ----
static void Diff(char tag, const void* p, size_t n, std::vector<unsigned char>& prev)
{
    if (!p) return;
    const unsigned char* b = (const unsigned char*)p;
    if (prev.size() != n) prev.assign(n, 0);
    size_t i = 0;
    while (i < n)
    {
        if (b[i] == prev[i]) { i++; continue; }
        size_t j = i + 1, last = i;
        while (j < n && j - last <= 4) { if (b[j] != prev[j]) last = j; j++; }
        fprintf(g_out, "%c %u", tag, (unsigned)i);
        for (size_t k = i; k <= last; k++) fprintf(g_out, " %02X", b[k]);
        fprintf(g_out, "\n");
        i = last + 1;
    }
    memcpy(&prev[0], b, n);
}

// main dialog / ModularAssistDevice state (changes only): "A name=value" for members and scan control / vacuum-beam fields,
// 'M' bytes = main dialog copy of INTEGRATED_COMMAND_STRUCT, 'L' bytes = CLIENT_PUBLIC_DATA
struct FieldDef { const char* name; char type; int count; size_t offset; };
#define FIELD(t, n) { #n, #t[0], 1, offsetof(SEM_SE_SCAN_CONTROL_PUBLIC_DATA, n) },
#define FIELDA(t, n, c) { #n, #t[0], c, offsetof(SEM_SE_SCAN_CONTROL_PUBLIC_DATA, n) },
static FieldDef g_scanFields[] = {
#include "scan_fields.inc"
};
#undef FIELD
#undef FIELDA
#define FIELD(t, n) { #n, #t[0], 1, offsetof(VACUUM_BEAM_ALIGN_FOCUS_DATA, n) },
#define FIELDA(t, n, c) { #n, #t[0], c, offsetof(VACUUM_BEAM_ALIGN_FOCUS_DATA, n) },
static FieldDef g_vbafFields[] = {
#include "VACUUM_BEAM_ALIGN_FOCUS_DATA.fields.inc"
};
#undef FIELD
#undef FIELDA

static void AddFields(std::vector<std::string>& cur, const char* prefix, const unsigned char* base, const FieldDef* f, size_t n)
{
    char b[600];
    for (size_t k = 0; k < n; k++)
        for (int i = 0; i < f[k].count; i++)
        {
            const unsigned char* p = base + f[k].offset;
            switch (f[k].type)
            {
            case 'I': sprintf(b, "%d", ((const int*)p)[i]); break;
            case 'U': sprintf(b, "%u", ((const unsigned*)p)[i]); break;
            case 'W': sprintf(b, "%u", (unsigned)((const WORD*)p)[i]); break;
            case 'B': sprintf(b, "%u", (unsigned)p[i]); break;
            case 'S': { std::string t; for (const unsigned char* q = p; *q; q++) { char e[8]; if (*q < 0x20 || *q >= 0x7F) { sprintf(e, "\\x%02X", *q); t += e; } else t += (char)*q; } snprintf(b, sizeof(b), "%s", t.c_str()); break; }
            default: sprintf(b, "%.17g", ((const double*)p)[i]); break;
            }
            cur.push_back(std::string(prefix) + f[k].name + (f[k].count > 1 ? "[" + std::to_string(i) + "]" : "") + "=" + b);
        }
}

static std::vector<std::string> g_prevAssist;
static std::vector<unsigned char> g_prevMain, g_prevClient;
static void DiffAssist()
{
    std::vector<std::string> cur;
    char b[64];
#define A(x) sprintf(b, "%d", (int)(x)); cur.push_back(std::string(#x) + "=" + b);
    A(g_assist.m_bSbrConnected) A(g_assist.m_bSemConnected) A(g_assist.m_bBseConnected) A(g_assist.m_bUsbConnected)
    A(g_assist.m_bSysInitilized) A(g_assist.m_nSeBseScanMode)
    A(g_main.m_bCLOnOffFlag) A(g_main.m_bAutoStartConfigChengeFlag) A(g_main.m_bPhotoCalibrationFlag) A(g_main.m_pdlgMessageNotice != NULL)
    A(g_main.m_pSEScanImageDataThreadParams != NULL) A(g_main.m_pSEScanImageDataProcessThread != NULL) A(g_main.m_nSpotSizeValue)
    A(g_main.m_nWDx10Position) A(g_main.m_nDialogStartStep)
#undef A
    cur.push_back("g_main.m_strDistDecimalPlaces=" + Narrow(g_main.m_strDistDecimalPlaces));
    AddFields(cur, "scan.", (const unsigned char*)&g_main.m_SemSEScanControlPublicData, g_scanFields, sizeof(g_scanFields) / sizeof(g_scanFields[0]));
    AddFields(cur, "vbaf.", (const unsigned char*)&g_main.m_VacuumBeamAlignFocusData, g_vbafFields, sizeof(g_vbafFields) / sizeof(g_vbafFields[0]));
    if (g_prevAssist.size() != cur.size()) g_prevAssist.assign(cur.size(), "");
    for (size_t i = 0; i < cur.size(); i++)
        if (cur[i] != g_prevAssist[i]) fprintf(g_out, "A %s\n", cur[i].c_str());
    g_prevAssist = cur;
    Diff('M', &g_main.m_IntegratedData, sizeof(INTEGRATED_COMMAND_STRUCT), g_prevMain);
    Diff('L', &g_main.m_ClientPublicData, sizeof(CLIENT_PUBLIC_DATA), g_prevClient);
}

static void DiffState()
{
    Diff('S', AfxGetIntegratedPointer(), sizeof(INTEGRATED_COMMAND_STRUCT), g_prevIntegrated);
    Diff('R', AfxGetScanRotationCalibrationDataPointer(), sizeof(SEM_SCAN_ROTATION_CALIBRATION_DATA), g_prevSRot);
    DiffAssist();
}

int main(int argc, char** argv)
{
    if (argc < 2) { fprintf(stderr, "usage: harness <script>\n"); return 2; }
    HMODULE fake = LoadLibraryW(L"Ftd2xx.dll");
    if (!fake) { fprintf(stderr, "Ftd2xx.dll (fake) not found\n"); return 2; }
    ((void (*)(void (*)(const char*)))GetProcAddress(fake, "Fake_SetSink"))(Sink);
    g_pushRx = (PushRxFn)GetProcAddress(fake, "Fake_PushRx");

    // ModularAssistDevice wiring of the original OnInitDialog (owner window, public data); its state starts at the constructor values
    g_main.m_pAssistDeviceTool = &g_assist;
    g_assist.SetOwner((HWND)1);
    g_assist.SetOwnerWindow(&g_main);
    g_assist.SetVacuumBeamAlignFocusPublicData(&g_main.m_VacuumBeamAlignFocusData);
    DiffAssist();

    std::map<std::string, Entry*> table;
    for (size_t i = 0; i < sizeof(g_entries) / sizeof(g_entries[0]); i++) table[g_entries[i].key] = &g_entries[i];

    FILE* f = fopen(argv[1], "r");
    if (!f) { fprintf(stderr, "cannot open %s\n", argv[1]); return 2; }
    static char line[100000];
    while (fgets(line, sizeof(line), f))
    {
        line[strcspn(line, "\r\n")] = 0;
        if ((unsigned char)line[0] == 0xEF && (unsigned char)line[1] == 0xBB && (unsigned char)line[2] == 0xBF) memmove(line, line + 3, strlen(line + 3) + 1); // UTF-8 BOM
        if (!line[0] || line[0] == '#') continue;
        std::vector<std::string> tok;
        for (char* t = strtok(line, " \t"); t; t = strtok(NULL, " \t")) tok.push_back(t);
        const std::string& cmd = tok[0];
        if (cmd == "CALL" && tok.size() >= 2)
        {
            std::string echo = "CALL";
            for (size_t i = 1; i < tok.size(); i++) echo += " " + tok[i];
            fprintf(g_out, "%s\n", echo.c_str());
            std::map<std::string, Entry*>::iterator it = table.find(tok[1]);
            if (it == table.end()) { fprintf(g_out, "NOFUNC\n"); continue; }
            Args a;
            a.v.assign(tok.begin() + 2, tok.end());
            std::string r = it->second->fn(a);
            if (!r.empty()) fprintf(g_out, "RET %s\n", r.c_str());
            DiffState();
        }
        else if (cmd == "SEREAD")
        {
            // one pass of the SE image read thread body (its parameter is a struct pointer, so it is not in dispatch.inc)
            // like one WAIT_OBJECT_0 pass of AfxThreadSEImageDataProcessing (USB_FT232H_Device.cpp:16810): copy the receive flag first
            fprintf(g_out, "CALL SEREAD\n");
            if (m_pSEImageDataThreadParams && AfxGetIntegratedPointer()) m_pSEImageDataThreadParams->bScanDataReceive = AfxGetIntegratedPointer()->bScanDataReceive;
            fprintf(g_out, "RET %d\n", AfxSEScanDataImageProcess(m_pSEImageDataThreadParams));
            DiffState();
        }
        else if (cmd == "PATH" && tok.size() >= 2) { AfxSetDeviceServerPath(CString(Widen(tok[1].c_str()).c_str())); }
        else if (cmd == "TICK" && tok.size() >= 2) { g_tick = (DWORD)strtoul(tok[1].c_str(), NULL, 0); }
        else if (cmd == "BOXRET" && tok.size() >= 2) { g_boxRet = atoi(tok[1].c_str()); }
        else if (cmd == "MAIN" && tok.size() >= 4)
        {
            // MAIN <msg> <wParam> NULL | INT <n> | REQ <id,type,opt;n..;d..;text> | INFO <cmd,state;n..;w..;dw..;d..;text;remark>
            // = SendMessage(main window, msg, wParam, lParam) like the device layer does
            std::string echo = "CALL MAIN";
            for (size_t i = 1; i < tok.size(); i++) echo += " " + tok[i];
            fprintf(g_out, "%s\n", echo.c_str());
            UINT msg = (UINT)strtoul(tok[1].c_str(), NULL, 0);
            WPARAM w = (WPARAM)strtoul(tok[2].c_str(), NULL, 0);
            Args a;
            a.v.assign(tok.begin() + 4, tok.end());
            LPARAM l = 0;
            static USB_DEVICE_DATA_INFO info;
            if (tok[3] == "INT") l = (LPARAM)a.Int(0);
            else if (tok[3] == "INTEG") l = (LPARAM)AfxGetIntegratedPointer(); // as ModularAssistDevice::UsbDeviceConnect passes it
            else if (tok[3] == "REQ") l = (LPARAM)a.Req(0);
            else if (tok[3] == "INFO") { a.Info(0, &info); l = (LPARAM)&info; }
            LRESULT r = SendMessage((HWND)1, msg, w, l);
            fprintf(g_out, "RET %d\n", (int)r);
            DiffState();
        }
        else if (cmd == "CLIENTINI" && tok.size() >= 2)
        {
            FILE* fi = fopen(tok[1].c_str(), "rb");
            if (fi) { fread(&g_main.m_ClientPublicData, 1, sizeof(CLIENT_PUBLIC_DATA), fi); fclose(fi); }
            DiffState();
        }
        else if (cmd == "AUTOBC" && tok.size() >= 2) { g_autoBCRun = atoi(tok[1].c_str()); }
        else if (cmd == "SETF" && tok.size() >= 4)
        {
            // SETF scan|vbaf <field> <value>: set a main dialog public data field (int or double fields only)
            const FieldDef* defs = tok[1] == "scan" ? g_scanFields : g_vbafFields;
            size_t n = tok[1] == "scan" ? sizeof(g_scanFields) / sizeof(g_scanFields[0]) : sizeof(g_vbafFields) / sizeof(g_vbafFields[0]);
            unsigned char* base = tok[1] == "scan" ? (unsigned char*)&g_main.m_SemSEScanControlPublicData : (unsigned char*)&g_main.m_VacuumBeamAlignFocusData;
            bool found = false;
            for (size_t k = 0; k < n && !found; k++)
            {
                if (tok[2] != defs[k].name) continue;
                found = true;
                if (defs[k].type == 'I') *(int*)(base + defs[k].offset) = (int)strtol(tok[3].c_str(), NULL, 0);
                else if (defs[k].type == 'D') *(double*)(base + defs[k].offset) = strtod(tok[3].c_str(), NULL);
                else found = false;
            }
            if (!found) fprintf(g_out, "NOFIELD %s\n", tok[2].c_str());
            DiffState();
        }
        else if (cmd == "RX" && tok.size() >= 3)
        {
            std::vector<unsigned char> b;
            for (size_t i = 2; i < tok.size(); i++) b.push_back((unsigned char)strtoul(tok[i].c_str(), NULL, 16));
            g_pushRx(atoi(tok[1].c_str()), b.empty() ? NULL : &b[0], (int)b.size());
        }
        else fprintf(g_out, "UNKNOWN %s\n", cmd.c_str());
        fflush(g_out);
    }
    fclose(f);
    return 0;
}
