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

namespace Imppoa.HtmlZoning.ColumnTreeConstruction
{
    /// <summary>
    /// Builds a column tree
    /// </summary>
    public class ColumnTreeBuilder
    {
        public static readonly double DEFAULT_TOLERANCE = 0.1;

        /// <summary>
        /// Creates an instance with the default configuration
        /// </summary>
        /// <returns>A ColumnTreeBuilder instance with the default configuration</returns>  
        public static ColumnTreeBuilder Create()
        {
            return new ColumnTreeBuilder(DEFAULT_TOLERANCE);
        }

        private readonly double _tolerance;
        private int _count;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnTreeBuilder"/> class
        /// </summary>
        /// <param name="tol">The relative tolerance for the column left and right positions</param>
        public ColumnTreeBuilder(double tol)
        {
            _tolerance = tol;
        }

        /// <summary>
        /// Builds a column tree
        /// </summary>
        /// <param name="zoneTree">The zone tree</param>
        /// <param name="collapseBranches">Whether to collapse branches and merge children</param>
        /// <returns>
        /// The created column tree
        /// </returns>
        public ColumnTree Build(ZoneTree zoneTree, bool collapseBranches)
        {
            _count = 0;
            var rootNode = this.CreateColumn(null, zoneTree.Root);
            this.Build(rootNode);
            var tree = new ColumnTree(rootNode, zoneTree);

            if (collapseBranches)
            {
                tree.Accept(new CollapseBranches(_tolerance), new BreadthFirstWalkerFactory().CreateReversed());
            }
          
            return tree;
        }

        /// <summary>
        /// Builds the column tree (recursive function)
        /// </summary>
        /// <param name="parent">The parent to break down</param>
        private void Build(Column parent)
        {
            foreach (var parentZone in parent.Zones)
            {
                Column columnChild = null;
                foreach (var childZone in parentZone.Children)
                {
                    if (columnChild == null)
                    {
                        columnChild = this.CreateColumnChild(parent, childZone);
                    }
                    else
                    {
                        bool mergeSuccessful = columnChild.TryAppend(childZone, _tolerance);
                        if (!mergeSuccessful)
                        {
                            columnChild = this.CreateColumnChild(parent, childZone);
                        }
                    }
                }
            }

            foreach (var child in parent.Children)
            {
                this.Build(child);
            }
        }

        /// <summary>
        /// Creates a column child and appends it to the supplied parent
        /// </summary>
        /// <param name="parent">The parent</param>
        /// <param name="initialZone">The initial zone</param>
        /// <returns>the created child</returns>
        private Column CreateColumnChild(Column parent, Zone initialZone)
        {
            var child = this.CreateColumn(parent, initialZone);
            parent.AddChild(child);
            return child;
        }

        /// <summary>
        /// Create a column with a unique id
        /// </summary>
        /// <param name="parent">The parent</param>
        /// <param name="initialZone">The initial zone</param>
        /// <returns>
        /// The created object
        /// </returns>
        private Column CreateColumn(Column parent, Zone initialZone)
        {
            _count++;
            var column = new Column(_count, parent, initialZone);
            return column;
        }
    }
}
