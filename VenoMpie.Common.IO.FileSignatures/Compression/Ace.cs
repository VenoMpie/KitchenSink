using System.Collections.Generic;

namespace VenoMpie.Common.IO.FileSignatures.Compression
{
    public class Ace : FileSignatureBase
    {
        const int HeaderLength = 7;
        public override IList<string> FileExtensions { get { return new List<string> { ".ace" }; } }
        public override IList<byte[]> HeaderSignature { get { return new List<byte[]>() {
            new byte[HeaderLength] { 0x2A, 0x2A, 0x41, 0x43, 0x45, 0x2A, 0x2A } //'*','*','A','C','E','*','*'
        }; } }

        public Ace(string fileName) : base(fileName) { }

        public override bool FileExtensionMatches()
        {
            bool retValue = GenericFileExtensionMatches();
            if (!retValue)
            {
                if (FileDetails.Extension.Length == 4)
                {
                    if (FileDetails.Extension.StartsWith(".c") && char.IsDigit(FileDetails.Extension[2]) && char.IsDigit(FileDetails.Extension[3]))
                        retValue = true;
                }
            }
            return retValue;
        }

        public override bool FileSignatureMatches()
        {
            return GenericFileSignatureMatches(HeaderLength, 0x07);
        }
    }
}
