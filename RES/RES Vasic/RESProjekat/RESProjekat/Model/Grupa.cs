using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESProjekat.Model
{
    public class Grupa
    {
        string kod;
        int brojJedinica;
        double trenutnaProizvodnja;
        double maxProizvodnja;

        public Grupa()
        {

        }

        public Grupa(string kod)
        {
            this.kod = kod;
            brojJedinica = 0;
            trenutnaProizvodnja = 0;
            maxProizvodnja = 0;
        }

        public string Kod { get => kod; set => kod = value; }
        public int BrojJedinica { get => brojJedinica; set => brojJedinica = value; }
        public double TrenutnaProizvodnja { get => trenutnaProizvodnja; set => trenutnaProizvodnja = value; }
        public double MaxProizvodnja { get => maxProizvodnja; set => maxProizvodnja = value; }
    }
}
