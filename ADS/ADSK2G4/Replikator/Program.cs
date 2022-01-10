using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Replikator
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<ILaptopService> factory = new ChannelFactory<ILaptopService>("Primarni");
            ILaptopService proxy = factory.CreateChannel();

            ChannelFactory<ILaptopService> factory2 = new ChannelFactory<ILaptopService>("Sekundarni");
            ILaptopService proxy2 = factory2.CreateChannel();

            while(true)
            {
                var lista = proxy.VratiSve();
                proxy2.UpisiSve(lista);
                Console.WriteLine("Replikacija uspesno odradjena");

                Thread.Sleep(5000);
             }


        }
    }
}
