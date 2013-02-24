using Selenite.Models;

namespace Selenite.Client.ViewModels.WebAutomation
{
    public class CategoryViewModel : TreeViewModelBase<TestViewModel>
    {
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

        public Category ToModel()
        {
            return new Category();
        }
    }
}