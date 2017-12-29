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
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Reflection;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using System.Globalization;
using NHunspell;
using Word = Microsoft.Office.Interop.Word;

namespace HySpellOL
{
    public partial class ThisAddIn
    {
        private string sTaskPaneName = "HySpell";
        private HySpellSettings m_oSettings;
        private int m_nLangId;
        private bool bInitSpellCheckCall = true;


        Dictionary<Guid, InspectorWrapper> _wrappedInspectors;
        Outlook.Inspectors m_Inspectors = null;
        Outlook.Inspector m_Inspector = null;
        Word.Document m_Document = null;

        private HySpellEncoder.Wrapper m_oEncoder = null;
        private Hunspell m_oHunspell = null;

        //private Word.Paragraphs oPars = null;
        private Word.Range SelWordRange = null;

        private HSTaskPane oHSTaskPaneControl;
        private Microsoft.Office.Tools.CustomTaskPane myCustomTaskPane;

        private int nSelWordEncoding = 2; // default is unicode
        private int nCurrentWordStart = 0;
        private int nCurrentWordEnd = 0;

        string sSCIIOutput = "";
        string sUniInput = "";
        string sUserSuggWord = "";
        ArrayList arrSuggestList = null;
        ArrayList arrPossibleWords = null;
        //bool bTerminateOrthoConvert = false;

        //bool bHasPrevTextBoxLink = false;
        //int nHit = 0;
        //int nParIndex = 1;
        int nWordLen = 1;
        int nRetFlag = -1;
        int j = 0;

        private string sCustomDictPath = "";

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
        //public bool TerminateOrthoConvert
        //{
        //    get { return bTerminateOrthoConvert; }
        //    set { bTerminateOrthoConvert = value; }
        //}
        public Word.Range CurrentSelectedRange
        {
            get { return SelWordRange; }
            set { SelWordRange = value; }
        }

        public bool ByPassAppWinSelectChange = false;

