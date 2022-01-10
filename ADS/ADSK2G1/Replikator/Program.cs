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
            /* ovo treba samo ako imamo autentifikaciju i replikaciju */
            /*
            // autentifikacija i preuzimanje tokena sa primara
            ChannelFactory<IBezbednosniMehanizmi> factory12 = new ChannelFactory<IBezbednosniMehanizmi>("BezbednostPrimarni");
            IBezbednosniMehanizmi authProxy1 = factory12.CreateChannel();
            string token1 = authProxy1.Autentifikacija("admin", "admin");
            // autentifikacija i preuzimanje tokena sa sekundara
            ChannelFactory<IBezbednosniMehanizmi> factory22 = new ChannelFactory<IBezbednosniMehanizmi>("BezbednostSekundarni");
            IBezbednosniMehanizmi authProxy2 = factory22.CreateChannel();
            string token2 = authProxy2.Autentifikacija("admin", "admin");
            */

            //konekcija na primarni server
            ChannelFactory<IBiblioteka> factory1 = new ChannelFactory<IBiblioteka>("BibliotekaPrimarni");
            IBiblioteka proxy1 = factory1.CreateChannel();

            //konekcija na sekundarni server
            ChannelFactory<IBiblioteka> factory2 = new ChannelFactory<IBiblioteka>("BibliotekaSekundarni");
            IBiblioteka proxy2 = factory2.CreateChannel();

            while(true)
            {
                try
                {
                    var lista = proxy1.UzmiSve();
                    proxy2.UpisiSve(lista);
                    Console.WriteLine("Replikacija odradjena...");
                }
                catch (FaultException<SecurityException> e) //ovo radite samo kod autentifikacije
                {
                    Console.WriteLine(e.Detail.Reason);
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
