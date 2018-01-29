/*
Government Usage Rights Notice:  The U.S. Government retains unlimited, royalty-free usage rights to this software, but not ownership, as provided by Federal law.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

•	Redistributions of source code must retain the above Government Usage Rights Notice, this list of conditions and the following disclaimer.

•	Redistributions in binary form must reproduce the above Government Usage Rights Notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

•	Neither the names of the National Library of Medicine, the National Institutes of Health, nor the names of any of the software developers may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE U.S. GOVERNMENT AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITEDTO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE U.S. GOVERNMENT
OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

namespace Imppoa.HtmlZoning.TreeStructure.Visitors
{
    /// <summary>
    /// Visits and classifies tree nodes
    /// </summary>
    public class TreeNodeClassifier : TreeNodeVisitor
    {
        private string _type;
        private TreeNodeFilter _filter;
        private bool _firstOnly;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNodeClassifier" /> class
        /// </summary>
        /// <param name="type">The classification type</param>
        /// <param name="filter">The filter defining the type</param>
        /// <param name="firstOnly">Whether to classify only the first occurrence</param>
        public TreeNodeClassifier(string type, TreeNodeFilter filter, bool firstOnly = false)
            : base()
        {
            this.Initialize(type, filter, firstOnly);
        }

        /// <summary>
        /// Initializes the class
        /// </summary>
        /// <param name="type">The classification type</param>
        /// <param name="filter">The filter defining the type</param>
        /// <param name="firstOnly">Whether to classify only the first occurrence</param>
        private void Initialize(string type, TreeNodeFilter filter, bool firstOnly)
        {
            _type = type;
            _filter = filter;
            _firstOnly = firstOnly;
        }

        /// <summary>
        /// The visit implementation
        /// </summary>
        /// <param name="node">The tree node</param>
        public override void Visit(TreeNode node)
        {
            this.Classify(node);
        }

        /// <summary>
        /// Classifies the node
        /// </summary>
        /// <param name="node">The classifiable node</param>
        private void Classify(TreeNode node)
        {
            if (_filter.AcceptNode(node))
            {
                node.AddClassification(_type);
                if (_firstOnly)
                {
                    this.Stop = true;
                }
            }
        }
    }
}
