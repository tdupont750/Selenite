namespace Selenite.Services
{
    public interface IConfigurationService
    {
        string ChromeDriverPath { get; }
        string IEDriverPath { get; }
        string PhantomJsPath { get; }
        string TestScriptsPath { get; set; }
        string ManifestFileName { get; }
    }
}