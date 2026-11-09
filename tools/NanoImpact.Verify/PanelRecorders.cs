using System.Globalization;
using System.IO;
using System.Text;
using NanoImpact.Core.Native;
using NanoImpact.Hardware.Device;

namespace NanoImpact.Verify
{
    /// <summary>
    /// PanelRecorders.g.cs(생성)의 공용 부분과, 메인 화면 장치 처리기가 부르는 범위 밖 화면 함수의 기록 (tools\device_harness harness.cpp와 같은 형식).
    /// </summary>
    internal static partial class PanelRecorders
    {
        internal static string Dbl(double v) => CRuntime.Format("%.17g", v);

        /// <summary>harness.cpp Text: 제어 문자를 \xHH로</summary>
        internal static string Txt(string s)
        {
            var sb = new StringBuilder();
            foreach (char c in s ?? "")
            {
                if (c < 0x20 || c == 0x7F) sb.Append("\\x").Append(((int)c).ToString("X2", CultureInfo.InvariantCulture));
                else sb.Append(c);
            }
            return sb.ToString();
        }

        internal static string Hash(IntegratedCommandStruct s)
        {
            if (s == null) return "0000000000000000";
            ulong h = 1469598103934665603UL;
            foreach (byte b in s.ToBytes()) { h ^= b; h *= 1099511628211UL; }
            return h.ToString("X16", CultureInfo.InvariantCulture);
        }

        internal sealed class RecText : ITextControl
        {
            private readonly TextWriter _out;
            private readonly string _name;
            public RecText(TextWriter output, string name) { _out = output; _name = name; }
            public void SetWindowTextW(string text) => _out.WriteLine("P " + _name + ".SetWindowTextW " + Txt(text));
            public void SetTextDataString(string text) => _out.WriteLine("P " + _name + ".SetTextDataString " + Txt(text));
        }

        internal static MainPanels Create(TextWriter o) => new MainPanels
        {
            AlignControl = new RecModularAlignControlDlg(o),
            BeamControl = new RecModularBeamControlDlg(o),
            DetailControl = new RecModularDetailControlDlg(o),
            EnvironmentTool = new RecModularEnvironmentToolDlg(o),
            FocusControl = new RecModularFocusControlDlg(o),
            ScanCalibrationTool = new RecModularScanCalibrationToolDlg(o),
            ScanControl = new RecModularScanControlDlg(o),
            StageCalibrationTool = new RecModularStageCalibrationToolDlg(o),
            StageControl = new RecModularStageControlDlg(o),
            VacuumControl = new RecModularVacuumControlDlg(o),
        };

        /// <summary>harness.cpp CNanoImpact2020cfeDlg의 범위 밖 함수와 같은 기록 ("V 이름 인자", "TIMER SET/KILL")</summary>
        internal sealed class RecMainView : IMainDeviceView
        {
            private readonly TextWriter _out;
            public RecMainView(TextWriter output) { _out = output; }
            public int AutoBCRun { get; set; }

            private void V(string name, params int[] a) => _out.WriteLine("V " + name + (a.Length > 0 ? " " + string.Join(" ", a) : ""));
            public void Update1stImageInfoBitmap(int nX, int nY, int nWidth, int nHeight) => V("Update1stImageInfoBitmap", nX, nY, nWidth, nHeight);
            public void Update2ndImageInfoBitmap(int nX, int nY, int nWidth, int nHeight) => V("Update2ndImageInfoBitmap", nX, nY, nWidth, nHeight);
            public void DisplayVacuumStateInfo(int n) => V("DisplayVacuumStateInfo", n);
            public void SetMainDlgMagComboIndex(int n) => V("SetMainDlgMagComboIndex", n);
            public void SetMainDlgGunHVComboIndex(int n) => V("SetMainDlgGunHVComboIndex", n);
            public int SemScanModeSelectAndScanStart(int a, int b, int c, int d, int e, int f, int g) { V("SemScanModeSelectAndScanStart", a, b, c, d, e, f, g); return 0; }
            public int SemSlow12ScanModeSelectAndScanStart(int a, int b, int c, int d, int e, int f) { V("SemSlow12ScanModeSelectAndScanStart", a, b, c, d, e, f); return 0; }
            public void SemSEScanImageDataUpdateControl() => V("SemSEScanImageDataUpdateControl");
            public void StageXYAxisMagnificationMovingSpeedChange(int n) => V("StageXYAxisMagnificationMovingSpeedChange", n);
            public void SetExecuteAutoProcessingStop() => V("SetExecuteAutoProcessingStop");
            public void SetExecuteAutoFocusProcessing() => V("SetExecuteAutoFocusProcessing");
            public void SetExecuteAutoContrastProcessing() => V("SetExecuteAutoContrastProcessing");
            public void SetExecuteAutoContrastBrightnessProcessing() => V("SetExecuteAutoContrastBrightnessProcessing");
            public void SetExecuteAutoBrightnessProcessing() => V("SetExecuteAutoBrightnessProcessing");
            public int SetAutoBCCompletedFlag(int b) { V("SetAutoBCCompletedFlag", b); return 1; }
            public int GetAutoBCRunFlag() { V("GetAutoBCRunFlag"); return AutoBCRun; }
            public void SetWDSetupEnabledOLValue(int n) => V("SetWDSetupEnabledOLValue", n);
            public void SetTimer(int id, int ms) => _out.WriteLine("TIMER SET " + id + " " + ms);
            public void KillTimer(int id) => _out.WriteLine("TIMER KILL " + id);
        }
    }
}
