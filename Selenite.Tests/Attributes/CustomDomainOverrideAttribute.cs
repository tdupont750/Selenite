using System.IO;

namespace Selenite.Tests
{
    public class CurrentDirectoryDomainOverrideAttribute : SeleniteDomainOverrideAttribute
    {
        public override string DomainOverride
        {
            get
            {
                var current = Directory.GetCurrentDirectory();
                return Path.Combine(current, "../../TestPages/");
            }
        }
    }
}
