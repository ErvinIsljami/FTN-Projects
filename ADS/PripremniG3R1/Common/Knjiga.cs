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
        string autor;
        double cena;
        int id;
        static int brojac = 0;

        public Knjiga(string naziv, string autor, double cena)
        {
            this.naziv = naziv;
            this.autor = autor;
            this.cena = cena;
            Id = Brojac++;
        }

        public Knjiga()
        {
            id = brojac++;
        }

        [DataMember]
        public string Naziv { get => naziv; set => naziv = value; }
        [DataMember]
        public string Autor { get => autor; set => autor = value; }
        [DataMember]
        public double Cena { get => cena; set => cena = value; }
        [DataMember]
        public int Id { get => id; set => id = value; }
        [DataMember]
        public static int Brojac { get => brojac; set => brojac = value; }



        public override string ToString()
        {
            return $"Knjiga {naziv} autora {Autor} kosta {cena} i ima id {Id}";
        }
    }
}
