using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.IO.FileReaders.Emulation.DOS.Doom.Lumps.Sound
{
    public class DMXGUS : SingleLumpBreakdownBase
    {
        public List<string> Patches { get; set; } = new List<string>();
        public string Comment { get; set; }

        public DMXGUS() : base() { }
        public DMXGUS(Lump lump) : base(lump) { }

        public override void Populate()
        {
            string[] splitStrings = Encoding.ASCII.GetString(TLump.Data).Split('\n');
            foreach (var item in splitStrings)
            {
                if (item.TrimStart().StartsWith("#"))
                    Comment += item + "\n";
                else
                    Patches.Add(item);
            }
        }
    }
}
