using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VenoMpie.Common.Interfaces.MobyGames;
using System.Threading;

namespace VenoMpie.Common.UnitTests.Interfaces
{
    [TestClass]
    public class MobyGamesTests
    {
        //Instantiate this class with your key to be able to run the tests
        private MobyGames mb = new MobyGames();

        [TestMethod]
        public void MobyGames_Search_Genres()
        {
            //We have to atleast wait 1 second before making another request so we force the sleep
            Thread.Sleep(1000);
            var results = mb.SearchGenres();
            Assert.IsTrue(results.Count > 207);
            Assert.IsTrue(results.Any(a => a.genre_name == "Action"));
            Assert.IsTrue(results.Any(a => a.genre_name == "Adventure"));
            Assert.IsTrue(results.Any(a => a.genre_name == "Simulation"));
            Assert.IsTrue(results.Any(a => a.genre_name == "Sports"));
        }
        [TestMethod]
        public void MobyGames_Search_Groups()
        {
            //We have to atleast wait 1 second before making another request so we force the sleep
            Thread.Sleep(1000);
            var results = mb.SearchGroups();
            //We get the defaults back so 100 items
            Assert.AreEqual(100, results.Count);
            Assert.IsTrue(results.Any(a => a.group_name == "Monkey Island series"));
            Assert.IsTrue(results.Any(a => a.group_name == "Sierra remakes"));
            Assert.IsTrue(results.Any(a => a.group_name == "MechWarrior series"));
            Assert.IsTrue(results.Any(a => a.group_name == "Leisure Suit Larry series"));
        }
        [TestMethod]
        public void MobyGames_Search_Platforms()
        {
            //We have to atleast wait 1 second before making another request so we force the sleep
            Thread.Sleep(1000);
            var results = mb.SearchPlatforms();
            Assert.IsTrue(results.Count > 210);
            Assert.IsTrue(results.Any(a => a.platform_name == "Linux"));
            Assert.IsTrue(results.Any(a => a.platform_name == "DOS"));
            Assert.IsTrue(results.Any(a => a.platform_name == "Windows"));
            Assert.IsTrue(results.Any(a => a.platform_name == "Dreamcast"));
        }
        [TestMethod]
        public void MobyGames_Search_Games_Title_StreetRod()
        {
            //We have to atleast wait 1 second before making another request so we force the sleep
            Thread.Sleep(1000);
            var results = mb.SearchGames("Street Rod");
            Assert.IsTrue(results.Count >= 3);
            Assert.IsTrue(results.Any(a => a.title == "Street Rod"));
            Assert.IsTrue(results.Any(a => a.title == "Street Rod 2: The Next Generation"));
            Assert.IsTrue(results.Any(a => a.title == "Street Rod: Data Disk"));
        }
        [TestMethod]
        public void MobyGames_Search_Games_IDs_StreetRod()
        {
            //We have to atleast wait 1 second before making another request so we force the sleep
            Thread.Sleep(1000);
            var results = mb.SearchGames(new List<int>() { 434, 447, 77734 });
            Assert.AreEqual(results.Count, 3);
            Assert.IsTrue(results.Any(a => a.title == "Street Rod"));
            Assert.IsTrue(results.Any(a => a.title == "Street Rod 2: The Next Generation"));
            Assert.IsTrue(results.Any(a => a.title == "Street Rod: Data Disk"));
        }
        [TestMethod]
        public void MobyGames_Search_Game_StreetRod()
        {
            //We have to atleast wait 1 second before making another request so we force the sleep
            Thread.Sleep(1000);
            var results = mb.SearchGame(434);
            Assert.AreEqual(results.game_id, 434);
            Assert.AreEqual(results.title, "Street Rod");
            Assert.AreEqual(results.moby_url, "http://www.mobygames.com/game/street-rod");
            Assert.AreEqual(results.moby_score, 3.8);
        }
        [TestMethod]
        public void MobyGames_Search_Game_Brief_StreetRod()
        {
            //We have to atleast wait 1 second before making another request so we force the sleep
            Thread.Sleep(1000);
            var results = mb.SearchGame_Brief(434);
            Assert.AreEqual(results.game_id, 434);
            Assert.AreEqual(results.title, "Street Rod");
            Assert.AreEqual(results.moby_url, "http://www.mobygames.com/game/street-rod");
        }
        [TestMethod]
        public void MobyGames_Search_Recent_Games_ID_Limit_5()
        {
            //We have to atleast wait 1 second before making another request so we force the sleep
            Thread.Sleep(1000);
            var results = mb.RecentGames_ID(21, 5);
            Assert.AreEqual(results.Count, 5);
        }
        [TestMethod]
        public void MobyGames_Search_Random_Games_ID_Limit_5()
        {
            //We have to atleast wait 1 second before making another request so we force the sleep
            Thread.Sleep(1000);
            var results = mb.RandomGames_ID(5);
            Assert.AreEqual(results.Count, 5);
        }
        [TestMethod]
        public void MobyGames_Search_GamePlatforms_StreetRod()
        {
            //We have to atleast wait 1 second before making another request so we force the sleep
            Thread.Sleep(1000);
            var results = mb.SearchGamePlatforms(434);
            Assert.AreEqual(results.Count, 3);
            Assert.IsTrue(results.Any(a => a.platform_name == "DOS"));
            Assert.IsTrue(results.Any(a => a.platform_name == "Commodore 64"));
            Assert.IsTrue(results.Any(a => a.platform_name == "Amiga"));
        }
        [TestMethod]
        public void MobyGames_Search_GamePlatform_StreetRod_DOS()
        {
            //We have to atleast wait 1 second before making another request so we force the sleep
            Thread.Sleep(1000);
            var results = mb.SearchGamePlatform(434, 2);
            Assert.AreEqual(results.game_id, 434);
            Assert.AreEqual(results.platform_id, 2);
            Assert.AreEqual(results.platform_name, "DOS");
            Assert.AreEqual(results.first_release_date, "1989");
            Assert.AreEqual(results.attributes.Count, 13);
        }
        [TestMethod]
        public void MobyGames_Search_GamePlatform_StreetRod_DOS_Screenshots()
        {
            //We have to atleast wait 1 second before making another request so we force the sleep
            Thread.Sleep(1000);
            var results = mb.SearchGamePlatformScreenshots(434, 2);
            Assert.IsTrue(results.Count > 15);
            Assert.IsTrue(results.Any(a => a.caption == "Street rod inside garage"));
            Assert.IsTrue(results.Any(a => a.caption == "My first car"));
        }
        [TestMethod]
        public void MobyGames_Search_GamePlatform_StreetRod_DOS_Covers()
        {
            //We have to atleast wait 1 second before making another request so we force the sleep
            Thread.Sleep(1000);
            var results = mb.SearchGamePlatformCovers(434, 2);
            Assert.IsTrue(results.Count >= 1);
            Assert.IsTrue(results[0].countries.Count > 0);
            Assert.IsTrue(results[0].countries.Any(a => a == "United States"));
            Assert.IsTrue(results[0].covers.Count > 1);
            Assert.IsTrue(results[0].covers.Any(a => a.description == "Street Rod front"));
            Assert.IsTrue(results[0].covers.Any(a => a.scan_of == "Front Cover"));
            Assert.IsTrue(results[0].covers.Any(a => a.description == "Street Rod back"));
            Assert.IsTrue(results[0].covers.Any(a => a.scan_of == "Back Cover"));
        }
    }
}
