using System.Windows.Input;
using Common.ViewModels;

namespace Selenite.Client.Manifests.ViewModels
{
    public class TestCollectionViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public bool IsEnabled
        {
            get { return Get(() => IsEnabled); }
            set
            {
                Set(value, () => IsEnabled);

                if (IsEnabledChangedCommand != null && IsEnabledChangedCommand.CanExecute(null))
                {
                    IsEnabledChangedCommand.Execute(null);
                }
            }
        }

        public ICommand IsEnabledChangedCommand { get; set; }
    }
}
