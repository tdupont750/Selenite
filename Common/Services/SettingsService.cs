using System;
using System.Linq;
using System.Windows;
using MahApps.Metro;
using Accent = Common.Models.Accent;
using Theme = Common.Models.Theme;

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

        public Accent GetAccent()
        {
            var accentSetting = Settings.Default.Accent;
            Accent accent;

            if (!Enum.TryParse(accentSetting, out accent))
            {
                accent = Accent.Blue;
            }

            return accent;
        }

        public void SetAccent(Accent accent)
        {
            Settings.Default.Accent = accent.ToString();
            Settings.Default.Save();
        }

        public Theme GetTheme()
        {
            var themeSetting = Settings.Default.Theme;
            Theme theme;

            if (!Enum.TryParse(themeSetting, out theme))
            {
                theme = Theme.Dark;
            }

            return theme;
        }

        public void SetTheme(Theme theme)
        {
            Settings.Default.Theme = theme.ToString();
            Settings.Default.Save();
        }

        public void UpdateTheme()
        {
            var accent = ThemeManager.DefaultAccents.FirstOrDefault(a => a.Name == GetAccent().ToString());
            var theme = GetTheme() == Theme.Dark ? MahApps.Metro.Theme.Dark : MahApps.Metro.Theme.Light;

            ThemeManager.ChangeTheme(Application.Current, accent, theme);
        }
    }
}
