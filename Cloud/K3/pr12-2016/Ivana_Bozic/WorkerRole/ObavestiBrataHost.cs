using Common;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole
{
    public class ObavestiBrataHost
    {
        private ServiceHost serviceHost;
        private string internalEndpointName = "InternalRequest";
        public ObavestiBrataHost()
        {
            NetTcpBinding binding = new NetTcpBinding();
            serviceHost = new ServiceHost(typeof(ObavestiBrata));
            RoleInstanceEndpoint inputEndpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints[internalEndpointName];
            string endpoint = String.Format("net.tcp://{0}/{1}", inputEndpoint.IPEndpoint, internalEndpointName);
            serviceHost.AddServiceEndpoint(typeof(IObavesti), binding, endpoint);
        }

        public void Open()
        {
            try
            {
                serviceHost.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void Close()
        {
            try
            {
                serviceHost.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
