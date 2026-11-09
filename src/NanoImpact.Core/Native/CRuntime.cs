using System;
using System.Globalization;
using System.Numerics;
using System.Text;

namespace NanoImpact.Core.Native
{
    /// <summary>
    /// 원본 C/C++ 코드를 거의 그대로 옮기기 위한 C 런타임 함수들 (MSVC UCRT와 같은 결과).
    /// 포팅한 파일에서 <c>using static NanoImpact.Core.Native.CRuntime;</c>로 쓴다.
    /// </summary>
    public static class CRuntime
    {
        private static readonly Encoding Ansi = NativeString.Ansi;

        // ---- 문자열 (char 배열) ----

        public static int strlen(ICharArray s)
        {
            int n = 0;
            while (n < s.Capacity && s[n] != 0) n++;
            return n;
        }

        public static string ToStr(ICharArray s) => Ansi.GetString(Bytes(s, strlen(s)));

        public static ICharArray strcpy(ICharArray dst, string src)
        {
            byte[] b = Ansi.GetBytes(src ?? "");
            if (b.Length >= dst.Capacity) throw new IndexOutOfRangeException("strcpy: 버퍼 넘침 (" + b.Length + " >= " + dst.Capacity + ")");
            for (int i = 0; i < b.Length; i++) dst[i] = b[i];
            dst[b.Length] = 0;
            return dst;
        }

        public static ICharArray strcpy(ICharArray dst, ICharArray src) => strcpy(dst, ToStr(src));
        public static int strcpy_s(ICharArray dst, string src) { strcpy(dst, src); return 0; }
        public static int strcpy_s(ICharArray dst, ICharArray src) { strcpy(dst, src); return 0; }

        public static ICharArray strncpy(ICharArray dst, string src, int count)
        {
            byte[] b = Ansi.GetBytes(src ?? "");
            for (int i = 0; i < count; i++) dst[i] = i < b.Length ? b[i] : (byte)0;
            return dst;
        }

        /// <summary>C strncpy: src의 NUL 전까지 최대 count 바이트를 복사하고 남은 칸은 0으로 채운다 (count 안에 NUL이 없으면 NUL을 붙이지 않는다)</summary>
        public static ICharArray strncpy(ICharArray dst, ICharArray src, int count)
        {
            int i = 0;
            for (; i < count && src[i] != 0; i++) dst[i] = src[i];
            for (; i < count; i++) dst[i] = 0;
            return dst;
        }

        /// <summary>C memcmp (바이트를 부호 없이 비교, 부호만 의미 있음)</summary>
        public static int memcmp(ICharArray a, ICharArray b, int count)
        {
            for (int i = 0; i < count; i++)
                if (a[i] != b[i]) return a[i] < b[i] ? -1 : 1;
            return 0;
        }

        /// <summary>memcmp(buf, "00", 2) — 문자열 상수는 ANSI 바이트 뒤에 NUL이 있는 배열</summary>
        public static int memcmp(ICharArray a, string b, int count)
        {
            byte[] bb = Ansi.GetBytes(b ?? "");
            for (int i = 0; i < count; i++)
            {
                byte y = i < bb.Length ? bb[i] : (byte)0;
                if (a[i] != y) return a[i] < y ? -1 : 1;
            }
            return 0;
        }

        public static ICharArray strcat(ICharArray dst, string src)
        {
            int n = strlen(dst);
            byte[] b = Ansi.GetBytes(src ?? "");
            if (n + b.Length >= dst.Capacity) throw new IndexOutOfRangeException("strcat: 버퍼 넘침");
            for (int i = 0; i < b.Length; i++) dst[n + i] = b[i];
            dst[n + b.Length] = 0;
            return dst;
        }

        public static ICharArray strcat(ICharArray dst, ICharArray src) => strcat(dst, ToStr(src));
        public static int strcat_s(ICharArray dst, string src) { strcat(dst, src); return 0; }

        public static int strcmp(string a, string b) => Math.Sign(string.CompareOrdinal(a ?? "", b ?? ""));
        public static int strcmp(ICharArray a, string b) => strcmp(ToStr(a), b);
        public static int strcmp(string a, ICharArray b) => strcmp(a, ToStr(b));
        public static int strcmp(ICharArray a, ICharArray b) => strcmp(ToStr(a), ToStr(b));

