using System;

namespace NanoImpact.Hardware.Device
{
    /// <summary>
    /// 장치 계층이 바깥으로 하는 일. 원본은 Win32 API를 바로 불렀다(::SendMessage, ::MessageBox, ::Sleep, AfxBeginThread).
    /// 실제 앱은 WPF 쪽 구현을, 원본 대조 시험은 기록하는 구현을 넣는다.
    /// </summary>
    public interface IDeviceHost
    {
        /// <summary>원본 ::MessageBox. 반환값은 IDOK 등.</summary>
        int MessageBox(int hWnd, string text, string caption, uint type);

        /// <summary>
        /// 원본 ::SendMessage(hWnd, msg, wParam, lParam). hWnd는 창 번호(원본 HWND 자리, 0 = 없음),
        /// lParam은 정수(int) 또는 구조체 객체(USB_DEVICE_DATA_INFO 등).
        /// </summary>
        int SendMessage(int hWnd, int msg, int wParam, object lParam);

        /// <summary>원본 ::PostMessage</summary>
        bool PostMessage(int hWnd, int msg, int wParam, object lParam);

        /// <summary>원본 ::Sleep</summary>
        void Sleep(int milliseconds);

        /// <summary>원본 ::GetTickCount</summary>
        uint GetTickCount();

        /// <summary>원본 AfxBeginThread(proc, param). 실제 구현은 스레드를 시작하고, 시험 구현은 기록만 한다.</summary>
        object BeginThread(Func<object, uint> proc, object param);
    }
}
