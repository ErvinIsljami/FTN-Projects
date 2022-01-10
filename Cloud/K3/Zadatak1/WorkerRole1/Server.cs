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
    class Server
    {
        public bool Opened { get; set; }

        private ServiceHost host;

        public Server(Type serviceType, Type interfaceType)
        {
            Opened = false;
            host = new ServiceHost(serviceType);
            host.Opened += Host_Opened;
            host.Closed += Host_Closed;
            host.AddServiceEndpoint(interfaceType, new NetTcpBinding(), $"net.tcp://{RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["Internal"].IPEndpoint}/Internal");
        }

        private void Host_Closed(object sender, EventArgs e)
        {
            Opened = false;
        }

        private void Host_Opened(object sender, EventArgs e)
        {
            Opened = true;
        }

        public void Open()
        {
            try
            {
                if (!Opened)
                {
                    host.Open();
                    Trace.WriteLine("Server opened.", "Server");
                    return;
                }
                Trace.WriteLine("Server already opened.", "Server");
            }
            catch (Exception)
            {
                Trace.WriteLine("Error opening the server.", "Server");
            }
        }

        public void Close()
        {
            try
            {
                if (Opened)
                {
                    host.Close();
                    Trace.WriteLine("Server closed.", "Server");
                    return;
                }
                Trace.WriteLine("Server is not opened.", "Server");
            }
            catch (Exception)
            {
                Trace.WriteLine("Error closing the server.", "Server");
            }
        }
    }
}
