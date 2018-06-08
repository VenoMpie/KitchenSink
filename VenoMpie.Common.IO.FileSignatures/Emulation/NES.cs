using System.Collections.Generic;
using System.IO;

namespace VenoMpie.Common.IO.FileSignatures.Emulation
{
    public class NES : FileSignatureBase
    {
        const int HeaderLength = 4;
        public override IList<string> FileExtensions { get { return new List<string> { ".nes" }; } }
        public override IList<byte[]> HeaderSignature { get { return new List<byte[]>() {
            new byte[HeaderLength] { 0x4E, 0x45, 0x53, 0x1A } //'N','E','S','.'
        }; } }

        public NES(string fileName) : base(fileName) { }
        public NES(FileInfo fileInfo) : base(fileInfo) { }

        public override bool FileExtensionMatches()
        {
            return GenericFileExtensionMatches();
        }

        public override bool FileSignatureMatches()
        {
            return GenericFileSignatureMatches(HeaderLength, 0);
        }
    }
}
