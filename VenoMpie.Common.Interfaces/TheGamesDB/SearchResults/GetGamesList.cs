using System.Xml.Serialization;
namespace VenoMpie.Common.Interfaces.TheGamesDB.SearchResults
{
    public class GetGamesList_Results
    {
        public int id { get; set; }
        public string GameTitle { get; set; }
        public string ReleaseDate { get; set; }
        public string Platform { get; set; }
    }
    [XmlRoot(ElementName="Data")]
    public class GetGamesList
    {
        [XmlElement(ElementName="Game")]
        public GetGamesList_Results[] Games;
    }
}