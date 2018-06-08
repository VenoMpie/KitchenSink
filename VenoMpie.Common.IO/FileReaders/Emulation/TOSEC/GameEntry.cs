using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VenoMpie.Common.IO.FileReaders.Emulation.General;

namespace VenoMpie.Common.IO.FileReaders.Emulation.TOSEC
{
    /// <summary>
    /// Parses a string with TOSEC Naming Convention into meaningful and strong-typed properties
    /// </summary>
    public class GameEntry : GameEntryBase
    {
        #region TOSEC Dictionary KeyValues
        protected Dictionary<string, string> ForbiddenCharacters { get; set; } = new Dictionary<string, string>()
        {
            { "/", "Slash" },
            { "\\", "Backslash" },
            { "?", "Question Mark" },
            { ":", "Colon" },
            { "*", "Asterisk" },
            { "\"", "Quote" },
            { "<", "Less Than" },
            { ">", "Greater Than" },
            { "|", "Vertical Pipe" },
        };
        protected Dictionary<string, string> Demos { get; set; } = new Dictionary<string, string>()
        {
            { "demo", "demonstration version" },
            { "demo-kiosk", "Retail demo units and kiosks" },
            { "demo-playable", "General demonstration version, playable" },
            { "demo-rolling", "General demonstration version, non-interactive" },
            { "demo-slideshow", "General demonstration version, non-interactive slideshow" },
        };
        protected Dictionary<string, string> Systems { get; set; } = new Dictionary<string, string>()
        {
            { "\\+2", "Sinclair ZX Spectrum" },
            { "\\+2a", "Sinclair ZX Spectrum" },
            { "\\+3", "Sinclair ZX Spectrum" },
            { "130XE", "Atari 8-bit" },
            { "A1000", "Commodore Amiga" },
            { "A1200", "Commodore Amiga" },
            { "A1200-A4000", "Commodore Amiga" },
            { "A2000", "Commodore Amiga" },
            { "A2000-A3000", "Commodore Amiga" },
            { "A2024", "Commodore Amiga" },
            { "A2500-A3000UX", "Commodore Amiga" },
            { "A3000", "Commodore Amiga" },
            { "A4000", "Commodore Amiga" },
            { "A4000T", "Commodore Amiga" },
            { "A500", "Commodore Amiga" },
            { "A500+", "Commodore Amiga" },
            { "A500-A1000-A2000", "Commodore Amiga" },
            { "A500-A1000-A2000-CDTV", "Commodore Amiga" },
            { "A500-A1200", "Commodore Amiga" },
            { "A500-A1200-A2000-A4000", "Commodore Amiga" },
            { "A500-A2000", "Commodore Amiga" },
            { "A500-A600-A2000", "Commodore Amiga" },
            { "A570 Commodore", "Amiga" },
            { "A600 Commodore", "Amiga" },
            { "A600HD Commodore", "Amiga" },
            { "AGA Commodore", "Amiga" },
            { "AGA-CD32", "Commodore Amiga" },
            { "Aladdin Deck Enhancer", "Nintendo NES" },
            { "CD32", "Commodore Amiga" },
            { "CDTV", "Commodore Amiga" },
            { "Computrainer", "Nintendo NES" },
            { "Doctor PC Jr.", "Nintendo NES" },
            { "ECS", "Commodore Amiga" },
            { "ECS-AGA", "Commodore Amiga" },
            { "Executive", "Osborne 1 & Executive" },
            { "Mega ST", "Atari ST" },
            { "Mega-STE", "Atari ST" },
            { "OCS", "Commodore Amiga" },
            { "OCS-AGA", "Commodore Amiga" },
            { "ORCH80", "???" },
            { "Osbourne 1", "Osborne 1 & Executive" },
            { "PIANO90", "???" },
            { "PlayChoice-10", "Nintendo NES" },
            { "Plus4", "???" },
            { "Primo-A", "Microkey Primo" },
            { "Primo-A64", "Microkey Primo" },
            { "Primo-B", "Microkey Primo" },
            { "Primo-B64", "Microkey Primo" },
            { "Pro-Primo", "Microkey Primo" },
            { "ST", "Atari ST" },
            { "STE", "Atari ST" },
            { "STE-Falcon", "???" },
            { "TT", "Atari ST" },
            { "TURBO-R GT", "MSX" },
            { "TURBO-R ST", "MSX" },
            { "VS DualSystem", "Nintendo NES" },
            { "VS UniSystem", "Nintendo NES" },
        };
        protected Dictionary<string, string> Videos { get; set; } = new Dictionary<string, string>()
        {
            { "CGA", "?" },
            { "EGA", "?" },
            { "HGC", "?" },
            { "MCGA", "?" },
            { "MDA", "?" },
            { "NTSC", "?" },
            { "NTSC-PAL", "?" },
            { "PAL", "?" },
            { "PAL-60", "?" },
            { "PAL-NTSC", "?" },
            { "SVGA", "?" },
            { "VGA", "?" },
            { "XGA", "?" },
        };
        protected Dictionary<string, string> Languages { get; set; } = new Dictionary<string, string>()
        {
            { "ar", "Arabic" },
            { "bg", "Bulgarian" },
            { "bs", "Bosnian" },
            { "cs", "Czech" },
            { "cy", "Welsh" },
            { "da", "Danish" },
            { "de", "German" },
            { "el", "Greek" },
            { "en", "English" },
            { "eo", "Esperanto" },
            { "es", "Spanish" },
            { "et", "Estonian" },
            { "fa", "Persian" },
            { "fi", "Finnish" },
            { "fr", "French" },
            { "ga", "Irish" },
            { "gu", "Gujarati" },
            { "he", "Hebrew" },
            { "hi", "Hindi" },
            { "hr", "Croatian" },
            { "hu", "Hungarian" },
            { "is", "Icelandic" },
            { "it", "Italian" },
            { "ja", "Japanese" },
            { "ko", "Korean" },
            { "lt", "Lithuanian" },
            { "lv", "Latvian" },
            { "ms", "Malay" },
            { "nl", "Dutch" },
            { "no", "Norwegian" },
            { "pl", "Polish" },
            { "pt", "Portuguese" },
            { "ro", "Romanian" },
            { "ru", "Russian" },
            { "sk", "Slovakian" },
            { "sl", "Slovenian" },
            { "sq", "Albanian" },
            { "sr", "Serbian" },
            { "sv", "Swedish" },
            { "th", "Thai" },
            { "tr", "Turkish" },
            { "ur", "Urdu" },
            { "vi", "Vietnamese" },
            { "yi", "Yiddish" },
            { "zh", "Chinese" },
        };
        protected Dictionary<string, string> Countries { get; set; } = new Dictionary<string, string>()
        {
            { "AE", "United Arab Emirates" },
            { "AL", "Albania" },
            { "AS", "Asia" },
            { "AT", "Austria" },
            { "AU", "Australia" },
            { "BA", "Bosnia and Herzegovina" },
            { "BE", "Belgium" },
            { "BG", "Bulgaria" },
            { "BR", "Brazil" },
            { "CA", "Canada" },
            { "CH", "Switzerland" },
            { "CL", "Chile" },
            { "CN", "China" },
            { "CS", "Serbia and Montenegro" },
            { "CY", "Cyprus" },
            { "CZ", "Czech Republic" },
            { "DE", "Germany" },
            { "DK", "Denmark" },
            { "EE", "Estonia" },
            { "EG", "Egypt" },
            { "ES", "Spain" },
            { "EU", "Europe" },
            { "FI", "Finland" },
            { "FR", "France" },
            { "GB", "United Kingdom" },
            { "GR", "Greece" },
            { "HK", "Hong Kong" },
            { "HR", "Croatia" },
            { "HU", "Hungary" },
            { "ID", "Indonesia" },
            { "IE", "Ireland" },
            { "IL", "Israel" },
            { "IN", "India" },
            { "IR", "Iran" },
            { "IS", "Iceland" },
            { "IT", "Italy" },
            { "JO", "Jordan" },
            { "JP", "Japan" },
            { "KR", "South Korea" },
            { "LT", "Lithuania" },
            { "LU", "Luxembourg" },
            { "LV", "Latvia" },
            { "MN", "Mongolia" },
            { "MX", "Mexico" },
            { "MY", "Malaysia" },
            { "NL", "Netherlands" },
            { "NO", "Norway" },
            { "NP", "Nepal" },
            { "NZ", "New Zealand" },
            { "OM", "Oman" },
            { "PE", "Peru" },
            { "PH", "Philippines" },
            { "PL", "Poland" },
            { "PT", "Portugal" },
            { "QA", "Qatar" },
            { "RO", "Romania" },
            { "RU", "Russia" },
            { "SE", "Sweden" },
            { "SG", "Singapore" },
            { "SI", "Slovenia" },
            { "SK", "Slovakia" },
            { "TH", "Thailand" },
            { "TR", "Turkey" },
            { "TW", "Taiwan" },
            { "US", "United States" },
            { "VN", "Vietnam" },
            { "YU", "Yugoslavia" },
            { "ZA", "South Africa" },
        };
        protected Dictionary<string, string> Copyrights { get; set; } = new Dictionary<string, string>()
        {
            { "CW", "Cardware" },
            { "CW-R", "Cardware-Registered" },
            { "FW", "Freeware" },
            { "GW", "Giftware" },
            { "GW-R", "Giftware-Registered" },
            { "LW", "Licenceware" },
            { "PD", "Public Domain" },
            { "SW", "Shareware" },
            { "SW-R", "Shareware-Registered" },
        };
        protected Dictionary<string, string> DevelopmentStatuses { get; set; } = new Dictionary<string, string>()
        {
            { "alpha", "Early test build" },
            { "beta", "Later, feature complete test build" },
            { "preview", "Near complete build" },
            { "pre-release", "Near complete build" },
            { "proto", "Unreleased, prototype software" },
        };
        protected Dictionary<string, string> MediaTypes { get; set; } = new Dictionary<string, string>()
        {
            { "Disc", "Optical disc based media" },
            { "Disk", "Magnetic disk based media" },
            { "File", "Individual files" },
            { "Part", "Individual parts" },
            { "Side", "Side of media" },
            { "Tape", "Magnetic tape based media" },
        };
        #endregion
        #region TOSEC Values
        public string Version { get; set; }
        public string Demo { get; set; }
        public string GameSystem { get; set; }
        public string Video { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }
        public string Copyright { get; set; }
        public string DevelopmentStatus { get; set; }
        public string MediaType { get; set; }
        public string MediaLabel { get; set; }
        #endregion

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

