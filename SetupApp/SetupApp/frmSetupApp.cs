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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Net;
using Ionic.Zip;

namespace SetupApp
{
    public partial class frmSetupApp : Form
    {
        private bool bUpdateProgramsList = true;
        private delegate void InstalledProgramsDelegate(List<InstalledProgram> Results);

        private struct Args
        {
            public string ComputerName;
            public bool IncludeUpdates;
        }
        // check on a background thread 
        private void BG_GetPrograms(object Arguments)
        {
            Args Arg = (Args)Arguments;
            List<InstalledProgram> InstalledPrograms = InstalledProgram.GetInstalledPrograms(Arg.ComputerName, Arg.IncludeUpdates);
            this.Invoke(new InstalledProgramsDelegate(GetProgramsFinished), InstalledPrograms);
        }
        private void GetProgramsFinished(List<InstalledProgram> InstalledPrograms)
        {
            if ((InstalledPrograms != null))
            {
                for (int i = 0; i <= InstalledPrograms.Count - 1; i++)
                {
                    ListViewItem listView = new ListViewItem(InstalledPrograms[i].DisplayName);
                    listView.SubItems.Add(new ListViewItem.ListViewSubItem(listView, InstalledPrograms[i].Version));
                    listView.SubItems.Add(new ListViewItem.ListViewSubItem(listView, InstalledPrograms[i].Publisher));
                    if (InstalledPrograms[i].Publisher.Contains("HySpell") 
                        || InstalledPrograms[i].Publisher.Contains("Epsilon-Logic Systems")
                        || InstalledPrograms[i].DisplayName.StartsWith("Microsoft Office"))
                        lstPrograms.Items.Add(listView);
                }                
            }
            //this.Enabled = true;
            prgStatus.Visible = false;
            if (lstPrograms.Items.Count == 0)
                MessageBox.Show("No file found!", "HySpell Setup and Diagnostics", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                SetupInstallControls();
        }

        public void SerializeAppInfoCollection(string sPath)
        {
            ApplicationInfoCollection refAppInfos = new ApplicationInfoCollection();

            CreateDefaultAppInfoCollection(ref refAppInfos);
            XmlSerializer xmlSer = new XmlSerializer(typeof(ApplicationInfoCollection));
            TextWriter writer = new StreamWriter(sPath);
            xmlSer.Serialize(writer, refAppInfos);
            writer.Close();
        }
        public void CreateDefaultAppInfoCollection(ref ApplicationInfoCollection refAppInfos)
        {
            refAppInfos.CollectionName = "ApplicationInfoCollection";

            ApplicationInfo app0 = new ApplicationInfo("FILES", "HySpell 3.0 Armenian Spellchecker for Office", "HySpell", "3.0.0");
            refAppInfos.Add(app0);
            ApplicationInfo app1 = new ApplicationInfo("SPELLCHECKER", "HySpell 3.0", "HySpell", "3.0.0.0");
            refAppInfos.Add(app1);
        }
        public void DeSerializeAppInfoCollection(ref ApplicationInfoCollection oRefAppInfos, string sPath)
        {
            if (!File.Exists(sPath))
                SerializeAppInfoCollection(sPath);
            XmlSerializer deserializer = new XmlSerializer(typeof(ApplicationInfoCollection));
            TextReader textReader = null;
            try
            {
                textReader = new StreamReader(sPath);
                oRefAppInfos = (ApplicationInfoCollection)deserializer.Deserialize(textReader);
                textReader.Close();
            }
            catch
            {
                CreateDefaultAppInfoCollection(ref oRefAppInfos);
            }
        }

        private void CheckForInstalledPrograms()
        {
            //this.Enabled = false;
            btnInstalFilesDicts.Enabled = Enabled;
            lblStep1.ForeColor = Color.Red;
            lblStep1.Text = "(REQUIRED)";
            btnInstallSpellChecker.Enabled = Enabled;
            lblStep2.ForeColor = Color.Red;
            lblStep2.Text = "(REQUIRED)";
            prgStatus.Style = ProgressBarStyle.Marquee;
            lstPrograms.Items.Clear();
            Args Arg = new Args();
            Arg.ComputerName = "";
            Arg.IncludeUpdates = false;
            //Start the enumeration process on a background thread
            System.Threading.Thread bgthread = new System.Threading.Thread(BG_GetPrograms);
            bgthread.IsBackground = true;
            bgthread.Start(Arg);
        }
        public frmSetupApp()
        {
            InitializeComponent();
        }

        private void btnInstalFilesDicts_Click(object sender, EventArgs e)
        {
            bUpdateProgramsList = false;
            int nIndex = Application.ExecutablePath.LastIndexOf('\\');
            string sCmdText = Application.ExecutablePath.Substring(0, nIndex) + "\\RequiredFiles\\setup.exe";
            try
            {
                btnInstalFilesDicts.Enabled = false;
                prgStatus.Visible = true;
                prgStatus.Style = ProgressBarStyle.Continuous;
                prgStatus.Value = 0;
                prgStatus.Maximum = 1000;
                prgStatus.Increment(500);
                using (System.Diagnostics.Process proc = System.Diagnostics.Process.Start(sCmdText))
                {
                    prgStatus.Increment(700);
                    Cursor.Current = Cursors.WaitCursor;
                    proc.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot find installer file!" + " " + sCmdText + "\nError: " + ex.Message,
                    "HySpell Setup and Diagnostics [Required Files]", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                btnInstalFilesDicts.Enabled = true;
                Cursor.Current = Cursors.Default;
                CheckForInstalledPrograms();
            }
            bUpdateProgramsList = true;
        }

        private void btnInstallSpellChecker_Click(object sender, EventArgs e)
        {
            btnInstallSpellChecker.Enabled = false;
            int nIndex = Application.ExecutablePath.LastIndexOf('\\');
            string sExecRoot = Application.ExecutablePath.Substring(0, nIndex);
            string sCmdText = sExecRoot;

            prgStatus.Visible = true;
            prgStatus.Style = ProgressBarStyle.Continuous;
            prgStatus.Value = 0;
            prgStatus.Maximum = 1000;
            prgStatus.Increment(500);

            if (is64bitOfficeInstallation)
                sCmdText += "\\OfficeCustomization\\x64\\setup.exe";
            else
                sCmdText += "\\OfficeCustomization\\x86\\setup.exe";
            string sCmdOLText = sCmdText.Replace("OfficeCustomization", "OfficeCustomizationOL");

            ExecuteCustomInstallCommand(sCmdText, "Office Word Customization Error: ");

            ExecuteCustomInstallCommand(sCmdOLText, "Office Outlook Customization Error: ");

            btnInstallSpellChecker.Enabled = true;
            Cursor.Current = Cursors.Default;
        }

        private void ExecuteCustomInstallCommand(string cmd, string errorHdr)
        {
            //int exitcode = 0;
            try
            {
                if (File.Exists(cmd))
                {
                    using (System.Diagnostics.Process proc = System.Diagnostics.Process.Start(cmd))
                    {
                        //prgStatus.Increment(700);
                        Cursor.Current = Cursors.WaitCursor;
                        proc.WaitForExit();
                        //exitcode = proc.ExitCode;
                        //CheckForInstalledPrograms();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(errorHdr + ex.Message, "HySpell Setup and Diagnostics [Customization]", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            //return exitcode;
        }

        private void btnInstallKeyboard_Click(object sender, EventArgs e)
        {
            if (lstKeyboarLayouts.SelectedIndex == -1)
                MessageBox.Show("Please select a keyboard layout to install, \nand then click the Install button.", "HySpell Setup and Diagnostics [Keyboard Layouts]", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                bUpdateProgramsList = false;
                string sSelected = lstKeyboarLayouts.SelectedItem.ToString();
                string sAlternateName = "";
                int nIndex = Application.ExecutablePath.LastIndexOf('\\');
                string sExecRoot = Application.ExecutablePath.Substring(0, nIndex);
                string sCmdText = sExecRoot;
                switch (lstKeyboarLayouts.SelectedIndex)
                {
                    case 0:
                        sCmdText += "\\armentwe\\setup.exe";
                        sAlternateName = "Armenian Typewriter";
                        break;
                    case 1:
                        sCmdText += "\\armenphe\\setup.exe";
                        sAlternateName = "Armenian Phonetic";
                        break;
                }

                foreach (ListViewItem item in lstPrograms.Items)
                {
                    if (item.SubItems[0].Text.Contains(sAlternateName) &&
                            (item.SubItems[2].Text.StartsWith("HySpell") || item.SubItems[2].Text.StartsWith("Epsilon-Logic Systems")))
                    {
                        MessageBox.Show(String.Format("Selected keyboard layout\n'{0}'\n is already installed on your system.", sSelected), 
                            "HySpell Setup and Diagnostics [Keyboard Layouts]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        bUpdateProgramsList = true;
                        return;
                    }
                }
                string sMsg = String.Format("Are you sure you want to install\n'{0}'\nkeyboard layout", sSelected);
                DialogResult dlgResult = MessageBox.Show(sMsg, "HySpell Setup and Diagnostics [Keyboard Layouts]", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dlgResult == System.Windows.Forms.DialogResult.Yes)
                {
                    btnInstallKeyboard.Enabled = false;
                    try
                    {
                        prgStatus.Visible = true;
                        prgStatus.Style = ProgressBarStyle.Continuous;
                        prgStatus.Maximum = 1000;
                        prgStatus.Increment(500);
                        using (System.Diagnostics.Process proc = System.Diagnostics.Process.Start(sCmdText))
                        {
                            prgStatus.Increment(700);
                            Cursor.Current = Cursors.WaitCursor;
                            proc.WaitForExit();
                            //if (proc.ExitCode == 0)
                            //{
                            //    CheckForInstalledPrograms();
                            //    bUpdateProgramsList = true;
                            //}
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Cannot find keyboard install file!" + " " + sCmdText + "\nError: " + ex.Message,
                            "HySpell Setup and Diagnostics [Keyboard Layouts]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    finally
                    {
                        btnInstallKeyboard.Enabled = true;
                        Cursor.Current = Cursors.Default;
                        CheckForInstalledPrograms();
                    }
                }
                bUpdateProgramsList = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSetupApp_Load(object sender, EventArgs e)
        {
            //CheckForInstalledPrograms();
        }
        private void SetupInstallControls()
        {
            ApplicationInfoCollection refAppInfos = new ApplicationInfoCollection();
            int nIndex = Application.ExecutablePath.LastIndexOf('\\');
            string sPath = Application.ExecutablePath.Substring(0, nIndex) + "\\Files\\ApplicationVersionInfo.xml";
//HM            
//string sPath = "C:\\Haro\\DBProjects\\HySpell\\SetupApp\\SetupApp\\Files\\ApplicationVersionInfo.xml";

            DeSerializeAppInfoCollection(ref refAppInfos, sPath);
            foreach (ApplicationInfo appInfo in refAppInfos)
            {
                if (appInfo.ProductID == "FILES")
                {
                    foreach (ListViewItem item in lstPrograms.Items)
                    {
                        if (item.SubItems[0].Text.Contains(appInfo.ProductName)
                                && item.SubItems[1].Text == appInfo.Version
                                && item.SubItems[2].Text.StartsWith(appInfo.Publisher))
                        {
                            btnInstalFilesDicts.Enabled = false;
                            lblStep1.ForeColor = SystemColors.GrayText;
                            lblStep1.Text = "(INSTALLED)";
                        }
                    }
                }
                else if (appInfo.ProductID == "SPELLCHECKER")
                {
                    bool bWInstalled = false;
                    bool bOLInstalled = false;
                    foreach (ListViewItem item in lstPrograms.Items)
                    {
                        if (item.SubItems[0].Text == appInfo.ProductName
                                && item.SubItems[1].Text == appInfo.Version
                                && item.SubItems[2].Text.StartsWith(appInfo.Publisher))
                        {
                            chkWInstalled.Checked = true;
                            chkWInstalled.ForeColor = System.Drawing.Color.Green;
                            bWInstalled = true;
                        }
                        if (item.SubItems[0].Text == appInfo.ProductName.Replace("HySpell", "HySpellOL")
                                && item.SubItems[1].Text == appInfo.Version
                                && item.SubItems[2].Text.StartsWith(appInfo.Publisher))
                        {
                            chkOLInstalled.Checked = true;
                            chkOLInstalled.ForeColor = System.Drawing.Color.Green;
                            bOLInstalled = true;
                        }
                    }
                    if (bWInstalled && bOLInstalled)
                    {
                        btnInstallSpellChecker.Enabled = false;
                        lblStep2.ForeColor = SystemColors.GrayText;
                        lblStep2.Text = "(INSTALLED)";
                    }
                }
                //else if (appInfo.ProductID == "SPELLCHECKEROL")
                //{
                //    btnInstallSpellChecker.Enabled = true;
                //    lblStep2.ForeColor = Color.Red;
                //    lblStep2.Text = "(REQUIRED)";
                //    foreach (ListViewItem item in lstPrograms.Items)
                //    {
                //        if (item.SubItems[0].Text == appInfo.ProductName
                //                && item.SubItems[1].Text == appInfo.Version
                //                && item.SubItems[2].Text.StartsWith(appInfo.Publisher))
                //        {
                //            btnInstallSpellChecker.Enabled = false;
                //            lblStep2.ForeColor = SystemColors.GrayText;
                //            lblStep2.Text = "(INSTALLED)";
                //        }
                //    }
                //}
            }
        }

        private void lnkUserGuide_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int nIndex = Application.ExecutablePath.LastIndexOf('\\');

            string sLink = Application.ExecutablePath.Substring(0, nIndex) + "\\Files\\UserGuide.pdf";
            try
            {
                System.Diagnostics.Process.Start(sLink);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot find User Guide document file!" + " " + sLink, "HySpell Setup and Diagnostics [Document]", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool is64bitOfficeInstallation = false;
        private void frmSetupApp_Activated(object sender, EventArgs e)
        {
            if (bUpdateProgramsList)
                CheckForInstalledPrograms();

            String OfficeVersion = (new VersionFinder()).GetOfficeVersion();
            if (OfficeVersion.Contains("64 bit"))
                is64bitOfficeInstallation = true;
        }

        private void btnViewLayout_Click(object sender, EventArgs e)
        {
            KeyboardLayout dlg = new KeyboardLayout();
            dlg.SelectedLayoutIndex = lstKeyboarLayouts.SelectedIndex;
            switch (lstKeyboarLayouts.SelectedIndex)
            {
                case 0:
                    dlg.Text = "Keyboard Layout: Armenian Typewriter Extended";
                    break;
                case 1:
                    dlg.Text = "Keyboard Layout: Armenian Phonetic Extended";
                    break;
            }
            dlg.ShowDialog();
        }

        private void lstKeyboarLayouts_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnViewLayout.Enabled = true;
        }

        private void CompressDictFiles(string path, string filename)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            DirectoryInfo[] dirs = dirInfo.GetDirectories();
            using (ZipFile zip = new ZipFile())
            {
                foreach (DirectoryInfo dir in dirs)
                {
                    FileInfo[] files = dir.GetFiles();
                    foreach(FileInfo file in files)
                    {
                        zip.AddFile(file.FullName, dir.Name);
                    }
                }
                zip.Save(filename);
            }
        }
        private bool DecompressDictFile(string filename, string path)
        {
            try
            {
                using (ZipFile zip = ZipFile.Read(filename))
                {
                    zip.ExtractAll(path, ExtractExistingFileAction.OverwriteSilently);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Failed to decompress dictionary files. [{0}]", ex.Message));
                return false;
            }

            return true;
        }
        private bool DownloadDictFile(string remoteUri, string localPath)
        {
            string fileName = "DictionaryUpdate.zip", myStringWebResource = null;
            try
            {
                WebClient myWebClient = new WebClient();
                myStringWebResource = remoteUri + fileName;
                myWebClient.DownloadFile(myStringWebResource, localPath + fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("Failed to connect to 'hyspell.com/support/' site. [{0}]", ex.Message));
                return false;
            }

            return true;
        }

        private void lnkUpdateRefDicts_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string sPrimaryURI = Properties.Settings.Default.PrimaryReferenceDictionaryURI;
            string sSecondaryURI = Properties.Settings.Default.SecondaryReferenceDictionaryURI;
            ReferenceDictionaries oRefDicts = new ReferenceDictionaries();
            bool bProceed = true;
            if (!ReadDictionaryReferencesFromURI(ref oRefDicts, sPrimaryURI))
            {
                if (!ReadDictionaryReferencesFromURI(ref oRefDicts, sSecondaryURI))
                {
                    MessageBox.Show("Cannot find dictionary reference XML file!", "HySpell Setup and Diagnostics [Dictionaries]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    bProceed = false;
                }
            }
            if (bProceed)
            {
                // open dialog with listing for user selection
                DicionaryReferenceList dlg = new DicionaryReferenceList();
                dlg.RefDicts = oRefDicts;
                DialogResult dlgResult = dlg.ShowDialog();
                if (dlgResult == System.Windows.Forms.DialogResult.OK)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    string uri = sSecondaryURI.Replace("OnlineDicReferences.xml", "");
                    string localPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\HySpell\\";
                    if (DownloadDictFile(uri, localPath))
                        DecompressDictFile(localPath + "DictionaryUpdate.zip", localPath);

                    // progam writes the new XML file to the User Application Data under HySpell folder
                    string sPath = localPath + "OnlineDicReferences.xml";
                    TextWriter writer = null;
                    try
                    {
                        XmlSerializer xmlSer = new XmlSerializer(typeof(ReferenceDictionaries));
                        writer = new StreamWriter(sPath);
                        xmlSer.Serialize(writer, dlg.OutRefDicts);
                        writer.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "HySpell Setup and Diagnostics [Dictionaries]", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    finally
                    {
                        if (writer != null)
                            writer.Close();
                    }

                    Cursor.Current = Cursors.Default;
                }
            }
        }
        private bool ReadDictionaryReferencesFromURI(ref ReferenceDictionaries oRefDicts, string sURI)
        {
            // open list of available Nayiri dictionaries
            WebClient client = new WebClient();
            Stream data = null;
            TextReader reader = null;
            bool nSuccess = true;

            //client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)"); //if using queries
            try
            {
                data = client.OpenRead(sURI);
                reader = new StreamReader(data);
                XmlSerializer deserializer = new XmlSerializer(typeof(ReferenceDictionaries));
                oRefDicts = (ReferenceDictionaries)deserializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                nSuccess = false;
            }
            finally
            {
                if (data != null)
                    data.Close();
                if (reader != null)
                    reader.Close();
            }

            return nSuccess;
        }

        private void lnkHySpell_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.hyspell.com");
        }

    }
    public class ApplicationInfo
    {
        private string _ProductID = string.Empty;
        private string _ProductName = string.Empty;
        private string _Publisher = string.Empty;
        private string _Version = string.Empty;
        public string ProductID
        {
            get { return _ProductID; }
            set { _ProductID = value; }
        }
        public string ProductName
        {
            get { return _ProductName; }
            set { _ProductName = value; }
        }
        public string Publisher
        {
            get { return _Publisher; }
            set { _Publisher = value; }
        }
        public string Version
        {
            get { return _Version; }
            set { _Version = value; }
        }
        public ApplicationInfo() { }
        public ApplicationInfo(string sProductID, string sProductName, string sPublisher, string sVersion)
        {
            _ProductID = sProductID;
            _ProductName = sProductName;
            _Publisher = sPublisher;
            _Version = sVersion;
        }
    }
    public class ApplicationInfoCollection : ICollection
    {
        public string CollectionName;
        private ArrayList rAppInfoArray = new ArrayList();

        public ApplicationInfo this[int index]
        {
            get { return (ApplicationInfo)rAppInfoArray[index]; }
        }
        public void CopyTo(Array a, int index)
        {
            rAppInfoArray.CopyTo(a, index);
        }
        public int Count
        {
            get { return rAppInfoArray.Count; }
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
            return rAppInfoArray.GetEnumerator();
        }

        public void Add(ApplicationInfo newAppInfo)
        {
            rAppInfoArray.Add(newAppInfo);
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
}
