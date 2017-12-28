namespace HySpell
{
    partial class HSTaskPane
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtWord = new System.Windows.Forms.TextBox();
            this.btnFind = new System.Windows.Forms.Button();
            this.txtMisspellIndicator = new System.Windows.Forms.TextBox();
            this.lblMisspellIndicator = new System.Windows.Forms.Label();
            this.rtMeaning = new System.Windows.Forms.RichTextBox();
            this.btnLoadDict = new System.Windows.Forms.Button();
            this.tLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.lblSeeAlso = new System.Windows.Forms.Label();
            this.lstStemFlex = new System.Windows.Forms.ListBox();
            this.lblWordStruct = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tblLayoutCopyRight = new System.Windows.Forms.TableLayoutPanel();
            this.btnAutoCorrect = new System.Windows.Forms.Button();
            this.btnChange = new System.Windows.Forms.Button();
            this.lstWords = new System.Windows.Forms.ListBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnIgnoreAll = new System.Windows.Forms.Button();
            this.btnIgnore = new System.Windows.Forms.Button();
            this.lblCurrentWord = new System.Windows.Forms.Label();
            this.tabControl.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arian AMU", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Բառ: ";
            // 
            // txtWord
            // 
            this.txtWord.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtWord.Font = new System.Drawing.Font("Arian AMU", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWord.Location = new System.Drawing.Point(6, 32);
            this.txtWord.Name = "txtWord";
            this.txtWord.Size = new System.Drawing.Size(194, 16);
            this.txtWord.TabIndex = 1;
            // 
            // btnFind
            // 
            this.btnFind.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnFind.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.btnFind.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFind.Font = new System.Drawing.Font("Arian AMU", 8F);
            this.btnFind.Location = new System.Drawing.Point(142, 57);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(58, 24);
            this.btnFind.TabIndex = 2;
            this.btnFind.Text = "Որոնել";
            this.btnFind.UseVisualStyleBackColor = false;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // txtMisspellIndicator
            // 
            this.txtMisspellIndicator.Font = new System.Drawing.Font("Arian AMU", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMisspellIndicator.Location = new System.Drawing.Point(1, 476);
            this.txtMisspellIndicator.Name = "txtMisspellIndicator";
            this.txtMisspellIndicator.ReadOnly = true;
            this.txtMisspellIndicator.Size = new System.Drawing.Size(199, 23);
            this.txtMisspellIndicator.TabIndex = 7;
            // 
            // lblMisspellIndicator
            // 
            this.lblMisspellIndicator.AutoSize = true;
            this.lblMisspellIndicator.Font = new System.Drawing.Font("Arian AMU", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMisspellIndicator.Location = new System.Drawing.Point(3, 456);
            this.lblMisspellIndicator.Name = "lblMisspellIndicator";
            this.lblMisspellIndicator.Size = new System.Drawing.Size(106, 15);
            this.lblMisspellIndicator.TabIndex = 6;
            this.lblMisspellIndicator.Text = "Ընտրուած Բառ: ";
            // 
            // rtMeaning
            // 
            this.rtMeaning.BackColor = System.Drawing.SystemColors.Window;
            this.rtMeaning.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtMeaning.Font = new System.Drawing.Font("Arian AMU", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtMeaning.Location = new System.Drawing.Point(6, 149);
            this.rtMeaning.Name = "rtMeaning";
            this.rtMeaning.ReadOnly = true;
            this.rtMeaning.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtMeaning.Size = new System.Drawing.Size(194, 162);
            this.rtMeaning.TabIndex = 9;
            this.rtMeaning.Text = "";
            // 
            // btnLoadDict
            // 
            this.btnLoadDict.Location = new System.Drawing.Point(126, 3);
            this.btnLoadDict.Name = "btnLoadDict";
            this.btnLoadDict.Size = new System.Drawing.Size(74, 23);
            this.btnLoadDict.TabIndex = 10;
            this.btnLoadDict.Text = "Load Dict";
            this.btnLoadDict.UseVisualStyleBackColor = true;
            this.btnLoadDict.Visible = false;
            this.btnLoadDict.Click += new System.EventHandler(this.btnLoadDict_Click);
            // 
            // tLayoutPanel
            // 
            this.tLayoutPanel.AutoSize = true;
            this.tLayoutPanel.ColumnCount = 1;
            this.tLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tLayoutPanel.Font = new System.Drawing.Font("Arian AMU", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tLayoutPanel.Location = new System.Drawing.Point(8, 336);
            this.tLayoutPanel.Name = "tLayoutPanel";
            this.tLayoutPanel.RowCount = 2;
            this.tLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tLayoutPanel.Size = new System.Drawing.Size(191, 103);
            this.tLayoutPanel.TabIndex = 12;
            // 
            // lblSeeAlso
            // 
            this.lblSeeAlso.AutoSize = true;
            this.lblSeeAlso.Font = new System.Drawing.Font("Arian AMU", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSeeAlso.Location = new System.Drawing.Point(5, 318);
            this.lblSeeAlso.Name = "lblSeeAlso";
            this.lblSeeAlso.Size = new System.Drawing.Size(70, 15);
            this.lblSeeAlso.TabIndex = 13;
            this.lblSeeAlso.Text = "Տես Նաեւ: ";
            // 
            // lstStemFlex
            // 
            this.lstStemFlex.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstStemFlex.Font = new System.Drawing.Font("Arian AMU", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstStemFlex.FormattingEnabled = true;
            this.lstStemFlex.ItemHeight = 15;
            this.lstStemFlex.Location = new System.Drawing.Point(6, 87);
            this.lstStemFlex.Name = "lstStemFlex";
            this.lstStemFlex.Size = new System.Drawing.Size(193, 45);
            this.lstStemFlex.TabIndex = 14;
            this.lstStemFlex.SelectedIndexChanged += new System.EventHandler(this.lstStemFlex_SelectedIndexChanged);
            // 
            // lblWordStruct
            // 
            this.lblWordStruct.AutoSize = true;
            this.lblWordStruct.Font = new System.Drawing.Font("Arian AMU", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWordStruct.Location = new System.Drawing.Point(3, 66);
            this.lblWordStruct.Name = "lblWordStruct";
            this.lblWordStruct.Size = new System.Drawing.Size(82, 15);
            this.lblWordStruct.TabIndex = 15;
            this.lblWordStruct.Text = "Բառակազմ: ";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(222, 613);
            this.tabControl.TabIndex = 16;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tabPage2.Controls.Add(this.rtMeaning);
            this.tabPage2.Controls.Add(this.lblWordStruct);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.lstStemFlex);
            this.tabPage2.Controls.Add(this.txtWord);
            this.tabPage2.Controls.Add(this.lblSeeAlso);
            this.tabPage2.Controls.Add(this.btnFind);
            this.tabPage2.Controls.Add(this.tLayoutPanel);
            this.tabPage2.Controls.Add(this.lblMisspellIndicator);
            this.tabPage2.Controls.Add(this.btnLoadDict);
            this.tabPage2.Controls.Add(this.txtMisspellIndicator);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(214, 587);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Բառարան";
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tabPage1.Controls.Add(this.tblLayoutCopyRight);
            this.tabPage1.Controls.Add(this.btnAutoCorrect);
            this.tabPage1.Controls.Add(this.btnChange);
            this.tabPage1.Controls.Add(this.lstWords);
            this.tabPage1.Controls.Add(this.btnAdd);
            this.tabPage1.Controls.Add(this.btnIgnoreAll);
            this.tabPage1.Controls.Add(this.btnIgnore);
            this.tabPage1.Controls.Add(this.lblCurrentWord);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(214, 587);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Ուղղագրիչ";
            this.tabPage1.Enter += new System.EventHandler(this.tabPage1_Enter);
            // 
            // tblLayoutCopyRight
            // 
            this.tblLayoutCopyRight.AutoSize = true;
            this.tblLayoutCopyRight.ColumnCount = 1;
            this.tblLayoutCopyRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblLayoutCopyRight.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tblLayoutCopyRight.Font = new System.Drawing.Font("Arian AMU", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tblLayoutCopyRight.Location = new System.Drawing.Point(3, 584);
            this.tblLayoutCopyRight.Name = "tblLayoutCopyRight";
            this.tblLayoutCopyRight.RowCount = 2;
            this.tblLayoutCopyRight.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblLayoutCopyRight.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblLayoutCopyRight.Size = new System.Drawing.Size(208, 0);
            this.tblLayoutCopyRight.TabIndex = 25;
            // 
            // btnAutoCorrect
            // 
            this.btnAutoCorrect.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnAutoCorrect.Enabled = false;
            this.btnAutoCorrect.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.btnAutoCorrect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAutoCorrect.Font = new System.Drawing.Font("Arian AMU", 8F);
            this.btnAutoCorrect.Location = new System.Drawing.Point(109, 222);
            this.btnAutoCorrect.Name = "btnAutoCorrect";
            this.btnAutoCorrect.Size = new System.Drawing.Size(80, 24);
            this.btnAutoCorrect.TabIndex = 23;
            this.btnAutoCorrect.Text = "Ինքնաշտկել";
            this.btnAutoCorrect.UseVisualStyleBackColor = false;
            this.btnAutoCorrect.Click += new System.EventHandler(this.btnAutoCorrect_Click);
            // 
            // btnChange
            // 
            this.btnChange.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnChange.Enabled = false;
            this.btnChange.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.btnChange.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChange.Font = new System.Drawing.Font("Arian AMU", 8F);
            this.btnChange.Location = new System.Drawing.Point(2, 222);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(68, 24);
            this.btnChange.TabIndex = 2;
            this.btnChange.Text = "Ուղղել";
            this.btnChange.UseVisualStyleBackColor = false;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            // 
            // lstWords
            // 
            this.lstWords.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstWords.Font = new System.Drawing.Font("Arian AMU", 9F);
            this.lstWords.FormattingEnabled = true;
            this.lstWords.ItemHeight = 15;
            this.lstWords.Location = new System.Drawing.Point(2, 64);
            this.lstWords.Name = "lstWords";
            this.lstWords.Size = new System.Drawing.Size(209, 150);
            this.lstWords.TabIndex = 21;
            this.lstWords.SelectedIndexChanged += new System.EventHandler(this.lstWords_SelectedIndexChanged);
            this.lstWords.DoubleClick += new System.EventHandler(this.lstWords_DoubleClick);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnAdd.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Font = new System.Drawing.Font("Arian AMU", 10F);
            this.btnAdd.Location = new System.Drawing.Point(164, 34);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(24, 24);
            this.btnAdd.TabIndex = 19;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnIgnoreAll
            // 
            this.btnIgnoreAll.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnIgnoreAll.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.btnIgnoreAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIgnoreAll.Font = new System.Drawing.Font("Arian AMU", 8F);
            this.btnIgnoreAll.Location = new System.Drawing.Point(72, 34);
            this.btnIgnoreAll.Name = "btnIgnoreAll";
            this.btnIgnoreAll.Size = new System.Drawing.Size(90, 24);
            this.btnIgnoreAll.TabIndex = 18;
            this.btnIgnoreAll.Text = "Անտեսել ամէն";
            this.btnIgnoreAll.UseVisualStyleBackColor = false;
            this.btnIgnoreAll.Click += new System.EventHandler(this.btnIgnoreAll_Click);
            // 
            // btnIgnore
            // 
            this.btnIgnore.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnIgnore.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.btnIgnore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIgnore.Font = new System.Drawing.Font("Arian AMU", 8F);
            this.btnIgnore.Location = new System.Drawing.Point(2, 34);
            this.btnIgnore.Name = "btnIgnore";
            this.btnIgnore.Size = new System.Drawing.Size(68, 24);
            this.btnIgnore.TabIndex = 0;
            this.btnIgnore.Text = "Անտեսել";
            this.btnIgnore.UseVisualStyleBackColor = false;
            this.btnIgnore.Click += new System.EventHandler(this.btnIgnore_Click);
            // 
            // lblCurrentWord
            // 
            this.lblCurrentWord.AutoSize = true;
            this.lblCurrentWord.Font = new System.Drawing.Font("Arian AMU", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentWord.ForeColor = System.Drawing.Color.Red;
            this.lblCurrentWord.Location = new System.Drawing.Point(4, 11);
            this.lblCurrentWord.Name = "lblCurrentWord";
            this.lblCurrentWord.Size = new System.Drawing.Size(0, 15);
            this.lblCurrentWord.TabIndex = 16;
            // 
            // HSTaskPane
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Name = "HSTaskPane";
            this.Size = new System.Drawing.Size(222, 613);
            this.Load += new System.EventHandler(this.HSTaskPane_Load);
            this.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.HSTaskPane_ControlRemoved);
            this.Resize += new System.EventHandler(this.HSTaskPane_Resize);
            this.tabControl.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtWord;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.TextBox txtMisspellIndicator;
        private System.Windows.Forms.Label lblMisspellIndicator;
        private System.Windows.Forms.RichTextBox rtMeaning;
        private System.Windows.Forms.Button btnLoadDict;
        private System.Windows.Forms.TableLayoutPanel tLayoutPanel;
        private System.Windows.Forms.Label lblSeeAlso;
        private System.Windows.Forms.ListBox lstStemFlex;
        private System.Windows.Forms.Label lblWordStruct;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnIgnoreAll;
        private System.Windows.Forms.Button btnIgnore;
        private System.Windows.Forms.Label lblCurrentWord;
        private System.Windows.Forms.ListBox lstWords;
        private System.Windows.Forms.Button btnAutoCorrect;
        private System.Windows.Forms.Button btnChange;
        private System.Windows.Forms.TableLayoutPanel tblLayoutCopyRight;
    }
}
