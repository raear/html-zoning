/*
Government Usage Rights Notice:  The U.S. Government retains unlimited, royalty-free usage rights to this software, but not ownership, as provided by Federal law.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

•	Redistributions of source code must retain the above Government Usage Rights Notice, this list of conditions and the following disclaimer.

•	Redistributions in binary form must reproduce the above Government Usage Rights Notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

•	Neither the names of the National Library of Medicine, the National Institutes of Health, nor the names of any of the software developers may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE U.S. GOVERNMENT AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITEDTO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE U.S. GOVERNMENT
OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using Imppoa.HtmlZoning;
using Imppoa.HtmlZoning.TreeStructure.FeatureValueExtractors;
using System.Collections.Generic;

namespace Imppoa.Labeling
{
    /// <summary>
    /// Base class for token counting
    /// </summary>
    public abstract class TokenCount : ExistingFeatureValue<Zone>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TokenCount"/> class
        /// </summary>
        /// <param name="tokensFeatureName">The tokens feature name</param>
        public TokenCount(string tokensFeatureName)
            : base(tokensFeatureName)
        {
        }

        /// <summary>
        /// Computes a feature for the tree node
        /// </summary>
        /// <param name="zone">The tree node</param>
        /// <returns>
        /// the computed feature value
        /// </returns>
        public override object ExtractValue(Zone zone)
        {
            int tokenCount = 0;
            var tokens = (List<string>) base.ExtractValue(zone);
            for (int tokenIndex = 0; tokenIndex < tokens.Count; tokenIndex++)
            {
                string token = tokens[tokenIndex].Trim();
                if (this.CountToken(token, tokenIndex))
                {
                    tokenCount++;
                }
            }

            return tokenCount;
        }

        /// <summary>
        /// Whether the token should be counted
        /// </summary>
        /// <param name="token">The token</param>
        /// <param name="tokenIndex">Token index</param>
        /// <returns>
        /// true, if the token should be counted, false otherwise
        /// </returns>
        protected abstract bool CountToken(string token, int tokenIndex);
    }
}
