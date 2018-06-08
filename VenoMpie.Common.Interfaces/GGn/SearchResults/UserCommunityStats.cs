using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VenoMpie.Common.Interfaces.GGn.SearchResults.SubResults;

namespace VenoMpie.Common.Interfaces.GGn.SearchResults
{
    public class UserCommunityStats
    {
        public string leeching { get; set; }
        public string seeding { get; set; }
        public string snatched { get; set; }
        public string usnatched { get; set; }
        public string downloaded { get; set; }
        public string udownloaded { get; set; }
        public decimal seedingperc { get; set; }
    }
}
