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
            ServerDatabase.direktorijumKorisnika.KorisnikAutentifikovan(token); //da li je autentifikovan ?
            ServerDatabase.direktorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);

            if (ServerDatabase.Clanovi.ContainsKey(clan.Jmbg) == false)
            {
                ServerDatabase.Clanovi.Add(clan.Jmbg, clan);
                Console.WriteLine("Dodali smo novog clana: " + clan);
            }
            else
            {
                DbException e = new DbException("Kornisk vec postoji sa zadatim jmbg-om");
                throw new FaultException<DbException>(e);
            }
        }

        public void IzbrisiClana(string jmbg, string token)
        {
            ServerDatabase.direktorijumKorisnika.KorisnikAutentifikovan(token); //da li je autentifikovan ?
            ServerDatabase.direktorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);

            if(ServerDatabase.Clanovi.ContainsKey(jmbg))
            {
                ServerDatabase.Clanovi.Remove(jmbg);
                Console.WriteLine($"Clan sa jmbg-om: {jmbg} izbrisan");
            }
            else
            {
                DbException e = new DbException($"Korisnik sa jmbg-om {jmbg} ne postoji.");
                throw new FaultException<DbException>(e);
            }
        }

        public void IzmeniClana(Clan clan, string token)
        {
            ServerDatabase.direktorijumKorisnika.KorisnikAutentifikovan(token); //da li je autentifikovan ?
            ServerDatabase.direktorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);

            if (ServerDatabase.Clanovi.ContainsKey(clan.Jmbg))
            {
                ServerDatabase.Clanovi[clan.Jmbg] = clan; //add or update
                Console.WriteLine($"Clan sa jmbg-om: {clan.Jmbg} azuriran");
            }
            else
            {
                DbException e = new DbException($"Clan sa jmbg-om {clan.Jmbg}  ne postoji.");
                throw new FaultException<DbException>(e);
            }
        }

        public void PosaljiBazu(Dictionary<string, Clan> baza, string token)
        {
            ServerDatabase.direktorijumKorisnika.KorisnikAutentifikovan(token); //da li je autentifikovan ?
            ServerDatabase.direktorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Repliciranje);

            ServerDatabase.Clanovi = baza;  //upisao sam novu bazu

            //foreach(var clan in baza)
            //{
            //    if(clan.vreme < DateTime.Now)
            //    ServerDatabase.Clanovi[clan.Jmbg] = clan;
            //}

            Console.WriteLine("Baza je azurirana.");
           
        }

        public Dictionary<string, Clan> PreuzmiBazu(string token)
        {
            ServerDatabase.direktorijumKorisnika.KorisnikAutentifikovan(token); //da li je autentifikovan ?
            ServerDatabase.direktorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Repliciranje);

            Console.WriteLine("Saljem celu bazu");
            return ServerDatabase.Clanovi;
        }
    }
}
