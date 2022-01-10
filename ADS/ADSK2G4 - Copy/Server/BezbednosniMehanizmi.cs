using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class BezbednosniMehanizmi : IBezbednosniMehanizmi
    {
        public string Autentifikacija(string username, string lozinka)
        {
            return ServerDatabase.direktorijumKorisnika.AutentifikacijaKorisnika(username, lozinka);
        }
    }
}
