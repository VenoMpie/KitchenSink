using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using VenoMpie.Common.Interfaces.WadArchive;

namespace VenoMpie.Common.UnitTests.Interfaces
{
    [TestClass]
    public class WadArchiveTests
    {
        private WadArchive wa = new WadArchive();
        [TestMethod]
        public void WadArchive_WadArchiveAPICall()
        {
            wa.AddSearchTerm("barbell.wad");
            SearchResult sr = wa.Search();
            Assert.AreEqual("barbell.wad", sr.filenames[0]);
        }
        [TestMethod]
        public void WadArchive_DeserializeSingle()
        {
            SearchResult sr = wa.Deserialize(Properties.Resources.Interfaces_WadArchive_Barbell_Wadseeker);
            Assert.AreEqual(1, sr.filenames.Length);
        }
        [TestMethod]
        public void WadArchive_DeserializeMultiple()
        {
            SearchResult sr = wa.Deserialize(Properties.Resources.Interfaces_WadArchive_Dwango5_Wadseeker);
            Assert.AreEqual(2, sr.filenames.Length);
        }
    }
}
