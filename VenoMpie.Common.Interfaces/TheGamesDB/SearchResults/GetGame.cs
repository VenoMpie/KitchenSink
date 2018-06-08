using System.Xml.Serialization;
namespace VenoMpie.Common.Interfaces.TheGamesDB.SearchResults
{
    public class GetGame_Results
    {
        public int id { get; set; }
        public string GameTitle { get; set; }
        public int PlatformId { get; set; }
        public string Platform { get; set; }
        public string ReleaseDate { get; set; }
        public string Overview { get; set; }
        public string ESRB { get; set; }
        [XmlElement(ElementName = "Genres")]
        public Genre[] Genres { get; set; }
        public string Players { get; set; }
        [XmlElement(ElementName="Co-op")]
        public string CoOp { get; set; }
        public string Youtube { get; set; }
        public string Publisher { get; set; }
        public string Developer { get; set; }
        public string Rating { get; set; }
        public SimilarGames Similar { get; set; }
        public GameImages Images;
    }
    [XmlRoot(ElementName = "Data")]
    public class GetGame
    {
        public string baseImgUrl { get; set; }

        [XmlElement(ElementName = "Game")]
        public GetGame_Results[] Games;
    }
}