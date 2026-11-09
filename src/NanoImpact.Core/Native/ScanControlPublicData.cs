#pragma warning disable IDE1006 // 원본 이름 그대로
namespace NanoImpact.Core.Native
{
    /// <summary>
    /// 원본 SEM_SE_SCAN_CONTROL_PUBLIC_DATA (DefineStructCode.h:5432). 메인 화면이 SE 스캔 영상 조립 상태로 쓰는 값 묶음 (파일로 저장하지 않음).
    /// 형식 대응: BOOL → int, COLORREF → uint, WORD → ushort.
    /// </summary>
    public sealed class SEM_SE_SCAN_CONTROL_PUBLIC_DATA
    {
        public int m_nABCCount;
        public int m_nAvrContrast;
        public int m_nAvrBright;
        public ushort[] m_wAvrAutoValue = new ushort[2];
        public int m_nSEFrameCount;
        public int m_nSEPixelCount; // Contrast Level 계산을 위한 Pixel 갯수
        public int m_nSEFrameBrightLevel; // Brightness Level 계산값 (FRAME단위)
        public int m_nSEPixelLevelSum; // Brightness Level 계산을 위한 Pixel 값   합계
        public int m_nSEPixelLevelCount; // Brightness Level 계산을 위한 Pixel 갯수 합계
        public int m_nSEDualScreenModeFlag;
        public int m_nSEDrawXPositionA;
        public int m_nSEDrawYPositionA;
        public int m_nSEDrawXPositionB;
        public int m_nSEDrawYPositionB;
        public int m_nSEBitmapXOffsetA;
        public int m_nSEBitmapYOffsetA;
        public int m_nSEBitmapXOffsetB;
        public int m_nSEBitmapYOffsetB;
        public int m_nSEAverageNumber;
        public int m_nSEAverageCount;
        public int m_nSEAverageIndex;
        public int m_nSEFrameAverageA;
        public int m_nSEFrameAverageB;
        public int m_bSEImageCaptureStart;
        public int m_nSEBitmapPixelWidth;
        public int m_nSEBitmapPixelHeight;
        public int m_nSemSEScanModeValue;
        public int m_nSemSEScanModeBackup;
        public int m_bPhotoScanCompleted; // SEM SE PHOTO MODE SCAN COMPLETED FLAG
        public int m_nPhotoScanMode;
        public int m_nSaveScanMode;
        public int m_nPhotoScanInfo;
        public int m_nSEAutoBrightContrastCount; // SEM SE Frame IMAGE DATA 의 Auto Bright, Contrast Code 갯수
        public int m_nSEAutoBrightValue; // SEM SE Frame IMAGE DATA 의 Auto Bright   값
        public int m_nSEAutoContrastValue; // SEM SE Frame IMAGE DATA 의 Auto Contrast 값
        public int m_nResultAutoProcess; // AUTO PROCESS 실행 결과 종류 ( FOCUS, CONTRAST, BRIGHTNESS, CONTRAST+BRIGHTNESS )
        public int m_nResultAutoState; // AUTO PROCESS 실행 결과 상태값 ( RUN, STOP, DONE ... )
        public int m_nResultOldScanMode; // AUTO PROCESS 실행 이전 스캔모드
        public int m_nResultFocusCoarse; // AUTO PROCESS 실행 Focus Coarse
        public int m_nResultFocusFine; // AUTO PROCESS 실행 Focus Fine
        public int m_nResultContrast; // AUTO PROCESS 실행 Contrast Value
        public int m_nResultBrightness; // AUTO PROCESS 실행 Brightness Value
        public int m_nResultMagIndex; // AUTO PROCESS 실행 Magnification Index
        public int m_nResultGunAlignX; // AUTO PROCESS 실행 Gun Align X
        public int m_nResultGunAlignY; // AUTO PROCESS 실행 Gun Align Y
        public int m_bAutoStartFlag;
        public int m_bAutoStartCancel;
        public int m_nAutoStartProcess;
        public int m_nAutoStartStep;
        public int m_bInit1stImage;
        public uint m_1stImageBackGndColor;
        public int m_n1stImageX;
        public int m_n1stImageY;
        public int m_n1stImageWidth;
        public int m_n1stImageHeight;
        public int m_n1stImageDrawX;
        public int m_n1stImageDrawY;
        public int m_bInit1stImageInfo;
        public uint m_1stImageInfoBackGndColor;
        public int m_n1stImageInfoX;
        public int m_n1stImageInfoY;
        public int m_n1stImageInfoWidth;
        public int m_n1stImageInfoHeight;
        public int m_n1stImageInfoDrawX;
        public int m_n1stImageInfoDrawY;
        public int m_bInit2ndImage640x480;
        public int m_bInit2ndImage1280x960;
        public int m_bInit2ndImage2560x1920;
        public int m_n2ndImageX;
        public int m_n2ndImageY;
        public int m_n2ndImageDrawX;
        public int m_n2ndImageDrawY;
        public int m_n2ndImageWidth;
        public int m_n2ndImageHeight;
        public uint m_2ndImageBackGndColor;
        public int m_n2ndImage640x480X;
        public int m_n2ndImage640x480Y;
        public int m_n2ndImage640x480Width;
        public int m_n2ndImage640x480Height;
        public int m_n2ndImage640x480DrawX;
        public int m_n2ndImage640x480DrawY;
        public int m_n2ndImage1280x960X;
        public int m_n2ndImage1280x960Y;
        public int m_n2ndImage1280x960Width;
        public int m_n2ndImage1280x960Height;
        public int m_n2ndImage1280x960DrawX;
        public int m_n2ndImage1280x960DrawY;
        public int m_n2ndImage2560x1920X;
        public int m_n2ndImage2560x1920Y;
        public int m_n2ndImage2560x1920Width;
        public int m_n2ndImage2560x1920Height;
        public int m_n2ndImage2560x1920DrawX;
        public int m_n2ndImage2560x1920DrawY;
        public int m_bInit2ndImageInfo;
        public uint m_2ndImageInfoBackGndColor;
        public int m_n2ndImageInfoX;
        public int m_n2ndImageInfoY;
        public int m_n2ndImageInfoWidth;
        public int m_n2ndImageInfoHeight;
        public int m_n2ndImageInfoDrawX;
        public int m_n2ndImageInfoDrawY;
        public int m_bInit3rdImage;
        public uint m_3rdImageBackGndColor;
        public int m_n3rdImageX;
        public int m_n3rdImageY;
        public int m_n3rdImageWidth;
        public int m_n3rdImageHeight;
        public int m_n3rdImageDrawX;
        public int m_n3rdImageDrawY;
        public int m_bInit3rdImageInfo;
        public uint m_3rdImageInfoBackGndColor;
        public int m_n3rdImageInfoX;
        public int m_n3rdImageInfoY;
        public int m_n3rdImageInfoWidth;
        public int m_n3rdImageInfoHeight;
        public int m_n3rdImageInfoDrawX;
        public int m_n3rdImageInfoDrawY;
        public int m_bInit4thImage;
        public uint m_4thImageBackGndColor;
        public int m_n4thImageX;
        public int m_n4thImageY;
        public int m_n4thImageWidth;
        public int m_n4thImageHeight;
        public int m_n4thImageDrawX;
        public int m_n4thImageDrawY;
        public int m_bInit4thImageInfo;
        public uint m_4thImageInfoBackGndColor;
        public int m_n4thImageInfoX;
        public int m_n4thImageInfoY;
        public int m_n4thImageInfoWidth;
        public int m_n4thImageInfoHeight;
        public int m_n4thImageInfoDrawX;
        public int m_n4thImageInfoDrawY;
        public int m_bInitLineTool;
        public uint m_LineToolBackGndColor;
        public int m_nLineToolX;
        public int m_nLineToolY;
        public int m_nLineToolWidth;
        public int m_nLineToolHeight;
        public int m_nLineToolDrawX;
        public int m_nLineToolDrawY;
        public double m_LayerRotAngle;
        public int m_bPhotoFilter1SelectFlag; // 필터1 : 이미지 부드럽게
        public int m_bPhotoFilter2SelectFlag; // 필터2 : 선명도 보정방법1
        public int m_bPhotoFilter3SelectFlag; // 필터3 : 선명도 보정방법2
        public int m_bPhotoFilter4SelectFlag; // 필터4 : 선명도 보정방법(OVERLAP)
        public int m_bOldPhotoFilter1SelectFlag; // 백업필터1 : 이미지 부드럽게
        public int m_bOldPhotoFilter2SelectFlag; // 백업필터2 : 선명도 보정방법1
        public int m_bOldPhotoFilter3SelectFlag; // 백업필터3 : 선명도 보정방법2
        public int m_bOldPhotoFilter4SelectFlag; // 백업필터4 : 선명도 보정방법(OVERLAP)
        public int m_bPhotoInverse1SelectFlag; // 음영반전
        public int m_bPhotoInverse2SelectFlag; // 좌우반전
        public int m_bPhotoInverse3SelectFlag; // 상하반전
        public int m_nSemPhotoAverageCombo; // Photo Average Combo Index
    }
}
