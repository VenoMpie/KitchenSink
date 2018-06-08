using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.Interfaces.Layer13.SearchResults
{
    public class GetPre : Layer13ResultBase
    {
        public string id { get; set; }
        public string section { get; set; }
        public string rlsname { get; set; }
        public string pretime { get; set; }
    }
}
