# NanoImpact → .NET Framework 4.8 포팅 계획

원본: `Downloads\NanoImpact_20230510\NanoImpact\source\NanoImpact2020cfe` (C++ MFC, v142, x86, 마지막 빌드 2025-06-20). 원본은 참고용으로 두고 고치지 않는다.
대상: 이 저장소 `NanoImpactNET` — C# 7.3, .NET Framework 4.8, WPF, **x86**.

## 1. 원칙

- **장비와 주고받는 바이트는 원본과 같게.** 송신 패킷·수신 파서는 원본 코드를 옮긴 C++ 기준 구현(`tools\golden`)과 출력이 같아야 한다.
- **설정 파일은 그대로 읽고 쓴다.** `*.INI` 3개는 텍스트가 아니라 x86 구조체를 그대로 덤프한 바이너리다. C# 구조체 배치를 MSVC x86과 바이트 단위로 맞춘다(크기·오프셋을 cl.exe로 뽑아 대조).
  - `CLIENTPUBLICDATA.INI` = `CLIENT_PUBLIC_DATA` (9,712 B, exe 폴더)
  - `USBINTEGRATEDDATA.INI` = `INTEGRATED_COMMAND_STRUCT` (28,432 B)
  - `SROTCALIBRATIONDATA.INI` = `SEM_SCAN_ROTATION_CALIBRATION_DATA` (132,480 B)
- **원본 버그는 고치되 기록한다** (아래 4절). 동작이 달라지는 수정은 장비 시험 때 확인한다.
- 화면은 MFC 대화상자를 1:1로 옮기지 않고 WPF로 다시 짜되, 기능·버튼 단위로 대응표를 유지한다.

## 2. 원본 구조 요약

| 영역 | 원본 | 규모 | 비고 |
|---|---|---|---|
| 장비 통신 | `USB_FT232H_Device.cpp` | 18.8k | FT232H 3개(ScanBri001 / ImgCap001 / BseICap001), `Afx*` 전역 함수 약 380곳에서 송신 |
| 메인 화면 | `NanoImpact2020cfeDlg.cpp` | 30.3k | 대부분 살아 있음. 영상 조립, 장비 데이터 처리, 자동 시작, Auto F/C/B(OpenCV), Spellman RS232 파싱 |
| 장비 파사드 | `ModularAssistDevice.cpp` | 3.4k | `SetSEM*` 약 60개, `UsbDeviceConnect/Disconnect` |
| 제어 패널 | `Modular*ControlDlg` | 2~8k씩 | Beam(=Spellman C-FE HV), Focus, Align, Scan, Vacuum, Stage, Detail, ImageCapture, Option(카메라) |
| 보정 도구 | Scan / Stage / SRotOrtho Calibration | 7.8k / 4.2k / 4.0k | 비밀번호 = 오늘 날짜 MMDD |
| 외부 연결 | `SocketProcess` (TCP 7321, Oxford EDS "MPO"), `CoreDlg_S_UDP` (UDP 4481 원격 저장/AutoBC), `CoreDlg_S_EDS` (UDP → 192.168.50.2:4482, HV/WD/Mag 1초 송신) | | |
| RS232 | `USB_RS232_Comm` | 0.5k | Spellman C-FE HV 전원, 115200 8N1, `:CC args CS\n` |
| 상수·구조체 | `DefineStructCode.h` | 5.4k | `#define` 3,010개, 구조체 61개 |

**포팅하지 않음 (죽은 코드):** `UDP\OpThreads.*`(1.7만 줄, 다른 프로젝트), `FEBeamManualControlDlg`/`FEBeamHVMonitorDlg`/`FEBeamAutoControl`, `ModularCfeHVPowerMonitorDlg_.*`, `ModularOptionDataToolDlg`, `ImageDisplayBoxDlg`, `CSemDataProcess::SetSemData` 경로, log4cxx(주석 처리됨).

### 시작 순서 (원본 timer 98)
1. ftd2xx 로드, 공용 데이터 생성, `CLIENTPUBLICDATA.INI` 읽기, Spellman COM 열기(115200)
2. FT232H 열기: Scan Bridge → SE 영상 → BSE 영상, 수신 스레드 시작
3. CNC 송신 스레드 → 2초 뒤 `RM0;` `RM1;` `RM2;` `RP0;` `RP1;` `RP2;` (축 최대 폭·현재 위치)
4. 진공 상태에 따라 Air→Evac 대기, Spellman 감시 시작, 조이스틱 타이머

