using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VenoMpie.Common.Interfaces.Scrapers;

namespace VenoMpie.Common.UnitTests.Scrapers
{
    [TestClass]
    public class SCiZESceneListTests
    {
        SCiZESceneList scraper = new SCiZESceneList();
        [TestMethod]
        public void SCiZEScraper_ScrapeJune1993File()
        {
            var checkDate = new DateTime(1993, 06, 01);
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(Properties.Resources.Scrapers_SCiZE_List);
            MemoryStream ms = new MemoryStream(byteArray);
            var list = scraper.ScrapeFile(ms);
            ms.Close();
            Assert.AreEqual(867, list.Count);
            Assert.IsTrue(list.All(a => a.Filename != null && a.Filename.Trim() != ""));
            Assert.IsTrue(list.All(a => a.Date.Year.Equals(checkDate.Year) && a.Date.Month.Equals(checkDate.Month)));
        }
    }
}
