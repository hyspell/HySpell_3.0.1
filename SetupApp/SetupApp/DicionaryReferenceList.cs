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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace SetupApp
{
    public partial class DicionaryReferenceList : Form
    {
        public ReferenceDictionaries RefDicts = new ReferenceDictionaries();
        public ReferenceDictionaries OutRefDicts = new ReferenceDictionaries();
        public DicionaryReferenceList()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (lstDictionaries.CheckedItems.Count > 20)
            {
                MessageBox.Show("Selected dictionaries exceed the maximum allowed. \nPlease select less than 20 items!", 
                                "HySpell Setup and Diagnostics [Dictionaries]", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                OutRefDicts.CollectionName = "ReferenceDictionaries";

                for (int i = 0; i < lstDictionaries.CheckedItems.Count; i++)
                {
                    ListViewItem item = lstDictionaries.CheckedItems[i];
                    ReferenceDictionary refDic = new ReferenceDictionary(i, item.SubItems[0].Text, item.SubItems[2].Text, item.SubItems[1].Text);
                    OutRefDicts.Add(refDic);
                }
                DialogResult = DialogResult.OK;
            }
        }

        private void DicionaryReferenceList_Load(object sender, EventArgs e)
        {
            PopulateReferenceDictionaries();

        }
        private void PopulateReferenceDictionaries()
        {
            if (RefDicts != null && RefDicts.Count > 0)
            {
                for (int i = 0; i < RefDicts.Count; i++)
                {
                    ListViewItem listView = new ListViewItem(RefDicts[i].LinkLabel);
                    listView.SubItems.Add(new ListViewItem.ListViewSubItem(listView, RefDicts[i].LinkDescription)); 
                    listView.SubItems.Add(new ListViewItem.ListViewSubItem(listView, RefDicts[i].LinkURL));
                    listView.SubItems.Add(new ListViewItem.ListViewSubItem(listView, RefDicts[i].LinkIndex.ToString()));
                    listView.Checked = true;
                    lstDictionaries.Items.Add(listView);
                }
                btnOK.Enabled = true;
            }
        }

        private void lstDictionaries_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (lstDictionaries.CheckedItems.Count > 0)
                btnOK.Enabled = true;
            else
                btnOK.Enabled = false;
        }

        private void lnkNayiri_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.nayiri.com");
        }
    }
}
