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
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace HySpellOL
{
    class Dictionaries
    {
        private Dictionary<string, string> m_Dictionary;
        private IFormatter formatter = new BinaryFormatter();

        public void FillDictionaryFromFile(string sFileName)
        {
            m_Dictionary = new Dictionary<string, string>();
            StreamReader rd = File.OpenText(sFileName);
            string sInLine = null;
            while ((sInLine = rd.ReadLine()) != null)
            {
                if (sInLine.Length > 0)
                {
                    // if the line is a comment line, skip line
                    if (sInLine.StartsWith("//"))
                        continue;
                    // else split to (key, meaning)
                    int nIndex = sInLine.IndexOf(' ');
                    string sKeyWord = sInLine.Substring(0, nIndex);
                    string sMeaning = sInLine.Substring(nIndex + 1);
                    if (!m_Dictionary.ContainsKey(sKeyWord))
                    {
                        m_Dictionary.Add(sKeyWord, sMeaning);
                    }

                }
            }
            rd.Close();
        }

        public string FindMeaning(string sWord)
        {
            string sMeaning = "";

            if (m_Dictionary.ContainsKey(sWord))
                sMeaning = m_Dictionary[sWord];

            return sMeaning;
        }

        public void SerializeDictionary(string sFileName)
        {
            using (FileStream s = File.Create(sFileName))
            {
                formatter.Serialize(s, m_Dictionary);
            }
        }
        public void DeSerializeDictionary(string sFileName)
        {
            using (FileStream s = File.OpenRead(sFileName))
            {
                m_Dictionary = (Dictionary<string, string>)formatter.Deserialize(s);
            }
        }
    }
}
