using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.IO.FileReaders.Torrents.TorrentTypes
{
    public class BList : IBType<List<IBValue>>
    {
        public List<IBValue> Parse(string value)
        {
            throw new NotImplementedException();
        }

        public List<IBValue> Parse(BinaryReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
