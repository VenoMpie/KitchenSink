using System.IO;
using System.Xml.Serialization;
using VenoMpie.Common.IO.FileReaders.Emulation.DataFiles;
using VenoMpie.Common.IO.FileReaders.Emulation.DataFiles.Resources;

namespace VenoMpie.Common.IO.FileReaders.Emulation.TOSEC
{
    /// <summary>
    /// Only New School TOSEC DAT files :P
    /// </summary>
    /// <remarks>
    /// Quick and Easy Serialization and Deserialization using .NET XmlSerializer
    /// </remarks>
    public class TosecDat_XML : IAmDataFile
    {
        public datafile Contents { get; set; }

        public void ReadFile(string path)
        {
            ReadFile(new FileStream(path, FileMode.Open));
        }
        public void ReadFile(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(datafile));
                Contents = (datafile)serializer.Deserialize(reader);
            }
        }
        public void WriteFile(string path)
        {
            WriteFile(path, FileMode.OpenOrCreate);
        }
        public void WriteFile(string path, FileMode fileMode)
        {
            WriteFile(new FileStream(path, fileMode));
        }
        public void WriteFile(Stream stream)
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(datafile));
                serializer.Serialize(writer, Contents);
            }
        }
    }
}
