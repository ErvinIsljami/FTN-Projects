using System.Security.Permissions;
using System.ServiceModel;

namespace Common.ServiceContracts
{
    [ServiceContract]
    public interface IBankAuditService
    {
        /// <summary>
        ///     Logs specific event data to a dedicated Windows event log
        /// </summary>
        /// <param name="eventLogData">Object that holds required event log data.</param>
        [OperationContract]
        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = "BankServices")]
        void Log(EventLogData.EventLogData eventLogData);
    }
}