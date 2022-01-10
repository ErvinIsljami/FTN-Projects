using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
namespace Server
{
    public class BezbednosniMehanizmi : IBezbednosniMehanizmi
    {
        public string Autentifikacija(string username, string lozinka)
        {
            return ServerDatabase.DirektorijumKorisnika.AutentifikacijaKorisnika(username, lozinka);
        }
    }
}
