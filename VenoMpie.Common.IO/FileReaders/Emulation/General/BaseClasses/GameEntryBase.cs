using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VenoMpie.Common.IO.FileReaders.Emulation.General;

namespace VenoMpie.Common.IO.FileReaders.Emulation.General
{
    public abstract class GameEntryBase: IGameEntry
    {
        protected virtual string ProgramRegExString { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string Publisher { get; set; }
        public string DumpFlags { get; set; }
        public string Additional { get; set; }
        public bool ErrorParsing { get; set; } = false;
        public GameEntryBase() { }
        public GameEntryBase(string line)
        {
            Initialize();
            Parse(line);
        }
        public GameEntryBase(string line, string regex)
        {
            ProgramRegExString = regex;
            Parse(line);
        }
        protected void Initialize()
        {
            ProgramRegExString = BuildRegex();
        }

        public abstract void Parse(string line);
        public abstract string BuildRegex();
    }
}
