using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.IO.FileReaders.Emulation.DOS.Doom.Lumps.Map
{
    public class Segment : ILumpBreakdown
    {
        public short FromVertix { get; set; }
        public short ToVertix { get; set; }
        public short BAMS { get; set; }
        public short LineNumber { get; set; }
        public short SegSide { get; set; }
        public short SegOffset { get; set; }

        public Segment() { }

        public void Populate(byte[] bytes)
        {
            FromVertix = BitConverter.ToInt16(bytes, 0);
            ToVertix = BitConverter.ToInt16(bytes, 2);
            BAMS = BitConverter.ToInt16(bytes, 4);
            LineNumber = BitConverter.ToInt16(bytes, 6);
            SegSide = BitConverter.ToInt16(bytes, 8);
            SegOffset = BitConverter.ToInt16(bytes, 10);
        }
    }
    public class Segments : LumpBreakdownBase<Segment>
    {
        protected override int LumpBreakdownSize { get; set; } = 12;

        public Segments(Lump lump) : base(lump) { }
    }
}
