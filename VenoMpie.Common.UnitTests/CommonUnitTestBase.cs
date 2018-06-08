using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace VenoMpie.Common.UnitTests
{
    [TestClass]
    public class CommonUnitTestBase
    {
        protected static string resourceDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
    }
}
