
namespace PODTool
{
    partial class PODMetaEdit
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
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxTitle = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxCopyright = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxAuthor = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.versionComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.revisionNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.priorityNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.separator = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.revisionNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.priorityNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Title";
            // 
            // textBoxTitle
            // 
            this.textBoxTitle.Location = new System.Drawing.Point(89, 6);
            this.textBoxTitle.Name = "textBoxTitle";
            this.textBoxTitle.Size = new System.Drawing.Size(298, 20);
            this.textBoxTitle.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Copyright";
            // 
            // textBoxCopyright
            // 
            this.textBoxCopyright.Location = new System.Drawing.Point(89, 32);
            this.textBoxCopyright.Name = "textBoxCopyright";
            this.textBoxCopyright.Size = new System.Drawing.Size(298, 20);
            this.textBoxCopyright.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Author";
            // 
            // textBoxAuthor
            // 
            this.textBoxAuthor.Location = new System.Drawing.Point(89, 58);
            this.textBoxAuthor.Name = "textBoxAuthor";
            this.textBoxAuthor.Size = new System.Drawing.Size(298, 20);
            this.textBoxAuthor.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 113);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "POD Version";
            // 
            // versionComboBox
            // 
            this.versionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.versionComboBox.FormattingEnabled = true;
            this.versionComboBox.Items.AddRange(new object[] {
            "EPD1",
            "POD1",
            "POD2",
            "POD3"});
            this.versionComboBox.Location = new System.Drawing.Point(89, 110);
            this.versionComboBox.Name = "versionComboBox";
            this.versionComboBox.Size = new System.Drawing.Size(298, 21);
            this.versionComboBox.TabIndex = 8;
            this.versionComboBox.SelectedIndexChanged += new System.EventHandler(this.versionComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Revision";
            // 
            // revisionNumericUpDown
            // 
            this.revisionNumericUpDown.Location = new System.Drawing.Point(89, 84);
            this.revisionNumericUpDown.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.revisionNumericUpDown.Name = "revisionNumericUpDown";
            this.revisionNumericUpDown.Size = new System.Drawing.Size(100, 20);
            this.revisionNumericUpDown.TabIndex = 10;
            // 
            // priorityNumericUpDown
            // 
            this.priorityNumericUpDown.Location = new System.Drawing.Point(287, 84);
            this.priorityNumericUpDown.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.priorityNumericUpDown.Name = "priorityNumericUpDown";
            this.priorityNumericUpDown.Size = new System.Drawing.Size(100, 20);
            this.priorityNumericUpDown.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(210, 86);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Priority";
            // 
            // separator
            // 
            this.separator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.separator.Location = new System.Drawing.Point(-5, 144);
            this.separator.Name = "separator";
            this.separator.Size = new System.Drawing.Size(452, 2);
            this.separator.TabIndex = 13;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(231, 152);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 14;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(312, 152);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // PODMetaEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 183);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.separator);
            this.Controls.Add(this.priorityNumericUpDown);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.revisionNumericUpDown);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.versionComboBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxAuthor);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxCopyright);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxTitle);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "PODMetaEdit";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit POD Properties";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PODMetaEdit_FormClosed);
            this.Load += new System.EventHandler(this.PODMetaEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.revisionNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.priorityNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxCopyright;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxAuthor;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox versionComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown revisionNumericUpDown;
        private System.Windows.Forms.NumericUpDown priorityNumericUpDown;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label separator;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}