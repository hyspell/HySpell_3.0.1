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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SetupApp
{
    public partial class KeyboardLayout : Form
    {
        public int SelectedLayoutIndex = 0;

        public KeyboardLayout()
        {
            InitializeComponent();
        }

        private void SetKeyboardLayoutImage()
        {
            switch (SelectedLayoutIndex)
            {
                case 0:
                case -1:
                    {
                        if (chkShift.Checked && chkAltGr.Checked)
                            pnlImage.BackgroundImage = SetupApp.Properties.Resources.ArmenTWE_ALTGRSHFT;
                        else if (!chkShift.Checked && chkAltGr.Checked)
                            pnlImage.BackgroundImage = SetupApp.Properties.Resources.ArmenTWE_ALTGR;
                        else if (chkShift.Checked && !chkAltGr.Checked)
                            pnlImage.BackgroundImage = SetupApp.Properties.Resources.ArmenTWE_SHFT;
                        else if (!chkShift.Checked && !chkAltGr.Checked)
                            pnlImage.BackgroundImage = SetupApp.Properties.Resources.ArmenTWE;
                    };
                    break;
                case 1:
                    {
                        if (chkShift.Checked && chkAltGr.Checked)
                            pnlImage.BackgroundImage = SetupApp.Properties.Resources.ArmenPHE_ALTGRSHFT;
                        else if (!chkShift.Checked && chkAltGr.Checked)
                            pnlImage.BackgroundImage = SetupApp.Properties.Resources.ArmenPHE_ALTGR;
                        else if (chkShift.Checked && !chkAltGr.Checked)
                            pnlImage.BackgroundImage = SetupApp.Properties.Resources.ArmenPHE_SHFT;
                        else if (!chkShift.Checked && !chkAltGr.Checked)
                            pnlImage.BackgroundImage = SetupApp.Properties.Resources.ArmenPHE;
                    };
                    break;
            }
        }
        private void chkShift_CheckedChanged(object sender, EventArgs e)
        {
            SetKeyboardLayoutImage();
        }

        private void chkAltGr_CheckedChanged(object sender, EventArgs e)
        {
            SetKeyboardLayoutImage();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void KeyboardLayout_Load(object sender, EventArgs e)
        {
            SetKeyboardLayoutImage();
        }
    }
}