        public static int strncmp(string a, string b, int n)
        {
            a = a ?? "";
            b = b ?? "";
            return Math.Sign(string.CompareOrdinal(a.Length > n ? a.Substring(0, n) : a, b.Length > n ? b.Substring(0, n) : b));
        }

        public static int strncmp(ICharArray a, string b, int n) => strncmp(ToStr(a), b, n);
        public static int strncmp(string a, ICharArray b, int n) => strncmp(a, ToStr(b), n);

        public static void memset(ICharArray dst, int value, int count)
        {
            for (int i = 0; i < count; i++) dst[i] = (byte)value;
        }

        /// <summary>memset(x, 0, sizeof(x)) — 버퍼 전체를 0으로</summary>
        public static void ZeroMem(ICharArray dst) => memset(dst, 0, dst.Capacity);
        public static void ZeroMem<T>(T[] dst) => Array.Clear(dst, 0, dst.Length);
        public static void ZeroMem(NativeRecord dst) => dst.Clear();
        public static void ZeroMem<T>(NativeArray<T> dst) where T : unmanaged { for (int i = 0; i < dst.Length; i++) dst[i] = default(T); }

        /// <summary>memset(배열, 0, 바이트 수) — 숫자 배열은 0으로 지우는 경우만 원본에 있다</summary>
        public static unsafe void memset<T>(T[] dst, int value, int byteCount) where T : unmanaged
        {
            if (value != 0) throw new NotSupportedException("숫자 배열 memset은 0만 지원");
            Array.Clear(dst, 0, Math.Min(dst.Length, byteCount / sizeof(T)));
        }

        public static unsafe void memset<T>(NativeArray<T> dst, int value, int byteCount) where T : unmanaged
        {
            if (value != 0) throw new NotSupportedException("숫자 배열 memset은 0만 지원");
            int n = Math.Min(dst.Length, byteCount / sizeof(T));
            for (int i = 0; i < n; i++) dst[i] = default(T);
        }

        public static int strcat_s(ICharArray dst, ICharArray src) { strcat(dst, src); return 0; }

        public static void memcpy(ICharArray dst, ICharArray src, int count)
        {
            for (int i = 0; i < count; i++) dst[i] = src[i];
        }

        /// <summary>memcpy(구조체 배열 필드, 지역 배열, 바이트 수)</summary>
        public static unsafe void memcpy<T>(NativeArray<T> dst, T[] src, int byteCount) where T : unmanaged
        {
            int n = byteCount / sizeof(T);
            for (int i = 0; i < n; i++) dst[i] = src[i];
        }

        /// <summary>memcmp(구조체 배열 필드, 지역 배열, 바이트 수) — 원본은 "다른가"(!= 0)로만 쓴다</summary>
        public static unsafe int memcmp<T>(NativeArray<T> a, T[] b, int byteCount) where T : unmanaged, IComparable<T>
        {
            int n = byteCount / sizeof(T);
            for (int i = 0; i < n; i++)
            {
                int c = a[i].CompareTo(b[i]);
                if (c != 0) return c < 0 ? -1 : 1;
            }
            return 0;
        }

        /// <summary>memcpy(지역 배열, 구조체 배열 필드, 바이트 수)</summary>
        public static unsafe void memcpy<T>(T[] dst, NativeArray<T> src, int byteCount) where T : unmanaged
        {
            int n = byteCount / sizeof(T);
            for (int i = 0; i < n; i++) dst[i] = src[i];
        }

        private static byte[] Bytes(ICharArray s, int n)
        {
            var b = new byte[n];
            for (int i = 0; i < n; i++) b[i] = s[i];
            return b;
        }

        /// <summary>C 배열 T a[n] = { init... }; — 모자란 원소는 0 (C 규칙)</summary>
        public static T[] NewArray<T>(int n, params T[] init)
        {
            var a = new T[n];
            Array.Copy(init, a, Math.Min(n, init.Length));
            return a;
        }

        // ---- 숫자 변환 ----

        /// <summary>C atoi: 앞 공백, 부호 하나, 숫자. 그 밖의 글자에서 멈춤.</summary>
        public static int atoi(string s)
        {
            s = s ?? "";
            int i = 0;
            while (i < s.Length && IsCSpace(s[i])) i++;
            int sign = 1;
            if (i < s.Length && (s[i] == '+' || s[i] == '-')) { if (s[i] == '-') sign = -1; i++; }
            long v = 0;
            while (i < s.Length && s[i] >= '0' && s[i] <= '9')
            {
                v = v * 10 + (s[i] - '0');
                if (v > 0x80000000L) { v = 0x80000000L; break; }
                i++;
            }
            v *= sign;
            if (v > int.MaxValue) return int.MaxValue;
            return (int)v;
        }

