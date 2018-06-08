using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VenoMpie.Common.IO.FileSignatures
{
    public abstract class FileSignatureBase : IAmFileSignature
    {
        /// <summary>
        /// List of Valid File Extensions
        /// </summary>
        public abstract IList<string> FileExtensions { get; }
        /// <summary>
        /// List of Byte Signatures to Identify the File
        /// </summary>
        /// <remarks>
        /// It can be a list per format as something like PKZip has 3 (or even more ...) different signatures
        /// </remarks>
        public abstract IList<byte[]> HeaderSignature { get; }

        public FileInfo FileDetails { get; set; }
        /// <summary>
        /// The File Extension is Valid
        /// </summary>
        public bool FileExtensionIsValid { get; set; }
        /// <summary>
        /// The Byte Signature of the File is Valid
        /// </summary>
        public bool FileSignatureIsValid { get; set; }
        /// <summary>
        /// Byte Signature and File Extension are Valid
        /// </summary>
        public bool EntireFileIsValid { get; set; }

        public FileSignatureBase(FileInfo fileInfo)
        {
            MapConstructor(fileInfo);
        }
        public FileSignatureBase(string fileName)
        {
            MapConstructor(new FileInfo(fileName));
        }
        private void MapConstructor(FileInfo fileInfo)
        {
            FileDetails = fileInfo;
            FileExtensionIsValid = FileExtensionMatches();
            FileSignatureIsValid = FileSignatureMatches();
            EntireFileIsValid = (FileExtensionIsValid && FileSignatureIsValid);
        }
        public abstract bool FileExtensionMatches();
        public abstract bool FileSignatureMatches();
        internal bool GenericFileExtensionMatches()
        {
            try
            {
                return FileExtensions.Contains(FileDetails.Extension);
            }
            catch
            {
                return false;
            }
        }
        internal bool GenericFileSignatureMatches(int BufferLength, int StartingAddress)
        {
            try
            {
                using (FileStream fileStream = File.OpenRead(FileDetails.FullName))
                {
                    byte[] buffer = new byte[BufferLength];
                    if (StartingAddress > 0) fileStream.Seek(StartingAddress, SeekOrigin.Begin);
                    fileStream.Read(buffer, 0, buffer.Length);
                    foreach (var signature in HeaderSignature)
                    {
                        if (buffer.SequenceEqual(signature))
                            return true;
                    }
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
