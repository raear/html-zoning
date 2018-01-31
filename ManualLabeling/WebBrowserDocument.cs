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
using Imppoa.HtmlRendering.DomModifications;
using Imppoa.HtmlZoning;
using Imppoa.HtmlZoning.Dom;
using Imppoa.HtmlZoning.Dom.Serialization;
using Imppoa.HtmlZoning.ElementClassification;
using Imppoa.HtmlZoning.ZoneTreeConstruction;
using mshtml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.Expando;

namespace Imppoa.ManualLabeling
{
    public class WebBrowserDocument : IBrowserDocument
    {
        private static readonly int SCROLL_BAR_WIDTH = 17;
        private static readonly string OPACITY_CUST_ATTR_NAME = "data-labeling-opacity";

        private readonly IDomModification[] _domModifications;
        private readonly PreZoningClassification _elementClassifier;
        private readonly ZoneTreeBuilder _zoneTreeBuilder;
        private readonly MsHtmlDocumentFactory _documentFactory;

        private readonly string _url;
        private readonly IHTMLDocument2 _msDoc;
        private readonly List<HtmlHighlight> _highlights;

        private bool _domModificationsApplied;
 
        public event EventHandler<Rectangle> HighlightsUpdated;

        public WebBrowserDocument(string url, IHTMLDocument2 msDoc)
        {
            _domModifications = new IDomModification[]
                {
                    new RemoveOrphanElements(),
                    new RemoveUnknownElements(),
                    new RemoveCommentElements(),
                    new RemoveEmptyFreeTextNodes(),
                    new ReplaceFreeTextNodes(),
                    new ReplacePreservedWhitespace(),
                    new ReplaceFreeTextNodes(),
                };
         
            _elementClassifier = new PreZoningClassification(HtmlElementType.SignificantBlock, HtmlElementType.SignificantInline, HtmlElementType.SignificantLinebreak, HtmlElementType.SignificantInvisible, HtmlElementType.BreakDown, HtmlElementType.Aname, HtmlElementType.Hidden);
            _zoneTreeBuilder = ZoneTreeBuilder.Create(HtmlElementType.SignificantBlock, HtmlElementType.SignificantInline, HtmlElementType.SignificantLinebreak, HtmlElementType.SignificantInvisible, HtmlElementType.BreakDown, HtmlElementType.Aname, HtmlElementType.Hidden);
            var defaultStyleLookup = DefaultStyleLookup.CreateForInternetExplorer();
            _documentFactory = new MsHtmlDocumentFactory(defaultStyleLookup);

            _url = url;
            _msDoc = msDoc;
            _highlights = new List<HtmlHighlight>();

            _domModificationsApplied = false;
        }

        public bool CanDoZoning()
        {
            return true;
        }

        public ZoneTree DoZoning()
        {
            this.ApplyDomModifications();
            var msdocument = _documentFactory.Create(_msDoc, _url);
            string html, text;
            var sDocument = SerializableDocument.Create(msdocument, out html, out text);
            _elementClassifier.Execute(sDocument);
            var zoneTree = _zoneTreeBuilder.Build(sDocument);
            return zoneTree;
        }

