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
using System;
using System.Drawing;

namespace Imppoa.HtmlRendering
{
    /// <summary>
    /// Abstract class for a web browser
    /// </summary>
    public abstract class WebBrowser :  IDisposable
    {
        protected static readonly int DEFAULT_RENDER_WIDTH = 1033;
        protected static readonly int DEFAULT_RENDER_HEIGHT = 729;

        protected readonly Size _size;

        /// <summary>
        /// The web browser default styles
        /// </summary>
        /// <value>
        /// The default styles
        /// </value>
        public DefaultStyleLookup DefaultStyles { get; private set; }

        public WebBrowser()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebBrowser" /> class
        /// </summary>
        /// <param name="size">The size</param>
        /// <param name="defaultStyles">The default styles</param>
        public WebBrowser(Size size, DefaultStyleLookup defaultStyles)
        {
            _size = size;
            this.DefaultStyles = defaultStyles;
            this.RenderingComplete = false;
        }

        /// <summary>
        /// Whether the browser has an url
        /// </summary>
        /// <value>
        /// true, if browser has an url, false otherwise
        /// </value>
        public virtual bool HasUrl
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.Url);
            }
        }

        /// <summary>
        /// Gets the URL
        /// </summary>
        /// <value>
        /// The URL
        /// </value>
        public virtual string Url { get; }

        /// <summary>
        /// Gets the original HTML
        /// </summary>
        /// <value>
        /// The original HTML
        /// </value>
        public virtual string OriginalHtml { get; }

        /// <summary>
        /// Gets whether the browser rendering is complete
        /// </summary>
        /// <value>
        ///   <c>true</c> if the browser rendering is complete; otherwise, <c>false</c>.
        /// </value>
        public virtual bool RenderingComplete { get; protected set; }

        /// <summary>
        /// Renders an URL
        /// </summary>
        /// <param name="url">The URL</param>
        public abstract void RenderUrl(string url);

        /// <summary>
        /// Renders a HTML string
        /// </summary>
        /// <param name="html">the HTML string</param>
        public abstract void RenderHtml(string html);

        /// <summary>
        /// Processes browser internal events
        /// </summary>
        public abstract void DoEvents();

        /// <summary>
        /// Modifies the DOM
        /// </summary>
        /// <param name="modification">The modification to apply</param>
        public abstract void ModifyDom(IDomModification modification);

        /// <summary>
        /// Copies the browser DOM into our own representation
        /// </summary>
        /// <returns>the copied dom</returns>
        public abstract HtmlDocument CopyDom();
        
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources
        /// </summary>
        public abstract void Dispose();
    }
}
