using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<IRoleEnvironment> factory = new ChannelFactory<IRoleEnvironment>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:502/Compute"));
            IRoleEnvironment proxy = factory.CreateChannel();
            Console.WriteLine(proxy.GetAddress("Assembly.dll", "3"));


            Console.ReadLine();
        }
    }
}
