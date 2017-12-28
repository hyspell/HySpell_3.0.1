namespace SetupApp
{
    partial class KeyboardLayout
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
            this.pnlImage = new System.Windows.Forms.Panel();
            this.chkShift = new System.Windows.Forms.CheckBox();
            this.chkAltGr = new System.Windows.Forms.CheckBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pnlImage
            // 
            this.pnlImage.BackgroundImage = global::SetupApp.Properties.Resources.ArmenTWE;
            this.pnlImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pnlImage.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlImage.Location = new System.Drawing.Point(0, 0);
            this.pnlImage.Name = "pnlImage";
            this.pnlImage.Size = new System.Drawing.Size(698, 197);
            this.pnlImage.TabIndex = 0;
            // 
            // chkShift
            // 
            this.chkShift.AutoSize = true;
            this.chkShift.Location = new System.Drawing.Point(28, 203);
            this.chkShift.Name = "chkShift";
            this.chkShift.Size = new System.Drawing.Size(68, 17);
            this.chkShift.TabIndex = 1;
            this.chkShift.Text = "Shift Key";
            this.chkShift.UseVisualStyleBackColor = true;
            this.chkShift.CheckedChanged += new System.EventHandler(this.chkShift_CheckedChanged);
            // 
            // chkAltGr
            // 
            this.chkAltGr.AutoSize = true;
            this.chkAltGr.Location = new System.Drawing.Point(118, 203);
            this.chkAltGr.Name = "chkAltGr";
            this.chkAltGr.Size = new System.Drawing.Size(73, 17);
            this.chkAltGr.TabIndex = 2;
            this.chkAltGr.Text = "Alt-Gr Key";
            this.chkAltGr.UseVisualStyleBackColor = true;
            this.chkAltGr.CheckedChanged += new System.EventHandler(this.chkAltGr_CheckedChanged);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(601, 203);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // KeyboardLayout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(698, 234);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.chkAltGr);
            this.Controls.Add(this.chkShift);
            this.Controls.Add(this.pnlImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KeyboardLayout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "KeyboardLayout";
            this.Load += new System.EventHandler(this.KeyboardLayout_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlImage;
        private System.Windows.Forms.CheckBox chkShift;
        private System.Windows.Forms.CheckBox chkAltGr;
        private System.Windows.Forms.Button btnClose;
    }
}