### 종료 순서 (원본 OnClose)
HV 동작 중이면 거부 → 확인 → UDP/EDS 정지, 스레드 정지, RS232 닫기 → 영상 캡처 끔, 스캔·HV 끔, `USBINTEGRATEDDATA.INI` 저장 → 일반 종료는 `AfxScanBridgePowerOff`, 셧다운은 `SetSemSystemShutDownExecute(0)` → FT232H 닫기, `CLIENTPUBLICDATA.INI` 저장.

## 3. 단계

| # | 단계 | 내용 | 상태 |
|---|---|---|---|
| 1 | 장비 통신 하위 계층 | ftd2xx P/Invoke, `Ft232hPort`(열기 순서·송신 3종), Scan Bridge 수신 파서·수신 스레드 | **완료, 기준 구현과 일치** (장비 미시험) |
| 2 | 상수·구조체·설정 파일 | `DefineStructCode.h` → C# 상수/구조체 자동 변환, MSVC x86 크기·오프셋 대조, INI 3개 읽기·쓰기 왕복 시험 | **완료** — 상수 2,971개 전부, 저장 구조체 3개 배치 MSVC와 동일, INI 왕복 동일 |
| 3 | 장비 명령 계층 | `USB_FT232H_Device.cpp` 전체 → `SemDevice` (전역 변수 → 필드, `Afx*` 함수 → 같은 이름 메서드). 451개는 `NanoImpact.CodeGen translate`로 옮긴 뒤 손질, FT 계층 30개는 손으로 | **완료, 원본 대조 하네스와 일치** (장비 미시험, 스레드 본문은 실제 스레드로 미실행) |
| 4 | 영상 취득 | SE 읽기(장치 계층) → `OnSemSEScanImageData` 마커 해석 → 라이브·포토 4종 조립을 `SemScanImageProcessor`로 (원본 Dlg.cpp:13488-16148 번역 + 화면은 `IScanImageView`) | **완료, 원본 대조 일치** — 화면 그리기(`*UpdateControl`, 정보 띠·눈금), BSE 영상, 실제 스레드 연결은 7단계에서 |
| 5 | Spellman C-FE HV | RS232 포트(`USB_RS232_Comm.cpp` → `UsbRS232Comm`, Win32 함수는 `ISerialCommApi`, 실제는 `SerialPort`), 빔 창 송신·감시 타이머(`CfeHVPowerControl`), 메인 화면 LF 조립·응답 해석·모니터 창 램핑 상태(`CfeHVPowerMonitor`), 화면은 `ICfeHVPowerView` | **완료, 원본 대조 일치** (장비 미시험) — 빔 창 버튼 처리(`OnBnClickedBeamControl*`)는 8단계, 실제 타이머·스레드 연결은 6~7단계 |
| 6a | 장비 명령 묶음·실제 호스트 | `ModularAssistDevice.cpp` 전체 → `AssistDevice` (154개 번역 후 손질, 메인 화면 호출은 `IAssistOwner`), 공용 상태 `VACUUM_BEAM_ALIGN_FOCUS_DATA`, 실제 `DeviceHost`(UI 스레드 SendMessage/PostMessage, 실제 스레드) | **완료, 원본 대조 일치** (장비 미시험) |
| 6b | 메인 화면 장치 처리기 | 메인의 `OnSemControl*Data`/`OnSemHWSetup*`/`OnSemControlStage*`(장치 → 화면, `UM_USB_SBR_*`), `OnSetUsbDevice*`/`OnGetUsbDevice*`(화면 → 장치) 83개 약 6천 줄 → `MainDeviceService` (원본 메시지 맵 118항목), 패널 10개는 생성한 인터페이스, 범위 밖 화면 함수는 `IMainDeviceView` | **완료, 원본 대조 일치** (장비 미시험) — 패널 구현은 8단계, 화면 함수·메인 생성자 초기값은 7단계 |
| 7 | WPF 셸 | 상단 바, 영상 4칸, 오른쪽 탭(Scan/Focus/Beam/Align/Vacuum/Option), Stage/CFE-HV, 하단 Detail, 캡처 목록. 원본과 같은 영역·기능을 표준 컨트롤로 새로 배치(창 크기에 맞춤). 조립 루트·창 1번 분배·타이머·시작(단계 0~9)·종료 순서, 스캔 시작·비트맵 채우기 번역, `--sim` 시뮬레이터 | **완료** — 스캔 시작은 원본 대조 일치, 앱은 시뮬레이터로 시작·스캔 7종·종료 확인 (장비 미시험). 패널은 8단계 전까지 호출 기록용 임시 패널 |
| 8 | 제어 패널 | Vacuum → Beam → CFE 모니터 → Focus·Align·Scan → Stage(+Load/Unload) → Detail | |
| 9 | 자동 기능 | 자동 시작 상태 기계, Auto Focus/Contrast/Brightness/Gun Align (OpenCV → OpenCvSharp 또는 HW_AF.dll) | |
| 10 | 주변·보정 | 이미지 캡처·저장, 카메라(TVideoGrabber), Monogram 단축키, Environment, Stage/Scan/SRot 보정 | |
| 11 | 외부 연결 | EDS TCP 7321(인증 `8da17fy758`, 1바이트 정렬 구조체), UDP 4481, UDP EDS 송신 | |

