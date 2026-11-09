// 원본 NanoImpact2020cfeDlg.cpp:7342-7359 을 NanoImpact.CodeGen translate로 옮긴 뒤 손으로 고친 코드.
#pragma warning disable IDE1006, IDE0059, CS0162, CS0168, CS0219
using System;
using NanoImpact.Core.Native;
using static NanoImpact.Core.Def;
using static NanoImpact.Core.Native.CRuntime;

namespace NanoImpact.Hardware.Imaging
{
    public sealed partial class SemScanImageProcessor
    {
        // ---- NanoImpact2020cfeDlg.cpp:7342 ----
        public void Draw1stREDScanBox(int nLeft, int nTop, int nRight, int nBottom, uint colorFill)
        {
            // 1st Image (SEM Scan Image) Box
            // RED Scan Mode 시 테두리를 그린다 (원본: m_1stImageMemDC에 1픽셀 펜으로 MoveTo/LineTo 네 변) → 화면 쪽이 그린다
            _view.DrawRedScanBox(nLeft, nTop, nRight, nBottom, colorFill);
        }

    }
}
