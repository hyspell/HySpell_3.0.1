// This is the main DLL file.

#include "stdafx.h"
#include <string.h>
#include <Windows.h>
#include "HySpellEncoder.h"
#include <vcclr.h>
#using <system.dll>

#define CUST_DICT_FILE "\\Custom.dic"
using namespace System;
using namespace System::Text;
using namespace Collections;
using namespace System::IO;
using namespace System::Runtime::InteropServices;

namespace HySpellEncoder
{
	static Char  arrUniToAscii [] =
	{
		0xB2,		// ARMENIAN CAPITAL LETTER AYB
		0xB4,		// ARMENIAN CAPITAL LETTER BEN
		0xB6,		// ARMENIAN CAPITAL LETTER GIM
		0xB8,		// ARMENIAN CAPITAL LETTER DA
		0xBA,		// ARMENIAN CAPITAL LETTER ECH
		0xBC,		// ARMENIAN CAPITAL LETTER ZA
		0xBE,		// ARMENIAN CAPITAL LETTER EH
		0xC0,		// ARMENIAN CAPITAL LETTER ET
		0xC2,		// ARMENIAN CAPITAL LETTER TO
		0xC4,		// ARMENIAN CAPITAL LETTER ZHE
		0xC6,		// ARMENIAN CAPITAL LETTER INI
		0xC8,		// ARMENIAN CAPITAL LETTER LIWN
		0xCA,		// ARMENIAN CAPITAL LETTER XEH
		0xCC,		// ARMENIAN CAPITAL LETTER CA
		0xCE,		// ARMENIAN CAPITAL LETTER KEN
		0xD0,		// ARMENIAN CAPITAL LETTER HO
		0xD2,		// ARMENIAN CAPITAL LETTER JA
		0xD4,		// ARMENIAN CAPITAL LETTER GHAD
		0xD6,		// ARMENIAN CAPITAL LETTER CHEH
		0xD8,		// ARMENIAN CAPITAL LETTER MEN
		0xDA,		// ARMENIAN CAPITAL LETTER YI
		0xDC,		// ARMENIAN CAPITAL LETTER NOW
		0xDE,		// ARMENIAN CAPITAL LETTER SHA
		0xE0,		// ARMENIAN CAPITAL LETTER VO
		0xE2,		// ARMENIAN CAPITAL LETTER CHA
		0xE4,		// ARMENIAN CAPITAL LETTER PEH
		0xE6,		// ARMENIAN CAPITAL LETTER JHEH
		0xE8,		// ARMENIAN CAPITAL LETTER RA
		0xEA,		// ARMENIAN CAPITAL LETTER SEH
		0xEC,		// ARMENIAN CAPITAL LETTER VEW
		0xEE,		// ARMENIAN CAPITAL LETTER TIWN
		0xF0,		// ARMENIAN CAPITAL LETTER REH
		0xF2,		// ARMENIAN CAPITAL LETTER CO
		0xF4,		// ARMENIAN CAPITAL LETTER YIWN
		0xF6,		// ARMENIAN CAPITAL LETTER PIWR
		0xF8,		// ARMENIAN CAPITAL LETTER KEH
		0xFA,		// ARMENIAN CAPITAL LETTER OH
		0xFC,		// ARMENIAN CAPITAL LETTER FEH
		0x00,		// This char serves only for the purpose of index position continuity
		0x00,		// This char serves only for the purpose of index position continuity
		0x00,		// This is accent character therefore strip it
		0x00,		// This is accent character therefore strip it
		0x00,		// This is accent character therefore strip it
		0x00,		// This is accent character therefore strip it
		0x00,		// This is punctuation character included for the purpose of index position continuity
		0x00,		// This is accent character therefore strip it
		0x00,		// This is accent character therefore strip it
		0x00,		// This char serves only for the purpose of index position continuity
		0xB3,		// ARMENIAN SMALL LETTER AYB
		0xB5,		// ARMENIAN SMALL LETTER BEN
		0xB7,		// ARMENIAN SMALL LETTER GIM
		0xB9,		// ARMENIAN SMALL LETTER DA
		0xBB,		// ARMENIAN SMALL LETTER ECH
		0xBD,		// ARMENIAN SMALL LETTER ZA
		0xBF,		// ARMENIAN SMALL LETTER EH
		0xC1,		// ARMENIAN SMALL LETTER ET
		0xC3,		// ARMENIAN SMALL LETTER TO
		0xC5,		// ARMENIAN SMALL LETTER ZHE
		0xC7,		// ARMENIAN SMALL LETTER INI
		0xC9,		// ARMENIAN SMALL LETTER LIWN
		0xCB,		// ARMENIAN SMALL LETTER XEH
		0xCD,		// ARMENIAN SMALL LETTER CA
		0xCF,		// ARMENIAN SMALL LETTER KEN
		0xD1,		// ARMENIAN SMALL LETTER HO
		0xD3,		// ARMENIAN SMALL LETTER JA
		0xD5,		// ARMENIAN SMALL LETTER GHAD
		0xD7,		// ARMENIAN SMALL LETTER CHEH
		0xD9,		// ARMENIAN SMALL LETTER MEN
		0xDB,		// ARMENIAN SMALL LETTER YI
		0xDD,		// ARMENIAN SMALL LETTER NOW
		0xDF,		// ARMENIAN SMALL LETTER SHA
		0xE1,		// ARMENIAN SMALL LETTER VO
		0xE3,		// ARMENIAN SMALL LETTER CHA
		0xE5,		// ARMENIAN SMALL LETTER PEH
		0xE7,		// ARMENIAN SMALL LETTER JHEH
		0xE9,		// ARMENIAN SMALL LETTER RA
		0xEB,		// ARMENIAN SMALL LETTER SEH
		0xED,		// ARMENIAN SMALL LETTER VEW
		0xEF,		// ARMENIAN SMALL LETTER TIWN
		0xF1,		// ARMENIAN SMALL LETTER REH
		0xF3,		// ARMENIAN SMALL LETTER CO
		0xF5,		// ARMENIAN SMALL LETTER VYUN
		0xF7,		// ARMENIAN SMALL LETTER PIWR
		0xF9,		// ARMENIAN SMALL LETTER KEH
		0xFB,		// ARMENIAN SMALL LETTER OH
		0xFD,		// ARMENIAN SMALL LETTER FEH
		0xA8, 	    // ARMENIAN SMALL LIGATURE YEV, ARMSCII-8 equivalent is 0xA8
		0x00,		// This char serves only for the purpose of index position continuity
		0x00,		// This is punctuation character included for the purpose of index position continuity
		0x00,		// This is accent character therefore strip it
		0x00,		// 0x058B
		0x00,		// 0x058C
		0x00,		// 0x058D
		0x00,		// 0x058E
		0x00,		// 0x058F
	};
	static Char  arrAsciiToAscii [] =
	{
		0x00, 		// ARMENIAN ETERNITY SIGN
		0xA8, 		// ARMENIAN LIGATURE YEV: standardize to 0xA8
		0x00,		// ARMENIAN FULL STOP: punctuation character, code included for the purpose of index position continuity
		0x00,		// ARMENIAN RIGHT PARENTHESIS: punctuation character, code included for the purpose of index position continuity
		0x00,		// ARMENIAN LEFT PARENTHESIS: punctuation character, code included for the purpose of index position continuity
		0x00,		// ARMENIAN RIGHT QUOTATION MARK: punctuation character, code included for the purpose of index position continuity
		0x00,		// ARMENIAN LEFT QUOTATION MARK: punctuation character, code included for the purpose of index position continuity
		0x00,		// ARMENIAN EM DASH: punctuation character, code included for the purpose of index position continuity
		0x00,		// ARMENIAN DOT: punctuation character, code included for the purpose of index position continuity
		0x00,		// ARMENIAN SEPARATION MARK: punctuation character, code included for the purpose of index position continuity
		0x00,		// ARMENIAN COMMA: punctuation character, code included for the purpose of index position continuity
		0x00,		// ARMENIAN EN DASH: punctuation character, code included for the purpose of index position continuity
		0x00,		// ARMENIAN HYPHEN: accent character therefore strip it
		0x00,		// ARMENIAN ELLIPSIS: accent character therefore strip it
		0x00,		// ARMENIAN EXCLAMATION MARK: accent character therefore strip it
		0x00,		// ARMENIAN ACCENT: accent character therefore strip it
		0x00,		// ARMENIAN QUESTION MARK: accent character therefore strip it
		0xB2,		// ARMENIAN CAPITAL LETTER AYB
		0xB3,		// ARMENIAN SMALL LETTER AYB
		0xB4,		// ARMENIAN CAPITAL LETTER BEN
		0xB5,		// ARMENIAN SMALL LETTER BEN
		0xB6,		// ARMENIAN CAPITAL LETTER GIM
		0xB7,		// ARMENIAN SMALL LETTER GIM
		0xB8,		// ARMENIAN CAPITAL LETTER DA
		0xB9,		// ARMENIAN SMALL LETTER DA
		0xBA,		// ARMENIAN CAPITAL LETTER YECH
		0xBB,		// ARMENIAN SMALL LETTER YECH
		0xBC,		// ARMENIAN CAPITAL LETTER ZA
		0xBD,		// ARMENIAN SMALL LETTER ZA
		0xBE,		// ARMENIAN CAPITAL LETTER E
		0xBF,		// ARMENIAN SMALL LETTER E
		0xC0,		// ARMENIAN CAPITAL LETTER AT
		0xC1,		// ARMENIAN SMALL LETTER AT
		0xC2,		// ARMENIAN CAPITAL LETTER TO
		0xC3,		// ARMENIAN SMALL LETTER TO
		0xC4,		// ARMENIAN CAPITAL LETTER ZHE
		0xC5,		// ARMENIAN SMALL LETTER ZHE
		0xC6,		// ARMENIAN CAPITAL LETTER INI
		0xC7,		// ARMENIAN SMALL LETTER INI
		0xC8,		// ARMENIAN CAPITAL LETTER LYUN
		0xC9,		// ARMENIAN SMALL LETTER LYUN
		0xCA,		// ARMENIAN CAPITAL LETTER KHE
		0xCB,		// ARMENIAN SMALL LETTER KHE
		0xCC,		// ARMENIAN CAPITAL LETTER TSA
		0xCD,		// ARMENIAN SMALL LETTER TSA
		0xCE,		// ARMENIAN CAPITAL LETTER KEN
		0xCF,		// ARMENIAN SMALL LETTER KEN
		0xD0,		// ARMENIAN CAPITAL LETTER HO
		0xD1,		// ARMENIAN SMALL LETTER HO
		0xD2,		// ARMENIAN CAPITAL LETTER DZA
		0xD3,		// ARMENIAN SMALL LETTER DZA
		0xD4,		// ARMENIAN CAPITAL LETTER GHAT
		0xD5,		// ARMENIAN SMALL LETTER GHAT
		0xD6,		// ARMENIAN CAPITAL LETTER TCHE
		0xD7,		// ARMENIAN SMALL LETTER TCHE
		0xD8,		// ARMENIAN CAPITAL LETTER MEN
		0xD9,		// ARMENIAN SMALL LETTER MEN
		0xDA,		// ARMENIAN CAPITAL LETTER HI
		0xDB,		// ARMENIAN SMALL LETTER HI
		0xDC,		// ARMENIAN CAPITAL LETTER NU
		0xDD,		// ARMENIAN SMALL LETTER NU
		0xDE,		// ARMENIAN CAPITAL LETTER SHA
		0xDF,		// ARMENIAN SMALL LETTER SHA
		0xE0,		// ARMENIAN CAPITAL LETTER VO
		0xE1,		// ARMENIAN SMALL LETTER VO
		0xE2,		// ARMENIAN CAPITAL LETTER CHA
		0xE3,		// ARMENIAN SMALL LETTER CHA
		0xE4,		// ARMENIAN CAPITAL LETTER PE
		0xE5,		// ARMENIAN SMALL LETTER PE
		0xE6,		// ARMENIAN CAPITAL LETTER JE
		0xE7,		// ARMENIAN SMALL LETTER JE
		0xE8,		// ARMENIAN CAPITAL LETTER RA
		0xE9,		// ARMENIAN SMALL LETTER RA
		0xEA,		// ARMENIAN CAPITAL LETTER SE
		0xEB,		// ARMENIAN SMALL LETTER SE
		0xEC,		// ARMENIAN CAPITAL LETTER VEV
		0xED,		// ARMENIAN SMALL LETTER VEV
		0xEE,		// ARMENIAN CAPITAL LETTER TYUN
		0xEF,		// ARMENIAN SMALL LETTER TYUN
		0xF0,		// ARMENIAN CAPITAL LETTER RE
		0xF1,		// ARMENIAN SMALL LETTER RE
		0xF2,		// ARMENIAN CAPITAL LETTER TSO
		0xF3,		// ARMENIAN SMALL LETTER TSO
		0xF4,		// ARMENIAN CAPITAL LETTER VYUN
		0xF5,		// ARMENIAN SMALL LETTER VYUN
		0xF6,		// ARMENIAN CAPITAL LETTER PYUR
		0xF7,		// ARMENIAN SMALL LETTER PYUR
		0xF8,		// ARMENIAN CAPITAL LETTER KE
		0xF9,		// ARMENIAN SMALL LETTER KE
		0xFA,		// ARMENIAN CAPITAL LETTER O
		0xFB,		// ARMENIAN SMALL LETTER O
		0xFC,		// ARMENIAN CAPITAL LETTER FE
		0xFD,		// ARMENIAN SMALL LETTER FE
		0x00,		// ARMENIAN APOSTROPHE: accent character therefore strip it
	};
	static Char  arrAsciiToUni [] =
	{
		0x0531,		// ARMENIAN CAPITAL LETTER AYB
		0x0561,		// ARMENIAN SMALL LETTER AYB
		0x0532,		// ARMENIAN CAPITAL LETTER BEN
		0x0562,		// ARMENIAN SMALL LETTER BEN
		0x0533,		// ARMENIAN CAPITAL LETTER GIM
		0x0563,		// ARMENIAN SMALL LETTER GIM
		0x0534,		// ARMENIAN CAPITAL LETTER DA
		0x0564,		// ARMENIAN SMALL LETTER DA
		0x0535,		// ARMENIAN CAPITAL LETTER YECH
		0x0565,		// ARMENIAN SMALL LETTER YECH
		0x0536,		// ARMENIAN CAPITAL LETTER ZA
		0x0566,		// ARMENIAN SMALL LETTER ZA
		0x0537,		// ARMENIAN CAPITAL LETTER E
		0x0567,		// ARMENIAN SMALL LETTER E
		0x0538,		// ARMENIAN CAPITAL LETTER AT
		0x0568,		// ARMENIAN SMALL LETTER AT
		0x0539,		// ARMENIAN CAPITAL LETTER TO
		0x0569,		// ARMENIAN SMALL LETTER TO
		0x053A,		// ARMENIAN CAPITAL LETTER ZHE
		0x056A,		// ARMENIAN SMALL LETTER ZHE
		0x053B,		// ARMENIAN CAPITAL LETTER INI
		0x056B,		// ARMENIAN SMALL LETTER INI
		0x053C,		// ARMENIAN CAPITAL LETTER LYUN
		0x056C,		// ARMENIAN SMALL LETTER LYUN
		0x053D,		// ARMENIAN CAPITAL LETTER KHE
		0x056D,		// ARMENIAN SMALL LETTER KHE
		0x053E,		// ARMENIAN CAPITAL LETTER TSA
		0x056E,		// ARMENIAN SMALL LETTER TSA
		0x053F,		// ARMENIAN CAPITAL LETTER KEN
		0x056F,		// ARMENIAN SMALL LETTER KEN
		0x0540,		// ARMENIAN CAPITAL LETTER HO
		0x0570,		// ARMENIAN SMALL LETTER HO
		0x0541,		// ARMENIAN CAPITAL LETTER DZA
		0x0571,		// ARMENIAN SMALL LETTER DZA
		0x0542,		// ARMENIAN CAPITAL LETTER GHAT
		0x0572,		// ARMENIAN SMALL LETTER GHAT
		0x0543,		// ARMENIAN CAPITAL LETTER TCHE
		0x0573,		// ARMENIAN SMALL LETTER TCHE
		0x0544,		// ARMENIAN CAPITAL LETTER MEN
		0x0574,		// ARMENIAN SMALL LETTER MEN
		0x0545,		// ARMENIAN CAPITAL LETTER HI
		0x0575,		// ARMENIAN SMALL LETTER HI
		0x0546,		// ARMENIAN CAPITAL LETTER NU
		0x0576,		// ARMENIAN SMALL LETTER NU
		0x0547,		// ARMENIAN CAPITAL LETTER SHA
		0x0577,		// ARMENIAN SMALL LETTER SHA
		0x0548,		// ARMENIAN CAPITAL LETTER VO
		0x0578,		// ARMENIAN SMALL LETTER VO
		0x0549,		// ARMENIAN CAPITAL LETTER CHA
		0x0579,		// ARMENIAN SMALL LETTER CHA
		0x054A,		// ARMENIAN CAPITAL LETTER PE
		0x057A,		// ARMENIAN SMALL LETTER PE
		0x054B,		// ARMENIAN CAPITAL LETTER JE
		0x057B,		// ARMENIAN SMALL LETTER JE
		0x054C,		// ARMENIAN CAPITAL LETTER RA
		0x057C,		// ARMENIAN SMALL LETTER RA
		0x054D,		// ARMENIAN CAPITAL LETTER SE
		0x057D,		// ARMENIAN SMALL LETTER SE
		0x054E,		// ARMENIAN CAPITAL LETTER VEV
		0x057E,		// ARMENIAN SMALL LETTER VEV
		0x054F,		// ARMENIAN CAPITAL LETTER TYUN
		0x057F,		// ARMENIAN SMALL LETTER TYUN
		0x0550,		// ARMENIAN CAPITAL LETTER RE
		0x0580,		// ARMENIAN SMALL LETTER RE
		0x0551,		// ARMENIAN CAPITAL LETTER TSO
		0x0581,		// ARMENIAN SMALL LETTER TSO
		0x0552,		// ARMENIAN CAPITAL LETTER VYUN
		0x0582,		// ARMENIAN SMALL LETTER VYUN
		0x0553,		// ARMENIAN CAPITAL LETTER PYUR
		0x0583,		// ARMENIAN SMALL LETTER PYUR
		0x0554,		// ARMENIAN CAPITAL LETTER KE
		0x0584,		// ARMENIAN SMALL LETTER KE
		0x0555,		// ARMENIAN CAPITAL LETTER O
		0x0585,		// ARMENIAN SMALL LETTER O
		0x0556,		// ARMENIAN CAPITAL LETTER FE
		0x0586,		// ARMENIAN SMALL LETTER FE
	};
	static Char  arrAsciiToUni_U [] =
	{
		0x0000, 	// ARMENIAN ETERNITY SIGN
		0x0565, 	// (+ 0x0582) ARMENIAN LIGATURE EW: un-ligature it to YECH VYUN
		0x0000,		// ARMENIAN FULL STOP: punctuation character, code included for the purpose of index position continuity
		0x0000,		// ARMENIAN RIGHT PARENTHESIS: punctuation character, code included for the purpose of index position continuity
		0x0000,		// ARMENIAN LEFT PARENTHESIS: punctuation character, code included for the purpose of index position continuity
		0x0000,		// ARMENIAN RIGHT QUOTATION MARK: punctuation character, code included for the purpose of index position continuity
		0x0000,		// ARMENIAN LEFT QUOTATION MARK: punctuation character, code included for the purpose of index position continuity
		0x0565, 	// (+ 0x0582) ARMENIAN LIGATURE EW: un-ligature it to YECH VYUN (place code name is DIAERESIS)		
		0x0000,		// ARMENIAN DOT: punctuation character, code included for the purpose of index position continuity
		0x0000,		// ARMENIAN SEPARATION MARK: punctuation character, code included for the purpose of index position continuity
		0x0000,		// ARMENIAN COMMA: punctuation character, code included for the purpose of index position continuity
		0x0000,		// ARMENIAN EN DASH: punctuation character, code included for the purpose of index position continuity
		0x0000,		// ARMENIAN HYPHEN: accent character therefore strip it
		0x0000,		// ARMENIAN ELLIPSIS: accent character therefore strip it
		0x0000,		// ARMENIAN EXCLAMATION MARK: accent character therefore strip it
		0x0000,		// ARMENIAN ACCENT: accent character therefore strip it
		0x0000,		// ARMENIAN QUESTION MARK: accent character therefore strip it
		0x0531,		// ARMENIAN CAPITAL LETTER AYB
		0x0561,		// ARMENIAN SMALL LETTER AYB
		0x0532,		// ARMENIAN CAPITAL LETTER BEN
		0x0562,		// ARMENIAN SMALL LETTER BEN
		0x0533,		// ARMENIAN CAPITAL LETTER GIM
		0x0563,		// ARMENIAN SMALL LETTER GIM
		0x0534,		// ARMENIAN CAPITAL LETTER DA
		0x0564,		// ARMENIAN SMALL LETTER DA
		0x0535,		// ARMENIAN CAPITAL LETTER YECH
		0x0565,		// ARMENIAN SMALL LETTER YECH
		0x0536,		// ARMENIAN CAPITAL LETTER ZA
		0x0566,		// ARMENIAN SMALL LETTER ZA
		0x0537,		// ARMENIAN CAPITAL LETTER E
		0x0567,		// ARMENIAN SMALL LETTER E
		0x0538,		// ARMENIAN CAPITAL LETTER AT
		0x0568,		// ARMENIAN SMALL LETTER AT
		0x0539,		// ARMENIAN CAPITAL LETTER TO
		0x0569,		// ARMENIAN SMALL LETTER TO
		0x053A,		// ARMENIAN CAPITAL LETTER ZHE
		0x056A,		// ARMENIAN SMALL LETTER ZHE
		0x053B,		// ARMENIAN CAPITAL LETTER INI
		0x056B,		// ARMENIAN SMALL LETTER INI
		0x053C,		// ARMENIAN CAPITAL LETTER LYUN
		0x056C,		// ARMENIAN SMALL LETTER LYUN
		0x053D,		// ARMENIAN CAPITAL LETTER KHE
		0x056D,		// ARMENIAN SMALL LETTER KHE
		0x053E,		// ARMENIAN CAPITAL LETTER TSA
		0x056E,		// ARMENIAN SMALL LETTER TSA
		0x053F,		// ARMENIAN CAPITAL LETTER KEN
		0x056F,		// ARMENIAN SMALL LETTER KEN
		0x0540,		// ARMENIAN CAPITAL LETTER HO
		0x0570,		// ARMENIAN SMALL LETTER HO
		0x0541,		// ARMENIAN CAPITAL LETTER DZA
		0x0571,		// ARMENIAN SMALL LETTER DZA
		0x0542,		// ARMENIAN CAPITAL LETTER GHAT
		0x0572,		// ARMENIAN SMALL LETTER GHAT
		0x0543,		// ARMENIAN CAPITAL LETTER TCHE
		0x0573,		// ARMENIAN SMALL LETTER TCHE
		0x0544,		// ARMENIAN CAPITAL LETTER MEN
		0x0574,		// ARMENIAN SMALL LETTER MEN
		0x0545,		// ARMENIAN CAPITAL LETTER HI
		0x0575,		// ARMENIAN SMALL LETTER HI
		0x0546,		// ARMENIAN CAPITAL LETTER NU
		0x0576,		// ARMENIAN SMALL LETTER NU
		0x0547,		// ARMENIAN CAPITAL LETTER SHA
		0x0577,		// ARMENIAN SMALL LETTER SHA
		0x0548,		// ARMENIAN CAPITAL LETTER VO
		0x0578,		// ARMENIAN SMALL LETTER VO
		0x0549,		// ARMENIAN CAPITAL LETTER CHA
		0x0579,		// ARMENIAN SMALL LETTER CHA
		0x054A,		// ARMENIAN CAPITAL LETTER PE
		0x057A,		// ARMENIAN SMALL LETTER PE
		0x054B,		// ARMENIAN CAPITAL LETTER JE
		0x057B,		// ARMENIAN SMALL LETTER JE
		0x054C,		// ARMENIAN CAPITAL LETTER RA
		0x057C,		// ARMENIAN SMALL LETTER RA
		0x054D,		// ARMENIAN CAPITAL LETTER SE
		0x057D,		// ARMENIAN SMALL LETTER SE
		0x054E,		// ARMENIAN CAPITAL LETTER VEV
		0x057E,		// ARMENIAN SMALL LETTER VEV
		0x054F,		// ARMENIAN CAPITAL LETTER TYUN
		0x057F,		// ARMENIAN SMALL LETTER TYUN
		0x0550,		// ARMENIAN CAPITAL LETTER RE
		0x0580,		// ARMENIAN SMALL LETTER RE
		0x0551,		// ARMENIAN CAPITAL LETTER TSO
		0x0581,		// ARMENIAN SMALL LETTER TSO
		0x0552,		// ARMENIAN CAPITAL LETTER VYUN
		0x0582,		// ARMENIAN SMALL LETTER VYUN
		0x0553,		// ARMENIAN CAPITAL LETTER PYUR
		0x0583,		// ARMENIAN SMALL LETTER PYUR
		0x0554,		// ARMENIAN CAPITAL LETTER KE
		0x0584,		// ARMENIAN SMALL LETTER KE
		0x0555,		// ARMENIAN CAPITAL LETTER O
		0x0585,		// ARMENIAN SMALL LETTER O
		0x0556,		// ARMENIAN CAPITAL LETTER FE
		0x0586,		// ARMENIAN SMALL LETTER FE
		0x0000,		// ARMENIAN APOSTROPHE: accent character therefore strip it
	};
	static Char  arrArasanToUni [] =
	{
		// code-offset 21
		0x0031,		// digit 1
		0x0553,		// ARMENIAN CAPITAL LETTER PYUR
		0x0545,		// ARMENIAN CAPITAL LETTER HI
		0x0034,		// digit 4
		0x0035,		// digit 5
		0x0037,		// digit 7
		0x0583,		// ARMENIAN SMALL LETTER PYUR
		0x0039,		// digit 9
		0x0555,		// ARMENIAN CAPITAL LETTER O
		0x0038,		// digit 8
		0x053A,		// ARMENIAN CAPITAL LETTER ZHE
		0x0577,		// ARMENIAN SMALL LETTER SHA
		0x057C,		// ARMENIAN SMALL LETTER RA
		0x0572,		// ARMENIAN SMALL LETTER GHAT
		0x056E,		// ARMENIAN SMALL LETTER TSA
		0x0585,		// ARMENIAN SMALL LETTER O
		0x0589,		// ARMENIAN FULL STOP
		0x0571,		// ARMENIAN SMALL LETTER DZA
		0x0575,		// ARMENIAN SMALL LETTER HI
		0x055B,		// ARMENIAN EMPHASIS MARK
		0x002C,		// COMMA
		0x2013,		// EN DASH
		0x002E,		// FULL STOP
		0x00AB,		// LEFT POINTING DOUBLE ANGLE QUOT
		0x00BB,		// RIGHT POINTING DOUBLE ANGLE QUOT
		0x0539,		// ARMENIAN CAPITAL LETTER TO
		0x0569,		// ARMENIAN SMALL LETTER TO
		0x0547,		// ARMENIAN CAPITAL LETTER SHA
		0x056A,		// ARMENIAN SMALL LETTER ZHE
		0x0542,		// ARMENIAN CAPITAL LETTER GHAT
		0x053E,		// ARMENIAN CAPITAL LETTER TSA
		0x0541,		// ARMENIAN CAPITAL LETTER DZA
		0x0531,		// ARMENIAN CAPITAL LETTER AYB
		0x054A,		// ARMENIAN CAPITAL LETTER PE
		0x0533,		// ARMENIAN CAPITAL LETTER GIM
		0x054F,		// ARMENIAN CAPITAL LETTER TYUN
		0x0537,		// ARMENIAN CAPITAL LETTER E
		0x0556,		// ARMENIAN CAPITAL LETTER FE
		0x053F,		// ARMENIAN CAPITAL LETTER KEN
		0x0540,		// ARMENIAN CAPITAL LETTER HO
		0x053B,		// ARMENIAN CAPITAL LETTER INI
		0x0543,		// ARMENIAN CAPITAL LETTER TCHE
		0x0554,		// ARMENIAN CAPITAL LETTER KE
		0x053C,		// ARMENIAN CAPITAL LETTER LYUN
		0x0544,		// ARMENIAN CAPITAL LETTER MEN
		0x0546,		// ARMENIAN CAPITAL LETTER NU
		0x0548,		// ARMENIAN CAPITAL LETTER VO
		0x0532,		// ARMENIAN CAPITAL LETTER BEN
		0x053D,		// ARMENIAN CAPITAL LETTER KHE
		0x0550,		// ARMENIAN CAPITAL LETTER RE
		0x054D,		// ARMENIAN CAPITAL LETTER SE
		0x0534,		// ARMENIAN CAPITAL LETTER DA
		0x0538,		// ARMENIAN CAPITAL LETTER AT
		0x0552,		// ARMENIAN CAPITAL LETTER VYUN
		0x054E,		// ARMENIAN CAPITAL LETTER VEV
		0x0551,		// ARMENIAN CAPITAL LETTER TSO
		0x0535,		// ARMENIAN CAPITAL LETTER YECH
		0x0536,		// ARMENIAN CAPITAL LETTER ZA
		0x0579,		// ARMENIAN SMALL LETTER CHA
		0x055A,		// ARMENIAN APOSTROPHE
		0x057B,		// ARMENIAN SMALL LETTER JE
		0x0036,		// digit 6
		0x054C,		// ARMENIAN CAPITAL LETTER RA
		0x055D,		// ARMENIAN COMMA
		0x0561,		// ARMENIAN SMALL LETTER AYB
		0x057A,		// ARMENIAN SMALL LETTER PE
		0x0563,		// ARMENIAN SMALL LETTER GIM
		0x057F,		// ARMENIAN SMALL LETTER TYUN
		0x0567,		// ARMENIAN SMALL LETTER E
		0x0586,		// ARMENIAN SMALL LETTER FE
		0x056F,		// ARMENIAN SMALL LETTER KEN
		0x0570,		// ARMENIAN SMALL LETTER HO
		0x056B,		// ARMENIAN SMALL LETTER INI
		0x0573,		// ARMENIAN SMALL LETTER TCHE
		0x0584,		// ARMENIAN SMALL LETTER KE
		0x056C,		// ARMENIAN SMALL LETTER LYUN
		0x0574,		// ARMENIAN SMALL LETTER MEN
		0x0576,		// ARMENIAN SMALL LETTER NU
		0x0578,		// ARMENIAN SMALL LETTER VO
		0x0562,		// ARMENIAN SMALL LETTER BEN
		0x056D,		// ARMENIAN SMALL LETTER KHE
		0x0580,		// ARMENIAN SMALL LETTER RE
		0x057D,		// ARMENIAN SMALL LETTER SE
		0x0564,		// ARMENIAN SMALL LETTER DA
		0x0568,		// ARMENIAN SMALL LETTER AT
		0x0582,		// ARMENIAN SMALL LETTER VYUN
		0x057E,		// ARMENIAN SMALL LETTER VEV
		0x0581,		// ARMENIAN SMALL LETTER TSO
		0x0565,		// ARMENIAN SMALL LETTER YECH
		0x0566,		// ARMENIAN SMALL LETTER ZA
		0x0549,		// ARMENIAN CAPITAL LETTER CHA
		0x055E,		// ARMENIAN QUESTION MARK
		0x054B,		// ARMENIAN CAPITAL LETTER JE
		0x055C,		// ARMENIAN EXCLAMATION MARK
		// code-offset 91______5E
		0x0583,		// ARMENIAN SMALL LETTER PYUR
		0x0092,		// (none)
		0x0093,		// (none)
		0x0553,		// ARMENIAN CAPITAL LETTER PYUR
		// code-offset A3______62
		0x0033,		// DIGIT 3
		0x2014,		// DIGIT EM-DASH
		0x0028,		// LEFT PARENTHESIS
		0x0587,		// ARMENIAN SMALL LIGATURE YEV
		0x00A7,		// (none)
		0x057C,		// ARMENIAN SMALL LETTER RA
		// code-offset AA______68
		0x0032,		// DIGIT 2
		// code-offset B3______69
		0x2026,		// HORIZONTAL ELLIPSIS
		// code-offset BB______6A
		0x0029,		// RIGHT PARENTHESIS
		0x0030,		// DIGIT 0
		0x056E,		// ARMENIAN SMALL LETTER TSA
		// code-offset D0______6D
		0x002D,		// Hyphen-Minus
		0x00D1,		// (none)
		0x0553,		// ARMENIAN CAPITAL LETTER PYUR
		0x0553,		// ARMENIAN CAPITAL LETTER PYUR
		0x0583,		// ARMENIAN SMALL LETTER PYUR
		0x0583,		// ARMENIAN SMALL LETTER PYUR
		// code-offset 2010______73
		0x057C,		// ARMENIAN SMALL LETTER RA
		// code-offset 2018______74
		0x0583,		// ARMENIAN SMALL LETTER PYUR
		0x0583,		// ARMENIAN SMALL LETTER PYUR
		// code-offset 201C______76
		0x0553,		// ARMENIAN CAPITAL LETTER PYUR
		0x0553,		// ARMENIAN CAPITAL LETTER PYUR
	};

