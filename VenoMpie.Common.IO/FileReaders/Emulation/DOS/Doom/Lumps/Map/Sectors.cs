using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.IO.FileReaders.Emulation.DOS.Doom.Lumps.Map
{
    public class Sector : ILumpBreakdown
    {
        public short FloorHeight { get; set; }
        public short CeilingHeight { get; set; }
        public string FloorPic { get; set; }
        public string CeilingPic { get; set; }
        public short LightLevel { get; set; }
        public short SpecialSector { get; set; }
        public short Tag { get; set; }

        public Sector() { }

        public void Populate(byte[] bytes)
        {
            FloorHeight = BitConverter.ToInt16(bytes, 0);
            CeilingHeight = BitConverter.ToInt16(bytes, 2);
            FloorPic = Encoding.UTF8.GetString(bytes, 4, 8).Replace("\0", "");
            CeilingPic = Encoding.UTF8.GetString(bytes, 12, 8).Replace("\0", "");
            LightLevel = BitConverter.ToInt16(bytes, 20);
            SpecialSector = BitConverter.ToInt16(bytes, 22);
            Tag = BitConverter.ToInt16(bytes, 24);
        }
    }
    public class Sectors : LumpBreakdownBase<Sector>
    {
        protected override int LumpBreakdownSize { get; set; } = 26;

        public Sectors(Lump lump) : base(lump) { }
    }
}
