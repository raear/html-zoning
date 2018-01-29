/*
Government Usage Rights Notice:  The U.S. Government retains unlimited, royalty-free usage rights to this software, but not ownership, as provided by Federal law.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

•	Redistributions of source code must retain the above Government Usage Rights Notice, this list of conditions and the following disclaimer.

•	Redistributions in binary form must reproduce the above Government Usage Rights Notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

•	Neither the names of the National Library of Medicine, the National Institutes of Health, nor the names of any of the software developers may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE U.S. GOVERNMENT AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITEDTO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE U.S. GOVERNMENT
OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using Imppoa.HtmlZoning.TreeStructure.FeatureValueExtractors;
using System.Collections.Generic;
using System.Linq;

namespace Imppoa.HtmlZoning.TreeStructure.Visitors
{
    /// <summary>
    /// Records feature values
    /// </summary>
    public class RecordFeatureValues<T> : TreeNodeVisitor<T> where T : TreeNode
    {
        private List<object> _values;
        private FeatureValueExtractor<T> _featureExtractor;

        /// <summary>
        /// Gets the feature values
        /// </summary>
        /// <value>
        /// The feature values
        /// </value>
        public IEnumerable<object> Values
        {
            get
            {
                return _values;
            }
        }

        /// <summary>
        /// Gets the number of values recorded
        /// </summary>
        /// <value>
        /// The number of values recorded
        /// </value>
        public int Count
        {
            get
            {
                return _values.Count;
            }
        }

        /// <summary>
        /// Gets the values cast to the desired type
        /// </summary>
        public IEnumerable<TValue> GetValues<TValue>()
        {
            return _values.Select(v => (TValue)v);
        }

        /// <summary>
        /// Computes the decimal average
        /// </summary>
        public decimal GetDecimalAverage()
        {
            return this.GetValues<decimal>().Average();
        }

        /// <summary>
        /// Computes the decimal max
        /// </summary>
        public decimal GetDecimalMax()
        {
            return this.GetValues<decimal>().Max();
        }

        /// <summary>
        /// Computes the decimal min
        /// </summary>
        public decimal GetDecimalMin()
        {
            return this.GetValues<decimal>().Min();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordFeatureValues{T}"/> class
        /// </summary>
        /// <param name="featureExtractor">The feature extractor</param>
        public RecordFeatureValues(FeatureValueExtractor<T> featureExtractor)
        {
            this.Reset();
            _featureExtractor = featureExtractor;
        }

        /// <summary>
        /// Resets this instance
        /// </summary>
        public void Reset()
        {
            _values = new List<object>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordFeatureValues{T}"/> class
        /// </summary>
        /// <param name="featureName">The feature name</param>
        public RecordFeatureValues(string featureName)
            : this(new ExistingFeatureValue<T>(featureName))
        {
        }

        /// <summary>
        /// Visits the tree node
        /// </summary>
        /// <param name="node">The tree node of type T</param>
        public override void Visit(T node)
        {
            if (_featureExtractor.HasFeature(node))
            {
                object value = _featureExtractor.ExtractValue(node);
                this._values.Add(value);
            }
        }
    }
}
