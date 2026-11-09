// 원본 NanoImpact2020cfeDlg.cpp:13488-16148 (SE 스캔 영상 조립)이 쓰는 메인 화면 멤버와 화면 객체 대체. 메서드 본문은 Imaging\Port.
#pragma warning disable IDE1006, CS0414, CS0649 // 원본 이름·원본에서 안 쓰는 멤버 그대로
using System;
using NanoImpact.Core.Native;
using static NanoImpact.Core.Def;

namespace NanoImpact.Hardware.Imaging
{
    /// <summary>원본 RGBQUAD (BGRA 32비트 픽셀)</summary>
    public struct RGBQUAD
    {
        public byte rgbBlue;
        public byte rgbGreen;
        public byte rgbRed;
        public byte rgbReserved;
    }

    /// <summary>원본 Win32 BITMAP 정보</summary>
    public sealed class BITMAP
    {
        public int bmType;
        public int bmWidth;
        public int bmHeight;
        public int bmWidthBytes;
        public ushort bmPlanes;
        public ushort bmBitsPixel;

        public BITMAP Clone() => (BITMAP)MemberwiseClone();
    }

    /// <summary>
    /// 영상 조립이 화면으로 내보내는 것 (원본은 대화상자 컨트롤·GDI 비트맵을 바로 건드렸다).
    /// 실제 앱은 WPF 쪽 구현, 원본 대조 시험은 기록하는 구현을 넣는다.
    /// </summary>
    public interface IScanImageView
    {
        /// <summary>원본 m_wndDetailControlDlg.SetClientInfomation0NText / m_ClientInfoText3·4.SetWindowTextW (정보 글자 칸)</summary>
        void SetInfoText(string control, string text);

        /// <summary>원본 CBitmap::SetBitmapBits — 포토 비트맵 내용 갱신</summary>
        void BitmapBitsSet(string bitmap, RGBQUAD[] bits, int byteCount);

        /// <summary>원본 Sem*UpdateControl() — 비트맵을 화면에 그림 (라이브: 1st, 포토: 2nd)</summary>
        void UpdateControl(string which);

        /// <summary>원본 m_wndScanControlDlg 호출 (스캔 버튼 누름, 필터 버튼 그림, 평균 횟수 글자)</summary>
        void ScanControl(string action, string argument);

        /// <summary>원본 DrawHistogramGraph</summary>
        void DrawHistogramGraph(double[] histogram, int cmax, int graphHeight);

        /// <summary>원본 MonogramToolsSetScanModeExecute()</summary>
        void MonogramToolsSetScanModeExecute();

        /// <summary>원본 IsRunImageGrab(type) — UDP 원격 저장 요청 중인지</summary>
        bool IsRunImageGrab(int type);

        /// <summary>원본 SetImageGrabEnd(TRUE, rgb)</summary>
        void SetImageGrabEnd(int ok, RGBQUAD[] rgb);

        /// <summary>원본 ::GetTickCount</summary>
        uint GetTickCount();
    }

    /// <summary>
    /// 원본 메인 화면(CNanoImpact2020cfeDlg)의 SE 스캔 영상 조립 부분. OnSemSEScanImageData가 SE 영상 보드에서 읽은 바이트 묶음을 받아
    /// 0xFF(프레임 시작)·AB/AC(자동 밝기·대비)·0x00(줄 시작)·픽셀을 해석하고, 스캔 모드별로 평균·필터를 거쳐 RGB 영상 버퍼를 채운다.
    /// </summary>
    public sealed partial class SemScanImageProcessor
    {
        private const int TRUE = 1;
        private const int FALSE = 0;
        private const int NULL = 0;

        // 원본 enum DEF_GRAB_TYPE (DefineStructCode.h:5778)
        public const int GRAB_TYPE_LIVE = 0;
        public const int GRAB_TYPE_SCAN = 1;

        private readonly IScanImageView _view;
        private readonly Func<IntegratedCommandStruct> _integrated;

