using ChatRoom;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SecondaryServer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(SecondaryServer));
            NetTcpBinding binding = new NetTcpBinding();
            host.AddServiceEndpoint(typeof(ISecondaryServer), binding, new Uri("net.tcp://localhost:8888/ISecondaryServer"));

            //binding.Security.Mode = SecurityMode.Transport;
            //binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            //binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            //host2 sluzi za replikaciju

            EndpointAddress address2 = new EndpointAddress(new Uri("net.tcp://localhost:4000/Replication"));
            NetTcpBinding binding2 = new NetTcpBinding();
            ChannelFactory<IReplication> factory = new ChannelFactory<IReplication>(binding2, address2);
            IReplication kanal = factory.CreateChannel();

            ServiceHost host2 = new ServiceHost(typeof(Replication));
            host2.Open();

            host.Open();
            

            Console.WriteLine("Secondary Server is started.");

            Console.ReadLine();
            host.Close();
        }
    }
}