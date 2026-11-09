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

LRESULT WINAPI HarnessSendMessage(HWND h, UINT msg, WPARAM w, LPARAM l) { PrintMessage("MSG", h, msg, w, l); return 0; }
BOOL WINAPI HarnessPostMessage(HWND h, UINT msg, WPARAM w, LPARAM l) { PrintMessage("POST", h, msg, w, l); return TRUE; }
void WINAPI HarnessSleep(DWORD ms) { fprintf(g_out, "SLEEP %u\n", ms); }
DWORD WINAPI HarnessGetTickCount(void) { return g_tick; }

CWinThread* AfxBeginThread(AFX_THREADPROC, LPVOID, int, UINT, DWORD, LPSECURITY_ATTRIBUTES)
{
    fprintf(g_out, "THREAD\n");
    return new CWinThread();
}

// ---- ModularAssistDevice (ModularAssistDevice.cpp:10-3914 pasted unmodified as assist_methods.inc) ----
static void PrintRequest(const REQUEST_USB_DEVICE_DATA* r)
{
    // REQ id type option n=... d=... sz=...  (trailing zero items left out)
    fprintf(g_out, " REQ %d %d %d n=", r->nRequestID, r->nRequestType, r->nRequestOption);
    int ln = REQUEST_HW_DATA_MAX_ITEM, ld = REQUEST_HW_DATA_MAX_ITEM;
    while (ln > 0 && r->nRequestData[ln - 1] == 0) ln--;
    while (ld > 0 && r->dRequestData[ld - 1] == 0.0) ld--;
    for (int i = 0; i < ln; i++) fprintf(g_out, "%s%d", i ? "," : "", r->nRequestData[i]);
    fprintf(g_out, " d=");
    for (int i = 0; i < ld; i++) fprintf(g_out, "%s%.17g", i ? "," : "", r->dRequestData[i]);
    fprintf(g_out, " sz=%s\n", r->szData);
}

// The main dialog functions ModularAssistDevice calls through m_pOwnerWindow (ported in step 6b): recorded here.
class CNanoImpact2020cfeDlg : public CWnd
{
public:
    LRESULT OnSetUsbDeviceSEMControlDataToDeviceServer(WPARAM w, LPARAM l)
    {
        fprintf(g_out, "OWNER OnSetUsbDeviceSEMControlDataToDeviceServer %u", (unsigned)w);
        PrintRequest((const REQUEST_USB_DEVICE_DATA*)l);
        return 1;
    }
    LRESULT OnSetUsbDeviceSEMControlDataToFlowControl(WPARAM w, LPARAM l)
    {
        fprintf(g_out, "OWNER OnSetUsbDeviceSEMControlDataToFlowControl %u", (unsigned)w);
        PrintRequest((const REQUEST_USB_DEVICE_DATA*)l);
        return 1;
    }
    LRESULT OnSemControlIntegratedData(WPARAM w, LPARAM l) { fprintf(g_out, "OWNER OnSemControlIntegratedData %u %s\n", (unsigned)w, l ? "P" : "0"); return 1; }
    LRESULT OnSemStageUsbControlData(WPARAM w, LPARAM l) { fprintf(g_out, "OWNER OnSemStageUsbControlData %u %s\n", (unsigned)w, l ? "P" : "0"); return 1; }
    void SetWDSetupEnabledOLValue(int n) { fprintf(g_out, "OWNER SetWDSetupEnabledOLValue %d\n", n); }
};

int AfxMessageBox(LPCTSTR text, UINT type = MB_OK, UINT = 0) { return HarnessMessageBox(NULL, text, _T(""), type); }

#include "ModularAssistDevice.h"
#include "assist_methods.inc"

static CModularAssistDevice g_assist;
static CNanoImpact2020cfeDlg g_owner;
static VACUUM_BEAM_ALIGN_FOCUS_DATA g_vbaf;

// ---- dispatch ----
struct Args
{
    std::vector<std::string> v;
    std::vector<std::vector<char> > bufs;
    std::vector<REQUEST_USB_DEVICE_DATA> reqs;
    CString CStr(size_t k) const { return CString(Widen(k < v.size() ? v[k].c_str() : "").c_str()); }
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

// ModularAssistDevice members and the two VACUUM_BEAM_ALIGN_FOCUS_DATA fields it writes (changes only)
static std::vector<std::string> g_prevAssist;
static void DiffAssist()
{
    std::vector<std::string> cur;
    char b[64];
#define A(x) sprintf(b, "%d", (int)(x)); cur.push_back(std::string(#x) + "=" + b);
    A(g_assist.m_bSbrConnected) A(g_assist.m_bSemConnected) A(g_assist.m_bBseConnected) A(g_assist.m_bUsbConnected)
    A(g_assist.m_bSysInitilized) A(g_assist.m_nSeBseScanMode) A(g_vbaf.m_nChamberState) A(g_vbaf.m_nGunValveState)
#undef A
    if (g_prevAssist.size() != cur.size()) g_prevAssist.assign(cur.size(), "");
    for (size_t i = 0; i < cur.size(); i++)
        if (cur[i] != g_prevAssist[i]) fprintf(g_out, "A %s\n", cur[i].c_str());
    g_prevAssist = cur;
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
    memset(&g_vbaf, 0x00, sizeof(VACUUM_BEAM_ALIGN_FOCUS_DATA));
    g_assist.SetOwner((HWND)1);
    g_assist.SetOwnerWindow(&g_owner);
    g_assist.SetVacuumBeamAlignFocusPublicData(&g_vbaf);
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
