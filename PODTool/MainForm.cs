using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PODTool.Extensions;
using PODTool.Native;
using PODTool.POD;

namespace PODTool
{
    public partial class MainForm : Form
    {
        private TreeNode lastTargetNode = null;
        private DateTime lastDragHoverTime;

        private bool useOverrideStatus = false;
        private string overrideStatus = string.Empty;
        private string lastStatus = string.Empty;

        public MainForm()
        {
            InitializeComponent();
        }

        private void BeginBlockingOP()
        {
            mainToolStrip.Enabled = fileTreeView.Enabled = false;
        }

        private void EndBlockingOP()
        {
            mainToolStrip.Enabled = fileTreeView.Enabled = true;
        }

        private void UpdateToolbarButtonsState()
        {
            btnSaveAll.Enabled = fileTreeView.Nodes.Count > 0;
            btnSave.Enabled = fileTreeView.Nodes.Count > 0;
        }

        private void SetStatus(string status)
        {
            lastStatus = status;
            if(!useOverrideStatus)
                lblStatus.Text = lastStatus;   
        }

        private void SetOverrideStatus(string status)
        {
            useOverrideStatus = true;
            overrideStatus = status;
            lblStatus.Text = overrideStatus;
        }

        private void ClearOverrideStatus()
        {
            useOverrideStatus = false;
            lblStatus.Text = lastStatus;
        }

