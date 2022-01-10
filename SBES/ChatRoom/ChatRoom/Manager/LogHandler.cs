using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class LogHandler
    {
        private static LogHandler instance;
        private static object _padLock = new object();

        public static LogHandler Instance
        {
            get
            {
                lock (_padLock)
                {

                    if (instance == null)
                        instance = new LogHandler();

                    return instance;
                }
            }
        }

        static LogHandler() { }

        public void WriteMessage(String text)
        {
            string name = text.Split(',')[0].Split(':')[1];
               
            using (StreamWriter sw = File.AppendText($"{name}.txt"))
            {
                sw.WriteLine($"\n[{DateTime.Now}]: {text}");
            }
        }
    }
}
