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

#pragma once

#ifndef HY_IGNORECASEHANDLING
	#define HY_IGNORECASEHANDLING
#endif

using namespace System;
using namespace Collections;

namespace HySpellEncoder 
{
	public ref class Wrapper
	{
		public:
			Wrapper();
			~Wrapper();

			int Encode(String^ sInput, ArrayList^ lstEncoded);
			bool Decode(String^ sInput, ArrayList^ lstDecoded);
			int Encode_U(String^ sInput, ArrayList^ lstEncoded);
			bool Decode_U(String^ sInput, ArrayList^ lstDecoded);
			bool DecodeMixed(String^ sInput, ArrayList^ lstDecoded);
			bool DecodeMixedWithAccents(String^ sInput, ArrayList^ lstDecoded);
			String^ EncodeASCIIStringToUnicode(String^ sInput);

		private:
			char* AllocUTF8FromString(String^ str);
			String^ AllocStringFromUTF8(char* chr);
			int EncodeUNIString(String^ sInput, ArrayList^ lstEncoded);
			int EncodeASCIIString(String^ sInput, ArrayList^ lstEncoded);
			int EncodeUNIString_U(String^ sInput, ArrayList^ lstEncoded);
			int EncodeASCIIString_U(String^ sInput, ArrayList^ lstEncoded);
	};
}
