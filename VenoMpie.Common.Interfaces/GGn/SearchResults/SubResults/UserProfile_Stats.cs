using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.Interfaces.GGn.SearchResults.SubResults
{
    public class UserProfile_Stats
    {
        public DateTime joinedDate { get; set; }
        public DateTime lastAccess { get; set; }
        public bool onIRC { get; set; }
        public long uploaded { get; set; }
        public long downloaded { get; set; }
        public long fullDownloaded { get; set; }
        public long purchasedDownload { get; set; }
        public decimal ratio { get; set; }
        public decimal requiredRatio { get; set; }
        public decimal shareScore { get; set; }
        public long gold { get; set; }
    }
}
