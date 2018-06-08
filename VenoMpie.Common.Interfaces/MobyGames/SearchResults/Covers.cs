using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.Interfaces.MobyGames.SearchResults
{
    public class Cover
    {
        public string comments { get; set; }
        public string description { get; set; }
        public int height { get; set; }
        public string image { get; set; }
        public string scan_of { get; set; }
        public string thumbnail_image { get; set; }
        public int width { get; set; }
    }
    public class CoverGroup
    {
        public string comments { get; set; }
        public List<string> countries { get; set; }
        public List<Cover> covers { get; set; }
    }
    public class CoverGroups : APICallBase
    {
        public List<CoverGroup> cover_groups { get; set; }
    }
}
