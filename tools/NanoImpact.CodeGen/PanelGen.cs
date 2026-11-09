using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NanoImpact.CodeGen
{
    /// <summary>
    /// 메인 화면 코드가 부르는 패널 메서드(m_wndXxx.Method(...))와 패널 컨트롤(m_wndXxx.m_Ctrl.SetWindowTextW 등) →
    /// C# 인터페이스(실제 패널이 구현), C# 기록 클래스(원본 대조), C++ 기록 클래스(원본 하네스).
    ///   NanoImpact.CodeGen panels &lt;원본 폴더&gt; &lt;원본.cpp&gt; &lt;줄 범위 a-b,c-d,...&gt; &lt;인터페이스.cs&gt; &lt;기록.cs&gt; &lt;panels.inc&gt;
    /// 기록 형식(양쪽 같음): "P 멤버.메서드 인자..." — 정수 %d, 실수 %.17g, 글자는 제어 문자 \xHH, 공용 데이터 포인터는 바이트 FNV 해시.
    /// </summary>
    internal static class PanelGen
    {
        private sealed class Param
        {
            public string CType;  // 원본 형식 (정규화)
            public string Name;
            public string Default; // 원본 기본값 (없으면 null)

            public string CsDefault()
            {
                if (Default == null) return "";
                string t = CsType[CType];
                if (t == "string") return " = " + (Default == "NULL" ? "null" : "\"\""); // _T("")는 헤더를 읽을 때 0으로 바뀌어 있다
                if (t == "IntegratedCommandStruct") return " = null";
                string v = Default == "TRUE" ? "1" : Default == "FALSE" || Default == "NULL" ? "0"
                    : Regex.IsMatch(Default, @"^[A-Z_][A-Z0-9_]*$") ? "NanoImpact.Core.Def." + Default : Default;
                return " = " + v;
            }

            public string CppDefault()
            {
                if (Default == null) return "";
                if (CType == "CString") return " = _T(\"\")";
                return " = " + Default;
            }
        }

        private sealed class Method
        {
            public string Name;
            public List<Param> Params;
        }

        private sealed class Panel
        {
            public string Member;   // m_wndStageControlDlg
            public string Class;    // CModularStageControlDlg
            public string Header;
            public readonly SortedSet<string> Calls = new SortedSet<string>(StringComparer.Ordinal);
            public readonly SortedSet<string> Controls = new SortedSet<string>(StringComparer.Ordinal);
            public readonly List<Method> Methods = new List<Method>();
        }

        private static readonly Dictionary<string, string> CsType = new Dictionary<string, string>
        {
            ["int"] = "int", ["BOOL"] = "int", ["UINT"] = "uint", ["DWORD"] = "uint", ["BYTE"] = "byte", ["WORD"] = "ushort",
            ["double"] = "double", ["CString"] = "string", ["char*"] = "string", ["LPCTSTR"] = "string",
            ["INTEGRATED_COMMAND_STRUCT*"] = "IntegratedCommandStruct",
        };

        public static void Run(string srcDir, string cppName, string ranges, string outIface, string outRec, string outInc)
        {
            Encoding cp949 = Encoding.GetEncoding(949);
            string[] lines = File.ReadAllLines(Path.Combine(srcDir, cppName), Encoding.UTF8); // BOM 있는 파일, 주석만 깨져도 된다
            var code = new StringBuilder();
            foreach (string r in ranges.Split(','))
            {
                string[] ab = r.Split('-');
                int a = int.Parse(ab[0]), b = int.Parse(ab[1]);
                for (int i = a; i <= b && i <= lines.Length; i++) code.Append(lines[i - 1]).Append('\n');
            }
            string body = Regex.Replace(code.ToString(), @"/\*.*?\*/", "", RegexOptions.Singleline);
            body = Regex.Replace(body, @"//[^\n]*", "");

            string mainHeader = File.ReadAllText(Path.Combine(srcDir, Path.ChangeExtension(cppName, ".h")), cp949);
            var panels = new SortedDictionary<string, Panel>(StringComparer.Ordinal);
            foreach (Match m in Regex.Matches(body, @"\b(m_wnd\w+)\.(\w+)\s*(\(|\.)"))
            {
                string member = m.Groups[1].Value;
                if (!panels.TryGetValue(member, out Panel p))
                {
                    Match d = Regex.Match(mainHeader, @"\b(C\w+)\s+" + member + @"\s*;");
                    if (!d.Success) throw new InvalidOperationException("멤버 형식을 찾지 못함: " + member);
                    p = new Panel { Member = member, Class = d.Groups[1].Value };
                    p.Header = Directory.GetFiles(srcDir, "*.h").First(f => Regex.IsMatch(File.ReadAllText(f, cp949), @"^class\s+" + p.Class + @"\s*:", RegexOptions.Multiline));
                    panels[member] = p;
                }
                if (m.Groups[3].Value == "(") p.Calls.Add(m.Groups[2].Value);
                else p.Controls.Add(m.Groups[2].Value);
            }

            foreach (Panel p in panels.Values)
            {
                string h = Regex.Replace(File.ReadAllText(p.Header, cp949), @"/\*.*?\*/", "", RegexOptions.Singleline);
                h = Regex.Replace(h, @"//[^\n]*", "");
                h = Regex.Replace(h, @"_T\s*\(\s*""[^""]*""\s*\)", "0");
                foreach (string name in p.Calls)
                {
                    Match d = Regex.Match(h, @"^\s*(?:afx_msg\s+|virtual\s+)*void\s+" + name + @"\s*\(([^)]*)\)\s*;", RegexOptions.Multiline);
                    if (!d.Success) throw new InvalidOperationException(p.Class + "." + name + ": void 선언을 찾지 못함");
                    var ps = new List<Param>();
                    int k = 0;
                    foreach (string p0 in d.Groups[1].Value.Split(','))
                    {
                        int eq = p0.IndexOf('=');
                        string def = eq >= 0 ? p0.Substring(eq + 1).Trim() : null;
                        string s = (eq >= 0 ? p0.Substring(0, eq) : p0).Trim();
                        if (s.Length == 0 || s == "void") continue;
                        Match pm = Regex.Match(s, @"^(.*?)([A-Za-z_]\w*)$");
                        string type = pm.Groups[1].Value.Trim();
                        string pname = pm.Groups[2].Value;
                        if (type.Length == 0) { type = s; pname = "a" + k; } // 이름 없는 인자
                        type = Regex.Replace(type.Replace("const ", ""), @"\s+", "");
                        if (!CsType.ContainsKey(type)) throw new InvalidOperationException(p.Class + "." + name + ": 모르는 인자 형식 " + type);
                        ps.Add(new Param { CType = type, Name = pname, Default = def });
                        k++;
                    }
                    p.Methods.Add(new Method { Name = name, Params = ps });
                }
            }

            WriteInterfaces(panels.Values, outIface);
            WriteRecorders(panels.Values, outRec);
            WriteInc(panels.Values, outInc);
            Console.WriteLine("panels: 패널 {0}개, 메서드 {1}개, 컨트롤 {2}개", panels.Count, panels.Values.Sum(p => p.Methods.Count), panels.Values.Sum(p => p.Controls.Count));
        }

        private static string IfaceName(Panel p) => "I" + p.Class.Substring(1);

        private static void WriteInterfaces(IEnumerable<Panel> panels, string path)
        {
            var sb = new StringBuilder();
            sb.Append("// Generated by NanoImpact.CodeGen panels. Do not edit.\r\n");
            sb.Append("// 메인 화면 장치 처리기가 부르는 패널 메서드·컨트롤. 실제 패널(8단계)이 구현하고, 원본 대조는 기록 클래스가 구현한다.\r\n");
            sb.Append("#pragma warning disable IDE1006 // 원본 이름 그대로\r\nusing NanoImpact.Core.Native;\r\n\r\nnamespace NanoImpact.Hardware.Device\r\n{\r\n");
            sb.Append("    /// <summary>패널의 글자 컨트롤 (원본 CStatic/CEdit/CTextProgressCtrl)</summary>\r\n");
            sb.Append("    public interface ITextControl\r\n    {\r\n        void SetWindowTextW(string text);\r\n        void SetTextDataString(string text);\r\n    }\r\n\r\n");
            foreach (Panel p in panels)
            {
                sb.Append("    /// <summary>원본 ").Append(p.Class).Append(" (").Append(Path.GetFileName(p.Header)).Append(") 중 메인 화면이 부르는 것</summary>\r\n");
                sb.Append("    public interface ").Append(IfaceName(p)).Append("\r\n    {\r\n");
                foreach (string c in p.Controls) sb.Append("        ITextControl ").Append(c).Append(" { get; }\r\n");
                foreach (Method m in p.Methods)
                    sb.Append("        void ").Append(m.Name).Append('(').Append(string.Join(", ", m.Params.Select(x => CsType[x.CType] + " " + x.Name + x.CsDefault()))).Append(");\r\n");
                sb.Append("    }\r\n\r\n");
            }
            sb.Append("}\r\n");
            File.WriteAllText(path, sb.ToString(), new UTF8Encoding(true));
        }

        private static string CsArg(Param x)
        {
            switch (CsType[x.CType])
            {
                case "double": return "Dbl(" + x.Name + ")";
                case "string": return "Txt(" + x.Name + ")";
                case "IntegratedCommandStruct": return "Hash(" + x.Name + ")";
                default: return x.Name + ".ToString(CultureInfo.InvariantCulture)";
            }
        }

        private static void WriteRecorders(IEnumerable<Panel> panels, string path)
        {
            var sb = new StringBuilder();
            sb.Append("// Generated by NanoImpact.CodeGen panels. Do not edit.\r\n");
            sb.Append("// tools\\device_harness panels.inc와 같은 기록: \"P 멤버.메서드 인자...\"\r\n");
            sb.Append("using System.Globalization;\r\nusing System.IO;\r\nusing NanoImpact.Core.Native;\r\nusing NanoImpact.Hardware.Device;\r\n\r\nnamespace NanoImpact.Verify\r\n{\r\n");
            sb.Append("    internal static partial class PanelRecorders\r\n    {\r\n");
            foreach (Panel p in panels)
            {
                string cls = "Rec" + p.Class.Substring(1);
                sb.Append("        internal sealed class ").Append(cls).Append(" : ").Append(IfaceName(p)).Append("\r\n        {\r\n");
                sb.Append("            private readonly TextWriter _out;\r\n");
                sb.Append("            public ").Append(cls).Append("(TextWriter output)\r\n            {\r\n                _out = output;\r\n");
                foreach (string c in p.Controls) sb.Append("                ").Append(c).Append(" = new RecText(output, \"").Append(p.Member).Append('.').Append(c).Append("\");\r\n");
                sb.Append("            }\r\n");
                foreach (string c in p.Controls) sb.Append("            public ITextControl ").Append(c).Append(" { get; }\r\n");
                foreach (Method m in p.Methods)
                {
                    sb.Append("            public void ").Append(m.Name).Append('(').Append(string.Join(", ", m.Params.Select(x => CsType[x.CType] + " " + x.Name))).Append(") => _out.WriteLine(\"P ")
                      .Append(p.Member).Append('.').Append(m.Name).Append('"');
                    foreach (Param x in m.Params) sb.Append(" + \" \" + ").Append(CsArg(x));
                    sb.Append(");\r\n");
                }
                sb.Append("        }\r\n\r\n");
            }
            sb.Append("    }\r\n}\r\n");
            File.WriteAllText(path, sb.ToString(), new UTF8Encoding(true));
        }

        private static void WriteInc(IEnumerable<Panel> panels, string path)
        {
            var sb = new StringBuilder();
            sb.Append("// Generated by NanoImpact.CodeGen panels. Do not edit. Panel stand-ins that record what the main dialog asks them.\r\n");
            foreach (Panel p in panels)
            {
                sb.Append("class ").Append(p.Class).Append("\r\n{\r\npublic:\r\n");
                foreach (string c in p.Controls) sb.Append("    CTextRec ").Append(c).Append("{ \"").Append(p.Member).Append('.').Append(c).Append("\" };\r\n");
                foreach (Method m in p.Methods)
                {
                    sb.Append("    void ").Append(m.Name).Append('(').Append(string.Join(", ", m.Params.Select(x => (x.CType == "INTEGRATED_COMMAND_STRUCT*" ? "INTEGRATED_COMMAND_STRUCT*" : x.CType == "char*" ? "const char*" : x.CType) + " " + x.Name + x.CppDefault()))).Append(")\r\n    {\r\n");
                    sb.Append("        PanelLine(\"").Append(p.Member).Append('.').Append(m.Name).Append("\");\r\n");
                    foreach (Param x in m.Params)
                    {
                        switch (CsType[x.CType])
                        {
                            case "double": sb.Append("        PanelDbl(").Append(x.Name).Append(");\r\n"); break;
                            case "string": sb.Append("        PanelTxt(").Append(x.CType == "char*" ? "CString(" + x.Name + ")" : "(LPCTSTR)" + x.Name).Append(");\r\n"); break;
                            case "IntegratedCommandStruct": sb.Append("        PanelHash(").Append(x.Name).Append(", sizeof(INTEGRATED_COMMAND_STRUCT));\r\n"); break;
                            case "uint": sb.Append("        PanelUInt((unsigned)").Append(x.Name).Append(");\r\n"); break;
                            default: sb.Append("        PanelInt((int)").Append(x.Name).Append(");\r\n"); break;
                        }
                    }
                    sb.Append("        PanelEnd();\r\n    }\r\n");
                }
                sb.Append("};\r\n\r\n");
            }
            File.WriteAllText(path, sb.ToString(), Encoding.ASCII);
        }
    }
}
