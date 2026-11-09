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
| 2 | 상수·구조체·설정 파일 | `DefineStructCode.h` → C# 상수/구조체 자동 변환, MSVC x86 크기·오프셋 대조, INI 3개 읽기·쓰기 왕복 시험 | 다음 |
| 3 | 장비 명령 계층 | `AfxScanBridge*`, `AfxIntegrated*` (HV, 필라멘트, 바이어스, CL/OL, 정렬, 스티그, 배율·Zoom 릴레이, 스캔 생성, 스테이지 CNC), Scan Bridge 수신 처리기(PD/AR/EV/VS/VL/EM/RP/RM/LT/…) | |
| 4 | 영상 취득 | SE/BSE 읽기 스레드 → `OnSemSEScanImageData`의 마커 해석(0xFF 프레임 시작, AB/AC, 0xFE/0xFD/0xFC) → 스캔 모드별(RED/FAST/SLOW/PHOTO1~6) 영상 조립 | |
| 5 | Spellman C-FE HV | `SerialPort`, 프레임·checksum(`0x200-sum`, `&0x7F|0x40`), LF 조립, 명령표 | |
| 6 | 장비 서비스 | `ModularAssistDevice` + 메인의 `OnSetUsbDevice*`/`SetSEM*` → `ISemDevice` 서비스, 상태 모델 | |
| 7 | WPF 셸 | 상단 바, 영상 4칸, 오른쪽 탭(Scan/Focus/Beam/Align/Vacuum/Option), Stage/CFE-HV, 하단 Detail, 캡처 목록 | |
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

## 5. 검증 도구

- `tools\golden\golden.cpp` — 원본 송신 패킷·수신 파서를 거의 그대로 옮긴 C++. `build.bat`로 MSVC x86 빌드.
- `tools\golden\make_vectors.ps1` — 고정 사례 + 시드 고정 무작위 입력(`vectors.txt`).
- `tools\NanoImpact.Verify` — `NanoImpact.Verify golden vectors.txt` 출력이 `golden.exe` 출력과 같아야 한다.

2026-11-09 결과: 738줄 동일 (CMD 208, AVR 212, 수신 MSG 83 / BAD 235).
