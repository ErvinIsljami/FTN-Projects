using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public static class ServerDatabase
    {
        public static Dictionary<int, Automobil> Automobili = new Dictionary<int, Automobil>();
        public static DirektorijumKorisnika direktorijum = new DirektorijumKorisnika();
    }
}
