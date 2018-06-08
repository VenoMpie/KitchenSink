using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using VenoMpie.Common.Interfaces.GGn;
using VenoMpie.Common.Interfaces.GGn.SearchResults;

namespace VenoMpie.Common.UnitTests.Interfaces
{
    [TestClass]
    public class GGNTests
    {
        //Instantiate this class with your key and fill in the Consts to be able to run the tests
        private const string UserName = "";
        private const int UserID = 0;
        private const string UserClass = "";

        private GGn ggn = new GGn();
        [TestMethod]
        public void GGN_QuickUserInfo()
        {
            GGnResult<QuickUserInfo> r = ggn.GetQuickUserInfo();
            Assert.AreEqual(UserID, r.response.id);
            Assert.AreEqual(UserName, r.response.username);
            Assert.AreEqual(UserClass, r.response.userstats.@class);
        }
    }
}
