using System;
using System.Text;

namespace NanoImpact.Hardware.ScanBridge
{
    /// <summary>
    /// Scan Bridge 수신 바이트를 메시지로 조립 (원본 AfxScanBridgeDataRead + AfxSbrCheckSumCommand).
    /// 바이트 단위 상태 기계이고, 버퍼 넘침 시 비우는 기준까지 원본과 같게 맞췄다.
    /// 원본과 다른 점 하나: 원본은 명령 찾기 반복문 조건이 `n &lt; 44`(읽은 바이트 위치)여서
    /// 한 번에 읽은 묶음의 44번째 바이트 이후에 끝나는 메시지는 무시됐다. 여기서는 모두 처리한다.
    /// </summary>
    public sealed class SbrReceiveParser
    {
        /// <summary>X/Y/D/S 버퍼 크기 (MAX_FT232H_XYDSStr_BUFFER)</summary>
        private const int FieldBuffer = 64;

        private readonly StringBuilder _cmd = new StringBuilder(2);
        private readonly StringBuilder _x = new StringBuilder(FieldBuffer);
        private readonly StringBuilder _y = new StringBuilder(FieldBuffer);
        private readonly StringBuilder _d = new StringBuilder(FieldBuffer);
        private readonly StringBuilder _s = new StringBuilder(FieldBuffer);
        private char _state = 'x';
        private int _commaCount;

        /// <summary>checksum이 맞고 목록에 있는 명령이 끝났을 때</summary>
        public event Action<SbrMessage> MessageReceived;

        /// <summary>Echo 모드에서 다음 메시지의 명령 글자만 받았을 때 (원본 m_szFT232H_EchoCmdStr)</summary>
        public event Action<string> EchoReceived;

        /// <summary>checksum이 틀린 메시지 (원본은 조용히 버림 — 진단용)</summary>
        public event Action<string> ChecksumFailed;

        /// <summary>
        /// 1이면 다음 ';'에서 메시지를 실행하지 않고 명령 글자만 EchoReceived로 넘긴 뒤 0으로 돌아간다
        /// (원본 m_nFT232H_EchoCommandEnable).
        /// </summary>
        public int EchoCommandEnable { get; set; }

        public void Feed(byte[] buffer, int count)
        {
            for (int n = 0; n < count; n++) Feed(buffer[n]);
        }

        public void Feed(byte bt)
        {
            if (bt >= 'A' && bt <= 'Z')
            {
                if (_cmd.Length < 2)
                {
                    _cmd.Append((char)bt);
                }
                else if (_cmd.ToString() == "TS" && _x.Length > 0 && _x[0] == '0')
                {
                    // TS 명령에서 X가 '0'으로 시작하면 이어지는 대문자는 Y 버퍼로
                    Append(_y, bt, resetAt: 64);
                }
            }
            else if ((bt >= '0' && bt <= '9') || bt == '.' || bt == '-')
            {
                switch (_state)
                {
                    case 'x': Append(_x, bt, resetAt: 64); break;
                    case 'y': Append(_y, bt, resetAt: 63); break; // 원본은 Y만 >= 63에서 비움
                    case 'd': Append(_d, bt, resetAt: 64); break;
                    case 's': Append(_s, bt, resetAt: 64); break;
                }
            }
            else if (bt == ',')
            {
                // "AAxxxxx,yyyyy,dddddd;" — 명령과 X 사이에는 ','가 없다
                _commaCount++;
                if (_commaCount == 1) _state = 'y';
                else if (_commaCount == 2) _state = 'd';
            }
            else if (bt == '?')
            {
                _state = 's';
            }
            else if (bt == ';')
            {
                Complete();
                Reset();
            }
        }

        private static void Append(StringBuilder sb, byte bt, int resetAt)
        {
            sb.Append((char)bt);
            if (sb.Length >= resetAt) sb.Clear(); // 버퍼 초과: 데이터 오류로 보고 비운다
        }

        private void Complete()
        {
            string cmd = _cmd.ToString();
            if (EchoCommandEnable > 0)
            {
                EchoCommandEnable = 0; // 한 번만
                EchoReceived?.Invoke(cmd);
                return;
            }

            if (!ChecksumOk())
            {
                ChecksumFailed?.Invoke(Assemble());
                return;
            }

            int index = SbrCommands.IndexOf(cmd);
            if (index < 0) return;
            MessageReceived?.Invoke(new SbrMessage(cmd, index, _x.ToString(), _y.ToString(), _d.ToString(), _s.ToString()));
        }

        /// <summary>S가 있으면 (명령+X+","+Y+","+D) 문자 합 % 100 == atoi(S). S가 없으면 통과.</summary>
        private bool ChecksumOk()
        {
            if (_s.Length == 0) return true;
            string text = _cmd.ToString() + _x;
            if (_y.Length > 0) text += "," + _y;
            if (_d.Length > 0) text += "," + _d;
            int sum = 0;
            foreach (char c in text) sum += (sbyte)c;
            return CAtoi(_s.ToString()) == sum % 100;
        }

        /// <summary>원본 AfxSbrAssembleCommand 형식의 문자열</summary>
        private string Assemble()
        {
            var sb = new StringBuilder(_cmd.ToString()).Append(_x);
            if (_y.Length > 0) sb.Append(',').Append(_y);
            if (_d.Length > 0) sb.Append(',').Append(_d);
            if (_s.Length > 0) sb.Append('?').Append(_s);
            return sb.Append(';').ToString();
        }

        /// <summary>C atoi: 부호 하나 + 숫자, 그 밖의 글자에서 멈춤</summary>
        internal static int CAtoi(string s)
        {
            int i = 0, sign = 1;
            if (i < s.Length && (s[i] == '-' || s[i] == '+'))
            {
                if (s[i] == '-') sign = -1;
                i++;
            }
            long v = 0;
            while (i < s.Length && s[i] >= '0' && s[i] <= '9')
            {
                v = v * 10 + (s[i] - '0');
                if (v > int.MaxValue) break;
                i++;
            }
            return (int)(sign * v);
        }

        private void Reset()
        {
            _cmd.Clear();
            _x.Clear();
            _y.Clear();
            _d.Clear();
            _s.Clear();
            _commaCount = 0;
            _state = 'x';
        }
    }
}
