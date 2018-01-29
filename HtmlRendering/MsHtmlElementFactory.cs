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
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices.Expando;

namespace Imppoa.HtmlRendering
{
    /// <summary>
    /// Factory for MsHtml elements
    /// </summary>
    public class MsHtmlElementFactory
    {
        private readonly int _sourceIndexOffset;
        private readonly DefaultStyleLookup _defaultStyleLookup;
      
        /// <summary>
        /// Initializes a new instance of the <see cref="MsHtmlElementFactory"/> class
        /// </summary>
        /// <param name="sourceIndexOffset">The source index offset</param>
        /// <param name="defaultStyleLookup">The default style lookup</param>
        public MsHtmlElementFactory(int sourceIndexOffset, DefaultStyleLookup defaultStyleLookup)
        {
            _sourceIndexOffset = sourceIndexOffset;
            _defaultStyleLookup = defaultStyleLookup;
        }

        /// <summary>
        /// Creates an MsHtmlElement
        /// </summary>
        /// <param name="msElement">The IE DOM element</param>
        /// <returns>the created instance</returns>
        public MsHtmlElement Create(object msElement)
        {
            return this.Create((IHTMLElement)msElement);
        }

        /// <summary>
        /// Creates an MsHtmlElement
        /// </summary>
        /// <param name="msElement">The IE DOM element</param>
        /// <returns>the created instance</returns>
        private MsHtmlElement Create(IHTMLElement msElement)
        {
            if (msElement == null)
            {
                throw new ArgumentNullException("msElement");
            }

            int id = this.GetId(msElement);
            int? parentId = this.GetParentId(msElement);
            int[] childrenIds = this.GetChildrenIds(msElement);
            string tagName = msElement.tagName;
            string innerHtml = this.GetInnerHtml(msElement);
            string outerHtml = this.GetOuterHtml(msElement);
            string outerText = this.GetOuterText(msElement);

            int? offsetParentId = this.GetOffsetParentId(msElement);
            Rectangle offsetRectangle = this.GetOffsetRectangle(msElement);

            Dictionary<string, string> attributes = this.GetAttributes(msElement);
            Dictionary<string, string> styles = this.GetStyles(tagName, msElement);
            IEnumerable<string> classifications = new List<string>();

            var msHtmlElement = new MsHtmlElement(id, id, parentId, childrenIds, classifications, tagName, outerHtml, innerHtml, outerText, attributes, styles, _defaultStyleLookup, offsetParentId, offsetRectangle);
            return msHtmlElement;
        }

        /// <summary>
        /// Gets the id
        /// </summary>
        /// <param name="msElement">The IE DOM element</param>
        /// <returns>the id</returns>
        private int GetId(IHTMLElement msElement)
        {
            return msElement.sourceIndex - _sourceIndexOffset;
        }

        /// <summary>
        /// Gets a nullbale id
        /// </summary>
        /// <param name="msElement">The IE DOM element</param>
        /// <returns>the nullbale id</returns>
        private int? GetNullableId(IHTMLElement msElement)
        {
            int? nullableId = null;
            int id = this.GetId(msElement);
            if (id > -1)
            {
                nullableId = id;
            }

            return nullableId;
        }

        /// <summary>
        /// Gets the parent id
        /// </summary>
        /// <param name="msElement">The IE DOM element</param>
        /// <returns>the parent id</returns>
        private int? GetParentId(IHTMLElement msElement)
        {
            int? parentId = null;

            var msHtmlParent = msElement.parentElement;
            if (msHtmlParent != null)
            {
                parentId = this.GetNullableId(msHtmlParent);
            }

            return parentId;
        }

        /// <summary>
        /// Gets the children ids
        /// </summary>
        /// <param name="msElement">The IE DOM element</param>
        /// <returns>
        /// the children ids
        /// </returns>
        private int[] GetChildrenIds(IHTMLElement msElement)
        {
            var childrenIds = new List<int>();
            var msHtmlChildren = (IHTMLElementCollection)msElement.children;
            foreach (IHTMLElement msHtmlChild in msHtmlChildren)
            {
                int? id = this.GetNullableId(msHtmlChild);
                if (id.HasValue)
                {
                    childrenIds.Add(id.Value);
                }
            }
            return childrenIds.ToArray();
        }

