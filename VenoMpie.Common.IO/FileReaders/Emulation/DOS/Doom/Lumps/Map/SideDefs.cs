using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.IO.FileReaders.Emulation.DOS.Doom.Lumps.Map
{
    public class SideDef : ILumpBreakdown
    {
        public short X_Offset { get; set; }
        public short Y_Offset { get; set; }
        public string UpperTexture { get; set; }
        public string LowerTexture { get; set; }
        public string MiddleTexture { get; set; }
        public short SectorReference { get; set; }

        public SideDef() { }

        public void Populate(byte[] bytes)
        {
            X_Offset = BitConverter.ToInt16(bytes, 0);
            Y_Offset = BitConverter.ToInt16(bytes, 2);
            UpperTexture = Encoding.UTF8.GetString(bytes, 4, 8).Replace("\0", "");
            LowerTexture = Encoding.UTF8.GetString(bytes, 12, 8).Replace("\0", "");
            MiddleTexture = Encoding.UTF8.GetString(bytes, 20, 8).Replace("\0", "");
            SectorReference = BitConverter.ToInt16(bytes, 28);
        }
    }
    public class SideDefs : LumpBreakdownBase<SideDef>
    {
        protected override int LumpBreakdownSize { get; set; } = 30;

        public SideDefs(Lump lump) : base(lump) { }
    }
}
