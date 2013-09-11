using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Selenite.Enums;

namespace Selenite.Models
{
    public class TestCollection
    {
        [JsonIgnore]
        public string File { get; set; }

        [JsonIgnore]
        public string ResolvedFile { get; set; }

        public string Description { get; set; }

        [DefaultValue(true)]
        public bool Enabled { get; set; }

        public string DefaultDomain { get; set; }

        public string SetupStepsFile { get; set; }

        public IList<DriverType> DriverTypes { get; set; }

        public IList<SeleniteTest> SetupSteps { get; set; }
        
        public IList<SeleniteTest> Tests { get; set; }
        
        public IDictionary<string, string> Macros { get; set; }
    }
}