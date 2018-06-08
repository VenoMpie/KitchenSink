using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VenoMpie.Common.IO.FileReaders.Emulation.DOS.Doom.Lumps
{
    public interface ILumpBreakdown
    {
        void Populate(byte[] bytes);
    }
    public interface ISingleLumpBreakdown
    {
        void Populate();
    }
    public interface ISetLumpBreakdown : ISingleLumpBreakdown
    {
        void SetLump(Lump lump);
    }
    public abstract class SingleLumpBreakdownBase : ISingleLumpBreakdown
    {
        protected Lump TLump { get; set; }

        public SingleLumpBreakdownBase() { }

        public SingleLumpBreakdownBase(Lump lump)
        {
            TLump = lump;
            Populate();
        }
        abstract public void Populate();
    }
    public abstract class LumpBreakdownBase<T> where T : ILumpBreakdown, new()
    {
        protected abstract int LumpBreakdownSize { get; set; }

        protected Lump TLump { get; set; }
        public List<T> ObjectList { get; set; } = new List<T>();

        public LumpBreakdownBase() { }
        public LumpBreakdownBase(Lump lump)
        {
            TLump = lump;
            Populate();
        }
        public void Populate()
        {
            for (int i = 0; i < TLump.Data.Length; i += LumpBreakdownSize)
            {
                byte[] buffer = new byte[LumpBreakdownSize];
                Array.Copy(TLump.Data, i, buffer, 0, LumpBreakdownSize);
                var obj = new T();
                obj.Populate(buffer);
                ObjectList.Add(obj);
            }
        }
    }

    public class Lump
    {
        public uint Position { get; set; }
        public uint Size { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }

        public Lump(BinaryReader reader)
        {
            Position = reader.ReadUInt32();
            Size = reader.ReadUInt32();
            Name = Encoding.UTF8.GetString(reader.ReadBytes(8)).Replace("\0", "");

            long previousPosition = reader.BaseStream.Position;
            reader.BaseStream.Position = Position;
            Data = reader.ReadBytes((int)Size);
            reader.BaseStream.Position = previousPosition;
        }
    }
}
