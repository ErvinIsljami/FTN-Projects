using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RESProject.Classes;
using System.Collections.Generic;

namespace ProjectTest
{
    [TestClass]
    public class TestClasses
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.IsNotNull(new CodeMaps());
            Assert.IsNotNull(new CollectionDescription());
            Assert.IsNotNull(new CollectionDescription(12, 2));
            Assert.IsNotNull(new CollectionDescription(13, 4, new List<DumpingProperty>()));
            Assert.IsNotNull(new DeltaCD());
            Assert.IsNotNull(new DeltaCD("asgafgsdfgrewqgewrger", new List<CollectionDescription>(), new List<CollectionDescription>(), new List<CollectionDescription>()));
            Assert.IsNotNull(new DumpingProperty());
            Assert.IsNotNull(new DumpingProperty(3, 4324));
            Assert.IsNotNull(new HistoricalDescription());
            Assert.IsNotNull(new HistoricalDescription(31, 2));
            Assert.IsNotNull(new HistoricalProperty());
            Assert.IsNotNull(new HistoricalProperty(Codes.CODE_ANALOG, 143));
        }
    }
}
