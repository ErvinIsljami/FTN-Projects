using Common;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Linq;

namespace Server
{
    public class ProdavnicaTelefona : IProdavnicaTelefona
    {
        public void DodajTelefon(Telefon t, string token)
        {
            ServerDatabase.DirektorijumKorisnika.KorisnikAutentifikovan(token);
            ServerDatabase.DirektorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);

            if (ServerDatabase.Telefoni.ContainsKey(t.Id) == false)
            {
                ServerDatabase.Telefoni.Add(t.Id, t);
            }
            else
            {
                DatabaseException e = new DatabaseException("Telefon vec postoji");
                throw new FaultException<DatabaseException>(e);
                //throw new FaultException<DatabaseException>(new DatabaseException("Telefon vec postoji u bazi"));

            }
        }

        public Telefon NadjiNajskupljiIzMarke(string marka, string token)
        {
            ServerDatabase.DirektorijumKorisnika.KorisnikAutentifikovan(token);
            ServerDatabase.DirektorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Citanje);

            Telefon ret = null;
            int max = 0;
            foreach(Telefon t in ServerDatabase.Telefoni.Values)
            {
                if(t.Marka == marka && t.Cena > max)
                {
                    max = t.Cena;
                    ret = t;
                }
            }

            if(ret == null)
            {
                DatabaseException e = new DatabaseException("Ne postoji telefon sa zadatom markom.");
                throw new FaultException<DatabaseException>(e);
            }
            return ret;
        }

        public void ObrisiTelefon(int id, string token)
        {
            ServerDatabase.DirektorijumKorisnika.KorisnikAutentifikovan(token);
            ServerDatabase.DirektorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);


            if (ServerDatabase.Telefoni.ContainsKey(id) == true)
            {
                ServerDatabase.Telefoni.Remove(id);
            }
            else
            {
                DatabaseException e = new DatabaseException("Telefon ne postoji");
                throw new FaultException<DatabaseException>(e);

            }
        }

        public List<Telefon> VratiSortirane(string token)
        {
            ServerDatabase.DirektorijumKorisnika.KorisnikAutentifikovan(token);
            ServerDatabase.DirektorijumKorisnika.KorisnikAutorizovan(token, EPravaPristupa.Citanje);

            return ServerDatabase.Telefoni.Values.OrderBy(x => x.Cena).ToList();
        }
    }
}
