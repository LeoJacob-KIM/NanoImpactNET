@echo off
rem Build the device harness: the ORIGINAL USB_FT232H_Device.cpp (copied unmodified) + shim headers + fake Ftd2xx.dll.
rem Arg 1: original source folder. Output: out\harness.exe, out\Ftd2xx.dll. Keep this file ASCII.
setlocal
set SRC=%~1
if "%SRC%"=="" set SRC=C:\Users\Administrator\Downloads\NanoImpact_20230510\NanoImpact\source\NanoImpact2020cfe
call "C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\VC\Auxiliary\Build\vcvars32.bat" >nul 2>nul
cd /d "%~dp0"
if not exist out\src mkdir out\src
for %%F in (USB_FT232H_Device.cpp USB_FT232H_Device.h DefineStructCode.h ftd2xx.h) do copy /y "%SRC%\%%F" out\src\ >nul || exit /b 1
copy /y shim\*.h out\src\ >nul
set CODEGEN=..\NanoImpact.CodeGen\bin\Debug\NanoImpact.CodeGen.exe
"%CODEGEN%" dispatch "%SRC%\USB_FT232H_Device.h" out\src\dispatch.inc > out\dispatch.log || (type out\dispatch.log & exit /b 1)
set FLAGS=/nologo /EHsc /fp:precise /DUNICODE /D_UNICODE /source-charset:.949 /wd4996 /wd4244 /wd4267 /wd4018 /wd4101 /wd4305 /wd4838 /wd4309
cl %FLAGS% /LD /I out\src /Fo:out\ /Fe:out\Ftd2xx.dll fake_ftd2xx.cpp /link /DEF:fake_ftd2xx.def >out\cl_fake.log 2>&1 || (type out\cl_fake.log & exit /b 1)
cl %FLAGS% /I out\src /Fo:out\ /Fe:out\harness.exe harness.cpp out\src\USB_FT232H_Device.cpp user32.lib ws2_32.lib >out\cl_harness.log 2>&1 || (type out\cl_harness.log & exit /b 1)
echo built out\harness.exe
