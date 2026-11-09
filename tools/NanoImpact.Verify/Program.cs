// 원본과 같은지 확인하는 도구.
//   NanoImpact.Verify golden <vectors.txt>   → golden.cpp와 같은 형식으로 출력 (두 출력을 diff)
//   NanoImpact.Verify layout                 → 저장 구조체 배치 (tools\layout_probe 출력과 diff)
//   NanoImpact.Verify ini <원본 INI 폴더>     → INI 3개 왕복·필드 쓰기 검사
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using NanoImpact.Hardware.Ftdi;
using NanoImpact.Hardware.ScanBridge;

namespace NanoImpact.Verify
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            try
            {
                if (args.Length >= 2 && args[0] == "golden") return Golden(args[1]);
                if (args.Length >= 1 && args[0] == "layout") return RecordChecks.PrintLayout();
                if (args.Length >= 2 && args[0] == "ini") return RecordChecks.CheckIni(args[1]);
                if (args.Length >= 2 && args[0] == "device") return DeviceHarness.Run(args[1]);
                if (args.Length >= 3 && args[0] == "image") return ImageHarness.Run(args[1], args[2]);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.GetType().FullName + ": " + ex.Message);
                Console.Error.WriteLine(ex.StackTrace);
                return 1;
            }
            Console.Error.WriteLine("사용법: NanoImpact.Verify golden <vectors.txt> | layout | ini <폴더>");
            return 2;
        }

        private static int Golden(string path)
        {
            var parser = new SbrReceiveParser();
            parser.MessageReceived += m => Console.WriteLine("MSG {0} {1}|{2}|{3}|{4}|{5}", m.CommandIndex, m.Command, m.X, m.Y, m.D, m.S);
            parser.ChecksumFailed += _ => Console.WriteLine("BAD");

            foreach (string raw in File.ReadLines(path))
            {
                string line = raw.TrimEnd('\r', '\n');
                if (line.Length == 0 || line[0] == '#') continue;
                string[] t = line.Split(' ');
                switch (t[0])
                {
                    case "CMD":
                        Print("CMD", Ft232hFrames.Command(ushort.Parse(t[1], NumberStyles.HexNumber), int.Parse(t[2], CultureInfo.InvariantCulture)));
                        break;
                    case "AVRV":
                        Print("AVR", Ft232hFrames.Avr(byte.Parse(t[1], NumberStyles.HexNumber), t[2], int.Parse(t[3], CultureInfo.InvariantCulture)));
                        break;
                    case "AVRS":
                        Print("AVR", Ft232hFrames.Avr(byte.Parse(t[1], NumberStyles.HexNumber), t[2]));
                        break;
                    case "RX":
                        byte[] rx = t.Skip(1).Select(h => byte.Parse(h, NumberStyles.HexNumber)).ToArray();
                        parser.Feed(rx, rx.Length);
                        break;
                }
            }
            return 0;
        }

        private static void Print(string tag, byte[] bytes)
        {
            Console.WriteLine(tag + string.Concat(bytes.Select(b => " " + b.ToString("X2"))));
        }
    }
}
