using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.Interfaces.YTS.SearchResults
{
    public class Torrent
    {
        public string url { get; set; }
        public string hash { get; set; }
        public string quality { get; set; }
        public int seeds { get; set; }
        public int peers { get; set; }
        public string size { get; set; }
        public long size_bytes { get; set; }
        public DateTime date_uploaded { get; set; }
        public long date_uploaded_unix { get; set; }
    }
}
