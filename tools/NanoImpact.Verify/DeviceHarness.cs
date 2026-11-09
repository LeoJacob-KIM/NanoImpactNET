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
        private byte[] _prevIntegrated;
        private byte[] _prevSRot;

        public DeviceHarness(TextWriter output)
        {
            _out = output;
            _ftdi = new FakeFtdi(output);
            _host = new RecordingHost(output);
            _dev = new SemDevice(_host, _ftdi);
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
        };

        private void Call(string key, string[] args)
        {
            int p = key.IndexOf('(');
            string name = key.Substring(0, p);
            string[] ctypes = key.Substring(p + 1, key.Length - p - 2).Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            MethodInfo method = typeof(SemDevice).GetMethods(BindingFlags.Public | BindingFlags.Instance)
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
                ret = method.Invoke(_dev, values);
            }
            catch (TargetInvocationException ex)
            {
                _out.WriteLine("EXCEPTION " + ex.InnerException.GetType().Name + ": " + ex.InnerException.Message);
                foreach (string fr in (ex.InnerException.StackTrace ?? "").Split('\n').Where(x => x.Contains("SemDevice")).Take(3)) _out.WriteLine("  at " + fr.Trim());
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

        // ---- 상태 변화 (harness.cpp Diff와 같은 규칙: 4바이트 이내 간격은 한 줄로) ----

        private void DiffState()
        {
            Diff('S', _dev.IntegratedForTest?.ToBytes(), ref _prevIntegrated);
            Diff('R', _dev.ScanRotCaliForTest?.ToBytes(), ref _prevSRot);
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

        private sealed class FakeFtdi : IFtdi
        {
            private readonly TextWriter _out;
            private readonly Queue<byte>[] _rx = { new Queue<byte>(), new Queue<byte>(), new Queue<byte>() };

            public FakeFtdi(TextWriter output) { _out = output; }

            public void PushRx(int idx, byte[] b)
            {
                if (idx < 0 || idx > 2) return;
                foreach (byte x in b) _rx[idx].Enqueue(x);
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
                while (n < toRead && idx >= 0 && idx < 3 && _rx[idx].Count > 0) buffer[n++] = _rx[idx].Dequeue();
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
                rx = idx >= 0 && idx < 3 ? (uint)_rx[idx].Count : 0;
                tx = 0;
                ev = 0;
                return 0;
            }

            public uint FT_GetQueueStatus(IntPtr h, out uint rx)
            {
                int idx = IndexOf(h);
                rx = idx >= 0 && idx < 3 ? (uint)_rx[idx].Count : 0;
                return 0;
            }

            public uint FT_SetBitMode(IntPtr h, byte mask, byte mode) => Ctl(h, "FT_SetBitMode", mask, mode);
            public uint FT_SetLatencyTimer(IntPtr h, byte ms) => Ctl(h, "FT_SetLatencyTimer", ms);
            public uint FT_SetUSBParameters(IntPtr h, uint i, uint o) => Ctl(h, "FT_SetUSBParameters", i, o);
            public uint FT_SetFlowControl(IntPtr h, ushort flow, byte xon, byte xoff) => Ctl(h, "FT_SetFlowControl", flow, xon, xoff);
            public uint FT_SetEventNotification(IntPtr h, uint mask, EventWaitHandle evt) => Ctl(h, "FT_SetEventNotification", mask);
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
                else l = "P";
                _out.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0} {1} {2} {3} {4}", tag, hWnd, unchecked((uint)msg), unchecked((uint)wParam), l));
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
