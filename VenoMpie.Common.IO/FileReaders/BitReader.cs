using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.IO.FileReaders
{
    /// <summary>
    /// BitReader class that takes a byte, shifts the value and returns the bit
    /// </summary>
    /// <remarks>
    /// It's not complete yet (or atleast not tested ... I wanted to use it for the SIDReader class with all the funny 3rd bit = x 4th bit = y etc. but realised that shifting the bits there is a lot less hassle)
    /// </remarks>
    public class BitReader
    {
        int _bit;
        byte _currentByte;
        Stream _stream;
        public BitReader(Stream stream)
        {
            _stream = stream;
        }
        public BitReader(byte readbyte)
        {
            _currentByte = readbyte;
        }
        public bool? ReadBit(bool bigEndian = false)
        {
            if (_bit == 8)
            {
                if (_stream != null)
                {
                    var r = _stream.ReadByte();
                    if (r == -1) return null;
                    _bit = 0;
                    _currentByte = (byte)r;
                }
                else
                    return null;
            }
            bool value = ReadBits(_bit, bigEndian) > 0;
            _bit++;
            return value;
        }
        public int? ReadBits(int shift, bool bigEndian = false)
        {
            if (!bigEndian)
                return (_currentByte & (1 << shift));
            else
                return (_currentByte & (1 << (7 - shift)));
        }
    }
}
