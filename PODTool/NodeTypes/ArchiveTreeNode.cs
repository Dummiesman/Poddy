using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PODTool
{
    public class ArchiveTreeNode : DirectoryTreeNode
    {
        public string Title = "Untitled Archive";
        public string Copyright = string.Empty;
        public string Author = string.Empty;
        public int Priority = 1000;
        public int Revision = 1000;
        public readonly List<AuditLogEntry> AuditLog = new List<AuditLogEntry>();

        public string PathOnDisk { get; protected set; }
        public bool IsSavedOnDisk => !string.IsNullOrEmpty(PathOnDisk);
        public bool IsDirty { get; private set; }

        public void ClearDirty()
        {
            IsDirty = false;
            UpdateNodeState();
        }

        public void MarkDirty()
        {
            IsDirty = true;
            UpdateNodeState();
        }

        public void OnFileDeleted()
        {
            // Fires when the archives real disk file is deleted
            var curEntries = EnumEntries(true).ToList();
            foreach (var entry in curEntries)
            {
                if (entry.Data is EditorPartialDiskPODEntryData partialData && partialData.FilePath == this.PathOnDisk)
                    entry.Remove();
            }
            if (EnumEntries(true).Count() == 0)
            {
                this.Remove();
            }
            else
            {
                this.PathOnDisk = string.Empty;
                MarkDirty();
                UpdateNodeState();
            }
        }

        public override void UpdateNodeState()
        {
            string text;
            this.ImageKey = "box.png";
            this.SelectedImageKey = this.ImageKey;

            if (IsSavedOnDisk)
            {
                text = Path.GetFileName(PathOnDisk) + " - " + Title;
            }
            else
            {
                text = "Unsaved - " + Title;
            }

            if (IsDirty)
            {
                text = "* " + text;
            }
            this.Text = text;
        }

        // Virtual methods
        /// <summary>
        /// Reload disk data
        /// </summary>
        public virtual void Reload() { }
        public virtual void Save(string filepath) { }
        public virtual void Load(string filepath) { }
        

        public ArchiveTreeNode() : base()
        {
        }
    }
}
