using System.Collections.Generic;

namespace Selenite.Models
{
    public class ManifestCollection
    {
        public string ActiveManifest { get; set; }
        public IList<Manifest> Manifests { get; set; } 
    }
}