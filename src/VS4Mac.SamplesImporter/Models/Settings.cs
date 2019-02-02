namespace VS4Mac.SamplesImporter.Models
{
	public class Settings
	{
		public string Token { get; set; }

		public static Settings Default()
		{
			return new Settings
			{
				Token = string.Empty
			};
		}
	}
}