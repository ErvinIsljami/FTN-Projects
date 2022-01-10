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
            IReplicate proxyPrimarni;
            ChannelFactory<IReplicate> factoryPrimarni = new ChannelFactory<IReplicate>("Replikacija");

            IBezbednosniMehanizmi proxy2Primarni;
            ChannelFactory<IBezbednosniMehanizmi> factory2Primarni = new ChannelFactory<IBezbednosniMehanizmi>("Kurcina");

            IReplicate proxySekundarni;
            ChannelFactory<IReplicate> factorySekundarni = new ChannelFactory<IReplicate>("Replikacija2");

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
                try
                {

                    var baza = proxyPrimarni.PreuzmiBazu(tokenPrimarni);
                    proxySekundarni.PosaljiBazu(tokenSekundarni, baza);
                }
                catch(FaultException<BezbednosniIzuzetak> e)
                {
                    Console.WriteLine(e.Detail.Reason);
                }
                catch(Exception e)
                { 
                    Console.WriteLine(e.Message);
                }

                Console.WriteLine("Replikacija...");

                Thread.Sleep(5000);
            }

        }
    }
}
