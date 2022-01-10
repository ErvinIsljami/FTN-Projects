using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RESProject.Components;
using RESProject.Classes;

namespace ProjectTest
{
    [TestClass]
    public class TestComponents
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.IsNotNull(new DumpingBuffer());
            Assert.IsNotNull(new Historical());
            Assert.IsNotNull(new Logger());
            Assert.IsNotNull(new Reader());
            Assert.IsNotNull(new Writer());
        }
    }
}
