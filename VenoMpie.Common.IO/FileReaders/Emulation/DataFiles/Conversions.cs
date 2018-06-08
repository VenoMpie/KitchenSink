using System.IO;
using VenoMpie.Common.IO.FileReaders.Emulation.TotalDosCollection;
using VenoMpie.Common.IO.FileReaders.Emulation.TOSEC;

namespace VenoMpie.Common.IO.FileReaders.Emulation.DataFiles
{
    public class Conversions
    {
        public static void ConvertTosecTextToXML(string sourceFile, string destinationFile) => ConvertTosecTextToXML(new FileStream(sourceFile, FileMode.Open), new FileStream(destinationFile, FileMode.OpenOrCreate));
        public static void ConvertTosecTextToXML(Stream sourceStream, string destinationFile) => ConvertTosecTextToXML(sourceStream, new FileStream(destinationFile, FileMode.OpenOrCreate));
        public static void ConvertTosecTextToXML(string sourceFile, Stream destinationStream) => ConvertTosecTextToXML(new FileStream(sourceFile, FileMode.Open), destinationStream);
        public static void ConvertTDCTextToXML(string sourceFile, string destinationFile) => ConvertTDCTextToXML(new FileStream(sourceFile, FileMode.Open), new FileStream(destinationFile, FileMode.OpenOrCreate));
        public static void ConvertTDCTextToXML(Stream sourceStream, string destinationFile) => ConvertTDCTextToXML(sourceStream, new FileStream(destinationFile, FileMode.OpenOrCreate));
        public static void ConvertTDCTextToXML(string sourceFile, Stream destinationStream) => ConvertTDCTextToXML(new FileStream(sourceFile, FileMode.Open), destinationStream);
        public static void ConvertTosecTextToXML(Stream sourceStream, Stream destinationStream)
        {
            TosecDat_Text tosecText = new TosecDat_Text();
            tosecText.ReadFile(sourceStream);
            TosecDat_XML tosecXML = new TosecDat_XML()
            {
                Contents = tosecText.Contents
            };
            tosecXML.WriteFile(destinationStream);
        }
        public static void ConvertTDCTextToXML(Stream sourceStream, Stream destinationStream)
        {
            TDCDat_Text tdcText = new TDCDat_Text();
            tdcText.ReadFile(sourceStream);
            TosecDat_XML tosecXML = new TosecDat_XML()
            {
                Contents = tdcText.Contents
            };
            tosecXML.WriteFile(destinationStream);
        }
    }
}