        public Bitmap TakeScreenshot(ZoneTree tree)
        {
            Bitmap screenshot = null;
            try
            {
                var html = (IHTMLElement2)_msDoc.body.parentElement;
                var htmlStyle = html.runtimeStyle;
                string overflowX = html.currentStyle.getAttribute(Css.Properties.OVERFLOWX)?.ToString();
                htmlStyle.setAttribute(Css.Properties.OVERFLOWX, Css.Values.OVERFLOW_HIDDEN);

                var verticalScrollArea = this.GetVerticalScrollArea(tree);

                var viewObject = (IViewObject)_msDoc;
                var window = _msDoc.parentWindow;
                int viewPortWidth = verticalScrollArea.Width;
                int viewPortHeight = (int)((IExpando)window).GetProperty("innerHeight", BindingFlags.Default).GetValue(window);
                int scrollHeight = verticalScrollArea.Height;
                int numScrolls = (int)Math.Ceiling((double)scrollHeight / viewPortHeight);

                screenshot = new Bitmap(viewPortWidth, scrollHeight);
                using (var screenshotGraphics = Graphics.FromImage(screenshot))
                {
                    screenshotGraphics.Clear(Color.Red);
                    for (int i = 0; i < numScrolls; i++)
                    {
                        if (i == 1)
                        {
                            new HidePositionFixed(OPACITY_CUST_ATTR_NAME).Apply(_msDoc);
                        }

                        bool last = (i == (numScrolls - 1));
                        int scrollTop;
                        if (last)
                        {
                            scrollTop = (scrollHeight - viewPortHeight);
                        }
                        else
                        {
                            scrollTop = (i * viewPortHeight);
                        }

                        window.scroll(0, scrollTop);
                        using (var fragment = new Bitmap(viewPortWidth, viewPortHeight))
                        using (var fragmentGraphics = Graphics.FromImage(fragment))
                        {
                            var fragmentHdc = fragmentGraphics.GetHdc();
                            try
                            {
                                viewObject.Draw(1, -1, (IntPtr)0, (IntPtr)0, (IntPtr)0, fragmentHdc, Rectangle.Empty, (IntPtr)0, (IntPtr)0, 0);
                            }
                            finally
                            {
                                fragmentGraphics.ReleaseHdc(fragmentHdc);
                            }
                            screenshotGraphics.DrawImage(fragment, 0, scrollTop);
                        }
                    }
                }

                new ShowPositionFixed(OPACITY_CUST_ATTR_NAME).Apply(_msDoc);
                if (overflowX != null)
                    htmlStyle.setAttribute(Css.Properties.OVERFLOWX, overflowX);
                else
                    htmlStyle.setAttribute(Css.Properties.OVERFLOWX, Css.Values.OVERFLOW_VISIBLE);
                window.scroll(0, 0);
            }
            catch
            {
                screenshot?.Dispose();
                screenshot = null;
            }

            return screenshot;
        }

        public void ShowHighlights(ZoneTree tree, IEnumerable<HighlightSpec> highlightSpecs)
        {
            var toRemove = _highlights.Except(highlightSpecs).ToArray();
            foreach (HtmlHighlight highlight in toRemove)
            {
                highlight.Remove();
                _highlights.Remove(highlight);
            }

            var toAdd = highlightSpecs.Except(_highlights);
            foreach (var spec in toAdd)
            {
                var highlight = HtmlHighlight.Create(_msDoc, spec);
                _highlights.Add(highlight);
            }

            var verticalScrollAreas = this.GetVerticalScrollArea(tree);
            this.NotifyHighlightsUpdated(verticalScrollAreas);
        }

        public void ClearHighlights()
        {
            foreach (HtmlHighlight highlight in _highlights.ToList())
            {
                highlight.Remove();
                _highlights.Remove(highlight);
            }
            this.NotifyHighlightsUpdated(Rectangle.Empty);
        }

        private void NotifyHighlightsUpdated(Rectangle verticalScrollArea)
        {
            this.HighlightsUpdated?.Invoke(this, verticalScrollArea);
        }

        private void ApplyDomModifications()
        {
            if (!_domModificationsApplied)
            {
                foreach(var modification in _domModifications)
                {
                    modification.Apply(_msDoc);
                }
                _domModificationsApplied = true;
            }
        }

        private Rectangle GetVerticalScrollArea(ZoneTree tree)
        {
            int width;
            int height;
            if (tree == null)
            {
                var body = (IHTMLElement2)_msDoc.body;
                width = body.clientWidth;
                height = body.clientHeight;
            }
            else
            {
                var window = _msDoc.parentWindow;
                width = (int)((IExpando)window).GetProperty("innerWidth", BindingFlags.Default).GetValue(window);
                width -= SCROLL_BAR_WIDTH;
                height = tree.Document.All.Max(e => e.BoundingBox.Bottom);
            }

            var bb = new Rectangle(0, 0, width, height);
            return bb;
        }
    }
}
