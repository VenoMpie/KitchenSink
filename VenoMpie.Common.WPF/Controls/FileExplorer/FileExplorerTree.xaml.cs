using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VenoMpie.Common.WPF.Controls;
using VenoMpie.Utils.Common.Drawing;

namespace VenoMpie.Common.WPF.Controls.FileExplorer
{
    /// <summary>
    /// Interaction logic for FileExplorer.xaml
    /// </summary>
    public partial class FileExplorerTree : UserControl
    {
        #region Design Time Properties
        [Category("Common")]
        [Description("Also lists files in the tree.")]
        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public bool ShowFiles { get; set; }
        #endregion
        #region Enums
        public enum FileAction
        {
            Copy,
            Move
        }
        #endregion
        #region Events
        #region FileHandling
        public event Action<bool, FileAction> FileActionSuccess;
        #endregion
        #region SelectedItemChanged
        public event RoutedEventArgsHandler FileExplorerTree_SelectedItemChanged;

        public delegate void RoutedEventArgsHandler(object sender, RoutedPropertyChangedEventArgs<object> e);
        private void tvwExplorer_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            FileExplorerTree_SelectedItemChanged?.Invoke(sender, e);
            SelectedTreeViewItem = ControlHelpers.FindHierarchyParent<TreeViewItem>(tvwExplorer.ItemContainerGenerator, e.NewValue);
        }
        #endregion
        #endregion
        #region Public Properties
        public TreeViewItem SelectedTreeViewItem { get; private set; }
        #endregion
        #region Private Properties
        Stream folderIconStream;
        Stream driveIconStream;
        BitmapSource driveIcon { get; set; }
        BitmapSource folderIcon { get; set; }

        #region Drag and Drop
        string[] DroppedFiles { get; set; }
        TreeViewItem DroppedTreeviewItem { get; set; }
        #endregion

        ObservableCollection<FileExplorerTreeItem> myList = new ObservableCollection<FileExplorerTreeItem>();
        #endregion

        public FileExplorerTree()
        {
            InitializeComponent();

            folderIconStream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("VenoMpie.Common.WPF.Controls.FileExplorer.Icons.folder.png");
            driveIconStream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("VenoMpie.Common.WPF.Controls.FileExplorer.Icons.drive.png");
            folderIcon = BitmapHelpers.ConvertStreamToBitmapImage(folderIconStream);
            driveIcon = BitmapHelpers.ConvertStreamToBitmapImage(driveIconStream);

            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo driveInfo in drives)
            {
                FileExplorerTreeItem fe = new FileExplorerTreeItem(driveInfo, driveIcon);
                fe.Items.Add(new FileExplorerTreeItem());
                myList.Add(fe);
            }

            tvwExplorer.ItemsSource = myList;
        }
        private void tvwExplorer_Expanded(object sender, RoutedEventArgs e)
        {
            FileExplorerTreeItem item = (e.OriginalSource as TreeViewItem).DataContext as FileExplorerTreeItem;
            if ((item.Items.Count == 1) && (item.Items[0].FullName == null))
            {
                LoadTreeItems(ref item);
            }
        }
        private void TreViewItem_Drop(object sender, DragEventArgs e)
        {
            ClearBorder(e.Source as DependencyObject);
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                TreeViewItem tItem = ControlHelpers.FindParent<TreeViewItem>(e.Source as DependencyObject);
                if (tItem != null)
                {
                    DroppedTreeviewItem = tItem;
                    DroppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
                    SetContextMenuStatus(true);
                }
            }
        }
        private void SetContextMenuStatus(bool isEnabled)
        {
            cmnuFileCommands.IsOpen = true;
        }

        private void TreViewItem_DragEnter(object sender, DragEventArgs e)
        {
            HighlightBorder(e.Source as DependencyObject);
        }
        private void TreViewItem_DragLeave(object sender, DragEventArgs e)
        {
            ClearBorder(e.Source as DependencyObject);
        }
        private void HighlightBorder(DependencyObject dependencyObject)
        {
            HighlightBorder(dependencyObject, new Thickness(1), new SolidColorBrush(Colors.Black));
        }
        private void HighlightBorder(DependencyObject dependencyObject, Thickness thickness, Brush brush)
        {
            Border parentItem = ControlHelpers.FindParent<Border>(dependencyObject);
            if (parentItem != null)
            {
                parentItem.BorderThickness = thickness;
                parentItem.BorderBrush = brush;
            }
        }
        private void ClearBorder(DependencyObject dependencyObject)
        {
            Border parentItem = ControlHelpers.FindParent<Border>(dependencyObject);
            if (parentItem != null)
            {
                parentItem.BorderThickness = new Thickness(0);
                parentItem.BorderBrush = null;
            }
        }

        private void mnuCopy_Click(object sender, RoutedEventArgs e)
        {
            ActionMoveCopyAction(FileAction.Copy);
        }
        private void mnuMove_Click(object sender, RoutedEventArgs e)
        {
            ActionMoveCopyAction(FileAction.Move);
        }
        private void ActionMoveCopyAction(FileAction action)
        {
            try
            {
                if (DroppedFiles != null)
                {
                    FileExplorerTreeItem item = (FileExplorerTreeItem)DroppedTreeviewItem.DataContext;
                    foreach (var file in DroppedFiles)
                    {
                        switch (action)
                        {
                            case FileAction.Copy:
                                File.Copy(file, Path.Combine(item.FullName, Path.GetFileName(file)));
                                break;
                            case FileAction.Move:
                                File.Move(file, Path.Combine(item.FullName, Path.GetFileName(file)));
                                break;
                        }
                    }
                    LoadTreeItems(ref item);
                    DroppedTreeviewItem = null;
                    DroppedFiles = null;
                    FileActionSuccess?.Invoke(true, action);
                }
            }
            catch (Exception ex)
            {
                FileActionSuccess?.Invoke(false, action);
                MessageBox.Show(ex.Message, "An error occurred");
            }
        }
        private void mnuCancel_Click(object sender, RoutedEventArgs e)
        {
            DroppedTreeviewItem = null;
            DroppedFiles = null;
        }

        private void LoadTreeItems(ref FileExplorerTreeItem item)
        {
            item.Items.Clear();
            try
            {
                foreach (DirectoryInfo subDir in new DirectoryInfo(item.FullName).GetDirectories())
                {
                    FileExplorerTreeItem fe = new FileExplorerTreeItem(subDir, folderIcon);
                    fe.Items.Add(new FileExplorerTreeItem());
                    item.Items.Add(fe);
                }
                if (ShowFiles)
                    foreach (FileInfo subFile in new DirectoryInfo(item.FullName).GetFiles())
                        item.Items.Add(new FileExplorerTreeItem(subFile));
            }
            catch { }
        }
    }
}
