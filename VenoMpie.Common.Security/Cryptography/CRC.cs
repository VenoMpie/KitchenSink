using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.Security.Cryptography
{
    public class CRCEnumAttribute : Attribute
    {
        public ulong Polynomial;
        public ulong Seed;
        public byte Width;
        public CRCEnumAttribute(ulong polynomial, ulong seed, byte width)
        {
            Polynomial = polynomial;
            Seed = seed;
            Width = width;
        }
    }
    public enum CRCEnum : uint
    {
        [CRCEnumAttribute(0xEDB88320, 0xFFFFFFFF, 32)]
        CRC32,
        [CRCEnumAttribute(0x04C11DB7, 0xFFFFFFFF, 32)]
        CRC32Standard,
        [CRCEnumAttribute(0x04C11DB7, 0x00000000, 32)]
        CRC32BZip2,
        [CRCEnumAttribute(0x04C11DB7, 0xFFFFFFFF, 32)]
        CRC32MPEG,
    }

    public class CRC : HashAlgorithm
    {
        protected ulong[] LookupTable = null;

        protected ulong Polynomial { get; set; }
        protected ulong Seed { get; set; }
        protected byte Width { get; set; }
        protected ulong CurrentHash { get; set; }
        //private bool ReverseCalculation { get; set; }

        public CRC(CRCEnum crctype)
        {
            CRCEnumAttribute attr = (CRCEnumAttribute)(typeof(CRCEnum).GetMember(crctype.ToString())[0].GetCustomAttributes(typeof(CRCEnumAttribute), false)[0]);
            InitializeCtor(attr.Polynomial, attr.Seed, attr.Width);
        }
        public CRC(ulong polynomial, ulong seed, byte width)
        {
            InitializeCtor(polynomial, seed, width);
        }
        private void InitializeCtor(ulong polynomial, ulong seed, byte width)
        {
            Polynomial = polynomial;
            Seed = seed;
            Width = width;
            Initialize();
        }
        public override void Initialize()
        {
            InitializeTable();
            CurrentHash = Seed;
        }

        /// <summary>
        /// Initialize a table for the lookup method
        /// </summary>
        protected void InitializeTable()
        {
            if (LookupTable != null) return;

            LookupTable = new ulong[256];
            for (var i = 0; i < 256; i++)
            {
                LookupTable[i] = Compute((ulong)i);
            }
        }

        /// <summary>
        /// Compute a Hash byte for byte. FinalizeCompute() MUST be called after all calls to Compute have been completed
        /// </summary>
        /// <param name="value"></param>
        /// <remarks>
        /// This is useful in compression algorithms where you have to calculate the CRC of the Header in order to validate the data
        /// </remarks>
        public void Compute(byte value)
        {
            CurrentHash = (CurrentHash >> ((int)Width / 4)) ^ LookupTable[(CurrentHash & 0xFF) ^ value];
        }
        private ulong Compute(ulong value)
        {
            for (var i = 0; i < 8; i++)
            {
                if ((value & 1) == 1)
                    value = (value >> 1) ^ Polynomial;
                else
                    value >>= 1;
            }
            return value;
        }
        protected override void HashCore(byte[] array, int ibStart, int cbSize)
        {
            for (var i = ibStart; i < cbSize - ibStart; i++)
            {
                Compute(array[i]);
            }
        }

        /// <summary>
        /// Finalizes the Hash where Compute(byte) was used
        /// </summary>
        /// <remarks>
        /// This MUST be called after all calls to Compute(Byte) have been done so that the bitwise complement can be applied, converted and returned
        /// </remarks>
        public void FinalizeCompute()
        {
            HashFinal();
        }
        protected override byte[] HashFinal()
        {
            HashValue = ConvertToBigEndianBytes(BitConverter.GetBytes(~CurrentHash));
            CurrentHash = Seed;
            return HashValue;
        }

        public override int HashSize => Width;

        /// <summary>
        /// Calculate CRC for an Array of Bytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns>Uppercase CRC String</returns>
        public string CalculateCRC(byte[] bytes)
        {
            ComputeHash(bytes);
            return ToString();
        }
        /// <summary>
        /// Calculate CRC for a Stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>Uppercase CRC String</returns>
        public string CalculateCRC(Stream stream)
        {
            ComputeHash(stream);
            return ToString();
        }
        /// <summary>
        /// Converts the filePath into a FileStream and Calculate the CRC for the Stream
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>Uppercase CRC String</returns>
        public string CalculateCRC(string filePath)
        {
            using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                CalculateCRC(fs);
            }
            return ToString();
        }
        /// <summary>
        /// Convert the Calculated Hash into a String
        /// </summary>
        /// <returns>Uppercase CRC String</returns>
        public override string ToString()
        {
            string retValue = "";
            for (var i = (int)(Width / 8); i < HashValue.Length; i++)
                retValue += HashValue[i].ToString("X2").ToUpper();

            return retValue;

            //return BitConverter.ToUInt32(ConvertToBigEndianBytes(CalculatedHash), 0).ToString("X" + (HashSize / 4).ToString());
        }
        public uint ToUInt32()
        {
            return BitConverter.ToUInt32(ReverseHashValue(), 0);
        }
        public uint ToUInt16()
        {
            return BitConverter.ToUInt16(ReverseHashValue(), 0);
        }

        protected byte[] ReverseHashValue()
        {
            var hashTemp = HashValue;
            Array.Reverse(hashTemp);
            return hashTemp;
        }
        protected byte[] ConvertToBigEndianBytes(byte[] value)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(value);

            return value;
        }
    }
}
