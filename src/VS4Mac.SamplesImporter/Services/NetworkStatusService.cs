using System.Net.NetworkInformation;

namespace VS4Mac.SamplesImporter.Services
{
	public class NetworkStatusService
	{
		public bool IsConnectedToInternet(int timeoutPerHostMillis = 1000)
		{
			try
			{
				var networkAvailable = NetworkInterface.GetIsNetworkAvailable();

				if (!networkAvailable)
					return false;

				var host = "www.microsoft.com";

				using (var ping = new Ping())
				{
					var pingReply = ping.Send(host, timeoutPerHostMillis);

					if (pingReply != null && pingReply.Status == IPStatus.Success)
						return true;
				}

				return false;
			}
			catch
			{
				return false;
			}
		}
	}
}
