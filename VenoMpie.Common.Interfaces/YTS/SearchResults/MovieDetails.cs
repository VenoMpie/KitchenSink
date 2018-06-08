using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VenoMpie.Common.Interfaces.YTS.SearchResults;

namespace VenoMpie.Common.Interfaces.YTS.SearchResults
{
    public enum MovieDetailsParametersEnum
    {
        with_images,
        with_cast,
    }

    [SearchClassAttribute("movie_details.json")]
    public class MovieDetails
    {
        public Movie movie { get; set; }
    }
}
