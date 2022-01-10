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
            ChannelFactory<IBiblioteka> factoryP = new ChannelFactory<IBiblioteka>("BibliotekaP");
            IBiblioteka proxyP = factoryP.CreateChannel();
            ChannelFactory<IBezbednosniMehanizmi> factory2P = new ChannelFactory<IBezbednosniMehanizmi>("BezbednostP");
            IBezbednosniMehanizmi proxy2P = factory2P.CreateChannel();

            ChannelFactory<IBiblioteka> factoryS = new ChannelFactory<IBiblioteka>("BibliotekaS");
            IBiblioteka proxyS = factoryS.CreateChannel();
            ChannelFactory<IBezbednosniMehanizmi> factory2S = new ChannelFactory<IBezbednosniMehanizmi>("BezbednostS");
            IBezbednosniMehanizmi proxy2S = factory2S.CreateChannel();

            string tokenP = "";
            string tokenS = "";

            try
            {
                tokenP = proxy2P.Autentifikacija("replikator1", "replikator1");
                tokenS = proxy2S.Autentifikacija("replikator2", "replikator2");
            }
            catch(FaultException<SecException> e)
            {
                Console.WriteLine(e.Detail.Reason);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            while(true)
            {
                try
                {
                    var baza = proxyP.PreuzmiBazu(tokenP);
                    proxyS.PosaljiBazu(baza, tokenS);
                    Console.WriteLine("Uspesno replicirani podaci");
                    Thread.Sleep(5000);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

        }
    }
}