	static Char  arrAsciiToUni_KeepAccents [] =
	{
		0x0000, 	// ARMENIAN ETERNITY SIGN
		0x0565, 	// (+ 0x0582) ARMENIAN LIGATURE EW: un-ligature it to YECH VYUN
		0x0589,		// ARMENIAN FULL STOP: punctuation character
		0x0029,		// ARMENIAN RIGHT PARENTHESIS: punctuation character
		0x0028,		// ARMENIAN LEFT PARENTHESIS: punctuation character
		0x00BB,		// ARMENIAN RIGHT QUOTATION MARK: punctuation character
		0x00AB,		// ARMENIAN LEFT QUOTATION MARK: punctuation character
		0x0565, 	// (+ 0x0582) ARMENIAN LIGATURE EW: un-ligature it to YECH VYUN (place code name is DIAERESIS)		
		0x002E,		// ARMENIAN DOT: punctuation character
		0x2014,		// ARMENIAN SEPARATION MARK: punctuation character
		0x055D,		// ARMENIAN COMMA: punctuation character
		0x2013,		// ARMENIAN EN DASH: punctuation character
		0x058A,		// ARMENIAN HYPHEN: accent character therefore strip it
		0x2026,		// ARMENIAN ELLIPSIS: accent character therefore strip it
		0x055C,		// ARMENIAN EXCLAMATION MARK: accent character therefore strip it
		0x055B,		// ARMENIAN ACCENT: accent character therefore strip it
		0x055E,		// ARMENIAN QUESTION MARK: accent character therefore strip it
		0x0531,		// ARMENIAN CAPITAL LETTER AYB
		0x0561,		// ARMENIAN SMALL LETTER AYB
		0x0532,		// ARMENIAN CAPITAL LETTER BEN
		0x0562,		// ARMENIAN SMALL LETTER BEN
		0x0533,		// ARMENIAN CAPITAL LETTER GIM
		0x0563,		// ARMENIAN SMALL LETTER GIM
		0x0534,		// ARMENIAN CAPITAL LETTER DA
		0x0564,		// ARMENIAN SMALL LETTER DA
		0x0535,		// ARMENIAN CAPITAL LETTER YECH
		0x0565,		// ARMENIAN SMALL LETTER YECH
		0x0536,		// ARMENIAN CAPITAL LETTER ZA
		0x0566,		// ARMENIAN SMALL LETTER ZA
		0x0537,		// ARMENIAN CAPITAL LETTER E
		0x0567,		// ARMENIAN SMALL LETTER E
		0x0538,		// ARMENIAN CAPITAL LETTER AT
		0x0568,		// ARMENIAN SMALL LETTER AT
		0x0539,		// ARMENIAN CAPITAL LETTER TO
		0x0569,		// ARMENIAN SMALL LETTER TO
		0x053A,		// ARMENIAN CAPITAL LETTER ZHE
		0x056A,		// ARMENIAN SMALL LETTER ZHE
		0x053B,		// ARMENIAN CAPITAL LETTER INI
		0x056B,		// ARMENIAN SMALL LETTER INI
		0x053C,		// ARMENIAN CAPITAL LETTER LYUN
		0x056C,		// ARMENIAN SMALL LETTER LYUN
		0x053D,		// ARMENIAN CAPITAL LETTER KHE
		0x056D,		// ARMENIAN SMALL LETTER KHE
		0x053E,		// ARMENIAN CAPITAL LETTER TSA
		0x056E,		// ARMENIAN SMALL LETTER TSA
		0x053F,		// ARMENIAN CAPITAL LETTER KEN
		0x056F,		// ARMENIAN SMALL LETTER KEN
		0x0540,		// ARMENIAN CAPITAL LETTER HO
		0x0570,		// ARMENIAN SMALL LETTER HO
		0x0541,		// ARMENIAN CAPITAL LETTER DZA
		0x0571,		// ARMENIAN SMALL LETTER DZA
		0x0542,		// ARMENIAN CAPITAL LETTER GHAT
		0x0572,		// ARMENIAN SMALL LETTER GHAT
		0x0543,		// ARMENIAN CAPITAL LETTER TCHE
		0x0573,		// ARMENIAN SMALL LETTER TCHE
		0x0544,		// ARMENIAN CAPITAL LETTER MEN
		0x0574,		// ARMENIAN SMALL LETTER MEN
		0x0545,		// ARMENIAN CAPITAL LETTER HI
		0x0575,		// ARMENIAN SMALL LETTER HI
		0x0546,		// ARMENIAN CAPITAL LETTER NU
		0x0576,		// ARMENIAN SMALL LETTER NU
		0x0547,		// ARMENIAN CAPITAL LETTER SHA
		0x0577,		// ARMENIAN SMALL LETTER SHA
		0x0548,		// ARMENIAN CAPITAL LETTER VO
		0x0578,		// ARMENIAN SMALL LETTER VO
		0x0549,		// ARMENIAN CAPITAL LETTER CHA
		0x0579,		// ARMENIAN SMALL LETTER CHA
		0x054A,		// ARMENIAN CAPITAL LETTER PE
		0x057A,		// ARMENIAN SMALL LETTER PE
		0x054B,		// ARMENIAN CAPITAL LETTER JE
		0x057B,		// ARMENIAN SMALL LETTER JE
		0x054C,		// ARMENIAN CAPITAL LETTER RA
		0x057C,		// ARMENIAN SMALL LETTER RA
		0x054D,		// ARMENIAN CAPITAL LETTER SE
		0x057D,		// ARMENIAN SMALL LETTER SE
		0x054E,		// ARMENIAN CAPITAL LETTER VEV
		0x057E,		// ARMENIAN SMALL LETTER VEV
		0x054F,		// ARMENIAN CAPITAL LETTER TYUN
		0x057F,		// ARMENIAN SMALL LETTER TYUN
		0x0550,		// ARMENIAN CAPITAL LETTER RE
		0x0580,		// ARMENIAN SMALL LETTER RE
		0x0551,		// ARMENIAN CAPITAL LETTER TSO
		0x0581,		// ARMENIAN SMALL LETTER TSO
		0x0552,		// ARMENIAN CAPITAL LETTER VYUN
		0x0582,		// ARMENIAN SMALL LETTER VYUN
		0x0553,		// ARMENIAN CAPITAL LETTER PYUR
		0x0583,		// ARMENIAN SMALL LETTER PYUR
		0x0554,		// ARMENIAN CAPITAL LETTER KE
		0x0584,		// ARMENIAN SMALL LETTER KE
		0x0555,		// ARMENIAN CAPITAL LETTER O
		0x0585,		// ARMENIAN SMALL LETTER O
		0x0556,		// ARMENIAN CAPITAL LETTER FE
		0x0586,		// ARMENIAN SMALL LETTER FE
		0x055A,		// ARMENIAN APOSTROPHE: accent character therefore strip it
	};

