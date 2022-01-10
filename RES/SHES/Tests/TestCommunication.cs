using Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Communication;
using Common.Wrappers;
using Common.SHES_Components;
using System.Threading;

namespace Tests
{
    [TestClass]
    public class TestCommunication
    {
        [TestMethod]
        public void TestGetDiff()
        {
            SHES shes = new SHES(new SHESToComponentsQueueMock());
            double diff = shes.GetDiff();

            Assert.IsTrue(diff == 200);  //za svaki queue po jedan zahtev od 100
        }

        [TestMethod]
        public void TestBatteryThreadConsuming()
        {
            ISHESToComponentsQueues queues = new SHESToComponentsQueueMock();
            BatteryList batteryList = new BatteryList(ref queues);

            Thread.Sleep(100);
            Assert.IsTrue(batteryList.IsConsumig);
        }

        [TestMethod]
        public void TestBatteryThreadGenerating()
        {
            ISHESToComponentsQueues queues = new SHESToComponentsQueueMock();
            Response response;
            queues.BatteryResponses.TryDequeue(out response);
            queues.BatteryResponses.Enqueue(new Response(0, "test"));

            BatteryList batteryList = new BatteryList(ref queues);

            Assert.IsFalse(batteryList.IsConsumig);
        }

        [TestMethod]
        public void TestConsumerThread()
        {
            ISHESToComponentsQueues queues = new SHESToComponentsQueueMock();
            ConsumersList consumersList = new ConsumersList(ref queues);

            Thread.Sleep(100);
            Request r;
            queues.ConsumersRequest.TryDequeue(out r);

            Assert.IsNotNull(r);
        }

        [TestMethod]
        public void TestSolarPanelThread()
        {
            ISHESToComponentsQueues queues = new SHESToComponentsQueueMock();
            SolarPanelList solarPanelList = new SolarPanelList(ref queues);

            Thread.Sleep(100);

            Assert.IsTrue(solarPanelList.SunPower == 100);
        }
    }
}