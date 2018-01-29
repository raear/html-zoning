/*
Government Usage Rights Notice:  The U.S. Government retains unlimited, royalty-free usage rights to this software, but not ownership, as provided by Federal law.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

•	Redistributions of source code must retain the above Government Usage Rights Notice, this list of conditions and the following disclaimer.

•	Redistributions in binary form must reproduce the above Government Usage Rights Notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

•	Neither the names of the National Library of Medicine, the National Institutes of Health, nor the names of any of the software developers may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE U.S. GOVERNMENT AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITEDTO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE U.S. GOVERNMENT
OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using Imppoa.HtmlZoning.TreeStructure.Walkers;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imppoa.HtmlZoning.Visualization
{
    /// <summary>
    /// Wrapper for column node
    /// </summary>
    internal class ColumnTreeNode : VisualizerTreeNode
    {
        private Column _column;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnTreeNode"/> class.
        /// </summary>
        /// <param name="column">The column.</param>
        public ColumnTreeNode(Column column)
            : base(column)
        {
            _column = column;
        }

        /// <summary>
        /// Gets the node html
        /// </summary>
        public override string GetHtml()
        {
            return _column.Html;
        }

        /// <summary>
        /// Gets the node text
        /// </summary>
        public override string GetText()
        {
            return string.Empty;
        }

        /// <summary>
        /// Gets the node children
        /// </summary>
        public override IEnumerable<VisualizerTreeNode> GetChildren()
        {
            return _column.Children.Select(z => new ColumnTreeNode(z));
        }

        /// <summary>
        /// Gets the node descendents
        /// </summary>
        public override IEnumerable<VisualizerTreeNode> GetDescendents()
        {
            return _column.GetDescendants().Select(z => new ColumnTreeNode(z));
        }

        /// <summary>
        /// Gets the ordered leaf descendents for the node
        /// Ordered by position in the html document
        /// </summary>
        public override IEnumerable<VisualizerTreeNode> GetOrderedLeafDescendents()
        {
            return _column.GetLeafNodes(new DepthFirstWalker()).Select(z => new ColumnTreeNode(z));
        }

        /// <summary>
        /// Gets formatted text describing the node features
        /// </summary>
        public override string GetFeatures()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Id: " + _column.Id);
            stringBuilder.AppendLine("Bounding Box: " + _column.BoundingBox.ToString());
            stringBuilder.AppendLine(string.Format("Zones: {0}", string.Join(",", _column.Zones.Select(z => z.TagString))));
            foreach (var name in _column.FeatureNames)
            {
                var feature = _column.GetFeature(name);
                stringBuilder.AppendLine(feature.ToString());
            }
            return stringBuilder.ToString();
        }
    }
}