        public void InitializeWrapper()
        {
            InitializeWrapper(true);
        }
        public void InitializeWrapper(bool bFromFile)
        {
            string sPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //sPath += "\\հարօ";
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

//// անհասկանելի էր այս տողերի նպատակը, փոխեցի, պէտք է փորձարկել հեռացման ազդեցութիւնը
//            while (rgWhole.Start > 0 && !rgWhole.Text.StartsWith("\r") && !rgWhole.Text.StartsWith(" ") && !rgWhole.Text.StartsWith("\t"))
//            {
//                int nOldStartPos = rgWhole.Start;
//                rgWhole.Start = rgWhole.Words.First.Start - 1;
//                // break loop if trapped inside infinite loop
//                if (rgWhole.Start >= nOldStartPos)
//                    break;
//            }
//            rgWhole.End = rgWhole.Words.Last.End;

//            while (rgWhole.Words.Last.Text != null && !rgWhole.Words.Last.Text.EndsWith("\r")
//                    && !rgWhole.Words.Last.Text.EndsWith(" ") && !rgWhole.Words.Last.Text.EndsWith("\t"))
//            {
//                int nOldEndPos = rgWhole.End;
//                rgWhole.End = rgWhole.Words.Last.End + 1;
//                // break loop if trapped inside infinite loop
//                if (rgWhole.End <= nOldEndPos)
//                    break;
//            }
//            rgWhole.End = rgWhole.Words.Last.End;

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

        public void ShowTaskPane(string sWord)
        {
            bool bTaskPaneIsOpen = false;

            m_oSettings = new HySpellSettings();
            m_Inspector = Globals.ThisAddIn.Application.ActiveInspector();

            ////var wrapper = InspectorWrapper.GetWrapperFor(m_Inspector);
            ////if (wrapper.Document == null)
            ////{
            ////    wrapper.Document = m_Inspector.WordEditor;
            ////    if (!m_Inspector.IsWordMail())
            ////        wrapper.Document = m_Inspector.CurrentItem as Word.Document;
            ////}
            ////m_nLangId = (int)wrapper.Document.Application.Language;


            m_Document = m_Inspector.WordEditor;
            if (!m_Inspector.IsWordMail())
                m_Document = m_Inspector.CurrentItem as Word.Document;
            m_nLangId = (int)m_Document.Application.Language;

            foreach (Microsoft.Office.Tools.CustomTaskPane cPane in this.CustomTaskPanes)
            {
                try
                {
                    var cpWindow = cPane.Window;
                    if (cPane.Title == sTaskPaneName && cpWindow == m_Inspector.Application.ActiveWindow())
                    {
                        cPane.Visible = true;
                        bTaskPaneIsOpen = true;
                    }
                }
                catch (Exception e)
                {
                }
            }

            if (!bTaskPaneIsOpen)
            {
                if (m_oHunspell == null)
                    InitializeWrapper();
                oHSTaskPaneControl = new HSTaskPane(m_oHunspell);
                myCustomTaskPane = this.CustomTaskPanes.Add(oHSTaskPaneControl, sTaskPaneName, m_Inspector.Application.ActiveWindow());
                myCustomTaskPane.Visible = true;
                (myCustomTaskPane.Control.Controls[0] as TabControl).TabPages.RemoveAt(1);
            }
            (myCustomTaskPane.Control.Controls[0] as TabControl).SelectTab(0);
            if (sWord != null && sWord.Length > 0)
            {
                foreach (Microsoft.Office.Tools.CustomTaskPane cPane in this.CustomTaskPanes)
                {
                    var cpWindow = cPane.Window;
                    if (cPane.Title == sTaskPaneName && cpWindow == m_Inspector.Application.ActiveWindow())
                    {
                        (cPane.Control as HSTaskPane).FindMeaning(sWord, true);
                        break;
                    }
                }
            }

        }

        public void LookUpWord(int nTagNum, string sSuggest = "")
        {
            if (nTagNum < 20)
            {
                // correct the current misspelled word with the selected suggest word
                Object oRS = nCurrentWordStart;
                Object oRE = nCurrentWordEnd;
                Word.Range rgCurrWord = m_Document.Application.ActiveDocument.Range(ref oRS, ref oRE);
                // check case for non-unicode text
                if (nSelWordEncoding < 2)
                    sSuggest = EncodeWithAccents(sSuggest);
                rgCurrWord.Text = sSuggest;
                rgCurrWord.Start = rgCurrWord.End;
                rgCurrWord.Select(); 
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
                            Word.Range rgCurrWord = m_Document.Application.ActiveDocument.Range(ref oRS, ref oRE);
                            ArrayList arrEncoded = new ArrayList();
                            int nRetFlag = m_oEncoder.Encode(rgCurrWord.Text, arrEncoded);
                            if (arrEncoded.Count != 0)
                                m_oHunspell.Add(arrEncoded[0].ToString());
                        }
                        break;
                    case 23: // Add to Dictionary
                        if (m_oEncoder != null && m_oHunspell != null)
                        {
                            frmAddNewWord dlg = new frmAddNewWord();
                            if (Globals.ThisAddIn.ProgramSettings.OrthographyType == 1)
                                dlg.IsClassicOrthography = false;
                            Object oRS = nCurrentWordStart;
                            Object oRE = nCurrentWordEnd;
                            Word.Range rgCurrWord = m_Document.Application.ActiveDocument.Range(ref oRS, ref oRE);
                            dlg.HSWrapper = m_oHunspell;
                            dlg.HSEncoder = m_oEncoder;
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
                            Word.Range rgCurrWord = m_Document.Application.ActiveDocument.Range(ref oRS, ref oRE);
                            string sWord = rgCurrWord.Text;
                            ShowTaskPane(sWord);
                        }
                        break;
                    default:
                        break;
                }
            }
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

