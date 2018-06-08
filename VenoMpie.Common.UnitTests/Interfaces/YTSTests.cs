using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using VenoMpie.Common.Interfaces.YTS.SearchResults;
using VenoMpie.Common.Interfaces.YTS;
using System.Linq;

namespace VenoMpie.Common.UnitTests.Interfaces
{
    [TestClass]
    public class YTSTests
    {
        private YTS yts = new YTS();

        [TestMethod]
        public void YTS_WebCall_ListMovies()
        {
            SearchResult<ListMovies> movies = yts.Search<ListMovies>();
            Assert.AreEqual(20, movies.data.movies.Count);
        }
        [TestMethod]
        public void YTS_WebCall_ListMovies_Limit10()
        {
            SearchResult<ListMovies> movies = yts.Search<ListMovies, ListMoviesParametersEnum>(ListMoviesParametersEnum.limit, "10");
            Assert.AreEqual(10, movies.data.movies.Count);
        }
        [TestMethod]
        public void YTS_WebCall_ListMovies_Query_LionKing()
        {
            SearchResult<ListMovies> movies = yts.Search<ListMovies, ListMoviesParametersEnum>(ListMoviesParametersEnum.query_term, "Lion King");
            Assert.IsTrue(movies.data.movies.Any(a => a.title == "The Lion King"));
        }
        [TestMethod]
        public void YTS_WebCall_MovieDetails()
        {
            SearchResult<MovieDetails> movies = yts.SearchMovieDetails(3468);
            Assert.AreEqual("The Lion King", movies.data.movie.title);
            Assert.AreEqual("English", movies.data.movie.language);
        }
        [TestMethod]
        public void YTS_WebCall_MovieSuggestions()
        {
            SearchResult<MovieSuggestions> movies = yts.SearchMovieSuggestions(3468);
            Assert.IsTrue(movies.data.movies.Count > 0);
        }
        [TestMethod]
        public void YTS_Deserialize_ListMovies()
        {
            SearchResult<ListMovies> movies = yts.Deserialize<SearchResult<ListMovies>>(Properties.Resources.Interfaces_YTS_List_Movies);
            Assert.AreEqual(20, movies.data.movies.Count);
            Assert.AreEqual("Pirates of the Caribbean: Dead Men Tell No Tales", movies.data.movies[0].title);
        }
        [TestMethod]
        public void YTS_Deserialize_MovieSuggestions()
        {
            SearchResult<MovieSuggestions> movies = yts.Deserialize<SearchResult<MovieSuggestions>>(Properties.Resources.Interfaces_YTS_Movie_Suggestions);
            Assert.AreEqual(4, movies.data.movie_count);
            Assert.AreEqual(4, movies.data.movies.Count);
            Assert.AreEqual("Babylon A.D.", movies.data.movies[0].title);
        }
    }
}
