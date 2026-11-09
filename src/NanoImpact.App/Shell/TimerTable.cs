// 원본 CWnd::SetTimer/KillTimer 자리: 창 하나의 타이머 번호 → DispatcherTimer. 시간이 되면 그 창의 OnTimer(id)를 UI 스레드에서 부른다.
using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace NanoImpact.App.Shell
{
    internal sealed class TimerTable
    {
        private readonly Dispatcher _dispatcher;
        private readonly Action<int> _onTimer;
        private readonly Dictionary<int, DispatcherTimer> _timers = new Dictionary<int, DispatcherTimer>();

        /// <param name="onTimer">원본 OnTimer(nIDEvent)</param>
        public TimerTable(Dispatcher dispatcher, Action<int> onTimer)
        {
            _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
            _onTimer = onTimer ?? throw new ArgumentNullException(nameof(onTimer));
        }

        /// <summary>원본처럼 같은 번호를 다시 걸면 주기만 바꾸고 처음부터 센다</summary>
        public void Set(int id, int elapseMs)
        {
            if (!_timers.TryGetValue(id, out DispatcherTimer t))
            {
                t = new DispatcherTimer(DispatcherPriority.Background, _dispatcher);
                t.Tick += (s, e) =>
                {
                    // 처리기 안에서 KillTimer 되었으면 이번 Tick도 버린다 (WM_TIMER는 KillTimer 뒤에 오지 않는다)
                    if (((DispatcherTimer)s).IsEnabled) _onTimer(id);
                };
                _timers[id] = t;
            }
            t.Stop();
            t.Interval = TimeSpan.FromMilliseconds(Math.Max(1, elapseMs));
            t.Start();
        }

        public void Kill(int id)
        {
            if (_timers.TryGetValue(id, out DispatcherTimer t)) t.Stop();
        }

        public void KillAll()
        {
            foreach (DispatcherTimer t in _timers.Values) t.Stop();
        }

        public bool IsRunning(int id) => _timers.TryGetValue(id, out DispatcherTimer t) && t.IsEnabled;
    }
}
