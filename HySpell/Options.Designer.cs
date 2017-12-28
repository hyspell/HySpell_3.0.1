namespace HySpell
{
    partial class frmOptions
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
            this.chkViewMixMode = new System.Windows.Forms.CheckBox();
            this.chkCheckGrammar = new System.Windows.Forms.CheckBox();
            this.cmbDicLang = new System.Windows.Forms.ComboBox();
            this.lblDictionaryLang = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkSetAsDefaultOptions = new System.Windows.Forms.CheckBox();
            this.pnlOrtho = new System.Windows.Forms.Panel();
            this.pnlOrtho.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkViewMixMode
            // 
            this.chkViewMixMode.AutoSize = true;
            this.chkViewMixMode.Checked = true;
            this.chkViewMixMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkViewMixMode.Font = new System.Drawing.Font("Sylfaen", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkViewMixMode.Location = new System.Drawing.Point(105, 64);
            this.chkViewMixMode.Name = "chkViewMixMode";
            this.chkViewMixMode.Size = new System.Drawing.Size(172, 18);
            this.chkViewMixMode.TabIndex = 23;
            this.chkViewMixMode.TabStop = false;
            this.chkViewMixMode.Text = "Խառն այլագրութեամբ դիտել";
            this.chkViewMixMode.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkViewMixMode.UseVisualStyleBackColor = true;
            // 
            // chkCheckGrammar
            // 
            this.chkCheckGrammar.AutoSize = true;
            this.chkCheckGrammar.Enabled = false;
            this.chkCheckGrammar.Font = new System.Drawing.Font("Sylfaen", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCheckGrammar.Location = new System.Drawing.Point(105, 38);
            this.chkCheckGrammar.Name = "chkCheckGrammar";
            this.chkCheckGrammar.Size = new System.Drawing.Size(158, 18);
            this.chkCheckGrammar.TabIndex = 21;
            this.chkCheckGrammar.TabStop = false;
            this.chkCheckGrammar.Text = "Ստուգել քերականութիւնը";
            this.chkCheckGrammar.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkCheckGrammar.UseVisualStyleBackColor = true;
            // 
            // cmbDicLang
            // 
            this.cmbDicLang.Font = new System.Drawing.Font("Sylfaen", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbDicLang.FormattingEnabled = true;
            this.cmbDicLang.Items.AddRange(new object[] {
            "Դասական Ուղղագրութիւն",
            "Նոր Ուղղագրութիւն"});
            this.cmbDicLang.Location = new System.Drawing.Point(105, 8);
            this.cmbDicLang.Name = "cmbDicLang";
            this.cmbDicLang.Size = new System.Drawing.Size(165, 22);
            this.cmbDicLang.TabIndex = 20;
            this.cmbDicLang.TabStop = false;
            this.cmbDicLang.Tag = "";
            this.cmbDicLang.Text = "Դասական Ուղղագրութիւն";
            // 
            // lblDictionaryLang
            // 
            this.lblDictionaryLang.AutoSize = true;
            this.lblDictionaryLang.Font = new System.Drawing.Font("Sylfaen", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDictionaryLang.Location = new System.Drawing.Point(10, 11);
            this.lblDictionaryLang.Name = "lblDictionaryLang";
            this.lblDictionaryLang.Size = new System.Drawing.Size(91, 14);
            this.lblDictionaryLang.TabIndex = 22;
            this.lblDictionaryLang.Text = "Ուղղագրութիւն :";
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Font = new System.Drawing.Font("Sylfaen", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(113, 139);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 24;
            this.btnOK.Text = "գրանցել";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Sylfaen", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(204, 139);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 25;
            this.btnCancel.Text = "Չեղարկել";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // chkSetAsDefaultOptions
            // 
            this.chkSetAsDefaultOptions.AutoSize = true;
            this.chkSetAsDefaultOptions.Font = new System.Drawing.Font("Sylfaen", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSetAsDefaultOptions.Location = new System.Drawing.Point(17, 105);
            this.chkSetAsDefaultOptions.Name = "chkSetAsDefaultOptions";
            this.chkSetAsDefaultOptions.Size = new System.Drawing.Size(260, 18);
            this.chkSetAsDefaultOptions.TabIndex = 26;
            this.chkSetAsDefaultOptions.TabStop = false;
            this.chkSetAsDefaultOptions.Text = "Կարգաբերել իբրեւ սկզբնական նախընտրանք";
            this.chkSetAsDefaultOptions.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSetAsDefaultOptions.UseVisualStyleBackColor = true;
            // 
            // pnlOrtho
            // 
            this.pnlOrtho.Controls.Add(this.lblDictionaryLang);
            this.pnlOrtho.Controls.Add(this.cmbDicLang);
            this.pnlOrtho.Controls.Add(this.chkViewMixMode);
            this.pnlOrtho.Controls.Add(this.chkCheckGrammar);
            this.pnlOrtho.Location = new System.Drawing.Point(3, 6);
            this.pnlOrtho.Name = "pnlOrtho";
            this.pnlOrtho.Size = new System.Drawing.Size(289, 94);
            this.pnlOrtho.TabIndex = 27;
            // 
            // frmOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 177);
            this.Controls.Add(this.chkSetAsDefaultOptions);
            this.Controls.Add(this.pnlOrtho);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmOptions";
            this.ShowInTaskbar = false;
            this.Text = "Ընտրանք";
            this.Load += new System.EventHandler(this.frmOptions_Load);
            this.pnlOrtho.ResumeLayout(false);
            this.pnlOrtho.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkViewMixMode;
        private System.Windows.Forms.CheckBox chkCheckGrammar;
        private System.Windows.Forms.ComboBox cmbDicLang;
        private System.Windows.Forms.Label lblDictionaryLang;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkSetAsDefaultOptions;
        private System.Windows.Forms.Panel pnlOrtho;
    }
}