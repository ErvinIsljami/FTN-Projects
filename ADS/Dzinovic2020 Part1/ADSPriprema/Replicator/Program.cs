using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Replicator
{
    class Program
    {
        static void Main(string[] args)
        {
            IBiblioteka proxyPrimarni;
            ChannelFactory<IBiblioteka> factoryPrimarni = new ChannelFactory<IBiblioteka>("Biblioteka");

            IBezbednosniMehanizmi proxy2Primarni;
            ChannelFactory<IBezbednosniMehanizmi> factory2Primarni = new ChannelFactory<IBezbednosniMehanizmi>("Kurcina");

            IBiblioteka proxySekundarni;
            ChannelFactory<IBiblioteka> factorySekundarni = new ChannelFactory<IBiblioteka>("Biblioteka2");

            IBezbednosniMehanizmi proxy2Sekundarni;
            ChannelFactory<IBezbednosniMehanizmi> factory2Sekundarni = new ChannelFactory<IBezbednosniMehanizmi>("Kurcina2");


            proxyPrimarni = factoryPrimarni.CreateChannel();
            proxy2Primarni = factory2Primarni.CreateChannel();

            proxySekundarni = factorySekundarni.CreateChannel();
            proxy2Sekundarni = factory2Sekundarni.CreateChannel();

            string tokenPrimarni = proxy2Primarni.Autentifikacija("rep", "rep");
            string tokenSekundarni = proxy2Sekundarni.Autentifikacija("rep", "rep");

            while(true)
            {
                var podaci = proxyPrimarni.PreuzmiBazu(tokenPrimarni);

                proxySekundarni.PosaljiBazu(tokenSekundarni, podaci);

                Console.WriteLine("Replikacija odradjena...");
                Thread.Sleep(5000);
            }


        }
    }
}
