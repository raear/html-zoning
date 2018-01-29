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
using Imppoa.HtmlZoning.TreeStructure.Walkers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imppoa.HtmlZoning.Visualization
{
    /// <summary>
    /// Wrapper for html elements
    /// </summary>
    internal class ElementTreeNode : VisualizerTreeNode
    {
        private HtmlElement _htmlElement;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementTreeNode"/> class
        /// </summary>
        /// <param name="htmlElement">The HTML element</param>
        public ElementTreeNode(HtmlElement htmlElement)
            : base(htmlElement)
        {
            _htmlElement = htmlElement;
        }

        /// <summary>
        /// Gets the node html
        /// </summary>
        public override string GetHtml()
        {
            return _htmlElement.OuterHtml;
        }

        /// <summary>
        /// Gets the node text
        /// </summary>
        public override string GetText()
        {
            return _htmlElement.OuterText;
        }

        /// <summary>
        /// Gets the node children
        /// </summary>
        public override IEnumerable<VisualizerTreeNode> GetChildren()
        {
            return _htmlElement.Children.Select(e => new ElementTreeNode(e));
        }

        /// <summary>
        /// Gets the node descendents
        /// </summary>
        public override IEnumerable<VisualizerTreeNode> GetDescendents()
        {
            return _htmlElement.GetDescendants().Select(e => new ElementTreeNode(e));
        }

        /// <summary>
        /// Gets the ordered leaf descendents for the node.
        /// Ordered by position in the html document
        /// </summary>
        public override IEnumerable<VisualizerTreeNode> GetOrderedLeafDescendents()
        {
            return _htmlElement.GetLeafNodes(new DepthFirstWalker()).Select(z => new ElementTreeNode(z));
        }

        /// <summary>
        /// Gets formatted text describing the node features
        /// </summary>
        public override string GetFeatures()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Id: " + _htmlElement.Id);
            stringBuilder.AppendLine("Bounding Box: " + _htmlElement.BoundingBox.ToString());
            foreach (var styleName in Css.Properties.PROPERTIES_OF_INTEREST)
            {
                var styleValue = _htmlElement.GetStyle(styleName);
                string styleText = string.Format("{0}: {1}, ", styleName, styleValue);
                stringBuilder.Append(styleText);
            }
            stringBuilder.Remove(stringBuilder.Length - 2, 2);
            return stringBuilder.ToString();
        }
    }
}
