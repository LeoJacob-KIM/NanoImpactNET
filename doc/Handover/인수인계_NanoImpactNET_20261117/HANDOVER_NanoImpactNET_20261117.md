# NanoImpactNET 인수인계 문서

작성 2026-11-17 · 대상: NanoImpact(C++ MFC) → .NET Framework 4.8 포팅 · 작성 사유: Claude ID 변경, 다른 Claude 세션이 이어서 작업

이 문서는 NanoImpactNET 포팅 작업만 따로 정리한 것이다. 다른 프로젝트(SEM Inspection SW, sem_proj, 이온 밀러 등)는
`D:\01. Code\16.SEM\07. Test\인수인계_20261117\HANDOVER_20261117.md`에 있다.

**새 Claude 세션 시작 문구:**
"D:\01. Code\16.SEM\07. Test\인수인계_NanoImpactNET_20261117\HANDOVER_NanoImpactNET_20261117.md를 읽고, 이어서 NanoImpactNET 8단계를 진행해줘."

---

## 1. 한눈에 보기

| 항목 | 내용 |
|---|---|
| 작업 저장소 | `D:\01. Code\16.SEM\07. Test\NanoImpactNET` |
| 원격 | https://github.com/LeoJacob-KIM/NanoImpactNET (private), 브랜치 `main` |
| 마지막 커밋 | `d59098a` "도구 기본 원본 경로를 새 위치로" (push 완료, 작업 트리 깨끗함). 7단계 본 커밋은 `15dba69` |
| 원본 소스 (참고용, 수정 금지) | `D:\01. Code\16.SEM\07. Test\Code_backup\NanoImpact_20230510\NanoImpact\source\NanoImpact2020cfe` |
| 원본 설정 파일(INI 3개) | `...\Code_backup\NanoImpact_20230510\NanoImpact\build\Out\Debug` |
| 계획서 | 저장소 `doc\PortingPlan.md` — 11단계 표, 원본 버그 표(4절), 검증 도구 설명(5절). **이어서 할 때 이 문서와 PortingPlan부터 읽는다.** |
| 진행 | 11단계 중 **7단계까지 완료**. 모두 원본 대조 하네스와 일치. **실제 장비 시험은 하나도 안 함.** |
| 다음 작업 | **8단계: 제어 패널** (Vacuum → Beam → CFE 모니터 → Focus·Align·Scan → Stage(+Load/Unload) → Detail) |

원본이 무엇인가: 자체 CFE FE-SEM 운영 SW `NanoImpact2020cfe` — 네이티브 C++ MFC 약 16만 줄. FTDI FT232H 3개(Scan Bridge `ScanBri001`,
SE 영상 `ImgCap001`, BSE 영상 `BseICap001`)를 직접 제어하고, Spellman C-FE HV 전원을 RS232로, Oxford EDS를 TCP로 다룬다.
사용자가 **C# 전면 포팅**(C++ DLL + C# UI 하이브리드가 아니라)을 선택했다.

---

## 2. 사용자와 일하는 방식 (꼭 지킬 것)

- 답변은 **한국어**. 코드 식별자·경로·로그는 그대로.
- 진행은 **단계 단위**: 사용자가 "N단계 시작"이라고 하면 그 단계를 끝까지 하고(구현 → 원본 대조 → 문서 갱신) 보고한다.
  6단계처럼 크면 6a/6b로 나눠서 각각 보고했다.
- **커밋·push는 사용자가 "커밋·push"라고 할 때만.** 단계를 끝내면 "커밋·push할까요?"로 묻고 기다린다.
  - 커밋 메시지는 한국어, 첫 줄 "N단계: 요약", 본문에 바뀐 것 목록. 맨 끝에 Claude 공동 작성자 줄(그 세션의 지시에 따름).
  - 여러 줄 메시지는 bash heredoc(`git commit -F -`)이나 파일로. PowerShell 5.1 인자로 넘기면 깨질 수 있다.
  - GitHub 인증은 GCM에 캐시되어 있어 `git push origin main`이 바로 된다.
