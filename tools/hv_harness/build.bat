@echo off
rem Build the Spellman C-FE HV harness: USB_RS232_Comm.cpp compiled as is, the send/receive code of the beam, main and
rem monitor dialogs copied unmodified by ..\image_harness\extract.ps1, stub dialogs and fake serial port in harness.cpp.
rem Keep this file ASCII.
setlocal
set SRC=%~1
if "%SRC%"=="" set SRC=C:\Users\Administrator\Downloads\NanoImpact_20230510\NanoImpact\source\NanoImpact2020cfe
call "C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\VC\Auxiliary\Build\vcvars32.bat" >nul 2>nul
cd /d "%~dp0"
if not exist out\src mkdir out\src
for %%F in (DefineStructCode.h ftd2xx.h USB_RS232_Comm.h USB_RS232_Comm.cpp) do copy /y "%SRC%\%%F" out\src\ >nul || exit /b 1
copy /y ..\device_harness\shim\*.h out\src\ >nul
copy /y shim\pch.h out\src\ >nul
set PS=powershell -NoProfile -ExecutionPolicy Bypass -File ..\image_harness\extract.ps1
set BEAM=%SRC%\ModularBeamControlDlg.cpp
set MAIN=%SRC%\NanoImpact2020cfeDlg.cpp
%PS% "%BEAM%" out\src\beam_methods.inc 6771 8845 || exit /b 1
%PS% "%BEAM%" out\src\beam_target.inc 8848 8932 || exit /b 1
%PS% "%MAIN%" out\src\port_open.inc 1473 1495 || exit /b 1
%PS% "%MAIN%" out\src\port_close.inc 2798 2805 || exit /b 1
%PS% "%MAIN%" out\src\main_port.inc 22540 22612 || exit /b 1
%PS% "%MAIN%" out\src\main_comport.inc 29789 || exit /b 1
%PS% "%MAIN%" out\src\main_receive.inc 29831 31999 || exit /b 1
%PS% "%MAIN%" out\src\main_firmware.inc 32100 32112 || exit /b 1
%PS% "%SRC%\ModularCfeHVPowerMonitorDlg.cpp" out\src\monitor_ramping.inc 1664 1733 || exit /b 1
set FLAGS=/nologo /EHsc /fp:precise /DUNICODE /D_UNICODE /source-charset:.949 /wd4996 /wd4244 /wd4267 /wd4018 /wd4101 /wd4305 /wd4838 /wd4309 /wd4554 /wd4840
cl %FLAGS% /I out\src /Fo:out\ /Fe:out\harness.exe harness.cpp out\src\USB_RS232_Comm.cpp user32.lib shlwapi.lib >out\cl_harness.log 2>&1 || (type out\cl_harness.log & exit /b 1)
echo built out\harness.exe
