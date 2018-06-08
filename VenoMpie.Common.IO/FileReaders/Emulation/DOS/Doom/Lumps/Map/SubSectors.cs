using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.IO.FileReaders.Emulation.DOS.Doom.Lumps.Map
{
    public class SubSector : ILumpBreakdown
    {
        public short NumberOfSegs { get; set; }
        public short RelatedSegNumber { get; set; }

        public SubSector() { }

        public void Populate(byte[] bytes)
        {
            NumberOfSegs = BitConverter.ToInt16(bytes, 0);
            RelatedSegNumber = BitConverter.ToInt16(bytes, 2);
        }
    }
    public class SubSectors : LumpBreakdownBase<SubSector>
    {
        protected override int LumpBreakdownSize { get; set; } = 4;

        public SubSectors(Lump lump) : base(lump) { }
    }
}
