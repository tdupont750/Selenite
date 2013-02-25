using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Selenite.Services.Implementation
{
    public class ConfigurationService : IConfigurationService
    {
        private const int MaxSearchDepth = 5;
        private const string ErrorMessageFormat = "Unable to locate {0} folder, please specify {1} in the ApplicationSettings";
        
        public string ChromeDriverPath
        {
            get { return GetDriverPath("ChromeDriverPath", "ChromeDriver"); }
        }

        public string IEDriverPath
        {
            get { return GetDriverPath("InternetExplorerDriverPath", "IEDriver"); }
        }

        public string TestScriptsPath
        {
            get { return GetTestScriptsPath(); }
        }

        public string ManifestFileName
        {
            get { return ".manifests.json"; }
        }

        private static string Get(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        private string GetDriverPath(string key, string driverName)
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

            for (var i = 0; i <= MaxSearchDepth; i++)
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

        private string GetTestScriptsPath()
        {
            var driverPath = Get("TestScriptsPath");

            if (!String.IsNullOrWhiteSpace(driverPath))
                return driverPath;

            if (FindTestScriptsPath(out driverPath))
                return driverPath;

            var message = String.Format(ErrorMessageFormat, "TestScripts", "TestScriptsPath");
            throw new ConfigurationErrorsException(message);
        }

        public bool FindTestScriptsPath(out string testScriptsPath)
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            for (var i = 0; i <= MaxSearchDepth; i++)
            {
                currentDirectory = Path.Combine(currentDirectory, "../");
                currentDirectory = Path.GetFullPath(currentDirectory);

                if (FindTestScriptsSubPath(currentDirectory, out testScriptsPath))
                    return true;
            }

            testScriptsPath = String.Empty;
            return false;
        }

        private bool FindTestScriptsSubPath(string currentDirectory, out string testScriptsPath)
        {
            var directories = Directory.GetDirectories(currentDirectory);
            var subPath = Path.Combine(currentDirectory, "TestScripts");

            if (directories.Contains(subPath))
            {
                var file = Directory.GetFiles(subPath, ManifestFileName).FirstOrDefault();

                if (!String.IsNullOrWhiteSpace(file))
                {
                    testScriptsPath = subPath;
                    return true;
                }
            }

            testScriptsPath = String.Empty;
            return false;
        }
    }
}