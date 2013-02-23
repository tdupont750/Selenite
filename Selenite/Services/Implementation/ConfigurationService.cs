using System.Configuration;

namespace Selenite.Services.Implementation
{
    public class ConfigurationService : IConfigurationService
    {
        public string DriversPath
        {
            get { return Get("DriversPath"); }
        }

        public string TestsPath
        {
            get { return Get("TestsPath"); }
        }

        public string ActiveManifest
        {
            get { return Get("ActiveManifest"); }
        }

        private static string Get(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}