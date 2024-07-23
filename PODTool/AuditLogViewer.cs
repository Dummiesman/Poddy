using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace PODTool
{
    public partial class AuditLogViewer : Form
    {
        const string TITLE_BASE = "Audit Log";

        public AuditLogViewer()
        {
            InitializeComponent();
        }

        private string FormatDate(DateTime date)
        {
            if (date == TimeExtensions.Epoch)
                return "---";
            else
                return date.ToLocalTime().ToString();
        }

        private string FormatSize(int size)
        {
            if (size == 0)
                return "---";
            else
                return size.ToString();
        }

        public void Init(string title, IEnumerable<AuditLogEntry> entries)
        {
            if (!string.IsNullOrEmpty(title))
                this.Text = $"{TITLE_BASE} ({title})";
            else
                this.Text = TITLE_BASE;
                
            logListView.Items.Clear();
            logListView.BeginUpdate();
            foreach(var entry in entries.OrderByDescending(x => x.Timestamp))
            {
                var itemData = new string[]
                {
                    entry.User,
                    FormatDate(entry.Timestamp),
                    entry.EntryPath,
                    entry.Action.ToString(),
                    FormatDate(entry.OldTimestamp),
                    FormatSize(entry.OldSize),
                    FormatDate(entry.NewTimestamp),
                    FormatSize(entry.NewSize)
                };

                logListView.Items.Add(new ListViewItem(itemData));
            }
            logListView.EndUpdate();
            toolStripStatusLabel.Text = $"{logListView.Items.Count} entries";
        }

        private void AuditLogViewer_Load(object sender, EventArgs e)
        {

        }
    }
}
