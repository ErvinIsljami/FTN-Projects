using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common.Transaction;
using Common.UserData;

namespace BankServiceApp.Replicator
{
    public class ReplicatorService : IReplicator
    {
        private static readonly ReplicationService _replicationServiceProvider = new ReplicationService();

        #region IReplicator

        [OperationBehavior(Impersonation = ImpersonationOption.Required)]
        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = "Replicator")]
        public void ReplicateTransaction(ITransaction transaction, string clientName)
        {
            throw new NotImplementedException();
        }

        [OperationBehavior(Impersonation = ImpersonationOption.Required)]
        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = "Replicator")]
        public void ReplicateClientData(IClient clientData)
        {
            throw new NotImplementedException();
        }

        [OperationBehavior(Impersonation = ImpersonationOption.Required)]
        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = "Replicator")]
        public ServiceState CheckState()
        {
            return _replicationServiceProvider.State;
        }

        #endregion
    }
}
