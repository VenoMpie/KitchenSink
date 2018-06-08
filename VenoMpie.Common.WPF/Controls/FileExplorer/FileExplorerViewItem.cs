using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VenoMpie.Utils.Common.Drawing;

namespace VenoMpie.Common.WPF.Controls.FileExplorer
{
    public class FileExplorerViewItem : IEquatable<FileExplorerViewItem>
    {
        public string Name { get; set; }
        public FileInfo FileInfo { get; set; }
        public BitmapSource Image { get; set; }
        public ObservableCollection<FileExplorerViewItem> Items { get; set; }
        public FileExplorerViewItem()
        {
            Initialise();
            Name = "..";
        }
        public FileExplorerViewItem(FileInfo file)
        {
            Initialise();
            AddFileInfo(file);
        }
        public FileExplorerViewItem(string fullName)
        {
            Initialise();
            AddFileInfo(new FileInfo(fullName));
        }
        private void AddFileInfo(FileInfo fi)
        {
            Name = fi.Name;
            FileInfo = fi;
            Image = BitmapHelpers.GetFileIcon(fi.FullName, 16, 16);
        }
        private void Initialise()
        {
            Items = new ObservableCollection<FileExplorerViewItem>();
        }

        public bool Equals(FileExplorerViewItem other)
        {
            return (this.FileInfo == other.FileInfo) ? true : false;
        }
    }
}
