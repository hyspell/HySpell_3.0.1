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
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Office = Microsoft.Office.Core;
using Outlook = Microsoft.Office.Interop.Outlook;
using Word = Microsoft.Office.Interop.Word;
using System.Web;
using NHunspell;

namespace HySpellOL
{
    public partial class HSTaskPane : UserControl
    {
        private Dictionaries dict = new Dictionaries();
        private LinkLabel[] links = new LinkLabel[20];
        private LinkLabel copyRightLink = new LinkLabel();
        private ReferenceDictionaries m_refDicts;
        private Dictionary<string, string> oLexMapDic;

        public HSTaskPane()
        {
            InitializeComponent();
        }
        public HSTaskPane(Hunspell oHunspell)
        {
            InitializeComponent();
            m_oEncoder = new HySpellEncoder.Wrapper();
            m_oHunspell = oHunspell;
            sCustomDictPath = Globals.ThisAddIn.CustomDictPath;
            this.LostFocus += new EventHandler(HSTaskPane_LostFocus);
            this.VisibleChanged += new EventHandler(HSTaskPane_VisibleChanged);
        }

        void HSTaskPane_VisibleChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("VisibleChanged");
        }

        void HSTaskPane_LostFocus(object sender, EventArgs e)
        {
        }

        private void HSTaskPane_Resize(object sender, EventArgs e)
        {
            this.Height = Math.Max(this.Height, 300);
            txtMisspellIndicator.Left = this.Left + 2;
            txtMisspellIndicator.Top = this.Bottom - txtMisspellIndicator.Height - 26;
            txtMisspellIndicator.Width = this.Width - 14;
            lblMisspellIndicator.Left = txtMisspellIndicator.Left;
            lblMisspellIndicator.Top = txtMisspellIndicator.Top - lblMisspellIndicator.Height;
            int nLeft = txtMisspellIndicator.Left;
            int nWidth = txtMisspellIndicator.Width;
            txtWord.Left = nLeft;
            txtWord.Width = nWidth;
            lstStemFlex.Left = nLeft;
            lstStemFlex.Width = nWidth;
            rtMeaning.Left = nLeft;
            rtMeaning.Width = nWidth;
            btnFind.Left = txtWord.Right - btnFind.Width;
            btnLoadDict.Left = txtWord.Right - btnLoadDict.Width;
            lstWords.Width = nWidth;
        }

        // this function is used for development purpose, the button should be hidden in release
        private void btnLoadDict_Click(object sender, EventArgs e)
        {
            dict.FillDictionaryFromFile(@"C:\Haro\Tools\Dictionaries\BookData\ArmenianDictionaryFiles\SmallArmDic.txt");
            dict.SerializeDictionary(@"C:\Haro\Tools\Dictionaries\BookData\ArmenianDictionaryFiles\SmallArmDic.bin");            
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            string sWord = txtWord.Text;
            FindMeaning(sWord, false);
        }
        public void FindMeaning(string sWord, bool bOverrideSearchWord)
        {
            this.tabControl.SelectTab(0);
            if (bOverrideSearchWord)
                txtWord.Text = sWord;
            sWord = sWord.Trim().ToLower();

            // analyze word and get stem structure
            ArrayList arrAnals = new ArrayList();
            lstStemFlex.SuspendLayout();
            lstStemFlex.Items.Clear();
            if (CheckSpelling(sWord, ref arrAnals))
            {
                foreach (string arItem in arrAnals)
                {
                    string[] sep = { " " };
                    string[] sParts = arItem.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                    // example: " st:հայ +եր +էն" or " [չ]+ st:գիտել -լ +ս"
                    string sFormatItem = "";
                    for (int i = 0; i < sParts.Length; i++)
                    {
                        string sPart = sParts[i].Replace("[", "").Replace("]", "").Trim();
                        if (sPart.Contains("st:"))
                            sPart = "[" + sPart.Replace("st:", "") + "]";
                        if (sPart.StartsWith("fl:30")) continue;
                        sFormatItem += sPart.Trim();
                    }
                    lstStemFlex.Items.Add(sFormatItem);
                }
                lstStemFlex.ResumeLayout();
                lstStemFlex.SelectedIndex = 0;
            }
            else
                DisplayMeaningOrReferences(sWord);
        }
        private void FormatMeaningItem(string sItem)
        {
            string sMItem = sItem.Trim().ToLower();
            string sWordTypeKey = "";
            string sWordType = "";
            if (StartsWithWordType(sMItem, ref sWordTypeKey, ref sWordType))
            {
                rtMeaning.AppendText("[" + sWordType +"]\r\n");
                sMItem = sMItem.Replace(sWordTypeKey, "").Trim();
            }
            rtMeaning.AppendText("- " + sMItem + ";\r\n");
            
        }
        private bool StartsWithWordType(string sItem, ref string sWordTypeKey, ref string sWordType)
        {
            bool bHasWordType = false;

            foreach (string sKey in Globals.ThisAddIn.WordType.Keys)
            {
                if (sItem.StartsWith(sKey))
                {
                    sWordTypeKey = sKey;
                    sWordType = Globals.ThisAddIn.WordType[sKey];
                    bHasWordType = true;
                    break;
                }
            }
            return bHasWordType;
        }

