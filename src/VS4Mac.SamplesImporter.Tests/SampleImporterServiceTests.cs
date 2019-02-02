using NUnit.Framework;
using VS4Mac.SamplesImporter.Services;

namespace VS4Mac.SamplesImporter.Tests
{
	[TestFixture]
    public class SampleImporterServiceTests
    {
        SampleImporterService _service;

        [SetUp]
        public void SetUp()
        {
            _service = new SampleImporterService();
        }

		[Test]
		public void GetSampleRepositoriesTests()
		{
			var repositories = _service.GetSampleRepositories();

			Assert.AreNotEqual(0, repositories.Count);
		}
    }
}