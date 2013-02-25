using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Selenite.Models;

namespace Selenite.Services.Implementation
{
    public class ManifestService : IManifestService
    {
        private readonly IConfigurationService _configurationService;
        private readonly IFileService _fileService;

        private readonly Lazy<ManifestCollection> _manifestCollection;

        public ManifestService(IConfigurationService configurationService, IFileService fileService)
        {
            _configurationService = configurationService;
            _fileService = fileService;

            _manifestCollection = new Lazy<ManifestCollection>(LoadManifestCollection);
        }
        
        private string ManifestPath
        {
            get { return Path.Combine(_configurationService.TestScriptsPath, _configurationService.ManifestFileName); }
        }

        private ManifestCollection LoadManifestCollection()
        {
            var manifestsJson = _fileService.ReadAllText(ManifestPath);
            return JsonConvert.DeserializeObject<ManifestCollection>(manifestsJson);
        }

        public IList<string> GetManifestNames()
        {
            return _manifestCollection.Value.Manifests
                .Select(m => m.Name)
                .ToList();
        }

        public string GetActiveManifestName()
        {
            return _manifestCollection.Value.ActiveManifest;
        }

        public Manifest GetManifest(string manifestName)
        {
            return _manifestCollection.Value.Manifests.FirstOrDefault(m => m.Name == manifestName);
        }

        public void SaveManifest(Manifest manifest)
        {
            var oldManifest = GetManifest(manifest.Name);

            if (oldManifest == null)
                _manifestCollection.Value.Manifests.Add(manifest);
            else
            {
                var i = _manifestCollection.Value.Manifests.IndexOf(oldManifest);
                _manifestCollection.Value.Manifests[i] = manifest;
            }

            var manifestsJson = JsonConvert.SerializeObject(_manifestCollection.Value);
            _fileService.WriteAllText(ManifestPath, manifestsJson);
        }
    }
}