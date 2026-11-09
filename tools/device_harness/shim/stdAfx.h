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

// ---- MFC types used by the original sources ----
// Like MFC's CString the object is a single pointer to the characters, so the original code can pass a CString
// straight to a "%s" varargs Format (strText.Format(_T("%s"), (CString)pszData)).
class CString
{
    TCHAR* p;
    void Set(const TCHAR* s)
    {
        size_t n = s ? _tcslen(s) : 0;
        TCHAR* q = new TCHAR[n + 1];
        if (n) memcpy(q, s, n * sizeof(TCHAR));
        q[n] = 0;
        delete[] p;
        p = q;
    }
public:
    CString() : p(NULL) { Set(NULL); }
    CString(const TCHAR* s) : p(NULL) { Set(s); }
#ifdef UNICODE
    // MFC converts char* with the ANSI code page
    CString(const char* s) : p(NULL)
    {
        int n = s ? MultiByteToWideChar(CP_ACP, 0, s, -1, NULL, 0) : 0;
        if (n <= 1) { Set(NULL); return; }
        wchar_t* w = new wchar_t[n];
        MultiByteToWideChar(CP_ACP, 0, s, -1, w, n);
        Set(w);
        delete[] w;
    }
    bool operator==(const char* s) const { return *this == CString(s); }
#endif
    CString(const CString& o) : p(NULL) { Set(o.p); }
    ~CString() { delete[] p; }
    CString& operator=(const CString& o) { if (this != &o) Set(o.p); return *this; }
    CString& operator=(const TCHAR* s) { Set(s); return *this; }
    bool operator==(const TCHAR* s) const { return _tcscmp(p, s ? s : _T("")) == 0; }
    bool operator==(const CString& o) const { return _tcscmp(p, o.p) == 0; }
    CString operator+(const CString& o) const { std::basic_string<TCHAR> r(p); r += o.p; return CString(r.c_str()); }
    CString operator+(const TCHAR* s) const { std::basic_string<TCHAR> r(p); r += s ? s : _T(""); return CString(r.c_str()); }
    friend CString operator+(const TCHAR* s, const CString& o) { std::basic_string<TCHAR> r(s ? s : _T("")); r += o.p; return CString(r.c_str()); }
    operator LPCTSTR() const { return p; }
    int GetLength() const { return (int)_tcslen(p); }
    void Format(LPCTSTR fmt, ...)
    {
        TCHAR buf[4096];
        va_list ap;
        va_start(ap, fmt);
        _vsntprintf_s(buf, _countof(buf), _TRUNCATE, fmt, ap);
        va_end(ap);
        Set(buf);
    }
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
