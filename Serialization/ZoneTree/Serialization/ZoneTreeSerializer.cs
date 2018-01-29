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
using Imppoa.HtmlZoning.Dom.Serialization;
using Imppoa.HtmlZoning.Utils;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace Imppoa.HtmlZoning.Serialization
{
    /// <summary>
    /// Serializes a zone tree
    /// </summary>
    public class ZoneTreeSerializer
    {
        private readonly XmlSerializer _serializer;
        private XmlWriterSettings _writerSettings;
        private XmlReaderSettings _readerSettings;
        private XmlSerializerNamespaces _namespaces;
        private readonly HtmlDocumentSerializer _docSerializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoneTreeSerializer"/> class
        /// </summary>
        public ZoneTreeSerializer()
            : this(DefaultStyleLookup.CreateForInternetExplorer())
        {
        }

        /// <summary>
        /// Initializes the <see cref="ZoneTree" /> class
        /// </summary>
        /// <param name="defaultStyleLookup">The default style lookup</param>
        public ZoneTreeSerializer(DefaultStyleLookup defaultStyleLookup)
        {
            _serializer = new XmlSerializer(typeof(SerializableZoneNodeArray));
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
            _docSerializer = new HtmlDocumentSerializer(defaultStyleLookup);
        }

        /// <summary>
        /// Serializes a zone tree
        /// </summary>
        /// <param name="zoneTree">The zone tree</param>
        /// <returns>
        /// the zone tree xml
        /// </returns>
        public virtual string Serialize(ZoneTree zoneTree)
        {
            try
            {
                var orderedNodes = zoneTree.All.OrderBy(n => n.Id).ToArray();
                var toSerialize = new SerializableZoneNodeArray { Zones = orderedNodes };

                using (var stringWriter = new StringWriter())
                using (var xmlWriter = XmlWriter.Create(stringWriter, _writerSettings))
                {
                    _serializer.Serialize(xmlWriter, toSerialize, _namespaces);
                    return stringWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new SerializationException("Error serializing zone tree", ex);
            }
        }

        /// <summary>
        /// Deserializes a zone tree
        /// </summary>
        /// <param name="documentHtml">The document HTML</param>
        /// <param name="documentText">The document text</param>
        /// <param name="labeledDocumentXml">The labeled document XML</param>
        /// <param name="zoneTreeXml">The zone tree XML</param>
        /// <param name="displayHtml">(optional) The display HTML</param>
        /// <returns>
        /// the zone tree
        /// </returns>
        /// <exception cref="System.Runtime.Serialization.SerializationException">Error deserializing zone tree</exception>
        public virtual ZoneTree Deserialize(string documentHtml, string documentText, string labeledDocumentXml, string zoneTreeXml, string displayHtml = null)
        {
            try
            {
                var document = _docSerializer.Deserialize(documentHtml, documentText, labeledDocumentXml, displayHtml);
                this.ConfirmDocumentElementsAreClassified(document); // Otherwise throw exception
                return this.Deserialize(document, zoneTreeXml);
            }
            catch (Exception ex)
            {
                throw new SerializationException("Error deserializing zone tree", ex);
            }
        }

        /// <summary>
        /// Deserializes a zone tree
        /// </summary>
        /// <param name="labeledDocument">The labeled document</param>
        /// <param name="zoneTreeXml">The zone tree XML</param>
        /// <returns>
        /// The deserialized zone tree
        /// </returns>
        private ZoneTree Deserialize(SerializableDocument labeledDocument, string zoneTreeXml)
        {
            SerializableZoneNodeArray zoneNodeArray;
            using (var stringReader = new StringReader(zoneTreeXml))
            using (var xmlReader = XmlReader.Create(stringReader, _readerSettings))
            {
                zoneNodeArray = (SerializableZoneNodeArray)_serializer.Deserialize(xmlReader);
            }

            var zoneNodes = zoneNodeArray.Zones;
            var htmlElements = labeledDocument.All;

            var zoneNodeLookup = SerializationUtils.CreateLookup(zoneNodes);
            var htmlElementLookup = SerializationUtils.CreateLookup(htmlElements);

            foreach (var zoneNode in zoneNodes)
            {
                zoneNode.Link(zoneNodeLookup, htmlElementLookup);
            }

            foreach (var zoneNode in zoneNodes)
            {
                zoneNode.CropText(labeledDocument.Text);
            }
            return new ZoneTree(zoneNodes[0], labeledDocument);
        }

        /// <summary>
        /// Confirms that the document elements are classified
        /// </summary>
        /// <param name="document">The document</param>
        private void ConfirmDocumentElementsAreClassified(HtmlDocument document)
        {
            bool elementsNotLabeled = document.All.All(e => e.ClassificationsCount == 0);
            if (elementsNotLabeled)
            {
                throw new Exception("DOM elements are not labeled");
            }
        }
    }
}
