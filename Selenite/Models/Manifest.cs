using System.Collections.Generic;
using Selenite.Enums;

namespace Selenite.Models
{
    public class Manifest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string OverrideDomain { get; set; }
        public IList<DriverType> DriverTypes { get; set; }
        public IList<string> Files { get; set; }
        public IDictionary<string, string> Metadata { get; set; }
    }
}