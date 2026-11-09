using System;
using System.Text;

namespace NanoImpact.Hardware.Ftdi
{
    /// <summary>
    /// FT232H로 보내는 바이트 열 만들기. 장비(FPGA/AVR)가 기대하는 형식이라 원본과 바이트 단위로 같아야 한다.
    /// 원본: USB_FT232H_Device.cpp AfxFT232HPortSendCommand / SendMultiCommand / SendAvrCommand.
    /// </summary>
    public static class Ft232hFrames
    {
        /// <summary>AVR 명령 머리 바이트: SE 영상 보드·Scan Bridge (DefineStructCode.h sAVR_COM_FIFO)</summary>
        public const byte AvrComFifo = 0xD3;
        /// <summary>AVR 명령 머리 바이트: BSE 영상 보드 (DefineStructCode.h bAVR_UART_TX)</summary>
        public const byte AvrUartTx = 0xD2;
        /// <summary>FPGA가 쓰는 종료 식별자</summary>
        public const byte FpgaTerminator = 0xFF;

        /// <summary>
        /// 단일 WORD 명령 4바이트. word0 = ((para &gt;&gt; 16)를 WORD로 자른 값 &lt;&lt; 8) | command (WORD로 자름), word1 = (WORD)para.
        /// 원본 그대로: command가 0xEA00처럼 위 바이트를 쓰면 para 위쪽 바이트와 OR로 섞인다.
        /// </summary>
        public static byte[] Command(ushort command, int para)
        {
            ushort hi = (ushort)(para >> 16);
            ushort w0 = (ushort)((hi << 8) | command);
            ushort w1 = (ushort)para;
            return new[] { (byte)w0, (byte)(w0 >> 8), (byte)w1, (byte)(w1 >> 8) };
        }

        /// <summary>WORD 배열을 little-endian 바이트로 (SendMultiCommand: 2 × nWords 바이트)</summary>
        public static byte[] Words(ushort[] words, int count)
        {
            if (count < 0 || count > words.Length) throw new ArgumentOutOfRangeException(nameof(count));
            var bytes = new byte[count * 2];
            Buffer.BlockCopy(words, 0, bytes, 0, bytes.Length);
            return bytes;
        }

        /// <summary>
        /// AVR 명령 + 정수 인자: [head]'0''0''0' + command + value + '?' + checksum + ';' + 0xFF.
        /// checksum = (command+value 문자 합) % 100, 문자는 signed char로 더한다(MSVC char).
        /// </summary>
        public static byte[] Avr(byte head, string command, int value)
        {
            byte[] body = Ascii(command + value.ToString(System.Globalization.CultureInfo.InvariantCulture));
            return AvrFrame(head, body, body.Length, Array.Empty<byte>());
        }

        /// <summary>
        /// AVR 명령 문자열(';'로 끝남): 첫 ';' 앞까지 합으로 checksum, 그 ';'를 '?'로 바꾸고 뒤에 checksum + ';' + 0xFF.
        /// 원본은 ';' 뒤에 남은 글자가 있으면 '?'와 checksum 사이에 그대로 남는다 — 그대로 따른다.
        /// </summary>
        public static byte[] Avr(byte head, string commandWithSemicolon)
        {
            byte[] text = Ascii(commandWithSemicolon);
            int semi = Array.IndexOf(text, (byte)';');
            if (semi < 0) throw new ArgumentException("AVR 명령은 ';'를 포함해야 합니다: " + commandWithSemicolon);
            var rest = new byte[text.Length - semi - 1];
            Array.Copy(text, semi + 1, rest, 0, rest.Length);
            return AvrFrame(head, text, semi, rest);
        }

        private static byte[] AvrFrame(byte head, byte[] body, int sumLength, byte[] afterQuestion)
        {
            int sum = 0;
            for (int i = 0; i < sumLength; i++) sum += (sbyte)body[i];
            sum %= 100;
            byte[] check = Ascii(sum.ToString(System.Globalization.CultureInfo.InvariantCulture));

            // 원본 버퍼는 256바이트(char szData[255+1]); 넘치면 원본은 메모리를 깨뜨리므로 여기서는 막는다.
            int len = 4 + sumLength + 1 + afterQuestion.Length + check.Length + 1 + 1;
            if (len > 255) throw new ArgumentException("AVR 명령이 너무 깁니다 (" + len + " 바이트)");

            var frame = new byte[len];
            int p = 0;
            frame[p++] = head;
            frame[p++] = (byte)'0';
            frame[p++] = (byte)'0';
            frame[p++] = (byte)'0';
            Array.Copy(body, 0, frame, p, sumLength); p += sumLength;
            frame[p++] = (byte)'?';
            Array.Copy(afterQuestion, 0, frame, p, afterQuestion.Length); p += afterQuestion.Length;
            Array.Copy(check, 0, frame, p, check.Length); p += check.Length;
            frame[p++] = (byte)';';
            frame[p] = FpgaTerminator;
            return frame;
        }

        private static byte[] Ascii(string s)
        {
            // 원본은 char* (CP949) — 명령은 ASCII뿐이지만 같은 코드 페이지로 바꾼다.
            return Encoding.GetEncoding(949).GetBytes(s);
        }
    }
}
