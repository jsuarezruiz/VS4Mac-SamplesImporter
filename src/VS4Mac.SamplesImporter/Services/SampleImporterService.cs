using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MonoDevelop.Core;
using Octokit;
using VS4Mac.SamplesImporter.Models;

namespace VS4Mac.SamplesImporter.Services
{
	public class SampleImporterService
    {
		GitHubClient _client;

		public void Init(string token)
		{
			Token = token;

			if (string.IsNullOrEmpty(Token))
				throw new Exception("It is necessary to add a GitHub Personal Access Token in the Preferences of Visual Studio.");

			_client = new GitHubClient(new ProductHeaderValue("vs4mac-samples-importer"))
			{
				Credentials = new Credentials(Token)
			};
		}

		public string Token { get; private set; }

		public List<SampleRepository> GetSampleRepositories()
		{
			return new List<SampleRepository> {
				new SampleRepository { SampleOwnerId = "xamarin", SampleRepositoryId = "monodroid-samples", Name = "Xamarin.Android", Platform = "Android" },
				new SampleRepository { SampleOwnerId = "xamarin", SampleRepositoryId = "ios-samples", Name = "Xamarin.iOS", Platform = "iOS" },
				new SampleRepository { SampleOwnerId = "xamarin", SampleRepositoryId = "xamarin-forms-samples", Name = "Xamarin.Forms", Platform = "Xamarin.Forms" },
				new SampleRepository { SampleOwnerId = "xamarin", SampleRepositoryId = "mac-samples", Name = "Xamarin.Mac", Platform = "macOS" },
				new SampleRepository { SampleOwnerId = "jsuarezruiz", SampleRepositoryId = "SamplesImporter-Community", Name = "Community", Platform = "Android, iOS, Xamarin.Forms, macOS" }
			};
		}

		public async Task<List<Sample>> GetSamplesAsync(
			string sampleOwnerId,
			string sampleRepositoryId, 
			CancellationToken cancellationToken)
		{
			if (string.IsNullOrEmpty(Token))
				throw new InvalidOperationException("You MUST call SampleImporterService.Init (Token); prior to using it.");

			List<Sample> samples = new List<Sample>();

			try
			{
				var repository = await _client.Repository.Get(sampleOwnerId, sampleRepositoryId);

				var folders = await _client.Repository.Content.GetAllContents(repository.Id);

				foreach (var folder in folders.Where(x => x.Type == ContentType.Dir))
				{
					if (cancellationToken.IsCancellationRequested)
					{
						break;
					}

					var files = await _client.Repository.Content.GetAllContents(repository.Id, folder.Path);

					foreach (var file in files.Where(x => x.Type == ContentType.File))
					{
						if (cancellationToken.IsCancellationRequested)
						{
							break;
						}

						if (file.Name.Equals("Metadata.xml", StringComparison.CurrentCulture))
						{
							var sample = new Sample
							{
								RepositoryId = repository.Id,
								Folder = folder.Path,
								Path = file.Path,
								Name = folder.Name,
								Url = folder.Url
							};

							samples.Add(sample);
						}
					}
				}
			}
			catch(Exception ex)
			{
				LoggingService.LogError("Getting samples failed.", ex);
			}

			return samples;
        }

		public async Task<Sample> GetSampleDetailsAsync(Sample sample)
		{
			if (string.IsNullOrEmpty(Token))
				throw new InvalidOperationException("You MUST call SampleImporterService.Init (Token); prior to using it.");

			var fileContents = await _client.Repository.Content.GetAllContents(sample.RepositoryId, sample.Path);
			var fileContent = fileContents.FirstOrDefault();

			if (fileContent != null)
			{
				var sampleDetails = Sample.FromXml(fileContent.Content);

				return sampleDetails;
			}

			return null;
		}

		public async Task<List<string>> GetSamplePicturesAsync(Sample sample)
		{
			if (string.IsNullOrEmpty(Token))
				throw new InvalidOperationException("You MUST call SampleImporterService.Init (Token); prior to using it.");

			List<string> pictures = new List<string>();

			try
			{
				var urls = await _client.Repository.Content.GetAllContents(sample.RepositoryId, Path.Combine(sample.Folder, "Screenshots"));

				foreach (var url in urls)
				{
					pictures.Add(url.DownloadUrl);
				}
			}
			catch(Exception ex)
			{
				LoggingService.LogError("Getting sample pictures failed.", ex);
			}

			return pictures;
		}

		public async Task<List<RepositoryContent>> GetSampleContentAsync(Sample sample)
		{
			List<RepositoryContent> content = new List<RepositoryContent>();

			var items = await _client.Repository.Content.GetAllContents(sample.RepositoryId, sample.Folder);

			foreach(var item in items)
			{
				if (item.Type == ContentType.File)
					content.Add(item);
				if (item.Type == ContentType.Dir)
				{
					content.Add(item);

					await AddChildItemsAsync(content, sample.RepositoryId, item.Path);
				}
			}

			return content;
		}

		internal async Task AddChildItemsAsync(List<RepositoryContent> content, long repositoryId, string path)
		{
			var items = await _client.Repository.Content.GetAllContents(repositoryId, path);

			foreach (var item in items)
			{
				if (item.Type == ContentType.File)
					content.Add(item);
				if (item.Type == ContentType.Dir)
				{
					content.Add(item);

					await AddChildItemsAsync(content, repositoryId, item.Path);
				}
			}
		}
	}
}