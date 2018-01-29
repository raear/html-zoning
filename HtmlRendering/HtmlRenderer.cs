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
using Imppoa.HtmlZoning.Dom.Serialization;
using System;

namespace Imppoa.HtmlRendering
{
    /// <summary>
    /// Html renderer interface
    /// </summary>
    public abstract class HtmlRenderer : IDisposable
    {
        /// <summary>
        /// Gets the default styles for the renderer
        /// </summary>
        /// <value>
        /// The default styles for the renderer
        /// </value>
        public DefaultStyleLookup DefaultStyles { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlRenderer"/> class
        /// </summary>
        /// <param name="defaultStyles">The default styles</param>
        public HtmlRenderer(DefaultStyleLookup defaultStyles)
        {
            this.DefaultStyles = defaultStyles;
        }

        /// <summary>
        /// Renders a html string
        /// </summary>
        /// <param name="html">The HTML to render</param>
        /// <returns>The render output</returns>
        public abstract SerializationOutput RenderHtml(string html);

        /// <summary>
        /// Renders an URL
        /// </summary>
        /// <param name="url">The URL to render</param>
        /// <returns>The render output</returns>
        public abstract SerializationOutput RenderUrl(string url);

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public abstract void Dispose();
    }
}
