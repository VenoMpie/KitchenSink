using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.IO.FileReaders.Emulation.DOS.Doom.Lumps.Map
{
    [Flags]
    public enum LineDefFlagsEnum : ushort
    {
        Blocking = 0x0001,
        BlockMonsters = 0x0002,
        TwoSided = 0x0004,
        DontPegTop = 0x0008,
        DontPegBottom = 0x0010,
        Secret = 0x0020,
        SoundBlock = 0x0040,
        DontDraw = 0x0080,
        Mapped = 0x0100,
    }
    public interface ILineDefs : ISetLumpBreakdown { }

    public class LineDef : ILumpBreakdown
    {
        public short FromVertix { get; set; }
        public short ToVertix { get; set; }
        public short Flags { get; set; }
        public short LineType { get; set; }
        public short SectorTag { get; set; }
        public short SideDef_Right { get; set; }
        public short SideDef_Left { get; set; }

        public LineDef() { }

        public void Populate(byte[] bytes)
        {
            FromVertix = BitConverter.ToInt16(bytes, 0);
            ToVertix = BitConverter.ToInt16(bytes, 2);
            Flags = BitConverter.ToInt16(bytes, 4);
            LineType = BitConverter.ToInt16(bytes, 6);
            SectorTag = BitConverter.ToInt16(bytes, 8);
            SideDef_Right = BitConverter.ToInt16(bytes, 10);
            SideDef_Left = BitConverter.ToInt16(bytes, 12);
        }
    }
    public class LineDef_Hexen : ILumpBreakdown
    {
        public short FromVertix { get; set; }
        public short ToVertix { get; set; }
        public short Flags { get; set; }
        public byte ActionSpecial { get; set; }
        public byte ActionArgument1 { get; set; }
        public byte ActionArgument2 { get; set; }
        public byte ActionArgument3 { get; set; }
        public byte ActionArgument4 { get; set; }
        public byte ActionArgument5 { get; set; }
        public short SideDef_Right { get; set; }
        public short SideDef_Left { get; set; }

        public LineDef_Hexen() { }

        public void Populate(byte[] bytes)
        {
            FromVertix = BitConverter.ToInt16(bytes, 0);
            ToVertix = BitConverter.ToInt16(bytes, 2);
            Flags = BitConverter.ToInt16(bytes, 4);
            ActionSpecial = bytes[6];
            ActionArgument1 = bytes[7];
            ActionArgument2 = bytes[8];
            ActionArgument3 = bytes[9];
            ActionArgument4 = bytes[10];
            ActionArgument5 = bytes[11];
            SideDef_Right = BitConverter.ToInt16(bytes, 12);
            SideDef_Left = BitConverter.ToInt16(bytes, 14);
        }
    }
    public class LineDefs : LumpBreakdownBase<LineDef>, ILineDefs
    {
        protected override int LumpBreakdownSize { get; set; } = 14;

        public LineDefs(Lump lump) : base(lump) { }
        public LineDefs() { }

        public void SetLump(Lump lump) { TLump = lump; }
    }
    public class LineDefs_Hexen : LumpBreakdownBase<LineDef_Hexen>, ILineDefs
    {
        protected override int LumpBreakdownSize { get; set; } = 16;

        public LineDefs_Hexen(Lump lump) : base(lump) { }
        public LineDefs_Hexen() { }

        public void SetLump(Lump lump) { TLump = lump; }
    }
}
