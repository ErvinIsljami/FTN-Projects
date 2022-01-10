using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(Biblioteka));
            ServiceHost host2 = new ServiceHost(typeof(Bezbednost));
            host.Open();
            host2.Open();
            Console.WriteLine("Servis je pokrenut");





            Console.ReadLine();
            host.Close();
            host2.Close();
          
        }
    }
}
