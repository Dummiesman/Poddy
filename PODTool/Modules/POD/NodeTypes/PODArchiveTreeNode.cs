using PODTool.POD;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PODTool
{
    public class PODArchiveTreeNode : ArchiveTreeNode
    {
        public PODVersion Version = PODVersion.POD2;
        private FileStream LockStream; // Used to lock the file from changes while we're using it

        public override void Reload()
        {
            Load(PathOnDisk);
        }

        public override void Load(string filepath)
        {
            var newPod = new PODFile(filepath);
            CopyMetadataFromPOD(newPod);
            FromRealPOD(newPod, true);
            UpdateNodeState();
        }

        public override void Save(string filepath)
        {
            string savePath = filepath;
            bool saveToTempPath = false;
            if (PathOnDisk == filepath)
            {
                // we may be holding references to portions of the disk file, lets save to a temp path to be sure
                // then delete the disk file and copy ours over
                saveToTempPath = true;
                savePath = Path.GetTempFileName();
            }

            //build up the file tree
            PODWriter writer = new PODWriter(savePath)
            {
                Title = this.Title,
                Author = this.Author,
                Copyright = this.Copyright,
                Priority = this.Priority,
                Revision = this.Revision,
                Version = this.Version
            };
            writer.AuditLog.AddRange(AuditLog);

            foreach (var entryNode in this.EnumEntries(true))
            {
                writer.Entries.Add(new EditorPODEntry() { Name = entryNode.GetFullPath(), Data = entryNode.Data });
            }
            writer.Save();

            // dispose of our old lock, all data we needed has been written
            if (LockStream != null)
                LockStream.Dispose();

            if (saveToTempPath)
            {
                File.Delete(filepath);
                File.Copy(savePath, filepath);
                File.Delete(savePath);
            }

            // re-lock the new saved file
            LockStream = File.OpenRead(filepath);

            ClearDirty();
            PathOnDisk = filepath;
        }

        public void CopyMetadataFromPOD(PODFile podFile)
        {
            this.Title = podFile.Title;
            this.Author = podFile.Author;
            this.Copyright = podFile.Copyright;
            this.Revision = podFile.Revision;
            this.Priority = podFile.Priority;
            this.Version = podFile.Version;
            this.AuditLog.Clear();
            this.AuditLog.AddRange(podFile.AuditLog);
        }

        /// <summary>
        /// Use a real POD as the data
        /// (Can only be done once per node)
        /// </summary>
        /// <param name="replaceMode">If false, this will be appended and an exception will be thrown on a duplicate entry, otherwise entries of the same name are replaced</param>
        public void FromRealPOD(PODFile podFile, bool replaceMode)
        {
            foreach (var entry in podFile.Entries)
            {
                string[] dirTree = entry.Name.Split('\\');
                string entryName = dirTree.Last();
                var convertedEntry = new EditorPartialDiskPODEntryData(podFile.Path, entry.Offset, entry.Size) { Timestamp = TimeExtensions.FromUnixTime(entry.Timestamp) };

                DirectoryTreeNode entryParent = this;

                // create the directory structure up to this entry
                for (int i = 0; i < dirTree.Length - 1; i++)
                {
                    string dir = dirTree[i];
                    var dirNode = entryParent.FindFirstItem(dir) as DirectoryTreeNode;
                    if (dirNode == null)
                    {
                        dirNode = entryParent.CreateDirectory(dir);
                    }
                    entryParent = dirNode;
                }

                // add entry
                var existingEntry = entryParent.FindFirstItem(entryName);
                if (existingEntry != null)
                {
                    if (replaceMode)
                        existingEntry.Remove();
                    else
                        throw new Exception($"POD merge exception: {entry.Name} already exists in the tree");
                }
                entryParent.AddEntry(entryName, convertedEntry, true);
            }

            // lock the pod file we're holding references to
            if (LockStream != null)
                LockStream.Dispose();
            LockStream = File.OpenRead(podFile.Path);
        }

        public PODArchiveTreeNode()
        {
            UpdateNodeState();
        }

        public void Close()
        {
            if (LockStream != null)
                LockStream.Dispose();
            this.Remove();
        }

        public PODArchiveTreeNode(PODFile realPod) : this()
        {
            PathOnDisk = realPod.Path;
            CopyMetadataFromPOD(realPod);
            FromRealPOD(realPod, false);

            UpdateNodeState();
        }
    }
}
