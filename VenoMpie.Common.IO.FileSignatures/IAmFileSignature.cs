using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.IO.FileSignatures
{
    public interface IAmFileSignature
    {
        IList<string> FileExtensions { get; }
        IList<byte[]> HeaderSignature { get; }
        FileInfo FileDetails { get; set; }
        bool FileExtensionIsValid { get; set; }
        bool FileSignatureIsValid { get; set; }
        bool EntireFileIsValid { get; set; }

        bool FileExtensionMatches();
        bool FileSignatureMatches();
    }
}
