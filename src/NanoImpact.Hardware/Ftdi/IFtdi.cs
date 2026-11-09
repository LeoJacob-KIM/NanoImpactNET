using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace NanoImpact.Hardware.Ftdi
{
    /// <summary>
    /// ftd2xx 함수들 (원본 AfxUSBFT232HLoadDLL이 GetProcAddress로 잡던 18개 중 쓰는 것).
    /// 실제 구현 <see cref="Ftd2xxApi"/>, 원본 대조 시험은 기록하는 가짜를 넣는다.
    /// </summary>
    public interface IFtdi
    {
        /// <summary>DLL이 있는지 (원본 LoadLibrary 성공 여부)</summary>
        bool Load();

        uint FT_CreateDeviceInfoList(out uint numDevs);
        uint FT_GetDeviceInfoList(uint numDevs);
        uint FT_OpenEx(string serialNumber, uint flags, out IntPtr handle);
        uint FT_Close(IntPtr handle);
        uint FT_Read(IntPtr handle, byte[] buffer, uint bytesToRead, out uint bytesRead);
        uint FT_Write(IntPtr handle, byte[] buffer, uint bytesToWrite, out uint bytesWritten);
        uint FT_ResetDevice(IntPtr handle);
        uint FT_Purge(IntPtr handle, uint mask);
        uint FT_SetTimeouts(IntPtr handle, uint readTimeout, uint writeTimeout);
        uint FT_GetStatus(IntPtr handle, out uint rxBytes, out uint txBytes, out uint eventStatus);
        uint FT_GetQueueStatus(IntPtr handle, out uint rxBytes);
        uint FT_SetBitMode(IntPtr handle, byte mask, byte mode);
        uint FT_SetLatencyTimer(IntPtr handle, byte latencyMs);
        uint FT_SetUSBParameters(IntPtr handle, uint inTransferSize, uint outTransferSize);
        uint FT_SetFlowControl(IntPtr handle, ushort flowControl, byte xon, byte xoff);
        uint FT_SetEventNotification(IntPtr handle, uint eventMask, EventWaitHandle evt);
    }

    /// <summary>실제 ftd2xx.dll</summary>
    public sealed class Ftd2xxApi : IFtdi
    {
        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr LoadLibrary(string name);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private struct FT_DEVICE_LIST_INFO_NODE
        {
            public uint Flags, Type, ID, LocId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)] public string SerialNumber;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)] public string Description;
            public IntPtr ftHandle;
        }

        [DllImport("ftd2xx.dll")] private static extern uint FT_GetDeviceInfoList([Out] FT_DEVICE_LIST_INFO_NODE[] dest, ref uint numDevs);

        public bool Load() => LoadLibrary("Ftd2xx.dll") != IntPtr.Zero;

        public uint FT_CreateDeviceInfoList(out uint numDevs) => Ftd2xx.FT_CreateDeviceInfoList(out numDevs);

        public uint FT_GetDeviceInfoList(uint numDevs)
        {
            var nodes = new FT_DEVICE_LIST_INFO_NODE[Math.Max(1, numDevs)];
            return FT_GetDeviceInfoList(nodes, ref numDevs);
        }

        public uint FT_OpenEx(string serialNumber, uint flags, out IntPtr handle) => Ftd2xx.FT_OpenEx(serialNumber, flags, out handle);
        public uint FT_Close(IntPtr handle) => Ftd2xx.FT_Close(handle);
        public uint FT_Read(IntPtr handle, byte[] buffer, uint bytesToRead, out uint bytesRead) => Ftd2xx.FT_Read(handle, buffer, bytesToRead, out bytesRead);
        public uint FT_Write(IntPtr handle, byte[] buffer, uint bytesToWrite, out uint bytesWritten) => Ftd2xx.FT_Write(handle, buffer, bytesToWrite, out bytesWritten);
        public uint FT_ResetDevice(IntPtr handle) => Ftd2xx.FT_ResetDevice(handle);
        public uint FT_Purge(IntPtr handle, uint mask) => Ftd2xx.FT_Purge(handle, mask);
        public uint FT_SetTimeouts(IntPtr handle, uint readTimeout, uint writeTimeout) => Ftd2xx.FT_SetTimeouts(handle, readTimeout, writeTimeout);
        public uint FT_GetStatus(IntPtr handle, out uint rxBytes, out uint txBytes, out uint eventStatus) => Ftd2xx.FT_GetStatus(handle, out rxBytes, out txBytes, out eventStatus);
        public uint FT_GetQueueStatus(IntPtr handle, out uint rxBytes) => Ftd2xx.FT_GetQueueStatus(handle, out rxBytes);
        public uint FT_SetBitMode(IntPtr handle, byte mask, byte mode) => Ftd2xx.FT_SetBitMode(handle, mask, mode);
        public uint FT_SetLatencyTimer(IntPtr handle, byte latencyMs) => Ftd2xx.FT_SetLatencyTimer(handle, latencyMs);
        public uint FT_SetUSBParameters(IntPtr handle, uint inTransferSize, uint outTransferSize) => Ftd2xx.FT_SetUSBParameters(handle, inTransferSize, outTransferSize);
        public uint FT_SetFlowControl(IntPtr handle, ushort flowControl, byte xon, byte xoff) => Ftd2xx.FT_SetFlowControl(handle, flowControl, xon, xoff);
        public uint FT_SetEventNotification(IntPtr handle, uint eventMask, EventWaitHandle evt) => Ftd2xx.FT_SetEventNotification(handle, eventMask, evt.SafeWaitHandle.DangerousGetHandle());
    }
}
