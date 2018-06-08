using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VenoMpie.Utils.Common.Drawing;

namespace VenoMpie.Common.WPF.Controls.FileExplorer
{
    public class FileExplorerTreeItem : IEquatable<FileExplorerTreeItem>
    {
        internal BitmapSource DriveIcon { get; set; }
        internal BitmapSource FolderIcon { get; set; }

        public string Name { get; set; }
        public string FullName { get; set; }
        public BitmapSource Image { get; set; }
        public ObservableCollection<FileExplorerTreeItem> Items { get; set; }
        public FileExplorerTreeItem()
        {
            Initialise();
        }
        public FileExplorerTreeItem(DriveInfo drive, BitmapSource driveIcon) : this()
        {
            DriveIcon = driveIcon;
            AddDriveInfo(drive);
        }
        public FileExplorerTreeItem(DirectoryInfo directory, BitmapSource folderIcon) : this()
        {
            FolderIcon = folderIcon;
            AddDirectoryInfo(directory);
        }
        public FileExplorerTreeItem(FileInfo file) : this()
        {
            AddFileInfo(file);
        }
        public FileExplorerTreeItem(string fullName, BitmapSource driveIcon, BitmapSource folderIcon) : this()
        {
            DriveIcon = driveIcon;
            FolderIcon = folderIcon;
            if (File.Exists(fullName))
                AddFileInfo(new FileInfo(fullName));
            else
                AddDirectoryInfo(new DirectoryInfo(fullName));

        }
        private void AddFileInfo(FileInfo fi)
        {
            FullName = fi.FullName;
            Name = fi.Name;
            Image = BitmapHelpers.GetFileIcon(fi.FullName, 16, 16);
        }
        private void AddDirectoryInfo(DirectoryInfo di)
        {
            FullName = di.FullName;
            Name = di.Name;
            Image = FolderIcon;
        }
        private void AddDriveInfo(DriveInfo di)
        {
            FullName = di.RootDirectory.FullName;
            Name = di.RootDirectory.FullName + ((di.IsReady) ? " (" + ((di.VolumeLabel == "") ? "Local Disk" : di.VolumeLabel) + ")" : "");
            Image = DriveIcon;
        }
        private void Initialise()
        {
            Items = new ObservableCollection<FileExplorerTreeItem>();
        }

        public bool Equals(FileExplorerTreeItem other)
        {
            return (this.FullName == other.FullName) ? true : false;
        }
    }
}
