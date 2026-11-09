// Image harness: runs the ORIGINAL SE scan image assembly of the main dialog (NanoImpact2020cfeDlg.cpp:13488-16148,
// pasted unmodified as image_methods.inc) against a stub dialog, and prints state changes and display output.
// NanoImpact.Verify image <script> must print exactly the same for the C# port (SemScanImageProcessor).
// Keep ASCII only (compiled with /source-charset:.949 together with the original sources).
//
// Script lines:
//   # comment
//   INI <file>                      load INTEGRATED_COMMAND_STRUCT returned by AfxGetIntegratedPointer()
//   SET <name> <value>              scan control field (scan_fields.inc) or dialog member (see SetMember)
//   TICK <n>                        GetTickCount() value
//   PHOTO <w> <h>                   prepare the photo bitmap like the original scan start (640x480 / 1280x960 / 2560x1920)
//   FRAME <w> <h> <seed> <chunk>    synthetic frame: FF AB AC, lines separated by 00, pixels 01..FE; fed in <chunk>-byte calls
//   RAW <hex...>                    one OnSemSEScanImageData call with these bytes
//   DUMP                            hashes of all image buffers
//   SCAN <mode> <w> <h> <xoff> <yoff> <avg> <gen>   SemScanModeSelectAndScanStart (step 7)
//   SLOW12 <mode> <w> <h> <xoff> <yoff> <avg>       SemSlow12ScanModeSelectAndScanStart
//   CL <cl1> <cl2> <curCL1> <curCL2> <clOnOff>      CL values the scan start reads
//   OPT <idx> <value>               INTEGRATED_COMMAND_STRUCT.nScanOptionData[idx]
//   BOPT <idx> <value>              INTEGRATED_COMMAND_STRUCT.bScanOptionFlag[idx]
#include "stdAfx.h"
#include "DefineStructCode.h"
#include <vector>
#include <string>

static FILE* g_out = stdout;
static DWORD g_tick = 0;

static std::string Narrow(LPCTSTR w)
{
    if (!w) return std::string();
    int n = WideCharToMultiByte(CP_UTF8, 0, w, -1, NULL, 0, NULL, NULL);
    std::string s(n > 0 ? n - 1 : 0, '\0');
    if (n > 1) WideCharToMultiByte(CP_UTF8, 0, w, -1, &s[0], n, NULL, NULL);
    return s;
}

static unsigned long long Fnv(const void* p, size_t n)
{
    const unsigned char* b = (const unsigned char*)p;
    unsigned long long h = 1469598103934665603ULL;
    for (size_t i = 0; i < n; i++) { h ^= b[i]; h *= 1099511628211ULL; }
    return h;
}

// ---- redirections declared in shim\stdAfx.h (not used by the image code, but the shim needs them) ----
int WINAPI HarnessMessageBox(HWND, LPCTSTR text, LPCTSTR caption, UINT) { fprintf(g_out, "BOX %s | %s\n", Narrow(caption).c_str(), Narrow(text).c_str()); return IDOK; }
LRESULT WINAPI HarnessSendMessage(HWND, UINT msg, WPARAM w, LPARAM) { fprintf(g_out, "MSG %u %u\n", msg, (unsigned)w); return 0; }
BOOL WINAPI HarnessPostMessage(HWND, UINT msg, WPARAM w, LPARAM) { fprintf(g_out, "POST %u %u\n", msg, (unsigned)w); return TRUE; }
void WINAPI HarnessSleep(DWORD ms) { fprintf(g_out, "SLEEP %u\n", ms); }
DWORD WINAPI HarnessGetTickCount(void) { return g_tick; }
CWinThread* AfxBeginThread(AFX_THREADPROC, LPVOID, int, UINT, DWORD, LPSECURITY_ATTRIBUTES) { fprintf(g_out, "THREAD\n"); return new CWinThread(); }

// ---- stand-ins for the dialog controls the image code touches ----
static void Info(const char* control, LPCTSTR text) { fprintf(g_out, "INFO %s %s\n", control, Narrow(text).c_str()); }

class CModularDetailControlDlg
{
public:
    void SetClientInfomation01Text(CString s) { Info("ClientInfomation01", s); }
    void SetClientInfomation02Text(CString s) { Info("ClientInfomation02", s); }
    void SetClientInfomation03Text(CString s) { Info("ClientInfomation03", s); }
    void SetClientInfomation04Text(CString s) { Info("ClientInfomation04", s); }
    void SetClientInfomation05Text(CString s) { Info("ClientInfomation05", s); }
};

