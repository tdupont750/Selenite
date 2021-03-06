using Selenite.Enums;
using Selenite.Models;

namespace Selenite.Services
{
    public interface IConfigurationService
    {
        string TestScriptsPath { get; set; }
        string ManifestFileName { get; }
        ManifestInfo ActiveManifestInfo { get; set; }
        string GetDriverPath(DriverType driverType);
    }
}