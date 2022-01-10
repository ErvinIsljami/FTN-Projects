using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    public class BankServer
    {
        private static ServiceHost serviceHost;
        private static string address;
        private static Bank bank;

        public BankServer(Bank b)
        {
            bank = b;
        }

        public void Start(string bankAddress)
        {
            address = bankAddress;
            Task t = new Task(() =>
            {
                serviceHost = new ServiceHost(bank);
                var behaviour = serviceHost.Description.Behaviors.Find<ServiceBehaviorAttribute>();
                behaviour.InstanceContextMode = InstanceContextMode.Single;

                var binding = new NetTcpBinding();
                var endpoint = $"net.tcp://localhost:{bankAddress.Split(':')[1]}/Bank";
                serviceHost = new ServiceHost(typeof(Bank));
                serviceHost.AddServiceEndpoint(typeof(IBank), binding, endpoint);
                serviceHost.Open();
            });
            t.Start();
            Console.WriteLine("Bank server started.");
        }

        public void Stop()
        {
            serviceHost.Close();
        }
    }
}
