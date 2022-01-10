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
            ServiceHost svc = new ServiceHost(typeof(AutomobilService));
            ServiceHost svc2 = new ServiceHost(typeof(BezbednosniMehanizmi));

            svc.Open();
            svc2.Open();


            Console.ReadLine();
            svc.Close();
            svc2.Close();
        }
    }
}
