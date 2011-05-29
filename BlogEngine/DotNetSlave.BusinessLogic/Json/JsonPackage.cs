using System;

namespace BlogEngine.Core.Json
{
    public class JsonPackage
    {
        public string PackageType { get; set; }

        public string Title { get; set; }

        public string Version { get; set; }

        public string Description { get; set; }

        public int DownloadCount { get; set; }

        public string LastUpdated { get; set; }

        public string Website { get; set; }

        public string PackageUrl { get; set; }

        public string IconUrl { get; set; }

        public string Authors { get; set; }
    }
}
