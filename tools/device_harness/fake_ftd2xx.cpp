// Fake Ftd2xx.dll for the device harness. The original code loads "Ftd2xx.dll" with LoadLibrary;
// this one sits next to harness.exe, records every write/setting, and returns queued receive bytes.
// Keep ASCII only.
#define FTD2XX_EXPORTS
#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#include <stdio.h>
#include <string.h>
#include <deque>
#include "ftd2xx.h"

typedef void (*SinkFn)(const char*);
static SinkFn g_sink = NULL;
static std::deque<unsigned char> g_rx[3];

static void Emit(const char* s) { if (g_sink) g_sink(s); }

static int IndexOf(FT_HANDLE h) { return (int)(INT_PTR)h - 1; }

static void EmitBytes(char tag, int idx, const unsigned char* p, DWORD n)
{
    static char buf[300000];
    int k = sprintf(buf, "%c %d", tag, idx);
    for (DWORD i = 0; i < n && k < (int)sizeof(buf) - 8; i++) k += sprintf(buf + k, " %02X", p[i]);
    sprintf(buf + k, "\n");
    Emit(buf);
}

static void EmitCtl(int idx, const char* what, unsigned a, unsigned b, unsigned c)
{
    char buf[200];
    sprintf(buf, "C %d %s %X %X %X\n", idx, what, a, b, c);
    Emit(buf);
}

extern "C" __declspec(dllexport) void Fake_SetSink(SinkFn fn) { g_sink = fn; }

extern "C" __declspec(dllexport) void Fake_PushRx(int idx, const unsigned char* p, int n)
{
    if (idx < 0 || idx > 2) return;
    for (int i = 0; i < n; i++) g_rx[idx].push_back(p[i]);
}

FT_STATUS WINAPI FT_CreateDeviceInfoList(LPDWORD lpdwNumDevs) { *lpdwNumDevs = 3; return FT_OK; }

FT_STATUS WINAPI FT_GetDeviceInfoList(FT_DEVICE_LIST_INFO_NODE* pDest, LPDWORD lpdwNumDevs)
{
    if (pDest) memset(pDest, 0, sizeof(FT_DEVICE_LIST_INFO_NODE) * *lpdwNumDevs);
    return FT_OK;
}

FT_STATUS WINAPI FT_Open(int deviceNumber, FT_HANDLE* pHandle) { *pHandle = (FT_HANDLE)(INT_PTR)(deviceNumber + 1); return FT_OK; }

FT_STATUS WINAPI FT_OpenEx(PVOID pArg1, DWORD Flags, FT_HANDLE* pHandle)
{
    const char* serial = (const char*)pArg1;
    int idx = !strcmp(serial, "ImgCap001") ? 0 : !strcmp(serial, "BseICap001") ? 1 : !strcmp(serial, "ScanBri001") ? 2 : -1;
    char buf[200];
    sprintf(buf, "C %d FT_OpenEx %s %X\n", idx, serial, Flags);
    Emit(buf);
    if (idx < 0) return FT_DEVICE_NOT_FOUND;
    *pHandle = (FT_HANDLE)(INT_PTR)(idx + 1);
    return FT_OK;
}

FT_STATUS WINAPI FT_ListDevices(PVOID pArg1, PVOID pArg2, DWORD Flags) { return FT_OK; }
FT_STATUS WINAPI FT_Close(FT_HANDLE h) { EmitCtl(IndexOf(h), "FT_Close", 0, 0, 0); return FT_OK; }

FT_STATUS WINAPI FT_Read(FT_HANDLE h, LPVOID lpBuffer, DWORD dwBytesToRead, LPDWORD lpBytesReturned)
{
    int idx = IndexOf(h);
    DWORD n = 0;
    unsigned char* p = (unsigned char*)lpBuffer;
    while (n < dwBytesToRead && idx >= 0 && idx < 3 && !g_rx[idx].empty()) { p[n++] = g_rx[idx].front(); g_rx[idx].pop_front(); }
    *lpBytesReturned = n;
    return FT_OK;
}

FT_STATUS WINAPI FT_Write(FT_HANDLE h, LPVOID lpBuffer, DWORD dwBytesToWrite, LPDWORD lpBytesWritten)
{
    EmitBytes('W', IndexOf(h), (const unsigned char*)lpBuffer, dwBytesToWrite);
    *lpBytesWritten = dwBytesToWrite;
    return FT_OK;
}

FT_STATUS WINAPI FT_ResetDevice(FT_HANDLE h) { EmitCtl(IndexOf(h), "FT_ResetDevice", 0, 0, 0); return FT_OK; }
FT_STATUS WINAPI FT_Purge(FT_HANDLE h, ULONG Mask) { EmitCtl(IndexOf(h), "FT_Purge", Mask, 0, 0); return FT_OK; }
FT_STATUS WINAPI FT_SetTimeouts(FT_HANDLE h, ULONG r, ULONG w) { EmitCtl(IndexOf(h), "FT_SetTimeouts", r, w, 0); return FT_OK; }

FT_STATUS WINAPI FT_GetStatus(FT_HANDLE h, DWORD* rx, DWORD* tx, DWORD* ev)
{
    int idx = IndexOf(h);
    *rx = (idx >= 0 && idx < 3) ? (DWORD)g_rx[idx].size() : 0;
    *tx = 0;
    *ev = 0;
    return FT_OK;
}

FT_STATUS WINAPI FT_GetQueueStatus(FT_HANDLE h, DWORD* rx)
{
    int idx = IndexOf(h);
    *rx = (idx >= 0 && idx < 3) ? (DWORD)g_rx[idx].size() : 0;
    return FT_OK;
}

FT_STATUS WINAPI FT_SetBitMode(FT_HANDLE h, UCHAR mask, UCHAR mode) { EmitCtl(IndexOf(h), "FT_SetBitMode", mask, mode, 0); return FT_OK; }
FT_STATUS WINAPI FT_SetLatencyTimer(FT_HANDLE h, UCHAR ms) { EmitCtl(IndexOf(h), "FT_SetLatencyTimer", ms, 0, 0); return FT_OK; }
FT_STATUS WINAPI FT_SetUSBParameters(FT_HANDLE h, ULONG in, ULONG out) { EmitCtl(IndexOf(h), "FT_SetUSBParameters", in, out, 0); return FT_OK; }
FT_STATUS WINAPI FT_SetFlowControl(FT_HANDLE h, USHORT flow, UCHAR xon, UCHAR xoff) { EmitCtl(IndexOf(h), "FT_SetFlowControl", flow, xon, xoff); return FT_OK; }
FT_STATUS WINAPI FT_SetEventNotification(FT_HANDLE h, DWORD mask, PVOID param) { EmitCtl(IndexOf(h), "FT_SetEventNotification", mask, 0, 0); return FT_OK; }
