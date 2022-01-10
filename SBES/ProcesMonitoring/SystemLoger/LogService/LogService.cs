using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SystemLoger.Auditing;

namespace SystemLoger.LogService
{
    public class LogService : ILogService
    {
        private string _logFileName = "proccessLoger.log";
        public void LogEvent(string processName, ECriticalLvl criticalLvl, DateTime time)
        {
            using (StreamWriter sw = new StreamWriter(_logFileName, true))
            {
                string message = $"[{criticalLvl.ToString()}]\t\t[{time.ToString("HH:mm:ss dd.mm.yyyy")}] - {processName}.";
                sw.WriteLine(message);
                Console.WriteLine(message);
                if(criticalLvl == ECriticalLvl.CRITICAL)
                    Audit.AuditEvent(message);
    
            }
        }
    }
}