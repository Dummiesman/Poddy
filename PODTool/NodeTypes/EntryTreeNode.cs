using System;

namespace PODTool
{
    public class EntryTreeNode : PoddyTreeNodeBase
    {
        public EditorPODEntryData Data { get; private set; }

        public void AuditMoved(string oldPath, string newPath)
        {
            var parent = GetParentArchive();
            var size = Data.Size;
            var timestamp = Data.Timestamp;

            parent.AuditLog.AddRange(AuditLogEntry.CreateMovedEntry(ProgramSettings.AuditLogUsername, oldPath, newPath, timestamp, size));
        }
        
        public void AuditChanged(EditorPODEntryData previousData, EditorPODEntryData newData)
        {
            var parent = GetParentArchive();
            parent.AuditLog.Add(AuditLogEntry.CreateChangedEntry(ProgramSettings.AuditLogUsername, GetFullPath(), 
                previousData.Timestamp, previousData.Size, newData.Timestamp, newData.Size));
        }

        public void AuditAdded()
        {
            var parent = GetParentArchive();
            parent.AuditLog.Add(AuditLogEntry.CreateAddedEntry(ProgramSettings.AuditLogUsername, GetFullPath(), Data.Timestamp, Data.Size));
        }

        public void AuditRemoved()
        {
            var parent = GetParentArchive();
            parent.AuditLog.Add(AuditLogEntry.CreateRemovedEntry(ProgramSettings.AuditLogUsername, GetFullPath(), Data.Timestamp, Data.Size));
        }

        public void Update(EditorPODEntryData newData)
        {
            this.AuditChanged(this.Data, newData);
            this.Data = newData;
            UpdateNodeState();
        }

        public override void UpdateNodeState()
        {
            if(Data is EditorDiskPODEntryData)
            {
                this.BackColor = System.Drawing.Color.Yellow;
            }
            else
            {
                this.BackColor = System.Drawing.Color.White;
            }
            this.ToolTipText = $"{this.Text}\nFile size: {Data.Size} bytes\nLast modified: {this.Data.Timestamp}";
        }

        public EntryTreeNode(string name, EditorPODEntryData entry)
        {
            this.Data = entry;
            this.ImageKey = "document.png";
            this.SelectedImageKey = this.ImageKey;
            this.Text = name;
            UpdateNodeState();
        }
    }
}
