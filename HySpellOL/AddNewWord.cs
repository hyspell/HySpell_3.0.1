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
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NHunspell;

namespace HySpellOL
{
    public enum enHS_AffixType
    {
        None,
        NounAdjective,
        Verb,
        Other,
    }	
    public partial class frmAddNewWord : Form
    {
        private HySpellEncoder.Wrapper oEncoder;
        private Hunspell oWrapper;
        private string sNewWord = "";
        private int nWordEncoding = 2; // unicode
        private string sAffExample = "";
        private enHS_AffixType enAffixType = enHS_AffixType.None;
        private bool bHasErPlural = false;
        private bool bHasNerPlural = true;
        private bool bIsClassicOrtho = true;

        public int WordEncoding
        {
            get { return nWordEncoding; }
            set { nWordEncoding = value; }
        }
        public Hunspell HSWrapper
        {
            set { oWrapper = value; }
        }
        public HySpellEncoder.Wrapper HSEncoder
        {
            set { oEncoder = value; }
        }
        public string NewWord
        {
            get { return sNewWord; }
            set { sNewWord = value; }
        }
        public string AffixExample
        {
            get { return sAffExample; }
            set { sAffExample = value; }
        }
        public enHS_AffixType AffixType
        {
            get { return enAffixType; }
            set { enAffixType = value; }
        }
        public bool HasErPlural
        {
            get { return bHasErPlural; }
            set { bHasErPlural = value; }
        }
        public bool HasNerPlural
        {
            get { return bHasNerPlural; }
            set { bHasNerPlural = value; }
        }
        public bool IsClassicOrthography
        {
            get { return bIsClassicOrtho; }
            set { bIsClassicOrtho = value; }
        }
	
        public frmAddNewWord()
        {
            InitializeComponent();
            InitializeControls();
        }

        private void InitializeControls()
        {
            txtNewWord.Text = sNewWord;
            rbNone.Checked = true;
        }

