using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using VenoMpie.Common.Interfaces.SRRDB;
using System.IO;

namespace VenoMpie.Common.UnitTests.Interfaces
{
    [TestClass]
    public class SRRDBTests
    {
        private SRRDB srrDB = new SRRDB();
        [TestMethod]
        public void SRRDB_SRRAPICall()
        {
            srrDB.SearchBy(SRRDB.SRRSearchKeyWordsEnum.r, "Harry.Potter.And.The.Chamber.Of.Secrets.2002.DVDRip.XViD-iNTERNAL-TDF");
            SearchResult sr = srrDB.Search();
            Assert.AreEqual(1, sr.results.Count);
        }
        [TestMethod]
        public void SRRDB_Deserialize_Elements_Release()
        {
            SearchResult sr = srrDB.Deserialize(Properties.Resources.Interfaces_SRRDB_SingleResult);
            Assert.AreEqual("Harry.Potter.And.The.Chamber.Of.Secrets.2002.DVDRip.XViD-iNTERNAL-TDF", sr.results[0].release);
        }
        [TestMethod]
        public void SRRDB_Deserialize_Elements_HasSRS()
        {
            SearchResult sr = srrDB.Deserialize(Properties.Resources.Interfaces_SRRDB_SingleResult);
            Assert.IsTrue(sr.results[0].hasSRS == "yes");
        }
        [TestMethod]
        public void SRRDB_Deserialize_Elements_HasNFO()
        {
            SearchResult sr = srrDB.Deserialize(Properties.Resources.Interfaces_SRRDB_SingleResult);
            Assert.IsTrue(sr.results[0].hasNFO == "yes");
        }
        [TestMethod]
        public void SRRDB_Deserialize_Elements_Date()
        {
            SearchResult sr = srrDB.Deserialize(Properties.Resources.Interfaces_SRRDB_SingleResult);
            Assert.AreEqual(sr.results[0].date, new DateTime(2011,07,11));
        }
        [TestMethod]
        public void SRRDB_DeserializeSingle()
        {
            SearchResult sr = srrDB.Deserialize(Properties.Resources.Interfaces_SRRDB_SingleResult);
            Assert.AreEqual(1, sr.results.Count);
        }
        [TestMethod]
        public void SRRDB_DeserializeMultiple()
        {
            SearchResult sr = srrDB.Deserialize(Properties.Resources.Interfaces_SRRDB_MultiResult);
            Assert.AreEqual(45, sr.results.Count);
        }
    }
}
