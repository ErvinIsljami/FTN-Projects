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
                if(Properties.Settings.Default.StanjeServera == EStanjeServera.Primarni)
                {
                    //ChannelFactory<IBiblioteka> factory = new ChannelFactory<IBiblioteka>("ServisLica");
                    //IBiblioteka proxy = factory.CreateChannel();

                    //proxy.DodajClana(clan, token);


                    ChannelFactory<IReplikatorService> factory = new ChannelFactory<IReplikatorService>("Replikacija");
                    IReplikatorService proxy = factory.CreateChannel();
                    proxy.PosaljiBazu(ServerDatabase.Clanovi);
                    Console.WriteLine("replicirao");
                }
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

        
    }
}