	Wrapper::Wrapper()
	{
	}

	Wrapper::~Wrapper()
	{
	}

	int Wrapper::Encode(String^ sInput, ArrayList^ lstEncoded)
	{
		array<Char>^ chInput = sInput->ToCharArray(); 
		if(/*chInput == null ||*/ chInput->Length == 0)
			return 0;
		if((chInput[0] > 0x530 && chInput[0] < 0x590)
			|| (chInput[0] > 0xFB12 && chInput[0] < 0xFB18))
			return EncodeUNIString(sInput, lstEncoded);
		else if(chInput[0] > 0xA0 && chInput[0] < 0xFF)
			return EncodeASCIIString(sInput, lstEncoded);
		else
			return 0;
	}
	int Wrapper::EncodeUNIString(String^ sInput, ArrayList^ lstEncoded)
	{
		int nRetVal = 2;
		array<Char>^ chInput = sInput->ToCharArray(); 
		StringBuilder^ sb = gcnew StringBuilder();

		for(int i= 0; i < chInput->Length; i++)
		{
			if(i == chInput->Length - 1 && chInput[i] == 0xBB)
				nRetVal = 3;
			else
			{
				if(chInput[i] == 0x2D || chInput[i] == 0x58A) // dash character or hy_hyphen
				{
					// dash character is used for Armenian hyphenation
					continue;
				}
				else if(chInput[i] == 0xFB13) // ARMENIAN SMALL LIGATURE MEN NOW:
				{
					// un-ligature it to MEN NOW
					sb->Append(Convert::ToChar(0xD9));
					sb->Append(Convert::ToChar(0xDD));
					continue;
				}
				else if(chInput[i] == 0xFB14) // ARMENIAN SMALL LIGATURE MEN ECH:
				{
					// un-ligature it to MEN ECH
					sb->Append(Convert::ToChar(0xD9));
					sb->Append(Convert::ToChar(0xBB));
					continue;
				}
				else if(chInput[i] == 0xFB15) // ARMENIAN SMALL LIGATURE MEN INI:
				{
					// un-ligature it to MEN INI
					sb->Append(Convert::ToChar(0xD9));
					sb->Append(Convert::ToChar(0xC7));
					continue;
				}
				else if(chInput[i] == 0xFB16) // ARMENIAN SMALL LIGATURE VEW NOW:
				{
					// un-ligature it to VEW NOW
					sb->Append(Convert::ToChar(0xED));
					sb->Append(Convert::ToChar(0xDD));
					continue;
				}
				else if(chInput[i] == 0xFB17) // ARMENIAN SMALL LIGATURE MEN XEH:
				{
					// un-ligature it to MEN XEH
					sb->Append(Convert::ToChar(0xD9));
					sb->Append(Convert::ToChar(0xCB));
					continue;
				}
				else
				{
					if(chInput[i] < 0x531 || chInput[i] > 0x58F)
					  return 0;
					Char enCh = arrUniToAscii[chInput[i] - 0x0531];
					if(enCh > 0)
						sb->Append(enCh);
// word with YEV will be included in the dic-file, therefore no need to separate into YECH VYUN
					//if(chInput[i] == 0x587) // ARMENIAN SMALL LIGATURE YEV
					//	sb->Append(Convert::ToChar(0xF5)); // un-ligature it to YECH VYUN
				}
			}
		}
		String^ sEncoded = gcnew String(sb->ToString());
		lstEncoded->Add(sEncoded);
		return nRetVal;
	}
	int Wrapper::EncodeASCIIString(String^ sInput, ArrayList^ lstEncoded)
	{
		int nRetVal = 1;
		array<Char>^ chInput = sInput->ToCharArray(); 
		StringBuilder^ sb = gcnew StringBuilder();

		for(int i= 0; i < chInput->Length; i++)
		{
			if(chInput[i] == 0x2D || chInput[i] == 0x58A) // dash character or hy_hyphen
			{
				// dash character is used for Armenian hyphenation
				continue;
			}
			else
			{
				if(chInput[i] < 0xA1 || chInput[i] > 0xFE)
				  return 0;
				Char enCh = arrAsciiToAscii[chInput[i] - 0xA1];
				if(enCh > 0)
					sb->Append(enCh);
// word with YEV will be included in the dic-file, therefore no need to separate into YECH VYUN
				//if(chInput[i] == 0xA2) // ARMENIAN SMALL LIGATURE YEV
				//	sb->Append(Convert::ToChar(0xF5)); // un-ligature it to YECH VYUN
			}
		}
		String^ sEncoded = gcnew String(sb->ToString());
		lstEncoded->Add(sEncoded);
		return nRetVal;
	}
	int Wrapper::EncodeUNIString_U(String^ sInput, ArrayList^ lstEncoded)
	{
		int nRetVal = 2;
		array<Char>^ chInput = sInput->ToCharArray(); 
		StringBuilder^ sb = gcnew StringBuilder();

		for(int i= 0; i < chInput->Length; i++)
		{
			if(i == chInput->Length - 1 && chInput[i] == 0xBB)
				nRetVal = 3;
			else
			{
				if(chInput[i] == 0x2D || chInput[i] == 0x58A) // dash character or hy_hyphen
				{
					// dash character is used for Armenian hyphenation
					continue;
				}
				else if(chInput[i] == 0xFB13) // ARMENIAN SMALL LIGATURE MEN NOW:
				{
					// un-ligature it to MEN NU
					sb->Append(Convert::ToChar(0x0574));
					sb->Append(Convert::ToChar(0x0576));
					continue;
				}
				else if(chInput[i] == 0xFB14) // ARMENIAN SMALL LIGATURE MEN ECH:
				{
					// un-ligature it to MEN ECH
					sb->Append(Convert::ToChar(0x0574));
					sb->Append(Convert::ToChar(0x0565));
					continue;
				}
				else if(chInput[i] == 0xFB15) // ARMENIAN SMALL LIGATURE MEN INI:
				{
					// un-ligature it to MEN INI
					sb->Append(Convert::ToChar(0x0574));
					sb->Append(Convert::ToChar(0x056B));
					continue;
				}
				else if(chInput[i] == 0xFB16) // ARMENIAN SMALL LIGATURE VEW NOW:
				{
					// un-ligature it to VEW NOW
					sb->Append(Convert::ToChar(0x057E));
					sb->Append(Convert::ToChar(0x0576));
					continue;
				}
				else if(chInput[i] == 0xFB17) // ARMENIAN SMALL LIGATURE MEN XEH:
				{
					// un-ligature it to MEN XEH
					sb->Append(Convert::ToChar(0x0574));
					sb->Append(Convert::ToChar(0x056D));
					continue;
				}
				else
				{
					if(chInput[i] < 0x531 || chInput[i] > 0x58F)
					  return 0;
					Char enCh = Convert::ToChar(chInput[i]);
// assumed that YEV words exist in the dictionary
					//if(chInput[i] == 0x587) // ARMENIAN SMALL LIGATURE YEV
					//{
					//	// un-ligature it to YECH VYUN 
					//	sb->Append(Convert::ToChar(0x0565)); 
					//	sb->Append(Convert::ToChar(0x0582)); 
					//}
					//else if(enCh > 0)
					if(enCh > 0)
						sb->Append(enCh);
				}
			}
		}
		String^ sEncoded = gcnew String(sb->ToString());
		lstEncoded->Add(sEncoded);
		return nRetVal;
	}
	int Wrapper::EncodeASCIIString_U(String^ sInput, ArrayList^ lstEncoded)
	{
		int nRetVal = 1;
		array<Char>^ chInput = sInput->ToCharArray(); 
		StringBuilder^ sb = gcnew StringBuilder();

		for(int i= 0; i < chInput->Length; i++)
		{
			if(chInput[i] == 0x2D || chInput[i] == 0x58A) // dash character or hy_hyphen
			{
				// dash character is used for Armenian hyphenation
				continue;
			}
			else
			{
				if(chInput[i] < 0xA1 || chInput[i] > 0xFE)
				  return 0;
				Char enCh = arrAsciiToUni_U[chInput[i] - 0xA1];
// assumed that YEV words exist in the dictionary
				if(chInput[i] == 0xA2 || chInput[i] == 0xA8) // ARMENIAN SMALL LIGATURE YEV
					sb->Append(Convert::ToChar(0x587));
				//{	// un-ligature it to YECH VYUN 
				//	sb->Append(Convert::ToChar(0x0565)); 
				//	sb->Append(Convert::ToChar(0x0582)); 
				//}
				else if(enCh > 0)
					sb->Append(enCh);
			}
		}
		String^ sEncoded = gcnew String(sb->ToString());
		lstEncoded->Add(sEncoded);
		return nRetVal;
	}
	bool Wrapper::Decode(String^ sInput, ArrayList^ lstDecoded)
	{
		array<Char>^ chInput = sInput->ToCharArray(); 
		StringBuilder^ sb = gcnew StringBuilder();

		for(int i= 0; i < chInput->Length; i++)
		{
			//if(chInput[i] < 0xA1 || chInput[i] > 0xFE)
			//  return false;
			Char enCh = arrAsciiToUni[chInput[i] - 0xB2];
			if(enCh > 0)
				sb->Append(enCh);
			if(chInput[i] == 0x20)
				sb->Append(Convert::ToChar(0x0020));
			//if(chInput[i] == 0xA2) // ARMENIAN SMALL LIGATURE YEV
			//	sb->Append(Convert::ToChar(0xF5)); // un-ligature it to YECH VYUN
		}
		String^ sDecoded = gcnew String(sb->ToString());
		lstDecoded->Add(sDecoded);
		return true;
	}

