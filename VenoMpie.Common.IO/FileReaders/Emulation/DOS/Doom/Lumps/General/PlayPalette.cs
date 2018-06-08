using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.IO.FileReaders.Emulation.DOS.Doom.Lumps
{
    public class PlayPalette : SingleLumpBreakdownBase
    {
        public PlayPalette() : base() { }
        public PlayPalette(Lump lump) : base(lump) { }
        public Dictionary<int, List<Color>> Palette { get; set; } = new Dictionary<int, List<Color>>();

        public override void Populate()
        {
            if (TLump.Data.Length % 3 != 0)
                throw new Exception("Invalid Data Length for Palettes");

            byte r = 0;
            byte g = 0;
            byte b = 0;
            int currentRow = 1;

            List<Color> addList = new List<Color>();

            for (int i = 0; i <= TLump.Data.Length - 1; i++)
            {
                if ((i + 1) % 3 == 1)
                    r = TLump.Data[i];
                if ((i + 1) % 3 == 2)
                    g = TLump.Data[i];
                if ((i + 1) % 3 == 0)
                {
                    b = TLump.Data[i];
                    addList.Add(Color.FromArgb(r, g, b));
                    r = 0;
                    g = 0;
                    b = 0;
                }
                if ((i + 1) % 768 == 0)
                {
                    Palette.Add(currentRow, addList);
                    addList = new List<Color>();
                    currentRow += 1;
                }
            }
        }
    }
}
