/*
Government Usage Rights Notice:  The U.S. Government retains unlimited, royalty-free usage rights to this software, but not ownership, as provided by Federal law.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

•	Redistributions of source code must retain the above Government Usage Rights Notice, this list of conditions and the following disclaimer.

•	Redistributions in binary form must reproduce the above Government Usage Rights Notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

•	Neither the names of the National Library of Medicine, the National Institutes of Health, nor the names of any of the software developers may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE U.S. GOVERNMENT AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITEDTO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE U.S. GOVERNMENT
OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System.Collections.Generic;

namespace Imppoa.HtmlZoning.Dom
{
    public static class Html
    {
        public static class CharacterEntities
        {
            public const string NON_BREAKING_SPACE = "&nbsp;";
        }

        public static class Tags
        {
            // Tag list taken from https://developer.mozilla.org/en-US/docs/Web/HTML/Element

            // THE ROOT ELEMENT

            public const string HTML = "HTML";

            // DOCUMENT METADATA

            public const string HEAD = "HEAD";
            public const string TITLE = "TITLE";
            public const string BASE = "BASE";
            public const string ISINDEX = "ISINDEX";
            public const string LINK = "LINK";
            public const string META = "META";
            public const string STYLE = "STYLE";
            public const string DOCTYPE = "!DOCTYPE";
            public const string COMMENT = "!";

            // SCRIPTING

            public const string SCRIPT = "SCRIPT";
            public const string NOSCRIPT = "NOSCRIPT";

            // SECTIONS

            public const string BODY = "BODY";
            public const string SECTION = "SECTION";
            public const string NAV = "NAV";
            public const string ARTICLE = "ARTICLE";
            public const string ASIDE = "ASIDE";
            public const string H1 = "H1";
            public const string H2 = "H2";
            public const string H3 = "H3";
            public const string H4 = "H4";
            public const string H5 = "H5";
            public const string H6 = "H6";
            public const string HGROUP = "HGROUP";
            public const string HEADER = "HEADER";
            public const string FOOTER = "FOOTER";
            public const string ADDRESS = "ADDRESS";

            // GROUPING CONTENT

            public const string P = "P";
            public const string HR = "HR";
            public const string PRE = "PRE";
            public const string BLOCKQUOTE = "BLOCKQUOTE";
            public const string OL = "OL";
            public const string UL = "UL";
            public const string LI = "LI";
            public const string DL = "DL";
            public const string DT = "DT";
            public const string DD = "DD";
            public const string FIGURE = "FIGURE";
            public const string FIGCAPTION = "FIGCAPTION";
            public const string DIV = "DIV";
            public const string CENTER = "CENTER";

            // TEXT-LEVEL SEMANTICS

            public const string A = "A";
            public const string ABBR = "ABBR";
            public const string ACRONYM = "ACRONYM";
            public const string B = "B";
            public const string BASEFONT = "BASEFONT";
            public const string BDO = "BDO";
            public const string BIG = "BIG";
            public const string BLINK = "BLINK";
            public const string BR = "BR";
            public const string CITE = "CITE";
            public const string CODE = "CODE";
            public const string DFN = "DFN";
            public const string EM = "EM";
            public const string FONT = "FONT"; 
            public const string I = "I";
            public const string KBD = "KBD";
            public const string LISTING = "LISTING";
            public const string MARK = "MARK";
            public const string MARQUEE = "MARQUEE";
            public const string NEXTID = "NEXTID";
            public const string NOBR = "NOBR";
            public const string Q = "Q";
            public const string RP = "RP";
            public const string RT = "RT";
            public const string RUBY = "RUBY";
            public const string S = "S";
            public const string SAMP = "SAMP";
            public const string SMALL = "SMALL";
            public const string SPACER = "SPACER";
            public const string SPAN = "SPAN";
            public const string STRIKE = "STRIKE";
            public const string STRONG = "STRONG";
            public const string SUB = "SUB";
            public const string SUP = "SUP";
            public const string TIME = "TIME";
            public const string TT = "TT";
            public const string U = "U"; 
            public const string VAR = "VAR";
            public const string WBR = "WBR";
            public const string XMP = "XMP";

            // EDITS

            public const string INS = "INS";
            public const string DEL = "DEL";

            // EMBEDDED CONTENT

            public const string IMG = "IMG";
            public const string IFRAME = "IFRAME";
            public const string EMBED = "EMBED";
            public const string OBJECT = "OBJECT";
            public const string PARAM = "PARAM";
            public const string VIDEO = "VIDEO";
            public const string AUDIO = "AUDIO";
            public const string SOURCE = "SOURCE";
            public const string TRACK = "TRACK";
            public const string CANVAS= "CANVAS";
            public const string MAP = "MAP";
            public const string AREA = "AREA";
            public const string MATH = "MATH"; 
            public const string SVG = "SVG"; 
            public const string APPLET = "APPLET";
            public const string FRAME = "FRAME";
            public const string FRAMESET = "FRAMESET";
            public const string NOFRAMES = "NOFRAMES";
            public const string BGSOUND = "BGSOUND";
            public const string NOEMBED = "NOEMBED";
            public const string PLAINTEXT = "PLAINTEXT";

            // TABLES

            public const string TABLE = "TABLE";
            public const string CAPTION = "CAPTION";
            public const string COLGROUP = "COLGROUP";
            public const string COL = "COL";
            public const string TBODY = "TBODY";
            public const string THEAD = "THEAD";
            public const string TFOOT = "TFOOT";
            public const string TR = "TR";
            public const string TD = "TD";
            public const string TH = "TH";

            // FORMS

            public const string FORM = "FORM";
            public const string FIELDSET = "FIELDSET";
            public const string LEGEND = "LEGEND";
            public const string LABEL = "LABEL";
            public const string INPUT = "INPUT";
            public const string BUTTON = "BUTTON";
            public const string SELECT = "SELECT";
            public const string DATALIST = "DATALIST";
            public const string OPTGROUP = "OPTGROUP";
            public const string OPTION = "OPTION";
            public const string TEXTAREA = "TEXTAREA";
            public const string KEYGEN = "KEYGEN";
            public const string OUTPUT = "OUTPUT";
            public const string PROGRESS = "PROGRESS";
            public const string METER = "METER";

            // INTERACTIVE

            public const string DETAILS = "DETAILS";
            public const string SUMMARY =  "SUMMARY";
            public const string COMMAND = "COMMAND";
            public const string MENU = "MENU";

            // ADDITIONAL
            public const string BDI = "BDI";
            public const string CONTENT = "CONTENT";
            public const string DATA = "DATA";
            public const string DIALOG = "DIALOG";
            public const string DIR = "DIR";
            public const string ELEMENT = "ELEMENT";
            public const string IMAGE = "IMAGE";
            public const string MAIN = "MAIN";
            public const string MENUITEM = "MENUITEM";
            public const string MULTICOL = "MULTICOL";
            public const string PICTURE = "PICTURE";
            public const string SHADOW = "SHADOW";
            public const string TEMPLATE = "TEMPLATE";

            public static readonly IEnumerable<string> ALL_TAG_NAMES;

            static Tags()
            {
                ALL_TAG_NAMES = new[]{
                    A,
                    ABBR,
                    ACRONYM,
                    ADDRESS,
                    APPLET,
                    AREA,
                    ARTICLE,
                    ASIDE,
                    AUDIO,
                    B,
                    BASE,
                    BASEFONT,
                    BDI,
                    BDO,
                    BGSOUND,
                    BIG,
                    BLINK,
                    BLOCKQUOTE,
                    BODY,
                    BR,
                    BUTTON,
                    CANVAS,
                    CAPTION,
                    CENTER,
                    CITE,
                    CODE,
                    COL,
                    COLGROUP,
                    COMMAND,
                    CONTENT,
                    DATA,
                    DATALIST,
                    DD,
                    DEL,
                    DETAILS,
                    DFN,
                    DIALOG,
                    DIR,
                    DIV,
                    DL,
                    DOCTYPE,
                    DT,
                    ELEMENT,
                    EM,
                    EMBED,
                    FIELDSET,
                    FIGCAPTION,
                    FIGURE,
                    FONT,
                    FOOTER,
                    FORM,
                    FRAME,
                    FRAMESET,
                    H1,
                    H2,
                    H3,
                    H4,
                    H5,
                    H6,
                    HEAD,
                    HEADER,
                    HGROUP,
                    HR,
                    HTML,
                    I,
                    IFRAME,
                    IMAGE,
                    IMG,
                    INPUT,
                    INS,
                    ISINDEX,
                    KBD,
                    KEYGEN,
                    LABEL,
                    LEGEND,
                    LI,
                    LINK,
                    LISTING,
                    MAIN,
                    MAP,
                    MARK,
                    MARQUEE,
                    MATH,
                    MENU,
                    MENUITEM,
                    META,
                    METER,
                    MULTICOL,
                    NAV,
                    NEXTID,
                    NOBR,
                    NOEMBED,
                    NOFRAMES,
                    NOSCRIPT,
                    OBJECT,
                    OL,
                    OPTGROUP,
                    OPTION,
                    OUTPUT,
                    P,
                    PARAM,
                    PICTURE,
                    PLAINTEXT,
                    PRE,
                    PROGRESS,
                    S,
                    SAMP,
                    SCRIPT,
                    SECTION,
                    SELECT,
                    SHADOW,
                    SMALL,
                    SOURCE,
                    SPACER,
                    SPAN,
                    STRIKE,
                    STRONG,
                    STYLE,
                    SUB,
                    SUMMARY,
                    SUP,
                    SVG,
                    TABLE,
                    TBODY,
                    TEMPLATE,
                    TD,
                    TEXTAREA,
                    TFOOT,
                    TH,
                    THEAD,
                    TIME,
                    TITLE,
                    TR,
                    TRACK,
                    TT,
                    Q,
                    RP,
                    RT,
                    RUBY,
                    U,
                    UL,
                    VAR,
                    VIDEO,
                    WBR,
                    XMP,
                };
            }
        }

        public static class Attributes
        {
            public const string CLASS = "class";
            public const string CLASSNAME = "classname";
            public const string NAME = "name";
            public const string STYLE = "style";
            public const string ALIGN = "align";
            public const string ID = "id";
            public const string HREF = "href";
        }

        public static class AttributeValues
        {
            public const string ALIGN_LEFT = "left";
            public const string ALIGN_RIGHT = "right";
            public const string ALIGN_CENTER = "center";
        }
    }
}
