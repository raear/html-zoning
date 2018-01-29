/*
Government Usage Rights Notice:  The U.S. Government retains unlimited, royalty-free usage rights to this software, but not ownership, as provided by Federal law.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

•	Redistributions of source code must retain the above Government Usage Rights Notice, this list of conditions and the following disclaimer.

•	Redistributions in binary form must reproduce the above Government Usage Rights Notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

•	Neither the names of the National Library of Medicine, the National Institutes of Health, nor the names of any of the software developers may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE U.S. GOVERNMENT AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITEDTO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE U.S. GOVERNMENT
OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

namespace Imppoa.HtmlZoning.Dom.Serialization
{
    /// <summary>
    /// Serializable html document
    /// </summary>
    public class SerializableDocument : HtmlDocument
    {
        /// <summary>
        /// Creates a serializable document
        /// </summary>
        /// <param name="doc">The html document to copy</param>
        /// <param name="documentHtml">(out) The document html</param>
        /// <param name="documentText">(out) The document text</param>
        /// <returns>
        /// the serializable document
        /// </returns>
        public static SerializableDocument Create(HtmlDocument doc, out string documentHtml, out string documentText)
        {
            var factory = new SerializableDocumentFactory();
            return factory.Create(doc, out documentHtml, out documentText);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableDocument" /> class
        /// </summary>
        /// <param name="root">The root element</param>
        /// <param name="info">The HTML Document information</param>
        /// <param name="defaultStyleLookup">The default style lookup</param>
        /// <param name="html">The html</param>
        /// <param name="text">The text</param>
        public SerializableDocument(SerializableElement root, HtmlDocumentInfo info, DefaultStyleLookup defaultStyleLookup, string html, string text)
            : base(root, info, defaultStyleLookup)
        {
            this.Html = html;
            this.Text = text;
        }

        /// <summary>
        /// Gets the document html
        /// </summary>
        /// <value>
        /// The document html
        /// </value>
        public string Html { get; private set; }

        /// <summary>
        /// Gets the document text
        /// </summary>
        /// <value>
        /// The document text
        /// </value>
        public string Text { get; private set; }
    }
}
