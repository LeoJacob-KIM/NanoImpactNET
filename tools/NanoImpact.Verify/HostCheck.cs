using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using NanoImpact.Core;
using NanoImpact.Hardware.Device;

namespace NanoImpact.Verify
{
    /// <summary>
    /// 실제 호스트(DeviceHost)와 실제 스레드 연결 검사.
    ///   NanoImpact.Verify hostcheck &lt;원본 INI 폴더&gt;
    /// 1) 다른 스레드의 SendMessage가 UI 스레드에서 실행되고 반환값이 돌아오는지, PostMessage 순서, MessageBox, 스레드 예외 보고.
    /// 2) 장치 계층(SemDevice)을 원본 시작 순서대로 열고 실제 수신 스레드를 돌린 뒤, 가짜 FTDI에 Scan Bridge 응답을 넣으면
    ///    수신 스레드가 읽어 메인 창(hWnd 1)으로 UM_USB_SBR_*를 보내 UI 스레드에서 받는지, 원본 OnClose 순서로 스레드가 끝나는지.
    /// </summary>
    internal static class HostCheck
    {
        private static int _fail;

        private static void Check(bool ok, string what)
        {
            Console.WriteLine((ok ? "OK   " : "FAIL ") + what);
            if (!ok) _fail++;
        }

        public static int Run(string iniFolder)
        {
            using (var ui = new UiThread())
            {
                DeviceHost host = null;
                int uiId = 0;
                ui.Invoke(() =>
                {
                    uiId = Thread.CurrentThread.ManagedThreadId;
                    host = new DeviceHost(SynchronizationContext.Current, (text, caption, type) =>
                    {
                        Check(Thread.CurrentThread.ManagedThreadId == uiId, "MessageBox가 UI 스레드에서 실행: " + text);
                        return 1;
                    });
                });

                // ---- 1) 호스트 동작 ----
                var order = new List<int>();
                int handlerThread = 0;
                ui.Invoke(() => host.RegisterWindow(1, (msg, w, l) =>
                {
                    handlerThread = Thread.CurrentThread.ManagedThreadId;
                    lock (order) order.Add(msg);
                    return msg + w;
                }));

                int ret = 0;
                var done = new ManualResetEventSlim();
                Exception failed = null;
                host.ThreadFailed += (name, ex) => { failed = ex; done.Set(); };
                host.BeginThread(_ =>
                {
                    ret = host.SendMessage(1, 100, 5, null);
                    host.PostMessage(1, 201, 0, null);
                    host.PostMessage(1, 202, 0, null);
                    host.MessageBox(0, "작업 스레드에서", "확인", 0);
                    done.Set();
                    return 0;
                }, null);
                Check(done.Wait(5000), "작업 스레드 끝남");
                ui.Invoke(() => { }); // 앞서 Post한 것이 처리되도록 한 바퀴
                Check(ret == 105, "SendMessage 반환값 (" + ret + ")");
                Check(handlerThread == uiId, "SendMessage 처리기가 UI 스레드에서 실행");
                lock (order) Check(order.Count == 3 && order[0] == 100 && order[1] == 201 && order[2] == 202, "메시지 순서 " + string.Join(",", order));

                done.Reset();
                host.BeginThread(_ => throw new InvalidOperationException("시험 예외"), null);
                Check(done.Wait(5000) && failed is InvalidOperationException, "스레드 예외가 ThreadFailed로 보고됨");

                int unhandled = 0;
                host.UnhandledMessage += (h, m) => unhandled = m;
                host.SendMessage(9, 777, 0, null);
                Check(unhandled == 777, "처리기 없는 창으로 간 메시지 보고");

                // ---- 2) 실제 장치 스레드 ----
                var sbr = new ConcurrentQueue<int>();
                int sbrThread = 0;
                ui.Invoke(() => host.RegisterWindow(1, (msg, w, l) =>
                {
                    sbrThread = Thread.CurrentThread.ManagedThreadId;
                    sbr.Enqueue(msg);
                    return 0;
                }));
                var ftdi = new DeviceHarness.FakeFtdi(TextWriter.Null);
                SemDevice dev = null;
                string cwd = Environment.CurrentDirectory;
                ui.Invoke(() =>
                {
                    // tools\device_harness\scripts\startup.txt와 같은 원본 시작 순서
                    Environment.CurrentDirectory = Path.GetDirectoryName(Path.GetFullPath(iniFolder.TrimEnd('\\', '/')));
                    dev = new SemDevice(host, ftdi);
                    dev.AfxUSBFT232HLoadDLL();
                    dev.AfxCreateIntegratedCommand();
                    dev.AfxSetDeviceServerPath(Path.GetFileName(iniFolder.TrimEnd('\\', '/')));
                    dev.AfxUSBFT232HSetOwnerWindow(1);
                    dev.AfxUSBFT232HSetHWSetupControlWindow(3);
                    dev.AfxUSBFT232HSetStageSetupControlWindow(4);
                    dev.AfxFT232HPortOpen(2);
                    dev.AfxFT232HPortOpen(0);
                    dev.AfxFT232HPortOpen(1);
                    dev.AfxInitilizePublicParameters();
                    dev.AfxFT232HDeviceUse(1);
                    dev.AfxSetSemModel(0, 2.4, 2.0);
                    dev.AfxGetIntegratedDataFromFile();
                    dev.AfxIntegratedSetWDOLSrotZoomingLinearize();
                    dev.AfxGetScanRotationCalibrationDataFromFile();
                    dev.AfxSEMSystemInitilize();
                    dev.AfxCreateCNCSendThread();
                });
                Thread.Sleep(300);
                // Scan Bridge 응답 "PD?48;" (tools\device_harness\scripts\rx.txt 첫 사례: 원본은 UM_USB_SBR_* 5026을 보낸다)
                ftdi.PushRx(2, new byte[] { 0x50, 0x44, 0x3F, 0x34, 0x38, 0x3B });
                var sw = System.Diagnostics.Stopwatch.StartNew();
                while (sbr.IsEmpty && sw.ElapsedMilliseconds < 5000) Thread.Sleep(20);
                Check(sbr.TryPeek(out int first) && first == 5026, "수신 스레드 → 메인 창 메시지 5026 (" + (sbr.IsEmpty ? "없음" : first.ToString()) + ", " + sw.ElapsedMilliseconds + " ms)");
                Check(sbrThread == uiId, "수신 스레드가 보낸 메시지를 UI 스레드에서 받음");

                // 원본 OnClose 순서로 끝내기
                sw.Restart();
                ui.Invoke(() =>
                {
                    dev.AfxIntegratedSetThreadAllStopFlag();
                    dev.AfxFT232HDeviceUse(0);
                    dev.AfxSEMSystemThreadClose();
                    dev.AfxReleaseIntegratedCommand();
                    dev.AfxReleasePublicParameters();
                    dev.AfxFT232HPortClose(2);
                    dev.AfxFT232HPortClose(0);
                    dev.AfxFT232HPortClose(1);
                    Environment.CurrentDirectory = cwd;
                });
                Check(sw.ElapsedMilliseconds < 10000, "종료 순서 끝남 (" + sw.ElapsedMilliseconds + " ms)");
                Check(failed is InvalidOperationException && failed.Message == "시험 예외", "장치 스레드에서 예외 없음");
            }
            Console.WriteLine(_fail == 0 ? "hostcheck: 모두 통과" : "hostcheck: 실패 " + _fail);
            return _fail == 0 ? 0 : 1;
        }

