/*
Government Usage Rights Notice:  The U.S. Government retains unlimited, royalty-free usage rights to this software, but not ownership, as provided by Federal law.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

•	Redistributions of source code must retain the above Government Usage Rights Notice, this list of conditions and the following disclaimer.

•	Redistributions in binary form must reproduce the above Government Usage Rights Notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

•	Neither the names of the National Library of Medicine, the National Institutes of Health, nor the names of any of the software developers may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE U.S. GOVERNMENT AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITEDTO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE U.S. GOVERNMENT
OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

namespace Imppoa.HtmlZoning.TreeStructure
{
    /// <summary>
    /// Generic tree node filter
    /// </summary>
    public abstract class TreeNodeFilter<T> : TreeNodeFilter where T : TreeNode
    {
        /// <summary>
        /// The accept node implementation
        /// </summary>
        /// <param name="node">The tree node</param>
        /// <returns>true, if the node matches, otherwise false</returns>
        protected override bool AcceptNodeImplementation(TreeNode node)
        {
            var castNode = node as T;

            bool acceptNode;
            if (castNode != null)
            {
                acceptNode = this.AcceptNode(castNode);
            }
            else
            {
                acceptNode = false;
            }

            return acceptNode;
        }

        /// <summary>
        /// Whether the tree node matches the filter
        /// </summary>
        /// <param name="node">The tree node of type T</param>
        /// <returns>
        /// true, if the node matches, otherwise false
        /// </returns>
        protected abstract bool AcceptNode(T node);
    }

    /// <summary>
    /// Filters tree nodes
    /// </summary>
    public abstract class TreeNodeFilter
    {
        /// <summary>
        /// The accept node implementation
        /// </summary>
        /// <param name="node">The tree node</param>
        /// <returns>true, if the node matches, otherwise false</returns>
        protected abstract bool AcceptNodeImplementation(TreeNode node);

        /// <summary>
        /// Whether the tree node matches the filter
        /// </summary>
        /// <param name="node">The tree node</param>
        /// <returns>
        /// true, if the node matches, otherwise false
        /// </returns>
        public bool AcceptNode(TreeNode node)
        {
            return this.AcceptNodeImplementation(node);
        }
    }
}
