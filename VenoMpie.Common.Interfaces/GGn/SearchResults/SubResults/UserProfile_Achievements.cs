using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.Interfaces.GGn.SearchResults.SubResults
{
    public class UserProfile_Achievements
    {
        public string userLevel { get; set; }
        public string nextLevel { get; set; }
        public int totalPoints { get; set; }
        public int pointsToNextLvl { get; set; }
    }
}
