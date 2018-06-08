using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VenoMpie.Common.Interfaces.Scrapers
{
    public class SCiZEFile
    {
        public string Filename { get; set; } = "";
        public long Size { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = "";
    }
    /// <summary>
    /// This class parses the HTML files found on SCiZE's site at http://scenelist.hopto.org/collection.html
    /// </summary>
    /// <remarks>
    /// SCiZE has got some of the best preserved old school scene data that I have ever seen
    /// Download only the specific month's file and use that
    /// </remarks>
    /// <see cref="http://scenelist.hopto.org/collection.html"/>
    public class SCiZESceneList
    {
        public List<SCiZEFile> ScrapeFile(string file) => ScrapeFile(new FileStream(file, FileMode.Open));
        public List<SCiZEFile> ScrapeFile(Stream stream)
        {
            bool foundStart = false;
            SCiZEFile previousFile = new SCiZEFile();
            List<SCiZEFile> retList = new List<SCiZEFile>();
            using (StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8))
            {
                string line;
                while (!(sr.EndOfStream))
                {
                    line = sr.ReadLine();
                    if (foundStart)
                        if (line.Contains("</div>") || line.Contains("</body>"))
                            foundStart = false;
                        else
                        {
                            var newFile = ScrapeLine(line);
                            if (newFile.Filename == "")
                                previousFile.Description = previousFile.Description + "\r\n" + newFile.Description;
                            else
                            {
                                if (previousFile.Filename != "")
                                    retList.Add(previousFile);

                                previousFile = newFile;
                            }
                        }
                    else
                        if (line.Contains("<div class=\"list\">"))
                        foundStart = true;
                }
            }
            return retList;
        }
        private SCiZEFile ScrapeLine(string line)
        {
            SCiZEFile file = new SCiZEFile()
            {
                Filename = line.Substring(0, line.IndexOf(' ')).Trim()
            };
            if (file.Filename != "")
            {
                string originalLine = line;
                line = line.Substring(line.IndexOf(' ')).Trim();
                var value = line.Substring(0, line.IndexOf(' ')).Trim();
                if (value.Length > 0) file.Size = Convert.ToInt64(value);
                line = line.Substring(line.IndexOf(' ')).Trim();
                if (line.Contains(' '))
                {
                    var fileDateString = line.Substring(0, line.IndexOf(' ')).Trim();
                    //This is a temporary (permanent!) hack. The BBS boards show Y2K as MM-dd-100, DateTime Can't Parse 100 so we change it to 00
                    if (fileDateString.EndsWith("-100")) fileDateString = fileDateString.Replace("-100", "-00");
                    file.Date = DateTime.ParseExact(fileDateString, "MM-dd-yy", null);
                    file.Description = originalLine.Substring(34);
                }
                else
                {
                    var fileDateString = line.Trim();
                    if (fileDateString.EndsWith("-100")) fileDateString = fileDateString.Replace("-100", "-00");
                    file.Date = DateTime.ParseExact(fileDateString, "MM-dd-yy", null);
                }
            }
            else
                file.Description = line.Substring(34);
            return file;
        }
    }
}
