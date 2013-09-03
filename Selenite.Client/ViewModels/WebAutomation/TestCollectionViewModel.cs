using System.Diagnostics;
using System.Windows.Input;
using Selenite.Client.Views.WebAutomation;
using Selenite.Global;
using Selenite.Models;

namespace Selenite.Client.ViewModels.WebAutomation
{
    public class TestCollectionViewModel : TreeViewModelBase<TestViewModel>
    {
        public TestCollectionViewModel()
        {
            OpenFileCommand = new RelayCommand(OpenTestCollection);
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

        public string FullPath { get; set; }

        public TestCollection ToModel()
        {
            return new TestCollection();
        }

        public ICommand OpenFileCommand { get; set; }

        private void OpenTestCollection(object parameter)
        {
            ServiceResolver.Get<HelpWindow>().Show();
            Process.Start(FullPath);
        }
    }
}