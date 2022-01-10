using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using Common;

namespace Server
{
    public class DirektorijumKorisnika
    {
        private Dictionary<string, Korisnik> korisnici = new Dictionary<string, Korisnik>();
        private Dictionary<string, string> autentifikovani = new Dictionary<string, string>();
        private Dictionary<string, SortedSet<EPravaPristupa>> prava = new Dictionary<string, SortedSet<EPravaPristupa>>();

        private const string _pepper = "P&0myWHq";
        public DirektorijumKorisnika()
        {
            DodajKorisnika("pera", "pera");
            DodajKorisnika("admin", "admin");
            DodajKorisnika("replikator", "replikator");


            SortedSet<EPravaPristupa> citanje = new SortedSet<EPravaPristupa>();
            citanje.Add(EPravaPristupa.Citanje);
            prava.Add("pera", citanje);

            SortedSet<EPravaPristupa> azuriranje = new SortedSet<EPravaPristupa>();
            azuriranje.Add(EPravaPristupa.Azuriranje);
            azuriranje.Add(EPravaPristupa.Citanje);
            prava.Add("admin", azuriranje);

            SortedSet<EPravaPristupa> replikacija = new SortedSet<EPravaPristupa>();
            replikacija.Add(EPravaPristupa.Replikacija);
            prava.Add("replikator", replikacija);

        }
        private string KodiranTekst(string lozinka)
        {
            using (var sha = SHA256.Create())
            {
                var computedHash = sha.ComputeHash(
                Encoding.Unicode.GetBytes(lozinka + _pepper));
                return Convert.ToBase64String(computedHash);
            }
        }
        public void DodajKorisnika(string ime, string lozinka)
        {
            korisnici.Add(ime, new Korisnik(ime, this.KodiranTekst(lozinka)));
        }
        public string AutentifikacijaKorisnika(string ime, string lozinka)
        {
            if (korisnici.ContainsKey(ime)
            && this.KodiranTekst(lozinka) == korisnici[ime].Lozinka)
            {
                korisnici[ime].Autentifikovan = true;
                string token = this.KodiranTekst(ime + _pepper);
                this.autentifikovani.Add(token, ime);
                return token;
            }
            else
            {
                throw new FaultException<SecurityException>(
                new SecurityException("Neispravno korisnicko ime i / ili lozinka."));
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

        public bool KorisnikAutorizovan(string token, EPravaPristupa pravo)
        {
            if (autentifikovani.ContainsKey(token)
            && prava.ContainsKey(autentifikovani[token])
            && prava[autentifikovani[token]].Contains(pravo))
                return true;
            else
                throw new FaultException<SecurityException>( new SecurityException("Korisnik nema pravo: " + pravo.ToString()));
        }
    }
}
