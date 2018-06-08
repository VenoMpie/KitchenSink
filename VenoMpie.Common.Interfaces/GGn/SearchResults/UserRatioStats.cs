using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VenoMpie.Common.Interfaces.GGn.SearchResults.SubResults;

namespace VenoMpie.Common.Interfaces.GGn.SearchResults
{
    public class UserRatioStats
    {
        public string uploaded { get; set; }
        public string downloaded { get; set; }
        public decimal ratio { get; set; }
        public long buffer { get; set; }
        public decimal disposable { get; set; }
        public string reqratio { get; set; }
    }
}
