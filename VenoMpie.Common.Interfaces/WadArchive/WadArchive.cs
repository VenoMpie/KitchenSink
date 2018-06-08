using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.Interfaces.WadArchive
{
    public class SearchResult
    {
        public long size { get; set; }
        public string type { get; set; }
        public string iwad { get; set; }
        public string[] port { get; set; }
        public string[] filenames { get; set; }
        public string[] links { get; set; }

        public SearchResult() { }
    }
    public class WadArchive : JSONAPISearchBase
    {
        public override string APIPath { get { return "http://www.wad-archive.com/wadseeker/"; } }

        public SearchResult Search()
        {
            return Search<SearchResult>();
        }
        public SearchResult Deserialize(string deserializeString)
        {
            return Deserialize<SearchResult>(deserializeString);
        }
    }
}
