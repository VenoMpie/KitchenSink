using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Globalization;

/// <summary>
/// I have to comment this extensively as one tends to forget quickly what comes where and why especially the switch between little and big endian for the LoadAddress
/// </summary>
/// <remarks>
/// Check out the File format specifications
/// </remarks>
/// <see cref="http://cpansearch.perl.org/src/LALA/Audio-SID-3.11/SID_file_format.txt"/>
namespace VenoMpie.Common.IO.FileReaders.Emulation.C64
{
    public enum SIDByteType : uint
    {
        Unknown = 0,
        PSID = 0x50534944,
        RSID = 0x52534944,
    }
    public enum Player : byte
    {
        BuiltInMusicPlayer = 0x00,
        ComputeSIDPlayer = 0x01
    }
    public enum Compatibility : byte
    {
        C64Compatible = 0x00,
        PlaySID_C64Basic = 0x01
    }
    public enum VideoStandardClock : byte
    {
        Unknown = 0x00,
        PAL = 0x01,
        NTSC = 0x02,
        PALandNTSC = 0x03
    }
    public enum SIDVersion : byte
    {
        Unknown = 0x00,
        MOS6581 = 0x01,
        MOS8580 = 0x02,
        MOS6581andMOS8580 = 0x03
    }
    #region SID File Definition Classes
    /// <summary>
    /// Basic SID Header Class
    /// </summary>
    public class SIDBasicHeader
    {
        /// <summary>
        /// File Identifier
        /// </summary>
        /// <remarks>
        /// 0x50534944 for PSID
        /// 0x52534944 for RSID
        /// We also have an Unknown in there for if it's not a valid SID File, but we can always also use some of the Common.IO.FileSignatures to determine this ... but meh
        /// </remarks>
        public SIDByteType FileType { get; set; }
        /// <summary>
        /// SID File Version
        /// </summary>
        /// <remarks>
        /// 0x0001 = v1
        /// 0x0002 = v2
        /// </remarks>
        public ushort Version { get; set; }

        public SIDBasicHeader() { }
    }
    /// <summary>
    /// SID v1 Header Class
    /// </summary>
    public class SIDHeader_v1 : SIDBasicHeader
    {
        /// <summary>
        /// Data Offset
        /// </summary>
        /// <remarks>
        /// v1 Data starts at 0x0076 while v2 starts at 0x007C
        /// </remarks>
        public ushort DataOffset { get; set; }
        /// <summary>
        /// C64 Memory Start Location in which the Binary Data must be placed
        /// </summary>
        /// <remarks>
        /// If this is 0 then the actual Load Address is the first 2 bytes of data in Little Endian
        /// For RSID this must ALWAYS be 0
        /// We must convert to Little Endian if used on a Real C64 Environment as we read Big Endian
        /// </remarks>
        public ushort LoadAddress { get; set; }
        /// <summary>
        /// C64 Memory Start Location Read from the first 2 bytes of data in Little Endian
        /// </summary>
        /// <remarks>
        /// If the LoadAddress is 0 then the data are stored in the Original C64 Binary Format.
        /// Load Address is then the first 2 bytes of the binary data
        /// </remarks>
        public ushort LoadAddressOriginalC64BinaryFormat { get; set; }
        /// <summary>
        /// Start Address of Machine Code Subroutine to Initialise the song
        /// </summary>
        /// <remarks>
        /// This MAY NOT fall within the ROM/IO Memory Region (0x0000-0x07E8, 0xA000-0xBFFF and 0xD000-0xFFFF)
        /// If C64 Basic Flag is set then this MUST be 0
        /// </remarks>
        public ushort InitAddress { get; set; }
        /// <summary>
        /// Continuous Play Address
        /// </summary>
        /// <remarks>
        /// Address of the Machine Code Subroutine Used to produce a continuous sound
        /// Must be 0 for RSID
        /// </remarks>
        public ushort PlayAddress { get; set; }
        /// <summary>
        /// Number of Songs (or Sound Effects) in the File. 1-256
        /// </summary>
        public ushort Songs { get; set; }
        /// <summary>
        /// Default Starting Song
        /// </summary>
        public ushort StartSong { get; set; }
        /// <summary>
        /// Speed of the Songs
        /// </summary>
        /// <remarks>
        /// Each bit represents the speed of the corresponding tune (i.e. bit 0 is the speed of the first song)
        /// If there are more than 32 tunes then the bit for tune 32 is also used for the rest
        /// </remarks>
        public uint Speed { get; set; }
        /// <summary>
        /// SID Title
        /// </summary>
        /// <remarks>
        /// Title ... self explanatory? :P
        /// This string is 32 bytes long but can only have 31 characters as it is NULL terminated
        /// </remarks>
        public string Title { get; set; }
        /// <summary>
        /// SID Author
        /// </summary>
        /// <remarks>
        /// This string is 32 bytes long but can only have 31 characters as it is NULL terminated
        /// </remarks>
        public string Author { get; set; }
        /// <summary>
        /// Copyrighted By
        /// </summary>
        /// <remarks>
        /// This string is 32 bytes long but can only have 31 characters as it is NULL terminated
        /// </remarks>
        public string Copyright { get; set; }
    }
    /// <summary>
    /// SID v2 Header Class
    /// </summary>
    public class SIDHeader_v2 : SIDHeader_v1
    {
        /// <summary>
        /// 16 bit Flags
        /// </summary>
        /// <remarks>
        /// Bit 0 is for the Music Player
        /// 0 = built-in music player, 1 = Compute!'s Sidplayer MUS data, music player must be merged
        /// 
        /// Bit 1 is to indicate PlaySID specific
        /// 0 = C64 compatible, 1 = PlaySID specific (PSID v2NG)/C64 BASIC flag (RSID)
        /// 
        /// Bit 2-3 is to indicate the Video Clock Speed
        /// 00 = Unknown,
        /// 01 = PAL,
        /// 10 = NTSC,
        /// 11 = PAL and NTSC.
        /// 
        /// Bit 4-5 is to indicate the Video Standard (MOS Chip Version)
        /// 00 = Unknown,
        /// 01 = MOS6581,
        /// 10 = MOS8580,
        /// 11 = MOS6581 and MOS8580.
        /// 
        /// Bit 6-15 are reserved
        /// </remarks>
        public ushort Flags { get; set; }
        /// <summary>
        /// Determining Free Memory Ranges
        /// </summary>
        /// <remarks>
        /// If not 0x00 or 0xFF then this value determines the free pages
        /// </remarks>
        public byte StartPage { get; set; }
        /// <summary>
        /// Determining Number of Free Pages After StartPage
        /// </summary>
        /// <remarks>
        /// If StartPage is 0 then this must be 0
        /// </remarks>
        public byte PageLength { get; set; }
        /// <summary>
        /// Reserved Field
        /// </summary>
        /// <remarks>
        /// What can I say? :P
        /// </remarks>
        public ushort Reserved { get; set; }

