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
            ServiceHost serviceHost = new ServiceHost(typeof(ProdavnicaTelefona));
            ServiceHost serviceHost2 = new ServiceHost(typeof(BezbednosniMehanizmi));
            ServiceHost serviceHost3 = new ServiceHost(typeof(Replikator));
            serviceHost.Open();
            serviceHost2.Open();
            serviceHost3.Open();





            Console.ReadLine();
            serviceHost.Close();
            serviceHost2.Close();
            serviceHost3.Close();
        }
    }
}
