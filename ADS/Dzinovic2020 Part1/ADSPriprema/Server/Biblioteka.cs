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
        public bool DobaviClana(string token, long jmbg, out Clan clan)
        {
            ServerDatabase.Direktorijum.KorisnikAutentifikovan(token);
            ServerDatabase.Direktorijum.KorisnikAutorizovan(token, EPravaPristupa.Citanje);

            if(ServerDatabase.Clanovi.ContainsKey(jmbg))
            {
                clan = ServerDatabase.Clanovi[jmbg];
                return true;
            }

            clan = null;
            return false;
        }

        public void DodajClana(string token, Clan clan)
        {
            ServerDatabase.Direktorijum.KorisnikAutentifikovan(token);
            ServerDatabase.Direktorijum.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);

            if (ServerDatabase.Clanovi.ContainsKey(clan.JMBG))
            {
                throw new FaultException<MyException>(new MyException("Clan sa datim jmbgom vec postoji"));
            }
            ServerDatabase.Clanovi.Add(clan.JMBG, clan);
        }

        public bool DodajKnjiguClanu(string token, long jmbg, params string[] knjige)
        {
            ServerDatabase.Direktorijum.KorisnikAutentifikovan(token);
            ServerDatabase.Direktorijum.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);

            if (!ServerDatabase.Clanovi.ContainsKey(jmbg))
            {
                return false;
            }
            Clan clan = ServerDatabase.Clanovi[jmbg];

            clan.Knjige.AddRange(knjige);

            return true;
        }

        public void IzbrisiClana(string token, long jmbg)
        {
            ServerDatabase.Direktorijum.KorisnikAutentifikovan(token);
            ServerDatabase.Direktorijum.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);

            if (!ServerDatabase.Clanovi.ContainsKey(jmbg))
            {
                throw new FaultException<MyException>(new MyException("Clan sa datim jmbgom ne postoji"));
            }

            ServerDatabase.Clanovi.Remove(jmbg);
        }

        public void IzmeniClana(string token, Clan clan)
        {
            ServerDatabase.Direktorijum.KorisnikAutentifikovan(token);
            ServerDatabase.Direktorijum.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);


            if (!ServerDatabase.Clanovi.ContainsKey(clan.JMBG))
            {
                throw new FaultException<MyException>(new MyException("Clan sa datim jmbgom ne postoji"));
            }

            ServerDatabase.Clanovi[clan.JMBG] = clan;
        }

        public bool ObrisiKnjiguClanu(string token, long jmbg, params string[] knjige)
        {
            ServerDatabase.Direktorijum.KorisnikAutentifikovan(token);
            ServerDatabase.Direktorijum.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);

            if (ServerDatabase.Clanovi.ContainsKey(jmbg))
            {
                return false;
            }

            Clan clan = ServerDatabase.Clanovi[jmbg];
            foreach (var k in knjige)
            {
                clan.Knjige.Remove(k);
            }
            return false;
        }

        public void PosaljiBazu(string token, Dictionary<long, Clan> baza)
        {
            ServerDatabase.Direktorijum.KorisnikAutentifikovan(token);
            ServerDatabase.Direktorijum.KorisnikAutorizovan(token, EPravaPristupa.Replikacija);

            ServerDatabase.Clanovi = baza;
        }

        public Dictionary<long, Clan> PreuzmiBazu(string token)
        {
            ServerDatabase.Direktorijum.KorisnikAutentifikovan(token);
            ServerDatabase.Direktorijum.KorisnikAutorizovan(token, EPravaPristupa.Replikacija);

            return ServerDatabase.Clanovi;
        }
    }
}
