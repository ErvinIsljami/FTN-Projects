using Contracts;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole1
{
    public class JobServer
    {
        private ServiceHost serviceHost;

   

        public JobServer()
        {
            string adresa = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["Internal"].IPEndpoint.ToString();
            string endpoint = $"net.tcp://{adresa}/Internal";

            NetTcpBinding binding = new NetTcpBinding();

            serviceHost = new ServiceHost(typeof(JobServerProvider));
            serviceHost.AddServiceEndpoint(typeof(IBrother), binding, endpoint);
        }

        public void Open()
        {
            try
            {
                serviceHost.Open();

                Trace.WriteLine("Konekcija otvorena.");
            }
            catch (Exception e)
            {
                Trace.WriteLine($"Greska prilikom otvaranja konekcije. Greska: {e.Message}");
            }
        }
        public void Close()
        {
            try
            {
                serviceHost.Close();
                Trace.WriteLine("Konekcija zatvorena.");
            }
            catch (Exception e)
            {
                Trace.WriteLine($"Greska prilikom zatvaranja konekcije. Greska: {e.Message}");
            }
        }
    }
}
