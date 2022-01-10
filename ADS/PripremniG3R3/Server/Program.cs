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
        private static ChannelFactory<IBiblioteka> factory;
        private static ChannelFactory<IBezbednosniMehanizmi> factorySec;

        public static IBiblioteka proxy;
        public static IBezbednosniMehanizmi proxySec;
        public static string token;

        static void Main(string[] args)
        {
            if(Properties.Settings.Default.StanjeServera == EStanjeServera.Primarni)
            {
                factorySec = new ChannelFactory<IBezbednosniMehanizmi>("Bezbednost");
                proxySec = factorySec.CreateChannel();

                factory = new ChannelFactory<IBiblioteka>("Biblioteka");
                proxy = factory.CreateChannel();

                try
                {
                    token = proxySec.Autentifikacija("replikator", "replikator");
                }
                catch(FaultException<SecException> e)
                {
                    Console.WriteLine(e.Detail.Reason);
                }
            }

            ServiceHost host = new ServiceHost(typeof(Biblioteka));
            ServiceHost host2 = new ServiceHost(typeof(BezbednosniMehanizmi));
            host.Open();
            host2.Open();




            Console.ReadLine();
            host.Close();
            host2.Close();
        }
    }
}
