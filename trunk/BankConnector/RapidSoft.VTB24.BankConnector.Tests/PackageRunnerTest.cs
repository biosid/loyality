using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RapidSoft.VTB24.BankConnector.Tests
{
    using System.IO;
    using System.Net.Mail;

    using RapidSoft.Etl.Logging;
    using RapidSoft.Etl.Runtime;

    using S22.Imap;

    [TestClass]
    public class PackageRunnerTest
    {
        private string outlookMail =             
            @"Return-Path: loyalty@bonus.vtb24.ru
Received: from winfe0 ([10.14.14.45])
	by mail.bonus.vtb24.ru
	; Tue, 6 Aug 2013 18:41:20 +0400
From: =?koi8-r?B?9/TiLevPzMzFy8PJ0Q==?= <loyalty@bonus.vtb24.ru>
To: <loyalty@bonus.vtb24.ru>
References: <201308061437.r76Eb49d028041@adwh-prod-node0.vtb24.ru>
In-Reply-To: <201308061437.r76Eb49d028041@adwh-prod-node0.vtb24.ru>
Subject: FW: test_file_for_rapidsoft
Date: Tue, 6 Aug 2013 18:41:20 +0400
Message-ID: <008401ce92b3$0411cdc0$0c356940$@bonus.vtb24.ru>
MIME-Version: 1.0
Content-Type: multipart/mixed;
	boundary=""----=_NextPart_000_0085_01CE92D4.8B25B7B0""
X-Mailer: Microsoft Outlook 14.0
Thread-Index: AQKQ/HnV5L7u8ieOmkROtExjB6+srpgDbRRw
Content-Language: ru

This is a multipart message in MIME format.

------=_NextPart_000_0085_01CE92D4.8B25B7B0
Content-Type: multipart/alternative;
	boundary=""----=_NextPart_001_0086_01CE92D4.8B25B7B0""


------=_NextPart_001_0086_01CE92D4.8B25B7B0
Content-Type: text/plain;
	charset=""koi8-r""
Content-Transfer-Encoding: 7bit

 
 
From: rewards_to_be@vtb24.ru [mailto:rewards_to_be@vtb24.ru] 
Sent: Tuesday, August 06, 2013 6:37 PM
To: loyalty@bonus.vtb24.ru; simakovlv@vtb24.ru; zaharova.ai@vtb24.ru
Subject: test_file_for_rapidsoft
 
 

------=_NextPart_001_0086_01CE92D4.8B25B7B0
Content-Type: text/html;
	charset=""koi8-r""
Content-Transfer-Encoding: quoted-printable

<html xmlns:o=3D""urn:schemas-microsoft-com:office:office"" =
xmlns:w=3D""urn:schemas-microsoft-com:office:word"" =
xmlns:m=3D""http://schemas.microsoft.com/office/2004/12/omml"" =
xmlns=3D""http://www.w3.org/TR/REC-html40""><head><meta =
http-equiv=3DContent-Type content=3D""text/html; charset=3Dkoi8-r""><meta =
name=3DProgId content=3DWord.Document><meta name=3DGenerator =
content=3D""Microsoft Word 14""><meta name=3DOriginator =
content=3D""Microsoft Word 14""><link rel=3DFile-List =
href=3D""cid:filelist.xml@01CE92D4.8B105AF0""><!--[if gte mso 9]><xml>
<o:OfficeDocumentSettings>
<o:AllowPNG/>
</o:OfficeDocumentSettings>
</xml><![endif]--><link rel=3DthemeData href=3D""~~themedata~~""><link =
rel=3DcolorSchemeMapping href=3D""~~colorschememapping~~""><!--[if gte mso =
9]><xml>
<w:WordDocument>
<w:View>Normal</w:View>
<w:TrackMoves/>
<w:TrackFormatting/>
<w:EnvelopeVis/>
<w:PunctuationKerning/>
<w:ValidateAgainstSchemas/>
<w:SaveIfXMLInvalid>false</w:SaveIfXMLInvalid>
<w:IgnoreMixedContent>false</w:IgnoreMixedContent>
<w:AlwaysShowPlaceholderText>false</w:AlwaysShowPlaceholderText>
<w:DoNotPromoteQF/>
<w:LidThemeOther>RU</w:LidThemeOther>
<w:LidThemeAsian>X-NONE</w:LidThemeAsian>
<w:LidThemeComplexScript>X-NONE</w:LidThemeComplexScript>
<w:Compatibility>
<w:BreakWrappedTables/>
<w:SnapToGridInCell/>
<w:WrapTextWithPunct/>
<w:UseAsianBreakRules/>
<w:DontGrowAutofit/>
<w:SplitPgBreakAndParaMark/>
<w:EnableOpenTypeKerning/>
<w:DontFlipMirrorIndents/>
<w:OverrideTableStyleHps/>
</w:Compatibility>
<m:mathPr>
<m:mathFont m:val=3D""Cambria Math""/>
<m:brkBin m:val=3D""before""/>
<m:brkBinSub m:val=3D""&#45;-""/>
<m:smallFrac m:val=3D""off""/>
<m:dispDef/>
<m:lMargin m:val=3D""0""/>
<m:rMargin m:val=3D""0""/>
<m:defJc m:val=3D""centerGroup""/>
<m:wrapIndent m:val=3D""1440""/>
<m:intLim m:val=3D""subSup""/>
<m:naryLim m:val=3D""undOvr""/>
</m:mathPr></w:WordDocument>
</xml><![endif]--><!--[if gte mso 9]><xml>
<w:LatentStyles DefLockedState=3D""false"" DefUnhideWhenUsed=3D""true"" =
DefSemiHidden=3D""true"" DefQFormat=3D""false"" DefPriority=3D""99"" =
LatentStyleCount=3D""267"">
<w:LsdException Locked=3D""false"" Priority=3D""0"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" QFormat=3D""true"" Name=3D""Normal""/>
<w:LsdException Locked=3D""false"" Priority=3D""9"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" QFormat=3D""true"" Name=3D""heading 1""/>
<w:LsdException Locked=3D""false"" Priority=3D""9"" QFormat=3D""true"" =
Name=3D""heading 2""/>
<w:LsdException Locked=3D""false"" Priority=3D""9"" QFormat=3D""true"" =
Name=3D""heading 3""/>
<w:LsdException Locked=3D""false"" Priority=3D""9"" QFormat=3D""true"" =
Name=3D""heading 4""/>
<w:LsdException Locked=3D""false"" Priority=3D""9"" QFormat=3D""true"" =
Name=3D""heading 5""/>
<w:LsdException Locked=3D""false"" Priority=3D""9"" QFormat=3D""true"" =
Name=3D""heading 6""/>
<w:LsdException Locked=3D""false"" Priority=3D""9"" QFormat=3D""true"" =
Name=3D""heading 7""/>
<w:LsdException Locked=3D""false"" Priority=3D""9"" QFormat=3D""true"" =
Name=3D""heading 8""/>
<w:LsdException Locked=3D""false"" Priority=3D""9"" QFormat=3D""true"" =
Name=3D""heading 9""/>
<w:LsdException Locked=3D""false"" Priority=3D""39"" Name=3D""toc 1""/>
<w:LsdException Locked=3D""false"" Priority=3D""39"" Name=3D""toc 2""/>
<w:LsdException Locked=3D""false"" Priority=3D""39"" Name=3D""toc 3""/>
<w:LsdException Locked=3D""false"" Priority=3D""39"" Name=3D""toc 4""/>
<w:LsdException Locked=3D""false"" Priority=3D""39"" Name=3D""toc 5""/>
<w:LsdException Locked=3D""false"" Priority=3D""39"" Name=3D""toc 6""/>
<w:LsdException Locked=3D""false"" Priority=3D""39"" Name=3D""toc 7""/>
<w:LsdException Locked=3D""false"" Priority=3D""39"" Name=3D""toc 8""/>
<w:LsdException Locked=3D""false"" Priority=3D""39"" Name=3D""toc 9""/>
<w:LsdException Locked=3D""false"" Priority=3D""35"" QFormat=3D""true"" =
Name=3D""caption""/>
<w:LsdException Locked=3D""false"" Priority=3D""10"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" QFormat=3D""true"" Name=3D""Title""/>
<w:LsdException Locked=3D""false"" Priority=3D""1"" Name=3D""Default =
Paragraph Font""/>
<w:LsdException Locked=3D""false"" Priority=3D""11"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" QFormat=3D""true"" Name=3D""Subtitle""/>
<w:LsdException Locked=3D""false"" Priority=3D""22"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" QFormat=3D""true"" Name=3D""Strong""/>
<w:LsdException Locked=3D""false"" Priority=3D""20"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" QFormat=3D""true"" Name=3D""Emphasis""/>
<w:LsdException Locked=3D""false"" Priority=3D""59"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Table Grid""/>
<w:LsdException Locked=3D""false"" UnhideWhenUsed=3D""false"" =
Name=3D""Placeholder Text""/>
<w:LsdException Locked=3D""false"" Priority=3D""1"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" QFormat=3D""true"" Name=3D""No Spacing""/>
<w:LsdException Locked=3D""false"" Priority=3D""60"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Light Shading""/>
<w:LsdException Locked=3D""false"" Priority=3D""61"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Light List""/>
<w:LsdException Locked=3D""false"" Priority=3D""62"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Light Grid""/>
<w:LsdException Locked=3D""false"" Priority=3D""63"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Shading 1""/>
<w:LsdException Locked=3D""false"" Priority=3D""64"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Shading 2""/>
<w:LsdException Locked=3D""false"" Priority=3D""65"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium List 1""/>
<w:LsdException Locked=3D""false"" Priority=3D""66"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium List 2""/>
<w:LsdException Locked=3D""false"" Priority=3D""67"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Grid 1""/>
<w:LsdException Locked=3D""false"" Priority=3D""68"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Grid 2""/>
<w:LsdException Locked=3D""false"" Priority=3D""69"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Grid 3""/>
<w:LsdException Locked=3D""false"" Priority=3D""70"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Dark List""/>
<w:LsdException Locked=3D""false"" Priority=3D""71"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Colorful Shading""/>
<w:LsdException Locked=3D""false"" Priority=3D""72"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Colorful List""/>
<w:LsdException Locked=3D""false"" Priority=3D""73"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Colorful Grid""/>
<w:LsdException Locked=3D""false"" Priority=3D""60"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Light Shading Accent 1""/>
<w:LsdException Locked=3D""false"" Priority=3D""61"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Light List Accent 1""/>
<w:LsdException Locked=3D""false"" Priority=3D""62"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Light Grid Accent 1""/>
<w:LsdException Locked=3D""false"" Priority=3D""63"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Shading 1 Accent 1""/>
<w:LsdException Locked=3D""false"" Priority=3D""64"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Shading 2 Accent 1""/>
<w:LsdException Locked=3D""false"" Priority=3D""65"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium List 1 Accent 1""/>
<w:LsdException Locked=3D""false"" UnhideWhenUsed=3D""false"" =
Name=3D""Revision""/>
<w:LsdException Locked=3D""false"" Priority=3D""34"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" QFormat=3D""true"" Name=3D""List Paragraph""/>
<w:LsdException Locked=3D""false"" Priority=3D""29"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" QFormat=3D""true"" Name=3D""Quote""/>
<w:LsdException Locked=3D""false"" Priority=3D""30"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" QFormat=3D""true"" Name=3D""Intense Quote""/>
<w:LsdException Locked=3D""false"" Priority=3D""66"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium List 2 Accent 1""/>
<w:LsdException Locked=3D""false"" Priority=3D""67"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Grid 1 Accent 1""/>
<w:LsdException Locked=3D""false"" Priority=3D""68"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Grid 2 Accent 1""/>
<w:LsdException Locked=3D""false"" Priority=3D""69"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Grid 3 Accent 1""/>
<w:LsdException Locked=3D""false"" Priority=3D""70"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Dark List Accent 1""/>
<w:LsdException Locked=3D""false"" Priority=3D""71"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Colorful Shading Accent 1""/>
<w:LsdException Locked=3D""false"" Priority=3D""72"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Colorful List Accent 1""/>
<w:LsdException Locked=3D""false"" Priority=3D""73"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Colorful Grid Accent 1""/>
<w:LsdException Locked=3D""false"" Priority=3D""60"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Light Shading Accent 2""/>
<w:LsdException Locked=3D""false"" Priority=3D""61"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Light List Accent 2""/>
<w:LsdException Locked=3D""false"" Priority=3D""62"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Light Grid Accent 2""/>
<w:LsdException Locked=3D""false"" Priority=3D""63"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Shading 1 Accent 2""/>
<w:LsdException Locked=3D""false"" Priority=3D""64"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Shading 2 Accent 2""/>
<w:LsdException Locked=3D""false"" Priority=3D""65"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium List 1 Accent 2""/>
<w:LsdException Locked=3D""false"" Priority=3D""66"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium List 2 Accent 2""/>
<w:LsdException Locked=3D""false"" Priority=3D""67"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Grid 1 Accent 2""/>
<w:LsdException Locked=3D""false"" Priority=3D""68"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Grid 2 Accent 2""/>
<w:LsdException Locked=3D""false"" Priority=3D""69"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Grid 3 Accent 2""/>
<w:LsdException Locked=3D""false"" Priority=3D""70"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Dark List Accent 2""/>
<w:LsdException Locked=3D""false"" Priority=3D""71"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Colorful Shading Accent 2""/>
<w:LsdException Locked=3D""false"" Priority=3D""72"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Colorful List Accent 2""/>
<w:LsdException Locked=3D""false"" Priority=3D""73"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Colorful Grid Accent 2""/>
<w:LsdException Locked=3D""false"" Priority=3D""60"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Light Shading Accent 3""/>
<w:LsdException Locked=3D""false"" Priority=3D""61"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Light List Accent 3""/>
<w:LsdException Locked=3D""false"" Priority=3D""62"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Light Grid Accent 3""/>
<w:LsdException Locked=3D""false"" Priority=3D""63"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Shading 1 Accent 3""/>
<w:LsdException Locked=3D""false"" Priority=3D""64"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Shading 2 Accent 3""/>
<w:LsdException Locked=3D""false"" Priority=3D""65"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium List 1 Accent 3""/>
<w:LsdException Locked=3D""false"" Priority=3D""66"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium List 2 Accent 3""/>
<w:LsdException Locked=3D""false"" Priority=3D""67"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Grid 1 Accent 3""/>
<w:LsdException Locked=3D""false"" Priority=3D""68"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Grid 2 Accent 3""/>
<w:LsdException Locked=3D""false"" Priority=3D""69"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Grid 3 Accent 3""/>
<w:LsdException Locked=3D""false"" Priority=3D""70"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Dark List Accent 3""/>
<w:LsdException Locked=3D""false"" Priority=3D""71"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Colorful Shading Accent 3""/>
<w:LsdException Locked=3D""false"" Priority=3D""72"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Colorful List Accent 3""/>
<w:LsdException Locked=3D""false"" Priority=3D""73"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Colorful Grid Accent 3""/>
<w:LsdException Locked=3D""false"" Priority=3D""60"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Light Shading Accent 4""/>
<w:LsdException Locked=3D""false"" Priority=3D""61"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Light List Accent 4""/>
<w:LsdException Locked=3D""false"" Priority=3D""62"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Light Grid Accent 4""/>
<w:LsdException Locked=3D""false"" Priority=3D""63"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Shading 1 Accent 4""/>
<w:LsdException Locked=3D""false"" Priority=3D""64"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Shading 2 Accent 4""/>
<w:LsdException Locked=3D""false"" Priority=3D""65"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium List 1 Accent 4""/>
<w:LsdException Locked=3D""false"" Priority=3D""66"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium List 2 Accent 4""/>
<w:LsdException Locked=3D""false"" Priority=3D""67"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Grid 1 Accent 4""/>
<w:LsdException Locked=3D""false"" Priority=3D""68"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Grid 2 Accent 4""/>
<w:LsdException Locked=3D""false"" Priority=3D""69"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Grid 3 Accent 4""/>
<w:LsdException Locked=3D""false"" Priority=3D""70"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Dark List Accent 4""/>
<w:LsdException Locked=3D""false"" Priority=3D""71"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Colorful Shading Accent 4""/>
<w:LsdException Locked=3D""false"" Priority=3D""72"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Colorful List Accent 4""/>
<w:LsdException Locked=3D""false"" Priority=3D""73"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Colorful Grid Accent 4""/>
<w:LsdException Locked=3D""false"" Priority=3D""60"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Light Shading Accent 5""/>
<w:LsdException Locked=3D""false"" Priority=3D""61"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Light List Accent 5""/>
<w:LsdException Locked=3D""false"" Priority=3D""62"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Light Grid Accent 5""/>
<w:LsdException Locked=3D""false"" Priority=3D""63"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Shading 1 Accent 5""/>
<w:LsdException Locked=3D""false"" Priority=3D""64"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Shading 2 Accent 5""/>
<w:LsdException Locked=3D""false"" Priority=3D""65"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium List 1 Accent 5""/>
<w:LsdException Locked=3D""false"" Priority=3D""66"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium List 2 Accent 5""/>
<w:LsdException Locked=3D""false"" Priority=3D""67"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Grid 1 Accent 5""/>
<w:LsdException Locked=3D""false"" Priority=3D""68"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Grid 2 Accent 5""/>
<w:LsdException Locked=3D""false"" Priority=3D""69"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Grid 3 Accent 5""/>
<w:LsdException Locked=3D""false"" Priority=3D""70"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Dark List Accent 5""/>
<w:LsdException Locked=3D""false"" Priority=3D""71"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Colorful Shading Accent 5""/>
<w:LsdException Locked=3D""false"" Priority=3D""72"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Colorful List Accent 5""/>
<w:LsdException Locked=3D""false"" Priority=3D""73"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Colorful Grid Accent 5""/>
<w:LsdException Locked=3D""false"" Priority=3D""60"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Light Shading Accent 6""/>
<w:LsdException Locked=3D""false"" Priority=3D""61"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Light List Accent 6""/>
<w:LsdException Locked=3D""false"" Priority=3D""62"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Light Grid Accent 6""/>
<w:LsdException Locked=3D""false"" Priority=3D""63"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Shading 1 Accent 6""/>
<w:LsdException Locked=3D""false"" Priority=3D""64"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Shading 2 Accent 6""/>
<w:LsdException Locked=3D""false"" Priority=3D""65"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium List 1 Accent 6""/>
<w:LsdException Locked=3D""false"" Priority=3D""66"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium List 2 Accent 6""/>
<w:LsdException Locked=3D""false"" Priority=3D""67"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Grid 1 Accent 6""/>
<w:LsdException Locked=3D""false"" Priority=3D""68"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Grid 2 Accent 6""/>
<w:LsdException Locked=3D""false"" Priority=3D""69"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Medium Grid 3 Accent 6""/>
<w:LsdException Locked=3D""false"" Priority=3D""70"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Dark List Accent 6""/>
<w:LsdException Locked=3D""false"" Priority=3D""71"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Colorful Shading Accent 6""/>
<w:LsdException Locked=3D""false"" Priority=3D""72"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Colorful List Accent 6""/>
<w:LsdException Locked=3D""false"" Priority=3D""73"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" Name=3D""Colorful Grid Accent 6""/>
<w:LsdException Locked=3D""false"" Priority=3D""19"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" QFormat=3D""true"" Name=3D""Subtle Emphasis""/>
<w:LsdException Locked=3D""false"" Priority=3D""21"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" QFormat=3D""true"" Name=3D""Intense Emphasis""/>
<w:LsdException Locked=3D""false"" Priority=3D""31"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" QFormat=3D""true"" Name=3D""Subtle Reference""/>
<w:LsdException Locked=3D""false"" Priority=3D""32"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" QFormat=3D""true"" Name=3D""Intense Reference""/>
<w:LsdException Locked=3D""false"" Priority=3D""33"" SemiHidden=3D""false"" =
UnhideWhenUsed=3D""false"" QFormat=3D""true"" Name=3D""Book Title""/>
<w:LsdException Locked=3D""false"" Priority=3D""37"" Name=3D""Bibliography""/>
<w:LsdException Locked=3D""false"" Priority=3D""39"" QFormat=3D""true"" =
Name=3D""TOC Heading""/>
</w:LatentStyles>
</xml><![endif]--><style><!--
/* Font Definitions */
@font-face
	{font-family:Calibri;
	panose-1:2 15 5 2 2 2 4 3 2 4;
	mso-font-charset:204;
	mso-generic-font-family:swiss;
	mso-font-pitch:variable;
	mso-font-signature:-520092929 1073786111 9 0 415 0;}
@font-face
	{font-family:Tahoma;
	panose-1:2 11 6 4 3 5 4 4 2 4;
	mso-font-charset:204;
	mso-generic-font-family:swiss;
	mso-font-pitch:variable;
	mso-font-signature:-520081665 -1073717157 41 0 66047 0;}
/* Style Definitions */
p.MsoNormal, li.MsoNormal, div.MsoNormal
	{mso-style-unhide:no;
	mso-style-qformat:yes;
	mso-style-parent:"";
	margin:0cm;
	margin-bottom:.0001pt;
	mso-pagination:widow-orphan;
	font-size:11.0pt;
	font-family:""Calibri"",""sans-serif"";
	mso-ascii-font-family:Calibri;
	mso-ascii-theme-font:minor-latin;
	mso-fareast-font-family:Calibri;
	mso-fareast-theme-font:minor-latin;
	mso-hansi-font-family:Calibri;
	mso-hansi-theme-font:minor-latin;
	mso-bidi-font-family:""Times New Roman"";
	mso-bidi-theme-font:minor-bidi;
	mso-fareast-language:EN-US;}
a:link, span.MsoHyperlink
	{mso-style-noshow:yes;
	mso-style-priority:99;
	color:blue;
	mso-themecolor:hyperlink;
	text-decoration:underline;
	text-underline:single;}
a:visited, span.MsoHyperlinkFollowed
	{mso-style-noshow:yes;
	mso-style-priority:99;
	color:purple;
	mso-themecolor:followedhyperlink;
	text-decoration:underline;
	text-underline:single;}
span.EmailStyle17
	{mso-style-type:personal-reply;
	mso-style-noshow:yes;
	mso-style-unhide:no;
	mso-ansi-font-size:11.0pt;
	mso-bidi-font-size:11.0pt;
	font-family:""Calibri"",""sans-serif"";
	mso-ascii-font-family:Calibri;
	mso-ascii-theme-font:minor-latin;
	mso-fareast-font-family:Calibri;
	mso-fareast-theme-font:minor-latin;
	mso-hansi-font-family:Calibri;
	mso-hansi-theme-font:minor-latin;
	mso-bidi-font-family:""Times New Roman"";
	mso-bidi-theme-font:minor-bidi;
	color:#1F497D;
	mso-themecolor:dark2;}
.MsoChpDefault
	{mso-style-type:export-only;
	mso-default-props:yes;
	font-family:""Calibri"",""sans-serif"";
	mso-ascii-font-family:Calibri;
	mso-ascii-theme-font:minor-latin;
	mso-fareast-font-family:Calibri;
	mso-fareast-theme-font:minor-latin;
	mso-hansi-font-family:Calibri;
	mso-hansi-theme-font:minor-latin;
	mso-bidi-font-family:""Times New Roman"";
	mso-bidi-theme-font:minor-bidi;
	mso-fareast-language:EN-US;}
@page WordSection1
	{size:612.0pt 792.0pt;
	margin:2.0cm 42.5pt 2.0cm 3.0cm;
	mso-header-margin:36.0pt;
	mso-footer-margin:36.0pt;
	mso-paper-source:0;}
div.WordSection1
	{page:WordSection1;}
--></style><!--[if gte mso 10]><style>/* Style Definitions */
table.MsoNormalTable
	{mso-style-name:""=EF=C2=D9=DE=CE=C1=D1 =D4=C1=C2=CC=C9=C3=C1"";
	mso-tstyle-rowband-size:0;
	mso-tstyle-colband-size:0;
	mso-style-noshow:yes;
	mso-style-priority:99;
	mso-style-parent:"";
	mso-padding-alt:0cm 5.4pt 0cm 5.4pt;
	mso-para-margin:0cm;
	mso-para-margin-bottom:.0001pt;
	mso-pagination:widow-orphan;
	font-size:11.0pt;
	font-family:""Calibri"",""sans-serif"";
	mso-ascii-font-family:Calibri;
	mso-ascii-theme-font:minor-latin;
	mso-hansi-font-family:Calibri;
	mso-hansi-theme-font:minor-latin;
	mso-bidi-font-family:""Times New Roman"";
	mso-bidi-theme-font:minor-bidi;
	mso-fareast-language:EN-US;}
</style><![endif]--></head><body lang=3DRU link=3Dblue vlink=3Dpurple =
style=3D'tab-interval:35.4pt'><div class=3DWordSection1><p =
class=3DMsoNormal><span =
style=3D'color:#1F497D;mso-themecolor:dark2'><o:p>&nbsp;</o:p></span></p>=
<p class=3DMsoNormal><span =
style=3D'color:#1F497D;mso-themecolor:dark2'><o:p>&nbsp;</o:p></span></p>=
<p class=3DMsoNormal><a name=3D""_MailOriginal""><b><span =
style=3D'font-size:10.0pt;font-family:""Tahoma"",""sans-serif"";mso-fareast-f=
ont-family:""Times New =
Roman"";mso-fareast-language:RU'>From:</span></b></a><span =
style=3D'mso-bookmark:_MailOriginal'><span =
style=3D'font-size:10.0pt;font-family:""Tahoma"",""sans-serif"";mso-fareast-f=
ont-family:""Times New Roman"";mso-fareast-language:RU'> =
rewards_to_be@vtb24.ru [mailto:rewards_to_be@vtb24.ru] <br><b>Sent:</b> =
Tuesday, August 06, 2013 6:37 PM<br><b>To:</b> loyalty@bonus.vtb24.ru; =
simakovlv@vtb24.ru; zaharova.ai@vtb24.ru<br><b>Subject:</b> =
test_file_for_rapidsoft<o:p></o:p></span></span></p><p =
class=3DMsoNormal><span =
style=3D'mso-bookmark:_MailOriginal'><o:p>&nbsp;</o:p></span></p><span =
style=3D'mso-bookmark:_MailOriginal'></span><p =
class=3DMsoNormal><o:p>&nbsp;</o:p></p></div></body></html>
------=_NextPart_001_0086_01CE92D4.8B25B7B0--

------=_NextPart_000_0085_01CE92D4.8B25B7B0
Content-Type: text/plain;
	name=""test_file_for_rapidsoft.txt""
Content-Transfer-Encoding: base64
Content-Disposition: attachment;
	filename=""test_file_for_rapidsoft.txt""

yu7y//LgIOLt8/Lw6CEKyCDk4OblIO3l5+D46PTw7uLg7e375SE=

------=_NextPart_000_0085_01CE92D4.8B25B7B0--

";

        private string teradataMail = 
@"Return-Path: rewards_to_be@vtb24.ru
Received: from mx01.vtb24.ru ([217.14.50.11])
	by mail.bonus.vtb24.ru
	; Tue, 6 Aug 2013 18:38:11 +0400
Received: from msk-mx01.vtb24.ru (msk-mx01.vtb24.ru [10.64.32.175])
	by mx01.vtb24.ru (8.14.3/8.14.3/SuSE Linux 0.8) with ESMTP id r76Eb4sm000883
	for <loyalty@bonus.vtb24.ru>; Tue, 6 Aug 2013 18:37:04 +0400
Received: from adwh-prod-node0.vtb24.ru (adwh-prod-node0.vtb24.ru [10.64.8.197])
	by msk-mx01.vtb24.ru (8.14.3/8.14.3/SuSE Linux 0.8) with ESMTP id r76Eb4KL003644;
	Tue, 6 Aug 2013 18:37:04 +0400
Received: from adwh-prod-node0.vtb24.ru (localhost [127.0.0.1])
	by adwh-prod-node0.vtb24.ru (8.14.4+Sun/8.13.8) with ESMTP id r76Eb5hb028049;
	Tue, 6 Aug 2013 18:37:05 +0400 (MSK)
Received: (from sasinst@localhost)
	by adwh-prod-node0.vtb24.ru (8.14.4+Sun/8.13.8/Submit) id r76Eb49d028041;
	Tue, 6 Aug 2013 18:37:05 +0400 (MSK)
Date: Tue, 6 Aug 2013 18:37:05 +0400 (MSK)
From: rewards_to_be@vtb24.ru
Message-Id: <201308061437.r76Eb49d028041@adwh-prod-node0.vtb24.ru>
X-Authentication-Warning: adwh-prod-node0.vtb24.ru: sasinst set sender to rewards_to_be@vtb24.ru using -r
To: loyalty@bonus.vtb24.ru;, simakovlv@vtb24.ru, zaharova.ai@vtb24.ru;
Subject: test_file_for_rapidsoft

begin 644 test_file_for_rapidsoft.txt
FRN[R__+@(.+M\_+PZ""$*R""#DX.;E(.WEY^#XZ/3P[N+@[>W[Y2$ 
 
end";

        [TestMethod]
        public void ShouldParseTeradataRawAttachTest()
        {
            var message = MessageBuilder.FromMIME822(this.teradataMail);

            Assert.IsTrue(message.Attachments.Count > 0, "Вложения не найдены");
            this.SaveAttachmentToFile(message.Attachments[0]);
        }

        [TestMethod]
        public void ShouldParseOutlookMailAttachTest()
        {
            var message = MessageBuilder.FromMIME822(this.outlookMail);

            Assert.IsTrue(message.Attachments.Count > 0, "Вложения не найдены");
            this.SaveAttachmentToFile(message.Attachments[0]);
        }

        private void SaveAttachmentToFile(Attachment attachment)
        {
	        const string dirName = @"c:\Temp\1";

			if (!Directory.Exists(dirName))
			{
				Directory.CreateDirectory(dirName);
			}

            var fileName = Path.Combine(dirName, attachment.Name);

            using (Stream file = File.OpenWrite(fileName))
            {
                var buffer = new byte[8 * 1024];
                int len;
                while ((len = attachment.ContentStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    file.Write(buffer, 0, len);
                }
            }
        }

        [Ignore]
        [TestMethod]
        public void ShouldRunPackageTest()
        {
            var package = EtlPackageXmlSerializer.Deserialize(@"<?xml version=""1.0"" encoding=""utf-16""?>
<EtlPackage xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Id>1560ecaf-7420-4c34-8eea-d6caee64acbc</Id>
  <Name>ReceiveRegistrationClients</Name>
  <RunIntervalSeconds>0</RunIntervalSeconds>
  <Enabled>true</Enabled>
  <Variables>
    <Variable>
      <Name>Temp</Name>
      <Modifier>Input</Modifier>
      <DefaultValue>C:\Temp</DefaultValue>
      <Binding>None</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
    <Variable>
      <Name>DB</Name>
      <Modifier>Input</Modifier>
      <DefaultValue>data source=.;initial catalog=RapidSoft.VTB24.BankConnector;integrated security=True;</DefaultValue>
      <Binding>None</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
    <Variable>
      <Name>SessionId</Name>
      <Modifier>Bound</Modifier>
      <Binding>EtlSessionId</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
	<Variable>
      <Name>LoyaltyImapHost</Name>
      <Modifier>Input</Modifier>
      <DefaultValue>notset</DefaultValue>
	  <Binding>None</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
	<Variable>
      <Name>LoyaltyImapPort</Name>
      <Modifier>Input</Modifier>
      <DefaultValue>notset</DefaultValue>
	  <Binding>None</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
	<Variable>
      <Name>LoyaltyImapUseSSL</Name>
      <Modifier>Input</Modifier>
      <DefaultValue>notset</DefaultValue>
	  <Binding>None</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
	<Variable>
      <Name>LoyaltyImapUserName</Name>
      <Modifier>Input</Modifier>
      <DefaultValue>notset</DefaultValue>
	  <Binding>None</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
	<Variable>
      <Name>LoyaltyImapPassword</Name>
      <Modifier>Input</Modifier>
      <DefaultValue>notset</DefaultValue>
	  <Binding>None</Binding>
      <IsSecure>false</IsSecure>
    </Variable>
  </Variables>
  <Steps>
    <EmailReceiveImap>
      <Name>Загрузка вложений писем</Name>
      <TimeoutMilliseconds xsi:nil=""true"" />
      <EmailServer>
        <Host>10.14.14.45</Host>
        <Port>143</Port>
        <UseSSL>false</UseSSL>
        <UserName>loyalty@bonus.vtb24.ru</UserName>
        <Password>mail</Password>
      </EmailServer>
      <EmailDbStorage>
        <ConnectionString>$(DB)</ConnectionString>
        <SchemaName>etl</SchemaName>
      </EmailDbStorage>
      <Message>
        <Filters>
          <ReceiveFilter>
            <SubjectContains>regPL</SubjectContains>
          </ReceiveFilter>
        </Filters>
        <AttachmentRegExp>^VTB_[1-2][0-9]{3}[0-1][0-9][0-3][0-9]_[0-9]+.regPL.response$</AttachmentRegExp>
      </Message>
      <Destination>
        <Name>Temp Folder</Name>
        <FilePath>$(Temp)\VTB_$(SessionId)</FilePath>
        <CodePage>1251</CodePage>
      </Destination>
    </EmailReceiveImap>
<!--     <Decrypt>
      <Name>Decrypt</Name>
      <TimeoutMilliseconds xsi:nil=""true"" />
      <WorkingDirectory>$(Temp)\VTB_$(SessionId)</WorkingDirectory>
    </Decrypt>
    <ImportCsvFileBatch>
      <Name>ImportCsvFileBatch</Name>
      <TimeoutMilliseconds xsi:nil=""true"" />
      <Source>
        <Name>Файл</Name>
        <FilePath>$(Temp)\VTB_$(SessionId)</FilePath>
        <CodePage>1251</CodePage>
        <HasHeaders>true</HasHeaders>
        <FieldDelimiter>;</FieldDelimiter>
      </Source>
      <Destination>
        <Name>База данных</Name>
        <ConnectionString>$(DB)</ConnectionString>
        <ProviderName>System.Data.SqlClient</ProviderName>
        <TableName>dbo.ClientForRegistrationResponse</TableName>
      </Destination>
      <BatchSize>0</BatchSize>
      <Mappings>
        <Mapping>
          <SourceFieldName>ClientId</SourceFieldName>
          <DestinationFieldName>ClientId</DestinationFieldName>
        </Mapping>
        <Mapping>
          <DestinationFieldName>SessionId</DestinationFieldName>
          <DefaultValue>$(SessionId)</DefaultValue>
        </Mapping>
        <Mapping>
          <SourceFieldName>Status</SourceFieldName>
          <DestinationFieldName>Status</DestinationFieldName>
        </Mapping>
        <Mapping>
          <SourceFieldName>Segment</SourceFieldName>
          <DestinationFieldName>Segment</DestinationFieldName>
        </Mapping>
      </Mappings>
      <DataLossBehavior>Fail</DataLossBehavior>
    </ImportCsvFileBatch>
    <InvokeMethod>
      <Name>Вызов оркестратора</Name>
      <TimeoutMilliseconds xsi:nil=""true"" />
      <Source>
        <Name>assembly</Name>
        <AssemblyName>RapidSoft.VTB24.BankConnector</AssemblyName>
        <TypeName>RapidSoft.VTB24.BankConnector.EtlInvokeHelper</TypeName>
        <MethodName>RegisterClients</MethodName>
        <Parameters />
      </Source>
    </InvokeMethod>
    <EmailDeleteImap>
      <Name>DeleteMailImap</Name>
      <TimeoutMilliseconds xsi:nil=""true"" />
      <EmailServer>
        <Host>$(LoyaltyImapHost)</Host>
        <Port>$(LoyaltyImapPort)</Port>
        <UseSSL>$(LoyaltyImapUseSSL)</UseSSL>
        <UserName>$(LoyaltyImapUserName)</UserName>
        <Password>$(LoyaltyImapPassword)</Password>
      </EmailServer>
      <EmailDbStorage>
        <ConnectionString>$(DB)</ConnectionString>
        <SchemaName>etl</SchemaName>
      </EmailDbStorage>
    </EmailDeleteImap> -->
  </Steps>
</EtlPackage>");

            package.Invoke(new MemoryEtlLogger());
           
        }
    }
}