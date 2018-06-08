using System;
using System.Text;
using System.Text.RegularExpressions;
using VenoMpie.Common.IO.FileReaders.Emulation.General;

namespace VenoMpie.Common.IO.FileReaders.Emulation.TotalDosCollection
{
    public class GameEntry : GameEntryBase
    {
        public bool DOSConversion { get; set; }
        public bool Installer { get; set; }
        public bool Shareware { get; set; }
        public string Version { get; set; }
        public string Language { get; set; }
        public string Genre { get; set; }

        public GameEntry() { }
        public GameEntry(string line)
        {
            Initialize();
            Parse(line);
        }
        public GameEntry(string line, string regex)
        {
            ProgramRegExString = regex;
            Parse(line);
        }
        public override void Parse(string line)
        {
            FullName = line;
            try
            {
                var regex = new Regex(ProgramRegExString);
                var matches = regex.Matches(line);
                FullName = line;
                Name = matches[0].Groups[1].Value;
                Version = matches[0].Groups[2].Value;
                Language = matches[0].Groups[4].Value;
                DumpFlags = matches[0].Groups[5].Value;
                DOSConversion = (string.IsNullOrWhiteSpace(matches[0].Groups[7].Value.Trim()) ? false : true);
                Shareware = (string.IsNullOrWhiteSpace(matches[0].Groups[9].Value.Trim()) ? false: true);
                Installer = (string.IsNullOrWhiteSpace(matches[0].Groups[11].Value.Trim()) ? false: true);
                Date = matches[0].Groups[13].Value;
                Publisher = matches[0].Groups[15].Value;
                Genre = matches[0].Groups[17].Value;
                Additional = matches[0].Groups[18].Value;
            }
            catch (Exception)
            {
                ErrorParsing = true;
            }
        }

        /// <summary>
        /// Returns the Regex that will be used to match a TDC Game
        /// </summary>
        /// <returns>String containing the constructed RegEx string</returns>
        public override string BuildRegex()
        {
            StringBuilder sb = new StringBuilder();
            //Name
            sb.Append(@"(.*?)\s{1}");

            //Version Info
            sb.Append(@"(Rev|v\d\.{1}\d+\w?|v\d{8}|v\d+\.?\d?[a-z]?)?\s?");

            //Language
            sb.Append(@"(\((As|Fr|En|It|De|Ru|Cn|Jp|Cz|Pl|No|Sp|US|Multi-\d*)\))?\s?");

            //Dump Flags
            sb.Append(@"(\[[a|b|f|h|o]\d+\]|[r|R]+\d+|\[[\!]\])?\s?");

            //DOS Conversion
            sb.Append(@"(\[(DC)\])?\s?");

            //Shareware
            sb.Append(@"(\[(SW|SWR)+\])?\s?");

            //Installer
            sb.Append(@"(\((Installer)\))?\s?");

            //Date
            sb.Append(@"(\((\d{4}|\d{2}xx|\d{3}x)\)){1}");

            //Publisher
            sb.Append(@"(\((.*)\)){1}\s?");

            //Genre
            sb.Append(@"(\[([a-zA-Z\,*() -]+)\]){1}");

            //And the rest
            sb.Append(@"(.*)");

            return sb.ToString();
        }
    }
}
