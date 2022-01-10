using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Contracts;

namespace AssetManagementSystem.Services
{
    public class LDToAMSServiceHost
    {
        private ServiceHost serviceHost;

        public LDToAMSServiceHost()
        {
            serviceHost = new ServiceHost(typeof(LDToAMSMeasurements));
            NetTcpBinding netTcpBinding = new NetTcpBinding();
            netTcpBinding.PortSharingEnabled = true;
            Uri address = new Uri("net.tcp://localhost:41000/AMS");
            serviceHost.AddServiceEndpoint(typeof(IDeviceChangeSetService), netTcpBinding, address);
        }

        public void OpenService()
        {
            if (serviceHost.State != CommunicationState.Opened)
            {
                serviceHost.Open();
            }

        }
        public void CloseService()
        {
            if (serviceHost.State == CommunicationState.Opened)
            {
                serviceHost.Close();
            }
        }
    }
}
