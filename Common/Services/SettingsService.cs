using System;
using System.Collections;
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

            // SO HACKY: Reload the Colours.xaml THEN reset the theme. This is necessary since some colors
            // get overridden in weird orders and I feel dirty for even figuring this out.
            // Essentially, some colors in the Colours.xaml are dynamic and based on definitions in the
            // accent files. Some are hard coded. This means that Colours has to be reloaded AFTER the
            // theme has changed, so that the dynamic colors get set correctly, but other colors get
            // overwritten so then the theme has to be set AGAIN so that the hard coded colors in
            // Colours.xaml don't get used. On top of that, because of the way some of the colors are set
            // some of the brushes need to be overridden (again) in the shells or they won't be picked up
            // until the app restarts.
            // If you followed all that you probably need a shower.
            var colorsResourceDictionary = new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Colours.xaml")
                };

            var appResources = Application.Current.Resources;

            foreach (DictionaryEntry entry in colorsResourceDictionary)
            {
                if (appResources.Contains(entry.Key))
                {
                    appResources.Remove(entry.Key);
                }

                appResources.Add(entry.Key, entry.Value);
            }

            ThemeManager.ChangeTheme(Application.Current, accent, theme);
        }
    }
}
