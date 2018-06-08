using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.Interfaces.MobyGames.SearchResults
{
    public class Group
    {
        public string group_description { get; set; }
        public int group_id { get; set; }
        public string group_name { get; set; }
    }
    public class Groups : APICallBase
    {
        public List<Group> groups { get; set; }
    }
}
