// 원본 USB_FT232H_Device.cpp:1026-1101 을 NanoImpact.CodeGen translate로 옮긴 뒤 손으로 고친 코드.
#pragma warning disable IDE1006, IDE0059, CS0162, CS0168, CS0219
using System;
using NanoImpact.Core.Native;
using static NanoImpact.Core.Def;
using static NanoImpact.Core.Native.CRuntime;

namespace NanoImpact.Hardware.Device
{
    public sealed partial class SemDevice
    {
        // ---- USB_FT232H_Device.cpp:1026 ----
        //
        //
        //
        //

















        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //
        //

        //
        // OWNER 윈도우 설정
        //
        public void AfxUSBFT232HSetOwnerWindow(int hOwner)
        {
            m_hOwnerWnd = hOwner;
        }

        // ---- USB_FT232H_Device.cpp:1030 ----
        public int AfxUSBFT232HGetOwnerWindow()
        {
            return m_hOwnerWnd;
        }

        // ---- USB_FT232H_Device.cpp:1037 ----
        //
        // Main Control Child 윈도우 설정
        //
        public void AfxUSBFT232HSetMainControlWindow(int hChild)
        {
            m_hMainControlWnd = hChild;
        }

        // ---- USB_FT232H_Device.cpp:1044 ----
        //
        // H/W Setup & Control Child 윈도우 설정
        //
        public void AfxUSBFT232HSetHWSetupControlWindow(int hChild)
        {
            m_hHWSetupControlWnd = hChild;
        }

        // ---- USB_FT232H_Device.cpp:1051 ----
        //
        // Stage Setup & Control Child 윈도우 설정
        //
        public void AfxUSBFT232HSetStageSetupControlWindow(int hChild)
        {
            m_hStageSetupControlWnd = hChild;
        }

        // ---- USB_FT232H_Device.cpp:1058 ----
        //
        // Device Etc Setup & Control Child 윈도우 설정
        //
        public void AfxUSBFT232HSetPopupHWSetupControlWindow(int hChild)
        {
            m_hPopupHWSetupControlWnd = hChild;
        }

        // ---- USB_FT232H_Device.cpp:1065 ----
        //
        // Device Etc Setup & Control Child 윈도우 설정
        //
        public void AfxUSBFT232HSetChild5Window(int hChild)
        {
            m_hChild5Wnd = hChild;
        }

        // ---- USB_FT232H_Device.cpp:1072 ----
        //
        // MAIN SERVER 실행파일 경로 설정 및 가져오기
        //
        public void AfxSetMainServerPath(string strPath)
        {
            m_strMainServerPath = strPath;
        }

        // ---- USB_FT232H_Device.cpp:1076 ----
        public string AfxGetMainServerPath()
        {
            return m_strMainServerPath;
        }

        // ---- USB_FT232H_Device.cpp:1083 ----
        //
        // DEVICE SERVER 실행파일 경로 설정 및 가져오기
        //
        public void AfxSetDeviceServerPath(string strPath)
        {
            m_strDeviceServerPath = strPath;
        }

        // ---- USB_FT232H_Device.cpp:1087 ----
        public string AfxGetDeviceServerPath()
        {
            return m_strDeviceServerPath;
        }

        // ---- USB_FT232H_Device.cpp:1094 ----
        //
        // 프로그램 루틴 소요시간 계산을 위한값
        //
        public uint AfxGetFirstTickCount()
        {
            return m_dwFirstTickCount;
        }

        // ---- USB_FT232H_Device.cpp:1098 ----
        public uint AfxGetSecondTickCount()
        {
            return m_dwSecondTickCount;
        }

    }
}
