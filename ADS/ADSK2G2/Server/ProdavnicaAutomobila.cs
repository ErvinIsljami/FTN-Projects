using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ProdavnicaAutomobila : IProdavnicaAutomobila
    {
        public void DodajAuto(Automobil auto, string token)
        {
            ServerDatabase.direktorijum.KorisnikAutentifikovan(token);
            ServerDatabase.direktorijum.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);

            if(!ServerDatabase.Automobili.ContainsKey(auto.Id))
            {
                ServerDatabase.Automobili.Add(auto.Id, auto);
            }
            else
            {
                DatabaseException e = new DatabaseException("Automobil vec postoji u bazi");
                throw new FaultException<DatabaseException>(e);
            }
        }

        public void ObrisiAuto(int id, string token)
        {
            ServerDatabase.direktorijum.KorisnikAutentifikovan(token);
            ServerDatabase.direktorijum.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);

            if (ServerDatabase.Automobili.ContainsKey(id))
            {
                ServerDatabase.Automobili.Remove(id);
            }
            else
            {
                DatabaseException e = new DatabaseException("Automobil ne postoji u bazi");
                throw new FaultException<DatabaseException>(e);
            }
        }

        public void UpisiSve(List<Automobil> automobili)
        {
            //da sam imao autentifikaciju i autorizaciju morao bih da prosledjujem token i ovo da radim:
            //ServerDatabase.direktorijum.KorisnikAutentifikovan(token);
            //ServerDatabase.direktorijum.KorisnikAutorizovan(token, EPravaPristupa.Azuriranje);

            //da je automobili bio dictionary mogli smo samo:
            //ServerDatabase.Automobili = automobili;
            ServerDatabase.Automobili.Clear();
            foreach (var a in automobili)
            {
                ServerDatabase.Automobili.Add(a.Id, a);
            }
            Console.WriteLine("Server upisao sve podatke");
        }

        public List<Automobil> UzmiSve()
        {
            Console.WriteLine("Server prosledio sve podatke");
            return ServerDatabase.Automobili.Values.ToList();
        }

        public Automobil VratiNajskupljiIzMarke(string marka, string token)
        {
            ServerDatabase.direktorijum.KorisnikAutentifikovan(token);
            ServerDatabase.direktorijum.KorisnikAutorizovan(token, EPravaPristupa.Citanje);

            Automobil ret = null;
            int max = 0;
            foreach(Automobil a in ServerDatabase.Automobili.Values)
            {
                if(a.Marka == marka && a.Cena > max)
                {
                    max = a.Cena;
                    ret = a;
                }
            }

            if(ret == null) //znaci da nema te marke u bazi
            {
                DatabaseException e = new DatabaseException("Za zadatu marku ne postoji nijedan automobil");
                throw new FaultException<DatabaseException>(e);
            }

            return ret;
        }

        public List<Automobil> VratiSveAutomobile(string token)
        {
            ServerDatabase.direktorijum.KorisnikAutentifikovan(token);
            ServerDatabase.direktorijum.KorisnikAutorizovan(token, EPravaPristupa.Citanje);

            //ServerDatabase.Automobili.Values.ToList().ForEach(x => { Console.WriteLine(x); });

            return ServerDatabase.Automobili.Values.ToList().OrderBy(x => x.Cena) as List<Automobil>;
        }
    }
}
