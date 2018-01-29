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
using System.Linq;

namespace Imppoa.HtmlZoning.ColumnTreeConstruction
{
    /// <summary>
    /// Collapses branches if in the same column
    /// </summary>
    public class CollapseBranches : TreeNodeVisitor<Column>
    {
        private double _tol;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollapseBranches" /> class
        /// </summary>
        /// <param name="tol">The relative tolerance used to determine if branches can be collapsed</param>
        public CollapseBranches(double tol) 
            : base ()
        {
            _tol = tol;
        }

        /// <summary>
        /// Visits the tree node
        /// </summary>
        /// <param name="node">The tree node</param>
        public override void Visit(Column parent)
        {
            foreach (var child in parent.Children)
            {
                if (parent.WithinTolerance(child, _tol))
                {
                    int indexOfChild = parent.Children.ToList().IndexOf(child);
                    parent.RemoveChild(child);
                    var grandChildren = child.Children;
                    foreach (var grandChild in grandChildren)
                    {
                        grandChild.SetParent(parent);
                    }
                    parent.InsertChildrenAt(indexOfChild, grandChildren);
                }
            }
        }
    }
}
