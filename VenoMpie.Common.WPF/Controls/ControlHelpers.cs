using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VenoMpie.Common.WPF.Controls
{
    public static class ControlHelpers
    {
        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);
            if (parent != null)
            {
                if (parent is T)
                    return parent as T;
                else
                    return FindParent<T>(parent);
            }
            return null;
        }
        public static T FindHierarchyParent<T>(ItemContainerGenerator containerGenerator, object item) where T : ItemsControl
        {
            ItemsControl container = containerGenerator.ContainerFromItem(item) as ItemsControl;
            if (container != null && container is T)
                return (T)container;

            foreach (var childItem in containerGenerator.Items)
            {
                ItemsControl parent = containerGenerator.ContainerFromItem(childItem) as ItemsControl;
                if (parent == null)
                    continue;

                container = parent.ItemContainerGenerator.ContainerFromItem(item) as TreeViewItem;
                if (container != null)
                    return (T)container;

                container = FindHierarchyParent<T>(parent.ItemContainerGenerator, item);
                if (container != null)
                    return (T)container;
            }
            return null;
        }
    }
}