	bool Wrapper::DecodeMixed(String^ sInput, ArrayList^ lstDecoded)
	{
		array<Char>^ chInput = sInput->ToCharArray(); 
		StringBuilder^ sb = gcnew StringBuilder();

		for(int i= 0; i < chInput->Length; i++)
		{
			// if code is in ARMSCII-8 alpha, convert to unicode
			// otherwise leave it as it is
			if (chInput[i] > 0xB1 && chInput[i] < 0xFD)
			{
				//if(chInput[i] < 0xA1 || chInput[i] > 0xFE)
				//  return false;
				Char enCh = arrAsciiToUni[chInput[i] - 0xB2];
				if(enCh > 0)
					sb->Append(enCh);
				if(chInput[i] == 0x20)
					sb->Append(Convert::ToChar(0x0020));
				//if(chInput[i] == 0xA2) // ARMENIAN SMALL LIGATURE YEV
				//	sb->Append(Convert::ToChar(0xF5)); // un-ligature it to YECH VYUN
			}
			else
				sb->Append(Convert::ToChar(chInput[i]));
		}
		String^ sDecoded = gcnew String(sb->ToString());
		lstDecoded->Add(sDecoded);
		return true;
	}

	bool Wrapper::DecodeMixedWithAccents(String^ sInput, ArrayList^ lstDecoded)
	{
		array<Char>^ chInput = sInput->ToCharArray(); 
		StringBuilder^ sb = gcnew StringBuilder();

		for(int i= 0; i < chInput->Length; i++)
		{
			// if code is in ARMSCII-8 alpha, convert to unicode
			// otherwise leave it as it is
			if (chInput[i] >= 0xA1 && chInput[i] <= 0xFE)
			{
				Char enCh = arrAsciiToUni_U[chInput[i] - 0xA1];
				if(chInput[i] == 0x20)
					sb->Append(Convert::ToChar(0x0020));
				else if(chInput[i] == 0xA2 || chInput[i] == 0xA8) // ARMENIAN SMALL LIGATURE YEV
					sb->Append(Convert::ToChar(0x0587)); 
				else if(enCh > 0)
					sb->Append(enCh);
			}
			else
				sb->Append(Convert::ToChar(chInput[i]));
		}
		String^ sDecoded = gcnew String(sb->ToString());
		lstDecoded->Add(sDecoded);
		return true;
	}

