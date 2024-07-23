using System;
using System.IO;
using PODTool.CRC;

namespace PODTool
{
    public class EditorPartialDiskPODEntryData : EditorPODEntryData
    {
        private string filePath;
        private int size;
        public override int Size => size;
        public int Offset { get; private set; }
        public string FilePath => filePath;
        

        public override void Save(Stream stream)
        {
            byte[] buf = new byte[Size];
            using(var fileStream = File.OpenRead(filePath))
            {
                fileStream.Seek(Offset, SeekOrigin.Begin);
                fileStream.Read(buf, 0, buf.Length);
                stream.Write(buf, 0, buf.Length);
            }
        }

        public override uint CalculateCRC<T>() 
        {
            var crc = new T();
            byte[] buf = new byte[Size];

            using (var fileStream = File.OpenRead(filePath))
            {
                fileStream.Seek(Offset, SeekOrigin.Begin);
                fileStream.Read(buf, 0, buf.Length);
            }
            crc.AddToChecksum(buf, 0, buf.Length);

            return crc.Value;
        }

        public EditorPartialDiskPODEntryData(string filePath, int offset, int size) : base()
        {
            this.Offset = offset;
            this.size = size;
            this.filePath = filePath;
        }
    }

    public class EditorDiskPODEntryData : EditorPODEntryData
    {
        public string FilePath { get; private set; }
        public override int Size => (int)(new FileInfo(FilePath).Length);

        public override uint CalculateCRC<T>()
        {
            var crc = new T();
            byte[] buf = new byte[1024];
            int readLen;

            using(var fileStream = File.OpenRead(FilePath))
            {
                do
                {
                    readLen = fileStream.Read(buf, 0, buf.Length);
                    crc.AddToChecksum(buf, 0, readLen);
                } while (readLen != 0);
            }

            return crc.Value;
        }

        public override void Save(Stream stream)
        {
            using (var fileStream = File.OpenRead(FilePath))
            {
                fileStream.CopyTo(stream);
            }
        }

        public EditorDiskPODEntryData(string path) 
        {
            this.FilePath = path;
            this.Timestamp = new FileInfo(path).LastWriteTimeUtc;
        }
    }

    public abstract class EditorPODEntryData
    {
        public abstract int Size { get; }

        public abstract void Save(Stream stream);
        public abstract uint CalculateCRC<T>() where T : ChecksumAlgorithm, new();
        public DateTime Timestamp;

        public byte[] ToArray()
        {
            using (var ms = new MemoryStream())
            {
                Save(ms);
                ms.Seek(0, SeekOrigin.Begin);
                return ms.ToArray();
            }
        }

        protected EditorPODEntryData()
        {
            Timestamp = TimeExtensions.Epoch;
        }
    }

    public class EditorPODEntry
    {
        public string Name { get; set; }
        public EditorPODEntryData Data;
    }
}
