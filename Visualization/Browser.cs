/*
Government Usage Rights Notice:  The U.S. Government retains unlimited, royalty-free usage rights to this software, but not ownership, as provided by Federal law.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

•	Redistributions of source code must retain the above Government Usage Rights Notice, this list of conditions and the following disclaimer.

•	Redistributions in binary form must reproduce the above Government Usage Rights Notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

•	Neither the names of the National Library of Medicine, the National Institutes of Health, nor the names of any of the software developers may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE U.S. GOVERNMENT AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITEDTO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE U.S. GOVERNMENT
OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using Imppoa.HtmlRendering;
using Imppoa.HtmlZoning.Dom.Serialization;
using mshtml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Imppoa.HtmlZoning.Visualization
{
    /// <summary>
    /// Web browser with highlights
    /// </summary>
    public partial class Browser : UserControl
    {
        protected static readonly int RENDER_WIDTH = 1200;
        protected static readonly int RENDER_HEIGHT = 729;

        private readonly double MIN_WAIT_TIME = 20; // seconds
        private readonly double MAX_WAIT_TIME = 60; // seconds
        private readonly string HIGHLIGHT_SELECTED_BORDER = "3px solid red";
        private readonly string HIGHLIGHT_SELECTED_BACKGROUND = "transparent";
        private readonly string HIGHLIGHT_MANY_BORDER = "1px solid black";
        private readonly string HIGHLIGHT_MANY_BACKGROUND = "rgba(255,255,0,0.5)";
        private readonly int HIGHLIGHT_Z_INDEX = 10000;
        private readonly int SCROLL_OFFSET = 150;
       

        private readonly List<HtmlElement> _highlightManyHighlights;
        private InProcessRenderer _renderer;
        private HtmlElement _selectedHighlight;

        internal event EventHandler UrlEntered;

        /// <summary>
        /// Initializes a new instance of the <see cref="Browser"/> class
        /// </summary>
        public Browser()
        {
            InitializeComponent();
            _selectedHighlight = null;
            _highlightManyHighlights = new List<HtmlElement>();
            bool validateResult = false;
            _renderer = InProcessRenderer.Create(MIN_WAIT_TIME, MAX_WAIT_TIME, validateResult, new InternetExplorer(_browserControl, new Size(RENDER_WIDTH, RENDER_HEIGHT)));
        }

        /// <summary>
        /// Whether the DOM has been constructed
        /// </summary>
        private bool Ready
        {
            get
            {
                return _browserControl.ReadyState == WebBrowserReadyState.Interactive
                    || _browserControl.ReadyState == WebBrowserReadyState.Complete;
            }
        }

        /// <summary>
        /// Gets the WinForms DOM
        /// </summary>
        private HtmlDocument Document
        {
            get
            {
                return _browserControl.Document;
            }
        }

        /// <summary>
        /// Gets the URL
        /// </summary>
        /// <value>
        /// The URL
        /// </value>
        internal string EnteredUrl
        {
            get
            {
                return _navigationUrlTextBox.Text;
            }
        }

        /// <summary>
        /// Navigates to the url
        /// </summary>
        /// <param name="url">The URL</param>
        internal void Navigate(string url)
        {
            _navigationUrlTextBox.Text = url;
            _renderer.SetUrl(url);
        }

        /// <summary>
        /// Waits for the rendering to finish
        /// </summary>
        internal void Wait()
        {
            _renderer.Wait();
        }

        /// <summary>
        /// Highlights the selected dom/zone node
        /// </summary>
        /// <param name="boundingBox">The bounding box</param>
        internal void HighlightSelected(Rectangle boundingBox)
        {
            if (this.Ready)
            {
                this.ClearSelectedHighlight();
                _selectedHighlight = this.InsertHighlight();
                this.StyleHighlight(_selectedHighlight, boundingBox, HIGHLIGHT_SELECTED_BACKGROUND, HIGHLIGHT_SELECTED_BORDER);
                this.Document.Window.ScrollTo(0, boundingBox.Top - SCROLL_OFFSET);
            }
        }

        /// <summary>
        /// Highlights many zones
        /// </summary>
        /// <param name="boundingBoxes">The zone bounding boxes</param>
        internal void HighlightMany(IEnumerable<Rectangle> boundingBoxes)
        {
            if (this.Ready)
            {
                this.ClearHighlightMany();
                foreach (var box in boundingBoxes)
                {
                    var highlightBox = this.InsertHighlight();
                    this.StyleHighlight(highlightBox, box, HIGHLIGHT_MANY_BACKGROUND, HIGHLIGHT_MANY_BORDER);
                    _highlightManyHighlights.Add(highlightBox);
                }
            }
        }

        /// <summary>
        /// Applies the DOM modifications
        /// </summary>
        internal void ApplyDomModifications()
        {
             _renderer.ApplyDomModifications();
        }

        /// <summary>
        /// Gets the render output
        /// </summary>
        /// <returns>
        /// the render output
        /// </returns>
        internal SerializationOutput GetRenderOutput()
        {
            return _renderer.GetRenderOutput();
        }

        /// <summary>
        /// Called when a key is pressed in the navigation text box
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="KeyPressEventArgs"/> instance containing the event data</param>
        private void OnNavigationUrlTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char) Keys.Return)
            {
                this.FireEvent(this.UrlEntered);
                this.Navigate(this.EnteredUrl);
            }
        }

        /// <summary>
        /// Inserts highlight into the DOM
        /// </summary>
        /// <returns>the inserted DOM element</returns>
        private HtmlElement InsertHighlight()
        {
            var highlightBox = this.Document.CreateElement(Dom.Html.Tags.DIV);
            this.Document.Body.InsertAdjacentElement(HtmlElementInsertionOrientation.BeforeEnd, highlightBox);
            return highlightBox;
        }

        /// <summary>
        /// Styles the highlight
        /// </summary>
        /// <param name="highlight">The highlight to style</param>
        /// <param name="dimensions">The dimensions to apply</param>
        /// <param name="borderStyle">The border style to apply</param>
        private void StyleHighlight(HtmlElement highlight, Rectangle dimensions, string backgroundStyle, string borderStyle)
        {
            string style = string.Format("background-color: {0}; position: absolute; left: {1}px; top: {2}px; width: {3}px; height: {4}px; border: {5};z-index: {6};",
            backgroundStyle, dimensions.Left, dimensions.Top, dimensions.Width, dimensions.Height, borderStyle, HIGHLIGHT_Z_INDEX);
            highlight.Style = style;
        }

        /// <summary>
        /// Clears the selected highlight
        /// </summary>
        private void ClearSelectedHighlight()
        {
            if (_selectedHighlight != null)
            {
                this.RemoveElement(_selectedHighlight);
            }
            _selectedHighlight = null;
        }

        /// <summary>
        /// Clears the highlight many highlights
        /// </summary>
        private void ClearHighlightMany()
        {
            foreach (HtmlElement hightlight in _highlightManyHighlights)
            {
                this.RemoveElement(hightlight);
            }
            _highlightManyHighlights.Clear();
        }

        /// <summary>
        /// Removes a HTML element
        /// </summary>
        /// <param name="element">The element to remove</param>
        private void RemoveElement(HtmlElement element)
        {
            var msNode = (IHTMLDOMNode) element.DomElement;
            msNode.removeNode(true);
        }

        /// <summary>
        /// Fires an event
        /// </summary>
        /// <param name="handler">The event handler</param>
        private void FireEvent(EventHandler handler)
        {
            handler?.Invoke(this, EventArgs.Empty);
        }
    }
}
