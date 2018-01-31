/*
Government Usage Rights Notice:  The U.S. Government retains unlimited, royalty-free usage rights to this software, but not ownership, as provided by Federal law.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

•	Redistributions of source code must retain the above Government Usage Rights Notice, this list of conditions and the following disclaimer.

•	Redistributions in binary form must reproduce the above Government Usage Rights Notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

•	Neither the names of the National Library of Medicine, the National Institutes of Health, nor the names of any of the software developers may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE U.S. GOVERNMENT AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITEDTO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE U.S. GOVERNMENT
OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using mshtml;
using System;
using System.Drawing;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using Point = System.Drawing.Point;

namespace Imppoa.ManualLabeling
{
    public partial class WebBrowserControl : BrowserControl
    {
        private readonly double _renderTimeout;
        private readonly Timer _timer;
        private readonly HighlightStyle _eventCaptureSurfaceStyle;

        private HtmlHighlight _eventCaptureSurface;
        private HtmlHighlight _selectionBox;

        private IHTMLDocument2 MshtmlDocument => (IHTMLDocument2)_wpfControl.Document;

        public WebBrowserControl()
        {
            this.InitializeComponent();
            _renderTimeout = new Configuration().RENDER_TIME_OUT;
            _timer = this.CreateTimer();
            _eventCaptureSurfaceStyle = new HighlightStyleFactory().CreateEventCaptureSurfaceStyle();
            _eventCaptureSurface = null;
            _selectionBox = null;  
            _wpfControl.LoadCompleted += WpfControl_LoadCompleted;
        }

        public static readonly DependencyProperty UrlProperty = DependencyProperty.Register("Url", 
                                                                typeof(string), 
                                                                typeof(WebBrowserControl),
                                                                new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnUrlChanged)));

        public string Url
        {
            get { return (string) GetValue(UrlProperty); }
            set { SetValue(UrlProperty, value); }
        }

        private static void OnUrlChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var browser = obj as WebBrowserControl;
            if (browser != null)
            {
                browser.OnUrlChanged(args);
            }
        }

        public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register("Document", 
                                                                    typeof(IBrowserDocument), 
                                                                    typeof(WebBrowserControl));

        public IBrowserDocument Document
        {
            get { return (IBrowserDocument) GetValue(DocumentProperty);}
            set { SetValue(DocumentProperty, value);}
        }

        public static readonly DependencyProperty OnAreaSelectedProperty = DependencyProperty.Register("OnAreaSelected", 
                                                                           typeof(ICommand), 
                                                                           typeof(WebBrowserControl));

        public ICommand OnAreaSelected
        {
            get { return (ICommand) GetValue(OnAreaSelectedProperty); }
            set { SetValue(OnAreaSelectedProperty, value); }
        }

        private void WpfControl_LoadCompleted(object sender, NavigationEventArgs e)
        {
            if (e.Uri == _wpfControl.Source)
            {
                this.OnNaviagtionComplete();
            }
        }

        private void Document_HighlightsUpdated(object sender, Rectangle bb)
        {
            _eventCaptureSurface?.Dispose();
            var spec = new HighlightSpec(_eventCaptureSurfaceStyle, bb);
            _eventCaptureSurface = HtmlHighlight.Create(this.MshtmlDocument, spec, OnMouseDownCallback, OnMouseMoveCallback, OnMouseUpCallback);
        }

        private void OnMouseDownCallback(IHTMLEventObj target)
        {
            var point = this.CreatePoint(target);
            base.OnMouseLeftButtonDown(point);
        }

        private void OnMouseMoveCallback(IHTMLEventObj target)
        {
            var point = this.CreatePoint(target);
            base.OnMouseMove(point);
        }

        private void OnMouseUpCallback(IHTMLEventObj target)
        {
            base.OnMouseLeftButtonUp();
        }

        protected override void SetUrl(string url)
        {
            try
            {
                var uriBuilder = new UriBuilder(url);
                var uri = uriBuilder.Uri;
                _wpfControl.Navigate(uri);
                _timer.Start();
                this.Document = null;
            }
            catch (UriFormatException)
            {
                // Do nothing
            }
        }

        protected override void ClearUrl()
        {
        }

        protected override void CreateSelectionBox(Rectangle boundingBox)
        {
            var spec = new HighlightSpec(_selectionBoxStyle, boundingBox);
            _selectionBox?.Dispose();
            _selectionBox = HtmlHighlight.Create(this.MshtmlDocument, spec);
        }

        protected override void UpdateSelectionBox(Rectangle boundingBox)
        {
            this.CreateSelectionBox(boundingBox);
        }

        protected override void RemoveSelectionBox()
        {
            _selectionBox?.Dispose();
            _selectionBox = null;
        }

        protected override void NotifyAreaSelected()
        {
            var area = _selectionBox.BoundingBox;
            if (this.OnAreaSelected.CanExecute(area))
            {
                this.OnAreaSelected?.Execute(area);
            }
        }

        private void OnNaviagtionComplete()
        {
            _timer.Stop();
            var document = new WebBrowserDocument(this.Url, this.MshtmlDocument);
            document.HighlightsUpdated += Document_HighlightsUpdated;
            this.Document = document;
        }

        private Timer CreateTimer()
        {
            var timer = new Timer(_renderTimeout);
            timer.AutoReset = false;
            timer.Elapsed += (s, e) => Dispatcher.Invoke(() => this.OnNaviagtionComplete());
            return timer;
        }

        private Point CreatePoint(IHTMLEventObj target)
        {
            return new Point(target.x, target.y);
        }
    }
}
