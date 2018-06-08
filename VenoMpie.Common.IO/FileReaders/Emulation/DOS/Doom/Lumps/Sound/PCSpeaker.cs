using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.IO.FileReaders.Emulation.DOS.Doom.Lumps.Sound
{
    public class PCSpeaker : SingleLumpBreakdownBase
    {
        public ushort Reserved { get; set; }
        public ushort SampleCount { get; set; }
        public byte[] Data { get; set; }

        public PCSpeaker() : base() { }
        public PCSpeaker(Lump lump) : base(lump) { }

        public override void Populate()
        {
            Reserved = BitConverter.ToUInt16(TLump.Data, 0);
            SampleCount = BitConverter.ToUInt16(TLump.Data, 2);
            Data = new byte[SampleCount];
            Array.Copy(TLump.Data, 4, Data, 0, SampleCount);
        }
    }
}
