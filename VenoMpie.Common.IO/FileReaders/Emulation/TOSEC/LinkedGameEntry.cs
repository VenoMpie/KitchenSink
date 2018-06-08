using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VenoMpie.Common.IO.FileReaders.Emulation.DataFiles.Resources;
using VenoMpie.Common.IO.FileReaders.Emulation.General;

namespace VenoMpie.Common.IO.FileReaders.Emulation.TOSEC
{
    public class LinkedGameEntry : GameEntry, ILinkedGameEntry
    {
        public game LinkedGame { get; private set; }
        public LinkedGameEntry(game game) : base(game.name) { }
        public LinkedGameEntry(game game, string regEx) : base(game.name, regEx) { LinkedGame = game; }
    }
}
