using PODTool.Native;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PODTool
{
    public abstract class PoddyTreeNodeBase : TreeNode
    {
        /// <summary>
        /// Refresh the node UI to reflect its current state
        /// </summary>
        public virtual void UpdateNodeState()
        {

        }

        public string GetFullPath()
        {
            if (this is PODArchiveTreeNode)
                return string.Empty;

            var pathBuilder = new StringBuilder();
            pathBuilder.Append(this.Text);

            var parent = this.Parent;
            while (parent != null && !(parent is PODArchiveTreeNode))
            {
                pathBuilder.Insert(0, $"{parent.Text}\\");
                parent = parent.Parent;
            }

            return pathBuilder.ToString();
        }

        public PODArchiveTreeNode GetParentArchive()
        {
            TreeNode node = this;
            while (node != null && !(node is PODArchiveTreeNode))
            {
                node = node.Parent;
            }
            return node as PODArchiveTreeNode;
        }

        public EntryTreeNode AddEntry(string name, EditorPODEntryData entry, bool surpressAudit = false)
        {
            var node = new EntryTreeNode(name, entry);
            this.Nodes.AddSorted(node);

            if (!surpressAudit)
            {
                node.AuditAdded();
            }
            return node;
        }

        private IEnumerable<KeyValuePair<string, EntryTreeNode>> BuildTreeInternal(string basePath, PoddyTreeNodeBase baseNode)
        {
            foreach (PoddyTreeNodeBase node in baseNode.Nodes)
            {
                string path = string.IsNullOrEmpty(basePath) ? node.Text : $"{basePath}/{node.Text}";
                if (node is DirectoryTreeNode dirNode)
                {
                    foreach (var kvp in BuildTreeInternal(path, dirNode))
                    {
                        yield return kvp;
                    }
                }
                else if (node is EntryTreeNode entryNode)
                {
                    
                    yield return new KeyValuePair<string, EntryTreeNode>(path, entryNode);
                }
            }
        }
        /// <summary>
        /// Compiles a list of (RelativePath, Entry) pairs
        /// </summary>
        public IEnumerable<KeyValuePair<string, EntryTreeNode>> BuildTree()
        {
            if (this is EntryTreeNode entryNode)
            {
                yield return new KeyValuePair<string, EntryTreeNode>($"{this.Text}", entryNode);
            }
            else
            {
                string basePath = string.Empty;
                if(this is DirectoryTreeNode dirNode)
                {
                    basePath = dirNode.Text;
                }

                foreach (var kvp in BuildTreeInternal(basePath, this))
                {
                    yield return kvp;
                }
            }
        }

        public IEnumerable<EntryTreeNode> EnumEntries(bool recursive)
        {
            foreach (PoddyTreeNodeBase child in this.Nodes)
            {
                if (child is DirectoryTreeNode && recursive)
                {
                    foreach (var subEnum in child.EnumEntries(recursive))
                        yield return subEnum;
                }
                else if (child is EntryTreeNode entry)
                {
                    yield return entry;
                }
            }
        }

        public void Delete()
        {
            if (this is PODArchiveTreeNode)
                throw new InvalidOperationException("Cannot delete a POD file node. Use Close() instead.");

            var podNode = this.GetParentArchive();
            if (this is EntryTreeNode entryNode)
            {
                entryNode.AuditRemoved();
            }
            else if (this is DirectoryTreeNode dirNode)
            {
                foreach (var entryNode2 in dirNode.EnumEntries(true))
                {
                    entryNode2.AuditRemoved();
                }
            }

            podNode.MarkDirty();
            this.Remove();
        }

        public void Rename(string newName)
        {
            if (this is PODArchiveTreeNode)
                throw new InvalidOperationException("Cannot rename a POD file node. Use the properties window for this.");

            foreach (TreeNode child in this.Parent.Nodes)
            {
                if (child.Text == newName)
                {
                    throw new Exception($"A file or directory named \"{newName}\" already exists under \"{this.Parent.Text}\".");
                }
            }

            if (this is EntryTreeNode entryNode)
            {
                string oldPath = entryNode.GetFullPath();

                this.Text = newName;

                string newPath = entryNode.GetFullPath();
                entryNode.AuditMoved(oldPath, newPath);
                entryNode.GetParentArchive().MarkDirty();
            }
            else if (this is DirectoryTreeNode dirNode)
            {
                var entries = dirNode.EnumEntries(true).ToArray();
                string[] entryOldPaths = entries.Select(x => x.GetFullPath()).ToArray();

                this.Text = newName;

                string[] entryNewPaths = entries.Select(x => x.GetFullPath()).ToArray();
                for (int i = 0; i < entries.Length; i++)
                {
                    entries[i].AuditMoved(entryOldPaths[i], entryNewPaths[i]);
                }
                dirNode.GetParentArchive().MarkDirty();
            }
            this.SortSingle();
        }

        public void SetParent(PoddyTreeNodeBase newParent)
        {
            if (this is PODArchiveTreeNode)
                throw new InvalidOperationException("PODFileTreeNode must only be at the root of the tree.");
            if(newParent is EntryTreeNode)
                throw new InvalidOperationException("PODEntryTreeNode cannot have children.");

            foreach (TreeNode child in newParent.Nodes)
            {
                if (child.Text == this.Text)
                {
                    throw new Exception($"A file or directory named \"{this.Text}\" already exists under \"{newParent.Text}\".");
                }
            }
            if (this is EntryTreeNode entryNode)
            {
                string oldPath = entryNode.GetFullPath();

                this.Remove();
                newParent.Nodes.AddSorted(this);

                string newPath = entryNode.GetFullPath();
                entryNode.AuditMoved(oldPath, newPath);
                entryNode.GetParentArchive().MarkDirty();
            }
            else if (this is DirectoryTreeNode dirNode)
            {
                var entries = dirNode.EnumEntries(true).ToArray();
                string[] entryOldPaths = entries.Select(x => x.GetFullPath()).ToArray();

                this.Remove();
                newParent.Nodes.AddSorted(this);

                string[] entryNewPaths = entries.Select(x => x.GetFullPath()).ToArray();
                for (int i = 0; i < entries.Length; i++)
                {
                    entries[i].AuditMoved(entryOldPaths[i], entryNewPaths[i]);
                }
                dirNode.GetParentArchive().MarkDirty();
            }
        }

        /// <summary>
        /// Creats a data object with the neccessary data for dragging into the filesystem
        /// as well as into other places of the tree
        /// </summary>
        public DataObjectEx CreateDataObject()
        {
            // add entries
            var entries = new List<KeyValuePair<string, EntryTreeNode>>(this.BuildTree());

            // setup the virtual file data
            DataObjectEx.SelectedItem[] SelectedItems = new DataObjectEx.SelectedItem[entries.Count];
            for (int i = 0; i < SelectedItems.Length; i++)
            {
                SelectedItems[i].FileName = entries[i].Key.Replace(this.TreeView.PathSeparator, Path.DirectorySeparatorChar.ToString());
                SelectedItems[i].WriteTime = DateTime.Now;
                SelectedItems[i].Data = entries[i].Value.Data;
            }
            DataObjectEx dataObject = new DataObjectEx(SelectedItems);
            dataObject.SetData(NativeMethods.CFSTR_FILEDESCRIPTORW, null);
            dataObject.SetData(NativeMethods.CFSTR_FILECONTENTS, null);
            dataObject.SetData(NativeMethods.CFSTR_PERFORMEDDROPEFFECT, null);
            dataObject.SetData(typeof(PoddyTreeNodeBase), this);
            return dataObject;
        }
    }
}