class CModularScanControlDlg
{
public:
    BOOL m_bPhotoFilter1SelectFlag = FALSE;
    BOOL m_bOldPhotoFilter1SelectFlag = FALSE;
    void OnBnClickedScanControlRedScanBtn() { fprintf(g_out, "CTL RedScan \n"); }
    void OnBnClickedScanControlFastScanBtn() { fprintf(g_out, "CTL FastScan \n"); }
    void OnBnClickedScanControlSlow1ScanBtn() { fprintf(g_out, "CTL Slow1Scan \n"); }
    void OnBnClickedScanControlSlow2ScanBtn() { fprintf(g_out, "CTL Slow2Scan \n"); }
    void SetPhotoAverageCountText(CString s) { fprintf(g_out, "CTL PhotoAverageCountText %s\n", Narrow(s).c_str()); }
    void LoadPhotoFilterSelectButtonBitmapImage(BOOL a, BOOL b, BOOL c, BOOL d) { fprintf(g_out, "CTL PhotoFilterButtons %d,%d,%d,%d\n", a, b, c, d); }
    void SetMagnificationUpDown(int n) { fprintf(g_out, "CTL MagnificationUpDown %d\n", n); }
};

// main dialog timer ids and display intervals (NanoImpact2020cfeDlg.cpp:408-432)
#define TIMER_ID_SE_SCAN_IMAGE_DISPLAY             10
#define TIMER_ID_SE_SCAN_IMAGE_DISPLAY_NEXT        11
#define TIMER_ID_SE_PHOTO12_SCAN_IMAGE_DISPLAY     20
#define TIMER_ID_SE_PHOTO345_SCAN_IMAGE_DISPLAY    21
#define TIMER_ID_SE_PHOTO6_SCAN_IMAGE_DISPLAY      22
#define SE_SCAN_IMAGE_UPDATE_ELAPSE_TIME           50
#define SE_PHOTO12_SCAN_IMAGE_UPDATE_ELAPSE_TIME   50
#define SE_PHOTO345_SCAN_IMAGE_UPDATE_ELAPSE_TIME  50
#define SE_PHOTO6_SCAN_IMAGE_UPDATE_ELAPSE_TIME    50

// step 7: scan start (SemScanModeSelectAndScanStart) callees
static int g_cl1 = 0, g_cl2 = 0, g_curCL1 = 0, g_curCL2 = 0;
class CModularScanCalibrationToolDlg
{
public:
    void SemSEScanModeDataUpdateControl(int n) { fprintf(g_out, "CAL SemSEScanModeDataUpdateControl %d\n", n); }
    int GetCurrentCL1Value() { return g_curCL1; }
    int GetCurrentCL2Value() { return g_curCL2; }
};
// memory DCs: only background fills are recorded (the new WPF screen draws the rest)
class CDCRec
{
    const char* m_name;
public:
    explicit CDCRec(const char* name) : m_name(name) {}
    void FillSolidRect(int x, int y, int w, int h, COLORREF c) { fprintf(g_out, "DC %s %d %d %d %d %u\n", m_name, x, y, w, h, (unsigned)c); }
};
int AfxIntegratedGetCL1() { return g_cl1; }
int AfxIntegratedGetCL2() { return g_cl2; }
int AfxIntegratedSetCL1(int n) { fprintf(g_out, "DEV AfxIntegratedSetCL1 %d\n", n); return 0; }
int AfxIntegratedSetCL1() { fprintf(g_out, "DEV AfxIntegratedSetCL1\n"); return 0; }
int AfxIntegratedSetCL2(int n) { fprintf(g_out, "DEV AfxIntegratedSetCL2 %d\n", n); return 0; }
int AfxIntegratedSetCL2() { fprintf(g_out, "DEV AfxIntegratedSetCL2\n"); return 0; }

class CStaticX
{
    const char* m_name;
public:
    explicit CStaticX(const char* name) : m_name(name) {}
    void SetWindowTextW(LPCTSTR s) { Info(m_name, s); }
};

class CBitmapX
{
public:
    const char* m_name;
    BITMAP m_info;
    std::vector<RGBQUAD> m_bits;
    CBitmapX(const char* name, int w, int h) : m_name(name)
    {
        memset(&m_info, 0, sizeof(m_info));
        m_info.bmWidth = w; m_info.bmHeight = h; m_info.bmWidthBytes = w * 4; m_info.bmPlanes = 1; m_info.bmBitsPixel = 32;
        m_bits.assign(w * h, RGBQUAD());
    }
    LONG SetBitmapBits(DWORD n, const void* p)
    {
        size_t m = n / 4 < m_bits.size() ? n / 4 : m_bits.size();
        memcpy(&m_bits[0], p, m * 4);
        fprintf(g_out, "BMP %s %u %016llX\n", m_name, n, Fnv(&m_bits[0], m_bits.size() * 4));
        return n;
    }
    DWORD GetBitmapBits(DWORD n, void* p) { size_t m = n < m_bits.size() * 4 ? n : m_bits.size() * 4; memcpy(p, &m_bits[0], m); return (DWORD)m; }
    int GetObject(int, void* p) { memcpy(p, &m_info, sizeof(BITMAP)); return sizeof(BITMAP); }
};
#define CBitmap CBitmapX

class CDCX {};

