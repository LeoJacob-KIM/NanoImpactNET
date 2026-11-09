# 프로토콜 기준 구현 대조

```bat
powershell -File make_vectors.ps1
build.bat
..\NanoImpact.Verify\bin\Debug\NanoImpact.Verify.exe golden vectors.txt > out\csharp.txt
fc out\golden.txt out\csharp.txt
```

`fc`가 차이 없음을 보고해야 한다. `golden.cpp`를 고칠 때는 원본 `USB_FT232H_Device.cpp`의 해당 함수와 나란히 두고 확인한다.
