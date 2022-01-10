using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileReader.Interfaces;

namespace FileReader.Loggers
{
    public class TxtLogger : ILogger
    {
        public void Log(string dataToLog, string logDirectory)
        {
            if (!String.IsNullOrWhiteSpace(dataToLog) && !String.IsNullOrWhiteSpace(logDirectory))
            {
                string logFilePath = Path.Combine(logDirectory, "Log.txt");
                using (StreamWriter sw = new StreamWriter(logFilePath, true))
                {
                    sw.WriteLine(dataToLog);
                }
            }
        }
    }
}
