<h1>HySpell Armenian Spellchecker [ <a href="http://www.hyspell.com">www.hyspell.com</a> ]</h1>
<p><a href="#Introduction">Introduction</a><br>
<a href="#GettingStarted">Getting Started and Setting up the Visual Studio Solution</a><br>
<a href="#LinkToBinaries">Links to HySpell Spellchecker Installers</a><br></p>
<h3><a name="#Introduction">Introduction</a></h3>
<p>The current HySpell GitHub project introduces the open source release of the actual HySpell Armenian Spellchecker Microsoft Office Add-in tool. The repository contains the complete source code of the latest 3.0.1.0 release, the binaries of which were previously released only in commercial package form.</p>
<p>The main goal of the current software package, historically speaking, was to
introduce a complete and practical spell checking support for Armenian 
language
in major text processing software applications, such as, Microsoft 
Office Word
and Outlook, Adobe PageMaker/InDesign, OpenOffice, main web browsers, as
 well
as, other similar systems. Back in 2008, with the popularity and success
 of Hunspell package as an open source project, it was decided
to implement our linguistically complete spell check proof tool around 
Hunspell by introducing a .NET managed code wrapper upon
the C-code Hunspell library. Moreover, given the
capabilities of Hunspell in regards of the
constructability of affix rule and dictionary systems, it was proper for
 usage
on highly inflectional language, such as Armenian. After a year of 
linguistic
work, an extensive lexicon of more than 150 thousand base words were 
collected
and corrected to form the base dictionary file. Moreover, to capture all
possible Armenian words, more than 300 affix rules were constructed, 
bringing
the spell check accuracy to near 95%.</p>
<p>Subsequent
versions of HySpell improved the software system both
in integration with applications and the linguistic accuracy of the proofing
tool. Given the fact that Hunspell got integrated
with all major word processing applications, such as: Mozilla software
(OpenOffice, Firefox), Adobe software (InDesign), support for Armenian spell
checking, automatically got carried over onto most application, with the
exception perhaps of the most important, namely, Microsoft Office suite (Word,
Outlook). </p>
<p>Since
Microsoft had no intention to integrate Hunspell as
alternative proofing engine to their proprietary language support interface,
even when Adobe adopted Hunspell as alternative, we
had no option but utilize Visual Studio Tools for Office (VSTO), in order to
implement Word and Outlook Add-in extensions to support the spell checking for
Armenian. Besides, given the fact that there are still a number of issues in
treatment of the Armenian language even at the Unicode level (for example
mid-word punctuation that is unique to Armenian language), language support at
a more custom level is required in the case of Armenian, and that VSTO Add-in
alternative did provide a few advantages. </p>
<p>The
current repository contains therefore the source code for this VSTO Add-in alternative
implementation of the Armenian language support in Microsoft Office Word and
Outlook (and for Windows OS).</p>
<p>The
current document also includes links to the actual compiled binaries, as well
as, language support for Mozilla series of applications, in particular, the
complete suite of OpenOffice, LibreOffice, Firefox, Thunderbird, SeaMonkey, Google Chrome web browser, Adobe suite, in
particular Adobe InDesign, and almost any software that is integrated with Hunspell. </p>
<p>Note
that the proofing support at the web browser level (e.g. Firefox and Chrome),
implies proofing support in any web application, such as Google Docs, Gmail,
etc..</p>
<h3><a name="user-content-GettingStarted">Getting Started and Setting up the Visual Studio Solution</a></h3>
<p>The
current HySpell source code repository is prepared to
be used based on the following platform requirements:</p>
<ul>1.  Visual
Studio 2017 on Windows OS (or above). Note that ever since version 2015, Microsoft
has made free Community version of their Visual Studio, and Visual Studio 2017
contains all tools and features that are needed for HySpell
compilation. Developers may download Visual Studio 2017 via the following
link: 
<br><a href="https://imagine.microsoft.com/en-us/Catalog/Product/530">Microsoft Visual Studio 2017 Community Edition</a>
</ul>
<ul>2.  The
Visual Studio must be installed with all Office Customization templates and
run-times (i.e. VSTO).</ul>
<ul>3.  In
addition, to be able to compile the Setup project that exists in the HySpell VS solution, developers must also install Visual
Studio Setup/Installer Project templates. The Setup/ Installer project template
may be downloaded via the following link: 
<br><a href="https://marketplace.visualstudio.com/items?itemName=VisualStudioProductTeam.MicrosoftVisualStudio2017InstallerProjects"> Microsoft Visual Studio 2017 Installer Projects</a>
</ul>
<p>Finally, also
note that in developing HySpell Armenian spellchecker
we took a strategic initiative in utilizing the popular Hunspell
C-library (developed by László Németh),
while in the initial releases, we wrote our own C++ wrapper to port the
C-library into .NET managed code, in the latest release of HySpell
(i.e. the current release), we utilized instead an extended version of 
such
wrapper, called NHunspell (developed by Thomas Maierhofer). The binaries
 of this .NET NHunspell.dll
