using System.Collections.Generic;
using Newtonsoft.Json;
using Selenite.Commands;

namespace Selenite.Models
{
    public class Test
    {
        [JsonIgnore]
        public string CollectionName { get; set; }

        public bool Enabled { get; set; }
        public string Name { get; set; }
        
        public string Url { get; set; }
        public IList<ICommand> Commands { get; set; }

        [JsonIgnore]
        public string DomainUrl { get; set; }
    }
}