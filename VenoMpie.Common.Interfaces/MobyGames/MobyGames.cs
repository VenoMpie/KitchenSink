using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using VenoMpie.Common.Interfaces.MobyGames.SearchResults;

namespace VenoMpie.Common.Interfaces.MobyGames
{
    /// <summary>
    /// Class for Interfacing with MobyGames
    /// </summary>
    /// <remarks>
    /// Who doesn't know MobyGames?
    /// Will comment this class later, my youngest son is born today so will probably not get to this in a while :P
    /// </remarks>
    /// <see cref="http://www.mobygames.com/"/>
    public class MobyGames : JSONAPISearchBase
    {
        public enum GameOutputFormat
        {
            ID,
            Brief,
            Normal
        }
        private const int DEFAULT_LIMIT = 100;
        private const int DEFAULT_OFFSET = 0;
        private const GameOutputFormat DEFAULT_SEARCH_OUTPUT_FORMAT = GameOutputFormat.Normal;
        private const GameOutputFormat DEFAULT_RECENT_OUTPUT_FORMAT = GameOutputFormat.ID;
        private const int DEFAULT_RECENT_AGE = 21;

        public override string APIPath { get { return "https://api.mobygames.com/v1"; } }
        public string API_Key { get; set; } = "YOUR KEY HERE";

        public MobyGames() { }
        public MobyGames(string apikey)
        {
            API_Key = apikey;
        }

