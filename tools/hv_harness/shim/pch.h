// Spellman harness stand-in for the original precompiled header: the device harness MFC shim, plus the MFC classes
// that USB_RS232_Comm.cpp uses and fake Win32 serial port functions (defined in harness.cpp, logged like
// NanoImpact.Verify's fake ISerialCommApi). Keep ASCII only.
#pragma once
#include "stdAfx.h"
#include <shlwapi.h>

// ---- MFC stand-ins used by USB_RS232_Comm.h/.cpp ----
class CObject { public: virtual ~CObject() {} };
class CCmdTarget : public CObject {};
#define DECLARE_DYNAMIC(c)
#define IMPLEMENT_DYNAMIC(c, b)
#define DECLARE_MESSAGE_MAP()
#define BEGIN_MESSAGE_MAP(c, b)
#define END_MESSAGE_MAP()
class CSyncObject { public: virtual ~CSyncObject() {} };
class CEvent : public CSyncObject
{
public:
    CEvent(BOOL = FALSE, BOOL = FALSE) {}
    BOOL SetEvent() { return TRUE; }
    BOOL ResetEvent() { return TRUE; }
};
class CSingleLock
{
public:
    CSingleLock(CSyncObject*, BOOL = FALSE) {}
    BOOL Lock(DWORD = INFINITE) { return TRUE; }
    BOOL Unlock() { return TRUE; }
};

// ---- fake serial port API ----
HANDLE WINAPI HvCreateFile(LPCTSTR name, DWORD access, DWORD share, LPSECURITY_ATTRIBUTES sa, DWORD disp, DWORD flags, HANDLE tmpl);
BOOL WINAPI HvSetupComm(HANDLE h, DWORD inQ, DWORD outQ);
BOOL WINAPI HvPurgeComm(HANDLE h, DWORD flags);
BOOL WINAPI HvSetCommTimeouts(HANDLE h, LPCOMMTIMEOUTS t);
BOOL WINAPI HvGetCommState(HANDLE h, LPDCB dcb);
BOOL WINAPI HvSetCommState(HANDLE h, LPDCB dcb);
BOOL WINAPI HvSetCommMask(HANDLE h, DWORD mask);
BOOL WINAPI HvWaitCommEvent(HANDLE h, LPDWORD mask, LPOVERLAPPED ov);
BOOL WINAPI HvClearCommError(HANDLE h, LPDWORD errors, LPCOMSTAT stat);
BOOL WINAPI HvReadFile(HANDLE h, LPVOID buf, DWORD n, LPDWORD read, LPOVERLAPPED ov);
BOOL WINAPI HvWriteFile(HANDLE h, LPCVOID buf, DWORD n, LPDWORD written, LPOVERLAPPED ov);
BOOL WINAPI HvGetOverlappedResult(HANDLE h, LPOVERLAPPED ov, LPDWORD n, BOOL wait);
BOOL WINAPI HvEscapeCommFunction(HANDLE h, DWORD func);
BOOL WINAPI HvCloseHandle(HANDLE h);
HANDLE WINAPI HvCreateEvent(LPSECURITY_ATTRIBUTES sa, BOOL manual, BOOL initial, LPCTSTR name);
#undef CreateFile
#define CreateFile HvCreateFile
#define SetupComm HvSetupComm
#define PurgeComm HvPurgeComm
#define SetCommTimeouts HvSetCommTimeouts
#define GetCommState HvGetCommState
#define SetCommState HvSetCommState
#define SetCommMask HvSetCommMask
#define WaitCommEvent HvWaitCommEvent
#define ClearCommError HvClearCommError
#define ReadFile HvReadFile
#define WriteFile HvWriteFile
#define GetOverlappedResult HvGetOverlappedResult
#define EscapeCommFunction HvEscapeCommFunction
#define CloseHandle HvCloseHandle
#undef CreateEvent
#define CreateEvent HvCreateEvent
