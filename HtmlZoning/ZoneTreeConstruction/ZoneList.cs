/*
Government Usage Rights Notice:  The U.S. Government retains unlimited, royalty-free usage rights to this software, but not ownership, as provided by Federal law.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

•	Redistributions of source code must retain the above Government Usage Rights Notice, this list of conditions and the following disclaimer.

•	Redistributions in binary form must reproduce the above Government Usage Rights Notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

•	Neither the names of the National Library of Medicine, the National Institutes of Health, nor the names of any of the software developers may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE U.S. GOVERNMENT AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITEDTO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE U.S. GOVERNMENT
OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using Imppoa.HtmlZoning.Dom.Serialization;
using System.Collections.Generic;

namespace Imppoa.HtmlZoning.ZoneTreeConstruction
{
    /// <summary>
    /// Helper class for dealing with list of zones
    /// </summary>
    public class ZoneList : List<Zone>
    {
        private int _prevId;
        private Zone _parent;

        private Zone CurrentZone
        {
            get
            {
                if (this.Count > 0)
                {
                    return this[this.Count - 1];
                }
                else
                {
                    return null;
                }
            }
        }

        private Zone PreviousZone
        {
            get
            {
                if (this.Count > 1)
                {
                    return this[this.Count - 2];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoneList" /> class
        /// </summary>
        /// <param name="prevId">The id of the previous zone</param>
        /// <param name="parent">The parent zone for all zones in the list</param>
        public ZoneList(int prevId, Zone parent)
        {
            _prevId = prevId;
            _parent = parent;
            this.StartNewUnknownZone();
        }

        /// <summary>
        /// (Optionally) Appends html element to new zone
        /// Starts a new zone (of unknown type)
        /// </summary>
        /// <param name="element">The DOM node</param>
        /// <param name="appendElement">Whether to append the element to the new zone</param>
        public void StartNewUnknownZone(SerializableElement element, bool appendElement)
        {
            this.StartNewUnknownZone();
            if (appendElement)
            {
                this.AppendToCurrentZone(element);
            }
        }

        /// <summary>
        /// Append element to an inline zone
        /// </summary>
        /// <param name="element">The html element</param>
        public void AppendToInlineZone(SerializableElement element)
        {
            if (!(this.CurrentZone.Type == ZoneType.Inline))
            {
                this.StartNewInlineZone();
            }
            this.AppendToCurrentZone(element);
        }

        /// <summary>
        /// Append element to a linebreak zone
        /// </summary>
        /// <param name="element">The html element</param>
        public void AppendToLinebreakZone(SerializableElement element)
        {
            this.StartNewLinebreakZone();
            this.AppendToCurrentZone(element);
        }

        /// <summary>
        /// Adds the html element to the current zone
        /// </summary>
        /// <param name="element">The html element</param>
        public void AppendToCurrentZone(SerializableElement element)
        {
            this.CurrentZone.AddElement(element);
        }

        /// <summary>
        /// Finalizes the list
        /// If current zone is of unknown type - adds it to the previous zone
        /// Removes empty zones
        /// </summary>
        public void Finalise()
        {
            if (this.CurrentZone.Type == ZoneType.Unknown && this.PreviousZone != null)
            {
                this.PreviousZone.AddElements(this.CurrentZone.Elements);
                this.Remove(this.CurrentZone);
            }
            this.RemoveEmptyZones();
        }

        /// <summary>
        /// Removes the empty zones
        /// </summary>
        private void RemoveEmptyZones()
        {
            this.RemoveAll(z => z.ElementCount == 0);
        }

        /// <summary>
        /// Appends a new zone to the list (if the current zone is not empty)
        /// </summary>
        /// <param name="type">The zone type</param>
        private void AppendZone(ZoneType type)
        {
            if (this.CurrentZone == null || (this.CurrentZone.ElementCount > 0 && !(this.CurrentZone.Type == ZoneType.Unknown)))
            {
                _prevId++;
                var zone = new Zone(_prevId, _parent);
                zone.SetType(type);
                this.Add(zone);
            }
            else
            {
                this.CurrentZone.SetType(type);
            }
        }

        /// <summary>
        /// Starts a new unknown zone
        /// </summary>
        private void StartNewUnknownZone()
        {
            this.AppendZone(ZoneType.Unknown);
        }

        /// <summary>
        /// Starts a new inline zone
        /// </summary>
        private void StartNewInlineZone()
        {
            this.AppendZone(ZoneType.Inline);
        }

        /// <summary>
        /// Starts a new linebreak zone
        /// </summary>
        private void StartNewLinebreakZone()
        {
            this.AppendZone(ZoneType.Linebreak);
        }
    }
}
