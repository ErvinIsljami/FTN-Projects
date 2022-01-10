using Common;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Linq;

namespace Server
{
    public class Banka : IBanka
    {
        public void DodajRacun(Racun r, string token)
        {
            ServerDatabase.direktorijumKorisnika.KorisnikAutentifikovan(token);
            ServerDatabase.direktorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);

            if (ServerDatabase.Racuni.ContainsKey(r.Id) == false)
            {
                ServerDatabase.Racuni.Add(r.Id, r);
                //ServerDatabase.Racuni[r.Id] = r;
                Console.WriteLine("Dodao sam novi racun: " + r);
            }
            else
            {
                DatabaseException e = new DatabaseException("Racun vec postoji u bazi.");
                throw new FaultException<DatabaseException>(e);
            }
        }

        public Racun NajbogatijiDinarski(string token)
        {
            ServerDatabase.direktorijumKorisnika.KorisnikAutentifikovan(token);
            ServerDatabase.direktorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Citanje);

            Racun ret = null;
            double max = 0;

            foreach (Racun r in ServerDatabase.Racuni.Values)
            {
                if (r.IsDevizni == false)
                {
                    if(r.StanjeRacuna > max)
                    {
                        max = r.StanjeRacuna;
                        ret = r;
                    }
                }

            }


            return ret;
        }

        public void ObrisiRacun(int id, string token)
        {
            ServerDatabase.direktorijumKorisnika.KorisnikAutentifikovan(token);
            ServerDatabase.direktorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);

            if (ServerDatabase.Racuni.ContainsKey(id))
            {
                ServerDatabase.Racuni.Remove(id);
            }
            else
            {
                DatabaseException e = new DatabaseException("Racun ne postoji u bazi.");
                throw new FaultException<DatabaseException>(e);
            }
        }

        public void UpisiSve(List<Racun> lista)
        {
            foreach(Racun r in lista)
            {
                ServerDatabase.Racuni[r.Id] = r;
            }
        }

        public List<Racun> UzmiSve()
        {
            return ServerDatabase.Racuni.Values.ToList();
        }

        public List<Racun> VratiSveDevizne(string token)
        {
            ServerDatabase.direktorijumKorisnika.KorisnikAutentifikovan(token);
            ServerDatabase.direktorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Citanje);

            List<Racun> ret = new List<Racun>();
            foreach (var r in ServerDatabase.Racuni.Values)
            {
                if (r.IsDevizni == true)
                    ret.Add(r);

            }

            ServerDatabase.Racuni.Values.OrderBy(x => x.StanjeRacuna).ToList();



            return ret.OrderBy(x => x.StanjeRacuna).ToList();
        }
    }
}