        /// <summary>시험용 UI 스레드: 자기 SynchronizationContext로 큐를 돈다 (WPF Dispatcher 자리)</summary>
        private sealed class UiThread : IDisposable
        {
            private readonly BlockingCollection<Action> _queue = new BlockingCollection<Action>();
            private readonly Thread _thread;

            public UiThread()
            {
                var ready = new ManualResetEventSlim();
                _thread = new Thread(() =>
                {
                    SynchronizationContext.SetSynchronizationContext(new Context(this));
                    ready.Set();
                    foreach (Action a in _queue.GetConsumingEnumerable()) a();
                }) { IsBackground = true, Name = "UI" };
                _thread.Start();
                ready.Wait();
            }

            public void Invoke(Action a)
            {
                if (Thread.CurrentThread == _thread) { a(); return; }
                Exception error = null;
                using (var done = new ManualResetEventSlim())
                {
                    _queue.Add(() => { try { a(); } catch (Exception ex) { error = ex; } finally { done.Set(); } });
                    done.Wait();
                }
                if (error != null) throw new InvalidOperationException("UI 스레드 예외", error);
            }

            public void Post(Action a) => _queue.Add(a);

            public void Dispose()
            {
                _queue.CompleteAdding();
                _thread.Join(2000);
            }

            private sealed class Context : SynchronizationContext
            {
                private readonly UiThread _ui;
                public Context(UiThread ui) { _ui = ui; }
                public override void Send(SendOrPostCallback d, object state) => _ui.Invoke(() => d(state));
                public override void Post(SendOrPostCallback d, object state) => _ui.Post(() => d(state));
            }
        }
    }
}
