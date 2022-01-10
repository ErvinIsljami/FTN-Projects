using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Models;
using FileReader.Readers;
using FileReader.ReadModel;
using NUnit.Framework;
using Moq;
using System.Xml.Serialization;

namespace PowerConsumptionUnitTests.PowerConsumptionUnitTests.FileReader
{
    public class XmlReaderTest
    {
        private string logDirectory;
        private XmlReader xmlReader;

        [SetUp]
        public void SetupTest()
        {
            // Until future fix, change this absolute path to whatever you like.
            logDirectory = @"C:\Users\Nikola Karaklic\Desktop\Readings";
            xmlReader = new Mock<XmlReader>().Object;
        }

        [Test]
        public void ReadGoodXmlData()
        {
            string fileName = Path.Combine(logDirectory, "prog_2018_05_07 - kim.xml");
            string errorMessage = "";
            List<PowerConsumptionData> listOfdata = xmlReader.Read(fileName, out errorMessage);

            XmlSerializer xs = new XmlSerializer(typeof(List<ReadDataType>), new XmlRootAttribute("PROGNOZIRANI_LOAD"));
            List<ReadDataType> duplicateData = new List<ReadDataType>();
            List<PowerConsumptionData> convertedData = new List<PowerConsumptionData>();
            using (StreamReader sr = new StreamReader(fileName))
            {
                duplicateData = xs.Deserialize(sr) as List<ReadDataType>;
            }

            int hour = 0;
            double consumption = 0;
            foreach (ReadDataType data in duplicateData)
            {
                Int32.TryParse(data.Hour, out hour);
                Double.TryParse(data.Load, out consumption);

                if (hour == 24)
                {
                    hour = 0;
                }
                // Long line because of number of lines covered
                convertedData.Add(new PowerConsumptionData
                {
                    Timestamp = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, 0, 0),
                    Consumption = consumption,
                    GeoAreaId = data.GeoAreaId
                });
            }

            bool equal = true;
            Assert.IsNotEmpty(convertedData);
            Assert.IsNotEmpty(listOfdata);
            foreach (PowerConsumptionData consumptionData in convertedData)
            {
                if (listOfdata.FirstOrDefault(x => x.Timestamp == consumptionData.Timestamp && 
                                                   x.GeoAreaId == consumptionData.GeoAreaId && 
                                                   x.Consumption == consumptionData.Consumption) == null)
                {
                    equal = false;
                    break;
                }
            }
            Assert.IsTrue(equal);
        }

        [Test]
        public void ReadDataWithSameHour()
        {
            string fileName = Path.Combine(logDirectory, "prog_2018_05_07 - kim - same hour.xml");
            string errorMessage = "";
            List<PowerConsumptionData> listOfdata = xmlReader.Read(fileName, out errorMessage);

            Assert.AreEqual($"Time: {DateTime.Now}, Message: 'There aren't 24 distinct values.'", errorMessage);
        }

        [Test]
        public void ReadDataWithMissingHour()
        {
            string fileName = Path.Combine(logDirectory, "prog_2018_05_07 - kim - missing hour.xml");
            string errorMessage = "";
            List<PowerConsumptionData> listOfdata = xmlReader.Read(fileName, out errorMessage);

            Assert.AreEqual($"Time: {DateTime.Now}, Message: 'There aren't 24 values.'", errorMessage);
        }

        [Test]
        public void ReadDataWithMissingHourFromOneToTwentyFour()
        {
            string fileName = Path.Combine(logDirectory, "prog_2018_05_07 - kim - missing hour from 1-24.xml");
            string errorMessage = "";
            List<PowerConsumptionData> listOfdata = xmlReader.Read(fileName, out errorMessage);

            Assert.IsTrue(errorMessage.Contains($"Time: {DateTime.Now}, "));
            Assert.IsTrue(errorMessage.Contains("Message: 'Data for hour"));
            Assert.IsTrue(errorMessage.Contains("is missing.'"));
        }

        [Test]
        public void ReadDataWithNoRoot()
        {
            string fileName = Path.Combine(logDirectory, "prog_2018_05_07 - kim - no root.xml");
            string errorMessage = "";
            List<PowerConsumptionData> listOfdata = xmlReader.Read(fileName, out errorMessage);

            Assert.AreEqual($"Time: {DateTime.Now}, Message: 'Either missing proper root or some other error.'", errorMessage);
        }
    }
}