- **실제 장비는 사용자 확인 없이 건드리지 않는다.** 특히 `--sim` 없이 앱을 실행하지 않는다 — Spellman용 COM 포트를 열고 명령을 보낸다.
  이 PC의 COM4/5/6/10은 무슨 장비인지 모른다. ftd2xx로 FT232H를 여는 것도 마찬가지.
- 원본 폴더(`Code_backup\...`)는 읽기만. 고치지 않는다.
- 보고는 "무엇을 했고, 무엇으로 확인했고(숫자), 무엇이 남았는지"를 짧게. 결정할 것이 있으면 선택지와 추천을 같이.

---

## 3. 환경과 경로

### 3.1 빌드
- SDK-style csproj, `TargetFramework net48`, **x86 고정**, C# 7.3 (`Directory.Build.props`). 원본 INI가 x86 구조체 덤프라 배치를 맞추기 위함.
- 빌드는 VS2019 MSBuild:
  ```
  "C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe" NanoImpactNET.sln -nologo -v:q -nodeReuse:false -p:UseSharedCompilation=false
  ```
  WPF 앱(`src\NanoImpact.App`)은 처음 한 번 `-restore`가 필요할 수 있다. .NET 5 SDK가 설치되어 있어 net48 WPF SDK-style 빌드가 된다.
- C++ 하네스는 MSVC x86(`vcvars32.bat`)으로 각 `tools\*\build.bat`가 빌드. PowerShell에서는 `cmd /c "<전체 경로>\build.bat"` (상대 경로는 실패).
- 이 PC에 MFC 헤더는 없다(원본 자체 빌드 불가). 그래서 하네스는 MFC 대신 `tools\device_harness\shim\*.h` 대체 헤더로 원본 .cpp를 그대로 컴파일한다.

### 3.2 도구 기본 경로 (2026-11-17 정리 완료)
2026-11에 폴더가 `C:\Users\Administrator\Downloads\...`에서 `D:\01. Code\16.SEM\07. Test\...`로 옮겨졌다.
커밋 `d59098a`에서 도구의 기본 원본 경로를 모두 `D:\01. Code\16.SEM\07. Test\Code_backup\NanoImpact_20230510\...`로 바꿨다
(하네스 `build.bat` 4개, `device_harness\make_assist.ps1`·`make_main.ps1`·`run.ps1 -IniSource`, CodeGen `DefaultHeader`, PortingPlan 3행).
이제 인자 없이 `build.bat`, `run.ps1 -Script ...`를 돌리면 된다. 바꾼 뒤 C++ 하네스 4개 빌드, layout 556줄, CodeGen 기본 실행(생성 파일 변화 없음),
device startup 16 / main_auto 340 / assist1 188, hv smoke 11, image overlap 61 모두 원본과 같음을 확인했다.
원본을 다시 옮기면 이 경로들을 같이 바꾸거나 인자(`build.bat "<원본 폴더>"`, `run.ps1 -IniSource "<INI 폴더>"`)로 넘긴다.
참고: `make_assist.ps1`/`make_main.ps1`을 인자 없이 돌리면 `scripts\assist.txt`/`main.txt`를 새로 만든다(저장소의 assist1/main1 등과 다른 이름 — 필요 없으면 지운다).

