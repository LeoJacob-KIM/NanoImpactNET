using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NanoImpact.Core;
using NanoImpact.Hardware.Device;
using NanoImpact.Hardware.Spellman;

namespace NanoImpact.Verify
{
    /// <summary>
    /// tools\hv_harness\harness.cpp(원본 Spellman RS232 코드 실행)와 같은 스크립트·같은 출력으로 C# CfeHVPowerMonitor/CfeHVPowerControl/UsbRS232Comm을 돌린다.
    ///   NanoImpact.Verify hv &lt;script&gt;
    /// 직렬 포트는 C++ 하네스의 가짜 함수와 같은 동작(스크립트 RX 조각을 차례로 내주고, 다 쓰면 WaitCommEvent 실패 = 포트 닫힘).
    /// </summary>
    internal sealed class HvHarness : ICfeHVPowerView, IDeviceHost, ISerialCommApi
    {
        private const uint ERROR_OPERATION_ABORTED = 995;
        private const int HWND_MAIN = 1;

        private readonly TextWriter _out;
        private readonly CfeHVPowerMonitor _m;
        private List<string> _prev = new List<string>();
        private int _check;
        private Func<object, uint> _threadProc;
        private object _threadParam;

        // 가짜 직렬 포트 상태
        private bool _failOpen;
        private uint _mask;
        private uint _lastError;
        private readonly Queue<byte[]> _chunks = new Queue<byte[]>();
        private readonly Queue<byte> _inQue = new Queue<byte>();

        private HvHarness(TextWriter output)
        {
            _out = output;
            _m = new CfeHVPowerMonitor(this, this, this, HWND_MAIN);
        }

        public static int Run(string script)
        {
            var stdout = new StreamWriter(Console.OpenStandardOutput(), new UTF8Encoding(false)) { AutoFlush = false, NewLine = "\n" };
            var h = new HvHarness(stdout);
            h.PrintState();
            foreach (string raw in File.ReadLines(script, Encoding.UTF8))
            {
                string line = raw.TrimEnd('\r', '\n').TrimStart('﻿');
                if (line.Length == 0 || line[0] == '#') continue;
                string[] t = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Length == 0) continue;
                if (t[0] == "RX")
                {
                    stdout.WriteLine("> RX " + line.Substring(3));
                    h._chunks.Enqueue(Unescape(line.Substring(3)));
                    continue;
                }
                stdout.WriteLine("> " + line);
                h.Execute(t);
                h.PrintState();
            }
            stdout.Flush();
            return 0;
        }

        private void Execute(string[] t)
        {
            CfeHVPowerControl b = _m.m_wndBeamControlDlg;
            switch (t[0])
            {
                case "OPEN":
                    _m.CfeHVPowerUsbRS232ComPortOpen(int.Parse(t[1], CultureInfo.InvariantCulture));
                    break;
                case "FAILOPEN":
                    _failOpen = true;
                    break;
                case "CLOSE":
                    _m.CfeHVPowerUsbRS232ComPortClose();
                    break;
                case "INIT":
                    b.SetDialogStart();
                    break;
                case "START":
                    b.SetCfeHVPowerMonitoringStart();
                    break;
                case "MODE":
                    b.SetCfeHVPowerMonitoringTimerExecute(int.Parse(t[1], CultureInfo.InvariantCulture));
                    break;
                case "TIMER":
                    for (int i = 0, n = int.Parse(t[2], CultureInfo.InvariantCulture); i < n; i++) b.OnTimer(int.Parse(t[1], CultureInfo.InvariantCulture));
                    break;
                case "SEND":
                    _out.WriteLine("RET " + Send(b, t[1], t.Skip(2).ToArray()));
                    break;
                case "FW":
                    if (int.Parse(t[1], CultureInfo.InvariantCulture) == 0) _m.GetCfeHVPowerGroundFirmwareVersion();
                    else _m.GetCfeHVPowerHVFirmwareVersion();
                    break;
                case "CHECK":
                    _check = int.Parse(t[1], CultureInfo.InvariantCulture);
                    break;
                case "RUN":
                    if (_threadProc != null)
                    {
                        Func<object, uint> p = _threadProc;
                        _threadProc = null;
                        p(_threadParam);
                    }
                    else _out.WriteLine("NOTHREAD");
                    _chunks.Clear();
                    break;
                default:
                    _out.WriteLine("UNKNOWN " + t[0]);
                    break;
            }
        }

