using Common;
using System.Collections.Generic;
using System.ServiceModel;

namespace Server
{
    public class Biblioteka : IBiblioteka
    {
        public void DodajNovuKnjigu(Knjiga k, string token)
        {
            ServerDatabase.direktorijumKorisnika.KorisnikAutentifikovan(token);
            ServerDatabase.direktorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);

            if (ServerDatabase.BazaKnjiga.ContainsKey(k.Id) == false)
            {
                ServerDatabase.BazaKnjiga.Add(k.Id, k);     //ako ne sadrzi dodam novu
            }
            else
            {
                DatabaseException ex = new DatabaseException();
                ex.Reason = "Knjiga vec postoji u bazi.";
                throw new FaultException<DatabaseException>(ex);
            }


        }

        public void ObrisiKnjigu(int id, string token)
        {
            ServerDatabase.direktorijumKorisnika.KorisnikAutentifikovan(token);
            ServerDatabase.direktorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);
            if (ServerDatabase.BazaKnjiga.ContainsKey(id))
            {
                ServerDatabase.BazaKnjiga.Remove(id);
            }
            else
            {
                DatabaseException ex = new DatabaseException();
                ex.Reason = "Knjiga ne postoji u bazi.";
                throw new FaultException<DatabaseException>(ex);
            }
        }

        public void ObrisiKnjiguPoNazivu(string naziv, string token)
        {
            ServerDatabase.direktorijumKorisnika.KorisnikAutentifikovan(token);
            ServerDatabase.direktorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);
            bool izbrisao = false;
            foreach (Knjiga knjiga in ServerDatabase.BazaKnjiga.Values)
            {
                if (knjiga.Naziv == naziv)
                {
                    ServerDatabase.BazaKnjiga.Remove(knjiga.Id);
                    izbrisao = true;
                    break;  //OBAVEZNO
                }
            }

            if (izbrisao == false)
            {
                DatabaseException ex = new DatabaseException();
                ex.Reason = "Knjiga ne postoji u bazi.";
                throw new FaultException<DatabaseException>(ex);
            }
        }

        public void UpisiSve(List<Knjiga> lista)
        {
            //ServerDatabase.direktorijumKorisnika.KorisnikAutentifikovan(token); //ako treba autentifikacija
            foreach (Knjiga k in lista)
            {
                ServerDatabase.BazaKnjiga[k.Id] = k;
                //dodati za vreme.. mozda ?
            }

            System.Console.WriteLine("Azurirao celu bazu");
        }

        public List<Knjiga> UzmiSve()
        {
            //ServerDatabase.direktorijumKorisnika.KorisnikAutentifikovan(token);
           
            List<Knjiga> lista = new List<Knjiga>();
            foreach (Knjiga k in ServerDatabase.BazaKnjiga.Values)
            {
                lista.Add(k);
            }

            System.Console.WriteLine("Vratio celu bazu");
            return lista;
        }


        public Knjiga VratiNajskupljuKnjiguAutora(string autor, string token)
        {
            ServerDatabase.direktorijumKorisnika.KorisnikAutentifikovan(token);
            ServerDatabase.direktorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Citanje);

            double max = 0;
            Knjiga ret = null;
            foreach (Knjiga k in ServerDatabase.BazaKnjiga.Values)
            {
                if (k.Autor == autor && k.Cena > max)
                {
                    max = k.Cena;
                    ret = k;
                }
            }

            if (ret == null)
            {
                DatabaseException ex = new DatabaseException();
                ex.Reason = "Autor nije u bazi.";
                throw new FaultException<DatabaseException>(ex);
            }


            return ret;



        }

        public List<Knjiga> VratiSortirane(string token)
        {
            ServerDatabase.direktorijumKorisnika.KorisnikAutentifikovan(token);
            ServerDatabase.direktorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Citanje);
            List<Knjiga> lista = new List<Knjiga>();
            foreach (Knjiga k in ServerDatabase.BazaKnjiga.Values)
            {
                lista.Add(k);
            }


            return lista;
        }
    }
}
