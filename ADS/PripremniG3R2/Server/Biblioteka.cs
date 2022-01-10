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

            if(ServerDatabase.Clanovi.ContainsKey(clan.Jmbg) == false)
            {
                ServerDatabase.Clanovi.Add(clan.Jmbg, clan);
                Console.WriteLine("Dodat novi clan: " + clan);
            }
            else
            {
                DbException e = new DbException("Clan sa tim jmbg-om vec postoji");
                throw new FaultException<DbException>(e);
            }
        }

        public void DodajKnjigu(Knjiga k, string token)
        {
            ServerDatabase.direktorijumKorisnika.KorisnikAutentifikovan(token);
            ServerDatabase.direktorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);
            
            if(ServerDatabase.Knjige.ContainsKey(k.Id) == false)
            {
                ServerDatabase.Knjige.Add(k.Id, k);
                Console.WriteLine("Dodali smo knjigu: " + k);
            }
            else
            {
                DbException e = new DbException("Clan sa tim jmbg-om vec postoji");
                throw new FaultException<DbException>(e);
            }
        }

        public void DodajKnjiguClanu(string jmbg, Knjiga k, string token)
        {
            ServerDatabase.direktorijumKorisnika.KorisnikAutentifikovan(token);
            ServerDatabase.direktorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);
            
            if(ServerDatabase.Clanovi.ContainsKey(jmbg))
            {
                if(ServerDatabase.Knjige.ContainsKey(k.Id))
                {
                    if(ServerDatabase.Clanovi[jmbg].ListaKnjiga.Contains(k) == false)
                    {
                        ServerDatabase.Clanovi[jmbg].ListaKnjiga.Add(k);
                    }
                    else
                    {
                        DbException e = new DbException("Clan vec ima datu knjigu");
                        throw new FaultException<DbException>(e);
                    }
                }
                else
                {
                    DbException e = new DbException("Knjiga ne postoji");
                    throw new FaultException<DbException>(e);
                }
            }
            else
            {
                DbException e = new DbException("Clan ne postoji");
                throw new FaultException<DbException>(e);
            }

        }

        public void DodajKnjiguClanuPoImenu(string ime, string prezime, Knjiga k, string token)
        {
            ServerDatabase.direktorijumKorisnika.KorisnikAutentifikovan(token);
            ServerDatabase.direktorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);

            Clan pom = null;
            foreach(Clan c in ServerDatabase.Clanovi.Values)
            {
                if (c.Ime == ime && c.Prezime == prezime)
                {
                    pom = c;
                    break;
                }
            }

            if (pom != null)
            {
                if (ServerDatabase.Knjige.ContainsKey(k.Id))
                {
                    if (pom.ListaKnjiga.Contains(k) == false)
                    {
                        pom.ListaKnjiga.Add(k);
                    }
                    else
                    {
                        DbException e = new DbException("Clan vec ima datu knjigu");
                        throw new FaultException<DbException>(e);
                    }
                }
                else
                {
                    DbException e = new DbException("Knjiga ne postoji");
                    throw new FaultException<DbException>(e);
                }
            }
            else
            {
                DbException e = new DbException("Clan ne postoji");
                throw new FaultException<DbException>(e);
            }
        }

        public void IzbrisiClana(string jmbg, string token)
        {
            ServerDatabase.direktorijumKorisnika.KorisnikAutentifikovan(token);
            ServerDatabase.direktorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);

            if (ServerDatabase.Clanovi.ContainsKey(jmbg))
            {
                ServerDatabase.Clanovi.Remove(jmbg);
                Console.WriteLine("Izbrisan clan sa jmbg-om " + jmbg);
            }
            else
            {
                DbException e = new DbException("Clan ne postoji");
                throw new FaultException<DbException>(e);
            }
        }

        public void IzmeniClana(Clan clan, string token)
        {
            ServerDatabase.direktorijumKorisnika.KorisnikAutentifikovan(token);
            ServerDatabase.direktorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);

            if (ServerDatabase.Clanovi.ContainsKey(clan.Jmbg))
            {
                ServerDatabase.Clanovi[clan.Jmbg] = clan;   //update
                Console.WriteLine("Updateovan clan " + clan.Ime);
            }
            else
            {
                DbException e = new DbException("Clan ne postoji");
                throw new FaultException<DbException>(e);
            }
        }

        
        public List<Clan> VratiSortiraneClanovePoUkupnojCeniKnjiga(string token)
        {
            ServerDatabase.direktorijumKorisnika.KorisnikAutentifikovan(token);
            ServerDatabase.direktorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Citanje);

            Clan[] nizClanova = ServerDatabase.Clanovi.Values.ToArray();  
            for(int i = 0; i < nizClanova.Count() - 1; i++)
            {
                for(int j = i; j < nizClanova.Count(); j++)
                {
                    double tempI = 0;
                    double tempJ = 0;

                    foreach(Knjiga k in nizClanova[i].ListaKnjiga)
                    {
                        tempI += k.Cena;
                    }
                    foreach (Knjiga k in nizClanova[j].ListaKnjiga)
                    {
                        tempJ += k.Cena;
                    }

                    if(tempI > tempJ)
                    {
                        Clan pom = nizClanova[i];
                        nizClanova[i] = nizClanova[j];
                        nizClanova[j] = pom;
                    }

                }
            }

            return ServerDatabase.Clanovi.Values.OrderBy(x => x.ListaKnjiga.Sum(a => a.Cena)).ToList();
        }
    }
}
