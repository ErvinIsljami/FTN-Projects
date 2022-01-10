using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RESProject.Components;

namespace ProjectTest
{
    [TestClass]
    public class WriterTest
    {
        [TestMethod]
       
        public void TestMethod1()
        {
            Writer writer = new Writer();
            Assert.IsNotNull(new Writer());
            Assert.AreEqual(true, writer.WriteToFile(1, 123));
            Assert.AreEqual(true, writer.WriteToFile(2, 123));
            Assert.AreEqual(true, writer.WriteToFile(3, 123));
            Assert.AreEqual(true, writer.WriteToFile(4, 123));
            Assert.AreEqual(true, writer.WriteToFile(5, 123));
            Assert.AreEqual(true, writer.WriteToFile(6, 123));
            Assert.AreEqual(true, writer.WriteToFile(7, 123));
            Assert.AreEqual(true, writer.WriteToFile(8, 123));
            Assert.AreEqual(true, writer.WriteToFile(9, 123));
            Assert.AreEqual(false, writer.WriteToFile(12, 123));
            Assert.AreEqual(false, writer.WriteToFile(-1, 123));
            
            
        }
    }
}
