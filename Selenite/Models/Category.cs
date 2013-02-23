using System.Collections.Generic;
using Newtonsoft.Json;

namespace Selenite.Models
{
    public class Category
    {
        [JsonIgnore]
        public string Name { get; set; }
        public bool Enabled { get; set; }
        
        public string Domain { get; set; }
        public IList<Test> Tests { get; set; }
    }
}