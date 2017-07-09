namespace VAR.UrlCompressor
{
    static class ByteExtensions
    {
        public static bool ReadBit(this byte[] bytes, int position, int offset)
        {
            if (offset < 0) { return false; }
            int tempPos = position + offset;
            int bytePosition = tempPos / 8;
            int bitPosition = tempPos % 8;
            if (bytePosition >= bytes.Length) { return false; }
            return (bytes[bytePosition] & (0x01 << (7 - bitPosition))) > 0;
        }

        public static void WriteBit(this byte[] bytes, int position, int offset, bool value)
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
    }
}
