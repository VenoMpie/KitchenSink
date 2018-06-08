using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using VenoMpie.Common.Security.Cryptography;

namespace VenoMpie.Common.UnitTests.Interfaces
{
    [TestClass]
    public class CRCTests
    {
        private const string testString = "123456789";
        private byte[] testStringBytes = System.Text.Encoding.ASCII.GetBytes(testString);
        private byte[] testStringCalculatedBytes = new byte[] { 0xff, 0xff, 0xff, 0xff, 0xcb, 0xf4, 0x39, 0x26 };
        private enum TestChecksCRC32 : uint
        {
            CRC32 = 0xCBF43926,
            CRC32BZip2 = 0xFC891918,
            CRC32C = 0xE3069283,
            CRC32D = 0x87315576,
            CRC32MPEG = 0x0376E6E7,
            POSIX = 0x765E7680,
            CRC32Q = 0x3010BF7F,
            JAMCRC = 0x340BC6D9,
            XFER = 0xBD0BE338
        }

        [TestMethod]
        public void TestCalculateCRC32_ASCIIString_FullArray()
        {
            CRC crcCalculator = new CRC(CRCEnum.CRC32);
            string retCRC = crcCalculator.CalculateCRC(testStringBytes);
            Assert.AreEqual("CBF43926", retCRC);
        }
        [TestMethod]
        public void TestCalculateCRC32_ASCIIString_FullArray_Incremental()
        {
            CRC crcCalculator = new CRC(CRCEnum.CRC32);
            for (var i = 0; i < testStringBytes.Length; i++)
            {
                crcCalculator.Compute(testStringBytes[i]);
            }
            crcCalculator.FinalizeCompute();
            string retCRC = crcCalculator.ToString();
            Assert.AreEqual("CBF43926", retCRC);
        }
        [TestMethod]
        public void TestCalculateCRC32_ASCIIString_FullArray_ToUInt32()
        {
            CRC crcCalculator = new CRC(CRCEnum.CRC32);
            crcCalculator.ComputeHash(testStringBytes);
            uint crcuint32 = crcCalculator.ToUInt32();
            Assert.AreEqual(Convert.ToUInt32("CBF43926", 16), crcuint32);
        }
        [TestMethod]
        public void TestCalculateCRC32_ASCIIString_CRC32Standard()
        {
            //CRC32 crcCalculatorStandard = new CRC32(CRCEnum.CRC32Standard);
            //string retCRC = crcCalculatorStandard.CalculateCRC(testStringBytes);
            //Assert.AreEqual("CBF43926", retCRC);
        }
        [TestMethod]
        public void TestCalculateCRC32_SeedGetsReset()
        {
            CRC crcCalculator = new CRC(CRCEnum.CRC32);
            string retCRC = crcCalculator.CalculateCRC(testStringBytes);
            Assert.AreEqual("CBF43926", retCRC);
            retCRC = crcCalculator.CalculateCRC(System.Text.Encoding.ASCII.GetBytes("123"));
            Assert.AreNotEqual("CBF43926", retCRC);
            retCRC = crcCalculator.CalculateCRC(testStringBytes);
            Assert.AreEqual("CBF43926", retCRC);
        }
    }
}
