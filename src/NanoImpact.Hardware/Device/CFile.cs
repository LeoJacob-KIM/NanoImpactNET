using System;
using System.IO;
using NanoImpact.Core.Native;

namespace NanoImpact.Hardware.Device
{
    /// <summary>
    /// 원본이 설정 파일(*.INI 구조체 덤프)에 쓰던 MFC CFile의 같은 동작.
    /// modeCreate = 새로 만들고 비움, modeWrite = 있는 파일을 처음부터 덮어씀(자르지 않음), modeRead = 읽기.
    /// </summary>
    public sealed class CFile : IDisposable
    {
        public const uint modeRead = 0x0000;
        public const uint modeWrite = 0x0001;
        public const uint modeReadWrite = 0x0002;
        public const uint modeCreate = 0x1000;

        private FileStream _f;

        public bool Open(string fileName, uint flags)
        {
            Close();
            try
            {
                if ((flags & modeCreate) != 0) _f = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
                else if ((flags & (modeWrite | modeReadWrite)) != 0) _f = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
                else _f = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                return true;
            }
            catch (IOException) { return false; }
            catch (UnauthorizedAccessException) { return false; }
        }

        public void Close()
        {
            _f?.Dispose();
            _f = null;
        }

        public void Dispose() => Close();

        public ulong GetLength() => _f == null ? 0 : (ulong)_f.Length;

        /// <summary>원본 Read(&amp;struct, n): 구조체 앞에서부터 n바이트를 채운다 (구조체보다 길면 구조체 크기까지)</summary>
        public uint Read(NativeRecord record, long count)
        {
            if (_f == null) return 0;
            var b = new byte[Math.Min(count, record.Size)];
            int n = _f.Read(b, 0, b.Length);
            byte[] cur = record.ToBytes();
            Buffer.BlockCopy(b, 0, cur, 0, n);
            record.CopyFrom(cur);
            return (uint)n;
        }

        public void Write(NativeRecord record, long count)
        {
            if (_f == null) return;
            byte[] b = record.ToBytes();
            _f.Write(b, 0, (int)Math.Min(count, b.Length));
        }
    }
}
