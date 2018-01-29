/*
Government Usage Rights Notice:  The U.S. Government retains unlimited, royalty-free usage rights to this software, but not ownership, as provided by Federal law.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

•	Redistributions of source code must retain the above Government Usage Rights Notice, this list of conditions and the following disclaimer.

•	Redistributions in binary form must reproduce the above Government Usage Rights Notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

•	Neither the names of the National Library of Medicine, the National Institutes of Health, nor the names of any of the software developers may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE U.S. GOVERNMENT AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITEDTO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE U.S. GOVERNMENT
OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Imppoa.HtmlZoning.Dom.Serialization
{
    [XmlRoot("HtmlDocument")]
    [Serializable]
    public class SerializableElementList
    {
        [XmlAttribute("fileVersion")]
        public string FileVersion = "1.0";

        [XmlAttribute("url")]
        public string Url;

        [XmlAttribute("browserVersion")]
        public string BrowserVersion;

        [XmlAttribute("codeVersion")]
        public string CodeVersion;

        [XmlAttribute("creationDate")]
        public DateTime CreationDate;

        [XmlArray("HtmlElements")]
        [XmlArrayItem("HtmlElement")]
        public List<SerializableElement> SerializableElements;

        public SerializableElementList()
        {
        }

        public SerializableElementList(List<SerializableElement> elements, HtmlDocumentInfo info)
        {
            this.SerializableElements = elements;
            this.Url = info.Url;
            this.BrowserVersion = info.BrowserVersion;
            this.CodeVersion = info.CodeVersion;
            this.CreationDate = info.CreationDate;
        }
    }
}
