using System;
using System.Runtime.InteropServices;

namespace NanoImpact.Hardware.Ftdi
{
    /// <summary>
    /// ftd2xx.dll 직접 호출 (원본: ftd2xx.h + USB_FT232H_Device.cpp의 AfxFT232H* 래퍼).
    /// 원본은 LoadLibrary/GetProcAddress로 불렀지만 같은 DLL·같은 함수다. x86에서는 WINAPI(stdcall).
    /// </summary>
    internal static class Ftd2xx
    {
        private const string Dll = "ftd2xx.dll";

        public const uint FT_OK = 0;
        public const uint FT_OPEN_BY_SERIAL_NUMBER = 1;
        public const ushort FT_FLOW_RTS_CTS = 0x0100;
        public const uint FT_PURGE_RX = 1;
        public const uint FT_PURGE_TX = 2;
        public const uint FT_EVENT_RXCHAR = 1;

        [DllImport(Dll)] public static extern uint FT_CreateDeviceInfoList(out uint numDevs);
        [DllImport(Dll, CharSet = CharSet.Ansi)] public static extern uint FT_OpenEx(string serialNumber, uint flags, out IntPtr handle);
        [DllImport(Dll)] public static extern uint FT_Close(IntPtr handle);
        [DllImport(Dll)] public static extern uint FT_Read(IntPtr handle, byte[] buffer, uint bytesToRead, out uint bytesRead);
        [DllImport(Dll)] public static extern uint FT_Write(IntPtr handle, byte[] buffer, uint bytesToWrite, out uint bytesWritten);
        [DllImport(Dll)] public static extern uint FT_ResetDevice(IntPtr handle);
        [DllImport(Dll)] public static extern uint FT_Purge(IntPtr handle, uint mask);
        [DllImport(Dll)] public static extern uint FT_SetTimeouts(IntPtr handle, uint readTimeout, uint writeTimeout);
        [DllImport(Dll)] public static extern uint FT_GetStatus(IntPtr handle, out uint rxBytes, out uint txBytes, out uint eventStatus);
        [DllImport(Dll)] public static extern uint FT_GetQueueStatus(IntPtr handle, out uint rxBytes);
        [DllImport(Dll)] public static extern uint FT_SetBitMode(IntPtr handle, byte mask, byte mode);
        [DllImport(Dll)] public static extern uint FT_SetLatencyTimer(IntPtr handle, byte latencyMs);
        [DllImport(Dll)] public static extern uint FT_SetUSBParameters(IntPtr handle, uint inTransferSize, uint outTransferSize);
        [DllImport(Dll)] public static extern uint FT_SetFlowControl(IntPtr handle, ushort flowControl, byte xon, byte xoff);
        [DllImport(Dll)] public static extern uint FT_SetEventNotification(IntPtr handle, uint eventMask, IntPtr eventHandle);
    }
}