class CModularAssistDevice
{
public:
    void InsertSort(int* pData, int n);
    void SetSEMImageCaptureOnOffValueUsbDeviceData(int a = 0, int b = 0) { fprintf(g_out, "DEV SetSEMImageCaptureOnOffValueUsbDeviceData %d %d\n", a, b); }
    void SetSEMScanGenerationValueUsbDeviceData(int a = 0) { fprintf(g_out, "DEV SetSEMScanGenerationValueUsbDeviceData %d\n", a); }
    void SetSEMSlow12ImageCaptureOnUsbDeviceData(int a = 0) { fprintf(g_out, "DEV SetSEMSlow12ImageCaptureOnUsbDeviceData %d\n", a); }
    void ExecuteCLLMHModeData(int a, int b) { fprintf(g_out, "DEV ExecuteCLLMHModeData %d %d\n", a, b); }
};

class CPublicToolClass
{
public:
    void HistogramNormalize(CBitmap* pBitmap, double* pFHisto, int* pCMax);
    void DrawHistogramGraph(CDCX*, CDCX*, double* pFHisto, int nCMax, int nGraphHeight)
    {
        fprintf(g_out, "HISTO %d %d %016llX\n", nCMax, nGraphHeight, Fnv(pFHisto, sizeof(double) * 256));
    }
};

class CApp { public: void MonogramToolsSetScanModeExecute() { fprintf(g_out, "MONO\n"); } } theApp;

static INTEGRATED_COMMAND_STRUCT g_integrated;
INTEGRATED_COMMAND_STRUCT* AfxGetIntegratedPointer(void) { return &g_integrated; }

class CNanoImpact2020cfeDlg
{
public:
    CNanoImpact2020cfeDlg()
        : m_ClientInfoText3("ClientInfoText3"), m_ClientInfoText4("ClientInfoText4"),
          m_2ndImage640x480Bitmap("2ndImage640x480", 640, 480), m_2ndImage1280x960Bitmap("2ndImage1280x960", 1280, 960),
          m_2ndImage2560x1920Bitmap("2ndImage2560x1920", 2560, 1920),
          m_1stImageBitmap("1stImage", STD_SE_SCAN_IMAGE_WIDTH, STD_SE_SCAN_IMAGE_HEIGHT), m_1stLayerBitmap("1stLayer", STD_SE_SCAN_IMAGE_WIDTH, STD_SE_SCAN_IMAGE_HEIGHT),
          m_2ndLayer640x480Bitmap("2ndLayer640x480", 640, 480), m_2ndLayer1280x960Bitmap("2ndLayer1280x960", 1280, 960),
          m_2ndLayer2560x1920Bitmap("2ndLayer2560x1920", 2560, 1920), m_2ndFullSizeLayer2560x1920Bitmap("2ndFullSizeLayer2560x1920", 2560, 1920),
          m_1stImageMemDC("1stImageMemDC"), m_1stLayerMemDC("1stLayerMemDC"), m_1stImageInfoMemDC("1stImageInfoMemDC"),
          m_2ndImage640x480MemDC("2ndImage640x480MemDC"), m_2ndImage1280x960MemDC("2ndImage1280x960MemDC"), m_2ndImage2560x1920MemDC("2ndImage2560x1920MemDC"),
          m_2ndLayer640x480MemDC("2ndLayer640x480MemDC"), m_2ndLayer1280x960MemDC("2ndLayer1280x960MemDC"), m_2ndLayer2560x1920MemDC("2ndLayer2560x1920MemDC"),
          m_2ndImageInfoMemDC("2ndImageInfoMemDC")
    {
        memset(&m_bm1stLayerBitmap, 0, sizeof(BITMAP));
        memset(&m_bm2ndLayerBitmap, 0, sizeof(BITMAP));
        memset(&m_SemSEScanControlPublicData, 0x00, sizeof(SEM_SE_SCAN_CONTROL_PUBLIC_DATA));
        // OnInitDialog:1509-1569
        m_nSemSEScanImageDataMax = MAX_SE_SCAN_IMAGE_WIDTH * MAX_SE_SCAN_IMAGE_HEIGHT;
        m_pSemSEScanImageReadBuffer = new BYTE[m_nSemSEScanImageDataMax]();
        m_pSemSEScanImageThreadBuffer = new BYTE[m_nSemSEScanImageDataMax]();
        m_pSemSEScanImageTempBuffer = new BYTE[m_nSemSEScanImageDataMax]();
        m_pSemSEScanImageBitmapBufferA = new BYTE[STD_SE_SCAN_IMAGE_WIDTH * STD_SE_SCAN_IMAGE_HEIGHT * MAX_SE_SCAN_IMAGE_AVERAGE]();
        m_pSemSEScanImageBitmapBufferB = (BYTE*)VirtualAlloc(NULL, (SIZE_T)MAX_SE_SCAN_IMAGE_WIDTH * MAX_SE_SCAN_IMAGE_HEIGHT * MAX_SE_SCAN_IMAGE_AVERAGE, MEM_RESERVE | MEM_COMMIT, PAGE_READWRITE);
        m_pSemSEScanImageBitmapBufferBF1 = new BYTE[MAX_SE_SCAN_IMAGE_WIDTH * MAX_SE_SCAN_IMAGE_HEIGHT]();
        m_pSemSEScanImageBitmapBufferBF2 = new BYTE[MAX_SE_SCAN_IMAGE_WIDTH * MAX_SE_SCAN_IMAGE_HEIGHT]();
        memset(&m_bm1stBitmap, 0, sizeof(BITMAP));
        m_bm1stBitmap.bmWidth = STD_SE_SCAN_IMAGE_WIDTH; m_bm1stBitmap.bmHeight = STD_SE_SCAN_IMAGE_HEIGHT;
        m_bm1stBitmap.bmWidthBytes = STD_SE_SCAN_IMAGE_WIDTH * 4; m_bm1stBitmap.bmPlanes = 1; m_bm1stBitmap.bmBitsPixel = 32;
        m_p1stBitmapBits = new BYTE[STD_SE_SCAN_IMAGE_WIDTH * STD_SE_SCAN_IMAGE_HEIGHT * 4]();
        m_p1stImageRgb = (RGBQUAD*)m_p1stBitmapBits;
        memset(&m_bm2ndBitmap, 0, sizeof(BITMAP));
    }

