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
    /// Custom menu
    /// </summary>
    public partial class Menu : UserControl
    {
        // Constants
        public static readonly string ZONING_RESULT_TYPE = "zoning";
        public static readonly string LAYOUT_ANALYSIS_ARTICLE_CONTENT_LABELING = "LAYOUT_ANALYSIS_ARTICLE_CONTENT_LABELING";
        public static readonly string ARTICLE_TAG_ARTICLE_CONTENT_LABELING = "ARTICLE_TAG_ARTICLE_CONTENT_LABELING";
        public static readonly string MAIN_TAG_ARTICLE_CONTENT_LABELING = "MAIN_TAG_ARTICLE_CONTENT_LABELING";
        private static readonly string FILE_FILTER = "Url files|*.url";

        private readonly AppServices _services;
        private bool _urlLoaded;
        private bool _zoneTreeFilesExist;

        public event EventHandler LoadPmidRequested;
        public event EventHandler SavePmidRequested;
        public event EventHandler<string> LoadResultRequested;
        public event EventHandler DoZoningRequested;
        public event EventHandler<string> LabelingRequested;
        public event EventHandler UpdateTreeViewRequested;

        public bool ZoneTreeShown { get; private set; }
        public bool DisplayOnlyLeafZones { get; private set; }
        public bool DisplayDomTree { get; private set; }
        public bool DisplayZoneTree { get; private set; }
        public bool DisplayColumnTree { get; private set; }
        public string CurrentPath { get; private set; }
        public string CurrentDirectory { get; private set; }
        public string Pmid { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Menu"/> class
        /// </summary>
        public Menu()
        {
            InitializeComponent();
            _services = new AppServices();
            this.Reset();
        }

        /// <summary>
        /// Resets the menu state
        /// </summary>
        public void Reset()
        {
            _urlLoaded = false;
            _zoneTreeFilesExist = false;

            this.CurrentPath = null;
            this.CurrentDirectory = null;
            this.Pmid = null;

            this.ResetViewMenuState();
            this.DisplayZoneTree = true; 
            this.DisplayOnlyLeafZones = true;
            this.ZoneTreeShown = false;
           
            this.ConfigureMenuState();
        }

        /// <summary>
        /// Resets the view menu state
        /// </summary>
        private void ResetViewMenuState()
        {
            this.DisplayZoneTree = false;
            this.DisplayColumnTree = false;
            this.DisplayDomTree = false;
        }

        /// <summary>
        /// Notifies the menu that an url has been loaded
        /// </summary>
        public void NotifyUrlLoaded()
        {
            _urlLoaded = true;
            this.ConfigureMenuState();
        }

        /// <summary>
        /// Notifies the menu that a zone tree has been shown
        /// </summary>
        public void NotifyZoneTreeShown()
        {
            this.ZoneTreeShown = true;
            this.ConfigureMenuState();
        }

        #region File

        /// <summary>
        /// Handles the Click event of the loadMenuItem control
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data</param>
        private void loadMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = this.CreateOpenFileDialog();
            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.LoadUrlFromFile(dlg.FileName);
                this.FireEvent(this.LoadPmidRequested);
            }
        }

        /// <summary>
        /// Handles the Click event of the saveMenuItem control
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data</param>
        private void saveAsMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = this.CreateSaveFileDialog();
            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.SetUrlFilePath(dlg.FileName);
                this.FireEvent(this.SavePmidRequested);
                this.SetUrlFilePath(dlg.FileName);
                this.ConfigureMenuState();
            }
        }

        #endregion

        #region Actions

        /// <summary>
        /// Handles the Click event of the loadZoningMenuItem control
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data</param>
        private void loadZoningMenuItem_Click(object sender, EventArgs e)
        {
            this.FireEvent(this.LoadResultRequested, ZONING_RESULT_TYPE);
        }

        /// <summary>
        /// Handles the Click event of the doZoningMenuItem control
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data</param>
        private void doZoningMenuItem_Click(object sender, EventArgs e)
        {
            this.FireEvent(this.DoZoningRequested);
        }

        /// <summary>
        /// Handles the Click event of the layoutAnalaysisArticleContentLabelingMenuItem_Click
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data</param>
        private void layoutAnalaysisArticleContentLabelingMenuItem_Click(object sender, EventArgs e)
        {
            this.FireEvent(this.LabelingRequested, LAYOUT_ANALYSIS_ARTICLE_CONTENT_LABELING);
        }

        /// <summary>
        /// Handles the Click event of the articleTagArticleContentLabelingMenuItem_Click
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data</param>
        private void articleTagArticleContentLabelingMenuItem_Click(object sender, EventArgs e)
        {
            this.FireEvent(this.LabelingRequested, ARTICLE_TAG_ARTICLE_CONTENT_LABELING);
        }

        /// <summary>
        /// Handles the Click event of the mainTagArticleContentLabelingMenuItem_Click
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data</param>
        private void mainTagArticleContentLabelingMenuItem_Click(object sender, EventArgs e)
        {
            this.FireEvent(this.LabelingRequested, MAIN_TAG_ARTICLE_CONTENT_LABELING);
        }

        #endregion

        #region View

        /// <summary>
        /// Handles the Click event of the displayOnlyLeafNodesMenuItem control
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data</param>
        private void displayOnlyLeafNodesMenuItem_Click(object sender, EventArgs e)
        {
            this.DisplayOnlyLeafZones = true;
            this.ConfigureMenuState();
            this.FireEvent(this.UpdateTreeViewRequested);
        }

        /// <summary>
        /// Handles the Click event of the displayAllNodesMenuItem control
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data</param>
        private void displayAllNodesMenuItem_Click(object sender, EventArgs e)
        {
            this.DisplayOnlyLeafZones = false;
            this.ConfigureMenuState();
            this.FireEvent(this.UpdateTreeViewRequested);
        }

        /// <summary>
        /// Handles the Click event of the displayDomTreeMenuItem control
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data</param>
        private void displayDomTreeMenuItem_Click(object sender, EventArgs e)
        {
            this.ResetViewMenuState();
            this.DisplayDomTree = true;
            this.ConfigureMenuState();
            this.FireEvent(this.UpdateTreeViewRequested);   
        }

        /// <summary>
        /// Handles the Click event of the displayZoneTreeMenuItem control
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data</param>
        private void displayZoneTreeMenuItem_Click(object sender, EventArgs e)
        {
            this.ResetViewMenuState();
            this.DisplayZoneTree = true;
            this.ConfigureMenuState();
            this.FireEvent(this.UpdateTreeViewRequested);
        }

        /// <summary>
        /// Handles the Click event of the displayColumnTreeMenuItem control
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data</param>
        private void displayColumnTreeMenuItem_Click(object sender, EventArgs e)
        {
            this.ResetViewMenuState();
            this.DisplayColumnTree = true;
            this.ConfigureMenuState();
            this.FireEvent(this.UpdateTreeViewRequested);
        }

        #endregion

        #region Helper functions

        /// <summary>
        /// Creates an open file dialog
        /// </summary>
        private OpenFileDialog CreateOpenFileDialog()
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = FILE_FILTER;
            return dlg;
        }

        /// <summary>
        /// Creates the save file dialog
        /// </summary>
        private SaveFileDialog CreateSaveFileDialog()
        {
            var dlg = new SaveFileDialog();
            dlg.Filter = FILE_FILTER;
            dlg.FileName = string.Format(AppServices.URL_FILENAME_TEMPLATE, this.Pmid);
            return dlg;
        }

        /// <summary>
        /// Fires an event
        /// </summary>
        /// <param name="eventHandler">The event handler</param>
        private void FireEvent(EventHandler eventHandler)
        {
            this.UseWaitCursor = true;
            Cursor.Current = Cursors.WaitCursor;
            eventHandler?.Invoke(this, EventArgs.Empty);
            Cursor.Current = Cursors.Default;
            this.UseWaitCursor = false;
        }

        /// <summary>
        /// Fires an event
        /// </summary>
        /// <param name="eventHandler">The event handler</param>
        /// <param name="data">The data</param>
        private void FireEvent<T>(EventHandler<T> eventHandler, T data)
        {
            this.UseWaitCursor = true;
            Cursor.Current = Cursors.WaitCursor;
            eventHandler?.Invoke(this, data);
            Cursor.Current = Cursors.Default;
            this.UseWaitCursor = false;
        }

        /// <summary>
        /// Configures the menu visibility
        /// </summary>
        private void ConfigureMenuState()
        {
            _saveAsMenuItem.Enabled = this.ZoneTreeShown;
            _loadZoningMenuItem.Enabled = _zoneTreeFilesExist;
            _doZoningMenuItem.Enabled = _urlLoaded;
            _layoutAnalysisArticleContentLabelingMenuItem.Enabled = this.ZoneTreeShown;
            _articleTagArticleContentLabelingMenuItem.Enabled = this.ZoneTreeShown;
            _mainTagArticleContentLabelingMenuItem.Enabled = this.ZoneTreeShown;

            _displayAllNodesMenuItem.Enabled = this.ZoneTreeShown;
            _displayOnlyLeafNodesMenuItem.Enabled = this.ZoneTreeShown;
            _displayDomTreeMenuItem.Enabled = this.ZoneTreeShown;
            _displayZoneTreeMenuItem.Enabled = this.ZoneTreeShown;
            _displayColumnTreeMenuItem.Enabled = this.ZoneTreeShown;

            _displayZoneTreeMenuItem.Visible = !this.DisplayZoneTree;
            _displayDomTreeMenuItem.Visible = !this.DisplayDomTree;
            _displayColumnTreeMenuItem.Visible = !this.DisplayColumnTree;

            _displayOnlyLeafNodesMenuItem.Visible = !this.DisplayOnlyLeafZones;
            _displayAllNodesMenuItem.Visible = this.DisplayOnlyLeafZones;
        }

        /// <summary>
        /// Sets the url file path
        /// </summary>
        /// <param name="filePath">The url file path</param>
        private void SetUrlFilePath(string filePath)
        {
            this.CurrentPath = filePath;
            this.CurrentDirectory = _services.GetRootDirectory(filePath);
            string pmid = _services.GetPmidFromFilePath(filePath);
            this.Pmid = pmid;
            _zoneTreeFilesExist = _services.ZoneTreeFilesExists(this.CurrentDirectory, this.Pmid);
        }

      
        /// <summary>
        /// Loads an URL from file
        /// </summary>
        /// <param name="filePath">The url file path</param>
        private void LoadUrlFromFile(string filePath)
        {
            this.Reset();
            _urlLoaded = true;
            this.SetUrlFilePath(filePath);
            this.ConfigureMenuState();
        }

        #endregion
    }
}
