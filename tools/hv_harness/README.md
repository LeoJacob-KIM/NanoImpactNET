# Spellman C-FE HV (RS232) 원본 대조 하네스

원본 Spellman C-FE HV 전원 RS232 코드를 **고치지 않고 그대로** 컴파일해서 시나리오를 돌리고, C# `src\NanoImpact.Hardware\Spellman`
(`UsbRS232Comm`, `CfeHVPowerControl`, `CfeHVPowerMonitor`)에 같은 시나리오를 넣어 블록(`> 명령`) 단위로 비교한다.

| 원본 | 범위 | 하는 일 |
|---|---|---|
| `USB_RS232_Comm.cpp` | 파일 전체 (그대로 컴파일) | COM 포트 열기·설정, 받는 스레드 `CommThread`, `Send`/`Receive` |
| `ModularBeamControlDlg.cpp` | 6771-8845, 8848-8932 | 감시 타이머, `SendingCfeHVPower*Command` 29개, 체크섬, 목표값 한 번만 넣기 |
| `NanoImpact2020cfeDlg.cpp` | 1473-1495, 2798-2805, 22540-22612, 29789-, 29831-31999, 32100-32112 | 포트 열기/닫기, 받은 바이트 LF까지 모으기, 응답 35종 해석, 펌웨어 버전 버튼 |
| `ModularCfeHVPowerMonitorDlg.cpp` | 1664-1733 | 빔 전압 램핑·목표 도달 상태, 포커스 디가우징 |

## 구성

| 파일 | 역할 |
|---|---|
| `build.bat` | 원본을 `out\src`로 복사·발췌(`..\image_harness\extract.ps1`, 바이트 그대로, 원본에 BOM이 있으면 BOM 유지), MSVC x86 빌드 → `out\harness.exe` |
| `shim\pch.h` | `..\device_harness\shim` 위에 `CCmdTarget`/`CEvent`/`CSingleLock` 대체와 가짜 Win32 직렬 함수(`CreateFile`, `WaitCommEvent`, `ReadFile`, `WriteFile` …) |
| `harness.cpp` | 대체 대화상자(원본 멤버 이름), 가짜 직렬 포트, 스크립트 실행기 |
| `run.ps1` | 원본과 `NanoImpact.Verify hv`를 같은 스크립트로 돌려 비교 |
| `make_send.ps1`, `make_rx.ps1` | 송신 전수(명령 29개 × 인자), 수신 전수(응답 코드 38개 × 인자 모양 52가지, 메시지 중간에서 끊은 조각) 시나리오 생성 |

가짜 직렬 포트: `RX`로 넣은 조각을 `WaitCommEvent`가 하나씩 내주고, 다 쓰면(또는 `SetCommMask(0)` 뒤) 실패를 돌려준다 →
받는 스레드가 끝나며 핸들을 닫고 클래스를 지운 뒤 `UM_CFE_HV_POWER_CONTROL_PORT_CLOSE`(wParam 0, 오류 995)를 보낸다. 그래서 `RUN` 한 번마다 포트가 닫힌다.

## 스크립트 명령

`OPEN com`(메인 화면이 COM 열기), `FAILOPEN`(다음 열기 실패), `CLOSE`, `INIT`(빔 창 초기화 끝), `START`(감시 시작), `MODE n`(0 일반 / 1 HV),
`TIMER id 횟수`(2002 일반 / 2003 HV), `SEND 코드 [인자…]`, `FW 0|1`(펌웨어 버전 버튼), `CHECK n`(받은 줄 보기 체크),
`RX 글자`(`\n \r \t \\ \xHH`, "RX " 뒤 전부가 데이터), `RUN`(받는 스레드 본문 실행).

## 출력

`W 바이트…`(포트로 보낸 것), `C 함수 인자`(포트 설정·핸들), `MSG 창 메시지 wParam lParam`, `BOX`, `SLEEP`, `THREAD`,
`UI 대상 글자`(모니터 창·빔 창·상단 바), `IMG 대상 번호 켜짐`(램프), `SPIN`, `LIST INSERT/SET`, `TIMER SET/KILL`, `DEGAUSS ms`, `F 필드=값`(바뀐 상태).
화면 글자의 제어 문자는 `\xHH`로 찍는다.

## 쓰는 법

```bat
build.bat
powershell -File make_rx.ps1
powershell -File run.ps1 scripts\rx.txt
```

## 2026-11-09 결과

| 시나리오 | 블록 | 같음 | 확인한 것 |
|---|---|---|---|
| send | 538 | 538 | 명령 29개 × 선택값 -1~11, 글자 값(범위 경계, 반올림 경계, `0x10`·`0x1.8p3`·`inf`·`nan`·`1e400`·`abc`), 포트 없을 때 |
| rx | 1854 | 1854 | 응답 코드 38개 × 인자 모양 52가지 + 이상한 줄 8가지(앞 잡음, CRLF, NUL, `::`), 1~200바이트 조각 |
| monitor | 19 | 19 | 초기화 전·포트 없을 때, 일반 340틱(12바퀴), HV 40틱, 모드 전환 |
| ramp | 16 | 16 | 램핑 뒤 첫 목표 도달에서만 디가우징(3회), 목표값 편집 칸은 처음 한 번만 |
| port | 28 | 28 | 열기 실패 대화상자, 닫기 후 스레드 종료 순서, 펌웨어 버전, LF 없는 조각 |
| smoke | 11 | 11 | 짧은 예 |

원본과 다르게 고친 것(빔 창이 지워진 포트를 계속 쓰는 문제 등)은 `doc\PortingPlan.md` 4절. 하네스는 그 경우를 시나리오에 넣지 않는다.