### 3.3 기타 도구
- Python은 있다(`py`, python-docx 1.2.0). 예전 세션에는 없다고 잘못 기록한 적 있음 — `python3`는 Windows Store 별칭이라 안 되고 `py`를 쓴다.
- Claude Code 메모리 파일은 `C:\Users\Administrator\.claude\projects\C--Users-Administrator-Downloads-sem-proj\memory\` (작업 폴더별 저장).
  다른 폴더에서 세션을 열면 안 읽히므로 이 문서 폴더 `memory\`에 사본을 두었다.

---

## 4. 저장소 구조

```
NanoImpactNET
├─ Directory.Build.props        net48 / x86 / C# 7.3 공통
├─ NanoImpactNET.sln            src: Core, Hardware, App / tools: Verify, CodeGen
├─ doc\PortingPlan.md
├─ src
│  ├─ NanoImpact.Core           상수·구조체·C 런타임 흉내
│  │  ├─ Generated\Defines.g.cs     원본 #define 2,971개 → Def.이름 (생성)
│  │  ├─ Generated\Records.g.cs     INI 구조체 3개(바이트 배열 오버레이, 원본 필드 이름) (생성)
│  │  └─ Native\                    CRuntime(sprintf/atof 등 UCRT와 같게), CharBuffer, NativeArray, ScanControlPublicData,
│  │                                VacuumBeamAlignFocusData, ControlData.g.cs(SEM_CONTROL_* 28종, 생성), Payloads
│  ├─ NanoImpact.Hardware
│  │  ├─ Ftdi\                      ftd2xx P/Invoke(IFtdi, Ftd2xxApi), Ft232hFrames
│  │  ├─ ScanBridge\                Scan Bridge 수신 파서
│  │  ├─ Device\SemDevice*.cs + Port\SemDevice.L*.cs      원본 USB_FT232H_Device.cpp 전체 (Afx* 함수)
│  │  ├─ Device\Assist\AssistDevice*.cs                   원본 ModularAssistDevice.cpp (154개)
│  │  ├─ Device\Main\MainDeviceService*.cs + Port\        원본 메인 화면의 장치 처리기 83개, 메시지 맵 118항목
│  │  ├─ Device\Main\Panels.g.cs                          패널 인터페이스 10개 (생성, 8단계가 구현)
│  │  ├─ Device\DeviceHost.cs                             실제 IDeviceHost (UI 스레드 SendMessage/PostMessage, 스레드)
│  │  ├─ Imaging\SemScanImageProcessor.cs + Port\         SE 영상 조립·스캔 시작·비트맵 채우기
│  │  └─ Spellman\                                        RS232(UsbRS232Comm), 빔 창 감시(CfeHVPowerControl), 응답 해석(CfeHVPowerMonitor)
│  └─ NanoImpact.App            WPF 셸 (7단계), 실행 파일 NanoImpact.exe
│     ├─ MainWindow.xaml(.cs)        새 디자인 화면
│     ├─ Composition\MainShell*.cs   조립 루트 = 원본 CNanoImpact2020cfeDlg 뼈대
│     ├─ Shell\                      ImageSurface(WriteableBitmap), TimerTable(SetTimer/KillTimer), HistogramView
│     ├─ Placeholders\               임시 패널(PanelStubs.g.cs, 생성) — 8단계에서 실제 패널로 교체
│     └─ Sim\                        SimFtdi, SimScanSource (--sim)
└─ tools
   ├─ NanoImpact.CodeGen        원본 → C# 생성기 (모드: translate, dispatch, dispatch-class, structs, panels, 기본=상수·구조체)
   ├─ NanoImpact.Verify         C# 쪽 대조 실행기 (golden, layout, ini, device, image, hv, hostcheck)
   ├─ golden                    1단계 송신·수신 기준 C++
   ├─ layout_probe              구조체 크기·오프셋 MSVC 대조
   ├─ device_harness            장비 계층·Assist·메인 처리기 원본 대조 (원본 .cpp 그대로 컴파일)
   ├─ image_harness             영상 조립·스캔 시작 원본 대조
   ├─ hv_harness                Spellman RS232 원본 대조
   └─ app_smoke\drive_app.ps1   앱 화면 확인 (UI Automation으로 버튼 누르고 화면 저장)