        private void HSTaskPane_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            //string sPath = Directory.GetParent(Globals.ThisAddIn.Application.Path).ToString();
            string sPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string sDicPath = sPath + @"\hyspell\SmallArmDic.bin";
            dict.DeSerializeDictionary(sDicPath);
            string sRefDicPath = sPath + @"\hyspell\OnlineDicReferences.xml";
            m_refDicts = new ReferenceDictionaries();
            Globals.ThisAddIn.DeSerializeDictionaryCollection(ref m_refDicts, sRefDicPath);            
            if (Globals.ThisAddIn.HSWrapper == null)
            {
                //Globals.ThisAddIn.Wrapper = new Wrapper();
                Globals.ThisAddIn.InitializeWrapper();
            }

            if (Globals.ThisAddIn.ProgramSettings.OrthographyType != 0)
            {
                string sLexMapdicPath = sPath + @"\hyspell\dictr\RCLexMap.dic";
                oLexMapDic = new Dictionary<string, string>();
                Globals.ThisAddIn.FillDictionaryFromFile(sLexMapdicPath, oLexMapDic);
            }

            copyRightLink.Text = "HySpell 3.0 © 2016, hyspell.com";
            copyRightLink.AutoSize = true;
            copyRightLink.Tag = "http://wwww.hyspell.com";
            copyRightLink.LinkClicked += new LinkLabelLinkClickedEventHandler(copyRightLink_LinkClicked);
            tblLayoutCopyRight.Controls.Add(copyRightLink);

