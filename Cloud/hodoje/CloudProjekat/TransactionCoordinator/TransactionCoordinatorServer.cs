using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DistributedTransaction
{
    public class TransactionCoordinatorServer
    {
        private static ServiceHost serviceHost;
        private static string address;
        private static TransactionCoordinator transactionCoordinator;

        public TransactionCoordinatorServer(TransactionCoordinator to)
        {
            transactionCoordinator = to;
        }

        public void Start(string transCoordAddress)
        {
            Task t = new Task(() =>
            {
                serviceHost = new ServiceHost(transactionCoordinator);
                var behaviour = serviceHost.Description.Behaviors.Find<ServiceBehaviorAttribute>();
                behaviour.InstanceContextMode = InstanceContextMode.Single;
                behaviour.IncludeExceptionDetailInFaults = true;

                var binding = new NetTcpBinding();
                var endpoint = $"net.tcp://{transCoordAddress}/TransactionCoordinator";
                serviceHost = new ServiceHost(typeof(TransactionCoordinator));
                serviceHost.AddServiceEndpoint(typeof(IPurchase), binding, endpoint);
                serviceHost.Open();
            });
            t.Start();
            Console.WriteLine("Transaction Coordinator Server started.");
        }

        public void Stop()
        {
            serviceHost.Close();
        }
    }
}
