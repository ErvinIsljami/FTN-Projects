using Common;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace JobWorker
{
    public class BrotherConnectionService
    {
        private ServiceHost host;

        public BrotherConnectionService()
        {
            host = new ServiceHost(typeof(BrotherConnection));
            string addr = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["InternalEndpoint"].IPEndpoint.Address.ToString();
            int port = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["InternalEndpoint"].IPEndpoint.Port;
            Uri address = new Uri($"net.tcp://{addr}:{port}/Brother");

            //Uri address = new Uri($"net.tcp://localhost:15000/Brother");

            NetTcpBinding binding = new NetTcpBinding();
            host.AddServiceEndpoint(typeof(IBrotherConnection), binding, address);
        }

        public void Open()
        {
            host = new ServiceHost(typeof(BrotherConnection));
            string addr = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["InternalEndpoint"].IPEndpoint.Address.ToString();
            int port = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["InternalEndpoint"].IPEndpoint.Port;
            Uri address = new Uri($"net.tcp://{addr}:{port}/Brother");

            //Uri address = new Uri($"net.tcp://localhost:15000/Brother");

            NetTcpBinding binding = new NetTcpBinding();
            host.AddServiceEndpoint(typeof(IBrotherConnection), binding, address);

            host.Open();
        }

        public bool IsOpened()
        {
            return host.State == CommunicationState.Opened;
        }

        public void Close()
        {
            host.Close();
        }
    }
}
