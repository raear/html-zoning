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
using System.Diagnostics;
using System.Threading;

namespace Imppoa.HtmlRendering
{
    /// <summary>
    /// Custom spin wait implementation
    /// </summary>
    public class SpinWait
    {
        private int _interval;
        private Stopwatch _stopwatch;

        /// <summary>
        /// Gets the spin count
        /// </summary>
        /// <value>
        /// The spin count
        /// </value>
        public int Count { get; private set; }

        /// <summary>
        /// Gets the wait time
        /// </summary>
        /// <value>
        /// The wait time
        /// </value>
        public double WaitTime { get; private set; }

        /// <summary>
        /// The condition to wait for
        /// </summary>
        public Func<double, bool> Condition;

        /// <summary>
        /// The action to perfomn on the spin
        /// </summary>
        public Action OnSpin;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpinWait"/> class
        /// </summary>
        public SpinWait()
        {
            _interval = 1;
            _stopwatch = new Stopwatch();
            this.Count = 0;
            this.WaitTime = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpinWait"/> class
        /// </summary>
        /// <param name="interval">The interval in milliseconds</param>
        public SpinWait(int interval)
            : this()
        {
            _interval = interval;
        }

        /// <summary>
        /// Waits until the condition is true
        /// The condition is checked each interval
        /// </summary>
        public virtual void Wait()
        {
            this.Count = 0;
            this.WaitTime = 0;
      
            _stopwatch.Reset();
            _stopwatch.Start();
            do
            {
                Thread.Sleep(_interval);
                OnSpin();
                this.Count += 1;
                this.WaitTime = _stopwatch.Elapsed.TotalMilliseconds;
            }
            while 
            (
                !Condition(this.WaitTime)
            );
            _stopwatch.Stop();
        }
    }
}