각 단계는 장비 없이 확인할 수 있는 것(기준 구현 대조, 파일 왕복)을 먼저 하고, 장비 시험 항목을 따로 적는다.

## 4. 원본에서 찾은 버그와 처리

| 위치 | 내용 | 포팅에서 |
|---|---|---|
| `USB_FT232H_Device.cpp:4653` | 수신 명령 찾기 반복문 조건이 `n < 44`(읽은 바이트 위치). 한 번에 읽은 묶음에서 44번째 바이트 뒤에 끝나는 메시지는 처리되지 않음 | 고침 (모두 처리) — 장비에서 전에 무시되던 응답이 반영되는지 확인 필요 |
| 같은 파일 송신 함수들 | 전역 `m_nFT232HIndex`를 스레드끼리 같이 바꿈 | 포트마다 잠금 |
| `AfxFT232HPortOpen` | 설정 단계 실패 시 핸들을 연 채로 반환 | 실패하면 닫음 |
| `RompishImage::SaveBitmap` | BMP 끝에 44바이트 쓰레기, 버퍼 밖 읽기 | 표준 BMP 인코더 사용 |
| `CoreDlg_S_UDP` | 수신 1회에 CRLF 메시지 1개만, 종료 시 무한 대기 가능 | 고침 |
| 설정 파일 | 디버그 빌드 미초기화 영역(0xFE)이 그대로 저장됨 | 읽은 바이트를 보존해서 다시 씀 |
| `AfxIntegratedSetWDOLSrotZoomingLinearize` 등 | `nWDOLCoarse[nWD + 1]` 등 60칸 배열의 61번째를 읽음(다음 필드 값) | 원본과 같은 바이트를 읽음, 진단 기록 |
| `AfxScanBridgeDataRead` 명령 탐색 | 위 44바이트 버그에 더해, 명령 글자가 없는 메시지는 목록 배열 밖까지 훑다가 우연히 맞으면 공용 데이터의 명령 문자열을 덮어씀 | 고침 (무시) |
| CL1/CL2 DA 함수 (`AfxIntegratedSetCL1(nSpotSize, …)` 등) | 1부터인 nSpotSize에 0이 오면 전역 배열 [-1]을 읽고 씀 | C#은 예외 — 호출하는 쪽에서 범위 확인 필요 |
| FT 래퍼 (`AfxFT232HClose(n)` 등) | 장치 번호 범위 검사 없음 | C#은 예외 |
| `AfxScanBridgeVLCommand` | 수신한 밸브 번호·값으로 5칸 배열 밖(구조체 밖 힙)에 씀 | C#은 예외 (실제 밸브 번호 범위에서는 원본과 같음) |
| `AfxFT232HPortSendAvrCommand(port, cmd)` | `;`가 없는 명령이면 버퍼 밖을 읽다 죽음 | C#은 예외 |
| 종료 순서 | `AfxSEMSystemThreadClose` 뒤 `AfxIntegratedSetThreadAllStopFlag`를 부르면 해제된 스레드 인자 접근 | 원본 OnClose 순서를 지킨다 |
| `ExecuteCfeHVPowerAllDataMonitoring` (Spellman 일반 감시) | 순번이 `>= 28`에서 0으로 돌아가 28번 갈래(10바퀴마다 전체 상태 `50`)에 닿지 않음 → `50`을 한 번도 보내지 않음 | **원본 그대로** — 보내야 하는지 확인 필요 |
| `ExecuteCfeHVPowerHVProcessDataMonitoring` (HV 동작 중 감시) | 같은 이유로(`>= 17`) 17번 갈래(히터 램프율 `2A`)를 보내지 않음 | **원본 그대로** — 확인 필요 |
| `CfeHVPowerCommand01Process` | `(BYTE)x & 0x02 == 1` 은 C 우선순위로 `x & (0x02 == 1)` → 오류 비트 0(빔 에너지)만 보고 추출기·억제기·히터는 늘 "OK" | **원본 그대로** (화면 표시만 영향) — 확인 필요 |
| 빔 창 `m_pUsbRS232Comm` | 빔 창 초기화 때 메인 화면의 포트 포인터를 복사해 둠. 포트가 닫혀 받는 스레드가 클래스를 지운 뒤에도 감시 타이머가 지워진 객체로 보냄 | 고침 (늘 메인 화면의 현재 포트를 본다, 없으면 안 보냄) |
| `CommThread` 넘침 계산 | 받는 버퍼가 넘칠 때 `nInSize = (m_nLength + nInSize) - 8192` (빈 칸이 아니라 넘친 양) → 버퍼 밖에 쓸 수 있음. 두 번째 `ReadFile`도 남은 칸이 아니라 `nSize`만큼 읽음 | 고침 (빈 칸만큼) — 동기 처리에서는 도달하지 않음 |
| `OnCfeHVPowerUsbRS232ComPortReceiveData` | 4096바이트 넘게 쌓이면 `Receive`가 -1 → `szData[-1]`에 씀(그리고 쌓인 것이 영영 안 빠짐). LF 없는 조각이 4096바이트를 넘으면 줄 버퍼 뒤 멤버를 덮어씀 | 고침 (쓰지 않음 / 모아 둔 조각 버림) |
| `CfeHVPowerRS232ReceiveDataProcess` | `:` 뒤 256바이트 넘게 LF가 없으면 지역 버퍼 밖에 씀 | 고침 (넘는 바이트 버림) |
| `CfeHVPowerCommandDataAnalysis` `50` | ARG1 칸을 못 찾으면 `n1` 대신 `m`을 0으로 (n1은 이미 0이라 결과 같음) | 원본 그대로 |

