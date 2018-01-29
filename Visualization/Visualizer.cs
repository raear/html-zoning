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
using System.Windows.Forms;

namespace Imppoa.HtmlZoning.Visualization
{
    /// <summary>
    /// UI for zone visualization
    /// </summary>
    public partial class Visualizer : Form
    {
        private readonly AppServices _appServices;

        private string _url;
        private ZoneTree _zoneTree;
        private ColumnTree _columnTree;
        private VisualizerTreeNode _rootZoneNode;
        private VisualizerTreeNode _rootDomNode;
        private VisualizerTreeNode _rootColumnNode;

        /// <summary>
        /// Gets the root zone/dom node
        /// </summary>
        /// <value>
        /// The root node
        /// </value>
        private VisualizerTreeNode RootNode
        {
            get
            {
                if (_menu.DisplayDomTree)
                {
                    return _rootDomNode;
                }
                else if (_menu.DisplayColumnTree)
                {
                    return _rootColumnNode;
                }
                else
                {
                    return _rootZoneNode;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Visualizer"/> class
        /// </summary>
        public Visualizer()
        {
            InitializeComponent();
            _appServices = new AppServices();
            _menu.LoadPmidRequested += menu_LoadPmidRequested;
            _menu.SavePmidRequested += menu_SavePmidRequested;
            _menu.UpdateTreeViewRequested += menu_UpdateTreeViewRequested;
            _menu.LoadResultRequested += menu_LoadResultRequested;
            _menu.DoZoningRequested += menu_DoZoningRequested;
            _menu.LabelingRequested += menu_LabelingRequested;
            _webBrowser.UrlEntered += OnWebBrowserUrlEntered;
            _selectLabels.SelectedLabelsUpdated += onSelectedLabelsUpdated;
            _treeView.TreeDataUpdated += treeView_TreeDataUpdated;
            this.Reset();
        }

        #region menu

        /// <summary>
        /// On load pmid requested
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Unused event args</param>
        private void menu_LoadPmidRequested(object sender, EventArgs e)
        {
            this.LoadPmid();
            this.UpdateTitle();
        }

        /// <summary>
        /// On save pmid requested
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Unused event args</param>
        private void menu_SavePmidRequested(object sender, EventArgs e)
        {
            this.SavePmid();
            this.UpdateTitle();
        }

        /// <summary>
        /// On load result requested
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="type">The result type type</param>
        private void menu_LoadResultRequested(object sender, string type)
        {
            if (type == Visualization.Menu.ZONING_RESULT_TYPE)
            {
                var zoneTree = _appServices.DeserializeZoneTree(_menu.CurrentDirectory, _menu.Pmid);
                this.SetZoneTree(zoneTree);
            }
            this.DisplayTree();
            this.UpdateTitle();
        }

        /// <summary>
        /// On do zoning requested
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Unused event args</param>
        private void menu_DoZoningRequested(object sender, EventArgs e)
        {
            this.UseWaitCursor = true;
            Cursor.Current = Cursors.WaitCursor;

            this.DoZoning();
            this.DisplayTree();
            this.UpdateTitle();

            Cursor.Current = Cursors.Default;
            this.UseWaitCursor = false;
        }

        /// <summary>
        /// On labeling requested
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="type">The type of labeling requested</param>
        private void menu_LabelingRequested(object sender, string type)
        {
            if (type == Visualization.Menu.LAYOUT_ANALYSIS_ARTICLE_CONTENT_LABELING)
            {
                _appServices.LayoutAnalysisArticleContentLabeling(_columnTree);
            }
            else if (type == Visualization.Menu.ARTICLE_TAG_ARTICLE_CONTENT_LABELING)
            {
                _appServices.ArticleTagArticleContentLabeling(_zoneTree);
            }
            else if (type == Visualization.Menu.MAIN_TAG_ARTICLE_CONTENT_LABELING)
            {
                _appServices.MainTagArticleContentLabeling(_zoneTree);
            }
            this.DisplayTree();
        }

        /// <summary>
        /// On update tree view requested
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Unused event args</param>
        private void menu_UpdateTreeViewRequested(object sender, EventArgs e)
        {
            this.DisplayTree();
            this.UpdateTitle();
        }

        #endregion

        #region browser

        /// <summary>
        /// Called when an url is entered in the web browser
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data</param>
        private void OnWebBrowserUrlEntered(object sender, EventArgs e)
        {
            _menu.Reset();
            this.Reset();
            _menu.NotifyUrlLoaded();
            _url = _webBrowser.EnteredUrl;
        }

        #endregion  

        #region tree view

        /// <summary>
        /// Handles the AfterSelect event of the treeView control
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">The <see cref="TreeViewEventArgs"/> instance containing the event data</param>
        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var selectedNode = (VisualizerTreeNode)_treeView.SelectedNode.Tag;
            _nodeData.Display(selectedNode);
            _webBrowser.HighlightSelected(selectedNode.BoundingBox);
        }

        /// <summary>
        /// Handles the TreeDataUpdated event of the treeView control
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data</param>
        private void treeView_TreeDataUpdated(object sender, EventArgs e)
        {
            var allLabels = this.RootNode.GetAllLabels();
            _selectLabels.SetLabels(this.RootNode.GetAllLabels());
        }

        #endregion

        #region selected labels

        /// <summary>
        /// Handles selected labels updated event
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data</param>
        private void onSelectedLabelsUpdated(object sender, EventArgs e)
        {
            this.HighlightSelectedLabels();
        }

        #endregion

        /// <summary>
        /// Sets the zone tree
        /// </summary>
        /// <param name="zoneTree">The zone tree</param>
        private void SetZoneTree(ZoneTree zoneTree)
        {
            this.ClearTreeData();
            if (zoneTree != null)
            {
                _zoneTree = zoneTree;
                _columnTree = _appServices.CreateColumnTree(zoneTree);
                _rootDomNode = new ElementTreeNode(_zoneTree.Document.Body);
                _rootZoneNode = new ZoneTreeNode(_zoneTree.Root);
                _rootColumnNode = new ColumnTreeNode(_columnTree.Root);
            }
        }

        /// <summary>
        /// Clears the tree data
        /// </summary>
        private void ClearTreeData()
        {
            _zoneTree = null;
            _columnTree = null;
            _rootZoneNode = null;
            _rootDomNode = null;
            _rootColumnNode = null;
        }

        /// <summary>
        /// Loads the pmid
        /// </summary>
        private void LoadPmid()
        {
            this.Reset();
            _url = _appServices.ReadUrl(_menu.CurrentDirectory, _menu.Pmid);
            _webBrowser.Navigate(_url);
        }

        /// <summary>
        /// Saves the pmid
        /// </summary>
        private void SavePmid()
        {
            _appServices.SerializeZoneTree(_menu.CurrentDirectory, _menu.Pmid, _zoneTree, _url);
        }

        /// <summary>
        /// Does the zoning if a zone tree has not already been loaded
        /// </summary>
        private void DoZoning()
        {
            _webBrowser.ApplyDomModifications();
            var renderOutput = _webBrowser.GetRenderOutput();
            var zoneTree = _appServices.DoZoning(renderOutput);
            this.SetZoneTree(zoneTree);
        }

        /// <summary>
        /// Displays the zone/dom tree
        /// </summary>
        private void DisplayTree()
        {
            if (_menu.DisplayOnlyLeafZones)
            {
                _treeView.DisplayLeafNodes(this.RootNode);
            }
            else
            {
                _treeView.DisplayFullTree(this.RootNode);
            }
            var allLabels = this.RootNode.GetAllLabels();
            _selectLabels.SetLabels(allLabels);
            this.HighlightSelectedLabels();
            _menu.NotifyZoneTreeShown();
        }

        /// <summary>
        /// Reset
        /// </summary>
        private void Reset()
        {
            _url = string.Empty;
            this.ClearTreeData();
            _treeView.Nodes.Clear();
            _nodeData.Clear();
            this.UpdateTitle();
            _selectLabels.Clear();
        }

        /// <summary>
        /// Updates the title
        /// </summary>
        private void UpdateTitle()
        {
            this.Text = "Visualizer";
            if (_menu.ZoneTreeShown)
            {
                this.Text = this.Text + " - ";

                string nodeType;
                if (_menu.DisplayDomTree)
                {
                    nodeType = "Dom Tree";
                }
                else if (_menu.DisplayColumnTree)
                {
                    nodeType = "Column Tree";
                }
                else
                {
                    nodeType = "Zone Tree";
                }
                string treeType = _menu.DisplayOnlyLeafZones ? "Leaf Nodes" : "All Nodes";
                this.Text = this.Text + nodeType + "|" + treeType;
            }
        }

        /// <summary>
        /// Highlights the selected labels
        /// </summary>
        private void HighlightSelectedLabels()
        {
            var selectedTypes = _selectLabels.GetSelected();
            var bbSet = this.RootNode.GetBoundingBoxesForLabels(selectedTypes, _menu.DisplayOnlyLeafZones);
            _webBrowser.HighlightMany(bbSet);
        }
    }
}
