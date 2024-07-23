using System.Collections.Generic;
using System.IO;

namespace PODTool.CRC
{
    public class CrcCalculatingStream<T> : Stream where T : ChecksumAlgorithm, new()
    {
        public bool CalculateOnWrite = true;
        public bool CalculateOnRead = false;

        private List<T> crcStack = new List<T>(16);
        private Stream baseStream;

        public override bool CanRead => baseStream.CanRead;

        public override bool CanSeek => baseStream.CanSeek;

        public override bool CanWrite => baseStream.CanWrite;

        public override long Length => baseStream.Length;

        public override long Position { get => baseStream.Position; set => baseStream.Position = value; }

        public void AddCRCToStack()
        {
            crcStack.Add(new T());
        }

        public uint PopCRCFromStack()
        {
            if(crcStack.Count == 0)
            {
                throw new System.Exception("CRC stack is empty");
            }

            uint crc = crcStack[crcStack.Count - 1].Value;
            crcStack.RemoveAt(crcStack.Count - 1);
            return crc;
        }
        

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (CalculateOnWrite)
            {
                for (int x = crcStack.Count - 1; x >= 0; x--)
                {
                    crcStack[x].AddToChecksum(buffer, offset, count);
                }
            }
            baseStream.Write(buffer, offset, count);
        }

        public override void Flush()
        {
            baseStream.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return baseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            baseStream.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int numRead = baseStream.Read(buffer, offset, count);
            if (CalculateOnRead)
            {
                for (int x = crcStack.Count - 1; x >= 0; x--)
                {
                    crcStack[x].AddToChecksum(buffer, offset, numRead);
                }
            }
            return numRead;
        }

        public CrcCalculatingStream(Stream stream)
        {
            this.baseStream = stream;            
        }
    }
}
