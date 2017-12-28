namespace SetupApp
{
    partial class frmSetupApp
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
            this.lstKeyboarLayouts = new System.Windows.Forms.ListBox();
            this.btnInstallKeyboard = new System.Windows.Forms.Button();
            this.lblKeyboards = new System.Windows.Forms.Label();
            this.btnInstallSpellChecker = new System.Windows.Forms.Button();
            this.lblInstallOfficeCustomization = new System.Windows.Forms.Label();
            this.pnlPanel = new System.Windows.Forms.Panel();
            this.chkOLInstalled = new System.Windows.Forms.CheckBox();
            this.chkWInstalled = new System.Windows.Forms.CheckBox();
            this.btnViewLayout = new System.Windows.Forms.Button();
            this.lblStep2 = new System.Windows.Forms.Label();
            this.lblStep1 = new System.Windows.Forms.Label();
            this.lblMenuDictstEtc = new System.Windows.Forms.Label();
            this.btnInstalFilesDicts = new System.Windows.Forms.Button();
            this.lblStep3 = new System.Windows.Forms.Label();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.lnkUpdateRefDicts = new System.Windows.Forms.LinkLabel();
            this.lnkUserGuide = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBoxInfo = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lnkHySpell = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.prgStatus = new System.Windows.Forms.ProgressBar();
            this.lstPrograms = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label2 = new System.Windows.Forms.Label();
            this.pnlPanel.SuspendLayout();
            this.pnlInfo.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstKeyboarLayouts
            // 
            this.lstKeyboarLayouts.Font = new System.Drawing.Font("Sylfaen", 8.25F);
            this.lstKeyboarLayouts.FormattingEnabled = true;
            this.lstKeyboarLayouts.IntegralHeight = false;
            this.lstKeyboarLayouts.ItemHeight = 14;
            this.lstKeyboarLayouts.Items.AddRange(new object[] {
            "Armenian Typewriter (Extended) – Գրամեքենայ (Ընդլայնուած)",
            "Armenian Phonetic (Extended) – Հնչիւնային (Ընդլայնուած)"});
            this.lstKeyboarLayouts.Location = new System.Drawing.Point(12, 127);
            this.lstKeyboarLayouts.Name = "lstKeyboarLayouts";
            this.lstKeyboarLayouts.Size = new System.Drawing.Size(371, 37);
            this.lstKeyboarLayouts.TabIndex = 3;
            this.lstKeyboarLayouts.SelectedIndexChanged += new System.EventHandler(this.lstKeyboarLayouts_SelectedIndexChanged);
            // 
            // btnInstallKeyboard
            // 
            this.btnInstallKeyboard.Location = new System.Drawing.Point(319, 168);
            this.btnInstallKeyboard.Name = "btnInstallKeyboard";
            this.btnInstallKeyboard.Size = new System.Drawing.Size(64, 23);
            this.btnInstallKeyboard.TabIndex = 4;
            this.btnInstallKeyboard.Text = "Install";
            this.btnInstallKeyboard.UseVisualStyleBackColor = true;
            this.btnInstallKeyboard.Click += new System.EventHandler(this.btnInstallKeyboard_Click);
            // 
            // lblKeyboards
            // 
            this.lblKeyboards.AutoSize = true;
            this.lblKeyboards.Location = new System.Drawing.Point(13, 110);
            this.lblKeyboards.Name = "lblKeyboards";
            this.lblKeyboards.Size = new System.Drawing.Size(240, 13);
            this.lblKeyboards.TabIndex = 2;
            this.lblKeyboards.Text = "STEP (3) – Armenian Extended Keyboard layouts:";
            // 
            // btnInstallSpellChecker
            // 
            this.btnInstallSpellChecker.Location = new System.Drawing.Point(319, 51);
            this.btnInstallSpellChecker.Name = "btnInstallSpellChecker";
            this.btnInstallSpellChecker.Size = new System.Drawing.Size(64, 23);
            this.btnInstallSpellChecker.TabIndex = 2;
            this.btnInstallSpellChecker.Text = "Install";
            this.btnInstallSpellChecker.UseVisualStyleBackColor = true;
            this.btnInstallSpellChecker.Click += new System.EventHandler(this.btnInstallSpellChecker_Click);
            // 
            // lblInstallOfficeCustomization
            // 
            this.lblInstallOfficeCustomization.AutoSize = true;
            this.lblInstallOfficeCustomization.Location = new System.Drawing.Point(13, 54);
            this.lblInstallOfficeCustomization.Name = "lblInstallOfficeCustomization";
            this.lblInstallOfficeCustomization.Size = new System.Drawing.Size(263, 13);
            this.lblInstallOfficeCustomization.TabIndex = 4;
            this.lblInstallOfficeCustomization.Text = "STEP (2) – Microsoft Office Customization Component:";
            // 
            // pnlPanel
            // 
            this.pnlPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPanel.Controls.Add(this.chkOLInstalled);
            this.pnlPanel.Controls.Add(this.chkWInstalled);
            this.pnlPanel.Controls.Add(this.btnViewLayout);
            this.pnlPanel.Controls.Add(this.lblStep2);
            this.pnlPanel.Controls.Add(this.lblStep1);
            this.pnlPanel.Controls.Add(this.lblMenuDictstEtc);
            this.pnlPanel.Controls.Add(this.btnInstalFilesDicts);
            this.pnlPanel.Controls.Add(this.lblStep3);
            this.pnlPanel.Controls.Add(this.lstKeyboarLayouts);
            this.pnlPanel.Controls.Add(this.lblInstallOfficeCustomization);
            this.pnlPanel.Controls.Add(this.btnInstallKeyboard);
            this.pnlPanel.Controls.Add(this.btnInstallSpellChecker);
            this.pnlPanel.Controls.Add(this.lblKeyboards);
            this.pnlPanel.Location = new System.Drawing.Point(2, 50);
            this.pnlPanel.Name = "pnlPanel";
            this.pnlPanel.Size = new System.Drawing.Size(476, 201);
            this.pnlPanel.TabIndex = 5;
            // 
            // chkOLInstalled
            // 
            this.chkOLInstalled.AutoCheck = false;
            this.chkOLInstalled.AutoSize = true;
            this.chkOLInstalled.ForeColor = System.Drawing.Color.Gray;
            this.chkOLInstalled.Location = new System.Drawing.Point(71, 87);
            this.chkOLInstalled.Name = "chkOLInstalled";
            this.chkOLInstalled.Size = new System.Drawing.Size(159, 17);
            this.chkOLInstalled.TabIndex = 14;
            this.chkOLInstalled.Text = "HySpell for Outlook Installed";
            this.chkOLInstalled.UseVisualStyleBackColor = true;
            // 
            // chkWInstalled
            // 
            this.chkWInstalled.AutoCheck = false;
            this.chkWInstalled.AutoSize = true;
            this.chkWInstalled.ForeColor = System.Drawing.Color.Gray;
            this.chkWInstalled.Location = new System.Drawing.Point(71, 70);
            this.chkWInstalled.Name = "chkWInstalled";
            this.chkWInstalled.Size = new System.Drawing.Size(148, 17);
            this.chkWInstalled.TabIndex = 13;
            this.chkWInstalled.Text = "HySpell for Word Installed";
            this.chkWInstalled.UseVisualStyleBackColor = true;
            // 
            // btnViewLayout
            // 
            this.btnViewLayout.Enabled = false;
            this.btnViewLayout.Location = new System.Drawing.Point(12, 168);
            this.btnViewLayout.Name = "btnViewLayout";
            this.btnViewLayout.Size = new System.Drawing.Size(135, 23);
            this.btnViewLayout.TabIndex = 10;
            this.btnViewLayout.Text = "View Keyboard Layout";
            this.btnViewLayout.UseVisualStyleBackColor = true;
            this.btnViewLayout.Click += new System.EventHandler(this.btnViewLayout_Click);
            // 
            // lblStep2
            // 
            this.lblStep2.AutoSize = true;
            this.lblStep2.ForeColor = System.Drawing.Color.Red;
            this.lblStep2.Location = new System.Drawing.Point(387, 56);
            this.lblStep2.Name = "lblStep2";
            this.lblStep2.Size = new System.Drawing.Size(70, 13);
            this.lblStep2.TabIndex = 9;
            this.lblStep2.Text = "(REQUIRED)";
            // 
            // lblStep1
            // 
            this.lblStep1.AutoSize = true;
            this.lblStep1.ForeColor = System.Drawing.Color.Red;
            this.lblStep1.Location = new System.Drawing.Point(387, 23);
            this.lblStep1.Name = "lblStep1";
            this.lblStep1.Size = new System.Drawing.Size(70, 13);
            this.lblStep1.TabIndex = 8;
            this.lblStep1.Text = "(REQUIRED)";
            // 
            // lblMenuDictstEtc
            // 
            this.lblMenuDictstEtc.AutoSize = true;
            this.lblMenuDictstEtc.Location = new System.Drawing.Point(13, 21);
            this.lblMenuDictstEtc.Name = "lblMenuDictstEtc";
            this.lblMenuDictstEtc.Size = new System.Drawing.Size(249, 13);
            this.lblMenuDictstEtc.TabIndex = 7;
            this.lblMenuDictstEtc.Text = "STEP (1) – HySpell Required Files and Dictionaries:";
            // 
            // btnInstalFilesDicts
            // 
            this.btnInstalFilesDicts.Location = new System.Drawing.Point(319, 18);
            this.btnInstalFilesDicts.Name = "btnInstalFilesDicts";
            this.btnInstalFilesDicts.Size = new System.Drawing.Size(64, 23);
            this.btnInstalFilesDicts.TabIndex = 1;
            this.btnInstalFilesDicts.Text = "Install";
            this.btnInstalFilesDicts.UseVisualStyleBackColor = true;
            this.btnInstalFilesDicts.Click += new System.EventHandler(this.btnInstalFilesDicts_Click);
            // 
            // lblStep3
            // 
            this.lblStep3.AutoSize = true;
            this.lblStep3.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblStep3.Location = new System.Drawing.Point(387, 173);
            this.lblStep3.Name = "lblStep3";
            this.lblStep3.Size = new System.Drawing.Size(67, 13);
            this.lblStep3.TabIndex = 5;
            this.lblStep3.Text = "(OPTIONAL)";
            // 
            // pnlInfo
            // 
            this.pnlInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlInfo.Controls.Add(this.lnkUpdateRefDicts);
            this.pnlInfo.Controls.Add(this.lnkUserGuide);
            this.pnlInfo.Controls.Add(this.label3);
            this.pnlInfo.Controls.Add(this.txtBoxInfo);
            this.pnlInfo.Location = new System.Drawing.Point(2, 274);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(476, 64);
            this.pnlInfo.TabIndex = 6;
            // 
            // lnkUpdateRefDicts
            // 
            this.lnkUpdateRefDicts.AutoSize = true;
            this.lnkUpdateRefDicts.BackColor = System.Drawing.Color.Transparent;
            this.lnkUpdateRefDicts.Font = new System.Drawing.Font("Sylfaen", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkUpdateRefDicts.Location = new System.Drawing.Point(315, 31);
            this.lnkUpdateRefDicts.Name = "lnkUpdateRefDicts";
            this.lnkUpdateRefDicts.Size = new System.Drawing.Size(114, 18);
            this.lnkUpdateRefDicts.TabIndex = 4;
            this.lnkUpdateRefDicts.TabStop = true;
            this.lnkUpdateRefDicts.Text = "Update Dictionaries";
            this.lnkUpdateRefDicts.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lnkUpdateRefDicts.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkUpdateRefDicts_LinkClicked);
            // 
            // lnkUserGuide
            // 
            this.lnkUserGuide.AutoSize = true;
            this.lnkUserGuide.BackColor = System.Drawing.Color.Transparent;
            this.lnkUserGuide.Font = new System.Drawing.Font("Sylfaen", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkUserGuide.Location = new System.Drawing.Point(315, 10);
            this.lnkUserGuide.Name = "lnkUserGuide";
            this.lnkUserGuide.Size = new System.Drawing.Size(136, 18);
            this.lnkUserGuide.TabIndex = 0;
            this.lnkUserGuide.TabStop = true;
            this.lnkUserGuide.Text = "User Guide - Ուղեգիրք";
            this.lnkUserGuide.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lnkUserGuide.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkUserGuide_LinkClicked);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "NOTE:";
            // 
            // txtBoxInfo
            // 
            this.txtBoxInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtBoxInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoxInfo.Location = new System.Drawing.Point(15, 21);
            this.txtBoxInfo.Multiline = true;
            this.txtBoxInfo.Name = "txtBoxInfo";
            this.txtBoxInfo.ReadOnly = true;
            this.txtBoxInfo.Size = new System.Drawing.Size(278, 36);
            this.txtBoxInfo.TabIndex = 0;
            this.txtBoxInfo.TabStop = false;
            this.txtBoxInfo.Text = "Extended Western and Eastern keyboard layouts are same as Microsoft\'s, along with" +
    " the Alt-Gr mode symbols.";
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(386, 342);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(64, 23);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::SetupApp.Properties.Resources.orange_arcs1;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.lnkHySpell);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(481, 47);
            this.panel1.TabIndex = 8;
            // 
            // lnkHySpell
            // 
            this.lnkHySpell.AutoSize = true;
            this.lnkHySpell.BackColor = System.Drawing.Color.Transparent;
            this.lnkHySpell.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkHySpell.Location = new System.Drawing.Point(394, 28);
            this.lnkHySpell.Name = "lnkHySpell";
            this.lnkHySpell.Size = new System.Drawing.Size(82, 13);
            this.lnkHySpell.TabIndex = 9;
            this.lnkHySpell.TabStop = true;
            this.lnkHySpell.Text = "HySpell Support";
            this.lnkHySpell.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkHySpell_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Sylfaen", 14F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.label1.Location = new System.Drawing.Point(11, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(249, 25);
            this.label1.TabIndex = 8;
            this.label1.Text = "Ծրագրաշարի Տեղադրիչ";
            // 
            // prgStatus
            // 
            this.prgStatus.ForeColor = System.Drawing.Color.Lime;
            this.prgStatus.Location = new System.Drawing.Point(2, 256);
            this.prgStatus.Name = "prgStatus";
            this.prgStatus.Size = new System.Drawing.Size(476, 10);
            this.prgStatus.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.prgStatus.TabIndex = 9;
            this.prgStatus.UseWaitCursor = true;
            // 
            // lstPrograms
            // 
            this.lstPrograms.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lstPrograms.Location = new System.Drawing.Point(2, 371);
            this.lstPrograms.Name = "lstPrograms";
            this.lstPrograms.Size = new System.Drawing.Size(476, 113);
            this.lstPrograms.TabIndex = 10;
            this.lstPrograms.TabStop = false;
            this.lstPrograms.UseCompatibleStateImageBehavior = false;
            this.lstPrograms.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 261;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Version";
            this.columnHeader2.Width = 76;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Publisher";
            this.columnHeader3.Width = 112;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 352);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(261, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Currently installed products that are related to HySpell:";
            // 
            // frmSetupApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 488);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lstPrograms);
            this.Controls.Add(this.prgStatus);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pnlInfo);
            this.Controls.Add(this.pnlPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Location = new System.Drawing.Point(200, 100);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSetupApp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "HySpell Setup and Diagnostics 3.0";
            this.Activated += new System.EventHandler(this.frmSetupApp_Activated);
            this.Load += new System.EventHandler(this.frmSetupApp_Load);
            this.pnlPanel.ResumeLayout(false);
            this.pnlPanel.PerformLayout();
            this.pnlInfo.ResumeLayout(false);
            this.pnlInfo.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstKeyboarLayouts;
        private System.Windows.Forms.Button btnInstallKeyboard;
        private System.Windows.Forms.Label lblKeyboards;
        private System.Windows.Forms.Button btnInstallSpellChecker;
        private System.Windows.Forms.Label lblInstallOfficeCustomization;
        private System.Windows.Forms.Panel pnlPanel;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.TextBox txtBoxInfo;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblMenuDictstEtc;
        private System.Windows.Forms.Button btnInstalFilesDicts;
        private System.Windows.Forms.Label lblStep3;
        private System.Windows.Forms.Label lblStep2;
        private System.Windows.Forms.Label lblStep1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar prgStatus;
        private System.Windows.Forms.ListView lstPrograms;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel lnkUserGuide;
        private System.Windows.Forms.Button btnViewLayout;
        private System.Windows.Forms.LinkLabel lnkUpdateRefDicts;
        private System.Windows.Forms.LinkLabel lnkHySpell;
        private System.Windows.Forms.CheckBox chkOLInstalled;
        private System.Windows.Forms.CheckBox chkWInstalled;
    }
}

