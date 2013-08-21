using System.Collections.Generic;

namespace Selenite.Models
{
    public class ManifestInfoCollection
    {
        public string ActiveManifest { get; set; }
        public Dictionary<string, ManifestInfo> Manifests { get; set; } 
    }
}
