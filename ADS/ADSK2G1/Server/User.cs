using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class User
    {
        string korisnik;
        string lozinka;
        bool autentifikovan = false;
        string token;


        public string KIme { get => korisnik; set => korisnik = value; }
        public string Lozinka { get => lozinka; set => lozinka = value; }
        public bool Autentifikovan { get => autentifikovan; set => autentifikovan = value; }
        public string Token { get => token; set => token = value; }


        public User(string ime, string lozinka)
        {
            this.KIme = ime;
            this.Lozinka = lozinka;
        }
    }
}
