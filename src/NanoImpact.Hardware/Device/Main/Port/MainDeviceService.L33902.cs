// 원본 NanoImpact2020cfeDlg.cpp:33902-33920 을 NanoImpact.CodeGen translate로 옮긴 뒤 손으로 고친 코드.
#pragma warning disable IDE1006, IDE0059, CS0162, CS0168, CS0219
using System;
using NanoImpact.Core.Native;
using static NanoImpact.Core.Def;
using static NanoImpact.Core.Native.CRuntime;

namespace NanoImpact.Hardware.Device
{
    public sealed partial class MainDeviceService
    {
        // ---- NanoImpact2020cfeDlg.cpp:33902 ----
        // USB CONTROL DATA : FOCUS COARSE DATA EXECUTE
        public int OnSemControlFocusCoarseDataExecute(int wParam, object lParam)
        {
            if (lParam == null) return 0;

            // 2021.07.22
            // FOCUS COARSE DATA EXECUTE
            USB_DEVICE_DATA_INFO UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();
            UsbDeviceDataInfo = new USB_DEVICE_DATA_INFO();

            UsbDeviceDataInfo.CopyFrom((USB_DEVICE_DATA_INFO)lParam);

            int nFocusCoarse = UsbDeviceDataInfo.nData[0];
            int nFocusFine   = UsbDeviceDataInfo.nData[1];

            // FOCUS COARSE DATA EXECUTE
            m_wndFocusControlDlg.ExecuteFocusControlFocusCoarseFineValueData(nFocusCoarse, nFocusFine);

            return 1;
        }

    }
}
