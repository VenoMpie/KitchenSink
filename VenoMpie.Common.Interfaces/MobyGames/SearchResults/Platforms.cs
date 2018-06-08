using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.Interfaces.MobyGames.SearchResults
{
    public class Platform
    {
        public int platform_id { get; set; }
        public string platform_name { get; set; }
    }
    public class GamePlatform : Platform
    {
        public string first_release_date { get; set; }
        public List<MBAttribute> attributes { get; set; }
        public int game_id { get; set; }
        //TODO: Check this out
        public List<string> patches { get; set; }
        //TODO: Check this out
        public List<string> ratings { get; set; }
        public List<Release> releases { get; set; }
    }
    public class Platforms : APICallBase
    {
        public List<Platform> platforms { get; set; }
    }
    public class GamePlatforms : APICallBase
    {
        public List<GamePlatform> platforms { get; set; }
    }
}
