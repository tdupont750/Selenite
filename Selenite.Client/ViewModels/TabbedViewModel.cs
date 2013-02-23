namespace Selenite.Client.ViewModels
{
    public abstract class TabbedViewModel : ViewModelBase
    {
        public string Header
        {
            get { return Get(() => Header); }
            set { Set(value, () => Header); }
        }

        public bool IsSelected
        {
            get { return Get(() => IsSelected); }
            set { Set(value, () => IsSelected); }
        }
    }
}