namespace PODTool.CRC
{
    /// <summary>
    /// Signed EPD Checksum Algorithm (Used to check whole file)
    /// </summary>
    class EPDCrc32S : ChecksumAlgorithm
    {
        private uint streamPos = 0;
        private uint value = 0;
        private byte lastByte = 0;

        public override uint Value => value;

        public override void AddToChecksum(byte[] bytes, int offset, int length)
        {
            for (int i = offset; i < bytes.Length && i < (offset + length); i++)
            {
                uint localI = ((uint)(i - offset)) + streamPos;
                uint lastByteInc = (uint)(lastByte + 1);
                value = (uint)((localI / lastByteInc) + value + ((sbyte)bytes[i]));
                lastByte = bytes[i];
            }
            streamPos += (uint)length;
        }

        public override void Reset()
        {
            value = 0;
            streamPos = 0;
            lastByte = 0;
        }
    }

    /// <summary>
    /// EPD Checksum Algorithm (For individual file data)
    /// </summary>
    class EPDCrc32 : ChecksumAlgorithm
    {
        private uint streamPos = 0;
        private uint value = 0;
        private byte lastByte = 0;

        public override uint Value => value;

        public override void AddToChecksum(byte[] bytes, int offset, int length)
        {
            for (int i = offset; i < bytes.Length && i < (offset + length); i++)
            {
                uint localI = ((uint)(i - offset)) + streamPos;
                uint lastByteInc = (uint)(lastByte + 1);
                value = (localI / lastByteInc) + value + bytes[i];
                lastByte = bytes[i];
            }
            streamPos += (uint)length;
        }

        public override void Reset()
        {
            value = 0;
            streamPos = 0;
            lastByte = 0;
        }
    }
}
