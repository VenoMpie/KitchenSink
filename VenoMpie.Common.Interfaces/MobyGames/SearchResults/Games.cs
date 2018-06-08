using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.Interfaces.MobyGames.SearchResults
{
    public class Game_Brief
    {
        public int game_id { get; set; }
        public string moby_url { get; set; }
        public string title { get; set; }
    }
    public class Game : Game_Brief
    {
        public string description { get; set; }
        public List<Genre> genres { get; set; }
        public double moby_score { get; set; }
        public int num_votes { get; set; }
        public string official_url { get; set; }
        public List<Platform> platforms { get; set; }
    }
    public abstract class Games_Base<T> : APICallBase where T : Game_Brief
    {
        public List<T> games { get; set; }
    }
    public class Games_Brief : Games_Base<Game_Brief> { }
    public class Games_Normal : Games_Base<Game> { }
    public class Games_ID : APICallBase
    {
        public List<int> games { get; set; }
    }
}
