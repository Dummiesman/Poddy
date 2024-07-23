
namespace PODTool
{
    partial class AuditLogViewer
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
            this.logListView = new System.Windows.Forms.ListView();
            this.columnHeaderUser = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderItem = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderAction = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderOldDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderOldSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderNewDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderNewSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // logListView
            // 
            this.logListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderUser,
            this.columnHeaderDate,
            this.columnHeaderItem,
            this.columnHeaderAction,
            this.columnHeaderOldDate,
            this.columnHeaderOldSize,
            this.columnHeaderNewDate,
            this.columnHeaderNewSize});
            this.logListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logListView.FullRowSelect = true;
            this.logListView.GridLines = true;
            this.logListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.logListView.HideSelection = false;
            this.logListView.Location = new System.Drawing.Point(0, 0);
            this.logListView.Name = "logListView";
            this.logListView.Size = new System.Drawing.Size(993, 541);
            this.logListView.TabIndex = 0;
            this.logListView.UseCompatibleStateImageBehavior = false;
            this.logListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderUser
            // 
            this.columnHeaderUser.Text = "User";
            this.columnHeaderUser.Width = 100;
            // 
            // columnHeaderDate
            // 
            this.columnHeaderDate.Text = "Date";
            this.columnHeaderDate.Width = 128;
            // 
            // columnHeaderItem
            // 
            this.columnHeaderItem.Text = "Item";
            this.columnHeaderItem.Width = 300;
            // 
            // columnHeaderAction
            // 
            this.columnHeaderAction.Text = "Action";
            // 
            // columnHeaderOldDate
            // 
            this.columnHeaderOldDate.Text = "Date (Old)";
            this.columnHeaderOldDate.Width = 128;
            // 
            // columnHeaderOldSize
            // 
            this.columnHeaderOldSize.Text = "Size (Old)";
            this.columnHeaderOldSize.Width = 64;
            // 
            // columnHeaderNewDate
            // 
            this.columnHeaderNewDate.Text = "Date (New)";
            this.columnHeaderNewDate.Width = 128;
            // 
            // columnHeaderNewSize
            // 
            this.columnHeaderNewSize.Text = "Size (New)";
            this.columnHeaderNewSize.Width = 64;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 541);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(993, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel.Text = "toolStripStatusLabel1";
            // 
            // AuditLogViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(993, 563);
            this.Controls.Add(this.logListView);
            this.Controls.Add(this.statusStrip1);
            this.Name = "AuditLogViewer";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Audit Log";
            this.Load += new System.EventHandler(this.AuditLogViewer_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView logListView;
        private System.Windows.Forms.ColumnHeader columnHeaderUser;
        private System.Windows.Forms.ColumnHeader columnHeaderDate;
        private System.Windows.Forms.ColumnHeader columnHeaderItem;
        private System.Windows.Forms.ColumnHeader columnHeaderAction;
        private System.Windows.Forms.ColumnHeader columnHeaderOldDate;
        private System.Windows.Forms.ColumnHeader columnHeaderOldSize;
        private System.Windows.Forms.ColumnHeader columnHeaderNewDate;
        private System.Windows.Forms.ColumnHeader columnHeaderNewSize;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
    }
}