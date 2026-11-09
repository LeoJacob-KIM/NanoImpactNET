// 메인 화면 창: 원본 화면 요소(진공 상태 글자, GUN HV·배율 콤보, 영상 칸, 정보 띠, CFE 받은 줄 목록)를 표준 컨트롤로 보이는 일만 한다.
// 장치·영상 연결은 Composition\MainShell.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using NanoImpact.App.Composition;
using NanoImpact.App.Shell;
using NanoImpact.Core.Native;
using static NanoImpact.Core.Def;

namespace NanoImpact.App
{
    public partial class MainWindow : Window
    {
        // 원본 콤보 목록 (NanoImpact2020cfeDlg.cpp:373 m_nTableGunHV, :392 m_nTableMagStepValue)
        private static readonly int[] TableGunHV = { 30000, 25000, 20000, 15000, 10000, 5000, 3000, 2900, 2800, 2700, 2600, 2500, 2400, 2300, 2200,
            2100, 2000, 1900, 1800, 1700, 1600, 1500, 1400, 1300, 1200, 1100, 1000, 900, 800, 700, 600, 500 };
        private static readonly int[] TableMagStepValue = { 20, 30, 35, 40, 45, 50, 60, 70, 80, 90, 100, 110, 120, 130, 150, 180, 200, 220, 250, 300, 350,
            400, 450, 500, 600, 700, 800, 900, 1000, 1100, 1200, 1300, 1500, 1800, 2000, 2200, 2500, 3000, 3500, 4000, 4500,
            5000, 6000, 7000, 8000, 9000, 10000, 11000, 12000, 13000, 15000, 18000, 20000, 22000, 25000, 30000, 35000, 40000,
            45000, 50000, 60000, 70000, 80000, 90000, 100000, 110000, 120000, 130000, 150000, 180000, 200000, 220000, 250000, 300000 };

        private readonly MainShell _shell;
        private readonly ObservableCollection<string[]> _cfeRows = new ObservableCollection<string[]>();
        private readonly SortedDictionary<string, string> _detailInfo = new SortedDictionary<string, string>(StringComparer.Ordinal);
        private readonly Stopwatch _fpsClock = Stopwatch.StartNew();
        private int _fpsFrames, _fpsRendered;

        internal ImageSurface First { get; }
        internal ImageSurface Second { get; }

        public MainWindow(bool sim, string iniFolder)
        {
            InitializeComponent();
            Title = CLIENT_DISPLAY_NAME + (sim ? "  [SIMULATOR]" : "");
            SimBadge.Visibility = sim ? Visibility.Visible : Visibility.Collapsed;
            First = new ImageSurface(FirstImage);
            Second = new ImageSurface(SecondImage);

            // 원본 SetMainDlgGunHVComboItem / SetMainDlgMagComboItem (배율은 큰 값부터)
            foreach (int hv in TableGunHV) GunHVCombo.Items.Add((hv / 1000.0).ToString("0.###", CultureInfo.InvariantCulture) + " kV");
            for (int i = 0; i < MAX_MAG_STEP; i++)
            {
                int mag = TableMagStepValue[(MAX_MAG_STEP - 1) - i];
                MagCombo.Items.Add(mag < 10000 ? "x" + mag : "x" + (mag / 1000) + "k");
            }
            GunHVCombo.SelectedIndex = 2;
            MagCombo.SelectedIndex = 2;
            CfeReceiveList.ItemsSource = _cfeRows;

            _shell = new MainShell(this, sim, iniFolder);
            PanelCallList.ItemsSource = _shell.PanelCalls;
            LogList.ItemsSource = _shell.Log;
            BuildScanButtons();
            Loaded += (s, e) => _shell.Start();
        }

        private void BuildScanButtons()
        {
            foreach (var m in new[] { (RED_SCAN_MODE, "RED"), (FAST_SCAN_MODE, "FAST"), (SLOW1_SCAN_MODE, "SLOW1"), (SLOW2_SCAN_MODE, "SLOW2") })
            {
                var b = new Button { Content = m.Item2 };
                int mode = m.Item1;
                b.Click += (s, e) => _shell.RequestLiveScan(mode);
                LiveScanButtons.Children.Add(b);
            }
            int[] photos = { PHOTO1_SCAN_MODE, PHOTO2_SCAN_MODE, PHOTO3_SCAN_MODE, PHOTO4_SCAN_MODE, PHOTO5_SCAN_MODE, PHOTO6_SCAN_MODE };
            for (int i = 0; i < photos.Length; i++)
            {
                var b = new Button { Content = "PHOTO" + (i + 1) };
                int mode = photos[i];
                b.Click += (s, e) => _shell.RequestPhotoScan(mode);
                PhotoScanButtons.Children.Add(b);
            }
            CaptureStopButton.Click += (s, e) => _shell.CaptureStop();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (!_shell.OnClose()) e.Cancel = true;
        }

        private void OnExitClick(object sender, RoutedEventArgs e) => Close();

        // ---- MainShell이 부르는 화면 갱신 ----

