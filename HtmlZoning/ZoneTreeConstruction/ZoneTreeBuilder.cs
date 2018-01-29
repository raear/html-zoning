/*
Government Usage Rights Notice:  The U.S. Government retains unlimited, royalty-free usage rights to this software, but not ownership, as provided by Federal law.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

•	Redistributions of source code must retain the above Government Usage Rights Notice, this list of conditions and the following disclaimer.

•	Redistributions in binary form must reproduce the above Government Usage Rights Notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

•	Neither the names of the National Library of Medicine, the National Institutes of Health, nor the names of any of the software developers may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE U.S. GOVERNMENT AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITEDTO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE U.S. GOVERNMENT
OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using Imppoa.HtmlZoning.Dom;
using Imppoa.HtmlZoning.Dom.Serialization;
using Imppoa.HtmlZoning.TreeStructure;
using Imppoa.HtmlZoning.TreeStructure.Filters;
using Imppoa.HtmlZoning.TreeStructure.Walkers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Imppoa.HtmlZoning.ZoneTreeConstruction
{
    /// <summary>
    /// Builds a zone tree
    /// </summary>
    public class ZoneTreeBuilder
    {
        /// <summary>
        /// Creates an instance of the zone tree builder
        /// </summary>
        /// <param name="significantBlockType">Significant block type</param>
        /// <param name="significantInlineType">Significant inline type</param>
        /// <param name="significantLinebreakType">Significant linebreak type</param>
        /// <param name="significantInvisibleType">Significant invisible type</param>
        /// <param name="breakDownType">Break down type</param>
        /// <param name="anameType">A name type</param>
        /// <param name="hiddenType">Hidden type</param>
        /// <returns>
        /// the created instance
        /// </returns>
        public static ZoneTreeBuilder Create(string significantBlockType, string significantInlineType, string significantLinebreakType, string significantInvisibleType, string breakDownType, string anameType, string hiddenType)
        {
            var startNewZoneFilter = new TypeFilter(significantLinebreakType);
            var breakDownFilter = new TypeFilter(breakDownType);
            var significantBlockFilter = new TypeFilter(significantBlockType);
            var significantInlineFilter = new TypeFilter(significantInlineType);
            var significantInvisibleFilter = new TypeFilter(significantInvisibleType);
            var postProcessingSteps = new TreeNodeVisitor[] { };
            var builder = new ZoneTreeBuilder(startNewZoneFilter, breakDownFilter, significantBlockFilter, significantInlineFilter, significantInvisibleFilter, postProcessingSteps);  
            return builder;
        }
        private TreeNodeFilter StartNewZoneFilter {get; set;}
        private TreeNodeFilter BreakDownFilter { get; set; }
        private TreeNodeFilter SignificantBlockFilter { get; set; }
        private TreeNodeFilter SignificantInlineFilter { get; set; }
        private TreeNodeFilter SignificantInvisibleFilter { get; set; }

        private IEnumerable<TreeNodeVisitor> PostProcessingSteps { get; set; }

        private int _currentId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoneTreeBuilder" /> class
        /// </summary>
        /// <param name="startNewZoneFilter">Start new zone filter</param>
        /// <param name="breakDownFilter">Break down filter</param>
        /// <param name="significantBlockFilter">Significant block filter</param>
        /// <param name="significantInlineFilter">Significant inline filter</param>
        /// <param name="significantInvisibleFilter">The significant invisible filter</param>
        /// <param name="postProcessingSteps">The post processing steps</param>
        public ZoneTreeBuilder(TreeNodeFilter startNewZoneFilter, TreeNodeFilter breakDownFilter, TreeNodeFilter significantBlockFilter, TreeNodeFilter significantInlineFilter, TreeNodeFilter significantInvisibleFilter, IEnumerable<TreeNodeVisitor> postProcessingSteps)
        {
            this.StartNewZoneFilter = startNewZoneFilter;
            this.BreakDownFilter = breakDownFilter;
            this.SignificantInlineFilter = significantInlineFilter;
            this.SignificantBlockFilter = significantBlockFilter;
            this.SignificantInvisibleFilter = significantInvisibleFilter;
            this.PostProcessingSteps = postProcessingSteps;
        }

        /// <summary>
        /// Builds the zone tree
        /// </summary>
        /// <param name="classifiedDoc">The classified html document</param>
        /// <returns>
        /// a zone tree
        /// </returns>
        public ZoneTree Build(SerializableDocument classifiedDoc)
        {
            try
            {
                _currentId = 0;
                var bodyZone = new Zone(_currentId, null);
                var bodyElement = (SerializableElement)classifiedDoc.Body;
                bodyZone.AddElement(bodyElement);
                this.Build(bodyZone);

                this.CropZoneText(bodyZone, classifiedDoc.Text);
                this.SetDisplayOrder(bodyZone);
                var tree = new ZoneTree(bodyZone, classifiedDoc);

                this.RunPostProcessingSteps(tree);

                return tree;
            }
            catch (Exception ex)
            {
                throw new Exception("Error building zone tree", ex);
            }
        }

        /// <summary>
        /// Recursively builds the zone tree by breaking down parent zones into children
        /// </summary>
        /// <param name="parentZone">The parent zone</param>
        private void Build(Zone parentZone)
        {
            HtmlElement toBreakDown;
            if (this.ShouldBreakDown(parentZone, out toBreakDown))
            {
                var childrenZones = this.BreakDownZone(parentZone, toBreakDown);
                parentZone.AddChildren(childrenZones);

                foreach (var child in parentZone.Children)
                {
                    this.Build(child);
                }
            }
        }

        /// <summary>
        /// Creates a list of children zones by merging inline elements
        /// </summary>
        /// <param name="parentZone">The parent zone</param>
        /// <param name="toBreakDown">Previously identified element to break down</param>
        /// <returns>
        /// Children zones
        /// </returns>
        private Zone[] BreakDownZone(Zone parentZone, HtmlElement toBreakDown)
        {
            var zoneList = new ZoneList(_currentId, parentZone);
            foreach (HtmlElement element in toBreakDown.Children)
            {
                var sElement = (SerializableElement)element;

                if (this.ShouldStartNewZone(element))
                {
                    zoneList.StartNewUnknownZone(sElement, false);
                }
                else if (this.ShouldBreakDownElement(element))
                {
                    zoneList.AppendToLinebreakZone(sElement);
                }
                else if (this.IsSignificantBlockElement(element))
                {
                    zoneList.AppendToLinebreakZone(sElement);
                }
                else if (this.IsSignificantInlineElement(element))
                {
                    zoneList.AppendToInlineZone(sElement);
                }
                else if (this.IsSignificantInvisibleElement(element))
                {
                    zoneList.AppendToCurrentZone(sElement);
                }
            }

            zoneList.Finalise();
            _currentId = _currentId + zoneList.Count;
            return zoneList.ToArray();
        }

        /// <summary>
        /// Whether the zone should be broken down
        /// </summary>
        /// <param name="parentZone">The zone</param>
        /// <param name="toBreakDown">If true, the element to break down, otherwise null</param>
        /// <returns>true, if zone should be broken down, false otherwise</returns>
        private bool ShouldBreakDown(Zone parentZone, out HtmlElement toBreakDown)
        {
            toBreakDown = parentZone.Elements.Where(n => this.ShouldBreakDownElement(n)).FirstOrDefault();
            return toBreakDown != null;
        }

        /// <summary>
        /// Whether a new zone should be started
        /// </summary>
        /// <param name="element">The element</param>
        /// <returns>true, if a new zone should be started, false otherwise</returns>
        private bool ShouldStartNewZone(HtmlElement element)
        {
            return this.StartNewZoneFilter.AcceptNode(element);
        }

        /// <summary>
        /// Whether the element should be broken down
        /// </summary>
        /// <param name="element">The html element</param>
        /// <returns>true, if the element should be broken down, false otherwise</returns>
        private bool ShouldBreakDownElement(HtmlElement element)
        {
            return this.BreakDownFilter.AcceptNode(element);
        }

        /// <summary>
        /// Determines whether the element is a significant block element
        /// </summary>
        /// <param name="element">The element</param>
        /// <returns>true, if is significant block element, false otherwise</returns>
        private bool IsSignificantBlockElement(HtmlElement element)
        {
            return this.SignificantBlockFilter.AcceptNode(element);
        }

        /// <summary>
        /// Determines whether the element is a significant inline element
        /// </summary>
        /// <param name="element">The element</param>
        /// <returns>true, if is significant inline element, false otherwise</returns>
        private bool IsSignificantInlineElement(HtmlElement element)
        {
            return this.SignificantInlineFilter.AcceptNode(element);
        }

        /// <summary>
        /// Determines whether the element is a significant invisible element
        /// </summary>
        /// <param name="element">The element</param>
        /// <returns>true, if is significant invisible element, false otherwise</returns>
        private bool IsSignificantInvisibleElement(HtmlElement element)
        {
            return this.SignificantInvisibleFilter.AcceptNode(element);
        }

        /// <summary>
        /// Crops the zone text from the document text
        /// </summary>
        /// <param name="bodyZone">The body zone</param>
        /// <param name="documentText">The document text</param>
        private void CropZoneText(Zone bodyZone, string documentText)
        {
            foreach (var zone in bodyZone.GetDescendantsAndSelf())
            {
                zone.CropText(documentText);
            }
        }

        /// <summary>
        /// Sets the display order
        /// </summary>
        /// <param name="bodyZone">The body zone</param>
        private void SetDisplayOrder(Zone bodyZone)
        {
            int order = 0;
            foreach (var zone in bodyZone.GetDescendantsAndSelf(new DepthFirstWalker()))
            {
                zone.SetDisplayOrder(order);
                order++;
            }
        }

        /// <summary>
        /// Runs the post processing steps
        /// </summary>
        /// <param name="tree">The zone tree</param>
        private void RunPostProcessingSteps(ZoneTree tree)
        {
            foreach (var step in this.PostProcessingSteps)
            {
                tree.Accept(step);
            }
        }
    }
}
