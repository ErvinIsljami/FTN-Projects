using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ServerDatabase
    {
        public static Dictionary<long, Clan> Clanovi { get; set; }
        public static DirektorijumKorisnika Direktorijum { get; set; }
        static ServerDatabase()
        {
            Clanovi = new Dictionary<long, Clan>();
            Direktorijum = new DirektorijumKorisnika();
        }
    }
}
