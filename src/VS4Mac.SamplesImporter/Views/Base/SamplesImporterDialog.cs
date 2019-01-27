using System.Collections.Generic;
using MonoDevelop.Components;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using VS4Mac.SamplesImporter.Controllers;
using VS4Mac.SamplesImporter.Controllers.Base;
using VS4Mac.SamplesImporter.Controls;
using VS4Mac.SamplesImporter.Models;
using Xwt;
using Xwt.Drawing;

namespace VS4Mac.SamplesImporter.Views.Base
{
    public interface ISamplesImporterView : IView
    {

    }

    public class SamplesImporterDialog : Xwt.Dialog, ISamplesImporterView
    {
        VBox _mainBox;
        RoundedFrameBox _headerFrameBox;
        HBox _headerBox;
        SearchTextEntry _searchEntry;
        RoundedFrameBox _contentFrameBox;
        HBox _contentBox;
        TreeView _samplesView;
        DataField<string> _nameField;
        DataField<Sample> _sampleField;
        TreeStore _samplesStore;
        VBox _descriptionBox;
        Label _titleLabel;
        Label _titleValueLabel;
        Label _descriptionLabel;
        Label _descriptionValueLabel;
        Label _platformsLabel;
        VBox _platformsBox;
        Label _tagsLabel;
        HBox _tagsBox;
        VBox _previewBox;
        Xwt.ImageView _previewView;
        HBox _buttonBox;
        Button _cancelButton;
        Button _continueButton;

        SamplesImporterController _controller;

        public SamplesImporterDialog()
        {
            Init();
            BuildGui();
            AttachEvents();
        }

        void Init()
        {
            _mainBox = new VBox
            {
                BackgroundColor = MonoDevelop.Ide.Gui.Styles.BackgroundColor,
                HeightRequest = 500,
                WidthRequest = 800
            };

            _headerFrameBox = new RoundedFrameBox
            {
                CornerRadiusTopLeft = 6,
                CornerRadiusTopRight = 6,
                CornerRadiusBottomLeft = 0,
                CornerRadiusBottomRight = 0,
                BorderBottom = false,
                BorderWidth = 2,
                InnerBackgroundColor = MonoDevelop.Ide.Gui.Styles.BackgroundColor
            };

            _headerBox = new HBox
            {
                HeightRequest = 36
            };

            _searchEntry = new SearchTextEntry
            {
                PlaceholderText = "Filter samples",
                HeightRequest = 24,
                WidthRequest = 200
            };

            _contentFrameBox = new RoundedFrameBox
            {
                CornerRadius = 0,
                BorderWidth = 2,
                InnerBackgroundColor = MonoDevelop.Ide.Gui.Styles.BackgroundColor,
                Margin = new WidgetSpacing(0, -6, 0, 0)
            };

            _contentBox = new HBox();

            _samplesView = new TreeView
            {
                BackgroundColor = MonoDevelop.Ide.Gui.Styles.BackgroundColor,
                GridLinesVisible = GridLines.None,
                HeadersVisible = false,
                BorderVisible = false,
                UseAlternatingRowColors = true,
                WidthRequest = 200
            };

            _nameField = new DataField<string>();
            _sampleField = new DataField<Sample>();

            _samplesStore = new TreeStore(_nameField, _sampleField);

            _descriptionBox = new VBox
            {
                WidthRequest = 300
            };

            _titleLabel = new Label("Title")
            {
                Font = Font.SystemFont.WithScaledSize(1.25),
                Margin = new WidgetSpacing(0, 12, 0, 0)
            };

            _titleValueLabel = new Label();

            _descriptionLabel = new Label("Description")
            {
                Font = Font.SystemFont.WithScaledSize(1.25),
                Margin = new WidgetSpacing(0, 12, 0, 0)
            };

            _descriptionValueLabel = new Label
            {
                Wrap = WrapMode.Word,
                WidthRequest = 150
            };

            _platformsLabel = new Label("Platforms")
            {
                Font = Font.SystemFont.WithScaledSize(1.25),
                Margin = new WidgetSpacing(0, 12, 0, 0)
            };

            _platformsBox = new VBox();

            _tagsLabel = new Label("Tags")
            {
                Font = Font.SystemFont.WithScaledSize(1.25),
                Margin = new WidgetSpacing(0, 12, 0, 0)
            };

            _tagsBox = new HBox();

            _previewBox = new VBox
            {
                WidthRequest = 300
            };

            _previewView = new Xwt.ImageView
            {
                BackgroundColor = Colors.Gray,
                HorizontalPlacement = WidgetPlacement.End,
                VerticalPlacement = WidgetPlacement.Start,
                Margin = new WidgetSpacing(2, 2, 2, 2)
            };

            _buttonBox = new HBox();

            _cancelButton = new Button("Cancel");

            _continueButton = new Button("Continue")
            {
                BackgroundColor = MonoDevelop.Ide.Gui.Styles.BaseSelectionBackgroundColor,
                LabelColor = MonoDevelop.Ide.Gui.Styles.BaseSelectionTextColor
            };
        }

