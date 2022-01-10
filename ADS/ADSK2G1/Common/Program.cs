using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<IBiblioteka> factory = new ChannelFactory<IBiblioteka>("Biblioteka");
            ChannelFactory<IBezbednosniMehanizmi> factory2 = new ChannelFactory<IBezbednosniMehanizmi>("Bezbednost");
            IBezbednosniMehanizmi authProxy = factory2.CreateChannel();
            IBiblioteka proxy = factory.CreateChannel();

            string token = authProxy.Autentifikacija("pera", "P3rA");

            Knjiga k = new Knjiga("LPRS2", "Branislav Atlagic", 100, "19273fasd789");




            try
            {
                proxy.DodajNovuKnjigu(k, token);
            }
            catch (FaultException<SecurityException> e)
            {
                Console.WriteLine(e.Detail.Reason);
            }
            catch(FaultException<DatabaseException> e)
            {
                Console.WriteLine(e.Detail.Reason);
            }


            try
            {
                Knjiga k1 = proxy.VratiNajskupljuKnjiguAutora("Dostojevski", token);
                Console.WriteLine(k1);
            }
            catch (FaultException<SecurityException> e1)
            {
                Console.WriteLine(e1.Detail.Reason);
            }
            catch (FaultException<DatabaseException> e2)
            {
                Console.WriteLine(e2.Detail.Reason);
            }





            Console.ReadLine();
        }
    }
}
