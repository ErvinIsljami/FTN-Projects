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
        public static Dictionary<string, Automobil> Automobili { get; private set; }
        public static DirektorijumKorisnika Direktorijum { get; set; }
        static ServerDatabase()
        {
            Automobili = new Dictionary<string, Automobil>();
            Direktorijum = new DirektorijumKorisnika();
        }
    }
}