## 5. 검증 도구

- `tools\golden\golden.cpp` — 원본 송신 패킷·수신 파서를 거의 그대로 옮긴 C++. `build.bat`로 MSVC x86 빌드.
- `tools\golden\make_vectors.ps1` — 고정 사례 + 시드 고정 무작위 입력(`vectors.txt`).
- `tools\NanoImpact.Verify` — `NanoImpact.Verify golden vectors.txt` 출력이 `golden.exe` 출력과 같아야 한다.

2026-11-09 결과: 738줄 동일 (CMD 208, AVR 212, 수신 MSG 83 / BAD 235).

### 상수·구조체 생성과 배치 대조

- `tools\NanoImpact.CodeGen` — 원본 `DefineStructCode.h`(CP949)를 읽어
  - `src\NanoImpact.Core\Generated\Defines.g.cs`: `#define` 전부를 `Def.이름` 상수로. 식(`WM_USER + 4000`, `RGB(...)`, 다른 상수 참조, 정수 나눗셈)은 C 규칙으로 미리 계산하고 원본 식은 주석으로 남긴다. 같은 이름 재정의 18건은 모두 같은 값.
  - `src\NanoImpact.Core\Generated\Records.g.cs`: 저장 구조체 3개. 바이트 배열을 그대로 들고 필드를 오프셋으로 읽고 쓴다(패딩·미초기화 바이트 보존). 이름은 원본 필드 이름 그대로.
  - `tools\layout_probe\probe_fields.inc`: MSVC 프로브용 필드 목록.
  - 원본 헤더가 바뀌면 생성기를 다시 돌리고 아래 두 검사를 다시 한다. 생성 파일은 직접 고치지 않는다.
