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
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace Imppoa.HtmlZoning.Dom.Serialization
{
    /// <summary>
    /// Html document serializer
    /// </summary>
    public class HtmlDocumentSerializer
    {
        private XmlSerializer _serializer;
        private XmlWriterSettings _writerSettings;
        private XmlReaderSettings _readerSettings;
        private XmlSerializerNamespaces _namespaces;
        private readonly bool _validateResult;
        private readonly DefaultStyleLookup _defaultStyleLookup;

        /// <summary>
        /// Initializes the <see cref="HtmlDocumentSerializer" /> class
        /// </summary>
        public HtmlDocumentSerializer()
            : this(DefaultStyleLookup.CreateForInternetExplorer())
        {
        }

        /// <summary>
        /// Initializes the <see cref="HtmlDocumentSerializer" /> class
        /// </summary>
        /// <param name="defaultStyleLookup">The default style lookup</param>
        /// <param name="validateResult">Whether to validate the serialization result</param>
        public HtmlDocumentSerializer(DefaultStyleLookup defaultStyleLookup, bool validateResult = false)
        {
            _serializer = new XmlSerializer(typeof(SerializableElementList));
            _writerSettings = new XmlWriterSettings
            {
                Indent = true,
                CheckCharacters = false,
                NewLineHandling = NewLineHandling.Entitize,
            };
            _readerSettings = new XmlReaderSettings
            {
                CheckCharacters = false,
                IgnoreWhitespace = true,
            };
            _namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            _defaultStyleLookup = defaultStyleLookup;
            _validateResult = validateResult;
        }

        /// <summary>
        /// Serializes the html document
        /// </summary>
        /// <param name="doc">The html document</param>
        /// <returns>The render output</returns>
        public virtual SerializationOutput Serialize(HtmlDocument doc)
        {
            try
            {
                string documentHtml;
                string documentText;
                var factory = new SerializableDocumentFactory();
                var serializableDoc = factory.Create(doc, out documentHtml, out documentText);

                string documentXml = this.Serialize(serializableDoc);
                if (_validateResult)
                {
                    this.ValidateResult(doc, documentHtml, documentText, documentXml);
                }

                return new SerializationOutput { DomTreeXml = documentXml, DomHtml = documentHtml, DomText = documentText, DisplayHtml = doc.DisplayHtml };
            }
            catch (Exception ex)
            {
                throw new SerializationException("Error serializing html document", ex);
            }
        }

        /// <summary>
        /// Deserializes a html document
        /// </summary>
        /// <param name="documentHtml">The document HTML</param>
        /// <param name="documentText">The document text</param>
        /// <param name="documentXml">The document text</param>
        /// <param name="displayHtml">(optional) The display HTML</param>
        /// <returns>
        /// the html document
        /// </returns>
        /// <exception cref="System.Runtime.Serialization.SerializationException">Error deserializing html document</exception>
        public virtual SerializableDocument Deserialize(string documentHtml, string documentText, string documentXml, string displayHtml = null)
        {
            try
            {
                SerializableElementList elementArray;
                using (var stringReader = new StringReader(documentXml))
                using (var xmlReader = XmlReader.Create(stringReader, _readerSettings))
                {
                    elementArray = (SerializableElementList)_serializer.Deserialize(xmlReader);
                }

                var elements = elementArray.SerializableElements;
                var elementLookup = SerializationUtils.CreateLookup(elements);
                foreach (var element in elements)
                {
                    element.Link(elementLookup);
                    element.CropHtmlAndText(documentHtml, documentText);
                    element.SetDefaultStyleLookup(_defaultStyleLookup);
                }

                var info = new HtmlDocumentInfo(elementArray.Url, elementArray.BrowserVersion, elementArray.CodeVersion, elementArray.CreationDate);
                var doc = new SerializableDocument(elements[0], info, _defaultStyleLookup, documentHtml, documentText);
                doc.DisplayHtml = displayHtml;
                return doc;
            }
            catch (Exception ex)
            {
                throw new SerializationException("Error deserializing html document", ex);
            }
        }

        /// <summary>
        /// Serializes a serializable document
        /// </summary>
        /// <param name="document">The serializable document</param>
        /// <returns>Document XML</returns>
        private string Serialize(SerializableDocument document)
        {
            var orderedElements = document.All.OrderBy(n => n.Id).ToArray().ConvertAll<SerializableElement>();
            var toSerialize = new SerializableElementList(orderedElements, document.Info);

            using (var stringWriter = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(stringWriter, _writerSettings))
            {
                _serializer.Serialize(xmlWriter, toSerialize, _namespaces);
                return stringWriter.ToString();
            }
        }

        /// <summary>
        /// Validates the serialization result
        /// Checks that the deserialized document is equal to the original document
        /// </summary>
        /// <param name="originalDocument">The original document</param>
        /// <param name="documentHtml">The document html</param>
        /// <param name="documentText">The document text</param>
        /// <param name="documentXml">The document XML</param>
        private void ValidateResult(HtmlDocument originalDocument, string documentHtml, string documentText, string documentXml)
        {
            var deserializedDocument = this.Deserialize(documentHtml, documentText, documentXml);
            IEnumerable<string> differences;
            if (!originalDocument.Equals(deserializedDocument, out differences))
            {
                string message = "Serialization validation failed because the deserialized document is not equal to the orginal document";
                throw new Exception(message);
            }
        }
    }
}
