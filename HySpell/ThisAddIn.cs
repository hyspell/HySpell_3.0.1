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
using System.Xml.Serialization;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;
using NHunspell;

namespace HySpell
{
    enum CapState
    {
        enNone = 0,
        enFirstCap = 1,
        enAllCaps = 2,
    }

    public class HySpellSettings
    {
        private bool bMixModeView = true;   // use mixed mode view by default
        private int nOrthographyType = 0;   // classical by default
        public bool MixModeView
        {
            get { return bMixModeView; }
            set { bMixModeView = value; }
        }
        public int OrthographyType
        {
            get { return nOrthographyType; }
            set { nOrthographyType = value; }
        }
    }

    public class ReferenceDictionary
    {
        private int _LinkIndex;
        private string _LinkLabel = "";
        private string _LinkURL = "";
        private string _LinkDesc = "";

        public int LinkIndex
        {
            get { return _LinkIndex; }
            set { _LinkIndex = value; }
        }
        public string LinkLabel
        {
            get { return _LinkLabel; }
            set { _LinkLabel = value; }
        }
        public string LinkURL
        {
            get { return _LinkURL; }
            set { _LinkURL = value; }
        }

        public string LinkDescription
        {
            get { return _LinkDesc; }
            set { _LinkDesc = value; }
        }

        public ReferenceDictionary() { }
        public ReferenceDictionary(int nLinkIndex, string sLinkLabel, string sLinkURL)
        {
            _LinkIndex = nLinkIndex;
            _LinkLabel = sLinkLabel;
            _LinkURL = sLinkURL;
        }
        public ReferenceDictionary(int nLinkIndex, string sLinkLabel, string sLinkURL, string sLinkDesc)
        {
            _LinkIndex = nLinkIndex;
            _LinkLabel = sLinkLabel;
            _LinkURL = sLinkURL;
            _LinkDesc = sLinkDesc;
        }
    }

    public class ReferenceDictionaries : ICollection
    {
        public string CollectionName;
        private ArrayList rDictArray = new ArrayList();

        public ReferenceDictionary this[int index]
        {
            get { return (ReferenceDictionary)rDictArray[index]; }
        }

        public void CopyTo(Array a, int index)
        {
            rDictArray.CopyTo(a, index);
        }
        public int Count
        {
            get { return rDictArray.Count; }
        }
        public object SyncRoot
        {
            get { return this; }
        }
        public bool IsSynchronized
        {
            get { return false; }
        }
        public IEnumerator GetEnumerator()
        {
            return rDictArray.GetEnumerator();
        }

        public void Add(ReferenceDictionary newDict)
        {
            rDictArray.Add(newDict);
        }
    }


    public partial class ThisAddIn
    {
        private string sTaskPaneName = "HySpell";
        private Word.Application myApplication;
        private int m_nLangId;
        private Office.CommandBar commandBar;
        private Office.CommandBarButton[] hsMenuControls;
        private Office.CommandBarButton[] hsSubMenuControls;
        private Office.CommandBarPopup hsAutoCorrectPopup;
        //private Office.CommandBarPopup hsSynonymsPopup;

        private Office.CommandBarButton hsMenuControlLookup;
        //private Office.CommandBarButton hsMenuControlSyn;
        //private Office.CommandBarButton hsMenuControlName;
        //private Word.Template customTemplate;
        private HSTaskPane oHSTaskPaneControl;
        private Microsoft.Office.Tools.CustomTaskPane myCustomTaskPane;

        private int nSelWordEncoding = 2; // default is unicode
        private int nCurrentWordStart = 0;
        private int nCurrentWordEnd = 0;

        //Word.Style hs_SEStyle = null;
//
        bool bShowErrors = true;

        Word.Paragraphs oPars = null;
        Word.Shapes oShapes = null;
        Word.Words words = null;
        Word.Range CurRange = null;
        Word.Range SelWordRange = null;
        string[] ArWords;
        string sParText = "";
        string sSCIIOutput = "";
        string sUniInput = "";
        string sUserSuggWord = "";
        ArrayList arrSuggestList = null;
        ArrayList arrPossibleWords = null;
        bool bTerminateOrthoConvert = false;
        CapState enCapState = CapState.enNone;
        //string sDocContents = "";

        bool bStart = true;
        bool bCheckNormalText = true;
        bool bCursorInShape = false;
        bool bRestartPar = false;
        bool bHasPrevTextBoxLink = false;
        int nHit = 0;
        int nCursorPos = 0;
        int nParIndex = 1;
        int nShapeIndex = 0;
        int nParCount = 0;
        int nParOffset = 0;
        int nIParOffset = 0;
        int nWordLen = 1;
        int nRetFlag = -1;
        int nStartPos = 0;
        int j = 0;
        
        private HySpellSettings m_oSettings;

        private bool bInitSpellCheckCall = true;
        private string sCustomDictPath = "";
        private HySpellEncoder.Wrapper m_oEncoder = null;
        private Hunspell m_oHunspell = null;
        private Hunspell m_oHunspell1 = null;
        private Hunspell m_oHunspell2 = null;

        private bool m_bFromClassic = true;
        public Dictionary<string, string> m_LexMapDic = null;
        private Dictionary<string, string> m_FlexMapDic = null;

        public HySpellSettings ProgramSettings
        {
            get { return m_oSettings; }
            set { m_oSettings = value; }
        }

        public HySpellEncoder.Wrapper HSEncoder
        {
            get { return m_oEncoder; }
            set { m_oEncoder = value; }
        }
        public Hunspell HSWrapper
        {
            get { return m_oHunspell; }
            set { m_oHunspell = value; }
        }
        public string CustomDictPath
        {
            get { return sCustomDictPath; }
            set { sCustomDictPath = value; }
        }

        public string CurrentWordOutPut
        {
            get { return sSCIIOutput; }
            set { sSCIIOutput = value; }
        }
        public string CurrentWord
        {
            get { return sUniInput; }
            set { sUniInput = value; }
        }
        public int CurrentWordLength
        {
            get { return nWordLen; }
            set { nWordLen = value; }
        }
        public string UserSuggestedWord
        {
            get { return sUserSuggWord; }
            set { sUserSuggWord = value; }
        }
        public ArrayList SuggestList
        {
            get { return arrSuggestList; }
            set { arrSuggestList = value; }
        }
        public ArrayList PossibleWords
        {
            get { return arrPossibleWords; }
            set { arrPossibleWords = value; }
        }
        public bool TerminateOrthoConvert
        {
            get { return bTerminateOrthoConvert; }
            set { bTerminateOrthoConvert = value; }
        }
        public Word.Range CurrentSelectedRange
        {
            get { return SelWordRange; }
            set { SelWordRange = value; }
        }

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            m_oSettings = new HySpellSettings();
            myApplication = this.Application;
            m_nLangId = (int)myApplication.Language;

            myApplication.WindowBeforeRightClick += new Microsoft.Office.Interop.Word.ApplicationEvents4_WindowBeforeRightClickEventHandler(myApplication_WindowBeforeRightClick);
            myApplication.WindowSelectionChange += new Microsoft.Office.Interop.Word.ApplicationEvents4_WindowSelectionChangeEventHandler(myApplication_WindowSelectionChange);
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            UnInitializeWrapper();
            // remove all task-panes
            RemoveTaskPanes();
            // remove all HySpell custom menu items
            RemoveExistingMenuItem("hs_Spelling");
            RemoveExistingMenuItem("Text");
            RemoveExistingMenuItem("Lists");
            RemoveExistingMenuItem("Table Text");
            RemoveExistingMenuItem("Table Lists");
            RemoveExistingMenuItem("Hyperlink Context Menu");
            RemoveExistingMenuItem("Headings");
            RemoveExistingMenuItem("Footnotes");
        }

