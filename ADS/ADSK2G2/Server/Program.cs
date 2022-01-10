using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(ProdavnicaAutomobila));
            ServiceHost host2 = new ServiceHost(typeof(SecurityService));
            ServiceHost host3 = new ServiceHost(typeof(ObradaStanja));
            host3.Open();
            host2.Open();
            host.Open();




            Console.ReadLine();
            host.Close();
            host2.Close();
        }
    }
}
