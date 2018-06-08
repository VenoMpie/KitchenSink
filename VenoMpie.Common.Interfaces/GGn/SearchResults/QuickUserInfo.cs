using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VenoMpie.Common.Interfaces.GGn.SearchResults.SubResults;

namespace VenoMpie.Common.Interfaces.GGn.SearchResults
{
    public class QuickUserInfo
    {
        public string username { get; set; }
        public int id { get; set; }
        public string authkey { get; set; }
        public string passkey { get; set; }
        public QuickUserInfo_Notifications notifications { get; set; }
        public QuickUserInfo_UserStats userstats { get; set; }
    }
}
