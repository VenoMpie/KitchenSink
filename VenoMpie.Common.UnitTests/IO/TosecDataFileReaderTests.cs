using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VenoMpie.Common.IO.FileReaders.Emulation.DataFiles;
using System.IO;
using System;
using System.Collections;
using VenoMpie.Common.IO.FileReaders.Emulation.DataFiles.Resources;
using VenoMpie.Common.IO.FileReaders.Emulation.TOSEC;

namespace VenoMpie.Common.UnitTests.IO
{
    [TestClass]
    public class TosecDataFileReaderTests
    {
        private const string CLRWriterTestDat = "CLRWriterTest.dat";
        private const string CLRWriterTextTestDat = "CLRWriterTestText.dat";

        [TestMethod]
        public void TosecReader_TestTosecDatReaderClrMamePro()
        {
            TosecDat_Text datReader = new TosecDat_Text();
            datReader.ReadFile(new MemoryStream(Properties.Resources.IO_FileReaders_ClrMemeProNESFull));
            Assert.AreEqual(datReader.Contents.game.Length, 12147);
            Assert.IsFalse(datReader.Contents.game.Any(a => a == null) || datReader.Contents.game.Any(a => a.rom == null));
        }
        [TestMethod]
        public void TosecReader_TestTosecDatWriterClrMamePro()
        {
            TosecDat_Text datReader = new TosecDat_Text();
            datReader.ReadFile(new MemoryStream(Properties.Resources.IO_FileReaders_ClrMemeProNESFull));

            if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CLRWriterTextTestDat)))
                File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CLRWriterTextTestDat));
            datReader.WriteFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CLRWriterTextTestDat));

            TosecDat_Text datWrittenReader = new TosecDat_Text();
            datWrittenReader.ReadFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CLRWriterTextTestDat));

            Assert.AreEqual(datReader.Contents.game.Length, 12147);
            Assert.AreEqual(datWrittenReader.Contents.game.Length, 12147);
            Assert.IsFalse(datWrittenReader.Contents.game.Any(a => a == null) || datWrittenReader.Contents.game.Any(a => a.rom == null));
        }
        [TestMethod]
        public void TosecReader_TestTosecDatReaderXMLClrMamePro()
        {
            TosecDat_XML datReader = new TosecDat_XML();
            datReader.ReadFile(new MemoryStream(Properties.Resources.IO_FileReaders_ClrMemeProNESFullXML));
            Assert.AreEqual(datReader.Contents.game.Length, 12147);
            Assert.IsFalse(datReader.Contents.game.Any(a => a == null) || datReader.Contents.game.Any(a => a.rom == null));
        }
        [TestMethod]
        public void TosecReader_TestParsedTosecDatReaderXMLClrMamePro()
        {
            ParsedTosecDat<TosecDat_XML> datReader = new ParsedTosecDat<TosecDat_XML>();
            datReader.ReadFile(new MemoryStream(Properties.Resources.IO_FileReaders_ClrMemeProNESFullXML));
            Assert.AreEqual(datReader.Games.Count, 12147);
            Assert.IsFalse(datReader.Games.Any(a => a.Value.Name == ""));
            Assert.IsFalse(datReader.Games.Any(a => a.Value.LinkedGame == null));
        }
        [TestMethod]
        public void TosecReader_TestMergedTosecDatReaderXMLClrMamePro()
        {
            MergedTosecDat<TosecDat_XML> datReader = new MergedTosecDat<TosecDat_XML>();
            datReader.ReadFile(new MemoryStream(Properties.Resources.IO_FileReaders_ClrMemeProNESFullXML));
            Assert.AreEqual(datReader.Games.Count, 12147);
            Assert.AreEqual(datReader.MergedGames.Count, 3660);
            int count = 0;
            foreach (var game in datReader.MergedGames)
                count += game.Games.Count;
            Assert.AreEqual(count, 12147);
            Assert.IsFalse(datReader.MergedGames.Any(a => a.Games.Any(b => b.LinkedGame == null)));
        }
        [TestMethod]
        public void TosecReader_TestTosecDatWriterXMLClrMamePro()
        {
            TosecDat_XML datReader = new TosecDat_XML();
            datReader.ReadFile(new MemoryStream(Properties.Resources.IO_FileReaders_ClrMemeProNESFullXML));

            if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CLRWriterTestDat)))
                File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CLRWriterTestDat));
            datReader.WriteFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CLRWriterTestDat));

            TosecDat_XML datWrittenReader = new TosecDat_XML();
            datWrittenReader.ReadFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CLRWriterTestDat));

            Assert.AreEqual(datReader.Contents.game.Length, 12147);
            Assert.AreEqual(datWrittenReader.Contents.game.Length, 12147);
            Assert.IsFalse(datWrittenReader.Contents.game.Any(a => a == null) || datWrittenReader.Contents.game.Any(a => a.rom == null));
        }
        [TestMethod]
        public void TosecReader_TestTosecGameEntry()
        {
            var testTitle = @"Test Title, The v1.4 (demo-kiosk) (2017)(Test Publisher)(CD32)(NTSC)(US)(en)(SW)(beta)(Disc 1 of 6)(Character Disk)[cr TRSI][h PDM][b]";
            var ge = new GameEntry(testTitle);
            Assert.AreEqual("Test Title, The", ge.Name);
            Assert.AreEqual("v1.4", ge.Version);
            Assert.AreEqual("demo-kiosk", ge.Demo);
            Assert.AreEqual("2017", ge.Date);
            Assert.AreEqual("Test Publisher", ge.Publisher);
            Assert.AreEqual("CD32", ge.GameSystem);
            Assert.AreEqual("NTSC", ge.Video);
            Assert.AreEqual("US", ge.Country);
            Assert.AreEqual("en", ge.Language);
            Assert.AreEqual("SW", ge.Copyright);
            Assert.AreEqual("beta", ge.DevelopmentStatus);
            Assert.AreEqual("Disc 1 of 6", ge.MediaType);
            Assert.AreEqual("Character Disk", ge.MediaLabel);
            Assert.AreEqual("[cr TRSI][h PDM][b]", ge.DumpFlags);
            Assert.AreEqual("", ge.Additional);
        }
    }
}
