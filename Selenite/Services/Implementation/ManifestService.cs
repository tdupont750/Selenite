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
            var manifestInfo = _configurationService.ActiveManifestInfo;

            return manifestInfo == null || string.IsNullOrEmpty(manifestInfo.ActiveManifest)
                       ? _manifestCollection.Value.ActiveManifest
                       : manifestInfo.ActiveManifest;
        }

        public void SetActiveManifest(string manifestName)
        {
            _manifestCollection.Value.ActiveManifest = manifestName;
            SaveManifestInfo();
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
            var manifest = _manifestCollection.Value.Manifests.FirstOrDefault(m => m.Name == manifestName);

            if (manifest == null)
                return null;

            var manifestInfo = _configurationService.ActiveManifestInfo;
            if (manifestInfo != null && manifestInfo.ManifestDomainOverride != null && manifestInfo.ManifestDomainOverride.ContainsKey(manifestName))
            {
                manifest.OverrideDomain = manifestInfo.ManifestDomainOverride[manifestName];
            }

            return manifest;
        }

        public void SaveManifest(Manifest manifest)
        {
            var oldManifest = _manifestCollection.Value.Manifests.FirstOrDefault(m => m.Name == manifest.Name);

            if (oldManifest == null)
                _manifestCollection.Value.Manifests.Add(manifest);
            else
            {
                var i = _manifestCollection.Value.Manifests.IndexOf(oldManifest);
                _manifestCollection.Value.Manifests[i] = manifest;
            }

            SaveManifestInfo();
        }

        public void SaveManifestInfo()
        {
            var manifestInfo = _configurationService.ActiveManifestInfo;
            manifestInfo.ActiveManifest = _manifestCollection.Value.ActiveManifest;
            manifestInfo.ManifestDomainOverride = _manifestCollection.Value.Manifests
                .ToDictionary(manifest => manifest.Name, manifest => manifest.OverrideDomain);

            _configurationService.ActiveManifestInfo = manifestInfo;
        }
    }
}