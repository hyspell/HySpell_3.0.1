namespace HySpell
{
    partial class frmAddNewWord
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblNewWord = new System.Windows.Forms.Label();
            this.txtNewWord = new System.Windows.Forms.TextBox();
            this.grpInflectLike = new System.Windows.Forms.GroupBox();
            this.lblPlural = new System.Windows.Forms.Label();
            this.chkEr = new System.Windows.Forms.CheckBox();
            this.chkNer = new System.Windows.Forms.CheckBox();
            this.lblExample = new System.Windows.Forms.Label();
            this.txtExample = new System.Windows.Forms.TextBox();
            this.rbNone = new System.Windows.Forms.RadioButton();
            this.rbOther = new System.Windows.Forms.RadioButton();
            this.rbVerb = new System.Windows.Forms.RadioButton();
            this.rbNounAdjective = new System.Windows.Forms.RadioButton();
            this.grpInflectLike.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Sylfaen", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(242, 209);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Չեղարկել";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("Sylfaen", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(161, 209);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "Աւելցնել";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblNewWord
            // 
            this.lblNewWord.AutoSize = true;
            this.lblNewWord.Font = new System.Drawing.Font("Sylfaen", 8.25F);
            this.lblNewWord.Location = new System.Drawing.Point(12, 11);
            this.lblNewWord.Name = "lblNewWord";
            this.lblNewWord.Size = new System.Drawing.Size(279, 14);
            this.lblNewWord.TabIndex = 200;
            this.lblNewWord.Text = "Նոր Բառ : (Հոլովման համար ներածել բառի արմատը)";
            // 
            // txtNewWord
            // 
            this.txtNewWord.Font = new System.Drawing.Font("Arian AMU", 10F);
            this.txtNewWord.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtNewWord.Location = new System.Drawing.Point(15, 27);
            this.txtNewWord.Name = "txtNewWord";
            this.txtNewWord.Size = new System.Drawing.Size(302, 24);
            this.txtNewWord.TabIndex = 1;
            // 
            // grpInflectLike
            // 
            this.grpInflectLike.Controls.Add(this.lblPlural);
            this.grpInflectLike.Controls.Add(this.chkEr);
            this.grpInflectLike.Controls.Add(this.chkNer);
            this.grpInflectLike.Controls.Add(this.lblExample);
            this.grpInflectLike.Controls.Add(this.txtExample);
            this.grpInflectLike.Controls.Add(this.rbNone);
            this.grpInflectLike.Controls.Add(this.rbOther);
            this.grpInflectLike.Controls.Add(this.rbVerb);
            this.grpInflectLike.Controls.Add(this.rbNounAdjective);
            this.grpInflectLike.Font = new System.Drawing.Font("Sylfaen", 8.25F);
            this.grpInflectLike.Location = new System.Drawing.Point(6, 56);
            this.grpInflectLike.Name = "grpInflectLike";
            this.grpInflectLike.Size = new System.Drawing.Size(317, 150);
            this.grpInflectLike.TabIndex = 2;
            this.grpInflectLike.TabStop = false;
            this.grpInflectLike.Text = "Հոլովական Ձեւ : ";
            // 
            // lblPlural
            // 
            this.lblPlural.AutoSize = true;
            this.lblPlural.Font = new System.Drawing.Font("Sylfaen", 8.25F);
            this.lblPlural.Location = new System.Drawing.Point(149, 40);
            this.lblPlural.Name = "lblPlural";
            this.lblPlural.Size = new System.Drawing.Size(61, 14);
            this.lblPlural.TabIndex = 204;
            this.lblPlural.Text = "Հոգնակի : ";
            this.lblPlural.Visible = false;
            // 
            // chkEr
            // 
            this.chkEr.AutoSize = true;
            this.chkEr.Enabled = false;
            this.chkEr.Font = new System.Drawing.Font("Sylfaen", 8.25F);
            this.chkEr.Location = new System.Drawing.Point(259, 38);
            this.chkEr.Name = "chkEr";
            this.chkEr.Size = new System.Drawing.Size(38, 18);
            this.chkEr.TabIndex = 5;
            this.chkEr.Text = "եր";
            this.chkEr.UseVisualStyleBackColor = true;
            this.chkEr.Visible = false;
            // 
            // chkNer
            // 
            this.chkNer.AutoSize = true;
            this.chkNer.Checked = true;
            this.chkNer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNer.Enabled = false;
            this.chkNer.Font = new System.Drawing.Font("Sylfaen", 8.25F);
            this.chkNer.Location = new System.Drawing.Point(214, 38);
            this.chkNer.Name = "chkNer";
            this.chkNer.Size = new System.Drawing.Size(44, 18);
            this.chkNer.TabIndex = 4;
            this.chkNer.Text = "ներ";
            this.chkNer.UseVisualStyleBackColor = true;
            this.chkNer.Visible = false;
            // 
            // lblExample
            // 
            this.lblExample.AutoSize = true;
            this.lblExample.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblExample.Font = new System.Drawing.Font("Sylfaen", 8.25F);
            this.lblExample.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblExample.Location = new System.Drawing.Point(32, 100);
            this.lblExample.Name = "lblExample";
            this.lblExample.Size = new System.Drawing.Size(267, 14);
            this.lblExample.TabIndex = 201;
            this.lblExample.Text = "Բառի Օրինակ : (Ներածել նման խոնարհուող բառը)";
            // 
            // txtExample
            // 
            this.txtExample.Enabled = false;
            this.txtExample.Font = new System.Drawing.Font("Arian AMU", 10F);
            this.txtExample.Location = new System.Drawing.Point(33, 118);
            this.txtExample.Name = "txtExample";
            this.txtExample.Size = new System.Drawing.Size(278, 24);
            this.txtExample.TabIndex = 8;
            // 
            // rbNone
            // 
            this.rbNone.AutoSize = true;
            this.rbNone.Font = new System.Drawing.Font("Sylfaen", 8.25F);
            this.rbNone.Location = new System.Drawing.Point(15, 19);
            this.rbNone.Name = "rbNone";
            this.rbNone.Size = new System.Drawing.Size(67, 18);
            this.rbNone.TabIndex = 2;
            this.rbNone.TabStop = true;
            this.rbNone.Text = "Անհոլով";
            this.rbNone.UseVisualStyleBackColor = true;
            this.rbNone.CheckedChanged += new System.EventHandler(this.rbNone_CheckedChanged);
            // 
            // rbOther
            // 
            this.rbOther.AutoSize = true;
            this.rbOther.Font = new System.Drawing.Font("Sylfaen", 8.25F);
            this.rbOther.Location = new System.Drawing.Point(15, 76);
            this.rbOther.Name = "rbOther";
            this.rbOther.Size = new System.Drawing.Size(79, 18);
            this.rbOther.TabIndex = 7;
            this.rbOther.TabStop = true;
            this.rbOther.Text = "Այլ տեսակ";
            this.rbOther.UseVisualStyleBackColor = true;
            this.rbOther.CheckedChanged += new System.EventHandler(this.rbOther_CheckedChanged);
            // 
            // rbVerb
            // 
            this.rbVerb.AutoSize = true;
            this.rbVerb.Font = new System.Drawing.Font("Sylfaen", 8.25F);
            this.rbVerb.Location = new System.Drawing.Point(15, 57);
            this.rbVerb.Name = "rbVerb";
            this.rbVerb.Size = new System.Drawing.Size(44, 18);
            this.rbVerb.TabIndex = 6;
            this.rbVerb.TabStop = true;
            this.rbVerb.Text = "Բայ";
            this.rbVerb.UseVisualStyleBackColor = true;
            this.rbVerb.CheckedChanged += new System.EventHandler(this.rbVerb_CheckedChanged);
            // 
            // rbNounAdjective
            // 
            this.rbNounAdjective.AutoSize = true;
            this.rbNounAdjective.Font = new System.Drawing.Font("Sylfaen", 8.25F);
            this.rbNounAdjective.Location = new System.Drawing.Point(15, 38);
            this.rbNounAdjective.Name = "rbNounAdjective";
            this.rbNounAdjective.Size = new System.Drawing.Size(120, 18);
            this.rbNounAdjective.TabIndex = 3;
            this.rbNounAdjective.TabStop = true;
            this.rbNounAdjective.Text = "Գոյական/Ածական";
            this.rbNounAdjective.UseVisualStyleBackColor = true;
            this.rbNounAdjective.CheckedChanged += new System.EventHandler(this.rbNounAdjective_CheckedChanged);
            // 
            // frmAddNewWord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(329, 242);
            this.Controls.Add(this.grpInflectLike);
            this.Controls.Add(this.txtNewWord);
            this.Controls.Add(this.lblNewWord);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmAddNewWord";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Աւելցնել Նոր Բառ";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmAddNewWord_FormClosed);
            this.Load += new System.EventHandler(this.frmAddNewWord_Load);
            this.grpInflectLike.ResumeLayout(false);
            this.grpInflectLike.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblNewWord;
        private System.Windows.Forms.TextBox txtNewWord;
        private System.Windows.Forms.GroupBox grpInflectLike;
        private System.Windows.Forms.RadioButton rbNounAdjective;
        private System.Windows.Forms.RadioButton rbNone;
        private System.Windows.Forms.RadioButton rbOther;
        private System.Windows.Forms.RadioButton rbVerb;
        private System.Windows.Forms.Label lblExample;
        private System.Windows.Forms.TextBox txtExample;
        private System.Windows.Forms.CheckBox chkNer;
        private System.Windows.Forms.Label lblPlural;
        private System.Windows.Forms.CheckBox chkEr;
    }
}