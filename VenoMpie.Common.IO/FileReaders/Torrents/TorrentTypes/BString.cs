using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.IO.FileReaders.Torrents.TorrentTypes
{
    class BString : IBType<string>
    {
        public string Parse(string value)
        {
            throw new NotImplementedException();
        }

        public string Parse(BinaryReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
