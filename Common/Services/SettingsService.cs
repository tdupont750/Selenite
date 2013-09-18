namespace Common.Services
{
    public class SettingsService : ISettingsService
    {
        public string GetEnabledBrowsers()
        {
            return Settings.Default.EnabledBrowsers;
        }

        public void SetEnabledBrowsers(string enabledBrowsers)
        {
            Settings.Default.EnabledBrowsers = enabledBrowsers;
            Settings.Default.Save();
        }
    }
}