        #region Genres
        public List<Genre> SearchGenres()
        {
            //try
            //{
            AddSearchTerm("/genres");
            return SearchWithKey<Genres>().genres ?? new List<Genre>();
            //}
            //catch (Exception ex)
            //{
            //    return PopulateException<Genres>(ex, ex.Message);
            //}
        }
        #endregion
        #region Groups
        public List<Group> SearchGroups(int limit = DEFAULT_LIMIT, int offset = DEFAULT_OFFSET)
        {
            AddSearchTerm("/groups");
            AddLimitOffset(limit, offset);

            return SearchWithKey<Groups>().groups ?? new List<Group>();
        }
        #endregion
        #region Platforms
        public List<Platform> SearchPlatforms()
        {
            AddSearchTerm("/platforms");
            return SearchWithKey<Platforms>().platforms ?? new List<Platform>();
        }
        #endregion
        #region Search Games
        #region ID
        public List<int> SearchGames_ID() => SearchGames_ID(new List<int>() { });
        public List<int> SearchGames_ID(string title) => SearchGames_ID(new List<int>(), new List<int>(), new List<int>(), title);
        public List<int> SearchGames_ID(List<int> ids)
        {
            BuildGamesSearch(ids, GameOutputFormat.ID);
            return SearchWithKey<Games_ID>().games ?? new List<int>();
        }
        public List<int> SearchGames_ID(List<int> platforms, List<int> genres, List<int> groups, string title = "", int limit = DEFAULT_LIMIT, int offset = DEFAULT_OFFSET)
        {
            BuildGamesSearch(platforms, genres, groups, GameOutputFormat.ID, title, limit, offset);
            return SearchWithKey<Games_ID>().games ?? new List<int>();
        }
        #endregion
        #region Brief
        public List<Game_Brief> SearchGames_Brief() => SearchGames_Brief(new List<int>() { });
        public List<Game_Brief> SearchGames_Brief(string title) => SearchGames_Brief(new List<int>(), new List<int>(), new List<int>(), title);
        public List<Game_Brief> SearchGames_Brief(List<int> ids)
        {
            BuildGamesSearch(ids, GameOutputFormat.Brief);
            return SearchWithKey<Games_Brief>().games ?? new List<Game_Brief>();
        }
        public List<Game_Brief> SearchGames_Brief(List<int> platforms, List<int> genres, List<int> groups, string title = "", int limit = DEFAULT_LIMIT, int offset = DEFAULT_OFFSET)
        {
            BuildGamesSearch(platforms, genres, groups, GameOutputFormat.Brief, title, limit, offset);
            return SearchWithKey<Games_Brief>().games ?? new List<Game_Brief>();
        }
        #endregion
        #region Normal
        public List<Game> SearchGames() => SearchGames(new List<int>() { });
        public List<Game> SearchGames(string title) => SearchGames(new List<int>(), new List<int>(), new List<int>(), title);
        public List<Game> SearchGames(List<int> ids)
        {
            BuildGamesSearch(ids, GameOutputFormat.Normal);
            return SearchWithKey<Games_Normal>().games ?? new List<Game>();
        }
        public List<Game> SearchGames(List<int> platforms, List<int> genres, List<int> groups, string title = "", int limit = DEFAULT_LIMIT, int offset = DEFAULT_OFFSET)
        {
            BuildGamesSearch(platforms, genres, groups, GameOutputFormat.Normal, title, limit, offset);
            return SearchWithKey<Games_Normal>().games ?? new List<Game>();
        }
        #endregion
        #region Single Game
        public Game_Brief SearchGame_Brief(int id)
        {
            BuildGamesSearch(id, GameOutputFormat.Brief);
            return SearchWithKey<Game>();
        }
        public Game SearchGame(int id)
        {
            BuildGamesSearch(id, GameOutputFormat.Normal);
            return SearchWithKey<Game>();
        }
        #endregion
        #region Build Search
        private void BuildGamesSearch(int id, GameOutputFormat format)
        {
            AddSearchTerm("/games/" + id.ToString());
            AddGamesOutputFormat(format);
        }
        private void BuildGamesSearch(List<int> ids, GameOutputFormat format)
        {
            AddSearchTerm("/games");
            foreach (var id in ids)
            {
                AddSearchTermArgument("id", id.ToString());
            }
            AddGamesOutputFormat(format);
        }
        private void BuildGamesSearch(List<int> platforms, List<int> genres, List<int> groups, GameOutputFormat format, string title = "", int limit = DEFAULT_LIMIT, int offset = DEFAULT_OFFSET)
        {
            AddSearchTerm("/games");
            foreach (var platform in platforms)
            {
                AddSearchTermArgument("platform", platform.ToString());
            }
            foreach (var genre in genres)
            {
                AddSearchTermArgument("genre", genre.ToString());
            }
            foreach (var group in groups)
            {
                AddSearchTermArgument("group", group.ToString());
            }
            if (title != string.Empty)
                AddSearchTermArgument("title", title);

            AddLimitOffset(limit, offset);
            AddGamesOutputFormat(format);
        }
        #endregion
        #endregion

        #region Recent Games
        public List<int> RecentGames_ID(int age = DEFAULT_RECENT_AGE, int limit = DEFAULT_LIMIT, int offset = DEFAULT_OFFSET)
        {
            BuildRecentGamesSearch(GameOutputFormat.ID, age, limit, offset);
            return SearchWithKey<Games_ID>().games ?? new List<int>();
        }
        public List<Game_Brief> RecentGames_Brief(int age = DEFAULT_RECENT_AGE, int limit = DEFAULT_LIMIT, int offset = DEFAULT_OFFSET)
        {
            BuildRecentGamesSearch(GameOutputFormat.Brief, age, limit, offset);
            return SearchWithKey<Games_Brief>().games ?? new List<Game_Brief>();
        }
        public List<Game> RecentGames(int age = DEFAULT_RECENT_AGE, int limit = DEFAULT_LIMIT, int offset = DEFAULT_OFFSET)
        {
            BuildRecentGamesSearch(GameOutputFormat.Normal, age, limit, offset);
            return SearchWithKey<Games_Normal>().games ?? new List<Game>();
        }
        private void BuildRecentGamesSearch(GameOutputFormat format, int age = DEFAULT_RECENT_AGE, int limit = DEFAULT_LIMIT, int offset = DEFAULT_OFFSET)
        {
            AddSearchTerm("/games/recent");
            if (age != DEFAULT_RECENT_AGE)
                AddSearchTermArgument("age", age.ToString());
            AddLimitOffset(limit, offset);
            AddGamesOutputFormat(format);
        }
        #endregion

