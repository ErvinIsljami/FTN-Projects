using Common;
using System;
using System.ServiceModel;
using System.Threading;

namespace Replikator
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<IBiblioteka> factoryP = new ChannelFactory<IBiblioteka>("ServisLicaP");
            ChannelFactory<IBezbednosniMehanizmi> factory2P = new ChannelFactory<IBezbednosniMehanizmi>("BezbednostP");
            IBiblioteka proxyP = factoryP.CreateChannel();
            IBezbednosniMehanizmi proxy2P = factory2P.CreateChannel();


            ChannelFactory<IBiblioteka> factoryS = new ChannelFactory<IBiblioteka>("ServisLicaS");
            ChannelFactory<IBezbednosniMehanizmi> factory2S = new ChannelFactory<IBezbednosniMehanizmi>("BezbednostS");
            IBiblioteka proxyS = factoryS.CreateChannel();
            IBezbednosniMehanizmi proxy2S = factory2S.CreateChannel();


            string tokenP = "";
            string tokenS = "";

            try
            {
                tokenP = proxy2P.Autentifikacija("replikator1", "replikator1");
                tokenS = proxy2S.Autentifikacija("replikator2", "replikator2");
            }
            catch (FaultException<SecException> e)
            {
                Console.WriteLine(e.Detail.Reason);
            }


            while (true)
            {
                try
                {
                    var baza = proxyP.PreuzmiBazu(tokenP);
                    proxyS.PosaljiBazu(baza, tokenS);
                    Console.WriteLine("Repliciranje zavrseno.");
                }
                catch(FaultException<SecException> e)
                {
                    Console.WriteLine(e.Detail.Reason);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                Thread.Sleep(5000);
            }

        }
    }
}