            Cursor.Current = Cursors.Default;
        }

        void copyRightLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string sLink = (sender as LinkLabel).Tag.ToString();
            System.Diagnostics.Process.Start(sLink);
        }

        private void AddLinksToAllOnlineDicts(string sWord)
        {
            // remove all existing links
            tLayoutPanel.SuspendLayout();
            tLayoutPanel.Controls.Clear();

            var encoded = HttpUtility.UrlPathEncode(sWord);

            // read from XML setup file all available dictionary information
            foreach (ReferenceDictionary refDic in m_refDicts)
                AddLinksToOnlineDicts(refDic.LinkIndex, refDic.LinkLabel, refDic.LinkURL + encoded);
            tLayoutPanel.ResumeLayout();
        }
        private void AddLinksToOnlineDicts(int nLinkIndex, string sLinkLabel, string sLinkURL)
        {
            links[nLinkIndex] = new LinkLabel();
            links[nLinkIndex].Text = sLinkLabel;
            links[nLinkIndex].AutoSize = true;
            links[nLinkIndex].Tag = sLinkURL;
            links[nLinkIndex].LinkClicked += new LinkLabelLinkClickedEventHandler(link_LinkClicked);
            
            tLayoutPanel.Controls.Add(links[nLinkIndex]);
        }

        void link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string sLink = (sender as LinkLabel).Tag.ToString();
            System.Diagnostics.Process.Start(sLink);
        }

        private bool CheckSpelling(string sWord, ref ArrayList arrAnals)
        {
            bool bFoundStem = false;
            HySpellEncoder.Wrapper oEncoder = new HySpellEncoder.Wrapper();

            if (Globals.ThisAddIn.HSWrapper == null)
            {
                //Globals.ThisAddIn.Wrapper = new Wrapper();
                Globals.ThisAddIn.InitializeWrapper();
            }

            int nRetFlag = -1;
            ArrayList arrEncoded = new ArrayList();
            nRetFlag = oEncoder.Encode_U(sWord, arrEncoded);
            if (arrEncoded.Count != 0)
            {
                string sOutWord = arrEncoded[0].ToString();
                if (nRetFlag > 0)
                {
                    int nAccIndex = sWord.IndexOfAny(Globals.ThisAddIn.IsArmenianAccent);
                    if (nAccIndex != -1)
                    {
                        string sAcc = sWord.Substring(nAccIndex, 1);
                        if (nRetFlag < 2)
                            sAcc = Globals.ThisAddIn.DecodeAccent(sAcc);
                        if (nRetFlag == 2)
                            sOutWord = sOutWord.Remove(nAccIndex, 1);
                    }
                    // check if stem-affix is found in dic
                    List<string> anals = Globals.ThisAddIn.HSWrapper.Analyze(sOutWord);
                    //if (Globals.ThisAddIn.Wrapper.Analyze_U(arrAnals, sOutWord) > 0)
                    if (anals.Count > 0)
                    {
                        foreach (string anal in anals)
                            arrAnals.Add(anal);
                        bFoundStem = true;
                    }
                }
            }

            return bFoundStem;
        }

        private void lstStemFlex_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstStemFlex.SelectedIndex > -1)
            {
                string sItem = lstStemFlex.SelectedItem.ToString();
                string sStem = sItem.Substring(sItem.IndexOf('[') + 1);
                sStem = sStem.Substring(0, sStem.IndexOf(']'));
                DisplayMeaningOrReferences(sStem);
            }
        }
        private void DisplayMeaningOrReferences(string sWord)
        {
            // show meaning of selected
            rtMeaning.Clear();
            rtMeaning.Font = new Font("Arian AMU", 9);
            if (sWord.Length > 0)
            {
                if (Globals.ThisAddIn.ProgramSettings.OrthographyType == 0)     // classic orthography
                {
                    string sMeaning = dict.FindMeaning(sWord);
                    if (sMeaning.EndsWith("։"))
                        sMeaning = sMeaning.Substring(0, sMeaning.Length - 1);
                    string[] sep = { ";" };
                    string[] sHoms = sMeaning.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < sHoms.Length; i++)
                        FormatMeaningItem(sHoms[i]);
                }
                else    // reformed orthography
                {
                    // Handle reformed ortho hi/ho multiple word issue here
                    string sOutWord = sWord;
                    if (oLexMapDic.ContainsKey(sWord))
                        sOutWord = oLexMapDic[sWord];
                    string[] sRetWords = sOutWord.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < sRetWords.Length; i++)
                    {
                        string sItemWord = sRetWords[i].Trim();
                        if (sRetWords[i].IndexOf(" (") > -1)
                            sItemWord = sRetWords[i].Substring(0, sRetWords[i].IndexOf(" (")).Trim();
                        string sMeaning = dict.FindMeaning(sItemWord);
                        if (sMeaning.Length > 0)
                            rtMeaning.AppendText("— " + sItemWord + " (դաս.) —\r\n");
                        if (sMeaning.EndsWith("։"))
                            sMeaning = sMeaning.Substring(0, sMeaning.Length - 1);
                        string[] sep = { ";" };
                        string[] sHoms = sMeaning.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < sHoms.Length; j++)
                            FormatMeaningItem(sHoms[j]); 
                    }
                }
            }
            AddLinksToAllOnlineDicts(sWord);
        }

        private void HSTaskPane_ControlRemoved(object sender, ControlEventArgs e)
        {
            if (oLexMapDic != null)
            {
                oLexMapDic.Clear();
                oLexMapDic = null;
            }
        }

