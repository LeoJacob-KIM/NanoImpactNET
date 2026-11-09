// 장비 없이 실행 (--sim): 영상 캡처가 켜지면(SetSEMImageCaptureOnOffValueUsbDeviceData(1, mode)) SE 영상 장치의 형식으로
// 합성 영상을 만들어 SimFtdi에 넣는다. 형식은 4단계 영상 하네스 FRAME과 같다: FF 밝기 대비, 줄 사이 00, 화소 01..FE.
// 프레임 시간은 원본 모드 시간을 줄였다 (RED 33 ms, FAST 66 ms, SLOW1 1.6 s, SLOW2 3 s, PHOTO1~6 2~10 s).
using System;
using System.Threading;
using static NanoImpact.Core.Def;

namespace NanoImpact.App.Sim
{
    internal sealed class SimScanSource
    {
        private const int SceneW = 1280, SceneH = 960;

        private readonly SimFtdi _ftdi;
        private readonly object _lock = new object();
        private byte[] _scene;
        private Thread _thread;
        private volatile bool _stop;
        private int _mode;

        public SimScanSource(SimFtdi ftdi)
        {
            _ftdi = ftdi ?? throw new ArgumentNullException(nameof(ftdi));
        }

        /// <summary>캡처 켜기(모드) / 끄기</summary>
        public void Capture(int onOff, int scanMode)
        {
            if (onOff == 0)
            {
                Stop();
                return;
            }
            if (!ModeGeometry(scanMode, out _, out _, out _)) return;
            lock (_lock)
            {
                StopLocked();
                _mode = scanMode;
                _stop = false;
                _thread = new Thread(Run) { IsBackground = true, Name = "SimScanSource" };
                _thread.Start();
            }
        }

        public void Stop()
        {
            lock (_lock) StopLocked();
        }

        private void StopLocked()
        {
            _stop = true;
            _thread?.Join(2000);
            _thread = null;
            _ftdi.ClearRx(SimFtdi.SE_IMAGE);
        }

        private static bool ModeGeometry(int mode, out int w, out int h, out int frameMs)
        {
            w = 640; h = 480; frameMs = 66;
            if (mode == RED_SCAN_MODE) { w = SE_RED_SCAN_PIXEL_WIDTH; h = SE_RED_SCAN_PIXEL_HEIGHT; frameMs = 33; }
            else if (mode == FAST_SCAN_MODE) frameMs = 66;
            else if (mode == SLOW1_SCAN_MODE) frameMs = 1600;
            else if (mode == SLOW2_SCAN_MODE) frameMs = 3000;
            else if (mode == PHOTO1_SCAN_MODE || mode == PHOTO2_SCAN_MODE) frameMs = 2000;
            else if (mode == PHOTO3_SCAN_MODE) frameMs = 3000;
            else if (mode == PHOTO4_SCAN_MODE) { w = 1280; h = 960; frameMs = 3000; }
            else if (mode == PHOTO5_SCAN_MODE) { w = 1280; h = 960; frameMs = 5000; }
            else if (mode == PHOTO6_SCAN_MODE) { w = 2560; h = 1920; frameMs = 10000; }
            else return false;
            return true;
        }