    // same as SemScanImageProcessor.PreparePhotoBitmap
    void PreparePhotoBitmap(int w, int h, COLORREF bg)
    {
        CBitmap* b = w == 640 ? &m_2ndImage640x480Bitmap : w == 1280 ? &m_2ndImage1280x960Bitmap : &m_2ndImage2560x1920Bitmap;
        m_bm2ndBitmap = b->m_info;
        delete[] m_p2ndBitmapBits;
        delete[] m_p2ndOrgBitmapBits;
        m_p2ndBitmapBits = new BYTE[m_bm2ndBitmap.bmWidthBytes * m_bm2ndBitmap.bmHeight]();
        m_p2ndOrgBitmapBits = new BYTE[m_bm2ndBitmap.bmWidthBytes * m_bm2ndBitmap.bmHeight]();
        m_p2ndImageRgb = (RGBQUAD*)m_p2ndBitmapBits;
        RGBQUAD c; c.rgbRed = GetRValue(bg); c.rgbGreen = GetGValue(bg); c.rgbBlue = GetBValue(bg); c.rgbReserved = 0;
        for (int i = 0; i < w * h; i++) m_p2ndImageRgb[i] = c;
    }

    // ---- members used by the image code (NanoImpact2020cfeDlg.h) ----
    SEM_SE_SCAN_CONTROL_PUBLIC_DATA m_SemSEScanControlPublicData;
    int m_nSemSEScanImageDataMax = 0, m_nSemSEScanImageSaveStart = 0, m_nSemSEScanImageSaveCount = 0;
    int m_nSemSEScanImageReadStart = 0, m_nSemSEScanImageReadSize = 0, m_nSemSEScanImageFrameSize = 0;
    BYTE *m_pSemSEScanImageReadBuffer = NULL, *m_pSemSEScanImageThreadBuffer = NULL, *m_pSemSEScanImageTempBuffer = NULL;
    BYTE *m_pSemSEScanImageBitmapBufferA = NULL, *m_pSemSEScanImageBitmapBufferB = NULL;
    BYTE *m_pSemSEScanImageBitmapBufferBF1 = NULL, *m_pSemSEScanImageBitmapBufferBF2 = NULL;
    BITMAP m_bm1stBitmap, m_bm2ndBitmap;
    RGBQUAD *m_p1stImageRgb = NULL, *m_p2ndImageRgb = NULL;
    BYTE *m_p2ndBitmapBits = NULL, *m_p2ndOrgBitmapBits = NULL;
    CBitmap m_2ndImage640x480Bitmap, m_2ndImage1280x960Bitmap, m_2ndImage2560x1920Bitmap;
    CDCX *m_p4thImageDC = NULL;
    CDCX m_4thImageMemDC;
    BOOL m_bSemSEFrameFirstCatch = FALSE, m_bPhotoCalibrationFlag = FALSE;
    int m_nAutoProcessRunStatus = 0, m_nHistogramAnalysisBright = 0, m_nHistogramAnalysisContrasrt = 0, m_nHistogramAnalysisGamma = 0;
    int m_nReceiveDataCount = 0, m_nReceiveTotalImageCount = 0;
    DWORD m_dwDisplayFrameCount = 0, m_dwDisplayUpdateCount = 0, m_dwFirstTick = 0, m_dwFrameDataCount = 0, m_dwFrameUpdateCount = 0, m_dwSecondTick = 0;
    CModularDetailControlDlg m_wndDetailControlDlg;
    CModularScanControlDlg m_wndScanControlDlg;
    CStaticX m_ClientInfoText3, m_ClientInfoText4;
    CModularAssistDevice m_AssistDevice;
    CModularAssistDevice* m_pAssistDeviceTool = &m_AssistDevice;
    CPublicToolClass m_PublicTool;
    CPublicToolClass* m_pAssistPublicTool = &m_PublicTool;
    // step 7: scan start members
    BYTE *m_p1stBitmapBits = NULL;
    CBitmap m_1stImageBitmap, m_1stLayerBitmap;
    BITMAP m_bm1stLayerBitmap, m_bm2ndLayerBitmap;
    BYTE *m_p1stLayerBitmapBits = NULL, *m_p2ndLayerBitmapBits = NULL;
    RGBQUAD *m_p1stLayerRgb = NULL, *m_p2ndLayerRgb = NULL;
    CBitmap m_2ndLayer640x480Bitmap, m_2ndLayer1280x960Bitmap, m_2ndLayer2560x1920Bitmap, m_2ndFullSizeLayer2560x1920Bitmap;
    int m_nLayerPhotoMode = 0, m_nNormalFullImageSizeFlag = 0, m_n2ndFullSizeImageScan = 0;
    BOOL m_bCLOnOffFlag = TRUE;
    void *m_pSEScanImageBufferA = NULL, *m_pSEScanImageBitmapA = NULL, *m_pSEScanImageBufferB = NULL, *m_pSEScanImageBitmapB = NULL;
    CDCRec m_1stImageMemDC, m_1stLayerMemDC, m_1stImageInfoMemDC;
    CDCRec m_2ndImage640x480MemDC, m_2ndImage1280x960MemDC, m_2ndImage2560x1920MemDC;
    CDCRec m_2ndLayer640x480MemDC, m_2ndLayer1280x960MemDC, m_2ndLayer2560x1920MemDC, m_2ndImageInfoMemDC;
    CModularScanCalibrationToolDlg m_wndScanCalibrationToolDlg;
    UINT_PTR SetTimer(UINT_PTR id, UINT ms, void*) { fprintf(g_out, "TIMER SET %u %u\n", (unsigned)id, ms); return id; }
    BOOL KillTimer(UINT_PTR id) { fprintf(g_out, "TIMER KILL %u\n", (unsigned)id); return TRUE; }
    void DrawAll1stLayerObjectList() { fprintf(g_out, "LAYER DrawAll1stLayerObjectList\n"); }
    // Draw1stREDScanBox draws with GDI pens on the 1st memory DC: the C# port hands it to the screen, so both record the box
    void Draw1stREDScanBox(int l, int t, int r, int b, COLORREF c) { fprintf(g_out, "REDBOX %d %d %d %d %u\n", l, t, r, b, (unsigned)c); }

