using System;
using System.Threading;
using NanoImpact.Hardware.Ftdi;

namespace NanoImpact.Hardware.ScanBridge
{
    /// <summary>
    /// Scan Bridge 수신 스레드 (원본 AfxCreateScanBridgeThread + AfxThreadScanBridgeProcessing).
    /// FT_EVENT_RXCHAR 이벤트를 250 ms 제한으로 기다리고, 신호가 오면 쌓인 바이트를 읽어 파서에 넣는다.
    /// </summary>
    public sealed class ScanBridgeReader : IDisposable
    {
        /// <summary>이벤트 대기 제한 [ms] (FT232H_EVENT_TIMEOUT)</summary>
        public const int EventTimeoutMs = 250;
        /// <summary>한 번에 읽는 최대 바이트 (MAX_FT232H_READ_BUFFER)</summary>
        public const int ReadBufferSize = 65536;

        private readonly Ft232hPort _port;
        private readonly byte[] _buffer = new byte[ReadBufferSize];
        private AutoResetEvent _rxEvent;
        private Thread _thread;
        private volatile bool _stop;

        public ScanBridgeReader(Ft232hPort port, SbrReceiveParser parser)
        {
            _port = port ?? throw new ArgumentNullException(nameof(port));
            Parser = parser ?? throw new ArgumentNullException(nameof(parser));
        }

        public SbrReceiveParser Parser { get; }

        public bool IsRunning => _thread != null && _thread.IsAlive;

        /// <summary>읽기 실패 등 (원본 RET_ERROR_FT232H_SBR_READ는 반환값으로만 남았다)</summary>
        public event Action<string> Error;

        public bool Start()
        {
            if (IsRunning) return true;
            if (!_port.IsOpen) return false;

            _rxEvent = new AutoResetEvent(false);
            if (!_port.SetRxEvent(_rxEvent))
            {
                _rxEvent.Dispose();
                _rxEvent = null;
                return false;
            }

            _stop = false;
            _thread = new Thread(Run) { IsBackground = true, Name = "ScanBridgeReader" };
            _thread.Start();
            return true;
        }

        public void Stop()
        {
            _stop = true;
            Thread t = _thread;
            if (t != null && t != Thread.CurrentThread) t.Join(EventTimeoutMs * 4);
            _thread = null;
            _rxEvent?.Dispose();
            _rxEvent = null;
        }

        public void Dispose() => Stop();

        private void Run()
        {
            while (!_stop)
            {
                bool signaled;
                try
                {
                    signaled = _rxEvent.WaitOne(EventTimeoutMs);
                }
                catch (ObjectDisposedException)
                {
                    return;
                }
                if (_stop || !_port.IsOpen) return;
                if (signaled) ReadAvailable();
            }
        }

        private void ReadAvailable()
        {
            int rx = _port.GetRxBytes();
            if (rx <= 0) return;
            int read = _port.Read(_buffer, Math.Min(rx, ReadBufferSize));
            if (read < 0)
            {
                Error?.Invoke("Scan Bridge 읽기 실패");
                return;
            }
            try
            {
                Parser.Feed(_buffer, read);
            }
            catch (Exception ex)
            {
                // 처리기 예외가 이 스레드를 죽이면 프로세스가 끝난다 — 알리고 계속 읽는다
                Error?.Invoke("Scan Bridge 메시지 처리 오류: " + ex.Message);
            }
        }
    }
}
