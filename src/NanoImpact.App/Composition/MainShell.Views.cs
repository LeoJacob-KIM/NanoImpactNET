// 메인 화면의 화면 쪽 인터페이스 3개: 장치 처리기(IMainDeviceView), 영상 조립(IScanImageView), Spellman 감시(ICfeHVPowerView).
// 7단계에서 그리는 것: 영상 칸, 히스토그램, 진공·HV 표시, RED 스캔 상자, 정보 글자, CFE 받은 줄. 나머지는 기록만 남기고 해당 단계를 적는다.
using System;
using System.Globalization;
using NanoImpact.Core;
using NanoImpact.Hardware.Device;
using NanoImpact.Hardware.Imaging;
using NanoImpact.Hardware.Spellman;
using static NanoImpact.Core.Def;

namespace NanoImpact.App.Composition
{
    internal sealed partial class MainShell
    {
        /// <summary>장치 처리기가 부르는 메인 화면 함수 (원본 CNanoImpact2020cfeDlg)</summary>
        private sealed class MainView : IMainDeviceView
        {
            private readonly MainShell _s;
            public MainView(MainShell s) { _s = s; }

            public void Update1stImageInfoBitmap(int nX, int nY, int nWidth, int nHeight) => _s._w.UpdateFirstInfo(_s._main.m_IntegratedData);
            public void Update2ndImageInfoBitmap(int nX, int nY, int nWidth, int nHeight) => _s._w.UpdateSecondInfo(_s._main.m_IntegratedData);
            public void DisplayVacuumStateInfo(int nVacuumState) => _s.DisplayVacuumStateInfo(nVacuumState);
            public void SetMainDlgMagComboIndex(int nIndex) => _s._w.SetMagIndex(nIndex);
            public void SetMainDlgGunHVComboIndex(int nIndex) => _s._w.SetGunHVIndex(nIndex);

            public int SemScanModeSelectAndScanStart(int nScanMode, int nScanWidth, int nScanHeight, int nBitmapXOffset, int nBitmapYOffset, int nBitmapAverage, int bScanGeneration)
            {
                _s._w.BeginScanMode(nScanMode, nScanWidth, nScanHeight);
                return _s._proc.SemScanModeSelectAndScanStart(nScanMode, nScanWidth, nScanHeight, nBitmapXOffset, nBitmapYOffset, nBitmapAverage, bScanGeneration);
            }

            public int SemSlow12ScanModeSelectAndScanStart(int nScanMode, int nScanWidth, int nScanHeight, int nBitmapXOffset, int nBitmapYOffset, int nBitmapAverage)
            {
                _s._w.BeginScanMode(nScanMode, nScanWidth, nScanHeight);
                return _s._proc.SemSlow12ScanModeSelectAndScanStart(nScanMode, nScanWidth, nScanHeight, nBitmapXOffset, nBitmapYOffset, nBitmapAverage);
            }

            public void SemSEScanImageDataUpdateControl() => _s.SemSEScanImageDataUpdateControl();
            public void StageXYAxisMagnificationMovingSpeedChange(int nMagIndex) => _s.StageXYAxisMagnificationMovingSpeedChange(nMagIndex);
            public void SetExecuteAutoProcessingStop() => _s.AddLog("SetExecuteAutoProcessingStop (step 9)");
            public void SetExecuteAutoFocusProcessing() => _s.AddLog("SetExecuteAutoFocusProcessing (step 9)");
            public void SetExecuteAutoContrastProcessing() => _s.AddLog("SetExecuteAutoContrastProcessing (step 9)");
            public void SetExecuteAutoContrastBrightnessProcessing() => _s.AddLog("SetExecuteAutoContrastBrightnessProcessing (step 9)");
            public void SetExecuteAutoBrightnessProcessing() => _s.AddLog("SetExecuteAutoBrightnessProcessing (step 9)");
            public int SetAutoBCCompletedFlag(int bOnOff) => 0;
            public int GetAutoBCRunFlag() => 0;
            public void SetWDSetupEnabledOLValue(int nCoarse) => ((Placeholders.IPanelSink)_s).Call("m_wndScanCalibrationToolDlg.SetWDSetupEnabledOLValue", nCoarse);
            public void SetTimer(int nIDEvent, int nElapse) => _s._mainTimers.Set(nIDEvent, nElapse);
            public void KillTimer(int nIDEvent) => _s._mainTimers.Kill(nIDEvent);
        }

        /// <summary>영상 조립이 부르는 화면 쪽</summary>
        private sealed class ScanView : IScanImageView
        {
            private readonly MainShell _s;
            public ScanView(MainShell s) { _s = s; }

