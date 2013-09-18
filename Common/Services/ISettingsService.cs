namespace Common.Services
{
    public interface ISettingsService
    {
        string GetEnabledBrowsers();
        void SetEnabledBrowsers(string enabledBrowsers);
    }
}