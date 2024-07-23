namespace PODTool.CRC
{
    public abstract class ChecksumAlgorithm
    {
        public abstract uint Value { get; }

        public abstract void Reset();
        public abstract void AddToChecksum(byte[] bytes, int offset, int length);
    }
}
