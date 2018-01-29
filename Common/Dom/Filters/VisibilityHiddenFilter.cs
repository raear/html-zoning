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

namespace Imppoa.HtmlZoning.Dom.Filters
{
    /// <summary>
    /// Filters hidden html elements based on the css visibility style
    /// </summary>
    public class VisibilityHiddenFilter : TreeNodeFilter<HtmlElement>
    {
        /// <summary>
        /// Whether to accept the html element
        /// </summary>
        /// <param name="htmlElement">The html element</param>
        /// <returns>true, if the element matches, otherwise false</returns>
        protected override bool AcceptNode(HtmlElement htmlElement)
        {
            string style = this.GetVisibilityStyle(htmlElement);
            return this.IsHiddenStyle(style);
        }

        /// <summary>
        /// Gets the visibility style for the DOM node
        /// </summary>
        /// <param name="domNode">The DOM node</param>
        /// <returns>the visibility style</returns>
        private string GetVisibilityStyle(HtmlElement domNode)
        {
            string style = domNode.GetStyle(Css.Properties.VISIBILITY);
            style = style.ToLower().Trim();
            return style;
        }

        /// <summary>
        /// Whether the visibility style will hide the element
        /// </summary>
        /// <param name="style">The style</param>
        /// <returns>Whether is hidden style</returns>
        private bool IsHiddenStyle(string style)
        {
            return style == Css.Values.VISIBILITY_HIDDEN ||
                   style == Css.Values.VISIBILITY_COLLAPSE;
        }
    }
}
