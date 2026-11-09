// 원본 NanoImpact2020cfeDlg.cpp:7791-8090 을 NanoImpact.CodeGen translate로 옮긴 뒤 손으로 고친 코드.
#pragma warning disable IDE1006, IDE0059, CS0162, CS0168, CS0219
using System;
using NanoImpact.Core.Native;
using static NanoImpact.Core.Def;
using static NanoImpact.Core.Native.CRuntime;

namespace NanoImpact.Hardware.Imaging
{
    public sealed partial class SemScanImageProcessor
    {
        // ---- NanoImpact2020cfeDlg.cpp:7791 ----
        public void Fill1stImageBitmap(int nX, int nY, int nWidth, int nHeight, uint colorFill, int bDCClear = FALSE)
        {
            // 1st Image (SEM Scan Image) Window
            // Set Background Color
            // 1st MemDC
            m_1stImageMemDC.FillSolidRect(nX, nY, nWidth, nHeight, colorFill);
            // DC
            if (bDCClear == TRUE)
            {
                // BITMAP DATA CLEAR
                if (m_p1stBitmapBits == null) return;
                if (m_p1stImageRgb == null) return;
                int  k = 0;
                byte btRED   = (byte)(colorFill);
                byte btGREEN = (byte)(((ushort)(colorFill)) >> 8);
                byte btBLUE  = (byte)(colorFill >> 16);
                for (int y = 0; y < nHeight; y++)
                {
                    for (int x = 0; x < nWidth; x++)
                    {
                        // PHOTO SCAN IMAGE DATA PIXEL POSITION
                        k = x + (y * nWidth);
                        // PIXEL DATA SET
                        m_p1stImageRgb[k].rgbRed   = btRED;
                        m_p1stImageRgb[k].rgbGreen = btGREEN;
                        m_p1stImageRgb[k].rgbBlue  = btBLUE;
                    }
                }
                // 1st IMAGE PIXEL BITMAP DATA SET
                m_1stImageBitmap.SetBitmapBits(m_bm1stBitmap.bmWidthBytes * m_bm1stBitmap.bmHeight, m_p1stBitmapBits);
            }
            // 2020.01.15
            // 1st IMAGE PIXEL BITMAP DATA SET + 1st LAYER OBJECT + 1st IMAGE DC DRAW
            DrawAll1stLayerObjectList();
        }

        // ---- NanoImpact2020cfeDlg.cpp:7828 ----
        public void Fill1stLayerBitmap(int nX, int nY, int nWidth, int nHeight, uint colorFill, int bDCClear = FALSE)
        {
            // 1st Layer Draw
            // Set Background Color
            // Layer MemDC
            m_1stLayerMemDC.FillSolidRect(nX, nY, nWidth, nHeight, colorFill);
            // DC
            if (bDCClear == TRUE)
            {
                // BITMAP DATA CLEAR
                if (m_p1stLayerBitmapBits == null) return;
                if (m_p1stLayerRgb == null) return;
                int  k = 0;
                byte btRED   = (byte)(colorFill);
                byte btGREEN = (byte)(((ushort)(colorFill)) >> 8);
                byte btBLUE  = (byte)(colorFill >> 16);
                for (int y = 0; y < nHeight; y++)
                {
                    for (int x = 0; x < nWidth; x++)
                    {
                        // LAYER DATA PIXEL POSITION
                        k = x + (y * nWidth);
                        // PIXEL DATA SET
                        m_p1stLayerRgb[k].rgbRed   = btRED;
                        m_p1stLayerRgb[k].rgbGreen = btGREEN;
                        m_p1stLayerRgb[k].rgbBlue  = btBLUE;
                    }
                }
                // 1st LAYER PIXEL BITMAP DATA SET
                m_1stLayerBitmap.SetBitmapBits(m_bm1stLayerBitmap.bmWidthBytes * m_bm1stLayerBitmap.bmHeight, m_p1stLayerBitmapBits);
            }
            // 2020.01.15
            // 1st LAYER PIXEL BITMAP DATA SET + 1st LAYER OBJECT + 1st IMAGE DC DRAW
            DrawAll1stLayerObjectList();
        }