- `tools\layout_probe\build.bat` — 원본 헤더를 cl.exe x86으로 컴파일해 `sizeof`/`offsetof`를 `out\msvc_layout.txt`로. `NanoImpact.Verify layout` 출력과 같아야 한다.
- `NanoImpact.Verify ini <폴더>` — 원본 INI 3개를 읽고 다시 쓴 바이트가 같은지, 필드 쓰기가 그 필드 바이트만 바꾸는지.

2026-11-09 결과: 배치 556줄 동일(크기 28,432 / 9,712 / 132,480 B), INI 3개 왕복 동일 (원본 `build\Out\Debug`의 파일).

### 장치 계층 (3단계)

- `tools\NanoImpact.CodeGen translate` — 원본 C++ 함수를 C# 초안으로 기계 변환(`->`, Win32 형식, `char[]` → `CharBuffer`, `NULL` 구분,
  `memset`, 기본 인자 등). 3단계에서 한 번 쓰고 결과(`Device\Port\*.cs`)는 손으로 고쳤으므로 **다시 돌리지 않는다**.
- C 런타임 흉내: `NanoImpact.Core.Native.CRuntime` (`strcpy/strcat/atoi/atof/_itoa/sprintf`, `<math.h>` 이름). `sprintf`의 `%f/%e/%g`는
  .NET Framework가 15자리에서 먼저 반올림하는 문제를 피해 정확한 이진값으로 반올림(UCRT와 같음).
- 실행 환경 분리: `IDeviceHost`(메시지·대화상자·Sleep·스레드), `IFtdi`(ftd2xx). 실제 앱용 WPF 구현은 6~7단계.
- 원본 대조: `tools\device_harness` (README 참고). 원본 .cpp를 그대로 컴파일한 실행 파일과 C#에 같은 시나리오를 넣어
  장비 쓰기 바이트·FT 설정·창 메시지·대화상자·Sleep·공용 데이터 변화를 호출 단위로 비교.

2026-11-09 결과: 시작 16 / 전수 464×3 / 수신 588 호출 모두 같음. 다른 것은 원본이 잘못된 인덱스로 배열 밖을 읽고 쓰는 경우뿐(위 4절).

### 영상 조립 (4단계)

- `src\NanoImpact.Hardware\Imaging\SemScanImageProcessor` — 원본 메인 화면 멤버를 같은 이름의 필드로 두고 6개 처리기를 번역(Imaging\Port, 다시 생성하지 않음).
  화면 쪽(정보 글자, 스캔 버튼, 비트맵, 히스토그램, 화면 갱신)은 `IScanImageView` 하나로 모았다. 상태 구조체는 `Core\Native\ScanControlPublicData.cs`.
- 원본은 같은 메모리를 `BYTE*`와 `RGBQUAD*`로 겹쳐 썼다(`m_p2ndBitmapBits` / `m_p2ndImageRgb`) → C#은 `RGBQUAD[]` 하나를 같이 가리킨다.
- 포토 평균 버퍼 B(2560×1920×120 = 590MB, 원본은 x86에서 한 번에 new)는 1MB 쪽을 처음 쓸 때 할당하는 `PagedBuffer`.
- 원본 대조: `tools\image_harness` (README 참고). live 767 / photo12 227 / photo345 137 / photo6 175 / overlap 61 블록 모두 같음.
  장치 쪽 SE 읽기는 `tools\device_harness` `se_read` 62 호출 같음.

### Spellman C-FE HV RS232 (5단계)

- `src\NanoImpact.Hardware\Spellman`
  - `UsbRS232Comm` — 원본 `CUsbRS232Comm`·`CommThread`를 손으로. Win32 직렬 함수는 `ISerialCommApi`(실제: `SerialCommApi` = `System.IO.Ports.SerialPort`),
    창 메시지·스레드는 `IDeviceHost`. 받는 스레드는 바이트를 쌓고 `UM_CFE_HV_POWER_CONTROL_RECEIVE_DATA`를 SendMessage, 포트가 닫히면 `..._PORT_CLOSE`.
  - `CfeHVPowerControl` (원본 빔 창, `Port\CfeHVPowerControl.L06771.cs`) — 감시 타이머 2개(일반 100 ms 순번 28칸 / HV 200 ms 17칸),
    `SendingCfeHVPower<코드>Command` 29개, 체크섬 `(0x200 - 합) & 0x7F | 0x40`, 목표값 한 번만 넣기.
  - `CfeHVPowerMonitor` (원본 메인 화면, `Port\CfeHVPowerMonitor.L*.cs`) — 포트 열기/닫기, LF까지 모으기, `:CC 인자 CS\n` 응답 35종 해석,
    모니터 창 램핑 상태(램핑 뒤 처음 목표 도달 → 포커스 디가우징).
  - 화면 쪽은 `ICfeHVPowerView` (글자·램프·스핀·받은 줄 목록·타이머·디가우징).