        /// <summary>
        /// Gets the outer html
        /// </summary>
        /// <param name="msElement">The IE DOM element</param>
        /// <returns>the outer html</returns>
        private string GetOuterHtml(IHTMLElement msElement)
        {
            string outerHtml = msElement.outerHTML ?? string.Empty;
            outerHtml = HtmlHelper.RemoveLineBreaks(outerHtml);
            outerHtml = HtmlHelper.CollapseWhitespace(outerHtml);
            return outerHtml;
        }

        /// <summary>
        /// Gets the inner html
        /// </summary>
        /// <param name="msElement">The IE DOM element</param>
        /// <returns>the inner html</returns>
        private string GetInnerHtml(IHTMLElement msElement)
        {
            string innerHtml = msElement.innerHTML ?? string.Empty;
            innerHtml = HtmlHelper.RemoveLineBreaks(innerHtml);
            innerHtml = HtmlHelper.CollapseWhitespace(innerHtml);
            return innerHtml;
        }

        /// <summary>
        /// Gets the outer text
        /// </summary>
        /// <param name="msElement">The IE DOM element</param>
        /// <returns>the outer text</returns>
        private string GetOuterText(IHTMLElement msElement)
        {
            string outerText = msElement.outerText ?? string.Empty;
            return outerText;
        }

        /// <summary>
        /// Gets the offset parent id
        /// </summary>
        /// <param name="msElement">The IE DOM element</param>
        /// <returns>The offset parent id</returns>
        private int? GetOffsetParentId(IHTMLElement msElement)
        {
            int? offsetParentId = null;

            IHTMLElement msHtmlOffsetParent = msElement.offsetParent;
            if (msHtmlOffsetParent != null)
            {
                offsetParentId = this.GetNullableId(msHtmlOffsetParent);
            }

            return offsetParentId;
        }

        /// <summary>
        /// Gets the offset rectangle
        /// </summary>
        /// <param name="msElement">The IE DOM element</param>
        /// <returns>The offset rectangle</returns>
        private Rectangle GetOffsetRectangle(IHTMLElement msElement)
        {
            int left = msElement.offsetLeft;
            int top = msElement.offsetTop;
            int width = msElement.offsetWidth;
            int height = msElement.offsetHeight;
            Rectangle rect = new Rectangle(left, top, width, height);
            return rect;
        }

        /// <summary>
        /// Gets all attributes on the element
        /// </summary>
        /// <param name="msElement">The IE DOM element</param>
        /// <returns>
        /// Dictionary of attribute values
        /// </returns>
        private Dictionary<string, string> GetAttributes(IHTMLElement msElement)
        {
            var attributeDictionary = new Dictionary<string, string>();
            var msNode = (IHTMLDOMNode)msElement;
            var attributeCollection = (IHTMLAttributeCollection)msNode.attributes;
            if (attributeCollection != null)
            {
                for (int index = 0; index < attributeCollection.length; index++)
                {
                    var attribute = (IHTMLDOMAttribute)attributeCollection.item(index);

                    string attributeName;
                    object attributeValue;
                    bool success = this.TryGetAttributeValue(attribute, out attributeName, out attributeValue);
                    if (success)
                    {
                        string attributeValueString = this.AttributeValueToString(attributeValue);
                        if (attributeValueString.Length > 0)
                        {
                            this.AddToDictionary(attributeDictionary, attributeName, attributeValueString);
                        }
                    }
                }
            }

            return attributeDictionary;
        }

        /// <summary>
        /// Tries to get the attribute name and value
        /// </summary>
        /// <param name="attribute">The attribute</param>
        /// <param name="attributeName">(out) The attribute name</param>
        /// <param name="attributeValue">(out) The attribute value</param>
        /// <returns>
        /// true, if able to get valid attribute name and value, false otherwise
        /// </returns>
        private bool TryGetAttributeValue(IHTMLDOMAttribute attribute, out string attributeName, out object attributeValue)
        {
            bool success = false;
            attributeName = null;
            attributeValue = null;

            try
            {
                if (attribute.specified)
                {
                    attributeName = attribute.nodeName;
                    attributeValue = attribute.nodeValue;
                    if (!string.IsNullOrWhiteSpace(attributeName) && attributeValue != null)
                    {
                        success = true;
                    }
                }
            }
            catch (NotImplementedException)
            {
                success = false;
                attributeName = null;
                attributeValue = null;
            }

            return success;
        }

