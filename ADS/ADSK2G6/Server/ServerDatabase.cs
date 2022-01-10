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
        public static Dictionary<int, Lek> Lekovi = new Dictionary<int, Lek>();
        public static DirektorijumKorisnika DirektorijumKorniska = new DirektorijumKorisnika();
    }
}