        /// <summary>
        /// Parses a Tosec Game Name into meaningful properties
        /// </summary>
        /// <param name="line">Game Name or Description</param>
        /// <example>
        /// Parse("Test Title, The v1.4 (demo-kiosk) (2017)(Test Publisher)(CD32)(NTSC)(US)(en)(SW)(beta)(Disc 1 of 6)(Character Disk)[cr TRSI][h PDM][b]");
        /// </example>
        public override void Parse(string line)
        {
            var regex = new Regex(ProgramRegExString);
            var matches = regex.Matches(line);
            FullName = line;
            Name = matches[0].Groups[1].Value;
            Version = matches[0].Groups[2].Value;
            Demo = matches[0].Groups[4].Value;
            Date = matches[0].Groups[5].Value;
            Publisher = matches[0].Groups[6].Value;
            GameSystem = matches[0].Groups[8].Value;
            Video = matches[0].Groups[10].Value;
            Country = matches[0].Groups[12].Value;
            Language = matches[0].Groups[14].Value;
            Copyright = matches[0].Groups[16].Value;
            DevelopmentStatus = matches[0].Groups[18].Value;
            MediaType = matches[0].Groups[19].Value;
            if (MediaType.StartsWith("(") && MediaType.EndsWith(")") && MediaType.Length > 2)
                MediaType = MediaType.Substring(1, MediaType.Length - 2);
            MediaLabel = matches[0].Groups[23].Value;
            DumpFlags = matches[0].Groups[24].Value;
            Additional = matches[0].Groups[27].Value;
        }

