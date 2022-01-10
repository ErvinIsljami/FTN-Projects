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
        private Dictionary<string, User> korisnici = new Dictionary<string, User>();
        private Dictionary<string, string> autentifikovani = new Dictionary<string, string>();

        private Dictionary<string, SortedSet<EPravaPristupa>> prava = new Dictionary<string, SortedSet<EPravaPristupa>>(); //autorizacija

        private const string _pepper = "P&0myWHq";

        public DirektorijumKorisnika()
        {
            DodajKorisnika("pera", "P3rA");
            DodajKorisnika("admin", "admin");

            /*******************AUTORIZACIJA****************************/
            SortedSet<EPravaPristupa> citanje = new SortedSet<EPravaPristupa>();
            citanje.Add(EPravaPristupa.Citanje);
            prava.Add("pera", citanje);
            
            SortedSet<EPravaPristupa> azuriranje = new SortedSet<EPravaPristupa>();
            azuriranje.Add(EPravaPristupa.Azuriranje);
            azuriranje.Add(EPravaPristupa.Citanje);
            prava.Add("admin", azuriranje);
            /********************************************************************/
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
            korisnici.Add(ime, new User(ime, this.KodiranTekst(lozinka)));
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
                SecurityException ex = new SecurityException();
                ex.Reason = "Neispravno korisnicko ime i / ili lozinka.";
                throw new FaultException<SecurityException>(ex);
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
                SecurityException ex = new SecurityException();
                ex.Reason = "Nije autentifikovan korisnik.";
                throw new FaultException<SecurityException>(ex);
            }
        }

        /*********************AUTORIZACIJA**************************/
        public bool KorisnikAutorizovan(string token, EPravaPristupa pravo)
        {
            if (autentifikovani.ContainsKey(token)
            && prava.ContainsKey(autentifikovani[token])
            && prava[autentifikovani[token]].Contains(pravo))
                return true;
            else
                throw new FaultException<SecurityException>(
                new SecurityException("Korisnik nema pravo: " + pravo.ToString()));
        }
        /**********************************************************/
    }
}

