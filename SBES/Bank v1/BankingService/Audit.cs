using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingService
{
    public class Audit
    {
        private static EventLog customLog = null;
        const string SourceName = "Application";
        const string LogName = "";

        public Audit()
        {
            try
            {
                if (!EventLog.SourceExists(SourceName))
                {
                    EventLog.CreateEventSource(SourceName, LogName);
                }

                customLog = new EventLog(LogName, Environment.MachineName, SourceName);
            }
            catch (Exception e)
            {
                customLog = null;
                Console.WriteLine("Error while trying to create log handle. Error = {0}", e.Message);
            }
        }

        public void AuditEvent(string message)
        {
            customLog.WriteEntry(message, EventLogEntryType.Information);
        }

        ~Audit()
        {
            customLog.Dispose();
        }

    }
}