        /// <summary>
        /// Converts an attribute value to string
        /// </summary>
        /// <param name="attributeValueObject">The attribute value object</param>
        /// <returns>the string representation</returns>
        private string AttributeValueToString(object attributeValueObject)
        {
            string attributeValueString = attributeValueObject.ToString();
            attributeValueString = HtmlHelper.RemoveLineBreaks(attributeValueString);
            attributeValueString = HtmlHelper.CollapseWhitespace(attributeValueString);
            attributeValueString = attributeValueString.Trim();
            return attributeValueString;
        }

        /// <summary>
        /// Gets all styles of interest
        /// </summary>
        /// <param name="tagName">The tag name</param>
        /// <param name="msElement">The IE DOM element</param>
        /// <returns>
        /// Dictionary of style values
        /// </returns>
        private Dictionary<string, string> GetStyles(string tagName, IHTMLElement msElement)
        {
            var styleDictionary = new Dictionary<string, string>();

            var computedStyles = this.GetComputedStyles(msElement);
            if (computedStyles != null)
            {
                foreach (string styleName in Css.Properties.PROPERTIES_OF_INTEREST)
                {
                    string value = this.GetComputedStyleValue(computedStyles, styleName);
                    this.AddStyleValue(styleDictionary, tagName, styleName, value);
                }
            }

            return styleDictionary;
        }

        /// <summary>
        /// Gets the computed styles
        /// </summary>
        /// <param name="element">The IE DOM element</param>
        /// <returns>The computed styles, null if error</returns>
        private object GetComputedStyles(IHTMLElement element)
        {
            object computedStyles = null;

            try
            {
                var doc2 = (IHTMLDocument2)element.document;
                var win2 = doc2.parentWindow;
                var method = ((IExpando)win2).GetMethod("getComputedStyle", BindingFlags.Default);
                if (method != null)
                {
                    computedStyles = method.Invoke(win2, new[] { element });
                }
            }
            catch
            {
                // Nothing to do here
            }

            return computedStyles;
        }

        /// <summary>
        /// Gets a computed style value
        /// </summary>
        /// <param name="computedStyles">The computed styles</param>
        /// <param name="styleName">The style name</param>
        /// <returns>The style value</returns>
        private string GetComputedStyleValue(object computedStyles, string styleName)
        {
            var property = ((IExpando)computedStyles).GetProperty(styleName, BindingFlags.Default);
            string value = property.GetValue(computedStyles).ToString();
            return value;
        }

        /// <summary>
        /// Adds a style value to the style dictionary
        /// But only if it is not a default value
        /// </summary>
        /// <param name="dic">The style dictionary</param>
        /// <param name="tagName">The tag name</param>
        /// <param name="name">The style name</param>
        /// <param name="value">The style value</param>
        private void AddStyleValue(Dictionary<string, string> dic, string tagName, string name, string value)
        {
            if (!_defaultStyleLookup.IsDefaultValue(tagName, name, value))
            {
                this.AddToDictionary(dic, name, value);
            }
        }

        /// <summary>
        /// Gets the font size
        /// First trys to get the computed font size but falls back to
        /// runtime style technique
        /// </summary>
        /// <param name="msElement">The IE DOM element</param>
        /// <returns>the font size as a string</returns>
        private string GetFontSize(IHTMLElement msElement)
        {
            // Save original value
            var element2 = (IHTMLElement2)msElement;
            var style = element2.runtimeStyle;
            object left = style.left;

            // Get pixel size of 1em
            style.left = Css.Values.LEFT_1EM;
            int fontSize = style.pixelLeft;

            // Revert left value
            if (left != null)
            {
                style.left = left;
            }
            else
            {
                style.left = Css.Values.LEFT_AUTO;
            }

            return fontSize.ToString();
        }

        /// <summary>
        /// Adds a key value pair to a string-string dictionary
        /// Only adds value if it is non whitespace
        /// Overwrites if key already exists
        /// </summary>
        /// <param name="dict">The dictionary</param>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        private void AddToDictionary(Dictionary<string, string> dict, string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                bool keyExists = dict.ContainsKey(key);
                if (keyExists)
                {
                    dict[key] = value;
                }
                else
                {
                    dict.Add(key, value);
                }
            }
        }
    }
}
