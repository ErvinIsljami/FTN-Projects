using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RESProject.Components;
using RESProject.Classes;

namespace ProjectTest
{
    [TestClass]
    public class HistoricalTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Historical historical = new Historical();
            Assert.AreEqual(true, historical.ManualWriting(Codes.CODE_ANALOG, 432));
            Assert.AreEqual(true, historical.ManualWriting(Codes.CODE_DIGITAL, 432));
            Assert.AreEqual(true, historical.ManualWriting(Codes.CODE_CONSUMER, 432));
            Assert.AreEqual(true, historical.ManualWriting(Codes.CODE_CUSTOM, 432));
            Assert.AreEqual(true, historical.ManualWriting(Codes.CODE_LIMITSET, 432));
            Assert.AreEqual(true, historical.ManualWriting(Codes.CODE_MOTION, 432));
            Assert.AreEqual(true, historical.ManualWriting(Codes.CODE_MULTIPLENODE, 432));
            Assert.AreEqual(true, historical.ManualWriting(Codes.CODE_SENSOR, 432));
            Assert.AreEqual(true, historical.ManualWriting(Codes.CODE_SINGLENODE, 432));
            Assert.AreEqual(true, historical.ManualWriting(Codes.CODE_SOURCE, 432));
            Assert.AreEqual(false, historical.ManualWriting((Codes)13, 432));
            Assert.AreEqual(false, historical.ManualWriting((Codes)133, 432));
            Assert.AreEqual(false, historical.ManualWriting((Codes)131, 432));
            Assert.AreEqual(false, historical.ManualWriting((Codes)(-1), 432));
            Assert.AreEqual(false, historical.ManualWriting((Codes)(-5), 432));
            Assert.AreEqual(false, historical.ManualWriting((Codes)(-17), 432));
            Assert.AreEqual(false, historical.ManualWriting((Codes)(-14), 432));

        }

        [TestMethod]
        public void TestMethod2()
        {
            Historical historical = new Historical();
            Assert.AreEqual(false, historical.WriteToXML(null));
            Assert.AreEqual(true, historical.WriteToXML(new DeltaCD()));
        }
        [TestMethod]
        public void TestMethod3()
        {
            Historical historical = new Historical();
            Assert.AreEqual(false, historical.CreateXmlDocument(1, "asgsfsdf"));
            try
            {
                historical.CreateXmlDocument(1, "DataSet1.xml");
            }
            catch
            {
                Assert.Fail("oisajoigfasj");
            }
            Assert.AreEqual(false, historical.CreateXmlDocument(6, "DataSet1.xml"));
            Assert.AreEqual(false, historical.CreateXmlDocument(-1, "DataSet387.xml"));

            Assert.AreEqual(true, historical.CreateXmlDocument(1, "dataSet37.xml"));
        }

        [TestMethod]
        public void TestMethod4()
        {
            Historical historical = new Historical();
            Assert.AreEqual(false, historical.IsOutOfDeadband(null));
            Assert.AreEqual(true, historical.IsOutOfDeadband(new DumpingProperty()));
            DumpingProperty dp = new DumpingProperty();
            dp.Code = Codes.CODE_DIGITAL;
            Assert.AreEqual(true, historical.IsOutOfDeadband(dp));
        }

        [TestMethod]
        public void TestMethod5()
        {
            Historical historical = new Historical();
            Assert.AreEqual(false, historical.AutomaticAdd(null));
            CollectionDescription cd = new CollectionDescription(123, 1);
            cd.DumpingPropertyCollection.Add(new DumpingProperty(0, 311));
            cd.DumpingPropertyCollection.Add(new DumpingProperty(5, 111));
            Assert.AreEqual(true, historical.AutomaticAdd(cd));

            CollectionDescription cd2 = new CollectionDescription(1231, 2);
            cd2.DumpingPropertyCollection.Add(new DumpingProperty(1, 311));
            cd2.DumpingPropertyCollection.Add(new DumpingProperty(6, 111));
            Assert.AreEqual(true, historical.AutomaticAdd(cd2));

            CollectionDescription cd3 = new CollectionDescription(1232, 3);
            cd3.DumpingPropertyCollection.Add(new DumpingProperty(2, 311));
            cd3.DumpingPropertyCollection.Add(new DumpingProperty(7, 111));
            Assert.AreEqual(true, historical.AutomaticAdd(cd3));

            CollectionDescription cd4 = new CollectionDescription(1233, 4);
            cd4.DumpingPropertyCollection.Add(new DumpingProperty(3, 311));
            cd4.DumpingPropertyCollection.Add(new DumpingProperty(8, 111));
            Assert.AreEqual(true, historical.AutomaticAdd(cd4));

            CollectionDescription cd5 = new CollectionDescription(1234, 5);
            cd5.DumpingPropertyCollection.Add(new DumpingProperty(4, 311));
            cd5.DumpingPropertyCollection.Add(new DumpingProperty(9, 111));
            Assert.AreEqual(true, historical.AutomaticAdd(cd5));



            cd.DumpingPropertyCollection.Add(new DumpingProperty(5, 113));
            Assert.AreEqual(false, historical.AutomaticAdd(cd));
            
        }
        [TestMethod]
        public void TestMethod6()
        {
            Historical historical = new Historical();
            Assert.AreEqual(false, historical.AutomaticUpdate(null));
            CollectionDescription cd = new CollectionDescription(123, 1);
            cd.DumpingPropertyCollection.Add(new DumpingProperty(0, 311));
            cd.DumpingPropertyCollection.Add(new DumpingProperty(5, 111));
            Assert.AreEqual(true, historical.AutomaticUpdate(cd));

            CollectionDescription cd2 = new CollectionDescription(1231, 2);
            cd2.DumpingPropertyCollection.Add(new DumpingProperty(1, 311));
            cd2.DumpingPropertyCollection.Add(new DumpingProperty(6, 111));
            Assert.AreEqual(true, historical.AutomaticUpdate(cd2));

            CollectionDescription cd3 = new CollectionDescription(1232, 3);
            cd3.DumpingPropertyCollection.Add(new DumpingProperty(2, 311));
            cd3.DumpingPropertyCollection.Add(new DumpingProperty(7, 111));
            Assert.AreEqual(true, historical.AutomaticUpdate(cd3));

            CollectionDescription cd4 = new CollectionDescription(1233, 4);
            cd4.DumpingPropertyCollection.Add(new DumpingProperty(3, 311));
            cd4.DumpingPropertyCollection.Add(new DumpingProperty(8, 111));
            Assert.AreEqual(true, historical.AutomaticUpdate(cd4));

            CollectionDescription cd5 = new CollectionDescription(1234, 5);
            cd5.DumpingPropertyCollection.Add(new DumpingProperty(4, 311));
            cd5.DumpingPropertyCollection.Add(new DumpingProperty(9, 111));
            Assert.AreEqual(true, historical.AutomaticUpdate(cd5));



            cd.DumpingPropertyCollection.Add(new DumpingProperty(5, 113));
            Assert.AreEqual(false, historical.AutomaticUpdate(cd));

        }
        [TestMethod]
        public void TestMethod7()
        {
            Historical historical = new Historical();
            Assert.AreEqual(false, historical.AutomaticDelete(null));

            CollectionDescription cd1 = new CollectionDescription(123, 1);
            cd1.DumpingPropertyCollection.Add(new DumpingProperty(0, 311));
            cd1.DumpingPropertyCollection.Add(new DumpingProperty(5, 111));
            Assert.AreEqual(true, historical.AutomaticDelete(cd1));

            CollectionDescription cd2 = new CollectionDescription(1231, 2);
            cd2.DumpingPropertyCollection.Add(new DumpingProperty(1, 311));
            cd2.DumpingPropertyCollection.Add(new DumpingProperty(6, 111));
            Assert.AreEqual(true, historical.AutomaticAdd(cd2));

            CollectionDescription cd3 = new CollectionDescription(1232, 3);
            cd3.DumpingPropertyCollection.Add(new DumpingProperty(2, 311));
            cd3.DumpingPropertyCollection.Add(new DumpingProperty(7, 111));
            Assert.AreEqual(true, historical.AutomaticAdd(cd3));

            CollectionDescription cd4 = new CollectionDescription(1233, 4);
            cd4.DumpingPropertyCollection.Add(new DumpingProperty(3, 311));
            cd4.DumpingPropertyCollection.Add(new DumpingProperty(8, 111));
            Assert.AreEqual(true, historical.AutomaticAdd(cd4));

            CollectionDescription cd5 = new CollectionDescription(1234, 5);
            cd5.DumpingPropertyCollection.Add(new DumpingProperty(4, 311));
            cd5.DumpingPropertyCollection.Add(new DumpingProperty(9, 111));
            Assert.AreEqual(true, historical.AutomaticAdd(cd5));


            cd1.DumpingPropertyCollection.Add(new DumpingProperty(5, 113));
            Assert.AreEqual(false, historical.AutomaticDelete(cd1));

        }
        [TestMethod]
        public void TestMethod8()
        {
            Historical historical = new Historical();
            Assert.AreEqual(false, historical.CheckIfUpdatable((Codes)13, 313123));
            Assert.AreEqual(true, historical.CheckIfUpdatable((Codes)1, 313123));
            Assert.AreEqual(true, historical.CheckIfUpdatable((Codes)2, 313123));
            Assert.AreEqual(true, historical.CheckIfUpdatable((Codes)3, 313123));
            Assert.AreEqual(true, historical.CheckIfUpdatable((Codes)4, 313123));
            Assert.AreEqual(true, historical.CheckIfUpdatable((Codes)5, 313123));
            Assert.AreEqual(true, historical.CheckIfUpdatable((Codes)6, 313123));
            Assert.AreEqual(true, historical.CheckIfUpdatable((Codes)7, 313123));
            Assert.AreEqual(true, historical.CheckIfUpdatable((Codes)8, 313123));
            Assert.AreEqual(true, historical.CheckIfUpdatable((Codes)9, 313123));
            

        }

    }
}