        /// <summary>SendingCfeHVPower&lt;code&gt;Command를 이름으로 찾아 인자 형식(int / string)대로 부른다</summary>
        private int Send(CfeHVPowerControl b, string code, string[] a)
        {
            MethodInfo mi = typeof(CfeHVPowerControl).GetMethod("SendingCfeHVPower" + code + "Command");
            if (mi == null)
            {
                _out.WriteLine("NOCMD " + code);
                return 0;
            }
            ParameterInfo[] ps = mi.GetParameters();
            var args = new object[ps.Length];
            for (int i = 0; i < ps.Length; i++)
            {
                string s = i < a.Length ? a[i] : "";
                args[i] = ps[i].ParameterType == typeof(int) ? (object)CRuntimeAtoi(s) : s;
            }
            return (int)mi.Invoke(b, args);
        }

        private static int CRuntimeAtoi(string s) => NanoImpact.Core.Native.CRuntime.atoi(s);

        private static byte[] Unescape(string s)
        {
            var r = new List<byte>();
            byte[] raw = Encoding.UTF8.GetBytes(s);
            for (int i = 0; i < raw.Length; i++)
            {
                if (raw[i] != (byte)'\\' || i + 1 >= raw.Length) { r.Add(raw[i]); continue; }
                char c = (char)raw[++i];
                if (c == 'n') r.Add(0x0A);
                else if (c == 'r') r.Add(0x0D);
                else if (c == 't') r.Add(0x09);
                else if (c == 'x' && i + 2 < raw.Length + 1)
                {
                    string hex = Encoding.ASCII.GetString(raw, i + 1, Math.Min(2, raw.Length - i - 1));
                    r.Add((byte)Convert.ToUInt32(hex, 16));
                    i += 2;
                }
                else r.Add((byte)c);
            }
            return r.ToArray();
        }

        // ---- 상태 출력 (바뀐 것만) ----

        private void PrintState()
        {
            CfeHVPowerControl b = _m.m_wndBeamControlDlg;
            var cur = new List<string>
            {
                "port=" + (_m.m_pUsbRS232Comm != null ? 1 : 0),
                "m_bUsbRS232ComPortOpen=" + _m.m_bUsbRS232ComPortOpen,
                "m_nReceiveDataListItemIndex=" + _m.m_nReceiveDataListItemIndex,
                "m_nCfeHVPowerMainOutputStatus=" + _m.m_nCfeHVPowerMainOutputStatus,
                "m_nCfeHVPowerErrorOutputStatus=" + _m.m_nCfeHVPowerErrorOutputStatus,
                "m_szCfeHVPowerDataBuffer=" + EscapeBytes(_m.m_szCfeHVPowerDataBuffer.Bytes),
                "m_strExtratorCurrent=" + _m.m_strExtratorCurrent,
                "m_nBeamRampingState=" + _m.m_wndCfeHVPowerMonitorDlg.m_nBeamRampingState,
                "m_nBeamOnTargetFlag=" + _m.m_wndCfeHVPowerMonitorDlg.m_nBeamOnTargetFlag,
                "m_bWindowInitStartFlag=" + b.m_bWindowInitStartFlag,
                "m_nCfeHVPowerMonitoringMode=" + b.m_nCfeHVPowerMonitoringMode,
                "m_nCfeHVPowerAllMonitoringIndex=" + b.m_nCfeHVPowerAllMonitoringIndex,
                "m_nCfeHVPowerHVMonitoringIndex=" + b.m_nCfeHVPowerHVMonitoringIndex,
                "m_nCfeHVPowerOneTimeMonitoring=" + b.m_nCfeHVPowerOneTimeMonitoring,
                "m_bSetBeamTargetValueFlag=" + b.m_bSetBeamTargetValueFlag,
                "m_bSetHeaterTargetValueFlag=" + b.m_bSetHeaterTargetValueFlag,
                "m_bSetSuppressorTargetValueFlag=" + b.m_bSetSuppressorTargetValueFlag,
                "m_bSetExtractorTargetValueFlag=" + b.m_bSetExtractorTargetValueFlag,
                "m_bSetExtractorTripTargetValueFlag=" + b.m_bSetExtractorTripTargetValueFlag,
            };
            if (_prev.Count != cur.Count) _prev = cur.Select(_ => "").ToList();
            for (int i = 0; i < cur.Count; i++)
                if (cur[i] != _prev[i]) _out.WriteLine("F " + cur[i]);
            _prev = cur;
        }

