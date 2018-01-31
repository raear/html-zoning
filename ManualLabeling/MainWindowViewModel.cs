/*
Government Usage Rights Notice:  The U.S. Government retains unlimited, royalty-free usage rights to this software, but not ownership, as provided by Federal law.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

•	Redistributions of source code must retain the above Government Usage Rights Notice, this list of conditions and the following disclaimer.

•	Redistributions in binary form must reproduce the above Government Usage Rights Notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

•	Neither the names of the National Library of Medicine, the National Institutes of Health, nor the names of any of the software developers may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE U.S. GOVERNMENT AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITEDTO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE U.S. GOVERNMENT
OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using Imppoa.HtmlZoning;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace Imppoa.ManualLabeling
{
    public class MainWindowViewModel : BindableBase
    {
        private static readonly string ZONE_OUTLINE_TYPE = "ZoneOutlineType";
        private static readonly string LABELED_ZONE_TYPE = "LabeledZone";
    
        private readonly Configuration _config;
        private readonly AppServices _appServices;
        private readonly HighlightStyleFactory _styleFactory;

        private Dictionary<string, IEnumerable<HighlightSpec>> _highlights;
        private string _url;
        private string _id;
        private bool _useWebBrowser;
        private string _webBrowserUrl;
        private string _imageBrowserUrl;
        private IBrowserDocument _document;
        private ZoneTree _zoneTree;
        private bool _addMode;
        private ObservableCollection<LabelInfo> _labels;

        public MainWindowViewModel()
        {
            _config = new Configuration();
            _appServices = new AppServices(_config);
            _styleFactory = new HighlightStyleFactory();
            this.LoadUrlCommand = new DelegateCommand(LoadUrlCommand_Execute);
            this.LoadScreenshotCommand = new DelegateCommand(LoadScreenshotCommand_Execute);
            this.SaveZoneTreeCommand = new DelegateCommand(SaveZoneTreeCommand_Execute, SaveZoneTreeCommand_CanExecute);
            this.SaveScreenshotCommand = new DelegateCommand(SaveScreenshotCommand_Execute, SaveScreenshotCommand_CanExecute);
            this.DoZoningCommand = new DelegateCommand(DoZoningCommand_Execute, DoZoningCommand_CanExecute);
            this.BrowserAreaSelectedCommand = new DelegateCommand<Rectangle?>(WebBrowserAreaSelectedCommand_Execute, WebBrowserAreaSelectedCommand_CanExecute);
            this.DeleteSelectedLabelsCommand = new DelegateCommand(DeleteSelectedLabelsCommand_Execute, DeleteSelectedLabelsCommand_CanExecute);
            this.ResetAll();
        }

        public string WindowTitle => "Manual Labeler " + (this.UseWebBrowser ? "(Web Browser)" : "(Image Viewer)") + " " + this.Id + " " + this.Url;

        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                this.OnPropertyChanged(() => this.WindowTitle);
            }
        }

        public string Url
        {
            get
            {
                return _url;
            }
            set
            {
                _url = value;
                this.OnPropertyChanged(() => this.WindowTitle);
            }
        }

        public bool UseWebBrowser
        {
            get
            {
                return _useWebBrowser;
            }
            set
            {
                _useWebBrowser = value;
                this.OnPropertyChanged(() => this.UseWebBrowser);
                this.OnPropertyChanged(() => this.WindowTitle);
            }
        }

        public string WebBrowserUrl
        {
            get
            {
                return _webBrowserUrl;
            }
            set
            {
                _webBrowserUrl = value;
                this.OnPropertyChanged(() => this.WebBrowserUrl);
            }
        }

        public string ImageBrowserUrl
        {
            get
            {
                return _imageBrowserUrl;
            }
            set
            {
                _imageBrowserUrl = value;
                this.OnPropertyChanged(() => this.ImageBrowserUrl);
            }
        }

        private bool DocumentLoaded => this.Document != null;

        public IBrowserDocument Document
        {
            get
            {
                return _document;
            }
            set
            {
                _document = value;
                this.OnPropertyChanged(() => this.Document);
                this.SaveScreenshotCommand.RaiseCanExecuteChanged();
                this.DoZoningCommand.RaiseCanExecuteChanged();
                this.OnDocumentChanged();
            }
        }

        public bool ZoneTreeLoaded => this.ZoneTree != null;

        private ZoneTree ZoneTree
        {
            get
            {
                return _zoneTree;
            }
            set
            {
                _zoneTree = value;
                this.OnPropertyChanged(() => this.ZoneTreeLoaded);
                this.SaveZoneTreeCommand.RaiseCanExecuteChanged();
            }
        }

        public bool AddMode
        {
            get
            {
                return _addMode;
            }
            set
            {
                _addMode = value;
                this.OnPropertyChanged(() => this.AddMode);
            }
        }

        public ObservableCollection<LabelInfo> Labels
        {
            get
            {
                return _labels;
            }
            set
            {
                _labels = value;
                this.SubscribeToCollectionChanged(value);
                this.SubscribeToLabelInfoPropertyChanged(value);
                this.OnPropertyChanged(() => this.Labels);
            }
        }

        private List<LabelInfo> SelectedLabels
        {
            get
            {
                return this.Labels.Where(l => l.Selected).ToList();
            }
        }

        public DelegateCommand LoadUrlCommand { get; }

        public DelegateCommand LoadScreenshotCommand { get; }

        public DelegateCommand SaveZoneTreeCommand { get; }

        public DelegateCommand SaveScreenshotCommand { get; }

        public DelegateCommand DoZoningCommand { get; }

        public DelegateCommand<Rectangle?> BrowserAreaSelectedCommand { get; }

        public DelegateCommand DeleteSelectedLabelsCommand { get; }

        private void LoadUrlCommand_Execute()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = _config.URL_FILENAME_FILTER;
            openFileDialog.FileName = this.Id;
            if (openFileDialog.ShowDialog() == true)
            {
                this.ResetAll();
                this.UseWebBrowser = true;
                string filepath = openFileDialog.FileName;
                this.Id = _appServices.GetId(filepath);
                this.Url = _appServices.LoadUrl(filepath);
                this.ZoneTree = _appServices.LoadZoneTree(filepath);
                this.WebBrowserUrl = this.Url;
            }
        }

        private void LoadScreenshotCommand_Execute()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = _config.SCREENSHOT_FILENAME_FILTER;
            openFileDialog.FileName = this.Id;
            if (openFileDialog.ShowDialog() == true)
            {
                this.ResetAll();
                this.UseWebBrowser = false;
                string filepath = openFileDialog.FileName;
                this.Id = _appServices.GetId(filepath);
                this.Url = _appServices.LoadUrl(filepath);
                this.ZoneTree = _appServices.LoadZoneTree(filepath);
                this.ImageBrowserUrl = filepath;
            }
        }

        private bool SaveZoneTreeCommand_CanExecute()
        {
            return this.ZoneTreeLoaded;
        }

        private void SaveZoneTreeCommand_Execute()
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = _config.ZONETREE_FILENAME_FILTER;
            saveFileDialog.FileName = this.Id;
            if (saveFileDialog.ShowDialog() == true)
            {
                string filename = saveFileDialog.FileName;
                _appServices.SaveZoneTree(filename, this.ZoneTree);
            }
        }

        private bool SaveScreenshotCommand_CanExecute()
        {
            return this.DocumentLoaded;
        }

        private void SaveScreenshotCommand_Execute()
        {
            this.Document.ClearHighlights();
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = _config.SCREENSHOT_FILENAME_FILTER;
            saveFileDialog.FileName = this.Id;
            if (saveFileDialog.ShowDialog() == true)
            {
                string filename = saveFileDialog.FileName;
                using (var screenshot = this.Document.TakeScreenshot(this.ZoneTree))
                {
                    _appServices.SaveScreenshot(filename, screenshot);
                }
            }
            this.ShowHighlights();
        }

        private bool DoZoningCommand_CanExecute()
        {
            return this.DocumentLoaded && this.Document.CanDoZoning();
        }

        private void DoZoningCommand_Execute()
        {
            this.Document.ClearHighlights();
            this.ZoneTree = this.Document.DoZoning();
            this.SetInitialLabels(this.ZoneTree);
            this.CreateOrUpdateZoneOutlineHighlights(this.ZoneTree);
            this.ShowHighlights();
        }

        private bool WebBrowserAreaSelectedCommand_CanExecute(Rectangle? area)
        {
            return this.DocumentLoaded && this.ZoneTreeLoaded;
        }

        private void WebBrowserAreaSelectedCommand_Execute(Rectangle? area)
        {
            var zones = this.ZoneTree.LeafNodes.Where(z => area.Value.IntersectsWith(z.BoundingBox));
            var labels = this.SelectedLabels.Select(l => l.Label);
            foreach (var zone in zones)
            {
                foreach (var label in labels)
                {
                    if (this.AddMode)
                    {
                        zone.AddClassification(label);
                    }
                    else
                    {
                        zone.RemoveClassification(label);
                    }
                }
            }
            this.CreateOrUpdateZoneLabelHighlights(this.SelectedLabels, this.ZoneTree);
        }

        private bool DeleteSelectedLabelsCommand_CanExecute()
        {
            return this.SelectedLabels.Count > 0;
        }

        private void DeleteSelectedLabelsCommand_Execute()
        {
            foreach (var selectedLabel in this.SelectedLabels)
            {
                if (_labels.Contains(selectedLabel))
                {
                    _labels.Remove(selectedLabel);
                }
            }
        }

        private void SubscribeToLabelInfoPropertyChanged(ObservableCollection<LabelInfo> labels)
        {
            foreach (var labelInfo in labels)
            {
                labelInfo.PropertyChanged += LabelInfo_PropertyChanged;
            }
        }

        private void SubscribeToCollectionChanged(ObservableCollection<LabelInfo> labels)
        {
            labels.CollectionChanged += (s, e) =>
            {
                if (e.OldItems != null)
                {
                    foreach (LabelInfo labelInfo in e.OldItems)
                    {
                        labelInfo.PropertyChanged -= LabelInfo_PropertyChanged;
                    }
                }

                if (e.NewItems != null)
                {
                    foreach (LabelInfo labelInfo in e.NewItems)
                    {
                        labelInfo.PropertyChanged += LabelInfo_PropertyChanged;
                    }
                }

                this.OnLabelInfoChanged();
            };
        }

        private void LabelInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.OnLabelInfoChanged();
        }

        private void OnLabelInfoChanged()
        {
            this.CreateOrUpdateZoneLabelHighlights(this.SelectedLabels, this.ZoneTree);
            this.DeleteSelectedLabelsCommand.RaiseCanExecuteChanged();
        }

        private void OnDocumentChanged()
        {
            if (this.DocumentLoaded && this.ZoneTreeLoaded)
            {
                this.SetInitialLabels(this.ZoneTree);
                this.CreateOrUpdateZoneOutlineHighlights(this.ZoneTree);
            }
        }

        private void SetInitialLabels(ZoneTree zoneTree)
        {
            var labelList = _config.SEED_LABELS.ToList();
            var zoneTreeLabels = zoneTree.LeafNodes.SelectMany(z => z.Classifications).Distinct();
            foreach(var zoneTreeLabel in zoneTreeLabels)
            {
                if (!labelList.Contains(zoneTreeLabel))
                {
                    labelList.Add(zoneTreeLabel);
                }
            }
            var labels = new ObservableCollection<LabelInfo>(labelList.Select(l => new LabelInfo(l, false)));
            this.Labels = labels;
        }

        private void CreateOrUpdateZoneOutlineHighlights(ZoneTree zoneTree)
        {
            var style = _styleFactory.CreateZoneOutlineStyle();
            var highlights = zoneTree.LeafNodes.Select(z => new HighlightSpec(style, z.BoundingBox));
            this.AddOrUpdateHighlights(ZONE_OUTLINE_TYPE, highlights);
        }

        private void CreateOrUpdateZoneLabelHighlights(IEnumerable<LabelInfo> selecectedLabels, ZoneTree zoneTree)
        {
            var highlights = new List<HighlightSpec>();
            foreach (var labelInfo in selecectedLabels)
            {
                string label = labelInfo.Label;
                var style = _styleFactory.CreateLabeledZoneStyle(labelInfo.ColorName);
                var labelHighlights = zoneTree.LeafNodes.Where(z => z.HasClassification(label)).Select(z => new HighlightSpec(style, z.BoundingBox));
                highlights.AddRange(labelHighlights);
            }
            this.AddOrUpdateHighlights(LABELED_ZONE_TYPE, highlights);
        }

        private void AddOrUpdateHighlights(string type, IEnumerable<HighlightSpec> highlights)
        {
            if (_highlights.ContainsKey(type))
            {
                _highlights[type] = highlights;
            }
            else
            {
                _highlights.Add(type, highlights);
            }
            this.ShowHighlights();
        }

        private void ShowHighlights()
        {
            if (this.DocumentLoaded)
            {
                this.Document.ShowHighlights(this.ZoneTree, _highlights.Values.SelectMany(h => h));
            }
        }

        private void ResetAll()
        {
            _highlights = new Dictionary<string, IEnumerable<HighlightSpec>>();
            _highlights.Add(ZONE_OUTLINE_TYPE, new List<HighlightSpec>());
            _highlights.Add(LABELED_ZONE_TYPE, new List<HighlightSpec>());
            this.ShowHighlights();

            this.Id = string.Empty;
            this.Url = string.Empty;
            this.UseWebBrowser = true;
            this.WebBrowserUrl = string.Empty;
            this.ImageBrowserUrl = string.Empty;
            this.Document = null;
            this.ZoneTree = null;

            this.AddMode = true;
            this.Labels = new ObservableCollection<LabelInfo>();
        }
    }
}
