using System;
using System.Windows.Forms;

namespace PODTool
{
    public partial class PODMetaEdit : Form
    {
        private PODArchiveTreeNode fileNode;
        private bool shownAuditWarning = false;
        private bool initialFormatSupportsAudits = false;

        public PODMetaEdit()
        {
            InitializeComponent();
        }

        private void SetFieldsEnabled(PODVersion version)
        {
            textBoxAuthor.Enabled = (version > PODVersion.POD2);
            textBoxCopyright.Enabled = (version > PODVersion.POD2);
            priorityNumericUpDown.Enabled = (version > PODVersion.POD2);
            revisionNumericUpDown.Enabled = (version > PODVersion.POD2);
        }

        public void Init(PODArchiveTreeNode file)
        {
            this.fileNode = file;
            textBoxAuthor.Text = file.Author;
            textBoxTitle.Text = file.Title;
            textBoxCopyright.Text = file.Copyright;
            versionComboBox.SelectedIndex = (int)file.Version;
            revisionNumericUpDown.Value = file.Revision;
            priorityNumericUpDown.Value = file.Priority;

            initialFormatSupportsAudits = PODFile.VersionSupportsAuditLogs(file.Version);
            shownAuditWarning = false;

            SetFieldsEnabled(fileNode.Version);
        }

        private void PODMetaEdit_Load(object sender, EventArgs e)
        {
        }

        private void versionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var newVersion = (PODVersion)versionComboBox.SelectedIndex; 
            SetFieldsEnabled(newVersion);

            if(ProgramSettings.EnableAuditLogWarning && !shownAuditWarning 
                && !PODFile.VersionSupportsAuditLogs(newVersion) && initialFormatSupportsAudits)
            {
                MessageBox.Show(PODTool.Properties.Resources.AuditWarning, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                shownAuditWarning = true;
            }
        }

        private void priorityNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            fileNode.Priority = (int)priorityNumericUpDown.Value;
        }

        private void revisionNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            fileNode.Revision = (int)revisionNumericUpDown.Value;
        }

        private void PODMetaEdit_FormClosed(object sender, FormClosedEventArgs e)
        {
            fileNode.UpdateNodeState();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            int newRevision = (int)revisionNumericUpDown.Value;
            int newPriority  = (int)priorityNumericUpDown.Value;
            var newVersion = (PODVersion)versionComboBox.SelectedIndex;

            if (newRevision != fileNode.Revision || newPriority != fileNode.Priority ||
                textBoxTitle.Text != fileNode.Title || textBoxAuthor.Text != fileNode.Author ||
                textBoxCopyright.Text != fileNode.Copyright || newVersion != fileNode.Version)
            {
                fileNode.MarkDirty();
            }

            fileNode.Revision = newRevision;
            fileNode.Priority = newPriority;
            fileNode.Title = textBoxTitle.Text;
            fileNode.Author = textBoxAuthor.Text;
            fileNode.Copyright = textBoxCopyright.Text;
            fileNode.Version = newVersion;

            fileNode.UpdateNodeState();
            this.Close();
        }
    }
}
