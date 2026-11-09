// 실행 인자:
//   --sim           장비 없이 실행 (가짜 FTDI + 합성 SE 영상, RS232 열지 않음)
//   --ini <폴더>     USBINTEGRATEDDATA.INI·CLIENTPUBLICDATA.INI가 있는 폴더 (기본: 실행 파일 폴더, 원본 GetExecutedPath)
using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace NanoImpact.App
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DispatcherUnhandledException += OnUnhandled;

            bool sim = false;
            string ini = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');
            for (int i = 0; i < e.Args.Length; i++)
            {
                if (string.Equals(e.Args[i], "--sim", StringComparison.OrdinalIgnoreCase)) sim = true;
                else if (string.Equals(e.Args[i], "--ini", StringComparison.OrdinalIgnoreCase) && i + 1 < e.Args.Length) ini = Path.GetFullPath(e.Args[++i]).TrimEnd('\\');
            }

            var w = new MainWindow(sim, ini);
            MainWindow = w;
            w.Show();
        }

        private static void OnUnhandled(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.ToString(), "NanoImpact - unhandled exception", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}
