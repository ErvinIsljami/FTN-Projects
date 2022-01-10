using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
namespace Common
{
    [DataContract]
    public class Knjiga
    {
        private string naziv;
        private string autor;
        private double cena;
        private string isbn;
        private int id;
        static int brojac = 0;
        public Knjiga()
        {
            id = brojac++;
        }

        public Knjiga(string naziv, string autor, double cena, string isbn)
        {
            this.Naziv = naziv;
            this.Autor = autor;
            this.Cena = cena;
            this.Isbn = isbn;
            id = brojac++;

        }

        [DataMember]
        public string Naziv { get => naziv; set => naziv = value; }
        [DataMember]
        public string Autor { get => autor; set => autor = value; }
        [DataMember]
        public double Cena { get => cena; set => cena = value; }
        [DataMember]
        public string Isbn { get => isbn; set => isbn = value; }
        [DataMember]
        public int Id { get => id; set => id = value; }

        public override string ToString()
        {
            return $"Knjiga {naziv} autora {autor} kosta {cena}";
        }
    }
}
