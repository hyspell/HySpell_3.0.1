// HySpellEncoder.h

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
