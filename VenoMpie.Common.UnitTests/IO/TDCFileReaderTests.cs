using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using VenoMpie.Common.IO.FileReaders.Emulation.TotalDosCollection;
using VenoMpie.Common.IO.FileReaders.Emulation.DataFiles;
using VenoMpie.Common.IO.FileReaders.Emulation.General;

namespace VenoMpie.Common.UnitTests.IO
{
    [TestClass]
    public class TDCFileReaderTests
    {
        [TestMethod]
        public void TDCReader_TestDatReaderTDC()
        {
            TDCDat_Text datReader = new TDCDat_Text();
            datReader.ReadFile(new MemoryStream(Properties.Resources.IO_FileReaders_TDCFull));
            Assert.IsFalse(datReader.Contents.game.Any(a => a == null) || datReader.Contents.game.Any(a => a.file == null));
        }
        [TestMethod]
        public void TDCReader_TestDatWriterTDC()
        {
            TDCDat_Text datReader = new TDCDat_Text();
            datReader.ReadFile(new MemoryStream(Properties.Resources.IO_FileReaders_TDC));
            datReader.WriteFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TDCWriterTestText.dat"));
        }
        [TestMethod]
        public void TDCReader_TestTDCConversion()
        {
            Conversions.ConvertTDCTextToXML(new MemoryStream(Properties.Resources.IO_FileReaders_TDC), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TDCXML.dat"));
        }
        [TestMethod]
        public void TDCReader_TestTDCFullList()
        {
            TDCDat_Text datReader = new TDCDat_Text();
            datReader.ReadFile(new MemoryStream(Properties.Resources.IO_FileReaders_TDCFull));
            List<GameEntry> list = new List<GameEntry>();
            foreach (var item in datReader.Contents.game)
            {
                list.Add(new LinkedGameEntry(item));
            }
            Assert.AreEqual(list.Count, 12700);
            Assert.AreEqual(list.Where(a => a.ErrorParsing).Count(), 3);
            //Some of the Games in TDC are not properly formatted but they will show up as errors
            //List of Games not formatted:
            //Positronic Bridge (1993)(Positronic Software, Inc) (Strategy, Cards]
            //Storymaker VGA v1.00e [SW] (1995)(Elson Embry] [Educational]
            //Mortal Pong v0.5c (1997)(Cheesy Software0 [Action]
        }
        [TestMethod]
        public void TDCReader_TestParsedTDCDatReaderXMLClrMamePro()
        {
            ParsedTDCDat<TDCDat_Text> datReader = new ParsedTDCDat<TDCDat_Text>();
            datReader.ReadFile(new MemoryStream(Properties.Resources.IO_FileReaders_TDCFull));
            Assert.AreEqual(datReader.Games.Count, 12700);
            Assert.IsFalse(datReader.Games.Any(a => a.Value.Name == ""));
            Assert.IsFalse(datReader.Games.Any(a => a.Value.LinkedGame == null));
        }
        [TestMethod]
        public void TDCReader_TestMergedTDCDatReaderXMLClrMamePro()
        {
            MergedTDCDat<TDCDat_Text> datReader = new MergedTDCDat<TDCDat_Text>();
            datReader.ReadFile(new MemoryStream(Properties.Resources.IO_FileReaders_TDCFull));
            Assert.AreEqual(datReader.Games.Count, 12700);
            Assert.AreEqual(datReader.MergedGames.Count, 9051);
            int count = 0;
            foreach (var game in datReader.MergedGames)
                count += game.Games.Count;
            Assert.AreEqual(count, 12700);
            Assert.IsFalse(datReader.MergedGames.Any(a => a.Games.Any(b => b.LinkedGame == null)));
        }
    }
}
