using System.Collections.Generic;

namespace VenoMpie.Common.IO.FileSignatures.Compression
{
    public class SRR : FileSignatureBase
    {
        const int HeaderLength = 5;
        public override IList<string> FileExtensions { get { return new List<string> { ".srr" }; } }
        public override IList<byte[]> HeaderSignature { get { return new List<byte[]>() {
            new byte[HeaderLength] { 0x69, 0x69, 0x69, 0x01, 0x00 } //'i','i','i','.','.'
        }; } }

        public SRR(string fileName) : base(fileName) { }

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
