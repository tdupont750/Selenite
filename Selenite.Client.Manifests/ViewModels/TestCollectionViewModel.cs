using System.Windows.Input;
using Common.Extensions;
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

                IsEnabledChangedCommand.VerifyAndExecute(null);
            }
        }

        public ICommand IsEnabledChangedCommand { get; set; }
    }
}
