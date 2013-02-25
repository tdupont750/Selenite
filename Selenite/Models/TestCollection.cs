using System.Collections.Generic;
using Newtonsoft.Json;

namespace Selenite.Models
{
    public class TestCollection
    {
        [JsonIgnore]
        public string File { get; set; }
        public bool Enabled { get; set; }
        
        public string DefaultDomain { get; set; }
        public IList<Test> Tests { get; set; }
    }
}