        private void Run()
        {
            if (_scene == null) _scene = BuildScene();
            ModeGeometry(_mode, out int w, out int h, out int frameMs);
            bool red = _mode == RED_SCAN_MODE;
            uint rng = 0x9E3779B9u;
            int frame = 0;
            const int sliceMs = 20;
            while (!_stop)
            {
                // 표본 흔들림 (스테이지 진동 정도)
                double dx = 6 * Math.Sin(frame * 0.07), dy = 4 * Math.Cos(frame * 0.05);
                int slices = Math.Max(1, frameMs / sliceMs);
                int linesPerSlice = (h + slices - 1) / slices;
                var due = DateTime.UtcNow;
                for (int y0 = 0; y0 < h && !_stop; y0 += linesPerSlice)
                {
                    int y1 = Math.Min(h, y0 + linesPerSlice);
                    int size = (y0 == 0 ? 3 : 0) + (y1 - y0) * (w + 1) - (y0 == 0 ? 1 : 0);
                    var buf = new byte[size];
                    int k = 0;
                    if (y0 == 0)
                    {
                        buf[k++] = 0xFF;
                        buf[k++] = 128; // 밝기
                        buf[k++] = 128; // 대비
                    }
                    for (int y = y0; y < y1; y++)
                    {
                        if (y > 0) buf[k++] = 0x00;
                        // FAST 화면 640x480이 장면 1280x960의 가운데 절반. RED는 그 가운데 320x240, 포토는 같은 시야를 더 촘촘히.
                        double sy = red ? (y + 120) : y * 480.0 / h;
                        for (int x = 0; x < w; x++)
                        {
                            double sx = red ? (x + 160) : x * 640.0 / w;
                            int v = Sample(320 + sx + dx, 240 + sy + dy);
                            rng ^= rng << 13; rng ^= rng >> 17; rng ^= rng << 5;
                            v += (int)(rng % 41) - 20; // 검출기 잡음
                            buf[k++] = (byte)(v < 1 ? 1 : v > 254 ? 254 : v);
                        }
                    }
                    if (!_ftdi.PushRx(SimFtdi.SE_IMAGE, buf))
                    {
                        Thread.Sleep(50);
                        continue;
                    }
                    due = due.AddMilliseconds(sliceMs);
                    int wait = (int)(due - DateTime.UtcNow).TotalMilliseconds;
                    if (wait > 0) Thread.Sleep(wait);
                }
                frame++;
            }
        }

        private int Sample(double x, double y)
        {
            int ix = (int)x, iy = (int)y;
            if (ix < 0) ix = 0; else if (ix > SceneW - 2) ix = SceneW - 2;
            if (iy < 0) iy = 0; else if (iy > SceneH - 2) iy = SceneH - 2;
            double fx = x - ix, fy = y - iy;
            if (fx < 0) fx = 0; else if (fx > 1) fx = 1;
            if (fy < 0) fy = 0; else if (fy > 1) fy = 1;
            int o = iy * SceneW + ix;
            double a = _scene[o] + (_scene[o + 1] - _scene[o]) * fx;
            double b = _scene[o + SceneW] + (_scene[o + SceneW + 1] - _scene[o + SceneW]) * fx;
            return (int)(a + (b - a) * fy);
        }

        /// <summary>SE 영상처럼 보이는 장면: 어두운 바탕 + 가장자리가 밝은 입자 + 선 패턴</summary>
        private static byte[] BuildScene()
        {
            var f = new float[SceneW * SceneH];
            var r = new Random(20230510);
            for (int y = 0; y < SceneH; y++)
                for (int x = 0; x < SceneW; x++)
                    f[y * SceneW + x] = 60 + 10 * (float)Math.Sin(x * 0.013 + y * 0.007) + (((x / 64) + (y / 64)) % 2 == 0 ? 4 : 0);

            // 가운데 띠: 선폭 패턴 (CD 측정 대상 같은)
            for (int y = 380; y < 580; y++)
                for (int x = 200; x < 1080; x++)
                    if ((x / 24) % 2 == 0)
                    {
                        int e = Math.Min(Math.Abs(x % 48 - 0), Math.Abs(x % 48 - 23));
                        f[y * SceneW + x] += 50 + (e < 3 ? 70 : 0);
                    }

            for (int n = 0; n < 90; n++)
            {
                double cx = r.NextDouble() * SceneW, cy = r.NextDouble() * SceneH, rad = 6 + r.NextDouble() * 38;
                int x0 = Math.Max(0, (int)(cx - rad - 2)), x1 = Math.Min(SceneW - 1, (int)(cx + rad + 2));
                int y0 = Math.Max(0, (int)(cy - rad - 2)), y1 = Math.Min(SceneH - 1, (int)(cy + rad + 2));
                for (int y = y0; y <= y1; y++)
                    for (int x = x0; x <= x1; x++)
                    {
                        double d = Math.Sqrt((x - cx) * (x - cx) + (y - cy) * (y - cy)) / rad;
                        if (d > 1.05) continue;
                        // 가장자리 효과: 안쪽은 조금 밝고 테두리가 가장 밝다
                        double v = d < 1 ? 40 + 110 * Math.Pow(d, 6) : 150 * (1.05 - d) / 0.05;
                        int i = y * SceneW + x;
                        f[i] = (float)Math.Max(f[i], 70 + v);
                    }
            }

            var b = new byte[f.Length];
            for (int i = 0; i < f.Length; i++) b[i] = (byte)Math.Max(1, Math.Min(254, (int)f[i]));
            return b;
        }
    }
}
