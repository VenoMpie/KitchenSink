using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VenoMpie.Common.Interfaces.YTS.SearchResults;

namespace VenoMpie.Common.Interfaces.YTS
{
    public class YTS : JSONAPISearchBase
    {
        public override string APIPath { get { return "https://yts.ag/api/v2/"; } }

        private void BuildAPIPath<T>()
        {
            var searchAttr = typeof(T).GetCustomAttributes(typeof(SearchClassAttribute), true);
            if (!searchAttr.Any())
                throw new Exception("Search Class must have the SearchClassAttribute attribute set");

            string searchCommand = ((SearchClassAttribute)searchAttr[0]).SearchCommand;

            AddSearchTerm(searchCommand);
        }
        private void AddSearchTerm<T>(Dictionary<T, string> parameters) where T : class
        {
            if (!typeof(T).IsEnum)
            {
                throw new Exception("Search Key must be an enumerable type");
            }
            foreach (var param in parameters)
                AddSearchTermArgument(Enum.GetName(typeof(T), param.Key), param.Value);
        }
        public new SearchResult<T> Search<T>() where T : class, new()
        {
            BuildAPIPath<T>();
            return base.Search<SearchResult<T>>();
        }
        public SearchResult<T> Search<T, Y>(Y searchKey, string value) where T : class, new() where Y : struct, IConvertible
        {
            BuildAPIPath<T>();
            AddEnumSearchTerm(searchKey, value);
            return base.Search<SearchResult<T>>();
        }
        public SearchResult<T> Search<T, Y>(Dictionary<Y, string> parameters) where T : class, new() where Y : struct, IConvertible
        {
            BuildAPIPath<T>();
            AddEnumSearchTerm(parameters);
            return base.Search<SearchResult<T>>();
        }
        public SearchResult<MovieDetails> SearchMovieDetails(int movieID)
        {
            BuildAPIPath<MovieDetails>();
            AddSearchTermArgument("movie_id", movieID.ToString());
            return base.Search<SearchResult<MovieDetails>>();
        }
        public SearchResult<MovieDetails> SearchMovieDetails<T>(int movieID, T searchKey, string value) where T : struct, IConvertible
        {
            BuildAPIPath<MovieDetails>();
            AddSearchTermArgument("movie_id", movieID.ToString());
            AddEnumSearchTerm(searchKey, value);
            return base.Search<SearchResult<MovieDetails>>();
        }
        public SearchResult<MovieDetails> SearchMovieDetails<T>(int movieID, Dictionary<T, string> parameters) where T : struct, IConvertible
        {
            BuildAPIPath<MovieDetails>();
            AddSearchTermArgument("movie_id", movieID.ToString());
            AddEnumSearchTerm(parameters);
            return base.Search<SearchResult<MovieDetails>>();
        }
        public SearchResult<MovieSuggestions> SearchMovieSuggestions(int movieID)
        {
            BuildAPIPath<MovieSuggestions>();
            AddSearchTermArgument("movie_id", movieID.ToString());
            return base.Search<SearchResult<MovieSuggestions>>();
        }

        private void AddEnumSearchTerm<T>(T searchKey, string value) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new Exception("Search Key must be an enumerable type");
            }
            AddSearchTermArgument(Enum.GetName(typeof(T), searchKey), value);
        }
        private void AddEnumSearchTerm<T>(Dictionary<T, string> parameters) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new Exception("Search Key must be an enumerable type");
            }
            foreach (var param in parameters)
                AddSearchTermArgument(Enum.GetName(typeof(T), param.Key), param.Value);
        }
    }
}
