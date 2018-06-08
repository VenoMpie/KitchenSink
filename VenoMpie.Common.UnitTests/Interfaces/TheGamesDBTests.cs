using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VenoMpie.Common.Interfaces.SRRDB;
using System.Collections.Generic;
using VenoMpie.Common.Interfaces.TheGamesDB;
using VenoMpie.Common.Interfaces.TheGamesDB.SearchResults;
using System.IO;

namespace VenoMpie.Common.UnitTests
{
    [TestClass]
    public class TheGamesDBTests
    {
        private static TheGamesDB gamesDBList;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            gamesDBList = new TheGamesDB();
        }

        #region Web Calls
        [TestMethod]
        public void TheGamesDB_GetArtCall()
        {
            Dictionary<SearchParameters, object> searchParameters = new Dictionary<SearchParameters, object>()
            {
                { SearchParameters.id, 2 }
            };
            GetArt list = gamesDBList.MakeWebCall<GetArt>(searchParameters);
            Assert.IsTrue(list.Images.Length > 0);
        }
        [TestMethod]
        public void TheGamesDB_GetGamesListCall()
        {
            Dictionary<SearchParameters, object> searchParameters = new Dictionary<SearchParameters, object>()
            {
                { SearchParameters.name, "Street%20Rod" }
            };
            GetGamesList list = gamesDBList.MakeWebCall<GetGamesList>(searchParameters);
            Assert.IsFalse(list.Games.Length <= 0);
        }
        [TestMethod]
        public void TheGamesDB_GetPlatformCall()
        {
            Dictionary<SearchParameters, object> searchParameters = new Dictionary<SearchParameters, object>()
            {
                { SearchParameters.id, 15 }
            };
            GetPlatform list = gamesDBList.MakeWebCall<GetPlatform>(searchParameters);
            Assert.AreNotEqual("", list.Platform);
        }
        [TestMethod]
        public void TheGamesDB_GetPlatformsListCall()
        {
            GetPlatformsList list = gamesDBList.MakeWebCall<GetPlatformsList>();
            Assert.IsFalse(list.Platforms.Length <= 0);
        }
        [TestMethod]
        public void TheGamesDB_GetPlatformGamesCall()
        {
            Dictionary<SearchParameters, object> searchParameters = new Dictionary<SearchParameters, object>()
            {
                { SearchParameters.platform, 1 }
            };
            GetPlatformGames list = gamesDBList.MakeWebCall<GetPlatformGames>(searchParameters);
            Assert.IsFalse(list.Games.Length <= 0);
        }
        [TestMethod]
        public void TheGamesDB_UpdatesCall()
        {
            Dictionary<SearchParameters, object> searchParameters = new Dictionary<SearchParameters, object>()
            {
                { SearchParameters.time, 20000 }
            };
            Updates list = gamesDBList.MakeWebCall<Updates>(searchParameters);
            if (list.Games != null)
                Assert.IsFalse(list.Games.Length <= 0);
            else
                Assert.IsNull(list.Games);
        }
        #endregion
        #region Deserialize
        [TestMethod]
        public void TheGamesDB_GetArtDeserialize()
        {
            GetArt list = gamesDBList.DeserializeOnly<GetArt>(Properties.Resources.Interfaces_TheGamesDB_GetArt);
            Assert.IsTrue(list.Images.Length > 0);
        }
        [TestMethod]
        public void TheGamesDB_GetGamesListDeserialize()
        {
            GetGamesList list = gamesDBList.DeserializeOnly<GetGamesList>(Properties.Resources.Interfaces_TheGamesDB_GetGamesList);
            Assert.IsTrue(list.Games.Length > 0);
        }
        [TestMethod]
        public void TheGamesDB_GetGameDeserialize()
        {
            GetGame game = gamesDBList.DeserializeOnly<GetGame>(Properties.Resources.Interfaces_TheGamesDB_GetGame);
            Assert.IsNotNull(game);
        }
        [TestMethod]
        public void TheGamesDB_GetPlatformDeserialize()
        {
            GetPlatform retItem = gamesDBList.DeserializeOnly<GetPlatform>(Properties.Resources.Interfaces_TheGamesDB_GetPlatform);
            Assert.AreNotEqual("", retItem.Platform);
        }
        [TestMethod]
        public void TheGamesDB_GetPlatformsListDeserialize()
        {
            GetPlatformsList list = gamesDBList.DeserializeOnly<GetPlatformsList>(Properties.Resources.Interfaces_TheGamesDB_GetPlatformList);
            Assert.IsTrue(list.Platforms.Length > 0);
        }
        [TestMethod]
        public void TheGamesDB_DeserializeUTF8()
        {
            System.Net.WebClient client = new System.Net.WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            string results = client.DownloadString("http://thegamesdb.net/api/GetPlatform.php?id=4946");
            //<Data><baseImgUrl>http://thegamesdb.net/banners/</baseImgUrl><Platform><id>4946</id><Platform>Commodore 128</Platform><console>http://www.youtube.com/watch?v=4946.png</console><overview>The Commodore 128 is the last 8-bit home computer that was commercially released by Commodore Business Machines (CBM). Introduced in January 1985 at the CES in Las Vegas, it appeared three years after its predecessor, the bestselling Commodore 64. Some 64 software such as Bard&#039;s Tale III and Kid Niki ran in 128 mode without stating this in the documentation, using the autoboot and the 1571&#039;s faster disk access.  Some Infocom text adventures took advantage of the 80 column screen and increased memory capacity. Some C64 games were ported to native mode like Kikstart 2 and The Last V8 from Mastertronic, which have separate 128 versions, and Ultima V: Warriors of Destiny from Origin Systems, which uses extra RAM for music if running on the C128. The vast majority of games simply ran in 64 mode.</overview><developer>Commodore International</developer><manufacturer>Commodore Business Machines</manufacturer><cpu>Zilog Z80A @ 4 MHz</cpu><memory>128 kB</memory><graphics>VIC-II E (320×200, 16 colors, sprites, raster interrupt)</graphics><sound>SID 6581/8580 (3× Osc, 4× Wave, Filter, ADSR, Ring)</sound><maxcontrollers>2</maxcontrollers><Rating>10</Rating><Images><boxart side=\"back\" width=\"500\" height=\"750\">platform/boxart/4946-1.jpg</boxart><consoleart>platform/consoleart/4946.png</consoleart><controllerart>platform/controllerart/4946.png</controllerart></Images></Platform></Data>
            Assert.IsTrue(results.Contains("<graphics>VIC-II E (320×200, 16 colors, sprites, raster interrupt)</graphics>"));
        }
        [TestMethod]
        public void TheGamesDB_TestHTMLDecode()
        {
            GetPlatform retItem = gamesDBList.DeserializeOnly<GetPlatform>("<?xml version=\"1.0\" encoding=\"UTF-8\" ?><Data><baseImgUrl>http://thegamesdb.net/banners/</baseImgUrl><Platform><id>4946</id><Platform>Commodore 128</Platform><console>http://www.youtube.com/watch?v=4946.png</console><overview>The Commodore 128 is the last 8-bit home computer that was commercially released by Commodore Business Machines (CBM). Introduced in January 1985 at the CES in Las Vegas, it appeared three years after its predecessor, the bestselling Commodore 64. Some 64 software such as Bard&amp;#039;s Tale III and Kid Niki ran in 128 mode without stating this in the documentation, using the autoboot and the 1571&amp;#039;s faster disk access.  Some Infocom text adventures took advantage of the 80 column screen and increased memory capacity. Some C64 games were ported to native mode like Kikstart 2 and The Last V8 from Mastertronic, which have separate 128 versions, and Ultima V: Warriors of Destiny from Origin Systems, which uses extra RAM for music if running on the C128. The vast majority of games simply ran in 64 mode.</overview><developer>Commodore International</developer><manufacturer>Commodore Business Machines</manufacturer><cpu>Zilog Z80A @ 4 MHz</cpu><memory>128 kB</memory><graphics>VIC-II E (320×200, 16 colors, sprites, raster interrupt)</graphics><sound>SID 6581/8580 (3× Osc, 4× Wave, Filter, ADSR, Ring)</sound><maxcontrollers>2</maxcontrollers><Rating>10</Rating><Images><boxart side=\"back\" width=\"500\" height=\"750\">platform/boxart/4946-1.jpg</boxart><consoleart>platform/consoleart/4946.png</consoleart><controllerart>platform/controllerart/4946.png</controllerart></Images></Platform></Data>");
            Assert.IsTrue(retItem.Platform.Overview.Contains("Bard's Tale III"));
        }
        #endregion
    }
}
