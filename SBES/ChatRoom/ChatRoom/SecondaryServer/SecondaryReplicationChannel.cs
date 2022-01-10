using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SecondaryServer
{
    public class SecondaryReplicationChannel:IDisposable
    {
        public IReplication ProxyReplication
        {
            get
            {
                NetTcpBinding binding = new NetTcpBinding();
                binding.Security.Mode = SecurityMode.Transport;
                binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
                binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

                EndpointAddress endpointAdress = new EndpointAddress(new Uri("net.tcp://localhost:4001/IReplication"), EndpointIdentity.CreateUpnIdentity("admin@w7entt"));

                ChannelFactory<IReplication> factory = new ChannelFactory<IReplication>(binding, endpointAdress);
                return factory.CreateChannel();
            }
        }

        public void Dispose()
        {

        }
    }
}
