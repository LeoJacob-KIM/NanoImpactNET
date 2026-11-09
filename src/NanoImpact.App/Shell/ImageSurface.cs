// 원본 화면 비트맵(CBitmap + 메모리 DC → 창 DC BitBlt) 자리: 처리기의 RGBQUAD 배열을 WriteableBitmap으로 옮겨 Image에 보인다.
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NanoImpact.Hardware.Imaging;

namespace NanoImpact.App.Shell
{
    /// <summary>영상 칸 하나 (1st 라이브, 2nd 포토 등). UI 스레드에서만 쓴다.</summary>
    internal sealed class ImageSurface
    {
        private readonly Image _image;
        private WriteableBitmap _bitmap;

        public ImageSurface(Image image)
        {
            _image = image ?? throw new ArgumentNullException(nameof(image));
        }

        public int Width => _bitmap?.PixelWidth ?? 0;
        public int Height => _bitmap?.PixelHeight ?? 0;

        private RGBQUAD[] _pending;
        private int _pendingW, _pendingH;

        /// <summary>화면에 실제로 옮긴 횟수 (화면 갱신 주기마다 최대 한 번)</summary>
        public int RenderCount { get; private set; }

        /// <summary>
        /// RGBQUAD(BGRA) 픽셀 width×height를 보이게 한다. 원본은 받은 조각마다 BitBlt 하므로 요청이 화면 갱신보다 잦다 —
        /// 다음 화면 갱신(CompositionTarget.Rendering) 때 마지막 요청 한 번만 옮긴다. 배열은 처리기 것이라 그때의 내용이 보인다.
        /// </summary>
        public void Show(RGBQUAD[] pixels, int width, int height)
        {
            if (pixels == null || width <= 0 || height <= 0 || pixels.Length < width * height) return;
            if (_pending == null) CompositionTarget.Rendering += OnRendering;
            _pending = pixels;
            _pendingW = width;
            _pendingH = height;
        }

        private void OnRendering(object sender, EventArgs e)
        {
            CompositionTarget.Rendering -= OnRendering;
            RGBQUAD[] p = _pending;
            _pending = null;
            if (p != null) ShowNow(p, _pendingW, _pendingH);
        }

        private unsafe void ShowNow(RGBQUAD[] pixels, int width, int height)
        {
            RenderCount++;
            if (_bitmap == null || _bitmap.PixelWidth != width || _bitmap.PixelHeight != height)
            {
                _bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr32, null);
                _image.Source = _bitmap;
            }
            fixed (RGBQUAD* p = pixels)
                _bitmap.WritePixels(new Int32Rect(0, 0, width, height), (IntPtr)p, width * height * 4, width * 4);
        }

        /// <summary>원본 FillSolidRect(배경색) — COLORREF(0x00BBGGRR)</summary>
        public void Fill(int x, int y, int width, int height, uint colorRef)
        {
            if (_bitmap == null) return;
            x = Math.Max(0, x);
            y = Math.Max(0, y);
            width = Math.Min(width, _bitmap.PixelWidth - x);
            height = Math.Min(height, _bitmap.PixelHeight - y);
            if (width <= 0 || height <= 0) return;
            int bgr = (int)(((colorRef & 0xFF) << 16) | (colorRef & 0xFF00) | ((colorRef >> 16) & 0xFF));
            var row = new int[width * height];
            for (int i = 0; i < row.Length; i++) row[i] = bgr;
            _bitmap.WritePixels(new Int32Rect(x, y, width, height), row, width * 4, 0);
        }
    }
}
