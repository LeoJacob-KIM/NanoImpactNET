using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using NanoImpact.Core.Native;
using NanoImpact.Hardware.Device;
using NanoImpact.Hardware.Ftdi;

namespace NanoImpact.Verify
{
    /// <summary>
    /// tools\device_harness\harness.cpp(원본 USB_FT232H_Device.cpp 실행)와 같은 스크립트·같은 출력 형식으로 C# SemDevice를 돌린다.
    ///   NanoImpact.Verify device &lt;script&gt;   (현재 폴더 기준으로 PATH 등을 푼다)
    /// </summary>
    internal sealed class DeviceHarness
    {
        private readonly TextWriter _out;
        private readonly FakeFtdi _ftdi;
        private readonly RecordingHost _host;
        private readonly SemDevice _dev;
        private readonly AssistDevice _assist;
        private readonly VACUUM_BEAM_ALIGN_FOCUS_DATA _vbaf = new VACUUM_BEAM_ALIGN_FOCUS_DATA();
        private byte[] _prevIntegrated;
        private byte[] _prevSRot;
        private List<string> _prevAssist = new List<string>();

        public DeviceHarness(TextWriter output)
        {
            _out = output;
            _ftdi = new FakeFtdi(output);
            _host = new RecordingHost(output);
            _dev = new SemDevice(_host, _ftdi);
            // harness.cpp main과 같은 ModularAssistDevice 연결 (원본 OnInitDialog)
            _assist = new AssistDevice(_dev, _host);
            _assist.SetOwner(1);
            _assist.SetOwnerWindow(new RecordingOwner(output));
            _assist.SetVacuumBeamAlignFocusPublicData(_vbaf);
            DiffAssist();
        }

        public static int Run(string script)
        {
            var stdout = new StreamWriter(Console.OpenStandardOutput(), new UTF8Encoding(false)) { AutoFlush = true, NewLine = "\n" };
            // 원본의 배열 범위 밖 접근은 비교 출력(stdout)에 섞지 않고 <스크립트>.out_of_range.txt(현재 폴더)에 모은다
            var oor = new Dictionary<string, int>();
            string current = "";
            NanoImpact.Core.Native.NativeDiagnostics.OutOfRange += s => { string k = current + ": " + s; oor[k] = oor.TryGetValue(k, out int c) ? c + 1 : 1; };
            var h = new DeviceHarness(stdout);
            h._onCall = name => current = name;
            foreach (string raw in File.ReadLines(script))
            {
                string line = raw.TrimEnd('\r', '\n');
                if (line.Length == 0 || line[0] == '#') continue;
                string[] tok = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                h.Execute(tok);
            }
            File.WriteAllLines(Path.GetFileNameWithoutExtension(script) + ".out_of_range.txt",
                oor.Select(kv => string.Format(CultureInfo.InvariantCulture, "{0} ({1}회)", kv.Key, kv.Value)), new UTF8Encoding(true));
            return 0;
        }

        private Action<string> _onCall = _ => { };

