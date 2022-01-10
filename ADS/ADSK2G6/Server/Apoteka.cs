using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Apoteka : IApoteka
    {
        public void DodajLek(Lek l, string token)
        {
            ServerDatabase.DirektorijumKorniska.KorisnikAutentifikovan(token);
            ServerDatabase.DirektorijumKorniska.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);
            if(ServerDatabase.Lekovi.ContainsKey(l.Id) == false)
            {
                ServerDatabase.Lekovi.Add(l.Id, l);
                //ServerDatabase.Lekovi[l.Id] = l;
                Console.WriteLine("Dodao sam: " + l);
            }
            else
            {
                DatabaseException e = new DatabaseException("Lek vec postoji");
                throw new FaultException<DatabaseException>(e);
            }
        }

        public void ObrisiLek(int id, string token)
        {
            ServerDatabase.DirektorijumKorniska.KorisnikAutentifikovan(token);
            ServerDatabase.DirektorijumKorniska.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);

            if (ServerDatabase.Lekovi.ContainsKey(id))
            {
                ServerDatabase.Lekovi.Remove(id);
            }
            else
            {
                DatabaseException e = new DatabaseException("Lek ne postoji");
                throw new FaultException<DatabaseException>(e);
            }
            Console.WriteLine("Obrisao sam lek sa id-ijem " + id);
        }

        public void PosaljiBazu(Dictionary<int, Lek> baza, string token)
        {
            ServerDatabase.DirektorijumKorniska.KorisnikAutentifikovan(token);
            ServerDatabase.DirektorijumKorniska.KorisnikAutorizovan(token, EPravaPristupa.Replikacija);

            ServerDatabase.Lekovi = baza;
        }

        public Dictionary<int, Lek> PreuzmiBazu(string token)
        {
            ServerDatabase.DirektorijumKorniska.KorisnikAutentifikovan(token);
            ServerDatabase.DirektorijumKorniska.KorisnikAutorizovan(token, EPravaPristupa.Replikacija);


            return ServerDatabase.Lekovi;
        }

        public int VratiBrojLekovaProizvodjaca(string proizvodjac, string token)
        {
            ServerDatabase.DirektorijumKorniska.KorisnikAutentifikovan(token);
            ServerDatabase.DirektorijumKorniska.KorisnikAutorizovan(token, EPravaPristupa.Citanje);

            int cnt = 0;

            foreach(Lek l in ServerDatabase.Lekovi.Values)
            {
                if (l.Proizvodjac == proizvodjac)
                    cnt++;
            }

            return cnt;
        }

        public Lek VratiNajskupljiOdProizvodjaca(string proizvodjac, string token)
        {
            ServerDatabase.DirektorijumKorniska.KorisnikAutentifikovan(token);
            ServerDatabase.DirektorijumKorniska.KorisnikAutorizovan(token, EPravaPristupa.Citanje);

            double max = 0;
            Lek ret = null;
            foreach (Lek l in ServerDatabase.Lekovi.Values)
            {
                if (l.Proizvodjac == proizvodjac && l.Cena > max)
                {
                    max = l.Cena;
                    ret = l;
                }
            }

            return ret;
        }

        public List<Lek> VratiSkupljeOd(double cena, string token)
        {
            ServerDatabase.DirektorijumKorniska.KorisnikAutentifikovan(token);
            ServerDatabase.DirektorijumKorniska.KorisnikAutorizovan(token, EPravaPristupa.Citanje);

            List<Lek> ret = new List<Lek>();
            foreach(Lek l in ServerDatabase.Lekovi.Values)
            {
                if(l.Cena > cena)
                    ret.Add(l);
            }

            return ret;
        }

        public List<Lek> VratiSortirane(string token)
        {
            ServerDatabase.DirektorijumKorniska.KorisnikAutentifikovan(token);
            ServerDatabase.DirektorijumKorniska.KorisnikAutorizovan(token, EPravaPristupa.Citanje);

            return ServerDatabase.Lekovi.Values.OrderBy(x => x.Cena).ToList();
        }
    }
}
