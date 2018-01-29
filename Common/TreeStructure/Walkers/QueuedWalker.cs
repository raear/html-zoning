/*
Government Usage Rights Notice:  The U.S. Government retains unlimited, royalty-free usage rights to this software, but not ownership, as provided by Federal law.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

•	Redistributions of source code must retain the above Government Usage Rights Notice, this list of conditions and the following disclaimer.

•	Redistributions in binary form must reproduce the above Government Usage Rights Notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

•	Neither the names of the National Library of Medicine, the National Institutes of Health, nor the names of any of the software developers may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE U.S. GOVERNMENT AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITEDTO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE U.S. GOVERNMENT
OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System.Collections.Generic;

namespace Imppoa.HtmlZoning.TreeStructure.Walkers
{
    /// <summary>
    /// Base class for custom walkers
    /// </summary>
   public abstract class QueuedWalker : TreeWalker
    {
        protected Queue<TreeNode> _queue;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueuedWalker" /> class
        /// </summary>
        /// <param name="filter">The filter to use</param>
        public QueuedWalker(TreeNodeFilter filter)
            : base(filter)
        {
            _queue = new Queue<TreeNode>();
        }

        /// <summary>
        /// The move next implementation
        /// </summary>
        /// <returns>
        /// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection
        /// </returns>
        protected override bool MoveNextImp()
        {
            if (_queue.Count == 0)
            {
                return false;
            }
            else
            {
                _current = _queue.Dequeue();
                return true;
            }
        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first element in the collection
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            _queue.Clear();
            this.PopulateQueue();
        }

        /// <summary>
        /// Populates the queue
        /// </summary>
        /// <param name="queue">The queue</param>
        /// <param name="root">The root node</param>
        protected abstract void PopulateQueue();
    }
}