        /// <summary>C++ Escape: NUL 전까지, 역슬래시는 두 개로, 출력 못 하는 바이트는 \xHH</summary>
        private static string EscapeBytes(byte[] p)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < p.Length && p[i] != 0; i++)
            {
                byte c = p[i];
                if (c == (byte)'\\') sb.Append("\\\\");
                else if (c >= 0x20 && c < 0x7F) sb.Append((char)c);
                else sb.Append("\\x").Append(c.ToString("X2", CultureInfo.InvariantCulture));
            }
            return sb.ToString();
        }

        /// <summary>C++ Text: 화면 글자의 제어 문자를 \xHH로</summary>
        private static string Text(string s)
        {
            var sb = new StringBuilder();
            foreach (char c in s ?? "")
            {
                if (c < 0x20 || c == 0x7F) sb.Append("\\x").Append(((int)c).ToString("X2", CultureInfo.InvariantCulture));
                else sb.Append(c);
            }
            return sb.ToString();
        }

        // ---- ICfeHVPowerView ----

        public void SetText(string target, string text) => _out.WriteLine("UI " + target + " " + Text(text));
        public void SetImage(string target, int index, int onOff) => _out.WriteLine("IMG " + target + " " + index + " " + onOff);
        public void SetSpinPos(string control, int pos) => _out.WriteLine("SPIN " + control + " " + pos);
        public void ListInsertItem(int item, string text) => _out.WriteLine("LIST INSERT " + item + " " + Text(text));
        public void ListSetItemText(int item, int subItem, string text) => _out.WriteLine("LIST SET " + item + " " + subItem + " " + Text(text));
        public int GetReceiveDataViewCheck() => _check;
        public void SetTimer(int id, int elapse) => _out.WriteLine("TIMER SET " + id + " " + elapse);
        public void KillTimer(int id) => _out.WriteLine("TIMER KILL " + id);
        public void ExecuteFocusDegaussing(int milliseconds) => _out.WriteLine("DEGAUSS " + milliseconds);

        // ---- IDeviceHost ----

        public int MessageBox(int hWnd, string text, string caption, uint type)
        {
            _out.WriteLine("BOX " + caption + " | " + text + " | " + type);
            return 1; // IDOK
        }

        public int SendMessage(int hWnd, int msg, int wParam, object lParam)
        {
            if (msg == Def.UM_CFE_HV_POWER_CONTROL_RECEIVE_DATA) _out.WriteLine("MSG {0} {1} {2} P", hWnd, msg, (uint)wParam);
            else _out.WriteLine("MSG {0} {1} {2} {3}", hWnd, msg, (uint)wParam, lParam is int n ? n : 0);
            if (msg == Def.UM_CFE_HV_POWER_CONTROL_RECEIVE_DATA) return _m.OnCfeHVPowerUsbRS232ComPortReceiveData(wParam, lParam);
            if (msg == Def.UM_CFE_HV_POWER_CONTROL_PORT_CLOSE) return _m.OnCfeHVPowerUsbRS232ComPortClose(wParam, lParam);
            return 0;
        }

        public bool PostMessage(int hWnd, int msg, int wParam, object lParam)
        {
            _out.WriteLine("POST " + msg + " " + (uint)wParam);
            return true;
        }

        public void Sleep(int milliseconds) => _out.WriteLine("SLEEP " + milliseconds);
        public uint GetTickCount() => 0;

        public object BeginThread(Func<object, uint> proc, object param)
        {
            _out.WriteLine("THREAD");
            _threadProc = proc;
            _threadParam = param;
            return new object();
        }

        // ---- ISerialCommApi (C++ 하네스 Hv* 가짜 함수와 같은 동작·기록) ----

        public object CreateFile(string portName)
        {
            _out.WriteLine("C CreateFile " + portName);
            if (_failOpen)
            {
                _failOpen = false;
                return null;
            }
            _mask = 0;
            _inQue.Clear();
            return "comm";
        }

        public void SetupComm(object h, int inQueue, int outQueue) => _out.WriteLine("C SetupComm " + inQueue + " " + outQueue);
        public void PurgeComm(object h, uint flags) => _out.WriteLine("C PurgeComm " + flags);

        public void SetCommTimeouts(object h, COMMTIMEOUTS t) =>
            _out.WriteLine("C SetCommTimeouts {0} {1} {2} {3} {4}", t.ReadIntervalTimeout, t.ReadTotalTimeoutMultiplier, t.ReadTotalTimeoutConstant,
                t.WriteTotalTimeoutMultiplier, t.WriteTotalTimeoutConstant);

        public void GetCommState(object h, DCB dcb)
        {
        }

        public void SetCommState(object h, DCB d) =>
            _out.WriteLine("C SetCommState BaudRate={0} fBinary={1} fParity={2} Parity={3} ByteSize={4} StopBits={5} fRtsControl={6} fDtrControl={7} " +
                "fOutxDsrFlow={8} fOutX={9} fInX={10} XonLim={11} XoffLim={12} fOutxCtsFlow={13}",
                d.BaudRate, d.fBinary, d.fParity, d.Parity, d.ByteSize, d.StopBits, d.fRtsControl, d.fDtrControl,
                d.fOutxDsrFlow, d.fOutX, d.fInX, d.XonLim, d.XoffLim, d.fOutxCtsFlow);

        public void SetCommMask(object h, uint mask)
        {
            _out.WriteLine("C SetCommMask " + mask);
            _mask = mask;
        }

        public bool WaitCommEvent(object h, ref uint evtMask)
        {
            if (_mask != 0 && _inQue.Count == 0 && _chunks.Count > 0)
            {
                foreach (byte x in _chunks.Dequeue()) _inQue.Enqueue(x);
            }
            if (_mask == 0 || _inQue.Count == 0)
            {
                evtMask = 0;
                _lastError = ERROR_OPERATION_ABORTED;
                return false;
            }
            evtMask = WinComm.EV_RXCHAR;
            return true;
        }

        public uint GetLastError() => _lastError;

        public void ClearCommError(object h, ref uint errors, COMSTAT stat)
        {
            errors = 0;
            if (stat != null)
            {
                stat.cbInQue = (uint)_inQue.Count;
                stat.cbOutQue = 0;
            }
        }

        public uint ReadFile(object h, byte[] buffer, int offset, int count)
        {
            uint k = 0;
            for (; k < count && _inQue.Count > 0; k++) buffer[offset + k] = _inQue.Dequeue();
            return k;
        }

        public bool WriteFile(object h, byte[] buffer, int count)
        {
            var sb = new StringBuilder("W");
            for (int i = 0; i < count; i++) sb.Append(' ').Append(buffer[i].ToString("X2", CultureInfo.InvariantCulture));
            _out.WriteLine(sb.ToString());
            return true;
        }

        public void EscapeCommFunction(object h, uint func) => _out.WriteLine("C EscapeCommFunction " + func);
        public void CloseHandle(object h) => _out.WriteLine("C CloseHandle " + (h as string ?? "?"));
        public object CreateEvent() => "event";
    }
}
