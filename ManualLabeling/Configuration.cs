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

namespace Imppoa.ManualLabeling
{
    public class Configuration
    {
        public readonly string URL_FILENAME_FILTER = "Url|*.url";
        public readonly string SCREENSHOT_FILENAME_FILTER = "Screenshot|*.bmp";
        public readonly string ZONETREE_FILENAME_FILTER = "Zone Tree|*.zonetree";
        public readonly string URL_FILENAME_TEMPLATE = "{0}.url";
        public readonly string DOM_HTML_FILENAME_TEMPLATE = "{0}.html";
        public readonly string DOM_TEXT_FILENAME_TEMPLATE = "{0}.txt";
        public readonly string DOM_TREE_FILENAME_TEMPLATE = "{0}.domtree";
        public readonly string ZONE_TREE_FILENAME_TEMPLATE = "{0}.zonetree";
        public readonly string SCREENSHOT_FILENAME_TEMPLATE = "{0}.bmp";

        public readonly double RENDER_TIME_OUT = 10000; // 10s

        public readonly IEnumerable<string> SEED_LABELS = new[] { "title", "author", "affiliation", "abstract", "reference", "content", "other", };
    }
}
