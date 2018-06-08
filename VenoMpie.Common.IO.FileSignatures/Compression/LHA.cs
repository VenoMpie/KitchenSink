using System.Collections.Generic;

namespace VenoMpie.Common.IO.FileSignatures.Compression
{
    public class LHA : FileSignatureBase
    {
        const int HeaderLength = 2;
        public override IList<string> FileExtensions { get { return new List<string> { ".z", ".tar.z" }; } }
        IList<byte[]> _headerSignature = new List<byte[]>() {
            new byte[HeaderLength] { 0x1F, 0xA0 } //'.','.'
        };
        public override IList<byte[]> HeaderSignature { get { return _headerSignature; } }

        public LHA(string fileName) : base(fileName) { }

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
