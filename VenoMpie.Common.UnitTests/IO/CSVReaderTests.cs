using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VenoMpie.Common.IO.FileReaders.CSV;
using System.IO;
using VenoMpie.Common.UnitTests;

namespace Gamebase.NET.UnitTests.IO
{
    [TestClass]
    public class CSVReaderTests : CommonUnitTestBase
    {
        [TestMethod]
        public void CSVReader_TestCSVFile()
        {
            using (CSVReader reader = new CSVReader(Path.Combine(resourceDir, "IO_FileReaders_CSVFile.txt"), ','))
            {
                string[][] entries = reader.ReadToEnd();
                Assert.AreEqual(entries.Length, 300);
                Assert.IsFalse(entries.Any(a => a.Length > 5));
            }
        }
    }
}
