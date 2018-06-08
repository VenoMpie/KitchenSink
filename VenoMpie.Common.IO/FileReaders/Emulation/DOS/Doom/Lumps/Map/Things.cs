using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.IO.FileReaders.Emulation.DOS.Doom.Lumps.Map
{
    public interface IThings : ISetLumpBreakdown { }

    public class Thing : ILumpBreakdown
    {
        public short X { get; set; }
        public short Y { get; set; }
        public ushort Angle { get; set; }
        public ushort ThingType { get; set; }
        public ushort Options { get; set; }

        public Thing() { }

        public virtual void Populate(byte[] bytes)
        {
            X = BitConverter.ToInt16(bytes, 0);
            Y = BitConverter.ToInt16(bytes, 2);
            Angle = BitConverter.ToUInt16(bytes, 4);
            ThingType = BitConverter.ToUInt16(bytes, 6);
            Options = BitConverter.ToUInt16(bytes, 8);
        }
    }
    public class Thing_Hexen : ILumpBreakdown
    {
        public ushort ThingID { get; set; }
        public short X { get; set; }
        public short Y { get; set; }
        public short Z { get; set; }
        public ushort Angle { get; set; }
        public ushort ThingType { get; set; }
        public ushort Options { get; set; }
        public byte ActionSpecial { get; set; }
        public byte ActionArgument1 { get; set; }
        public byte ActionArgument2 { get; set; }
        public byte ActionArgument3 { get; set; }
        public byte ActionArgument4 { get; set; }
        public byte ActionArgument5 { get; set; }

        public Thing_Hexen() { }

        public virtual void Populate(byte[] bytes)
        {
            ThingID = BitConverter.ToUInt16(bytes, 0);
            X = BitConverter.ToInt16(bytes, 2);
            Y = BitConverter.ToInt16(bytes, 4);
            Z = BitConverter.ToInt16(bytes, 6);
            Angle = BitConverter.ToUInt16(bytes, 8);
            ThingType = BitConverter.ToUInt16(bytes, 10);
            Options = BitConverter.ToUInt16(bytes, 12);
            ActionSpecial = bytes[14];
            ActionArgument1 = bytes[15];
            ActionArgument2 = bytes[16];
            ActionArgument3 = bytes[17];
            ActionArgument4 = bytes[18];
            ActionArgument5 = bytes[19];
        }
    }
    public class Things : LumpBreakdownBase<Thing>, IThings
    {
        protected override int LumpBreakdownSize { get; set; } = 10;

        public Things(Lump lump) : base(lump) { }
        public Things() { }

        public void SetLump(Lump lump) { TLump = lump; }
    }
    public class Things_Hexen : LumpBreakdownBase<Thing_Hexen>, IThings
    {
        protected override int LumpBreakdownSize { get; set; } = 20;

        public Things_Hexen(Lump lump) : base(lump) { }
        public Things_Hexen() { }

        public void SetLump(Lump lump) { TLump = lump; }
    }
}