        public static int atoi(ICharArray s) => atoi(ToStr(s));
        public static int _wtoi(string s) => atoi(s);

        /// <summary>
        /// C atof (MSVC UCRT strtod): 앞 공백 뒤 가장 긴 실수 접두어. 10진, 16진 실수("0x10" = 16, "0x1.8p3" = 12), inf/infinity/nan,
        /// 범위를 넘으면 ±무한대.
        /// </summary>
        public static double atof(string s)
        {
            s = s ?? "";
            int i = 0;
            while (i < s.Length && IsCSpace(s[i])) i++;
            int start = i;
            bool negative = false;
            if (i < s.Length && (s[i] == '+' || s[i] == '-')) { negative = s[i] == '-'; i++; }
            if (StartsWithNoCase(s, i, "inf")) return negative ? double.NegativeInfinity : double.PositiveInfinity;
            if (StartsWithNoCase(s, i, "nan")) return double.NaN;
            if (i + 1 < s.Length && s[i] == '0' && (s[i + 1] == 'x' || s[i + 1] == 'X'))
            {
                double h = HexFloat(s, i + 2, out bool any);
                if (any) return negative ? -h : h;
                return negative ? -0.0 : 0.0; // "0x"만 있으면 "0"까지 읽는다
            }
            int digits = 0;
            while (i < s.Length && IsDigit09(s[i])) { i++; digits++; }
            if (i < s.Length && s[i] == '.') { i++; while (i < s.Length && IsDigit09(s[i])) { i++; digits++; } }
            if (digits == 0) return 0.0;
            int end = i;
            if (i < s.Length && (s[i] == 'e' || s[i] == 'E'))
            {
                int j = i + 1;
                if (j < s.Length && (s[j] == '+' || s[j] == '-')) j++;
                if (j < s.Length && IsDigit09(s[j])) { while (j < s.Length && IsDigit09(s[j])) j++; end = j; }
            }
            try
            {
                return double.Parse(s.Substring(start, end - start), NumberStyles.Float, CultureInfo.InvariantCulture);
            }
            catch (OverflowException)
            {
                return negative ? double.NegativeInfinity : double.PositiveInfinity; // .NET Framework은 넘치면 예외, C는 HUGE_VAL
            }
        }

        private static bool IsDigit09(char c) => c >= '0' && c <= '9';

        private static bool StartsWithNoCase(string s, int i, string word) =>
            i + word.Length <= s.Length && string.Compare(s, i, word, 0, word.Length, StringComparison.OrdinalIgnoreCase) == 0;

        /// <summary>16진 실수 본문 (0x 뒤): 16진 숫자, '.', 16진 소수, p[부호]10진 지수</summary>
        private static double HexFloat(string s, int i, out bool any)
        {
            ulong mant = 0;
            int exp2 = 0;
            any = false;
            bool dot = false;
            for (; i < s.Length; i++)
            {
                int d = HexDigit(s[i]);
                if (d < 0)
                {
                    if (s[i] == '.' && !dot) { dot = true; continue; }
                    break;
                }
                any = true;
                if (mant < (1UL << 60)) { mant = mant * 16 + (ulong)d; if (dot) exp2 -= 4; }
                else if (!dot) exp2 += 4; // 유효 자릿수를 넘는 정수 자리
            }
            if (!any) return 0.0;
            if (i < s.Length && (s[i] == 'p' || s[i] == 'P'))
            {
                int j = i + 1, sign = 1, e = 0;
                if (j < s.Length && (s[j] == '+' || s[j] == '-')) { if (s[j] == '-') sign = -1; j++; }
                if (j < s.Length && IsDigit09(s[j]))
                {
                    while (j < s.Length && IsDigit09(s[j])) { if (e < 100000) e = e * 10 + (s[j] - '0'); j++; }
                    exp2 += sign * e;
                }
            }
            return mant * Math.Pow(2, exp2);
        }

