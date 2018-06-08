using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.IO.FileReaders.Torrents.TorrentTypes
{
    public class BInteger : IBType<int>
    {
        public int Parse(string value)
        {
            throw new NotImplementedException();
        }

        public int Parse(BinaryReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
