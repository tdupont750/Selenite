using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Selenite.Client.Converters
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// http://stackoverflow.com/questions/1000040/selecteditem-in-a-wpf-treeview
    /// </remarks>
    public class TreeViewHelper : DependencyObject
    {
        private static readonly Dictionary<DependencyObject, TreeViewSelectedItemBehavior> Behaviors =
            new Dictionary<DependencyObject, TreeViewSelectedItemBehavior>();

        public static object GetSelectedItem(DependencyObject obj)
        {
            return obj.GetValue(SelectedItemProperty);
        }

        public static void SetSelectedItem(DependencyObject obj, object value)
        {
            obj.SetValue(SelectedItemProperty, value);
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.RegisterAttached("SelectedItem", typeof (object), typeof (TreeViewHelper),
                                                new UIPropertyMetadata(null, OnSelectedItemChanged));

        private static void OnSelectedItemChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (!(obj is TreeView))
                return;

            if (!Behaviors.ContainsKey(obj))
                Behaviors.Add(obj, new TreeViewSelectedItemBehavior(obj as TreeView));

            TreeViewSelectedItemBehavior view = Behaviors[obj];
            view.ChangeSelectedItem(e.NewValue);
        }

        private class TreeViewSelectedItemBehavior
        {
            private readonly TreeView _view;
            public TreeViewSelectedItemBehavior(TreeView view)
            {
                _view = view;
                view.SelectedItemChanged += (sender, e) => SetSelectedItem(view, e.NewValue);
            }

            internal void ChangeSelectedItem(object p)
            {
                var item = (TreeViewItem)_view.ItemContainerGenerator.ContainerFromItem(p);

                if (item != null)
                    item.IsSelected = true;
            }
        }
    }
}