using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Bank
{
    public class BankServer
    {
        private static ServiceHost serviceHost;
        private static string bankInternalEndpointName = "BankInternalEndpoint";

        public BankServer()
        {
            Start();
        }

        public void Start()
        {
            Task t = new Task(() =>
            {
                var binding = new NetTcpBinding();
                var internalEndpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints[bankInternalEndpointName].IPEndpoint;
                var endpoint = $"net.tcp://{internalEndpoint}/{bankInternalEndpointName}";
                serviceHost = new ServiceHost(typeof(Bank));
                serviceHost.AddServiceEndpoint(typeof(IBank), binding, endpoint);
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
