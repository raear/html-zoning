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

namespace Imppoa.HtmlZoning.TreeStructure
{
    /// <summary>
    /// A feature extractor that is configurable at run time
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Imppoa.HtmlZoning.TreeStructure.FeatureValueExtractor{T}" />
    public class GenericFeatureValueExtractor<T> : FeatureValueExtractor<T> where T : TreeNode
    {
        private Func<T, bool> _hasFeatureFunc;
        private Func<T, object> _extractFeatureFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericFeatureValueExtractor{T}"/> class
        /// </summary>
        /// <param name="hasFeatureFunc">The has feature function</param>
        /// <param name="extractFeatureFunc">The extract feature function</param>
        public GenericFeatureValueExtractor(Func<T, bool> hasFeatureFunc, Func<T, object> extractFeatureFunc)
        {
            _hasFeatureFunc = hasFeatureFunc;
            _extractFeatureFunc = extractFeatureFunc;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericFeatureValueExtractor{T}"/> class
        /// </summary>
        /// <param name="extractFeatureFunc">The extract feature function</param>
        public GenericFeatureValueExtractor(Func<T, object> extractFeatureFunc)
        {
            _hasFeatureFunc = null;
            _extractFeatureFunc = extractFeatureFunc;
        }

        /// <summary>
        /// Whether the node has the feature
        /// </summary>
        /// <param name="node">The node</param>
        /// <returns>
        /// returns true, if the node has the feature, false otherwise
        /// </returns>
        public override bool HasFeature(T node)
        {
            if (_hasFeatureFunc == null)
            {
                return base.HasFeature(node);
            }

            return _hasFeatureFunc(node);
        }

        /// <summary>
        /// Computes a feature for the tree node
        /// </summary>
        /// <param name="node">The tree node</param>
        /// <returns>
        /// the computed feature value
        /// </returns>
        public override object ExtractValue(T node)
        {
            return _extractFeatureFunc(node);
        }
    }
}
