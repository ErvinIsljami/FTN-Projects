using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESProject.Components
{
    public sealed class Logger
    {
        //implementirati logger, svaka komponenta ce pozivati ovu metodu i upisvati koja je to komponenta i sta je uradila
        //logger ce upisivati ili na konzoli ili ce upisivati u fajl(videcemo)
        private static string path = @"Logger.log";
        private static readonly object padlock = new object();
        private static Logger instance = null;
        private static StreamWriter sw = null; 
        static Logger()
        {
            sw = File.AppendText(path);
           
        }
        ~Logger()
        {
            sw.Close();
        }
        public static Logger Instance()
        {
            lock(padlock)
            {
                if(instance == null)
                {
                    instance = new Logger();
                }
                return instance;
            }
        }
        public void LogEvent(string componentId, string text)
        {
            if(componentId == null || text == null)
            {
                throw new Exception("Null argument passed.");
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("NEW LOG ENTRY:" + "\r\n");
            sb.Append(string.Format("Time of entry: {0}", DateTime.Now) + "\r\n");
            sb.Append(string.Format("Component: {0}", componentId) + "\r\n");
            sb.Append(string.Format("Message: {0}\r\r\n", text) + "\r\n");
            sw.Write(sb);
            Trace.Write(sb);
        }


    }
}
