using System.Collections.Generic;
using System.Windows.Input;
using Common.Extensions;
using Common.Models;
using Common.ViewModels;

namespace Selenite.Client.Settings.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public IList<Accent> Accents { get; set; }
        public Accent SelectedAccent
        {
            get { return Get(() => SelectedAccent); }
            set
            {
                Set(value, () => SelectedAccent);
                SelectionChangedCommand.VerifyAndExecute(value);
            }
        }

        public IList<Theme> Themes { get; set; }
        public Theme SelectedTheme
        {
            get { return Get(() => SelectedTheme); }
            set
            {
                Set(value, () => SelectedTheme);
                SelectionChangedCommand.VerifyAndExecute(value);
            }
        }

        public bool IsDirty
        {
            get { return Get(() => IsDirty); }
            set
            {
                Set(value, () => IsDirty);
                ApplyChangesCommand.RaiseCanExecuteChanged();
                SaveChangesCommand.RaiseCanExecuteChanged();
            }
        }

        public ICommand ApplyChangesCommand { get; set; }
        public ICommand SaveChangesCommand { get; set; }
        public ICommand CloseWindowCommand { get; set; }

        public ICommand SelectionChangedCommand { get; set; }
    }
}
