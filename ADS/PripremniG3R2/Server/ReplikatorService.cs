using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ReplikatorService : IReplikatorService
    {
        public void PosaljiBazu(Dictionary<string, Clan> baza)
        {
            
            ServerDatabase.Clanovi = baza;
            Console.WriteLine("Updateovana cela baza");
        }

        public Dictionary<string, Clan> PreuzmiBazu()
        {
            
            Console.WriteLine("Vracam celu bazu");
            return ServerDatabase.Clanovi;
        }
    }
}
