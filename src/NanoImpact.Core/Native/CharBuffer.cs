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
    }

    partial class NativeString : ICharArray
    {
    }
}
