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
using Imppoa.HtmlZoning.TreeStructure;
using Imppoa.HtmlZoning.TreeStructure.Filters;
using Imppoa.HtmlZoning.TreeStructure.Walkers;
using Imppoa.HtmlZoning.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Imppoa.HtmlZoning
{
    /// <summary>
    /// Zone: merged html elements
    /// </summary>
    public class Zone : TreeNode<ZoneTree, Zone>, IXmlSerializable
    {
        private List<SerializableElement> _elements;
        private List<int> _elementIds;

        /// <summary>
        /// Initializes a new instance of the <see cref="Zone"/> class
        /// </summary>
        public Zone()
            : base()
        {
            _elements = new List<SerializableElement>();
            _elementIds = new List<int>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Zone" /> class
        /// </summary>
        /// <param name="id">The id</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="parentId">The parent id</param>
        /// <param name="childrenIds">The children ids</param>
        /// <param name="elementIds">The element ids</param>
        /// <param name="classifications">The classifications</param>
        public Zone(int id, int displayOrder, int? parentId, IReadOnlyList<int> childrenIds, IReadOnlyList<int> elementIds, IEnumerable<string> classifications)
            : this()
        {
            this.Initialize(id, displayOrder, parentId, childrenIds, elementIds, classifications);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Zone"/> class
        /// </summary>
        /// <param name="id">The id</param>
        /// <param name="parent">The parent</param>
        public Zone(int id, Zone parent)
             : this()
        {
            base.Initialize(id, -1, parent, new TreeNode[0], Rectangle.Empty, new string[0]);
        }

        /// <summary>
        /// Initializes the zone node
        /// </summary>
        /// <param name="id">The id</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="parentId">The parent id</param>
        /// <param name="childrenIds">The children ids</param>
        /// <param name="elementIds">The element ids</param>
        /// <param name="classifications">The classifications</param>
        public void Initialize(int id, int displayOrder, int? parentId, IReadOnlyList<int> childrenIds, IReadOnlyList<int> elementIds, IEnumerable<string> classifications)
        {
            if (elementIds == null)
            {
                throw new ArgumentNullException("elementIds");
            }

            base.Initialize(id, displayOrder, parentId, childrenIds, Rectangle.Empty, classifications);
            this.SetElementIds(elementIds);
        }

        #region tree node

        /// <summary>
        /// Sets the parent
        /// </summary>
        /// <param name="parent">The parent</param>
        public void SetParent(Zone parent)
        {
            base.SetParent(parent);
        }

        /// <summary>
        /// Sets the children
        /// </summary>
        /// <param name="children">The children</param>
        public void SetChildren(IReadOnlyList<Zone> children)
        {
            base.SetChildren(children);
        }

        /// <summary>
        /// Adds a child to the end of the list
        /// </summary>
        /// <param name="children">The child</param>
        public void AddChild(Zone child)
        {
            base.AddChild(child);
        }

        /// <summary>
        /// Adds children to the end of the list
        /// </summary>
        /// <param name="children">The children</param>
        public void AddChildren(IReadOnlyList<Zone> children)
        {
            base.AddChildren(children);
        }

        /// <summary>
        /// Inserts a child at the specified index
        /// </summary>
        /// <param name="index">The index</param>
        /// <param name="child">The child</param>
        public void InsertChildAt(int index, Zone child)
        {
            base.InsertChildAt(index, child);
        }

        /// <summary>
        /// Insert the children at the specified index
        /// </summary>
        /// <param name="index">The index</param>
        /// <param name="children">The children</param>
        public void InsertChildrenAt(int index, IReadOnlyList<Zone> children)
        {
            base.InsertChildrenAt(index, children);
        }

        /// <summary>
        /// Removes a child
        /// </summary>
        /// <param name="child">The child</param>
        public void RemoveChild(Zone child)
        {
            base.RemoveChild(child);
        }

        /// <summary>
        /// Removes all children
        /// </summary>
        public new void RemoveAllChildren()
        {
            base.RemoveAllChildren();
        }

        /// <summary>
        /// Creates the tree links
        /// </summary>
        /// <param name="zoneLookup">The zone node lookup</param>
        /// <param name="elementLookup">The html element lookup</param>
        public void Link(IDictionary<int, TreeNode> zoneLookup, IDictionary<int, TreeNode> elementLookup)
        {
            base.Link(zoneLookup);
            foreach (int elementId in this.ElementIds)
            {
                _elements.Add((SerializableElement)elementLookup[elementId]);
            }
        }

        #endregion

        #region Elements

        /// <summary>
        /// Gets the HTML elements
        /// </summary>
        /// <value>
        /// The HTML elements
        /// </value>
        public IReadOnlyList<SerializableElement> Elements
        {
            get
            {
                return _elements;
            }
        }

        /// <summary>
        /// Gets the element ids
        /// </summary>
        /// <value>
        /// The element ids
        /// </value>
        public IReadOnlyList<int> ElementIds
        {
            get
            {
                return _elementIds;
            }
        }

        /// <summary>
        /// Sets the element ids
        /// </summary>
        /// <param name="elementIds">The element ids</param>
        private void SetElementIds(IReadOnlyList<int> elementIds)
        {
            _elementIds = new List<int>(elementIds);
        }

        /// <summary>
        /// Sets the html elements
        /// </summary>
        /// <param name="elements">The html elements</param>
        public void SetElements(IReadOnlyList<SerializableElement> elements)
        {
            if (elements == null)
            {
                throw new ArgumentNullException("elements");
            }

            _elements = new List<SerializableElement>(elements);
            var ids = this.GetIds(elements);
            this.SetElementIds(ids);
        }

        /// <summary>
        /// Adds a html element
        /// </summary>
        /// <param name="element">The html element</param>
        public void AddElement(SerializableElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            _elements.Add(element);
            _elementIds.Add(element.Id);
        }

        /// <summary>
        /// Adds the html elements
        /// </summary>
        /// <param name="elements">The html elements</param>
        public void AddElements(IReadOnlyList<SerializableElement> elements)
        {
            if (elements == null)
            {
                throw new ArgumentNullException("elements");
            }

            _elements.AddRange(elements);
            _elementIds.AddRange(this.GetIds(elements));
        }

        /// <summary>
        /// Inserts the html element at the specified index
        /// </summary>
        /// <param name="index">The index</param>
        /// <param name="element">The html element</param>
        public void InsertElementAt(int index, SerializableElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            _elements.Insert(index, element);
            _elementIds.Insert(index, element.Id);
        }

        /// <summary>
        /// Inserts html elements at the specified index
        /// </summary>
        /// <param name="index">The index</param>
        /// <param name="elements">The html elements</param>
        public void InsertElementsAt(int index, IReadOnlyList<SerializableElement> elements)
        {
            if (elements == null)
            {
                throw new ArgumentNullException("elements");
            }

            _elements.InsertRange(index, elements);
            _elementIds.InsertRange(index, this.GetIds(elements));
        }

        /// <summary>
        /// Removes the html element
        /// </summary>
        /// <param name="element">The html element</param>
        public void RemoveElement(SerializableElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            _elements.Remove(element);
            _elementIds.Remove(element.Id);
        }

        /// <summary>
        /// Removes all html elements
        /// </summary>
        public void RemoveAllElements()
        {
            _elements.Clear();
            _elementIds.Clear();
        }

        /// <summary>
        /// Gets the number of Html elements
        /// </summary>
        /// <value>
        /// The number of Html elements
        /// </value>
        public int ElementCount
        {
            get
            {
                return _elements.Count;
            }
        }

        #endregion

        #region Bounding box

        /// <summary>
        /// Gets the bounding box
        /// </summary>
        /// <value>
        /// The bounding box
        /// </value>
        public override Rectangle BoundingBox
        {
            get
            {
                return this.ComputeBoundingBox(this.Elements);
            }

            protected set
            {
                // Cannot set
            }
        }

        #endregion

        #region Html element like

        /// <summary>
        /// Gets the tag string
        /// </summary>
        /// <value>
        /// The tag string
        /// </value>
        public string TagString
        {
            get
            {
                var tagNames = _elements.Select(n => n.TagName);
                return string.Join("&", tagNames);
            }
        }

        /// <summary>
        /// Gets all elements (including descendants) contained within the zone
        /// </summary>
        /// <param name="walker">The walker</param>
        /// <returns>
        /// the descendant elements
        /// </returns>
        public IReadOnlyList<SerializableElement> GetDescendantElements(TreeWalker walker = null)
        {
            var descendantElements = new List<SerializableElement>();
            foreach (var element in _elements)
            {
                var toAdd = element.GetDescendantsAndSelf(walker).ConvertAll<SerializableElement>();
                descendantElements.AddRange(toAdd);
            }
            return descendantElements;
        }

        /// <summary>
        /// Gets html elements by tag name
        /// </summary>
        /// <param name="tagName">The tag name</param>
        /// <returns>html elements by tag name</returns>
        public IReadOnlyList<SerializableElement> GetElementsByTagName(string tagName)
        {
            var elementsWithTag = new List<SerializableElement>();
            foreach (var element in _elements)
            {
                var toAdd = element.GetElementsByTagName(tagName).ConvertAll<SerializableElement>();
                elementsWithTag.AddRange(toAdd);
            }
            return elementsWithTag;
        }

        /// <summary>
        /// Gets the HTML start position
        /// </summary>
        /// <value>
        /// The HTML start position
        /// </value>
        public int HtmlStartPos
        {
            get
            {
                return _elements.First().OuterHtmlStartPos;
            }
        }

        /// <summary>
        /// Gets the HTML end position
        /// </summary>
        /// <value>
        /// The HTML end position
        /// </value>
        public int HtmlEndPos
        {
            get
            {
                return _elements.Last().OuterHtmlEndPos;
            }
        }

        /// <summary>
        /// Gets the html
        /// </summary>
        /// <value>
        /// The html
        /// </value>
        public string Html
        {
            get
            {
                return _elements.Select(z => z.OuterHtml).Aggregate((current, next) => current + next);
            }
        }

        /// <summary>
        /// Gets the text start position
        /// </summary>
        /// <value>
        /// The text start position
        /// </value>
        public int TextStartPos
        {
            get
            {
                return _elements.First().TextStartPos;
            }
        }

        /// <summary>
        /// Gets the text end position
        /// </summary>
        /// <value>
        /// The text end position
        /// </value>
        public int TextEndPos
        {
            get
            {
                return _elements.Last().TextEndPos;
            }
        }

        /// <summary>
        /// Gets the visible text index ranges
        /// </summary>
        /// <returns>the visible text index ranges</returns>
        private IReadOnlyList<Range<int>> GetVisibleTextIndexRanges()
        {
            var indexRanges = new List<Range<int>>();
            var walker = new DepthFirstWalker(new LeafFilter());
            var orderedLeafElements = this.GetDescendantElements(walker).OrderBy(e => e.DisplayOrder);

            Range<int> currentRange = null;
            foreach (SerializableElement element in orderedLeafElements)
            {
                bool isVisible = element.HasArea() && !element.HasClassification(HtmlElementType.Hidden);
                if (isVisible)
                {
                    if (currentRange == null)
                    {
                        currentRange = new Range<int>()
                        {
                            Min = element.TextStartPos,
                            Max = element.TextEndPos,
                        };
                        indexRanges.Add(currentRange);
                    }
                    else
                    {
                        currentRange.Max = element.TextEndPos;
                    }
                }
                else
                {
                    if (currentRange != null)
                    {
                        currentRange.Max = element.TextStartPos;
                    }
                    currentRange = null;
                }
            }

            return indexRanges;
        }

        /// <summary>
        /// Gets the text
        /// </summary>
        /// <value>
        /// The text
        /// </value>
        public string Text { get; protected set; }

        /// <summary>
        /// Gets the visible text
        /// </summary>
        /// <value>
        /// The visible text
        /// </value>
        public string VisibleText { get; protected set; }

        /// <summary>
        /// Sets the text
        /// </summary>
        /// <param name="text">The text</param>
        public void SetText(string text)
        {
            this.Text = text;
            this.VisibleText = text;
        }

        /// <summary>
        /// Crops the text
        /// </summary>
        /// <param name="documentText">The document text</param>
        public void CropText(string documentText)
        {
            this.Text = documentText.GetSubstring(this.TextStartPos, this.TextEndPos);

            this.VisibleText = string.Empty;
            foreach(var range in this.GetVisibleTextIndexRanges())
            {
                this.VisibleText += documentText.GetSubstring(range.Min, range.Max);
            }
        }

        #endregion

        #region Zone specific

        /// <summary>
        /// Gets or sets the custom user data
        /// </summary>
        /// <value>
        /// the custom user data
        /// </value>
        public dynamic UserData { get; set; }

        /// <summary>
        /// Gets the type
        /// </summary>
        /// <value>
        /// The type
        /// </value>
        public ZoneType Type { get; private set; }

        /// <summary>
        /// Sets the type
        /// </summary>
        /// <param name="type">The type</param>
        public void SetType(ZoneType type)
        {
            this.Type = type;
        }

        #endregion

        /// <summary>
        /// Whether this zone node equals the other tree node
        /// </summary>
        /// <param name="other">The other tree node</param>
        /// <param name="outDifferences">(out) The differences</param>
        /// <returns>
        /// Whether this zone node equals the other tree node
        /// </returns>
        public override bool Equals(TreeNode other, out IEnumerable<string> outDifferences)
        {
            bool areEqual = true;
            var differences = new List<string>();

            IEnumerable<string> baseDifferences;
            areEqual = base.Equals(other, out baseDifferences);
            differences.AddRange(baseDifferences);

            Zone otherZone = other as Zone;
            if (otherZone == null)
            {
                differences.Add("Other tree node is not a zone node");
            }
            else
            {
                if (this.Type!= otherZone.Type)
                {
                    areEqual = false;
                    string message = string.Format("Zone, {0}, different type: {1}|||{2}", this.Id, this.Type, otherZone.Type);
                    differences.Add(message);
                }

                if (this.ElementCount != otherZone.ElementCount)
                {
                    areEqual = false;
                    string message = string.Format("Zone, {0}, different number of elements: {1}|||{2}", this.Id, this.ElementCount, otherZone.ElementCount);
                    differences.Add(message);
                }
                else
                {
                    if (!this.ElementIds.SequenceEqual(otherZone.ElementIds))
                    {
                        areEqual = false;
                        string message = string.Format("Id, {0}, different element id sequence: {1}|||{2}", this.Id, string.Join(",", this.ElementIds), string.Join(",", otherZone.ElementIds));
                        differences.Add(message);
                    }
                }
            }

            outDifferences = differences;

            return areEqual;
        }

        /// <summary>
        /// Gets the pretty print node text
        /// </summary>
        /// <returns>
        /// the pretty print node text
        /// </returns>
        public override string GetDisplayString()
        {
            return this.TagString + "[" + string.Join("|", this.Classifications) + "]";
        }

        /// <summary>
        /// Gets the element ids
        /// </summary>
        /// <param name="elements">The elements</param>
        /// <returns>the element ids</returns>
        private IReadOnlyList<int> GetIds(IReadOnlyList<TreeNode> elements)
        {
            return elements.Select(e => e.Id).ToList();
        }

        #region IXmlSerializable

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute" /> to the class
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema" /> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)" /> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)" /> method
        /// </returns>
        public virtual XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Converts an object into its XML representation
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> stream to which the object is serialized</param>
        public virtual void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("Id", this.Id.ToString());
            writer.WriteElementString("DisplayOrder", this.DisplayOrder.ToString());
            writer.WriteElementString("ParentId", this.ParentId.ToString());
            writer.WriteIntList("ChildrenIds", this.ChildrenIds);
            writer.WriteElementString("Type", this.Type.ToString());
            writer.WriteIntList("ElementIds", this.ElementIds);
            writer.WriteStringList("Classifications", this.Classifications.ToList());
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader" /> stream from which the object is deserialized.</param>
        public virtual void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement();

            int id = reader.ReadElementContentAsInt();
            int displayOrder = reader.ReadElementContentAsInt();
            int? parentId = reader.ReadElementContentAsNullableInt();
            var childrenIds = reader.ReadElementContentAsIntList();
            string typeString = reader.ReadElementContentAsString();
            ZoneType type = (ZoneType)Enum.Parse(typeof(ZoneType), typeString);
            var elementIds = reader.ReadElementContentAsIntList();
            var classifications = reader.ReadElementContentAsStringArray();
         
            reader.ReadEndElement();

            this.Initialize(id, displayOrder, parentId, childrenIds, elementIds, classifications);
            this.SetType(type);
        }

        #endregion
    }
}
