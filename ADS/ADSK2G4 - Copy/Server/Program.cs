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
            ServiceHost host = new ServiceHost(typeof(LaptopService));
            ServiceHost hostSec = new ServiceHost(typeof(BezbednosniMehanizmi));
            hostSec.Open();
            host.Open();




            Console.ReadLine();
            host.Close();
            hostSec.Close();
        }
    }
}
