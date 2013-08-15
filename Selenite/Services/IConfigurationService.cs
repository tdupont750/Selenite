namespace Selenite.Services
{
    public interface IConfigurationService
    {
        string TestScriptsPath { get; set; }
        string ManifestFileName { get; }
    }
}