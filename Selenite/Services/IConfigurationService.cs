namespace Selenite.Services
{
    public interface IConfigurationService
    {
        string DriversPath { get; }
        string TestsPath { get; }
        string ActiveManifest { get; }
    }
}