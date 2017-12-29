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
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Office = Microsoft.Office.Core;
using System.Windows.Forms;


namespace HySpellOL
{
    public partial class ThisAddIn
    {
        public char[] IsArmenianChar = {/* UNICODE chars */
                                        '\u0531',		// ARMENIAN CAPITAL LETTER AYB
		                                '\u0561',		// ARMENIAN SMALL LETTER AYB
		                                '\u0532',		// ARMENIAN CAPITAL LETTER BEN
		                                '\u0562',		// ARMENIAN SMALL LETTER BEN
		                                '\u0533',		// ARMENIAN CAPITAL LETTER GIM
		                                '\u0563',		// ARMENIAN SMALL LETTER GIM
		                                '\u0534',		// ARMENIAN CAPITAL LETTER DA
		                                '\u0564',		// ARMENIAN SMALL LETTER DA
		                                '\u0535',		// ARMENIAN CAPITAL LETTER YECH
		                                '\u0565',		// ARMENIAN SMALL LETTER YECH
		                                '\u0536',		// ARMENIAN CAPITAL LETTER ZA
		                                '\u0566',		// ARMENIAN SMALL LETTER ZA
		                                '\u0537',		// ARMENIAN CAPITAL LETTER E
		                                '\u0567',		// ARMENIAN SMALL LETTER E
		                                '\u0538',		// ARMENIAN CAPITAL LETTER AT
		                                '\u0568',		// ARMENIAN SMALL LETTER AT
		                                '\u0539',		// ARMENIAN CAPITAL LETTER TO
		                                '\u0569',		// ARMENIAN SMALL LETTER TO
		                                '\u053A',		// ARMENIAN CAPITAL LETTER ZHE
		                                '\u056A',		// ARMENIAN SMALL LETTER ZHE
		                                '\u053B',		// ARMENIAN CAPITAL LETTER INI
		                                '\u056B',		// ARMENIAN SMALL LETTER INI
		                                '\u053C',		// ARMENIAN CAPITAL LETTER LYUN
		                                '\u056C',		// ARMENIAN SMALL LETTER LYUN
		                                '\u053D',		// ARMENIAN CAPITAL LETTER KHE
		                                '\u056D',		// ARMENIAN SMALL LETTER KHE
		                                '\u053E',		// ARMENIAN CAPITAL LETTER TSA
		                                '\u056E',		// ARMENIAN SMALL LETTER TSA
		                                '\u053F',		// ARMENIAN CAPITAL LETTER KEN
		                                '\u056F',		// ARMENIAN SMALL LETTER KEN
		                                '\u0540',		// ARMENIAN CAPITAL LETTER HO
		                                '\u0570',		// ARMENIAN SMALL LETTER HO
		                                '\u0541',		// ARMENIAN CAPITAL LETTER DZA
		                                '\u0571',		// ARMENIAN SMALL LETTER DZA
		                                '\u0542',		// ARMENIAN CAPITAL LETTER GHAT
		                                '\u0572',		// ARMENIAN SMALL LETTER GHAT
		                                '\u0543',		// ARMENIAN CAPITAL LETTER TCHE
		                                '\u0573',		// ARMENIAN SMALL LETTER TCHE
		                                '\u0544',		// ARMENIAN CAPITAL LETTER MEN
		                                '\u0574',		// ARMENIAN SMALL LETTER MEN
		                                '\u0545',		// ARMENIAN CAPITAL LETTER HI
		                                '\u0575',		// ARMENIAN SMALL LETTER HI
		                                '\u0546',		// ARMENIAN CAPITAL LETTER NU
		                                '\u0576',		// ARMENIAN SMALL LETTER NU
		                                '\u0547',		// ARMENIAN CAPITAL LETTER SHA
		                                '\u0577',		// ARMENIAN SMALL LETTER SHA
		                                '\u0548',		// ARMENIAN CAPITAL LETTER VO
		                                '\u0578',		// ARMENIAN SMALL LETTER VO
		                                '\u0549',		// ARMENIAN CAPITAL LETTER CHA
		                                '\u0579',		// ARMENIAN SMALL LETTER CHA
		                                '\u054A',		// ARMENIAN CAPITAL LETTER PE
		                                '\u057A',		// ARMENIAN SMALL LETTER PE
		                                '\u054B',		// ARMENIAN CAPITAL LETTER JE
		                                '\u057B',		// ARMENIAN SMALL LETTER JE
		                                '\u054C',		// ARMENIAN CAPITAL LETTER RA
		                                '\u057C',		// ARMENIAN SMALL LETTER RA
		                                '\u054D',		// ARMENIAN CAPITAL LETTER SE
		                                '\u057D',		// ARMENIAN SMALL LETTER SE
		                                '\u054E',		// ARMENIAN CAPITAL LETTER VEV
		                                '\u057E',		// ARMENIAN SMALL LETTER VEV
		                                '\u054F',		// ARMENIAN CAPITAL LETTER TYUN
		                                '\u057F',		// ARMENIAN SMALL LETTER TYUN
		                                '\u0550',		// ARMENIAN CAPITAL LETTER RE
		                                '\u0580',		// ARMENIAN SMALL LETTER RE
		                                '\u0551',		// ARMENIAN CAPITAL LETTER TSO
		                                '\u0581',		// ARMENIAN SMALL LETTER TSO
		                                '\u0552',		// ARMENIAN CAPITAL LETTER VYUN
		                                '\u0582',		// ARMENIAN SMALL LETTER VYUN
		                                '\u0553',		// ARMENIAN CAPITAL LETTER PYUR
		                                '\u0583',		// ARMENIAN SMALL LETTER PYUR
		                                '\u0554',		// ARMENIAN CAPITAL LETTER KE
		                                '\u0584',		// ARMENIAN SMALL LETTER KE
		                                '\u0555',		// ARMENIAN CAPITAL LETTER O
		                                '\u0585',		// ARMENIAN SMALL LETTER O
		                                '\u0556',		// ARMENIAN CAPITAL LETTER FE
		                                '\u0586',		// ARMENIAN SMALL LETTER FE
                                        /* ARMSCII-8 chars */
  		                                '\u00B2',		// ARMENIAN CAPITAL LETTER AYB
		                                '\u00B3',		// ARMENIAN SMALL LETTER AYB
		                                '\u00B4',		// ARMENIAN CAPITAL LETTER BEN
		                                '\u00B5',		// ARMENIAN SMALL LETTER BEN
		                                '\u00B6',		// ARMENIAN CAPITAL LETTER GIM
		                                '\u00B7',		// ARMENIAN SMALL LETTER GIM
		                                '\u00B8',		// ARMENIAN CAPITAL LETTER DA
		                                '\u00B9',		// ARMENIAN SMALL LETTER DA
		                                '\u00BA',		// ARMENIAN CAPITAL LETTER YECH
		                                '\u00BB',		// ARMENIAN SMALL LETTER YECH
		                                '\u00BC',		// ARMENIAN CAPITAL LETTER ZA
		                                '\u00BD',		// ARMENIAN SMALL LETTER ZA
		                                '\u00BE',		// ARMENIAN CAPITAL LETTER E
		                                '\u00BF',		// ARMENIAN SMALL LETTER E
		                                '\u00C0',		// ARMENIAN CAPITAL LETTER AT
		                                '\u00C1',		// ARMENIAN SMALL LETTER AT
		                                '\u00C2',		// ARMENIAN CAPITAL LETTER TO
		                                '\u00C3',		// ARMENIAN SMALL LETTER TO
		                                '\u00C4',		// ARMENIAN CAPITAL LETTER ZHE
		                                '\u00C5',		// ARMENIAN SMALL LETTER ZHE
		                                '\u00C6',		// ARMENIAN CAPITAL LETTER INI
		                                '\u00C7',		// ARMENIAN SMALL LETTER INI
		                                '\u00C8',		// ARMENIAN CAPITAL LETTER LYUN
		                                '\u00C9',		// ARMENIAN SMALL LETTER LYUN
		                                '\u00CA',		// ARMENIAN CAPITAL LETTER KHE
		                                '\u00CB',		// ARMENIAN SMALL LETTER KHE
		                                '\u00CC',		// ARMENIAN CAPITAL LETTER TSA
		                                '\u00CD',		// ARMENIAN SMALL LETTER TSA
		                                '\u00CE',		// ARMENIAN CAPITAL LETTER KEN
		                                '\u00CF',		// ARMENIAN SMALL LETTER KEN
		                                '\u00D0',		// ARMENIAN CAPITAL LETTER HO
		                                '\u00D1',		// ARMENIAN SMALL LETTER HO
		                                '\u00D2',		// ARMENIAN CAPITAL LETTER DZA
		                                '\u00D3',		// ARMENIAN SMALL LETTER DZA
		                                '\u00D4',		// ARMENIAN CAPITAL LETTER GHAT
		                                '\u00D5',		// ARMENIAN SMALL LETTER GHAT
		                                '\u00D6',		// ARMENIAN CAPITAL LETTER TCHE
		                                '\u00D7',		// ARMENIAN SMALL LETTER TCHE
		                                '\u00D8',		// ARMENIAN CAPITAL LETTER MEN
		                                '\u00D9',		// ARMENIAN SMALL LETTER MEN
		                                '\u00DA',		// ARMENIAN CAPITAL LETTER HI
		                                '\u00DB',		// ARMENIAN SMALL LETTER HI
		                                '\u00DC',		// ARMENIAN CAPITAL LETTER NU
		                                '\u00DD',		// ARMENIAN SMALL LETTER NU
		                                '\u00DE',		// ARMENIAN CAPITAL LETTER SHA
		                                '\u00DF',		// ARMENIAN SMALL LETTER SHA
		                                '\u00E0',		// ARMENIAN CAPITAL LETTER VO
		                                '\u00E1',		// ARMENIAN SMALL LETTER VO
		                                '\u00E2',		// ARMENIAN CAPITAL LETTER CHA
		                                '\u00E3',		// ARMENIAN SMALL LETTER CHA
		                                '\u00E4',		// ARMENIAN CAPITAL LETTER PE
		                                '\u00E5',		// ARMENIAN SMALL LETTER PE
		                                '\u00E6',		// ARMENIAN CAPITAL LETTER JE
		                                '\u00E7',		// ARMENIAN SMALL LETTER JE
		                                '\u00E8',		// ARMENIAN CAPITAL LETTER RA
		                                '\u00E9',		// ARMENIAN SMALL LETTER RA
		                                '\u00EA',		// ARMENIAN CAPITAL LETTER SE
		                                '\u00EB',		// ARMENIAN SMALL LETTER SE
		                                '\u00EC',		// ARMENIAN CAPITAL LETTER VEV
		                                '\u00ED',		// ARMENIAN SMALL LETTER VEV
		                                '\u00EE',		// ARMENIAN CAPITAL LETTER TYUN
		                                '\u00EF',		// ARMENIAN SMALL LETTER TYUN
		                                '\u00F0',		// ARMENIAN CAPITAL LETTER RE
		                                '\u00F1',		// ARMENIAN SMALL LETTER RE
		                                '\u00F2',		// ARMENIAN CAPITAL LETTER TSO
		                                '\u00F3',		// ARMENIAN SMALL LETTER TSO
		                                '\u00F4',		// ARMENIAN CAPITAL LETTER VYUN
		                                '\u00F5',		// ARMENIAN SMALL LETTER VYUN
		                                '\u00F6',		// ARMENIAN CAPITAL LETTER PYUR
		                                '\u00F7',		// ARMENIAN SMALL LETTER PYUR
		                                '\u00F8',		// ARMENIAN CAPITAL LETTER KE
		                                '\u00F9',		// ARMENIAN SMALL LETTER KE
		                                '\u00FA',		// ARMENIAN CAPITAL LETTER O
		                                '\u00FB',		// ARMENIAN SMALL LETTER O
		                                '\u00FC',		// ARMENIAN CAPITAL LETTER FE
		                                '\u00FD',		// ARMENIAN SMALL LETTER FE                                      
                                        };
        public char[] IsArmenianAccent = {/* UNICODE ACCENTS */
                                        '\u055E',		// ARMENIAN QUESTION MARK
                                        '\u055B',       // ARMENIAN EMPHASIS MARK
                                        '\u055C',       // ARMENIAN EXCLAMATION MARK
                                        '\u058A',       // ARMENIAN HYPHEN
                                        '\u2010',       // SOFT HYPHEN
                                        /* ARMSCII-8 ACCENTS */
                                        '\u00B1',		// ARMENIAN QUESTION MARK
                                        '\u00B0',       // ARMENIAN EMPHASIS MARK
                                        '\u00AF',       // ARMENIAN EXCLAMATION MARK
                                        '\u00AD',       // HYPHEN
                                        '\u002D',       // MINUS-HYPHEN
                                        };
        public char[] IsArmenianApostrophe = {/* UNICODE ACCENTS */
                                        '\u055A',       // ARMENIAN APOSTROPHE
                                        /* ARMSCII-8 ACCENTS */
                                        '\u00FE',       // ARMENIAN APOSTROPHE
                                        '\u0027',       // APOSTROPHE
                                        };
        public char[] IsArmenianHyphen = {/* UNICODE ACCENTS */
                                        '\u058A',       // ARMENIAN HYPHEN
                                        '\u2010',       // SOFT HYPHEN
                                        /* ARMSCII-8 ACCENTS */
                                        '\u00AD',       // HYPHEN
                                        '\u002D',       // MINUS-HYPHEN
                                        };

