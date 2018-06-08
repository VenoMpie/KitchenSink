using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.IO.FileReaders
{
    /// <summary>
    /// Simple binary reader class with the addition of the Big Endian read functions
    /// </summary>
    /// <remarks>
    /// All functions just reverse the byte order that the base BinaryReader reads
    /// </remarks>
    public class EndianAwareBinaryReader : BinaryReader
    {
        public EndianAwareBinaryReader(Stream input)  : base(input) { }
        public EndianAwareBinaryReader(Stream input, Encoding encoding) : base(input, encoding) { }
        public EndianAwareBinaryReader(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen) { }
        public short ReadInt16BigEndian()
        {
            return BitConverter.ToInt16(ReverseBytes(base.ReadBytes(2)), 0);
        }
        public ushort ReadUInt16BigEndian()
        {
            return BitConverter.ToUInt16(ReverseBytes(base.ReadBytes(2)), 0);
        }
        public int ReadInt32BigEndian()
        {
            return BitConverter.ToInt32(ReverseBytes(base.ReadBytes(4)), 0);
        }
        public uint ReadUInt32BigEndian()
        {
            return BitConverter.ToUInt32(ReverseBytes(base.ReadBytes(4)), 0);
        }
        public long ReadInt64BigEndian()
        {
            return BitConverter.ToInt64(ReverseBytes(base.ReadBytes(8)), 0);
        }
        public ulong ReadUInt64BigEndian()
        {
            return BitConverter.ToUInt64(ReverseBytes(base.ReadBytes(8)), 0);
        }
        private byte[] ReverseBytes(byte[] bytes)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            return bytes;
        }
    }
}
