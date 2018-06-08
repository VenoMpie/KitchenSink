using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VenoMpie.Common.WPF.Controls;

namespace VenoMpie.Common.WPF.Controls.FileExplorer
{
    /// <summary>
    /// Interaction logic for FileExplorer.xaml
    /// </summary>
    public partial class FileExplorer : UserControl
    {
        #region Public Properties
        public bool ExecuteFileOnDoubleClick { get { return this.fetView.ExecuteFileOnDoubleClick; } set { this.fetView.ExecuteFileOnDoubleClick = value; } }
        #endregion
        #region Events
        #region SelectedItemChanged
        public event RoutedEventArgsHandler SelectedItemChanged_FileTree;
        public delegate void RoutedEventArgsHandler(object sender, RoutedPropertyChangedEventArgs<object> e);
        private void fetTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedItem = e.NewValue as FileExplorerTreeItem;
            fetView.SetObjects(selectedItem.FullName);
            SelectedItemChanged_FileTree?.Invoke(sender, e);
        }
        #endregion
        #region SelectedItemChanged
        public event SelectionChangedEventArgsHandler SelectionChanged_FileView;
        public delegate void SelectionChangedEventArgsHandler(object sender, SelectionChangedEventArgs e);
        private void fetView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionChanged_FileView?.Invoke(sender, e);
        }
        #endregion
        #endregion
        public FileExplorer()
        {
            InitializeComponent();
        }

        private void fetView_FileExplorerView_ParentMouseDoubleClick(object sender, FileExplorerViewItem parent)
        {
            if (fetTree.SelectedTreeViewItem != null)
            {
                TreeViewItem treeViewParent = ControlHelpers.FindParent<TreeViewItem>(fetTree.SelectedTreeViewItem);
                if (treeViewParent != null) treeViewParent.IsSelected = true;
            }
        }

        private void fetTree_FileActionSuccess(bool arg1, FileExplorerTree.FileAction arg2)
        {
            if (arg1 == true && arg2 == FileExplorerTree.FileAction.Move)
            {
                ObservableCollection<FileExplorerViewItem> collection = (fetView.lvwExplorer.ItemsSource as ObservableCollection<FileExplorerViewItem>);
                if (collection != null)
                {
                    foreach (FileExplorerViewItem item in fetView.lvwExplorer.SelectedItems.Cast<FileExplorerViewItem>().ToList())
                        collection.Remove(item);
                }
            }
        }
    }
}
