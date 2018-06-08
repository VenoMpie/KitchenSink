using System.Xml.Serialization;
namespace VenoMpie.Common.Interfaces.TheGamesDB.SearchResults
{
    public class GetPlatform_Result
    {
        public int id { get; set; }
        public string Platform { get; set; }
        [XmlElement(ElementName = "console")]
        public string Console { get; set; }
        [XmlElement(ElementName = "controller")]
        public string Controller { get; set; }
        [XmlElement(ElementName = "overview")]
        public string Overview { get; set; }
        [XmlElement(ElementName = "developer")]
        public string Developer { get; set; }
        [XmlElement(ElementName = "manufacturer")]
        public string Manufacturer { get; set; }
        [XmlElement(ElementName = "cpu")]
        public string CPU { get; set; }
        [XmlElement(ElementName = "memory")]
        public string Memory { get; set; }
        [XmlElement(ElementName = "graphics")]
        public string Graphics { get; set; }
        [XmlElement(ElementName = "sound")]
        public string Sound { get; set; }
        [XmlElement(ElementName = "display")]
        public string Display { get; set; }
        [XmlElement(ElementName = "media")]
        public string Media { get; set; }
        [XmlElement(ElementName = "maxcontrollers")]
        public string MaxControllers { get; set; }
        public string Rating { get; set; }
        public PlatformImages Images;
    }
    [XmlRoot(ElementName = "Data")]
    public class GetPlatform
    {
        public string baseImgUrl { get; set; }
        [XmlElement(ElementName = "Platform")]
        public GetPlatform_Result Platform;
    }
}