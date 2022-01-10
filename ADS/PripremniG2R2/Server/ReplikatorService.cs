using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Server
{
    public class ReplikatorService : IReplikatorService
    {
        public void PosaljiBazu(Dictionary<string, Clan> baza)
        {
            ServerDatabase.Clanovi = baza;  //upisao sam novu bazu
            Console.WriteLine("Baza je azurirana.");

        }

        public Dictionary<string, Clan> PreuzmiBazu()
        {
            Console.WriteLine("Saljem celu bazu");
            return ServerDatabase.Clanovi;
        }
    }
}
