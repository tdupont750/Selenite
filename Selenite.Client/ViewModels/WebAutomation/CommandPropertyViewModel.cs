namespace Selenite.Client.ViewModels.WebAutomation
{
    public class CommandPropertyViewModel : ViewModelBase
    {
        public string Name
        {
            get { return Get(() => Name); }
            set { Set(value, () => Name); }
        }

        public string Value
        {
            get { return Get(() => Value); }
            set { Set(value, () => Value); }
        }
    }
}