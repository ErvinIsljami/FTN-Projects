using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class BezbednosniMehanizmi : IBezbednosniMehanizmi
    {
        public string Autentifikacija(string korisnik, string lozinka)
        {
            return ServerDatabase.DirektorijumKorniska.AutentifikacijaKorisnika(korisnik, lozinka);
        }
    }
}
