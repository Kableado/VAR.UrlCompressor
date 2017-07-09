using System;
using System.Text;

namespace VAR.UrlCompressor
{
    class Base62
    {
        private static string Base62CodingSpace = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

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
                bool bit0 = original.ReadBit(bitPosition, 0 + pad);
                bool bit1 = original.ReadBit(bitPosition, 1 + pad);
                bool bit2 = original.ReadBit(bitPosition, 2 + pad);
                bool bit3 = original.ReadBit(bitPosition, 3 + pad);
                bool bit4 = original.ReadBit(bitPosition, 4 + pad);
                bool bit5 = original.ReadBit(bitPosition, 5 + pad);

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
                    bytes.WriteBit(bitPosition, 0 - pad, (charIdx & 0x20) > 0);
                    bytes.WriteBit(bitPosition, 1 - pad, (charIdx & 0x10) > 0);
                    bytes.WriteBit(bitPosition, 2 - pad, (charIdx & 0x08) > 0);
                    bytes.WriteBit(bitPosition, 3 - pad, (charIdx & 0x04) > 0);
                    bytes.WriteBit(bitPosition, 4 - pad, (charIdx & 0x02) > 0);
                    bytes.WriteBit(bitPosition, 5 - pad, (charIdx & 0x01) > 0);

                    break;
                }

                if (charIdx == 60)
                {
                    bytes.WriteBit(bitPosition, 0, true);
                    bytes.WriteBit(bitPosition, 1, true);
                    bytes.WriteBit(bitPosition, 2, true);
                    bytes.WriteBit(bitPosition, 3, true);
                    bytes.WriteBit(bitPosition, 4, false);
                    bitPosition += 5;
                }
                else if (charIdx == 61)
                {
                    bytes.WriteBit(bitPosition, 0, true);
                    bytes.WriteBit(bitPosition, 1, true);
                    bytes.WriteBit(bitPosition, 2, true);
                    bytes.WriteBit(bitPosition, 3, true);
                    bytes.WriteBit(bitPosition, 4, true);
                    bitPosition += 5;
                }
                else
                {
                    bytes.WriteBit(bitPosition, 0, (charIdx & 0x20) > 0);
                    bytes.WriteBit(bitPosition, 1, (charIdx & 0x10) > 0);
                    bytes.WriteBit(bitPosition, 2, (charIdx & 0x08) > 0);
                    bytes.WriteBit(bitPosition, 3, (charIdx & 0x04) > 0);
                    bytes.WriteBit(bitPosition, 4, (charIdx & 0x02) > 0);
                    bytes.WriteBit(bitPosition, 5, (charIdx & 0x01) > 0);
                    bitPosition += 6;
                }
            }

            return bytes;
        }
    }
}
