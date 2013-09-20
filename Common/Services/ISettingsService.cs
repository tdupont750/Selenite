using Common.Models;

namespace Common.Services
{
    public interface ISettingsService
    {
        string GetEnabledBrowsers();
        void SetEnabledBrowsers(string enabledBrowsers);

        Accent GetAccent();
        void SetAccent(Accent accent);

        Theme GetTheme();
        void SetTheme(Theme theme);

        void UpdateTheme();
    }
}