        public char[] IsArmenianCharOrAccent = {/* UNICODE chars */
                                        '\u0531',		// ARMENIAN CAPITAL LETTER AYB
		                                '\u0561',		// ARMENIAN SMALL LETTER AYB
		                                '\u0532',		// ARMENIAN CAPITAL LETTER BEN
		                                '\u0562',		// ARMENIAN SMALL LETTER BEN
		                                '\u0533',		// ARMENIAN CAPITAL LETTER GIM
		                                '\u0563',		// ARMENIAN SMALL LETTER GIM
		                                '\u0534',		// ARMENIAN CAPITAL LETTER DA
		                                '\u0564',		// ARMENIAN SMALL LETTER DA
		                                '\u0535',		// ARMENIAN CAPITAL LETTER YECH
		                                '\u0565',		// ARMENIAN SMALL LETTER YECH
		                                '\u0536',		// ARMENIAN CAPITAL LETTER ZA
		                                '\u0566',		// ARMENIAN SMALL LETTER ZA
		                                '\u0537',		// ARMENIAN CAPITAL LETTER E
		                                '\u0567',		// ARMENIAN SMALL LETTER E
		                                '\u0538',		// ARMENIAN CAPITAL LETTER AT
		                                '\u0568',		// ARMENIAN SMALL LETTER AT
		                                '\u0539',		// ARMENIAN CAPITAL LETTER TO
		                                '\u0569',		// ARMENIAN SMALL LETTER TO
		                                '\u053A',		// ARMENIAN CAPITAL LETTER ZHE
		                                '\u056A',		// ARMENIAN SMALL LETTER ZHE
		                                '\u053B',		// ARMENIAN CAPITAL LETTER INI
		                                '\u056B',		// ARMENIAN SMALL LETTER INI
		                                '\u053C',		// ARMENIAN CAPITAL LETTER LYUN
		                                '\u056C',		// ARMENIAN SMALL LETTER LYUN
		                                '\u053D',		// ARMENIAN CAPITAL LETTER KHE
		                                '\u056D',		// ARMENIAN SMALL LETTER KHE
		                                '\u053E',		// ARMENIAN CAPITAL LETTER TSA
		                                '\u056E',		// ARMENIAN SMALL LETTER TSA
		                                '\u053F',		// ARMENIAN CAPITAL LETTER KEN
		                                '\u056F',		// ARMENIAN SMALL LETTER KEN
		                                '\u0540',		// ARMENIAN CAPITAL LETTER HO
		                                '\u0570',		// ARMENIAN SMALL LETTER HO
		                                '\u0541',		// ARMENIAN CAPITAL LETTER DZA
		                                '\u0571',		// ARMENIAN SMALL LETTER DZA
		                                '\u0542',		// ARMENIAN CAPITAL LETTER GHAT
		                                '\u0572',		// ARMENIAN SMALL LETTER GHAT
		                                '\u0543',		// ARMENIAN CAPITAL LETTER TCHE
		                                '\u0573',		// ARMENIAN SMALL LETTER TCHE
		                                '\u0544',		// ARMENIAN CAPITAL LETTER MEN
		                                '\u0574',		// ARMENIAN SMALL LETTER MEN
		                                '\u0545',		// ARMENIAN CAPITAL LETTER HI
		                                '\u0575',		// ARMENIAN SMALL LETTER HI
		                                '\u0546',		// ARMENIAN CAPITAL LETTER NU
		                                '\u0576',		// ARMENIAN SMALL LETTER NU
		                                '\u0547',		// ARMENIAN CAPITAL LETTER SHA
		                                '\u0577',		// ARMENIAN SMALL LETTER SHA
		                                '\u0548',		// ARMENIAN CAPITAL LETTER VO
		                                '\u0578',		// ARMENIAN SMALL LETTER VO
		                                '\u0549',		// ARMENIAN CAPITAL LETTER CHA
		                                '\u0579',		// ARMENIAN SMALL LETTER CHA
		                                '\u054A',		// ARMENIAN CAPITAL LETTER PE
		                                '\u057A',		// ARMENIAN SMALL LETTER PE
		                                '\u054B',		// ARMENIAN CAPITAL LETTER JE
		                                '\u057B',		// ARMENIAN SMALL LETTER JE
		                                '\u054C',		// ARMENIAN CAPITAL LETTER RA
		                                '\u057C',		// ARMENIAN SMALL LETTER RA
		                                '\u054D',		// ARMENIAN CAPITAL LETTER SE
		                                '\u057D',		// ARMENIAN SMALL LETTER SE
		                                '\u054E',		// ARMENIAN CAPITAL LETTER VEV
		                                '\u057E',		// ARMENIAN SMALL LETTER VEV
		                                '\u054F',		// ARMENIAN CAPITAL LETTER TYUN
		                                '\u057F',		// ARMENIAN SMALL LETTER TYUN
		                                '\u0550',		// ARMENIAN CAPITAL LETTER RE
		                                '\u0580',		// ARMENIAN SMALL LETTER RE
		                                '\u0551',		// ARMENIAN CAPITAL LETTER TSO
		                                '\u0581',		// ARMENIAN SMALL LETTER TSO
		                                '\u0552',		// ARMENIAN CAPITAL LETTER VYUN
		                                '\u0582',		// ARMENIAN SMALL LETTER VYUN
		                                '\u0553',		// ARMENIAN CAPITAL LETTER PYUR
		                                '\u0583',		// ARMENIAN SMALL LETTER PYUR
		                                '\u0554',		// ARMENIAN CAPITAL LETTER KE
		                                '\u0584',		// ARMENIAN SMALL LETTER KE
		                                '\u0555',		// ARMENIAN CAPITAL LETTER O
		                                '\u0585',		// ARMENIAN SMALL LETTER O
		                                '\u0556',		// ARMENIAN CAPITAL LETTER FE
		                                '\u0586',		// ARMENIAN SMALL LETTER FE
                                        /* ARMSCII-8 chars */
  		                                '\u00B2',		// ARMENIAN CAPITAL LETTER AYB
		                                '\u00B3',		// ARMENIAN SMALL LETTER AYB
		                                '\u00B4',		// ARMENIAN CAPITAL LETTER BEN
		                                '\u00B5',		// ARMENIAN SMALL LETTER BEN
		                                '\u00B6',		// ARMENIAN CAPITAL LETTER GIM
		                                '\u00B7',		// ARMENIAN SMALL LETTER GIM
		                                '\u00B8',		// ARMENIAN CAPITAL LETTER DA
		                                '\u00B9',		// ARMENIAN SMALL LETTER DA
		                                '\u00BA',		// ARMENIAN CAPITAL LETTER YECH
		                                '\u00BB',		// ARMENIAN SMALL LETTER YECH
		                                '\u00BC',		// ARMENIAN CAPITAL LETTER ZA
		                                '\u00BD',		// ARMENIAN SMALL LETTER ZA
		                                '\u00BE',		// ARMENIAN CAPITAL LETTER E
		                                '\u00BF',		// ARMENIAN SMALL LETTER E
		                                '\u00C0',		// ARMENIAN CAPITAL LETTER AT
		                                '\u00C1',		// ARMENIAN SMALL LETTER AT
		                                '\u00C2',		// ARMENIAN CAPITAL LETTER TO
		                                '\u00C3',		// ARMENIAN SMALL LETTER TO
		                                '\u00C4',		// ARMENIAN CAPITAL LETTER ZHE
		                                '\u00C5',		// ARMENIAN SMALL LETTER ZHE
		                                '\u00C6',		// ARMENIAN CAPITAL LETTER INI
		                                '\u00C7',		// ARMENIAN SMALL LETTER INI
		                                '\u00C8',		// ARMENIAN CAPITAL LETTER LYUN
		                                '\u00C9',		// ARMENIAN SMALL LETTER LYUN
		                                '\u00CA',		// ARMENIAN CAPITAL LETTER KHE
		                                '\u00CB',		// ARMENIAN SMALL LETTER KHE
		                                '\u00CC',		// ARMENIAN CAPITAL LETTER TSA
		                                '\u00CD',		// ARMENIAN SMALL LETTER TSA
		                                '\u00CE',		// ARMENIAN CAPITAL LETTER KEN
		                                '\u00CF',		// ARMENIAN SMALL LETTER KEN
		                                '\u00D0',		// ARMENIAN CAPITAL LETTER HO
		                                '\u00D1',		// ARMENIAN SMALL LETTER HO
		                                '\u00D2',		// ARMENIAN CAPITAL LETTER DZA
		                                '\u00D3',		// ARMENIAN SMALL LETTER DZA
		                                '\u00D4',		// ARMENIAN CAPITAL LETTER GHAT
		                                '\u00D5',		// ARMENIAN SMALL LETTER GHAT
		                                '\u00D6',		// ARMENIAN CAPITAL LETTER TCHE
		                                '\u00D7',		// ARMENIAN SMALL LETTER TCHE
		                                '\u00D8',		// ARMENIAN CAPITAL LETTER MEN
		                                '\u00D9',		// ARMENIAN SMALL LETTER MEN
		                                '\u00DA',		// ARMENIAN CAPITAL LETTER HI
		                                '\u00DB',		// ARMENIAN SMALL LETTER HI
		                                '\u00DC',		// ARMENIAN CAPITAL LETTER NU
		                                '\u00DD',		// ARMENIAN SMALL LETTER NU
		                                '\u00DE',		// ARMENIAN CAPITAL LETTER SHA
		                                '\u00DF',		// ARMENIAN SMALL LETTER SHA
		                                '\u00E0',		// ARMENIAN CAPITAL LETTER VO
		                                '\u00E1',		// ARMENIAN SMALL LETTER VO
		                                '\u00E2',		// ARMENIAN CAPITAL LETTER CHA
		                                '\u00E3',		// ARMENIAN SMALL LETTER CHA
		                                '\u00E4',		// ARMENIAN CAPITAL LETTER PE
		                                '\u00E5',		// ARMENIAN SMALL LETTER PE
		                                '\u00E6',		// ARMENIAN CAPITAL LETTER JE
		                                '\u00E7',		// ARMENIAN SMALL LETTER JE
		                                '\u00E8',		// ARMENIAN CAPITAL LETTER RA
		                                '\u00E9',		// ARMENIAN SMALL LETTER RA
		                                '\u00EA',		// ARMENIAN CAPITAL LETTER SE
		                                '\u00EB',		// ARMENIAN SMALL LETTER SE
		                                '\u00EC',		// ARMENIAN CAPITAL LETTER VEV
		                                '\u00ED',		// ARMENIAN SMALL LETTER VEV
		                                '\u00EE',		// ARMENIAN CAPITAL LETTER TYUN
		                                '\u00EF',		// ARMENIAN SMALL LETTER TYUN
		                                '\u00F0',		// ARMENIAN CAPITAL LETTER RE
		                                '\u00F1',		// ARMENIAN SMALL LETTER RE
		                                '\u00F2',		// ARMENIAN CAPITAL LETTER TSO
		                                '\u00F3',		// ARMENIAN SMALL LETTER TSO
		                                '\u00F4',		// ARMENIAN CAPITAL LETTER VYUN
		                                '\u00F5',		// ARMENIAN SMALL LETTER VYUN
		                                '\u00F6',		// ARMENIAN CAPITAL LETTER PYUR
		                                '\u00F7',		// ARMENIAN SMALL LETTER PYUR
		                                '\u00F8',		// ARMENIAN CAPITAL LETTER KE
		                                '\u00F9',		// ARMENIAN SMALL LETTER KE
		                                '\u00FA',		// ARMENIAN CAPITAL LETTER O
		                                '\u00FB',		// ARMENIAN SMALL LETTER O
		                                '\u00FC',		// ARMENIAN CAPITAL LETTER FE
		                                '\u00FD',		// ARMENIAN SMALL LETTER FE                                      
                                        /* UNICODE ACCENTS */
                                        '\u055E',		// ARMENIAN QUESTION MARK
                                        '\u055B',       // ARMENIAN EMPHASIS MARK
                                        '\u055A',       // ARMENIAN APOSTROPHE
                                        '\u055C',       // ARMENIAN EXCLAMATION MARK
                                        '\u058A',       // ARMENIAN HYPHEN
                                        '\u2010',       // SOFT HYPHEN
                                        /* ARMSCII-8 ACCENTS */
                                        '\u00B1',		// ARMENIAN QUESTION MARK
                                        '\u00B0',       // ARMENIAN EMPHASIS MARK
                                        '\u00FE',       // ARMENIAN APOSTROPHE
                                        '\u0027',       // APOSTROPHE
                                        '\u00AF',       // ARMENIAN EXCLAMATION MARK
                                        '\u00AD',       // HYPHEN
                                        '\u002D',       // MINUS-HYPHEN
                                        };

