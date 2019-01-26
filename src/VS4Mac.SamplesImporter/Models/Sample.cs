using System.Collections.Generic;

namespace VS4Mac.SamplesImporter.Models
{
    public class Sample
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Screenshot { get; set; }
        public string Url { get; set; }
        public List<string> Platforms { get; set; }
        public List<string> Tags { get; set; }
    }
}