using System.Collections.Generic;

namespace VenoMpie.Common.IO.FileSignatures.Compression
{
    public class TAR : FileSignatureBase
    {
        const int HeaderLength = 8;
        public override IList<string> FileExtensions { get { return new List<string> { ".tar" }; } }
        IList<byte[]> _headerSignature = new List<byte[]>() {
            new byte[HeaderLength] { 0x75, 0x73, 0x74, 0x61, 0x72, 0x00, 0x30, 0x30 }, //'u','s','t','a','r','.','0','0'
            new byte[HeaderLength] { 0x75, 0x73, 0x74, 0x61, 0x72, 0x20, 0x20, 0x00 } //'u','s','t','a','r',' ',' ','.'
        };
        public override IList<byte[]> HeaderSignature { get { return _headerSignature; } }

        public TAR(string fileName) : base(fileName) { }

        public override bool FileExtensionMatches()
        {
            return GenericFileExtensionMatches();
        }

        public override bool FileSignatureMatches()
        {
            return GenericFileSignatureMatches(HeaderLength, 257);
        }
    }
}
