using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LocalController
{
    public class AMSProxy
    {
        public IControllerChangeSetService Proxy { get; set; }

        public AMSProxy()
        {
            EndpointAddress address = new EndpointAddress("net.tcp://localhost:42000/AMS");
            NetTcpBinding netTcpBinding = new NetTcpBinding();
            ChannelFactory<IControllerChangeSetService> channelFactory = new ChannelFactory<IControllerChangeSetService>(netTcpBinding, address);
            Proxy = channelFactory.CreateChannel();
        }
    }
}
