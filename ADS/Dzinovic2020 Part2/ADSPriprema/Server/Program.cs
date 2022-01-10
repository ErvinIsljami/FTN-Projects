using Common;
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
            ServiceHost svc = new ServiceHost(typeof(Biblioteka));
            ServiceHost svc2 = new ServiceHost(typeof(BezbednosniMehanizmi));
            ServiceHost svc3 = new ServiceHost(typeof(Replicate));
            svc.Open();
            svc2.Open();
            svc3.Open();



            Console.ReadLine();
            svc.Close();
            svc2.Close();
            svc3.Close();
        }
    }
}
