using Common;
using System;
using System.ServiceModel;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<IBiblioteka> factory = new ChannelFactory<IBiblioteka>("Biblioteka");
            ChannelFactory<IBezbednosniMehanizmi> factorySec = new ChannelFactory<IBezbednosniMehanizmi>("Bezbednost");
            IBiblioteka proxy = factory.CreateChannel();
            IBezbednosniMehanizmi proxySec = factorySec.CreateChannel();

            Knjiga k1 = new Knjiga("Ana Karenjina", 900, "23452345");
            Knjiga k2 = new Knjiga("Idiot", 1200, "3474262435");
            Knjiga k3 = new Knjiga("Zlocin i kazna", 850, "236775474325");
            Knjiga k4 = new Knjiga("Mali princ", 500, "2345234164");
            Knjiga k5 = new Knjiga("Faust", 2000, "456745365");

            Clan clan1 = new Clan("ervin", "isljami", "30039967335213245");
            Clan clan2 = new Clan("aleksandar", "puskin", "7457863457");
            Clan clan3 = new Clan("lav nikolajevic", "tolstoj", "547436745546");

            string token = null;

            try
            {
                token = proxySec.Autentifikacija("admin", "admin");
                Console.WriteLine("Token: " + token);
            }
            catch (FaultException<SecurityException> e)
            {
                Console.WriteLine(e.Detail.Reason);
            }

            Console.ReadLine();

            try
            {
                proxy.DodajClana(clan3, token);
            }
            catch(FaultException<SecurityException> e)
            {
                Console.WriteLine(e.Detail.Reason);
            }
            catch (FaultException<DatabaseException> e)
            {
                Console.WriteLine(e.Detail.Reason);
            }





            Console.ReadLine();

        }
    }
}
