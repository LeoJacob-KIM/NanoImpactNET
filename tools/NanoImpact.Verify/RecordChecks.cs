using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NanoImpact.Core;
using NanoImpact.Core.Native;

namespace NanoImpact.Verify
{
    /// <summary>저장 구조체 검사: 배치 출력(MSVC 프로브와 비교), 원본 INI 왕복.</summary>
    internal static class RecordChecks
    {
        private sealed class Entry
        {
            public string CName;
            public string File;
            public int Size;
            public NativeField[] Layout;
            public Func<NativeRecord> Create;
        }

        private static readonly Entry[] Entries =
        {
            new Entry { CName = "INTEGRATED_COMMAND_STRUCT", File = "USBINTEGRATEDDATA.INI", Size = IntegratedCommandStruct.NativeSize, Layout = IntegratedCommandStruct.Layout, Create = () => new IntegratedCommandStruct() },
            new Entry { CName = "CLIENT_PUBLIC_DATA", File = "CLIENTPUBLICDATA.INI", Size = ClientPublicData.NativeSize, Layout = ClientPublicData.Layout, Create = () => new ClientPublicData() },
            new Entry { CName = "SEM_SCAN_ROTATION_CALIBRATION_DATA", File = "SROTCALIBRATIONDATA.INI", Size = ScanRotationCalibrationData.NativeSize, Layout = ScanRotationCalibrationData.Layout, Create = () => new ScanRotationCalibrationData() },
        };

        /// <summary>probe.cpp와 같은 형식: "T size N", "T.f offset size"</summary>
        public static int PrintLayout()
        {
            foreach (Entry e in Entries)
            {
                Console.WriteLine("{0} size {1}", e.CName, e.Size);
                foreach (NativeField f in e.Layout) Console.WriteLine("{0}.{1} {2} {3}", e.CName, f.Name, f.Offset, f.Size);
            }
            return 0;
        }

        /// <summary>원본 INI 폴더의 세 파일: 크기, 읽고 다시 쓴 바이트 동일, 필드 쓰기가 그 자리만 바꾸는지, 몇 가지 값</summary>
        public static int CheckIni(string dir)
        {
            int fail = 0;
            foreach (Entry e in Entries)
            {
                string path = Path.Combine(dir, e.File);
                byte[] original = File.ReadAllBytes(path);
                NativeRecord rec = e.Create();
                int copied = rec.LoadFile(path);
                byte[] back = rec.ToBytes();
                bool sizeOk = original.Length == e.Size;
                bool same = copied == original.Length && back.SequenceEqual(original);
                Console.WriteLine("{0}: 파일 {1} B, 구조체 {2} B → 크기 {3}, 왕복 {4}", e.File, original.Length, e.Size, sizeOk ? "같음" : "다름", same ? "동일" : "다름");
                if (!sizeOk || !same) fail++;
            }

            fail += CheckWriteTouchesOnlyField();

            var cp = new ClientPublicData();
            cp.LoadFile(Path.Combine(dir, "CLIENTPUBLICDATA.INI"));
            Console.WriteLine("CLIENT_PUBLIC_DATA: dwVersion={0} dwBuild={1} szLastUpdate='{2}' Server={3}:{4} Company='{5}' Model='{6}' CFE COM={7} 마지막 종료='{8}'",
                cp.dwVersion, cp.dwBuild, cp.szLastUpdate.Value, cp.szServerIPAddress.Value, cp.szServerPort.Value, cp.szCompanyName.Value,
                cp.szModelName.Value, cp.nOptionData[Def.IDX_CLIENT_OPTION_INT_SEM_CFE_COM_PORT], cp.szOption32Text1.Value);

            var ic = new IntegratedCommandStruct();
            ic.LoadFile(Path.Combine(dir, "USBINTEGRATEDDATA.INI"));
            Console.WriteLine("INTEGRATED_COMMAND_STRUCT: dwVersion=0x{0:X8} nSemModel={1} dSemModelAmp={2} dSemModelRef={3}",
                ic.dwVersion, ic.nSemModel, ic.dSemModelAmp, ic.dSemModelRef);
            Console.WriteLine("  dAxisMMPerPulse = [{0}]", string.Join(", ", ic.dAxisMMPerPulse.ToArray()));
            Console.WriteLine("  nAxisMaxWidthPulse = [{0}]", string.Join(", ", ic.nAxisMaxWidthPulse.ToArray()));
            Console.WriteLine("  nXBmpPixel = [{0}]", string.Join(", ", ic.nXBmpPixel.ToArray()));

            var sr = new ScanRotationCalibrationData();
            sr.LoadFile(Path.Combine(dir, "SROTCALIBRATIONDATA.INI"));
            int flags = Enumerable.Range(0, sr.bSRotCaliFlag.Length).Count(i => sr.bSRotCaliFlag[i] != 0);
            Console.WriteLine("SEM_SCAN_ROTATION_CALIBRATION_DATA: 보정된 각도 {0}/{1}, nSRotRelayZoomXData[0,0..4] = [{2}]",
                flags, sr.bSRotCaliFlag.Length, string.Join(", ", Enumerable.Range(0, 5).Select(i => sr.nSRotRelayZoomXData[0, i])));

            Console.WriteLine(fail == 0 ? "결과: 통과" : "결과: 실패 " + fail);
            return fail == 0 ? 0 : 1;
        }

        /// <summary>각 형식의 필드에 값을 쓰면 그 필드 바이트만 바뀌는지</summary>
        private static int CheckWriteTouchesOnlyField()
        {
            var rnd = new Random(7);
            var r = new ClientPublicData();
            var fill = new byte[ClientPublicData.NativeSize];
            rnd.NextBytes(fill);
            r.CopyFrom(fill);

            var actions = new List<KeyValuePair<string, Action>>
            {
                new KeyValuePair<string, Action>("dwVersion", () => r.dwVersion = 0xA1B2C3D4),
                new KeyValuePair<string, Action>("dSampleWidth", () => r.dSampleWidth = 12.5),
                new KeyValuePair<string, Action>("wOptionData", () => r.wOptionData[255] = 0xBEEF),
                new KeyValuePair<string, Action>("dOptionData", () => r.dOptionData[3] = -1.25),
                new KeyValuePair<string, Action>("szServerPort", () => r.szServerPort.Value = "7321"),
                new KeyValuePair<string, Action>("szRemark", () => r.szRemark[255] = 0x41),
            };
            int fail = 0;
            foreach (var a in actions)
            {
                byte[] before = r.ToBytes();
                a.Value();
                byte[] after = r.ToBytes();
                NativeField f = ClientPublicData.Layout.Single(x => x.Name == a.Key);
                var changed = Enumerable.Range(0, before.Length).Where(i => before[i] != after[i]).ToList();
                bool ok = changed.Count > 0 && changed.All(i => i >= f.Offset && i < f.Offset + f.Size);
                if (!ok)
                {
                    fail++;
                    Console.WriteLine("  필드 쓰기 실패: {0} (바뀐 바이트 {1}개, 범위 밖 {2}개)", a.Key, changed.Count, changed.Count(i => i < f.Offset || i >= f.Offset + f.Size));
                }
            }
            if (r.dSampleWidth != 12.5 || r.wOptionData[255] != 0xBEEF || r.szServerPort.Value != "7321" || r.dwVersion != 0xA1B2C3D4)
            {
                fail++;
                Console.WriteLine("  필드 다시 읽기 실패");
            }
            Console.WriteLine("필드 쓰기 검사: {0}", fail == 0 ? "통과 (쓴 필드 바이트만 바뀜)" : "실패");
            return fail;
        }
    }
}