        private static int HexDigit(char c) =>
            c >= '0' && c <= '9' ? c - '0' : c >= 'a' && c <= 'f' ? c - 'a' + 10 : c >= 'A' && c <= 'F' ? c - 'A' + 10 : -1;

        public static double atof(ICharArray s) => atof(ToStr(s));
        public static double _wtof(string s) => atof(s);

        private static bool IsCSpace(char c) => c == ' ' || (c >= '\t' && c <= '\r');

        /// <summary>_itoa: 10진수는 부호 포함, 그 밖의 진법은 unsigned 32비트로.</summary>
        public static ICharArray _itoa(int value, ICharArray buffer, int radix)
        {
            strcpy(buffer, Itoa(value, radix));
            return buffer;
        }

        public static string Itoa(int value, int radix)
        {
            if (radix == 10) return value.ToString(CultureInfo.InvariantCulture);
            uint u = unchecked((uint)value);
            if (u == 0) return "0";
            var sb = new StringBuilder();
            while (u > 0) { uint d = u % (uint)radix; sb.Insert(0, (char)(d < 10 ? '0' + d : 'a' + d - 10)); u /= (uint)radix; }
            return sb.ToString();
        }

        // ---- 수학 (원본 <math.h> 이름) ----

        public static double sqrt(double x) => Math.Sqrt(x);
        public static double fabs(double x) => Math.Abs(x);
        public static int abs(int x) => x == int.MinValue ? x : Math.Abs(x);
        public static double abs(double x) => Math.Abs(x);
        public static int labs(int x) => abs(x);
        public static double pow(double x, double y) => Math.Pow(x, y);
        public static double floor(double x) => Math.Floor(x);
        public static double ceil(double x) => Math.Ceiling(x);
        public static double fmod(double x, double y) => x % y; // C# double %는 C fmod와 같다 (몫을 0 쪽으로 자름, 부호는 x)
        public static double sin(double x) => Math.Sin(x);
        public static double cos(double x) => Math.Cos(x);
        public static double tan(double x) => Math.Tan(x);
        public static double asin(double x) => Math.Asin(x);
        public static double acos(double x) => Math.Acos(x);
        public static double atan(double x) => Math.Atan(x);
        public static double atan2(double y, double x) => Math.Atan2(y, x);
        public static double exp(double x) => Math.Exp(x);
        public static double log(double x) => Math.Log(x);
        public static double log10(double x) => Math.Log10(x);

        // ---- printf ----

        /// <summary>sprintf(buffer, format, ...) — 결과를 buffer에 쓰고 길이를 돌려준다.</summary>
        public static int sprintf(ICharArray buffer, string format, params object[] args)
        {
            string s = Format(format, args);
            strcpy(buffer, s);
            return Ansi.GetByteCount(s);
        }

        public static int sprintf_s(ICharArray buffer, string format, params object[] args) => sprintf(buffer, format, args);