        static public void SerializeToXML(HySpellSettings oSettings, string sPath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(HySpellSettings));
            TextWriter textWriter = new StreamWriter(sPath);
            serializer.Serialize(textWriter, oSettings);
            textWriter.Close();
        }
        static public HySpellSettings DeserializeFromXML(HySpellSettings oDefaultSettings, string sPath)
        {
            if (!File.Exists(sPath))
                SerializeToXML(oDefaultSettings, sPath);
            XmlSerializer deserializer = new XmlSerializer(typeof(HySpellSettings));
            TextReader textReader = new StreamReader(sPath);
            HySpellSettings oSettings = (HySpellSettings)deserializer.Deserialize(textReader);
            textReader.Close();

            return oSettings;
        }
        public void SerializeDictionaryCollection(string sPath)
        {
            ReferenceDictionaries refDicts = new ReferenceDictionaries();

            refDicts.CollectionName = "ReferenceDictionaries";

            ReferenceDictionary dic0 = new ReferenceDictionary(0, "Մալխասեանց (1944)", "http://www.nayiri.com/imagedDictionaryBrowser.jsp?dictionaryId=6&query=");
            refDicts.Add(dic0);
            ReferenceDictionary dic1 = new ReferenceDictionary(1, "Աղայան (1976)", "http://www.nayiri.com/imagedDictionaryBrowser.jsp?dictionaryId=24&query=");
            refDicts.Add(dic1);
            ReferenceDictionary dic2 = new ReferenceDictionary(2, "Սուքիասեան (1967)", "http://www.nayiri.com/imagedDictionaryBrowser.jsp?dictionaryId=25&query=");
            refDicts.Add(dic2);
            ReferenceDictionary dic3 = new ReferenceDictionary(3, "Խաչատուրեան (1992)", "http://www.nayiri.com/imagedDictionaryBrowser.jsp?dictionaryId=8&query=");
            refDicts.Add(dic3);
            ReferenceDictionary dic4 = new ReferenceDictionary(4, "Գույումճեան (1981)", "http://www.nayiri.com/imagedDictionaryBrowser.jsp?dictionaryId=3&query=");
            refDicts.Add(dic4);
            ReferenceDictionary dic5 = new ReferenceDictionary(5, "Աճառեան (1926)", "http://www.nayiri.com/imagedDictionaryBrowser.jsp?dictionaryId=7&query=");
            refDicts.Add(dic5);

            XmlSerializer xmlSer = new XmlSerializer(typeof(ReferenceDictionaries));
            TextWriter writer = new StreamWriter(sPath);
            xmlSer.Serialize(writer, refDicts);
        }
        public void DeSerializeDictionaryCollection(ref ReferenceDictionaries oRefDicts, string sPath)
        {
            if (!File.Exists(sPath))
                SerializeDictionaryCollection(sPath);
            XmlSerializer deserializer = new XmlSerializer(typeof(ReferenceDictionaries));
            TextReader textReader = new StreamReader(sPath);
            oRefDicts = (ReferenceDictionaries)deserializer.Deserialize(textReader);
            textReader.Close();
        }
        public void SetProgramSettings()
        {
            //string sPath = Directory.GetParent(this.Application.Path).ToString();
            string sPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string sXMLPath = sPath + @"\hyspell\HyspellSettings.xml";
            SerializeToXML(m_oSettings, sXMLPath);
        }
        public void GetProgramSettings()
        {
            //string sPath = Directory.GetParent(this.Application.Path).ToString();
            string sPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string sXMLPath = sPath + @"\hyspell\HyspellSettings.xml";
            m_oSettings = DeserializeFromXML(m_oSettings, sXMLPath);
        }

        public string PutAccent(string sWord, string sAccent)
        {
            if (sAccent.Length > 0)
            {
                char[] IsArmenianUVowel = {/* UNICODE chars */
                                        '\u0531',		// ARMENIAN CAPITAL LETTER AYB
		                                '\u0561',		// ARMENIAN SMALL LETTER AYB
		                                '\u0535',		// ARMENIAN CAPITAL LETTER YECH
		                                '\u0565',		// ARMENIAN SMALL LETTER YECH
		                                '\u0537',		// ARMENIAN CAPITAL LETTER E
		                                '\u0567',		// ARMENIAN SMALL LETTER E
		                                '\u053B',		// ARMENIAN CAPITAL LETTER INI
		                                '\u056B',		// ARMENIAN SMALL LETTER INI
		                                '\u0548',		// ARMENIAN CAPITAL LETTER VO
		                                '\u0578',		// ARMENIAN SMALL LETTER VO
		                                '\u0555',		// ARMENIAN CAPITAL LETTER O
		                                '\u0585',		// ARMENIAN SMALL LETTER O
                                        };
                int nIndex = sWord.LastIndexOfAny(IsArmenianUVowel);
                if (nIndex > 0)
                    sWord = sWord.Insert(nIndex + 1, sAccent);
            }

            return sWord;
        }
        public string DecodeAccent(string sSCIIAccent)
        {
            string sTemp = sSCIIAccent;
            switch (sSCIIAccent.ToCharArray()[0])
            {
                case '\u00B1':
                    sTemp = '\u055E'.ToString();
                    break;
                case '\u00B0':
                    sTemp = '\u055B'.ToString();
                    break;
                case '\u00FE':
                    sTemp = '\u055A'.ToString();
                    break;
                case '\u00AF':
                    sTemp = '\u055C'.ToString();
                    break;
            }

            return sTemp;
        }
        public string EncodeAccent(string sUNIAccent)
        {
            string sTemp = sUNIAccent;
            switch (sUNIAccent.ToCharArray()[0])
            {
                case '\u055E':
                    sTemp = '\u00B1'.ToString();
                    break;
                case '\u055B':
                    sTemp = '\u00B0'.ToString();
                    break;
                case '\u055A':
                    sTemp = '\u00FE'.ToString();
                    break;
                case '\u055C':
                    sTemp = '\u00AF'.ToString();
                    break;
            }

            return sTemp;
        }

        public string EncodeWithAccents(string sWord, HySpellEncoder.Wrapper oEncoder)
        {
            string sTemp = "";
            string sStart = "";
            string sAccent = "";
            string sEnd = "";
            int nIndex = sWord.IndexOfAny(Globals.ThisAddIn.IsArmenianApostrophe);
            if (nIndex != -1)
            {
                sStart = sWord.Substring(0, nIndex);
                sAccent = sWord.Substring(nIndex, 1);
                sEnd = sWord.Substring(nIndex + 1, sWord.Length - nIndex - 1);
                // encode the beginning of the word
                ArrayList arrEncoded1 = new ArrayList();
                int nRet = oEncoder.Encode(sStart, arrEncoded1);
                if (arrEncoded1.Count != 0)
                    sTemp = arrEncoded1[0].ToString();
                // encode the apostrophe character
                sTemp += EncodeAccent(sAccent);
            }
            else
                sEnd = sWord;

            nIndex = sEnd.IndexOfAny(Globals.ThisAddIn.IsArmenianAccent);
            if (nIndex != -1)
            {
                string sStart1 = sEnd.Substring(0, nIndex);
                string sAccent1 = sEnd.Substring(nIndex, 1);
                string sEnd1 = sEnd.Substring(nIndex + 1, sEnd.Length - nIndex - 1);
                // encode the beginning of the word
                ArrayList arrEncoded3 = new ArrayList();
                int nRet = oEncoder.Encode(sStart1, arrEncoded3);
                if (arrEncoded3.Count != 0)
                    sTemp += arrEncoded3[0].ToString();
                // encode the accent character
                sTemp += EncodeAccent(sAccent1);
                // encode the end of the word
                ArrayList arrEncoded2 = new ArrayList();
                nRet = oEncoder.Encode(sEnd1, arrEncoded2);
                if (arrEncoded2.Count != 0)
                    sTemp += arrEncoded2[0].ToString();
            }
            else
            {
                ArrayList arrEncoded3 = new ArrayList();
                int nRet = oEncoder.Encode(sEnd, arrEncoded3);
                if (arrEncoded3.Count != 0)
                    sTemp += arrEncoded3[0].ToString();
            }

            return sTemp;
        }
        public string EncodeWithAccents(string sWord)
        {
            string sTemp = "";
            string sStart = "";
            string sAccent = "";
            string sEnd = "";
            int nIndex = sWord.IndexOfAny(Globals.ThisAddIn.IsArmenianApostrophe);
            if (nIndex != -1)
            {
                sStart = sWord.Substring(0, nIndex);
                sAccent = sWord.Substring(nIndex, 1);
                sEnd = sWord.Substring(nIndex + 1, sWord.Length - nIndex - 1);
                // encode the beginning of the word
                ArrayList arrEncoded1 = new ArrayList();
                int nRetFlag = m_oEncoder.Encode(sStart, arrEncoded1);
                if (arrEncoded1.Count != 0)
                    sTemp = arrEncoded1[0].ToString();
                // encode the apostrophe character
                sTemp += EncodeAccent(sAccent);
            }
            else
                sEnd = sWord;
 
            nIndex = sEnd.IndexOfAny(Globals.ThisAddIn.IsArmenianAccent);
            if (nIndex != -1)
            {
                string sStart1 = sEnd.Substring(0, nIndex);
                string sAccent1 = sEnd.Substring(nIndex, 1);
                string sEnd1 = sEnd.Substring(nIndex + 1, sEnd.Length - nIndex - 1);
                // encode the beginning of the word
                ArrayList arrEncoded3 = new ArrayList();
                nRetFlag = m_oEncoder.Encode(sStart1, arrEncoded3);
                if (arrEncoded3.Count != 0)
                    sTemp += arrEncoded3[0].ToString();
                // encode the accent character
                sTemp += EncodeAccent(sAccent1);
                // encode the end of the word
                ArrayList arrEncoded2 = new ArrayList();
                nRetFlag = m_oEncoder.Encode(sEnd1, arrEncoded2);
                if (arrEncoded2.Count != 0)
                    sTemp += arrEncoded2[0].ToString();
            }
            else
            {
                ArrayList arrEncoded3 = new ArrayList();
                nRetFlag = m_oEncoder.Encode(sEnd, arrEncoded3);
                if (arrEncoded3.Count != 0)
                    sTemp += arrEncoded3[0].ToString();
            }

            return sTemp;
        }
        public bool IsWordArmenian(Word.Selection selWord, ref ArrayList arrEncoded, ref string sApostrophe, ref string sAccent, ref Word.Range rgWord)
        {
            rgWord = selWord.Range;
            Word.Range rgWhole = selWord.Range;
            rgWhole.Start = rgWhole.Words.First.Start;
            rgWhole.End = rgWhole.Words.First.End;
            while (rgWhole.Start > 0 && !rgWhole.Text.StartsWith("\r") && !rgWhole.Text.StartsWith(" ") && !rgWhole.Text.StartsWith("\t"))
            {
                int nOldStartPos = rgWhole.Start;
                rgWhole.Start = rgWhole.Words.First.Start - 1;
                // break loop if trapped inside infinite loop
                if (rgWhole.Start >= nOldStartPos)
                    break;
            }
            rgWhole.End = rgWhole.Words.Last.End;
            while (rgWhole.Words.Last.Text != null && !rgWhole.Words.Last.Text.EndsWith("\r") 
                    && !rgWhole.Words.Last.Text.EndsWith(" ") && !rgWhole.Words.Last.Text.EndsWith("\t"))
            {
                int nOldEndPos = rgWhole.End;
                rgWhole.End = rgWhole.Words.Last.End + 1;
                // break loop if trapped inside infinite loop
                if (rgWhole.End <= nOldEndPos)
                    break;
            }
            rgWhole.End = rgWhole.Words.Last.End;
            char[] cGet = new char[] { '\n', '\r', '\t', '\f', '\v', '\a', 
                                 ' ', ',', '.', ';', ':', '\"', '\'', '?', '&', '–', '_', 
                                 '|', '/', '<', '>', '[', ']', '{', '}', '(', ')', 
                                 '«', /*'»',*/'…', '‘', '’', '‚', '“', '”', '„', '£', 
                                 '¤', '¥', '¦', '§', /*'¨',*/ '©', 'ª', '®', '¬',
                                 '\u055D', '\u0589'};
            string sTemp = rgWhole.Text.Trim(cGet);
            int nIndex = sTemp.IndexOfAny(Globals.ThisAddIn.IsArmenianApostrophe);
            if (nIndex != -1)
            {
                sApostrophe = sTemp.Substring(nIndex, 1);
                sTemp = sTemp.Remove(nIndex, 1);
            }
            nIndex = sTemp.IndexOfAny(Globals.ThisAddIn.IsArmenianAccent);
            if (nIndex != -1)
            {
                string sAcc = sTemp.Substring(nIndex, 1);
                if (sAcc.IndexOfAny(Globals.ThisAddIn.IsArmenianHyphen) == -1)
                    sAccent = sAcc;
                sTemp = sTemp.Remove(nIndex, 1); 
            }
            if (sTemp.Length > 0)
            {
                rgWord.Start = rgWhole.Start + rgWhole.Text.IndexOf(sTemp[0]);
                rgWord.End = rgWhole.Start + rgWhole.Text.LastIndexOf(sTemp[sTemp.Length - 1]) + 1;
            }

            nSelWordEncoding = m_oEncoder.Encode_U(sTemp, arrEncoded);
            if (sAccent.Length > 0 && nSelWordEncoding == 1)
                sAccent = DecodeAccent(sAccent);

            return !(nSelWordEncoding == 0);
        }

        void myApplication_WindowBeforeRightClick(Microsoft.Office.Interop.Word.Selection Sel, ref bool Cancel)
        {
            // only if the selected word is armenian make the context menu changes, otherwise do not show HySpell menus
            string sWord = Sel.Words.First.Text;
            int nIndex = -1;
            if (sWord != null)
                nIndex = sWord.IndexOfAny(Globals.ThisAddIn.IsArmenianCharOrAccent);
            if (nIndex != -1 && m_oHunspell == null)
                InitializeWrapper();
            
            ArrayList arrEncoded = new ArrayList();
            string sApostrophe = "";
            string sAccent = "";
            Word.Range rgWord = null;
            if (nIndex != -1 && IsWordArmenian(Sel, ref arrEncoded, ref sApostrophe, ref sAccent, ref rgWord))
            {
                // based on spelling correctness show the HySpell context menus
                rgWord.SpellingChecked = false;
                string sSCIIOutput = arrEncoded[0].ToString();
                bool bLegal = m_oHunspell.Spell(sSCIIOutput);
                if (!bLegal)
                {
                    ArrayList arrSuggests = new ArrayList();
                    List<string> suggestions = m_oHunspell.Suggest(sSCIIOutput);
                    if (suggestions.Count > 0)
                    {
                        foreach (string suggestion in suggestions)
                        {
                            string sWrd = suggestion.Insert(1, sApostrophe);
                            arrSuggests.Add(PutAccent(sWrd, sAccent));
                        }
                    }

                    nCurrentWordStart = rgWord.Start;
                    nCurrentWordEnd = rgWord.End;
                    // construct hs_Spelling context menu based on suggestions
                    AddContextMenu("hs_Spelling", arrSuggests);
                    Cancel = true;

                }
                else
                {
                    Sel.Words.First.SpellingChecked = true;

                    nCurrentWordStart = rgWord.Start;
                    nCurrentWordEnd = rgWord.End;

                    //string s;
                    //s = Sel.Words.First.Text;
                    //int nLinks = Sel.Words.First.Hyperlinks.Count;  // ? > 0
                    //int nTables = Sel.Words.First.Tables.Count;     // ? > 0
                    //object oListStyle = Sel.Words.First.ListStyle;  // ? == null

                    // construct context menu in the other cases with synonyms sub-menu
                    AddMainMenuItems("Text");
                    AddMainMenuItems("Lists");
                    AddMainMenuItems("Table Text");
                    AddMainMenuItems("Table Lists");
                    AddMainMenuItems("Hyperlink Context Menu");
                    AddMainMenuItems("Headings");
                    AddMainMenuItems("Footnotes");
                }

            }
            else
            {
                RemoveExistingMenuItem("Text");
                RemoveExistingMenuItem("Lists");
                RemoveExistingMenuItem("Table Text");
                RemoveExistingMenuItem("Table Lists");
                RemoveExistingMenuItem("Hyperlink Context Menu");
                RemoveExistingMenuItem("Headings");
                RemoveExistingMenuItem("Footnotes");
            }
        }

        public bool ByPassAppWinSelectChange = false;

        void myApplication_WindowSelectionChange(Microsoft.Office.Interop.Word.Selection Sel)
        {
            bool bTaskPaneIsVisible = false;
            Word.Document doc = this.Application.ActiveDocument;
            HSTaskPane cCurrentPane = null;
            foreach (Microsoft.Office.Tools.CustomTaskPane cPane in this.CustomTaskPanes)
            {
                Word.Window cpWindow = (Word.Window)cPane.Window;
                if (cPane.Title == sTaskPaneName && cpWindow == doc.ActiveWindow && cPane.Visible)
                {
                    bTaskPaneIsVisible = true;
                    cCurrentPane = (cPane.Control as HSTaskPane);
                }
            }

            if (!bTaskPaneIsVisible) return;

            string sWord = Sel.Words.First.Text;
            int nIndex = sWord.IndexOfAny(Globals.ThisAddIn.IsArmenianCharOrAccent);
            if (nIndex != -1 && m_oHunspell == null)
                InitializeWrapper();
            
            ArrayList arrEncoded = new ArrayList();
            string sApostrophe = "";
            string sAccent = "";
            Word.Range rgWord = null;
            if (nIndex != -1 && IsWordArmenian(Sel, ref arrEncoded, ref sApostrophe, ref sAccent, ref rgWord))
            {
                // based on spelling correctness show the HySpell context menus
                ((TextBox)cCurrentPane.Controls["tabControl"].Controls[0].Controls["txtMisspellIndicator"]).Text = rgWord.Text;
                //rgWord.LanguageID = Microsoft.Office.Interop.Word.WdLanguageID.wdArmenian;
                rgWord.SpellingChecked = false;
                string sSCIIOutput = arrEncoded[0].ToString();
                bool bLegal = m_oHunspell.Spell(sSCIIOutput);
                if (!bLegal)
                {
                    ((TextBox)cCurrentPane.Controls["tabControl"].Controls[0].Controls["txtMisspellIndicator"]).ForeColor = System.Drawing.Color.White;
                    ((TextBox)cCurrentPane.Controls["tabControl"].Controls[0].Controls["txtMisspellIndicator"]).BackColor = System.Drawing.Color.Red;
                }
                else
                {
                    Sel.Words.First.SpellingChecked = true;
                    ((TextBox)cCurrentPane.Controls["tabControl"].Controls[0].Controls["txtMisspellIndicator"]).ForeColor = System.Drawing.Color.Black;
                    ((TextBox)cCurrentPane.Controls["tabControl"].Controls[0].Controls["txtMisspellIndicator"]).BackColor = System.Drawing.Color.White;
                }
            }
            else
            {
                ((TextBox)cCurrentPane.Controls["tabControl"].Controls[0].Controls["txtMisspellIndicator"]).Text = sWord;
                ((TextBox)cCurrentPane.Controls["tabControl"].Controls[0].Controls["txtMisspellIndicator"]).ForeColor = System.Drawing.Color.Black;
                ((TextBox)cCurrentPane.Controls["tabControl"].Controls[0].Controls["txtMisspellIndicator"]).BackColor = System.Drawing.Color.White;
            }

            if (cCurrentPane.Controls["tabControl"].Controls["tabPage1"] != null && !bInitSpellCheckCall && !ByPassAppWinSelectChange)
                (myCustomTaskPane.Control as HSTaskPane).SetSpellingState("Ստուգել", false);
            bInitSpellCheckCall = false;
            ByPassAppWinSelectChange = false;
        }

        void MenuControl_Click(Microsoft.Office.Core.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            string[] sTemps = Ctrl.Tag.Replace("hs_MenuItem", "").Split(':');
            int nTagNum = Convert.ToInt32(sTemps[0]);
            if (nTagNum < 20)
            {
                // correct the current misspelled word with the selected suggest word
                string sSuggest = Ctrl.Caption;
                Object oRS = nCurrentWordStart;
                Object oRE = nCurrentWordEnd;
                Word.Range rgCurrWord = myApplication.ActiveDocument.Range(ref oRS, ref oRE);
                // check case for non-unicode text
                if (nSelWordEncoding < 2)
                    sSuggest = EncodeWithAccents(sSuggest);
                rgCurrWord.Text = sSuggest;
            }
            else
            {
                switch (nTagNum)
                {
                    case 21: // Ignore
                        break;
                    case 22: // Ignore All
                        if (m_oEncoder != null && m_oHunspell != null)
                        {
                            Object oRS = nCurrentWordStart;
                            Object oRE = nCurrentWordEnd;
                            Word.Range rgCurrWord = myApplication.ActiveDocument.Range(ref oRS, ref oRE);
                            ArrayList arrEncoded = new ArrayList();
                            int nRetFlag = m_oEncoder.Encode(rgCurrWord.Text, arrEncoded);
                            if (arrEncoded.Count != 0)
                                m_oHunspell.Add(arrEncoded[0].ToString());
                        }
                        break;
                    case 23: // Add to Dictionary
                        if (m_oHunspell != null)
                        {
                            frmAddNewWord dlg = new frmAddNewWord();
                            if (Globals.ThisAddIn.ProgramSettings.OrthographyType == 1)
                                dlg.IsClassicOrthography = false;
                            Object oRS = nCurrentWordStart;
                            Object oRE = nCurrentWordEnd;
                            Word.Range rgCurrWord = myApplication.ActiveDocument.Range(ref oRS, ref oRE);
                            dlg.HSWrapper = m_oHunspell;
                            dlg.NewWord = rgCurrWord.Text;
                            if (dlg.ShowDialog() == DialogResult.OK)
                            {
                                string sNewWord = dlg.NewWord;
                                string sExample = dlg.AffixExample;
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
                            }
                        }
                        break;
                    case 24: // Look Up
                        {
                            Object oRS = nCurrentWordStart;
                            Object oRE = nCurrentWordEnd;
                            Word.Range rgCurrWord = myApplication.ActiveDocument.Range(ref oRS, ref oRE);
                            string sWord = rgCurrWord.Text;
                            ShowTaskPane(sWord);
                        }
                        break;
                    default:
                        break;
                }
            }
            CancelDefault = true;
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
                    using (StreamWriter sw = File.CreateText(sCustomDictPath)){}
                }
                using (StreamWriter sw = File.AppendText(sCustomDictPath))
                {
                    sw.WriteLine(newWord);
                }
            }
            catch {}
        }

        public void ShowTaskPane(string sWord)
        {
            bool bTaskPaneIsOpen = false;
            Word.Document doc = this.Application.ActiveDocument;
            foreach (Microsoft.Office.Tools.CustomTaskPane cPane in this.CustomTaskPanes)
            {
                Word.Window cpWindow = (Word.Window)cPane.Window;
                if (cPane.Title == sTaskPaneName && cpWindow == doc.ActiveWindow)
                {
                    cPane.Visible = true;
                    bTaskPaneIsOpen = true;
                }
            }
            if (!bTaskPaneIsOpen)
            {
                if (m_oHunspell == null)
                    InitializeWrapper();
                oHSTaskPaneControl = new HSTaskPane(m_oHunspell);
                myCustomTaskPane = this.CustomTaskPanes.Add(oHSTaskPaneControl, sTaskPaneName, doc.ActiveWindow);
                myCustomTaskPane.Visible = true;
                (myCustomTaskPane.Control.Controls[0] as TabControl).TabPages.RemoveAt(1);
            }
            (myCustomTaskPane.Control.Controls[0] as TabControl).SelectTab(0);
            if (sWord.Length > 0)
            {
                foreach (Microsoft.Office.Tools.CustomTaskPane cPane in this.CustomTaskPanes)
                {
                    Word.Window cpWindow = (Word.Window)cPane.Window;
                    if (cPane.Title == sTaskPaneName && cpWindow == doc.ActiveWindow)
                    {
                        (cPane.Control as HSTaskPane).FindMeaning(sWord, true);
                        break;
                    }
                }                
            }

        }
        public void StartSpellingViaTaskPane()
        {
            bool bTaskPaneIsOpen = false;
            Word.Document doc = this.Application.ActiveDocument;
            foreach (Microsoft.Office.Tools.CustomTaskPane cPane in this.CustomTaskPanes)
            {
                Word.Window cpWindow = (Word.Window)cPane.Window;
                if (cPane.Title == sTaskPaneName && cpWindow == doc.ActiveWindow)
                {
                    cPane.Visible = true;
                    bTaskPaneIsOpen = true;
                }
            }
            if (!bTaskPaneIsOpen)
            {
                oHSTaskPaneControl = new HSTaskPane(m_oHunspell);
                myCustomTaskPane = this.CustomTaskPanes.Add(oHSTaskPaneControl, sTaskPaneName, doc.ActiveWindow);
                myCustomTaskPane.Visible = true;
            }

            foreach (Microsoft.Office.Tools.CustomTaskPane cPane in this.CustomTaskPanes)
            {
                Word.Window cpWindow = (Word.Window)cPane.Window;
                if (cPane.Title == sTaskPaneName && cpWindow == doc.ActiveWindow)
                {
                    (cPane.Control as HSTaskPane).StartSpellChecking();
                    break;
                }
            }           
        }

        public void RemoveTaskPanes()
        {
            if (this.Application.Documents.Count > 0)
            {
                for (int i = this.CustomTaskPanes.Count; i > 0; i--)
                {
                    Microsoft.Office.Tools.CustomTaskPane cPane = this.CustomTaskPanes[i - 1];
                    if (cPane.Title == sTaskPaneName)
                        this.CustomTaskPanes.Remove(cPane);
                }
            }
        }
        void AutoCorrectMenuControl_Click(Microsoft.Office.Core.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            // add entries to the autocorrect list
            string sAutoCorrectWord = Ctrl.Caption;
            Object oRS = nCurrentWordStart;
            Object oRE = nCurrentWordEnd;
            Word.Range rgCurrWord = myApplication.ActiveDocument.Range(ref oRS, ref oRE);
            this.Application.AutoCorrect.Entries.Add(rgCurrWord.Text, sAutoCorrectWord);

            CancelDefault = true;
        }
        private void AddMenu_Click(object sender, EventArgs e)
        {
            // loop through all words spell checking each word and displaying red underlines wherever misspelled
//
//            if (hs_SEStyle == null)
//            {
//                object type = Word.WdStyleType.wdStyleTypeCharacter;
//                hs_SEStyle = myApplication.ActiveDocument.Styles.Add("hs_SpellingErrorStyle", ref type);
//                hs_SEStyle.Font.Underline = Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineWavy;
//                hs_SEStyle.Font.UnderlineColor = Microsoft.Office.Interop.Word.WdColor.wdColorRed;
//                hs_SEStyle.Hidden = true; 
//            }
//            else
//                hs_SEStyle.Font.Underline = Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineWavy;

            // initialize variables
            bShowErrors = true;
            bStart = true;
            bCheckNormalText = true;
            bCursorInShape = false;
            bRestartPar = false;
            bHasPrevTextBoxLink = false;
            nHit = 0;
            nCursorPos = 0;
            nParIndex = 1;
            nShapeIndex = 0;
            nParCount = 0;
            nParOffset = 0;
            nIParOffset = 0;
            nWordLen = 1;
            nRetFlag = -1;
            nStartPos = 0;
            j = 0;
//

            nCursorPos = myApplication.Selection.Start;
            // get all shapes in the document
            oShapes = myApplication.ActiveDocument.Shapes;
            Word.ShapeRange oShpRng = myApplication.Selection.ShapeRange;
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
        }
        private void RestartParagraph()
        {
            bHasPrevTextBoxLink = false;

            if (bCheckNormalText)
            {
                // get all paragraphs of normal text area (including field regions)
                oPars = myApplication.ActiveDocument.Paragraphs;
                nParCount = oPars.Count;

                Word.Paragraph CurPar = oPars[nParIndex];
                int nCurPos = Math.Max(CurPar.Range.Start, nCursorPos);

                // start with the range of the whole active document
                Object oRS = null, oRE = null;
                Word.Range rg = myApplication.ActiveDocument.Range(ref oRS, ref oRE);
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
                    return;
                }
            }
            else
            {
                if (!bCursorInShape)
                    nCursorPos = 0;
                if (nShapeIndex > oShapes.Count)
                    return;
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
            CurRange = par.Range;
            CurRange.Start += nIParOffset;
            sParText = par.Range.Text;
            CheckNext_U();
        }
        private void CheckNext_U()
        {
            nStartPos += nWordLen;
            if (bStart)
            {
                // extract all possible punctuation characters
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
            }

            if (j < ArWords.Length && !bRestartPar)
            {
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

                if (m_oEncoder != null)
                {
                    if (sInput.Length > 0)
                    {
                        sUniInput = m_oEncoder.EncodeASCIIStringToUnicode(sInput);
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
                                // select the corresponding word in the source document
                                SelWordRange = null;
                                Word.Range rg = CurRange.Duplicate;
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
                                if (SelWordRange != null)
                                {
                                    bool bLegal = m_oHunspell2.Spell(sSCIIOutput);
                                    int nIsNotException = 1;
                                    if (!m_bFromClassic)
                                        nIsNotException = IsNotExceptionWord(sSCIIOutput);
                                    if (nRetFlag >= 2)
                                        SelWordRange.LanguageID = Microsoft.Office.Interop.Word.WdLanguageID.wdArmenian;
                                    if (!bLegal || nIsNotException == 0)
                                    {
                                        // check ALLCAP or FIRSTCAP cases
                                        if (IsAllCapitized(sSCIIOutput))
                                            enCapState = CapState.enAllCaps;
                                        else if (IsCapitized(sSCIIOutput))
                                            enCapState = CapState.enFirstCap;
                                        else
                                            enCapState = CapState.enNone;
                                        // check if stem-affix is found in dic
                                        List<string> anals = m_oHunspell1.Analyze(sSCIIOutput);
                                        if (anals.Count > 0)
                                        {
                                            // if found, then simply map (stem, affix) using lex and flex dictionaries
                                            string sAnItem = anals[0].Trim();
                                            if (m_bFromClassic)
                                            {
                                                // if from classic orthography to reform
                                                string sOut = "";
                                                if (TransformWord(sAnItem, ref sOut))
                                                {
                                                    // put back the accent if applicable
                                                    if (sAccent.Length > 0)
                                                        sOut = Globals.ThisAddIn.PutAccent(sOut, sAccent);
                                                    nWordLen = SelWordRange.Text.Length;
                                                    if (nRetFlag == 1)
                                                        sOut = EncodeWithAccents(sOut, m_oEncoder);
                                                    SelWordRange.Text = (nRetFlag == 3) ? sOut + "»" : sOut;
                                                }
                                                else
                                                {
                                                    // put back the accent if applicable
                                                    if (sAccent.Length > 0)
                                                        sSCIIOutput = Globals.ThisAddIn.PutAccent(sSCIIOutput, sAccent);
                                                    // prompt user to manually enter a target word in frmConvertOrtho form with no suggestions
                                                    nWordLen = SelWordRange.Text.Length;
                                                    return;
                                                }
                                            }
                                            else
                                            {
                                                // from reform to classic must use different form
                                                ArrayList arrOut = new ArrayList();
                                                int nOutWordCount = TransformWord(sAnItem, ref arrOut, sAccent);
                                                // based on the output word count, if single word, then process silently,
                                                // otherwise prompt user for further input
                                                if (nOutWordCount == 1)
                                                {
                                                    // put back the accent if applicable
                                                    string sOut = arrOut[0].ToString();
                                                    //if (sAccent.Length > 0)
                                                    //    sOut = Globals.ThisAddIn.PutAccent(sOut, sAccent);
                                                    nWordLen = SelWordRange.Text.Length;
                                                    if (nRetFlag == 1)
                                                        sOut = EncodeWithAccents(sOut, m_oEncoder);
                                                    SelWordRange.Text = (nRetFlag == 3) ? sOut + "»" : sOut;
                                                }
                                                else if (nOutWordCount == 2)
                                                {
                                                    // reset the member arrays to null
                                                    if (arrPossibleWords != null)
                                                    {
                                                        arrPossibleWords.Clear();
                                                        arrPossibleWords = null;
                                                    }
                                                    arrPossibleWords = new ArrayList();
                                                    for (int k = 0; k < arrOut.Count; k++)
                                                    {
                                                        string sO = arrOut[k].ToString();
                                                        //if (sAccent.Length > 0)
                                                        //    sO = Globals.ThisAddIn.PutAccent(sO, sAccent);
                                                        arrPossibleWords.Add(sO);
                                                    }
                                                    // prompt user to select possible word or manually enter a target word in frmConvertOrthoC form (no suggestions)
                                                    nWordLen = SelWordRange.Text.Length;
                                                    return;
                                                }
                                                else
                                                {
                                                    // put back the accent if applicable
                                                    if (sAccent.Length > 0)
                                                        sSCIIOutput = Globals.ThisAddIn.PutAccent(sSCIIOutput, sAccent);
                                                    // prompt user to manually enter a target word in frmConvertOrthoC form with no suggestions
                                                    nWordLen = SelWordRange.Text.Length;
                                                    return;
                                                }
                                            }
                                            CheckNext_U();
                                        }
                                        else
                                        {
                                            // display status in frmConvertOrtho form with suggestions
                                            if (arrSuggestList != null)
                                            {
                                                arrSuggestList.Clear();
                                                arrSuggestList = null;
                                            }
                                            arrSuggestList = new ArrayList();
                                            List<string> suggests = m_oHunspell1.Suggest(sSCIIOutput);
                                            // put back the accent 
                                            if (sAccent.Length > 0)
                                                sSCIIOutput = Globals.ThisAddIn.PutAccent(sSCIIOutput, sAccent);
                                            for (int i = 0; i < suggests.Count; i++)
                                            {
                                                string sAccentedWord = suggests[i];
                                                if (sAccent.Length > 0)
                                                    sAccentedWord = Globals.ThisAddIn.PutAccent(sAccentedWord, sAccent);
                                                arrSuggestList.Add(sAccentedWord);
                                            }
                                            nWordLen = SelWordRange.Text.Length;
                                            return;
                                        }
                                    }
                                    else
                                        CheckNext_U();
                                }
                                else
                                    CheckNext_U();
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
                        Word.Range rg = myApplication.ActiveDocument.Range(ref oRS, ref oRE);
                        rg.End = int.MaxValue;
                        // search for the first armenian character starting from the current cursor position
                        nCurPos = SearchForArmenianChar(rg, nCurPos);
                        //if (nParIndex >= nParCount)
                        if (nParIndex > nParCount)
                        {
                            bCheckNormalText = false;
                            nShapeIndex++;
                            RestartParagraph();
                            bTerminateOrthoConvert = true;
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
                    {
                        bTerminateOrthoConvert = true;
                        return;
                    }
                }
            }
        }

        public void SubstituteAndSelectWord(string sInWord, string sOutWord)
        {
            if (sInWord != sOutWord)
            {
                ArrayList arrDecoded = new ArrayList();
                bool bRet = m_oEncoder.DecodeMixedWithAccents(sOutWord, arrDecoded);
                if (arrDecoded.Count > 0)
                    sOutWord = arrDecoded[0].ToString();
                if (nRetFlag == 1)
                    sOutWord = EncodeWithAccents(sOutWord, m_oEncoder);
                SelWordRange.Text = (nRetFlag == 3) ? sOutWord + "»" : sOutWord;
            }
            SelWordRange.Select();
        }

        private bool IsAllCapitized(string sWord)
        {
            if (sWord.Length == 0) 
                return false;
            return string.Compare(sWord, sWord.ToUpper(), false) == 0;
        }
        // use this function after checking all-caps case
        private bool IsCapitized(string sWord)
        {
            if (sWord.Length == 0)
                return false;
            return string.Compare(sWord.Substring(0, 1), sWord.Substring(0, 1).ToUpper(), false) == 0;
        }
        private bool IsFirstCapitized(string sWord)
        {
            if (sWord.Length == 0)
                return false;
            return string.Compare(sWord.Substring(0, 1), sWord.Substring(0, 1).ToUpper(), false) == 0
                    && string.Compare(sWord, sWord.ToUpper(), false) != 0;
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
        private int SearchForArmenianChar(Word.Range rg, int nCursorPosition)
        {
            nHit++;
            int nNewPos = Math.Max(nCursorPosition, rg.Start);
            int nGap = rg.End - nNewPos;
            while (nGap > 0)
            {
                rg.Start = nNewPos;
                nGap = rg.Text.IndexOfAny(this.IsArmenianChar);
                if (nGap != -1)
                    nNewPos += nGap;
                else
                    nNewPos = rg.End;
            }
            // restrict the range to get the index of the first applicable paragraph
            if (!bHasPrevTextBoxLink) rg.Start = 0;
            rg.End = nNewPos;
            nParIndex = rg.Paragraphs.Count;
            if (oPars[nParIndex].Range.End <= nNewPos)
                nParIndex++;

            return nNewPos;
        }
        private int IsNotExceptionWord(string sWord)
        {
            for (int i = 0; i < Globals.ThisAddIn.IsWordExceptionPattern.Length; i++)
                if (sWord.ToLower().Contains(Globals.ThisAddIn.IsWordExceptionPattern[i]))
                    return 0;
            return 1;
        }
        // return true if transform is successful, false if stem does not map
        private bool TransformWord(string sInAnalsString, ref string sOutWord)
        {
            string sPrefix = "";
            string sInString = sInAnalsString;
            int nFlIndex = sInString.IndexOf(" fl:");
            if (nFlIndex > -1)
                sInString = sInString.Substring(0, nFlIndex);
            if (sInString.StartsWith("["))
            {
                string[] sInitSplits = sInString.Split(new string[] {"]+ "}, StringSplitOptions.RemoveEmptyEntries);
                sPrefix = sInitSplits[0].Substring(1, sInitSplits[0].Length - 1);
                if (m_FlexMapDic.ContainsKey(sPrefix))
                    sPrefix = m_FlexMapDic[sPrefix];
                sInString = sInitSplits[1];
            }
            string[] sOutTerms = sInString.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            string sStem = sOutTerms[0].Replace("st:", "");
            if (m_LexMapDic.ContainsKey(sStem))
                sStem = m_LexMapDic[sStem];
            else
                return false;
            sOutWord = sPrefix + sStem;
            if (sOutTerms.Length > 1)
            {
                string sOutTerm1 = sOutTerms[1];
                if (m_FlexMapDic.ContainsKey(sOutTerm1))
                    sOutTerm1 = m_FlexMapDic[sOutTerm1];
                if (sOutTerm1.IndexOf('|') > 0)
                {
                    string[] sAff1 = sOutTerm1.Split(new char[] { '-', '|', '+' }, StringSplitOptions.RemoveEmptyEntries);
                    if (sOutWord.EndsWith(sAff1[0]))
                    {
                        int nIndex = sOutWord.LastIndexOf(sAff1[0]);
                        sOutWord = sOutWord.Substring(0, nIndex) + sAff1[1];
                    }
                    sOutWord += sAff1[2];
                }
                else
                {
                    string[] sAff1 = sOutTerm1.Split(new char[] { '-', '+' }, StringSplitOptions.RemoveEmptyEntries);
                    if (sAff1.Length == 1)
                        sOutWord += sAff1[0];
                    else if (sAff1.Length == 2)
                    {
                        int nIndex = sOutWord.LastIndexOf(sAff1[0]);
                        sOutWord = sOutWord.Substring(0, nIndex) + sAff1[1];
                    }
                }
            }
            if (sOutTerms.Length > 2)
            {
                string sOutTerm2 = sOutTerms[2];
                if (m_FlexMapDic.ContainsKey(sOutTerm2))
                    sOutTerm2 = m_FlexMapDic[sOutTerm2];
                if (sOutTerm2.IndexOf('|') > 0)
                {
                    string[] sAff2 = sOutTerm2.Split(new char[] { '-', '|', '+' }, StringSplitOptions.RemoveEmptyEntries);
                    if (sOutWord.EndsWith(sAff2[0]))
                    {
                        int nIndex = sOutWord.LastIndexOf(sAff2[0]);
                        sOutWord = sOutWord.Substring(0, nIndex) + sAff2[1];
                    }
                    sOutWord += sAff2[2];
                }
                else
                {
                    string[] sAff2 = sOutTerm2.Split(new char[] { '-', '+' }, StringSplitOptions.RemoveEmptyEntries);
                    if (sAff2.Length == 1)
                        sOutWord += sAff2[0];
                    else if (sAff2.Length == 2)
                    {
                        int nIndex = sOutWord.LastIndexOf(sAff2[0]);
                        sOutWord = sOutWord.Substring(0, nIndex) + sAff2[1];
                    }
                }
            }
            // hanlde the ALLCAP, FIRSTCAP and the yev cases
            switch (enCapState)
            {
                case CapState.enAllCaps:
                    sOutWord = sOutWord.ToUpper(new CultureInfo("hy", false));
                    sOutWord = sOutWord.Replace("և", "ԵՎ");
                    break;
                case CapState.enFirstCap:
                    if (sOutWord.Substring(0, 1) == "և")
                        sOutWord = "Եվ" + sOutWord.Substring(1, sOutWord.Length - 1);
                    else
                        sOutWord = sOutWord.Substring(0, 1).ToUpper(new CultureInfo("hy", false)) + sOutWord.Substring(1, sOutWord.Length - 1);
                    break;
            }

            return true;
        }
        // return 1, if input word trasforms to a unique word, 2 if multiple word, 0 if stem does not map
        private int TransformWord(string sInAnalsString, ref ArrayList arrOutWord, string sAccent)
        {
            int nRet = 0;
            string sInString = sInAnalsString;
            int nFlIndex = sInString.IndexOf(" fl:");
            if (nFlIndex > -1)
                sInString = sInString.Substring(0, nFlIndex);
            string sPrefix = "";
            if (sInString.StartsWith("["))
            {
                string[] sInitSplits = sInString.Split(new string[] { "]+ " }, StringSplitOptions.RemoveEmptyEntries);
                sPrefix = sInitSplits[0].Substring(1, sInitSplits[0].Length - 1);
                if (m_FlexMapDic.ContainsKey(sPrefix))
                    sPrefix = m_FlexMapDic[sPrefix];
                sInString = sInitSplits[1];
            }
            string[] sOutTerms = sInString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string sStem = sOutTerms[0].Replace("st:", "");
            if (m_LexMapDic.ContainsKey(sStem))
                sStem = m_LexMapDic[sStem];
            else
                return nRet;
            // split sStem at ";" character, e.g. հանգեցնել (հանգչեցնել);յանգեցնել (վերջացնել)
            // if sStem has uniquely mapped word then proceed with the standard method returning value 1,
            // else extract each pure-stem part and after inflect recompose with the synonym part
            string[] arrStems = sStem.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            string[] arrSyns = new string[arrStems.Length];
            if (arrStems.Length > 1)
            {
                for (int i = 0; i < arrStems.Length; i++)
                {
                    arrSyns[i] = arrStems[i].Substring(arrStems[i].IndexOf(" ("));
                    arrStems[i] = arrStems[i].Substring(0, arrStems[i].IndexOf(" (")).Trim();
                }
                nRet = 2;
            }
            else
                nRet = 1;
            for (int j = 0; j < arrStems.Length; j++)
            {
                string sOutWord = sPrefix + arrStems[j];
                if (sOutTerms.Length > 1)
                {
                    string sOutTerm1 = sOutTerms[1];
                    if (m_FlexMapDic.ContainsKey(sOutTerm1))
                        sOutTerm1 = m_FlexMapDic[sOutTerm1];
                    if (sOutTerm1.IndexOf('|') > 0)
                    {
                        string[] sAff1 = sOutTerm1.Split(new char[] { '-', '|', '+' }, StringSplitOptions.None);
                        if (sOutWord.EndsWith(sAff1[1]))
                        {
                            int nIndex = sOutWord.LastIndexOf(sAff1[1]);
                            sOutWord = sOutWord.Substring(0, nIndex) + sAff1[2];
                        }
                        sOutWord += sAff1[3];
                    }
                    else
                    {
                        string[] sAff1 = sOutTerm1.Split(new char[] { '-', '+' }, StringSplitOptions.RemoveEmptyEntries);
                        if (sAff1.Length == 1)
                            sOutWord += sAff1[0];
                        else if (sAff1.Length == 2)
                        {
                            int nIndex = sOutWord.LastIndexOf(sAff1[0]);
                            sOutWord = sOutWord.Substring(0, nIndex) + sAff1[1];
                        }
                    }
                }
                if (sOutTerms.Length > 2)
                {
                    string sOutTerm2 = sOutTerms[2];
                    if (m_FlexMapDic.ContainsKey(sOutTerm2))
                        sOutTerm2 = m_FlexMapDic[sOutTerm2];
                    if (sOutTerm2.IndexOf('|') > 0)
                    {
                        string[] sAff2 = sOutTerm2.Split(new char[] { '-', '|', '+' }, StringSplitOptions.None);
                        if (sOutWord.EndsWith(sAff2[1]))
                        {
                            int nIndex = sOutWord.LastIndexOf(sAff2[1]);
                            sOutWord = sOutWord.Substring(0, nIndex) + sAff2[2];
                        }
                        sOutWord += sAff2[3];
                    }
                    else
                    {
                        string[] sAff2 = sOutTerm2.Split(new char[] { '-', '+' }, StringSplitOptions.RemoveEmptyEntries);
                        if (sAff2.Length == 1)
                            sOutWord += sAff2[0];
                        else if (sAff2.Length == 2)
                        {
                            int nIndex = sOutWord.LastIndexOf(sAff2[0]);
                            sOutWord = sOutWord.Substring(0, nIndex) + sAff2[1];
                        }
                    }
                }
                // hanlde the ALLCAP, FIRSTCAP and the yev cases
                switch (enCapState)
                {
                    case CapState.enAllCaps:
                        sOutWord = sOutWord.ToUpper(new CultureInfo("hy", false));
                        sOutWord = sOutWord.Replace("և", "ԵՎ");
                        break;
                    case CapState.enFirstCap:
                        if (sOutWord.Substring(0, 1) == "և")
                            sOutWord = "Եվ" + sOutWord.Substring(1, sOutWord.Length - 1);
                        else
                            sOutWord = sOutWord.Substring(0, 1).ToUpper(new CultureInfo("hy", false)) + sOutWord.Substring(1, sOutWord.Length - 1);
                        break;
                }
                // put back the accent if applicable
                if (sAccent.Length > 0)
                    sOutWord = Globals.ThisAddIn.PutAccent(sOutWord, sAccent);
                // re-concatenate the synonym part if applicable, and add to ouput array
                if (nRet > 1)
                    sOutWord += arrSyns[j];
                arrOutWord.Add(sOutWord);
            }

            return nRet;
        }

        public string ConvertSuggestWord(string sWord)
        {
            if (m_oHunspell1 == null)
                return sWord;
            string sOutWord = sWord;
            // check ALLCAP or FIRSTCAP cases
            if (IsAllCapitized(sWord))
                enCapState = CapState.enAllCaps;
            else if (IsCapitized(sWord))
                enCapState = CapState.enFirstCap;
            else
                enCapState = CapState.enNone;
            // remove accents if any
            string sAccent = "";
            int nAccIndex = sWord.IndexOfAny(Globals.ThisAddIn.IsArmenianAccent);
            if (nAccIndex != -1)
            {
                sAccent = sWord.Substring(nAccIndex, 1);
                sWord = sWord.Remove(nAccIndex, 1);
            }
            // check if stem-affix is found in dic
            List<string> arrAnals = m_oHunspell1.Analyze(sWord);
            if (arrAnals.Count > 0)
            {
                // if found, then simply map (stem, affix) using lex and flex dictionaries
                string sAnItem = arrAnals[0].Trim();
                string sOut = "";
                if (TransformWord(sAnItem, ref sOut))
                {
                    // put back the accent if applicable
                    if (sAccent.Length > 0)
                        sOut = Globals.ThisAddIn.PutAccent(sOut, sAccent);
                    sOutWord = sOut;
                }
            }

            return sOutWord;
        }
        public int ConvertSuggestWord(string sWord, ref ArrayList arrPossWords)
        {
            int nOutWordCount = 0;
            if (m_oHunspell1 == null)
                return nOutWordCount;
            // check ALLCAP or FIRSTCAP cases
            if (IsAllCapitized(sWord))
                enCapState = CapState.enAllCaps;
            else if (IsCapitized(sWord))
                enCapState = CapState.enFirstCap;
            else
                enCapState = CapState.enNone;
            // remove accents if any
            string sAccent = "";
            int nAccIndex = sWord.IndexOfAny(Globals.ThisAddIn.IsArmenianAccent);
            if (nAccIndex != -1)
            {
                sAccent = sWord.Substring(nAccIndex, 1);
                sWord = sWord.Remove(nAccIndex, 1);
            }
            // check if stem-affix is found in dic
            List<string> arrAnals = m_oHunspell1.Analyze(sWord);
            if (arrAnals.Count > 0)
            {
                // if found, then simply map (stem, affix) using lex and flex dictionaries
                string sAnItem = arrAnals[0].Trim();
                nOutWordCount = TransformWord(sAnItem, ref arrPossWords, sAccent);
            }

            return nOutWordCount;
        }

        public void FillDictionaryFromFile(string sFilePath, Dictionary<string, string> oDict)
        {
            StreamReader rd = File.OpenText(sFilePath);
            string sInLine = null;
            while ((sInLine = rd.ReadLine()) != null)
            {
                string[] sep = { ":", "\r\n" };
                string[] sWords = sInLine.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                if (sWords.Length == 2 && !oDict.ContainsKey(sWords[0]))
                    oDict.Add(sWords[0], sWords[1]);
            }
            rd.Close();
        }
        private void InitializeMapDictionaries(string sLexMapPath, string sFlexMapPath)
        {
            m_LexMapDic = new Dictionary<string, string>();
            m_FlexMapDic = new Dictionary<string, string>();
            try
            {
                // fill lexicon map dictionary from file
                FillDictionaryFromFile(sLexMapPath, m_LexMapDic);
                // fill inflection map dictionary from file
                FillDictionaryFromFile(sFlexMapPath, m_FlexMapDic);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void InitializeWrappers()
        {
            if (m_oEncoder == null)
                m_oEncoder = new HySpellEncoder.Wrapper();
            
            string sPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            m_oHunspell1 = new Hunspell();
            m_oHunspell2 = new Hunspell();
            if (m_bFromClassic)
            {
                m_oHunspell1 = new Hunspell(sPath + @"\hyspell\dictc\hy-c.aff", sPath + @"\hyspell\dictc\hy-c.dic");
                AddWordsFromCustomDict(sPath + @"\hyspell\dictc\Custom.dic", ref m_oHunspell1);
                m_oHunspell2 = new Hunspell(sPath + @"\hyspell\dictr\hy-r.aff", sPath + @"\hyspell\dictr\hy-r.dic");
                AddWordsFromCustomDict(sPath + @"\hyspell\dictr\Custom.dic", ref m_oHunspell2);   //??
                InitializeMapDictionaries(sPath + @"\hyspell\dictc\CRLexMap.dic", sPath + @"\hyspell\dictc\CRFLexMap.dic");
            }
            else
            {
                m_oHunspell1 = new Hunspell(sPath + @"\hyspell\dictr\hy-r.aff", sPath + @"\hyspell\dictr\hy-r.dic");
                AddWordsFromCustomDict(sPath + @"\hyspell\dictr\Custom.dic", ref m_oHunspell1);

                m_oHunspell2 = new Hunspell(sPath + @"\hyspell\dictc\hy-c.aff", sPath + @"\hyspell\dictc\hy-c.dic");
                AddWordsFromCustomDict(sPath + @"\hyspell\dictc\Custom.dic", ref m_oHunspell2); //??
                // in this case, it is assumed that a value of the dictionary may comprise
                // of multiple word separated by the ";" delimiter
                InitializeMapDictionaries(sPath + @"\hyspell\dictr\RCLexMap.dic",
                                                sPath + @"\hyspell\dictr\RCFLexMap.dic");
            }
        }
        public void UnInitializeWrappers()
        {
            // dispose map dictionaries
            if (m_LexMapDic != null)
            {
                m_LexMapDic.Clear();
                m_LexMapDic = null;
            }
            if (m_FlexMapDic != null)
            {
                m_FlexMapDic.Clear();
                m_FlexMapDic = null;
            }
            // dispose wrappers
            if (m_oHunspell1 != null)
            {
                if (m_oHunspell1 is IDisposable)
                    m_oHunspell1.Dispose();
                m_oHunspell1 = null;
            }
            if (m_oHunspell2 != null)
            {
                if (m_oHunspell2 is IDisposable)
                    m_oHunspell2.Dispose();
                m_oHunspell2 = null;
            }
        }
        public void ConvertOrthography(bool bFromClassic)
        {
            // reload wrappers if convert direction changes or wrappers are null
            if (bFromClassic != m_bFromClassic || m_oHunspell1 == null || m_oHunspell2 == null)
            {
                UnInitializeWrappers();
                m_bFromClassic = bFromClassic;
                InitializeWrappers();
            }
            // initialize variables
            bStart = true;
            bCheckNormalText = true;
            bCursorInShape = false;
            nShapeIndex = 0;
            bRestartPar = false;
            bHasPrevTextBoxLink = false;
            nParCount = 0;
            nParIndex = 1;
            nCursorPos = 0;
            nParOffset = 0;
            nIParOffset = 0;
            nStartPos = 0;
            j = 0;
            nWordLen = 1;
            nRetFlag = -1;
            nHit = 0;
            nCursorPos = myApplication.Selection.End;
            // get all shapes in the document
            oShapes = myApplication.ActiveDocument.Shapes;
            Word.ShapeRange oShpRng = myApplication.Selection.ShapeRange;
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
            if ((nParIndex > nParCount || nParCount == 0) && (nShapeIndex > oShapes.Count || oShapes.Count == 0))
                bTerminateOrthoConvert = true;
        }

        private Boolean ExecuteFind(Word.Find find)
        {
            return ExecuteFind(find, Type.Missing, Type.Missing);
        }
        private Boolean ExecuteFind(
          Word.Find find, Object wrapFind, Object forwardFind)
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

        private void RemoveSpellErrorFormats()
        {
            for (int i = 1; i <= CurRange.Words.Count; i++)
            {
                if (CurRange.Words[i].Underline == Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineWavy
                                            && CurRange.Words[i].Font.UnderlineColor == Microsoft.Office.Interop.Word.WdColor.wdColorRed)
                    CurRange.Words[i].Underline = Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineNone;
            }

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
                    Word.Range rg = myApplication.ActiveDocument.Range(ref oRS, ref oRE);
                    rg.End = int.MaxValue;
                    // search for the first armenian character starting from the current cursor position
                    nCurPos = SearchForArmenianChar(rg, nCurPos);
                    //if (nParIndex >= nParCount)
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
                    return;
            }

        }

        private void RemoveMenu_Click(object sender, EventArgs e)
        {
            //////hs_SEStyle.Font.Underline = Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineNone;
            ////////return;

            ////Cursor.Current = Cursors.WaitCursor;

            ////bShowErrors = false;
            ////bStart = true;
            ////bCheckNormalText = true;
            ////bCursorInShape = false;
            ////bRestartPar = false;
            ////bHasPrevTextBoxLink = false;
            ////nHit = 0;
            ////nCursorPos = 0;
            ////nParIndex = 1;
            ////nShapeIndex = 0;
            ////nParCount = 0;
            ////nParOffset = 0;
            ////nIParOffset = 0;
            ////nWordLen = 1;
            ////nRetFlag = -1;
            ////nStartPos = 0;
            ////j = 0;
            //////

            ////nCursorPos = myApplication.Selection.Start;
            ////// get all shapes in the document
            ////oShapes = myApplication.ActiveDocument.Shapes;
            ////Word.ShapeRange oShpRng = myApplication.Selection.ShapeRange;
            ////if (oShpRng.Count > 0)
            ////{
            ////    object obj = 1;
            ////    Word.Shape shp = oShpRng.get_Item(ref obj);
            ////    bCheckNormalText = false;
            ////    bCursorInShape = true;

            ////    for (int i = 1; i <= oShapes.Count; i++)
            ////    {
            ////        obj = i;
            ////        nShapeIndex = i;
            ////        Word.Shape oShp = oShapes.get_Item(ref obj);
            ////        if (oShp.Name == shp.Name)
            ////            break;
            ////    }
            ////}
            ////RestartParagraph();
        }

        #region VSTO generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        #endregion

        public int ConvertTextFileToUTF8(string sSrcPath, string sDstPath, bool bInputFileIsUTF8)
        {
            // exit of input file not found
            if (!File.Exists(sSrcPath))
                return -1;
            // if path is missing subfolders, create them
            string sPath = sDstPath.Substring(0, sDstPath.LastIndexOf(@"\"));
            if (!Directory.Exists(sPath))
                Directory.CreateDirectory(sPath);
            // read and process line by line
            StreamReader sr = null;
            StreamWriter sw = null;
            try
            {
                // if input file is UTF-8 then use OpenText, otherwise assume code-page = 1252
                if (bInputFileIsUTF8)
                    sr = File.OpenText(sSrcPath);
                else
                    sr = new StreamReader( (System.IO.Stream)File.OpenRead(sSrcPath),
                                        System.Text.Encoding.GetEncoding("Windows-1252"));
                sw = File.CreateText(sDstPath);
                string sLine;
                while ((sLine = sr.ReadLine()) != null)
                {
                    sLine = m_oEncoder.EncodeASCIIStringToUnicode(sLine);
                    sw.WriteLine(sLine);
                }                
            }
            catch (IOException ioex)
            {
                MessageBox.Show("Error: " + ioex.Message);
                return -2;
            }
            finally
            {
                sr.Close();
                sw.Close();
            }

            return 0;
        }

        private void AddWordsFromCustomDict(string path, ref Hunspell hunspell)
        {
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);
                foreach (var line in lines)
                {
                    string[] wordParts = line.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                    if (wordParts.Length > 1)
                        hunspell.AddWithAffix(wordParts[0], wordParts[1]);
                    else
                        hunspell.Add(line);
                }
            }
        }

        public void InitializeWrapper()
        {
            InitializeWrapper(true);
        }
        public void InitializeWrapper(bool bFromFile)
        {
            string sPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (bFromFile)
            {
                string sXMLPath = sPath + @"\hyspell\HyspellSettings.xml";
                m_oSettings = DeserializeFromXML(m_oSettings, sXMLPath);
            }

            // load encoder and hunspell libs
            m_oEncoder = new HySpellEncoder.Wrapper();
            if (m_oSettings.OrthographyType == 0)
            {
                m_oHunspell = new Hunspell(sPath + @"\hyspell\dictc\hy-c.aff", sPath + @"\hyspell\dictc\hy-c.dic");
                sCustomDictPath = sPath + @"\hyspell\dictc\Custom.dic";
            }
            else
            {
                m_oHunspell = new Hunspell(sPath + @"\hyspell\dictr\hy-r.aff", sPath + @"\hyspell\dictr\hy-r.dic");
                sCustomDictPath = sPath + @"\hyspell\dictr\Custom.dic";
            }
            AddWordsFromCustomDict(sCustomDictPath, ref m_oHunspell);
        }

        public void UnInitializeWrapper()
        {
            if (m_oHunspell != null)
            {
                if (m_oHunspell is IDisposable)
                    m_oHunspell.Dispose();
                m_oHunspell = null;
            }
            if (m_oEncoder != null)
            {
                if (m_oEncoder is IDisposable)
                    m_oEncoder.Dispose();
                m_oEncoder = null;
            }
        }

        private void AddMainMenuItems(string sMenuBarName)
        {
            Office.CommandBar contextMenu = myApplication.CommandBars[sMenuBarName];
            Office.MsoControlType menuItem = Office.MsoControlType.msoControlButton;
            Office.CommandBarButton control =
                (Office.CommandBarButton)contextMenu.FindControl
                (Office.MsoControlType.msoControlButton, missing,
                "hs_MenuItem24", true, true);
            if (control != null)
                RemoveExistingMenuItem(sMenuBarName);
            hsMenuControlLookup =
                (Office.CommandBarButton)myApplication.CommandBars[sMenuBarName].Controls.Add
                (menuItem, missing, missing, 1, true);
            hsMenuControlLookup.Style = Office.MsoButtonStyle.msoButtonCaption;
            hsMenuControlLookup.Caption = "Loo&k Up (Բառարան)...";
            hsMenuControlLookup.Tag = "hs_MenuItem24";
            hsMenuControlLookup.BeginGroup = true;
            hsMenuControlLookup.Click +=
                new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler(MenuControl_Click);

            //hsMenuControlSyn =
            //    (Office.CommandBarButton)myApplication.CommandBars[sMenuBarName].Controls.Add
            //    (menuItem, missing, missing, 2, true);
            //hsMenuControlSyn.Style = Office.MsoButtonStyle.msoButtonCaption;
            //hsMenuControlSyn.Caption = "&Synonyms (Հոմանիշներ)...";
            //hsMenuControlSyn.Tag = "hs_MenuItem52";
            //hsMenuControlSyn.Click +=
            //    new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler(MenuControl_Click);
        }
        private void RemoveExistingMenuItem(string sMenuBarName)
        {
            try
            {
                Office.CommandBar contextMenu = myApplication.CommandBars[sMenuBarName];

                for (int i = 1; i < 31; i++)
                {
                    Office.CommandBarButton control =
                        (Office.CommandBarButton)contextMenu.FindControl
                        (Office.MsoControlType.msoControlButton, missing,
                        "hs_MenuItem" + i.ToString(), true, true);
                    if ((control != null))
                    {
                        control.Delete(true);
                    }
                }
            }
            catch { };

        }
        private void AddContextMenu(string sHSCommandBarName, ArrayList arrSuggests)
        {
            commandBar = myApplication.CommandBars.Add(sHSCommandBarName, 
                Office.MsoBarPosition.msoBarPopup, missing, true);

            if (commandBar != null)
            {
                hsMenuControls = new Microsoft.Office.Core.CommandBarButton[30];
                hsSubMenuControls = new Microsoft.Office.Core.CommandBarButton[30];
                int nCount = 1;
                for (int i = 0; i < Math.Min(20, arrSuggests.Count); i++)
                {
                    hsMenuControls[i] = (Office.CommandBarButton)commandBar.Controls.Add(
                    Office.MsoControlType.msoControlButton, missing, missing, missing, true);
                    hsMenuControls[i].Style = Office.MsoButtonStyle.msoButtonCaption;
                    hsMenuControls[i].Caption = arrSuggests[i].ToString();
                    hsMenuControls[i].Tag = "hs_MenuItem" + nCount.ToString() + ":" + Guid.NewGuid().ToString();
                    hsMenuControls[i].Click +=
                        new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler(MenuControl_Click);
                    nCount++;
                }
                hsMenuControls[nCount - 1] = (Office.CommandBarButton)commandBar.Controls.Add(
                    Office.MsoControlType.msoControlButton, missing, missing, missing, true);
                hsMenuControls[nCount - 1].Style = Office.MsoButtonStyle.msoButtonCaption;
                hsMenuControls[nCount - 1].Caption = "I&gnore (Անտեսել)";
                hsMenuControls[nCount - 1].Tag = "hs_MenuItem21" + ":" + Guid.NewGuid().ToString();
                hsMenuControls[nCount - 1].BeginGroup = true;
                hsMenuControls[nCount - 1].Click +=
                    new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler(MenuControl_Click);
                nCount++;
                hsMenuControls[nCount - 1] = (Office.CommandBarButton)commandBar.Controls.Add(
                    Office.MsoControlType.msoControlButton, missing, missing, missing, true);
                hsMenuControls[nCount - 1].Style = Office.MsoButtonStyle.msoButtonCaption;
                hsMenuControls[nCount - 1].Caption = "&Ignore All (Անտեսել Բոլորը)";
                hsMenuControls[nCount - 1].Tag = "hs_MenuItem22" + ":" + Guid.NewGuid().ToString();
                hsMenuControls[nCount - 1].Click +=
                    new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler(MenuControl_Click);
                nCount++;
                hsMenuControls[nCount - 1] = (Office.CommandBarButton)commandBar.Controls.Add(
                    Office.MsoControlType.msoControlButton, missing, missing, missing, true);
                hsMenuControls[nCount - 1].Style = Office.MsoButtonStyle.msoButtonCaption;
                hsMenuControls[nCount - 1].Caption = "&Add to Dictionary (Աւելցնել Բառարան)...";
                hsMenuControls[nCount - 1].Tag = "hs_MenuItem23" + ":" + Guid.NewGuid().ToString();
                hsMenuControls[nCount - 1].Click +=
                    new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler(MenuControl_Click);
                nCount++;

                hsMenuControls[nCount - 1] = (Office.CommandBarButton)commandBar.Controls.Add(
                    Office.MsoControlType.msoControlButton, missing, missing, missing, true);
                hsMenuControls[nCount - 1].Style = Office.MsoButtonStyle.msoButtonIconAndCaption;
                hsMenuControls[nCount - 1].Caption = "Loo&k Up (Բառարան)...";
                hsMenuControls[nCount - 1].Tag = "hs_MenuItem24" + ":" + Guid.NewGuid().ToString();
                hsMenuControls[nCount - 1].Picture = getImage();
                hsMenuControls[nCount - 1].BeginGroup = true;
                hsMenuControls[nCount - 1].Click +=
                    new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler(MenuControl_Click);
                nCount++;

                hsAutoCorrectPopup = (Office.CommandBarPopup)commandBar.Controls.Add(
                    Office.MsoControlType.msoControlPopup, missing, missing, missing, true);
                hsAutoCorrectPopup.Caption = "A&utoCorrect (Ինքնաշտկել)";
                hsAutoCorrectPopup.Tag = "hs_MenuItem25" + ":" + Guid.NewGuid().ToString();
                //hsAutoCorrectPopup.BeginGroup = true;
                nCount++;
                int nSMCount = 1;
                for (int i = 0; i < Math.Min(20, arrSuggests.Count); i++)
                {
                    hsSubMenuControls[i] = (Office.CommandBarButton)hsAutoCorrectPopup.Controls.Add(
                    Office.MsoControlType.msoControlButton, missing, missing, missing, true);
                    hsSubMenuControls[i].Style = Office.MsoButtonStyle.msoButtonCaption;
                    hsSubMenuControls[i].Caption = arrSuggests[i].ToString();
                    hsSubMenuControls[i].Tag = "hs_SubMenuItem" + nSMCount.ToString() + ":" + Guid.NewGuid().ToString();
                    hsSubMenuControls[i].Click +=
                        new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler(AutoCorrectMenuControl_Click); 
                    nSMCount++;
                }
                hsSubMenuControls[nSMCount - 1] = (Office.CommandBarButton)hsAutoCorrectPopup.Controls.Add(
                    Office.MsoControlType.msoControlButton, 793, missing, missing, true);
                hsSubMenuControls[nSMCount - 1].Tag = "hs_SubMenuItem" + nSMCount.ToString() + ":" + Guid.NewGuid().ToString();
                hsSubMenuControls[nSMCount - 1].BeginGroup = true;

                //hsMenuControls[nCount - 1] = (Office.CommandBarButton)commandBar.Controls.Add(
                //    Office.MsoControlType.msoControlButton, missing, missing, missing, true);
                //hsMenuControls[nCount - 1].Style = Office.MsoButtonStyle.msoButtonIconAndCaption;
                //hsMenuControls[nCount - 1].Caption = "Loo&k Up (Բառարան)...";
                //hsMenuControls[nCount - 1].Tag = "hs_MenuItem24" + ":" + Guid.NewGuid().ToString();
                //hsMenuControls[nCount - 1].Picture = getImage();
                //hsMenuControls[nCount - 1].Click +=
                //    new Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler(MenuControl_Click);
                //nCount++;

                hsMenuControls[nCount - 1] = (Office.CommandBarButton)commandBar.Controls.Add(
                    Office.MsoControlType.msoControlButton, 21, missing, missing, true);
                hsMenuControls[nCount - 1].Tag = "hs_MenuItem26" + ":" + Guid.NewGuid().ToString();
                hsMenuControls[nCount - 1].BeginGroup = true;
                nCount++;
                hsMenuControls[nCount - 1] = (Office.CommandBarButton)commandBar.Controls.Add(
                    Office.MsoControlType.msoControlButton, 19, missing, missing, true);
                hsMenuControls[nCount - 1].Tag = "hs_MenuItem27" + ":" + Guid.NewGuid().ToString();
                nCount++;
                hsMenuControls[nCount - 1] = (Office.CommandBarButton)commandBar.Controls.Add(
                    Office.MsoControlType.msoControlButton, 22, missing, missing, true);
                hsMenuControls[nCount - 1].Tag = "hs_MenuItem28" + ":" + Guid.NewGuid().ToString();
                nCount++;

                commandBar.ShowPopup(missing, missing);
            }
        }

        private stdole.IPictureDisp getImage()
        {
            stdole.IPictureDisp tempImage = null;
            try
            {
                System.Drawing.Bitmap newIcon = Properties.Resources.HySpellLookupIcon;

                ImageList newImageList = new ImageList();
                newImageList.Images.Add(newIcon);
                tempImage = ConvertImage.Convert(newImageList.Images[0]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return tempImage;
        }
        sealed public class ConvertImage : System.Windows.Forms.AxHost
        {
            private ConvertImage() : base(null)
            {
            }
            public static stdole.IPictureDisp Convert
                (System.Drawing.Image image)
            {
                return (stdole.IPictureDisp)System.
                    Windows.Forms.AxHost.GetIPictureDispFromPicture(image);
            }
        }

    }

}
