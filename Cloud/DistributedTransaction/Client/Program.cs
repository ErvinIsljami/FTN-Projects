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
            Connect();
            if (proxy.OrderItem("MaliPrinc", "Ervin"))
            {
                Console.WriteLine("Uspesno kupljena knjiga");
            }
            else
            {
                Console.WriteLine("Transakcija neuspesna");
            }
            Console.ReadLine();
        }

        static IPurchase proxy;
        static void Connect()
        {
            var binding = new NetTcpBinding();
            ChannelFactory<IPurchase> factory = new
           ChannelFactory<IPurchase>(binding, new
           EndpointAddress("net.tcp://localhost:10100/Purchase"));
            proxy = factory.CreateChannel();

        }
    }
}
