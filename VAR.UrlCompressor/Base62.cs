using System;
using System.Text;

namespace VAR.UrlCompressor
{
    class Base62
    {
        private static string Base62CodingSpace = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        private static bool ReadBit(byte[] bytes, int position, int offset)
        {
            if (offset < 0) { return false; }
            int tempPos = position + offset;
            int bytePosition = tempPos / 8;
            int bitPosition = tempPos % 8;
            if (bytePosition >= bytes.Length) { return false; }
            return (bytes[bytePosition] & (0x01 << (7 - bitPosition))) > 0;
        }

        private static void WriteBit(byte[] bytes, int position, int offset, bool value)
        {
            if (offset < 0) { return; }
            int tempPos = position + offset;
            int bytePosition = tempPos / 8;
            int bitPosition = tempPos % 8;
            if (bytePosition >= bytes.Length) { return; }
            if (value)
            {
                bytes[bytePosition] = (byte)(bytes[bytePosition] | (0x01 << (7 - bitPosition)));
            }
            else
            {
                bytes[bytePosition] = (byte)(bytes[bytePosition] & (0xffffffff - (0x1 << (7 - bitPosition))));
            }
        }
        
        public static string Encode(byte[] original)
        {
            StringBuilder sb = new StringBuilder();
            
            int bitPosition = 0;
            int lenght = original.Length * 8;
            while (bitPosition < lenght)
            {
                int pad = 0;
                if (bitPosition + 6 > lenght)
                {
                    pad = lenght - (bitPosition + 6);
                }
                bool bit0 = ReadBit(original, bitPosition, 0 + pad);
                bool bit1 = ReadBit(original, bitPosition, 1 + pad);
                bool bit2 = ReadBit(original, bitPosition, 2 + pad);
                bool bit3 = ReadBit(original, bitPosition, 3 + pad);
                bool bit4 = ReadBit(original, bitPosition, 4 + pad);
                bool bit5 = ReadBit(original, bitPosition, 5 + pad);

                if (bit0 == true && bit1 == true && bit2 == true && bit3 == true && bit4 == true)
                {
                    sb.Append(Base62CodingSpace[61]);
                    bitPosition += 5;
                    continue;
                }
                if (bit0 == true && bit1 == true && bit2 == true && bit3 == true && bit4 == false)
                {
                    sb.Append(Base62CodingSpace[60]);
                    bitPosition += 5;
                    continue;
                }
                int charIdx = (bit0 ? 0x20 : 0) | (bit1 ? 0x10 : 0) | (bit2 ? 0x08 : 0) | (bit3 ? 0x04 : 0) | (bit4 ? 0x02 : 0) | (bit5 ? 0x01 : 0);
                sb.Append(Base62CodingSpace[charIdx]);
                bitPosition += 6;
            }
            return sb.ToString();
        }

        public static byte[] Decode(string base62)
        {
            byte[] bytes = new byte[base62.Length * 6 / 8];
            int bitPosition = 0;
            for (int i = 0; i < base62.Length; i++)
            {
                int charIdx = Base62CodingSpace.IndexOf(base62[i]);
                if ((i + 1) == base62.Length)
                {
                    // Last symbol
                    int rest = 8 - (bitPosition % 8);
                    if (rest == 8) { throw new Exception("Extra symbol at end"); }
                    if ((charIdx >> rest) > 0) { throw new Exception("Invalid ending symbol"); }
                    int pad = 6 - rest;
                    WriteBit(bytes, bitPosition, 0 - pad, (charIdx & 0x20) > 0);
                    WriteBit(bytes, bitPosition, 1 - pad, (charIdx & 0x10) > 0);
                    WriteBit(bytes, bitPosition, 2 - pad, (charIdx & 0x08) > 0);
                    WriteBit(bytes, bitPosition, 3 - pad, (charIdx & 0x04) > 0);
                    WriteBit(bytes, bitPosition, 4 - pad, (charIdx & 0x02) > 0);
                    WriteBit(bytes, bitPosition, 5 - pad, (charIdx & 0x01) > 0);

                    break;
                }

                if (charIdx == 60)
                {
                    WriteBit(bytes, bitPosition, 0, true);
                    WriteBit(bytes, bitPosition, 1, true);
                    WriteBit(bytes, bitPosition, 2, true);
                    WriteBit(bytes, bitPosition, 3, true);
                    WriteBit(bytes, bitPosition, 4, false);
                    bitPosition += 5;
                }
                else if (charIdx == 61)
                {
                    WriteBit(bytes, bitPosition, 0, true);
                    WriteBit(bytes, bitPosition, 1, true);
                    WriteBit(bytes, bitPosition, 2, true);
                    WriteBit(bytes, bitPosition, 3, true);
                    WriteBit(bytes, bitPosition, 4, true);
                    bitPosition += 5;
                }
                else
                {
                    WriteBit(bytes, bitPosition, 0, (charIdx & 0x20) > 0);
                    WriteBit(bytes, bitPosition, 1, (charIdx & 0x10) > 0);
                    WriteBit(bytes, bitPosition, 2, (charIdx & 0x08) > 0);
                    WriteBit(bytes, bitPosition, 3, (charIdx & 0x04) > 0);
                    WriteBit(bytes, bitPosition, 4, (charIdx & 0x02) > 0);
                    WriteBit(bytes, bitPosition, 5, (charIdx & 0x01) > 0);
                    bitPosition += 6;
                }
            }

            return bytes;
        }
    }
}
