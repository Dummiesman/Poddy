using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PODTool.POD
{
    class PODStringBuf
    {
        private List<int> entryOffsets;
        public IReadOnlyList<int> EntryOffsets => entryOffsets;
        public readonly byte[] Buffer;
        public int Size => Buffer.Length;

        public string GetString(int index)
        {
            if (index < 0 || index >= entryOffsets.Count)
                throw new ArgumentOutOfRangeException("index");
            return GetStringAt(entryOffsets[index]);
        }

        public string GetStringAt(int bufferPosition)
        {
            int endPosition = Buffer.Length;
            for(int i=bufferPosition; i < Buffer.Length; i++)
            {
                if (Buffer[i] == 0x00)
                {
                    endPosition = i;
                    break;
                }
            }
            return Encoding.ASCII.GetString(Buffer, bufferPosition, endPosition - bufferPosition);
        }

        public PODStringBuf(Stream stream, int size)
        {
            Buffer = new byte[size];
            stream.Read(Buffer, 0, size);

            entryOffsets = new List<int>();
            int readCount = 0;
            while(readCount < size)
            {
                entryOffsets.Add(readCount);
                for(int i=readCount; i < size; i++)
                {
                    if (Buffer[i] == 0x00)
                    {
                        readCount = i;
                        continue;
                    }
                }
            }
        }

        public PODStringBuf(List<EditorPODEntry> entries)
        {
            int stringBufSize = 0;
            for (int i = 0; i < entries.Count; i++)
            {
                var entry = entries[i];
                stringBufSize += Encoding.ASCII.GetByteCount(entry.Name) + 1;
            }

            Buffer = new byte[stringBufSize];
            entryOffsets = new List<int>(entries.Count);
            int stringBufOffset = 0;

            for (int i = 0; i < entries.Count; i++)
            {
                var entry = entries[i];
                string name = entry.Name;
                byte[] bytes = Encoding.ASCII.GetBytes(name);

                entryOffsets.Add(stringBufOffset);
                for (int j = 0; j < bytes.Length; j++)
                {
                    Buffer[stringBufOffset++] = bytes[j];
                }
                Buffer[stringBufOffset++] = 0x00;
            }
        }
    }
}
