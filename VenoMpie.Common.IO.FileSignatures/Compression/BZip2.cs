using System.Collections.Generic;

namespace VenoMpie.Common.IO.FileSignatures.Compression
{
    public class BZip2 : FileSignatureBase
    {
        const int HeaderLength = 3;
        public override IList<string> FileExtensions { get { return new List<string> { ".bz2" }; } }
        IList<byte[]> _headerSignature = new List<byte[]>() {
            new byte[HeaderLength] { 0x42, 0x5A, 0x68 } //'B','Z','h'
        };
        public override IList<byte[]> HeaderSignature { get { return _headerSignature; } }

        public BZip2(string fileName) : base(fileName) { }

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
