namespace Selenite.Services
{
    public interface IConfigurationService
    {
        string ChromeDriverPath { get; }
        string InternetExplorerDriverPath { get; }
        string TestsPath { get; }
        string ActiveManifest { get; }
    }
}