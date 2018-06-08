using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VenoMpie.Common.IO.FileReaders.Emulation.TotalDosCollection
{
    /// <summary>
    /// A simple Reader class that reads the TDC Master List and returns cleaned entries
    /// </summary>
    public class TDCMasterListReader : IDisposable
    {
        private StreamReader Reader { get; set; }
        public TDCMasterListReader(string path) : this(path, Encoding.Default) { }
        public TDCMasterListReader(Stream stream) : this(stream, Encoding.Default) { }
        public TDCMasterListReader(string path, Encoding encoding) : this(new FileStream(path, FileMode.Open), encoding) { }
        public TDCMasterListReader(Stream stream, Encoding encoding) { Reader = new StreamReader(stream, encoding); }

        public GameEntry Read()
        {
            if (!Reader.EndOfStream)
                return new GameEntry(Reader.ReadLine().Replace(".zip", ""));

            return null;
        }
        public List<GameEntry> ReadToEnd()
        {
            List<GameEntry> retValue = new List<GameEntry>();
            while (!Reader.EndOfStream)
            {
                retValue.Add(Read());
            }
            return retValue;
        }

        public void Dispose() { Reader.Close(); }
    }
}
