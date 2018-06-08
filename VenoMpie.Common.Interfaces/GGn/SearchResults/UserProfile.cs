using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VenoMpie.Common.Interfaces.GGn.SearchResults.SubResults;

namespace VenoMpie.Common.Interfaces.GGn.SearchResults
{
    public class UserProfile
    {
        public int id { get; set; }
        public string username { get; set; }
        public string avatar { get; set; }
        public int avatarType { get; set; }
        public bool isFriend { get; set; }
        public string bbProfileText { get; set; }
        public string profileText { get; set; }
        public string bbTitle { get; set; }
        public string title { get; set; }
        public UserProfile_Stats stats { get; set; }
        public UserProfile_Personal personal { get; set; }
        public UserProfile_Community community { get; set; }
        public UserProfile_Buffs buffs { get; set; }
        public UserProfile_Achievements achievements { get; set; }
    }
}
