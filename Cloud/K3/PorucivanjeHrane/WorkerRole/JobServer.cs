using Data;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole
{
    public class JobServer
    {
        ServiceHost host;
        string endpointName = "InternalRequest";

        public JobServer()
        {
            NetTcpBinding binding = new NetTcpBinding();

            host = new ServiceHost(typeof(JobServiceProvider));
            RoleInstanceEndpoint endpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints[endpointName];
            String address = $"net.tcp://{endpoint.IPEndpoint}/{endpointName}";
            host.AddServiceEndpoint(typeof(INotify), binding, address);
        }

        public void Open()
        {
            try
            {
                host.Open();
                Trace.WriteLine("JobServer opened.");
            }
            catch (Exception e)
            {
                Trace.WriteLine("Job server failed to open");
            }
        }

        public void Close()
        {
            try
            {
                host.Close();
                Trace.WriteLine("JobServer closed.");
            }
            catch (Exception e)
            {
                Trace.WriteLine("Job server failed to close");
            }
        }
    }
}