        /// <param name="view">화면 쪽</param>
        /// <param name="integrated">원본 AfxGetIntegratedPointer() (장치 계층의 공용 데이터)</param>
        public SemScanImageProcessor(IScanImageView view, Func<IntegratedCommandStruct> integrated)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _integrated = integrated ?? throw new ArgumentNullException(nameof(integrated));
            m_wndDetailControlDlg = new DetailControlDlg(view);
            m_wndScanControlDlg = new ScanControlDlg(view);
            m_ClientInfoText3 = new CStatic(view, "ClientInfoText3");
            m_ClientInfoText4 = new CStatic(view, "ClientInfoText4");
            m_2ndImage640x480Bitmap = new CBitmap(view, "2ndImage640x480", 640, 480);
            m_2ndImage1280x960Bitmap = new CBitmap(view, "2ndImage1280x960", 1280, 960);
            m_2ndImage2560x1920Bitmap = new CBitmap(view, "2ndImage2560x1920", 2560, 1920);
            m_pAssistPublicTool = new PublicToolClass(view);

            // 원본 OnInitDialog:1509-1569, InitBitmap(1st 이미지 640x480 32bpp)
            m_nSemSEScanImageDataMax = MAX_SE_SCAN_IMAGE_WIDTH * MAX_SE_SCAN_IMAGE_HEIGHT;
            m_pSemSEScanImageReadBuffer = new byte[m_nSemSEScanImageDataMax];
            m_pSemSEScanImageThreadBuffer = new byte[m_nSemSEScanImageDataMax];
            m_pSemSEScanImageTempBuffer = new byte[m_nSemSEScanImageDataMax];
            m_pSemSEScanImageBitmapBufferA = new byte[STD_SE_SCAN_IMAGE_WIDTH * STD_SE_SCAN_IMAGE_HEIGHT * MAX_SE_SCAN_IMAGE_AVERAGE];
            m_pSemSEScanImageBitmapBufferB = new PagedBuffer((long)MAX_SE_SCAN_IMAGE_WIDTH * MAX_SE_SCAN_IMAGE_HEIGHT * MAX_SE_SCAN_IMAGE_AVERAGE);
            m_pSemSEScanImageBitmapBufferBF1 = new byte[MAX_SE_SCAN_IMAGE_WIDTH * MAX_SE_SCAN_IMAGE_HEIGHT];
            m_pSemSEScanImageBitmapBufferBF2 = new byte[MAX_SE_SCAN_IMAGE_WIDTH * MAX_SE_SCAN_IMAGE_HEIGHT];
            m_bm1stBitmap = new BITMAP { bmWidth = STD_SE_SCAN_IMAGE_WIDTH, bmHeight = STD_SE_SCAN_IMAGE_HEIGHT, bmWidthBytes = STD_SE_SCAN_IMAGE_WIDTH * 4, bmPlanes = 1, bmBitsPixel = 32 };
            m_p1stImageRgb = new RGBQUAD[STD_SE_SCAN_IMAGE_WIDTH * STD_SE_SCAN_IMAGE_HEIGHT];
            m_bm2ndBitmap = new BITMAP();
        }

        // ---- 원본 멤버 (NanoImpact2020cfeDlg.h, 같은 이름) ----

        public SEM_SE_SCAN_CONTROL_PUBLIC_DATA m_SemSEScanControlPublicData = new SEM_SE_SCAN_CONTROL_PUBLIC_DATA();

        // SE 스캔 데이터 버퍼 (원본 BYTE*)
        public int m_nSemSEScanImageDataMax;
        public int m_nSemSEScanImageSaveStart;
        public int m_nSemSEScanImageSaveCount;
        public int m_nSemSEScanImageReadStart;
        public int m_nSemSEScanImageReadSize;
        public int m_nSemSEScanImageFrameSize;
        public byte[] m_pSemSEScanImageReadBuffer;
        public byte[] m_pSemSEScanImageThreadBuffer;
        public byte[] m_pSemSEScanImageTempBuffer;
        public byte[] m_pSemSEScanImageBitmapBufferA;       // 라이브 (RED, FAST, SLOW1/2) 평균용: 640×480×120
        public PagedBuffer m_pSemSEScanImageBitmapBufferB;  // 포토 평균용: 2560×1920×120 = 590MB (원본은 한 번에 new) → 쪽 단위로 필요할 때 할당
        public byte[] m_pSemSEScanImageBitmapBufferBF1;     // 포토 필터 #1
        public byte[] m_pSemSEScanImageBitmapBufferBF2;     // 포토 필터 #2

