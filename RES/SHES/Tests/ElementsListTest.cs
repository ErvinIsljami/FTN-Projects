using System;
using System.Threading;
using Common;
using Common.Wrappers;
using Common.SHES_Components;
using Common.Wrappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class ElementsListTest
    {
        [TestMethod]
        public void TestConsumeBattery()
        {
            BatteryList batteryList = new BatteryList();
            Battery b = new Battery(100, 100, "test");
            batteryList.List.Add(b);

            var consumption = batteryList.Consume();

            Assert.IsTrue(consumption < 0);
        }

        [TestMethod]
        public void TestGenerateBattery()
        {
            BatteryList batteryList = new BatteryList();
            Battery b = new Battery(100, 100, "test");
            batteryList.List.Add(b);

            var generating = batteryList.Generate();
            //double expected = 0.0166666666666667;

            Assert.IsTrue(generating > 0);
        }

        [TestMethod]
        public void TestConsumers()
        {
            ConsumersList consumersList = new ConsumersList();
            Consumer c = new Consumer(100, "test");
            Consumer c1 = new Consumer(100, "test");
            Consumer c2 = new Consumer(100, "test");
            consumersList.List.Add(c);
            consumersList.List.Add(c1);
            consumersList.List.Add(c2);

            var consumption = consumersList.Consume();

            Assert.IsTrue(consumption == -300);
        }

        [TestMethod]
        public void TestSolarPanels()
        {
            SolarPanelList solarPanelList = new SolarPanelList();
            SolarPanel sp1 = new SolarPanel(100, "sp1");
            SolarPanel sp2 = new SolarPanel(100, "sp2");
            solarPanelList.List.Add(sp1);
            solarPanelList.List.Add(sp2);

            solarPanelList.SunPower = 1;
            var generating = solarPanelList.Generate();

            Assert.IsTrue(generating == 200);
        }

        [TestMethod]
        public void TestSolarPanels50Sun()
        {
            SolarPanelList solarPanelList = new SolarPanelList();
            SolarPanel sp1 = new SolarPanel(100, "sp1");
            SolarPanel sp2 = new SolarPanel(100, "sp2");
            solarPanelList.List.Add(sp1);
            solarPanelList.List.Add(sp2);

            solarPanelList.SunPower = 0.5;
            var generating = solarPanelList.Generate();

            Assert.IsTrue(generating == 100);
        }

        [TestMethod]
        public void TestGlobalClockSpeedUp()
        {
            var before = GlobalClock.Instance.GetCurrentTime();

            Thread.Sleep(1000);
            var after = GlobalClock.Instance.GetCurrentTime();
            var second = GlobalClock.Instance.Second;

            Assert.IsTrue(after.Second - before.Second > 1);
            Assert.IsTrue(second < 1000);   //milliseconds
        }

    }
}
