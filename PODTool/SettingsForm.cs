using System;
using System.Windows.Forms;

namespace PODTool
{
    public partial class SettingsForm : Form
    {
        ToolTip toolTip = new ToolTip();

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void ShowToolTip(Control control, string title, string text)
        {
           toolTip.ToolTipTitle = title;
           toolTip.UseFading = true;
           toolTip.UseAnimation = true;
           toolTip.ShowAlways = true;
           toolTip.AutoPopDelay = 5000;
           toolTip.InitialDelay = 1000;
           toolTip.ReshowDelay = 0;
           toolTip.SetToolTip(control, text);
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            ProgramSettings.AuditLogUsername = userNameTextBox.Text;
            ProgramSettings.DefaultPODVersion = (PODVersion)versionComboBox.SelectedIndex;
            ProgramSettings.EnableAuditLogWarning = checkBoxAuditWarning.Checked;
            ProgramSettings.Save();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            checkBoxAuditWarning.Checked = ProgramSettings.EnableAuditLogWarning;
            userNameTextBox.Text = ProgramSettings.AuditLogUsername;
            versionComboBox.SelectedIndex = (int)ProgramSettings.DefaultPODVersion;
        }

        private void versionComboBox_MouseHover(object sender, EventArgs e)
        {
            ShowToolTip(versionComboBox, "Default POD Version", "The default POD version for newly created POD files.");
        }
    }
}
