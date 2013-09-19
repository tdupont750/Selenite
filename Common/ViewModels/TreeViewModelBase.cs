using System.Collections.ObjectModel;

namespace Common.ViewModels
{
    public abstract class TreeViewModelBase<T> : ViewModelBase where T : ViewModelBase
    {
        protected TreeViewModelBase()
        {
            Children = new ObservableCollection<T>();
        }

        public ObservableCollection<T> Children { get; set; }

        public bool IsExpanded
        {
            get { return Get(() => IsExpanded); }
            set { Set(value, () => IsExpanded); }
        }

        public bool IsSelected
        {
            get { return Get(() => IsSelected); }
            set { Set(value, () => IsSelected); }
        }
    }
}