        // 화면 비트맵 (원본 m_p1stImageRgb = (RGBQUAD*)m_p1stBitmapBits, m_p2ndImageRgb = (RGBQUAD*)m_p2ndBitmapBits — 같은 메모리)
        public BITMAP m_bm1stBitmap;
        public BITMAP m_bm2ndBitmap;
        public RGBQUAD[] m_p1stImageRgb;
        public RGBQUAD[] m_p2ndImageRgb;
        public RGBQUAD[] m_p2ndBitmapBits;
        public RGBQUAD[] m_p2ndOrgBitmapBits;
        public CBitmap m_2ndImage640x480Bitmap;
        public CBitmap m_2ndImage1280x960Bitmap;
        public CBitmap m_2ndImage2560x1920Bitmap;
        public object m_p4thImageDC;      // 히스토그램 그림 DC (화면 쪽이 그림)
        public object m_4thImageMemDC;

        public int m_bSemSEFrameFirstCatch;
        public int m_bPhotoCalibrationFlag;
        public int m_nAutoProcessRunStatus;
        public int m_nHistogramAnalysisBright;
        public int m_nHistogramAnalysisContrasrt;
        public int m_nHistogramAnalysisGamma;
        public int m_nReceiveDataCount;
        public int m_nReceiveTotalImageCount;
        public uint m_dwDisplayFrameCount;
        public uint m_dwDisplayUpdateCount;
        public uint m_dwFirstTick;
        public uint m_dwFrameDataCount;
        public uint m_dwFrameUpdateCount;
        public uint m_dwSecondTick;

        // 화면 객체 대체 (원본 메서드 이름 그대로 부를 수 있게)
        public readonly DetailControlDlg m_wndDetailControlDlg;
        public readonly ScanControlDlg m_wndScanControlDlg;
        public readonly CStatic m_ClientInfoText3;
        public readonly CStatic m_ClientInfoText4;
        public readonly ModularAssistDevice m_pAssistDeviceTool = new ModularAssistDevice();
        public readonly PublicToolClass m_pAssistPublicTool;

        /// <summary>
        /// 포토 모드 시작 때 원본이 하는 비트맵 준비 (SetSeScanModeOn의 포토 분기): 크기에 맞는 2nd 비트맵을 고르고
        /// m_p2ndBitmapBits/m_p2ndOrgBitmapBits를 새로 잡아 배경색으로 채운다.
        /// </summary>
        public void PreparePhotoBitmap(int width, int height, uint backgroundColor)
        {
            CBitmap b = width == 640 ? m_2ndImage640x480Bitmap : width == 1280 ? m_2ndImage1280x960Bitmap : m_2ndImage2560x1920Bitmap;
            m_bm2ndBitmap = b.Info.Clone();
            m_p2ndBitmapBits = new RGBQUAD[width * height];
            m_p2ndOrgBitmapBits = new RGBQUAD[width * height];
            m_p2ndImageRgb = m_p2ndBitmapBits;
            var c = new RGBQUAD { rgbRed = (byte)backgroundColor, rgbGreen = (byte)(backgroundColor >> 8), rgbBlue = (byte)(backgroundColor >> 16) };
            for (int i = 0; i < m_p2ndImageRgb.Length; i++) m_p2ndImageRgb[i] = c;
        }

        // ---- 원본 메인 화면의 다른 메서드 (이 범위 밖) ----

        private IntegratedCommandStruct AfxGetIntegratedPointer() => _integrated();
        private uint GetTickCount() => _view.GetTickCount();
        private int IsRunImageGrab(int type) => _view.IsRunImageGrab(type) ? TRUE : FALSE;
        private void SetImageGrabEnd(int ok, RGBQUAD[] rgb) => _view.SetImageGrabEnd(ok, rgb);
        private void MonogramToolsSetScanModeExecute() => _view.MonogramToolsSetScanModeExecute();
        private void SemSEScanImageDataUpdateControl() => _view.UpdateControl("SemSEScanImageDataUpdateControl");
        private void SemSEPhotoScan12ImageDataUpdateControl() => _view.UpdateControl("SemSEPhotoScan12ImageDataUpdateControl");
        private void SemSEPhotoScan345ImageDataUpdateControl() => _view.UpdateControl("SemSEPhotoScan345ImageDataUpdateControl");
        private void SemSEPhotoScan6ImageDataUpdateControl() => _view.UpdateControl("SemSEPhotoScan6ImageDataUpdateControl");

