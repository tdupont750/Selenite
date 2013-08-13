using System.Collections.Generic;

namespace Selenite.Models
{
    public class Manifest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string OverrideDomain { get; set; }
        public IList<string> Files { get; set; }
    }
}