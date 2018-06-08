using System.Xml.Serialization;

namespace VenoMpie.Common.Interfaces.TheGamesDB.SearchResults
{
    [XmlRoot(ElementName = "Data")]
    public class GetPlatformsList
    {
        public string basePlatformUrl { get; set; }
        [XmlArray(ElementName = "Platforms")]
        public Platform[] Platforms;
    }
}