        private void Execute(string[] tok)
        {
            switch (tok[0])
            {
                case "CALL" when tok.Length >= 2:
                    _out.WriteLine("CALL " + string.Join(" ", tok.Skip(1)));
                    _onCall(tok[1]);
                    Call(tok[1], tok.Skip(2).ToArray());
                    break;
                case "SEREAD":
                    // SE 영상 읽기 스레드 본문 1회 (인자가 구조체라 호출 표에 없다)
                    // AfxThreadSEImageDataProcessing의 WAIT_OBJECT_0 한 바퀴처럼: 수신 허용 플래그를 먼저 복사 (원본 16810행)
                    _out.WriteLine("CALL SEREAD");
                    _onCall("SEREAD");
                    if (_dev.SEImageDataThreadParamsForTest != null && _dev.IntegratedForTest != null)
                        _dev.SEImageDataThreadParamsForTest.bScanDataReceive = _dev.IntegratedForTest.bScanDataReceive;
                    _out.WriteLine("RET " + _dev.AfxSEScanDataImageProcess(_dev.SEImageDataThreadParamsForTest));
                    DiffState();
                    break;
                case "PATH" when tok.Length >= 2:
                    _dev.AfxSetDeviceServerPath(tok[1]);
                    break;
                case "TICK" when tok.Length >= 2:
                    _host.Tick = (uint)ParseCInt(tok[1]);
                    break;
                case "BOXRET" when tok.Length >= 2:
                    _host.BoxReturn = (int)ParseCInt(tok[1]);
                    break;
                case "RX" when tok.Length >= 3:
                    _ftdi.PushRx((int)ParseCInt(tok[1]), tok.Skip(2).Select(x => byte.Parse(x, NumberStyles.HexNumber)).ToArray());
                    break;
                default:
                    _out.WriteLine("UNKNOWN " + tok[0]);
                    break;
            }
        }

        // ---- 원본 키 "AfxName(type,type)" → C# 메서드 ----

        private static readonly Dictionary<string, Type[]> CTypes = new Dictionary<string, Type[]>
        {
            ["int"] = new[] { typeof(int) }, ["INT"] = new[] { typeof(int) }, ["BOOL"] = new[] { typeof(int) }, ["HWND"] = new[] { typeof(int) },
            ["UINT"] = new[] { typeof(uint) }, ["DWORD"] = new[] { typeof(uint) }, ["ULONG"] = new[] { typeof(uint) },
            ["WORD"] = new[] { typeof(ushort) }, ["USHORT"] = new[] { typeof(ushort) },
            ["BYTE"] = new[] { typeof(byte) }, ["UCHAR"] = new[] { typeof(byte) },
            ["double"] = new[] { typeof(double) },
            ["char*"] = new[] { typeof(string), typeof(ICharArray), typeof(CharBuffer) },
            ["CString"] = new[] { typeof(string) },
            ["REQUEST_USB_DEVICE_DATA*"] = new[] { typeof(REQUEST_USB_DEVICE_DATA) },
        };

        private void Call(string key, string[] args)
        {
            int p = key.IndexOf('(');
            string name = key.Substring(0, p);
            // "A.Name(...)" = ModularAssistDevice 메서드, 나머지는 장치 계층 Afx* 함수
            bool assist = name.StartsWith("A.", StringComparison.Ordinal);
            if (assist) name = name.Substring(2);
            object target = assist ? (object)_assist : _dev;
            string[] ctypes = key.Substring(p + 1, key.Length - p - 2).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            MethodInfo method = target.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m.Name == name && m.GetParameters().Length == ctypes.Length)
                .FirstOrDefault(m => m.GetParameters().Select((pi, i) => CTypes.TryGetValue(ctypes[i], out Type[] ok) && ok.Contains(pi.ParameterType)).All(x => x));
            if (method == null)
            {
                _out.WriteLine("NOFUNC");
                return;
            }

            ParameterInfo[] ps = method.GetParameters();
            var values = new object[ps.Length];
            for (int i = 0; i < ps.Length; i++)
            {
                string a = i < args.Length ? args[i] : null;
                Type t = ps[i].ParameterType;
                if (t == typeof(double)) values[i] = a == null ? 0.0 : CStrtod(a);
                else if (t == typeof(string)) values[i] = a ?? "";
                else if (t == typeof(REQUEST_USB_DEVICE_DATA)) values[i] = ParseRequest(a ?? "");
                else if (typeof(ICharArray).IsAssignableFrom(t)) values[i] = new CharBuffer((a ?? "").Length + 256, a ?? "");
                else
                {
                    long v = a == null ? 0 : ParseCInt(a);
                    values[i] = t == typeof(int) ? (object)unchecked((int)v) : t == typeof(uint) ? unchecked((uint)v)
                        : t == typeof(ushort) ? unchecked((ushort)v) : (object)unchecked((byte)v);
                }
            }

