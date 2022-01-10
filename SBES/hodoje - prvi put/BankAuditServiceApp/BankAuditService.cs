using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Threading;
using Common.EventLogData;
using Common.ServiceContracts;

namespace BankAuditServiceApp
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single)]
    public class BankAuditService : IBankAuditService
    {
        private readonly string _logName = BankAuditServiceConfig.LogName;

        public void Log(EventLogData eventLogData)
        {
            if (!EventLog.SourceExists(eventLogData.BankName))
            {
                EventLog.CreateEventSource(eventLogData.BankName, _logName);

                // Giving OS the time to register the source
                Thread.Sleep(50);
            }

            using (var log = new EventLog(_logName))
            {
                log.MachineName = Environment.MachineName;
                log.Source = eventLogData.BankName;
                log.WriteEntry($"{eventLogData.AccountName}: {eventLogData.LogMessage}", eventLogData.EventLogType);
            }
        }
    }
}