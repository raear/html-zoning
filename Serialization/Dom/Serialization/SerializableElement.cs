/*
Government Usage Rights Notice:  The U.S. Government retains unlimited, royalty-free usage rights to this software, but not ownership, as provided by Federal law.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

•	Redistributions of source code must retain the above Government Usage Rights Notice, this list of conditions and the following disclaimer.

•	Redistributions in binary form must reproduce the above Government Usage Rights Notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

•	Neither the names of the National Library of Medicine, the National Institutes of Health, nor the names of any of the software developers may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE U.S. GOVERNMENT AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITEDTO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE U.S. GOVERNMENT
OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using Imppoa.HtmlZoning.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Imppoa.HtmlZoning.Dom.Serialization
{
    /// <summary>
    /// Serializable html element
    /// </summary>
    public class SerializableElement : HtmlElement, IXmlSerializable
    {
        public int OuterHtmlStartPos { get; private set; }
        public int OuterHtmlEndPos { get; private set; }
        public int InnerHtmlStartPos { get; private set; }
        public int InnerHtmlEndPos { get; private set; }
        public int TextStartPos { get; private set; }
        public int TextEndPos { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableElement"/> class
        /// </summary>
        public SerializableElement()
        {
        }

        /// <summary>
        /// Initializes the serializable element
        /// </summary>
        /// <param name="element">The html element</param>
        /// <param name="outerHtmlStartPos">The outer html start position</param>
        /// <param name="outerHtmlEndPos">The outer html end position</param>
        /// <param name="innerHtmlStartPos">The inner html start position</param>
        /// <param name="innerHtmlEndPos">The inner html end position</param>
        /// <param name="textStartPos">The text start position</param>
        /// <param name="textEndPos">The text end position</param>
        /// <param name="defaultStyleLookup">The default style lookup</param>
        public void Initialize(HtmlElement element, int outerHtmlStartPos, int outerHtmlEndPos, int innerHtmlStartPos, int innerHtmlEndPos, int textStartPos, int textEndPos, DefaultStyleLookup defaultStyleLookup)
        {
            var attributeDictionary = ConstructAttributeDictionary(element);
            var styleDictionary = ConstructStyleDictionary(element);

            this.Initialize(element.Id, element.DisplayOrder, element.ParentId, element.ChildrenIds, element.BoundingBox, element.Classifications,
                element.TagName, attributeDictionary, styleDictionary, defaultStyleLookup,
                outerHtmlStartPos, outerHtmlEndPos,
                innerHtmlStartPos, innerHtmlEndPos,
                textStartPos, textEndPos);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableElement" /> class
        /// </summary>
        /// <param name="element">The html element</param>
        /// <param name="outerHtmlStartPos">The outer html start position</param>
        /// <param name="outerHtmlEndPos">The outer html end position</param>
        /// <param name="innerHtmlStartPos">The inner html start position</param>
        /// <param name="innerHtmlEndPos">The inner html end position</param>
        /// <param name="textStartPos">The text start position</param>
        /// <param name="textEndPos">The text end position</param>
        /// <param name="defaultStyleLookup">The default style lookup</param>
        public SerializableElement(HtmlElement element, int outerHtmlStartPos, int outerHtmlEndPos, int innerHtmlStartPos, int innerHtmlEndPos, int textStartPos, int textEndPos, DefaultStyleLookup defaultStyleLookup)
        {
            this.Initialize(element, outerHtmlStartPos, outerHtmlEndPos, innerHtmlStartPos, innerHtmlEndPos, textStartPos, textEndPos, defaultStyleLookup);
        }

        /// <summary>
        /// Initializes the serializable element
        /// </summary>
        /// <param name="id">The id</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="parentId">The parent id</param>
        /// <param name="childrenIds">The children ids</param>
        /// <param name="boundingBox">The bounding box</param>
        /// <param name="classifications">The classifications</param>
        /// <param name="tagName">The tag name</param>
        /// <param name="attributes">The attributes</param>
        /// <param name="styles">The styles</param>
        /// <param name="deafultStyleLookup">The deafult style lookup</param>
        /// <param name="outerHtmlStartPos">The outer html start position</param>
        /// <param name="outerHtmlEndPos">The outer html end position</param>
        /// <param name="innerHtmlStartPos">The inner html start position</param>
        /// <param name="innerHtmlEndPos">The inner html end position</param>
        /// <param name="textStartPos">The text start position</param>
        /// <param name="textEndPos">The text end position</param>
        public void Initialize(int id, int displayOrder, int? parentId, IReadOnlyList<int> childrenIds, Rectangle boundingBox, IEnumerable<string> classifications, string tagName,
                               IDictionary<string, string> attributes, IDictionary<string, string> styles, DefaultStyleLookup deafultStyleLookup,
                               int outerHtmlStartPos, int outerHtmlEndPos,
                               int innerHtmlStartPos, int innerHtmlEndPos,
                               int textStartPos, int textEndPos)
        {
            base.Initialize(id, displayOrder, parentId, childrenIds, boundingBox, classifications, tagName, string.Empty, string.Empty, string.Empty, attributes, styles, deafultStyleLookup);

            this.OuterHtmlStartPos = outerHtmlStartPos;
            this.OuterHtmlEndPos = outerHtmlEndPos;

            this.InnerHtmlStartPos = innerHtmlStartPos;
            this.InnerHtmlEndPos = innerHtmlEndPos;

            this.TextStartPos = textStartPos;
            this.TextEndPos = textEndPos;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableElement" /> class
        /// </summary>
        /// <param name="id">The id</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="parentId">The parent id</param>
        /// <param name="childrenIds">The children ids</param>
        /// <param name="boundingBox">The bounding box</param>
        /// <param name="classifications">The classifications</param>
        /// <param name="tagName">The tag name</param>
        /// <param name="attributes">The attributes</param>
        /// <param name="styles">The styles</param>
        /// <param name="defaultStyleLookup">The default style lookup</param>
        /// <param name="outerHtmlStartPos">The outer html start position</param>
        /// <param name="outerHtmlEndPos">The outer html end position</param>
        /// <param name="innerHtmlStartPos">The inner html start position</param>
        /// <param name="innerHtmlEndPos">The inner html end position</param>
        /// <param name="textStartPos">The text start position</param>
        /// <param name="textEndPos">The text end position</param>
        public SerializableElement(int id, int displayOrder, int? parentId, IReadOnlyList<int> childrenIds, Rectangle boundingBox, IEnumerable<string> classifications, string tagName,
                               IDictionary<string, string> attributes, IDictionary<string, string> styles, DefaultStyleLookup defaultStyleLookup,
                               int outerHtmlStartPos, int outerHtmlEndPos,
                               int innerHtmlStartPos, int innerHtmlEndPos,
                               int textStartPos, int textEndPos)
        {
            this.Initialize(id, displayOrder, parentId, childrenIds, boundingBox, classifications,
                            tagName, attributes, styles, defaultStyleLookup,
                            outerHtmlStartPos, outerHtmlEndPos,
                            innerHtmlStartPos, innerHtmlEndPos,
                            textStartPos, textEndPos);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableElement" /> class
        /// For testing purposes
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
        public SerializableElement(int id, int displayOrder, int? parentId, IReadOnlyList<int> childrenIds, Rectangle boundingBox, IEnumerable<string> classifications,
            string tagName, string outerHtml, string innerHtml, string outerText,
            IDictionary<string, string> attributes, IDictionary<string, string> styles, DefaultStyleLookup defaultStyleLookup)
        {
            base.Initialize(id, displayOrder, parentId, childrenIds, boundingBox, classifications, tagName, outerHtml, innerHtml, outerText, attributes, styles, defaultStyleLookup);
        }

        /// <summary>
        /// Crops the html and text
        /// </summary>
        /// <param name="documentHtml">The document html</param>
        /// <param name="documentText">The document text</param>
        public void CropHtmlAndText(string documentHtml, string documentText)
        {
            this.OuterHtml = documentHtml.GetSubstring(this.OuterHtmlStartPos, this.OuterHtmlEndPos);
            this.InnerHtml = documentHtml.GetSubstring(this.InnerHtmlStartPos, this.InnerHtmlEndPos);
            this.OuterText = documentText.GetSubstring(this.TextStartPos, this.TextEndPos);
        }

        /// <summary>
        /// Sets the default style lookup
        /// </summary>
        /// <param name="defaultStyleLookup">The default style lookup</param>
        public new void SetDefaultStyleLookup(DefaultStyleLookup defaultStyleLookup)
        {
            base.SetDefaultStyleLookup(defaultStyleLookup);
        }

        /// <summary>
        /// Constructs an attribute dictionary for an element
        /// </summary>
        /// <param name="element">The element</param>
        /// <returns>the attribute dictionary</returns>
        private IDictionary<string, string> ConstructAttributeDictionary(HtmlElement element)
        {
            var attributeDictionary = new Dictionary<string, string>();
            foreach (string attributeName in element.AttributeNames)
            {
                string attributeValue = element.GetAttribute(attributeName);
                attributeDictionary.AddNonWhitespaceValue(attributeName, attributeValue);
            }
            return attributeDictionary;
        }

        /// <summary>
        /// Construct style dictionary
        /// </summary>
        /// <param name="element">The element</param>
        /// <returns>the style dictionary</returns>
        private IDictionary<string, string> ConstructStyleDictionary(HtmlElement element)
        {
            var styleDictionary = new Dictionary<string, string>();
            foreach (string styleName in element.NonDefaultStyleNames)
            {
                string styleValue = element.GetStyle(styleName);
                styleDictionary.AddNonWhitespaceValue(styleName, styleValue);
            }
            return styleDictionary;
        }

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute" /> to the class
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema" /> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)" /> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)" /> method
        /// </returns>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Converts an object into its XML representation
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> stream to which the object is serialized</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("Id", this.Id.ToString());
            writer.WriteElementString("DisplayOrder", this.DisplayOrder.ToString());
            writer.WriteElementString("ParentId", this.ParentId.ToString());
            writer.WriteIntList("ChildrenIds", this.ChildrenIds);
            writer.WriteElementString("TagName", this.TagName);
            this.WriteTextPosition(writer, "OuterHtml", this.OuterHtmlStartPos, this.OuterHtmlEndPos);
            this.WriteTextPosition(writer, "InnerHtml", this.InnerHtmlStartPos, this.InnerHtmlEndPos);
            this.WriteTextPosition(writer, "Text", this.TextStartPos, this.TextEndPos);
            this.WriteDictionaryValues(writer, this.Attributes, "Attributes", "Attribute", "Name", false);
            this.WriteDictionaryValues(writer, this.Styles, "Styles", "Style", "Name", false);
            this.WriteBoundingBox(writer);
            writer.WriteStringList("Classifications", this.Classifications.ToArray());
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> stream from which the object is deserialized.</param>
        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement();

            int id = reader.ReadElementContentAsInt();
            int displayOrder = reader.ReadElementContentAsInt();
            int? parentId = reader.ReadElementContentAsNullableInt();
            var childrenIds = reader.ReadElementContentAsIntList();
            string tagName = reader.ReadElementContentAsString();

            int outerHtmlStartPos, outerHtmlEndPos;
            this.ReadTextPosition(reader, out outerHtmlStartPos, out outerHtmlEndPos);

            int innerHtmlStartPos, innerHtmlEndPos;
            this.ReadTextPosition(reader, out innerHtmlStartPos, out innerHtmlEndPos);

            int textStartPos, textEndPos;
            this.ReadTextPosition(reader, out textStartPos, out textEndPos);
       
            var attributes = this.ReadDictionaryValues(reader, "Attributes", "Attribute", "Name");
            var styles = this.ReadDictionaryValues(reader, "Styles", "Style", "Name");
            Rectangle boundingBox = this.ReadBoundingBox(reader);

            IEnumerable<string> classifications = reader.ReadElementContentAsStringArray();

            reader.ReadEndElement();

            this.Initialize(id, displayOrder, parentId, childrenIds, boundingBox, classifications, tagName, attributes, styles, new DefaultStyleLookup(),
                            outerHtmlStartPos, outerHtmlEndPos,
                            innerHtmlStartPos, innerHtmlEndPos,
                            textStartPos, textEndPos);
        }

        /// <summary>
        /// Writes a text position
        /// </summary>
        /// <param name="writer">The writer</param>
        /// <param name="elementName">The element name</param>
        /// <param name="startPos">The start position</param>
        /// <param name="endPos">The end position</param>
        private void WriteTextPosition(XmlWriter writer, string elementName, int startPos, int endPos)
        {
            writer.WriteStartElement(elementName);
            writer.WriteAttributeString("StartPos", startPos.ToString());
            writer.WriteAttributeString("EndPos", endPos.ToString());
            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the attribute or style dictionary values
        /// </summary>
        /// <param name="writer">The writer</param>
        /// <param name="dict">The dictionary containing the values to write</param>
        /// <param name="collectionTag">The collection tag to use</param>
        /// <param name="itemTag">The item tag to use</param>
        /// <param name="keyAttributeName">The name of the attribute that holds the key</param>
        /// <param name="preserveWhitespace">Whether to preserve whitespace</param>
        private void WriteDictionaryValues(XmlWriter writer, IDictionary<string, string> dict, string collectionTag, string itemTag, string keyAttributeName, bool preserveWhitespace)
        {
            writer.WriteStartElement(collectionTag);
            if (preserveWhitespace)
            {
                this.AddPreserveWhitespaceAttribute(writer);
            }
            foreach (KeyValuePair<string, string> keyValuePair in dict)
            {
                writer.WriteStartElement(itemTag);
                writer.WriteAttributeString(keyAttributeName, keyValuePair.Key);
                writer.WriteString(keyValuePair.Value);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        /// <summary>
        /// Writes the bounding box
        /// </summary>
        /// <param name="writer">The writer</param>
        private void WriteBoundingBox(XmlWriter writer)
        {
            string boundingBoxString = string.Format("{0},{1},{2},{3}",
            this.BoundingBox.X.ToString(), this.BoundingBox.Y.ToString(), this.BoundingBox.Width.ToString(), this.BoundingBox.Height.ToString());
            writer.WriteElementString("BoundingBox", boundingBoxString);
        }

        /// <summary>
        /// Reads a text position
        /// </summary>
        /// <param name="reader">The reader</param>
        /// <param name="startPos">(out) The start position</param>
        /// <param name="endPos">(out) The end position</param>
        private void ReadTextPosition(XmlReader reader, out int startPos, out int endPos)
        {         
            string startPosString = reader.GetAttribute("StartPos");
            string endPosString = reader.GetAttribute("EndPos");
         
            int.TryParse(startPosString, out startPos);
            int.TryParse(endPosString, out endPos);
            reader.ReadElementContentAsString();
        }

        /// <summary>
        /// Reads the attribute or style dictionary values
        /// </summary>
        /// <param name="reader">The reader</param>
        /// <param name="collectionTag">The collection tag</param>
        /// <param name="itemTag">The item tag</param>
        /// <param name="keyAttributeName">The name of the attribute that holds the key</param>
        /// <returns>the created dictionary of values</returns>
        private IDictionary<string, string> ReadDictionaryValues(XmlReader reader, string collectionTag, string itemTag, string keyAttributeName)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (reader.IsEmptyElement)
            {
                reader.ReadStartElement(collectionTag);
            }
            else
            {
                reader.ReadStartElement(collectionTag);
                this.MoveToContent(reader);
                while (reader.Name == itemTag)
                {
                    string name = reader.GetAttribute(keyAttributeName);
                    string value = reader.ReadElementContentAsString(itemTag, string.Empty);
                    dict.AddNonWhitespaceValue(name, value);
                    this.MoveToContent(reader);
                }

                reader.ReadEndElement();
            }

            return dict;
        }

        /// <summary>
        /// Moves past whitespace
        /// </summary>
        /// <param name="reader">The XML reader</param>
        private void MoveToContent(XmlReader reader)
        {
            if (reader.NodeType == XmlNodeType.SignificantWhitespace)
            {
                reader.MoveToContent();
            }
        }

        /// <summary>
        /// Reads the bounding box
        /// </summary>
        /// <param name="reader">The XML reader</param>
        /// <returns>the bounding box</returns>
        private Rectangle ReadBoundingBox(XmlReader reader)
        {
            string boundingBoxString = reader.ReadElementContentAsString();
            string[] dims = boundingBoxString.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            int x = int.Parse(dims[0]);
            int y = int.Parse(dims[1]);
            int width = int.Parse(dims[2]);
            int height = int.Parse(dims[3]);
            return new Rectangle(x, y, width, height);
        }

        /// <summary>
        /// Adds the preserve whitespace attribute
        /// </summary>
        /// <param name="writer">The XML writer</param>
        private void AddPreserveWhitespaceAttribute(XmlWriter writer)
        {
            writer.WriteAttributeString("xml:space", "preserve");
        }
    }
}