    // ---- other dialog methods the image code calls (outside the pasted range) ----
    BOOL IsRunImageGrab(int) { return FALSE; }
    void SetImageGrabEnd(BOOL, RGBQUAD*) { fprintf(g_out, "GRABEND\n"); }
    void SemSEScanImageDataUpdateControl() { Update("SemSEScanImageDataUpdateControl", m_p1stImageRgb, STD_SE_SCAN_IMAGE_WIDTH * STD_SE_SCAN_IMAGE_HEIGHT); }
    void SemSEPhotoScan12ImageDataUpdateControl() { Update("SemSEPhotoScan12ImageDataUpdateControl", m_p2ndImageRgb, m_bm2ndBitmap.bmWidth * m_bm2ndBitmap.bmHeight); }
    void SemSEPhotoScan345ImageDataUpdateControl() { Update("SemSEPhotoScan345ImageDataUpdateControl", m_p2ndImageRgb, m_bm2ndBitmap.bmWidth * m_bm2ndBitmap.bmHeight); }
    void SemSEPhotoScan6ImageDataUpdateControl() { Update("SemSEPhotoScan6ImageDataUpdateControl", m_p2ndImageRgb, m_bm2ndBitmap.bmWidth * m_bm2ndBitmap.bmHeight); }
    void Update(const char* which, const RGBQUAD* p, int n) { fprintf(g_out, "UPDATE %s %016llX\n", which, p ? Fnv(p, (size_t)n * 4) : 0ULL); }

