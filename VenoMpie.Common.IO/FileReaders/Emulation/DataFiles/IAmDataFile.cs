using System.IO;
using VenoMpie.Common.IO.FileReaders.Emulation.DataFiles.Resources;

namespace VenoMpie.Common.IO.FileReaders.Emulation.DataFiles
{
    public interface IAmDataFile
    {
        datafile Contents { get; set; }
        void ReadFile(string path);
        void ReadFile(Stream stream);
        void WriteFile(string path);
        void WriteFile(string path, FileMode fileMode);
        void WriteFile(Stream stream);
    }
}
