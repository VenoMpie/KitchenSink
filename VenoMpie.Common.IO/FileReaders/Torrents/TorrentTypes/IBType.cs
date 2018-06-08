using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.IO.FileReaders.Torrents.TorrentTypes
{
    public interface IBType<T> : IBValue
    {
        T Parse(string value);
        T Parse(BinaryReader reader);
    }
}