        // ---- NanoImpact2020cfeDlg.cpp:7865 ----
        public void Fill1stImageInfoBitmap(int nX, int nY, int nWidth, int nHeight, uint colorFill)
        {
            // 1st Image (SEM Scan Image) Info Window
            // Set Background Color
            m_1stImageInfoMemDC.FillSolidRect(nX, nY, nWidth, nHeight, colorFill);
        }

        // ---- NanoImpact2020cfeDlg.cpp:7873 ----
        public void Fill2ndImage640x480Bitmap(int nX, int nY, int nWidth, int nHeight, uint colorFill, int bDCClear = FALSE)
        {
            // 2nd Image : Camera(SEM Photo, CMOS Camera, Vacuum USB Camera) Image Window
            // Set Background Color
            // MemDC
            m_2ndImage640x480MemDC.FillSolidRect(nX, nY, nWidth, nHeight, colorFill);
            // DC
            if (bDCClear == TRUE)
            {
                // BITMAP DATA CLEAR
                if (m_p2ndBitmapBits == null) return;
                if (m_p2ndImageRgb == null) return;
                int  k = 0;
                byte btRED   = (byte)(colorFill);
                byte btGREEN = (byte)(((ushort)(colorFill)) >> 8);
                byte btBLUE  = (byte)(colorFill >> 16);
                for (int y = 0; y < nHeight; y++)
                {
                    for (int x = 0; x < nWidth; x++)
                    {
                        // PHOTO SCAN IMAGE DATA PIXEL POSITION
                        k = x + (y * nWidth);
                        // PIXEL DATA SET
                        m_p2ndImageRgb[k].rgbRed   = btRED;
                        m_p2ndImageRgb[k].rgbGreen = btGREEN;
                        m_p2ndImageRgb[k].rgbBlue  = btBLUE;
                    }
                }
                // 2nd PIXEL BITMAP DATA SET
                m_2ndImage640x480Bitmap.SetBitmapBits(m_bm2ndBitmap.bmWidthBytes * m_bm2ndBitmap.bmHeight, m_p2ndBitmapBits);
            }
        }

        // ---- NanoImpact2020cfeDlg.cpp:7907 ----
        public void Fill2ndLayer640x480Bitmap(int nX, int nY, int nWidth, int nHeight, uint colorFill, int bDCClear = FALSE)
        {
            // 2nd Layer Image
            // Set Background Color
            // Layer MemDC
            m_2ndLayer640x480MemDC.FillSolidRect(nX, nY, nWidth, nHeight, colorFill);
            // DC
            if (bDCClear == TRUE)
            {
                // LAYER BITMAP DATA CLEAR
                if (m_p2ndLayerBitmapBits == null) return;
                if (m_p2ndLayerRgb == null) return;
                int  k = 0;
                byte btRED   = (byte)(colorFill);
                byte btGREEN = (byte)(((ushort)(colorFill)) >> 8);
                byte btBLUE  = (byte)(colorFill >> 16);
                for (int y = 0; y < nHeight; y++)
                {
                    for (int x = 0; x < nWidth; x++)
                    {
                        // PHOTO LAYER IMAGE DATA PIXEL POSITION
                        k = x + (y * nWidth);
                        // LAYER PIXEL DATA SET
                        m_p2ndLayerRgb[k].rgbRed   = btRED;
                        m_p2ndLayerRgb[k].rgbGreen = btGREEN;
                        m_p2ndLayerRgb[k].rgbBlue  = btBLUE;
                    }
                }
                // 2nd LAYER PIXEL BITMAP DATA SET
                m_2ndLayer640x480Bitmap.SetBitmapBits(m_bm2ndLayerBitmap.bmWidthBytes * m_bm2ndLayerBitmap.bmHeight, m_p2ndLayerBitmapBits);
            }
        }

