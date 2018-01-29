/*
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
using System.Collections.Generic;
using System.Drawing;

namespace Imppoa.HtmlRendering
{
    /// <summary>
    /// MsHtml element
    /// </summary>
    public class MsHtmlElement : HtmlElement
    {
        private int? _offsetParentId;
        private Rectangle _offsetRectangle;
        
        private MsHtmlElement _offsetParent;

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlElement" /> class
        /// </summary>
        /// <param name="id">The id</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="parentId">The parent id</param>
        /// <param name="childrenIds">The children ids</param>
        /// <param name="classifications">The classifications</param>
        /// <param name="tagName">The tag name</param>
        /// <param name="outerHtml">The outer html</param>
        /// <param name="innerHtml">The inner html</param>
        /// <param name="text">The text</param>
        /// <param name="attributes">The attributes</param>
        /// <param name="styles">The styles</param>
        /// <param name="defaultStyleLookup">The default style lookup</param>
        /// <param name="offsetParentId">The offset parent id</param>
        /// <param name="offsetRectangle">The offset rectangle</param>
        public MsHtmlElement(int id, int displayOrder, int? parentId, IReadOnlyList<int> childrenIds, IEnumerable<string> classifications,
            string tagName, string outerHtml, string innerHtml, string text,
            IDictionary<string, string> attributes, IDictionary<string, string> styles, DefaultStyleLookup defaultStyleLookup,
            int? offsetParentId, Rectangle offsetRectangle)
            : base(id, id, parentId, childrenIds, Rectangle.Empty, classifications, tagName, outerHtml, innerHtml, text, attributes, styles, defaultStyleLookup)
        {
            _offsetParentId = offsetParentId;
            _offsetRectangle = offsetRectangle;
        }

        /// <summary>
        /// Creates the tree links
        /// </summary>
        /// <param name="lookup">The tree node lookup</param>
        public override void Link(IDictionary<int,TreeNode> lookup)
        {
            base.Link(lookup);

            if (_offsetParentId.HasValue)
            {
                _offsetParent = (MsHtmlElement) lookup[_offsetParentId.Value];
            }

            this.ComputeBoundingBox();
        }

        /// <summary>
        /// Computes the bounding box
        /// from the offset parent and offset rectangle
        /// </summary>
        private void ComputeBoundingBox()
        {
            Rectangle boundingBox = _offsetRectangle;
            MsHtmlElement offsetParent = _offsetParent;
            while (offsetParent != null)
            {
                boundingBox.X += offsetParent._offsetRectangle.X;
                boundingBox.Y += offsetParent._offsetRectangle.Y;
                offsetParent = offsetParent._offsetParent;
            }

            this.BoundingBox = boundingBox;
        }
    }
}
