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
using System.Linq;

namespace Imppoa.HtmlZoning.TreeStructure
{
    /// <summary>
    /// Generic tree class
    /// </summary>
    public abstract class Tree<T> : Tree where T : TreeNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Tree{T}"/> class
        /// </summary>
        public Tree()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tree{T}"/> class
        /// </summary>
        /// <param name="root">The root node</param>
        public Tree(T root)
            : base(root)
        {
        }

        /// <summary>
        /// Initializes the tree
        /// </summary>
        /// <param name="root">The root node</param>
        public void Initialize(T root)
        {
            base.Initialize(root);
        }

        /// <summary>
        /// Gets the root node
        /// </summary>
        /// <value>
        /// The root node
        /// </value>
        public new T Root
        {
            get
            {
                return (T)base.Root;
            }
        }

        /// <summary>
        /// Gets all nodes
        /// </summary>
        /// <value>
        /// All nodes
        /// </value>
        public new IReadOnlyList<T> All
        {
            get
            {
                return base.All.ConvertAll<T>();
            }
        }

        /// <summary>
        /// Gets the leaf nodes in display order
        /// </summary>
        /// <value>
        /// The leaf nodes in display order
        /// </value>
        public new IReadOnlyList<T> LeafNodes
        {
            get
            {
                return base.LeafNodes.ConvertAll<T>();
            }
        }

        /// <summary>
        /// Gets a node by id
        /// </summary>
        /// <param name="id">The id</param>
        /// <returns>the node with the id</returns>
        public new T GetById(int id)
        {
            return (T) base.GetById(id);
        }
    }

    /// <summary>
    /// Tree
    /// </summary>
    public abstract class Tree
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Tree"/> class
        /// </summary>
        public Tree()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tree"/> class
        /// </summary>
        /// <param name="root">The root node</param>
        public Tree(TreeNode root)
        {
            this.Initialize(root);
        }

        /// <summary>
        /// Initializes the tree
        /// </summary>
        /// <param name="root">The root node</param>
        public void Initialize(TreeNode root)
        {
            if (root == null)
            {
                throw new ArgumentNullException("root");
            }

            this.Root = root;
            this.SetTree();
        }

        /// <summary>
        /// Gets the root node
        /// </summary>
        /// <value>
        /// The root node
        /// </value>
        public TreeNode Root { get; private set; }

        /// <summary>
        /// Gets all nodes
        /// </summary>
        /// <value>
        /// All nodes
        /// </value>
        public IReadOnlyList<TreeNode> All
        {
            get
            {
                return this.Root.GetDescendantsAndSelf();
            }
        }

        /// <summary>
        /// Gets the leaf nodes in display order
        /// </summary>
        /// <value>
        /// The leaf nodes in display order
        /// </value>
        public IReadOnlyList<TreeNode> LeafNodes
        {
            get
            {
                return this.Root.GetLeafNodes();
            }
        }

        /// <summary>
        /// Gets the leaf node count
        /// </summary>
        /// <value>
        /// The leaf node count
        /// </value>
        public int LeafNodeCount
        {
            get
            {
                return this.LeafNodes.Count;
            }
        }

        /// <summary>
        /// Gets the number of nodes
        /// </summary>
        /// <value>
        /// The number of nodes
        /// </value>
        public int Count
        {
            get
            {
                return this.All.Count;
            }
        }

        /// <summary>
        /// Gets a node by id
        /// </summary>
        /// <param name="id">The id</param>
        /// <returns>the node with the id</returns>
        public TreeNode GetById(int id)
        {
            return this.All.Where(n => n.Id == id).SingleOrDefault();
        }

        /// <summary>
        /// Accepts a tree node visitor
        /// </summary>
        /// <param name="visitor">The visitor</param>
        public virtual void Accept(TreeNodeVisitor visitor)
        {
            this.Accept(visitor, new BreadthFirstWalker());
        }

        /// <summary>
        /// Accepts a tree node visitor
        /// </summary>
        /// <param name="visitor">The visitor</param>
        /// <param name="walker">The walker</param>
        public void Accept(TreeNodeVisitor visitor, TreeWalker walker)
        {
            walker.Initialize(this.Root);
            while (walker.MoveNext())
            {
                var node = (TreeNode)walker.Current;
                node.Accept(visitor);
                if (visitor.Stop)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Whether this tree equals the other tree
        /// </summary>
        /// <param name="otherTree">The other tree</param>
        /// <param name="outDifferences">(out) The differences</param>
        /// <returns>true, if this tree equals the other tree, false otherwise</returns>
        public virtual bool Equals(Tree otherTree, out IEnumerable<string> outDifferences)
        {
            bool areEqual = true;
            var differences = new List<string>();

            if (otherTree == null)
            {
                areEqual = false;
                differences.Add("Other tree is null");
            }
            else
            {
                if (this.Count != otherTree.Count)
                {
                    areEqual = false;
                    string message = string.Format("Different number of nodes: {0}|||{1}", this.Count, otherTree.Count);
                    differences.Add(message);
                }
                else
                {
                    foreach (var selfNode in this.All)
                    {
                        int id = selfNode.Id;
                        var otherNode = otherTree.GetById(id);

                        IEnumerable<string> nodeDifferences;
                        if (!selfNode.Equals(otherNode, out nodeDifferences))
                        {
                            areEqual = false;
                            differences.AddRange(nodeDifferences);
                        }
                    }
                }
            }

            outDifferences = differences;

            return areEqual;
        }

        /// <summary>
        /// Sets the tree on all nodes
        /// </summary>
        private void SetTree()
        {
            foreach (var node in this.All)
            {
                node.SetTree(this);
            }
        }
    }
}
