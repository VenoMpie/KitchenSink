using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VenoMpie.Common.IO.FileReaders.Torrents.TorrentTypes;

namespace VenoMpie.Common.IO.FileReaders.Torrents
{
    public class Torrent
    {
        public string Announce { get; set; }
        public List<string> AnnounceList { get; set; }
        public int CreationDate { get; set; }
        public string Comment { get; set; }
        public string CreatedBy { get; set; }

        public List<IBType<object>> Values { get; set; } = new List<IBType<object>>();

        public Torrent(string filename) : this(new BinaryReader(new FileStream(filename, FileMode.Open))) { }
        public Torrent(BinaryReader br)
        {

        }
        public void ParseTorrent(BinaryReader br)
        {
            while (br.BaseStream.Position <= br.BaseStream.Length)
            {
                //switch (br.PeekChar())
                //{
                //    case "d":
                //        Values.Add(new BDictionary().Parse(br));
                //}
            }
        }
    }
}
