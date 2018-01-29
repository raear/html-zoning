/*
Government Usage Rights Notice:  The U.S. Government retains unlimited, royalty-free usage rights to this software, but not ownership, as provided by Federal law.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

•	Redistributions of source code must retain the above Government Usage Rights Notice, this list of conditions and the following disclaimer.

•	Redistributions in binary form must reproduce the above Government Usage Rights Notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

•	Neither the names of the National Library of Medicine, the National Institutes of Health, nor the names of any of the software developers may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE U.S. GOVERNMENT AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITEDTO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE U.S. GOVERNMENT
OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using HtmlAgilityPack;
using System;

namespace Imppoa.HtmlRendering
{
    public static class WebUtils
    {
        /// <summary>
        /// Returns whether the supplied URL is valid
        /// </summary>
        /// <param name="url">The URL</param>
        /// <returns>true, if the URL is valid, false otherwise</returns>
        public static bool UrlIsValid(string url)
        {
            bool valid = true;
            try
            {
                new UriBuilder(url);
            }
            catch
            {
                valid = false;
            }
            return valid;
        }

        /// <summary>
        /// Gets the base url
        /// </summary>
        /// <param name="uri">The url</param>
        /// <returns>the base url</returns>
        public static string GetBaseUrl(string url)
        {
            var builder = new UriBuilder(url);
            Uri address = builder.Uri;
            Uri directory = new Uri(address, ".");
            return directory.OriginalString;
        }

        /// <summary>
        /// Inserts a base tag in the header, if it exists
        /// </summary>
        /// <param name="html">The html to modify</param>
        /// <param name="baseUrl">The base url</param>
        /// <returns>
        /// html with base tag
        /// </returns>
        public static string InsertBaseTag(string html, string baseUrl)
        {
            var htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(html);

            var docNode = htmlDoc.DocumentNode;
            var existingBaseNode = docNode.SelectSingleNode("//base");
            if (existingBaseNode != null)
            {
                return html;
            }

            var htmlNode = docNode.SelectSingleNode("//html");
            if (htmlNode == null)
            {
                string modHtml = "<html>" + html + "</html>";
                return InsertBaseTag(modHtml, baseUrl);
            }
           
            HtmlNode targetNode;
            string newNodeHtml;
            var headNode = docNode.SelectSingleNode("//head");
            if (headNode == null)
            {
                targetNode = htmlNode;
                newNodeHtml = string.Format(@"<head><base href=""{0}""></head>", baseUrl);
            }
            else
            {
                targetNode = headNode;
                newNodeHtml = string.Format(@"<base href=""{0}"">", baseUrl);
            }

            var newNode = HtmlNode.CreateNode(newNodeHtml);
            targetNode.PrependChild(newNode);
            return htmlDoc.DocumentNode.OuterHtml;
        }
    }
}
