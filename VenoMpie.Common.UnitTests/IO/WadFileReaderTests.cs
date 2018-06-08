using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using VenoMpie.Common.IO.FileReaders.Emulation.DOS.Doom;

namespace VenoMpie.Common.UnitTests.IO
{
    [TestClass]
    public class WadFileReaderTests : CommonUnitTestBase
    {
        //Change this to wherever your IWAD files are
        private static string Doom1RegisteredMap = @"D:\Games\SteamLibrary\SteamApps\common\Ultimate Doom\base\DOOM.WAD";
        private static string Doom2RegisteredMap = @"D:\Games\SteamLibrary\SteamApps\common\Doom 2\base\DOOM2.WAD";
        private static string HereticRegisteredMap = @"D:\Games\SteamLibrary\SteamApps\common\Heretic Shadow of the Serpent Riders\base\HERETIC.WAD";
        private static string HexenRegisteredMap = @"D:\Games\SteamLibrary\SteamApps\common\Hexen\base\HEXEN.WAD";
        private static string HexenDODCRegisteredMap = @"D:\Games\SteamLibrary\SteamApps\common\Hexen Deathkings of the Dark Citadel\base\HEXEN.WAD";
        [TestMethod]
        public void WadFileReader_TestPWADReader()
        {
            WADFile_Doom file = new WADFile_Doom(new FileStream(Path.Combine(resourceDir, "FileSignature_Emulation_PWADFile.wad"), FileMode.Open));
            Assert.IsNotNull(file.Header);
        }
        [TestMethod]
        public void WadFileReader_TestIWADReader_Doom1()
        {
            WADFile_Doom file = new WADFile_Doom(new FileStream(Doom1RegisteredMap, FileMode.Open));
            Assert.IsNotNull(file.Header);
            Assert.IsTrue(file.Maps.Any(a => a.MapNumber == "E1M9"));
            Assert.IsTrue(file.Maps.Any(a => a.MapNumber == "E1M5"));
            Assert.IsTrue(file.Maps.Any(a => a.MapNumber == "E2M9"));
            Assert.IsTrue(file.Maps.Any(a => a.MapNumber == "E2M1"));
            Assert.IsTrue(file.Maps.Any(a => a.MapNumber == "E3M9"));
            Assert.IsTrue(file.Maps.Any(a => a.MapNumber == "E3M7"));
            Assert.IsTrue(file.Maps.Any(a => a.MapNumber == "E4M2"));
            //Make sure we get the last map in as well
            Assert.IsTrue(file.Maps.Any(a => a.MapNumber == "E4M9"));
            //This is my breakpoint, the CyberDemon map :P
            //I already know what I want out of this map so it's easy to use it
            var map = file.Maps.First(a => a.MapNumber == "E2M8");
        }
        [TestMethod]
        public void WadFileReader_TestIWADReader_Doom2()
        {
            WADFile_Doom file = new WADFile_Doom(new FileStream(Doom2RegisteredMap, FileMode.Open));
            Assert.IsNotNull(file.Header);
            Assert.IsTrue(file.Maps.Any(a => a.MapNumber == "MAP01"));
            Assert.IsTrue(file.Maps.Any(a => a.MapNumber == "MAP05"));
            Assert.IsTrue(file.Maps.Any(a => a.MapNumber == "MAP14"));
            //Make sure we get the last map in as well
            Assert.IsTrue(file.Maps.Any(a => a.MapNumber == "MAP32"));
            //Entryway!
            var map = file.Maps.First(a => a.MapNumber == "MAP01");
        }
        [TestMethod]
        public void WadFileReader_TestIWADReader_Heretic()
        {
            WADFile_Heretic file = new WADFile_Heretic(new FileStream(HereticRegisteredMap, FileMode.Open));
            Assert.IsNotNull(file.Header);
            Assert.IsTrue(file.Maps.Any(a => a.MapNumber == "E1M9"));
            Assert.IsTrue(file.Maps.Any(a => a.MapNumber == "E1M5"));
            Assert.IsTrue(file.Maps.Any(a => a.MapNumber == "E2M9"));
            Assert.IsTrue(file.Maps.Any(a => a.MapNumber == "E2M1"));
            Assert.IsTrue(file.Maps.Any(a => a.MapNumber == "E3M9"));
            Assert.IsTrue(file.Maps.Any(a => a.MapNumber == "E3M7"));
            Assert.IsTrue(file.Maps.Any(a => a.MapNumber == "E4M2"));
            Assert.IsTrue(file.Maps.Any(a => a.MapNumber == "E4M9"));
            Assert.IsTrue(file.Maps.Any(a => a.MapNumber == "E5M6"));
            Assert.IsTrue(file.Maps.Any(a => a.MapNumber == "E5M9"));
            //Make sure we get the last map in as well
            Assert.IsTrue(file.Maps.Any(a => a.MapNumber == "E6M3"));
            //First map that I know the best
            var map = file.Maps.First(a => a.MapNumber == "E1M1");
        }
        [TestMethod]
        public void WadFileReader_TestIWADReader_Hexen()
        {
            WADFile_Hexen file = new WADFile_Hexen(new FileStream(HexenRegisteredMap, FileMode.Open));
            Assert.IsNotNull(file.Header);
            Assert.IsTrue(file.Maps.Any(a => a.MapNumber == "MAP01"));
            Assert.IsTrue(file.Maps.Any(a => a.MapNumber == "MAP05"));
            //No use testing for MAP07, there is nothing :P
            //Hexen's maps are not consecutively numbered
            //https://doomwiki.org/wiki/Hexen
            Assert.IsTrue(file.Maps.Any(a => a.MapNumber == "MAP08"));
            Assert.IsTrue(file.Maps.Any(a => a.MapNumber == "MAP33"));
            //Make sure we get the last map in as well
            Assert.IsTrue(file.Maps.Any(a => a.MapNumber == "MAP40"));
            //First map that I know the best
            var map = file.Maps.First(a => a.MapNumber == "MAP01");
        }
        [TestMethod]
        public void WadFileReader_TestIWADReader_Hexen_DeathkingsOfTheDarkCitidel()
        {
            WADFile_Hexen file = new WADFile_Hexen(new FileStream(HexenDODCRegisteredMap, FileMode.Open));
            Assert.IsNotNull(file.Header);
            Assert.IsTrue(file.Maps.Any(a => a.MapNumber == "MAP01"));
            Assert.IsTrue(file.Maps.Any(a => a.MapNumber == "MAP05"));
            Assert.IsTrue(file.Maps.Any(a => a.MapNumber == "MAP08"));
            Assert.IsTrue(file.Maps.Any(a => a.MapNumber == "MAP33"));
            //Make sure we get the last map in as well
            Assert.IsTrue(file.Maps.Any(a => a.MapNumber == "MAP40"));
            //First map that I know the best
            var map = file.Maps.First(a => a.MapNumber == "MAP01");
        }
        [TestMethod]
        public void WadFileReader_TestIWADReader_Reject()
        {
            WADFile_Doom file = new WADFile_Doom(new FileStream(HereticRegisteredMap, FileMode.Open));
            var map = file.Maps.First(a => a.MapNumber == "E1M1");
            //Make sure we atleast get a proper reject map
            Assert.IsTrue(map.Reject.RejectMap[0, 25] == map.Reject.RejectMap[25, 0]);
            Assert.IsTrue(map.Reject.RejectMap[1, 5] == map.Reject.RejectMap[5, 1]);
            Assert.IsTrue(map.Reject.RejectMap[1, 27] == map.Reject.RejectMap[27, 1]);
            Assert.IsTrue(map.Reject.RejectMap[10, 33] == map.Reject.RejectMap[33, 10]);
        }
        //What a waste. Will try using MonoGame or something to play the sounds
        //[TestMethod]
        //public void WadFileReader_TestIWADReader_PlayPCSound()
        //{
        //    var doomplayer = new DoomPCSpeakerPlayer();
        //    WADFile_Doom file = new WADFile_Doom(new FileStream(Doom1RegisteredMap, FileMode.Open));
        //    var sound = file.PCSpeakerSounds[0];
        //    doomplayer.PlaySound(sound.Data);
        //}
    }
}
