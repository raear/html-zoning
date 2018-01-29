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

namespace Imppoa.HtmlZoning.Utils
{
    internal static class ExtensionMethods
    {
        #region Dictionary

        /// <summary>
        /// Adds a non whitespace string value to the dictionary
        /// Null or whitespace values are ignored
        /// </summary>
        /// <param name="dict">The dictionary</param>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        public static void AddNonWhitespaceValue(this Dictionary<string, string> dict, string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                dict.AddOrOverwriteValue(key, value);
            }
        }

        /// <summary>
        /// Converts the dictionary to a case insensitive dictionary
        /// </summary>
        /// <param name="dict">The dictionary</param>
        /// <returns>The case insensitive dictionary</returns>
        public static Dictionary<string, string> ToCaseInsensitive(this IDictionary<string, string> dict)
        {
            var caseInsenstiveDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var keyValuePair in dict)
            {
                string key = keyValuePair.Key;
                string value = keyValuePair.Value;
                caseInsenstiveDictionary.AddOrOverwriteValue(key, value);
            }
            return caseInsenstiveDictionary;
        }

        /// <summary>
        /// Adds or overwrites dictionary value
        /// </summary>
        /// <param name="dict">The dictionary</param>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        public static void AddOrOverwriteValue(this Dictionary<string, string> dict, string key, string value)
        {
            bool keyExists = dict.ContainsKey(key);
            if (keyExists)
            {
                dict[key] = value;
            }
            else
            {
                dict.Add(key, value);
            }
        }

        #endregion

        #region Text

        /// <summary>
        /// Gets a substring
        /// </summary>
        /// <param name="str">The string</param>
        /// <param name="startPos">The start position</param>
        /// <param name="endPos">The end position</param>
        /// <returns>the substring</returns>
        public static string GetSubstring(this string str, int startPos, int endPos)
        {
            string subString = null;
            if (str != null)
            {
                if (startPos > -1 && endPos > -1)
                {
                    if (endPos >= startPos)
                    {
                        int length = endPos - startPos;
                        subString = str.Substring(startPos, length);
                    }
                }
            }
            return subString;
        }

        /// <summary>
        /// Delegates to String IndexOf method, using ordinal string comparison
        /// </summary>
        /// <param name="str">The string to search in</param>
        /// <param name="subStr">The sub string to search for</param>
        /// <param name="startIndex">The start index to search from</param>
        /// <returns>the start index of the substring or -1 if not found</returns>
        public static int GetIndexOf(this string str, string subStr, int startIndex = 0)
        {
            return str.IndexOf(subStr, startIndex, StringComparison.Ordinal);
        }

        #endregion
    }
}
