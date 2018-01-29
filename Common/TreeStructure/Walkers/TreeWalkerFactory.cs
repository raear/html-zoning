/*
Government Usage Rights Notice:  The U.S. Government retains unlimited, royalty-free usage rights to this software, but not ownership, as provided by Federal law.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

•	Redistributions of source code must retain the above Government Usage Rights Notice, this list of conditions and the following disclaimer.

•	Redistributions in binary form must reproduce the above Government Usage Rights Notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

•	Neither the names of the National Library of Medicine, the National Institutes of Health, nor the names of any of the software developers may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE U.S. GOVERNMENT AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITEDTO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE U.S. GOVERNMENT
OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using Imppoa.HtmlZoning.TreeStructure.Filters;
using System.Collections.Generic;

namespace Imppoa.HtmlZoning.TreeStructure.Walkers
{
    /// <summary>
    /// Creates tree walkers
    /// </summary>
    public abstract class TreeWalkerFactory
    {
        /// <summary>
        /// Creates the tree walker
        /// </summary>
        /// <param name="filter">The filter to use</param>
        /// <returns>the tree walker</returns>
        public abstract TreeWalker Create(TreeNodeFilter filter);

        /// <summary>
        /// Creates a reversed walker
        /// </summary>
        /// <param name="filter">The filter to use</param>
        /// <returns>the reversed walker</returns>
        public TreeWalker CreateReversed(TreeNodeFilter filter)
        {
            return new ReversedWalker(this.Create(filter));
        }

        /// <summary>
        /// Creates the tree walker
        /// </summary>
        /// <param name="leafNodesOnly">if true, the walker will return only leaf nodes</param>
        /// <param name="typeFilter">if true, the walker will filter by type</param>
        /// <param name="type">The type to filter by</param>
        /// <param name="reverse">if true, the walker order will be reversed</param>
        /// <returns>
        /// the tree walker
        /// </returns>
        public TreeWalker Create(bool leafNodesOnly, bool reverse, bool typeFilter, string type)
        {
            return this.Create(leafNodesOnly, reverse, typeFilter, new string[] { type });
        }

        /// <summary>
        /// Creates the tree walker
        /// </summary>
        /// <param name="leafNodesOnly">if true, the walker will return only leaf nodes</param>
        /// <param name="typeFilter">if true, the walker will filter by type</param>
        /// <param name="types">The types to filter by</param>
        /// <param name="reverse">if true, the walker order will be reversed</param>
        /// <returns>
        /// the tree walker
        /// </returns>
        public TreeWalker Create(bool leafNodesOnly, bool reverse, bool typeFilter, IEnumerable<string> types)
        {
            var allFilter = new AllFilter();
            if (leafNodesOnly)
            {
                allFilter.Add(new LeafFilter());
            }
            if (typeFilter)
            {
                foreach (string type in types)
                {
                    allFilter.Add(new TypeFilter(type));
                }
            }

            TreeWalker walker;
            if (reverse)
            {
                walker = this.CreateReversed(allFilter);
            }
            else
            {
                walker = this.Create(allFilter);
            }

            return walker;
        }

        /// <summary>
        /// Creates
        /// </summary>
        /// <returns>the walker</returns>
        public TreeWalker Create()
        {
            return this.Create(false, false, false, string.Empty);
        }

        /// <summary>
        /// Creates a reversed walker
        /// </summary>
        /// <returns>the reversed walker</returns>
        public TreeWalker CreateReversed()
        {
            return this.Create(false, true, false, string.Empty);
        }

        /// <summary>
        /// Creates a leaf walker
        /// </summary>
        /// <returns>the leaf walker</returns>
        public TreeWalker CreateLeafWalker()
        {
            return this.Create(true, false, false, string.Empty);
        }

        /// <summary>
        /// Creates a type walker
        /// </summary>
        /// <param name="types">The types</param>
        /// <returns>
        /// the type walker
        /// </returns>
        public TreeWalker CreateTypeWalker(IEnumerable<string> types)
        {
            return this.Create(false, false, true, types);
        }

        /// <summary>
        /// Creates a type walker
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>
        /// the type walker
        /// </returns>
        public TreeWalker CreateTypeWalker(string type)
        {
            return this.CreateTypeWalker(new[] { type });
        }
    }
}