        // Enums Used for the bit flags so that we can more easily work with it
        public Player Player { get; set; }
        public Compatibility Compatibility { get; set; }
        public VideoStandardClock VideoStandardClock { get; set; }
        public SIDVersion SIDVersion { get; set; }
    }
    #endregion

    /// <summary>
    /// Basic Header Class that reads through SID files. Used to identify which Reader (v1 or v2) class to use to read the file
    /// </summary>
    /// <remarks>
    /// Only for the basic SID Header. Use this to Determine which SIDHeaderReader class to use
    /// </remarks>
    public class SIDBasicHeaderReader : IDisposable
    {
        protected Stream sidStream;
        protected Encoding encoding = Encoding.UTF8;
        // SIDs are Big Endian ... the .NET BinaryReader only reads LittleEndian so we have to reverse the bytes, thus the Big Endian BinaryReader :P
        protected EndianAwareBinaryReader binaryReader;

        // All SID headers have atleast the 4 byte file type and 2 byte version signature
        protected virtual ushort HeaderLength { get { return 6; } }

        public virtual SIDBasicHeader Header { get; set; }
        public SIDBasicHeaderReader(string path)
        {
            sidStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        }
        public SIDBasicHeaderReader(Stream stream)
        {
            sidStream = stream;
        }

        public virtual void ReadSidHeader()
        {
            Header = FillHeader<SIDBasicHeader>();
        }
        protected virtual T FillHeader<T>() where T : SIDBasicHeader, new()
        {
            T retValue = new T();

            if (HeaderLength > sidStream.Length)
                return null;

            byte[] headerBuff = new byte[HeaderLength];
            sidStream.Read(headerBuff, 0, HeaderLength);
            binaryReader = new EndianAwareBinaryReader(new MemoryStream(headerBuff), encoding);
            string fileType = Encoding.ASCII.GetString(binaryReader.ReadBytes(4));
            retValue.FileType = (SIDByteType)(Enum.IsDefined(typeof(SIDByteType), fileType) ? Enum.Parse(typeof(SIDByteType), fileType) : SIDByteType.Unknown);
            //if (retValue.FileType == SidByteType.Unknown) throw new Exception("Not a valid SID File"); // To throw or not to throw :P
            retValue.Version = binaryReader.ReadUInt16BigEndian();
            return retValue;
        }
        public void Dispose()
        {
            sidStream.Close();
        }
    }

