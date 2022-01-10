using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Contracts;

namespace AssetManagementSystem.Services
{
    public class LCToAMSServiceHost
    {
        private ServiceHost serviceHost;

        public LCToAMSServiceHost()
        {
            serviceHost = new ServiceHost(typeof(LCToAMSMeasurements));
            NetTcpBinding netTcpBinding = new NetTcpBinding();
            netTcpBinding.PortSharingEnabled = true;
            Uri address = new Uri("net.tcp://localhost:42000/AMS");
            serviceHost.AddServiceEndpoint(typeof(IControllerChangeSetService), netTcpBinding, address);
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
