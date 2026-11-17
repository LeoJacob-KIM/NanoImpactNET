---
name: nanoimpact-net48-port
description: "신규 SW = NanoImpact(자체 CFE FE-SEM 운영 SW, C++ MFC 약 16만 줄)을 .NET Framework 4.8로 옮기는 작업 — 소스 위치·구조·규모"
metadata:
  node_type: memory
  type: project
  originSessionId: 1575a77a-38f9-47d6-9945-197ab75556b2
  modified: 2026-11-17T00:09:22.312Z
---

사용자 요청(2026-11-09): "신규 SW 개발" = `C:\Users\Administrator\Downloads\NanoImpact_20230510`의 소스를 **.NET Framework 4.8로 업그레이드**.

현황(2026-11-09 확인):
- 솔루션 `NanoImpact\source\NanoImpact2020cfe.sln`, 프로젝트 NanoImpact2020cfe.vcxproj — **네이티브 C++ MFC**(v142, MFC Static, Unicode, Win32/x64). .NET 코드는 전혀 없음 → "4.8 업그레이드" = C# 포팅(재작성)이거나 하이브리드.
- 규모: 자체 .cpp/.h 약 164k 줄. 큰 파일: NanoImpact2020cfeDlg.cpp 30k(메인 대화상자), USB_FT232H_Device.cpp 18.8k(장비 통신·보정 데이터·스레드, Afx* 전역 함수 API), UDP\OpThreads.cpp 17.4k, FEBeamManualControlDlg 8.3k, ModularBeamControlDlg 8.1k, ModularScanCalibrationToolDlg 7.8k, DefineStructCode.h 5.4k(구조체 정의).
- 모듈: FEBeam HV 모니터/수동 제어, Beam/Focus/Align/Scan/Detail 제어, Stage·Vacuum, 시료 Load/Unload, Scan/Stage/SRot 직교 보정, 영상 캡처·히스토그램, EDS(CoreDlg_S_EDS), UDP(CoreDlg_S_UDP), Monogram(CL), 소켓 서버(SocketProcess).
- 하드웨어: FTDI FT232H(ftd2xx, AfxFT232HPortSendCommand(port, WORD cmd, para) 등), AVR 명령, RS232, UDP 영상/스캔 스레드. 외부: OpenCV 4.1.0, log4cxx, TVideoGrabber 11.2, HW_AF.dll.
- 설정 파일: build\Out\Debug의 CLIENTPUBLICDATA.INI, USBINTEGRATEDDATA.INI, SROTCALIBRATIONDATA.INI.
- 주석은 CP949(EUC-KR) 한글 — 읽을 때 인코딩 주의.
- 마지막 빌드 2025-06-20(EDS/UDP/메인 Dlg 수정). git 없음.

