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
            this.tlpListSize = new System.Windows.Forms.TableLayoutPanel();
            this.lblListSize = new System.Windows.Forms.Label();
            this.numListSize = new System.Windows.Forms.NumericUpDown();
            this.tlpMain.SuspendLayout();
            this.tlpListSize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numListSize)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.tlpListSize, 0, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 2;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMain.Size = new System.Drawing.Size(459, 491);
            this.tlpMain.TabIndex = 0;
            // 
            // tlpListSize
            // 
            this.tlpListSize.ColumnCount = 2;
            this.tlpListSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tlpListSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpListSize.Controls.Add(this.lblListSize, 0, 0);
            this.tlpListSize.Controls.Add(this.numListSize, 1, 0);
            this.tlpListSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpListSize.Location = new System.Drawing.Point(3, 3);
            this.tlpListSize.Name = "tlpListSize";
            this.tlpListSize.RowCount = 1;
            this.tlpListSize.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpListSize.Size = new System.Drawing.Size(453, 26);
            this.tlpListSize.TabIndex = 0;
            // 
            // lblListSize
            // 
            this.lblListSize.AutoSize = true;
            this.lblListSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblListSize.Location = new System.Drawing.Point(3, 0);
            this.lblListSize.Name = "lblListSize";
            this.lblListSize.Size = new System.Drawing.Size(74, 26);
            this.lblListSize.TabIndex = 0;
            this.lblListSize.Text = "List height:";
            this.lblListSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numListSize
            // 
            this.numListSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numListSize.Location = new System.Drawing.Point(83, 3);
            this.numListSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numListSize.Name = "numListSize";
            this.numListSize.Size = new System.Drawing.Size(367, 20);
            this.numListSize.TabIndex = 1;
            this.numListSize.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // Settings
            // 
            this.Controls.Add(this.tlpMain);
            this.Name = "Settings";
            this.Size = new System.Drawing.Size(459, 491);
            this.tlpMain.ResumeLayout(false);
            this.tlpListSize.ResumeLayout(false);
            this.tlpListSize.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numListSize)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.TableLayoutPanel tlpListSize;
        private System.Windows.Forms.Label lblListSize;
        private System.Windows.Forms.NumericUpDown numListSize;
    }
}
