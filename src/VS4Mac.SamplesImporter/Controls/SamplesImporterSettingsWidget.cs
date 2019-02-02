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

		SettingsService _settingsService;

		public SamplesImporterSettingsWidget()
		{
			Init();
			BuildGui();
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
		}

		void BuildGui()
		{
			_linkBox.PackStart(_tokenDescriptionLabel);
			_linkBox.PackStart(_createTokenLink);

			PackStart(_linkBox);
			PackStart(_tokenLabel);
			PackStart(_tokenEntry);
		}

		void LoadData()
		{
			var settings = _settingsService.Load();
			_tokenEntry.Text = settings.Token;
		}

		public void ApplyChanges()
		{
			Models.Settings settings = new Models.Settings
			{
				Token = _tokenEntry.Text
			};

			_settingsService.Save(settings);
		}
	}
}