        public string[] IsWordExceptionPattern = {
                                        "անվան",
                                        "արոս",
                                        "բոթ",
                                        "դեհ",
                                        "դեն",
                                        "դոն",
                                        "զեն",
                                        "ընդեր",
                                        "թոզ",
                                        "թոթով",
                                        "թոն",
                                        "թոշ",
                                        "լոշ",
                                        "լոռ",
                                        "լորիկ",
                                        "լվա",
                                        "խեչ",
                                        "խոզ",
                                        "խոխ",
                                        "խոյանք",
                                        "խոն",
                                        "ծնոտ",
                                        "ծոփ",
                                        "կարոտ",
                                        "կոշ",
                                        "հագ",
                                        "հաջող",
                                        "հատ",
                                        "հար",
                                        "հետ",
                                        "հեց",
                                        "հեք",
                                        "հոդ",
                                        "հոլով",
                                        "հոռ",
                                        "հորդ",
                                        "հույ",
                                        "հուշ",
                                        "մեկ",
                                        "մեն",
                                        "մղոն",
                                        "մոզ",
                                        "մոռ",
                                        "մոր",
                                        "պետ",
                                        "քարավազ",
                                        "քող",
                                        };
        public HySpellRibbon ribbon;
        public bool SpellOn = false;
        public Dictionary<string, string> WordType = new Dictionary<string, string>()
            {
                {"գ. ած.", "Գոյական / ածական"},
                {"ած. գ.", "Գոյական / ածական"},
                {"ած. մկ.", "Ածական / մակբայ"},
                {"մկ. ած.", "Ածական / մակբայ"},
                {"մկ. շղ.", "Մակբայ / շաղկապ"},
                {"շղ. մկ.", "Մակբայ / շաղկապ"},
                {"մկ. նխ.", "Մակբայ / նախդիր"},
                {"նխ. մկ.", "Մակբայ / նախդիր"},
                {"նրգ. չզ.", "Ներգործական / չեզոք բայ"},
                {"չզ. նրգ.", "Ներգործական / չեզոք բայ"},
                {"ած. դեր.", "Ածական / դերանուն"},
                {"դեր. ած.", "Ածական / դերանուն"},
                {"գ.", "Գոյական"},
                {"ած.", "Ածական"},
                {"նրգ.", "Ներգործական բայ"},
                {"չզ.", "Չեզոք բայ"},
                {"մկ.", "Մակբայ"},
                {"ձյն.", "Ձայնային"},
                {"շղ.", "Շաղկապ"},
                {"դեր.", "Դերանուն"},
                {"նխ.", "Նախդիր"},
                {"օտր.", "Օտար բառ"},
            };

