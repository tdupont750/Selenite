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

        private Lazy<ManifestCollection> _manifestCollection;

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

        public void ReloadManifest()
        {
            _manifestCollection = new Lazy<ManifestCollection>(LoadManifestCollection);
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

        public void SetActiveManifest(string manifestName)
        {
            _manifestCollection.Value.ActiveManifest = manifestName;
            SaveManifestCollection();
        }

        public void SetActiveManifestDomain(string domainOverride)
        {
            var manifest = _manifestCollection.Value.Manifests
                .FirstOrDefault(m => m.Name == _manifestCollection.Value.ActiveManifest);

            if (manifest == null) return;

            manifest.OverrideDomain = domainOverride;

            SaveManifest(manifest);
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

            SaveManifestCollection();
        }

        public void SaveManifestCollection()
        {
            var manifestsJson = JsonConvert.SerializeObject(_manifestCollection.Value, Formatting.Indented);
            _fileService.WriteAllText(ManifestPath, manifestsJson);
        }
    }
}