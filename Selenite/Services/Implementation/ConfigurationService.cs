using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Selenite.Services.Implementation
{
    public class ConfigurationService : IConfigurationService
    {
        private const int MaxSearchDepth = 5;
        private const string ErrorMessageFormat = "Unable to locate {0}, please specify {1} in the ApplicationSettings";

        #region TestScriptsPath

        public string TestScriptsPath
        {
            get
            {
                return GetPath("TestScripts", "TestScriptsPath", FindTestScriptsPath);
            }
            set
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings.Remove("TestScriptsPath");
                config.AppSettings.Settings.Add("TestScriptsPath", value);
                config.Save(ConfigurationSaveMode.Modified, true);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }

        public string ManifestFileName
        {
            get { return ".manifests.json"; }
        }

        private string FindTestScriptsPath(string currentDirectory, dynamic args)
        {
            var directories = Directory.GetDirectories(currentDirectory);
            var subPath = Path.Combine(currentDirectory, "TestScripts");

            if (directories.Contains(subPath))
            {
                var file = Directory.GetFiles(subPath, ManifestFileName).FirstOrDefault();

                if (!String.IsNullOrWhiteSpace(file))
                    return subPath;
            }

            return String.Empty;
        }

        #endregion

        #region Helpers

        protected virtual string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        private string GetPath(string name, string key, Func<string, dynamic, string> subPathFunc, dynamic args = null)
        {
            var path = GetAppSetting(key);

            var resolvedPath = ResolvePath(path, subPathFunc, args);

            if (string.IsNullOrWhiteSpace(resolvedPath))
            {
                var message = String.Format(ErrorMessageFormat, name, key);
                throw new ConfigurationErrorsException(message);
            }

            return resolvedPath;
        }

        private string ResolvePath(string path, Func<string, dynamic, string> subPathFunc, dynamic args = null)
        {
            if (!String.IsNullOrWhiteSpace(path))
                return path;

            path = FindPath(subPathFunc, args);
            if (!String.IsNullOrWhiteSpace(path))
                return path;

            return null;
        }

        private string FindPath(Func<string, dynamic, string> subPathFunc, dynamic args)
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            var path = subPathFunc(currentDirectory, args);

            if (!string.IsNullOrEmpty(path))
            {
                return path;
            }

            for (var i = 0; i <= MaxSearchDepth; i++)
            {
                currentDirectory = Path.Combine(currentDirectory, "../");
                currentDirectory = Path.GetFullPath(currentDirectory);

                var subPath = subPathFunc(currentDirectory, args);
                if (!String.IsNullOrWhiteSpace(subPath))
                    return subPath;
            }

            return String.Empty;
        }
        #endregion
    }
}