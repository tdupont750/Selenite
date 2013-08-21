using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Selenite.Models;

namespace Selenite.Services.Implementation
{
    public class ConfigurationService : IConfigurationService
    {
        private const int MaxSearchDepth = 5;
        private const string ErrorMessageFormat = "Unable to locate {0}, please specify {1} in the ApplicationSettings";

        #region TestScriptsPath

        public string TestScriptsPath
        {
            get { return GetManifestPath(); }
            set { SetManifestPath(value); }
        }

        public ManifestInfo ActiveManifestInfo
        {
            get
            {
                var manifests = GetManifestInfoCollection();
                return manifests.Manifests[manifests.ActiveManifest];
            }
            set
            {
                var manifests = GetManifestInfoCollection();
                manifests.Manifests[manifests.ActiveManifest] = value;
                SaveManifestInfoCollection(manifests);
            }
        }

        private ManifestInfoCollection GetManifestInfoCollection()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var manifestPath = Path.Combine(currentDirectory, ManifestFileName);

            if (!File.Exists(manifestPath))
            {
                return null;
            }

            var manifests = JsonConvert.DeserializeObject<ManifestInfoCollection>(File.ReadAllText(manifestPath));

            if (manifests == null || !manifests.Manifests.Any() || string.IsNullOrEmpty(manifests.ActiveManifest))
            {
                return null;
            }

            return manifests;
        }

        private void SaveManifestInfoCollection(ManifestInfoCollection manifests)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var manifestPath = Path.Combine(currentDirectory, ManifestFileName);

            using (var sw = new StreamWriter(manifestPath, false))
            {
                sw.WriteLine(JsonConvert.SerializeObject(manifests, Formatting.Indented));
            }
        }

        private string GetManifestPath()
        {
            var manifests = GetManifestInfoCollection();

            if (manifests == null)
            {
                return DefaultManifestPath;
            }

            return manifests.ActiveManifest;
        }

        private string DefaultManifestPath
        {
            get
            {
                var path = GetPath("TestScripts", "TestScriptsPath", FindTestScriptsPath);
                SetManifestPath(path);
                return path;
            }
        }

        private void SetManifestPath(string path)
        {
            var manifests = GetManifestInfoCollection();

            if (manifests == null)
            {
                var manifestInfo = new ManifestInfo();

                manifests = new ManifestInfoCollection
                    {
                        ActiveManifest = path,
                        Manifests = new Dictionary<string, ManifestInfo> { { path, manifestInfo } },
                    };
            }
            else
            {
                if (manifests.Manifests.ContainsKey(path))
                {
                    manifests.ActiveManifest = path;
                }
                else
                {
                    var manifestInfo = new ManifestInfo();

                    manifests.Manifests.Add(path, manifestInfo);
                    manifests.ActiveManifest = path;
                }
            }

            SaveManifestInfoCollection(manifests);
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