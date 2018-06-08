using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.Interfaces.GGn.SearchResults.SubResults
{
    public class QuickUserInfo_UserStats
    {
        public long uploaded { get; set; }
        public long downloaded { get; set; }
        public decimal ratio { get; set; }
        public decimal requiredratio { get; set; }
        public string @class { get; set; }
    }
}
