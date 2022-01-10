using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using BankServiceApp.ServiceHosts;
using Common.Transaction;
using Common.UserData;

namespace BankServiceApp.Replicator
{
    public enum ServiceState
    {
        Hot,
        Standby
    }

    [ServiceContract(ProtectionLevel = ProtectionLevel.EncryptAndSign)]
    public interface IReplicator
    {
        [OperationContract]
        void ReplicateTransaction(ITransaction transaction, string clientName);

        [OperationContract]
        void ReplicateClientData(IClient clientData);

        [OperationContract]
        ServiceState CheckState();
    }
}
