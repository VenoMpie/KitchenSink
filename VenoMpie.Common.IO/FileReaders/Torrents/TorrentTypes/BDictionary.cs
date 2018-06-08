using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.IO.FileReaders.Torrents.TorrentTypes
{
    public class BDictionary : IBType<Dictionary<string, IBValue>>
    {
        public Dictionary<string, IBValue> Parse(string value)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, IBValue> Parse(BinaryReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
