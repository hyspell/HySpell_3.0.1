namespace SetupApp
{
    partial class DicionaryReferenceList
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
            this.lstDictionaries = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblSelected = new System.Windows.Forms.Label();
            this.lblCredits = new System.Windows.Forms.Label();
            this.lnkNayiri = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(565, 294);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Enabled = false;
            this.btnOK.Location = new System.Drawing.Point(475, 294);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "Update";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lstDictionaries
            // 
            this.lstDictionaries.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstDictionaries.CheckBoxes = true;
            this.lstDictionaries.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lstDictionaries.Font = new System.Drawing.Font("Arian AMU", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstDictionaries.FullRowSelect = true;
            this.lstDictionaries.GridLines = true;
            this.lstDictionaries.Location = new System.Drawing.Point(3, 20);
            this.lstDictionaries.Name = "lstDictionaries";
            this.lstDictionaries.Size = new System.Drawing.Size(646, 268);
            this.lstDictionaries.TabIndex = 2;
            this.lstDictionaries.UseCompatibleStateImageBehavior = false;
            this.lstDictionaries.View = System.Windows.Forms.View.Details;
            this.lstDictionaries.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lstDictionaries_ItemChecked);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 145;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Description";
            this.columnHeader2.Width = 474;
            // 
            // lblSelected
            // 
            this.lblSelected.AutoSize = true;
            this.lblSelected.Location = new System.Drawing.Point(3, 4);
            this.lblSelected.Name = "lblSelected";
            this.lblSelected.Size = new System.Drawing.Size(110, 13);
            this.lblSelected.TabIndex = 3;
            this.lblSelected.Text = "Selected Dictionaries:";
            // 
            // lblCredits
            // 
            this.lblCredits.AutoSize = true;
            this.lblCredits.Location = new System.Drawing.Point(7, 299);
            this.lblCredits.Name = "lblCredits";
            this.lblCredits.Size = new System.Drawing.Size(310, 13);
            this.lblCredits.TabIndex = 4;
            this.lblCredits.Text = "Reference dictionaries are periodically updated and provided by:";
            // 
            // lnkNayiri
            // 
            this.lnkNayiri.AutoSize = true;
            this.lnkNayiri.Location = new System.Drawing.Point(320, 299);
            this.lnkNayiri.Name = "lnkNayiri";
            this.lnkNayiri.Size = new System.Drawing.Size(81, 13);
            this.lnkNayiri.TabIndex = 5;
            this.lnkNayiri.TabStop = true;
            this.lnkNayiri.Text = "www.nayiri.com";
            this.lnkNayiri.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkNayiri_LinkClicked);
            // 
            // DicionaryReferenceList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 324);
            this.Controls.Add(this.lnkNayiri);
            this.Controls.Add(this.lblCredits);
            this.Controls.Add(this.lblSelected);
            this.Controls.Add(this.lstDictionaries);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DicionaryReferenceList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Available Dictionaries";
            this.Load += new System.EventHandler(this.DicionaryReferenceList_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ListView lstDictionaries;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label lblSelected;
        private System.Windows.Forms.Label lblCredits;
        private System.Windows.Forms.LinkLabel lnkNayiri;
    }
}