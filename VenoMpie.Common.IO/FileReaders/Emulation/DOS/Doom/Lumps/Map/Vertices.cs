using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.IO.FileReaders.Emulation.DOS.Doom.Lumps.Map
{
    public class Vertex : ILumpBreakdown
    {
        public short X { get; set; }
        public short Y { get; set; }

        public Vertex() { }

        public void Populate(byte[] bytes)
        {
            X = BitConverter.ToInt16(bytes, 0);
            Y = BitConverter.ToInt16(bytes, 2);
        }
    }
    public class Vertices : LumpBreakdownBase<Vertex>
    {
        protected override int LumpBreakdownSize { get; set; } = 4;

        public Vertices(Lump lump) : base(lump) { }
    }
}
