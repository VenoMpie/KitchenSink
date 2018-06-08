using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VenoMpie.Common.IO.FileReaders.Emulation.DataFiles;
using VenoMpie.Common.IO.FileReaders.Emulation.DataFiles.Resources;

namespace VenoMpie.Common.IO.FileReaders.Emulation.TotalDosCollection
{
    public class MergedTDCDat<T> : ParsedTDCDat<T> where T : IAmDataFile, new()
    {
        public List<MergedGameEntry> MergedGames { get; private set; } = new List<MergedGameEntry>();

        public MergedTDCDat() : base() { }

        public new void ReadFile(string path)
        {
            base.ReadFile(path);
            ParseGames();
        }
        public new void ReadFile(Stream stream)
        {
            base.ReadFile(stream);
            ParseGames();
        }

        private void ParseGames()
        {
            var distinctGames = Games.Values.Select(a => a.Name).Distinct();
            foreach (var game in distinctGames)
            {
                var mergedGame = new MergedGameEntry(game);
                mergedGame.Games.AddRange(Games.Values.Where(a => a.Name == game));
                MergedGames.Add(mergedGame);
            }
        }
    }
}
