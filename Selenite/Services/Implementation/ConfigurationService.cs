using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Selenite.Services.Implementation
{
    public class ConfigurationService : IConfigurationService
    {
        public string ChromeDriverPath
        {
            get { return Get("ChromeDriverPath", "ChromeDriver"); }
        }

        public string InternetExplorerDriverPath
        {
            get { return Get("InternetExplorerDriverPath", "IEDriver"); }
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

        private string Get(string key, string driverName)
        {
            var driverPath = Get(key);

            if (!String.IsNullOrWhiteSpace(driverPath))
                return driverPath;

            if (FindDriverPath(driverName, out driverPath))
                return driverPath;

            var message = String.Format("Unable to locate {0}, please specify {1} in the ApplicationSettings", driverName, key);
            throw new ConfigurationErrorsException(message);
        }

        public bool FindDriverPath(string driverName, out string driverPath)
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            for (var i = 0; i <= 5; i++)
            {
                currentDirectory = Path.Combine(currentDirectory, "../");
                currentDirectory = Path.GetFullPath(currentDirectory);

                if (FindDriverSubPath(currentDirectory, driverName, out driverPath))
                    return true;
            }

            driverPath = String.Empty;
            return false;
        }

        private bool FindDriverSubPath(string currentDirectory, string driverName, out string driverPath)
        {
            var directories = Directory.GetDirectories(currentDirectory);
            var subPath = Path.Combine(currentDirectory, "packages");

            if (directories.Contains(subPath))
            {
                directories = Directory.GetDirectories(subPath);

                var prefixPath = Path.Combine(subPath, "WebDriver." + driverName);
                subPath = directories.FirstOrDefault(c => c.StartsWith(prefixPath, StringComparison.InvariantCultureIgnoreCase));

                if (!String.IsNullOrWhiteSpace(subPath))
                {
                    subPath = Path.Combine(subPath, "tools");

                    if (Directory.Exists(subPath))
                    {
                        driverPath = subPath;
                        return true;
                    }
                }
            }

            driverPath = String.Empty;
            return false;
        }
    }
}