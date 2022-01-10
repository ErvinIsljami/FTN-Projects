using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Bookstore
{
    public class BookstoreServer
    {
        private static ServiceHost serviceHost;
        private static string bookstoreInternalEndpointName = "BookstoreInternalEndpoint";

        public BookstoreServer()
        {
            Start();
        }

        public void Start()
        {
            Task t = new Task(() =>
            {
                var binding = new NetTcpBinding();
                var internalEndpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints[bookstoreInternalEndpointName].IPEndpoint;
                var endpoint = $"net.tcp://{internalEndpoint}/{bookstoreInternalEndpointName}";
                serviceHost = new ServiceHost(typeof(Bookstore));
                serviceHost.AddServiceEndpoint(typeof(IBookstore), binding, endpoint);
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
