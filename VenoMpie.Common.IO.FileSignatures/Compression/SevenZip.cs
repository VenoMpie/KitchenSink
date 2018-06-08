using System.Collections.Generic;

namespace VenoMpie.Common.IO.FileSignatures.Compression
{
    public class SevenZip : FileSignatureBase
    {
        const int HeaderLength = 6;
        public override IList<string> FileExtensions { get { return new List<string> { ".7z" }; } }
        public override IList<byte[]> HeaderSignature { get { return new List<byte[]>() {
            new byte[HeaderLength] { 0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C } //'7','z','¼','¯',''','.'
        }; } }

        public SevenZip(string fileName) : base(fileName) { }

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
