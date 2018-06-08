using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.IO.FileReaders.Emulation.DOS.Doom.Lumps.Map
{
    public class Box
    {
        public short Y_Upperbound { get; set; }
        public short Y_Lowerbound { get; set; }
        public short X_Upperbound { get; set; }
        public short X_Lowerbound { get; set; }

        public Box(byte[] bytes)
        {
            Y_Upperbound = BitConverter.ToInt16(bytes, 0);
            Y_Lowerbound = BitConverter.ToInt16(bytes, 2);
            X_Upperbound = BitConverter.ToInt16(bytes, 4);
            X_Lowerbound = BitConverter.ToInt16(bytes, 6);
        }
    }
    public class Node : ILumpBreakdown
    {
        public short X { get; set; }
        public short Y { get; set; }
        public short Diagonal_X { get; set; }
        public short Diagonal_Y { get; set; }
        public Box RightChild_Box { get; set; }
        public Box LeftChild_Box { get; set; }
        public ushort RightChild_SectorNumber { get; set; }
        public ushort LeftChild_SectorNumber { get; set; }

        public Node() { }

        public void Populate(byte[] bytes)
        {
            X = BitConverter.ToInt16(bytes, 0);
            Y = BitConverter.ToInt16(bytes, 2);
            Diagonal_X = BitConverter.ToInt16(bytes, 4);
            Diagonal_Y = BitConverter.ToInt16(bytes, 6);

            byte[] rcbBuffer = new byte[8];
            Array.Copy(bytes, 8, rcbBuffer, 0, 8);
            RightChild_Box = new Box(rcbBuffer);

            byte[] lcbBuffer = new byte[8];
            Array.Copy(bytes, 16, lcbBuffer, 0, 8);
            LeftChild_Box = new Box(lcbBuffer);

            RightChild_SectorNumber = BitConverter.ToUInt16(bytes, 24);
            LeftChild_SectorNumber = BitConverter.ToUInt16(bytes, 26);
        }
    }
    public class Nodes : LumpBreakdownBase<Node>
    {
        protected override int LumpBreakdownSize { get; set; } = 28;

        public Nodes(Lump lump) : base(lump) { }
    }
}
