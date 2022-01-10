using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static ChannelFactory<IBiblioteka> factory;
        static ChannelFactory<IBezbednosniMehanizmi> factorySec;
        public static IBiblioteka proxy;
        public static IBezbednosniMehanizmi proxySec;
        public static string token;
        static void Main(string[] args)
        {
            factory = new ChannelFactory<IBiblioteka>("Biblioteka");
            factorySec = new ChannelFactory<IBezbednosniMehanizmi>("Bezbednost");
            proxy = factory.CreateChannel();
            proxySec = factorySec.CreateChannel();
            if(ObradaStanja.konfiguracija.StanjeServera == EStanjeServera.Primarni)
                token = proxySec.Autentifikacija("replikator", "replikator");
            



            ServiceHost service = new ServiceHost(typeof(Biblioteka));
            ServiceHost service2 = new ServiceHost(typeof(BezbednosniMehanizmi));
            service.Open();
            service2.Open();



            Console.ReadLine();
            service.Close();
        }
    }
}
