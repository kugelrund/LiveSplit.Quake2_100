namespace LiveSplit.Quake2_100
{
    partial class Settings
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.lblListSize = new System.Windows.Forms.Label();
            this.numListSize = new System.Windows.Forms.NumericUpDown();
            this.chkShowKills = new System.Windows.Forms.CheckBox();
            this.chkShowSecrets = new System.Windows.Forms.CheckBox();
            this.tlpMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numListSize)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 2;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.Controls.Add(this.chkShowKills, 0, 1);
            this.tlpMain.Controls.Add(this.numListSize, 1, 0);
            this.tlpMain.Controls.Add(this.lblListSize, 0, 0);
            this.tlpMain.Controls.Add(this.chkShowSecrets, 1, 1);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 3;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Size = new System.Drawing.Size(459, 491);
            this.tlpMain.TabIndex = 0;
            // 
            // lblListSize
            // 
            this.lblListSize.AutoSize = true;
            this.lblListSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblListSize.Location = new System.Drawing.Point(3, 0);
            this.lblListSize.Name = "lblListSize";
            this.lblListSize.Size = new System.Drawing.Size(223, 32);
            this.lblListSize.TabIndex = 10;
            this.lblListSize.Text = "List height:";
            this.lblListSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numListSize
            // 
            this.numListSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numListSize.Location = new System.Drawing.Point(232, 3);
            this.numListSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numListSize.Name = "numListSize";
            this.numListSize.Size = new System.Drawing.Size(224, 22);
            this.numListSize.TabIndex = 11;
            this.numListSize.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // chkShowKills
            // 
            this.chkShowKills.AutoSize = true;
            this.chkShowKills.Checked = true;
            this.chkShowKills.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowKills.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkShowKills.Location = new System.Drawing.Point(3, 35);
            this.chkShowKills.Name = "chkShowKills";
            this.chkShowKills.Size = new System.Drawing.Size(223, 26);
            this.chkShowKills.TabIndex = 13;
            this.chkShowKills.Text = "Show Kills";
            this.chkShowKills.UseVisualStyleBackColor = true;
            // 
            // chkShowSecrets
            // 
            this.chkShowSecrets.AutoSize = true;
            this.chkShowSecrets.Checked = true;
            this.chkShowSecrets.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowSecrets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkShowSecrets.Location = new System.Drawing.Point(232, 35);
            this.chkShowSecrets.Name = "chkShowSecrets";
            this.chkShowSecrets.Size = new System.Drawing.Size(224, 26);
            this.chkShowSecrets.TabIndex = 14;
            this.chkShowSecrets.Text = "Show Secrets";
            this.chkShowSecrets.UseVisualStyleBackColor = true;
            // 
            // Settings
            // 
            this.Controls.Add(this.tlpMain);
            this.Name = "Settings";
            this.Size = new System.Drawing.Size(459, 491);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numListSize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.CheckBox chkShowKills;
        private System.Windows.Forms.NumericUpDown numListSize;
        private System.Windows.Forms.Label lblListSize;
        private System.Windows.Forms.CheckBox chkShowSecrets;
    }
}
