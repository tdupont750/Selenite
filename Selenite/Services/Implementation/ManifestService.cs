using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Selenite.Models;

namespace Selenite.Services.Implementation
{
    public class ManifestService : IManifestService
    {
        private const string ManifestExtension = "manifest";

        private readonly IConfigurationService _configurationService;
        private readonly IFileService _fileService;

        public ManifestService(IConfigurationService configurationService, IFileService fileService)
        {
            _configurationService = configurationService;
            _fileService = fileService;
        }

        public IList<string> GetManifestNames()
        {
            var pathRoot = Path.GetFullPath(_configurationService.TestsPath);

            return _fileService
                .GetFiles(pathRoot, "*." + ManifestExtension)
                .Select(Path.GetFileNameWithoutExtension)
                .ToList();
        }

        public Manifest GetManifest(string manifestName)
        {
            var pathRoot = Path.GetFullPath(_configurationService.TestsPath);
            var path = Path.Combine(pathRoot, manifestName + "." + ManifestExtension);

            var manifestJson = _fileService.ReadAllText(path);
            return JsonConvert.DeserializeObject<Manifest>(manifestJson);
        }

        public void SaveManifest(Manifest manifest)
        {
            var pathRoot = Path.GetFullPath(_configurationService.TestsPath);
            var path = Path.Combine(pathRoot, manifest.Name + "." + ManifestExtension);

            var manifestJson = JsonConvert.SerializeObject(manifest);
            _fileService.WriteAllText(path, manifestJson);
        }
    }
}