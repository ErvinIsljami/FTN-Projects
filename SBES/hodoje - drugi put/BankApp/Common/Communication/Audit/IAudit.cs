using System.Diagnostics;

namespace Common.Communication
{ 
	public interface IAudit
	{
		void Log(string accountName = "", string logMessage = "", EventLogEntryType eventLogEntryType = EventLogEntryType.Information);
	}
}
