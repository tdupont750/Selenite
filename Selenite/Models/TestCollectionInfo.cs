using System.Collections.Generic;

namespace Selenite.Models
{
    public class TestCollectionInfo
    {
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public IList<string> DisabledTests { get; set; } 
    }
}
