using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LocalController.Services
{
    public class ControllerDeviceServiceHost
    {
        private ServiceHost serviceHost;

        public ControllerDeviceServiceHost(string path)
        {
            serviceHost = new ServiceHost(typeof(LDToLCMeasurements));
            NetTcpBinding netTcpBinding = new NetTcpBinding();
            netTcpBinding.PortSharingEnabled = true;
            Uri address = new Uri("net.tcp://localhost:41000/" + path);
            serviceHost.AddServiceEndpoint(typeof(IDeviceChangeSetService), netTcpBinding, address);
        }

        public bool OpenService()
        {
            if(serviceHost.State != CommunicationState.Opened)
            {
                serviceHost.Open();
                return true;
            }
            return false;
            
        }
        public bool CloseService()
        {
            if(serviceHost.State == CommunicationState.Opened)
            {
                serviceHost.Close();
                return true;
            }
            return false;
        }
    }
}
