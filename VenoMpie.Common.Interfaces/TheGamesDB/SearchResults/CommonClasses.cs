using System.Xml.Serialization;
namespace VenoMpie.Common.Interfaces.TheGamesDB.SearchResults
{
    public class GameImages
    {
        [XmlElement(ElementName = "boxart")]
        public GameImage_BoxArt[] BoxArt { get; set; }
        [XmlElement(ElementName = "fanart")]
        public GameImage_V2[] Fanart { get; set; }
        [XmlElement(ElementName = "banner")]
        public GameImageBase[] Banners { get; set; }
        [XmlElement(ElementName = "screenshot")]
        public GameImage_V2[] Screenshots { get; set; }
        [XmlElement(ElementName = "clearlogo")]
        public GameImageBase[] ClearLogo { get; set; }
    }
    public class PlatformImages : GameImages
    {
        [XmlElement(ElementName = "consoleart")]
        public string[] ConsoleArt { get; set; }
        [XmlElement(ElementName = "controllerart")]
        public string[] ControllerArt { get; set; }
    }
    public class GameImageBase
    {
        [XmlAttribute(AttributeName = "width")]
        public int Width { get; set; }
        [XmlAttribute(AttributeName = "height")]
        public int Height { get; set; }
        [XmlText]
        public string Path { get; set; }
    }
    public class GameImage_BoxArt : GameImageBase
    {
        [XmlAttribute(AttributeName = "side")]
        public string Side { get; set; }
        [XmlAttribute(AttributeName = "thumb")]
        public string Thumb { get; set; }
    }
    public class GameImage_V2
    {
        [XmlElement(ElementName = "original")]
        public GameImageBase Original { get; set; }
        [XmlElement(ElementName = "thumb")]
        public string Thumb { get; set; }
    }
    public class SimilarGame
    {
        public int id { get; set; }
        public int PlatformId { get; set; }
    }
    public class SimilarGames
    {
        public int SimilarCount { get; set; }
        [XmlElement(ElementName = "Game")]
        public SimilarGame[] Games { get; set; }
    }
    public class Genre
    {
        [XmlElement(ElementName = "genre")]
        public string[] genre { get; set; }
    }
    public class Platform
    {
        public int id { get; set; }
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "alias")]
        public string Alias { get; set; }
    }
}
