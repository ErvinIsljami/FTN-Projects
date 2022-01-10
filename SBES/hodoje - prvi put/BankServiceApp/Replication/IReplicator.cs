using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using Common;
using Common.UserData;

namespace BankServiceApp.Replication
{
    [ServiceContract]
    [ServiceKnownType(typeof(ReplicationItem))]
    [ServiceKnownType(typeof(Client))]
    [ServiceKnownType(typeof(Account))]
    [ServiceKnownType(typeof(X509Certificate2))]
    public interface IReplicator
    {
        [OperationContract]
        void ReplicateData(IReplicationItem replicationData);

        [OperationContract]
        ServiceState CheckState();
    }
}