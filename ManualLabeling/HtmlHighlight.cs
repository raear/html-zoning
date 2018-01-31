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
using System;
using System.Drawing;

namespace Imppoa.ManualLabeling
{
    public class HtmlHighlight : HighlightSpec,  IDisposable
    {
        private static readonly string BACKGROUND_COLOR_CSS_TEMPLATE = "rgba({0},{1},{2},{3})";
        private static readonly string BORDER_CSS_TEMPLATE = "{0}px {1} rgb({2},{3},{4})";
        private static string NO_BORDER_STYLE = "none";
        private static string SOLID_BORDER_STYLE = "solid";

        public static HtmlHighlight Create(IHTMLDocument2 doc, HighlightSpec spec, Action<IHTMLEventObj> mouseDownCallBack = null, Action<IHTMLEventObj> mouseMoveCallBack = null, Action<IHTMLEventObj> mouseUpCallBack = null)
        {
            // Create element
            var div = doc.createElement(Html.Tags.DIV);
            var body = (IHTMLElement2) doc.body;
            var element = body.insertAdjacentElement("BeforeEnd", div);

            // Set call backs
            var events = (HTMLElementEvents2_Event)element;
            if (mouseDownCallBack != null) { events.onmousedown += new HTMLElementEvents2_onmousedownEventHandler(mouseDownCallBack); }
            if (mouseMoveCallBack != null) { events.onmousemove += new HTMLElementEvents2_onmousemoveEventHandler(mouseMoveCallBack); }
            if (mouseUpCallBack != null) { events.onmouseup += new HTMLElementEvents2_onmouseupEventHandler(mouseUpCallBack); }

            // Compute style values
            var style = spec.Style;
            Rectangle bb = spec.BoundingBox;
            if (style.ShowBorder)
            {
                bb = new Rectangle(bb.Left, bb.Top, bb.Width - 2, bb.Height - 2);
            }

            var borderStyle = style.ShowBorder ? SOLID_BORDER_STYLE : NO_BORDER_STYLE;
            var backgroundColorCss = string.Format(BACKGROUND_COLOR_CSS_TEMPLATE, (ushort)style.BackgroundColor.R, (ushort)style.BackgroundColor.G, (ushort)style.BackgroundColor.B, style.BackgroundOpacity);
            var borderCss = string.Format(BORDER_CSS_TEMPLATE, style.BorderWidth, borderStyle, (ushort)style.BorderColor.R, (ushort)style.BorderColor.G, (ushort)style.BorderColor.B);
            var zIndexCss = style.ZIndex.ToString();

            // Set style values
            var runtimeStyle = ((IHTMLElement2)element).runtimeStyle;

            runtimeStyle.setAttribute("position", "absolute");
            runtimeStyle.pixelLeft = bb.Left;
            runtimeStyle.pixelTop = bb.Top;
            runtimeStyle.pixelWidth = bb.Width;
            runtimeStyle.pixelHeight = bb.Height;
         
            runtimeStyle.backgroundColor = backgroundColorCss;
            runtimeStyle.border = borderCss;
            runtimeStyle.zIndex = zIndexCss;

            var highlight = new HtmlHighlight(spec, element);
            return highlight;
        }

        private IHTMLElement _element;

        private HtmlHighlight(HighlightSpec spec, IHTMLElement element)
            : base(spec)
        {
            _element = element;
        }

        public void Remove()
        {
            ((IHTMLDOMNode)_element).removeNode(true);
        }

        public void Dispose()
        {
            this.Remove();
        }
    }
}
