using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Korisnik
    {
        string username;
        string lozinka;
        bool autentifikovan = false;
        string token;

        public string Username { get => username; set => username = value; }
        public string Lozinka { get => lozinka; set => lozinka = value; }
        public bool Autentifikovan { get => autentifikovan; set => autentifikovan = value; }
        public string Token { get => token; set => token = value; }

        public Korisnik(string ime, string lozinka)
        {
            this.Username = ime;
            this.Lozinka = lozinka;
        }
    }
}