```

### 4.1 생성 파일 vs 손질 파일 (중요)
- **다시 생성하면 안 되는 것** (기계 번역 후 손으로 고친 파일): `Device\Port\SemDevice.L*.cs`, `Device\Assist\AssistDevice.L*.cs`,
  `Device\Main\Port\MainDeviceService.L*.cs`, `Imaging\Port\SemScanImageProcessor.L*.cs`, `Spellman\Port\*.cs`.
  파일 이름의 `L숫자`는 원본 .cpp의 시작 줄 번호다.
- **생성기로만 고치는 것** (`*.g.cs`, 헤더에 "Do not edit"): `Defines.g.cs`, `Records.g.cs`, `ControlData.g.cs`, `Panels.g.cs`,
  `MainDeviceService.MessageMap.g.cs`, `Verify\PanelRecorders.g.cs`, `App\Placeholders\PanelStubs.g.cs`, `device_harness\panels.inc`.
  패널 4개 파일을 다시 만드는 명령 (원본 경로는 새 위치로):
  ```
  tools\NanoImpact.CodeGen\bin\Debug\NanoImpact.CodeGen.exe panels "<원본 폴더>" NanoImpact2020cfeDlg.cpp 16149-16582,16901-21569,21606-22313,22906-23141,33902-33922 src\NanoImpact.Hardware\Device\Main\Panels.g.cs tools\NanoImpact.Verify\PanelRecorders.g.cs tools\device_harness\panels.inc src\NanoImpact.App\Placeholders\PanelStubs.g.cs
  ```

### 4.2 이름·번역 규칙
- 원본 이름을 그대로 쓴다(`m_*`, `Afx*`, 원본 오타 포함 — `Infomation`, `Contrasrt`, `Gernerarion`). 원본 전역 `Afx*` 호출은 `m_dev.Afx*`.
- 형식: BOOL → int(TRUE/FALSE 상수), HWND → int 창 번호, `char[]` → `CharBuffer`, `NULL` → null 또는 0, `BYTE*` 영상 → `byte[]`/`RGBQUAD[]`.
- 창 번호: 메인 화면 1, Scan 보정 창(HW setup) 3, Stage 보정 창 4.
- 원본 메인 화면 함수 중 다른 계층이 부르는 것은 인터페이스로 끊는다: `IMainDeviceView`, `IScanImageView`, `IScanDevice`, `ICfeHVPowerView`, `IAssistOwner`.
- 주석은 한국어, 원본 줄 번호를 남긴다(`// ---- ModularAssistDevice.cpp:3745 ----`).

---

## 5. 단계별 현황

| # | 내용 | 커밋 | 원본 대조 결과 |
|---|---|---|---|
| 1 | FT232H 통신 하위 계층 (ftd2xx, 열기 순서, 송신 3종, Scan Bridge 수신) | 045b83c | golden 738줄 동일 |
| 2 | `DefineStructCode.h` → 상수 2,971개 + INI 구조체 3개 | 85917a9 | 배치 556줄 MSVC와 동일, INI 3개 왕복 동일 |
| 3 | 장비 명령 계층 `USB_FT232H_Device.cpp` 전체 → `SemDevice` | b58aab9 | startup 16, sweep1/2/7 각 464, rx 588 호출 동일 |
| 4 | SE 영상 조립 → `SemScanImageProcessor` | d919d0c | live 767 / photo12 227 / photo345 137 / photo6 175 / overlap 61 블록, se_read 62 동일 |
| 5 | Spellman C-FE HV RS232 | 71d94bb | send 538 / rx 1854 / monitor 19 / ramp 16 / port 28 / smoke 11 블록 동일 |
| 6a | `ModularAssistDevice.cpp` → `AssistDevice`, 실제 `DeviceHost` | 6ffbc3f | assist1/2/7 각 188, assist_modes 369 동일, hostcheck 통과 |
| 6b | 메인 화면 장치 처리기 83개 → `MainDeviceService` | c44bc11 | main0/1/2 약 2,410, main_auto 340 호출 동일, 패널 메서드 118개 모두 호출됨 |
| 7 | WPF 셸, 조립 루트, 스캔 시작 번역, `--sim` | 15dba69 | image scan 572 블록 동일, 시뮬레이터로 시작·스캔·종료 확인 |
| 8 | 제어 패널 | — | **다음 작업** |
| 9 | 자동 기능 (자동 시작 상태 기계, AF/AC/AB/Gun Align — OpenCV → OpenCvSharp 또는 HW_AF.dll) | — | |
| 10 | 주변·보정 (캡처·저장, 카메라 TVideoGrabber, Monogram 단축키, Environment, Stage/Scan/SRot 보정, 정보 띠 눈금자·측정 레이어) | — | |
| 11 | 외부 연결 (EDS TCP 7321 인증 `8da17fy758` 1바이트 정렬, UDP 4481, UDP EDS 송신 192.168.50.2:4482) | — | |