            public void SetInfoText(string control, string text) => _s._w.SetDetailInfo(control, text);
            public void BitmapBitsSet(string bitmap, RGBQUAD[] bits, int byteCount) { } // 표시는 *UpdateControl 타이머가 CBitmap.Bits를 그린다
            public void UpdateControl(string which)
            {
                if (which == "SemSEScanImageDataUpdateControl") _s.SemSEScanImageDataUpdateControl();
                else if (which == "SemSEPhotoScan12ImageDataUpdateControl") _s.SemSEPhotoScanImageDataUpdateControl(_s._proc.m_2ndImage640x480Bitmap);
                else if (which == "SemSEPhotoScan345ImageDataUpdateControl") _s.SemSEPhotoScanImageDataUpdateControl(_s._proc.m_2ndImage1280x960Bitmap);
                else if (which == "SemSEPhotoScan6ImageDataUpdateControl") _s.SemSEPhotoScanImageDataUpdateControl(_s._proc.m_2ndImage2560x1920Bitmap);
            }
            public void ScanControl(string action, string argument) => ((Placeholders.IPanelSink)_s).Call("m_wndScanControlDlg." + action, argument);
            public void DrawHistogramGraph(double[] histogram, int cmax, int graphHeight) => _s._w.Histogram.SetHistogram(histogram, cmax);
            public void MonogramToolsSetScanModeExecute() { } // Monogram 단축키 (10단계)
            public bool IsRunImageGrab(int type) => false;    // UDP 원격 저장 (11단계)
            public void SetImageGrabEnd(int ok, RGBQUAD[] rgb) { }
            public uint GetTickCount() => unchecked((uint)Environment.TickCount);
            public void DcFill(string dc, int x, int y, int width, int height, uint color)
            {
                if (dc == "1stImageMemDC") _s._w.First.Fill(x, y, width, height, color);
            }
            public void DrawRedScanBox(int left, int top, int right, int bottom, uint color) => _s._w.ShowRedScanBox(left, top, right, bottom);
            public void DrawLayerObjects(string which) { } // 측정 레이어 (10단계)
            public void SetTimer(int id, int elapse) => _s._mainTimers.Set(id, elapse);
            public void KillTimer(int id) => _s._mainTimers.Kill(id);
            public void ScanCalibration(string action, string argument) => ((Placeholders.IPanelSink)_s).Call("m_wndScanCalibrationToolDlg." + action, argument);

            /// <summary>원본은 Scan 보정 창의 현재 CL 값 (8단계 패널). 그 전까지는 장치 공용 데이터의 현재 값을 그대로 다시 적용한다.</summary>
            public int GetCurrentCLValue(int n) => n == 1 ? _s._dev.AfxIntegratedGetCL1() : _s._dev.AfxIntegratedGetCL2();
        }

        /// <summary>Spellman 감시가 부르는 화면 쪽 (모니터 창·Beam 창·상단 막대·받은 줄 목록)</summary>
        private sealed class CfeView : ICfeHVPowerView
        {
            private readonly MainShell _s;
            public CfeView(MainShell s) { _s = s; }

            public void SetText(string target, string text)
            {
                if (target == "m_TopToolFEPowerStatusText") _s._w.FEPowerStatusText.Text = text;
                else if (target == "m_TopToolExtractorCurrentText") _s._w.ExtractorCurrentText.Text = text;
                else _s._w.CfeStatusText.Text = target + ": " + text;
            }
            public void SetImage(string target, int index, int onOff) => ((Placeholders.IPanelSink)_s).Call("cfe." + target, index, onOff);
            public void SetSpinPos(string control, int pos) => ((Placeholders.IPanelSink)_s).Call("cfe." + control + ".SetPos", pos);
            public void ListInsertItem(int item, string text) => _s._w.CfeListInsert(item, text);
            public void ListSetItemText(int item, int subItem, string text) => _s._w.CfeListSetText(item, subItem, text);
            public int GetReceiveDataViewCheck() => 0;
            public void SetTimer(int id, int elapse) => _s._beamTimers.Set(id, elapse);
            public void KillTimer(int id) => _s._beamTimers.Kill(id);
            public void ExecuteFocusDegaussing(int milliseconds) => _s.m_pAssistDeviceTool.ExecuteFocusDegaussing(milliseconds);
        }
    }

    /// <summary>스캔 시작(SemScanModeSelectAndScanStart)이 부르는 장치 쪽: ModularAssistDevice·Afx* CL 함수·메인 화면 m_bCLOnOffFlag</summary>
    internal sealed class ScanDeviceAdapter : IScanDevice
    {
        private readonly AssistDevice _assist;
        private readonly SemDevice _dev;
        private readonly MainDeviceService _main;
        private readonly Sim.SimScanSource _sim;

        public ScanDeviceAdapter(AssistDevice assist, SemDevice dev, MainDeviceService main, Sim.SimScanSource sim)
        {
            _assist = assist;
            _dev = dev;
            _main = main;
            _sim = sim;
        }

        public void SetSEMImageCaptureOnOffValueUsbDeviceData(int nOnOff, int nScanMode)
        {
            _assist.SetSEMImageCaptureOnOffValueUsbDeviceData(nOnOff, nScanMode);
            _sim?.Capture(nOnOff, nScanMode);
        }

        public void SetSEMScanGenerationValueUsbDeviceData(int nScanMode) => _assist.SetSEMScanGenerationValueUsbDeviceData(nScanMode);

        public void SetSEMSlow12ImageCaptureOnUsbDeviceData(int nScanMode)
        {
            _assist.SetSEMSlow12ImageCaptureOnUsbDeviceData(nScanMode);
            _sim?.Capture(1, nScanMode);
        }

        public void ExecuteCLLMHModeData(int nCLLMHMode, int nCLLMH123Mode) => _assist.ExecuteCLLMHModeData(nCLLMHMode, nCLLMH123Mode);
        public int AfxIntegratedGetCL1() => _dev.AfxIntegratedGetCL1();
        public int AfxIntegratedGetCL2() => _dev.AfxIntegratedGetCL2();
        public void AfxIntegratedSetCL1(int nCL1) => _dev.AfxIntegratedSetCL1(nCL1);
        public void AfxIntegratedSetCL1() => _dev.AfxIntegratedSetCL1();
        public void AfxIntegratedSetCL2(int nCL2) => _dev.AfxIntegratedSetCL2(nCL2);
        public void AfxIntegratedSetCL2() => _dev.AfxIntegratedSetCL2();
        public int m_bCLOnOffFlag => _main.m_bCLOnOffFlag;
    }
}
