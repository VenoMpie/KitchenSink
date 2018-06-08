using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VenoMpie.Common.Interfaces.GGn.SearchResults.SubResults;

namespace VenoMpie.Common.Interfaces.GGn.SearchResults
{
    public class Messages
    {
        public int currentPage { get; set; }
        public int pages { get; set; }
        public Messages_Message[] messages { get; set; }
    }
}
