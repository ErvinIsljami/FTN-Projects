using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Threading;
using BankServiceApp.Arbitration;
using Common;

namespace BankServiceApp.Replication
{
    public class ReplicatorProxy : IReplicator, IDisposable
    {
        private readonly AutoResetEvent _threadFinishedEvent = new AutoResetEvent(false);

        private readonly HashSet<CommunicationState> invalidFactoryStates = new HashSet<CommunicationState>
        {
            CommunicationState.Closed,
            CommunicationState.Closing,
            CommunicationState.Faulted
        };

        private IArbitrationServiceProvider _arbitrationServiceProvider;
        private bool _disposed;

        private ConcurrentQueue<IReplicationItem> _replicationQueue = new ConcurrentQueue<IReplicationItem>();

        private Thread _replicationThread;
        private CancellationTokenSource _replicationTokenSource;
        private IReplicator _replicatorProxy;
        private ChannelFactory<IReplicator> _replicatorProxyFactory;

        public ReplicatorProxy()
        {
            _arbitrationServiceProvider = ServiceLocator.GetInstance<IArbitrationServiceProvider>();
            if (BankAppConfig.InstanceNo > 1)
            {
                _replicationTokenSource = new CancellationTokenSource();
                _replicationThread = new Thread(ReplicationWorker);
                _replicationThread.Start(_replicationTokenSource.Token);
            }
            else
            {
                Console.WriteLine("Only one instance of bank service is specified replicator won't start.");
            }
        }

        public void Dispose()
        {
            if (!_disposed && BankAppConfig.InstanceNo > 1)
            {
                _disposed = true;

                _replicationTokenSource.Cancel();
                _threadFinishedEvent.WaitOne(10000);

                _replicationTokenSource.Dispose();
                _replicationTokenSource = null;
                _replicationThread = null;

                _replicatorProxyFactory = null;

                while (_replicationQueue.TryDequeue(out var item)) ;
                _replicationQueue = null;
            }

            _arbitrationServiceProvider = null;
        }

        private void ReplicationWorker(object param)
        {
            var cancelationToken = (CancellationToken) param;

            while (!cancelationToken.IsCancellationRequested)
                if (_arbitrationServiceProvider?.State == ServiceState.Hot)
                    try
                    {
                        if (_replicatorProxy != null)
                        {
                            while (!_replicationQueue.IsEmpty)
                            {
                                // Check connection before replicating to keep replication data 
                                // on queue in case backup service fails
                                _replicatorProxy.CheckState();
                                if (_replicationQueue.TryDequeue(out var replicationItem))
                                    _replicatorProxy.ReplicateData(replicationItem);
                            }
                        }
                        else
                        {
                            _replicatorProxy = CreateReplicatorProxy();
                            if (_replicatorProxy?.CheckState() == ServiceState.Standby)
                                Console.WriteLine(
                                    $"Replicator proxy to {_replicatorProxyFactory.Endpoint.Address} opened.");
                            else
                                Thread.Sleep(100);
                        }
                    }
                    catch (SecurityAccessDeniedException secEx)
                    {
                        Console.WriteLine(
                            $"({nameof(BankServiceApp)}) [{nameof(ReplicatorProxy)}] Security exception while trying to replicate: {secEx.Message}");
                        _replicatorProxy = null;

                        // Break on sec exception since replicator probably doesn't have necessary privileges.
                        break;
                    }
                    catch (ObjectDisposedException)
                    {
                        if (_disposed) break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            $"({nameof(BankServiceApp)}) [{nameof(ReplicatorProxy)}] Error: {ex.Message}");
                        _replicatorProxy = null;
                        Thread.Sleep(100);
                    }
                else
                    Thread.Sleep(500);

            _threadFinishedEvent.Set();
        }

        private IReplicator CreateReplicatorProxy()
        {
            if (_replicatorProxyFactory == null || invalidFactoryStates.Contains(_replicatorProxyFactory.State))
            {
                var replicatorEndpoint =
                    $"{BankAppConfig.Endpoints.FirstOrDefault(x => !x.Equals(BankAppConfig.MyEndpoint))}/{BankAppConfig.ReplicatorName}";
                _replicatorProxyFactory = ProxyPool.CreateSecureProxyFactory<IReplicator>(replicatorEndpoint);
            }

            if (invalidFactoryStates.Contains(_replicatorProxyFactory.State)) _replicatorProxyFactory = null;

            return _replicatorProxyFactory?.CreateChannel();
        }

        #region IReplicator Methods

        public void ReplicateData(IReplicationItem replicationData)
        {
            if (BankAppConfig.InstanceNo > 1) _replicationQueue.Enqueue(replicationData);
        }

        public ServiceState CheckState()
        {
            return _replicatorProxy.CheckState();
        }

        #endregion
    }
}