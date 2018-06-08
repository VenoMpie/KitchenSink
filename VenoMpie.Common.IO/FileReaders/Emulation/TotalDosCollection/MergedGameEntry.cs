using System.Collections.Generic;
using VenoMpie.Common.IO.FileReaders.Emulation.General;

namespace VenoMpie.Common.IO.FileReaders.Emulation.TotalDosCollection
{
    public class MergedGameEntry
    {
        public string Name { get; set; }
        public List<LinkedGameEntry> Games { get; set; } = new List<LinkedGameEntry>();

        public MergedGameEntry(string name) { Name = name; }
    }
}
