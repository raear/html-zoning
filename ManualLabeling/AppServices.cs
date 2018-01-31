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
using Imppoa.HtmlZoning.Dom.Serialization;
using Imppoa.HtmlZoning.Serialization;
using System.Drawing;
using System.IO;

namespace Imppoa.ManualLabeling
{
    public class AppServices
    {
        private readonly Configuration _config;
        private readonly HtmlDocumentSerializer _documentSerializer;
        private readonly ZoneTreeSerializer _zoneTreeSerializer;
        
        public AppServices(Configuration config)
        {
            _config = config;
            _documentSerializer = new HtmlDocumentSerializer();
            _zoneTreeSerializer = new ZoneTreeSerializer();
        }

        public string GetId(string openFilepath)
        {
            string id = Path.GetFileNameWithoutExtension(openFilepath);
            return id;
        }

        public string LoadUrl(string openFilepath)
        {
            var paths = this.ConstructFilePaths(openFilepath);
            string url = File.ReadAllText(paths.UrlPath);
            return url;
        }

        public ZoneTree LoadZoneTree(string openFilepath)
        {
            ZoneTree tree = null;
            var paths = this.ConstructFilePaths(openFilepath);
            if (File.Exists(paths.ZoneTreeXmlPath))
            {
                var documentHtml = File.ReadAllText(paths.DocumentHtmlPath);
                var documentText = File.ReadAllText(paths.DocumentTextPath);
                var documentXml = File.ReadAllText(paths.DocumentXmlPath);
                var zoneTreeXml = File.ReadAllText(paths.ZoneTreeXmlPath);
                tree = _zoneTreeSerializer.Deserialize(documentHtml, documentText, documentXml, zoneTreeXml);
            }
            return tree;
        }

        public void SaveZoneTree(string saveFilePath, ZoneTree tree)
        {
            var sOutput = _documentSerializer.Serialize(tree.Document);
            string zoneTreeXml = _zoneTreeSerializer.Serialize(tree);

            var paths = this.ConstructFilePaths(saveFilePath);
            File.WriteAllText(paths.DocumentHtmlPath, sOutput.DomHtml);
            File.WriteAllText(paths.DocumentTextPath, sOutput.DomText);
            File.WriteAllText(paths.DocumentXmlPath, sOutput.DomTreeXml);
            File.WriteAllText(paths.ZoneTreeXmlPath, zoneTreeXml);
        }

        public void SaveScreenshot(string saveFilePath, Bitmap screenshot)
        {
            var paths = this.ConstructFilePaths(saveFilePath);

            try
            {
                screenshot?.Save(paths.ScreenshotPath);
            }
            catch
            {
                // Do nothing (error when save and load path are the same)
            }
        }

        private dynamic ConstructFilePaths(string openFilePath)
        {
            string rootDir = Path.GetDirectoryName(openFilePath);
            string id = this.GetId(openFilePath);

            string urlPath = Path.Combine(rootDir, string.Format(_config.URL_FILENAME_TEMPLATE, id));
            string domHtmlPath = Path.Combine(rootDir, string.Format(_config.DOM_HTML_FILENAME_TEMPLATE, id));
            string domTextPath = Path.Combine(rootDir, string.Format(_config.DOM_TEXT_FILENAME_TEMPLATE, id));
            string domTreePath = Path.Combine(rootDir, string.Format(_config.DOM_TREE_FILENAME_TEMPLATE, id));
            string zoneTreePath = Path.Combine(rootDir, string.Format(_config.ZONE_TREE_FILENAME_TEMPLATE, id));
            string screenshotPath = Path.Combine(rootDir, string.Format(_config.SCREENSHOT_FILENAME_TEMPLATE, id));

            return new
            {
                UrlPath = urlPath,
                DocumentHtmlPath = domHtmlPath,
                DocumentTextPath = domTextPath,
                DocumentXmlPath = domTreePath,
                ZoneTreeXmlPath = zoneTreePath,
                ScreenshotPath = screenshotPath,
            };
        }
    }
}
