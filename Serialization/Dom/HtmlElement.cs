/*
Government Usage Rights Notice:  The U.S. Government retains unlimited, royalty-free usage rights to this software, but not ownership, as provided by Federal law.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

•	Redistributions of source code must retain the above Government Usage Rights Notice, this list of conditions and the following disclaimer.

•	Redistributions in binary form must reproduce the above Government Usage Rights Notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

•	Neither the names of the National Library of Medicine, the National Institutes of Health, nor the names of any of the software developers may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE U.S. GOVERNMENT AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITEDTO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE U.S. GOVERNMENT
OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using Imppoa.HtmlZoning.TreeStructure;
using Imppoa.HtmlZoning.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Imppoa.HtmlZoning.Dom
{
    /// <summary>
    /// Html element
    /// </summary>
    public abstract class HtmlElement : TreeNode<HtmlDocument,HtmlElement>
    {
        protected Dictionary<string, string> Attributes { get; private set; }
        protected Dictionary<string, string> Styles { get; private set; }

        private DefaultStyleLookup _defaultStyleLookup;

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlElement"/> class.
        /// </summary>
        public HtmlElement()
            : base()
        {
            this.SetAttributes(new Dictionary<string, string>());
            this.SetStyles(new Dictionary<string, string>());
            this.SetDefaultStyleLookup(new DefaultStyleLookup());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlElement" /> class
        /// </summary>
        /// <param name="id">The id</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="parentId">The parent id</param>
        /// <param name="childrenIds">The children ids</param>
        /// <param name="boundingBox">The bounding box</param>
        /// <param name="classifications">The classifications</param>
        /// <param name="tagName">Tag name</param>
        /// <param name="outerHtml">The outer html</param>
        /// <param name="innerHtml">The inner html</param>
        /// <param name="outerText">The outer text</param>
        /// <param name="attributes">The attributes</param>
        /// <param name="styles">The styles</param>
        /// <param name="defaultStyleLookup">The default style lookup</param>
        public HtmlElement(int id, int displayOrder, int? parentId, IReadOnlyList<int> childrenIds, Rectangle boundingBox, IEnumerable<string> classifications,
            string tagName, string outerHtml, string innerHtml, string outerText, 
            IDictionary<string, string> attributes, IDictionary<string, string> styles, DefaultStyleLookup defaultStyleLookup)
        {
            this.Initialize(id, displayOrder, parentId, childrenIds, boundingBox, classifications, tagName, outerHtml, innerHtml, outerText, attributes, styles, defaultStyleLookup);
        }

        /// <summary>
        /// Initializes the class
        /// </summary>
        /// <param name="id">The id</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="parentId">The parent identifier</param>
        /// <param name="childrenIds">The children ids</param>
        /// <param name="boundingBox">The bounding box</param>
        /// <param name="classifications">The classifications</param>
        /// <param name="tagName">The tag name</param>
        /// <param name="outerHtml">The outer html</param>
        /// <param name="innerHtml">The inner html</param>
        /// <param name="outerText">The outer text</param>
        /// <param name="attributes">The attributes</param>
        /// <param name="styles">The styles</param>
        /// <param name="defaultStyleLookup">The default style lookup</param>
        public void Initialize(int id, int displayOrder, int? parentId, IReadOnlyList<int> childrenIds, Rectangle boundingBox, IEnumerable<string> classifications,
            string tagName, string outerHtml, string innerHtml, string outerText, 
            IDictionary<string, string> attributes, IDictionary<string, string> styles, DefaultStyleLookup defaultStyleLookup)
        {
            if (attributes == null)
            {
                throw new ArgumentNullException("attributes");
            }

            if (styles == null)
            {
                throw new ArgumentNullException("styles");
            }

            if (defaultStyleLookup == null)
            {
                throw new ArgumentNullException("defaultStyleLookup");
            }

            base.Initialize(id, displayOrder, parentId, childrenIds, boundingBox, classifications);
            this.TagName = tagName;
            this.InnerHtml = innerHtml;
            this.OuterHtml = outerHtml;
            this.OuterText = outerText;
            this.SetAttributes(attributes);
            this.SetStyles(styles);
            this.SetDefaultStyleLookup(defaultStyleLookup);
        }

        #region Tree

        /// <summary>
        /// Gets the pretty print node text
        /// </summary>
        /// <returns>the pretty print node text</returns>
        public override string GetDisplayString()
        {
            return string.Format("{0}[{1}]", this.TagName, string.Join("|", this.Classifications));
        }

        /// <summary>
        /// Whether this html element equals the other tree node
        /// </summary>
        /// <param name="other">The other tree node</param>
        /// <param name="outDifferences">(out) The differences</param>
        /// <returns>
        /// Whether this html element equals the other tree node
        /// </returns>
        public override bool Equals(TreeNode other, out IEnumerable<string> outDifferences)
        {
            bool areEqual = true;
            var differences = new List<string>();

            IEnumerable<string> baseDifferences;
            areEqual = base.Equals(other, out baseDifferences);
            differences.AddRange(baseDifferences);

            HtmlElement otherElement = other as HtmlElement;
            if (otherElement == null)
            {
                differences.Add("other is not a Html Element");
            }
            else
            {
                if (this.InnerHtml != otherElement.InnerHtml)
                {
                    areEqual = false;
                    string message = string.Format("Id, {0}, different Inner Html: {1}|||{2}", this.Id, this.InnerHtml, otherElement.InnerHtml);
                    differences.Add(message);
                }

                if (this.OuterHtml != otherElement.OuterHtml)
                {
                    areEqual = false;
                    string message = string.Format("Id, {0}, different Outer Html: {1}|||{2}", this.Id, this.OuterHtml, otherElement.OuterHtml);
                    differences.Add(message);
                }

                if (this.OuterText != otherElement.OuterText)
                {
                    areEqual = false;
                    string message = string.Format("Id, {0}, different Outer Text: {1}|||{2}", this.Id, this.OuterText, otherElement.OuterText);
                    differences.Add(message);
                }

                if (this.TagName != otherElement.TagName)
                {
                    areEqual = false;
                    string message = string.Format("Id, {0}, different Tag Name: {1}|||{2}", this.Id, this.TagName, otherElement.TagName);
                    differences.Add(message);
                }

                if (this.AttributeCount != otherElement.AttributeCount)
                {
                    areEqual = false;
                    string message = string.Format("Id, {0}, different number of attributes: {1}|||{2}", this.Id, this.AttributeCount, otherElement.AttributeCount);
                    differences.Add(message);
                }

                foreach (string attributeName in this.AttributeNames)
                {
                    if (!otherElement.HasAttribute(attributeName))
                    {
                        areEqual = false;
                        string message = string.Format("Id, {0}, other element does not have, {1}, attribute", this.Id, attributeName);
                        differences.Add(message);
                    }
                }

                foreach (string attributeName in this.AttributeNames)
                {
                    if (otherElement.HasAttribute(attributeName))
                    {
                        string selfAtttributeValue = this.GetAttribute(attributeName);
                        string otherAtttributeValue = otherElement.GetAttribute(attributeName);
                        if (selfAtttributeValue != otherAtttributeValue)
                        {
                            areEqual = false;
                            string message = string.Format("Id, {0}, different, {1}, attribute value: {2}|||{3}", this.Id, attributeName, selfAtttributeValue, otherAtttributeValue);
                            differences.Add(message);
                        }
                    }
                }

                if (this.StyleCount != otherElement.StyleCount)
                {
                    areEqual = false;
                    string message = string.Format("Id, {0}, different number of styles: {1}|||{2}", this.Id, this.StyleCount, otherElement.StyleCount);
                    differences.Add(message);
                }

                foreach (string styleName in this.StyleNames)
                {
                    if (!otherElement.HasStyle(styleName))
                    {
                        areEqual = false;
                        string message = string.Format("Id, {0}, other element does not have, {1}, style", this.Id, styleName);
                        differences.Add(message);
                    }
                }

                foreach (string styleName in this.StyleNames)
                {
                    if (otherElement.HasStyle(styleName))
                    {
                        string selfStyleValue = this.GetStyle(styleName);
                        string otherStyleValue = otherElement.GetStyle(styleName);
                        if (selfStyleValue != otherStyleValue)
                        {
                            areEqual = false;
                            string message = string.Format("Id, {0}, different, {1}, style value: {2}|||{3}", this.Id, styleName, selfStyleValue, otherStyleValue);
                            differences.Add(message);
                        }
                    }
                }
            }

            outDifferences = differences;

            return areEqual;
        }

        #endregion

        #region Html element

        /// <summary>
        /// Gets the start tag
        /// </summary>
        /// <value>
        /// The start tag
        /// </value>
        public string StartTag
        {
            get
            {
                string startTag = string.Empty;
                string outerHtml = this.OuterHtml;
                if (!string.IsNullOrWhiteSpace(outerHtml))
                {
                    int startIndex = outerHtml.GetIndexOf("<");
                    int endIndex = outerHtml.GetIndexOf(">");
                    if (startIndex > -1 && endIndex > -1)
                    {
                        startTag = outerHtml.GetSubstring(startIndex, endIndex + 1);
                    }
                }

                return startTag;
            }
        }

        /// <summary>
        /// Gets the end tag
        /// </summary>
        /// <value>
        /// The end tag
        /// </value>
        public string EndTag
        {
            get
            {
                string endTag = string.Empty;

                string startTag = this.StartTag;
                string outerHtml = this.OuterHtml;
                string innerHtml = this.InnerHtml;
             
                int startTagLength = startTag.Length;
                int outerHtmlLength = outerHtml.Length;
                int innerHtmlLength = innerHtml.Length;
                int count = outerHtmlLength - startTagLength - innerHtmlLength + 1;
                if (startTagLength > 0 && count > 0)
                { 
                    int startIndex = outerHtml.LastIndexOf("<", outerHtmlLength, count);
                    int endIndex = outerHtml.LastIndexOf(">", outerHtmlLength, count);
                    if (startIndex > -1 && endIndex > -1)
                    {
                        endTag = outerHtml.GetSubstring(startIndex, endIndex + 1);
                    }
                }

                return endTag;
            }
        }

        /// <summary>
        /// Gets the tag name
        /// </summary>
        /// <value>
        /// The tag name
        /// </value>
        public string TagName { get; private set; }

        /// <summary>
        /// Gets elements with the specified tag name
        /// </summary>
        /// <param name="tag">The tag name</param>
        /// <returns>
        /// Elements with the specified tag name
        /// </returns>
        public IReadOnlyList<HtmlElement> GetElementsByTagName(string tag)
        {
            return this.GetDescendantsAndSelf().Where(e => e.TagName.ToLower() == tag.ToLower()).ToList();
        }

        /// <summary>
        /// Gets the inner HTML
        /// </summary>
        /// <value>
        /// The inner HTML
        /// </value>
        public string InnerHtml { get; protected set; }

        /// <summary>
        /// Gets the outer HTML
        /// </summary>
        /// <value>
        /// The outer HTML
        /// </value>
        public string OuterHtml { get; protected set; }

        /// <summary>
        /// Gets the outer text
        /// </summary>
        /// <value>
        /// The outer text
        /// </value>
        public string OuterText { get; protected set; }

        /// <summary>
        /// Gets or sets the custom user data
        /// </summary>
        /// <value>
        /// the custom user data
        /// </value>
        public dynamic UserData { get; set; }

        /// <summary>
        /// Determines whether element has text
        /// </summary>
        /// <returns>
        /// whether element has text
        /// </returns>
        public bool HasText()
        {
            return !string.IsNullOrEmpty(this.OuterText);
        }

        /// <summary>
        /// Whether element contains only white space
        /// </summary>
        public bool IsWhiteSpace()
        {
            return this.HasText() && string.IsNullOrWhiteSpace(this.OuterText);
        }

        /// <summary>
        /// Determines whether element contains no text or whitespace
        /// </summary>
        /// <returns>
        /// whether element contains no text or whitespace
        /// </returns>
        public bool NoTextOrWhiteSpace()
        {
            return string.IsNullOrWhiteSpace(this.OuterText);
        }

        /// <summary>
        /// Sets the attributes
        /// </summary>
        /// <param name="attributes">The attributes</param>
        private void SetAttributes(IDictionary<string, string> attributes)
        {
            this.Attributes = attributes.ToCaseInsensitive();
        }

        /// <summary>
        /// Gets the attribute names
        /// </summary>
        /// <value>
        /// The attribute names
        /// </value>
        public IReadOnlyCollection<string> AttributeNames
        {
            get
            {
                return this.Attributes.Keys.ToList();
            }
        }

        /// <summary>
        /// Gets the number of attributes
        /// </summary>
        /// <value>
        /// The number of attributes
        /// </value>
        public int AttributeCount
        {
            get
            {
                return this.Attributes.Count;
            }
        }

        /// <summary>
        /// Whether element has attribute
        /// </summary>
        /// <param name="name">The attribute name</param>
        /// <returns>
        /// Whether element has attribute
        /// </returns>
        public bool HasAttribute(string name)
        {
            return this.Attributes.ContainsKey(name);
        }

        /// <summary>
        /// Gets an attribute value
        /// </summary>
        /// <param name="stdAttributeName">The attribute name.</param>
        /// <returns>
        /// the attribute value
        /// returns string.Empty if attribute does not exist to be consistent
        /// with the windows forms implementation.
        /// </returns>
        public string GetAttribute(string attributeName)
        {
            if (this.AttributeNamesEqual(attributeName, Html.Attributes.CLASSNAME))
            {
                attributeName = Html.Attributes.CLASS.ToLower();
            }

            string attrValue = string.Empty;
            if (this.HasAttribute(attributeName))
            {
                attrValue = this.Attributes[attributeName];
            }
            return attrValue;
        }

        /// <summary>
        /// Whether two attibute names are equal
        /// </summary>
        /// <param name="firstName">The first name</param>
        /// <param name="secondName">The second name</param>
        /// <returns>true, if the names are equal, false otherwise</returns>
        private bool AttributeNamesEqual(string firstName, string secondName)
        {
            return firstName.ToLower().Trim() == secondName.ToLower().Trim();
        }

        /// <summary>
        /// Sets the styles
        /// </summary>
        /// <param name="styles">The styles</param>
        private void SetStyles(IDictionary<string, string> styles)
        {
            this.Styles = styles.ToCaseInsensitive();
        }

        /// <summary>
        /// Gets the non default style names
        /// </summary>
        /// <value>
        /// The non default style names
        /// </value>
        public IReadOnlyCollection<string> NonDefaultStyleNames
        {
            get
            {
                return this.Styles.Keys.ToList();
            }
        }

        /// <summary>
        /// Gets the style names
        /// </summary>
        /// <value>
        /// The style names
        /// </value>
        public IReadOnlyCollection<string> StyleNames 
        {  
            get
            {
                return Css.Properties.PROPERTIES_OF_INTEREST.ToList();
            }
        }

        /// <summary>
        /// Gets the number of styles
        /// </summary>
        /// <value>
        /// The number of styles
        /// </value>
        public int StyleCount
        {
            get
            {
                return this.StyleNames.Count();
            }
        }

        /// <summary>
        /// Whether element has style
        /// </summary>
        /// <param name="name">The style name</param>
        /// <returns>
        /// Whether element has style
        /// </returns>
        public bool HasStyle(string name)
        {
            return this.StyleNames.Contains(name, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets a style
        /// </summary>
        /// <param name="name">The style name</param>
        /// <returns>
        /// The style value
        /// </returns>
        public string GetStyle(string styleName)
        {
            string styleValue;
            if (this.Styles.ContainsKey(styleName))
            {
                styleValue = this.Styles[styleName];
            }
            else if (this.HasStyle(styleName))
            {
                styleValue = _defaultStyleLookup.GetDefaultValue(this.TagName, styleName);
            }
            else
            {
                throw new Exception(string.Format("Style, {0}, not found", styleName));
            }
            return styleValue;
        }

        /// <summary>
        /// Sets the default style lookup
        /// </summary>
        /// <param name="defaultStyleLookup">The default style lookup</param>
        protected void SetDefaultStyleLookup(DefaultStyleLookup defaultStyleLookup)
        {
            _defaultStyleLookup = defaultStyleLookup;
        }

        #endregion
    }
}
