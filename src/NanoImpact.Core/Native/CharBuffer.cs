using System;

namespace NanoImpact.Core.Native
{
    /// <summary>C char 배열 (구조체 필드 NativeString과 지역 배열 CharBuffer 공통). CRuntime의 str* 함수가 받는다.</summary>
    public interface ICharArray
    {
        int Capacity { get; }
        byte this[int index] { get; set; }
    }

    /// <summary>
    /// 원본의 지역·전역 char 배열 (char szX[N]). C처럼 NUL로 끝나는 바이트 배열로 다루고, ToString은 NUL까지를 CP949로 읽는다.
    /// </summary>
    public sealed class CharBuffer : ICharArray
    {
        private readonly byte[] _b;

        public CharBuffer(int capacity)
        {
            _b = new byte[capacity];
        }

        public CharBuffer(int capacity, string initial) : this(capacity)
        {
            CRuntime.strcpy(this, initial);
        }

        public int Capacity => _b.Length;

        /// <summary>원본처럼 버퍼 메모리를 직접 넘길 때 (FT_Read 등)</summary>
        public byte[] Bytes => _b;

        public byte this[int index]
        {
            get => _b[index];
            set => _b[index] = value;
        }

        public override string ToString() => CRuntime.ToStr(this);

        /// <summary>원본 포인터 산술 szX + n (memcpy(szARG1, pszCommand + k, 8), strncpy(szCommand + 3, ...))</summary>
        public static CharPtr operator +(CharBuffer b, int offset) => new CharPtr(b, offset);
    }

    /// <summary>char 배열 안의 한 위치를 가리키는 원본 char* (배열 + 오프셋). 읽고 쓰는 것은 바탕 배열에 그대로 간다.</summary>
    public sealed class CharPtr : ICharArray
    {
        private readonly ICharArray _base;
        private readonly int _offset;

        public CharPtr(ICharArray b, int offset)
        {
            _base = b;
            _offset = offset;
        }

        public int Capacity => _base.Capacity - _offset;

        public byte this[int index]
        {
            get => _base[_offset + index];
            set => _base[_offset + index] = value;
        }

        public static CharPtr operator +(CharPtr p, int offset) => new CharPtr(p._base, p._offset + offset);

        public override string ToString() => CRuntime.ToStr(this);
    }

    partial class NativeString : ICharArray
    {
    }
}
