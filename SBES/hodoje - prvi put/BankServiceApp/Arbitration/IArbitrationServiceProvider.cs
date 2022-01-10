using System;
using BankServiceApp.ServiceHosts;
using Common;

namespace BankServiceApp.Arbitration
{
    public interface IArbitrationServiceProvider : IDisposable
    {
        ServiceState State { get; }
        void RegisterService(IServiceHost service);

        void UnRegisterService(IServiceHost service);

        void OpenServices();

        void CloseServices();
    }
}