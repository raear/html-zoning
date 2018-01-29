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
    public static class Css
    {
        public static class Properties
        {
            public const string BACKGROUND_COLOR = "backgroundColor";
            public const string BACKGROUND_IMAGE = "backgroundImage";
            public const string BASELINE_SHIFT = "baselineShift";

            public const string BORDER_BOTTOM_COLOR = "borderBottomColor";
            public const string BORDER_BOTTOM_LEFT_RADIUS = "borderBottomLeftRadius";
            public const string BORDER_BOTTOM_RIGHT_RADIUS = "borderBottomRightRadius";
            public const string BORDER_BOTTOM_STYLE = "borderBottomStyle";
            public const string BORDER_BOTTOM_WIDTH = "borderBottomWidth";

            public const string BORDER_COLLAPSE = "borderCollapse";

            public const string BORDER_LEFT_COLOR = "borderLeftColor";
            public const string BORDER_LEFT_STYLE = "borderLeftStyle";
            public const string BORDER_LEFT_WIDTH = "borderLeftWidth";

            public const string BORDER_RIGHT_COLOR = "borderRightColor";
            public const string BORDER_RIGHT_STYLE = "borderRightStyle";
            public const string BORDER_RIGHT_WIDTH = "borderRightWidth";

            public const string BORDER_SPACING = "borderSpacing";

            public const string BORDER_TOP_COLOR = "borderTopColor";
            public const string BORDER_TOP_LEFT_RADIUS = "borderTopLeftRadius";
            public const string BORDER_TOP_RIGHT_RADIUS = "borderTopRightRadius";
            public const string BORDER_TOP_STYLE = "borderTopStyle";
            public const string BORDER_TOP_WIDTH = "borderTopWidth";

            public const string BOX_SHADOW = "boxShadow";
            public const string BOX_SIZING = "boxSizing";

            public const string CAPTION_SIDE = "captionSide";
            public const string CLEAR = "clear";
            public const string COLOR = "color";
            public const string COLUMN_COUNT = "columnCount";
            public const string COLUMN_FILL = "columnFill";
            public const string COLUMN_GAP = "columnGap";
            public const string COLUMN_RULE_COLOR = "columnRuleColor";
            public const string COLUMN_RULE_STYLE = "columnRuleStyle";
            public const string COLUMN_RULE_WIDTH = "columnRuleWidth";
            public const string COLUMN_SPAN = "columnSpan";
            public const string COLUMN_WIDTH = "columnWidth";
            public const string CSS_FLOAT = "cssFloat";

            public const string DISPLAY = "display";

            public const string EMPTY_CELLS = "emptyCells";

            public const string FONT_FAMILY = "fontFamily";
            public const string FONT_FEATURE_SETTINGS = "fontFeatureSettings";
            public const string FONT_SIZE = "fontSize";
            public const string FONT_SIZE_ADJUST = "fontSizeAdjust";
            public const string FONT_STRETCH = "fontStretch";
            public const string FONT_STYLE = "fontStyle";
            public const string FONT_VARIANT = "fontVariant";
            public const string FONT_WEIGHT = "fontWeight";

            public const string KERNING = "kerning";

            public const string LETTER_SPACING = "letterSpacing";
            public const string LINE_HEIGHT = "lineHeight";
            public const string LIST_STYLE_IMAGE = "listStyleImage";
            public const string LIST_STYLE_POSITION = "listStylePosition";
            public const string LIST_STYLE_TYPE = "listStyleType";

            public const string MARGIN_BOTTOM = "marginBottom";
            public const string MARGIN_LEFT = "marginLeft";
            public const string MARGIN_RIGHT = "marginRight";
            public const string MARGIN_TOP = "marginTop";
            public const string MARKER = "marker";

            public const string OPACITY = "opacity";
            public const string OUTLINE_COLOR = "outlineColor";
            public const string OUTLINE_STYLE = "outlineStyle";
            public const string OUTLINE_WIDTH = "outlineWidth";

            public const string OVERFLOWX = "overflow-x"; // Not of interest

            public const string PADDING_BOTTOM = "paddingBottom";
            public const string PADDING_LEFT = "paddingLeft";
            public const string PADDING_RIGHT = "paddingRight";
            public const string PADDING_TOP = "paddingTop";
            public const string POSITION = "position";

            public const string TABLE_LAYOUT = "tableLayout";
            public const string TEXT_ALIGN = "textAlign";
            public const string TEXT_ALIGN_LAST = "textAlignLast";
            public const string TEXT_DECORATION = "textDecoration";
            public const string TEXT_INDENT = "textIndent";
            public const string TEXT_JUSTIFY = "textJustify";
            public const string TEXT_SHADOW = "textShadow";
            public const string TEXT_TRANSFORM = "textTransform";
            public const string TEXT_UNDERLINE_POSITION = "textUnderlinePosition";

            public const string VERTICAL_ALIGN = "verticalAlign";
            public const string VISIBILITY = "visibility";

            public const string WORD_SPACING = "wordSpacing";

            public const string ZINDEX = "zIndex";

            public static readonly IEnumerable<string> PROPERTIES_OF_INTEREST;

            static Properties()
            {
                PROPERTIES_OF_INTEREST = new[]
                {
                    BACKGROUND_COLOR,
                    BACKGROUND_IMAGE,
                    BASELINE_SHIFT,

                    BORDER_BOTTOM_COLOR,
                    BORDER_BOTTOM_LEFT_RADIUS,
                    BORDER_BOTTOM_RIGHT_RADIUS,
                    BORDER_BOTTOM_STYLE,
                    BORDER_BOTTOM_WIDTH,

                    BORDER_COLLAPSE,

                    BORDER_LEFT_COLOR,
                    BORDER_LEFT_STYLE,
                    BORDER_LEFT_WIDTH,

                    BORDER_RIGHT_COLOR,
                    BORDER_RIGHT_STYLE,
                    BORDER_RIGHT_WIDTH,

                    BORDER_SPACING,

                    BORDER_TOP_COLOR,
                    BORDER_TOP_LEFT_RADIUS,
                    BORDER_TOP_RIGHT_RADIUS,
                    BORDER_TOP_STYLE,
                    BORDER_TOP_WIDTH,

                    BOX_SHADOW,
                    BOX_SIZING,

                    CAPTION_SIDE,
                    CLEAR,
                    COLOR,
                    COLUMN_COUNT,
                    COLUMN_FILL,
                    COLUMN_GAP,
                    COLUMN_RULE_COLOR,
                    COLUMN_RULE_STYLE,
                    COLUMN_RULE_WIDTH,
                    COLUMN_SPAN,
                    COLUMN_WIDTH,
                    CSS_FLOAT,

                    DISPLAY,

                    EMPTY_CELLS,

                    FONT_FAMILY,
                    FONT_FEATURE_SETTINGS,
                    FONT_SIZE,
                    FONT_SIZE_ADJUST,
                    FONT_STRETCH,
                    FONT_STYLE,
                    FONT_VARIANT,
                    FONT_WEIGHT,

                    KERNING,

                    LETTER_SPACING,
                    LINE_HEIGHT,
                    LIST_STYLE_IMAGE,
                    LIST_STYLE_POSITION,
                    LIST_STYLE_TYPE,

                    MARGIN_BOTTOM,
                    MARGIN_LEFT,
                    MARGIN_RIGHT,
                    MARGIN_TOP,
                    MARKER,

                    OPACITY,
                    OUTLINE_COLOR,
                    OUTLINE_STYLE,
                    OUTLINE_WIDTH,

                    PADDING_BOTTOM,
                    PADDING_LEFT,
                    PADDING_RIGHT,
                    PADDING_TOP,
                    POSITION,

                    TABLE_LAYOUT,
                    TEXT_ALIGN,
                    TEXT_ALIGN_LAST,
                    TEXT_DECORATION,
                    TEXT_INDENT,
                    TEXT_JUSTIFY,
                    TEXT_SHADOW,
                    TEXT_TRANSFORM,
                    TEXT_UNDERLINE_POSITION,

                    VERTICAL_ALIGN,
                    VISIBILITY,

                    WORD_SPACING,

                    ZINDEX,
                };
            }
        }

        public static class Values
        {
            public const string DISPLAY_INLINE = "inline";
            public const string DISPLAY_BLOCK = "block";
            public const string DISPLAY_FLEX = "flex";
            public const string DISPLAY_LIST_ITEM = "list-item";
            public const string DISPLAY_RUN_IN = "run-in";
            public const string DISPLAY_INLINE_BLOCK = "inline-block";
            public const string DISPLAY_TABLE = "table";
            public const string DISPLAY_INLINE_FLEX = "inline-flex";
            public const string DISPLAY_INLINE_TABLE = "inline-table";
            public const string DISPLAY_MS_FLEX = "-ms-flexbox";
            public const string DISPLAY_MS_GRID = "-ms-grid";
            public const string DISPLAY_MS_INLINE_FLEX = "-ms-inline-flexbox";
            public const string DISPLAY_MS_INLINE_GRID = "-ms-inline-grid";
            public const string DISPLAY_TABLE_ROW_GROUP = "table-row-group";
            public const string DISPLAY_TABLE_HEADER_GROUP = "table-header-group";
            public const string DISPLAY_TABLE_FOOTER_GROUP = "table-footer-group";
            public const string DISPLAY_TABLE_ROW = "table-row";
            public const string DISPLAY_TABLE_COLUMN_GROUP = "table-column-group";
            public const string DISPLAY_TABLE_COLUMN = "table-column";
            public const string DISPLAY_TABLE_CELL = "table-cell";
            public const string DISPLAY_TABLE_CAPTION = "table-caption";
            public const string DISPLAY_NONE = "none";

            public const string OVERFLOW_HIDDEN = "hidden";
            public const string OVERFLOW_VISIBLE = "visible";

            public const string POSITION_FIXED = "fixed";

            public const string VISIBILITY_HIDDEN = "hidden";
            public const string VISIBILITY_COLLAPSE = "collapse";

            public const string WHITESPACE_PRE = "pre";
            public const string WHITESPACE_PRE_LINE = "pre-line";
            public const string WHITESPACE_PRE_WRAP = "pre-wrap";

            public const string LEFT_1EM = "1em";
            public const string LEFT_AUTO = "auto";
        }
    }
}
