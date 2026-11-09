using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using NanoImpact.Core;
using NanoImpact.Core.Native;
using NanoImpact.Hardware.Imaging;

namespace NanoImpact.Verify
{
    /// <summary>
    /// tools\image_harness\harness.cpp(원본 영상 조립 코드 실행)와 같은 스크립트·같은 출력으로 C# SemScanImageProcessor를 돌린다.
    ///   NanoImpact.Verify image &lt;script&gt; &lt;scan_fields.inc&gt;
    /// </summary>
    internal sealed class ImageHarness : IScanImageView
    {
        private readonly TextWriter _out;
        private readonly SemScanImageProcessor _p;
        private readonly IntegratedCommandStruct _integrated = new IntegratedCommandStruct();
        private readonly List<KeyValuePair<string, char>> _fields = new List<KeyValuePair<string, char>>();
        private readonly Dictionary<string, int> _arrayCounts = new Dictionary<string, int>();
        private List<string> _prev = new List<string>();
        private uint _tick;
        private uint _rng;

        private static readonly string[] MemberNames =
        {
            "m_bSemSEFrameFirstCatch:I", "m_nSemSEScanImageSaveStart:I", "m_nSemSEScanImageSaveCount:I", "m_nReceiveDataCount:I",
            "m_nReceiveTotalImageCount:I", "m_dwDisplayFrameCount:U", "m_dwDisplayUpdateCount:U", "m_dwFirstTick:U", "m_dwFrameDataCount:U",
            "m_dwFrameUpdateCount:U", "m_dwSecondTick:U", "m_bPhotoCalibrationFlag:I", "m_nAutoProcessRunStatus:I", "m_nHistogramAnalysisBright:I",
            "m_nHistogramAnalysisContrasrt:I", "m_nHistogramAnalysisGamma:I",
            "m_nLayerPhotoMode:I", "m_nNormalFullImageSizeFlag:I", "m_n2ndFullSizeImageScan:I", "m_nSemSEScanImageReadStart:I", "m_nSemSEScanImageReadSize:I",
        };

        private readonly RecScanDevice _scanDev;
        private int _curCL1, _curCL2;

        private ImageHarness(TextWriter output, string fieldsInc)
        {
            _out = output;
            _scanDev = new RecScanDevice(output);
            _p = new SemScanImageProcessor(this, () => _integrated, _scanDev);
            foreach (string line in File.ReadLines(fieldsInc))
            {
                Match m = Regex.Match(line, @"^FIELD(A?)\((\w), (\w+)(?:, (\w+))?\)");
                if (!m.Success) continue;
                _fields.Add(new KeyValuePair<string, char>(m.Groups[3].Value, m.Groups[2].Value[0]));
                if (m.Groups[1].Value == "A") _arrayCounts[m.Groups[3].Value] = int.Parse(m.Groups[4].Value, CultureInfo.InvariantCulture);
            }
        }

        public static int Run(string script, string fieldsInc)
        {
            var stdout = new StreamWriter(Console.OpenStandardOutput(), new UTF8Encoding(false)) { AutoFlush = false, NewLine = "\n" };
            var h = new ImageHarness(stdout, fieldsInc);
            h.PrintState();
            foreach (string raw in File.ReadLines(script))
            {
                string line = raw.TrimEnd('\r', '\n').TrimStart('﻿');
                if (line.Length == 0 || line[0] == '#') continue;
                h.Execute(line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries));
                stdout.Flush();
            }
            stdout.Flush();
            return 0;
        }

