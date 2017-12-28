namespace HySpell
{
    partial class frmConvertOrtho
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
            this.txtInputWord = new System.Windows.Forms.TextBox();
            this.lblInputWord = new System.Windows.Forms.Label();
            this.btnChange = new System.Windows.Forms.Button();
            this.lstWords = new System.Windows.Forms.ListBox();
            this.lblSuggestions = new System.Windows.Forms.Label();
            this.grpInput = new System.Windows.Forms.GroupBox();
            this.grpOutput = new System.Windows.Forms.GroupBox();
            this.lblOutputWord = new System.Windows.Forms.Label();
            this.txtOutputWord = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.grpInput.SuspendLayout();
            this.grpOutput.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtInputWord
            // 
            this.txtInputWord.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtInputWord.Font = new System.Drawing.Font("Arian AMU", 10F);
            this.txtInputWord.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtInputWord.Location = new System.Drawing.Point(11, 40);
            this.txtInputWord.Name = "txtInputWord";
            this.txtInputWord.ReadOnly = true;
            this.txtInputWord.Size = new System.Drawing.Size(344, 24);
            this.txtInputWord.TabIndex = 0;
            this.txtInputWord.TabStop = false;
            // 
            // lblInputWord
            // 
            this.lblInputWord.AutoSize = true;
            this.lblInputWord.Font = new System.Drawing.Font("Arian AMU", 9F);
            this.lblInputWord.Location = new System.Drawing.Point(11, 21);
            this.lblInputWord.Name = "lblInputWord";
            this.lblInputWord.Size = new System.Drawing.Size(220, 15);
            this.lblInputWord.TabIndex = 203;
            this.lblInputWord.Text = "Բառը Դասական Ուղղագրութեամբ :";
            // 
            // btnChange
            // 
            this.btnChange.Font = new System.Drawing.Font("Arian AMU", 9F);
            this.btnChange.Location = new System.Drawing.Point(274, 280);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(85, 23);
            this.btnChange.TabIndex = 10;
            this.btnChange.Text = "Փոխարկել";
            this.btnChange.UseVisualStyleBackColor = true;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            // 
            // lstWords
            // 
            this.lstWords.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstWords.Font = new System.Drawing.Font("Arian AMU", 10F);
            this.lstWords.FormattingEnabled = true;
            this.lstWords.ItemHeight = 17;
            this.lstWords.Location = new System.Drawing.Point(11, 86);
            this.lstWords.Name = "lstWords";
            this.lstWords.Size = new System.Drawing.Size(344, 87);
            this.lstWords.TabIndex = 1;
            this.lstWords.SelectedIndexChanged += new System.EventHandler(this.lstWords_SelectedIndexChanged);
            // 
            // lblSuggestions
            // 
            this.lblSuggestions.AutoSize = true;
            this.lblSuggestions.Font = new System.Drawing.Font("Arian AMU", 9F);
            this.lblSuggestions.Location = new System.Drawing.Point(11, 68);
            this.lblSuggestions.Name = "lblSuggestions";
            this.lblSuggestions.Size = new System.Drawing.Size(92, 15);
            this.lblSuggestions.TabIndex = 204;
            this.lblSuggestions.Text = "Առաջարկներ :";
            // 
            // grpInput
            // 
            this.grpInput.Controls.Add(this.lstWords);
            this.grpInput.Controls.Add(this.lblInputWord);
            this.grpInput.Controls.Add(this.lblSuggestions);
            this.grpInput.Controls.Add(this.txtInputWord);
            this.grpInput.Font = new System.Drawing.Font("Arian AMU", 9F);
            this.grpInput.Location = new System.Drawing.Point(4, 7);
            this.grpInput.Name = "grpInput";
            this.grpInput.Size = new System.Drawing.Size(366, 189);
            this.grpInput.TabIndex = 206;
            this.grpInput.TabStop = false;
            this.grpInput.Text = "Ներածման տուեալներ :";
            // 
            // grpOutput
            // 
            this.grpOutput.Controls.Add(this.lblOutputWord);
            this.grpOutput.Controls.Add(this.txtOutputWord);
            this.grpOutput.Font = new System.Drawing.Font("Arian AMU", 9F);
            this.grpOutput.Location = new System.Drawing.Point(4, 199);
            this.grpOutput.Name = "grpOutput";
            this.grpOutput.Size = new System.Drawing.Size(366, 75);
            this.grpOutput.TabIndex = 207;
            this.grpOutput.TabStop = false;
            this.grpOutput.Text = "Արտածման տուեալներ :";
            // 
            // lblOutputWord
            // 
            this.lblOutputWord.AutoSize = true;
            this.lblOutputWord.Font = new System.Drawing.Font("Arian AMU", 9F);
            this.lblOutputWord.Location = new System.Drawing.Point(11, 21);
            this.lblOutputWord.Name = "lblOutputWord";
            this.lblOutputWord.Size = new System.Drawing.Size(180, 15);
            this.lblOutputWord.TabIndex = 203;
            this.lblOutputWord.Text = "Բառը Նոր Ուղղագրութեամբ :";
            // 
            // txtOutputWord
            // 
            this.txtOutputWord.Font = new System.Drawing.Font("Arian AMU", 10F);
            this.txtOutputWord.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtOutputWord.Location = new System.Drawing.Point(11, 40);
            this.txtOutputWord.Name = "txtOutputWord";
            this.txtOutputWord.Size = new System.Drawing.Size(344, 24);
            this.txtOutputWord.TabIndex = 2;
            this.txtOutputWord.TextChanged += new System.EventHandler(this.txtOutputWord_TextChanged);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Sylfaen", 8.25F);
            this.lblStatus.Location = new System.Drawing.Point(15, 287);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 14);
            this.lblStatus.TabIndex = 208;
            // 
            // frmConvertOrtho
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 315);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.grpOutput);
            this.Controls.Add(this.grpInput);
            this.Controls.Add(this.btnChange);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmConvertOrtho";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Փոխարկել Նոր Ուղղագրութեան";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmConvertOrtho_FormClosing);
            this.Load += new System.EventHandler(this.frmConvertOrtho_Load);
            this.grpInput.ResumeLayout(false);
            this.grpInput.PerformLayout();
            this.grpOutput.ResumeLayout(false);
            this.grpOutput.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtInputWord;
        private System.Windows.Forms.Label lblInputWord;
        private System.Windows.Forms.Button btnChange;
        private System.Windows.Forms.ListBox lstWords;
        private System.Windows.Forms.Label lblSuggestions;
        private System.Windows.Forms.GroupBox grpInput;
        private System.Windows.Forms.GroupBox grpOutput;
        private System.Windows.Forms.Label lblOutputWord;
        private System.Windows.Forms.TextBox txtOutputWord;
        private System.Windows.Forms.Label lblStatus;
    }
}