        protected override object RequestService(Guid serviceGuid)
        {
            if (serviceGuid == typeof(Office.IRibbonExtensibility).GUID)
            {
                if (ribbon == null)
                    ribbon = new HySpellRibbon();
                return ribbon;
            }

            return base.RequestService(serviceGuid);
        }

    }

    internal class OfficeWin32Window : IWin32Window
    {
        [DllImport("user32")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        #region IWin32Window Members
        // This holds the window handle for the found Window.
        IntPtr _windowHandle = IntPtr.Zero;

        // The Handle of the Office WindowObject.
        public IntPtr Handle
        {
            get { return _windowHandle; }
        }
        #endregion

        public OfficeWin32Window(object windowObject)
        {
            string caption = windowObject.GetType().InvokeMember("Caption",
                System.Reflection.BindingFlags.GetProperty, null, windowObject, null).ToString();

            Byte[] uCaption = Encoding.ASCII.GetBytes(caption);
            uCaption = Encoding.Convert(Encoding.ASCII, Encoding.Unicode, uCaption);

            char[] uChars = new char[Encoding.Unicode.GetCharCount(uCaption, 0, uCaption.Length)];
            Encoding.Unicode.GetChars(uCaption, 0, uCaption.Length, uChars, 0);
            string sCaption = new string(uChars);

            // try to get the HWND ptr from the windowObject
            //_windowHandle = FindWindow("OpusApp\0", Globals.ThisAddIn.Application.Caption /*sCaption + " - Microsoft Word\0"*/);
        }
    }

