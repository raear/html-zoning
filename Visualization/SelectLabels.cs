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
using System.Linq;
using System.Windows.Forms;

namespace Imppoa.HtmlZoning.Visualization
{
    /// <summary>
    /// Control used to select zones with particular labels
    /// </summary>
    public partial class SelectLabels : UserControl
    {
        private static readonly int CHK_BOX_WIDTH = 175;
        private static readonly int CHK_BOX_HEIGHT = 20;

        public event EventHandler SelectedLabelsUpdated;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectLabels"/> class
        /// </summary>
        public SelectLabels()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets the labels that are available for selection
        /// </summary>
        /// <param name="labels">The labels</param>
        public void SetLabels(IEnumerable<string> labels)
        {
            var existing = new List<string>();
            var toRemove = new List<Control>();
            foreach (object control in _flowLayoutPanel.Controls)
            {
                var chkBox = (CheckBox)control;
                var label = chkBox.Text;
                if (labels.Contains(label))
                {
                    existing.Add(label);
                }
                else
                {
                    toRemove.Add(chkBox);
                }
            }

            foreach(var control in toRemove)
            {
                _flowLayoutPanel.Controls.Remove(control);
            }

            var toAdd = labels.Except(existing);
            foreach (var label in toAdd)
            {
                var chkBox = new CheckBox();
                chkBox.Text = label;
                chkBox.Width = CHK_BOX_WIDTH;
                chkBox.Height = CHK_BOX_HEIGHT;
                chkBox.CheckedChanged += OnChkBoxCheckedChanged;
                _flowLayoutPanel.Controls.Add(chkBox);
            }
        }

        /// <summary>
        /// Clears the control
        /// </summary>
        public void Clear()
        {
            _flowLayoutPanel.Controls.Clear();
        }

        /// <summary>
        /// Gets the selected labels
        /// </summary>
        public IEnumerable<string> GetSelected()
        {
            var selected = new List<string>();
            foreach(var control in _flowLayoutPanel.Controls)
            {
                var chkBox = (CheckBox)control;
                if (chkBox.Checked)
                {
                    selected.Add(chkBox.Text);
                }
            }
            return selected;
        }

        /// <summary>
        /// Called when a check box is checked or unchecked
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data</param>
        private void OnChkBoxCheckedChanged(object sender, EventArgs e)
        {
            this.SelectedLabelsUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}