        private void Execute(string[] tok)
        {
            _out.WriteLine("> " + tok[0]);
            switch (tok[0])
            {
                case "INI" when tok.Length >= 2:
                    if (File.Exists(tok[1])) _integrated.LoadFile(tok[1]);
                    break;
                case "SET" when tok.Length >= 3:
                    if (!SetField(tok[1], tok[2])) _out.WriteLine("NOFIELD " + tok[1]);
                    PrintState();
                    break;
                case "TICK" when tok.Length >= 2:
                    _tick = uint.Parse(tok[1], CultureInfo.InvariantCulture);
                    break;
                case "PHOTO" when tok.Length >= 3:
                    _p.PreparePhotoBitmap(int.Parse(tok[1], CultureInfo.InvariantCulture), int.Parse(tok[2], CultureInfo.InvariantCulture), Def.DISPLAY_2ND_IMAGE_BACKGROUND_COLOR);
                    break;
                case "FRAME" when tok.Length >= 5:
                {
                    int w = int.Parse(tok[1], CultureInfo.InvariantCulture), hgt = int.Parse(tok[2], CultureInfo.InvariantCulture);
                    _rng = uint.Parse(tok[3], CultureInfo.InvariantCulture);
                    if (_rng == 0) _rng = 1;
                    var data = new List<byte> { 0xFF, (byte)(1 + NextRand() % 254), (byte)(1 + NextRand() % 254) };
                    for (int y = 0; y < hgt; y++)
                    {
                        if (y > 0) data.Add(0x00);
                        for (int x = 0; x < w; x++) data.Add((byte)(1 + NextRand() % 254));
                    }
                    Feed(data.ToArray(), int.Parse(tok[4], CultureInfo.InvariantCulture));
                    break;
                }
                case "RAW" when tok.Length >= 2:
                {
                    byte[] data = tok.Skip(1).Select(x => byte.Parse(x, NumberStyles.HexNumber)).ToArray();
                    Feed(data, data.Length);
                    break;
                }
                case "DUMP":
                    Dump();
                    break;
                case "SCAN" when tok.Length >= 8:
                {
                    int[] a = tok.Skip(1).Take(7).Select(x => int.Parse(x, CultureInfo.InvariantCulture)).ToArray();
                    _out.WriteLine("RET " + _p.SemScanModeSelectAndScanStart(a[0], a[1], a[2], a[3], a[4], a[5], a[6]));
                    PrintState();
                    break;
                }
                case "SLOW12" when tok.Length >= 7:
                {
                    int[] a = tok.Skip(1).Take(6).Select(x => int.Parse(x, CultureInfo.InvariantCulture)).ToArray();
                    _out.WriteLine("RET " + _p.SemSlow12ScanModeSelectAndScanStart(a[0], a[1], a[2], a[3], a[4], a[5]));
                    PrintState();
                    break;
                }
                case "OPT" when tok.Length >= 3:
                    _integrated.nScanOptionData[int.Parse(tok[1], CultureInfo.InvariantCulture)] = int.Parse(tok[2], CultureInfo.InvariantCulture);
                    break;
                case "BOPT" when tok.Length >= 3:
                    _integrated.bScanOptionFlag[int.Parse(tok[1], CultureInfo.InvariantCulture)] = int.Parse(tok[2], CultureInfo.InvariantCulture);
                    break;
                case "CL" when tok.Length >= 6:
                    _scanDev.CL1 = int.Parse(tok[1], CultureInfo.InvariantCulture);
                    _scanDev.CL2 = int.Parse(tok[2], CultureInfo.InvariantCulture);
                    _curCL1 = int.Parse(tok[3], CultureInfo.InvariantCulture);
                    _curCL2 = int.Parse(tok[4], CultureInfo.InvariantCulture);
                    _scanDev.CLOnOff = int.Parse(tok[5], CultureInfo.InvariantCulture);
                    break;
                default:
                    _out.WriteLine("UNKNOWN " + tok[0]);
                    break;
            }
        }

        private uint NextRand()
        {
            _rng ^= _rng << 13;
            _rng ^= _rng >> 17;
            _rng ^= _rng << 5;
            return _rng;
        }

        private void Feed(byte[] data, int chunk)
        {
            for (int off = 0; off < data.Length; off += chunk)
            {
                int n = Math.Min(chunk, data.Length - off);
                var buf = new byte[n];
                Buffer.BlockCopy(data, off, buf, 0, n);
                _out.WriteLine("DATA " + n);
                int r;
                try
                {
                    r = _p.OnSemSEScanImageData(n, buf);
                }
                catch (Exception ex)
                {
                    _out.WriteLine("EXCEPTION " + ex.GetType().Name + ": " + ex.Message);
                    foreach (string fr in (ex.StackTrace ?? "").Split('\n').Where(x => x.Contains("SemScanImageProcessor")).Take(2)) _out.WriteLine("  at " + fr.Trim());
                    PrintState();
                    continue;
                }
                _out.WriteLine("RET " + r);
                PrintState();
            }
        }

        // ---- 상태 (harness.cpp PrintState와 같은 순서·형식) ----

        private static readonly FieldInfo ScanControlDlgFilter = typeof(SemScanImageProcessor.ScanControlDlg).GetField("m_bPhotoFilter1SelectFlag");
        private static readonly FieldInfo ScanControlDlgOldFilter = typeof(SemScanImageProcessor.ScanControlDlg).GetField("m_bOldPhotoFilter1SelectFlag");