    // ---- the original code ----
    LRESULT OnSemSEScanImageData(WPARAM wParam, LPARAM lParam);
    UINT SemSEScanImageDataUpdateProcessing(void);
    UINT SemSEPhotoScan12ImageDataUpdateProcessing(void);
    UINT SemSEPhotoScan345ImageDataUpdateProcessing(void);
    UINT SemSEPhotoScan6ImageDataUpdateProcessing(void);
    UINT SemSEPhotoScanOverlapModeImageDataUpdateProcessing(void);
    int SemScanModeSelectAndScanStart(int nScanMode, int nScanWidth, int nScanHeight, int nBitmapXOffset, int nBitmapYOffset, int nBitmapAverage, BOOL bScanGeneration);
    int SemSlow12ScanModeSelectAndScanStart(int nScanMode, int nScanWidth, int nScanHeight, int nBitmapXOffset, int nBitmapYOffset, int nBitmapAverage);
    void Fill1stImageBitmap(int nX, int nY, int nWidth, int nHeight, COLORREF colorFill, BOOL bDCClear = FALSE);
    void Fill1stLayerBitmap(int nX, int nY, int nWidth, int nHeight, COLORREF colorFill, BOOL bDCClear = FALSE);
    void Fill1stImageInfoBitmap(int nX, int nY, int nWidth, int nHeight, COLORREF colorFill);
    void Fill2ndImage640x480Bitmap(int nX, int nY, int nWidth, int nHeight, COLORREF colorFill, BOOL bDCClear = FALSE);
    void Fill2ndLayer640x480Bitmap(int nX, int nY, int nWidth, int nHeight, COLORREF colorFill, BOOL bDCClear = FALSE);
    void Fill2ndImage1280x960Bitmap(int nX, int nY, int nWidth, int nHeight, COLORREF colorFill, BOOL bDCClear = FALSE);
    void Fill2ndLayer1280x960Bitmap(int nX, int nY, int nWidth, int nHeight, COLORREF colorFill, BOOL bDCClear = FALSE);
    void Fill2ndImage2560x1920Bitmap(int nX, int nY, int nWidth, int nHeight, COLORREF colorFill, BOOL bDCClear = FALSE);
    void Fill2ndLayer2560x1920Bitmap(int nX, int nY, int nWidth, int nHeight, COLORREF colorFill, BOOL bDCClear = FALSE);
    void Fill2ndImageInfoBitmap(int nX, int nY, int nWidth, int nHeight, COLORREF colorFill);
};

#include "scan_start.inc"
#include "fill_bitmaps.inc"
#include "image_methods.inc"
#include "insert_sort.inc"
#include "histogram.inc"

// ---- state printing ----
struct FieldDef { const char* name; char type; int count; size_t offset; };
#define FIELD(t, n) { #n, #t[0], 1, offsetof(SEM_SE_SCAN_CONTROL_PUBLIC_DATA, n) },
#define FIELDA(t, n, c) { #n, #t[0], c, offsetof(SEM_SE_SCAN_CONTROL_PUBLIC_DATA, n) },
static FieldDef g_fields[] = {
#include "scan_fields.inc"
};
#undef FIELD
#undef FIELDA

static std::string FieldValue(const unsigned char* base, const FieldDef& f, int i)
{
    char b[64];
    const unsigned char* p = base + f.offset;
    switch (f.type)
    {
    case 'I': sprintf(b, "%d", ((const int*)p)[i]); break;
    case 'U': sprintf(b, "%u", ((const unsigned*)p)[i]); break;
    case 'W': sprintf(b, "%u", (unsigned)((const WORD*)p)[i]); break;
    default: sprintf(b, "%.17g", ((const double*)p)[i]); break;
    }
    return b;
}

static CNanoImpact2020cfeDlg* g_dlg;
static std::vector<std::string> g_prev;

struct MemberDef { const char* name; char type; void* p; };
static std::vector<MemberDef> Members(CNanoImpact2020cfeDlg* d)
{
    std::vector<MemberDef> m;
#define M(t, x) m.push_back(MemberDef{ #x, t, &d->x });
    M('I', m_bSemSEFrameFirstCatch) M('I', m_nSemSEScanImageSaveStart) M('I', m_nSemSEScanImageSaveCount)
    M('I', m_nReceiveDataCount) M('I', m_nReceiveTotalImageCount) M('U', m_dwDisplayFrameCount) M('U', m_dwDisplayUpdateCount)
    M('U', m_dwFirstTick) M('U', m_dwFrameDataCount) M('U', m_dwFrameUpdateCount) M('U', m_dwSecondTick) M('I', m_bPhotoCalibrationFlag)
    M('I', m_nAutoProcessRunStatus) M('I', m_nHistogramAnalysisBright) M('I', m_nHistogramAnalysisContrasrt) M('I', m_nHistogramAnalysisGamma)
    M('I', m_nLayerPhotoMode) M('I', m_nNormalFullImageSizeFlag) M('I', m_n2ndFullSizeImageScan) M('I', m_nSemSEScanImageReadStart) M('I', m_nSemSEScanImageReadSize)
#undef M
    m.push_back(MemberDef{ "m_wndScanControlDlg.m_bPhotoFilter1SelectFlag", 'I', &d->m_wndScanControlDlg.m_bPhotoFilter1SelectFlag });
    m.push_back(MemberDef{ "m_wndScanControlDlg.m_bOldPhotoFilter1SelectFlag", 'I', &d->m_wndScanControlDlg.m_bOldPhotoFilter1SelectFlag });
    return m;
}

static void PrintState()
{
    std::vector<std::string> cur;
    const unsigned char* base = (const unsigned char*)&g_dlg->m_SemSEScanControlPublicData;
    for (size_t k = 0; k < sizeof(g_fields) / sizeof(g_fields[0]); k++)
        for (int i = 0; i < g_fields[k].count; i++)
            cur.push_back(std::string(g_fields[k].name) + (g_fields[k].count > 1 ? "[" + std::to_string(i) + "]" : "") + "=" + FieldValue(base, g_fields[k], i));
    for (const MemberDef& m : Members(g_dlg))
    {
        char b[64];
        if (m.type == 'I') sprintf(b, "%d", *(int*)m.p); else sprintf(b, "%u", *(unsigned*)m.p);
        cur.push_back(std::string(m.name) + "=" + b);
    }
    if (g_prev.size() != cur.size()) g_prev.assign(cur.size(), "");
    for (size_t i = 0; i < cur.size(); i++)
        if (cur[i] != g_prev[i]) fprintf(g_out, "F %s\n", cur[i].c_str());
    g_prev = cur;
}

