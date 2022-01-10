using Common;
using System;
using System.ServiceModel;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<IBiblioteka> factory = new ChannelFactory<IBiblioteka>("ServisLica");
            ChannelFactory<IBezbednosniMehanizmi> factory2 = new ChannelFactory<IBezbednosniMehanizmi>("Bezbednost");
            IBiblioteka proxy = factory.CreateChannel();
            IBezbednosniMehanizmi proxy2 = factory2.CreateChannel();

            Clan clan1 = new Clan("Pera", "Peric", "4362356364634");
            Clan clan2 = new Clan("Laza", "Kostic", "548567346436");
            Clan clan3 = new Clan("Velimir", "Josipovic", "34536436456");

            string token = "";

            try
            {
                token = proxy2.Autentifikacija("admin", "admin");
            }
            catch (FaultException<SecException> e)
            {
                Console.WriteLine(e.Detail.Reason);
            }


            try
            {
                proxy.DodajClana(clan1, token);
            }
            catch (FaultException<SecException> e)
            {
                Console.WriteLine(e.Detail.Reason);
            }
            catch (FaultException<DbException> e)
            {
                Console.WriteLine(e.Detail.Reason);
            }



            Console.ReadLine();

        }

    }
}
