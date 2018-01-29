/*
Government Usage Rights Notice:  The U.S. Government retains unlimited, royalty-free usage rights to this software, but not ownership, as provided by Federal law.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

•	Redistributions of source code must retain the above Government Usage Rights Notice, this list of conditions and the following disclaimer.

•	Redistributions in binary form must reproduce the above Government Usage Rights Notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

•	Neither the names of the National Library of Medicine, the National Institutes of Health, nor the names of any of the software developers may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE U.S. GOVERNMENT AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITEDTO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE U.S. GOVERNMENT
OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using Imppoa.HtmlRendering.DomModifications;
using Imppoa.HtmlZoning.Dom.Serialization;
using System;
using System.Collections.Generic;

namespace Imppoa.HtmlRendering
{
    /// <summary>
    /// Renders html in process
    /// </summary>
    public class InProcessRenderer : HtmlRenderer
    {
        /// <summary>
        /// Creates an instance with the default configuration
        /// </summary>
        /// <param name="minRenderTime">The minimum render time in seconds (default: 10s)</param>
        /// <param name="maxRenderTime">The maximum render time in seconds (default: 60s)</param>
        /// <param name="validateSerializationResult">Whether to validate the serialization result (default: false)</param>
        /// <param name="browser">The browser (default: Internet Explorer)</param>
        /// <param name="domModifications">The dom modifications to apply (default: see below)</param>
        /// <returns>
        /// the instance with the default configuration
        /// </returns>
        public static InProcessRenderer Create(double minRenderTime = 20, double maxRenderTime = 60, bool validateSerializationResult = false, WebBrowser browser = null, IEnumerable<IDomModification> domModifications = null)
        {
            if (domModifications == null)
            {
                domModifications = new IDomModification[]
                {
                    new RemoveOrphanElements(),
                    new RemoveUnknownElements(),
                    new RemoveCommentElements(),
                    new RemoveEmptyFreeTextNodes(),
                    new ReplaceFreeTextNodes(),
                    new ReplacePreservedWhitespace(),
                    new ReplaceFreeTextNodes(),
                };
            }

            if (browser == null)
            {
                browser = new InternetExplorer();
            }

            var spinWait = new SpinWait(SPIN_INTERVAL);
            return new InProcessRenderer(browser, domModifications, minRenderTime, maxRenderTime, validateSerializationResult, spinWait);
        }

        private static readonly int SPIN_INTERVAL = 100;

        private SpinWait _spinWait;
        private double _minRenderTime;
        private double _maxRenderTime;
        private WebBrowser _browser;
    
        private HtmlDocumentSerializer _domSerializer;
        private IEnumerable<IDomModification> _domModifications;

        private bool _domModificationsApplied;

        /// <summary>
        /// Initializes a new instance of the <see cref="InProcessRenderer" /> class.
        /// </summary>
        /// <param name="browser">The browser</param>
        /// <param name="domModifications">The dom modifications to apply</param>
        /// <param name="minRenderTime">The minimum render time in seconds (default: 10s)</param>
        /// <param name="maxRenderTime">The maximum render time in seconds (default: 60s)</param>
        /// <param name="validateSerializationResult">Whether to validate the serialization result</param>
        /// <param name="spinWait">The spin wait to use</param>
        public InProcessRenderer(WebBrowser browser, IEnumerable<IDomModification> domModifications, double minRenderTime, double maxRenderTime, bool validateSerializationResult, SpinWait spinWait)
            : base(browser.DefaultStyles)
        {
            _browser = browser;
            _domModifications = domModifications;
            _minRenderTime = minRenderTime * 1000;
            _maxRenderTime = maxRenderTime * 1000;
            _domSerializer = new HtmlDocumentSerializer(browser.DefaultStyles);
            _spinWait = spinWait;
            _spinWait.OnSpin = () => _browser.DoEvents();
            _spinWait.Condition = this.RenderingFinished;

            _domModificationsApplied = false;
        }

        /// <summary>
        /// Renders a html string
        /// </summary>
        /// <param name="html">The HTML to render</param>
        /// <returns>
        /// The render output
        /// </returns>
        public override SerializationOutput RenderHtml(string html)
        {
            this.SetHtml(html);
            this.Wait();
            this.ApplyDomModifications();
            return this.GetRenderOutput();
        }

        /// <summary>
        /// Renders an URL
        /// </summary>
        /// <param name="url">The URL to render</param>
        /// <returns>
        /// The render output
        /// </returns>
        public override SerializationOutput RenderUrl(string url)
        {
            this.SetUrl(url);
            this.Wait();
            string displayHtml = this.GetDisplayHtml();
            this.ApplyDomModifications();
            return this.GetRenderOutput();
        }

        /// <summary>
        /// Sets the HTML
        /// </summary>
        /// <param name="html">The HTML</param>
        /// <exception cref="System.Exception">Error rendering html</exception>
        public void SetHtml(string html)
        {
            try
            {
                _domModificationsApplied = false;
                _browser.RenderHtml(html);
            }
            catch (Exception ex)
            {
                throw new Exception("Error rendering html", ex);
            }
        }

        /// <summary>
        /// Sets the URL
        /// </summary>
        /// <param name="url">The URL</param>
        /// <exception cref="System.Exception">Error rendering url</exception>
        public void SetUrl(string url)
        {
            try
            {
                _domModificationsApplied = false;
                _browser.RenderUrl(url);
            }
            catch (Exception ex)
            {
                throw new Exception("Error rendering url", ex);
            }
        }

        /// <summary>
        /// Waits for rendering to complete
        /// </summary>
        public void Wait()
        {
            _spinWait.Wait();
        }

        /// <summary>
        /// Applies the DOM modifications if they have not been applied before
        /// </summary>
        public void ApplyDomModifications()
        {
            try
            {
                if (!_domModificationsApplied)
                {
                    foreach (var modification in _domModifications)
                    {
                        _browser.ModifyDom(modification);
                    }                   
                    _domModificationsApplied = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error applying DOM modifications", ex);
            }
        }

        /// <summary>
        /// Gets the display html
        /// </summary>
        /// <returns>the display html</returns>
        public string GetDisplayHtml()
        {
            try
            {
                string displayHtml = _browser.OriginalHtml;
                if (displayHtml != null)
                {
                    if (_browser.HasUrl)
                    {
                        string url = _browser.Url;
                        if (WebUtils.UrlIsValid(url))
                        {
                            string baseUrl = WebUtils.GetBaseUrl(url);
                            displayHtml = WebUtils.InsertBaseTag(displayHtml, baseUrl);
                        }
                    }
                }
                return displayHtml;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting display html", ex);
            }
        }

        /// <summary>
        /// Gets the render output
        /// </summary>
        /// <returns>
        /// the render output
        /// </returns>
        public SerializationOutput GetRenderOutput()
        {
            try
            {
                var dom = _browser.CopyDom();
                dom.DisplayHtml = this.GetDisplayHtml();
                return _domSerializer.Serialize(dom);
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting render output", ex);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources
        /// </summary>
        public override void Dispose()
        {
            _browser.Dispose();
        }

        /// <summary>
        /// Determines whether rendering has finished
        /// </summary>
        /// <param name="elapsedMilliseconds">The elapsed milliseconds</param>
        /// <returns>true, if rendering has finished, false otherwise</returns>
        private bool RenderingFinished(double elapsedMilliseconds)
        {
            bool beforeMinRenderTime = elapsedMilliseconds < _minRenderTime;
            bool afterMaxRenderTime = elapsedMilliseconds >= _maxRenderTime;
            bool browserRenderComplete = _browser.RenderingComplete;
            return afterMaxRenderTime || (browserRenderComplete && !beforeMinRenderTime);
        }
    }
}