    internal class ImageConverter : System.Windows.Forms.AxHost
    {
        private ImageConverter()
            : base(null)
        {
        }

        public static stdole.IPictureDisp Convert(System.Drawing.Image image)
        {
            return (stdole.IPictureDisp)
                AxHost.GetIPictureDispFromPicture(image);
        }
    }

    [ComVisible(true)]
    public class HySpellRibbon : Office.IRibbonExtensibility
    {
        private Office.IRibbonUI ribbon;

        public HySpellRibbon()
        {
        }

        #region IRibbonExtensibility Members

        public string GetCustomUI(string ribbonID)
        {
            string ribbonXML = String.Empty;

            if (ribbonID == "Microsoft.Outlook.Mail.Compose"
                || ribbonID == "Microsoft.Outlook.Appointment"
                || ribbonID == "Microsoft.Outlook.Contact"
                || ribbonID == "Microsoft.Outlook.Task"
                || ribbonID == "Microsoft.Outlook.Journal"
                || ribbonID == "Microsoft.Outlook.Meeting"
                || ribbonID == "Microsoft.Outlook.Post.Compose"
                )
            {
                ribbonXML = GetResourceText("HySpellOL.HySpellRibbon.xml");
            }
            if (ribbonID == "Microsoft.Outlook.Post"
                || ribbonID == "Microsoft.Outlook.Mail.Read")
            {
                ribbonXML = GetResourceText("HySpellOL.HySpellRibbonReadOnly.xml");
            }

            return ribbonXML;
        }
        #endregion

