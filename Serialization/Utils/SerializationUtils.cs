/*
Government Usage Rights Notice:  The U.S. Government retains unlimited, royalty-free usage rights to this software, but not ownership, as provided by Federal law.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

•	Redistributions of source code must retain the above Government Usage Rights Notice, this list of conditions and the following disclaimer.

•	Redistributions in binary form must reproduce the above Government Usage Rights Notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

•	Neither the names of the National Library of Medicine, the National Institutes of Health, nor the names of any of the software developers may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE U.S. GOVERNMENT AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITEDTO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE U.S. GOVERNMENT
OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using Imppoa.HtmlZoning.TreeStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Imppoa.HtmlZoning.Utils
{
    /// <summary>
    /// Serialization helper methods
    /// </summary>
    internal static class SerializationUtils
    {
        /// <summary>
        /// Converts tree node collection to tree node lookup
        /// </summary>
        /// <param name="treeNodes">The tree node collection</param>
        /// <returns>the tree node lookup</returns>
        public static Dictionary<int, TreeNode> CreateLookup(IEnumerable<TreeNode> treeNodes)
        {
            return treeNodes.ToDictionary(n => n.Id);
        }

        /// <summary>
        /// Reads the element content as a nullable integer
        /// </summary>
        /// <param name="reader">The xml reader</param>
        /// <returns>
        /// the element content as a nullable integer
        /// </returns>
        public static int? ReadElementContentAsNullableInt(this XmlReader reader)
        {
            int? contents = null;
            string stringContents = reader.ReadElementContentAsString();
            if (!string.IsNullOrWhiteSpace(stringContents))
            {
                contents = int.Parse(stringContents);
            }
            return contents;
        }

        /// <summary>
        /// Reads the element contents as integer list
        /// </summary>
        /// <param name="reader">The xml reader</param>
        /// <returns>the element contents as integer list</returns>
        public static IReadOnlyList<int> ReadElementContentAsIntList(this XmlReader reader)
        {
            string elementContent = reader.ReadElementContentAsString();
            var stringIdList = elementContent.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            return stringIdList.Select(sId => int.Parse(sId)).ToList();
        }

        /// <summary>
        /// Reads the element content as a string list
        /// </summary>
        /// <param name="reader">The xml reader</param>
        /// <returns>the element content as a string list</returns>
        public static IReadOnlyList<string> ReadElementContentAsStringArray(this XmlReader reader)
        {
            string innerXml = reader.ReadElementContentAsString();
            string[] classificationStrings = innerXml.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            return classificationStrings.ToList();
        }

        /// <summary>
        /// Writes an integer list
        /// </summary>
        /// <param name="writer">The xml writer</param>
        /// <param name="elementName">The xml element name</param>
        /// <param name="list">The integer list</param>
        public static void WriteIntList(this XmlWriter writer, string elementName, IReadOnlyList<int> list)
        {
            writer.WriteElementString(elementName, string.Join(",", list));
        }

        /// <summary>
        /// Writes a string list
        /// </summary>
        /// <param name="writer">The xml writer</param>
        /// <param name="elementName">The xml element name</param>
        /// <param name="list">The string list</param>
        public static void WriteStringList(this XmlWriter writer, string elementName, IReadOnlyList<string> list)
        {
            writer.WriteElementString(elementName, string.Join(", ", list));
        }
    }
}
