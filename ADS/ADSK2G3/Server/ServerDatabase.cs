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
        public static Dictionary<int, Telefon> Telefoni = new Dictionary<int, Telefon>();
        public static DirektorijumKorisnika DirektorijumKorisnika = new DirektorijumKorisnika();
    }
}
