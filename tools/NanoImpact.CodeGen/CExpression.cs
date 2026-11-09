using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace NanoImpact.CodeGen
{
    internal enum CKind { Int, UInt, Long, Double, Str }

    /// <summary>#define 값 하나 (C 전처리 후 상수식의 결과)</summary>
    internal struct CVal
    {
        public CKind Kind;
        public long I;
        public double D;
        public string S;

        public static CVal Int(long v) => new CVal { Kind = CKind.Int, I = (int)v };
        public static CVal UInt(long v) => new CVal { Kind = CKind.UInt, I = (uint)v };
        public static CVal Long(long v) => new CVal { Kind = CKind.Long, I = v };
        public static CVal Dbl(double v) => new CVal { Kind = CKind.Double, D = v };
        public static CVal Str(string v) => new CVal { Kind = CKind.Str, S = v };

        public bool IsInteger => Kind == CKind.Int || Kind == CKind.UInt || Kind == CKind.Long;
        public double AsDouble => Kind == CKind.Double ? D : I;

        public bool SameAs(CVal o) => Kind == o.Kind && I == o.I && D.Equals(o.D) && S == o.S;

        public override string ToString()
        {
            switch (Kind)
            {
                case CKind.Double: return D.ToString("R", CultureInfo.InvariantCulture);
                case CKind.Str: return "\"" + S + "\"";
                default: return I.ToString(CultureInfo.InvariantCulture);
            }
        }
    }

    internal sealed class CEvalException : Exception
    {
        public CEvalException(string message) : base(message) { }
    }

    /// <summary>
    /// #define 값을 C 규칙으로 계산: 정수 리터럴 형(int/unsigned/long long), 산술 변환, 정수 나눗셈, 시프트,
    /// 인접 문자열 연결, _T(), RGB(), 형 변환 (WORD) 등.
    /// </summary>
    internal sealed class CExpression
    {
        private static readonly Dictionary<string, CVal> Builtins = new Dictionary<string, CVal>
        {
            ["WM_USER"] = CVal.Int(0x0400),
            ["TRUE"] = CVal.Int(1),
            ["FALSE"] = CVal.Int(0),
            ["NULL"] = CVal.Int(0),
            ["MAX_PATH"] = CVal.Int(260),
        };

        private static readonly Dictionary<string, Func<CVal, CVal>> Casts = new Dictionary<string, Func<CVal, CVal>>
        {
            ["int"] = v => CVal.Int(ToLong(v)),
            ["long"] = v => CVal.Int(ToLong(v)),
            ["LONG"] = v => CVal.Int(ToLong(v)),
            ["BOOL"] = v => CVal.Int(ToLong(v)),
            ["short"] = v => CVal.Int((short)ToLong(v)),
            ["char"] = v => CVal.Int((sbyte)ToLong(v)),
            ["BYTE"] = v => CVal.Int((byte)ToLong(v)),
            ["UCHAR"] = v => CVal.Int((byte)ToLong(v)),
            ["WORD"] = v => CVal.Int((ushort)ToLong(v)),
            ["USHORT"] = v => CVal.Int((ushort)ToLong(v)),
            ["DWORD"] = v => CVal.UInt(ToLong(v)),
            ["UINT"] = v => CVal.UInt(ToLong(v)),
            ["ULONG"] = v => CVal.UInt(ToLong(v)),
            ["COLORREF"] = v => CVal.UInt(ToLong(v)),
            ["double"] = v => CVal.Dbl(v.AsDouble),
            ["float"] = v => CVal.Dbl((float)v.AsDouble),
        };

        private readonly IReadOnlyDictionary<string, CVal> _defines;
        private List<string> _tok;
        private int _p;

        public CExpression(IReadOnlyDictionary<string, CVal> defines)
        {
            _defines = defines;
        }

        public CVal Evaluate(string text)
        {
            _tok = Tokenize(text);
            _p = 0;
            if (_tok.Count == 0) throw new CEvalException("빈 값");
            CVal v = Or();
            if (_p != _tok.Count) throw new CEvalException("해석 못 한 토큰: " + _tok[_p]);
            return v;
        }

        private static long ToLong(CVal v)
        {
            if (v.Kind == CKind.Str) throw new CEvalException("문자열을 숫자로 쓸 수 없음");
            return v.Kind == CKind.Double ? (long)v.D : v.I;
        }

        // ---- 토큰 ----
        private static List<string> Tokenize(string s)
        {
            var t = new List<string>();
            int i = 0;
            while (i < s.Length)
            {
                char c = s[i];
                if (char.IsWhiteSpace(c)) { i++; continue; }
                if (c == '"' || c == '\'')
                {
                    int j = i + 1;
                    while (j < s.Length && s[j] != c) j += s[j] == '\\' ? 2 : 1;
                    if (j >= s.Length) throw new CEvalException("닫히지 않은 따옴표");
                    t.Add(s.Substring(i, j - i + 1));
                    i = j + 1;
                    continue;
                }
                if (char.IsDigit(c) || (c == '.' && i + 1 < s.Length && char.IsDigit(s[i + 1])))
                {
                    int j = i;
                    if (c == '0' && j + 1 < s.Length && (s[j + 1] == 'x' || s[j + 1] == 'X'))
                    {
                        j += 2;
                        while (j < s.Length && Uri.IsHexDigit(s[j])) j++;
                    }
                    else
                    {
                        while (j < s.Length && (char.IsDigit(s[j]) || s[j] == '.')) j++;
                        if (j < s.Length && (s[j] == 'e' || s[j] == 'E'))
                        {
                            j++;
                            if (j < s.Length && (s[j] == '+' || s[j] == '-')) j++;
                            while (j < s.Length && char.IsDigit(s[j])) j++;
                        }
                    }
                    while (j < s.Length && "uUlLfF".IndexOf(s[j]) >= 0) j++;
                    t.Add(s.Substring(i, j - i));
                    i = j;
                    continue;
                }
                if (char.IsLetter(c) || c == '_')
                {
                    int j = i;
                    while (j < s.Length && (char.IsLetterOrDigit(s[j]) || s[j] == '_')) j++;
                    t.Add(s.Substring(i, j - i));
                    i = j;
                    continue;
                }
                string two = i + 1 < s.Length ? s.Substring(i, 2) : "";
                if (two == "<<" || two == ">>" || two == "==" || two == "!=" || two == "<=" || two == ">=" || two == "&&" || two == "||")
                {
                    t.Add(two);
                    i += 2;
                    continue;
                }
                if ("+-*/%&|^~!()<>,".IndexOf(c) >= 0)
                {
                    t.Add(c.ToString());
                    i++;
                    continue;
                }
                throw new CEvalException("알 수 없는 글자: " + c);
            }
            return t;
        }

        private string Peek => _p < _tok.Count ? _tok[_p] : null;

        private void Expect(string s)
        {
            if (Peek != s) throw new CEvalException("'" + s + "'가 필요함");
            _p++;
        }

        // ---- 우선순위 ----
        private CVal Or() { CVal a = Xor(); while (Peek == "|") { _p++; a = Bin("|", a, Xor()); } return a; }
        private CVal Xor() { CVal a = And(); while (Peek == "^") { _p++; a = Bin("^", a, And()); } return a; }
        private CVal And() { CVal a = Eq(); while (Peek == "&") { _p++; a = Bin("&", a, Eq()); } return a; }
        private CVal Eq() { CVal a = Rel(); while (Peek == "==" || Peek == "!=") { string o = _tok[_p++]; a = Bin(o, a, Rel()); } return a; }
        private CVal Rel() { CVal a = Shift(); while (Peek == "<" || Peek == ">" || Peek == "<=" || Peek == ">=") { string o = _tok[_p++]; a = Bin(o, a, Shift()); } return a; }
        private CVal Shift() { CVal a = Add(); while (Peek == "<<" || Peek == ">>") { string o = _tok[_p++]; a = Bin(o, a, Add()); } return a; }
        private CVal Add() { CVal a = Mul(); while (Peek == "+" || Peek == "-") { string o = _tok[_p++]; a = Bin(o, a, Mul()); } return a; }
        private CVal Mul() { CVal a = Unary(); while (Peek == "*" || Peek == "/" || Peek == "%") { string o = _tok[_p++]; a = Bin(o, a, Unary()); } return a; }

        private CVal Unary()
        {
            string t = Peek;
            if (t == "-") { _p++; CVal v = Unary(); return v.Kind == CKind.Double ? CVal.Dbl(-v.D) : Wrap(v.Kind, -ToLong(v)); }
            if (t == "+") { _p++; return Unary(); }
            if (t == "~") { _p++; CVal v = Unary(); return Wrap(v.Kind == CKind.Double ? throw new CEvalException("~ double") : v.Kind, ~ToLong(v)); }
            if (t == "!") { _p++; CVal v = Unary(); return CVal.Int(v.AsDouble == 0 ? 1 : 0); }
            if (t == "(" && _p + 2 < _tok.Count && Casts.ContainsKey(_tok[_p + 1]) && _tok[_p + 2] == ")")
            {
                Func<CVal, CVal> cast = Casts[_tok[_p + 1]];
                _p += 3;
                return cast(Unary());
            }
            return Primary();
        }

        private CVal Primary()
        {
            string t = Peek ?? throw new CEvalException("식이 끝남");
            _p++;
            if (t == "(") { CVal v = Or(); Expect(")"); return v; }
            if (t[0] == '"')
            {
                var sb = new StringBuilder(Unescape(t.Substring(1, t.Length - 2)));
                while (Peek != null && Peek[0] == '"') sb.Append(Unescape(_tok[_p].Substring(1, _tok[_p++].Length - 2)));
                return CVal.Str(sb.ToString());
            }
            if (t[0] == '\'')
            {
                string u = Unescape(t.Substring(1, t.Length - 2));
                if (u.Length != 1) throw new CEvalException("문자 상수: " + t);
                return CVal.Int((sbyte)u[0]);
            }
            if (char.IsDigit(t[0]) || t[0] == '.') return Number(t);

            if (Peek == "(")
            {
                _p++;
                var args = new List<CVal>();
                if (Peek != ")")
                {
                    args.Add(Or());
                    while (Peek == ",") { _p++; args.Add(Or()); }
                }
                Expect(")");
                if (t == "_T" || t == "TEXT" || t == "_TEXT")
                {
                    if (args.Count != 1) throw new CEvalException("_T 인자");
                    return args[0];
                }
                if (t == "RGB")
                {
                    if (args.Count != 3) throw new CEvalException("RGB 인자");
                    long r = (byte)ToLong(args[0]), g = (byte)ToLong(args[1]), b = (byte)ToLong(args[2]);
                    return CVal.UInt(r | (g << 8) | (b << 16));
                }
                throw new CEvalException("함수 매크로 모름: " + t);
            }

            if (_defines.TryGetValue(t, out CVal d)) return d;
            if (Builtins.TryGetValue(t, out CVal b2)) return b2;
            throw new CEvalException("모르는 이름: " + t);
        }

        private static CVal Number(string t)
        {
            string body = t.TrimEnd('u', 'U', 'l', 'L', 'f', 'F');
            string suffix = t.Substring(body.Length).ToUpperInvariant();
            bool hex = body.StartsWith("0x", StringComparison.OrdinalIgnoreCase);
            if (!hex && (body.Contains(".") || body.IndexOfAny(new[] { 'e', 'E' }) >= 0 || suffix.Contains("F")))
            {
                double d = double.Parse(body, NumberStyles.Float, CultureInfo.InvariantCulture);
                return CVal.Dbl(suffix.Contains("F") ? (float)d : d);
            }

            ulong v;
            if (hex) v = ulong.Parse(body.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            else if (body.Length > 1 && body[0] == '0') v = Convert.ToUInt64(body, 8);
            else v = ulong.Parse(body, CultureInfo.InvariantCulture);

            bool unsignedSuffix = suffix.Contains("U");
            bool longLong = suffix.Contains("LL");
            // MSVC: long은 32비트. 10진수는 int → (long long), 16진/8진은 int → unsigned int → long long
            if (!longLong)
            {
                if (!unsignedSuffix && v <= int.MaxValue) return CVal.Int((long)v);
                if ((unsignedSuffix || hex || body[0] == '0') && v <= uint.MaxValue) return CVal.UInt((long)v);
                if (!hex && v <= uint.MaxValue && !unsignedSuffix) return CVal.Long((long)v);
            }
            return CVal.Long((long)v);
        }

        private static CVal Wrap(CKind kind, long v)
        {
            switch (kind)
            {
                case CKind.UInt: return CVal.UInt(v);
                case CKind.Long: return CVal.Long(v);
                default: return CVal.Int(v);
            }
        }

        private static CVal Bin(string op, CVal a, CVal b)
        {
            if (a.Kind == CKind.Str || b.Kind == CKind.Str) throw new CEvalException("문자열 연산");

            bool cmp = op == "==" || op == "!=" || op == "<" || op == ">" || op == "<=" || op == ">=";
            if (a.Kind == CKind.Double || b.Kind == CKind.Double)
            {
                double x = a.AsDouble, y = b.AsDouble;
                switch (op)
                {
                    case "+": return CVal.Dbl(x + y);
                    case "-": return CVal.Dbl(x - y);
                    case "*": return CVal.Dbl(x * y);
                    case "/": return CVal.Dbl(x / y);
                    case "==": return CVal.Int(x == y ? 1 : 0);
                    case "!=": return CVal.Int(x != y ? 1 : 0);
                    case "<": return CVal.Int(x < y ? 1 : 0);
                    case ">": return CVal.Int(x > y ? 1 : 0);
                    case "<=": return CVal.Int(x <= y ? 1 : 0);
                    case ">=": return CVal.Int(x >= y ? 1 : 0);
                    default: throw new CEvalException("double에 " + op);
                }
            }

            if (op == "<<" || op == ">>")
            {
                int n = (int)b.I;
                CKind k = a.Kind;
                if (k == CKind.UInt) return CVal.UInt(op == "<<" ? (long)((uint)a.I << n) : (long)((uint)a.I >> n));
                if (k == CKind.Long) return CVal.Long(op == "<<" ? a.I << n : a.I >> n);
                return CVal.Int(op == "<<" ? (int)a.I << n : (int)a.I >> n);
            }

            // 보통 산술 변환: long long > unsigned int > int
            CKind kind = a.Kind == CKind.Long || b.Kind == CKind.Long ? CKind.Long
                       : a.Kind == CKind.UInt || b.Kind == CKind.UInt ? CKind.UInt : CKind.Int;
            long l = a.I, r = b.I;
            if (kind == CKind.UInt) { l = (uint)l; r = (uint)r; }
            if (cmp)
            {
                bool res;
                switch (op)
                {
                    case "==": res = l == r; break;
                    case "!=": res = l != r; break;
                    case "<": res = l < r; break;
                    case ">": res = l > r; break;
                    case "<=": res = l <= r; break;
                    default: res = l >= r; break;
                }
                return CVal.Int(res ? 1 : 0);
            }
            switch (op)
            {
                case "+": return Wrap(kind, l + r);
                case "-": return Wrap(kind, l - r);
                case "*": return Wrap(kind, l * r);
                case "/": if (r == 0) throw new CEvalException("0으로 나눔"); return Wrap(kind, l / r);
                case "%": if (r == 0) throw new CEvalException("0으로 나눔"); return Wrap(kind, l % r);
                case "&": return Wrap(kind, l & r);
                case "|": return Wrap(kind, l | r);
                case "^": return Wrap(kind, l ^ r);
                default: throw new CEvalException("연산자 " + op);
            }
        }

        private static string Unescape(string s)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (c != '\\' || i + 1 >= s.Length) { sb.Append(c); continue; }
                char e = s[++i];
                switch (e)
                {
                    case 'n': sb.Append('\n'); break;
                    case 'r': sb.Append('\r'); break;
                    case 't': sb.Append('\t'); break;
                    case 'a': sb.Append('\a'); break;
                    case '0': sb.Append('\0'); break;
                    case 'x':
                        int j = i + 1;
                        while (j < s.Length && Uri.IsHexDigit(s[j])) j++;
                        sb.Append((char)Convert.ToInt32(s.Substring(i + 1, j - i - 1), 16));
                        i = j - 1;
                        break;
                    default: sb.Append(e); break; // \\ \" \' \?
                }
            }
            return sb.ToString();
        }
    }
}