    /// <summary>
    /// Reads through v1 SID files.
    /// </summary>
    /// <remarks>
    /// Only for SID v1. Use the SIDBasicHeader Class to determine which reader to use. This is for Version = 0x0001
    /// </remarks>
    public class SIDv1Reader : SIDBasicHeaderReader
    {
        // We set the header length so that we can atleast read the basic header of the SID file (v1)
        // All SID headers have the byte characteristics of SIDHeader_v1
        // Including 96 bytes for the fixed size of the SidName, Author and Copyright which are each 32 bytes long
        // We also include the size of a word for if we have to read the loadAddress from the data
        protected override ushort HeaderLength { get { return 120; } }

        public new SIDHeader_v1 Header { get; set; }

        public SIDv1Reader(string path) : base(path) { }
        public SIDv1Reader(Stream stream) : base(stream) { }

        public override void ReadSidHeader()
        {
            Header = FillHeader<SIDHeader_v1>();
        }
        protected new T FillHeader<T>() where T : SIDHeader_v1, new()
        {
            T retValue = new T();

            retValue = base.FillHeader<T>();
            if (retValue == null) return null;

            retValue.DataOffset = binaryReader.ReadUInt16BigEndian();
            retValue.LoadAddress = binaryReader.ReadUInt16BigEndian();
            retValue.InitAddress = binaryReader.ReadUInt16BigEndian();
            retValue.PlayAddress = binaryReader.ReadUInt16BigEndian();
            retValue.Songs = binaryReader.ReadUInt16BigEndian();
            retValue.StartSong = binaryReader.ReadUInt16BigEndian();
            retValue.Speed = binaryReader.ReadUInt32BigEndian();
            retValue.Title = Encoding.UTF8.GetString(binaryReader.ReadBytes(32)).Replace("\0", ""); //Replace the null terminations
            retValue.Author = Encoding.UTF8.GetString(binaryReader.ReadBytes(32)).Replace("\0", "");
            retValue.Copyright = Encoding.UTF8.GetString(binaryReader.ReadBytes(32)).Replace("\0", "");
            if (retValue.Version == 0x01 && retValue.LoadAddress == 0x0)
            {
                retValue.LoadAddressOriginalC64BinaryFormat = binaryReader.ReadUInt16();
            }
            return retValue;
        }

        public virtual byte[] ReadSidData(ushort offset)
        {
            return ReadSidData(offset, (int)sidStream.Length - offset);
        }
        public virtual byte[] ReadSidData(ushort offset, int length)
        {
            sidStream.Seek(offset, SeekOrigin.Begin);
            byte[] data = new byte[length];
            sidStream.Read(data, 0, length);
            return data;
        }
    }
    /// <summary>
    /// Reads through v2 SID files.
    /// </summary>
    /// <remarks>
    /// Only for SID v2. Use the SIDBasicHeader Class to determine which reader to use. This is for Version = 0x0002
    /// </remarks>
    public class SIDv2Reader : SIDv1Reader
    {
        // This is specific for Sid Header v2 as it contains 4 additional fields (2 bytes and 2 words)
        // We also include the size of a word for if we have to read the loadAddress from the data
        protected override ushort HeaderLength { get { return 126; } }

        public new SIDHeader_v2 Header { get; set; }
        public SIDv2Reader(string path) : base(path) { }
        public SIDv2Reader(Stream stream) : base(stream) { }

        public override void ReadSidHeader()
        {
            Header = FillHeader<SIDHeader_v2>();
        }
        protected new T FillHeader<T>() where T : SIDHeader_v2, new()
        {
            T retValue = new T();

            retValue = base.FillHeader<T>();
            if (retValue == null) return null;

            retValue.Flags = binaryReader.ReadUInt16BigEndian();
            retValue.StartPage = binaryReader.ReadByte();
            retValue.PageLength = binaryReader.ReadByte();
            retValue.Reserved = binaryReader.ReadUInt16BigEndian();
            if (retValue.Version == 0x02 && retValue.LoadAddress == 0x0)
            {
                retValue.LoadAddressOriginalC64BinaryFormat = binaryReader.ReadUInt16();
            }
            retValue.Player = (Player)((retValue.Flags >> 7) & 0x1);
            retValue.Compatibility = (Compatibility)((retValue.Flags >> 6) & 0x1);
            retValue.VideoStandardClock = (VideoStandardClock)((retValue.Flags >> 4) & 0x3);
            retValue.SIDVersion = (SIDVersion)((retValue.Flags >> 2) & 0x3);
            return retValue;
        }
    }
}
