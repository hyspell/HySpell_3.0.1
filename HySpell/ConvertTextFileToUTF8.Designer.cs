namespace HySpell
{
    partial class ConvertTextFileToUTF8
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
            this.btnClose = new System.Windows.Forms.Button();
            this.btnConvert = new System.Windows.Forms.Button();
            this.lblSourceFile = new System.Windows.Forms.Label();
            this.lblDestFile = new System.Windows.Forms.Label();
            this.txtSourceFile = new System.Windows.Forms.TextBox();
            this.txtDestFile = new System.Windows.Forms.TextBox();
            this.btnSourceFile = new System.Windows.Forms.Button();
            this.btnDestFile = new System.Windows.Forms.Button();
            this.chkInputFileIsUTF8 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Sylfaen", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(285, 123);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Փակել";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnConvert
            // 
            this.btnConvert.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnConvert.Enabled = false;
            this.btnConvert.Font = new System.Drawing.Font("Sylfaen", 8.25F);
            this.btnConvert.Location = new System.Drawing.Point(193, 123);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(75, 23);
            this.btnConvert.TabIndex = 1;
            this.btnConvert.Text = "Փոխարկել";
            this.btnConvert.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // lblSourceFile
            // 
            this.lblSourceFile.AutoSize = true;
            this.lblSourceFile.Font = new System.Drawing.Font("Sylfaen", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSourceFile.Location = new System.Drawing.Point(8, 9);
            this.lblSourceFile.Name = "lblSourceFile";
            this.lblSourceFile.Size = new System.Drawing.Size(150, 14);
            this.lblSourceFile.TabIndex = 2;
            this.lblSourceFile.Text = "ARMSCII-8 ներածուող նիշք ։";
            // 
            // lblDestFile
            // 
            this.lblDestFile.AutoSize = true;
            this.lblDestFile.Font = new System.Drawing.Font("Sylfaen", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDestFile.Location = new System.Drawing.Point(8, 76);
            this.lblDestFile.Name = "lblDestFile";
            this.lblDestFile.Size = new System.Drawing.Size(151, 14);
            this.lblDestFile.TabIndex = 3;
            this.lblDestFile.Text = "UNICODE արտածման նիշք ։";
            // 
            // txtSourceFile
            // 
            this.txtSourceFile.Location = new System.Drawing.Point(10, 29);
            this.txtSourceFile.Name = "txtSourceFile";
            this.txtSourceFile.Size = new System.Drawing.Size(322, 20);
            this.txtSourceFile.TabIndex = 4;
            this.txtSourceFile.TextChanged += new System.EventHandler(this.txtSourceFile_TextChanged);
            // 
            // txtDestFile
            // 
            this.txtDestFile.Location = new System.Drawing.Point(10, 94);
            this.txtDestFile.Name = "txtDestFile";
            this.txtDestFile.Size = new System.Drawing.Size(322, 20);
            this.txtDestFile.TabIndex = 5;
            this.txtDestFile.TextChanged += new System.EventHandler(this.txtDestFile_TextChanged);
            // 
            // btnSourceFile
            // 
            this.btnSourceFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSourceFile.Location = new System.Drawing.Point(333, 29);
            this.btnSourceFile.Name = "btnSourceFile";
            this.btnSourceFile.Size = new System.Drawing.Size(27, 20);
            this.btnSourceFile.TabIndex = 6;
            this.btnSourceFile.Text = "...";
            this.btnSourceFile.UseVisualStyleBackColor = true;
            this.btnSourceFile.Click += new System.EventHandler(this.btnSourceFile_Click);
            // 
            // btnDestFile
            // 
            this.btnDestFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDestFile.Location = new System.Drawing.Point(333, 94);
            this.btnDestFile.Name = "btnDestFile";
            this.btnDestFile.Size = new System.Drawing.Size(27, 20);
            this.btnDestFile.TabIndex = 7;
            this.btnDestFile.Text = "...";
            this.btnDestFile.UseVisualStyleBackColor = true;
            this.btnDestFile.Click += new System.EventHandler(this.btnDestFile_Click);
            // 
            // chkInputFileIsUTF8
            // 
            this.chkInputFileIsUTF8.AutoSize = true;
            this.chkInputFileIsUTF8.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkInputFileIsUTF8.Font = new System.Drawing.Font("Sylfaen", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkInputFileIsUTF8.Location = new System.Drawing.Point(171, 53);
            this.chkInputFileIsUTF8.Name = "chkInputFileIsUTF8";
            this.chkInputFileIsUTF8.Size = new System.Drawing.Size(161, 18);
            this.chkInputFileIsUTF8.TabIndex = 8;
            this.chkInputFileIsUTF8.Text = "Ներածուող նիշքը UTF-8 է ";
            this.chkInputFileIsUTF8.UseVisualStyleBackColor = true;
            // 
            // ConvertTextFileToUTF8
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 159);
            this.Controls.Add(this.chkInputFileIsUTF8);
            this.Controls.Add(this.btnDestFile);
            this.Controls.Add(this.btnSourceFile);
            this.Controls.Add(this.txtDestFile);
            this.Controls.Add(this.txtSourceFile);
            this.Controls.Add(this.lblDestFile);
            this.Controls.Add(this.lblSourceFile);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConvertTextFileToUTF8";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Փոխարկել ARMSCII-8 նիշքը UNICODE-ի";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.Label lblSourceFile;
        private System.Windows.Forms.Label lblDestFile;
        private System.Windows.Forms.TextBox txtSourceFile;
        private System.Windows.Forms.TextBox txtDestFile;
        private System.Windows.Forms.Button btnSourceFile;
        private System.Windows.Forms.Button btnDestFile;
        private System.Windows.Forms.CheckBox chkInputFileIsUTF8;
    }
}