        private void CloseFile(PODArchiveTreeNode fileNode)
        {
            if (fileNode.IsDirty)
            {
                var mbr = MessageBox.Show("File has been modified. Save?", "Question", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if(mbr == DialogResult.Cancel)
                {
                    return;
                }
                else if(mbr == DialogResult.Yes)
                {
                    bool saveSuccess = SaveFile(fileNode);
                    if (!saveSuccess)
                    {
                        return;
                    }
                }
            }

            fileNode.Close();
            UpdateToolbarButtonsState();
            SetStatus($"Closed {fileNode.Text}");
        }

        private PODArchiveTreeNode FindOpenFile(string filePath)
        {
            foreach (PODArchiveTreeNode podNode in fileTreeView.Nodes)
            {
                if (podNode.IsSavedOnDisk && podNode.PathOnDisk == filePath)
                {
                    return podNode;
                }
            }
            return null;
        }

        private bool IsFileOpen(string filePath)
        {
            return FindOpenFile(filePath) != null;
        }

        public void OpenFilesFromArgs(string[] args)
        {
            foreach (string arg in args)
            {
                if (File.Exists(arg) && (arg.EndsWith(".pod", StringComparison.OrdinalIgnoreCase) 
                                        || arg.EndsWith(".epd", StringComparison.OrdinalIgnoreCase)))
                {
                    OpenFromDisk(arg);
                }
            }
        }

        /// <summary>
        /// Load a POD or EPD from disk into the tree
        /// </summary>
        /// <returns>true when the file was successfuly loaded, false otherwise</returns>
        private bool OpenFromDisk(string filePath)
        {
            var openFile = FindOpenFile(filePath);
            if(openFile != null)
            {
                openFile.EnsureVisible();
                fileTreeView.SelectedNode = openFile;
                return false;
            }

            try
            {
                var roPod = new PODFile(filePath);
                AddPODTreeNode(roPod);
                UpdateToolbarButtonsState();
                ProgramSettings.AddRecentFile(filePath);
                UpdateRecentFilesList();
                return true;
            }
            catch (Exception ex)
            {
                var fi = new FileInfo(filePath);
                MessageBox.Show($"An error occurred opening \"{fi.Name}\": {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void OpenFilesFromDisk(string[] filePaths)
        {
            int loaded = 0;
            foreach(string path in filePaths)
            {
                if (OpenFromDisk(path))
                {
                    loaded++;
                }
            }
            SetStatus($"Opened {loaded} file(s)");
        }

        /// <summary>
        /// Add a POD node merged with podFiles entries
        /// </summary>
        private PODArchiveTreeNode AddPODTreeNode(PODFile podFile)
        {
            var node = new PODArchiveTreeNode(podFile);
            fileTreeView.Nodes.Add(node);
            return node;
        }

        /// <summary>
        /// Add a new blank POD node 
        /// </summary>
        private PODArchiveTreeNode AddPODTreeNode()
        {
            var node = new PODArchiveTreeNode() { Version = ProgramSettings.DefaultPODVersion };
            fileTreeView.Nodes.Add(node);
            return node;
        }

        private void ExtractFromTree(string destination, PoddyTreeNodeBase node)
        {
            if(node == null) return;
            if(node is DirectoryTreeNode dirNode && dirNode.Text == ".")  return;

            foreach(var entry in node.BuildTree())
            {
                string relativePath = entry.Key;
                string expandedPath = Path.Combine(destination, relativePath.Replace('/', Path.DirectorySeparatorChar));
                Directory.CreateDirectory(Path.GetDirectoryName(expandedPath));
                
                if(entry.Value is EntryTreeNode entryNode)
                {
                    using (var stream = File.OpenWrite(expandedPath))
                    {
                        entryNode.Data.Save(stream);
                    }
                }
            }
        }

        private void ExtractFromTree()
        {
            var res = folderBrowserDialog.ShowDialog();
            if (res != DialogResult.OK)
                return;

            ExtractFromTree(folderBrowserDialog.SelectedPath, fileTreeView.SelectedNode as PoddyTreeNodeBase);
            System.Media.SystemSounds.Beep.Play();
        }


        /// <summary>
        // Import files from disk into a node
        /// </summary>
        /// <returns>The number of successfully imported files</returns>
        private int ImportFiles(PoddyTreeNodeBase parentNode, string[] filePaths)
        {
            if(parentNode is DirectoryTreeNode parentDir)
            {
                return parentDir.ImportFiles(filePaths);
            }
            return 0;
        }

        private void DeleteFileOrDirectory(PoddyTreeNodeBase node, bool skipConfirmation = false)
        {
            if (!skipConfirmation)
            {
                var mbr = MessageBox.Show($"Are you sure you want to delete \"{node.GetFullPath()}\"", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (mbr == DialogResult.No)
                {
                    return;
                }
            }

            node.Delete();
        }

        /// <summary>
        /// The final function in the save chain
        /// </summary>
        /// <returns>true if the save went through, false on failure or cancel.</returns>
        private bool SaveFile(PODArchiveTreeNode podNode, string path)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                // save and re-init tree from new data
                bool wasSaved = podNode.IsSavedOnDisk;
                podNode.Save(path);
                podNode.Reload();
                if (!wasSaved)
                {
                    ProgramSettings.AddRecentFile(path);
                    UpdateRecentFilesList();
                }
                SetStatus($"Saved {Path.GetFileName(path)}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save \"{podNode.Text}\": {ex.Message}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            System.Diagnostics.Debug.WriteLine($"Took {sw.ElapsedMilliseconds}ms to save");
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="podNode"></param>
        /// <returns>true if the save went through, false on failure or cancel.</returns>
        private bool SaveFileAs(PODArchiveTreeNode podNode)
        {
            // setup save filter
            if (podNode.Version == PODVersion.EPD1)
            {
                saveFileDialog.Filter = "EPD Files|*.EPD";
            }
            else
            {
                saveFileDialog.Filter = "POD Files|*.POD";
            }

            // setup save location and name
            if (podNode.IsSavedOnDisk)
            {
                saveFileDialog.FileName = Path.GetFileNameWithoutExtension(podNode.PathOnDisk);
                saveFileDialog.InitialDirectory = Path.GetDirectoryName(podNode.PathOnDisk);
            }
            else
            {
                saveFileDialog.FileName = string.Join("_", podNode.Title.Split(Path.GetInvalidFileNameChars()));
                saveFileDialog.InitialDirectory = string.Empty;
            }

            // open save dialog
            var dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                if (IsFileOpen(saveFileDialog.FileName))
                {
                    MessageBox.Show($"Failed to save \"{podNode.Text}\": This path you attempted to save to is already open in the tool.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                return SaveFile(podNode, saveFileDialog.FileName);
            }
            return false;
        }

        /// <summary>
        /// Saves the POD file. Will ask user for confirmation to clear audit logs, and ask user to pick a save path if not saved already
        /// </summary>
        /// <param name="podNode"></param>
        /// <returns>true if the save went through, false on failure or cancel./returns>
        private bool SaveFile(PODArchiveTreeNode podNode, bool saveAs = false)
        {
            if (podNode == null)
                return false;
            bool returnValue;
           
            BeginBlockingOP();
            if (saveAs || !podNode.IsSavedOnDisk)
            {
                returnValue = SaveFileAs(podNode);
            }
            else
            {
                returnValue = SaveFile(podNode, podNode.PathOnDisk);
            }
            EndBlockingOP();

            return returnValue;
        }

        private void UpdateRecentFilesList()
        {
            var fileList = ProgramSettings.RecentFiles;
            btnOpen.DropDownButtonWidth = (fileList.Count > 0) ? 11 : 0;
            btnOpen.DropDownItems.Clear();

            var topItem = btnOpen.DropDownItems.Add("Recent Files");
            topItem.Enabled = false;

            int counter = 1;
            for(int i=fileList.Count - 1; i >= 0; i--)
            {
                string file = fileList[i];
                var item = btnOpen.DropDownItems.Add($"{counter} {file}");
                item.Tag = file;
                counter++;
            }
        }


#region Event Handlers
        private void MainForm_Load(object sender, EventArgs e)
        {
            fileTreeView.NodeMouseClick += (sender_, args_) => fileTreeView.SelectedNode = args_.Node;
            fileTreeView.EnableDoubleBuffering();
            UpdateToolbarButtonsState();
            UpdateRecentFilesList();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            fileTreeView.SelectedNode = AddPODTreeNode();
            UpdateToolbarButtonsState();
        }

        private void emptyFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileTreeView.SelectedNode = AddPODTreeNode();
            UpdateToolbarButtonsState();
        }

        private void btnOpen_ButtonClick(object sender, EventArgs e)
        {
            openPodFileDialog.ShowDialog();
        }

        private void btnOpen_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string path = e.ClickedItem.Tag as string;
            if (path == null) return;
            OpenFilesFromDisk(new string[] { e.ClickedItem.Tag as string });
        }

        private void openPodFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            OpenFilesFromDisk(openPodFileDialog.FileNames);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (fileTreeView.SelectedNode is PoddyTreeNodeBase node)
                SaveFile(node.GetParentArchive());
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            bool selectedIsPOD = fileTreeView.SelectedNode is PODArchiveTreeNode;
            bool selectedIsDir = fileTreeView.SelectedNode is DirectoryTreeNode;
            bool selectedIsEntry = fileTreeView.SelectedNode is EntryTreeNode;
            bool selectedIsAnything = fileTreeView.SelectedNode != null;

            toolStripSeparator.Visible = selectedIsAnything && selectedIsPOD;
            toolStripSeparator3.Visible = selectedIsAnything && selectedIsPOD;

            closeToolStripMenuItem.Visible = selectedIsAnything && selectedIsPOD;
            editMetadataToolStripMenuItem.Visible = selectedIsAnything && selectedIsPOD;
            auditLogToolStripMenuItem.Visible = selectedIsAnything && selectedIsPOD;
            saveToolStripMenuItem.Visible = selectedIsAnything && selectedIsPOD;
            saveAsToolStripMenuItem.Visible = selectedIsAnything && selectedIsPOD;
            renameToolStripMenuItem.Visible = selectedIsAnything && !selectedIsPOD;
            deleteToolStripMenuItem.Visible = selectedIsAnything && !selectedIsPOD;

            createDirectoryToolStripMenuItem.Visible = selectedIsAnything && (selectedIsDir || selectedIsPOD);

            importToolStripMenuItem.Visible = selectedIsAnything;
            extractToolStripMenuItem.Visible = selectedIsAnything;

            noItemSelectedToolStripMenuItem.Visible = !selectedIsAnything;
        }

        private void editMetadataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var podNode = fileTreeView.SelectedNode as PODArchiveTreeNode;
            
            var metaEdit = new PODMetaEdit();
            metaEdit.Init(podNode);
            metaEdit.ShowDialog();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseFile(fileTreeView.SelectedNode as PODArchiveTreeNode);
        }

        private void extractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExtractFromTree();
        }

        private void fileTreeView_DragOver(object sender, DragEventArgs e)
        {
            Point targetPoint = fileTreeView.PointToClient(new Point(e.X, e.Y));
            PoddyTreeNodeBase targetNode = fileTreeView.GetNodeAt(targetPoint) as PoddyTreeNodeBase;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else if (e.Data.GetDataPresent(typeof(PoddyTreeNodeBase)))
            {
                // cannot move things between files currently
                // maybe in the future if I keep better track of the partial entries
                PoddyTreeNodeBase droppedNode = e.Data.GetData(typeof(PoddyTreeNodeBase)) as PoddyTreeNodeBase;
                if (targetNode == null || targetNode == droppedNode || targetNode.GetParentArchive() != droppedNode.GetParentArchive())
                {
                    e.Effect = DragDropEffects.None;
                }
                else
                {
                    e.Effect = DragDropEffects.Move;
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }

            // if the current drag operation can succeed, select the node under the mouse, and expand if neccessary
            if(targetNode != null && (e.Effect == DragDropEffects.Move || e.Effect == DragDropEffects.Link))
            {
                if (targetNode != lastTargetNode)
                {
                    fileTreeView.SelectedNode = targetNode;
                    lastTargetNode = targetNode;
                    lastDragHoverTime = DateTime.Now;
                }
                else
                {
                    TimeSpan hoverTime = DateTime.Now.Subtract(lastDragHoverTime);
                    if (hoverTime.TotalSeconds > 1d)
                    {
                        targetNode.Expand();
                    }
                }
                fileTreeView.Scroll();
            }
        }

        private void fileTreeView_DragDrop(object sender, DragEventArgs e)
        {
            Point targetPoint = fileTreeView.PointToClient(new Point(e.X, e.Y));
            PoddyTreeNodeBase targetedNode = fileTreeView.GetNodeAt(targetPoint) as PoddyTreeNodeBase;
            if (targetedNode == null)
                targetedNode = fileTreeView.SelectedNode as PoddyTreeNodeBase;

            // drag drop from file system
            object fileDropData = e.Data.GetData(DataFormats.FileDrop);
            if(fileDropData != null)
            {
                string[] files = fileDropData as string[];
                HashSet<string> fileExtensions = new HashSet<string>();

                // firstly check if a POD or group of POD have been dragged on
                foreach (string path in files)
                {
                    FileAttributes attr = File.GetAttributes(path);
                    if (!attr.HasFlag(FileAttributes.Directory))
                    {
                        var fileInfo = new FileInfo(path);
                        fileExtensions.Add(fileInfo.Extension.ToLowerInvariant());
                    }
                }

                bool shouldOpen = fileExtensions.Count == 1 && (fileExtensions.Contains(".pod") || fileExtensions.Contains(".epd"));
                shouldOpen |= fileExtensions.Count == 2 && fileExtensions.Contains(".pod") && fileExtensions.Contains(".epd");
                if (shouldOpen)
                {
                    OpenFilesFromDisk(files);
                }
                else if(targetedNode != null)
                {
                    var newParent = targetedNode;
                    if (newParent is EntryTreeNode)
                        newParent = newParent.Parent as PoddyTreeNodeBase;

                    if (ImportFiles(newParent, files) > 0)
                        newParent.Expand();
                }
            }
            
            // drag drop from tree
            PoddyTreeNodeBase droppedNode = e.Data.GetData(typeof(PoddyTreeNodeBase)) as PoddyTreeNodeBase;
            if(targetedNode != null && droppedNode != null && !(droppedNode is PODArchiveTreeNode))
            {
                var newParent = targetedNode;
                if (newParent is EntryTreeNode)
                    newParent = newParent.Parent as PoddyTreeNodeBase;

                if (newParent != droppedNode.Parent && newParent != droppedNode)
                {
                    try
                    {
                        droppedNode.SetParent(newParent);
                        newParent.Expand();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Move failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void fileTreeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // create data object
                var dragRoot = (PoddyTreeNodeBase)e.Item;
                var dataObject = dragRoot.CreateDataObject();

                // set temp status
                if(dragRoot is PODArchiveTreeNode)
                    SetOverrideStatus($"Dragging item: {dragRoot.Text}...");
                else
                    SetOverrideStatus($"Dragging item: {dragRoot.GetFullPath()}...");

                // and finally start the dragdrop
                fileTreeView.DoDragDrop(dataObject, DragDropEffects.Move | DragDropEffects.Copy);

                // clear temp status
                ClearOverrideStatus();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteFileOrDirectory(fileTreeView.SelectedNode as PoddyTreeNodeBase);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool anyDirty = fileTreeView.Nodes.Cast<PODArchiveTreeNode>().Any(x => x.IsDirty);
            if (anyDirty)
            {
                var mbr = MessageBox.Show("There are unsaved changes to POD files in the list. Are you sure you want to exit?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                e.Cancel = (mbr == DialogResult.No);
            }
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Properties.Resources.AboutMessage, "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = fileTreeView.SelectedNode;
            if (node != null)
                node.BeginEdit();
        }

        private void btnSaveAll_Click(object sender, EventArgs e)
        {
            var mbr = MessageBox.Show("Are you sure you want to save all modified archives in the list?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (mbr == DialogResult.Yes)
            {
                foreach (var node in fileTreeView.Nodes.Cast<PODArchiveTreeNode>().Where(x => x.IsDirty))
                {
                    SaveFile(node);
                }
            }
        }

        private void createDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileTreeView.SelectedNode is DirectoryTreeNode node)
            {
                var newDir = node.CreateDirectory();
                node.Expand();
                newDir.BeginEdit();
            }
        }

        private void fileTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            e.CancelEdit = true;
            if (e.Node is PODArchiveTreeNode) // can't change a POD name this way
                return;
            if (e.Node.Text == e.Label || string.IsNullOrWhiteSpace(e.Label) || string.IsNullOrEmpty(e.Label)) // no same/blank name
                return;

            try
            {
                // apply the new name
                (e.Node as PoddyTreeNodeBase).Rename(e.Label);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Rename failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void auditLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileTreeView.SelectedNode is PODArchiveTreeNode node)
            {
                var auditLogViewer = new AuditLogViewer();
                auditLogViewer.Init(node.Text, node.AuditLog);
                auditLogViewer.ShowDialog();
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            new SettingsForm().ShowDialog();
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var res = importFilesDialog.ShowDialog();
            if (res != DialogResult.OK)
                return;

            if (fileTreeView.SelectedNode is PoddyTreeNodeBase node)
            {
                if(ImportFiles(node, importFilesDialog.FileNames) > 0)
                    node.Expand();
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete)
            {
                if (fileTreeView.SelectedNode is PODArchiveTreeNode)
                    CloseFile(fileTreeView.SelectedNode as PODArchiveTreeNode);
                else if(fileTreeView.SelectedNode != null)
                    DeleteFileOrDirectory(fileTreeView.SelectedNode as PoddyTreeNodeBase, e.Modifiers.HasFlag(Keys.Shift));
            }
            else if(e.KeyCode == Keys.F2)
            {
                if (fileTreeView.SelectedNode != null && !(fileTreeView.SelectedNode is PODArchiveTreeNode))
                    fileTreeView.SelectedNode.BeginEdit();
            }
            else if(e.KeyCode == Keys.O && e.Modifiers.HasFlag(Keys.Control))
            {
                openPodFileDialog.ShowDialog();
            }
            else if(e.KeyCode == Keys.S && e.Modifiers.HasFlag(Keys.Control))
            {
                if (fileTreeView.SelectedNode != null)
                    SaveFile((fileTreeView.SelectedNode as PoddyTreeNodeBase).GetParentArchive());
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileTreeView.SelectedNode is PoddyTreeNodeBase node)
                SaveFile(node.GetParentArchive());
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileTreeView.SelectedNode is PoddyTreeNodeBase node)
                SaveFile(node.GetParentArchive(), true);
        }
        #endregion

        private void showContextMenuBtn_Click(object sender, EventArgs e)
        {
            contextMenuStrip.Show(Cursor.Position);
        }

        private void fromResponseFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BeginBlockingOP();
            openResponseFileDialog.ShowDialog();
            EndBlockingOP();
        }


        private void openResponseFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            var responseFile = new ResponseFile(openResponseFileDialog.FileName);
            string rspDirectory = Path.GetDirectoryName(openResponseFileDialog.FileName);
            string podFullPath = Path.Combine(rspDirectory, responseFile.PODFilename);

            // validate file list
            var missingFiles = responseFile.FileList.Where(x => !File.Exists(x)).ToList();
            if(missingFiles.Count > 0)
            {
                var mbr = MessageBox.Show($"The following files cannot be found on disk:\n{string.Join("\n", missingFiles)}\nContinue?", "Some files don't exist", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (mbr == DialogResult.No) return;
            }

            // check if we need to overwrite
            if (File.Exists(podFullPath))
            {
                var mbr = MessageBox.Show($"File \"{responseFile.PODFilename}\" already exists, overwrite?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (mbr == DialogResult.No) return;
            }

            // delete existing from list 
            var existing = FindOpenFile(podFullPath);
            existing?.Close();
            File.Delete(podFullPath);

            // write pod
            var writer = new PODWriter(podFullPath)
            {
                Title = responseFile.VolumeName
            };

            try
            {
                foreach (var filePath in responseFile.FileList)
                {
                    string realFilePath = Path.Combine(rspDirectory, filePath);
                    var data = new EditorDiskPODEntryData(realFilePath);
                    writer.Entries.Add(new EditorPODEntry() { Name = filePath, Data = data });
                    writer.AuditLog.Add(AuditLogEntry.CreateAddedEntry(ProgramSettings.AuditLogUsername, filePath, data.Timestamp, data.Size));
                }
                writer.Save();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"An error occurred while writing the POD file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // open this new file
            OpenFilesFromDisk(new[] { podFullPath });
        }
    }
}