        #region Random Games
        public List<int> RandomGames_ID(int limit = DEFAULT_LIMIT)
        {
            BuildRandomGamesSearch(GameOutputFormat.ID, limit);
            return SearchWithKey<Games_ID>().games ?? new List<int>();
        }
        public List<Game_Brief> RandomGames_Brief(int limit = DEFAULT_LIMIT)
        {
            BuildRandomGamesSearch(GameOutputFormat.Brief, limit);
            return SearchWithKey<Games_Brief>().games ?? new List<Game_Brief>();
        }
        public List<Game> RandomGames(int limit = DEFAULT_LIMIT)
        {
            BuildRandomGamesSearch(GameOutputFormat.Normal, limit);
            return SearchWithKey<Games_Normal>().games ?? new List<Game>();
        }
        private void BuildRandomGamesSearch(GameOutputFormat format, int limit = DEFAULT_LIMIT)
        {
            AddSearchTerm("/games/random");
            if (limit != DEFAULT_LIMIT)
                AddSearchTermArgument("limit", limit.ToString());
            AddGamesOutputFormat(format);
        }
        #endregion

        #region Game Specific
        public List<GamePlatform> SearchGamePlatforms(int id)
        {
            AddSearchTerm("/games/" + id.ToString() + "/platforms");
            return SearchWithKey<GamePlatforms>().platforms ?? new List<GamePlatform>();
        }
        public GamePlatform SearchGamePlatform(int id, int platformId)
        {
            AddSearchTerm("/games/" + id.ToString() + "/platforms/" + platformId.ToString());
            return SearchWithKey<GamePlatform>();
        }
        public List<Screenshot> SearchGamePlatformScreenshots(int id, int platformId)
        {
            AddSearchTerm("/games/" + id.ToString() + "/platforms/" + platformId.ToString() + "/screenshots");
            return SearchWithKey<Screenshots>().screenshots ?? new List<Screenshot>();
        }
        public List<CoverGroup> SearchGamePlatformCovers(int id, int platformId)
        {
            AddSearchTerm("/games/" + id.ToString() + "/platforms/" + platformId.ToString() + "/covers");
            return SearchWithKey<CoverGroups>().cover_groups ?? new List<CoverGroup>();
        }
        #endregion

        private T SearchWithKey<T>() where T : class, new()
        {
            AddAPIKey();
            return Search<T>();
        }
        private void AddAPIKey()
        {
            AddSearchTermArgument("api_key", API_Key);
        }
        private void AddLimitOffset(int limit, int offset)
        {
            if (limit != DEFAULT_LIMIT)
                AddSearchTermArgument("limit", limit.ToString());
            if (offset != DEFAULT_OFFSET)
                AddSearchTermArgument("offset", offset.ToString());
        }
        private void AddGamesOutputFormat(GameOutputFormat format)
        {
            if (format != DEFAULT_SEARCH_OUTPUT_FORMAT)
                AddSearchTermArgument("format", format.ToString().ToLower());
        }
        private void AddRecentOutputFormat(GameOutputFormat format)
        {
            if (format != DEFAULT_RECENT_OUTPUT_FORMAT)
                AddSearchTermArgument("format", format.ToString().ToLower());
        }

        //No Use, I don't get the error JSON back so might as well not have this, just throw the error and the calling app can do with it whatever
        //Will see if I can use this somewhere
        //private T PopulateException<T>(Exception ex, string result) where T : APICallBase, new()
        //{
        //    T retClass = new T();
        //    retClass.IsError = true;
        //    retClass.Error = new Errors();
        //    return retClass;
        //}
    }
}
