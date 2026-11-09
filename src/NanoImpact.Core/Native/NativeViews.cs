using System;
using System.Text;

namespace NanoImpact.Core.Native
{
    /// <summary>구조체 안의 1차원 배열 (예: int nSyncVal[MAX_SCAN_MODE]). 원본 바이트를 직접 읽고 쓴다.</summary>
    public sealed class NativeArray<T> where T : unmanaged
    {
        private readonly byte[] _raw;
        private readonly int _offset;

        public NativeArray(byte[] raw, int offset, int length)
        {
            _raw = raw;
            _offset = offset;
            Length = length;
        }

        public int Length { get; }

        public unsafe T this[int index]
        {
            get
            {
                if ((uint)index >= (uint)Length) throw new IndexOutOfRangeException();
                fixed (byte* p = &_raw[_offset]) return ((T*)p)[index];
            }
            set
            {
                if ((uint)index >= (uint)Length) throw new IndexOutOfRangeException();
                fixed (byte* p = &_raw[_offset]) ((T*)p)[index] = value;
            }
        }

        public T[] ToArray()
        {
            var a = new T[Length];
            for (int i = 0; i < Length; i++) a[i] = this[i];
            return a;
        }
    }

    /// <summary>구조체 안의 2차원 배열 (예: int nSRotRelayZoomXInput[20][360]). C와 같은 행 우선 배치.</summary>
    public sealed class NativeArray2<T> where T : unmanaged
    {
        private readonly byte[] _raw;
        private readonly int _offset;
        private readonly int _rows;
        private readonly int _cols;

        public NativeArray2(byte[] raw, int offset, int rows, int cols)
        {
            _raw = raw;
            _offset = offset;
            _rows = rows;
            _cols = cols;
        }

        public int GetLength(int dimension) => dimension == 0 ? _rows : dimension == 1 ? _cols : throw new ArgumentOutOfRangeException(nameof(dimension));

        public unsafe T this[int row, int col]
        {
            get
            {
                Check(row, col);
                fixed (byte* p = &_raw[_offset]) return ((T*)p)[row * _cols + col];
            }
            set
            {
                Check(row, col);
                fixed (byte* p = &_raw[_offset]) ((T*)p)[row * _cols + col] = value;
            }
        }

        /// <summary>한 행을 1차원 배열로 (원본 arr[row])</summary>
        public unsafe NativeArray<T> Row(int row)
        {
            if ((uint)row >= (uint)_rows) throw new IndexOutOfRangeException();
            return new NativeArray<T>(_raw, _offset + row * _cols * sizeof(T), _cols);
        }

        private void Check(int row, int col)
        {
            if ((uint)row >= (uint)_rows || (uint)col >= (uint)_cols) throw new IndexOutOfRangeException();
        }
    }

    /// <summary>
    /// 구조체 안의 char 배열 (예: char szServerIPAddress[16]). Value는 NUL까지를 CP949로 읽는다.
    /// 쓸 때는 strcpy처럼 NUL 하나만 붙이고 그 뒤 바이트는 건드리지 않는다(원본 파일 왕복 보존).
    /// </summary>
    public sealed class NativeString
    {
        /// <summary>원본 문자열 코드 페이지 (한국어 Windows ANSI)</summary>
        public static readonly Encoding Ansi = Encoding.GetEncoding(949);

        private readonly byte[] _raw;
        private readonly int _offset;

        public NativeString(byte[] raw, int offset, int capacity)
        {
            _raw = raw;
            _offset = offset;
            Capacity = capacity;
        }

        /// <summary>배열 크기 (NUL 포함)</summary>
        public int Capacity { get; }

        public byte this[int index]
        {
            get
            {
                if ((uint)index >= (uint)Capacity) throw new IndexOutOfRangeException();
                return _raw[_offset + index];
            }
            set
            {
                if ((uint)index >= (uint)Capacity) throw new IndexOutOfRangeException();
                _raw[_offset + index] = value;
            }
        }

        public string Value
        {
            get
            {
                int n = 0;
                while (n < Capacity && _raw[_offset + n] != 0) n++;
                return Ansi.GetString(_raw, _offset, n);
            }
            set
            {
                byte[] b = Ansi.GetBytes(value ?? "");
                if (b.Length >= Capacity) throw new ArgumentException("문자열이 배열보다 깁니다 (" + b.Length + " >= " + Capacity + ")");
                Buffer.BlockCopy(b, 0, _raw, _offset, b.Length);
                _raw[_offset + b.Length] = 0;
            }
        }

        /// <summary>memset(sz, 0, sizeof(sz))</summary>
        public void Clear() => Array.Clear(_raw, _offset, Capacity);

        public override string ToString() => Value;
    }

    /// <summary>구조체 안의 2차원 char 배열 (char sz[n][len]) — 문자열 n개</summary>
    public sealed class NativeStringArray
    {
        private readonly byte[] _raw;
        private readonly int _offset;
        private readonly int _capacity;

        public NativeStringArray(byte[] raw, int offset, int count, int capacity)
        {
            _raw = raw;
            _offset = offset;
            Length = count;
            _capacity = capacity;
        }

        public int Length { get; }

        public NativeString this[int index]
        {
            get
            {
                if ((uint)index >= (uint)Length) throw new IndexOutOfRangeException();
                return new NativeString(_raw, _offset + index * _capacity, _capacity);
            }
        }
    }
}
