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
using Imppoa.HtmlZoning.Dom;
using mshtml;

namespace Imppoa.ManualLabeling
{
    public class HidePositionFixed : MsDomModification
    {
        private static readonly string DEFAULT_OPACITY_VALUE = "1";
        private readonly string _opacityAttrName;

        public HidePositionFixed(string opacityAttrName)
        {
            _opacityAttrName = opacityAttrName;
        }

        protected override bool ShouldModify(IHTMLDOMNode node)
        {
            bool shouldModify = false;
            if (this.IsElementNode(node))
            {
                var element1 = (IHTMLElement)node;
                string opacityAttrValue = element1.getAttribute(_opacityAttrName)?.ToString();
                if (string.IsNullOrWhiteSpace(opacityAttrValue))
                {
                    var element = (IHTMLElement2)node;
                    var position = element.currentStyle?.position?.Trim()?.ToLower();
                    shouldModify = position == Css.Values.POSITION_FIXED;
                }
            }
            return shouldModify;
        }

        protected override void Modify(IHTMLDOMNode node)
        {
            var element2 = (IHTMLElement2)node;
            string opacity = element2.currentStyle.getAttribute(Css.Properties.OPACITY)?.ToString();
            element2.runtimeStyle.setAttribute(Css.Properties.OPACITY, 0);

            if (string.IsNullOrWhiteSpace(opacity))
            {
                opacity = DEFAULT_OPACITY_VALUE;
            }

            var element1 = (IHTMLElement)node;
            element1.setAttribute(_opacityAttrName, opacity);
        }
    }
}
