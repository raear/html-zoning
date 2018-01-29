/*
Government Usage Rights Notice:  The U.S. Government retains unlimited, royalty-free usage rights to this software, but not ownership, as provided by Federal law.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

•	Redistributions of source code must retain the above Government Usage Rights Notice, this list of conditions and the following disclaimer.

•	Redistributions in binary form must reproduce the above Government Usage Rights Notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

•	Neither the names of the National Library of Medicine, the National Institutes of Health, nor the names of any of the software developers may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE U.S. GOVERNMENT AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITEDTO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE U.S. GOVERNMENT
OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace Imppoa.HtmlZoning.Dom
{
    public static class CssHelper
    {
        private static readonly IEnumerable<string> IN_LINE_DISPLAY_VALUES = new[]
            {
                Css.Values.DISPLAY_INLINE_FLEX,
                Css.Values.DISPLAY_INLINE,
                Css.Values.DISPLAY_INLINE_BLOCK,
                Css.Values.DISPLAY_INLINE_TABLE,
                Css.Values.DISPLAY_MS_INLINE_FLEX,
                Css.Values.DISPLAY_MS_INLINE_GRID,
                Css.Values.DISPLAY_TABLE_CELL,
                Css.Values.DISPLAY_RUN_IN,
            };

        private static readonly IEnumerable<string> BLOCK_DISPLAY_VALUES = new[]
        {
                Css.Values.DISPLAY_BLOCK,
                Css.Values.DISPLAY_FLEX,
                Css.Values.DISPLAY_LIST_ITEM,
                Css.Values.DISPLAY_MS_FLEX,
                Css.Values.DISPLAY_MS_GRID,
                Css.Values.DISPLAY_TABLE,
                Css.Values.DISPLAY_TABLE_ROW,
                Css.Values.DISPLAY_TABLE_CAPTION,
                Css.Values.DISPLAY_TABLE_HEADER_GROUP,
                Css.Values.DISPLAY_TABLE_ROW_GROUP,
                Css.Values.DISPLAY_TABLE_FOOTER_GROUP,
            };

        private static readonly IEnumerable<string> INVISIBLE_DISPLAY_VALUES = new[]
        {
                Css.Values.DISPLAY_NONE,
                Css.Values.DISPLAY_TABLE_COLUMN_GROUP,
                Css.Values.DISPLAY_TABLE_COLUMN,
            };

        /// <summary>
        /// Whether display value is an inline display value
        /// </summary>
        /// <param name="value">The value</param>
        public static bool IsInlineDisplayValue(string value)
        {
            return IN_LINE_DISPLAY_VALUES.Contains(value, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Whether display value is a block display value
        /// </summary>
        /// <param name="value">The value</param>
        public static bool IsBlockDisplayValue(string value)
        {
            return BLOCK_DISPLAY_VALUES.Contains(value, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Whether display value is an invisible display value
        /// </summary>
        /// <param name="value">The value</param>
        public static bool IsNonRenderedDisplayValue(string value)
        {
            return INVISIBLE_DISPLAY_VALUES.Contains(value, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Whether display value is display none
        /// </summary>
        /// <param name="displayValue">The display value</param>
        /// <returns>true, if the value is display none, otherwise false</returns>
        public static bool IsDisplayNone(string displayValue)
        {
            return displayValue.Trim().ToLower() == Css.Values.DISPLAY_NONE.ToLower();
        }
    }
}