	String^ Wrapper::EncodeASCIIStringToUnicode(String^ sInput)
	{
		int nRetVal = 1;
		array<Char>^ chInput = sInput->ToCharArray(); 
		StringBuilder^ sb = gcnew StringBuilder();

		for(int i= 0; i < chInput->Length; i++)
		{
			if(chInput[i] < 0xA1 || chInput[i] > 0xFE)
				sb->Append(Convert::ToChar(chInput[i]));
			else
			{
				Char enCh = arrAsciiToUni_KeepAccents[chInput[i] - 0xA1];
				if(chInput[i] == 0xA2 || chInput[i] == 0xA8) // ARMENIAN SMALL LIGATURE YEV
					sb->Append(Convert::ToChar(0x0587));
				//{	// un-ligature it to YECH VYUN 
				//	sb->Append(Convert::ToChar(0x0565)); 
				//	sb->Append(Convert::ToChar(0x0582)); 
				//}
				else if(enCh > 0)
					sb->Append(enCh);
			}
		}
		String^ sEncoded = gcnew String(sb->ToString());
		
		return sEncoded; 
	}
	
	char* Wrapper::AllocUTF8FromString(String^ str)
	{
		array<Byte>^ byteArray = Encoding::UTF8->GetBytes(str);
		int size = Marshal::SizeOf(byteArray[0]) * (byteArray->Length + 1);
		IntPtr buffer = Marshal::AllocHGlobal(size);
		Marshal::Copy(byteArray, 0, buffer, byteArray->Length);
		Marshal::WriteByte(buffer, size - 1, 0);
		return (char*) buffer.ToPointer();
	}
	String^ Wrapper::AllocStringFromUTF8(char* chr)
	{
		int size = strlen(chr);
		array<Byte> ^ byteArray = gcnew array<Byte>(size);
		Marshal::Copy(IntPtr(chr), byteArray, 0, size);
		return Encoding::UTF8->GetString(byteArray);
	}
	
	int Wrapper::Encode_U(String^ sInput, ArrayList^ lstEncoded)
	{
		array<Char>^ chInput = sInput->ToCharArray(); 
		if(chInput->Length == 0)
			return 0;
		if((chInput[0] > 0x530 && chInput[0] < 0x590)
			|| (chInput[0] > 0xFB12 && chInput[0] < 0xFB18))
			return EncodeUNIString_U(sInput, lstEncoded);
		else if(chInput[0] > 0xA0 && chInput[0] < 0xFF)
			return EncodeASCIIString_U(sInput, lstEncoded);
		else
			return 0;
	}
	bool Wrapper::Decode_U(String^ sInput, ArrayList^ lstDecoded)
	{
		array<Char>^ chInput = sInput->ToCharArray(); 
		StringBuilder^ sb = gcnew StringBuilder();

		for(int i= 0; i < chInput->Length; i++)
		{
			Char enCh = arrUniToAscii[chInput[i] - 0x0531];
			if(enCh > 0)
				sb->Append(enCh);
			if(chInput[i] == 0x0020)
				sb->Append(Convert::ToChar(0x0020));
		}
		String^ sDecoded = gcnew String(sb->ToString());
		lstDecoded->Add(sDecoded);
		return true;
	}

}