using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.Interfaces.GGn.SearchResults.SubResults
{
    public class Conversations_Message
    {
        public int messageId { get; set; }
        public int senderId { get; set; }
        public string senderName { get; set; }
        public DateTime sentDate { get; set; }
        public string avatar { get; set; }
        public string bbBody { get; set; }
        public string body { get; set; }
    }
}
