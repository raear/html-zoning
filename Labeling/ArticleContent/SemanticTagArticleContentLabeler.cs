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
using Imppoa.HtmlZoning.Filter;
using Imppoa.HtmlZoning.TreeStructure;
using Imppoa.HtmlZoning.TreeStructure.Filters;
using Imppoa.HtmlZoning.TreeStructure.Visitors;

namespace Imppoa.Labeling.ArticleContent
{
    /// <summary>
    /// Labels the article content based on the supplied HTML 5 semantic tag name
    /// </summary>
    public class SemanticTagArticleContentLabeler : Algorithm<ZoneTree>
    {
        private readonly string _semanticTagName;
        private readonly string _articleContentLabel;

        /// <summary>
        /// Constructs an new instance of the SemanticTagArticleContentLabeler class
        /// </summary>
        /// <param name="semanticTagName">The semantic tag name of interest</param>
        /// <param name="articleContentLabel">The label to use for the article content</param>
        public SemanticTagArticleContentLabeler(string semanticTagName, string articleContentLabel)
        {
            _semanticTagName = semanticTagName;
            _articleContentLabel = articleContentLabel;
        }

        /// <summary>
        /// Executes the algorithm
        /// </summary>
        /// <param name="zoneTree">The zone tree</param>
        protected override void ExecuteImplementation(ZoneTree zoneTree)
        {
            zoneTree.Accept(new TreeNodeClassifier(_articleContentLabel, new ZoneElementsTagNameFilter(_semanticTagName)));
            zoneTree.Accept(new TreeNodeClassifier(_articleContentLabel, new DescendantFilter(_articleContentLabel, _articleContentLabel)));
        }
    }
}
