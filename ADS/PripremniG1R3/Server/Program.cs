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
            ServiceHost service = new ServiceHost(typeof(Biblioteka));
            ServiceHost service2 = new ServiceHost(typeof(BezbednosniMehanizmi));
            service.Open();
            service2.Open();



            Console.ReadLine();
            service.Close();
        }
    }
}