        private void frmAddNewWord_Load(object sender, EventArgs e)
        {
            txtNewWord.Text = sNewWord;
        }
        private void SetAffixRadioButton()
        {
            switch (enAffixType)
            {
                case enHS_AffixType.NounAdjective:
                    rbNounAdjective.Checked = true;
                    break;
                case enHS_AffixType.Verb:
                    rbVerb.Checked = true;
                    break;
                case enHS_AffixType.Other:
                    rbOther.Checked = true;
                    break;
                default:
                    rbNone.Checked = true;
                    break;
            }
        }
        private bool IsWordInVerbForm(string sWord)
        {
            return (sWord.EndsWith("ել") || sWord.EndsWith("իլ") || sWord.EndsWith("ալ") || sWord.EndsWith("ուլ"));
        }
        private int IsWordInBasicInflection(string sWord)
        {
            //nWordEncoding ??
            int nRet = 0;
            if (sWord.EndsWith("ը"))
                nRet = -1;
            if (sWord.EndsWith("ներ"))
                nRet = -2;
            if (sWord.EndsWith("ն")
                && !sWord.EndsWith("ան")
                && !sWord.EndsWith("են")
                && !sWord.EndsWith("էն")
                && !sWord.EndsWith("ին")
                && !sWord.EndsWith("իւն")
                && !sWord.EndsWith("ուն")
                && !sWord.EndsWith("ոն")
                && !sWord.EndsWith("օն")
                && !sWord.EndsWith("յն")
                )
                nRet = -3;

            return nRet;
        }
        private bool IsWordArmenianAlpha(string sWord)
        {
            string sSubstr = sWord;
            int nPos = 0;
            while (sSubstr.Length > 0)
            {
                int nGap = sSubstr.IndexOfAny(Globals.ThisAddIn.IsArmenianChar);
                if (nGap != -1)
                    nPos += 1;
                else
                    return false;
                sSubstr = sWord.Substring(nPos);
            }

            return true;
        }
        private string DecodeMix(string sWord)
        {
            string sUNIWord = "";
            ArrayList arrDecoded = new ArrayList();
            bool bRetFlag = oEncoder.DecodeMixed(sWord, arrDecoded);
            if (arrDecoded.Count != 0)
            {
                sUNIWord = arrDecoded[0].ToString();
                sUNIWord = sUNIWord.ToLower();
            }

            return sUNIWord;
        }
        private string Encode(string sWord)
        {
            string sSCIIWord = "";
            ArrayList arrEncoded = new ArrayList();
            int nRet = oEncoder.Encode(sWord, arrEncoded);
            if (arrEncoded.Count != 0)
            {
                sSCIIWord = arrEncoded[0].ToString();
            }

            return sSCIIWord;
        }
        private string GetBuiltinExample(string sWord, string sStem, enHS_AffixType enWAffixType)
        {
            string sRetStem = "";
            switch (enWAffixType)
            {
                case enHS_AffixType.NounAdjective:
                    if (bIsClassicOrtho)
                    {
                        if (sWord.EndsWith("թիւն"))
                            sRetStem = "գիտութիւն";
                        else if (sWord.EndsWith("իւն"))
                            sRetStem = "անկիւն";
                        else if (sWord.EndsWith("եան"))
                            sRetStem = "արեւելեան";
                        else if (sWord.EndsWith("ական"))
                            sRetStem = "գիտական";
                        else if (sWord.EndsWith("արան"))
                            sRetStem = "բառարան";
                        else if (sWord.EndsWith("պան"))
                            sRetStem = "կառապան";
                        else if (sWord.EndsWith("ստան"))
                            sRetStem = "հայաստան";
                        else if (sWord.EndsWith("եղէն"))
                            sRetStem = "ընդեղէն";
                        else if (sWord.EndsWith("արէն"))
                            sRetStem = "յունարէն";
                        else if (sWord.EndsWith("երէն"))
                            sRetStem = "հայերէն";
                        else if (sWord.EndsWith("օրէն"))
                            sRetStem = "ազատօրէն";
                        else if (sWord.EndsWith("գին"))
                            sRetStem = "թանկագին";
                        else if (sWord.EndsWith("ային"))
                            sRetStem = "բարբառային";
                        else if (sWord.EndsWith("ովին"))
                            sRetStem = "կամովին";
                        else if (sWord.EndsWith("ին"))
                            sRetStem = "անկողին";
                        else if (sWord.EndsWith("գոյն"))
                            sRetStem = "գերագոյն";
                        else if (sWord.EndsWith("ուն"))
                            sRetStem = "արթուն";
                        else if (sWord.EndsWith("օն"))
                            sRetStem = "գործօն";
                        else if (sWord.EndsWith("ում"))
                            sRetStem = "ամրացում";
                        else if (sWord.EndsWith("իա"))
                            sRetStem = "ասիա";
                        else if (sWord.EndsWith("ա"))
                            sRetStem = "աֆրիկա";
                        else if (sWord.EndsWith("իւ"))
                            sRetStem = "թիւ";
                        else if (sWord.EndsWith("եր"))
                            sRetStem = "գրեր";
                        else if (sWord.EndsWith("ուայ"))
                            sRetStem = "այսօրուայ";
                        else if (sWord.EndsWith("եայ"))
                            sRetStem = "ադամանդեայ";
                        else if (sWord.EndsWith("իայ"))
                            sRetStem = "կրիայ";
                        else if (sWord.EndsWith("բայ") || sWord.EndsWith("հայ") || sWord.EndsWith("ճայ") || sWord.EndsWith("վայ"))
                            sRetStem = "հայ";
                        else if (sWord.EndsWith("այ"))
                            sRetStem = "ակռայ";
                        else if (sWord.EndsWith("ւոյ"))
                            sRetStem = "աթենացւոյ";
                        else if (sWord.EndsWith("ածոյ"))
                            sRetStem = "հաւաքածոյ";
                        else if (sWord.EndsWith("ոյ"))
                            sRetStem = "երեկոյ";
                        else if (sWord.EndsWith("օ"))
                            sRetStem = "մանգօ";
                        else if (sWord.EndsWith("իչ"))
                            sRetStem = "արարիչ";
                        else if (sWord.EndsWith("եղ"))
                            sRetStem = "աղեղ";
                        else if (sWord.EndsWith("ող"))
                            sRetStem = "գրող";
                        else if (sWord.EndsWith("անոց"))
                            sRetStem = "խոհանոց";
                        else if (sWord.EndsWith("նոց"))
                            sRetStem = "ակնոց";
                        else if (sWord.EndsWith("ոց"))
                            sRetStem = "ամրոց";
                        else if (sWord.EndsWith("եակ"))
                            sRetStem = "արբանեակ";
                        else if (sWord.EndsWith("իկ"))
                            sRetStem = "մարդիկ";
                        else if (sWord.EndsWith("ուկ"))
                            sRetStem = "արմուկ";
                        else if (sWord.EndsWith("իք"))
                            sRetStem = "ալիք";
                        else if (sWord.EndsWith("պէս"))
                            sRetStem = "ազգապէս";
                        else if (sWord.EndsWith("ատ"))
                            sRetStem = "աղքատ";
                        else if (sWord.EndsWith("աւէտ"))
                            sRetStem = "կենսաւէտ";
                        else if (sWord.EndsWith("ոտ"))
                            sRetStem = "երազկոտ";
                        else if (sWord.EndsWith("մուտ"))
                            sRetStem = "լուսամուտ";
                        else if (sWord.EndsWith("ուտ"))
                            sRetStem = "ժեռուտ";
                        else if (sWord.EndsWith("րորդ"))
                            sRetStem = "երկրորդ";
                        else if (sWord.EndsWith("որդ"))
                            sRetStem = "առաջնորդ";
                        else if (sWord.EndsWith("ուրդ"))
                            sRetStem = "խորհուրդ";
                        else if (sWord.EndsWith("ացու"))
                            sRetStem = "մայրացու";
                        else if (sWord.EndsWith("ցու"))
                            sRetStem = "եկեղեցու";
                        else if (sWord.EndsWith("հու"))
                            sRetStem = "դիցուհու";
                        else if (sWord.EndsWith("ու"))
                            sRetStem = "առտու";
                        else if (sWord.EndsWith("բար"))
                            sRetStem = "աշխարհաբար";
                        else if (sWord.EndsWith("աւոր"))
                            sRetStem = "աղեղնաւոր";
                        else if (sWord.EndsWith("ուոր"))
                            sRetStem = "զինուոր";
                        else if (sWord.EndsWith("ոյթ"))
                            sRetStem = "դրոյթ";
                        else if (sWord.EndsWith("ուած"))
                            sRetStem = "զանգուած";
                        else if (sWord.EndsWith("անք"))
                            sRetStem = "աշխատանք";
                        else if (sWord.EndsWith("ուհի"))
                            sRetStem = "դիցուհի";
                        else if (sWord.EndsWith("հի"))
                            sRetStem = "ամեհի";
                        else if (sWord.EndsWith("ացի"))
                            sRetStem = "գիւղացի";
                        else if (sWord.EndsWith("եցի"))
                            sRetStem = "դրսեցի";
                        else if (sWord.EndsWith("ցի"))
                            sRetStem = "գաւառցի";
                        else if (sWord.EndsWith("անի"))
                            sRetStem = "բազմոտանի";
                        else if (sWord.EndsWith("ենի"))
                            sRetStem = "այծենի";
                        else if (sWord.EndsWith("ալի"))
                            sRetStem = "զուարճալի";
                        else if (sWord.EndsWith("ելի"))
                            sRetStem = "ահռելի";
                        else if (sWord.EndsWith("ուի"))
                            sRetStem = "արծուի";
                        else if (sWord.EndsWith("ի"))
                            sRetStem = "յայտնի";
                        else
                            sRetStem = "նամակ";
                    }
                    else // reform case
                    {
                        if (sWord.EndsWith("թյուն"))
                            sRetStem = "գիտություն";
                        else if (sWord.EndsWith("յուն"))
                            sRetStem = "անկյուն";
                        else if (sWord.EndsWith("յան"))
                            sRetStem = "ավագյան";
                        else if (sWord.EndsWith("ական"))
                            sRetStem = "գիտական";
                        else if (sWord.EndsWith("արան"))
                            sRetStem = "բառարան";
                        else if (sWord.EndsWith("պան"))
                            sRetStem = "կառապան";
                        else if (sWord.EndsWith("ստան"))
                            sRetStem = "հայաստան";
                        else if (sWord.EndsWith("եղեն"))
                            sRetStem = "ընդեղեն";
                        else if (sWord.EndsWith("արեն"))
                            sRetStem = "հունարեն";
                        else if (sWord.EndsWith("երեն"))
                            sRetStem = "հայերեն";
                        else if (sWord.EndsWith("որեն"))
                            sRetStem = "ազատորեն";
                        else if (sWord.EndsWith("գին"))
                            sRetStem = "թանկագին";
                        else if (sWord.EndsWith("ային"))
                            sRetStem = "բարբառային";
                        else if (sWord.EndsWith("ովին"))
                            sRetStem = "կամովին";
                        else if (sWord.EndsWith("ին"))
                            sRetStem = "անկողին";
                        else if (sWord.EndsWith("գույն"))
                            sRetStem = "գերագույն";
                        else if (sWord.EndsWith("ուն"))
                            sRetStem = "արթուն";
                        else if (sWord.EndsWith("ոն"))
                            sRetStem = "գործոն";
                        else if (sWord.EndsWith("ում"))
                            sRetStem = "ամրացում";
                        else if (sWord.EndsWith("իա"))
                            sRetStem = "ասիա";
                        else if (sWord.EndsWith("ա"))
                            sRetStem = "աֆրիկա";
                        else if (sWord.EndsWith("իվ"))
                            sRetStem = "թիվ";
                        else if (sWord.EndsWith("եր"))
                            sRetStem = "գրեր";
                        else if (sWord.EndsWith("վա"))
                            sRetStem = "այսօրվա";
                        else if (sWord.EndsWith("յա"))
                            sRetStem = "ադամանդյա";
                        else if (sWord.EndsWith("բայ") || sWord.EndsWith("հայ") || sWord.EndsWith("ճայ") || sWord.EndsWith("վայ"))
                            sRetStem = "հայ";
                        else if (sWord.EndsWith("ա"))
                            sRetStem = "ակռա";
                        else if (sWord.EndsWith("վո"))
                            sRetStem = "աթենացվո";
                        else if (sWord.EndsWith("ածո"))
                            sRetStem = "հավաքածո";
                        else if (sWord.EndsWith("ո"))
                            sRetStem = "երեկո";
                        else if (sWord.EndsWith("օ"))
                            sRetStem = "մանգօ";
                        else if (sWord.EndsWith("իչ"))
                            sRetStem = "արարիչ";
                        else if (sWord.EndsWith("եղ"))
                            sRetStem = "աղեղ";
                        else if (sWord.EndsWith("ող"))
                            sRetStem = "գրող";
                        else if (sWord.EndsWith("անոց"))
                            sRetStem = "խոհանոց";
                        else if (sWord.EndsWith("նոց"))
                            sRetStem = "ակնոց";
                        else if (sWord.EndsWith("ոց"))
                            sRetStem = "ամրոց";
                        else if (sWord.EndsWith("յակ"))
                            sRetStem = "արբանյակ";
                        else if (sWord.EndsWith("իկ"))
                            sRetStem = "մարդիկ";
                        else if (sWord.EndsWith("ուկ"))
                            sRetStem = "արմուկ";
                        else if (sWord.EndsWith("իք"))
                            sRetStem = "ալիք";
                        else if (sWord.EndsWith("պես"))
                            sRetStem = "ազգապես";
                        else if (sWord.EndsWith("ատ"))
                            sRetStem = "աղքատ";
                        else if (sWord.EndsWith("ավետ"))
                            sRetStem = "կենսավետ";
                        else if (sWord.EndsWith("ոտ"))
                            sRetStem = "երազկոտ";
                        else if (sWord.EndsWith("մուտ"))
                            sRetStem = "լուսամուտ";
                        else if (sWord.EndsWith("ուտ"))
                            sRetStem = "ժեռուտ";
                        else if (sWord.EndsWith("րորդ"))
                            sRetStem = "երկրորդ";
                        else if (sWord.EndsWith("որդ"))
                            sRetStem = "առաջնորդ";
                        else if (sWord.EndsWith("ուրդ"))
                            sRetStem = "խորհուրդ";
                        else if (sWord.EndsWith("ացու"))
                            sRetStem = "մայրացու";
                        else if (sWord.EndsWith("ցու"))
                            sRetStem = "եկեղեցու";
                        else if (sWord.EndsWith("հու"))
                            sRetStem = "դիցուհու";
                        else if (sWord.EndsWith("ու"))
                            sRetStem = "առտու";
                        else if (sWord.EndsWith("բար"))
                            sRetStem = "աշխարհաբար";
                        else if (sWord.EndsWith("ավոր"))
                            sRetStem = "աղեղնավոր";
                        else if (sWord.EndsWith("վոր"))
                            sRetStem = "զինվոր";
                        else if (sWord.EndsWith("ույթ"))
                            sRetStem = "դրույթ";
                        else if (sWord.EndsWith("ված"))
                            sRetStem = "զանգված";
                        else if (sWord.EndsWith("անք"))
                            sRetStem = "աշխատանք";
                        else if (sWord.EndsWith("ուհի"))
                            sRetStem = "դիցուհի";
                        else if (sWord.EndsWith("հի"))
                            sRetStem = "ամեհի";
                        else if (sWord.EndsWith("ացի"))
                            sRetStem = "գյուղացի";
                        else if (sWord.EndsWith("եցի"))
                            sRetStem = "դրսեցի";
                        else if (sWord.EndsWith("ցի"))
                            sRetStem = "գավառցի";
                        else if (sWord.EndsWith("անի"))
                            sRetStem = "բազմոտանի";
                        else if (sWord.EndsWith("ենի"))
                            sRetStem = "այծենի";
                        else if (sWord.EndsWith("ալի"))
                            sRetStem = "զվարճալի";
                        else if (sWord.EndsWith("ելի"))
                            sRetStem = "ահռելի";
                        else if (sWord.EndsWith("վի"))
                            sRetStem = "արծվի";
                        else if (sWord.EndsWith("ի"))
                            sRetStem = "հայտնի";
                        else
                            sRetStem = "նամակ";
                    }
                    break;
                case enHS_AffixType.Verb:
                    if (bIsClassicOrtho)
                    {
                        if (sWord.EndsWith("ցնել"))
                            sRetStem = "կարդացնել";
                        else if (sWord.EndsWith("ձնել"))
                            sRetStem = "դարձնել";
                        else if (sWord.EndsWith("անել"))
                            sRetStem = "սերմանել";
                        else if (sWord.EndsWith("նել"))
                            sRetStem = "գտնել";
                        else if (sWord.EndsWith("չել"))
                            sRetStem = "հանգչել";
                        else if (sWord.EndsWith("ոտել"))
                            sRetStem = "կոխոտել";
                        else if (sWord.EndsWith("տել"))
                            sRetStem = "պատռտել";
                        else if (sWord.EndsWith("նուել"))
                            sRetStem = "տեսնուել";
                        else if (sWord.EndsWith("ցուել"))
                            sRetStem = "կարդացուել";
                        else if (sWord.EndsWith("ուել"))
                            sRetStem = "սիրուել";
                        else if (sWord.EndsWith("ել"))
                            sRetStem = "գործել";
                        else if (sWord.EndsWith("նիլ"))
                            sRetStem = "բուսնիլ";
                        else if (sWord.EndsWith("չիլ"))
                            sRetStem = "թռչիլ";
                        else if (sWord.EndsWith("նուիլ"))
                            sRetStem = "անկանուիլ";
                        else if (sWord.EndsWith("ցուիլ"))
                            sRetStem = "կարդացուիլ";
                        else if (sWord.EndsWith("ուիլ"))
                            sRetStem = "ալարուիլ";
                        else if (sWord.EndsWith("իլ"))
                            sRetStem = "խօսիլ";
                        else if (sWord.EndsWith("անալ"))
                            sRetStem = "վերջանալ";
                        else if (sWord.EndsWith("ենալ"))
                            sRetStem = "այգենալ";
                        else if (sWord.EndsWith("նալ"))
                            sRetStem = "աւելնալ";
                        else if (sWord.EndsWith("ալ"))
                            sRetStem = "խաղալ";
                        else if (sWord.EndsWith("ուլ"))
                            sRetStem = "զբօսնուլ";
                    }
                    else  // reform case
                    {
                        if (sWord.EndsWith("ցնել"))
                            sRetStem = "կարդացնել";
                        else if (sWord.EndsWith("ձնել"))
                            sRetStem = "դարձնել";
                        else if (sWord.EndsWith("անել"))
                            sRetStem = "սերմանել";
                        else if (sWord.EndsWith("նել"))
                            sRetStem = "գտնել";
                        else if (sWord.EndsWith("չել"))
                            sRetStem = "հանգչել";
                        else if (sWord.EndsWith("ոտել"))
                            sRetStem = "կոխոտել";
                        else if (sWord.EndsWith("տել"))
                            sRetStem = "պատռտել";
                        else if (sWord.EndsWith("նվել"))
                            sRetStem = "տեսնվել";
                        else if (sWord.EndsWith("ցվել"))
                            sRetStem = "կարդացվել";
                        else if (sWord.EndsWith("վել"))
                            sRetStem = "սիրվել";
                        else if (sWord.EndsWith("ել"))
                            sRetStem = "գործել";
                        else if (sWord.EndsWith("նիլ"))
                            sRetStem = "բուսնիլ";
                        else if (sWord.EndsWith("չիլ"))
                            sRetStem = "թռչիլ";
                        else if (sWord.EndsWith("նվիլ"))
                            sRetStem = "անկանվիլ";
                        else if (sWord.EndsWith("ցվիլ"))
                            sRetStem = "կարդացվիլ";
                        else if (sWord.EndsWith("վիլ"))
                            sRetStem = "ալարվիլ";
                        else if (sWord.EndsWith("իլ"))
                            sRetStem = "խոսիլ";
                        else if (sWord.EndsWith("անալ"))
                            sRetStem = "վերջանալ";
                        else if (sWord.EndsWith("ենալ"))
                            sRetStem = "այգենալ";
                        else if (sWord.EndsWith("նալ"))
                            sRetStem = "ավելնալ";
                        else if (sWord.EndsWith("ալ"))
                            sRetStem = "խաղալ";
                        else if (sWord.EndsWith("ուլ"))
                            sRetStem = "զբոսնուլ";
                    }
                    break;
                case enHS_AffixType.Other:
                    sRetStem = sStem;
                    break;
                default:
                    break;
            }
            return sRetStem;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string sUNIStem = "";
            string sNWord = txtNewWord.Text;
            // check if all entry characters are unicode Armenian alphas
            if (!IsWordArmenianAlpha(sNWord))
            {
                MessageBox.Show("Նոր Բառ դաշտում կարելի է միայն հայերէն տառեր ներածել։",
                                 "Microsoft Office Word", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNewWord.Focus();
                return;
            }
            // convert the word string into unicode and then lower case
            sNWord = DecodeMix(sNWord);

            // case when Noun/Adjective: check if word ends with ներ, ն, ը
            if (IsWordInBasicInflection(sNWord) == -1)
            {
                MessageBox.Show("Ընտրել էք Գոյական/Ածական խոնարհման ձեւը, սակայն Նոր Բառ դաշտում հոլովուած է 'ը' յօդով, եթէ կարելի է հեռացնել այս յօդը եւ վերստին փորձել։",
                                 "Microsoft Office Word", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNewWord.Focus();
                return;
            }
            else if (IsWordInBasicInflection(sNWord) == -2)
            {
                if (MessageBox.Show("Ընտրել էք Գոյական/Ածական խոնարհման ձեւը, սակայն Նոր Բառ դաշտում հոլովուած է 'ներ' յօդով։ Կ՚ցանկանայի՞ք շարունակել եւ աւելացնել բառը բառարանում։",
                                 "Microsoft Office Word", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    txtNewWord.Focus();
                    return;
                }
            }
            else if (IsWordInBasicInflection(sNWord) == -3)
            {
                if (MessageBox.Show("Ընտրել էք Գոյական/Ածական խոնարհման ձեւը, սակայն Նոր Բառ դաշտում հոլովուած է 'ն' յօդով։ Կ՚ցանկանայի՞ք շարունակել եւ աւելացնել բառը բառարանում։",
                                 "Microsoft Office Word", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    txtNewWord.Focus();
                    return;
                }
            }
            // check if verb type is selected and the new word is in infinitive form
            if (rbVerb.Checked && !IsWordInVerbForm(sNWord))
            {
                MessageBox.Show("Ընտրել էք Բայ խոնարհման ձեւը, Նոր Բառ դաշտում անհրաժեշտ է ներածել բայի աներեւոյթ ձեւը։ Բայի աներեւոյթ եղանակը ունի հետեւեալ վերջաւորութիւնները՝ 'ել', 'իլ', 'ալ' կամ 'ուլ'։",
                                "Microsoft Office Word", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNewWord.Focus();
                return;
            }

            // case other and matching of the Example Word entry
            if (rbOther.Checked)
            {
                string sExample = txtExample.Text;
                if (sExample.Length > 0)
                {
                    // encode word into ARMSCII to pass it to wrapper
                    string sSCIIOutput;
                    ArrayList arrEncoded = new ArrayList();
                    int nCount = oEncoder.Encode_U(DecodeMix(sExample), arrEncoded);
                    if (arrEncoded.Count != 0)
                    {
                        sSCIIOutput = arrEncoded[0].ToString();
                        //ArrayList arrStems = new ArrayList();
                        // get stem of the example word if exists
                        //int nStemCount = oWrapper.GetStem_U(arrStems, sSCIIOutput);
                        List<string> stems = oWrapper.Stem(sSCIIOutput);
                        if (stems.Count != 0)
                        {
                            string sMsg = "Բառի Օրինակ դաշտի արժէքի բառարմատը '{0}' է, Նոր Բառ դաշտի մէջ ներածէք '{1}'–ի նման վերջաւորութեամբ բառարմատ, կամ փորձէք մի ուրիշ բառօրինակ։";
                            string sMessage = "";
                            // get min-lenght stem
                            int nMinIndex = 0;
                            int nMinLength = stems[0].Length;
                            for (int j = 0; j < stems.Count; j++)
                            {
                                string sDCode = DecodeMix(stems[j].ToString());
                                if (sNWord.EndsWith(sDCode) && sDCode.Length == 3 && sDCode.EndsWith("ի"))
                                {
                                    nMinIndex = j;
                                    break;
                                }
                                if (IsWordInVerbForm(sDCode))
                                {
                                    nMinIndex = j;
                                    break;
                                }
                                if (stems[j].ToString().Length < nMinLength)
                                {
                                    nMinLength = stems[j].ToString().Length;
                                    nMinIndex = j;
                                }
                            }
                            sSCIIOutput = stems[nMinIndex].ToString();
                            sUNIStem = DecodeMix(sSCIIOutput);
                            // if the example word's stem is a infinitive verb form ending, must see if the new word has 
                            // similar ending: ել, իլ, ալ, ուլ, ուել, ուիլ, նել, չել, ցնել, նալ
                            if (IsWordInVerbForm(sUNIStem))
                            {
                                if (sUNIStem.EndsWith("ել"))
                                {
                                    if (!sNWord.EndsWith("ել"))
                                        sMessage = String.Format(sMsg, sUNIStem, "ել");
                                    else
                                    {
                                        if (sUNIStem.EndsWith("ցնել") && !sNWord.EndsWith("ցնել"))
                                            sMessage = String.Format(sMsg, sUNIStem, "ցնել");
                                        else if (sUNIStem.EndsWith("նել") && !sNWord.EndsWith("նել"))
                                            sMessage = String.Format(sMsg, sUNIStem, "նել");
                                        else if (sUNIStem.EndsWith("չել") && !sNWord.EndsWith("չել"))
                                            sMessage = String.Format(sMsg, sUNIStem, "չել");
                                        else if (sUNIStem.EndsWith("ուել") && !sNWord.EndsWith("ուել"))
                                            sMessage = String.Format(sMsg, sUNIStem, "ուել");
                                        else if (sUNIStem.EndsWith("վել") && !sNWord.EndsWith("վել"))
                                            sMessage = String.Format(sMsg, sUNIStem, "վել");
                                    }
                                }
                                else if (sUNIStem.EndsWith("իլ"))
                                {
                                    if (!sNWord.EndsWith("իլ"))
                                        sMessage = String.Format(sMsg, sUNIStem, "իլ");
                                    else
                                    {
                                        if (sUNIStem.EndsWith("ուիլ") && !sNWord.EndsWith("ուիլ"))
                                            sMessage = String.Format(sMsg, sUNIStem, "ուիլ");
                                        else if (sUNIStem.EndsWith("վիլ") && !sNWord.EndsWith("վիլ"))
                                            sMessage = String.Format(sMsg, sUNIStem, "վիլ");
                                    }
                                }
                                else if (sUNIStem.EndsWith("ալ"))
                                {
                                    if (!sNWord.EndsWith("ալ"))
                                        sMessage = String.Format(sMsg, sUNIStem, "ալ");
                                    else
                                    {
                                        if (sUNIStem.EndsWith("նալ") && !sNWord.EndsWith("նալ"))
                                            sMessage = String.Format(sMsg, sUNIStem, "նալ");
                                    }
                                }
                                else if (sUNIStem.EndsWith("ուլ") && !sNWord.EndsWith("ուլ"))
                                    sMessage = String.Format(sMsg, sUNIStem, "ուլ");
                            }
                            else
                            {
                                // other cases that need consideration are: հու, ու, ի, հի, ացի, ուի, օ, ոյ, այ, եր, իւ, ա, իւն, ուայ, ում, իա
                                if (sUNIStem.EndsWith("ու"))
                                {
                                    if (!sNWord.EndsWith("ու"))
                                        sMessage = String.Format(sMsg, sUNIStem, "ու");
                                    else
                                    {
                                        if (sUNIStem.EndsWith("հու") && !sNWord.EndsWith("հու"))
                                            sMessage = String.Format(sMsg, sUNIStem, "հու");
                                    }
                                }
                                else if (sUNIStem.EndsWith("ի"))
                                {
                                    if (!sNWord.EndsWith("ի"))
                                        sMessage = String.Format(sMsg, sUNIStem, "ի");
                                    else
                                    {
                                        if (sUNIStem.EndsWith("հի") && !sNWord.EndsWith("հի"))
                                            sMessage = String.Format(sMsg, sUNIStem, "հի");
                                        if (sUNIStem.EndsWith("ացի") && !sNWord.EndsWith("ացի"))
                                            sMessage = String.Format(sMsg, sUNIStem, "ացի");
                                        if (sUNIStem.EndsWith("ուի") && !sNWord.EndsWith("ուի"))
                                            sMessage = String.Format(sMsg, sUNIStem, "ուի");
                                        if (sUNIStem.EndsWith("վի") && !sNWord.EndsWith("վի"))
                                            sMessage = String.Format(sMsg, sUNIStem, "վի");
                                    }
                                }
                                else if (sUNIStem.EndsWith("օ") && !sNWord.EndsWith("օ"))
                                    sMessage = String.Format(sMsg, sUNIStem, "օ");
                                else if (sUNIStem.EndsWith("ո") && !sNWord.EndsWith("ո"))
                                    sMessage = String.Format(sMsg, sUNIStem, "ո");
                                else if (sUNIStem.EndsWith("ա"))
                                {
                                    if (!sNWord.EndsWith("ա"))
                                        sMessage = String.Format(sMsg, sUNIStem, "ա");
                                    else if (sUNIStem.EndsWith("իա") && !sNWord.EndsWith("իա"))
                                        sMessage = String.Format(sMsg, sUNIStem, "իա");
                                    else if (sUNIStem.EndsWith("յա") && !sNWord.EndsWith("յա"))
                                        sMessage = String.Format(sMsg, sUNIStem, "յա");
                                }
                                else if (sUNIStem.EndsWith("յ"))
                                {
                                    if (sUNIStem.EndsWith("այ") && !sNWord.EndsWith("այ"))
                                        sMessage = String.Format(sMsg, sUNIStem, "այ");
                                    if (sUNIStem.EndsWith("ուայ") && !sNWord.EndsWith("ուայ"))
                                        sMessage = String.Format(sMsg, sUNIStem, "ուայ");
                                    if (sUNIStem.EndsWith("ոյ") && !sNWord.EndsWith("ոյ"))
                                        sMessage = String.Format(sMsg, sUNIStem, "ոյ");
                                }
                                else if (sUNIStem.EndsWith("իւն") && !sNWord.EndsWith("իւն"))
                                    sMessage = String.Format(sMsg, sUNIStem, "իւն");
                                else if (sUNIStem.EndsWith("յուն") && !sNWord.EndsWith("յուն"))
                                    sMessage = String.Format(sMsg, sUNIStem, "յուն");
                                else if (sUNIStem.EndsWith("իւ") && !sNWord.EndsWith("իւ"))
                                    sMessage = String.Format(sMsg, sUNIStem, "իւ");
                                else if (sUNIStem.EndsWith("իվ") && !sNWord.EndsWith("իվ"))
                                    sMessage = String.Format(sMsg, sUNIStem, "իվ");
                                else if (sUNIStem.EndsWith("ում") && !sNWord.EndsWith("ում"))
                                    sMessage = String.Format(sMsg, sUNIStem, "ում");
                                else if (sUNIStem.EndsWith("եր") && !sNWord.EndsWith("եր"))
                                    sMessage = String.Format(sMsg, sUNIStem, "եր");
                            }
                            // if message is not empty then there was a mismatch between new word and example word stem
                            if (sMessage.Length > 0)
                            {
                                MessageBox.Show(sMessage, "Microsoft Office Word", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                txtNewWord.Focus();
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Բառի Օրինակ դաշտի բառը չի գտնւում բառարանում, եթէ կարելի է մի ուրիշ բառ ներածէք։",
                                            "Microsoft Office Word", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtExample.Focus();
                            return;
                        }
                    }
                    else
                    {
                        string str = string.Format("{0}, {1}", oEncoder == null, nCount);
                        MessageBox.Show(str);
                        return; // encode problem
                    }
                }
                else return;
            }
            
            //// match a buildin example word based on the word-type and the new word's ending pattern
            //// pass the (word, example) pair to the parent of the dialog in ARMSCII encoding
            //sNewWord = Encode(sNWord);
            sNewWord = sNWord;
            sAffExample = GetBuiltinExample(sNWord, sUNIStem, enAffixType);
            //if (sAffExample.Length > 0)
            //    sAffExample = Encode(sAffExample);

            DialogResult = DialogResult.OK;
        }

        private void rbVerb_CheckedChanged(object sender, EventArgs e)
        {
            string sNWord = txtNewWord.Text;
            if (sNWord.Length > 0 && rbVerb.Checked)
            {
                if (!IsWordInVerbForm(sNWord))
                {
                    MessageBox.Show("Նախ քան այս ընտրանքը՝ անհրաժեշտ է Նոր Բառ դաշտում ներածել բայի աներեւոյթ ձեւը։ Բայի աներեւոյթ եղանակը ունի հետեւեալ վերջաւորութիւնները՝ 'ել', 'իլ', 'ալ' կամ 'ուլ'։",
                                    "Microsoft Office Word", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    SetAffixRadioButton();
                }
                else
                    enAffixType = enHS_AffixType.Verb;
            }
            else if (rbVerb.Checked)
            {
                MessageBox.Show("Նոր Բառ դաշտը պէտք չէ դատարկ լինի։",
                                "Microsoft Office Word", MessageBoxButtons.OK, MessageBoxIcon.Information);
                rbNone.Checked = true;
            }
        }
        private void rbNone_CheckedChanged(object sender, EventArgs e)
        {
            enAffixType = enHS_AffixType.None;
        }
        private void rbNounAdjective_CheckedChanged(object sender, EventArgs e)
        {
            enAffixType = enHS_AffixType.NounAdjective;
            chkEr.Enabled = chkNer.Enabled = rbNounAdjective.Checked;
        }
        private void rbOther_CheckedChanged(object sender, EventArgs e)
        {
            enAffixType = enHS_AffixType.Other;
            txtExample.Enabled = rbOther.Checked;
        }

        private void frmAddNewWord_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (oEncoder != null)
            {
                oEncoder.Dispose();
                oEncoder = null;
            }
        }

    }
}