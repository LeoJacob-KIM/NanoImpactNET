using System;
using System.Text;

namespace NanoImpact.Core.Native
{
    /// <summary>
    /// 원본 C 코드의 배열 범위 밖 접근 기록. 원본은 구조체 안에서 배열 끝을 넘어 다음 필드를 읽고 쓰는 곳이 있다
    /// (예: AfxIntegratedSetWDOLSrotZoomingLinearize의 nWDOLCoarse[nWD + 1]). C#도 같은 바이트를 읽고 쓰되 여기에 알린다.
    /// </summary>
    public static class NativeDiagnostics
    {
        public static event Action<string> OutOfRange;

        internal static void Report(int offset, int index, int length, string kind)
        {
            OutOfRange?.Invoke(kind + " 오프셋 " + offset + " 배열[" + length + "]의 " + index + "번 접근");
        }
    }

    /// <summary>
    /// 구조체 안의 1차원 배열 (예: int nSyncVal[MAX_SCAN_MODE]). 원본 바이트를 직접 읽고 쓴다.
    /// C처럼 배열 범위를 넘어도 구조체 바이트 안이면 그대로 접근하고(원본 동작 재현) NativeDiagnostics에 알린다.
    /// </summary>
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

        private unsafe int ByteOffset(int index, string kind)
        {
            if ((uint)index >= (uint)Length) NativeDiagnostics.Report(_offset, index, Length, kind);
            long b = _offset + (long)index * sizeof(T);
            if (b < 0 || b + sizeof(T) > _raw.Length) throw new IndexOutOfRangeException("구조체 밖 접근: 오프셋 " + _offset + " 배열[" + Length + "]의 " + index + "번");
            return (int)b;
        }

        public unsafe T this[int index]
        {
            get
            {
                fixed (byte* p = &_raw[ByteOffset(index, "읽기")]) return *(T*)p;
            }
            set
            {
                fixed (byte* p = &_raw[ByteOffset(index, "쓰기")]) *(T*)p = value;
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
                fixed (byte* p = &_raw[ByteOffset(row, col, "읽기")]) return *(T*)p;
            }
            set
            {
                fixed (byte* p = &_raw[ByteOffset(row, col, "쓰기")]) *(T*)p = value;
            }
        }

        /// <summary>C처럼 행·열이 범위를 넘어도 구조체 바이트 안이면 접근한다 (NativeArray와 같은 규칙)</summary>
        private unsafe int ByteOffset(int row, int col, string kind)
        {
            if ((uint)row >= (uint)_rows || (uint)col >= (uint)_cols) NativeDiagnostics.Report(_offset, row * _cols + col, _rows * _cols, kind);
            long b = _offset + ((long)row * _cols + col) * sizeof(T);
            if (b < 0 || b + sizeof(T) > _raw.Length) throw new IndexOutOfRangeException("구조체 밖 접근: 오프셋 " + _offset + " [" + row + "," + col + "]");
            return (int)b;
        }

        /// <summary>한 행을 1차원 배열로 (원본 arr[row])</summary>
        public unsafe NativeArray<T> Row(int row)
        {
            if ((uint)row >= (uint)_rows) throw new IndexOutOfRangeException();
            return new NativeArray<T>(_raw, _offset + row * _cols * sizeof(T), _cols);
        }

    }

    /// <summary>
    /// 구조체 안의 char 배열 (예: char szServerIPAddress[16]). Value는 NUL까지를 CP949로 읽는다.
    /// 쓸 때는 strcpy처럼 NUL 하나만 붙이고 그 뒤 바이트는 건드리지 않는다(원본 파일 왕복 보존).
    /// </summary>
    public sealed partial class NativeString
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
    public sealed partial class NativeStringArray
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
