namespace Imppoa.HtmlZoning.Visualization
{
    partial class NodeData
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
            this._splitContainer = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this._htmlTextBox = new System.Windows.Forms.TextBox();
            this._plainTextBox = new System.Windows.Forms.TextBox();
            this._featuresTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).BeginInit();
            this._splitContainer.Panel1.SuspendLayout();
            this._splitContainer.Panel2.SuspendLayout();
            this._splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // _splitContainer
            // 
            this._splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitContainer.Location = new System.Drawing.Point(0, 0);
            this._splitContainer.Name = "_splitContainer";
            this._splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // _splitContainer.Panel1
            // 
            this._splitContainer.Panel1.Controls.Add(this.splitContainer2);
            // 
            // _splitContainer.Panel2
            // 
            this._splitContainer.Panel2.Controls.Add(this._featuresTextBox);
            this._splitContainer.Size = new System.Drawing.Size(1035, 245);
            this._splitContainer.SplitterDistance = 164;
            this._splitContainer.SplitterWidth = 1;
            this._splitContainer.TabIndex = 0;
            this._splitContainer.TabStop = false;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this._htmlTextBox);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this._plainTextBox);
            this.splitContainer2.Size = new System.Drawing.Size(1035, 164);
            this.splitContainer2.SplitterDistance = 82;
            this.splitContainer2.SplitterWidth = 1;
            this.splitContainer2.TabIndex = 0;
            this.splitContainer2.TabStop = false;
            // 
            // _htmlTextBox
            // 
            this._htmlTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._htmlTextBox.Location = new System.Drawing.Point(0, 0);
            this._htmlTextBox.Multiline = true;
            this._htmlTextBox.Name = "_htmlTextBox";
            this._htmlTextBox.ReadOnly = true;
            this._htmlTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._htmlTextBox.Size = new System.Drawing.Size(1035, 82);
            this._htmlTextBox.TabIndex = 0;
            this._htmlTextBox.TabStop = false;
            // 
            // _plainTextBox
            // 
            this._plainTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._plainTextBox.Location = new System.Drawing.Point(0, 0);
            this._plainTextBox.Multiline = true;
            this._plainTextBox.Name = "_plainTextBox";
            this._plainTextBox.ReadOnly = true;
            this._plainTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._plainTextBox.Size = new System.Drawing.Size(1035, 81);
            this._plainTextBox.TabIndex = 0;
            this._plainTextBox.TabStop = false;
            // 
            // _featuresTextBox
            // 
            this._featuresTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this._featuresTextBox.Location = new System.Drawing.Point(0, 0);
            this._featuresTextBox.Multiline = true;
            this._featuresTextBox.Name = "_featuresTextBox";
            this._featuresTextBox.ReadOnly = true;
            this._featuresTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._featuresTextBox.Size = new System.Drawing.Size(1035, 80);
            this._featuresTextBox.TabIndex = 0;
            this._featuresTextBox.TabStop = false;
            // 
            // NodeData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._splitContainer);
            this.Name = "NodeData";
            this.Size = new System.Drawing.Size(1035, 245);
            this._splitContainer.Panel1.ResumeLayout(false);
            this._splitContainer.Panel2.ResumeLayout(false);
            this._splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).EndInit();
            this._splitContainer.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer _splitContainer;
        private System.Windows.Forms.TextBox _featuresTextBox;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TextBox _htmlTextBox;
        private System.Windows.Forms.TextBox _plainTextBox;
    }
}
