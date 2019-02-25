using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using MonoDevelop.Core;

namespace VS4Mac.SamplesImporter.Models
{
	[XmlRoot(ElementName = "SampleMetadata")]
	public class Sample
	{
		public Sample()
		{
			Pictures = new List<string>();
		}

		[XmlElement(ElementName = "ID")]
		public string Id { get; set; }
		[XmlElement(ElementName = "IsFullApplication")]
		public string IsFullApplication { get; set; }
		[XmlElement(ElementName = "Level")]
		public string Level { get; set; }
		[XmlElement(ElementName = "Tags")]
		public string Tags { get; set; }
		[XmlElement(ElementName = "SupportedPlatforms")]
		public string SupportedPlatforms { get; set; }
		[XmlElement(ElementName = "LicenseRequirement")]
		public string LicenseRequirement { get; set; }
		[XmlElement(ElementName = "Gallery")]
		public string Gallery { get; set; }
		[XmlElement(ElementName = "Brief")]
		public string Brief { get; set; }
		[XmlElement(ElementName = "DownloadUrl")]
		public string DownloadUrl { get; set; }
		[XmlElement(ElementName = "Author")]
		public string Author { get; set; }

		public long RepositoryId { get; set; }
		public string Folder { get; set; }
		public string Path { get; set; }
		public string Name { get; set; }
		public string Url { get; set; }
		public List<string> Pictures { get; set; }

		public static Sample FromXml(string xmlContent)
		{
			Sample sample = null;

			try
			{
				XmlSerializer serializer = new XmlSerializer(typeof(Sample));

				using (XmlReader reader = XmlReader.Create(new StringReader(xmlContent)))
				{
					sample = (Sample)serializer.Deserialize(reader);
				}
			}
			catch(Exception ex)
			{
				LoggingService.LogError("Parse the sample failed.", ex);
			}

			return sample;
		}
	}
}