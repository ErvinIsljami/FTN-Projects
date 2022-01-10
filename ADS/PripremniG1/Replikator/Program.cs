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
            ChannelFactory<IBiblioteka> factory1 = new ChannelFactory<IBiblioteka>("BibliotekaP");
            ChannelFactory<IBezbednosniMehanizmi> factorySec1 = new ChannelFactory<IBezbednosniMehanizmi>("BezbednostP");
            IBiblioteka proxy1 = factory1.CreateChannel();
            IBezbednosniMehanizmi proxySec1 = factorySec1.CreateChannel();

            ChannelFactory<IBiblioteka> factory2 = new ChannelFactory<IBiblioteka>("BibliotekaS");
            ChannelFactory<IBezbednosniMehanizmi> factorySec2 = new ChannelFactory<IBezbednosniMehanizmi>("BezbednostS");
            IBiblioteka proxy2 = factory2.CreateChannel();
            IBezbednosniMehanizmi proxySec2 = factorySec2.CreateChannel();

            string tokenP = proxySec1.Autentifikacija("replikator", "replikator");
            string tokenS = proxySec2.Autentifikacija("replikator1", "replikator1");

            while(true)
            {
                var baza = proxy1.PreuzmiBazu(tokenP);
                proxy2.PosaljiBazu(baza, tokenS);
                Console.WriteLine("Replikacija uspesna");

                Thread.Sleep(5000);
            }
            
        }
    }
}
