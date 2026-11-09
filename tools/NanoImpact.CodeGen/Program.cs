// 원본 DefineStructCode.h를 읽어 C# 코드를 만든다.
//   NanoImpact.CodeGen [DefineStructCode.h 경로] [저장소 루트]
//   NanoImpact.CodeGen dispatch <USB_FT232H_Device.h> <dispatch.inc>   (원본 대조 하네스용 호출 표)
// 결과:
//   src\NanoImpact.Core\Generated\Defines.g.cs   — #define 전부 (C 규칙으로 계산한 값, 원본 이름)
//   src\NanoImpact.Core\Generated\Records.g.cs   — 설정 파일로 저장되는 구조체 (MSVC x86 배치)
//   tools\layout_probe\probe_fields.inc          — cl.exe로 같은 구조체의 sizeof/offsetof를 찍는 목록
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NanoImpact.CodeGen
{
    internal static class Program
    {
        private const string DefaultHeader = @"C:\Users\Administrator\Downloads\NanoImpact_20230510\NanoImpact\source\NanoImpact2020cfe\DefineStructCode.h";

        /// <summary>설정 파일로 저장되는 구조체 → C# 클래스 이름</summary>
        private static readonly Dictionary<string, string> Records = new Dictionary<string, string>
        {
            ["INTEGRATED_COMMAND_STRUCT"] = "IntegratedCommandStruct",              // USBINTEGRATEDDATA.INI
            ["CLIENT_PUBLIC_DATA"] = "ClientPublicData",                            // CLIENTPUBLICDATA.INI
            ["SEM_SCAN_ROTATION_CALIBRATION_DATA"] = "ScanRotationCalibrationData", // SROTCALIBRATIONDATA.INI
        };

        private static int Main(string[] args)
        {
            try
            {
                if (args.Length >= 5 && args[0] == "translate")
                {
                    // NanoImpact.CodeGen translate <원본.cpp> <원본.h> <출력 폴더> <클래스> [묶음 줄 수] [시작 줄] [끝 줄] [네임스페이스]
                    CppTranslator.Run(args[1], args[2], args[3], args[4], args.Length > 5 ? int.Parse(args[5]) : 2000,
                        args.Length > 6 ? int.Parse(args[6]) : 1, args.Length > 7 ? int.Parse(args[7]) : int.MaxValue,
                        args.Length > 8 ? args[8] : "NanoImpact.Hardware.Device");
                    return 0;
                }
                if (args.Length >= 3 && args[0] == "dispatch")
                {
                    // NanoImpact.CodeGen dispatch <USB_FT232H_Device.h> <dispatch.inc>
                    DispatchGen.Run(args[1], args[2]);
                    return 0;
                }
                if (args.Length >= 5 && args[0] == "dispatch-class")
                {
                    // NanoImpact.CodeGen dispatch-class <ModularAssistDevice.h> <assist_dispatch.inc> A g_assist
                    DispatchGen.RunClass(args[1], args[2], args[3], args[4]);
                    return 0;
                }
                string header = args.Length > 0 ? args[0] : DefaultHeader;
                string root = args.Length > 1 ? args[1] : FindRepoRoot();
                Run(header, root);
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.GetType().Name + ": " + ex.Message);
                return 1;
            }
        }

        private static string FindRepoRoot()
        {
            string d = AppDomain.CurrentDomain.BaseDirectory;
            while (d != null && !File.Exists(Path.Combine(d, "NanoImpactNET.sln"))) d = Path.GetDirectoryName(d.TrimEnd('\\'));
            return d ?? throw new InvalidOperationException("NanoImpactNET.sln을 찾지 못했습니다. 저장소 루트를 인자로 주세요.");
        }

        private sealed class DefineEntry
        {
            public string Name;
            public string Source;
            public string Comment;
            public int Line;
            public CVal Value;
            public string Error;
            public bool IsSimpleHex;
            public List<string> Earlier = new List<string>();
        }

        private sealed class FieldInfo
        {
            public string Type;
            public string Name;
            public int[] Dims;
            public string DimText;
            public string Comment;
            public int Offset;
            public int ElemSize;
            public int Size;
        }

        private sealed class RecordInfo
        {
            public string CName;
            public string CsName;
            public int Line;
            public List<FieldInfo> Fields = new List<FieldInfo>();
            public int Size;
        }

        private static void Run(string headerPath, string root)
        {
            string[] lines = StripBlockComments(ReadSource(headerPath)).Split('\n');

            var table = new Dictionary<string, CVal>();
            var eval = new CExpression(table);
            var defines = new List<DefineEntry>();
            var byName = new Dictionary<string, DefineEntry>();
            var records = new List<RecordInfo>();

            var defineRx = new Regex(@"^\s*#\s*define\s+([A-Za-z_]\w*)(?!\()(?:\s+(.*))?$");
            var structEndRx = new Regex(@"^\s*\}\s*(\w+)\s*(,.*)?;");

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].TrimEnd('\r');
                while (line.EndsWith("\\") && i + 1 < lines.Length) line = line.Substring(0, line.Length - 1) + " " + lines[++i].TrimEnd('\r');

                Match m = defineRx.Match(line);
                if (m.Success)
                {
                    SplitComment(m.Groups[2].Value, out string value, out string comment);
                    value = value.Trim().TrimEnd(';').Trim();
                    if (value.Length == 0) continue; // #define _HEAD_ 같은 표시용

                    var e = new DefineEntry { Name = m.Groups[1].Value, Source = value, Comment = comment, Line = i + 1 };
                    e.IsSimpleHex = Regex.IsMatch(value, @"^0[xX][0-9A-Fa-f]+[uUlL]*$");
                    try
                    {
                        e.Value = eval.Evaluate(value);
                        table[e.Name] = e.Value;
                    }
                    catch (CEvalException ex)
                    {
                        e.Error = ex.Message;
                        table.Remove(e.Name);
                    }

                    if (byName.TryGetValue(e.Name, out DefineEntry prev))
                    {
                        e.Earlier.AddRange(prev.Earlier);
                        e.Earlier.Add(prev.Error == null ? prev.Value.ToString() : prev.Source);
                        defines.Remove(prev);
                    }
                    byName[e.Name] = e;
                    defines.Add(e);
                    continue;
                }

                if (Regex.IsMatch(line, @"^\s*typedef\s+struct\b"))
                {
                    // 본문은 따로 훑고 i는 그대로 둔다 (구조체 안에 #define이 있어도 위의 처리를 거치도록)
                    var body = new List<KeyValuePair<int, string>>();
                    string cname = null;
                    for (int j = i + 1; j < lines.Length; j++)
                    {
                        string l2 = lines[j].TrimEnd('\r');
                        Match em = structEndRx.Match(l2);
                        if (em.Success) { cname = em.Groups[1].Value; break; }
                        body.Add(new KeyValuePair<int, string>(j + 1, l2));
                    }
                    if (cname != null && Records.TryGetValue(cname, out string csName))
                        records.Add(Layout(cname, csName, i + 1, body, eval));
                }
            }

            foreach (string want in Records.Keys)
                if (!records.Any(r => r.CName == want)) throw new InvalidOperationException("구조체를 찾지 못함: " + want);

            string genDir = Path.Combine(root, "src", "NanoImpact.Core", "Generated");
            Directory.CreateDirectory(genDir);
            WriteUtf8Bom(Path.Combine(genDir, "Defines.g.cs"), EmitDefines(defines, headerPath));
            WriteUtf8Bom(Path.Combine(genDir, "Records.g.cs"), EmitRecords(records));
            string probeDir = Path.Combine(root, "tools", "layout_probe");
            Directory.CreateDirectory(probeDir);
            WriteUtf8Bom(Path.Combine(probeDir, "probe_fields.inc"), EmitProbe(records));

            int failed = defines.Count(d => d.Error != null);
            Console.WriteLine("#define {0}개: 변환 {1}, 변환 못 함 {2}, 재정의 {3}", defines.Count, defines.Count - failed, failed, defines.Count(d => d.Earlier.Count > 0));
            foreach (DefineEntry d in defines.Where(d => d.Error != null)) Console.WriteLine("  못 함 {0}:{1} {2} = {3} ({4})", Path.GetFileName(headerPath), d.Line, d.Name, d.Source, d.Error);
            foreach (RecordInfo r in records) Console.WriteLine("구조체 {0}: 필드 {1}, x86 크기 {2}", r.CName, r.Fields.Count, r.Size);
        }

        // ---- 원본 읽기 ----

        private static string ReadSource(string path)
        {
            byte[] b = File.ReadAllBytes(path);
            if (b.Length >= 3 && b[0] == 0xEF && b[1] == 0xBB && b[2] == 0xBF) return Encoding.UTF8.GetString(b, 3, b.Length - 3);
            return Encoding.GetEncoding(949).GetString(b); // 원본은 CP949
        }

        /// <summary>/* */ 주석을 지우되 줄 수는 유지. 문자열·문자 상수·// 주석 안의 /*는 건드리지 않는다.</summary>
        private static string StripBlockComments(string s)
        {
            var sb = new StringBuilder(s.Length);
            int i = 0;
            while (i < s.Length)
            {
                char c = s[i];
                if (c == '"' || c == '\'')
                {
                    int j = i + 1;
                    while (j < s.Length && s[j] != c && s[j] != '\n') j += s[j] == '\\' ? 2 : 1;
                    j = Math.Min(j + 1, s.Length);
                    sb.Append(s, i, j - i);
                    i = j;
                }
                else if (c == '/' && i + 1 < s.Length && s[i + 1] == '/')
                {
                    int j = s.IndexOf('\n', i);
                    if (j < 0) j = s.Length;
                    sb.Append(s, i, j - i);
                    i = j;
                }
                else if (c == '/' && i + 1 < s.Length && s[i + 1] == '*')
                {
                    int j = s.IndexOf("*/", i + 2, StringComparison.Ordinal);
                    if (j < 0) j = s.Length - 2;
                    for (int k = i; k < j + 2; k++) sb.Append(s[k] == '\n' ? '\n' : ' ');
                    i = j + 2;
                }
                else
                {
                    sb.Append(c);
                    i++;
                }
            }
            return sb.ToString();
        }

        /// <summary>값과 // 주석 나누기 (문자열 안의 //는 무시)</summary>
        private static void SplitComment(string text, out string value, out string comment)
        {
            bool inStr = false;
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (c == '"' && (i == 0 || text[i - 1] != '\\')) inStr = !inStr;
                if (!inStr && c == '/' && i + 1 < text.Length && text[i + 1] == '/')
                {
                    value = text.Substring(0, i);
                    comment = text.Substring(i + 2).Trim();
                    return;
                }
            }
            value = text;
            comment = "";
        }

        // ---- 구조체 배치 (MSVC x86, /Zp8) ----

        private static readonly Dictionary<string, KeyValuePair<int, string>> Prims = new Dictionary<string, KeyValuePair<int, string>>
        {
            ["char"] = new KeyValuePair<int, string>(1, "sbyte"),
            ["BYTE"] = new KeyValuePair<int, string>(1, "byte"),
            ["UCHAR"] = new KeyValuePair<int, string>(1, "byte"),
            ["short"] = new KeyValuePair<int, string>(2, "short"),
            ["WORD"] = new KeyValuePair<int, string>(2, "ushort"),
            ["USHORT"] = new KeyValuePair<int, string>(2, "ushort"),
            ["int"] = new KeyValuePair<int, string>(4, "int"),
            ["BOOL"] = new KeyValuePair<int, string>(4, "int"),
            ["long"] = new KeyValuePair<int, string>(4, "int"),
            ["LONG"] = new KeyValuePair<int, string>(4, "int"),
            ["UINT"] = new KeyValuePair<int, string>(4, "uint"),
            ["DWORD"] = new KeyValuePair<int, string>(4, "uint"),
            ["ULONG"] = new KeyValuePair<int, string>(4, "uint"),
            ["COLORREF"] = new KeyValuePair<int, string>(4, "uint"),
            ["float"] = new KeyValuePair<int, string>(4, "float"),
            ["double"] = new KeyValuePair<int, string>(8, "double"),
            ["ULONGLONG"] = new KeyValuePair<int, string>(8, "ulong"),
        };

        private static readonly HashSet<string> Reserved = new HashSet<string>
        {
            "Raw", "Size", "CopyFrom", "Clear", "ToBytes", "LoadFile", "SaveFile", "Read", "Write", "Layout", "NativeSize",
        };

        private static RecordInfo Layout(string cname, string csName, int line, List<KeyValuePair<int, string>> body, CExpression eval)
        {
            var rec = new RecordInfo { CName = cname, CsName = csName, Line = line };
            var fieldRx = new Regex(@"^\s*(\w+)\s+(\w+)\s*((?:\[[^\]]+\]\s*)*);\s*(?://(.*))?$");
            int offset = 0, maxAlign = 1;
            foreach (var kv in body)
            {
                string l = kv.Value;
                if (l.Trim().Length == 0 || l.Trim() == "{" || l.TrimStart().StartsWith("//")) continue;
                Match m = fieldRx.Match(l);
                if (!m.Success) throw new InvalidOperationException(cname + " " + kv.Key + "행: 필드로 해석 못 함: " + l.Trim());
                string type = m.Groups[1].Value;
                if (!Prims.TryGetValue(type, out var prim)) throw new InvalidOperationException(cname + " " + kv.Key + "행: 모르는 형식 " + type);
                var dims = new List<int>();
                foreach (Match dm in Regex.Matches(m.Groups[3].Value, @"\[([^\]]+)\]"))
                {
                    CVal v = eval.Evaluate(dm.Groups[1].Value);
                    if (!v.IsInteger || v.I <= 0) throw new InvalidOperationException(cname + "." + m.Groups[2].Value + " 배열 크기: " + dm.Groups[1].Value);
                    dims.Add((int)v.I);
                }
                if (dims.Count > 2) throw new InvalidOperationException(cname + "." + m.Groups[2].Value + ": 3차원 이상 배열은 지원 안 함");
                if (Reserved.Contains(m.Groups[2].Value)) throw new InvalidOperationException(cname + "." + m.Groups[2].Value + ": 기반 클래스 이름과 겹침");

                int elem = prim.Key;
                int align = Math.Min(elem, 8);
                offset = (offset + align - 1) / align * align;
                int count = dims.Aggregate(1, (a, b) => a * b);
                rec.Fields.Add(new FieldInfo
                {
                    Type = type,
                    Name = m.Groups[2].Value,
                    Dims = dims.ToArray(),
                    DimText = m.Groups[3].Value.Replace(" ", ""),
                    Comment = m.Groups[4].Value.Trim(),
                    Offset = offset,
                    ElemSize = elem,
                    Size = elem * count,
                });
                offset += elem * count;
                maxAlign = Math.Max(maxAlign, align);
            }
            rec.Size = (offset + maxAlign - 1) / maxAlign * maxAlign;
            return rec;
        }

        // ---- 출력 ----

        private const string GeneratedBanner =
            "// <auto-generated>\r\n" +
            "// NanoImpact.CodeGen이 원본 DefineStructCode.h에서 만든 파일입니다. 직접 고치지 말고 생성기를 다시 실행하세요.\r\n" +
            "// </auto-generated>\r\n";

        private static string EmitDefines(List<DefineEntry> defines, string headerPath)
        {
            var sb = new StringBuilder(GeneratedBanner);
            sb.Append("#pragma warning disable IDE1006 // 원본 이름 그대로\r\n");
            sb.Append("namespace NanoImpact.Core\r\n{\r\n");
            sb.Append("    /// <summary>\r\n");
            sb.Append("    /// 원본 ").Append(Path.GetFileName(headerPath)).Append("의 #define. 이름은 원본 그대로, 값은 C 규칙(MSVC x86)으로 미리 계산했다.\r\n");
            sb.Append("    /// 같은 이름이 여러 번 정의된 것은 마지막 값(원본 .cpp에서 보이는 값)을 쓴다.\r\n");
            sb.Append("    /// </summary>\r\n");
            sb.Append("    public static partial class Def\r\n    {\r\n");
            foreach (DefineEntry d in defines)
            {
                var notes = new List<string>();
                if (d.Error == null && !IsLiteral(d.Source)) notes.Add("= " + d.Source);
                if (d.Earlier.Count > 0 && d.Earlier.Any(x => x != (d.Error == null ? d.Value.ToString() : d.Source)))
                    notes.Add("원본에서 재정의됨, 이전 값 " + string.Join(", ", d.Earlier));
                if (d.Comment.Length > 0) notes.Add(d.Comment);
                string note = notes.Count > 0 ? " // " + string.Join(" | ", notes) : "";

                if (d.Error != null)
                {
                    sb.Append("        // [변환 못 함: ").Append(d.Error).Append("] ").Append(d.Name).Append(" = ").Append(d.Source).Append(note).Append("\r\n");
                    continue;
                }
                sb.Append("        public const ").Append(CsType(d.Value)).Append(' ').Append(d.Name).Append(" = ")
                  .Append(CsLiteral(d.Value, d.IsSimpleHex ? d.Source.TrimEnd('u', 'U', 'l', 'L') : null)).Append(';').Append(note).Append("\r\n");
            }
            sb.Append("    }\r\n}\r\n");
            return sb.ToString();
        }

        private static bool IsLiteral(string s) =>
            Regex.IsMatch(s, @"^-?(\d+[uUlL]*|0[xX][0-9A-Fa-f]+[uUlL]*|\d*\.\d+([eE][-+]?\d+)?[fF]?|\d+\.\d*[fF]?)$") || Regex.IsMatch(s, "^\"([^\"\\\\]|\\\\.)*\"$");

        private static string CsType(CVal v)
        {
            switch (v.Kind)
            {
                case CKind.Int: return "int";
                case CKind.UInt: return "uint";
                case CKind.Long: return "long";
                case CKind.Double: return "double";
                default: return "string";
            }
        }

        private static string CsLiteral(CVal v, string hexText)
        {
            switch (v.Kind)
            {
                case CKind.Double:
                    string d = v.D.ToString("R", CultureInfo.InvariantCulture);
                    return d.Contains(".") || d.Contains("E") ? d : d + ".0";
                case CKind.Str:
                    return Quote(v.S);
                case CKind.UInt:
                    return (hexText ?? v.I.ToString(CultureInfo.InvariantCulture)) + "u";
                case CKind.Long:
                    return (hexText ?? v.I.ToString(CultureInfo.InvariantCulture)) + "L";
                default:
                    return hexText ?? v.I.ToString(CultureInfo.InvariantCulture);
            }
        }

        private static string Quote(string s)
        {
            var sb = new StringBuilder("\"");
            foreach (char c in s)
            {
                switch (c)
                {
                    case '\\': sb.Append("\\\\"); break;
                    case '"': sb.Append("\\\""); break;
                    case '\n': sb.Append("\\n"); break;
                    case '\r': sb.Append("\\r"); break;
                    case '\t': sb.Append("\\t"); break;
                    case '\0': sb.Append("\\0"); break;
                    default:
                        if (c < 0x20) sb.Append("\\u").Append(((int)c).ToString("X4"));
                        else sb.Append(c);
                        break;
                }
            }
            return sb.Append('"').ToString();
        }

        private static string EmitRecords(List<RecordInfo> records)
        {
            var sb = new StringBuilder(GeneratedBanner);
            sb.Append("#pragma warning disable IDE1006 // 원본 필드 이름 그대로\r\n");
            sb.Append("namespace NanoImpact.Core.Native\r\n{\r\n");
            foreach (RecordInfo r in records)
            {
                sb.Append("    /// <summary>원본 ").Append(r.CName).Append(" (DefineStructCode.h:").Append(r.Line).Append("), MSVC x86 크기 ").Append(r.Size).Append(" 바이트</summary>\r\n");
                sb.Append("    public sealed partial class ").Append(r.CsName).Append(" : NativeRecord\r\n    {\r\n");
                sb.Append("        public const int NativeSize = ").Append(r.Size).Append(";\r\n\r\n");
                // 배열·문자열 필드는 뷰 객체를 생성자에서 한 번 만들어 둔다 (ic.nSyncVal[i] = v 처럼 바로 쓸 수 있게)
                sb.Append("        public ").Append(r.CsName).Append("() : base(NativeSize)\r\n        {\r\n");
                foreach (FieldInfo f in r.Fields.Where(x => x.Dims.Length > 0))
                    sb.Append("            ").Append(f.Name).Append(" = ").Append(ViewNew(f)).Append(";\r\n");
                sb.Append("        }\r\n");
                foreach (FieldInfo f in r.Fields)
                {
                    sb.Append("\r\n");
                    string summary = f.Type + " " + f.Name + f.DimText + (f.Comment.Length > 0 ? " — " + f.Comment : "");
                    sb.Append("        /// <summary>").Append(System.Security.SecurityElement.Escape(summary)).Append("</summary>\r\n");
                    if (f.Dims.Length == 0)
                        sb.AppendFormat(CultureInfo.InvariantCulture, "        public {0} {1} {{ get => Read<{0}>({2}); set => Write({2}, value); }}\r\n", Prims[f.Type].Value, f.Name, f.Offset);
                    else
                        sb.Append("        public ").Append(ViewType(f)).Append(' ').Append(f.Name).Append(" { get; }\r\n");
                }
                sb.Append("\r\n        /// <summary>필드 배치 (검증용)</summary>\r\n");
                sb.Append("        public static readonly NativeField[] Layout =\r\n        {\r\n");
                foreach (FieldInfo f in r.Fields)
                    sb.AppendFormat(CultureInfo.InvariantCulture, "            new NativeField(\"{0}\", {1}, {2}),\r\n", f.Name, f.Offset, f.Size);
                sb.Append("        };\r\n    }\r\n\r\n");
            }
            sb.Length -= 2;
            sb.Append("}\r\n");
            return sb.ToString();
        }

        private static string ViewType(FieldInfo f)
        {
            string cs = Prims[f.Type].Value;
            if (f.Type == "char") return f.Dims.Length == 1 ? "NativeString" : "NativeStringArray";
            return f.Dims.Length == 1 ? "NativeArray<" + cs + ">" : "NativeArray2<" + cs + ">";
        }

        private static string ViewNew(FieldInfo f)
        {
            string dims = string.Join(", ", f.Dims.Select(d => d.ToString(CultureInfo.InvariantCulture)));
            return "new " + ViewType(f) + "(Raw, " + f.Offset.ToString(CultureInfo.InvariantCulture) + ", " + dims + ")";
        }

        private static string EmitProbe(List<RecordInfo> records)
        {
            var sb = new StringBuilder("// NanoImpact.CodeGen이 만든 목록. probe.cpp가 include한다.\r\n");
            foreach (RecordInfo r in records)
            {
                sb.Append("S(").Append(r.CName).Append(")\r\n");
                foreach (FieldInfo f in r.Fields) sb.Append("F(").Append(r.CName).Append(", ").Append(f.Name).Append(")\r\n");
            }
            return sb.ToString();
        }

        private static void WriteUtf8Bom(string path, string text) => File.WriteAllText(path, text, new UTF8Encoding(true));
    }
}
