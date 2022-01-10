using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankServiceApp.ServiceHosts;

namespace BankServiceApp.Replicator
{
    public interface IReplicationService : IDisposable
    {
        void RegisterService(IServiceHost service);

        void UnRegisterService(IServiceHost service);

        void OpenServices();

        void CloseServices();

        IReplicator GetReplicatorProxy();

        ServiceState State { get; }
    }
}
