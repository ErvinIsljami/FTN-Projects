using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Bezbednost : IBezbednosniMehanizmi
    {
        public string Autentifikacija(string korisnik, string lozinka)
        {
            return ServerDatabase.direktorijumKorisnika.AutentifikacijaKorisnika(korisnik, lozinka);    //vracam token korisniku
        }
    }
}
