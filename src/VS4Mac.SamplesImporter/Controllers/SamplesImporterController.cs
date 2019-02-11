using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using Octokit;
using VS4Mac.SamplesImporter.Controllers.Base;
using VS4Mac.SamplesImporter.Extensions;
using VS4Mac.SamplesImporter.Models;
using VS4Mac.SamplesImporter.Services;
using VS4Mac.SamplesImporter.Views;

namespace VS4Mac.SamplesImporter.Controllers
{
	public class SamplesImporterController : IController
    {
        readonly ISamplesImporterView _view;

        readonly SampleImporterService _sampleImporterService;
		readonly DownloaderService _downloaderService;
		readonly SettingsService _settingsService;
		readonly NetworkStatusService _networkStatusService;

		string _projectsPath;

        public SamplesImporterController(ISamplesImporterView view)
        {
            _view = view;

			_sampleImporterService = new SampleImporterService();
			_downloaderService = new DownloaderService();
			_settingsService = new SettingsService();
			_networkStatusService = new NetworkStatusService();

			Samples = new List<Sample>();
			_projectsPath = string.Empty;

			Init();

			view.SetController(this);
        }

		public List<Sample> Samples { get; set; }
		public Sample SelectedSample { get; set; }
		public SampleRepository SelectedSampleRepository { get; set; }

		public List<SampleRepository> LoadSampleRepositories()
		{
			return _sampleImporterService.GetSampleRepositories();
		}

		public async Task<List<Sample>> LoadSamplesAsync(string sampleOwnerId, string sampleRepositoryId, CancellationToken cancellationToken)
		{
			Samples.Clear();

			if (string.IsNullOrEmpty(_sampleImporterService.Token))
			{
				throw new Exception("It is necessary to add a GitHub Personal Access Token in the Preferences of Visual Studio.");
			}

			if(!_networkStatusService.IsConnectedToInternet())
			{
				throw new Exception("There is no Internet conection.");
			}

			var samples = await _sampleImporterService.GetSamplesAsync(sampleOwnerId, sampleRepositoryId, cancellationToken);

			Samples = samples;

			return samples;
		}

		public async Task UpdateSampleDetail()
		{
			var sampleDetails = await _sampleImporterService.GetSampleDetailsAsync(SelectedSample);

			SelectedSample.Id = sampleDetails.Id;
			SelectedSample.IsFullApplication = sampleDetails.IsFullApplication;
			SelectedSample.Level = sampleDetails.Level;
			SelectedSample.LicenseRequirement = sampleDetails.LicenseRequirement;
			SelectedSample.SupportedPlatforms = sampleDetails.SupportedPlatforms;
			SelectedSample.Tags = sampleDetails.Tags;
			SelectedSample.Brief = sampleDetails.Brief;
			SelectedSample.DownloadUrl = sampleDetails.DownloadUrl;
			SelectedSample.Author = sampleDetails.Author;

			await LoadSamplePicturesAsync();
		}

		public async Task LoadSamplePicturesAsync()
		{
			var pictures = await _sampleImporterService.GetSamplePicturesAsync(SelectedSample);
			SelectedSample.Pictures = pictures;
		}

		public Stream LoadPicture(string url)
		{
			try
			{
				WebClient wc = new WebClient();
				byte[] bytes = wc.DownloadData(url);
				MemoryStream ms = new MemoryStream(bytes);

				return ms;
			}
			catch
			{
				return null;
			}
		}

		public List<Sample> FilterData(string filter)
        {
			List<Sample> filteredSample = new List<Sample>();

			foreach(var sample in Samples)
			{
				if (sample.Name.Contains(filter, StringComparison.InvariantCultureIgnoreCase))
					filteredSample.Add(sample);

				if(!string.IsNullOrEmpty(sample.Tags) && sample.Tags.Split(',').Any(t => t.Contains(filter)))
					filteredSample.Add(sample);
			}

			return filteredSample;
        }

        public async Task<string> DownloadSampleAsync(ProgressMonitor progressMonitor = null)
        {
            try
            {
				string projectPath = string.Empty;

				if (!string.IsNullOrEmpty(SelectedSample.DownloadUrl))
				{
					progressMonitor?.BeginTask("Downloading...", 1);

					string zipProjectPath = Path.Combine(UserProfile.Current.TempDir, $"{SelectedSample.Name}.zip");
					projectPath = Path.Combine(UserProfile.Current.TempDir, SelectedSample.Name);

					if (!File.Exists(zipProjectPath))
					{
						await _sampleImporterService.DownloadSampleAsync(new Uri(SelectedSample.DownloadUrl), zipProjectPath);
					}

					progressMonitor?.Step();

					if (!Directory.Exists(projectPath))
					{
						Directory.CreateDirectory(projectPath);
						ZipFile.ExtractToDirectory(zipProjectPath, projectPath);
					}

					var solutions = Directory.GetFiles(projectPath, "*.sln", SearchOption.AllDirectories);
					var solution = solutions.FirstOrDefault();

					if (!string.IsNullOrEmpty(solution))
						projectPath = solution;
				}
				else
				{
					var content = await _sampleImporterService.GetSampleContentAsync(SelectedSample);

					progressMonitor?.BeginTask("Downloading...", content.Count);

					var folders = content.Where(c => c.Type == ContentType.Dir);

					foreach (var folder in folders)
					{
						string directoryPath = Path.Combine(_projectsPath, folder.Path);

						if (!Directory.Exists(directoryPath))
						{
							progressMonitor?.Step();
							progressMonitor?.Log.WriteLine("Creating folder" + ": " + directoryPath);
							Directory.CreateDirectory(directoryPath);
						}
					}

					var files = content.Where(c => c.Type == ContentType.File);

					foreach (var file in files)
					{
						string filePath = Path.Combine(_projectsPath, file.Path);
						progressMonitor?.Step();
						progressMonitor?.Log.WriteLine("Downloading file" + ": " + filePath);
						await _downloaderService.DownloadFileAsync(file.DownloadUrl, filePath, CancellationToken.None);
					}

					var projectFile = files.FirstOrDefault(c => c.Path.EndsWith(".sln", StringComparison.InvariantCultureIgnoreCase));

					if (projectFile != null)
					{
						projectPath = Path.Combine(_projectsPath, projectFile.Path);
					}
				}

				return projectPath;
            }
            catch(Exception ex)
            {
                LoggingService.LogError(ex.Message);

                return string.Empty;
            }
        }

        public async Task OpenSolutionAsync(string solutionPath)
        {
            if(!string.IsNullOrEmpty(solutionPath))
                await IdeApp.Workspace.OpenWorkspaceItem(new FilePath(solutionPath), true);
		}

		internal void Init()
		{
			var settings = _settingsService.Load();

			_sampleImporterService.Init(settings.Token);
			_downloaderService.Init(settings.Token);
			_projectsPath = settings.SamplesPath;
		}
	}
}