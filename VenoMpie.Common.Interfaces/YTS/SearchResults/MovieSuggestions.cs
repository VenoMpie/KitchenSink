using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VenoMpie.Common.Interfaces.YTS.SearchResults;

namespace VenoMpie.Common.Interfaces.YTS.SearchResults
{
    [SearchClassAttribute("movie_suggestions.json")]
    public class MovieSuggestions
    {
        public int movie_count { get; set; }
        public List<Movie> movies { get; set; } = new List<Movie>();
    }
}
