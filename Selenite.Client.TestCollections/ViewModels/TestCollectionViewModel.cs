using Common.ViewModels;
using Selenite.Models;
using System.Windows.Input;

namespace Selenite.Client.TestCollections.ViewModels
{
    public class TestCollectionViewModel : TreeViewModelBase<TestViewModel>
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

        public string FullPath { get; set; }

        public TestCollection ToModel()
        {
            return new TestCollection();
        }

        public ICommand OpenFileCommand { get; set; }
    }
}