// Minimal stand-in for MFC so that the original USB_FT232H_Device.cpp compiles unmodified
// (this PC has no MFC headers). Keep ASCII only: compiled with /source-charset:.949.
#pragma once
#define _CRT_SECURE_NO_WARNINGS
#define _WINSOCK_DEPRECATED_NO_WARNINGS
#define WIN32_LEAN_AND_MEAN
#include <winsock2.h>
#include <windows.h>
#include <tchar.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <math.h>
#include <string>

// ---- harness redirections: recorded instead of reaching Windows ----
int WINAPI HarnessMessageBox(HWND hWnd, LPCTSTR text, LPCTSTR caption, UINT type);
LRESULT WINAPI HarnessSendMessage(HWND hWnd, UINT msg, WPARAM w, LPARAM l);
BOOL WINAPI HarnessPostMessage(HWND hWnd, UINT msg, WPARAM w, LPARAM l);
void WINAPI HarnessSleep(DWORD ms);
DWORD WINAPI HarnessGetTickCount(void);
#undef MessageBox
#define MessageBox HarnessMessageBox
#undef SendMessage
#define SendMessage HarnessSendMessage
#undef PostMessage
#define PostMessage HarnessPostMessage
#define Sleep HarnessSleep
#define GetTickCount HarnessGetTickCount

// ---- MFC types used by USB_FT232H_Device.cpp ----
class CString
{
    std::basic_string<TCHAR> s;
public:
    CString() {}
    CString(const TCHAR* p) : s(p ? p : _T("")) {}
    CString& operator=(const TCHAR* p) { s = p ? p : _T(""); return *this; }
    CString operator+(const CString& o) const { CString r; r.s = s + o.s; return r; }
    CString operator+(const TCHAR* p) const { CString r; r.s = s + (p ? p : _T("")); return r; }
    friend CString operator+(const TCHAR* p, const CString& o) { CString r; r.s = std::basic_string<TCHAR>(p ? p : _T("")) + o.s; return r; }
    operator LPCTSTR() const { return s.c_str(); }
    int GetLength() const { return (int)s.size(); }
};

class CFile
{
    FILE* f;
public:
    enum { modeRead = 0x0000, modeWrite = 0x0001, modeReadWrite = 0x0002, modeCreate = 0x1000 };
    CFile() : f(NULL) {}
    ~CFile() { Close(); }
    BOOL Open(LPCTSTR name, UINT flags)
    {
        Close();
        const TCHAR* mode = (flags & modeCreate) ? _T("w+b") : (flags & (modeWrite | modeReadWrite)) ? _T("r+b") : _T("rb");
        f = _tfopen(name, mode);
        return f != NULL;
    }
    void Close() { if (f) { fclose(f); f = NULL; } }
    UINT Read(void* p, UINT n) { return f ? (UINT)fread(p, 1, n, f) : 0; }
    void Write(const void* p, UINT n) { if (f) fwrite(p, 1, n, f); }
    ULONGLONG GetLength() { if (!f) return 0; long cur = ftell(f); fseek(f, 0, SEEK_END); long len = ftell(f); fseek(f, cur, SEEK_SET); return (ULONGLONG)len; }
};

class CWinThread { public: HANDLE m_hThread; CWinThread() : m_hThread(NULL) {} };
typedef UINT (*AFX_THREADPROC)(LPVOID);
// Threads are never started in the harness: calls are driven one by one from the script.
CWinThread* AfxBeginThread(AFX_THREADPROC proc, LPVOID param, int priority = 0, UINT stack = 0, DWORD flags = 0, LPSECURITY_ATTRIBUTES sa = NULL);

// ---- empty stand-ins for MFC types that DefineStructCode.h uses in runtime-only structs ----
class CBrush { void* p; };
class CPen { void* p; };
class CFont { void* p; };
class CDC { void* p; };
class CStatic { void* p; };
class CWnd { void* p; };
class CVideoGrabber { void* p; };
struct CSize { LONG cx, cy; CSize(int x = 0, int y = 0) : cx(x), cy(y) {} };