#region spell checker variables
        private HySpellEncoder.Wrapper m_oEncoder = null;
        private Hunspell m_oHunspell = null;

        Outlook.Inspector m_Inspector = null;
        Word.Document m_Document = null;

        private Word.Paragraphs oPars = null;
        private Word.Words words = null;
        private Word.Range SelWordRange = null;
        private Word.Range CurRange = null;
        private Word.Shapes oShapes = null;

        private string[] ArWords;
        private int nHit = 0;
        private bool bCheckNormalText = true;
        private bool bCursorInShape = false;
        private bool bStart = true;
        private bool bRestartPar = false;
        private bool bHasPrevTextBoxLink = false;

        private int j = 0;
        private int nRetFlag = -1;
        private int nStartPos = 0;
        private int nCursorPos = 0;
        private int nParOffset = 0;
        private int nParCount = 0;
        private int nIParOffset = 0;
        private int nWordLen = 1;

        private string sSCIIOutput = "";
        private string sUniInput = "";
        private string sDocContents = "";
        private string sParText = "";

        private int nShapeIndex = 0;
        private int nParIndex = 1;

        private string sCustomDictPath = "";

#endregion

        private void btnIgnore_Click(object sender, EventArgs e)
        {
            Globals.ThisAddIn.ByPassAppWinSelectChange = true;
            CheckNext_U();

            SetSpellingState("Անտեսել");
        }

        private void btnIgnoreAll_Click(object sender, EventArgs e)
        {
            if (m_oHunspell != null)
            {
                Globals.ThisAddIn.ByPassAppWinSelectChange = true;
                m_oHunspell.Add(sSCIIOutput);
                CheckNext_U();
            }
            SetSpellingState("Անտեսել");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (m_oHunspell != null && m_oEncoder != null)
            {
                frmAddNewWord dlg = new frmAddNewWord();
                if (Globals.ThisAddIn.ProgramSettings.OrthographyType == 1)
                    dlg.IsClassicOrthography = false;
                dlg.HSWrapper = m_oHunspell;
                dlg.HSEncoder = m_oEncoder;
                dlg.NewWord = sUniInput;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string sNewWord = dlg.NewWord;
                    string sExample = dlg.AffixExample;
                    // add word to custom dictionary function
                    if (sExample.Length > 0)
                    {
                        m_oHunspell.AddWithAffix(sNewWord, sExample);
                        AddCustomWord(sNewWord, sExample);
                    }
                    else
                    {
                        m_oHunspell.Add(sNewWord);
                        AddCustomWord(sNewWord);
                    }
                    Globals.ThisAddIn.ByPassAppWinSelectChange = true;
                    CheckNext_U();
                }
            }
            SetSpellingState("Անտեսել");
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            Globals.ThisAddIn.ByPassAppWinSelectChange = true;
            ChangeWord();
        }

        private void btnAutoCorrect_Click(object sender, EventArgs e)
        {
            // add entries to the autocorrect list after correcting the misspelled word
            ChangeWord(true);
        }

        private void lstWords_DoubleClick(object sender, EventArgs e)
        {
            Globals.ThisAddIn.ByPassAppWinSelectChange = true;
            ChangeWord();
        }

        private void tabPage1_Enter(object sender, EventArgs e)
        {
        }


