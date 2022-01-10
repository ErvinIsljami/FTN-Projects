using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESProjekat.Model
{
    public class Transformator
    {
        string kod;
        ETipJedinice tip;
        double trenutnaAktivnaSnaga;
        double min;
        double max;
        ETipKontrole tipKontrole;
        double cena;

        public Transformator()
        {
        }

        public Transformator(string kod, ETipJedinice tip, double trenutnaAktivnaSnaga, double min, double max, ETipKontrole tipKontrole, double cena)
        {
            this.kod = kod;
            this.tip = tip;
            this.trenutnaAktivnaSnaga = trenutnaAktivnaSnaga;
            this.min = min;
            this.max = max;
            this.tipKontrole = tipKontrole;
            this.cena = cena;
        }

        public string Kod { get => kod; set => kod = value; }
        public double TrenutnaAktivnaSnaga { get => trenutnaAktivnaSnaga; }
        public double Min { get => min; set => min = value; }
        public double Max { get => max; set => max = value; }
        public double Cena { get => cena; set => cena = value; }
        public ETipJedinice Tip { get => tip; set => tip = value; }
        public ETipKontrole TipKontrole { get => tipKontrole; set => tipKontrole = value; }

        public bool AzurirajAktivnuSnagu()
        {
            double novaVrednost = 0;
            Random r = new Random();
            if(tipKontrole == ETipKontrole.LOCAL)
            {
                do
                {
                    novaVrednost = r.Next((int)min, (int)max);

                } while (novaVrednost < trenutnaAktivnaSnaga * 0.9 || novaVrednost > trenutnaAktivnaSnaga * 1.1);
                trenutnaAktivnaSnaga = novaVrednost;
            }
            else
            {
                //uzmi vrednost od modula 2
            }

            return true;
        }
    }
}
