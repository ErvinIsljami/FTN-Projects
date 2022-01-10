using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BankServiceApp.ServiceHosts;
using Common.Transaction;
using Common.UserData;

namespace BankServiceApp.Replicator
{
    public class ReplicationService : IReplicationService
    {
        private static readonly ReaderWriterLockSlim _stateLock = new ReaderWriterLockSlim();
        private readonly List<IServiceHost> _registeredServices = new List<IServiceHost>(10);

        private static IReplicator _replicatorProxy;
        private static ChannelFactory<IReplicator> _replicatorFactory;

        private static ServiceState _state = ServiceState.Standby;
        private static bool ServiceStarted = false;
        private readonly string _myEndpoint = null;
        private readonly ServiceHost _replicatorHost = null;

        public ReplicationService()
        {
            // If static service provider is populated don't run startup procedure
            if (ServiceStarted)
                return;

            var endpoints = BankAppConfig.Endpoints;
            var replicator = BankAppConfig.ReplicatorName;

            var binding = new NetTcpBinding(SecurityMode.Transport);
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = ProtectionLevel.EncryptAndSign;

            List<string> successList = new List<string>(BankAppConfig.InstanceNo);

            foreach (var endpoint in endpoints)
            {
                try
                {
                    var factory = new ChannelFactory<IReplicator>(binding, $"{endpoint}/{replicator}");
                    factory.Credentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;

                    var channel = factory.CreateChannel();
                    if (channel.CheckState() == ServiceState.Hot)
                    {
                        successList.Add(endpoint);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"Failed to establish connection to other service on {endpoint}.");
                    BankAppConfig.MyAddress = _myEndpoint = _myEndpoint ?? endpoint;
                }
            }

            _replicatorHost = new ServiceHost(typeof(ReplicatorService));
            _replicatorHost.AddServiceEndpoint(typeof(IReplicator), binding, $"{_myEndpoint}/{replicator}");
            _replicatorHost.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.UseWindowsGroups;
            _replicatorHost.Authorization.ImpersonateCallerForAllOperations = true;

            _replicatorHost.Open();
            Console.WriteLine($"Replication service open on {_myEndpoint}/{replicator}");

            if (successList.Count == 0)
            {
                State = ServiceState.Hot;
                Console.WriteLine($"No {nameof(BankServiceApp)} is HOT => {nameof(BankServiceApp)}_{Process.GetCurrentProcess().Id} will assert.");
            }

            ServiceStarted = true;
        }

        #region IReplicationService
    
        public void RegisterService(IServiceHost service)
        {
            _registeredServices.Add(service);
        }

        public void UnRegisterService(IServiceHost service)
        {
            _registeredServices.Remove(service);
        }

        public IReplicator GetReplicatorProxy()
        {
            if (_replicatorFactory == null || _replicatorFactory.State != CommunicationState.Opened)
            {
                NetTcpBinding binding = new NetTcpBinding();
                binding.Security.Mode = SecurityMode.Transport;
                binding.Security.Transport.ProtectionLevel = ProtectionLevel.EncryptAndSign;
                binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;

                _replicatorFactory = new ChannelFactory<IReplicator>(binding);
            }

            if (_replicatorProxy == null)
            {
                try
                {
                    _replicatorFactory.CreateChannel();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error opening proxy to replicator => {e.Message}");
                    _replicatorProxy = null;
                }
            }

            return _replicatorProxy;
        }

        public ServiceState State
        {
            get
            {
                ServiceState state;
                _stateLock.EnterReadLock();
                {
                    state = _state;
                }
                _stateLock.ExitReadLock();
                return state;
            }

            private set
            {
                _stateLock.EnterWriteLock();
                {
                    _state = value;
                }
                _stateLock.ExitWriteLock();
            }
        }

        public void OpenServices()
        {
            if (State == ServiceState.Hot)
            {
                foreach (var registeredService in _registeredServices)
                {
                    registeredService.OpenService();
                }
            }
            else
            {
                Console.WriteLine($"Services startup aborted since server is in STANDBY mode.");
            }
        }

        public void CloseServices()
        {
            foreach (var registeredService in _registeredServices)
            {
                registeredService.CloseService();
            }
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            State = ServiceState.Standby;
            CloseServices();
            _registeredServices.Clear();
            _stateLock.Dispose();
            (_replicatorHost as IDisposable).Dispose();
        }
        
        #endregion
    }
}