            object ret;
            try
            {
                ret = method.Invoke(target, values);
            }
            catch (TargetInvocationException ex)
            {
                _out.WriteLine("EXCEPTION " + ex.InnerException.GetType().Name + ": " + ex.InnerException.Message);
                foreach (string fr in (ex.InnerException.StackTrace ?? "").Split('\n').Where(x => x.Contains("SemDevice") || x.Contains("AssistDevice")).Take(3)) _out.WriteLine("  at " + fr.Trim());
                DiffState();
                return;
            }
            if (method.ReturnType == typeof(double)) _out.WriteLine("RET " + CRuntime.Format("%.17g", (double)ret));
            else if (method.ReturnType != typeof(void)) _out.WriteLine("RET " + Convert.ToInt64(ret, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture));
            DiffState();
        }

        /// <summary>C strtoll(s, NULL, 0): 0x → 16진, 앞자리 0 → 8진</summary>
        private static long ParseCInt(string s)
        {
            s = s.Trim();
            bool neg = s.StartsWith("-");
            if (neg || s.StartsWith("+")) s = s.Substring(1);
            long v;
            if (s.StartsWith("0x", StringComparison.OrdinalIgnoreCase)) v = long.Parse(s.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            else if (s.Length > 1 && s[0] == '0') v = Convert.ToInt64(s, 8);
            else v = long.Parse(s, CultureInfo.InvariantCulture);
            return neg ? -v : v;
        }

        private static double CStrtod(string s) => CRuntime.atof(s);

        /// <summary>harness.cpp Args::Req와 같은 형식 "id,type,option;n0,n1,..;d0,d1,..;text" (빠진 칸은 0)</summary>
        private static REQUEST_USB_DEVICE_DATA ParseRequest(string s)
        {
            var r = new REQUEST_USB_DEVICE_DATA();
            string[] part = s.Split(';');
            string[] Items(string x) => x.Length == 0 ? new string[0] : x.Split(',');
            string[] h = Items(part[0]);
            if (h.Length > 0) r.nRequestID = unchecked((int)ParseCInt(h[0]));
            if (h.Length > 1) r.nRequestType = unchecked((int)ParseCInt(h[1]));
            if (h.Length > 2) r.nRequestOption = unchecked((int)ParseCInt(h[2]));
            if (part.Length > 1) { string[] n = Items(part[1]); for (int i = 0; i < n.Length && i < r.nRequestData.Length; i++) r.nRequestData[i] = unchecked((int)ParseCInt(n[i])); }
            if (part.Length > 2) { string[] d = Items(part[2]); for (int i = 0; i < d.Length && i < r.dRequestData.Length; i++) r.dRequestData[i] = CStrtod(d[i]); }
            if (part.Length > 3) { string t = part[3].Length >= r.szData.Capacity ? part[3].Substring(0, r.szData.Capacity - 1) : part[3]; CRuntime.strcpy(r.szData, t); }
            return r;
        }

        /// <summary>harness.cpp PrintRequest와 같은 형식</summary>
        private static string FormatRequest(REQUEST_USB_DEVICE_DATA r)
        {
            int ln = r.nRequestData.Length, ld = r.dRequestData.Length;
            while (ln > 0 && r.nRequestData[ln - 1] == 0) ln--;
            while (ld > 0 && r.dRequestData[ld - 1] == 0.0) ld--;
            return string.Format(CultureInfo.InvariantCulture, " REQ {0} {1} {2} n={3} d={4} sz={5}", r.nRequestID, r.nRequestType, r.nRequestOption,
                string.Join(",", r.nRequestData.Take(ln)), string.Join(",", r.dRequestData.Take(ld).Select(x => CRuntime.Format("%.17g", x))), r.szData);
        }

        /// <summary>ModularAssistDevice가 m_pOwnerWindow로 부르는 메인 화면 함수 (6b에서 포팅) — harness.cpp CNanoImpact2020cfeDlg와 같은 기록</summary>
        private sealed class RecordingOwner : IAssistOwner
        {
            private readonly TextWriter _out;
            public RecordingOwner(TextWriter output) { _out = output; }

            public int OnSetUsbDeviceSEMControlDataToDeviceServer(int wParam, object lParam)
            {
                _out.WriteLine("OWNER OnSetUsbDeviceSEMControlDataToDeviceServer " + unchecked((uint)wParam) + FormatRequest((REQUEST_USB_DEVICE_DATA)lParam));
                return 1;
            }

            public int OnSetUsbDeviceSEMControlDataToFlowControl(int wParam, object lParam)
            {
                _out.WriteLine("OWNER OnSetUsbDeviceSEMControlDataToFlowControl " + unchecked((uint)wParam) + FormatRequest((REQUEST_USB_DEVICE_DATA)lParam));
                return 1;
            }

            public int OnSemControlIntegratedData(int wParam, object lParam) { _out.WriteLine("OWNER OnSemControlIntegratedData " + unchecked((uint)wParam) + " " + (lParam != null ? "P" : "0")); return 1; }
            public int OnSemStageUsbControlData(int wParam, object lParam) { _out.WriteLine("OWNER OnSemStageUsbControlData " + unchecked((uint)wParam) + " " + (lParam != null ? "P" : "0")); return 1; }
            public void SetWDSetupEnabledOLValue(int nCoarse) => _out.WriteLine("OWNER SetWDSetupEnabledOLValue " + nCoarse);
        }

        // ---- 상태 변화 (harness.cpp Diff와 같은 규칙: 4바이트 이내 간격은 한 줄로) ----

        private void DiffState()
        {
            Diff('S', _dev.IntegratedForTest?.ToBytes(), ref _prevIntegrated);
            Diff('R', _dev.ScanRotCaliForTest?.ToBytes(), ref _prevSRot);
            DiffAssist();
        }

        /// <summary>harness.cpp DiffAssist와 같은 순서·이름</summary>
        private void DiffAssist()
        {
            var cur = new List<string>
            {
                "g_assist.m_bSbrConnected=" + _assist.m_bSbrConnected,
                "g_assist.m_bSemConnected=" + _assist.m_bSemConnected,
                "g_assist.m_bBseConnected=" + _assist.m_bBseConnected,
                "g_assist.m_bUsbConnected=" + _assist.m_bUsbConnected,
                "g_assist.m_bSysInitilized=" + _assist.m_bSysInitilized,
                "g_assist.m_nSeBseScanMode=" + _assist.m_nSeBseScanMode,
                "g_vbaf.m_nChamberState=" + _vbaf.m_nChamberState,
                "g_vbaf.m_nGunValveState=" + _vbaf.m_nGunValveState,
            };
            if (_prevAssist.Count != cur.Count) _prevAssist = cur.Select(_ => "").ToList();
            for (int i = 0; i < cur.Count; i++)
                if (cur[i] != _prevAssist[i]) _out.WriteLine("A " + cur[i]);
            _prevAssist = cur;
        }

        private void Diff(char tag, byte[] b, ref byte[] prev)
        {
            if (b == null) return;
            if (prev == null || prev.Length != b.Length) prev = new byte[b.Length];
            int n = b.Length;
            int i = 0;
            var sb = new StringBuilder();
            while (i < n)
            {
                if (b[i] == prev[i]) { i++; continue; }
                int j = i + 1, last = i;
                while (j < n && j - last <= 4) { if (b[j] != prev[j]) last = j; j++; }
                sb.Clear().Append(tag).Append(' ').Append(i);
                for (int k = i; k <= last; k++) sb.Append(' ').Append(b[k].ToString("X2"));
                _out.WriteLine(sb.ToString());
                i = last + 1;
            }
            Buffer.BlockCopy(b, 0, prev, 0, n);
        }

        // ---- 가짜 FTDI (fake_ftd2xx.cpp와 같은 기록) ----

        /// <summary>가짜 FTDI. HostCheck(실제 스레드)도 쓴다: 받는 큐는 잠그고, RX가 들어오면 FT_SetEventNotification으로 받은 이벤트를 켠다.</summary>
        internal sealed class FakeFtdi : IFtdi
        {
            private readonly TextWriter _out;
            private readonly Queue<byte>[] _rx = { new Queue<byte>(), new Queue<byte>(), new Queue<byte>() };
            private readonly EventWaitHandle[] _events = new EventWaitHandle[3];

            public FakeFtdi(TextWriter output) { _out = output; }

            public void PushRx(int idx, byte[] b)
            {
                if (idx < 0 || idx > 2) return;
                lock (_rx[idx]) foreach (byte x in b) _rx[idx].Enqueue(x);
                _events[idx]?.Set();
            }

            private static int IndexOf(IntPtr h) => (int)h - 1;

            private uint Ctl(IntPtr h, string what, uint a = 0, uint b = 0, uint c = 0)
            {
                _out.WriteLine(string.Format(CultureInfo.InvariantCulture, "C {0} {1} {2:X} {3:X} {4:X}", IndexOf(h), what, a, b, c));
                return 0;
            }

            public bool Load() => true;
            public uint FT_CreateDeviceInfoList(out uint numDevs) { numDevs = 3; return 0; }
            public uint FT_GetDeviceInfoList(uint numDevs) => 0;

            public uint FT_OpenEx(string serial, uint flags, out IntPtr handle)
            {
                int idx = serial == "ImgCap001" ? 0 : serial == "BseICap001" ? 1 : serial == "ScanBri001" ? 2 : -1;
                _out.WriteLine(string.Format(CultureInfo.InvariantCulture, "C {0} FT_OpenEx {1} {2:X}", idx, serial, flags));
                handle = idx < 0 ? IntPtr.Zero : new IntPtr(idx + 1);
                return idx < 0 ? 2u : 0u; // FT_DEVICE_NOT_FOUND
            }

            public uint FT_Close(IntPtr h) => Ctl(h, "FT_Close");

            public uint FT_Read(IntPtr h, byte[] buffer, uint toRead, out uint read)
            {
                int idx = IndexOf(h);
                uint n = 0;
                if (idx >= 0 && idx < 3) lock (_rx[idx]) while (n < toRead && _rx[idx].Count > 0) buffer[n++] = _rx[idx].Dequeue();
                read = n;
                return 0;
            }

            public uint FT_Write(IntPtr h, byte[] buffer, uint toWrite, out uint written)
            {
                var sb = new StringBuilder("W ").Append(IndexOf(h));
                for (int i = 0; i < toWrite; i++) sb.Append(' ').Append(buffer[i].ToString("X2"));
                _out.WriteLine(sb.ToString());
                written = toWrite;
                return 0;
            }

            public uint FT_ResetDevice(IntPtr h) => Ctl(h, "FT_ResetDevice");
            public uint FT_Purge(IntPtr h, uint mask) => Ctl(h, "FT_Purge", mask);
            public uint FT_SetTimeouts(IntPtr h, uint r, uint w) => Ctl(h, "FT_SetTimeouts", r, w);

            public uint FT_GetStatus(IntPtr h, out uint rx, out uint tx, out uint ev)
            {
                int idx = IndexOf(h);
                rx = idx >= 0 && idx < 3 ? (uint)Count(idx) : 0;
                tx = 0;
                ev = 0;
                return 0;
            }

            public uint FT_GetQueueStatus(IntPtr h, out uint rx)
            {
                int idx = IndexOf(h);
                rx = idx >= 0 && idx < 3 ? (uint)Count(idx) : 0;
                return 0;
            }

            public uint FT_SetBitMode(IntPtr h, byte mask, byte mode) => Ctl(h, "FT_SetBitMode", mask, mode);
            public uint FT_SetLatencyTimer(IntPtr h, byte ms) => Ctl(h, "FT_SetLatencyTimer", ms);
            public uint FT_SetUSBParameters(IntPtr h, uint i, uint o) => Ctl(h, "FT_SetUSBParameters", i, o);
            public uint FT_SetFlowControl(IntPtr h, ushort flow, byte xon, byte xoff) => Ctl(h, "FT_SetFlowControl", flow, xon, xoff);
            private int Count(int idx) { lock (_rx[idx]) return _rx[idx].Count; }

            public uint FT_SetEventNotification(IntPtr h, uint mask, EventWaitHandle evt)
            {
                int idx = IndexOf(h);
                if (idx >= 0 && idx < 3) _events[idx] = evt;
                return Ctl(h, "FT_SetEventNotification", mask);
            }
        }

        // ---- 기록하는 호스트 (harness.cpp의 Harness* 함수와 같은 기록) ----

        private sealed class RecordingHost : IDeviceHost
        {
            private readonly TextWriter _out;

            public RecordingHost(TextWriter output) { _out = output; }

            public uint Tick { get; set; }
            public int BoxReturn { get; set; } = 1; // IDOK

            public int MessageBox(int hWnd, string text, string caption, uint type)
            {
                _out.WriteLine("BOX " + caption + " | " + text);
                return BoxReturn;
            }

            private void Print(string tag, int hWnd, int msg, int wParam, object lParam)
            {
                string l;
                if (lParam == null) l = "0";
                else if (lParam is int v) l = unchecked((uint)v) < 0x10000 ? v.ToString(CultureInfo.InvariantCulture) : "P";
                else if (lParam is byte[] buf && IsImageDataMessage(msg)) l = "P " + Fnv(buf, wParam).ToString("X16", CultureInfo.InvariantCulture);
                else l = "P";
                _out.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0} {1} {2} {3} {4}", tag, hWnd, unchecked((uint)msg), unchecked((uint)wParam), l));
            }

            private static bool IsImageDataMessage(int msg) =>
                msg == NanoImpact.Core.Def.UM_SEM_SE_SCAN_IMAGE_DATA || msg == NanoImpact.Core.Def.UM_SEM_SE_SCAN_IMAGE_DATA_AUTO_FOCUS ||
                msg == NanoImpact.Core.Def.UM_SEM_SE_SCAN_IMAGE_DATA_AUTO_CONTRAST || msg == NanoImpact.Core.Def.UM_SEM_SE_SCAN_IMAGE_DATA_AUTO_BRIGHTNESS ||
                msg == NanoImpact.Core.Def.UM_SEM_SE_SCAN_IMAGE_DATA_AUTO_CONT_BRIG || msg == NanoImpact.Core.Def.UM_SEM_SE_SCAN_IMAGE_DATA_AUTO_GUN_ALIGN;

            private static ulong Fnv(byte[] b, int n)
            {
                ulong h = 1469598103934665603UL;
                for (int i = 0; i < n && i < b.Length; i++) { h ^= b[i]; h *= 1099511628211UL; }
                return h;
            }

            public int SendMessage(int hWnd, int msg, int wParam, object lParam) { Print("MSG", hWnd, msg, wParam, lParam); return 0; }
            public bool PostMessage(int hWnd, int msg, int wParam, object lParam) { Print("POST", hWnd, msg, wParam, lParam); return true; }
            public void Sleep(int milliseconds) => _out.WriteLine("SLEEP " + unchecked((uint)milliseconds));
            public uint GetTickCount() => Tick;

            public object BeginThread(Func<object, uint> proc, object param)
            {
                _out.WriteLine("THREAD");
                return new object();
            }
        }
    }
}
