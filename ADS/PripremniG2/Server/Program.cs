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
            ServiceHost host = new ServiceHost(typeof(Biblioteka));
            ServiceHost host2 = new ServiceHost(typeof(BezbednosniMehanizmi));
            host.Open();
            host2.Open();




            Console.ReadLine();
            host.Close();
        }
    }
}
