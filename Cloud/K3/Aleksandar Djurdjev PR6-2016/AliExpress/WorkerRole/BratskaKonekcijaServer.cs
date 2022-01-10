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
    public class BratskaKonekcijaServer
    {
        private ServiceHost serviceHost;
        private string endpointName = "Internal";
        public BratskaKonekcijaServer()
        {
            RoleInstanceEndpoint internalEndpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints[endpointName];
            string endpoint = String.Format("net.tcp://{0}/{1}", internalEndpoint.IPEndpoint, endpointName);
            serviceHost = new ServiceHost(typeof(BratskaKonekcijaProvider));
            serviceHost.AddServiceEndpoint(typeof(IBratskaKonekcija), new NetTcpBinding(), endpoint);
        }
        public void Open()
        {
            serviceHost.Open();
        }
        public void Close()
        {
            serviceHost.Close();
        }
    }
}
