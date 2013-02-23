using System.Collections.Generic;

namespace Selenite.Models
{
    public class Manifest
    {
        public string Name { get; set; }
        public string Domain { get; set; }
        public IList<string> Categories { get; set; }
    }
}