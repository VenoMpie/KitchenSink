using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace VenoMpie.Common.IO.FileReaders.Compression.SRR
{
    /// <summary>
    /// Rar Block Enum
    /// </summary>
    /// <remarks>
    /// SRR blocks are based of RAR and also uses some of the RAR block types
    /// </remarks>
    /// <see cref="http://forensicswiki.org/wiki/RAR"/>
    public enum SRRBlockType : byte
    {
        Unknown = 0x00,
        SrrHeader = 0x69,
        SrrStoredFile = 0x6A,
        SrrOSOHashBlock = 0x6B,
        SrrPadBlock = 0x6C,
        SrrRarFile = 0x71,
        RarHeader = 0x72,
        RarArchive = 0x73,
        RarPackedFile = 0x74,
        RarComment = 0x75,
        RarExtraInfo = 0x76,
        RarSubBlock = 0x77,
        RarRecovery = 0x78,
        RarNewSub = 0x7A,
        RarTerminator = 0x7B,
    }

    /// <summary>
    /// Generic RAR Block
    /// </summary>
    /// <remarks>
    /// RAR Blocks always have the properties of this class so it's basically an abstract but we need to be able to instantiate to determine other blocks from the type
    /// </remarks>
    public class RarBlock
    {
        /// We don't put the Additional Block Size in the base as like with normal RAR as SRR doesn't have the additional data (some headers include the long block flag for it but will fail if read)
        /// I put the enum here for derived flags
        [Flags()]
        public enum FlagValues : ushort
        {
            LongBlock = 0x8000
        }

        protected Encoding encoding = Encoding.UTF8;

        public ushort Crc { get; set; }
        public byte RawType { get; set; }
        public SRRBlockType Type { get; set; }
        public ushort Flags { get; set; }
        public ushort BlockSize { get; set; }
        public uint AdditionalSize { get; set; }
        public byte[] BlockData { get; set; }
        public uint TotalBlockSize { get { return BlockSize + AdditionalSize; } }
        public int TotalBlockSizeLessHeader { get { return (BlockSize + (int)AdditionalSize) - 7; } }

        public RarBlock(BinaryReader reader)
        {
            ReadBlock(reader);
        }
        public RarBlock(RarBlock readBlock)
        {
            ReadBlock(readBlock);
            ReadData();
        }
        public virtual void ReadBlock(BinaryReader reader)
        {
            Crc = reader.ReadUInt16();
            RawType = reader.ReadByte();
            Type = Enum.IsDefined(typeof(SRRBlockType), RawType) ? (SRRBlockType)RawType : SRRBlockType.Unknown;
            Flags = reader.ReadUInt16();
            BlockSize = reader.ReadUInt16();

            //RarPackedFiles have the additional size but the data isn't there
            //SRRs also have the recovery blocks also without data
            if ((Flags & (ushort)FlagValues.LongBlock) != 0
                && Type != SRRBlockType.RarPackedFile
                && Type != SRRBlockType.RarRecovery
                && Type != SRRBlockType.RarSubBlock
                && Type != SRRBlockType.RarNewSub)
            {
                AdditionalSize = reader.ReadUInt32();

                //We remove 4 bytes for the sizeof(uint)
                BlockData = reader.ReadBytes(TotalBlockSizeLessHeader - 4);
            }
            else if (TotalBlockSizeLessHeader > 0)
                BlockData = reader.ReadBytes(TotalBlockSizeLessHeader);

            /* In Some RARE cases e.g. The.Matrix.1999.iNTERNAL.DVDRip.XviD.AC3-XviK.srr and Apache.Longbow.GoG.Classic-I_KnoW.srr
            the SRR seems to be incorrect (according to the spec it will have no flag in a previous SRR block when data should be read)
            In the cases above the flag is set to 1 indicating that data shouldn't be read yet one must?
            From the SRR File Spec:
              1. If end of file encountered then terminate archive processing,
                 else read 7 bytes into fields HEAD_CRC, HEAD_TYPE, HEAD_FLAGS,
                 HEAD_SIZE.
              2. Check HEAD_TYPE.
                 if HEAD_TYPE == 0x74 or 0x77 or 0x78
                   read file header ( first 7 bytes already read )
                   no archived data to be read (unless no flag is set in a previous SRR
                   RAR block to indicate the recovery records are removed)
                 else
                   read HEAD_SIZE - 7 bytes
                   if (HEAD_FLAGS & 0x8000)
                     read ADD_SIZE bytes
                     read or skip sizeof(BLOCK) - HEAD_SIZE bytes
              3. go to 1.*/
            if (Type == SRRBlockType.RarPackedFile || Type == SRRBlockType.RarSubBlock || Type == SRRBlockType.RarRecovery || Type == SRRBlockType.RarNewSub)
            {
                if (reader.BaseStream.Position + 3 >= reader.BaseStream.Length)
                {
                    return;
                }
                byte[] peekBytes = reader.ReadBytes(3);
                reader.BaseStream.Position = reader.BaseStream.Position - 3;
                if (!Enum.IsDefined(typeof(SRRBlockType), peekBytes[2]))
                {
                    AdditionalSize = BitConverter.ToUInt32(BlockData, 0);
                    byte[] additionalBytes = reader.ReadBytes((int)AdditionalSize);
                    byte[] newBlockData = new byte[BlockData.Length + additionalBytes.Length];
                    Array.Copy(BlockData, newBlockData, BlockData.Length);
                    Array.Copy(additionalBytes, 0, newBlockData, BlockData.Length, additionalBytes.Length);
                    BlockData = newBlockData;
                }
            }
        }
        public virtual void ReadBlock(RarBlock readBlock)
        {
            Crc = readBlock.Crc;
            RawType = readBlock.RawType;
            Type = readBlock.Type;
            Flags = readBlock.Flags;
            BlockSize = readBlock.BlockSize;
            AdditionalSize = readBlock.AdditionalSize;
            BlockData = readBlock.BlockData;
        }
        public virtual void ReadData() { }
    }

    /// <summary>
    /// SRR Header Block.
    /// </summary>
    public class SrrHeaderBlock : RarBlock
    {
        [Flags()]
        public new enum FlagValues : ushort
        {
            AppNamePresent = 0x1
        }

        public string AppName { get; set; }
        public ushort AppNameSize { get; set; }

        public SrrHeaderBlock(RarBlock readBlock) : base(readBlock) { }
        public SrrHeaderBlock(BinaryReader reader) : base(reader) { }
        public override void ReadData()
        {
            if ((Flags & (ushort)FlagValues.AppNamePresent) != 0)
            {
                AppNameSize = BitConverter.ToUInt16(BlockData, 0);
                AppName = encoding.GetString(BlockData, 2, AppNameSize);
            }
            else
                AppName = "Unknown";
        }
    }

    /// <summary>
    /// SRR Stored File Block
    /// </summary>
    public class SrrStoredFileBlock : RarBlock
    {
        [Flags()]
        public new enum FlagValues : ushort
        {
            PathsSaved = 0x2,
        }

        public string FileName { get; set; }
        public ushort FileNameSize { get; set; }
        public string FileContents { get; set; }
        public byte[] FileContentsBinary { get; set; }

        public SrrStoredFileBlock(RarBlock readBlock) : base(readBlock) { }
        public SrrStoredFileBlock(BinaryReader reader) : base(reader) { }
        public override void ReadData()
        {
            FileNameSize = BitConverter.ToUInt16(BlockData, 0);
            FileName = encoding.GetString(BlockData, 2, FileNameSize);
            //We remove 6 bytes for the sizeof(uint) -> additional size and sizeof(ushort) -> filename size
            byte[] buffer = new byte[TotalBlockSizeLessHeader - FileNameSize - 6];
            Array.Copy(BlockData, 2 + FileNameSize, buffer, 0, buffer.Length);
            FileContentsBinary = buffer;

            //The stored files are so small so I read them here.
            //Useful if you want to display the NFO or read the SFV
            FileContents = Encoding.GetEncoding(437).GetString(FileContentsBinary);
        }
    }

    /// <summary>
    /// SRR Rar File Block
    /// </summary>
    public class SrrRarFileBlock : RarBlock
    {
        [Flags()]
        public new enum FlagValues : ushort
        {
            RecoveryBlocksRemoved = 0x1,
            PathsSaved = 0x2
        }

        public string FileName { get; set; }
        public ushort FileNameSize { get; set; }

        public SrrRarFileBlock(BinaryReader reader) : base(reader) { }
        public SrrRarFileBlock(RarBlock readBlock) : base(readBlock) { }
        public override void ReadData()
        {
            FileNameSize = BitConverter.ToUInt16(BlockData, 0);
            FileName = encoding.GetString(BlockData, 2, FileNameSize);
        }
    }

    //Useless block, we don't do anything with it that's why I just skip it in the reader
    //public class RarArchiveHeaderBlock : RarBlock
    //{
    //    [Flags()]
    //    public new enum FlagValues : ushort
    //    {
    //        Volume = 0x0001,
    //        CommentPresent = 0x0002,
    //        ArchiveLock = 0x0004,
    //        Solid = 0x0004,
    //        NewVolumeNaming = 0x0010,
    //        Authenticity = 0x0020,
    //        Recovery = 0x0040,
    //        Encrypted = 0x0080,
    //        FirstVolume = 0x0100
    //    }

    //    //This is pretty much useless especially for SRR, I need to skip this in the reader actually
    //    public ushort Reserved0 { get; set; }
    //    public uint Reserved1 { get; set; }

    //    public RarArchiveHeaderBlock(BinaryReader reader) : base(reader) { }
    //    public RarArchiveHeaderBlock(BinaryReader reader, RarBlock readBlock) : base(reader, readBlock) { }
    //    public override void ReadBlock(BinaryReader reader)
    //    {
    //        Reserved0 = reader.ReadUInt16();
    //        Reserved1 = reader.ReadUInt32();
    //    }
    //}

    public class RarFileHeaderBlock : RarBlock
    {
        [Flags()]
        public new enum FlagValues : ushort
        {
            ContinueFromPrevious = 0x1,
            ContinueInNext = 0x2,
            PasswordEncrypted = 0x4,
            Comment = 0x8,
            PreviousInfoUsed = 0x10,
            LargeFile = 0x100,
            UTF8FileName = 0x200,
            HasSalt = 0x400,
            Version = 0x800,
            ExtendedTime = 0x1000,
        }

        public uint PackedSize { get; protected set; }
        public uint UnpackedSize { get; protected set; }
        public byte HostOS { get; protected set; }
        public uint FileCRC { get; set; }
        public uint FileTime { get; set; }
        public byte UnpackVersion { get; protected set; }
        public byte CompressionMethod { get; protected set; }
        public ushort NameSize { get; protected set; }
        public uint FileAttributes { get; protected set; }
        public uint HighPackedSize { get; protected set; }
        public uint HighUnpackedSize { get; protected set; }
        public string FileName { get; protected set; }
        //public ulong Salt { get; set; }

        public RarFileHeaderBlock(BinaryReader reader) : base(reader) { }
        public RarFileHeaderBlock(RarBlock readBlock) : base(readBlock) { }
        public override void ReadData()
        {
            PackedSize = BitConverter.ToUInt32(BlockData, 0);
            UnpackedSize = BitConverter.ToUInt32(BlockData, 4);
            HostOS = BlockData[8];
            FileCRC = BitConverter.ToUInt32(BlockData, 9);
            FileTime = BitConverter.ToUInt32(BlockData, 13);
            UnpackVersion = BlockData[17];
            CompressionMethod = BlockData[18];
            NameSize = BitConverter.ToUInt16(BlockData, 19);
            FileAttributes = BitConverter.ToUInt32(BlockData, 23);

            if ((Flags & (ushort)FlagValues.LargeFile) != 0)
            {
                HighPackedSize = BitConverter.ToUInt32(BlockData, 26);
                HighUnpackedSize = BitConverter.ToUInt32(BlockData, 30);
                FileName = encoding.GetString(BlockData, 33, NameSize);
            }
            else
                FileName = encoding.GetString(BlockData, 25, NameSize);

            if (FileName.IndexOf('\0') >= 0)
                FileName = FileName.Substring(0, FileName.IndexOf('\0'));

            //if ((Flags & (ushort)FlagValues.HasSalt) != 0)
            //{
            //    Salt = reader.ReadUInt64();
            //}
        }
    }
}
