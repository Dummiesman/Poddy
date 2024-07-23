using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PODTool
{
    public class DirectoryTreeNode : PoddyTreeNodeBase
    {
        /// <summary>
        /// Creates a subdirectory with the specified name, will throw an exception if it already exists
        /// </summary>
        public DirectoryTreeNode CreateDirectory(string name)
        {
            if (this.Nodes.Cast<TreeNode>().Any(x => x is DirectoryTreeNode && x.Text == name))
            {
                throw new Exception($"Directory already exists: {name}");
            }
            else
            {
                DirectoryTreeNode node = new DirectoryTreeNode(name);
                this.Nodes.AddSorted(node);
                return node;
            }
        }

        /// <summary>
        /// Creates a subdirectory with the name 'New Directory' with a numeric suffix if required
        /// </summary>
        public DirectoryTreeNode CreateDirectory()
        {
            // try to create a new directory
            string dirName = "New Directory";
            int counter = 2;

            while (true)
            {
                if (this.ContainsItem(dirName))
                {
                    dirName = $"New Directory ({counter})";
                    counter++;
                    continue;
                }
                return this.CreateDirectory(dirName);
            }
        }

        /// <summary>
        /// Import files from disk
        /// This method attempts to find the common root and import relative to that
        /// </summary>
        /// <returns>The number of files that were accepted for import</returns>
        public int ImportFiles(string[] filePaths)
        {
            if (filePaths == null || filePaths.Length == 0)
                return 0;

            int numAcceptedFiles = 0;
            List<string> filesToAdd = new List<string>();
            string originPoint = null;

            // preprocess
            foreach (var path in filePaths)
            {
                string pathOrigin;
                FileAttributes attr = File.GetAttributes(path);
                if (attr.HasFlag(FileAttributes.Directory))
                {
                    DirectoryInfo di = new DirectoryInfo(path);
                    filesToAdd.AddRange(Directory.GetFiles(di.FullName, "*", SearchOption.AllDirectories));
                    pathOrigin = di.Parent.FullName;
                }
                else
                {
                    FileInfo fi = new FileInfo(path);
                    filesToAdd.Add(fi.FullName);
                    pathOrigin = fi.Directory.FullName;
                }
                if (originPoint == null || pathOrigin.Length < originPoint.Length)
                {
                    originPoint = pathOrigin;
                }
            }

            var podNode = this.GetParentArchive();
            foreach (var path in filesToAdd)
            {
                string relative = path.Substring(originPoint.Length + 1, path.Length - originPoint.Length - 1);
                string[] hierarchy = relative.Split(Path.DirectorySeparatorChar);
                string entryName = hierarchy.Last();

                var parent = this;
                for (int i = 0; i < hierarchy.Length - 1; i++)
                {
                    string dirName = hierarchy[i];
                    var dirNode = parent.FindFirstItem(dirName) as DirectoryTreeNode;
                    if (dirNode == null)
                    {
                        dirNode = parent.CreateDirectory(dirName);
                    }
                    parent = dirNode;
                }

                var existingEntry = parent.FindFirstItem(entryName) as EntryTreeNode;
                if (existingEntry != null)
                {
                    var mbr = MessageBox.Show($"File \"{entryName}\" already exists, overwrite?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (mbr == DialogResult.No)
                    {
                        continue;
                    }
                }

                var entryData = new EditorDiskPODEntryData(path);
                if (existingEntry != null)
                {
                    existingEntry.Update(entryData);
                }
                else
                {
                    parent.AddEntry(entryName, entryData);
                }
                numAcceptedFiles++;
            }

            if (numAcceptedFiles > 0)
                podNode.MarkDirty();
            return numAcceptedFiles;
        }
        
        public DirectoryTreeNode(string directoryName)
        {
            this.ImageKey = "folder.png";
            this.SelectedImageKey = this.ImageKey;
            this.Name = directoryName;
            this.Text = directoryName;
        }

        protected DirectoryTreeNode()
        {
            // Used for extending directory
        }
    }
}
