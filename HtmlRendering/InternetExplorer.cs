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
using mshtml;
using System.Drawing;

namespace Imppoa.HtmlRendering
{
    /// <summary>
    /// Internet explorer web browser
    /// </summary>
    public class InternetExplorer : WebBrowser
    {
        private static readonly string ABOUT_BLANK = "about:blank";
        private readonly System.Windows.Forms.WebBrowser _winFormsBrowser;
        private readonly MsHtmlDocumentFactory _msDocFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="InternetExplorer" /> class
        /// </summary>
        public InternetExplorer()
            : this(new System.Windows.Forms.WebBrowser())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InternetExplorer" /> class
        /// </summary>
        /// <param name="winFormsBrowser">The windows forms browser</param>
        public InternetExplorer(System.Windows.Forms.WebBrowser winFormsBrowser)
            : this(winFormsBrowser, new Size(DEFAULT_RENDER_WIDTH, DEFAULT_RENDER_HEIGHT))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InternetExplorer" /> class
        /// </summary>
        /// <param name="winFormsBrowser">The windows forms browser</param>
        /// <param name="size">The size</param>
        public InternetExplorer(System.Windows.Forms.WebBrowser winFormsBrowser, Size size)
            : this(winFormsBrowser, size, DefaultStyleLookup.CreateForInternetExplorer())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InternetExplorer" /> class
        /// </summary>
        /// <param name="winFormsBrowser">The windows forms browser</param>
        /// <param name="size">The size</param>
        /// <param name="defaultStyles">The default styles</param>
        public InternetExplorer(System.Windows.Forms.WebBrowser winFormsBrowser, Size size, DefaultStyleLookup defaultStyles)
            : base(size, defaultStyles)
        {
            _winFormsBrowser = winFormsBrowser;
            _msDocFactory = new MsHtmlDocumentFactory(defaultStyles);
            this.ConfigureBrowser();
        }

        /// <summary>
        /// Whether the browser has an url
        /// </summary>
        /// <value>
        /// true, if browser has an url, false otherwise
        /// </value>
        public override bool HasUrl
        {
            get
            {
                return base.HasUrl && this.Url.ToLower().Trim() != ABOUT_BLANK;
            }
        }

        /// <summary>
        /// Gets the URL
        /// </summary>
        /// <value>
        /// The URL
        /// </value>
        public override string Url
        {
            get
            {
                return _winFormsBrowser.Url.ToString();
            }
        }

        /// <summary>
        /// Gets the rendered HTML
        /// </summary>
        /// <value>
        /// The rendered HTML
        /// </value>
        public override string OriginalHtml
        {
            get
            {
                return _winFormsBrowser.DocumentText;
            }
        }

        /// <summary>
        /// Gets the windows forms document
        /// </summary>
        /// <value>
        /// The windows forms document
        /// </value>
        private System.Windows.Forms.HtmlDocument WinFormsDoc
        {
            get
            {
                return _winFormsBrowser.Document;
            }
        }

        /// <summary>
        /// Gets the MSHTML document
        /// </summary>
        /// <value>
        /// The MSHTML document
        /// </value>
        private IHTMLDocument2 MsHtmlDoc
        {
            get
            {
                return (IHTMLDocument2) _winFormsBrowser.Document.DomDocument;
            }
        }

        /// <summary>
        /// Gets the MSHTML dom
        /// </summary>
        /// <returns>The MSHTML dom</returns>
        public object GetDom()
        {
            return this.MsHtmlDoc;
        }

        /// <summary>
        /// Modifies the DOM
        /// </summary>
        /// <param name="modification">The modification to apply</param>
        public override void ModifyDom(IDomModification modification)
        {
            modification.Apply(this.MsHtmlDoc);
        }

        /// <summary>
        /// Copies the browser DOM into our own representation
        /// </summary>
        /// <returns>
        /// the copied dom
        /// </returns>
        public override HtmlDocument CopyDom()
        {
            return _msDocFactory.Create(this.MsHtmlDoc, this.Url);
        }

        /// <summary>
        /// Renders a HTML string
        /// </summary>
        /// <param name="html">the HTML string</param>
        public override void RenderHtml(string html)
        {
            _winFormsBrowser.DocumentText = html;
        }

        /// <summary>
        /// Renders an URL
        /// </summary>
        /// <param name="url">The URL</param>
        public override void RenderUrl(string url)
        {
            _winFormsBrowser.Navigate(url);
        }

        /// <summary>
        /// Processes browser internal events
        /// </summary>
        public override void DoEvents()
        {
            System.Windows.Forms.Application.DoEvents();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources
        /// </summary>
        public override void Dispose()
        {
            _winFormsBrowser.Dispose();
        }

        /// <summary>
        /// Configures the browser
        /// </summary>
        private void ConfigureBrowser()
        {
            _winFormsBrowser.Size = _size;
            _winFormsBrowser.MinimumSize = _size;
            _winFormsBrowser.MaximumSize = _size;
            _winFormsBrowser.ScriptErrorsSuppressed = true;
            _winFormsBrowser.DocumentCompleted += browser_DocumentCompleted;
        }

        /// <summary>
        /// Handles the DocumentCompleted event of the browser control
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.WebBrowserDocumentCompletedEventArgs"/> instance containing the event data</param>
        private void browser_DocumentCompleted(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
        {
            var wb = (System.Windows.Forms.WebBrowser)sender;
            if (e.Url == wb.Document.Url)
            {
                base.RenderingComplete = true;
            }
        }
    }
}
