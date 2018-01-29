using System.Windows.Forms;
namespace Imppoa.HtmlZoning.Visualization
{
    partial class Browser
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
            this._navigationUrlTextBox = new System.Windows.Forms.TextBox();
            this._browserControl = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // _navigationUrlTextBox
            // 
            this._navigationUrlTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this._navigationUrlTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._navigationUrlTextBox.Location = new System.Drawing.Point(0, 0);
            this._navigationUrlTextBox.Name = "_navigationUrlTextBox";
            this._navigationUrlTextBox.Size = new System.Drawing.Size(1033, 22);
            this._navigationUrlTextBox.TabIndex = 0;
            this._navigationUrlTextBox.TabStop = false;
            this._navigationUrlTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnNavigationUrlTextBoxKeyPress);
            // 
            // _browserControl
            // 
            this._browserControl.Location = new System.Drawing.Point(0, 23);
            this._browserControl.Margin = new System.Windows.Forms.Padding(0);
            this._browserControl.MaximumSize = new System.Drawing.Size(1200, 729);
            this._browserControl.MinimumSize = new System.Drawing.Size(1200, 729);
            this._browserControl.Name = "_browserControl";
            this._browserControl.ScriptErrorsSuppressed = true;
            this._browserControl.Size = new System.Drawing.Size(1200, 729);
            this._browserControl.TabIndex = 0;
            this._browserControl.TabStop = false;
            // 
            // Browser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._browserControl);
            this.Controls.Add(this._navigationUrlTextBox);
            this.Name = "Browser";
            this.Size = new System.Drawing.Size(1033, 775);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _navigationUrlTextBox;
        private System.Windows.Forms.WebBrowser _browserControl;
    }
}