        internal void SetVacuumState(string text, int state, bool visible)
        {
            VacuumStateText.Text = text;
            VacuumStateText.Opacity = visible ? 1.0 : 0.35;
            if (state == -1) return; // 깜빡임만
            Color c;
            if (state == CLIENT_VAC_OK_STATE || state == CLIENT_HV_VAC_OK_STATE) c = Color.FromRgb(0x2E, 0x7D, 0x32);
            else if (state == CLIENT_HV_START_STATE || state == CLIENT_HV_ON_STATE) c = Color.FromRgb(0x1E, 0x63, 0xB5);
            else if (state == CLIENT_POWER_OFF_STATE) c = Color.FromRgb(0x8E, 0x24, 0x24);
            else c = Color.FromRgb(0x8A, 0x5A, 0x00);
            VacuumStateBox.Background = new SolidColorBrush(c);
        }

        internal void SetMagIndex(int nIndex)
        {
            if (nIndex >= 0 && nIndex < MagCombo.Items.Count) MagCombo.SelectedIndex = nIndex;
        }

        internal void SetGunHVIndex(int nIndex)
        {
            if (nIndex >= 0 && nIndex < GunHVCombo.Items.Count) GunHVCombo.SelectedIndex = nIndex;
        }

        /// <summary>스캔 시작 직전: RED 상자는 RED일 때만 남긴다, 영상 칸 머리글에 모드</summary>
        internal void BeginScanMode(int nScanMode, int width, int height)
        {
            string name = ScanModeName(nScanMode);
            bool photo = nScanMode >= PHOTO1_SCAN_MODE && nScanMode <= PHOTO6_SCAN_MODE;
            if (nScanMode != RED_SCAN_MODE && !photo) RedScanBox.Visibility = Visibility.Collapsed;
            if (photo) SecondImageModeText.Text = name + "  " + width + "x" + height;
            else FirstImageModeText.Text = name + "  " + width + "x" + height;
            ScanModeText.Text = "scan " + name;
        }

        internal static string ScanModeName(int nScanMode)
        {
            if (nScanMode == RED_SCAN_MODE) return "RED";
            if (nScanMode == FAST_SCAN_MODE) return "FAST";
            if (nScanMode == SLOW1_SCAN_MODE) return "SLOW1";
            if (nScanMode == SLOW2_SCAN_MODE) return "SLOW2";
            if (nScanMode >= PHOTO1_SCAN_MODE && nScanMode <= PHOTO6_SCAN_MODE) return "PHOTO" + (nScanMode - PHOTO1_SCAN_MODE + 1);
            return "mode " + nScanMode;
        }

        /// <summary>원본 Draw1stREDScanBox (1st 영상 640x480 좌표)</summary>
        internal void ShowRedScanBox(int left, int top, int right, int bottom)
        {
            Canvas.SetLeft(RedScanBox, left);
            Canvas.SetTop(RedScanBox, top);
            RedScanBox.Width = Math.Max(0, right - left);
            RedScanBox.Height = Math.Max(0, bottom - top);
            RedScanBox.Visibility = Visibility.Visible;
        }

        /// <summary>원본 Update1stImageInfoBitmap — 정보 띠 (배율·HV·스캔). 눈금자와 글꼴 그림은 10단계.</summary>
        internal void UpdateFirstInfo(IntegratedCommandStruct ic) => FirstImageInfoText.Text = InfoLine(ic);

        internal void UpdateSecondInfo(IntegratedCommandStruct ic) => SecondImageInfoText.Text = InfoLine(ic);

        private static string InfoLine(IntegratedCommandStruct ic)
        {
            if (ic == null) return " ";
            int mag = ic.nMagStepIndex >= 0 && ic.nMagStepIndex < TableMagStepValue.Length ? TableMagStepValue[ic.nMagStepIndex] : 0;
            return string.Format(CultureInfo.InvariantCulture, "HV {0:0.##} kV   MAG x{1}   SPOT {2}", ic.nGunHVCurValue / 1000.0, mag, ic.nSpotSize);
        }

        internal void SetDetailInfo(string control, string text)
        {
            _detailInfo[control] = text;
            DetailInfoList.ItemsSource = _detailInfo.Select(kv => kv.Key + "  " + kv.Value).ToList();
        }

        internal void CfeListInsert(int item, string text)
        {
            var row = new[] { item.ToString(CultureInfo.InvariantCulture), text, "" };
            if (item >= 0 && item <= _cfeRows.Count) _cfeRows.Insert(item, row);
            else _cfeRows.Add(row);
            while (_cfeRows.Count > 500) _cfeRows.RemoveAt(_cfeRows.Count - 1);
        }

        internal void CfeListSetText(int item, int subItem, string text)
        {
            if (item < 0 || item >= _cfeRows.Count || subItem < 0 || subItem > 2) return;
            string[] row = (string[])_cfeRows[item].Clone();
            row[subItem == 0 ? 1 : 2] = text;
            _cfeRows[item] = row;
        }

        /// <summary>1st 영상 화면 갱신 수 (상태 막대 fps)</summary>
        internal void CountFrame()
        {
            _fpsFrames++;
            double s = _fpsClock.Elapsed.TotalSeconds;
            if (s >= 1.0)
            {
                int rendered = First.RenderCount - _fpsRendered;
                _fpsRendered = First.RenderCount;
                FrameRateText.Text = string.Format(CultureInfo.InvariantCulture, "1st redraw {0:0.0}/s (requests {1:0.0}/s)", rendered / s, _fpsFrames / s);
                _fpsFrames = 0;
                _fpsClock.Restart();
            }
        }
    }
}
