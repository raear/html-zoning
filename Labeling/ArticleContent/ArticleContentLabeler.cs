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
using Imppoa.HtmlZoning.TreeStructure;
using Imppoa.HtmlZoning.TreeStructure.Filters;
using Imppoa.HtmlZoning.TreeStructure.Visitors;
using Imppoa.HtmlZoning.TreeStructure.Walkers;

namespace Imppoa.Labeling.ArticleContent
{
    /// <summary>
    /// Labels the article content
    /// </summary>
    public class ArticleContentLabeler : Algorithm<ColumnTree>
    {
        private static readonly decimal DELTA_THRESHOLD = 0.3m;

        private readonly INaturalLanguageProcessing _naturalLanguageProcessor;
        private readonly string _articleContentLabel;
        private readonly string _paragraphLabel;
        private readonly string _tokenFeatureName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleContentLabeler" /> class
        /// </summary>
        /// <param name="naturalLanguageProcessor">The natural language processor to use</param>
        /// <param name="paragraphLabel">The paragraph label</param>
        /// <param name="articleContentLabel">The article content label</param>
        /// <param name="tokenFeatureName">The token feature name</param>
        public ArticleContentLabeler(INaturalLanguageProcessing naturalLanguageProcessor, string paragraphLabel, string articleContentLabel, string tokenFeatureName)
        {
            _naturalLanguageProcessor = naturalLanguageProcessor;
            _paragraphLabel = paragraphLabel;
            _articleContentLabel = articleContentLabel;
            _tokenFeatureName = tokenFeatureName;
        }

        /// <summary>
        /// Executes the algorithm on the tree strcuture
        /// </summary>
        /// <param name="tree">The tree structure</param>
        protected override void ExecuteImplementation(ColumnTree columnTree)
        {
            // Zone tree
            var zoneTree = columnTree.ZoneTree;

            var paragraphClassifier = new ParagraphLabeler(_naturalLanguageProcessor, _paragraphLabel);
            paragraphClassifier.Execute(zoneTree);

            var leafWalker = new BreadthFirstWalkerFactory().CreateLeafWalker();
            zoneTree.Accept(new FeatureExtractionVisitor<Zone>(_tokenFeatureName, new Tokens(_naturalLanguageProcessor)), leafWalker);
            zoneTree.Accept(new FeatureExtractionVisitor<Zone>(ZoneFeature.ArticleContent_WordCount, new WordCount(_tokenFeatureName)), leafWalker);
            zoneTree.Accept(new FeatureExtractionVisitor<Zone>(ZoneFeature.ArticleContent_Score, new ArticleContentScore(_paragraphLabel, ZoneFeature.ArticleContent_WordCount)), leafWalker);
            zoneTree.Accept(new FeatureExtractionVisitor<Zone>(ZoneFeature.ArticleContent_AggregateScore, new ArticleContentScoreAggregator(ZoneFeature.ArticleContent_Score, ZoneFeature.ArticleContent_AggregateScore)), new BreadthFirstWalkerFactory().CreateReversed());

            int totalScore = zoneTree.Root.GetFeature(ZoneFeature.ArticleContent_AggregateScore).AsInt();

            if (totalScore > 0)
            {
                zoneTree.Accept(new FeatureExtractionVisitor<TreeNode>(ZoneFeature.ArticleContent_ScoreFraction, new ArticleContentScoreFraction(totalScore, ZoneFeature.ArticleContent_AggregateScore)));

                // Column tree
                columnTree.Accept(new FeatureExtractionVisitor<Column>(ColumnFeature.ArticleContent_ScoreFraction, new ColumnArticleContentScoreFraction(ZoneFeature.ArticleContent_ScoreFraction)));
                columnTree.Accept(new FeatureExtractionVisitor<TreeNode>(ColumnFeature.ArticleContent_DeltaScoreFraction, new DeltaArticleContentScoreFraction(ColumnFeature.ArticleContent_ScoreFraction)));
                columnTree.Accept(new TreeNodeClassifier(_articleContentLabel, new FeatureGreaterThanOrEqual<Column>(ColumnFeature.ArticleContent_DeltaScoreFraction, DELTA_THRESHOLD)), new ArticleContentScoreFractionWalker(ColumnFeature.ArticleContent_ScoreFraction));
            }
            else
            {
                columnTree.Root.AddClassification(_articleContentLabel);
            }

            columnTree.Accept(new TreeNodeClassifier(_articleContentLabel, new DescendantFilter(_articleContentLabel, _articleContentLabel)));
            zoneTree.Accept(new TreeNodeClassifier(_articleContentLabel, new DescendantFilter(_articleContentLabel, _articleContentLabel)));
        }
    }
}
