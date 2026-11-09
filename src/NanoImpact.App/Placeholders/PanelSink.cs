// 8단계 패널 전까지: 메인 화면 장치 처리기가 패널에 넘긴 값을 화면 기록 칸에 남긴다.
using System;
using System.Globalization;
using System.Linq;
using NanoImpact.Hardware.Device;

namespace NanoImpact.App.Placeholders
{
    /// <summary>임시 패널(PanelStubs.g.cs)이 부른 것을 받는 곳</summary>
    internal interface IPanelSink
    {
        /// <summary>패널 메서드 호출 (원본 m_wndXxx.Method(args))</summary>
        void Call(string name, params object[] args);
    }

    /// <summary>패널의 글자 컨트롤 자리</summary>
    internal sealed class StubText : ITextControl
    {
        private readonly IPanelSink _sink;
        private readonly string _name;

        public StubText(IPanelSink sink, string name)
        {
            _sink = sink;
            _name = name;
        }

        public void SetWindowTextW(string text) => _sink.Call(_name + ".SetWindowTextW", text);
        public void SetTextDataString(string text) => _sink.Call(_name + ".SetTextDataString", text);
    }

    /// <summary>호출을 한 줄 글자로 (같은 이름이 연달아 오면 마지막 값만 남기도록 이름을 따로 준다)</summary>
    internal static class PanelCallFormat
    {
        public static string Args(object[] args) =>
            string.Join(" ", (args ?? new object[0]).Select(a => a == null ? "null"
                : a is string s ? "\"" + s + "\""
                : a is double d ? d.ToString("G6", CultureInfo.InvariantCulture)
                : a is IFormattable f ? f.ToString(null, CultureInfo.InvariantCulture)
                : a.GetType().Name));
    }
}
