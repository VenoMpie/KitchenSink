using System.Collections.Generic;
using System.IO;

namespace VenoMpie.Common.IO.FileSignatures.Emulation
{
    public class IWAD : FileSignatureBase
    {
        const int HeaderLength = 4;
        public override IList<string> FileExtensions { get { return new List<string> { ".wad" }; } }
        public override IList<byte[]> HeaderSignature { get { return new List<byte[]>() {
            new byte[HeaderLength] { 0x49, 0x57, 0x41, 0x44 } //'I','W','A','D'
        }; } }

        public IWAD(string fileName) : base(fileName) { }
        public IWAD(FileInfo fileInfo) : base(fileInfo) { }

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