        #region Ribbon Callbacks
        //Create callback methods here. For more information about adding callback methods, select the Ribbon XML item in Solution Explorer and then press F1

        public void Ribbon_Load(Office.IRibbonUI ribbonUI)
        {
            this.ribbon = ribbonUI;
            
        }

        #endregion

        #region Helpers
        public void OnClickHySpell(Office.IRibbonControl control)
        {
            try
            {
                CallSpellChecker(control);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void OnCheckSpelling(Office.IRibbonControl control)
        {
            CallSpellChecker(control);
        }
        private void CallSpellChecker(Office.IRibbonControl control)
        {
            try
            {
                if (Globals.ThisAddIn.HSWrapper == null)
                    Globals.ThisAddIn.InitializeWrapper();

                Globals.ThisAddIn.StartSpellingViaTaskPane();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void OnClickThesaurus(Office.IRibbonControl control)
        {
            Globals.ThisAddIn.ShowTaskPane("");
        }

        public void OnDynamicMenuAction(Office.IRibbonControl control)
        {            
            //this.ribbon.InvalidateControl("MyDynamicMenu");
            string sId = control.Id.Replace("hs_MenuItem", "");
            int nId = Convert.ToInt32(sId);
            Globals.ThisAddIn.LookUpWord(nId, control.Tag);
        }

        public void OnLookUpAction(Office.IRibbonControl control)
        {
            Globals.ThisAddIn.LookUpWord(24);
        }

        public string GetContent(Office.IRibbonControl control)
        {
            return Globals.ThisAddIn.BuildContextMenu(Globals.ThisAddIn.ContextSuggestList);
        }

        public void OnClickOptions(Office.IRibbonControl control)
        {
            frmOptions dlg = new frmOptions();
            dlg.ShowDialog();
        }

        public bool IsEnabled(Office.IRibbonControl button)
        {
            //bool nRet = false;
            //if (!Globals.ThisAddIn.SpellOn)
            //    nRet = true;
            //this.ribbon.InvalidateControl("toggleButton1");
            //this.ribbon.InvalidateControl("btnCheckSpelling");
            //this.ribbon.InvalidateControl("btnThesaurus");
            //this.ribbon.InvalidateControl("btnHySpellOptions");

            //return nRet;
            return true;
        }


        private static string GetResourceText(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resourceNames = asm.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; ++i)
            {
                if (string.Compare(resourceName, resourceNames[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(resourceNames[i])))
                    {
                        if (resourceReader != null)
                        {
                            return resourceReader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }

        public static System.Drawing.Bitmap GetResourceBitmap(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resources = asm.GetManifestResourceNames();
            System.Drawing.Bitmap image = null;

            foreach (string resource in resources)
            {
                if (resource.EndsWith(resourceName))
                {
                    System.IO.Stream stream =
                        asm.GetManifestResourceStream(resource);
                    if (stream == null)
                    {
                        break;
                    }
                    string extension =
                        System.IO.Path.GetExtension(resourceName).ToLower();
                    switch (extension)
                    {
                        case ".ico":
                            image = new System.Drawing.Icon(stream).ToBitmap();
                            break;
                        case ".jpg":
                        case ".bmp":
                        default:
                            image = new System.Drawing.Bitmap(stream);
                            image.MakeTransparent();
                            break;
                    }
                    stream.Close();
                    break;
                }
            }

            return image;
        }

        public stdole.IPictureDisp GetImage(Office.IRibbonControl control)
        {
            stdole.IPictureDisp pictureDisp = null;
            switch (control.Id)
            {
                case "toggleButton1":
                case "btnCheckSpelling":
                    pictureDisp =
                       ImageConverter.Convert(GetResourceBitmap("HySpellIcon.png"));
                    break;
                case "btnThesaurus":
                    pictureDisp =
                       ImageConverter.Convert(GetResourceBitmap("HySpellLookupIcon.png"));
                    break;
                case "mnuConvert":
                    pictureDisp =
                       ImageConverter.Convert(GetResourceBitmap("HySpellConvert.png"));
                    break;

            }
            return pictureDisp;
        }
        #endregion
    }
}
