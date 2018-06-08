using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Globalization;

namespace VenoMpie.Common.IO.FileReaders.CSV
{
    /// <summary>
    /// A Reader class that reads through CSV Files
    /// </summary>
    public class CSVReader : IDisposable
    {
        private StreamReader Reader { get; set; }
        public CSVReader(string path, char seperator) : this(path, Encoding.Default, seperator) { }
        public CSVReader(Stream stream, char seperator) : this(stream, Encoding.Default, seperator) { }
        public CSVReader(string path, Encoding encoding, char seperator) : this(new FileStream(path, FileMode.Open, FileAccess.Read), encoding, seperator) { }
        public CSVReader(Stream stream, Encoding encoding, char seperator)
        {
            Reader = new StreamReader(stream, encoding);
            SeperatorChar = seperator;
        }

        private char SeperatorChar { get; set; }

        public string[][] ReadToEnd()
        {
            List<string[]> retValue = new List<string[]>();
            string line = "";
            while ((line = Reader.ReadLine()) != null)
            {
                retValue.Add(Read(line));
            }
            Reader.Close();
            return retValue.ToArray();
        }
        public string[] Read(string line)
        {
            List<string> retVal = new List<string>();
            bool isQuoted = false;
            string value = "";
            int currentIter = -1;
            for (int i = 0; i < line.Length; i++)
            {
                currentIter += 1;
                if (line[i] == '\"')
                {
                    if (currentIter == 0)
                    {
                        isQuoted = true;
                    }
                    else
                    {
                        isQuoted = false;
                        currentIter = -1;
                    }
                }
                else if (line[i] == SeperatorChar && !isQuoted)
                {
                    retVal.Add(value);
                    value = "";
                    isQuoted = false;
                    currentIter = -1;
                }
                else if (i == line.Length - 1)
                {
                    //if (line[i] == SeperatorChar)
                    //{
                    //    retVal.Add(value);
                    //    break;
                    //}
                    //else if (isQuoted && line[i] != '\"')
                    if (isQuoted && line[i] != '\"')
                        value += line[i];

                    retVal.Add(value);
                }
                else
                    value += line[i];
            }
            return retVal.ToArray();
        }
        public void Dispose() { Reader.Close(); }
    }
}
