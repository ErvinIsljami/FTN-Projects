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
            IBiblioteka proxy;
            ChannelFactory<IBiblioteka> factory = new ChannelFactory<IBiblioteka>("Biblioteka");
            
            IBezbednosniMehanizmi proxy2;
            ChannelFactory<IBezbednosniMehanizmi> factory2 = new ChannelFactory<IBezbednosniMehanizmi>("Kurcina");
            
            
            proxy = factory.CreateChannel();
            proxy2 = factory2.CreateChannel();

            string token = "";
            string token2 = "";
            
            try
            {
                token = proxy2.Autentifikacija("admin", "admin");
                token2 = proxy2.Autentifikacija("pera", "pera");
            }
            catch(FaultException<BezbednosniIzuzetak> e)
            {
                Console.WriteLine(e.Detail.Reason);
            }

            Console.WriteLine(token);

            Clan c1 = new Clan("ana", "jovic", 534553453245) { Knjige = { "Alisa u zemlji cuda", "GoT" } };
            Clan c2 = new Clan("marija", "jovic", 87987685678) { Knjige = { "Harry Potter 3", "GoT", "Harry Potter 4" } };
            Clan c3 = new Clan("jovana", "jovic", 56785678578) { Knjige = { "Druzina prstena", "Dve Kule", "Povratak kralja"} };
            Clan c4 = new Clan("milojka", "jovic", 456745674567);

            try
            {
                proxy.DodajClana(token, c1);
                proxy.DodajClana(token, c2);
                proxy.DodajClana(token, c3);
                proxy.DodajClana(token, c4);
                proxy.DodajClana(token2, c4);
                
                if(proxy.DodajKnjiguClanu(token, 564645645, "53453") == false)
                {
                    Console.WriteLine("Nije uspesno dodao knjigu clanu");
                }
            }
            catch (FaultException<BezbednosniIzuzetak> e)
            {
                Console.WriteLine(e.Detail.Reason);
            }












            Console.ReadLine();
        }
    }
}
