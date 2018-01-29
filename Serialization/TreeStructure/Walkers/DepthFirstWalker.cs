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
using System.Collections;
using System.Collections.Generic;

namespace Imppoa.HtmlZoning.TreeStructure.Walkers
{
    /// <summary>
    /// Walks the tree depth first
    /// </summary>
    public class DepthFirstWalker : TreeWalker
    {
        private Stack<IEnumerator> _enumerators;

        /// <summary>
        /// Initializes a new instance of the <see cref="DepthFirstWalker" /> class
        /// </summary>
        public DepthFirstWalker()
            : this(new AcceptFilter())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DepthFirstWalker" /> class
        /// </summary>
        /// <param name="filter">The filter to use</param>
        public DepthFirstWalker(TreeNodeFilter filter)
            : base(filter)
        {
            _enumerators = new Stack<IEnumerator>();
        }

        /// <summary>
        /// The move next implementation
        /// </summary>
        /// <returns>
        /// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection
        /// </returns>
        protected override bool MoveNextImp()
        {
            if (_current == null)
            {
                this.Reset();
                _current = _root;
                _enumerators.Push(_current.Children.GetEnumerator());
                return true;
            }
            while (_enumerators.Count > 0)
            {
                var enumerator = _enumerators.Peek();

                if (enumerator.MoveNext())
                {
                    _current = (TreeNode)enumerator.Current;
                    _enumerators.Push(_current.Children.GetEnumerator());
                    return true;
                }
                else
                {
                    _enumerators.Pop();
                }
            }
            return false;
        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first element in the collection
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            _enumerators.Clear();
        }
    }
}
