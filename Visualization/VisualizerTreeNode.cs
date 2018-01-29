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
using Imppoa.HtmlZoning.TreeStructure.Walkers;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Imppoa.HtmlZoning.Visualization
{
    /// <summary>
    /// Wrapper interface for DOM and zone nodes
    /// </summary>
    internal abstract class VisualizerTreeNode
    {
        private TreeNode _treeNode;

        /// <summary>
        /// Initializes a new instance of the <see cref="VisualizerTreeNode"/> class.
        /// </summary>
        /// <param name="treeNode">The tree node</param>
        public VisualizerTreeNode(TreeNode treeNode)
        {
            _treeNode = treeNode;
        }

        /// <summary>
        /// Gets the node html
        /// </summary>
        public abstract string GetHtml();

        /// <summary>
        /// Gets the node text
        /// </summary>
        public abstract string GetText();

        /// <summary>
        /// Gets the node children
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<VisualizerTreeNode> GetChildren();

        /// <summary>
        /// Gets the node descendents
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<VisualizerTreeNode> GetDescendents();

        /// <summary>
        /// Gets the ordered leaf descendents for the node.
        /// Ordered by position in the html document
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<VisualizerTreeNode> GetOrderedLeafDescendents();

        /// <summary>
        /// Gets formatted text describing the node features
        /// </summary>
        public abstract string GetFeatures();

        /// <summary>
        /// Gets the display string for the node
        /// </summary>
        public virtual string GetDisplayString()
        {
            return _treeNode.GetDisplayString();
        }

        /// <summary>
        /// Gets the bounding box
        /// </summary>
        /// <value>
        /// The bounding box
        /// </value>
        public Rectangle BoundingBox
        {
            get
            {
                return _treeNode.BoundingBox;
            }
        }

        /// <summary>
        /// Whether the node is a leaf node
        /// </summary>
        public bool IsLeaf()
        {
            return _treeNode.IsLeaf();
        }

        /// <summary>
        /// Gets the set of classifications for the tree
        /// </summary>
        public IEnumerable<string> GetAllLabels()
        {
            var labels = new List<string>();
            var depthFirstWalker = new DepthFirstWalkerFactory().Create();
            foreach (var node in _treeNode.GetDescendantsAndSelf(depthFirstWalker))
            {
                labels.AddRange(node.Classifications);
            }
            return labels.Distinct();
        }

        /// <summary>
        /// Get the bounding boxes for nodes with any of the specified labels
        /// </summary>
        /// <param name="labels">The labels</param>
        /// <param name="leafNodesOnly">Whether to return bounding boxes only for leaf nodes</param>
        /// <returns></returns>
        public IEnumerable<Rectangle> GetBoundingBoxesForLabels(IEnumerable<string> labels, bool leafNodesOnly)
        {
            IEnumerable<VisualizerTreeNode> descendents;
            if (leafNodesOnly)
            {
                descendents = this.GetOrderedLeafDescendents();
            }
            else
            {
                descendents = this.GetDescendents();
            }
            var bbSet = new HashSet<Rectangle>();
            foreach (var label in labels)
            {
                var nodes = descendents.Where(n => n.HasLabel(label));
                var boundingBoxes = nodes.Select(n => n.BoundingBox);
                bbSet.UnionWith(boundingBoxes);
            }
            return bbSet;
        }

        /// <summary>
        /// Gets the underlying tree node
        /// </summary>
        /// <returns>the underlying tree node</returns>
        public TreeNode GetTreeNode()
        {
            return _treeNode;
        }

        /// <summary>
        /// Determines whether the tree node has the label
        /// </summary>
        /// <param name="label">The label</param>
        private bool HasLabel(string label)
        {
            return _treeNode.HasClassification(label);
        }
    }
}
