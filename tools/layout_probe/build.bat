@echo off
rem Build probe.cpp (MSVC x86) against the original header and write out\msvc_layout.txt.
rem Arg 1: original source folder (DefineStructCode.h, ftd2xx.h). Keep this file ASCII: cmd reads it as CP949.
setlocal
set SRC=%~1
if "%SRC%"=="" set SRC=C:\Users\Administrator\Downloads\NanoImpact_20230510\NanoImpact\source\NanoImpact2020cfe
call "C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\VC\Auxiliary\Build\vcvars32.bat" >nul 2>nul
cd /d "%~dp0"
if not exist out mkdir out
cl /nologo /EHsc /source-charset:.949 /I"%SRC%" /Fo:out\ /Fe:out\probe.exe probe.cpp ws2_32.lib >out\cl.log 2>&1 || (type out\cl.log & exit /b 1)
out\probe.exe > out\msvc_layout.txt
