using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.IO.FileReaders.Emulation.DOS.Doom.Lumps.Map
{
    public class Map
    {
        public string MapNumber { get; set; }
        public IThings Things { get; set; }
        public ILineDefs LineDefs { get; set; }
        public SideDefs SideDefs { get; set; }
        public Vertices Vertexes { get; set; }
        public Segments Segs { get; set; }
        public SubSectors SSectors { get; set; }
        public Nodes Nodes { get; set; }
        public Sectors Sectors { get; set; }
        public Reject Reject { get; set; }
        public Lump BlockMap { get; set; }
        public Map(Lump mapLump)
        {
            MapNumber = mapLump.Name;
        }
    }
    public class Map_Hexen : Map
    {
        public Lump Behaviour { get; set; }
        public Map_Hexen(Lump mapLump) : base(mapLump) { }
    }
}
