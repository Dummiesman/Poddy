using System;
using System.IO;

namespace PODTool
{
    public enum AuditLogAction
    {
        /// <summary>
        /// Indicates a file was added
        /// </summary>
        Add,

        /// <summary>
        /// Indicates a file was removed
        /// </summary>
        Remove,

        /// <summary>
        /// Indicates a file was changed out with a different one
        /// </summary>
        Change
    }

    public class AuditLogEntry
    {
        public string User = string.Empty;
        public DateTime Timestamp;
        public AuditLogAction Action;
        public string EntryPath;

        public DateTime OldTimestamp;
        public int OldSize;
        public DateTime NewTimestamp;
        public int NewSize;

        public static AuditLogEntry[] CreateMovedEntry(string author, string oldPath, string newPath, DateTime timestamp, int size)
        {
            AuditLogEntry[] entries = new AuditLogEntry[2];
            entries[0] = new AuditLogEntry()
            {
                User = author,
                NewTimestamp = TimeExtensions.Epoch,
                NewSize = 0,
                OldTimestamp = timestamp,
                OldSize = size,
                Action = AuditLogAction.Remove,
                EntryPath = oldPath,
                Timestamp = DateTime.UtcNow
            };

            entries[1] = new AuditLogEntry()
            {
                User = author,
                OldTimestamp = TimeExtensions.Epoch,
                OldSize = 0,
                NewTimestamp = timestamp,
                NewSize = size,
                Action = AuditLogAction.Add,
                EntryPath = newPath,
                Timestamp = DateTime.UtcNow
            };

            return entries;
        }

        public static AuditLogEntry CreateChangedEntry(string author, string entryPath, DateTime oldTimestamp, int oldSize, DateTime newTimestamp, int newSize)
        {
            return new AuditLogEntry()
            {
                User = author,
                OldTimestamp = oldTimestamp,
                OldSize = oldSize,
                NewTimestamp = newTimestamp,
                NewSize = newSize,
                Action = AuditLogAction.Change,
                EntryPath = entryPath,
                Timestamp = DateTime.UtcNow
            };
        }

        public static AuditLogEntry CreateAddedEntry(string author, string entryPath, DateTime newTimestamp, int newSize)
        {
            return new AuditLogEntry()
            {
                User = author,
                OldTimestamp = TimeExtensions.Epoch,
                OldSize = 0,
                NewTimestamp = newTimestamp,
                NewSize = newSize,
                Action = AuditLogAction.Add,
                EntryPath = entryPath,
                Timestamp = DateTime.UtcNow
            };
        }

        public static AuditLogEntry CreateRemovedEntry(string author, string entryPath, DateTime oldTimestamp, int oldSize)
        {
            return new AuditLogEntry()
            {
                User = author,
                NewTimestamp = TimeExtensions.Epoch,
                NewSize = 0,
                OldTimestamp = oldTimestamp,
                OldSize = oldSize,
                Action = AuditLogAction.Remove,
                EntryPath = entryPath,
                Timestamp = DateTime.UtcNow
            };
        }

        public void Read(BinaryReader reader)
        {
            User = reader.ReadFixedSizeCharArray(32);
            Timestamp = TimeExtensions.FromUnixTime(reader.ReadUInt32());
            Action = (AuditLogAction)reader.ReadInt32();
            EntryPath = reader.ReadFixedSizeCharArray(256);

            OldTimestamp = TimeExtensions.FromUnixTime(reader.ReadUInt32());
            OldSize = reader.ReadInt32();
            NewTimestamp = TimeExtensions.FromUnixTime(reader.ReadUInt32());
            NewSize = reader.ReadInt32();
        }

        public void Write(BinaryWriter writer)
        {
            writer.WriteFixedSizeCharArray(User, 32);
            writer.Write((uint)TimeExtensions.ToUnixTime(Timestamp));
            writer.Write((int)Action);
            writer.WriteFixedSizeCharArray(EntryPath, 256);

            writer.Write((uint)TimeExtensions.ToUnixTime(OldTimestamp));
            writer.Write(OldSize);
            writer.Write((uint)TimeExtensions.ToUnixTime(NewTimestamp));
            writer.Write(NewSize);
        }
    }
}
