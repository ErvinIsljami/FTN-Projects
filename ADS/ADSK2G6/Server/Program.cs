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
            ServiceHost service = new ServiceHost(typeof(Apoteka));
            ServiceHost service2 = new ServiceHost(typeof(BezbednosniMehanizmi));
            service2.Open();
            service.Open();



            Console.ReadLine();
            service.Close();
            service2.Close();
        }
    }
}
