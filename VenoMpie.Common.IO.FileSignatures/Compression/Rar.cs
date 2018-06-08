using System.Collections.Generic;

namespace VenoMpie.Common.IO.FileSignatures.Compression
{
    public class Rar : FileSignatureBase
    {
        const int HeaderLength = 7;
        public override IList<string> FileExtensions { get { return new List<string> { ".rar" }; } }
        public override IList<byte[]> HeaderSignature { get { return new List<byte[]>() {
            new byte[HeaderLength] { 0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x00 } //'R','a','r','!','.','.','.'
        }; } }

        public Rar(string fileName) : base(fileName) { }

        public override bool FileExtensionMatches()
        {
            bool retValue = GenericFileExtensionMatches();
            if (!retValue)
            {
                if (FileDetails.Extension.Length == 4)
                {
                    if ((FileDetails.Extension.StartsWith(".r") || FileDetails.Extension.StartsWith(".s")) && char.IsDigit(FileDetails.Extension[2]) && char.IsDigit(FileDetails.Extension[3]))
                        retValue = true;
                }
            }
            return retValue;
        }

        public override bool FileSignatureMatches()
        {
            return GenericFileSignatureMatches(HeaderLength, 0);
        }
    }
}
