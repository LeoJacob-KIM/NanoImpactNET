using System;

namespace NanoImpact.Hardware.ScanBridge
{
    /// <summary>
    /// Scan Bridge 명령 목록 (DefineStructCode.h m_szSbrCommandList). 위치 = 원본 SBR_CMD_* 값.
    /// </summary>
    public static class SbrCommands
    {
        public const int PD = 0;   // Power Down               ( PC <-> Avr )
        public const int AR = 1;   // Air                      ( PC <-> Avr )
        public const int EV = 2;   // Evacuation               ( PC <-> Avr )
        public const int VL = 3;   // Valve On-Off             ( PC --> Avr )
        public const int VS = 4;   // Vacuum State ( Read )    ( PC <-> Avr )
        public const int HV = 5;   // HV SMPS On/Off           ( PC <-> Avr )
        public const int FV = 6;   // Filament Voltage         ( PC --> Avr )
        public const int BV = 7;   // Bias Voltage             ( PC --> Avr )
        public const int AV = 8;   // Gun ACC Voltage          ( PC --> Avr )
        public const int EM = 9;   // Emission Current         ( PC <-> Avr )
        public const int AE = 10;  // Auto EM Current Send     ( PC <-> Avr )
        public const int AT = 11;  // Auto EM Test Send        ( PC <-> Avr )
        public const int RF = 12;  // Read Filament Voltage    ( PC <-> Avr )
        public const int RG = 13;  // Read Gun ACC Voltage     ( PC <-> Avr )
        public const int SN = 14;  // Step Number per 1mm      ( PC <-> Avr )
        public const int RN = 15;  // Read Step Number per 1mm ( PC <-> Avr )
        public const int SA = 16;  // Set Accelation Table     ( PC <-> Avr )
        public const int RA = 17;  // Read Accelation Table    ( PC <-> Avr )
        public const int SP = 18;  // Set  Position            ( PC <-> Avr )
        public const int RP = 19;  // Read Position            ( PC <-> Avr )
        public const int SM = 20;  // Set MaxWidth Go Home     ( PC <-> Avr )
        public const int RM = 21;  // Read Maximum Width       ( PC <-> Avr )
        public const int MH = 22;  // Motion Home              ( PC --> Avr )
        public const int MC = 23;  // Motion Center            ( PC --> Avr )
        public const int MP = 24;  // Motion Pulse             ( PC --> Avr )
        public const int MA = 25;  // Motion Absolute Pulse    ( PC --> Avr )
        public const int MM = 26;  // Motion mm                ( PC --> Avr )
        public const int TS = 27;  // TrackBall State          ( PC <-> Avr )
        public const int MG = 28;  // Mag                      ( PC --> Avr )
        public const int AP = 29;  // Auto Pulse Position Send ( PC <-> Avr )
        public const int PR = 30;  // PosReset CNC             ( PC <-> Avr )
        public const int LT = 31;  // Limit SW Test            ( PC <-> Avr )
        public const int RB = 32;  // Read Backlash Pulse      ( PC <-> Avr )
        public const int ZT = 33;  // ZTopLimitSwPulse         ( PC <-> Avr )
        public const int SB = 34;  // Set Backlash Pulse       ( PC <-> Avr )
        public const int TC = 35;  // TrackBall Count          ( PC <-> Avr )
        public const int LS = 36;  // LimitSW, EmergencyStop   ( PC <-> Avr )
        public const int TB = 37;  // TrackBall Command        ( PC <-> Avr )
        public const int BT = 38;  // BlueTooth Command        ( PC <-> Avr )
        public const int EC = 39;  // Echo Command             ( PC <-- Avr )
        public const int VR = 40;  // H/W Version String       ( PC <-- Avr )
        public const int TM = 41;  // (원본에 SBR_CMD_ 정의 없음)
        public const int UL = 42;  // Sample Load,Unload State ( PC <-> Avr )
        public const int BF = 43;  // Rotary Area Pulse        ( PC <-> Avr )

        private static readonly string[] Names =
        {
            "PD", "AR", "EV", "VL", "VS", "HV", "FV", "BV", "AV", "EM",
            "AE", "AT", "RF", "RG", "SN", "RN", "SA", "RA", "SP", "RP",
            "SM", "RM", "MH", "MC", "MP", "MA", "MM", "TS", "MG", "AP",
            "PR", "LT", "RB", "ZT", "SB", "TC", "LS", "TB", "BT", "EC",
            "VR", "TM", "UL", "BF",
        };

        public static int Count => Names.Length;

        public static string NameOf(int index) => Names[index];

        /// <summary>명령 글자의 앞 2글자로 위치 찾기 (원본 strncmp(list[i], cmd, 2)). 없으면 -1.</summary>
        public static int IndexOf(string command)
        {
            if (command == null) return -1;
            string key = command.Length > 2 ? command.Substring(0, 2) : command;
            return Array.IndexOf(Names, key);
        }
    }
}
