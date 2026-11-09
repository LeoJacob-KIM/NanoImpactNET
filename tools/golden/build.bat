@echo off
rem Build golden.cpp (protocol reference) with MSVC x86 and write out\golden.txt for vectors.txt.
rem Keep this file ASCII: cmd reads it as CP949.
setlocal
call "C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\VC\Auxiliary\Build\vcvars32.bat" >nul 2>nul
cd /d "%~dp0"
if not exist out mkdir out
cl /nologo /EHsc /O2 /utf-8 /Fo:out\ /Fe:out\golden.exe golden.cpp >out\cl.log 2>&1 || (type out\cl.log & exit /b 1)
out\golden.exe vectors.txt > out\golden.txt
