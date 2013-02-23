using System.Collections.ObjectModel;
using Selenite.Models;

namespace Selenite.Client.ViewModels.WebAutomation
{
    public class CategoryViewModel : ViewModelBase
    {
        public CategoryViewModel()
        {
            PageTests = new ObservableCollection<TestViewModel>();
        }

        public string Name
        {
            get { return Get(() => Name); }
            set { Set(value, () => Name); }
        }

        public string Domain
        {
            get { return Get(() => Domain); }
            set { Set(value, () => Domain); }
        }

        public bool IsEnabled
        {
            get { return Get(() => IsEnabled); }
            set { Set(value, () => IsEnabled); }
        }

        public ObservableCollection<TestViewModel> PageTests { get; set; }

        public Category ToCategory()
        {
            return new Category();
        }
    }
}