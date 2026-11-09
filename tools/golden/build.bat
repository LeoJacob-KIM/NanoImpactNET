@echo off
rem 원본 프로토콜 기준 구현(golden.cpp)을 MSVC x86으로 빌드하고 vectors.txt 결과를 out\golden.txt로 남긴다.
setlocal
call "C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\VC\Auxiliary\Build\vcvars32.bat" >nul || exit /b 1
cd /d "%~dp0"
if not exist out mkdir out
cl /nologo /EHsc /O2 /Fo:out\ /Fe:out\golden.exe golden.cpp >out\cl.log || (type out\cl.log & exit /b 1)
out\golden.exe vectors.txt > out\golden.txt
