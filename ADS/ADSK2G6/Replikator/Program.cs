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
            ChannelFactory<IApoteka> factoryApotekaP = new ChannelFactory<IApoteka>("PrimarniApoteka");
            ChannelFactory<IBezbednosniMehanizmi> factoryBezbednostP = new ChannelFactory<IBezbednosniMehanizmi>("PrimarniBezbednost"); 
            IApoteka proxyApotekaP = factoryApotekaP.CreateChannel();
            IBezbednosniMehanizmi proxyBezbednostP = factoryBezbednostP.CreateChannel();

            ChannelFactory<IApoteka> factoryApotekaS = new ChannelFactory<IApoteka>("SekundarniApoteka");
            ChannelFactory<IBezbednosniMehanizmi> factoryBezbednostS = new ChannelFactory<IBezbednosniMehanizmi>("SekundarniBezbednost");
            IApoteka proxyApotekaS = factoryApotekaP.CreateChannel();
            IBezbednosniMehanizmi proxyBezbednostS = factoryBezbednostP.CreateChannel();

            string tokenPrimarni = proxyBezbednostP.Autentifikacija("replikator", "replikator");
            string tokenSekundarni = proxyBezbednostS.Autentifikacija("replikator2", "replikator2");

            while(true)
            {
                try
                {
                    Dictionary<int, Lek> baza = proxyApotekaP.PreuzmiBazu(tokenPrimarni);
                    proxyApotekaS.PosaljiBazu(baza, tokenSekundarni);
                }
                catch(FaultException<SecurityException> e)
                {
                    Console.WriteLine(e.Detail.Reason);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                Thread.Sleep(5000);
                Console.WriteLine("Replikacija odradjena");
            }




        }
    }
}