        public void StartSpellingViaTaskPane()
        {
            bool bTaskPaneIsOpen = false;

            if (m_Inspector == null)
            {
                m_Inspector = Globals.ThisAddIn.Application.ActiveInspector();
                m_Document = m_Inspector.WordEditor;
                if (!m_Inspector.IsWordMail())
                    m_Document = m_Inspector.CurrentItem as Word.Document;
                m_nLangId = (int)m_Document.Application.Language;
            }

            foreach (Microsoft.Office.Tools.CustomTaskPane cPane in this.CustomTaskPanes)
            {
                try
                {
                    var cpWindow = cPane.Window;
                    if (cPane.Title == sTaskPaneName && cpWindow == m_Inspector.Application.ActiveWindow())
                    {
                        cPane.Visible = true;
                        bTaskPaneIsOpen = true;
                    }
                }
                catch (Exception e)
                {
                }
            }

            if (!bTaskPaneIsOpen)
            {
                if (m_oHunspell == null)
                    InitializeWrapper();
                oHSTaskPaneControl = new HSTaskPane(m_oHunspell);
                myCustomTaskPane = this.CustomTaskPanes.Add(oHSTaskPaneControl, sTaskPaneName, m_Inspector.Application.ActiveWindow());
                myCustomTaskPane.Visible = true;
            }

            foreach (Microsoft.Office.Tools.CustomTaskPane cPane in this.CustomTaskPanes)
            {
                var cpWindow = cPane.Window;
                if (cPane.Title == sTaskPaneName && cpWindow == m_Inspector.Application.ActiveWindow())
                {
                    (cPane.Control as HSTaskPane).StartSpellChecking();
                    break;
                }
            }
        }
       
        public void RemoveTaskPanes()
        {
            if (m_Document.Application.Documents.Count > 0)
            {
                for (int i = this.CustomTaskPanes.Count; i > 0; i--)
                {
                    Microsoft.Office.Tools.CustomTaskPane cPane = this.CustomTaskPanes[i - 1];
                    if (cPane.Title == sTaskPaneName)
                        this.CustomTaskPanes.Remove(cPane);
                }
            }
        }
        private Microsoft.Office.Core.IRibbonExtensibility hsRibbon;
        protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
            hsRibbon = new HySpellRibbon();
            return hsRibbon;
        }

        public ArrayList ContextSuggestList { get; set; }