알려진 차이(원본 버그라서 맞추지 않은 것): sweep0 457/464, sweep20 449/464, assist0 186/188 — 모두 원본의 배열 밖 접근(spot size 0 → CL[-1] 등).
자세한 표는 PortingPlan 4절.

---

## 6. 검증 방법 (원본 대조 하네스)

공통 방식: 원본 .cpp를 **수정 없이** MSVC x86으로 컴파일한 실행 파일(가짜 장비·기록용 대체 클래스)과 C#(`NanoImpact.Verify`)에
같은 시나리오 스크립트를 넣고 출력(장비 쓰기 바이트, 창 메시지, 패널 호출, 상태 변화, 버퍼 해시)을 호출·블록 단위로 비교한다.

| 하네스 | 빌드 | 실행 예 |
|---|---|---|
| device_harness | `build.bat` | `powershell -File run.ps1 -Script scripts\main1.txt` |
| image_harness | `build.bat` | `powershell -File run.ps1 scripts\scan.txt` (photo6는 몇 분 걸림 — 백그라운드로) |
| hv_harness | `build.bat` | `powershell -File run.ps1 scripts\send.txt` |
| layout_probe | `build.bat` | `NanoImpact.Verify.exe layout > out\csharp_layout.txt` 후 `out\msvc_layout.txt`와 비교 (556줄) |
| hostcheck | — | `tools\NanoImpact.Verify\bin\Debug\NanoImpact.Verify.exe hostcheck tools\device_harness\out\ini` → "모두 통과" |
| app_smoke | 앱 빌드 | 아래 7.3 |

