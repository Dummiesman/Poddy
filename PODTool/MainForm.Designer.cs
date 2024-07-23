
namespace PODTool
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.fileTreeView = new System.Windows.Forms.TreeView();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.noItemSelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.auditLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editMetadataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.createDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.mainToolStrip = new System.Windows.Forms.ToolStrip();
            this.btnNew = new System.Windows.Forms.ToolStripSplitButton();
            this.emptyFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fromResponseFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOpen = new System.Windows.Forms.ToolStripSplitButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnSaveAll = new System.Windows.Forms.ToolStripButton();
            this.btnSettings = new System.Windows.Forms.ToolStripButton();
            this.btnAbout = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.showContextMenuBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.openPodFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.importFilesDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openResponseFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.contextMenuStrip.SuspendLayout();
            this.mainToolStrip.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileTreeView
            // 
            this.fileTreeView.AllowDrop = true;
            this.fileTreeView.ContextMenuStrip = this.contextMenuStrip;
            this.fileTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileTreeView.HideSelection = false;
            this.fileTreeView.ImageKey = "document.png";
            this.fileTreeView.ImageList = this.imageList;
            this.fileTreeView.LabelEdit = true;
            this.fileTreeView.Location = new System.Drawing.Point(0, 35);
            this.fileTreeView.Name = "fileTreeView";
            this.fileTreeView.SelectedImageIndex = 0;
            this.fileTreeView.ShowNodeToolTips = true;
            this.fileTreeView.Size = new System.Drawing.Size(446, 323);
            this.fileTreeView.TabIndex = 0;
            this.fileTreeView.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.fileTreeView_AfterLabelEdit);
            this.fileTreeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.fileTreeView_ItemDrag);
            this.fileTreeView.DragDrop += new System.Windows.Forms.DragEventHandler(this.fileTreeView_DragDrop);
            this.fileTreeView.DragOver += new System.Windows.Forms.DragEventHandler(this.fileTreeView_DragOver);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noItemSelectedToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator3,
            this.closeToolStripMenuItem,
            this.auditLogToolStripMenuItem,
            this.editMetadataToolStripMenuItem,
            this.toolStripSeparator,
            this.createDirectoryToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.importToolStripMenuItem,
            this.extractToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip1";
            this.contextMenuStrip.Size = new System.Drawing.Size(181, 280);
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // noItemSelectedToolStripMenuItem
            // 
            this.noItemSelectedToolStripMenuItem.Enabled = false;
            this.noItemSelectedToolStripMenuItem.Name = "noItemSelectedToolStripMenuItem";
            this.noItemSelectedToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.noItemSelectedToolStripMenuItem.Text = "No item selected";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = global::PODTool.Properties.Resources.disk;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Image = global::PODTool.Properties.Resources.disk_rename;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(177, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Image = global::PODTool.Properties.Resources.cross_white;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.closeToolStripMenuItem.Text = "Close File";
            this.closeToolStripMenuItem.ToolTipText = "Closes the selected POD file and removes it from the list.";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // auditLogToolStripMenuItem
            // 
            this.auditLogToolStripMenuItem.Image = global::PODTool.Properties.Resources.address_book__pencil;
            this.auditLogToolStripMenuItem.Name = "auditLogToolStripMenuItem";
            this.auditLogToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.auditLogToolStripMenuItem.Text = "Audit Log";
            this.auditLogToolStripMenuItem.ToolTipText = "View the Audit Log associated with the selected POD file.";
            this.auditLogToolStripMenuItem.Click += new System.EventHandler(this.auditLogToolStripMenuItem_Click);
            // 
            // editMetadataToolStripMenuItem
            // 
            this.editMetadataToolStripMenuItem.Image = global::PODTool.Properties.Resources.box__pencil;
            this.editMetadataToolStripMenuItem.Name = "editMetadataToolStripMenuItem";
            this.editMetadataToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.editMetadataToolStripMenuItem.Text = "Properties";
            this.editMetadataToolStripMenuItem.ToolTipText = "Modify the metadata, and file format of the selected POD file.";
            this.editMetadataToolStripMenuItem.Click += new System.EventHandler(this.editMetadataToolStripMenuItem_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(177, 6);
            // 
            // createDirectoryToolStripMenuItem
            // 
            this.createDirectoryToolStripMenuItem.Image = global::PODTool.Properties.Resources.folder__plus;
            this.createDirectoryToolStripMenuItem.Name = "createDirectoryToolStripMenuItem";
            this.createDirectoryToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.createDirectoryToolStripMenuItem.Text = "Create Directory";
            this.createDirectoryToolStripMenuItem.Click += new System.EventHandler(this.createDirectoryToolStripMenuItem_Click);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Image = global::PODTool.Properties.Resources.blue_document__pencil;
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = global::PODTool.Properties.Resources.cross_script;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Image = global::PODTool.Properties.Resources.blue_document_import;
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.importToolStripMenuItem.Text = "Import...";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // extractToolStripMenuItem
            // 
            this.extractToolStripMenuItem.Image = global::PODTool.Properties.Resources.blue_document_export;
            this.extractToolStripMenuItem.Name = "extractToolStripMenuItem";
            this.extractToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.extractToolStripMenuItem.Text = "Export...";
            this.extractToolStripMenuItem.Click += new System.EventHandler(this.extractToolStripMenuItem_Click);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "blank_16x16.png");
            this.imageList.Images.SetKeyName(1, "folder.png");
            this.imageList.Images.SetKeyName(2, "document.png");
            this.imageList.Images.SetKeyName(3, "box.png");
            this.imageList.Images.SetKeyName(4, "box--pencil.png");
            this.imageList.Images.SetKeyName(5, "cross-circle.png");
            // 
            // mainToolStrip
            // 
            this.mainToolStrip.AutoSize = false;
            this.mainToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNew,
            this.btnOpen,
            this.btnSave,
            this.btnSaveAll,
            this.btnSettings,
            this.btnAbout,
            this.toolStripSeparator1,
            this.showContextMenuBtn,
            this.toolStripButton1});
            this.mainToolStrip.Location = new System.Drawing.Point(0, 0);
            this.mainToolStrip.Name = "mainToolStrip";
            this.mainToolStrip.Padding = new System.Windows.Forms.Padding(5, 0, 1, 0);
            this.mainToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.mainToolStrip.Size = new System.Drawing.Size(446, 35);
            this.mainToolStrip.TabIndex = 1;
            this.mainToolStrip.Text = "toolStrip1";
            // 
            // btnNew
            // 
            this.btnNew.AutoToolTip = false;
            this.btnNew.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.emptyFileToolStripMenuItem,
            this.fromResponseFileToolStripMenuItem});
            this.btnNew.Image = global::PODTool.Properties.Resources.document;
            this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(63, 32);
            this.btnNew.Text = "New";
            this.btnNew.ButtonClick += new System.EventHandler(this.btnNew_Click);
            // 
            // emptyFileToolStripMenuItem
            // 
            this.emptyFileToolStripMenuItem.Name = "emptyFileToolStripMenuItem";
            this.emptyFileToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.emptyFileToolStripMenuItem.Text = "Empty File";
            this.emptyFileToolStripMenuItem.Click += new System.EventHandler(this.emptyFileToolStripMenuItem_Click);
            // 
            // fromResponseFileToolStripMenuItem
            // 
            this.fromResponseFileToolStripMenuItem.Name = "fromResponseFileToolStripMenuItem";
            this.fromResponseFileToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.fromResponseFileToolStripMenuItem.Text = "From Response File";
            this.fromResponseFileToolStripMenuItem.Click += new System.EventHandler(this.fromResponseFileToolStripMenuItem_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.AutoToolTip = false;
            this.btnOpen.Image = global::PODTool.Properties.Resources.folder_open_document;
            this.btnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(68, 32);
            this.btnOpen.Text = "Open";
            this.btnOpen.ButtonClick += new System.EventHandler(this.btnOpen_ButtonClick);
            this.btnOpen.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.btnOpen_DropDownItemClicked);
            // 
            // btnSave
            // 
            this.btnSave.AutoToolTip = false;
            this.btnSave.Image = global::PODTool.Properties.Resources.disk;
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(119, 32);
            this.btnSave.Text = "Save Selected File";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSaveAll
            // 
            this.btnSaveAll.AutoToolTip = false;
            this.btnSaveAll.Image = global::PODTool.Properties.Resources.disks;
            this.btnSaveAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveAll.Name = "btnSaveAll";
            this.btnSaveAll.Size = new System.Drawing.Size(68, 32);
            this.btnSaveAll.Text = "Save All";
            this.btnSaveAll.Click += new System.EventHandler(this.btnSaveAll_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSettings.Image = global::PODTool.Properties.Resources.gear;
            this.btnSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(23, 32);
            this.btnSettings.Text = "Settings";
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAbout.Image = global::PODTool.Properties.Resources.question_white;
            this.btnAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(23, 32);
            this.btnAbout.Text = "About";
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 35);
            // 
            // showContextMenuBtn
            // 
            this.showContextMenuBtn.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.showContextMenuBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.showContextMenuBtn.Image = global::PODTool.Properties.Resources.ui_menu_blue;
            this.showContextMenuBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.showContextMenuBtn.Name = "showContextMenuBtn";
            this.showContextMenuBtn.Size = new System.Drawing.Size(23, 32);
            this.showContextMenuBtn.Text = "Additional Options";
            this.showContextMenuBtn.Click += new System.EventHandler(this.showContextMenuBtn_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = global::PODTool.Properties.Resources.tick_circle;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(89, 20);
            this.toolStripButton1.Text = "Validate File";
            this.toolStripButton1.Visible = false;
            // 
            // openPodFileDialog
            // 
            this.openPodFileDialog.FileName = "...";
            this.openPodFileDialog.Filter = "Supported files|*.pod;*.epd";
            this.openPodFileDialog.Multiselect = true;
            this.openPodFileDialog.Title = "Open POD File";
            this.openPodFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openPodFileDialog_FileOk);
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.Description = "Select extraction path";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 358);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(446, 24);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblStatus.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenInner;
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(46, 19);
            this.lblStatus.Text = "Ready.";
            // 
            // importFilesDialog
            // 
            this.importFilesDialog.Filter = "All files|*";
            this.importFilesDialog.Multiselect = true;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "POD Files|*.POD";
            // 
            // openResponseFileDialog
            // 
            this.openResponseFileDialog.FileName = "...";
            this.openResponseFileDialog.Filter = "Response Files|*.rsp";
            this.openResponseFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openResponseFileDialog_FileOk);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 382);
            this.Controls.Add(this.fileTreeView);
            this.Controls.Add(this.mainToolStrip);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Poddy - POD Manipulation Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.contextMenuStrip.ResumeLayout(false);
            this.mainToolStrip.ResumeLayout(false);
            this.mainToolStrip.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView fileTreeView;
        private System.Windows.Forms.ToolStrip mainToolStrip;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.OpenFileDialog openPodFileDialog;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem extractToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.ToolStripButton btnSaveAll;
        private System.Windows.Forms.ToolStripMenuItem editMetadataToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btnAbout;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem auditLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btnSettings;
        private System.Windows.Forms.OpenFileDialog importFilesDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton showContextMenuBtn;
        private System.Windows.Forms.ToolStripMenuItem noItemSelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripSplitButton btnOpen;
        private System.Windows.Forms.ToolStripSplitButton btnNew;
        private System.Windows.Forms.ToolStripMenuItem fromResponseFileToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openResponseFileDialog;
        private System.Windows.Forms.ToolStripMenuItem emptyFileToolStripMenuItem;
    }
}

