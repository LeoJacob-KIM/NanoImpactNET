using System;
using System.Collections.Generic;
using System.Threading;

namespace NanoImpact.Hardware.Device
{
    /// <summary>
    /// <see cref="IDeviceHost"/>의 실제 구현. 원본 Win32 동작을 그대로 따른다.
    /// <list type="bullet">
    /// <item>SendMessage: 창 번호(hWnd)에 등록된 처리기를 UI 스레드에서 실행하고 끝날 때까지 기다린다 (다른 스레드에서 불러도 UI 스레드에서 실행).</item>
    /// <item>PostMessage: UI 스레드 큐에 넣고 바로 돌아온다.</item>
    /// <item>BeginThread: 배경 스레드 시작. 스레드 본문에서 난 예외는 <see cref="ThreadFailed"/>로 알린다 (원본은 프로그램이 죽었다).</item>
    /// </list>
    /// UI 스레드는 생성자에 넘긴 SynchronizationContext (WPF는 DispatcherSynchronizationContext).
    /// </summary>
    public sealed class DeviceHost : IDeviceHost
    {
        /// <summary>창 메시지 처리기: (msg, wParam, lParam) → 반환값 (원본 LRESULT)</summary>
        public delegate int WindowProc(int msg, int wParam, object lParam);

        private readonly SynchronizationContext _ui;
        private readonly int _uiThreadId;
        private readonly Func<string, string, uint, int> _messageBox;
        private readonly Dictionary<int, WindowProc> _windows = new Dictionary<int, WindowProc>();
        private readonly object _lock = new object();

        /// <param name="uiContext">UI 스레드의 SynchronizationContext (이 생성자를 UI 스레드에서 부른다)</param>
        /// <param name="messageBox">원본 ::MessageBox(text, caption, type) → IDOK 등. UI 스레드에서 부른다.</param>
        public DeviceHost(SynchronizationContext uiContext, Func<string, string, uint, int> messageBox)
        {
            _ui = uiContext ?? throw new ArgumentNullException(nameof(uiContext));
            _uiThreadId = Thread.CurrentThread.ManagedThreadId;
            _messageBox = messageBox ?? throw new ArgumentNullException(nameof(messageBox));
        }

        /// <summary>스레드 본문에서 처리되지 않은 예외 (스레드 이름, 예외)</summary>
        public event Action<string, Exception> ThreadFailed;

        /// <summary>처리기가 없는 창 번호로 온 메시지 (hWnd, msg) — 원본에서는 버려졌다</summary>
        public event Action<int, int> UnhandledMessage;

        /// <summary>창 번호 hWnd(원본 HWND 자리)에 메시지 처리기를 붙인다. null이면 뗀다.</summary>
        public void RegisterWindow(int hWnd, WindowProc proc)
        {
            lock (_lock)
            {
                if (proc == null) _windows.Remove(hWnd);
                else _windows[hWnd] = proc;
            }
        }

        private bool OnUiThread => Thread.CurrentThread.ManagedThreadId == _uiThreadId;

        private int Dispatch(int hWnd, int msg, int wParam, object lParam)
        {
            WindowProc proc;
            lock (_lock) _windows.TryGetValue(hWnd, out proc);
            if (proc == null)
            {
                UnhandledMessage?.Invoke(hWnd, msg);
                return 0;
            }
            return proc(msg, wParam, lParam);
        }

        private T OnUi<T>(Func<T> f)
        {
            if (OnUiThread) return f();
            T result = default(T);
            Exception error = null;
            _ui.Send(_ =>
            {
                try { result = f(); }
                catch (Exception ex) { error = ex; }
            }, null);
            if (error != null) throw new InvalidOperationException("UI 스레드 처리 중 예외", error);
            return result;
        }

        public int MessageBox(int hWnd, string text, string caption, uint type) => OnUi(() => _messageBox(text, caption, type));

        public int SendMessage(int hWnd, int msg, int wParam, object lParam) => OnUi(() => Dispatch(hWnd, msg, wParam, lParam));

        public bool PostMessage(int hWnd, int msg, int wParam, object lParam)
        {
            _ui.Post(_ => Dispatch(hWnd, msg, wParam, lParam), null);
            return true;
        }

        public void Sleep(int milliseconds) => Thread.Sleep(Math.Max(0, milliseconds));

        public uint GetTickCount() => unchecked((uint)Environment.TickCount);

        public object BeginThread(Func<object, uint> proc, object param)
        {
            string name = proc.Method.Name;
            var t = new Thread(() =>
            {
                try
                {
                    proc(param);
                }
                catch (Exception ex)
                {
                    ThreadFailed?.Invoke(name, ex);
                }
            })
            {
                IsBackground = true,
                Name = name,
            };
            t.Start();
            return t;
        }
    }
}
