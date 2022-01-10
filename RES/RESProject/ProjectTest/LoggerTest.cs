using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RESProject.Components;

namespace ProjectTest
{
    [TestClass]
    public class LoggerTest
    {
        [TestMethod]
        [ExpectedException(typeof(Exception), "Null argument passed.")]
        public void TestMethod1()
        {
            Logger loger = new Logger();
            loger.LogEvent(null, "fsadf");
            loger.LogEvent("fsadf", null);
            loger.LogEvent(null, null);
        }
        [TestMethod]

        public void TestMethod2()
        {
            Logger loger = Logger.Instance();
            try
            {
                loger.LogEvent("asfdas", "fasdf");
            }
            catch(Exception e)
            {
                Assert.Fail("Expected no exception but got one");
            }




        }
    }
}
