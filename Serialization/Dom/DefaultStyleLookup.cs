/*
Government Usage Rights Notice:  The U.S. Government retains unlimited, royalty-free usage rights to this software, but not ownership, as provided by Federal law.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

•	Redistributions of source code must retain the above Government Usage Rights Notice, this list of conditions and the following disclaimer.

•	Redistributions in binary form must reproduce the above Government Usage Rights Notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

•	Neither the names of the National Library of Medicine, the National Institutes of Health, nor the names of any of the software developers may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE U.S. GOVERNMENT AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITEDTO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE U.S. GOVERNMENT
OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using Imppoa.HtmlZoning.Properties;
using Imppoa.HtmlZoning.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace Imppoa.HtmlZoning.Dom
{
    /// <summary>
    /// Lookup for default CSS styles
    /// </summary>
    public class DefaultStyleLookup
    {
        /// <summary>
        /// Creates a default style lookup
        /// </summary>
        /// <returns>
        /// the default style lookup
        /// </returns>
        public static DefaultStyleLookup CreateForInternetExplorer()
        {
            var defaultStyles = new Dictionary<string, Dictionary<string, string>>();
            var serializer = new DataContractSerializer(typeof(Dictionary<string, string>));

            var resourceSet = Resources.ResourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, true);
            foreach (DictionaryEntry entry in resourceSet)
            {
                string tagName = (string) entry.Key;
                string xml = (string) entry.Value;

                using (var stringReader = new StringReader(xml))
                {
                    using (var xmlReader = XmlReader.Create(stringReader))
                    {
                        var elementDefaultStyles = (Dictionary<string, string>)serializer.ReadObject(xmlReader);
                        defaultStyles.Add(tagName, elementDefaultStyles);
                    }
                }
            }

            return new DefaultStyleLookup(defaultStyles);
        }

        private Dictionary<string, Dictionary<string, string>> _deafultStyles;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultStyleLookup"/> class
        /// </summary>
        public DefaultStyleLookup()
        {
            _deafultStyles = new Dictionary<string, Dictionary<string, string>>(StringComparer.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultStyleLookup"/> class
        /// </summary>
        /// <param name="defaultStyles">The default styles</param>
        public DefaultStyleLookup(Dictionary<string, Dictionary<string, string>> defaultStyles)
            : this()
        {
            foreach (var keyValuePair in defaultStyles)
            {
                _deafultStyles.Add(keyValuePair.Key, keyValuePair.Value.ToCaseInsensitive());
            }
        }
      
        /// <summary>
        /// Gets the default style value
        /// </summary>
        /// <param name="tagName">The tag name</param>
        /// <param name="styleName">The style name</param>
        /// <returns>
        /// the default style value
        /// </returns>
        public string GetDefaultValue(string tagName, string styleName)
        {
            bool containsTagName = _deafultStyles.ContainsKey(tagName);
            bool constainsTagNameStyle = false;
            if (containsTagName)
            {
                constainsTagNameStyle = _deafultStyles[tagName].ContainsKey(styleName);
            }

            if (containsTagName && constainsTagNameStyle)
            {
                return _deafultStyles[tagName][styleName];
            }

            containsTagName = _deafultStyles.ContainsKey(Html.Tags.SPAN);
            constainsTagNameStyle = false;
            if (containsTagName)
            {
                constainsTagNameStyle = _deafultStyles[Html.Tags.SPAN].ContainsKey(styleName);
            }

            if (containsTagName && constainsTagNameStyle)
            {
                return _deafultStyles[Html.Tags.SPAN][styleName];
            }
            else
            {
                string message = string.Format("Style, {0}, not found", styleName);
                throw new Exception(message);
            }
        }

        /// <summary>
        /// Determines whether the style value is the default style value
        /// </summary>
        /// <param name="tagName">The tag name</param>
        /// <param name="styleName">The style name</param>
        /// <param name="value">The style value</param>
        /// <returns>
        /// true, if is the default value, false otherwise
        /// </returns>
        public bool IsDefaultValue(string tagName, string styleName, string value)
        {
            string defaultValue = this.GetDefaultValue(tagName, styleName).ToLower().Trim();
            value = value.ToLower().Trim();
            bool isDefault = (defaultValue == value);
            return isDefault;
        }
    }
}
