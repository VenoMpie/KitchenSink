using System.Xml.Serialization;

namespace VenoMpie.Common.Interfaces.TheGamesDB.SearchResults
{
    public class PlatformGames_Results
    {
        public int id { get; set; }
        public string GameTitle { get; set; }
        public string thumb { get; set; }
    }
    /// <summary>
    /// Must be searched with a platform name
    /// </summary>
    [XmlRoot(ElementName = "Data")]
    public class PlatformGames
    {
        [XmlElement(ElementName="Game")]
        public PlatformGames_Results[] Games;
    }
}
