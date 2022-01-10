using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class SecurityService : ISecurityService
    {
        public string AuthenticateUser(string username, string password)
        {
            return ServerDatabase.direktorijum.AutentifikacijaKorisnika(username, password);
        }
    }
}
