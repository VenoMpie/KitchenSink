using System.Collections.Generic;

namespace VenoMpie.Common.IO.FileSignatures.Compression
{
    public class Zip : FileSignatureBase
    {
        const int HeaderLength = 4;
        public override IList<string> FileExtensions { get { return new List<string> { ".zip", ".jar", ".odt", ".ods", ".odp", ".docx", ".xlsx", ".pptx", ".apk" }; } }
        IList<byte[]> _headerSignature = new List<byte[]>() {
            new byte[HeaderLength] { 0x50, 0x4B, 0x03, 0x04 }, //'P','K','.','.' --> Empty Archive
            new byte[HeaderLength] { 0x50, 0x4B, 0x05, 0x06 }, //'P','K','.','.' --> Empty Archive
            new byte[HeaderLength] { 0x50, 0x4B, 0x07, 0x08 } //'P','K','.','.' --> Spanned Archive
        };
        public override IList<byte[]> HeaderSignature { get { return _headerSignature; } }

        public Zip(string fileName) : base(fileName) { }

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
