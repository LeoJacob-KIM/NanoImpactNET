using System;
using System.IO;

namespace NanoImpact.Core.Native
{
    /// <summary>필드 하나의 위치 (원본 MSVC x86 배치). 검증 도구가 cl.exe 결과와 대조한다.</summary>
    public struct NativeField
    {
        public NativeField(string name, int offset, int size)
        {
            Name = name;
            Offset = offset;
            Size = size;
        }

        public string Name { get; }
        public int Offset { get; }
        public int Size { get; }
    }

    /// <summary>
    /// 원본 C 구조체를 바이트 배열 그대로 들고 필드를 오프셋으로 읽고 쓴다.
    /// 원본 설정 파일(*.INI)은 구조체를 memcpy로 덤프한 것이라, 패딩·미초기화 바이트까지 보존해야 왕복해도 같은 파일이 된다.
    /// </summary>
    public abstract class NativeRecord
    {
        protected NativeRecord(int size)
        {
            Raw = new byte[size];
        }

        /// <summary>구조체 바이트 (크기 = sizeof, x86)</summary>
        protected byte[] Raw { get; }

        public int Size => Raw.Length;

        /// <summary>
        /// 원본 CFile::Read(p, fileSize)와 같다: 앞에서부터 덮어쓰고, 파일이 짧으면 뒤쪽은 지금 값을 유지한다.
        /// 원본은 파일이 구조체보다 길면 메모리를 넘쳐 썼다 — 여기서는 구조체 크기까지만 쓴다. 복사한 바이트 수를 돌려준다.
        /// </summary>
        public int CopyFrom(byte[] source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            int n = Math.Min(source.Length, Raw.Length);
            Buffer.BlockCopy(source, 0, Raw, 0, n);
            return n;
        }

        public void CopyFrom(NativeRecord other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            if (other.Raw.Length != Raw.Length) throw new ArgumentException("크기가 다른 구조체입니다.");
            Buffer.BlockCopy(other.Raw, 0, Raw, 0, Raw.Length);
        }

        /// <summary>memset(p, 0, sizeof)</summary>
        public void Clear() => Array.Clear(Raw, 0, Raw.Length);

        public byte[] ToBytes() => (byte[])Raw.Clone();

        public int LoadFile(string path) => CopyFrom(File.ReadAllBytes(path));

        public void SaveFile(string path) => File.WriteAllBytes(path, Raw);

        protected unsafe T Read<T>(int offset) where T : unmanaged
        {
            fixed (byte* p = &Raw[offset]) return *(T*)p;
        }

        protected unsafe void Write<T>(int offset, T value) where T : unmanaged
        {
            fixed (byte* p = &Raw[offset]) *(T*)p = value;
        }
    }
}