        /// <summary>
        /// Returns the Regex that will be used to match a TOSEC Game
        /// </summary>
        /// <returns>String containing the constructed RegEx string from the pre-defined dictionaries</returns>
        public override string BuildRegex()
        {
            StringBuilder sb = new StringBuilder();
            //Name
            sb.Append(@"(.*?)\s{1}");

            //Version Info
            sb.Append(@"(Rev|v\d\.{1}\d+\w?|v\d{8})?\s?");

            //Demo
            sb.Append(@"(\((");
            foreach (var item in Demos)
                sb.Append(item.Key + "|");
            sb.Remove(sb.Length - 1, 1);
            sb.Append(@")\)\s{1})?");

            //Date
            sb.Append(@"\(([0-9x\-]+).*?\)");

            //Publisher
            sb.Append(@"\(([^\)]*)\)");

            //System
            sb.Append(@"(\((");
            foreach (var item in Systems)
                sb.Append(item.Key + "|");
            sb.Remove(sb.Length - 1, 1);
            sb.Append(@")\))?");

            //Video
            sb.Append(@"(\((");
            foreach (var item in Videos)
                sb.Append(item.Key + "|");
            sb.Remove(sb.Length - 1, 1);
            sb.Append(@")\))?");

            //Country
            sb.Append(@"(\((");
            foreach (var item in Countries)
                sb.Append(item.Key + "|");
            sb.Remove(sb.Length - 1, 1);
            sb.Append(@")\))?");

            //Language
            sb.Append(@"(\((");
            foreach (var item in Languages)
                sb.Append(item.Key + "|");
            sb.Remove(sb.Length - 1, 1);
            sb.Append(@")\))?");

            //Copyright
            sb.Append(@"(\((");
            foreach (var item in Copyrights)
                sb.Append(item.Key + "|");
            sb.Remove(sb.Length - 1, 1);
            sb.Append(@")\))?");

            //Development Status
            sb.Append(@"(\((");
            foreach (var item in DevelopmentStatuses)
                sb.Append(item.Key + "|");
            sb.Remove(sb.Length - 1, 1);
            sb.Append(@")\))?");

            //Media Type
            sb.Append(@"(\((");
            foreach (var item in MediaTypes)
                sb.Append(item.Key + "|");
            sb.Remove(sb.Length - 1, 1);
            sb.Append(@")(\s\d\sof\s\d)+\))?");

            //Media Label
            sb.Append(@"(\((.*)\))?");

            //Dump Flags
            sb.Append(@"(\[(cr|f|h|m|p|t|tr|o|u|v|b|a|!){1}(\s{1}.*)?\])*");

            //And the rest
            sb.Append(@"(.*)");

            return sb.ToString();
        }
    }
}
