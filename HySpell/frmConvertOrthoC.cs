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
using System.Collections;
using System.Windows.Forms;

namespace HySpell
{
    public partial class frmConvertOrthoC : Form
    {        
        public frmConvertOrthoC()
        {
            InitializeComponent();
        }

        private void lstWords_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstPossibleWords.Items.Clear();
            ArrayList arrPossWords = new ArrayList();
            int nRet = Globals.ThisAddIn.ConvertSuggestWord(lstWords.SelectedItem.ToString(), ref arrPossWords);
            if (nRet == 0)
            {
                lstPossibleWords.Enabled = false;
                txtOutputWord.Text = lstWords.SelectedItem.ToString();
            }
            else
            {
                foreach (object oItem in arrPossWords)
                    lstPossibleWords.Items.Add(oItem.ToString());
                lstPossibleWords.Enabled = true;
                lstPossibleWords.SelectedIndex = 0;
            }
        }

        private void txtOutputWord_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Globals.ThisAddIn.UserSuggestedWord = txtOutputWord.Text;
            Globals.ThisAddIn.SubstituteAndSelectWord(txtInputWord.Text, txtOutputWord.Text);
            // reset the add-in member arrays to null
            if (Globals.ThisAddIn.PossibleWords != null)
            {
                Globals.ThisAddIn.PossibleWords.Clear();
                Globals.ThisAddIn.PossibleWords = null;
            }
            if (Globals.ThisAddIn.SuggestList != null)
            {
                Globals.ThisAddIn.SuggestList.Clear();
                Globals.ThisAddIn.SuggestList = null;
            }
            Globals.ThisAddIn.ConvertOrthography(false);
            if (Globals.ThisAddIn.TerminateOrthoConvert)
            {
                Cursor.Current = Cursors.Default;
                this.Close();
                return;
            }
            txtInputWord.Text = Globals.ThisAddIn.CurrentWord;
            lstWords.Items.Clear();
            lstWords.Enabled = true;
            lstPossibleWords.Items.Clear();
            lstPossibleWords.Enabled = true;
            if (Globals.ThisAddIn.PossibleWords != null)
            {
                foreach (object oItem in Globals.ThisAddIn.PossibleWords)
                    lstPossibleWords.Items.Add(oItem.ToString());
            }
            else if (Globals.ThisAddIn.SuggestList != null)
            {
                if (Globals.ThisAddIn.SuggestList.Count > 0)
                {
                    foreach (object oItem in Globals.ThisAddIn.SuggestList)
                        lstWords.Items.Add(oItem.ToString());
                }
                else
                {
                    lstWords.Items.Add("(Ուղղագրիչը առաջարկ չունի)");
                    lstWords.Enabled = false;
                    lstPossibleWords.Enabled = false;
                }
            }
            else
            {
                lstWords.Items.Add("(Ուղղագրիչը առաջարկ չունի)");
                lstWords.Enabled = false;
                lstPossibleWords.Enabled = false;
            }
            if (Globals.ThisAddIn.CurrentWord.IndexOfAny(Globals.ThisAddIn.IsArmenianHyphen) == -1)
                txtOutputWord.Text = Globals.ThisAddIn.CurrentWordOutPut;
            else
                txtOutputWord.Text = Globals.ThisAddIn.CurrentWord;
            Cursor.Current = Cursors.Default;
        }
        private void frmConvertOrthoC_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Globals.ThisAddIn.TerminateOrthoConvert = false;
            Globals.ThisAddIn.ConvertOrthography(false);
            if (Globals.ThisAddIn.TerminateOrthoConvert)
            {
                Cursor.Current = Cursors.Default;
                this.Close();
                return;
            }
            txtInputWord.Text = Globals.ThisAddIn.CurrentWord;
            if (Globals.ThisAddIn.PossibleWords != null)
            {
                foreach (object oItem in Globals.ThisAddIn.PossibleWords)
                    lstPossibleWords.Items.Add(oItem.ToString());
            }            
            else if (Globals.ThisAddIn.SuggestList != null)
            {
                if (Globals.ThisAddIn.SuggestList.Count > 0)
                {
                    foreach (object oItem in Globals.ThisAddIn.SuggestList)
                        lstWords.Items.Add(oItem.ToString());
                }
                else
                {
                    lstWords.Items.Add("(Ուղղագրիչը առաջարկ չունի)");
                    lstWords.Enabled = false;
                    lstPossibleWords.Enabled = false;
                }
            }
            else
            {
                lstWords.Items.Add("(Ուղղագրիչը առաջարկ չունի)");
                lstWords.Enabled = false;
                lstPossibleWords.Enabled = false;
            }
//HM---------->
            if (Globals.ThisAddIn.CurrentWord.IndexOfAny(Globals.ThisAddIn.IsArmenianHyphen) == -1)
                txtOutputWord.Text = Globals.ThisAddIn.CurrentWordOutPut;
            else
                txtOutputWord.Text = Globals.ThisAddIn.CurrentWord;
            Cursor.Current = Cursors.Default;
        }
        private void frmConvertOrthoC_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Globals.ThisAddIn.SuggestList != null)
            {
                Globals.ThisAddIn.SuggestList.Clear();
                Globals.ThisAddIn.SuggestList = null;
            }
            if (Globals.ThisAddIn.PossibleWords != null)
            {
                Globals.ThisAddIn.PossibleWords.Clear();
                Globals.ThisAddIn.PossibleWords = null;
            }
            Globals.ThisAddIn.CurrentWord = "";
            Globals.ThisAddIn.UnInitializeWrappers();
        }

        private void lstPossibleWords_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] arrPWords = lstPossibleWords.SelectedItem.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            txtOutputWord.Text = arrPWords[0];
        }

    }
}