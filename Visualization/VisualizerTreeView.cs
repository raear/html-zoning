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
using System.Windows.Forms;

namespace Imppoa.HtmlZoning.Visualization
{
    /// <summary>
    /// Custom tree view for visualizer application
    /// </summary>
    internal class VisualizerTreeView : TreeView
    {
        internal event EventHandler TreeDataUpdated;

        /// <summary>
        /// Displays leaf nodes
        /// </summary>
        /// <param name="rootNode">The root node</param>
        internal void DisplayLeafNodes(VisualizerTreeNode rootNode)
        {
            if (rootNode != null)
            {
                IEnumerable<VisualizerTreeNode> leafNodes = rootNode.GetOrderedLeafDescendents();

                TreeNode rootTreeViewNode = this.ConstructTreeViewNode(rootNode);
                foreach (VisualizerTreeNode leafNode in leafNodes)
                {
                    TreeNode treeViewNode = this.ConstructTreeViewNode(leafNode);
                    rootTreeViewNode.Nodes.Add(treeViewNode);
                }

                this.SetupTreeView(rootTreeViewNode);
            }
        }

        /// <summary>
        /// Displays the full tree
        /// </summary>
        /// <param name="rootNode">The root node</param>
        internal void DisplayFullTree(VisualizerTreeNode rootNode)
        {
            TreeNode rootTreeViewNode = this.ConstructTreeViewNode(rootNode);
            this.BuildTree(rootNode, rootTreeViewNode);
            this.SetupTreeView(rootTreeViewNode);
        }

        /// <summary>
        /// Notifies the tree view that its data has been updated
        /// </summary>
        internal void NotifyTreeDataUpdated()
        {
            this.Refresh();
            this.TreeDataUpdated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Sets up the Tree View
        /// </summary>
        /// <param name="rootTreeViewNode">The root tree view node</param>
        private void SetupTreeView(TreeNode rootTreeViewNode)
        {
            this.Nodes.Clear();
            this.Nodes.Add(rootTreeViewNode);
            this.ExpandAll();
        }

        /// <summary>
        /// Builds the tree
        /// </summary>
        /// <param name="treeNode">The tree node</param>
        /// <param name="treeViewNode">The tree view node</param>
        private void BuildTree(VisualizerTreeNode treeNode, TreeNode treeViewNode)
        {
            foreach (VisualizerTreeNode child in treeNode.GetChildren())
            {
                TreeNode treeViewChild = this.ConstructTreeViewNode(child);
                treeViewNode.Nodes.Add(treeViewChild);
                this.BuildTree(child, treeViewChild);
            }
        }

        /// <summary>
        /// Constructs a tree view node
        /// </summary>
        /// <param name="node">The tree node</param>
        /// <returns>a tree view node</returns>
        private TreeNode ConstructTreeViewNode(VisualizerTreeNode node)
        {
            string label = node.GetDisplayString();
            var treeNode = new TreeNode(label);
            treeNode.Tag = node;
            return treeNode;
        }
    }
}
