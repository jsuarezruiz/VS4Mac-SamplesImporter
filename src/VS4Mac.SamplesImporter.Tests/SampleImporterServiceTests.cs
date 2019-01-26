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
        public void GetSamplesTests()
        {
            var samples = _service.GetSamples();

            Assert.AreNotEqual(0, samples.Count);
        }
    }
}