﻿/*
Government Usage Rights Notice:  The U.S. Government retains unlimited, royalty-free usage rights to this software, but not ownership, as provided by Federal law.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

•	Redistributions of source code must retain the above Government Usage Rights Notice, this list of conditions and the following disclaimer.

•	Redistributions in binary form must reproduce the above Government Usage Rights Notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

•	Neither the names of the National Library of Medicine, the National Institutes of Health, nor the names of any of the software developers may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE U.S. GOVERNMENT AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITEDTO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE U.S. GOVERNMENT
OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using Imppoa.HtmlZoning.Dom;
using Imppoa.HtmlZoning.TreeStructure;
using System;

namespace Imppoa.HtmlZoning.ElementClassification
{
    /// <summary>
    /// Filters significant linebreak elements - that break up significant text
    /// </summary>
    public class SignificantLinebreakFilter : TreeNodeFilter<HtmlElement>
    {
        private readonly string _linebreakType;
        private readonly string _significantTextType;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignificantLinebreakFilter"/> class.
        /// </summary>
        /// <param name="linebreakType">Linebreak type</param>
        /// <param name="significantTextType">Significant text type</param>
        public SignificantLinebreakFilter(string linebreakType, string significantTextType)
        {
            _linebreakType = linebreakType;
            _significantTextType = significantTextType;
        }

        /// <summary>
        /// Whether to accept the html element
        /// </summary>
        /// <param name="htmlElement">The html element</param>
        /// <returns>true, if the element matches, otherwise false</returns>
        protected override bool AcceptNode(HtmlElement htmlElement)
        {
            bool acceptNode = false;

            bool isLinebreakElement = htmlElement.HasClassification(_linebreakType);
            if (isLinebreakElement)
            {
                bool leftSiblingIsSignificantText = this.SiblingIsSignificantText(htmlElement, (e) => e.LeftSibling);
                bool rightSiblingIsSignificantText = this.SiblingIsSignificantText(htmlElement, (e) => e.RightSibling);
                acceptNode = leftSiblingIsSignificantText && rightSiblingIsSignificantText;
            }

            return acceptNode;
        }

        /// <summary>
        /// Determines whether the next (left or right) sibling is significant text
        /// </summary>
        /// <param name="htmlElement">The html element</param>
        /// <param name="iterator">The iterator to get the next (left or right) sibling</param>
        /// <returns>true, if the next sibling is significant text, false otherwise</returns>
        private bool SiblingIsSignificantText(HtmlElement htmlElement, Func<HtmlElement, HtmlElement> iterator)
        {
            bool result = false;

            var sibling = iterator(htmlElement);
            while(sibling != null)
            {
                bool isLinebreakElement = sibling.HasClassification(_linebreakType);
                bool isSignificantText = sibling.HasClassification(_significantTextType);
    
                if (isSignificantText && !isLinebreakElement)
                {
                    result = true;
                    break;
                }

                sibling = iterator(sibling);
            }

            return result;
        }
    }
}
