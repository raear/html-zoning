/*
Government Usage Rights Notice:  The U.S. Government retains unlimited, royalty-free usage rights to this software, but not ownership, as provided by Federal law.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

•	Redistributions of source code must retain the above Government Usage Rights Notice, this list of conditions and the following disclaimer.

•	Redistributions in binary form must reproduce the above Government Usage Rights Notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

•	Neither the names of the National Library of Medicine, the National Institutes of Health, nor the names of any of the software developers may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE U.S. GOVERNMENT AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITEDTO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE U.S. GOVERNMENT
OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using Imppoa.HtmlZoning.ColumnTreeConstruction;
using Imppoa.HtmlZoning.Dom;
using Imppoa.HtmlZoning.Dom.Serialization;
using Imppoa.HtmlZoning.ElementClassification;
using Imppoa.HtmlZoning.Serialization;
using Imppoa.HtmlZoning.ZoneTreeConstruction;
using Imppoa.Labeling;
using Imppoa.Labeling.ArticleContent;
using Imppoa.NLP;
using System.IO;

namespace Imppoa.HtmlZoning.Visualization
{
    internal class AppServices
    {
        public static readonly string URL_FILENAME_TEMPLATE = "{0}.url";
        private static readonly string DOM_HTML_FILENAME_TEMPLATE = "{0}.html";
        private static readonly string DOM_TEXT_FILENAME_TEMPLATE = "{0}.txt";
        private static readonly string DOM_TREE_FILENAME_TEMPLATE = "{0}.domtree";
        private static readonly string ZONE_TREE_FILENAME_TEMPLATE = "{0}.zonetree";

        private readonly HtmlDocumentSerializer _htmlDocumentSerializer;
        private readonly ZoneTreeSerializer _zoneTreeSerializer;
        private readonly PreZoningClassification _elementClassifier;
        private readonly ZoneTreeBuilder _zoneTreeBuilder;
        private readonly ColumnTreeBuilder _columnTreeBuilder;
        private readonly ArticleContentLabeler _layoutAnalysisArticleContentLabeler;
        private readonly SemanticTagArticleContentLabeler _articleTagArticleContentLabeler;
        private readonly SemanticTagArticleContentLabeler _mainTagArticleContentLabeler;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppServices"/> class
        /// </summary>
        public AppServices()
        {
            var defaultStyleLookup = DefaultStyleLookup.CreateForInternetExplorer();
            bool validateSerializationResult = false;
            _htmlDocumentSerializer = new HtmlDocumentSerializer(defaultStyleLookup, validateSerializationResult);
            _zoneTreeSerializer = new ZoneTreeSerializer(defaultStyleLookup);
            
            _elementClassifier = new PreZoningClassification(HtmlElementType.SignificantBlock, HtmlElementType.SignificantInline, HtmlElementType.SignificantLinebreak, HtmlElementType.SignificantInvisible, HtmlElementType.BreakDown, HtmlElementType.Aname, HtmlElementType.Hidden);
            _zoneTreeBuilder = ZoneTreeBuilder.Create(HtmlElementType.SignificantBlock, HtmlElementType.SignificantInline, HtmlElementType.SignificantLinebreak, HtmlElementType.SignificantInvisible, HtmlElementType.BreakDown, HtmlElementType.Aname, HtmlElementType.Hidden);
            _columnTreeBuilder = ColumnTreeBuilder.Create();

            var naturalLanguageProcessor = new OpenNaturalLanguageProcessor();
            _layoutAnalysisArticleContentLabeler = new ArticleContentLabeler(naturalLanguageProcessor, ZoneLabel.Paragraph, ZoneLabel.ArticleContent, ZoneFeature.Common_Tokens);
            _articleTagArticleContentLabeler = new SemanticTagArticleContentLabeler(Html.Tags.ARTICLE, ZoneLabel.ArticleContent);
            _mainTagArticleContentLabeler = new SemanticTagArticleContentLabeler(Html.Tags.MAIN, ZoneLabel.ArticleContent);
        }

        /// <summary>
        /// Gets the root directory for the file path
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <returns>the root directory</returns>
        internal string GetRootDirectory(string filePath)
        {
            return Path.GetDirectoryName(filePath);
        }

        /// <summary>
        /// Gets the pmid from the file path
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <returns>the pmid</returns>
        internal string GetPmidFromFilePath(string filePath)
        {
            string pmid = Path.GetFileNameWithoutExtension(filePath);
            return pmid;
        }

        /// <summary>
        /// Reads the URL from the URL file
        /// </summary>
        /// <param name="rootDirectory">The root directory</param>
        /// <param name="pmid">The pmid</param>
        /// <returns>the URL</returns>
        internal string ReadUrl(string rootDirectory, string pmid)
        {
            var paths = this.ConstructFilePaths(rootDirectory, pmid);
            return File.ReadAllText(paths.UrlPath);
        }

        /// <summary>
        /// Determines whether a zone tree exists
        /// </summary>
        /// <param name="rootDirectory">The root directory</param>
        /// <param name="pmid">The pmid</param>
        /// <returns>whether a zone tree exists</returns>
        internal bool ZoneTreeFilesExists(string rootDirectory, string pmid)
        {
            var paths = this.ConstructFilePaths(rootDirectory, pmid);
            return File.Exists(paths.DocumentHtmlPath) &&
                   File.Exists(paths.DocumentTextPath) &&
                   File.Exists(paths.DocumentXmlPath) &&
                   File.Exists(paths.ZoneTreeXmlPath);
        }

        /// <summary>
        /// Deserializes a zone tree
        /// </summary>
        /// <param name="rootDirectory">The root directory</param>
        /// <param name="pmid">The pmid</param>
        /// <returns>
        /// the zone tree
        /// </returns>
        internal ZoneTree DeserializeZoneTree(string rootDirectory, string pmid)
        {
            var paths = this.ConstructFilePaths(rootDirectory, pmid);
            string documentHtml = File.ReadAllText(paths.DocumentHtmlPath);
            string documentText = File.ReadAllText(paths.DocumentTextPath);
            string docXml = File.ReadAllText(paths.DocumentXmlPath);
            string zoneTreeXml = File.ReadAllText(paths.ZoneTreeXmlPath);

            return _zoneTreeSerializer.Deserialize(documentHtml, documentText, docXml, zoneTreeXml);
        }

        /// <summary>
        /// Serializes a zone tree
        /// </summary>
        /// <param name="rootDirectory">The root directory</param>
        /// <param name="pmid">The pmid</param>
        /// <param name="zoneTree">The zone tree</param>
        /// <param name="url">The URL</param>
        internal void SerializeZoneTree(string rootDirectory, string pmid, ZoneTree zoneTree, string url)
        {
            var output = _htmlDocumentSerializer.Serialize(zoneTree.Document);
            string zoneTreeXml = _zoneTreeSerializer.Serialize(zoneTree);
        
            var paths = this.ConstructFilePaths(rootDirectory, pmid);
            File.WriteAllText(paths.DocumentHtmlPath, output.DomHtml);
            File.WriteAllText(paths.DocumentTextPath, output.DomText);
            File.WriteAllText(paths.DocumentXmlPath, output.DomTreeXml);
            File.WriteAllText(paths.ZoneTreeXmlPath, zoneTreeXml);
            File.WriteAllText(paths.UrlPath, url);
        }

        /// <summary>
        /// Does the zoning
        /// </summary>
        /// <param name="output">The serialized DOM</param>
        /// <returns>the zone tree</returns>
        internal ZoneTree DoZoning(SerializationOutput output)
        {
            var dom = _htmlDocumentSerializer.Deserialize(output.DomHtml, output.DomText, output.DomTreeXml, output.DisplayHtml);
            _elementClassifier.Execute(dom);
            return _zoneTreeBuilder.Build(dom);
        }

        /// <summary>
        /// Creates a column tree
        /// </summary>
        /// <param name="zoneTree">The zone tree</param>
        /// <returns></returns>
        internal ColumnTree CreateColumnTree(ZoneTree zoneTree)
        {
            bool collapseBranches = true;
            return _columnTreeBuilder.Build(zoneTree, collapseBranches);
        }

        /// <summary>
        /// Labels the article content using the layout analysis algorithm
        /// </summary>
        /// <param name="columnTree">The column tree</param>
        internal void LayoutAnalysisArticleContentLabeling(ColumnTree columnTree)
        {
            _layoutAnalysisArticleContentLabeler.Execute(columnTree);
        }

        /// <summary>
        /// Labels the article content using the article semantic tag algorithm
        /// </summary>
        /// <param name="zoneTree">The zone tree</param>
        internal void ArticleTagArticleContentLabeling(ZoneTree zoneTree)
        {
             _articleTagArticleContentLabeler.Execute(zoneTree);
        }

        /// <summary>
        /// Labels the article content using the main semantic tag algorithm
        /// </summary>
        /// <param name="zoneTree">The zone tree</param>
        internal void MainTagArticleContentLabeling(ZoneTree zoneTree)
        {
            _mainTagArticleContentLabeler.Execute(zoneTree);
        }

        /// <summary>
        /// Constructs the zone tree file paths
        /// </summary>
        /// <param name="rootDirectory">The root directory</param>
        /// <param name="pmid">The pmid</param>
        /// <returns>object containing the path data</returns>
        private dynamic ConstructFilePaths(string rootDirectory, string pmid)
        {
            string domHtmlPath = Path.Combine(rootDirectory, string.Format(DOM_HTML_FILENAME_TEMPLATE, pmid));
            string domTextPath = Path.Combine(rootDirectory, string.Format(DOM_TEXT_FILENAME_TEMPLATE, pmid));
            string domTreePath = Path.Combine(rootDirectory, string.Format(DOM_TREE_FILENAME_TEMPLATE, pmid));
            string zoneTreePath = Path.Combine(rootDirectory, string.Format(ZONE_TREE_FILENAME_TEMPLATE, pmid));
            string urlPath = Path.Combine(rootDirectory, string.Format(URL_FILENAME_TEMPLATE, pmid));

            return new
            {
                DocumentHtmlPath = domHtmlPath,
                DocumentTextPath = domTextPath,
                DocumentXmlPath = domTreePath,
                ZoneTreeXmlPath = zoneTreePath,
                UrlPath = urlPath,
            };
        }
    }
}
