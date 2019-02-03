using System;
using VS4Mac.SamplesImporter.Services;
using Xwt;

namespace VS4Mac.SamplesImporter.Controls
{
	public class SamplesImporterSettingsWidget : VBox
	{
		HBox _linkBox;
		Label _tokenDescriptionLabel;
		LinkLabel _createTokenLink;
		Label _tokenLabel;
		TextEntry _tokenEntry;
		Label _pathDescriptionLabel;
		HBox _pathBox;
		TextEntry _pathEntry;
		Button _browseButton;

		SettingsService _settingsService;

		public SamplesImporterSettingsWidget()
		{
			Init();
			BuildGui();
			AttachEvents();
			LoadData();
		}

		void Init()
		{
			_settingsService = new SettingsService();

			_linkBox = new HBox();

			_tokenDescriptionLabel = new Label("To get the samples it is necessary to use a GitHub Personal Access Token API Key.")
			{
				Font = Font.WithSize(10)
			};

			_createTokenLink = new LinkLabel("Create Token")
			{
				Font = Font.WithSize(10),
				Uri = new Uri("https://help.github.com/articles/creating-a-personal-access-token-for-the-command-line/")
			};

			_tokenLabel = new Label("Token");
			_tokenEntry = new TextEntry();

			_pathDescriptionLabel = new Label("Samples folder");
			_pathBox = new HBox();

			_pathEntry = new TextEntry
			{
				ReadOnly = true
			};

			_browseButton = new Button("Browse Folder");
		}

		void BuildGui()
		{
			_linkBox.PackStart(_tokenDescriptionLabel);
			_linkBox.PackStart(_createTokenLink);

			_pathBox.PackStart(_pathEntry, true);
			_pathBox.PackEnd(_browseButton, false);

			PackStart(_linkBox);
			PackStart(_tokenLabel);
			PackStart(_tokenEntry);
			PackStart(_pathDescriptionLabel);
			PackStart(_pathBox);
		}

		void AttachEvents()
		{
			_browseButton.Clicked += OnBrowse;
		}

		void LoadData()
		{
			var settings = _settingsService.Load();
			_tokenEntry.Text = settings.Token;
			_pathEntry.Text = settings.SamplesPath;
		}

		public void ApplyChanges()
		{
			Models.Settings settings = new Models.Settings
			{
				Token = _tokenEntry.Text,
				SamplesPath = _pathEntry.Text
			};

			_settingsService.Save(settings);
		}

		void OnBrowse(object sender, EventArgs args)
		{
			using (SelectFolderDialog folderSelect = new SelectFolderDialog("Browse Folder"))
			{
				folderSelect.Multiselect = false;
				folderSelect.CanCreateFolders = true;

				if (folderSelect.Run(MessageDialog.RootWindow))
				{
					_pathEntry.Text = folderSelect.Folder;
				}
			}
		}
	}
}