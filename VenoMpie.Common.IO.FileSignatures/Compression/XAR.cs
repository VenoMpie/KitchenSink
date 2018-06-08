using System.Collections.Generic;

namespace VenoMpie.Common.IO.FileSignatures.Compression
{
    public class XAR : FileSignatureBase
    {
        const int HeaderLength = 4;
        public override IList<string> FileExtensions { get { return new List<string> { ".xar" }; } }
        IList<byte[]> _headerSignature = new List<byte[]>() {
            new byte[HeaderLength] { 0x78, 0x61, 0x72, 0x21 } //'x','a','r','!'
        };
        public override IList<byte[]> HeaderSignature { get { return _headerSignature; } }

        public XAR(string fileName) : base(fileName) { }

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