- 번역기: `NanoImpact2020cfeDlg.cpp`는 BOM 있는 UTF-8 파일에 CP949 주석과 UTF-8 주석이 섞여 있어 줄마다 판별해 읽는다.
  C 런타임에 `char*` 산술(`CharPtr`), `memcmp`, `strncpy(char*, char*)`, UCRT `atof`의 16진 실수·inf·nan·넘침을 더했다.
- 원본 대조: `tools\hv_harness` (README 참고). send 538 / rx 1854 / monitor 19 / ramp 16 / port 28 / smoke 11 블록 모두 같음.

### 장비 명령 묶음·실제 호스트 (6a)

- `src\NanoImpact.Hardware\Device\Assist\AssistDevice` — 원본 `CModularAssistDevice` 메서드 154개 (`Assist\AssistDevice.L*.cs`, 다시 생성하지 않음).
  원본 전역 `Afx*` 호출은 `m_dev.Afx*`(SemDevice), 메인 화면 호출(`OnSetUsbDeviceSEMControlDataToDeviceServer` 등 5개)은 `IAssistOwner` — 6b가 구현한다.
- `src\NanoImpact.Core\Native\VacuumBeamAlignFocusData.cs` — 원본 `VACUUM_BEAM_ALIGN_FOCUS_DATA` 필드 176개 (진공·빔·정렬·포커스 패널 공용).
- `src\NanoImpact.Hardware\Device\DeviceHost` — `IDeviceHost` 실제 구현. 창 번호마다 처리기를 붙이고(`RegisterWindow`), SendMessage는 UI 스레드에서
  실행해 끝날 때까지 기다리고, PostMessage는 UI 큐에, BeginThread는 배경 스레드(예외는 `ThreadFailed`로). UI 스레드가 장치 스레드를 기다리는 중에
  그 스레드가 SendMessage를 하면 서로 기다린다(원본 Win32도 같은 구조) — 6b·7단계에서 UI 쪽이 장치 스레드를 막고 기다리지 않게 한다.
- 원본 대조: `tools\device_harness`에 `ModularAssistDevice.cpp` 메서드를 그대로 붙이고(`assist_methods.inc`), 호출 표 `assist_dispatch.inc`
  (`NanoImpact.CodeGen dispatch-class`)로 `CALL A.이름(형식)`을 부른다. 메인 화면 호출은 요청 구조체 내용까지 `OWNER` 줄로 비교.
  assist1/2/7 각 188 호출, assist_modes 369 호출(스캔 모드·진공 상태·CL 모드·축 이동·Spellman 조합) 모두 같음.
  assist0의 차이 2건은 spot size 0 → CL 배열 [-1] (위 4절 CL1/CL2 항목과 같은 원본 버그).
- `NanoImpact.Verify hostcheck <INI 폴더>` — 실제 호스트와 실제 장치 스레드: 다른 스레드의 SendMessage가 UI 스레드에서 실행·반환, Post 순서,
  스레드 예외 보고, 원본 시작 순서로 연 장치의 Scan Bridge 수신 스레드가 가짜 FTDI 응답을 읽어 메인 창으로 `5026`을 보냄, 원본 OnClose 순서로 종료.

### 메인 화면 장치 처리기 (6b)

- `src\NanoImpact.Hardware\Device\Main\MainDeviceService` — 원본 메인 화면 처리기 83개(`Main\Port\*.cs`, 번역 후 손질, 다시 생성하지 않음).
  원본 멤버(`m_IntegratedData` 메인 화면 사본, `m_ClientPublicData`, `m_SemSEScanControlPublicData`, `m_VacuumBeamAlignFocusData` 등)를 같은 이름으로 들고,
  `WindowProc`(원본 메시지 맵 118항목, `MainDeviceService.MessageMap.g.cs`)으로 장치 계층의 창 메시지를 받으며, `IAssistOwner`로 ModularAssistDevice와 연결된다.
  생성자 초기값은 원본 메인 화면 생성자에서 이 처리기들이 쓰는 것만 (`m_bCLOnOffFlag = TRUE`, `m_strDistDecimalPlaces = "%6.2f"`).
