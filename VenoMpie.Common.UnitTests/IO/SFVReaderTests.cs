using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VenoMpie.Common.IO.FileReaders.SFV;
using System.IO;
using VenoMpie.Common.UnitTests;

namespace Gamebase.NET.UnitTests.IO
{
    [TestClass]
    public class SFVReaderTests : CommonUnitTestBase
    {
        [TestMethod]
        public void SFVReader_TestSFVFile()
        {
            SfvStreamReader reader = new SfvStreamReader(Path.Combine(resourceDir, "IO_FileReaders_SFVFile.sfv"));
            List<SfvEntry> entries = reader.ReadToEnd();
            Assert.AreEqual(entries.Count, 87);
        }
    }
}
