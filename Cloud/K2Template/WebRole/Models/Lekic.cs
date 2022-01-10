using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRole.Models
{
    public class Lekic
    {
        public string Naziv { get; set; }
        public int Cena { get; set; }
        public string SifraLeka { get; set; }

        public Lekic()
        {
        }

        public Lekic(string naziv, int cena, string sifraLeka)
        {
            Naziv = naziv;
            Cena = cena;
            SifraLeka = sifraLeka;
        }
    }
}