- 패널: `NanoImpact.CodeGen panels`가 처리기 본문의 `m_wndXxx.Method(...)` 호출과 패널 헤더 선언으로 만든 인터페이스 10개(`Main\Panels.g.cs`, 메서드 118개,
  글자 컨트롤 2개) — 8단계 실제 패널이 구현한다. 원본 대조용 기록 클래스(`Verify\PanelRecorders.g.cs`, C++ `device_harness\panels.inc`)도 같이 생성.
- 범위 밖 메인 화면 함수(정보 띠 갱신, 진공 상태 표시, 스캔 시작, 자동 기능 실행, 타이머 등 19개)는 `IMainDeviceView` — 7·9단계.
- 처리기 지역 구조체 28종은 `NanoImpact.CodeGen structs`로 `Core\Native\ControlData.g.cs`에 생성 (`CopyFrom` = 원본 memcpy).
- 원본 대조: `tools\device_harness`에 원본 처리기 5구간을 그대로 붙이고 메인 창(hWnd 1) 메시지를 원본 메시지 맵대로 처리기에 넘긴다 →
  장비 응답(rx) → 처리기 → 패널, 패널 명령(assist) → 처리기 → 장비 쓰기가 끝에서 끝까지 비교된다.
  main0/1/2 각 2,404~2,411 호출(메시지 118종 × 처리기가 비교하는 요청 번호·옵션·데이터값·상태값), main_auto 340 호출(자동 기능 결과 × 이전 스캔 모드),
  기존 rx/assist/sweep 시나리오 모두 같음. 패널 메서드 118개와 컨트롤 2개가 모두 한 번 이상 불렸다.

### WPF 셸 (7단계)

- `src\NanoImpact.App` (net48 WPF x86, 실행 파일 `NanoImpact.exe`) — 원본 2560×1440 비트맵 스킨 대신 같은 영역을 표준 컨트롤로 새로 배치했다.
  상단 막대(진공 상태, START/STANDBY, AIR/EVAC, GUN HV·배율 콤보, 자동 기능 버튼, FE HV·추출기 전류), 1st 라이브 / 2nd 포토 / 3rd 카메라 /
  4th 히스토그램, 오른쪽 제어 탭(SCAN·FOCUS·BEAM·ALIGN·VACUUM·OPTION), STAGE·CFE-HV·NAVI, 아래 DETAIL·캡처 목록·패널 호출·기록. 칸 사이는 끌어서 크기 조절.
- 조립 루트 `Composition\MainShell` = 원본 `CNanoImpact2020cfeDlg`의 뼈대:
  - `DeviceHost`(Dispatcher 문맥) → `SemDevice`(실제 `Ftd2xxApi` 또는 `--sim`의 `SimFtdi`) → `AssistDevice` → `MainDeviceService`(임시 패널) →
    `SemScanImageProcessor`(SE 스캔 공용 상태를 장치 처리기와 함께 씀) → `CfeHVPowerMonitor`(`SerialCommApi`).
  - 창 1번 처리기: `UM_SEM_SE_SCAN_IMAGE_DATA` → 영상 조립, `UM_CFE_HV_POWER_CONTROL_*` → Spellman, 자동 기능 영상 5종은 9단계, 나머지는 원본 메시지 맵(`MainDeviceService.Dispatch`).
    창 3·4번(Scan·Stage 보정 창)은 8단계 전까지 기록만.
  - 타이머: 창마다 `TimerTable`(DispatcherTimer, 원본 SetTimer/KillTimer 의미). 메인 `OnTimer`는 영상 표시(10/11/20/21/22), 배율 가속(40), 진공 깜빡임(90),
    시작(98), 종료(99)를 원본대로. Beam 창 Spellman 감시 타이머는 `CfeHVPowerControl.OnTimer`로.
  - 시작: 원본 `OnInitDialog`의 장치 쪽(DLL, 공용 데이터, `CLIENTPUBLICDATA.INI`, Spellman COM 115200) → 타이머 98 단계 0 `DialogStart`(장치 서버 경로·소유 창·
    `UsbDeviceConnect`) → 1 CNC 송신 스레드 → 2초 뒤 2~7 `RM0;`~`RP2;` → 8 Air→Evac 대기 상태 해제 → 9 Spellman 감시 시작·조이스틱 타이머.
  - 종료: 원본 `OnClose` — HV 동작 중이면 거부, 확인 → 스레드 정지 표시·RS232 닫기 → 500 ms 뒤 캡처 끔·장치 기본값·`USBINTEGRATEDDATA.INI` 저장·
    Scan Bridge 끔 → `OnDestroy`(USB 끊기, `CLIENTPUBLICDATA.INI` 저장, 공용 데이터 해제). 시스템 끄기(99) 갈래는 8단계 확인 창과 함께.
