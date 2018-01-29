/*
Government Usage Rights Notice:  The U.S. Government retains unlimited, royalty-free usage rights to this software, but not ownership, as provided by Federal law.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

•	Redistributions of source code must retain the above Government Usage Rights Notice, this list of conditions and the following disclaimer.

•	Redistributions in binary form must reproduce the above Government Usage Rights Notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

•	Neither the names of the National Library of Medicine, the National Institutes of Health, nor the names of any of the software developers may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE U.S. GOVERNMENT AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITEDTO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE U.S. GOVERNMENT
OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using Imppoa.HtmlZoning.TreeStructure;
using System.Collections.Generic;
using System.Linq;

namespace Imppoa.HtmlZoning.Dom
{
    /// <summary>
    /// Html document
    /// </summary>
    public abstract class HtmlDocument : Tree<HtmlElement>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlDocument"/> class
        /// </summary>
        public HtmlDocument()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlDocument" /> class
        /// </summary>
        /// <param name="root">The root element</param>
        /// <param name="info">The HTML document information</param>
        /// <param name="defaultStyleLookup">The default style lookup</param>
        public HtmlDocument(HtmlElement root, HtmlDocumentInfo info, DefaultStyleLookup defaultStyleLookup)
            : base(root)
        {
            this.Info = info;
            this.DefaultStyleLookup = defaultStyleLookup;
        }

        /// <summary>
        /// Initializes the class
        /// </summary>
        /// <param name="root">The root element</param>
        /// <param name="info">The HTML document information</param>
        /// <param name="defaultStyleLookup">The default style lookup</param>
        public void Initialize(HtmlElement root, HtmlDocumentInfo info, DefaultStyleLookup defaultStyleLookup)
        {
            base.Initialize(root);
            this.Info = info;
            this.DefaultStyleLookup = defaultStyleLookup;
        }

        /// <summary>
        /// Gets or sets the document information
        /// </summary>
        /// <value>
        /// The information
        /// </value>
        public HtmlDocumentInfo Info { get; set; }

        /// <summary>
        /// Gets the default style lookup
        /// </summary>
        /// <value>
        /// The default style lookup
        /// </value>
        public DefaultStyleLookup DefaultStyleLookup { get; private set; }

        /// <summary>
        /// Gets or sets the display html
        /// </summary>
        /// <value>
        /// The display html
        /// </value>
        public string DisplayHtml { get; set; }

        /// <summary>
        /// Gets the header
        /// </summary>
        /// <value>
        /// The header
        /// </value>
        public HtmlElement Header
        {
            get
            {
                return this.GetElementsByTagName(Html.Tags.HEAD).FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets the body
        /// </summary>
        /// <value>
        /// The body
        /// </value>
        public HtmlElement Body
        {
            get
            {
                return this.GetElementsByTagName(Html.Tags.BODY).FirstOrDefault();
            }
        }

        /// <summary>
        /// Gets elements with the specified tag name
        /// </summary>
        /// <param name="tag">The tag name</param>
        /// <returns>
        /// Elements with the specified tag name
        /// </returns>
        public IReadOnlyList<HtmlElement> GetElementsByTagName(string tag)
        {
            return this.Root.GetElementsByTagName(tag);
        }
    }
}

