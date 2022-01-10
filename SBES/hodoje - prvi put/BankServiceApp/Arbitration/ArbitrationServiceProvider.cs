using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Security;
using System.ServiceModel;
using System.Threading;
using BankServiceApp.Replication;
using BankServiceApp.ServiceHosts;
using Common;

namespace BankServiceApp.Arbitration
{
    public class ArbitrationServiceProvider : IArbitrationServiceProvider
    {
        private static ServiceState _state = ServiceState.Standby;

        private readonly Thread _stateCheckerThread;
        private readonly AutoResetEvent _stateCheckerThreadFinishedEvent;
        private readonly CancellationTokenSource _stateCheckerTokenSource;

        private bool _disposed;
        private List<IServiceHost> _registeredServices = new List<IServiceHost>(10);

        private ServiceHost _replicatorHost;
        private ReaderWriterLockSlim _stateLock = new ReaderWriterLockSlim();

        public ArbitrationServiceProvider()
        {
            if (BankAppConfig.InstanceNo < 1 || BankAppConfig.InstanceNo > 2)
                throw new ArbitrationException($"Invalid instance number. Range [1,2], is {BankAppConfig.InstanceNo}");

            if (BankAppConfig.InstanceNo > 1)
            {
                var activeInstanceCount = GetActiveInstanceCount();

                if (activeInstanceCount == 0)
                {
                    State = ServiceState.Hot;
                    Console.WriteLine(
                        $"No {nameof(BankServiceApp)} is HOT => {nameof(BankServiceApp)}_{Process.GetCurrentProcess().Id} will assert.");
                }

                OpenReplicationService();
            }
            else
            {
                BankAppConfig.MyEndpoint = BankAppConfig.Endpoints[0];
                State = ServiceState.Hot;
            }

            if (BankAppConfig.InstanceNo > 1)
            {
                _stateCheckerTokenSource = new CancellationTokenSource();
                _stateCheckerThreadFinishedEvent = new AutoResetEvent(false);
                _stateCheckerThread = new Thread(StateCheckerWorker);
                _stateCheckerThread.Start(_stateCheckerTokenSource.Token);
            }
        }

        #region IDisposable

        public void Dispose()
        {
            try
            {
                if (!_disposed)
                {
                    _disposed = true;
                    if (BankAppConfig.InstanceNo > 1)
                    {
                        _stateCheckerTokenSource.Cancel();
                        _stateCheckerThreadFinishedEvent.WaitOne(10000);
                    }

                    State = ServiceState.Standby;
                    CloseServices();
                    _registeredServices.Clear();
                    _registeredServices = null;

                    _stateLock.Dispose();
                    _stateLock = null;

                    (_replicatorHost as IDisposable).Dispose();
                    _replicatorHost = null;
                }
            }
            catch
            {
                if (_disposed)
                    Console.WriteLine("(BankServiceApp) [ReplicationService] Dispose called on disposing object.");
            }
        }

        #endregion

        private void StateCheckerWorker(object param)
        {
            var cancellationToken = (CancellationToken) param;
            var success = false;

            while (!cancellationToken.IsCancellationRequested)
                if (State != ServiceState.Hot)
                {
                    foreach (var endpoint in BankAppConfig.Endpoints.Where(x => !x.Equals(BankAppConfig.MyEndpoint)))
                    {
                        var factory =
                            ProxyPool.CreateSecureProxyFactory<IReplicator>(
                                $"{endpoint}/{BankAppConfig.ReplicatorName}");
                        try
                        {
                            var proxy = factory.CreateChannel();
                            if (proxy.CheckState() == ServiceState.Hot) success = true;
                        }
#if DEBUG
                        catch (Exception ex)
                        {
                            Console.WriteLine(
                                $"({nameof(BankServiceApp)}) [{nameof(ArbitrationServiceProvider)}] Failed to establish connection to replication service at {endpoint}. Reason: {ex.Message}");
                        }
#else
                        catch
                        {
                            Console.WriteLine(
                                $"({nameof(BankServiceApp)}) [{nameof(ArbitrationServiceProvider)}] Failed to establish connection to replication service at {endpoint}.");
                        }
#endif
                    }

                    if (!success)
                    {
                        Console.WriteLine($"Switching state from {State} to {ServiceState.Hot}");
                        State = ServiceState.Hot;
                        OpenServices();
                    }

                    success = false;
                    Thread.Sleep(500);
                }
                else
                {
                    Thread.Sleep(1000);
                }

            _stateCheckerThreadFinishedEvent.Set();
        }

        private void OpenReplicationService()
        {
            var binding = new NetTcpBinding(SecurityMode.Transport);
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = ProtectionLevel.EncryptAndSign;
            binding.OpenTimeout = binding.CloseTimeout = TimeSpan.FromSeconds(2);


            _replicatorHost = new ServiceHost(typeof(ReplicatorService));
            _replicatorHost.AddServiceEndpoint(typeof(IReplicator), binding,
                $"{BankAppConfig.MyEndpoint}/{BankAppConfig.ReplicatorName}");

            _replicatorHost.Open();
            Console.WriteLine($"Replication service open on {BankAppConfig.MyEndpoint}/{BankAppConfig.ReplicatorName}");
        }

        private int GetActiveInstanceCount()
        {
            var successList = new List<string>(BankAppConfig.InstanceNo);
            foreach (var endpoint in BankAppConfig.Endpoints)
                try
                {
                    var replicatorEndpoint = $"{endpoint}/{BankAppConfig.ReplicatorName}";

                    var factory = ProxyPool.CreateSecureProxyFactory<IReplicator>(replicatorEndpoint);
                    var channel = factory.CreateChannel();

                    if (channel.CheckState() == ServiceState.Hot) successList.Add(endpoint);
                }
                catch (Exception)
                {
                    Console.WriteLine($"Failed to establish connection to other service on {endpoint}.");
                    BankAppConfig.MyEndpoint = BankAppConfig.MyEndpoint ?? endpoint;
                }

            return successList.Count;
        }

        #region IArbitrationServiceProvider Methods

        public void RegisterService(IServiceHost service)
        {
            _registeredServices.Add(service);
        }

        public void UnRegisterService(IServiceHost service)
        {
            _registeredServices.Remove(service);
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
                foreach (var registeredService in _registeredServices)
                    registeredService.OpenService();
            else
                Console.WriteLine("Services startup aborted since server is in STANDBY mode.");
        }

        public void CloseServices()
        {
            foreach (var registeredService in _registeredServices) registeredService.CloseService();
        }

        #endregion
    }
}