static bool SetField(const std::string& name, const std::string& value)
{
    unsigned char* base = (unsigned char*)&g_dlg->m_SemSEScanControlPublicData;
    for (size_t k = 0; k < sizeof(g_fields) / sizeof(g_fields[0]); k++)
    {
        if (name != g_fields[k].name) continue;
        unsigned char* p = base + g_fields[k].offset;
        switch (g_fields[k].type)
        {
        case 'I': *(int*)p = (int)strtol(value.c_str(), NULL, 0); break;
        case 'U': *(unsigned*)p = (unsigned)strtoul(value.c_str(), NULL, 0); break;
        case 'W': *(WORD*)p = (WORD)strtoul(value.c_str(), NULL, 0); break;
        default: *(double*)p = strtod(value.c_str(), NULL); break;
        }
        return true;
    }
    for (const MemberDef& m : Members(g_dlg))
        if (name == m.name) { *(int*)m.p = (int)strtol(value.c_str(), NULL, 0); return true; }
    return false;
}

static void Dump()
{
    CNanoImpact2020cfeDlg* d = g_dlg;
    fprintf(g_out, "H A %016llX\n", Fnv(d->m_pSemSEScanImageBitmapBufferA, STD_SE_SCAN_IMAGE_WIDTH * STD_SE_SCAN_IMAGE_HEIGHT * MAX_SE_SCAN_IMAGE_AVERAGE));
    fprintf(g_out, "H B %016llX\n", Fnv(d->m_pSemSEScanImageBitmapBufferB, (size_t)MAX_SE_SCAN_IMAGE_WIDTH * MAX_SE_SCAN_IMAGE_HEIGHT * MAX_SE_SCAN_IMAGE_AVERAGE));
    fprintf(g_out, "H BF1 %016llX\n", Fnv(d->m_pSemSEScanImageBitmapBufferBF1, MAX_SE_SCAN_IMAGE_WIDTH * MAX_SE_SCAN_IMAGE_HEIGHT));
    fprintf(g_out, "H BF2 %016llX\n", Fnv(d->m_pSemSEScanImageBitmapBufferBF2, MAX_SE_SCAN_IMAGE_WIDTH * MAX_SE_SCAN_IMAGE_HEIGHT));
    fprintf(g_out, "H 1stRgb %016llX\n", Fnv(d->m_p1stImageRgb, STD_SE_SCAN_IMAGE_WIDTH * STD_SE_SCAN_IMAGE_HEIGHT * 4));
    size_t n2 = (size_t)d->m_bm2ndBitmap.bmWidth * d->m_bm2ndBitmap.bmHeight * 4;
    fprintf(g_out, "H 2ndRgb %016llX\n", d->m_p2ndBitmapBits ? Fnv(d->m_p2ndBitmapBits, n2) : 0ULL);
    fprintf(g_out, "H 2ndOrg %016llX\n", d->m_p2ndOrgBitmapBits ? Fnv(d->m_p2ndOrgBitmapBits, n2) : 0ULL);
    size_t nl = (size_t)d->m_bm2ndLayerBitmap.bmWidth * d->m_bm2ndLayerBitmap.bmHeight * 4;
    fprintf(g_out, "H 2ndLayer %d %d %016llX\n", (int)d->m_bm2ndLayerBitmap.bmWidth, (int)d->m_bm2ndLayerBitmap.bmHeight, d->m_p2ndLayerBitmapBits ? Fnv(d->m_p2ndLayerBitmapBits, nl) : 0ULL);
    fprintf(g_out, "H 2ndBmp %d %d\n", (int)d->m_bm2ndBitmap.bmWidth, (int)d->m_bm2ndBitmap.bmHeight);
}

// same generator as NanoImpact.Verify ImageHarness: xorshift32, pixels 1..254
static unsigned g_rng;
static unsigned NextRand() { g_rng ^= g_rng << 13; g_rng ^= g_rng >> 17; g_rng ^= g_rng << 5; return g_rng; }

static void Feed(const std::vector<unsigned char>& data, size_t chunk)
{
    for (size_t off = 0; off < data.size(); off += chunk)
    {
        size_t n = data.size() - off < chunk ? data.size() - off : chunk;
        std::vector<unsigned char> buf(data.begin() + off, data.begin() + off + n);
        fprintf(g_out, "DATA %u\n", (unsigned)n);
        LRESULT r = g_dlg->OnSemSEScanImageData((WPARAM)n, (LPARAM)&buf[0]);
        fprintf(g_out, "RET %d\n", (int)r);
        PrintState();
    }
}

