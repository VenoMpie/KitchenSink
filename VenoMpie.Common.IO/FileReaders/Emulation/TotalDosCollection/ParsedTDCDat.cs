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
    public class ParsedTDCDat<T> : IAmDataFile where T : IAmDataFile, new()
    {
        private T Reader;
        public datafile Contents { get => Reader.Contents; set => Reader.Contents = value; }
        public Dictionary<string, LinkedGameEntry> Games { get; private set; } = new Dictionary<string, LinkedGameEntry>();

        public ParsedTDCDat()
        {
            Reader = new T();
        }
        public void ReadFile(string path)
        {
            Reader.ReadFile(path);
            ParseGames();
        }
        public void ReadFile(Stream stream)
        {
            Reader.ReadFile(stream);
            ParseGames();
        }
        public void WriteFile(string path) => Reader.WriteFile(path);
        public void WriteFile(string path, FileMode fileMode) => Reader.WriteFile(path, fileMode);
        public void WriteFile(Stream stream) => Reader.ReadFile(stream);
        private void ParseGames()
        {
            var regEx = new GameEntry().BuildRegex();

            foreach (var game in Contents.game)
            {
                var linkedGame = new LinkedGameEntry(game, regEx);
                Games.Add(game.name, linkedGame);
            }
        }
    }
}
