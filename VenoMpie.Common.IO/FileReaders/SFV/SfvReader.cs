using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Globalization;

namespace VenoMpie.Common.IO.FileReaders.SFV
{
    public class SfvEntry
    {
        public string FileName { get; set; }
        public uint Crc32 { get; set; }
    }

    public abstract class SfvReaderBase
    {
        protected SfvEntry ParseString(string line)
        {
            if (uint.TryParse(line.Substring(line.Length - 8), NumberStyles.HexNumber, null, out uint i))
                return new SfvEntry()
                {
                    FileName = line.Substring(0, line.Length - 9).Trim(),
                    Crc32 = i
                };
            else
                return null;
        }
    }

    /// <summary>
    /// A Reader class that reads through SFV Files
    /// </summary>
    public class SfvStreamReader : SfvReaderBase, IDisposable
    {
        private StreamReader Reader { get; set; }
        public SfvStreamReader(string path) : this(path, Encoding.Default) { }
        public SfvStreamReader(Stream stream) : this(stream, Encoding.Default) { }
        public SfvStreamReader(string path, Encoding encoding) : this(new FileStream(path, FileMode.Open, FileAccess.Read), encoding) { }
        public SfvStreamReader(Stream stream, Encoding encoding) { Reader = new StreamReader(stream, encoding); }

        public List<SfvEntry> ReadToEnd()
        {
            List<SfvEntry> retValue = new List<SfvEntry>();
            SfvEntry retRead;
            while ((retRead = Read()) != null)
                retValue.Add(retRead);

            Reader.Close();
            return retValue;
        }
        public SfvEntry Read()
        {
            // We While loop with the following conditions
            // Comment lines in SFV's start with semicolons (;), so skip those.
            // Skip any lines that are too short to have both a File Name and CRC
            // CRC is 8 with a space = 9, so CRC without a file is wrong
            // Skip any entry that has illegal characters (We must actually be careful of this one as it may differ system to system. I test on Windows, so stuff like @, ", etc. are invalid)
            string line;
            do
                line = Reader.ReadLine();
            while (line != null && (line.Trim().Length < 10 || line.TrimStart().StartsWith(";") || line.IndexOfAny(Path.GetInvalidPathChars().Where(a => a != '\t').ToArray()) >= 0));

            if (line == null) return null;

            return ParseString(line);
        }
        public void Dispose() { Reader.Close(); }
    }

    /// <summary>
    /// A Reader class that reads through SFV Files
    /// </summary>
    public class SfvStringReader : SfvReaderBase
    {
        private string SFVContents { get; set; }

        public SfvStringReader(string sfvContents) { SFVContents = sfvContents; }

        public List<SfvEntry> ReadToEnd()
        {
            List<SfvEntry> retValue = new List<SfvEntry>();
            string[] lineSplit = SFVContents.Split(new char[] { '\n' }).Select(a => a.Replace("\r", "")).ToArray();
            foreach (string line in lineSplit)
            {
                if (line.Trim().Length < 10 || line.TrimStart().StartsWith(";") || line.IndexOfAny(Path.GetInvalidPathChars().Where(a => a != '\t').ToArray()) >= 0)
                    continue;

                retValue.Add(ParseString(line));
            }

            return retValue;
        }
    }
}
