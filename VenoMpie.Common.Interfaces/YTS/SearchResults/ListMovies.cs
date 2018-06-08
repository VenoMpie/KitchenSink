using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VenoMpie.Common.Interfaces.YTS.SearchResults;

namespace VenoMpie.Common.Interfaces.YTS.SearchResults
{
    public enum ListMoviesParametersEnum
    {
        limit,
        page,
        quality,
        minimum_rating,
        query_term,
        genre,
        sort_by,
        order_by,
        with_rt_ratings
    }

    [SearchClassAttribute("list_movies.json")]
    public class ListMovies
    {
        public int movie_count { get; set; }
        public int limit { get; set; }
        public int page_number { get; set; }
        public List<Movie> movies { get; set; } = new List<Movie>();
    }
}
