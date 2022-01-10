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
        public static Dictionary<string, Clan> Clanovi = new Dictionary<string, Clan>();
        public static Dictionary<int, Knjiga> Knjige = new Dictionary<int, Knjiga>();
        public static DirektorijumKorisnika direktorijumKorisnika = new DirektorijumKorisnika();
    }
}