        private void PrintState()
        {
            var cur = new List<string>();
            object sc = _p.m_SemSEScanControlPublicData;
            foreach (var f in _fields)
            {
                object v = sc.GetType().GetField(f.Key).GetValue(sc);
                if (_arrayCounts.TryGetValue(f.Key, out int count))
                {
                    var a = (Array)v;
                    for (int i = 0; i < count; i++) cur.Add(f.Key + "[" + i + "]=" + Value(a.GetValue(i), f.Value));
                }
                else cur.Add(f.Key + "=" + Value(v, f.Value));
            }
            foreach (string m in MemberNames)
            {
                string[] nt = m.Split(':');
                cur.Add(nt[0] + "=" + Value(typeof(SemScanImageProcessor).GetField(nt[0]).GetValue(_p), nt[1][0]));
            }
            cur.Add("m_wndScanControlDlg.m_bPhotoFilter1SelectFlag=" + ScanControlDlgFilter.GetValue(_p.m_wndScanControlDlg));
            cur.Add("m_wndScanControlDlg.m_bOldPhotoFilter1SelectFlag=" + ScanControlDlgOldFilter.GetValue(_p.m_wndScanControlDlg));
            if (_prev.Count != cur.Count) _prev = Enumerable.Repeat("", cur.Count).ToList();
            for (int i = 0; i < cur.Count; i++)
                if (cur[i] != _prev[i]) _out.WriteLine("F " + cur[i]);
            _prev = cur;
        }

        private static string Value(object v, char type)
        {
            switch (type)
            {
                case 'D': return CRuntime.Format("%.17g", Convert.ToDouble(v, CultureInfo.InvariantCulture));
                default: return Convert.ToString(v, CultureInfo.InvariantCulture);
            }
        }

        private bool SetField(string name, string value)
        {
            object sc = _p.m_SemSEScanControlPublicData;
            FieldInfo f = sc.GetType().GetField(name);
            if (f == null) f = typeof(SemScanImageProcessor).GetField(name);
            object target = f != null && f.DeclaringType == typeof(SemScanImageProcessor) ? (object)_p : sc;
            if (f == null && name == "m_wndScanControlDlg.m_bPhotoFilter1SelectFlag") { f = ScanControlDlgFilter; target = _p.m_wndScanControlDlg; }
            if (f == null && name == "m_wndScanControlDlg.m_bOldPhotoFilter1SelectFlag") { f = ScanControlDlgOldFilter; target = _p.m_wndScanControlDlg; }
            if (f == null) return false;
            long iv = value.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? long.Parse(value.Substring(2), NumberStyles.HexNumber) : 0;
            Type t = f.FieldType;
            object v = t == typeof(double) ? (object)CRuntime.atof(value)
                : t == typeof(uint) ? (object)(value.StartsWith("0x") ? (uint)iv : uint.Parse(value, CultureInfo.InvariantCulture))
                : t == typeof(ushort) ? (object)(value.StartsWith("0x") ? (ushort)iv : ushort.Parse(value, CultureInfo.InvariantCulture))
                : (object)(value.StartsWith("0x") ? (int)iv : int.Parse(value, CultureInfo.InvariantCulture));
            f.SetValue(target, v);
            return true;
        }

        private void Dump()
        {
            _out.WriteLine("H A " + Hex(Fnv(_p.m_pSemSEScanImageBitmapBufferA)));
            _out.WriteLine("H B " + Hex(_p.m_pSemSEScanImageBitmapBufferB.Fnv1a()));
            _out.WriteLine("H BF1 " + Hex(Fnv(_p.m_pSemSEScanImageBitmapBufferBF1)));
            _out.WriteLine("H BF2 " + Hex(Fnv(_p.m_pSemSEScanImageBitmapBufferBF2)));
            _out.WriteLine("H 1stRgb " + Hex(Fnv(_p.m_p1stImageRgb, _p.m_p1stImageRgb.Length)));
            int n2 = _p.m_bm2ndBitmap.bmWidth * _p.m_bm2ndBitmap.bmHeight;
            _out.WriteLine("H 2ndRgb " + Hex(_p.m_p2ndBitmapBits == null ? 0 : Fnv(_p.m_p2ndBitmapBits, n2)));
            _out.WriteLine("H 2ndOrg " + Hex(_p.m_p2ndOrgBitmapBits == null ? 0 : Fnv(_p.m_p2ndOrgBitmapBits, n2)));
            int nl = _p.m_bm2ndLayerBitmap.bmWidth * _p.m_bm2ndLayerBitmap.bmHeight;
            _out.WriteLine("H 2ndLayer " + _p.m_bm2ndLayerBitmap.bmWidth + " " + _p.m_bm2ndLayerBitmap.bmHeight + " " + Hex(_p.m_p2ndLayerBitmapBits == null ? 0 : Fnv(_p.m_p2ndLayerBitmapBits, nl)));
            _out.WriteLine("H 2ndBmp " + _p.m_bm2ndBitmap.bmWidth + " " + _p.m_bm2ndBitmap.bmHeight);
        }

