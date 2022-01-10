using Common;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;

namespace Replikator
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<IBanka> factory = new ChannelFactory<IBanka>("Primarni");
            ChannelFactory<IBanka> factory2 = new ChannelFactory<IBanka>("Sekundarni");
            IBanka proxy = factory.CreateChannel();
            IBanka proxy2 = factory2.CreateChannel();


            while (true)
            {
                try
                {

                    List<Racun> lista = proxy.UzmiSve();
                    proxy2.UpisiSve(lista);

                    Console.WriteLine("Izvrsena replikacija");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                Thread.Sleep(10000);
            }



        }
    }
}
