using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class Knjiga
    {
        string naziv;
        string pisac;
        double cena;
        int id;
        static int brojac;

        [DataMember]
        public string Naziv { get => naziv; set => naziv = value; }
        [DataMember]
        public string Pisac { get => pisac; set => pisac = value; }
        [DataMember]
        public double Cena { get => cena; set => cena = value; }
        [DataMember]
        public int Id { get => id; set => id = value; }
        [DataMember]
        public static int Brojac { get => brojac; set => brojac = value; }

        public Knjiga()
        {
        }

        public Knjiga(string naziv, string pisac, double cena)
        {
            this.Naziv = naziv;
            this.Pisac = pisac;
            this.Cena = cena;
            Id = Brojac++;
        }

        public override string ToString()
        {
            return $"Knjiga {naziv} napisao {pisac} kosta {cena}";
        }
    }
}