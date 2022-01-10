using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Common
{
    [DataContract]
    public class Lek
    {
        string naziv;
        string proizvodjac;
        double cena;
        int id;
        DateTime datumKreiranja;
        static int brojac = 0;

        [DataMember]
        public string Naziv { get => naziv; set => naziv = value; }
        [DataMember]
        public string Proizvodjac { get => proizvodjac; set => proizvodjac = value; }
        [DataMember]
        public double Cena { get => cena; set => cena = value; }
        [DataMember]
        public int Id { get => id; set => id = value; }
        [DataMember]
        public static int Brojac { get => brojac; set => brojac = value; }
        [DataMember]
        public DateTime DatumKreiranja { get => datumKreiranja; set => datumKreiranja = value; }


        public Lek(string naziv, string proizvodjac, double cena)
        {
            this.Naziv = naziv;
            this.Proizvodjac = proizvodjac;
            this.Cena = cena;
            this.Id = Brojac++;
            datumKreiranja = DateTime.Now;
        }

        public Lek()
        {
            Id = Brojac++;
            datumKreiranja = DateTime.Now;
        }

        public override string ToString()
        {
            return $"Lek {naziv} prizvodjaca {proizvodjac} kosta {cena} a ima id {id}.";
        }
    }
}
