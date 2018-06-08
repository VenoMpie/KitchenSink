using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.IO.FileReaders.Compression.SRR
{
    public class SRRReader : IDisposable
    {
        private const int headerLength = 0x7;

        private Stream SrrStream { get; set; }
        private BinaryReader Reader { get; set; }

        public SrrHeaderBlock Header { get; set; }
        public List<SrrRarFileBlock> RarFiles { get; set; } = new List<SrrRarFileBlock>();
        public List<SrrStoredFileBlock> StoredFiles { get; set; } = new List<SrrStoredFileBlock>();
        public List<RarFileHeaderBlock> PackedFiles { get; set; } = new List<RarFileHeaderBlock>();

        public SRRReader(string filePath) : this(new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)) { }
        public SRRReader(Stream srrStream)
        {
            SrrStream = srrStream;
            Reader = new BinaryReader(SrrStream);
            ReadFile();
        }

        private void ReadFile()
        {
            while (Reader.BaseStream.Position != Reader.BaseStream.Length)
            {
                if (SrrStream.Position + headerLength > SrrStream.Length) break;
                var rarBlock = new RarBlock(Reader);

                //We only care about these block types, the rest are useless and the RarBlock class will handle that
                switch (rarBlock.Type)
                {
                    case SRRBlockType.SrrHeader:
                        Header = new SrrHeaderBlock(rarBlock);
                        break;
                    case SRRBlockType.SrrStoredFile:
                        StoredFiles.Add(new SrrStoredFileBlock(rarBlock));
                        break;
                    case SRRBlockType.SrrRarFile:
                        RarFiles.Add(new SrrRarFileBlock(rarBlock));
                        break;
                    case SRRBlockType.RarPackedFile:
                        PackedFiles.Add(new RarFileHeaderBlock(rarBlock));
                        break;
                }
            }
            Reader.Close();
        }

        public void Dispose()
        {
            SrrStream.Close();
            Reader.Close();
        }
    }
}
