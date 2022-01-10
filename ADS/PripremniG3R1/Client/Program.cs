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
            IBiblioteka proxy = factory.CreateChannel();

            ChannelFactory<IBezbednosniMehanizmi> factory2 = new ChannelFactory<IBezbednosniMehanizmi>("Bezbednost");
            IBezbednosniMehanizmi proxy2 = factory2.CreateChannel();

            Knjiga k1 = new Knjiga("Mali princ", "Antonio de santoaisdjf", 800);
            Knjiga k2 = new Knjiga("Zlocin i kazna", "Dostojevski", 1100);
            Knjiga k3 = new Knjiga("Na Drini cuprija", "Ivo Andric", 1250);
            Knjiga k4 = new Knjiga("Orlovi rano lete", "Branko Copic", 700);
            Knjiga k5 = new Knjiga("Ana Karenjina", "Tolstoj", 1200);
            Knjiga k6 = new Knjiga("Harry Potter", "J.K.Rowling", 1300);

            Clan c1 = new Clan("Evgenije", "Petrovic", "21876492871");
            Clan c2 = new Clan("Pero", "Peric", "2984728454");
            Clan c3 = new Clan("Radivoje", "Karadjordjevic", "1298379384");

            c1.ListaKnjiga.Add(k1);
            c1.ListaKnjiga.Add(k2);
            c1.ListaKnjiga.Add(k3);

            c2.ListaKnjiga.Add(k4);

            c3.ListaKnjiga.Add(k5);
            c3.ListaKnjiga.Add(k6);


            string token = "";

            try
            {
                token = proxy2.Autentifikacija("admin", "admin");
            }
            catch(FaultException<SecException> e)
            {
                Console.WriteLine(e.Detail.Reason);
            }


            try
            {
                proxy.DodajClana(c1, token);
                proxy.DodajClana(c2, token);
                proxy.DodajClana(c3, token);
                var lista = proxy.VratiSortiraneClanovePoUkupnojCeniKnjiga(token);
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
