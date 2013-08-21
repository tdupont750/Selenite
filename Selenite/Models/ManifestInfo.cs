using System.Collections.Generic;

namespace Selenite.Models
{
    public class ManifestInfo
    {
        public string ActiveManifest { get; set; }
        public Dictionary<string, string> ManifestDomainOverride { get; set; }
        public IList<TestCollectionInfo> TestCollections { get; set; } 
    }
}