        /// <summary>C printf 형식 (%[-+ 0#][폭][.정밀도][h|l|ll]d i u x X o c s f F e E g G %). 실수는 정확한 이진값 기준으로 반올림.</summary>
        public static string Format(string format, params object[] args)
        {
            var sb = new StringBuilder();
            int ai = 0;
            int i = 0;
            while (i < format.Length)
            {
                char c = format[i++];
                if (c != '%') { sb.Append(c); continue; }
                if (i < format.Length && format[i] == '%') { sb.Append('%'); i++; continue; }

                bool left = false, plus = false, space = false, zero = false, alt = false;
                for (; i < format.Length; i++)
                {
                    char f = format[i];
                    if (f == '-') left = true;
                    else if (f == '+') plus = true;
                    else if (f == ' ') space = true;
                    else if (f == '0') zero = true;
                    else if (f == '#') alt = true;
                    else break;
                }
                int width = 0;
                if (i < format.Length && format[i] == '*') { width = ToInt(args[ai++]); i++; if (width < 0) { left = true; width = -width; } }
                else while (i < format.Length && char.IsDigit(format[i])) width = width * 10 + (format[i++] - '0');
                int prec = -1;
                if (i < format.Length && format[i] == '.')
                {
                    i++;
                    prec = 0;
                    if (i < format.Length && format[i] == '*') { prec = ToInt(args[ai++]); i++; }
                    else while (i < format.Length && char.IsDigit(format[i])) prec = prec * 10 + (format[i++] - '0');
                }
                bool is64 = false;
                while (i < format.Length && "hlLqjzt".IndexOf(format[i]) >= 0)
                {
                    if (format[i] == 'l' && i + 1 < format.Length && format[i + 1] == 'l') { is64 = true; i++; }
                    i++;
                }
                if (i >= format.Length) break;
                char conv = format[i++];
                object arg = ai < args.Length ? args[ai] : null;
                if (conv != '%') ai++;

                string body;
                string sign = "";
                bool numeric = true;
                switch (conv)
                {
                    case 'd':
                    case 'i':
                    {
                        long v = is64 ? ToLong(arg) : ToInt(arg);
                        if (v < 0) { sign = "-"; body = Digits(BigInteger.Negate(v), 10, false); }
                        else { sign = plus ? "+" : space ? " " : ""; body = Digits(v, 10, false); }
                        body = IntPrecision(body, prec, v == 0);
                        break;
                    }
                    case 'u':
                    case 'x':
                    case 'X':
                    case 'o':
                    {
                        ulong v = is64 ? unchecked((ulong)ToLong(arg)) : unchecked((uint)ToInt(arg));
                        int radix = conv == 'u' ? 10 : conv == 'o' ? 8 : 16;
                        body = IntPrecision(Digits(v, radix, conv == 'X'), prec, v == 0);
                        if (alt && v != 0 && radix == 16) sign = conv == 'X' ? "0X" : "0x";
                        if (alt && radix == 8 && !body.StartsWith("0")) body = "0" + body;
                        break;
                    }
                    case 'c':
                        numeric = false;
                        body = ((char)(ToInt(arg) & 0xFF)).ToString();
                        break;
                    case 's':
                    {
                        numeric = false;
                        string s = arg is ICharArray ca ? ToStr(ca) : arg?.ToString() ?? "(null)";
                        body = prec >= 0 && prec < s.Length ? s.Substring(0, prec) : s;
                        break;
                    }
                    case 'f':
                    case 'F':
                    case 'e':
                    case 'E':
                    case 'g':
                    case 'G':
                    {
                        double v = ToDouble(arg);
                        bool neg = v < 0 || (v == 0 && 1 / v < 0);
                        sign = neg ? "-" : plus ? "+" : space ? " " : "";
                        body = FormatFloat(Math.Abs(v), conv, prec < 0 ? 6 : prec, alt);
                        if (double.IsNaN(v) || double.IsInfinity(v)) zero = false;
                        break;
                    }
                    default:
                        numeric = false;
                        body = "%" + conv;
                        break;
                }

                int len = sign.Length + body.Length;
                if (len >= width) sb.Append(sign).Append(body);
                else if (left) sb.Append(sign).Append(body).Append(' ', width - len);
                else if (zero && numeric && !(prec >= 0 && "diuxXo".IndexOf(conv) >= 0)) sb.Append(sign).Append('0', width - len).Append(body);
                else sb.Append(' ', width - len).Append(sign).Append(body);
            }
            return sb.ToString();
        }

        private static string IntPrecision(string digits, int prec, bool isZero)
        {
            if (prec == 0 && isZero) return "";
            if (prec > digits.Length) return new string('0', prec - digits.Length) + digits;
            return digits;
        }

        private static string Digits(BigInteger v, int radix, bool upper)
        {
            if (v.IsZero) return "0";
            var sb = new StringBuilder();
            while (!v.IsZero)
            {
                int d = (int)(v % radix);
                sb.Insert(0, (char)(d < 10 ? '0' + d : (upper ? 'A' : 'a') + d - 10));
                v /= radix;
            }
            return sb.ToString();
        }

        private static int ToInt(object o)
        {
            switch (o)
            {
                case null: return 0;
                case int x: return x;
                case uint x: return unchecked((int)x);
                case short x: return x;
                case ushort x: return x;
                case byte x: return x;
                case sbyte x: return x;
                case char x: return x;
                case long x: return unchecked((int)x);
                case ulong x: return unchecked((int)x);
                case double x: return unchecked((int)(long)x);
                case bool x: return x ? 1 : 0;
                default: return Convert.ToInt32(o, CultureInfo.InvariantCulture);
            }
        }

        private static long ToLong(object o)
        {
            switch (o)
            {
                case long x: return x;
                case ulong x: return unchecked((long)x);
                case uint x: return x;
                default: return ToInt(o);
            }
        }

