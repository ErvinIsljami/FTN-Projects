using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class LaptopService : ILaptopService
    {
        public void DodajLaptop(Laptop l, string token)
        {
            ServerDatabase.direktorijumKorisnika.KorisnikAutentifikovan(token);
            ServerDatabase.direktorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);

            if(ServerDatabase.Laptopovi.ContainsKey(l.Id) == false)
            {
                ServerDatabase.Laptopovi.Add(l.Id, l);
                Console.WriteLine("Dodao sam " + l);
            }
            else
            {
                DatabaseException e = new DatabaseException("Laptop sa zadatim id-ijem vec postoji u bazi.");
                throw new FaultException<DatabaseException>(e);
            }
        }

        public void ObrisiLaptop(int id, string token)
        {
            ServerDatabase.direktorijumKorisnika.KorisnikAutentifikovan(token);
            ServerDatabase.direktorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);

            if (ServerDatabase.Laptopovi.ContainsKey(id) == true)
            {
                ServerDatabase.Laptopovi.Remove(id);
            }
            else
            {
                DatabaseException e = new DatabaseException("Laptop sa zadatim id-ijem ne postoji u bazi.");
                throw new FaultException<DatabaseException>(e);
            }
        }

        public void UpisiSve(List<Laptop> lista)
        {
            foreach (Laptop laptop in lista)
            {
                ServerDatabase.Laptopovi[laptop.Id] = laptop;
            }
            Console.WriteLine("Sve upisano...");
        }

        public Laptop VratiNajskupljiIzMarke(string marka, string token)
        {
            ServerDatabase.direktorijumKorisnika.KorisnikAutentifikovan(token);
            ServerDatabase.direktorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Citanje);


            Laptop l = null;    //koristi null a ne new
            double maxCena = 0;
            foreach (Laptop laptop in ServerDatabase.Laptopovi.Values)
            {
                if(laptop.Marka == marka && laptop.Cena > maxCena)
                {
                    maxCena = laptop.Cena;
                    l = laptop;
                }
            }

            return l;
        }

        public List<Laptop> VratiSortirane(string token)
        {
            ServerDatabase.direktorijumKorisnika.KorisnikAutentifikovan(token);
            ServerDatabase.direktorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Citanje);

            return ServerDatabase.Laptopovi.Values.OrderBy(x => x.Cena).ToList();
        }

        public List<Laptop> VratiSve()
        {
            Console.WriteLine("Sve poslato");
            return ServerDatabase.Laptopovi.Values.ToList();
        }

        public List<Laptop> VratiUIntervalu(double gornjaGranica, double donjaGranica, string token)
        {
            ServerDatabase.direktorijumKorisnika.KorisnikAutentifikovan(token);
            ServerDatabase.direktorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Citanje);

            List<Laptop> ret = new List<Laptop>();
            foreach(Laptop l in ServerDatabase.Laptopovi.Values)
            {
                if(l.Cena > donjaGranica && l.Cena < gornjaGranica)
                {
                    ret.Add(l);
                }
            }
            

            return ret;
        }
    }
}
