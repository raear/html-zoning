/*
Government Usage Rights Notice:  The U.S. Government retains unlimited, royalty-free usage rights to this software, but not ownership, as provided by Federal law.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

•	Redistributions of source code must retain the above Government Usage Rights Notice, this list of conditions and the following disclaimer.

•	Redistributions in binary form must reproduce the above Government Usage Rights Notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

•	Neither the names of the National Library of Medicine, the National Institutes of Health, nor the names of any of the software developers may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE U.S. GOVERNMENT AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITEDTO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE U.S. GOVERNMENT
OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System.Linq;
using Imppoa.HtmlZoning.TreeStructure;
using Imppoa.HtmlZoning.TreeStructure.Filters;

namespace Imppoa.Labeling.ArticleContent
{
    /// <summary>
    /// Walks the tree, following the highest article content score fraction
    /// </summary>
    public class ArticleContentScoreFractionWalker : TreeWalker
    {
        private readonly string _articleContentScoreFractionFeatureName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleContentScoreFractionWalker"/> class.
        /// </summary>
        /// <param name="articleContentScoreFractionFeatureName">Article content score fraction feature name</param>
        public ArticleContentScoreFractionWalker(string articleContentScoreFractionFeatureName)
            : base(new AcceptFilter())
        {
            _articleContentScoreFractionFeatureName = articleContentScoreFractionFeatureName;
        }

        /// <summary>
        /// The move next implementation
        /// </summary>
        /// <returns>
        /// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection
        /// </returns>
        protected override bool MoveNextImp()
        {
            bool success = false;
            if (_current == null)
            {
                _current = _root;
                success = true;
            }
            else
            {
                var next = _current.Children
                                   .OrderByDescending(n => n.GetFeature(_articleContentScoreFractionFeatureName).AsDecimal())
                                   .FirstOrDefault();
                if (next != null)
                {
                    _current = next;
                    success = true;
                }
            }

            return success;
        }
    }
}
