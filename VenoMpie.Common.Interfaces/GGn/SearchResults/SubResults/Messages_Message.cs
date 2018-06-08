using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.Interfaces.GGn.SearchResults.SubResults
{
    public class Messages_Message
    {
        public int convId { get; set; }
        public string subject { get; set; }
        public bool unread { get; set; }
        public bool sticky { get; set; }
        public Messages_Participants newAnnouncement { get; set; }
        public DateTime date { get; set; }
    }
}
