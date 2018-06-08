using System.Xml.Serialization;

namespace VenoMpie.Common.Interfaces.TheGamesDB.SearchResults
{
    [XmlRoot(ElementName = "Items")]
    public class Updates
    {
        public long Time { get; set; }
        [XmlElement(ElementName="Game")]
        public int[] Games;
    }
}
