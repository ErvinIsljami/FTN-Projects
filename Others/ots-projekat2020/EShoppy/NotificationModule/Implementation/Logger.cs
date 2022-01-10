using EShoppy.NotificationModule.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppy.NotificationModule.Implementation
{
    public class Logger : ILogger
    {
        public void LogAction(string message)
        {
            using (StreamWriter sw = File.AppendText("logFile.txt"))
            {
                sw.WriteLine($"[{DateTime.Now}] - {message}");
            }
        }
    }
}