int main(int argc, char** argv)
{
    if (argc < 2) { fprintf(stderr, "usage: harness <script>\n"); return 2; }
    g_dlg = new CNanoImpact2020cfeDlg();
    PrintState();
    FILE* f = fopen(argv[1], "r");
    if (!f) { fprintf(stderr, "cannot open %s\n", argv[1]); return 2; }
    static char line[200000];
    while (fgets(line, sizeof(line), f))
    {
        line[strcspn(line, "\r\n")] = 0;
        if ((unsigned char)line[0] == 0xEF && (unsigned char)line[1] == 0xBB && (unsigned char)line[2] == 0xBF) memmove(line, line + 3, strlen(line + 3) + 1);
        if (!line[0] || line[0] == '#') continue;
        std::vector<std::string> tok;
        for (char* t = strtok(line, " \t"); t; t = strtok(NULL, " \t")) tok.push_back(t);
        const std::string& cmd = tok[0];
        fprintf(g_out, "> %s\n", cmd.c_str());
        if (cmd == "INI" && tok.size() >= 2)
        {
            FILE* fi = fopen(tok[1].c_str(), "rb");
            if (fi) { fread(&g_integrated, 1, sizeof(g_integrated), fi); fclose(fi); }
        }
        else if (cmd == "SET" && tok.size() >= 3) { if (!SetField(tok[1], tok[2])) fprintf(g_out, "NOFIELD %s\n", tok[1].c_str()); PrintState(); }
        else if (cmd == "TICK" && tok.size() >= 2) g_tick = (DWORD)strtoul(tok[1].c_str(), NULL, 0);
        else if (cmd == "PHOTO" && tok.size() >= 3) { g_dlg->PreparePhotoBitmap(atoi(tok[1].c_str()), atoi(tok[2].c_str()), DISPLAY_2ND_IMAGE_BACKGROUND_COLOR); }
        else if (cmd == "FRAME" && tok.size() >= 5)
        {
            int w = atoi(tok[1].c_str()), h = atoi(tok[2].c_str());
            g_rng = (unsigned)strtoul(tok[3].c_str(), NULL, 0);
            if (g_rng == 0) g_rng = 1;
            std::vector<unsigned char> data;
            data.push_back(0xFF);
            data.push_back((unsigned char)(1 + NextRand() % 254));
            data.push_back((unsigned char)(1 + NextRand() % 254));
            for (int y = 0; y < h; y++)
            {
                if (y > 0) data.push_back(0x00);
                for (int x = 0; x < w; x++) data.push_back((unsigned char)(1 + NextRand() % 254));
            }
            Feed(data, (size_t)atoi(tok[4].c_str()));
        }
        else if (cmd == "RAW" && tok.size() >= 2)
        {
            std::vector<unsigned char> data;
            for (size_t i = 1; i < tok.size(); i++) data.push_back((unsigned char)strtoul(tok[i].c_str(), NULL, 16));
            Feed(data, data.size());
        }
        else if (cmd == "DUMP") Dump();
        else if (cmd == "SCAN" && tok.size() >= 8)
        {
            // SCAN mode width height xoff yoff average generation = SemScanModeSelectAndScanStart (step 7)
            int r = g_dlg->SemScanModeSelectAndScanStart(atoi(tok[1].c_str()), atoi(tok[2].c_str()), atoi(tok[3].c_str()), atoi(tok[4].c_str()), atoi(tok[5].c_str()), atoi(tok[6].c_str()), atoi(tok[7].c_str()));
            fprintf(g_out, "RET %d\n", r);
            PrintState();
        }
        else if (cmd == "SLOW12" && tok.size() >= 7)
        {
            int r = g_dlg->SemSlow12ScanModeSelectAndScanStart(atoi(tok[1].c_str()), atoi(tok[2].c_str()), atoi(tok[3].c_str()), atoi(tok[4].c_str()), atoi(tok[5].c_str()), atoi(tok[6].c_str()));
            fprintf(g_out, "RET %d\n", r);
            PrintState();
        }
        else if (cmd == "OPT" && tok.size() >= 3) g_integrated.nScanOptionData[atoi(tok[1].c_str())] = atoi(tok[2].c_str());
        else if (cmd == "BOPT" && tok.size() >= 3) g_integrated.bScanOptionFlag[atoi(tok[1].c_str())] = atoi(tok[2].c_str());
        else if (cmd == "CL" && tok.size() >= 6)
        {
            // CL <AfxIntegratedGetCL1> <GetCL2> <panel GetCurrentCL1Value> <CL2> <m_bCLOnOffFlag>
            g_cl1 = atoi(tok[1].c_str()); g_cl2 = atoi(tok[2].c_str()); g_curCL1 = atoi(tok[3].c_str()); g_curCL2 = atoi(tok[4].c_str());
            g_dlg->m_bCLOnOffFlag = atoi(tok[5].c_str());
        }
        else fprintf(g_out, "UNKNOWN %s\n", cmd.c_str());
        fflush(g_out);
    }
    fclose(f);
    return 0;
}
