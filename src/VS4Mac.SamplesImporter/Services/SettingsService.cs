using System.IO;
using System.Xml.Linq;
using MonoDevelop.Core;
using VS4Mac.SamplesImporter.Helpers;

namespace VS4Mac.SamplesImporter.Services
{
	public class SettingsService
	{
		const string ConfigName = "MonoDevelop.SamplesImporter.config";
		readonly string ConfigurationPath = UserProfile.Current.ConfigDir;

		public SettingsService()
		{
			ConfigurationPath = Path.Combine(ConfigurationPath, ConfigName);
		}

		public bool SettingsExists()
		{
			return File.Exists(ConfigurationPath);
		}

		public void Save(Models.Settings settings)
		{
			XDocument doc = new XDocument();
			doc.Add(new XElement("SamplesImporter"));
			doc.Root.Add(new XElement("Token", settings.Token));
			doc.Root.Add(new XElement("SamplesPath", settings.SamplesPath));
			doc.Save(ConfigurationPath);
		}

		public Models.Settings Load()
		{
			var settings = Models.Settings.Default();

			EnsureDocumentSettingsExists(settings);
			var document = XDocument.Load(ConfigurationPath);

			if (document.Root == null)
			{
				return settings;
			}

			var token = document.Root.Element("Token");

			if (token != null)
			{
				settings.Token = token.Value;
			}

			var samplesPath = document.Root.Element("SamplesPath");

			if (samplesPath != null)
			{
				settings.SamplesPath = samplesPath.Value;
			}

			return settings;
		}

		private void EnsureDocumentSettingsExists(Models.Settings settings)
		{
			if (SettingsExists() && XDocumentHelper.CanBeLoaded(ConfigurationPath))
				return;

			Save(settings);
		}
	}
}