결정·진행(2026-11-09): 사용자가 **C# 전면 포팅** 선택. 새 저장소 `C:\Users\Administrator\Downloads\NanoImpactNET`
(git main, 로컬 identity jongsu.jung). 원격 origin = https://github.com/LeoJacob-KIM/NanoImpactNET.git (private, 사용자가 별도 저장소로 결정).
첫 커밋 045b83c push 완료(2026-11-09) — 도구 셸에서 push가 바로 됨(GCM 자격 캐시됨). 원본 C++ 소스는 저장소에 넣지 않음. SDK-style csproj net48 + Directory.Build.props(x86 고정, C# 7.3),
VS2019 MSBuild로 빌드(`-nodeReuse:false -p:UseSharedCompilation=false`). 계획서 `doc\PortingPlan.md`(11단계, 원본 버그 표).
- 1단계 완료: `src\NanoImpact.Hardware` — Ftd2xx P/Invoke, Ft232hPort(열기 순서, SendCommand 4B, Words, AVR 프레임 0xD3/0xD2+"000"+cmd+'?'+cs+';'+0xFF),
  SbrReceiveParser(원본 넘침 기준까지 동일), ScanBridgeReader(250 ms 이벤트). `tools\golden`(원본 코드 옮긴 C++ 기준, cl x86) vs `tools\NanoImpact.Verify golden` 738줄 동일. 장비 미시험.
- 원본 버그: 수신 명령 찾기 `for(nIdx=0; n<44; nIdx++)` → 묶음 44바이트 이후 메시지 무시. 포팅에서 고침.
- 분석 결과: OpThreads.cpp는 죽은 코드(다른 프로젝트). 실제 영상 = FT232H SE 스레드 → UM_SEM_SE_SCAN_IMAGE_DATA → Dlg OnSemSEScanImageData(13488, 마커 0xFF/AB/AC/0xFE/0xFD/0xFC).
  Spellman C-FE HV RS232 115200 `:CC args CS\n`, EDS TCP 7321(인증 "8da17fy758", Pack=1), UDP 4481(SIGL/SIGF/SBCS), UDP→192.168.50.2:4482.
- 이 PC에 MFC 헤더 없음(원본 빌드 불가), cl.exe x86은 있음 → 구조체 크기·오프셋 대조용 프로브 가능. vcvars32는 한글 출력에 잡음을 내지만 동작함.
- 2단계 완료(2026-11-09, 커밋 85917a9 push됨): tools\NanoImpact.CodeGen이 원본 헤더(CP949)를 읽어 Core\Generated\Defines.g.cs(`Def.원본이름`, 2,971개, C 규칙 계산)와
  Records.g.cs(IntegratedCommandStruct/ClientPublicData/ScanRotationCalibrationData — byte[] 오버레이, 원본 필드 이름, 뷰는 class로 생성자에서 생성:
  struct 뷰는 CS1612로 `x.arr[i]=v`가 막혔음) 생성. tools\layout_probe(cl x86, MFC 없이 빈 형식으로 대체)와 배치 556줄 동일, INI 3개 왕복 동일.
  .bat 파일은 ASCII로만 (UTF-8 한글 주석을 cmd가 CP949로 읽어 명령으로 실행함).
- 3단계 완료(2026-11-09, 커밋 b58aab9 push됨): USB_FT232H_Device.cpp 전체 → `SemDevice` (Hardware\Device). 전역 → 필드, Afx 함수 → 같은 이름 public 메서드
  (BOOL→int TRUE/FALSE, HWND→int, char[]→CharBuffer, FT 핸들→IntPtr). 451개는 CodeGen `translate`로 기계 변환 후 손질(Device\Port\SemDevice.L*.cs,
  **다시 생성 금지**), FT 계층 30개는 SemDevice.Ftdi.cs에 손으로. Core: CRuntime(C 런타임, 정확한 %f/%e/%g), CharBuffer, CFile 대체, IDeviceHost/IFtdi.
  NativeArray는 C처럼 구조체 안이면 범위 밖 접근 허용 + NativeDiagnostics 기록(원본이 60칸 배열의 61번째를 읽는 곳 재현).
- 대조 하네스 tools\device_harness: 원본 .cpp를 수정 없이 MSVC x86 컴파일(shim\stdAfx.h, 가짜 Ftd2xx.dll), dispatch.inc(450 함수), run.ps1이 호출 단위 비교.
  결과: startup 16, sweep1/2/7 각 464, rx 588 호출 모두 원본과 동일. sweep0/20 차이는 원본의 배열 밖 접근(잘못된 인덱스)뿐.
- 함정: Write 도구로 만든 .ps1은 BOM이 없어 PS5.1이 한글을 깨뜨림 → BOM 붙여 저장. PowerShell 문자열 "$m?"는 변수 `m?` → "${m}?".
  C++ 하네스 스크립트에 BOM 있으면 첫 줄 깨짐(harness.cpp에서 BOM 건너뛰게 고침). 원본은 `;` 없는 AVR 문자열이나 종료 순서 역전에서 죽음.
- 4단계 완료(2026-11-09, 커밋 d919d0c push됨): Hardware\Imaging\SemScanImageProcessor(+Port 번역, IScanImageView, PagedBuffer, CBitmap 등 대체),
  tools\image_harness(원본 함수 extract.ps1로 CP949 그대로 복사, 대체 대화상자, scan_fields.inc 공유). live 767/photo12 227/photo345 137/photo6 175/overlap 61 블록 동일.
  device_harness에 SEREAD(스레드 한 바퀴: bScanDataReceive 복사 후 AfxSEScanDataImageProcess) 추가, se_read 62 동일(영상 메시지 버퍼 해시 포함).
  남은 것: *UpdateControl 화면 그리기·정보 띠, BSE 영상, 실제 스레드 연결(7단계). 번역기는 Class::Method·줄 범위·네임스페이스 인자 지원.
- 5단계 완료(2026-11-09, 커밋 71d94bb push됨): Hardware\Spellman — UsbRS232Comm(USB_RS232_Comm.cpp 손 포팅, Win32 직렬 함수는 ISerialCommApi,
  실제 SerialCommApi=SerialPort), CfeHVPowerControl(빔 창 6771-8932: 감시 타이머·Sending 29개·체크섬), CfeHVPowerMonitor(메인 22540-, 29831-31999:
  LF 조립·응답 35종, 모니터 창 램핑→디가우징), ICfeHVPowerView. tools\hv_harness: send 538/rx 1854/monitor 19/ramp 16/port 28 블록 동일.
  원본 버그(그대로 둠, 사용자 확인 필요): 일반 감시가 >=28에서 돌아 50(전체 상태)을 안 보냄, HV 감시는 2A 안 보냄, 01 응답 오류 비트 우선순위(&0x02==1).
  고친 것: 빔 창이 지워진 포트 포인터 계속 사용, CommThread 넘침 계산, 수신 버퍼 넘침.
  함정: Dlg.cpp는 UTF-8 BOM + CP949/UTF-8 주석 혼재 → 번역기는 줄마다 판별, extract.ps1은 바이트 그대로+BOM 유지(원본 빌드는 UTF-8로 컴파일).
  공용 shim CString을 MFC처럼 포인터 하나로 바꿈(Format("%s", CString) 가변 인자). perl -pi로 C 문자열의 "\\x" 치환하면 NUL이 생김 → PowerShell로 고침.
  이 PC의 COM4/5/6/10은 무슨 장비인지 몰라 열지 않음. x86 DLL 시험은 SysWOW64 powershell.
- 6단계는 6a/6b로 나눔(2026-11-09). 6a 완료(커밋 6ffbc3f push됨): Device\Assist\AssistDevice(ModularAssistDevice.cpp 154개 번역, Afx* → m_dev.Afx*,
  메인 화면 호출 5개는 IAssistOwner), Core\Native\VacuumBeamAlignFocusData.cs(176필드), Device\DeviceHost(실제 IDeviceHost: RegisterWindow(hWnd, proc),
  SendMessage는 UI 스레드 동기, ThreadFailed 이벤트). device_harness 확장: assist_methods.inc + CodeGen dispatch-class → CALL A.이름(형식),
  OWNER 줄로 요청 구조체 비교. assist1/2/7 188, assist_modes 369 동일. Verify hostcheck(실제 스레드·5026 수신·종료) 통과.
  6b 완료(2026-11-09, 커밋 c44bc11 push됨): Device\Main\MainDeviceService(처리기 83개 번역, 메시지 맵 118항목 → WindowProc, IAssistOwner 구현),
  패널 인터페이스 10개 Main\Panels.g.cs(CodeGen panels: 메서드 118·컨트롤 2, 기본 인자 포함), IMainDeviceView(범위 밖 메인 함수 19개),
  Core\Native\ControlData.g.cs(CodeGen structs, SEM_CONTROL_* 28종). device_harness에 원본 처리기 5구간 + 메인 창 메시지 디스패치 →
  끝에서 끝까지 대조. main0/1/2 ~2,410, main_auto 340, 기존 rx/assist/sweep 모두 동일, 패널 메서드 전부 호출됨.
  함정: bash heredoc/perl 안의 "\\x"가 "\x"로 바뀌어 C/C# 문자열이 깨짐 → 역슬래시는 Edit/PowerShell로. 메인 생성자 초기값(m_bCLOnOffFlag=TRUE) 확인 필요했음.
- 7단계 완료(2026-11-09, 커밋 15dba69 push됨): UI는 사용자가 **"새 WPF 디자인"** 선택(원본 영역·기능 그대로, 표준 컨트롤, 창 크기에 맞춤; 원본 2560x1440 비트맵 스킨 아님).
  src\NanoImpact.App(net48 WPF, NanoImpact.exe, 실행 `--sim`, `--ini <폴더>`). Composition\MainShell = 원본 메인 대화상자 뼈대(조립 루트, 창1 분배, TimerTable,
  시작 타이머98 단계0~9, OnClose/OnDestroy). 패널은 CodeGen panels 8번째 인자로 만든 임시 패널(Placeholders\PanelStubs.g.cs → PANEL CALLS 칸).
  스캔 시작·Fill* 번역(Imaging\Port L09564/L07342/L07791) image_harness scan 572 블록 동일. 시뮬레이터: SimFtdi + SimScanSource가 SE 형식 합성 영상 →
  실제 SE 수신 스레드 → 처리기 → 화면. UI Automation 스크립트(PrintWindow 화면 저장, 소유 MessageBox는 main 창의 자식으로 찾음)로 시작·스캔·종료 확인.
  비-sim 실행은 이 PC의 COM 포트를 열 수 있어 하지 않음. Python은 `py`로 있음(python-docx 1.2.0) — `python3`만 Store 별칭이라 안 됨.
- 인수인계(2026-11-17): 이 작업만 따로 `D:\01. Code\16.SEM\07. Test\인수인계_NanoImpactNET_20261117\HANDOVER_NanoImpactNET_20261117.md`
  (+docx, memory 사본, 화면, make_docx.py). 저장소는 이제 `D:\01. Code\16.SEM\07. Test\NanoImpactNET`, 원본은 `Code_backup\NanoImpact_20230510`.
  tools 기본 원본 경로(build.bat 4개·make_*.ps1·run.ps1·CodeGen DefaultHeader)는 커밋 d59098a(push됨)에서 Code_backup 경로로 정리, 하네스 재확인 일치.
  다음 = 8단계(제어 패널), 사용자가 "8단계 시작"이라고 할 때.
- (4단계 메모) 대상 = Dlg.cpp 13488-16148 (OnSemSEScanImageData + SemSEScanImageDataUpdateProcessing(라이브) +
  PhotoScan12/345/6/OverlapMode 처리기, ~2,660줄). 외부 계산 의존은 CModularAssistDevice::InsertSort(ModularAssistDevice.cpp:3915),
  CPublicToolClass::HistogramNormalize(PublicToolClass.cpp:1592)뿐, 나머지는 화면(SetBitmapBits, SetClientInfomation0NText, 스캔 버튼, *UpdateControl).
  계획: 같은 방식(원본 줄 범위 그대로 잘라 붙인 C++ 하네스 + C# SemScanImageProcessor, 비트맵은 해시 비교). 상태 구조체 SEM_SE_SCAN_CONTROL_PUBLIC_DATA는
  Core\Native\ScanControlPublicData.cs(필드 154, 원본 이름)로 생성함. m_p2ndBitmapBits와 m_p2ndImageRgb는 같은 메모리(BYTE*/RGBQUAD*) → C#은 RGBQUAD[] 하나 공유.
  버퍼 B는 2560*1920*120 = 590MB(원본도 x86에서 new) → C#은 쪽 단위 지연 할당 버퍼 고려. PowerShell $n/$N 같은 변수 주의.

**Why:** 사용자의 다음 주력 작업. 기존 sem_proj·SEMInspectionSW(TESCAN SharkSEM 원격)와 달리 장비를 직접 하드웨어로 제어하는 SW.
**How to apply:** 원본 폴더는 참고용으로 두고 새 폴더에서 작업하는 기존 방식([[sem-inspection-sw-plan]])을 따를지 먼저 확인. 포팅 방식(전면 C# / C++ DLL+C# UI 하이브리드)은 2026-11-09 사용자에게 질문 중.