        void BuildGui()
        {
            Title = "Samples Importer";

            _headerBox.PackEnd(_searchEntry);
            _headerFrameBox.Content = _headerBox;

            _samplesView.Columns.Add(new ListViewColumn(string.Empty, new TextCellView(_nameField)));
            _samplesView.DataSource = _samplesStore;

            _descriptionBox.PackStart(_titleLabel);
            _descriptionBox.PackStart(_titleValueLabel);
            _descriptionBox.PackStart(_descriptionLabel);
            _descriptionBox.PackStart(_descriptionValueLabel);
            _descriptionBox.PackStart(_platformsLabel);
            _descriptionBox.PackStart(_platformsBox);
            _descriptionBox.PackStart(_tagsLabel);
            _descriptionBox.PackStart(_tagsBox);

            _previewBox.PackStart(_previewView);

            _contentBox.PackStart(_samplesView, false);
            _contentBox.PackStart(_descriptionBox, true);
            _contentBox.PackStart(_previewBox, false);

            _contentFrameBox.Content = _contentBox;

            _buttonBox.PackEnd(_continueButton);
            _buttonBox.PackEnd(_cancelButton);

            _mainBox.PackStart(_headerFrameBox);
            _mainBox.PackStart(_contentFrameBox, true);
            _mainBox.PackStart(_buttonBox);

            Content = _mainBox;
            Resizable = false;
        }

        void AttachEvents()
        {
            _searchEntry.Changed += OnSearchEntryChanged;
            _samplesView.SelectionChanged += OnSamplesViewSelectionChanged;
            _cancelButton.Clicked += OnCancelButtonClicked;
            _continueButton.Clicked += OnContinueButtonClicked;
        }

        public void SetController(IController controller)
        {
            _controller = (SamplesImporterController)controller;

            LoadData();
        }

        internal void LoadData()
        {
            Loading(true);

            var samples = _controller.LoadData();

            FillData(samples);

            Loading(false);
        }

        internal void FillData(List<Sample> samples)
        {
            _samplesStore.Clear();

            foreach (var sample in samples)
            {
                var row = _samplesStore.AddNode();
                row.SetValue(_nameField, sample.Name);
                row.SetValue(_sampleField, sample);
            }

            var firstNode = _samplesStore.GetFirstNode();
            _samplesView.SelectRow(firstNode.CurrentPosition);
        }

        internal void LoadSelectedSample()
        {
            _titleValueLabel.Text = _controller.SelectedSample.Name;
            _descriptionValueLabel.Text = _controller.SelectedSample.Description;

            if (!string.IsNullOrWhiteSpace(_controller.SelectedSample.Screenshot))
                _previewView.Image = Image.FromFile(_controller.SelectedSample.Screenshot).WithBoxSize(420);
            else
                _previewView.Image = null;

            _platformsBox.Clear();

            if (_controller.SelectedSample.Platforms != null)
            {
                foreach (var platform in _controller.SelectedSample.Platforms)
                {
                    var platformLabel = new Label($"- {platform}");

                    _platformsBox.PackStart(platformLabel);
                }
            }

            _tagsBox.Clear();

            if (_controller.SelectedSample.Tags != null)
            {
                foreach (var platform in _controller.SelectedSample.Tags)
                {
                    var tagWidget = new TagWidget
                    {
                        Text = platform
                    };

                    _tagsBox.PackStart(tagWidget);
                }
            }

            _continueButton.Sensitive = !string.IsNullOrEmpty(_controller.SelectedSample.Url);
        }

        void Loading(bool isLoading)
        {
            _searchEntry.Sensitive = !isLoading;
            _continueButton.Sensitive = !isLoading;
        }

        void OnSearchEntryChanged(object sender, System.EventArgs e)
        {
            var filter = _searchEntry.Text;
            var filteredSamples = _controller.FilterData(filter);
            FillData(filteredSamples);
        }

        void OnSamplesViewSelectionChanged(object sender, System.EventArgs e)
        {
            var row = _samplesView.SelectedRow;

            if (row != null)
            {
                var node = _samplesStore.GetNavigatorAt(row);

                if (node != null)
                {
                    var sample = node.GetValue(_sampleField) as Sample;
                    _controller.SelectedSample = sample;
                    LoadSelectedSample();
                }
            }
        }

        void OnCancelButtonClicked(object sender, System.EventArgs e)
        {
            Respond(Command.Cancel);
            Close();
        }

        async void OnContinueButtonClicked(object sender, System.EventArgs e)
        {
            if (_controller.SelectedSample == null)
                return;

            var progressMonitor = IdeApp.Workbench.ProgressMonitors.GetStatusProgressMonitor("Opening project...", Stock.StatusSolutionOperation, false, true, false);

            Loading(true);

            var projectPath = await _controller.DownloadSampleAsync();

            Loading(false);

            if (!string.IsNullOrWhiteSpace(projectPath))
            {
                Respond(Command.Ok);
                Close();
                await _controller.OpenSolutionAsync(projectPath);
            }

            progressMonitor.EndTask();
            progressMonitor.Dispose();
        }
    }
}
