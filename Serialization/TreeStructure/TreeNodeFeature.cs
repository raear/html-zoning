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

namespace Imppoa.HtmlZoning.TreeStructure
{
    /// <summary>
    /// A computed tree node feature
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TreeNodeFeature
    {
        private object _value;

        /// <summary>
        /// Gets or sets the feature name
        /// </summary>
        /// <value>
        /// The feature name
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeNodeFeature"/> class.
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="value">The value</param>
        public TreeNodeFeature(string name, object value)
        {
            this.Name = name;
            _value = value;
        }

        /// <summary>
        /// Gets the value
        /// </summary>
        /// <returns>the value as an object</returns>
        public object GetValue()
        {
            return _value;
        }

        /// <summary>
        /// Gets the value
        /// </summary>
        /// <typeparam name="T">the type of the value</typeparam>
        /// <returns>the value</returns>
        public T GetValue<T>()
        {
            return (T) _value;
        }

        /// <summary>
        /// Returns the value as a double
        /// </summary>
        /// <returns>the value as a double</returns>
        public double AsDouble()
        {
            return this.GetValue<double>();
        }

        /// <summary>
        /// Returns the value as a decimal
        /// </summary>
        /// <returns>the value as a decimal</returns>
        public decimal AsDecimal()
        {
            return this.GetValue<decimal>();
        }

        /// <summary>
        /// Returns the value as an integer
        /// </summary>
        /// <returns>the value as an integer</returns>
        public int AsInt()
        { 
            return this.GetValue<int>();
        }

        /// <summary>
        /// Returns the value as a string
        /// </summary>
        /// <returns>the value as a string</returns>
        public string AsString()
        {
            return this.GetValue<string>();
        }

        /// <summary>
        /// Returns the value as a boolean
        /// </summary>
        /// <returns>the value as a boolean</returns>
        public bool AsBool()
        {
            return this.GetValue<bool>();
        }

        /// <summary>
        /// Whether this tree node feature is equal to the other tree node feature
        /// </summary>
        /// <param name="other">The other feature</param>
        /// <param name="differences">(out) The differences</param>
        /// <returns>true, if equal, otherwise false</returns>
        public bool Equals(TreeNodeFeature other, out IEnumerable<string> outDifferences)
        {
            bool areEqual = true;
            var differences = new List<string>();

            if (other == null)
            {
                areEqual = false;
                differences.Add("Other tree node feature is null");
            }
            else
            {
                if (this.Name != other.Name)
                {
                    areEqual = false;
                    differences.Add(string.Format("Different tree node feature name: {0}|{1}", this.Name, other.Name));
                }

                if (!_value.Equals(other._value))
                {
                    areEqual = false;
                    differences.Add(string.Format("Different tree node feature value"));
                }
            }

            outDifferences = differences;

            return areEqual;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            string stringRepresentation = (_value != null) ? _value.ToString() : string.Empty;
            return string.Format("{0}: {1}", this.Name, stringRepresentation);
        }
    }
}
