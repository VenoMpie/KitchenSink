using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
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

namespace VenoMpie.Common.WPF.Controls.FileExplorer
{
    /// <summary>
    /// Interaction logic for FileExplorerView.xaml
    /// </summary>
    public partial class FileExplorerView : UserControl
    {
        #region Public Properties
        public bool ExecuteFileOnDoubleClick { get; set; }
        #endregion
        #region Private Properties
        ObservableCollection<FileExplorerViewItem> myList = new ObservableCollection<FileExplorerViewItem>();
        #endregion
        #region Events
        #region SelectedItemChanged
        public event SelectionChangedEventArgsHandler FileExplorerView_SelectionChanged;
        public event ParentMouseDoubleClickEventArgsHandler FileExplorerView_ParentMouseDoubleClick;
        public event MouseButtonEventHandler FileExplorerView_MouseDoubleClick;
        public delegate void SelectionChangedEventArgsHandler(object sender, SelectionChangedEventArgs e);
        public delegate void ParentMouseDoubleClickEventArgsHandler(object sender, FileExplorerViewItem parent);
        public delegate void MouseButtonEventArgsHandler(object sender, MouseButtonEventArgs e);
        private void lvwExplorer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FileExplorerView_SelectionChanged?.Invoke(sender, e);
        }
        private void lvwExplorer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FileExplorerView_MouseDoubleClick?.Invoke(sender, e);
            if (lvwExplorer.SelectedItems.Count == 1 && lvwExplorer.SelectedItem != null)
            {
                FileExplorerViewItem parent = lvwExplorer.SelectedItem as FileExplorerViewItem;
                if (parent != null)
                {
                    if (parent.FileInfo == null)
                        FileExplorerView_ParentMouseDoubleClick?.Invoke(sender, parent);
                    else if (ExecuteFileOnDoubleClick)
                    {
                        System.Diagnostics.Process p = new System.Diagnostics.Process();
                        p.StartInfo = new System.Diagnostics.ProcessStartInfo(parent.FileInfo.FullName);
                        p.Start();
                    }
                }
            }
        }
        #endregion
        #endregion

        public FileExplorerView()
        {
            InitializeComponent();
        }
        public void SetObjects(string fullPath)
        {
            myList = new ObservableCollection<FileExplorerViewItem>();
            myList.Add(new FileExplorerViewItem());
            try
            {
                foreach (var item in Directory.GetFiles(fullPath).Select(a => new FileInfo(a)).AsParallel())
                {
                    myList.Add(new FileExplorerViewItem(item));
                }
            }
            catch { }
            lvwExplorer.ItemsSource = myList;
        }

        private void lvwExplorer_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed && lvwExplorer.SelectedItems.Count > 0)
            {
                StringCollection files = new StringCollection();
                foreach (var item in lvwExplorer.SelectedItems.Cast<FileExplorerViewItem>())
                {
                    if (item.FileInfo != null) files.Add(item.FileInfo.FullName);
                }
                if (files.Count > 0)
                {
                    DataObject data = new DataObject();
                    data.SetFileDropList(files);
                    DragDrop.DoDragDrop(lvwExplorer, data, DragDropEffects.Move);
                }
            }
        }
    }
}