library along with 64-bit and 32-bit binaries of the actual C-code 
Hunspell are already included in the current HySpell repository. 
Therefore, there is no need to download
anything from Hunspell or NHunspell
sites.</p>
<p>For the
completion of all references and to respect the open source MIT and/or LGPL
license terms of respective authors, we have included the links to NHunspell and Hunspell sites
below: </p>
<ul>NHunspell site: <a href="http://www.nuget.org/packages/NHunspell/">http://www.nuget.org/packages/NHunspell/</a><br>
Hunspell site: <a href="https://github.com/hunspell/hunspell">https://github.com/hunspell/hunspell</a></ul>
<p>After installing
and/or setting up all the required development tools and platforms, download the
HySpell source file package from the current GitHub repository
and extract source code files into a directory. Then, find and open the <b>HySpell.sln</b> solution file via the
Visual Studio 2017. The solution should initialize and load into the IDE, and
should look like the screenshot in Figure 1.</p>
<p><img src="/image001.png" border="0" style="max-width:100%;"><br>
Figure 1. HySpell solution loaded in VS 2017</p>
<p>In
particular, the HySpell solution comprises of the
following VS projects:</p>
<ul>1.  <b>HySpell</b>, which is the
VSTO Word Add-in project, the main spellcheck controller that functions upon
the Word DOM.</ul>
<ul>2.  <b>HySpellOL</b>, which is the
VSTO Outlook Add-in project, the main spellcheck controller that functions upon
the Outlook DOM.</ul>
<ul>3.  <b>SetupApp</b>, which is the HySpell setup application project.</ul>
<ul>4.  <b>SetupForRequiredFiles</b>, which is the HySpell setup for the linguistic support files, along with
two extended Armenian keyboard layout drivers, standard Armenian fonts, and
other auxiliary support files.</ul>
<p>Finally, in order to be able to debug HySpell source code, a version of Microsoft Office must be installed on the development machine. In particular, for HySpell Word Add-in, Word 2010 or above must be installed and activated, while for HySpellOL Outlook Add-in, Outlook 2010 (or above). Note that the current HySpell version is tested against Windows 7, Windows 8 and Windows 10, with Microsoft Office versions 2007, 2010, 2013, 2016, Office 365, and all respective editions.
</p>
<h3><a name="user-content-LinkToBinaries">Links to HySpell Spellchecker Installers</a></h3>
<p>The following list contains links to the latest version of HySpell Spellchecker installers for various applications (in respective orthographies). These downloads are provided free of charge by <a href="http://www.hyspell.com">www.hyspell.com</a>:</p>
<ul><b>For Firefox (any OS) - Classical Orthography | Դասական Ուղղագրութիւն, Size: 909KB</b><br>
<a href="http://www.hyspell.com/Free/20170601/hyspellarmenianspellingmozillasuite_2.4.2.xpi">hyspellarmenianspellingmozillasuite_2.4.2.xpi</a><br>
<b>For Firefox (any OS) - Reformed Orthography | Նոր Ուղղագրութիւն, Size: 917KB</b><br>
<a href="http://www.hyspell.com/Free/20170601/hyspellarmenianspellingmozillasuitero_2.4.2.xpi">hyspellarmenianspellingmozillasuitero_2.4.2.xpi</a></ul>
<ul><b>OpenOffice (any OS) - Classical Orthography | Դասական Ուղղագրութիւն) Size: 904KB</b><br>
<a href="http://www.hyspell.com/Free/20170601/hyspellarmenianspellingopenoffice_2.4.2.oxt">hyspellarmenianspellingopenoffice_2.4.2.oxt</a><br>
<b>OpenOffice (any OS) - Reformed Orthography | Նոր Ուղղագրութիւն) Size: 913KB</b><br>
<a href="http://www.hyspell.com/Free/20170601/hyspellarmenianspellingopenofficero_2.4.2.oxt">hyspellarmenianspellingopenofficero_2.4.2.oxt</a></ul>
<ul><b>Microsft Office  Word and Outlook 2010 (or above, Windows only) - Size: 36.3 MB</b><br>
<a href="http://www.hyspell.com/Free/20170601/hyspell30installerwo.exe">hyspell30installerwo.exe</a></ul>

