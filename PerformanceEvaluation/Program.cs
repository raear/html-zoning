/*
Government Usage Rights Notice:  The U.S. Government retains unlimited, royalty-free usage rights to this software, but not ownership, as provided by Federal law.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

•	Redistributions of source code must retain the above Government Usage Rights Notice, this list of conditions and the following disclaimer.

•	Redistributions in binary form must reproduce the above Government Usage Rights Notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

•	Neither the names of the National Library of Medicine, the National Institutes of Health, nor the names of any of the software developers may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE U.S. GOVERNMENT AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITEDTO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE U.S. GOVERNMENT
OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using Imppoa.HtmlZoning.Dom;
using Imppoa.HtmlZoning.Serialization;
using Imppoa.HtmlZoning.TreeStructure;
using Imppoa.HtmlZoning.TreeStructure.Visitors;
using Imppoa.HtmlZoning.TreeStructure.Walkers;
using Imppoa.Labeling;
using Imppoa.Labeling.ArticleContent;
using Imppoa.NLP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Imppoa.HtmlZoning.PerformanceEvaluation
{
    /// <summary>
    /// Console application to evaluate article content detection algorithm performance
    /// </summary>
    public class Program
    {
        private const string LAYOUT_ANALYSIS_ALG = "layout";
        private const string ARTICLE_SEMANTIC_TAG_ALG = "articletag";
        private const string MAIN_SEMANTIC_TAG_ALG = "maintag";

        private static readonly string URL_EX = "url";
        private static readonly string HTML_EX = "html";
        private static readonly string TEXT_EX = "txt";
        private static readonly string DOMTREE_EX = "domtree";
        private static readonly string ZONETREE_EX = "zonetree";

        private static readonly string TOKENS_FEATURE_NAME = ZoneFeature.Common_Tokens;
        private static readonly string WORD_COUNT_FEATURE_NAME = ZoneFeature.ArticleContent_WordCount;
        private static readonly string ARTICLE_CONTENT_LABEL = ZoneLabel.ArticleContent;

        private static readonly INaturalLanguageProcessing _naturalLanguageProcessor = new OpenNaturalLanguageProcessor();
        private static readonly ZoneTreeSerializer _zoneTreeSerializer = new ZoneTreeSerializer();

        /// <summary>
        /// Run the performance evaluation
        /// </summary>
        /// <param name="args">
        /// args[0]: directory containing the manually labeled baseline data 
        /// args[1]: article content label used in manual labeling
        /// args[2]: the name of the algorithm to evaluate ("layout": layout analysis algorithm, "articletag": article tag semantic markup algorithm, "maintag": main tag semantic markup algorithm)
        /// args[3]: csv file path to save the result to
        /// </param>
        public static void Main(string[] args)
        {
            string baselineDir = args[0];
            string baselineLabel = args[1];
            string algName = args[2]?.ToLower();
            string resultsFile = args[3];

            var pmids = GetPmids(baselineDir);
            var labeler = CreateLabeler(algName);

            var sb = new StringBuilder();
            foreach (var pmid in pmids)
            {
                var tree = LoadZoneTree(baselineDir, pmid);
                labeler.Execute(tree);
                AddOrOverwriteWordCountFeature(tree);
                var metrics = Evaluate(tree, baselineLabel);
                string line = string.Format("{0},{1:0.00000},{2:0.00000},{3:0.00000}",
                                            pmid, metrics.Item1, metrics.Item2, metrics.Item3);
                sb.AppendLine(line);
                Console.WriteLine(line);
            }

            File.WriteAllText(resultsFile, sb.ToString());
        }

        private static Tuple<double, double, double> Evaluate(ZoneTree tree, string baselineLabel)
        {
            int predictedWordCount = PredictedWordCount(tree);
            int correctlyPredictedWordCount = CorrectlyPredictedWordCount(tree, baselineLabel);
            int baselineWordCount = BaselineWordCount(tree, baselineLabel);

            var precision = Precision(correctlyPredictedWordCount, predictedWordCount);
            var recall = Recall(correctlyPredictedWordCount, baselineWordCount);
            var fScore = FScore(precision, recall);

            return new Tuple<double, double, double>(precision, recall, fScore);
        }

        #region File Access

        private static IEnumerable<string> GetPmids(string baselineDir)
        {
            var filePaths = Directory.EnumerateFiles(baselineDir, "*." + URL_EX);
            var pmids = filePaths.Select(fp => Path.GetFileNameWithoutExtension(fp));
            return pmids;
        }

        private static ZoneTree LoadZoneTree(string baselineDir, string pmid)
        {
            var html = ReadFile(baselineDir, pmid, HTML_EX);
            var text = ReadFile(baselineDir, pmid, TEXT_EX);
            var domTree = ReadFile(baselineDir, pmid, DOMTREE_EX);
            var zoneTree = ReadFile(baselineDir, pmid, ZONETREE_EX);
            var tree = _zoneTreeSerializer.Deserialize(html, text, domTree, zoneTree);
            return tree;
        }

        private static string ReadFile(string baselineDir, string pmid, string ext)
        {
            string path = Path.Combine(baselineDir, pmid + "." + ext);
            return File.ReadAllText(path);
        }

        #endregion

        #region Zone Tree Commands

        private static Algorithm<ZoneTree> CreateLabeler(string algName)
        {
            Algorithm<ZoneTree> alg;
            switch (algName)
            {
                case LAYOUT_ANALYSIS_ALG:
                    alg = new ZoneTreeArticleContentLabeler(_naturalLanguageProcessor, ZoneLabel.Paragraph, ARTICLE_CONTENT_LABEL, TOKENS_FEATURE_NAME);
                    break;
                case ARTICLE_SEMANTIC_TAG_ALG:
                    alg = new SemanticTagArticleContentLabeler(Html.Tags.ARTICLE, ARTICLE_CONTENT_LABEL);
                    break;
                case MAIN_SEMANTIC_TAG_ALG:
                    alg = new SemanticTagArticleContentLabeler(Html.Tags.MAIN, ARTICLE_CONTENT_LABEL);
                    break;
                default:
                    throw new ArgumentException("Algorithm name not recognized", algName);
            }
            return alg;
        }

        private static void AddOrOverwriteWordCountFeature(ZoneTree tree)
        {
            var leafWalker = new BreadthFirstWalkerFactory().CreateLeafWalker();
            tree.Accept(new FeatureExtractionVisitor<Zone>(TOKENS_FEATURE_NAME, new Tokens(_naturalLanguageProcessor)), leafWalker);
            tree.Accept(new FeatureExtractionVisitor<Zone>(WORD_COUNT_FEATURE_NAME, new WordCount(TOKENS_FEATURE_NAME)), leafWalker);
        }

        #endregion

        #region Zone Tree Queries

        private static int PredictedWordCount(ZoneTree tree)
        {
            int count = WordCount(tree, ARTICLE_CONTENT_LABEL);
            return count;
        }

        private static int BaselineWordCount(ZoneTree tree, string baselineLabel)
        {
            int count = WordCount(tree, baselineLabel);
            return count;
        }

        private static int WordCount(ZoneTree tree, string label)
        {
            int count = tree.LeafNodes.Where(l => HasLabel(l, label)).Sum(l => l.GetFeature(WORD_COUNT_FEATURE_NAME).AsInt());
            return count;
        }

        private static int CorrectlyPredictedWordCount(ZoneTree tree, string baselineLabel)
        {
            int count = tree.LeafNodes.Where(l => HasLabel(l, baselineLabel) && HasLabel(l, ARTICLE_CONTENT_LABEL)).Sum(l => l.GetFeature(WORD_COUNT_FEATURE_NAME).AsInt());
            return count;
        }

        private static bool HasLabel(Zone zone, string label)
        {
            return zone.HasClassification(label);
        }

        #endregion

        #region Metrics

        private static double Precision(int matchCount, int detectedCount)
        {
            double p = 0;
            if (detectedCount > 0)
            {
                p = (double) matchCount / detectedCount;
            }
            return p;
        }

        private static double Recall(int matchCount, int baselineCount)
        {
            double r = 1;
            if (baselineCount > 0)
            {
                r = (double)matchCount / baselineCount;
            }
            return r;
        }

        private static double FScore(double p, double r)
        {
            double f = 0;
            double denominator = p + r;
            if (denominator > 0)
            {
                f = 2 * ((p * r) / denominator);
            }
            return f;
        }

        #endregion
    }
}
