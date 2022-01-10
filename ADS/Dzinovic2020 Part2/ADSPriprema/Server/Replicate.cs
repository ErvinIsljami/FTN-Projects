using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Replicate : IReplicate
    {

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
