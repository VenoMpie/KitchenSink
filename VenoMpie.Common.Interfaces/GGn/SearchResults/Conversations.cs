using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VenoMpie.Common.Interfaces.GGn.SearchResults.SubResults;

namespace VenoMpie.Common.Interfaces.GGn.SearchResults
{
    public class Conversations
    {
        public int convId { get; set; }
        public string subject { get; set; }
        public bool sticky { get; set; }
        public Conversations_Message[] messages { get; set; }
    }
}