        /// <summary>harness.cpp의 DEV 기록 (스캔 시작이 부르는 장치 쪽)</summary>
        private sealed class RecScanDevice : IScanDevice
        {
            private readonly TextWriter _out;
            public RecScanDevice(TextWriter o) { _out = o; }
            public int CL1, CL2, CLOnOff = 1;
            public void SetSEMImageCaptureOnOffValueUsbDeviceData(int a, int b) => _out.WriteLine("DEV SetSEMImageCaptureOnOffValueUsbDeviceData " + a + " " + b);
            public void SetSEMScanGenerationValueUsbDeviceData(int a) => _out.WriteLine("DEV SetSEMScanGenerationValueUsbDeviceData " + a);
            public void SetSEMSlow12ImageCaptureOnUsbDeviceData(int a) => _out.WriteLine("DEV SetSEMSlow12ImageCaptureOnUsbDeviceData " + a);
            public void ExecuteCLLMHModeData(int a, int b) => _out.WriteLine("DEV ExecuteCLLMHModeData " + a + " " + b);
            public int AfxIntegratedGetCL1() => CL1;
            public int AfxIntegratedGetCL2() => CL2;
            public void AfxIntegratedSetCL1(int n) => _out.WriteLine("DEV AfxIntegratedSetCL1 " + n);
            public void AfxIntegratedSetCL1() => _out.WriteLine("DEV AfxIntegratedSetCL1");
            public void AfxIntegratedSetCL2(int n) => _out.WriteLine("DEV AfxIntegratedSetCL2 " + n);
            public void AfxIntegratedSetCL2() => _out.WriteLine("DEV AfxIntegratedSetCL2");
            public int m_bCLOnOffFlag => CLOnOff;
        }

        private static string Hex(ulong v) => v.ToString("X16", CultureInfo.InvariantCulture);

        private static ulong Fnv(byte[] b)
        {
            ulong h = 1469598103934665603UL;
            foreach (byte x in b) { h ^= x; h *= 1099511628211UL; }
            return h;
        }

        private static ulong Fnv(RGBQUAD[] p, int count)
        {
            ulong h = 1469598103934665603UL;
            for (int i = 0; i < count; i++)
            {
                h ^= p[i].rgbBlue; h *= 1099511628211UL;
                h ^= p[i].rgbGreen; h *= 1099511628211UL;
                h ^= p[i].rgbRed; h *= 1099511628211UL;
                h ^= p[i].rgbReserved; h *= 1099511628211UL;
            }
            return h;
        }

        private static unsafe ulong Fnv(double[] d)
        {
            ulong h = 1469598103934665603UL;
            fixed (double* p = d)
            {
                byte* b = (byte*)p;
                for (int i = 0; i < d.Length * 8; i++) { h ^= b[i]; h *= 1099511628211UL; }
            }
            return h;
        }

        // ---- IScanImageView: harness.cpp의 대체 화면 객체와 같은 기록 ----

        public void SetInfoText(string control, string text) => _out.WriteLine("INFO " + control + " " + text);
        public void BitmapBitsSet(string bitmap, RGBQUAD[] bits, int byteCount) => _out.WriteLine("BMP " + bitmap + " " + byteCount + " " + Hex(Fnv(bits, bits.Length)));
        public void UpdateControl(string which)
        {
            RGBQUAD[] p = which == "SemSEScanImageDataUpdateControl" ? _p.m_p1stImageRgb : _p.m_p2ndImageRgb;
            int n = which == "SemSEScanImageDataUpdateControl" ? p.Length : _p.m_bm2ndBitmap.bmWidth * _p.m_bm2ndBitmap.bmHeight;
            _out.WriteLine("UPDATE " + which + " " + Hex(p == null ? 0 : Fnv(p, n)));
        }
        public void ScanControl(string action, string argument) => _out.WriteLine("CTL " + action + " " + argument);
        public void DrawHistogramGraph(double[] histogram, int cmax, int graphHeight) => _out.WriteLine("HISTO " + cmax + " " + graphHeight + " " + Hex(Fnv(histogram)));
        public void MonogramToolsSetScanModeExecute() => _out.WriteLine("MONO");
        public bool IsRunImageGrab(int type) => false;
        public void SetImageGrabEnd(int ok, RGBQUAD[] rgb) => _out.WriteLine("GRABEND");
        public uint GetTickCount() => _tick;
        public void DcFill(string dc, int x, int y, int width, int height, uint color) => _out.WriteLine("DC " + dc + " " + x + " " + y + " " + width + " " + height + " " + color);
        public void DrawRedScanBox(int left, int top, int right, int bottom, uint color) => _out.WriteLine("REDBOX " + left + " " + top + " " + right + " " + bottom + " " + color);
        public void DrawLayerObjects(string which) => _out.WriteLine("LAYER " + which);
        public void SetTimer(int id, int elapse) => _out.WriteLine("TIMER SET " + id + " " + elapse);
        public void KillTimer(int id) => _out.WriteLine("TIMER KILL " + id);
        public void ScanCalibration(string action, string argument) => _out.WriteLine("CAL " + action + " " + argument);
        public int GetCurrentCLValue(int n) => n == 1 ? _curCL1 : _curCL2;
    }
}
