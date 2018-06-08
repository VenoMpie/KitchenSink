using System.Xml.Serialization;

namespace VenoMpie.Common.Interfaces.TheGamesDB.SearchResults
{
    [XmlRoot(ElementName = "Data")]
    public class GetArt
    {
        public string baseImgUrl { get; set; }
        [XmlElement(ElementName="Images")]
        public GameImages[] Images;
    }
}
