using Common;
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
        ServiceHost host;
        public BrotherConnectionService()
        {
            host = new ServiceHost(typeof(BrotherConnection));
            NetTcpBinding binding = new NetTcpBinding();
            Uri address = new Uri("net.tcp://localhost:10100/BrotherConnection");
            host.AddServiceEndpoint(typeof(IBrotherConnection), binding, address);

        }

        public void Open()
        {
            host.Open();
        }

        public void Close()
        {
            host.Close();
        }

        public bool IsOpen()
        {
            return host.State == CommunicationState.Opened;
        }
    }
}
