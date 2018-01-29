/*
Government Usage Rights Notice:  The U.S. Government retains unlimited, royalty-free usage rights to this software, but not ownership, as provided by Federal law.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

•	Redistributions of source code must retain the above Government Usage Rights Notice, this list of conditions and the following disclaimer.

•	Redistributions in binary form must reproduce the above Government Usage Rights Notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

•	Neither the names of the National Library of Medicine, the National Institutes of Health, nor the names of any of the software developers may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE U.S. GOVERNMENT AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITEDTO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE U.S. GOVERNMENT
OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

namespace Imppoa.HtmlZoning.TreeStructure.Filters
{
    /// <summary>
    /// Filters nodes with a feature value greater than a threshold
    /// </summary>
    public class FeatureGreaterThan<T> : ThresholdFilter<T> where T : TreeNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureGreaterThan{T}"/> class
        /// </summary>
        /// <param name="featureName">The feature name</param>
        /// <param name="threshold">The threshold</param>
        public FeatureGreaterThan(string featureName, decimal threshold)
            : base(featureName, threshold)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureGreaterThan{T}"/> class.
        /// </summary>
        /// <param name="featureValueExtractor">The feature value extractor</param>
        /// <param name="threshold">The threshold</param>
        public FeatureGreaterThan(FeatureValueExtractor<T> featureValueExtractor, decimal threshold)
            : base(featureValueExtractor, threshold)
        {
        }

        /// <summary>
        /// The comparison function
        /// </summary>
        /// <param name="featureValue">The feature value</param>
        /// <param name="threshold">The threshold</param>
        /// <returns>
        /// The comparison result
        /// </returns>
        protected override bool ComparisonFunc(decimal featureValue, decimal threshold)
        {
            return featureValue > threshold;
        }
    }
}
