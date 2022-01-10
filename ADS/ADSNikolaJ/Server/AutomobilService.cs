using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class AutomobilService : IAutomobilService
    {
        public void DodajAutomobil(Automobil a, string token)
        {
            ServerDatabase.Direktorijum.KorisnikAutentifikovan(token);
            ServerDatabase.Direktorijum.KorisnikAutorizovan(token, EPravaPristupa.Write);

            if(ServerDatabase.Automobili.ContainsKey(a.BrojSasije))
            {
                throw new FaultException<MyException>(new MyException("Automobil sa datim brojem sasije vec postoji u bazi."));
            }

            ServerDatabase.Automobili.Add(a.BrojSasije, a);
        }

        public List<Automobil> IzlistajAutomobileNekeMarke(string marka, string token)
        {
            ServerDatabase.Direktorijum.KorisnikAutentifikovan(token);
            ServerDatabase.Direktorijum.KorisnikAutorizovan(token, EPravaPristupa.Read);

            return ServerDatabase.Automobili.Values.Where(x => x.Marka == marka).ToList();
        }

        public string IzlistajSveAutomobile(string token)
        {
            ServerDatabase.Direktorijum.KorisnikAutentifikovan(token);
            ServerDatabase.Direktorijum.KorisnikAutorizovan(token, EPravaPristupa.Read);

            string str = "";
            str += "--------------------------------------------------------\n";
            
            foreach(Automobil a in ServerDatabase.Automobili.Values)
            {
                str += a.ToString() + "\n";
            }

            str += "--------------------------------------------------------\n";

            return str;
        }

        public void ObrisiAutomobil(string brojSasije, string token)
        {
            ServerDatabase.Direktorijum.KorisnikAutentifikovan(token);
            ServerDatabase.Direktorijum.KorisnikAutorizovan(token, EPravaPristupa.Write);

            if (!ServerDatabase.Automobili.ContainsKey(brojSasije))
            {
                throw new FaultException<MyException>(new MyException("Automobil sa datim brojem sasije ne postoji."));
            }
            ServerDatabase.Automobili.Remove(brojSasije);
        }

        public List<Automobil> SortirajAutomobile(string token)
        {
            ServerDatabase.Direktorijum.KorisnikAutentifikovan(token);
            ServerDatabase.Direktorijum.KorisnikAutorizovan(token, EPravaPristupa.Read);


            return ServerDatabase.Automobili.Values.OrderBy(x => x.Cena).ToList();
        }
    }
}
