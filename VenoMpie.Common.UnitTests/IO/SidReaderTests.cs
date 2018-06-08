using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VenoMpie.Common.IO.FileSignatures.Compression;
using VenoMpie.Common.IO.FileSignatures.Emulation;
using System.IO;
using VenoMpie.Common.IO.FileReaders.Emulation.C64;
using VenoMpie.Common.IO.FileReaders;
using VenoMpie.Common.UnitTests;

namespace Gamebase.NET.UnitTests.IO
{
    [TestClass]
    public class SidReaderTests : CommonUnitTestBase
    {
        #region Emulation
        [TestMethod]
        public void SidReader_ReadPSIDBasic()
        {
            SIDBasicHeaderReader sr = new SIDBasicHeaderReader(Path.Combine(resourceDir, "FileSignature_Emulation_PSIDFile.sid"));
            sr.ReadSidHeader();
            Assert.AreEqual(SIDByteType.PSID, sr.Header.FileType);
        }
        [TestMethod]
        public void SidReader_ReadPSIDV2_Version()
        {
            SIDv2Reader sr = new SIDv2Reader(Path.Combine(resourceDir, "FileSignature_Emulation_PSIDFile.sid"));
            sr.ReadSidHeader();
            Assert.AreEqual(0x02, sr.Header.Version);
        }
        [TestMethod]
        public void SidReader_ReadPSIDV2_DataOffset()
        {
            SIDv2Reader sr = new SIDv2Reader(Path.Combine(resourceDir, "FileSignature_Emulation_PSIDFile.sid"));
            sr.ReadSidHeader();
            Assert.AreEqual(0x7C, sr.Header.DataOffset);
        }
        [TestMethod]
        public void SidReader_ReadRSIDBasic()
        {
            SIDBasicHeaderReader sr = new SIDBasicHeaderReader(Path.Combine(resourceDir, "FileSignature_Emulation_RSIDFile.sid"));
            sr.ReadSidHeader();
            Assert.AreEqual(SIDByteType.RSID, sr.Header.FileType);
        }
        [TestMethod]
        public void SidReader_ReadRSIDV2_Version()
        {
            SIDv2Reader sr = new SIDv2Reader(Path.Combine(resourceDir, "FileSignature_Emulation_RSIDFile.sid"));
            sr.ReadSidHeader();
            Assert.AreEqual(0x02, sr.Header.Version);
        }
        [TestMethod]
        public void SidReader_ReadRSIDV2_TrazDetails_Flags()
        {
            SIDv2Reader sr = new SIDv2Reader(Path.Combine(resourceDir, "Traz.sid"));
            sr.ReadSidHeader();
            Assert.AreEqual(0x02, sr.Header.Version);
            Assert.AreEqual(SIDVersion.MOS6581, sr.Header.SIDVersion);
            Assert.AreEqual(VideoStandardClock.PAL, sr.Header.VideoStandardClock);
        }
        [TestMethod]
        public void SidReader_ReadRSIDV2_TrazDetails_Data()
        {
            SIDv2Reader sr = new SIDv2Reader(Path.Combine(resourceDir, "Traz.sid"));
            sr.ReadSidHeader();
            byte[] data = sr.ReadSidData(sr.Header.DataOffset);
        }
        [TestMethod]
        public void SidReader_TestBitShifts()
        {
            byte retByte;
            retByte = ((1 >> 0) & 1);
            Assert.AreEqual(0x01, retByte);
            retByte = ((2 >> 1) & 1);
            Assert.AreEqual(0x01, retByte);
            retByte = ((4 >> 2) & 1);
            Assert.AreEqual(0x01, retByte);
            retByte = ((8 >> 3) & 1);
            Assert.AreEqual(0x01, retByte);
            retByte = ((16 >> 4) & 1);
            Assert.AreEqual(0x01, retByte);
            retByte = ((32 >> 5) & 1);
            Assert.AreEqual(0x01, retByte);
            retByte = ((64 >> 6) & 1);
            Assert.AreEqual(0x01, retByte);
            retByte = ((128 >> 7) & 1);
            Assert.AreEqual(0x01, retByte);
        }
        #endregion
    }
}
