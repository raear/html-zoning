/*
Government Usage Rights Notice:  The U.S. Government retains unlimited, royalty-free usage rights to this software, but not ownership, as provided by Federal law.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

•	Redistributions of source code must retain the above Government Usage Rights Notice, this list of conditions and the following disclaimer.

•	Redistributions in binary form must reproduce the above Government Usage Rights Notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

•	Neither the names of the National Library of Medicine, the National Institutes of Health, nor the names of any of the software developers may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE U.S. GOVERNMENT AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITEDTO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE U.S. GOVERNMENT
OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using Imppoa.Labeling;
using OpenNLP.Tools.PosTagger;
using OpenNLP.Tools.SentenceDetect;
using OpenNLP.Tools.Tokenize;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Imppoa.NLP
{
    /// <summary>
    /// Natural language processor using the OpenNLP library
    /// </summary>
    public class OpenNaturalLanguageProcessor : INaturalLanguageProcessing
    {
        private const string VERB_TAG_PREFIX = "vb";
        private static EnglishMaximumEntropyTokenizer _tokenizer;
        private static EnglishMaximumEntropySentenceDetector _sentenceDetector;
        private static EnglishMaximumEntropyPosTagger _posTagger;
        private static bool _isInitialized = false;

        /// <summary>
        /// Initializes this instance
        /// </summary>
        private static void Initialize()
        {
            string assFilename = Assembly.GetExecutingAssembly().Location;
            string rootPath = Path.GetDirectoryName(assFilename);
            Initialize(rootPath);
        }

        /// <summary>
        /// Initializes the processor
        /// </summary>
        /// <param name="rootPath">The root path</param>
        private static void Initialize(string rootPath)
        {
            string tkModelPath = Path.Combine(rootPath, "EnglishTok.nbin");
            string sdModelPath = Path.Combine(rootPath, "EnglishSD.nbin");
            string posModelPath = Path.Combine(rootPath, "EnglishPOS.nbin");
            _tokenizer = new EnglishMaximumEntropyTokenizer(tkModelPath);
            _sentenceDetector = new EnglishMaximumEntropySentenceDetector(sdModelPath);
            _posTagger = new EnglishMaximumEntropyPosTagger(posModelPath);
            _isInitialized = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenNaturalLanguageProcessor"/> class
        /// </summary>
        public OpenNaturalLanguageProcessor()
        {
            if (!_isInitialized)
            {
                Initialize();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenNaturalLanguageProcessor"/> class
        /// </summary>
        /// <param name="rootPath">The root path.</param>
        public OpenNaturalLanguageProcessor(string rootPath)
        {
            if (!_isInitialized)
            {
                Initialize(rootPath);
            }
        }

        /// <summary>
        /// Gets the tokens
        /// </summary>
        /// <param name="text">The text</param>
        /// <returns>
        /// the tokens
        /// </returns>
        public string[] GetTokens(string text)
        {
            return _tokenizer.Tokenize(text);
        }

        /// <summary>
        /// Counts the number of tokens
        /// </summary>
        /// <param name="text">The text</param>
        /// <returns>
        /// The number of tokens
        /// </returns>
        public int CountTokens(string text)
        {
            var words = this.GetTokens(text);
            return words.Length;
        }

        /// <summary>
        /// Counts the number of sentances with a verb
        /// </summary>
        /// <param name="text">The text</param>
        /// <returns>
        /// the number of sentances with a verb
        /// </returns>
        public int CountSentencesWithVerb(string text)
        {
            int sentenceCount = 0;
            var sentences = GetSentences(text);
            foreach(var sentence in sentences)
            {
                var tokens = GetTokens(sentence);
                var tags = GetPosTags(tokens);
                if (tags.Any(t => IsVerbTag(t)))
                {
                    sentenceCount++;
                }
            }

            return sentenceCount;
        }

        /// <summary>
        /// Gets the sentences
        /// </summary>
        /// <param name="text">The text</param>
        /// <returns>the sentences</returns>
        private string[] GetSentences(string text)
        {
            return _sentenceDetector.SentenceDetect(text);
        }

        /// <summary>
        /// Gets the parts of speach tags
        /// </summary>
        /// <param name="tokens">The tokens</param>
        /// <returns>the parts of speach tags</returns>
        private string[] GetPosTags(string[] tokens)
        {
            return _posTagger.Tag(tokens);
        }

        /// <summary>
        /// Determines whether the tag is a verb tag
        /// </summary>
        /// <param name="tag">The tag</param>
        /// <returns>true, if the tag is a verb tag, otherwise false</returns>
        private bool IsVerbTag(string tag)
        {
            return tag.ToLower().StartsWith(VERB_TAG_PREFIX);
        }
    }
}