#region spell checker functions
        public void SetSpellingState(string ignoreButtonName, bool buttonVisible = true)
        {
            if (!Globals.ThisAddIn.SpellOn) return;

            btnIgnore.Text = ignoreButtonName;
            if (!buttonVisible)
                lstWords.Items.Clear();
            btnIgnoreAll.Visible = buttonVisible;
            btnChange.Visible = buttonVisible;
            btnAdd.Visible = buttonVisible;
            btnAutoCorrect.Visible = buttonVisible;

            bRestartPar = !buttonVisible;
        }

        public void StartSpellChecking()
        {
            Cursor.Current = Cursors.WaitCursor;

            Globals.ThisAddIn.SpellOn = true;
            ResetAllInitVariables();

            if (!tabControl.TabPages.Contains(tabPage1))
                tabControl.TabPages.Add(tabPage1);
            tabControl.SelectTab(tabPage1);

            m_Inspector = Globals.ThisAddIn.Application.ActiveInspector();
            m_Document = m_Inspector.WordEditor;
            if (!m_Inspector.IsWordMail())
                m_Document = m_Inspector.CurrentItem as Word.Document;

            // get the cursor position in the document
            nCursorPos = m_Document.Application.Selection.Start;
            // get all shapes in the document
            oShapes = m_Document.Application.ActiveDocument.Shapes;

            Word.ShapeRange oShpRng = m_Document.Application.Selection.ShapeRange;
            if (oShpRng.Count > 0)
            {
                object obj = 1;
                Word.Shape shp = oShpRng.get_Item(ref obj);
                bCheckNormalText = false;
                bCursorInShape = true;

                for (int i = 1; i <= oShapes.Count; i++)
                {
                    obj = i;
                    nShapeIndex = i;
                    Word.Shape oShp = oShapes.get_Item(ref obj);
                    if (oShp.Name == shp.Name)
                        break;
                }
            }
            RestartParagraph();
            SetSpellingState("Անտեսել");
        }

        private void RestartParagraph()
        {
            bHasPrevTextBoxLink = false;

////
//            // if restart-par, then change the paragraph in the original document
//            if (bRestartPar)
//            {
//                Word.Range rgPar = oPars[nParIndex].Range;
//                rgPar.Text = txtText.Text;
//            }

            if (bCheckNormalText)
            {
                // get all paragraphs of normal text area (including field regions)
                oPars = m_Document.Application.ActiveDocument.Paragraphs;
                nParCount = oPars.Count;

                Word.Paragraph CurPar = oPars[nParIndex];
                int nCurPos = Math.Max(CurPar.Range.Start, nCursorPos);

                // start with the range of the whole active document
                Object oRS = null, oRE = null;
                Word.Range rg = m_Document.Application.ActiveDocument.Range(ref oRS, ref oRE);
                rg.End = int.MaxValue;

                // search for the first armenian character starting from the current cursor position
                nCurPos = SearchForArmenianChar(rg, nCurPos);
                if (nParIndex > nParCount)
                {
                    bCheckNormalText = false;
                    if (oShapes.Count > 0)
                    {
                        nShapeIndex = 1;
                        RestartParagraph();
                    }
                    else
                        TerminateSpellCheck();
                    return;
                }
            }
            else
            {
                if (!bCursorInShape)
                    nCursorPos = 0;
                if (nShapeIndex > oShapes.Count)
                {
                    TerminateSpellCheck();
                    return;
                }
                object obj = nShapeIndex;
                Word.Shape shp = oShapes.get_Item(ref obj);

                while (nShapeIndex <= oShapes.Count && !HasText(shp))
                {
                    obj = ++nShapeIndex;
                    if (nShapeIndex > oShapes.Count)
                    {
                        RestartParagraph();
                        return;
                    }
                    shp = oShapes.get_Item(ref obj);
                }
                Word.Range rg = null;
                if (nShapeIndex <= oShapes.Count && HasText(shp))
                {
                    rg = shp.TextFrame.TextRange;
                    oPars = rg.Paragraphs;
                    nParCount = oPars.Count;
                    if (shp.TextFrame.ContainingRange.Start < shp.TextFrame.TextRange.Start)
                        bHasPrevTextBoxLink = true;
                }
                else
                {
                    nShapeIndex++;
                    RestartParagraph();
                    return;
                }
                nParIndex = 1;

                Word.Range rgOffset = shp.TextFrame.TextRange;
                // search for the first armenian character in the shape object
                nCursorPos = SearchForArmenianChar(rgOffset, nCursorPos);
                if (nParIndex > nParCount)
                {
                    bCheckNormalText = false;
                    if (oShapes.Count > 0)
                    {
                        nShapeIndex++;
                        RestartParagraph();
                    }
                    else
                        TerminateSpellCheck();
                    return;
                }

            }

            if (!bRestartPar)
            {
                Word.Paragraph par = oPars[nParIndex];
                Word.Range pRg = par.Range;
                nIParOffset = Math.Max(nCursorPos - pRg.Start, 0);
                pRg.End = pRg.Start + nIParOffset;
                nParOffset = 0;
                if (nIParOffset > 0 && pRg.Text != null)
                    nParOffset += pRg.Text.Length;
                CheckParagraph(par);
            }
            bRestartPar = false;
        }

        private void CheckParagraph(Word.Paragraph par)
        {
            Cursor.Current = Cursors.WaitCursor;

            CurRange = par.Range;
            CurRange.Start += nIParOffset;  // last par does not reset the nIParOffset (must reset)
            sParText = par.Range.Text;
            CheckNext_U();
        }

        private void ResetAllInitVariables()
        {
            // reset all variables
            nHit = 0;
            bCheckNormalText = true;
            bCursorInShape = false;
            bStart = true;
            bRestartPar = false;
            bHasPrevTextBoxLink = false;
            j = 0;
            nRetFlag = -1;
            nStartPos = 0;
            nCursorPos = 0;
            nParOffset = 0;
            nParCount = 0;
            nIParOffset = 0;
            nWordLen = 1;
            sSCIIOutput = "";
            sUniInput = "";
            sDocContents = "";
            sParText = "";
            nShapeIndex = 0;
            nParIndex = 1;
            Globals.ThisAddIn.ByPassAppWinSelectChange = true;
        }

        private void TerminateSpellCheck()
        {
            Globals.ThisAddIn.SpellOn = false;
            Office.IRibbonControl control = null;
            Globals.ThisAddIn.ribbon.IsEnabled(control);

            if (tabControl.TabPages.Contains(tabPage1))
                tabControl.TabPages.Remove(tabPage1);

            ResetAllInitVariables();

            MessageBox.Show("Ուղղագրութեան ստուգումը հասաւ աւարտին։", "Microsoft Office Word",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private int SearchForArmenianChar(Word.Range rg, int nCursorPosition)
        {
            nHit++;
            int nNewPos = Math.Max(nCursorPosition, rg.Start);
            int nGap = rg.End - nNewPos;
            while (nGap > 0)
            {
                rg.Start = nNewPos;
                nGap = rg.Text.IndexOfAny(Globals.ThisAddIn.IsArmenianChar);
                if (nGap != -1)
                    nNewPos += nGap;
                else
                    nNewPos = rg.End;
            }
            // restrict the range to get the index of the first applicable paragraph
            if(!bHasPrevTextBoxLink) rg.Start = 0;
            rg.End = nNewPos;
            nParIndex = rg.Paragraphs.Count;
            if (oPars[nParIndex].Range.End <= nNewPos)
                nParIndex++; 

            return nNewPos;
        }

        private bool HasText(Word.Shape oShape)
        {
            bool bHasText = false;
            try
            {
                Word.TextFrame txtFrame = oShape.TextFrame;
                if (txtFrame.HasText != 0)
                    bHasText = true;
            }
            catch (Exception e)
            {
                bHasText = false;
            }

            return bHasText;
        }

        private void CheckNext_U()
        {
            nStartPos += nWordLen;
            if (bStart)
            {
                // extract all possible punctuation characters
                // HM: 2009-11-27 removed the dash char as word separator and used it for hyphen,
                // the em-dash is assumed to be used for word separator
                string[] charSeparators =
                    new string[] { "\n", "\r", "\t", "\f", "\v",
                                 " ", ",", ".", ";", ":", "\"", "'", "?", "&", /*"-",*/ "–", "_",  
                                 "|", "/", "<", ">", "[", "]", "{", "}", "(", ")", 
                                 "«", /*"»",*/ "…", "‘", "’", "‚", "“", "”", "„", "£", 
                                 "¤", "¥", "¦", "§", "¨", "©", "ª", "®", "¬",
                                 "\u055D", "\u0589"
                                };
                string sText = sParText.Substring(nParOffset);
                ArWords = sText.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
                nStartPos = nParOffset;
                nParOffset = 0;
                j = 0;
                btnAdd.Enabled = btnChange.Enabled = false;
            }

            if (j < ArWords.Length && !bRestartPar)
            {
                lstWords.Items.Clear();
                sSCIIOutput = "";

                string sInput = ArWords[j].ToString();
                string sInputStartDel = "";
                string sInputEndDel = "";
                int nNHStartPos = nStartPos;
                nStartPos = sParText.IndexOf(sInput, nStartPos, StringComparison.Ordinal);
                nWordLen = sInput.Length;
                int nNHLen = nWordLen;
                if (nNHStartPos < nStartPos)
                {
                    nNHStartPos = nStartPos - 1;
                    nNHLen++;
                    sInputStartDel = sParText.Substring(nNHStartPos, 1);
                }
                if (nStartPos + nWordLen < sParText.Length)
                {
                    nNHLen++;
                    sInputEndDel = sParText.Substring(nStartPos + nWordLen, 1);
                }
                if (sInputEndDel == "\r" || sInputEndDel == "\n") sInputEndDel = "";
                string sInputNbrhood = sInputStartDel + sInput + sInputEndDel;
                bStart = false;
                j += 1;

                if (m_oHunspell != null && m_oEncoder != null)
                {
                    if (sInput.Length > 0)
                    {
                        sUniInput = sInput;
                        // apply Encode to get the string in good form ready to 
                        // apply the Spell function. The return value is stored
                        // in the nRetFlag global variable for later use.
                        nRetFlag = -1;
                        ArrayList arrEncoded = new ArrayList();
                        nRetFlag = m_oEncoder.Encode_U(sInput, arrEncoded);
                        if (arrEncoded.Count != 0)
                        {
                            sSCIIOutput = arrEncoded[0].ToString();
                            // only consider applying Spell if return is positive,
                            // otherwise, skip the word and check the next word.
                            if (nRetFlag > 0)
                            {
                                // check for accent and extract it. Note that sInput is used instead of encoded
                                // sSCIIOutput, because in the case of ARMSCII-8 input text, the encode function
                                // that is used strips the accent characters, although the other case does not
                                string sAccent = "";
                                int nAccIndex = sInput.IndexOfAny(Globals.ThisAddIn.IsArmenianAccent);
                                if (nAccIndex != -1)
                                {
                                    string sAcc = sInput.Substring(nAccIndex, 1);
                                    if (nRetFlag < 2)
                                        sAcc = Globals.ThisAddIn.DecodeAccent(sAcc);
                                    if (sAcc.IndexOfAny(Globals.ThisAddIn.IsArmenianHyphen) == -1)
                                    {
                                        sAccent = sAcc;
                                        // strip from the encoded output only for UNICODE case, since in the 
                                        // other case it is already stripped
                                        if (nRetFlag == 2)
                                            sSCIIOutput = sSCIIOutput.Remove(nAccIndex, 1);
                                    }
                                }
                                bool bLegal = m_oHunspell.Spell(sSCIIOutput);
                                if (!bLegal)
                                {
                                    lblCurrentWord.Text = sInput;
                                    // select the corresponding word in the source document
                                    Word.Range rg = CurRange;
                                    Word.Find fnd = rg.Find;
                                    fnd.Text = sInputNbrhood;
                                    fnd.Forward = true;
                                    fnd.ClearFormatting();
                                    if (ExecuteFind(fnd))
                                    {
                                        if (sInputStartDel.Length > 0)
                                            rg.Start += 1;
                                        if (sInputEndDel.Length > 0)
                                            rg.End -= 1;
                                        rg.Select();
                                        SelWordRange = rg.Duplicate;
                                    }
                                    CurRange.Start = rg.End;
                                    List<string> suggests = m_oHunspell.Suggest(sSCIIOutput);
                                    if (suggests.Count > 0)
                                    {
                                        lstWords.Enabled = true;
                                        for (int i = 0; i < suggests.Count; i++)
                                        {
                                            string sAccentedWord = suggests[i];
                                            if (sAccent.Length > 0)
                                                sAccentedWord = Globals.ThisAddIn.PutAccent(sAccentedWord, sAccent);
                                            lstWords.Items.Add(sAccentedWord);
                                        }
                                        lstWords.SelectedIndex = 0;
                                        lstWords.Focus();
                                    }
                                    else
                                    {
                                        lstWords.Items.Add("(Ուղղագրիչը առաջարկ չունի)");
                                        lstWords.Enabled = false;
                                        btnIgnore.Focus();
                                        btnChange.Enabled = false;
                                        btnAdd.Enabled = true;
                                        return;
                                    }
                                    btnAdd.Enabled = btnChange.Enabled = lstWords.Enabled;
                                }
                                else CheckNext_U();
                            }
                            else CheckNext_U();
                        }
                        else CheckNext_U();
                    }
                }
            }
            else
            {
                if (bRestartPar)
                    RestartParagraph();
                else
                    nParIndex += 1;
                if (nParIndex <= nParCount)
                {
                    bStart = true;
                    if (bCheckNormalText)
                    {
                        // search for the next offset of armenian character starting from current paragraph
                        Word.Paragraph CurPar = oPars[nParIndex];
                        int nCurPos = CurPar.Range.Start;
                        Object oRS = null, oRE = null;
                        Word.Range rg = m_Document.Application.ActiveDocument.Range(ref oRS, ref oRE);
                        rg.End = int.MaxValue;
                        // search for the first armenian character starting from the current cursor position
                        nCurPos = SearchForArmenianChar(rg, nCurPos);
                        if (nParIndex > nParCount)
                        {
                            bCheckNormalText = false;
                            nShapeIndex++;
                            RestartParagraph();
                            return;
                        }
                    }

                    // continue with the resulting paragraph
                    Word.Paragraph par = oPars[nParIndex];
                    words = par.Range.Words;
                    nIParOffset = 0;
                    CheckParagraph(par);
                }
                else
                {
                    bCheckNormalText = false;
                    nShapeIndex++;
                    if (nShapeIndex <= oShapes.Count)
                    {
                        bStart = true;
                        bCursorInShape = false;
                        RestartParagraph();
                    }
                    else
                        TerminateSpellCheck();
                }
            }
        }

        private Boolean ExecuteFind(Word.Find find)
        {
            return ExecuteFind(find, Type.Missing, Type.Missing);
        }

        private Boolean ExecuteFind(Word.Find find, Object wrapFind, Object forwardFind)
        {
            Object findText = Type.Missing;
            Object matchCase = Type.Missing;
            Object matchWholeWord = Type.Missing;
            Object matchWildcards = Type.Missing;
            Object matchSoundsLike = Type.Missing;
            Object matchAllWordForms = Type.Missing;
            Object forward = forwardFind;
            Object wrap = wrapFind;
            Object format = Type.Missing;
            Object replaceWith = Type.Missing;
            Object replace = Type.Missing;
            Object matchKashida = Type.Missing;
            Object matchDiacritics = Type.Missing;
            Object matchAlefHamza = Type.Missing;
            Object matchControl = Type.Missing;

            return find.Execute(ref findText, ref matchCase,
                ref matchWholeWord, ref matchWildcards, ref matchSoundsLike,
                ref matchAllWordForms, ref forward, ref wrap, ref format,
                ref replaceWith, ref replace, ref matchKashida,
                ref matchDiacritics, ref matchAlefHamza, ref matchControl);
        }

        private void ChangeWord(bool bAddToAutocorrect = false)
        {
            string sNewWord = "";
            string sSugestWord = lstWords.SelectedItem.ToString();

            if (nRetFlag == 2 || nRetFlag == 3)
            {
                if (nRetFlag == 3)
                {
                }
                sNewWord = sSugestWord;
            }
            else if (nRetFlag > 0)
            {
                ArrayList arrEncoded = new ArrayList();
                int nEncodeRet = m_oEncoder.Encode(sSugestWord, arrEncoded);
                if (arrEncoded.Count > 0)
                    sNewWord = arrEncoded[0].ToString();
            }

            if (bAddToAutocorrect)
            {
                Object oRS = SelWordRange.Start;
                Object oRE = SelWordRange.End;
                Word.Range rgCurrWord = m_Document.Application.ActiveDocument.Range(ref oRS, ref oRE);
                m_Document.Application.AutoCorrect.Entries.Add(rgCurrWord.Text, sNewWord);
            }

            if (sNewWord.Length > 0)
            {
                string sOrigWord = ArWords[j - 1].ToString();
                if (nRetFlag == 3)
                    sOrigWord = sOrigWord.Substring(0, sOrigWord.Length - 1);
                SelWordRange.Text = (nRetFlag == 3) ? sNewWord + "»" : sNewWord;
            }

            CheckNext_U();
            SetSpellingState("Անտեսել");
        }

        private void AddCustomWord(string word, string sample = "")
        {
            string newWord = word;
            if (sample != "")
                newWord += "|" + sample;
            try
            {
                if (!File.Exists(sCustomDictPath))
                {
                    using (StreamWriter sw = File.CreateText(sCustomDictPath)) { }
                }
                using (StreamWriter sw = File.AppendText(sCustomDictPath))
                {
                    sw.WriteLine(newWord);
                }
            }
            catch { }
        }

#endregion

        private void lstWords_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool enable = lstWords.SelectedItems.Count > 0;
            btnChange.Enabled = enable;
            btnAutoCorrect.Enabled = enable;
        }



    }
}
