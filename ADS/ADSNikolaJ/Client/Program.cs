using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            IAutomobilService proxy;
            IBezbednosniMehanizmi proxy2;
            ChannelFactory<IAutomobilService> factory = new ChannelFactory<IAutomobilService>("Automobili");
            ChannelFactory<IBezbednosniMehanizmi> factory2 = new ChannelFactory<IBezbednosniMehanizmi>("Bezbednost");

            proxy = factory.CreateChannel();
            proxy2 = factory2.CreateChannel();

            string token = proxy2.Autentifikacija("admin", "admin");

            Automobil a1 = new Automobil(4300, "Audi", "A3", "faisoj9132r223");
            Automobil a2 = new Automobil(5345, "Audi", "A4", "f345asdfkjr223");
            Automobil a3 = new Automobil(12354, "Audi", "A5", "fg3sdfgsdf");
            Automobil a4 = new Automobil(1245, "Audi", "A6", "f34ggsdfgw31");
            Automobil a5 = new Automobil(6544, "Opel", "Astra", "g324gefsgsfdg");
            Automobil a6 = new Automobil(4300, "Audi", "A3", "faisoj9132r223");

            try
            {
                proxy.DodajAutomobil(a1, token);
                proxy.DodajAutomobil(a2, token);
                proxy.DodajAutomobil(a3, token);
                proxy.DodajAutomobil(a4, token);
                proxy.DodajAutomobil(a5, token);
                proxy.DodajAutomobil(a6, token);
            }
            catch(FaultException<MyException> e)
            {
                Console.WriteLine(e.Detail.Reason);
            }
            catch (FaultException<BezbednosniIzuzetak> e)
            {
                Console.WriteLine(e.Detail.Reason);
            }

            try
            {
                proxy.ObrisiAutomobil("f34ggsdfgw31", token);
                proxy.ObrisiAutomobil("f34ggsdgsdfgsdfgsdgfgfgw31", token);
            }
            catch (FaultException<MyException> e)
            {
                Console.WriteLine(e.Detail.Reason);
            }
            catch (FaultException<BezbednosniIzuzetak> e)
            {
                Console.WriteLine(e.Detail.Reason);
            }

            var sviAutomobili = proxy.IzlistajSveAutomobile(token);
            Console.WriteLine(sviAutomobili);

            var sviAudi = proxy.IzlistajAutomobileNekeMarke("Audi", token);

            var sortirani = proxy.SortirajAutomobile(token);


            Console.ReadLine();
        }
    }
}
