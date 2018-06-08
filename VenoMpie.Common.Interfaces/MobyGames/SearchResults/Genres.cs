using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.Interfaces.MobyGames.SearchResults
{
    public class Genre
    {
        public string genre_category { get; set; }
        public int genre_category_id { get; set; }
        public string genre_description { get; set; }
        public int genre_id { get; set; }
        public string genre_name { get; set; }
    }
    public class Genres : APICallBase
    {
        public List<Genre> genres { get; set; }
    }
}