- 스캔 시작·비트맵 채우기 번역: `Imaging\Port\SemScanImageProcessor.L09564.cs`(스캔 시작 2개), `.L07342.cs`(RED 상자), `.L07791.cs`(Fill* 10개).
  장치 쪽은 `IScanDevice`(앱: `ScanDeviceAdapter` = AssistDevice + Afx CL + 메인 `m_bCLOnOffFlag`), 화면 쪽은 `IScanImageView`에 7개 추가
  (메모리 DC 칠하기, RED 상자, 레이어 다시 그리기, 타이머, Scan 보정 창, 현재 CL).
  원본 대조 `tools\image_harness` scan 572 블록 같음, 기존 5개 시나리오도 그대로 같음.
- 화면 그리기: 처리기의 `RGBQUAD[]`를 `WriteableBitmap`(Bgr32)으로. 원본은 SE 데이터를 받을 때마다 1st 영상을 다시 그리므로(약 100회/s)
  화면 갱신 주기마다 마지막 요청 한 번만 옮긴다. 포토는 `m_2ndImage*Bitmap`(원본 `SetBitmapBits` 대상)을 그린다.
  정보 띠의 눈금자·글꼴 그림과 측정 레이어는 10단계, 정보 글자(`SetClientInfomation0N`)는 DETAIL 칸에.
- 임시 패널: `NanoImpact.CodeGen panels`의 8번째 인자로 `Placeholders\PanelStubs.g.cs`를 같이 만든다(인터페이스 10개 구현, 부른 내용을 PANEL CALLS 칸에).
  SCAN 탭의 RED~PHOTO6 버튼은 원본 `CModularScanControlDlg::OnBnClickedScanControl*Btn`과 같은 요청(`OnSetSemSEScanControlDataUpdate`)을 만든다 — 8단계에서 실제 Scan 패널로 바꾼다.
- 실행: `NanoImpact.exe [--sim] [--ini <INI 폴더>]` (기본 INI 폴더 = 실행 파일 폴더, 원본 `GetExecutedPath`).
  `--sim`: `SimFtdi`가 FT232H 3개를 연 것처럼 하고, 캡처가 켜지면 `SimScanSource`가 원본 SE 형식(FF 밝기 대비, 줄 사이 00, 화소 01~FE)으로 합성 영상을 넣는다 →
  **실제 SE 수신 스레드 → 창 메시지 → 영상 조립 → 화면**이 그대로 돈다. 프레임 시간은 줄였다(RED 33 ms, FAST 66 ms, SLOW1 1.6 s, SLOW2 3 s, PHOTO 2~10 s). RS232는 열지 않는다.
- 2026-11-09 시뮬레이터 확인(`tools\app_smoke\drive_app.ps1`: UI Automation으로 버튼을 눌러 화면 저장): 시작 단계 0→9, FAST·RED(상자·바탕)·SLOW1 라이브, PHOTO1·4·6 완성과 히스토그램,
  종료 확인 창 아니오 → 계속, 예 → 스레드 정지·INI 저장 후 프로세스 스스로 끝남. 기록 칸에 스레드 예외·처리기 없는 메시지 없음.
- 장비 시험 항목: 실제 FT232H 열기 순서와 SE 영상 속도, Spellman COM 포트, 종료 때 Scan Bridge 끔, 다른 스레드 SendMessage와 UI 대기 사이의 교착 여부.
