using PODTool.CRC;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PODTool.POD
{
    class FileHeader
    {
        public int StringOffset;
        public int Offset;
        public int Size;
        public uint CRC;
        public uint Timestamp;
    }

    public class PODWriter
    {
        public string Title = "Untitled POD";
        public string Copyright = string.Empty;
        public string Author = string.Empty;
        public int Priority;
        public int Revision;
        public PODVersion Version = PODVersion.POD2;

        public readonly List<EditorPODEntry> Entries = new List<EditorPODEntry>();
        public readonly List<AuditLogEntry> AuditLog = new List<AuditLogEntry>();

        private byte[] POD3HeaderCRCBuf = new byte[Constants.POD3_HEADER_SIZE - 8];
        private string SavePath;

        private List<EditorPODEntry> GetSortedEntries()
        {
            return Entries.OrderBy(x => x.Name).ToList();
        }

        private void SaveEPD1(Stream stream)
        {
            var entries = GetSortedEntries();
            FileHeader[] headers = new FileHeader[entries.Count];
            for (int i = 0; i < entries.Count; i++) headers[i] = new FileHeader();

            // calculate file info
            int dataOffset = Constants.EPD1_HEADER_SIZE + (Constants.EPD1_ENTRY_SIZE * headers.Length);
            for (int i = 0; i < entries.Count; i++)
            {
                var entry = entries[i];
                headers[i].Offset = dataOffset;
                headers[i].Size = entry.Data.Size;
                dataOffset += headers[i].Size;
            }

            var crcTask = Parallel.For(0, entries.Count, (i) => headers[i].CRC = entries[i].Data.CalculateCRC<EPDCrc32>());
            while (!crcTask.IsCompleted) { }

            // write the EPD
            var wrappedStream = new CrcCalculatingStream<EPDCrc32S>(stream);
            long writeOrigin = stream.Position;
            var writer = new BinaryWriter(wrappedStream, Encoding.ASCII);

            writer.Write(0x65787464); //dtxe
            writer.WriteFixedSizeCharArray(Title, 256);

            writer.Write(entries.Count);
            writer.Write(0x00); // version (always 0?)
            writer.Write(0x00); // checksum placeholder

            for (int i = 0; i < entries.Count; i++)
            {
                writer.WriteFixedSizeCharArray(entries[i].Name, 64);
                writer.Write(headers[i].Size);
                writer.Write(headers[i].Offset);
                writer.Write(headers[i].Timestamp);
                writer.Write(headers[i].CRC);
            }

            // write file data
            wrappedStream.AddCRCToStack();
            for (int i=0; i < entries.Count; i++)
            {
                entries[i].Data.Save(writer.BaseStream);
            }

            // write  checksum
            stream.Seek(writeOrigin + 268, SeekOrigin.Begin);
            writer.Write(wrappedStream.PopCRCFromStack());
        }

        private void SavePOD1(Stream stream)
        {
            var entries = GetSortedEntries();
            FileHeader[] headers = new FileHeader[entries.Count];
            for (int i = 0; i < entries.Count; i++) headers[i] = new FileHeader();

            // calculate file info
            int dataOffset = Constants.POD1_HEADER_SIZE + (Constants.POD1_ENTRY_SIZE * headers.Length);
            for (int i = 0; i < entries.Count; i++)
            {
                var entry = entries[i];
                headers[i].Offset = dataOffset;
                headers[i].Size = entry.Data.Size;
                dataOffset += headers[i].Size;
            }

            // write the POD
            var writer = new BinaryWriter(stream, Encoding.ASCII);
           
            writer.Write(entries.Count);
            writer.WriteFixedSizeCharArray(Title, 80);

            for (int i = 0; i < headers.Length; i++)
            {
                writer.WriteFixedSizeCharArray(entries[i].Name, 32);
                writer.Write(headers[i].Size);
                writer.Write(headers[i].Offset);
            }

            for (int i = 0; i < entries.Count; i++)
            {
                entries[i].Data.Save(writer.BaseStream);
            }    
        }

        private void SavePOD2(Stream stream)
        {
            var entries = GetSortedEntries();
            FileHeader[] headers = new FileHeader[entries.Count];
            for (int i = 0; i < entries.Count; i++) headers[i] = new FileHeader();

            // create stringbuf
            var stringBuf = new PODStringBuf(entries);

            // calculate file info
            int entryStart = Constants.POD2_HEADER_SIZE;
            int stringTableStart = entryStart + (Constants.POD2_ENTRY_SIZE * entries.Count);
            int stringTableEnd = stringTableStart + stringBuf.Size;
            int dataOffset = stringTableEnd;

            for (int i = 0; i < entries.Count; i++)
            {
                var entry = entries[i];
                headers[i].Offset = dataOffset;
                headers[i].Size = entry.Data.Size;
                headers[i].Timestamp = (uint)TimeExtensions.ToUnixTime(entry.Data.Timestamp);
                headers[i].StringOffset = stringBuf.EntryOffsets[i];
                dataOffset += headers[i].Size;
            }
            
            var crcTask = Parallel.For(0, entries.Count, (i) => headers[i].CRC = entries[i].Data.CalculateCRC<Crc32CCITT>());
            while (!crcTask.IsCompleted) { }

            // write the POD
            var wrappedStream = new CrcCalculatingStream<Crc32CCITT>(stream);
            long writeOrigin = stream.Position;
            var writer = new BinaryWriter(wrappedStream, Encoding.ASCII);

            writer.Write(0x32444F50); //POD2
            writer.Write(0x00); // checksum placeholder
            wrappedStream.AddCRCToStack();

            writer.WriteFixedSizeCharArray(Title, 80);

            writer.Write(entries.Count);
            writer.Write(AuditLog.Count);

            for (int i = 0; i < entries.Count; i++)
            {
                writer.Write(headers[i].StringOffset);
                writer.Write(headers[i].Size);
                writer.Write(headers[i].Offset);
                writer.Write(headers[i].Timestamp);
                writer.Write(headers[i].CRC);
            }
            writer.Write(stringBuf.Buffer);

            for (int i = 0; i < entries.Count; i++)
            {
                entries[i].Data.Save(writer.BaseStream);
            }

            for(int i=0; i < AuditLog.Count; i++)
            {
                AuditLog[i].Write(writer);
            }

            // write  checksum
            stream.Seek(writeOrigin + 4, SeekOrigin.Begin);
            writer.Write(wrappedStream.PopCRCFromStack());
        }

        private void SavePOD3(Stream stream)
        {
            var entries = GetSortedEntries();
            FileHeader[] headers = new FileHeader[entries.Count];
            for (int i = 0; i < entries.Count; i++) headers[i] = new FileHeader();

            // create stringbuf
            var stringBuf = new PODStringBuf(entries);

            // calculate file locations / other info
            int dataOffset = Constants.POD3_HEADER_SIZE;
            uint directoryCrc = 0xFFFFFFFF;
            uint dependencyRecordCRC = 0xFFFFFFFF;
            uint auditCRC = 0xFFFFFFFF;
            uint headerCRC = 0xFFFFFFFF;

            for (int i = 0; i < entries.Count; i++)
            {
                var entry = entries[i];
                headers[i].Offset = dataOffset;
                headers[i].Size = entry.Data.Size;
                headers[i].Timestamp = (uint)TimeExtensions.ToUnixTime(entry.Data.Timestamp);
                headers[i].StringOffset = stringBuf.EntryOffsets[i];
                dataOffset += headers[i].Size;
            }

            var crcTask = Parallel.For(0, entries.Count, (i) => headers[i].CRC = entries[i].Data.CalculateCRC<Crc32CCITT>());
            while (!crcTask.IsCompleted) { }

            // write the POD
            var wrappedStream = new CrcCalculatingStream<Crc32CCITT>(stream) { CalculateOnRead = true };
            long writeOrigin = stream.Position;
            var writer = new BinaryWriter(wrappedStream, Encoding.ASCII);

            writer.Write(0x33444F50); //POD3
            writer.Write(0x00); // checksum placeholder

            writer.WriteFixedSizeCharArray(Title, 80);

            writer.Write(entries.Count);
            writer.Write(AuditLog.Count);

            writer.Write(Revision);
            writer.Write(Priority);

            writer.WriteFixedSizeCharArray(Author, 80);
            writer.WriteFixedSizeCharArray(Copyright, 80);


            writer.Write(dataOffset); // directory offset
            writer.Write(0x00); // directory hash (TODO)
            writer.Write(stringBuf.Size); // string buf size
            writer.Write(0x00); // dependency record count
            writer.Write(0xFFFFFFFF); // dependency record hash
            writer.Write(0x00); // audit hash

            for (int i = 0; i < entries.Count; i++)
            {
                entries[i].Data.Save(writer.BaseStream);
            }

            wrappedStream.AddCRCToStack();
            for (int i = 0; i < entries.Count; i++)
            {
                writer.Write(headers[i].StringOffset);
                writer.Write(headers[i].Size);
                writer.Write(headers[i].Offset);
                writer.Write(headers[i].Timestamp);
                writer.Write(headers[i].CRC);
            }            
            writer.Write(stringBuf.Buffer);
            directoryCrc = wrappedStream.PopCRCFromStack();

            wrappedStream.AddCRCToStack();
            for (int i = 0; i < AuditLog.Count; i++)
            {
                AuditLog[i].Write(writer);
            }
            auditCRC = wrappedStream.PopCRCFromStack();

            // write  checksums, and then calculate and write the header checksum
            stream.Seek(writeOrigin + 0x10C, SeekOrigin.Begin);
            writer.Write(directoryCrc);
            stream.Seek(writeOrigin + 0x118, SeekOrigin.Begin);
            writer.Write(dependencyRecordCRC);
            writer.Write(auditCRC);

            stream.Seek(8, SeekOrigin.Begin);
            wrappedStream.AddCRCToStack();
            stream.Read(POD3HeaderCRCBuf, 0, POD3HeaderCRCBuf.Length);
            headerCRC = wrappedStream.PopCRCFromStack();
            stream.Seek(4, SeekOrigin.Begin);
            writer.Write(headerCRC);
        }

        private void Save(Stream stream)
        {
            if (Version >= PODVersion.POD4)
            {
                throw new System.Exception($"Saving {Version} is currently unsupported.");
            }

            switch (Version)
            {
                case PODVersion.EPD1:
                    SaveEPD1(stream);
                    break;
                case PODVersion.POD1:
                    SavePOD1(stream);
                    break;
                case PODVersion.POD2:
                    SavePOD2(stream);
                    break;
                case PODVersion.POD3:
                    SavePOD3(stream);
                    break;
            }
        }

        public void Save()
        {
            using(var fs = File.Open(SavePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                Save(fs);
            }
        }

        public PODWriter(string path)
        {
            SavePath = path;
        }
    }
}
