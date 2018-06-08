using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.Interfaces.MobyGames.SearchResults
{
    public class Screenshot
    {
        public string caption { get; set; }
        public int height { get; set; }
        public string image { get; set; }
        public string thumbnail_image { get; set; }
        public int width { get; set; }
    }
    public class Screenshots : APICallBase
    {
        public List<Screenshot> screenshots { get; set; }
    }
}
