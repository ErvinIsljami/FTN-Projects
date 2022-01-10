using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ClientServerChannel : IDisposable
    {
        public IChatService ProxyChat
        {
            get
            {              
                NetTcpBinding binding = new NetTcpBinding();
                binding.Security.Mode = SecurityMode.Transport;
                binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
                binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

                EndpointAddress endpointAdress = new EndpointAddress(new Uri("net.tcp://localhost:9999/IChatService"), EndpointIdentity.CreateUpnIdentity("admin@w7ent"));

                DuplexChannelFactory<IChatService> factory = new DuplexChannelFactory<IChatService>(new InstanceContext(new ChatServiceCallback()), binding, endpointAdress);
                return factory.CreateChannel();
            }
        }

        public void Dispose()
        {
            
        }
    }
}
