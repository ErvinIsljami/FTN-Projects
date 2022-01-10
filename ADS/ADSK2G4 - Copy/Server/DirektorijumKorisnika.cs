using Common;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

namespace Server
{
    public class DirektorijumKorisnika
    {
        private Dictionary<string, Korisnik> korisnici = new Dictionary<string, Korisnik>();
        private Dictionary<string, string> autentifikovani
        = new Dictionary<string, string>();
        private const string _pepper = "P&0myWHq";
        public DirektorijumKorisnika()
        {
            DodajKorisnika("pera", "pera");
            DodajKorisnika("admin", "admin");
        }
        private string KodiranTekst(string lozinka)
        {
            using (var sha = SHA256.Create())
            {
                var computedHash = sha.ComputeHash(Encoding.Unicode.GetBytes(lozinka + _pepper));
                return Convert.ToBase64String(computedHash);
            }
        }
        public void DodajKorisnika(string ime, string lozinka)
        {
            korisnici.Add(ime, new Korisnik(ime, this.KodiranTekst(lozinka)));
        }
        public string AutentifikacijaKorisnika(string ime, string lozinka)
        {
            if (korisnici.ContainsKey(ime) && this.KodiranTekst(lozinka) == korisnici[ime].Lozinka)
            {
                korisnici[ime].Autentifikovan = true;
                string token = this.KodiranTekst(ime + _pepper);
                this.autentifikovani.Add(token, ime);
                Console.WriteLine($"Autentifikovan korisnik {ime} sa tokenom {token}.");
                return token;
            }
            else
            {
                throw new FaultException<SecurityException>( new SecurityException("Neispravno korisnicko ime i / ili lozinka."));

            }
        }
        public bool KorisnikAutentifikovan(string token)
        {
            if (autentifikovani.ContainsKey(token))
            {
                return true;
            }
            else
            {
                throw new FaultException<SecurityException>(
                new SecurityException("Korisnik nije autentifikovan"));
            }
        }
    }
}