        // ---- NanoImpact2020cfeDlg.cpp:7941 ----
        public void Fill2ndImage1280x960Bitmap(int nX, int nY, int nWidth, int nHeight, uint colorFill, int bDCClear = FALSE)
        {
            // 2nd Image : Camera(SEM Photo, CMOS Camera, Vacuum USB Camera) Image Window
            // Set Background Color
            // MemDC
            m_2ndImage1280x960MemDC.FillSolidRect(nX, nY, nWidth, nHeight, colorFill);
            // DC
            if (bDCClear == TRUE)
            {
                // BITMAP DATA CLEAR
                if (m_p2ndBitmapBits == null) return;
                if (m_p2ndImageRgb == null) return;
                int  k = 0;
                byte btRED   = (byte)(colorFill);
                byte btGREEN = (byte)(((ushort)(colorFill)) >> 8);
                byte btBLUE  = (byte)(colorFill >> 16);
                for (int y = 0; y < nHeight; y++)
                {
                    for (int x = 0; x < nWidth; x++)
                    {
                        // PHOTO SCAN IMAGE DATA PIXEL POSITION
                        k = x + (y * nWidth);
                        // PIXEL DATA SET
                        m_p2ndImageRgb[k].rgbRed   = btRED;
                        m_p2ndImageRgb[k].rgbGreen = btGREEN;
                        m_p2ndImageRgb[k].rgbBlue  = btBLUE;
                    }
                }
                // 2nd PIXEL BITMAP DATA SET
                m_2ndImage1280x960Bitmap.SetBitmapBits(m_bm2ndBitmap.bmWidthBytes * m_bm2ndBitmap.bmHeight, m_p2ndBitmapBits);
            }
        }

        // ---- NanoImpact2020cfeDlg.cpp:7975 ----
        public void Fill2ndLayer1280x960Bitmap(int nX, int nY, int nWidth, int nHeight, uint colorFill, int bDCClear = FALSE)
        {
            // 2nd Layer Image
            // Set Background Color
            // Layer MemDC
            m_2ndLayer1280x960MemDC.FillSolidRect(nX, nY, nWidth, nHeight, colorFill);
            // DC
            if (bDCClear == TRUE)
            {
                // LAYER BITMAP DATA CLEAR
                if (m_p2ndLayerBitmapBits == null) return;
                if (m_p2ndLayerRgb == null) return;
                int  k = 0;
                byte btRED   = (byte)(colorFill);
                byte btGREEN = (byte)(((ushort)(colorFill)) >> 8);
                byte btBLUE  = (byte)(colorFill >> 16);
                for (int y = 0; y < nHeight; y++)
                {
                    for (int x = 0; x < nWidth; x++)
                    {
                        // PHOTO LAYER IMAGE DATA PIXEL POSITION
                        k = x + (y * nWidth);
                        // PIXEL DATA SET
                        m_p2ndLayerRgb[k].rgbRed   = btRED;
                        m_p2ndLayerRgb[k].rgbGreen = btGREEN;
                        m_p2ndLayerRgb[k].rgbBlue  = btBLUE;
                    }
                }
                // 2nd LAYER PIXEL BITMAP DATA SET
                m_2ndLayer1280x960Bitmap.SetBitmapBits(m_bm2ndLayerBitmap.bmWidthBytes * m_bm2ndLayerBitmap.bmHeight, m_p2ndLayerBitmapBits);
            }
        }