        private static double ToDouble(object o)
        {
            switch (o)
            {
                case double x: return x;
                case float x: return x;
                case null: return 0.0;
                default: return ToLong(o);
            }
        }

        // ---- 정확한 실수 출력 ----

        /// <summary>|v| (부호 없음)를 %f/%e/%g로. 정확한 이진값을 round-half-even으로 반올림(UCRT와 같음).</summary>
        private static string FormatFloat(double v, char conv, int prec, bool alt)
        {
            bool upper = char.IsUpper(conv);
            if (double.IsNaN(v)) return upper ? "NAN" : "nan";
            if (double.IsInfinity(v)) return upper ? "INF" : "inf";
            switch (char.ToLowerInvariant(conv))
            {
                case 'f':
                    return Fixed(v, prec, alt);
                case 'e':
                    return Sci(v, prec, alt, upper);
                default:
                {
                    int p = prec == 0 ? 1 : prec;
                    int exp = v == 0 ? 0 : SciDigits(v, p - 1, out _);
                    string s = p > exp && exp >= -4 ? Fixed(v, p - 1 - exp, alt) : Sci(v, p - 1, alt, upper);
                    if (!alt) s = TrimZeros(s);
                    return s;
                }
            }
        }

        private static string TrimZeros(string s)
        {
            int e = s.IndexOfAny(new[] { 'e', 'E' });
            string mant = e >= 0 ? s.Substring(0, e) : s;
            string tail = e >= 0 ? s.Substring(e) : "";
            if (mant.Contains(".")) mant = mant.TrimEnd('0').TrimEnd('.');
            return mant + tail;
        }

        /// <summary>v = m × 2^e (m 정수)</summary>
        private static void Decompose(double v, out BigInteger m, out int e)
        {
            long bits = BitConverter.DoubleToInt64Bits(v);
            int exp = (int)((bits >> 52) & 0x7FF);
            long frac = bits & 0xFFFFFFFFFFFFFL;
            if (exp == 0) { m = frac; e = -1074; }
            else { m = frac | (1L << 52); e = exp - 1075; }
        }

        /// <summary>round(v × 10^scale) (scale은 음수 가능), round-half-even</summary>
        private static BigInteger ScaledRound(double v, int scale)
        {
            Decompose(v, out BigInteger m, out int e);
            BigInteger num = m, den = BigInteger.One;
            if (e >= 0) num <<= e; else den <<= -e;
            if (scale >= 0) num *= BigInteger.Pow(10, scale); else den *= BigInteger.Pow(10, -scale);
            BigInteger q = BigInteger.DivRem(num, den, out BigInteger r);
            int cmp = (r * 2).CompareTo(den);
            if (cmp > 0 || (cmp == 0 && !q.IsEven)) q += 1;
            return q;
        }

        private static string Fixed(double v, int prec, bool alt)
        {
            string d = ScaledRound(v, prec).ToString(CultureInfo.InvariantCulture);
            if (prec == 0) return alt ? d + "." : d;
            if (d.Length <= prec) d = new string('0', prec - d.Length + 1) + d;
            return d.Substring(0, d.Length - prec) + "." + d.Substring(d.Length - prec);
        }

        /// <summary>v를 유효숫자 prec+1자리로 반올림했을 때의 10진 지수와 숫자열</summary>
        private static int SciDigits(double v, int prec, out string digits)
        {
            if (v == 0) { digits = new string('0', prec + 1); return 0; }
            int exp = (int)Math.Floor(Math.Log10(v));
            for (int guard = 0; guard < 4; guard++)
            {
                BigInteger q = ScaledRound(v, prec - exp);
                string s = q.ToString(CultureInfo.InvariantCulture);
                if (s.Length > prec + 1) { exp++; continue; }
                if (s.Length < prec + 1) { exp--; continue; }
                digits = s;
                return exp;
            }
            digits = ScaledRound(v, prec - exp).ToString(CultureInfo.InvariantCulture);
            return exp;
        }

        private static string Sci(double v, int prec, bool alt, bool upper)
        {
            int exp = SciDigits(v, prec, out string d);
            string mant = d.Substring(0, 1) + (prec > 0 || alt ? "." : "") + d.Substring(1);
            string es = Math.Abs(exp).ToString("00", CultureInfo.InvariantCulture);
            return mant + (upper ? "E" : "e") + (exp < 0 ? "-" : "+") + es;
        }
    }
}
