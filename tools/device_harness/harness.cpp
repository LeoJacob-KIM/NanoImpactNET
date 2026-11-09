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

static void PrintMessage(const char* tag, HWND h, UINT msg, WPARAM w, LPARAM l)
{
    // lParam is often a pointer to a stack struct: print only whether it is a pointer
    if ((ULONG_PTR)l < 0x10000) fprintf(g_out, "%s %d %u %u %d\n", tag, (int)(INT_PTR)h, msg, (unsigned)w, (int)l);
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

// ---- dispatch ----
struct Args
{
    std::vector<std::string> v;
    std::vector<std::vector<char> > bufs;
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

static void DiffState()
{
    Diff('S', AfxGetIntegratedPointer(), sizeof(INTEGRATED_COMMAND_STRUCT), g_prevIntegrated);
    Diff('R', AfxGetScanRotationCalibrationDataPointer(), sizeof(SEM_SCAN_ROTATION_CALIBRATION_DATA), g_prevSRot);
}

int main(int argc, char** argv)
{
    if (argc < 2) { fprintf(stderr, "usage: harness <script>\n"); return 2; }
    HMODULE fake = LoadLibraryW(L"Ftd2xx.dll");
    if (!fake) { fprintf(stderr, "Ftd2xx.dll (fake) not found\n"); return 2; }
    ((void (*)(void (*)(const char*)))GetProcAddress(fake, "Fake_SetSink"))(Sink);
    g_pushRx = (PushRxFn)GetProcAddress(fake, "Fake_PushRx");

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
