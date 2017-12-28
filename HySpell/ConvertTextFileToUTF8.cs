///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//  AUTHOR'S MEMO AND MIT LICENSE STATEMENT
//  ///////////////////////////////////////
//
//  Author: Haro Mherian, Ph.D. Mathematics, Computer Sciences, Linguistics, etc.
//  This software was originally created in 2008. Being the only linguistically complete Armenian spell-checker 
//  tool (in both Classical and Reformed Orthographies), it was commercially available under "HySpell Armenian 
//  Spell-Checker" name until June 2017. It was then made open source in order to promote software development
//  in the direction and advancement of the Armenian language.
//  For further information, as well as, further linguistic tools, please contact: www.hyspell.com 
//
//  Copyright (c) 2017 hyspell.com
//  
//  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
//  documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
//  the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
//  and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
//  The above copyright notice, along with author's memo, and permission notice shall be included in all copies 
//  or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//  TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//  CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//  DEALINGS IN THE SOFTWARE.
//   
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Windows.Forms;

namespace HySpell
{
    public partial class ConvertTextFileToUTF8 : Form
    {
        private string sSrcFilePath = "";
        private string sDstFilePath = "";
        private bool bInputFileIsUTF8 = false;

        public string SourceFilePath
        {
            get { return sSrcFilePath; }
            set { value = sSrcFilePath; }
        }
        public string DestinationFilePath
        {
            get { return sDstFilePath; }
            set { value = sDstFilePath; }
        }
        public bool InputFileIsUTF8
        {
            get { return bInputFileIsUTF8; }
            set { value = bInputFileIsUTF8; }
        }

        public ConvertTextFileToUTF8()
        {
            InitializeComponent();
        }

        private void btnSourceFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog oOpenDialog = new OpenFileDialog();
            oOpenDialog.Title = "Ընտրել տառաթուային ներածման նիշք";
            oOpenDialog.Filter = "TEXT Files|*.txt|CSV Files|*.csv|All files|*.*";
            oOpenDialog.FilterIndex = 1;
            oOpenDialog.RestoreDirectory = true;
            if (oOpenDialog.ShowDialog() == DialogResult.OK)
                txtSourceFile.Text = oOpenDialog.FileName;
        }

        private void btnDestFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog oSaveDialog = new SaveFileDialog();
            oSaveDialog.Title = "Ընտրել արտածման նիշք";
            oSaveDialog.Filter = "TEXT Files|*.txt|CSV Files|*.csv";
            oSaveDialog.FilterIndex = 1;
            oSaveDialog.RestoreDirectory = true;
            if (oSaveDialog.ShowDialog() == DialogResult.OK)
                txtDestFile.Text = oSaveDialog.FileName;
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            sSrcFilePath = txtSourceFile.Text;
            sDstFilePath = txtDestFile.Text;
            bInputFileIsUTF8 = chkInputFileIsUTF8.Checked;
        }

        private void txtSourceFile_TextChanged(object sender, EventArgs e)
        {
            btnConvert.Enabled = (txtSourceFile.Text.Length > 0);
        }

        private void txtDestFile_TextChanged(object sender, EventArgs e)
        {
            btnConvert.Enabled = (txtDestFile.Text.Length > 0);
        }
    }
}