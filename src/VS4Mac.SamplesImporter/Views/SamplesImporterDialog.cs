using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MonoDevelop.Components;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.ProgressMonitoring;
using VS4Mac.SamplesImporter.Controllers;
using VS4Mac.SamplesImporter.Controllers.Base;
using VS4Mac.SamplesImporter.Controls;
using VS4Mac.SamplesImporter.Models;
using VS4Mac.SamplesImporter.Views.Base;
using Xwt;
using Xwt.Drawing;

namespace VS4Mac.SamplesImporter.Views
{
    public interface ISamplesImporterView : IView
    {

    }

    public class SamplesImporterDialog : Xwt.Dialog, ISamplesImporterView
    {
        VBox _mainBox;
        RoundedFrameBox _headerFrameBox;
        HBox _headerBox;
		ComboBox _feedBox;
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
		MDSpinner _loadingSpinner;
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
                InnerBackgroundColor = Styles.BackgroundColor
            };

            _headerBox = new HBox
            {
                HeightRequest = 36
            };

			_feedBox = new ComboBox
			{
				WidthRequest = 200
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
                InnerBackgroundColor = Styles.BackgroundColor,
                Margin = new WidgetSpacing(0, -6, 0, 0)
            };

            _contentBox = new HBox();

            _samplesView = new TreeView
            {
                BackgroundColor = Styles.BackgroundColor,
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

			_loadingSpinner = new MDSpinner
			{
				Animate = true
			};

			_buttonBox = new HBox();

            _cancelButton = new Button("Cancel");

            _continueButton = new Button("Continue")
            {
                BackgroundColor = Styles.BaseSelectionBackgroundColor,
                LabelColor = Styles.BaseSelectionTextColor
            };
        }

        void BuildGui()
        {
            Title = "Samples Importer";
            
			_headerBox.PackStart(_feedBox); 
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

			_contentBox.PackStart(_loadingSpinner, true, WidgetPlacement.Center);
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
			_feedBox.SelectionChanged += OnFeedBoxSelectionChanged;
            _searchEntry.Changed += OnSearchEntryChanged;
            _samplesView.SelectionChanged += OnSamplesViewSelectionChanged;
            _cancelButton.Clicked += OnCancelButtonClicked;
            _continueButton.Clicked += OnContinueButtonClicked;
		}

		public void SetController(IController controller)
        {
            _controller = (SamplesImporterController)controller;

			LoadRepositories();
		}

		internal void LoadRepositories()
		{
			var repositories = _controller.LoadSampleRepositories();

			foreach(var repository in repositories)
			{
				_feedBox.Items.Add(repository, repository.Name);
			}

			_feedBox.SelectedIndex = 2;
		}

		internal async Task LoadDataAsync(string sampleRepositoryId)
		{
			var progressMonitor = IdeApp.Workbench.ProgressMonitors.GetStatusProgressMonitor("Loading samples...", Stock.StatusSolutionOperation, false, true, false);

			try
			{
				Loading(true);

				var samples = await _controller.LoadSamplesAsync(sampleRepositoryId, CancellationToken.None);

				FillData(samples);

				Loading(false);
			}
			catch(Exception ex)
			{
				MessageDialog.ShowError(ex.Message);
			}
			finally
			{
				progressMonitor.EndTask();
				progressMonitor.Dispose();
			}
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

			if (firstNode != null)
			{
				_samplesView.SelectRow(firstNode.CurrentPosition);
			}
        }

        internal async Task LoadSelectedSampleAsync()
        {
			await _controller.UpdateSampleDetail();

            _titleValueLabel.Text = _controller.SelectedSample.Name;
            _descriptionValueLabel.Text = _controller.SelectedSample.Brief;

			_platformsBox.Clear();

            if (!string.IsNullOrEmpty(_controller.SelectedSample.SupportedPlatforms))
            {
				var platforms = _controller.SelectedSample.SupportedPlatforms.Split(',');

                foreach (var platform in platforms)
                {
                    var platformLabel = new Label($"- {platform}");

                    _platformsBox.PackStart(platformLabel);
                }
            }

            _tagsBox.Clear();

            if (!string.IsNullOrEmpty(_controller.SelectedSample.Tags))
            {
				var tags = _controller.SelectedSample.Tags.Split(',');

                foreach (var platform in tags)
                {
                    var tagWidget = new TagWidget
                    {
                        Text = platform
                    };

                    _tagsBox.PackStart(tagWidget);
                }
            }

            _continueButton.Sensitive = !string.IsNullOrEmpty(_controller.SelectedSample.Url);

			_previewView.Image = null;

			if (_controller.SelectedSample.Pictures.Any())
			{
				var pictureUrl = _controller.SelectedSample.Pictures.First();
				var picture = _controller.LoadPicture(pictureUrl);

				if (picture != null)
				{
					_previewView.Image = Image.FromStream(picture).WithBoxSize(420);
				}
			}
		}

		void Loading(bool isLoading)
        {
			_loadingSpinner.Visible = isLoading;
			_samplesView.Visible = !isLoading;
			_descriptionBox.Visible = !isLoading;
			_previewBox.Visible = !isLoading;
			_feedBox.Sensitive = !isLoading;
			_searchEntry.Sensitive = !isLoading;
            _continueButton.Sensitive = !isLoading;
        }

		async void OnFeedBoxSelectionChanged(object sender, System.EventArgs e)
		{
			var row = _feedBox.SelectedIndex;

			if (row == -1)
			{
				return;
			}

			if (_feedBox.SelectedItem is SampleRepository sampleRepository)
			{
				await LoadDataAsync(sampleRepository.SampleRepositoryId);
			}
		}

		void OnSearchEntryChanged(object sender, System.EventArgs e)
        {
            var filter = _searchEntry.Text;
            var filteredSamples = _controller.FilterData(filter);
            FillData(filteredSamples);
        }

        async void OnSamplesViewSelectionChanged(object sender, System.EventArgs e)
        {
            var row = _samplesView.SelectedRow;

            if (row != null)
            {
                var node = _samplesStore.GetNavigatorAt(row);

                if (node != null)
                {
                    var sample = node.GetValue(_sampleField) as Sample;
                    _controller.SelectedSample = sample;
                    await LoadSelectedSampleAsync();
                }
            }
        }

        void OnCancelButtonClicked(object sender, System.EventArgs e)
        {
            Respond(Command.Cancel);
            Close();
        }

        async void OnContinueButtonClicked(object sender, EventArgs e)
        {
            if (_controller.SelectedSample == null)
                return;

			ProgressMonitor progressMonitor = new MessageDialogProgressMonitor(true, false, true, true);

			Loading(true);

            var projectPath = await _controller.DownloadSampleAsync(progressMonitor);

            Loading(false);
          
			if (string.IsNullOrWhiteSpace(projectPath))
            {
				var errorMessage = "An error has occurred downloading the sample.";
				progressMonitor.ReportError(errorMessage);
				MessageService.ShowError(errorMessage);
			}
			else
			{
				progressMonitor.ReportSuccess("Sample downloaded.");
			}

			progressMonitor.EndTask();
			progressMonitor.Dispose();

			bool downloaded = !string.IsNullOrWhiteSpace(projectPath);

			if (downloaded)
			{
				Respond(Command.Ok);
				Close();

				await _controller.OpenSolutionAsync(projectPath);
			}
		}
    }
}
