using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RESProject.Classes;
using RESProject.Components;

namespace ProjectTest
{
    [TestClass]
    public class DumpingBufferTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            DumpingBuffer db = new DumpingBuffer();

            Assert.AreEqual(true, DumpingBuffer.WriteToHistory(1, 95));
            Assert.AreEqual(true, DumpingBuffer.WriteToHistory(2, 95));
            Assert.AreEqual(true, DumpingBuffer.WriteToHistory(3, 95));
            Assert.AreEqual(true, DumpingBuffer.WriteToHistory(4, 95));
            Assert.AreEqual(true, DumpingBuffer.WriteToHistory(5, 95));
            Assert.AreEqual(true, DumpingBuffer.WriteToHistory(6, 95));
            Assert.AreEqual(true, DumpingBuffer.WriteToHistory(7, 95));
            Assert.AreEqual(true, DumpingBuffer.WriteToHistory(8, 95));
            Assert.AreEqual(true, DumpingBuffer.WriteToHistory(9, 95));
            Assert.AreEqual(false, DumpingBuffer.WriteToHistory(-1, 95));
            Assert.AreEqual(false, DumpingBuffer.WriteToHistory(11, 95));
            Assert.AreEqual(false, DumpingBuffer.WriteToHistory(1412, 95));
            Assert.AreEqual(false, DumpingBuffer.WriteToHistory(1, 5412));
        }
        [TestMethod]
        public void TestMethod2()
        {
            Assert.AreEqual(true, DumpingBuffer.DelaySending(1, 95));
            Assert.AreEqual(true, DumpingBuffer.DelaySending(2, 95));
            Assert.AreEqual(true, DumpingBuffer.DelaySending(3, 95));
            Assert.AreEqual(true, DumpingBuffer.DelaySending(4, 95));
            Assert.AreEqual(true, DumpingBuffer.DelaySending(5, 95));
            Assert.AreEqual(true, DumpingBuffer.DelaySending(6, 95));
            Assert.AreEqual(true, DumpingBuffer.DelaySending(7, 95));
            Assert.AreEqual(true, DumpingBuffer.DelaySending(8, 95));
            Assert.AreEqual(true, DumpingBuffer.DelaySending(9, 95));
            Assert.AreEqual(false, DumpingBuffer.DelaySending(-1, 95));
            Assert.AreEqual(false, DumpingBuffer.DelaySending(11, 95));
            Assert.AreEqual(false, DumpingBuffer.DelaySending(1412, 95));
            Assert.AreEqual(false, DumpingBuffer.DelaySending(1, 5412));
        }
        [TestMethod]
        public void TestMethod3()
        {
            Assert.AreEqual(true, DumpingBuffer.SendToHistorical());
        }
    }
}
