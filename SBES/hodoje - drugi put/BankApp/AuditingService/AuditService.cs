using Common.DataContracts.Dtos;
using Common.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AuditingService
{
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single, InstanceContextMode = InstanceContextMode.PerSession)]
	public class AuditService : IAuditService
	{
		private readonly string _logName = AuditServiceConfig.LogName;

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
				string message = String.Empty;
				if (!String.IsNullOrEmpty(eventLogData.AccountName))
				{
					message += $"{eventLogData.AccountName} : ";
				}

				if (String.IsNullOrEmpty(eventLogData.LogMessage))
				{
					return;
				}

				message += eventLogData.LogMessage;

				if (String.IsNullOrEmpty(message))
				{
					return;
				}

				log.MachineName = Environment.MachineName;
				log.Source = eventLogData.BankName;

				log.WriteEntry($"{eventLogData.AccountName}: {eventLogData.LogMessage}", eventLogData.EventLogType);
			}
		}
	}
}