        // ---- 바이트 복사 (원본 memcpy/memset을 버퍼 종류별로) ----

        private static void memcpy(byte[] dst, int dstOffset, byte[] src, int count) => Buffer.BlockCopy(src, 0, dst, dstOffset, count);
        private static void memcpy(byte[] dst, byte[] src, int count) => Buffer.BlockCopy(src, 0, dst, 0, count);
        private static void memcpy(byte[] dst, byte[] src, uint count) => Buffer.BlockCopy(src, 0, dst, 0, (int)count);
        private static void memset(byte[] dst, int value, uint count) { for (int i = 0; i < count; i++) dst[i] = (byte)value; }
        private static void memset(byte[] dst, int value, int count) { for (int i = 0; i < count; i++) dst[i] = (byte)value; }
        private static void memset<T>(T[] dst, int value, int byteCount) where T : unmanaged => CRuntime.memset(dst, value, byteCount);

        /// <summary>RGBQUAD 버퍼 사이 바이트 복사 (원본 memcpy(m_p2ndOrgBitmapBits, m_p2ndBitmapBits, 바이트 수))</summary>
        private static void memcpy(RGBQUAD[] dst, RGBQUAD[] src, int byteCount) => Array.Copy(src, dst, byteCount / 4);

        // ---- 화면 객체 대체 ----

        public sealed class DetailControlDlg
        {
            private readonly IScanImageView _v;
            internal DetailControlDlg(IScanImageView v) { _v = v; }
            public void SetClientInfomation01Text(string s) => _v.SetInfoText("ClientInfomation01", s);
            public void SetClientInfomation02Text(string s) => _v.SetInfoText("ClientInfomation02", s);
            public void SetClientInfomation03Text(string s) => _v.SetInfoText("ClientInfomation03", s);
            public void SetClientInfomation04Text(string s) => _v.SetInfoText("ClientInfomation04", s);
            public void SetClientInfomation05Text(string s) => _v.SetInfoText("ClientInfomation05", s);
        }

        public sealed class ScanControlDlg
        {
            private readonly IScanImageView _v;
            internal ScanControlDlg(IScanImageView v) { _v = v; }
            public void OnBnClickedScanControlRedScanBtn() => _v.ScanControl("RedScan", "");
            public void OnBnClickedScanControlFastScanBtn() => _v.ScanControl("FastScan", "");
            public void OnBnClickedScanControlSlow1ScanBtn() => _v.ScanControl("Slow1Scan", "");
            public void OnBnClickedScanControlSlow2ScanBtn() => _v.ScanControl("Slow2Scan", "");
            public void SetPhotoAverageCountText(string s) => _v.ScanControl("PhotoAverageCountText", s);
            // 원본이 처리기 안에서 직접 쓰는 스캔 제어 창의 필터 선택 상태
            public int m_bPhotoFilter1SelectFlag;
            public int m_bOldPhotoFilter1SelectFlag;
            public void LoadPhotoFilterSelectButtonBitmapImage(int f1, int f2, int f3, int f4) => _v.ScanControl("PhotoFilterButtons", f1 + "," + f2 + "," + f3 + "," + f4);
        }

        public sealed class CStatic
        {
            private readonly IScanImageView _v;
            private readonly string _name;
            internal CStatic(IScanImageView v, string name) { _v = v; _name = name; }
            public void SetWindowTextW(string s) => _v.SetInfoText(_name, s);
        }

        /// <summary>원본 CBitmap (화면 호환 32bpp 비트맵). 내용은 SetBitmapBits로 받은 픽셀을 들고 있다.</summary>
        public sealed class CBitmap
        {
            private readonly IScanImageView _v;
            private readonly string _name;
            private RGBQUAD[] _bits;

            internal CBitmap(IScanImageView v, string name, int width, int height)
            {
                _v = v;
                _name = name;
                Info = new BITMAP { bmWidth = width, bmHeight = height, bmWidthBytes = width * 4, bmPlanes = 1, bmBitsPixel = 32 };
                _bits = new RGBQUAD[width * height];
            }

            public BITMAP Info { get; }

            public RGBQUAD[] Bits => _bits;

            public int SetBitmapBits(int byteCount, RGBQUAD[] bits)
            {
                int n = Math.Min(byteCount / 4, _bits.Length);
                Array.Copy(bits, _bits, Math.Min(n, bits.Length));
                _v.BitmapBitsSet(_name, _bits, byteCount);
                return byteCount;
            }
        }

