using Microsoft.VisualBasic;
using PODTool.POD;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PODTool
{
    public class PODFileEntry
    {
        public string Name;
        public int Offset;
        public int Size;
        public uint Timestamp;
    }
    
    public class PODFile
    {
        public string Title { get; private set; } = string.Empty;
        public string Author { get; private set; } = string.Empty;
        public string Copyright { get; private set; } = string.Empty;
        public string Path { get; private set; }
        public readonly List<PODFileEntry> Entries = new List<PODFileEntry>();
        public readonly List<AuditLogEntry> AuditLog = new List<AuditLogEntry>();
        public PODVersion Version { get; private set; }
        public int Priority { get; private set; } = 1000;
        public int Revision { get; private set; } = 1000;

        public static bool VersionSupportsAuditLogs(PODVersion version)
        {
            return version >= PODVersion.POD2;
        }

        public static bool VersionSupportsDependencyRecords(PODVersion version)
        {
            return version >= PODVersion.POD3;
        }

        private static PODVersion DetermineVersion(int magic)
        {
            switch (magic)
            {
                case 0x6C617374: // 'tsal'
                case 0x65787464: // 'dtxe'
                    return PODVersion.EPD1;
                case 0x32444F50:
                    return PODVersion.POD2;
                case 0x33444F50:
                    return PODVersion.POD3;
                case 0x34444F50:
                    return PODVersion.POD4;
                case 0x35444F50:
                    return PODVersion.POD5;
                default:
                    return PODVersion.POD1; // Headerless, erroneous detection of this will be caught via checking if the entries count is huge
            }
        }

        private static bool SanityCheckPOD1(Stream stream, int fileCount)
        {
            return fileCount <= 65535 && stream.Length >= (Constants.POD1_HEADER_SIZE + (fileCount * Constants.POD1_ENTRY_SIZE));
        }

        private void LoadEPD(BinaryReader reader)
        {
            reader.BaseStream.Seek(4, SeekOrigin.Begin);
            Title = reader.ReadFixedSizeCharArray(256);

            int entryCount = reader.ReadInt32();
            int version = reader.ReadInt32();
            uint checksum = reader.ReadUInt32();

            for(int i=0; i < entryCount; i++)
            {
                string filePath = reader.ReadFixedSizeCharArray(64);
                int size = reader.ReadInt32();
                int offset = reader.ReadInt32();
                uint timestamp = reader.ReadUInt32();
                uint fileChecksum = reader.ReadUInt32();

                Entries.Add(new PODFileEntry()
                {
                    Size = size,
                    Offset = offset,
                    Name = filePath,
                    Timestamp = timestamp
                });
            }
        }

        private void LoadPOD1(BinaryReader reader)
        {
            reader.BaseStream.Seek(0, SeekOrigin.Begin);

            int fileCount = reader.ReadInt32();
            Title = reader.ReadFixedSizeCharArray(80);

            for (int i = 0; i < fileCount; i++)
            {
                string filePath = reader.ReadFixedSizeCharArray(32);
                int size = reader.ReadInt32();
                int offset = reader.ReadInt32();

                Entries.Add(new PODFileEntry()
                {
                    Size = size,
                    Offset = offset,
                    Name = filePath
                });
            }
        }

        private void LoadPOD2(BinaryReader reader)
        {
            reader.BaseStream.Seek(4, SeekOrigin.Begin);

            int crc = reader.ReadInt32();
            Title = reader.ReadFixedSizeCharArray(80);
            int fileCount = reader.ReadInt32();
            int auditFileCount = reader.ReadInt32();
            int directoryOffset = Constants.POD2_HEADER_SIZE;
            int namesOffset = directoryOffset + (Constants.POD2_ENTRY_SIZE * fileCount);

            // read directory
            reader.BaseStream.Seek(directoryOffset, SeekOrigin.Begin);
            for (int i = 0; i < fileCount; i++)
            {
                int nameOffset = reader.ReadInt32();
                int size = reader.ReadInt32();
                int offset = reader.ReadInt32();

                uint timestamp = reader.ReadUInt32();
                int checksum = reader.ReadInt32();

                long seekBackTo = reader.BaseStream.Position;
                reader.BaseStream.Seek(namesOffset + nameOffset, SeekOrigin.Begin);
                string name = reader.ReadNullTerminatedString();
                reader.BaseStream.Seek(seekBackTo, SeekOrigin.Begin);

                Entries.Add(new PODFileEntry()
                {
                    Size = size,
                    Offset = offset,
                    Name = name,
                    Timestamp = timestamp
                });
            }

            if (Entries.Count == 0)
                return;

            var lastEntry = Entries.Last();
            int auditTrailOffset = lastEntry.Offset + lastEntry.Size;

            reader.BaseStream.Seek(auditTrailOffset, SeekOrigin.Begin);
            for (int i = 0; i < auditFileCount; i++)
            {
                var auditEntry = new AuditLogEntry();
                auditEntry.Read(reader);
                AuditLog.Add(auditEntry);
            }
        }

        private void LoadPOD3(BinaryReader reader)
        {
            reader.BaseStream.Seek(4, SeekOrigin.Begin);
            uint crc = reader.ReadUInt32();

            Title = reader.ReadFixedSizeCharArray(80);
            int fileCount = reader.ReadInt32();
            int auditFileCount = reader.ReadInt32();
            
            Revision = reader.ReadInt32();
            Priority = reader.ReadInt32();
            Author = reader.ReadFixedSizeCharArray(80);
            Copyright = reader.ReadFixedSizeCharArray(80);

            int directoryOffset = reader.ReadInt32();
            int directoryCrc  = reader.ReadInt32(); 
            int stringTableSize = reader.ReadInt32();
            int dependencyRecordCount = reader.ReadInt32();
            int dependencyRecordCrc = reader.ReadInt32(); 
            int auditTrailCrc = reader.ReadInt32();
            int namesOffset = directoryOffset + (Constants.POD3_ENTRY_SIZE * fileCount);

            if (dependencyRecordCount > 0)
            {
                // Dependency records appear to be entirely unused, but just in case
                System.Diagnostics.Debug.WriteLine($"Found dependency records in this POD file!");
            }

            // read directory
            reader.BaseStream.Seek(directoryOffset, SeekOrigin.Begin);
            for (int i = 0; i < fileCount; i++)
            {
                int nameOffset = reader.ReadInt32();
                int size = reader.ReadInt32();
                int offset = reader.ReadInt32();

                uint timestamp = reader.ReadUInt32();
                int checksum = reader.ReadInt32();

                long seekBackTo = reader.BaseStream.Position;
                reader.BaseStream.Seek(namesOffset + nameOffset, SeekOrigin.Begin);
                string name = reader.ReadNullTerminatedString();
                reader.BaseStream.Seek(seekBackTo, SeekOrigin.Begin);

                Entries.Add(new PODFileEntry()
                {
                    Size = size,
                    Offset = offset,
                    Name = name,
                    Timestamp = timestamp
                });
            }

            if (Entries.Count == 0)
                return;

            int auditTrailOffset = stringTableSize + namesOffset + (264 * dependencyRecordCount);
            reader.BaseStream.Seek(auditTrailOffset, SeekOrigin.Begin);
            for(int i=0; i < auditFileCount; i++)
            {
                var auditEntry = new AuditLogEntry();
                auditEntry.Read(reader);
                AuditLog.Add(auditEntry);
            }
        }

        private void Load(Stream stream)
        {
            using (var reader = new BinaryReader(stream, Encoding.ASCII))
            {
                int magic = reader.ReadInt32();
                Version = DetermineVersion(magic);

                if(Version > PODVersion.POD3)
                {
                    throw new Exception($"POD version {Version} is currently unsupported.");
                }

                switch (Version)
                {
                    case PODVersion.POD1:
                        if (!SanityCheckPOD1(stream, magic))
                        {
                            throw new Exception($"This does not appear to be a POD file.");
                        }
                        LoadPOD1(reader);
                        break;
                    case PODVersion.EPD1:
                        LoadEPD(reader);
                        break;
                    case PODVersion.POD3:
                        LoadPOD3(reader);
                        break;
                    case PODVersion.POD2:
                        LoadPOD2(reader);
                        break;
                }
            }
        }

        /// <summary>
        /// Some user made tools created duplicate entries, which are not valid for POD files
        /// Typically these all point to the same data so we can safely discard them
        /// </summary>
        public void RemoveDuplicateEntries()
        {
            HashSet<string> uniqueNames = new HashSet<string>(Entries.Select(x => x.Name));
            for(int i=Entries.Count - 1; i >=0; i--)
            {
                var entry = Entries[i];
                if (!uniqueNames.Remove(entry.Name))
                {
                    Entries.RemoveAt(i);
                }
            }
        }

        public PODFile(string path)
        {
            this.Path = path;
            using (var stream = File.OpenRead(path))
            {
                Load(stream);
            }
            RemoveDuplicateEntries();
        }
    }
}
