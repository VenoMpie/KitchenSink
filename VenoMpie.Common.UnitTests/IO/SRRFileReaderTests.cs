using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using VenoMpie.Common.UnitTests;
using VenoMpie.Common.IO.FileReaders.Compression.SRR;

namespace Gamebase.NET.UnitTests.IO
{
    [TestClass]
    public class SRRFileReaderTests : CommonUnitTestBase
    {
        [TestMethod]
        public void SRRReader_TestSRRReadFile_AOE2_PRS04()
        {
            using (SRRReader testFileReader = new SRRReader(Path.Combine(resourceDir, "Age.of.Empires.II.HD-RELOADED.srr")))
            {
                Assert.AreEqual("pyReScene Auto 0.4", testFileReader.Header.AppName);
                Assert.AreEqual(22, testFileReader.RarFiles.Count);
                Assert.AreEqual("rld-aoe2hd.rar", testFileReader.RarFiles[0].FileName);
                Assert.AreEqual("rld-aoe2hd.r20", testFileReader.RarFiles[21].FileName);
                Assert.AreEqual(2, testFileReader.StoredFiles.Count);
                Assert.AreEqual("reloaded.nfo", testFileReader.StoredFiles[0].FileName);
                Assert.AreEqual("rld-aoe2hd.sfv", testFileReader.StoredFiles[1].FileName);
                Assert.IsFalse(testFileReader.RarFiles.Any(a => a.FileName == string.Empty));
                Assert.IsFalse(testFileReader.PackedFiles.Any(a => a.FileName == string.Empty));
                Assert.IsFalse(testFileReader.StoredFiles.Any(a => a.FileName == string.Empty));
                Assert.IsTrue(testFileReader.PackedFiles.Any(a => a.FileCRC == 0x9398514D));
                Assert.IsTrue(testFileReader.PackedFiles.All(a => a.FileName == "rld-aoe2hd.iso"));
            }
        }
        [TestMethod]
        public void SRRReader_TestSRRReadFile_AmericanGreed_PRS05()
        {
            using (SRRReader testFileReader = new SRRReader(Path.Combine(resourceDir, "American.Greed.S11E04.HDTV.x264-WaLMaRT.srr")))
            {
                Assert.AreEqual("pyReScene Auto 0.5", testFileReader.Header.AppName);
                Assert.AreEqual(19, testFileReader.RarFiles.Count);
                Assert.AreEqual("wmt-american.greed.11x4_sd.rar", testFileReader.RarFiles[0].FileName);
                Assert.AreEqual("wmt-american.greed.11x4_sd.r17", testFileReader.RarFiles[18].FileName);
                Assert.AreEqual(3, testFileReader.StoredFiles.Count);
                Assert.AreEqual("wmt-american.greed.11x4_sd.nfo", testFileReader.StoredFiles[0].FileName);
                Assert.AreEqual("Sample/wmt-american.greed.11x4_sd.sample.srs", testFileReader.StoredFiles[1].FileName);
                Assert.AreEqual("wmt-american.greed.11x4_sd.sfv", testFileReader.StoredFiles[2].FileName);
                Assert.IsFalse(testFileReader.RarFiles.Any(a => a.FileName == string.Empty));
                Assert.IsFalse(testFileReader.PackedFiles.Any(a => a.FileName == string.Empty));
                Assert.IsFalse(testFileReader.StoredFiles.Any(a => a.FileName == string.Empty));
                Assert.IsTrue(testFileReader.PackedFiles.Any(a => a.FileCRC == 0x44358D40));
                Assert.IsTrue(testFileReader.PackedFiles.All(a => a.FileName == "american.greed.11x4_sd.mkv"));
            }
        }
        [TestMethod]
        public void SRRReader_TestSRRReadFile_AlanWakeXB360_PRS05()
        {
            //This one has Recovery Blocks
            using (SRRReader testFileReader = new SRRReader(Path.Combine(resourceDir, "Alan.Wake.XBOX360-GLoBAL.srr")))
            {
                Assert.AreEqual("pyReScene Auto 0.5", testFileReader.Header.AppName);
                Assert.AreEqual(67, testFileReader.RarFiles.Count);
                Assert.AreEqual("awx360.rar", testFileReader.RarFiles[0].FileName);
                Assert.AreEqual("awx360.r65", testFileReader.RarFiles[66].FileName);
                Assert.AreEqual(3, testFileReader.StoredFiles.Count);
                Assert.AreEqual("awx360.nfo", testFileReader.StoredFiles[0].FileName);
                Assert.AreEqual("awx360.jpg", testFileReader.StoredFiles[1].FileName);
                Assert.AreEqual("awx360.sfv", testFileReader.StoredFiles[2].FileName);
                Assert.IsFalse(testFileReader.RarFiles.Any(a => a.FileName == string.Empty));
                Assert.IsFalse(testFileReader.PackedFiles.Any(a => a.FileName == string.Empty));
                Assert.IsFalse(testFileReader.StoredFiles.Any(a => a.FileName == string.Empty));
                Assert.IsTrue(testFileReader.PackedFiles.Any(a => a.FileCRC == 0x5485A1F7));
                Assert.IsTrue(testFileReader.PackedFiles.Any(a => a.FileName == "awx360.iso"));
                Assert.IsTrue(testFileReader.PackedFiles.Any(a => a.FileName == "awx360.dvd"));
            }
        }
        [TestMethod]
        public void SRRReader_TestSRRReadFile_AmericanGods_PRS06()
        {
            using (SRRReader testFileReader = new SRRReader(Path.Combine(resourceDir, "American.Gods.S01E01.HDTV.x264-FLEET.srr")))
            {
                Assert.AreEqual("pyReScene Auto 0.6", testFileReader.Header.AppName);
                Assert.AreEqual(35, testFileReader.RarFiles.Count);
                Assert.AreEqual("american.gods.s01e01.hdtv.x264-fleet.rar", testFileReader.RarFiles[0].FileName);
                Assert.AreEqual("american.gods.s01e01.hdtv.x264-fleet.r33", testFileReader.RarFiles[34].FileName);
                Assert.AreEqual(3, testFileReader.StoredFiles.Count);
                Assert.AreEqual("american.gods.s01e01.hdtv.x264-fleet.nfo", testFileReader.StoredFiles[0].FileName);
                Assert.AreEqual("Sample/american.gods.s01e01.hdtv.x264-fleet.sample.srs", testFileReader.StoredFiles[1].FileName);
                Assert.AreEqual("american.gods.s01e01.hdtv.x264-fleet.sfv", testFileReader.StoredFiles[2].FileName);
                Assert.IsFalse(testFileReader.RarFiles.Any(a => a.FileName == string.Empty));
                Assert.IsFalse(testFileReader.PackedFiles.Any(a => a.FileName == string.Empty));
                Assert.IsFalse(testFileReader.StoredFiles.Any(a => a.FileName == string.Empty));
                Assert.IsTrue(testFileReader.PackedFiles.Any(a => a.FileCRC == 0xBAF74648));
                Assert.IsTrue(testFileReader.PackedFiles.All(a => a.FileName == "American.Gods.S01E01.HDTV.x264-FLEET.mkv"));
            }
        }
        [TestMethod]
        public void SRRReader_TestSRRReadFile_TarzanJane_PRS06()
        {
            using (SRRReader testFileReader = new SRRReader(Path.Combine(resourceDir, "Tarzan.and.Jane.2017.S01E04.WEBRiP.x264-QCF.srr")))
            {
                Assert.AreEqual("pyReScene Auto 0.6", testFileReader.Header.AppName);
                Assert.AreEqual(9, testFileReader.RarFiles.Count);
                Assert.AreEqual("tarzan.and.jane.s01e04.webrip.x264-qcf.rar", testFileReader.RarFiles[0].FileName);
                Assert.AreEqual("tarzan.and.jane.s01e04.webrip.x264-qcf.r07", testFileReader.RarFiles[8].FileName);
                Assert.AreEqual(3, testFileReader.StoredFiles.Count);
                Assert.AreEqual("tarzan.and.jane.s01e04.webrip.x264-qcf.nfo", testFileReader.StoredFiles[0].FileName);
                Assert.AreEqual("Sample/sample-tarzan.and.jane.s01e04.webrip.x264-qcf.srs", testFileReader.StoredFiles[1].FileName);
                Assert.AreEqual("tarzan.and.jane.s01e04.webrip.x264-qcf.sfv", testFileReader.StoredFiles[2].FileName);
                Assert.IsFalse(testFileReader.RarFiles.Any(a => a.FileName == string.Empty));
                Assert.IsFalse(testFileReader.PackedFiles.Any(a => a.FileName == string.Empty));
                Assert.IsFalse(testFileReader.StoredFiles.Any(a => a.FileName == string.Empty));
                Assert.IsTrue(testFileReader.PackedFiles.Any(a => a.FileCRC == 0x93039C70));
                Assert.IsTrue(testFileReader.PackedFiles.All(a => a.FileName == "Tarzan.and.Jane.S01E04.WEBRiP.x264-QCF.mkv"));
            }
        }
        [TestMethod]
        public void SRRReader_TestSRRReadFile_Preacher_PRS06()
        {
            using (SRRReader testFileReader = new SRRReader(Path.Combine(resourceDir, "Preacher.S02E05.HDTV.x264-SVA.srr")))
            {
                Assert.AreEqual("pyReScene Auto 0.6", testFileReader.Header.AppName);
                Assert.AreEqual(14, testFileReader.RarFiles.Count);
                Assert.AreEqual("preacher.s02e05.hdtv.x264-sva.rar", testFileReader.RarFiles[0].FileName);
                Assert.AreEqual("preacher.s02e05.hdtv.x264-sva.r12", testFileReader.RarFiles[13].FileName);
                Assert.AreEqual(3, testFileReader.StoredFiles.Count);
                Assert.AreEqual("preacher.s02e05.hdtv.x264-sva.nfo", testFileReader.StoredFiles[0].FileName);
                Assert.AreEqual("Sample/sample-preacher.s02e05.hdtv.x264-sva.srs", testFileReader.StoredFiles[1].FileName);
                Assert.AreEqual("preacher.s02e05.hdtv.x264-sva.sfv", testFileReader.StoredFiles[2].FileName);
                Assert.IsFalse(testFileReader.RarFiles.Any(a => a.FileName == string.Empty));
                Assert.IsFalse(testFileReader.PackedFiles.Any(a => a.FileName == string.Empty));
                Assert.IsFalse(testFileReader.StoredFiles.Any(a => a.FileName == string.Empty));
                Assert.IsTrue(testFileReader.PackedFiles.Any(a => a.FileCRC == 0xD34EB0F7));
                Assert.IsTrue(testFileReader.PackedFiles.All(a => a.FileName == "Preacher.S02E05.HDTV.x264-SVA.mkv"));
            }
        }
        [TestMethod]
        public void SRRReader_TestSRRReadFile_MasterChef_PRS06()
        {
            using (SRRReader testFileReader = new SRRReader(Path.Combine(resourceDir, "MasterChef.US.S08E08.WEB.h264-TBS.srr")))
            {
                Assert.AreEqual("pyReScene Auto 0.6", testFileReader.Header.AppName);
                Assert.AreEqual(23, testFileReader.RarFiles.Count);
                Assert.AreEqual("masterchef.us.s08e08.web.h264-tbs.rar", testFileReader.RarFiles[0].FileName);
                Assert.AreEqual("masterchef.us.s08e08.web.h264-tbs.r21", testFileReader.RarFiles[22].FileName);
                Assert.AreEqual(3, testFileReader.StoredFiles.Count);
                Assert.AreEqual("masterchef.us.s08e08.web.h264-tbs.nfo", testFileReader.StoredFiles[0].FileName);
                Assert.AreEqual("Sample/sample-masterchef.us.s08e08.web.h264-tbs.srs", testFileReader.StoredFiles[1].FileName);
                Assert.AreEqual("masterchef.us.s08e08.web.h264-tbs.sfv", testFileReader.StoredFiles[2].FileName);
                Assert.IsFalse(testFileReader.RarFiles.Any(a => a.FileName == string.Empty));
                Assert.IsFalse(testFileReader.PackedFiles.Any(a => a.FileName == string.Empty));
                Assert.IsFalse(testFileReader.StoredFiles.Any(a => a.FileName == string.Empty));
                Assert.IsTrue(testFileReader.PackedFiles.Any(a => a.FileCRC == 0xCEAFB372));
                Assert.IsTrue(testFileReader.PackedFiles.All(a => a.FileName == "masterchef.us.s08e08.web.h264-tbs.mkv"));
            }
        }
        [TestMethod]
        public void SRRReader_TestSRRReadFile_Matrix_NetGui133()
        {
            using (SRRReader testFileReader = new SRRReader(Path.Combine(resourceDir, "The.Matrix.1999.iNTERNAL.DVDRip.XviD.AC3-XviK.srr")))
            {
                Assert.AreEqual("ReScene .NET 1.3.3 GUI (beta)", testFileReader.Header.AppName);
                Assert.AreEqual(100, testFileReader.RarFiles.Count);
                Assert.AreEqual("CD1/xvik-matrixa.rar", testFileReader.RarFiles[0].FileName);
                Assert.AreEqual("CD2/xvik-matrixb.r48", testFileReader.RarFiles[99].FileName);
                Assert.AreEqual(6, testFileReader.StoredFiles.Count);
                Assert.AreEqual("the.matrix.1999.internal.dvdrip.xvid.ac3-xvik.nfo", testFileReader.StoredFiles[0].FileName);
                Assert.AreEqual("Sample/sample-matrix-xvik.srs", testFileReader.StoredFiles[1].FileName);
                Assert.AreEqual("Subs/subs-matrix-xvik.srr", testFileReader.StoredFiles[2].FileName);
                Assert.IsFalse(testFileReader.RarFiles.Any(a => a.FileName == string.Empty));
                Assert.IsFalse(testFileReader.PackedFiles.Any(a => a.FileName == string.Empty));
                Assert.IsFalse(testFileReader.StoredFiles.Any(a => a.FileName == string.Empty));
                Assert.IsTrue(testFileReader.PackedFiles.Any(a => a.FileCRC == 0x3E9D0351));
                Assert.IsTrue(testFileReader.PackedFiles.Any(a => a.FileName == "The.Matrix.1999.iNTERNAL.DVDRip.XviD.AC3.CD1-XviK.avi"));
            }
        }
        [TestMethod]
        public void SRRReader_TestSRRReadFile_ApacheGOG_PRS06()
        {
            using (SRRReader testFileReader = new SRRReader(Path.Combine(resourceDir, "Apache.Longbow.GoG.Classic-I_KnoW.srr")))
            {
                Assert.AreEqual("pyReScene Auto 0.6", testFileReader.Header.AppName);
                Assert.AreEqual(testFileReader.RarFiles.Count, 19);
                Assert.AreEqual(testFileReader.RarFiles[0].FileName, "ik-along.001");
                Assert.AreEqual(testFileReader.RarFiles[18].FileName, "ik-along.019");
                Assert.AreEqual(testFileReader.StoredFiles.Count, 2);
                Assert.AreEqual(testFileReader.StoredFiles[0].FileName, "iknow.nfo");
                Assert.AreEqual(testFileReader.StoredFiles[1].FileName, "ik-along.sfv");
                Assert.IsFalse(testFileReader.RarFiles.Any(a => a.FileName == string.Empty));
                Assert.IsFalse(testFileReader.PackedFiles.Any(a => a.FileName == string.Empty));
                Assert.IsFalse(testFileReader.StoredFiles.Any(a => a.FileName == string.Empty));
                Assert.IsTrue(testFileReader.PackedFiles.Any(a => a.FileCRC == 0x02904B74));
                Assert.IsTrue(testFileReader.PackedFiles.Any(a => a.FileName == "ik-along.bin"));
                Assert.IsTrue(testFileReader.PackedFiles.Any(a => a.FileName == "ik-along.cue"));
            }
        }
        [TestMethod]
        public void SRRReader_TestSRRReadFile_ContractJack_Usenet15()
        {
            using (SRRReader testFileReader = new SRRReader(Path.Combine(resourceDir, "Contract_JACK-FLT.srr")))
            {
                Assert.AreEqual("pyReScene Usenet 1.5", testFileReader.Header.AppName);
                Assert.AreEqual(testFileReader.RarFiles.Count, 76);
                Assert.AreEqual(testFileReader.RarFiles[0].FileName, "flt-cj1.001");
                Assert.AreEqual(testFileReader.RarFiles[75].FileName, "flt-cj2.023");
                Assert.AreEqual(testFileReader.StoredFiles.Count, 3);
                Assert.AreEqual(testFileReader.StoredFiles[0].FileName, "flt-cj.nfo");
                Assert.AreEqual(testFileReader.StoredFiles[1].FileName, "CD1/flt-cj1.sfv");
                Assert.AreEqual(testFileReader.StoredFiles[2].FileName, "CD2/flt-cj2.sfv");
                Assert.IsFalse(testFileReader.RarFiles.Any(a => a.FileName == string.Empty));
                Assert.IsFalse(testFileReader.PackedFiles.Any(a => a.FileName == string.Empty));
                Assert.IsFalse(testFileReader.StoredFiles.Any(a => a.FileName == string.Empty));
                Assert.IsTrue(testFileReader.PackedFiles.Any(a => a.FileCRC == 0xFE1D0098));
                Assert.IsTrue(testFileReader.PackedFiles.Any(a => a.FileName == "FLT-CJ1.CUE"));
                Assert.IsTrue(testFileReader.PackedFiles.Any(a => a.FileName == "FLT-CJ1.BIN"));
            }
        }
        [TestMethod]
        public void SRRReader_TestSRRReadFile_AgeOfMythology_PRS05()
        {
            using (SRRReader testFileReader = new SRRReader(Path.Combine(resourceDir, "Age_of_Mythology-FLT.srr")))
            {
                Assert.AreEqual("pyReScene 0.5", testFileReader.Header.AppName);
                Assert.AreEqual(testFileReader.RarFiles.Count, 62);
                Assert.AreEqual(testFileReader.RarFiles[0].FileName, "CD1/flt-am2a.001");
                Assert.AreEqual(testFileReader.RarFiles[61].FileName, "CD2/flt-am2b.037");
                Assert.AreEqual(testFileReader.StoredFiles.Count, 3);
                Assert.AreEqual(testFileReader.StoredFiles[0].FileName, "FLT-AM2.nfo");
                Assert.AreEqual(testFileReader.StoredFiles[1].FileName, "CD1/FLT-AM2A.sfv");
                Assert.AreEqual(testFileReader.StoredFiles[2].FileName, "CD2/FLT-AM2B.SFV");
                Assert.IsFalse(testFileReader.RarFiles.Any(a => a.FileName == string.Empty));
                Assert.IsFalse(testFileReader.PackedFiles.Any(a => a.FileName == string.Empty));
                Assert.IsFalse(testFileReader.StoredFiles.Any(a => a.FileName == string.Empty));
                Assert.IsTrue(testFileReader.PackedFiles.Any(a => a.FileCRC == 0x8E32A059));
                Assert.IsTrue(testFileReader.PackedFiles.Any(a => a.FileName == "FLT-AM2A.CUE"));
                Assert.IsTrue(testFileReader.PackedFiles.Any(a => a.FileName == "FLT-AM2A.BIN"));
            }
        }
    }
}
