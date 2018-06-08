using System.Xml.Serialization;

namespace VenoMpie.Common.Interfaces.TheGamesDB.SearchResults
{
    public class GetPlatformGames_Results
    {
        public int id { get; set; }
        public string GameTitle { get; set; }
        public string ReleaseDate { get; set; }
    }
    [XmlRoot(ElementName = "Data")]
    public class GetPlatformGames
    {
        [XmlElement(ElementName="Game")]
        public GetPlatformGames_Results[] Games;
    }
}
