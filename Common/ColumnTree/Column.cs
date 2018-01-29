/*
Government Usage Rights Notice:  The U.S. Government retains unlimited, royalty-free usage rights to this software, but not ownership, as provided by Federal law.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

•	Redistributions of source code must retain the above Government Usage Rights Notice, this list of conditions and the following disclaimer.

•	Redistributions in binary form must reproduce the above Government Usage Rights Notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

•	Neither the names of the National Library of Medicine, the National Institutes of Health, nor the names of any of the software developers may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE U.S. GOVERNMENT AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITEDTO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE U.S. GOVERNMENT
OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System.Collections.Generic;
using System;
using System.Linq;
using Imppoa.HtmlZoning.TreeStructure;
using System.Drawing;

namespace Imppoa.HtmlZoning
{
    /// <summary>
    /// Html column
    /// Zones merged based on left and right coordinates
    /// </summary>
    public class Column : TreeNode<ColumnTree, Column>
    {
        private List<Zone> _zones;

        #region dynamic properties

        /// <summary>
        /// Gets the zone bounding boxes
        /// </summary>
        /// <value>
        /// The zone bounding boxes
        /// </value>
        private IReadOnlyList<Rectangle> ZoneBoundingBoxes
        {
            get
            {
                return _zones.Select(z => z.BoundingBox).ToList();
            }
        }

        /// <summary>
        /// Gets the average zone left coordinate
        /// </summary>
        /// <value>
        /// The average zone left coordinate
        /// </value>
        private double AvgLeftCoord
        {
            get
            {
                return this.ComputeAverage(this.ZoneBoundingBoxes.Select(b => b.Left));
            }
        }

        /// <summary>
        /// Gets the top coordinate
        /// </summary>
        /// <value>
        /// The top coordinate
        /// </value>
        private int TopCoord
        {
            get
            {
                return this.ZoneBoundingBoxes.Min(b => b.Top);
            }
        }

        /// <summary>
        /// Gets the average zone right coordinate
        /// </summary>
        /// <value>
        /// The average zone right coordinate
        /// </value>
        private double AvgRightCoord
        {
            get
            {
                return this.ComputeAverage(this.ZoneBoundingBoxes.Select(b => b.Right));
            }
        }

        /// <summary>
        /// Gets the bottom coordinate
        /// </summary>
        /// <value>
        /// The bottom coordinate
        /// </value>
        private int BottomCoord
        {
            get
            {
                return this.ZoneBoundingBoxes.Max(b => b.Bottom);
            }
        }

        /// <summary>
        /// Gets the average zone width
        /// </summary>
        /// <value>
        /// The average zone width
        /// </value>
        private double AvgWidth
        {
            get
            {
                return this.ComputeAverage(this.ZoneBoundingBoxes.Select(b => b.Width));
            }
        }

        /// <summary>
        /// Gets column zones
        /// </summary>
        /// <value>
        /// The zones
        /// </value>
        public IReadOnlyList<Zone> Zones
        {
            get
            {
                return _zones;
            }
        }

        /// <summary>
        /// Gets the zone count
        /// </summary>
        /// <value>
        /// The zone count
        /// </value>
        public int ZoneCount
        {
            get
            {
                return this.Zones.Count;
            }
        }

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
                int left = Convert.ToInt32(this.AvgLeftCoord);
                int right = Convert.ToInt32(this.AvgRightCoord);
                return Rectangle.FromLTRB(left, this.TopCoord, right, this.BottomCoord);
            }

            protected set
            {
                // Cannot set
            }
        }

        /// <summary>
        /// Gets the HTML
        /// </summary>
        /// <value>
        /// The HTML
        /// </value>
        public string Html
        {
            get
            {
                return _zones.Select(z => z.Html).Aggregate((current, next) => current + next);
            }
        }

        #endregion

        #region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Column" /> class
        /// </summary>
        /// <param name="id">The id</param>
        /// <param name="parent">The parent</param>
        /// <param name="initialZone">The initial zone</param>
        public Column(int id, Column parent, Zone initialZone)
            : base()
        {
            _zones = new List<Zone>();
            this.Id = id;
            this.SetParent(parent);
            this.Append(initialZone);
            this.SetDisplayOrder(-1);
        }

        #endregion

        #region overrides

        /// <summary>
        /// Gets the display string
        /// </summary>
        public override string GetDisplayString()
        {
            return string.Format("Column({0},{1})[{2}]",
                                Math.Round(this.AvgLeftCoord),
                                Math.Round(this.AvgRightCoord),
                                string.Join("|", this.Classifications));
        }

        /// <summary>
        /// Adds a classification
        /// </summary>
        /// <param name="type">The classification</param>
        public override void AddClassification(string type)
        {
            base.AddClassification(type);
            foreach (var zone in this.Zones)
            {
                zone.AddClassification(type);
            }
        }

        #endregion

        #region modification

        /// <summary>
        /// Appends a zone to the column if left and right coordinates are within tolerance
        /// </summary>
        /// <param name="zone">The zone</param>
        /// <param name="relTol">The relative tolerance</param>
        /// <returns>
        /// true, if the zone was appended, false otherwise
        /// </returns>
        public bool TryAppend(Zone zone, double relTol)
        {
            bool success = this.WithinTolerance(zone, relTol);
            if (success)
            {
                this.Append(zone);
            }
            return success;
        }

        /// <summary>
        /// Whether the zone left and right coordinates are within tolerance
        /// </summary>
        /// <param name="zone">The zone</param>
        /// <param name="relTol">The relative tolerance</param>
        /// <returns>
        /// true, if coordinates are within tolerance, false otherwise
        /// </returns>
        public bool WithinTolerance(Zone zone, double relTol)
        {
            double tol = this.AvgWidth * relTol;
            double deltaLeft = Math.Abs(this.AvgLeftCoord - zone.BoundingBox.Left);
            double deltaRight = Math.Abs(this.AvgRightCoord - zone.BoundingBox.Right);
            return deltaLeft <= tol && deltaRight <= tol;
        }

        /// <summary>
        /// Appends a zone
        /// </summary>
        /// <param name="zone">The zone to append</param>
        private void Append(Zone zone)
        {
            _zones.Add(zone);
        }

        /// <summary>
        /// Appends column, if the left and right coordinates are within tolerance
        /// </summary>
        /// <param name="other">The column to append</param>
        /// <param name="relTol">The relative tolerance</param>
        /// <returns>
        /// true, if the column was appended, false otherwise
        /// </returns>
        public bool TryAppend(Column other, double relTol)
        {
            bool success = this.WithinTolerance(other, relTol);
            if (success)
            {
                this.Append(other);
            }
            return success;
        }

        /// <summary>
        /// Whether the column left and right coordinates are within tolerance
        /// </summary>
        /// <param name="column">The column</param>
        /// <param name="relTol">The relative tolerance</param>
        /// <returns>
        /// true, if coordinates are within tolerance, false otherwise
        /// </returns>
        public bool WithinTolerance(Column column, double relTol)
        {
            double tol = this.AvgWidth * relTol;
            double deltaLeft = Math.Abs(this.AvgLeftCoord - column.AvgLeftCoord);
            double deltaRight = Math.Abs(this.AvgRightCoord - column.AvgRightCoord);
            return deltaLeft <= tol && deltaRight <= tol;
        }

        /// <summary>
        /// Appends column to this column
        /// </summary>
        /// <param name="column">The column</param>
        private void Append(Column column)
        {
            _zones.AddRange(column.Zones);
            this.AddChildren(column.Children);
        }

        /// <summary>
        /// Adds a child
        /// </summary>
        /// <param name="child">The child to add</param>
        public void AddChild(Column child)
        {
            base.AddChild(child);
        }

        /// <summary>
        /// Removes the child
        /// </summary>
        /// <param name="child">The child</param>
        public void RemoveChild(Column child)
        {
            base.RemoveChild(child);
        }

        /// <summary>
        /// Sets the parent
        /// </summary>
        /// <param name="parent">The parent</param>
        public void SetParent(Column parent)
        {
            base.SetParent(parent);
        }

        /// <summary>
        /// Inserts children at
        /// </summary>
        /// <param name="index">The index to insert at</param>
        /// <param name="children">The children</param>
        public void InsertChildrenAt(int index, IReadOnlyList<Column> children)
        {
            base.InsertChildrenAt(index, children);
        }

        #endregion

        #region helper methods

        /// <summary>
        /// Computes the average of a collection of integers
        /// </summary>
        /// <param name="dataSet">The data set</param>
        /// <returns>the average</returns>
        private double ComputeAverage(IEnumerable<int> dataSet)
        {
            return dataSet.Average();
        }

        #endregion
    }
}
