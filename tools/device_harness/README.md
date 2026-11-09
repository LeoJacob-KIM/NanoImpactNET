# 장치 계층 원본 대조 하네스

원본 `USB_FT232H_Device.cpp`(장비 통신·제어, 21,627줄)를 **고치지 않고 그대로** 컴파일해서 시나리오를 돌리고,
C# `SemDevice`(src\NanoImpact.Hardware\Device)에 같은 시나리오를 돌려 출력을 호출 단위로 비교한다.

## 구성

| 파일 | 역할 |
|---|---|
| `build.bat` | 원본 .cpp/.h를 `out\src`로 복사, 대체 헤더(`shim\`)와 함께 MSVC x86 빌드 → `out\harness.exe`, 가짜 `out\Ftd2xx.dll` |
| `shim\stdAfx.h` | MFC 없이 컴파일하기 위한 최소 대체(CString, CFile, AfxBeginThread). `MessageBox/SendMessage/PostMessage/Sleep/GetTickCount`를 기록 함수로 돌린다 |
| `fake_ftd2xx.cpp` | 원본이 `LoadLibrary("Ftd2xx.dll")`로 부르는 FT_* 18개. 쓰기·설정을 기록하고 `RX`로 넣은 바이트를 돌려준다 |
| `harness.cpp` | 스크립트 실행기. 호출 표 `out\src\dispatch.inc`는 `NanoImpact.CodeGen dispatch`가 헤더에서 만든다 (450개 함수) |
| `run.ps1` | 원본·C# 양쪽 실행(매번 `out\ini`에 원본 INI 복사본) → 호출 단위 비교 |
| `make_sweep.ps1`, `make_rx.ps1` | 전수 시나리오(모든 함수 1회), 수신 처리기 시나리오(명령 44종 × 13가지) 생성 |

## 출력 형식 (양쪽 같음)

```
CALL <키> <인자...>      W <포트> <바이트...>        장비로 쓴 바이트
C <포트> FT_<이름> ...   FT 설정 호출                MSG/POST <창> <메시지> <wParam> <lParam|P>
BOX <제목> | <내용>      SLEEP <ms>   THREAD         RET <반환값>
S <오프셋> <바이트...>   INTEGRATED_COMMAND_STRUCT 중 바뀐 바이트 (R = 스캔 회전 보정 데이터)
```

## 쓰는 법

```bat
build.bat
powershell -File make_sweep.ps1 -Value 1 -Name sweep1
powershell -File run.ps1 scripts\sweep1.txt
```

`run.ps1`은 "같음 / 다름 / C# 예외" 수와 첫 차이들을 보여 준다. 원본 코드가 배열 범위 밖을 읽고 쓰는 곳은 C#도 같은 바이트를 접근하고
`out\<이름>.out_of_range.txt`에 따로 적는다(비교 출력에는 넣지 않는다).

## 2026-11-09 결과

| 시나리오 | 호출 | 같음 | 비고 |
|---|---|---|---|
| startup | 16 | 16 | 원본 앱 시작 순서 |
| sweep1 / sweep2 / sweep7 | 464 × 3 | 전부 | 모든 함수 1회, 인자 1·2·7 |
| sweep0 | 464 | 457 | 1부터 시작하는 인덱스에 0 → 원본이 전역 배열 [-1]을 읽고 씀 (C# 예외 6, 그 여파 1) |
| sweep20 | 464 | 449 | 장치 번호 20 → 원본이 핸들 배열 밖을 읽음 (C# 예외 15) |
| rx | 588 | 588 | Scan Bridge 수신 명령 44종 |

## 주의

- 스크립트는 하네스 두 개가 같이 읽는다. 문자열 인자는 `;`로 끝내야 한다(원본 `AfxFT232HPortSendAvrCommand(port, cmd)`는 `;`가 없으면 버퍼 밖을 읽다 죽는다).
- 종료 함수는 원본 OnClose 순서로: `AfxIntegratedSetThreadAllStopFlag` → … → `AfxSEMSystemThreadClose`. 거꾸로 부르면 원본이 해제된 스레드 인자를 건드려 죽는다.
- 하네스에서는 스레드를 시작하지 않는다(`THREAD`만 기록). 스레드 본문(수신·CNC 송신·SE 영상·디가우징)은 시나리오에서 함수를 직접 불러 확인한다.
