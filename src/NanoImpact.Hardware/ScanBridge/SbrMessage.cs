namespace NanoImpact.Hardware.ScanBridge
{
    /// <summary>
    /// Scan Bridge가 보낸 메시지 하나. 형식: "CC" + X + "," + Y + "," + D + "?" + S + ";"
    /// (CC = 대문자 2글자 명령, X/Y/D = 숫자·'.'·'-', S = checksum). 원본 m_pIntegrated->szComStr/szXStr/szYStr/szDStr/szSStr.
    /// </summary>
    public sealed class SbrMessage
    {
        public SbrMessage(string command, int commandIndex, string x, string y, string d, string s)
        {
            Command = command;
            CommandIndex = commandIndex;
            X = x;
            Y = y;
            D = d;
            S = s;
        }

        /// <summary>2글자 명령 (예: "EV", "RP")</summary>
        public string Command { get; }

        /// <summary>SbrCommands.List 안의 위치 (원본 SBR_CMD_* 값)</summary>
        public int CommandIndex { get; }

        public string X { get; }
        public string Y { get; }
        public string D { get; }
        public string S { get; }

        public override string ToString() => Command + X + "," + Y + "," + D + "?" + S + ";";
    }
}
