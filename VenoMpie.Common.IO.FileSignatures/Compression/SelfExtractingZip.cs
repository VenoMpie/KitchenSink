using System.Collections.Generic;

namespace VenoMpie.Common.IO.FileSignatures.Compression
{
    public class SelfExtractingZip : FileSignatureBase
    {
        const int HeaderLength = 5;
        public override IList<string> FileExtensions { get { return new List<string> { ".exe" }; } }
        public override IList<byte[]> HeaderSignature { get { return new List<byte[]>() {
            new byte[HeaderLength] { 0x50, 0x4B, 0x53, 0x70, 0x58 } //'P','K','S','p','X'
        }; } }

        public SelfExtractingZip(string fileName) : base(fileName) { }

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
