using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VenoMpie.Common.IO.FileSignatures.Compression;
using VenoMpie.Common.IO.FileSignatures.Emulation;
using System.IO;
using VenoMpie.Common.UnitTests;

namespace Gamebase.NET.UnitTests.IO
{
    [TestClass]
    public class FileSignatureTests : CommonUnitTestBase
    {
        #region Compression
        [TestMethod]
        public void FileSignatures_TestAceFile00()
        {
            Ace testFile = new Ace(Path.Combine(resourceDir, "FileSignature_Compression_AceFile.c00"));
            Assert.AreEqual(true, testFile.EntireFileIsValid);
        }
        #region RAR
        [TestMethod]
        public void FileSignatures_TestRarFile()
        {
            Rar testFile = new Rar(Path.Combine(resourceDir, "FileSignature_Compression_RarFile.rar"));
            Assert.AreEqual(true, testFile.EntireFileIsValid);
        }
        [TestMethod]
        public void FileSignatures_TestRarFile00()
        {
            Rar testFile = new Rar(Path.Combine(resourceDir, "FileSignature_Compression_RarFile.r00"));
            Assert.AreEqual(true, testFile.EntireFileIsValid);
        }
        [TestMethod]
        public void FileSignatures_TestRarFile01()
        {
            Rar testFile = new Rar(Path.Combine(resourceDir, "FileSignature_Compression_RarFile.r01"));
            Assert.AreEqual(true, testFile.EntireFileIsValid);
        }
        [TestMethod]
        public void FileSignatures_TestRarFile02()
        {
            Rar testFile = new Rar(Path.Combine(resourceDir, "FileSignature_Compression_RarFile.r02"));
            Assert.AreEqual(true, testFile.EntireFileIsValid);
        }
        #endregion
        [TestMethod]
        public void FileSignatures_TestSevenZipFile()
        {
            SevenZip testFile = new SevenZip(Path.Combine(resourceDir, "FileSignature_Compression_SevenZipFile.7z"));
            Assert.AreEqual(true, testFile.EntireFileIsValid);
        }
        [TestMethod]
        public void FileSignatures_TestSRRFile()
        {
            SRR testFile = new SRR(Path.Combine(resourceDir, "FileSignature_Compression_SRRFile.srr"));
            Assert.AreEqual(true, testFile.EntireFileIsValid);
        }
        [TestMethod]
        public void FileSignatures_TestZipFile()
        {
            Zip testFile = new Zip(Path.Combine(resourceDir, "FileSignature_Compression_ZipFile.zip"));
            Assert.AreEqual(true, testFile.EntireFileIsValid);
        }
        #endregion
        #region Emulation
        [TestMethod]
        public void FileSignatures_TestNESFile()
        {
            NES testFile = new NES(Path.Combine(resourceDir, "FileSignature_Emulation_NESFile.nes"));
            Assert.AreEqual(true, testFile.EntireFileIsValid);
        }
        [TestMethod]
        public void FileSignatures_TestPSIDFile()
        {
            PSID testFile = new PSID(Path.Combine(resourceDir, "FileSignature_Emulation_PSIDFile.sid"));
            Assert.AreEqual(true, testFile.EntireFileIsValid);
        }
        [TestMethod]
        public void FileSignatures_TestPWADFile()
        {
            PWAD testFile = new PWAD(Path.Combine(resourceDir, "FileSignature_Emulation_PWADFile.wad"));
            Assert.AreEqual(true, testFile.EntireFileIsValid);
        }
        [TestMethod]
        public void FileSignatures_TestRSIDFile()
        {
            RSID testFile = new RSID(Path.Combine(resourceDir, "FileSignature_Emulation_RSIDFile.sid"));
            Assert.AreEqual(true, testFile.EntireFileIsValid);
        }
        #endregion
    }
}
