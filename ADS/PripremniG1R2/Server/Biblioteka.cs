using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Biblioteka : IBiblioteka
    {
        public void DodajClana(Clan clan, string token)
        {
            ServerDatabase.direktorijumKorisnika.KorisnikAutentifikovan(token);
            ServerDatabase.direktorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);

            //if(ServerDatabase.Korisnici[username].Autentifikovan == true)
            //{

            //}

            if(ObradaStanja.konfiguracija.StanjeServera == EStanjeServera.Primarni)
            {
                Program.proxy.PosaljiBazu(ServerDatabase.Clanovi, Program.token);
                Console.WriteLine("Poslao sam sekundaru sve");
            }

            if (ServerDatabase.Clanovi.ContainsKey(clan.Jmbg) == false)
            {
                ServerDatabase.Clanovi.Add(clan.Jmbg, clan);
                Console.WriteLine("Dodat novi korisnik: " + clan.ToString());
            }
            else
            {
                DatabaseException e = new DatabaseException("Korisnik sa zadatim jmbg-om vec postoji");
                throw new FaultException<DatabaseException>(e);
            }
        }

        public void DodajKnjigu(Knjiga knjiga, string token)
        {
            ServerDatabase.direktorijumKorisnika.KorisnikAutentifikovan(token);
            ServerDatabase.direktorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);

            if (ServerDatabase.Knjige.ContainsKey(knjiga.Isbn) == false)
            {
                ServerDatabase.Knjige.Add(knjiga.Isbn, knjiga);
                Console.WriteLine("Dodali smo: " + knjiga);
            }
            else
            {
                DatabaseException e = new DatabaseException("Knjiga vec postoji");
                throw new FaultException<DatabaseException>(e);
            }




        }

        public void IzbrisiClana(string jmbg, string token)
        {
            ServerDatabase.direktorijumKorisnika.KorisnikAutentifikovan(token);
            ServerDatabase.direktorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);
            if (ServerDatabase.Clanovi.ContainsKey(jmbg))
            {
                ServerDatabase.Clanovi.Remove(jmbg);
                Console.WriteLine("Izbrisan je korisnik sa jmbgom: " + jmbg);
            }
            else
            {
                DatabaseException e = new DatabaseException("Korisnik ne postoji");
                throw new FaultException<DatabaseException>(e);
            }
        }

        public void IzmeniClana(Clan clan, string token)
        {
            ServerDatabase.direktorijumKorisnika.KorisnikAutentifikovan(token);
            ServerDatabase.direktorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);
            if (ServerDatabase.Clanovi.ContainsKey(clan.Jmbg))
            {
                ServerDatabase.Clanovi[clan.Jmbg] = clan;
            }
            else
            {
                DatabaseException e = new DatabaseException("Korisnik ne postoji");
                throw new FaultException<DatabaseException>(e);
            }
        }

        public Clan NajveciBurzuj(string token)
        {
            ServerDatabase.direktorijumKorisnika.KorisnikAutentifikovan(token);
            ServerDatabase.direktorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Citanje);
            Clan ret = null;
            double max = 0;

            foreach (Clan c in ServerDatabase.Clanovi.Values)
            {
                //if(max < c.Lista.Sum(x => x.Cena))
                //{
                //    max = c.Lista.Sum(x => x.Cena);
                //    ret = c;
                //}
                double suma = 0;
                foreach (Knjiga k in c.Lista)
                {
                    suma += k.Cena;
                }
                if (max < suma)
                {
                    max = suma;
                    ret = c;
                }



            }


            return ret;
        }

        public void ObrisiKnjigu(string isbn, string token)
        {
            ServerDatabase.direktorijumKorisnika.KorisnikAutentifikovan(token);
            ServerDatabase.direktorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);
            if (ServerDatabase.Knjige.ContainsKey(isbn))
            {
                ServerDatabase.Knjige.Remove(isbn);
            }
            else
            {
                DatabaseException e = new DatabaseException("Knjiga ne postoji");
                throw new FaultException<DatabaseException>(e);
            }
        }

        public void PosaljiBazu(Dictionary<string, Clan> baza, string token)
        {
            ServerDatabase.Clanovi = baza;
            Console.WriteLine("Baza je azurirana");
        }

        public Dictionary<string, Clan> PreuzmiBazu(string token)
        {
            Console.WriteLine("Poslao celu bazu");
            return ServerDatabase.Clanovi;
        }
    }
}