        public sealed class ModularAssistDevice
        {
            // 원본 CModularAssistDevice::InsertSort (ModularAssistDevice.cpp:3915) — PHOTO IMAGE FILTERING을 위한 SORT 함수
            public void InsertSort(int[] pData, int n)
            {
                if (pData == null) return;
                int i, j;
                int temp;
                for (i = 1; i < n; i++)
                {
                    temp = pData[i];
                    j = i - 1;
                    while (j > -1 && pData[j] > temp)
                    {
                        pData[j + 1] = pData[j];
                        j--;
                    }
                    pData[j + 1] = temp;
                }
            }
        }

        public sealed class PublicToolClass
        {
            private readonly IScanImageView _v;
            internal PublicToolClass(IScanImageView v) { _v = v; }

            // 원본 CPublicToolClass::HistogramNormalize (PublicToolClass.cpp:1592): 비트맵 첫 바이트(파랑)로 히스토그램, 최빈값, 넓이로 정규화
            public void HistogramNormalize(ref CBitmap pBitmap, double[] pFHisto, ref int pCMax)
            {
                if (pBitmap == null) return;
                if (pFHisto == null) return;
                BITMAP bi = pBitmap.Info;
                RGBQUAD[] bits = pBitmap.Bits;
                int nWidth = bi.bmWidth;
                int nHeight = bi.bmHeight;
                var nHisto = new int[256];
                for (int nY = 0; nY < nHeight; nY++)
                    for (int nX = 0; nX < nWidth; nX++)
                        nHisto[bits[nY * nWidth + nX].rgbBlue]++;
                int nVal = 0;
                for (int nX = 0; nX < 256; nX++)
                {
                    if (nHisto[nX] > nVal)
                    {
                        nVal = nHisto[nX];
                        pCMax = nX;
                    }
                }
                double dArea = (double)(nWidth * nHeight);
                for (int nX = 0; nX < 256; nX++) pFHisto[nX] = nHisto[nX] / dArea;
            }

            public void DrawHistogramGraph(object pDC, ref object pMemDC, double[] pFHisto, int nCMax, int nGraphHeight) => _v.DrawHistogramGraph(pFHisto, nCMax, nGraphHeight);
        }
    }

    /// <summary>
    /// 큰 바이트 버퍼를 1MB 쪽 단위로 처음 쓸 때 할당한다 (읽기만 한 쪽은 0). 원본 new BYTE[2560*1920*120] 자리.
    /// x86 .NET에서 590MB 연속 배열은 주소 공간 조각 때문에 잡히지 않을 수 있다.
    /// </summary>
    public sealed class PagedBuffer
    {
        private const int PageBits = 20;
        private const int PageSize = 1 << PageBits;
        private readonly byte[][] _pages;

        public PagedBuffer(long length)
        {
            Length = length;
            _pages = new byte[(length + PageSize - 1) >> PageBits][];
        }

        public long Length { get; }

        public byte this[long index]
        {
            get
            {
                if ((ulong)index >= (ulong)Length) throw new IndexOutOfRangeException();
                byte[] p = _pages[index >> PageBits];
                return p == null ? (byte)0 : p[index & (PageSize - 1)];
            }
            set
            {
                if ((ulong)index >= (ulong)Length) throw new IndexOutOfRangeException();
                byte[] p = _pages[index >> PageBits];
                if (p == null)
                {
                    if (value == 0) return;
                    p = _pages[index >> PageBits] = new byte[PageSize];
                }
                p[index & (PageSize - 1)] = value;
            }
        }

        /// <summary>memset(buf, 0, 전체)</summary>
        public void Clear() => Array.Clear(_pages, 0, _pages.Length);

        /// <summary>시험용: 내용 해시 (할당 안 된 쪽은 0으로 본다)</summary>
        public ulong Fnv1a()
        {
            ulong h = 1469598103934665603UL;
            var zero = new byte[PageSize];
            for (int i = 0; i < _pages.Length; i++)
            {
                byte[] p = _pages[i] ?? zero;
                long n = Math.Min(PageSize, Length - ((long)i << PageBits));
                for (int k = 0; k < n; k++) { h ^= p[k]; h *= 1099511628211UL; }
            }
            return h;
        }
    }
}
