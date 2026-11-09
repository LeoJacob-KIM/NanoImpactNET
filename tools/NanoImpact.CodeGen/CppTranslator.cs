using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NanoImpact.CodeGen
{
    /// <summary>
    /// 원본 C++ 함수 → C# 초안 (기계적 변환). 결과는 손으로 고쳐 쓰는 출발점이며 다시 생성하지 않는다.
    ///   NanoImpact.CodeGen translate &lt;원본.cpp&gt; &lt;원본.h&gt; &lt;출력 폴더&gt; &lt;클래스 이름&gt; [묶음 줄 수]
    /// 함수마다 원본 위치를 주석으로 남기고, 원본 줄 순서대로 여러 파일로 나눈다.
    /// </summary>
    internal static class CppTranslator
    {
        private static readonly Dictionary<string, string> TypeMap = new Dictionary<string, string>
        {
            ["BOOL"] = "int", ["WORD"] = "ushort", ["DWORD"] = "uint", ["BYTE"] = "byte", ["UINT"] = "uint", ["INT"] = "int",
            ["ULONG"] = "uint", ["UCHAR"] = "byte", ["USHORT"] = "ushort", ["LONG"] = "int", ["long"] = "int", ["HWND"] = "int",
            ["FT_STATUS"] = "uint", ["FT_HANDLE"] = "IntPtr", ["HANDLE"] = "object", ["LPVOID"] = "object", ["PVOID"] = "object",
            ["CString"] = "string", ["COLORREF"] = "uint", ["LRESULT"] = "int", ["WPARAM"] = "int", ["LPARAM"] = "object",
            ["INT_PTR"] = "int", ["LPDWORD"] = "ref uint", ["CWinThread"] = "object",
            ["INTEGRATED_COMMAND_STRUCT"] = "IntegratedCommandStruct", ["LPINTEGRATED_COMMAND_STRUCT"] = "IntegratedCommandStruct",
            ["SEM_SCAN_ROTATION_CALIBRATION_DATA"] = "ScanRotationCalibrationData", ["LPSEM_SCAN_ROTATION_CALIBRATION_DATA"] = "ScanRotationCalibrationData",
            ["CLIENT_PUBLIC_DATA"] = "ClientPublicData", ["LPCLIENT_PUBLIC_DATA"] = "ClientPublicData",
            ["LPSCANBRIDGETHREADPARAMS"] = "SCANBRIDGETHREADPARAMS", ["LPSEIMAGEDATATHREADPARAMS"] = "SEIMAGEDATATHREADPARAMS",
            ["LPFOCUSDEGAUSSINGTHREADPARAMS"] = "FOCUSDEGAUSSINGTHREADPARAMS", ["LPPUBLICTOOLSPARAMS"] = "PUBLICTOOLSPARAMS",
            ["LPUSB_DEVICE_DATA_INFO"] = "USB_DEVICE_DATA_INFO", ["LPREQUEST_USB_DEVICE_DATA"] = "REQUEST_USB_DEVICE_DATA",
        };

        /// <summary>바이트 배열로 덮는 저장 구조체 (memset/memcpy가 Clear/CopyFrom이 된다)</summary>
        private static readonly Dictionary<string, string> Records = new Dictionary<string, string>
        {
            ["INTEGRATED_COMMAND_STRUCT"] = "IntegratedCommandStruct",
            ["SEM_SCAN_ROTATION_CALIBRATION_DATA"] = "ScanRotationCalibrationData",
            ["CLIENT_PUBLIC_DATA"] = "ClientPublicData",
        };

        /// <summary>참조형 클래스로 옮긴 구조체 (memset → new)</summary>
        private static readonly HashSet<string> Classes = new HashSet<string>
        {
            "USB_DEVICE_DATA_INFO", "REQUEST_USB_DEVICE_DATA", "SCANBRIDGETHREADPARAMS", "SEIMAGEDATATHREADPARAMS",
            "FOCUSDEGAUSSINGTHREADPARAMS", "PUBLICTOOLSPARAMS",
        };

        /// <summary>손으로 옮기는 함수 (번역에서 뺀다). USB_FT232H_Device.cpp → SemDevice.Ftdi.cs, SemDevice.cs</summary>
        private static readonly HashSet<string> HandPorted = new HashSet<string>
        {
            "AfxUSBFT232HLoadDLL", "AfxUSBFT232HFreeDLL", "AfxFT232HCreateDeviceInfoList", "AfxFT232HGetDeviceInfoList", "AfxFT232HGetHandle",
            "AfxFT232HOpen", "AfxFT232HOpenEx", "AfxFT232HClose", "AfxFT232HRead", "AfxFT232HWrite", "AfxFT232HListDevices",
            "AfxFT232HResetDevice", "AfxFT232HPurge", "AfxFT232HSetTimeouts", "AfxFT232HGetStatus", "AfxFT232HGetQueueStatus",
            "AfxFT232HSetBitMode", "AfxFT232HSetLatencyTimer", "AfxFT232HSetUSBParameters", "AfxFT232HSetFlowControl",
            "AfxFT232HSetEventNotification", "AfxFT232HPortOpen", "AfxFT232HPortClose", "AfxFT232HPortSendCommand",
            "AfxFT232HPortSendMultiCommand", "AfxFT232HPortSendAvrCommand",
            "ProcessProcFunction", "AfxThreadWaitDialog", "AfxThreadWaitDialogProcessing",
        };

        private static string _namespace = "NanoImpact.Hardware.Device";

        private sealed class Func
        {
            public int StartLine;
            public int EndLine;
            public string Leading;
            public string Signature;
            public string Body;
            public string Name;
        }

        public static void Run(string cppPath, string hPath, string outDir, string className, int chunkLines, int fromLine = 1, int toLine = int.MaxValue,
            string ns = "NanoImpact.Hardware.Device")
        {
            Encoding cp949 = Encoding.GetEncoding(949);
            string[] lines = ReadMixedLines(cppPath, cp949);
            // 기본 인자는 헤더와 .cpp 앞쪽의 전방 선언 양쪽에 있다
            Dictionary<string, List<string>> defaults = ReadDefaults(File.ReadAllText(hPath, cp949) + "\n" + string.Join("\n", lines));
            List<Func> all = Extract(lines);
            List<Func> funcs = all.Where(f => !HandPorted.Contains(f.Name) && f.StartLine >= fromLine && f.StartLine <= toLine).ToList();
            _namespace = ns;
            Console.WriteLine("손으로 옮기는 함수 {0}개 제외", all.Count - funcs.Count);
            Directory.CreateDirectory(outDir);

            var groups = new List<List<Func>>();
            foreach (Func f in funcs)
            {
                if (groups.Count == 0 || f.StartLine - groups.Last()[0].StartLine > chunkLines) groups.Add(new List<Func>());
                groups.Last().Add(f);
            }

            string src = Path.GetFileName(cppPath);
            foreach (List<Func> g in groups)
            {
                string file = Path.Combine(outDir, string.Format(CultureInfo.InvariantCulture, "{0}.L{1:D5}.cs", className, g[0].StartLine));
                var sb = new StringBuilder();
                sb.Append("// 원본 ").Append(src).Append(':').Append(g[0].StartLine).Append('-').Append(g.Last().EndLine)
                  .Append(" 을 NanoImpact.CodeGen translate로 옮긴 뒤 손으로 고친 코드.\r\n");
                sb.Append("#pragma warning disable IDE1006, IDE0059, CS0162, CS0168, CS0219\r\n");
                sb.Append("using System;\r\nusing NanoImpact.Core.Native;\r\nusing static NanoImpact.Core.Def;\r\nusing static NanoImpact.Core.Native.CRuntime;\r\n\r\n");
                sb.Append("namespace ").Append(_namespace).Append("\r\n{\r\n    public sealed partial class ").Append(className).Append("\r\n    {\r\n");
                foreach (Func f in g)
                {
                    sb.Append("        // ---- ").Append(src).Append(':').Append(f.StartLine).Append(" ----\r\n");
                    if (f.Leading.Trim().Length > 0) sb.Append(Indent(f.Leading.TrimEnd(), "        ")).Append("\r\n");
                    sb.Append("        ").Append(Signature(f.Signature, defaults)).Append("\r\n");
                    sb.Append(Indent(Body(f.Body), "        ")).Append("\r\n\r\n");
                }
                sb.Append("    }\r\n}\r\n");
                File.WriteAllText(file, sb.ToString(), new UTF8Encoding(true));
            }
            Console.WriteLine("translate: 함수 {0}개 → 파일 {1}개 ({2})", funcs.Count, groups.Count, outDir);
        }

        /// <summary>
        /// 원본 .cpp를 줄 단위로 읽는다. 대부분 CP949지만 NanoImpact2020cfeDlg.cpp는 나중에 고친 구간의 주석이 UTF-8이라,
        /// UTF-8로 깨짐 없이 읽히는 비ASCII 줄은 UTF-8로, 나머지는 CP949로 읽는다.
        /// </summary>
        private static string[] ReadMixedLines(string path, Encoding cp949)
        {
            var utf8 = new UTF8Encoding(false, true);
            byte[] all = File.ReadAllBytes(path);
            var lines = new List<string>();
            int start = 0;
            for (int i = 0; i <= all.Length; i++)
            {
                if (i < all.Length && all[i] != (byte)'\n') continue;
                int end = i > start && all[i - 1] == (byte)'\r' ? i - 1 : i;
                var seg = new ArraySegment<byte>(all, start, end - start);
                string text;
                try { text = seg.Any(b => b >= 0x80) ? utf8.GetString(seg.Array, seg.Offset, seg.Count) : cp949.GetString(seg.Array, seg.Offset, seg.Count); }
                catch (DecoderFallbackException) { text = cp949.GetString(seg.Array, seg.Offset, seg.Count); }
                lines.Add(text);
                start = i + 1;
            }
            return lines.ToArray();
        }

        // ---- 함수 떼어 내기 ----

        // 반환형 + (Class::)이름 + (인자). 멤버 함수 정의(CNanoImpact2020cfeDlg::X)도 받는다.
        private static readonly Regex SigRx = new Regex(@"^([A-Za-z_][\w\s\*]*?)\s+\**\s*(?:\w+::)?([A-Za-z_]\w*)\s*\(([^;{}]*)\)\s*$");

        private static List<Func> Extract(string[] lines)
        {
            var result = new List<Func>();
            bool[] commented = BlockCommentedLines(lines);
            int lastEnd = -1;
            for (int i = 0; i < lines.Length - 1; i++)
            {
                if (commented[i]) continue; // /* */ 안의 함수는 원본에서도 컴파일되지 않는다
                if (lines[i].Length == 0 || char.IsWhiteSpace(lines[i][0]) || !lines[i + 1].StartsWith("{")) continue;
                Match m = SigRx.Match(lines[i].TrimEnd());
                if (!m.Success) continue;
                int end = MatchBrace(lines, i + 1);
                if (end < 0) continue;

                // 바로 위의 주석 줄은 함수 설명으로 함께 가져간다
                int lead = i;
                while (lead - 1 > lastEnd && (lines[lead - 1].TrimStart().StartsWith("//") || lines[lead - 1].Trim().Length == 0)) lead--;
                result.Add(new Func
                {
                    StartLine = i + 1,
                    EndLine = end + 1,
                    Leading = string.Join("\n", lines.Skip(lead).Take(i - lead)).Trim('\n'),
                    Signature = lines[i].Trim(),
                    Body = string.Join("\n", lines.Skip(i + 1).Take(end - i)),
                    Name = m.Groups[2].Value,
                });
                lastEnd = end;
                i = end;
            }
            return result;
        }

        /// <summary>줄 시작이 /* */ 주석 안인 줄</summary>
        private static bool[] BlockCommentedLines(string[] lines)
        {
            var r = new bool[lines.Length];
            bool block = false;
            for (int i = 0; i < lines.Length; i++)
            {
                r[i] = block;
                string l = lines[i];
                for (int k = 0; k < l.Length; k++)
                {
                    char c = l[k];
                    if (block) { if (c == '*' && k + 1 < l.Length && l[k + 1] == '/') { block = false; k++; } continue; }
                    if (c == '/' && k + 1 < l.Length && l[k + 1] == '/') break;
                    if (c == '/' && k + 1 < l.Length && l[k + 1] == '*') { block = true; k++; continue; }
                    if (c == '"' || c == '\'') { k++; while (k < l.Length && l[k] != c) k += l[k] == '\\' ? 2 : 1; }
                }
            }
            return r;
        }

        private static int MatchBrace(string[] lines, int start)
        {
            int depth = 0;
            bool block = false;
            for (int i = start; i < lines.Length; i++)
            {
                string l = lines[i];
                for (int k = 0; k < l.Length; k++)
                {
                    char c = l[k];
                    if (block) { if (c == '*' && k + 1 < l.Length && l[k + 1] == '/') { block = false; k++; } continue; }
                    if (c == '/' && k + 1 < l.Length && l[k + 1] == '/') break;
                    if (c == '/' && k + 1 < l.Length && l[k + 1] == '*') { block = true; k++; continue; }
                    if (c == '"' || c == '\'') { k++; while (k < l.Length && l[k] != c) k += l[k] == '\\' ? 2 : 1; continue; }
                    if (c == '{') depth++;
                    else if (c == '}' && --depth == 0) return i;
                }
            }
            return -1;
        }

        // ---- 기본 인자 (헤더의 extern 선언에서) ----

        private static Dictionary<string, List<string>> ReadDefaults(string header)
        {
            var d = new Dictionary<string, List<string>>();
            foreach (Match m in Regex.Matches(header, @"^\s*(?:extern\s+)?[A-Za-z_][\w\s\*]*?\b(\w+)\s*\(([^)]*=[^)]*)\)\s*;", RegexOptions.Multiline))
            {
                string[] ps = m.Groups[2].Value.Split(',');
                var defs = ps.Select(p => p.Contains("=") ? p.Substring(p.IndexOf('=') + 1).Trim() : null).ToList();
                if (defs.Any(x => x != null)) d[m.Groups[1].Value + "/" + ps.Length] = defs;
            }
            return d;
        }

        private static string Signature(string sig, Dictionary<string, List<string>> defaults)
        {
            Match m = SigRx.Match(sig);
            string ret = MapType(m.Groups[1].Value.Trim(), sig.Contains("*") && !m.Groups[3].Value.Contains("*") ? 1 : 0);
            if (Regex.IsMatch(m.Groups[1].Value, @"\*\s*$") || Regex.IsMatch(sig, @"^\w+\s*\*")) ret = MapPointer(m.Groups[1].Value.Replace("*", "").Trim());
            string name = m.Groups[2].Value;
            string plist = m.Groups[3].Value.Trim();
            var outParams = new List<string>();
            string[] ps = plist.Length == 0 || plist == "void" ? new string[0] : plist.Split(',');
            defaults.TryGetValue(name + "/" + ps.Length, out List<string> defs);
            for (int i = 0; i < ps.Length; i++)
            {
                string p = Regex.Replace(ps[i], @"=.*$", "").Trim();
                Match pm = Regex.Match(p, @"^(.*?)([A-Za-z_]\w*)\s*(\[\s*\])?$");
                string type = pm.Groups[1].Value.Trim(), pname = pm.Groups[2].Value;
                bool ptr = type.EndsWith("*") || pm.Groups[3].Success;
                type = type.TrimEnd('*', ' ').Replace("const ", "").Trim();
                string cs = ptr ? MapPointer(type) : MapType(type, 0);
                string def = defs != null && i < defs.Count && defs[i] != null ? " = " + MapDefault(defs[i]) : "";
                outParams.Add(cs + " " + pname + def);
            }
            return "public " + ret + " " + name + "(" + string.Join(", ", outParams) + ")";
        }

        private static string MapDefault(string d) => d == "NULL" ? "null" : d;

        private static string MapType(string t, int _)
        {
            t = Regex.Replace(t, @"\s+", " ").Replace("unsigned int", "uint").Replace("unsigned char", "byte").Replace("unsigned short", "ushort").Replace("static ", "").Trim();
            return TypeMap.TryGetValue(t, out string cs) ? cs : t;
        }

        private static string MapPointer(string t)
        {
            switch (t)
            {
                case "char": return "string";
                case "TCHAR": return "string";
                case "BYTE": return "byte[]";
                case "WORD": return "ushort[]";
                case "int": return "int[]";
                case "double": return "double[]";
                case "void": return "object";
                default: return MapType(t, 0);
            }
        }

        // ---- 본문 변환 ----

        private static string Body(string body)
        {
            // 문자열·문자 상수·주석을 가려 두고 코드에만 규칙을 적용한다
            var saved = new List<string>();
            var masked = new StringBuilder();
            for (int k = 0; k < body.Length; k++)
            {
                char c = body[k];
                if (c == '/' && k + 1 < body.Length && (body[k + 1] == '/' || body[k + 1] == '*'))
                {
                    int e = body[k + 1] == '/' ? body.IndexOf('\n', k) : body.IndexOf("*/", k + 2, StringComparison.Ordinal) + 2;
                    if (e < k + 1) e = body.Length;
                    saved.Add(body.Substring(k, e - k));
                    masked.Append('\u0001').Append(saved.Count - 1).Append('\u0001');
                    k = e - 1;
                }
                else if (c == '"' || c == '\'')
                {
                    int e = k + 1;
                    while (e < body.Length && body[e] != c && body[e] != '\n') e += body[e] == '\\' ? 2 : 1;
                    string lit = body.Substring(k, Math.Min(e + 1, body.Length) - k);
                    saved.Add(c == '\'' ? "(byte)" + lit : lit);
                    masked.Append('\u0002').Append(saved.Count - 1).Append('\u0002');
                    k = e;
                }
                else masked.Append(c);
            }

            string s = masked.ToString();
            const string LIT = "\u0002\\d+\u0002";

            // _T("x") / _T(IDENT) → 안쪽 값
            s = Regex.Replace(s, @"\b_T\s*\(\s*(" + LIT + @"|\w+)\s*\)", "$1");
            // 전역 함수 표시 ::Func → Func, Class::Member → Class.Member
            s = Regex.Replace(s, @"(?<![\w)])::\s*(\w)", "$1");
            s = Regex.Replace(s, @"(\w)::(\w)", "$1.$2");
            s = s.Replace("->", ".");
            // 원본 Win32 캐스트는 지운다
            s = Regex.Replace(s, @"\(\s*(LPARAM|WPARAM|LPVOID|LPCTSTR|LPTSTR)\s*\)\s*&?", "");
            s = Regex.Replace(s, @"\(\s*char\s*\)", "(byte)");
            s = Regex.Replace(s, @"\(\s*char\s*\*\s*\)", "");
            s = Regex.Replace(s, @"\(\s*BYTE\s*\*\s*\)", "");

            // 지역 char 배열: char szX[N]; → CharBuffer szX = new CharBuffer(N);
            s = Regex.Replace(s, @"\bchar\s+(\w+)\s*\[([^\]]+)\]\s*(=\s*\{[^}]*\}|=\s*" + LIT + @")?\s*;", m =>
            {
                string init = m.Groups[3].Success && m.Groups[3].Value.Contains("\u0002") ? ", " + m.Groups[3].Value.Substring(m.Groups[3].Value.IndexOf('\u0002')) : "";
                return "CharBuffer " + m.Groups[1].Value + " = new CharBuffer(" + m.Groups[2].Value + init + ");";
            });

            // memset/memcpy
            s = Regex.Replace(s, @"\bmemset\s*\(\s*&?\s*([\w\.\[\]]+)\s*,\s*0(x00)?\s*,\s*sizeof\s*\(\s*(\w+)\s*\)\s*\)", m =>
            {
                string t = m.Groups[3].Value;
                if (Records.ContainsKey(t)) return m.Groups[1].Value + ".Clear()";
                if (Classes.Contains(t)) return m.Groups[1].Value + " = new " + t + "()";
                return "ZeroMem(" + m.Groups[1].Value + ")";
            });
            // memset(x, 0, sizeof(x)) 등 버퍼 전체 지우기
            s = Regex.Replace(s, @"\bmemset\s*\(\s*([\w\.\[\]]+)\s*,\s*0(?:x00)?\s*,\s*(?:sizeof\s*\(\s*\1\s*\)|[A-Z_][A-Z0-9_]*(?:\s*\+\s*1)?)\s*\)", "ZeroMem($1)");
            s = Regex.Replace(s, @"\bmemcpy\s*\(\s*&?\s*([\w\.\[\]]+)\s*,\s*&?\s*([\w\.\[\]]+)\s*,\s*sizeof\s*\(\s*(\w+)\s*\)\s*\)", m =>
                Records.ContainsKey(m.Groups[3].Value) ? m.Groups[1].Value + ".CopyFrom(" + m.Groups[2].Value + ")" : m.Value);
            s = Regex.Replace(s, @"\bsizeof\s*\(\s*(\w+)\s*\)", m =>
                Records.TryGetValue(m.Groups[1].Value, out string r) ? r + ".NativeSize" : m.Groups[1].Value.StartsWith("sz") ? m.Groups[1].Value + ".Capacity" : m.Value);
            s = Regex.Replace(s, @"\bsizeof\s*\(\s*([\w\.]+\.sz\w+)\s*\)", "$1.Capacity");

            // new STRUCT; / delete p;
            s = Regex.Replace(s, @"\bnew\s+(\w+)\s*;", m => "new " + (Records.TryGetValue(m.Groups[1].Value, out string r) ? r : m.Groups[1].Value) + "();");
            s = Regex.Replace(s, @"^\s*delete\s+[\w\.\[\]]+\s*;\s*$", "", RegexOptions.Multiline);

            // NULL: 핸들(IntPtr)·포인터(객체)·나머지(int 0)로 나눈다
            const string IDX = @"(?:\[[^\]]+\])?";
            s = Regex.Replace(s, @"\b((?:\w+\.)*(?:hEventHandle|m_hEventHandle)" + IDX + @")\s*(==|!=|=)\s*NULL\b", "$1 $2 null");
            s = Regex.Replace(s, @"\b((?:\w+\.)*(?:\w*[hH]andle\w*|m_hFT232HDevice|m_hUSBFT232H_\w+)" + IDX + @")\s*(==|!=|=)\s*NULL\b", "$1 $2 IntPtr.Zero");
            s = Regex.Replace(s, @"\b(pb[fm]Ptr\w*)\s*=\s*NULL\b", "$1 = 0");
            s = Regex.Replace(s, @"\b((?:\w+\.)*(?:m_)?p(?:sz|[A-Z])\w*" + IDX + @"(?:\.\w+)*)\s*(==|!=|=)\s*NULL\b", "$1 $2 null");
            s = Regex.Replace(s, @"\breturn\s+NULL\s*;", "return null;");
            // 2차원 구조체 필드: x.a[i][j] → x.a[i, j]
            s = Regex.Replace(s, @"\.(\w+)\[([^\[\]]+)\]\[([^\[\]]+)\]", ".$1[$2, $3]");
            // !strcmp(...) → 0 == strcmp(...)
            s = Regex.Replace(s, @"!\s*(strn?cmp\s*\()", "0 == $1");

            // 형식 이름 (선언·캐스트)
            foreach (var kv in TypeMap.Where(k => !k.Value.StartsWith("ref ")))
                s = Regex.Replace(s, @"\b" + kv.Key + @"\b(?!\s*\.)", kv.Value);
            s = Regex.Replace(s, @"\b__int64\b", "long");
            s = Regex.Replace(s, @"\bunsigned\s+long\b", "uint");
            s = Regex.Replace(s, @"(?<![\w\.])(\d+)[lL]\b", "$1"); // MSVC long은 32비트
            // 단순한 정수 조건: if (x) / while (x) / if (!x)  → != 0 / == 0  (포인터 이름은 null)
            s = Regex.Replace(s, @"\b(if|while)\s*\(\s*(!?)\s*((?:m_)?p(?:sz|[A-Z])\w*(?:\.\w+)*)\s*\)", m => m.Groups[1].Value + " (" + m.Groups[3].Value + (m.Groups[2].Value == "!" ? " == null)" : " != null)"));
            s = Regex.Replace(s, @"\b(if|while)\s*\(\s*(!?)\s*([A-Za-z_][\w\.]*(?:\[[^\]\(\)]+\])?)\s*\)", m =>
                m.Groups[3].Value == "TRUE" || m.Groups[3].Value == "FALSE" || m.Groups[3].Value == "true" || m.Groups[3].Value == "false"
                    ? m.Value : m.Groups[1].Value + " (" + m.Groups[3].Value + (m.Groups[2].Value == "!" ? " == 0)" : " != 0)"));
            s = Regex.Replace(s, @"\bunsigned\s+int\b", "uint");
            s = Regex.Replace(s, @"\bunsigned\s+char\b", "byte");
            s = Regex.Replace(s, @"\bunsigned\s+short\b", "ushort");
            // 숫자 지역 배열: ushort w[N] = {..}; → ushort[] w = NewArray<ushort>(N, ..); (C처럼 모자란 원소는 0)
            s = Regex.Replace(s, @"\b(ushort|uint|int|byte|short|double|sbyte)\s+(\w+)\s*\[([^\]]+)\]\s*(?:=\s*\{([^}]*)\})?\s*;", m =>
                m.Groups[1].Value + "[] " + m.Groups[2].Value + " = NewArray<" + m.Groups[1].Value + ">(" + m.Groups[3].Value.Trim() +
                (m.Groups[4].Success && m.Groups[4].Value.Trim().Length > 0 ? ", " + m.Groups[4].Value.Trim() : "") + ");");
            // 구조체 지역 변수: TYPE x; → TYPE x = new TYPE();
            s = Regex.Replace(s, @"^(\s*)(" + string.Join("|", Classes) + @")\s+(\w+)\s*;", "$1$2 $3 = new $2();", RegexOptions.Multiline);
            s = Regex.Replace(s, @"^(\s*)CFile\s+(\w+)\s*;", "$1CFile $2 = new CFile();", RegexOptions.Multiline);
            // 구조체 포인터 지역 변수: IntegratedCommandStruct *p = ... → IntegratedCommandStruct p = ...
            s = Regex.Replace(s, @"\b(IntegratedCommandStruct|ScanRotationCalibrationData|ClientPublicData|\w+PARAMS|USB_DEVICE_DATA_INFO)\s*\*\s*(\w+)", "$1 $2");

            // CString.Format(...) → s = Format(...)
            s = Regex.Replace(s, @"\b(\w+)\.Format\s*\(", "$1 = Format(");
            // 주소 인자: f(a, &x) → f(a, ref x)  (쉼표·여는 괄호 뒤의 &는 비트 AND가 될 수 없다)
            s = Regex.Replace(s, @"([(,]\s*)&\s*([A-Za-z_][\w\.]*(?:\[[^\]]+\])?)", "$1ref $2");

            // 되돌리기
            s = Regex.Replace(s, "[\u0001\u0002](\\d+)[\u0001\u0002]", m => saved[int.Parse(m.Groups[1].Value, CultureInfo.InvariantCulture)]);
            return s;
        }

        private static string Indent(string text, string pad) =>
            string.Join("\r\n", text.Replace("\r\n", "\n").Split('\n').Select(l => l.Length == 0 ? l : pad + l.Replace("\t", "    ")));
    }
}
