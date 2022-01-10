using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace TransactionCoordinator
{
    public class TransactionCoordinatorServer
    {
        private static ServiceHost serviceHost;
        private static string transactionCoordinatorInputEndpointName = "TransactionInputEndpoint";

        public TransactionCoordinatorServer()
        {
            Start();
        }

        public void Start()
        {
            Task t = new Task(() =>
            {
                var binding = new NetTcpBinding();
                var inputEndpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints[transactionCoordinatorInputEndpointName].IPEndpoint;
                var endpoint = $"net.tcp://{inputEndpoint}/{transactionCoordinatorInputEndpointName}";
                serviceHost = new ServiceHost(typeof(TransactionCoordinator));
                serviceHost.AddServiceEndpoint(typeof(IPurchase), binding, endpoint);
                serviceHost.Open();
            });
            t.Start();
        }

        public void Stop()
        {
            serviceHost.Close();
        }
    }
}
