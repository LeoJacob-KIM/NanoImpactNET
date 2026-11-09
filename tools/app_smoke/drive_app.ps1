# Drive NanoImpact.exe --sim with UI Automation and save window screenshots (PrintWindow). Keep this file ASCII.
#   powershell -File drive_app.ps1 -Exe ..\..\src\NanoImpact.App\bin\Debug\NanoImpact.exe -Ini <copy of INI folder> -OutDir <png folder> [-Steps ...]
# Steps: wait:<sec>  shot:<name>  click:<button text>  tab:<tab header>  answer:<dialog button text part>  exited:<sec>
# Use a COPY of the INI folder: closing the app with answer:Y saves USBINTEGRATEDDATA.INI there.
param(
    [string]$Exe,
    [string]$Ini,
    [string]$OutDir,
    [string[]]$Steps = @('wait:6', 'shot:start', 'click:FAST', 'wait:4', 'shot:fast', 'click:RED', 'wait:3', 'shot:red', 'click:PHOTO1', 'wait:5', 'shot:photo1')
)
Add-Type -AssemblyName UIAutomationClient, UIAutomationTypes
Add-Type @'
using System;
using System.Runtime.InteropServices;
public static class W32 {
  [DllImport("user32.dll")] public static extern bool PrintWindow(IntPtr h, IntPtr hdc, uint flags);
  [DllImport("user32.dll")] public static extern bool GetWindowRect(IntPtr h, out RECT r);
  [DllImport("user32.dll")] public static extern bool SetForegroundWindow(IntPtr h);
  [StructLayout(LayoutKind.Sequential)] public struct RECT { public int L, T, R, B; }
}
'@
Add-Type -AssemblyName System.Drawing

function Shot($hwnd, $path) {
    $r = New-Object W32+RECT
    [W32]::GetWindowRect($hwnd, [ref]$r) | Out-Null
    $w = $r.R - $r.L; $h = $r.B - $r.T
    $bmp = New-Object System.Drawing.Bitmap $w, $h
    $g = [System.Drawing.Graphics]::FromImage($bmp)
    $hdc = $g.GetHdc()
    [W32]::PrintWindow($hwnd, $hdc, 2) | Out-Null
    $g.ReleaseHdc($hdc); $g.Dispose()
    $bmp.Save($path, [System.Drawing.Imaging.ImageFormat]::Png); $bmp.Dispose()
    "shot $path"
}

function Dialogs($procId) {
    $cond = New-Object System.Windows.Automation.PropertyCondition([System.Windows.Automation.AutomationElement]::ProcessIdProperty, $procId)
    [System.Windows.Automation.AutomationElement]::RootElement.FindAll([System.Windows.Automation.TreeScope]::Children, $cond)
}

$p = Start-Process -FilePath $Exe -ArgumentList @('--sim', '--ini', $Ini) -PassThru
try {
    $main = $null
    for ($i = 0; $i -lt 100 -and -not $main; $i++) {
        Start-Sleep -Milliseconds 200
        foreach ($wnd in (Dialogs $p.Id)) { if ($wnd.Current.Name -like 'NanoImpact2020cfe*') { $main = $wnd } }
    }
    if (-not $main) { 'main window not found'; return }
    $hwnd = [IntPtr]$main.Current.NativeWindowHandle
    foreach ($s in $Steps) {
        $k, $v = $s -split ':', 2
        # blocking dialog (MessageBox): print its text and press the first button
        if ($p.HasExited) { "process exited (code $($p.ExitCode))"; break }
        foreach ($wnd in (Dialogs $p.Id)) {
            if ($k -ne 'answer' -and $wnd.Current.NativeWindowHandle -ne $main.Current.NativeWindowHandle) {
                $txt = ($wnd.FindAll([System.Windows.Automation.TreeScope]::Descendants, [System.Windows.Automation.Condition]::TrueCondition) | ForEach-Object { $_.Current.Name } | Where-Object { $_ }) -join ' | '
                "dialog: $($wnd.Current.Name): $txt"
                $ok = $wnd.FindFirst([System.Windows.Automation.TreeScope]::Descendants, (New-Object System.Windows.Automation.PropertyCondition([System.Windows.Automation.AutomationElement]::ControlTypeProperty, [System.Windows.Automation.ControlType]::Button)))
                if ($ok) { $ok.GetCurrentPattern([System.Windows.Automation.InvokePattern]::Pattern).Invoke() }
            }
        }
        switch ($k) {
            'wait' { Start-Sleep -Seconds ([double]$v) }
            'shot' { Shot $hwnd (Join-Path $OutDir "$v.png") }
            'click' {
                $b = $main.FindFirst([System.Windows.Automation.TreeScope]::Descendants, (New-Object System.Windows.Automation.PropertyCondition([System.Windows.Automation.AutomationElement]::NameProperty, $v)))
                if ($b) { $b.GetCurrentPattern([System.Windows.Automation.InvokePattern]::Pattern).Invoke(); "click $v" } else { "button $v not found" }
            }
            'answer' {
                $done = $false
                for ($i = 0; $i -lt 25 -and -not $done; $i++) {
                    $owned = $main.FindAll([System.Windows.Automation.TreeScope]::Children, (New-Object System.Windows.Automation.PropertyCondition([System.Windows.Automation.AutomationElement]::ControlTypeProperty, [System.Windows.Automation.ControlType]::Window)))
                    foreach ($wnd in @(Dialogs $p.Id) + @($owned)) {
                        if ($wnd.Current.NativeWindowHandle -eq $main.Current.NativeWindowHandle) { continue }
                        $btns = $wnd.FindAll([System.Windows.Automation.TreeScope]::Descendants, (New-Object System.Windows.Automation.PropertyCondition([System.Windows.Automation.AutomationElement]::ControlTypeProperty, [System.Windows.Automation.ControlType]::Button)))
                        $names = ($btns | ForEach-Object { $_.Current.Name }) -join ','
                        foreach ($b in $btns) {
                            if ($b.Current.Name -like "*$v*") { "dialog $($wnd.Current.Name) [$names] -> $($b.Current.Name)"; $b.GetCurrentPattern([System.Windows.Automation.InvokePattern]::Pattern).Invoke(); $done = $true; break }
                        }
                        if ($done) { break }
                    }
                    if (-not $done) { Start-Sleep -Milliseconds 200 }
                }
                if (-not $done) { "no dialog button like $v" }
            }
            'exited' {
                $ok = $p.WaitForExit([int]([double]$v * 1000))
                "exited: $ok"
                if ($ok) { break }
            }
            'tab' {
                $t = $main.FindFirst([System.Windows.Automation.TreeScope]::Descendants, (New-Object System.Windows.Automation.PropertyCondition([System.Windows.Automation.AutomationElement]::NameProperty, $v)))
                if ($t) { $t.GetCurrentPattern([System.Windows.Automation.SelectionItemPattern]::Pattern).Select(); "tab $v" } else { "tab $v not found" }
            }
        }
    }
    "alive: $(-not $p.HasExited)"
} finally {
    if (-not $p.HasExited) { $p.Kill() }
}
