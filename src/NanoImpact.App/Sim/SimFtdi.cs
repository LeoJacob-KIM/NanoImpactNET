// 장비 없이 실행 (--sim): FT232H 3개(SE 영상 ImgCap001, BSE 영상 BseICap001, Scan Bridge ScanBri001)가 열린 것처럼 하고,
// 보낸 것은 버리며, SimScanSource가 넣은 SE 영상 바이트를 장치 계층의 실제 SE 수신 스레드가 읽어 가게 한다.
using System;
using System.Collections.Generic;
using System.Threading;
using NanoImpact.Hardware.Ftdi;

namespace NanoImpact.App.Sim
{
    internal sealed class SimFtdi : IFtdi
    {
        public const int SE_IMAGE = 0;
        public const int BSE_IMAGE = 1;
        public const int SCAN_BRIDGE = 2;
        private const long MaxBacklog = 64L * 1024 * 1024; // 읽는 쪽이 느리면 버린다

        private sealed class RxQueue
        {
            public readonly Queue<byte[]> Chunks = new Queue<byte[]>();
            public int HeadOffset;
            public long Count;
            public EventWaitHandle Event;
        }

        private readonly RxQueue[] _rx = { new RxQueue(), new RxQueue(), new RxQueue() };

        /// <summary>장치 idx의 받는 쪽에 바이트를 넣는다 (다른 스레드에서 불러도 된다). 밀린 양이 너무 많으면 버리고 false.</summary>
        public bool PushRx(int idx, byte[] data)
        {
            if (idx < 0 || idx > 2 || data == null || data.Length == 0) return true;
            RxQueue q = _rx[idx];
            EventWaitHandle ev;
            lock (q)
            {
                if (q.Count > MaxBacklog) return false;
                q.Chunks.Enqueue(data);
                q.Count += data.Length;
                ev = q.Event;
            }
            ev?.Set();
            return true;
        }

        public void ClearRx(int idx)
        {
            RxQueue q = _rx[idx];
            lock (q)
            {
                q.Chunks.Clear();
                q.HeadOffset = 0;
                q.Count = 0;
            }
        }

        private static int IndexOf(IntPtr h) => (int)h - 1;
        private RxQueue Q(IntPtr h)
        {
            int i = IndexOf(h);
            return i >= 0 && i < 3 ? _rx[i] : null;
        }

        public bool Load() => true;

        public uint FT_CreateDeviceInfoList(out uint numDevs)
        {
            numDevs = 3;
            return 0;
        }

        public uint FT_GetDeviceInfoList(uint numDevs) => 0;

        public uint FT_OpenEx(string serialNumber, uint flags, out IntPtr handle)
        {
            int idx = serialNumber == "ImgCap001" ? SE_IMAGE : serialNumber == "BseICap001" ? BSE_IMAGE : serialNumber == "ScanBri001" ? SCAN_BRIDGE : -1;
            handle = idx < 0 ? IntPtr.Zero : new IntPtr(idx + 1);
            return idx < 0 ? 2u : 0u; // FT_DEVICE_NOT_FOUND
        }

        public uint FT_Close(IntPtr handle) => 0;

        public uint FT_Read(IntPtr handle, byte[] buffer, uint bytesToRead, out uint bytesRead)
        {
            bytesRead = 0;
            RxQueue q = Q(handle);
            if (q == null) return 0;
            lock (q)
            {
                uint n = 0;
                while (n < bytesToRead && q.Chunks.Count > 0)
                {
                    byte[] head = q.Chunks.Peek();
                    int take = (int)Math.Min(bytesToRead - n, (uint)(head.Length - q.HeadOffset));
                    Buffer.BlockCopy(head, q.HeadOffset, buffer, (int)n, take);
                    n += (uint)take;
                    q.HeadOffset += take;
                    if (q.HeadOffset >= head.Length)
                    {
                        q.Chunks.Dequeue();
                        q.HeadOffset = 0;
                    }
                }
                q.Count -= n;
                bytesRead = n;
            }
            return 0;
        }

        public uint FT_Write(IntPtr handle, byte[] buffer, uint bytesToWrite, out uint bytesWritten)
        {
            bytesWritten = bytesToWrite;
            return 0;
        }

        public uint FT_ResetDevice(IntPtr handle) => 0;
        public uint FT_Purge(IntPtr handle, uint mask) => 0;
        public uint FT_SetTimeouts(IntPtr handle, uint readTimeout, uint writeTimeout) => 0;

        public uint FT_GetStatus(IntPtr handle, out uint rxBytes, out uint txBytes, out uint eventStatus)
        {
            FT_GetQueueStatus(handle, out rxBytes);
            txBytes = 0;
            eventStatus = 0;
            return 0;
        }

        public uint FT_GetQueueStatus(IntPtr handle, out uint rxBytes)
        {
            RxQueue q = Q(handle);
            if (q == null) rxBytes = 0;
            else lock (q) rxBytes = (uint)Math.Min(q.Count, uint.MaxValue);
            return 0;
        }

        public uint FT_SetBitMode(IntPtr handle, byte mask, byte mode) => 0;
        public uint FT_SetLatencyTimer(IntPtr handle, byte latencyMs) => 0;
        public uint FT_SetUSBParameters(IntPtr handle, uint inTransferSize, uint outTransferSize) => 0;
        public uint FT_SetFlowControl(IntPtr handle, ushort flowControl, byte xon, byte xoff) => 0;

        public uint FT_SetEventNotification(IntPtr handle, uint eventMask, EventWaitHandle evt)
        {
            RxQueue q = Q(handle);
            if (q != null) lock (q) q.Event = evt;
            return 0;
        }
    }
}
