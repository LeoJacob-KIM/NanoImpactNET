@echo off
rem Build the image harness: original SE scan image assembly (NanoImpact2020cfeDlg.cpp:13488-16148), InsertSort and
rem HistogramNormalize copied unmodified, compiled against stub dialog classes. Keep this file ASCII.
setlocal
set SRC=%~1
if "%SRC%"=="" set SRC=C:\Users\Administrator\Downloads\NanoImpact_20230510\NanoImpact\source\NanoImpact2020cfe
call "C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\VC\Auxiliary\Build\vcvars32.bat" >nul 2>nul
cd /d "%~dp0"
if not exist out\src mkdir out\src
for %%F in (DefineStructCode.h ftd2xx.h) do copy /y "%SRC%\%%F" out\src\ >nul || exit /b 1
copy /y ..\device_harness\shim\*.h out\src\ >nul
copy /y scan_fields.inc out\src\ >nul
set PS=powershell -NoProfile -ExecutionPolicy Bypass -File extract.ps1
%PS% "%SRC%\NanoImpact2020cfeDlg.cpp" out\src\image_methods.inc 13488 16148 || exit /b 1
rem step 7: scan start (SemScanModeSelectAndScanStart, SemSlow12...) and bitmap fills (Fill1stImageBitmap ... Fill2ndImageInfoBitmap)
%PS% "%SRC%\NanoImpact2020cfeDlg.cpp" out\src\scan_start.inc 9564 10183 || exit /b 1
%PS% "%SRC%\NanoImpact2020cfeDlg.cpp" out\src\fill_bitmaps.inc 7791 8090 || exit /b 1
%PS% "%SRC%\ModularAssistDevice.cpp" out\src\insert_sort.inc 3915 || exit /b 1
%PS% "%SRC%\PublicToolClass.cpp" out\src\histogram.inc 1592 || exit /b 1
set FLAGS=/nologo /EHsc /fp:precise /DUNICODE /D_UNICODE /source-charset:.949 /wd4996 /wd4244 /wd4267 /wd4018 /wd4101 /wd4305 /wd4838 /wd4309 /wd5033
cl %FLAGS% /I out\src /Fo:out\ /Fe:out\harness.exe harness.cpp user32.lib /link /LARGEADDRESSAWARE >out\cl_harness.log 2>&1 || (type out\cl_harness.log & exit /b 1)
echo built out\harness.exe
