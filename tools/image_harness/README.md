# 영상 조립 원본 대조 하네스

원본 메인 화면 `NanoImpact2020cfeDlg.cpp`의 SE 스캔 영상 조립 부분(13488~16148행: `OnSemSEScanImageData`, 라이브
`SemSEScanImageDataUpdateProcessing`, 포토 `…Scan12/345/6/OverlapMode…`)과 여기서 부르는 `CModularAssistDevice::InsertSort`,
`CPublicToolClass::HistogramNormalize`를 **원본 바이트 그대로 잘라 붙여** 컴파일하고, C# `SemScanImageProcessor`
(src\NanoImpact.Hardware\Imaging)와 같은 입력으로 비교한다. 대화상자 컨트롤·GDI 비트맵은 기록하는 대체 클래스로 바꾼다.

| 파일 | 역할 |
|---|---|
| `build.bat` | `extract.ps1`로 원본 함수를 `out\src\*.inc`에 복사(바이트 그대로, 원본에 BOM이 있으면 BOM 유지 — 원본 빌드처럼 UTF-8로 읽힌다), `harness.cpp`와 MSVC x86 빌드(`/LARGEADDRESSAWARE`) |
| `harness.cpp` | 대체 대화상자(원본 멤버 이름·형식), 화면 객체 기록, 스크립트 실행 |
| `scan_fields.inc` | `SEM_SE_SCAN_CONTROL_PUBLIC_DATA` 필드 154개 목록 — 원본·C# 하네스가 같은 순서로 상태를 찍는다 |
| `run.ps1` | 원본과 `NanoImpact.Verify image`를 같은 스크립트로 돌려 블록(`> 명령` / `DATA`) 단위 비교 |
| `scripts\*.txt` | live(FAST·RED·SLOW1, 조각 경계에 걸친 0xFF/AB/AC), photo12, photo345, photo6, overlap |

## 스크립트 명령

`INI 파일`, `SET 필드 값`(상태 구조체 필드·대화상자 멤버), `TICK n`, `PHOTO 너비 높이`(포토 비트맵 준비),
`FRAME 너비 높이 시드 조각`(FF AB AC + 줄마다 00 + 픽셀 01~FE를 xorshift32로 만들어 조각 크기씩 넣음), `RAW 바이트…`, `DUMP`(버퍼 해시).
시나리오의 `SET` 순서는 원본 `SemScanModeSelectAndScanStart`가 상태를 넣는 순서를 따른다.

## 출력

`F 필드=값`(바뀐 상태), `INFO 칸 글자`, `BMP 비트맵 바이트수 해시`, `UPDATE 함수 해시`(화면에 그릴 RGB 버퍼), `CTL 동작 인자`(스캔 버튼 등),
`HISTO 최빈값 높이 해시`, `MONO`, `H 버퍼 해시`(평균 버퍼 A/B, 필터 BF1/BF2, 1st/2nd RGB, 원본 보관).

## 2026-11-09 결과

| 시나리오 | 블록 | 같음 | 탄 경로 |
|---|---|---|---|
| live | 767 | 767 | 라이브 프레임 평균, 줄·프레임 표시, 조각 경계 |
| photo12 | 227 | 227 | 사진 완성, 필터 1·2, 히스토그램, 라이브 복귀 |
| photo345 | 137 | 137 | 1280×960 완성 2회, 필터 3 |
| photo6 | 175 | 175 | 2560×1920 완성 |
| overlap | 61 | 61 | 겹침 모드 완성 |

SE 보드에서 바이트를 읽어 이 처리기로 넘기는 장치 쪽(`AfxSEScanDataImageProcess`)은 `tools\device_harness`의 `SEREAD` 시나리오(`scripts\se_read.txt`)로 대조한다.
