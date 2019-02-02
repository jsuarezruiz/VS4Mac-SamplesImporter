using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace VS4Mac.SamplesImporter.Services
{
	public class DownloaderService
	{
		public DownloaderService()
		{
			ServicePointManager.SecurityProtocol |= (SecurityProtocolType)3072;
		}

		public void Init(string token)
		{
			Token = token;

			if (string.IsNullOrEmpty(Token))
				throw new Exception("It is necessary to add a GitHub Personal Access Token in the Preferences of Visual Studio.");
		}

		public string Token { get; private set; }

		public async Task DownloadFileAsync(
		  string url,
		  string filePath,
		  CancellationToken cancellationToken)
		{
			if (string.IsNullOrWhiteSpace(url))
			{
				return;
			}

			using (var httpClient = new HttpClient())
			{
				using (var request = new HttpRequestMessage(HttpMethod.Get, url))
				{
					request.Headers.Add("Authorization", string.Format("Token {0}", Token));

					using (
						Stream contentStream = await (await httpClient.SendAsync(request)).Content.ReadAsStreamAsync(),
						stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 1024, true))
					{
						await contentStream.CopyToAsync(stream);
					}
				}
			}
		}
	}
}