각 하네스 폴더의 README에 스크립트 명령·출력 형식·마지막 결과가 있다.
`out\`(빌드 결과, ini 사본)은 git에 없지만 지금 작업 폴더에는 빌드된 상태로 남아 있다.

새 원본 함수를 옮기는 순서(3~7단계에서 굳어진 방법):
1. 원본 줄 범위를 정한다(함수 목록은 `grep -n "^[A-Za-z].*CClass::"`).
2. `NanoImpact.CodeGen translate <cpp> <h> <outDir> <class> [chunk] [from] [to] [ns]`로 C# 초안 → `Port\클래스.L시작줄.cs`.
3. 손질: MFC/GDI 호출은 인터페이스로, 포인터 산술은 배열·오프셋으로, 원본 의미 유지.
4. C++ 하네스에 같은 줄 범위를 `extract.ps1`로 바이트 그대로 붙이고(`*.inc`), 대체 클래스·기록 함수 추가, 시나리오 작성.
5. C# 하네스(`NanoImpact.Verify`)에 같은 기록 추가 → `run.ps1`로 비교 → 차이가 0이 될 때까지(원본 버그는 문서화).
6. PortingPlan 표·절, 하네스 README 갱신 → 사용자에게 보고 → "커밋·push" 기다림.

---

## 7. 7단계 결과물 자세히 (8단계가 바로 이어 붙일 곳)

### 7.1 앱 화면 (사용자 선택: "새 WPF 디자인")
원본 2560×1440 고정 비트맵 스킨을 흉내내지 않고, **원본과 같은 영역·기능을 WPF 표준 컨트롤로 새로 배치**, 창 크기에 맞춤(칸 사이 끌어서 조절).
- 상단 막대: 진공 상태(원본 문구 "SEM STANDBY" 등, 깜빡임), START/STANDBY, AIR/EVAC, GUN HV·배율 콤보(원본 표), GUN ALIGN/AUTO C&B/AUTO FOCUS, FE HV·EXTRACTOR, EXIT.
  START~AUTO 버튼은 아직 비활성(8·9단계).
- 영상: 1st LIVE(RED 상자 오버레이), 2nd PHOTO, 3rd CAMERA(10단계), 4th HISTOGRAM.
- 오른쪽 탭 SCAN(임시 RED~PHOTO6·CAPTURE STOP 버튼) / FOCUS / BEAM / ALIGN / VACUUM / OPTION — FOCUS 이하는 "step 8" 자리 표시.
- 아래: STAGE / CFE-HV(받은 줄 목록) / NAVI, DETAIL(처리기 정보 글자) / CAPTURE LIST / PANEL CALLS(임시 패널 호출 기록) / LOG.

### 7.2 조립 루트 `Composition\MainShell`
- 만드는 순서: `DeviceHost`(Dispatcher) → `SemDevice`(실제 `Ftd2xxApi` 또는 `SimFtdi`) → `AssistDevice` → `MainDeviceService`(임시 패널 `PanelStubs.Create(this)`)
  → `SemScanImageProcessor`(SE 스캔 공용 상태 객체를 MainDeviceService와 **같이** 씀) → `CfeHVPowerMonitor`(`SerialCommApi`).
- 창 1번 처리기 `MainWindowProc`: SE 영상 → 처리기, Spellman 수신/닫힘 → 모니터, 자동 기능 영상 5종 → 무시(9단계), 나머지 → `MainDeviceService.Dispatch`.
- 타이머: 메인 `TimerTable`(원본 OnTimer 갈래 10/11/20/21/22/40/90/98/99/100), Beam 창 `TimerTable` → `CfeHVPowerControl.OnTimer`.
- 시작(원본 OnInitDialog 장치 쪽 + 타이머 98 단계 0~9), 종료(OnClose: HV 동작 중 거부 → 확인 → 스레드 정지·RS232 닫기 → 500 ms 뒤
  캡처 끔·기본값·USBINTEGRATEDDATA.INI 저장·Scan Bridge 끔 → OnDestroy). 시스템 끄기(코드 99) 갈래는 8단계 확인 창과 함께 남김.
- 8단계에서 할 일과 연결점:
  - `PanelStubs.Create(this)` 자리를 실제 패널 객체로 바꾼다(`MainPanels`의 10개 필드). 패널이 한 번에 다 안 되면 하나씩 바꾸고 나머지는 임시 패널 유지.
  - SCAN 탭 임시 버튼(`MainShell.RequestLiveScan/RequestPhotoScan`)은 원본 `CModularScanControlDlg::OnBnClickedScanControl*Btn` 요청을 흉내낸 것 — 실제 Scan 패널로 대체.
  - 원본 패널이 메인 화면을 직접 부르는 곳(`((CNanoImpact2020cfeDlg*)m_pOwnerWindow)->...`)은 MainDeviceService/MainShell 쪽 메서드로.
  - Beam 패널은 이미 `Spellman\CfeHVPowerControl`(빔 창의 Spellman 부분)이 있다 — 버튼 처리(`OnBnClickedBeamControl*`)만 남음.
  - 단계 8의 Air→Evac 대기 안내(`SetExecuteAirToEvacWaitMessage`)는 지금 기록만 함 → Vacuum 패널과 같이.
  - `ScanView.GetCurrentCLValue`는 지금 장치 공용 데이터의 현재 CL을 돌려줌 → Scan 보정 창 구현 시 원본처럼 그 창 값으로.

### 7.3 시뮬레이터와 화면 확인
- 실행: `NanoImpact.exe --sim --ini <INI 폴더 사본>` (INI 기본값은 실행 파일 폴더 = 원본 GetExecutedPath).
  `--sim`은 FT232H 3개가 열린 척하고, 캡처가 켜지면 원본 SE 형식(FF 밝기 대비, 줄 사이 00, 화소 01~FE)의 합성 영상을 넣는다 →
  **실제 SE 수신 스레드 → 창 메시지 → 영상 조립 → 화면**이 그대로 돈다. 프레임 시간은 줄임. RS232는 열지 않음.
- 확인 스크립트(INI는 반드시 사본 — 종료 때 USBINTEGRATEDDATA.INI를 저장함):
  ```
  powershell -File tools\app_smoke\drive_app.ps1 -Exe src\NanoImpact.App\bin\Debug\NanoImpact.exe -Ini <INI 사본> -OutDir <png 폴더> -Steps @('wait:6','click:FAST','wait:3','shot:fast','click:EXIT','answer:Y','exited:8')
  ```
  단계: `wait:초`, `shot:이름`, `click:버튼글자`, `tab:탭이름`, `answer:대화상자버튼글자일부`(한국어 Windows는 "예(Y)"/"아니요(N)" → `answer:Y`), `exited:초`.
  주의: 소유 창이 있는 MessageBox는 UI Automation에서 메인 창의 **자식**으로 보인다(스크립트가 처리함). 앱이 띄운 창은 사용자 화면에 나타난다.
- 2026-11-09 확인 화면은 이 폴더 `화면\` (시작, FAST, RED 상자, PHOTO1/4/6과 히스토그램, 패널 호출 기록, 종료 확인).

---

## 8. 8단계 작업 계획 (제안)

원본 패널 크기(줄 수): Vacuum 2,846 / Beam 9,239(Spellman 부분은 5단계에 이미 옮김) / CfeHVPowerMonitor 1,734 / Focus 2,488 / Align 2,046 /
Scan 3,101 / Stage 6,511 + SampleLoadUnloadDlg / Detail 3,119. (보정 도구 Scan 8,992 / Stage 4,695, Environment 1,429는 10단계.)

권장 진행:
1. 패널마다 원본 `.h`의 멤버·메시지 맵과 `.cpp`의 처리기를 나눈다: **장치·상태 로직**(번역 + 원본 대조) / **화면**(WPF로 새로).
2. 로직은 `src\NanoImpact.Hardware`(또는 새 `Panels` 폴더)에 `Modular*Control` 클래스로 번역하고 `Panels.g.cs`의 인터페이스를 구현.
   화면 컨트롤 호출(`SetWindowText`, 비트맵 버튼, 스핀)은 패널별 뷰 인터페이스로 끊는다(5단계 `ICfeHVPowerView`와 같은 방식).
3. device_harness에 원본 패널 처리기를 붙여(`panels.inc` 기록 클래스 대신 실제 원본 함수) 버튼 → 처리기 → 장비 쓰기를 끝에서 끝까지 비교.
4. 앱에서 해당 탭에 WPF 화면을 만들고 `PanelStubs` 자리를 교체, `drive_app.ps1`로 시뮬레이터 확인.
5. 순서는 계획서대로 Vacuum → Beam → CFE 모니터 → Focus·Align·Scan → Stage(+Load/Unload) → Detail. 크면 8a/8b…로 나눠 사용자에게 알린다.

---

## 9. 사용자 결정이 필요한 것 (미해결)

1. **원본 Spellman 버그 3건 — 고칠지 원본대로 둘지** (지금은 원본 그대로):
   - 일반 감시 순번이 `>= 28`에서 0으로 돌아가 전체 상태 명령 `50`을 한 번도 보내지 않음.
   - HV 감시가 `>= 17`에서 돌아 히터 램프율 `2A`를 보내지 않음.
   - `CfeHVPowerCommand01Process`의 `(BYTE)x & 0x02 == 1` 우선순위 → 빔 에너지 외 오류 비트(추출기·억제기·히터)가 늘 OK로 표시.
   → Beam/CFE 모니터 패널(8단계) 때 같이 묻는 것을 권장.
2. **장비 시험 시점·장소**: 실제 FT232H 열기 순서와 SE 영상 속도, Spellman COM 포트 번호(`CLIENTPUBLICDATA.INI`의 `IDX_CLIENT_OPTION_INT_SEM_CFE_COM_PORT`),
   종료 때 Scan Bridge 끔, 다른 스레드 SendMessage와 UI 대기 사이 교착 여부, 고친 원본 버그(44바이트 수신 등)가 장비에서 맞는지.

---

## 10. 함정 모음 (지금까지 실제로 걸린 것)

- **인코딩**: 원본 주석은 CP949. `NanoImpact2020cfeDlg.cpp`는 **UTF-8 BOM + CP949/UTF-8 주석 혼재** → 번역기는 줄마다 판별, `extract.ps1`은 바이트 그대로+BOM 유지.
  읽어 보기만 할 때는 `iconv -f cp949 -t utf-8 -c`.
- `.bat`는 **ASCII만**(cmd가 한글 주석을 CP949로 읽어 명령으로 실행). `.ps1`은 ASCII 또는 **BOM 있는 UTF-8**(BOM 없는 UTF-8 한글이면 PS 5.1 파싱 오류).
  Write 도구로 만든 파일은 BOM이 없다.
- bash heredoc·perl 치환 안의 `"\\x"`가 `"\x"`나 NUL로 바뀌어 C/C# 문자열이 깨짐 → 역슬래시가 있는 코드는 Edit 도구나 PowerShell `.Replace()`로.
- PowerShell 문자열 `"$m?"`는 변수 `m?` → `"${m}?"`. `$n`/`$N` 같은 이름 충돌 주의.
- x86 DLL을 PowerShell `Add-Type`로 시험하면 BadImageFormat → `C:\Windows\SysWOW64\WindowsPowerShell\v1.0\powershell.exe`.
- C++ 하네스 스크립트 파일에 BOM이 있으면 첫 줄이 깨짐(harness.cpp는 BOM 건너뜀).
- 원본 하네스는 포인터를 캐스트하는 처리기에 정수 lParam을 주면 죽는다 → `make_main.ps1`은 처리기의 캐스트 형식을 보고 넣을 값(REQ/INFO/INTEG/NULL/INT)을 고른다.
- 메인 생성자 초기값(`m_bCLOnOffFlag = TRUE`, `m_strDistDecimalPlaces = "%6.2f"`)처럼 원본 생성자에만 있는 값을 빠뜨리면 대조가 어긋난다.
- `DeviceHost`: UI 스레드가 장치 스레드를 기다리는 동안 그 스레드가 SendMessage하면 교착(원본 Win32도 같은 구조) — UI 쪽에서 장치 스레드를 막고 기다리지 않게.
- 원본은 SE 데이터를 받을 때마다 1st 영상을 다시 그린다(약 100회/s, 원본 2020-05-04 주석) — 앱은 화면 갱신 주기마다 한 번만 옮긴다. 이건 의도된 차이.
- 포토 평균 버퍼 B는 2560×1920×120 = 590 MB(원본은 x86에서 한 번에 new) → C#은 1 MB 쪽 지연 할당 `PagedBuffer`.
- 커밋 전에 `git status`로 bin/obj·out이 안 들어가는지 확인(.gitignore 있음).

---

## 11. 이 폴더 내용

| 항목 | 내용 |
|---|---|
| `HANDOVER_NanoImpactNET_20261117.md` | 이 문서 (새 Claude에게 읽힐 원본) |
| `인수인계문서_NanoImpactNET_20261117.docx` | 같은 내용 Word 판 |
| `memory\` | Claude 메모리 사본 중 이 작업 관련(`nanoimpact-net48-port.md` — 단계별 상세 기록, `respond-in-korean.md`, `handover-20261117.md`) |
| `화면\` | 7단계 시뮬레이터 확인 화면(png) |
| `작업스크립트\make_docx.py` | 이 md → docx 변환 스크립트 (md를 고친 뒤 `py make_docx.py`로 다시 만듦) |