        // ---- NanoImpact2020cfeDlg.cpp:8009 ----
        public void Fill2ndImage2560x1920Bitmap(int nX, int nY, int nWidth, int nHeight, uint colorFill, int bDCClear = FALSE)
        {
            // 2nd Image : Camera(SEM Photo, CMOS Camera, Vacuum USB Camera) Image Window
            // Set Background Color
            // MemDC
            m_2ndImage2560x1920MemDC.FillSolidRect(nX, nY, nWidth, nHeight, colorFill);
            // DC
            if (bDCClear == TRUE)
            {
                // BITMAP DATA CLEAR
                if (m_p2ndBitmapBits == null) return;
                if (m_p2ndImageRgb == null) return;
                int  k = 0;
                byte btRED   = (byte)(colorFill);
                byte btGREEN = (byte)(((ushort)(colorFill)) >> 8);
                byte btBLUE  = (byte)(colorFill >> 16);
                for (int y = 0; y < nHeight; y++)
                {
                    for (int x = 0; x < nWidth; x++)
                    {
                        // PHOTO SCAN IMAGE DATA PIXEL POSITION
                        k = x + (y * nWidth);
                        // PIXEL DATA SET
                        m_p2ndImageRgb[k].rgbRed   = btRED;
                        m_p2ndImageRgb[k].rgbGreen = btGREEN;
                        m_p2ndImageRgb[k].rgbBlue  = btBLUE;
                    }
                }
                // 2nd PIXEL BITMAP DATA SET
                m_2ndImage2560x1920Bitmap.SetBitmapBits(m_bm2ndBitmap.bmWidthBytes * m_bm2ndBitmap.bmHeight, m_p2ndBitmapBits);
            }
        }

        // ---- NanoImpact2020cfeDlg.cpp:8043 ----
        public void Fill2ndLayer2560x1920Bitmap(int nX, int nY, int nWidth, int nHeight, uint colorFill, int bDCClear = FALSE)
        {
            // 2nd Layer Image
            // Set Background Color
            // Layer MemDC
            m_2ndLayer2560x1920MemDC.FillSolidRect(nX, nY, nWidth, nHeight, colorFill);
            // DC
            if (bDCClear == TRUE)
            {
                // LAYER BITMAP DATA CLEAR
                if (m_p2ndLayerBitmapBits == null) return;
                if (m_p2ndLayerRgb == null) return;
                int  k = 0;
                byte btRED   = (byte)(colorFill);
                byte btGREEN = (byte)(((ushort)(colorFill)) >> 8);
                byte btBLUE  = (byte)(colorFill >> 16);
                for (int y = 0; y < nHeight; y++)
                {
                    for (int x = 0; x < nWidth; x++)
                    {
                        // PHOTO LAYER IMAGE DATA PIXEL POSITION
                        k = x + (y * nWidth);
                        // LAYER PIXEL DATA SET
                        m_p2ndLayerRgb[k].rgbRed   = btRED;
                        m_p2ndLayerRgb[k].rgbGreen = btGREEN;
                        m_p2ndLayerRgb[k].rgbBlue  = btBLUE;
                    }
                }
                if (m_nNormalFullImageSizeFlag == 0)
                {
                    // 2nd NORMAL SIZE LAYER PIXEL BITMAP DATA SET
                    m_2ndLayer2560x1920Bitmap.SetBitmapBits(m_bm2ndLayerBitmap.bmWidthBytes * m_bm2ndLayerBitmap.bmHeight, m_p2ndLayerBitmapBits);
                }
                else
                {
                    // 2nd FULL SIZE LAYER PIXEL BITMAP DATA SET
                    m_2ndFullSizeLayer2560x1920Bitmap.SetBitmapBits(m_bm2ndLayerBitmap.bmWidthBytes * m_bm2ndLayerBitmap.bmHeight, m_p2ndLayerBitmapBits);
                }
            }
        }

        // ---- NanoImpact2020cfeDlg.cpp:8085 ----
        public void Fill2ndImageInfoBitmap(int nX, int nY, int nWidth, int nHeight, uint colorFill)
        {
            // 2nd Image : Camera(SEM Photo, CMOS Camera, Vacuum USB Camera) Image Info Window
            // Set Background Color
            m_2ndImageInfoMemDC.FillSolidRect(nX, nY, nWidth, nHeight, colorFill);
        }

    }
}
