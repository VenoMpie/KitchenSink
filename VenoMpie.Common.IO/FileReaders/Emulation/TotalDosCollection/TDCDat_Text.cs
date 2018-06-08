using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VenoMpie.Common.IO.FileReaders.Emulation.DataFiles;
using VenoMpie.Common.IO.FileReaders.Emulation.DataFiles.Resources;

namespace VenoMpie.Common.IO.FileReaders.Emulation.TotalDosCollection
{
    /// <summary>
    /// This is a TDC Specific Dat file reader.
    /// </summary>
    /// <remarks>
    /// This class is purely here because it's the first one I wrote and used for quite a while so I trust it, I prefer using the TOSEC Reader as it's more generic but meh :P.
    /// TDC is maintened by hargle and is an open source project based on TOSEC but is specifically for DOS.
    /// It is the best DOS based collection that I have ever seen (TOSEC is lacking on the DOS side by far), thus the TDC reader :P
    /// </remarks>
    /// <see cref="http://www.totaldoscollection.org/"/>
    public class TDCDat_Text : IAmDataFile
    {
        #region Constants
        private const string DOSCenter = "DOSCenter (";
        private const string DAT_Name = "Name:";
        private const string DAT_Description = "Description:";
        private const string DAT_Version = "Version:";
        private const string DAT_Date = "Date:";
        private const string DAT_Author = "Author:";
        private const string DAT_Homepage = "Homepage:";
        private const string DAT_Comment = "Comment:";

        private const string Game_Game = "game (";
        private const string Game_Name = "name ";
        private const string Game_File = "file (";
        #endregion
        public datafile Contents { get; set; }
        public TDCDat_Text() { Contents = new datafile(); }

        private void ReadHeader(StreamReader reader)
        {
            Contents.header = new header();
            bool isReadingHeader = false;
            int currentLine = 0;
            string line;
            while (!reader.EndOfStream)
            {
                currentLine++;
                if (currentLine > 20) throw new Exception("Invalid TDC Dat File");
                line = ReplaceIllegalTDCCharacters(reader.ReadLine());
                if (isReadingHeader)
                {
                    if (line.Contains(DAT_Name)) Contents.header.name = ParseLine(line, DAT_Name);
                    if (line.Contains(DAT_Description)) Contents.header.description = ParseLine(line, DAT_Description);
                    if (line.Contains(DAT_Version)) Contents.header.version = ParseLine(line, DAT_Version);
                    if (line.Contains(DAT_Date)) Contents.header.date = ParseLine(line, DAT_Date);
                    if (line.Contains(DAT_Author)) Contents.header.author = ParseLine(line, DAT_Author);
                    if (line.Contains(DAT_Homepage)) Contents.header.homepage = ParseLine(line, DAT_Homepage);
                    if (line.Contains(DAT_Comment)) Contents.header.comment = ParseLine(line, DAT_Comment);
                    if (line.Trim() == ")") return;
                }
                if (line.Contains(DOSCenter)) isReadingHeader = true;
            }
        }
        private void ReadGames(StreamReader reader)
        {
            string line;
            bool isReadingGame = false;
            List<game> retGames = new List<game>();
            List<file> retFiles = new List<file>();
            game currentGame = new game();
            while (!reader.EndOfStream)
            {
                line = ReplaceIllegalTDCCharacters(reader.ReadLine());

                if (isReadingGame)
                {
                    if (!line.Contains(Game_File))
                    {
                        if (line.Contains(Game_Name)) currentGame.name = ParseLine(line, Game_Name);
                    }
                    if (line.Contains(Game_File))
                    {
                        List<string> romParseLines = line.Substring(line.IndexOf(Game_File) + Game_File.Length, line.LastIndexOf(" )") - line.IndexOf(Game_File) - Game_File.Length).Replace(" name ", "|").Replace(" size ", "|").Replace(" date ", "|").Replace(" crc ", "|").Split('|').Where(a => a.Trim() != "").ToList();
                        file currentFile = new file();
                        currentFile.name = romParseLines[0];
                        currentFile.size = romParseLines[1];
                        currentFile.date = romParseLines[2];
                        currentFile.crc = romParseLines[3];
                        retFiles.Add(currentFile);
                    }
                    if (line.Trim() == ")")
                    {
                        currentGame.file = retFiles.ToArray();
                        retGames.Add(currentGame);
                        retFiles = new List<file>();
                        currentGame = new game();
                    }
                }
                if (line.StartsWith(Game_Game)) isReadingGame = true;
            }
            Contents.game = retGames.ToArray();
        }
        public void ReadFile(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                Contents = new datafile();
                ReadHeader(reader);
                ReadGames(reader);
            }
        }
        public void ReadFile(string path)
        {
            ReadFile(new FileStream(path, FileMode.Open));
        }
        public void WriteFile(Stream stream)
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine("DOSCenter (");
                writer.WriteLine("\t{0}: {1}", "Name", Contents.header.name);
                writer.WriteLine("\t{0}: {1}", "Description", Contents.header.description);
                writer.WriteLine("\t{0}: {1}", "Version", Contents.header.version);
                writer.WriteLine("\t{0}: {1}", "Date", Contents.header.date);
                writer.WriteLine("\t{0}: {1}", "Author", Contents.header.author);
                writer.WriteLine("\t{0}: {1}", "Homepage", Contents.header.homepage);
                writer.WriteLine("\t{0}: {1}", "Comment", Contents.header.comment);
                writer.WriteLine(")");
                writer.WriteLine("");
                foreach (var retGame in Contents.game)
                {
                    writer.WriteLine("game (");
                    writer.WriteLine("\tname \"{0}\"", retGame.name);
                    foreach (var retFile in retGame.file)
                    {
                        writer.WriteLine("\tfile ( name {0} size {1} date {2} crc {3} )", retFile.name, retFile.size, retFile.date, retFile.crc);
                    }
                    writer.WriteLine(")");
                    writer.WriteLine("");
                }
            }
        }
        public void WriteFile(string path)
        {
            WriteFile(path, FileMode.OpenOrCreate);
        }
        public void WriteFile(string path, FileMode fileMode)
        {
            WriteFile(new FileStream(path, fileMode));
        }
        private string ParseLine(string lineToParse, string staticFieldToCheck)
        {
            return lineToParse.Substring(lineToParse.IndexOf(staticFieldToCheck) + staticFieldToCheck.Length).Trim().Replace("\"", "");
        }
        private string ReplaceIllegalTDCCharacters(string stringToReplace)
        {
            return stringToReplace.Replace(".zip", "").Replace("\u000f", "");
        }
    }
}
