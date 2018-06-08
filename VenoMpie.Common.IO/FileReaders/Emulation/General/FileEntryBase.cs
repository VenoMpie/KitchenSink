using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.IO.FileReaders.Emulation.General
{
    public class FileEntryBase : IEquatable<FileEntryBase>
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }
        public string Crc32 { get; set; }
        public DateTime FileDate { get; set; }

        public FileEntryBase() { }
        public bool Equals(FileEntryBase other)
        {
            return
                this.Name.Equals(other.Name)
                && this.Extension.Equals(other.Extension)
                && this.Size.Equals(other.Size)
                && this.Crc32.Equals(other.Crc32)
                && this.FileDate.Equals(other.FileDate);
        }
        public override int GetHashCode()
        {
            int hash = 13;
            hash = (hash * 7) + this.Name.GetHashCode();
            hash = (hash * 7) + this.Extension.GetHashCode();
            hash = (hash * 7) + this.Size.GetHashCode();
            hash = (hash * 7) + this.Crc32.GetHashCode();
            hash = (hash * 7) + this.FileDate.GetHashCode();
            return hash;
        }
    }
}
