/*
Government Usage Rights Notice:  The U.S. Government retains unlimited, royalty-free usage rights to this software, but not ownership, as provided by Federal law.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

•	Redistributions of source code must retain the above Government Usage Rights Notice, this list of conditions and the following disclaimer.

•	Redistributions in binary form must reproduce the above Government Usage Rights Notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

•	Neither the names of the National Library of Medicine, the National Institutes of Health, nor the names of any of the software developers may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE U.S. GOVERNMENT AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITEDTO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE U.S. GOVERNMENT
OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using Imppoa.HtmlZoning.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imppoa.HtmlZoning.Dom.Serialization
{
    /// <summary>
    /// Serializable document factory
    /// </summary>
    internal class SerializableDocumentFactory
    {
        /// <summary>
        /// Creates a serialzable document (alternative unused implementation)
        /// </summary>
        /// <param name="document">The html document</param>
        /// <param name="documentHtml">(out) The document HTML</param>
        /// <param name="documentText">(out) The document text</param>
        /// <returns>the serialzable document</returns>
        public SerializableDocument Create(HtmlDocument document, out string documentHtml, out string documentText)
        {
            var documentHtmlBuilder = new StringBuilder();
            var documentTextBuilder = new StringBuilder();

            var allSerializableElements = new List<SerializableElement>();
            var defaultStyleLookup = document.DefaultStyleLookup;
            CreateElements(document.Root, allSerializableElements, documentHtmlBuilder, documentTextBuilder, defaultStyleLookup);    
         
            var lookup = SerializationUtils.CreateLookup(allSerializableElements);
            documentHtml = documentHtmlBuilder.ToString();
            documentText = documentTextBuilder.ToString();
            foreach (var element in allSerializableElements)
            {
                element.Link(lookup);
                element.CropHtmlAndText(documentHtml, documentText);
            }

            return new SerializableDocument(allSerializableElements[0], document.Info, defaultStyleLookup, documentHtml, documentText);
        }

        /// <summary>
        /// Creates the serializable elements
        /// </summary>
        /// <param name="element">The original element</param>
        /// <param name="sElements">All serializable element</param>
        /// <param name="htmlBuilder">The HTML builder</param>
        /// <param name="textBuilder">The text builder</param>
        /// <param name="defaultStyleLookup">The default style lookup</param>
        public void CreateElements(HtmlElement element, List<SerializableElement> sElements, StringBuilder htmlBuilder, StringBuilder textBuilder, DefaultStyleLookup defaultStyleLookup)
        {
            // Create element
            var sElement = new SerializableElement();
            sElements.Add(sElement);

            // The variables need to be set
            int outerHtmlStartPos;
            int outerHtmlEndPos;
            int innerHtmlStartPos;
            int innerHtmlEndPos;
            int textStartPos;
            int textEndPos;

            // Record outer start positions
            outerHtmlStartPos = htmlBuilder.Length;
     
            // Add start tag & then record inner html start position
            htmlBuilder.Append(element.StartTag);
            innerHtmlStartPos = htmlBuilder.Length;

            // Add start text
            string startText;
            string endText;
            GetElementText(element, out startText, out endText);
            textBuilder.Append(startText);
            textStartPos = textBuilder.Length;

            // Add inner html/text
            if (element.ChildrenCount > 0)
            {
                foreach (HtmlElement child in element.Children)
                {
                    CreateElements(child, sElements, htmlBuilder, textBuilder, defaultStyleLookup);
                }
            }
            else
            { 
                htmlBuilder.Append(element.InnerHtml);
                textBuilder.Append(element.OuterText);
            }

            // Add end tag
            innerHtmlEndPos = htmlBuilder.Length;
            htmlBuilder.Append(element.EndTag);
            outerHtmlEndPos = htmlBuilder.Length;

            // Add end text
            textEndPos = textBuilder.Length;
            textBuilder.Append(endText);
        
            // Initalize element
            sElement.Initialize(element, outerHtmlStartPos, outerHtmlEndPos, innerHtmlStartPos, innerHtmlEndPos, textStartPos, textEndPos, defaultStyleLookup);
        }

        /// <summary>
        /// Gets the text that the element creates
        /// </summary>
        /// <param name="el">The element</param>
        /// <param name="startText">The start text</param>
        /// <param name="endText">The end text</param>
        private void GetElementText(HtmlElement el, out string startText, out string endText)
        {
            startText = string.Empty;
            endText = string.Empty;

            // No parent case
            if (!el.HasParent)
            {
                return;
            }

            // To be computed
            int startTextStartIndex = -1;
            int startTextEndIndex = -1;
            int endTextStartIndex = -1;
            int endTextEndIndex = -1;

            // Required variables
            var parent = el.Parent;
            int indexOfSelf = parent.Children.ToList().IndexOf(el);
            string parentText = parent.OuterText;

            // Compute child text indicies
            ComputeChildIndices(parent);

            if (el.HasLeftSibling)
            {
                int leftSiblingEndIndex = parent.UserData[indexOfSelf - 1].Item2;
                if (leftSiblingEndIndex > -1)
                {
                    startTextStartIndex = leftSiblingEndIndex;
                }
            }
            else
            {
                startTextStartIndex = 0;
            }

            int startIndex = parent.UserData[indexOfSelf].Item1;
            if (startIndex > -1)
            {
                startTextEndIndex = startIndex;
            }

            int endIndex = parent.UserData[indexOfSelf].Item2;
            if (endIndex > -1)
            {
                endTextStartIndex = endIndex;
                endTextEndIndex = endTextStartIndex;
            }

            if (!el.HasRightSibling)
            {
                endTextEndIndex = parentText.Length;
            }

            if (startTextStartIndex > -1 && startTextEndIndex > -1)
            {
                startText = parentText.GetSubstring(startTextStartIndex, startTextEndIndex);
            }

            if (endTextStartIndex > -1 && endTextEndIndex > -1)
            {
                endText = parentText.GetSubstring(endTextStartIndex, endTextEndIndex);
            }
        }

        /// <summary>
        /// Computes start/end indices of children text
        /// </summary>
        /// <param name="element">The html element</param>
        private void ComputeChildIndices(HtmlElement element)
        {
            if (element.UserData == null)
            {
                var childIndices = new List<Tuple<int, int>>();

                int startPos = 0;
                string outerText = element.OuterText;
                foreach (var child in element.Children)
                {
                    int startIdx = -1;
                    int endIdx = -1;

                    string childText = child.OuterText;
                    if (childText.Length != 0)
                    {
                        startIdx = outerText.GetIndexOf(childText, startPos);
                        if (startIdx > -1)
                        {
                            endIdx = startIdx + childText.Length;
                            startPos = endIdx;
                        }
                    }
                    else
                    {
                        startIdx = startPos;
                        endIdx = startPos;
                    }
                    childIndices.Add(new Tuple<int, int>(startIdx, endIdx));
                }

                element.UserData = childIndices;
            }
        }
    }
}
