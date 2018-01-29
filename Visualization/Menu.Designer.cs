namespace Imppoa.HtmlZoning.Visualization
{
    partial class Menu
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._menuStrip = new System.Windows.Forms.MenuStrip();
            this._fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._loadMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._saveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._actionsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._loadZoningMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._doZoningMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this._layoutAnalysisArticleContentLabelingMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._articleTagArticleContentLabelingMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._viewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._displayOnlyLeafNodesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._displayAllNodesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._displayDomTreeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._displayZoneTreeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._displayColumnTreeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._mainTagArticleContentLabelingMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this._menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _menuStrip
            // 
            this._menuStrip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._menuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this._menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._fileMenuItem,
            this._actionsMenuItem,
            this._viewMenuItem});
            this._menuStrip.Location = new System.Drawing.Point(0, 0);
            this._menuStrip.Name = "_menuStrip";
            this._menuStrip.Padding = new System.Windows.Forms.Padding(0);
            this._menuStrip.Size = new System.Drawing.Size(191, 24);
            this._menuStrip.TabIndex = 1;
            // 
            // _fileMenuItem
            // 
            this._fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._loadMenuItem,
            this._saveAsMenuItem});
            this._fileMenuItem.Name = "_fileMenuItem";
            this._fileMenuItem.Size = new System.Drawing.Size(37, 24);
            this._fileMenuItem.Text = "File";
            // 
            // _loadMenuItem
            // 
            this._loadMenuItem.Name = "_loadMenuItem";
            this._loadMenuItem.Size = new System.Drawing.Size(123, 22);
            this._loadMenuItem.Text = "Load...";
            this._loadMenuItem.Click += new System.EventHandler(this.loadMenuItem_Click);
            // 
            // _saveAsMenuItem
            // 
            this._saveAsMenuItem.Enabled = false;
            this._saveAsMenuItem.Name = "_saveAsMenuItem";
            this._saveAsMenuItem.Size = new System.Drawing.Size(123, 22);
            this._saveAsMenuItem.Text = "Save As...";
            this._saveAsMenuItem.Click += new System.EventHandler(this.saveAsMenuItem_Click);
            // 
            // _actionsMenuItem
            // 
            this._actionsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._loadZoningMenuItem,
            this.toolStripSeparator1,
            this._doZoningMenuItem,
            this.toolStripSeparator2,
            this._layoutAnalysisArticleContentLabelingMenuItem,
            this._articleTagArticleContentLabelingMenuItem,
            this._mainTagArticleContentLabelingMenuItem});
            this._actionsMenuItem.Name = "_actionsMenuItem";
            this._actionsMenuItem.Size = new System.Drawing.Size(59, 24);
            this._actionsMenuItem.Text = "Actions";
            // 
            // _loadZoningMenuItem
            // 
            this._loadZoningMenuItem.Enabled = false;
            this._loadZoningMenuItem.Name = "_loadZoningMenuItem";
            this._loadZoningMenuItem.Size = new System.Drawing.Size(329, 22);
            this._loadZoningMenuItem.Text = "Load Zoning";
            this._loadZoningMenuItem.Click += new System.EventHandler(this.loadZoningMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(326, 6);
            // 
            // _doZoningMenuItem
            // 
            this._doZoningMenuItem.Enabled = false;
            this._doZoningMenuItem.Name = "_doZoningMenuItem";
            this._doZoningMenuItem.Size = new System.Drawing.Size(329, 22);
            this._doZoningMenuItem.Text = "Do Zoning";
            this._doZoningMenuItem.Click += new System.EventHandler(this.doZoningMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(326, 6);
            // 
            // _layoutAnalysisArticleContentLabelingMenuItem
            // 
            this._layoutAnalysisArticleContentLabelingMenuItem.Enabled = false;
            this._layoutAnalysisArticleContentLabelingMenuItem.Name = "_layoutAnalysisArticleContentLabelingMenuItem";
            this._layoutAnalysisArticleContentLabelingMenuItem.Size = new System.Drawing.Size(329, 22);
            this._layoutAnalysisArticleContentLabelingMenuItem.Text = "Label Article Content (Layout Analysis Alg.)";
            this._layoutAnalysisArticleContentLabelingMenuItem.Click += new System.EventHandler(this.layoutAnalaysisArticleContentLabelingMenuItem_Click);
            // 
            // _articleTagArticleContentLabelingMenuItem
            // 
            this._articleTagArticleContentLabelingMenuItem.Enabled = false;
            this._articleTagArticleContentLabelingMenuItem.Name = "_articleTagArticleContentLabelingMenuItem";
            this._articleTagArticleContentLabelingMenuItem.Size = new System.Drawing.Size(343, 22);
            this._articleTagArticleContentLabelingMenuItem.Text = "Label Article Content (<article> Semantic Tag Alg.)";
            this._articleTagArticleContentLabelingMenuItem.Click += new System.EventHandler(this.articleTagArticleContentLabelingMenuItem_Click);
            // 
            // _viewMenuItem
            // 
            this._viewMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._displayOnlyLeafNodesMenuItem,
            this._displayAllNodesMenuItem,
            this._displayDomTreeMenuItem,
            this._displayZoneTreeMenuItem,
            this._displayColumnTreeMenuItem});
            this._viewMenuItem.Name = "_viewMenuItem";
            this._viewMenuItem.Size = new System.Drawing.Size(44, 24);
            this._viewMenuItem.Text = "View";
            // 
            // _displayOnlyLeafNodesMenuItem
            // 
            this._displayOnlyLeafNodesMenuItem.Enabled = false;
            this._displayOnlyLeafNodesMenuItem.Name = "_displayOnlyLeafNodesMenuItem";
            this._displayOnlyLeafNodesMenuItem.Size = new System.Drawing.Size(202, 22);
            this._displayOnlyLeafNodesMenuItem.Text = "Display Only Leaf Nodes";
            this._displayOnlyLeafNodesMenuItem.Visible = false;
            this._displayOnlyLeafNodesMenuItem.Click += new System.EventHandler(this.displayOnlyLeafNodesMenuItem_Click);
            // 
            // _displayAllNodesMenuItem
            // 
            this._displayAllNodesMenuItem.Enabled = false;
            this._displayAllNodesMenuItem.Name = "_displayAllNodesMenuItem";
            this._displayAllNodesMenuItem.Size = new System.Drawing.Size(202, 22);
            this._displayAllNodesMenuItem.Text = "Display All Nodes";
            this._displayAllNodesMenuItem.Click += new System.EventHandler(this.displayAllNodesMenuItem_Click);
            // 
            // _displayDomTreeMenuItem
            // 
            this._displayDomTreeMenuItem.Enabled = false;
            this._displayDomTreeMenuItem.Name = "_displayDomTreeMenuItem";
            this._displayDomTreeMenuItem.Size = new System.Drawing.Size(202, 22);
            this._displayDomTreeMenuItem.Text = "Show Dom Tree";
            this._displayDomTreeMenuItem.Click += new System.EventHandler(this.displayDomTreeMenuItem_Click);
            // 
            // _displayZoneTreeMenuItem
            // 
            this._displayZoneTreeMenuItem.Enabled = false;
            this._displayZoneTreeMenuItem.Name = "_displayZoneTreeMenuItem";
            this._displayZoneTreeMenuItem.Size = new System.Drawing.Size(202, 22);
            this._displayZoneTreeMenuItem.Text = "Show Zone Tree";
            this._displayZoneTreeMenuItem.Visible = false;
            this._displayZoneTreeMenuItem.Click += new System.EventHandler(this.displayZoneTreeMenuItem_Click);
            // 
            // _displayColumnTreeMenuItem
            // 
            this._displayColumnTreeMenuItem.Enabled = false;
            this._displayColumnTreeMenuItem.Name = "_displayColumnTreeMenuItem";
            this._displayColumnTreeMenuItem.Size = new System.Drawing.Size(202, 22);
            this._displayColumnTreeMenuItem.Text = "Show Column Tree";
            this._displayColumnTreeMenuItem.Click += new System.EventHandler(this.displayColumnTreeMenuItem_Click);
            // 
            // _mainTagArticleContentLabelingMenuItem
            // 
            this._mainTagArticleContentLabelingMenuItem.Enabled = false;
            this._mainTagArticleContentLabelingMenuItem.Name = "_mainTagArticleContentLabelingMenuItem";
            this._mainTagArticleContentLabelingMenuItem.Size = new System.Drawing.Size(343, 22);
            this._mainTagArticleContentLabelingMenuItem.Text = "Label Article Content (<main> Semantic Tag Alg.)";
            this._mainTagArticleContentLabelingMenuItem.Click += new System.EventHandler(this.mainTagArticleContentLabelingMenuItem_Click);
            // 
            // Menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._menuStrip);
            this.Name = "Menu";
            this.Size = new System.Drawing.Size(409, 22);
            this._menuStrip.ResumeLayout(false);
            this._menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip _menuStrip;
        private System.Windows.Forms.ToolStripMenuItem _fileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _loadMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _saveAsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _actionsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _loadZoningMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _viewMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _displayOnlyLeafNodesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _displayAllNodesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _displayDomTreeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _displayZoneTreeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _doZoningMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _displayColumnTreeMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem _layoutAnalysisArticleContentLabelingMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _articleTagArticleContentLabelingMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _mainTagArticleContentLabelingMenuItem;
    }
}
