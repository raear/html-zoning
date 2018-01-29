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

namespace Imppoa.HtmlRendering.DomModifications
{
    /// <summary>
    /// An Internet Explorer DOM modification
    /// </summary>
    public abstract class MsDomModification : IDomModification
    {
        private static readonly int ELEMENT_NODE_TYPE = 1;
        private static readonly int TEXT_NODE_TYPE = 3;
        private static readonly int COMMENT_NODE_TYPE = 8;
       
        protected IHTMLDocument2 _msDoc;

        /// <summary>
        /// Applies the modification to the DOM
        /// </summary>
        /// <param name="dom">The DOM</param>
        public void Apply(object dom)
        {
            _msDoc = (IHTMLDocument2) dom;
            var bodyNode = (IHTMLDOMNode)_msDoc.body;
            var htmlNode = bodyNode.parentNode;
            this.ApplyInternal(htmlNode);
        }

        /// <summary>
        /// Determines whether the node should be modified
        /// </summary>
        /// <param name="node">The node</param>
        /// <returns>Whether the node should be modified</returns>
        protected abstract bool ShouldModify(IHTMLDOMNode node);

        /// <summary>
        /// Modifies a node
        /// </summary>
        /// <param name="node">The node to be modified</param>
        protected abstract void Modify(IHTMLDOMNode node);

        /// <summary>
        /// Traverses the dom tree and applys the modification
        /// </summary>
        /// <param name="parent">The parent node</param>
        private void ApplyInternal(IHTMLDOMNode parent)
        {
            var children = (IHTMLDOMChildrenCollection) parent.childNodes;
            foreach (IHTMLDOMNode child in children)
            {
                if (this.ShouldModify(child))
                {
                    this.Modify(child);
                }
                else
                {
                    if (child.hasChildNodes())
                    {
                        this.ApplyInternal(child);
                    }
                }
            }
        }

        /// <summary>
        /// Whether node is a text node
        /// </summary>
        /// <param name="node">The node</param>
        protected bool IsTextNode(IHTMLDOMNode node)
        {
            return node.nodeType == TEXT_NODE_TYPE;
        }

        /// <summary>
        /// Whether node is an element node
        /// </summary>
        /// <param name="node">The node</param>
        protected bool IsElementNode(IHTMLDOMNode node)
        {
            return node.nodeType == ELEMENT_NODE_TYPE;
        }

        /// <summary>
        /// Whether node is a comment node
        /// </summary>
        /// <param name="node">The node</param>
        protected bool IsCommentNode(IHTMLDOMNode node)
        {
            return node.nodeType == COMMENT_NODE_TYPE;
        }

        /// <summary>
        /// Whether the element has the specified tag name
        /// </summary>
        /// <param name="element">The element</param>
        /// <param name="tagNameToTest">The tag name to test</param>
        protected bool ElementHasTagName(IHTMLElement element, string tagNameToTest)
        {
            string tagName = element.tagName.ToUpper().Trim();
            tagNameToTest = tagNameToTest.ToUpper().Trim();
            return tagName == tagNameToTest;
        }

        /// <summary>
        /// Whether whitespace is preserved for the text node
        /// </summary>
        /// <param name="textNode">The text node</param>
        protected bool TextNodeWhitespaceIsPreserved(IHTMLDOMNode textNode)
        {
            string whitespaceStyle = this.GetTextNodeWhitespaceStyle(textNode);
            bool whitespacePreserved = whitespaceStyle == Css.Values.WHITESPACE_PRE ||
                                       whitespaceStyle == Css.Values.WHITESPACE_PRE_LINE ||
                                       whitespaceStyle == Css.Values.WHITESPACE_PRE_WRAP;
            return whitespacePreserved;
        }

        /// <summary>
        /// Gets whitespace style for a text node
        /// </summary>
        /// <param name="textNode">The text node</param>
        protected string GetTextNodeWhitespaceStyle(IHTMLDOMNode textNode)
        {
            string whitespaceStyle = string.Empty;

            IHTMLDOMNode parentNode = textNode.parentNode;
            var parentElement2 = (IHTMLElement2)parentNode;
            var currentStyle3 = (IHTMLCurrentStyle3)parentElement2.currentStyle;
            if (currentStyle3 != null)
            {
                whitespaceStyle = currentStyle3.whiteSpace.ToLower().Trim();
            }

            return whitespaceStyle;
        }

        /// <summary>
        /// Whether text node hastext
        /// </summary>
        /// <param name="textNode">The text node</param>
        protected bool TextNodeContainsText(IHTMLDOMNode textNode)
        {
            string text = (string)textNode.nodeValue;
            return !string.IsNullOrEmpty(text);
        }

        /// <summary>
        /// Whether text node has non-whitespace text
        /// </summary>
        /// <param name="textNode">The text node</param>
        protected bool TextNodeContainsNonWhitespaceText(IHTMLDOMNode textNode)
        {
            string text = (string)textNode.nodeValue;
            return !string.IsNullOrWhiteSpace(text);
        }

        /// <summary>
        /// Whether the text node has whitespace text
        /// </summary>
        /// <param name="textNode">The text node</param>
        protected bool TextNodeContainsOnlyWhitespace(IHTMLDOMNode textNode)
        {
            string text = (string)textNode.nodeValue;
            return !string.IsNullOrEmpty(text) && string.IsNullOrWhiteSpace(text);
        }

        /// <summary>
        /// Whether node is a free text node
        /// </summary>
        /// <param name="node">The node</param>
        protected bool IsFreeTextNode(IHTMLDOMNode node)
        {
            bool isTextNode = this.IsTextNode(node);
            bool hasSiblings = (node.previousSibling != null || node.nextSibling != null);
            return isTextNode && hasSiblings;
        }
    }
}
