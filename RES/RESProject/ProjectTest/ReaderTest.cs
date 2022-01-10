using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RESProject.Classes;
using RESProject.Components;

namespace ProjectTest
{
    [TestClass]
    public class ReaderTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Reader reader = Reader.Instance();
            Assert.AreEqual(true, reader.ReadInterval(Codes.CODE_DIGITAL, DateTime.Parse("12:14"), DateTime.Parse("12:13")));
            Assert.AreEqual(true, reader.ReadInterval(Codes.CODE_ANALOG, DateTime.Parse("12:14"), DateTime.Parse("12:13")));
            Assert.AreEqual(true, reader.ReadInterval(Codes.CODE_CONSUMER, DateTime.Parse("12:14"), DateTime.Parse("12:13")));
            Assert.AreEqual(true, reader.ReadInterval(Codes.CODE_CUSTOM, DateTime.Parse("12:14"), DateTime.Parse("12:13")));
            Assert.AreEqual(true, reader.ReadInterval(Codes.CODE_LIMITSET, DateTime.Parse("12:14"), DateTime.Parse("12:13")));
            Assert.AreEqual(true, reader.ReadInterval(Codes.CODE_MOTION, DateTime.Parse("12:14"), DateTime.Parse("12:13")));
            Assert.AreEqual(true, reader.ReadInterval(Codes.CODE_MULTIPLENODE, DateTime.Parse("12:14"), DateTime.Parse("12:13")));
            Assert.AreEqual(true, reader.ReadInterval(Codes.CODE_SENSOR, DateTime.Parse("12:14"), DateTime.Parse("12:13")));
            Assert.AreEqual(true, reader.ReadInterval(Codes.CODE_SINGLENODE, DateTime.Parse("12:14"), DateTime.Parse("12:13")));
            Assert.AreEqual(true, reader.ReadInterval(Codes.CODE_SOURCE, DateTime.Parse("12:14"), DateTime.Parse("12:13")));
            Assert.AreEqual(false, reader.ReadInterval((Codes)13, DateTime.Parse("12:14"), DateTime.Parse("12:13")));
            Assert.AreEqual(true, reader.ReadInterval(Codes.CODE_DIGITAL, new DateTime(), DateTime.Parse("12:13")));
            Assert.AreEqual(true, reader.ReadInterval(Codes.CODE_DIGITAL, DateTime.Parse("12:13"), new DateTime()));
            Assert.AreEqual(true, reader.ReadInterval(Codes.CODE_DIGITAL, new DateTime(), new DateTime()));

        }
    }
}
