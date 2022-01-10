using Common.DataContracts.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataContracts.Dtos
{
	/// <summary>
	///     Represents an item for EventLog
	/// </summary>
	[DataContract]
	public class EventLogData
	{
		public EventLogData()
		{
		}

		public EventLogData(string bankName, string accountName, string logMessage, EventLogEntryType eventLogEntryType)
		{
			if (string.IsNullOrWhiteSpace(bankName))
			{
				throw new ArgumentNullException(nameof(bankName));
			}

			if (string.IsNullOrWhiteSpace(logMessage))
			{
				throw new ArgumentNullException(nameof(logMessage));
			}

			if (!Enum.IsDefined(typeof(EventLogEntryType), eventLogEntryType))
			{
				throw new InvalidEventLogTypeException();
			}

			BankName = bankName;
			AccountName = accountName;
			LogMessage = logMessage;
			EventLogType = eventLogEntryType;
		}

		[DataMember]
		public string BankName { get; set; }

		[DataMember]
		public string AccountName { get; set; }

		[DataMember]
		public string LogMessage { get; set; }

		[DataMember]
		public EventLogEntryType EventLogType { get; set; }
	}
}
