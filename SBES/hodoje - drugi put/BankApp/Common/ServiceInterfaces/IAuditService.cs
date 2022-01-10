using Common.DataContracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.ServiceInterfaces
{
	[ServiceContract]
	public interface IAuditService
	{
		/// <summary>
		///     Logs specific event data to a dedicated Windows event log
		/// </summary>
		/// <param name="eventLogData">Object that holds required event log data.</param>
		[OperationContract]
		[PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = "BankServices")]
		void Log(EventLogData eventLogData);
	}
}
