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
using Imppoa.HtmlZoning.TreeStructure.Visitors;
using Imppoa.HtmlZoning.TreeStructure.Walkers;
using Imppoa.HtmlZoning.TreeStructure;
using Imppoa.HtmlZoning.TreeStructure.Filters;
using Imppoa.HtmlZoning.Dom.Filters;

namespace Imppoa.HtmlZoning.ElementClassification
{
    /// <summary>
    /// Classifies html elements in preparation for the construction of the zone tree
    /// </summary>
    public class PreZoningClassification : Algorithm<HtmlDocument>
    {
        private readonly string _significantBlockType;
        private readonly string _significantInlineType;
        private readonly string _significantLinebreakType;
        private readonly string _significantInvisibleType;
        private readonly string _breakDownType;
        private readonly string _anameType;
        private readonly string _hiddenType;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreZoningClassification" /> class
        /// </summary>
        /// <param name="significantBlockType">Significant block type</param>
        /// <param name="significantInlineType">Significant inline type</param>
        /// <param name="significantLinebreakType">Significant linebreak type</param>
        /// <param name="significantInvisibleType">Significant invisible type</param>
        /// <param name="breakDownType">Break down type</param>
        /// <param name="anameType">A name type</param>
        /// <param name="hiddenType">Hidden type</param>
        public PreZoningClassification(string significantBlockType, string significantInlineType, string significantLinebreakType, string significantInvisibleType, string breakDownType, string anameType, string hiddenType)
        {
            _significantBlockType = significantBlockType;
            _significantInlineType = significantInlineType;
            _significantLinebreakType = significantLinebreakType;
            _significantInvisibleType = significantInvisibleType;
            _breakDownType = breakDownType;
            _anameType = anameType;
            _hiddenType = hiddenType;
        }

        /// <summary>
        /// Executes the algorithm on the tree strcuture
        /// </summary>
        /// <param name="tree">The tree structure</param>
        protected override void ExecuteImplementation(HtmlDocument doc)
        {
            doc.Accept(new TreeNodeClassifier(HtmlElementType.DisplayBlock, new DisplayBlockFilter()));
            doc.Accept(new TreeNodeClassifier(HtmlElementType.DisplayInline, new DisplayInlineFilter()));
            doc.Accept(new TreeNodeClassifier(HtmlElementType.DisplayNone, new DisplayNoneFilter()));
            doc.Accept(new TreeNodeClassifier(HtmlElementType.Linebreak, new TagNameFilter(Html.Tags.BR)));
            doc.Accept(new TreeNodeClassifier(HtmlElementType.Linebreak, new TagNameFilter(Html.Tags.HR)));

            doc.Accept(new TreeNodeClassifier(HtmlElementType.HasText, new HasTextFilter()));
            doc.Accept(new TreeNodeClassifier(HtmlElementType.HasArea, new HasAreaFilter()));
            doc.Accept(new TreeNodeClassifier(_hiddenType, new VisibilityHiddenFilter()));
            doc.Accept(new TreeNodeClassifier(_hiddenType, new DisplayHiddenFilter(doc.DefaultStyleLookup)));
            doc.Accept(new TreeNodeClassifier(_hiddenType, new StyleValueFilter(Css.Properties.OPACITY, "0")));
            doc.Accept(new TreeNodeClassifier(_hiddenType, new OffscreenFilter(doc.Body.BoundingBox.Right)));
            doc.Accept(new TreeNodeClassifier(_hiddenType, new DescendantFilter(_hiddenType, _hiddenType)));
         
            doc.Accept(new TreeNodeClassifier(HtmlElementType.SignificantText, new SignificantTextFilter(HtmlElementType.HasText, HtmlElementType.HasArea, _hiddenType, HtmlElementType.DisplayNone)));
            doc.Accept(new TreeNodeClassifier(_significantBlockType, new SignificantBlockFilter(HtmlElementType.DisplayBlock, HtmlElementType.Linebreak, HtmlElementType.SignificantText)));
            doc.Accept(new TreeNodeClassifier(_significantInlineType, new SignificantInlineFilter(HtmlElementType.DisplayInline, HtmlElementType.Linebreak, HtmlElementType.SignificantText)));
            doc.Accept(new TreeNodeClassifier(_significantLinebreakType, new SignificantLinebreakFilter(HtmlElementType.Linebreak, HtmlElementType.SignificantText)));
            doc.Accept(new TreeNodeClassifier(_anameType, new AnameFilter()));


            doc.Accept(new TreeNodeClassifier(_significantInvisibleType, new SignificantInvisibleFilter(_anameType, HtmlElementType.SignificantText)));

            doc.Accept(new TreeNodeClassifier(_breakDownType, new BreakDownFilter(_significantBlockType, _significantInlineType, _significantLinebreakType, _breakDownType)), new BreadthFirstWalkerFactory().CreateReversed());
        }
    }
}
