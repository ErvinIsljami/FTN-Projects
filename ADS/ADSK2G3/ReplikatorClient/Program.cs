using Common;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;

namespace ReplikatorClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<IReplikator> factory1 = new ChannelFactory<IReplikator>("Primarni"); //endpoint name iz app.config-a
            ChannelFactory<IReplikator> factory2 = new ChannelFactory<IReplikator>("Sekundarni"); //endpoint name iz app.config-a

            IReplikator proxy1 = factory1.CreateChannel();
            IReplikator proxy2 = factory2.CreateChannel();

            while (true)
            {
                try
                {
                    List<Telefon> lista = proxy1.UzmiSve();
                    proxy2.UpisiSve(lista);
                    Console.WriteLine("Replikacija odradjena");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                Thread.Sleep(5000);


            }

        }
    }
}
