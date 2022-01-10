using Common;
using SecondaryServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ReplicationServer
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();

            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            ServiceHost host = new ServiceHost(typeof(ReplicationServer));
            host.AddServiceEndpoint(typeof(IReplication), binding, new Uri("net.tcp://localhost:4001/IReplication"));

            host.Open();
            Console.WriteLine("Replication Server is started.");
            Console.ReadLine();
  
            host.Close();
        }
    }
}
