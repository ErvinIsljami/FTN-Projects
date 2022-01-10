using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileReader;
using FileReader.Loggers;
using Moq;
using NUnit.Framework;

namespace PowerConsumptionUnitTests.PowerConsumptionUnitTests.FileReader
{
    [TestFixture]
    public class TxtLoggerTest
    {
        private string dataToLog;
        private string logDirectory;
        private TxtLogger txtLogger;

        [SetUp]
        public void SetupTest()
        {
            SetupMethod();
        }

        [Test]
        public void SetupMethod()
        {
            dataToLog = "Data to log.";
            // Until future fix, change this absolute path to whatever you like.
            logDirectory = @"C:\Users\Nikola Karaklic\Desktop\Log";
            txtLogger = new Mock<TxtLogger>().Object;
        }

        // All tests pass.
        // They are commented because of the nature of the logger that appends text in a file.
        // Before running each tests, clean the Log.txt file that you are writing to.

        //[Test]
        //public void LogTestGoodParameters()
        //{
        //    // Logger appends data to end 
        //    // so we will pick the first line and see if it matches the data to be written
        //    txtLogger.Log(dataToLog, logDirectory);
        //    string logFile = Path.Combine(logDirectory, "Log.txt");
        //    string[] resultLines = File.ReadAllLines(logFile);
        //    Assert.AreEqual(dataToLog, resultLines[0]);
        //}

        //[Test]
        //public void LogTestDataToLogIsEmpty()
        //{
        //    txtLogger.Log("", logDirectory);
        //    string logFile = Path.Combine(logDirectory, "Log.txt");
        //    string[] resultLines = File.ReadAllLines(logFile);
        //    string result = resultLines.Length <= 0 ? "" : resultLines[0];
        //    Assert.AreNotEqual(dataToLog, result);
        //}

        //[Test]
        //public void LogTestLogDirectoryIsEmpty()
        //{
        //    txtLogger.Log(dataToLog, "");
        //    string logFile = Path.Combine(logDirectory, "Log.txt");
        //    string[] resultLines = File.ReadAllLines(logFile);
        //    string result = resultLines.Length <= 0 ? "" : resultLines[0];
        //    Assert.AreNotEqual(dataToLog, result);
        //}

        //[Test]
        //public void LogTestDataToLogIsNull()
        //{
        //    txtLogger.Log(null, logDirectory);
        //    string logFile = Path.Combine(logDirectory, "Log.txt");
        //    string[] resultLines = File.ReadAllLines(logFile);
        //    string result = resultLines.Length <= 0 ? "" : resultLines[0];
        //    Assert.AreEqual(String.Empty, result);
        //}

        //[Test]
        //public void LogTestLogDirectoryIsNull()
        //{
        //    txtLogger.Log(dataToLog, null);
        //    string logFile = Path.Combine(logDirectory, "Log.txt");
        //    string[] resultLines = File.ReadAllLines(logFile);
        //    string result = resultLines.Length <= 0 ? "" : resultLines[0];
        //    Assert.AreEqual(String.Empty, result);
        //}
    }
}
