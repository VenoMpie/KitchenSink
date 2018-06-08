using System.Collections.Generic;
using System.IO;

namespace VenoMpie.Common.IO.FileSignatures.Emulation
{
    public class PSID : FileSignatureBase
    {
        const int HeaderLength = 4;
        public override IList<string> FileExtensions { get { return new List<string> { ".sid" }; } }
        public override IList<byte[]> HeaderSignature { get { return new List<byte[]>() {
            new byte[HeaderLength] { 0x50, 0x53, 0x49, 0x44 } //'P','S','I','D'
        }; } }

        public PSID(string fileName) : base(fileName) { }
        public PSID(FileInfo fileInfo) : base(fileInfo) { }

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
