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
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Point = System.Drawing.Point;

namespace Imppoa.ManualLabeling
{
    public partial class ImageBrowserControl : BrowserControl
    {
        private WpfHighlight _selectionBox;

        public ImageBrowserControl()
        {
            this.InitializeComponent();
            _selectionBox = null;
            _content.MouseLeftButtonDown += OnMouseLeftButtonDown;
            _content.MouseMove += OnMouseMove;
            _content.MouseLeftButtonUp += OnMouseLeftButtonUp;
        }

        public static readonly DependencyProperty UrlProperty = DependencyProperty.Register("Url", 
                                                                typeof(string), 
                                                                typeof(ImageBrowserControl),
                                                                new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnUrlChanged)));

        public string Url
        {
            get { return (string) GetValue(UrlProperty); }
            set { SetValue(UrlProperty, value); }
        }

        private static void OnUrlChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var browser = obj as ImageBrowserControl;
            if (browser != null)
            {
                browser.OnUrlChanged(args);
            }
        }

        public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register("Document", 
                                                                                                typeof(IBrowserDocument), 
                                                                                                typeof(ImageBrowserControl));

        public IBrowserDocument Document
        {
            get { return (IBrowserDocument) GetValue(DocumentProperty); }
            set { SetValue(DocumentProperty, value); }
        }

        public static readonly DependencyProperty OnAreaSelectedProperty = DependencyProperty.Register("OnAreaSelected",
                                                                           typeof(ICommand),
                                                                           typeof(ImageBrowserControl));

        public ICommand OnAreaSelected
        {
            get { return (ICommand) GetValue(OnAreaSelectedProperty); }
            set { SetValue(OnAreaSelectedProperty, value); }
        }

      
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var point = this.GetAbsoluteMousePosition(e);
            base.OnMouseLeftButtonDown(point);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var point = this.GetAbsoluteMousePosition(e);
            base.OnMouseMove(point);
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var point = this.GetAbsoluteMousePosition(e);
            base.OnMouseLeftButtonUp();
        }

        private Point GetAbsoluteMousePosition(MouseEventArgs args)
        {
            var relPosition = args.GetPosition(this);
            var absPosition = new System.Windows.Point(relPosition.X + _scrollViewer.ContentHorizontalOffset, relPosition.Y + _scrollViewer.ContentVerticalOffset);
            return Utils.Convert(absPosition);
        }

        protected override void SetUrl(string url)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(url);
            bitmap.EndInit();
            _image.Source = bitmap;
            this.Document = new ImageBrowserDocument(url, _highlightCanvas);
        }

        protected override void ClearUrl()
        {
            _image.Source = new BitmapImage();
            this.Document = null;
        }

        protected override void CreateSelectionBox(Rectangle boundingBox)
        {
            _selectionBox = new WpfHighlight(_selectionBoxStyle, boundingBox);
            _highlightCanvas.Children.Add(_selectionBox);
        }

        protected override void UpdateSelectionBox(Rectangle boundingBox)
        {
            _selectionBox.SetBoundingBox(boundingBox);
        }

        protected override void RemoveSelectionBox()
        {
            _highlightCanvas.Children.Remove(_selectionBox);
            _selectionBox = null;
        }

        protected override void NotifyAreaSelected()
        {
            var area = _selectionBox.GetBoundingBox(_highlightCanvas);
            if (this.OnAreaSelected.CanExecute(area))
            {
                this.OnAreaSelected?.Execute(area);
            }
        }
    }
}
