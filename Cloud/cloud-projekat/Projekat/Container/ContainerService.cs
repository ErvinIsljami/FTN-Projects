using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Contracts;
namespace Container
{
    public class ContainerService
    {
        private ServiceHost serviceHost;
        public static int port;
        public ContainerService(string portt)
        {
            port = Int32.Parse(portt);
            Start(portt);
        }
        public void Start(string port)
        {
            serviceHost = new ServiceHost(typeof(Container));
            NetTcpBinding binding = new NetTcpBinding();
            Uri adresa = new Uri("net.tcp://localhost:"+ port + "/Container");    
            serviceHost.AddServiceEndpoint(typeof(IContainer), binding, adresa);
            serviceHost.Open();
            Console.WriteLine("Service opened on " + port);
        }
        public void Stop()
        {
            serviceHost.Close();
        }
    }
}
