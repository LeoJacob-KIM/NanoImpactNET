// 원본 CPublicToolClass::DrawHistogramGraph 자리 (4th 영상 칸): 256칸 히스토그램을 최대 칸(nCMax) 기준으로 높이를 맞춰 선으로 그린다.
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace NanoImpact.App.Shell
{
    internal sealed class HistogramView : FrameworkElement
    {
        private static readonly Pen AxisPen = Freeze(new Pen(new SolidColorBrush(Color.FromRgb(0x9B, 0xA3, 0xAB)), 1));
        private static readonly Pen LinePen = Freeze(new Pen(Brushes.White, 1.5));
        private static readonly Typeface Face = new Typeface("Segoe UI");

        private readonly double[] _histo = new double[256];
        private bool _has;

        public static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register(nameof(Background), typeof(Brush), typeof(HistogramView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush Background
        {
            get => (Brush)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        /// <summary>원본 nBmpHisto[x] = H * pFHisto[x] / pFHisto[nCMax]</summary>
        public void SetHistogram(double[] histogram, int cmax)
        {
            if (histogram == null || histogram.Length < 256 || cmax < 0 || cmax > 255) return;
            double max = histogram[cmax];
            for (int x = 0; x < 256; x++) _histo[x] = max > 0 ? histogram[x] / max : 0;
            _has = true;
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext dc)
        {
            var r = new Rect(0, 0, ActualWidth, ActualHeight);
            if (Background != null) dc.DrawRectangle(Background, null, r);
            const double left = 30, bottom = 22, top = 8, right = 8;
            double w = r.Width - left - right, h = r.Height - top - bottom;
            if (w < 20 || h < 20) return;

            double y0 = top + h;
            dc.DrawLine(AxisPen, new Point(left, top), new Point(left, y0));
            dc.DrawLine(AxisPen, new Point(left, y0), new Point(left + w, y0));
            for (int k = 0; k <= 4; k++)
            {
                double x = left + w * k / 4;
                dc.DrawLine(AxisPen, new Point(x, y0), new Point(x, y0 + 4));
                var t = new FormattedText((k * 64).ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, Face, 10, AxisPen.Brush, 1.0);
                dc.DrawText(t, new Point(x - t.Width / 2, y0 + 5));
            }
            if (!_has) return;

            var g = new StreamGeometry();
            using (StreamGeometryContext c = g.Open())
            {
                for (int x = 0; x < 256; x++)
                {
                    var p = new Point(left + w * x / 255.0, y0 - h * _histo[x]);
                    if (x == 0) c.BeginFigure(p, false, false);
                    else c.LineTo(p, true, false);
                }
            }
            g.Freeze();
            dc.DrawGeometry(null, LinePen, g);
        }

        private static Pen Freeze(Pen p)
        {
            p.Freeze();
            return p;
        }
    }
}
