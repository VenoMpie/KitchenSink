using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.Interfaces.GGn.SearchResults.SubResults
{
    public class UserProfile_Community
    {
        public int profileViews { get; set; }
        public decimal hourlyGold { get; set; }
        public int posts { get; set; }
        public int actualPosts { get; set; }
        public int threads { get; set; }
        public int forumLikes { get; set; }
        public int forumDislikes { get; set; }
        public int ircLines { get; set; }
        public int ircActualLines { get; set; }
        public int torrentComments { get; set; }
        public int collections { get; set; }
        public int requestsFilled { get; set; }
        public long bountyEarnedUpload { get; set; }
        public long bountyEarnedGold { get; set; }
        public int requestsVoted { get; set; }  
        public long bountySpentUpload { get; set; }
        public long bountySpentGold { get; set; }
        public int reviews { get; set; }
        public int uploaded { get; set; }
        public int seeding { get; set; }
        public int leeching { get; set; }
        public int snatched { get; set; }
        public int uniqueSnatched { get; set; }
        public long seedSize { get; set; }
        public int invited { get; set; }
    }
}
