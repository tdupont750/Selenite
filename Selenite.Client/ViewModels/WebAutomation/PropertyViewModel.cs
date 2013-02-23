using System.Text.RegularExpressions;

namespace Selenite.Client.ViewModels.WebAutomation
{
    public class PropertyViewModel : ViewModelBase
    {
        public Regex Validator { get; set; }
        public bool HasValidator
        {
            get { return Validator != null; }
        }

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