/*
Government Usage Rights Notice:  The U.S. Government retains unlimited, royalty-free usage rights to this software, but not ownership, as provided by Federal law.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

•	Redistributions of source code must retain the above Government Usage Rights Notice, this list of conditions and the following disclaimer.

•	Redistributions in binary form must reproduce the above Government Usage Rights Notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

•	Neither the names of the National Library of Medicine, the National Institutes of Health, nor the names of any of the software developers may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE U.S. GOVERNMENT AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITEDTO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE U.S. GOVERNMENT
OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using Imppoa.HtmlZoning.TreeStructure.Walkers;
using Imppoa.HtmlZoning.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Imppoa.HtmlZoning.TreeStructure
{
    /// <summary>
    /// Generic tree node
    /// </summary>
    public abstract class TreeNode<TTree,TNode> : TreeNode where TTree : Tree where TNode : TreeNode
    {
        /// <summary>
        /// Gets or sets the tree
        /// </summary>
        /// <value>
        /// The tree
        /// </value>
        public new TTree Tree
        {
            get
            {
                return (TTree)base.Tree;
            }
        }

        /// <summary>
        /// Gets the parent
        /// </summary>
        /// <value>
        /// The parent
        /// </value>
        public new TNode Parent
        {
            get
            {
                return (TNode)base.Parent;
            }
        }

        /// <summary>
        /// Gets the children
        /// </summary>
        /// <value>
        /// The children
        /// </value>
        public new IReadOnlyList<TNode> Children
        {
            get
            {
                return base.Children.ConvertAll<TNode>();
            }
        }

        /// <summary>
        /// Gets the left sibling
        /// </summary>
        /// <value>
        /// The left sibling
        /// </value>
        public new TNode LeftSibling
        {
            get
            {
                return (TNode)base.LeftSibling;
            }
        }

        /// <summary>
        /// Gets the right sibling
        /// </summary>
        /// <value>
        /// The right sibling
        /// </value>
        public new TNode RightSibling
        {
            get
            {
                return (TNode)base.RightSibling;
            }
        }

        /// <summary>
        /// Gets the descendants and self
        /// </summary>
        /// <param name="walker">The walker to use</param>
        /// <returns>the descendants and self</returns>
        public new IReadOnlyList<TNode> GetDescendantsAndSelf(TreeWalker walker = null)
        {
            return base.GetDescendantsAndSelf(walker).ConvertAll<TNode>();
        }

        /// <summary>
        /// Gets the descendants
        /// </summary>
        /// <param name="walker">The walker to use</param>
        /// <returns>the descendants</returns>
        public new IReadOnlyList<TNode> GetDescendants(TreeWalker walker = null)
        {
            return base.GetDescendants(walker).ConvertAll<TNode>();
        }

        /// <summary>
        /// Gets the leaf nodes
        /// </summary>
        /// <param name="walker">The walker to use</param>
        /// <returns>the leaf nodes</returns>
        public new IReadOnlyList<TNode> GetLeafNodes(TreeWalker walker = null)
        {
            return base.GetLeafNodes(walker).ConvertAll<TNode>();
        }
    }

    /// <summary>
    /// Tree node
    /// </summary>
    /// <seealso cref="Imppoa.HtmlZoning.TreeStructure.TreeNode" />
    public abstract class TreeNode
    {
        private List<int> _childrenIds;
        protected List<TreeNode> _children;
        private List<string> _classifications;
        private Dictionary<string, TreeNodeFeature> _features;
        private TreeWalker _defaultWalker;

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNode"/> class.
        /// </summary>
        public TreeNode()
        {
            _defaultWalker = new BreadthFirstWalker();
            _childrenIds = new List<int>();
            _children = new List<TreeNode>();
            _classifications = new List<string>();
            _features = new Dictionary<string, TreeNodeFeature>();
            this.BoundingBox = Rectangle.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNode" /> class
        /// </summary>
        /// <param name="id">The id</param>
        /// <param name="parentId">The parent id</param>
        /// <param name="childrenIds">The children ids</param>
        /// <param name="boundingBox">The bounding box</param>
        /// <param name="classifications">The classifications</param>
        public TreeNode(int id, int displayOrder, int? parentId, IReadOnlyList<int> childrenIds, Rectangle boundingBox, IEnumerable<string> classifications)
        {
            this.Initialize(id, displayOrder, parentId, childrenIds, boundingBox, classifications);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNode" /> class
        /// </summary>
        /// <param name="id">The id</param>
        /// <param name="displayOrder">The display order.</param>
        /// <param name="parent">The parent</param>
        /// <param name="children">The children</param>
        /// <param name="boundingBox">The bounding box</param>
        /// <param name="classifications">The classifications</param>
        public TreeNode(int id, int displayOrder, TreeNode parent, IReadOnlyList<TreeNode> children, Rectangle boundingBox, IEnumerable<string> classifications)
        {
            this.Initialize(id, displayOrder, parent, children, boundingBox, classifications);
        }

        /// <summary>
        /// Initializes the tree node
        /// </summary>
        /// <param name="id">The id</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="parentId">The parent id</param>
        /// <param name="childrenIds">The children ids</param>
        /// <param name="boundingBox">The bounding box</param>
        /// <param name="classifications">The classifications</param>
        /// <exception cref="System.ArgumentNullException">childrenIds</exception>
        public void Initialize(int id, int displayOrder, int? parentId, IReadOnlyList<int> childrenIds, Rectangle boundingBox, IEnumerable<string> classifications)
        {
            this.SetId(id);
            this.SetDisplayOrder(displayOrder);
            this.ParentId = parentId;
            if (childrenIds == null)
            {
                throw new ArgumentNullException("childrenIds");
            }
            this.SetChildrenIds(childrenIds);
            this.BoundingBox = boundingBox;
            this.AddClassifications(classifications);
        }

        /// <summary>
        /// Initializes the tree node
        /// </summary>
        /// <param name="id">The id</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="parent">The parent</param>
        /// <param name="children">The children</param>
        /// <param name="boundingBox">The bounding box</param>
        /// <param name="classifications">The classifications</param>
        public void Initialize(int id, int displayOrder, TreeNode parent, IReadOnlyList<TreeNode> children, Rectangle boundingBox, IEnumerable<string> classifications)
        {
            this.SetId(id);
            this.SetDisplayOrder(displayOrder);
            this.SetParent(parent);
            this.SetChildren(children);
            this.BoundingBox = boundingBox;
            this.AddClassifications(classifications);
        }

        /// <summary>
        /// Creates the tree links
        /// </summary>
        /// <param name="lookup">The tree node lookup</param>
        public virtual void Link(IDictionary<int, TreeNode> lookup)
        {
            if (this.ParentId.HasValue)
            {
                this.Parent = lookup[this.ParentId.Value];
            }

            foreach (int childId in _childrenIds)
            {
                _children.Add(lookup[childId]);
            }
        }

        #endregion

        #region tree query

        /// <summary>
        /// Gets or sets the tree
        /// </summary>
        /// <value>
        /// The tree
        /// </value>
        public Tree Tree { get; private set; }

        /// <summary>
        /// Sets the tree
        /// </summary>
        /// <param name="tree">The tree to set</param>
        public void SetTree(Tree tree)
        {
            this.Tree = tree;
        }

        /// <summary>
        /// Gets or sets the id
        /// </summary>
        /// <value>
        /// The id
        /// </value>
        public int Id { get; protected set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        /// <value>
        /// The display order
        /// </value>
        public int DisplayOrder { get; protected set; }

        /// <summary>
        /// Gets or sets the parent id
        /// </summary>
        /// <value>
        /// The parent id
        /// </value>
        public int? ParentId { get; protected set; }

        /// <summary>
        /// Gets or sets the children ids
        /// </summary>
        /// <value>
        /// The children ids
        /// </value>
        public IReadOnlyList<int> ChildrenIds
        {
            get
            {
                return _childrenIds;
            }
        }

        /// <summary>
        /// Whether node has a parent
        /// </summary>
        /// <value>
        /// <c>true</c> if node has a parent; otherwise, <c>false</c>.
        /// </value>
        public bool HasParent
        {
            get
            {
                return this.Parent != null;
            }
        }

        /// <summary>
        /// Gets the parent
        /// </summary>
        /// <value>
        /// The parent
        /// </value>
        public TreeNode Parent { get; protected set; }

        /// <summary>
        /// Gets the number of children
        /// </summary>
        /// <value>
        /// The number of children
        /// </value>
        public int ChildrenCount
        {
            get
            {
                return _children.Count;
            }
        }

        /// <summary>
        /// Gets the children
        /// </summary>
        /// <value>
        /// The children
        /// </value>
        public IReadOnlyList<TreeNode> Children
        {
            get
            {
                return _children;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has a left sibling
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has a left sibling; otherwise, <c>false</c>
        /// </value>
        public bool HasLeftSibling
        {
            get
            {
                return this.LeftSibling != null;
            }
        }

        /// <summary>
        /// Gets the left sibling
        /// </summary>
        /// <value>
        /// The left sibling
        /// </value>
        public TreeNode LeftSibling
        {
            get
            {
                TreeNode leftSibling = null;
                if (this.HasParent)
                {
                    int indexOfSelf = this.Parent._children.IndexOf(this);
                    if (indexOfSelf != 0)
                    {
                        int leftSiblingIndex = indexOfSelf - 1;
                        leftSibling = this.Parent._children[leftSiblingIndex];
                    }
                }

                return leftSibling;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has a right sibling
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has a right sibling; otherwise, <c>false</c>
        /// </value>
        public bool HasRightSibling
        {
            get
            {
                return this.RightSibling != null;
            }
        }

        /// <summary>
        /// Gets the right sibling
        /// </summary>
        /// <value>
        /// The right sibling
        /// </value>
        public TreeNode RightSibling
        {
            get
            {
                TreeNode rightSibling = null;
                if (this.HasParent)
                {
                    var parentChildren = this.Parent._children;
                    int indexOfSelf = parentChildren.IndexOf(this);
                    int lastIndex = parentChildren.Count - 1;
                    if (indexOfSelf != lastIndex)
                    {
                        int rightSiblingIndex = indexOfSelf + 1;
                        rightSibling = parentChildren[rightSiblingIndex];
                    }
                }

                return rightSibling;
            }
        }

        /// <summary>
        /// Whether this is a leaf node
        /// </summary>
        public bool IsLeaf()
        {
            return this.ChildrenCount == 0;
        }

        /// <summary>
        /// Gets the descendants and self
        /// </summary>
        /// <param name="walker">The walker to use</param>
        /// <returns>
        /// the descendants and self
        /// </returns>
        public IReadOnlyList<TreeNode> GetDescendantsAndSelf(TreeWalker walker = null)
        {
            if (walker == null)
            {
                walker = _defaultWalker;
            }
            walker.Initialize(this);

            var toReturn = new List<TreeNode>();
            while(walker.MoveNext())
            {
                toReturn.Add((TreeNode)walker.Current);
            }

            return toReturn;
        }

        /// <summary>
        /// Gets the descendants
        /// </summary>
        /// <param name="walker">The walker to use</param>
        /// <returns>
        /// the descendants
        /// </returns>
        public IReadOnlyList<TreeNode> GetDescendants(TreeWalker walker = null)
        {
            return this.GetDescendantsAndSelf(walker).Except(new[] { this }).ToList();
        }

        /// <summary>
        /// Gets the leaf nodes
        /// </summary>
        /// <param name="walker">The walker to use</param>
        /// <returns>the leaf nodes</returns>
        public IReadOnlyList<TreeNode> GetLeafNodes(TreeWalker walker = null)
        {
            return this.GetDescendantsAndSelf(walker).Where(n => n.IsLeaf()).ToList();
        }

        #endregion

        #region tree modification

        /// <summary>
        /// Sets the id
        /// </summary>
        /// <param name="id">The id</param>
        private void SetId(int id)
        {
            this.Id = id;
        }

        /// <summary>
        /// Sets the children ids
        /// </summary>
        /// <param name="childrenIds">The children ids</param>
        private void SetChildrenIds(IReadOnlyList<int> childrenIds)
        {
            _childrenIds = new List<int>(childrenIds);
        }

        /// <summary>
        /// Sets the display order
        /// </summary>
        /// <param name="displayOrder">The display order</param>
        public void SetDisplayOrder(int displayOrder)
        {
            this.DisplayOrder = displayOrder;
        }

        /// <summary>
        /// Sets the parent
        /// </summary>
        /// <param name="parent">The parent</param>
        protected void SetParent(TreeNode parent)
        {
            this.Parent = parent;
            this.UpdateParentId();
        }

        /// <summary>
        /// Sets the children
        /// </summary>
        /// <param name="children">The children</param>
        protected void SetChildren(IReadOnlyList<TreeNode> children)
        {
            if (children == null)
            {
                throw new ArgumentNullException("children");
            }

            _children = new List<TreeNode>(children);
            this.UpdateChildrenIds();
        }

        /// <summary>
        /// Adds a child to the end of the list
        /// </summary>
        /// <param name="children">The child</param>
        protected void AddChild(TreeNode child)
        {
            if (child == null)
            {
                throw new ArgumentNullException("child");
            }

            _children.Add(child);
            _childrenIds.Add(child.Id);
        }

        /// <summary>
        /// Adds children to the end of the list
        /// </summary>
        /// <param name="children">The children</param>
        protected void AddChildren(IReadOnlyList<TreeNode> children)
        {
            if (children == null)
            {
                throw new ArgumentNullException("children");
            }

            _children.AddRange(children);
            _childrenIds.AddRange(children.Select(c => c.Id));
        }

        /// <summary>
        /// Inserts a child at the specified index
        /// </summary>
        /// <param name="index">The index</param>
        /// <param name="child">The child</param>
        protected void InsertChildAt(int index, TreeNode child)
        {
            if (child == null)
            {
                throw new ArgumentNullException("child");
            }

            _children.Insert(index, child);
            _childrenIds.Insert(index, child.Id);
        }

        /// <summary>
        /// Insert the children at the specified index
        /// </summary>
        /// <param name="index">The index</param>
        /// <param name="children">The children</param>
        protected void InsertChildrenAt(int index, IReadOnlyList<TreeNode> children)
        {
            if (children == null)
            {
                throw new ArgumentNullException("children");
            }

            _children.InsertRange(index, children);
            _childrenIds.InsertRange(index, children.Select(c => c.Id));
        }

        /// <summary>
        /// Removes a child
        /// </summary>
        /// <param name="child">The child</param>
        protected void RemoveChild(TreeNode child)
        {
            if (child == null)
            {
                throw new ArgumentNullException("child");
            }

            _children.Remove(child);
            _childrenIds.Remove(child.Id);
        }

        /// <summary>
        /// Removes all children
        /// </summary>
        protected void RemoveAllChildren()
        {
            _children.Clear();
            _childrenIds.Clear();
        }

        /// <summary>
        /// Updates the parent id
        /// </summary>
        private void UpdateParentId()
        {
            if (this.Parent != null)
            {
                this.ParentId = this.Parent.Id;
            }
            else
            {
                this.ParentId = null;
            }
        }

        /// <summary>
        /// Updates the children ids
        /// </summary>
        private void UpdateChildrenIds()
        {
            _childrenIds = new List<int>();
            foreach (var child in _children)
            {
                _childrenIds.Add(child.Id);
            }
        }

        #endregion

        #region bounding box

        /// <summary>
        /// Gets the bounding box
        /// </summary>
        /// <value>
        /// The bounding box
        /// </value>
        public virtual Rectangle BoundingBox { get; protected set; }

        /// <summary>
        /// Determines whether element has area when rendered
        /// Must compute bounding box first
        /// </summary>
        /// <returns>
        /// whether element has area
        /// </returns>
        public bool HasArea()
        {
            return this.BoundingBox.Left < this.BoundingBox.Right && this.BoundingBox.Top < this.BoundingBox.Bottom;
        }

        /// <summary>
        /// Computes bounding boxes from tree nodes
        /// </summary>
        /// <param name="nodes">The tree nodes</param>
        /// <returns>bounding box</returns>
        protected Rectangle ComputeBoundingBox(IEnumerable<TreeNode> nodes)
        {
            Rectangle boundingBox = Rectangle.Empty;
            foreach (var node in nodes)
            {
                var nodeBoundingBox = node.BoundingBox;
                if (nodeBoundingBox.Width > 0 || nodeBoundingBox.Height > 0)
                {
                    if (boundingBox.IsEmpty)
                    {
                        boundingBox = nodeBoundingBox;
                    }
                    else
                    {
                        boundingBox = Rectangle.Union(boundingBox, nodeBoundingBox);
                    }
                }
            }

            return boundingBox;
        }

        #endregion

        #region classifications

        /// <summary>
        /// Gets the classifications
        /// </summary>
        /// <value>
        /// The classifications
        /// </value>
        public IReadOnlyCollection<string> Classifications
        {
            get
            {
                return _classifications;
            }
        }

        /// <summary>
        /// Gets the number of classifications
        /// </summary>
        /// <value>
        /// The number of classifications
        /// </value>
        public int ClassificationsCount
        {
            get
            {
                return _classifications.Count;
            }
        }

        /// <summary>
        /// Adds a classification
        /// </summary>
        /// <param name="classification">The classification</param>
        public virtual void AddClassification(string classification)
        {
            classification = classification.Trim();
            if (!_classifications.Contains(classification))
            {
                _classifications.Add(classification);
            }
        }

        /// <summary>
        /// Adds classifications
        /// </summary>
        /// <param name="classifications">The classifications</param>
        public void AddClassifications(IEnumerable<string> classifications)
        {
            if (classifications == null)
            {
                throw new ArgumentNullException("classifications");
            }

            foreach(var classification in classifications)
            {
                this.AddClassification(classification);
            }
        }

        /// <summary>
        /// Removes a classification
        /// </summary>
        /// <param name="classification">The classification</param>
        public void RemoveClassification(string classification)
        {
            _classifications.Remove(classification);
        }

        /// <summary>
        /// Removes all classifications
        /// </summary>
        public void RemoveAllClassifications()
        {
            _classifications.Clear();
        }

        /// <summary>
        /// Whether the node has the classification
        /// </summary>
        /// <param name="classification">The classification</param>
        /// <returns>true, if the node has the classification, false otherwise</returns>
        public bool HasClassification(string classification)
        {
            return _classifications.Contains(classification);
        }

        #endregion

        #region features

        /// <summary>
        /// Gets the feature names
        /// </summary>
        /// <value>
        /// The feature names
        /// </value>
        public IReadOnlyCollection<string> FeatureNames
        {
            get
            {
                return _features.Keys.OrderBy(v=>v).ToList();
            }
        }

        /// <summary>
        /// Gets the feature count
        /// </summary>
        /// <value>
        /// The feature count
        /// </value>
        public int FeatureCount
        {
            get
            {
                return _features.Count;
            }
        }

        /// <summary>
        /// Whether node has a feature with the specified name
        /// </summary>
        /// <param name="name">The name</param>
        /// <returns>true, if has feature, false otherwise</returns>
        public bool HasFeature(string name)
        {
            return _features.ContainsKey(name);
        }

        /// <summary>
        /// Adds a feature
        /// </summary>
        /// <param name="feature">The feature to add</param>
        public void AddFeature(TreeNodeFeature feature)
        {
            if (feature == null)
            {
                throw new ArgumentNullException("feature");
            }

            string featureName = feature.Name;
            if (this.HasFeature(featureName))
            {
                _features[featureName] = feature;
            }
            else
            {
                _features.Add(featureName, feature);
            }
        }

        /// <summary>
        /// Gets a feature
        /// </summary>
        /// <param name="name">The feature name</param>
        /// <returns>The feature</returns>
        public TreeNodeFeature GetFeature(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Invalid feature name", "name");
            }

            TreeNodeFeature feature = null;
            if (this.HasFeature(name))
            {
                feature = _features[name];
            }
            return feature;
        }

        /// <summary>
        /// Removes a feature
        /// </summary>
        /// <param name="name">The feature name</param>
        public void RemoveFeature(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Invalid feature name", "name");
            }

            if (this.HasFeature(name))
            {
                _features.Remove(name);
            }
        }

        /// <summary>
        /// Removes all features
        /// </summary>
        public void RemoveAllFeatures()
        {
            _features.Clear();
        }

        #endregion

        #region visitor

        /// <summary>
        /// Accepts the a tree node visitor
        /// </summary>
        /// <param name="visitor">The visitor</param>
        public void Accept(TreeNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        #endregion

        #region Print

        /// <summary>
        /// Pretty prints the node and its descendants
        /// </summary>
        public string PrettyPrint()
        {
            var sb = new StringBuilder();
            this.PrettyPrint(sb, string.Empty, true);
            string print = sb.ToString();
            print = print.TrimEnd(Environment.NewLine.ToCharArray());
            return print;
        }

        /// <summary>
        /// Pretty prints the node and its descendants
        /// </summary>
        /// <param name="sb">The string builder</param>
        /// <param name="indent">The indent to apply</param>
        /// <param name="last">Whether this is the last child</param>
        private void PrettyPrint(StringBuilder sb, string indent, bool last)
        {
            sb.Append(indent);
            if (last)
            {
                sb.Append(@"\-");
                indent += "    ";
            }
            else
            {
                sb.Append("|-");
                indent += "|   ";
            }

            string nodeText = this.GetDisplayString();
            sb.AppendLine(nodeText);

            for (int i = 0; i < this.ChildrenCount; i++)
            {
                bool isLastChild = i == this.ChildrenCount - 1;
                var child = _children[i];
                child.PrettyPrint(sb, indent, isLastChild);
            }
        }

        /// <summary>
        /// Gets the node display string
        /// </summary>
        /// <returns>the node display string</returns>
        public virtual string GetDisplayString()
        {
            return this.Id + "[" + string.Join("|", this.Classifications) + "]";
        }

        #endregion

        #region Equals

        /// <summary>
        /// Whether this tree node equals the other tree node
        /// </summary>
        /// <param name="other">The other tree node</param>
        /// <param name="outDifferences">(out) The differences</param>
        /// <returns>Whether this tree node equals the other tree node</returns>
        public virtual bool Equals(TreeNode other, out IEnumerable<string> outDifferences)
        {
            bool areEqual = true;
            var differences = new List<string>();

            if (other == null)
            {
                areEqual = false;
                differences.Add("Other tree node is null");
            }
            else
            {
                if (this.Id != other.Id)
                {
                    areEqual = false;
                    string message = string.Format("Different id: {0}|||{1}", this.Id, other.Id);
                    differences.Add(message);
                }

                if (this.DisplayOrder != other.DisplayOrder)
                {
                    areEqual = false;
                    string message = string.Format("Different display order: {0}|||{1}", this.DisplayOrder, other.DisplayOrder);
                    differences.Add(message);
                }

                if (this.ParentId != other.ParentId)
                {
                    areEqual = false;
                    string message = string.Format("Id, {0}, different parent ids: {1}|||{2}", this.Id, this.ParentId, other.ParentId);
                    differences.Add(message);
                }

                if (this.ChildrenCount != other.ChildrenCount)
                {
                    areEqual = false;
                    string message = string.Format("Id, {0}, different number of children: {1}|||{2}", this.Id, this.ChildrenCount, other.ChildrenCount);
                    differences.Add(message);
                }
                else
                {
                    if (!this.ChildrenIds.SequenceEqual(other.ChildrenIds))
                    {
                        areEqual = false;
                        string message = string.Format("Id, {0}, different children id sequence: {1}|||{2}", this.Id, string.Join(",", _childrenIds), string.Join(",", other._childrenIds));
                        differences.Add(message);
                    }
                }

                if (this.BoundingBox != other.BoundingBox)
                {
                    areEqual = false;
                    string message = string.Format("Id, {0}, different bounding box: {1}|||{2}", this.Id, this.BoundingBox, other.BoundingBox);
                    differences.Add(message);
                }

                if (this.ClassificationsCount != other.ClassificationsCount)
                {
                    areEqual = false;
                    string message = string.Format("Id, {0}, different number of classifications: {1}|||{2}", this.Id, this.ClassificationsCount, other.ClassificationsCount);
                    differences.Add(message);
                }

                foreach (string type in this.Classifications)
                {
                    if (!other.HasClassification(type))
                    {
                        areEqual = false;
                        string message = string.Format("Id, {0}, other node is not, {1}, type", this.Id, type);
                        differences.Add(message);
                    }
                }

                if (this.FeatureCount != other.FeatureCount)
                {
                    areEqual = false;
                    string message = string.Format("Id, {0}, different number of features: {1}|||{2}", this.Id, this.FeatureCount, other.FeatureCount);
                    differences.Add(message);
                }

                foreach (string featureName in this.FeatureNames)
                {
                    if (other.HasFeature(featureName))
                    {
                        var selfFeature = this.GetFeature(featureName);
                        var otherFeature = other.GetFeature(featureName);

                        bool featuresAreEqual;
                        IEnumerable<string> featureDifferences;
                        featuresAreEqual = selfFeature.Equals(otherFeature, out featureDifferences);
                        if (!featuresAreEqual)
                        {
                            areEqual = false;
                            differences.AddRange(featureDifferences);
                        }
                    }
                    else
                    {
                        areEqual = false;
                        string message = string.Format("Id, {0}, other node does not have, {1}, feature", this.Id, featureName);
                        differences.Add(message);
                    }
                }
            }

            outDifferences = differences;

            return areEqual;
        }

        #endregion
    }
}