        void Application_WindowBeforeRightClick(Microsoft.Office.Interop.Word.Selection Sel, ref bool Cancel)
        {
            if (ContextSuggestList != null)
            {
                ContextSuggestList.Clear();
                ContextSuggestList = null;
            }

            // only if the selected word is armenian make the context menu changes, otherwise do not show HySpell menus
            string sWord = Sel.Words.First.Text;
            int nIndex = -1;
            if (sWord != null)
                nIndex = sWord.IndexOfAny(Globals.ThisAddIn.IsArmenianCharOrAccent);
            if (nIndex != -1 && m_oHunspell == null)
                InitializeWrapper();

            if (m_oEncoder == null)
                m_oEncoder = new HySpellEncoder.Wrapper();

            ArrayList arrEncoded = new ArrayList();
            string sApostrophe = "";
            string sAccent = "";
            Word.Range rgWord = null;
            if (/*nIndex != -1 &&*/ IsWordArmenian(Sel, ref arrEncoded, ref sApostrophe, ref sAccent, ref rgWord))
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
                    ContextSuggestList = arrSuggests;
                }
                else
                {
                    Sel.Words.First.SpellingChecked = true;

                    nCurrentWordStart = rgWord.Start;
                    nCurrentWordEnd = rgWord.End;
                }
            }
            else
            {
                if (rgWord != null)
                {
                    nCurrentWordStart = rgWord.Start;
                    nCurrentWordEnd = rgWord.End;
                }
            }
        }

        public string BuildContextMenu(ArrayList arrSuggests = null)
        {
            StringBuilder contextMenuXML = new StringBuilder(@"<menu xmlns=""http://schemas.microsoft.com/office/2006/01/customui"" >");
            if (arrSuggests != null && arrSuggests.Count > 0)
            {

                for (int i = 0; i < arrSuggests.Count; i++)
                {
                    string tag = arrSuggests[i].ToString();
                    contextMenuXML.Append(@"<button id=""hs_MenuItem" + i.ToString() + @""" label=""" + tag + @""" tag=""" + tag + @""" onAction=""OnDynamicMenuAction"" />");
                }
                contextMenuXML.Append(@"<menuSeparator id=""menusep1"" />");
                contextMenuXML.Append(@"<button id=""hs_MenuItem21"" label=""Ignore (Անտեսել)"" onAction=""OnDynamicMenuAction"" />");
                contextMenuXML.Append(@"<button id=""hs_MenuItem22"" label=""Ignore All (Անտեսել Բոլորը)"" onAction=""OnDynamicMenuAction"" />");
                contextMenuXML.Append(@"<button id=""hs_MenuItem23"" label=""Add to Dictionary (Աւելցնել Բառարան)..."" onAction=""OnDynamicMenuAction"" />");
                contextMenuXML.Append(@"<menuSeparator id=""menusep2"" />");
            }
            else
                contextMenuXML.Append(@"<button id=""hs_MenuItem50"" label=""Spelling Correct (Ճշգրիտ)"" enabled=""false"" />");            
            contextMenuXML.Append(@"</menu>");

            return contextMenuXML.ToString();
        }

        void AutoCorrectMenuControl_Click(Microsoft.Office.Core.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            // add entries to the autocorrect list
            string sAutoCorrectWord = Ctrl.Caption;
            Object oRS = nCurrentWordStart;
            Object oRE = nCurrentWordEnd;
            Word.Range rgCurrWord = m_Document.Application.ActiveDocument.Range(ref oRS, ref oRE);
            m_Document.Application.AutoCorrect.Entries.Add(rgCurrWord.Text, sAutoCorrectWord);

            CancelDefault = true;
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
            private ConvertImage()
                : base(null)
            {
            }
            public static stdole.IPictureDisp Convert
                (System.Drawing.Image image)
            {
                return (stdole.IPictureDisp)System.
                    Windows.Forms.AxHost.GetIPictureDispFromPicture(image);
            }
        }

        void Application_WindowSelectionChange(Microsoft.Office.Interop.Word.Selection Sel)
        {
            bool bTaskPaneIsVisible = false;
            HSTaskPane cCurrentPane = null;
            foreach (Microsoft.Office.Tools.CustomTaskPane cPane in this.CustomTaskPanes)
            {
                try
                {
                    var cpWindow = cPane.Window;
                    if (cPane.Title == sTaskPaneName && cpWindow == m_Inspector.Application.ActiveWindow() && cPane.Visible)
                    {
                        bTaskPaneIsVisible = true;
                        cCurrentPane = (cPane.Control as HSTaskPane);
                    }
                }
                catch (Exception e)
                {
                }
            }

            if (!bTaskPaneIsVisible) return;
            
            if (cCurrentPane.Controls["tabControl"].Controls["tabPage1"] != null && !bInitSpellCheckCall && !ByPassAppWinSelectChange)
                (myCustomTaskPane.Control as HSTaskPane).SetSpellingState("Ստուգել", false);
            bInitSpellCheckCall = false;
            ByPassAppWinSelectChange = false;
        }

        // Wrap an Inspector, if required, and store it in memory to get events of the wrapped Inspector.
        // <param name="inspector">The Outlook Inspector instance.</param>

        private void WrapInspector(Outlook.Inspector inspector)
        {
            m_Inspector = inspector;
            var wrapper = InspectorWrapper.GetWrapperFor(inspector);
            if (wrapper != null)
            {
                // Register the Closed event.
                wrapper.Closed += WrapperClosed;
                wrapper.Activated += WrapperActivated;

                // Remember the inspector in memory.
                _wrappedInspectors[wrapper.Id] = wrapper;
            }
        }

        // Method is called when an inspector has been closed.
        // Removes reference from memory.
        // <param name="id">The unique id of the closed inspector</param>
        private void WrapperClosed(Guid id)
        {
            if (m_Document != null)
            {
                m_Document.Application.WindowBeforeRightClick -= Application_WindowBeforeRightClick;
                m_Document.Application.WindowSelectionChange -= Application_WindowSelectionChange;
            }
            _wrappedInspectors.Remove(id);
        }
        private void WrapperActivated(Guid id)
        {
            if (m_Inspector.CurrentItem is Outlook.MailItem
                    || m_Inspector.CurrentItem is Outlook.AppointmentItem
                    || m_Inspector.CurrentItem is Outlook.PostItem
                    || m_Inspector.CurrentItem is Outlook.TaskItem
                    || m_Inspector.CurrentItem is Outlook.ContactItem
                    || m_Inspector.CurrentItem is Outlook.JournalItem)
            {
                m_Document = m_Inspector.WordEditor;
                if (!m_Inspector.IsWordMail())
                    m_Document = m_Inspector.CurrentItem as Word.Document;
                if (m_Document != null)
                {
                    m_nLangId = (int)m_Document.Application.Language;
                    m_Document.Application.WindowBeforeRightClick -= Application_WindowBeforeRightClick;
                    m_Document.Application.WindowSelectionChange -= Application_WindowSelectionChange;
                    m_Document.Application.WindowBeforeRightClick += new Microsoft.Office.Interop.Word.ApplicationEvents4_WindowBeforeRightClickEventHandler(Application_WindowBeforeRightClick);
                    m_Document.Application.WindowSelectionChange += new Microsoft.Office.Interop.Word.ApplicationEvents4_WindowSelectionChangeEventHandler(Application_WindowSelectionChange);
                }
            }

        }

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            _wrappedInspectors = new Dictionary<Guid, InspectorWrapper>();
            m_Inspectors = Globals.ThisAddIn.Application.Inspectors;
            m_Inspectors.NewInspector += WrapInspector;

            foreach (Outlook.Inspector inspector in m_Inspectors)
            {
                WrapInspector(inspector);
            }
        }
        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            UnInitializeWrapper();
            // remove all task-panes
            RemoveTaskPanes();

            _wrappedInspectors.Clear();
            m_Inspectors.NewInspector -= WrapInspector;
            m_Inspectors = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
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
    }

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

#region InspectorWrapper code
    // Eventhandler used to correctly clean up resources.
    // <param name="id">The unique id of the Inspector instance.</param>
    internal delegate void InspectorWrapperClosedEventHandler(Guid id);
    internal delegate void InspectorWrapperActivatedEventHandler(Guid id);

    // The base class for all inspector wrappers.
    internal abstract class InspectorWrapper
    {
        public Word.Document Document = null;
        public Microsoft.Office.Tools.CustomTaskPane TaskPane = null;

        // The unique ID that identifies an inspector window.
        protected InspectorWrapper(Outlook.Inspector inspector)
        {
            Id = Guid.NewGuid();
            Inspector = inspector;
            // Register Inspector events here
            ((Outlook.InspectorEvents_10_Event)Inspector).Close += InspectorClose;
            ((Outlook.InspectorEvents_10_Event)Inspector).Activate += Activate;
            (Inspector).Deactivate += Deactivate;
            (Inspector).BeforeMaximize += BeforeMaximize;
            (Inspector).BeforeMinimize += BeforeMinimize;
            (Inspector).BeforeMove += BeforeMove;
            (Inspector).BeforeSize += BeforeSize;
            (Inspector).PageChange += PageChange;

            // Initialize is called to give the derived wrappers.
            Initialize();
        }

        public Guid Id { get; private set; }

        // The Outlook Inspector instance.
        public Outlook.Inspector Inspector { get; private set; }
        public event InspectorWrapperClosedEventHandler Closed;
        public event InspectorWrapperActivatedEventHandler Activated;

        // Event handler for the Inspector Close event.
        private void InspectorClose()
        {
            // Call the Close Method - the derived classes can implement cleanup code
            // by overriding the Close method.
            Close();

            // Unregister Inspector events.
            ((Outlook.InspectorEvents_10_Event)Inspector).Close -= InspectorClose;

            // Clean up resources and do a GC.Collect().
            Inspector = null;
            Document = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Raise the Close event.
            if (Closed != null)
            {
                Closed(Id);
            }
        }

        ////// Method is called when the inspector is activated.
        ////protected virtual void Activate() { }
        private void Activate()
        {
            //Activate();
            if (Activated != null)
            {
                Activated(Id);
            }
        }

        protected virtual void Initialize() { }

        // Method is called when another page of the inspector has been selected.
        // <param name="ActivePageName">The active page name by reference.</param>
        protected virtual void PageChange(ref string ActivePageName) { }

        // Method is called before the inspector is resized.
        // <param name="Cancel">To prevent resizing, set Cancel to true.</param>
        protected virtual void BeforeSize(ref bool Cancel) { }

        // Method is called before the inspector is moved around.
        // <param name="Cancel">To prevent moving, set Cancel to true.</param>
        protected virtual void BeforeMove(ref bool Cancel) { }

        // Method is called before the inspector is minimized.
        // <param name="Cancel">To prevent minimizing, set Cancel to true.</param>
        protected virtual void BeforeMinimize(ref bool Cancel) { }

        // Method is called before the inspector is maximized.
        // <param name="Cancel">To prevent maximizing, set Cancel to true.</param>
        protected virtual void BeforeMaximize(ref bool Cancel) { }

        // Method is called when the inspector is deactivated.
        protected virtual void Deactivate() { }

        // Derived classes can do a cleanup by overriding this method.
        protected virtual void Close() { }

        // This factory method returns a specific InspectorWrapper or null if not handled.
        // <param name=”inspector”>The Outlook Inspector instance.</param>
        // Returns the specific wrapper or null.
        public static InspectorWrapper GetWrapperFor(Outlook.Inspector inspector)
        {
            // Retrieve the message class by using late binding.
            string messageClass = inspector.CurrentItem.GetType().InvokeMember("MessageClass", BindingFlags.GetProperty,
                                                                               null, inspector.CurrentItem, null);
            switch (messageClass)
            {
                case "IPM.Contact":
                    return new ContactItemWrapper(inspector); 
                case "IPM.Journal":
                    return new MailItemWrapper(inspector);
                case "IPM.Activity":
                    return new JournalItemWrapper(inspector); 
                case "IPM.Appointment":
                    return new AppointmentItemWrapper(inspector); 
                case "IPM.Note":
                    return new MailItemWrapper(inspector);
                case "IPM.Post":
                    return new PostItemWrapper(inspector); 
                case "IPM.Task":
                    return new TaskItemWrapper(inspector); 
            }

            return null;
        }
    }

    internal class MailItemWrapper : InspectorWrapper
    {
        public MailItemWrapper(Outlook.Inspector inspector) : base(inspector)
        {
            Item = (Outlook.MailItem)Inspector.CurrentItem;
            
            // register Item events.
            Item.Open += Item_Open;
            Item.Write += Item_Write;
        }
        
        public Outlook.MailItem Item { get; private set; }

        // This method is called when the item is visible and the UI is initialized.
        // <param name="Cancel">When you set this property to true, the Inspector is closed.</param>
        private void Item_Open(ref bool Cancel)
        {
            //TODO: Implement something
        }

        // This method is called when the item is saved.
        // <param name="Cancel">When set to true, the save operation is cancelled.</param>
        private void Item_Write(ref bool Cancel)
        {
            //TODO: Implement something 
        }

        // The Close method is called when the inspector has been closed.
        // Do your cleanup tasks here.
        // The UI is gone, cannot access it here.
        protected override void Close()
        {
            // Unregister events.
            Item.Write -= Item_Write;
            Item.Open -= Item_Open;

            // Release references to COM objects.
            Item = null;

            // Set item to null to keep a reference in memory of the garbage collector.
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
    internal class JournalItemWrapper : InspectorWrapper
    {
        public JournalItemWrapper(Outlook.Inspector inspector)
            : base(inspector)
        {
            Item = (Outlook.JournalItem)Inspector.CurrentItem;

            // register Item events.
            Item.Open += Item_Open;
            Item.Write += Item_Write;
        }

        public Outlook.JournalItem Item { get; private set; }

        // This method is called when the item is visible and the UI is initialized.
        // <param name="Cancel">When you set this property to true, the Inspector is closed.</param>
        private void Item_Open(ref bool Cancel)
        {
            //TODO: Implement something
        }

        // This method is called when the item is saved.
        // <param name="Cancel">When set to true, the save operation is cancelled.</param>
        private void Item_Write(ref bool Cancel)
        {
            //TODO: Implement something 
        }

        // The Close method is called when the inspector has been closed.
        // Do your cleanup tasks here.
        // The UI is gone, cannot access it here.
        protected override void Close()
        {
            // Unregister events.
            Item.Write -= Item_Write;
            Item.Open -= Item_Open;

            // Release references to COM objects.
            Item = null;

            // Set item to null to keep a reference in memory of the garbage collector.
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }

    internal class PostItemWrapper : InspectorWrapper
    {
        public PostItemWrapper(Outlook.Inspector inspector)
            : base(inspector)
        {
            Item = (Outlook.PostItem)Inspector.CurrentItem;

            // register Item events.
            Item.Open += Item_Open;
            Item.Write += Item_Write;
        }

        public Outlook.PostItem Item { get; private set; }

        private void Item_Open(ref bool Cancel)
        {
            //TODO: Implement something
        }

        private void Item_Write(ref bool Cancel)
        {
            //TODO: Implement something 
        }

        protected override void Close()
        {
            // Unregister events.
            Item.Write -= Item_Write;
            Item.Open -= Item_Open;

            // Release references to COM objects.
            Item = null;

            // Set item to null to keep a reference in memory of the garbage collector.
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
    internal class AppointmentItemWrapper : InspectorWrapper
    {
        public AppointmentItemWrapper(Outlook.Inspector inspector)
            : base(inspector)
        {
            Item = (Outlook.AppointmentItem)Inspector.CurrentItem;

            // register Item events.
            Item.Open += Item_Open;
            Item.Write += Item_Write;
        }

        public Outlook.AppointmentItem Item { get; private set; }

        private void Item_Open(ref bool Cancel)
        {
            //TODO: Implement something
        }

        private void Item_Write(ref bool Cancel)
        {
            //TODO: Implement something 
        }

        protected override void Close()
        {
            // Unregister events.
            Item.Write -= Item_Write;
            Item.Open -= Item_Open;

            // Release references to COM objects.
            Item = null;

            // Set item to null to keep a reference in memory of the garbage collector.
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
    internal class ContactItemWrapper : InspectorWrapper
    {
        public ContactItemWrapper(Outlook.Inspector inspector)
            : base(inspector)
        {
            Item = (Outlook.ContactItem)Inspector.CurrentItem;

            // register Item events.
            Item.Open += Item_Open;
            Item.Write += Item_Write;
        }

        public Outlook.ContactItem Item { get; private set; }

        private void Item_Open(ref bool Cancel)
        {
            //TODO: Implement something
        }

        private void Item_Write(ref bool Cancel)
        {
            //TODO: Implement something 
        }

        protected override void Close()
        {
            // Unregister events.
            Item.Write -= Item_Write;
            Item.Open -= Item_Open;

            // Release references to COM objects.
            Item = null;

            // Set item to null to keep a reference in memory of the garbage collector.
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
    internal class TaskItemWrapper : InspectorWrapper
    {
        public TaskItemWrapper(Outlook.Inspector inspector)
            : base(inspector)
        {
            Item = (Outlook.TaskItem)Inspector.CurrentItem;

            // register Item events.
            Item.Open += Item_Open;
            Item.Write += Item_Write;
        }

        public Outlook.TaskItem Item { get; private set; }

        private void Item_Open(ref bool Cancel)
        {
            //TODO: Implement something
        }

        private void Item_Write(ref bool Cancel)
        {
            //TODO: Implement something 
        }

        protected override void Close()
        {
            // Unregister events.
            Item.Write -= Item_Write;
            Item.Open -= Item_Open;

            // Release references to COM objects.
            Item = null;

            // Set item to null to keep a reference in memory of the garbage collector.
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
#endregion


}
